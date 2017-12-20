Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class UC_TaskInfo
    Inherits BaseControl
    Implements IViewTaskInfo

#Region "Context"
    Private _Presenter As TaskInfoPresenter
    Private ReadOnly Property CurrentPresenter() As TaskInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TaskInfoPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdTask As Long Implements IViewTaskInfo.IdTask
        Get
            Return ViewStateOrDefault("IdTask", 0)
        End Get
        Set(value As Long)
            ViewState("IdTask") = value
        End Set
    End Property
    Private Property IdAssignment As Long Implements IViewTaskInfo.IdAssignment
        Get
            Return ViewStateOrDefault("IdAssignment", 0)
        End Get
        Set(value As Long)
            ViewState("IdAssignment") = value
        End Set
    End Property
    Private Property isCompleted As Boolean Implements IViewTaskInfo.isCompleted
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
        End Set
    End Property
   
    Private Property CurrentPage As PageListType Implements IViewTaskInfo.CurrentPage
        Get
            Return ViewStateOrDefault("CurrentPage", PageListType.None)
        End Get
        Set(value As PageListType)
            ViewState("CurrentPage") = value
        End Set
    End Property

    'Public Property LoaderAssignments As System.Collections.Generic.List(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoActivityCompletion) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewTaskInfo.LoaderAssignments
    '    Get
    '        Return Nothing
    '    End Get
    '    Set(value As System.Collections.Generic.List(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoActivityCompletion))

    '    End Set
    'End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Public Event UpdateProjectInfo(idProject As Long, completion As Dictionary(Of ResourceActivityStatus, Long))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

            .setButton(BTNconfirmTaskCompletion, True)
            .setButton(BTNsaveTaskDialogSettings, True)
            .setHyperLink(HYPcloseTaskDialog, False, True)
            .setLiteral(LTtaskGeneralSettings)
            .setLiteral(LTtaskAttachments)

            .setLabel(LBmyActivityCompletion_t)
            .setLabel(LBactivityStartDate_t)
            .setLabel(LBactivityEndDate_t)
            .setLabel(LBactivityName_t)
            .setLabel(LBactivityDuration_t)
            .setLabel(LBactivityDeadLine_t)
            .setLabel(LBactivityDescription_t)
            .setLabel(LBactivityNote_t)
            .setLabel(LBmyActivityCompletion_t)

            .setLabel(LBactivityCompletion_t)
            .setLabel(LBactivityCompletionStatus_t)

            .setLabel(LBactivityCompletionResources_t)
            .setLiteral(LTactivityResourceAssigned_t)
            .setLiteral(LTactivityResourceCompletion_t)


        End With
    End Sub
#End Region

