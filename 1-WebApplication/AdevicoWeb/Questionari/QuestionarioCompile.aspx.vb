Imports COL_Questionario
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Questionario
Imports System.Linq
Imports lm.Comol.Core.DomainModel


Partial Public Class QuestionarioCompile
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

    Private Property QuestionnarieSaved As Boolean
        Get
            Return ViewStateOrDefault("QuestionnarieSaved", False)
        End Get
        Set(value As Boolean)
            ViewState("QuestionnarieSaved") = value
        End Set
    End Property
#Region "condivisa"


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

    Private _CurrentQuestionnaireKey As String
    Private ReadOnly Property CurrentQuestionnaireKey As String
        Get
            If String.IsNullOrWhiteSpace(_CurrentQuestionnaireKey) Then
                If Not IsNothing(QuestionarioCorrente) AndAlso QuestionarioCorrente.id > 0 Then
                    _CurrentQuestionnaireKey = "_questionnaire_key_" & QuestionarioCorrente.id.ToString()
                Else
                    _CurrentQuestionnaireKey = "_questionnaire_key_empty"
                End If
            End If
            Return _CurrentQuestionnaireKey
        End Get
    End Property

    Private Property NoTimeToComplete() As Boolean
        Get
            Dim _noTime As Boolean = False
            Boolean.TryParse(Session("NoTimeToComplete_" & CurrentQuestionnaireKey), _noTime)
            Return _noTime
        End Get
        Set(value As Boolean)
            Session("NoTimeToComplete_" & CurrentQuestionnaireKey) = value
        End Set
    End Property
    Private Property IsValidForAttempts() As Boolean
        Get
            Return ViewStateOrDefault("IsValidForAttempts", False)
        End Get
        Set(value As Boolean)
            ViewState("IsValidForAttempts") = value
        End Set
    End Property
    Private ReadOnly Property CurrentService() As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_Service) Then
                _Service = New COL_Questionario.Business.ServiceQuestionnaire(Me.CurrentContext)
            End If
            Return _Service
        End Get
    End Property
    'Private _serviceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service
    'Private ReadOnly Property ServiceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service
    '    Get
    '        If IsNothing(_serviceEduPath) Then
    '            _serviceEduPath = New lm.Comol.Modules.EduPath.BusinessLogic.Service(PageUtility.CurrentContext)
    '        End If
    '        Return _serviceEduPath
    '    End Get
    'End Property
#End Region

    'valutare se convertire queste public shared in property come startTime
    Public Shared iDom As Integer
    Public Shared _iPag As Integer
    Public Shared isCorrezione As Boolean
    Dim oPagedDataSource As New PagedDataSource
    Dim bindDone As Boolean
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneRisposte As New GestioneRisposte
    Dim oGestioneQuest As New GestioneQuestionario
    Private _SmartTagsAvailable As SmartTags
    Public Shared idCollection As New List(Of Integer)
    Private ReadOnly Property qs_linkId As String
        Get
            Return Request.QueryString("lId")
        End Get
    End Property
    Private ReadOnly Property ViewOnly As Boolean
        Get
            If Not String.IsNullOrWhiteSpace(Request.QueryString("View")) Then
                Return Request.QueryString("View").ToLower() = "true"
            Else
                Return False
            End If
        End Get
    End Property


    Private Function MustUpdateExternalServiceStatistics() As Boolean
        Dim result As Boolean = True
        ' IN TEORIA QUI SE HO CAMBIATO LA DATA DI MOTIFICA E SE HO RISPOSTO PRIMA DELLA MODIFICA DELLE IMPOSTAZIONI
        ' DOVREI CONSENTIRE IL RICALCOLO ANCHE IN BASE ALLE ATTIVITA' IMPOSTATE NEL PERCORSO FORMATIVO
        ' AL MOMENTO CONTROLLO SOLO IL VIEW ONLY
        result = Not ViewOnly

        Return result
    End Function

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
    Public ReadOnly Property displayDifficulty() As String
        Get
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
                Return "hide" 'False
            Else
                Return "show" 'True
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


    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase())
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
        MLVquestionari.SetActiveView(VIWmessaggi)
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)

        Try
            iDom = e.Item.ItemIndex
            'oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, False)
            'Dim ctrlRisposta As Control
            'If Me.QuestionarioCorrente.pagine(iPag).domande.Item(iDom).tipo = Domanda.TipoDomanda.RatingStars Then

            '    ctrlRisposta = oGestioneDomande.addDomandaRatingStarsV2(Me.QuestionarioCorrente, iPag, iDom, Not IsEditableQuestionnaire)
            'Else
            Dim ctrlRisposta As Control = oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, Not IsEditableQuestionnaire)
            'End If

            DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(ctrlRisposta)
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
                'Else 'nasconde difficolta', che e' gia' gestita da showDifficulty, ma non và una sega perchè è runat="server"!!!
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
            Dim dvPageNameBottom As HtmlControl = DLPagine.Controls(0).FindControl("DIVNomePaginaFooter")
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
    '    If IsValidForAttempts Then
    '        bindDLPagine()
    '        isFirstRun = True
    '    End If

    'End Sub
    Protected Sub LkbBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles LkbBack.Click
        If iPag > 0 Then
            Dim isValida As Boolean = True

            Dim ObbligatorieSaltate As Integer = 0
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)

            ShowMandatory(ObbligatorieSaltate)
            If ObbligatorieSaltate > 0 Then
                Return
            End If

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
        If IsValidForAttempts Then
            bindDLPagine()
            isFirstRun = True
        End If

    End Sub
    'Protected Sub IMBdopo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBdopo.Click
    '    TMDurata_Tick(sender, e)
    '    If IsValidForAttempts Then
    '        vaiPaginaDopo()
    '        isFirstRun = True
    '    End If
    'End Sub
    Protected Sub LkbNext_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles LkbNext.Click
        TMDurata_Tick(sender, e)
        If IsValidForAttempts Then
            vaiPaginaDopo()
            isFirstRun = True
        End If
    End Sub
    Protected Sub vaiPaginaDopo()
        LBnoRisposta.Visible = False
        If iPag > -2 Then
            Dim isValida As Boolean = True
            Dim ObbligatorieSaltate As Integer = 0  'Ok, SE vado avanti: CONTROLLO!

            If isCorrezione Or Not Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione Then
                Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)
            End If

            ShowMandatory(ObbligatorieSaltate)
            If ObbligatorieSaltate > 0 Then
                Return
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
                                BTNSalvaAutovalutazione.Visible = True
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
        If IsValidForAttempts Then
            vaiPaginaDopo()
        End If
    End Sub
    Private Sub BTNRestartAutoEval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNRestartAutoEval.Click
        Response.Redirect(Request.RawUrl)

    End Sub

