Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.UI.Presentation
Imports System.Text.RegularExpressions

Partial Public Class UC_TaskDetail
    Inherits BaseControlSession
    Implements IViewUC_TaskDetail


    Private _Presenter As TaskDetailUCPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public Event BTN_SaveTaskClicked(ByVal oTask As Task)
    Public Event ReloadPage()

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As TaskDetailUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TaskDetailUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#Region "IViewUC_TaskDetail Property"



    Public Property CurrentTaskID() As Long Implements IViewUC_TaskDetail.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property

    Public Property CurrentTaskAssignmentID() As Long Implements IViewUC_TaskDetail.CurrentTaskAssignmentID
        Get
            Return Me.ViewState("CurrentTaskAssignmentID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskAssignmentID") = value
        End Set
    End Property

    Public Property TaskPermission() As TaskPermissionEnum Implements IViewUC_TaskDetail.TaskPermission
        Get
            Return Me.ViewState("Permission")
        End Get
        Set(ByVal value As TaskPermissionEnum)
            Me.ViewState("Permission") = value
        End Set
    End Property

    Public Property CurrentViewType() As IViewUC_TaskDetail.viewDetailType Implements IViewUC_TaskDetail.CurrentViewType
        Get
            Return Me.ViewState("CurrentViewType")
        End Get
        Set(ByVal value As IViewUC_TaskDetail.viewDetailType)
            Me.ViewState("CurrentViewType") = value
        End Set
    End Property

    Public Property isTaskChild() As Boolean Implements IViewUC_TaskDetail.isTaskChild
        Get
            Return Me.ViewState("isTaskChild")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isTaskChild") = value
        End Set
    End Property

#End Region

