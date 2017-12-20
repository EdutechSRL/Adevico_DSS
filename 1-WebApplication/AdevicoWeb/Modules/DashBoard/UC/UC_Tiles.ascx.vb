Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.ActionDataContract
Public Class UC_Tiles
    Inherits DBbaseControl
    Implements IViewDashboardTilesList

#Region "Context"
    Private _Presenter As DashboardTilesListPresenter
    Private ReadOnly Property CurrentPresenter() As DashboardTilesListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DashboardTilesListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewDashboardTilesList.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Private Property TileDisplayItems As Int32 Implements IViewDashboardTilesList.TileDisplayItems
        Get
            Return ViewStateOrDefault("TileDisplayItems", 6)
        End Get
        Set(value As Int32)
            ViewState("TileDisplayItems") = value
        End Set
    End Property
    Private Property DisplayLessCommand As Boolean Implements IViewDashboardTilesList.DisplayLessCommand
        Get
            If Me.Visible Then
                Return LNBviewLess.Visible
            Else
                Return ViewStateOrDefault("DisplayLessCommand", False)
            End If
        End Get
        Set(value As Boolean)
            ViewState("DisplayLessCommand") = value
            LNBviewLess.Visible = value
        End Set
    End Property
    Private Property DisplayMoreCommand As Boolean Implements IViewDashboardTilesList.DisplayMoreCommand
        Get
            If Me.Visible Then
                Return LNBviewAll.Visible
            Else
                Return ViewStateOrDefault("DisplayMoreCommand", False)
            End If
        End Get
        Set(value As Boolean)
            ViewState("DisplayMoreCommand") = value
            LNBviewAll.Visible = value
        End Set
    End Property
    Private Property AutoUpdateLayout As Boolean Implements IViewDashboardTilesList.AutoUpdateLayout
        Get
            Return ViewStateOrDefault("AutoUpdateLayout", False)
        End Get
        Set(value As Boolean)
            ViewState("AutoUpdateLayout") = value
        End Set
    End Property
    Private Property IdCurrentDashboard As Long Implements IViewDashboardTilesList.IdCurrentDashboard
        Get
            Return ViewStateOrDefault("IdCurrentDashboard", -1)
        End Get
        Set(value As Long)
            ViewState("IdCurrentDashboard") = value
        End Set
    End Property
    Private Property IdCurrentCommunity As Integer Implements IViewDashboardTilesList.IdCurrentCommunity
        Get
            Return ViewStateOrDefault("IdCurrentCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdCurrentCommunity") = value
        End Set
    End Property
    Private Property MoreItemsAs As DisplayMoreItems Implements IViewDashboardTilesList.MoreItemsAs
        Get
            Return ViewStateOrDefault("MoreItemsAs", DisplayMoreItems.AsLink)
        End Get
        Set(value As DisplayMoreItems)
            ViewState("MoreItemsAs") = value
            '   DVtileViewMore.Visible = False
            DVviewMoreOrLess.Visible = False
            Select Case value
                Case DisplayMoreItems.AsLink
                    DVviewMoreOrLess.Visible = True
                Case DisplayMoreItems.AsTile
                    '  DVtileViewMore.Visible = True
            End Select
        End Set
    End Property
    Private Property CurrentTileLayout As TileLayout Implements IViewDashboardTilesList.CurrentTileLayout
        Get
            Return ViewStateOrDefault("CurrentTileLayout", TileLayout.grid_4)
        End Get
        Set(value As TileLayout)
            ViewState("CurrentTileLayout") = value
        End Set
    End Property
    Private Property CurrentGroupItemsBy As GroupItemsBy Implements IViewDashboardTilesList.CurrentGroupItemsBy
        Get
            Return ViewStateOrDefault("CurrentGroupItemsBy", GroupItemsBy.Tile)
        End Get
        Set(value As GroupItemsBy)
            ViewState("CurrentGroupItemsBy") = value
        End Set
    End Property
    Public Property CurrentOrderItemsBy As OrderItemsBy Implements IViewDashboardTilesList.CurrentOrderItemsBy
        Get
            Return ViewStateOrDefault("CurrentOrderItemsBy", OrderItemsBy.LastAccess)
        End Get
        Set(value As OrderItemsBy)
            ViewState("CurrentOrderItemsBy") = value
        End Set
    End Property
    Private Property CurrentDisplayNoticeboard As DisplayNoticeboard Implements IViewDashboardTilesList.CurrentDisplayNoticeboard
        Get
            Return ViewStateOrDefault("CurrentDisplayNoticeboard", DisplayNoticeboard.OnRight)
        End Get
        Set(value As DisplayNoticeboard)
            ViewState("CurrentDisplayNoticeboard") = value
        End Set
    End Property
    Public Property IsPreview As Boolean Implements IViewDashboardTilesList.IsPreview
        Get
            Return ViewStateOrDefault("IsPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPreview") = value
        End Set
    End Property
    Private Property TileRedirectOn As DashboardViewType Implements IViewDashboardTilesList.TileRedirectOn
        Get
            Return ViewStateOrDefault("TileRedirectOn", DashboardViewType.Combined)
        End Get
        Set(value As DashboardViewType)
            ViewState("TileRedirectOn") = value
        End Set
    End Property
