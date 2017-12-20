Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tags.Presentation
Imports lm.Comol.Core.Tag.Domain
Public Class BulkTagAssignments
    Inherits PageBase
    Implements IViewBulkTagsAssignment


#Region "Context"
    Private _presenter As BulkTagsAssignmentPresenter
    Protected Friend ReadOnly Property CurrentPresenter As BulkTagsAssignmentPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New BulkTagsAssignmentPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Preload"
    Private ReadOnly Property PreloadFromPage As Boolean Implements IViewBulkTagsAssignment.PreloadFromPage
        Get
            Return (Not String.IsNullOrEmpty(Request.QueryString("FromPage")) AndAlso Request.QueryString("FromPage").ToLower = Boolean.TrueString.ToLower())
        End Get
    End Property
    Private ReadOnly Property PreloadFromPortal As Boolean Implements IViewBulkTagsAssignment.PreloadFromPortal
        Get
            Return (String.IsNullOrEmpty(Request.QueryString("FromPortal")) OrElse Request.QueryString("FromPortal").ToLower = Boolean.TrueString.ToLower())
        End Get
    End Property
    Private ReadOnly Property PreloadIdOrganization As Integer Implements IViewBulkTagsAssignment.PreloadIdOrganization
        Get
            If IsNumeric(Request.QueryString("IdOrganization")) Then
                Return CInt(Request.QueryString("IdOrganization"))
            End If
            Return 0
        End Get
    End Property
    Private ReadOnly Property PreloadAssigned As Boolean Implements IViewBulkTagsAssignment.PreloadAssigned
        Get
            Return (Not String.IsNullOrEmpty(Request.QueryString("assigned")) AndAlso Request.QueryString("assigned").ToLower = Boolean.TrueString.ToLower())
        End Get
    End Property
