Imports lm.Comol.Core.BaseModules.Tiles.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class ViewTile
    Inherits TLeditPageBase
    Implements IViewTile

#Region "Context"
    Private _presenter As ViewTilePresenter
    Protected Friend ReadOnly Property CurrentPresenter As ViewTilePresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New ViewTilePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadIdTile, PreloadDashboardType, PreloadIdCommunity)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPgoTo_TileListTop, False, True)
            .setHyperLink(HYPgoTo_TileListBottom, False, True)
            Master.ServiceTitle = .getValue("Tile.ViewTile")
            Master.ServiceNopermission = .getValue("Tile.ViewTileNoPermission")
        End With
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.TileEdit(PreloadIdTile, PreloadDashboardType, PreloadIdCommunity, False, PreloadIdDashboard, PreloadStep), IdContainerCommunity)
    End Sub

    Protected Friend Overrides Function GetBackUrlItemBottom() As HyperLink
        Return HYPgoTo_TileListBottom
    End Function
    Protected Friend Overrides Function GetBackUrlItemTop() As HyperLink
        Return HYPgoTo_TileListTop
    End Function

    Protected Friend Overrides Function GetMessageControl() As UC_ActionMessages
        Return CTRLmessages
    End Function
#End Region

#Region "Implements"
    Private Function GetDefaultLanguageName() As String Implements IViewTile.GetDefaultLanguageName
        Return Resource.getValue("GetDefaultLanguageName")
    End Function
    Private Function GetDefaultLanguageCode() As String Implements IViewTile.GetDefaultLanguageCode
        Return Resource.getValue("GetDefaultLanguageCode")
    End Function
    Private Sub LoadTile(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, itemName As String, languages As List(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem), translations As List(Of lm.Comol.Core.BaseModules.Tiles.Domain.dtoTileFullTranslation)) Implements IViewTile.LoadTile
        CTRLview.InitializeControl(tile, itemName, languages, translations)
    End Sub
    Private Sub LoadTile(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, languages As List(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem), translations As List(Of lm.Comol.Core.BaseModules.Tiles.Domain.dtoTileFullTranslation)) Implements IViewTile.LoadTile
        CTRLview.InitializeControl(tile, languages, translations)
    End Sub
    Private Sub LoadTile(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, tags As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Long)), languages As List(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem), translations As List(Of lm.Comol.Core.BaseModules.Tiles.Domain.dtoTileFullTranslation)) Implements IViewTile.LoadTile
        CTRLview.InitializeControl(tile, tags, languages, translations)
    End Sub
#End Region

#Region "Internal"
    Private Sub AddTile_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
    Private Sub CTRLview_SessionTimeout() Handles CTRLview.SessionTimeout
        DisplaySessionTimeout()
    End Sub
#End Region

End Class