
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class PreviewSettings
    Inherits DBpageBase
    Implements IViewDashboardPreview

#Region "Context"
    Private _presenter As DashboardPreviewPresenter
    Protected Friend ReadOnly Property CurrentPresenter As DashboardPreviewPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New DashboardPreviewPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Private ReadOnly Property PreloadStep As WizardDashboardStep Implements IViewDashboardPreview.PreloadStep
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WizardDashboardStep).GetByString(Request.QueryString("step"), WizardDashboardStep.None)
        End Get
    End Property
    Private ReadOnly Property PreloadDashboardType As DashboardType Implements IViewDashboardPreview.PreloadDashboardType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DashboardType).GetByString(Request.QueryString("type"), DashboardType.Portal)
        End Get
    End Property
    Private ReadOnly Property PreloadIdDashboard As Long Implements IViewDashboardPreview.PreloadIdDashboard
        Get
            If IsNumeric(Request.QueryString("idDashboard")) Then
                Return CLng(Request.QueryString("idDashboard"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdTile As Long Implements IViewDashboardPreview.PreloadIdTile
        Get
            If IsNumeric(Request.QueryString("idTile")) Then
                Return CLng(Request.QueryString("idTile"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdTag As Long Implements IViewDashboardPreview.PreloadIdTag
        Get
            If IsNumeric(Request.QueryString("idTag")) Then
                Return CLng(Request.QueryString("idTag"))
            Else
                Return -1
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadGroupBy As GroupItemsBy Implements IViewDashboardPreview.PreloadGroupBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of GroupItemsBy).GetByString(Request.QueryString("g"), GroupItemsBy.None)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadOrderBy As OrderItemsBy Implements IViewDashboardPreview.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderItemsBy).GetByString(Request.QueryString("o"), OrderItemsBy.LastAccess)
        End Get
    End Property
    Private ReadOnly Property PreloadViewType As DashboardViewType Implements IViewDashboardPreview.PreloadViewType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DashboardViewType).GetByString(Request.QueryString("vType"), DashboardViewType.List)
        End Get
    End Property
    Protected Friend Property TagsToLoad As List(Of Long) Implements IViewDashboardPreview.TagsToLoad
        Get
            Return ViewStateOrDefault("TagsToLoad", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("TagsToLoad") = value
        End Set
    End Property
#End Region

#Region "Settings"
    Protected Friend Property CurrentStep As WizardDashboardStep Implements IViewDashboardPreview.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", WizardDashboardStep.None)
        End Get
        Set(ByVal value As WizardDashboardStep)
            Me.ViewState("CurrentStep") = value
        End Set
    End Property
    Protected Friend Property DashboardType As DashboardType Implements IViewDashboardPreview.DashboardType
        Get
            Return ViewStateOrDefault("DashboardType", lm.Comol.Core.Dashboard.Domain.DashboardType.Portal)
        End Get
        Set(ByVal value As DashboardType)
            Me.ViewState("DashboardType") = value
        End Set
    End Property
    Protected Friend Property IdDashboard As Long Implements IViewDashboardPreview.IdDashboard
        Get
            Return ViewStateOrDefault("IdDashboard", 0)
        End Get
        Set(value As Long)
            ViewState("IdDashboard") = value
        End Set
    End Property
    Private Property CurrentLayout As PlainLayout Implements IViewDashboardPreview.CurrentLayout
        Get
            Return ViewStateOrDefault("CurrentLayout", PlainLayout.ignore)
        End Get
        Set(value As PlainLayout)
            ViewState("CurrentLayout") = value
        End Set
    End Property
    Private Property SelectedGroupBy As GroupItemsBy Implements IViewDashboardPreview.SelectedGroupBy
        Get
            Return ViewStateOrDefault("SelectedGroupBy", GroupItemsBy.None)
        End Get
        Set(value As GroupItemsBy)
            ViewState("SelectedGroupBy") = value
        End Set
    End Property
    Private Property CurrentViewType As DashboardViewType Implements IViewDashboardPreview.CurrentViewType
        Get
            Return ViewStateOrDefault("CurrentViewType", DashboardViewType.List)
        End Get
        Set(value As DashboardViewType)
            ViewState("CurrentViewType") = value
        End Set
    End Property
    Private Property CurrentSettings As UserCurrentSettings Implements IViewDashboardPreview.CurrentSettings
        Get
            Return ViewStateOrDefault("CurrentSettings", New UserCurrentSettings)
        End Get
        Set(value As UserCurrentSettings)
            ViewState("CurrentSettings") = value
        End Set
    End Property
    Private Property CurrentOrderItems As Dictionary(Of DashboardViewType, List(Of dtoItemFilter(Of OrderItemsBy))) Implements IViewDashboardPreview.CurrentOrderItems
        Get
            Return ViewStateOrDefault("CurrentOrderItems", New Dictionary(Of DashboardViewType, List(Of dtoItemFilter(Of OrderItemsBy))))
        End Get
        Set(value As Dictionary(Of DashboardViewType, List(Of dtoItemFilter(Of OrderItemsBy))))
            ViewState("CurrentOrderItems") = value
        End Set
    End Property
    Protected Friend Property IsInitialized As Boolean Implements IViewDashboardPreview.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
#End Region

#End Region
#Region "Internal"
    Protected Friend Property CurrentDisplayNoticeboard() As DisplayNoticeboard
        Get
            Return ViewStateOrDefault("CurrentDisplayNoticeboard", DisplayNoticeboard.Hide)
        End Get
        Set(value As DisplayNoticeboard)
            ViewState("CurrentDisplayNoticeboard") = value
        End Set
    End Property
#End Region

#Region "Inherits Method"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        If Not Page.IsPostBack Then
            CTRLfiltersHeader.SetTransacionIdContainer(Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID & "_" & PageUtility.CurrentContext.UserContext.WorkSessionID.ToString)
        End If
        CurrentPresenter.InitView(PreloadStep, PreloadIdDashboard, PreloadIdCommunity, PreloadDashboardType)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        If IsInitialized Then
            Select Case CurrentViewType
                Case DashboardViewType.Combined, DashboardViewType.Search
                    RedirectOnSessionTimeOut(RootObject.DashboardPreview(IdDashboard, CurrentViewType, SelectedGroupBy, PreloadOrderBy, PreloadIdTile, PreloadIdTag), DashboardIdCommunity, lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow)
                Case Else
                    RedirectOnSessionTimeOut(RootObject.DashboardPreview(IdDashboard, PreloadDashboardType, DashboardIdCommunity, CurrentStep), DashboardIdCommunity, lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow)
            End Select
        Else
            RedirectOnSessionTimeOut(RootObject.DashboardPreview(PreloadIdDashboard, PreloadDashboardType, PreloadIdCommunity, PreloadStep), PreloadIdCommunity, lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow)
        End If
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBviewSelectorDescription)
            .setLabel(LBgroupedSelectorDescription)
            .setButton(BTNsearchByName, True)

            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLiteral(LTsearchFiltersTitle)
            .setLinkButton(LNBapplySearchFilters, False, True)

            Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.PreviewDashboard")
            Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.PreviewDashboard.NoPermission")
        End With
    End Sub
#End Region

#Region "Implements"
#Region "Common"
    Private Sub EnableFullWidth(value As Boolean) Implements IViewDashboardPreview.EnableFullWidth
        Master.EnabledFullWidth = value
    End Sub
    Private Sub InitializeLayout(layout As PlainLayout, display As DisplayNoticeboard) Implements IViewDashboardPreview.InitializeLayout
        CurrentLayout = layout
        CurrentDisplayNoticeboard = display
        If display <> DisplayNoticeboard.Hide Then
            CTRLnoticeboard.Visible = True
            CTRLnoticeboard.InitalizeControl(layout, 0)
            CTRLnoticeboardCombinedBlock.Visible = True
            CTRLnoticeboardCombinedBlock.InitalizeControl(layout, 0)
        Else
            CTRLnoticeboard.Visible = False
            CTRLnoticeboardCombinedBlock.Visible = False
        End If
    End Sub
    Private Sub InitializeSettingsInfo(settings As liteDashboardSettings) Implements IViewDashboardPreview.InitializeSettingsInfo
        Master.ServiceTitle = String.Format(Resource.getValue("DashboardSettings.ServiceTitle.PreviewDashboard.Date"), GetDateToString(settings.ModifiedOn, ""), GetTimeToString(settings.ModifiedOn, ""))
        Master.ServiceTitleToolTip = String.Format(Resource.getValue("DashboardSettings.ServiceTitle.PreviewDashboard.Date"), settings.Name, GetDateToString(settings.ModifiedOn, ""), GetTimeToString(settings.ModifiedOn, ""))
    End Sub
#End Region

#Region "TopBar"
    Private Sub InitializeGroupBySelector(items As List(Of dtoItemFilter(Of GroupItemsBy))) Implements IViewDashboardPreview.InitializeGroupBySelector
        If items.Count > 1 Then
            DVgroupedSelector.Visible = True
            RPTgroupBy.DataSource = items
            RPTgroupBy.DataBind()
            LBgroupBySelected.Text = Resource.getValue("GroupItemsBy." & items.Where(Function(i) i.Selected).FirstOrDefault.Value.ToString)
        ElseIf items.Any() Then
            DVgroupedSelector.Visible = False
            SelectedGroupBy = items.FirstOrDefault().Value
            LBgroupBySelected.Text = Resource.getValue("GroupItemsBy." & SelectedGroupBy.ToString)
        Else
            SelectedGroupBy = GroupItemsBy.None
            LBgroupBySelected.Text = ""
            DVgroupedSelector.Visible = False
        End If
    End Sub
    Private Sub InitializeSearch(settings As DisplaySearchItems) Implements IViewDashboardPreview.InitializeSearch
        TXBsearchByName.Text = ""
        DVsearch.Visible = Not settings = DisplaySearchItems.Hide
    End Sub
    Private Sub InitializeViewSelector(items As List(Of dtoItemFilter(Of DashboardViewType))) Implements IViewDashboardPreview.InitializeViewSelector
        If items.Any() Then
            SPNviewSelector.Visible = True

            LNBgotoCombinedView.Visible = False
            LNBgotoListView.Visible = False
            LNBgotoTileView.Visible = False
            If items.Where(Function(v) v.Selected).Any() Then
                CurrentViewType = items.Where(Function(v) v.Selected).Select(Function(v) v.Value).FirstOrDefault()
            End If
            'LBviewSelectorDescription.Visible = items.Count > 0

            For Each i As dtoItemFilter(Of DashboardViewType) In items.Where(Function(v) v.Value <> DashboardViewType.Search)
                Dim oLinkButton As LinkButton = Nothing
                Dim oLiteral As Literal = Nothing
                Select Case i.Value
                    Case DashboardViewType.List
                        oLinkButton = LNBgotoListView
                        oLiteral = LTtemplateviewList
                    Case DashboardViewType.Combined
                        oLinkButton = LNBgotoCombinedView
                        oLiteral = LTtemplateviewCombined
                    Case DashboardViewType.Tile
                        oLinkButton = LNBgotoTileView
                        oLiteral = LTtemplateviewTile
                End Select
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.Visible = True
                    oLinkButton.Text = String.Format(oLiteral.Text, Resource.getValue("view." & i.Value.ToString & ".ToolTip"), Resource.getValue("view." & i.Value.ToString & ".Text"), IIf(i.Selected, LTcssActiveClass.Text, ""))
                End If
            Next
        Else
            SPNviewSelector.Visible = False
        End If
    End Sub
#End Region

#Region "Content"
    Private Sub InitializeCommunitiesList(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy))) Implements IViewDashboardPreview.InitializeCommunitiesList
        MLVviews.SetActiveView(VIWlist)
        CTRLlistMyCommunities.Visible = True
        CTRLlistMyCommunities.AutoDisplayTitle = Resource.getValue("AutoDisplayTitle.MyCommunities")
        CTRLlistMyCommunities.InitalizeControl(pSettings, userSettings, items, lm.Comol.Core.DomainModel.ItemDisplayOrder.first, 0)

        If CTRLlistMyCommunities.ItemsCount = 0 Then
            CTRLlistMyOrganizations.IsCollapsed = False
        Else
            CTRLlistMyOrganizations.IsCollapsed = Not IsNothing(pSettings) AndAlso Not pSettings.ExpandOrganizationList
        End If
        CTRLlistMyOrganizations.Visible = True
        CTRLlistMyOrganizations.AutoDisplayTitle = Resource.getValue("AutoDisplayTitle.MyOrganization")
        CTRLlistMyOrganizations.InitalizeControlForCommunityType(pSettings, userSettings, items, 0, lm.Comol.Core.DomainModel.ItemDisplayOrder.last)
    End Sub
    Private Sub IntializeCombinedView(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idDashboard As Long, Optional idTile As Long = -1) Implements IViewDashboardPreview.IntializeCombinedView
        MLVviews.SetActiveView(VIWcombined)
        CTRLminiTile.Visible = True
        CTRLminiTile.InitalizeControlForTile(pSettings, userSettings, items, idDashboard, False, False, idTile)
    End Sub
    Private Sub IntializeTileView(idCommunity As Integer, noticeboard As DisplayNoticeboard, pSettings As litePageSettings, userSettings As UserCurrentSettings, idDashboard As Long) Implements IViewDashboardPreview.IntializeTileView
        MLVviews.SetActiveView(VIWtile)
        CTRLtiles.InitalizeControl(idCommunity, noticeboard, pSettings, userSettings, idDashboard, False)
    End Sub
    Private Sub InitializeSearchView(pageSettings As litePageSettings, filtersToLoad As List(Of lm.Comol.Core.DomainModel.Filters.Filter), filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters, items As List(Of dtoItemFilter(Of OrderItemsBy)), tile As liteTile, idLanguage As Integer, idDefaultLanguage As Integer) Implements IViewDashboardPreview.InitializeSearchView
        MLVviews.SetActiveView(VIWsearch)
        SetListTitle(filters, tile, idLanguage, idDefaultLanguage)
        CTRLfiltersHeader.SetDefaultFilters(filtersToLoad)
        CTRLsearchList.InitalizeControl(pageSettings, filters, items, tile)
    End Sub
    Private Sub ApplyFilters(pageSettings As litePageSettings, filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters, tile As liteTile, idLanguage As Integer, idDefaultLanguage As Integer) Implements IViewDashboardPreview.ApplyFilters
        SetListTitle(filters, tile, idLanguage, idDefaultLanguage)
        CTRLsearchList.ApplyFilters(pageSettings, filters)
    End Sub
    Protected Friend Function GetSubmittedFilters() As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewDashboardPreview.GetSubmittedFilters
        Dim filter As New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        filter.Availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed
        If Not Page.IsPostBack Then
            filter.IdOrganization = -1
            filter.IdcommunityType = -1
        End If

        With filter
            Dim keys As List(Of String) = Request.Form.AllKeys.ToList()
            For Each item As lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType))
                Select Case item
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.communitytype
                        If keys.Contains(item.ToString) Then
                            .IdcommunityType = CInt(Request.Form(item.ToString))
                        Else
                            .IdcommunityType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.coursetime
                        If keys.Contains(item.ToString) Then
                            .IdCourseTime = CInt(Request.Form(item.ToString))
                        Else
                            .IdCourseTime = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.degreetype
                        If keys.Contains(item.ToString) Then
                            .IdDegreeType = CInt(Request.Form(item.ToString))
                        Else
                            .IdDegreeType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.organization
                        If keys.Contains(item.ToString) Then
                            .IdOrganization = CInt(Request.Form(item.ToString))
                        Else
                            .IdOrganization = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.responsible
                        If keys.Contains(item.ToString) Then
                            .IdResponsible = CInt(Request.Form(item.ToString))
                        Else
                            .IdResponsible = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.status
                        If keys.Contains(item.ToString) Then
                            .Status = CInt(Request.Form(item.ToString))
                        Else
                            .Status = lm.Comol.Core.Communities.CommunityStatus.None
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.year
                        If keys.Contains(item.ToString) Then
                            .Year = CInt(Request.Form(item.ToString))
                        Else
                            .Year = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag
                        If keys.Contains(item.ToString) Then
                            For Each idTag As String In Request.Form(item.ToString).Split(",")
                                .IdTags.Add(CLng(idTag))
                            Next
                        Else
                            .IdTags = New List(Of Long)
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.name
                        .SearchBy = lm.Comol.Core.BaseModules.CommunityManagement.SearchCommunitiesBy.Contains
                        .Value = Request.Form(item.ToString)
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.letters
                        Dim charInt As Integer = CInt(Request.Form(item.ToString))
                        Select Case charInt
                            Case -1
                                .StartWith = ""
                            Case -9
                                .StartWith = "#"
                            Case Else
                                .StartWith = Char.ConvertFromUtf32(charInt).ToLower()
                        End Select
                End Select
            Next
        End With

        Return filter
    End Function
    Private Sub SetListTitle(filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters, tile As liteTile, idLanguage As Integer, idDefaultLanguage As Integer)
        Dim displayName As String = Resource.getValue("SearchAutoDisplayTitle.MyCommunityType." & filters.IdcommunityType)
        If Not IsNothing(tile) Then 'AndAlso (Not String.IsNullOrEmpty(tile.ImageCssClass) OrElse Not String.IsNullOrEmpty(tile.ImageUrl)) Then
            If Not String.IsNullOrEmpty(tile.ImageCssClass) Then
                CTRLsearchList.TitleCssClass = tile.ImageCssClass
            ElseIf Not String.IsNullOrEmpty(tile.ImageCssClass) Then
                CTRLsearchList.TitleImage = tile.ImageCssClass
            Else
                CTRLsearchList.TitleCssClass = LTcssClassTitle.Text
            End If
            If String.IsNullOrEmpty(displayName) Then
                Dim translation As lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation = tile.GetTranslation(idLanguage, idDefaultLanguage)
                If IsNothing(translation) OrElse String.IsNullOrEmpty(translation.Title) Then
                    displayName = Resource.getValue("SearchAutoDisplayTitle.Communities")
                Else
                    displayName = translation.Title
                End If
            End If
        Else
            CTRLsearchList.TitleCssClass = LTcssClassTitle.Text
            displayName = Resource.getValue("SearchAutoDisplayTitle.Communities")
            CTRLsearchList.AutoDisplayTitle = Resource.getValue("AutoDisplayTitle.MyOrganization")
        End If
        CTRLsearchList.AutoDisplayTitle = displayName
    End Sub
