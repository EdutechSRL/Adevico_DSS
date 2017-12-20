Imports System.Linq
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Public Class UC_ActivityEditor
    Inherits BaseControl
    Implements IViewEditActivity

#Region "Context"
    Private _Presenter As EditActivityPresenter
    Private ReadOnly Property CurrentPresenter() As EditActivityPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditActivityPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property AllowDelete As Boolean Implements IViewEditActivity.AllowDelete
        Get
            Return ViewStateOrDefault("AllowDelete", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowDelete") = value
            BTNdeleteCurrentActivity.Visible = value
        End Set
    End Property
    Private Property AllowEdit As Boolean Implements IViewEditActivity.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            BTNsaveCurrentActivity.Visible = value
        End Set
    End Property
    Private Property IdActivity As Long Implements IViewEditActivity.IdActivity
        Get
            Return ViewStateOrDefault("IdActivity", 0)
        End Get
        Set(value As Long)
            ViewState("IdActivity") = value
        End Set
    End Property
    Private Property IdProject As Long Implements IViewEditActivity.IdProject
        Get
            Return ViewStateOrDefault("IdProject", 0)
        End Get
        Set(value As Long)
            ViewState("IdProject") = value
        End Set
    End Property
    Private Property ProjectIdCommunity As Integer Implements IViewEditActivity.ProjectIdCommunity
        Get
            Return ViewStateOrDefault("ProjectIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("ProjectIdCommunity") = value
        End Set
    End Property
    Private Property Activities As List(Of dtoLiteMapActivity) Implements IViewEditActivity.Activities
        Get
            Return ViewStateOrDefault("Activities", New List(Of dtoLiteMapActivity))
        End Get
        Set(value As List(Of dtoLiteMapActivity))
            ViewState("Activities") = value
        End Set
    End Property
    Private ReadOnly Property UnknownUser As String Implements IViewEditActivity.UnknownUser
        Get
            Return Resource.getValue("UnknownUser")
        End Get
    End Property
    Private Property LoaderLinks As List(Of dtoActivityLink) Implements IViewEditActivity.LoaderLinks
        Get
            Return ViewStateOrDefault("LoaderLinks", New List(Of dtoActivityLink))
        End Get
        Set(value As List(Of dtoActivityLink))
            ViewState("LoaderLinks") = value
        End Set
    End Property
    Private Property LoaderAssignments As List(Of dtoActivityCompletion) Implements IViewEditActivity.LoaderAssignments
        Get
            Return ViewStateOrDefault("LoaderAssignments", New List(Of dtoActivityCompletion))
        End Get
        Set(value As List(Of dtoActivityCompletion))
            ViewState("LoaderAssignments") = value
        End Set
    End Property
    Private Property isCompleted As Boolean Implements IViewEditActivity.isCompleted
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
        End Set
    End Property
    Private Property DefaultWorkingDay As dtoWorkingDay Implements IViewEditActivity.DefaultWorkingDay
        Get
            Return ViewStateOrDefault("DefaultWorkingDay", dtoWorkingDay.GetDefault())
        End Get
        Set(value As dtoWorkingDay)
            ViewState("DefaultWorkingDay") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event CloseWindow()
    Public Event VirtualDeleteActivity(ByVal idActivity As Long)
    Public Event SavedActivity(idActivity As Long, startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?))
    Public Event GetProjectDate(ByRef startDate As DateTime?, ByRef endDate As DateTime?, ByRef deadline As DateTime?)

    Public Property RaiseCommandEvents As Boolean
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean
        Get
            Return ViewStateOrDefault("DisplayCommands", True)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
    Protected ReadOnly Property CurrentShortDatePattern As String
        Get
            Return LoaderCultureInfo.DateTimeFormat.ShortDatePattern
        End Get
    End Property
    Public Property LoaderCultureInfo As System.Globalization.CultureInfo
        Get
            Return ViewStateOrDefault("LoaderCultureInfo", Resource.CultureInfo)
        End Get
        Set(value As System.Globalization.CultureInfo)
            ViewState("LoaderCultureInfo") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNdeleteCurrentActivity, True)
            .setButton(BTNsaveCurrentActivity, True)
            .setLinkButton(LNBcloseCurrentActivity, False, True)
            .setLiteral(LTeditSettings)
            .setLiteral(LTeditLinks)
            .setLiteral(LTeditResources)
            .setLiteral(LTeditCompletion)

            .setLabel(LBactivityStartDate_t)
            .setLabel(LBactivityEndDate_t)
            .setLabel(LBactivityName_t)
            .setLabel(LBactivityDuration_t)
            .setLabel(LBactivityDeadLine_t)
            .setLabel(LBactivityDescription_t)
            .setLabel(LBactivityNote_t)

            .setLabel(LBactivityNewPredecessor_t)
            .setLabel(LBactivityPredecessor_t)
            .setLiteral(LTlinkIdActivity_t)
            .setLiteral(LTlinkType_t)
            .setLiteral(LTlinkLeadlag)
            .setLiteral(LTlinkActivityName_t)

            SLBavailablePredecessors.Attributes.Add("data-placeholder", Resource.getValue("SelectActivity.data-placeholder"))
            .setLinkButton(LNBaddPredecessorToActivity, False, True)

            .setLabel(LBactivityCompletionStartDate_t)
            .setLabel(LBactivityCompletionEndDate_t)
            .setLabel(LBactivityCompletion_t)
            .setLinkButton(LNBconfirmActivityCompletion, False, True)

            .setLabel(LBactivityCompletionStatus_t)

            .setLabel(LBactivityCompletionResources_t)
            .setLiteral(LTactivityResourceAssigned_t)
            .setLiteral(LTactivityResourceCompletion_t)


        End With
    End Sub
