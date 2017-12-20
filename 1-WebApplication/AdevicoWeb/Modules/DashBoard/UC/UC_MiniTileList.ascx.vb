Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.ActionDataContract
Public Class UC_MiniTileList
    Inherits DBbaseControl
    Implements IViewMiniTileList

#Region "Context"
    Private _Presenter As MiniTileListPresenter
    Private ReadOnly Property CurrentPresenter() As MiniTileListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MiniTileListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewMiniTileList.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Private Property MiniTileDisplayItems As Int32 Implements IViewMiniTileList.MiniTileDisplayItems
        Get
            Return ViewStateOrDefault("MiniTileDisplayItems", 6)
        End Get
        Set(value As Int32)
            ViewState("MiniTileDisplayItems") = value
        End Set
    End Property
    Private Property DisplayLessCommand As Boolean Implements IViewMiniTileList.DisplayLessCommand
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
    Private Property DisplayMoreCommand As Boolean Implements IViewMiniTileList.DisplayMoreCommand
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
    Private Property AutoUpdateLayout As Boolean Implements IViewMiniTileList.AutoUpdateLayout
        Get
            Return ViewStateOrDefault("AutoUpdateLayout", False)
        End Get
        Set(value As Boolean)
            ViewState("AutoUpdateLayout") = value
        End Set
    End Property
    Private Property IdCurrentDashboard As Long Implements IViewMiniTileList.IdCurrentDashboard
        Get
            Return ViewStateOrDefault("IdCurrentDashboard", -1)
        End Get
        Set(value As Long)
            ViewState("IdCurrentDashboard") = value
        End Set
    End Property
    Public Property IdCurrentTile As Long Implements IViewMiniTileList.IdCurrentTile
        Get
            Return ViewStateOrDefault("IdCurrentTile", -1)
        End Get
        Set(value As Long)
            ViewState("IdCurrentTile") = value
        End Set
    End Property
    Public Property IdCurrentTag As Long Implements IViewMiniTileList.IdCurrentTag
        Get
            Return ViewStateOrDefault("IdCurrentTag", -1)
        End Get
        Set(value As Long)
            ViewState("IdCurrentTag") = value
        End Set
    End Property
    Private Property MoreItemsAs As DisplayMoreItems Implements IViewMiniTileList.MoreItemsAs
        Get
            Return ViewStateOrDefault("MoreItemsAs", DisplayMoreItems.AsLink)
        End Get
        Set(value As DisplayMoreItems)
            ViewState("MoreItemsAs") = value
            DVtileViewMore.Visible = False
            DVviewMore.Visible = False
            Select Case value
                Case DisplayMoreItems.AsLink
                    DVviewMore.Visible = True
                Case DisplayMoreItems.AsTile
                    DVtileViewMore.Visible = True
            End Select
        End Set
    End Property
    Private Property CurrentTileLayout As TileLayout Implements IViewMiniTileList.CurrentTileLayout
        Get
            Return ViewStateOrDefault("CurrentTileLayout", TileLayout.grid_4)
        End Get
        Set(value As TileLayout)
            ViewState("CurrentTileLayout") = value
        End Set
    End Property
    Private Property CurrentGroupItemsBy As GroupItemsBy Implements IViewMiniTileList.CurrentGroupItemsBy
        Get
            Return ViewStateOrDefault("CurrentGroupItemsBy", GroupItemsBy.Tile)
        End Get
        Set(value As GroupItemsBy)
            ViewState("CurrentGroupItemsBy") = value
        End Set
    End Property
    Public Property CurrentOrderItemsBy As OrderItemsBy Implements IViewMiniTileList.CurrentOrderItemsBy
        Get
            Return ViewStateOrDefault("CurrentOrderItemsBy", OrderItemsBy.LastAccess)
        End Get
        Set(value As OrderItemsBy)
            ViewState("CurrentOrderItemsBy") = value
        End Set
    End Property
    Private Property CurrentDisplayNoticeboard As DisplayNoticeboard Implements IViewMiniTileList.CurrentDisplayNoticeboard
        Get
            Return ViewStateOrDefault("DisplayMoreCommand", DisplayNoticeboard.OnRight)
        End Get
        Set(value As DisplayNoticeboard)
            ViewState("CurrentDisplayNoticeboard") = value
        End Set
    End Property
    Public Property IsPreview As Boolean Implements IViewMiniTileList.IsPreview
        Get
            Return ViewStateOrDefault("IsPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPreview") = value
            CTRLlistMyCommunities.IsPreview = value
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
            .setLiteral(LTminiTileTitle)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitalizeControlForTile(pageSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idDashboard As Long, moreTiles As Boolean, moreCommunities As Boolean, Optional idPreloadTile As Long = -1) Implements IViewMiniTileList.InitalizeControlForTile
        CurrentPresenter.InitView(pageSettings, userSettings, items, idDashboard, moreTiles, moreCommunities, idPreloadTile)
    End Sub
    Private Sub DisplayErrorFromDB() Implements IViewMiniTileList.DisplayErrorFromDB
        HideItems()
    End Sub
    Private Sub DisplaySessionTimeout(ByVal url As String) Implements IViewMiniTileList.DisplaySessionTimeout
        HideItems()
        RaiseEvent SessionTimeout(url)
    End Sub
    Private Sub LoadDashboard(url As String) Implements IViewMiniTileList.LoadDashboard
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Sub DisplayUnableToLoadTile(url As String) Implements IViewMiniTileList.DisplayUnableToLoadTile
        If Not IsPreview Then
            PageUtility.RedirectToUrl(url)
        End If
    End Sub
    Private Sub LoadMiniTiles(items As List(Of dtoTileDisplay)) Implements IViewMiniTileList.LoadMiniTiles

        DVminiTiles.Visible = items.Any()

        RPTminiTiles.DataSource = items
        RPTminiTiles.DataBind()
    End Sub
    Private Sub UpdateUserSettings(settings As UserCurrentSettings) Implements IViewMiniTileList.UpdateUserSettings
        If Not IsPreview Then
            SaveCurrentCookie(settings)
        End If
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idDashboard As Long, action As ModuleDashboard.ActionType) Implements IViewMiniTileList.SendUserAction
        If Not IsPreview Then
            Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Dashboard, idDashboard.ToString), InteractionType.UserWithLearningObject)
        End If
    End Sub
    Private Sub InitializeCommunitiesList(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), moreCommunities As Boolean, tile As dtoTileDisplay) Implements IViewMiniTileList.InitializeCommunitiesList
        If Not IsNothing(tile) Then
            Select Case tile.Type
                Case TileType.CombinedTags, TileType.CommunityTag
                    CTRLlistMyCommunities.Visible = True
                    CTRLlistMyCommunities.TitleCssClass = tile.ImageCssClass
                    CTRLlistMyCommunities.TitleImage = tile.ImageUrl
                    If String.IsNullOrEmpty(tile.ImageUrl) AndAlso String.IsNullOrEmpty(tile.ImageCssClass) Then
                        CTRLlistMyCommunities.TitleCssClass = LTcssClassDefaultItemClass.Text
                    End If
                    CTRLlistMyCommunities.AutoDisplayTitle = tile.Translation.Title
                    CTRLlistMyCommunities.InitalizeControlForTile(pSettings, userSettings, items, tile)
                Case TileType.CommunityType
                    If Not IsNothing(tile) Then
                        CTRLlistMyCommunities.Visible = True
                        CTRLlistMyCommunities.TitleCssClass = tile.ImageCssClass
                        CTRLlistMyCommunities.TitleImage = tile.ImageUrl
                        If String.IsNullOrEmpty(tile.ImageUrl) AndAlso String.IsNullOrEmpty(tile.ImageCssClass) Then
                            CTRLlistMyCommunities.TitleCssClass = LTcssClassDefaultItemClass.Text
                        End If
                        CTRLlistMyCommunities.AutoDisplayTitle = tile.Translation.Title
                        CTRLlistMyCommunities.InitalizeControlForCommunityType(pSettings, userSettings, items, tile.CommunityTypes.FirstOrDefault())
                    End If
            End Select
        End If

    End Sub
    Private Sub HideItems()
        LNBviewAll.Visible = False
        LNBviewLess.Visible = False
        DVtileViewMore.Visible = False
        DVviewMore.Visible = False
        DVminiTiles.Visible = False
    End Sub
