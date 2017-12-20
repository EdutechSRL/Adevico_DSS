Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Public Class UC_SelectUsers
    Inherits BaseControl
    Implements IViewSelectUsers


#Region "Context"
    Private _Presenter As SelectUsersPresenter
    Private ReadOnly Property CurrentPresenter() As SelectUsersPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SelectUsersPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Common"
    Public Property isInitialized As Boolean Implements IViewSelectUsers.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property SelectionMode As UserSelectionType Implements IViewSelectUsers.SelectionMode
        Get
            Return ViewStateOrDefault("SelectionMode", UserSelectionType.CommunityUsers)
        End Get
        Set(value As UserSelectionType)
            ViewState("SelectionMode") = value
        End Set
    End Property
    Private Property OrderAscending As Boolean Implements IViewSelectUsers.OrderAscending
        Get
            Return ViewStateOrDefault("OrderAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("OrderAscending") = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewSelectUsers.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseUserChangedEvent As Boolean Implements IViewSelectUsers.RaiseUserChangedEvent
        Get
            Return ViewStateOrDefault("RaiseUserChangedEvent", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseUserChangedEvent") = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewSelectUsers.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Private ReadOnly Property AllowSearchByTaxCode As Boolean Implements IViewSelectUsers.AllowSearchByTaxCode
        Get
            Return Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        End Get
    End Property
    Public Property AllowSearchByAgency As Boolean Implements IViewSelectUsers.AllowSearchByAgency
        Get
            Return ViewStateOrDefault("AllowSearchByAgency", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSearchByAgency") = value
        End Set
    End Property
    Public Property MultipleSelection As Boolean Implements IViewSelectUsers.MultipleSelection
        Get
            Return ViewStateOrDefault("MultipleSelection", True)
        End Get
        Set(value As Boolean)
            ViewState("MultipleSelection") = value
            BTNselect.Visible = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewSelectUsers.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = DefaultPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)

            DVpageSizeBottom.Visible = AllowSelectPageSize AndAlso SelectPageSizeOnBottom
            DVpageSizeTop.Visible = AllowSelectPageSize AndAlso Not SelectPageSizeOnBottom
            DVpagerTop.Visible = AllowSelectPageSize AndAlso Not SelectPageSizeOnBottom

            If AllowSelectPageSize AndAlso Not PageSizeSet Then
                DefaultPageSize = 50
            End If
            '  Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Public Property HasAvailableUsers As Boolean Implements IViewSelectUsers.HasAvailableUsers
        Get
            Return ViewStateOrDefault("HasAvailableUsers", False)
        End Get
        Set(value As Boolean)
            ViewState("HasAvailableUsers") = value
        End Set
    End Property
    Private Property SelectedIdUsers As List(Of Integer) Implements IViewSelectUsers.SelectedIdUsers
        Get
            Return ViewStateOrDefault("SelectedIdUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("SelectedIdUsers") = value
        End Set
    End Property
    Private Property AvailableColumns As List(Of ProfileColumn) Implements IViewSelectUsers.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of ProfileColumn))
        End Get
        Set(value As List(Of ProfileColumn))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Private Property CurrentStartWith As String Implements IViewSelectUsers.CurrentStartWith
        Get
            Return CTRLalphabetSelector.SelectedItem
        End Get
        Set(value As String)
            CTRLalphabetSelector.SelectedItem = value
        End Set
    End Property
    Private Property CurrentValue As String Implements IViewSelectUsers.CurrentValue
        Get
            Dim v As String = ""
            If SelectionMode = UserSelectionType.SystemUsers Then
                v = TXBprofileValue.Text
            Else
                v = TXBsubscriptionValue.Text
            End If
            If Not String.IsNullOrEmpty(v) Then
                v = v.Trim
            End If
            Return v
        End Get
        Set(value As String)
            If SelectionMode = UserSelectionType.SystemUsers Then
                TXBprofileValue.Text = value
            Else
                TXBsubscriptionValue.Text = value
            End If
        End Set
    End Property
    Public Property DefaultPageSize As Integer Implements IViewSelectUsers.DefaultPageSize
        Get
            Return ViewStateOrDefault("DefaultPageSize", 50)
        End Get
        Set(value As Integer)
            ViewState("DefaultPageSize") = value
            If Not PageSizeSet Then
                InitializePageSizeSelector(DDLpageSizeTop, value)
                InitializePageSizeSelector(DDLpageSizeBottom, value)
                PageSizeSet = True
            End If
        End Set
    End Property
    Public ReadOnly Property CurrentPageSize As Integer Implements IViewSelectUsers.CurrentPageSize
        Get
            Dim size As Integer = DefaultPageSize
            Select Case SelectPageSizeOnBottom
                Case True
                    If DDLpageSizeBottom.SelectedIndex > -1 Then
                        size = CInt(DDLpageSizeBottom.SelectedValue)
                    End If
                Case Else
                    If DDLpageSizeTop.SelectedIndex > -1 Then
                        size = CInt(DDLpageSizeTop.SelectedValue)
                    End If
            End Select
            Return size
        End Get
    End Property


    Private Property SelectedIdAgency As Long Implements IViewSelectUsers.SelectedIdAgency
        Get
            Dim result As Long = 0
            Select Case SelectionMode
                Case UserSelectionType.SystemUsers
                    If (Me.DDLprofileAgencies.SelectedIndex > -1) Then
                        result = CLng(Me.DDLprofileAgencies.SelectedValue)
                    End If
                Case UserSelectionType.CommunityUsers
                    If (Me.DDLcommunityAgencies.SelectedIndex > -1) Then
                        result = Me.DDLcommunityAgencies.SelectedValue
                    End If
            End Select
            Return result
        End Get
        Set(value As Long)
            Select Case SelectionMode
                Case UserSelectionType.SystemUsers
                    If Not IsNothing(Me.DDLprofileAgencies.Items.FindByValue(value)) Then
                        Me.DDLprofileAgencies.SelectedValue = value
                    End If
                Case UserSelectionType.CommunityUsers
                    If Not IsNothing(Me.DDLcommunityAgencies.Items.FindByValue(value)) Then
                        Me.DDLcommunityAgencies.SelectedValue = value
                    End If
            End Select
        End Set
    End Property
    Private Property SelectedSearchBy As SearchProfilesBy Implements IViewSelectUsers.SelectedSearchBy
        Get
            Dim result As SearchProfilesBy = SearchProfilesBy.All
            Select Case SelectionMode
                Case UserSelectionType.SystemUsers
                    If (Me.DDLsearchProfileBy.SelectedIndex > -1) Then
                        result = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchProfilesBy).GetByString(Me.DDLsearchProfileBy.SelectedValue, SearchProfilesBy.All)
                    End If
                Case UserSelectionType.CommunityUsers
                    If (Me.DDLsearchSubscriptionsBy.SelectedIndex > -1) Then
                        result = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchProfilesBy).GetByString(Me.DDLsearchSubscriptionsBy.SelectedValue, SearchProfilesBy.All)
                    End If
            End Select
            Return result
        End Get
        Set(value As SearchProfilesBy)
            Select Case SelectionMode
                Case UserSelectionType.SystemUsers
                    If Not IsNothing(Me.DDLsearchProfileBy.Items.FindByValue(value.ToString)) Then
                        Me.DDLsearchProfileBy.SelectedValue = value.ToString
                    End If
                Case UserSelectionType.CommunityUsers
                    If Not IsNothing(Me.DDLsearchProfileBy.Items.FindByValue(value.ToString)) Then
                        Me.DDLsearchProfileBy.SelectedValue = value.ToString
                    End If
            End Select
        End Set
    End Property

    Private Property SelectedIdProfileType As Integer Implements IViewSelectUsers.SelectedIdProfileType
        Get
            Dim result As Integer = 0
            Select Case SelectionMode
                Case UserSelectionType.SystemUsers
                    If (Me.DDLprofileType.SelectedIndex > -1) Then
                        result = Me.DDLprofileType.SelectedValue
                    End If
                Case UserSelectionType.CommunityUsers
                    If (Me.DDLcommunityProfileType.SelectedIndex > -1) Then
                        result = Me.DDLcommunityProfileType.SelectedValue
                    End If
            End Select
            Return result
        End Get
        Set(value As Integer)
            Select Case SelectionMode
                Case UserSelectionType.SystemUsers
                    If Not IsNothing(Me.DDLprofileType.Items.FindByValue(value)) Then
                        Me.DDLprofileType.SelectedValue = value
                    End If
                Case UserSelectionType.CommunityUsers
                    If Not IsNothing(Me.DDLcommunityProfileType.Items.FindByValue(value)) Then
                        Me.DDLcommunityProfileType.SelectedValue = value
                    End If
            End Select
        End Set
    End Property
    Private Property FromAllMyCommunity As Boolean Implements IViewSelectUsers.FromAllMyCommunity
        Get
            Return ViewStateOrDefault("FromAllMyCommunity", False)
        End Get
        Set(value As Boolean)
            ViewState("FromAllMyCommunity") = value
        End Set
    End Property
    Private Property FromCommunities As List(Of Integer) Implements IViewSelectUsers.FromCommunities
        Get
            Return ViewStateOrDefault("FromCommunities", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("FromCommunities") = value
        End Set
    End Property
    Private Property UnavailableIdUsers As List(Of Integer) Implements IViewSelectUsers.UnavailableIdUsers
        Get
            Return ViewStateOrDefault("UnavailableIdUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("UnavailableIdUsers") = value
        End Set
    End Property
    Private Property SelectAllUsers As Boolean Implements IViewSelectUsers.SelectAllUsers
        Get
            Return ViewStateOrDefault("SelectAllUsers", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("SelectAllUsers") = value
        End Set
    End Property

    Public Property ShowSubscriptionsProfileTypeColumn As Boolean Implements IViewSelectUsers.ShowSubscriptionsProfileTypeColumn
        Get
            Return ViewStateOrDefault("ShowSubscriptionsProfileTypeColumn", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowSubscriptionsProfileTypeColumn") = value
        End Set
    End Property
    Public Property ShowSubscriptionsFilterByProfile As Boolean Implements IViewSelectUsers.ShowSubscriptionsFilterByProfile
        Get
            Return ViewStateOrDefault("ShowSubscriptionsFilterByProfile", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowSubscriptionsFilterByProfile") = value
        End Set
    End Property

    Private Property IsFirstLoad As Boolean Implements IViewSelectUsers.IsFirstLoad
        Get
            Return ViewStateOrDefault("IsFirstLoad", True)
        End Get
        Set(value As Boolean)
            ViewState("IsFirstLoad") = value
        End Set
    End Property
    Private Property SystemMaxGridItems As Integer Implements IViewSelectUsers.SystemMaxGridItems
        Get
            Return ViewStateOrDefault("SystemMaxGridItems", 5000)
        End Get
        Set(value As Integer)
            ViewState("SystemMaxGridItems") = value
        End Set
    End Property

#Region "Preview"
    Private Property TemporaryIdUsers As List(Of Integer) Implements IViewSelectUsers.TemporaryIdUsers
        Get
            Return ViewStateOrDefault("TemporaryIdUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("TemporaryIdUsers") = value
        End Set
    End Property
    Public Property AllowSelectAllFromPreview As Boolean Implements IViewSelectUsers.AllowSelectAllFromPreview
        Get
            Return ViewStateOrDefault("AllowSelectAllFromPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelectAllFromPreview") = value
        End Set
    End Property
    Public Property DefaultMaxPreviewItems As Integer Implements IViewSelectUsers.DefaultMaxPreviewItems
        Get
            Return ViewStateOrDefault("DefaultMaxPreviewItems", 200)
        End Get
        Set(value As Integer)
            ViewState("DefaultMaxPreviewItems") = value
        End Set
    End Property
    Public Property ShowItemsExceeding As Boolean Implements IViewSelectUsers.ShowItemsExceeding
        Get
            Return ViewStateOrDefault("ShowItemsExceeding", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowItemsExceeding") = value
        End Set
    End Property
    Private Property TemporaryItemsCount As Integer Implements IViewSelectUsers.TemporaryItemsCount
        Get
            Return ViewStateOrDefault("TemporaryItemsCount", 0)
        End Get
        Set(value As Integer)
            ViewState("TemporaryItemsCount") = value
        End Set
    End Property
    Private Property IsFirstPreviewLoad As Boolean Implements IViewSelectUsers.IsFirstPreviewLoad
        Get
            Return ViewStateOrDefault("IsFirstPreviewLoad", True)
        End Get
        Set(value As Boolean)
            ViewState("IsFirstPreviewLoad") = value
        End Set
    End Property
    Private Property PreviewPager As PagerBase Implements IViewSelectUsers.PreviewPager
        Get
            Dim pagesize As Integer = DefaultMaxPreviewItems
            If pagesize = 0 Then
                pagesize = SystemMaxGridItems
            End If
            Return ViewStateOrDefault("PreviewPager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = pagesize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("PreviewPager") = value
            Me.PGgridPreview.Pager = value
            Me.DVpagerPreview.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            '  Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Private Property SearchPreviewLetter As String Implements IViewSelectUsers.SearchPreviewLetter
        Get
            Return CTRLpreviewAlphabetSelector.SelectedItem
        End Get
        Set(value As String)
            CTRLpreviewAlphabetSelector.SelectedItem = value
        End Set
    End Property
    Private Property SearchPreviewValue As String Implements IViewSelectUsers.SearchPreviewValue
        Get
            Return ViewStateOrDefault("SearchPreviewValue", "")
        End Get
        Set(value As String)
            ViewState("SearchPreviewValue") = value
        End Set
    End Property
#End Region

#End Region

#Region "Profile Filters"
    Private Property SelectedProfileStatus As StatusProfile Implements IViewSelectUsers.SelectedProfileStatus
        Get
            Dim result As StatusProfile = StatusProfile.AllStatus
            If (Me.DDLprofileStatus.SelectedIndex > -1) Then
                result = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatusProfile).GetByString(Me.DDLprofileStatus.SelectedValue, StatusProfile.AllStatus)
            End If
            Return result
        End Get
        Set(value As StatusProfile)
            If Not IsNothing(Me.DDLprofileStatus.Items.FindByValue(value.ToString)) Then
                Me.DDLprofileStatus.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private ReadOnly Property GetTranslatedProfileTypes As List(Of TranslatedItem(Of Integer)) Implements IViewSelectUsers.GetTranslatedProfileTypes
        Get
            Return (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID)
                    Select New TranslatedItem(Of Integer) With {.Id = o.ID, .Translation = o.Descrizione}).ToList
        End Get
    End Property
    Private Property AvailableOrganizations As List(Of Integer) Implements IViewSelectUsers.AvailableOrganizations
        Get
            Return ViewStateOrDefault("AvailableOrganizations", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("AvailableOrganizations") = value
        End Set
    End Property
    Private ReadOnly Property GetCurrentProfileFilters As dtoFilters Implements IViewSelectUsers.GetCurrentProfileFilters
        Get
            Dim dto As New dtoFilters

            With dto
                .Ascending = OrderAscending
                .OrderBy = OrderProfilesBy
                .IdAvailableOrganization = AvailableOrganizations
                .IdOrganization = SelectedIdOrganization
                .idProvider = SelectedIdProvider
                .PageIndex = Me.Pager.PageIndex
                .PageSize = CurrentPageSize
                .IdProfileType = SelectedIdProfileType
                If SelectedIdProfileType = UserTypeStandard.Employee Then
                    .IdAgency = SelectedIdAgency
                Else
                    .IdAgency = -1
                End If
                .SearchBy = SelectedSearchBy
                .StartWith = CurrentStartWith
                .Status = SelectedProfileStatus
                .Value = CurrentValue
                .DisplayLoginInfo = False
            End With
            Return dto
        End Get
    End Property
    Private Property SelectedIdOrganization As Integer Implements IViewSelectUsers.SelectedIdOrganization
        Get
            If (Me.DDLorganizations.SelectedIndex > -1) Then
                Return Me.DDLorganizations.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLorganizations.Items.FindByValue(value)) Then
                Me.DDLorganizations.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedIdProvider As Long Implements IViewSelectUsers.SelectedIdProvider
        Get
            If (Me.DDLauthenticationType.SelectedIndex > -1) Then
                Return Me.DDLauthenticationType.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Long)
            If Not IsNothing(Me.DDLauthenticationType.Items.FindByValue(value)) Then
                Me.DDLauthenticationType.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SearchProfileFilters As dtoFilters Implements IViewSelectUsers.SearchProfileFilters
        Get
            If IsFirstLoad Then
                Dim filter As dtoFilters = GetCurrentProfileFilters
                ViewState("SearchFilters") = filter
                Return filter
            Else
                Return ViewStateOrDefault("SearchFilters", GetCurrentProfileFilters)
            End If
        End Get
        Set(value As dtoFilters)
            ViewState("SearchFilters") = value
        End Set
    End Property
    Private Property OrderProfilesBy As OrderProfilesBy Implements IViewSelectUsers.OrderProfilesBy
        Get
            Return ViewStateOrDefault("OrderProfilesBy", OrderProfilesBy.SurnameAndName)
        End Get
        Set(value As OrderProfilesBy)
            ViewState("OrderProfilesBy") = value
        End Set
    End Property
#End Region

#Region "User filters"
    Private Property SelectedSubscriptionStatus As SubscriptionStatus Implements IViewSelectUsers.SelectedSubscriptionStatus
        Get
            Dim result As SubscriptionStatus = SubscriptionStatus.all
            If (Me.DDLsubscriptionStatus.SelectedIndex > -1) Then
                result = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubscriptionStatus).GetByString(Me.DDLsubscriptionStatus.SelectedValue, SubscriptionStatus.all)
            End If
            Return result
        End Get
        Set(value As SubscriptionStatus)
            If Not IsNothing(Me.DDLsubscriptionStatus.Items.FindByValue(value.ToString)) Then
                Me.DDLsubscriptionStatus.SelectedValue = value.ToString
            End If
        End Set
    End Property

    Private ReadOnly Property GetTranslatedRoles As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) Implements IViewSelectUsers.GetTranslatedRoles
        Get
            Return COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).Select(Function(r) New TranslatedItem(Of Integer) With {.Id = r.ID, .Translation = r.Name}).ToList()
        End Get
    End Property
    Private Property SelectedIdRole As Integer Implements IViewSelectUsers.SelectedIdRole
        Get
            Dim result As Integer = -1
            If (Me.DDLcommunityRole.SelectedIndex > -1) Then
                result = Me.DDLcommunityRole.SelectedValue
            End If

            Return result
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLcommunityRole.Items.FindByValue(value)) Then
                Me.DDLcommunityRole.SelectedValue = value
            End If
        End Set
    End Property
    Private ReadOnly Property GetCurrentUserFilters As dtoUserFilters Implements IViewSelectUsers.GetCurrentUserFilters
        Get
            Dim dto As New dtoUserFilters

            With dto
                .Ascending = OrderAscending
                .OrderBy = OrderProfilesBy
                .IdRole = SelectedIdRole
                .PageIndex = Me.Pager.PageIndex
                .PageSize = CurrentPageSize
                .IdProfileType = SelectedIdProfileType
                .IdCommunities = FromCommunities
                If SelectedIdProfileType = UserTypeStandard.Employee OrElse (Me.DDLcommunityAgencies.Visible AndAlso Not ShowSubscriptionsFilterByProfile) Then
                    .IdAgency = SelectedIdAgency
                Else
                    .IdAgency = -1
                End If
                .SearchBy = SelectedSearchBy
                .StartWith = CurrentStartWith
                .Status = SelectedSubscriptionStatus
                .Value = CurrentValue
            End With
            Return dto
        End Get
    End Property
    Private Property SearchUserFilters As dtoUserFilters Implements IViewSelectUsers.SearchUserFilters
        Get
            If IsFirstLoad Then
                Dim filter As dtoUserFilters = GetCurrentUserFilters
                ViewState("SearchUserFilters") = filter
                Return filter
            Else
                Return ViewStateOrDefault("SearchUserFilters", GetCurrentUserFilters)
            End If

        End Get
        Set(value As dtoUserFilters)
            ViewState("SearchUserFilters") = value
        End Set
    End Property

    Private Property OrderUsersBy As OrderUsersBy Implements IViewSelectUsers.OrderUsersBy
        Get
            Return ViewStateOrDefault("OrderUsersBy", OrderUsersBy.SurnameAndName)
        End Get
        Set(value As OrderUsersBy)
            ViewState("OrderUsersBy") = value
        End Set
    End Property
#End Region

    Public Property DisplayHeaderSelectAll As Boolean Implements IViewSelectUsers.DisplayHeaderSelectAll
        Get
            Return ViewStateOrDefault("DisplayHeaderSelectAll", True)
        End Get
        Set(value As Boolean)
            ViewState("DisplayHeaderSelectAll") = value
        End Set
    End Property

#End Region

#Region "Property"
    Public Property InModalWindow As Boolean
        Get
            Return ViewStateOrDefault("InModalWindow", True)
        End Get
        Set(value As Boolean)
            ViewState("InModalWindow") = value
            DVcommands.Visible = value
        End Set
    End Property
    Public ReadOnly Property isColumnVisible(ByVal column As ProfileColumn) As Boolean
        Get
            Return AvailableColumns.Contains(column)
        End Get
    End Property
    Public ReadOnly Property BackGroundItem(ByVal status As StatusProfile) As String
        Get
            If status = StatusProfile.Active Then
                Return "ROW_Normal_Small"
            ElseIf status = StatusProfile.Disabled Then
                Return "ROW_Disabilitate_Small"
            Else
                Return "ROW_Alternate_Small"
            End If
        End Get
    End Property
    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Me.Resource.getValue("OnLoadingTranslation")
        End Get
    End Property
    Public ReadOnly Property GetApplyFilterButton As Button
        Get
            Return BTNapplyFilters
        End Get
    End Property
    Public ReadOnly Property GetApplyFilterButtonUniqueID As String
        Get
            Return Me.BTNapplyFilters.UniqueID
        End Get
    End Property
    Public ReadOnly Property GetDefaultTextField As String
        Get
            If Me.DVselectors.Visible Then
                Select Case SelectionMode
                    Case UserSelectionType.SystemUsers
                        Return TXBprofileValue.UniqueID
                    Case UserSelectionType.CommunityUsers
                        Return TXBsubscriptionValue.UniqueID
                End Select
            Else
                Return TXBsearchInPreview.UniqueID
            End If
        End Get
    End Property

    Public Event CloseWindow()
    Public Event UsersSelected(ByVal idUsers As List(Of Integer))
    Public Event UserSelected(ByVal idUser As Integer)
    Public Property AllowSelectPageSize As Boolean
        Get
            Return ViewStateOrDefault("AllowSelectPageSize", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelectPageSize") = value
        End Set
    End Property
    Public Property SelectPageSizeOnBottom As Boolean
        Get
            Return ViewStateOrDefault("SelectPageSizeOnBottom", True)
        End Get
        Set(value As Boolean)
            ViewState("SelectPageSizeOnBottom") = value
        End Set
    End Property
    Private Property PageSizeSet As Boolean
        Get
            Return ViewStateOrDefault("PageSizeSet", False)
        End Get
        Set(value As Boolean)
            ViewState("PageSizeSet") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Pager
        Me.PGgridPreview.Pager = PreviewPager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesManagement", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNapplyFilters, True)
            .setButton(BTNcancel, True)
            .setButton(BTNselect, True)
            .setButton(BTNreviewSelection, True)

            .setLabel(LBprofileOrganizationFilter_t)
            .setLabel(LBprofileTypeFilter_t)
            .setLabel(LBprofileStatusFilter_t)

            .setLabel(LBprofileAuthenticationTypeFilter_t)
            .setLabel(LBsearchProfileByFilter_t)
            .setLabel(LBprofileAgencyFilter_t)

            .setLabel(LBsearchSubscriptionsByFilter_t)
            .setLabel(LBsubscriptionStatusFilter_t)
            .setLabel(LBcommunityRoleFilter_t)
            .setLabel(LBcommunityProfileTypeFilter_t)
            .setLabel(LBcommunityAgencyFilter_t)

            .setButton(BTNapplyPreviewFilters, True)
            .setLabel(LBpreviewDescription)

            .setButton(BTNcloseUserSelectionPreview, True)
            .setButton(BTNconfirmSelectionEdit, True)
            .setButton(BTNconfirmSelectionEditAndInsert, True)
            .setLabel(LBrecordsForPage_t)
            LBrecordsForPageTop_t.Text = LBrecordsForPage_t.Text
        End With
    End Sub
#End Region

#Region "Implements"
    Private Sub DisplaySessionTimeout() Implements IViewSelectUsers.DisplaySessionTimeout
        MLVusers.SetActiveView(VIWsessionTimeout)
    End Sub
    Private Sub NoPermission() Implements IViewSelectUsers.NoPermission
        MLVusers.SetActiveView(VIWnoPermission)
    End Sub
    Private Function GetCurrentSelectedItems() As List(Of dtoSelectItem(Of Integer)) Implements IViewSelectUsers.GetCurrentSelectedItems
        Dim items As New List(Of dtoSelectItem(Of Integer))

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTprofiles.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXuser")
            Dim oLiteral As Literal = row.FindControl("LTidProfile")
            items.Add(New dtoSelectItem(Of Integer)() With {.Id = CLng(oLiteral.Text), .Selected = oCheck.Checked})
        Next
        Return items
    End Function
    Private Function GetTemporarySelectedItems() As List(Of dtoSelectItem(Of Integer)) Implements IViewSelectUsers.GetTemporarySelectedItems
        Dim items As New List(Of dtoSelectItem(Of Integer))

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTpreviewItems.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXuser")
            Dim oLiteral As Literal = row.FindControl("LTidProfile")
            items.Add(New dtoSelectItem(Of Integer)() With {.Id = CLng(oLiteral.Text), .Selected = oCheck.Checked})
        Next
        Return items
    End Function

    ''' <summary>
    ''' Inizializzazione dello user control per le modalità più estese, ossia gli utenti di sistema o gli utenti delle comunità che gestisco
    ''' </summary>
    ''' <param name="mode">Per trovare gli utenti a livello di piattaforma o di comunità</param>
    ''' <param name="multipleSelection">Indica se sono consentite o meno selezioni multiple</param>
    ''' <param name="fromAllMyCommunity">Indica se gli utenti vanno cercati fra tutti quelli presenti nelle comunità che posso gestire</param>
    ''' <param name="unloadIdUsers">Indica la lista degli utenti da escludere dalla ricerca</param>
    ''' <param name="selectIdUsers">Indica gli utenti che devono essere preselezionati nella lista</param>
    ''' <param name="description">Indica una descrizione da visualizzare sopra i filtri di ricerca, se non specificata il campo description non viene visualizzato</param>
    ''' <remarks></remarks>
    Public Sub InitializeControl(mode As UserSelectionType, multipleSelection As Boolean, fromAllMyCommunity As Boolean, Optional unloadIdUsers As List(Of Integer) = Nothing, Optional selectIdUsers As List(Of Integer) = Nothing, Optional description As String = "") Implements IViewSelectUsers.InitializeControl
        InitializeControl(mode, multipleSelection, description)
        Me.CurrentPresenter.InitView(mode, fromAllMyCommunity, unloadIdUsers, selectIdUsers)
    End Sub

    ''' <summary>
    ''' Inizializzazione dello user control 
    ''' </summary>
    ''' <param name="mode">Per trovare gli utenti a livello di piattaforma o di comunità</param>
    ''' <param name="multipleSelection">Indica se sono consentite o meno selezioni multiple</param>
    ''' <param name="idCommunity">Indica l'id della comunità da cui recuperare gli utenti</param>
    ''' <param name="unloadIdUsers">Indica la lista degli utenti da escludere dalla ricerca</param>
    ''' <param name="selectIdUsers">Indica gli utenti che devono essere preselezionati nella lista</param>
    ''' <param name="description">Indica una descrizione da visualizzare sopra i filtri di ricerca, se non specificata il campo description non viene visualizzato</param>
    ''' <remarks></remarks>
    Public Sub InitializeControl(mode As UserSelectionType, multipleSelection As Boolean, idCommunity As Integer, Optional unloadIdUsers As List(Of Integer) = Nothing, Optional selectIdUsers As List(Of Integer) = Nothing, Optional description As String = "") Implements IViewSelectUsers.InitializeControl
        InitializeControl(mode, multipleSelection, description)
        Me.CurrentPresenter.InitView(mode, FromAllMyCommunity, idCommunity, unloadIdUsers, selectIdUsers)
    End Sub
    ''' <summary>
    ''' Inizializzazione dello user control 
    ''' </summary>-
    ''' <param name="mode">Per trovare gli utenti a livello di piattaforma o di comunità</param>
    ''' <param name="multipleSelection">Indica se sono consentite o meno selezioni multiple</param>
    ''' <param name="idCommunities">Lista di id di comunità da cui recuperare gli utenti</param>
    ''' <param name="unloadIdUsers">Indica la lista degli utenti da escludere dalla ricerca</param>
    ''' <param name="selectIdUsers">Indica gli utenti che devono essere preselezionati nella lista</param>
    ''' <param name="description">Indica una descrizione da visualizzare sopra i filtri di ricerca, se non specificata il campo description non viene visualizzato</param>
    ''' <remarks></remarks>
    Public Sub InitializeControl(mode As UserSelectionType, multipleSelection As Boolean, idCommunities As List(Of Integer), Optional unloadIdUsers As List(Of Integer) = Nothing, Optional selectIdUsers As List(Of Integer) = Nothing, Optional description As String = "") Implements IViewSelectUsers.InitializeControl
        InitializeControl(mode, multipleSelection, description)
        Me.CurrentPresenter.InitView(mode, FromAllMyCommunity, idCommunities, unloadIdUsers, selectIdUsers)
    End Sub
    Public Sub InitializeControlForSingleSelection(mode As UserSelectionType, fromAllMyCommunity As Boolean, Optional unloadIdUsers As List(Of Integer) = Nothing, Optional selectedIdUser As Integer = 0, Optional description As String = "") Implements lm.Comol.Core.BaseModules.ProfileManagement.Presentation.IViewSelectUsers.InitializeControlForSingleSelection
        InitializeControl(mode, False, description)
        Dim selectIdUsers As New List(Of Integer)
        If selectedIdUser > 0 Then
            selectIdUsers.Add(selectedIdUser)
        End If
        Me.CurrentPresenter.InitView(mode, fromAllMyCommunity, unloadIdUsers, selectIdUsers)
    End Sub
    Public Sub InitializeControlForSingleSelection(mode As UserSelectionType, idCommunity As Integer, Optional unloadIdUsers As List(Of Integer) = Nothing, Optional selectedIdUser As Integer = 0, Optional description As String = "") Implements lm.Comol.Core.BaseModules.ProfileManagement.Presentation.IViewSelectUsers.InitializeControlForSingleSelection
        InitializeControl(mode, False, description)
        Dim selectIdUsers As New List(Of Integer)
        If selectedIdUser > 0 Then
            selectIdUsers.Add(selectedIdUser)
        End If
        Me.CurrentPresenter.InitView(mode, FromAllMyCommunity, idCommunity, unloadIdUsers, selectIdUsers)
    End Sub
    Public Sub InitializeControlForSingleSelection(mode As UserSelectionType, idCommunities As List(Of Integer), Optional unloadIdUsers As List(Of Integer) = Nothing, Optional selectedIdUser As Integer = 0, Optional description As String = "") Implements IViewSelectUsers.InitializeControlForSingleSelection
        InitializeControl(mode, False, description)
        Dim selectIdUsers As New List(Of Integer)
        If selectedIdUser > 0 Then
            selectIdUsers.Add(selectedIdUser)
        End If
        Me.CurrentPresenter.InitView(mode, FromAllMyCommunity, idCommunities, unloadIdUsers, selectIdUsers)
    End Sub

    Private Sub InitializeControl(mode As UserSelectionType, multipleSelection As Boolean, Optional description As String = "")
        DDLpageSizeTop.AutoPostBack = False
        BTNreviewSelection.Visible = False
        Me.SelectionMode = mode
        Me.MultipleSelection = multipleSelection
        Me.PGgrid.Visible = False
        Me.DVpagerPreview.Visible = False
        HideUsersPreview()
        If String.IsNullOrEmpty(description) Then
            DisplayDescription = False
        Else
            Me.LBdescription.Text = description
        End If
        Me.RPTprofiles.Visible = False
        Me.SelectedIdUsers = New List(Of Integer)
        Select Case mode
            Case UserSelectionType.CommunityUsers
                Me.MLVfilters.SetActiveView(VIWcommunityFilters)
            Case UserSelectionType.SystemUsers
                Me.MLVfilters.SetActiveView(VIWportalFilters)
            Case Else
                Me.MLVfilters.SetActiveView(VIWemptyFilters)
        End Select
    End Sub
    Private Sub SelectAllItems() Implements IViewSelectUsers.SelectAllItems
        CurrentPresenter.EditItemsSelection(True)
    End Sub
#Region "Filters"
#Region "Common"
    Private Sub LoadProfileTypes(idProfileTypes As List(Of Integer), IdDefaultProfileType As Integer) Implements IViewSelectUsers.LoadProfileTypes
        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest AndAlso (idProfileTypes.Contains(o.ID)) Select o).ToList
        Select Case SelectionMode
            Case UserSelectionType.SystemUsers
                Me.DDLprofileType.DataSource = oUserTypes
                Me.DDLprofileType.DataValueField = "ID"
                Me.DDLprofileType.DataTextField = "Descrizione"
                Me.DDLprofileType.DataBind()
                If oUserTypes.Count > 1 Then
                    Me.DDLprofileType.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLprofileType." & -1), -1))
                End If
                Me.SelectedIdProfileType = IdDefaultProfileType
            Case UserSelectionType.CommunityUsers
                Me.DDLcommunityProfileType.DataSource = oUserTypes
                Me.DDLcommunityProfileType.DataValueField = "ID"
                Me.DDLcommunityProfileType.DataTextField = "Descrizione"
                Me.DDLcommunityProfileType.DataBind()
                If oUserTypes.Count > 1 Then
                    Me.DDLcommunityProfileType.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLprofileType." & -1), -1))
                End If
                Me.SelectedIdProfileType = IdDefaultProfileType
                DVcommunityProfileFilter.Visible = ShowSubscriptionsFilterByProfile
        End Select

    End Sub

    Private Sub LoadAgencies(items As Dictionary(Of Long, String), idDefaultAgency As Long, mode As UserSelectionType) Implements IViewSelectUsers.LoadAgencies
        Select Case mode
            Case UserSelectionType.SystemUsers
                Me.DDLprofileAgencies.Items.Clear()
                Me.DDLprofileAgencies.DataSource = items
                Me.DDLprofileAgencies.DataValueField = "Key"
                Me.DDLprofileAgencies.DataTextField = "Value"
                Me.DDLprofileAgencies.DataBind()

                If items.Count > 1 OrElse items.Count = 0 Then
                    Me.DDLprofileAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLprofileAgencies." & -1), -1))
                End If
                Me.LBprofileAgencyFilter_t.Visible = True
                Me.DDLprofileAgencies.Visible = True
            Case UserSelectionType.CommunityUsers
                Me.DDLcommunityAgencies.Items.Clear()
                Me.DDLcommunityAgencies.DataSource = items
                Me.DDLcommunityAgencies.DataValueField = "Key"
                Me.DDLcommunityAgencies.DataTextField = "Value"
                Me.DDLcommunityAgencies.DataBind()

                If items.Count = 0 Then
                    Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.ignorefilter"), -1))
                Else
                    If items.Count > 1 Then
                        Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.all"), -2))
                    End If
                    Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.noemployeee"), -3))
                    Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.ignorefilter"), -1))
                End If
                Me.LBcommunityAgencyFilter_t.Visible = True
                Me.DDLcommunityAgencies.Visible = True
        End Select

        Me.SelectedIdAgency = idDefaultAgency
    End Sub
    Private Sub UnLoadAgencies() Implements IViewSelectUsers.UnLoadAgencies
        Select Case SelectionMode
            Case UserSelectionType.SystemUsers
                If Me.DDLprofileAgencies.SelectedIndex > -1 Then
                    Me.DDLprofileAgencies.SelectedIndex = 0
                End If
                Me.LBprofileAgencyFilter_t.Visible = False
                Me.DDLprofileAgencies.Visible = False

            Case UserSelectionType.CommunityUsers
                If Me.DDLcommunityAgencies.SelectedIndex > -1 Then
                    Me.DDLcommunityAgencies.SelectedIndex = 0
                End If
                Me.LBcommunityAgencyFilter_t.Visible = False
                Me.DDLcommunityAgencies.Visible = False
        End Select
    End Sub
    Private Sub LoadSearchProfilesBy(list As List(Of SearchProfilesBy), defaultSearch As SearchProfilesBy, mode As UserSelectionType) Implements IViewSelectUsers.LoadSearchProfilesBy
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SearchProfilesBy." & s.ToString)}).ToList

        Select Case mode
            Case UserSelectionType.SystemUsers
                Me.DDLsearchProfileBy.DataSource = translations
                Me.DDLsearchProfileBy.DataValueField = "Id"
                Me.DDLsearchProfileBy.DataTextField = "Translation"
                Me.DDLsearchProfileBy.DataBind()
            Case UserSelectionType.CommunityUsers
                Me.DDLsearchSubscriptionsBy.DataSource = translations
                Me.DDLsearchSubscriptionsBy.DataValueField = "Id"
                Me.DDLsearchSubscriptionsBy.DataTextField = "Translation"
                Me.DDLsearchSubscriptionsBy.DataBind()
        End Select
        Me.SelectedSearchBy = defaultSearch
    End Sub
    Private Sub InitializeWordSelector(words As List(Of String)) Implements IViewSelectUsers.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(words)
    End Sub
    Private Sub InitializeWordSelector(words As List(Of String), activeWord As String) Implements IViewSelectUsers.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(words, activeWord)
    End Sub
#End Region

#Region "Profiles"
    Private Sub LoadAvailableStatus(list As List(Of StatusProfile), defaultStatus As StatusProfile) Implements IViewSelectUsers.LoadAvailableStatus
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("StatusProfile." & s.ToString)}).ToList
        Me.DDLprofileStatus.DataSource = translations
        Me.DDLprofileStatus.DataValueField = "Id"
        Me.DDLprofileStatus.DataTextField = "Translation"
        Me.DDLprofileStatus.DataBind()
        Me.SelectedProfileStatus = defaultStatus
    End Sub
    Private Sub LoadAvailableOrganizations(items As List(Of Organization), idDefaultOrganization As Integer) Implements IViewSelectUsers.LoadAvailableOrganizations
        If items.Any Then
            AvailableOrganizations = items.Select(Function(i) i.Id).ToList()
        End If
        Me.DDLorganizations.DataSource = items
        Me.DDLorganizations.DataValueField = "Id"
        Me.DDLorganizations.DataTextField = "Name"
        Me.DDLorganizations.DataBind()
        If items.Count > 1 Then
            Me.DDLorganizations.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLorganizations." & -1), -1))
        End If
    End Sub

    Private Sub LoadAuthenticationProviders(providers As List(Of lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider), IdDefaultProvider As Long) Implements IViewSelectUsers.LoadAuthenticationProviders
        Dim translations As List(Of TranslatedItem(Of String)) = (From p In providers Select New TranslatedItem(Of String) With {.Id = p.IdProvider, .Translation = p.Translation.Name}).ToList

        Me.DDLauthenticationType.Items.Clear()
        Me.DDLauthenticationType.DataSource = translations
        Me.DDLauthenticationType.DataValueField = "Id"
        Me.DDLauthenticationType.DataTextField = "Translation"
        Me.DDLauthenticationType.DataBind()

        If translations.Count > 1 Then
            Me.DDLauthenticationType.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLauthenticationType." & -1), -1))
        End If
        Me.SelectedIdProvider = IdDefaultProvider
    End Sub
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))) Implements IViewSelectUsers.LoadProfiles
        Me.RPTprofiles.DataSource = items
        Me.RPTprofiles.DataBind()
        Me.RPTprofiles.Visible = True
        BTNreviewSelection.Visible = (items.Any OrElse SelectedIdUsers.Any()) AndAlso MultipleSelection
    End Sub
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))) Implements IViewSelectUsers.LoadProfiles
        Me.RPTprofiles.DataSource = items
        Me.RPTprofiles.DataBind()
        Me.RPTprofiles.Visible = True
        BTNreviewSelection.Visible = (items.Any OrElse SelectedIdUsers.Any()) AndAlso MultipleSelection
    End Sub
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))) Implements IViewSelectUsers.LoadProfiles
        Me.RPTprofiles.DataSource = items
        Me.RPTprofiles.DataBind()
        Me.RPTprofiles.Visible = True
        BTNreviewSelection.Visible = (items.Any OrElse SelectedIdUsers.Any()) AndAlso MultipleSelection
    End Sub
