Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class GanttPresenter
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
        Public Overloads ReadOnly Property View() As IviewGantt
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewGantt)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            Me.View.ProjectID = Me.CurrentTaskManager.GetProjectID(Me.View.TaskID)
            Dim TaskPermission As TaskPermissionEnum = Me.CurrentTaskManager.GetPermissionOverAllProject(Me.View.ProjectID, Me.AppContext.UserContext.CurrentUserID)
            If (TaskPermission And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView Then
                Dim dtoProjectDetail As dtoTaskDetail = Me.CurrentTaskManager.GetTaskDetail(Me.View.ProjectID)
                Me.View.ShowGantt(dtoProjectDetail.TaskWBS & " " & dtoProjectDetail.TaskName)
            Else
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
            End If

        End Sub

    End Class
End Namespace