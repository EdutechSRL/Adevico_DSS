Imports COL_Questionario
Imports System.Reflection
Imports lm.Comol.Core.Mail
Imports lm.Comol.Core.DomainModel

Partial Public Class MeetingWiz
    Inherits PageBaseQuestionario
    Implements IViewInvitedUsers
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
    Private _Presenter As InvitedUsersPresenter
    Private ReadOnly Property CurrentPresenter() As InvitedUsersPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InvitedUsersPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private Property SelectedIdTemplate() As Integer Implements IViewInvitedUsers.SelectedIdTemplate
        Get
            If Me.DDLTemplate.Items.Count = 0 OrElse Me.DDLTemplate.SelectedItem Is Nothing Then
                Return 0
            Else
                Return CInt(Me.DDLTemplate.SelectedValue)
            End If
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLTemplate.Items.FindByValue(value.ToString)) Then
                Me.DDLTemplate.SelectedValue = value.ToString
            End If
        End Set
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property SessionIdQuestionnnaire As Integer Implements IViewInvitedUsers.SessionIdQuestionnnaire
        Get
            Return Me.QuestionarioCorrente.id
        End Get
    End Property
    Public ReadOnly Property PreloadedIdQuestionnnaire As Integer Implements IViewInvitedUsers.PreloadedIdQuestionnnaire
        Get
            Return qs_questId
        End Get
    End Property
    Public Property CurrentIdQuestionnnaire As Integer Implements IViewInvitedUsers.CurrentIdQuestionnnaire
        Get
            Return ViewStateOrDefault("CurrentIdQuestionnnaire", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdQuestionnnaire") = value
        End Set
    End Property
    Public ReadOnly Property MailContent As lm.Comol.Core.Mail.dtoMailContent Implements IViewInvitedUsers.MailContent
        Get
            Return Me.CTRLmailEditor.Mail
        End Get
    End Property
    Public ReadOnly Property CurrentSmtpConfig As lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig Implements IViewInvitedUsers.CurrentSmtpConfig
        Get
            Return PageUtility.CurrentSmtpConfig
        End Get
    End Property
#End Region
    Dim isFirstStart As Boolean = False

    Private _nRisposte As Int32 = -1
    Private ReadOnly Property nRisposte() As Int32
        Get
            If _nRisposte = -1 Then
                _nRisposte = DALRisposte.countRisposteBYIDQuestionario(Me.QuestionarioCorrente.id)
            End If
            Return _nRisposte
        End Get
    End Property
    Protected Sub CUVNome_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim Nome As String
        Dim oQuest As Questionario
        Dim oGestioneQuest As New GestioneQuestionario

        oQuest = Me.QuestionarioCorrente
        Nome = TXBNome_WQ.Text
        args.IsValid = Not oGestioneQuest.IsDuplicatedName(oQuest.id, oQuest.tipo, Nome) 'oGestioneQuest.controllaNome(Nome)
    End Sub
    Protected Sub CUVdate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim DataInizio As Date
        Dim DataFine As Date
        DataInizio = RDPDataInizio_WQ.DateInput.SelectedDate.Value.AddHours(DDLOraInizio_WQ.SelectedValue).AddMinutes(DDLMinutiInizio_WQ.SelectedValue)
        DataFine = RDPDataFine_WQ.DateInput.SelectedDate.Value.AddHours(DDLOraFine_WQ.SelectedValue).AddMinutes(DDLMinutiFine_WQ.SelectedValue)
        If DateDiff(DateInterval.Second, DataInizio, DataFine) < 0 And Not DataFine = Date.MaxValue Then
            args.IsValid = False
            Exit Sub
        Else
            Me.QuestionarioCorrente.dataInizio = DataInizio
            Me.QuestionarioCorrente.dataFine = DataFine
        End If
    End Sub

    Public Overrides Sub BindDati()
        If Request.QueryString("new") = 1 Then
            QuestionarioCorrente = New Questionario
        End If
        If Me.QuestionarioCorrente.id = 0 Then
            Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting
            Me.QuestionarioCorrente.descrizione = String.Empty
            setPage(Nothing)
            LNBPreview.Visible = False
        Else
            Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)
            Me.QuestionarioCorrente.url = Me.EncryptedUrl(RootObject.compileUrlUI, "idq=" & Me.QuestionarioCorrente.id & "&idl=" & Me.LinguaQuestionario & "&ida=1", SecretKeyUtil.EncType.Questionario)
            If Me.QuestionarioCorrente.pagine.Count = 0 Then
                setPage(Nothing)
            Else
                If Me.QuestionarioCorrente.pagine(0).domande.Count = 0 Then
                    initOption(True)
                Else
                    Me.DomandaCorrente = Me.QuestionarioCorrente.pagine(0).domande(0)
                End If
            End If
        End If
        bindDateTime()
        MLVquestionari.SetActiveView(VIWStep1)
        isFirstStart = True
    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVquestionari.SetActiveView(VIWMessaggi)
        LBErrore.Text = Me.Resource.getValue("MSGNoPermessi")
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin)
    End Function
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Protected Sub bindDateTime()
        Dim counter As Int32
        For counter = 0 To 9
            DDLOraInizio_WQ.Items.Add(counter)
            DDLOraFine_WQ.Items.Add(counter)
            DDLOraInizio_WQ.Items(counter).Text = "0" & counter
            DDLOraInizio_WQ.Items(counter).Value = counter
            DDLOraFine_WQ.Items(counter).Text = "0" & counter
            DDLOraFine_WQ.Items(counter).Value = counter
        Next
        For counter = 10 To 23
            DDLOraInizio_WQ.Items.Add(counter)
            DDLOraFine_WQ.Items.Add(counter)
        Next
        For counter = 0 To 1
            'visualizza 00 e 05 per i minuti
            DDLMinutiInizio_WQ.Items.Add(counter)
            DDLMinutiInizio_WQ.Items(counter).Text = "0" & counter * 5
            DDLMinutiInizio_WQ.Items(counter).Value = counter * 5
            DDLMinutiFine_WQ.Items.Add(counter)
            DDLMinutiFine_WQ.Items(counter).Text = "0" & counter * 5
            DDLMinutiFine_WQ.Items(counter).Value = counter * 5
        Next
        For counter = 2 To 11
            DDLMinutiInizio_WQ.Items.Add(counter)
            DDLMinutiInizio_WQ.Items(counter).Text = counter * 5
            DDLMinutiInizio_WQ.Items(counter).Value = counter * 5
            DDLMinutiFine_WQ.Items.Add(counter)
            DDLMinutiFine_WQ.Items(counter).Text = counter * 5
            DDLMinutiFine_WQ.Items(counter).Value = counter * 5
        Next

        RDPDataInizio_WQ.SelectedDate = Now.Date
        RDPDataFine_WQ.SelectedDate = RDPDataFine_WQ.MaxDate
        DDLOraInizio_WQ.SelectedValue = Now.Hour
        DDLOraFine_WQ.SelectedValue = Date.MaxValue.Hour

        Dim minIndex As Int16
        minIndex = (Now.Minute / 5)
        If minIndex = -1 Then
            minIndex = 0
        End If
        DDLMinutiInizio_WQ.SelectedIndex = minIndex
        'minIndex = Date.MaxValue.Minute / 5
        'If minIndex = 12 Then
        '    minIndex = 11
        'End If
        'DDLMinutiFine_WQ.SelectedIndex = minIndex
        DDLMinutiFine_WQ.SelectedIndex = 11
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        'solo il sysadmin può creare modelli pubblici, e li puo' creare solo prima di accedere a una comunita'
        'non esiste una comunita', quindi non ha senso "compilabile dagli utenti della comunita'"
        If Me.ComunitaCorrenteID = 0 Then
            CHKUtentiComunita_WQ.Checked = False
            CHKUtentiComunita_WQ.Visible = False
            LNBaddSysUsers.Visible = False
        End If
        If MyBase.Servizio.Admin Then
            CHKUtentiNonComunita_WQ.Visible = True
        Else
            CHKUtentiNonComunita_WQ.Visible = False
        End If
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MeetingWiz", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        SetServiceTitle(Master)
        If LBOptionNumber.Text = String.Empty Then
            With Me.Resource

                'eliminare da pg_MeetingAdd la robaccia inutile copiata da QuestionarioAdd
                .setLabel(DirectCast(Master.FindControl("LBtitolo"), Label))
                .setLinkButton(LNBPreview, False, False)

                .setLabel(LBNome_WQ)
                .setCustomValidator(CUVNome_WQ)
                .setRequiredFieldValidator(RFVNome_WQ, True, False)
                .setLabel(LBDataInizioTitolo_WQ)
                .setLabel(LBDataFineTitolo_WQ)
                .setCustomValidator(CUVdate_WQ)
                .setLabel(LBvisualizzazione_WQ)
                .setLabel(LBvisualizzaRisposta_WQ)
                .setLabel(LBeditaRisposta_WQ)
                .setLabel(LBtitoloDestinatari_WQ)
                .setLabel(LBmail)
                .setTextBox(TXBmessage)
                .setLabel(LBtitoloS1)
                .setLabel(LBdescrizioneS1)
                .setLabel(LBtitoloS2)
                .setLabel(LBtitoloS3)
                .setLabel(LBdescrizioneS3)
                .setCheckBox(CHKUtentiComunita_WQ)
                .setCheckBox(CHKUtentiNonComunita_WQ)
                .setCheckBox(CHKUtentiInvitati_WQ)
                '.setLabel(LBavvisoBloccato_WQ)
                LNBAvanti1.Text = .getValue("MSGAvanti")
                .setLinkButtonForName(LNBAvanti3, "LNBPreview", False, True)
                LNBindietro3.Text = .getValue("MSGIndietro")

                'step2
                .setLabel(LBTestoDomanda)
                .setLabel(LBTitoloOpzioni)
                .setLabel(LBNumeroOpzioni)
                .setLabel(LBZone)
                .setLabel(LBTestoDopoDomanda)
                .setLabel(LBOptionNumber)
                .setLabel(LBErrorNoSelection)
                .setLinkButton(LNBAddDays, False, True)
                .setLinkButton(LNBDeleteDays, False, True, , True)
                LNBAvanti2.Text = .getValue("MSGAvanti")
                LNBIndietro2.Text = .getValue("MSGIndietro")

                'step3
                .setLinkButton(LNBaddExternalUser, False, False)
                .setHeaderGridView(Me.GRVElenco, 1, "headerCognome", True)
                .setHeaderGridView(Me.GRVElenco, 2, "headerNome", True)
                .setHeaderGridView(Me.GRVElenco, 3, "headerEmail", True)
                .setLinkButton(LNBaddSysUsers, False, True)
                .setLinkButton(LKBsendMail, False, True)
                .setLinkButton(LKBadvanced, False, True)
                'importa da comunità
                .setLinkButton(LNBCancelIDC, False, True)
                .setLabel(LBmessageTitle)
                .setLabel(LBmessageTXBTitle)

                'mail
                .setLabel(LBErroreNoTag)
                .setLabel(LBMsgQuestionarioBloccato)
                .setLabel(LBTitoloTemplate)
                .setButton(BTNElimina)
                .setButton(BTNNuovo)
                .setLabel(LBDestinatario)
                .setLinkButton(LKBAggiungiTutti, False, True)
                .setLinkButton(LKBAggiungiNonInvitati, False, True)
                .setLinkButton(LKBSelezionaUtenti, False, True)
                .setLinkButton(LKBAggiungiNonCompletati, False, True)
                .setLabel(LBMsgSbloccaInvia)
                .setLabel(LBAnteprima)
                .setButton(BTNSalvaTemplate)
                .setCheckBox(CHKInoltraMittente)
                .setButton(BTNSalvaTemplateConNome)
                .setButton(BTNSbloccaInvia)
                .setLabel(LBMsgInvia)
                .setButton(BTNInviaMail)
                LNBCancelMail.Text = .getValue("LNBCancelIDC.text")

                'riepilogo
                LBRiepilogo.Text = .getValue("MSGRiepilogo")
                LNBBackPreview.Text = .getValue("MSGIndietro")
                .setLinkButton(LNBSave, False, True)
                .setLinkButton(LNBSaveAndUnlock, False, True)
                .setLinkButtonForName(LNBSaveAndManageMail, "LKBadvanced", False, True)

                'messaggi
                .setLinkButton(LNBBackToMail, False, True)
            End With
        End If
    End Sub
    Private Function saveMeetingData() As Boolean
        Dim saveOK As Boolean
        Dim oGestioneQuest As New GestioneQuestionario
        Me.QuestionarioCorrente.idLingua = LinguaQuestionario
        Me.QuestionarioCorrente.isReadOnly = False
        Me.QuestionarioCorrente.visualizzaCorrezione = False
        Me.QuestionarioCorrente.visualizzaSuggerimenti = False
        Me.QuestionarioCorrente.isRandomOrder = False
        Me.QuestionarioCorrente.idPersonaEditor = Me.UtenteCorrente.ID
        Me.QuestionarioCorrente.dataModifica = Now
        Me.QuestionarioCorrente.idGruppo = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrente.Id)
        Me.QuestionarioCorrente.idPersonaCreator = Me.UtenteCorrente.ID
        If CUVdate_WQ.IsValid And CUVNome_WQ.IsValid Then
            If Not Me.QuestionarioCorrente.nome.TrimStart("") Is String.Empty Then
                saveOK = oGestioneQuest.salvaQuestionario(Me.QuestionarioCorrente)
                Me.DomandaCorrente.idQuestionario = Me.QuestionarioCorrente.id
            End If
        End If
        Return saveOK
    End Function
    Private Sub saveMeeting(ByRef redirectURL As String, Optional ByRef redirView As View = Nothing, Optional ByRef reload As Boolean = False)
        Dim oGestioneQuest As New GestioneQuestionario
        Dim saveOK As Boolean
        saveOK = saveMeetingData()
        If saveOK And Not nRisposte > 0 Then
            setPage(Me.QuestionarioCorrente.pagine(0))
            DALPagine.Pagina_Salva(Me.QuestionarioCorrente.pagine(0))
            'Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)
            Dim oPagina As New QuestionarioPagina
            oPagina = DALPagine.readPaginaByIDQuestionario(Me.QuestionarioCorrente.idQuestionarioMultilingua, 1)
            Me.QuestionarioCorrente.pagine(0).id = oPagina.id
            Me.DomandaCorrente.idPagina = Me.QuestionarioCorrente.pagine(0).id
            Me.DomandaCorrente.idLingua = LinguaQuestionario
            PaginaCorrenteID = Me.DomandaCorrente.idPagina
            Dim returnValue As Int16
            For Each oDomanda As Domanda In Me.QuestionarioCorrente.pagine(0).domande

                returnValue = (salvaDomanda(oDomanda))
                Select Case returnValue
                    Case 1
                        MLVquestionari.SetActiveView(VIWMessaggi)
                        LBErrore.Text = Me.Resource.getValue("MSGErroreOpzioni")
                        LBErrore.Visible = True
                        Me.Resource.setLinkButton(LNBBackToStep2, False, False)
                        LNBBackToStep2.Visible = True
                    Case 2
                        MLVquestionari.SetActiveView(VIWMessaggi)
                        LBErrore.Text = Me.Resource.getValue("MSGErroreTestoDomanda")
                        LBErrore.Visible = True
                        Me.Resource.setLinkButton(LNBBackToStep2, False, False)
                        LNBBackToStep2.Visible = True
                    Case 3
                        MLVquestionari.SetActiveView(VIWMessaggi)
                        LBErrore.Text = Me.Resource.getValue("ErroreNoCorretta")
                        LBErrore.Visible = True
                End Select
            Next
        End If
        If Not MLVquestionari.Views(MLVquestionari.ActiveViewIndex).ID = VIWMessaggi.ID Then
            If Not Me.QuestionarioCorrente.isBloccato Then
                oGestioneQuest.notifyCurrentQuestionnaire()
            End If
            'l'idDomanda viene comunque aggiornato, quindi a cosa serve? inoltre incasina qualcosa e l'app. crasha
            'If reload Then
            '    Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)
            'End If
            If redirectURL = String.Empty Then
                MLVquestionari.SetActiveView(redirView)
            Else
                Me.RedirectToUrl(redirectURL)
            End If
        End If
    End Sub
    Protected Sub LNBSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBSave.Click
        saveMeeting(RootObject.QuestionariGestioneList & "&type=6")
    End Sub
    Private Function setPage(ByVal oPagina As QuestionarioPagina)
        If oPagina Is Nothing Then
            oPagina = New QuestionarioPagina
            oPagina.id = 0
            Me.PaginaCorrenteID = 0
            Me.QuestionarioCorrente.pagine.Add(oPagina)
            initOption(True)
        End If
        oPagina = Me.QuestionarioCorrente.pagine(0)
        oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
        oPagina.descrizione = ""
        oPagina.nomePagina = Me.QuestionarioCorrente.nome
        oPagina.randomOrdineDomande = False
        oPagina.numeroPagina = 1
        oPagina.dallaDomanda = 0
        oPagina.allaDomanda = Me.QuestionarioCorrente.pagine(0).domande.Count
    End Function
    Public ReadOnly Property visibilityValutazione() As String
        Get
            Select Case Me.QuestionarioCorrente.tipo
                Case Questionario.TipoQuestionario.Questionario
                    Return "block"
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    Return "block"
                Case Else
                    Return "none"
            End Select
        End Get
    End Property
    Public ReadOnly Property isDomandaReadOnly() As Boolean
        Get
            If Me.DomandaCorrente.isReadOnly Then
                Return Me.DomandaCorrente.isReadOnly
            Else
                Return Me.QuestionarioCorrente.isReadOnly
            End If
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If MLVquestionari.Views.Item(MLVquestionari.ActiveViewIndex).UniqueID = VIWMail.UniqueID Then
            'caricaTags()
            Me.LBpreviewDisplay.Text = Me.CTRLmailEditor.MailBodyPreview
        End If
        'If  Then
        '    caricaTags()
        'End If
    End Sub
    Private Sub bindFields()
        bindFieldDomandaMeetingWiz(Me.DomandaCorrente, Me.QuestionarioCorrente.pagine(0), Me.QuestionarioCorrente)
    End Sub
    Public Sub bindFieldDomandaMeetingWiz(ByVal oDomanda As Domanda, ByVal oPagina As QuestionarioPagina, ByVal oQuest As Questionario)
        CTRLeditorTestoDomanda.HTML = oDomanda.testo

        TXBTestoDopoDomanda.Text = oDomanda.testoDopo
        Dim oCal As Telerik.Web.UI.RadCalendar
        oCal = RDCLCalendar
        oCal.SelectedDates.Clear()
        For Each item As Date In oDomanda.domandaRating.intestazioniMeeting
            Dim oDate As New Telerik.Web.UI.RadDate
            oDate.Date = item
            oCal.SelectedDates.Add(oDate)
        Next
        If oDomanda.domandaRating.opzioniRating.Count < 1 Then
            Dim opz As New DomandaOpzione
            oDomanda.domandaRating.opzioniRating.Add(opz)
        End If
        DDLNumeroOpzioni.SelectedValue = oDomanda.domandaRating.opzioniRating.Count
    End Sub
    Public Sub clearFieldDomandaMeetingWiz()
        CTRLeditorTestoDomanda.HTML = String.Empty
        TXBTestoDopoDomanda.Text = String.Empty
        RDCLCalendar.SelectedDates.Clear()
        'If oDomanda.domandaRating.opzioniRating.Count < 1 Then
        '    Dim opz As New DomandaOpzione
        '    oDomanda.domandaRating.opzioniRating.Add(opz)
        'End If
        DDLNumeroOpzioni.SelectedValue = 1
    End Sub
    Protected Sub bindZone()
        RPTZone.DataSource = Me.DomandaCorrente.domandaRating.opzioniRating
        RPTZone.DataBind()
    End Sub
    Protected Sub bindIntestazioni()
        Me.DomandaCorrente.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi
    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(LBZone)
            .setLabel(LBTestoDomanda)
            .setLabel(LBNumeroOpzioni)
            .setLabel(LBTestoDopoDomanda)
        End With
    End Sub
    'Public Sub SetInternazionalizzazioneDLOpzioni(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
    '    With Me.Resource
    '        .setCheckBox(e.Item.FindControl("CBisAltro"))
    '        .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
    '        .setLabel(e.Item.FindControl("LBTestoMax"))
    '        .setRegularExpressionValidator(e.Item.FindControl("REVTXBTestoMin"))
    '        .setRegularExpressionValidator(e.Item.FindControl("REVTXBTestoMax"))

    '    End With
    'End Sub
    Public Sub SetInternazionalizzazioneDLIntestazioni(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBIntestazione"))
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBIntestazione"))
        End With
    End Sub
    Protected Sub DLIntestazioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        SetInternazionalizzazioneDLIntestazioni(e)

    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oGestioneDomande As New GestioneDomande
        setDomandaMeetingWiz(Me.DomandaCorrente, Me.QuestionarioCorrente)
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DDLNumeroOpzioni.SelectedValue, Nothing)
        bindZone()
        VIWStep2_Unload()
    End Sub
    Protected Sub selectOption(ByVal sender As Object, ByVal e As System.EventArgs)
        ReadDomandaFromPage()
        VIWStep2_Unload()
        'setDomandaMeetingWiz(Me.DomandaCorrente, Me.QuestionarioCorrente)
        RDCLCalendar.SelectedDates.Clear()
        Me.DomandaCorrente = Me.QuestionarioCorrente.pagine(0).domande(DDLOptionNumber.SelectedIndex)
        VIWStep2_Load()
    End Sub
    Public Sub setDomandaMeetingWiz(ByRef oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByVal isChangingTable As Boolean = False)
        'bisogna ordinare gli elementi in uscita dal radControl, altrimenti escono nell'ordine nel quale son stati selezionati
        oDomanda.domandaRating.intestazioniMeeting = (From x In RootObject.RadDate_ToList(RDCLCalendar.SelectedDates) Order By x.Date).ToList
        oDomanda.domandaRating.mostraND = False
        oDomanda.domandaRating.numeroRating = oDomanda.domandaRating.intestazioniMeeting.Count  'oDates.Count  'oDate.Count 'DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue
        oDomanda.domandaRating.opzioniRating.Clear()
        'le opzioni vuote non vengono aggiunte
        For Each item As RepeaterItem In RPTZone.Items
            Dim oDomandaOpzione As New DomandaOpzione
            Dim txb As New TextBox
            txb = DirectCast(item.Controls(1), TextBox)
            If Not txb Is Nothing Then
                oDomandaOpzione.testo = txb.Text
                If Not oDomandaOpzione.testo.Trim() Is String.Empty Then
                    oDomanda.domandaRating.opzioniRating.Add(oDomandaOpzione)
                End If
            End If
        Next
        'se tutte le opzioni son state lasciate vuote, una viene creata
        If oDomanda.domandaRating.opzioniRating.Count = 0 Then
            Dim oDomandaOpzione As New DomandaOpzione
            oDomandaOpzione.testo = String.Empty
            oDomanda.domandaRating.opzioniRating.Add(oDomandaOpzione)
        End If
        oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi
    End Sub
    Private Function ReadDomandaFromPage() As Boolean
        If RDCLCalendar.SelectedDates.Count > 0 Then
            Dim oDomanda As New Domanda
            oDomanda = Me.DomandaCorrente
            If oDomanda.id = 0 Then
                oDomanda.idLingua = Me.LinguaQuestionario
                oDomanda.idPersonaCreator = Me.UtenteCorrente.ID
                oDomanda.dataCreazione = Now
                oDomanda.idQuestionario = Me.QuestionarioCorrente.id
                oDomanda.isObbligatoria = False
                oDomanda.suggerimento = String.Empty
            End If
            oDomanda.dataModifica = Now
            oDomanda.idPersonaEditor = Me.UtenteCorrente.ID
            Dim nuovoTesto As String
            nuovoTesto = RootObject.removeBRfromStringEnd(CTRLeditorTestoDomanda.HTML)
            'se cambia testo in un gruppo, deve essere cambiato per tutti:
            If Not oDomanda.testo = nuovoTesto OrElse Not TXBTestoDopoDomanda.Text = oDomanda.testoDopo Then
                oDomanda.testo = nuovoTesto
                oDomanda.testoDopo = TXBTestoDopoDomanda.Text
                For Each oDom As Domanda In Me.QuestionarioCorrente.pagine(0).domande
                    oDom.testo = nuovoTesto
                    oDom.testoDopo = TXBTestoDopoDomanda.Text
                Next
            End If
            If Me.QuestionarioCorrente.pagine.Count > 0 Then
                oDomanda.idPagina = Me.QuestionarioCorrente.pagine(0).id
            End If
            setDomandaMeetingWiz(oDomanda, Me.QuestionarioCorrente)
            If oDomanda.numero = 0 Then
                oDomanda.numero = Me.QuestionarioCorrente.pagine(0).domande.Count
                oDomanda.numeroPagina = Me.QuestionarioCorrente.pagine(0).numeroPagina
            End If
            If Me.QuestionarioCorrente.pagine(0).domande.Count >= oDomanda.numero Then
                Me.QuestionarioCorrente.pagine(0).domande(oDomanda.numero - 1) = oDomanda
            Else
                Me.QuestionarioCorrente.pagine(0).domande.Add(Me.DomandaCorrente)
                Me.QuestionarioCorrente.pagine(0).allaDomanda += 1
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Private ReadOnly Property testo1() As String
        Get
            Return Me.QuestionarioCorrente.pagine(0).domande(0).testo
        End Get
    End Property
    Private Sub LNBAvanti1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBAvanti1.Click
        If VIWStep1_Unload() Then
            If nRisposte > 0 Then
                MLVquestionari.SetActiveView(VIWStep3)
            Else
                MLVquestionari.SetActiveView(VIWStep2)
            End If
        End If
    End Sub
    Private Sub LNBAvanti2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBAvanti2.Click
        If ReadDomandaFromPage() Then
            If VIWStep2_Unload() Then
                MLVquestionari.SetActiveView(VIWStep3)
            End If
        Else
            LBErrorNoSelection.Visible = True
        End If
    End Sub
    Public Function salvaDomanda(ByRef oDomanda As Domanda) As Integer
        Dim retVal As Integer
        If Not oDomanda.testo.Trim() = String.Empty Or oDomanda.testo.Trim() = "<br>" Then
            Dim saveOK As Boolean
            saveOK = DALDomande.Salva(oDomanda, False, False)
            If saveOK Then
                'Dim oGestioneDomande As New GestioneDomande
                'oGestioneDomande.ricalcoloPagine(oDomanda, Me.QuestionarioCorrente)
                'Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)
                retVal = 0
            Else
                ' 1 = errore opzioni
                retVal = 1
            End If
        Else
            ' 2 = errore testo domanda
            retVal = 2
        End If
        Return retVal
    End Function
    Private Sub LNBIndietro2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBIndietro2.Click
        If ReadDomandaFromPage() Then
            If VIWStep2_Unload() Then
                MLVquestionari.SetActiveView(VIWStep1)
            End If
        Else
            LBErrorNoSelection.Visible = True
        End If
    End Sub
    Private Function VIWStep1_Unload() As Boolean
        Me.QuestionarioCorrente.nome = TXBNome_WQ.Text
        Me.QuestionarioCorrente.visualizzaRisposta = CHKvisualizzaRisposta_WQ.Checked
        Me.QuestionarioCorrente.editaRisposta = CHKeditaRisposta_WQ.Checked
        Return saveMeetingData()
    End Function
    Private Sub VIWStep3_Unload()
        Me.QuestionarioCorrente.forUtentiComunita = CHKUtentiComunita_WQ.Checked
        Me.QuestionarioCorrente.forUtentiPortale = CHKUtentiNonComunita_WQ.Checked
        Me.QuestionarioCorrente.forUtentiInvitati = CHKUtentiInvitati_WQ.Checked
        Me.QuestionarioCorrente.forUtentiEsterni = False
    End Sub
    Private Sub LNBAvanti3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBAvanti3.Click
        VIWStep3_Unload()
        'If Me.QuestionarioCorrente.forUtentiInvitati Then
        '    Me.MLVquestionari.SetActiveView(VIWStep4)
        'Else
        Me.MLVquestionari.SetActiveView(VIWRiepilogo)
        'End If
    End Sub
    Private Sub LNBindietro3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro3.Click
        VIWStep3_Unload()
        If nRisposte > 0 Then
            MLVquestionari.SetActiveView(VIWStep1)
        Else
            Me.MLVquestionari.SetActiveView(VIWStep2)
        End If
    End Sub
    'Private Sub step4()



    'End Sub
    'Private Sub LNBAvanti4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBAvanti4.Click
    '    step4()
    '    Me.MLVquestionari.SetActiveView(VIWRiepilogo)
    'End Sub
    'Private Sub LNBindietro4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro4.Click
    '    step4()
    '    Me.MLVquestionari.SetActiveView(VIWStep3)
    'End Sub
    Private Sub LNBPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBPreview.Click
        Dim goToPreview As Boolean = True
        Select Case MLVquestionari.Views(MLVquestionari.ActiveViewIndex()).ID
            Case VIWStep1.ID
                goToPreview = VIWStep1_Unload()
            Case VIWStep2.ID
                goToPreview = ReadDomandaFromPage() And VIWStep2_Unload()
                If Not goToPreview Then
                    LBErrore.Visible = True
                End If
            Case VIWStep3.ID
                VIWStep3_Unload()
        End Select
        If goToPreview Then
            Me.MLVquestionari.SetActiveView(VIWRiepilogo)
        End If
    End Sub
    Private Sub VIWRiepilogo_Load()
        LNBPreview.Visible = False

        LNBSaveAndManageMail.Visible = Me.QuestionarioCorrente.forUtentiInvitati
        LNBSaveAndUnlock.Visible = Me.QuestionarioCorrente.isBloccato
        LBTestoMeeting.Text = Me.QuestionarioCorrente.pagine(0).domande(0).testo
        DLDomande.DataSource = Me.QuestionarioCorrente.pagine(0).domande
        DLDomande.DataBind()
    End Sub
    Private Function maxDate(ByRef oDate As Date) As Date
        Dim RADdate As New Telerik.Web.UI.RadDatePicker
        Dim retval As Date
        If RADdate.MaxDate < oDate.Date Then
            retval = RADdate.MaxDate
        Else
            retval = oDate
        End If
        Return retval
    End Function

    Private Sub VIWStep1_Load()
        ' e' il primo caricamento di un nuovo meeting, quindi non deve fare niente
        If Not Me.QuestionarioCorrente.dataInizio Is Nothing Then
            LNBPreview.Visible = True
            TXBNome_WQ.Text = Me.QuestionarioCorrente.nome
            RDPDataInizio_WQ.SelectedDate = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Date
            RDPDataFine_WQ.SelectedDate = maxDate(DateTime.Parse(Me.QuestionarioCorrente.dataFine).Date)
            DDLOraInizio_WQ.SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Hour
            DDLOraFine_WQ.SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Hour
            DDLMinutiInizio_WQ.SelectedIndex = Math.Floor(DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Minute / 5)
            DDLMinutiFine_WQ.SelectedIndex = Math.Floor(DateTime.Parse(Me.QuestionarioCorrente.dataFine).Minute / 5)
            CHKvisualizzaRisposta_WQ.Checked = Me.QuestionarioCorrente.visualizzaRisposta
            CHKeditaRisposta_WQ.Checked = Me.QuestionarioCorrente.editaRisposta
        End If

    End Sub

    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        Dim iDom As Int16
        Dim oGestioneDomande As New GestioneDomande
        iDom = e.Item.ItemIndex

        oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, 0, iDom, False)
        DLDomande.Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, 0, iDom, True))
    End Sub
    Private _SmartTagsAvailable As SmartTags
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase())
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    'Protected Sub caricaTags()
    '    For Each oTag As TemplateTag In ListaTags
    '        Dim btTag As New Button
    '        btTag.Text = Me.Resource.getValue(oTag.Name)
    '        btTag.Attributes.Add("onclick", "insertAtCursor('" + oTag.Tag + "');return false;")
    '        If oTag.isMandatory Then
    '            btTag.ForeColor = Color.Red
    '        End If
    '        PHTags.Controls.Add(btTag)
    '        Dim aCapo As New LiteralControl
    '        aCapo.Text = "&nbsp;&nbsp;"
    '        PHTags.Controls.Add(aCapo)
    '    Next
    'End Sub
    Private Sub MLVquestionari_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MLVquestionari.ActiveViewChanged
        Select Case MLVquestionari.Views(MLVquestionari.ActiveViewIndex()).ID
            Case VIWStep1.ID
                VIWStep1_Load()
            Case VIWStep2.ID
                VIWStep2_Load()
            Case VIWStep3.ID
                VIWStep3_Load()
            Case VIWaddExternalUser.ID
                VIWaddExternalUser_Load()
            Case VIWimportaDaComunita.ID
                VIWimportaDaComunita_Load()
            Case VIWRiepilogo.ID
                VIWRiepilogo_Load()
            Case VIWMail.ID
                VIWMail_Load()
        End Select
    End Sub
    Private Sub LNBSaveAndUnlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBSaveAndUnlock.Click
        Me.QuestionarioCorrente.isBloccato = False
        saveMeeting(RootObject.QuestionariGestioneList & "&type=6")
    End Sub
    Private Sub LNBBackPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBBackPreview.Click
        Me.MLVquestionari.SetActiveView(VIWStep3)
    End Sub
    Private Sub LNBSaveAndManageMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBSaveAndManageMail.Click
        UtentiInvitatiLista.Clear()
        saveMeeting(String.Empty, VIWMail, True)
    End Sub
    Private Sub initOption(ByRef isNewOption As Boolean)
        If isNewOption OrElse Me.DomandaCorrente Is Nothing OrElse Me.DomandaCorrente.testo Is Nothing Then
            Me.DomandaCorrente = New Domanda
            Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Meeting
            If Me.QuestionarioCorrente.pagine(0).domande.Count = 0 Then
                Me.DomandaCorrente.testo = String.Empty
                Me.DomandaCorrente.numero = 1
                Me.QuestionarioCorrente.pagine(0).domande.Add(Me.DomandaCorrente)
            Else
                Me.DomandaCorrente.testo = Me.QuestionarioCorrente.pagine(0).domande(0).testo
                Me.DomandaCorrente.testoDopo = Me.QuestionarioCorrente.pagine(0).domande(0).testoDopo
                Me.DomandaCorrente.numero = Me.QuestionarioCorrente.pagine(0).domande.Count + 1
                Me.QuestionarioCorrente.pagine(0).domande.Add(Me.DomandaCorrente)
            End If
            DDLOptionNumber.Items.Clear()
            Dim c As Int16 = 0
            Do
                Dim ddlItem As New ListItem
                ddlItem.Value = c + 1
                DDLOptionNumber.Items.Add(ddlItem)
                c += 1
            Loop While c < Me.QuestionarioCorrente.pagine(0).domande.Count
            DDLOptionNumber.SelectedValue = Me.QuestionarioCorrente.pagine(0).domande.Count
            isFirstStart = True
        Else
            Me.DomandaCorrente = Me.QuestionarioCorrente.pagine(0).domande(DDLOptionNumber.SelectedIndex)
        End If
        If Me.QuestionarioCorrente.pagine.Count = 0 Then
            setPage(Nothing)
        End If
        If Not isNewOption Then

            bindFields()
            bindZone()
        Else
            clearFieldDomandaMeetingWiz()
            RPTZone.DataBind()
        End If
        bindIntestazioni()

        DDLNumeroOpzioni.SelectedValue = IIf(Me.DomandaCorrente.domandaRating.opzioniRating.Count = 0, 1, Me.DomandaCorrente.domandaRating.opzioniRating.Count)

    End Sub
