Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel

Partial Public Class TasksMap
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap


    Private _presenter As lm.Comol.Modules.Base.Presentation.TaskList.TasksMapPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region " Base"


    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub BindDati()

        Me.Master.ShowNoPermission = False
        If Not IsPostBack Then
            Me.CurrentPresenter.InitView()
        Else
            Me.CurrentPresenter.ReloadInfo()
        End If
        If Me.MainPage = ViewModeType.TaskAdmin Then
            Me.IsAdminMode = True
        End If
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
        MyBase.SetCulture("pg_UC_GeneralMap", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setHyperLink(HYPaddSubTask, True, True)
            .setHyperLink(HYPgantt, True, True)
            .setHyperLink(HYPreturnToTaskList, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property

    Private ReadOnly Property CurrentService() As Services_TaskList
        Get
            If IsNothing(_Servizio) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _Servizio = Services_TaskList.Create
                    With _Servizio
                        .AddCommunityProject = False
                        .AddPersonalProject = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                        .Administration = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ViewCommunityProjects = (PersonTypeID <> Main.TipoPersonaStandard.Guest)

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

    Private Function TranslateComolPermissionToModulePermission(ByVal oService As Services_TaskList) As ModuleTaskList
        Dim oModulePermission As New ModuleTaskList
        With oService
            'oModulePermission.DeleteMessage = .Admin OrElse .Write
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

    Public ReadOnly Property CurrentPresenter() As TasksMapPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TasksMapPresenter(Me.CurrentContext, Me)
            End If
            Return _presenter
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

#End Region


#Region "IView Function and Sub"
    Public Sub ShowError(ByVal ErrorString As String) Implements IViewTaskMap.ShowError

        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.HYPaddSubTask.Visible = False
        Me.HYPmap.Visible = False
        Me.HYPgantt.Visible = False
        Dim Url As String = ""
        Select Case MainPage
            Case ViewModeType.TodayTasks
                Url = BaseUrl & "TaskList/AssignedTasks.aspx?View=" & Me.MainPage.ToString & "&CommunityFilter=AllCommunities" & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
            Case ViewModeType.TasksManagement
                Url = BaseUrl & "TaskList/TasksManagement.aspx?View=" & Me.MainPage.ToString & "&TaskType=" & Me.TypeOfTask.ToString & "&CommunityFilter=AllCommunities" & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
            Case ViewModeType.InvolvingProjects
                Url = BaseUrl & "TaskList/InvolvingProjects.aspx?View=" & Me.MainPage.ToString & "&CommunityFilter=AllCommunities" & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
            Case Else
                Url = BaseUrl & "TaskList/AssignedTasks.aspx?View=" & Me.MainPage.ToString & "&CommunityFilter=AllCommunities" & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                ' Url = BaseUrl & "TaskList/AssignedTasks.aspx?View=" & Me.ViewToLoad.TodayTasks.ToString & "&CommunityFilter=" & Me.Filter.AllCommunities.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.Community.ToString
        End Select
        Me.HYPreturnToTaskList.NavigateUrl = Url
        Me.LBerror.Text = ErrorString
        Me.MLVtaskDetail.SetActiveView(Me.VIWerror)
    End Sub


    Private Sub InitMap() Implements IViewTaskMap.InitMap
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.StartViewProjectMap, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Project, Me.CurrentProjectID), InteractionType.UserWithLearningObject)
        Me.CTRLmap.CurrentPresenter.InitView(Me.CurrentProjectID, Me.TaskPermission, Me.MainPage)
        Me.MLVtaskDetail.SetActiveView(Me.VIWmap)
        Dim oList As IList = Me.CTRLmap.CurrentPresenter.GetTasks()
        If oList.Count > 2 Then
            Me.HYPmap.Visible = True
        Else
            Me.HYPmap.Visible = False
        End If
        'Aggiunta controllo del TaskMap
        If Me.MainPage = ViewModeType.TaskAdmin Then
            Me.IsAdminMode = False
        End If
    End Sub

    Private Sub SetIfCanAddChildrenTask(ByVal CanAddChildren As Boolean) Handles CTRLmap.GetCanAddSubTask
        Me.HYPaddSubTask.Visible = CanAddChildren
    End Sub


    Private Sub InitSwichMap() Implements IViewTaskMap.InitSwichMap
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.StartViewProjectMap, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Project, Me.CurrentProjectID), InteractionType.UserWithLearningObject)
        Me.HYPaddSubTask.Visible = False
        Me.CTRLswichTask.CurrentPresenter.Init(Me.CurrentProjectID)
        Me.MLVtaskDetail.SetActiveView(Me.VIWswichMap)
    End Sub

    Public Sub SetTaskName(ByVal TaskName As String) Implements IViewTaskMap.SetTaskName
        Me.LBtitolo.Text = System.Web.HttpUtility.HtmlEncode(TaskName)
    End Sub

