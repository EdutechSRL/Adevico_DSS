Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Public Class UC_TaskListTree
    Inherits PMtasksListBase
    Implements IViewTasksListTree

#Region "Context"
    Private _Presenter As TasksListTreePresenter
    Private ReadOnly Property CurrentPresenter() As TasksListTreePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TasksListTreePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property CurrentGroupBy As ItemsGroupBy Implements IViewTasksListTree.CurrentGroupBy
        Get
            Return ViewStateOrDefault("CurrentGroupBy", ItemsGroupBy.Community)
        End Get
        Set(value As ItemsGroupBy)
            ViewState("CurrentGroupBy") = value
            LNBtoggleProjectCommunity.Visible = (value <> ItemsGroupBy.Community) AndAlso PageContainer <> PageContainerType.ProjectDashboard
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event GetCurrentFilter(ByRef filter As dtoItemsFilter)
    Public Event CompletionSaved(ByVal saved As Boolean, ByVal idTask As Long, ByVal taskname As String, updateSummary As Boolean)
    Public Event AllowMyCompletionSave()
    Public Event TasksCompletionSaved(ByVal savedTasks As Integer, ByVal unsavedTasks As Integer, ByVal tasks As Integer, updateSummary As Boolean)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTpageBottom)
            .setLiteral(LTtaskName_t)
            .setLiteral(LTtaskProjectName_t)
            .setLiteral(LTtaskStatus_t)
            .setLiteral(LTtaskDeadline_t)
            .setLiteral(LTtaskCompletion_t)
            .setLiteral(LTtaskMyCompletion_t)


            .setLinkButton(LNBtoggleTaskStatus, False, True)
            .setLinkButton(LNBtoggleProjectCommunity, False, True)
        End With
    End Sub

    Public Overrides Sub InitializeControl(context As dtoProjectContext, ByVal idContainerCommunity As Integer, filter As dtoItemsFilter, containerPage As PageContainerType, fromPage As PageListType, currentPage As PageListType)
        CurrentPresenter.InitView(context, idContainerCommunity, filter, containerPage, fromPage, currentPage, UnknownUser)
    End Sub
    Protected Overrides Function GetMyTasksCompletion() As List(Of dtoMyAssignmentCompletion)
        Dim items As New List(Of dtoMyAssignmentCompletion)

        For Each rContainer As RepeaterItem In RPTcontainer.Items
            Dim tRepeater As Repeater = rContainer.FindControl("RPTtasks")
            For Each row As RepeaterItem In tRepeater.Items
                Dim item As New dtoMyAssignmentCompletion()

                Dim oControl As UC_InLineInput = row.FindControl("CTRLmyCompleteness")
                If Not (String.IsNullOrEmpty(oControl.NewValue)) Then
                    item.MyCompletion.Current = oControl.NewValue
                End If
                item.MyCompletion.Init = oControl.OldValue
                item.MyCompletion.InEditMode = (oControl.NewValue <> oControl.OldValue)
                item.IdTask = CLng(DirectCast(row.FindControl("LTidTask"), Literal).Text)
                item.IdAssignment = CLng(DirectCast(row.FindControl("LTidAssignment"), Literal).Text)
                items.Add(item)
            Next
        Next
        Return items
    End Function

    Protected Overrides Sub SetPager(value As lm.Comol.Core.DomainModel.PagerBase)
        Me.PGgridBottom.Pager = value
        Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
    End Sub
    Protected Overrides Sub DisplayPager(display As Boolean)
        DVpagerBottom.Visible = display
    End Sub
    Protected Overrides Sub LoadedNoTasks()
        LoadTasks(New List(Of dtoTasksGroup))
    End Sub
    Public Overrides Sub SaveMyCompletions()
        Me.CurrentPresenter.SaveMyCompletions(GetMyTasksCompletion())
    End Sub
    Protected Overrides Sub DisplayTasksCompletionSaved(items As List(Of dtoMyAssignmentCompletion), savedTasks As Integer, unsavedTasks As Integer, updateSummary As Boolean)
        UpdateTasks(items)
        RaiseEvent TasksCompletionSaved(savedTasks, unsavedTasks, items.Where(Function(i) i.MyCompletion.InEditMode OrElse i.MyCompletion.IsUpdated).Count, updateSummary)
    End Sub
#End Region