#Region "IViewUC_TaskDetail Metodi"

    Private Sub SetStatusImage(ByVal Status As lm.Comol.Modules.TaskList.Domain.TaskStatus)
        Select Case Status
            Case TaskStatus.completed
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/completed20.png"
            Case TaskStatus.notStarted
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/NOTSTARTEDoe.png"
            Case TaskStatus.suspended
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/SUSPENDEDoe.png"
            Case TaskStatus.started
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/STARTEDoe.png"
        End Select
        Me.IMstatus.ToolTip = Me.Resource.getValue("TaskStatus." & Status.ToString)
    End Sub

    Private Sub SetPriorityImage(ByVal Priority As lm.Comol.Modules.TaskList.Domain.TaskPriority)
        Select Case Priority
            Case TaskPriority.high
                Me.IMpriority.ImageUrl = Me.BaseUrl & "images/TaskList/priority_high.png"
            Case TaskPriority.normal
                Me.IMpriority.ImageUrl = Me.BaseUrl & "images/TaskList/priority_normal.png"
            Case TaskPriority.low
                Me.IMpriority.ImageUrl = Me.BaseUrl & "images/TaskList/priority_low.png"
        End Select
        Me.IMpriority.ToolTip = Me.Resource.getValue("TaskPriority." & Priority.ToString)
    End Sub

    Public Sub LoadAvailableCategory(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.TaskCategory))
        Me.DDLcategory.Items.Clear()
        For Each category As TaskCategory In oList
            Me.DDLcategory.Items.Add(category.Name)
        Next
    End Sub

    Public Shared Function StripHTML(ByVal strHTML As String) As String
        Dim objRegExp As New Regex("<(.|\n)+?>", RegexOptions.IgnoreCase)
        Dim strOutput As String

        strOutput = objRegExp.Replace(strHTML, "")

        Return strOutput
    End Function

    Public Sub LoadAvailablePriority(ByVal oPriorityList As List(Of lm.Comol.Modules.TaskList.Domain.TaskPriority))
        Me.DDLpriorityEdit.Items.Clear()
        For Each priority As TaskPriority In oPriorityList
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("TaskPriority." & priority.ToString)
            oItem.Value = priority.ToString
            If priority = TaskPriority.normal Then
                oItem.Selected = True
            End If
            Me.DDLpriorityEdit.Items.Add(oItem)
        Next
    End Sub

    Public Sub LoadAvailableStatus(ByVal oListOfStatus As List(Of TaskStatus))
        Me.DDLstatusEdit.Items.Clear()
        For Each status As TaskStatus In oListOfStatus
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("TaskStatus." & status.ToString)
            oItem.Value = status.ToString
            If status = TaskStatus.notStarted Then
                oItem.Selected = True
            End If
            Me.DDLstatusEdit.Items.Add(oItem)
        Next
    End Sub


    Private Sub InitViewUpdate(ByVal TaskDetail As dtoTaskDetail, ByVal oListOfStatus As List(Of TaskStatus), ByVal oPriorityList As List(Of TaskPriority), ByVal oListOfCategory As List(Of TaskCategory), ByVal MainPageType As ViewModeType, ByVal DetailType As IViewTaskDetail.viewDetailType) Implements IViewUC_TaskDetail.InitViewUpdate
        Me.HYPaddSubTaskEdit.NavigateUrl = BaseUrl & "TaskList/AddTask.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&PreviusPage=" & IViewAddTask.PreviusPage.DetailUpdate.ToString
        Me.HYPaddSubTaskEdit.Visible = ((Me.TaskPermission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate) And Not TaskDetail.isDeleted
        Me.HYPgoToProjectMapEdit.NavigateUrl = BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&MainPage=" & MainPageType.ToString & "&DetailType=" & DetailType.ToString
        Me.HYPganttEdit.NavigateUrl = BaseUrl & "TaskList/Gantt.aspx?TaskID=" & Me.CurrentTaskID & "&PreviusPage=" & IviewGantt.PageType.DetailUpdate.ToString
        Me.LBwbsEdit.Text = TaskDetail.TaskWBS
        Me.TXTnameEdit.Text = StripHTML(TaskDetail.TaskName) 'System.Web.HttpUtility.HtmlEncode(TaskDetail.TaskName)
        Me.LoadAvailableCategory(oListOfCategory)
        Me.LoadAvailablePriority(oPriorityList)
        Me.LoadAvailableStatus(oListOfStatus)
        Me.DDLcategory.SelectedValue = TaskDetail.Category
        Me.DDLpriorityEdit.SelectedValue = TaskDetail.Priority.ToString
        Me.DDLstatusEdit.SelectedValue = TaskDetail.Status.ToString
        If TaskDetail.CommunityName = "" Then
            Me.LBcommunityEdit.Text = Me.Resource.getValue("Portal")
        Else
            Me.LBcommunityEdit.Text = TaskDetail.CommunityName 'System.Web.HttpUtility.HtmlEncode(TaskDetail.CommunityName)
        End If
        Me.LBprojectEdit.Text = StripHTML(TaskDetail.ProjectName)  'System.Web.HttpUtility.HtmlEncode(TaskDetail.ProjectName)
        Me.CTRLeditorDescription.HTML = TaskDetail.Description
        Me.LBcategoryName.Text = TaskDetail.Category 'System.Web.HttpUtility.HtmlEncode(TaskDetail.Category) 'fare sett della dropdown
        'date in sola lettura=sono un task nodo e nn ho i permessi
        If TaskDetail.StartDate.HasValue Then
            Me.LBstartDateEdit.Text = TaskDetail.StartDate
        End If
        Me.LBstartDateEdit.Visible = Not Me.isTaskChild
        If TaskDetail.EndDate.HasValue Then
            Me.LBendDateEdit.Text = TaskDetail.EndDate
        End If
        Me.LBendDateEdit.Visible = Not Me.isTaskChild
        If TaskDetail.Deadline.HasValue Then
            Me.LBdeadlineEdit.Text = TaskDetail.Deadline '.ToString("dd/MM/yyyy")
        Else
        End If
        If TaskDetail.Deadline.HasValue AndAlso TaskDetail.Deadline.Value < Date.Now AndAlso TaskDetail.Status <> TaskStatus.completed Then
            Me.LBdeadlineEdit.CssClass = "erroreSmall"
        Else
            Me.LBdeadlineEdit.CssClass = "Bookmark_TestoBold"
        End If
        Me.LBdeadlineEdit.Visible = Not Me.isTaskChild
        'date modificabili=sono task foglia e ho i permessi
        Me.RDPstartDateEdit.SelectedDate = TaskDetail.StartDate
        Me.RDPstartDateEdit.Visible = Me.isTaskChild
        Me.RDPendDateEdit.SelectedDate = TaskDetail.EndDate
        Me.RDPendDateEdit.Visible = Me.isTaskChild
        Me.RDPdeadlineEdit.SelectedDate = TaskDetail.Deadline
        Me.RDPdeadlineEdit.Visible = Me.isTaskChild

        Me.LBtaskCompletenssEdit.Text = TaskDetail.TaskCompleteness 'System.Web.HttpUtility.HtmlEncode(TaskDetail.TaskCompleteness)

        Dim oImage As System.Web.UI.WebControls.Image
        oImage = Me.IMtaskCompletenessEdit
        If Not IsNothing(oImage) Then
            oImage.Height = "15"
            oImage.Width = TaskDetail.TaskCompleteness.ToString
            oImage.ToolTip = TaskDetail.TaskCompleteness.ToString() & "%"
            oImage.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
        End If
        Me.DIVpersonalCompletenessEdit.Visible = ((Me.TaskPermission And TaskPermissionEnum.TaskSetCompleteness) = TaskPermissionEnum.TaskSetCompleteness)

        Me.TXTpersonalCompletenessEdit.Text = TaskDetail.PersonalCompleteness

        oImage = Me.IMpersonalCompletenessEdit
        If Not IsNothing(oImage) Then
            oImage.Height = "15"
            If TaskDetail.PersonalCompleteness >= 0 Then
                oImage.Width = TaskDetail.PersonalCompleteness.ToString
            Else
                oImage.Visible = False
            End If
            oImage.ToolTip = TaskDetail.PersonalCompleteness.ToString() & "%"
            oImage.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
        End If

        Me.TXTpersonalCompletenessEdit.Text = TaskDetail.PersonalCompleteness
        Me.TXTnoteEdit.Text = TaskDetail.Notes 'System.Web.HttpUtility.HtmlEncode(TaskDetail.Notes)

        Me.MLVdetail.SetActiveView(Me.VIWdetailEditable)
    End Sub

    Private Sub InitAddTask(ByVal TaskDetail As dtoTaskDetail, ByVal isProject As Boolean, ByVal oListOfStatus As List(Of TaskStatus), ByVal oPriorityList As List(Of TaskPriority), ByVal oListOfCategory As List(Of TaskCategory)) Implements IViewUC_TaskDetail.InitAddTask
        Me.DIVtitleEdit.Visible = False
        Me.DIVbuttonEditable.Visible = False
        Me.LoadAvailableCategory(oListOfCategory)
        Me.LoadAvailablePriority(oPriorityList)
        Me.LoadAvailableStatus(oListOfStatus)
        Me.TXTnameEdit.Text = ""
        Me.TXTnoteEdit.Text = ""
        Me.CTRLeditorDescription.HTML = ""
        If TaskDetail.CommunityName = "" Then
            Me.LBcommunityEdit.Text = Me.Resource.getValue("Portal")
        Else
            Me.LBcommunityEdit.Text = TaskDetail.CommunityName
        End If
        Me.DIVprojectEdit.Visible = False
        Me.LBstartDateEdit.Visible = False
        Me.LBendDateEdit.Visible = False
        Me.LBdeadlineEdit.Visible = False
        Me.RDPstartDateEdit.SelectedDate = TaskDetail.StartDate
        Me.RDPendDateEdit.SelectedDate = TaskDetail.EndDate
        Me.RDPdeadlineEdit.SelectedDate = TaskDetail.Deadline
        Me.DIVtaskCompletenessEdit.Visible = False
        Me.DIVpersonalCompletenessEdit.Visible = False
        If Not isProject Then
            Me.DDLcategory.SelectedValue = TaskDetail.Category
            Me.DDLpriorityEdit.SelectedValue = TaskDetail.Priority.ToString
            Me.DDLstatusEdit.SelectedValue = TaskDetail.Status.ToString
        End If
        Me.BTNsaveTask.Text = "Save"

        Me.MLVdetail.SetActiveView(Me.VIWdetailEditable)
    End Sub

    Private Sub InitViewRead(ByVal TaskDetail As dtoTaskDetail, ByVal MainPageType As ViewModeType, ByVal DetailType As IViewTaskDetail.viewDetailType) Implements IViewUC_TaskDetail.InitViewRead
        Me.HYPaddSubTask.NavigateUrl = BaseUrl & "TaskList/AddTask.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&PreviusPage=" & IViewAddTask.PreviusPage.DeatailReadOnly.ToString
        Me.HYPaddSubTask.Visible = ((Me.TaskPermission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate) And Not TaskDetail.isDeleted
        Me.HYPgoToProjectMap.NavigateUrl = BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&MainPage=" & MainPageType.ToString & "&DetailType=" & DetailType.ToString
        Me.HYPgantt.NavigateUrl = BaseUrl & "TaskList/Gantt.aspx?TaskID=" & Me.CurrentTaskID & "&PreviusPage=" & IviewGantt.PageType.DetailRead.ToString
        Me.LBwbs.Text = TaskDetail.TaskWBS
        Me.LBtaskName.Text = StripHTML(TaskDetail.TaskName) 'System.Web.HttpUtility.HtmlEncode(TaskDetail.TaskName)
        SetStatusImage(TaskDetail.Status)
        SetPriorityImage(TaskDetail.Priority)
        If TaskDetail.CommunityName = "" Then
            Me.LBcommunityName.Text = Me.Resource.getValue("Portal")
        Else
            Me.LBcommunityName.Text = TaskDetail.CommunityName
        End If
        Me.LBprojectName.Text = StripHTML(TaskDetail.ProjectName) 'System.Web.HttpUtility.HtmlEncode(TaskDetail.ProjectName)
        If TaskDetail.Description = "" Then
            Me.LTspace.Text = "<br />"
        Else
            Me.LBdescriptionText.Text = TaskDetail.Description
        End If

        Me.LBcategoryName.Text = TaskDetail.Category
        Me.LBstartDate.Text = TaskDetail.StartDate
        If TaskDetail.EndDate.HasValue Then
            Me.LBendDate.Text = TaskDetail.EndDate.Value
        End If

        If TaskDetail.Deadline.HasValue Then
            Me.LBdeadline.Text = TaskDetail.Deadline
            If TaskDetail.Deadline < Date.Now Then
                Me.LBdeadline.CssClass = "erroreSmall"
            Else
                Me.LBdeadline.CssClass = "Bookmark_TestoBold"
            End If
        End If


        Me.LBtaskCompleteness.Text = TaskDetail.TaskCompleteness

        Dim oImage As System.Web.UI.WebControls.Image
        oImage = Me.IMtaskCompleteness
        If Not IsNothing(oImage) Then
            'oImage.Height = "15"
            oImage.Width = TaskDetail.TaskCompleteness.ToString
            oImage.ToolTip = TaskDetail.TaskCompleteness.ToString() & "%"
            oImage.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
        End If
        'Tia: nella parte sotto agginta la visualizzazione del caso di Manager e User contemporanei
        Me.DIVpersonalCompleteness.Visible = ((Me.TaskPermission And TaskPermissionEnum.TaskSetCompleteness) = TaskPermissionEnum.TaskSetCompleteness) ' Or (Me.TaskPermission And TaskPermissionEnum.ManagementUser = TaskPermissionEnum.ManagementUser)

        Me.TXTpersonalCompleteness.Text = TaskDetail.PersonalCompleteness

        oImage = Me.IMpersonalCompleteness
        If Not IsNothing(oImage) Then
            oImage.Height = "15"
            If TaskDetail.PersonalCompleteness >= 0 Then
                oImage.Width = TaskDetail.PersonalCompleteness.ToString
            Else
                oImage.Visible = False
            End If
            oImage.ToolTip = TaskDetail.PersonalCompleteness.ToString() & "%"
            oImage.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
        End If


        Me.LBnoteText.Text = TaskDetail.Notes 'System.Web.HttpUtility.HtmlEncode(TaskDetail.Notes)
        Me.MLVdetail.SetActiveView(Me.VIWdetailReadOnly)
    End Sub

    Public Function GetTask()
        Dim oTask As New Task
        oTask.ID = Me.CurrentTaskID
        oTask.Name = StripHTML(Me.TXTnameEdit.Text) 'System.Web.HttpUtility.HtmlEncode(Me.TXTnameEdit.Text)
        oTask.Description = Me.CTRLeditorDescription.HTML
        oTask.Category = Me.CurrentPresenter.GetTaskCategory(Me.DDLcategory.SelectedValue)
        oTask.StartDate = RDPstartDateEdit.SelectedDate
        oTask.EndDate = RDPendDateEdit.SelectedDate
        oTask.Deadline = RDPdeadlineEdit.SelectedDate
        oTask.Priority = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskPriority).GetByString(Me.DDLpriorityEdit.SelectedItem.Value, TaskPriority.normal)
        oTask.Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskStatus).GetByString(Me.DDLstatusEdit.SelectedItem.Value, TaskStatus.notStarted)
        oTask.Notes = Me.TXTnoteEdit.Text 'System.Web.HttpUtility.HtmlEncode(Me.TXTnoteEdit.Text)

        Return oTask
    End Function

    Public Function GetValidateTask()

        Me.Page.Validate()
        If Me.Page.IsValid Then
            Return Me.GetTask
        Else

            Return Nothing
        End If

    End Function


    Public Function PageIsValid()
        Me.Page.Validate()
        Return Me.Page.IsValid
    End Function

