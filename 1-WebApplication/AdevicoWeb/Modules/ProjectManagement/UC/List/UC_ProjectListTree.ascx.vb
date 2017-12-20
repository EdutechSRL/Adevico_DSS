Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain

Public Class UC_ProjectListTree
    Inherits PMprojectListBase
    Implements IViewProjectListTree

#Region "Context"
    Private _Presenter As ProjectListTreePresenter
    Private ReadOnly Property CurrentPresenter() As ProjectListTreePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProjectListTreePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewProjectListTree.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property
    Public Property CurrentPageSize As Integer Implements IViewProjectListTree.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Public Property CurrentGroupBy As ItemsGroupBy Implements IViewProjectListTree.CurrentGroupBy
        Get
            Return ViewStateOrDefault("CurrentGroupBy", ItemsGroupBy.Community)
        End Get
        Set(value As ItemsGroupBy)
            ViewState("CurrentGroupBy") = value
            LNBtoggleProjectCommunity.Visible = (value <> ItemsGroupBy.Community)
        End Set
    End Property
    Public ReadOnly Property CurrentPageIndex As Integer Implements IViewProjectListTree.CurrentPageIndex
        Get
            Return Pager.PageIndex
        End Get
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
    Public Event GetCurrentFilter(ByRef filter As dtoItemsFilter)
    Public Event AddDeletedStatus()
    Public Event RemoveDeletedStatus()
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTpageBottom)
            .setLiteral(LTprojectName_t)
            .setLiteral(LTprojectStatus_t)
            .setLiteral(LTprojectDeadline_t)
            .setLiteral(LTprojectRoles_t)
            .setLiteral(LTmyCompletion_t)
            .setLiteral(LTprojectCompletion_t)
            .setLabel(LBprojectActions_t)

            .setLinkButton(LNBtoggleProjectStatus, False, True)
            .setLinkButton(LNBtoggleProjectRoles, False, True)
            .setLinkButton(LNBtoggleProjectCommunity, False, True)
        End With
    End Sub
    Protected Overrides Sub DisplayToggleProjectRoles(display As Boolean)
        LNBtoggleProjectRoles.Visible = display
    End Sub
#End Region
#Region "Implements"
    Public Sub InitializeControl(context As dtoProjectContext, filter As dtoItemsFilter, cPageType As PageListType) Implements IViewProjectListTree.InitializeControl
        CurrentPageType = cPageType
        CurrentPresenter.InitView(context, filter, cPageType)
    End Sub
    Private Sub DisplayPager(display As Boolean) Implements IViewProjectListTree.DisplayPager
        DVpagerBottom.Visible = display
    End Sub

    Private Sub LoadedNoProjects(view As PageListType) Implements IViewProjectListTree.LoadedNoProjects
        THmyCompleteness.Visible = (view = PagelistType.ListResource)
        THroles.Visible = (view <> PageListType.ListAdministrator)
        Select Case view
            Case PageListType.ListAdministrator
                TDfooter.ColSpan = 5
            Case PageListType.ListManager
                TDfooter.ColSpan = 6
            Case PageListType.ListResource
                TDfooter.ColSpan = 7
        End Select
        RPTcontainer.DataSource = New List(Of dtoPlainProject)
        RPTcontainer.DataBind()
        TDfooter.Visible = False
    End Sub
    Private Sub LoadProjects(items As List(Of dtoProjectsGroup), view As PageListType) Implements IViewProjectListTree.LoadProjects
        THmyCompleteness.Visible = (view = PageListType.ListResource)
        THroles.Visible = (view <> PageListType.ListAdministrator)
        Select Case view
            Case PageListType.ListAdministrator
                TDfooter.ColSpan = 5
            Case PageListType.ListManager
                TDfooter.ColSpan = 6
            Case PageListType.ListResource
                TDfooter.ColSpan = 7
        End Select
        RPTcontainer.DataSource = items
        RPTcontainer.DataBind()
        TDfooter.Visible = True
        SPNcommands.Visible = (items.Count > 0)
    End Sub

    Private Function GetCellsCount(ByVal view As PageListType) As Integer Implements IViewProjectListTree.GetCellsCount
        Select Case view
            Case PagelistType.ListResource
                Return 7
            Case Else
                Return 6
        End Select
    End Function
    Private Function GetContainerCssClass(ByVal gType As ItemsGroupBy) As String Implements IViewProjectListTree.GetContainerCssClass
        Select Case gType
            Case ItemsGroupBy.Community
                Return LTitemCssClassCommunity.Text
            Case ItemsGroupBy.EndDate
                Return LTitemCssClassDeadline.Text
            Case Else
                Return ""
        End Select
    End Function
    Private Function GetPortalName() As String Implements IViewProjectListTree.GetPortalName
        Return Resource.getValue("ProjectContainer.Portal")
    End Function
    Private Function GetUnknownCommunityName() As String Implements IViewProjectListTree.GetUnknownCommunityName
        Return Resource.getValue("ProjectContainer.UnknownCommunity")
    End Function
    Private Function GetStartRowId(gType As ItemsGroupBy) As String Implements IViewProjectListTree.GetStartRowId
        Return gType.ToString.ToLower() & "-"
    End Function
    Private Function GetTimeTranslations() As Dictionary(Of TimeGroup, String) Implements IViewProjectListTree.GetTimeTranslations
        Return (From i As TimeGroup In [Enum].GetValues(GetType(TimeGroup)).Cast(Of TimeGroup)() Select i).ToDictionary(Function(i) i, Function(i) Resource.getValue("TimeGroup." & i.ToString()))
    End Function
    Private Function GetDateTimePattern() As String Implements IViewProjectListTree.GetDateTimePattern
        Return Resource.CultureInfo.DateTimeFormat.ShortDatePattern
    End Function
    Private Function GetMonthNames() As Dictionary(Of Integer, String) Implements IViewProjectListTree.GetMonthNames
        Return (From i As Integer In Enumerable.Range(1, 12) Select i).ToDictionary(Of Integer, String)(Function(i) i, Function(i) Resource.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i))
    End Function
