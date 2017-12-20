Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class DBpageBasePortalDashboard
    Inherits DBpageBaseDashboardLoader
    Implements IViewPortalDashboard

#Region "Implements"
#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdTile As Long Implements IViewPortalDashboard.PreloadIdTile
        Get
            If IsNumeric(Me.Request.QueryString("idTile")) Then
                Return CLng(Me.Request.QueryString("idTile"))
            Else
                Return -1
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdTag As Long Implements IViewPortalDashboard.PreloadIdTag
        Get
            If IsNumeric(Me.Request.QueryString("idTag")) Then
                Return CLng(Me.Request.QueryString("idTag"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property LoadFromUrl As Boolean Implements IViewPortalDashboard.LoadFromUrl
        Get
            Dim value As String = Request.QueryString("lfu")
            If Not String.IsNullOrEmpty(value) Then
                value = value.ToLower.Trim
            End If
            Return (value = "true")
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadMoreTiles As Boolean Implements IViewPortalDashboard.PreloadMoreTiles
        Get
            Dim value As String = Request.QueryString("mt")
            If Not String.IsNullOrEmpty(value) Then
                value = value.ToLower.Trim
            End If
            Return (value = "true")
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadMoreCommunities As Boolean Implements IViewPortalDashboard.PreloadMoreCommunities
        Get
            Dim value As String = Request.QueryString("mc")
            If Not String.IsNullOrEmpty(value) Then
                value = value.ToLower.Trim
            End If
            Return (value = "true")
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadSearch As DisplaySearchItems Implements IViewPortalDashboard.PreloadSearch
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DisplaySearchItems).GetByString(Request.QueryString("s"), DisplaySearchItems.Simple)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadSearchText As String Implements IViewPortalDashboard.PreloadSearchText
        Get
            Return Request.QueryString("t")
        End Get
    End Property
#End Region

    Protected Friend Property CurrentLayout As PlainLayout Implements IViewPortalDashboard.CurrentLayout
        Get
            Return ViewStateOrDefault("CurrentLayout", PlainLayout.ignore)
        End Get
        Set(value As PlainLayout)
            ViewState("CurrentLayout") = value
        End Set
    End Property
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

#Region "Implements"
    Private Sub LoadDashboard(url As String) Implements IViewDefaultDashboardLoader.LoadDashboard
        If Not String.IsNullOrEmpty(url) Then
            PageUtility.RedirectToUrl(url)
        Else
            PageUtility.RedirectToUrl("modules/Dashboard/portal.aspx")
        End If
    End Sub
    Private Sub InitalizeTopBar(settings As liteDashboardSettings, userSettings As UserCurrentSettings, moreTiles As Boolean, Optional ByVal searchBy As String = "") Implements IViewPortalDashboard.InitalizeTopBar
        Dim oControl As UC_DashboardTopBar = GetTopBarControl()
        If Not IsNothing(oControl) Then
            oControl.InitalizeControl(GetCurrentView, settings, userSettings, moreTiles, searchBy)
        End If
    End Sub
    Protected Friend MustOverride Sub EnableFullWidth(value As Boolean) Implements IViewPortalDashboard.EnableFullWidth
    Protected Friend MustOverride Sub InitializeLayout(layout As PlainLayout, display As DisplayNoticeboard) Implements IViewPortalDashboard.InitializeLayout
    Protected Friend MustOverride Sub InitializeCommunitiesList(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy))) Implements IViewPortalDashboard.InitializeCommunitiesList
    Protected Friend MustOverride Sub IntializeCombinedView(pSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idDashboard As Long, ByVal moreTiles As Boolean, ByVal moreCommunities As Boolean, Optional idPreloadTile As Long = -1) Implements IViewPortalDashboard.IntializeCombinedView
    Protected Friend MustOverride Sub IntializeTileView(idCommunity As Integer, noticeboard As DisplayNoticeboard, pSettings As litePageSettings, userSettings As UserCurrentSettings, idDashboard As Long, moreTiles As Boolean) Implements IViewPortalDashboard.IntializeTileView

#End Region

#Region "MustOverride"
    Protected Friend MustOverride Function GetTopBarControl() As UC_DashboardTopBar
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
#End Region




End Class