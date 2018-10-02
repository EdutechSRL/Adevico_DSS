Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class AddProjectPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager

        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.BaseManager.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_TaskList.Codex)
                End If
                Return _ModuleID
            End Get
        End Property

#Region "Standard"
        Public Overloads Property CurrentManager() As TaskManager
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As TaskManager)
                _CurrentManager = value
            End Set
        End Property

        Private Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewAddProject
            Get
                Return MyBase.View
            End Get
        End Property
        Public Property CurrentTaskManager() As TaskManager
            Get
                Return _BaseTaskManager
            End Get
            Set(ByVal value As TaskManager)
                _BaseTaskManager = value
            End Set
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewAddProject)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

#Region "PERMESSI"
        Private _Permission As ModuleTaskList
        Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleTaskList))
        Private ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As ModuleTaskList
            Get
                If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
                    _Permission = Me.View.ModulePermission
                    Return _Permission
                ElseIf CommunityID > 0 Then
                    _Permission = (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
                    If IsNothing(_Permission) Then
                        _Permission = New ModuleTaskList
                    End If
                    Return _Permission
                Else
                    Return _Permission
                End If
                Return _Permission
            End Get
        End Property
        Private ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))
            Get
                If IsNothing(_CommunitiesPermission) Then
                    _CommunitiesPermission = Me.View.CommunitiesPermission()
                End If
                Return _CommunitiesPermission
            End Get
        End Property
