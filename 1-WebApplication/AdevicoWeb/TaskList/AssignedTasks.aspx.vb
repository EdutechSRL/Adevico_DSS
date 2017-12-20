
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel
Imports System.Linq
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract
Imports System.Enum
Imports Telerik.Web.UI
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation


Partial Public Class AssignedTasks
    Inherits PageBase
    Implements IViewAssignedTasks

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
    Private _Presenter As AssignedTasksPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _Servizio As Services_TaskList

#End Region

#Region "Public Accessors Methods"
    Private ReadOnly Property CurrentPresenter() As AssignedTasksPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AssignedTasksPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
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
        'ltlscript.Text = ""
        If Page.IsPostBack = False Then 'messo Thom
            Me.PageUtility.AddAction(Me.ComunitaCorrenteID, Services_TaskList.ActionType.ViewAssignedTask, Nothing, InteractionType.UserWithLearningObject)

            Me.CurrentPresenter.InitView()
        End If 'messo Thom
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.Master.ShowNoPermission = True
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
            .setLiteral(Me.LTorder)
            .setLiteral(Me.LTfilterby)
            .setLiteral(Me.LTsortby)
            .setHyperLink(Me.HYPaddProject, True, True)
            .setLabel(Me.LBlegendaVD)
            .setRadioButtonList(Me.RBLview, 1)
            .setRadioButtonList(Me.RBLview, 2)
            .setRadioButtonList(Me.RBLview, 3)
            .setRadioButtonList(Me.RBLview, 4)
            .setRadioButtonList(Me.RBLview, 5)

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
    Private ReadOnly Property PortalName() As String Implements IViewAssignedTasks.PortalName
        Get
            Return SystemSettings.Presenter.PortalDisplay.LocalizeName(PageUtility.LinguaID)
        End Get
    End Property
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewAssignedTasks.Pager
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

    Public ReadOnly Property CurrentPageIndex() As Integer Implements IViewAssignedTasks.CurrentPageIndex
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

    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IViewAssignedTasks.PreLoadedPageSize
        Get
            Dim PageSize As Integer = 50 ' Me.DDLpage.Items(0).Value
            Try
                PageSize = Request.QueryString("PageSize")
            Catch ex As Exception

            End Try
            If IsNothing(Request.QueryString("PageSize")) Then
                Return 50
            Else
                Return PageSize
            End If
        End Get
    End Property
    Public Property CurrentPageSize() As Integer Implements IViewAssignedTasks.CurrentPageSize
        Get
            Return 50
        End Get
        Set(ByVal value As Integer)
            ' Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedView() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.PreLoadedView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public ReadOnly Property PreLoadedCommunityFilter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.PreLoadedCommunityFilter
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskFilter).GetByString(Request.QueryString("CommunityFilter"), lm.Comol.Modules.TaskList.Domain.TaskFilter.AllCommunities)
        End Get
    End Property
    Public Property CurrentCommunityFilter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.CurrentCommunityFilter
        Get
            Return Me.DDLfilterby.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TaskFilter)
            Me.DDLfilterby.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.PreLoadedSorting
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.Sorting).GetByString(Request.QueryString("Sorting"), lm.Comol.Modules.TaskList.Domain.Sorting.DeadlineOrder)
        End Get
    End Property

    Public Property CurrentSorting() As lm.Comol.Modules.TaskList.Domain.Sorting Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.CurrentSorting
        Get
            Return Me.DDLsortBy.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.Sorting)
            Me.DDLsortBy.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property PreLoadedOrderBy() As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.PreLoadedOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TasksPageOrderBy).GetByString(Request.QueryString("OrderBy"), TasksPageOrderBy.Community)
        End Get
    End Property
    Public Property CurrentOrderBy() As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.CurrentOrderBy
        Get
            If Not IsNothing(Me.RBLview.SelectedItem) Then
                Return Me.RBLview.SelectedValue
            Else
                Return TasksPageOrderBy.None
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy)
            Try
                Me.RBLview.SelectedValue = value
            Catch ex As Exception

            End Try
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

    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) Implements IViewAssignedTasks.CommunitiesPermission
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

    Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IViewAssignedTasks.ModulePersmission
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
        End With
        Return oModulePermission
    End Function

