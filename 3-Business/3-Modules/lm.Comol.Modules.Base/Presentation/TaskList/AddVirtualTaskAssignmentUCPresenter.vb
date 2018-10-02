Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports Comol.Entity
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Modules.TaskList.Business


Namespace lm.Comol.Modules.Base.Presentation.TaskList

    Public Class AddVirtualTaskAssignmentUCPresenter
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
        Public Overloads ReadOnly Property View() As IviewUC_AddVirtualTaskAssignments
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewUC_AddVirtualTaskAssignments)
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

        Private Function GetRoleList()
            Dim listRole As New List(Of TaskRole)
            listRole.Add(TaskRole.Manager)
            listRole.Add(TaskRole.Resource)
            listRole.Add(TaskRole.Visitor)
            Return listRole
        End Function




        Public Function GetDtoReallocateTAforSelectedUser()
            Dim ListOfDto As New List(Of dtoReallocateTA)
            Dim ListOfMembersContact As List(Of MemberContact) = Me.View.GetSelectedUser
            'For Each item In ListOfMembersContact
            '    ListOfDto.Add(New dtoReallocateTA(Me.View.CurrentTaskID, Me.View.CurrentRole, item.Id, item.Surname & " " & item.Name))
            'Next
            ListOfDto.AddRange((From item In ListOfMembersContact Select New dtoReallocateTA(Me.View.CurrentTaskID, Me.View.CurrentRole, item.Id, item.Surname & " " & item.Name)).ToList())
            Return ListOfDto
        End Function


        Public Sub Reload()
            Me.View.CurrentRole = Me.View.GetCurretnRole
            InitUserSelection()
        End Sub

        Public Sub InitView(ByVal CurrentTaskID As Long, ByVal CurrentCommunityID As Integer, ByVal ListOfAssignedPersonWithRole As List(Of dtoUserWithRole))
            Me.View.CurrentTaskID = CurrentTaskID
            Me.View.ListOfAssignedPersonWithRole = ListOfAssignedPersonWithRole
            Me.View.CurrentCommunityID = CurrentCommunityID
            Me.View.CurrentRole = TaskRole.Manager
            Me.View.LoadRole(GetRoleList)
            Me.InitUserSelection()
        End Sub

        Private Sub InitUserSelection()
            Dim listOfPersonIDToHide As New List(Of Integer)
            Dim listtemp As New List(Of Integer)
            Me.View.ListOfCommunityID = listtemp
            If Me.View.CurrentCommunityID = 0 Then 'TEMPORANEA!!! CON IL PORTALE METTE SEMPRE LA COM 10!!!!!
                Me.View.CurrentCommunityID = -1
            End If

            Me.View.ListOfCommunityID.Add(Me.View.CurrentCommunityID)
            listOfPersonIDToHide = GetPersonsToHide()
            Me.View.InitUserSelection(listOfPersonIDToHide)

        End Sub


        Public Function GetPersonsToHide()
            Dim listOfPersonIDToHide As List(Of Integer) = (From o In Me.View.ListOfAssignedPersonWithRole Where o.Role = Me.View.CurrentRole Select o.PersonID).ToList
            Return listOfPersonIDToHide
        End Function



    End Class
End Namespace