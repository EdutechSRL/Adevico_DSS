Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    Public Class SelectTaskPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager

#Region "Standard"

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewSelectTask
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewSelectTask)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region



        Public Sub Init(ByVal CurrentTaskID As Long)
            Me.View.CurrentTaskID = CurrentTaskID
            Me.LoadTask()
        End Sub

        Public Sub SetSelectedTask(ByVal TaskID As Long)
            Me.View.SelectedTaskID = TaskID
            Me.LoadTask()

        End Sub

        Public Sub LoadTask()

            Dim ListOfTasks As IList(Of dtoSelectTask) = Me.CurrentTaskManager.GetSelectTaskMap(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
            ' Me.View.listOfTasks = Me.CurrentTaskManager.GetSelectTaskMap(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
            If ListOfTasks.Count > 0 Then
                Me.View.StartLevel = ListOfTasks.ElementAt(0).Level
            End If

            Me.View.LoadTask(ListOfTasks)
        End Sub


    End Class
End Namespace