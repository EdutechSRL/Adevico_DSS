Imports lm.Comol.Core.DomainModel
Imports TK = lm.Comol.Core.BaseModules.Tickets
Imports DM = lm.Comol.Core.DomainModel
Imports CM = lm.Comol.Core.BaseModules.CommunityManagement

Public Class Add
    Inherits TicketBase 'PageBase
    Implements TK.Presentation.View.iViewTicketAdd
    
#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketAddPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketAddPresenter
        Get

            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketAddPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne

    Public ReadOnly Property ComDialogTitle As String
        Get
            Return Resource.getValue("CommunitiesDialog.Title")
        End Get
    End Property

    Public ReadOnly Property InternalUserAddTitle As String
        Get
            Return Resource.getValue("Modal.InternalUsers.Description")
        End Get
    End Property



    Private Property SelectedCommunityId As Int32
        Get
            Dim ComId As Integer = -1
            Try
                ComId = System.Convert.ToInt32(ViewStateOrDefault("SelectedComID", -1))
            Catch ex As Exception

            End Try

            Return ComId
        End Get
        Set(value As Int32)
            ViewState("SelectedComID") = value
        End Set
    End Property

    Private ReadOnly Property DraftData As TK.Domain.DTO.DTO_Ticket
        Get
            Dim TkData As New TK.Domain.DTO.DTO_Ticket()
            With TkData
                .TicketId = Me.CurrentTicketId
                .CategoryId = Me.CTRLddlCat.SelectedId
                .LanguageCode = Me.DDLlanguage.SelectedValue
                '.MailSettings = Me.CTRLmailSets.MailSettings
                .Text = Me.CTRLeditorText.HTML
                .Preview = Me.CTRLeditorText.Text
                .Title = Me.TXBtitle.Text
                .HideToOwner = Me.CBXhideToOwner.Checked

                .CommunityId = SelectedCommunityId

            End With

            Return TkData
        End Get
    End Property

#End Region

#Region "Implements"
    'Property della VIEW
    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewTicketAdd.ViewCommunityId
        Get
            Dim ComId As Integer = 0
            Try
                ComId = ViewStateOrDefault("CurrentComId", -1)
            Catch ex As Exception
            End Try

            If ComId < 0 Then
                Try
                    ComId = System.Convert.ToInt32(Request.QueryString("CommunityId"))
                Catch ex As Exception
                End Try
            End If

            Return ComId
        End Get
        Set(value As Integer)
            Me.ViewState("CurrentComId") = value
        End Set
    End Property
    Public ReadOnly Property SelectedCatId As Long Implements TK.Presentation.View.iViewTicketAdd.SelectedCatId
        Get
            Return Me.CTRLddlCat.SelectedId
        End Get
    End Property

    'Private _currentTicketId As Int64 = 0
    Public Property CurrentTicketId As Int64 Implements TK.Presentation.View.iViewTicketAdd.CurrentTicketId
        Get
            'If (Me._currentTicketId = 0) Then
            '    Try
            '        Me._currentTicketId = System.Convert.ToInt64(Me.ViewState("TicketId"))
            '    Catch ex As Exception

            '    End Try
            'End If

            'If (Me._currentTicketId = 0) Then
            '    Try
            '        Me._currentTicketId = System.Convert.ToInt64(Request.QueryString("Id"))
            '    Catch ex As Exception
            '    End Try
            'End If

            'Return Me._currentTicketId

            Dim TkId As Int64 = -1
            Try
                TkId = System.Convert.ToInt64(Request.QueryString("Id"))
                'TK.Domain.RootObject.GetTicketId(System.Convert.ToInt64(Request.QueryString("Id")))
            Catch ex As Exception
            End Try

            If (TkId <= 0) Then
                TkId = TK.Domain.RootObject.GetTicketId(Request.QueryString("Id"))
            End If

            Return ViewStateOrDefault("TicketId", TkId)
        End Get
        Set(value As Int64)
            Me.ViewState("TicketId") = value
            'Me._currentTicketId = value
        End Set
    End Property

    Private Property TicketCode As String
        Get
            Return ViewStateOrDefault("TicketCode", "")
        End Get
        Set(value As String)
            ViewState("TicketCode") = value
        End Set
    End Property


    Public Property DraftMsgId As Long Implements TK.Presentation.View.iViewTicketAdd.DraftMsgId
        Get
            Return ViewStateOrDefault("DraftId", 0)
        End Get
        Set(value As Long)
            ViewState("DraftId") = value
        End Set
    End Property

#End Region

#Region "Inherits"

