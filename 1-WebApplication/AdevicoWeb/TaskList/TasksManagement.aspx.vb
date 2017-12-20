Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports System.Linq
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract
Imports System.Enum
Imports Telerik.Web.UI
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList.Domain


Partial Public Class TasksManagement
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement


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
    Private _Presenter As TasksManagementPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))
    Private _Servizio As Services_TaskList

#End Region

#Region "Public Accessors Methods"
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.TaskList.TasksManagementPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TasksManagementPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property



    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewTasksManagement.Pager
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

    Public Property CurrentSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.CurrentSorting
        Get
            Return Me.DDLsortBy.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.Sorting)
            Me.DDLsortBy.SelectedValue = value
        End Set
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

    Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IViewTasksManagement.ModulePersmission
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

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) Implements IViewTasksManagement.CommunitiesPermission
        Get

            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.PageUtility.CurrentContext.UserContext.CurrentUser.Id, Services_TaskList.Codex) _
                                          Select New ModuleCommunityPermission(Of ModuleTaskList)() With {.ID = sb.CommunityID, .Permissions = New ModuleTaskList(New Services_TaskList(sb.PermissionString))}).ToList
                If _CommunitiesPermission Is Nothing Then
                    _CommunitiesPermission = New List(Of ModuleCommunityPermission(Of ModuleTaskList))
                End If
                _CommunitiesPermission.Add(New ModuleCommunityPermission(Of ModuleTaskList)() With {.ID = 0, .Permissions = ModuleTaskList.CreatePortalmodule(Me.PageUtility.CurrentContext.UserContext.UserTypeID)})
            End If
            Return _CommunitiesPermission
        End Get
    End Property

    Private ReadOnly Property PortalName() As String Implements IViewTasksManagement.PortalName
        Get
            Return SystemSettings.Presenter.PortalDisplay.LocalizeName(PageUtility.LinguaID)
        End Get
    End Property

#End Region

