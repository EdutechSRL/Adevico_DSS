Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports COL_BusinessLogic_v2.Comunita

Partial Public Class AddTask
    Inherits PageBase
    Implements IViewAddTask


    Private _presenter As AddTaskPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList


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
            Me.PageUtility.AddAction(Services_TaskList.ActionType.StartAddTasks, , InteractionType.UserWithLearningObject)
        Else
            Me.CurrentPresenter.Reload()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AddTask", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setLabel(LBtitoloSuperiore)
            .setLabel(LBselectParent)
            .setLabel(LBtaskProperties)
            .setLabel(LBmanageResource)
            .setLabel(LBaddUsersTitle)
            .setLabel(LBresume)
            .setLabel(LBerror)
            .setLabel(LBmanagerError)
            .setHyperLink(HYPreturnToTaskList, True, True)
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

    Public ReadOnly Property CurrentPresenter() As AddTaskPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New AddTaskPresenter(Me.CurrentContext, Me)
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



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Iview property"

    Public ReadOnly Property BackUrl() As String Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.BackUrl
        Get
            Dim PreviusPage As String = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewAddTask.PreviusPage).GetByString(Request.QueryString("PreviusPage"), IViewAddTask.PreviusPage.TaskMap).ToString
            Select Case PreviusPage
                Case IViewAddTask.PreviusPage.DeatailReadOnly.ToString
                    Return "TaskList/TaskDetail.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString
                Case IViewAddTask.PreviusPage.DetailUpdate.ToString
                    Return "TaskList/TaskDetail.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString
                Case Else
                    Return "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.CurrentTaskID
            End Select
        End Get
    End Property

    Public Property CurrentStep() As lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.ViewStep Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.CurrentStep
        Get
            Return Me.ViewState("CurrentStep")
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.ViewStep)
            Me.ViewState("CurrentStep") = value
        End Set
    End Property


    Public ReadOnly Property CurrentTaskID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.CurrentTaskID
        Get
            Try
                Return CInt(Me.Request.QueryString("CurrentTaskID"))
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property

    Public Property CurrentTaskIsChild() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.CurrentTaskIsChild
        Get
            Return Me.ViewState("isChild")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isChild") = value
        End Set
    End Property


    Public Property ParentID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.ParentID
        Get
            Return Me.ViewState("ParentID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("ParentID") = value
        End Set
    End Property

    Public Property TaskPermission() As lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.TaskPermission
        Get
            Return Me.ViewState("TaskPermission")
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum)
            Me.ViewState("TaskPermission") = value
        End Set
    End Property

    Public Property dtoParentProperty() As lm.Comol.Modules.TaskList.Domain.dtoTaskDetailWithPermission Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.dtoParentProperty
        Get
            If Not TypeOf Me.Session("dtoParentProperty_" & SessionUniqueKey.ToString) Is dtoTaskDetailWithPermission Then
                Me.Session("dtoParentProperty_" & SessionUniqueKey.ToString) = New dtoTaskDetailWithPermission()
            End If
            Return Me.Session("dtoParentProperty_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.dtoTaskDetailWithPermission)
            Me.Session("dtoParentProperty_" & SessionUniqueKey.ToString) = value
        End Set
    End Property

    Public Property TasksToAdd() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.Task) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.TasksToAdd
        Get
            If Not TypeOf Me.Session("Task_" & SessionUniqueKey.ToString) Is List(Of Task) Then
                Me.Session("Task_" & SessionUniqueKey.ToString) = New List(Of Task)
            End If
            Return Me.Session("Task_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.Task))
            Me.Session("Task_" & SessionUniqueKey.ToString) = value
        End Set
    End Property

    Public Property SessionUniqueKey() As System.Guid Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.SessionUniqueKey
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

    Public Property dtoParentReallocateTA() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTA) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.dtoParentReallocateTA
        Get
            If Not TypeOf Me.Session("dtoParentReallocateTA_" & SessionUniqueKey.ToString) Is List(Of dtoReallocateTA) Then
                Me.Session("dtoParentReallocateTA_" & SessionUniqueKey.ToString) = New List(Of dtoReallocateTA)
            End If
            Return Me.Session("dtoParentReallocateTA_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTA))
            Me.Session("dtoParentReallocateTA_" & SessionUniqueKey.ToString) = value
        End Set
    End Property

    Public Property dtoReallocateTAToAdd() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTA) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.dtoReallocateTAToAdd
        Get
            If Not TypeOf Me.Session("dtoReallocateTAToAdd_" & SessionUniqueKey.ToString) Is List(Of dtoReallocateTA) Then
                Me.Session("dtoReallocateTAToAdd_" & SessionUniqueKey.ToString) = New List(Of dtoReallocateTA)
            End If
            Return Me.Session("dtoReallocateTAToAdd_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTA))
            Me.Session("dtoReallocateTAToAdd_" & SessionUniqueKey.ToString) = value
        End Set
    End Property
    Public Property AddAnotherChildClicked() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.AddAnotherChildClicked
        Get
            Return Me.Session("AddAnotherChildClicked_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As Boolean)
            Me.Session("AddAnotherChildClicked_" & SessionUniqueKey.ToString) = value
        End Set
    End Property
    Public Property CurrentCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property
#End Region

#Region "Sub Function Iview"

    Public Sub GoBackPage(ByVal Action As COL_BusinessLogic_v2.UCServices.Services_TaskList.ActionType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.GoBackPage
        If Action = Services_TaskList.ActionType.AnnulAddTasks Then
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Action, , InteractionType.UserWithLearningObject)
        End If
        Me.PageUtility.RedirectToUrl(Me.BackUrl)
    End Sub

    Public Sub GoBackPage(ByVal Action As COL_BusinessLogic_v2.UCServices.Services_TaskList.ActionType, ByVal ListOfTaskID As List(Of Long)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.GoBackPage
        If Action = Services_TaskList.ActionType.TaskAdded Then
            For Each TaskID As Long In ListOfTaskID
                Me.PageUtility.AddAction(Me.CurrentCommunityID, Action, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Project, TaskID), InteractionType.UserWithLearningObject)
            Next
        End If
        Me.PageUtility.RedirectToUrl(Me.BackUrl)
    End Sub

    Public Function GetVirtualTaskAssignmentToAdd() As List(Of dtoReallocateTA) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.GetVirtualTaskAssignmentToAdd
        Return Me.CTRLaddVirtualAssignment.CurrentPresenter.GetDtoReallocateTAforSelectedUser
    End Function

    Public Function GetTaskChild() As Task Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.GetTaskChild
        Return Me.CTRLdetail.GetValidateTask
    End Function

    Public Function GetParentID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.GetParentID
        Return Me.CTRLselectTask.GetSelectedTaskID()
    End Function

    Public Sub ClearUniqueKey() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.ClearUniqueKey
        Me.Session("Task_" & Me.SessionUniqueKey.ToString) = Nothing
        Me.Session("dtoReallocateTAToAdd_" & Me.SessionUniqueKey.ToString) = Nothing
        Me.Session("dtoParentReallocateTA_" & Me.SessionUniqueKey.ToString) = Nothing
        Me.Session("dtoParentProperty_" & Me.SessionUniqueKey.ToString) = Nothing
        Me.Session("AddAnotherChildClicked_" & SessionUniqueKey.ToString) = Nothing
    End Sub


    Public Sub ShowError(ByVal ErrorType As String) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.ShowError
        Me.HYPreturnToTaskList.NavigateUrl = Me.BaseUrl & Me.BackUrl
        Select Case ErrorType
            Case "TaskDeleted"
                Me.LBerror.Text = Me.Resource.getValue("ERROR:TaskDeleted")
                Me.PageUtility.AddAction(Services_TaskList.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case Else
                Me.LBerror.Text = ErrorType
                Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        End Select
        Me.MLVaddProject.SetActiveView(Me.VIWerror)
    End Sub

    Public Sub InitSelectTaskMap() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.InitSelectParentMap

        Me.CTRLselectTask.CurrentPresenter.Init(Me.CurrentTaskID)
        Me.WZRaddTask.ActiveStepIndex = IViewAddTask.ViewStep.SelectParent
    End Sub

    Public Sub InitSetTaskProperty(ByVal ParentName As String, ByVal dtoTaskDetailToView As dtoTaskDetailWithPermission, ByVal ViewDetailType As IViewUC_TaskDetail.viewDetailType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.InitSetTaskProperty
        Me.LBtaskPropertiesChild.Text = System.Web.HttpUtility.HtmlEncode(ParentName)
        Me.CTRLdetail.CurrentPresenter.InitView(ViewDetailType, dtoTaskDetailToView, "", ViewModeType.None, IViewTaskDetail.viewDetailType.None)
        Me.WZRaddTask.ActiveStepIndex = IViewAddTask.ViewStep.SetProperty
    End Sub
    Public Sub InitAddVirtualTaskAssignment(ByVal CurrentChildID As Long, ByVal TaskName As String, ByVal CurrentCommunityID As Integer, ByVal ListOfVirtualAssignment As List(Of dtoUserWithRole)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.InitAddVirtualTaskAssignment
        Me.LBaddUserTaskName.Text = System.Web.HttpUtility.HtmlEncode(TaskName)
        Me.CTRLaddVirtualAssignment.CurrentPresenter.InitView(CurrentChildID, CurrentCommunityID, ListOfVirtualAssignment)
        Me.WZRaddTask.ActiveStepIndex = IViewAddTask.ViewStep.AddUser
    End Sub

    Public Sub InitFinalResumeTasks(ByVal ParentName As String, ByVal ListOfdtoOfTaskWithResource As List(Of dtoReallocateTAWithHeader)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.InitFinalResumeTasks
        Me.LBresumeParentName.Text = System.Web.HttpUtility.HtmlEncode(ParentName)
        Me.CTRLresumeChildrenResource.CurrentPresenter.InitView(ListOfdtoOfTaskWithResource, IViewUC_ReallocateResourcesOnNodes.EditType.Read)
        Me.WZRaddTask.ActiveStepIndex = IViewAddTask.ViewStep.FinalResumeTasks
    End Sub

    Public Sub InitResumeTasksWithModifyResources(ByVal ParentName As String, ByVal ListOfdtoOfTaskWithResource As List(Of dtoReallocateTAWithHeader), ByVal ShowManagerError As Boolean) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.InitResumeTasksWithModifyResources
        Me.LBmanagerError.Visible = ShowManagerError
        Me.LBmangeResourceParentName.Text = System.Web.HttpUtility.HtmlEncode(ParentName)
        Me.CTRLmanageChildrenResource.CurrentPresenter.InitView(ListOfdtoOfTaskWithResource, IViewUC_ReallocateResourcesOnNodes.EditType.Edit)
        Me.WZRaddTask.ActiveStepIndex = IViewAddTask.ViewStep.UserManagement
    End Sub

    Public Function GetUserFromResumeWithModify() As System.Collections.Generic.List(Of lm.Comol.Modules.TaskList.Domain.dtoReallocateTA) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.GetUserFromResumeWithModify
        Return Me.CTRLmanageChildrenResource.GetValidateUsersList
    End Function

#End Region

    Private Sub WZRaddTask_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRaddTask.CancelButtonClick
        Me.CurrentPresenter.CancelAddChildrenOperation()
    End Sub

    Private Sub WZRaddTask_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRaddTask.FinishButtonClick
        Me.CurrentPresenter.PersistData()
    End Sub

    Private Sub BTNAddAnotherChildClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.CurrentPresenter.InitAddAnotherChild()
    End Sub

    Private Sub BTNaddUserClick(ByVal TaskID As Long, ByVal ListOfNewDtoTaskAssignmentToAdd As List(Of dtoReallocateTA)) Handles CTRLmanageChildrenResource.FatherSelected
        Me.CurrentPresenter.InitAddVirtualAssignments(TaskID, ListOfNewDtoTaskAssignmentToAdd)
    End Sub

    Private Sub BTNnextClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.CurrentPresenter.NextStep()
    End Sub

    Private Sub BTNupdateChildProperties(ByVal TaskID As Long, ByVal ListOfNewDtoTaskAssignmentToAdd As List(Of dtoReallocateTA)) Handles CTRLmanageChildrenResource.ModifyProps
        Me.CurrentPresenter.InitUpdateChildProperties(TaskID, ListOfNewDtoTaskAssignmentToAdd)
    End Sub

    Private Sub BTNchangeParentClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.CurrentPresenter.ChangeParent()
    End Sub

    Private Sub BTNpreviousButton(ByVal sender As Object, ByVal e As EventArgs)
        Me.CurrentPresenter.Previus()
    End Sub
    'Private Sub WZRaddTask_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRaddTask.PreviousButtonClick
    '    Me.CurrentPresenter.Previus()
    'End Sub

    Public Sub InitButton() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewAddTask.InitButton
        Dim oButton As Button

        'SelectPArent e ChangeParent
        If Me.CurrentStep = IViewAddTask.ViewStep.SelectParent Or Me.CurrentStep = IViewAddTask.ViewStep.ChangeParent Then
            oButton = Me.WZRaddTask.FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
            End If
            oButton = Me.WZRaddTask.FindControl("StartNavigationTemplateContainerID").FindControl("BTNnext")
            If Not IsNothing(oButton) Then
                Me.Resource.setButtonByValue(oButton, "selectParent", True)
                AddHandler oButton.Click, AddressOf BTNnextClick
            End If


        ElseIf Me.CurrentStep = IViewAddTask.ViewStep.FinalResumeTasks Then
            oButton = Me.WZRaddTask.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
            End If
            oButton = Me.WZRaddTask.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
            End If
            oButton = Me.WZRaddTask.FindControl("FinishNavigationTemplateContainerID").FindControl("BTNchangeParent")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
                oButton.Visible = Not Me.CurrentTaskIsChild
                AddHandler oButton.Click, AddressOf BTNchangeParentClick
            End If
            'Modified per gestire il previous step 
            'oButton = Me.WZRaddTask.FindControl("FinishNavigationTemplateContainerID").FindControl("PreviousButton")
            'If Not IsNothing(oButton) Then
            '    Me.Resource.setButton(oButton, True)
            '    AddHandler oButton.Click, AddressOf BTNchangeParentClick

            oButton = Me.WZRaddTask.FindControl("FinishNavigationTemplateContainerID").FindControl("BTNpreviousButton")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
                oButton.Visible = True
                AddHandler oButton.Click, AddressOf BTNpreviousButton
            End If

            oButton = Me.WZRaddTask.FindControl("FinishNavigationTemplateContainerID").FindControl("BTNAddAnotherChild")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
                oButton.Visible = (Me.CurrentStep = IViewAddTask.ViewStep.SetProperty) Or (Me.CurrentStep = IViewAddTask.ViewStep.UserManagement)
                AddHandler oButton.Click, AddressOf BTNAddAnotherChildClick
            End If


        ElseIf Me.CurrentStep = IViewAddTask.ViewStep.UserManagement Then
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
            End If
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNAddAnotherChild")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
                oButton.Visible = (Me.CurrentStep = IViewAddTask.ViewStep.SetProperty) Or (Me.CurrentStep = IViewAddTask.ViewStep.UserManagement)
                AddHandler oButton.Click, AddressOf BTNAddAnotherChildClick
            End If
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNchangeParent")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
                oButton.Visible = Not Me.CurrentTaskIsChild And (Me.CurrentStep = IViewAddTask.ViewStep.UserManagement)
                AddHandler oButton.Click, AddressOf BTNchangeParentClick
            End If
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNpreviousButton")
            oButton.Visible = False
            'If Not IsNothing(oButton) Then
            '    oButton.Visible = True
            Me.Resource.setButton(oButton, True)
            '    'ConTROLLA QUI
            '    AddHandler oButton.Click, AddressOf BTNpreviousButton
            'End If
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNnext")
            If Not IsNothing(oButton) Then
                If Me.CurrentStep = IViewAddTask.ViewStep.AddUser Then
                    Me.Resource.setButtonByValue(oButton, "addUsers", True)
                Else
                    Me.Resource.setButton(oButton, True)
                End If
                AddHandler oButton.Click, AddressOf BTNnextClick
            End If

        Else
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
            End If
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNAddAnotherChild")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
                oButton.Visible = (Me.CurrentStep = IViewAddTask.ViewStep.SetProperty) Or (Me.CurrentStep = IViewAddTask.ViewStep.UserManagement)
                AddHandler oButton.Click, AddressOf BTNAddAnotherChildClick
            End If
            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNchangeParent")
            If Not IsNothing(oButton) Then
                Me.Resource.setButton(oButton, True)
                oButton.Visible = Not Me.CurrentTaskIsChild And (Me.CurrentStep = IViewAddTask.ViewStep.UserManagement)
                AddHandler oButton.Click, AddressOf BTNchangeParentClick
            End If

            'oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("PreviousButton")
            'oButton.Visible = True 'False 'Not (Me.CurrentStep = IViewAddTask.ViewStep.SetProperty) And Not (Me.CurrentStep = IViewAddTask.ViewStep.UserManagement)
            'If Not IsNothing(oButton) Then
            '    Me.Resource.setButton(oButton, True)
            '    AddHandler oButton.Click, AddressOf BTNchangeParentClick
            'End If

            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNpreviousButton")
            If Not IsNothing(oButton) Then
                oButton.Visible = True
                Me.Resource.setButton(oButton, True)
                AddHandler oButton.Click, AddressOf BTNpreviousButton
            End If

            oButton = Me.WZRaddTask.FindControl("StepNavigationTemplateContainerID").FindControl("BTNnext")
            If Not IsNothing(oButton) Then
                If Me.CurrentStep = IViewAddTask.ViewStep.AddUser Then
                    Me.Resource.setButtonByValue(oButton, "addUsers", True)
                Else
                    Me.Resource.setButton(oButton, True)
                End If
                AddHandler oButton.Click, AddressOf BTNnextClick
            End If
        End If

    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_TaskList.Codex)
    End Sub

End Class