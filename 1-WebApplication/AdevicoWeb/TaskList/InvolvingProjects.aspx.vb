Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports System.Linq
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract
Imports System.Enum
Imports Telerik.Web.UI
Imports lm.Comol.Modules.Base
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain

Partial Public Class InvolvingProjects
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects

    Public ReadOnly Property DeletedClass(ByVal isDeleted As Boolean, ByVal isAlternating As Boolean) As String
        Get
            If isDeleted Then
                Return "ROW_Disabilitate_Small"
            ElseIf isAlternating Then
                Return "ROW_Alternate_Small"
            Else
                Return "ROW_Normal_Small"
            End If
        End Get
    End Property

#Region "Private Property"

    Private _TaskContext As TaskListContext
    Private _Pager As lm.Comol.Core.DomainModel.PagerBase
    Private _PageUtility As OLDpageUtility
    Private _Presenter As InvolvingProjectsPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))
    Private _Servizio As Services_TaskList
   

#End Region

#Region "Public Accessors Methods"
    Private ReadOnly Property PortalName() As String Implements IViewInvolvingProjects.PortalName
        Get
            Return SystemSettings.Presenter.PortalDisplay.LocalizeName(PageUtility.LinguaID)
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As InvolvingProjectsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InvolvingProjectsPresenter(Me.CurrentContext, Me)
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

    Private ReadOnly Property CurrentService() As Services_TaskList
        Get
            If IsNothing(_Servizio) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _Servizio = Services_TaskList.Create
                    With _Servizio
                        'Tia: tratti dai worbook
                        '. = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
                        '.Read = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                        '.GrantPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        '.Write = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)

                        'Tia : Tutti da controllare con Fra
                        .AddCommunityProject = False
                        .AddPersonalProject = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                        .Administration = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ViewCommunityProjects = (PersonTypeID <> Main.TipoPersonaStandard.Guest)



                        '7400_Services_TaskList
                        'AddCommunityProject = 8192 '13 Add
                        'Administration = 64 '6 Admin
                        'ManagementPermission = 32 '5 Grant
                        'ViewCommunityProjects = 1024 '10 Browse
                        'AddPersonalProject = 2 '1 Write
                    End With
                ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_TaskList(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_TaskList.Codex))
                Else
                    _Servizio = Me.PageUtility.GetCurrentServices.Find(Services_TaskList.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = Services_TaskList.Create
                    End If
                End If
            End If
            Return _Servizio
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

    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IViewInvolvingProjects.ModulePersmission
        Get
            Return TranslateComolPermissionToModulePermission(Me.CurrentService)
        End Get
    End Property

    Private Function TranslateComolPermissionToModulePermission(ByVal oService As Services_TaskList) As ModuleTaskList
        Dim oModulePermission As New ModuleTaskList
        With oService
            'Tia
            oModulePermission.Administration = .Administration
            oModulePermission.CreateCommunityProject = .AddCommunityProject OrElse .Administration
            'oModulePermission.CreatePersonalCommunityProject = True
            oModulePermission.CreatePersonalProject = True
            oModulePermission.DownloadAllowed = True
            oModulePermission.ManagementPermission = True
            oModulePermission.PrintTaskList = True
            oModulePermission.ViewTaskList = True

            'oModulePermission.DeleteMessage = 
            'oModulePermission.EditMessage = .Admin OrElse .Write
            'oModulePermission.ManagementPermission = .GrantPermission
            'oModulePermission.PrintMessage = .Read OrElse .Write OrElse .Admin
            'oModulePermission.RetrieveOldMessage = .Write OrElse .Admin
            'oModulePermission.ServiceAdministration = .Admin OrElse .Write
            'oModulePermission.ViewCurrentMessage = .Read OrElse .Write OrElse .Admin
            'oModulePermission.ViewOldMessage = .Read OrElse .Write OrElse .Admin
        End With
        Return oModulePermission
    End Function

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) Implements IViewInvolvingProjects.CommunitiesPermission
        Get

            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_TaskList.Codex) _
                                          Select New ModuleCommunityPermission(Of ModuleTaskList)() With {.ID = sb.CommunityID, .Permissions = New ModuleTaskList(New Services_TaskList(sb.PermissionString))}).ToList
                If _CommunitiesPermission Is Nothing Then
                    _CommunitiesPermission = New List(Of ModuleCommunityPermission(Of ModuleTaskList))
                End If
                _CommunitiesPermission.Add(New ModuleCommunityPermission(Of ModuleTaskList)() With {.ID = 0, .Permissions = ModuleTaskList.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)})
            End If
            Return _CommunitiesPermission
        End Get
    End Property