#End Region

#Region "IViewProperty"

    'Nuova variabile per gestire l accesso in modalita admin
    Public Property IsAdminMode As Boolean Implements IViewTaskMap.IsAdminMode
        Get
            If IsNothing(Me.Session("IsAdminMode")) Then
                Me.Session("IsAdminMode") = False
            End If
            Return Me.Session("IsAdminMode")
        End Get
        Set(ByVal value As Boolean)
            Me.Session("IsAdminMode") = value
        End Set
    End Property


    Public ReadOnly Property CurrentMapType() As lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.viewMapType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.CurrentMapType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewTaskMap.viewMapType).GetByString(Request.QueryString("CurrentMapType"), IViewTaskMap.viewMapType.ClassicMap)
        End Get
    End Property

    Public ReadOnly Property DetailType() As lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskDetail.viewDetailType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.DetailType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewTaskDetail.viewDetailType).GetByString(Request.QueryString("DetailType"), IViewTaskDetail.viewDetailType.Read)
        End Get
    End Property

    Public ReadOnly Property OrderBy() As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.OrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TasksPageOrderBy).GetByString(Request.QueryString("OrderBy"), TasksPageOrderBy.None)
        End Get
    End Property

    Public ReadOnly Property Filter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements IViewTaskMap.Filter
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskFilter).GetByString(Request.QueryString("Filter"), TaskFilter.AllCommunities)
        End Get
    End Property

    Public ReadOnly Property MainPage() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements IViewTaskMap.MainPage
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("MainPage"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public ReadOnly Property PageIndex() As Integer Implements IViewTaskMap.PageIndex
        Get
            If Me.Request.QueryString("PageIndex") Is Nothing Then
                Return 0
            Else
                Try
                    Return CInt(Me.Request.QueryString("PageIndex"))
                Catch ex As Exception
                    Return 0
                End Try
            End If
        End Get
    End Property

    Public ReadOnly Property PageSize() As Integer Implements IViewTaskMap.PageSize
        Get
            Dim Size As Integer = 50
            Try
                Size = Request.QueryString("PageSize")
            Catch ex As Exception

            End Try
            If IsNothing(Request.QueryString("PageSize")) Then
                Return 50

            Else

                Return Size

            End If
        End Get
    End Property

    Public ReadOnly Property TypeOfTask() As lm.Comol.Modules.TaskList.Domain.TaskManagedType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.TypeOfTask
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskManagedType).GetByString(Request.QueryString("TypeOfTask"), TaskManagedType.None) '.ToString()
        End Get
    End Property




    Public ReadOnly Property CurrentTaskID() As Long Implements IViewTaskMap.CurrentTaskID
        Get
            Try
                Return CInt(Me.Request.QueryString("CurrentTaskID"))
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property


    Public Property TaskPermission() As TaskPermissionEnum Implements IViewTaskMap.TaskPermission
        Get
            Return Me.ViewState("TaskPermission")
        End Get
        Set(ByVal value As TaskPermissionEnum)
            Me.ViewState("TaskPermission") = value
        End Set
    End Property

    Public Property CurrentProjectID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.CurrentProjectID
        Get
            Return Me.ViewState("CurrentProjectID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentProjectID") = value
        End Set
    End Property

