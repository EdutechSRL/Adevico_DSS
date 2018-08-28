Imports COL_Questionario
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Web
Imports CrystalDecisions.Shared

Partial Public Class SondaggioAdmin
    Inherits PageBaseQuestionario
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneQuest As New GestioneQuestionario
    Public Shared isAperto As Boolean

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Service As COL_Questionario.Business.ServiceQuestionnaire
    Private ReadOnly Property CurrentService() As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_Service) Then
                _Service = New COL_Questionario.Business.ServiceQuestionnaire(Me.CurrentContext)
            End If
            Return _Service
        End Get
    End Property
#End Region


#Region "property ReadOnly"
    Public ReadOnly Property questionariSuInvito() As String
        Get
            Return (MyBase.Servizio.QuestionariSuInvito Or MyBase.Servizio.Admin).ToString
            'Return MyBase.Servizio.QuestionariSuInvito.ToString()
        End Get
    End Property

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

#End Region
#Region "Button Click Events"
    Protected Sub BTNCopiaCartella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCopiaCartella.Click
        'oQuest = Me.QuestionarioCorrente
        Me.QuestionarioCorrente.idGruppo = DDLCartelle.SelectedItem.Value
        Dim redUrl As String = oGestioneQuest.copiaQuestionario(FRVQuestionario)
    End Sub
    Protected Sub BTNCopiaComunita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCopiaComunita.Click
        If Not DDLComunita.SelectedItem.Value = Integer.MinValue Then
            Me.QuestionarioCorrente.idGruppo = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(DDLComunita.SelectedItem.Value)
            If Me.QuestionarioCorrente.idGruppo = 0 Then
                oGestioneQuest.creaGruppoDefault(DDLComunita.SelectedItem.Value)
                Me.QuestionarioCorrente.idGruppo = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(DDLComunita.SelectedItem.Value)
            End If
            LBCopiaComunita.Visible = True
            LBerrore.Visible = False
            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
            SetInternazionalizzazione()
            Dim redUrl As String = oGestioneQuest.copiaQuestionario(FRVQuestionario)
            If Not redUrl = String.Empty Then
                Me.RedirectToUrl(redUrl)
            End If
        Else
            LBerrore.Text = LBerrore.Text & Me.Resource.getValue("MSGNoComunita")
            MLVquestionari.SetActiveView(VIWmessaggi)
        End If

    End Sub
    Protected Sub BTNComunitaGruppo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNComunitaGruppo.Click
        DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Enabled = False
        DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Enabled = False

        DDLComunitaGruppo.DataSource = DALQuestionarioGruppo.readGruppi(DDLComunita.SelectedValue)
        DDLComunitaGruppo.Visible = True
        DDLComunitaGruppo.DataBind()
    End Sub
    Protected Sub BTNSalvaAltraLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNSalvaAltraLingua.Click
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, 0, False)
        Me.QuestionarioCorrente.isReadOnly = True
        DALQuestionario.chiudiQuestionario(Me.QuestionarioCorrente)
        If Not DDLNuovaLingua.SelectedValue = String.Empty Then
            Me.QuestionarioCorrente.idLingua = DDLNuovaLingua.SelectedValue
            Me.QuestionarioCorrente.nome = Me.QuestionarioCorrente.nome & " (" & DDLNuovaLingua.SelectedItem.Text & ")"
            Me.RedirectToUrl(oGestioneQuest.copiaQuestionarioMultilingua(FRVQuestionario))
        End If
    End Sub
    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        Try
            tornaAllaLista()
        Catch ex As Exception
            BindDati()
        End Try
    End Sub
    Protected Sub tornaAllaLista()
        Try
            Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&" & qs_questType & Questionario.TipoQuestionario.Sondaggio)
        Catch ex As Exception
            BindDati()
        End Try
    End Sub
    Private Sub save(Optional ByVal goToList As Boolean = True)
        Dim saveOK As Boolean
        If Me.QuestionarioCorrente Is Nothing Then
            Me.QuestionarioCorrente = New Questionario
        End If
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio
        If Not DDLGruppo.SelectedValue = String.Empty Then
            Me.QuestionarioCorrente.idGruppo = DDLGruppo.SelectedValue
        End If

        Me.QuestionarioCorrente.forUtentiComunita = CHKUtentiComunita.Checked
        Me.QuestionarioCorrente.forUtentiPortale = CHKUtentiNonComunita.Checked
        'If Me.ComunitaCorrenteID = 0 Then
        '    Me.QuestionarioCorrente.forUtentiPortale = True
        'Else
        '    Me.QuestionarioCorrente.forUtentiPortale = False
        'End If
        Me.QuestionarioCorrente.forUtentiInvitati = CHKUtentiInvitati.Checked
        Me.QuestionarioCorrente.forUtentiEsterni = CHKUtentiEsterni.Checked
        Me.QuestionarioCorrente.idPersonaEditor = Me.UtenteCorrente.ID
        Me.QuestionarioCorrente.dataModifica = Now

        saveOK = oGestioneQuest.salvaQuestionario(FRVQuestionario, Me.QuestionarioCorrente)

        If Not DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList).SelectedValue = Me.QuestionarioCorrente.idLingua Then
            DALQuestionario.updateQuestionarioDefault(Me.QuestionarioCorrente.id, Integer.Parse(DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList).SelectedValue))
        End If
        If saveOK Then
            setPagina(Me.QuestionarioCorrente.pagine(0))
            DALPagine.Pagina_Salva(Me.QuestionarioCorrente.pagine(0))
            Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)
            PaginaCorrenteID = Me.QuestionarioCorrente.pagine(0).id
            Me.DomandaCorrente.idPagina = PaginaCorrenteID
            'Me.DomandaCorrente = Me.QuestionarioCorrente.pagine(0).domande(0)
            Dim returnValue As Int16
            Dim frvdomanda As New FormView
            frvdomanda = DirectCast(GestioneDomande.FindControlRecursive(PHOpzioni, "FRVDomanda"), FormView)
            'oGestioneDomande.setDomandaMeeting(frvdomanda,
            returnValue = oGestioneDomande.salvaDomanda(frvdomanda)

            Select Case returnValue
                Case 0
                    If Not Me.QuestionarioCorrente.isBloccato Then
                        oGestioneQuest.notifyCurrentQuestionnaire()
                    End If
                    If goToList Then
                        tornaAllaLista()
                    End If
                Case 1
                    LBerrore.Text = Me.Resource.getValue("ErroreOpzioni")
                    LBerrore.Visible = True
                Case 2
                    LBerrore.Text = Me.Resource.getValue("ErroreTestoDomanda")
                    LBerrore.Visible = True
                Case 3
                    LBerrore.Text = Me.Resource.getValue("ErroreNoCorretta")
                    LBerrore.Visible = True
            End Select

        End If
        If Not goToList Then
            Me.RedirectToUrl(RootObject.UtentiInvitati & "?IdQ=" & QuestionarioCorrente.id)
        End If
    End Sub
    Protected Sub LNBSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva.Click
        save()
    End Sub
    Protected Sub BTNInvitaUtenti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNInvitaUtenti.Click
        save(False)
    End Sub