#End Region


    Private Sub LNBcloseCurrentActivity_Click(sender As Object, e As System.EventArgs) Handles LNBcloseCurrentActivity.Click
        Activities = Nothing
        If RaiseCommandEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub

#Region "Implements"
    Public Sub InitializeControl(idCommunity As Integer, idActivity As Long, activities As List(Of dtoLiteMapActivity)) Implements IViewEditActivity.InitializeControl
        CTRLmessages.Visible = False
        ProjectIdCommunity = idCommunity
        Me.HDNselectedTab.Value = "0"
        CurrentPresenter.InitView(idActivity, UnknownUser, activities)
    End Sub
    Private Sub DisplayUnknownActivity() Implements IViewEditActivity.DisplayUnknownActivity
        Me.AllowDelete = False
        Me.AllowEdit = False
        Me.MLVsettings.SetActiveView(VIWunknownActivity)
        LTmessage.Text = Resource.getValue("DisplayUnknownActivity")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewEditActivity.DisplaySessionTimeout
        Me.AllowDelete = False
        Me.AllowEdit = False
        Me.MLVsettings.SetActiveView(VIWunknownActivity)
        LTmessage.Text = Resource.getValue("IViewEditActivity.DisplaySessionTimeout")
    End Sub
    Private Sub LoadResources(allowEdit As Boolean, resources As List(Of dtoResource), selected As List(Of Long)) Implements IViewEditActivity.LoadResources
        CTRLselectActivityResources.InitializeControl(Not allowEdit, resources, selected, "", Resource.getValue("ManageResourceFromProjectMap"))
    End Sub
    Private Sub LoadSettings(activity As dtoActivity, allowDurationEstimated As Boolean, allowMilestones As Boolean) Implements IViewEditActivity.LoadSettings
        Me.MLVsettings.SetActiveView(VIWsettings)
        LIeditLinks.Visible = activity.Permission.SetPredecessors OrElse activity.Permission.ViewPredecessors
        DVeditLinks.Visible = activity.Permission.SetPredecessors OrElse activity.Permission.ViewPredecessors
        DVnewLink.Visible = activity.Permission.SetPredecessors
        LIeditResources.Visible = activity.Permission.SetResources
        DVeditResources.Visible = activity.Permission.SetResources
        LIcompletion.Visible = activity.Permission.SetMyCompletion OrElse activity.Permission.SetOthersCompletion
        THlinkActions.Visible = activity.Permission.SetPredecessors
        THcompletionAction.Visible = activity.Permission.SetMyCompletion OrElse activity.Permission.SetOthersCompletion
        LNBaddPredecessorToActivity.Visible = activity.Permission.SetPredecessors

        TXBname.Text = activity.Name
        TXBname.ReadOnly = Not activity.Permission.Edit
        TXBdescription.Text = activity.Description
        TXBdescription.ReadOnly = Not activity.Permission.Edit
        TXBnote.Text = activity.Note
        TXBnote.ReadOnly = Not activity.Permission.Edit

        If allowDurationEstimated Then
            CTRLdurationInput.ValidationExpression = LTdurationEstimatedRegex.Text
        Else
            CTRLdurationInput.ValidationExpression = LTdurationRegex.Text
        End If
        If Not allowMilestones Then
            CTRLdurationInput.ValidationExpression = Replace(CTRLdurationInput.ValidationExpression, "\d", "[1-9]")
        End If
        If activity.IsSummary OrElse Not activity.Permission.SetDuration Then
            LBactivityDuration_t.AssociatedControlID = "LBactivityDuration"
            LBactivityDuration.Text = activity.Duration.Value & IIf(activity.Duration.IsEstimated, "?", "")
            If Not CTRLdurationInput.ContainerCssClass.Contains("disabled") Then
                CTRLdurationInput.ContainerCssClass &= " disabled"
            End If

            CTRLdurationInput.Visible = False
            LBactivityDuration.Visible = True
        Else
            If CTRLdurationInput.ContainerCssClass.Contains("disabled") Then
                CTRLdurationInput.ContainerCssClass = CTRLdurationInput.ContainerCssClass.Replace(" disabled", "")
            End If
            LBactivityDuration_t.AssociatedControlID = "CTRLdurationInput"
            LBactivityDuration.Visible = False
            CTRLdurationInput.Visible = True
            CTRLdurationInput.AutoInitialize(activity.Duration.Value & IIf(activity.Duration.IsEstimated, "?", ""))
        End If

        If Not activity.Permission.SetStartDate OrElse activity.IsSummary Then
            CTRLstartDateInput.ReadOnlyInput = True
            If Not CTRLstartDateInput.ContainerCssClass.Contains("disabled") Then
                CTRLstartDateInput.ContainerCssClass &= " disabled"
            End If
        Else
            If CTRLstartDateInput.ContainerCssClass.Contains("disabled") Then
                CTRLstartDateInput.ContainerCssClass = CTRLstartDateInput.ContainerCssClass.Replace(" disabled", "")
            End If
        End If
        If activity.EarlyStartDate.HasValue Then
            CTRLstartDateInput.AutoInitialize(activity.EarlyStartDate.Value.ToString(CurrentShortDatePattern))
            CTRLcompletionStartDate.AutoInitialize(activity.EarlyStartDate.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLstartDateInput.AutoInitialize("")
            CTRLcompletionStartDate.AutoInitialize("")
        End If

        If activity.EarlyFinishDate.HasValue Then
            CTRLendDateInput.AutoInitialize(activity.EarlyFinishDate.Value.ToString(CurrentShortDatePattern))
            CTRLcompletionEndDate.AutoInitialize(activity.EarlyFinishDate.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLendDateInput.AutoInitialize("")
            CTRLcompletionEndDate.AutoInitialize("")
        End If
        If Not activity.Permission.SetDeadline Then
            CTRLdeadlineInput.ReadOnlyInput = True
            If Not CTRLdeadlineInput.ContainerCssClass.Contains("disabled") Then
                CTRLdeadlineInput.ContainerCssClass &= " disabled"
            End If
        Else
            If CTRLdeadlineInput.ContainerCssClass.Contains("disabled") Then
                CTRLdeadlineInput.ContainerCssClass = CTRLdeadlineInput.ContainerCssClass.Replace(" disabled", "")
            End If
        End If
        If activity.Deadline.HasValue Then
            CTRLdeadlineInput.AutoInitialize(activity.Deadline.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLdeadlineInput.AutoInitialize("")
        End If

        Dim availableStatus As List(Of ProjectItemStatus) = (From e As ProjectItemStatus In [Enum].GetValues(GetType(ProjectItemStatus)) Select e).ToList()
        'If activity.Completeness = 100 AndAlso activity.IsCompleted Then
        '    availableStatus = availableStatus.Where(Function(s) (s <> ProjectItemStatus.notstarted AndAlso activity.Status <> ProjectItemStatus.notstarted) OrElse (s <> ProjectItemStatus.started AndAlso activity.Status <> s)).ToList()
        'ElseIf activity.Completeness > 0 Then
        '    availableStatus = availableStatus.Where(Function(s) s <> ProjectItemStatus.notstarted AndAlso activity.Status <> s).ToList()
        'Else
        '    availableStatus = availableStatus.Where(Function(s) s <> ProjectItemStatus.completed).ToList()
        'End If

        DDLstatus.DataSource = availableStatus.Select(Function(e) New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Translation.ProjectItemStatus." & e.ToString)}).ToList()
        DDLstatus.DataTextField = "Translation"
        DDLstatus.DataValueField = "Id"
        DDLstatus.DataBind()
        DDLstatus.Enabled = Not activity.IsSummary AndAlso (activity.Permission.SetCompleteness OrElse activity.Permission.Edit)
        DDLstatus.SelectedValue = activity.Status
    End Sub
    Private Function CurrentSettings() As dtoActivity Implements IViewEditActivity.CurrentSettings
        Dim activity As New dtoActivity
        activity.Id = IdActivity


        activity.Name = TXBname.Text
        activity.Description = TXBdescription.Text
        activity.Note = TXBnote.Text

        Dim oldDuration As New dtoDuration()
        Dim newDuration As New dtoDuration()
        If CTRLdurationInput.OldValue.Contains("?") Then
            Double.TryParse(Replace(CTRLdurationInput.OldValue, "?", ""), oldDuration.Value)
            oldDuration.IsEstimated = True
        Else
            Double.TryParse(CTRLdurationInput.OldValue, oldDuration.Value)
        End If

        If CTRLdurationInput.NewValue.Contains("?") Then
            Double.TryParse(Replace(CTRLdurationInput.NewValue, "?", ""), newDuration.Value)
            newDuration.IsEstimated = True
        Else
            If Not Double.TryParse(CTRLdurationInput.NewValue, newDuration.Value) Then
                newDuration = oldDuration
            End If
        End If
        activity.Duration = newDuration


        Try
            activity.EarlyStartDate = GetDateFromString(CTRLstartDateInput.OldValue, Nothing)
            If Not String.IsNullOrEmpty(CTRLstartDateInput.NewValue) AndAlso CTRLstartDateInput.IsValid Then
                activity.EarlyStartDate = GetDateFromString(CTRLstartDateInput.NewValue, activity.EarlyStartDate)
            End If
        Catch ex As Exception

        End Try

        Try
            If String.IsNullOrEmpty(CTRLdeadlineInput.OldValue) Then
                activity.Deadline = Nothing
            Else
                activity.Deadline = GetDateFromString(CTRLdeadlineInput.OldValue, Nothing)
            End If

            If Not String.IsNullOrEmpty(CTRLdeadlineInput.NewValue) AndAlso CTRLdeadlineInput.IsValid Then
                activity.Deadline = GetDateFromString(CTRLdeadlineInput.NewValue, activity.Deadline)
            End If
        Catch ex As Exception

        End Try
        If activity.EarlyStartDate.HasValue AndAlso activity.EarlyStartDate.Value.Hour = 0 Then
            activity.EarlyStartDate = activity.EarlyStartDate.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
        End If
        If activity.Deadline.HasValue AndAlso activity.Deadline.Value.Hour = 0 Then
            activity.Deadline = activity.Deadline.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
        End If

        activity.Status = CInt(DDLstatus.SelectedValue)
        Return activity
    End Function

#Region "Links"
    Private Sub LoadAvailableLinks(activities As Dictionary(Of Long, String), links As List(Of dtoActivityLink)) Implements IViewEditActivity.LoadAvailableLinks
        RPTpredecessors.DataSource = links
        RPTpredecessors.DataBind()
        SLBavailablePredecessors.DataSource = activities
        SLBavailablePredecessors.DataValueField = "Key"
        SLBavailablePredecessors.DataTextField = "Value"
        SLBavailablePredecessors.DataBind()
    End Sub
    Private Sub DisplayLinkAdded(count As Integer) Implements IViewEditActivity.DisplayLinkAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewEditActivity.DisplayLinkAdded." & IIf(count > 1, "n", "1")), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub
    Private Sub DisplayLinkRemoved() Implements IViewEditActivity.DisplayLinkRemoved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewEditActivity.DisplayLinkRemoved"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub
    Private Sub DisplayUnableToAddLink() Implements IViewEditActivity.DisplayUnableToAddLink
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewEditActivity.DisplayUnableToAddLink"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnableToRemoveLink() Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewEditActivity.DisplayUnableToRemoveLink
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewEditActivity.DisplayUnableToRemoveLink"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    'Private Sub DisplayLinksSaved() Implements IViewEditActivity.DisplayLinksSaved

    'End Sub
    'Private Sub DisplayLinksUnsaved() Implements IViewEditActivity.DisplayLinksUnsaved

    'End Sub

    Private Sub DisplayLinksInCycles(items As List(Of dtoActivityLink)) Implements IViewEditActivity.DisplayLinksInCycles
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl("DisplayLinksInCycles", lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Function CurrentLinks() As List(Of dtoActivityLink) Implements IViewEditActivity.CurrentLinks
        Dim items As List(Of dtoActivityLink) = LoaderLinks

        For Each row As RepeaterItem In RPTpredecessors.Items
            Dim idLink As Long = CLng(DirectCast(row.FindControl("LTidLink"), Literal).Text)
            Dim uniqueId As String = DirectCast(row.FindControl("LTuniqueId"), Literal).Text

            Dim item As dtoActivityLink = items.Where(Function(i) i.UniqueId = uniqueId).FirstOrDefault()
            If Not IsNothing(item) Then
                item.Type = DirectCast(row.FindControl("DDLlinkType"), DropDownList).SelectedValue
                Dim t As String = DirectCast(row.FindControl("TXBleadlag"), TextBox).Text
                If String.IsNullOrEmpty(t) Then
                    item.LeadLag = 0
                ElseIf IsNumeric(t) Then
                    item.LeadLag = t
                Else
                    DirectCast(row.FindControl("TXBleadlag"), TextBox).Text = item.LeadLag
                End If
            End If
        Next
        Return items
    End Function
    Private Sub ReloadSettings(activity As dtoActivity, allowDurationEstimated As Boolean, allowMilestones As Boolean) Implements IViewEditActivity.ReloadSettings
        LIeditLinks.Visible = activity.Permission.SetPredecessors OrElse activity.Permission.ViewPredecessors
        DVeditLinks.Visible = activity.Permission.SetPredecessors OrElse activity.Permission.ViewPredecessors
        LIcompletion.Visible = activity.Permission.SetMyCompletion OrElse activity.Permission.SetOthersCompletion
        DVnewLink.Visible = activity.Permission.SetPredecessors

        TXBname.ReadOnly = Not activity.Permission.Edit
        TXBdescription.ReadOnly = Not activity.Permission.Edit
        TXBnote.ReadOnly = Not activity.Permission.Edit

        If Not activity.Permission.Edit Then
            TXBname.Text = activity.Name
            TXBnote.Text = activity.Note
            TXBdescription.Text = activity.Description
        End If


        If allowDurationEstimated Then
            CTRLdurationInput.ValidationExpression = LTdurationEstimatedRegex.Text
        Else
            CTRLdurationInput.ValidationExpression = LTdurationRegex.Text
        End If
        If Not allowMilestones Then
            CTRLdurationInput.ValidationExpression = Replace(CTRLdurationInput.ValidationExpression, "\d", "[1-9]")
        End If

        If Not activity.Permission.SetStartDate OrElse activity.IsSummary Then
            CTRLstartDateInput.ReadOnlyInput = True
            If Not CTRLstartDateInput.ContainerCssClass.Contains("disabled") Then
                CTRLstartDateInput.ContainerCssClass &= " disabled"
            End If

            If activity.EarlyStartDate.HasValue Then
                CTRLstartDateInput.AutoInitialize(activity.EarlyStartDate.Value.ToString(CurrentShortDatePattern))
            Else
                CTRLstartDateInput.AutoInitialize("")
            End If
        Else
            If CTRLstartDateInput.ContainerCssClass.Contains("disabled") Then
                CTRLstartDateInput.ContainerCssClass = CTRLstartDateInput.ContainerCssClass.Replace(" disabled", "")
            End If

        End If


        If activity.EarlyFinishDate.HasValue Then
            CTRLendDateInput.AutoInitialize(activity.EarlyFinishDate.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLendDateInput.AutoInitialize("")
        End If
    End Sub
#End Region

#Region "Assignments"
    Private Function CurrentAssignments() As List(Of dtoActivityCompletion) Implements IViewEditActivity.CurrentAssignments
        Dim items As List(Of dtoActivityCompletion) = LoaderAssignments

        Dim idResources As List(Of Long) = CTRLselectActivityResources.GetSelectedIdResources
        For Each row As RepeaterItem In RPTresourcesCompletion.Items
            Dim uniqueId As String = DirectCast(row.FindControl("LTuniqueId"), Literal).Text

            Dim item As dtoActivityCompletion = items.Where(Function(i) i.UniqueId = uniqueId).FirstOrDefault()
            If Not IsNothing(item) Then
                Dim t As String = DirectCast(row.FindControl("TXBcompletion"), TextBox).Text
                If String.IsNullOrEmpty(t) Then
                    item.Completeness = 0
                ElseIf IsNumeric(t) Then
                    item.Completeness = IIf(CInt(t) > 100, 100, IIf(CInt(t) < 0, 0, CInt(t)))
                Else
                    DirectCast(row.FindControl("TXBcompletion"), TextBox).Text = item.Completeness
                End If
                If item.Completeness < 100 Then
                    item.IsApproved = False
                End If
                item.Deleted = (idResources.Contains(item.IdResource))
            End If

        Next
        Return items
    End Function
    Private Sub LoadCompletion(completion As Integer, completed As Boolean, assignments As List(Of dtoActivityCompletion), permission As dtoActivityPermission) Implements IViewEditActivity.LoadCompletion
        LBactivityCompletion.Text = completion & "%"
        TXBcompletion.Text = completion
        'If Not permission.SetCompleteness Then
        LBactivityCompletion.Visible = True
        LBcompletionPercentage.Visible = False
        TXBcompletion.Visible = False
        'Else
        '    If Not assignments.Any Then

        '    End If
        'End If
        'LBactivityCompletion.Visible = Not permission.SetCompleteness

        'TXBcompletion.Visible = permission.SetCompleteness
        'RFVcompletion.Visible = permission.SetCompleteness
        'RNVcompletion.Visible = permission.SetCompleteness
        LNBconfirmActivityCompletion.Visible = permission.SetCompleteness
        RPTresourcesCompletion.DataSource = assignments
        RPTresourcesCompletion.DataBind()
        isCompleted = completed
        DVassignments.Visible = True
        DDLstatus.SelectedValue = IIf(isCompleted AndAlso completion = 100 AndAlso DDLstatus.SelectedValue <> CInt(ProjectItemStatus.suspended), CInt(ProjectItemStatus.completed), DDLstatus.SelectedValue)

        THcompletionAction.Visible = THcompletionAction.Visible AndAlso assignments.Where(Function(a) a.AllowDelete).Any()
        Resource.setLinkButton(LNBconfirmActivityCompletion, False, True)
    End Sub

    Private Sub ReloadCompletion(completion As Integer, completed As Boolean, assignments As List(Of dtoActivityCompletion)) Implements IViewEditActivity.ReloadCompletion
        THcompletionAction.Visible = THcompletionAction.Visible AndAlso assignments.Where(Function(a) a.AllowDelete).Any()
        TXBcompletion.Text = completion
        LBactivityCompletion.Text = completion & "%"
        isCompleted = completed
        RPTresourcesCompletion.DataSource = assignments
        RPTresourcesCompletion.DataBind()
        DDLstatus.SelectedValue = IIf(isCompleted AndAlso completion = 100 <> CInt(ProjectItemStatus.suspended), CInt(ProjectItemStatus.completed), DDLstatus.SelectedValue)
    End Sub

    Private Sub RemoveResourceFromSelection(idResource As Long) Implements IViewEditActivity.RemoveResourceFromSelection
        CTRLselectActivityResources.ChangeResourceSelection(idResource, False)
    End Sub

    Private Sub LoadSummaryCompletion(completion As Integer, completed As Boolean, permission As dtoActivityPermission) Implements IViewEditActivity.LoadSummaryCompletion
        DVassignments.Visible = False

        LBactivityCompletion.Visible = True
        TXBcompletion.Visible = False
        LBactivityCompletion.Text = completion & "%"
        RFVcompletion.Visible = False
        RNVcompletion.Visible = False
        LNBconfirmActivityCompletion.Visible = False
        LNBconfirmActivityCompletion.Visible = Not completed

        isCompleted = completed
        DDLstatus.Enabled = False

        Resource.setLinkButtonToValue(LNBconfirmActivityCompletion, "summary", False, True)
    End Sub
#End Region

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idProject As Long, action As ModuleProjectManagement.ActionType) Implements IViewEditActivity.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Project, idProject.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayUnableToRemoveActivity(name As String, children As Long) Implements IViewEditActivity.DisplayUnableToRemoveActivity
        DisplayActivityMessage("DisplayUnableToRemoveActivity", Helpers.MessageType.error, name, children)
    End Sub
    Private Sub DisplayActivityMessage(messageKey As String, type As Helpers.MessageType, activityName As String, children As Long)
        CTRLmessages.Visible = True
        Dim message As String = messageKey & "."
        If children < 0 Then
            children = 0
        End If
        If children < 2 Then
            message &= children.ToString
        Else
            message &= "n"
        End If
        If Not String.IsNullOrEmpty(activityName) Then
            message &= ".Name"
            activityName = activityName.Replace("""", "'")
        End If
        CTRLmessages.Visible = True
        If String.IsNullOrEmpty(activityName) Then
            If children > 1 Then
                CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), children), type)
            Else
                CTRLmessages.InitializeControl(Resource.getValue(message), type)
            End If
        ElseIf children <= 1 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), activityName), type)
        Else
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), children, activityName), type)
        End If
    End Sub
    Private Sub DisplayActivitySaved(idActivity As Long, startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?)) Implements IViewEditActivity.DisplayActivitySaved
        CTRLmessages.Visible = False
        If RaiseCommandEvents Then
            RaiseEvent SavedActivity(idActivity, startDate, endDate, deadLine)
        End If
    End Sub
    Private Sub DisplayUnableToSaveActivity() Implements IViewEditActivity.DisplayUnableToSaveActivity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewEditActivity.DisplayUnableToSaveActivity"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Function GetDateFromString(ByVal inputDate As String, ByVal defaultDateTime As DateTime?) As DateTime
        Try
            Dim dItems As String() = inputDate.Split("/")
            If dItems.Count = 3 Then
                If dItems(0).Count = 1 Then
                    dItems(0) = "0" + dItems(0)
                End If
                If dItems(1).Count = 1 Then
                    dItems(1) = "0" + dItems(1)
                End If
            End If
            Return DateTime.ParseExact(String.Join("/", dItems), CurrentShortDatePattern, LoaderCultureInfo.CurrentCulture.InvariantCulture)
        Catch ex As Exception
            Return defaultDateTime
        End Try
        Return defaultDateTime
    End Function
#End Region

#Region "Predecessors"
    Private Sub RPTpredecessors_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpredecessors.ItemDataBound
        Dim dto As dtoActivityLink = e.Item.DataItem
        Dim oCell As HtmlTableCell = e.Item.FindControl("TDlinkActions")
        oCell.Visible = dto.AllowDelete
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBvirtualDeletePredecessor")
        oLinkButton.Visible = dto.AllowDelete
        If dto.AllowDelete Then
            Resource.setLinkButton(oLinkButton, False, True)
        End If
        Dim oDropdownList As DropDownList = e.Item.FindControl("DDLlinkType")
        LoadDropDownList(oDropdownList, dto.Type)
    End Sub
    Private Sub RPTpredecessors_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTpredecessors.ItemCommand
        Select Case e.CommandName
            Case "virtualdelete"
                CurrentPresenter.VirtualDeleteLink(IdProject, IdActivity, e.CommandArgument, Activities)
        End Select
    End Sub
    Private Sub LNBaddPredecessorToActivity_Click(sender As Object, e As System.EventArgs) Handles LNBaddPredecessorToActivity.Click
        CurrentPresenter.AddLink(IdProject, IdActivity, GetSelectedActivities, CurrentLinks, Activities)
    End Sub
    Private Function GetSelectedActivities() As List(Of Long)
        If SLBavailablePredecessors.SelectedIndex > -1 Then
            Return (From i As ListItem In SLBavailablePredecessors.Items Where i.Selected Select CLng(i.Value)).ToList
        Else
            Return New List(Of Long)
        End If
    End Function
    Private Sub LoadDropDownList(oDropdownList As DropDownList, selected As PmActivityLinkType)
        oDropdownList.DataSource = (From t In [Enum].GetValues(GetType(PmActivityLinkType)) Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(t), .Translation = Resource.getValue("Translation." & t.ToString)}).ToList()
        oDropdownList.DataTextField = "Translation"
        oDropdownList.DataValueField = "Id"
        oDropdownList.DataBind()
        If Not IsNothing(oDropdownList.Items.FindByValue(CInt(selected))) Then
            oDropdownList.SelectedValue = CInt(selected)
        End If
    End Sub
#End Region

#Region "Completion"
    Private Sub RPTresourcesCompletion_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTresourcesCompletion.ItemDataBound
        Dim dto As dtoActivityCompletion = e.Item.DataItem
        Dim oCell As HtmlTableCell = e.Item.FindControl("TDcompletionAction")
        oCell.Visible = dto.AllowDelete

        If dto.AllowDelete Then
            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBvirtualDeleteAssignment")
            oLinkButton.Visible = dto.AllowDelete
            Resource.setLinkButton(oLinkButton, False, True)
        End If

        Dim oLabel As Label = e.Item.FindControl("LBcompletion")
        oLabel.Visible = Not dto.AllowEdit

        Dim oTextbox As TextBox = e.Item.FindControl("TXBcompletion")
        oTextbox.Visible = dto.AllowEdit

        Dim oRangeValidator As RangeValidator = e.Item.FindControl("RNVcompletion")
        oRangeValidator.Visible = dto.AllowEdit

        Dim oRequiredFieldValidator As RequiredFieldValidator = e.Item.FindControl("RFVcompletion")
        oRequiredFieldValidator.Visible = dto.AllowEdit
    End Sub
    'Private Sub RPTresourcesCompletion_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTresourcesCompletion.ItemCommand
    '    Select Case e.CommandName
    '        Case "virtualdelete"
    '            CurrentPresenter.VirtualDeleteAssignment(e.CommandArgument)
    '    End Select
    'End Sub
    Private Sub LNBconfirmActivityCompletion_Click(sender As Object, e As System.EventArgs) Handles LNBconfirmActivityCompletion.Click
        CurrentPresenter.ConfirmCompletion(CTRLselectActivityResources.GetSelectedIdResources())
    End Sub
    Protected Function GetRowCssClass(ByVal isDeleted As Boolean) As String
        Return IIf(isDeleted, "disabled", "")
    End Function
#End Region


    Private Sub BTNdeleteCurrentActivity_Click(sender As Object, e As System.EventArgs) Handles BTNdeleteCurrentActivity.Click
        If CurrentPresenter.VirtualDeleteActivity(IdProject, IdActivity, Activities) AndAlso RaiseCommandEvents Then
            RaiseEvent VirtualDeleteActivity(IdActivity)
        End If
    End Sub

    Private Sub BTNsaveCurrentActivity_Click(sender As Object, e As System.EventArgs) Handles BTNsaveCurrentActivity.Click
        Dim startDate As DateTime?
        Dim endDate As DateTime?
        Dim deadline As DateTime?
        RaiseEvent GetProjectDate(startDate, endDate, deadline)
        CurrentPresenter.SaveActivity(IdProject, IdActivity, CurrentSettings, CTRLselectActivityResources.GetSelectedIdResources, CurrentLinks, CurrentAssignments, isCompleted, startDate, endDate, deadline)

    End Sub
   
End Class