#Region "common"
    Private Sub deleteDays(ByRef groupIndex As Int16, ByRef viwRedir As View)
        DALDomande.Domanda_Delete(Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.domande(groupIndex).numero, Me.QuestionarioCorrente.domande(groupIndex).id)
        Me.QuestionarioCorrente.pagine(0).domande.RemoveAt(groupIndex)
        For c As Integer = groupIndex To Me.QuestionarioCorrente.pagine(0).domande.Count - 1
            Me.QuestionarioCorrente.pagine(0).domande(c).numero = c + 1
        Next
        Me.QuestionarioCorrente.pagine(0).allaDomanda = Me.QuestionarioCorrente.domande.Count
        'saveMeeting(String.Empty, viwRedir)
        DDLOptionNumber.Items.Clear()
    End Sub
#End Region
#Region "step2"
    Private Function VIWStep2_Unload() As Boolean
        If RootObject.removeBRfromStringEnd(CTRLeditorTestoDomanda.HTML).Trim = String.Empty Then
            Me.Resource.setLabel(LBErrorNoQuestion)
            LBErrorNoQuestion.Visible = True
            Return False
        Else
            LBErrorNoQuestion.Visible = False
            Return True
        End If
    End Function
    Private Sub VIWStep2_Load()
        LNBPreview.Visible = True
        LBErrorNoSelection.Visible = False
        If Me.QuestionarioCorrente.pagine(0).domande.Count < 2 Then
            LNBDeleteDays.Visible = False
        Else
            LNBDeleteDays.Visible = True
        End If
        If DDLOptionNumber.SelectedIndex = -1 Then
            DDLOptionNumber.Items.Clear()
            Dim c As Int16 = 0
            Do
                Dim ddlItem As New ListItem
                ddlItem.Value = c + 1
                DDLOptionNumber.Items.Add(ddlItem)
                c += 1
            Loop While c < Me.QuestionarioCorrente.pagine(0).domande.Count
            DDLOptionNumber.SelectedIndex = 0
        Else
            Me.DomandaCorrente = Me.QuestionarioCorrente.pagine(0).domande(DDLOptionNumber.SelectedIndex)
        End If
        isFirstStart = True
        initOption(False)
        If Me.DomandaCorrente.testo Is Nothing Then
            If Me.QuestionarioCorrente.pagine(0).domande.Count = 0 Then
                Me.QuestionarioCorrente.pagine(0).domande.Add(New Domanda)
            End If
            Me.DomandaCorrente = Me.QuestionarioCorrente.pagine(0).domande(DDLOptionNumber.SelectedIndex)
        End If

        If CTRLeditorTestoDomanda.HTML = String.Empty Then
            CTRLeditorTestoDomanda.HTML = Me.DomandaCorrente.testo
            TXBTestoDopoDomanda.Text = Me.DomandaCorrente.testoDopo
            Dim oCal As Telerik.Web.UI.RadCalendar
            oCal = RDCLCalendar
            oCal.SelectedDates.Clear()
            For Each item As Date In Me.DomandaCorrente.domandaRating.intestazioniMeeting
                Dim oDate As New Telerik.Web.UI.RadDate
                oDate.Date = item
                oCal.SelectedDates.Add(oDate)
            Next
            If Me.DomandaCorrente.domandaRating.opzioniRating.Count < 1 Then
                Dim opz As New DomandaOpzione
                Me.DomandaCorrente.domandaRating.opzioniRating.Add(opz)
            End If
            DDLNumeroOpzioni.SelectedValue = Me.DomandaCorrente.domandaRating.opzioniRating.Count
        End If
    End Sub
    Private Sub LNBAddDays_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBAddDays.Click
        If ReadDomandaFromPage() Then
            initOption(True)
            VIWStep2_Load()
            MLVquestionari.SetActiveView(VIWStep2)
        Else
            LBErrorNoSelection.Visible = True
        End If
    End Sub
    Protected Sub RDCLCalendar_SelectionChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDatesEventArgs) Handles RDCLCalendar.SelectionChanged
        If isFirstStart Then
            RDCLCalendar.SelectedDates.Clear()
            isFirstStart = False
        End If
    End Sub
    Private Sub LNBDeleteDays_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBDeleteDays.Click
        deleteDays(DDLOptionNumber.SelectedIndex, VIWStep2)
        VIWStep2_Load()
    End Sub
