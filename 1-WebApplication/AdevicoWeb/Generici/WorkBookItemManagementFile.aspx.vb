Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel

Partial Public Class WorkBookItemManagementFile
    Inherits PageBase
    Implements IWorkBookItemManagementFile

#Region "Default view"
    Private _BaseUrl As String
    Private _PageUtility As OLDpageUtility
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As lm.Comol.Modules.Base.Presentation.WorkBookItemManagementFilePresenter
    Private _CommunityRepositoryPermission As List(Of ModuleCommunityRepository)
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

    Public ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission) Implements IWorkBookItemManagementFile.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) _
                                          Select New WorkBookCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleWorkBook(New Services_WorkBook(sb.PermissionString))}).ToList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
    Public ReadOnly Property AllowPublish() As Boolean Implements IWorkBookItemManagementFile.AllowPublish
        Get
            If IsNothing(_CommunityRepositoryPermission) OrElse _CommunityRepositoryPermission.Count = 0 Then
                _CommunityRepositoryPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).ToList
            End If
            Return (From sb In _CommunityRepositoryPermission Where sb.Administration OrElse sb.UploadFile).Any
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
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.WorkBookItemManagementFilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.Base.Presentation.WorkBookItemManagementFilePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Interface"

    Public WriteOnly Property AllowUpload() As Boolean Implements lm.Comol.Modules.Base.Presentation.IWorkBookItemManagementFile.AllowUpload
        Set(ByVal value As Boolean)
            Me.HYPmultipleUpload.Visible = value
        End Set
    End Property

    Public WriteOnly Property BackToWorkbook() As System.Guid Implements lm.Comol.Modules.Base.Presentation.IWorkBookItemManagementFile.BackToWorkbook
        Set(ByVal value As System.Guid)
            Me.HYPbackToItems.Visible = Not (value = System.Guid.Empty)
            Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemsList.aspx?WorkBookID=" & value.ToString & "&View=" & Me.PreviousWorkBookView.ToString & "#" & Me.PreloadedItemID.ToString
        End Set
    End Property
    Public ReadOnly Property PreloadedItemID() As System.Guid Implements lm.Comol.Modules.Base.Presentation.IWorkBookItemManagementFile.PreloadedItemID
        Get
            Dim UrlID As String = Request.QueryString("ItemID")
            If Not UrlID = "" Then
                Try
                    Return New System.Guid(UrlID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IWorkBookItemManagementFile.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                   Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
        If IsNothing(oModule) Then
            oModule = New ModuleCommunityRepository
        End If
        Return oModule
    End Function
    Public ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter Implements IWorkBookItemManagementFile.PreviousWorkBookView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property
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
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("WorkBookItemManagementFile", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLiteral(Me.LTuploadToCommunity_t)
            .setLiteral(Me.LTuploadToWorkBook_t)
            .setLiteral(Me.LTitemFiles_t)
            .setLiteral(Me.LTaddFromCommunity_t)
            .setHyperLink(Me.HYPmultipleUpload, True, True)
            .setHyperLink(Me.HYPbackToItem, True, True)
            .setHyperLink(Me.HYPbackToItems, True, True)
            .setLiteral(Me.LTlinkToCommunity)
            .setButton(Me.BTNlinkCommunityFile, True, , , True)
            .setButton(Me.BTNaddCommunityFile, True, , , True)
            .setButton(Me.BTNaddToWorkbook, True, , , True)
            '.setButton(Me.BTNaddCommunityFileWithPermission, True, , , True)
            Me.BTNaddCommunityFile.OnClientClick = "return HideWorkBookUpload();"
            Me.BTNaddToWorkbook.OnClientClick = "return HideCommunityUpload();"
            'Me.BTNaddCommunityFileWithPermission.OnClientClick = "return HideWorkBookUpload();"
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region


    Public Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer) Implements IWorkBookItemManagementFile.NoPermissionToManagementFiles
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Sub ReturnToItemsList(ByVal WorkBookID As System.Guid) Implements IWorkBookItemManagementFile.ReturnToItemsList
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & Me.PreviousWorkBookView.ToString & "#" & Me.PreloadedItemID.ToString)
    End Sub


