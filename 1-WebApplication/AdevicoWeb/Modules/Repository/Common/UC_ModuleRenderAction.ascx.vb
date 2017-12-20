Imports lm.Comol.Core.ModuleLinks
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public Class UC_RepositoryRenderAction
    Inherits FRbaseControl
    Implements IViewRepositoryRenderAction


#Region "Context"
    Private _Presenter As RepositoryRenderActionPresenter
    Private ReadOnly Property CurrentPresenter() As RepositoryRenderActionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RepositoryRenderActionPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Base"
    Public Property CssClass As String Implements IViewModuleRenderAction.CssClass
        Get
            Dim css As String = ViewStateOrDefault("CssClass", "")
            If Not String.IsNullOrWhiteSpace(css) Then
                Return " " & css
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            ViewState("CssClass") = value
        End Set
    End Property
    Public Property InsideOtherModule As Boolean Implements IViewModuleRenderAction.InsideOtherModule
        Get
            Return ViewStateOrDefault("InsideOtherModule", True)
        End Get
        Set(value As Boolean)
            ViewState("InsideOtherModule") = value
        End Set
    End Property
    Private Property SourceModuleCode As String Implements IViewModuleRenderAction.SourceModuleCode
        Get
            Return ViewStateOrDefault("SourceModuleCode", "")
        End Get
        Set(value As String)
            ViewState("SourceModuleCode") = value
        End Set
    End Property
    Private Property SourceIdModule As Integer Implements IViewModuleRenderAction.SourceIdModule
        Get
            Return ViewStateOrDefault("SourceIdModule", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("SourceIdModule") = value
        End Set
    End Property
    Private Property Display As DisplayActionMode Implements IViewModuleRenderAction.Display
        Get
            Return ViewStateOrDefault("Display", DisplayActionMode.defaultAction)
        End Get
        Set(value As DisplayActionMode)
            ViewState("Display") = value
            If value = DisplayActionMode.none Then
                LTfileName.Text = " "
                LTfileName.Visible = True
                RPTactions.Visible = False
                CTRLmenu.Visible = False
                HYPitemUrl.Visible = False
                LBfileSize.Visible = False
                DVextraInfo.Visible = False
            Else
                RPTactions.Visible = ((value And DisplayActionMode.actions) > 0)
                LTfileName.Visible = ((value And DisplayActionMode.text) > 0)
                CTRLmenu.Visible = ((value And DisplayActionMode.menuActions) > 0)
                DVextraInfo.Visible = ((value And DisplayActionMode.extraInfo) > 0)
                If (value And DisplayActionMode.text) > 0 OrElse (value And DisplayActionMode.textDefault) > 0 Then
                    HYPitemUrl.Visible = False
                ElseIf (value And DisplayActionMode.defaultAction) > 0 OrElse ((value And DisplayActionMode.adminMode) > 0) Then
                    HYPitemUrl.Visible = True
                    LTfileName.Visible = False
                End If
            End If
        End Set
    End Property
    Public Property EnableAnchor As Boolean Implements IViewModuleRenderAction.EnableAnchor
        Get
            Return ViewStateOrDefault("EnableAnchor", True)
        End Get
        Set(value As Boolean)
            ViewState("EnableAnchor") = value
        End Set
    End Property
    Public Property ShortDescription As Boolean Implements IViewModuleRenderAction.ShortDescription
        Get
            Return ViewStateOrDefault("ShortDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("ShortDescription") = value
        End Set
    End Property
    Private Property ForIdUser As Integer Implements IViewModuleRenderAction.ForIdUser
        Get
            Return ViewStateOrDefault("ForUserId", PageUtility.CurrentContext.UserContext.CurrentUserID)
        End Get
        Set(value As Integer)
            ViewState("ForIdUser") = value
        End Set
    End Property
#End Region
    'Public Property IsReadyToPlay As Boolean Implements IViewRepositoryRenderAction.IsReadyToPlay
    '    Get
    '        Return ViewStateOrDefault("IsReadyToPlay", True)
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("IsReadyToPlay") = value
    '    End Set
    'End Property
    Private Property ItemIdentifier As String Implements IViewRepositoryRenderAction.ItemIdentifier
        Get
            Return ViewStateOrDefault("ItemIdentifier", "")
        End Get
        Set(value As String)
            ViewState("ItemIdentifier") = value
            If String.IsNullOrEmpty(value) Then
                LTanchor.Visible = False
            ElseIf LTanchor.Text.Contains("{0}") Then
                LTanchor.Text = String.Format(LTanchor.Text, value)
                LTanchor.Visible = EnableAnchor
            Else
                LTanchor.Visible = EnableAnchor
            End If
        End Set
    End Property
    Private ReadOnly Property DestinationUrl As String Implements IViewRepositoryRenderAction.DestinationUrl
        Get
            Dim url As String = Request.Url.LocalPath
            If LocalBaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, LocalBaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query & IIf(EnableAnchor, "#" & ItemIdentifier, ""))
            Return url
        End Get
    End Property
    Private ReadOnly Property PreLoadedContentView As lm.Comol.Core.DomainModel.ContentView Implements IViewRepositoryRenderAction.PreLoadedContentView
        Get
            Return PageUtility.PreLoadedContentView
        End Get
    End Property
    Public Property CssAvailability As String Implements IViewRepositoryRenderAction.CssAvailability
        Get
            If Availability <> lm.Comol.Core.FileRepository.Domain.ItemAvailability.available Then
                Return " " & Availability.ToString.ToLower.ToString()
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            ViewState("CssClass") = value
        End Set
    End Property
    Private Property IdRequiredAction As Integer Implements IViewRepositoryRenderAction.IdRequiredAction
        Get
            Return ViewStateOrDefault("IdRequiredAction", 0)
        End Get
        Set(value As Integer)
            ViewState("IdRequiredAction") = value
        End Set
    End Property
    Public Property Availability As lm.Comol.Core.FileRepository.Domain.ItemAvailability Implements IViewRepositoryRenderAction.Availability
        Get
            Return ViewStateOrDefault("Availability", lm.Comol.Core.FileRepository.Domain.ItemAvailability.notavailable)
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.ItemAvailability)
            ViewState("Availability") = value
        End Set
    End Property
    Public Property ItemType As lm.Comol.Core.FileRepository.Domain.ItemType Implements IViewRepositoryRenderAction.ItemType
        Get
            Return ViewStateOrDefault("ItemType", lm.Comol.Core.FileRepository.Domain.ItemType.File)
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.ItemType)
            ViewState("ItemType") = value
        End Set
    End Property
    Public Property DisplayExtraInfo As Boolean Implements IViewRepositoryRenderAction.DisplayExtraInfo
        Get
            Return ViewStateOrDefault("DisplayExtraInfo", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayExtraInfo") = value
            DVextraInfo.Visible = value
        End Set
    End Property
    Public Property DisplayLinkedBy As Boolean Implements IViewRepositoryRenderAction.DisplayLinkedBy
        Get
            Return ViewStateOrDefault("DisplayLinkedBy", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayLinkedBy") = value
            DVlinkedBy.Visible = value
        End Set
    End Property
    Public Property DisplayUploader As Boolean Implements IViewRepositoryRenderAction.DisplayUploader
        Get
            Return ViewStateOrDefault("DisplayUploader", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayUploader") = value
            DVauthor.Visible = value
        End Set
    End Property
    Public Property DisplayTags As Boolean Implements IViewRepositoryRenderAction.DisplayTags
        Get
            Return ViewStateOrDefault("DisplayTags", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayTags") = value
            DVtag.Visible = value
        End Set
    End Property
    Public Property ExtraInfoDescription As String Implements IViewRepositoryRenderAction.ExtraInfoDescription
        Get
            Return ViewStateOrDefault("ExtraInfoDescription", "")
        End Get
        Set(value As String)
            ViewState("ExtraInfoDescription") = value
        End Set
    End Property
    Public Property OverrideItemDisplayMode As lm.Comol.Core.FileRepository.Domain.DisplayMode Implements IViewRepositoryRenderAction.OverrideItemDisplayMode
        Get
            Return ViewStateOrDefault("OverrideItemDisplayMode", lm.Comol.Core.FileRepository.Domain.DisplayMode.none)
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.DisplayMode)
            ViewState("OverrideItemDisplayMode") = value
        End Set
    End Property

#End Region

#Region "Internal"
    Private _LocalBaseUrl As String
    Private ReadOnly Property LocalBaseUrl As String
        Get
            If String.IsNullOrWhiteSpace(_LocalBaseUrl) Then
                _LocalBaseUrl = PageUtility.BaseUrl
            End If
            Return _LocalBaseUrl
        End Get
    End Property
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
    Private _DisplaySize As Boolean?
    Public Property DisplaySize As Boolean
        Get
            If Not _DisplaySize.HasValue Then
                _DisplaySize = ViewStateOrDefault("DisplaySize", False)
            End If
            Return _DisplaySize.Value
        End Get
        Set(value As Boolean)
            _DisplaySize = value
            ViewState("DisplaySize") = value
        End Set
    End Property

    Public Event MenuItemCommand(ByVal idItem As Long, ByVal idVersion As Long, ByVal action As lm.Comol.Core.FileRepository.Domain.ItemAction)
    'Public Event SelectedFolder(idFolder As Long, path As String, type As FolderType)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Private Sub InternalInitialize(idUser As Integer, Optional _cssClass As String = "")
        If idUser > 0 Then
            ForIdUser = idUser
        End If
        If Not String.IsNullOrWhiteSpace(_cssClass) Then
            CssClass = _cssClass
        End If
    End Sub
    Private Sub LNBitemUrl_Click(sender As Object, e As EventArgs) Handles LNBitemUrl.Click
        Dim oLinkButton As LinkButton = DirectCast(sender, LinkButton)
        Dim idFolder As Long = 0
        Long.TryParse(oLinkButton.CommandName, idFolder)
        Dim items As List(Of String) = oLinkButton.CommandArgument.Split("|").ToList()
        'RaiseEvent SelectedFolder(idFolder, items.FirstOrDefault(), lm.Comol.Core.DomainModel.Helpers.EnumParser(Of FolderType).GetByString(items.Last(), FolderType.standard))
    End Sub


    'Private Sub SetCssClass(hyp As HyperLink, type As ItemType, displayMode As DisplayActionMode, ByVal openLinkCssClass As String)
    '    Dim cssClass As String = ""
    '    If Not String.IsNullOrWhiteSpace(openLinkCssClass) AndAlso type = ItemType.Multimedia Then
    '        openLinkCssClass = Replace(openLinkCssClass, "scorm", "multimedia")
    '    End If

    '    If (InsideOtherModule AndAlso displayMode <> DisplayActionMode.adminMode) Then
    '        cssClass = "fileRepositoryCookieNoBlockUI"
    '        If onModalPage AndAlso Not String.IsNullOrWhiteSpace(openLinkCssClass) Then
    '            cssClass = openLinkCssClass
    '        End If
    '    Else
    '        cssClass = openLinkCssClass
    '    End If
    '    hyp.CssClass = cssClass
    'End Sub
#End Region

#Region "Implements"

#Region "Initializers"
    Public Sub InitializeControl(idUser As Integer, initializer As dtoObjectRenderInitializer, actionType As lm.Comol.Core.DomainModel.StandardActionType, action As DisplayActionMode, Optional _cssClass As String = "") Implements IViewModuleRenderAction.InitializeControl
        InternalInitialize(idUser, _cssClass)
        CurrentPresenter.InitView(idUser, initializer, actionType, action)
    End Sub
    Public Sub InitializeControl(idUser As Integer, initializer As dtoObjectRenderInitializer, action As DisplayActionMode, Optional _cssClass As String = "") Implements IViewModuleRenderAction.InitializeControl
        InternalInitialize(idUser, _cssClass)
        CurrentPresenter.InitView(idUser, initializer, action)
    End Sub

    Public Sub InitializeControl(initializer As dtoObjectRenderInitializer, actionType As lm.Comol.Core.DomainModel.StandardActionType, action As DisplayActionMode, Optional _cssClass As String = "") Implements IViewModuleRenderAction.InitializeControl
        InitializeControl(PageUtility.CurrentContext.UserContext.CurrentUserID, initializer, actionType, action, _cssClass)
    End Sub
    Public Sub InitializeControl(initializer As dtoObjectRenderInitializer, action As DisplayActionMode, Optional _cssClass As String = "") Implements IViewModuleRenderAction.InitializeControl
        InitializeControl(PageUtility.CurrentContext.UserContext.CurrentUserID, initializer, action, _cssClass)
    End Sub
    Public Function InitializeRemoteControl(idUser As Integer, initializer As dtoObjectRenderInitializer, actionsToDisplay As lm.Comol.Core.DomainModel.StandardActionType, action As DisplayActionMode) As List(Of lm.Comol.Core.DomainModel.dtoModuleActionControl) Implements IViewModuleRenderAction.InitializeRemoteControl
        Return CurrentPresenter.InitRemoteControlView(idUser, initializer, actionsToDisplay, action)
    End Function
    Public Function InitializeRemoteControl(initializer As dtoObjectRenderInitializer, actionsToDisplay As lm.Comol.Core.DomainModel.StandardActionType, action As DisplayActionMode) As List(Of lm.Comol.Core.DomainModel.dtoModuleActionControl) Implements IViewModuleRenderAction.InitializeRemoteControl
        Return InitializeRemoteControl(PageUtility.CurrentContext.UserContext.CurrentUserID, initializer, actionsToDisplay, action)
    End Function
#End Region

#Region "Display Actions"
    Private Sub DisplayTextAction(item As dtoDisplayObjectRepositoryItem) Implements IViewRepositoryRenderAction.DisplayTextAction
        LBfileSize.Visible = (item.Type <> ItemType.Folder AndAlso item.Type <> ItemType.Link) AndAlso DisplaySize
        LBfileSize.Text = item.Size
        LTdescription.Text = item.Description
        LBauthor.Text = item.Owner
        LBlinkedBy.Text = item.LinkedBy
        If DisplayTags Then
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
        End If
        LTfileName.Visible = True
        HYPitemUrl.Visible = False
        Select Case item.Type
            Case ItemType.Folder
                LTfileName.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemFolderCssClass.Text)
            Case ItemType.Link
                LTfileName.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemUrlCssClass.Text)
            Case ItemType.Multimedia
                LTfileName.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemMultimediaCssClass.Text)
            Case ItemType.ScormPackage
                LTfileName.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemScormPackageCssClass.Text)
            Case ItemType.SharedDocument
            Case ItemType.File
                If item.Extension.StartsWith(".") Then
                    LTfileName.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemExtensionCssClass.Text & Replace(item.Extension, ".", ""))
                Else
                    LTfileName.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemExtensionCssClass.Text)
                End If
        End Select
        LTfileName.Text = Replace(LTfileName.Text, "#name#", item.DisplayName)
    End Sub

    Private Sub DisplayActiveAction(item As dtoDisplayObjectRepositoryItem, displayMode As DisplayMode, objUrl As String, refreshContainerPage As Boolean, onModalPage As Boolean, saveObjectExecution As Boolean, saveOwnerLinkExecution As Boolean) Implements IViewRepositoryRenderAction.DisplayActiveAction
        DisplayTextAction(item)
        Select Case item.Type
            Case ItemType.Folder
                'LNBitemUrl.CommandName = item.Id
                'LNBitemUrl.CommandArgument = item.IdentifierPath & "|" & item.FolderType.ToString
                'If Not AutoPostBack Then
                '    HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemFolderCssClass.Text)
                '    Select Case item.FolderType
                '        Case FolderType.standard
                '            HYPitemUrl.NavigateUrl = BaseUrl & RootObject.RepositoryItems(Type, idCommunity, idPerson, item.Id, item.Id, item.FolderType, order, ascending)
                '        Case Else
                '            HYPitemUrl.NavigateUrl = BaseUrl & RootObject.FolderUrlTemplate(item.Id, item.FolderType, item.IdentifierPath, Type, idCommunity, idPerson)
                '            HYPitemUrl.NavigateUrl = Replace(Replace(HYPitemUrl.NavigateUrl, "#OrderBy#", order.ToString), "#Boolean#", ascending.ToString().ToLower)
                '    End Select
                'Else
                '    LNBitemUrl.Text = Replace(LNBitemUrl.Text, "#ico#", LTitemFolderCssClass.Text)
                '    LNBitemUrl.Text = Replace(LNBitemUrl.Text, "#name#", item.DisplayName)
                'End If
            Case ItemType.Link
                Dim destinationUri As Uri = Nothing
                HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemUrlCssClass.Text)

                If String.IsNullOrWhiteSpace(item.Url) Then
                    HYPitemUrl.Enabled = False
                    HYPitemUrl.NavigateUrl = "#"
                Else
                    HYPitemUrl.Target = "_blank"
                    HYPitemUrl.NavigateUrl = item.Url
                    If Not (item.Url.ToLower().StartsWith("http://") OrElse item.Url.ToLower().StartsWith("https://")) Then
                        HYPitemUrl.NavigateUrl = "http://" & item.Url
                    End If
                    HYPitemUrl.Enabled = True
                End If

            Case ItemType.Multimedia
                HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemMultimediaCssClass.Text)
                If onModalPage Then
                    HYPitemUrl.CssClass = LTmodalCssClass.Text & " " & item.Type.ToString.ToLower()
                Else
                    Select Case item.DisplayMode
                        Case displayMode.inModal, displayMode.downloadOrPlayOrModal
                            HYPitemUrl.CssClass = LTmodalCssClass.Text & " " & item.Type.ToString.ToLower()
                        Case Else
                            HYPitemUrl.Target = "_blank"
                            If String.IsNullOrWhiteSpace(HYPitemUrl.Attributes("class")) Then
                                HYPitemUrl.CssClass = IIf(InsideOtherModule AndAlso displayMode <> DisplayActionMode.adminMode, IIf(refreshContainerPage, "fileRepositoryCookieNoBlockUI", "fileRepositoryCookieNoRefresh"), "")
                            Else
                                HYPitemUrl.Attributes("class") &= IIf(InsideOtherModule AndAlso displayMode <> DisplayActionMode.adminMode, IIf(refreshContainerPage, " fileRepositoryCookieNoBlockUI", " fileRepositoryCookieNoRefresh"), "")
                            End If
                    End Select
                End If
                HYPitemUrl.NavigateUrl = GetBaseUrl() & objUrl
            Case ItemType.ScormPackage
                HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemScormPackageCssClass.Text)
                If onModalPage Then
                    HYPitemUrl.CssClass = LTmodalCssClass.Text & " " & item.Type.ToString.ToLower()
                Else
                    Select Case item.DisplayMode
                        Case displayMode.inModal, lm.Comol.Core.FileRepository.Domain.DisplayMode.downloadOrPlayOrModal
                            HYPitemUrl.CssClass = LTmodalCssClass.Text & " " & item.Type.ToString.ToLower()
                        Case Else
                            HYPitemUrl.Target = "_blank"

                            If String.IsNullOrWhiteSpace(HYPitemUrl.Attributes("class")) Then
                                HYPitemUrl.CssClass = IIf(InsideOtherModule AndAlso displayMode <> DisplayActionMode.adminMode, IIf(refreshContainerPage, "fileRepositoryCookieNoBlockUI", "fileRepositoryCookieNoRefresh"), "")
                            Else
                                HYPitemUrl.Attributes("class") &= IIf(InsideOtherModule AndAlso displayMode <> DisplayActionMode.adminMode, IIf(refreshContainerPage, " fileRepositoryCookieNoBlockUI", " fileRepositoryCookieNoRefresh"), "")
                            End If
                    End Select
                End If

                HYPitemUrl.NavigateUrl = GetBaseUrl() & objUrl


            Case ItemType.SharedDocument
            Case ItemType.File
                HYPitemUrl.NavigateUrl = GetBaseUrl() & objUrl

                If item.Extension.StartsWith(".") Then
                    HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemExtensionCssClass.Text & Replace(item.Extension, ".", ""))
                Else
                    HYPitemUrl.Text = Replace(HYPitemUrl.Text, "#ico#", LTitemExtensionCssClass.Text)
                End If
                Select Case item.DisplayMode
                    Case displayMode.downloadOrPlayOrModal, displayMode.inModal
                        If item.AutoThumbnail Then
                            For Each i As String In LTpreviewImages.Text.Split("|")
                                HYPitemUrl.Attributes.Add(i.Split(":")(0), i.Split(":")(1))
                            Next
                        End If
                End Select
                If String.IsNullOrWhiteSpace(HYPitemUrl.Attributes("class")) Then
                    HYPitemUrl.CssClass = IIf(InsideOtherModule AndAlso displayMode <> DisplayActionMode.adminMode, IIf(refreshContainerPage, "fileRepositoryCookie", "fileRepositoryCookieNoRefresh"), "")
                Else
                    HYPitemUrl.Attributes("class") &= IIf(InsideOtherModule AndAlso displayMode <> DisplayActionMode.adminMode, IIf(refreshContainerPage, " fileRepositoryCookie", " fileRepositoryCookieNoRefresh"), "")
                End If
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
    End Sub

    Private Sub DisplayActions(actions As List(Of lm.Comol.Core.DomainModel.dtoModuleActionControl)) Implements IViewRepositoryRenderAction.DisplayActions
        RPTactions.Visible = Not (actions.Count = 0)
        RPTactions.DataSource = actions
        RPTactions.DataBind()
    End Sub