#End Region
    Public Property CurrentCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property


    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) Implements IViewTaskMap.CommunitiesPermission
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


    Private Function GetMainPageUrl() As String
        Dim Url As String = ""
        Select Case MainPage
            Case ViewModeType.TodayTasks
                Url = "TaskList/AssignedTasks.aspx?View=" & Me.MainPage.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
            Case ViewModeType.TasksManagement
                Url = "TaskList/TasksManagement.aspx?View=" & Me.MainPage.ToString & "&TaskType=" & Me.TypeOfTask.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
            Case ViewModeType.InvolvingProjects
                Url = "TaskList/InvolvingProjects.aspx?View=" & Me.MainPage.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
            Case Else
                Url = "TaskList/AssignedTasks.aspx?View=" & Me.MainPage.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                ' Url = BaseUrl & "TaskList/AssignedTasks.aspx?View=" & Me.ViewToLoad.TodayTasks.ToString & "&CommunityFilter=" & Me.Filter.AllCommunities.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.Community.ToString
        End Select
        Return Url
    End Function

    Public Sub InitHyperlinkUrl(ByVal CanSwichTask As Boolean) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskMap.InitHyperlinkUrl
        Me.HYPaddSubTask.NavigateUrl = BaseUrl & "TaskList/AddTask.aspx?CurrentTaskID=" & Me.CurrentProjectID & "&PreviusPage=" & IViewAddTask.PreviusPage.TaskMap.ToString
        Me.HYPreturnToTaskList.NavigateUrl = BaseUrl & GetMainPageUrl()

        If Me.CurrentMapType = IViewTaskMap.viewMapType.ClassicMap Then
            Me.HYPmap.NavigateUrl = BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.CurrentTaskID.ToString & "&CurrentViewType=" & Me.DetailType.ToString & "&ViewToLoad=" & Me.MainPage.ToString & "&OrderBy=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize.ToString & "&PageIndex=" & Me.PageIndex.ToString & "&CurrentMapType=" & IViewTaskMap.viewMapType.SwichMap.ToString & "&OrderBy=" & Me.OrderBy.ToString
            Me.HYPmap.Text = Me.Resource.getValue("HYPswichTask.text")
            Me.HYPmap.ToolTip = Me.Resource.getValue("HYPswichTask.Tooltip")
            Me.HYPmap.Visible = CanSwichTask
            Me.HYPgantt.NavigateUrl = BaseUrl & "TaskList/Gantt.aspx?TaskID=" & Me.CurrentTaskID & "&PreviusPage=" & IviewGantt.PageType.GeneralMap.ToString
        Else
            Me.HYPmap.NavigateUrl = BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.CurrentTaskID.ToString & "&CurrentViewType=" & Me.DetailType.ToString & "&ViewToLoad=" & Me.MainPage.ToString & "&OrderBy=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize.ToString & "&PageIndex=" & Me.PageIndex.ToString & "&CurrentMapType=" & IViewTaskMap.viewMapType.ClassicMap.ToString & "&OrderBy=" & Me.OrderBy.ToString
            Me.HYPmap.Text = Me.Resource.getValue("HYPtaskMap.text")
            Me.HYPmap.ToolTip = Me.Resource.getValue("HYPtaskMap.Tooltip")
            Me.HYPgantt.NavigateUrl = BaseUrl & "TaskList/Gantt.aspx?TaskID=" & Me.CurrentTaskID & "&PreviusPage=" & IviewGantt.PageType.SwichMap.ToString

        End If
    End Sub

    Public Sub GoToMainPage() Handles CTRLmap.LoadMainPage
        Me.PageUtility.RedirectToUrl(GetMainPageUrl)
    End Sub


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub

End Class