#End Region

    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        'Me.CTRLcommunity.SingleSelectEvent = True

        'End If
    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        If Not Page.IsPostBack Then
            Me.CurrentPresenter.InitView()
        End If

        Me.TMsession.Interval = Me.SystemSettings.Presenter.AjaxTimer
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Add", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTtitle_t)

            .setLabel(LBsender_t)
            .setLabel(LBlanguage_t)
            .setLabel(LBname_t)
            .setLabel(LBsname_t)
            .setLabel(LBmail_t)
            .setLabel(LBcontent_t)
            .setLabel(LBtitle_t)
            .setLabel(LBcategory_t)
            .setLabel(LBcommunity_t)
            .setLabel(LBtext_t)
            .setLabel(LBnotificheMail_t)

            .setLabel(LBattachment_t)

            .setLabel(LBnoCategory)

            .setLinkButton(LNBsaveDraft, True, True)
            .setLinkButton(LNBsend, True, True)

            .setLinkButton(LNBdelete, True, True, False, True)

            .setHyperLink(HYPopenCommunities, True, True)

            .setHyperLink(HYPbackTop, True, True)
            HYPbackTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(ViewCommunityId)

            '.setLinkButton(LNBcloseCommunityWindow, True, True)


            .setLinkButton(LNBcommunitySetPortal, True, True)
            '.setLinkButton(LNBopenCommunity, True, True)

            .setLinkButton(LNBaddUsers, True, True, False, False)
            .setLinkButton(LNBremoveBehalf, True, True, False, False)

            .setCheckBox(CBXhideToOwner)

            .setLabel(LBbehalfAction_t)

            '.setLinkButton(Me.LNBhideToOwner, True, True, False, False)
            '.setLinkButton(Me.LNBshowToOwner, True, True, False, False)

            '.setLiteral(Me.LTcreator_t)
            .setLabel(LBcreator_t)

            .setLabel(LBmailVisibility_t)

            .setCheckBox(CBXhideToOwner)


            '.setLabel(LBLmailSettings)
            .setLabel(LBMailSetOwner_t)
            .setLabel(LBMailSetCreator_t)
            .setLiteral(LTownerInfo)
            .setLiteral(LTcreatorInfo)

            .setLinkButton(LNBcommunitySetCurrent, True, True)


        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)

        Dim Url As String = ""

        If (Me.CurrentTicketId > 0) Then
            Url = TK.Domain.RootObject.TicketAdd(CommunityId, Me.CurrentTicketId)
        Else
            Url = TK.Domain.RootObject.TicketAdd(CommunityId)
        End If

        RedirectOnSessionTimeOut(Url, CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

#End Region

#End Region

#Region "Implements"
    'Sub e function della View

    Public Sub refreshCategory( _
            Categories As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryTree), _
            SelectedCateId As Int64) _
        Implements TK.Presentation.View.iViewTicketAdd.refreshCategory

        If IsNothing(Categories) OrElse Categories.Count <= 0 Then
            ShowByCategories(False)
        Else
            ShowByCategories(True)
        End If
        CTRLddlCat.InitDDL(Categories, SelectedCateId, Resource.getValue("Category.select"))
    End Sub

    Public Sub ShowCantCreate(Info As TK.Domain.Enums.CantCreate) Implements TK.Presentation.View.iViewTicketAdd.ShowCantCreate

        If Info = TK.Domain.Enums.CantCreate.MaxSend Then
            'Resource.setLinkButton(Me.LNBsend, True, True, True, False)
            Me.SetSendButton(SendButtonStatus.CantCreate)
            'Me.LNBsend.Enabled = False
        Else
            Me.Master.ShowNoPermission = True
            Me.Master.ServiceNopermission = Resource.getValue("Error." & Info.ToString)
        End If

        If (Info = TK.Domain.Enums.CantCreate.System OrElse Info = TK.Domain.Enums.CantCreate.permission) Then
            Me.HYPbackTop.Visible = False
        Else
            Me.HYPbackTop.Visible = True
        End If

    End Sub

    Public Sub UpdateCommunity(NewComId As Integer, NewComName As String) Implements TK.Presentation.View.iViewTicketAdd.UpdateCommunity

        If (NewComName = TK.TicketService.ComPortalName) Then
            Me.LBcommunity.Text = Resource.getValue("Community.Portal")
        Else
            Me.LBcommunity.Text = NewComName
        End If

        Me.SelectedCommunityId = NewComId
    End Sub

    Public Sub InitView(
                       Values As TK.Domain.DTO.DTO_AddInit, _
                        actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), _
                        dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, _
                        rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, _
                        idMessage As Long) Implements TK.Presentation.View.iViewTicketAdd.InitView



        SetSendButton(SendButtonStatus.Enabled)

        If (Not Page.IsPostBack AndAlso Not String.IsNullOrEmpty(Request.QueryString("n"))) Then
            Values.IsNew = True
        Else
            Values.IsNew = False
        End If



        TicketCode = Values.TicketData.Code
        Me.TXBname.Text = Values.TicketData.OwnerName 'Values.CurrentUser.Name
        Me.TXBsname.Text = Values.TicketData.OwnerSName ' Values.CurrentUser.SName
        Me.TXBmail.Text = Values.TicketData.OwnerMail ' Values.CurrentUser.Mail

        Me.TXBname.Enabled = Values.CanModifyUser
        Me.TXBsname.Enabled = Values.CanModifyUser
        Me.TXBmail.Enabled = Values.CanModifyUser

        DDLlanguage.Items.Clear()
        DDLlanguage.DataSource = Values.availableLanguages()
        DDLlanguage.DataTextField = "Name"
        DDLlanguage.DataValueField = "Code"
        DDLlanguage.DataBind()
        If (String.IsNullOrEmpty(Values.TicketData.LanguageCode)) Then
            DDLlanguage.SelectedValue = Values.CurrentUser.LanguageCode
        Else
            DDLlanguage.SelectedValue = Values.TicketData.LanguageCode
        End If


        Me.TXBtitle.Text = Values.TicketData.Title
        Me.CTRLeditorText.HTML = Values.TicketData.Text

        'Me.CTRLmailSets.MailSettings = Values.TicketData.MailSettings

        'InitComSelector(Me.SelectedCommunityId)

        If Not IsNothing(Values.TicketData.Attachments) Then
            DIVatchCom.Attributes.Add("class", "fieldrow attachments")
        Else
            DIVatchCom.Attributes.Add("class", "fieldrow attachments empty")
        End If

        Me.CTRLattView.InitUc(Values.TicketData.Attachments, False, -1, True)

        Dim IsFromList As Boolean = IIf(Not String.IsNullOrEmpty(Request.QueryString("FL")) AndAlso Request.QueryString("FL") = "1", True, False)

        If Values.IsNew Then
            If Values.HasOtherDraft AndAlso Not IsFromList Then
                Me.SetErrorMessage_NEW(Resource.getValue("Message.NewDraftAndOther"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            Else
                Me.SetErrorMessage_NEW(Resource.getValue("Message.NewDraft"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
            End If
        Else
            Me.SetErrorMessage_NEW("", lm.Comol.Core.DomainModel.Helpers.MessageType.none)
        End If

        'Behalf

        PNLbehalf.Visible = Values.CanBehalf OrElse Values.TicketData.IsBehalf
        If (Values.CanBehalf) Then
            'Inizializzazione: prima di visibilità bottoni
            'Me.CurrentPresenter.InitPersonSelector()
        End If

        'Me.LNBaddUsers.Enabled = Values.CanBehalf AndAlso Values.CanListUsers 'CTRLselectUsers.HasAvailableUsers
        Me.LNBaddUsers.Visible = Values.CanBehalf AndAlso Values.CanListUsers 'AndAlso CTRLselectUsers.HasAvailableUsers

        'Me.LNBremoveBehalf.Visible = False
        'Me.LNBremoveBehalf.Enabled = False

        Me.LNBremoveBehalf.Visible = Values.TicketData.IsBehalf
        'Me.LNBremoveBehalf.Enabled = Values.TicketData.IsBehalf

        If (Values.TicketData.IsBehalf) Then
            PNLbehalfVisibility.Visible = True

            CBXhideToOwner.Checked = Values.TicketData.HideToOwner

            If Not CBXhideToOwner.Checked Then
                Me.PNLownerNotification.Visible = True
            End If

        Else
            PNLbehalfVisibility.Visible = False
            CBXhideToOwner.Checked = False
        End If

        If Values.TicketData.IsBehalf AndAlso Not Values.CanBehalf Then
            'ToDo
            Me.SetSendButton(SendButtonStatus.DisabledBehalf)
            'Me.LNBremoveBehalf.Visible = True
            'Me.LNBremoveBehalf.Enabled = True
        End If

        ''Tasti visibilità TICKET
        'Me.LNBshowToOwner.Visible = Values.TicketData.HideToOwner
        'Me.LNBhideToOwner.Visible = Not Values.TicketData.HideToOwner


        Dim CssClass As String = "fieldobject fieldgroup"

        If (Values.TicketData.IsBehalf) Then
            CssClass &= " isbehalf"
            If (Values.CanBehalf) Then  'To
                'Me.LNBremoveBehalf.Visible = True
                'Me.LNBremoveBehalf.Enabled = True
            End If

            If (Values.TicketData.HideToOwner) Then
                CssClass &= " hidetoowner"
            End If
        End If

        Me.DVsubmitter.Attributes.Add("class", CssClass)

        If Values.TicketData.IsBehalf Then      'AndAlso Values.TicketData.IsOwner
            PNLbehalfCreator.Visible = True
            Me.LTcreator.Text = Values.TicketData.Creator
        Else
            PNLbehalfCreator.Visible = False

        End If


        'Notifiche
        'Me.PNLownerNotification.Visible = False
        If Values.TicketData.IsBehalf Then
            If Me.CBXhideToOwner.Checked Then
                Me.PNLownerNotification.Visible = False
                'Me.LBMailSetCreator_t.Visible = False
            Else
                Me.PNLownerNotification.Visible = True
                'Me.LBMailSetCreator_t.Visible = True
            End If

            Me.CTRLmailSetOwner.BindSettings(True, Values.IsDefaultNotOwner, Values.OwnerMailSettings, TK.Domain.Enums.ViewSettingsUser.Owner, Values.IsOwnerNotificationEnable, True, False)

            Me.CTRLmailSetCreator.BindSettings(True, Values.IsDefaultNotCreator, Values.CreatorMailSettings, TK.Domain.Enums.ViewSettingsUser.Owner, Values.IsCreatorNotificationEnable, True, True)

        Else
            Me.PNLownerNotification.Visible = False
            'Me.LBMailSetCreator_t.Visible = False
            Me.CTRLmailSetCreator.BindSettings(True, Values.IsDefaultNotCreator, Values.CreatorMailSettings, TK.Domain.Enums.ViewSettingsUser.Owner, Values.IsCreatorNotificationEnable, True, False)
        End If

        'Files
        'Passato da Business all'inizializzazione
        CTRLcommands.Visible = actions.Any()
        If actions.Any() Then
            CTRLcommands.InitializeControlForJQuery(actions, dAction)
            Me.CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))

            'Internal Upload
            CTRLinternalUpload.Visible = True
            CTRLinternalUpload.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, 0, False) ' No Com ID => portale!

            'Upload su message E community
            If Values.FileCommunityId > 0 AndAlso actions.Contains(Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
                CTRLcommunityUpload.Visible = True
                CTRLcommunityUpload.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity, Values.FileCommunityId, rPermissions)
            Else
                CTRLcommunityUpload.Visible = False
            End If

            'Link da comunità
            If Values.FileCommunityId > 0 AndAlso actions.Contains(Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
                CTRLlinkFromCommunity.Visible = True
                Me.CTRLlinkFromCommunity.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.linkfromcommunity, Values.FileCommunityId, rPermissions)
            Else
                CTRLlinkFromCommunity.Visible = False
            End If
        Else
            CTRLlinkFromCommunity.Visible = False
            CTRLcommunityUpload.Visible = False
            CTRLinternalUpload.Visible = False
        End If
        'Me.CurrentPresenter.initCommunitySelector(Me.ViewCommunityId)

    End Sub

    Public Sub ShowError([Error] As TK.Domain.Enums.TicketCreateError) Implements TK.Presentation.View.iViewTicketAdd.ShowError
        SetErrorMessage_NEW(Resource.getValue("Error." & [Error].ToString()), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)

    End Sub

    Public Sub TicketCreated(TicketId As Long, IsDraft As Boolean) Implements TK.Presentation.View.iViewTicketAdd.TicketCreated

        If Not IsDraft Then
            Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(ViewCommunityId))
        Else
            Me.CurrentTicketId = TicketId
        End If

    End Sub

    Public Function GetDraftPreview() As String Implements TK.Presentation.View.iViewTicketAdd.GetDraftPreview
        Return Resource.getValue("TicketDraft.Preview")
    End Function

    Public Function GetDraftTitle() As String Implements TK.Presentation.View.iViewTicketAdd.GetDraftTitle
        Return Resource.getValue("TicketDraft.Title")
    End Function

    Private Function GetUploadedItems(draftMessage As TK.Domain.Message, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) Implements TK.Presentation.View.iViewTicketAdd.GetUploadedItems
        Select Case action
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                Return CTRLinternalUpload.UploadFiles(False, draftMessage)
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity
                Return CTRLcommunityUpload.UploadFiles(True, draftMessage)
            Case Else
                Return New List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem)
        End Select
    End Function

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

    Private Sub ShowByCategories(ByVal HasCategory As Boolean)
        If HasCategory Then
            CTRLddlCat.Visible = True
            LBnoCategory.Visible = False

            Me.LNBsaveDraft.Enabled = True
            'Me.LNBsend.Enabled = True
        Else
            CTRLddlCat.Visible = False
            LBnoCategory.Visible = True

            Me.LNBsaveDraft.Enabled = False
            Me.SetSendButton(SendButtonStatus.DisabledCategory)
            'Me.LNBsend.Enabled = False
        End If
    End Sub

    Public Function ValidateFields(ByVal isDraft As Boolean) As Boolean

        SetErrorMessage_NEW("", lm.Comol.Core.DomainModel.Helpers.MessageType.none)

        If Me.TXBtitle.Text.Length > 255 Then
            Me.TXBtitle.Text = Me.TXBtitle.Text.Remove(255)
        End If

        Dim IsValid As Boolean = True
        Me.LBcatRequired.Visible = False
        Me.LBtextRequired.Visible = False
        Me.LBtitleRequired.Visible = False

        If Me.CTRLddlCat.SelectedId <= 0 Then
            Me.LBcatRequired.Visible = True
            IsValid = False
        End If

        If Not isDraft AndAlso String.IsNullOrEmpty(Me.CTRLeditorText.Text) Then
            Me.LBtextRequired.Visible = True
            IsValid = False
            SetErrorMessage_NEW(Resource.getValue("Message.EmptyMessage"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        End If

        If String.IsNullOrEmpty(Me.TXBtitle.Text) Then
            Me.LBtitleRequired.Visible = True
            IsValid = False

            SetErrorMessage_NEW(Resource.getValue("Message.EmptyMessage"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        End If

        Return IsValid
    End Function

    Private Function GetTicketData(ByVal IsDraft As Boolean) As TK.Domain.DTO.DTO_Ticket

        Dim DTOTicket As New TK.Domain.DTO.DTO_Ticket()

        With DTOTicket
            .CategoryId = Me.CTRLddlCat.SelectedId
            .IsDraft = IsDraft
            .LanguageCode = Me.DDLlanguage.SelectedValue
            '.MailSettings = Me.CTRLmailSets.MailSettings
            .Text = Me.CTRLeditorText.HTML
            .Preview = Me.CTRLeditorText.Text
            .Title = Me.TXBtitle.Text

        End With

        Return DTOTicket

    End Function

    Private Sub Save(ByVal IsDraft As Boolean, Optional ByVal IsForUpload As Boolean = False)

        Dim TkData As TK.Domain.DTO.DTO_Ticket = DraftData
        TkData.IsDraft = IsDraft

        'NOTIFICATION
        Dim creatorSets As TK.Domain.Enums.MailSettings = Me.CTRLmailSetCreator.GetSettings()

        Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
        If Me.PNLownerNotification.Visible Then
            ownerSets = Me.CTRLmailSetOwner.GetSettings()
        End If

        Me.CurrentPresenter.SaveTicket(TkData, ownerSets, creatorSets, IsForUpload)

    End Sub



    Public Sub SetErrorMessage_NEW(ByVal message As String, ByVal Type As lm.Comol.Core.DomainModel.Helpers.MessageType)
        If (Type = lm.Comol.Core.DomainModel.Helpers.MessageType.none OrElse String.IsNullOrEmpty(message)) Then

            Me.DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else

            Me.DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(message, Type)
        End If

    End Sub

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)

#Region "Old Community"

    'Private Sub LNBopenCommunity_Click(sender As Object, e As EventArgs) Handles LNBopenCommunity.Click
    '    Me.InitComSelector(Me.SelectedCommunityId)
    '    Me.DVselectCom.Visible = True
    'End Sub

    'Private Sub LNBcloseCommunityWindow_Click(sender As Object, e As EventArgs) Handles LNBcloseCommunityWindow.Click
    '    Me.DVselectCom.Visible = False
    'End Sub

    'Private Sub InitComSelector(ByVal ComId As Integer)

    '    Me.CTRLcomSelector.SelectionMode = ListSelectionMode.Single
    '    Me.CTRLcomSelector.isModalitaAmministrazione = True
    '    Me.CTRLcomSelector.AllowMultipleOrganizationSelection = False
    '    Me.CTRLcomSelector.AllowCommunityChangedEvent = True

    '    Try
    '        If (ComId > 0) Then
    '            Me.CTRLcomSelector.InitializeControl(ComId)
    '        Else
    '            Me.CTRLcomSelector.InitializeControl(-1)
    '        End If
    '    Catch ex As Exception

    '    End Try


    'End Sub



    'Private Sub CTRLcomSelector_SelectedCommunityChanged(CommunityID As Integer) Handles CTRLcomSelector.SelectedCommunityChanged

    '    If CommunityID > 0 AndAlso Not Me.SelectedCommunityId = CommunityID Then
    '        Dim TkData As TK.Domain.DTO.DTO_Ticket = DraftData
    '        TkData.IsDraft = True
    '        TkData.CommunityId = CommunityID

    '        'NOTIFICATION
    '        Dim creatorSets As TK.Domain.Enums.MailSettings = Me.CTRLmailSetCreator.GetSettings()

    '        Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
    '        If Me.PNLownerNotification.Visible Then
    '            ownerSets = Me.CTRLmailSetOwner.GetSettings()
    '        End If

    '        Me.CurrentPresenter.SaveTicket(TkData, ownerSets, creatorSets, True)
    '        Me.CurrentPresenter.UpdateCommunity(CommunityID, Me.CTRLddlCat.SelectedId)

    '        Me.DVselectCom.Visible = False

    '        Me.CurrentPresenter.InitView()

    '    End If

    'End Sub

    Private Sub LNBcommunitySetPortal_Click(sender As Object, e As EventArgs) Handles LNBcommunitySetPortal.Click
        'Me.SelectedCommunityId = 0
        Me.CurrentPresenter.UpdateCommunity(0, Me.CTRLddlCat.SelectedId)
        'Me.DVselectCom.Visible = False
    End Sub

    Private Sub LNBcommunitySetCurrent_Click(sender As Object, e As EventArgs) Handles LNBcommunitySetCurrent.Click
        'Me.SelectedCommunityId = -1
        Me.CurrentPresenter.UpdateCommunity(-1, Me.CTRLddlCat.SelectedId)
    End Sub
#End Region

    Private Sub LNBsend_Click(sender As Object, e As System.EventArgs) Handles LNBsend.Click
        If Me.ValidateFields(False) Then
            Me.Save(False)
        End If
    End Sub

    Private Sub LNBsaveDraft_Click(sender As Object, e As System.EventArgs) Handles LNBsaveDraft.Click
        If Me.ValidateFields(True) Then
            Me.Save(True)

            Me.SetErrorMessage_NEW(Resource.getValue("Info.Saved"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
        End If
    End Sub

    Private Sub TMsession_Tick(sender As Object, e As System.EventArgs) Handles TMsession.Tick

        Me.CurrentPresenter.SendTimerAction()

        If IsNothing(Me.CurrentPresenter.UserContext) OrElse Me.CurrentPresenter.UserContext.CurrentUserID <= 0 Then
            Me.TMsession.Enabled = False
        End If

    End Sub



    Private Sub CTRLattView_FileAction(sender As Object, e As RepeaterCommandEventArgs) Handles CTRLattView.FileAction
        Dim idAttachment As Int64 = 0
        Int64.TryParse(e.CommandArgument, idAttachment)

        If e.CommandName = "File_Delete" AndAlso idAttachment > 0 Then
            Me.CurrentPresenter.DeleteFile(idAttachment, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
        End If
    End Sub



    Private Sub LNBdelete_Click(sender As Object, e As EventArgs) Handles LNBdelete.Click

        Dim respError As TK.Domain.Enums.TicketDraftDeleteError = Me.CurrentPresenter.DeleteDraft(PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)

        If respError = TK.Domain.Enums.TicketDraftDeleteError.none Then
            Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(ViewCommunityId, True))

        ElseIf Not respError = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketDraftDeleteError.hide Then

            Dim message As String = Resource.getValue("Message.DeleteDraft." & respError.ToString())
            SetErrorMessage_NEW(message, lm.Comol.Core.DomainModel.Helpers.MessageType.error)
        End If

    End Sub

#End Region

    Private Sub LNBremoveBehalf_Click(sender As Object, e As EventArgs) Handles LNBremoveBehalf.Click
        Me.CurrentPresenter.RemoveBehalf()

    End Sub

    Private Sub CTRLselectUsers_CloseWindow() Handles CTRLselectUsers.CloseWindow
        Me.DIVusers.Visible = False
    End Sub

    Private Sub CTRLselectUsers_UserSelected(idUser As Integer) Handles CTRLselectUsers.UserSelected
        Me.CurrentPresenter.SetBehalfPerson(idUser, True) 'Me.CBXhideToOwner.Checked)
        Me.DIVusers.Visible = False
    End Sub

    Private Sub LNBaddUsers_Click(sender As Object, e As EventArgs) Handles LNBaddUsers.Click
        Me.CurrentPresenter.InitPersonSelector()
        Me.DIVusers.Visible = True
    End Sub



    Private _canListUSer As Boolean = False

    Public Sub InitPersonSelector(hidePersonId As List(Of Integer), CommunityId As Integer) Implements TK.Presentation.View.iViewTicketAdd.InitPersonSelector

        If (CommunityId > 0) Then
            Dim comIds As New List(Of Integer)
            comIds.Add(CommunityId)

            CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, _
                                              False, comIds, hidePersonId, Nothing, Resource.getValue("Modal.InternalUsers.Description"))


            'CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, False, True, hidePersonId, comIds, Resource.getValue("Modal.InternalUsers.Description"))
        Else
            CTRLselectUsers.InitializeControl( _
            lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, False, True, hidePersonId, Nothing, Resource.getValue("Modal.InternalUsers.Description"))
        End If


        ''Se amministratori di alto livello, mostro sistema


        'Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, True, False, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription." & CallType.ToString & ".text"))




        'Se non trovo utenti, provo con le comunità dell'utente stesso
        'If Not CTRLselectUsers.HasAvailableUsers Then
        'CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, False, True, hidePersonId, Nothing, Resource.getValue("Modal.InternalUsers.Description"))
        'Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunities, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription." & CallType.ToString & ".text"))
        'End If


        'Me.DIVusers.Visible = CTRLselectUsers.



    End Sub


    Public Sub ShowBehalfError(behalfError As TK.Domain.Enums.BehalfError) Implements TK.Presentation.View.iViewTicketAdd.ShowBehalfError

        If (behalfError = TK.Domain.Enums.BehalfError.none) Then
            SetErrorMessage_NEW("", lm.Comol.Core.DomainModel.Helpers.MessageType.none)
            Return
        End If

        Me.DVmessages.Visible = True

        Dim ErrType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error

        If behalfError = TK.Domain.Enums.BehalfError.success _
            OrElse behalfError = TK.Domain.Enums.BehalfError.deleteSuccess _
            OrElse behalfError = TK.Domain.Enums.BehalfError.visibilitySuccess Then

            ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End If

        SetErrorMessage_NEW(Me.Resource.getValue("BehalfOperation." & behalfError.ToString()), ErrType)

    End Sub

    Private Sub SetVisibility(ByVal HideToUser As Boolean)
        Me.CurrentPresenter.SetTicketVisibility(HideToUser)
    End Sub

    'Private Sub LNBshowToOwner_Click(sender As Object, e As EventArgs) Handles LNBshowToOwner.Click
    '    SetVisibility(False)
    'End Sub


    'Private Sub LNBhideToOwner_Click(sender As Object, e As EventArgs) Handles LNBhideToOwner.Click
    '    SetVisibility(True)
    'End Sub

    Private Sub CBXhideToOwner_CheckedChanged(sender As Object, e As EventArgs) Handles CBXhideToOwner.CheckedChanged
        Me.PNLownerNotification.Visible = Not CBXhideToOwner.Checked
        'Me.LBMailSetCreator_t.Visible = Not CBXhideToOwner.Checked
    End Sub

    Public Sub GotoNewTicketCreated(newTicketCode As String) Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewTicketAdd.GotoNewTicketCreated
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketAdd(ViewCommunityId, newTicketCode, True))
    End Sub

    Private Sub Add_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If (Not PNLbehalf.Visible) Then
            Exit Sub
        End If

        Dim itemCount As Integer = (If(LNBremoveBehalf.Visible, 1, 0)) + _
            (If(LNBaddUsers.Visible, 1, 0)) + _
            (If(LNBaddTicketUser.Visible, 1, 0)) + _
            (If(LNBaddNewTicketUSer.Visible, 1, 0))

        '(If(LNBhideToOwner.Visible, 1, 0)) + _
        '(If(LNBshowToOwner.Visible, 1, 0)) + _

        TicketHelper.CheckDropDownButton(DVddlBehalfContainer, itemCount)

    End Sub

    Private Sub SetSendButton(ByVal status As SendButtonStatus)
        Me.LNBsend.ToolTip = Resource.getValue("LNBsend.ToolTip")
        Select Case status
            Case SendButtonStatus.Hide
                Me.LNBsend.Visible = False
            Case SendButtonStatus.Enabled
                Me.LNBsend.Visible = True
                Me.LNBsend.Enabled = True
            Case SendButtonStatus.DisabledBehalf
                Me.LNBsend.Visible = True
                Me.LNBsend.Enabled = False
                Me.LNBsend.ToolTip = Resource.getValue("LNBsend.ToolTip.NoBehalf")
            Case SendButtonStatus.DisabledCategory
                Me.LNBsend.Visible = True
                Me.LNBsend.Enabled = False
                Me.LNBsend.ToolTip = Resource.getValue("LNBsend.ToolTip.DisabledCategory")
            Case SendButtonStatus.CantCreate
                Me.LNBsend.Visible = True
                Me.LNBsend.Enabled = False
                Me.LNBsend.ToolTip = Resource.getValue("LNBsend.ToolTip.CantCreate")
        End Select



    End Sub

    Private Enum SendButtonStatus
        Enabled
        DisabledBehalf
        DisabledCategory
        CantCreate
        Hide
    End Enum

#Region "Upload Items"
    Private Sub CTRLinternalUpload_ItemsNotAdded(action As Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.ItemsNotAdded, CTRLcommunityUpload.ItemsNotAdded, CTRLlinkFromCommunity.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLinternalUpload_NoFilesToAdd(action As Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.NoFilesToAdd, CTRLcommunityUpload.ItemsNotAdded, CTRLlinkFromCommunity.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
    End Sub

    Private Sub CTRLinternalUpload_UploadToModuleObject() Handles CTRLinternalUpload.UploadToModuleObject
        Dim TkData As TK.Domain.DTO.DTO_Ticket = DraftData
        TkData.IsDraft = True

        'NOTIFICATION
        Dim creatorSets As TK.Domain.Enums.MailSettings = Me.CTRLmailSetCreator.GetSettings()

        Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
        If Me.PNLownerNotification.Visible Then
            ownerSets = Me.CTRLmailSetOwner.GetSettings()
        End If

        CurrentPresenter.AttachmentsAddInternal(DraftData, creatorSets, ownerSets)
        Master.ClearOpenedDialogOnPostback()
        'Me.Save(True)
    End Sub
    Private Sub CTRLlinkFromCommunity_LinkFromRepository(items As List(Of ModuleActionLink)) Handles CTRLlinkFromCommunity.LinkFromRepository

        'Save(True, True)
        Dim TkData As TK.Domain.DTO.DTO_Ticket = DraftData
        TkData.IsDraft = True

        'NOTIFICATION
        Dim creatorSets As TK.Domain.Enums.MailSettings = Me.CTRLmailSetCreator.GetSettings()

        Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
        If Me.PNLownerNotification.Visible Then
            ownerSets = Me.CTRLmailSetOwner.GetSettings()
        End If

        Me.CurrentPresenter.AttachmentsLinkFromCommunity(DraftData, creatorSets, ownerSets, items)
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLcommunityUpload_UploadFileToCommunity() Handles CTRLcommunityUpload.UploadToRepository


        Dim TkData As TK.Domain.DTO.DTO_Ticket = DraftData
        TkData.IsDraft = True

        'NOTIFICATION
        Dim creatorSets As TK.Domain.Enums.MailSettings = Me.CTRLmailSetCreator.GetSettings()

        Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
        If Me.PNLownerNotification.Visible Then
            ownerSets = Me.CTRLmailSetOwner.GetSettings()
        End If

        Me.CurrentPresenter.AttachmentsAddAlsoToCommunity(DraftData, creatorSets, ownerSets, True)
        Master.ClearOpenedDialogOnPostback()
    End Sub
#End Region
    


  

   

    Public Sub InitializeCommunitySelector( _
                                          forAdministration As Boolean, _
                                          curretPersonId As Integer, _
                                          unloadIdCommunities As List(Of Integer), _
                                          availability As CM.CommunityAvailability, _
                                          requiredPermissions As Dictionary(Of Integer, Long)) _
                                      Implements TK.Presentation.View.iViewTicketAdd.InitializeCommunitySelector

        CTRLcommunity.Visible = True

        Me.CTRLcommunity.SelectionMode = ListSelectionMode.Single
        Me.CTRLcommunity.SingleSelectEvent = True
        
        Me.CTRLcommunity.InitializeAdministrationControl(curretPersonId, unloadIdCommunities, availability, New List(Of Integer))

        'SetInternazionalizzazione della pagina avviene PRIMA di quello del controllo, che così ne sovrascrive le impostazionie.
        'L'internaizonalizzazione è quindi spostata qui.
        With Me.Resource
            CTRLcommunitySelectorHeader.ModalTitle = .getValue("ComSelector.ModalTitle")
            CTRLcommunity.Description = .getValue("ComSelector.Description")
            CTRLcommunity.CloseButtonText = .getValue("ComSelector.CloseButtonText")
            CTRLcommunity.CloseButtonToolTip = .getValue("ComSelector.CloseButtonToolTip")
            CTRLcommunity.SelectButtonText = .getValue("ComSelector.SelectButtonText")
            CTRLcommunity.SelectButtonToolTip = .getValue("ComSelector.SelectButtonToolTip")
        End With

    End Sub

    'Public Sub OpenCommunitySelector()
    '    Master.SetOpenDialogOnPostbackByCssClass(CTRLcommunity.ModalIdentifier)
    'End Sub

    'Private Sub LNBopenCommunity_Click(sender As Object, e As EventArgs) Handles LNBopenCommunity.Click
    '    OpenCommunitySelector()
    '    '
    'End Sub

    'Private Sub CTRLcommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLcommunity.SelectedCommunities
    '    If (Not IsNothing(idCommunities) AndAlso idCommunities.Any()) Then
    '        CTRLcommunity_SelectedCommunity(idCommunities.FirstOrDefault(), identifier)
    '    Else
    '        CTRLcommunity_SelectedCommunity(0, identifier)
    '    End If
    'End Sub
    
    Private Sub CTRLcommunity_SelectedCommunity(idCommunity As Integer, identifier As String) Handles CTRLcommunity.SelectedCommunity
        
        'CTRLcommunity.Visible = False
        Master.ClearOpenedDialogOnPostback()

        If idCommunity > 0 AndAlso Not Me.SelectedCommunityId = idCommunity Then
            Dim TkData As TK.Domain.DTO.DTO_Ticket = DraftData
            TkData.IsDraft = True
            TkData.CommunityId = idCommunity

            'NOTIFICATION
            Dim creatorSets As TK.Domain.Enums.MailSettings = Me.CTRLmailSetCreator.GetSettings()

            Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
            If Me.PNLownerNotification.Visible Then
                ownerSets = Me.CTRLmailSetOwner.GetSettings()
            End If

            Me.CurrentPresenter.SaveTicket(TkData, ownerSets, creatorSets, True)
            Me.CurrentPresenter.UpdateCommunity(idCommunity, Me.CTRLddlCat.SelectedId)

            'Me.CurrentPresenter.InitView()

        End If
    End Sub

    Private Sub CTRLcommunity_LoadDefaultFiltersToHeader( _
                                                        filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), _
                                                        requiredPermissions As Dictionary(Of Integer, Long), _
                                                        unloadIdCommunities As List(Of Integer), _
                                                        availability As CM.CommunityAvailability, _
                                                        onlyFromOrganizations As List(Of Integer)) _
                                                    Handles CTRLcommunity.LoadDefaultFiltersToHeader

        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)



        'CTRLcommunity.ModalIdentifier

        'CTRLcommunitySelectorHeader.ModalIdentifier



    End Sub

    Public ReadOnly Property CommunityModalId As String
        Get
            If (Me.CTRLcommunity.ModalIdentifier.StartsWith(".")) Then
                Return Me.CTRLcommunity.ModalIdentifier
            Else
                Return String.Format(".{0}", Me.CTRLcommunity.ModalIdentifier)
            End If
        End Get
    End Property

End Class