#Region "VERIFICARE"
    Public Sub InitializeCommunityUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository) Implements IWorkBookItemManagementFile.InitializeCommunityUploader
        Me.CTRLCommunityFile.InitializeControl(FolderID, CommunityID, ItemRepositoryToCreate.File, oPermission)
    End Sub
    Public Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWorkBookItemManagementFile.AddCommunityFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.UploadCommunityFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_WorkBook.ObjectType.WorkBookItem, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub AddInternalFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWorkBookItemManagementFile.AddInternalFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.UploadInternalFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_WorkBook.ObjectType.WorkBookItem, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IWorkBookItemManagementFile.NotifyAddCommunityFile
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemAddCommunityFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub

    Public Sub NotifyAddInternalFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IWorkBookItemManagementFile.NotifyAddInternalFile
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemAddInternalFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
#End Region

    Public WriteOnly Property AllowCommunityUpload() As Boolean Implements IWorkBookItemManagementFile.AllowCommunityUpload
        Set(ByVal value As Boolean)
            Me.DVcommunity.Visible = value
        End Set
    End Property

    Public WriteOnly Property AllowCommunityLink() As Boolean Implements IWorkBookItemManagementFile.AllowCommunityLink
        Set(ByVal value As Boolean)
            Me.DVcommunityLink.Visible = value
        End Set
    End Property

    Public Sub ReturnToFileManagementWithErrors(ByVal NUinternalFile As List(Of lm.Comol.Core.DomainModel.BaseFile), ByVal NUcommunityFile As System.Collections.Generic.List(Of dtoUploadedFile), ByVal oView As WorkBookTypeFilter) Implements IWorkBookItemManagementFile.ReturnToFileManagementWithErrors
        If (IsNothing(NUcommunityFile) OrElse NUcommunityFile.Count = 0) AndAlso (IsNothing(NUinternalFile) OrElse NUinternalFile.Count = 0) Then
            Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & Me.PreloadedItemID.ToString & "&View=" & oView.ToString)
        Else
            Dim oRemotePost As New RemotePost
            oRemotePost.Url = Me.BaseUrl & "Generici/WorkBookError.aspx?FromView=management&ItemID=" & Me.PreloadedItemID.ToString & "&View=" & oView.ToString

            Dim index As Integer = 0
            If Not IsNothing(NUinternalFile) Then
                For Each oFile As lm.Comol.Core.DomainModel.BaseFile In NUinternalFile
                    oRemotePost.Add("FILE_I_" & index.ToString, oFile.DisplayName)
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
            oRemotePost.Add("ItemID", Me.PreloadedItemID.ToString)
            oRemotePost.Post()
        End If
    End Sub

    Private Sub BTNlinkCommunityFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlinkCommunityFile.Click
        Me.PageUtility.RedirectToUrl("Generici/WorkbookItemCommunityFile.aspx?ItemID=" & Me.PreloadedItemID.ToString & "&View=" & Me.PreviousWorkBookView.ToString)
    End Sub

    Private Sub BTNaddCommunityFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddCommunityFile.Click
        Me.CurrentPresenter.AddCommunityFile(Me.CTRLCommunityFile.AddFileToCommunityRepository, Me.CTRLCommunityFile.DownlodableByCommunity)
    End Sub

    Private Sub BTNaddToWorkbook_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddToWorkbook.Click
        Me.CurrentPresenter.AddInternalFile(Me.CTRLWorkBookUpload.AddToUserRepository(Me.PageUtility.BaseUserRepositoryPath))
    End Sub

    Public Sub ReturnToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements lm.Comol.Modules.Base.Presentation.IWorkBookItemManagementFile.ReturnToFileManagement
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & oView.ToString)
    End Sub

    Public Sub LoadFilesToManage(ByVal ItemID As System.Guid, ByVal CommunityID As Integer, ByVal AllowManagement As Boolean, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository, ByVal AllowPublish As Boolean) Implements IWorkBookItemManagementFile.LoadFilesToManage
        Me.CTRLmanagementFile.Visible = True
        Me.CTRLmanagementFile.InitializeControl(ItemID, CommunityID, True, AllowManagement, False, oPermission, oModule, AllowPublish)
    End Sub

    Private Sub CTRLmanagementFile_UpdateToParent() Handles CTRLmanagementFile.UpdateToParent
        Me.ReturnToFileManagement(Me.PreloadedItemID, Me.PreviousWorkBookView)
    End Sub
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub

    Public Sub SetBackToItemUrl(ByVal WorkBookID As System.Guid, ByVal ItemID As System.Guid) Implements IWorkBookItemManagementFile.SetBackToItemUrl
        Me.HYPbackToItem.Visible = Not (ItemID = System.Guid.Empty)
        Me.HYPbackToItem.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItem.aspx?WorkBookID=" & WorkBookID.ToString & "&ItemID=" & ItemID.ToString & "&View=" & Me.PreviousWorkBookView.ToString
    End Sub

    Public Sub SetMultipleUploadUrl(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IWorkBookItemManagementFile.SetMultipleUploadUrl
        Me.HYPmultipleUpload.Visible = Not (ItemID = System.Guid.Empty)
        Me.HYPmultipleUpload.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemMultipleUpload.aspx?ItemID=" & ItemID.ToString & "&View=" & oView.ToString
    End Sub

    Private Sub CTRLCommunityFile_changeItemPermission(ByVal forAll As Boolean, ByVal ForCreate As ItemRepositoryToCreate) Handles CTRLCommunityFile.changeItemPermission
        Me.BTNaddCommunityFile.Visible = (forAll AndAlso ForCreate = ItemRepositoryToCreate.File)
        'Me.BTNaddCommunityFileWithPermission.Visible = (Not forAll AndAlso ForCreate = ItemRepositoryToCreate.File)
    End Sub

    'Private Sub BTNaddCommunityFileWithPermission_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddCommunityFileWithPermission.Click
    '    Me.CurrentPresenter.AddCommunityFile(Me.CTRLCommunityFile.AddFileToCommunityRepository, False)
    'End Sub

    Public Sub LoadEditingPermission(ByVal ItemID As Long, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal GotoPage As RepositoryPage, ByVal WorkBookItemID As Guid) Implements IWorkBookItemManagementFile.LoadEditingPermission
        Me.RedirectToUrl("Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" & ItemID.ToString & "&FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&PreviousPage=" & GotoPage.ToString & "&Action=" & PermissionAction.SingleUpload & "&RemoteID=" & WorkBookItemID.ToString & "&PreserveUrl=True&ForService=true")
    End Sub
End Class