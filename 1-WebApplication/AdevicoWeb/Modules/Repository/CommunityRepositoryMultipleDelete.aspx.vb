Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Core.BaseModules.Repository.Domain

Partial Public Class CommunityRepositoryMultipleDelete
    Inherits PageBase
    Implements IviewMultipleDelete


#Region "View"
    Private _Presenter As CRmultipleDeletePresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IviewMultipleDelete.CommunityRepositoryPermission
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
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As CRmultipleDeletePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRmultipleDeletePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public WriteOnly Property AllowMultipleDelete() As Boolean Implements IviewMultipleDelete.AllowMultipleDelete
        Set(ByVal value As Boolean)
            Me.LNBmultipleDelete.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IviewMultipleDelete.AllowManagement
        Set(ByVal value As Boolean)
            Me.HYPbackToManagement.Visible = value
            If value Then
                Me.HYPbackToManagement.NavigateUrl = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, oView.ToString, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property

    Public ReadOnly Property PreLoadedPageIndex() As Integer Implements IviewMultipleDelete.PreLoadedPageIndex
        Get
            If IsNumeric(Request.QueryString("Page")) Then
                Return CInt(Request.QueryString("Page"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedFolder() As Long Implements IviewMultipleDelete.PreLoadedFolder
        Get
            If Not IsNumeric(Request.QueryString("FolderID")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("FolderID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedCommunityID() As Integer Implements IviewMultipleDelete.PreLoadedCommunityID
        Get
            If Not IsNumeric(Request.QueryString("CommunityID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("CommunityID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedView() As IViewExploreCommunityRepository.ViewRepository Implements IviewMultipleDelete.PreLoadedView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return IViewExploreCommunityRepository.ViewRepository.FileList
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("View"), IViewExploreCommunityRepository.ViewRepository.FileList)
            End If
        End Get
    End Property
    'Public ReadOnly Property PreLoadedContentView As ContentView Implements IviewMultipleDelete.PreLoadedContentView
    '    Get
    '        If IsNumeric(Request.QueryString("CV")) Then
    '            Try
    '                Return Request.QueryString("CV")
    '            Catch ex As Exception
    '                Return ContentView.viewAll
    '            End Try
    '        Else
    '            Return ContentView.viewAll
    '        End If
    '    End Get
    'End Property
    Public Property RepositoryCommunityID() As Integer Implements IviewMultipleDelete.RepositoryCommunityID
        Get
            If IsNumeric(Me.ViewState("RepositoryCommunityID")) Then
                Return CInt(Me.ViewState("RepositoryCommunityID"))
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Private ReadOnly Property Portalname() As String Implements IviewMultipleDelete.Portalname
        Get
            Return Me.Resource.getValue("Portalname")
        End Get
    End Property
    Public WriteOnly Property TitleCommunity() As String Implements IviewMultipleDelete.TitleCommunity
        Set(ByVal value As String)
            Dim CommunityName As String = value
            If Len(CommunityName) > 62 Then
                CommunityName = Left(value, 32) & " ... " & Right(value, 20)
            End If
            Me.Master.ServiceTitle = String.Format(Me.Resource.getValue("serviceMultipleDeleteTitleCommunityName"), CommunityName)
            Me.Master.ServiceTitleToolTip = String.Format(Me.Resource.getValue("serviceMultipleDeleteTitleCommunityName"), value)
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


#Region ""
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
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
        MyBase.SetCulture("pg_CommunityRepository", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceMultipleDeleteTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(Me.HYPbackToManagement, True, True)
            .setLabel(Me.LBnoPermissionToDelete)
            .setLinkButton(Me.LNBmultipleDelete, True, True, , True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
    Public Sub NoPermission(ByVal CommunityID As Integer) Implements IviewMultipleDelete.NoPermission
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.BindNoPermessi()
    End Sub
#End Region

#Region "Action"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_File.Codex)
    End Sub
#End Region



    Public Sub InitializeFileSelector(ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IviewMultipleDelete.InitializeFileSelector
        Me.MLVmultiple.SetActiveView(Me.VIWfileSelector)
        Me.CTRLCommunityFile.InitializeControl(CommunityID, New List(Of Long), ShowHiddenItems, AdminPurpose, False, Repository.RepositoryItemType.None)
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.ViewDeleteMultiplePage, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub



    Public Sub NoPermissionToDelete(ByVal CommunityID As Integer) Implements IviewMultipleDelete.NoPermissionToDelete
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.MLVmultiple.SetActiveView(Me.VIWpermissionToDelete)
    End Sub

    Private Sub LNBdelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmultipleDelete.Click
        Dim CommunityPath As String = ""
        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
            CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\" & Me.RepositoryCommunityID
        Else
            CommunityPath = Me.SystemSettings.File.Materiale.DrivePath & "\" & Me.RepositoryCommunityID
        End If
        CommunityPath = Replace(CommunityPath, "\\", "\")
        If CommunityPath <> "" Then
            Me.CurrentPresenter.DeleteSelectedItems(Me.CTRLCommunityFile.GetSelectedItems(), CommunityPath)
        End If
    End Sub

    Public Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage) Implements IviewMultipleDelete.LoadRepositoryPage
        Dim cacheKey As String = "CommunityRepositorySize_" & CommunityID
        GenericCacheManager.PurgeCacheItems(cacheKey)
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


    Public Sub NotifyDeletedItems(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal oList As List(Of dtoDeletedItem)) Implements IviewMultipleDelete.NotifyDeletedItems
        Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
        Dim BaseFolder As String = Me.Resource.getValue("BaseFolder")
        Dim objects As New List(Of PresentationLayer.WS_Actions.ObjectAction)
        For Each oItem As dtoDeletedItem In oList
            If oItem.FolderName = "" Then oItem.FolderName = BaseFolder

            If oItem.isFile Then
                oSender.NotifyFileDeleted(CommunityID, oItem.FolderID, oItem.ID, oItem.Name, oItem.FolderName, oItem.Type)
            Else
                oSender.NotifyFolderDeleted(CommunityID, oItem.FolderID, oItem.ID, oItem.Name, oItem.FolderName)
            End If
            objects.Add(Me.PageUtility.CreateObjectAction(ModuleID, oItem.Type, oItem.ID))
        Next
        Me.PageUtility.AddAction(CommunityID, Services_File.ActionType.DeleteMultiple, objects, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class