#End Region

    Public Sub LoadAssignedTasksByCommunity(ByVal oList As System.Collections.Generic.List(Of dtoAssignedTasksWithCommunityHeader)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.LoadAssignedTasksByCommunity
        Me.RPTassignedTasksByProject.Visible = False
        Me.RPTassignedTasksByCommunity.Visible = True
        Me.RPTassignedTasksByCommunity.DataSource = oList
        Me.RPTassignedTasksByCommunity.DataBind()

    End Sub

    Public Sub LoadAssignedTasksByProject(ByVal oList As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoAssignedTasksWithProjectHeader)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.LoadAssignedTasksByProject
        Me.RPTassignedTasksByProject.Visible = True
        Me.RPTassignedTasksByCommunity.Visible = False
        Me.RPTassignedTasksByProject.DataSource = oList
        Me.RPTassignedTasksByProject.DataBind()
    End Sub

    Public Sub RPTassignedTasksComponentCommunity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAssignedTasks As dtoAssignedTasks = e.Item.DataItem
            If Not IsNothing(e.Item.DataItem) Then
                Dim oIMsuspendedTask, oIMstartedTask, oIMnotStartedTask, oIMcompletedTask, oIMpendingTask As System.Web.UI.WebControls.Image
                Dim oHypModifica, oHypProject, oHypTask As HyperLink
                Dim oLNBcancellaDefinitivo, oLNBelimina, oLNBundelete As System.Web.UI.WebControls.LinkButton
                Dim oLTdeadline, oLTcompleteness As System.Web.UI.WebControls.Literal 'oLTcompleteness 

                oLNBcancellaDefinitivo = e.Item.FindControl("LNBdelete")
                oLNBelimina = e.Item.FindControl("LNBelimina")
                oLNBundelete = e.Item.FindControl("LNBundelete")
                oIMsuspendedTask = e.Item.FindControl("IMsuspendedTask")
                oIMstartedTask = e.Item.FindControl("IMstartedTask")
                oIMnotStartedTask = e.Item.FindControl("IMnotStartedTask")
                oIMcompletedTask = e.Item.FindControl("IMcompletedTask")
                oIMpendingTask = e.Item.FindControl("IMpendingTask")

                oHypModifica = e.Item.FindControl("HYPmodifica")
                oHypTask = e.Item.FindControl("HYPtaskByCommunity")
                oHypProject = e.Item.FindControl("HYPprojectByCommunity")

                oLTdeadline = e.Item.FindControl("LTdeadline")
                oLTcompleteness = e.Item.FindControl("LTcompleteness")

                oIMsuspendedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.suspended)
                oIMstartedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.started)
                oIMnotStartedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.notStarted)
                oIMcompletedTask.Visible = (oDtoAssignedTasks.Status = TaskStatus.completed)

                If Not IsNothing(oIMcompletedTask) Then
                    oIMcompletedTask.ImageUrl = Me.BaseUrl & "images/TaskList/completed20.png"
                End If
                If Not IsNothing(oIMstartedTask) Then
                    oIMstartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/STARTEDoe.png"
                    Me.Resource.setImage(oIMstartedTask, True)
                End If
                If Not IsNothing(oIMnotStartedTask) Then
                    oIMnotStartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/NOTSTARTEDoe.png"
                    Me.Resource.setImage(oIMnotStartedTask, True)
                End If
                If Not IsNothing(oIMsuspendedTask) Then
                    oIMsuspendedTask.ImageUrl = Me.BaseUrl & "images/TaskList/SUSPENDEDoe.png"
                    Me.Resource.setImage(oIMsuspendedTask, True)
                End If

                'Eseguire controllo permessi: Se son almeno manager link ad Editable, se son resource ,link a Read
                If Not IsNothing(oHypModifica) Then
                    oHypModifica.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"

                    Me.Resource.setHyperLink(oHypModifica, True, True)

                    If Me.CurrentPresenter.CanUpdate(oDtoAssignedTasks.Permissions) Then
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString & "&ViewToLoad=" & ViewModeType.TodayTasks.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                    Else
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TodayTasks.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
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
                    oHypTask.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TodayTasks.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oHypProject) Then
                    'oHypProject.Text = e.Item.DataItem.ProjectName.ToString
                    oHypProject.Text = System.Web.HttpUtility.HtmlEncode(e.Item.DataItem.ProjectName.ToString)
                    oHypProject.NavigateUrl = Me.BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & oDtoAssignedTasks.ProjectID.ToString & "&MainPage=" & ViewModeType.TodayTasks.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oLTdeadline) Then
                    If Not IsNothing(e.Item.DataItem.Deadline) Then
                        Dim oDeadline As Date
                        Dim oDate As Date
                        oDate = Date.Now
                        oDeadline = CDate(e.Item.DataItem.Deadline)
                        If oDate < oDeadline Then
                            oLTdeadline.Text = oDeadline.ToString("dd/MM/yy")
                        Else
                            oLTdeadline.Text = "<b><div style='background-color:trasparent;color:#FF0000'>" & oDeadline.ToString("dd/MM/yy") & "</div></b>"
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

        End If
    End Sub

    Public Sub RPTassignedTasksByCommunity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTassignedTasksByCommunity.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAssignedTasksByCommunity As dtoAssignedTasksWithCommunityHeader = e.Item.DataItem
            'Internazionalizzazione
            'Try
            '    Me.Resource.setLabel(e.Item.FindControl("LTcommunityName"))
            'Catch ex As Exception
            'End Try
            Dim LTheader As Literal
            LTheader = e.Item.FindControl("LTheader")
            LTheader.Text = System.Web.HttpUtility.HtmlEncode(oDtoAssignedTasksByCommunity.CommunityName)
            'LTcommunityName.DataBind()
            Dim RPTassignedTasksComponentCommunity As Repeater
            RPTassignedTasksComponentCommunity = e.Item.FindControl("RPTassignedTasksComponentCommunity")
            RPTassignedTasksComponentCommunity.DataSource = oDtoAssignedTasksByCommunity.AssignedTasks
            AddHandler RPTassignedTasksComponentCommunity.ItemDataBound, AddressOf RPTassignedTasksComponentCommunity_ItemDataBound
            RPTassignedTasksComponentCommunity.DataBind()
            'End If
        End If
    End Sub

    Public Sub RPTassignedTasksComponentProject_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAssignedTasks As dtoAssignedTasks = e.Item.DataItem
            If Not IsNothing(oDtoAssignedTasks) Then
                Dim oIMsuspendedTask, oIMstartedTask, oIMnotStartedTask, oIMcompletedTask As System.Web.UI.WebControls.Image
                Dim oHypModifica, oHypTaskByProject As System.Web.UI.WebControls.HyperLink
                Dim oLNBcancellaDefinitivo, oLNBelimina, oLNBundelete As System.Web.UI.WebControls.LinkButton
                Dim oLTdeadline, oLTcompleteness As Literal

                oLNBcancellaDefinitivo = e.Item.FindControl("LNBdelete")
                oLNBelimina = e.Item.FindControl("LNBelimina")
                oLNBundelete = e.Item.FindControl("LNBundelete")

                oIMsuspendedTask = e.Item.FindControl("IMsuspendedTask")
                oIMstartedTask = e.Item.FindControl("IMstartedTask")
                oIMnotStartedTask = e.Item.FindControl("IMnotStartedTask")
                oIMcompletedTask = e.Item.FindControl("IMcompletedTask")

                oHypModifica = e.Item.FindControl("HYPmodifica")
                oHypTaskByProject = e.Item.FindControl("HYPtaskByProject")

                oLTdeadline = e.Item.FindControl("LTdeadline")
                oLTcompleteness = e.Item.FindControl("LTcompleteness")

                oIMsuspendedTask.Visible = (e.Item.DataItem.Status = TaskStatus.suspended)
                oIMstartedTask.Visible = (e.Item.DataItem.Status = TaskStatus.started)
                oIMnotStartedTask.Visible = (e.Item.DataItem.Status = TaskStatus.notStarted)
                oIMcompletedTask.Visible = (e.Item.DataItem.Status = TaskStatus.completed)

                If Not IsNothing(oIMcompletedTask) Then
                    oIMcompletedTask.ImageUrl = Me.BaseUrl & "images/TaskList/completed20.png"
                    Me.Resource.setImage(oIMcompletedTask, True)
                End If
                If Not IsNothing(oIMstartedTask) Then
                    oIMstartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/STARTEDoe.png"
                    Me.Resource.setImage(oIMstartedTask, True)
                End If
                If Not IsNothing(oIMnotStartedTask) Then
                    oIMnotStartedTask.ImageUrl = Me.BaseUrl & "images/TaskList/NOTSTARTEDoe.png"
                    Me.Resource.setImage(oIMnotStartedTask, True)
                End If
                If Not IsNothing(oIMsuspendedTask) Then
                    oIMsuspendedTask.ImageUrl = Me.BaseUrl & "images/TaskList/SUSPENDEDoe.png"
                    Me.Resource.setImage(oIMsuspendedTask, True)
                End If


                If Not IsNothing(oHypModifica) Then
                    oHypModifica.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"
                    Me.Resource.setHyperLink(oHypModifica, True, True) 'oHypModifica.ToolTip = "Modify"
                    If Me.CurrentPresenter.CanUpdate(oDtoAssignedTasks.Permissions) Then
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString & "&ViewToLoad=" & ViewModeType.TodayTasks.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                    Else
                        oHypModifica.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TodayTasks.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
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



                If Not IsNothing(oHypTaskByProject) Then
                    oHypTaskByProject.Text = System.Web.HttpUtility.HtmlEncode(oDtoAssignedTasks.TaskName.ToString)
                    oHypTaskByProject.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoAssignedTasks.TaskId.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&ViewToLoad=" & ViewModeType.TodayTasks.ToString & "&OrderBy=" & Me.CurrentOrderBy.ToString & "&Filter=" & Me.CurrentCommunityFilter.ToString & "&PageSize=" & Me.CurrentPageSize.ToString & "&PageIndex=" & Me.CurrentPageIndex.ToString
                End If

                If Not IsNothing(oLTdeadline) Then
                    If Not IsNothing(e.Item.DataItem.Deadline) Then
                        Dim oDeadline As Date
                        Dim oDate As Date
                        oDate = Date.Now
                        oDeadline = CDate(e.Item.DataItem.Deadline)
                        If oDate < oDeadline Then
                            oLTdeadline.Text = oDeadline.ToString("dd/MM/yy")
                        Else
                            oLTdeadline.Text = "<b><div style='background-color:trasparent;color:#FF0000'>" & oDeadline.ToString("dd/MM/yy") & "</div></b>"
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
            Dim oLTheaderDeadline As Literal = e.Item.FindControl("LTheaderDeadline")
            Dim oLTheaderCompleteness As Literal = e.Item.FindControl("LTheaderCompleteness")

            Try
                Me.Resource.setLiteral(oLTheaderStatus)
                Me.Resource.setLiteral(oLTheaderDelete)
                Me.Resource.setLiteral(oLTheaderModify)
                Me.Resource.setLiteral(oLTheaderTask)
                Me.Resource.setLiteral(oLTheaderCompleteness)
                Me.Resource.setLiteral(oLTheaderDeadline)
            Catch ex As Exception

            End Try


        End If

    End Sub

    Public Sub RPTassignedTasksByProject_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTassignedTasksByProject.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAssignedTasksByProject As dtoAssignedTasksWithProjectHeader = e.Item.DataItem
            'Internazionalizzazione
            'Try
            '    Me.Resource.setLabel(e.Item.FindControl("LTcommunityName"))
            'Catch ex As Exception
            'End Try
            Dim LTheader As Literal
            LTheader = e.Item.FindControl("LTheader")
            LTheader.Text = System.Web.HttpUtility.HtmlEncode(oDtoAssignedTasksByProject.HeaderProject)
            'LTcommunityName.DataBind()
            Dim RPTassignedTasksComponentProject As Repeater
            RPTassignedTasksComponentProject = e.Item.FindControl("RPTassignedTasksComponentProject")
            RPTassignedTasksComponentProject.DataSource = oDtoAssignedTasksByProject.AssignedTasks
            AddHandler RPTassignedTasksComponentProject.ItemDataBound, AddressOf RPTassignedTasksComponentProject_ItemDataBound
            RPTassignedTasksComponentProject.DataBind()
        End If
    End Sub

    Public Sub RPTassignedTasksComponent_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) 'Handles RPTassignedTasksByCommunity.ItemCommand
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



    Public Sub ShowDeletedParentError() Implements IViewAssignedTasks.ShowDeletedParentError
        Me.PageUtility.AddAction(Me.ComunitaCorrenteID, Services_TaskList.ActionType.GenericError, , InteractionType.UserWithLearningObject)
        Dim str As String = "XXX"
        str = Me.Resource.getValue("ltlscript")
        Me.ltlscript.Text = "<script type=""text/javascript"">alert(""" + str + """);</script>"
    End Sub
    'Carica i valori di filtraggio (TaskFilter.cs) nella DDL di asswignedTask.aspx 
    Public Sub LoadFilters(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.TaskFilter)) Implements IViewAssignedTasks.LoadFilters
        Me.DDLfilterBy.Items.Clear()
        For Each oFilter As lm.Comol.Modules.TaskList.Domain.TaskFilter In oList

            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLfilterBy." & oFilter)
            oItem.Value = oFilter '.ToString
            Me.DDLfilterBy.Items.Add(oItem)
        Next
    End Sub

    Public Sub LoadSorts(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.Sorting)) Implements IViewAssignedTasks.LoadSorts
        Me.DDLsortBy.Items.Clear()
        For Each oSort As lm.Comol.Modules.TaskList.Domain.Sorting In oList
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLsortBy." & oSort)
            oItem.Value = oSort
            Me.DDLsortBy.Items.Add(oItem)
        Next
    End Sub

    Public Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType)) Implements IViewAssignedTasks.LoadTaskTabs
        Me.TBStasklist.Tabs.Clear()

        For Each oViewMode As ViewModeType In oList
            Dim oTab As New RadTab()
            oTab.Value = oViewMode
            oTab.Text = Me.Resource.getValue("TBStasklist." & oViewMode) 'oViewMode.ToString
            Me.TBStasklist.Tabs.Add(oTab)

            If oTab.Value = ViewModeType.TaskAdmin Then
                If (From c In Me.CommunitiesPermission Where c.Permissions.Administration).Count > 0 Then
                    oTab.Visible = True
                Else
                    oTab.Visible = False
                End If
            End If

            If oTab.Value = ViewModeType.TodayTasks Then
                oTab = Me.TBStasklist.SelectedTab
            End If
        Next

    End Sub

    '????
    Private Function GetOrderByString(ByVal SortExpression As String) As TaskListOrder
        Dim iResponse As TaskListOrder
        If System.Enum.IsDefined(GetType(TaskListOrder), SortExpression) Then
            iResponse = System.Enum.Parse(GetType(TaskListOrder), SortExpression)
        Else
            iResponse = TaskListOrder.Project
        End If
        Return iResponse
    End Function

    Public Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.NavigationUrl ' ByVal ViewMode As lm.Comol.Modules.TaskList.Domain.ViewModeType
        Me.PGgrid.BaseNavigateUrl = Me.BaseUrl & "TaskList/AssignedTasks.aspx?View=TodayTasks&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page={0}"
        'Me.PGgrid.BaseNavigateUrl = Me.BaseUrl & "TaskList/" & ViewMode.ToString & ".aspx?&OrderBy=" & OrderBy.ToString & "&View=" & ViewMode.ToString & "&QueryString=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page={0}"

    End Sub

    Public Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.SetNavigationUrlToAssignedTask
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TodayTasks)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/AssignedTasks.aspx?View=TodayTasks&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.SetNavigationUrlToProject
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.InvolvingProjects)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/InvolvingProjects.aspx?View=InvolvingProjects&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub
    Public Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal TaskType As lm.Comol.Modules.TaskList.Domain.TaskManagedType, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.SetNavigationUrlToManage
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TasksManagement)

        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&TaskType=Projects" & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
            'oTab.NavigateUrl = Me.BaseUrl & "TaskList/TasksManagement.aspx?View=TaskManagement&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If

    End Sub

    Public Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.SetNavigationUrlToAdministration
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBStasklist.FindTabByValue(ViewModeType.TaskAdmin)
        If Not IsNothing(oTab) Then
            oTab.NavigateUrl = Me.BaseUrl & "TaskList/TaskAdministration.aspx?View=TaskAdmin&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&Sorting=" & SortBy.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
        End If
    End Sub

    Public Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.SetNavigationUrlToAddProject
        Me.HYPaddProject.NavigateUrl = Me.BaseUrl & "TaskList/AddProject.aspx?View=TodayTasks&OrderBy=" & OrderBy.ToString & "&CommunityFilter=" & Filter.ToString & "&PageSize=" & PageSize.ToString & "&Page=0"
    End Sub

    Private Sub DDLorderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfilterBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadAssignedTasks()
    End Sub

    Private Sub DDLsortBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLsortBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadAssignedTasks()
    End Sub
    Private Sub RBLview_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLview.SelectedIndexChanged
        Me.CurrentPresenter.LoadAssignedTasks()
    End Sub

    Private Sub RPTassignedTasksByProject_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles RPTassignedTasksByProject.Load

    End Sub

    Public Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAssignedTasks.GoToReallocateResource
        Me.PageUtility.RedirectToUrl("/TaskList/ReallocateUsers.aspx?CurrentTaskID=" & TaskID & "&CurrentModeType=" & ReallocateType.ToString & "&PreviusPage=" & IViewReallocateUsers.PreviusPageName.AssignedTasks.ToString)
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub

End Class