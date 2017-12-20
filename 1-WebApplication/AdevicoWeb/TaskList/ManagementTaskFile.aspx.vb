Imports lm.Comol.Modules.Base.Presentation
'Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
'Imports lm.Comol.Core.BaseModules.CommunityDiary.Presentation
'Imports lm.Comol.Core.BaseModules.CommunityDiary.Domain
'Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList
Imports lm.Comol.Modules.TaskList.Domain

    Partial Public Class ManagementTaskFile
        Inherits PageBase
    Implements IViewManagementTaskFile

#Region "Default view"
        Private _BaseUrl As String
        Private _PageUtility As OLDpageUtility
    Private _CommunityTaskPermissions As IList(Of ModuleCommunityPermission(Of lm.Comol.Modules.TaskList.ModuleTasklist))
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ManagementTaskFilePresenter
    Private _RepositoryPermission As List(Of CoreModuleRepository)
    Protected ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of lm.Comol.Modules.TaskList.ModuleTasklist)) Implements IViewManagementTaskFile.CommunitiesPermission
        Get
            If IsNothing(_CommunityTaskPermissions) Then
                _CommunityTaskPermissions = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, ModuleTasklist.UniqueID) _
                                          Select New ModuleCommunityPermission(Of lm.Comol.Modules.TaskList.ModuleTasklist)() With {.ID = sb.CommunityID, .Permissions = New ModuleTasklist(sb.PermissionLong)}).ToList
            End If
            Return _CommunityTaskPermissions
        End Get
    End Property
    Public Property AllowPublish() As Boolean Implements IViewManagementTaskFile.AllowPublish
        Get
            If TypeOf Me.ViewState("AllowPublish") Is Boolean Then
                Return CBool(Me.ViewState("AllowPublish"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowPublish") = value
        End Set
    End Property
        Public Overloads ReadOnly Property BaseUrl() As String
            Get
                If _BaseUrl = "" Then
                    _BaseUrl = Me.PageUtility.BaseUrl
                End If
                Return _BaseUrl
            End Get
    End Property

    Public ReadOnly Property CurrentPresenter() As ManagementTaskFilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ManagementTaskFilePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public Property CurrentItemID() As Long Implements IViewManagementTaskFile.ItemID
        Get
            If IsNumeric(Me.ViewState("CurrentTaskID")) Then
                Return CLng(Me.ViewState("CurrentTaskID"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property
    Public Property ItemCommunityId As Integer Implements IViewManagementTaskFile.ItemCommunityId
        Get
            If IsNumeric(Me.ViewState("TaskId")) Then
                Return CInt(Me.ViewState("TaskId"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("TaskId") = value
        End Set
    End Property
#End Region

#Region "Interface"
    Public WriteOnly Property AllowUpload() As Boolean Implements IViewManagementTaskFile.AllowUpload
        Set(ByVal value As Boolean)
            Me.HYPmultipleUpload.Visible = value
        End Set
    End Property

        'Public WriteOnly Property BackToItem() As Long Implements IViewManagementFile.BackToItem
        '    Set(ByVal value As Long)
        '        Me.HYPbackToItems.Visible = Not (value = -1)
        '        Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & RootObject.CommunityDiary(value, Me.PreloadedItemID)
        '    End Set
        'End Property
    Public Sub SetBackToItemsUrl(ByVal idCommunity As Integer, ByVal itemId As Long) Implements IViewManagementTaskFile.SetBackToItemsUrl
        Me.HYPbackToItems.Visible = True
        'Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & lm.Comol.Modules.TaskList.Domain.RootObject.TodayTask
        Me.HYPbackToItems.NavigateUrl = "~/" & lm.Comol.Modules.TaskList.Domain.RootObject.TodayTask
    End Sub
    Public ReadOnly Property PreloadedItemID() As Long Implements IViewManagementTaskFile.PreloadedItemID
        Get
            If IsNumeric(Request.QueryString("TaskID")) Then
                Return CLng(Request.QueryString("TaskID"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public Function RepositoryPermission(ByVal CommunityID As Integer) As CoreModuleRepository Implements IViewManagementTaskFile.RepositoryPermission
        Dim oModule As CoreModuleRepository = Nothing
        If CommunityID = 0 Then
            oModule = CoreModuleRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, CoreModuleRepository.UniqueID) _
                  Where sb.CommunityID = CommunityID Select New CoreModuleRepository(sb.PermissionString)).FirstOrDefault
            If IsNothing(oModule) Then
                oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, CoreModuleRepository.UniqueID, True) _
                 Where sb.CommunityID = CommunityID Select New CoreModuleRepository(sb.PermissionString)).FirstOrDefault
                If IsNothing(oModule) Then
                    oModule = New CoreModuleRepository
                End If
            End If
        End If
        Return oModule
    End Function
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

#Region "Default inherited"
        Public Overrides Sub BindDati()
            Me.Master.ShowNoPermission = False
            If Me.Page.IsPostBack = False Then
                Me.CurrentPresenter.InitView()
            Else
                Me.CurrentPresenter.ReloadManagementFileView()
            End If
        End Sub

        Public Overrides Sub BindNoPermessi()
            Me.Master.ShowNoPermission = True
            'Me.PageUtility.AddAction###(ModuleCommunityDiary.ActionType.NoPermission, Nothing, InteractionType.Generic)
        End Sub

        Public Overrides Function HasPermessi() As Boolean
            Return True
        End Function

        Public Overrides Sub RegistraAccessoPagina()

        End Sub

        Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_TaskListManagementFile", "TaskList")
        End Sub

        Public Overrides Sub SetInternazionalizzazione()
            With MyBase.Resource
                Me.Master.ServiceTitle = .getValue("serviceTitleManagementFile")
                Me.Master.ServiceNopermission = .getValue("nopermissionManagementFile")
                .setLiteral(Me.LTuploadToCommunity_t)
                .setLiteral(Me.LTuploadToDiaryItem_t)
                .setLiteral(Me.LTitemFiles_t)
                .setLiteral(Me.LTaddFromCommunity_t)
                .setHyperLink(Me.HYPmultipleUpload, True, True)
                .setHyperLink(Me.HYPbackToItem, True, True)
                .setHyperLink(Me.HYPbackToItems, True, True)
                .setLiteral(Me.LTlinkToCommunity)
                .setButton(Me.BTNlinkCommunityFile, True, , , True)
                .setButton(Me.BTNaddCommunityFile, True, , , True)
                .setButton(Me.BTNaddToItem, True, , , True)
                Me.BTNaddCommunityFile.OnClientClick = "return HideWorkBookUpload();"
                Me.BTNaddToItem.OnClientClick = "return HideCommunityUpload();"
            End With
        End Sub

        Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

        End Sub
#End Region

    Public Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewManagementTaskFile.NoPermissionToManagementFiles
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTaskList.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Sub ReturnToItemsList(ByVal CommunityID As Integer, ByVal TaskID As Long) Implements IViewManagementTaskFile.ReturnToItemsList
        Me.PageUtility.RedirectToUrl(lm.Comol.Modules.TaskList.Domain.RootObject.TaskDetailEditable(TaskID))
        ' Me.PageUtility.RedirectToUrl("Modules/CommunityDiary/CommunityDiary.aspx?CommunityID=" & CommunityID.ToString & "#" & ItemID.ToString)
    End Sub


#Region "VERIFICARE"
    Public Sub InitializeCommunityUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As CoreModuleRepository) Implements IViewManagementTaskFile.InitializeCommunityUploader
        Me.CTRLCommunityFile.InitializeControl(FolderID, CommunityID, ItemRepositoryToCreate.File, lm.Comol.Modules.Base.DomainModel.ModuleCommunityRepository.CreateFromCore(oPermission))
    End Sub
    Public Sub InitializeModuleUploader(ByVal CommunityID As Integer) Implements IViewManagementTaskFile.InitializeModuleUploader
        Me.DVinternal.Visible = True
        Me.CTRLinternalFileUploader.InitializeControl(CommunityID, False)
    End Sub
    Public Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewManagementTaskFile.AddCommunityFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTasklist.ActionType.AddFile, Me.PageUtility.CreateObjectsList(ModuleID, ModuleTasklist.ObjectType.Task, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub AddModuleFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewManagementTaskFile.AddModuleFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTasklist.ActionType.AddFile, Me.PageUtility.CreateObjectsList(ModuleID, ModuleTasklist.ObjectType.Task, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub AddManagementFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewManagementTaskFile.AddManagementFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTasklist.ActionType.EditItemNoDate, Me.PageUtility.CreateObjectsList(ModuleID, ModuleTasklist.ObjectType.Task, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub SendActionEditFileItemVisibility(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal IdItemFileLink As Long, ByVal isVisible As Boolean) Implements IViewManagementTaskFile.SendActionEditFileItemVisibility
        Me.PageUtility.AddActionToModule(IdCommunity, IdModule, IIf(isVisible, ModuleTasklist.ActionType.ShowTaskFile, ModuleTasklist.ActionType.HideTaskFile), Me.PageUtility.CreateObjectsList(IdModule, ModuleTasklist.ObjectType.TaskLinkedFile, IdCommunity.ToString), InteractionType.UserWithLearningObject)
    End Sub
    'Public Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IViewManagementFile.NotifyAddCommunityFile
    '    Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
    '    oService.NotifyItemAddCommunityFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, "")
    'End Sub

        'Public Sub NotifyAddInternalFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IViewManagementFile.NotifyAddInternalFile
        '    Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        '    oService.NotifyItemAddInternalFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, "")
        'End Sub
#End Region

    Public WriteOnly Property AllowCommunityUpload() As Boolean Implements IViewManagementTaskFile.AllowCommunityUpload
        Set(ByVal value As Boolean)
            Me.DVcommunity.Visible = value
        End Set
    End Property

    Public WriteOnly Property AllowCommunityLink() As Boolean Implements IViewManagementTaskFile.AllowCommunityLink
        Set(ByVal value As Boolean)
            Me.DVcommunityLink.Visible = value
        End Set
    End Property

    Public Sub ReturnToFileManagementWithErrors(ByVal NUinternalFile As List(Of dtoModuleUploadedFile), ByVal NUcommunityFile As List(Of dtoUploadedFile)) Implements IViewManagementTaskFile.ReturnToFileManagementWithErrors
        If (IsNothing(NUcommunityFile) OrElse NUcommunityFile.Count = 0) AndAlso (IsNothing(NUinternalFile) OrElse NUinternalFile.Count = 0) Then
            Me.PageUtility.RedirectToUrl(lm.Comol.Modules.TaskList.Domain.RootObject.ItemManagementFiles(Me.CurrentItemID))
            '"Modules/CommunityDiary/CDitemManagementFile.aspx?ItemID=" & Me.CurrentItemID.ToString)
        Else
            Dim oRemotePost As New lm.Comol.Modules.Base.DomainModel.RemotePost
            oRemotePost.Url = Me.BaseUrl & lm.Comol.Modules.TaskList.Domain.RootObject.ItemError()
            oRemotePost.Add("BackUrl", lm.Comol.Modules.TaskList.Domain.RootObject.ItemManagementFiles(CurrentItemID))
            Dim index As Integer = 0
            If Not IsNothing(NUinternalFile) Then
                For Each dto As dtoModuleUploadedFile In NUinternalFile
                    oRemotePost.Add("FILE_I_" & index.ToString, dto.File.DisplayName)
                    oRemotePost.Add("FILE_I_SavedFilePath_" & index.ToString, dto.SavedFilePath)
                    oRemotePost.Add("FILE_I_SavedName_" & index.ToString, dto.File.Name)
                    oRemotePost.Add("FILE_I_SavedExtension_" & index.ToString, dto.File.Extension)
                    index += 1
                Next
            End If
            If Not IsNothing(NUcommunityFile) Then
                index = 0
                For Each oFile As dtoUploadedFile In NUcommunityFile
                    oRemotePost.Add("FILE_C_Name_" & index.ToString, oFile.File.DisplayName)
                    oRemotePost.Add("FILE_C_isFile_" & index.ToString, oFile.File.isFile)
                    oRemotePost.Add("FILE_C_Status_" & index.ToString, oFile.Status.ToString)
                    oRemotePost.Add("FILE_C_FolderId_" & index.ToString, oFile.File.FolderId)
                    oRemotePost.Add("FILE_C_SavedFilePath_" & index.ToString, oFile.SavedFilePath)
                    oRemotePost.Add("FILE_C_SavedName_" & index.ToString, oFile.File.Name)
                    oRemotePost.Add("FILE_C_SavedExtension_" & index.ToString, oFile.File.Extension)
                    index += 1
                Next
            End If
            oRemotePost.Add("FromView", "management")
            oRemotePost.Add("TaskID", Me.CurrentItemID.ToString)
            oRemotePost.Post()
        End If
    End Sub

        Private Sub BTNlinkCommunityFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlinkCommunityFile.Click
        Me.PageUtility.RedirectToUrl(lm.Comol.Modules.TaskList.Domain.RootObject.LinkFileToItem(CurrentItemID))
            '("Modules/CommunityDiary/DiaryItemAddCommunityFile.aspx?ItemID=" & Me.CurrentItemID.ToString)
        End Sub

        Private Sub BTNaddCommunityFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddCommunityFile.Click
            Me.CurrentPresenter.AddCommunityFile(Me.CTRLCommunityFile.AddFileToCommunityRepository, Me.CTRLCommunityFile.DownlodableByCommunity)
        End Sub

        Private Sub BTNaddToItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddToItem.Click
            Me.CurrentPresenter.AddInternalFile()
        End Sub
    Public Function GetUploadedModuleFile(ByVal item As Object, ByVal itemTypeId As Integer, ByVal moduleCode As String, ByVal moduleOwnerActionID As Integer, ByVal moduleId As Integer) As ModuleActionLink Implements IViewManagementTaskFile.GetUploadedModuleFile
        Return Me.CTRLinternalFileUploader.UploadAndLinkInternalFile(FileRepositoryType.InternalLong, item, moduleCode, moduleOwnerActionID, itemTypeId)
    End Function

    Public Sub ReturnToFileManagement(ByVal CommunityID As Integer, ByVal ItemID As Long) Implements IViewManagementTaskFile.ReturnToFileManagement
        Me.PageUtility.RedirectToUrl(lm.Comol.Modules.TaskList.Domain.RootObject.ItemManagementFiles(ItemID))
        ' Me.PageUtility.RedirectToUrl("Modules/CommunityDiary/CDitemManagementFile.aspx?ItemID=" & ItemID.ToString)
    End Sub

    Public Sub LoadFilesToManage(ByVal ItemID As Long, ByVal oPermission As CoreItemPermission, ByVal files As IList(Of iCoreItemFileLink(Of Long)), ByVal urlToPublish As String) Implements IViewManagementTaskFile.LoadFilesToManage
        Me.CTRLItemManagementFile.Visible = True
        Me.CTRLItemManagementFile.ShowManagementButtons = oPermission.AllowEdit
        Me.CTRLItemManagementFile.InitalizeControl(ItemID, oPermission, files, urlToPublish)
    End Sub

        'Private Sub CTRLmanagementFile_UpdateToParent() Handles CTRLmanagementFile.UpdateToParent
        '    Me.ReturnToFileManagement(Me.CurrentItemID)
        'End Sub

    Public Sub SetBackToItemUrl(ByVal CommunityId As Integer, ByVal TaskID As Long) Implements IViewManagementTaskFile.SetBackToItemUrl
        Me.HYPbackToItem.Visible = Not (TaskID = 0)
        Me.HYPbackToItem.NavigateUrl = Me.BaseUrl & lm.Comol.Modules.TaskList.Domain.RootObject.TaskDetailEditable(TaskID)
        '& "Modules/CommunityDiary/DiaryItem.aspx?ItemID=" & ItemID.ToString & "&CommunityId=" & CommunityId.ToString
    End Sub

    Public Sub SetMultipleUploadUrl(ByVal TaskID As Long) Implements IViewManagementTaskFile.SetMultipleUploadUrl
        Me.HYPmultipleUpload.Visible = Not (TaskID = 0)
        Me.HYPmultipleUpload.NavigateUrl = Me.BaseUrl & lm.Comol.Modules.TaskList.Domain.RootObject.MultipleUploadToItem(TaskID)
        '"Modules/CommunityDiary/CDmultipleUpload.aspx?ItemID=" & ItemID.ToString
    End Sub

        'Public Sub LoadEditingPermission(ByVal ItemID As Long, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal GotoPage As RepositoryPage, ByVal DiaryItemID As Long) Implements IViewManagementFile.LoadEditingPermission
        '    Me.RedirectToUrl("Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" & ItemID.ToString & "&FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&PreviousPage=" & GotoPage.ToString & "&Action=" & PermissionAction.SingleUpload & "&RemoteItemID=" & DiaryItemID.ToString & "&PreserveUrl=True&ForService=true")
    'End Sub


    Public Sub LoadEditingPermission(ByVal fileId As Long, ByVal communityId As Integer, ByVal folderId As Long, ByVal TaskID As Long) Implements IViewManagementTaskFile.LoadEditingPermission
        Dim DestinationUrl As String = Request.Url.LocalPath
        If Me.BaseUrl <> "/" Then
            DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
        End If
        DestinationUrl = Server.HtmlEncode(DestinationUrl & Request.Url.Query)
        Dim view As ContentView = ContentView.viewAll

        Me.RedirectToUrl(lm.Comol.Core.BaseModules.Repository.Domain.RootObject.ModuleEditingSingleItemPermission(fileId, folderId, communityId, DestinationUrl, view))
    End Sub

    Private Sub CTRLItemManagementFile_EditFileItemVisibility(ByVal TaskID As Long, ByVal TaskLinkId As Long, ByVal visibleForModule As Boolean, ByVal visibleForRepository As Boolean) Handles CTRLItemManagementFile.EditFileItemVisibility
        CurrentPresenter.EditFileItemVisibility(TaskID, TaskLinkId, visibleForModule, visibleForRepository)
    End Sub
        'Private Sub CTRLItemManagementFile_EditCommunityItemVisibility(ByVal ItemID As Long, ByVal ItemLinkId As Long) Handles CTRLItemManagementFile.EditCommunityItemVisibility
        '    CurrentPresenter.EditFileRepositoryVisibility(ItemID, ItemLinkId)
        'End Sub

        'Private Sub CTRLItemManagementFile_EditItemVisibility(ByVal ItemID As Long, ByVal ItemLinkId As Long) Handles CTRLItemManagementFile.EditItemVisibility
        '    CurrentPresenter.EditFileVisibility(ItemID, ItemLinkId)
        'End Sub

    Private Sub CTRLItemManagementFile_PhysicalDelete(ByVal TaskID As Long, ByVal TaskLinkId As Long) Handles CTRLItemManagementFile.PhysicalDelete
        Dim cacheKey As String = "CommunityRepositorySize_" & Me.ItemCommunityId
        Dim CommunityPath As String = ""
        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
            CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
        Else
            CommunityPath = Me.SystemSettings.File.Materiale.DrivePath
        End If

        GenericCacheManager.PurgeCacheItems(cacheKey)
        Me.CurrentPresenter.PhisicalDelete(TaskID, TaskLinkId, CommunityPath)
    End Sub

    Private Sub CTRLItemManagementFile_UnDelete(ByVal TaskID As Long, ByVal TaskLinkId As Long) Handles CTRLItemManagementFile.UnDelete
        CurrentPresenter.VirtualUndelete(TaskID, TaskLinkId)
    End Sub

    Private Sub CTRLItemManagementFile_UnlinkRepositoryItem(ByVal TaskID As Long, ByVal TaskLinkId As Long) Handles CTRLItemManagementFile.UnlinkRepositoryItem
        CurrentPresenter.UnlinkRepositoryItem(TaskID, TaskLinkId)
    End Sub

    Private Sub CTRLItemManagementFile_VirtualDelete(ByVal TaskID As Long, ByVal TaskLinkId As Long) Handles CTRLItemManagementFile.VirtualDelete
        CurrentPresenter.VirtualDelete(TaskID, TaskLinkId)
    End Sub


    End Class

