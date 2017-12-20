Imports TK = lm.Comol.Core.BaseModules.Tickets
Imports lm.Comol.Core.DomainModel

Public Class AddExternal
    Inherits TicketBase
    Implements TK.Presentation.View.iViewTicketAddExt

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketAddExtPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketAddExtPresenter
        Get

            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketAddExtPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne

    Private ReadOnly Property DraftData As TK.Domain.DTO.DTO_Ticket
        Get
            Dim TkData As New TK.Domain.DTO.DTO_Ticket()
            With TkData
                .TicketId = Me.CurrentTicketId
                .CategoryId = Me.CTRLddlCat.SelectedId
                .LanguageCode = Me.DDLlanguage.SelectedValue

                .MailSettings = Me.CTRLmailSets.MailSettings
                .Text = Me.CTRLeditorText.HTML
                .Preview = Me.CTRLeditorText.Text
                .Title = Me.TXBtitle.Text

                .CommunityId = Me.ViewCommunityId
            End With

            Return TkData
        End Get
    End Property

#End Region

#Region "Implements"
    'Property della VIEW
    Private _currentTicketId As Int64 = 0
    Public Property CurrentTicketId As Long Implements TK.Presentation.View.iViewTicketAddExt.CurrentTicketId
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
        Set(value As Long)
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

    Public ReadOnly Property CurrentUser As TK.Domain.DTO.DTO_User Implements TK.Presentation.View.iViewTicketAddExt.CurrentUser
        Get
            Dim Usr As New TK.Domain.DTO.DTO_User

            Try
                Usr = DirectCast(Session(TicketHelper.SessionExtUser), TK.Domain.DTO.DTO_User)
            Catch ex As Exception

            End Try

            Return Usr
        End Get
    End Property

    Public ReadOnly Property SelectedCatId As Long Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewTicketAddExt.SelectedCatId
        Get
            Return Me.CTRLddlCat.SelectedId
        End Get
    End Property

#End Region

