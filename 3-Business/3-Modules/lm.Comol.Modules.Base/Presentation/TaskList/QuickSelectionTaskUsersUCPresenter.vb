Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class QuickSelectionTaskUsersUCPresenter
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

        Public Overloads ReadOnly Property View() As IViewUC_QuickSelectionTaskUsers
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

        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUC_QuickSelectionTaskUsers)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region


        Public Sub InitView(ByVal CurrentTaskID As Long)
            
            Me.LoadRoles()

            If Me.View.PreLoadedRole = TaskRole.None Then
                Me.View.CurrentRole = TaskRole.Manager
            Else
                Me.View.CurrentRole = Me.View.PreLoadedRole
            End If

            Me.LoadQuickUsers() '(CurrentTaskID, Me.View.CurrentRole)

        End Sub

        Public Sub LoadQuickUsers() '(ByVal CurrentTaskID As Long, ByVal CurrentTaskRole As TaskRole)

            'Setto il task corrente e la prop
            'Me.View.CurrentTaskID = CurrentTaskID
            'Dim ChildNumber = Me.CurrentTaskManager.GetNumberOfChildren(Me.View.CurrentTaskID, False)
            'Me.View.isChild = (ChildNumber = 0)

            Dim QuickUsersList As List(Of dtoUsers) = New List(Of dtoUsers)

            'Dim oTaskID As Long = Me.View.CurrentTaskID
            QuickUsersList = Me.CurrentTaskManager.GetQuickSelectionUsersFiltered(Me.View.CurrentTaskID, Me.View.CurrentRole)

            Me.View.LoadQuickSelUsers(QuickUsersList)

            'Me.View.NavigationUrl(Me.View.CurrentRole, Me.View.CurrentTaskID)

        End Sub

        Public Sub LoadRoles()

            Dim ChildNumber = Me.CurrentTaskManager.GetNumberOfChildren(Me.View.CurrentTaskID, False)
            Me.View.isChild = (ChildNumber = 0)

            Dim listRole As New List(Of TaskRole)
            listRole.Add(TaskRole.Manager)
            If Me.View.isChild Then
                listRole.Add(TaskRole.Resource)
            End If
            listRole.Add(TaskRole.Visitor)

            Me.View.LoadRoles(listRole)

            If (listRole.Contains(Me.View.PreLoadedRole)) Then
                Me.View.CurrentRole = Me.View.PreLoadedRole
            Else
                Me.View.CurrentRole = TaskRole.Manager
            End If
        End Sub

        Public Function GetPeopleID() As List(Of Person)
            Dim list As New List(Of Person)
            Return list
        End Function

        Public Sub SaveTaskAssignment()

            Dim oTask As Task
            Dim oRole As TaskRole
            Dim ListOfPersonToAssign As New List(Of Person)

            ListOfPersonToAssign = Me.View.GetQuickSelectionUsers()
            oTask = Me.CurrentTaskManager.GetTask(Me.View.CurrentTaskID)
            oRole = Me.View.CurrentRole

            Me.CurrentTaskManager.AddTaskAssignments(ListOfPersonToAssign, oRole, oTask, Me.AppContext.UserContext.CurrentUser)

        End Sub

    End Class
End Namespace

