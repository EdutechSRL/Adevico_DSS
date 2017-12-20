Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.Comunita

Partial Public Class ReallocateUser
    Inherits PageBase
    Implements IViewReallocateUsers

    Private _presenter As ReallocateUsersPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#Region " Base"


    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub BindDati()

        Me.Master.ShowNoPermission = False
        If Not IsPostBack Then
            Me.CurrentPresenter.InitView()
            Dim Action As Services_TaskList.ActionType
            If Me.CurrentModeType = IViewReallocateUsers.ModeType.Undelete Then
                Action = Services_TaskList.ActionType.StartUnDeleteWithReallocateResource
            ElseIf Me.CurrentModeType = IViewReallocateUsers.ModeType.VirtualDelete Then
                Action = Services_TaskList.ActionType.StartVirtualDeleteWithReallocateResource
            End If
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Action, , InteractionType.UserWithLearningObject)
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ReallocateUsers", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setLabel_To_Value(LBselectResource, "LBselectResource." & Me.CurrentModeType.ToString)
            .setLabel_To_Value(LBresumeUsers, "LBresumeUsers." & Me.CurrentModeType.ToString)
            .setLabel_To_Value(LBtitoloSuperiore, "LBtitoloSuperiore." & Me.CurrentModeType.ToString)
            Dim oButton As Button
            oButton = Me.WZRreallocateUsers.FindControl("StartNavigationTemplateContainerID").FindControl("NextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRreallocateUsers.FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRreallocateUsers.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRreallocateUsers.FindControl("FinishNavigationTemplateContainerID").FindControl("PreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRreallocateUsers.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property

    Private ReadOnly Property CurrentService() As Services_TaskList
        Get
            If IsNothing(_Servizio) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _Servizio = Services_TaskList.Create
                    With _Servizio
                        '. = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
                        '.Read = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                        '.GrantPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        '.Write = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)

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

    Public ReadOnly Property CurrentPresenter() As ReallocateUsersPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New ReallocateUsersPresenter(Me.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

#End Region

#Region "IViewProperty"
    Public Property CurrentCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property
    Public ReadOnly Property CurrentModeType() As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.CurrentModeType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewReallocateUsers.ModeType).GetByString(Request.QueryString("CurrentModeType"), IViewReallocateUsers.ModeType.None)
        End Get
    End Property

    Public Property CurrentStep() As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.StepType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.CurrentStep
        Get
            Return Me.ViewState("CurrentStep")
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.StepType)
            Me.ViewState("CurrentStep") = value
        End Set
    End Property

    Public ReadOnly Property CurrentTaskID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.CurrentTaskID
        Get
            Try
                Return CInt(Me.Request.QueryString("CurrentTaskID"))
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property

    Public Property ListOfUsers() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTAWithHeader) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ListOfUsers
        Get
            If Not TypeOf Me.Session("ListOfUsers_" & SessionUniqueKey.ToString) Is List(Of dtoReallocateTAWithHeader) Then
                Me.Session("ListOfUsers_" & SessionUniqueKey.ToString) = New List(Of dtoReallocateTAWithHeader)
            End If
            Return Me.Session("ListOfUsers_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTAWithHeader))
            Me.Session("ListOfUsers_" & SessionUniqueKey.ToString) = value
        End Set
    End Property

    Public Property SessionUniqueKey() As System.Guid Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.SessionUniqueKey
        Get
            If Not TypeOf Me.ViewState("SessionUniqueKey") Is System.Guid Then
                Me.ViewState("SessionUniqueKey") = System.Guid.Empty
            End If
            Return Me.ViewState("SessionUniqueKey")
        End Get
        Set(ByVal value As System.Guid)
            Me.ViewState("SessionUniqueKey") = value
        End Set
    End Property

    Public Property TaskToAssignName() As String Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.TaskToAssignName
        Get
            Return Me.ViewState("TaskToAssignName")
        End Get
        Set(ByVal value As String)
            Me.ViewState("TaskToAssignName") = value
        End Set
    End Property

    Public Property TaskToAssignResourceID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ParentID
        Get
            Return Me.ViewState("TaskToAssignResourceID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("TaskToAssignResourceID") = value
        End Set
    End Property

#End Region

#Region "Function Sub Iview"
    Public Function GetUserFromSelectUsersWithHeader() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTAWithHeader) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.GetUserFromSelectUsersWithHeader
        Return Me.CTRLresourceEditable.GetValidateUsersListWithHeader
    End Function
    Public Function GetUserFromSelectUsers() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTA) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.GetUserFromSelectUsers
        Return Me.CTRLresourceEditable.GetValidateUsersList()
    End Function
    Public Function GetUserFromResumeUsers() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTA) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.GetUserFromResumeUsers
        Return Me.CTRLresourceResume.GetValidateUsersList()
    End Function

    Public Sub InitFinalUserResume(ByVal ListOfdtoOfTaskWithResource As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTAWithHeader)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.InitFinalUserResume
        Me.LBtaskNameResume.Text = Me.TaskToAssignName
        Me.CTRLresourceResume.CurrentPresenter.InitView(ListOfdtoOfTaskWithResource, IViewUC_ReallocateResourcesOnNodes.EditType.Read)
        Me.WZRreallocateUsers.ActiveStepIndex = IViewReallocateUsers.StepType.ResumeUsers
    End Sub

    Public Sub InitSelectUsers(ByVal ListOfdtoOfTaskWithResource As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTAWithHeader)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.InitSelectUsers
        Me.LBtaskNameSelect.Text = Me.TaskToAssignName
        Me.CTRLresourceEditable.CurrentPresenter.InitView(ListOfdtoOfTaskWithResource, IViewUC_ReallocateResourcesOnNodes.EditType.EditNoButton)
        Me.WZRreallocateUsers.ActiveStepIndex = IViewReallocateUsers.StepType.SelectUsers
    End Sub

    Public Sub ShowError(ByVal ErrorString As String) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ShowError
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.LBerror.Text = ErrorString
        Me.MLVreallocateUsers.SetActiveView(Me.VIWerror)
    End Sub
    Public Sub ClearUniqueKey() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ClearUniqueKey
        Me.Session("ListOfUsers_" & Me.SessionUniqueKey.ToString) = Nothing
    End Sub
