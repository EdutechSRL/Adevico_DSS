
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain

Public Class UC_ProjectBaseSettings
    Inherits BaseControl
    Implements IViewProjectSettings



#Region "Context"
    'Private _Presenter As ManageProjectResourcesPresenter
    'Private ReadOnly Property CurrentPresenter() As ManageProjectResourcesPresenter
    '    Get
    '        If IsNothing(_Presenter) Then
    '            _Presenter = New ManageProjectResourcesPresenter(Me.PageUtility.CurrentContext, Me)
    '        End If
    '        Return _Presenter
    '    End Get
    'End Property
#End Region

#Region "Implements"
    Public Property AllowEditDateCalculationByCpm As Boolean Implements IViewProjectSettings.AllowEditDateCalculationByCpm
        Get
            Return ViewStateOrDefault("AllowEditDateCalculationByCpm", False)
        End Get
        Set(value As Boolean)
            Me.CBXdateCalculationByCpm.Enabled = value
            ViewState("AllowEditDateCalculationByCpm") = value
        End Set
    End Property
    Public Property AllowSelectOwnerFromCommunity As Boolean Implements IViewProjectSettings.AllowSelectOwnerFromCommunity
        Get
            Return LNBselectOwnerFromCommunity.Enabled
        End Get
        Set(value As Boolean)
            LNBselectOwnerFromCommunity.Visible = value
            LNBselectOwnerFromCommunity.Enabled = value
        End Set
    End Property
    Public Property AllowSelectOwnerFromResources As Boolean Implements IViewProjectSettings.AllowSelectOwnerFromResources
        Get
            Return LNBselectOwnerFromResources.Enabled
        End Get
        Set(value As Boolean)
            LNBselectOwnerFromResources.Visible = value
            LNBselectOwnerFromResources.Enabled = value
        End Set
    End Property
    Public Property DisplayCompletion As Boolean Implements IViewProjectSettings.DisplayCompletion
        Get
            Return ViewStateOrDefault("DisplayCompletion", False)
        End Get
        Set(value As Boolean)
            Me.DVcompletion.Visible = value
            ViewState("DisplayCompletion") = value
        End Set
    End Property
    Public Property DisplayDuration As Boolean Implements IViewProjectSettings.DisplayDuration
        Get
            Return Me.TXBduration.Enabled
        End Get
        Set(value As Boolean)
            Me.DVduration.Visible = value
            Me.TXBduration.Enabled = value
        End Set
    End Property
    Public Property DisplayActivitiesToAdd As Boolean Implements IViewProjectSettings.DisplayActivitiesToAdd
        Get
            Return Me.TXBactivitiesToAdd.Enabled
        End Get
        Set(value As Boolean)
            Me.DVactivitiesToAdd.Visible = value
            Me.TXBactivitiesToAdd.Enabled = value
        End Set
    End Property

    Public Property DisplayOwnerChanger As Boolean Implements IViewProjectSettings.DisplayOwnerChanger
        Get
            Return ViewStateOrDefault("DisplayOwnerChanger", False)
        End Get
        Set(value As Boolean)
            DVprojectOwner.Visible = value
            ViewState("DisplayOwnerChanger") = value
        End Set
    End Property
    Public Property InEditingMode As Boolean Implements IViewProjectSettings.InEditingMode
        Get
            Return ViewStateOrDefault("InEditingMode", False)
        End Get
        Set(value As Boolean)
            ViewState("InEditingMode") = value
            SetItemsEditingMode(value)
        End Set
    End Property
    Public Property IsInitialized As Boolean Implements IViewProjectSettings.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property DisplayStatus As Boolean Implements IViewProjectSettings.DisplayStatus
        Get
            Return ViewStateOrDefault("DisplayStatus", False)
        End Get
        Set(value As Boolean)
            DVstatus.Visible = value
            ViewState("DisplayStatus") = value
        End Set
    End Property
    Public Property ForPortal As Boolean Implements IViewProjectSettings.ForPortal
        Get
            Return ViewStateOrDefault("ForPortal", True)
        End Get
        Set(value As Boolean)
            ViewState("ForPortal") = value
        End Set
    End Property
    Public Property IsPersonal As Boolean Implements IViewProjectSettings.IsPersonal
        Get
            Return ViewStateOrDefault("IsInitialized", True)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
            LBprojectIsPersonal.Visible = value
        End Set
    End Property
    Public Property IdProject As Long Implements IViewProjectSettings.IdProject
        Get
            Return ViewStateOrDefault("IdProject", 0)
        End Get
        Set(value As Long)
            ViewState("IdProject") = value
        End Set
    End Property
    Public Property IdCommunity As Integer Implements IViewProjectSettings.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property Status As ProjectItemStatus Implements IViewProjectSettings.Status
        Get
            Return ViewStateOrDefault("Status", ProjectItemStatus.notStarted)
        End Get
        Set(value As ProjectItemStatus)
            ViewState("Status") = value
        End Set
    End Property
    Public Property ActivitiesToAdd As Integer Implements IViewProjectSettings.ActivitiesToAdd
        Get
            If String.IsNullOrEmpty(TXBactivitiesToAdd.Text) OrElse Not IsNumeric(TXBactivitiesToAdd.Text) Then
                Return 0
            Else
                Return CInt(TXBactivitiesToAdd.Text)
            End If
        End Get
        Set(value As Integer)
            TXBactivitiesToAdd.Text = value
        End Set
    End Property
    Private Property DefaultWorkingDay As dtoWorkingDay Implements IViewProjectSettings.DefaultWorkingDay
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
    Public Event RequireNewOwnerFromCommunity()
    Public Event RequireNewOwnerFromResources()
    Public Event AddNewOwner(ByVal idPerson As Integer)
    Public Event SelectedNewOwner(ByVal idResource As Long)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBprojectName_t)
            .setLabel(LBprojectIsPersonal)
            .setLabel(LBprojectDescription_t)
            .setLabel(LBstartdate_t)
            .setLabel(LBendDate_t)
            .setLabel(LBdeadline_t)
            .setLabel(LBdefaultResourceForActivity)
            .setCustomValidator(CSVdeadline)
            .setLabel(LBduration_t)
            .setLabel(LBweekcalendar_t)
            .setHyperLink(HYPmonday, False, True)
            .setHyperLink(HYPtuesday, False, True)
            .setHyperLink(HYPwednesday, False, True)
            .setHyperLink(HYPthursday, False, True)
            .setHyperLink(HYPfriday, False, True)
            .setHyperLink(HYPsaturday, False, True)
            .setHyperLink(HYPsunday, False, True)
            .setHyperLink(HYPmanageCalendars, False, True)
            .setLabel(LBprojectOwner_t)
            .setLinkButton(LNBselectOwnerFromResources, False, True)
            .setLinkButton(LNBselectOwnerFromCommunity, False, True)
            .setLabel(LBprojectDefaultResourceForActivity_t)
            .setLabel(LBdefaultResourceForActivityDescription)
            .setLabel(LBselectDefaultResources_t)
            .setLabel(LBprojectAvailability_t)
            .setLabel(LBcompletion_t)
            .setLabel(LBprojectStatus_t)
            .setLabel(LBprojectAllowMilestones_t)
            .setLabel(LBprojectAllowMilestones)
            .setLabel(LBprojectAllowMilestonesDescription)
            .setLabel(LBprojectVisibility_t)
            .setLabel(LBprojectVisibility)
            .setLabel(LBprojectVisibilityDescription)
            .setLabel(LBprojectConfirmCompletion_t)
            .setLabel(LBprojectConfirmCompletion)
            .setLabel(LBprojectConfirmCompletionDescription)
            .setLabel(LBprojectDateCalculationByCpm_t)
            .setLabel(LBprojectDateCalculationByCpm)
            .setLabel(LBprojectDateCalculationByCpmDescription)
            .setLabel(LBallowEstimatedDuration_t)
            .setLabel(LBallowEstimatedDuration)
            .setLabel(LBallowEstimatedDurationDescription)
            .setCheckBoxList(CBLweekcalendar, "Monday")
            .setCheckBoxList(CBLweekcalendar, "Tuesday")
            .setCheckBoxList(CBLweekcalendar, "Wednesday")
            .setCheckBoxList(CBLweekcalendar, "Thursday")
            .setCheckBoxList(CBLweekcalendar, "Friday")
            .setCheckBoxList(CBLweekcalendar, "Saturday")
            .setCheckBoxList(CBLweekcalendar, "Sunday")
            .setLabel(LBprojectSummaryActivities_t)
            .setLabel(LBprojectSummaryActivities)
            .setLabel(LBprojectSummaryActivitiesDescription)
            .setLabel(LBprojectActivitiesToAdd_t)
            .setRangeValidator(RNVactivitiesToAdd)
            .setLabel(LBprojectActivitiesToAdd)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(project As dtoProject, owner As dtoResource, items As List(Of ProjectAvailability), ByVal selected As ProjectAvailability, fromView As PageListType, idContainerCommunity As Integer, Optional ByVal activitiesToadd As Integer = 0) Implements IViewProjectSettings.InitializeControl
        LoadProject(project, items, selected, activitiesToadd, fromView, idContainerCommunity)
        If IsNothing(owner) Then
            DisplayOwnerInfo(Me.Resource.getValue("UnknownUser"))
        Else
            DisplayOwnerInfo(owner.LongName)
        End If

    End Sub
    Private Sub DisplayOwnerInfo(name As String) Implements IViewProjectSettings.DisplayOwnerInfo
        LBprojectOwner.Text = name
    End Sub

    Public Function GetProject() As dtoProject Implements IViewProjectSettings.GetProject
        Dim dto As New dtoProject
        With dto
            .Id = IdProject
            .IdCommunity = IdCommunity
            .isPortal = ForPortal
            .isPersonal = IsPersonal
            .Status = Status

            .AllowEstimatedDuration = CBXallowEstimatedDuration.Checked
            .AllowMilestones = CBXallowMilestones.Checked
            If RBLavailability.SelectedIndex >= 0 Then
                .Availability = CInt(RBLavailability.SelectedValue)
            Else
                .Availability = IIf(ID > 0, ProjectAvailability.Draft, ProjectAvailability.Suspended)
            End If
            .ConfirmCompletion = CBXconfirmCompletion.Checked
            .DateCalculationByCpm = CBXdateCalculationByCpm.Checked
            .DaysOfWeek = FlagDayOfWeek.None
            For Each fItem As FlagDayOfWeek In [Enum].GetValues(GetType(FlagDayOfWeek))
                If CBLweekcalendar.Items.Cast(Of ListItem)().Where(Function(i) i.Selected AndAlso i.Value = fItem.ToString).Any() Then
                    .DaysOfWeek = .DaysOfWeek Or fItem
                End If
            Next
            .Deadline = RDPdeadline.SelectedDate
            If .Deadline.HasValue AndAlso .Deadline.Value.Hour = 0 Then
                .Deadline = .Deadline.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
            End If
            If CBXdefaultResourceForActivity.Checked Then
                If Me.SLBresources.Items.Count > 0 Then
                    .DefaultActivityResources = Me.SLBresources.Items.Cast(Of ListItem).Where(Function(i) i.Selected).Select(Function(i) New dtoResource() With {.IdResource = CLng(i.Value)}).ToList()
                Else
                    .DefaultActivityResources = New List(Of dtoResource)
                End If
            Else
                .DefaultActivityResources = New List(Of dtoResource)
            End If
            .SetDefaultResourcesToNewActivity = CBXdefaultResourceForActivity.Checked AndAlso .DefaultActivityResources.Any()

            .Description = CTRLeditorDescription.HTML
            .Duration = TXBduration.Text
            .Name = TXBname.Text
            .AllowSummary = CBXallowSummaryActivities.Checked
            .StartDate = RDPstartDate.SelectedDate
            If .StartDate.HasValue AndAlso .StartDate.Value.Hour = 0 Then
                .StartDate = .StartDate.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
            End If
            If .StartDate.HasValue AndAlso .Deadline.HasValue AndAlso .StartDate.Value > .Deadline.Value Then
                .Deadline = .EndDate
                RDPdeadline.SelectedDate = .EndDate
            ElseIf .Deadline.HasValue AndAlso .Deadline.Value.Hour = 0 Then
                .Deadline = .Deadline.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
            End If
            .Visibility = IIf(CBXvisibility.Checked, ProjectVisibility.Full, ProjectVisibility.InvolvedTasks)
        End With
        Return dto
    End Function

    Private Sub LoadProject(project As dtoProject, items As List(Of ProjectAvailability), cAvailability As ProjectAvailability, ByVal activitiesToadd As Integer, fromView As PageListType, idContainerCommunity As Integer) Implements IViewProjectSettings.LoadProject
        InitializeCalendar(project, project.DaysOfWeek, project.CalendarsCount, project.DateExceptionsCount, fromView, idContainerCommunity)
        With project
            IdProject = .Id
            IdCommunity = .IdCommunity
            LBprojectIsPersonal.Visible = .isPersonal
            TXBname.Text = .Name
            CTRLeditorDescription.HTML = .Description

            RDPstartDate.SelectedDate = .StartDate
            LBendDate.Visible = (project.Id > 0)
            LBendDate_t.Visible = (project.Id > 0)
            If .EndDate.HasValue Then
                LBendDate.Text = .EndDate.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
            Else
                LBendDate_t.Visible = False
            End If

            RDPdeadline.SelectedDate = .Deadline
            If Not AllowEditDateCalculationByCpm AndAlso project.Id > 0 Then
                RDPdeadline.MinDate = RDPdeadline.MinDate
            End If
            CBXdefaultResourceForActivity.Checked = .DefaultActivityResources.Any() AndAlso .SetDefaultResourcesToNewActivity
            SLBresources.DataSource = .Resources
            SLBresources.DataTextField = "LongName"
            SLBresources.DataValueField = "IdResource"
            SLBresources.DataBind()
            If .DefaultActivityResources.Any() Then
                For Each resource As dtoResource In .DefaultActivityResources
                    Dim oListItem As ListItem = SLBresources.Items.FindByValue(resource.IdResource)
                    If Not IsNothing(oListItem) Then
                        oListItem.Selected = True
                    End If
                Next
            ElseIf .Id < 1 AndAlso SLBresources.Items.Count > 0 Then
                SLBresources.Items(0).Selected = True
            End If
            SLBresources.Attributes.Add("data-placeholder", Resource.getValue("DefaulResources.data-placeholder"))

            CBXallowEstimatedDuration.Checked = .AllowEstimatedDuration
            CBXconfirmCompletion.Checked = .ConfirmCompletion
            CBXdateCalculationByCpm.Checked = .DateCalculationByCpm
            CBXallowMilestones.Checked = .AllowMilestones
            CBXvisibility.Checked = (.Visibility = ProjectVisibility.Full)
            CBXallowSummaryActivities.Checked = .AllowSummary
            LBcompletion.Text = .Completeness & "%"

            TXBactivitiesToAdd.Text = activitiesToadd
            TXBduration.Text = .Duration
            TXBduration.Visible = Not .DateCalculationByCpm
            LBduration.Visible = .DateCalculationByCpm
            If .DateCalculationByCpm Then
                LBduration_t.AssociatedControlID = "LBduration"
            Else
                LBduration_t.AssociatedControlID = "TXBduration"
            End If
            LBduration.Text = .Duration & IIf(.IsDurationEstimated, "?", "")
            RBLavailability.Items.Clear()
            For Each item As ProjectAvailability In items
                RBLavailability.Items.Add(New ListItem(Resource.getValue("RBLavailability." & item.ToString), CInt(item)) With {.Selected = (item = cAvailability)})
            Next
            DVavailability.Visible = RBLavailability.Items.Count > 1
            LBprojectStatus.Text = Resource.getValue("Project.Select.ProjectItemStatus." & .Status.ToString)
            Status = .Status
        End With
    End Sub
    Private Sub InitializeCalendar(project As dtoProject, daysOfWeek As FlagDayOfWeek, calendar As Long, exceptions As Long, fromView As PageListType, idContainerCommunity As Integer) Implements IViewProjectSettings.InitializeCalendar
        HYPmanageCalendars.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.ProjectCalendars(project.Id, project.IdCommunity, project.isPortal, project.isPersonal, fromView, idContainerCommunity)
        Dim baseValue As String = ""
        Select Case calendar
            Case 0, 1
                baseValue &= calendar & "."
            Case Else
                If calendar > 1 Then
                    baseValue &= "n."
                Else
                    baseValue &= "0."
                End If
        End Select
        Select Case exceptions
            Case 0, 1
                baseValue &= exceptions
            Case Else
                If exceptions > 1 Then
                    baseValue &= "n"
                Else
                    baseValue &= "0"
                End If
        End Select
        If calendar > 1 AndAlso exceptions > 1 Then
            HYPmanageCalendars.Text = String.Format(Resource.getValue("manageCalendarsText." & baseValue), calendar, exceptions)
        ElseIf calendar > 1 Then
            HYPmanageCalendars.Text = String.Format(Resource.getValue("manageCalendarsText." & baseValue), calendar)
        ElseIf exceptions > 1 Then
            HYPmanageCalendars.Text = String.Format(Resource.getValue("manageCalendarsText." & baseValue), exceptions)
        Else
            HYPmanageCalendars.ToolTip = Resource.getValue("manageCalendarsToolTip." & baseValue)
        End If
        HYPmanageCalendars.ToolTip = Resource.getValue("manageCalendarsToolTip." & baseValue)
        HYPmanageCalendars.Visible = InEditingMode AndAlso (calendar > 0 OrElse exceptions > 0)

        If daysOfWeek = FlagDayOfWeek.AllWeek Then
            For Each item As ListItem In CBLweekcalendar.Items
                item.Selected = True
            Next
        Else
            Dim days As New List(Of String)
            Select Case daysOfWeek
                Case FlagDayOfWeek.Weekend
                    days.Add(FlagDayOfWeek.Saturday.ToString)
                    days.Add(FlagDayOfWeek.Sunday.ToString)
                Case FlagDayOfWeek.WorkWeek
                    days.Add(FlagDayOfWeek.Friday.ToString)
                    days.Add(FlagDayOfWeek.Monday.ToString)
                    days.Add(FlagDayOfWeek.Thursday.ToString)
                    days.Add(FlagDayOfWeek.Tuesday.ToString)
                    days.Add(FlagDayOfWeek.Wednesday.ToString)
                Case Else
                    For Each i As FlagDayOfWeek In [Enum].GetValues(GetType(FlagDayOfWeek))
                        If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(i, daysOfWeek) Then
                            days.Add(i.ToString)
                        End If
                    Next
            End Select
            For Each item As ListItem In (From i As ListItem In CBLweekcalendar.Items.Cast(Of ListItem)() Where days.Contains(i.Value))
                item.Selected = True
            Next
        End If

    End Sub
    Public Sub InitializeControlToSelectOwner(idCommunity As Integer, hideUsers As List(Of Integer)) Implements IViewProjectSettings.InitializeControlToSelectOwner
        Me.DVselectFromCommunity.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, False, idCommunity, hideUsers, Nothing, Resource.getValue("SelectOwnerFromCommunity.Description"))
    End Sub

    Public Sub InitializeControlToSelectOwnerFromProject(resources As List(Of dtoProjectResource)) Implements IViewProjectSettings.InitializeControlToSelectOwnerFromProject
        Me.DVselectFromProjectResource.Visible = True
        Me.CTRLselectFromResources.InitializeControl(resources, Resource.getValue("SelectOwnerFromProject.Description"))
    End Sub
    Public Sub UpdateDefaultResourceSelector(resources As List(Of dtoResource)) Implements IViewProjectSettings.UpdateDefaultResourceSelector
        Dim selected As List(Of Long) = (From i As ListItem In SLBresources.Items Where i.Selected Select CLng(i.Value)).ToList
        SLBresources.DataSource = resources
        SLBresources.DataTextField = "LongName"
        SLBresources.DataValueField = "IdResource"
        SLBresources.DataBind()
        If CBXdefaultResourceForActivity.Checked Then
            For Each idResource As Long In selected
                Dim oListItem As ListItem = SLBresources.Items.FindByValue(idResource)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
        End If
        If resources.Where(Function(r) r.ProjectRole = ActivityRole.ProjectOwner).Any() Then
            LBprojectOwner.Text = resources.Where(Function(r) r.ProjectRole = ActivityRole.ProjectOwner).FirstOrDefault().LongName
        End If
        SLBresources.Attributes.Add("data-placeholder", Resource.getValue("DefaulResources.data-placeholder"))
    End Sub
    Public Sub UpdateSettings(actions As dtoProjectSettingsSelectedActions, startDate As DateTime?, endDate As DateTime?) Implements IViewProjectSettings.UpdateSettings
        If actions.EstimatedAction = ConfirmActions.Hold Then
            CBXallowEstimatedDuration.Checked = Not CBXallowEstimatedDuration.Checked
        End If
        If actions.MilestonesAction = ConfirmActions.Hold Then
            CBXallowMilestones.Checked = Not CBXallowMilestones.Checked
        End If
        If actions.SummariesAction = ConfirmActions.Hold Then
            CBXallowSummaryActivities.Checked = Not CBXallowSummaryActivities.Checked
        End If
        If actions.CpmAction = ConfirmActions.Hold OrElse actions.ManualAction = ConfirmActions.Hold Then
            CBXdateCalculationByCpm.Checked = Not CBXdateCalculationByCpm.Checked
        End If
        If actions.DateAction = ConfirmActions.Hold Then
            RDPstartDate.SelectedDate = startDate
        End If
        If endDate.HasValue Then
            LBendDate.Text = endDate.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
        Else
            LBendDate.Text = ""
        End If

    End Sub
    Public Sub UpdateSettings(startDate As DateTime?, endDate As DateTime?) Implements IViewProjectSettings.UpdateSettings
        RDPstartDate.SelectedDate = startDate
        If endDate.HasValue Then
            LBendDate.Text = endDate.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
        Else
            LBendDate.Text = ""
        End If
    End Sub
