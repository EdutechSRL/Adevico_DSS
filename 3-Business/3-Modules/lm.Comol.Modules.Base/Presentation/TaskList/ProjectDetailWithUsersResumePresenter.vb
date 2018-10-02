Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList


    Public Class ProjectDetailWithUsersResumePresenter
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

        Public Overloads ReadOnly Property View() As iViewProjectDetailWithUsersResume
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

        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As iViewProjectDetailWithUsersResume)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub

#End Region

        Public Sub InitView()

            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission) 'nn loggato allora errore
            Else
                If (Me.View.ViewModeType = ViewModeType.TaskAdmin) Then
                    Me.View.TaskPermission = Me.CurrentTaskManager.GetRolePermissions(TaskRole.ProjectOwner)
                Else
                    Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.CurrentProjectID, Me.AppContext.UserContext.CurrentUserID)
                End If
                Dim oProject As Task = Me.CurrentTaskManager.GetTask(Me.View.CurrentProjectID)
                Dim oProjectName As String = oProject.Name
                Me.View.InitView(Me.View.CurrentProjectID, oProjectName)
            End If

        End Sub



    End Class
End Namespace