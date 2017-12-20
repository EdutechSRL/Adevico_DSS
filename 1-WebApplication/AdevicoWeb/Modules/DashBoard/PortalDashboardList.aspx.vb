Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class PortalDashboardList
    Inherits DBpageBasePortalDashboard

#Region "Context"
    Private _Presenter As PortalDashboardPresenter
    Private ReadOnly Property CurrentPresenter() As PortalDashboardPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PortalDashboardPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(GetCurrentView)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Protected Friend Overrides Function GetTopBarControl() As UC_DashboardTopBar
        Return CTRLdashboardTopBar
    End Function
#End Region

#Region "Implements"
    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim settings As UserCurrentSettings = GetCurrentCookie()
        If IsNothing(settings) Then
            settings = New UserCurrentSettings
            settings.View = DashboardViewType.List
            settings.GroupBy = GroupItemsBy.None
            settings.OrderBy = CTRLlistMyCommunities.CurrentOrderBy
            settings.Ascending = False
            settings.ListNoticeboard = CurrentDisplayNoticeboard
        End If

        RedirectOnSessionTimeOut(RootObject.LoadPortalView(0, settings), 0)
    End Sub
    Protected Friend Overrides Function GetCurrentView() As lm.Comol.Core.Dashboard.Domain.DashboardViewType
        Return lm.Comol.Core.Dashboard.Domain.DashboardViewType.List
    End Function
    Protected Friend Overrides Sub EnableFullWidth(value As Boolean)
        Master.EnabledFullWidth = value
    End Sub
    Protected Friend Overrides Sub InitializeLayout(layout As PlainLayout, display As DisplayNoticeboard)
        CurrentLayout = layout
        CurrentDisplayNoticeboard = display
        If display <> DisplayNoticeboard.Hide Then
            CTRLnoticeboard.Visible = True
            CTRLnoticeboard.InitalizeControl(layout, 0)
        Else
            CTRLnoticeboard.Visible = False
        End If
    End Sub
    Protected Friend Overrides Sub InitializeCommunitiesList(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)))
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
#Region "Ignore"
    Protected Friend Overrides Sub IntializeCombinedView(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idDashboard As Long, ByVal moreTiles As Boolean, ByVal moreCommunities As Boolean, Optional idPreloadTile As Long = -1)
    End Sub
    Protected Friend Overrides Sub IntializeTileView(idCommunity As Integer, noticeboard As DisplayNoticeboard, pSettings As litePageSettings, userSettings As UserCurrentSettings, idDashboard As Long, moreTiles As Boolean)
    End Sub
#End Region
#End Region

#Region "Internal"
    Private Sub PortalDashboardLoader_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.BRheaderActive = False
        Master.DisplayTitleRow = False
        Master.ShowDocType = True
        Master.ShowHeaderLanguageChanger = True
    End Sub
    Private Sub CTRLlistMyCommunities_SessionTimeout() Handles CTRLlistMyCommunities.SessionTimeout
        RedirectOnSessionTimeOut(RootObject.LoadPortalView(0, GetCurrentView, CTRLdashboardTopBar.SelectedGroupBy, CTRLlistMyCommunities.CurrentOrderBy, CurrentDisplayNoticeboard, -1, -1, True, False, CTRLlistMyCommunities.DisplayMoreCommand), 0)
    End Sub
    Private Sub CTRLlistMyOrganizations_SessionTimeout() Handles CTRLlistMyOrganizations.SessionTimeout
        RedirectOnSessionTimeOut(RootObject.LoadPortalView(0, GetCurrentView, CTRLdashboardTopBar.SelectedGroupBy, CTRLlistMyCommunities.CurrentOrderBy, CurrentDisplayNoticeboard, -1, -1, True, False, CTRLlistMyCommunities.DisplayMoreCommand), 0)
    End Sub
#End Region


  
End Class