Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel

Partial Public Class WorkbookItemCommunityFile
    Inherits PageBase
    Implements IWKSelectCommunityFile

#Region "Default Inherited"
    Private _BaseUrl As String
    Private _PageUtility As OLDpageUtility
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As lm.Comol.Modules.Base.Presentation.WorkBookItemSelectCommunityFile

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

    Public ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission) Implements lm.Comol.Modules.Base.Presentation.IWKSelectCommunityFile.CommunitiesPermission
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
    Private ReadOnly Property CurrentPresenter() As WorkBookItemSelectCommunityFile
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WorkBookItemSelectCommunityFile(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public WriteOnly Property AllowCommunityLink() As Boolean Implements lm.Comol.Modules.Base.Presentation.IWKSelectCommunityFile.AllowCommunityLink
        Set(ByVal value As Boolean)
            Me.LNBlinkToItem.Enabled = value
        End Set
    End Property

    Public WriteOnly Property BackToManagement() As System.Guid Implements lm.Comol.Modules.Base.Presentation.IWKSelectCommunityFile.BackToManagement
        Set(ByVal value As System.Guid)
            Me.HYPbackToFileManagement.Visible = Not (value = System.Guid.Empty)
            Me.HYPbackToFileManagement.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemManagementFile.aspx?ItemID=" & value.ToString & "&View=" & Me.PreviousWorkBookView.ToString
        End Set
    End Property
    Public ReadOnly Property PreloadedItemID() As System.Guid Implements lm.Comol.Modules.Base.Presentation.IWKSelectCommunityFile.PreloadedItemID
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
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As lm.Comol.Modules.Base.DomainModel.ModuleCommunityRepository Implements lm.Comol.Modules.Base.Presentation.IWKSelectCommunityFile.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                   Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
        If IsNothing(oModule) Then
            oModule = New ModuleCommunityRepository
        End If
        Return oModule
    End Function

#End Region
    Public ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter Implements IWKSelectCommunityFile.PreviousWorkBookView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property

#Region "Default Method"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Me.Page.IsPostBack = False Then
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
        MyBase.SetCulture("WorkBookItemManagementFile", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleItemCommunityFile")
            Me.Master.ServiceNopermission = .getValue("nopermissionItemCommunityFile")
            .setHyperLink(Me.HYPbackToFileManagement, True, True)
            .setLinkButton(LNBlinkToItem, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub InitializeFileSelector(ByVal CommunityID As Integer, ByVal SelectedFiles As System.Collections.Generic.List(Of Long), ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IWKSelectCommunityFile.InitializeFileSelector
        Me.CTRLCommunityFile.InitializeControl(CommunityID, SelectedFiles, ShowHiddenItems, AdminPurpose, True, lm.Comol.Core.DomainModel.Repository.RepositoryItemType.None)
    End Sub
    Public Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWKSelectCommunityFile.NoPermissionToManagementFiles
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub
    Public Sub ReturnToFileManagement(ByVal ItemID As System.Guid) Implements IWKSelectCommunityFile.ReturnToFileManagement
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & PreviousWorkBookView.ToString)
    End Sub

    Public Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWKSelectCommunityFile.AddCommunityFileAction
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.UploadCommunityFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_WorkBook.ObjectType.WorkBookItem, Me.PreloadedItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IWKSelectCommunityFile.NotifyAddCommunityFile
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemAddCommunityFileLink(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub

    Private Sub LNBlinkToItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBlinkToItem.Click
        Me.CurrentPresenter.UpdateFileLink(Me.CTRLCommunityFile.GetSelectedFiles)
    End Sub
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub
End Class