#Region "implements"
    Public Sub InitializeControl(task As dtoPlainTask, displayOthersCompletion As Boolean, cPage As PageListType) Implements IViewTaskInfo.InitializeControl
        CurrentPage = cPage
        CurrentPresenter.InitView(task, displayOthersCompletion)
        If Page.IsPostBack Then
            Me.SetInternazionalizzazione()
        End If
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewTaskInfo.DisplaySessionTimeout
        Me.MLVtaskInfo.SetActiveView(VIWmessage)
        LTmessage.Text = Resource.getValue("IViewTaskInfo.DisplaySessionTimeout")
    End Sub
    Private Sub DisplayUnknownTask() Implements IViewTaskInfo.DisplayUnknownTask
        Me.MLVtaskInfo.SetActiveView(VIWmessage)
        LTmessage.Text = Resource.getValue("DisplayUnknownTask")
    End Sub

    Private Sub LoadSettings(task As dtoPlainTask, myCompleteness As dtoField(Of String), allowEdit As Boolean) Implements IViewTaskInfo.LoadSettings
        LoadSettings(task, False)
        DVmyCompletion.Visible = True
        TXBmyCompletion.Text = myCompleteness.Current
        LBmyActivityCompletion.Text = myCompleteness.Current
        If allowEdit Then
            LBmyActivityCompletion_t.AssociatedControlID = "TXBmyCompletion"
            TXBmyCompletion.Visible = True
            LBmyActivityCompletion.Visible = False
        Else
            LBmyActivityCompletion_t.AssociatedControlID = "LBmyActivityCompletion_t"
            LBmyActivityCompletion.Visible = True
            TXBmyCompletion.Visible = False
        End If
    End Sub

    Public Sub LoadSettings(task As dtoPlainTask, assignments As List(Of dtoActivityCompletion), allowEdit As Boolean) Implements IViewTaskInfo.LoadSettings
        LoadSettings(task, True)

        BTNconfirmTaskCompletion.Visible = allowEdit
        RPTresourcesCompletion.DataSource = assignments
        RPTresourcesCompletion.DataBind()
        isCompleted = task.IsCompleted
        DVassignments.Visible = True
    End Sub

    Private Sub LoadSettings(task As dtoPlainTask, displayStatus As Boolean) Implements IViewTaskInfo.LoadSettings
        Me.MLVtaskInfo.SetActiveView(VIWsettings)
        LItaskAttachments.Visible = False

        With task
            LBactivityName.Text = .Name
            LBactivityDescription.Text = .Description
            LBactivityNote.Text = .Notes
            LBactivityDuration.Text = .Duration & IIf(.IsDurationEstimated, "?", "")
            LBactivityStartDate.Text = .StartDate.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
            If .EndDate.HasValue Then
                LBactivityEndDate.Text = .EndDate.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
            Else
                LBactivityEndDate.Text = "//"
            End If
            If .Deadline.HasValue Then
                LBactivityDeadLine.Text = .Deadline.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
            Else
                DVdeadline.Visible = False
            End If
            DVdescription.Visible = Not String.IsNullOrEmpty(.Description)
            DVnote.Visible = Not String.IsNullOrEmpty(.Notes)
            LBactivityCompletionStatus.Text = Resource.getValue("Translation.ProjectItemStatus." & .Status.ToString)
            DVstatus.Visible = displayStatus


            DVmanagerComplation.Visible = True
            LBactivityCompletion.Text = task.Completeness & "%"

        End With
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTaks As Long, action As lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.ActionType) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewTaskInfo.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Task, idTaks.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Function GetAssignments() As List(Of dtoActivityCompletion) Implements IViewTaskInfo.GetAssignments
        Dim items As New List(Of dtoActivityCompletion) '= LoaderAssignments

        For Each row As RepeaterItem In RPTresourcesCompletion.Items
            Dim item As New dtoActivityCompletion
            item.Id = CLng(DirectCast(row.FindControl("LTidAssignment"), Literal).Text)

            Dim oldValue As Integer = CInt(DirectCast(row.FindControl("LToldCompletion"), Literal).Text)

            Dim t As String = DirectCast(row.FindControl("TXBcompletion"), TextBox).Text
            If (t.Contains("%")) Then
                t = t.Replace("%", "")
            End If
            If String.IsNullOrEmpty(t) Then
                item.Completeness = 0
            ElseIf IsNumeric(t) Then
                item.Completeness = IIf(CInt(t) > 100, 100, IIf(CInt(t) < 0, 0, CInt(t)))
            Else
                item.Completeness = oldValue
            End If

            Dim oRadioButtonList As RadioButtonList = row.FindControl("RBLapproveUserTaskCompletion")

            If item.Completeness < 100 Then
                item.IsApproved = False
            End If
            items.Add(item)
        Next
        Return items
    End Function
    Public Sub LoadAttachments(allowDownload As Boolean) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewTaskInfo.LoadAttachments

    End Sub

    Private Sub UpdateMyCompletion(assignment As dtoMyAssignmentCompletion) Implements IViewTaskInfo.UpdateMyCompletion
        TXBmyCompletion.Text = assignment.MyCompletion.Current
        LBmyActivityCompletion.Text = assignment.MyCompletion.Current
        LBactivityCompletion.Text = assignment.TaskCompletion & "%"
        LBactivityCompletionStatus.Text = Resource.getValue("Translation.ProjectItemStatus." & assignment.TaskStatus.ToString)
        RaiseEvent UpdateProjectInfo(assignment.IdProject, assignment.ProjectCompletion)
    End Sub
    Private Sub UpdateTaskCompletion(tCompletion As dtoTaskCompletion) Implements IViewTaskInfo.UpdateTaskCompletion
        LBactivityCompletion.Text = tCompletion.Completion & "%"
        isCompleted = tCompletion.IsCompleted
        LBactivityCompletionStatus.Text = Resource.getValue("Translation.ProjectItemStatus." & tCompletion.Status.ToString)

        If tCompletion.Completion = 100 AndAlso tCompletion.IsCompleted Then
            For Each row As RepeaterItem In RPTresourcesCompletion.Items
                DirectCast(row.FindControl("LToldCompletion"), Literal).Text = 100
                DirectCast(row.FindControl("TXBcompletion"), TextBox).Text = 100

                Dim oRadioButtonList As RadioButtonList = row.FindControl("RBLapproveUserTaskCompletion")

                oRadioButtonList.SelectedValue = Boolean.TrueString
            Next
        End If
    End Sub