#Region "PageBase"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
    End Sub

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        Me.PageUtility.AddAction(Me.ComunitaCorrenteID, Services_TaskList.ActionType.ViewTaskManagement, Nothing, InteractionType.UserWithLearningObject)
        Me.CurrentPresenter.InitView()
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
        MyBase.SetCulture("pg_TasksManagement", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setLabel(Me.LBlegendaVD)
            .setLiteral(Me.LTorder)
            .setLiteral(Me.LTfilterby)
            .setLiteral(Me.LTsortby)
            .setLiteral(Me.LTshowActive)
            '.setLiteral(Me.LTshowactive)
            .setHyperLink(Me.HYPaddProject, True, True)
            .setRadioButtonList(Me.RBLview, 1)
            .setRadioButtonList(Me.RBLview, 2)
            .setRadioButtonList(Me.RBLtype, 3)
            .setRadioButtonList(Me.RBLtype, 4)
            .setRadioButtonList(Me.RBLtype, 5)
            '    .setLabel(Me.LBNopermessi)
            '    .setLabel(Me.LBtitoloServizio)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get

        End Get
    End Property
#End Region

    'Non usata. Lasciata per eventuali usi futuri : 
    Public Sub ShowDeletedParentError() Implements IViewTasksManagement.ShowDeletedParentError
        Me.PageUtility.AddAction(Me.ComunitaCorrenteID, Services_TaskList.ActionType.GenericError, , InteractionType.UserWithLearningObject)
        Dim str As String = "XXX"
        str = Me.Resource.getValue("ltlscript")
        Me.ltlscript.Text = "<script type=""text/javascript"">alert(""" + str + """);</script>"
    End Sub

    Public ReadOnly Property CurrentPageIndex() As Integer Implements IViewTasksManagement.CurrentPageIndex
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

    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IViewTasksManagement.PreLoadedPageSize
        Get

            Dim PageSize As Integer = 50 '0 ' Me.DDLpage.Items(0).Value
            Try
                PageSize = Request.QueryString("PageSize")
            Catch ex As Exception

            End Try
            If IsNothing(Request.QueryString("PageSize")) Then
                Return 50 '0
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

    Public Property CurrentPageSize() As Integer Implements IViewTasksManagement.CurrentPageSize
        Get
            Return 50 '0 ' Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            ' Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedView() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.PreLoadedView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.TasksManagement)
        End Get
    End Property

    Public ReadOnly Property PreLoadedOrderBy() As ProjectOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.PreLoadedOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.ProjectOrderBy).GetByString(Request.QueryString("OrderBy"), ProjectOrderBy.AllActive)
        End Get
    End Property
    Public Property CurrentOrderBy() As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.CurrentOrderBy
        Get
            If Not IsNothing(Me.RBLtype.SelectedItem) Then
                Return Me.RBLtype.SelectedValue
            Else
                Return ProjectOrderBy.None
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy)
            Try
                Me.RBLtype.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property

    Public ReadOnly Property PreLoadedCommunityFilter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.PreLoadedCommunityFilter
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskFilter).GetByString(Request.QueryString("CommunityFilter"), TaskFilter.AllCommunities)
        End Get
    End Property
    Public Property CurrentCommunityFilter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.CurrentCommunityFilter
        Get
            Return Me.DDLfilterby.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TaskFilter)
            Me.DDLfilterby.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedTaskTypeSelected() As lm.Comol.Modules.TaskList.Domain.TaskManagedType Implements IViewTasksManagement.PreLoadedTaskTypeSelected
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskManagedType).GetByString(Request.QueryString("TaskType"), TaskManagedType.Projects)
        End Get
    End Property
    Public Property CurrentTaskTypeSelected() As lm.Comol.Modules.TaskList.Domain.TaskManagedType Implements IViewTasksManagement.CurrentTaskTypeSelected
        Get
            If Not IsNothing(Me.RBLview.SelectedItem) Then
                Return Me.RBLview.SelectedValue
            Else
                Return TaskManagedType.None
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TaskManagedType)
            Try
                Me.RBLview.SelectedValue = value
            Catch ex As Exception

            End Try

        End Set
    End Property

    Public Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.LoadTaskTabs
        Me.TBStasklist.Tabs.Clear()

        For Each oViewMode As ViewModeType In oList
            Dim oTab As New RadTab()
            oTab.Value = oViewMode
            oTab.Text = Me.Resource.getValue("TBStasklist." & oViewMode)
            Me.TBStasklist.Tabs.Add(oTab)

            If oTab.Value = ViewModeType.TaskAdmin Then
                If (From c In Me.CommunitiesPermission Where c.Permissions.Administration).Count > 0 Then
                    oTab.Visible = True
                Else
                    oTab.Visible = False
                End If
            End If

            If oTab.Value = ViewModeType.TasksManagement Then
                oTab = Me.TBStasklist.SelectedTab
            End If
        Next

    End Sub

    Public Sub LoadFilters(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.TaskFilter)) Implements IViewTasksManagement.LoadFilters
        Me.DDLfilterby.Items.Clear()
        For Each oFilter As TaskFilter In oList
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLfilterby." & oFilter)
            oItem.Value = oFilter
            Me.DDLfilterby.Items.Add(oItem) '(New ListItem(oFilter.ToString, oFilter))
        Next
    End Sub

    Public Sub LoadSorts(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.Sorting)) Implements IViewTasksManagement.LoadSorts
        Me.DDLsortBy.Items.Clear()
        For Each oSort As lm.Comol.Modules.TaskList.Domain.Sorting In oList
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLsortBy." & oSort)
            oItem.Value = oSort
            Me.DDLsortBy.Items.Add(oItem)
        Next
    End Sub

    Sub LoadManagedTasks(ByVal oList As System.Collections.Generic.List(Of dtoAssignedTasksWithCommunityHeader)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.LoadManagedTasks
        Me.RPTmanagedTasks.Visible = True
        Me.RPTmanagedTasks.DataSource = oList
        Me.RPTmanagedTasks.DataBind()
    End Sub

    Public ReadOnly Property PreLoadedSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.PreLoadedSorting
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.Sorting).GetByString(Request.QueryString("Sorting"), lm.Comol.Modules.TaskList.Domain.Sorting.DeadlineOrder)
        End Get
    End Property


    Private Sub DDLsortBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLsortBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadManagedTasks()
    End Sub



    Public Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy, ByVal Type As TaskManagedType, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.NavigationUrl ' ByVal ViewMode As lm.Comol.Module.BaseModules.Presentation.TaskList.ViewModeType
        Me.PGgrid.BaseNavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&TaskType=" & Type.ToString & "&PageSize=" & PageSize.ToString & "&Page={0}"
        'Me.PGgrid.BaseNavigateUrl = Me.BaseUrl & "TaskList/" & ViewMode.ToString & ".aspx?&OrderBy=" & OrderBy.ToString & "&View=" & ViewMode.ToString & "&QueryString=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page={0}"

    End Sub
    Public Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.SetNavigationUrlToAssignedTask
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TodayTasks)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/AssignedTasks.aspx?View=TodayTasks&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.SetNavigationUrlToProject
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.InvolvingProjects)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/InvolvingProjects.aspx?View=InvolvingProjects&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy, ByVal Type As TaskManagedType, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.SetNavigationUrlToManage
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TasksManagement)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&TaskType=Projects" & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
            'oTab.NavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&TaskType=" & Type.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.SetNavigationUrlToAdministration
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TaskAdmin)
        If Not IsNothing(oTab) Then
            'oTab.NavigateUrl = Me.BaseUrl & "TaskList/TaskAdministration.aspx?View=TaskAdmin&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TaskAdministration.aspx?View=TaskAdmin&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"

        End If
    End Sub

    'Public Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy) Implements lm.Comol.Modules.Base.Presentation.IViewAssignedTasks.SetNavigationUrlToAdministration
    '    Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TaskAdmin)
    '    If Not IsNothing(oTab) Then
    '        oTab.NavigateUrl = Me.BaseUrl & "TaskList/TaskAdministration.aspx?View=TaskAdmin&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
    '    End If
    'End Sub


    Public Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal Tasktype As TaskManagedType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.SetNavigationUrlToAddProject
        Me.HYPaddProject.NavigateUrl = Me.BaseUrl & "TaskList/AddProject.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0" & "&TypeOfTask=" & Tasktype.ToString
    End Sub
    Private Sub DDLorderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfilterby.SelectedIndexChanged
        Me.CurrentPresenter.LoadManagedTasks()
    End Sub
    Private Sub RBLview_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLview.SelectedIndexChanged
        Me.CurrentPresenter.LoadManagedTasks()
    End Sub
    Private Sub RBLtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLtype.SelectedIndexChanged
        Me.CurrentPresenter.LoadManagedTasks()
    End Sub

    Public Sub RPTmanagedTasksComponent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAssignedTasks As dtoAssignedTasks = e.Item.DataItem
            If Not IsNothing(e.Item.DataItem) Then
                Dim oIMsuspendedTask, oIMstartedTask, oIMnotStartedTask, oIMcompletedTask As System.Web.UI.WebControls.Image
                Dim oHypModifica, oHypProject, oHypTask, oHypUserDetail As HyperLink
                Dim oLNBcancellaDefinitivo, oLNBelimina, oLNBundelete As System.Web.UI.WebControls.LinkButton
                Dim oLTdeadline, oLTcompleteness As System.Web.UI.WebControls.Literal 'oLTcompleteness 

                Dim oTDitemTask, oTDalternItemTask As HtmlTableCell


                'oTDheaderText = e.Item.FindControl("TDheaderTask")
                oTDitemTask = e.Item.FindControl("TDitemTask")
                oTDalternItemTask = e.Item.FindControl("TDalternItemTask")

                If (Me.CurrentTaskTypeSelected = TaskManagedType.Projects) Then
                    If e.Item.ItemType = ListItemType.Item Then 'Then If Not IsNothing(ListItemType.Item) Then
                        oTDitemTask.Visible = False
                        'End If
                    ElseIf Not IsNothing(ListItemType.AlternatingItem) Then
                        oTDalternItemTask.Visible = False
                    End If
                End If



                oLNBcancellaDefinitivo = e.Item.FindControl("LNBdelete")
                oLNBelimina = e.Item.FindControl("LNBelimina")
                oLNBundelete = e.Item.FindControl("LNBundelete")
                oIMsuspendedTask = e.Item.FindControl("IMsuspendedTask")
                oIMstartedTask = e.Item.FindControl("IMstartedTask")
                oIMnotStartedTask = e.Item.FindControl("IMnotStartedTask")
                oIMcompletedTask = e.Item.FindControl("IMcompletedTask")

                oHypModifica = e.Item.FindControl("HYPmodifica")
                oHypTask = e.Item.FindControl("HYPtaskByCommunity")
                oHypProject = e.Item.FindControl("HYPprojectByCommunity")
                oHypUserDetail = e.Item.FindControl("HYPUserDetail")

                oLTdeadline = e.Item.FindControl("LTdeadline")
                oLTcompleteness = e.Item.FindControl("LTcompleteness")

                oIMsuspendedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.suspended)
                oIMstartedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.started)
                oIMnotStartedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.notStarted)
                oIMcompletedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.completed)

                If Not IsNothing(oIMcompletedTask) Then
                    oIMcompletedTask.ImageUrl = Me.BaseUrl & "images/TaskList/completed20.png"
                    oIMcompletedTask.ToolTip = Me.Resource.getValue("TaskStatus.Completed") ' "Completed"
                End If
                If Not IsNothing(oIMstartedTask) Then
                    oIMstartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/STARTEDoe.png"
                    oIMstartedTask.ToolTip = Me.Resource.getValue("TaskStatus.Started") '"Started"

                End If
                If Not IsNothing(oIMnotStartedTask) Then
                    oIMnotStartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/NOTSTARTEDoe.png"
                    oIMnotStartedTask.ToolTip = Me.Resource.getValue("TaskStatus.notStarted") 'Not Started"
                End If
                If Not IsNothing(oIMsuspendedTask) Then
                    oIMsuspendedTask.ImageUrl = Me.BaseUrl & "images/TaskList/SUSPENDEDoe.png"
                    oIMsuspendedTask.ToolTip = Me.Resource.getValue("TaskStatus.Suspended") '"Suspended"
                    Me.Resource.setImage(oIMsuspendedTask, True)
                End If

                If Not IsNothing(oHypUserDetail) Then
                    Me.Resource.setHyperLink(oHypUserDetail, False, True, , False)
                    oHypUserDetail.ImageUrl = Me.BaseUrl & "images/TaskList/permessiutenti.jpg"
                    oHypUserDetail.NavigateUrl = Me.BaseUrl & "TaskList/ProjectDetailWithUsersResume.aspx?ProjectID=" & oDtoAssignedTasks.ProjectID.ToString & "&ViewToLoad=" & ViewModeType.TasksManagement.ToString '& "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & Me.CurrentTaskTypeSelected.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                'Eseguire controllo permessi: Se son almeno manager link ad Editable, se son resource ,link a Read
                If Not IsNothing(oHypModifica) Then
                    Me.Resource.setHyperLink(oHypModifica, False, True, , False)
                    oHypModifica.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"
                    'oHypModifica.ToolTip = Me.Resource.getValue("Modify") '"Modify"
                    If Me.CurrentPresenter.CanUpdate(oDtoAssignedTasks.Permissions) Then
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString & "&ViewToLoad=" & ViewModeType.TasksManagement.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & Me.CurrentTaskTypeSelected.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                    Else
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TasksManagement.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & Me.CurrentTaskTypeSelected.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
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

                oLNBelimina.Visible = Not e.Item.DataItem.isDeleted AndAlso ((oDtoAssignedTasks.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                oLNBundelete.Visible = e.Item.DataItem.isDeleted AndAlso ((oDtoAssignedTasks.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete) 'AndAlso oItem.Permission.UndeleteWorkBook
                oLNBcancellaDefinitivo.Visible = e.Item.DataItem.isDeleted AndAlso ((oDtoAssignedTasks.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete) 'AndAlso oItem.Permission.DeleteWorkBook

                If Not IsNothing(oHypTask) Then
                    'oHypTask.Text = e.Item.DataItem.TaskName.ToString
                    oHypTask.Text = System.Web.HttpUtility.HtmlEncode(e.Item.DataItem.TaskName.ToString)
                    oHypTask.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TasksManagement.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & Me.CurrentTaskTypeSelected.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oHypProject) Then
                    'oHypProject.Text = e.Item.DataItem.ProjectName.ToString
                    oHypProject.Text = System.Web.HttpUtility.HtmlEncode(oDtoAssignedTasks.ProjectName.ToString)
                    oHypProject.NavigateUrl = Me.BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & oDtoAssignedTasks.ProjectID.ToString & "&MainPage=" & ViewModeType.TasksManagement.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & Me.CurrentTaskTypeSelected.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oLTdeadline) Then
                    If Not IsNothing(e.Item.DataItem.Deadline) Then
                        Dim oDeadline As Date
                        Dim oDate As Date
                        oDate = Date.Now
                        oDeadline = CDate(e.Item.DataItem.Deadline)
                        If (oDate > oDeadline) And (oDtoAssignedTasks.Status <> TaskStatus.completed) Then
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
            Dim oLTheaderTask As Literal = e.Item.FindControl("LTheaderTask")
            Dim oLTheaderProject As Literal = e.Item.FindControl("LTheaderProject")
            Dim oLTheaderDeadline As Literal = e.Item.FindControl("LTheaderDeadline")
            Dim oLTheaderCompleteness As Literal = e.Item.FindControl("LTheaderCompleteness")
            Dim oLTheaderUserResume As Literal = e.Item.FindControl("LTheaderUserResume")

            Try
                Me.Resource.setLiteral(oLTheaderStatus)
                Me.Resource.setLiteral(oLTheaderDelete)
                Me.Resource.setLiteral(oLTheaderModify)
                Me.Resource.setLiteral(oLTheaderTask)
                Me.Resource.setLiteral(oLTheaderProject)
                Me.Resource.setLiteral(oLTheaderCompleteness)
                Me.Resource.setLiteral(oLTheaderDeadline)
                Me.Resource.setLiteral(oLTheaderUserResume)

            Catch ex As Exception

            End Try
            Dim oTDheaderTask As HtmlTableCell
            oTDheaderTask = e.Item.FindControl("TDheaderTask")
            If (Me.CurrentTaskTypeSelected = TaskManagedType.Projects) Then
                oTDheaderTask.Visible = False

            End If
        End If
    End Sub

    Public Sub RPTmanagedTasks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmanagedTasks.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAssignedTasksByCommunity As dtoAssignedTasksWithCommunityHeader = e.Item.DataItem
            'Internazionalizzazione
            'Try
            '    Me.Resource.setLabel(e.Item.FindControl("LTcommunityName"))
            'Catch ex As Exception
            'End Try
            Dim LTheader As Literal
            LTheader = e.Item.FindControl("LTheader")
            LTheader.Text = oDtoAssignedTasksByCommunity.CommunityName
            'LTcommunityName.DataBind()
            Dim RPTmanagedTasksComponent As Repeater
            RPTmanagedTasksComponent = e.Item.FindControl("RPTmanagedTasksComponent")
            RPTmanagedTasksComponent.DataSource = oDtoAssignedTasksByCommunity.AssignedTasks
            AddHandler RPTmanagedTasksComponent.ItemDataBound, AddressOf RPTmanagedTasksComponent_ItemDataBound
            RPTmanagedTasksComponent.DataBind()
            'End If
        End If
    End Sub

    Public Sub RPTmanagedTasksComponent_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) 'Handles RPTassignedTasksByCommunity.ItemCommand
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

    'Private Function GetOrderByString(ByVal SortExpression As String) As TaskListOrder
    '    Dim iResponse As TaskListOrder
    '    If System.Enum.IsDefined(GetType(TaskListOrder), SortExpression) Then
    '        iResponse = System.Enum.Parse(GetType(TaskListOrder), SortExpression)
    '    Else
    '        iResponse = TaskListOrder.Project
    '    End If
    '    Return iResponse
    'End Function

    'Private Function GetTypeByString(ByVal SortExpression As String) As TaskManagedType
    '    Dim iResponse As TaskManagedType
    '    If System.Enum.IsDefined(GetType(TaskManagedType), SortExpression) Then
    '        iResponse = System.Enum.Parse(GetType(TaskManagedType), SortExpression)
    '    Else
    '        iResponse = TaskManagedType.Projects
    '    End If
    '    Return iResponse
    'End Function

    Public Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTasksManagement.GoToReallocateResource
        Me.PageUtility.RedirectToUrl("/TaskList/ReallocateUsers.aspx?CurrentTaskID=" & TaskID & "&CurrentModeType=" & ReallocateType.ToString & "&PreviusPage=" & IViewReallocateUsers.PreviusPageName.ManageTaskAssignment.ToString)
    End Sub
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub
End Class