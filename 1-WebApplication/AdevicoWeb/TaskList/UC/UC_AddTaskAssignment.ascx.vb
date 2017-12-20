Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports COL_BusinessLogic_v2.Comunita



Partial Public Class UC_AddTaskAssignment
    Inherits BaseControlSession
    Implements IviewUC_AddTaskAssignment


    Private _Presenter As AddTaskAssignmentUCPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As AddTaskAssignmentUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddTaskAssignmentUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Base"

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_AddTaskAssignment", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBrole)
            .setCheckBox(CKBisResource)
        End With
    End Sub
#End Region

#Region "Permessi"
    Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IviewUC_AddTaskAssignment.ModulePersmission
        Get
            Return TranslateComolPermissionToModulePermission(Me.CurrentService)
        End Get
    End Property

    Private Function TranslateComolPermissionToModulePermission(ByVal oService As Services_TaskList) As ModuleTaskList
        Dim oModulePermission As New ModuleTaskList
        With oService
            'oModulePermission.DeleteMessage = .Admin OrElse .Write
            'oModulePermission.EditMessage = .Admin OrElse .Write
            'oModulePermission.ManagementPermission = .GrantPermission
            'oModulePermission.PrintMessage = .Read OrElse .Write OrElse .Admin
            'oModulePermission.RetrieveOldMessage = .Write OrElse .Admin
            'oModulePermission.ServiceAdministration = .Admin OrElse .Write
            'oModulePermission.ViewCurrentMessage = .Read OrElse .Write OrElse .Admin
            'oModulePermission.ViewOldMessage = .Read OrElse .Write OrElse .Admin
        End With
        Return oModulePermission
    End Function

    Private ReadOnly Property CurrentService() As Services_TaskList
        Get
            If IsNothing(_Servizio) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _Servizio = Services_TaskList.Create
                    With _Servizio
                        .AddCommunityProject = False
                        .AddPersonalProject = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                        .Administration = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ViewCommunityProjects = (PersonTypeID <> Main.TipoPersonaStandard.Guest)

                    End With
                ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_TaskList(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_TaskList.Codex))
                Else
                    _Servizio = Me.PageUtility.GetCurrentServices.Find(Services_TaskList.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = Services_TaskList.Create
                    End If
                End If
            End If
            Return _Servizio
        End Get
    End Property

    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.ModuleCommunityPermission(Of lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList)) Implements IviewUC_AddTaskAssignment.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                Dim oList As New List(Of ModuleCommunityPermission(Of ModuleTaskList))
                Dim PermissionsList As IList(Of ServiceBase) = ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_TaskList.Codex)

                For Each oPermission As ServiceBase In PermissionsList
                    oList.Add(New ModuleCommunityPermission(Of ModuleTaskList)() With {.ID = oPermission.CommunityID, .Permissions = TranslateComolPermissionToModulePermission(New Services_TaskList(oPermission.PermissionString))})
                Next
                _CommunitiesPermission = oList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
#End Region

#Region "IViewProperty"

    Public Property CurrentCommunityID() As Integer Implements IviewUC_AddTaskAssignment.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property

    Public Property CurrentRole() As TaskRole Implements IviewUC_AddTaskAssignment.CurrentRole
        Get
            Return Me.ViewState("CurrentRole")
        End Get
        Set(ByVal value As TaskRole)
            Me.ViewState("CurrentRole") = value
        End Set
    End Property

    Public Property CurrentTaskID() As Long Implements IviewUC_AddTaskAssignment.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property

    Public Property ListOfCommunityID() As List(Of Integer) Implements IviewUC_AddTaskAssignment.ListOfCommunityID
        Get
            Return Me.ViewState("ListOfCommunityID")
        End Get
        Set(ByVal value As List(Of Integer))
            Me.ViewState("ListOfCommunityID") = value
        End Set
    End Property

    Public Property isChild() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewUC_AddTaskAssignment.isChild
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

#End Region

#Region "IView Function and Sub"


    Sub InitUserSelection(ByVal ListOfPersonIDToHide As List(Of Integer)) Implements IviewUC_AddTaskAssignment.InitUserSelection

        Dim listOfRole As List(Of TaskRole)
        listOfRole = Me.CurrentPresenter.LoadRoleList
        For Each role As TaskRole In listOfRole
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue(role.ToString & ".DDLItem")
            oItem.Value = role.ToString
            Me.DDLrole.Items.Add(oItem)
        Next
        Me.CKBisResource.Visible = Me.isChild
        Me.CKBisResource.Checked = False
        Me.CTRLsearchUser.CurrentPresenter.Init(Me.ListOfCommunityID, ListSelectionMode.Multiple, ListOfPersonIDToHide)
        'Parte default Button
        Me.SetDefaultButton()

    End Sub

    Public Sub SetDefaultButton()
        Me.Page.Form.DefaultButton = Me.CTRLsearchUser.SearchButtonUniqueID
        Me.Page.Form.DefaultFocus = Me.CTRLsearchUser.SearchDefaultTextField
    End Sub

    Public Function GetSelectedUser() As Object Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewUC_AddTaskAssignment.GetSelectedUser
        Dim ListOfPersonToAssign As New List(Of Integer)
        Dim ListOfSelectedMembers As List(Of MemberContact)
        Dim RoleName As String = Me.DDLrole.SelectedValue
        ListOfSelectedMembers = Me.CTRLsearchUser.CurrentPresenter.GetConfirmedUsers()
        If ListOfSelectedMembers.Count > 0 Then
            For Each member As MemberContact In ListOfSelectedMembers
                ListOfPersonToAssign.Add(member.Id)
            Next

        End If
        Return ListOfPersonToAssign
    End Function

    Protected Sub DDLrole_SelectIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDLrole.SelectedIndexChanged
        Dim ListPersonToHide As List(Of Integer)
        Me.CurrentRole = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskRole).GetByString(Me.DDLrole.SelectedValue, TaskRole.None)
        ListPersonToHide = Me.CurrentPresenter.GetPersonsToHide()
        Me.CKBisResource.Visible = (Me.CurrentRole = TaskRole.Manager) And Me.isChild

        Me.CTRLsearchUser.CurrentPresenter.Init(Me.ListOfCommunityID, ListSelectionMode.Multiple, ListPersonToHide)
        Me.SetDefaultButton()
    End Sub

#End Region

    Public Function GetIfManagerIsResource() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewUC_AddTaskAssignment.GetIfManagerIsResource
        Return Me.CKBisResource.Checked
    End Function
End Class