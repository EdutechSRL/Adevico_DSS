Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class PortalDashboardSearch
    Inherits DBpageBaseSearch
    Implements IViewSearchDashboard


#Region "Context"
    Private _Presenter As SearchPresenter
    Private ReadOnly Property CurrentPresenter() As SearchPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SearchPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Protected Friend ReadOnly Property PreloadMyCommunities As Boolean Implements IViewSearchDashboard.PreloadMyCommunities
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("my")) AndAlso Request.QueryString("my").ToLower = Boolean.TrueString.ToLower Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdTile As Long Implements IViewSearchDashboard.PreloadIdTile
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idTile")) AndAlso IsNumeric(Request.QueryString("idTile")) Then
                Return CLng(Request.QueryString("idTile"))
            Else
                Return -1
            End If
        End Get
    End Property
    Protected Friend Property TagsToLoad As List(Of Long) Implements IViewSearchDashboard.TagsToLoad
        Get
            Return ViewStateOrDefault("TagsToLoad", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("TagsToLoad") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        If Not Page.IsPostBack Then
            CTRLfiltersHeader.SetTransacionIdContainer(Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID & "_" & PageUtility.CurrentContext.UserContext.WorkSessionID.ToString)
        End If
        Me.CurrentPresenter.InitView(PreloadSearch, PreloadSearchText)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLiteral(LTsearchFiltersTitle)
            .setLinkButton(LNBapplySearchFilters, False, True)
        End With
    End Sub
    Protected Friend Overrides Sub EnableFullWidth(value As Boolean)
        Master.EnabledFullWidth = value
    End Sub

#End Region

#Region "Implements"
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(RootObject.Search(0, PreloadSearch, PreloadSearchText, False, PreloadIdcommunityType, PreloadMyCommunities), 0)
    End Sub
    Protected Friend Overrides Function GetCurrentView() As lm.Comol.Core.Dashboard.Domain.DashboardViewType
        Return lm.Comol.Core.Dashboard.Domain.DashboardViewType.Search
    End Function

#Region "Ignore"

#End Region
    Private Sub InitalizeTopBar(settings As liteDashboardSettings, userSettings As UserCurrentSettings, searchBy As String) Implements IViewSearchDashboard.InitalizeTopBar
        CTRLdashboardTopBar.InitalizeControl(GetCurrentView, settings, userSettings, False, searchBy)
    End Sub
    Private Sub LoadDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Implements IViewSearchDashboard.LoadDefaultFilters
        CTRLfiltersHeader.SetDefaultFilters(filters)
    End Sub
    Private Sub InitializeCommunitiesList(pageSettings As litePageSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), tile As liteTile, idLanguage As Integer, idDefaultLanguage As Integer) Implements IViewSearchDashboard.InitializeCommunitiesList
        DVfilters.Visible = False
        SetListTitle(Nothing, tile, idLanguage, idDefaultLanguage)
        CTRLlistMyCommunities.InitalizeControl()
    End Sub
    Private Sub InitializeCommunitiesList(pageSettings As litePageSettings, filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters, items As List(Of dtoItemFilter(Of OrderItemsBy)), tile As liteTile, idLanguage As Integer, idDefaultLanguage As Integer) Implements IViewSearchDashboard.InitializeCommunitiesList
        DVfilters.Visible = True
        SetListTitle(filters, tile, idLanguage, idDefaultLanguage)
        CTRLlistMyCommunities.InitalizeControl(pageSettings, filters, items, tile)
    End Sub
    Private Sub ApplyFilters(pageSettings As litePageSettings, filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters, tile As liteTile, idLanguage As Integer, idDefaultLanguage As Integer) Implements IViewSearchDashboard.ApplyFilters
        SetListTitle(filters, tile, idLanguage, idDefaultLanguage)
        CTRLlistMyCommunities.ApplyFilters(pageSettings, filters)
    End Sub

    Private Sub SetListTitle(filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters, tile As liteTile, idLanguage As Integer, idDefaultLanguage As Integer)
        Dim displayName As String = ""
        If Not IsNothing(filters) Then
            displayName = Resource.getValue("SearchAutoDisplayTitle.MyCommunityType." & filters.IdcommunityType)
        Else
            displayName = Resource.getValue("SearchAutoDisplayTitle.MyCommunityType." & PreloadIdcommunityType)
        End If

        If Not PreloadMyCommunities AndAlso Not IsNothing(tile) Then 'AndAlso (Not String.IsNullOrEmpty(tile.ImageCssClass) OrElse Not String.IsNullOrEmpty(tile.ImageUrl)) Then
            If Not String.IsNullOrEmpty(tile.ImageCssClass) Then
                CTRLlistMyCommunities.TitleCssClass = tile.ImageCssClass
            ElseIf Not String.IsNullOrEmpty(tile.ImageCssClass) Then
                CTRLlistMyCommunities.TitleImage = tile.ImageCssClass
            Else
                CTRLlistMyCommunities.TitleCssClass = LTcssClassTitle.Text
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
            CTRLlistMyCommunities.TitleCssClass = LTcssClassTitle.Text
            If PreloadMyCommunities Then
                displayName = Resource.getValue("SearchAutoDisplayTitle.MyCommunities")
            Else
                displayName = Resource.getValue("SearchAutoDisplayTitle.Communities")
            End If
            CTRLlistMyCommunities.AutoDisplayTitle = Resource.getValue("AutoDisplayTitle.MyOrganization")
        End If
        CTRLlistMyCommunities.AutoDisplayTitle = displayName
    End Sub
