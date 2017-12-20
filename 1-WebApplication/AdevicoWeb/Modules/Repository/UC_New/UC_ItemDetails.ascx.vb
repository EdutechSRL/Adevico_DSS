Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.ActionDataContract

Public Class UC_ItemDetails
    Inherits FRbaseControl
    Implements IViewItemDetails

#Region "Context"
    Private _Presenter As ItemDetailsPresenter
    Public ReadOnly Property CurrentPresenter() As ItemDetailsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ItemDetailsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdRepositoryItem As Long Implements IViewItemDetails.IdRepositoryItem
        Get
            Return ViewStateOrDefault("IdRepositoryItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdRepositoryItem") = value
        End Set
    End Property
    Private Property RepositoryItemType As ItemType Implements IViewItemDetails.RepositoryItemType
        Get
            Return ViewStateOrDefault("RepositoryItemType", ItemType.File)
        End Get
        Set(value As ItemType)
            ViewState("RepositoryItemType") = value
        End Set
    End Property
    Private Function GetFolderTypeTranslation() As Dictionary(Of FolderType, String) Implements IViewItemDetails.GetFolderTypeTranslation
        Return (From e As FolderType In [Enum].GetValues(GetType(FolderType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.FolderType." & e.ToString))
    End Function
    Private Function GetTypesTranslations() As Dictionary(Of ItemType, String) Implements IViewItemDetails.GetTypesTranslations
        Return (From e As ItemType In [Enum].GetValues(GetType(ItemType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.ItemType." & e.ToString))
    End Function
    Private Function GetUnknownUserName() As String Implements IViewItemDetails.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Function GetRootFolderFullname() As String Implements IViewItemDetails.GetRootFolderFullname
        Return Resource.getValue("RootFolder")
    End Function
#End Region

#Region "Internal"
    Public Event DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
    Public Event UpdateContainerTags(tags As List(Of String))
    Public Event CheckIsValidOperation(ByRef isvalid As Boolean)
    Private Property EditMode As Boolean
        Get
            Return ViewStateOrDefault("EditMode", False)
        End Get
        Set(value As Boolean)
            ViewState("EditMode") = value
        End Set
    End Property
    Private _previewExtensions As String
    Private Property PreviewExtensions As String
        Get
            If String.IsNullOrWhiteSpace(_previewExtensions) Then
                _previewExtensions = ViewStateOrDefault("previewExtensions", "")
            End If
            Return _previewExtensions
        End Get
        Set(value As String)
            ViewState("previewExtensions") = value
            _previewExtensions = value
        End Set
    End Property
    Public ReadOnly Property AllowUpload As Boolean
        Get
            Return TRallowUpload.Visible AndAlso CBXallowUpload.Checked
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTinformationSection)
            .setLiteral(LTthumbnailSection)
            .setLiteral(LTdescriptionSection)
            .setLabel(LBitemTagsTitle)
            .setLiteral(LTitemName_t)
            .setLiteral(LTitemUrl_t)
            .setLiteral(LTitemPathTitle)
            .setLiteral(LTitemSizeTitle)
            .setLiteral(LTitemAuthorTitle)
            .setLiteral(LTitemCreatedOnTitle)
            .setLiteral(LTitemUpdatedOnTitle)
            .setLiteral(LTitemMyDownloadsTitle)
            .setLiteral(LTitemDownloadsTitle)
            .setLiteral(LTitemTypeTitle)
            .setLiteral(LTitemAllowUploadTitle)
            .setLiteral(LTitemVisibilityTitle)
            .setLiteral(LTitemStatusTitle)
            .setLiteral(LTitemUrlName_t)
            .setLabel(LBitemEditTagsTitle)
            .setLabel(LBemptyPreview)
            .setHyperLink(HYPplayItem, False, True)
            .setHyperLink(HYPdownloadItem, False, True)
            .setLiteral(LTdisplayModeTitle)
            .setCheckBox(CBXallowUpload)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(edit As Boolean, item As dtoDisplayRepositoryItem, thumbnailWidth As Integer, thumbnailHeight As Integer, pExtensions As String) Implements IViewItemDetails.InitializeControl
        InitializeRepositoryPath(item.Repository)
        IdRepositoryItem = item.Id
        RepositoryItemType = item.Type
        EditMode = edit
        previewExtensions = pExtensions
        UpdateDisplayInfo(edit, item)
    End Sub
    Private Sub SetItemNameInfo(editMode As Boolean, item As dtoDisplayRepositoryItem)
        TRurl.Visible = False
        TRurlName.Visible = False
        LTitemName.Visible = False
        LBitemExtension.Visible = False
        Select Case item.Type
            Case ItemType.Folder
                LTitemName.Text = item.Name
                TXBitemName.Text = item.Name
                LTitemName.Visible = Not editMode
                TXBitemName.Visible = editMode
                RFVitemName.Visible = editMode
            Case ItemType.Multimedia, ItemType.ScormPackage, ItemType.SharedDocument, ItemType.File
                LTitemName.Text = item.DisplayName
                TXBitemName.Text = item.Name
                LTitemName.Visible = Not editMode
                TXBitemName.Visible = editMode
                LBitemExtension.Visible = editMode
                LBitemExtension.Text = item.Extension
                RFVitemName.Visible = editMode
            Case ItemType.Link
                TRitemName.Visible = False
                TRurl.Visible = True
                TRurlName.Visible = True

                HYPurl.Visible = Not editMode
                TXBitemUrl.Visible = editMode
                RFVitemUrl.Visible = editMode
                LTitemUrlName.Visible = Not editMode
                TXBitemUrlName.Visible = editMode
                If editMode Then
                    TXBitemUrl.Text = item.Url
                    TXBitemUrlName.Text = item.Name
                Else
                    LTitemUrlName.Text = item.DisplayName
                    Dim destinationUri As Uri = Nothing
                    If String.IsNullOrWhiteSpace(item.Url) Then
                        HYPurl.Enabled = False
                        HYPurl.NavigateUrl = "#"
                    Else
                         Dim linkUrl As String = SanitizeLinkUrl(item.Url)
                        If String.IsNullOrWhiteSpace(linkUrl) Then
                            HYPurl.Enabled = False
                            HYPurl.NavigateUrl = "#"
                        Else
                            HYPurl.NavigateUrl = linkUrl
                            HYPurl.Enabled = True
                        End If
                    End If
                End If
        End Select
    End Sub
#Region "Messages"
    Private Sub DisplaySessionTimeout() Implements IViewItemDetails.DisplaySessionTimeout

    End Sub
    Private Sub DisplayUserMessage(messageType As UserMessageType) Implements IViewItemDetails.DisplayUserMessage
        RaiseEvent DisplayMessage(Resource.getValue("UserMessageType." & messageType.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub



    Private Sub DisplayDuplicateName(name As String, type As ItemType, folderName As String) Implements IViewItemDetails.DisplayDuplicateName
        Dim message As String = Resource.getValue("IViewItemDetails.NameExist.ItemType." & type.ToString)
        message = Replace(message, "#name#", name)
        message = Replace(message, "#foldername#", folderName)
        RaiseEvent DisplayMessage(message, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayDuplicateUrl(url As String, folderName As String) Implements IViewItemDetails.DisplayDuplicateUrl
        Dim message As String = Resource.getValue("IViewItemDetails.UrlExist")
        message = Replace(message, "#url#", url)
        message = Replace(message, "#foldername#", folderName)
        RaiseEvent DisplayMessage(message, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnavailableItem(action As ItemAction) Implements IViewItemDetails.DisplayUnavailableItem
        RaiseEvent DisplayMessage(Resource.getValue("IViewRepository.DisplayUnavailableItem.ItemAction." & action.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnknownItem(action As ItemAction) Implements IViewItemDetails.DisplayUnknownItem
        RaiseEvent DisplayMessage(Resource.getValue("IViewItemDetails.DisplayUnknownItem.ItemAction." & action.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUpdateMessage(action As ItemAction, executed As Boolean, name As String, extension As String, type As ItemType) Implements IViewItemDetails.DisplayUpdateMessage
        Dim key As String = "IViewRepository.DisplayMessage.Executed." & executed.ToString() & ".ItemAction." & action.ToString
        Dim message As String = String.Format(Resource.getValue(key), GetFilenameRender(name, extension, type))
        RaiseEvent DisplayMessage(message, IIf(executed, lm.Comol.Core.DomainModel.Helpers.MessageType.success, lm.Comol.Core.DomainModel.Helpers.MessageType.error))
    End Sub
    Private Sub DisplayUpdateMessage(status As ItemSaving) Implements IViewItemDetails.DisplayUpdateMessage
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case status
            Case ItemSaving.Saved
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case ItemSaving.NameAndUrlDuplicate, ItemSaving.NameDuplicate, ItemSaving.UrlDuplicate
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End Select
        RaiseEvent DisplayMessage(Resource.getValue("IViewItemDetails.ItemSaving." & status.ToString), mType)
    End Sub
#End Region
    Private Sub UpdateDownloads(downloads As Long, myDownloads As Long) Implements IViewItemDetails.UpdateDownloads
        Select Case myDownloads
            Case 0, 1
                LTitemMyDownloadsValue.Text = Resource.getValue("downloadinfo." & myDownloads)
            Case Else
                LTitemMyDownloadsValue.Text = String.Format(Resource.getValue("downloadinfo.n"), myDownloads)
        End Select
        Select Case downloads
            Case 0, 1
                LTitemDownloadsValue.Text = Resource.getValue("downloadinfo." & downloads)
            Case Else
                LTitemDownloadsValue.Text = String.Format(Resource.getValue("downloadinfo.n"), downloads)
        End Select
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRepository.ActionType, idItem As Long, objType As ModuleRepository.ObjectType) Implements IViewItemDetails.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objType, idItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub UpdateDefaultTags(tags As List(Of String)) Implements IViewItemDetails.UpdateDefaultTags
        RaiseEvent UpdateContainerTags(tags)
    End Sub
    Private Sub UpdateDisplayInfo(item As dtoDisplayRepositoryItem) Implements IViewItemDetails.UpdateDisplayInfo
        UpdateDisplayInfo(EditMode, item)
    End Sub
    Private Function CleanBackUrl(backUrl As String) As String
        backUrl = Replace(backUrl, PageUtility.ApplicationUrlBase(True), "")
        backUrl = Replace(backUrl, PageUtility.ApplicationUrlBase(False), "")
        Return backUrl
    End Function
    Private Function SanitizeFolderUrl(folderUrl As String) As String
        If Not folderUrl.StartsWith(PageUtility.ApplicationUrlBase(True)) AndAlso Not folderUrl.StartsWith(PageUtility.ApplicationUrlBase(False)) Then
            folderUrl = PageUtility.ApplicationUrlBase & folderUrl
        End If
        Return folderUrl
    End Function
    Private Sub NotifyVisibilityChanged(idModule As Integer, idFolder As Long, folderName As String, folderUrl As String, item As liteRepositoryItem) Implements IViewItemDetails.NotifyVisibilityChanged
        Dim oSender As New RepositoryCommunityNewsUtility(idModule, PageUtility)
        Dim backUrl As String = CleanBackUrl(folderUrl)
        folderUrl = SanitizeFolderUrl(folderUrl)
        oSender.ItemEditVisibility(item.IsVisible, item.Repository.IdCommunity, item.Id, item.IdVersion, item.Type, item.DisplayName, folderName, GetItemDownloadOrPlayUrl(item, False), folderUrl)

    End Sub
#End Region

#Region "Internal"
    Private Sub UpdateDisplayInfo(editMode As Boolean, item As dtoDisplayRepositoryItem)
        SetItemNameInfo(editMode, item)
        SetPreviewInfo(editMode, item)
        SetBaseInfo(editMode, item)
        SetDisplayMode(editMode, item)
        SetItemStatus(item)
        SetTags(editMode, item)
    End Sub
    Private Sub SetPreviewInfo(editMode As Boolean, item As dtoDisplayRepositoryItem)
        If Not String.IsNullOrEmpty(item.Thumbnail) Then
            DVpreview.Visible = True
            DVmain.Attributes("class") = Replace(DVmain.Attributes("class"), "grid_12", "grid_9")
            LBemptyPreview.Visible = False
            HYPthumbnail.Visible = True
            Dim fExtension As String = item.Extension
            If Not String.IsNullOrWhiteSpace(fExtension) AndAlso fExtension.StartsWith(".") AndAlso fExtension.Length > 1 Then
                fExtension = fExtension.Substring(1)
            End If
            If item.Permissions.Download AndAlso (PreviewExtensions.Contains(item.Extension) OrElse PreviewExtensions.Contains(fExtension)) Then
                HYPthumbnail.NavigateUrl = GetItemUrl(item, ItemAction.download, item.Repository.Type, item.Repository.IdCommunity, OrderBy.name, True)
            Else
                HYPthumbnail.NavigateUrl = GetFinalThumbnailVirtualPath() & item.Thumbnail
            End If
            HYPthumbnail.Text = Replace("<img src=""#url#"" />", "#url#", GetFinalThumbnailVirtualPath() & item.Thumbnail)
        ElseIf item.AutoThumbnail Then
            DVpreview.Visible = True
            DVmain.Attributes("class") = Replace(DVmain.Attributes("class"), "grid_12", "grid_9")
            LBemptyPreview.Visible = True
            HYPthumbnail.Visible = False
        Else
            DVpreview.Visible = False
            DVmain.Attributes("class") = Replace(DVmain.Attributes("class"), "grid_9", "grid_12")
        End If
    End Sub

    Private Sub SetBaseInfo(editMode As Boolean, item As dtoDisplayRepositoryItem)
        DVdescription.Visible = Not editMode
        TXAdescription.Visible = editMode
        TXAdescription.Value = item.Description
        LTdescription.Text = item.Description
        DVdescriptionSection.Visible = editMode OrElse Not String.IsNullOrWhiteSpace(item.Description)
        TRitemSize.Visible = (item.Type <> ItemType.Folder AndAlso item.Type <> ItemType.Link)
        LTitemSizeValue.Text = item.GetSize()


        If (item.Type = ItemType.SharedDocument OrElse (item.HasVersions AndAlso item.ModifiedOn.HasValue)) Then
            LTitemAuthorValue.Text = item.ModifiedBy
        Else
            LTitemAuthorValue.Text = item.OwnerName
        End If
        TRpath.Visible = True
        LTitemPath.Text = item.Path

        If item.FolderType = FolderType.standard OrElse item.IsFile Then
            LTitemCreatedOnValue.Text = GetDateTimeString(item.CreatedOn, "")

            If (item.Type = ItemType.SharedDocument OrElse (item.HasVersions AndAlso item.ModifiedOn.HasValue)) Then
                LTitemUpdatedOnValue.Text = GetDateTimeString(item.ModifiedOn, "")
                TRitemUpdatedOn.Visible = True
            Else
                TRitemUpdatedOn.Visible = False
            End If
            If item.IsFile Then
                TRmyDownloads.Visible = True
                Select Case item.MyDownloads
                    Case 0, 1
                        LTitemMyDownloadsValue.Text = Resource.getValue("downloadinfo." & item.MyDownloads)
                    Case Else
                        LTitemMyDownloadsValue.Text = String.Format(Resource.getValue("downloadinfo.n"), item.MyDownloads)
                End Select

                TRdownloads.Visible = item.Permissions.ViewOtherStatistics
                Select Case item.Downloaded
                    Case 0, 1
                        LTitemDownloadsValue.Text = Resource.getValue("downloadinfo." & item.Downloaded)
                    Case Else
                        LTitemDownloadsValue.Text = String.Format(Resource.getValue("downloadinfo.n"), item.Downloaded)
                End Select
            Else
                TRmyDownloads.Visible = False
                TRdownloads.Visible = False
            End If
        Else
            TRitemCreatedOn.Visible = False
        End If
        LTitemTypeValue.Text = Resource.getValue("Info.ItemType." & item.Type.ToString())

        TRallowUpload.Visible = (item.FolderType = FolderType.standard)
        If item.FolderType = FolderType.standard Then
            LTitemAllowUploadValue.Text = Resource.getValue("Folder.AllowUpload." & item.AllowUpload.ToString)
            CBXallowUpload.Checked = item.AllowUpload
            CBXallowUpload.Visible = editMode
            LTitemAllowUploadValue.Visible = Not editMode
        End If

        If editMode Then
            TRvisibility.Visible = True
            CBXvisible.Visible = editMode
            CBXvisible.Checked = Not item.IsVisible
            LTitemVisibilityValue.Text = Resource.getValue("Item.Visibility.False")
        ElseIf Not item.IsVisible AndAlso (item.IsFile OrElse item.FolderType = FolderType.standard) Then
            TRvisibility.Visible = True
            CBXvisible.Visible = False
            LTitemVisibilityValue.Text = Resource.getValue("Item.Visibility.False")
        Else
            TRvisibility.Visible = False
        End If
    End Sub
    Private Sub SetTags(editMode As Boolean, item As dtoDisplayRepositoryItem)
        DVeditTags.Visible = editMode
        If item.TagsList.Any() Then
            TXBtags.Text = Join(item.TagsList.ToArray, ",")
        End If

        If item.TagsList.Any() OrElse item.Type = ItemType.Multimedia OrElse item.Type = ItemType.ScormPackage OrElse item.Type = ItemType.SharedDocument Then
            DVtag.Visible = True
            Select Case item.Type
                Case ItemType.Multimedia, ItemType.ScormPackage, ItemType.SharedDocument, ItemType.VideoStreaming
                    LBtagItemType.Visible = True
                Case Else
                    LBtagItemType.Visible = False
            End Select
            LBtagItemType.Text = Resource.getValue("LBtagItemType.ItemType." & item.Type.ToString())
            If item.TagsList.Any() Then
                RPTtags.DataSource = item.TagsList
                RPTtags.DataBind()
                RPTtags.Visible = True
            Else
                RPTtags.Visible = False
            End If
        Else
            DVtag.Visible = False
        End If
    End Sub
    Private Sub SetItemStatus(item As dtoDisplayRepositoryItem)
        TRstatus.Visible = (item.Type = ItemType.Multimedia OrElse item.Type = ItemType.ScormPackage)
        HYPdownloadItem.Visible = False
        HYPplayItem.Visible = False
        Select Case item.Availability
            Case ItemAvailability.available
                TRstatus.Visible = False
                SetItemCommands(item)
            Case ItemAvailability.notavailable
                TRstatus.Visible = True
                LTitemStatusValue.Text = Resource.getValue("Info.ItemAvailability." & item.Availability.ToString())
            Case Else
                TRstatus.Visible = True
                LTitemStatusValue.Text = Resource.getValue("Info.ItemAvailability." & item.Availability.ToString())
                SetItemCommands(item)
        End Select
    End Sub
    Private Sub SetItemCommands(item As dtoDisplayRepositoryItem)
        Select Case item.Type
            Case ItemType.File, ItemType.Multimedia, ItemType.ScormPackage
                HYPplayItem.Visible = item.Permissions.Play
                HYPplayItem.NavigateUrl = GetItemUrl(item, ItemAction.play, item.Repository.Type, item.Repository.IdCommunity, OrderBy.name, True)

                If item.Permissions.Play Then
                    Select Case item.DisplayMode
                        Case DisplayMode.inModal, DisplayMode.downloadOrPlayOrModal
                            HYPplayItem.CssClass = LTmodalCssClass.Text & " " & item.Type.ToString.ToLower()
                        Case Else
                            HYPplayItem.Target = "_blank"
                    End Select
                End If
                HYPdownloadItem.NavigateUrl = GetItemUrl(item, ItemAction.download, item.Repository.Type, item.Repository.IdCommunity, OrderBy.name, True)
                HYPdownloadItem.Visible = item.Permissions.Download
            Case Else
                HYPdownloadItem.Visible = False
                HYPplayItem.Visible = False
        End Select
    End Sub
    Private Sub SetDisplayMode(editMode As Boolean, item As dtoDisplayRepositoryItem)
        Dim items As New List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer))
        Select Case item.Type
            Case ItemType.None, ItemType.RootFolder, ItemType.Folder
                TRdisplayMode.Visible = False
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.none), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.auto.ToString)})
                Exit Sub
            Case ItemType.Link
                TRdisplayMode.Visible = False
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.downloadOrPlay), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.auto.ToString)})
                Exit Sub
            Case ItemType.File, ItemType.SharedDocument
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.none), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.auto.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.downloadOrPlay), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.onlydownload.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.downloadOrPlayOrModal), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.downloadandpreview.ToString)})
            Case ItemType.Multimedia, ItemType.ScormPackage
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.none), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.auto.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.downloadOrPlay), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.onlyotherwindow.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.downloadOrPlayOrModal), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.otherwindowormodal.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.inModal), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.onlymodal.ToString)})
            Case ItemType.VideoStreaming
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.none), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.auto.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.downloadOrPlay), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.onlyotherwindow.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.downloadOrPlayOrModal), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.otherwindowormodal.ToString)})
                items.Add(New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(DisplayMode.inModal), .Translation = Resource.getValue("Select.DisplayModeTranslation." & DisplayModeTranslation.onlymodal.ToString)})
        End Select


        RBLdisplayMode.DataSource = items.OrderBy(Function(i) i.Translation).ToList()
        RBLdisplayMode.DataTextField = "Translation"
        RBLdisplayMode.DataValueField = "Id"
        RBLdisplayMode.DataBind()
        If items.Any(Function(i) i.Id = CInt(item.DisplayMode)) Then
            RBLdisplayMode.SelectedValue = CInt(item.DisplayMode)
        Else
            RBLdisplayMode.SelectedIndex = 0
        End If
        RBLdisplayMode.Enabled = editMode
        SetRadioButtonListItemsCssClass(RBLdisplayMode)
    End Sub
    Private Enum DisplayModeTranslation As Integer
        auto = 0
        onlydownload = 1
        onlyotherwindow = 2
        onlymodal = 3
        downloadandpreview = 4
        otherwindowormodal = 5
    End Enum

    Private Function GetFilenameRender(fullname As String, fileExtension As String, type As ItemType) As String
        Dim template As String = LTtemplateFile.Text
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                template = Replace(template, "#ico#", LTitemFolderCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                template = Replace(template, "#ico#", LTitemUrlCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                template = Replace(template, "#ico#", LTitemMultimediaCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                template = Replace(template, "#ico#", LTitemScormPackageCssClass.Text)
            Case Else
                If Not String.IsNullOrWhiteSpace(fileExtension) Then
                    fileExtension = fileExtension.ToLower
                End If
                If fileExtension.StartsWith(".") Then
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text & Replace(fileExtension, ".", ""))
                Else
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text)
                End If
        End Select
        template = Replace(template, "#name#", fullname)
        Return template
    End Function
    Public Function ChangeVisibility(action As ItemAction) As Boolean
        Return CurrentPresenter.ExecuteAction(IdRepositoryItem, action)
    End Function
    Public Function SaveDetails() As Boolean
        Dim tags As New List(Of String)
        If Not String.IsNullOrWhiteSpace(TXBtags.Text) Then
            tags = TXBtags.Text.Split(",").ToList()
        End If
        Dim mode As DisplayMode?
        If RBLdisplayMode.SelectedIndex >= 0 Then
            mode = DirectCast(CInt(RBLdisplayMode.SelectedValue), DisplayMode)
        End If
        If RepositoryItemType = ItemType.Link Then
            Return CurrentPresenter.SaveItem(IdRepositoryItem, TXAdescription.Value, TXBitemUrlName.Text, TXBitemUrl.Text, mode, Not CBXvisible.Checked, CBXallowUpload.Checked, tags)
        Else
            Return CurrentPresenter.SaveItem(IdRepositoryItem, TXAdescription.Value, TXBitemName.Text, "", mode, Not CBXvisible.Checked, CBXallowUpload.Checked, tags)
        End If
    End Function
#End Region

End Class