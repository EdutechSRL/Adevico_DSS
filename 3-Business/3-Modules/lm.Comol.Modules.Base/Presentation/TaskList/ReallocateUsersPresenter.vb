Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class ReallocateUsersPresenter
        Inherits DomainPresenter
        Private _BaseManager As BusinessLogic.ManagerCommon
        Private _BaseTaskManager As TaskManager
#Region "Standard"

        Public Property BaseManager() As BusinessLogic.ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As BusinessLogic.ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewReallocateUsers
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
            Me.BaseManager = New BusinessLogic.ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewReallocateUsers)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New BusinessLogic.ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission) 'nn loggato allora errore
            Else
                If VerifyPermission() Then
                    Me.View.SessionUniqueKey = System.Guid.NewGuid
                    Dim dtoTaskToAssignResource As dtoTaskSimple
                    Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                    dtoTaskToAssignResource = Me.CurrentTaskManager.GetParentNameAndID(Me.View.CurrentTaskID)
                    Me.View.ParentID = dtoTaskToAssignResource.ID
                    '    Me.View.ListOfUsers = New List(Of dtoReallocateTAWithHeader)
                    Select Case Me.View.CurrentModeType
                        Case IViewReallocateUsers.ModeType.Undelete
                            Me.View.TaskToAssignName = Me.CurrentTaskManager.GetTaskNameAndID(Me.View.CurrentTaskID).Name
                            Me.View.ListOfUsers.Add(Me.CurrentTaskManager.GetDtoReallacateUsersWhithHeaderForSingleTask(Me.View.ParentID))
                            Me.View.InitSelectUsers(Me.View.ListOfUsers)
                        Case IViewReallocateUsers.ModeType.VirtualDelete
                            Me.View.TaskToAssignName = dtoTaskToAssignResource.Name
                            Me.View.ListOfUsers = Me.CurrentTaskManager.GetDtoReallacateUsersWhithHeaderForTaskTree(Me.View.CurrentTaskID)
                            Me.View.InitSelectUsers(Me.View.ListOfUsers)
                        Case Else
                            Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                    End Select

                Else
                    Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                End If
            End If
        End Sub

        Private Function VerifyPermission() As Boolean
            Dim Permission As TaskPermissionEnum = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
            Dim ProjectID As Long = Me.CurrentTaskManager.GetProjectID(Me.View.CurrentTaskID)
            If (ProjectID = Me.View.CurrentTaskID) And ((Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete) Then
                Return True
            ElseIf ((Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Sub NextStep()
            Select Case Me.View.CurrentModeType
                Case IViewReallocateUsers.ModeType.Undelete
                    NextStepUndeleteTask()
                Case IViewReallocateUsers.ModeType.VirtualDelete
                    NextStepVirtualDeleteTask()
                Case Else
                    Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
            End Select
        End Sub
        Private Sub NextStepUndeleteTask()
            Select Case Me.View.CurrentStep
                Case IViewReallocateUsers.StepType.SelectUsers
                    Dim ListOfDtoReallocateTA As List(Of dtoReallocateTA) = Me.View.GetUserFromSelectUsers
                    Me.View.ListOfUsers = Me.View.GetUserFromSelectUsersWithHeader
                    Dim ListDtoWithHeader As New List(Of dtoReallocateTAWithHeader)
                    Dim dtoWithHeader As New dtoReallocateTAWithHeader
                    dtoWithHeader.TaskName = Me.View.TaskToAssignName
                    dtoWithHeader.TaskAssignments = (From item In ListOfDtoReallocateTA Where Not item.isDeleted Select item).ToList
                    ListDtoWithHeader.Add(dtoWithHeader)
                    Me.View.CurrentStep = IViewReallocateUsers.StepType.ResumeUsers
                    Me.View.InitFinalUserResume(ListDtoWithHeader)
                Case IViewReallocateUsers.StepType.ResumeUsers
                    Dim ListDtoReallocate As List(Of dtoReallocateTA) = Me.View.GetUserFromResumeUsers
                    Dim ListOfTAIDToReallocate As List(Of Long) = Me.CurrentTaskManager.ReallocateResourceAfterUndeleteOfTask(Me.View.CurrentTaskID, Me.View.ParentID, ListDtoReallocate, Me.AppContext.UserContext.CurrentUser)
                    Me.View.GoBackPage(ListOfTAIDToReallocate)
                    Me.View.ClearUniqueKey()
            End Select
        End Sub
        Private Sub NextStepVirtualDeleteTask()
            Select Case Me.View.CurrentStep
                Case IViewReallocateUsers.StepType.SelectUsers
                    Dim ListOfDtoReallocateTA As List(Of dtoReallocateTA) = Me.View.GetUserFromSelectUsers
                    Me.View.ListOfUsers = Me.View.GetUserFromSelectUsersWithHeader
                    Dim ListDtoWithHeader As New List(Of dtoReallocateTAWithHeader)
                    Dim dtoWithHeader As New dtoReallocateTAWithHeader

                    dtoWithHeader.TaskName = Me.View.TaskToAssignName
                    Dim temp = (From item In ListOfDtoReallocateTA Where Not item.isDeleted Group item By item.PersonID, item.PersonSurnameName, item.Role Into Average(item.Completeness)).ToList()
                    dtoWithHeader.TaskAssignments = (From item In temp Select New dtoReallocateTA() With {.Completeness = item.Average, .Role = item.Role, .isDeleted = False, .PersonID = item.PersonID, .PersonSurnameName = item.PersonSurnameName}).ToList
                    ListDtoWithHeader.Add(dtoWithHeader)
                    Me.View.CurrentStep = IViewReallocateUsers.StepType.ResumeUsers
                    Me.View.InitFinalUserResume(ListDtoWithHeader)
                Case IViewReallocateUsers.StepType.ResumeUsers
                    Dim ListDtoReallocate As List(Of dtoReallocateTA) = Me.View.GetUserFromResumeUsers
                    Dim ListOfUserIDToReallocate As List(Of Long) = Me.CurrentTaskManager.ReallocateResourceAfterVirtualDeleteOfTask(Me.View.CurrentTaskID, ListDtoReallocate, Me.AppContext.UserContext.CurrentUser)
                    Me.View.GoBackPage(ListOfUserIDToReallocate)
                    Me.View.ClearUniqueKey()
            End Select
        End Sub

        Public Sub PreviusStep()
            Me.View.CurrentStep = IViewReallocateUsers.StepType.SelectUsers
            Me.View.InitSelectUsers(Me.View.ListOfUsers)
        End Sub

        Public Sub ClearUniqueKey()
            Me.View.ClearUniqueKey()
        End Sub

    End Class
End Namespace