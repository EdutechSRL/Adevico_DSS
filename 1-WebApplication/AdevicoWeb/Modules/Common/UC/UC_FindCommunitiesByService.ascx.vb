Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports System.Linq
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.BaseModules.CommunityManagement

Public Class UC_FindCommunitiesByService
    Inherits DBbaseControl
    Implements IViewSearchCommunities

#Region "Context"
    Private _Presenter As SearchCommunitiesPresenter
    Private ReadOnly Property CurrentPresenter() As SearchCommunitiesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SearchCommunitiesPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewSearchCommunities.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property AdministrationMode As Boolean Implements IViewSearchCommunities.AdministrationMode
        Get
            Return ViewStateOrDefault("AdministrationMode", False)
        End Get
        Set(value As Boolean)
            ViewState("AdministrationMode") = value
        End Set
    End Property

    Private Property SearchFilters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewSearchCommunities.SearchFilters
        Get
            Try
                Return ViewState("SearchFilters")
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
        Set(value As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters)
            ViewState("SearchFilters") = value
        End Set
    End Property
    Public Property OrderAscending As Boolean Implements IViewSearchCommunities.OrderAscending
        Get
            Return ViewStateOrDefault("OrderAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("OrderAscending") = value
        End Set
    End Property
    Public Property OrderBy As lm.Comol.Core.Dashboard.Domain.OrderItemsBy Implements IViewSearchCommunities.OrderBy
        Get
            Return ViewStateOrDefault("OrderBy", lm.Comol.Core.Dashboard.Domain.OrderItemsBy.Name)
        End Get
        Set(value As lm.Comol.Core.Dashboard.Domain.OrderItemsBy)
            ViewState("OrderBy") = value
        End Set
    End Property
    Private Property CurrentAvailability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability Implements IViewSearchCommunities.CurrentAvailability
        Get
            Return ViewStateOrDefault("CurrentAvailability", lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed)
        End Get
        Set(value As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability)
            ViewState("CurrentAvailability") = value
        End Set
    End Property
    Private Property CurrentPageSize As Integer Implements IViewSearchCommunities.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewSearchCommunities.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = CurrentPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            LBpagesize_t.Visible = (Not value.Count < Me.DefaultPageSize)
            DDLpage.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Private ReadOnly Property MultipleSelection As Boolean Implements IViewSearchCommunities.MultipleSelection
        Get
            Return (Me.SelectionMode = ListSelectionMode.Multiple)
        End Get
    End Property
    Private Property IdProfile As Integer Implements IViewSearchCommunities.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property RaiseCommunityChangedEvent As Boolean Implements IViewSearchCommunities.RaiseCommunityChangedEvent
        Get
            Return ViewStateOrDefault("RaiseCommunityChangedEvent", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommunityChangedEvent") = value
        End Set
    End Property
    Private Property ExcludeCommunities As List(Of Integer) Implements IViewSearchCommunities.ExcludeCommunities
        Get
            Return ViewStateOrDefault("ExcludeCommunities", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("ExcludeCommunities") = value
        End Set
    End Property
    Private Property OnlyFromOrganizations As List(Of Integer) Implements IViewSearchCommunities.OnlyFromOrganizations
        Get
            Return ViewStateOrDefault("OnlyFromOrganizations", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("OnlyFromOrganizations") = value
        End Set
    End Property
    Public Property HasAvailableCommunities As Boolean Implements IViewSearchCommunities.HasAvailableCommunities
        Get
            Return ViewStateOrDefault("HasAvailableCommunities", False)
        End Get
        Set(value As Boolean)
            ViewState("HasAvailableCommunities") = value
        End Set
    End Property
    'Public Property CurrentAvailability As CommunityAvailability Implements IViewSearchCommunitiesByService.CurrentAvailability
    '    Get
    '        Return ViewStateOrDefault("CurrentAvailability", CommunityAvailability.All)
    '    End Get
    '    Set(value As CommunityAvailability)
    '        ViewState("CurrentAvailability") = value
    '    End Set
    'End Property
    Private Property SelectedIdCommunities As List(Of Integer) Implements IViewSearchCommunities.SelectedIdCommunities
        Get
            Return ViewStateOrDefault("SelectedIdCommunities", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("SelectedIdCommunities") = value
        End Set
    End Property

    Private Property RequiredPermissions As Dictionary(Of Integer, Long) Implements IViewSearchCommunities.RequiredPermissions
        Get
            Return ViewStateOrDefault("RequiredPermissions", New List(Of lm.Comol.Core.DomainModel.dtoModulePermission)).ToDictionary(Function(p) p.IdModule, Function(p) p.Permissions)
        End Get
        Set(value As Dictionary(Of Integer, Long))
            ViewState("RequiredPermissions") = value.Select(Function(p) New lm.Comol.Core.DomainModel.dtoModulePermission() With {.IdModule = p.Key, .Permissions = p.Value}).ToList()
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
    Private _noItemsToDisplay As Boolean
    Public Event LoadDefaultFiltersToHeader(filters As List(Of Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
    Public ReadOnly Property DefaultPageSize() As Integer
        Get
            Return Me.DDLpage.Items(0).Value
        End Get
    End Property
    Public Property SelectionMode As ListSelectionMode
        Get
            Return ViewStateOrDefault("SelectionMode", ListSelectionMode.Multiple)
        End Get
        Set(value As ListSelectionMode)
            ViewState("SelectionMode") = value
        End Set
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Pager
        'Me.Page.Form.DefaultButton = Me.BTNcerca.UniqueID
        Me.Page.Form.DefaultFocus = LNBapplySearchFilters.UniqueID
        'Me.Master.Page.Form.DefaultButton = Me.BTNcerca.UniqueID
        Me.Page.Master.Page.Form.DefaultFocus = LNBapplySearchFilters.UniqueID
    End Sub

#Region "Inherits"
    'Protected Overrides Sub SetCultureSettings()
    '    MyBase.SetCulture("pg_UC_SelectCommunities", "Modules", "Common")
    'End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBpagesize_t)
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLiteral(LTsearchFiltersTitle)
            .setLinkButton(LNBapplySearchFilters, False, True)
        End With
    End Sub
#End Region

#Region "implements"
    Public Sub InitializeAdministrationControl(idProfile As Integer, unloadIdCommunities As List(Of Integer), preloadedAvailability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Implements IViewSearchCommunities.InitializeAdministrationControl
        _noItemsToDisplay = True
        CurrentPresenter.InitAdministrationView(idProfile, unloadIdCommunities, preloadedAvailability, onlyFromOrganizations)
    End Sub
    Public Sub InitializeControlByModules(idProfile As Integer, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), preloadedAvailability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Implements IViewSearchCommunities.InitializeControlByModules
        _noItemsToDisplay = True
        CurrentPresenter.InitByModulesView(idProfile, requiredPermissions, unloadIdCommunities, preloadedAvailability, onlyFromOrganizations)
    End Sub
    Public Sub InitializeControlByModules(idProfile As Integer, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), preloadedAvailability As CommunityAvailability) Implements IViewSearchCommunities.InitializeControlByModules
        _noItemsToDisplay = True
        InitializeControlByModules(idProfile, requiredPermissions, unloadIdCommunities, preloadedAvailability, New List(Of Integer))
    End Sub
    Public Sub ReloadAdministrationControl(unloadIdCommunities As List(Of Integer), preloadedAvailability As CommunityAvailability) Implements IViewSearchCommunities.ReloadAdministrationControl
        _noItemsToDisplay = True
        CurrentPresenter.RefreshAdministrationView(unloadIdCommunities, preloadedAvailability)
    End Sub

    Public Sub ReloadControlByModules(unloadIdCommunities As List(Of Integer), preloadedAvailability As CommunityAvailability) Implements IViewSearchCommunities.ReloadControlByModules
        _noItemsToDisplay = True
        CurrentPresenter.RefreshByModuleView(unloadIdCommunities, preloadedAvailability)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewSearchCommunities.DisplaySessionTimeout
        MLVsearch.SetActiveView(VIWsessionTimeout)
    End Sub
    Private Sub LoadNothing() Implements IViewSearchCommunities.LoadNothing
        _noItemsToDisplay = True
        Me.RPTcommunities.DataSource = New List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem)
        Me.RPTcommunities.DataBind()

        MLVsearch.SetActiveView(VIWlist)
    End Sub
    Private Sub LoadDefaultFilters(filters As List(Of Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Implements IViewSearchCommunities.LoadDefaultFilters
        RaiseEvent LoadDefaultFiltersToHeader(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub

    Private Sub LoadItems(items As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem)) Implements IViewSearchCommunities.LoadItems
        Me.RPTcommunities.DataSource = items
        Me.RPTcommunities.DataBind()

        MLVsearch.SetActiveView(VIWlist)
    End Sub
    Public Function GetIdSelectedItems() As List(Of Integer) Implements IViewSearchCommunities.GetIdSelectedItems
        Dim selection As Dictionary(Of Boolean, List(Of Integer)) = GetCurrentSelection()
        Dim currentItems As List(Of Integer) = selection(True)
        currentItems.AddRange(SelectedIdCommunities.Where(Function(c) Not selection(False).Contains(c)).ToList())

        SelectedIdCommunities = currentItems.Distinct.ToList()
        Return currentItems
    End Function
    Public Function GetSelectedItems() As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem) Implements IViewSearchCommunities.GetSelectedItems
        Return Me.CurrentPresenter.GetSelectedCommunities(SelectedIdCommunities, ExcludeCommunities, IdProfile)
    End Function
    Private Function GetCurrentSelection() As Dictionary(Of Boolean, List(Of Integer)) Implements IViewSearchCommunities.GetCurrentSelection
        Dim result As New Dictionary(Of Boolean, List(Of Integer))
        result.Add(False, New List(Of Integer))
        result.Add(True, New List(Of Integer))
        Dim isMultiple As Boolean = MultipleSelection
        For Each row As RepeaterItem In (From r As RepeaterItem In RPTcommunities.Items Where r.ItemType <> ListItemType.Header Select r).ToList()
            Dim oRadio As RadioButton = row.FindControl("RBselect")
            Dim oCheckBox As CheckBox = row.FindControl("CBXselect")
            Dim oLiteral As Literal = row.FindControl("LTidCommunity")
            Dim oLabel As Label = row.FindControl("LBcommunityName")
            Dim name As String = oLabel.Text
            If isMultiple Then
                result(oCheckBox.Checked).Add(CInt(oLiteral.Text))
            Else
                result(oRadio.Checked).Add(CInt(oLiteral.Text))
            End If
        Next
        Return result
    End Function

    Protected Friend Function GetSubmittedFilters() As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewSearchCommunities.GetSubmittedFilters
        Dim filter As New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        filter.Availability = CurrentAvailability
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
                        If Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
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
#End Region

#Region "Filter use"
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        CurrentPresenter.LoadCommunities(IdProfile, SearchFilters, ExcludeCommunities, Pager.PageIndex, Pager.PageSize)
    End Sub
    Private Sub DDLpage_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        CurrentPresenter.LoadCommunities(IdProfile, SearchFilters, ExcludeCommunities, Pager.PageIndex, DDLpage.SelectedValue)
    End Sub
    Private Sub LNBapplySearchFilters_Click(sender As Object, e As System.EventArgs) Handles LNBapplySearchFilters.Click
        CurrentPresenter.LoadCommunities(IdProfile, GetSubmittedFilters(), ExcludeCommunities, 0, CurrentPageSize)
    End Sub
#End Region

#Region "Loading communities"

    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem = DirectCast(e.Item.DataItem, lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem)
                Dim oRadio As RadioButton = e.Item.FindControl("RBselect")
                oRadio.Visible = Not MultipleSelection
                Dim oCheckBox As CheckBox = e.Item.FindControl("CBXselect")
                oCheckBox.Visible = MultipleSelection
                If MultipleSelection Then
                    oCheckBox.Checked = SelectedIdCommunities.Contains(item.Community.Id)
                ElseIf SelectedIdCommunities.Count = 1 Then
                    oRadio.Checked = SelectedIdCommunities.Contains(item.Community.Id)
                End If
                Dim oLabel As Label = e.Item.FindControl("LBcommunityName")

                'If Not IsNothing(item.Community.Path) Then
                '    If item.Paths.Count = 1 Then
                '        oLabel.ToolTip = String.Join(" / ", item.Paths(0).FathersName.ToArray())
                '    Else
                '        'Dim s As String = ""
                '        'For Each o As dtoCommunityPlainPath In item.Paths
                '        '    s &= String.Join("/", o.FathersName.ToArray()) & vbCrLf
                '        'Next
                '        oLabel.ToolTip = String.Join(" / ", item.Paths.Where(Function(p) p.isPrimary).FirstOrDefault().FathersName.ToArray())
                '    End If
                'End If
                'oLabel = e.Item.FindControl("LBcommunityType")
                'oLabel.Text = item.Community.CommunityType '.TranslatedCommunityTypes(item.IdCommunityType)

                Dim oControl As HtmlControl = e.Item.FindControl("DVtag")
                oControl.Visible = item.Community.Tags.Any()
                If item.Community.Tags.Any() Then
                    oLabel = e.Item.FindControl("LBcommunityTagsTitle")
                    Resource.setLabel(oLabel)
                End If
                If item.Community.IdType = lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization Then
                    oLabel = e.Item.FindControl("LBtagCommunityType")
                    oLabel.Visible = True
                    oLabel.Text = Resource.getValue("LBtagCommunityType." & lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization.ToString)
                End If
            Case ListItemType.Header
                Dim oLabel As Label = e.Item.FindControl("LBthcommunityName")
                Resource.setLabel(oLabel)
                oLabel = e.Item.FindControl("LBthcommunitySelect")
                Resource.setLabel(oLabel)
                oLabel = e.Item.FindControl("LBthcommunityType")
                Resource.setLabel(oLabel)
            Case ListItemType.Footer
                If (RPTcommunities.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")

                    If _noItemsToDisplay = True Then
                        _noItemsToDisplay = False
                        oLabel.Text = Resource.getValue("NoCommunities.AddCommunitites.NoItems")
                    Else
                        oLabel.Text = Resource.getValue("NoCommunities.AddCommunitites")
                    End If
                End If
        End Select
    End Sub
#End Region

End Class