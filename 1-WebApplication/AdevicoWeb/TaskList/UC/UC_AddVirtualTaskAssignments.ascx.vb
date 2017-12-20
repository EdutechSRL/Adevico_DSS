Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports Comol.Entity
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita

Partial Public Class UC_AddVirtualTaskAssignments
    Inherits BaseControlSession
    Implements IviewUC_AddVirtualTaskAssignments

    Private _Presenter As AddVirtualTaskAssignmentUCPresenter
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
    Public ReadOnly Property CurrentPresenter() As AddVirtualTaskAssignmentUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddVirtualTaskAssignmentUCPresenter(Me.CurrentContext, Me)
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
        With Me.Resource
            .setLabel(LBrole)

        End With
    End Sub
#End Region

#Region "Permessi"
    Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IviewUC_AddVirtualTaskAssignments.ModulePersmission
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
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.ModuleCommunityPermission(Of lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList)) Implements IviewUC_AddVirtualTaskAssignments.CommunitiesPermission
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

    Public Property CurrentCommunityID() As Integer Implements IviewUC_AddVirtualTaskAssignments.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property



    Public Property CurrentRole() As TaskRole Implements IviewUC_AddVirtualTaskAssignments.CurrentRole
        Get
            Return Me.ViewState("CurrentRole")
        End Get
        Set(ByVal value As TaskRole)
            Me.ViewState("CurrentRole") = value
        End Set
    End Property


    Public Property CurrentTaskID() As Long Implements IviewUC_AddVirtualTaskAssignments.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property



    Public Property ListOfCommunityID() As List(Of Integer) Implements IviewUC_AddVirtualTaskAssignments.ListOfCommunityID
        Get
            Return Me.ViewState("ListOfCommunityID")
        End Get
        Set(ByVal value As List(Of Integer))
            Me.ViewState("ListOfCommunityID") = value
        End Set
    End Property

    Public Property ListOfAssignedPersonWithRole() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoUserWithRole) Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewUC_AddVirtualTaskAssignments.ListOfAssignedPersonWithRole
        Get
            Return Me.ViewState("ListOfAssignedPersonWithRole")
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoUserWithRole))
            Me.ViewState("ListOfAssignedPersonWithRole") = value
        End Set
    End Property

#End Region

#Region "IView Function and Sub"
    Sub InitUserSelection(ByVal ListOfPersonIDToHide As List(Of Integer)) Implements IviewUC_AddVirtualTaskAssignments.InitUserSelection
        Me.CTRLsearchUser.CurrentPresenter.Init(Me.ListOfCommunityID, ListSelectionMode.Multiple, ListOfPersonIDToHide)
    End Sub

    Public Function GetSelectedUser() As Object Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewUC_AddVirtualTaskAssignments.GetSelectedUser

        Return Me.CTRLsearchUser.CurrentPresenter.GetConfirmedUsers()
    End Function

    Protected Sub DDLrole_SelectIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDLrole.SelectedIndexChanged
        Me.CurrentPresenter.Reload()

    End Sub

    Public Sub LoadRole(ByVal listOfRole As List(Of TaskRole)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewUC_AddVirtualTaskAssignments.LoadRole
        Me.DDLrole.Items.Clear()
        For Each role As TaskRole In listOfRole
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue(role.ToString & ".DDLRole")
            oItem.Value = role.ToString
            Me.DDLrole.Items.Add(oItem)
        Next
    End Sub

    Public Function GetCurretnRole() Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewUC_AddVirtualTaskAssignments.GetCurretnRole
        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskRole).GetByString(Me.DDLrole.SelectedValue, TaskRole.None)

    End Function



#End Region




End Class