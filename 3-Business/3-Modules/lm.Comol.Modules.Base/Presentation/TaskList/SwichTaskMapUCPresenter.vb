Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class SwichTaskMapUCPresenter
        Inherits DomainPresenter
        Private _BaseManager As BusinessLogic.ManagerCommon
        Private _BaseTaskManager As Modules.TaskList.Business.TaskManager


#Region "Standard"

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As BusinessLogic.ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewUC_SwichTaskMap
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUC_SwichTaskMap)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        '#Region "PERMESSI"
        '        Private _Permission As ModuleTaskList
        '        Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleTaskList))
        '        Private ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As ModuleTaskList
        '            Get
        '                If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
        '                    _Permission = Me.View.ModulePersmission
        '                    Return _Permission
        '                ElseIf CommunityID > 0 Then
        '                    _Permission = (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
        '                    If IsNothing(_Permission) Then
        '                        _Permission = New ModuleTaskList
        '                    End If
        '                    Return _Permission
        '                Else
        '                    Return _Permission
        '                End If
        '                Return _Permission
        '            End Get
        '        End Property
        '        Private ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))
        '            Get
        '                If IsNothing(_CommunitiesPermission) Then
        '                    _CommunitiesPermission = Me.View.CommunitiesPermission()
        '                End If
        '                Return _CommunitiesPermission
        '            End Get
        '        End Property
        '#End Region

        'Public Sub SetStartDateVisible(ByVal isVisible As Boolean)
        '    Me.View.isStartDateVisible = isVisible
        '    Me.View.LoadTasks()
        'End Sub

        'Public Sub SetEndDateVisible(ByVal isVisible As Boolean)
        '    Me.View.isEndDateVisible = isVisible
        '    Me.View.LoadTasks()
        'End Sub



        Public Sub Init(ByVal CurrentTaskID As Long)
            Me.View.CurrentTaskID = CurrentTaskID

            Dim ListOfTasks As List(Of dtoSwichTask) = Me.GetTasks()
            If (ListOfTasks.Count > 1) Then

                ListOfTasks.RemoveAt(0)
                Me.View.StartLevel = ListOfTasks.First().Level
                Me.View.LastLevel = Me.CurrentTaskManager.GetMaxLevel(ListOfTasks)
                Me.View.LoadDdlWBSlevel()
                Me.View.LoadTasks(ListOfTasks)

            End If
        End Sub

        Private Function GetTasks()
            Return Me.CurrentTaskManager.GetSwichTaskMap(Me.View.CurrentTaskID)

        End Function


        Public Sub MoveTaskWbsPrevius(ByVal TaskID As Long)
            Dim isSwiched As Boolean
            isSwiched = Me.CurrentTaskManager.MoveTaskWbsPrevius(TaskID)

            ReloadAfterSwich(isSwiched)

        End Sub

        Public Sub MoveTaskWbsNext(ByVal TaskID As Long)
            Dim isSwiched As Boolean
            isSwiched = Me.CurrentTaskManager.MoveTaskWbsNext(TaskID)

            ReloadAfterSwich(isSwiched)

        End Sub


        Private Sub ReloadAfterSwich(ByVal isSwiched As Boolean)

            If isSwiched Then
                Dim ListOfTasks As List(Of dtoSwichTask) = Me.GetTasks()
                ListOfTasks.RemoveAt(0)
                Me.View.LoadTasks(ListOfTasks)

            End If

        End Sub

    End Class
End Namespace