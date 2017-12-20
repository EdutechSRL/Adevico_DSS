Public Class TileRecycleBin
    Inherits TLpageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Not Page.IsPostBack Then
            CTRLheader.SetTransacionIdContainer(Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID & "_" & PageUtility.CurrentContext.UserContext.WorkSessionID.ToString)
        End If
        CurrentPresenter.InitView(PreloadDashboardType, True, PreloadIdCommunity, PreloadIdDashboard, PreloadStep)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
        Master.ServiceNopermission = Resource.getValue("Tiles.ServiceTitle.RecycleBin.NoPermission")
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.TileList(PreloadDashboardType, True, IdTilesCommunity, PreloadIdTile, PreloadIdDashboard, PreloadStep), IdContainerCommunity)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("Tiles.ServiceTitle.RecycleBin")
            .setHyperLink(HYPgoTo_TileList, False, True)

        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem() As HyperLink
        Return HYPgoTo_TileList
    End Function
    Protected Friend Overrides Function GetDashboardSettingsBackUrlItem() As HyperLink
        Resource.setHyperLink(HYPgotoDashboardSettings, False, True)
        Return HYPgotoDashboardSettings
    End Function
    Protected Friend Overrides Function GetListControl() As UC_TileList
        Return CTRLtiles
    End Function
    Protected Friend Overrides Function GetRecycleUrlItem() As HyperLink
        Return Nothing
    End Function
    Protected Friend Overrides Function GetAddButton() As HyperLink
        Return Nothing
    End Function
    Protected Friend Overrides Function GetListControlHeader() As UC_TileListHeader
        Return CTRLheader
    End Function
    Protected Friend Overrides WriteOnly Property AllowCommunityTypesTileAutoGenerate As Boolean
        Set(value As Boolean)

        End Set
    End Property
#End Region

#Region "Internal"
    Private Sub TagRecycleBin_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True

    End Sub
    Private Sub CTRLtiles_LanguageChanged(idLanguage As Integer) Handles CTRLtiles.LanguageChanged
        CTRLheader.FilterIdLanguage = idLanguage
    End Sub
    Private Sub CTRLtiles_SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Handles CTRLtiles.SetDefaultFilters
        CTRLheader.SetDefaultFilters(filters)
    End Sub
    Private Sub CTRLtiles_SessionTimeout() Handles CTRLtiles.SessionTimeout
        DisplaySessionTimeout()
    End Sub
#End Region
End Class