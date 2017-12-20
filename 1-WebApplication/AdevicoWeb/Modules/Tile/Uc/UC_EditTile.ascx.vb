Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tiles.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Tiles.Domain

Public Class UC_EditTile
    Inherits TGbaseControl
    Implements IViewTileEdit

#Region "Context"
    Private _presenter As TileEditPresenter
    Protected Friend ReadOnly Property CurrentPresenter As TileEditPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TileEditPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Protected Friend Property IdTile As Long Implements IViewTileEdit.IdTile
        Get
            Return ViewStateOrDefault("IdTile", 0)
        End Get
        Set(value As Long)
            ViewState("IdTile") = value
        End Set
    End Property
    Protected Friend Property IdTileCommunity As Integer Implements IViewTileEdit.IdTileCommunity
        Get
            Return ViewStateOrDefault("IdTileCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTileCommunity") = value
        End Set
    End Property
    Protected Friend Property IdContainerCommunity As Integer Implements IViewTileEdit.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdContainerCommunity") = value
        End Set
    End Property
    Protected Friend Property CurrentType As TileType Implements IViewTileEdit.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", IIf(IdContainerCommunity = 0, TileType.DashboardUserDefined, TileType.UserDefined))
        End Get
        Set(value As TileType)
            ViewState("CurrentType") = value
            ReloadType(value)
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event SessionTimeout()
    Public Event EventMessage(ByVal action As lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType, idTile As Long, idCommunity As Integer, idModule As Integer)
    Public Event EventErrorMessage(ByVal action As lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType, idTile As Long, idCommunity As Integer, idModule As Integer, errorType As lm.Comol.Core.BaseModules.Tiles.Business.ErrorMessageType)
    Public Event EventRedirectToEdit(idCommunity As Integer, idModule As Integer, idTile As Long, action As ModuleDashboard.ActionType)
    Public ReadOnly Property TilesVirtualPath As String
        Get
            Return SystemSettings.File.Tiles.VirtualPath
        End Get
    End Property
    Private _TilesPath As String
    Private ReadOnly Property TilesFilesPath As String
        Get
            If String.IsNullOrWhiteSpace(_TilesPath) Then
                If (Not SystemSettings.File.Tiles.isInvalid) Then
                    If Not String.IsNullOrWhiteSpace(SystemSettings.File.Tiles.DrivePath) Then
                        _TilesPath = SystemSettings.File.Tiles.DrivePath
                    ElseIf Not String.IsNullOrWhiteSpace(TilesVirtualPath) Then
                        _TilesPath = Server.MapPath(PageUtility.BaseUrl & TilesVirtualPath)
                    End If
                End If
            End If

            Return _TilesPath
        End Get
    End Property
    Public ReadOnly Property UploadButtonClientId As String
        Get
            Return CTRLuploader.UploadButtonClientId
        End Get
    End Property
    Public ReadOnly Property MaxFileSize As String
        Get
            Return CTRLuploader.GetMaxFileSize
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBtileType_t)
            .setLabel(LBtileUrl_t)
            .setLabel(LBtileTags_t)
            .setLabel(LBtileCssImages)
            .setLabel(LBtileCssClass_t)
            .setLabel(LBtileUploadedImages)
            .setLabel(LBtileImage_t)
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