#End Region

    Private Sub WZRreallocateUsers_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRreallocateUsers.CancelButtonClick
        Me.CurrentPresenter.ClearUniqueKey()
        Dim Action As Services_TaskList.ActionType
        If Me.CurrentModeType = IViewReallocateUsers.ModeType.Undelete Then
            Action = Services_TaskList.ActionType.AnnulUnDeleteWithReallocateResource
        ElseIf Me.CurrentModeType = IViewReallocateUsers.ModeType.VirtualDelete Then
            Action = Services_TaskList.ActionType.AnnulVirtualDeleteWithReallocateResource
        End If
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Action, , InteractionType.UserWithLearningObject)
        Me.GoBack()
    End Sub

    Private Sub WZRreallocateUsers_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRreallocateUsers.FinishButtonClick
        Me.CurrentPresenter.NextStep()
    End Sub

    Private Sub WZRreallocateUsers_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRreallocateUsers.NextButtonClick
        Me.CurrentPresenter.NextStep()
    End Sub

    Private Sub WZRreallocateUsers_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRreallocateUsers.PreviousButtonClick
        Me.CurrentPresenter.PreviusStep()
    End Sub

    Private Sub GoBack()
        Dim Url As String
        Select Case PreviusPage
            Case IViewReallocateUsers.PreviusPageName.AssignedTasks
                Url = "TaskList/AssignedTasks.aspx?View=" & ViewModeType.TodayTasks.ToString
            Case IViewReallocateUsers.PreviusPageName.ManageTaskAssignment
                Url = "TaskList/TasksManagement.aspx?View=" & ViewModeType.TasksManagement.ToString
            Case IViewReallocateUsers.PreviusPageName.InvolvingProject
                Url = "TaskList/InvolvingProjects.aspx?View=" & ViewModeType.InvolvingProjects.ToString
            Case Else
                Url = "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.CurrentTaskID
        End Select
        Me.PageUtility.RedirectToUrl(Url)
    End Sub

    Public ReadOnly Property PreviusPage() As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.PreviusPageName Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.PreviusPage
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewReallocateUsers.PreviusPageName).GetByString(Request.QueryString("PreviusPage"), IViewReallocateUsers.PreviusPageName.TaskMap)
        End Get
    End Property

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub


    Public Sub GoBackPage(ByVal ListOfReallocateTA As System.Collections.Generic.List(Of Long)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.GoBackPage
        Dim UserAction As Services_TaskList.ActionType
        Dim TaskAction As Services_TaskList.ActionType
        If Me.CurrentModeType = IViewReallocateUsers.ModeType.Undelete Then
            UserAction = Services_TaskList.ActionType.FinishUnDeleteWithReallocateResource
            TaskAction = Services_TaskList.ActionType.FinishUndeleteTask
        ElseIf Me.CurrentModeType = IViewReallocateUsers.ModeType.VirtualDelete Then
            UserAction = Services_TaskList.ActionType.FinishVirtualDeleteWithReallocateResource
            TaskAction = Services_TaskList.ActionType.FinishVirtualDeleteTask
        End If
        For Each TaskAssignmentID As Long In ListOfReallocateTA
            Me.PageUtility.AddAction(Me.CurrentCommunityID, UserAction, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.TaskAssignment, TaskAssignmentID.ToString), InteractionType.UserWithLearningObject)
        Next
        Me.PageUtility.AddAction(Me.CurrentCommunityID, TaskAction, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Task, Me.CurrentTaskID.ToString), InteractionType.UserWithLearningObject)
        GoBack()
    End Sub
End Class