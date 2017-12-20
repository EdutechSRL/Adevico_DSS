Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ProjectMap
    Inherits PMbaseProjectMap
    Implements IViewProjectMap

#Region "Context"
    Private _Presenter As ProjectMapPresenter
    Private ReadOnly Property CurrentPresenter() As ProjectMapPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProjectMapPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Settings"
    Private Property AllowAddActivity As Boolean Implements IViewProjectMap.AllowAddActivity
        Get
            Return ViewStateOrDefault("AllowAddActivity", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAddActivity") = value
            Me.LNBaddActivityTop.Visible = value
            Me.LNBaddActivityBottom.Visible = value
        End Set
    End Property
    Private Property AllowAddCommunityUser As Boolean Implements IViewProjectMap.AllowAddCommunityUser
        Get
            Return ViewStateOrDefault("AllowAddCommunityUser", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAddCommunityUser") = value
            Me.LNBaddUsersFromCommunity.Visible = value
            Me.LNBaddUsersFromCommunityBottom.Visible = value
        End Set
    End Property
    Private Property AllowAddExternalUser As Boolean Implements IViewProjectMap.AllowAddExternalUser
        Get
            Return ViewStateOrDefault("AllowAddExternalUser", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAddExternalUser") = value
            Me.LNBaddCustomUsers.Visible = value
            Me.LNBaddCustomUser.Visible = value
            Me.LNBaddCustomUsersBottom.Visible = value
            Me.LNBaddCustomUserBottom.Visible = value
        End Set
    End Property
    Private Property AllowManageResources As Boolean Implements IViewProjectMap.AllowManageResources
        Get
            Return ViewStateOrDefault("AllowManageResources", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowManageResources") = value
            Me.LNBmanageProjectResources.Visible = value
            Me.LNBaddCustomUserBottom.Visible = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewProjectMap.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveProjectMapBottom.Visible = value
            Me.BTNsaveProjectMapTop.Visible = value
        End Set
    End Property
    Private Property AllowMilestones As Boolean Implements IViewProjectMap.AllowMilestones
        Get
            Return ViewStateOrDefault("AllowMilestones", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowMilestones") = value
        End Set
    End Property
    Private Property AllowEstimatedDuration As Boolean Implements IViewProjectMap.AllowEstimatedDuration
        Get
            Return ViewStateOrDefault("AllowMilestones", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEstimatedDuration") = value
        End Set
    End Property

    Private Property ProjectResources As List(Of dtoResource) Implements IViewProjectMap.ProjectResources
        Get
            Return ViewStateOrDefault("ProjectResources", New List(Of dtoResource))
        End Get
        Set(value As List(Of dtoResource))
            ViewState("ProjectResources") = value
        End Set
    End Property
    'Private Property PRdeadline As DateTime? Implements IViewProjectMap.PRdeadline
    '    Get
    '        Return ViewState("PRdeadline")
    '    End Get
    '    Set(value As DateTime?)
    '        ViewState("PRdeadline") = value
    '    End Set
    'End Property
    Private ReadOnly Property PRendDate As DateTime? ' Implements IViewProjectMap.PRendDate
        Get
            Return CTRLprojectInfo.ProjectEndDate  '        Return ViewState("PRendDate")
        End Get
        '    Set(value As DateTime?)
        '        ViewState("PRdeadline") = value
        '    End Set
    End Property
    'Private Property PRstartDate As DateTime? Implements IViewProjectMap.PRstartDate
    '    Get
    '        Return ViewState("PRstartDate")
    '    End Get
    '    Set(value As DateTime?)
    '        ViewState("PRstartDate") = value
    '    End Set
    'End Property
    Private ReadOnly Property InEditStartDate As DateTime? Implements IViewProjectMap.InEditStartDate
        Get
            Return CTRLprojectInfo.InEditStartDate
        End Get
    End Property
    Private ReadOnly Property InEditDeadline As DateTime? Implements IViewProjectMap.InEditDeadline
        Get
            Return CTRLprojectInfo.InEditDeadline
        End Get
    End Property
  
    Private Function GetDefaultActivityName() As String Implements IViewProjectMap.GetDefaultActivityName
        Return Resource.getValue("GetDefaultActivityName")
    End Function
    Private Property DefaultWorkingDay As dtoWorkingDay Implements IViewProjectMap.DefaultWorkingDay
        Get
            Return ViewStateOrDefault("DefaultWorkingDay", dtoWorkingDay.GetDefault())
        End Get
        Set(value As dtoWorkingDay)
            ViewState("DefaultWorkingDay") = value
        End Set
    End Property
#End Region

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPbackToProjectsBottom, True, True)
            .setHyperLink(HYPbackToProjectsTop, True, True)
            .setLinkButton(LNBaddCustomUsers, False, True)
            .setLinkButton(LNBaddCustomUser, False, True)
            .setLinkButton(LNBaddUsersFromCommunity, False, True)
            LNBaddCustomUsersBottom.Text = LNBaddCustomUsers.Text
            LNBaddCustomUsersBottom.ToolTip = LNBaddCustomUsers.ToolTip
            LNBaddCustomUserBottom.Text = LNBaddCustomUser.Text
            LNBaddCustomUserBottom.ToolTip = LNBaddCustomUser.Text
            LNBaddUsersFromCommunityBottom.Text = LNBaddUsersFromCommunity.Text
            LNBaddUsersFromCommunityBottom.ToolTip = LNBaddUsersFromCommunity.ToolTip
            .setLinkButton(LNBmanageProjectResources, False, True)
            LNBmanageProjectResourcesBottom.Text = LNBmanageProjectResources.Text
            LNBmanageProjectResourcesBottom.ToolTip = LNBmanageProjectResources.ToolTip
            .setLinkButton(LNBaddActivityTop, False, True)
            .setLinkButton(LNBaddActivityBottom, False, True)
            .setButton(BTNsaveProjectMapTop, True)
            BTNsaveProjectMapBottom.Text = BTNsaveProjectMapTop.Text
            BTNsaveProjectMapBottom.ToolTip = BTNsaveProjectMapTop.ToolTip

            Master.ServiceTitle = .getValue("ServiceTitle.ProjectMap")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ProjectMap.ToolTip")

            '.setLabel(LBprojectStartDate_t)
            '.setLabel(LBprojectEndDate_t)
            '.setLabel(LBprojectDeadline_t)
            '.setLabel(LBprojectType_t)

            .setLinkButton(LNBshowResourcesColumn, False, True)
            .setLinkButton(LNBhideResourcesColumn, False, True)
            .setLinkButton(LNBshowDateColumns, False, True)
            .setLinkButton(LNBhideDateColumns, False, True)
            .setLinkButton(LNBshowPredecessorsColumn, False, True)
            .setLinkButton(LNBhidePredecessorsColumn, False, True)


            .setHyperLink(HYPprojectMap, False, True)
            .setHyperLink(HYPprojectMapReorder, False, True)
            .setHyperLink(HYPprojectMapBulkEdit, False, True)
            .setLabel(LBthDisplayOrder)
            .setLabel(LBthName)
            .setLabel(LBthStatus)
            .setLabel(LBthDuration)
            .setLabel(LBthPredecessors)
            .setLabel(LBthPredecessorsLegend)
            .setLabel(LBthStartDate)
            .setLabel(LBthEndDate)
            .setLabel(LBthResources)

            .setLinkButton(LNBexpandNodes, False, True)
            .setLinkButton(LNBcompressNodes, False, True)
            .setLinkButton(LNBwideview, False, True)
            .setLinkButton(LNBnarrowview, False, True)
            .setLinkButton(LNBtoggleresources, False, True)
            .setLinkButton(LNBrestoreview, False, True)

            DVselectResources.Attributes("title") = Resource.getValue("DVselectResources.Title")
            .setLabel(LBactivity_t)
            .setLinkButton(LNBcloseSelectActivityResources, False, True)
            .setButton(BTNselectActivityResources, True)

            .setHyperLink(HYPprojectGantt, False, True)

            .setHyperLink(HYPgoToProjectEditTop, False, True)
            .setHyperLink(HYPbackToResourceDashboardTop, False, True)
            .setHyperLink(HYPbackToManagerDashboardTop, False, True)
            .setHyperLink(HYPgoToProjectEditBottom, False, True)
            .setHyperLink(HYPbackToResourceDashboardBottom, False, True)
            .setHyperLink(HYPbackToManagerDashboardBottom, False, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#Region "Display"
    Protected Overrides Sub DisplayUnknownProject()
        Me.MLVprojectMap.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownProject.ProjectMap")
    End Sub
    Protected Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(RootObject.ProjectMap(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity), PreloadIdCommunity)
    End Sub
#End Region

    Protected Overrides Sub LoadProjectDateInfo(project As dtoProject, allowEdit As Boolean)
        Me.DVresourcesTop.Visible = AllowAddCommunityUser OrElse AllowAddExternalUser OrElse AllowManageResources
        Me.DVresourcesTop.Attributes.Add("class", IIf((AllowAddExternalUser OrElse (AllowManageResources AndAlso AllowAddCommunityUser)), "ddbuttonlist enabled", "ddbuttonlist"))
        DVresourcesBottom.Visible = AllowAddCommunityUser OrElse AllowAddExternalUser OrElse AllowManageResources
        DVresourcesBottom.Attributes.Add("class", IIf((AllowAddExternalUser OrElse (AllowManageResources AndAlso AllowAddCommunityUser)), "ddbuttonlist enabled", "ddbuttonlist"))

        LoaderCultureInfo = Resource.CultureInfo
        LTprojectName.Text = project.Name

        CTRLprojectInfo.InitializeControl(project, Resource.CultureInfo, Resource.CultureInfo.DateTimeFormat.ShortDatePattern, allowEdit)

        AllowEstimatedDuration = project.AllowEstimatedDuration
        AllowMilestones = project.AllowMilestones
        DVpredecessorsCommand.Visible = project.DateCalculationByCpm
        CTRLpredecessorsHelper.Visible = project.DateCalculationByCpm
        LBthStatus.ToolTip = project.Completeness & "%"
        LBthStatus.Text = String.Format(LTstatusContent.Text, project.Completeness & "%")
        LBthStatus.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(project.Completeness)
        HYPprojectMap.NavigateUrl = ApplicationUrlBase & RootObject.ProjectMap(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)
        '  HYPprojectMapBulkEdit.Visible = AllowSave
        HYPprojectMapReorder.NavigateUrl = ApplicationUrlBase & RootObject.MapReorder(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapReorder.Visible = AllowSave
        HYPprojectGantt.NavigateUrl = ApplicationUrlBase & RootObject.Gantt(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectGantt.Visible = True

        HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)

        THpredecessors.Visible = project.DateCalculationByCpm
        TDfooterToolBar.ColSpan = IIf(project.DateCalculationByCpm, CInt(LTfullColSpan.Text), CInt(LTnocpmColSpan.Text))

        CTRLinLineAdd.Visible = AllowAddActivity
        CTRLinLineAdd.InitializeControl(AllowAddActivity, project.DateCalculationByCpm, project.AllowSummary)
    End Sub

#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayErrorGetActivitiesFromDB() Implements IViewProjectMap.DisplayErrorGetActivitiesFromDB
        CTRLinLineAdd.Visible = False
        AllowAddActivity = False
        AllowSave = False
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayErrorGetActivitiesFromDB"), Helpers.MessageType.alert)
        DisableInLineEditing()
    End Sub
#End Region

#Region "Project resources"

#Region "Display"
    Private Sub DisplayNoResources() Implements IViewProjectMap.DisplayNoResources
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoResources"), Helpers.MessageType.info)
    End Sub
    Private Sub DisplayUnableToAddExternalResource() Implements IViewProjectMap.DisplayUnableToAddExternalResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToAddExternalResource"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnableToSaveResources() Implements IViewProjectMap.DisplayUnableToSaveResources
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToSaveResources"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnableToAddInternalResource() Implements IViewProjectMap.DisplayUnableToAddInternalResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToAddInternalResource"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnableToAddInternalResources() Implements IViewProjectMap.DisplayUnableToAddInternalResources
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToAddInternalResources"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayErrors(multipleLongName As Integer, multipleShortName As Integer) Implements IViewProjectMap.DisplayErrors
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(GetMessage("DisplayErrors.", multipleLongName, multipleShortName), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayRemovedResource(name As String) Implements IViewProjectMap.DisplayRemovedResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayRemovedResource"), name), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayResourceAdded(added As Integer, Optional multipleLongName As Integer = 0, Optional multipleShortName As Integer = 0) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewProjectMap.DisplayResourceAdded
        CTRLmessages.Visible = True

        If added > 0 Then
            Dim message As String = "DisplayResourceAdded." & IIf(added = 1, "1", "n")
            Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)

            If added > 1 Then
                items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue(message), added), .Type = Helpers.MessageType.success})
            Else
                items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = Resource.getValue(message), .Type = Helpers.MessageType.success})
            End If
            If multipleLongName > 0 OrElse multipleShortName > 0 Then
                items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = GetMessage("DisplayErrors.", multipleLongName, multipleShortName), .Type = Helpers.MessageType.alert})
            End If
            CTRLmessages.InitializeControl(items)
        Else
            DisplayErrors(multipleLongName, multipleShortName)
        End If

    End Sub
    Private Sub DisplayUnableToRemoveResource(name As String) Implements IViewProjectMap.DisplayUnableToRemoveResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayUnableToRemoveResource"), name), Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySavedSettings(Optional multipleLongName As Integer = 0, Optional multipleShortName As Integer = 0) Implements IViewProjectMap.DisplaySavedSettings
        Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        CTRLmessages.Visible = True
        items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = Resource.getValue("DisplaySavedSettings"), .Type = Helpers.MessageType.success})

        If multipleLongName > 0 OrElse multipleShortName > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = GetMessage("DisplayErrors.", multipleLongName, multipleShortName), .Type = Helpers.MessageType.alert})
        End If
        CTRLmessages.InitializeControl(items)
    End Sub
    Private Sub DisplayUnableToRemoveResource(idProject As Long, idResource As Long, name As String, assignedTasks As Long, completedTasks As Long) Implements IViewProjectMap.DisplayUnableToRemoveResource
        Me.CTRLmessages.Visible = False
        MLVmanageResources.SetActiveView(VIWvirtualDeleteResource)

        CTRLconfirmRemoveResource.InitializeControl(idProject, idResource, name, assignedTasks, completedTasks, Resource.getValue("RemoveResource.Description"))
    End Sub
    Private Function GetMessage(ByVal startMessage As String, multipleLongName As Integer, multipleShortName As Integer) As String
        Dim message As String = startMessage
        Select Case multipleLongName
            Case 0, 1
                message &= multipleLongName.ToString & "."
            Case Else
                If multipleLongName > 1 Then
                    message &= "n."
                Else
                    message &= "0."
                End If
        End Select
        Select Case multipleShortName
            Case 0, 1
                message &= multipleShortName.ToString
            Case Else
                If multipleShortName > 1 Then
                    message &= "n"
                Else
                    message &= "0"
                End If
        End Select
        If multipleLongName > 1 AndAlso multipleShortName > 1 Then
            Return String.Format(Resource.getValue(message), multipleLongName, multipleShortName)
        ElseIf multipleLongName > 1 Then
            Return String.Format(Resource.getValue(message), multipleLongName)
        ElseIf multipleShortName > 1 Then
            Return String.Format(Resource.getValue(message), multipleShortName)
        Else
            Return Resource.getValue(message)
        End If
    End Function
