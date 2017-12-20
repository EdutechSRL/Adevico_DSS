Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tiles.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Tiles.Domain

Public Class UC_ViewTile
    Inherits TGbaseControl

#Region "Internal"
    Public Event SessionTimeout()
    Public ReadOnly Property TilesVirtualPath As String
        Get
            Return SystemSettings.File.Tiles.VirtualPath
        End Get
    End Property
    Private _TilesPath As String
    Private ReadOnly Property TilesFilesPath As String
        Get
            If String.IsNullOrEmpty(_TilesPath) Then
                If Not String.IsNullOrEmpty(SystemSettings.File.Tiles.DrivePath) Then
                    _TilesPath = SystemSettings.File.Tiles.DrivePath
                Else
                    _TilesPath = Server.MapPath(PageUtility.BaseUrl & TilesVirtualPath)
                End If
            End If

            Return _TilesPath
        End Get
    End Property
    Private Property CurrentType As TileType
        Get
            Return ViewStateOrDefault("CurrentType", TileType.None)
        End Get
        Set(value As TileType)
            ViewState("CurrentType") = value
            ReloadType(value)
        End Set
    End Property
    Protected Friend Property TileCssClass As String
        Get
            Return ViewStateOrDefault("TileCssClass", "generic")
        End Get
        Set(value As String)
            ViewState("TileCssClass") = value
        End Set
    End Property
    Protected Friend Property TileImage As String
        Get
            Return ViewStateOrDefault("TileImage", "")
        End Get
        Set(value As String)
            ViewState("TileImage") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBtileType_t)
            .setLabel(LBtileUrl_t)
            .setLabel(LBtileSelectedTags_t)
            .setLabel(LBtileCssImages)
            .setLabel(LBtileCssClass_t)
            .setLabel(LBtileUploadedImages)
            .setLabel(LBactionNew_t)
            .setLabel(LBactionNewPageSelector_t)
            .setLabel(LBactionNewPermissions_t)
            .setLabel(LBactionStat_t)
            .setLabel(LBactionStatPageSelector_t)
            .setLabel(LBactionStatPagePermissions_t)
            .setLabel(LBactionSettings_t)
            .setLabel(LBactionSettingsPageSelector_t)
            .setLabel(LBactionSettingsPagePermissions_t)
            .setLabel(LBtileStatus_t)
            .setLiteral(LTeditTileTitleActions)
            .setLiteral(LTeditTileTitleSettings)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, itemName As String, languages As List(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem), translations As List(Of lm.Comol.Core.BaseModules.Tiles.Domain.dtoTileFullTranslation))
        DVassociatedEntry.Visible = True
        LBtileAssociatedEntry_t.Text = Resource.getValue("LBtileAssociatedEntry_t." & tile.Type.ToString)
        LBtileAssociatedEntry.Text = itemName
        InitializeControl(tile, languages, translations)
    End Sub
    Public Sub InitializeControl(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, tags As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Long)), languages As List(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem), translations As List(Of lm.Comol.Core.BaseModules.Tiles.Domain.dtoTileFullTranslation))
        DVtagsSelector.Visible = True
        LBtileSelectedTags.Text = String.Join(",", tags.Where(Function(t) tile.IdTags.Contains(t.Id)).Select(Function(t) t.Translation).ToList())
        InitializeControl(tile, languages, translations)
    End Sub
    Public Sub InitializeControl(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, languages As List(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem), translations As List(Of lm.Comol.Core.BaseModules.Tiles.Domain.dtoTileFullTranslation))
        LBtileUrl.Text = tile.NavigateUrl
        CurrentType = tile.Type
        RPTlanguages.DataSource = languages
        RPTlanguages.DataBind()

        RPTtranslations.DataSource = translations
        RPTtranslations.DataBind()

        TXBtileCssClass.Text = tile.ImageCssClass
        TileCssClass = tile.ImageCssClass
        TileImage = tile.ImageUrl
        UpdateStatus(tile.Deleted, tile.Status)
        LBtileType.Text = Resource.getValue("RBLtype.TileType." & tile.Type.ToString)
        If Not String.IsNullOrEmpty(tile.ImageUrl) Then
            DVimage.Visible = True
            DVcssClass.Visible = False
        Else
            DVimage.Visible = False
            DVcssClass.Visible = True
            If String.IsNullOrEmpty(tile.ImageCssClass) Then
                tile.ImageCssClass = LTcssClassGeneric.Text
                TileCssClass = LTcssClassGeneric.Text
            End If
        End If
    End Sub
    Private Sub UpdateStatus(deleted As lm.Comol.Core.DomainModel.BaseStatusDeleted, status As AvailableStatus)
        Select Case deleted
            Case lm.Comol.Core.DomainModel.BaseStatusDeleted.None
                LBtileStatus.Text = Resource.getValue("LBtileStatus." & status.ToString)
            Case Else
                LBtileStatus.Text = Resource.getValue("LBtileStatus.Deleted")
        End Select
    End Sub
    Private Sub ReloadType(type As TileType)
        DVurl.Visible = False
        DVactions.Visible = False
        DVtagsSelector.Visible = False
        Select Case type
            Case TileType.CombinedTags
                DVtagsSelector.Visible = True
            Case TileType.CommunityTag

            Case TileType.CommunityType

            Case TileType.DashboardUserDefined
                DVurl.Visible = True
            Case TileType.Module
                DVactions.Visible = True
                'DDLstandardAction.Visible = True
                'TXBurl.Visible = False

                'LBtileUrl_t.AssociatedControlID = DDLstandardAction.ID
            Case TileType.UserDefined
                DVactions.Visible = True

                DVurl.Visible = True
        End Select
    End Sub
    Private Sub DisplaySessionTimeout()
        RaiseEvent SessionTimeout()
    End Sub
    Private Sub RPTtranslations_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtranslations.ItemDataBound
        Dim item As dtoTileFullTranslation = DirectCast(e.Item.DataItem, dtoTileFullTranslation)

        Dim oLabel As Label = e.Item.FindControl("LBtileTranslationName_t")
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBtileTranslationDescription_t")
        Resource.setLabel(oLabel)

        Dim oControl As HtmlGenericControl = e.Item.FindControl("DVactions")
        If CurrentType = TileType.UserDefined OrElse CurrentType = TileType.Module Then
            oControl.Visible = True
            oLabel = e.Item.FindControl("LBtileTranslationActions")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtileActionNew")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtileActionStat")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtileActionSettings")
            Resource.setLabel(oLabel)
        End If
    End Sub
#End Region

End Class