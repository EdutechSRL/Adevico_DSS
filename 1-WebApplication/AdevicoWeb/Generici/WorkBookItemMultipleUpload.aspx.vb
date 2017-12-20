Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel

Partial Public Class WorkBookItemMultipleUpload
    Inherits PageBase
    Implements IWorkbookMultipleFileUpload

#Region "Default view"
    Private _BaseUrl As String
    Private _PageUtility As OLDpageUtility
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As WorkBookItemMultipleUploadPresenter

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

    Public ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission) Implements IWorkbookMultipleFileUpload.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) _
                                          Select New WorkBookCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleWorkBook(New Services_WorkBook(sb.PermissionString))}).ToList
            End If
            Return _CommunitiesPermission
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
    Public ReadOnly Property CurrentPresenter() As WorkBookItemMultipleUploadPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WorkBookItemMultipleUploadPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter Implements IWorkbookMultipleFileUpload.PreviousWorkBookView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property
#End Region

#Region "interface"
    Public WriteOnly Property AllowUpload() As Boolean Implements IWorkbookMultipleFileUpload.AllowUpload
        Set(ByVal value As Boolean)
            Me.LNBupload.Enabled = value
        End Set
    End Property
    Public Sub SetUrlToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IWorkbookMultipleFileUpload.SetUrlToFileManagement
        Me.HYPbackToFileManagement.Visible = Not (ItemID = System.Guid.Empty)
        Me.HYPbackToFileManagement.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & oView.ToString
    End Sub
    Public Sub SetUrlToItem(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IWorkbookMultipleFileUpload.SetUrlToItem
        Me.HYPbackToItem.Visible = Not (ItemID = System.Guid.Empty)
        Me.HYPbackToItem.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItem.aspx?ItemID=" & ItemID.ToString & "&View=" & oView.ToString
    End Sub
    Public Sub SetUrlToWorkbook(ByVal WorkBookID As System.Guid, ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IWorkbookMultipleFileUpload.SetUrlToWorkbook
        Me.HYPbackToItems.Visible = Not (WorkBookID = System.Guid.Empty)
        Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView.ToString & "#" & ItemID.ToString
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub LNBupload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBupload.Click
        Me.CurrentPresenter.AddFiles(Me.CTRLCommunityFile.AddFilesToCommunityRepository, _
                                     Me.CTRLWorkBookUpload.AddToUserRepository(Me.PageUtility.BaseUserRepositoryPath))
    End Sub

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
        MyBase.SetCulture("pg_WorkBookItemMultipleUpload", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLiteral(Me.LTuploadToCommunity_t)
            .setLiteral(Me.LTuploadToWorkBook_t)
            .setLinkButton(Me.LNBupload, True, True)
            .setHyperLink(Me.HYPbackToFileManagement, True, True)
            .setHyperLink(Me.HYPbackToItem, True, True)
            .setHyperLink(Me.HYPbackToItems, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public WriteOnly Property AllowCommunityUpload() As Boolean Implements IWorkbookMultipleFileUpload.AllowCommunityUpload
        Set(ByVal value As Boolean)
            Me.DVcommunity.Visible = value
            Me.DVcommunity.Style.Add("display", IIf(value, "block", "none"))
        End Set
    End Property
    Public ReadOnly Property PreloadedItemID() As System.Guid Implements IWorkbookMultipleFileUpload.PreloadedItemID
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

    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As lm.Comol.Modules.Base.DomainModel.ModuleCommunityRepository Implements IWorkbookMultipleFileUpload.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                   Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
        If IsNothing(oModule) Then
            oModule = New ModuleCommunityRepository
        End If
        Return oModule
    End Function

    Public Sub NoPermissionToAddFiles(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWorkbookMultipleFileUpload.NoPermissionToAddFiles
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Sub InitializeCommunityUploader(ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository) Implements IWorkbookMultipleFileUpload.InitializeCommunityUploader
        Me.CTRLCommunityFile.InitializeControl(0, CommunityID, oPermission)
    End Sub
    Public Sub ReturnToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IWorkbookMultipleFileUpload.ReturnToFileManagement
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & oView.ToString)
    End Sub
    Public Sub ReturnToItemsList(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IWorkbookMultipleFileUpload.ReturnToItemsList
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView.ToString & "#" & Me.PreloadedItemID.ToString)
    End Sub
    Public Sub ReturnToFileManagement(ByVal ItemID As System.Guid, ByVal NUinternalFile As List(Of lm.Comol.Core.DomainModel.BaseFile), ByVal NUcommunityFile As List(Of dtoUploadedFile), ByVal oView As WorkBookTypeFilter) Implements IWorkbookMultipleFileUpload.ReturnToFileManagementWithErrors
        If (IsNothing(NUcommunityFile) OrElse NUcommunityFile.Count = 0) AndAlso (IsNothing(NUinternalFile) OrElse NUinternalFile.Count = 0) Then
            Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & Me.PreloadedItemID.ToString & "&View=" & oView.ToString)
        Else
            Dim oRemotePost As New RemotePost
            oRemotePost.Url = Me.BaseUrl & "Generici/WorkBookError.aspx?FromView=upload&ItemID=" & ItemID.ToString & "&View=" & oView.ToString

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
            oRemotePost.Add("FromView", "upload")
            oRemotePost.Add("ItemID", ItemID.ToString)

            oRemotePost.Post()
        End If
    End Sub

#Region "Notification / Action"
    Public Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWorkbookMultipleFileUpload.AddCommunityFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.UploadCommunityFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_WorkBook.ObjectType.WorkBookItem, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Public Sub AddInternalFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWorkbookMultipleFileUpload.AddInternalFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.UploadInternalFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_WorkBook.ObjectType.WorkBookItem, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Public Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IWorkbookMultipleFileUpload.NotifyAddCommunityFile
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemAddCommunityFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
    Public Sub NotifyAddInternalFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IWorkbookMultipleFileUpload.NotifyAddInternalFile
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemAddInternalFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
    'Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
    '    PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    'End Sub
#End Region




End Class