#Region "Implements"
    Private Sub LoadTasks(items As List(Of dtoTasksGroup)) Implements IViewTasksListTree.LoadTasks
        THmyCompleteness.Visible = (PageType = PageListType.DashboardResource)
        THprojectName.Visible = (CurrentGroupBy <> ItemsGroupBy.Project AndAlso PageContainer <> PageContainerType.ProjectDashboard)
        THactions.Visible = (CurrentGroupBy = ItemsGroupBy.Project AndAlso (PageType = PageListType.DashboardManager OrElse PageType = PageListType.DashboardAdministrator))

        TDfooter.ColSpan = GetCellsCount(PageType, CurrentGroupBy)
        TDfooter.Visible = items.Any()
        SPNcommands.Visible = (items.Count > 0)


        RPTcontainer.DataSource = items
        RPTcontainer.DataBind()

        If items.Count > 0 AndAlso items.Where(Function(t) t.ProjectInfo.SetMyCompletion).Any() Then
            RaiseEvent AllowMyCompletionSave()
        End If
    End Sub
    Private Function GetCellsCount(ByVal cPage As PageListType, ByVal gType As ItemsGroupBy) As Integer Implements IViewTasksListTree.GetCellsCount
        Select Case cPage
            Case PageListType.DashboardResource
                If cPage = PageListType.DashboardResource AndAlso gType <> ItemsGroupBy.Project Then
                    Return 6
                Else
                    Return 5
                End If
            Case PageListType.ProjectDashboardManager
                Return 4
            Case PageListType.ProjectDashboardResource
                Return 5
            Case Else
                Return 5
        End Select
    End Function
    Private Function GetContainerCssClass(ByVal gType As ItemsGroupBy) As String Implements IViewTasksListTree.GetContainerCssClass
        Select Case gType
            Case ItemsGroupBy.Community
                Return LTitemCssClassCommunity.Text
            Case ItemsGroupBy.EndDate
                Return LTitemCssClassDeadline.Text
            Case Else
                Return ""
        End Select
    End Function
    Private Function GetPortalName() As String Implements IViewTasksListTree.GetPortalName
        Return Resource.getValue("ProjectContainer.Portal")
    End Function
    Private Function GetUnknownCommunityName() As String Implements IViewTasksListTree.GetUnknownCommunityName
        Return Resource.getValue("ProjectContainer.UnknownCommunity")
    End Function
    Private Function GetStartRowId(gType As ItemsGroupBy) As String Implements IViewTasksListTree.GetStartRowId
        Return gType.ToString.ToLower() & "-"
    End Function
    Private Function GetTimeTranslations() As Dictionary(Of TimeGroup, String) Implements IViewTasksListTree.GetTimeTranslations
        Return (From i As TimeGroup In [Enum].GetValues(GetType(TimeGroup)).Cast(Of TimeGroup)() Select i).ToDictionary(Function(i) i, Function(i) Resource.getValue("TimeGroup." & i.ToString()))
    End Function
    Private Function GetDateTimePattern() As String Implements IViewTasksListTree.GetDateTimePattern
        Return Resource.CultureInfo.DateTimeFormat.ShortDatePattern
    End Function
    Private Function GetMonthNames() As Dictionary(Of Integer, String) Implements IViewTasksListTree.GetMonthNames
        Return (From i As Integer In Enumerable.Range(1, 12) Select i).ToDictionary(Of Integer, String)(Function(i) i, Function(i) Resource.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i))
    End Function
    Private Overloads Sub DisplayTasksCompletionSaved(items As List(Of dtoMyAssignmentCompletion), projectCompletions As Dictionary(Of Long, Dictionary(Of ResourceActivityStatus, Long)), savedTasks As Integer, unsavedTasks As Integer, updateSummary As Boolean) Implements IViewTasksListTree.DisplayTasksCompletionSaved
        UpdateTasks(items, projectCompletions)
        RaiseEvent TasksCompletionSaved(savedTasks, unsavedTasks, items.Where(Function(i) i.MyCompletion.InEditMode OrElse i.MyCompletion.IsUpdated).Count, updateSummary)
    End Sub
#End Region