#End Region

#Region "internal"
    Public Event SessionTimeout(ByVal url As String)
    Private ReadOnly Property TilesFilePath As String
        Get
            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.Tiles.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Tiles.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.Tiles.DrivePath
            End If
            Return baseFilePath
        End Get
    End Property
    Private ReadOnly Property TilesVirtualPath As String
        Get
            Return SystemSettings.File.Tiles.VirtualPath
        End Get
    End Property

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLinkButton(LNBviewAll, False, True)
            .setLinkButton(LNBviewLess, False, True)
            '    .setLiteral(LTminiTileTitle)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitalizeControl(idCommunity As Integer, noticeboard As DisplayNoticeboard, pageSettings As litePageSettings, userSettings As UserCurrentSettings, idDashboard As Long, moreTiles As Boolean) Implements IViewDashboardTilesList.InitalizeControl
        CurrentPresenter.InitView(idCommunity, noticeboard, pageSettings, userSettings, idDashboard, moreTiles)
    End Sub
    Private Sub DisplayErrorFromDB() Implements IViewDashboardTilesList.DisplayErrorFromDB
        HideItems()
    End Sub
    Private Sub DisplaySessionTimeout(ByVal url As String) Implements IViewDashboardTilesList.DisplaySessionTimeout
        HideItems()
        RaiseEvent SessionTimeout(url)
    End Sub
    Private Sub LoadDashboard(url As String) Implements IViewDashboardTilesList.LoadDashboard
        If Not IsPreview Then
            PageUtility.RedirectToUrl(url)
        End If
    End Sub
    Public Sub DisplayUnableToLoadTile(url As String) Implements IViewDashboardTilesList.DisplayUnableToLoadTile
        If Not IsPreview Then
            PageUtility.RedirectToUrl(url)
        End If
    End Sub
    Private Sub LoadTiles(noticeboard As DisplayNoticeboard, items As List(Of dtoTileDisplay)) Implements IViewDashboardTilesList.LoadTiles
        CTRLnoticeboard.Visible = (noticeboard <> DisplayNoticeboard.Hide)
        If noticeboard <> DisplayNoticeboard.Hide AndAlso Not CTRLnoticeboard.IsInitialized Then
            CTRLnoticeboard.InitalizeControl(CurrentTileLayout, IdCurrentCommunity)
        End If
        RPTtiles.DataSource = items
        RPTtiles.DataBind()
    End Sub
    Private Sub UpdateUserSettings(settings As UserCurrentSettings) Implements IViewDashboardTilesList.UpdateUserSettings
        If Not IsPreview Then
            SaveCurrentCookie(settings)
        End If
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idDashboard As Long, action As ModuleDashboard.ActionType) Implements IViewDashboardTilesList.SendUserAction
        If Not IsPreview Then
            Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Dashboard, idDashboard.ToString), InteractionType.UserWithLearningObject)
        End If
    End Sub

    Private Sub HideItems()
        LNBviewAll.Visible = False
        LNBviewLess.Visible = False
        '    DVtileViewMore.Visible = False
        DVviewMoreOrLess.Visible = False
    End Sub
#End Region

#Region "Internal"
    Private Sub RPTtiles_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtiles.ItemDataBound
        Dim dto As dtoTileDisplay = e.Item.DataItem
        Dim oControl As UC_Tile = e.Item.FindControl("CTRLtile")
        oControl.InitializeControl(CurrentTileLayout, dto)
    End Sub
    Private Sub LNBviewAll_Click(sender As Object, e As EventArgs) Handles LNBviewAll.Click
        CurrentPresenter.ShowMoreTiles(CurrentDisplayNoticeboard, IdCurrentDashboard, TileDisplayItems, CurrentTileLayout, AutoUpdateLayout, CurrentGroupItemsBy, True)
    End Sub
    Private Sub LNBviewLess_Click(sender As Object, e As EventArgs) Handles LNBviewLess.Click
        CurrentPresenter.ShowMoreTiles(CurrentDisplayNoticeboard, IdCurrentDashboard, TileDisplayItems, CurrentTileLayout, AutoUpdateLayout, CurrentGroupItemsBy, False)
    End Sub
#End Region

End Class