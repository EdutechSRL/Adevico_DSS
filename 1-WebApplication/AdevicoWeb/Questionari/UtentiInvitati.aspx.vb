Imports COL_Questionario
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.Comol.Configurations
Imports COL_BusinessLogic_v2.Comol.Entities.Configuration
Imports Comol.Entity.Configuration
Imports System.Reflection
Imports lm.Comol.Core.File
Imports lm.Comol.Core.Mail
Imports lm.Comol.Core.DomainModel

Partial Public Class UtentiInvitati
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
            If IsNothing(QuestionarioCorrente) Then
                Return 0
            Else
                Return QuestionarioCorrente.id
            End If
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

#Region "Page"
    Private _ObjectPath As ObjectFilePath
    Private _tipoQuest As String
    Private _QuestionnaireType As QuestionnaireType = QuestionnaireType.Compilabili
    Private Property tipoQuest() As String
        Get
            If _tipoQuest = String.Empty Then
                Dim type As QuestionnaireType = QuestionnaireType.Standard
                Try
                    type = QuestionarioCorrente.tipo
                Catch ex As Exception

                End Try
                _tipoQuest = Resource.getValue("QuestionnaireType." & type.ToString)
            End If
            Return _tipoQuest
        End Get
        Set(ByVal value As String)
            _tipoQuest = value
        End Set
    End Property
    Private Property CurrentType() As QuestionnaireType
        Get
            If _QuestionnaireType = QuestionnaireType.Compilabili Then
                Try
                    _QuestionnaireType = QuestionarioCorrente.tipo
                Catch ex As Exception
                    _QuestionnaireType = QuestionnaireType.Standard
                End Try
            End If
            Return _tipoQuest
        End Get
        Set(ByVal value As QuestionnaireType)
            _QuestionnaireType = value
        End Set
    End Property
    Public Enum VIWattiva As Short
        VIWdestinatari = 0
        VIWgestioneUI = 1
        VIWdettagli = 2
        VIWMail = 3
        VIWupload = 4
        VIWnoPermessi = 5
        VIWconferma = 6
        VIWerrore = 7
        VIWStampa = 8
        VIWimportaUtenti = 9
    End Enum
    Public Property editMode() As Boolean
        Get
            Return ViewState("editMode")
        End Get
        Set(ByVal value As Boolean)
            ViewState("editMode") = value
        End Set
    End Property
    Public ReadOnly Property ListaDrivePath() As String
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.ObjectPath(Me.SystemSettings.BulkInsert.Questionario)
            End If
            Path = _ObjectPath.Drive
            Path = Replace(Path, "\/", "\")
            Return Path
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
    Public Shared Property testoAnteprima() As String
        Get
            Return testoAnteprima
        End Get
        Set(ByVal value As String)
            testoAnteprima = value
        End Set
    End Property
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property
#End Region

    Public Overrides Sub BindDati()
        IMBHelpDettagli.Attributes.Add("onclick", COL_Questionario.RootObject.apriPopUp(COL_Questionario.RootObject.helpUtentiInvitati_dettagli, "target", "yes", "yes"))
        If Me.UtenteInvitatoCorrenteID > 0 Then
            SetView(VIWattiva.VIWdettagli)
        Else
            SetView(VIWattiva.VIWgestioneUI)
        End If

    End Sub
    Public Overrides Sub BindNoPermessi()
        SetView(VIWattiva.VIWnoPermessi)
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        PNLgeneraQuestionari.Visible = Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random
    End Sub
    Public Sub SetView(ByVal VIWcorrente As VIWattiva)


        GRVElenco.Columns(_COL_Url).Visible = Me.ShowUrl()
        GRVElenco.Columns(_COL_Description).Visible = Me.ShowDescription()
        

        'Me.viewPrecedente = VIWcorrente
        'Select Case Me.viewPrecedente_readOnly 'in questo modo per qualche motivo viewprecedente readonly cancella la variabile in sessione
        Select Case VIWcorrente
            Case VIWattiva.VIWdettagli
                'inserimento o visualizzazione dei dati completi di un singolo utente invitato
                Me.MLVquestionari.SetActiveView(Me.VIWdettagli)
                LNBGestioneMail.Visible = True
                LNBQuestionarioAdmin.Visible = False
                LNBNuovoUtente.Visible = False
                LNBImportaCSV.Visible = True
                LNBcommunityImport.Visible = True
                LNBSalva.Visible = True
                LNBGestioneRubrica.Visible = True
                LNBSelezionaDestinatari.Visible = False
                'LNKStampa.Visible = False
                LBlistaMessaggi.Visible = False
                Me.viewPrecedente = VIWattiva.VIWdettagli

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

            Case VIWattiva.VIWdestinatari
                'elenco UI con CB di selezione dei destinatari della mail
                bindList()
                Me.MLVquestionari.SetActiveView(Me.VIWlista)
                GRVElenco.Columns(_COL_CheckBox).Visible = True
                GRVElenco.Columns(_COL_Commands).Visible = False

                GRVElenco.Columns(_COL_Url).Visible = False
                GRVElenco.Columns(_COL_Description).Visible = False

                LNKConfermaUtenti.Visible = True
                LNBGestioneMail.Visible = True
                LNBQuestionarioAdmin.Visible = False
                LNBNuovoUtente.Visible = True
                LNBImportaCSV.Visible = True
                LNBcommunityImport.Visible = True
                LNBSalva.Visible = False
                LNBGestioneRubrica.Visible = True
                LNBSelezionaDestinatari.Visible = False
                'LNKStampa.Visible = False
                CHKisPassword.Visible = False
                PNLgeneraQuestionari.Visible = False
                LBlistaMessaggi.Visible = False
                Me.viewPrecedente = VIWattiva.VIWdestinatari

            Case VIWattiva.VIWgestioneUI
                'elenco UI con icone per modifica ed eliminazione singolo invito
                Me.MLVquestionari.SetActiveView(Me.VIWlista)
                GRVElenco.Columns(_COL_CheckBox).Visible = False 'colonna CheckBox
                GRVElenco.Columns(_COL_Commands).Visible = True 'colonna controlli edit/cancellazione
                LNKConfermaUtenti.Visible = False
                LNBGestioneMail.Visible = True
                LNBQuestionarioAdmin.Visible = True
                LNBNuovoUtente.Visible = True
                LNBImportaCSV.Visible = True
                LNBcommunityImport.Visible = True
                LNBSalva.Visible = False
                LNBGestioneRubrica.Visible = False
                LNBSelezionaDestinatari.Visible = False
                'LNKStampa.Visible = True
                CHKisPassword.Visible = True

                Me.viewPrecedente = VIWattiva.VIWgestioneUI
                bindList()
                CHKisPassword.Checked = Me.QuestionarioCorrente.isPassword

            Case VIWattiva.VIWMail
                'template mail e invio
                LoadTemplatesList()
                CurrentPresenter.LoadTemplate()
                Me.MLVquestionari.SetActiveView(Me.VIWMail)
                LNBGestioneMail.Visible = False
                LNBQuestionarioAdmin.Visible = True
                LNBNuovoUtente.Visible = False
                LNBImportaCSV.Visible = False
                LNBcommunityImport.Visible = False
                LNBSalva.Visible = False
                LNBGestioneRubrica.Visible = True
                LNBSelezionaDestinatari.Visible = True
                'LNKStampa.Visible = False
                Dim numeroUtenti As Integer = DALUtenteInvitato.countUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
                If numeroUtenti = 0 Then
                    Me.LNKConfermaUtenti.Visible = False
                End If
                Me.viewPrecedente = VIWattiva.VIWMail
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

            Case VIWattiva.VIWupload
                'importazione UI da CSV
                Me.MLVquestionari.SetActiveView(Me.VIWupload)
                LNBGestioneMail.Visible = True
                LNBQuestionarioAdmin.Visible = False
                LNBNuovoUtente.Visible = False
                LNBImportaCSV.Visible = False
                LNBcommunityImport.Visible = False
                LNBSalva.Visible = False
                LNBGestioneRubrica.Visible = True
                LNBSelezionaDestinatari.Visible = False
                'LNKStampa.Visible = False
                Me.viewPrecedente = VIWattiva.VIWupload

            Case VIWattiva.VIWnoPermessi
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                PNLmenu.Visible = False
                LBconferma.Visible = False
                LBerrore.Visible = True
                'LNKStampa.Visible = False
                LBerrore.Text = Me.Resource.getValue("MSGnoPermessi")
                Me.viewPrecedente = VIWattiva.VIWnoPermessi

            Case VIWattiva.VIWconferma
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                LBconferma.Visible = True
                LBerrore.Visible = False
                LNBGestioneMail.Visible = False
                LNBQuestionarioAdmin.Visible = False
                LNBNuovoUtente.Visible = False
                LNBImportaCSV.Visible = False
                LNBcommunityImport.Visible = False
                LNBSalva.Visible = False
                LNBGestioneRubrica.Visible = False
                LNBSelezionaDestinatari.Visible = False
                'LNKStampa.Visible = False
                Me.viewPrecedente = VIWattiva.VIWconferma

            Case VIWattiva.VIWerrore
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                LBconferma.Visible = False
                LBerrore.Visible = True
                LNBGestioneMail.Visible = True
                LNBQuestionarioAdmin.Visible = False
                LNBNuovoUtente.Visible = False
                LNBImportaCSV.Visible = False
                LNBSalva.Visible = False
                LNBGestioneRubrica.Visible = False
                LNBSelezionaDestinatari.Visible = False
                'LNKStampa.Visible = False
                Me.viewPrecedente = VIWattiva.VIWerrore

            Case VIWattiva.VIWStampa
                'inserimento o visualizzazione dei dati completi di un singolo utente invitato
                LNBGestioneMail.Visible = False
                LNBQuestionarioAdmin.Visible = False
                LNBNuovoUtente.Visible = False
                LNBImportaCSV.Visible = False
                LNBcommunityImport.Visible = False
                LNBSalva.Visible = False
                LNBGestioneRubrica.Visible = False
                LNBSelezionaDestinatari.Visible = False
                'LNKStampa.Visible = False
                Me.MLVquestionari.SetActiveView(Me.VIWStampa)
                Me.viewPrecedente = VIWattiva.VIWStampa
            Case VIWattiva.VIWimportaUtenti
                LBlistaMessaggi.Visible = False
                Me.MLVquestionari.SetActiveView(Me.VIWimportaDaComunita)
                Me.viewPrecedente = VIWattiva.VIWimportaUtenti
                Dim oCommunitiesIdList As New List(Of Integer)
                Dim oTempInvitedUserList As New List(Of UtenteInvitato)
                If Me.UtentiInvitatiLista Is Nothing Or Me.UtentiInvitatiLista.Count = 0 Then
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
                Me.Page.Form.DefaultButton = Me.UCsearchUser.SearchButtonUniqueID
                Me.Page.Form.DefaultFocus = Me.UCsearchUser.SearchDefaultTextField

        End Select
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.QuestionariSuInvito)
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UtentiInvitati", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle")
            .setLinkButton(LNBGestioneMail, False, False)
            .setLinkButton(LNBQuestionarioAdmin, False, False)

            .setLinkButton(LNBNuovoUtente, False, False)
            .setLinkButton(LNBImportaCSV, False, False)
            .setLinkButton(LNBcommunityImport, False, False)
            .setLinkButton(LNBSalva, False, False)
            .setLinkButton(LNBGestioneRubrica, False, False)
            .setLinkButton(LKBAggiungiTutti, False, False)
            .setLinkButton(LKBAggiungiNonInvitati, False, False)
            .setLinkButton(LKBSelezionaUtenti, False, False)

            .setLinkButton(LNKConfermaUtenti, False, False)
            .setLinkButton(LNBSelezionaDestinatari, False, False)
            .setLinkButton(LNBIndietro, False, False)
            .setLabel(LBAnteprima)
            .setLabel(LBerrore)
            .setLabel(LBAiutoMail)
            .setLabel(LBAiutoDettagli)
            .setLabel(LBTitoloTemplate)
            .setLabel(LBErroreNoTag)
            .setLabel(LBDownload)
            .setButton(BTNElimina)
            .setButton(BTNNuovo)
            .setButton(BTNconfirm)
            .setButton(BTNAggiungiUtente)
            .setLabel(LBDestinatario)
            .setLinkButton(LKBSelezionaUtenti, False, False)

            .setHeaderGridView(Me.GRVElenco, _COL_Cognome, "headerCognome", True)
            .setHeaderGridView(Me.GRVElenco, _COL_Nome, "headerNome", True)
            .setHeaderGridView(Me.GRVElenco, _COL_Mail, "headerEmail", True)

            'ToDo: Check e XML
            .setHeaderGridView(Me.GRVElenco, _COL_Description, "headerDescription", True)
            .setHeaderGridView(Me.GRVElenco, _COL_Url, "headerUrl", True)


            .setLabel(LBTerminatore)
            .setButton(BTNSalvaTemplateConNome)
            .setButton(BTNSalvaTemplate)
            .setCheckBox(CHKInoltraMittente)
            LBMsgSbloccaInvia.Text = .getValue("LBMsgSbloccaInvia." & CurrentType.ToString)
            LNBQuestionarioAdmin.Text = .getValue("LNBQuestionarioAdmin." & CurrentType.ToString)
            CHKisPassword.Text = .getValue("CHKisPassword." & CurrentType.ToString)
            LBTitolo.Text = .getValue("LBTitolo." & CurrentType.ToString)
            LBgeneraQuestionari.Text = .getValue("LBgeneraQuestionari." & CurrentType.ToString)
            LBMsgQuestionarioBloccato.Text = .getValue("LBMsgQuestionarioBloccato." & CurrentType.ToString)
            LBMsgInvia.Text = .getValue("LBMsgInvia." & CurrentType.ToString)
            LBStampaTutti.Text = .getValue("LBStampaTutti." & CurrentType.ToString)
            LBStampaUtentiDomande.Text = .getValue("LBStampaUtentiDomande." & CurrentType.ToString)
            LBStampaSelezionati.Text = .getValue("LBStampaSelezionati." & CurrentType.ToString)
            .setButton(BTNInviaMail, False, False, False, False)
            .setButton(BTNSbloccaInvia, False, False, False, False)
            .setButton(Me.BTNcancel, True)
            .setLabel(LBUploadFile)
            .setButton(BTNUpload)
            .setButton(BTNimportaDaComunita)
            .setButton(BTNgeneraQuestionari)
            '.setLinkButton(LNKStampa, False, False)
            .setLinkButton(LNKStampaTutti, False, False)
            .setLinkButton(LNKStampaUtentiDomande, False, False)
            .setLinkButton(LNKStampaSelezionati, False, False)
            .setButton(BTNpreview, True, , , True)
            .setButton(BTNclosePreview, True, , , True)
            .setLinkButtonToValue(LNBaddNotCompleted, DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString, False, True)
            If String.IsNullOrEmpty(LNBaddNotCompleted.Text) Then
                .setLinkButton(LNBaddNotCompleted, False, True)
            End If
            .setLinkButtonToValue(LNBaddNotStarted, DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString, False, True)
            If String.IsNullOrEmpty(LNBaddNotStarted.Text) Then
                .setLinkButton(LNBaddNotStarted, False, True)
            End If
            .setLinkButtonToValue(LNBaddNotStartedNotCompleted, DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString, False, True)
            If String.IsNullOrEmpty(LNBaddNotStartedNotCompleted.Text) Then
                .setLinkButton(LNBaddNotStartedNotCompleted, False, True)
            End If
            .setLinkButtonToValue(LNBaddCompleted, DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString, False, True)
            If String.IsNullOrEmpty(LNBaddCompleted.Text) Then
                .setLinkButton(LNBaddCompleted, False, True)
            End If
        End With
    End Sub
    Protected Sub setInternazionalizzazioneGRVElencoControls(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        With Me.Resource
            .setImageButton(e.Row.FindControl("IMBGestione"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBElimina"), False, True, True, False)
        End With
    End Sub
    Protected Sub bindUtente()
        Dim oUtente As New UtenteInvitato
        oUtente = DALUtenteInvitato.readUtenteInvitatoByID(Me.UtenteInvitatoCorrenteID)
        Dim listaUt As New List(Of UtenteInvitato)
        listaUt.Add(oUtente)
        FRVUtente.DataSource = listaUt
        FRVUtente.DataBind()
    End Sub
    Protected Sub LNBNuovoUtente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNuovoUtente.Click
        SetView(VIWattiva.VIWdettagli)
    End Sub
    Private Sub LNBImportaCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBImportaCSV.Click
        PageUtility.RedirectToUrl("Questionari/ImportUsersFromCSV.aspx")
        '' da riattivare quando si sistema importazione
        ''SetView(VIWattiva.VIWupload)

        '' da disattivare quando si sistema importazione
        'BTNimportaDaComunita_Click(sender, e)
    End Sub
    Protected Sub LNBcommunityImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcommunityImport.Click
        SetView(VIWattiva.VIWimportaUtenti)
    End Sub

    Protected Sub LNBSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva.Click
        Dim oUtente As New UtenteInvitato
        oUtente.ID = Me.UtenteInvitatoCorrenteID
        oUtente.Cognome = DirectCast(FRVUtente.FindControl("TXBCognome"), TextBox).Text
        oUtente.Nome = DirectCast(FRVUtente.FindControl("TXBNome"), TextBox).Text
        oUtente.QuestionarioID = Me.QuestionarioCorrente.id
        oUtente.Mail = DirectCast(FRVUtente.FindControl("TXBEmail"), TextBox).Text
        oUtente.Descrizione = DirectCast(FRVUtente.FindControl("TXBDescrizione"), TextBox).Text
        DALUtenteInvitato.Salva(oUtente)
        Me.UtenteInvitatoCorrenteID = 0
        SetView(Me.viewPrecedente)
    End Sub
    Protected Sub BTNAggiungiUtente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNAggiungiUtente.Click
        LNBSalva_Click(sender, e)
    End Sub
    Private Property UtenteInvitatoCorrenteID() As Integer
        Get
            UtenteInvitatoCorrenteID = ViewState("UtenteInvitatoCorrenteID")
        End Get
        Set(ByVal value As Integer)
            ViewState("UtenteInvitatoCorrenteID") = value
        End Set
    End Property
    Private Property UtentiInvitatiLista() As List(Of UtenteInvitato)
        Get
            UtentiInvitatiLista = ViewStateOrDefault("UtenteInvitatiLista", New List(Of UtenteInvitato))
        End Get
        Set(ByVal value As List(Of UtenteInvitato))
            ViewState("UtenteInvitatiLista") = value
        End Set
    End Property
    Private ReadOnly Property ListaTags() As List(Of TemplateTag)
        Get
            Return Me.SystemSettings.Tag.Questionario
        End Get
    End Property

#Region "GRVElenco"
    Private Sub GRVElenco_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRVElenco.DataBound
        'Each time the data is bound to the grid we need to build up the CheckBoxIDs array
        If GRVElenco.Rows.Count > 0 Then
            'Get the header CheckBox
            Dim cbHeader As CheckBox = CType(GRVElenco.HeaderRow.FindControl("CHKInvitaHeader"), CheckBox)

            'Run the ChangeCheckBoxState client-side function whenever the
            'header checkbox is checked/unchecked
            cbHeader.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"

            'Add the CheckBox's ID to the client-side CheckBoxIDs array
            Dim ArrayValues As New List(Of String)
            ArrayValues.Add(String.Concat("'", cbHeader.ClientID, "'"))

            For Each gvr As GridViewRow In GRVElenco.Rows
                'Get a programmatic reference to the CheckBox control
                Dim cb As CheckBox = CType(gvr.FindControl("CHKInvitaRow"), CheckBox)

                'If the checkbox is unchecked, ensure that the Header CheckBox is unchecked
                cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"

                'Add the CheckBox's ID to the client-side CheckBoxIDs array
                ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
            Next

            'Output the array to the Literal control (CheckBoxIDsArray)
            CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf & _
               "<!--" & vbCrLf & _
               String.Concat("var CheckBoxIDs =  new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf & _
               "// -->" & vbCrLf & _
               "</script>"
        End If

    End Sub
    Protected Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVElenco.RowCommand
        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Me.UtenteInvitatoCorrenteID = GRVElenco.DataKeys(row.RowIndex).Value

            Select Case e.CommandName
                Case "Modifica"
                    Me.editMode = True
                    BTNAggiungiUtente.Visible = False
                    BindDati()
                Case "Elimina"
                    DALUtenteInvitato.UtenteInvitato_Delete(Me.UtenteInvitatoCorrenteID)
                    Me.UtenteInvitatoCorrenteID = 0
                    BindDati()
            End Select
        End If
    End Sub
    Protected Sub GRVElenco_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        selezionaUtenti()

        GRVElenco.DataSource = Me.UtentiInvitatiLista
        GRVElenco.DataBind()
    End Sub
    Private Sub GRVElenco_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVElenco.RowDataBound
        setInternazionalizzazioneGRVElencoControls(e)
    End Sub
    Protected Sub selezionaUtenti()
        Dim iRiga As Integer = 0
        For Each gvRow As GridViewRow In GRVElenco.Rows
            Dim chkInvita As New CheckBox
            chkInvita = DirectCast(gvRow.Cells(0).FindControl("CHKInvitaRow"), CheckBox)
            If chkInvita.Checked Then
                Dim utenteSelezionato As New UtenteInvitato
                utenteSelezionato = UtenteInvitato.findUtenteBYID(Me.UtentiInvitatiLista, GRVElenco.DataKeys(iRiga).Value)
                utenteSelezionato.isSelezionato = True
            End If
            iRiga = iRiga + 1
        Next
    End Sub
    Protected Sub bindList()
        LBTitolo.Text = " " + Me.QuestionarioCorrente.nome
        Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
        If Me.UtentiInvitatiLista.Count = 0 Then
            SetView(VIWattiva.VIWdettagli)
            LNBIndietro.Visible = False
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
                LNBIndietro.PostBackUrl = RootObject.SondaggioAdminShort + "?type=" + Me.QuestionarioCorrente.tipo.ToString()
            ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting Then
                LNBIndietro.PostBackUrl = RootObject.MeetingWizShort + "?type=" + Me.QuestionarioCorrente.tipo.ToString()
            Else
                LNBIndietro.PostBackUrl = RootObject.QuestionarioAdminShort + "?type=" + Me.QuestionarioCorrente.tipo.ToString()
            End If
            LBAiutoDettagli.Visible = True
            LBAiutoDettagli.Text = String.Format(Me.Resource.getValue("MSGnoUtentiInvitati"), tipoQuest)
        Else

            If (Me.ShowUrl) Then
                Me.GenerateUrl()
            End If

            GRVElenco.DataSource = Me.UtentiInvitatiLista
            GRVElenco.DataBind()
        End If
    End Sub
#End Region

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

    Protected Sub LNBQuestionarioAdmin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBQuestionarioAdmin.Click
        Me.RedirectToUrl(COL_Questionario.RootObject.QuestionarioAdmin + "?type=" + Me.QuestionarioCorrente.tipo.ToString() & "&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua)
    End Sub
    Protected Sub LNBGestioneRubrica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneRubrica.Click
        Me.UtenteInvitatoCorrenteID = 0
        Me.editMode = True
        SetView(VIWattiva.VIWgestioneUI)
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If MLVquestionari.Views.Item(MLVquestionari.ActiveViewIndex).UniqueID = VIWMail.UniqueID Then 'al primo accesso vengono caricati sui "setView"
            'caricaTags()
            Me.LBpreviewDisplay.Text = Me.CTRLmailEditor.MailBodyPreview
        End If
        If Not Page.IsPostBack Then
            LNBIndietro.Visible = False
            LNBQuestionarioAdmin.Visible = True
            If SessionIdQuestionnnaire <> PreloadedIdQuestionnnaire Then
                LoadQuestionnaireById(PreloadedIdQuestionnnaire)
                CurrentIdQuestionnnaire = SessionIdQuestionnnaire
            End If
        End If
    End Sub

    'Private Sub BTNUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNUpload.Click
    '    If FLUcsv.HasFile Then
    '        Dim path As String
    '        Dim oImpersonate As lm.Comol.Core.File.Impersonate
    '        path = Me.ListaDrivePath & FLUcsv.FileName & Now.Ticks.ToString
    '        Try
    '            oImpersonate.ImpersonateValidUser()
    '            FLUcsv.SaveAs(path)
    '            DALUtenteInvitato.importaCSV(path, TXBTerminatore.Text, Me.QuestionarioCorrente.id)
    '            LBconferma.Text = String.Format(Me.Resource.getValue("LBConferma.text"), FLUcsv.PostedFile.FileName)
    '            SetView(VIWattiva.VIWconferma)
    '            LNBSelezionaDestinatari.Visible = True
    '        Catch ex As Exception
    '            oImpersonate.UndoImpersonation()
    '            LBerrore.Text = Me.Resource.getValue("MSGNoUpload")
    '            SetView(VIWattiva.VIWerrore)
    '            '   File.Delete(path)
    '        Finally
    '            oImpersonate.UndoImpersonation()
    '        End Try
    '        Delete.File(path)
    '    Else
    '        LBerrore.Text = "File non trovato."
    '        SetView(VIWattiva.VIWerrore)
    '    End If
    'End Sub
    Private Sub LNBIndietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBIndietro.Click
        SetView(viewPrecedente)
    End Sub
    Protected Sub CHKisPassword_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CHKisPassword.CheckedChanged
        Me.QuestionarioCorrente.isPassword = CHKisPassword.Checked
        DALQuestionario.updateIsPassword(Me.QuestionarioCorrente.id, CHKisPassword.Checked)
    End Sub
    Protected Sub BTNgeneraQuestionari_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNgeneraQuestionari.Click
        Dim gestioneQuest As New GestioneQuestionario
        gestioneQuest.generaQuestionariRandomDestinatario(Me.UtentiInvitatiLista)

        SetView(VIWattiva.VIWconferma)
        LBconferma.Visible = True
        LBconferma.Text = Me.Resource.getValue("MSGGenerazioneOK")
        LNBGestioneMail.Visible = True
        LNBGestioneRubrica.Visible = True
    End Sub
    'Protected Sub LNKStampa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNKStampa.Click
    '    SetView(VIWattiva.VIWStampa)
    'End Sub
    'Protected Sub LNKStampaTutti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNKStampaTutti.Click
    '    Dim oGS As New GestioneStampa

    '    Dim oUtenti As New List(Of UtenteInvitato)
    '    oUtenti = DALUtenteInvitato.readUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)

    '    oGS.creaReportElencoUtenti(oUtenti)

    'End Sub
    'Protected Sub LNKStampaUtentiDomande_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNKStampaUtentiDomande.Click

    '    Dim oGS As New GestioneStampa
    '    BTNgeneraQuestionari_Click(sender, e)
    '    oGS.creaReportFogliUtentiInvitatiDomande(Me.UtentiInvitatiLista, PageUtility.ApplicationUrlBase(True))

    'End Sub
    'Protected Sub LNKStampaSelezionati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNKStampaSelezionati.Click
    '    SetView(VIWattiva.VIWdestinatari)
    '    LNKConfermaUtenti.Visible = False
    '    LNKStampaUtenti.Visible = True
    'End Sub
    'Protected Sub LNKStampaUtenti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNKStampaUtenti.Click
    '    selezionaUtenti()
    '    UtenteInvitato.removeUtentiNonSelezionati(Me.UtentiInvitatiLista)
    '    BTNgeneraQuestionari_Click(sender, e)
    '    Dim oGS As New GestioneStampa
    '    oGS.creaReportFogliUtentiInvitatiDomande(Me.UtentiInvitatiLista, PageUtility.ApplicationUrlBase(True))
    'End Sub
    Private Sub BTNimportaDaComunita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNimportaDaComunita.Click
        SetView(VIWattiva.VIWimportaUtenti)

    End Sub


#Region "mail"
#Region "lista utenti"
    Private Sub updateListaUtentiInvitati(ByRef IdList As Generic.List(Of Integer))
        Dim OldSelectedList As Generic.List(Of Integer)
        Try
            For Each oUtente As UtenteInvitato In Me.UtentiInvitatiLista
                If oUtente.PersonaID > 0 Then
                    OldSelectedList.Add(oUtente.PersonaID)
                End If
            Next
        Catch ex As Exception
        End Try

        If Not IsNothing(OldSelectedList) Then
            If OldSelectedList.Count > 0 Then

                'Controllo ogni nuovo elemento, se e' gia' presente lo tolgo da quelli da inserire
                For Each Id As Integer In IdList
                    If Id > 0 Then
                        If OldSelectedList.Contains(Id) Then
                            IdList.Remove(Id)
                        End If
                    End If
                Next
            End If
        End If

        'Aggiungo quelli che non sono ancora stati aggiunti
        For Each Id As Integer In IdList
            If Id > 0 Then
                Dim oUtenteInvitato As New UtenteInvitato
                oUtenteInvitato.PersonaID = Id
                oUtenteInvitato.QuestionarioID = Me.QuestionarioCorrente.id

                Me.UtentiInvitatiLista.Add(oUtenteInvitato)
                DALUtenteInvitato.UtenteInvitato_Insert(oUtenteInvitato)
            End If
        Next
    End Sub

    Private Sub BTNconfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconfirm.Click

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
        SetView(VIWattiva.VIWgestioneUI)
        If oList.Count > 0 Then
            LBlistaMessaggi.Visible = True
            LBlistaMessaggi.Text = Me.Resource.getValue("MSGconfermaImportazioneComOL")
        End If
    End Sub
#End Region
#Region "tags"
    Public Sub AddScript()

        Dim code As String = "<script type=""text/javascript""> var arrayvars='{0}';</script>"

        Dim script As String = ""
        Dim vars As String = "#DESTINATARIO#={0}|#NOMEQUESTIONARIO#={1}|#LINKQUESTIONARIO#={2}|#DATAINIZIO#={3}|#DATAFINE#={4}|#DURATA#={5}|#AUTORE#={6}|#DESCRIZIONEQUESTIONARIO#={7}"



        Dim descrizione As String = ""

        'Aggiunto o rischia di andare in CATCH!
        If Not String.IsNullOrEmpty(Me.QuestionarioCorrente.descrizione) Then
            descrizione = Me.QuestionarioCorrente.descrizione.Replace("<br>", "\n")

            descrizione = descrizione.Replace("’", "")
            descrizione = descrizione.Replace("'", "")

            descrizione = COL_Questionario.RootObject.StripHTML(descrizione)

        End If

        'descrizione = descrizione.Replace("<p>", vbCrLf)


        Dim varscoded As String = String.Format(vars, "Utente X", Me.QuestionarioCorrente.nome, "LINKQUESTIONARIO", Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.durata, Me.QuestionarioCorrente.creator, descrizione)

        script = String.Format(code, varscoded)

        LTRvariables.Text = script
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LNBIndietro.Visible = viewPrecedente_isVisible()
        AddScript()
    End Sub
#End Region


    Protected Sub BTNInviaMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNInviaMail.Click

        inviaMail(False)

    End Sub
    Protected Sub BTNSbloccaInvia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSbloccaInvia.Click
        inviaMail(True)
    End Sub

    Protected Sub LNKConfermaUtenti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNKConfermaUtenti.Click
        selezionaUtenti()
        UtenteInvitato.removeUtentiNonSelezionati(Me.UtentiInvitatiLista)
        LoadTemplatesList()
        SetView(VIWattiva.VIWMail)
        For Each utente As UtenteInvitato In Me.UtentiInvitatiLista
            TXBDestinatario.Text += utente.Anagrafica + ";"
        Next
    End Sub
    Protected Sub LKBSelezionaUtenti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBSelezionaUtenti.Click
        TXBDestinatario.Text = String.Empty
        SetView(VIWattiva.VIWdestinatari)
    End Sub
    Private Sub LNBSelezionaDestinatari_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBSelezionaDestinatari.Click
        LKBSelezionaUtenti_Click(sender, e)
    End Sub
    Protected Sub LKBAggiungiTutti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBAggiungiTutti.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Me.Resource.getValue("TXBTuttiUtenti")
    End Sub
    Protected Sub LKBAggiungiNonInvitati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBAggiungiNonInvitati.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.readUtentiInvitatiNoMailByIDQuestionario(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Me.Resource.getValue("TXBUtentiNonInvitati")
    End Sub
    Protected Sub LNBaddNotStartedNotCompleted_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBaddNotStartedNotCompleted.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.GetInvitedUsersWithQuestionnairesNotStartedOrCompleted(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Resource.getValue("Questionnaire.NotStarted.NotCompleted." & DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString)
    End Sub
    Protected Sub LNBaddNotCompleted_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBaddNotCompleted.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.GetInvitedUsersWithQuestionnairesNotCompleted(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Resource.getValue("Questionnaire.NotCompleted." & DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString)
    End Sub
    Protected Sub LNBaddNotStarted_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBaddNotStarted.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.GetInvitedUsersWithQuestionnairesNotStarted(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Resource.getValue("Questionnaire.NotStarted." & DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString)
    End Sub
    Protected Sub LNBaddCompleted_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBaddCompleted.Click
        Me.UtentiInvitatiLista = DALUtenteInvitato.GetInvitedUsersWithQuestionnairesCompleted(Me.QuestionarioCorrente.id)
        TXBDestinatario.Text = Resource.getValue("Questionnaire.Completed." & DirectCast(Me.QuestionarioCorrente.tipo, Questionario.TipoQuestionario).ToString)
    End Sub
    Protected Sub LNBGestioneMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneMail.Click
        SetView(VIWattiva.VIWMail)
    End Sub
#End Region
    Private Sub BTNcancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcancel.Click
        LNBIndietro_Click(sender, e)
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
                    Dim beginMessage As String = Resource.getValue("Template.Begin." & CurrentType.ToString)
                    If String.IsNullOrWhiteSpace(beginMessage) Then
                        beginMessage = Resource.getValue("Template.Begin")
                    End If
                    beginMessage = beginMessage.Replace("#name#", QuestionarioCorrente.nome)
                    beginMessage = beginMessage.Replace("#sentdate#", GetDateTimeString(sentDate, DateTime.Now.ToShortDateString(), True))
                    CurrentPresenter.SendMail(dto, users, CHKInoltraMittente.Checked, beginMessage)
                    LBconferma.Text = Me.Resource.getValue("MSGInvioMail")
                    SetView(VIWattiva.VIWconferma)
                End If
            Else
                LBerrore.Text = Me.Resource.getValue("MSGnoInvioMail")
                SetView(VIWattiva.VIWerrore)
            End If
        Else
            LBErroreNoTag.Visible = True
            LBErroreNoTag.Text = Me.Resource.getValue("MSGOggettoObbligatorio")
        End If
    End Sub
    Private Sub BTNclosePreview_Click(sender As Object, e As System.EventArgs) Handles BTNclosePreview.Click
        Me.MLVquestionari.SetActiveView(VIWMail)
    End Sub

    Private Sub BTNpreview_Click(sender As Object, e As System.EventArgs) Handles BTNpreview.Click
        Me.LBpreview.Text = CTRLmailEditor.Mail.Body
        Dim items As New List(Of TranslatedItem(Of String))
        items.Add(New TranslatedItem(Of String) With {.Id = "#DESTINATARIO#", .Translation = "Utente X"})
        items.Add(New TranslatedItem(Of String) With {.Id = "#NOMEQUESTIONARIO#", .Translation = Me.QuestionarioCorrente.nome})
        items.Add(New TranslatedItem(Of String) With {.Id = "#LINKQUESTIONARIO#", .Translation = "LINKQUESTIONARIO"})
        items.Add(New TranslatedItem(Of String) With {.Id = "#DATAINIZIO#", .Translation = Me.QuestionarioCorrente.dataInizio})
        items.Add(New TranslatedItem(Of String) With {.Id = "#DATAFINE#", .Translation = Me.QuestionarioCorrente.dataFine})
        items.Add(New TranslatedItem(Of String) With {.Id = "#DURATA#", .Translation = Me.QuestionarioCorrente.durata})
        items.Add(New TranslatedItem(Of String) With {.Id = "#AUTORE#", .Translation = Me.QuestionarioCorrente.creator})

        Dim descrizione As String = Me.QuestionarioCorrente.descrizione.Replace("<br>", "\n")
        descrizione = descrizione.Replace("’", "")
        descrizione = descrizione.Replace("'", "")
        descrizione = COL_Questionario.RootObject.StripHTML(descrizione)
        items.Add(New TranslatedItem(Of String) With {.Id = "#DESCRIZIONEQUESTIONARIO#", .Translation = descrizione})

        For Each item As TranslatedItem(Of String) In items.Where(Function(i) Me.LBpreview.Text.Contains(i.Id)).ToList
            Me.LBpreview.Text = Replace(Me.LBpreview.Text, item.Id, item.Translation)
        Next

        Me.MLVquestionari.SetActiveView(VIWpreview)
    End Sub
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
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property

#Region "Gestione colonne URL e Description"
    Private Function ShowDescription() As Boolean
        'If SystemSettings.
        Return SystemSettings.QuestionnaireSettings.ShowDescription
    End Function

    Private Function ShowUrl() As Boolean
        With SystemSettings.QuestionnaireSettings.ShowUrlsSettings
            If Not .Enabled Then
                Return False
            End If

            If .IsForAll Then
                Return True
            End If

            Dim UserId As Integer = CurrentContext.UserContext.CurrentUserID
            If .UsersId.Contains(UserId) Then
                Return True
            End If


            Dim UserTypeID As Integer = CurrentContext.UserContext.UserTypeID

            If .UsersTypeId.Contains(UserTypeID) Then
                Return True
            End If


            Dim CommunityId As Integer = CurrentContext.UserContext.CurrentCommunityID
            Dim CommunityTypeId As Integer = CurrentPresenter.DataContext.GetById(Of Community)(CommunityId).IdTypeOfCommunity

            Dim CurrentRoleId As IList(Of Integer) = CurrentContext.UserContext.RolesID


            For Each RoleId As Integer In CurrentRoleId
                If .EnabledCommunityTypeRoles.ContainsKey(CommunityTypeId) Then
                    If .EnabledCommunityTypeRoles(CommunityTypeId).Contains(RoleId) OrElse _
                        .EnabledCommunityTypeRoles(CommunityTypeId).Contains(-1) Then
                        Return True
                    End If
                End If


                If .EnabledCommunityRoles.ContainsKey(CommunityId) AndAlso .EnabledCommunityRoles(CommunityId).Contains(RoleId) Then
                    Return True
                End If
            Next

        End With

        Return False
    End Function

    Private Sub GenerateUrl()
        For Each user As UtenteInvitato In Me.UtentiInvitatiLista

            Dim idLanguage As Integer = Me.QuestionarioCorrente.idLingua
            Dim idQ As Integer = Me.QuestionarioCorrente.id
            
            user.Url = Me.EncryptedUrl(COL_Questionario.RootObject.compileUrlUI, "idq=" & idQ & "&idu=" & user.ID & "&idl=" & idLanguage, SecretKeyUtil.EncType.Questionario)
        Next

    End Sub

#End Region

#Region "Definizioni colonne"
    Private Const _COL_CheckBox As Integer = 0
    Private Const _COL_Cognome As Integer = 1
    Private Const _COL_Nome As Integer = 2
    Private Const _COL_Mail As Integer = 3
    Private Const _COL_Url As Integer = 4
    Private Const _COL_Description As Integer = 5
    Private Const _COL_Commands As Integer = 6
#End Region
End Class