#End Region

#Region "Community"
    Private Sub LoadAvailableSubscriptionsStatus(list As List(Of SubscriptionStatus), defaultStatus As SubscriptionStatus) Implements IViewSelectUsers.LoadAvailableSubscriptionsStatus
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("StatusCommunitySubscription." & s.ToString)}).ToList
        Me.DDLsubscriptionStatus.DataSource = translations
        Me.DDLsubscriptionStatus.DataValueField = "Id"
        Me.DDLsubscriptionStatus.DataTextField = "Translation"
        Me.DDLsubscriptionStatus.DataBind()
        Me.SelectedSubscriptionStatus = defaultStatus
    End Sub
    Private Sub LoadRolesTypes(idRoles As List(Of Integer), idDefaultRole As Integer) Implements IViewSelectUsers.LoadRolesTypes
        Dim roles As List(Of TranslatedItem(Of Integer)) = GetTranslatedRoles.Where(Function(r) idRoles.Contains(r.Id)).ToList()

        Me.DDLcommunityRole.DataSource = roles
        Me.DDLcommunityRole.DataValueField = "ID"
        Me.DDLcommunityRole.DataTextField = "Translation"
        Me.DDLcommunityRole.DataBind()
        If idRoles.Count > 1 Then
            Me.DDLcommunityRole.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLprofileType." & -1), -1))
        End If
        Me.SelectedIdRole = idDefaultRole
    End Sub
    Private Sub LoadSubscriptions(items As List(Of dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))) Implements IViewSelectUsers.LoadSubscriptions
        Me.RPTprofiles.DataSource = items
        Me.RPTprofiles.DataBind()
        Me.RPTprofiles.Visible = True
        BTNreviewSelection.Visible = (items.Any OrElse SelectedIdUsers.Any()) AndAlso MultipleSelection
    End Sub
    Private Sub LoadSubscriptions(items As List(Of dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))) Implements IViewSelectUsers.LoadSubscriptions
        Me.RPTprofiles.DataSource = items
        Me.RPTprofiles.DataBind()
        Me.RPTprofiles.Visible = True
        BTNreviewSelection.Visible = (items.Any OrElse SelectedIdUsers.Any()) AndAlso MultipleSelection
    End Sub
    Private Sub LoadSubscriptions(items As List(Of dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))) Implements IViewSelectUsers.LoadSubscriptions
        Me.RPTprofiles.DataSource = items
        Me.RPTprofiles.DataBind()
        Me.RPTprofiles.Visible = True
        BTNreviewSelection.Visible = (items.Any OrElse SelectedIdUsers.Any()) AndAlso MultipleSelection
    End Sub