#End Region


    Private Sub InitializeControlForResourcesSelection(resources As List(Of dtoProjectResource)) Implements IViewProjectMap.InitializeControlForResourcesSelection
        DVselectResources.Visible = resources.Any()
        CTRLselectActivityResources.InitializeControl(resources)
    End Sub
    Private Sub LoadResources(resources As List(Of dtoProjectResource)) Implements IViewProjectMap.LoadResources
        Me.CTRLmessages.Visible = False
        Me.DVprojectResources.Visible = True
        MLVmanageResources.SetActiveView(VIWmanageResources)
        Me.CTRLmanageResources.InitializeControl(resources, Resource.getValue("ManageResourceFromProjectMap"))
    End Sub
    Private Function GetResources() As List(Of dtoProjectResource) Implements IViewProjectMap.GetResources
        Return CTRLmanageResources.GetResources()
    End Function
    Private Sub InitializeControlToAddUsers(idCommunity As Integer, hideUsers As List(Of Integer)) Implements IViewProjectMap.InitializeControlToAddUsers
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunity, hideUsers, Nothing, Resource.getValue("SelectResourcesFromCommunity.Description"))
    End Sub
    Private Sub InitializeControlToAddExternalResource() Implements IViewProjectMap.InitializeControlToAddExternalResource
        Me.CTRLmessages.Visible = False
        Me.DVaddExternalResources.Visible = True
        CTRLaddExternalResources.InitializeControl(Resource.getValue("AddExternalResource.Description"))
    End Sub

    Private Sub ReloadResources(items As Dictionary(Of Long, List(Of dtoResource)), Optional ByVal cItems As Dictionary(Of Long, dtoCompletion) = Nothing) Implements IViewProjectMap.ReloadResources
        If IsNothing(cItems) Then
            cItems = New Dictionary(Of Long, dtoCompletion)
        End If
        If Not IsNothing(items) Then
            For Each row As RepeaterItem In RPTactivities.Items
                Dim idActivity As Long = CLng(DirectCast(row.FindControl("LTidActivity"), Literal).Text)
                Dim isSummary As Boolean = (Boolean.TrueString = DirectCast(row.FindControl("LTisSummary"), Literal).Text)

                If items.ContainsKey(idActivity) Then
                    Dim oControlResources As UC_InLineResources = row.FindControl("CTRLinLineResources")
                    oControlResources.InitializeControl(items(idActivity), Not isSummary)
                End If
                If cItems.ContainsKey(idActivity) Then
                    Dim oLabel As Label = row.FindControl("LBactivityStatus")
                    oLabel.ToolTip = cItems(idActivity).Completeness & "%"
                    oLabel.Text = String.Format(LTstatusContent.Text, cItems(idActivity).Completeness & "%")
                    oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(cItems(idActivity).Completeness, cItems(idActivity).IsCompleted)
                End If
            Next
        End If

    End Sub