#Region "Internal"
    Protected Friend Function GetTableCssClass() As String
        Select Case PageType
            Case PageListType.DashboardManager, PageListType.DashboardAdministrator
                Return LTcssClassManager.Text & GetTableCssClass(CurrentGroupBy)
            Case PageListType.DashboardResource
                Return LTcssClassResource.Text & GetTableCssClass(CurrentGroupBy)
        End Select
        Return ""
    End Function
    Protected Friend Function GetTableCssClass(ByVal gType As ItemsGroupBy) As String
        Select Case gType
            Case ItemsGroupBy.Community
                Return " " & LTcssClassByCommunity.Text
            Case ItemsGroupBy.EndDate
                Return " " & LTcssClassByDeadline.Text
            Case ItemsGroupBy.Project
                Return " " & LTcssClassByProject.Text
        End Select
        Return ""
    End Function

    Private Sub RPTcontainer_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcontainer.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoTasksGroup = e.Item.DataItem
                Dim oTableRow As HtmlTableRow = e.Item.FindControl("TRpreviousPage")
                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBcontinueFromPreviousRow")
                oTableRow.Visible = (dto.PreviousPageIndex > -1)
                If dto.PreviousPageIndex > -1 Then
                    Resource.setLinkButton(oLinkButton, False, True)
                    oLinkButton.CommandArgument = dto.PreviousPageIndex
                    oTableRow.Attributes("class") = LTcssClassContinueTop.Text & dto.IdRow
                End If

                oTableRow = e.Item.FindControl("TRnextPage")
                oTableRow.Visible = (dto.NextPageIndex > -1)
                oLinkButton = e.Item.FindControl("LNBcontinueToNextRow")
                If dto.NextPageIndex > -1 Then
                    Resource.setLinkButton(oLinkButton, False, True)
                    oLinkButton.CommandArgument = dto.NextPageIndex
                    oTableRow.Attributes("class") = LTcssClassContinueBottom.Text & dto.IdRow
                End If

                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDgroupName")
                Select Case CurrentGroupBy
                    Case ItemsGroupBy.Project
                        oTableCell.Visible = False
                        oTableCell = e.Item.FindControl("TDprojectName")
                        oTableCell.Visible = True

                        Dim oHyperLink As HyperLink = e.Item.FindControl("HYPprojectName")
                        oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectDashboard
                        oHyperLink.Text = dto.Name
                        Dim oLabel As Label = e.Item.FindControl("LBcommunityName")
                        Select Case dto.ProjectInfo.IdCommunity
                            Case 0
                                oLabel.Text = Resource.getValue("ProjectContainer.Portal")
                            Case -1
                                oLabel.Text = Resource.getValue("ProjectContainer.UnknownCommunity")
                            Case Else
                                oLabel.Text = dto.ProjectInfo.CommunityName
                        End Select

                        oTableCell = e.Item.FindControl("TDprojectStatusCompleteness")
                        oTableCell.Visible = True
                        oLabel = e.Item.FindControl("LBprojectStatusCompleteness")
                        oLabel.ToolTip = dto.ProjectInfo.Completeness & "%"
                        oLabel.Text = String.Format(LTstatusContent.Text, dto.ProjectInfo.Completeness)
                        oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(dto.ProjectInfo.Completeness, dto.ProjectInfo.IsCompleted)


                        oTableCell = e.Item.FindControl("TDprojectStatus")
                        oTableCell.Visible = True
                        oLabel = e.Item.FindControl("LBprojectStatus")
                        oLabel.Text = Resource.getValue("Project.List.ProjectItemStatus." & dto.ProjectInfo.Status.ToString)

                        oTableCell = e.Item.FindControl("TDprojectDeadline")
                        oTableCell.Visible = True
                        oLabel = e.Item.FindControl("LBdeadline")
                        If dto.ProjectInfo.Deadline.HasValue Then
                            oLabel.Text = dto.ProjectInfo.Deadline.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                        ElseIf dto.ProjectInfo.EndDate.HasValue Then
                            oLabel.Text = dto.ProjectInfo.VirtualEndDate.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                        Else
                            oLabel.Text = "//"
                        End If

                        oTableCell = e.Item.FindControl("TDprojectMyCompleteness")
                        oTableCell.Visible = dto.ProjectInfo.SetMyCompletion OrElse dto.ProjectInfo.ViewMyCompletion
                        If dto.ProjectInfo.SetMyCompletion OrElse dto.ProjectInfo.ViewMyCompletion Then
                            Dim oProgressControl As UC_AdvancedProgressBar = e.Item.FindControl("CTRLmyCompletion")
                            oProgressControl.Visible = True
                            oProgressControl.InitializeControl(GenerateProgressBar(dto.ProjectInfo.UserCompletion))
                        ElseIf PageType = PageListType.DashboardResource Then
                            oTableCell = e.Item.FindControl("TDmyCompletenessEmpty")
                            oTableCell.Visible = True
                        End If
                        oTableCell = e.Item.FindControl("TDprojectActions")
                        oTableCell.Visible = (PageType <> PageListType.DashboardResource)
                        If (PageType <> PageListType.DashboardResource) Then
                            oHyperLink = e.Item.FindControl("HYPeditProjectMap")
                            Resource.setHyperLink(oHyperLink, False, True)
                            oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectUrls.ProjectMap
                            oHyperLink.Visible = dto.HasPermission(PmActivityPermission.ManageProject)

                            oHyperLink = e.Item.FindControl("HYPviewProjectMap")
                            Resource.setHyperLink(oHyperLink, False, True)
                            oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectUrls.ProjectMap
                            oHyperLink.Visible = dto.HasPermission(PmActivityPermission.ViewProjectMap) AndAlso Not dto.HasPermission(PmActivityPermission.ManageProject)


                            oHyperLink = e.Item.FindControl("HYPeditProject")
                            Resource.setHyperLink(oHyperLink, False, True)
                            oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectUrls.Edit
                            oHyperLink.Visible = dto.HasPermission(PmActivityPermission.ManageProject)
                        End If
                    Case Else
                        oTableCell.Visible = True
                        oTableCell.ColSpan = dto.CellsCount
                        oTableCell = e.Item.FindControl("TDprojectName")
                        oTableCell.Visible = False

                        oTableCell = e.Item.FindControl("TDprojectStatusCompleteness")
                        oTableCell.Visible = False
                        oTableCell = e.Item.FindControl("TDprojectStatus")
                        oTableCell.Visible = False
                        oTableCell = e.Item.FindControl("TDprojectDeadline")
                        oTableCell.Visible = False
                        oTableCell = e.Item.FindControl("TDprojectMyCompleteness")
                        oTableCell.Visible = False
                        oTableCell = e.Item.FindControl("TDprojectActions")
                        oTableCell.Visible = False
                End Select
                'oTableCell = e.Item.FindControl("TDmyCompleteness")
                'oTableCell.Visible = project.Permissions.ViewMyCompletion
                'If project.Permissions.ViewMyCompletion Then
                '    Dim oProgressControl As UC_AdvancedProgressBar = e.Item.FindControl("CTRLmyCompletion")
                '    oProgressControl.Visible = True
                '    oProgressControl.InitializeControl(GenerateProgressBar(project.UserCompletion))
                'End If
                'oHyperLink = e.Item.FindControl("HYPeditProjectMap")
                'Resource.setHyperLink(oHyperLink, False, True)
                'oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.ProjectMap
                'oHyperLink.Visible = project.Permissions.EditMap

                'oHyperLink = e.Item.FindControl("HYPviewProjectMap")
                'Resource.setHyperLink(oHyperLink, False, True)
                'oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.ProjectMap
                'oHyperLink.Visible = project.Permissions.ViewMap AndAlso Not project.Permissions.EditMap


                'oHyperLink = e.Item.FindControl("HYPeditProject")
                'Resource.setHyperLink(oHyperLink, False, True)
                'oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.Edit
                'oHyperLink.Visible = project.Permissions.Edit

            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTcontainer.Items.Count = 0)
                If (RPTcontainer.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    oTableCell.ColSpan = GetCellsCount(PageType, CurrentGroupBy)
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")

                    oLabel.Text = Resource.getValue("NoTasksForSelection")
                End If
        End Select
    End Sub

    Private Sub RPTcontainer_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcontainer.ItemCommand
        Select Case e.CommandName
            Case "gotopage"
                Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageType, CurrentGroupBy)
                RaiseEvent GetCurrentFilter(filter)

                filter.Ascending = CurrentAscending
                filter.OrderBy = CurrentOrderBy
                filter.PageIndex = CInt(e.CommandArgument)
                filter.PageSize = Me.PGgridBottom.Pager.PageSize
                CurrentPresenter.LoadTasks(UnknownUser, PageContext, IdContainerCommunity, filter, PageContainer, CurrentFromPage, PageType, CurrentGroupBy, filter.PageIndex, filter.PageSize)
        End Select
    End Sub

    Protected Sub RPTtasks_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim task As dtoPlainTask = e.Item.DataItem
                Dim oHyperLink As HyperLink = e.Item.FindControl("HYPtaskName")

                oHyperLink.Text = task.Name
                Dim oDialog As UC_TaskInfo = e.Item.FindControl("CTRLcompletionDialog")
                oDialog.InitializeControl(task, False, PageType)

                Dim oLabel As Label = Nothing

                oLabel = e.Item.FindControl("LBtaskStatusCompleteness")
                oLabel.ToolTip = task.Completeness & "%"
                oLabel.Text = String.Format(LTstatusContent.Text, task.Completeness)
                oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(task.Completeness, task.IsCompleted)

                oLabel = e.Item.FindControl("LBtaskStatus")

                oLabel.Text = Resource.getValue("Activity.ProjectItemStatus." & task.Status.ToString)

                oLabel = e.Item.FindControl("LBdeadline")
                If task.Deadline.HasValue Then
                    oLabel.Text = task.Deadline.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                ElseIf task.EndDate.HasValue Then
                    oLabel.Text = task.VirtualEndDate.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                Else
                    oLabel.Text = "//"
                End If

                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDmyCompleteness")
                oTableCell.Visible = task.ProjectInfo.SetMyCompletion OrElse task.ProjectInfo.ViewMyCompletion

                If task.ProjectInfo.SetMyCompletion OrElse task.ProjectInfo.ViewMyCompletion Then
                    Dim oInput As UC_InLineInput = e.Item.FindControl("CTRLmyCompleteness")
                    oInput.Visible = True
                    If task.ProjectInfo.SetMyCompletion AndAlso task.MyCompleteness.InEditMode Then
                        oInput.AutoInitialize(task.MyCompleteness.Init, task.MyCompleteness.Edit)
                    ElseIf task.ProjectInfo.SetMyCompletion Then
                        oInput.AutoInitialize(task.MyCompleteness.GetValue & IIf(task.MyCompleteness.GetValue.EndsWith("%"), "", "%"))
                    Else
                        oInput.ContainerCssClass &= " disabled"
                        oInput.AutoInitialize(task.MyCompleteness.GetValue & IIf(task.MyCompleteness.GetValue.EndsWith("%"), "", "%"))
                    End If
                End If

                If PageContainer = PageContainerType.ProjectDashboard Then
                    oTableCell = e.Item.FindControl("TDprojectName")
                    oTableCell.Visible = False
                End If
                oTableCell = e.Item.FindControl("TDactions")
                Select Case CurrentGroupBy
                    Case ItemsGroupBy.Project
                        Select Case PageType
                            Case PageListType.DashboardResource, PageListType.ProjectDashboardResource,
                                oTableCell.Visible = False
                            Case Else
                                oTableCell.Visible = True
                        End Select

                        oTableCell = e.Item.FindControl("TDprojectName")
                        oTableCell.Visible = False
                    Case Else
                        oTableCell.Visible = False
                        oHyperLink = e.Item.FindControl("HYPprojectName")
                        oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & task.ProjectDashboard
                        oHyperLink.Text = task.ProjectInfo.Name
                        oLabel = e.Item.FindControl("LBcommunityName")
                        Select Case task.ProjectInfo.IdCommunity
                            Case 0
                                oLabel.Text = Resource.getValue("ProjectContainer.Portal")
                            Case -1
                                oLabel.Text = Resource.getValue("ProjectContainer.UnknownCommunity")
                            Case Else
                                oLabel.Text = task.ProjectInfo.CommunityName
                        End Select

                End Select
        End Select
    End Sub
    Protected Sub RPTtasks_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim results As List(Of String) = e.CommandArgument.ToString().Split(",").ToList()

        If Boolean.TrueString = results(0) Then
            Dim oDialog As UC_TaskInfo = e.Item.FindControl("CTRLcompletionDialog")
            Dim oCell As HtmlTableCell = e.Item.FindControl("TDmyCompleteness")
            Dim oControl As UC_InLineInput = e.Item.FindControl("CTRLmyCompleteness")
            oControl.AutoInitialize(oDialog.GetMyCompletion)
            oCell.Attributes("class") = LTtdmyCompleteness.Text & " " & FieldStatus.updated.ToString

            Dim oLabel As Label = e.Item.FindControl("LBtaskStatus")
            oLabel.Text = oDialog.GetTaskStatus

            oLabel = e.Item.FindControl("LBtaskStatusCompleteness")
            oLabel.ToolTip = oDialog.GetTaskCompletion & "%"
            oLabel.Text = String.Format(LTstatusContent.Text, oDialog.GetTaskCompletion)
            oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(oDialog.GetTaskCompletion, oDialog.GetTaskIsCompleted)
           
        End If
        RaiseEvent CompletionSaved(Boolean.TrueString = results(0), CLng(DirectCast(e.Item.FindControl("LTidTask"), Literal).Text), DirectCast(e.Item.FindControl("HYPtaskName"), HyperLink).Text, Boolean.TrueString = results(1))
    End Sub
    Protected Sub UpdateProjectInfo(idProject As Long, completion As Dictionary(Of ResourceActivityStatus, Long))
        If CurrentGroupBy = ItemsGroupBy.Project Then
            For Each rContainer As RepeaterItem In RPTcontainer.Items
                If Not IsNothing(completion) Then
                    Dim oProgressControl As UC_AdvancedProgressBar = rContainer.FindControl("CTRLmyCompletion")
                    If oProgressControl.Visible AndAlso idProject = CLng(DirectCast(rContainer.FindControl("LTid"), Literal).Text) Then
                        oProgressControl.InitializeControl(GenerateProgressBar(completion))
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub


    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageType, CurrentGroupBy)
        RaiseEvent GetCurrentFilter(filter)

        filter.Ascending = CurrentAscending
        filter.OrderBy = CurrentOrderBy
        filter.PageIndex = Me.PGgridBottom.Pager.PageIndex
        filter.PageSize = Me.PGgridBottom.Pager.PageSize
        CurrentPresenter.LoadTasks(UnknownUser, PageContext, IdContainerCommunity, filter, PageContainer, CurrentFromPage, PageType, CurrentGroupBy, filter.PageIndex, filter.PageSize)
    End Sub
    Private Sub UpdateTasks(tasks As List(Of dtoMyAssignmentCompletion), Optional projectCompletions As Dictionary(Of Long, Dictionary(Of ResourceActivityStatus, Long)) = Nothing)
        For Each rContainer As RepeaterItem In RPTcontainer.Items
            If Not IsNothing(projectCompletions) Then
                Dim oProgressControl As UC_AdvancedProgressBar = rContainer.FindControl("CTRLmyCompletion")
                Dim idProject As Long = CLng(DirectCast(rContainer.FindControl("LTid"), Literal).Text)
                If oProgressControl.Visible AndAlso projectCompletions.ContainsKey(idProject) Then
                    oProgressControl.InitializeControl(GenerateProgressBar(projectCompletions(idProject)))
                End If
            End If

          

            Dim tRepeater As Repeater = rContainer.FindControl("RPTtasks")
            For Each row As RepeaterItem In tRepeater.Items
                Dim task As dtoMyAssignmentCompletion = tasks.Where(Function(t) t.IdAssignment = CLng(DirectCast(row.FindControl("LTidAssignment"), Literal).Text)).FirstOrDefault()

                Dim oCell As HtmlTableCell = row.FindControl("TDmyCompleteness")
                If Not IsNothing(task) Then
                    Dim oControl As UC_InLineInput = row.FindControl("CTRLmyCompleteness")
                    If task.MyCompletion.IsUpdated OrElse task.MyCompletion.InEditMode = False Then
                        oControl.AutoInitialize(task.MyCompletion.GetValue & IIf(task.MyCompletion.GetValue.EndsWith("%"), "", "%"))
                        Dim oDialog As UC_TaskInfo = row.FindControl("CTRLcompletionDialog")
                        oDialog.UpdateCompletion(task.MyCompletion.GetValue & IIf(task.MyCompletion.GetValue.EndsWith("%"), "", "%"), task.TaskCompletion, task.TaskStatus)
                        If task.MyCompletion.IsUpdated Then
                            oCell.Attributes("class") = LTtdmyCompleteness.Text & " " & task.MyCompletion.Status.ToString
                            Dim oLabel As Label = row.FindControl("LBtaskStatus")
                            oLabel.Text = Resource.getValue("Activity.ProjectItemStatus." & task.TaskStatus.ToString)

                            oLabel = row.FindControl("LBtaskStatusCompleteness")
                            oLabel.ToolTip = task.TaskCompletion & "%"
                            oLabel.Text = String.Format(LTstatusContent.Text, task.TaskCompletion)
                            oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(task.TaskCompletion, task.TaskIsCompleted)
                        Else
                            oCell.Attributes("class") = LTtdmyCompleteness.Text
                        End If
                    Else
                        oCell.Attributes("class") = LTtdmyCompleteness.Text & " " & task.MyCompletion.Status.ToString
                    End If
                Else
                    oCell.Attributes("class") = LTtdmyCompleteness.Text
                End If
            Next
        Next
    End Sub


  

#End Region

    
End Class