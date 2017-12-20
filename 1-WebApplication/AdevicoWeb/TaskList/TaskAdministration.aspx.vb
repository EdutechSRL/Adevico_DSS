Imports lm.Comol.Core.DomainModel
Imports System.Linq
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract
Imports System.Enum
Imports Telerik.Web.UI
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.UI.Presentation


Public Class TaskAdministration
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration

    Private _TaskContext As TaskListContext
    Private _Pager As lm.Comol.Core.DomainModel.PagerBase
    Private _PageUtility As OLDpageUtility
    Private _Presenter As TaskAdministrationPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _Servizio As Services_TaskList
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) Implements IViewTaskAdministration.CommunitiesPermission
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

    'Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IViewTaskAdministration.ModulePersmission
    '    Get
    '        Return TranslateComolPermissionToModulePermissionAdmin(Me.CurrentService)
    '    End Get
    'End Property

    'Private ReadOnly Property CurrentService() As Services_TaskList
    '    Get
    '        If IsNothing(_Servizio) Then
    '            If isPortalCommunity Then
    '                Dim PersonTypeID As Integer = Me.TipoPersonaID
    '                _Servizio = Services_TaskList.Create
    '                With _Servizio
    '                    _Servizio = Services_TaskList.Create 'Aggiunta all 11 ottobre 
    '                    .AddCommunityProject = False
    '                    .AddPersonalProject = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
    '                    .Administration = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
    '                    .ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
    '                    .ViewCommunityProjects = (PersonTypeID <> Main.TipoPersonaStandard.Guest)

    '                End With
    '            ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
    '                _Servizio = New Services_TaskList(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_TaskList.Codex))
    '            Else
    '                _Servizio = Me.PageUtility.GetCurrentServices.Find(Services_TaskList.Codex)
    '                If IsNothing(_Servizio) Then
    '                    _Servizio = Services_TaskList.Create
    '                End If
    '            End If
    '        End If
    '        Return _Servizio
    '    End Get
    'End Property

    'Private Function TranslateComolPermissionToModulePermissionAdmin(ByVal oService As Services_TaskList) As ModuleTaskList
    '    Dim oModulePermission As New ModuleTaskList
    '    With oService
    '        'Tia
    '        oModulePermission.Administration = .Administration
    '        oModulePermission.CreateCommunityProject = .Administration '.AddCommunityProject OrElse .Administration
    '        'oModulePermission.CreatePersonalCommunityProject = True
    '        oModulePermission.CreatePersonalProject = .Administration '.AddPersonalProject 'True
    '        oModulePermission.DownloadAllowed = .Administration
    '        oModulePermission.ManagementPermission = .Administration
    '        oModulePermission.PrintTaskList = .Administration
    '        oModulePermission.ViewTaskList = .Administration
    '    End With
    '    Return oModulePermission
    'End Function

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

