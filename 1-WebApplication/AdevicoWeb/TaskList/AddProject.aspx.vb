Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.Comunita

Partial Public Class AddProject
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject


    Private _presenter As AddProjectPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList
    Private _BaseUrl As String
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))


#Region " Base"


    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub BindDati()

        Me.Master.ShowNoPermission = False
        If Not IsPostBack Then
            Me.MLVaddProject.SetActiveView(Me.VIWaddProject)
            Me.PageUtility.AddAction(Services_TaskList.ActionType.StartAddProject, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Project, "-1"), InteractionType.UserWithLearningObject)
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
        MyBase.SetCulture("pg_AddProject", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")

            .setLabel(LBtitoloSuperiore)
            .setLabel(LBprojectType)
            .setLabel(LBselectCommunity)
            .setLabel(LBprojectProperties)
            Dim oButton As Button
            oButton = Me.WZRtaskProject.FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRtaskProject.FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRtaskProject.FindControl("StepNavigationTemplateContainerID").FindControl("PreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRtaskProject.FindControl("StepNavigationTemplateContainerID").FindControl("BTNnext")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRtaskProject.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRtaskProject.FindControl("FinishNavigationTemplateContainerID").FindControl("PreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRtaskProject.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get

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
            'Tia
            oModulePermission.Administration = .Administration
            oModulePermission.CreateCommunityProject = .AddCommunityProject OrElse .Administration
            'oModulePermission.CreatePersonalCommunityProject = True
            oModulePermission.CreatePersonalProject = True
            oModulePermission.DownloadAllowed = True
            oModulePermission.ManagementPermission = .ManagementPermission OrElse .Administration
            oModulePermission.PrintTaskList = True
            oModulePermission.ViewTaskList = .ViewCommunityProjects

            
        End With
        Return oModulePermission
    End Function

    Public ReadOnly Property CurrentPresenter() As AddProjectPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New AddProjectPresenter(Me.CurrentContext, Me)
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



#Region "Iview Property"
    Public ReadOnly Property OrderBy() As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.OrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TasksPageOrderBy).GetByString(Request.QueryString("OrderBy"), TasksPageOrderBy.None)
        End Get
    End Property

    Public Property CurrentCommunityID() As Integer Implements IViewAddProject.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property

    Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IViewAddProject.ModulePermission
        Get
            Return TranslateComolPermissionToModulePermission(Me.CurrentService)
        End Get
    End Property



    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) Implements IViewAddProject.CommunitiesPermission
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


    Public Property isPersonal() As Boolean Implements IViewAddProject.isPersonal
        Get
            Return Me.ViewState("isPersonal")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isPersonal") = value
        End Set
    End Property

    Public Property isPortal() As Boolean Implements IViewAddProject.isPortal
        Get
            Return Me.ViewState("isPortal")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isPortal") = value
        End Set
    End Property

    Public ReadOnly Property Filter() As lm.Comol.Modules.TaskList.Domain.TaskFilter Implements IViewAddProject.Filter
        Get

            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.TaskList.Domain.TaskFilter).GetByString(Request.QueryString("Filter"), TaskFilter.AllCommunities)

        End Get
    End Property

    Public ReadOnly Property ViewToLoad() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements IViewAddProject.ViewToLoad
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public ReadOnly Property PageIndex() As Integer Implements IViewAddProject.PageIndex
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

    Public ReadOnly Property PageSize() As Integer Implements IViewAddProject.PageSize
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

    Public ReadOnly Property TypeOfTask() As lm.Comol.Modules.TaskList.Domain.TaskManagedType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.TypeOfTask
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskManagedType).GetByString(Request.QueryString("TypeOfTask"), TaskManagedType.None)
        End Get
    End Property

    Public ReadOnly Property BackUrl() As String Implements IViewAddProject.BackUrl
        Get
            Dim Url As String = ""
            Select Case ViewToLoad.ToString
                Case ViewModeType.TodayTasks.ToString
                    Url = "TaskList/AssignedTasks.aspx?View=" & Me.ViewToLoad.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                Case ViewModeType.TasksManagement.ToString
                    Url = "TaskList/TasksManagement.aspx?View=" & Me.ViewToLoad.ToString & "TaskType=" & Me.TypeOfTask.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                Case ViewModeType.InvolvingProjects.ToString
                    Url = "TaskList/InvolvingProjects.aspx?View=" & Me.ViewToLoad.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
                Case Else
                    Url = "TaskList/AssignedTasks.aspx?View=" & Me.ViewToLoad.ToString & "&CommunityFilter=" & Me.Filter.ToString & "&PageSize=" & Me.PageSize & "&Page=" & Me.PageIndex & "&OrderBy=" & Me.OrderBy.ToString
            End Select
            Return Url
        End Get
    End Property


    Public Property CurrentStep() As lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.StepType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.CurrentStep
        Get
            Return Me.ViewState("CurrentStep")
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.StepType)
            Me.ViewState("CurrentStep") = value
        End Set
    End Property


    Public Property SessionUniqueKey() As System.Guid Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.SessionUniqueKey
        Get
            If Not TypeOf Me.ViewState("SessionUniqueKey") Is System.Guid Then
                Me.ViewState("SessionUniqueKey") = System.Guid.Empty
            End If
            Return Me.ViewState("SessionUniqueKey")
        End Get
        Set(ByVal value As System.Guid)
            Me.ViewState("SessionUniqueKey") = value
        End Set
    End Property

    Public Property dtoProject() As lm.Comol.Modules.TaskList.Domain.dtoTaskDetail Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.dtoProject
        Get
            Dim dtoTemp As dtoTaskDetail = Me.Session("dtoProject_" & SessionUniqueKey.ToString)
            If IsNothing(dtoTemp) Then
                dtoTemp = New dtoTaskDetail()
                Me.Session("dtoProject_" & SessionUniqueKey.ToString) = dtoTemp
            End If
            Return dtoTemp
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.dtoTaskDetail)
            Me.Session("dtoProject_" & SessionUniqueKey.ToString) = value
        End Set
    End Property

