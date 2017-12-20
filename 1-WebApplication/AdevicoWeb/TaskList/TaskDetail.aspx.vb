Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
'Imports lm.Comol.Modules.TaskList


Partial Public Class TaskDetail
    Inherits PageBase
    Implements IViewTaskDetail


    Private _presenter As TaskDetailPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList
    Private _Service As lm.Comol.Modules.TaskList.ServiceTaskList
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))
    Private _CommunitiesPermissionCS As List(Of ModuleCommunityPermission(Of lm.Comol.Modules.TaskList.ModuleTasklist))


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region " Base"

    Public ReadOnly Property Service As lm.Comol.Modules.TaskList.ServiceTaskList
        Get
            If _Service Is Nothing Then
                _Service = New lm.Comol.Modules.TaskList.ServiceTaskList(Me.CurrentPresenter.AppContext)
            End If
            Return _Service
        End Get
    End Property

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
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
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_TaskDetail", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setHyperLink(Me.HYPreturnToTaskList, True, True)
            .setHyperLink(Me.HypFileManagement, True, True)
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

    Public ReadOnly Property CurrentPresenter() As TaskDetailPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TaskDetailPresenter(Me.CurrentContext, Me)
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
    Public Sub ShowError(ByVal ErrorString As String) Implements IViewTaskDetail.ShowError
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.HYPreturnToTaskList.NavigateUrl = Me.BaseUrl & Me.BackUrl
        Me.LBerror.Text = ErrorString
        Me.MLVtaskDetail.SetActiveView(Me.VIWerror)
    End Sub

    Public Sub InitViewReadOnly(ByVal TaskDetailWithPermission As dtoTaskDetailWithPermission) Implements IViewTaskDetail.InitViewReadOnly ', ByVal task As Task, ByVal moduleTask As CoreItemPermission) 
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.StartViewTaskDetail, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Task, Me.CurrentTaskID), InteractionType.UserWithLearningObject)
        Me.CTRLdetail.CurrentPresenter.InitView(IViewUC_TaskDetail.viewDetailType.Read, TaskDetailWithPermission, Me.BaseUrl & Me.BackUrl, Me.ViewToLoad, Me.CurrentViewDetailType)
        Me.CTRLuser.CurrentPresenter.InitView(Me.CurrentTaskID, Me.TaskPermission, IViewUC_AssignUsers_new.viewMode.Read)
        Me.MLVtaskDetail.SetActiveView(Me.VIWtaskDetail)

        Me.HypFileManagement.NavigateUrl = Me.BaseUrl + "TaskList/ManagementTaskFile.aspx?TaskID=" + Me.CurrentTaskID.ToString()

    End Sub

    Public Sub InitViewEditable(ByVal TaskDetailWithPermission As dtoTaskDetailWithPermission) Implements IViewTaskDetail.InitViewEditable
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.StartUpdateTaskDetail, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Task, Me.CurrentTaskID), InteractionType.UserWithLearningObject)
        If (Me.TaskPermission And TaskPermissionEnum.TaskSetCategory) = TaskPermissionEnum.TaskSetCategory Then
            Me.CTRLdetail.CurrentPresenter.InitView(IViewUC_TaskDetail.viewDetailType.Update, TaskDetailWithPermission, Me.BaseUrl & Me.BackUrl, Me.ViewToLoad, Me.CurrentViewDetailType)
        Else
            Me.CTRLdetail.CurrentPresenter.InitView(IViewUC_TaskDetail.viewDetailType.Read, TaskDetailWithPermission, Me.BaseUrl & Me.BackUrl, Me.ViewToLoad, Me.CurrentViewDetailType)
        End If
        If (Me.TaskPermission And TaskPermissionEnum.ManagementUser) = TaskPermissionEnum.ManagementUser Then
            Me.CTRLuser.CurrentPresenter.InitView(Me.CurrentTaskID, Me.TaskPermission, IViewUC_AssignUsers_new.viewMode.Edit)
        Else
            Me.CTRLuser.CurrentPresenter.InitView(Me.CurrentTaskID, Me.TaskPermission, IViewUC_AssignUsers_new.viewMode.Read)
        End If

        Dim files As IList(Of iCoreItemFileLink(Of Long)) = Service.GetTaskFiles(Me.CurrentPresenter.CurrentTaskManager.GetTask(TaskDetailWithPermission.dtoTaskDetail.TaskID), True)

        Me.MLVtaskDetail.SetActiveView(Me.VIWtaskDetail)

        Me.HypFileManagement.NavigateUrl = Me.BaseUrl + "TaskList/ManagementTaskFile.aspx?TaskID=" + Me.CurrentTaskID.ToString()

    End Sub

    Public Sub UpdateTask(ByVal oTask As Task) Handles CTRLdetail.BTN_SaveTaskClicked
        Me.CurrentPresenter.UpdateTask(oTask)
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.FinishUpdateTaskDetail, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Task, Me.CurrentTaskID), InteractionType.UserWithLearningObject)
        Me.PageUtility.RedirectToUrl(Me.BackUrl)
    End Sub

    Private Sub ReloadPage() Handles CTRLuser.ReloadAllPage, CTRLdetail.ReloadPage
        If Me.CurrentViewDetailType = IViewTaskDetail.viewDetailType.Editable Then
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.FinishUpdateTaskDetail, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Task, Me.CurrentTaskID), InteractionType.UserWithLearningObject)
        Else
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.FinishViewTaskDetail, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Task, Me.CurrentTaskID), InteractionType.UserWithLearningObject)
        End If

        If Me.CurrentPresenter.CanReloadCurrentPage Then
            Me.PageUtility.RedirectToUrl("TaskList/TaskDetail.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&ViewToLoad=" & Me.ViewToLoad.ToString & "&OrderBy=" & Me.OrderBy.ToString & "&Filter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&PageIndex=" & Me.PageIndex & "&CurrentViewType=" & Me.CurrentViewDetailType.ToString & "&Sorting=" & Me.SortOfTask.ToString)
        Else
            Me.PageUtility.RedirectToUrl(Me.BaseUrl & Me.BackUrl)
        End If

    End Sub