#End Region

        Public Sub InitView()
            Me.View.SessionUniqueKey = System.Guid.NewGuid

            InitSetProjectType()
        End Sub


        Private Sub InitSetProjectType()
            Me.View.CurrentStep = IViewAddProject.StepType.SelectType
            Dim oListOfProjectType As New List(Of IViewAddProject.viewTaskListType)

            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID

            If CommunityID = 0 Then
                If (From c In Me.View.CommunitiesPermission Where c.Permissions.Administration OrElse c.Permissions.CreateCommunityProject).Count > 0 Then
                    oListOfProjectType.Add(IViewAddProject.viewTaskListType.Community)
                End If
            Else
                If (From c In Me.View.CommunitiesPermission Where c.ID = CommunityID AndAlso (c.Permissions.Administration OrElse c.Permissions.CreateCommunityProject)).Count > 0 Then
                    oListOfProjectType.Add(IViewAddProject.viewTaskListType.Community)
                End If
            End If

            'Tia 
            If (From c In Me.View.CommunitiesPermission Where c.ID = CommunityID AndAlso (c.Permissions.Administration OrElse c.Permissions.CreatePersonalProject)).Count > 0 Then
                oListOfProjectType.Add(IViewAddProject.viewTaskListType.Personal)
                oListOfProjectType.Add(IViewAddProject.viewTaskListType.PersonalCommunity)
            End If

            If oListOfProjectType.Count > 0 Then
                Me.View.InitSelectProjectType(oListOfProjectType)
            Else
                Me.View.LoadNoPermissionToCreate(CommunityID, Me.ModuleID)
            End If

        End Sub

        Public Sub SetProjectType(ByVal ProjectType As IViewAddProject.viewTaskListType)

            Me.View.CurrentCommunityID = Me.AppContext.UserContext.CurrentCommunityID

            Select Case ProjectType
                Case IViewAddProject.viewTaskListType.Community
                    Me.View.isPortal = False
                    Me.View.isPersonal = False
                    If (Me.View.CurrentCommunityID = 0) Then
                        InitSelectCommunity()
                    Else
                        InitSetProjectProperty()
                    End If

                Case IViewAddProject.viewTaskListType.Personal
                    Me.View.isPersonal = True
                    Me.View.isPortal = True
                    InitSetProjectProperty()

                Case IViewAddProject.viewTaskListType.PersonalCommunity
                    Me.View.isPersonal = True
                    Me.View.isPortal = False
                    If (Me.View.CurrentCommunityID = 0) Then
                        InitSelectCommunity()
                    Else
                        InitSetProjectProperty()
                    End If

                Case Else
                    Me.View.ShowError(My.Resources.ModuleBaseResource.ItemNotFound)

            End Select
        End Sub

        Public Sub PreviusStep()
            Select Case Me.View.CurrentStep
                Case IViewAddProject.StepType.SelectCommunity
                    Me.InitSetProjectType()
                Case IViewAddProject.StepType.SetProperty
                    SetDtoParent()
                    'If Me.View.isPortal Then
                    Me.InitSetProjectType()
                    'Else
                    '    Me.InitSetProjectProperty()
                    'End If
                Case Else
                    Me.InitSetProjectType()
            End Select
        End Sub

        Private Sub SetDtoParent()
            Me.View.dtoProject = New dtoTaskDetail(Me.View.GetProject)
            Me.View.dtoProject.TaskWBS = ""
        End Sub

        Public Sub SetCurrentCommunityID(ByVal ListCommunityID As List(Of Integer))
            If ListCommunityID.Count > 0 Then
                Me.View.CurrentCommunityID = ListCommunityID.Last
                InitSetProjectProperty()
            Else
                Me.InitSelectCommunity()
            End If
        End Sub

        Private Sub InitSelectCommunity()
            Me.View.CurrentStep = IViewAddProject.StepType.SelectCommunity
            Dim oCommunitiesID As New List(Of Integer)
            oCommunitiesID = (From o In Me.CommunitiesPermission Select o.ID).ToList
            If IsNothing(oCommunitiesID) Then
                oCommunitiesID = New List(Of Integer)
            End If

            Me.View.InitCommunitySelection(oCommunitiesID)
        End Sub


        Private Sub InitSetProjectProperty()
            Me.View.CurrentStep = IViewAddProject.StepType.SetProperty
            Dim TaskDetailWithPermission As New dtoTaskDetailWithPermission
            Dim CommunityName As String
            If (Me.View.isPortal Or Me.View.CurrentCommunityID = 0) Then
                CommunityName = "Portal"
            Else
                CommunityName = Me.BaseManager.GetCommunity(Me.View.CurrentCommunityID).Name
            End If
            If Me.View.dtoProject.TaskID = -1 Then
                Me.View.dtoProject.CommunityName = CommunityName
                TaskDetailWithPermission.dtoTaskDetail = Me.View.dtoProject
                Me.View.InitSetProjectProperty(TaskDetailWithPermission, IViewUC_TaskDetail.viewDetailType.AddProject, Me.View.BackUrl)
            Else
                Me.View.dtoProject.CommunityName = CommunityName
                TaskDetailWithPermission.dtoTaskDetail = Me.View.dtoProject
                Me.View.InitSetProjectProperty(TaskDetailWithPermission, IViewUC_TaskDetail.viewDetailType.Update, Me.View.BackUrl)
            End If
        End Sub

        Public Sub SaveProject(ByVal oTask As Task)
            If (Not Me.View.isPortal) Then
                oTask.Community = Me.BaseManager.GetCommunity(Me.View.CurrentCommunityID)
            End If
            oTask.isPersonal = Me.View.isPersonal
            oTask.isPortal = Me.View.isPortal
            oTask = Me.CurrentTaskManager.AddProject(oTask, Me.AppContext.UserContext.CurrentUser)
            Me.View.dtoProject = New dtoTaskDetail(oTask)
            Me.CurrentTaskManager.AddTaskAssignment(Me.AppContext.UserContext.CurrentUser, TaskRole.ProjectOwner, oTask, Me.AppContext.UserContext.CurrentUser)
            Me.CurrentTaskManager.AddTaskAssignment(Me.AppContext.UserContext.CurrentUser, TaskRole.Resource, oTask, Me.AppContext.UserContext.CurrentUser)

            '
            Me.View.UrlTaskID = oTask.ID
            '
            Me.View.ClearUniqueKey()
            Me.View.GoBackPage(Services_TaskList.ActionType.ProjectAdded)

        End Sub

        Public Sub ReturnToMainPage()

            Me.View.ClearUniqueKey()
            Me.View.GoBackPage(Services_TaskList.ActionType.AnnulAddProject)
        End Sub

    End Class
End Namespace