#End Region
    Private Sub DisplayUnknownDashboard() Implements IViewDashboardPreview.DisplayUnknownDashboard
        MLVdashboard.SetActiveView(VIWwrongSettings)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnknownDashboard"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayNoViewAvailable() Implements IViewDashboardPreview.DisplayNoViewAvailable
        MLVdashboard.SetActiveView(VIWwrongSettings)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoViewAvailable"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
#End Region

#Region "Internal"
    Protected Friend Function GetNoticeboardPosition() As String
        Select Case CurrentDisplayNoticeboard
            Case DisplayNoticeboard.OnLeft
                Return "noticeboardleft"
            Case DisplayNoticeboard.OnRight
                Return "noticeboardright"
        End Select
    End Function
    Public Function GetContentItemColspan() As Integer
        Select Case CurrentLayout
            Case PlainLayout.box7box5
                Return 7
            Case PlainLayout.box8box4
                Return 8
            Case PlainLayout.full
                Return 12
            Case PlainLayout.ignore
                Return 0
        End Select
    End Function
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
    Public Function GetCollapsedCssClass() As String
        If Not Page.IsPostBack Then
            Return ""
        ElseIf Not Page.IsPostBack Then
            Return LTcssClassCollapsed.Text
        End If
    End Function
    Private Sub PreviewSettings_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub

#Region "TopBar"
    Public Function GetItemCssClass(ByVal item As dtoItemFilter(Of GroupItemsBy)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssActiveClass.Text
        End If
        Return cssClass
    End Function
    Protected Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
        Dim cssClass As String = ""
        Select Case d
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & d.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function
    Private Sub RPTgroupBy_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTgroupBy.ItemDataBound
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBgroupItemsBy")
        Dim oItem As dtoItemFilter(Of GroupItemsBy) = e.Item.DataItem


        oLinkButton.CommandArgument = CInt(oItem.Value)
        oLinkButton.Text = String.Format(LTgroupItemsByTemplate.Text, Resource.getValue("GroupItemsBy." & oItem.Value.ToString))
        Dim oControl As HtmlControl = e.Item.FindControl("DVitemGroupBy")
        oControl.Attributes("class") = LTcssClassGroupBy.Text & " " & GetItemCssClass(oItem)
    End Sub
    Private Sub RPTgroupBy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTgroupBy.ItemCommand

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemGroupBy")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPTgroupBy.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("DVitemGroupBy")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        SelectedGroupBy = CInt(e.CommandArgument)
        LBgroupBySelected.Text = Resource.getValue("GroupItemsBy." & SelectedGroupBy.ToString)
        CurrentPresenter.ChangeGroupBy(IdDashboard, CurrentViewType, e.CommandArgument, CurrentSettings)
    End Sub
    Private Sub LNBgotoTileView_Click(sender As Object, e As EventArgs) Handles LNBgotoTileView.Click
        CurrentPresenter.ChangeView(DashboardViewType.Tile, IdDashboard, CurrentSettings, CurrentOrderItems)
    End Sub
    Private Sub LNBgotoListView_Click(sender As Object, e As EventArgs) Handles LNBgotoListView.Click
        CurrentPresenter.ChangeView(DashboardViewType.List, IdDashboard, CurrentSettings, CurrentOrderItems)
    End Sub
    Private Sub LNBgotoCombinedView_Click(sender As Object, e As EventArgs) Handles LNBgotoCombinedView.Click
        CurrentPresenter.ChangeView(DashboardViewType.Combined, IdDashboard, CurrentSettings, CurrentOrderItems)
    End Sub
    Private Sub BTNsearchByName_Click(sender As Object, e As EventArgs) Handles BTNsearchByName.Click
        CurrentPresenter.ChangeView(DashboardViewType.Search, IdDashboard, CurrentSettings, CurrentOrderItems, TXBsearchByName.Text)
    End Sub
    Private Sub LNBapplySearchFilters_Click(sender As Object, e As EventArgs) Handles LNBapplySearchFilters.Click
        CurrentPresenter.ApplyFilters(IdDashboard, GetSubmittedFilters())
    End Sub
#End Region
#End Region


  
End Class