#End Region
#Region "Bind Controls"
    Protected Sub bindDateTime()
        Dim counter As Int32
        For counter = 0 To 9
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items(counter).Text = "0" & counter
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items(counter).Value = counter
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items(counter).Text = "0" & counter
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items(counter).Value = counter
        Next
        For counter = 10 To 23
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items.Add(counter)
        Next
        For counter = 0 To 1
            'visualizza 00 e 05 per i minuti
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Text = "0" & counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Value = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Text = "0" & counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Value = counter * 5
        Next
        For counter = 2 To 11
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Text = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Value = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Text = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Value = counter * 5
        Next

        DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).SelectedDate = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Date
        DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).SelectedDate = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Date
        DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Hour
        DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Hour


        Dim minIndex As Int16
        minIndex = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Minute / 5
        If minIndex = 12 Then
            minIndex = 11
        End If
        DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedIndex = minIndex
        minIndex = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Minute / 5
        If minIndex = 12 Then
            minIndex = 11
        End If
        DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedIndex = minIndex
    End Sub
    Protected Sub bindDDLCartelle()
        'temporaneamente disattivo
        LBAltraCartella.Visible = False
        BTNCopiaCartella.Visible = False
        LBGruppo.Visible = False
        DDLCartelle.Visible = False
        DDLGruppo.Visible = False
        BTNComunitaGruppo.Visible = False
        DDLComunitaGruppo.Visible = False
        'DDLCartelle.DataSource = DALQuestionarioGruppo.readGruppi(Me.ComunitaCorrenteID)
        'DDLCartelle.DataBind()
    End Sub
    Protected Sub bindDDLComunita()
        DDLComunita.DataSource = DALQuestionario.readComunitaByIDPersona(Me.UtenteCorrente.ID, Me.ComunitaCorrente)
        DDLComunita.DataBind()
    End Sub
    Protected Sub bindDDLGruppo()
        Try
            DDLGruppo.DataSource = DALQuestionarioGruppo.readGruppi(Me.ComunitaCorrenteID)
            DDLGruppo.DataBind()
            DDLGruppo.SelectedValue = Me.GruppoCorrente.id
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub bindDDLLingua()
        DDLNuovaLingua.DataSource = DALQuestionario.readLingueNonPresentiQuestionario(Me.QuestionarioCorrente.id)
        DDLNuovaLingua.DataBind()
    End Sub
