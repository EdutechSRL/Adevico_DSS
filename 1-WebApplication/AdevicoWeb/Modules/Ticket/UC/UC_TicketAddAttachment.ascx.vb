Imports lm.Comol.Core.BaseModules.Tickets.Presentation
Imports lm.Comol.Core.BaseModules.Tickets.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.Tickets

Public Class UC_TicketAddAttachment
    Inherits BaseControl
    Implements IViewAddAttachment



#Region "Context"
    Private _Presenter As AddAttachmentPresenter
    Private ReadOnly Property CurrentPresenter() As AddAttachmentPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddAttachmentPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdMessage As Long Implements IViewAddAttachment.IdMessage
        Get
            Return ViewStateOrDefault("IdMessage", 0)
        End Get
        Set(value As Long)
            ViewState("IdMessage") = value
        End Set
    End Property
    Public Property IdTicketUser As Long Implements IViewAddAttachment.IdTicketUser
        Get
            Return ViewStateOrDefault("IdTicketUser", 0)
        End Get
        Set(value As Long)
            ViewState("IdTicketUser") = value
        End Set
    End Property
    Private Property IdTicketCommunity As Integer Implements IViewAddAttachment.IdTicketCommunity
        Get
            Return ViewStateOrDefault("IdTicketCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTicketCommunity") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewAddAttachment.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property AllowAnonymousUpload As Boolean Implements IViewAddAttachment.AllowAnonymousUpload
        Get
            Return ViewStateOrDefault("AllowAnonymousUpload", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAnonymousUpload") = value
        End Set
    End Property

    Public Property DisplayDescription As Boolean Implements IViewAddAttachment.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewAddAttachment.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean Implements IViewAddAttachment.DisplayCommands
        Get
            Return ViewStateOrDefault("DisplayCommands", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
    Private Property CurrentAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions Implements IViewAddAttachment.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
            ViewState("CurrentAction") = value
            Resource.setButtonByValue(BTNaddAttachment, value.ToString, True)
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Property UploaderCssClass As String
        Get
            Return ViewStateOrDefault("UploaderCssClass", "")
        End Get
        Set(value As String)
            ViewState("UploaderCssClass") = value
        End Set
    End Property
    Public Event WorkingSessionExpired()
    Public Event ItemsAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
    Public Event ItemsNotAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
    Public Event NoFilesToAdd(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
    Public Event MessageNotFound()

    Public Event UploadToModuleObject()
    Public Event UploadToRepository()
    Public Event LinkFromRepository(items As List(Of ModuleActionLink))

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditResolver", "Modules", "Ticket")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setButton(BTNaddAttachment, True, False, False, True)
            .setLinkButton(LNBcloseAttachmentWindow, True, True)
            .setLiteral(LTdescription)
        End With
    End Sub
#End Region

#Region "Implements"

#Region "Initializers"
    Public Sub InitializeControl(idMessage As Long, action As Repository.RepositoryAttachmentUploadActions, idCommunity As Integer, _allowAnonymousUpload As Boolean, Optional idUser As Integer = 0, Optional dialogclass As String = "") Implements IViewAddAttachment.InitializeControl
        BaseInitializeControl(action, dialogclass)
        AllowAnonymousUpload = _allowAnonymousUpload
        Select Case action
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CTRLinternalUploader.AllowAnonymousUpload = _allowAnonymousUpload
        End Select

        If idUser = 0 Then
            idUser = PageUtility.CurrentContext.UserContext.CurrentUserID
        End If

        CurrentPresenter.InitView(idMessage, action, idCommunity, idUser)
    End Sub
    Public Sub InitializeControl(idMessage As Long, action As Repository.RepositoryAttachmentUploadActions, idCommunity As Integer, rPermissions As ModuleRepository) Implements IViewAddAttachment.InitializeControl
        BaseInitializeControl(action)
        CurrentPresenter.InitView(idMessage, action, idCommunity, rPermissions)
    End Sub
    Public Sub InitializeControl(idMessage As Long, action As Repository.RepositoryAttachmentUploadActions, identifier As RepositoryIdentifier, rPermissions As ModuleRepository) Implements IViewAddAttachment.InitializeControl
        BaseInitializeControl(action)
        CurrentPresenter.InitView(idMessage, action, identifier, rPermissions)
    End Sub


    Private Sub BaseInitializeControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, Optional dialogclass As String = "")
        BTNaddAttachment.OnClientClick = ""
        If Not String.IsNullOrEmpty(dialogclass) Then
            UploaderCssClass = dialogclass
        End If
    End Sub
#End Region

    Public Function UploadFiles(addToRepository As Boolean, message As Message) As List(Of dtoModuleUploadedItem) Implements IViewAddAttachment.UploadFiles
        If addToRepository Then
            Return CTRLrepositoryItemsUploader.AddFilesToRepository(message, message.Id, CInt(ModuleTicket.ObjectType.Message), ModuleTicket.UniqueCode, ModuleTicket.ActionType.MessageAttachDownload)
        Else
            Return CTRLinternalUploader.AddModuleInternalFiles(message, message.Id, CInt(ModuleTicket.ObjectType.Message), ModuleTicket.UniqueCode, ModuleTicket.ActionType.MessageAttachDownload)
        End If
    End Function
    Private Sub InitializeUploaderControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, idUploader As Integer) Implements IViewAddAttachment.InitializeUploaderControl
        Select Case action
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CTRLinternalUploader.Visible = True
                CTRLinternalUploader.InitializeControl(idUploader, identifier)
                'BTNaddAttachment.OnClientClick = "ProgressStart();"
        End Select
    End Sub
    Private Sub InitializeCommunityUploader(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddAttachment.InitializeCommunityUploader
        CTRLrepositoryItemsUploader.Visible = True
        CTRLrepositoryItemsUploader.InitializeControl(0, identifier)
    End Sub
    Private Sub InitializeLinkRepositoryItems(idUser As Integer, rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, alreadyLinkedFiles As List(Of RepositoryItemLinkBase(Of Long))) Implements IViewAddAttachment.InitializeLinkRepositoryItems
        CTRLlinkItems.Visible = True
        CTRLlinkItems.InitializeControl(idUser, identifier, alreadyLinkedFiles, False, False, rPermissions.Administration, rPermissions.Administration)
    End Sub

#Region "Display Messages"
    Private Sub DisplayWorkingSessionExpired() Implements IViewAddAttachment.DisplayWorkingSessionExpired
        BTNaddAttachment.Enabled = False
        If RaiseCommandEvents Then
            RaiseEvent WorkingSessionExpired()
        End If
    End Sub
    Private Sub DisplayItemsAdded() Implements IViewAddAttachment.DisplayItemsAdded
        If RaiseCommandEvents Then
            RaiseEvent ItemsAdded(CurrentAction)
        End If
    End Sub
    Private Sub DisplayItemsNotAdded() Implements IViewAddAttachment.DisplayItemsNotAdded
        If RaiseCommandEvents Then
            RaiseEvent ItemsNotAdded(CurrentAction)
        End If
    End Sub
    Private Sub DisplayMessageNotFound() Implements IViewAddAttachment.DisplayMessageNotFound
        If RaiseCommandEvents Then
            RaiseEvent MessageNotFound()
        End If
    End Sub
    Private Sub DisplayNoFilesToAdd() Implements IViewAddAttachment.DisplayNoFilesToAdd
        If RaiseCommandEvents Then
            RaiseEvent NoFilesToAdd(CurrentAction)
        End If
    End Sub
#End Region


#End Region

#Region "Internal"
    Private Sub CTRLinternalUploader_IsValidOperation(ByRef isvalid As Boolean) Handles CTRLinternalUploader.IsValidOperation
        isvalid = True
    End Sub

    Private Sub CTRLrepositoryItemsUploader_AllowUploadUpdate(allowUpload As Boolean) Handles CTRLrepositoryItemsUploader.AllowUploadUpdate
        BTNaddAttachment.Enabled = allowUpload
    End Sub
    Private Sub CTRLrepositoryItemsUploader_IsValidOperation(ByRef isvalid As Boolean) Handles CTRLrepositoryItemsUploader.IsValidOperation
        isvalid = True
    End Sub
    Private Sub BTNaddAttachment_Click(sender As Object, e As System.EventArgs) Handles BTNaddAttachment.Click
        Dim objOwner As lm.Comol.Core.DomainModel.ModuleObject '= GetOwner()
        Select Case CurrentAction
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity
                RaiseEvent LinkFromRepository(CTRLlinkItems.GetNewRepositoryItemLinks())
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                RaiseEvent UploadToModuleObject()
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity
                RaiseEvent UploadToRepository()
        End Select
    End Sub
    Private Sub LNBcloseAttachmentWindow_Click(sender As Object, e As System.EventArgs) Handles LNBcloseAttachmentWindow.Click
        If RaiseCommandEvents Then
        End If
    End Sub
#End Region



   
End Class