#End Region
    Private Sub DisplayRemovedObject() Implements IViewModuleRenderAction.DisplayRemovedObject
        Display = DisplayActionMode.none
        LTfileName.Text = Resource.getValue("action.RemovedObject")
    End Sub
    Public Sub DisplayEmptyAction() Implements IViewModuleRenderAction.DisplayEmptyAction
        Display = DisplayActionMode.none
        LTfileName.Text = ""
    End Sub
    Private Function GetIstanceBaseUrl() As String Implements IViewRepositoryRenderAction.GetIstanceBaseUrl
        Return PageUtility.BaseUrl
    End Function
    Private Function GetIstanceFullUrl() As String Implements IViewRepositoryRenderAction.GetIstanceFullUrl
        Return PageUtility.ApplicationUrlBase()
    End Function
    Private Function GetUnknownUserName() As String Implements IViewRepositoryRenderAction.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Sub DisplayPlaceHolders(items As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder)) Implements IViewRepositoryRenderAction.DisplayPlaceHolders
        Dim places As New Dictionary(Of PlaceHolderType, Integer)
        places.Add(PlaceHolderType.zero, 0)
        places.Add(PlaceHolderType.one, 1)
        places.Add(PlaceHolderType.two, 2)
        places.Add(PlaceHolderType.three, 3)
        places.Add(PlaceHolderType.four, 4)
        places.Add(PlaceHolderType.five, 5)

        For Each item As lm.Comol.Core.ModuleLinks.dtoPlaceHolder In items.Where(Function(i) i.Type <> PlaceHolderType.fullContainer AndAlso i.Type <> PlaceHolderType.none).ToList()
            Dim oLabel As Label = FindControl("LBplace" & places(item.Type))
            If Not IsNothing(oLabel) Then
                oLabel.Text = item.Text
                oLabel.Visible = True
                If Not String.IsNullOrEmpty(item.CssClass) Then
                    oLabel.CssClass = "plh plh" & places(item.Type).ToString() & " " & item.CssClass
                End If
            End If
        Next
    End Sub

    Private Function SanitizeLinkUrl(sourceUrl As String) As String Implements IViewRepositoryRenderAction.SanitizeLinkUrl
        Dim sUrl As String = sourceUrl
        If Not String.IsNullOrWhiteSpace(sUrl) Then
            If Not (sourceUrl.ToLower().StartsWith("http://") OrElse sourceUrl.ToLower().StartsWith("https://")) Then
                sUrl = "http://" & sourceUrl
            End If
        End If
        Return sUrl
    End Function


    Public Function GetDescriptionByLink(link As lm.Comol.Core.DomainModel.liteModuleLink, isGeneric As Boolean) As String Implements IViewModuleRenderAction.GetDescriptionByLink
        Return Me.CurrentPresenter.GetDescriptionByLink(link)
    End Function

    Public Function GetInLineDescriptionByLink(link As lm.Comol.Core.DomainModel.liteModuleLink, isGeneric As Boolean) As String Implements IViewModuleRenderAction.GetInLineDescriptionByLink
        Return Me.CurrentPresenter.GetDescriptionByLink(link)
    End Function

    Public Function GetDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements IViewModuleRenderAction.GetDescriptionByLink
        Return Me.CurrentPresenter.GetDescriptionByLink(link)
    End Function

    Private Function GetDisplayItemDescription(name As String, extension As String, fullName As String, type As ItemType, size As Long, displaySize As String) As String Implements IViewRepositoryRenderAction.GetDisplayItemDescription
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.File
                Return fullName & " (" & displaySize & ")"
            Case Else
                Return name
        End Select
        Return name
    End Function