#Region "Implements"
    Private Sub DisplaySessionTimeout() Implements IViewTileEdit.DisplaySessionTimeout
        CTRLuploader.Visible = False
        RaiseEvent SessionTimeout()
    End Sub
    Public Sub InitalizeControl(tile As dtoEditTile, idTile As Long, idCommunity As Integer, idTileCommunity As Integer, itemName As String) Implements IViewTileEdit.InitalizeControl
        DVassociatedEntry.Visible = True
        LBtileAssociatedEntry_t.Text = Resource.getValue("LBtileAssociatedEntry_t." & tile.Type.ToString)
        LBtileAssociatedEntry.Text = itemName
        InternalInitializeControl(tile, idTile, idCommunity, idTileCommunity, Nothing, Nothing)
    End Sub
    Public Sub InitalizeControl(tile As dtoEditTile, idTile As Long, idCommunity As Integer, idTileCommunity As Integer, Optional types As List(Of TileType) = Nothing) Implements IViewTileEdit.InitalizeControl
        InternalInitializeControl(tile, idTile, idCommunity, idTileCommunity, Nothing, types)
    End Sub
    Public Sub InitalizeControl(tile As dtoEditTile, idTile As Long, idCommunity As Integer, idTileCommunity As Integer, tags As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Long)), Optional types As List(Of TileType) = Nothing) Implements IViewTileEdit.InitalizeControl
        InternalInitializeControl(tile, idTile, idCommunity, idTileCommunity, tags, types)
    End Sub
    Private Function GetDefaultLanguageName() As String Implements IViewTileEdit.GetDefaultLanguageName
        Return Resource.getValue("GetDefaultLanguageName")
    End Function
    Private Function GetDefaultLanguageCode() As String Implements IViewTileEdit.GetDefaultLanguageCode
        Return Resource.getValue("GetDefaultLanguageCode")
    End Function
    Private Sub ReloadTags(tags As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Long))) Implements IViewTileEdit.ReloadTags
        DVtagsSelector.Visible = True
        If SLtags.Items.Count = 0 Then
            SLtags.Attributes.Add("data-placeholder", Resource.getValue("SelectTags.data-placeholder"))
            SLtags.DataSource = tags
            SLtags.DataTextField = "Translation"
            SLtags.DataValueField = "Id"
            SLtags.DataBind()
        End If
    End Sub
    Private Sub ReloadType(type As TileType) Implements IViewTileEdit.ReloadType
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
                DDLstandardAction.Visible = True
                TXBurl.Visible = False

                LBtileUrl_t.AssociatedControlID = DDLstandardAction.ID
            Case TileType.UserDefined
                DVactions.Visible = True

                DVurl.Visible = True
        End Select
    End Sub
    Private Sub LoadTranslations(languages As List(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem), translations As List(Of dtoTileFullTranslation)) Implements IViewTileEdit.LoadTranslations
        RPTlanguages.DataSource = languages
        RPTlanguages.DataBind()

        RPTtranslations.DataSource = translations
        RPTtranslations.DataBind()
    End Sub
    Private Sub LoadCssAndImages(cssClasses As List(Of dtoItemFilter(Of String)), images As List(Of dtoItemFilter(Of String)), allowUpload As Boolean) Implements IViewTileEdit.LoadCssAndImages
        If cssClasses.Any AndAlso cssClasses.Count > 0 Then
            RPTcssClass.DataSource = cssClasses
            RPTcssClass.DataBind()
            DVcssGallery.Visible = True
        Else
            DVcssGallery.Visible = False
        End If
        CTRLuploader.Visible = allowUpload
        LoadImages(images)
    End Sub
    Private Sub DisplayMessage(idCommunity As Integer, idModule As Integer, idTile As Long, action As ModuleDashboard.ActionType) Implements IViewTileEdit.DisplayMessage
        RaiseEvent EventMessage(action, idTile, idCommunity, idModule)
    End Sub
    Private Sub DisplayMessage(idCommunity As Integer, idModule As Integer, idTile As Long, action As ModuleDashboard.ActionType, errorType As lm.Comol.Core.BaseModules.Tiles.Business.ErrorMessageType) Implements IViewTileEdit.DisplayMessage
        RaiseEvent EventErrorMessage(action, idTile, idCommunity, idModule, errorType)
    End Sub
    Public Sub SetStatus(idTile As Long, status As AvailableStatus) Implements IViewTileEdit.SetStatus
        CurrentPresenter.SetStatus(idTile, status)
    End Sub
    Private Sub UpdateStatus(deleted As lm.Comol.Core.DomainModel.BaseStatusDeleted, status As AvailableStatus) Implements IViewTileEdit.UpdateStatus
        Select Case deleted
            Case lm.Comol.Core.DomainModel.BaseStatusDeleted.None
                LBtileStatus.Text = Resource.getValue("LBtileStatus." & status.ToString)
            Case Else
                LBtileStatus.Text = Resource.getValue("LBtileStatus.Deleted")
        End Select
    End Sub
    Public Sub VirtualDelete(idTile As Long, delete As Boolean) Implements IViewTileEdit.VirtualDelete
        CurrentPresenter.VirtualDelete(idTile, delete)
    End Sub
    Private Sub LoadImages(images As List(Of dtoItemFilter(Of String))) Implements IViewTileEdit.LoadImages
        RPTimages.DataSource = images
        RPTimages.DataBind()
    End Sub
    Public Sub Save() Implements IViewTileEdit.Save
        Dim path As String = TilesFilesPath
        CTRLuploader.TemporaryFolder = path & "\Temporary\"
        CTRLuploader.TargetFolder = path & "\Parsing\"
        CurrentPresenter.Save(IdTile, GetTile, Server.MapPath(PageUtility.BaseUrl & LTtileCssFilePath.Text) & LTtileCssFile.Text, path)
    End Sub

    Public Function GetTile() As dtoEditTile Implements IViewTileEdit.GetTile
        Dim dto As New dtoEditTile()
        Dim translations As New Dictionary(Of Integer, Dictionary(Of TileItemType, String))
        Dim hasSubitems As Boolean = (CurrentType = TileType.Module OrElse CurrentType = TileType.Module)

        With dto
            .Id = IdTile
            '.AutoNavigateUrl 
            '.Deleted 
            ' .HasModulePage 
            .IdCommunity = IdTileCommunity
            '.IdCommunityTypes 
            '.IdModulePage 
            .ImageCssClass = TXBtileCssClass.Text
            .ImageUrl = HDimage.Value
            .NavigateUrl = TXBurl.Text
            .Type = CurrentType
            .IdTags = New List(Of Long)
            If SLtags.Items.Count > 0 Then
                .IdTags = (From i As ListItem In SLtags.Items Where i.Selected Select CLng(i.Value)).ToList()
            End If

            For Each row As RepeaterItem In RPTtranslations.Items
                Dim oLiteral As Literal = row.FindControl("LTidLanguage")
                Dim idLanguage As Integer = CInt(oLiteral.Text)

                Dim translation As New lm.Comol.Core.DomainModel.Languages.dtoBaseObjectTranslation
                Dim oTextBox As TextBox = row.FindControl("TXBname")
                translation.IdLanguage = idLanguage
                translation.Translation.Title = oTextBox.Text

                oTextBox = row.FindControl("TXBdescription")
                translation.Translation.Description = oTextBox.Text
                .Translations.Add(translation)

                If hasSubitems Then
                    Dim t As New Dictionary(Of TileItemType, String)
                    t.Add(TileItemType.NewUrl, DirectCast(row.FindControl("TXBnew"), TextBox).Text)
                    t.Add(TileItemType.StatisticUrl, DirectCast(row.FindControl("TXBstat"), TextBox).Text)
                    t.Add(TileItemType.SettingsUrl, DirectCast(row.FindControl("TXBsettings"), TextBox).Text)
                    translations.Add(idLanguage, t)
                End If
            Next

            If hasSubitems Then
                Dim item As New lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTileItem
                item.Type = TileItemType.NewUrl
                item.NavigateUrl = TXBactionNew.Text
                item.IdModulePage = DDLactionNew.SelectedValue
                item.ToolTip = translations(0)(TileItemType.NewUrl)
                item.Translations = translations.Where(Function(t) t.Key <> 0).Select(Function(t) New dtoToolTipLanguageItem() With {.IdLanguage = t.Key, .ToolTip = t.Value(TileItemType.NewUrl)}).ToList
                'CBLactionNewPermissions
                .SubItems.Add(item)


                item = New lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTileItem
                item.Type = TileItemType.StatisticUrl
                item.NavigateUrl = TXBactionStat.Text
                item.IdModulePage = DDLactionStat.SelectedValue
                item.ToolTip = translations(0)(TileItemType.NewUrl)
                item.Translations = translations.Where(Function(t) t.Key <> 0).Select(Function(t) New dtoToolTipLanguageItem() With {.IdLanguage = t.Key, .ToolTip = t.Value(TileItemType.StatisticUrl)}).ToList
                'CBLactionStatPermissions
                .SubItems.Add(item)

                item = New lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTileItem
                item.Type = TileItemType.SettingsUrl
                item.NavigateUrl = TXBactionSettings.Text
                item.IdModulePage = DDLactionSettings.SelectedValue
                item.ToolTip = translations(0)(TileItemType.NewUrl)
                item.Translations = translations.Where(Function(t) t.Key <> 0).Select(Function(t) New dtoToolTipLanguageItem() With {.IdLanguage = t.Key, .ToolTip = t.Value(TileItemType.SettingsUrl)}).ToList
                'CBLactionSettingsPermissions
                .SubItems.Add(item)

            End If
        End With
        Return dto
    End Function
    Private Sub RedirectToEdit(idCommunity As Integer, idModule As Integer, idTile As Long, action As ModuleDashboard.ActionType) Implements IViewTileEdit.RedirectToEdit
        RaiseEvent EventRedirectToEdit(idCommunity, idModule, idTile, action)
    End Sub
