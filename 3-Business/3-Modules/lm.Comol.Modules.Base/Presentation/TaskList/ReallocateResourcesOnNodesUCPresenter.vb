Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class ReallocateResourcesOnNodesUCPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager

#Region "Standard"
        Public Overloads Property CurrentManager() As TaskManager
            Get
                Return _BaseTaskManager
            End Get
            Set(ByVal value As TaskManager)
                _BaseTaskManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IViewUC_ReallocateResourcesOnNodes
            Get
                Return MyBase.View
            End Get
        End Property

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New TaskManager(MyBase.AppContext)
        End Sub

        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUC_ReallocateResourcesOnNodes)
            MyBase.New(oContext, view)
            Me.CurrentManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub

#End Region

        Public Sub InitView(ByVal oList As List(Of dtoReallocateTAWithHeader), ByVal EditMode As IViewUC_ReallocateResourcesOnNodes.EditType)
            Me.View.CurrentEditMode = EditMode
            Me.View.LoadResources(oList)
        End Sub

        Public Function GetUserList()
            Return Me.View.GetUserList()
        End Function


    End Class
End Namespace
