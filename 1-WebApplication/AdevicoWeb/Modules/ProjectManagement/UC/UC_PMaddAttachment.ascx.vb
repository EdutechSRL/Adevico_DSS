Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.FileRepository.Domain

Public Class UC_PMaddAttachment
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
    Public Property isInitialized As Boolean Implements IViewAddAttachment.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
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
    Private Property IdActivity As Long Implements IViewAddAttachment.IdActivity
        Get
            Return ViewStateOrDefault("IdActivity", 0)
        End Get
        Set(value As Long)
            ViewState("IdActivity") = value
        End Set
    End Property
    Private Property IdProject As Long Implements IViewAddAttachment.IdProject
        Get
            Return ViewStateOrDefault("IdProject", 0)
        End Get
        Set(value As Long)
            ViewState("IdProject") = value
        End Set
    End Property
    Private Property IdProjectCommunity As Integer Implements IViewAddAttachment.IdProjectCommunity
        Get
            Return ViewStateOrDefault("IdProjectCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProjectCommunity") = value
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
    Public Event ProjectNotFound()
    Public Event ActivityNotFound()
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
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
    Public Sub InitializeControl(idProject As Long, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, Optional description As String = "", Optional dialogclass As String = "") Implements IViewAddAttachment.InitializeControl
        InitializeControl(idProject, 0, action, identifier, rPermissions, description, dialogclass)
    End Sub
    Public Sub InitializeControl(idProject As Long, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, Optional description As String = "", Optional dialogclass As String = "") Implements IViewAddAttachment.InitializeControl
        InitializeControl(idProject, 0, action, identifier, description, dialogclass)
    End Sub
    Public Sub InitializeControl(idProject As Long, idActivity As Long, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, Optional description As String = "", Optional dialogclass As String = "") Implements IViewAddAttachment.InitializeControl
        BaseInitializeControl(action, description, dialogclass)
        CurrentPresenter.InitView(idProject, idActivity, action, identifier)
    End Sub
    Public Sub InitializeControl(idProject As Long, idActivity As Long, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, Optional description As String = "", Optional dialogclass As String = "") Implements IViewAddAttachment.InitializeControl
        BaseInitializeControl(action, description, dialogclass)
        CurrentPresenter.InitView(idProject, idActivity, action, identifier, rPermissions)
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

    Private Function UploadFiles(activity As PmActivity, addToRepository As Boolean) As List(Of dtoModuleUploadedItem) Implements IViewAddAttachment.UploadFiles
        If addToRepository Then
            Return CTRLrepositoryItemsUploader.AddFilesToRepository(activity, activity.Id, CInt(ModuleProjectManagement.ObjectType.Task), ModuleProjectManagement.UniqueCode, ModuleProjectManagement.ActionType.AssignmentDownload)
        Else
            Return CTRLinternalUploader.AddModuleInternalFiles(activity, activity.Id, CInt(ModuleProjectManagement.ObjectType.Task), ModuleProjectManagement.UniqueCode, ModuleProjectManagement.ActionType.AssignmentDownload)
        End If
    End Function
    Private Function UploadFiles(project As Project, addToRepository As Boolean) As List(Of dtoModuleUploadedItem) Implements IViewAddAttachment.UploadFiles
        If addToRepository Then
            Return CTRLrepositoryItemsUploader.AddFilesToRepository(project, project.Id, CInt(ModuleProjectManagement.ObjectType.Project), ModuleProjectManagement.UniqueCode, ModuleProjectManagement.ActionType.AssignmentDownload)
        Else
            Return CTRLinternalUploader.AddModuleInternalFiles(project, project.Id, CInt(ModuleProjectManagement.ObjectType.Project), ModuleProjectManagement.UniqueCode, ModuleProjectManagement.ActionType.AssignmentDownload)
        End If
    End Function
    Private Sub InitializeUploaderControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddAttachment.InitializeUploaderControl
        Select Case action
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem
                CTRLurls.Visible = True
                CTRLurls.InitializeControl()
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CTRLinternalUploader.Visible = True
                CTRLinternalUploader.InitializeControl(PageUtility.CurrentContext.UserContext.CurrentUserID, identifier)
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
    Private Sub DisplayActivityNotFound() Implements IViewAddAttachment.DisplayActivityNotFound
        If RaiseCommandEvents Then
            RaiseEvent ActivityNotFound()
        End If
    End Sub
    Private Sub DisplayProjectNotFound() Implements IViewAddAttachment.DisplayProjectNotFound
        If RaiseCommandEvents Then
            RaiseEvent ProjectNotFound()
        End If
    End Sub
    Private Sub DisplayNoFilesToAdd() Implements IViewAddAttachment.DisplayNoFilesToAdd
        If RaiseCommandEvents Then
            RaiseEvent NoFilesToAdd(CurrentAction)
        End If
    End Sub
#End Region

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idProject As Long, idActivity As Long, action As ModuleProjectManagement.ActionType) Implements IViewAddAttachment.SendUserAction
        If idActivity > 0 Then
            Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Task, idActivity.ToString), InteractionType.UserWithLearningObject)
        Else
            Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Project, idProject.ToString), InteractionType.UserWithLearningObject)
        End If
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
        Dim objOwner As lm.Comol.Core.DomainModel.ModuleObject '= GetOwner()
        Select Case CurrentAction
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem
                CurrentPresenter.AddUrlToItem(IdProject, IdActivity, CTRLurls.GetUrls)
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity
                CurrentPresenter.AddCommunityFilesToItem(IdProject, IdActivity, CTRLlinkItems.GetNewRepositoryItemLinks())
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CurrentPresenter.AddFilesToItem(IdProject, IdActivity)
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity
                CurrentPresenter.AddCommunityFilesToItem(IdProject, IdActivity)
        End Select
    End Sub
    Private Sub LNBcloseAttachmentWindow_Click(sender As Object, e As System.EventArgs) Handles LNBcloseAttachmentWindow.Click
        If RaiseCommandEvents Then
        End If
    End Sub
#End Region

   
End Class