#Region "Public Accessors Methods"
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.TaskList.TaskAdministrationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TaskAdministrationPresenter(Me.CurrentContext, Me)
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

    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewTaskAdministration.Pager
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
        MyBase.SetCulture("pg_TaskAdministration", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setLabel(Me.LBlegendaVD)
            .setLiteral(Me.LTorderby)
            .setLiteral(Me.LTfilterby)
            .setLiteral(Me.LTsortby)
            .setHyperLink(Me.HYPaddProject, True, True)
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
    Public Sub ShowDeletedParentError() Implements IViewTaskAdministration.ShowDeletedParentError
        Me.PageUtility.AddAction(Me.ComunitaCorrenteID, Services_TaskList.ActionType.GenericError, , InteractionType.UserWithLearningObject)
        Dim str As String = "XXX"
        str = Me.Resource.getValue("ltlscript")
        Me.ltlscript.Text = "<script type=""text/javascript"">alert(""" + str + """);</script>"
    End Sub

    Public ReadOnly Property CurrentPageIndex() As Integer Implements IViewTaskAdministration.CurrentPageIndex
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

    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IViewTaskAdministration.PreLoadedPageSize
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
    Public Property CurrentPageSize() As Integer Implements IViewTaskAdministration.CurrentPageSize
        Get
            Return 50
        End Get
        Set(ByVal value As Integer)
            ' Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedView() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.PreLoadedView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public ReadOnly Property PreLoadedOrderBy() As ProjectOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.PreLoadedOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.ProjectOrderBy).GetByString(Request.QueryString("OrderBy"), ProjectOrderBy.AllActive)
        End Get
    End Property
    Public Property CurrentOrderBy() As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.CurrentOrderBy
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
    Public Property CurrentSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.CurrentSorting
        Get
            Return Me.DDLsortBy.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.Sorting)
            Me.DDLsortBy.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.PreLoadedSorting
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.Sorting).GetByString(Request.QueryString("Sorting"), lm.Comol.Modules.TaskList.Domain.Sorting.DeadlineOrder)
        End Get
    End Property

    Private Sub DDLsortBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLsortBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadAdministratedTasks()
    End Sub


    Public Sub LoadSorts(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.Sorting)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.LoadSorts
        Me.DDLsortBy.Items.Clear()
        For Each oSort As lm.Comol.Modules.TaskList.Domain.Sorting In oList
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLsortBy." & oSort)
            oItem.Value = oSort
            Me.DDLsortBy.Items.Add(oItem)
        Next
    End Sub



    Public ReadOnly Property PreLoadedCommunityFilter() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.PreLoadedCommunityFilter
        Get
            Return -1 'lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskFilter).GetByString(Request.QueryString("CommunityFilter"), TaskFilter.AllCommunities)
        End Get
    End Property
    Public Property CurrentCommunityFilter() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.CurrentCommunityFilter
        Get
            Return Me.DDLfilterby.SelectedValue
        End Get
        Set(ByVal value As Integer)
            Me.DDLfilterby.SelectedValue = value
        End Set
    End Property

    Public Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.LoadTaskTabs
        Me.TBStasklist.Tabs.Clear()

        For Each oViewMode As ViewModeType In oList
            Dim oTab As New RadTab()
            oTab.Value = oViewMode
            oTab.Text = Me.Resource.getValue("TBStasklist." & oViewMode)
            Me.TBStasklist.Tabs.Add(oTab)

            If oTab.Value = ViewModeType.TaskAdmin Then
                oTab = Me.TBStasklist.SelectedTab
            End If
        Next

    End Sub

    'New metohd
    Public Sub LoadAdministredTask(ByVal ListOfDtoAdministeredTasks As List(Of dtoAdminProjectsWithCommunityHeader)) Implements IViewTaskAdministration.LoadAdministredTask
        Me.RPTmanagedTasks.Visible = True
        Me.RPTmanagedTasks.DataSource = ListOfDtoAdministeredTasks
        Me.RPTmanagedTasks.DataBind()
    End Sub

    Public Sub LoadFilters(ByVal oList As List(Of dtoCommunityForDDL)) Implements IViewTaskAdministration.LoadFilters
        Me.DDLfilterby.Items.Clear()
        Dim oAllCommItem As New ListItem
        oAllCommItem.Value = -1
        oAllCommItem.Text = Me.Resource.getValue("DDLfilterBy.1")
        Me.DDLfilterby.Items.Add(oAllCommItem)
        For Each oItem As lm.Comol.Modules.TaskList.Domain.dtoCommunityForDDL In oList
            Dim oListitem As New ListItem
            oListitem.Text = oItem.Name
            oListitem.Value = oItem.ID
            Me.DDLfilterby.Items.Add(oListitem)
        Next
    End Sub

    Public Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.NavigationUrl ' ByVal ViewMode As lm.Comol.Modules.TaskList.Domain.ViewModeType
        Me.PGgrid.BaseNavigateUrl = Me.BaseUrl & "TaskList/AssignedTasks.aspx?View=TodayTasks&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & PageSize.ToString & "&Page={0}"
        'Me.PGgrid.BaseNavigateUrl = Me.BaseUrl & "TaskList/" & ViewMode.ToString & ".aspx?&OrderBy=" & OrderBy.ToString & "&View=" & ViewMode.ToString & "&QueryString=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page={0}"

    End Sub

    Public Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.SetNavigationUrlToAssignedTask
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TodayTasks)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/AssignedTasks.aspx?View=TodayTasks&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.SetNavigationUrlToProject
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.InvolvingProjects)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/InvolvingProjects.aspx?View=InvolvingProjects&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy, ByVal Type As TaskManagedType, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.SetNavigationUrlToManage
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TasksManagement)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Me.CurrentCommunityFilter.ToString & "&TaskType=Projects" & "&PageSize=" & PageSize.ToString & "&Page=0"
            'oTab.NavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&TaskType=" & Type.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If

    End Sub
    Public Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.SetNavigationUrlToAdministration
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TaskAdmin)
        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TaskAdministration.aspx?View=TaskAdministration&OrderBy=" & OrderBy.ToString & "&CommunityFilter=-1" & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.ProjectOrderBy) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.SetNavigationUrlToAddProject
        Me.HYPaddProject.NavigateUrl = Me.BaseUrl & "TaskList/AddProject.aspx?View=TaskAdministration&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
    End Sub
    Private Sub DDLorderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfilterby.SelectedIndexChanged
        Me.CurrentPresenter.LoadAdministratedTasks()
    End Sub
   
    Private Sub RBLtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLtype.SelectedIndexChanged
        Me.CurrentPresenter.LoadAdministratedTasks()
    End Sub

    Public Sub RPTmanagedTasksComponent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAdminProjects As dtoAdminProjects = e.Item.DataItem
            If Not IsNothing(oDtoAdminProjects) Then
                Dim oIMsuspendedTask, oIMstartedTask, oIMnotStartedTask, oIMcompletedTask As System.Web.UI.WebControls.Image
                Dim oHypModifica, oHypProject, oHypTask As HyperLink
                Dim oLNBcancellaDefinitivo, oLNBelimina, oLNBundelete As System.Web.UI.WebControls.LinkButton
                Dim oLTdeadline, oLTcompleteness As System.Web.UI.WebControls.Literal 'oLTcompleteness 

                Dim oTDitemTask, oTDalternItemTask As HtmlTableCell

                oTDitemTask = e.Item.FindControl("TDitemTask")
                oTDalternItemTask = e.Item.FindControl("TDalternItemTask")

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

                oLTdeadline = e.Item.FindControl("LTdeadline")
                oLTcompleteness = e.Item.FindControl("LTcompleteness")

                oIMsuspendedTask.Visible = (oDtoAdminProjects.Status = TaskStatus.suspended)
                oIMstartedTask.Visible = (oDtoAdminProjects.Status = TaskStatus.started)
                oIMnotStartedTask.Visible = (oDtoAdminProjects.Status = TaskStatus.notStarted)
                oIMcompletedTask.Visible = (oDtoAdminProjects.Status = TaskStatus.completed)


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

                'Eseguire controllo permessi: Se son almeno manager link ad Editable, se son resource ,link a Read
                If Not IsNothing(oHypModifica) Then
                    oHypModifica.Visible = oDtoAdminProjects.AllowEdit
                    Me.Resource.setHyperLink(oHypModifica, False, True, , False)
                    oHypModifica.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"
                    'oHypModifica.ToolTip = Me.Resource.getValue("Modify") '"Modify"
                    'If Me.CurrentPresenter.CanUpdate(oDtoAdminProjects.Permissions) Then
                    oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAdminProjects.ProjectID.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString & "&ViewToLoad=" & ViewModeType.TaskAdmin.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                    'Else
                    'oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAdminProjects.ProjectID.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TasksManagement.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                    'End If
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

                oLNBcancellaDefinitivo.CommandArgument = oDtoAdminProjects.ProjectID.ToString
                oLNBelimina.CommandArgument = oDtoAdminProjects.ProjectID.ToString
                oLNBundelete.CommandArgument = oDtoAdminProjects.ProjectID.ToString

                oLNBelimina.Visible = Not oDtoAdminProjects.isDeleted AndAlso (oDtoAdminProjects.AllowVirtualDelete) 'AndAlso ((oDtoAssignedTasks.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                oLNBundelete.Visible = oDtoAdminProjects.isDeleted AndAlso (oDtoAdminProjects.AllowUndelete) 'AndAlso ((oDtoAssignedTasks.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete) 'AndAlso oItem.Permission.UndeleteWorkBook
                oLNBcancellaDefinitivo.Visible = oDtoAdminProjects.isDeleted AndAlso (oDtoAdminProjects.AllowDelete) 'AndAlso ((oDtoAssignedTasks.Permissions And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete) 'AndAlso oItem.Permission.DeleteWorkBook

                If Not IsNothing(oHypTask) Then
                    'oHypTask.Text = oDtoAdminProjects.TaskName.ToString
                    oHypTask.Text = System.Web.HttpUtility.HtmlEncode(oDtoAdminProjects.ProjectName.ToString)
                    oHypTask.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAdminProjects.ProjectID.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TaskAdmin.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oHypProject) Then
                    'oHypProject.Text = oDtoAdminProjects.ProjectName.ToString
                    oHypProject.Text = System.Web.HttpUtility.HtmlEncode(oDtoAdminProjects.ProjectName.ToString)
                    oHypProject.NavigateUrl = Me.BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & oDtoAdminProjects.ProjectID.ToString & "&MainPage=" & ViewModeType.TaskAdmin.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&TypeOfTask=" & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oLTdeadline) Then
                    If Not IsNothing(oDtoAdminProjects.Deadline) Then
                        Dim oDeadline As Date
                        Dim oDate As Date
                        oDate = Date.Now
                        oDeadline = CDate(oDtoAdminProjects.Deadline)
                        If (oDate > oDeadline) And (oDtoAdminProjects.Status <> TaskStatus.completed) Then
                            oLTdeadline.Text = "<b><div style='background-color:trasparent;color:#FF0000'>" & oDeadline.ToString("dd/MM/yy") & "</div></b>"
                        Else
                            oLTdeadline.Text = oDeadline.ToString("dd/MM/yy")
                        End If

                    End If
                End If
                If Not IsNothing(oLTcompleteness) Then
                    oLTcompleteness.Text = oDtoAdminProjects.Completeness.ToString() & " %"
                End If
                Dim oImage As System.Web.UI.WebControls.Image
                oImage = e.Item.FindControl("IMcompleteness")
                If Not IsNothing(oImage) Then
                    oImage.Height = "15"
                    oImage.Width = oDtoAdminProjects.Completeness.ToString()
                    oImage.ToolTip = oDtoAdminProjects.Completeness.ToString() & "%"
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

            Try
                Me.Resource.setLiteral(oLTheaderStatus)
                Me.Resource.setLiteral(oLTheaderDelete)
                Me.Resource.setLiteral(oLTheaderModify)
                Me.Resource.setLiteral(oLTheaderTask)
                Me.Resource.setLiteral(oLTheaderProject)
                Me.Resource.setLiteral(oLTheaderCompleteness)
                Me.Resource.setLiteral(oLTheaderDeadline)
            Catch ex As Exception

            End Try
            Dim oTDheaderTask As HtmlTableCell
            oTDheaderTask = e.Item.FindControl("TDheaderTask")
            'If (Me.CurrentTaskTypeSelected = TaskManagedType.Projects) Then
            '    oTDheaderTask.Visible = Falsea
            'End If
        End If
    End Sub

    Public Sub RPTmanagedTasks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmanagedTasks.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAdminProjectsByCommunity As dtoAdminProjectsWithCommunityHeader = e.Item.DataItem

            Dim LTheader As Literal
            LTheader = e.Item.FindControl("LTheader")
            LTheader.Text = oDtoAdminProjectsByCommunity.CommunityName
            'LTcommunityName.DataBind()
            Dim RPTmanagedTasksComponent As Repeater
            RPTmanagedTasksComponent = e.Item.FindControl("RPTmanagedTasksComponent")
            RPTmanagedTasksComponent.DataSource = oDtoAdminProjectsByCommunity.ProjectsList
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

    Public Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskAdministration.GoToReallocateResource
        Me.PageUtility.RedirectToUrl("/TaskList/ReallocateUsers.aspx?CurrentTaskID=" & TaskID & "&CurrentModeType=" & ReallocateType.ToString & "&PreviusPage=" & IViewReallocateUsers.PreviusPageName.ManageTaskAssignment.ToString)
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub

    'Public ReadOnly Property PreLoadedTaskTypeSelected() As lm.Comol.Modules.TaskList.Domain.TaskManagedType Implements IViewTaskAdministration.PreLoadedTaskTypeSelected
    '    Get
    '        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskManagedType).GetByString(Request.QueryString("TaskType"), TaskManagedType.Projects)
    '    End Get
    'End Property
    'Public Property CurrentTaskTypeSelected() As lm.Comol.Modules.TaskList.Domain.TaskManagedType Implements IViewTaskAdministration.CurrentTaskTypeSelected
    '    Get
    '        If Not IsNothing(Me.RBLview.SelectedItem) Then
    '            Return Me.RBLview.SelectedValue
    '        Else
    '            Return TaskManagedType.None
    '        End If
    '    End Get
    '    Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TaskManagedType)
    '        Try
    '            Me.RBLview.SelectedValue = value
    '        Catch ex As Exception

    '        End Try

    '    End Set
    'End Property

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

End Class