#End Region

#Region "Internal"
    Private Sub InternalInitializeControl(tile As dtoEditTile, idItem As Long, idCommunity As Integer, idTileCommunity As Integer, tags As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Long)), types As List(Of TileType))
        IdTile = idItem
        idTileCommunity = idTileCommunity
        IdContainerCommunity = idCommunity

        TXBurl.Text = tile.NavigateUrl
        TXBtileCssClass.Text = tile.ImageCssClass
        HDimage.Value = tile.ImageUrl

        InitalizeView(tile.Type, tile.IdTags, tags, types)
        Dim path As String = TilesFilesPath
        CTRLuploader.TemporaryFolder = path & "\Temporary\"
        CTRLuploader.TargetFolder = path & "\Parsing\"
        UpdateStatus(tile.Deleted, tile.Status)
        CurrentPresenter.InitView(tile, Server.MapPath(PageUtility.BaseUrl & LTtileCssFilePath.Text) & LTtileCssFile.Text, path)
    End Sub
    Private Sub InitalizeView(type As TileType, selectedTags As List(Of Long), tags As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Long)), types As List(Of TileType))

        DDLstandardAction.Visible = False
        RBLtype.Visible = Not IsNothing(types)
        LBtileType.Visible = IsNothing(types)
        LBtileType.Text = Resource.getValue("RBLtype.TileType." & type.ToString)
        If Not IsNothing(types) Then
            RBLtype.DataSource = (From t As TileType In types Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(t), .Translation = Resource.getValue("RBLtype.TileType." & t.ToString)}).ToList()
            RBLtype.DataValueField = "Id"
            RBLtype.DataTextField = "Translation"
            RBLtype.DataBind()
            RBLtype.SelectedValue = CInt(type)
        End If

        Select Case type
            Case TileType.CombinedTags
                ReloadTags(tags)
                If selectedTags.Any() Then
                    For Each idTag As Long In selectedTags
                        Dim oListItem As ListItem = SLtags.Items.FindByValue(idTag)
                        If Not IsNothing(oListItem) Then
                            oListItem.Selected = True
                        End If
                    Next
                End If
        End Select
        ReloadType(type)
    End Sub
    Private Sub RBLtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBLtype.SelectedIndexChanged
        CurrentPresenter.ChangeType(CInt(RBLtype.SelectedValue))
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
    Private Sub CTRLuploader_UploadedFiles(files As Telerik.Web.UI.UploadedFileCollection) Handles CTRLuploader.UploadedFiles
        CurrentPresenter.UploadTileImage(TilesFilesPath, CTRLuploader.TargetFolder & files(0).FileName, files(0).GetExtension)
    End Sub
    Public Function GetItemCssClass(item As dtoItemFilter(Of String)) As String
        Dim cssClass As String = "" ' GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssClassActive.Text
        End If
        Return cssClass
    End Function
#End Region

End Class