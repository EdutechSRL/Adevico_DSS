Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.ActionDataContract

Public Class UC_CallAddAttachments
    Inherits BaseControl
    Implements IViewAddCallAttachment

#Region "Context"
    Private _Presenter As AddCallAttachmentPresenter
    Private ReadOnly Property CurrentPresenter() As AddCallAttachmentPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddCallAttachmentPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewAddCallAttachment.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewAddCallAttachment.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewAddCallAttachment.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean Implements IViewAddCallAttachment.DisplayCommands
        Get
            Return ViewStateOrDefault("DisplayCommands", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
    Private Property CurrentAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions Implements IViewAddCallAttachment.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
            ViewState("CurrentAction") = value
            Resource.setButtonByValue(BTNaddAttachment, value.ToString, True)
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewAddCallAttachment.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewAddCallAttachment.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewAddCallAttachment.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCallCommunity") = value
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
    Public Event CallNotFound(type As CallForPaperType)

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '    .setButton(BTNaddAttachment, True)
            .setLinkButton(LNBcloseAttachmentWindow, False, True)
        End With
    End Sub
#End Region

#Region "Implements"

#Region "Initializers"
    Public Sub InitializeControl(idCall As Long, type As CallForPaperType, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, Optional description As String = "", Optional dialogclass As String = "") Implements IViewAddCallAttachment.InitializeControl
        BaseInitializeControl(action, description, dialogclass)
        CurrentPresenter.InitView(idCall, type, action, identifier)
    End Sub
    Private Sub BaseInitializeControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, Optional description As String = "", Optional dialogclass As String = "")
        BTNaddAttachment.OnClientClick = ""
        If Not String.IsNullOrEmpty(dialogclass) Then
            UploaderCssClass = dialogclass
        End If
        If String.IsNullOrEmpty(description) Then
            DVdescription.Visible = False
        Else
            LTdescription.Text = description
        End If
    End Sub
#End Region

    Private Function UploadFiles(moduleCode As String, idObjectType As Integer, idAction As Integer, addToRepository As Boolean) As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) Implements IViewAddCallAttachment.UploadFiles
        If addToRepository Then
            Return CTRLrepositoryItemsUploader.AddFilesToRepository(New AttachmentFile(), 0, idObjectType, moduleCode, idAction)
        Else
            Return CTRLinternalUploader.AddModuleInternalFiles(New AttachmentFile(), 0, idObjectType, moduleCode, idAction)
        End If
    End Function

    Private Sub InitializeUploaderControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddCallAttachment.InitializeUploaderControl
        Select Case action
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CTRLinternalUploader.Visible = True
                CTRLinternalUploader.InitializeControl(PageUtility.CurrentContext.UserContext.CurrentUserID, identifier)
                'BTNaddAttachment.OnClientClick = "ProgressStart();"
        End Select
    End Sub
    Private Sub InitializeCommunityUploader(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddCallAttachment.InitializeCommunityUploader
        CTRLrepositoryItemsUploader.Visible = True
        CTRLrepositoryItemsUploader.InitializeControl(0, identifier)
    End Sub
    Private Sub InitializeLinkRepositoryItems(idUser As Integer, rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, alreadyLinkedFiles As List(Of RepositoryItemLinkBase(Of Long))) Implements IViewAddCallAttachment.InitializeLinkRepositoryItems
        CTRLlinkItems.Visible = True
        CTRLlinkItems.InitializeControl(idUser, identifier, alreadyLinkedFiles, False, False, rPermissions.Administration, rPermissions.Administration)
    End Sub

#Region "Display Messages"
    Private Sub DisplayWorkingSessionExpired() Implements IViewAddCallAttachment.DisplayWorkingSessionExpired
        BTNaddAttachment.Enabled = False
        If RaiseCommandEvents Then
            RaiseEvent WorkingSessionExpired()
        End If
    End Sub
    Private Sub DisplayItemsAdded() Implements IViewAddCallAttachment.DisplayItemsAdded
        If RaiseCommandEvents Then
            RaiseEvent ItemsAdded(CurrentAction)
        End If
    End Sub
    Private Sub DisplayItemsNotAdded() Implements IViewAddCallAttachment.DisplayItemsNotAdded
        If RaiseCommandEvents Then
            RaiseEvent ItemsNotAdded(CurrentAction)
        End If
    End Sub
    Private Sub DisplayCallNotFound(type As CallForPaperType) Implements IViewAddCallAttachment.DisplayCallNotFound
        If RaiseCommandEvents Then
            RaiseEvent CallNotFound(type)
        End If
    End Sub

    Private Sub DisplayNoFilesToAdd() Implements IViewAddCallAttachment.DisplayNoFilesToAdd
        If RaiseCommandEvents Then
            RaiseEvent NoFilesToAdd(CurrentAction)
        End If
    End Sub
#End Region

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, idAttachment As Long, action As ModuleCallForPaper.ActionType) Implements IViewAddCallAttachment.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.AttachmentFile, idAttachment.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, idAttachment As Long, action As ModuleRequestForMembership.ActionType) Implements IViewAddCallAttachment.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.AttachmentFile, idAttachment.ToString), InteractionType.UserWithLearningObject)
    End Sub
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
        Select Case CurrentAction
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CurrentPresenter.AddFilesToItem(IdCall, CallType)
        End Select
    End Sub
    Private Sub LNBcloseAttachmentWindow_Click(sender As Object, e As System.EventArgs) Handles LNBcloseAttachmentWindow.Click
        If RaiseCommandEvents Then
        End If
    End Sub
#End Region







End Class