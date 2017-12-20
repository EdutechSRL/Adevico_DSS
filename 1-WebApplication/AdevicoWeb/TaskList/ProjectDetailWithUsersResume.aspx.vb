Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports System.Text.RegularExpressions

Public Class ProjectDetailWithUsersResume
    Inherits PageBase
    Implements iViewProjectDetailWithUsersResume


    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))
    Private _presenter As ProjectDetailWithUsersResumePresenter
    'Private _Servizio As Services_TaskList


    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property


    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) 'Implements iViewProjectDetailWithUsersResume.CommunitiesPermission
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

    Public ReadOnly Property CurrentPresenter() As ProjectDetailWithUsersResumePresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New ProjectDetailWithUsersResumePresenter(Me.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property

    Public Sub InitView(ByVal TaskID As Long, ByVal ProjectName As String) Implements iViewProjectDetailWithUsersResume.InitView
        Me.HYPdetail.NavigateUrl = Me.BaseUrl & Me.BackUrl
        Me.HYPreturn.NavigateUrl = "~" & Me.DetailUrl
        Me.LBtitoloSuperiore.Text = StripHTML(ProjectName)
        Me.CTRLinvolvedUsersDetail.CurrentPresenter.InitView(Me.CurrentProjectID) ', Me.TaskPermission, Me.CurrentViewToLoad)
        Me.MLVprojectUsersTable.SetActiveView(Me.VIWusersTablePerProject)
    End Sub

    Public Shared Function StripHTML(ByVal strHTML As String) As String
        Dim objRegExp As New Regex("<(.|\n)+?>", RegexOptions.IgnoreCase)
        Dim strOutput As String

        strOutput = objRegExp.Replace(strHTML, "")

        Return strOutput
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Public ReadOnly Property CurrentProjectID As Long Implements iViewProjectDetailWithUsersResume.CurrentProjectID
        Get
            Return Request.QueryString("ProjectID")
        End Get
    End Property

    Public ReadOnly Property ViewModeType() As ViewModeType Implements iViewProjectDetailWithUsersResume.ViewModeType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("ViewToLoad"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public Property TaskPermission As TaskPermissionEnum Implements iViewProjectDetailWithUsersResume.TaskPermission
        Get
            Return Me.ViewState("TasPermission")
        End Get
        Set(ByVal value As TaskPermissionEnum)
            Me.ViewState("TaskPermission") = value
        End Set
    End Property

    Public ReadOnly Property CurrentViewToLoad As ViewModeType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("ViewToLoad"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public Sub ShowError(ByVal ErrorString As String) Implements iViewProjectDetailWithUsersResume.ShowError
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.LBerror.Text = ErrorString
        Me.HYPreturnError.NavigateUrl = Me.BaseUrl & Me.BackUrl
        Me.MLVprojectUsersTable.SetActiveView(Me.VIWerror)
    End Sub

    Public ReadOnly Property BackUrl() As String Implements lm.Comol.Modules.Base.Presentation.TaskList.iViewProjectDetailWithUsersResume.BackUrl
        Get
            Return "TaskList/TaskDetail.aspx?CurrentTaskID=" & Me.CurrentProjectID.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString
        End Get
    End Property

    Public ReadOnly Property DetailUrl As String Implements lm.Comol.Modules.Base.Presentation.TaskList.iViewProjectDetailWithUsersResume.DetailUrl
        Get
            If Me.CurrentViewToLoad = lm.Comol.Modules.TaskList.Domain.ViewModeType.TasksManagement Then
                Return "/TaskList/TasksManagement.aspx?View=TasksManagement&OrderBy=AllActive&CommunityFilter=AllCommunities&TaskType=Projects&Sorting=DeadlineOrder&PageSize=50&Page=0"
            ElseIf Me.CurrentViewToLoad = lm.Comol.Modules.TaskList.Domain.ViewModeType.TaskAdmin Then
                Return "TaskList/TaskAdministration.aspx?View=TaskAdmin&OrderBy=AllActive&CommunityFilter=CurrentCommunity&Sorting=DeadlineOrder&PageSize=50&Page=0"
            End If

        End Get
    End Property


#Region "Base"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Not IsPostBack Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectDetailWithUsersResume", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setHyperLink(Me.HYPreturn, True, True)
            .setHyperLink(Me.HYPdetail, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region


End Class