#End Region

#Region "internal"
    Private Sub CTRLdashboardTopBar_SearchCommunity(name As String) Handles CTRLdashboardTopBar.SearchCommunity
        Dim filter As New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        filter.Availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed
        filter.SearchBy = lm.Comol.Core.BaseModules.CommunityManagement.SearchCommunitiesBy.Contains
        filter.Value = name
        filter.IdcommunityType = -1
        filter.IdOrganization = -1

        CurrentPresenter.ApplyFilters(filter)
    End Sub
    Private Sub LNBapplySearchFilters_Click(sender As Object, e As EventArgs) Handles LNBapplySearchFilters.Click
        CurrentPresenter.ApplyFilters(GetSubmittedFilters())
    End Sub

    Private Sub CTRLlistMyCommunities_DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType) Handles CTRLlistMyCommunities.DisplayMessage
        CTRLdashboardTopBar.DisplayMessage(message, type)
    End Sub
    Private Sub CTRLlistMyCommunities_HideDisplayMessage() Handles CTRLlistMyCommunities.HideDisplayMessage
        CTRLdashboardTopBar.HideDisplayMessage()
    End Sub

    Private Sub CTRLlistMyCommunities_OpenConfirmDialog(openCssClass As String) Handles CTRLlistMyCommunities.OpenConfirmDialog
        Master.SetOpenDialogOnPostbackByCssClass(openCssClass)
    End Sub
    Private Sub CTRLlistMyCommunities_SessionTimeout() Handles CTRLlistMyCommunities.SessionTimeout
        DisplaySessionTimeout()
    End Sub

    Private Sub PortalDashboardLoader_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.BRheaderActive = False
        Master.DisplayTitleRow = False
        Master.ShowDocType = True
        Master.ShowHeaderLanguageChanger = True
    End Sub
    Public Function GetCollapsedCssClass() As String
        If Not Page.IsPostBack AndAlso PreloadSearch = DisplaySearchItems.Advanced Then
            Response.SetCookie(New HttpCookie("collapsed-cl-filters", "false"))
            Return ""
        ElseIf Not Page.IsPostBack Then
            Response.SetCookie(New HttpCookie("collapsed-cl-filters", "true"))
            Return LTcssClassCollapsed.Text
        End If
        Return ""
    End Function
#End Region


   
End Class