#End Region
#Region "Step 3"
    Protected Sub bindList()
        Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
        If Me.UtentiInvitatiLista.Count = 0 Then
            GRVElenco.Visible = False
            Resource.setLabel(LBnoRecipient)
            LBnoRecipient.Visible = True
            CHKUtentiInvitati_WQ.Checked = True
        Else
            GRVElenco.Visible = True
            LBnoRecipient.Visible = False
            CHKUtentiInvitati_WQ.Checked = False
            GRVElenco.DataSource = Me.UtentiInvitatiLista
            GRVElenco.DataBind()

            'SetView(VIWattiva.VIWdettagli)
            'LNBIndietro.Visible = False
            'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            '    LNBIndietro.PostBackUrl = RootObject.SondaggioAdminShort + "?"&questType + Me.QuestionarioCorrente.tipo.ToString()
            'ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting Then
            '    LNBIndietro.PostBackUrl = RootObject.MeetingWizShort + "?"&questType + Me.QuestionarioCorrente.tipo.ToString()
            'Else
            '    LNBIndietro.PostBackUrl = RootObject.QuestionarioAdminShort + "?"&questType + Me.QuestionarioCorrente.tipo.ToString()
            'End If
            'LBAiutoDettagli.Visible = True
            'LBAiutoDettagli.Text = String.Format(Me.Resource.getValue("MSGnoUtentiInvitati"), tipoQuest)
        End If
    End Sub
    Private Property UtentiInvitatiLista() As List(Of UtenteInvitato)
        Get
            If Session("UtentiInvitatiLista") Is Nothing Then
                Return New List(Of UtenteInvitato)
            Else
                Return Session("UtentiInvitatiLista")
            End If
        End Get
        Set(ByVal value As List(Of UtenteInvitato))
            Session("UtentiInvitatiLista") = value
        End Set
    End Property
    Private Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVElenco.RowCommand
        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Me.UtenteInvitatoCorrenteID = GRVElenco.DataKeys(row.RowIndex).Value

            Select Case e.CommandName
                Case "Modifica"
                    MLVquestionari.SetActiveView(VIWaddExternalUser)
                    'Me.editMode = True
                    'BTNAggiungiUtente.Visible = False
                    'BindDati()
                Case "Elimina"
                    DALUtenteInvitato.UtenteInvitato_Delete(Me.UtenteInvitatoCorrenteID)
                    Me.UtenteInvitatoCorrenteID = 0
                    VIWStep3_Load()
                    'BindDati()
            End Select
        End If
    End Sub
    Private Sub GRVElenco_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        GRVElenco.DataSource = Me.UtentiInvitatiLista
        GRVElenco.DataBind()
    End Sub
    Private Sub GRVElenco_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVElenco.RowDataBound
        With Me.Resource
            .setImageButton(e.Row.FindControl("IMBGestione"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBElimina"), False, True, True, False)
        End With
    End Sub
    Private Sub VIWStep3_Load()
        LNBPreview.Visible = False
        bindList()
        CHKUtentiComunita_WQ.Checked = Me.QuestionarioCorrente.forUtentiComunita
        CHKUtentiNonComunita_WQ.Checked = Me.QuestionarioCorrente.forUtentiPortale
        CHKUtentiInvitati_WQ.Checked = Me.QuestionarioCorrente.forUtentiInvitati
        LBsubject.Text = String.Format(Resource.getValue("LBsubject.text"), Me.QuestionarioCorrente.nome)
        LBmessage.Text = String.Format(Resource.getValue("LBmessage.text"), UtenteCorrente.Anagrafica, Me.QuestionarioCorrente.nome, Resource.getValue("MSGlink"))

    End Sub
    Private Property UtenteInvitatoCorrenteID() As Integer
        Get
            UtenteInvitatoCorrenteID = ViewState("UtenteInvitatoCorrenteID")
        End Get
        Set(ByVal value As Integer)
            ViewState("UtenteInvitatoCorrenteID") = value
        End Set
    End Property
    Private Sub LNBaddSysUsers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaddSysUsers.Click
        VIWStep3_Unload()
        If Me.UtentiInvitatiLista.Count = 0 Then
            Me.QuestionarioCorrente.forUtentiInvitati = True
        End If
        Me.MLVquestionari.SetActiveView(VIWimportaDaComunita)
    End Sub
    Private Sub LNBaddExternalUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaddExternalUser.Click
        VIWStep3_Unload()
        If Me.UtentiInvitatiLista.Count = 0 Then
            Me.QuestionarioCorrente.forUtentiInvitati = True
        End If
        Me.MLVquestionari.SetActiveView(VIWaddExternalUser)
    End Sub
#End Region
#Region "VIWimportaDaComunita"
    Private Sub VIWimportaDaComunita_Load()
        LNBPreview.Visible = False
        Dim oCommunitiesIdList As New List(Of Integer)
        Dim oTempInvitedUserList As New List(Of UtenteInvitato)
        If Me.UtentiInvitatiLista.Count = 0 Then
            oTempInvitedUserList = DALUtenteInvitato.readUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
        Else
            oTempInvitedUserList = Me.UtentiInvitatiLista
        End If
        Dim oInvitedUserIdList As New List(Of Integer)
        For Each element As UtenteInvitato In oTempInvitedUserList
            If element.PersonaID > 0 Then
                oInvitedUserIdList.Add(element.PersonaID)
            End If
        Next
        'per ora si importano solo dalla comunita' corrente
        oCommunitiesIdList.Add(ComunitaCorrenteID)
        Me.UCsearchUser.CurrentPresenter.Init(oCommunitiesIdList, ListSelectionMode.Multiple, oInvitedUserIdList)
    End Sub
    Private Sub LNBconfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBconfirm.Click
        Dim oList As New List(Of MemberContact)
        Dim names As String = String.Empty
        oList = UCsearchUser.CurrentPresenter.GetConfirmedUsers()

        For Each oUser As MemberContact In oList
            Dim oUtente As New UtenteInvitato
            oUtente.PersonaID = oUser.Id
            oUtente.Cognome = oUser.Surname
            oUtente.Nome = oUser.Name
            oUtente.QuestionarioID = Me.QuestionarioCorrente.id
            oUtente.Mail = oUser.Mail
            DALUtenteInvitato.Salva(oUtente)
        Next
        MLVquestionari.SetActiveView(VIWStep3)
    End Sub
    Private Sub LNBCancelIDC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBCancelIDC.Click
        MLVquestionari.SetActiveView(VIWStep3)
    End Sub
#End Region
#Region "VIWaddExternalUser"
    Protected Sub bindUtente()
        Dim oUtente As New UtenteInvitato
        oUtente = DALUtenteInvitato.readUtenteInvitatoByID(Me.UtenteInvitatoCorrenteID)
        Dim listaUt As New List(Of UtenteInvitato)
        listaUt.Add(oUtente)
        FRVUtente.DataSource = listaUt
        FRVUtente.DataBind()
    End Sub
    Private Sub FRVUtente_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVUtente.DataBound
        With Me.Resource
            .setLabel(FRVUtente.FindControl("LBTitoloUtente"))
            .setLabel(FRVUtente.FindControl("LBCognome"))
            .setLabel(FRVUtente.FindControl("LBNome"))
            .setLabel(FRVUtente.FindControl("LBEmail"))
            .setLabel(FRVUtente.FindControl("LBDescrizione"))
            DirectCast(Me.FRVUtente.FindControl("RFVCognome"), RequiredFieldValidator).ErrorMessage = .getValue("CampoObbligatorio")
            DirectCast(Me.FRVUtente.FindControl("RFVNome"), RequiredFieldValidator).ErrorMessage = .getValue("CampoObbligatorio")
            DirectCast(Me.FRVUtente.FindControl("RFVEmail"), RequiredFieldValidator).ErrorMessage = .getValue("CampoObbligatorio")
        End With
    End Sub
    Private Sub VIWaddExternalUser_Load()
        With Me.Resource
            .setLinkButton(LNBSaveExternalUser, False, True)
            .setLinkButton(LNBCancelAEU, False, True)

        End With
        'inserimento o visualizzazione dei dati completi di un singolo utente invitato
        If Me.UtenteInvitatoCorrenteID = 0 Then
            Dim oUtente As New UtenteInvitato
            Dim listaUt As New List(Of UtenteInvitato)
            listaUt.Add(oUtente)
            FRVUtente.DataSource = listaUt
            FRVUtente.DataBind()
            Me.UtenteInvitatoCorrenteID = oUtente.ID
        Else
            bindUtente()
        End If
    End Sub
    Protected Sub LNBSaveExternalUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSaveExternalUser.Click
        Dim oUtente As New UtenteInvitato
        oUtente.ID = Me.UtenteInvitatoCorrenteID
        oUtente.Cognome = DirectCast(FRVUtente.FindControl("TXBCognome"), TextBox).Text
        oUtente.Nome = DirectCast(FRVUtente.FindControl("TXBNome"), TextBox).Text
        oUtente.QuestionarioID = Me.QuestionarioCorrente.id
        oUtente.Mail = DirectCast(FRVUtente.FindControl("TXBEmail"), TextBox).Text
        oUtente.Descrizione = DirectCast(FRVUtente.FindControl("TXBDescrizione"), TextBox).Text
        DALUtenteInvitato.Salva(oUtente)
        Me.UtenteInvitatoCorrenteID = 0
        Me.MLVquestionari.SetActiveView(VIWStep3)
    End Sub
    Private Sub LNBCancelAEU_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBCancelAEU.Click
        MLVquestionari.SetActiveView(VIWStep3)
    End Sub
#End Region
#Region "Mail"
    Private Sub VIWMail_Load()
        LNBPreview.Visible = False
        'template mail e invio
        'caricaCampiMail()
        'caricaTags()
        'caricaTemplate()
        LoadTemplatesList()
        CurrentPresenter.LoadTemplate()
        If Me.UtentiInvitatiLista.Count = 0 Then
            Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiNoMailByIDQuestionario(Me.QuestionarioCorrente.id)
            TXBDestinatario.Text = Me.Resource.getValue("TXBUtentiNonInvitati")
        End If
        Dim numeroUtenti As Integer = DALUtenteInvitato.countUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
        If Me.QuestionarioCorrente.isBloccato Then
            Me.LBMsgQuestionarioBloccato.Visible = True
            Me.LBMsgSbloccaInvia.Visible = Me.Servizio.Admin
            Me.LBMsgInvia.Visible = True
            Me.BTNSbloccaInvia.Visible = Me.Servizio.Admin
        Else
            Me.LBMsgQuestionarioBloccato.Visible = False
            Me.LBMsgSbloccaInvia.Visible = False
            Me.LBMsgInvia.Visible = False
            Me.BTNSbloccaInvia.Visible = False
        End If
    End Sub
    'Protected Sub caricaCampiMail()
    '    DDLTemplate.DataSource = DALTemplate.readListaTemplate(Me.UtenteCorrente.ID)
    '    DDLTemplate.DataBind()
    '   
    'End Sub
    'Protected Sub caricaTemplate()
    '    If Not (DDLTemplate.SelectedItem Is Nothing) Then
    '        Dim oTemplate As New Template
    '        oTemplate = DALTemplate.readTemplateByID(DDLTemplate.SelectedItem.Value)
    '        TXBNomeTemplate.Text = oTemplate.nome
    '        TXBTestoMessaggio.Text = oTemplate.testo
    '        TXBOggetto.Text = oTemplate.titolo
    '    Else
    '        TXBOggetto.Text = Me.Resource.getValue("TXBOggetto.default")
    '    End If
    'End Sub

    'Protected Sub LKBSelezionaUtenti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBSelezionaUtenti.Click
    '    TXBDestinatario.Text = String.Empty
    '    SetView(VIWdestinatari)
    'End Sub
    Protected Sub LKBAggiungiNonCompletati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBAggiungiNonCompletati.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiNonCompletati(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = String.Format(Me.Resource.getValue("TXBUtentiNonCompilati"), Me.Resource.getValue("Meeting"))
    End Sub
    Protected Sub LKBAggiungiTutti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBAggiungiTutti.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Me.Resource.getValue("TXBTuttiUtenti")
    End Sub
    Protected Sub LKBAggiungiNonInvitati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBAggiungiNonInvitati.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiNoMailByIDQuestionario(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Me.Resource.getValue("TXBUtentiNonInvitati")
    End Sub
    Protected Sub BTNInviaMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNInviaMail.Click
        inviaMail(False)

    End Sub
    Protected Sub BTNSbloccaInvia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSbloccaInvia.Click
        inviaMail(True)
    End Sub
 


    Private ReadOnly Property ListaTags() As List(Of TemplateTag)
        Get
            Return Me.SystemSettings.Tag.Questionario
        End Get
    End Property
    Public Function GetNested(ByVal WorkObj As Object, ByVal PropertiesList As String) As Object
        If WorkObj IsNot Nothing Then
            Dim WorkingType As Type = WorkObj.GetType
            Dim Properties() As String
            Dim WorkingObject As Object
            Properties = PropertiesList.Split(".")
            Try
                WorkingObject = WorkObj

                For i As Integer = 0 To Properties.Length - 1
                    Dim y As PropertyInfo = WorkingType.GetProperty(Properties(i))
                    WorkingObject = y.GetValue(WorkingObject, Nothing)
                    WorkingType = y.PropertyType
                Next

                Return WorkingObject
            Catch ex As Exception
                Return Nothing
            End Try
        Else
            Return Nothing
        End If

    End Function
    Private Sub LNBCancelMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBCancelMail.Click
        MLVquestionari.SetActiveView(VIWStep3)
    End Sub
#Region "tags"
    Public Sub AddScript()

        Dim code As String = "<script type=""text/javascript""> var arrayvars='{0}';</script>"

        Dim script As String = ""
        Dim vars As String = "#DESTINATARIO#={0}|#NOMEQUESTIONARIO#={1}|#LINKQUESTIONARIO#={2}|#DATAINIZIO#={3}|#DATAFINE#={4}|#DURATA#={5}|#AUTORE#={6}|#DESCRIZIONEQUESTIONARIO#={7}"

        Dim descrizione As String = String.Empty
        If Not Me.QuestionarioCorrente Is Nothing Then
            Me.QuestionarioCorrente.descrizione.Replace("<br>", "\n")
            'descrizione = descrizione.Replace("<p>", vbCrLf)
            descrizione = descrizione.Replace("’", "")
            descrizione = descrizione.Replace("'", "")
            descrizione = COL_Questionario.RootObject.StripHTML(descrizione)
        End If
        Dim varscoded As String = String.Format(vars, "Utente X", Me.QuestionarioCorrente.nome, "LINKQUESTIONARIO", Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.durata, Me.QuestionarioCorrente.creator, descrizione)

        script = String.Format(code, varscoded)

        LTRvariables.Text = script

    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        AddScript()
    End Sub
#End Region
#End Region
#Region "viwRiepilogo"
    Private Sub DLDomande_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles DLDomande.ItemCommand
        Dim groupIndex = CType(CType(e.CommandSource, Control).NamingContainer, DataListItem).ItemIndex
        Select Case e.CommandName
            Case "Edit"
                DDLOptionNumber.SelectedIndex = groupIndex
                Me.MLVquestionari.SetActiveView(VIWStep2)
            Case "Delete"
                deleteDays(groupIndex, VIWRiepilogo)
                VIWRiepilogo_Load()
        End Select
    End Sub
    Private Sub DLDomande_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLDomande.ItemDataBound
        With Me.Resource
            If nRisposte = 0 Then
                Dim lnbEdit As New LinkButton
                lnbEdit = e.Item.FindControl("LNBEdit")
                .setLinkButton(lnbEdit, False, True)
                lnbEdit.Visible = True
            End If
            If Me.QuestionarioCorrente.pagine(0).domande.Count > 1 Then
                .setLinkButton(e.Item.FindControl("LNBDelete"), False, True, , True)
            Else
                e.Item.FindControl("LNBDelete").Visible = False
            End If
        End With
    End Sub
#End Region

#Region "VIWMessaggi"
    Private Sub LNBBackToStep2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBBackToStep2.Click
        Me.MLVquestionari.SetActiveView(VIWStep2)
    End Sub
    Private Sub LNBBackToMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBBackToMail.Click
        Me.MLVquestionari.SetActiveView(VIWMail)
    End Sub
#End Region

    Private Sub LKBadvanced_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBadvanced.Click
        LNBSaveAndManageMail_Click(sender, e)
    End Sub

    Private Sub LKBsendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBsendMail.Click
        inviaMail(Me.QuestionarioCorrente.nome, String.Format(Resource.getValue("LBmessage.text"), UtenteCorrente.Anagrafica, Me.QuestionarioCorrente.nome, "#LINKQUESTIONARIO#") & TXBmessage.Text, UtentiInvitatiLista, False, True)
    End Sub

#Region "Implements"
    Public Sub DisplayNoPermission() Implements IViewInvitedUsers.DisplayNoPermission
        BindNoPermessi()
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewInvitedUsers.DisplaySessionTimeout
        BindNoPermessi()
    End Sub

    Public Function HasPermission() As Boolean Implements IViewInvitedUsers.HasPermission
        Return HasPermessi()
    End Function

    Public Function LoadQuestionnaire(id As Integer) As Questionario Implements IViewInvitedUsers.LoadQuestionnaire
        Return LoadQuestionnaireById(id)
    End Function

#Region "Mail"
    Private Sub inviaMail(ByVal sbloccaQuestionario As Boolean)
        If Not Me.CTRLmailEditor.Validate Then
            Exit Sub
        End If
        Dim dto As dtoMailContent = Me.CTRLmailEditor.Mail
        If Not IsNothing(dto) AndAlso Not String.IsNullOrEmpty(dto.Subject) AndAlso Not String.IsNullOrEmpty(dto.Body) Then
            If Not Me.UtentiInvitatiLista Is Nothing Then
                If Me.UtentiInvitatiLista.Count > 0 Then
                    Dim sentDate As DateTime = DateTime.Now
                    Dim sender
                    ' se il questionario è bloccato lo sblocco
                    If sbloccaQuestionario Then
                        DALQuestionario.IsBloccatoByIdQuestionario_Update(Me.QuestionarioCorrente.id, False)
                    End If
                    For Each oTag As TemplateTag In ListaTags.Where(Function(t) t.Fase = 1).ToList
                        Dim newBody As String
                        newBody = dto.Body.Replace(oTag.Tag, GetNested(Me.QuestionarioCorrente, oTag.Proprieta))
                        If oTag.isMandatory Then
                            If dto.Body = newBody Then
                                LBErroreNoTag.Visible = True
                                Exit Sub
                            End If
                        End If
                        dto.Subject = dto.Subject.Replace(oTag.Tag, GetNested(Me.QuestionarioCorrente, oTag.Proprieta))
                        dto.Body = newBody
                    Next

                    dto.Subject = dto.Subject.Replace("<br>", "")
                    dto.Subject = COL_Questionario.RootObject.StripHTML(dto.Subject)

                    Dim users As List(Of UtenteInvitato) = Me.UtentiInvitatiLista
                    Dim idLanguage As Integer = Me.QuestionarioCorrente.idLingua
                    Dim idQ As Integer = Me.QuestionarioCorrente.id
                    For Each user As UtenteInvitato In users
                        user.Url = Me.EncryptedUrl(COL_Questionario.RootObject.compileUrlUI, "idq=" & idQ & "&idu=" & user.ID & "&idl=" & idLanguage, SecretKeyUtil.EncType.Questionario)
                    Next
                    Dim beginMessage As String = Resource.getValue("Template.Begin." & QuestionnaireType.Meeting.ToString)
                    If String.IsNullOrWhiteSpace(beginMessage) Then
                        beginMessage = Resource.getValue("Template.Begin")
                    End If
                    beginMessage = beginMessage.Replace("#name#", QuestionarioCorrente.nome)
                    beginMessage = beginMessage.Replace("#sentdate#", GetDateTimeString(sentDate, DateTime.Now.ToShortDateString(), True))
                    CurrentPresenter.SendMail(dto, users, CHKInoltraMittente.Checked, beginMessage)
                    LBConfermaRiepilogo.Text = Me.Resource.getValue("MSGInvioMail")
                    MLVquestionari.SetActiveView(VIWRiepilogo)
                End If
            Else
                LBErrore.Text = Me.Resource.getValue("MSGnoInvioMail")
                MLVquestionari.SetActiveView(VIWMessaggi)
                LNBBackToMail.Visible = True
            End If
        Else
            LBErroreNoTag.Visible = True
            LBErroreNoTag.Text = Me.Resource.getValue("MSGOggettoObbligatorio")
            LNBBackToMail.Visible = True
        End If
    End Sub
    Private Sub inviaMail(ByRef subject As String, ByRef body As String, ByRef recipients As List(Of UtenteInvitato), Optional ByVal replaceTags As Boolean = True, Optional ByVal sbloccaQuestionario As Boolean = True, Optional ByVal forwardToSender As Boolean = False)
        Dim dto As dtoMailContent = New dtoMailContent()
        dto.Body = body
        dto.Subject = subject
        dto.Settings = New lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings() With {.CopyToSender = False, .IsBodyHtml = False, .NotifyToSender = False, .SenderType = lm.Comol.Core.MailCommons.Domain.SenderUserType.LoggedUser, .PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration}
        If recipients.Count > 0 Then
            Dim sentDate As DateTime = DateTime.Now
            ' se il questionario è bloccato lo sblocco
            If sbloccaQuestionario Then
                DALQuestionario.IsBloccatoByIdQuestionario_Update(Me.QuestionarioCorrente.id, False)
            End If
            For Each oTag As TemplateTag In ListaTags.Where(Function(t) t.Fase = 1).ToList
                Dim newBody As String
                newBody = dto.Body.Replace(oTag.Tag, GetNested(Me.QuestionarioCorrente, oTag.Proprieta))
                If oTag.isMandatory Then
                    If dto.Body = newBody Then
                        LBErroreNoTag.Visible = True
                        Exit Sub
                    End If
                End If
                dto.Subject = dto.Subject.Replace(oTag.Tag, GetNested(Me.QuestionarioCorrente, oTag.Proprieta))
                dto.Body = newBody
            Next

            dto.Subject = dto.Subject.Replace("<br>", "")
            dto.Subject = COL_Questionario.RootObject.StripHTML(dto.Subject)
            dto.Body = dto.Body.Replace("<br>", vbCrLf)
            dto.Body = COL_Questionario.RootObject.StripHTML(dto.Body)

            Dim users As List(Of UtenteInvitato) = Me.UtentiInvitatiLista
            Dim idLanguage As Integer = Me.QuestionarioCorrente.idLingua
            Dim idQ As Integer = Me.QuestionarioCorrente.id
            For Each user As UtenteInvitato In users
                user.Url = Me.EncryptedUrl(COL_Questionario.RootObject.compileUrlUI, "idq=" & idQ & "&idu=" & user.ID & "&idl=" & idLanguage, SecretKeyUtil.EncType.Questionario)
            Next
            Dim beginMessage As String = Resource.getValue("Template.Begin." & QuestionnaireType.Meeting.ToString)
            If String.IsNullOrWhiteSpace(beginMessage) Then
                beginMessage = Resource.getValue("Template.Begin")
            End If
            beginMessage = beginMessage.Replace("#name#", QuestionarioCorrente.nome)
            beginMessage = beginMessage.Replace("#sentdate#", GetDateTimeString(sentDate, DateTime.Now.ToShortDateString(), True))
            CurrentPresenter.SendMail(dto, users, forwardToSender, beginMessage)

            LBConfermaRiepilogo.Text = Me.Resource.getValue("MSGInvioMail")
            MLVquestionari.SetActiveView(VIWRiepilogo)

        Else
            LBErrore.Text = Me.Resource.getValue("MSGnoInvioMail")
            MLVquestionari.SetActiveView(VIWMessaggi)
            LNBBackToMail.Visible = True
        End If

    End Sub
    Private Sub CTRLmailEditor_UpdateView() Handles CTRLmailEditor.UpdateView
        Me.LBpreviewDisplay.Visible = True
        Me.LBpreviewDisplay.Text = Me.CTRLmailEditor.MailBodyPreview
    End Sub
    Private ReadOnly Property MSGUnSendedMail As String Implements IViewInvitedUsers.MSGUnSendedMail
        Get
            Return Me.Resource.getValue("MSGUnSendedMail")
        End Get
    End Property
    Private ReadOnly Property MSGSendedMail As String Implements IViewInvitedUsers.MSGSendedMail
        Get
            Return Me.Resource.getValue("MSGSendedMail")
        End Get
    End Property
    Function AnalyzeContent(ByVal content As String, user As UtenteInvitato) As String Implements IViewInvitedUsers.AnalyzeContent
        For Each oTag As TemplateTag In ListaTags.Where(Function(t) t.Fase = 2).ToList
            content = content.Replace(oTag.Tag, GetNested(user, oTag.Proprieta))
        Next
        Return content
    End Function

    'Private Sub BTNclosePreview_Click(sender As Object, e As System.EventArgs) Handles BTNclosePreview.Click
    '    Me.MLVquestionari.SetActiveView(VIWMail)
    'End Sub

    'Private Sub BTNpreview_Click(sender As Object, e As System.EventArgs) Handles BTNpreview.Click
    '    Me.LBpreview.Text = CTRLmailEditor.Mail.Body
    '    Dim items As New List(Of TranslatedItem(Of String))
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#DESTINATARIO#", .Translation = "Utente X"})
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#NOMEQUESTIONARIO#", .Translation = Me.QuestionarioCorrente.nome})
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#LINKQUESTIONARIO#", .Translation = "LINKQUESTIONARIO"})
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#DATAINIZIO#", .Translation = Me.QuestionarioCorrente.dataInizio})
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#DATAFINE#", .Translation = Me.QuestionarioCorrente.dataFine})
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#DURATA#", .Translation = Me.QuestionarioCorrente.durata})
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#AUTORE#", .Translation = Me.QuestionarioCorrente.creator})

    '    Dim descrizione As String = Me.QuestionarioCorrente.descrizione.Replace("<br>", "\n")
    '    descrizione = descrizione.Replace("’", "")
    '    descrizione = descrizione.Replace("'", "")
    '    descrizione = COL_Questionario.RootObject.StripHTML(descrizione)
    '    items.Add(New TranslatedItem(Of String) With {.Id = "#DESCRIZIONEQUESTIONARIO#", .Translation = descrizione})

    '    For Each item As TranslatedItem(Of String) In items.Where(Function(i) Me.LBpreview.Text.Contains(i.Id)).ToList
    '        Me.LBpreview.Text = Replace(Me.LBpreview.Text, item.Id, item.Translation)
    '    Next

    '    Me.MLVquestionari.SetActiveView(VIWpreview)
    'End Sub
#End Region

#Region "Templates"
    Private Sub LoadTemplates(items As Dictionary(Of Integer, String)) Implements IViewInvitedUsers.LoadTemplates
        DDLTemplate.DataSource = items
        DDLTemplate.DataTextField = "Value"
        DDLTemplate.DataValueField = "Key"
        DDLTemplate.DataBind()
    End Sub
    Private Sub LoadTemplatesList() Implements IViewInvitedUsers.LoadTemplatesList
        LoadTemplates(CurrentPresenter.LoadTemplates())
    End Sub
    Private Sub LoadTemplate(ByVal name As String, dto As dtoMailContent, senderEdit As Boolean, subjectEdit As Boolean) Implements IViewInvitedUsers.LoadTemplate
        If String.IsNullOrEmpty(dto.Subject) Then
            dto.Subject = IIf(ListaTags.Select(Function(t) t.Tag).Contains("#NOMEQUESTIONARIO#"), "#NOMEQUESTIONARIO#", "--")
        End If
        Dim attributes As List(Of TranslatedItem(Of String)) = ListaTags.Select(Function(t) New TranslatedItem(Of String) With {.Id = t.Tag, .Translation = t.Name}).ToList
        Dim mandatory As List(Of TranslatedItem(Of String)) = ListaTags.Where(Function(t) t.isMandatory).Select(Function(t) New TranslatedItem(Of String) With {.Id = t.Tag, .Translation = t.Name}).ToList

        Me.CTRLmailEditor.InitializeControl(dto, senderEdit, subjectEdit, attributes, mandatory, True)
        Me.LBTitoloTemplate.Text = name

        Me.LBpreviewDisplay.Text = Me.CTRLmailEditor.MailBodyPreview
    End Sub

#Region "PageControls"
    Protected Sub BTNLoadTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNLoadTemplate.Click
        CurrentPresenter.LoadTemplate()
        LoadTemplatesList()
        BTNSalvaTemplate.Visible = True
    End Sub
    Protected Sub BTNSalvaTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSalvaTemplate.Click
        Dim dto As dtoMailContent = CTRLmailEditor.Mail
        Dim template As New LazyTemplate With {.Id = SelectedIdTemplate}
        template.Name = TXBNomeTemplate.Text
        template.MailSettings = dto.Settings
        template.Subject = dto.Subject
        template.Body = dto.Body
        '  If CTRLmailEditor.Validate Then
        Me.CurrentPresenter.SaveTemplate(template)
        'End If


        'Me.RedirectToUrl(COL_Questionario.RootObject.UtentiInvitati)
    End Sub
    Protected Sub BTNSalvaTemplateConNome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSalvaTemplateConNome.Click
        If TXBNomeTemplate.Text = String.Empty Then
            LBErroreNoTag.Visible = True
            LBErroreNoTag.Text = Me.Resource.getValue("MSGErroreNoName")
        Else
            Dim dto As dtoMailContent = CTRLmailEditor.Mail
            Dim template As New LazyTemplate
            template.Name = TXBNomeTemplate.Text
            template.MailSettings = dto.Settings
            template.Subject = dto.Subject
            template.Body = dto.Body
            '  If CTRLmailEditor.Validate Then
            Me.CurrentPresenter.SaveTemplate(template)
            BTNSalvaTemplate.Visible = True
            '   End If
            'Me.RedirectToUrl(COL_Questionario.RootObject.UtentiInvitati)
        End If

    End Sub
    Protected Sub BTNElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNElimina.Click
        CurrentPresenter.DeleteTemplate(SelectedIdTemplate)
    End Sub
    Protected Sub BTNNuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNNuovo.Click
        Me.TXBNomeTemplate.Text = ""
        CurrentPresenter.NewTemplate()
        BTNSalvaTemplate.Visible = False
        Me.LBpreviewDisplay.Text = ""
        LoadTemplatesList()

    End Sub
#End Region


#End Region

    Protected Friend Function GetDateTimeString(ByVal datetime As DateTime?, defaultString As String, Optional removeZero As Boolean = False)
        If datetime.HasValue Then
            Dim time As String = GetTimeToString(datetime, defaultString, removeZero)
            If String.IsNullOrEmpty(time) Then
                Return GetDateToString(datetime, defaultString)
            Else
                Return GetDateToString(datetime, defaultString) & " " & time
            End If
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetDateToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Dim pattern As String = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
            If (pattern.Contains("yyyy")) Then
                pattern = pattern.Replace("yyyy", "yy")
            End If
            Return datetime.Value.ToString(pattern)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetTimeToString(ByVal datetime As DateTime?, defaultString As String, Optional removeZero As Boolean = False)
        If datetime.HasValue Then
            If removeZero AndAlso datetime.Value.Minute = 0 Then
                Return ""
            Else
                Return datetime.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
            End If
        Else
            Return defaultString
        End If
    End Function
#End Region
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class