#End Region
#End Region

    Private Sub DisplayUsersPreview(items As List(Of lm.Comol.Core.Authentication.dtoBaseProfile), itemsCount As Integer) Implements IViewSelectUsers.DisplayUsersPreview
        Dim maxItems As Integer = DefaultMaxPreviewItems
        Me.DVselectors.Visible = False
        Me.DVpreview.Visible = True
        Me.RPTpreviewItems.DataSource = items
        Me.RPTpreviewItems.DataBind()
        Me.CTRLmessages.Visible = (maxItems > 0 AndAlso Not Me.AllowSelectAllFromPreview AndAlso maxItems < itemsCount)

        If (maxItems > 0 AndAlso Not Me.AllowSelectAllFromPreview AndAlso maxItems < itemsCount) Then
            If itemsCount = TemporaryItemsCount Then
                Me.CTRLmessages.InitializeControl(String.Format(Resource.getValue("MaxUsersPreview"), itemsCount, maxItems), MessageType.alert, Resource.getValue("buttonConfirmLoadAll"))
            Else
                Me.CTRLmessages.InitializeControl(String.Format(Resource.getValue("MaxUsersPreview.Filter"), items.Count, TemporaryItemsCount, maxItems), MessageType.alert, Resource.getValue("buttonConfirmLoadAll"))
            End If
        End If
    End Sub
    Private Sub DisplayStartUsersPreview(items As List(Of lm.Comol.Core.Authentication.dtoBaseProfile), availableWords As List(Of String), itemsCount As Integer) Implements IViewSelectUsers.DisplayStartUsersPreview
        Me.CTRLpreviewAlphabetSelector.InitializeControl(availableWords, "")
        DisplayUsersPreview(items, itemsCount)
    End Sub

    Private Sub HideUsersPreview() Implements IViewSelectUsers.HideUsersPreview
        Me.DVpreview.Visible = False
        Me.DVselectors.Visible = True
        Me.CTRLmessages.Visible = False
    End Sub
    Public Function GetSelectedUsers() As List(Of Integer) Implements IViewSelectUsers.GetSelectedUsers
        Return Me.CurrentPresenter.GetSelectedIdUsers()
    End Function