#End Region

#Region "PageBase"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.Master.ShowNoPermission = False
        Me.PGgrid.Pager = Me.Pager

    End Sub

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.PageUtility.AddAction(Me.ComunitaCorrenteID, Services_TaskList.ActionType.ViewInvolvingProject, Nothing, InteractionType.UserWithLearningObject)
            Me.CurrentPresenter.InitView()

        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AssignedTasks", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setLabel(Me.LBlegendaVD2)
            .setLiteral(Me.LTshowActive)
            .setLiteral(Me.LTfilterby)
            .setLiteral(Me.LTsortby)
            .setHyperLink(Me.HYPaddProject, True, True)
            .setRadioButtonList(Me.RBLselectPrj, 3)
            .setRadioButtonList(Me.RBLselectPrj, 4)
            .setRadioButtonList(Me.RBLselectPrj, 5)
            'For Each item As RadTab In TBStasklist.Tabs
            '    item.Text = .getValue(TBStasklist, item.Value)
            '    'item.ToolTip = item.Text
            'Next
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get

        End Get
    End Property
#End Region


#Region "Iview Property"
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewInvolvingProjects.Pager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            '   Me.DIVpageSize.Style.Add("display", IIf(Me.PGgrid.Visible, "block", "none"))
        End Set
    End Property

    Public ReadOnly Property CurrentPageIndex() As Integer Implements IViewInvolvingProjects.CurrentPageIndex
        Get
            If Me.Request.QueryString("Page") Is Nothing Then
                Return 0
            Else
                Try
                    Return CInt(Me.Request.QueryString("Page"))
                Catch ex As Exception
                    Return 0
                End Try
            End If
        End Get
    End Property

    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IViewInvolvingProjects.PreLoadedPageSize
        Get

            Dim PageSize As Integer = 50 ' Me.DDLpage.Items(0).Value
            Try
                PageSize = Request.QueryString("PageSize")
            Catch ex As Exception

            End Try
            If IsNothing(Request.QueryString("PageSize")) Then
                Return 50
                ' Return DDLpage.SelectedValue
            Else
                'If IsNothing(DDLpage.Items.FindByValue(PageSize)) Then
                'Return DDLpage.SelectedValue
                'Else
                Return PageSize
                'End If
            End If
        End Get
    End Property
    Public Property CurrentPageSize() As Integer Implements IViewInvolvingProjects.CurrentPageSize
        Get
            Return 50 ' Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            ' Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedView() As ViewModeType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.PreLoadedView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.InvolvingProjects)
        End Get
    End Property

    Public ReadOnly Property PreLoadedCommunityFilter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements IViewInvolvingProjects.PreLoadedCommunityFilter
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskFilter).GetByString(Request.QueryString("CommunityFilter"), lm.Comol.Modules.TaskList.Domain.TaskFilter.AllCommunities)
        End Get
    End Property
    Public Property CurrentCommunityFilter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.CurrentCommunityFilter
        Get
            Return Me.DDLfilterBy.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TaskFilter)
            Me.DDLfilterBy.SelectedValue = value
        End Set
    End Property

    Public Property CurrentSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.CurrentSorting
        Get
            Return Me.DDLsortBy.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.Sorting)
            Me.DDLsortBy.SelectedValue = value
        End Set
    End Property



    Public ReadOnly Property PreLoadedSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.PreLoadedSorting
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.Sorting).GetByString(Request.QueryString("Sorting"), lm.Comol.Modules.TaskList.Domain.Sorting.DeadlineOrder)
        End Get
    End Property


    Private Sub DDLsortBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLsortBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadInvolvingProjects()
    End Sub


    Public Sub LoadSorts(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.Sorting)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.LoadSorts
        Me.DDLsortBy.Items.Clear()
        For Each oSort As lm.Comol.Modules.TaskList.Domain.Sorting In oList
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLsortBy." & oSort)
            oItem.Value = oSort
            Me.DDLsortBy.Items.Add(oItem)
        Next
    End Sub


    Public ReadOnly Property PreLoadedOrderBy() As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.PreLoadedOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TasksPageOrderBy).GetByString(Request.QueryString("OrderBy"), TasksPageOrderBy.AllActive)
        End Get
    End Property
    Public Property CurrentOrderBy() As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.CurrentOrderBy
        Get
            If Not IsNothing(Me.RBLselectPrj.SelectedItem) Then
                Return Me.RBLselectPrj.SelectedValue
            Else
                Return TasksPageOrderBy.None
            End If
            'If Me.RBLselectPrj.Items.Count > 0 Then
            '    Return Me.RBLselectPrj.SelectedValue
            'Else
            '    Return TasksPageOrderBy.None
            'End If
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy)
            Try
                Me.RBLselectPrj.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property

    Private Function GetOrderByString(ByVal SortExpression As String) As TaskListOrder
        Dim iResponse As TaskListOrder
        If System.Enum.IsDefined(GetType(TaskListOrder), SortExpression) Then
            iResponse = System.Enum.Parse(GetType(TaskListOrder), SortExpression)
        Else
            iResponse = TaskListOrder.Project
        End If
        Return iResponse
    End Function

    Public Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType)) Implements IViewInvolvingProjects.LoadTaskTabs
        Me.TBStasklist.Tabs.Clear()

        For Each oViewMode As ViewModeType In oList
            Dim oTab As New RadTab()
            oTab.Value = oViewMode
            oTab.Text = Me.Resource.getValue("TBStasklist." & oViewMode)
            Me.TBStasklist.Tabs.Add(oTab)
            If oTab.Value = ViewModeType.InvolvingProjects Then
                oTab = Me.TBStasklist.SelectedTab
            End If

            If oTab.Value = ViewModeType.TaskAdmin Then
                If (From c In Me.CommunitiesPermission Where c.Permissions.Administration).Count > 0 Then
                    oTab.Visible = True
                Else
                    oTab.Visible = False
                End If
            End If



        Next
    End Sub

    Public Sub LoadFilters(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.TaskFilter)) Implements IViewInvolvingProjects.LoadFilters
        Me.DDLfilterBy.Items.Clear()
        For Each oFilter As lm.Comol.Modules.TaskList.Domain.TaskFilter In oList
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLfilterBy." & oFilter)
            oItem.Value = oFilter
            Me.DDLfilterBy.Items.Add(oItem) '(New ListItem(oFilter.ToString, oFilter))
        Next
    End Sub

    Public Sub LoadInvolvingProjects(ByVal oList As System.Collections.Generic.List(Of dtoInvolvingProjectsWithRolesWithHeader)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.LoadInvolvingProjects
        Me.RPTinvolvingProjects.Visible = True
        Me.RPTinvolvingProjects.DataSource = oList
        Me.RPTinvolvingProjects.DataBind()
    End Sub

    Public Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.NavigationUrl ' ByVal ViewMode As lm.Comol.Modules.Base.Presentation.TaskList.ViewModeType
        Me.PGgrid.BaseNavigateUrl = Me.BaseUrl & "TaskList/InvolvingProjects.aspx?View=InvolvingProjects&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page={0}"
    End Sub

    Public Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.SetNavigationUrlToAssignedTask
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TodayTasks)
        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/AssignedTasks.aspx?View=TodayTasks" & "&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.SetNavigationUrlToProject
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.InvolvingProjects)
        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/InvolvingProjects.aspx?View=InvolvingProjects&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal TaskType As lm.Comol.Modules.TaskList.Domain.TaskManagedType, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.SetNavigationUrlToManage
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TasksManagement)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&TaskType=Projects" & "&PageSize=" & PageSize.ToString & "&Page=0"
            'oTab.NavigateUrl = Me.BaseUrl & "TaskList/TaskManagement.aspx?View=TaskManagement&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.SetNavigationUrlToAdministration

        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TaskAdmin)
        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TaskAdministration.aspx?View=TaskAdmin&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.SetNavigationUrlToAddProject
        Me.HYPaddProject.NavigateUrl = Me.BaseUrl & "TaskList/AddProject.aspx?View=InvolvingProjects&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
    End Sub

    Public Sub RPTsingleProjects_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoInvolvingProjects As dtoInvolvingProjectsWithRoles = e.Item.DataItem
            If Not IsNothing(oDtoInvolvingProjects) Then
                Dim oIMsuspendedTask, oIMstartedTask, oIMnotStartedTask, oIMcompletedTask As System.Web.UI.WebControls.Image
                Dim oHypModifica, oHypProject As System.Web.UI.WebControls.HyperLink
                Dim oLNBcancellaDefinitivo, oLNBelimina, oLNBundelete As System.Web.UI.WebControls.LinkButton
                Dim oLTdeadline, oLTcompleteness, oLTroles As Literal
                Dim oLBLroles, oLBLtitle As Label

                oLBLroles = e.Item.FindControl("LBLroles")
                oLBLtitle = e.Item.FindControl("LBLtitle")

                oLNBcancellaDefinitivo = e.Item.FindControl("LNBdelete")
                oLNBelimina = e.Item.FindControl("LNBelimina")
                oLNBundelete = e.Item.FindControl("LNBundelete")

                oIMsuspendedTask = e.Item.FindControl("IMsuspendedTask")
                oIMstartedTask = e.Item.FindControl("IMstartedTask")
                oIMnotStartedTask = e.Item.FindControl("IMnotStartedTask")
                oIMcompletedTask = e.Item.FindControl("IMcompletedTask")

                oHypModifica = e.Item.FindControl("HYPmodifica")
                oHypProject = e.Item.FindControl("HYPproject")

                oLTdeadline = e.Item.FindControl("LTdeadline")
                oLTcompleteness = e.Item.FindControl("LTcompleteness")
                oLTroles = e.Item.FindControl("LTroles")

                oIMsuspendedTask.Visible = (e.Item.DataItem.Status = TaskStatus.suspended)
                oIMstartedTask.Visible = (e.Item.DataItem.Status = TaskStatus.started)
                oIMnotStartedTask.Visible = e.Item.DataItem.Status = TaskStatus.notStarted
                oIMcompletedTask.Visible = (e.Item.DataItem.Status = TaskStatus.completed)

                If Not IsNothing(oIMcompletedTask) Then
                    oIMcompletedTask.ImageUrl = Me.BaseUrl & "images/TaskList/completed20.png"
                    MyBase.Resource.setImage(oIMcompletedTask, True)
                End If
                If Not IsNothing(oIMstartedTask) Then
                    oIMstartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/STARTEDoe.png"
                    Me.Resource.setImage(oIMstartedTask, True)
                End If
                If Not IsNothing(oIMnotStartedTask) Then
                    oIMnotStartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/NOTSTARTEDoe.png"
                    MyBase.Resource.setImage(oIMnotStartedTask, True)
                End If
                If Not IsNothing(oIMsuspendedTask) Then
                    oIMsuspendedTask.ImageUrl = Me.BaseUrl & "images/TaskList/SUSPENDEDoe.png"
                    Me.Resource.setImage(oIMsuspendedTask, True)
                End If

                If Not IsNothing(oHypModifica) Then
                    oHypModifica.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"
                    Me.Resource.setHyperLink(oHypModifica, True, True)
                    If Me.CurrentPresenter.CanUpdate(oDtoInvolvingProjects.Permissions) Then
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoInvolvingProjects.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString & "&ViewToLoad=" & ViewModeType.InvolvingProjects.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                    Else
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoInvolvingProjects.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.InvolvingProjects.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                    End If
                End If

                If Not IsNothing(oLNBcancellaDefinitivo) Then
                    Me.Resource.setLinkButton(oLNBcancellaDefinitivo, False, True, , True)
                    oLNBcancellaDefinitivo.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLNBcancellaDefinitivo.Text = String.Format(oLNBcancellaDefinitivo.Text, Me.BaseUrl & "images/Grid/eliminato1.gif", oLNBcancellaDefinitivo.ToolTip)
                End If
                If Not IsNothing(oLNBelimina) Then
                    Me.Resource.setLinkButton(oLNBelimina, False, True, , True)
                    oLNBelimina.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLNBelimina.Text = String.Format(oLNBelimina.Text, Me.BaseUrl & "images/Grid/cancella.gif", oLNBelimina.ToolTip)
                End If
                If Not IsNothing(oLNBundelete) Then
                    Me.Resource.setLinkButton(oLNBundelete, False, True)
                    oLNBundelete.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLNBundelete.Text = String.Format(oLNBundelete.Text, Me.BaseUrl & "images/Grid/ripristina.gif", oLNBundelete.ToolTip)
                End If

                oLNBcancellaDefinitivo.CommandArgument = e.Item.DataItem.TaskId.ToString
                oLNBelimina.CommandArgument = e.Item.DataItem.TaskId.ToString
                oLNBundelete.CommandArgument = e.Item.DataItem.TaskId.ToString

                oLNBelimina.Visible = Not e.Item.DataItem.isDeleted AndAlso ((oDtoInvolvingProjects.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                oLNBundelete.Visible = e.Item.DataItem.isDeleted AndAlso ((oDtoInvolvingProjects.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete) 'AndAlso oItem.Permission.UndeleteWorkBook
                oLNBcancellaDefinitivo.Visible = e.Item.DataItem.isDeleted AndAlso ((oDtoInvolvingProjects.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete) 'AndAlso oItem.Permission.DeleteWorkBook

                If Not IsNothing(oLBLtitle) Then
                    Me.Resource.setLabel(oLBLtitle)
                End If
                If Not IsNothing(oLBLroles) Then
                    Me.Resource.setLabel(oLBLroles)
                End If

                If Not IsNothing(oHypProject) Then
                    'oHypProject.Text = oDtoInvolvingProjects.TaskName.ToString
                    oHypProject.Text = System.Web.HttpUtility.HtmlEncode(oDtoInvolvingProjects.TaskName.ToString)
                    oHypProject.NavigateUrl = Me.BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & oDtoInvolvingProjects.ProjectID.ToString & "&MainPage=" & ViewModeType.InvolvingProjects.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oLTroles) Then
                    Dim oString As String = ""
                    For i As Integer = 0 To (oDtoInvolvingProjects.Roles.Count - 1)
                        oString = oString & oDtoInvolvingProjects.Roles.Item(i).ToString & "; "
                    Next
                    oLTroles.Text = oString
                End If

                If Not IsNothing(oLTdeadline) Then
                    If Not IsNothing(e.Item.DataItem.Deadline) Then
                        Dim oDeadline As Date
                        Dim oDate As Date
                        oDate = Date.Now
                        oDeadline = CDate(e.Item.DataItem.Deadline)
                        If (oDate > oDeadline) And (oDtoInvolvingProjects.Status <> TaskStatus.completed) Then
                            oLTdeadline.Text = "<b><div style='background-color:trasparent;color:#FF0000'>" & oDeadline.ToString("dd/MM/yy") & "</div></b>"
                        Else
                            oLTdeadline.Text = oDeadline.ToString("dd/MM/yy")
                        End If

                    End If
                End If

                If Not IsNothing(oLTcompleteness) Then
                    oLTcompleteness.Text = e.Item.DataItem.Completeness.ToString() & " %"
                End If
                Dim oImage As System.Web.UI.WebControls.Image
                oImage = e.Item.FindControl("IMcompleteness")
                If Not IsNothing(oImage) Then
                    oImage.Height = "15"
                    oImage.Width = e.Item.DataItem.Completeness.ToString()
                    oImage.ToolTip = e.Item.DataItem.Completeness.ToString() & "%"
                    oImage.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
                End If
            End If



        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLTheaderStatus As Literal = e.Item.FindControl("LTheaderStatus")
            Dim oLTheaderDelete As Literal = e.Item.FindControl("LTheaderDelete")
            Dim oLTheaderModify As Literal = e.Item.FindControl("LTheaderModify")
            Dim oLTheaderProject As Literal = e.Item.FindControl("LTheaderProject")
            Dim oLTheaderDeadline As Literal = e.Item.FindControl("LTheaderDeadline")
            Dim oLTheaderCompleteness As Literal = e.Item.FindControl("LTheaderCompleteness")

            Try
                Me.Resource.setLiteral(oLTheaderStatus)
                Me.Resource.setLiteral(oLTheaderDelete)
                Me.Resource.setLiteral(oLTheaderModify)
                Me.Resource.setLiteral(oLTheaderProject)
                Me.Resource.setLiteral(oLTheaderCompleteness)
                Me.Resource.setLiteral(oLTheaderDeadline)
            Catch ex As Exception

            End Try

        End If

    End Sub

    Public Sub RPTinvolvingProjects_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTinvolvingProjects.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoInvolvingProjectsWithHeader As dtoInvolvingProjectsWithRolesWithHeader = e.Item.DataItem
            'Internazionalizzazione
            'Try
            '    Me.Resource.setLabel(e.Item.FindControl("LTcommunityName"))
            'Catch ex As Exception
            'End Try
            Dim LTheader As Literal
            LTheader = e.Item.FindControl("LTheader")
            LTheader.Text = oDtoInvolvingProjectsWithHeader.CommunityName
            Dim RPTsingleProjects As Repeater
            RPTsingleProjects = e.Item.FindControl("RPTsingleProjects")
            RPTsingleProjects.DataSource = oDtoInvolvingProjectsWithHeader.AssignedTasks
            AddHandler RPTsingleProjects.ItemDataBound, AddressOf RPTsingleProjects_ItemDataBound
            RPTsingleProjects.DataBind()
        End If
    End Sub

#End Region

    Public Sub ShowDeletedParentError() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.ShowDeletedParentError
        Me.PageUtility.AddAction(Me.ComunitaCorrenteID, Services_TaskList.ActionType.GenericError, , InteractionType.UserWithLearningObject)
        Dim str As String = "XXX"
        str = Me.Resource.getValue("ltlscript")
        Me.ltlscript.Text = "<script type=""text/javascript"">alert(""" + str + """);</script>"
    End Sub

    Private Sub DDLorderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfilterBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadInvolvingProjects()
    End Sub
    Private Sub RBLselectPrj_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLselectPrj.SelectedIndexChanged
        Me.CurrentPresenter.LoadInvolvingProjects()
    End Sub
    Public Sub RPTsingleProjects_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) 'Handles RPTassignedTasksByCommunity.ItemCommand
        Try
            Select Case e.CommandName
                Case "virtualdelete"
                    Me.CurrentPresenter.VirtualDelete(e.CommandArgument)

                Case "undelete"
                    Me.CurrentPresenter.Undelete(e.CommandArgument)

                Case "delete"
                    Me.CurrentPresenter.Delete(e.CommandArgument)

            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewInvolvingProjects.GoToReallocateResource
        Me.PageUtility.RedirectToUrl("/TaskList/ReallocateUsers.aspx?CurrentTaskID=" & TaskID & "&CurrentModeType=" & ReallocateType.ToString & "&PreviusPage=" & lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.PreviusPageName.InvolvingProject.ToString)
    End Sub
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub

End Class