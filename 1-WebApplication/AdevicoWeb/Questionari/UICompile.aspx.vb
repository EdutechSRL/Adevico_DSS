Imports COL_Questionario
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Questionario

Partial Public Class UICompile_
    Inherits PageBaseQuestionario


    Private _EditableQuestionnaire As Boolean?
    Private Property IsEditableQuestionnaire As Boolean
        Get
            If Not _EditableQuestionnaire.HasValue Then
                _EditableQuestionnaire = QuestionarioCorrente.id > 0 AndAlso (QuestionarioCorrente.editaRisposta OrElse (Not QuestionarioCorrente.editaRisposta AndAlso CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id)))
                If _EditableQuestionnaire.Value AndAlso QuestionarioCorrente.durata > 0 Then
                    _EditableQuestionnaire = CurrentService.IsAnswerSavingAllowed(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.id)
                End If
            End If
            Return _EditableQuestionnaire.Value
        End Get
        Set(value As Boolean)
            _EditableQuestionnaire = value
        End Set
    End Property

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

#Region "condivisa"


    ' fino a isCorrezione erano tutte Public Shared 
    Dim iDom As Integer
    Dim _iPag As Integer
    Dim isCorrezione As Boolean

    Dim oPagedDataSource As New PagedDataSource
    Dim bindDone As Boolean
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneRisposte As New GestioneRisposte
    Dim oGestioneQuest As New GestioneQuestionario
    Public ReadOnly Property displayDifficulty() As String
        Get
            If Not (Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting) AndAlso QuestionarioCorrente.isPassword Then
                Return "hide" '"text-align: right;" 'True
            Else
                Return "show" '"display:none;" 'False
            End If
        End Get
    End Property
    Public ReadOnly Property showDifficulty() As Boolean
        Get
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private Property startTime() As DateTime
        Get
            Return CDate(ViewState("startTime"))
        End Get
        Set(ByVal value As DateTime)
            ViewState("startTime") = value
        End Set
    End Property
    Private Property isFirstRun() As Boolean
        Get
            Return CBool(ViewState("isFirstRun"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("isFirstRun") = value
        End Set
    End Property
    Private _SmartTagsAvailable As SmartTags
    Public Shared idCollection As New List(Of Integer)
    Private Sub addUCpnlValutazione()
        'PNLmenu.Visible = False
        PHucValutazione.Controls.Clear()
        Dim ctrl As New Control
        ctrl = Page.LoadControl(RootObject.ucPNLValutazione)
        ctrl.ID = "ucPNLvaluta"
        DirectCast(ctrl, UCpnlValutazione).carica()
        PHucValutazione.Controls.Add(ctrl)
    End Sub
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = True
        End Get
    End Property
    Public ReadOnly Property isAutovalutazione() As Boolean
        Get
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Private Property iPag() As Integer
        Get
            Return ViewState("_iPag")
        End Get
        Set(ByVal value As Integer)
            ViewState("_iPag") = value
        End Set
    End Property
    Protected ReadOnly Property MandatoryDisplay(ByVal question As Domanda) As String
        Get
            Dim mandatory As String = ""
            If question.isObbligatoria Then
                mandatory = "<span class=""mandatory"">" & Me.Resource.getValue("isMandatory") & "</span>"
            End If
            Return mandatory
        End Get
    End Property
    Protected ReadOnly Property MandatoryToolTip(ByVal question As Domanda) As String
        Get
            Return IIf(question.isObbligatoria, Me.Resource.getValue("isMandatory.Title"), "")
        End Get
    End Property
    Protected Sub inviaMailErrore(ByVal ex As Exception)
        'Dim oResourceConfig As New ResourceManager
        'oResourceConfig = GetResourceConfig(Me.LinguaCode)
        Dim oSettings As ComolSettings = Me.SystemSettings
        Dim oMail As New COL_E_Mail(Me.LocalizedMail)

        'Dim oMail As New COL_E_Mail(oResourceConfig)
        oMail.HasCopiaMittente = False ' DOPO METTERE A TRUE!!
        oMail.Mittente = New MailAddress(RootObject.mailAdminQuestionari)
        Try

            oMail.Oggetto = "errore questionario utente " + Me.Invito.Anagrafica + "(" + Me.Invito.ID.ToString() + "): " + ex.Message

        Catch ex1 As Exception
            oMail.Oggetto = "errore questionario utente: " + ex.Message + " " + ex1.Message
        End Try
        oMail.Body = "Messaggio: " + ex.Message + vbCrLf + "Stack: " + ex.StackTrace
        oMail.IndirizziTO.Add(New MailAddress(RootObject.mailAdminQuestionari))

        oMail.InviaMail()

    End Sub
    Public Overrides Sub BindNoPermessi()
        CTRLerrorMessages.Visible = True
        CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoPermessi"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
        MLVquestionari.SetActiveView(VIWmessaggi)
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        Try
            iDom = e.Item.ItemIndex
            'oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, False)
            DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, Not IsEditableQuestionnaire))
            If Not (Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting AndAlso Not iDom = Me.QuestionarioCorrente.pagine(0).domande.Count - 1) Then
                Dim LBtestoDopoDomanda As New Label
                LBtestoDopoDomanda.Text = Me.QuestionarioCorrente.pagine(iPag).domande(iDom).testoDopo
                If Not LBtestoDopoDomanda.Text Is String.Empty Then
                    Dim aCapo As New LiteralControl
                    aCapo.Text = "<br>"
                    DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(aCapo)
                    DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(LBtestoDopoDomanda)
                End If
            End If

            If Me.QuestionarioCorrente.visualizzaSuggerimenti Then
                If Not ((Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione = Me.QuestionarioCorrente.tipo And isCorrezione) Or (Not Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione = Me.QuestionarioCorrente.tipo And Me.QuestionarioCorrente.isPrimaRisposta)) Then ' senza questo visualizza i suggerimenti anche quando non deve
                    Dim suggerimento As String
                    suggerimento = DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("LBSuggerimento"), Label).Text.Trim
                    If suggerimento = String.Empty Then
                        DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("LBSuggerimento"), Label).Text = String.Empty
                    Else
                        DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("LBSuggerimento"), Label).Text = Me.Resource.getValue("LBSuggerimento.text") & suggerimento
                    End If
                    DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("LBsuggerimentoOpzione"), Label).Text = Me.Resource.getValue("LBsuggerimentoOpzione.text") & DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("LBsuggerimentoOpzione"), Label).Text
                    DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("LBSuggerimento"), Label).Visible = True
                End If
            End If
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting AndAlso iDom > 0 Then
                DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("DIVDomanda"), HtmlControl).Style("display") = "none"
                'Else 'nasconde difficolta', che e' gia' gestita da showDifficulty
                '    DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("DIVDomanda"), HtmlControl).Style("display") = "block"
                '    DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("DIVCode"), HtmlControl).Style("display") = "none"
            End If
        Catch ex As Exception
            inviaMailErrore(ex)
        End Try
    End Sub
    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound
        Try
            Dim dvPageNameTop As HtmlControl = DLPagine.Controls(0).FindControl("DVpageName")
            Dim dvPageNameBottom As HtmlControl = DLPagine.Controls(0).FindControl("DVpageNameBottom")
            Dim dvPageDescription As HtmlControl = DLPagine.Controls(0).FindControl("DVpageDescription")

            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
                dvPageNameTop.Visible = False
                dvPageDescription.Visible = False
                dvPageNameBottom.Visible = False
            Else
                Dim page As QuestionarioPagina = DirectCast(e.Item.DataItem, QuestionarioPagina)
                dvPageDescription.Visible = Not String.IsNullOrEmpty(page.descrizione)
                dvPageNameTop.Visible = Not String.IsNullOrEmpty(page.nomePagina) AndAlso page.nomePagina <> "--"
                dvPageNameBottom.Visible = Not String.IsNullOrEmpty(page.nomePagina) AndAlso page.nomePagina <> "--"
            End If

            Dim dlDomande As New DataList
            dlDomande = DLPagine.Controls(0).FindControl("DLDomande")
            dlDomande.DataSource = Me.QuestionarioCorrente.pagine.Item(iPag).domande
            dlDomande.DataBind()
        Catch ex As Exception
            inviaMailErrore(ex)
        End Try
    End Sub
    Protected Sub LNBdescrizione_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBdescrizione.Click
        If Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione Then
            Response.Redirect(Request.RawUrl)
        Else
            If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
                DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
            End If
            iPag = -1
            Me.MLVquestionari.SetActiveView(Me.VIWdescrizione)
            LBdescrizioneVIWDescrizione.Text = Me.SmartTagsAvailable.TagAll(Me.QuestionarioCorrente.descrizione)
            LBnomeVIWDescrizione.Text = Me.QuestionarioCorrente.nome
        End If
    End Sub
    'Protected Sub SMTimer_AsyncPostBackError(ByVal sender As Object, ByVal e As AsyncPostBackErrorEventArgs) Handles SMTimer.AsyncPostBackError
    '    SMTimer.AsyncPostBackErrorMessage = e.Exception.Message + e.Exception.StackTrace
    'End Sub
    'Protected Sub IMBprima_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBprima.Click
    '    If iPag > 0 Then
    '        Dim isValida As Boolean = True
    '        Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida)
    '        If isValida Then
    '            If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
    '                DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
    '            End If
    '            iPag = iPag - 1
    '            LBTroppeRispostePagina.Visible = False
    '        Else
    '            LBTroppeRispostePagina.Visible = True
    '        End If
    '    End If
    '    TMDurata_Tick(sender, e)
    '    bindDLPagine()
    '    isFirstRun = True
    'End Sub
    Private Sub LkbBack_Click(sender As Object, e As EventArgs) Handles LkbBack.Click
        If iPag > 0 Then
            Dim isValida As Boolean = True
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida)
            If isValida Then
                If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
                    DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
                End If
                iPag = iPag - 1
                LBTroppeRispostePagina.Visible = False
            Else
                LBTroppeRispostePagina.Visible = True
            End If
        End If
        TMDurata_Tick(sender, e)
        bindDLPagine()
        isFirstRun = True
    End Sub

    'Protected Sub IMBdopo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBdopo.Click

    'End Sub
    Private Sub LkbNext_Click(sender As Object, e As EventArgs) Handles LkbNext.Click
        TMDurata_Tick(sender, e)
        vaiPaginaDopo()
        isFirstRun = True
    End Sub
    Protected Sub vaiPaginaDopo()
        LBnoRisposta.Visible = False
        If iPag > -2 Then
            Dim isValida As Boolean = True
            If isCorrezione Or Not Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione Then
                Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida)
            End If
            If isValida Then
                LBTroppeRispostePagina.Visible = False
                If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                    If isCorrezione Then
                        isCorrezione = False
                    Else
                        If iPag = Me.QuestionarioCorrente.domande.Count - 1 Then
                            If idCollection.Count > 0 Then
                                Dim oGestioneQuestionario As New GestioneQuestionario
                                Dim oPagina As New QuestionarioPagina
                                oPagina.allaDomanda = Me.QuestionarioCorrente.pagine.Count + 1
                                oPagina.dallaDomanda = oPagina.allaDomanda - 1
                                Dim oDomanda As New Domanda
                                oDomanda = oGestioneQuestionario.domandaSelect(idCollection, Me.QuestionarioCorrente.idLingua)
                                oDomanda.numero = oPagina.allaDomanda
                                oPagina.domande.Add(oDomanda)
                                Me.QuestionarioCorrente.pagine.Add(oPagina)
                                Me.QuestionarioCorrente.domande.Add(oDomanda)
                            Else
                                CTRLerrorMessages.Visible = True
                                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoMoreQuestions"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                                BTNRestartAutoEval.Visible = True
                                'da controlloare a che serve e se serve anche qui
                                'BTNSalvaAutovalutazione.Visible = True
                                Exit Sub
                            End If
                            isCorrezione = True
                        End If
                        If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
                            DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
                        End If
                        iPag = iPag + 1
                    End If
                ElseIf Me.QuestionarioCorrente.pagine.Count > iPag + 1 Then
                    'senza questo controllo se si aggiorna la pagina va alla pagine seguente anche se non esiste
                    If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
                        DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
                    End If
                    iPag = iPag + 1
                    LBTroppeRispostePagina.Visible = False
                End If
            Else
                LBTroppeRispostePagina.Visible = True
            End If
        End If
        bindDLPagine()
    End Sub
    Private Sub BTNIniziaFacile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNIniziaFacile.Click
        idCollection = DALDomande.readIdDomandeAutovalutazione(Me.QuestionarioCorrente.id, Domanda.DifficoltaDomanda.Bassa)
        BTNinizia_Click(sender, e)
    End Sub
    Private Sub BTNIniziaMedio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNIniziaMedio.Click
        idCollection = DALDomande.readIdDomandeAutovalutazione(Me.QuestionarioCorrente.id, Domanda.DifficoltaDomanda.Media)
        BTNinizia_Click(sender, e)
    End Sub
    Private Sub BTNIniziaDifficile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNIniziaDifficile.Click
        idCollection = DALDomande.readIdDomandeAutovalutazione(Me.QuestionarioCorrente.id, Domanda.DifficoltaDomanda.Alta)
        BTNinizia_Click(sender, e)
    End Sub
    Private Sub BTNIniziaMisto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNIniziaMisto.Click
        idCollection = DALDomande.readIdDomandeAutovalutazione(Me.QuestionarioCorrente.id, -1)
        BTNinizia_Click(sender, e)
    End Sub
    Private Sub BTNDopo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNDopo.Click
        TMDurata_Tick(sender, e)
        vaiPaginaDopo()
    End Sub
    Private Sub BTNRestartAutoEval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNRestartAutoEval.Click
        Response.Redirect(Request.RawUrl)

    End Sub
    'se si riattiva e' necessario far caricare tutte le risposte al meeting
    'Class dtoMeetingStat
    '    Public meetingDate As Date
    '    Public ReadOnly Property meetingDateStr() As String
    '        Get
    '            Return meetingDate
    '        End Get
    '    End Property
    '    Public ReadOnly Property isAll() As Boolean
    '        Get
    '            If votes = maxVotes Then
    '                Return True
    '            Else : Return False
    '            End If
    '        End Get
    '    End Property
    '    Public zoneIndex As Integer
    '    Public zone As String
    '    Public votes As String
    '    Public maxVotes As String
    'End Class
    'Private Sub VIWMeeting_load()
    '    Dim oStatList As New List(Of dtoMeetingStat)
    '    For Each oDomanda As Domanda In Me.QuestionarioCorrente.domande
    '        For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating
    '            For c As Integer = 1 To oDomanda.domandaRating.numeroRating
    '                Dim oStat As New dtoMeetingStat
    '                Dim count = (From ri In oDomanda.risposteDomanda Where ri.valore = c And ri.idDomandaOpzione = oOpz.id Select ri.id).ToList.Count
    '                oStat.votes = count
    '                oStat.maxVotes = Me.QuestionarioCorrente.risposteQuestionario.Count
    '                oStat.meetingDate = oDomanda.domandaRating.intestazioniMeeting(c - 1)
    '                oStat.zone = oOpz.testo
    '                oStat.zoneIndex = oOpz.numero
    '                oStatList.Add(oStat)
    '            Next
    '        Next
    '    Next
    '    'La lista viene ordinata per numero di risposte, quindi per data, quindi per indice della zona
    '    RPTRisposte.DataSource = oStatList.OrderByDescending(Function(c) c.votes).ThenBy(Function(c) c.meetingDate).ThenBy(Function(c) c.zoneIndex).ToList
    '    RPTRisposte.DataBind()
    'End Sub
