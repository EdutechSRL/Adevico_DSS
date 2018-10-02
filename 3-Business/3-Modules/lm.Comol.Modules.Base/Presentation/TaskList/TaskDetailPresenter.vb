
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Modules.TaskList


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class TaskDetailPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager
        Private _Service As lm.Comol.Modules.TaskList.ServiceTaskList

#Region "Standard"

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewTaskDetail
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewTaskDetail)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Public Sub IsAdminModeAndAdminRole()
            If Me.View.ViewToLoad = ViewModeType.TaskAdmin And (From c In Me.View.CommunitiesPermission Where c.Permissions.Administration).Count > 0 Then
                Me.View.TaskPermission = CurrentTaskManager.GetRolePermissions(TaskRole.ProjectOwner)
            End If
        End Sub

        Public Sub InitView()

            'Me.IsAdminModeAndAdminRole()

            'Dim oPermission As CoreItemPermission = Service.GetTaskPermission(Me.CurrentTaskManager.GetTask(View.CurrentTaskID), [module], moduleRepository)

            'CoreItemPermission oPermission = Service.GetTaskPermission(oTask, module, moduleRepository);

            'Me.LoadTaskFiles(Me.CurrentTaskManager.GetTask(View.CurrentTaskID), oPermission)


            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission) 'nn loggato allora errore
            Else
                'inizializzo le variabili per i files che varranno uguali per tutti, tanto è solo READ.No controllo sui ruoli
                Dim oTask As Task = Service.GetTask(View.CurrentTaskID)
                If oTask Is Nothing Then
                    Me.View.ShowError(My.Resources.ModuleBaseResource.ItemNotFound)
                Else
                    Dim files As IList(Of iCoreItemFileLink(Of Long)) = Service.GetTaskFiles(oTask, True)
                    Dim oModule As lm.Comol.Modules.TaskList.ModuleTasklist = New lm.Comol.Modules.TaskList.ModuleTasklist()
                    If Not IsNothing(oTask.Community) Then
                        'View.CurrentCommunityID = oTask.Community.Id
                        oModule = (From p In View.CommunitiesPermissionCS Where p.ID = oTask.Community.Id Select p.Permissions).FirstOrDefault()
                        If IsNothing(oModule) Then
                            oModule = New lm.Comol.Modules.TaskList.ModuleTasklist()
                        End If

                    Else
                        oModule = lm.Comol.Modules.TaskList.ModuleTasklist.CreatePortalmodule(UserContext.UserTypeID)
                    End If

                    Dim moduleRepository As CoreModuleRepository = View.RepositoryPermission(View.CurrentCommunityID)
                    Dim oPermission As CoreItemPermission = Service.GetTaskPermission(oTask, oModule, moduleRepository)
                    LoadTaskFiles(oTask, oPermission)
                End If
               

            End If

            'Nuova parte A
            If Me.View.ViewToLoad = ViewModeType.TaskAdmin Then
                Me.IsAdminModeAndAdminRole()
            Else
                Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
            End If


            If ((Me.View.TaskPermission And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView) Then
                Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                Select Case Me.View.CurrentViewDetailType
                    Case IViewTaskDetail.viewDetailType.Read
                        If Me.View.ViewToLoad = ViewModeType.TaskAdmin And (From c In Me.View.CommunitiesPermission Where c.Permissions.Administration).Count > 0 Then
                            Me.View.InitViewReadOnly(GetTaskDetailWithAdminPermission)
                        Else
                            Me.View.InitViewReadOnly(GetTaskDetailWithPermission())
                        End If

                    Case IViewTaskDetail.viewDetailType.Editable
                        If Me.View.ViewToLoad = ViewModeType.TaskAdmin And (From c In Me.View.CommunitiesPermission Where c.Permissions.Administration).Count > 0 Then
                            Me.View.InitViewEditable(GetTaskDetailWithAdminPermission)
                        Else
                            If Me.CanUpdate(Me.View.TaskPermission) Then
                                Me.View.InitViewEditable(GetTaskDetailWithPermission())
                            Else
                                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                            End If
                        End If

                        'Nuova parte gestione Administration Tab deprecata?Gia gestita in Nuova parte A
                        'Case IViewTaskDetail.viewDetailType.Admin
                        '  Me.View.InitViewEditable(Get)

                    Case Else
                        Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                End Select
            Else
                Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionOverAllProject(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
                If (Me.View.TaskPermission And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView Then
                    Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                    Me.View.InitViewReadOnly(GetTaskDetailWithPermission())
                Else
                    Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                End If
            End If

            'End If
        End Sub

        Public Sub LoadTaskFiles(ByVal task As Task, ByVal moduleTask As CoreItemPermission)
            '  = View.RepositoryPermission(CommunityId);
            '    CoreItemPermission oPermission
            Dim files As IList(Of iCoreItemFileLink(Of Long)) = Service.GetTaskFiles(task, moduleTask.AllowEdit)
            View.LoadFilesToManage(task.ID, moduleTask, files, lm.Comol.Modules.TaskList.Domain.RootObject.PublishUrl())

        End Sub



        Private ReadOnly Property Service() As lm.Comol.Modules.TaskList.ServiceTaskList
            Get
                If _Service Is Nothing Then
                    _Service = New ServiceTaskList(AppContext)
                End If
                Return _Service
            End Get
        End Property



        Public Function GetTaskDetailWithPermission()
            Dim dto As dtoTaskDetailWithPermission = CurrentTaskManager.GetTaskDetailWithPermission(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
            If dto.dtoTaskDetail.TaskWBS = "0" Then
                dto.dtoTaskDetail.TaskWBS = ""
            End If
            Return dto
        End Function
        Public Function GetTaskDetailWithAdminPermission()
            Dim dto As dtoTaskDetailWithPermission = CurrentTaskManager.GetTaskDetailWithAdminPermission(Me.View.CurrentTaskID)
            If dto.dtoTaskDetail.TaskWBS = "0" Then
                dto.dtoTaskDetail.TaskWBS = ""
            End If
            Return dto
        End Function

        Public Sub UpdateTask(ByVal oTask As Task)
            Me.CurrentTaskManager.UpdateTaskDetail(Me.View.CurrentTaskID, oTask, Me.AppContext.UserContext.CurrentUser)
        End Sub

        Public Function CanReloadCurrentPage()
            Dim NewPermission As TaskPermissionEnum = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
            Return CanUpdate(NewPermission)
        End Function

        Private Function CanUpdate(ByRef Permission As TaskPermissionEnum)

            '   Select Case Me.View.TaskPermission
            If ((Permission And TaskPermissionEnum.AddFile) = TaskPermissionEnum.AddFile) Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.ManagementUser) = TaskPermissionEnum.ManagementUser Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete Then
                Return True
            ElseIf ((Permission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate) Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.TaskSetCategory) = TaskPermissionEnum.TaskSetCategory Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.TaskSetDeadline) = TaskPermissionEnum.TaskSetDeadline Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.TaskSetEndDate) = TaskPermissionEnum.TaskSetEndDate Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.TaskSetPriority) = TaskPermissionEnum.TaskSetPriority Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.TaskSetStartDate) = TaskPermissionEnum.TaskSetStartDate Then
                Return True
            ElseIf (Permission And TaskPermissionEnum.TaskSetStatus) = TaskPermissionEnum.TaskSetStatus Then
                Return True
            Else
                Return False
                '  End Select
            End If
        End Function


    End Class
End Namespace