#Region "Inherits"
    'Property del PageBase

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
        Me.CTRLEditorText.ModuleCode = TK.ModuleTicket.UniqueCode

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.Page_Title = Resource.getValue("Page.Title.External")

        Dim actions As New List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)

        actions.Add(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)

        If Not IsNothing(Me.CurrentUser) AndAlso Me.CurrentUser.PersonId > 0 Then
            Dim dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none

            Me.CTRLcommands.InitializeControlForJQuery(actions, dAction)
        Else
            Me.CTRLcommands.Visible = False
        End If

    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        Me.Master.BindSkin()

        Me.CurrentPresenter.InitView()

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
            '.setLabel(LBLcommunity_t)
            .setLabel(LBtext_t)
            .setLabel(LBnotificheMail_t)

            .setLabel(LBattachment_t)

            .setLinkButton(LNBsaveDraft, True, True)
            .setLinkButton(LNBsend, True, True)

            .setLinkButton(LNBdelete, True, True, False, True)
            '.setHyperLink(HYPopenCommunities, True, True)

            .setHyperLink(HYPbackTop, True, True)
            HYPbackTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalList()

            .setLabel(LBLmailSettings)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    'Sub e function della View
    Public Sub InitView(Values As TK.Domain.DTO.DTO_AddInit) Implements TK.Presentation.View.iViewTicketAddExt.InitView
        TicketCode = Values.TicketData.Code

        Me.TXBname.Text = Values.CurrentUser.Name
        Me.TXBsname.Text = Values.CurrentUser.SName
        Me.TXBmail.Text = Values.CurrentUser.Mail

        Me.TXBname.Enabled = Values.CanModifyUser
        Me.TXBsname.Enabled = Values.CanModifyUser
        Me.TXBmail.Enabled = Values.CanModifyUser

        DDLlanguage.Items.Clear()
        DDLlanguage.DataSource = Values.availableLanguages()
        DDLlanguage.DataTextField = "Name"
        DDLlanguage.DataValueField = "Code"
        DDLlanguage.DataBind()

        DDLlanguage.SelectedValue = Values.CurrentUser.LanguageCode

        Me.TXBtitle.Text = Values.TicketData.Title
        Me.CTRLEditorText.HTML = Values.TicketData.Text

        'Me.CTRLmailSets.MailSettings = Values.TicketData.MailSettings

        If Values.IsNew Then
            Me.SetErrorMessage_NEW(Resource.getValue("Message.NewDraft"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
        Else
            Me.SetErrorMessage_NEW("", lm.Comol.Core.DomainModel.Helpers.MessageType.none)
        End If

        If (Me.CurrentUser.PersonId >= 0) Then
            Dim actions As New List(Of Repository.RepositoryAttachmentUploadActions)
            actions.Add(Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)
            Me.DIVattachments.Visible = True
            CTRLcommands.Visible = True
            CTRLcommands.InitializeControlForJQuery(actions, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)
            If Not IsNothing(Values.TicketData.Attachments) Then
                DIVatchCom.Attributes.Add("class", "fieldrow attachments")
            Else
                DIVatchCom.Attributes.Add("class", "fieldrow attachments empty")
            End If

            Me.CTRLattView.InitUc(Values.TicketData.Attachments, False, -1, True)

            'Upload File

            Me.CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))

            'Inizializzatore Dialog
            CTRLinternalUpload.Visible = True
            CTRLinternalUpload.InitializeControl(0, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, 0, True, CurrentUser.PersonId) 'NO COM ID => portale!


            'Impostaizoni notifica
            'Notifiche
            Me.CTRLmailSetOwner.BindSettings(True, _
                                             Values.IsDefaultNotOwner, _
                                             Values.OwnerMailSettings, _
                                             TK.Domain.Enums.ViewSettingsUser.Owner, _
                                             Values.IsOwnerNotificationEnable, _
                                             False, False)


        Else
            Me.DIVattachments.Visible = False
            CTRLcommands.Visible = False
        End If

    End Sub

    Public Sub ShowCantCreate(NoCategory As Boolean, ToMuchTicket As Boolean) Implements TK.Presentation.View.iViewTicketAddExt.ShowCantCreate

        Me.PNLcontent.Visible = False
        Me.PNLerror.Visible = True

        Dim msg As String = ""

        If (NoCategory) Then
            msg = Resource.getValue("Error." & TK.Domain.Enums.TicketCreateError.NoCategory.ToString())

        ElseIf (ToMuchTicket) Then
            msg = Resource.getValue("Error." & TK.Domain.Enums.TicketCreateError.ToMuchTicket.ToString())
        Else
            msg = Resource.getValue("Error." & TK.Domain.Enums.TicketCreateError.NoPermission.ToString())
        End If

        Me.SetErrorMessage_NEW(msg, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)

    End Sub

    Public Sub ShowError([Error] As TK.Domain.Enums.TicketCreateError) Implements TK.Presentation.View.iViewTicketAddExt.ShowError

        Dim MsgType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.none

        Select Case [Error]
            Case TK.Domain.Enums.TicketCreateError.dBUnknown
                MsgType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case TK.Domain.Enums.TicketCreateError.NoCategory
                MsgType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case TK.Domain.Enums.TicketCreateError.none
                MsgType = lm.Comol.Core.DomainModel.Helpers.MessageType.none
            Case TK.Domain.Enums.TicketCreateError.NoPermission
                MsgType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case TK.Domain.Enums.TicketCreateError.NoText
                MsgType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case TK.Domain.Enums.TicketCreateError.NoTitle
                MsgType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case TK.Domain.Enums.TicketCreateError.ToMuchTicket
                MsgType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert

        End Select

        SetErrorMessage_NEW(Resource.getValue("Error." & [Error].ToString()), MsgType)

    End Sub

    Public Sub TicketCreated(TicketId As Long, IsDraft As Boolean) Implements TK.Presentation.View.iViewTicketAddExt.TicketCreated
        If Not IsDraft Then
            Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(ViewCommunityId))
        Else
            Me.CurrentTicketId = TicketId
        End If
    End Sub

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        'Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.ExternalLogin)
        Me.CTRLtopBar.LogOut()
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.PNLerror.Visible = True
        Me.PNLcontent.Visible = False

        Resource.setLabel_To_Value(LBmainError, "Error.NoAccess")
    End Sub

    Public Function GetDraftPreview() As String Implements TK.Presentation.View.iViewTicketAddExt.GetDraftPreview
        Return Resource.getValue("TicketDraft.Preview")
    End Function

    Public Function GetDraftTitle() As String Implements TK.Presentation.View.iViewTicketAddExt.GetDraftTitle
        Return Resource.getValue("TicketDraft.Title")
    End Function

    Public Sub refreshCategory(Categories As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryTree), SelectedCategoryId As Int64) Implements TK.Presentation.View.iViewTicketAddExt.refreshCategory

        If SelectedCategoryId > 0 Then
            CTRLddlCat.InitDDL(Categories, SelectedCategoryId, Resource.getValue("Category.select"))
        Else
            CTRLddlCat.InitDDL(Categories, -1, Resource.getValue("Category.select"))
        End If

    End Sub


    Public Property DraftMsgId As Long Implements TK.Presentation.View.iViewTicketAddExt.DraftMsgId
        Get
            Return ViewStateOrDefault("DraftId", 0)
        End Get
        Set(value As Long)
            ViewState("DraftId") = value
        End Set
    End Property

    Private Function GetUploadedItems(draftMessage As TK.Domain.Message) As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) Implements TK.Presentation.View.iViewTicketAddExt.GetUploadedItems
        Return CTRLinternalUpload.UploadFiles(False, draftMessage)
    End Function

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

    Public Function ValidateFields() As Boolean

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

        If String.IsNullOrEmpty(Me.CTRLEditorText.HTML) Then
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


    Private Sub Save(ByVal IsDraft As Boolean)
        Me.CurrentPresenter.SaveTicket(Me.DraftData)
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
    Private Sub LNBsaveDraft_Click(sender As Object, e As System.EventArgs) Handles LNBsaveDraft.Click
        If Me.ValidateFields() Then
            Me.Save(True)
        End If
    End Sub

    Private Sub LNBsend_Click(sender As Object, e As System.EventArgs) Handles LNBsend.Click
        If Me.ValidateFields() Then
            Me.Save(False)
        End If
    End Sub


    Private Sub CTRLtopBar_UpdateInternationalization(oLingua As Comol.Entity.Lingua) Handles CTRLtopBar.UpdateInternationalization
        MyBase.UpdateLanguage(oLingua)
        Me.BindDati()
    End Sub

    Private Sub TMsession_Tick(sender As Object, e As System.EventArgs) Handles TMsession.Tick

        Me.CurrentPresenter.SendTimerAction()

        If IsNothing(Me.CurrentUser) OrElse Me.CurrentUser.UserId <= 0 Then
            Me.TMsession.Enabled = False
        End If
    End Sub

    Private Sub LNBdelete_Click(sender As Object, e As EventArgs) Handles LNBdelete.Click
        Dim respError As TK.Domain.Enums.TicketDraftDeleteError = Me.CurrentPresenter.DeleteDraft(PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)

        If respError = TK.Domain.Enums.TicketDraftDeleteError.none Then
            Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketListExternal(True))

        ElseIf Not respError = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketDraftDeleteError.hide Then

            Dim message As String = Resource.getValue("Message.DeleteDraft." & respError.ToString())
            SetErrorMessage_NEW(message, lm.Comol.Core.DomainModel.Helpers.MessageType.error)
            'Show delete error!
        End If
    End Sub

    Private Sub CTRLinternalUpload_ItemsNotAdded(action As Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLinternalUpload_MessageNotFound() Handles CTRLinternalUpload.MessageNotFound
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLinternalUpload_NoFilesToAdd(action As Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.NoFilesToAdd
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLinternalUpload_UploadFile() Handles CTRLinternalUpload.UploadToModuleObject
        CurrentPresenter.AttachmentsAddInternal(DraftData, True)
        Master.ClearOpenedDialogOnPostback()
    End Sub

    Private Sub CTRLattView_FileAction(sender As Object, e As RepeaterCommandEventArgs) Handles CTRLattView.FileAction
        Dim idAttacchment As Int64 = 0
        Int64.TryParse(e.CommandArgument, idAttacchment)
        If e.CommandName = "File_Delete" AndAlso idAttacchment > 0 Then
            CurrentPresenter.DeleteFile(idAttacchment, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
        End If
    End Sub

#End Region

  
End Class