#End Region

#Region "Project Activities"
    Private Sub LoadActivities(activities As List(Of dtoMapActivity)) Implements IViewProjectMap.LoadActivities
        RPTactivities.DataSource = activities
        RPTactivities.DataBind()
        CTRLlegend.Visible = True


        DVtableCommands.Visible = activities.Any()
        DVcommandsBottom.Visible = activities.Any() AndAlso activities.Count > 15
        LNBexpandNodes.Visible = activities.Where(Function(a) a.IsSummary).Any()
        LNBcompressNodes.Visible = activities.Where(Function(a) a.IsSummary).Any()
    End Sub
    Private Sub LoadActivities(activities As List(Of dtoMapActivity), startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?)) Implements IViewProjectMap.LoadActivities
        RPTactivities.DataSource = activities
        RPTactivities.DataBind()
        CTRLlegend.Visible = True

        DVtableCommands.Visible = activities.Any()
        DVcommandsBottom.Visible = activities.Any() AndAlso activities.Count > 15
        LNBexpandNodes.Visible = activities.Where(Function(a) a.IsSummary).Any()
        LNBcompressNodes.Visible = activities.Where(Function(a) a.IsSummary).Any()

        CTRLprojectInfo.UpdateDateAndStatus(startDate, endDate, deadLine)

    End Sub
    Private Function GetActivities() As List(Of dtoLiteMapActivity) Implements IViewProjectMap.GetActivities
        Dim activities As New List(Of dtoLiteMapActivity)
        Dim rowNumber As Long = 1
        For Each row As RepeaterItem In RPTactivities.Items
            Dim activity As New dtoLiteMapActivity
            activity.IdActivity = CLng(DirectCast(row.FindControl("LTidActivity"), Literal).Text)
            activity.IdParent = CLng(DirectCast(row.FindControl("LTidParent"), Literal).Text)
            activity.RowNumber = rowNumber
            Boolean.TryParse(DirectCast(row.FindControl("LTisSummary"), Literal).Text, activity.IsSummary)

            Dim oControlInput As UC_InLineInput = row.FindControl("CTRLnameInput")
            If Not (String.IsNullOrEmpty(oControlInput.NewValue)) Then
                activity.Current.Name = oControlInput.NewValue.Trim
            End If
            activity.Previous.Name = oControlInput.OldValue

            oControlInput = row.FindControl("CTRLdurationInput")
            If oControlInput.OldValue.Contains("?") Then
                Double.TryParse(Replace(oControlInput.OldValue, "?", ""), activity.Previous.Duration.Value)
                activity.Previous.Duration.IsEstimated = True
            Else
                Double.TryParse(oControlInput.OldValue, activity.Previous.Duration.Value)
            End If

            If oControlInput.NewValue.Contains("?") Then
                Double.TryParse(Replace(oControlInput.NewValue, "?", ""), activity.Current.Duration.Value)
                activity.Current.Duration.IsEstimated = True
            Else
                If Not Double.TryParse(oControlInput.NewValue, activity.Current.Duration.Value) Then
                    activity.Current.Duration = activity.Previous.Duration
                End If
            End If


            If THpredecessors.Visible AndAlso Not activity.IsSummary Then
                oControlInput = row.FindControl("CTRLpredecessorsInput")
                activity.Current.Predecessors = oControlInput.NewValue
                activity.Previous.Predecessors = oControlInput.OldValue
            End If
            oControlInput = row.FindControl("CTRLstartDateInput")
            Try
                activity.Previous.EarlyStartDate = GetDateFromString(oControlInput.OldValue, Nothing)
                If Not String.IsNullOrEmpty(oControlInput.NewValue) AndAlso oControlInput.IsValid Then
                    activity.Current.EarlyStartDate = GetDateFromString(oControlInput.NewValue, activity.Current.EarlyStartDate)
                Else
                    activity.Current.EarlyStartDate = activity.Previous.EarlyStartDate
                End If


                If activity.Previous.EarlyStartDate.HasValue AndAlso activity.Previous.EarlyStartDate.Value.Hour = 0 Then
                    activity.Previous.EarlyStartDate = activity.Previous.EarlyStartDate.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
                End If
                If activity.Current.EarlyStartDate.HasValue AndAlso activity.Current.EarlyStartDate.Value.Hour = 0 Then
                    activity.Current.EarlyStartDate = activity.Current.EarlyStartDate.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
                End If
            Catch ex As Exception

            End Try

            rowNumber += 1
            activities.Add(activity)
        Next
        Return activities
    End Function

