Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Domain

Partial Public Class CommunityRepositoryMultipleUpload
    Inherits PageBase
    Implements IViewCommunityFileMultipleUpload

#Region "View"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As CRmultipleUploadPresenter
    Public ReadOnly Property CurrentPresenter() As CRmultipleUploadPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRmultipleUploadPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
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
    Public WriteOnly Property AllowBackToDownloads(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IViewCommunityFileMultipleUpload.AllowBackToDownloads
        Set(ByVal value As Boolean)
            Me.HYPbackToDownloads.Visible = value
            If value Then
                Me.HYPbackToDownloads.NavigateUrl = Me.BaseUrl & RootObject.RepositoryCurrentList(FolderID, CommunityID, oView.ToString, 0, PreLoadedContentView)
                'Me.HYPbackToDownloads.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property
    Public WriteOnly Property AllowBackToManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IViewCommunityFileMultipleUpload.AllowBackToManagement
        Set(ByVal value As Boolean)
            Me.HYPbackToManagement.Visible = value
            If value Then
                Me.HYPbackToManagement.NavigateUrl = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, oView.ToString, PreLoadedContentView)
                ' Me.HYPbackToManagement.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property

    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IViewCommunityFileMultipleUpload.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
            If IsNothing(oModule) Then
                oModule = New ModuleCommunityRepository
            End If
        End If

        Return oModule
    End Function

    Public ReadOnly Property PreloadedCommunityID() As Integer Implements IViewCommunityFileMultipleUpload.PreloadedCommunityID
        Get
            If Not IsNumeric(Request.QueryString("CommunityID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("CommunityID"))
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedFolderID() As Long Implements IViewCommunityFileMultipleUpload.PreloadedFolderID
        Get
            If Not IsNumeric(Request.QueryString("FolderID")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("FolderID"))
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository Implements IViewCommunityFileMultipleUpload.PreloadedPreviousView
        Get
            If IsNothing(Request.QueryString("PreviousView")) Then
                Return IViewExploreCommunityRepository.ViewRepository.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("PreviousView"), IViewExploreCommunityRepository.ViewRepository.None)
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedPage() As RepositoryPage Implements IViewCommunityFileMultipleUpload.PreLoadedPage
        Get
            If IsNothing(Request.QueryString("PreviousPage")) Then
                Return RepositoryPage.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RepositoryPage).GetByString(Request.QueryString("PreviousPage"), RepositoryPage.DownLoadPage)
            End If
        End Get
    End Property
    Public Property RepositoryCommunityID() As Integer Implements IViewCommunityFileMultipleUpload.RepositoryCommunityID
        Get
            Return CInt(Me.ViewState("RepositoryCommunityID"))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
#End Region

#Region "Inherits"
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
#End Region

  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


#Region "Default"
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

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityRepositoryUpload", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceMultipleUploadTitleFile")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(HYPbackToDownloads, True, True)
            .setHyperLink(HYPbackToManagement, True, True)
            .setButton(Me.BTNupload, True, , , True)
            .setButton(Me.BTNuploadAndPermission, True, , , True)
            .setLabel(LBnoPermissionToUpload)
        End With
    End Sub

    Public Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewCommunityFileMultipleUpload.NoPermission
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.BindNoPermessi()
    End Sub

    Public Sub NoPermissionToAdd(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewCommunityFileMultipleUpload.NoPermissionToAdd
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.MLVupload.SetActiveView(Me.VIWpermissionToUpload)
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub InitializeUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository) Implements IViewCommunityFileMultipleUpload.InitializeUploader
        Me.MLVupload.SetActiveView(Me.VIWupload)
        Me.BTNupload.Visible = True
        Me.BTNuploadAndPermission.Visible = False
        Me.CTRLuploader.InitializeControl(FolderID, CommunityID, oPermission)
        Me.Master.ServiceTitle = Me.Resource.getValue("serviceMultipleUploadTitleFile")
    End Sub


    Private Sub BTNupload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNupload.Click
        Dim cacheKey As String = "CommunityRepositorySize_" & Me.RepositoryCommunityID
        GenericCacheManager.PurgeCacheItems(cacheKey)

        Dim iResponse As MultipleUploadResult(Of dtoUploadedFile) = Me.CTRLuploader.AddFilesToCommunityRepository()
        If iResponse.NotuploadedFile.Count > 0 Then
            Me.GoToErrorPage(iResponse.NotuploadedFile)
        Else
            Me.CurrentPresenter.GotoManagementDownloadPage(iResponse.UploadedFile(0).File)
        End If
    End Sub

    Private Sub GoToErrorPage(ByVal ErrorsFile As List(Of dtoUploadedFile))
        Dim oRemotePost As New RemotePost
        oRemotePost.Url = Me.BaseUrl & RootObject.RepositoryError(RepositoryPage.MultipleUpload.ToString, Me.PreloadedPreviousView.ToString, Me.PreLoadedPage.ToString, PreLoadedContentView)
        '"Modules/Repository/CommunityRepositoryItemError.aspx?PageSender=" & RepositoryPage.MultipleUpload.ToString & "&PreviousView=" & Me.PreloadedPreviousView.ToString & "&PreviousPage=" & Me.PreLoadedPage.ToString
        Dim index As Integer = 0
        If Not IsNothing(ErrorsFile) Then
            index = 0
            For Each oDto As dtoUploadedFile In ErrorsFile
                oRemotePost.Add("FILE_C_Name_" & index.ToString, oDto.File.DisplayName)
                oRemotePost.Add("FILE_C_isFile_" & index.ToString, oDto.File.isFile)
                oRemotePost.Add("FILE_C_Status_" & index.ToString, oDto.Status.ToString)
                oRemotePost.Add("FILE_C_FolderId_" & index.ToString, oDto.File.FolderId)
                oRemotePost.Add("FILE_C_SavedFilePath_" & index.ToString, oDto.SavedFilePath)
                oRemotePost.Add("FILE_C_SavedName_" & index.ToString, oDto.File.Name)
                oRemotePost.Add("FILE_C_SavedExtension_" & index.ToString, oDto.File.Extension)
                index += 1
            Next
            oRemotePost.Add("FolderId", Me.CTRLuploader.FolderID)
            oRemotePost.Add("CommunityID", Me.CTRLuploader.RepositoryCommunityID)
            oRemotePost.Add("PreloadedCreate", ItemRepositoryToCreate.File)
        End If
        oRemotePost.Post()
    End Sub

    Public Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage) Implements IViewCommunityFileMultipleUpload.LoadRepositoryPage
        Select Case GotoPage
            Case RepositoryPage.CommunityDiaryPage

            Case RepositoryPage.DownLoadPage
                Me.RedirectToUrl(RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView))
                ' Me.RedirectToUrl("Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
            Case RepositoryPage.ManagementPage
                Me.RedirectToUrl(RootObject.RepositoryManagement(FolderID, CommunityID, View.ToString, PreLoadedContentView))
                'Me.RedirectToUrl("Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
            Case RepositoryPage.None
                Me.RedirectToUrl(RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView))
                '"Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)

        End Select
    End Sub


#Region "Action"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_File.Codex)
    End Sub
    Public Sub ActionInitialize(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewCommunityFileMultipleUpload.ActionInitialize
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.StartOtherUpload, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub
    Public Sub ActionUpload(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal FileID As Long) Implements IViewCommunityFileMultipleUpload.ActionUpload
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.UploadFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_File.ObjectType.File, FileID.ToString), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
#End Region
    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class