#End Region

#Region "Internal"
    Protected Function GetContainerCssClass() As String
        Select Case CurrentPage
            Case PageListType.DashboardAdministrator, PageListType.DashboardManager
                Return LTcssClassManager.Text
            Case Else
                Return LTcssClassResource.Text
        End Select
    End Function
    Protected Function GetDialogTitle() As String
        Return Resource.getValue("IViewTaskInfo.GetDialogTitle")
    End Function
    Private Sub RPTresourcesCompletion_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTresourcesCompletion.ItemDataBound
        Dim dto As dtoActivityCompletion = e.Item.DataItem
        'Dim oCell As HtmlTableCell = e.Item.FindControl("TDcompletionAction")
        'oCell.Visible = dto.AllowDelete

        'If dto.AllowDelete Then
        '    Dim oLinkButton As LinkButton = e.Item.FindControl("LNBvirtualDeleteAssignment")
        '    oLinkButton.Visible = dto.AllowDelete
        '    Resource.setLinkButton(oLinkButton, False, True)
        'End If

        Dim oLabel As Label = e.Item.FindControl("LBcompletion")
        oLabel.Visible = Not dto.AllowEdit

        Dim oTextbox As TextBox = e.Item.FindControl("TXBcompletion")
        oTextbox.Visible = dto.AllowEdit


        Dim oRadioButtonList As RadioButtonList = e.Item.FindControl("RBLapproveUserTaskCompletion")
        oRadioButtonList.SelectedValue = dto.IsApproved
        'Dim oRangeValidator As RangeValidator = e.Item.FindControl("RNVcompletion")
        'oRangeValidator.Visible = dto.AllowEdit

        'Dim oRequiredFieldValidator As RequiredFieldValidator = e.Item.FindControl("RFVcompletion")
        'oRequiredFieldValidator.Visible = dto.AllowEdit
    End Sub


    Public Sub UpdateCompletion(mycompletion As String, taskCompletion As Integer, status As ProjectItemStatus)
        TXBmyCompletion.Text = mycompletion
        LBmyActivityCompletion.Text = mycompletion
        LBactivityCompletion.Text = taskCompletion & "%"
        LBactivityCompletionStatus.Text = Resource.getValue("Translation.ProjectItemStatus." & status.ToString)
    End Sub

    Private Sub BTNconfirmTaskCompletion_Click(sender As Object, e As System.EventArgs) Handles BTNconfirmTaskCompletion.Click
        Dim updateSummary As Boolean = False
        Dim saved As Boolean = CurrentPresenter.ConfirmCompletion(IdTask, updateSummary)

        BTNconfirmTaskCompletion.CommandArgument = saved & "," & updateSummary
    End Sub

    Private Sub BTNsaveTaskDialogSettings_Click(sender As Object, e As System.EventArgs) Handles BTNsaveTaskDialogSettings.Click
        Dim saved As Boolean = False
        Dim updateSummary As Boolean = False
        If RPTresourcesCompletion.Items.Count > 0 Then
            saved = CurrentPresenter.SaveSettings(IdTask, GetAssignments(), updateSummary)
        ElseIf DVmyCompletion.Visible Then
            Dim completeness As Integer = 0
            Dim t As String = TXBmyCompletion.Text
            If (t.Contains("%")) Then
                t = t.Replace("%", "")
            End If
            If String.IsNullOrEmpty(t) Then
                completeness = 0
            ElseIf IsNumeric(t) Then
                completeness = IIf(CInt(t) > 100, 100, IIf(CInt(t) < 0, 0, CInt(t)))
            Else
                completeness = CInt(LBmyActivityCompletion.Text.Replace("%", ""))
            End If

            saved = CurrentPresenter.SaveSettings(IdTask, IdAssignment, completeness, updateSummary)
        End If
        BTNsaveTaskDialogSettings.CommandArgument = saved & "," & updateSummary
    End Sub
    Public Function GetMyCompletion() As String
        Return TXBmyCompletion.Text
    End Function
    Public Function GetTaskCompletion() As Integer
        If LBactivityCompletion.Text.Contains("%") Then
            Return CInt(LBactivityCompletion.Text.Replace("%", ""))
        Else
            Return CInt(LBactivityCompletion.Text)
        End If
    End Function
    Public Function GetTaskIsCompleted() As String
        Return isCompleted
    End Function
    Public Function GetTaskStatus() As String
        Return LBactivityCompletionStatus.Text
    End Function

#End Region


End Class