#Region "To do"
    Private Sub DisplayTextAction(folderName As String, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType) Implements IViewRepositoryRenderAction.DisplayTextAction

    End Sub
    Private Sub DisplayActiveAction(folderName As String, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType) Implements IViewRepositoryRenderAction.DisplayActiveAction

    End Sub

#End Region
#End Region


    '#Region "Actions url"
    '    Public Function GetUrlForPlayScorm(idLink As Long, idItem As Long, uniqueId As System.Guid, idCommunity As Integer, idModule As Integer, notSaveStat As Boolean, refreshContainerPage As Boolean, onModalPage As Boolean, saveLinkExecution As Boolean) As String Implements IViewModuleRepositoryAction.GetUrlForPlayScorm
    '        Dim url As String = ""
    '        Dim BasePlayUrl As String = ""
    '        If SystemSettings.Icodeon.OverrideSSLsettings Then
    '            BasePlayUrl = Me.BaseUrlNoSSL
    '        Else
    '            BasePlayUrl = Me.BaseUrl
    '        End If
    '        url = BasePlayUrl & Me.PageUtility.SystemSettings.Icodeon.GetScormGenericModuleLink(idItem, uniqueId, Me.CurrentContext.UserContext.CurrentUserID, idModule, idCommunity, idLink, Me.PageUtility.LinguaCode, notSaveStat, refreshContainerPage, onModalPage, saveLinkExecution)
    '        Return url
    '    End Function

    '    Public Function GetUrlForPlayScorm(idLink As Long, idItem As Long, uniqueId As System.Guid, idCommunity As Integer, idModule As Integer, idAction As Integer, notSaveStat As Boolean, refreshContainerPage As Boolean, onModalPage As Boolean, saveLinkExecution As Boolean) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.GetUrlForPlayScorm
    '        Dim url As String = ""
    '        Dim BasePlayUrl As String = ""
    '        If SystemSettings.Icodeon.OverrideSSLsettings Then
    '            BasePlayUrl = Me.BaseUrlNoSSL
    '        Else
    '            BasePlayUrl = Me.BaseUrl
    '        End If
    '        url = BasePlayUrl & Me.PageUtility.SystemSettings.Icodeon.GetScormOtherModuleLinkUpdateAction(idItem, uniqueId, Me.CurrentContext.UserContext.CurrentUserID, idModule, idAction, idCommunity, idLink, Me.PageUtility.LinguaCode, notSaveStat, Me.PageUtility.UniqueGuidSession, refreshContainerPage, onModalPage, saveLinkExecution)
    '        Return url

    '    End Function
    '#End Region

    '#End Region

    '#Region "Display Actions"
    '    Private Sub DisplayActions(actions As List(Of dtoModuleActionControl)) Implements IViewModuleRepositoryAction.DisplayActions
    '        If actions.Count = 0 Then
    '            Me.RPTactions.Visible = False
    '        End If
    '        Me.RPTactions.DataSource = actions
    '        Me.RPTactions.DataBind()
    '        Me.MLVcontrol.SetActiveView(VIWdata)
    '    End Sub
    '    Private Sub DisplayEmptyActions() Implements IViewModuleRepositoryAction.DisplayEmptyActions
    '        Me.RPTactions.Visible = False
    '    End Sub

    '    Private Function GetUrlForMultimediaFile() As String Implements IViewModuleRepositoryAction.GetUrlForMultimediaFile
    '        Return PageUtility.BaseUrl(False)
    '    End Function
    '    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
    '        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim action As dtoModuleActionControl = DirectCast(e.Item.DataItem, dtoModuleActionControl)
    '            Dim link As HyperLink = e.Item.FindControl("HYPaction")
    '            link.Target = IIf(action.isPopupUrl, "_blank", "")
    '            link.CssClass = "action "
    '            Select Case action.ControlType
    '                Case StandardActionType.Play
    '                    If action.LinkUrl.ToLower.Contains(".repository") Then
    '                        link.CssClass &= "download" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, " fileRepositoryCookie", "")

    '                    ElseIf action.LinkUrl.ToLower.Contains("scormplayerloader.aspx") Then
    '                        link.CssClass &= "scorm" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, " fileRepositoryCookieNoBlockUI", " ")
    '                    ElseIf action.LinkUrl.ToLower.Contains("multimediafileloader.aspx") Then
    '                        link.CssClass &= "multimedia" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, " fileRepositoryCookieNoBlockUI", " ")
    '                    End If
    '                    link.ToolTip = Me.Resource.getValue("action.RepositoryItemType." & ItemType.ToString)
    '                Case StandardActionType.ViewAdvancedStatistics
    '                    link.CssClass &= "stats" & IconSizeToString
    '                    link.ToolTip = Me.Resource.getValue("statistic.RepositoryItemType." & ItemType.ToString)
    '                Case StandardActionType.ViewPersonalStatistics
    '                    link.CssClass &= "stats" & IconSizeToString
    '                    link.ToolTip = Me.Resource.getValue("statistic.RepositoryItemType." & ItemType.ToString)
    '                Case StandardActionType.ViewUserStatistics
    '                    link.CssClass &= "stats" & IconSizeToString
    '                    link.ToolTip = Me.Resource.getValue("statistic.RepositoryItemType." & ItemType.ToString)
    '                Case StandardActionType.EditMetadata
    '                    link.CssClass &= "config" & IconSizeToString
    '                    link.ToolTip = Me.Resource.getValue("settings.RepositoryItemType." & ItemType.ToString)
    '                Case StandardActionType.EditPermission
    '                    link.CssClass &= "permissions" & IconSizeToString
    '                Case StandardActionType.DownloadItem
    '                    link.CssClass &= "download" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, IIf(ItemType = Repository.RepositoryItemType.FileStandard, " fileRepositoryCookie", " fileRepositoryCookieNoBlockUI"), "")
    '                    link.ToolTip = Me.Resource.getValue("action.RepositoryItemType." & ItemType.ToString)
    '            End Select
    '            link.Text = " " 'action.ControlType.ToString().Chars(0)
    '            If Not (ItemType = Repository.RepositoryItemType.ScormPackage AndAlso action.ControlType = StandardActionType.Play) Then
    '                link.NavigateUrl = IIf(action.LinkUrl.StartsWith(PageUtility.BaseUrl(False)), action.LinkUrl, PageUtility.BaseUrl(False) & action.LinkUrl)
    '            Else
    '                link.NavigateUrl = action.LinkUrl
    '            End If
    '        End If
    '    End Sub
    '#End Region

    '#Region "Display Folder"
    '    Public Sub DisplayCreateFolder(folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayCreateFolder

    '    End Sub
    '    Public Sub DisplayFolder(folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayFolder

    '    End Sub

    '    Public Sub DisplayFolder(name As String, url As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayFolder

    '    End Sub

    '    Public Sub DisplayFolderAction(idFolder As Long, folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayFolderAction

    '    End Sub
    '#End Region

    '#Region "Display Upload"

    '    Public Sub DisplayUploadAction(idFolder As Long, folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayUploadAction

    '    End Sub

    '    Public Sub DisplayUploadFile(folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayUploadFile

    '    End Sub



    '#End Region
    '#Region "Display item"
    '    Public Sub DisplayItem(name As String, extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements IViewModuleRepositoryAction.DisplayItem
    '        Me.LBfileName.Visible = True
    '        Me.LBfileName.Text = name
    '        BaseDisplayItem(extension, size, type)
    '    End Sub
    '    Public Sub DisplayItem(name As String, url As String, extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType, refreshContainerPage As Boolean, saveLinkExecution As Boolean) Implements IViewModuleRepositoryAction.DisplayItem
    '        Me.HYPfileName.NavigateUrl = PageUtility.BaseUrl & url
    '        Me.HYPfileName.Target = "_blank"
    '        Me.HYPfileName.Text = name

    '        Me.HYPfileName.ToolTip = Resource.getValue("fileTitle") & " " & name & " (" & GetFileSize(size) & ")."
    '        Me.HYPfileName.CssClass = IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, IIf(refreshContainerPage, "fileRepositoryCookie", "fileRepositoryCookieNoRefresh"), "")
    '        Me.HYPfileName.Visible = True
    '        BaseDisplayItem(extension, size, type)
    '    End Sub
    '    Public Sub DisplayPlayItem(name As String, url As String, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType, additionalCssClass As String, refreshContainerPage As Boolean, onModalPage As Boolean, saveLinkExecution As Boolean) Implements IViewModuleRepositoryAction.DisplayPlayItem
    '        Dim cssClass As String = ""
    '        If Not String.IsNullOrWhiteSpace(additionalCssClass) AndAlso type = Repository.RepositoryItemType.Multimedia Then
    '            additionalCssClass = Replace(additionalCssClass, "scorm", "multimedia")
    '        End If

    '        If (InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode) Then
    '            cssClass = "fileRepositoryCookieNoBlockUI"
    '            If onModalPage AndAlso Not String.IsNullOrWhiteSpace(additionalCssClass) Then
    '                cssClass = additionalCssClass
    '            End If
    '        Else
    '            cssClass = additionalCssClass
    '        End If

    '        Me.HYPfileName.NavigateUrl = url
    '        Me.HYPfileName.Target = "_blank"
    '        Me.HYPfileName.Text = name
    '        Me.HYPfileName.ToolTip = Me.Resource.getValue("action.RepositoryItemType." & type.ToString) & " " & name

    '        Me.HYPfileName.CssClass = cssClass
    '        Me.HYPfileName.Visible = True
    '        BaseDisplayItem("", 0, type)
    '    End Sub
    '    Private Sub BaseDisplayItem(extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType)
    '        Me.MLVcontrol.SetActiveView(VIWdata)
    '        Me.LBsize.Text = GetFileSize(size)
    '        Me.LBsize.Visible = (type = Repository.RepositoryItemType.FileStandard)

    '        Dim ico As String = "<span class=""fileIco {0}"">&nbsp;</span>"

    '        Me.LBfileName.Text = String.Format(ico, GetIconClass(extension, type)) & Me.LBfileName.Text
    '        Me.HYPfileName.Text = String.Format(ico, GetIconClass(extension, type)) & Me.HYPfileName.Text
    '        Me.LBfileName.Visible = False
    '        Me.HYPfileName.Visible = False
    '    End Sub


    '#End Region




    '    Public Function getDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements IGenericModuleDisplayAction.getDescriptionByLink
    '        Return Me.CurrentPresenter.GetDescriptionByLink(link)
    '    End Function

    '    Public Function GetInLineDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements IGenericModuleDisplayAction.GetInLineDescriptionByLink
    '        Return Me.CurrentPresenter.GetDescriptionByLink(link)
    '    End Function
    '    Public Function GetDisplayItemDescription(name As String, extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.GetDisplayItemDescription
    '        Dim result As String = name
    '        If type = Repository.RepositoryItemType.FileStandard Then
    '            result &= " (" & GetFileSize(size) & ")"
    '        End If
    '        Return result
    '    End Function

    '#Region "to Do"
    '    Public Function CreateFolderDescription(name As String) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.CreateFolderDescription
    '        Return name
    '    End Function
    '    Public Function FolderDescription(name As String) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.FolderDescription
    '        Return name
    '    End Function
    '    Public Function UploadFileDescription(name As String) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.UploadFileDescription
    '        Return name
    '    End Function
    '#End Region











   
End Class