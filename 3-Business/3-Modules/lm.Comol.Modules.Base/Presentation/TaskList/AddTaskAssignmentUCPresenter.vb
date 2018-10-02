Imports lm.Comol.Core.DomainModel.Common

Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain



Namespace lm.Comol.Modules.Base.Presentation.TaskList

    Public Class AddTaskAssignmentUCPresenter
        Inherits DomainPresenter


        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager


#Region "Standard"

        Public Overloads Property CurrentManager() As TaskManager
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As TaskManager)
                _CurrentManager = value
            End Set
        End Property

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IviewUC_AddTaskAssignment
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

        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewUC_AddTaskAssignment)
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
                    _Permission = Me.View.ModulePersmission
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

        Function LoadRoleList()
            Dim listRole As New List(Of TaskRole)
            listRole.Add(TaskRole.Manager)
            If Me.View.isChild Then
                listRole.Add(TaskRole.Resource)
            End If
            listRole.Add(TaskRole.Visitor)
            Return listRole
        End Function

        Public Function SaveTaskAssignment() As List(Of Long)
            Dim oTask As Task
            Dim ListOfPersonToAssign As New List(Of Person)
            Dim ListOfPersonID As List(Of Integer) = Me.View.GetSelectedUser

            oTask = Me.CurrentTaskManager.GetTask(Me.View.CurrentTaskID)
            For Each personID As Integer In ListOfPersonID
                ListOfPersonToAssign.Add(Me.BaseManager.GetPerson(personID))
            Next
            Dim ListOfTAID As List(Of Long) = Me.CurrentTaskManager.AddTaskAssignments(ListOfPersonToAssign, Me.View.CurrentRole, oTask, Me.AppContext.UserContext.CurrentUser)
            If Me.View.CurrentRole = TaskRole.Manager And Me.View.GetIfManagerIsResource Then
                Dim listOfTaskAssignments As List(Of TaskAssignment)
                listOfTaskAssignments = Me.CurrentTaskManager.GetTaskAssignments(Me.View.CurrentTaskID, TaskRole.Resource)
                Dim ListOfManagerResource As List(Of Person) = (From person In ListOfPersonToAssign Select person).Except((From assign In listOfTaskAssignments Select assign.AssignedUser).ToList).ToList
                Me.CurrentTaskManager.AddTaskAssignments(ListOfManagerResource, TaskRole.Resource, oTask, Me.AppContext.UserContext.CurrentUser)
            End If

            Me.InitUserSelection()
            Return ListOfTAID
        End Function


        Public Function GetSelectedUser()
            Return Me.View.GetSelectedUser()
        End Function

        Public Function GetCurretRole()
            Return Me.View.CurrentRole()
        End Function

        Public Function GetIfManagerIsResource() As Boolean
            Return Me.View.GetIfManagerIsResource
        End Function

        Public Sub InitView(ByVal CurrentTaskID As Long)
            Me.View.CurrentTaskID = CurrentTaskID
            Dim ChildNumber = Me.CurrentTaskManager.GetNumberOfChildren(Me.View.CurrentTaskID, False)
            Me.View.isChild = (ChildNumber = 0)

            Me.View.CurrentRole = TaskRole.Manager
            Me.InitUserSelection()
        End Sub

        Private Sub InitUserSelection()
            Dim listOfPersonIDToHide As New List(Of Integer)

            Me.View.ListOfCommunityID = New List(Of Integer)
            Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetTaskCommunity(Me.View.CurrentTaskID)

            If Me.View.CurrentCommunityID = 0 Then 'TEMPORANEA!!! CON IL PORTALE METTE SEMPRE LA COM 10!!!!!
                Me.View.CurrentCommunityID = -1
            End If
            Me.View.ListOfCommunityID.Add(Me.View.CurrentCommunityID)
            listOfPersonIDToHide = GetPersonsToHide()
            ' listOfPersonIDToHide.Add(1)
            Me.View.InitUserSelection(listOfPersonIDToHide)

        End Sub


        Public Function GetPersonsToHide()
            Dim listOfPersonIDToHide As New List(Of Integer)
            Dim listOfTaskAssignments As List(Of TaskAssignment)
            listOfTaskAssignments = Me.CurrentTaskManager.GetTaskAssignments(Me.View.CurrentTaskID, Me.View.CurrentRole)

            For Each ta As TaskAssignment In listOfTaskAssignments
                listOfPersonIDToHide.Add(ta.AssignedUser.Id)
            Next
            Return listOfPersonIDToHide
        End Function



    End Class
End Namespace