#End Region

#Region "Filters"
    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem
        Select Case SelectionMode
            Case UserSelectionType.SystemUsers
                Dim dto As dtoFilters = Me.SearchProfileFilters

                dto.PageIndex = 0
                dto.PageSize = CurrentPageSize
                dto.StartWith = letter
                Me.CurrentStartWith = letter
                Me.SearchProfileFilters = dto
                Me.CurrentPresenter.LoadProfiles(0, CurrentPageSize, True)
            Case UserSelectionType.CommunityUsers
                Dim dto As dtoUserFilters = Me.SearchUserFilters
                dto.PageIndex = 0
                dto.PageSize = CurrentPageSize
                dto.StartWith = letter
                Me.CurrentStartWith = letter
                Me.SearchUserFilters = dto
                Me.CurrentPresenter.LoadSubscriptions(0, CurrentPageSize, True)
        End Select
    End Sub

#Region "Profiles"
    Private Sub DDLorganizations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizations.SelectedIndexChanged
        Me.CurrentPresenter.ChangeOrganization(SelectedIdOrganization, SelectedIdProfileType, Me.Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub DDLprofileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLprofileType.SelectedIndexChanged
        Me.CurrentPresenter.ChangeProfileType(SelectedIdOrganization, SelectedIdProfileType, Me.Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub DDLstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLprofileStatus.SelectedIndexChanged
        Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, CurrentPageSize, True)
    End Sub
    Private Sub DDLagencies_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLprofileAgencies.SelectedIndexChanged
        Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, CurrentPageSize, True)
    End Sub
    Private Sub DDLauthenticationType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLauthenticationType.SelectedIndexChanged
        Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, CurrentPageSize, True)
    End Sub
    Private Sub RPTprofiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTprofiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBuserSurnameHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserNameHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserMailHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserCompanyNameHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserAgencyNameHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserProfileTypeHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserProfileStatusHeader_t")
            Me.Resource.setLabel(oLabel)

            Dim oTableCell As HtmlTableCell = e.Item.FindControl("THmultiSelect")
            If MultipleSelection Then
                oTableCell.Visible = True
                Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNheaderSelectAll")
                If DisplayHeaderSelectAll Then
                    oSpan.Visible = True
                    Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBselectProfileItemsIntoAllPages")
                    Resource.setLinkButton(oLinkbutton, True, True)

                    oLinkbutton = e.Item.FindControl("LNBunselectProfileItemsIntoAllPages")
                    Resource.setLinkButton(oLinkbutton, True, True)

                    Dim oControl As HtmlControl = e.Item.FindControl("DVselectorAll")
                    oControl.Visible = (Pager.Count > CurrentPageSize)

                    oControl = e.Item.FindControl("DVselectorNone")
                    oControl.Visible = (Pager.Count > CurrentPageSize)
                Else
                    oSpan.Visible = False
                End If
                oTableCell = e.Item.FindControl("THsingleSelect")
                oTableCell.Visible = False
            Else
                oTableCell.Visible = False
                oTableCell = e.Item.FindControl("THsingleSelect")
                oTableCell.Visible = True

                Dim oLiteral As Literal = e.Item.FindControl("LTsingleSelectHeader")
                Resource.setLiteral(oLiteral)
            End If


            oLabel = e.Item.FindControl("LBuserRoleHeader_t")
            Me.Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLabel As Label = e.Item.FindControl("LBcompanyName")
            Dim idUser As Integer = 0
            Dim oLiteral As Literal
            If TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))
                oLabel = e.Item.FindControl("LBprofileStatus")
                oLabel.Text = Resource.getValue("status.Profile." & item.Status.ToString)
                idUser = item.Id
            ElseIf TypeOf e.Item.DataItem Is dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) Then
                Dim item As dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) = DirectCast(e.Item.DataItem, dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))
                oLabel = e.Item.FindControl("LBprofileStatus")
                oLabel.Text = Resource.getValue("status.Subscription." & item.Status.ToString)
                oLabel = e.Item.FindControl("LBroleName")
                oLabel.Text = item.RoleName
                idUser = item.Id
            ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))
                oLabel.Text = DirectCast(item.Profile, lm.Comol.Core.Authentication.dtoCompany).Info.Name
                oLabel = e.Item.FindControl("LBprofileStatus")
                oLabel.Text = Resource.getValue("status.Profile." & item.Status.ToString)
                idUser = item.Id
            ElseIf TypeOf e.Item.DataItem Is dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) Then
                Dim item As dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) = DirectCast(e.Item.DataItem, dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))
                oLabel.Text = DirectCast(item.Profile, lm.Comol.Core.Authentication.dtoCompany).Info.Name
                oLabel = e.Item.FindControl("LBprofileStatus")
                oLabel.Text = Resource.getValue("status.Subscription." & item.Status.ToString)
                oLabel = e.Item.FindControl("LBroleName")
                oLabel.Text = item.RoleName

                idUser = item.Id
            ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))
                oLabel = e.Item.FindControl("LBagencyName")
                If IsNothing(item.Profile.CurrentAgency) Then
                    oLabel.Text = ""
                Else
                    oLabel.Text = item.Profile.CurrentAgency.Value
                End If
                oLabel = e.Item.FindControl("LBprofileStatus")
                oLabel.Text = Resource.getValue("status.Profile." & item.Status.ToString)
                idUser = item.Id
            ElseIf TypeOf e.Item.DataItem Is dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) Then
                Dim item As dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) = DirectCast(e.Item.DataItem, dtoSubscriptionProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))
                oLabel = e.Item.FindControl("LBagencyName")
                If IsNothing(item.Profile.CurrentAgency) Then
                    oLabel.Text = ""
                Else
                    oLabel.Text = item.Profile.CurrentAgency.Value
                End If
                oLabel = e.Item.FindControl("LBprofileStatus")
                oLabel.Text = Resource.getValue("status.Subscription." & item.Status.ToString)

                oLabel = e.Item.FindControl("LBroleName")
                oLabel.Text = item.RoleName

                idUser = item.Id
            End If

            Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDmultiSelect")
            If MultipleSelection Then
                oTableCell.Visible = True

                Dim oCheckBox As HtmlInputCheckBox = e.Item.FindControl("CBXuser")
                oCheckBox.Checked = SelectAllUsers OrElse SelectedIdUsers.Contains(idUser)

                oTableCell = e.Item.FindControl("TDsingleSelect")
                oTableCell.Visible = False
            Else
                oTableCell.Visible = False
                oTableCell = e.Item.FindControl("TDsingleSelect")
                oTableCell.Visible = True

                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBselectUser")
                Resource.setLinkButton(oLinkbutton, False, True)
            End If


        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (Me.RPTprofiles.Items.Count = 0)
            If (Me.RPTprofiles.Items.Count = 0) Then
                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                oTableCell.ColSpan = 3 + AvailableColumns.Where(Function(c) c = ProfileColumn.agency OrElse c = ProfileColumn.mail OrElse c = ProfileColumn.status OrElse c = ProfileColumn.type OrElse c = ProfileColumn.companyName OrElse c = ProfileColumn.communityrole).Count

                Dim oLabel As Label = e.Item.FindControl("LBprofileEmptyItems")

                oLabel.Text = Resource.getValue("profileEmptyItems." & SelectionMode.ToString & "." & IsFirstLoad.ToString.ToLower)
            End If
        End If
    End Sub
    Private Sub RPTprofiles_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTprofiles.ItemCommand
        Select Case e.CommandName
            Case "all"
                Me.CurrentPresenter.EditItemsSelection(True)
            Case "none"
                Me.CurrentPresenter.EditItemsSelection(False)
            Case "selectuser"
                RaiseEvent UserSelected(CInt(e.CommandArgument))
        End Select
    End Sub