#End Region

    Private Sub caricaDati()
        If Not Me.QuestionarioCorrente Is Nothing Then

            If Me.QuestionarioCorrente.id > 0 Then
                Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)
                Me.QuestionarioCorrente.url = Me.EncryptedUrl(RootObject.compileUrlUI, "idq=" & Me.QuestionarioCorrente.id & "&idl=" & Me.QuestionarioCorrente.idLingua & "&ida=1", SecretKeyUtil.EncType.Questionario)
                isAperto = Not Me.QuestionarioCorrente.isReadOnly
                HYPUrl.NavigateUrl = Me.QuestionarioCorrente.url
                CHKUtentiComunita.Checked = Me.QuestionarioCorrente.forUtentiComunita
                CHKUtentiNonComunita.Checked = Me.QuestionarioCorrente.forUtentiPortale
                CHKUtentiInvitati.Checked = Me.QuestionarioCorrente.forUtentiInvitati
                CHKUtentiEsterni.Checked = Me.QuestionarioCorrente.forUtentiEsterni
            Else
                isAperto = True
                Me.QuestionarioCorrente.dataCreazione = Now
                Me.QuestionarioCorrente.dataInizio() = Now
                Me.QuestionarioCorrente.dataFine() = Date.MaxValue
                Me.QuestionarioCorrente.scalaValutazione() = RootObject.scalaValutazione
                Me.QuestionarioCorrente.idPersonaCreator = Me.UtenteCorrente.ID
                Me.QuestionarioCorrente.risultatiAnonimi = True
                CHKUtentiComunita.Checked = True
                'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random Then
                Me.QuestionarioCorrente.visualizzaRisposta = False
                Me.QuestionarioCorrente.editaRisposta = False
                'Else
                '    Me.QuestionarioCorrente.visualizzaRisposta = True
                '    Me.QuestionarioCorrente.editaRisposta = True
                'End If
                Me.QuestionarioCorrente.visualizzaCorrezione = False
                Me.QuestionarioCorrente.visualizzaSuggerimenti = False
                Me.QuestionarioCorrente.isBloccato = True
            End If
        Else
            Me.QuestionarioCorrente = New Questionario
        End If

        bindDDLCartelle()
        bindDDLComunita()
        bindDDLLingua()

        Dim idGruppo As Integer
        Try
            idGruppo = Me.GruppoCorrente.id
            bindDDLGruppo()
        Catch ex As Exception
            idGruppo = 0
        End Try
        'Me.DomandaCorrente = Me.QuestionarioCorrente.pagine(0).domande(0)
        oGestioneQuest.bindFieldQuestionario(FRVQuestionario, PNLDestinatari, Me.QuestionarioCorrente, idGruppo)
        bindDateTime()

    End Sub
    Protected Sub CUVNome_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim Nome As String
        Dim oQuest As Questionario
        oQuest = Me.QuestionarioCorrente
        Nome = DirectCast(FRVQuestionario.FindControl("TXBNome"), TextBox).Text
        args.IsValid = Not oGestioneQuest.IsDuplicatedName(oQuest.id, oQuest.tipo, Nome)
    End Sub
    Protected Sub CUVdate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim DataInizio As Date
        Dim DataFine As Date
        DataInizio = DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedValue)
        DataFine = DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedValue)
        If DateDiff(DateInterval.Second, DataInizio, DataFine) < 0 And Not DataFine = Date.MaxValue Then
            args.IsValid = False
            Exit Sub
        End If
    End Sub
    Private Function setPagina(ByVal oPagina As QuestionarioPagina)
        If oPagina Is Nothing Then
            oPagina = New QuestionarioPagina
        End If

        oPagina.id = Me.PaginaCorrenteID
        oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua

        oPagina.descrizione = ""
        oPagina.nomePagina = Me.QuestionarioCorrente.nome


        oPagina.randomOrdineDomande = 0 'DirectCast(FRVPagina.FindControl("CBRandomOrdine"), CheckBox).Checked
        If oPagina.id = 0 Then
            oPagina.numeroPagina = 1
            oPagina.dallaDomanda = 0
            oPagina.allaDomanda = 0
        End If
        Me.QuestionarioCorrente.pagine.Clear()
        Me.QuestionarioCorrente.pagine.Add(oPagina)

    End Function
    Public Overrides Sub BindDati()
        caricaDati()
        'o cosi', o in quelle due pagine si gestisce il fatto che le risposte vengono cancellate quando si fa update delle opzioni
        'Me.QuestionarioCorrente.id = 0 serve solo per escludere le risp. orfane, che bloccherebbero il sistema per tutti i nuovi
        If Not (DALRisposte.countRisposteBYIDQuestionario(Me.QuestionarioCorrente.id) = 0 OrElse Me.QuestionarioCorrente.id = 0) Then
            RedirectToUrl(RootObject.QuestionarioAdmin & "?IdQ=" & qs_questId & "&type=2")
        End If
        SetCampiVisibili()
        If Me.QuestionarioCorrente.linguePresenti.Count > 1 Then
            DirectCast(FRVQuestionario.FindControl("CKisChiuso"), CheckBox).Enabled = False
        End If
        FRVQuestionario.FindControl("LBDescrizioneQuestionario").Visible = False
        FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario").Visible = False
        Me.DomandaCorrente = New Domanda
        Me.DomandaCorrente.id = 0
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla


        'bind paginaEdit
        Dim oPagina As New QuestionarioPagina
        'oQuest = Me.QuestionarioCorrente
        If Me.QuestionarioCorrente.pagine.Count > 0 Then
            Me.PaginaCorrenteID = Me.QuestionarioCorrente.pagine(0).id
        End If
        If Me.PaginaCorrenteID > 0 Then
            oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
            ViewState("oPagina") = oPagina
        End If
        If oPagina Is Nothing Then
            setPagina(oPagina)
        Else
            If oPagina.id = 0 Then
                setPagina(oPagina)
            End If
        End If


    End Sub
    Public Sub SetCampiVisibili()
        If Not (Me.QuestionarioCorrente.id > 0 And Me.QuestionarioCorrente.domande.Count > 0) Then
            PNLCopiaQuestionario.Style.Item("display") = "none"
            LBGestioneAvanzata.Visible = False
        End If

        DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Visible = True
        DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Checked = True
        DirectCast(FRVQuestionario.FindControl("CHKvisualizzaCorrezione"), CheckBox).Visible = False
        DirectCast(FRVQuestionario.FindControl("LBvisualizzaCorrezione"), Label).Visible = False
        DirectCast(FRVQuestionario.FindControl("DIVDurata"), HtmlControls.HtmlControl).Style("display") = "none"
        'DirectCast(FRVQuestionario.FindControl("LBdurata"), Label).Visible = False
        'DirectCast(FRVQuestionario.FindControl("TBdurata"), TextBox).Visible = False
        DirectCast(FRVQuestionario.FindControl("LBScalaValutazione"), Label).Visible = False
        DirectCast(FRVQuestionario.FindControl("TXBScalaValutazione"), TextBox).Visible = False
        DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom"), Label).Visible = False
        DirectCast(FRVQuestionario.FindControl("CHKOrdineDomandeRandom"), CheckBox).Visible = False
        DirectCast(FRVQuestionario.FindControl("CHKvisualizzaSuggerimenti"), CheckBox).Visible = False
        DirectCast(FRVQuestionario.FindControl("LBvisualizzaSuggerimenti"), Label).Visible = False
        DirectCast(FRVQuestionario.FindControl("CHKeditaRisposta"), CheckBox).Checked = True
       
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWdati)
        LNBSalva.Visible = (MyBase.Servizio.Admin)
        PNLQuestionario.Enabled = (MyBase.Servizio.Admin)
        PNLDestinatari.Enabled = (MyBase.Servizio.Admin)
        PNLTipoGrafico.Enabled = (MyBase.Servizio.Admin)
        PNLCopiaQuestionario.Enabled = (MyBase.Servizio.Admin Or MyBase.Servizio.CopiaQuestionario)
        BTNSalvaAltraLingua.Enabled = (MyBase.Servizio.Admin)
        'solo il sysadmin può creare modelli pubblici, e li puo' creare solo prima di accedere a una comunita'
        'non esiste una comunita', quindi non ha senso "compilabile dagli utenti della comunita'"
        If Me.ComunitaCorrenteID = 0 And MyBase.Servizio.Admin Then
            DIVutentiNonComunita.Style("display") = "block"
            DIVutentiComunita.Style("display") = "none"
            If Me.QuestionarioCorrente.id = 0 Then
                'viene impostato solo se il questionario e' appena stato creato
                CHKUtentiNonComunita.Checked = True
            End If
        Else
            DIVutentiNonComunita.Style("display") = "none"
            DIVutentiComunita.Style("display") = "block"
            CHKUtentiNonComunita.Checked = False
        End If
        DirectCast(FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario"), Comunita_OnLine.UC_Editor).IsEnabled = (MyBase.Servizio.Admin)
    End Sub
    Public Overrides Sub BindNoPermessi()
        LBerrore.Visible = True
        LBerrore.Text = LBerrore.Text & Me.Resource.getValue("MSGNoPermessi")
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito)
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SondaggioAdmin", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            If qs_questId > 0 Then
                Master.ServiceTitle = .getValue("ServiceTitle.Edit." & qs_questIdType.ToString)
            Else
                Master.ServiceTitle = .getValue("ServiceTitle." & qs_questIdType.ToString)
            End If
            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBSalva, False, False)
            LNBSalvaBottom.Text = LNBSalva.Text
            LNBSalvaBottom.ToolTip = LNBSalva.ToolTip
            .setLinkButton(LNBTornaGestioneQuestionari, False, False)

            .setLabel(LBDestinatari)
            .setLabel(LBDescrizione)
            .setLabel(LBGestioneAvanzata)
            .setLabel(LBGruppo)
            .setLabel(LBAltraCartella)
            .setLabel(LBAltraComunita)
            .setLabel(LBNuovaLingua)
            .setLabel(LBinserireNome)
            .setLabel(LBerrore)
            .setLabel(LBCopiaComunita)
            .setLabel(LBCopiaSottocartella)
            .setLabel(LBTipoGrafico)
            .setLabel(LBTitoloUrl)

            .setCheckBox(CHKUtentiComunita)
            .setCheckBox(CHKUtentiNonComunita)
            .setCheckBox(CHKUtentiInvitati)
            .setCheckBox(CHKUtentiEsterni)

            .setButton(BTNInvitaUtenti, False, False, False, False)
            .setButton(BTNCopiaCartella, False, False, False, False)
            .setButton(BTNComunitaGruppo, False, False, False, False)
            .setButton(BTNCopiaComunita, False, False, False, False)
            .setButton(BTNSalvaAltraLingua, False, False, False, False)


        End With
        'LNBElimina.OnClientClick = Me.Resource.getValue("LNBElimina.OnClientClick")
    End Sub
    Private Sub FRVQuestionario_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVQuestionario.DataBound

        Dim oImg As ImageButton

        oImg = Me.FRVQuestionario.FindControl("IMBHelp")

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpQuestionario, "target", "yes", "yes"))

        With Me.Resource
            .setLabel(FRVQuestionario.FindControl("LBDatiQuestionario"))
            .setLabel(FRVQuestionario.FindControl("LBNome"))
            .setCustomValidator(FRVQuestionario.FindControl("CUVNome"))
            .setRequiredFieldValidator(FRVQuestionario.FindControl("RFVNome"), True, False)
            .setLabel(FRVQuestionario.FindControl("LBLingua"))
            .setLabel(FRVQuestionario.FindControl("LBdescrizioneQuestionario"))
            .setLabel(FRVQuestionario.FindControl("LBTitoloDataCreazione"))
            .setLabel(FRVQuestionario.FindControl("LBDataInizioTitolo"))
            .setLabel(FRVQuestionario.FindControl("LBDataFineTitolo"))
            .setCustomValidator(FRVQuestionario.FindControl("CUVdate"))
            .setLabel(FRVQuestionario.FindControl("LBisBloccato"))
            .setLabel(FRVQuestionario.FindControl("LBisChiuso"))
            .setLabel(FRVQuestionario.FindControl("LBModalita"))
            .setLabel(FRVQuestionario.FindControl("LBDurata"))
            .setLabel(FRVQuestionario.FindControl("LBScalaValutazione"))
            '.setLabel(FRVQuestionario.FindControl("LBRisultatiAnonimi"))
            .setLabel(FRVQuestionario.FindControl("LBTitoloUrl"))
            .setLabel(FRVQuestionario.FindControl("LBLinguaDefault"))
            .setLabel(FRVQuestionario.FindControl("LBpermessiCompilatore"))
            .setLabel(FRVQuestionario.FindControl("LBvisualizzaRisposta"))
            .setLabel(FRVQuestionario.FindControl("LBvisualizzaCorrezione"))
            .setLabel(FRVQuestionario.FindControl("LBvisualizzaSuggerimenti"))
            .setLabel(FRVQuestionario.FindControl("LBeditaRisposta"))
            .setLabel(FRVQuestionario.FindControl("LBAiuto"))
            .setCompareValidator(FRVQuestionario.FindControl("COVDurataInt"))
            .setCompareValidator(FRVQuestionario.FindControl("COVScalaValutazioneInt"))
            .setLabel(FRVQuestionario.FindControl("LBOrdineDomandeRandom"))
            .setLabel(FRVQuestionario.FindControl("LBanonymousResults_t"))
            .setLabel(FRVQuestionario.FindControl("LBanonymousResults"))
        End With

        Dim editor As Comunita_OnLine.UC_Editor = FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario")
        If Not editor.isInitialized Then
            editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
        End If


        'Try
        '    If Not IsNothing(FRVQuestionario.DataItem) Then
        '        editor.HTML = DirectCast(FRVQuestionario.DataItem, Questionario).descrizione
        '    End If

        'Catch ex As Exception

        'End Try

        Dim CHKvisualizzaCorrezione As CheckBox
        Dim CHKvisualizzaRisposta As CheckBox
        Dim CHKeditaRisposta As CheckBox
        Dim CHKvisualizzaSuggerimenti As CheckBox
        Dim oCheckbox As CheckBox = FRVQuestionario.FindControl("CBXanonymousResults")
        oCheckbox.Enabled = (Me.QuestionarioCorrente.id = 0) OrElse CurrentService.CanChangeAnonymousStatus(Me.QuestionarioCorrente.id)

        CHKvisualizzaCorrezione = FRVQuestionario.FindControl("CHKvisualizzaCorrezione")
        CHKvisualizzaRisposta = FRVQuestionario.FindControl("CHKvisualizzaRisposta")
        CHKeditaRisposta = FRVQuestionario.FindControl("CHKeditaRisposta")
        CHKvisualizzaSuggerimenti = FRVQuestionario.FindControl("CHKvisualizzaSuggerimenti")

        CHKvisualizzaRisposta.Attributes.Add("onclick", "JSvisualizzaRisposta(this,'" & CHKvisualizzaCorrezione.ClientID & "','" & CHKeditaRisposta.ClientID & "','" & CHKvisualizzaSuggerimenti.ClientID & "'); return true;")
        CHKvisualizzaCorrezione.Attributes.Add("onclick", "JSvisualizzaCorrezione(this,'" & CHKvisualizzaRisposta.ClientID & "','" & CHKeditaRisposta.ClientID & "','" & CHKvisualizzaSuggerimenti.ClientID & "'); return true;")
        CHKvisualizzaSuggerimenti.Attributes.Add("onclick", "JSvisualizzaSuggerimenti(this,'" & CHKvisualizzaRisposta.ClientID & "','" & CHKvisualizzaCorrezione.ClientID & "','" & CHKeditaRisposta.ClientID & "'); return true;")
        CHKeditaRisposta.Attributes.Add("onclick", "JSeditaRisposta(this,'" & CHKvisualizzaSuggerimenti.ClientID & "','" & CHKvisualizzaCorrezione.ClientID & "','" & CHKvisualizzaRisposta.ClientID & "'); return true;")

        CHKUtentiInvitati.Visible = Me.questionariSuInvito
        BTNInvitaUtenti.Visible = Me.questionariSuInvito
    End Sub
    Private Sub LNBTornaGestioneQuestionari_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBTornaGestioneQuestionari.Click
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.ComunitaCorrenteID > 0 AndAlso Request.QueryString("isZero") = "true" Then
            GoToPortale()
        End If
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla
        PHOpzioni.Controls.Clear()
        PHOpzioni.Controls.Add(oGestioneDomande.addUCDomandaEdit(Me.DomandaCorrente.tipo))

    End Sub
    Private Sub LNBSalvaBottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBSalvaBottom.Click
        LNBSalva_Click(sender, e)
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class