#End Region

#Region "IViewProperty"

    Public ReadOnly Property BackUrl() As String Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskDetail.BackUrl

        Get
            Dim Url As String = ""
            Select Case ViewToLoad
                Case ViewModeType.TodayTasks
                    Url = "TaskList/AssignedTasks.aspx?View=" & Me.ViewToLoad.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&Sorting=" & Me.SortOfTask.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                Case ViewModeType.TasksManagement
                    Url = "TaskList/TasksManagement.aspx?View=" & Me.ViewToLoad.ToString & "&TaskType=" & Me.TypeOfTask.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&Sorting=" & Me.SortOfTask.ToString & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                Case ViewModeType.InvolvingProjects
                    Url = "TaskList/InvolvingProjects.aspx?View=" & Me.ViewToLoad.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&Sorting=" & Me.SortOfTask.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                Case ViewModeType.TaskAdmin
                    Url = "TaskList/TaskAdministration.aspx?View=" & Me.ViewToLoad.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&Sorting=" & Me.SortOfTask.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                Case ViewModeType.TaskMap
                    Url = "TaskList/TasksMap.aspx?View=" & Me.ViewToLoad.ToString & "&CurrentTaskID=" & Me.CurrentTaskID.ToString
                    'hhp://localhost/Comol_Elle3/TaskList/TasksMap.aspx?CurrentTaskID=2643&MainPage=TodayTasks&OrderBy=Community&Filter=CurrentCommunity&PageSize=50&PageIndex=0
                Case Else
                    Url = "TaskList/AssignedTasks.aspx?View=" & Me.ViewToLoad.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                    ' Url = "TaskList/AssignedTasks.aspx?View=" & Me.ViewToLoad.TodayTasks.ToString & "&CommunityFilter=" & Me.Filter.AllCommunities.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.Community.ToString
            End Select
            Return Url
        End Get
    End Property

    Public ReadOnly Property OrderBy() As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskDetail.OrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TasksPageOrderBy).GetByString(Request.QueryString("OrderBy"), TasksPageOrderBy.None)
        End Get
    End Property

    Public ReadOnly Property Filter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements IViewTaskDetail.Filter
        Get

            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskFilter).GetByString(Request.QueryString("Filter"), TaskFilter.AllCommunities)

        End Get
    End Property

    Public ReadOnly Property ViewToLoad() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements IViewTaskDetail.ViewToLoad
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("ViewToLoad"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public ReadOnly Property PageIndex() As Integer Implements IViewTaskDetail.PageIndex
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

    Public ReadOnly Property PageSize() As Integer Implements IViewTaskDetail.PageSize
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

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) Implements IViewTaskDetail.CommunitiesPermission
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

    Public ReadOnly Property CommunitiesPermissionCS() As IList(Of ModuleCommunityPermission(Of lm.Comol.Modules.TaskList.ModuleTasklist)) Implements IViewTaskDetail.CommunitiesPermissionCS
        Get
            If IsNothing(_CommunitiesPermissionCS) Then
                _CommunitiesPermissionCS = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, lm.Comol.Modules.TaskList.ModuleTasklist.UniqueID) _
                                          Select New ModuleCommunityPermission(Of lm.Comol.Modules.TaskList.ModuleTasklist)() With {.ID = sb.CommunityID, .Permissions = New lm.Comol.Modules.TaskList.ModuleTasklist(sb.PermissionLong)}).ToList
            End If
            Return _CommunitiesPermissionCS
        End Get
    End Property

    Public Function RepositoryPermission(ByVal CommunityID As Integer) As CoreModuleRepository Implements IViewTaskDetail.RepositoryPermission
        Dim oModule As CoreModuleRepository = Nothing
        If CommunityID = 0 Then
            oModule = CoreModuleRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, CoreModuleRepository.UniqueID) _
                  Where sb.CommunityID = CommunityID Select New CoreModuleRepository(sb.PermissionString)).FirstOrDefault
            If IsNothing(oModule) Then
                oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, CoreModuleRepository.UniqueID, True) _
                 Where sb.CommunityID = CommunityID Select New CoreModuleRepository(sb.PermissionString)).FirstOrDefault
                If IsNothing(oModule) Then
                    oModule = New CoreModuleRepository
                End If
            End If
        End If
        Return oModule
    End Function

    Public Property CurrentCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskDetail.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property

    Public ReadOnly Property SortOfTask() As lm.Comol.Modules.TaskList.Domain.Sorting Implements IViewTaskDetail.SortOfTask
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of Sorting).GetByString(Request.QueryString("Sorting"), Sorting.None)
        End Get
    End Property

    Public ReadOnly Property TypeOfTask() As lm.Comol.Modules.TaskList.Domain.TaskManagedType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewTaskDetail.TypeOfTask
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskManagedType).GetByString(Request.QueryString("TypeOfTask"), TaskManagedType.None)
        End Get
    End Property
    Public ReadOnly Property CurrentViewDetailType() As IViewTaskDetail.viewDetailType Implements IViewTaskDetail.CurrentViewDetailType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewTaskDetail.viewDetailType).GetByString(Request.QueryString("CurrentViewType"), IViewTaskDetail.viewDetailType.Read)
        End Get
    End Property
    Public ReadOnly Property CurrentTaskID() As Long Implements IViewTaskDetail.CurrentTaskID
        Get
            Try
                Return CInt(Me.Request.QueryString("CurrentTaskID"))
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property
    Public Property TaskPermission() As TaskPermissionEnum Implements IViewTaskDetail.TaskPermission
        Get
            Return Me.ViewState("TaskPermission")
        End Get
        Set(ByVal value As TaskPermissionEnum)
            Me.ViewState("TaskPermission") = value
        End Set
    End Property

    'Gestione Files
    Public Sub LoadFilesToManage(ByVal ItemID As Long, ByVal oPermission As CoreItemPermission, ByVal files As IList(Of iCoreItemFileLink(Of Long)), ByVal urlToPublish As String) Implements IViewTaskDetail.LoadFilesToManage
        Me.CTRLtaskManagementFile.Visible = True
        Me.CTRLtaskManagementFile.ShowManagementButtons = False
        Me.CTRLtaskManagementFile.InitalizeControl(ItemID, oPermission, files, urlToPublish)
    End Sub

#End Region

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub
End Class