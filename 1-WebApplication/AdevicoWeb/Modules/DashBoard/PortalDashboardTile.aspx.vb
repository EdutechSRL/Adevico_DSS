Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class PortalDashboardTile
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
        CheckDisplayError()
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
            settings.View = DashboardViewType.Tile
            settings.GroupBy = CTRLdashboardTopBar.SelectedGroupBy
            settings.OrderBy = OrderItemsBy.ActivatedOn
            settings.Ascending = False
            settings.TileNoticeboard = CurrentDisplayNoticeboard
            settings.IdSelectedTag = -1
            settings.IdSelectedTile = -1
        End If

        RedirectOnSessionTimeOut(RootObject.LoadPortalView(0, settings), 0)
    End Sub
    Protected Friend Overrides Function GetCurrentView() As lm.Comol.Core.Dashboard.Domain.DashboardViewType
        Return lm.Comol.Core.Dashboard.Domain.DashboardViewType.Tile
    End Function
    Protected Friend Overrides Sub EnableFullWidth(value As Boolean)
        Master.EnabledFullWidth = value
    End Sub

    Protected Friend Overrides Sub InitializeLayout(layout As PlainLayout, display As DisplayNoticeboard)
        CurrentLayout = layout
        CurrentDisplayNoticeboard = display
    End Sub
    Protected Friend Overrides Sub IntializeTileView(idCommunity As Integer, noticeboard As DisplayNoticeboard, pSettings As litePageSettings, userSettings As UserCurrentSettings, idDashboard As Long, moreTiles As Boolean)
        CTRLtiles.InitalizeControl(idCommunity, noticeboard, pSettings, userSettings, idDashboard, moreTiles)
    End Sub
#Region "Ignore"
    Protected Friend Overrides Sub InitializeCommunitiesList(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)))

    End Sub
    Protected Friend Overrides Sub IntializeCombinedView(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idDashboard As Long, ByVal moreTiles As Boolean, ByVal moreCommunities As Boolean, Optional idPreloadTile As Long = -1)

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
    Private Sub CTRLdashboardTopBar_GroupByChanged(groupBy As GroupItemsBy) Handles CTRLdashboardTopBar.GroupByChanged
        CurrentPresenter.ChangeGroupBy(GetCurrentView(), groupBy)
    End Sub
    Private Sub CTRLtiles_SessionTimeout(ByVal url As String) Handles CTRLtiles.SessionTimeout
        RedirectOnSessionTimeOut(url, 0)
    End Sub
    Private Sub CheckDisplayError()
        Dim err As String = Session("Redirect.Alert.NoComunityPermission") + ""
        Session("Redirect.Alert.NoComunityPermission") = ""
        If Not String.IsNullOrEmpty(err) Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(Resource.getValue("Alert.NoComunityPermission"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        Else
            CTRLmessages.Visible = False
        End If
    End Sub
#End Region
    
End Class