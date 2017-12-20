Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.Comunita

Partial Public Class ManageTaskAssignment
    Inherits PageBase
    Implements IViewManageTaskAssignment




    Private _presenter As ManageTaskAssignmentPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList

#Region "Permessi"
    Public ReadOnly Property ModulePermission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList Implements IViewManageTaskAssignment.ModulePersmission
        Get
            Return TranslateComolPermissionToModulePermission(Me.CurrentService)
        End Get
    End Property

    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.ModuleCommunityPermission(Of lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList)) Implements IViewManageTaskAssignment.CommunitiesPermission
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

    Public Property CurrentCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewManageTaskAssignment.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property

    Public Property TaskPermission() As TaskPermissionEnum Implements IViewManageTaskAssignment.TaskPermission
        Get
            Return Me.ViewState("TaskPermission")
        End Get
        Set(ByVal value As TaskPermissionEnum)
            Me.ViewState("TaskPermission") = value
        End Set
    End Property

    Public ReadOnly Property CurrentTaskID() As Long Implements IViewManageTaskAssignment.CurrentTaskID
        Get
            Return Request.QueryString("CurrentTaskID")
        End Get
    End Property

    Public ReadOnly Property ViewModeType() As ViewModeType Implements IViewManageTaskAssignment.ViewModeType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("ViewToLoad"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public ReadOnly Property ViewType() As IViewManageTaskAssignment.viewAssignmentType Implements IViewManageTaskAssignment.ViewType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewManageTaskAssignment.viewAssignmentType).GetByString(Request.QueryString("ViewType"), IViewManageTaskAssignment.viewAssignmentType.None)

        End Get
    End Property

    Public ReadOnly Property BackUrl() As String Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewManageTaskAssignment.BackUrl
        Get
            Return "TaskList/TaskDetail.aspx?CurrentTaskID=" & Me.CurrentTaskID.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString
        End Get
    End Property

    Public ReadOnly Property Url() As String Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewManageTaskAssignment.Url
        Get
            Return "TaskList/ManageTaskAssignment.aspx?CurrentTaskID=" & Me.CurrentTaskID.ToString & "&ViewToLoad=None" & "&ViewType=" & Me.ViewType.ToString
        End Get
    End Property

#End Region


#Region "IView Function and Sub"

    Public Sub SetTaskName(ByVal WBS As String, ByVal Name As String) Implements IViewManageTaskAssignment.SetTaskName
        Me.LBtaskName.Text = Name
        Me.LBwbs.Text = WBS
    End Sub

    Public Sub ShowError(ByVal ErrorString As String) Implements IViewManageTaskAssignment.ShowError
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.LBerror.Text = ErrorString
        Me.HYPreturnError.NavigateUrl = Me.BaseUrl & Me.BackUrl
        Me.MLVtaskAssignment.SetActiveView(Me.VIWerror)
    End Sub

    Sub InitUserSelection() Implements IViewManageTaskAssignment.InitUserSelection
        Me.HYPreturn.NavigateUrl = Me.BaseUrl & Me.BackUrl


        If Me.ViewType = ViewType.AddTaskAssignment Then
            Me.CTRLaddUser.CurrentPresenter.InitView(Me.CurrentTaskID)
            Me.MLVtaskAssignment.SetActiveView(Me.VIWaddAssignmment)
        Else
            Me.CTRLquickUsersSelection.InitUcParametersAndInitView(Me.CurrentTaskID)
            Me.MLVtaskAssignment.SetActiveView(Me.VIWquickSelection)
        End If

        'Me.CTRLaddUser.CurrentPresenter.InitView(Me.CurrentTaskID)
        'Me.MLVtaskAssignment.SetActiveView(Me.VIWaddAssignmment) 
    End Sub


    Public Sub InitAssignedPersons() Implements IViewManageTaskAssignment.InitAssignedPersons
        Me.MLVtaskAssignment.SetActiveView(Me.VIWassignedPersons)
    End Sub

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub





#Region " Base"


    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub BindDati()

        Me.Master.ShowNoPermission = False
        If Not IsPostBack Then
            Me.PageUtility.AddAction(Services_TaskList.ActionType.StartManageTaskAssignment, Nothing, InteractionType.UserWithLearningObject)
            Me.CurrentPresenter.InitView()
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
        MyBase.SetCulture("pg_ManageTaskAssignment", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setLabel(LBtitolo)
            .setButton(BTNaddTaskAssignments, True)
            .setHyperLink(HYPreturn, True, True)
            .setHyperLink(HYPreturnError, True, True)
            .setButton(BTNswitchUserSelection, True)
            .setButton(BTNsaveQuickLoad, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get

        End Get
    End Property

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

    Private Function TranslateComolPermissionToModulePermission(ByVal oService As Services_TaskList) As ModuleTaskList
        Dim oModulePermission As New ModuleTaskList
        With oService
            'Tia
            oModulePermission.Administration = .Administration
            oModulePermission.CreateCommunityProject = .AddCommunityProject OrElse .Administration
            'oModulePermission.CreatePersonalCommunityProject = True
            oModulePermission.CreatePersonalProject = True
            oModulePermission.DownloadAllowed = True
            oModulePermission.ManagementPermission = .ManagementPermission OrElse .Administration
            oModulePermission.PrintTaskList = True
            oModulePermission.ViewTaskList = .ViewCommunityProjects

        End With
        Return oModulePermission
    End Function

    Public ReadOnly Property CurrentPresenter() As ManageTaskAssignmentPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New ManageTaskAssignmentPresenter(Me.CurrentContext, Me)
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


    Protected Sub BTNaddTaskAssignments_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNaddTaskAssignments.Click
        Me.CTRLaddUser.CurrentPresenter.SaveTaskAssignment()
        'Dim ListOfTAID As List(Of Long) =
        '       For Each TaskAssignmentID In ListOfTAID
        '           Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.AddedTaskAssignment, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.TaskAssignment, TaskAssignmentID.ToString), InteractionType.UserWithLearningObject)
        '       Next
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_TaskList.ActionType.FinishManageTaskAssignment, , InteractionType.UserWithLearningObject)
        Me.PageUtility.RedirectToUrl(Me.BackUrl)
    End Sub

    Protected Sub BTNswitchUserSelection_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNswitchUserSelection.Click
        If Me.MLVtaskAssignment.GetActiveView.ID = Me.VIWquickSelection.ID Then
            Me.MLVtaskAssignment.SetActiveView(Me.VIWaddAssignmment)
        Else
            Me.MLVtaskAssignment.SetActiveView(Me.VIWquickSelection)
            Me.CTRLquickUsersSelection.InitUcParametersAndInitView(Me.CurrentTaskID) 'LoadQuickSelUsers() 'InitView(Me.CurrentTaskID) '
        End If
        'controllare che funzi anche con le altre VIW T.
    End Sub

    Protected Sub BTNsaveQuickLoad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNsaveQuickLoad.Click
        Me.CTRLquickUsersSelection.CurrentPresenter.SaveTaskAssignment()
        Me.CTRLquickUsersSelection.CurrentPresenter.LoadQuickUsers()
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub


End Class