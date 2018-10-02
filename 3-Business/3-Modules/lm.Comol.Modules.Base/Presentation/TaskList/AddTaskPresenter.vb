Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class AddTaskPresenter
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
        Public Overloads ReadOnly Property View() As IViewAddTask
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewAddTask)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission) 'nn loggato allora errore
            Else
                Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
                Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                If (Me.View.TaskPermission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate Then
                    SubInitView()
                Else
                    Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionOverAllProject(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
                    If (Me.View.TaskPermission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate Then
                        SubInitView()
                    Else
                        Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                    End If
                End If
            End If
        End Sub

        Private Sub SubInitView()
            Me.View.SessionUniqueKey = System.Guid.NewGuid
            Me.View.TasksToAdd = New List(Of Task)
            Me.View.dtoReallocateTAToAdd = New List(Of dtoReallocateTA)
            SelectFirstStep()
        End Sub


        Private Sub SetDtoReallocateTAParent()
            Dim ParentResourceTA As List(Of TaskAssignment) = Me.CurrentTaskManager.GetTaskAssignments(Me.View.ParentID, TaskRole.Resource)
            Me.View.dtoParentReallocateTA = (From oTA In ParentResourceTA Select New dtoReallocateTA(oTA)).ToList
            Me.View.dtoParentReallocateTA.Add(New dtoReallocateTA(Me.View.ParentID, TaskRole.Manager, Me.AppContext.UserContext.CurrentUserID, Me.AppContext.UserContext.CurrentUser.SurnameAndName))
        End Sub

        Private Sub SetDdtoReallocateTAToAddAfterChangeParent()
            For Each parentResource In Me.View.dtoParentReallocateTA
                Me.View.dtoReallocateTAToAdd = SubSetDdtoReallocateTAToAddAfterChangeParent(parentResource)
            Next
            GetParentProperties()
            For Each parentResource In Me.View.dtoParentReallocateTA()
                For Each oTask In Me.View.TasksToAdd
                    Dim tempDtoReallocate As New dtoReallocateTA(parentResource)
                    tempDtoReallocate.TaskID = oTask.ID
                    Me.View.dtoReallocateTAToAdd.Add(tempDtoReallocate)
                Next
            Next
        End Sub

        Private Function SubSetDdtoReallocateTAToAddAfterChangeParent(ByVal ParentResource As dtoReallocateTA)
            Return (From t In Me.View.dtoReallocateTAToAdd Where Not (t.PersonID = ParentResource.PersonID And t.Role = ParentResource.Role) Select t).ToList
        End Function



        Public Sub SelectFirstStep()
            Dim ChildCount As Integer = Me.CurrentTaskManager.GetNumberOfChildren(Me.View.CurrentTaskID, False)
            If ChildCount = 0 Then
                Me.View.CurrentTaskIsChild = True
                Me.View.ParentID = Me.View.CurrentTaskID
                Me.InitSetChildProperty()
            Else
                Me.View.CurrentTaskIsChild = False
                Me.View.CurrentStep = IViewAddTask.ViewStep.SelectParent
                Me.View.InitButton()
                Me.View.InitSelectParentMap()
            End If
        End Sub
        Private Sub InitSetChildProperty()
            Me.GetParentProperties()
            Me.View.CurrentStep = IViewAddTask.ViewStep.SetProperty
            Me.View.InitButton()
            Dim ParentName As String
            If String.Equals(Me.View.dtoParentProperty.dtoTaskDetail.TaskWBS, "0") Then
                ParentName = Me.View.dtoParentProperty.dtoTaskDetail.TaskName
            Else
                ParentName = Me.View.dtoParentProperty.dtoTaskDetail.TaskWBS & " " & Me.View.dtoParentProperty.dtoTaskDetail.TaskName
            End If

            Me.View.AddAnotherChildClicked = True
            Me.View.InitSetTaskProperty(ParentName, Me.View.dtoParentProperty, IViewUC_TaskDetail.viewDetailType.AddTask)
        End Sub

        Public Sub GetParentProperties()
            SetDtoReallocateTAParent()
            Dim dtoParent As New dtoTaskDetailWithPermission
            dtoParent.dtoTaskDetail = Me.CurrentTaskManager.GetTaskDetail(Me.View.ParentID)
            dtoParent.dtoTaskDetail.Description = ""
            dtoParent.dtoTaskDetail.Notes = ""
            dtoParent.dtoTaskDetail.PersonalCompleteness = 0
            Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.ParentID, Me.AppContext.UserContext.CurrentUserID)
            dtoParent.Permission = Me.View.TaskPermission
            Me.View.dtoParentProperty = dtoParent
            If Me.View.dtoParentProperty.dtoTaskDetail.isDeleted Then
                Me.View.ShowError("TaskDeleted")
            End If
        End Sub

        Public Sub Reload()
            Me.View.InitButton()
            Select Case Me.View.CurrentStep
                Case IViewAddTask.ViewStep.SelectParent
                    Me.View.InitSelectParentMap()
                Case IViewAddTask.ViewStep.ChangeParent
                    Me.View.InitSelectParentMap()
                    'Case IViewAddTask.ViewStep.SetProperty
                    '    Me.View.InitSelectParentMap()
                    'Case IViewAddTask.ViewStep.UserManagement

                    'Case IViewAddTask.ViewStep.AddUser
                    '    Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                    '    Me.InitResumeTasksWithModifyResources(ListOfDto, False)

            End Select
        End Sub

        Public Sub InitAddAnotherChild()
            If Me.View.CurrentStep = IViewAddTask.ViewStep.SetProperty Then
                Dim oTask As Task = Me.View.GetTaskChild
                SaveTaskInSession(oTask)
            ElseIf Me.View.CurrentStep = IViewAddTask.ViewStep.UserManagement Then
                Me.View.dtoReallocateTAToAdd = Me.View.GetUserFromResumeWithModify
            End If
            Me.InitSetChildProperty()
        End Sub

        Public Sub ChangeParent()
            Me.View.CurrentStep = IViewAddTask.ViewStep.ChangeParent
            Me.View.InitButton()
            Me.View.InitSelectParentMap()
        End Sub

        Public Sub NextStep()
            Select Case Me.View.CurrentStep
                Case IViewAddTask.ViewStep.SelectParent
                    Me.View.ParentID = Me.View.GetParentID
                    If Me.View.ParentID > 0 Then
                        Me.InitSetChildProperty()
                    Else
                        Reload()
                    End If

                Case IViewAddTask.ViewStep.SetProperty
                    If Me.View.AddAnotherChildClicked Then
                        SaveTaskInSession(Me.View.GetTaskChild)
                        Me.View.AddAnotherChildClicked = False
                    End If
                    Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                    Me.InitResumeTasksWithModifyResources(ListOfDto, False)

                Case IViewAddTask.ViewStep.AddUser
                    AddDtoReallocateTAToAdd()
                    Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                    Me.InitResumeTasksWithModifyResources(ListOfDto, False)

                Case IViewAddTask.ViewStep.UserManagement
                    Me.View.dtoReallocateTAToAdd = Me.View.GetUserFromResumeWithModify
                    Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                    If ExistManagerForAllChildren(ListOfDto) Then
                        Me.InitFinalResumeTask()
                    Else
                        Me.InitResumeTasksWithModifyResources(ListOfDto, True)
                    End If

                Case IViewAddTask.ViewStep.ChangeParent
                    Me.View.ParentID = Me.View.GetParentID
                    If Me.View.ParentID > 0 Then
                        Me.SetDdtoReallocateTAToAddAfterChangeParent()
                        Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                        Me.InitResumeTasksWithModifyResources(ListOfDto, False)
                    Else
                        Reload()
                    End If

                Case IViewAddTask.ViewStep.UpdateChildProperties
                    Me.UpdateChildProperties(Me.View.GetTaskChild())
                    Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                    Me.InitResumeTasksWithModifyResources(ListOfDto, False)
            End Select

        End Sub

        Private Sub UpdateChildProperties(ByVal oTask As Task)
            Dim i As Integer
            For i = 0 To Me.View.TasksToAdd.Count - 1
                If Me.View.TasksToAdd.ElementAt(i).ID = oTask.ID Then
                    Exit For
                End If
            Next
            Me.View.TasksToAdd.RemoveAt(i)
            Me.View.TasksToAdd.Insert(i, oTask)
        End Sub

        Public Sub InitUpdateChildProperties(ByVal ChildID As Long, ByVal ListOfNewDtoReallocateTAToAdd As List(Of dtoReallocateTA))
            Me.View.dtoReallocateTAToAdd = ListOfNewDtoReallocateTAToAdd
            Me.View.CurrentStep = IViewAddTask.ViewStep.UpdateChildProperties
            Me.View.InitButton()
            Dim InterstedChild As Task = (From t In Me.View.TasksToAdd Where t.ID = ChildID Select t).First
            Dim dtoChild As New dtoTaskDetailWithPermission(New dtoTaskDetail(InterstedChild), TaskPermissionEnum.None)
            dtoChild.dtoTaskDetail.TaskWBS = ""
            Dim ParentName As String = GetParentName(Me.View.dtoParentProperty.dtoTaskDetail.TaskWBS, Me.View.dtoParentProperty.dtoTaskDetail.TaskName)
            Me.View.InitSetTaskProperty(ParentName, dtoChild, IViewUC_TaskDetail.viewDetailType.Update)
        End Sub

        Private Sub InitFinalResumeTask()
            Me.View.CurrentStep = IViewAddTask.ViewStep.FinalResumeTasks
            Me.View.InitButton()
            Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(True)
            Dim ParentName As String = GetParentName(Me.View.dtoParentProperty.dtoTaskDetail.TaskWBS, Me.View.dtoParentProperty.dtoTaskDetail.TaskName)
            Me.View.InitFinalResumeTasks(ParentName, ListOfDto)
        End Sub

        Private Sub InitResumeTasksWithModifyResources(ByVal ListOfDto As List(Of dtoReallocateTAWithHeader), ByVal ShowManagerError As Boolean)
            Me.View.CurrentStep = IViewAddTask.ViewStep.UserManagement
            Me.View.InitButton()
            Dim ParentName As String = GetParentName(Me.View.dtoParentProperty.dtoTaskDetail.TaskWBS, Me.View.dtoParentProperty.dtoTaskDetail.TaskName)
            Me.View.InitResumeTasksWithModifyResources(ParentName, ListOfDto, ShowManagerError)
        End Sub

        Private Function GetParentName(ByRef WBS As String, ByRef Name As String) As String
            If WBS = "0" Then
                Return Name
            Else
                Return WBS & " " & Name
            End If
        End Function


        Private Function GetListOfDtoReallocateTAWhithHeader(ByVal OnlyActiveVirtualAssignments As Boolean)
            Dim ListOfDto As List(Of dtoReallocateTAWithHeader)
            If OnlyActiveVirtualAssignments Then
                ListOfDto = (From o In Me.View.TasksToAdd Select New dtoReallocateTAWithHeader(o, (From a In Me.View.dtoReallocateTAToAdd Where a.TaskID = o.ID And Not a.isDeleted Select a).ToList)).ToList
            Else
                ListOfDto = (From o In Me.View.TasksToAdd Select New dtoReallocateTAWithHeader(o, (From a In Me.View.dtoReallocateTAToAdd Where a.TaskID = o.ID Select a).ToList)).ToList
            End If
            Return ListOfDto
        End Function

        Private Function ExistManagerForAllChildren(ByVal ListOfDto As List(Of dtoReallocateTAWithHeader))
            Dim ExistManager As Boolean = True
            Dim ManagerNumber As Integer
            For Each item In ListOfDto
                ManagerNumber = (From o In item.TaskAssignments Where o.Role = TaskRole.Manager And Not o.isDeleted Select o).Count
                If ManagerNumber = 0 Then
                    ExistManager = False
                    Exit For
                End If
            Next
            Return ExistManager
        End Function

        Public Sub InitAddVirtualAssignments(ByVal InterestedTaskID As Long, ByVal ListOfNewDtoReallocateTAToAdd As List(Of dtoReallocateTA))
            Me.View.dtoReallocateTAToAdd = ListOfNewDtoReallocateTAToAdd
            Dim CommunityID As Integer = Me.CurrentTaskManager.GetCommunityID(Me.View.ParentID)
            Me.View.CurrentStep = IViewAddTask.ViewStep.AddUser
            Me.View.InitButton()
            Dim TaskName As String = (From t In Me.View.TasksToAdd Where t.ID = InterestedTaskID Select t.Name).First
            Dim ListOfVirtualAssignment As List(Of dtoUserWithRole) = (From o In Me.View.dtoReallocateTAToAdd Where o.TaskID = InterestedTaskID Select New dtoUserWithRole(o)).ToList
            Me.View.InitAddVirtualTaskAssignment(InterestedTaskID, TaskName, CommunityID, ListOfVirtualAssignment)
        End Sub

        Private Sub AddDtoReallocateTAToAdd()
            Dim ListOfDtoReallocateTAToAdd As List(Of dtoReallocateTA) = Me.View.GetVirtualTaskAssignmentToAdd
            Me.View.CurrentStep = IViewAddTask.ViewStep.AddUser
            Me.View.InitButton()
            Me.View.dtoReallocateTAToAdd.AddRange(ListOfDtoReallocateTAToAdd)

            Me.View.dtoReallocateTAToAdd = (From dto In Me.View.dtoReallocateTAToAdd Select dto).Distinct(New dtoReallocateTACompare()).ToList
            ' Me.View.dtoReallocateTAToAdd = (From dto In Me.View.dtoReallocateTAToAdd Group By dto.PersonID, dto.Role, dto.TaskID Into g = Group Select g).ToList
        End Sub

        Public Sub Previus()

            Select Me.View.CurrentStep

                Case IViewAddTask.ViewStep.SetProperty

                    Me.View.CurrentStep = IViewAddTask.ViewStep.SelectParent
                    Me.View.InitButton()
                    Me.View.InitSelectParentMap()

                Case IViewAddTask.ViewStep.AddUser
                    'da UC Seleziona utenti, a User Management

                    If Me.View.AddAnotherChildClicked Then
                        SaveTaskInSession(Me.View.GetTaskChild)
                        Me.View.AddAnotherChildClicked = False
                    End If
                    Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                    Me.InitResumeTasksWithModifyResources(ListOfDto, False)


                    'Case IViewAddTask.ViewStep.UserManagement
                    ' non serve piu perche c'è il modifica apposito
                    'Me.InitSetChildProperty()

                Case IViewAddTask.ViewStep.FinalResumeTasks
                    'da Riepilogo a seleziona utenti

                    'DEVO TORNARE AD ADD User

                    Dim ListOfDto As List(Of dtoReallocateTAWithHeader) = GetListOfDtoReallocateTAWhithHeader(False)
                    Me.InitResumeTasksWithModifyResources(ListOfDto, False)
                    Me.View.CurrentStep = IViewAddTask.ViewStep.UserManagement

                Case IViewAddTask.ViewStep.ChangeParent
                    Me.SelectFirstStep()

                Case IViewAddTask.ViewStep.UpdateChildProperties
                    Me.InitSetChildProperty()

            End Select

        End Sub

        Private Function SaveTaskInSession(ByVal oTask As Task)
            If IsNothing(oTask) Then
                Return False
            Else
                oTask.ID = TimeOfDay.Ticks
                Me.View.TasksToAdd.Add(oTask)
                For Each item In Me.View.dtoParentReallocateTA
                    Dim tempDtoReallocate As New dtoReallocateTA(item)
                    tempDtoReallocate.TaskID = oTask.ID
                    Me.View.dtoReallocateTAToAdd.Add(tempDtoReallocate)
                Next
                Return True
            End If
        End Function

        Public Sub CancelAddChildrenOperation()
            Me.View.ClearUniqueKey()
            Me.View.GoBackPage(COL_BusinessLogic_v2.UCServices.Services_TaskList.ActionType.AnnulAddTasks)
        End Sub

        Public Sub PersistData()
            Dim ListDtoReallocateTAToAdd As List(Of dtoReallocateTA) = (From o In Me.View.dtoReallocateTAToAdd Where Not o.isDeleted Select o).ToList
            Dim ListOfTaskID As List(Of Long) = Me.CurrentTaskManager.AddTaskChildren(Me.View.TasksToAdd, ListDtoReallocateTAToAdd, Me.View.ParentID, Me.AppContext.UserContext.CurrentUser)
            Me.View.ClearUniqueKey()
            Me.View.GoBackPage(COL_BusinessLogic_v2.UCServices.Services_TaskList.ActionType.TaskAdded, ListOfTaskID)
        End Sub

    End Class
End Namespace