#End Region

#Region "Internal"
    Private Sub RPTminiTiles_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTminiTiles.ItemDataBound

        Dim dto As dtoTileDisplay = e.Item.DataItem
        Dim isCustom As Boolean = Not String.IsNullOrEmpty(TilesVirtualPath) AndAlso Not String.IsNullOrEmpty(dto.ImageUrl)
        Dim oControl As HtmlGenericControl = e.Item.FindControl("DVtile")
        Dim oTileControl As HtmlGenericControl = e.Item.FindControl("DVtitle")
        Dim oCustomControl As HtmlGenericControl = e.Item.FindControl("DVcustom")
        Dim customCssClass As String = ""
        If isCustom Then
            customCssClass = LTcssClassCustomTile.Text
        End If

        oTileControl.Visible = Not isCustom
        oCustomControl.Visible = isCustom

        Dim oLiteral As Literal = Nothing
        If isCustom Then
            oLiteral = e.Item.FindControl("LTcustomTileTitle")
            oLiteral.Text = dto.Translation.Title
            Dim oImage As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGtileIcon")
            oImage.ImageUrl = PageUtility.ApplicationUrlBase & TilesVirtualPath & dto.ImageUrl
        Else
            oLiteral = e.Item.FindControl("LTtileTitle")
            oLiteral.Text = dto.Translation.Title
        End If

        oControl.Attributes("class") = LTcssClassDefaultTile.Text & " " & customCssClass & " " & CurrentTileLayout.ToString

        oLiteral = e.Item.FindControl("LTlinkOpen")
        If String.IsNullOrEmpty(dto.CommandUrl) Then
            oLiteral.Visible = False
            oLiteral = e.Item.FindControl("LTlinkClose")
            oLiteral.Visible = False
        Else
            oLiteral.Text = String.Format(oLiteral.Text, BaseUrl & dto.CommandUrl, dto.Translation.Title)
        End If
    End Sub
    Private Sub LNBviewAll_Click(sender As Object, e As EventArgs) Handles LNBviewAll.Click
        CurrentPresenter.ShowMoreCommunities(IdCurrentDashboard, MiniTileDisplayItems, CurrentTileLayout, AutoUpdateLayout, CurrentGroupItemsBy, CurrentOrderItemsBy, IdCurrentTile, True, CTRLlistMyCommunities.DisplayMoreCommand)
    End Sub
    Private Sub LNBviewLess_Click(sender As Object, e As EventArgs) Handles LNBviewLess.Click
        CurrentPresenter.ShowMoreCommunities(IdCurrentDashboard, MiniTileDisplayItems, CurrentTileLayout, AutoUpdateLayout, CurrentGroupItemsBy, CurrentOrderItemsBy, IdCurrentTile, False, CTRLlistMyCommunities.DisplayMoreCommand)
    End Sub
#Region "Css"
    Public Function GetTitleCssClass(ByVal item As dtoTileDisplay) As String
        If Not String.IsNullOrEmpty(item.ImageCssClass) Then
            Return item.ImageCssClass
        ElseIf Not String.IsNullOrEmpty(item.ImageUrl) Then
            Return ""
        Else
            Return LTcssClassDefaultItemClass.Text
        End If
    End Function
#End Region
#End Region
End Class