#End Region

#Region "Internal"
    Private Sub SetItemsEditingMode(enabled As Boolean)
        TXBname.ReadOnly = Not enabled
        CTRLeditorDescription.IsEnabled = enabled
        CBLweekcalendar.Enabled = enabled
        HYPmonday.Enabled = enabled
        HYPfriday.Enabled = enabled
        HYPsaturday.Enabled = enabled
        HYPsunday.Enabled = enabled
        HYPthursday.Enabled = enabled
        HYPtuesday.Enabled = enabled
        HYPwednesday.Enabled = enabled

        CBXallowEstimatedDuration.Enabled = enabled
        CBXallowMilestones.Enabled = enabled
        CBXallowSummaryActivities.Enabled = enabled
        CBXconfirmCompletion.Enabled = enabled
        CBXdateCalculationByCpm.Enabled = enabled AndAlso AllowEditDateCalculationByCpm
        CBXdefaultResourceForActivity.Enabled = enabled
        CBXvisibility.Enabled = enabled

    End Sub

    Private Sub LNBselectOwnerFromCommunity_Click(sender As Object, e As System.EventArgs) Handles LNBselectOwnerFromCommunity.Click
        RaiseEvent RequireNewOwnerFromCommunity()
    End Sub
    Private Sub CLTRselectUsers_CloseWindow() Handles CLTRselectUsers.CloseWindow
        Me.DVselectFromCommunity.Visible = False
    End Sub
    Private Sub CLTRselectUsers_UserSelected(idUser As Integer) Handles CLTRselectUsers.UserSelected
        Me.DVselectFromCommunity.Visible = False
        RaiseEvent AddNewOwner(idUser)
    End Sub

    Private Sub LNBselectOwnerFromResources_Click(sender As Object, e As System.EventArgs) Handles LNBselectOwnerFromResources.Click
        RaiseEvent RequireNewOwnerFromResources()
    End Sub
    Private Sub CTRLselectFromResources_CloseWindow() Handles CTRLselectFromResources.CloseWindow
        Me.DVselectFromProjectResource.Visible = False
    End Sub
    Private Sub CTRLselectFromResources_SelectedResource(idResource As Long) Handles CTRLselectFromResources.SelectedResource
        Me.DVselectFromProjectResource.Visible = False
        RaiseEvent SelectedNewOwner(idResource)
    End Sub
#End Region


End Class