#End Region


#Region "Iview metodi"
    Public Sub ClearUniqueKey() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.ClearUniqueKey
        Me.Session("dtoProject_" & Me.SessionUniqueKey.ToString) = Nothing
    End Sub

    'PAGINA DI SELEZIONE TIPO DI PROJECT
    Public Sub InitSelectProjectType(ByVal oList As List(Of IViewAddProject.viewTaskListType)) Implements IViewAddProject.InitSelectProjectType
        Me.CTRLprojectType.DefineSelectableTypes(oList)
        Me.WZRtaskProject.ActiveStepIndex = IViewAddProject.StepType.SelectType

    End Sub

    Public Sub InitSetProjectProperty(ByVal TaskDetailWithPermission As dtoTaskDetailWithPermission, ByVal ViewDetailType As IViewUC_TaskDetail.viewDetailType, ByVal BackUrl As String) Implements IViewAddProject.InitSetProjectProperty
        Me.CTRLdetail.CurrentPresenter.InitView(ViewDetailType, TaskDetailWithPermission, BackUrl, Me.ViewToLoad, IViewTaskDetail.viewDetailType.None)
        Me.WZRtaskProject.ActiveStepIndex = IViewAddProject.StepType.SetProperty

    End Sub

    Private Sub CTRLprojectType_TypeSelected(ByVal oType As IViewAddProject.viewTaskListType) Handles CTRLprojectType.TypeSelected
        Me.CurrentPresenter.SetProjectType(oType)
    End Sub

    Public Sub ShowError(ByVal ErrorString As String) Implements IViewAddProject.ShowError
        Me.LBNerror.Text = ErrorString
        Me.MLVaddProject.SetActiveView(Me.VIWerror)
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    'Preso da ScormStatistiche Mirco
    Public ReadOnly Property Servizio() As UCServices.Services_TaskList
        Get
            If IsNothing(_Servizio) Then
                If Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_TaskList(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_File.Codex))
                Else
                    _Servizio = MyBase.ElencoServizi.Find(Services_File.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = New Services_TaskList("00000000000000000000000000000000")
                    End If
                End If
            End If
            Servizio = _Servizio
        End Get
    End Property

    Public Sub InitCommunitySelection(ByVal oListCommunitiesID As System.Collections.Generic.List(Of Integer)) Implements IViewAddProject.InitCommunitySelection
        Dim oService As Services_TaskList = Services_TaskList.Create()  'temporaneo
        oService.AddPersonalProject = True

        Dim oServiceBase As New ServiceBase(0, oService.Codex, oService.PermessiAssociati)
        Dim oClause As New GenericClause(Of ServiceClause)
        oClause.OperatorForNextClause = OperatorType.OrCondition
        oClause.Clause = New ServiceClause(oServiceBase, OperatorType.OrCondition)

        Me.CTRLcommunity.ServiceClauses = oClause
        Me.CTRLcommunity.SelectionMode = ListSelectionMode.Single
        Me.CTRLcommunity.BindDati()
        Me.WZRtaskProject.ActiveStepIndex = IViewAddProject.StepType.SelectCommunity
    End Sub

    Public Function GetProject() As lm.Comol.Modules.TaskList.Domain.Task Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.GetProject
        Return Me.CTRLdetail.GetTask
    End Function
#End Region

    Private Sub WZRtaskProject_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRtaskProject.CancelButtonClick
        Me.CurrentPresenter.ReturnToMainPage()
    End Sub

    Private Sub WZRtaskProject_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRtaskProject.FinishButtonClick
        Me.CurrentPresenter.SaveProject(Me.CTRLdetail.GetValidateTask)
    End Sub


    Public Sub BTNnextClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.CurrentPresenter.SetCurrentCommunityID(Me.CTRLcommunity.SelectedCommunitiesID)
    End Sub

    Private Sub WZRtaskProject_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRtaskProject.Init

    End Sub

    Private Sub WZRtaskProject_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRtaskProject.PreviousButtonClick
        Me.CurrentPresenter.PreviusStep()
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub

    Public Sub GoBackPage(ByVal Action As COL_BusinessLogic_v2.UCServices.Services_TaskList.ActionType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.GoBackPage
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Action, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Project, Me.dtoProject.TaskID), InteractionType.UserWithLearningObject)
        Me.PageUtility.RedirectToUrl("TaskList/AssignedTasks.aspx?View=TodayTasks&OrderBy=Community&CommunityFilter=AllCommunities&PageSize=50&Page=0")
    End Sub


    Public Sub LoadNoPermissionToCreate(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddProject.LoadNoPermissionToCreate
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.Master.ShowNoPermission = True
    End Sub

    Public Property UrlTaskID As Long Implements IViewAddProject.UrlTaskID
        Get
            Return Me.ViewState("UrlTaskID")
            'Return UrlTaskID
        End Get
        Set(ByVal value As Long)
            Me.ViewState("UrlTaskID") = value
            'UrlTaskID = value
        End Set
    End Property
End Class