#Region "Display"
    Private Sub DisplaySavedActivities(Optional ByVal verifyLinks As Boolean = False, Optional ByVal fatherLinks As Boolean = False, Optional ByVal summaryLinks As Boolean = False) Implements IViewProjectMap.DisplaySavedActivities
        CTRLmessages.Visible = True

        If verifyLinks OrElse fatherLinks OrElse summaryLinks Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplaySavedActivities") & "<br/>" & Resource.getValue("DisplaySavedActivities." & verifyLinks.ToString & "." & fatherLinks.ToString & "." & summaryLinks.ToString), Helpers.MessageType.alert)
        Else
            CTRLmessages.InitializeControl(Resource.getValue("DisplaySavedActivities"), Helpers.MessageType.success)
        End If
    End Sub
    Private Sub DisplayActivityMoved(Optional ByVal verifyLinks As Boolean = False, Optional ByVal fatherLinks As Boolean = False, Optional ByVal summaryLinks As Boolean = False) Implements IViewProjectMap.DisplayActivityMoved
        CTRLmessages.Visible = True

        If verifyLinks OrElse fatherLinks OrElse summaryLinks Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayActivityMoved") & "<br/>" & Resource.getValue("DisplayActivityMoved." & verifyLinks.ToString & "." & fatherLinks.ToString & "." & summaryLinks.ToString), Helpers.MessageType.alert)
        Else
            CTRLmessages.InitializeControl(Resource.getValue("DisplayActivityMoved"), Helpers.MessageType.success)
        End If
    End Sub
    Private Sub DisplayActivitiesAdded() Implements IViewProjectMap.DisplayActivitiesAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayActivitiesAdded"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayUnableToAddActivities() Implements IViewProjectMap.DisplayUnableToAddActivities
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToAddActivities"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayActivityRemoved(name As String, children As Long) Implements IViewProjectMap.DisplayActivityRemoved
        DisplayActivityMessage("DisplayActivityRemoved", Helpers.MessageType.success, name, children)
    End Sub
    Private Sub DisplayUnableToRemoveActivity(name As String, children As Long) Implements IViewProjectMap.DisplayUnableToRemoveActivity
        DisplayActivityMessage("DisplayUnableToRemoveActivity", Helpers.MessageType.error, name, children)
    End Sub
    Private Sub DisplaySavedActivityResources(name As String, resources As Long) Implements IViewProjectMap.DisplaySavedActivityResources
        DisplayActivityResourcesMessage("DisplaySavedActivityResources", Helpers.MessageType.success, name, resources)
    End Sub
    Private Sub DisplayUnableToSaveActivityResources(name As String) Implements IViewProjectMap.DisplayUnableToSaveActivityResources
        DisplayActivityResourcesMessage("DisplayUnableToSaveActivityResources", Helpers.MessageType.error, name, -1)
    End Sub
    Private Sub DisplayActivityResourcesMessage(messageKey As String, type As Helpers.MessageType, activityName As String, resources As Long)
        CTRLmessages.Visible = True
        Dim message As String = messageKey & "."
        If resources < 2 Then
            message &= resources.ToString
        Else
            message &= "n"
        End If
        If Not String.IsNullOrEmpty(activityName) Then
            message &= ".Name"
            activityName = activityName.Replace("""", "'")
        End If
        CTRLmessages.Visible = True
        If String.IsNullOrEmpty(activityName) Then
            If resources > 1 Then
                CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), resources), type)
            Else
                CTRLmessages.InitializeControl(Resource.getValue(message), type)
            End If
        ElseIf resources <= 1 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), activityName), type)
        Else
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), resources, activityName), type)
        End If
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
    Private Sub DisplaySavingActivitiesErrors(toUpdate As Long, updated As Long, alreadyModified As Long) Implements IViewProjectMap.DisplaySavingActivitiesErrors
        Dim message As String = ""
        CTRLmessages.Visible = True
        If toUpdate = 0 AndAlso updated = 0 Then
            message = Resource.getValue("DisplaySavingActivitiesErrors")
        ElseIf toUpdate = 1 Then
            message = Resource.getValue("DisplaySavingActivitiesErrors.1.0")
        ElseIf toUpdate > 1 AndAlso updated = 0 Then
            message = Resource.getValue("DisplaySavingActivitiesErrors.n.0")
        ElseIf toUpdate > 1 Then
            message = String.Format(Resource.getValue("DisplaySavingActivitiesErrors.n.n"), updated, toUpdate)
        End If
        If alreadyModified = 1 Then
            message &= "<p>" & Resource.getValue("DisplaySavingActivitiesErrors.alreadyModified.1") & "</p>"
        ElseIf alreadyModified > 1 Then
            message &= "<p>" & String.Format(Resource.getValue("DisplaySavingActivitiesErrors.alreadyModified.n"), alreadyModified) & "</p>"
        End If
        CTRLmessages.InitializeControl(message, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnableToMoveActivity() Implements IViewProjectMap.DisplayUnableToMoveActivity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToMoveActivity"), Helpers.MessageType.error)
    End Sub
#End Region

    Private Sub ReloadCompletion(items As Dictionary(Of Long, dtoCompletion)) Implements IViewProjectMap.ReloadCompletion
        If Not IsNothing(items) Then
            For Each row As RepeaterItem In RPTactivities.Items
                Dim idActivity As Long = CLng(DirectCast(row.FindControl("LTidActivity"), Literal).Text)
                If items.ContainsKey(idActivity) Then
                    Dim oLabel As Label = row.FindControl("LBactivityStatus")
                    oLabel.ToolTip = items(idActivity).Completeness & "%"
                    oLabel.Text = String.Format(LTstatusContent.Text, items(idActivity).Completeness & "%")
                    oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(items(idActivity).Completeness, items(idActivity).IsCompleted)
                End If
            Next
        End If
    End Sub
#End Region

#Region "Edit Activity"
    Private Sub DisplayRemovedActivity() Implements IViewProjectMap.DisplayRemovedActivity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayRemovedActivity"), Helpers.MessageType.alert)
    End Sub

    Private Sub InitializeActivityControl(idActivity As Long) Implements IViewProjectMap.InitializeActivityControl
        Me.CTRLmessages.Visible = False
        Me.DVactivity.Visible = True
        CTRLactivityEdit.LoaderCultureInfo = LoaderCultureInfo
        CTRLactivityEdit.InitializeControl(ProjectIdCommunity, idActivity, GetActivities())
    End Sub

    Private Sub DisplayActivitySaved(activity As dtoMapActivity, startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?)) Implements IViewProjectMap.DisplayActivitySaved
        CTRLmessages.Visible = True
        Me.DVactivity.Visible = False
        If IsNothing(activity) Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayActivitySaved.None"), Helpers.MessageType.success)
        Else
            CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayActivitySaved.Name"), activity.Name.Init), Helpers.MessageType.success)
        End If

        CTRLprojectInfo.UpdateDate(startDate, endDate, deadLine)
    End Sub
#End Region

    Protected Overrides Sub SetProjectsUrl(url As String)
        If Not String.IsNullOrEmpty(url) Then
            HYPbackToProjectsTop.Visible = True
            HYPbackToProjectsBottom.Visible = True
            HYPbackToProjectsTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPbackToProjectsBottom.NavigateUrl = HYPbackToProjectsTop.NavigateUrl
        End If
    End Sub
    Protected Overrides Sub SetEditProjectUrl(url As String)
        If Not String.IsNullOrEmpty(url) Then
            HYPgoToProjectEditTop.Visible = True
            HYPgoToProjectEditBottom.Visible = True
            HYPgoToProjectEditTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPgoToProjectEditBottom.NavigateUrl = HYPgoToProjectEditTop.NavigateUrl
        End If
    End Sub
    Protected Overrides Sub SetDashboardUrl(url As String, fromPage As PageListType)
        If Not String.IsNullOrEmpty(url) Then
            Select Case fromPage
                Case PageListType.DashboardAdministrator, PageListType.DashboardManager, PageListType.ProjectDashboardManager
                    HYPbackToManagerDashboardTop.Visible = True
                    HYPbackToManagerDashboardBottom.Visible = True
                    HYPbackToManagerDashboardTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
                    HYPbackToManagerDashboardBottom.NavigateUrl = HYPbackToManagerDashboardTop.NavigateUrl
                Case Else
                    HYPbackToResourceDashboardTop.Visible = True
                    HYPbackToResourceDashboardBottom.Visible = True
                    HYPbackToResourceDashboardTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
                    HYPbackToResourceDashboardBottom.NavigateUrl = HYPbackToResourceDashboardTop.NavigateUrl
            End Select

        End If
    End Sub
#End Region

#Region "Internal"

    'Private Function GetCssStatuslight(completeness As Integer)
    '    If completeness = 100 Then
    '        Return "green"
    '    ElseIf completeness > 0 Then
    '        Return "yellow"
    '    Else
    '        Return "gray"
    '    End If
    'End Function
    'Private Function GetCssStatuslight(completeness As Integer, isCompleted As Boolean)
    '    If completeness = 100 AndAlso isCompleted Then
    '        Return "green"
    '    ElseIf completeness > 0 Then
    '        Return "yellow"
    '    Else
    '        Return "gray"
    '    End If
    'End Function
#Region "Project Resources"
    Private Sub LNBaddCustomUser_Click(sender As Object, e As System.EventArgs) Handles LNBaddCustomUser.Click, LNBaddCustomUserBottom.Click
        Me.CurrentPresenter.AddExternalResource(IdProject, UnknownUser)
    End Sub
    Private Sub LNBaddCustomUsers_Click(sender As Object, e As System.EventArgs) Handles LNBaddCustomUsers.Click, LNBaddActivityBottom.Click
        Me.CurrentPresenter.SelectExternalResources(IdProject, UnknownUser)
    End Sub
    Private Sub LNBaddUsersFromCommunity_Click(sender As Object, e As System.EventArgs) Handles LNBaddUsersFromCommunity.Click, LNBaddUsersFromCommunityBottom.Click
        Me.CurrentPresenter.SelectResourcesFromCommunity(IdProject, UnknownUser)
    End Sub
    Private Sub LNBmanageProjectResources_Click(sender As Object, e As System.EventArgs) Handles LNBmanageProjectResources.Click, LNBmanageProjectResourcesBottom.Click
        CurrentPresenter.LoadProjectResources(IdProject, UnknownUser)
    End Sub

#Region "Add Internal Resources"
    Private Sub CLTRselectUsers_CloseWindow() Handles CLTRselectUsers.CloseWindow
        Me.DVselectUsers.Visible = False
    End Sub
    Private Sub CLTRselectUsers_UsersSelected(idUsers As List(Of Integer)) Handles CLTRselectUsers.UsersSelected
        Me.DVselectUsers.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.AddInternalResources(IdProject, idUsers, UnknownUser)
    End Sub
#End Region

#Region "Add External Resources"
    Private Sub CTRLaddExternalResources_CloseWindow() Handles CTRLaddExternalResources.CloseWindow
        Me.DVaddExternalResources.Visible = False
    End Sub
    Private Sub CTRLaddExternalResources_AddingResources(resources As List(Of dtoExternalResource)) Handles CTRLaddExternalResources.AddingResources
        Me.DVaddExternalResources.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.AddExternalResources(IdProject, resources, UnknownUser)
    End Sub
#End Region

#Region "Confirm Delete"
    Private Sub CTRLconfirmRemoveResource_CloseWindow() Handles CTRLconfirmRemoveResource.CloseWindow
        MLVmanageResources.SetActiveView(VIWmanageResources)
    End Sub
    Private Sub CTRLconfirmRemoveResource_DeletingResource(idResource As Long, resourceName As String, rAction As RemoveAction) Handles CTRLconfirmRemoveResource.DeletingResource
        MLVmanageResources.SetActiveView(VIWmanageResources)
        DVprojectResources.Visible = False
        Me.CurrentPresenter.ConstrainVirtualDeleteResource(idResource, resourceName, rAction)
    End Sub
#End Region

    Private Sub CTRLmanageResources_CloseWindow() Handles CTRLmanageResources.CloseWindow
        DVprojectResources.Visible = False
    End Sub
    Private Sub CTRLmanageResources_SaveSettings(resources As List(Of dtoProjectResource)) Handles CTRLmanageResources.SaveSettings
        DVprojectResources.Visible = False
        CurrentPresenter.SaveProjectResources(IdProject, resources, UnknownUser)
    End Sub
    Private Sub CTRLmanageResources_VirtualDeleteResource(idResource As Long, name As String) Handles CTRLmanageResources.VirtualDeleteResource
        CurrentPresenter.VirtualDeleteResource(idResource, name)
    End Sub
#End Region

#Region "Activities"

    '#Region "CssClass"
    '    Protected Function GetRowCssClass(activity As dtoMapActivity) As String
    '        Dim cssClass As String = ""
    '        If activity.IdParent > 0 Then
    '            cssClass = " child-of-" & activity.IdParent.ToString
    '        End If
    '        cssClass &= IIf(activity.Status.HasFlag(FieldStatus.error), " " & FieldStatus.error.ToString(), "")
    '        Return cssClass
    '    End Function
    '    Protected Function GetCellCssClass(status As FieldStatus) As String
    '        Return IIf(status.HasFlag(FieldStatus.error) OrElse status.HasFlag(FieldStatus.errorfatherlinked) OrElse status.HasFlag(FieldStatus.errorsummarylinked), " " & FieldStatus.error.ToString(), " " & status.ToString)
    '    End Function
    '    Protected Function GetDeadlineCssClass(isAfterDeadline As Boolean) As String
    '        Return IIf(isAfterDeadline, " " & FieldStatus.error.ToString(), "")
    '    End Function
    '    Protected Function GetResourcesCssClass(item As dtoMapActivity) As String
    '        Dim cssClass As String = IIf(item.Resources.Status.HasFlag(FieldStatus.error), " " & FieldStatus.error.ToString(), " " & item.Resources.Status.ToString)

    '        If Not item.IsSummary AndAlso (IsNothing(item.Resources.GetValue) OrElse (Not IsNothing(item.Resources.GetValue) AndAlso Not item.Resources.GetValue().Any())) Then
    '            cssClass &= " noresource"
    '        End If

    '        Return cssClass
    '    End Function
    '#End Region

#Region "List"
    Private Sub RPTactivities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactivities.ItemDataBound
        Dim dto As dtoMapActivity = DirectCast(e.Item.DataItem, dtoMapActivity)

        Dim oControl As UC_ActivityInLineMenu = e.Item.FindControl("CTRLinlineMenu")
        oControl.InitializeControl(dto.IdActivity, dto.IsSummary, dto.Permission)
        oControl.Visible = AllowSave
        Dim oLabel As Label = e.Item.FindControl("LBerror")
        oLabel.Visible = dto.Status.HasFlag(FieldStatus.error)
        If dto.Status.HasFlag(FieldStatus.error) Then

        End If
        Dim oControlInput As UC_InLineInput = e.Item.FindControl("CTRLnameInput")
        If dto.Name.InEditMode AndAlso AllowSave Then
            oControlInput.AutoInitialize(dto.Name.Init, dto.Name.Edit)
        Else
            oControlInput.AutoInitialize(dto.Name.GetValue)
        End If

        oLabel = e.Item.FindControl("LBactivityStatus")
        oLabel.ToolTip = dto.Completeness & "%"
        oLabel.Text = String.Format(LTstatusContent.Text, dto.Completeness & "%")
        oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(dto.Completeness, dto.IsCompleted)
        oControlInput = e.Item.FindControl("CTRLdurationInput")
        If AllowEstimatedDuration Then
            oControlInput.ValidationExpression = LTdurationEstimatedRegex.Text
        Else
            oControlInput.ValidationExpression = LTdurationRegex.Text
        End If
        If Not AllowMilestones Then
            oControlInput.ValidationExpression = Replace(oControlInput.ValidationExpression, "\d", "[1-9]")
        End If
        If Not dto.Permission.SetDuration Then
            oControlInput.ContainerCssClass &= " disabled"
        End If
        'oControlInput.MinimumValue = IIf(AllowMilestones, 0, 1)
        If dto.Duration.InEditMode AndAlso AllowSave AndAlso Not dto.IsSummary Then
            oControlInput.AutoInitialize(dto.Duration.Init.Value & IIf(dto.Duration.Init.IsEstimated, "?", ""), dto.Duration.Edit.Value & IIf(dto.Duration.Edit.IsEstimated, "?", ""))
        Else
            oControlInput.AutoInitialize(dto.Duration.GetValue.Value & IIf(dto.Duration.GetValue.IsEstimated, "?", ""))
        End If


        Dim oTDpredecessors As HtmlTableCell = e.Item.FindControl("TDpredecessors")
        oTDpredecessors.Visible = THpredecessors.Visible
        If THpredecessors.Visible Then
            oTDpredecessors.Attributes("class") = LTpredecessorsCssClass.Text & IIf(dto.Predecessors.Status = FieldStatus.error, " linkserror", GetCellCssClass(dto.Predecessors.Status))
            oControlInput = e.Item.FindControl("CTRLpredecessorsInput")
            If Not dto.Permission.SetPredecessors Then
                oControlInput.ContainerCssClass &= " disabled"
            End If
            If dto.Predecessors.InEditMode AndAlso AllowSave AndAlso Not dto.IsSummary Then
                oControlInput.AutoInitialize(dto.Predecessors.Init, dto.Predecessors.Edit)
            Else
                oControlInput.AutoInitialize(dto.Predecessors.GetValue)
            End If
        End If

        oControlInput = e.Item.FindControl("CTRLstartDateInput")
        oControlInput.ReadOnlyInput = THpredecessors.Visible
        If THpredecessors.Visible OrElse dto.IsSummary Then
            oControlInput.ContainerCssClass &= " disabled"
        End If
        If dto.EarlyStartDate.InEditMode AndAlso AllowSave Then
            Dim initStartDate As String = "", editStartDate As String = ""
            If (dto.EarlyStartDate.Init.HasValue) Then
                initStartDate = dto.EarlyStartDate.Init.Value.ToString(CurrentShortDatePattern)
            End If
            If (dto.EarlyStartDate.Edit.HasValue) Then
                editStartDate = dto.EarlyStartDate.Edit.Value.ToString(CurrentShortDatePattern)
            End If
            oControlInput.AutoInitialize(initStartDate, editStartDate)
        Else
            If (dto.EarlyStartDate.GetValue.HasValue) Then
                oControlInput.AutoInitialize(dto.EarlyStartDate.GetValue.Value.ToString(CurrentShortDatePattern))
            Else
                oControlInput.AutoInitialize("")
            End If
        End If



        oLabel = e.Item.FindControl("LBactivityEndDate")
        If dto.EarlyFinishDate.GetValue.HasValue Then
            oLabel.Text = dto.EarlyFinishDate.GetValue.Value.ToString(CurrentShortDatePattern)
        Else
            oLabel.Text = "&nbsp;"
            oLabel.CssClass &= " empty"
        End If
        oLabel = e.Item.FindControl("LBactivityDeadLine")
        If dto.Deadline.GetValue.HasValue Then
            oLabel.ToolTip = dto.Deadline.GetValue.Value.ToString(CurrentShortDatePattern)
            oLabel.Visible = True
        Else
            oLabel.Visible = False
        End If
        Dim oControlResources As UC_InLineResources = e.Item.FindControl("CTRLinLineResources")
        oControlResources.InitializeControl(dto.Resources.GetValue, Not dto.IsSummary)
    End Sub
    Private Sub RPTactivities_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTactivities.ItemCommand
        Dim idActivity As Long = CLng(DirectCast(e.Item.FindControl("LTidActivity"), Literal).Text)


        Select Case e.CommandName
            Case "edit"
                CurrentPresenter.EditActivity(IdProject, idActivity, GetActivities())
            Case "indentleft"
                CurrentPresenter.ToFaher(IdProject, idActivity, GetActivities())
            Case "indentright"
                Dim currentDepth As Long = CLng(DirectCast(e.Item.FindControl("LTdepth"), Literal).Text)
                Dim idParent As Long = 0
                If e.Item.ItemIndex > 0 Then
                    Dim rowIndex As Integer = e.Item.ItemIndex - 1
                    While rowIndex >= 0
                        If currentDepth = DirectCast(RPTactivities.Items(rowIndex).FindControl("LTdepth"), Literal).Text Then
                            idParent = CLng(DirectCast(RPTactivities.Items(rowIndex).FindControl("LTidActivity"), Literal).Text)
                            rowIndex = -1
                        Else
                            rowIndex -= 1
                        End If
                    End While
                End If
                CurrentPresenter.MoveActivityTo(IdProject, idActivity, idParent, GetActivities())
            Case "addchildren"
                Dim number As Integer = CInt(Replace(e.CommandSource.ID, "LNBaddChildren", ""))
                CurrentPresenter.AddChildren(IdProject, idActivity, number, False, GetActivities())
            Case "addlinkedchildren"
                Dim number As Integer = CInt(Replace(e.CommandSource.ID, "LNBaddLinkedChildren", ""))
                CurrentPresenter.AddChildren(IdProject, idActivity, number, True, GetActivities())
            Case "addafter"
                Dim number As Integer = CInt(e.CommandSource.Text)
                CurrentPresenter.AddActivitiesAfter(IdProject, idActivity, number, False, GetActivities())
            Case "addlinkedafter"
                Dim number As Integer = CInt(e.CommandSource.Text)
                CurrentPresenter.AddActivitiesAfter(IdProject, idActivity, number, True, GetActivities())
            Case "addbefore"
                Dim number As Integer = CInt(e.CommandSource.Text)
                CurrentPresenter.AddActivitiesBefore(IdProject, idActivity, number, GetActivities())
            Case "virtualdelete"
                CurrentPresenter.VirtualDeleteActivity(IdProject, idActivity, GetActivities())
        End Select
    End Sub
#End Region

#Region "Actions"
    Private Sub CTRLinLineAdd_AddActivities(number As Integer, isLinked As Boolean) Handles CTRLinLineAdd.AddActivities
        CurrentPresenter.AddActivitiesToProject(IdProject, number, isLinked, GetActivities())
    End Sub
    Private Sub CTRLinLineAdd_AddActivity() Handles CTRLinLineAdd.AddActivity
        CurrentPresenter.AddActivitiesToProject(IdProject, 1, False, GetActivities())
    End Sub
    Private Sub CTRLinLineAdd_AddSummaryActivity(number As Integer, isLinked As Boolean, children As Integer) Handles CTRLinLineAdd.AddSummaryActivity
        CurrentPresenter.AddSummaryToProject(IdProject, number, isLinked, children, GetActivities())
    End Sub

    Private Sub DisableInLineEditing()
        Dim rowNumber As Long = 1
        For Each row As RepeaterItem In RPTactivities.Items
            Dim oControl As UC_ActivityInLineMenu = row.FindControl("CTRLinlineMenu")
            oControl.Visible = False

            Dim oControlInput As UC_InLineInput = row.FindControl("CTRLnameInput")
            oControlInput.ContainerCssClass &= " disabled"
            oControlInput = row.FindControl("CTRLdurationInput")
            oControlInput.ContainerCssClass &= " disabled"
            oControlInput = row.FindControl("CTRLpredecessorsInput")
            oControlInput.ContainerCssClass &= " disabled"
            oControlInput = row.FindControl("CTRLstartDateInput")
            oControlInput.ContainerCssClass &= " disabled"
        Next
    End Sub
#End Region

#Region "Select Resources"
    Private Sub BTNselectActivityResources_Click(sender As Object, e As System.EventArgs) Handles BTNselectActivityResources.Click
        Dim idActivity As String = HDNactivityRowNumber.Value
        If Not String.IsNullOrEmpty(idActivity) AndAlso IsNumeric(idActivity) Then
            Me.CurrentPresenter.SetResources(CLng(idActivity), CTRLselectActivityResources.GetSelectedIdResources(), GetActivities())
        End If
    End Sub
#End Region

#Region "Edit"
    Private Sub CTRLactivityEdit_CloseWindow() Handles CTRLactivityEdit.CloseWindow
        Me.DVactivity.Visible = False
    End Sub

    Private Sub CTRLactivityEdit_GetProjectDate(ByRef startDate As Date?, ByRef endDate As Date?, ByRef deadline As Date?) Handles CTRLactivityEdit.GetProjectDate
        startDate = InEditStartDate
        endDate = PRendDate
        deadline = InEditDeadline
    End Sub

    Private Sub CTRLactivityEdit_SavedActivity(idActivity As Long, startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?)) Handles CTRLactivityEdit.SavedActivity
        Me.DVactivity.Visible = False
        CurrentPresenter.SavedActivity(IdProject, idActivity, GetActivities(), startDate, endDate, deadLine)
    End Sub
    Private Sub CTRLactivityEdit_VirtualDeleteActivity(ByVal idActivity As Long) Handles CTRLactivityEdit.VirtualDeleteActivity
        Me.DVactivity.Visible = False
        CurrentPresenter.ActivityVirtualDeleted(IdProject, idActivity, GetActivities())
    End Sub
#End Region

#End Region
    Private Sub LNBaddActivityTop_Click(sender As Object, e As System.EventArgs) Handles LNBaddActivityTop.Click, LNBaddActivityBottom.Click
        CurrentPresenter.AddActivitiesToProject(IdProject, 1, False, GetActivities())
    End Sub

    Private Sub BTNsaveProjectMapTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveProjectMapTop.Click, BTNsaveProjectMapBottom.Click
        If Page.IsValid Then
            CurrentPresenter.SaveActivities(IdProject, GetActivities())
        End If
    End Sub

    'Private Function GetDateFromString(ByVal inputDate As String, ByVal defaultDateTime As DateTime?) As DateTime
    '    Try
    '        Dim dItems As String() = inputDate.Split("/")
    '        If dItems.Count = 3 Then
    '            If dItems(0).Count = 1 Then
    '                dItems(0) = "0" + dItems(0)
    '            End If
    '            If dItems(1).Count = 1 Then
    '                dItems(1) = "0" + dItems(1)
    '            End If
    '        End If
    '        Return DateTime.ParseExact(String.Join("/", dItems), CurrentShortDatePattern, LoaderCultureInfo.CurrentCulture.InvariantCulture)
    '    Catch ex As Exception
    '        Return defaultDateTime
    '    End Try
    '    Return defaultDateTime
    'End Function
#End Region

    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

End Class