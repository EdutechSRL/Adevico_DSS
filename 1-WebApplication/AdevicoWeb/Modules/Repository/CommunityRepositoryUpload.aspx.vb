Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Domain

Partial Public Class CommunityRepositoryUpload
    Inherits PageBase
    Implements IViewCommunityFileUpload


#Region "View"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As CRuploadPresenter
    Public ReadOnly Property CurrentPresenter() As CRuploadPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRuploadPresenter(Me.CurrentContext, Me)
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
    Public WriteOnly Property AllowBackToDownloads(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IViewCommunityFileUpload.AllowBackToDownloads
        Set(ByVal value As Boolean)
            Me.HYPbackToDownloads.Visible = value
            If value Then
                Me.HYPbackToDownloads.NavigateUrl = Me.BaseUrl & RootObject.RepositoryCurrentList(FolderID, CommunityID, oView.ToString, 0, PreLoadedContentView)
                '"Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property
    Public WriteOnly Property AllowBackToManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IViewCommunityFileUpload.AllowBackToManagement
        Set(ByVal value As Boolean)
            Me.HYPbackToManagement.Visible = value
            If value Then
                Me.HYPbackToManagement.NavigateUrl = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, oView.ToString, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property

    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IViewCommunityFileUpload.CommunityRepositoryPermission
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

    Public ReadOnly Property PreloadedCommunityID() As Integer Implements IViewCommunityFileUpload.PreloadedCommunityID
        Get
            If Not IsNumeric(Request.QueryString("CommunityID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("CommunityID"))
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedCreate() As ItemRepositoryToCreate Implements IViewCommunityFileUpload.PreloadedCreate
        Get
            If IsNothing(Request.QueryString("Create")) Then
                Return ItemRepositoryToCreate.File
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemRepositoryToCreate).GetByString(Request.QueryString("Create"), ItemRepositoryToCreate.File)
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedFolderID() As Long Implements IViewCommunityFileUpload.PreloadedFolderID
        Get
            If Not IsNumeric(Request.QueryString("FolderID")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("FolderID"))
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository Implements IViewCommunityFileUpload.PreloadedPreviousView
        Get
            If IsNothing(Request.QueryString("PreviousView")) Then
                Return IViewExploreCommunityRepository.ViewRepository.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("PreviousView"), IViewExploreCommunityRepository.ViewRepository.None)
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedPage() As RepositoryPage Implements IViewCommunityFileUpload.PreLoadedPage
        Get
            If IsNothing(Request.QueryString("PreviousPage")) Then
                Return RepositoryPage.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RepositoryPage).GetByString(Request.QueryString("PreviousPage"), RepositoryPage.DownLoadPage)
            End If
        End Get
    End Property
    Public Property RepositoryCommunityID() As Integer Implements IViewCommunityFileUpload.RepositoryCommunityID
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
            Me.Master.ServiceTitle = .getValue("serviceSingleUploadTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(HYPbackToDownloads, True, True)
            .setHyperLink(HYPbackToManagement, True, True)
            .setButton(Me.BTNupload, True, , , True)
         
            ' .setButton(Me.BTNuploadAndPermission, True, , , True)
            .setButton(BTNcreate, True, , , True)
            '.setButton(BTNcreateAndPermission, True, , , True)
            .setLabel(LBnoPermissionToUpload)

            BTNuploadBottom.Text = BTNupload.Text
            BTNuploadBottom.ToolTip = BTNupload.ToolTip
            BTNcreateBottom.Text = BTNcreate.Text
            BTNcreateBottom.ToolTip = BTNcreate.ToolTip
        End With
    End Sub

   

    Public Sub NoPermission(ByVal CommunityID As Integer) Implements IViewCommunityFileUpload.NoPermission
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.BindNoPermessi()
    End Sub

    Public Sub NoPermissionToAdd(ByVal CommunityID As Integer) Implements IViewCommunityFileUpload.NoPermissionToAdd
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.MLVupload.SetActiveView(Me.VIWpermissionToUpload)
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub InitializeUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ForCreate As ItemRepositoryToCreate, ByVal oPermission As ModuleCommunityRepository) Implements IViewCommunityFileUpload.InitializeUploader
        Me.MLVupload.SetActiveView(Me.VIWupload)
        Me.BTNcreate.Visible = (ForCreate = ItemRepositoryToCreate.Folder)
        Me.BTNupload.Visible = (ForCreate = ItemRepositoryToCreate.File)
        Me.BTNcreateBottom.Visible = (ForCreate = ItemRepositoryToCreate.Folder)
        Me.BTNuploadBottom.Visible = (ForCreate = ItemRepositoryToCreate.File)
        '     Me.BTNcreateAndPermission.Visible = False
        '   Me.BTNuploadAndPermission.Visible = False
        Me.CTRLuploader.InitializeControl(FolderID, CommunityID, ForCreate, oPermission)

        If ForCreate = ItemRepositoryToCreate.File Then
            Me.Master.ServiceTitle = Me.Resource.getValue("serviceSingleUploadTitleFile")
        Else
            Me.Master.ServiceTitle = Me.Resource.getValue("serviceSingleUploadTitleFolder")
        End If
        'If ForCreate = ItemRepositoryToCreate.File Then
        '    Me.BTNupload.Attributes.Add("onclick", "getRadProgressManager().startProgressPolling();return true;")
        '    Me.BTNuploadAndPermission.Attributes.Add("onclick", "getRadProgressManager().startProgressPolling();return true;")
        'End If
    End Sub

    'Private Sub CTRLuploader_changeItemPermission(ByVal forAll As Boolean, ByVal forCreate As ItemRepositoryToCreate) Handles CTRLuploader.changeItemPermission
    '    Me.BTNcreate.Visible = (forAll AndAlso forCreate = ItemRepositoryToCreate.Folder)
    '    Me.BTNupload.Visible = (forAll AndAlso forCreate = ItemRepositoryToCreate.File)
    '    Me.BTNcreateAndPermission.Visible = (Not forAll AndAlso forCreate = ItemRepositoryToCreate.Folder)
    '    Me.BTNuploadAndPermission.Visible = (Not forAll AndAlso forCreate = ItemRepositoryToCreate.File)
    'End Sub

    Private Sub BTNcreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcreate.Click, BTNcreateBottom.Click
        Dim iResponse As MultipleUploadResult(Of dtoUploadedFile) = Me.CTRLuploader.AddFolderToCommunityRepository()

        If iResponse.NotuploadedFile.Count > 0 Then
            Me.GoToErrorPage(iResponse.NotuploadedFile)
        ElseIf Me.CTRLuploader.DownlodableByCommunity AndAlso iResponse.UploadedFile.Count > 0 Then
            Me.CurrentPresenter.GotoManagementDownloadPage(iResponse.UploadedFile(0).File)
        Else If iResponse.UploadedFile.Count > 0 Then
            Me.CurrentPresenter.GotoPermissionManagement(iResponse.UploadedFile(0).File)
        End If
    End Sub
    Private Sub BTNupload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNupload.Click, BTNuploadBottom.Click
        Dim cacheKey As String = "CommunityRepositorySize_" & Me.RepositoryCommunityID
        GenericCacheManager.PurgeCacheItems(cacheKey)

        Dim iResponse As MultipleUploadResult(Of dtoUploadedFile) = Me.CTRLuploader.AddFileToCommunityRepository()
        If iResponse.NotuploadedFile.Count > 0 Then
            Me.GoToErrorPage(iResponse.NotuploadedFile)
        ElseIf Me.CTRLuploader.DownlodableByCommunity AndAlso iResponse.UploadedFile.Count > 0 Then
            Me.CurrentPresenter.GotoManagementDownloadPage(iResponse.UploadedFile(0).File)
        ElseIf iResponse.UploadedFile.Count > 0 Then
            Me.CurrentPresenter.GotoPermissionManagement(iResponse.UploadedFile(0).File)
        End If
    End Sub
    'Private Sub BTNuploadAndPermission_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNuploadAndPermission.Click, BTNcreateAndPermission.Click
    '    Dim iResponse As MultipleUploadResult(Of dtoUploadedFile)

    '    If sender Is BTNcreateAndPermission Then
    '        iResponse = Me.CTRLuploader.AddFolderToCommunityRepository()
    '    Else
    '        iResponse = Me.CTRLuploader.AddFileToCommunityRepository()
    '    End If

    '    If iResponse.NotuploadedFile.Count > 0 Then
    '        Me.GoToErrorPage(iResponse.NotuploadedFile)
    '    Else
    '        Me.CurrentPresenter.GotoPermissionManagement(iResponse.UploadedFile(0).File)
    '    End If
    'End Sub

    Private Sub GoToErrorPage(ByVal ErrorsFile As List(Of dtoUploadedFile))
        Dim oRemotePost As New RemotePost
        oRemotePost.Url = Me.BaseUrl & RootObject.RepositoryError(RepositoryPage.SingleUploadPage.ToString, Me.PreloadedPreviousView.ToString, Me.PreLoadedPage.ToString, PreLoadedContentView)
        '"Modules/Repository/CommunityRepositoryItemError.aspx?PageSender=" & RepositoryPage.SingleUploadPage.ToString & "&PreviousView=" & Me.PreloadedPreviousView.ToString & "&PreviousPage=" & Me.PreLoadedPage.ToString
        Dim index As Integer = 0
        If Not IsNothing(ErrorsFile) Then
            index = 0
            For Each oDto As dtoUploadedFile In ErrorsFile
                oRemotePost.Add("FILE_C_UniqueID_" & index.ToString, oDto.File.UniqueID.ToString)
                oRemotePost.Add("FILE_C_Name_" & index.ToString, oDto.File.DisplayName)
                oRemotePost.Add("FILE_C_isFile_" & index.ToString, oDto.File.isFile)
                oRemotePost.Add("FILE_C_Status_" & index.ToString, oDto.Status.ToString)
                oRemotePost.Add("FILE_C_FolderId_" & index.ToString, oDto.File.FolderId)
                oRemotePost.Add("FILE_C_SavedFilePath_" & index.ToString, oDto.SavedFilePath)
                oRemotePost.Add("FILE_C_SavedName_" & index.ToString, oDto.File.Name)
                oRemotePost.Add("FILE_C_SavedExtension_" & index.ToString, oDto.File.Extension)
                index += 1
            Next
            oRemotePost.Add("FolderId", ErrorsFile(0).File.FolderId)
            If ErrorsFile(0).File.CommunityOwner Is Nothing Then
                oRemotePost.Add("CommunityID", 0)
            Else
                oRemotePost.Add("CommunityID", ErrorsFile(0).File.CommunityOwner.Id.ToString)
            End If
            oRemotePost.Add("PreloadedCreate", Me.PreloadedCreate.ToString)
        End If
        oRemotePost.Post()
    End Sub

    Public Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage) Implements IViewCommunityFileUpload.LoadRepositoryPage
        Select Case GotoPage
            Case RepositoryPage.CommunityDiaryPage

            Case RepositoryPage.DownLoadPage
                ' Me.RedirectToUrl("Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
                Me.RedirectToUrl(RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView))
            Case RepositoryPage.ManagementPage
                Me.RedirectToUrl(RootObject.RepositoryManagement(FolderID, CommunityID, View.ToString, PreLoadedContentView))
                'Me.RedirectToUrl("Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
            Case RepositoryPage.None
                Me.RedirectToUrl(RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView))
                'Me.RedirectToUrl("Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)

        End Select
    End Sub

    Public Sub LoadEditingPermission(ByVal ItemID As Long, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As lm.Comol.Modules.Base.Presentation.IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage) Implements IViewCommunityFileUpload.LoadEditingPermission
        Me.RedirectToUrl("Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" & ItemID.ToString & "&FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString & "&PreviousPage=" & GotoPage.ToString & "&Action=" & PermissionAction.SingleUpload)
    End Sub

#Region "Action"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_File.Codex)
    End Sub
    Public Sub ActionInitialize(ByVal CommunityID As Integer) Implements IViewCommunityFileUpload.ActionInitialize
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.StartUpload, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub
    Public Sub ActionCreateFolder(ByVal CommunityID As Integer, ByVal FolderID As Long) Implements IViewCommunityFileUpload.ActionCreateFolder
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.UploadFile, Me.PageUtility.CreateObjectsList(Services_File.ObjectType.File, FolderID.ToString), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

    Public Sub ActionUpload(ByVal CommunityID As Integer, ByVal FileID As Long) Implements IViewCommunityFileUpload.ActionUpload
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.UploadFile, Me.PageUtility.CreateObjectsList(Services_File.ObjectType.File, FileID.ToString), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
#End Region

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class