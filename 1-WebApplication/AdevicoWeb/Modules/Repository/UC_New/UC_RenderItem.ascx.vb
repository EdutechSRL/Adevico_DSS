Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_RenderItem
    Inherits FRbaseControl

#Region "Internal"
    Private _AutoPostBack As Boolean?
    Public Property AutoPostBack As Boolean
        Get
            If Not _AutoPostBack.HasValue Then
                _AutoPostBack = ViewStateOrDefault("AutoPostBack", False)
            End If
            Return _AutoPostBack.Value
        End Get
        Set(value As Boolean)
            _AutoPostBack = value
            ViewState("AutoPostBack") = value
        End Set
    End Property

    Public Event MenuItemCommand(ByVal idItem As Long, ByVal action As lm.Comol.Core.FileRepository.Domain.ItemAction)
    Public Event SelectedFolder(idFolder As Long, path As String, type As FolderType)
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            SetInternazionalizzazione()
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBitemTagsTitle)
            .setLiteral(LTitemPathTitle)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As dtoDisplayRepositoryItem, ByVal displayPath As Boolean, ByVal type As lm.Comol.Core.FileRepository.Domain.RepositoryType, ByVal idCommunity As Integer, order As OrderBy, ByVal ascending As Boolean)
        Dim actionsUrl As New Dictionary(Of lm.Comol.Core.FileRepository.Domain.ItemAction, String)
        LBfileSize.Visible = (item.Type <> ItemType.Folder AndAlso item.Type <> ItemType.Link)
        LBfileSize.Text = item.GetSize()
        If Not String.IsNullOrWhiteSpace(item.Description) Then
            DVdescription.Visible = True
            LTdescription.Text = item.Description
        Else
            DVdescription.Visible = False
        End If

        If (item.Type = ItemType.SharedDocument OrElse (item.HasVersions AndAlso item.ModifiedOn.HasValue)) Then
            LBauthor.Text = item.ModifiedBy
        Else
            LBauthor.Text = item.OwnerName
        End If
        DVpath.Visible = displayPath
        LTitemPath.Text = item.Path

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
        Select Case item.Type
            Case ItemType.Folder
                LNBitemUrl.CommandName = item.Id
                LNBitemUrl.CommandArgument = item.IdentifierPath & "|" & item.FolderType.ToString
                If Not AutoPostBack Then
                    HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemFolderCssClass.Text)
                    Select Case item.FolderType
                        Case FolderType.standard
                            HYPitemUrl.NavigateUrl = BaseUrl & RootObject.RepositoryItems(type, idCommunity, item.Id, item.Id, item.FolderType, order, ascending)
                        Case Else
                            HYPitemUrl.NavigateUrl = BaseUrl & RootObject.FolderUrlTemplate(item.Id, item.FolderType, item.IdentifierPath, type, idCommunity)
                            HYPitemUrl.NavigateUrl = Replace(Replace(HYPitemUrl.NavigateUrl, "#OrderBy#", order.ToString), "#Boolean#", ascending.ToString().ToLower)
                    End Select
                Else
                    LNBitemUrl.Text = Replace(LNBitemUrl.Text, "#ico#", LTitemFolderCssClass.Text)
                    LNBitemUrl.Text = Replace(LNBitemUrl.Text, "#name#", item.DisplayName)
                End If
            Case ItemType.Link
                Dim destinationUri As Uri = Nothing
                HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemUrlCssClass.Text)

                If String.IsNullOrWhiteSpace(item.Url) Then
                    HYPitemUrl.Enabled = False
                    HYPitemUrl.NavigateUrl = "#"
                Else
                    HYPitemUrl.Target = "_blank"
                    Dim linkUrl As String = SanitizeLinkUrl(item.Url)
                    If String.IsNullOrWhiteSpace(linkUrl) Then
                        HYPitemUrl.Enabled = False
                        HYPitemUrl.NavigateUrl = "#"
                    Else
                        HYPitemUrl.NavigateUrl = linkUrl
                        HYPitemUrl.Enabled = True
                    End If
                End If

            Case ItemType.Multimedia
                HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemMultimediaCssClass.Text)
                Select Case item.DisplayMode
                    Case DisplayMode.inModal, DisplayMode.downloadOrPlayOrModal
                        HYPitemUrl.CssClass = LTmodalCssClass.Text & " " & item.Type.ToString.ToLower()
                    Case Else
                        HYPitemUrl.Target = "_blank"
                End Select

                HYPitemUrl.NavigateUrl = GetItemUrl(item, ItemAction.play, type, idCommunity, order, ascending)
            Case ItemType.ScormPackage
                HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemScormPackageCssClass.Text)
                Select Case item.DisplayMode
                    Case DisplayMode.inModal, DisplayMode.downloadOrPlayOrModal
                        HYPitemUrl.CssClass = LTmodalCssClass.Text & " " & item.Type.ToString.ToLower()
                    Case Else
                        HYPitemUrl.Target = "_blank"
                End Select
                HYPitemUrl.NavigateUrl = GetItemUrl(item, ItemAction.play, type, idCommunity, order, ascending)
            Case ItemType.SharedDocument
            Case ItemType.File
                HYPitemUrl.NavigateUrl = GetItemUrl(item, ItemAction.download, type, idCommunity, order, ascending)

                If item.Extension.StartsWith(".") Then
                    HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemExtensionCssClass.Text & Replace(item.Extension, ".", ""))
                Else
                    HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemExtensionCssClass.Text)
                End If
                actionsUrl(ItemAction.download) = HYPitemUrl.NavigateUrl
                Select Case item.DisplayMode
                    Case DisplayMode.downloadOrPlayOrModal, DisplayMode.inModal
                        If item.AutoThumbnail Then
                            For Each i As String In LTpreviewImages.Text.Split("|")
                                HYPitemUrl.Attributes.Add(i.Split(":")(0), i.Split(":")(1))
                            Next
                        End If
                End Select
        End Select
        HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#name#", item.DisplayName)
        If item.Availability = ItemAvailability.available Then
            LNBitemUrl.Visible = AutoPostBack AndAlso item.Type = ItemType.Folder
            HYPitemUrl.Visible = Not (AutoPostBack AndAlso item.Type = ItemType.Folder)
            LTfileName.Visible = False
        Else
            HYPitemUrl.Visible = False
            LNBitemUrl.Visible = False
            LTfileName.Visible = True
            LTfileName.Text = HYPitemUrl.Text
        End If
        LTanchor.Text = String.Format(LTanchor.Text, item.Id)
        CTRLmenu.Visible = True
        CTRLmenu.InitializeControl(item, actionsUrl, type, idCommunity, order, ascending)

    End Sub
    Private Sub CTRLmenu_ItemCommand(idItem As Long, action As ItemAction) Handles CTRLmenu.ItemCommand
        RaiseEvent MenuItemCommand(idItem, action)
    End Sub
    Private Sub LNBitemUrl_Click(sender As Object, e As EventArgs) Handles LNBitemUrl.Click
        Dim oLinkButton As LinkButton = DirectCast(sender, LinkButton)
        Dim idFolder As Long = 0
        Long.TryParse(oLinkButton.CommandName, idFolder)
        Dim items As List(Of String) = oLinkButton.CommandArgument.Split("|").ToList()
        RaiseEvent SelectedFolder(idFolder, items.FirstOrDefault(), lm.Comol.Core.DomainModel.Helpers.EnumParser(Of FolderType).GetByString(items.Last(), FolderType.standard))
    End Sub
#End Region

End Class