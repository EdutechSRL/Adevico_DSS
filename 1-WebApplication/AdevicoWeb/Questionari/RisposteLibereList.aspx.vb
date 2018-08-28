Imports COL_Questionario

Partial Public Class RisposteLibereList
    Inherits PageBaseQuestionario

    Dim oRisposte As New list(Of RispostaDomanda)
    Dim oRispQ As New RispostaQuestionario
    Public Shared iDom As Integer
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneRisposte As New GestioneRisposte
    Dim oGestioneQuest As New GestioneQuestionario
#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property
    Private Property iPag() As Integer
        Get
            Return ViewState("idPag")
        End Get
        Set(ByVal value As Integer)
            ViewState("idPag") = value
        End Set
    End Property
    Private ReadOnly Property idDomanda() As Integer
        Get
            Return Request.QueryString("id")
        End Get

    End Property
    Private ReadOnly Property idDomandaOpzione() As Integer
        Get
            Return Request.QueryString("idO")
        End Get

    End Property
    Private Property domandaCorrente() As Domanda
        Get
            Return ViewState("domandaCorrente")
        End Get
        Set(ByVal value As Domanda)
            ViewState("domandaCorrente") = value
        End Set
    End Property

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.domandaCorrente.tipo = Me.domandaCorrente.TipoDomanda.TestoLibero Then
                GRVElenco.Columns(1).Visible = False
            Else
                If Me.domandaCorrente.tipo = Me.domandaCorrente.TipoDomanda.Numerica Then
                    GRVElenco.Columns(0).Visible = False
                    GRVElenco.Columns(2).Visible = False
                Else
                    GRVElenco.Columns(1).Visible = False
                End If
                BTNConferma.Visible = False
            End If
        Catch ex As Exception
            LBErrore.Visible = True
            MLVRisposte.SetActiveView(VIWMessaggi)
        End Try
    End Sub
    Public Overrides Sub BindDati()
        If Me.idDomanda > 0 Then

            Dim rispQuest As New RispostaQuestionario
            If Me.domandaCorrente Is Nothing Then
                Me.domandaCorrente = New Domanda
            End If

            Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, 0, True)

            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.RandomRepeat Then
                oGestioneQuest.readDomandeQuestionarioRandom()
            End If

            Me.domandaCorrente = Me.domandaCorrente.findDomandaBYID(Me.QuestionarioCorrente.domande, Me.idDomanda)

            LBTestoDomanda.Text = Me.domandaCorrente.testo

            Dim rispDomanda As New List(Of RispostaDomanda)
            rispDomanda = rispQuest.findRisposteLibere(Me.QuestionarioCorrente, Me.domandaCorrente, Me.idDomandaOpzione)
            If rispDomanda.Count > 0 Then
                MLVRisposte.SetActiveView(VIWDati)
                GRVElenco.DataSource = rispDomanda
                GRVElenco.DataBind()
            Else
                LBErrore.Text = Me.Resource.getValue("LBNoRisposte.text")
                MLVRisposte.SetActiveView(VIWMessaggi)
            End If
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        LBErrore.Text = Me.Resource.getValue("LBNoPermessi.text")
        MLVRisposte.SetActiveView(VIWMessaggi)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        If qs_ownerTypeId = OwnerType_enum.None Then
            If Not qs_questId < 1 Then
                'se il questionario con id in querystring non e' della comunita' corrente ritorna false
                Dim oQuest As New Questionario
                oQuest = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, qs_questId, LinguaID, UtenteCorrente.ID, 0)
                'se il quest e' della com corrente...
                If DALQuestionarioGruppo.ComunitaByGruppo(oQuest.idGruppo) = ComunitaCorrenteID Then
                    '..e ho i permessi necessari nella comunita'
                    If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting Then
                        If MyBase.Servizio.Compila Then
                            'metto il quest in sessione e vado avanti
                            Me.QuestionarioCorrente = oQuest
                            Return True
                        End If
                    Else
                        If (MyBase.Servizio.Admin OrElse MyBase.Servizio.GestioneDomande OrElse MyBase.Servizio.VisualizzaStatistiche) Then
                            'metto il quest in sessione e vado avanti
                            Me.QuestionarioCorrente = oQuest
                            Return True
                        End If
                    End If

                    'altrimenti nego i permessi
                    Return False
                Else
                    'altrimenti nego i permessi
                    Return False
                End If
            Else
                If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting Then
                    Return MyBase.Servizio.Compila
                Else
                    Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande Or MyBase.Servizio.VisualizzaStatistiche)
                End If
            End If
        Else
            If RootObject.RequestAuthorizationToOwner(CurrentContext, qs_ownerTypeId, Me.UtenteCorrente.ID, qs_questIdType, RootObject.EduPath_Permission.Evaluate, Me.PageUtility.CurrentRoleID, qs_ownerId, , qs_PersonaId) Then
                If qs_questId < 1 Then
                    Return False
                Else
                    Dim oQuest As New Questionario
                    oQuest = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, qs_questId, LinguaID, IIf(qs_PersonaId = 0, Me.UtenteCorrente.ID, qs_PersonaId), 0)
                    If oQuest.ownerType = qs_ownerTypeId AndAlso oQuest.ownerId = qs_ownerId Then
                        'owner in querystring deve corrispondere con quello che del questionario
                        Me.QuestionarioCorrente = oQuest
                        Return True
                    Else : Return False
                    End If
                End If
            End If
        End If

    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_RisposteLibereList", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        SetServiceTitle(Master)
        With Me.Resource
            .setLabel(LBTitolo)
            .setLabel(LBErrore)
            .setLabel(LBConferma)
            .setLinkButton(LNBListaRisposte, True, False, False, False)
            .setLinkButton(LNBStatistiche, True, False, False, False)

            .setHeaderGridView(Me.GRVElenco, 2, "headerPunteggio", True)
            .setHeaderGridView(Me.GRVElenco, 3, "headerVediQuestionario", True)

        End With
    End Sub
    Protected Sub LNBListaRisposte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBListaRisposte.Click
        Me.RedirectToUrl(RootObject.RisposteLibere + "?id=" + Me.idDomanda.ToString() + "&idO=" + Me.idDomandaOpzione.ToString())
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)


        iDom = e.Item.ItemIndex

        DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, True))


    End Sub

    Private Sub GRVElenco_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        Page.Validate()
        If Page.IsValid Then
            For Each row As GridViewRow In GRVElenco.Rows
                Dim ctrlTXB As Control
                Dim ctrlLB As Control
                ctrlTXB = row.FindControl("TXBValutazioneOpzioneLibera")
                ctrlLB = row.FindControl("LBRispostaID")
                Dim TXBValutazioneOpzioneLibera As TextBox
                TXBValutazioneOpzioneLibera = ctrlTXB
                Dim LBRispostaID As Label
                LBRispostaID = ctrlLB
                DALRisposte.ValutazioneRispostaTestoLibero_Update(LBRispostaID.Text, TXBValutazioneOpzioneLibera.Text)
            Next
        End If
        BindDati()
    End Sub
    Protected Sub GRVElenco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GRVElenco.SelectedIndexChanged
        If Not GRVElenco.SelectedDataKey Is Nothing Then
            Dim idR As String = GRVElenco.SelectedDataKey.Value
            Dim oRisposta As New RispostaQuestionario

            For Each ris As RispostaQuestionario In Me.QuestionarioCorrente.risposteQuestionario
                If ris.id = idR Then
                    oRisposta = ris
                End If
            Next

            PNLQuestionarioUtente.Visible = True
            PNLQuestionari.Visible = False

            Me.QuestionarioCorrente = DALQuestionario.readQuestionarioStatisticheByPersona(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.tipo, 0, oRisposta.id)

            With Me.Resource
                .setLabel(LBNomeUtente)
            End With

            Dim oPersona As New UtenteInvitato
            If oRisposta.idUtenteInvitato > 0 Then
                oPersona = DALUtenteInvitato.readUtenteInvitatoByID(oRisposta.idUtenteInvitato)
            ElseIf oRisposta.idPersona > 0 Then
                oPersona = DALUtenteInvitato.readPersonaByID(oRisposta.idPersona)
            End If

            LBNomeUtente.Text += oPersona.Anagrafica
            DLPagine.DataSource = Me.QuestionarioCorrente.pagine
            DLPagine.DataBind()
            DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, Me.QuestionarioCorrente.domande)

            LNBListaRisposte.Visible = True
            LBConferma.Visible = False
        End If
    End Sub
    Private Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound
        iPag = e.Item.ItemIndex

        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(iPag).FindControl("DLDomande")

        dlDomande.DataSource = Me.QuestionarioCorrente.pagine.Item(iPag).domande
        dlDomande.DataBind()
    End Sub
    Protected Sub LNBStatistiche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBStatistiche.Click
        LNBListaRisposte.Visible = False
        Me.RedirectToUrl(RootObject.QuestionarioStatisticheGenerali)
    End Sub
    Protected Sub BTNConferma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNConferma.Click
        If Page.IsValid Then


            For Each row As GridViewRow In GRVElenco.Rows
                Dim ctrlTXB As Control
                Dim ctrlLB As Control
                ctrlTXB = row.FindControl("TXBValutazioneOpzioneLibera")
                ctrlLB = row.FindControl("LBRispostaID")
                Dim TXBValutazioneOpzioneLibera As TextBox
                TXBValutazioneOpzioneLibera = ctrlTXB
                Dim LBRispostaID As Label
                LBRispostaID = ctrlLB

                Dim risposteDomanda As New List(Of RispostaDomanda)
                risposteDomanda = Me.QuestionarioCorrente.rispostaQuest.findRisposteLibere(Me.QuestionarioCorrente, Me.domandaCorrente, Me.idDomandaOpzione)
                Dim rispostaDom As RispostaDomanda = (From ris In risposteDomanda Where ris.id = LBRispostaID.Text Select ris).First
                If Not rispostaDom.valutazione = TXBValutazioneOpzioneLibera.Text Then
                    DALRisposte.ValutazioneRispostaTestoLibero_Update(LBRispostaID.Text, TXBValutazioneOpzioneLibera.Text)
                    Dim oQuestTemp As New Questionario
                    'mi serve per caricare il questionario corretto
                    Dim rispostaQuest As RispostaQuestionario = (From ris In Me.QuestionarioCorrente.risposteQuestionario Where ris.id = rispostaDom.idRispostaQuestionario Select ris).First
                    'il quest temporaneo mi serve per aggiornare il punteggio di una persona senza toccare il quest in sessione
                    oQuestTemp = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, rispostaQuest.idPersona, rispostaQuest.idUtenteInvitato, rispostaQuest.id)
                    'aggiorno la valutazione della risposta (sul db c'e' quella vecchia)
                    Dim oRisposta As RispostaDomanda = (From ris In oQuestTemp.rispostaQuest.risposteDomande Where ris.id = rispostaDom.id Select ris).First
                    oRisposta.valutazione = TXBValutazioneOpzioneLibera.Text
                    'aggiorno la valutazione in memoria, per gestire correttamente gli aggiornamenti di punteggio quando si clicca piu' volte su BTNConferma
                    rispostaDom.valutazione = oRisposta.valutazione

                    oQuestTemp.rispostaQuest.oStatistica = oGestioneRisposte.calcoloPunteggioByQuestionario(oQuestTemp, oQuestTemp.rispostaQuest.oStatistica, oQuestTemp.scalaValutazione)
                    DALRisposte.RispostaQuestionario_Update(True, oQuestTemp.rispostaQuest, False)
                End If
                LBConferma.Visible = True
            Next
        Else
            LBConferma.Visible = False
        End If
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            GRVElenco.Columns(2).Visible = False
            If Not MyBase.Servizio.Admin Then
                GRVElenco.Columns(3).Visible = True
            Else
                GRVElenco.Columns(3).Visible = False
            End If
        End If
    End Sub
    Protected Sub CUVvalutazioneOpzioneLibera_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If args.Value.Trim(" ") = String.Empty Then
            args.IsValid = True
        Else
            Try
                Dim Punteggio As Integer
                Punteggio = CInt(args.Value)
                If Punteggio > -101 And Punteggio < 101 Then
                    args.IsValid = True
                Else
                    args.IsValid = False
                End If
            Catch ex As Exception
                args.IsValid = False
            End Try
        End If
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class