#End Region

    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewBulkTagsAssignment.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpager.Visible = (Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize))
        End Set
    End Property
    Private Property CurrentPageSize As Integer Implements IViewBulkTagsAssignment.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Private ReadOnly Property CurrentPageIndex As Integer Implements IViewBulkTagsAssignment.CurrentPageIndex
        Get
            Return Pager.PageIndex
        End Get
    End Property
    Private Property CurrentFilters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewBulkTagsAssignment.CurrentFilters
        Get
            Return ViewStateOrDefault("CurrentFilters", New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters() With {.WithoutTags = True, .Availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All})
        End Get
        Set(value As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
    Private Property CurrentIdOrganization As Integer Implements IViewBulkTagsAssignment.CurrentIdOrganization
        Get
            Return ViewStateOrDefault("CurrentIdOrganization", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdOrganization") = value
        End Set
    End Property
    Private Property CurrentIdCommunity As Integer Implements IViewBulkTagsAssignment.CurrentIdCommunity
        Get
            Return ViewStateOrDefault("CurrentIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdCommunity") = value
        End Set
    End Property
    Private Property CurrentAssignedTags As List(Of dtoBulkCommunityForTags) Implements IViewBulkTagsAssignment.CurrentAssignedTags
        Get
            Return ViewStateOrDefault("CurrentAssignedTags", New List(Of dtoBulkCommunityForTags))
        End Get
        Set(value As List(Of dtoBulkCommunityForTags))
            ViewState("CurrentAssignedTags") = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewBulkTagsAssignment.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            LNBapplyTagsTop.Visible = value
            LNBapplyTagsBottom.Visible = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private _noItemsFound As Boolean
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PGgridBottom.Pager = Pager
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        If Not Page.IsPostBack Then
            CTRLfiltersHeader.FilterIdTransaction = Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID
        End If
        Dim url As String = ""
        If Not IsNothing(Request.UrlReferrer) Then
            url = Request.UrlReferrer.AbsoluteUri
        End If
        CurrentPresenter.InitView(CurrentPageSize, PreloadIdOrganization, PreloadAssigned, PreloadFromPortal, PreloadFromPage, url)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Dashboard", "Modules", "Dashboard")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLiteral(LTmultipleTagAssignmentsFiltersTitle)
            .setLinkButton(LNBapplySearchFilters, False, True)
            Master.ServiceTitle = Resource.getValue("MultipleTagAssignments.ServiceTitle")
            Master.ServiceNopermission = Resource.getValue("MultipleTagAssignments.ServiceTitle.NoPermission")

            .setLiteral(LTthCommunityName)
            .setLiteral(LTthTagToAssign)

            .setLinkButton(LNBapplyTagsTop, False, True)
            .setLinkButton(LNBapplyTagsBottom, False, True)
            .setHyperLink(HYPgoTo_BackPageFromBulkTop, False, True)
            .setHyperLink(HYPgoTo_BackPageFromBulkBottom, False, True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Actions/message"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleTags.ActionType) Implements IViewBulkTagsAssignment.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBulkTagsAssignment.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleTags.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBulkTagsAssignment.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.BulkTagsAssignment(PreloadIdOrganization, PreloadFromPortal, False), 0)
    End Sub
    Private Sub DisplaySessionTimeout(ByVal destinationUrl As String, idCommunity As Integer) Implements IViewBulkTagsAssignment.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub
    Private Sub RedirectToUrl(url As String) Implements IViewBulkTagsAssignment.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region
    Private Function GetSubmittedFilters() As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewBulkTagsAssignment.GetSubmittedFilters
        Dim filter As New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        filter.Availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All
        If Not Page.IsPostBack Then
            filter.IdOrganization = -1
            filter.IdcommunityType = -1
        End If

        With filter
            Dim keys As List(Of String) = Request.Form.AllKeys.ToList()
            For Each item As lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType))
                Select Case item
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.communitytype
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdcommunityType = CInt(Request.Form(item.ToString))
                        Else
                            .IdcommunityType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.coursetime
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdCourseTime = CInt(Request.Form(item.ToString))
                        Else
                            .IdCourseTime = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.degreetype
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdDegreeType = CInt(Request.Form(item.ToString))
                        Else
                            .IdDegreeType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.organization
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdOrganization = CInt(Request.Form(item.ToString))
                        Else
                            .IdOrganization = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.responsible
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdResponsible = CInt(Request.Form(item.ToString))
                        Else
                            .IdResponsible = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.status
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Status = CInt(Request.Form(item.ToString))
                        Else
                            .Status = lm.Comol.Core.Communities.CommunityStatus.None
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.year
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Year = CInt(Request.Form(item.ToString))
                        Else
                            .Year = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tagassociation
                        .WithoutTags = (Request.Form(item.ToString & "_0") = "on")
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag
                        .IdTags = New List(Of Long)
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            For Each idTag As String In Request.Form(item.ToString).Split(",")
                                .IdTags.Add(CLng(idTag))
                            Next
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.name
                        .SearchBy = lm.Comol.Core.BaseModules.CommunityManagement.SearchCommunitiesBy.Contains
                        .Value = Request.Form(item.ToString)
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.letters
                        .StartWith = ""
                        If  Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            Dim charInt As Integer = CInt(Request.Form(item.ToString))
                            Select Case charInt
                                Case -1
                                    .StartWith = ""
                                Case -9
                                    .StartWith = "#"
                                Case Else
                                    .StartWith = Char.ConvertFromUtf32(charInt).ToLower()
                            End Select
                        End If
                End Select
            Next
        End With

        Return filter
    End Function
    Private Sub LoadCommunities(communities As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags)) Implements IViewBulkTagsAssignment.LoadCommunities
        RPTcommunities.DataSource = communities
        RPTcommunities.DataBind()
        CBXselectAll.Visible = (communities.Count > 1)
        CTRLbulkActionTop.Visible = (communities.Count > 1)
        If communities.Count > 15 Then
            CTRLbulkActionBottom.Visible = True
            DVbottomCommands.Visible = True
        End If
        LNBapplyTagsBottom.Visible = (communities.Count > 0)
        LNBapplyTagsTop.Visible = (communities.Count > 0)
    End Sub
    Private Sub DisplayErrorFromDB() Implements IViewBulkTagsAssignment.DisplayErrorFromDB
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewBulkTagsAssignment.DisplayErrorFromDB"), Helpers.MessageType.error)
    End Sub
    Private Sub SetBackUrl(url As String) Implements IViewBulkTagsAssignment.SetBackUrl
        HYPgoTo_BackPageFromBulkBottom.NavigateUrl = url
        HYPgoTo_BackPageFromBulkBottom.Visible = Not String.IsNullOrEmpty(url)
        HYPgoTo_BackPageFromBulkTop.NavigateUrl = url
        HYPgoTo_BackPageFromBulkTop.Visible = Not String.IsNullOrEmpty(url)
    End Sub
    Private Sub HideFilters() Implements IViewBulkTagsAssignment.HideFilters
        DVfilters.Visible = False
    End Sub
    Private Sub LoadDefaultTags(tags As List(Of dtoTagSelectItem), hasMultiPage As Boolean) Implements IViewBulkTagsAssignment.LoadDefaultTags
        CTRLbulkActionBottom.InitializeControl(tags, hasMultiPage)
        CTRLbulkActionTop.InitializeControl(tags, hasMultiPage)
    End Sub
    Private Function GetAssignedTags() As List(Of dtoBulkCommunityForTags) Implements IViewBulkTagsAssignment.GetSelectedItems
        Dim result As New List(Of dtoBulkCommunityForTags)

        For Each row As RepeaterItem In RPTcommunities.Items
            Dim oLiteral As Literal = row.FindControl("LTidCommunity")
            Dim oControl As UC_TagsSelectorForCommunity = row.FindControl("CTRLtagsSelector")
            Dim item As New dtoBulkCommunityForTags
            item.IdCommunity = CInt(oLiteral.Text)
            item.IdSelectedTags = oControl.GetSelectedTags
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXcommunity")
            item.Selected = oCheck.Checked
            result.Add(item)
        Next
        Return result
    End Function

    Private Sub DisplayMessage(action As ModuleTags.ActionType) Implements IViewBulkTagsAssignment.DisplayMessage
        Dim mType As Helpers.MessageType = Helpers.MessageType.error
        CTRLmessages.Visible = True
        If action = ModuleTags.ActionType.BulkTagsAssigned Then
            mType = Helpers.MessageType.success
        End If
        CTRLmessages.InitializeControl(Resource.getValue("DisplayMessage.ActionType." & action.ToString), mType)
    End Sub

    Private Function GetSelectedCommunities() As List(Of Integer) Implements IViewBulkTagsAssignment.GetSelectedCommunities
        Return GetAssignedTags.Where(Function(s) s.Selected).Select(Function(s) s.IdCommunity).Distinct().ToList
    End Function
    Private Sub DeselectAll() Implements IViewBulkTagsAssignment.DeselectAll
        CBXselectAll.Checked = False
    End Sub
    Private Sub LoadDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Implements IViewBulkTagsAssignment.LoadDefaultFilters
        CTRLfiltersHeader.SetDefaultFilters(filters)
    End Sub
    Private Sub DisplayNoCommunitiesToLoad() Implements IViewBulkTagsAssignment.DisplayNoCommunitiesToLoad
        HideFilters()
        _noItemsFound = True
        LoadCommunities(New List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags))
    End Sub