#End Region

    Protected Sub bindDLPagine()
        Me.SetInternazionalizzazione()
        oPagedDataSource.DataSource = Me.QuestionarioCorrente.pagine
        oPagedDataSource.AllowPaging = True
        oPagedDataSource.PageSize = 1
        oPagedDataSource.CurrentPageIndex = iPag
        Dim counter As Int16 = 0
        Try
            If Me.QuestionarioCorrente.pagine.Count > 1 And iPag + 1 < Me.QuestionarioCorrente.pagine.Count Then
                LkbNext.Visible = True
                If iPag < 1 Then
                    LkbBack.Visible = False
                Else
                    LkbBack.Visible = True
                End If
                BTNFine.Visible = False
                LBAvvisoFine.Visible = False
            Else
                If Not Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                    LkbNext.Visible = False
                    BTNFine.Visible = True
                    LBAvvisoFine.Visible = True
                Else
                    BTNFine.Visible = False
                    LBAvvisoFine.Visible = False
                    If iPag < 1 Then
                        LkbBack.Visible = False
                    Else
                        LkbBack.Visible = True
                    End If
                    LkbNext.Visible = True
                End If
                If iPag > 0 Then
                    LkbBack.Visible = True
                End If
            End If
        Catch ex As Exception
            inviaMailErrore(ex)
        End Try

        'MB: era iPag > -1
        If iPag > -1 Then

            Try
                DLPagine.DataSource = oPagedDataSource
                DLPagine.DataBind()
            Catch ex As Exception
                inviaMailErrore(ex)
            End Try

            Dim showMandatoryInfo As Boolean = QuestionarioCorrente.pagine.Where(Function(p) p.domande.Where(Function(d) d.isObbligatoria).Any).Any()
            Me.LBisMandatoryInfoBottom.Visible = showMandatoryInfo
            LBisMandatoryInfoTop.Visible = showMandatoryInfo

            If Not PHnumeroPagina.Controls.Count = (Me.QuestionarioCorrente.pagine.Count) Or PHnumeroPagina.Controls.Count = 0 Then
                PHnumeroPagina.Controls.Clear()
                For counter = 1 To Me.QuestionarioCorrente.pagine.Count
                    Dim LKBpagina As New LinkButton
                    LKBpagina.Text = "&nbsp;" + counter.ToString + "&nbsp;"
                    If iPag = counter - 1 Then
                        LKBpagina.Style.Item("font-style") = "italic"
                        LKBpagina.Style.Item("font-weight") = "bolder"
                        LKBpagina.Style.Item("text-decoration") = "underline"
                    End If
                    LKBpagina.ID = "LKBpagina_" + counter.ToString
                    LKBpagina.Attributes.Add("click", "LKBpagina_OnClientClick()")
                    AddHandler LKBpagina.Click, AddressOf LKBpagina_OnClientClick
                    LKBpagina.ToolTip = "Pagina " + counter.ToString
                    Try
                        PHnumeroPagina.Controls.Add(LKBpagina)
                    Catch ex As Exception
                        inviaMailErrore(ex)
                    End Try
                Next
            Else
                Dim pageNumber As Integer = 0
                For Each item As System.Web.UI.Control In PHnumeroPagina.Controls
                    If Not iPag = pageNumber Then
                        DirectCast(item, LinkButton).Style.Clear()
                    End If
                    pageNumber += 1
                Next
            End If
            Try
                If (Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Autovalutazione) Then
                    If Not iPag = Me.QuestionarioCorrente.pagine.Count - 1 Or Not isCorrezione Then
                        'l'and serve per fare in modo che cliccando indietro le pagine vengano visualizzate sempre in modalita' "correzione"
                        DLPagine = oGestioneRisposte.setRispostePaginaCorrette(DLPagine, Me.QuestionarioCorrente.domande)
                    Else
                        DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, Me.QuestionarioCorrente.domande, Not LBTroppeRispostePagina.Visible)
                    End If
                Else
                    DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, Me.QuestionarioCorrente.domande)
                End If
                If Me.QuestionarioCorrente.visualizzaCorrezione And Not Me.QuestionarioCorrente.isPrimaRisposta And Not Me.QuestionarioCorrente.rispostaQuest.oStatistica.punteggio = Decimal.MinValue And Not Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione Then
                    addUCpnlValutazione()
                Else
                    PHucValutazione.Controls.Clear()
                End If
            Catch ex As Exception
                inviaMailErrore(ex)
            End Try
            Me.MLVquestionari.SetActiveView(Me.VIWdati)
        End If
        setCampiVisibili()

    End Sub

    Private Property Loaded() As System.Guid
        Get
            Return ViewStateOrDefault("Loaded", Guid.Empty)
        End Get
        Set(value As System.Guid)
            ViewState("Loaded") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            bindDLPagine()
        Else
            InitializeMaster()
        End If
    End Sub

    Private Sub InitializeMaster()
        isFirstRun = True
        If Loaded = Guid.Empty Then
            Dim idQuest As Integer = Me.QuestionarioCorrente.id
            Dim dto As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext
            dto = CurrentService.GetExternalContext(idQuest, Me.Invito.ID)
            dto.ShowDocType = True
            dto.CssClass = "bodyUICompile"
            Me.Master.Page_Title = "" ' Resource.getValue("Page_Title")
            Me.Master.InitializeMaster(dto)
            'TMSessione.Enabled = False
            'TMDurata.Enabled = False
            Loaded = New Guid
            If Page.IsPostBack = False Then
                BTNSalvaContinua.Visible = PageUtility.SystemSettings.Presenter.AllowSaveBetweenPageQuestionnaire
            End If
        End If
    End Sub
    Public Function gestioneVIWMessaggi() As Boolean
        CTRLerrorMessages.Visible = False
        If Not Me.QuestionarioCorrente Is Nothing Then
            If Session("isNoTempo") = True Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoTempo"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Session.Remove("isNoTempo")
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Session("isSessioneScaduta") = True Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGSessioneScaduta"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                TMSessione.Enabled = False
                Session.Remove("isSessioneScaduta")
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then
                Me.QuestionarioCorrente.rispostaQuest.idPersona = idUtenteAnonimo()
            ElseIf Me.EncryptedQueryString("idu", SecretKeyUtil.EncType.Questionario) <> "" Then
                Me.QuestionarioCorrente.rispostaQuest.idPersona = Me.Invito.PersonaID
            Else
                Me.QuestionarioCorrente.rispostaQuest.idPersona = Me.UtenteCorrente.ID
            End If
            If Me.Invito.ID = -1 Then
                'If Not DALQuestionario.isInvitato(Me.QuestionarioCorrente.id, Me.Invito.ID) Then
                'verifica che l'invito sia valido, se c'e'
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoInvito"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
                'End If
            End If
            'Me.QuestionarioCorrente = DALQuestionario.readQuestionarioByPersona(True, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, Me.UtenteCorrente.Id, Me.Invito.ID)
            'Me.QuestionarioCorrente.rispostaQuest.idQuestionario = Me.QuestionarioCorrente.id
            If IsSessioneScaduta(False) Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGSessioneScaduta"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Me.QuestionarioCorrente.isCancellato Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGQuestionarioCancellato"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Me.QuestionarioCorrente.id = 0 OrElse Me.QuestionarioCorrente.dataInizio Is Nothing OrElse Not (Me.QuestionarioCorrente.visualizzaRisposta Or Me.QuestionarioCorrente.isPrimaRisposta) Then
                'se il questionario non esiste o non ho i permessi per visualizzarlo e non e' la prima risposta
                '(se si tenta di caricare un questionario con id errato il questionario esiste, ha l'id preso in querystring, ma le date sono nothing)
                CTRLerrorMessages.Visible = True
                If Not QuestionarioCorrente.editaRisposta Then
                    If String.IsNullOrWhiteSpace(QuestionarioCorrente.rispostaQuest.dataFine) Then
                        CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.LeavedCompile.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    Else
                        CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    End If
                ElseIf Not QuestionarioCorrente.visualizzaRisposta Then
                    If String.IsNullOrWhiteSpace(QuestionarioCorrente.rispostaQuest.dataFine) Then
                        CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.LeavedCompile.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    Else
                        CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.NotViewCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    End If
                ElseIf Not QuestionarioCorrente.isPrimaRisposta Then
                    CTRLerrorMessages.InitializeControl(Resource.getValue("MSGQuestionarioNonValido"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                ElseIf Me.QuestionarioCorrente.id = 0 OrElse Me.QuestionarioCorrente.dataInizio Is Nothing Then
                    CTRLerrorMessages.InitializeControl("QuestionnaireError.Unknown.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                Else
                    CTRLerrorMessages.InitializeControl(Resource.getValue("MSGQuestionarioNonValido"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                End If
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Not (DateDiff(DateInterval.Second, Now, DateTime.Parse(Me.QuestionarioCorrente.dataFine)) > 0 And DateDiff(DateInterval.Second, Now, DateTime.Parse(Me.QuestionarioCorrente.dataInizio)) < 0) Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoData"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Me.QuestionarioCorrente.isBloccato Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGQuestionarioBloccato"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Me.QuestionarioCorrente.domande.Count = 0 Then
                'verifico che il questionario abbia domande
                If (Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Random Or Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione) And Me.QuestionarioCorrente.librerieQuestionario.Count > 0 Then
                Else
                    CTRLerrorMessages.Visible = True
                    CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoDomande"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                    Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    Return False
                End If

            End If
            If Not Me.QuestionarioCorrente.rispostaQuest.dataFine Is Nothing Then
                'se il questionario ha gia' una risposta..
                If Not Me.QuestionarioCorrente.rispostaQuest.dataFine = String.Empty Then
                    '..se la risposta e' completa..
                    If Not Me.QuestionarioCorrente.visualizzaRisposta Then
                        '..e non ho i permessi di editing/visualizzazione da errore
                        CTRLerrorMessages.Visible = True
                        CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.NotViewCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                        Return False
                    End If
                End If
            End If
            If Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then
                'se si accede al questionario tramite link anonimo e' necessario un controllo in più:
                If Not Me.QuestionarioCorrente.forUtentiEsterni Then
                    'non e' aperto agli anonimi:
                    CTRLerrorMessages.Visible = True
                    CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoPermessi"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    Return False
                End If
            Else
                If Not Me.QuestionarioCorrente.forUtentiInvitati Then
                    'non e' aperto agli invitati:
                    CTRLerrorMessages.Visible = True
                    CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoPermessi"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    Return False
                End If
            End If
            Return True
        Else
            CTRLerrorMessages.Visible = True
            CTRLerrorMessages.InitializeControl(Resource.getValue("MSGQuestionarioNonValido"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
            Return False
        End If
    End Function

    Protected Sub setCampiVisibili()
        If Not Me.QuestionarioCorrente.descrizione Is Nothing Then
            PNLmenu.Visible = (Not Me.QuestionarioCorrente.descrizione.TrimStart = "" Or Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione) And iPag > -1
        Else
            PNLmenu.Visible = Me.QuestionarioCorrente.tipo = (Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione) And iPag > -1
        End If
        LNBdescrizione.Visible = PNLmenu.Visible
        TXBPassword.Visible = Me.QuestionarioCorrente.isPassword
        LBLPassword.Visible = Me.QuestionarioCorrente.isPassword
        If Me.QuestionarioCorrente.durata = 0 Then
            LBdurata.Visible = False
        End If
        If iPag = Me.QuestionarioCorrente.pagine.Count - 1 Then
            LBAvvisoSalva.Visible = False
            LTsaveAndExit.Visible = False
            BTNSalvaContinua.Visible = False
        Else
            LBAvvisoSalva.Visible = PageUtility.SystemSettings.Presenter.AllowSaveAndContinueQuestionnaire
            LTsaveAndExit.Visible = PageUtility.SystemSettings.Presenter.AllowSaveAndContinueQuestionnaire
            BTNSalvaContinua.Visible = PageUtility.SystemSettings.Presenter.AllowSaveBetweenPageQuestionnaire
        End If

        If Not (Me.QuestionarioCorrente.editaRisposta Or Me.QuestionarioCorrente.isPrimaRisposta()) Then
            BTNSalvaContinua.Visible = False
            BTNSalvaEEsci.Visible = False
            BTNFine.Visible = False
            LBAvvisoFine.Visible = False
        End If
        If Me.isUtenteAnonimo And Me.QuestionarioCorrente.rispostaQuest.idUtenteInvitato > 0 Then
            PNLIndietro.Visible = False
            BTNSalvaEEsci.Visible = False
        End If
      
        If Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then
            BTNSalvaContinua.Visible = False
            LBAvvisoSalva.Visible = False
            LTsaveAndExit.Visible = False
        End If
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            BTNinizia.Visible = False
            BTNIniziaDifficile.Visible = True
            BTNIniziaFacile.Visible = True
            BTNIniziaMedio.Visible = True
            BTNIniziaMisto.Visible = True
            BTNSalvaContinua.Visible = False
            LBAvvisoSalva.Visible = False
            LTsaveAndExit.Visible = False
            BTNDopo.Visible = True
            LBAvvisoFine.Visible = False
            BTNFine.Visible = False
        ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            LBAvvisoSalva.Visible = False
            LTsaveAndExit.Visible = False
            BTNSalvaContinua.Visible = False
            BTNSalvaEEsci.Visible = False
        End If
    End Sub

    Public Overrides Sub BindDati()
        Dim isValidPage As Boolean = False
        If Me.EncryptedQueryString("isSessionError", SecretKeyUtil.EncType.Questionario) = "1" Then
            Dim alertMSG As String = ""
            MyBase.SetCulture("pg_QuestionarioCompile", "Questionari")
            alertMSG = Me.Resource.getValue("MSGSessionError").Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If
        isValidPage = gestioneVIWMessaggi()
        If isValidPage Then
            Dim currentType As QuestionnaireType = Me.QuestionarioCorrente.tipo
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random And Me.QuestionarioCorrente.idFiglio = 0 And Me.QuestionarioCorrente.pagine.Count = 0 Then
                Dim oGestioneQuestionario As New GestioneQuestionario
                oGestioneQuestionario.generaQuestionarioRandomDestinatario(False, currentType)
            End If

            If Not Me.QuestionarioCorrente.descrizione.TrimStart = "" Or Me.QuestionarioCorrente.durata > 0 Or Me.QuestionarioCorrente.isPassword = True Or Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Or Not Me.QuestionarioCorrente.editaRisposta Then
                If Not Me.QuestionarioCorrente.editaRisposta And Not Me.QuestionarioCorrente.durata > 0 Then
                    LBAvvisoRispostaNonEditabile.Visible = True
                End If

                If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
                    DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
                End If
                iPag = -1
                Me.MLVquestionari.SetActiveView(Me.VIWdescrizione)
                LBdescrizioneVIWDescrizione.Text = Me.SmartTagsAvailable.TagAll(Me.QuestionarioCorrente.descrizione)
                LBnomeVIWDescrizione.Text = Me.QuestionarioCorrente.nome
                'l'alert sull'avvio del calcolo del tempo appare solo al primo accesso.
                If Me.QuestionarioCorrente.isPrimaRisposta And Me.QuestionarioCorrente.durata > 0 Then
                    LBdurata.Visible = True
                    LBTempoRimanenteVIWDescrizione.Visible = True
                    LBTempoRimanenteVIWDescrizione.Text = String.Format(LBTempoRimanente.Text, Me.QuestionarioCorrente.durata)
                End If
            Else
                If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
                    DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
                End If
                iPag = 0
                Me.MLVquestionari.SetActiveView(Me.VIWdati)
                Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, True)
                If Not Me.QuestionarioCorrente.rispostaQuest.dataInizio Is Nothing Then
                    If Me.QuestionarioCorrente.rispostaQuest.dataInizio.TrimEnd = String.Empty Then
                        Me.QuestionarioCorrente.rispostaQuest.dataInizio = DateTime.Now
                        Me.QuestionarioCorrente.rispostaQuest.indirizzoIPStart = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                    End If
                Else
                    Me.QuestionarioCorrente.rispostaQuest.dataInizio = DateTime.Now
                    Me.QuestionarioCorrente.rispostaQuest.indirizzoIPStart = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                End If
                bindDLPagine()
                'se si riattiva e' necessario far caricare tutte le risposte al meeting
                'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting AndAlso Me.QuestionarioCorrente.visualizzaRisposta Then
                '    VIWMeeting_load()
                'End If
                If Page.IsPostBack = False Then
                    oGestioneQuest.CompileStartActionAdd()
                End If
            End If
        End If
        setCampiVisibili()
        If isValidPage Then
            If Not QuestionarioCorrente.editaRisposta OrElse Not QuestionarioCorrente.visualizzaRisposta Then
                DVconfirmSubmit.Attributes("title") = Resource.getValue("DVconfirmSubmit.title")
                Resource.setLabel(LBconfirmOptions)
                If Not QuestionarioCorrente.editaRisposta AndAlso BTNSalvaEEsci.Visible Then
                    DVconfirmExit.Attributes("title") = Resource.getValue("DVconfirmExit.title")
                    If QuestionarioCorrente.durata > 0 Then
                        BTNSalvaEEsci.CssClass = ""
                    Else
                        BTNSalvaEEsci.CssClass = LTconfirmExitDialogCssClass.Text
                    End If
                    Resource.setLabel(LBundoExitOption)
                    Resource.setLabel(LBconfirmExitOption)
                    LBconfirmExitOptions.Text = Resource.getValue("ExitConfirm.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString)
                End If
                DVconfirmExit.Visible = Not QuestionarioCorrente.editaRisposta AndAlso BTNSalvaEEsci.Visible
                If Not IsNothing(QuestionarioCorrente) AndAlso QuestionarioCorrente.id > 0 Then
                    LBconfirmOptions.Text = Resource.getValue("Confirm.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString & ".EditaRisposta." & QuestionarioCorrente.editaRisposta.ToString & ".VisualizzaRisposta." & QuestionarioCorrente.visualizzaRisposta.ToString)
                End If
                Resource.setLabel(LBundoOption)
                Resource.setLabel(LBconfirmOption)
                If QuestionarioCorrente.durata > 0 Then
                    BTNFine.CssClass = ""
                    BTNundoOption.CssClass = Replace(BTNundoOption.CssClass, LTcloseDialogCssClass.Text, "")
                Else
                    BTNFine.CssClass = LTconfirmDialogCssClass.Text
                End If
                DVconfirmSubmit.Visible = True
            Else
                'DVconfirmSubmit.Visible = False
                DVconfirmExit.Visible = True
            End If
        End If
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        If Not Me.QuestionarioCorrente Is Nothing Then
            If Me.QuestionarioCorrente.TipoQuestionario.Sondaggio = Me.QuestionarioCorrente.tipo OrElse Me.QuestionarioCorrente.TipoQuestionario.Meeting = Me.QuestionarioCorrente.tipo Then
                MyBase.SetCulture("pg_QuestionarioCompile_Sondaggio", "Questionari")
            Else
                MyBase.SetCulture("pg_QuestionarioCompile", "Questionari")
            End If
        Else
            MyBase.SetCulture("pg_QuestionarioCompile", "Questionari")
        End If
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '.setLabel(LBErrore)
            .setLabel(LBConferma)
            .setLabel(LBdurata)
            .setLabel(LBpagina)
            .setLabel(LBAvvisoFine)
            .setLabel(LBAvvisoSalva)
            .setLabel(LBTroppeRispostePagina)
            .setLabel(LBLErrorePassword)
            .setLabel(LBAvvisoRispostaNonEditabile)
            .setLabel(LBnoRisposta)
            .setButton(BTNFine, False, False, False, False)
            If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting AndAlso Me.QuestionarioCorrente.visualizzaRisposta Then
                BTNFine.Text = BTNFine.Text & .getValue("MSGeRisultati")
            End If
            .setButton(BTNSalvaEEsci, False, False, False, False)
            .setButton(BTNSalvaContinua, False, False, IIf(QuestionarioCorrente.durata = 0, False, True), False)
            .setButton(BTNinizia, True, False, False, False)
            .setButton(BTNIniziaDifficile, False, False, False, False)
            .setButton(BTNIniziaFacile, False, False, False, False)
            .setButton(BTNIniziaMedio, False, False, False, False)
            .setButton(BTNIniziaMisto, False, False, False, False)
            .setButton(BTNDopo, False, False, False, False)
            .setButton(BTNRestartAutoEval, False, False, False, False)
            .setLinkButton(LNBdescrizione, False, False)
            .setLinkButton(LNBindietro, False, False)

            If Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then
                HIDmessaggio.Value = String.Format(Me.Resource.getValue("LBTempoRimanente.text"), " {secondi} ", Me.Resource.getValue("secondi"))
            Else
                Try
                    If Me.Invito.Anagrafica.TrimEnd = String.Empty Then
                        HIDmessaggio.Value = Me.UtenteCorrente.Cognome & " " & Me.UtenteCorrente.Nome & ", " & String.Format(Me.Resource.getValue("LBTempoRimanente.text"), " {secondi} ", Me.Resource.getValue("secondi"))
                    Else
                        HIDmessaggio.Value = Me.Invito.Anagrafica & ", " & String.Format(Me.Resource.getValue("LBTempoRimanente.text"), " {secondi} ", Me.Resource.getValue("secondi"))
                    End If
                Catch ex As Exception
                    HttpContext.Current.Session.Abandon()
                    HttpContext.Current.Session.Clear()
                    Dim url As String = Me.EncryptedUrl(RootObject.compileUrlUI, "idq=" & Me.EncryptedQueryString("idq", SecretKeyUtil.EncType.Questionario) & "&idu=" & Me.EncryptedQueryString("idu", SecretKeyUtil.EncType.Questionario) & "&pwd=" & Me.EncryptedQueryString("pwd", SecretKeyUtil.EncType.Questionario) & "&isSessionError=1", SecretKeyUtil.EncType.Questionario)
                    Response.Redirect(url)
                End Try
            End If
            Me.LBisMandatoryInfoTop.Text = Resource.getValue("LBisMandatoryInfo.text") & "<br/>"
            Me.LBisMandatoryInfoBottom.Text = Resource.getValue("LBisMandatoryInfo.text") & "<br/>"

            Dim name As String = Me.QuestionarioCorrente.nome
            Dim qType As COL_Questionario.Questionario.TipoQuestionario = CInt(QuestionarioCorrente.tipo)
            Dim serviceTitle As String = Me.Resource.getValue("ServiceTitle." & qType.ToString)
            If Not String.IsNullOrEmpty(serviceTitle) Then
                Me.Master.ServiceTitle = serviceTitle
                Master.ServiceTitleToolTip = String.Format(Me.Resource.getValue("ServiceTitleToolTip." & qType.ToString), name)
            Else
                Master.ServiceTitle = Resource.getValue("ServiceTitle")
                Master.ServiceTitleToolTip = String.Format(Resource.getValue("ServiceTitleToolTip"), name)
            End If
            .setButton(BTNundoOption, True)
            .setLabel(LBundoOption)
            .setButton(BTNconfirmOption, True)
            .setLabel(LBconfirmOption)
        End With
    End Sub

    Protected Sub BTNFine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNFine.Click
        HDNcurrentTime.Value = HIDtempoRimanente.Value
        HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
        If Not IsNothing(QuestionarioCorrente) Then
            If Not QuestionarioCorrente.editaRisposta AndAlso QuestionarioCorrente.durata > 0 Then
                Master.OpenDialogOnPostback = True
                Master.SetOpenDialogOnPostbackByCssClass(LTdlgconfirmsubmit.Text)
                DVconfirmSubmit.Visible = True
            Else
                SaveCompletedAnswer(sender, e)
            End If
        Else
            SaveCompletedAnswer(sender, e)
        End If
    End Sub
    Private Sub SaveCompletedAnswer(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            'Dim CurrentUserIdByInvitation As Integer = Me.

            If Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Exit Sub
            End If
            Dim isValida As Boolean = True
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida)
            If isValida Then
                Dim pageRedirect As Integer = oGestioneRisposte.checkMandatoryAnswers(QuestionarioCorrente)
                If pageRedirect = -10 Then
                    oGestioneQuest.setCampiRispostaQuestionario(True)
                    Me.QuestionarioCorrente.rispostaQuest.dataFine = Now()
                    Me.QuestionarioCorrente.rispostaQuest.indirizzoIPEnd = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                    Dim oGestioneRisposte As New GestioneRisposte



                    oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, True)



                    oGestioneQuest.CompileEndActionAdd()
                    LBConferma.Text &= Me.Resource.getValue("MSGConfermaFine")
                    LBConferma.Visible = True
                    Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    TMDurata.Enabled = False
                    DIVpanelTimer.Style.Item("display") = "none"
                    If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting AndAlso Me.QuestionarioCorrente.visualizzaRisposta Then
                        Response.Redirect(RootObject.QuestionarioStatisticheGeneraliShort + "&comp=2")
                    End If
                    TMSessione.Enabled = False
                    TMDurata.Enabled = False
                Else
                    'cosa fa quando si accorge che mancano delle risposte obbligatorie
                    oGestioneQuest.setCampiRispostaQuestionario(True)
                    'iPag = Me.QuestionarioCorrente.rispostaQuest.domandeNoRisp(0).numeroPagina - 1
                    iPag = pageRedirect - 1
                    ''IMBDopo.AlternateText = Me.Resource.getValue("IMBprimaDopo") & (iPag + 2).ToString()
                    'LkbNext.ToolTip = Me.Resource.getValue("IMBprimaDopo") & (iPag + 2).ToString()
                    'LkbNext.Text = Me.Resource.getValue("IMBprimaDopo") '& (iPag + 2).ToString()

                    ''IMBprima.AlternateText = Me.Resource.getValue("IMBprimaDopo") & (iPag).ToString()
                    'LkbBack.ToolTip = Me.Resource.getValue("IMBprimaDopo") & (iPag).ToString()
                    'LkbBack.Text = Me.Resource.getValue("IMBprimaDopo") '& (iPag).ToString()

                    LBTroppeRispostePagina.Visible = False
                    TMDurata_Tick(sender, e) 'il salvataggio viene fatto qui
                    bindDLPagine()
                    isFirstRun = True
                    Dim pages As List(Of Integer) = oGestioneRisposte.GetPageNumberOfSkippedMandatoryQuestion(QuestionarioCorrente)
                    Select Case pages.Count
                        Case 0
                            LBnoRisposta.Text = Resource.getValue("LBnoRisposta.0")
                        Case 1
                            LBnoRisposta.Text = String.Format(Resource.getValue("LBnoRisposta.1"), pages(0))
                        Case Else
                            If pages.Count > 0 Then
                                LBnoRisposta.Text = String.Format(Resource.getValue("LBnoRisposta.n"), String.Join(",", pages.ToArray()))
                            Else
                                LBnoRisposta.Text = Resource.getValue("LBnoRisposta.-1")
                            End If
                    End Select
                    LBnoRisposta.Visible = True
                End If
            Else
                LBTroppeRispostePagina.Visible = True
                HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
                bindDLPagine()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub BTNSalvaEEsci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSalvaEEsci.Click
        If Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
            CTRLerrorMessages.Visible = True
            CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            MLVquestionari.SetActiveView(Me.VIWmessaggi)
            Exit Sub
        End If
        Try
            Dim isValida As Boolean = True
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida)
            If isValida Then
                oGestioneQuest.setCampiRispostaQuestionario(True)
                Dim oGestioneRisposte As New GestioneRisposte
                oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, True)
                LBConferma.Text &= Me.Resource.getValue("MSGConfermaSalvaEdEsci")
                LBConferma.Visible = True
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                LBTroppeRispostePagina.Visible = False
            Else
                LBTroppeRispostePagina.Visible = True
                bindDLPagine()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub BTNSalvaContinua_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSalvaContinua.Click
        Try
            Dim isValida As Boolean = True
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida)
            If isValida Then

                oGestioneQuest.setCampiRispostaQuestionario(True)
                If Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                    CTRLerrorMessages.Visible = True
                    CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    Exit Sub
                Else
                    Dim oGestioneRisposte As New GestioneRisposte
                    oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False)
                    LBTroppeRispostePagina.Visible = False
                End If
            Else
                LBTroppeRispostePagina.Visible = True
            End If
            HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
            bindDLPagine()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub BTNinizia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNinizia.Click
        If (Me.QuestionarioCorrente.isPassword And Not TXBPassword.Text = Me.Invito.Password) Then
            'se la password e' sbagliata vai in errore
            LBLErrorePassword.Style.Item("display") = "block"
            BindDati()
            Exit Sub
        Else

            'Ora, quando si preme su "INIZIA",
            'SE ho un utente invitato (e verificato tramite password se richiesto)
            'ed ho ancora un utente anonimo, carico l'utente dell'invito (di piattaforma)
            If Me.Invito.ID > 0 AndAlso
                (IsNothing(UtenteCorrente) OrElse UtenteCorrente.ID = 0 OrElse UtenteCorrente.ID = idUtenteAnonimo()) Then
                CaricaUtenteInvitatoEVerificato(Me.Invito.PersonaID)

            End If


            If iPag = -1 Then
                    TMSessione.Enabled = True
                    TMSessione.Interval = RootObject.tickMassimo
                    TMSessione_Tick(sender, e)
                    If Me.QuestionarioCorrente.isPrimaRisposta Or Me.QuestionarioCorrente.editaRisposta Then
                        If RootObject.setNullDate(Me.QuestionarioCorrente.rispostaQuest.dataInizio) Is System.DBNull.Value Then
                            Me.QuestionarioCorrente.rispostaQuest.dataInizio = Now()
                            Me.QuestionarioCorrente.rispostaQuest.indirizzoIPStart = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                        End If
                        isFirstRun = True
                        'DirectCast(UPTempo.FindControl("LBTempoRimanente"), Label).Visible = LBTempoRimanenteVIWDescrizione.Visible
                        DirectCast(UPTempo.FindControl("LBTempoRimanente"), Label).Text = String.Format(LBTempoRimanente.Text, Me.QuestionarioCorrente.durata)
                        'Me.SetFocus(IMBdopo)
                        'per disattivare i timer commentare l'if seguente
                        If Me.QuestionarioCorrente.durata > 0 Then
                            LBTempoRimanente.Text = String.Format(Me.Resource.getValue("LBTempoRimanente.text"), Me.QuestionarioCorrente.durata, Me.Resource.getValue("minuti"))
                            'DIVpanelTempo.Attributes.CssStyle.Clear()
                            DIVpanelTempo_Container.Attributes.Add("class", "div_paneltempo_container time_visible")
                            'DIVpanelTempo.Style.Item("display") = "block"

                            TMDurata.Enabled = True
                            TMDurata.Interval = RootObject.autoSaveTimer
                            TMDurata_Tick(sender, e)
                        Else
                            ' salvo la risposta al questionario
                            oGestioneQuest.setCampiRispostaQuestionario(True)
                            Me.QuestionarioCorrente.rispostaQuest.dataFine = Date.MinValue.ToString
                        If oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False) = "-1" Then
                            CTRLerrorMessages.Visible = True
                            CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AnswerNotSaved"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                            MLVquestionari.SetActiveView(Me.VIWmessaggi)
                        End If
                    End If

                    End If

                    isCorrezione = False 'serve per i test di autovalutazione
                    vaiPaginaDopo()
                    oGestioneQuest.CompileStartActionAdd()
                End If

            End If
    End Sub
    Protected Sub LNBindietro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.RedirectToUrl(RootObject.QuestionariList & "?Type=" & Me.QuestionarioCorrente.tipo)
    End Sub
    Protected Sub TMSessione_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMSessione.Tick
        If isFirstRun Then
            TMSessione.Interval = RootObject.tickMassimo
            startTime = Now
            isFirstRun = False
            'If Not (MLVquestionari.ActiveViewIndex = 0) Then
            '    TMSessione.Enabled = False
            '    TMDurata.Enabled = False
            'End If
        End If
        If DateDiff("n", startTime, Now) > RootObject.vitaSessione_max Then
            TMDurata_Tick(sender, e)
            Session("isSessioneScaduta") = True
            Response.Redirect(RootObject.compileUrlUI_short)
        End If
    End Sub
    Protected Sub TMDurata_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMDurata.Tick
        Dim leaveTime As Integer = 0
        Dim previousLeaveTime As Integer = 0
        If Me.QuestionarioCorrente.isPrimaRisposta Or Me.QuestionarioCorrente.editaRisposta Then
            If Me.QuestionarioCorrente.durata > 0 Then
                If Not MLVquestionari.ActiveViewIndex = 2 Then
                    If Not Me.QuestionarioCorrente.rispostaQuest.dataInizio Is Nothing Then
                        leaveTime = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
                        HIDtempoRimanente.Value = leaveTime
                        HIDstarter.Value = 1
                        If (leaveTime * 1000) < TMDurata.Interval Then
                            If leaveTime > 0 Then
                                TMDurata.Interval = Math.Min((leaveTime * 1000), RootObject.autoSaveTimer)
                            Else
                                TMDurata.Interval = 1000
                            End If
                        End If
                        Integer.TryParse(HDNcurrentTime.Value, previousLeaveTime)
                        'If TempoRimanente < 1 Then
                        'Try
                        Dim isValida As Boolean = True
                        Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, True)
                        oGestioneQuest.setCampiRispostaQuestionario(True)
                        Me.QuestionarioCorrente.rispostaQuest.dataFine = Date.MinValue.ToString
                        If leaveTime < 1 AndAlso previousLeaveTime < 1 Then
                            CTRLerrorMessages.Visible = True
                            CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoTempo"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                            TMDurata.Enabled = False
                            TMSessione.Enabled = False
                        End If
                        If oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False) = "-1" Then
                            CTRLerrorMessages.Visible = True
                            CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AnswerNotSaved"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                            MLVquestionari.SetActiveView(Me.VIWmessaggi)
                        End If
                        'Catch ex As Exception
                        '    Response.Write(ex.Message)
                        'End Try
                        If leaveTime < 1 Then
                            Session("isNoTempo") = True
                            Response.Redirect(RootObject.compileUrlUI_short)
                        End If
                    End If
                End If
            Else
                TMDurata.Enabled = False
                leaveTime = Integer.MaxValue
                'salva comunque la risposta
                Dim isValida As Boolean = True
                Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, True)
                oGestioneQuest.setCampiRispostaQuestionario(True)
                If Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                    CTRLerrorMessages.Visible = True
                    CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                    MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    Exit Sub
                Else

                    If oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False) = "-1" Then
                        CTRLerrorMessages.Visible = True
                        CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AnswerNotSaved"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                        MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub LKBpagina_OnClientClick(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'MB: PROVO a controllare la modifica in fase di cambio pagina (se non già sottomesso, etc...)
        Try
            If Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        LBnoRisposta.Visible = False
        Dim isValida As Boolean = True
        Try
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida)
            If isValida Then
                Dim LKBpag As New LinkButton
                LKBpag = DirectCast(sender, LinkButton)
                DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Clear()
                iPag = Integer.Parse(LKBpag.ID.Substring(10)) - 1
                ''IMBDopo.AlternateText = Me.Resource.getValue("IMBprimaDopo") & (iPag + 2).ToString()
                'LkbNext.ToolTip = Me.Resource.getValue("IMBprimaDopo") & (iPag + 2).ToString()
                'LkbNext.Text = Me.Resource.getValue("IMBprimaDopo") '& (iPag + 2).ToString()

                ''IMBprima.AlternateText = Me.Resource.getValue("IMBprimaDopo") & (iPag).ToString()
                'LkbBack.ToolTip = Me.Resource.getValue("IMBprimaDopo") & (iPag).ToString()
                'LkbBack.Text = Me.Resource.getValue("IMBprimaDopo") '& (iPag).ToString()
                LBTroppeRispostePagina.Visible = False
            Else
                LBTroppeRispostePagina.Visible = True
            End If
            TMDurata_Tick(sender, e)
            bindDLPagine()
            isFirstRun = True
        Catch ex As Exception
            inviaMailErrore(ex)
        End Try
    End Sub
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
            DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Item("font-style") = "italic"
            DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Item("font-weight") = "bolder"
            DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Item("text-decoration") = "underline"
        End If
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property

    Private Sub Page_PreLoad1(sender As Object, e As EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
    Private Sub BTNconfirmExitOption_Click(sender As Object, e As EventArgs) Handles BTNconfirmExitOption.Click
        BTNSalvaEEsci_Click(sender, e)
        If Me.QuestionarioCorrente.durata > 0 Then
            HDNcurrentTime.Value = 0
            HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
        End If
    End Sub
    Private Sub BTNundoExitOption_Click(sender As Object, e As EventArgs) Handles BTNundoExitOption.Click
        If Me.QuestionarioCorrente.durata > 0 Then
            HDNcurrentTime.Value = 0
            HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
        End If
        BTNSalvaContinua_Click(sender, e)
        If Me.QuestionarioCorrente.durata > 0 Then
            HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
        End If
    End Sub

    Private Sub BTNconfirmOption_Click(sender As Object, e As EventArgs) Handles BTNconfirmOption.Click
        Master.OpenDialogOnPostback = False
        Master.ClearOpenedDialogOnPostback()
        'DVconfirmSubmit.Visible = False
        HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
        SaveCompletedAnswer(sender, e)
        HDNcurrentTime.Value = 0
        HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
    End Sub

    Private Sub BTNundoOption_Click(sender As Object, e As EventArgs) Handles BTNundoOption.Click
        Master.OpenDialogOnPostback = False
        Master.ClearOpenedDialogOnPostback()
        'DVconfirmSubmit.Visible = False
        HDNcurrentTime.Value = 0
        HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
    End Sub


    Public Sub HideDiv(ByVal Div As HtmlControl)
        Div.Attributes.Add("style", "display:none;")
    End Sub

    Private Sub UICompile__PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim value As String = Me.Resource.getValue("LkbNext.Text")

        If String.IsNullOrEmpty(value) Then
            value = "&gt;"
        End If

        LkbNext.Text = value

        value = Me.Resource.getValue("LkbNext.ToolTip")

        If String.IsNullOrEmpty(value) Then
            value = "Avanti"
        End If

        LkbNext.ToolTip = value


        value = Me.Resource.getValue("LkbBack.Text")

        If String.IsNullOrEmpty(value) Then
            value = "&gt;"
        End If

        LkbBack.Text = value

        value = Me.Resource.getValue("LkbBack.ToolTip")

        If String.IsNullOrEmpty(value) Then
            value = "Indietro"
        End If

        LkbBack.ToolTip = value
    End Sub



    ''' <summary>
    ''' Aggiunto per il salva!
    ''' Risulta NECESSARIO VERIFICARE che le risposte SALVATE siano effettivamente
    ''' quelle dell'utente corrente, per EVITARE sovrascritture o cancellazioni incongrue.
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property UserId As Integer
        Get
            Return UtenteQuestionario.ID
            'Return Me.UtenteCorrente.ID
        End Get
    End Property
End Class