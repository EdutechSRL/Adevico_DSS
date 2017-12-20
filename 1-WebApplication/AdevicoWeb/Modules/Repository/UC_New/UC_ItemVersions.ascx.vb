Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.ActionDataContract
Public Class UC_ItemVersions
    Inherits FRbaseControl
    Implements IViewItemVersions

#Region "Context"
    Private _Presenter As ItemVersionsPresenter
    Public ReadOnly Property CurrentPresenter() As ItemVersionsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ItemVersionsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdRepositoryItem As Long Implements IViewItemVersions.IdRepositoryItem
        Get
            Return ViewStateOrDefault("IdRepositoryItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdRepositoryItem") = value
        End Set
    End Property
    Private Property RepositoryItemType As ItemType Implements IViewItemVersions.RepositoryItemType
        Get
            Return ViewStateOrDefault("RepositoryItemType", ItemType.File)
        End Get
        Set(value As ItemType)
            ViewState("RepositoryItemType") = value
        End Set
    End Property
    Private Function GetFolderTypeTranslation() As Dictionary(Of FolderType, String) Implements IViewItemVersions.GetFolderTypeTranslation
        Return (From e As FolderType In [Enum].GetValues(GetType(FolderType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.FolderType." & e.ToString))
    End Function
    Private Function GetTypesTranslations() As Dictionary(Of ItemType, String) Implements IViewItemVersions.GetTypesTranslations
        Return (From e As ItemType In [Enum].GetValues(GetType(ItemType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.ItemType." & e.ToString))
    End Function
    Private Function GetUnknownUserName() As String Implements IViewItemVersions.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Function GetRootFolderFullname() As String Implements IViewItemVersions.GetRootFolderFullname
        Return Resource.getValue("RootFolder")
    End Function
#End Region

#Region "Internal"
    Private _ShowActions As Boolean
    Public Event DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
    Public Event GenericVersionUpdated()
    Public Event VersionUpdated(item As dtoDisplayRepositoryItem, thumbnailWidth As Integer, thumbnailHeight As Integer, allowedExtensionsForPreview As String)

    Public Event SessionTimeout()
    Private Property EditMode As Boolean
        Get
            Return ViewStateOrDefault("EditMode", False)
        End Get
        Set(value As Boolean)
            ViewState("EditMode") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTversionSection)
            .setLiteral(LTversionName_t)
            .setLiteral(LTversionSize_t)
            .setLiteral(LTversionDate_t)
            .setLiteral(LTversionAuthor_t)
            .setButton(BTNaddVersionToItem, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(edit As Boolean, item As dtoDisplayRepositoryItem) Implements IViewItemVersions.InitializeControl
        InitializeRepositoryPath(item.Repository)
        IdRepositoryItem = item.Id
        RepositoryItemType = item.Type
        EditMode = edit
        CurrentPresenter.InitView(edit, item, GetUnknownUserName, GetRepositoryDiskPath)
    End Sub

    Private Sub DisplayUserMessage(messageType As UserMessageType) Implements IViewItemVersions.DisplayUserMessage
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case messageType
            Case UserMessageType.versionAdded, UserMessageType.versionVirtualDeleted, UserMessageType.versionPhisicalDeleted, UserMessageType.versionPromoted
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        RaiseEvent DisplayMessage(Resource.getValue("UserMessageType." & messageType.ToString), mType)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRepository.ActionType, idItem As Long, objType As ModuleRepository.ObjectType) Implements IViewItemVersions.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objType, idItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub LoadVersions(items As List(Of dtoDisplayVersionItem)) Implements IViewItemVersions.LoadVersions
        _ShowActions = EditMode AndAlso items.Any(Function(i) i.Permissions.SetActive)
        THactions.Visible = _ShowActions
        THsize.Visible = Not items.Any(Function(f) f.Type = ItemType.Link)
        RPTversions.DataSource = items
        RPTversions.DataBind()
        BTNaddVersionToItem.Visible = False
    End Sub
    Private Sub LoadVersions(items As List(Of dtoDisplayVersionItem), item As dtoDisplayRepositoryItem, quota As dtoContainerQuota) Implements IViewItemVersions.LoadVersions
        _ShowActions = EditMode AndAlso items.Any(Function(i) i.Permissions.SetActive)
        THactions.Visible = _ShowActions
        THsize.Visible = item.Type <> ItemType.Link
        RPTversions.DataSource = items
        RPTversions.DataBind()
        BTNaddVersionToItem.Visible = True
        CTRLaddVersion.Visible = True
        CTRLaddVersion.InitializeControl(item, quota)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewItemVersions.DisplaySessionTimeout
        BTNaddVersionToItem.Visible = False
        RaiseEvent SessionTimeout()
        For Each row As RepeaterItem In RPTversions.Items
            Dim cell As HtmlControls.HtmlTableCell = DirectCast(row.FindControl("TDactions"), HtmlTableCell)
            cell.Visible = False
        Next
        THactions.Visible = False
    End Sub

    Private Sub CurrentVersionUpdated() Implements IViewItemVersions.CurrentVersionUpdated
        RaiseEvent GenericVersionUpdated()
    End Sub
    Private Sub CurrentVersionUpdated(item As dtoDisplayRepositoryItem, thumbnailWidth As Integer, thumbnailHeight As Integer, allowedExtensionsForPreview As String) Implements IViewItemVersions.CurrentVersionUpdated
        RaiseEvent VersionUpdated(item, thumbnailWidth, thumbnailHeight, allowedExtensionsForPreview)
    End Sub
    Private Sub NotifyAddedVersion(idModule As Integer, idFolder As Long, folderName As String, folderUrl As String, addedVersion As dtoCreatedItem) Implements IViewItemVersions.NotifyAddedVersion
        Dim oSender As New RepositoryCommunityNewsUtility(idModule, PageUtility)
        Dim backUrl As String = folderUrl
        backUrl = Replace(backUrl, PageUtility.ApplicationUrlBase(True), "")
        backUrl = Replace(backUrl, PageUtility.ApplicationUrlBase(False), "")
        If Not folderUrl.StartsWith(PageUtility.ApplicationUrlBase(True)) AndAlso Not folderUrl.StartsWith(PageUtility.ApplicationUrlBase(False)) Then
            folderUrl = PageUtility.ApplicationUrlBase & folderUrl
        End If
        Dim permissions As Integer = IIf(addedVersion.Added.IsVisible, oSender.PermissionToSee, oSender.PermissionToAdmin)
        oSender.ItemAdded(addedVersion.Added.Repository.IdCommunity, addedVersion.Added.Id, addedVersion.Added.IdVersion, addedVersion.Added.Type, addedVersion.Added.DisplayName, folderName, GetItemDownloadOrPlayUrl(addedVersion.Added, True, Server.HtmlEncode(backUrl)), folderUrl, permissions)
    End Sub
#End Region

#Region "internal"
    Private Sub CTRLaddVersion_AddVersion(idItem As Long, file As dtoUploadedItem) Handles CTRLaddVersion.AddVersion
        CurrentPresenter.AddVersionToFile(SystemSettings.NotificationErrorService.ComolUniqueID, idItem, file, GetUnknownUserName, GetRepositoryDiskPath)
    End Sub
    Private Sub RPTversions_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTversions.ItemCommand
        Dim idVersion As Long = 0
        Long.TryParse(e.CommandArgument, idVersion)
        If idVersion > 0 Then
            CurrentPresenter.ExecuteAction(IdRepositoryItem, idVersion, ItemAction.addVersion, GetUnknownUserName, GetRepositoryDiskPath, GetRepositoryThumbnailDiskPath)
        End If
    End Sub
    Private Sub RPTversions_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTversions.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoDisplayVersionItem = DirectCast(e.Item.DataItem, dtoDisplayVersionItem)
                Dim cell As HtmlControls.HtmlTableCell = DirectCast(e.Item.FindControl("TDactions"), HtmlTableCell)
                cell.Visible = _ShowActions

                cell = DirectCast(e.Item.FindControl("TDsize"), HtmlTableCell)
                cell.Visible = dto.Type <> ItemType.Link
                If _ShowActions Then
                    Dim oButton As Button = DirectCast(e.Item.FindControl("BTNpromoteItemVersion"), Button)
                    oButton.Visible = dto.Permissions.SetActive
                    Resource.setButton(oButton, True)
                End If
                Dim oLabel As Label = DirectCast(e.Item.FindControl("LBautor"), Label)
                oLabel.Text = dto.Author

                oLabel = DirectCast(e.Item.FindControl("LBdate"), Label)
                oLabel.Text = GetDateTimeString(dto.CreatedOn, "//")

                oLabel = DirectCast(e.Item.FindControl("LBversionName"), Label)
                oLabel.Text = dto.DisplayName & " v." & dto.Number
                Dim oHyperLink As HyperLink = DirectCast(e.Item.FindControl("HYPversionItem"), HyperLink)
                oHyperLink.Text = dto.DisplayName & " v." & dto.Number
                Select Case dto.Type
                    Case ItemType.File, ItemType.Multimedia, ItemType.ScormPackage
                        If dto.Permissions.Play Then
                            oHyperLink.NavigateUrl = GetItemUrl(dto, ItemAction.play)
                            oHyperLink.Visible = True
                            oLabel.Visible = False

                            Select Case dto.DisplayMode
                                Case DisplayMode.inModal, DisplayMode.downloadOrPlayOrModal
                                    oHyperLink.CssClass = LTmodalCssClass.Text & " " & dto.Type.ToString.ToLower()
                                Case Else
                                    oHyperLink.Target = "_blank"
                            End Select

                        ElseIf dto.Permissions.Download Then
                            oHyperLink.NavigateUrl = GetItemUrl(dto, ItemAction.download)
                            oHyperLink.Visible = True
                            oLabel.Visible = False
                        Else
                            oLabel.Visible = False
                            oHyperLink.Visible = False
                        End If
                    Case Else
                        oLabel.Visible = False
                        oHyperLink.Visible = False
                End Select


                oLabel = DirectCast(e.Item.FindControl("LBfileSize"), Label)
                oLabel.Visible = (dto.Type <> ItemType.Folder AndAlso dto.Type <> ItemType.Link)
                oLabel.Text = dto.GetSize()
                Dim oHyperLinkPlay As HyperLink = DirectCast(e.Item.FindControl("HYPplayItem"), HyperLink)
                Dim oHyperLinkDownload As HyperLink = DirectCast(e.Item.FindControl("HYPdownloadItem"), HyperLink)
                Dim oLiteral As Literal = DirectCast(e.Item.FindControl("LTseparatorItem"), Literal)
                Resource.setHyperLink(oHyperLinkPlay, False, True)
                Resource.setHyperLink(oHyperLinkDownload, False, True)

                Select Case dto.Type
                    Case ItemType.Multimedia, ItemType.ScormPackage
                        'oHyperLinkPlay.Visible = dto.Permissions.Play
                        'oHyperLinkPlay.NavigateUrl = GetItemUrl(dto, ItemAction.play)

                        oHyperLinkDownload.NavigateUrl = GetItemUrl(dto, ItemAction.download)
                        oHyperLinkDownload.Visible = dto.Permissions.Download
                        'oLiteral.Visible = dto.Permissions.Download AndAlso dto.Permissions.Play
                    Case Else
                        oLiteral.Visible = False
                        oHyperLinkDownload.Visible = False
                        oHyperLinkDownload.Visible = False
                End Select


        End Select
    End Sub

#End Region

End Class