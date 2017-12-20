Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.Comunita

Public Class UC_QuickSelectionTaskUsers
    Inherits BaseControlSession
    Implements IViewUC_QuickSelectionTaskUsers

    Private _Presenter As QuickSelectionTaskUsersUCPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Public ReadOnly Property CurrentPresenter() As QuickSelectionTaskUsersUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New QuickSelectionTaskUsersUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#Region "Base"
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_QuickSelectionTaskUsers", "TaskList")
        'SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBquickRole)
        End With
    End Sub
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            SetInternazionalizzazione()

        End If
    End Sub


    Public Property isChild() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_QuickSelectionTaskUsers.isChild
        Get
            Dim x As Boolean = Me.ViewState("isChild")
            If IsNothing(x) Then
                Return False
            Else
                Return x
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isChild") = value
        End Set
    End Property


    Public Sub LoadQuickSelUsers(ByVal list As List(Of dtoUsers)) Implements IViewUC_QuickSelectionTaskUsers.LoadQuickSelUsers

        Me.CBLquickSelection.DataSource = list
        Me.CBLquickSelection.DataBind()
        Me.CBLquickSelection.Visible = True
        Me.DDLquickRole.Visible = True

    End Sub

    Public Sub LoadRoles(ByVal list As List(Of TaskRole)) Implements IViewUC_QuickSelectionTaskUsers.LoadRoles

        Me.DDLquickRole.Items.Clear()
        For Each oRole As TaskRole In List
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("DDLquickRole." & oRole)
            oItem.Value = oRole
            Me.DDLquickRole.Items.Add(oItem)
        Next

    End Sub

    Public Sub InitUcParametersAndInitView(ByVal TaskID As Long)
        Me.CurrentTaskID = TaskID
        Me.PreLoadedRole = TaskRole.Manager
        Me.CurrentPresenter.InitView(TaskID)
    End Sub

    Public Property CurrentRole() As TaskRole Implements IViewUC_QuickSelectionTaskUsers.CurrentRole
        Get
            Return Me.DDLquickRole.SelectedValue   'lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskRole).GetByString(Me.DDLquickRole.SelectedValue, TaskRole.Manager)  'Me.DDLquickRole.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TaskRole)
            Me.DDLquickRole.SelectedValue = value
        End Set
    End Property

    Public Property PreLoadedRole() As TaskRole Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_QuickSelectionTaskUsers.PreLoadedRole
        Get
            Return Me.ViewState("PreLoadedRole")
            'Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskRole).GetByString(Request.QueryString("Role"), TaskRole.Manager)
        End Get
        Set(ByVal value As TaskRole)
            Me.ViewState("PreLoadedRole") = value
            'PreLoadedRole = value
        End Set
    End Property

    Protected Sub DDLquickRole_SelectIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDLquickRole.SelectedIndexChanged
        'Me.CurrentRole = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskRole).GetByString(Me.DDLquickRole.SelectedValue, TaskRole.None)
        Me.CurrentPresenter.LoadQuickUsers()
    End Sub

    Public Property CurrentTaskID() As Long Implements IViewUC_QuickSelectionTaskUsers.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property

    Public Function GetQuickSelectionUsers() As List(Of Person) Implements IViewUC_QuickSelectionTaskUsers.GetQuickSelectionUsers
        'Dim list As List(Of Integer) = New List(Of Integer)
        Dim listP As List(Of Person) = New List(Of Person)
        Dim chbx As ListItem

        For Each chbx In CBLquickSelection.Items
            If chbx.Selected = True Then
                'list.Add(chbx.Value)
                listP.Add(Me.CurrentPresenter.CurrentTaskManager.GetPerson(chbx.Value))
            End If
        Next

        Return listP
    End Function

    Public Sub SaveQuickTaskAssignments()
        Dim i As Integer
        Dim list As New List(Of Long)
        For i = 0 To CBLquickSelection.Items.Count - 1

            If Me.CBLquickSelection.Items.Item(i).Selected Then
                list.Add(CBLquickSelection.Items.Item(i).Value)
            End If

        Next
    End Sub

    Public Function GetSelectedUser(ByVal oRole As String) As Object Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_QuickSelectionTaskUsers.GetSelectedUser
        Dim ListOfPersonToAssign As New List(Of Integer)
        'Dim ListOfSelectedMembers As List(Of dtoUsersOnQuickSelection)
        'Dim RoleName As String = oRole
        'ListOfSelectedMembers = Me.GetQuickSelectionUsers()
        'If ListOfSelectedMembers.Count > 0 Then
        '    For Each member As MemberContact In ListOfSelectedMembers
        '        ListOfPersonToAssign.Add(member.Id)
        '    Next

        'End If
        Return ListOfPersonToAssign
    End Function

    Public Sub RoleRefreshesUsersList()
        'Me.CurrentRole = Me.DDLquickRole.SelectedValue
        Me.CurrentPresenter.LoadQuickUsers()
    End Sub

    Public Sub ReloadFromExternalPage(ByVal TaskID As Long, ByVal Role As TaskRole)
        'Me.CurrentRole = Me.DDLquickRole.SelectedValue.ToString
        Me.CurrentPresenter.InitView(CurrentTaskID) 'LoadQuickUsers(Role, TaskID)
    End Sub

    Public Sub NavigationUrl(ByVal Role As lm.Comol.Modules.TaskList.Domain.TaskRole, ByVal oTaskID As Long) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_QuickSelectionTaskUsers.NavigationUrl ' ByVal ViewMode As lm.Comol.Modules.Base.Presentation.TaskList.ViewModeType
        ' Me.PageUtility.RedirectToUrl(Me.BaseUrl & "TaskList/ManageTaskAssignment.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&ViewToLoad=None" & "&ViewType=" & IViewManageTaskAssignment.viewAssignmentType.AddQuickTaskAssignment.ToString)
        Me.PageUtility.RedirectToUrl("TaskList/ManageTaskAssignment.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&ViewToLoad=None" & "&ViewType=" & IViewManageTaskAssignment.viewAssignmentType.AddQuickTaskAssignment.ToString)

    End Sub

    'Sub InitView(ByVal p1 As Long)
    '    Throw New NotImplementedException
    'End Sub

    
End Class