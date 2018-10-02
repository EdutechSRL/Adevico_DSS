Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    Public Class InvolvedUsersDetailUCPresenter
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

        Public Overloads ReadOnly Property View() As IViewUC_involvedUsersDetail
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

        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUC_involvedUsersDetail)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub

#End Region

        Public Sub InitView(ByVal oID As Long)
            Dim oTask As Task = Me.CurrentTaskManager.GetTask(oID)
            Dim oList As List(Of dtoUsersOnQuickSelection) = Me.CurrentTaskManager.GetPoweredUserDetail(oTask)
            Me.View.InitView(oList)
        End Sub

        Public Sub Load()

        End Sub



    End Class

End Namespace