#End Region

    Protected Sub bindDLPagine()
        Dim pages As List(Of QuestionarioPagina)
        Me.SetInternazionalizzazione()
        Try
            pages = Me.QuestionarioCorrente.pagine
            oPagedDataSource.DataSource = pages
        Catch ex As Exception
            pages = New List(Of QuestionarioPagina)
            inviaMailErrore(ex)
        End Try

        oPagedDataSource.AllowPaging = True
        oPagedDataSource.PageSize = 1
        oPagedDataSource.CurrentPageIndex = iPag
        Dim counter As Int16 = 0
        Try
            If Me.QuestionarioCorrente.pagine.Count > 1 And iPag + 1 < Me.QuestionarioCorrente.pagine.Count Then
                LkbNext.Visible = True
                If iPag < 1 Then
                    'IMBprima.Visible = False
                    LkbBack.Visible = False
                Else
                    'IMBprima.Visible = True
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
                BTNFine.Visible = True
                LBAvvisoFine.Visible = True

            End If
        Catch ex As Exception
            inviaMailErrore(ex)
        End Try
        If iPag > -1 Then

            Try
                DLPagine.DataSource = oPagedDataSource
                DLPagine.DataBind()
            Catch ex As Exception
                inviaMailErrore(ex)
            End Try

            Dim showMandatoryInfo As Boolean = pages.Where(Function(p) p.domande.Where(Function(d) d.isObbligatoria).Any).Any()
            Me.LBisMandatoryInfoBottom.Visible = showMandatoryInfo
            LBisMandatoryInfoTop.Visible = showMandatoryInfo


            If Not PHnumeroPagina.Controls.Count = (Me.QuestionarioCorrente.pagine.Count) Or PHnumeroPagina.Controls.Count = 0 Then
                'Dim startCounter As Integer
                'startCounter = Math.Max(1, PHnumeroPagina.Controls.Count)
                PHnumeroPagina.Controls.Clear()
                For counter = 1 To Me.QuestionarioCorrente.pagine.Count
                    Dim LKBpagina As New LinkButton
                    LKBpagina.Text = "&nbsp;" + counter.ToString + "&nbsp;"
                    LKBpagina.ID = "LKBpagina_" + counter.ToString
                    LKBpagina.Attributes.Add("click", "LKBpagina_OnClientClick()")
                    AddHandler LKBpagina.Click, AddressOf LKBpagina_OnClientClick
                    LKBpagina.ToolTip = "Pagina " + counter.ToString
                    'LKBpagina.Style.Clear()
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Request.QueryString("View")) AndAlso Request.QueryString("View").ToLower() = "true" Then
            Dim redirectUrl As String = Request.Url.AbsoluteUri.Replace("QuestionarioCompile", "QuestionarioStat").Replace("&View=true", "&mode=3")
            Response.Redirect(redirectUrl)
        End If

        If Page.IsPostBack Then

            If Not QuestionarioCorrente.idDestinatario_Persona <= 0 AndAlso QuestionarioCorrente.idDestinatario_Persona <> QsDestUserId Then
                Dim responseUrl As String = Request.QueryString("BackUrl")
                If String.IsNullOrEmpty(responseUrl) Then
                    responseUrl = String.Format("/Modules/common/RedirectToDefaultModule.aspx?IdCommunity={0}", Me.ComunitaCorrenteID)
                End If
                Response.Redirect(Me.BaseUrl & responseUrl)
            End If
            bindDLPagine()
        Else
            isFirstRun = True
            'Me.QsDestUserId = QuestionarioCorrente.idDestinatario_Persona   del
            'Me.QsCompileId = QuestionarioCorrente.ownerId
        End If
        'PHHeader.Controls.Add(Page.LoadControl(RootObject.ucHeaderCompile))
    End Sub
    Public Function gestioneVIWMessaggi() As Boolean
        CTRLerrorMessages.Visible = False
        If Not Me.QuestionarioCorrente Is Nothing Then
            If NoTimeToComplete Then
                'If Session("isNoTempo") = True Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoTempo"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                NoTimeToComplete = False
                'Session.Remove("isNoTempo")
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf Session("isSessioneScaduta") = True Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGSessioneScaduta"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                TMSessione.Enabled = False
                Session.Remove("isSessioneScaduta")
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            ElseIf isAnonymousCompiler Then
                'ElseIf Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then
                Me.QuestionarioCorrente.rispostaQuest.idPersona = idUtenteAnonimo()
            ElseIf Me.EncryptedQueryString("idu", SecretKeyUtil.EncType.Questionario) <> "" Then
                Me.QuestionarioCorrente.rispostaQuest.idPersona = Me.Invito.PersonaID
            ElseIf Me.QuestionarioCorrente.forUtentiComunita OrElse DALUtenteInvitato.isInvited(QuestionarioCorrente.id, UtenteCorrente.ID) OrElse Me.QuestionarioCorrente.forUtentiPortale Then  'senza forutentiportale se ci sono errori nel salvataggio dei parametri non viene permessa la compilazione
                Me.QuestionarioCorrente.rispostaQuest.idPersona = Me.UtenteCorrente.ID
            Else
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoPermessi"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            End If
            Select Case QuestionarioCorrente.tipo
                Case QuestionnaireType.RandomMultipleAttempts
                    If Not CurrentService.IsValidAttempts(QuestionarioCorrente.id, CurrentContext.UserContext.CurrentUserID, 0, -1) Then
                        ' LBErrore.Text &= Me.Resource.getValue("MultipleAttempts")
                        IsValidForAttempts = False
                        CTRLerrorMessages.Visible = True
                        CTRLerrorMessages.InitializeControl(Resource.getValue("MultipleAttempts"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                        Me.MLVquestionari.SetActiveView(VIWmessaggi)
                        Return False
                    Else
                        IsValidForAttempts = True
                    End If
                Case Else
                    IsValidForAttempts = True
            End Select
            If Me.Invito.ID = -1 Then
                'verifica che l'invito sia valido, se c'e'
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoInvito"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Return False
            End If
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
            ElseIf Me.QuestionarioCorrente.isPrimaRisposta AndAlso Not (DateDiff(DateInterval.Second, DateTime.Now, DateTime.Parse(Me.QuestionarioCorrente.dataFine)) > 0 AndAlso DateDiff(DateInterval.Second, DateTime.Now, DateTime.Parse(Me.QuestionarioCorrente.dataInizio)) < 0) Then
                'si visualizzano i risultati di meeting e sondaggi anche se il tempo per compilare e' scaduto
                If (Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting) AndAlso DateDiff(DateInterval.Second, DateTime.Now, DateTime.Parse(Me.QuestionarioCorrente.dataInizio)) < 0 Then
                    RedirectToUrl(RootObject.QuestionarioStatistiche & "?mode=2" & "&comp=1" & "&idq=" & Me.QuestionarioCorrente.id) ' & "&comp=")
                End If
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
                If Not ((Me.QuestionarioCorrente.tipo = QuestionnaireType.Random OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.AutoEvaluation) AndAlso Me.QuestionarioCorrente.librerieQuestionario.Count > 0) Then
                    CTRLerrorMessages.Visible = True
                    CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoDomande"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                    Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    Return False
                End If
            End If
            If Not Me.QuestionarioCorrente.rispostaQuest.dataFine Is Nothing Then
                'nota:deve essere l'ultimo controllo, altrimenti saltano i controlli successivi quando c'e' una risposta ma la modifica e' attiva
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

                    'ElseIf Not Me.QuestionarioCorrente.durata = 0 Then
                    '    '..se il questionario e' a tempo non si puo' ricompilare
                    '    LBgiaCompilato.Visible = True
                    '    Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                End If
            End If
            Return True
        Else
            CTRLerrorMessages.Visible = True
            CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoPermessi"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            MLVquestionari.SetActiveView(Me.VIWmessaggi)
            Return False
        End If
    End Function


    Protected Sub setCampiVisibili()
        If Not String.IsNullOrWhiteSpace(Me.QuestionarioCorrente.descrizione) Then
            LNBdescrizione.Visible = (QuestionarioCorrente.durata < 1 AndAlso (QuestionarioCorrente.tipo <> Questionario.TipoQuestionario.RandomRepeat OrElse QuestionarioCorrente.tipo = QuestionarioCorrente.TipoQuestionario.Autovalutazione)) AndAlso iPag > 0
        Else
            LNBdescrizione.Visible = Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione AndAlso iPag > 0
        End If
        'BTNSalvaContinua.Visible = Not Me.QuestionarioCorrente.isReadOnly
        If Me.QuestionarioCorrente.durata = 0 Then
            LBdurata.Visible = False
        End If
        If iPag = Me.QuestionarioCorrente.pagine.Count - 1 Then
            LBAvvisoSalva.Visible = PageUtility.SystemSettings.Presenter.AllowSaveAndContinueQuestionnaire
            LTsaveAndExit.Visible = PageUtility.SystemSettings.Presenter.AllowSaveAndContinueQuestionnaire
            BTNSalvaContinua.Visible = PageUtility.SystemSettings.Presenter.AllowSaveBetweenPageQuestionnaire
        End If

        '' ADDED FOR MULTIPLE ITEMS
        'If (Not (Me.QuestionarioCorrente.editaRisposta OrElse Me.QuestionarioCorrente.isPrimaRisposta)) 
        '   OrElse (Not Me.QuestionarioCorrente.isPrimaRisposta 
        '           AndAlso (Me.QuestionarioCorrente.durata > 0 
        '           AndAlso (String.IsNullOrEmpty(Me.QuestionarioCorrente.rispostaQuest.dataInizio) 
        '           OrElse (DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, Now) > RootObject.maxOvertimeSalvataggio)))) Then

        Dim notEdit As Boolean = Not (Me.QuestionarioCorrente.editaRisposta OrElse Me.QuestionarioCorrente.isPrimaRisposta)

        Dim byDate As Boolean = False

        If Me.QuestionarioCorrente.durata > 0 Then

            If Not (String.IsNullOrEmpty(Me.QuestionarioCorrente.rispostaQuest.dataInizio)) Then

                'Dim byOverTime As Boolean = DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now) > RootObject.maxOvertimeSalvataggio
                Dim byScadenza As Boolean = DateDiff("n", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now) < QuestionarioCorrente.durata '* 60

                byDate = Not byScadenza 'orElse byOverTime 

            End If
        End If

        If notEdit OrElse byDate Then
            '((Me.QuestionarioCorrente.durata > 0 AndAlso (String.IsNullOrEmpty(Me.QuestionarioCorrente.rispostaQuest.dataInizio) OrElse (DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, Now) > RootObject.maxOvertimeSalvataggio)))) Then
            BTNSalvaContinua.Visible = False
            BTNSalvaEEsci.Visible = False
            Dim sVisible As Boolean = Me.QuestionarioCorrente.isPrimaRisposta AndAlso QuestionarioCorrente.tipo <> QuestionnaireType.AutoEvaluation AndAlso (QuestionarioCorrente.pagine.Count = iPag OrElse QuestionarioCorrente.pagine.Count = iPag + 1)
            BTNFine.Visible = sVisible
            LBAvvisoFine.Visible = sVisible
            LNBsalvaContinua.Visible = False
            LNBsalvaEsci.Visible = False
            LNBFinito.Visible = False

            Dim idUser As Integer = PageUtility.CurrentContext.UserContext.CurrentUserID
            If MustUpdateExternalServiceStatistics() AndAlso Not PageUtility.CurrentContext.UserContext.isAnonymous AndAlso Not String.IsNullOrEmpty(qs_linkId) AndAlso Not SubActivityCompleted(qs_linkId, idUser) Then
                Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, QuestionarioCorrente.id, idUser, QuestionarioCorrente.rispostaQuest.id)
                'If IsNothing(calc) Then
                '    calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = QuestionarioCorrente.id, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                'End If
                If Not IsNothing(calc) Then
                    executedAction(qs_linkId, calc.isStarted, calc.isPassed, calc.Completion, calc.isCompleted, calc.Mark, UtenteCorrente.ID)
                End If
            End If

        End If

        If Me.isUtenteAnonimo And Me.QuestionarioCorrente.rispostaQuest.idUtenteInvitato > 0 Then
            PNLIndietro.Visible = False
            PNLmenu.Visible = False
            BTNSalvaContinua.Visible = False
            BTNSalvaEEsci.Visible = False
        End If


        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            If Page.IsPostBack = False Then
                Dim items As List(Of DifficultyLevel) = CurrentService.GetQuestionnaireLibrariesDifficulty(QuestionarioCorrente.id)
                BTNIniziaDifficile.Visible = items.Contains(DifficultyLevel.high)
                BTNIniziaFacile.Visible = items.Contains(DifficultyLevel.easy)
                BTNIniziaMedio.Visible = items.Contains(DifficultyLevel.medium)
                BTNIniziaMisto.Visible = (items.Count > 1)
            End If

            BTNinizia.Visible = False

            BTNSalvaContinua.Visible = False
            LBAvvisoSalva.Visible = False
            LTsaveAndExit.Visible = False
            BTNDopo.Visible = True
            LBAvvisoFine.Visible = False
            BTNFine.Visible = False
            LNBannulla.Visible = False
            BTNSalvaEEsci.Visible = PageUtility.SystemSettings.Presenter.AllowSaveAndContinueQuestionnaire
            BTNSalvaEEsci.Text = Resource.getValue("Autoevaluate.Save.text")
            BTNSalvaEEsci.ToolTip = Resource.getValue("Autoevaluate.Save.Tooltip")
            LBAvvisoSalva.Text = Resource.getValue("Autoevaluate.Save")
        ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            DIVNumeriPagina.Visible = False
            DIVSalvaQuestionario.Visible = False
        End If
        If iPag < 0 Then
            DIVpanelTimer.Style.Item("display") = "none"
        Else
            DIVpanelTimer.Style.Item("display") = "block"
        End If

        If Not Me.ComunitaCorrenteID = 0 Then
            LNBTornaHome.Visible = False
        End If
        LBAvvisoSalva.Visible = PageUtility.SystemSettings.Presenter.AllowSaveAndContinueQuestionnaire AndAlso (BTNSalvaContinua.Visible OrElse BTNSalvaEEsci.Visible)
    End Sub
    Public Overrides Sub BindDati()
        Dim isValidPage As Boolean = False
        If Page.IsPostBack = False Then
            BTNSalvaContinua.Visible = PageUtility.SystemSettings.Presenter.AllowSaveBetweenPageQuestionnaire
        End If
        isValidPage = gestioneVIWMessaggi()
        If isValidPage Then
            Dim currentType As QuestionnaireType = Me.QuestionarioCorrente.tipo
            If (currentType = QuestionnaireType.Random OrElse (currentType = QuestionnaireType.RandomMultipleAttempts AndAlso qs_randomQuestId = 0)) AndAlso Me.QuestionarioCorrente.idFiglio = 0 AndAlso Me.QuestionarioCorrente.pagine.Count = 0 Then
                Dim oGestioneQuestionario As New GestioneQuestionario
                oGestioneQuestionario.generaQuestionarioRandomDestinatario(False, currentType)
            End If
            'la descrizione deve apparire sempre se: c'e' una descrizione, e' a tempo ed e' la prima compilazione, e' di autovalutazione
            If Not Me.QuestionarioCorrente.descrizione.TrimStart = "" Or (Me.QuestionarioCorrente.durata > 0 And Me.QuestionarioCorrente.isPrimaRisposta) Or Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
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
                'setLabelTitolo()
                Dim ObbligatorieSaltate As Integer = 0
                Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, True, ObbligatorieSaltate)
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
                If Page.IsPostBack = False Then
                    oGestioneQuest.CompileStartActionAdd()
                End If
            End If
            If Me.QuestionarioCorrente.descrizione.TrimStart = "" Then
                LNBdescrizione.Visible = False
            End If
        End If

        If QuestionarioCorrente.pagine.Count < 2 Then
            PHnumeroPagina.Visible = False
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
                DVconfirmSubmit.Visible = True 'Modificato MB
                DVconfirmExit.Visible = True
            End If
        End If
    End Sub

    Private Sub setLabelTitolo()
        If LBname.Text = String.Empty Then
            Dim name As String = Me.QuestionarioCorrente.nome
            Dim qType As COL_Questionario.Questionario.TipoQuestionario = CInt(QuestionarioCorrente.tipo)
            Dim serviceTitle As String = Me.Resource.getValue("ServiceTitle." & qType.ToString)
            If Not String.IsNullOrEmpty(serviceTitle) Then
                Master.ServiceTitle = serviceTitle
                Master.ServiceTitleToolTip = String.Format(Me.Resource.getValue("ServiceTitleToolTip." & qType.ToString), name)
            Else
                Master.ServiceTitle = Resource.getValue("ServiceTitle")
                Master.ServiceTitleToolTip = String.Format(Resource.getValue("ServiceTitleToolTip"), name)
            End If
            LBname.Text = name
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        If qs_ownerTypeId = OwnerType_enum.None Then
            If Not qs_questId < 1 Then
                'se il questionario con id in querystring non e' della comunita' corrente ritorna false
                Dim oQuest As New Questionario
                oQuest = DALQuestionario.readQuestionarioByPersona(PageUtility.CurrentContext, True, qs_questId, LinguaID, UtenteCorrente.ID, 0, , , qs_randomQuestId)
                'se il quest e' della com corrente...
                If DALQuestionarioGruppo.ComunitaByGruppo(oQuest.idGruppo) = ComunitaCorrenteID Then
                    '..e ho i permessi necessari nella comunita'
                    If (MyBase.Servizio.Admin Or MyBase.Servizio.Compila) Then
                        'metto il quest in sessione e vado avanti
                        Me.QuestionarioCorrente = oQuest
                        Return True
                    End If
                    'altrimenti nego i permessi
                    Return False
                Else
                    'altrimenti nego i permessi
                    Return False
                End If
            Else
                Return (MyBase.Servizio.Admin Or MyBase.Servizio.Compila)
            End If
        Else
            'If AllowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Play, UtenteCorrente.ID, 0, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_ownerId, COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex), Nothing) Then
            Dim oDestinationObject As New lm.Comol.Core.DomainModel.ModuleObject()
            oDestinationObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_questId, COL_BusinessLogic_v2.UCServices.Services_Questionario.ObjectType.Questionario, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)
            Dim oSourceObject As New lm.Comol.Core.DomainModel.ModuleObject()
            oSourceObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_ownerId, qs_ownerTypeId, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
            If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Play, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                'If RootObject.RequestAuthorizationToOwner(CurrentContext, qs_ownerTypeId, Me.UtenteCorrente.ID, qs_questTypeId, RootObject.EduPath_Permission.Compile, Me.PageUtility.CurrentRoleID, qs_ownerId, ) Then
                If qs_questId < 1 Then
                    Return False
                Else
                    Dim oQuest As New Questionario
                    oQuest = DALQuestionario.readQuestionarioByPersona(PageUtility.CurrentContext, True, qs_questId, LinguaID, UtenteCorrente.ID, 0, , , qs_randomQuestId)
                    'If oQuest.ownerType = qs_ownerTypeId AndAlso oQuest.ownerId = qs_ownerId Then
                    Me.QuestionarioCorrente = oQuest
                    Return True
                    'Else : Return False
                End If
            End If
        End If
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
        SetCultureSettings()
        With Me.Resource
            '.setLabel(LBErrore)
            .setLabel(LBdurata)
            .setLabel(LBpagina)
            '.setLabel(LBTempoRimanente)
            .setLabel(LBAvvisoFine)
            .setLabel(LBAvvisoSalva)
            .setLabel(LBTroppeRispostePagina)
            .setLabel(LBnoRisposta)

            .setButton(BTNFine, False, False, False, False)
            If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting AndAlso Me.QuestionarioCorrente.visualizzaRisposta Then
                BTNFine.Text = BTNFine.Text & .getValue("MSGeRisultati")
            End If
            .setButton(BTNSalvaEEsci, False, False, False, False)
            .setButton(BTNSalvaContinua, False, False, IIf(QuestionarioCorrente.durata = 0, False, True), False)
            If Me.QuestionarioCorrente.isPrimaRisposta Then
                .setButton(BTNinizia, True, False, False, False)
            ElseIf Me.QuestionarioCorrente.editaRisposta Then
                .setButtonByValue(BTNinizia, "edit", True)
            ElseIf Me.QuestionarioCorrente.visualizzaCorrezione Then
                .setButtonByValue(BTNinizia, "corrette", True)
            ElseIf Me.QuestionarioCorrente.visualizzaRisposta Then
                .setButtonByValue(BTNinizia, "view", True)
            End If

            .setButton(BTNIniziaDifficile, False, False, False, False)
            .setButton(BTNIniziaFacile, False, False, False, False)
            .setButton(BTNIniziaMedio, False, False, False, False)
            .setButton(BTNIniziaMisto, False, False, False, False)
            .setButton(BTNDopo, False, False, False, False)
            .setButton(BTNRestartAutoEval, False, False, False, False)
            .setButton(BTNSalvaAutovalutazione, False, False, False, False)
            .setLinkButton(LNBannulla, False, False)
            DVundoExit.Visible = False
            If LNBannulla.CssClass.Contains(LTopenUndoDialogCssClass.Text) Then
                LNBannulla.CssClass = Replace(LNBannulla.CssClass, " " & LTopenUndoDialogCssClass.Text, "")
            End If
            If IsNothing(QuestionarioCorrente) OrElse QuestionarioCorrente.id < 1 Then
                .setLinkButton(LNBannulla, False, False)
            Else
                Select Case QuestionarioCorrente.tipo
                    Case QuestionnaireType.RandomMultipleAttempts
                        If IsEditableQuestionnaire Then
                            If Not LNBannulla.CssClass.Contains(LTopenUndoDialogCssClass.Text) Then
                                LNBannulla.CssClass &= " " & LTopenUndoDialogCssClass.Text
                            End If

                            DVundoExit.Visible = True
                            LBundoActionMessage.Text = Resource.getValue("LeaveQuestionnaire.Undo_RandomMultipleAttempts")

                            .setButton(BTNundoLeaveQuestionnaireOption, True)
                            .setLabel(LBundoLeaveQuestionnaireOption)
                            .setButtonByValue(BTNconfirmLeaveQuestionnaireOption, "LeaveQuestionnaire.Undo_RandomMultipleAttempts", True)
                            .setLabel_To_Value(LBconfirmLeaveQuestionnaireOption, "LeaveQuestionnaire.LBconfirmLeaveQuestionnaireOption.Undo_RandomMultipleAttempts")
                        End If
                    Case Else
                        If Not QuestionarioCorrente.editaRisposta AndAlso QuestionnarieSaved Then
                            .setLinkButtonToValue(LNBannulla, "EditAnswerNotAvailable", False, True, , True)
                            If Not LNBannulla.CssClass.Contains(LTopenUndoDialogCssClass.Text) Then
                                LNBannulla.CssClass &= " " & LTopenUndoDialogCssClass.Text
                            End If
                            DVundoExit.Visible = True
                            LBundoActionMessage.Text = Resource.getValue("LeaveQuestionnaire.EditAnswerNotAvailable")

                            .setButton(BTNundoLeaveQuestionnaireOption, True)
                            .setLabel(LBundoLeaveQuestionnaireOption)
                            .setButtonByValue(BTNconfirmLeaveQuestionnaireOption, "LeaveQuestionnaire.EditAnswerNotAvailable", True)
                            .setLabel_To_Value(LBconfirmLeaveQuestionnaireOption, "LeaveQuestionnaire.LBconfirmLeaveQuestionnaireOption.EditAnswerNotAvailable")
                        End If
                End Select
            End If

            .setLinkButton(LNBFinito, False, False)
            .setLinkButton(LNBsalvaEsci, False, False)
            .setLinkButton(LNBsalvaContinua, False, False)
            .setLinkButton(LNBdescrizione, False, False)
            .setLinkButton(LNBindietro, False, False)

            HIDmessaggio.Value = Me.UtenteCorrente.Cognome & " " & Me.UtenteCorrente.Nome & ", " & String.Format(Me.Resource.getValue("LBTempoRimanente.text"), " {secondi} ", Me.Resource.getValue("secondi"))
            If String.IsNullOrWhiteSpace(qs_ownerId) OrElse qs_ownerId = "0" Then
                .setLinkButton(LNBTornaLista, False, False)
            Else
                .setLinkButtonToValue(LNBTornaLista, "EduPath", False, False)
            End If

            .setLinkButton(LNBTornaHome, False, False)
            .setHyperLink(HYPnewAttempt, True, True)
            Me.LBisMandatoryInfoTop.Text = Resource.getValue("LBisMandatoryInfo.text") & "<br/>"
            Me.LBisMandatoryInfoBottom.Text = Resource.getValue("LBisMandatoryInfo.text")



            .setButton(BTNundoOption, True)
            .setLabel(LBundoOption)
            .setButton(BTNconfirmOption, True)
            .setLabel(LBconfirmOption)
        End With
        setLabelTitolo()
    End Sub
    Protected Sub BTNFine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNFine.Click


        If Me.QuestionarioCorrente.durata <= 0 Then
            SalvaEdEsci()

        Else


            '
            'BTNFine_Original(sender, e)

            'Modificato tempo
            HDNcurrentTime.Value = HIDtempoRimanente.Value
            Dim tempoRimanente As Long = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
            HIDtempoRimanente.Value = tempoRimanente
            If Not IsNothing(QuestionarioCorrente) Then
                'If Not QuestionarioCorrente.editaRisposta AndAlso QuestionarioCorrente.durata > 0 Then
                If QuestionarioCorrente.editaRisposta _
                    OrElse (QuestionarioCorrente.durata > 0 AndAlso tempoRimanente > 0) Then

                    SaveCompletedAnswer(sender, e)
                Else
                    'tempo scaduto o non edita...
                    Master.OpenDialogOnPostback = True
                    Master.SetOpenDialogOnPostbackByCssClass(LTdlgconfirmsubmit.Text)
                    DVconfirmSubmit.Visible = True

                End If
            Else
                SaveCompletedAnswer(sender, e)
            End If
        End If
    End Sub

    'ORIGINALE:
    Protected Sub BTNFine_Original(ByVal sender As Object, ByVal e As System.EventArgs)
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
            If QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                Dim isValidAttempt As Boolean = IsValidForAttempts
                If Not isValidAttempt Then
                    isValidAttempt = CurrentService.IsValidAttempts(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id)
                End If
                If Not isValidAttempt Then
                    IsValidForAttempts = isValidAttempt
                    DisplayInvalidAttempt()
                    Exit Sub
                End If
            ElseIf Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Exit Sub
            End If
            Dim allowSaveByTime As Boolean = True
            If QuestionarioCorrente.durata > 0 Then
                Dim previousLeaveTime As Integer = 0
                Dim leaveTime As Integer = 0
                Integer.TryParse(HIDtempoRimanente.Value, leaveTime)
                Integer.TryParse(HDNcurrentTime.Value, previousLeaveTime)
                allowSaveByTime = leaveTime > 0 OrElse previousLeaveTime > 0
                If Not allowSaveByTime Then
                    CTRLerrorMessages.Visible = True
                    CTRLerrorMessages.InitializeControl(Resource.getValue("MSGNoTempo"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                    MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    TMDurata.Enabled = False
                    TMSessione.Enabled = False
                End If
            End If

            Dim isValida As Boolean = True
            Dim ObbligatorieSaltate As Integer = 0
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)

            If isValida Then
                Dim pageRedirect As Integer = oGestioneRisposte.checkMandatoryAnswers(QuestionarioCorrente)
                If pageRedirect = -10 Then

                    oGestioneQuest.setCampiRispostaQuestionario(False)
                    Me.QuestionarioCorrente.rispostaQuest.dataFine = DateTime.Now()
                    Me.QuestionarioCorrente.rispostaQuest.indirizzoIPEnd = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                    Dim oGestioneRisposte As New GestioneRisposte
                    oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, True)
                    QuestionnarieSaved = True
                    oGestioneQuest.CompileEndActionAdd()
                    LBConferma.Text &= Me.Resource.getValue("MSGConfermaFine")
                    LBConferma.Visible = True
                    Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                    TMDurata.Enabled = False
                    DIVpanelTimer.Style.Item("display") = "none"
                    Select Case qs_ownerTypeId
                        Case OwnerType_enum.EduPathSubActivity, OwnerType_enum.EduPathActivity
                            If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                                Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, QuestionarioCorrente.id, UtenteCorrente.ID, QuestionarioCorrente.rispostaQuest.id)
                                If IsNothing(calc) Then
                                    calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = QuestionarioCorrente.id, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                                End If

                                If Not calc.isPassed Then
                                    calc.isCompleted = False
                                End If

                                executedAction(qs_linkId, calc.isStarted, calc.isPassed, calc.Completion, calc.isCompleted, calc.Mark, UtenteCorrente.ID)
                                If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                                    PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                                ElseIf (Me.QuestionarioCorrente.ownerId > 0) Then
                                    Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                                End If
                            Else
                                executedAction(qs_linkId, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), True, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio, UtenteCorrente.ID)
                                If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                                    PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                                Else
                                    Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                                End If
                            End If
                            '  executedAction(qs_linkId, UtenteCorrente.ID, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), True, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio)

                            'Case OwnerType_enum.EduPathActivity
                            '    executedAction(qs_linkId, UtenteCorrente.ID, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), True, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio)
                            '    Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                        Case Else
                            If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting AndAlso Me.QuestionarioCorrente.visualizzaRisposta Then
                                Me.RedirectToUrl(RootObject.QuestionarioStatisticheGenerali + "&comp=1")
                            ElseIf Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                                Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, QuestionarioCorrente.id, UtenteCorrente.ID, QuestionarioCorrente.rispostaQuest.id)
                                If IsNothing(calc) Then
                                    calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = QuestionarioCorrente.id, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                                End If
                                If Not calc.isCompleted Then
                                    Dim items As List(Of LazyUserResponse) = DALQuestionario.GetQuestionnaireAttempts(PageUtility.CurrentContext, QuestionarioCorrente.id, PageUtility.CurrentUser.ID, Invito.ID)
                                    HYPnewAttempt.Visible = False
                                    If items.Count >= QuestionarioCorrente.MaxAttempts Then
                                        LBConferma.Text = "<h2>" & Me.Resource.getValue("RandomMultipleAttempts.MaxAttempts") & "</h2>"
                                    Else
                                        LBConferma.Text = "<h2>" & Me.Resource.getValue("RandomMultipleAttempts.MustRepeat") & "</h2>"
                                        HYPnewAttempt.Visible = True
                                        HYPnewAttempt.NavigateUrl = Me.EncryptedUrl(RootObject.compileUrl, "idq=" & QuestionarioCorrente.id & "&type=" & QuestionarioCorrente.tipo & "&idrQ=0", SecretKeyUtil.EncType.Questionario)
                                    End If

                                    LBConferma.Visible = True
                                End If
                            End If
                    End Select
                    TMSessione.Enabled = False
                    TMDurata.Enabled = False
                Else
                    'cosa fa quando si accorge che mancano delle risposte obbligatorie
                    oGestioneQuest.setCampiRispostaQuestionario(True)
                    iPag = pageRedirect - 1
                    'LkbNext.ToolTip = Me.Resource.getValue("LkbNext.ToolTip") & (iPag + 2).ToString()
                    'LkbNext.Text = Me.Resource.getValue("LkbNext.Text") '& (iPag + 2).ToString()

                    ''IMBprima.AlternateText = Me.Resource.getValue("IMBprimaDopo") & (iPag).ToString()
                    'LkbBack.ToolTip = Me.Resource.getValue("LkbBack.ToolTip") & (iPag).ToString()
                    'LkbBack.Text = Me.Resource.getValue("LkbBack.Text") '& (iPag).ToString()

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
            'Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub LNBFinito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBFinito.Click
        BTNFine_Click(sender, e)
    End Sub


    Private Sub SalvaEdEsci()
        Try
            If QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                Dim isValidAttempt As Boolean = IsValidForAttempts
                If Not isValidAttempt Then
                    isValidAttempt = CurrentService.IsValidAttempts(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id)
                End If
                If Not isValidAttempt Then
                    IsValidForAttempts = isValidAttempt
                    DisplayInvalidAttempt()
                    Exit Sub
                End If
            ElseIf Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Exit Sub
            End If
            Dim isValida As Boolean = True
            Dim ObbligatorieSaltate As Integer = 0
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)


            ShowMandatory(ObbligatorieSaltate)
            If ObbligatorieSaltate > 0 Then
                Return
            End If

            If isValida Then

                oGestioneQuest.setCampiRispostaQuestionario(False)

                If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                    'se il questionario e' di autovalutazione deve essere salvato il questionario figlio al quale associare le domande
                    If Me.QuestionarioCorrente.idFiglio = 0 Then
                        DALQuestionario.InsertRandomDestinatario(Me.QuestionarioCorrente, Me.UtenteCorrente.ID)
                    End If

                    For Each oDomanda As Domanda In Me.QuestionarioCorrente.domande
                        DALDomande.connectQuestionToSurvey(oDomanda, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idFiglio)
                    Next
                    Me.QuestionarioCorrente.rispostaQuest.dataFine = DateTime.Now
                    Me.QuestionarioCorrente.rispostaQuest.indirizzoIPEnd = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                    LBConferma.Text &= Me.Resource.getValue("MSGConfermaSalvaAutovalutazione")
                Else
                    LBConferma.Text &= Me.Resource.getValue("MSGConfermaSalvaEdEsci")
                End If
                Me.QuestionarioCorrente.rispostaQuest.idQuestionarioRandom = Me.QuestionarioCorrente.idFiglio
                oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, True)
                QuestionnarieSaved = True
                LBConferma.Visible = True
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                LBTroppeRispostePagina.Visible = False
                Select Case qs_ownerTypeId
                    Case OwnerType_enum.EduPathSubActivity, OwnerType_enum.EduPathActivity
                        If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then

                            Dim idRandom As Integer = Me.QuestionarioCorrente.idFiglio 'QuestionarioCorrente.id
                            Dim idQuest As Integer = Me.QuestionarioCorrente.id
                            Dim idAnswer As Integer = Me.QuestionarioCorrente.rispostaQuest.id

                            'Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)

                            'If (idAnswer <= 0) Then
                            '    calc = DALQuestionario.CalculateComplationRandomId(PageUtility.CurrentContext, idQuest, UtenteCorrente.ID, idRandom)
                            'Else
                            '    calc = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, idQuest, UtenteCorrente.ID, idAnswer)
                            'End If


                            'VERIFICARE IL CALCOLO DEL PUNTEGGIO!!!!
                            Dim calc As dtoItemEvaluation(Of Long) = Nothing
                            calc = CurrentService.CalculateComplation(idQuest, UtenteCorrente.ID, idAnswer)

                            ''Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, QuestionarioCorrente.id, UtenteCorrente.ID, QuestionarioCorrente.rispostaQuest.id)
                            'If IsNothing(calc) Then
                            '    'calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = QuestionarioCorrente.id, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                            '    calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = idQuest, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                            'End If
                            If Not calc.isPassed Then
                                calc.isCompleted = False
                            End If

                            executedAction(
                                qs_linkId,
                                calc.isStarted,
                                calc.isPassed,
                                calc.Completion,
                                calc.isCompleted,
                                calc.Mark,
                                UtenteCorrente.ID)

                            If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                                PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                            Else
                                Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                            End If
                        Else
                            Dim completed As Boolean = True

                            executedAction(
                                qs_linkId,
                                True,
                                True,
                                CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count),
                                True,
                                QuestionarioCorrente.rispostaQuest.oStatistica.punteggio,
                                UtenteCorrente.ID)

                            If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                                PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                            Else
                                Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                            End If
                        End If

                    Case Else
                        If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting AndAlso Me.QuestionarioCorrente.visualizzaRisposta Then
                            Me.RedirectToUrl(RootObject.QuestionarioStatisticheGenerali + "&comp=1")
                        ElseIf Me.QuestionarioCorrente.tipo = QuestionnaireType.AutoEvaluation Then
                            RedirectToUrl(RootObject.QuestionariList & "?type=5&idq=" & Me.QuestionarioCorrente.id.ToString)
                        End If
                End Select
            Else
                LBTroppeRispostePagina.Visible = True
                bindDLPagine()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub



    Protected Sub BTNSalvaEEsci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSalvaEEsci.Click
        SalvaEdEsci()
    End Sub
    Protected Sub LNBSalvaEsci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBsalvaEsci.Click
        BTNSalvaEEsci_Click(sender, e)
    End Sub
    Protected Sub BTNSalvaContinua_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSalvaContinua.Click
        Try
            If QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                Dim isValidAttempt As Boolean = IsValidForAttempts
                If Not isValidAttempt Then
                    isValidAttempt = CurrentService.IsValidAttempts(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id)
                End If
                If Not isValidAttempt Then
                    IsValidForAttempts = isValidAttempt
                    DisplayInvalidAttempt()
                    Exit Sub
                End If
            ElseIf Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                CTRLerrorMessages.Visible = True
                CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                MLVquestionari.SetActiveView(Me.VIWmessaggi)
                Exit Sub
            End If
            Dim isValida As Boolean = True
            Dim ObbligatorieSaltate As Integer = 0
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)
            If isValida Then

                oGestioneQuest.setCampiRispostaQuestionario(False)

                Dim oGestioneRisposte As New GestioneRisposte
                oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False)
                QuestionnarieSaved = True
                LBTroppeRispostePagina.Visible = False
                If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                    'OLD
                    'Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, QuestionarioCorrente.id, UtenteCorrente.ID, QuestionarioCorrente.rispostaQuest.id)

                    'New
                    Dim idRandom As Integer = Me.QuestionarioCorrente.idFiglio 'QuestionarioCorrente.id
                    Dim idQuest As Integer = Me.QuestionarioCorrente.id
                    Dim idAnswer As Integer = Me.QuestionarioCorrente.rispostaQuest.id

                    Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)

                    If (idAnswer <= 0) Then
                        calc = DALQuestionario.CalculateComplationRandomId(PageUtility.CurrentContext, idQuest, UtenteCorrente.ID, idRandom)
                    Else
                        calc = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, idQuest, UtenteCorrente.ID, idAnswer)
                    End If
                    'End new


                    If IsNothing(calc) Then
                        calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = QuestionarioCorrente.id, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                    End If
                    executedAction(qs_linkId, calc.isStarted, calc.isPassed, calc.Completion, False, calc.Mark, UtenteCorrente.ID)
                Else
                    executedAction(qs_linkId, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), False, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio, UtenteCorrente.ID)
                End If
                'Select Case qs_ownerTypeId
                '    Case OwnerType_enum.EduPathSubActivity
                '        executedAction(qs_linkId, UtenteCorrente.ID, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), False, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio)
                '    Case OwnerType_enum.EduPathActivity
                '        executedAction(qs_linkId, UtenteCorrente.ID, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), False, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio)
                'End Select
            Else
                LBTroppeRispostePagina.Visible = True
            End If
            HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
            bindDLPagine()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub LNBSalvaContinua_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBsalvaContinua.Click
        BTNSalvaContinua_Click(sender, e)
    End Sub
    Protected Sub BTNinizia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNinizia.Click
        If iPag = -1 Then
            isFirstRun = True
            TMSessione.Enabled = True
            TMSessione.Interval = RootObject.tickMassimo
            TMSessione_Tick(sender, e)
            If Me.QuestionarioCorrente.isPrimaRisposta Or Me.QuestionarioCorrente.editaRisposta Or Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                If RootObject.setNullDate(Me.QuestionarioCorrente.rispostaQuest.dataInizio) Is System.DBNull.Value Then
                    Me.QuestionarioCorrente.rispostaQuest.dataInizio = DateTime.Now()
                    Me.QuestionarioCorrente.rispostaQuest.indirizzoIPStart = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                End If

                'DirectCast(UPTempo.FindControl("LBTempoRimanente"), Label).Visible = LBTempoRimanenteVIWDescrizione.Visible
                DirectCast(UPTempo.FindControl("LBTempoRimanente"), Label).Text = String.Format(LBTempoRimanente.Text, Me.QuestionarioCorrente.durata)
                'Me.SetFocus(IMBdopo)

                'per disattivare i timer commentare l'if seguente
                If Me.QuestionarioCorrente.durata > 0 Then
                    LBTempoRimanente.Text = String.Format(Me.Resource.getValue("LBTempoRimanente.text"), Me.QuestionarioCorrente.durata, Me.Resource.getValue("minuti"))
                    DIVpanelTempo.Style.Item("display") = "block"
                    TMSessione.Enabled = True
                    TMDurata.Enabled = True
                    TMDurata.Interval = RootObject.autoSaveTimer
                    TMDurata_Tick(sender, e)
                End If
            End If
            If IsValidForAttempts Then
                isCorrezione = False 'serve per i test di autovalutazione
                vaiPaginaDopo()
                oGestioneQuest.CompileStartActionAdd()
            End If

        End If
    End Sub
    Protected Sub LNBannulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Select Case qs_ownerTypeId
            Case OwnerType_enum.EduPathSubActivity, OwnerType_enum.EduPathActivity
                If Not PageUtility.CurrentContext.UserContext.isAnonymous Then
                    Dim isValidAttempt As Boolean = True
                    If QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                        isValidAttempt = IsValidForAttempts
                        If Not isValidAttempt Then
                            isValidAttempt = CurrentService.IsValidAttempts(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id)
                        End If

                        If Not isValidAttempt Then
                            IsValidForAttempts = isValidAttempt
                            DisplayInvalidAttempt()
                            Exit Sub
                        End If
                    End If
                    If Not QuestionarioCorrente.visualizzaRisposta OrElse QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                        Dim availableTime As Integer = 1
                        If QuestionarioCorrente.durata > 0 Then
                            availableTime = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
                        End If
                        If availableTime > 0 Then
                            '    ' POSSO SALVARE I DATI
                            '    ' DEVO SALVARE DEI DATI ASSOLUTAMENTE !
                            Dim isValida As Boolean = True
                            Dim ObbligatorieSaltate As Integer = 0
                            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)
                            If isValida AndAlso Not QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts AndAlso (QuestionarioCorrente.editaRisposta OrElse (Not QuestionarioCorrente.editaRisposta AndAlso CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id))) Then
                                oGestioneQuest.setCampiRispostaQuestionario(False)
                                Dim oGestioneRisposte As New GestioneRisposte
                                oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False)
                                QuestionnarieSaved = True
                            Else
                                SaveEmptyAttempt(QuestionarioCorrente)
                            End If
                        Else
                            SaveEmptyAttempt(QuestionarioCorrente)
                        End If
                    End If
                    If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                        If MustUpdateExternalServiceStatistics() AndAlso QuestionnarieSaved Then
                            Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, QuestionarioCorrente.id, UtenteCorrente.ID, QuestionarioCorrente.rispostaQuest.id)
                            If IsNothing(calc) Then
                                calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = QuestionarioCorrente.id, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                            End If
                            executedAction(qs_linkId, calc.isStarted, calc.isPassed, calc.Completion, False, calc.Mark, UtenteCorrente.ID)
                        End If
                    ElseIf MustUpdateExternalServiceStatistics() AndAlso QuestionnarieSaved Then
                        executedAction(qs_linkId, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), False, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio, UtenteCorrente.ID)
                        'executedAction(qs_linkId, True, True, 0, False, 0, UtenteCorrente.ID)
                    End If
                    If (Me.QuestionarioCorrente.ownerId > 0) Then
                        If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                            PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                        Else
                            Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                        End If
                    End If
                End If
            Case Else
                If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                    PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                    RedirectToUrl(RootObject.QuestionariList & "?type=5&idq=" & Me.QuestionarioCorrente.id.ToString)
                Else
                    Me.RedirectToUrl(RootObject.QuestionariList + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
                End If
        End Select
        'End If
    End Sub
    ''' <summary>
    ''' Aggiunto per gestire i tentativi a vuoto da PF !
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveEmptyAttempt(ByRef questionnaire As COL_Questionario.Questionario)
        Dim oGestioneRisposte As New GestioneRisposte
        If questionnaire.rispostaQuest.id > 0 Then
            ' RIMOSSO IL 19/02/2014
            DALRisposte.clearRisposte(questionnaire.rispostaQuest.id)
        End If
        For Each question As Domanda In questionnaire.domande.Where(Function(q) q.risposteDomanda.Any())
            question.risposteDomanda = New List(Of RispostaDomanda)()
        Next
        'DALRisposte.cancellaRispostaBYID(QuestionarioCorrente.rispostaQuest.id)
        questionnaire.rispostaQuest.dataFine = DateTime.Now
        questionnaire.rispostaQuest.risposteDomande = New List(Of RispostaDomanda)
        oGestioneQuest.setCampiRispostaQuestionario(False)
        oGestioneRisposte.SalvaRisposta(questionnaire, UserId, True)
        QuestionnarieSaved = True
    End Sub
    Protected Sub LNBindietro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.RedirectToUrl(RootObject.QuestionariList + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
    End Sub
    Protected Sub LNBTornaHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBTornaHome.Click
        Me.RedirectToUrl(Me.SystemSettings.Presenter.DefaultLogonPage)
        'RootObject.HomePagePortale)
    End Sub
    Protected Sub LKBpagina_OnClientClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim isValida As Boolean = True
        LBnoRisposta.Visible = False
        Try
            Dim ObbligatorieSaltate As Integer = 0
            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)

            ShowMandatory(ObbligatorieSaltate)
            If ObbligatorieSaltate > 0 Then
                Return
            End If

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
            If IsValidForAttempts Then
                bindDLPagine()
                isFirstRun = True
            End If
        Catch ex As Exception
            inviaMailErrore(ex)
        End Try

    End Sub
    Protected Sub TMSessione_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMSessione.Tick
        If isFirstRun Then
            TMSessione.Interval = RootObject.tickMassimo
            startTime = DateTime.Now
            isFirstRun = False
            If Not (MLVquestionari.ActiveViewIndex = 0) Then
                TMSessione.Enabled = False
                TMDurata.Enabled = False
            End If
        End If
        If DateDiff("n", startTime, DateTime.Now) > RootObject.vitaSessione_max Then
            TMDurata_Tick(sender, e)
            Session("isSessioneScaduta") = True
            Response.Redirect(RootObject.compileUrlUI_short)
        End If
    End Sub
    Protected Sub TMDurata_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMDurata.Tick
        Dim leaveTime As Integer = 0
        Dim previousLeaveTime As Integer = 0
        If Me.QuestionarioCorrente.isPrimaRisposta OrElse Me.QuestionarioCorrente.editaRisposta Then
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
                        Try
                            Dim isValida As Boolean = True
                            Dim ObbligatorieSaltate As Integer = 0
                            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate, True)

                            oGestioneQuest.setCampiRispostaQuestionario(False)

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
                            Else
                                QuestionnarieSaved = True
                            End If

                        Catch ex As Exception
                            Response.Write(ex.Message)
                        End Try
                        If leaveTime < 1 AndAlso previousLeaveTime < 1 Then

                            ' Precedente
                            'Response.Redirect( RootObject.compileUrl_short)
                            Select Case qs_ownerTypeId
                                Case OwnerType_enum.EduPathSubActivity, OwnerType_enum.EduPathActivity
                                    If Not Me.QuestionarioCorrente.visualizzaRisposta OrElse QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                                        Me.QuestionarioCorrente.rispostaQuest.idQuestionarioRandom = Me.QuestionarioCorrente.idFiglio
                                        Me.QuestionarioCorrente.rispostaQuest.dataFine = DateTime.Now()
                                        oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False)
                                        QuestionnarieSaved = True
                                    End If

                                    If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                                        If MustUpdateExternalServiceStatistics() Then
                                            Dim calc As lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) = DALQuestionario.CalculateComplation(PageUtility.CurrentContext, QuestionarioCorrente.id, UtenteCorrente.ID, QuestionarioCorrente.rispostaQuest.id)
                                            If IsNothing(calc) Then
                                                calc = New lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long) With {.Item = QuestionarioCorrente.id, .Completion = 0, .isCompleted = False, .isStarted = True, .isPassed = True, .Mark = 0}
                                            End If

                                            executedAction(qs_linkId, calc.isStarted, calc.isPassed, calc.Completion, False, calc.Mark, UtenteCorrente.ID)
                                        End If


                                        If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                                            PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                                        Else
                                            Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                                        End If
                                    Else
                                        If MustUpdateExternalServiceStatistics() Then
                                            executedAction(qs_linkId, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate * 100 / QuestionarioCorrente.domande.Count), False, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio, UtenteCorrente.ID)
                                        End If
                                        If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                                            PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                                        Else
                                            Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                                        End If
                                    End If
                                Case Else
                                    NoTimeToComplete = True
                                    'Session("isNoTempo") = True
                                    Response.Redirect(Request.RawUrl)
                            End Select
                        End If
                    Else

                    End If
                End If
            Else
                TMDurata.Enabled = False
                leaveTime = Integer.MaxValue
                Dim isvalidAttempt As Boolean = True
                Select Case QuestionarioCorrente.tipo
                    Case CInt(QuestionnaireType.RandomMultipleAttempts)
                        isvalidAttempt = CurrentService.IsValidAttempts(QuestionarioCorrente.id, CurrentContext.UserContext.CurrentUserID, 0, QuestionarioCorrente.rispostaQuest.id)

                End Select

                IsValidForAttempts = isvalidAttempt
                If isvalidAttempt Then
                    'salva comunque la risposta
                    If Not QuestionarioCorrente.editaRisposta AndAlso Not CurrentService.IsValidSave(QuestionarioCorrente.id, QuestionarioCorrente.rispostaQuest.idPersona, QuestionarioCorrente.rispostaQuest.idUtenteInvitato, QuestionarioCorrente.rispostaQuest.id) Then
                        CTRLerrorMessages.Visible = True
                        CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AlreadyCompiled.TipoQuestionario." & DirectCast(QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                        MLVquestionari.SetActiveView(Me.VIWmessaggi)
                        Exit Sub
                    Else
                        Dim isValida As Boolean = True
                        Dim ObbligatorieSaltate As Integer = 0
                        Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate, True)
                        oGestioneQuest.setCampiRispostaQuestionario(True)
                        If QuestionarioCorrente.tipo = QuestionnaireType.AutoEvaluation AndAlso QuestionarioCorrente.idFiglio = 0 Then
                            DALQuestionario.InsertRandomDestinatario(Me.QuestionarioCorrente, Me.UtenteCorrente.ID)
                            Me.QuestionarioCorrente.rispostaQuest.idQuestionarioRandom = Me.QuestionarioCorrente.idFiglio
                        End If
                        If oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, False) = "-1" Then
                            CTRLerrorMessages.Visible = True
                            CTRLerrorMessages.InitializeControl(Resource.getValue("QuestionnaireError.AnswerNotSaved"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                            MLVquestionari.SetActiveView(Me.VIWmessaggi)
                        Else
                            QuestionnarieSaved = True
                        End If
                    End If
                Else
                    DisplayInvalidAttempt()
                End If
            End If
        End If
    End Sub

    Private Sub DisplayInvalidAttempt()
        CTRLerrorMessages.Visible = True
        CTRLerrorMessages.InitializeControl(Resource.getValue("MultipleAttempts"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
        Me.MLVquestionari.SetActiveView(VIWmessaggi)
    End Sub
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Protected Sub LNBTornaLista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBTornaLista.Click
        Dim ownerType As Int64
        ownerType = IIf(QuestionarioCorrente.ownerId = 0, IIf(IsNumeric(qs_ownerType), qs_ownerType, 0), QuestionarioCorrente.ownerId)
        Select Case qs_ownerTypeId
            Case OwnerType_enum.EduPathSubActivity
                If MustUpdateExternalServiceStatistics() AndAlso IsValidForAttempts Then
                    executedAction(qs_linkId, True, True, CInt(100 - QuestionarioCorrente.rispostaQuest.oStatistica.nRisposteSaltate / Math.Max(1, QuestionarioCorrente.domande.Count) * 100), True, QuestionarioCorrente.rispostaQuest.oStatistica.punteggio, UtenteCorrente.ID) 'senza il "max" divide per 0 se non ci sono domande.
                End If

                If String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                    Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                Else
                    PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                End If

            Case OwnerType_enum.EduPathActivity
                If String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                    Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                Else
                    PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                End If
            Case Else
                If Me.QuestionarioCorrente.tipo = QuestionnaireType.Random OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then ' OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.AutoEvaluation Then
                    Me.RedirectToUrl(RootObject.QuestionariList & "?type=0")
                Else
                    Me.RedirectToUrl(RootObject.QuestionariList & "?type=" & Me.QuestionarioCorrente.tipo)
                End If
        End Select
    End Sub
    Private Sub BTNSalvaAutovalutazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNSalvaAutovalutazione.Click
        BTNSalvaEEsci_Click(sender, e)
    End Sub
    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If PHnumeroPagina.Controls.Count > iPag And iPag >= 0 Then
            DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Item("font-style") = "italic"
            DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Item("font-weight") = "bolder"
            DirectCast(PHnumeroPagina.Controls(iPag), LinkButton).Style.Item("text-decoration") = "underline"
        End If
    End Sub

    '#Region "Context"
    '    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    '    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
    '        Get
    '            If IsNothing(_CurrentContext) Then
    '                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
    '            End If
    '            Return _CurrentContext
    '        End Get
    '    End Property
    '#End Region
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property

    Private Sub Page_PreInit1(sender As Object, e As EventArgs) Handles Me.PreInit
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
        DVconfirmSubmit.Visible = True 'Modificato MB
        HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
        SaveCompletedAnswer(sender, e)
        HDNcurrentTime.Value = 0
        HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
    End Sub

    Private Sub BTNundoOption_Click(sender As Object, e As EventArgs) Handles BTNundoOption.Click
        Master.OpenDialogOnPostback = False
        Master.ClearOpenedDialogOnPostback()
        DVconfirmSubmit.Visible = True 'Modificato MB
        HDNcurrentTime.Value = 0
        HIDtempoRimanente.Value = (Me.QuestionarioCorrente.durata * 60) - DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, DateTime.Now)
    End Sub

    Private Sub BTNconfirmLeaveQuestionnaireOption_Click(sender As Object, e As EventArgs) Handles BTNconfirmLeaveQuestionnaireOption.Click
        LNBannulla_Click(sender, e)
    End Sub

    Private Sub QuestionarioCompile_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender


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



    Private Sub ShowMandatory(ByVal mandatoryNum As Integer)

        If mandatoryNum <= 0 Then
            LBMandatoryNotAnswered.Visible = False
        Else
            LBMandatoryNotAnswered.Visible = True
            LBMandatoryNotAnswered.Text = String.Format("Risposte saltate: {0}", mandatoryNum)
        End If

    End Sub


    ''' <summary>
    ''' Aggiunto per il salva!
    ''' Risulta NECESSARIO VERIFICARE che le risposte SALVATE siano effettivamente
    ''' quelle dell'utente corrente, per EVITARE sovrascritture o cancellazioni incongrue.
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property UserId As Integer
        Get
            Return Me.UtenteCorrente.ID
        End Get
    End Property

    'messa in BASE
    '''' <summary>
    '''' Per verifica che la compilazione in SESSIONE corrisponda a quella visualizzata sulla pagina,
    '''' nel caso in cui la sessione venga sovrascritta con altri questionari.
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks>
    '''' NON sono ancora chiare le cause!!!
    '''' </remarks>
    'Private Property QsDestUserId As Integer
    '    Get
    '        Return ViewStateOrDefault("QsCompileId", 0)
    '    End Get
    '    Set(value As Integer)
    '        ViewState("QsCompileId") = value
    '    End Set
    'End Property

End Class