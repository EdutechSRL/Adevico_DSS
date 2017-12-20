Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList
Imports lm.Comol.Modules.TaskList.Domain


Public Class TaskMultipleUpload
    Inherits PageBase
    Implements lm.Comol.Modules.TaskList.IViewTaskMultipleUpload

#Region "Default view"
    Private _BaseUrl As String
    Private _PageUtility As OLDpageUtility
    Private _CommunityDiaryPermissions As IList(Of ModuleCommunityPermission(Of ModuleTasklist))
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As TaskMultipleUploadPresenter

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
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As TaskMultipleUploadPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TaskMultipleUploadPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property


#End Region

#Region "interface"
    Public WriteOnly Property AllowUpload() As Boolean Implements IViewTaskMultipleUpload.AllowUpload
        Set(ByVal value As Boolean)
            Me.LNBupload.Enabled = value
        End Set
    End Property
    Public Sub SetUrlToFileManagement(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewTaskMultipleUpload.SetUrlToFileManagement
        Me.HYPbackToFileManagement.Visible = Not (IdItem = 0)
        Me.HYPbackToFileManagement.NavigateUrl = Me.BaseUrl & RootObject.ItemManagementFiles(IdItem)
    End Sub
    Public Sub SetUrlToItem(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewTaskMultipleUpload.SetUrlToItem
        Me.HYPbackToItem.Visible = Not (IdItem = 0)
        Me.HYPbackToItem.NavigateUrl = Me.BaseUrl & RootObject.TaskDetailEditable(IdItem)
    End Sub
    Public Sub SetUrlToDiary(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewTaskMultipleUpload.SetUrlToDiary
        Me.HYPbackToItems.Visible = True 'Not (IdCommunity = 0)
        Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & RootObject.TodayTask
    End Sub
    Public Property CurrentItemID() As Long Implements IViewTaskMultipleUpload.CurrentItemID
        Get
            If IsNumeric(Me.ViewState("CurrentItemID")) Then
                Return CLng(Me.ViewState("CurrentItemID"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentItemID") = value
        End Set
    End Property
    Public Property CurrentItemCommunityID As Integer Implements IViewTaskMultipleUpload.CurrentItemCommunityID
        Get
            If IsNumeric(Me.ViewState("CurrentItemCommunityID")) Then
                Return CInt(Me.ViewState("CurrentItemCommunityID"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentItemCommunityID") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedCommunityID() As Integer Implements IViewTaskMultipleUpload.PreloadedCommunityID
        Get
            If IsNumeric(Request.QueryString("CommunityID")) Then
                Return CInt(Request.QueryString("CommunityID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public WriteOnly Property AllowCommunityUpload() As Boolean Implements IViewTaskMultipleUpload.AllowCommunityUpload
        Set(ByVal value As Boolean)
            Me.DVcommunity.Visible = value
            Me.DVcommunity.Style.Add("display", IIf(value, "block", "none"))
        End Set
    End Property
    Public ReadOnly Property PreloadedItemID() As Long Implements IViewTaskMultipleUpload.PreloadedItemID
        Get
            If IsNumeric(Request.QueryString("TaskID")) Then
                Return CLng(Request.QueryString("TaskID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public Function RepositoryPermission(ByVal CommunityID As Integer) As CoreModuleRepository Implements IViewTaskMultipleUpload.RepositoryPermission
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
    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTasklist)) Implements IViewTaskMultipleUpload.CommunitiesPermission
        Get
            If IsNothing(_CommunityDiaryPermissions) Then
                _CommunityDiaryPermissions = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, ModuleTasklist.UniqueID, True) _
                                          Select New ModuleCommunityPermission(Of ModuleTasklist)() With {.ID = sb.CommunityID, .Permissions = New ModuleTasklist(sb.PermissionLong)}).ToList
            End If
            Return _CommunityDiaryPermissions
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Private Sub LNBupload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBupload.Click
    '    Me.CurrentPresenter.AddFiles(Me.CTRLCommunityFile.AddFilesToCommunityRepository, _
    '                                 Me.CTRLdiaryUpload.AddToUserRepository(Me.PageUtility.BaseUserRepositoryPath))
    'End Sub

#Region "default method"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
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
            Me.Master.ServiceTitle = .getValue("serviceTitleMultipleUpload")
            Me.Master.ServiceNopermission = .getValue("nopermissionMultipleUpload")
            .setLiteral(Me.LTuploadToCommunity_t)
            .setLiteral(Me.LTuploadToDiary_t)
            .setLinkButton(Me.LNBupload, True, True)
            .setHyperLink(Me.HYPbackToFileManagement, True, True)
            .setHyperLink(Me.HYPbackToItem, True, True)
            .setHyperLink(Me.HYPbackToItems, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "File uploaders"
    Private Sub LNBupload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBupload.Click
        Me.CurrentPresenter.AddFiles(Me.CTRLRepositoryUpload.AddFilesToCommunityRepository)
    End Sub
    Public Sub InitializeCommunityUploader(ByVal CommunityID As Integer, ByVal repository As CoreModuleRepository) Implements IViewTaskMultipleUpload.InitializeCommunityUploader
        Me.CTRLRepositoryUpload.InitializeControl(0, CommunityID, repository)
    End Sub
    Public Sub InitializeModuleUploader(ByVal IdCommmunity As Integer) Implements IViewTaskMultipleUpload.InitializeModuleUploader
        Me.CTRLmoduleUpload.InitializeControl(IdCommmunity)
    End Sub

    Public Function GetUploadedModuleFile(ByVal item As Task, ByVal itemTypeId As Integer, ByVal moduleCode As String, ByVal moduleOwnerActionID As Integer, ByVal moduleId As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile) Implements IViewTaskMultipleUpload.GetUploadedModuleFile
        Return Me.CTRLmoduleUpload.AddModuleInternalFiles(FileRepositoryType.InternalLong, item, moduleCode, moduleOwnerActionID, itemTypeId)
    End Function

#End Region
    Public Sub ReturnToFileManagement(ByVal IdCommmunity As Integer, ByVal ItemID As Long) Implements IViewTaskMultipleUpload.ReturnToFileManagement
        Me.PageUtility.RedirectToUrl(RootObject.ItemManagementFiles(ItemID))
    End Sub
    Public Sub ReturnToItemsList(ByVal IdCommmunity As Integer) Implements IViewTaskMultipleUpload.ReturnToItemsList
        Me.PageUtility.RedirectToUrl(RootObject.TaskDetailRead(IdCommmunity))
    End Sub
    Public Sub ReturnToFileManagementWithErrors(ByVal IdCommmunity As Integer, ByVal IdItem As Long, ByVal NUinternalFile As List(Of dtoModuleUploadedFile), ByVal NUcommunityFile As List(Of dtoUploadedFile)) Implements IViewTaskMultipleUpload.ReturnToFileManagementWithErrors
        If (IsNothing(NUcommunityFile) OrElse NUcommunityFile.Count = 0) AndAlso (IsNothing(NUinternalFile) OrElse NUinternalFile.Count = 0) Then
            Me.PageUtility.RedirectToUrl(RootObject.ItemManagementFiles(IdItem))
        Else
            Dim oRemotePost As New lm.Comol.Modules.Base.DomainModel.RemotePost
            oRemotePost.Url = Me.BaseUrl & RootObject.ItemError()
            oRemotePost.Add("BackUrl", RootObject.ItemManagementFiles(CurrentItemID))
            Dim index As Integer = 0
            If Not IsNothing(NUinternalFile) Then
                For Each dto As dtoModuleUploadedFile In NUinternalFile
                    oRemotePost.Add("FILE_I_Name_" & index.ToString, dto.File.DisplayName)
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
            oRemotePost.Add("ItemID", IdItem.ToString)

            oRemotePost.Post()
        End If
    End Sub

#Region "Notification / Action"
    Public Sub NoPermissionToAddFiles(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewTaskMultipleUpload.NoPermissionToAddFiles
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTasklist.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewTaskMultipleUpload.AddCommunityFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTasklist.ActionType.AddFiles, Me.PageUtility.CreateObjectsList(ModuleID, ModuleTasklist.ObjectType.Task, Me.CurrentItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Public Sub AddInternalFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewTaskMultipleUpload.AddInternalFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTasklist.ActionType.AddFiles, Me.PageUtility.CreateObjectsList(ModuleID, ModuleTasklist.ObjectType.Task, Me.CurrentItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    'Public Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IViewMultipleUpload.NotifyAddCommunityFile
    '    Dim oService As New LessonDiaryNotificationUtility(Me.PageUtility)
    '    oService.(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors)
    'End Sub
    'Public Sub NotifyAddInternalFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IViewMultipleUpload.NotifyAddInternalFile
    '    Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
    '    oService.NotifyItemAddInternalFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors)
    'End Sub
    'Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
    '    PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    'End Sub


    Public Sub NotifyAddFile(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal FileNumber As Integer, ByVal isvisible As Boolean) Implements IViewTaskMultipleUpload.NotifyAddFile
        Dim oService As New LessonDiaryNotificationUtility(Me.PageUtility)
        oService.NotifyAddFilesToItem(CommunityID, ItemID, StartDate, EndDate, FileNumber, isvisible)
    End Sub

    Public Sub NotifyAddFileNoDate(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal Title As String, ByVal FileNumber As Integer, ByVal isvisible As Boolean) Implements IViewTaskMultipleUpload.NotifyAddFileNoDate
        Dim oService As New LessonDiaryNotificationUtility(Me.PageUtility)
        oService.NotifyAddFilesToItem(CommunityID, ItemID, Title, FileNumber, isvisible)
    End Sub
    Public Sub SendInitAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long) Implements IViewTaskMultipleUpload.SendInitAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, ModuleTasklist.ActionType.InitUploadMultipleFiles, Me.PageUtility.CreateObjectsList(ModuleID, ModuleTasklist.ObjectType.Task, ItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    'Public Sub NotifyAddCommunityFile(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal StartDate As Date, ByVal EndDate As Date, ByVal FileID As Long, ByVal FileName As String, ByVal isScorm As Boolean, ByVal UniqueId As System.Guid, ByVal isVisible As Boolean) Implements IViewMultipleUpload.NotifyAddCommunityFile
    '    Dim oService As New LessonDiaryNotificationUtility(Me.PageUtility)
    '    Dim FileUrl As String = Me.BaseUrl
    '    If isScorm Then
    '        FileUrl &= Me.PageUtility.SystemSettings.Icodeon.GetScormLink(FileID, UniqueId)
    '    Else
    '        FileUrl = "File.repository?FileID=" & FileID.ToString & "&ModuleID=" & ModuleID.ToString & "&ItemID=" & ItemID.ToString
    '    End If
    '    oService.NotifyAddFileToItem(CommunityID, ItemID, StartDate, EndDate, FileName, FileUrl, isVisible)
    'End Sub

    'Public Sub NotifyAddCommunityFile(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal Title As String, ByVal FileID As Long, ByVal FileName As String, ByVal isScorm As Boolean, ByVal UniqueId As System.Guid, ByVal isVisible As Boolean) Implements IViewMultipleUpload.NotifyAddCommunityFile
    '    Dim oService As New LessonDiaryNotificationUtility(Me.PageUtility)
    '    Dim FileUrl As String = Me.BaseUrl
    '    If isScorm Then
    '        FileUrl &= Me.PageUtility.SystemSettings.Icodeon.GetScormLink(FileID, UniqueId)
    '    Else
    '        FileUrl = "File.repository?FileID=" & FileID.ToString & "&ModuleID=" & ModuleID.ToString & "&ItemID=" & ItemID.ToString
    '    End If
    '    oService.NotifyAddFileToItem(CommunityID, ItemID, Title, FileName, FileUrl, isVisible)
    'End Sub

    'Public Sub NotifyAddInternalFile(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal StartDate As Date, ByVal EndDate As Date, ByVal FileID As System.Guid, ByVal FileName As String, ByVal isScorm As Boolean, ByVal isVisible As Boolean) Implements IViewMultipleUpload.NotifyAddInternalFile
    '    Dim oService As New LessonDiaryNotificationUtility(Me.PageUtility)
    '    Dim FileUrl As String = Me.BaseUrl & "FileStore/" & FileID.ToString & "/" & FileName

    '    oService.NotifyAddFileToItem(CommunityID, ItemID, StartDate, EndDate, FileName, FileUrl, isVisible)
    'End Sub
    'Public Sub NotifyAddInternalFile(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal Title As String, ByVal FileID As System.Guid, ByVal FileName As String, ByVal isScorm As Boolean, ByVal isVisible As Boolean) Implements IViewMultipleUpload.NotifyAddInternalFile
    '    Dim oService As New LessonDiaryNotificationUtility(Me.PageUtility)
    '    Dim FileUrl As String = Me.BaseUrl & "FileStore/" & FileID.ToString & "/" & FileName
    '    oService.NotifyAddFileToItem(CommunityID, ItemID, Title, FileName, FileUrl, isVisible)
    'End Sub
#End Region
End Class