#End Region

#Region "Internal"
    Protected Friend Function GetTableCssClass() As String
        Select Case CurrentPageType
            Case PageListType.ListManager, PageListType.ListAdministrator
                Return LTcssClassManager.Text & GetTableCssClass(CurrentGroupBy)
            Case PageListType.ListResource
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
        End Select
        Return ""
    End Function

    Private Sub RPTcontainer_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcontainer.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoProjectsGroup = e.Item.DataItem
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
            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTcontainer.Items.Count = 0)
                If (RPTcontainer.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    oTableCell.ColSpan = IIf(CurrentPageType = PageListType.ListResource, 7, 6)
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")

                    oLabel.Text = Resource.getValue("NoProjectsForSelection")
                End If
        End Select
    End Sub

    Private Sub RPTcontainer_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcontainer.ItemCommand
        Select Case e.CommandName
            Case "gotopage"
                Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageContainerType.ProjectsList, CurrentGroupBy)
                RaiseEvent GetCurrentFilter(filter)

                filter.Ascending = CurrentAscending
                filter.OrderBy = CurrentOrderBy
                filter.PageIndex = CInt(e.CommandArgument)
                filter.PageSize = Me.PGgridBottom.Pager.PageSize
                CurrentPresenter.LoadProjects(filter, CurrentGroupBy, CurrentPageType, IdCurrentCommunityForList, CInt(e.CommandArgument), filter.PageSize)
        End Select
    End Sub
    Protected Sub RPTprojects_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageContainerType.ProjectsList, CurrentGroupBy)
        RaiseEvent GetCurrentFilter(filter)
        Select Case e.CommandName
            Case "virtualdetete"
                If CurrentPresenter.VirtualDeleteProject(CLng(e.CommandArgument), filter, CurrentGroupBy, CurrentPageType, IdCurrentCommunityForList, filter.PageIndex, filter.PageSize) Then
                    RaiseEvent AddDeletedStatus()
                End If
            Case ("recover")
                Dim deletedRecords As Long = 0
                CurrentPresenter.VirtualUndeleteProject(CLng(e.CommandArgument), filter, CurrentGroupBy, CurrentPageType, IdCurrentCommunityForList, filter.PageIndex, filter.PageSize, deletedRecords)
                If (deletedRecords = 0) Then
                    RaiseEvent RemoveDeletedStatus()
                End If
        End Select
    End Sub
    Protected Sub RPTprojects_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim project As dtoPlainProject = e.Item.DataItem
                Dim oHyperLink As HyperLink = e.Item.FindControl("HYPprojectName")

                oHyperLink.Text = project.Name
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.ProjectUrl

                Dim oLabel As Label = e.Item.FindControl("LBcommunityName")
                Select Case project.IdCommunity
                    Case 0
                        oLabel.Text = Resource.getValue("ProjectContainer.Portal")
                    Case -1
                        oLabel.Text = Resource.getValue("ProjectContainer.UnknownCommunity")
                    Case Else
                        oLabel.Text = project.CommunityName
                End Select
                oLabel = e.Item.FindControl("LBprojectStatusCompleteness")
                oLabel.ToolTip = project.Completeness & "%"
                oLabel.Text = String.Format(LTstatusContent.Text, project.Completeness)
                oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(project.Completeness, project.IsCompleted)

                oLabel = e.Item.FindControl("LBprojectStatus")

                oLabel.Text = Resource.getValue("Project.List.ProjectItemStatus." & project.Status.ToString)

                oLabel = e.Item.FindControl("LBdeadline")
                If project.Deadline.HasValue Then
                    oLabel.Text = project.Deadline.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                ElseIf project.EndDate.HasValue Then
                    oLabel.Text = project.EndDate.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                Else
                    oLabel.Text = "//"
                End If

                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDroles")
                If CurrentPageType = PageListType.ListAdministrator Then
                    oTableCell.Visible = False
                Else
                    Dim oControl As UC_InLineProjectRoles = e.Item.FindControl("CTRLroles")
                    oControl.InitializeControl(project.Roles)
                End If
                oTableCell = e.Item.FindControl("TDmyCompleteness")
                oTableCell.Visible = project.Permissions.ViewMyCompletion
                If project.Permissions.ViewMyCompletion Then
                    Dim oProgressControl As UC_AdvancedProgressBar = e.Item.FindControl("CTRLmyCompletion")
                    oProgressControl.Visible = True
                    oProgressControl.InitializeControl(GenerateProgressBar(project.UserCompletion))
                End If
                oHyperLink = e.Item.FindControl("HYPeditProjectMap")
                Resource.setHyperLink(oHyperLink, False, True)
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.ProjectMap
                oHyperLink.Visible = project.Permissions.EditMap

                oHyperLink = e.Item.FindControl("HYPviewProjectMap")
                Resource.setHyperLink(oHyperLink, False, True)
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.ProjectMap
                oHyperLink.Visible = project.Permissions.ViewMap AndAlso Not project.Permissions.EditMap

                oHyperLink = e.Item.FindControl("HYPeditProjectResources")
                Resource.setHyperLink(oHyperLink, False, True)
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.EditResources
                oHyperLink.Visible = project.Permissions.EditResources

                oHyperLink = e.Item.FindControl("HYPeditProject")
                Resource.setHyperLink(oHyperLink, False, True)
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.Edit
                oHyperLink.Visible = project.Permissions.Edit

                oHyperLink = e.Item.FindControl("HYPdeleteProject")
                Resource.setHyperLink(oHyperLink, False, True)
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & project.Urls.PhisicalDelete
                oHyperLink.Visible = project.Permissions.PhisicalDelete

                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBvirtualDeleteProject")

                Resource.setLinkButton(oLinkButton, False, True)
                oLinkButton.Visible = project.Permissions.VirtualDelete

                oLinkButton = e.Item.FindControl("LNBvirtualUnDeleteProject")
                Resource.setLinkButton(oLinkButton, False, True)
                oLinkButton.Visible = project.Permissions.VirtualUndelete
        End Select
    End Sub

    Protected Friend Function RowCssClass(project As dtoPlainProject) As String
        If project.Deadline.HasValue Then
            Return LThasdeadline.Text
        Else
            Return LTnodeadline.Text
        End If
    End Function
    Private Function GetCssStatuslight(completeness As Integer)
        If completeness = 100 Then
            Return "green"
        ElseIf completeness > 0 Then
            Return "yellow"
        Else
            Return "gray"
        End If
    End Function
    Private Function GetCssStatuslight(completeness As Integer, isCompleted As Boolean)
        If completeness = 100 AndAlso isCompleted Then
            Return "green"
        ElseIf completeness > 0 Then
            Return "yellow"
        Else
            Return "gray"
        End If
    End Function
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageContainerType.ProjectsList, CurrentGroupBy)
        RaiseEvent GetCurrentFilter(filter)

        filter.Ascending = CurrentAscending
        filter.OrderBy = CurrentOrderBy
        filter.PageIndex = Me.PGgridBottom.Pager.PageIndex
        filter.PageSize = Me.PGgridBottom.Pager.PageSize
        CurrentPresenter.LoadProjects(filter, CurrentGroupBy, CurrentPageType, IdCurrentCommunityForList, filter.PageIndex, filter.PageSize)
    End Sub
    Private Function GenerateProgressBar(items As Dictionary(Of ResourceActivityStatus, Long)) As lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar
        Dim bar As lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar = Nothing
        If Not IsNothing(items) AndAlso items.ContainsKey(ResourceActivityStatus.all) Then
            bar = New lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar(items(ResourceActivityStatus.all))

            For Each item As KeyValuePair(Of ResourceActivityStatus, Long) In items.Where(Function(i) i.Value > 0).Select(Function(i) i).ToList()
                bar.Items.Add(New lm.Comol.Core.BaseModules.WebControls.Generic.ProgressBarItem(item.Value, items(ResourceActivityStatus.all)) _
                              With {.CssClass = item.Key.ToString, .PercentageTranslation = Resource.getValue("ProgressBarItem.PercentageTranslation." & item.Key.ToString), .ValueTranslation = Resource.getValue("ProgressBarItem.ValueTranslation")})


            Next
            If bar.Items.Count = 1 Then
                bar.Items(0).DisplayOrder = lm.Comol.Core.DomainModel.ItemDisplayOrder.first Or lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            ElseIf bar.Items.Count > 1 Then
                bar.Items.First().DisplayOrder = lm.Comol.Core.DomainModel.ItemDisplayOrder.first
                bar.Items.Last().DisplayOrder = lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            End If
        End If
        Return bar
    End Function
#End Region

End Class