#End Region

#Region "Internal"
    Private Sub MultipleTagAssignments_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
    Public Function GetCollapsedCssClass() As String
        If Not Page.IsPostBack Then
            Response.SetCookie(New HttpCookie("collapsed-cl-filters.MultipleTags", "false"))
            Return ""
        ElseIf Not Page.IsPostBack Then
            Response.SetCookie(New HttpCookie("collapsed-cl-filters.MultipleTags", "true"))
            Return LTcssClassCollapsed.Text
        End If
        Return ""
    End Function
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
    Private Sub LNBapplyTagsTop_Click(sender As Object, e As EventArgs) Handles LNBapplyTagsTop.Click, LNBapplyTagsBottom.Click
        CTRLmessages.Visible = False
        CurrentPresenter.ApplyTags(GetAssignedTags.Where(Function(s) s.Selected).ToList(), CurrentFilters, CurrentIdCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        CTRLmessages.Visible = False
        CurrentPresenter.LoadCommunities(CurrentIdCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub LNBapplySearchFilters_Click(sender As Object, e As EventArgs) Handles LNBapplySearchFilters.Click
        CTRLmessages.Visible = False
        CurrentPresenter.ApplyFilters(CurrentIdCommunity, GetSubmittedFilters, CurrentPageSize)
    End Sub
    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags = e.Item.DataItem
                Dim oControl As UC_TagsSelectorForCommunity = e.Item.FindControl("CTRLtagsSelector")
                oControl.InitializeControl(item.AvailableTags)
                Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBXcommunity")
                oCheck.Checked = CurrentAssignedTags.Where(Function(a) a.IdCommunity.Equals(item.Id) AndAlso a.Selected).Any
            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTcommunities.Items.Count = 0)
                If (RPTcommunities.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")

                    oLabel.Text = Resource.getValue("NoCommunitiesForBulkTagAssignmentsFound")
                End If
        End Select
    End Sub
    Private Sub CTRLbulkActionTop_ApplyTags(tags As List(Of Long), applyToAll As Boolean) Handles CTRLbulkActionTop.ApplyTags
        Dim idCommunities As List(Of Integer)
        If Not applyToAll Then
            idCommunities = GetSelectedCommunities()
        End If
        CurrentPresenter.ApplyTags(idCommunities, tags, applyToAll, CurrentFilters, CurrentIdCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub CTRLbulkActionBottom_ApplyTags(tags As List(Of Long), applyToAll As Boolean) Handles CTRLbulkActionBottom.ApplyTags
        Dim idCommunities As List(Of Integer)
        If Not applyToAll Then
            idCommunities = GetSelectedCommunities()
        End If
        CurrentPresenter.ApplyTags(idCommunities, tags, applyToAll, CurrentFilters, CurrentIdCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
#End Region

    
End Class