#End Region



    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_TaskDetail", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBcategoryEdit)
            .setLabel(LBcategoryTitle)
            .setLabel(LBcommunityTitle)
            .setLabel(LBcommunityTitleEdit)
            .setLabel(LBprojectTitle)
            .setLabel(LBprojectTitleEdit)
            .setLabel(LBdescriptionTitle)
            .setLabel(LBdescriptionTitleEdit)
            .setLabel(LBstartDateTitle)
            .setLabel(LBstartDateTitleEdit)
            .setLabel(LBendDateTitle)
            .setLabel(LBendDateTitleEdit)
            .setLabel(LBdeadlineTitle)
            .setLabel(LBdeadlineTitleEdit)
            .setLabel(LBtaskCompletenessTitle)
            .setLabel(LBtaskCompletenessTitleEdit)
            .setLabel(LBpersonalCompletnessTitle)
            .setLabel(LBpersonalCompletenessTitleEdit)
            .setLabel(LBnote)
            .setLabel(LBnoteTitleEdit)
            .setLabel(LBnameTitleEdit)
            .setLabel(LBtitleEdit)
            .setButton(BTNsaveTask, True)
            .setButton(BTNupdatePersonalCompleteness, True)
            .setButton(BTNupdatePersonalCompletenessEdit, True)
            .setHyperLink(HYPaddSubTask, True, True)
            .setHyperLink(HYPaddSubTaskEdit, True, True)
            .setHyperLink(HYPreturnToTaskList, True, True)
            .setHyperLink(HYPreturnToTaskListEdit, True, True)
            .setHyperLink(HYPgoToProjectMap, True, True)
            .setHyperLink(HYPgoToProjectMapEdit, True, True)
            .setHyperLink(HYPgantt, True, True)
            .setHyperLink(HYPganttEdit, True, True)
            Me.REVnoteLength.ErrorMessage = .getValue("Error.NoteLength")
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

        End If
    End Sub

    Private Sub BTNupdatePersonalCompleteness_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNupdatePersonalCompleteness.Click
        If Page.IsValid Then
            Dim personalCompleteness As Integer = CType(Me.TXTpersonalCompleteness.Text, Integer)
            Me.CurrentPresenter.UpdatePersonalCompleteness(personalCompleteness)
            RaiseEvent ReloadPage()
        End If
    End Sub

    Private Sub BTNupdatePersonalCompletenessEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNupdatePersonalCompletenessEdit.Click
        If Page.IsValid Then
            Dim personalCompleteness As Integer = CType(Me.TXTpersonalCompletenessEdit.Text, Integer)
            Me.CurrentPresenter.UpdatePersonalCompleteness(personalCompleteness)
            RaiseEvent ReloadPage()
        End If
    End Sub

    Private Sub BTNsaveTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsaveTask.Click
        If Page.IsValid Then
            Dim oTask = Me.GetTask()
            If (IViewUC_TaskDetail.viewDetailType.Update = Me.CurrentViewType) Then
                If (Me.TaskPermission And TaskPermissionEnum.TaskSetCompleteness) = TaskPermissionEnum.TaskSetCompleteness Then
                    Dim personalCompleteness As Integer = CType(Me.TXTpersonalCompletenessEdit.Text, Integer)
                    Me.CurrentPresenter.UpdatePersonalCompleteness(personalCompleteness)
                End If
            End If

            RaiseEvent BTN_SaveTaskClicked(oTask)


        End If
    End Sub
    Public Sub SetBackUrl(ByVal BackUrl As String) Implements IViewUC_TaskDetail.SetBackUrl
        Me.HYPreturnToTaskList.NavigateUrl = BackUrl
        Me.HYPreturnToTaskListEdit.NavigateUrl = BackUrl
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        CTRLeditorDescription.InitializeControl(UCServices.Services_TaskList.Codex)
    End Sub
End Class