#End Region

#Region "Community users"
    Private Sub DDLcommunityRole_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLcommunityRole.SelectedIndexChanged
        Me.CurrentPresenter.ChangeRoleType(SelectedIdOrganization, SelectedIdProfileType, Me.Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub DDLcommunityProfileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLcommunityProfileType.SelectedIndexChanged
        Me.CurrentPresenter.ChangeCommunityProfileType(SelectedIdProfileType, Me.Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub DDLsubscriptionStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLsubscriptionStatus.SelectedIndexChanged
        Me.CurrentPresenter.LoadSubscriptions(Me.Pager.PageIndex, CurrentPageSize, True)
    End Sub
    Private Sub DDLcommunityAgencies_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLcommunityAgencies.SelectedIndexChanged
        Me.CurrentPresenter.LoadSubscriptions(Me.Pager.PageIndex, CurrentPageSize, True)
    End Sub
#End Region

    Private Sub BTNapplyFilters_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNapplyFilters.Click
        Select Case SelectionMode
            Case UserSelectionType.SystemUsers
                Dim dto As dtoFilters = GetCurrentProfileFilters
                dto.PageIndex = 0
                dto.PageSize = Me.DefaultPageSize
                Me.SearchProfileFilters = dto
                Me.CurrentPresenter.LoadProfiles(0, CurrentPageSize, True)
            Case UserSelectionType.CommunityUsers
                Dim dto As dtoUserFilters = GetCurrentUserFilters
                dto.PageIndex = 0
                dto.PageSize = Me.DefaultPageSize
                Me.SearchUserFilters = dto
                Me.CurrentPresenter.LoadSubscriptions(0, CurrentPageSize, True)
        End Select
        DDLpageSizeTop.AutoPostBack = True
    End Sub
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Select Case SelectionMode
            Case UserSelectionType.SystemUsers
                Me.CurrentPresenter.LoadProfiles(Me.PGgrid.Pager.PageIndex, CurrentPageSize, True)
            Case UserSelectionType.CommunityUsers
                Me.CurrentPresenter.LoadSubscriptions(Me.PGgrid.Pager.PageIndex, CurrentPageSize, True)
        End Select
    End Sub
    Private Sub DDLpageSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLpageSizeBottom.SelectedIndexChanged, DDLpageSizeTop.SelectedIndexChanged
        Select Case SelectionMode
            Case UserSelectionType.SystemUsers
                Me.CurrentPresenter.LoadProfiles(Me.PGgrid.Pager.PageIndex, CurrentPageSize, True)
            Case UserSelectionType.CommunityUsers
                Me.CurrentPresenter.LoadSubscriptions(Me.PGgrid.Pager.PageIndex, CurrentPageSize, True)
        End Select
    End Sub
#End Region

#Region "Internal"
    Private Sub InitializePageSizeSelector(dropdwon As DropDownList, value As Integer)
        Dim items As List(Of Int32) = (From i As ListItem In dropdwon.Items Select CInt(i.Value)).ToList()
        If items.Any() AndAlso items.Contains(value) Then
            dropdwon.SelectedValue = value.ToString
        Else
            If Not items.Any() Then
                items = (From e As Integer In Enumerable.Range(1, 6) Select e * 25).ToList()
            End If
            If items.Any() Then
                items.Add(value)
            End If

            dropdwon.DataSource = items.Distinct().OrderBy(Function(i) i).ToList
            dropdwon.DataBind()
            dropdwon.SelectedValue = value.ToString
        End If

    End Sub
  
    Private Sub BTNcancel_Click(sender As Object, e As System.EventArgs) Handles BTNcancel.Click
        If RaiseCommandEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub
    Private Sub BTNselect_Click(sender As Object, e As System.EventArgs) Handles BTNselect.Click
        If RaiseCommandEvents Then
            RaiseEvent UsersSelected(Me.CurrentPresenter.GetSelectedIdUsers())
        End If
    End Sub
    Private Sub BTNreviewSelection_Click(sender As Object, e As System.EventArgs) Handles BTNreviewSelection.Click
        Me.CurrentPresenter.DisplayPreviewSelection()
    End Sub

#Region "Preview"
    Private Sub BTNapplyPreviewFilters_Click(sender As Object, e As System.EventArgs) Handles BTNapplyPreviewFilters.Click
        Me.CurrentPresenter.LoadPreviewItems(TXBsearchInPreview.Text, Me.CTRLpreviewAlphabetSelector.SelectedItem)
    End Sub
    Private Sub CTRLmessages_ItemCommand(cancel As Boolean, value As String) Handles CTRLmessages.ItemCommand
        Me.AllowSelectAllFromPreview = Not cancel
        Me.CurrentPresenter.LoadPreviewItems(TXBsearchInPreview.Text, CTRLpreviewAlphabetSelector.SelectedItem)
    End Sub
    Private Sub CTRLpreviewAlphabetSelector_SelectItem(letter As String) Handles CTRLpreviewAlphabetSelector.SelectItem
        CTRLpreviewAlphabetSelector.SelectedItem = letter
        Me.CurrentPresenter.LoadPreviewItems(TXBsearchInPreview.Text, letter)
    End Sub
    Private Sub RPTpreviewItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpreviewItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBuserSurnameHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserNameHeader_t")
            Me.Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'Dim oLabel As Label ' = e.Item.FindControl("LBcompanyName")
            'Dim idUser As Integer = 0
            'Dim oLiteral As Literal
            'If TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) Then
            '    Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))
            '    oLabel = e.Item.FindControl("LBprofileStatus")
            '    oLabel.Text = Resource.getValue("status.Profile." & item.Status.ToString)
            '    idUser = item.Id

            'ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) Then
            '    Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))
            '    oLabel.Text = DirectCast(item.Profile, lm.Comol.Core.Authentication.dtoCompany).Info.Name
            '    oLabel = e.Item.FindControl("LBprofileStatus")
            '    oLabel.Text = Resource.getValue("status.Profile." & item.Status.ToString)
            '    idUser = item.Id
            'ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) Then
            '    Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))
            '    oLabel = e.Item.FindControl("LBagencyName")
            '    If IsNothing(item.Profile.CurrentAgency) Then
            '        oLabel.Text = ""
            '    Else
            '        oLabel.Text = item.Profile.CurrentAgency.Value
            '    End If
            '    oLabel = e.Item.FindControl("LBprofileStatus")
            '    oLabel.Text = Resource.getValue("status.Profile." & item.Status.ToString)
            '    idUser = item.Id
            'End If
            Dim oCheckbox As HtmlInputCheckBox = e.Item.FindControl("CBXuser")
            oCheckbox.Checked = True

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (RPTpreviewItems.Items.Count = 0)
            If (RPTpreviewItems.Items.Count = 0) Then
                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                '  oTableCell.ColSpan = 3 + AvailableColumns.Where(Function(c) c = ProfileColumn.agency OrElse c = ProfileColumn.mail OrElse c = ProfileColumn.status OrElse c = ProfileColumn.type OrElse c = ProfileColumn.companyName).Count

                Dim oLabel As Label = e.Item.FindControl("LBnoPreviewItems")
                oLabel.Text = Resource.getValue("noPreviewItems." & IsFirstPreviewLoad)
            End If
        End If
    End Sub
    Private Sub PGgridPreview_OnPageSelected() Handles PGgridPreview.OnPageSelected
        Me.CurrentPresenter.LoadPreviewItems(Me.PGgridPreview.Pager.PageIndex, Me.DefaultMaxPreviewItems, True)
    End Sub
    Private Sub BTNconfirmSelectionEdit_Click(sender As Object, e As System.EventArgs) Handles BTNconfirmSelectionEdit.Click
        Me.DVpreview.Visible = False
        Me.DVselectors.Visible = True
        Me.CurrentPresenter.ConfirmUserSelection(True)
    End Sub
    Private Sub BTNconfirmSelectionEditAndInsert_Click(sender As Object, e As System.EventArgs) Handles BTNconfirmSelectionEditAndInsert.Click
        If RaiseCommandEvents Then
            RaiseEvent UsersSelected(Me.CurrentPresenter.ConfirmUserSelection(False))
        End If
    End Sub
    Private Sub BTNcloseUserSelectionPreview_Click(sender As Object, e As System.EventArgs) Handles BTNcloseUserSelectionPreview.Click
        Me.HideUsersPreview()
    End Sub
#End Region
#End Region

End Class