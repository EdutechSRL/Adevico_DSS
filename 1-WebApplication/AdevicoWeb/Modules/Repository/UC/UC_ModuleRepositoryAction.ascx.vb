Imports lm.Comol.Core.ModuleLinks
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.Core.DomainModel

Public Class UC_ModuleRepositoryAction
    Inherits BaseControl
    Implements IViewModuleRepositoryAction

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleRepositoryActionPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleRepositoryActionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleRepositoryActionPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Base"
    Public Property ContainerCSS As String Implements IGenericModuleDisplayAction.ContainerCSS
        Get
            Return ViewStateOrDefault("ContainerCSS", "")
        End Get
        Set(value As String)
            ViewState("ContainerCSS") = value
        End Set
    End Property
    Public Property Display As DisplayActionMode Implements IGenericModuleDisplayAction.Display
        Get
            Return ViewStateOrDefault("Display", DisplayActionMode.defaultAction)
        End Get
        Set(value As DisplayActionMode)
            ViewState("Display") = value
            If value = DisplayActionMode.none Then
                Me.MLVcontrol.SetActiveView(VIWempty)
                Me.LBempty.Text = " "
            Else
                Me.MLVcontrol.SetActiveView(VIWdata)
                Me.RPTactions.Visible = ((value And DisplayActionMode.actions) > 0)
                Me.LBfileName.Visible = ((value And DisplayActionMode.text) > 0)
                If (value And DisplayActionMode.text) > 0 OrElse (value And DisplayActionMode.textDefault) > 0 Then
                    Me.HYPfileName.Visible = False
                ElseIf (value And DisplayActionMode.defaultAction) > 0 OrElse ((value And DisplayActionMode.adminMode) > 0) Then
                    Me.HYPfileName.Visible = True
                    LBfileName.Visible = False
                End If
            End If
        End Set
    End Property
    Public Property IconSize As lm.Comol.Core.DomainModel.Helpers.IconSize Implements IGenericModuleDisplayAction.IconSize
        Get
            Return ViewStateOrDefault("IconSize", lm.Comol.Core.DomainModel.Helpers.IconSize.Medium)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Helpers.IconSize)
            ViewState("IconSize") = value
        End Set
    End Property
    Private ReadOnly Property IconSizeToString As String
        Get
            Dim result As String = ViewStateOrDefault("IconSizeToString", "")
            If String.IsNullOrEmpty(result) Then
                Select Case IconSize
                    Case Helpers.IconSize.Large
                        result = "_l"
                    Case Helpers.IconSize.Medium
                        result = "_m"
                    Case Helpers.IconSize.Small
                        result = "_s"
                    Case Helpers.IconSize.Smaller
                        result = "_xs"
                End Select
                ViewState("IconSizeToString") = result
            End If
            Return result
        End Get
    End Property
    Public Property ShortDescription As Boolean Implements IGenericModuleDisplayAction.ShortDescription
        Get
            Return ViewStateOrDefault("ShortDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("ShortDescription") = value
        End Set
    End Property
  
#End Region
    Public Property IsReadyToPlay As Boolean Implements IViewModuleRepositoryAction.IsReadyToPlay
        Get
            Return ViewStateOrDefault("IsReadyToPlay", True)
        End Get
        Set(value As Boolean)
            ViewState("IsReadyToPlay") = value
        End Set
    End Property
    Private Property ServiceCode As String Implements IViewModuleRepositoryAction.ServiceCode
        Get
            Return ViewStateOrDefault("ServiceCode", "")
        End Get
        Set(value As String)
            ViewState("ServiceCode") = value
        End Set
    End Property
    Private Property ServiceID As Integer Implements IViewModuleRepositoryAction.ServiceID
        Get
            Return ViewStateOrDefault("ServiceID", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("ServiceID") = value
        End Set
    End Property

    Public Property ForUserId As Integer Implements IViewModuleRepositoryAction.ForUserId
        Get
            Return ViewStateOrDefault("ForUserId", CurrentContext.UserContext.CurrentUserID)
        End Get
        Set(value As Integer)
            ViewState("ForUserId") = value
        End Set
    End Property
    Public Property InsideOtherModule As Boolean Implements IViewModuleRepositoryAction.InsideOtherModule
        Get
            Return ViewStateOrDefault("InsideOtherModule", False)
        End Get
        Set(value As Boolean)
            ViewState("InsideOtherModule") = value
        End Set
    End Property
    Private Property ItemIdentifier As String Implements IViewModuleRepositoryAction.ItemIdentifier
        Get
            Return ViewStateOrDefault("ItemIdentifier", "")
        End Get
        Set(value As String)
            ViewState("ItemIdentifier") = value
            If String.IsNullOrEmpty(value) Then
                LTidentifier.Visible = False
            Else
                LTidentifier.Text = "<a name=""" & value & """> </a>"
                LTidentifier.Visible = True
            End If
        End Set
    End Property
    Public Property ItemType As lm.Comol.Core.DomainModel.Repository.RepositoryItemType Implements IViewModuleRepositoryAction.ItemType
        Get
            Return ViewStateOrDefault("ItemType", lm.Comol.Core.DomainModel.Repository.RepositoryItemType.None)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Repository.RepositoryItemType)
            ViewState("ItemType") = value
        End Set
    End Property
    Private ReadOnly Property PreLoadedContentView As ContentView Implements IViewModuleRepositoryAction.PreLoadedContentView
        Get
            Return PageUtility.PreLoadedContentView
        End Get
    End Property

    Private ReadOnly Property WorkingSessionId As System.Guid Implements IViewModuleRepositoryAction.WorkingSessionId
        Get
            Return PageUtility.UniqueGuidSession
        End Get
    End Property
    Private ReadOnly Property DestinationUrl As String Implements IViewModuleRepositoryAction.DestinationUrl
        Get
            Dim url As String = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query & IIf(EnableAnchor, "#" & ItemIdentifier, ""))
            Return url
        End Get
    End Property
    Public Function AllowDownload(type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) As Boolean Implements IViewModuleRepositoryAction.AllowDownload
        Return SystemSettings.Presenter.RepositoryConfiguration.AllowDownload.Contains(type)
    End Function
    Public Property EnableAnchor As Boolean Implements IViewModuleRepositoryAction.EnableAnchor
        Get
            Return ViewStateOrDefault("EnableAnchor", False)
        End Get
        Set(value As Boolean)
            ViewState("EnableAnchor") = value
        End Set
    End Property
    Private ReadOnly Property GetBaseUrl As String Implements IViewModuleRepositoryAction.GetBaseUrl
        Get
            Return PageUtility.BaseUrl
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Private _BaseUrlNoSSL As String
    Private Overloads ReadOnly Property BaseUrlNoSSL() As String
        Get
            If _BaseUrlNoSSL = "" Then
                _BaseUrlNoSSL = Me.PageUtility.ApplicationUrlBase()
                If Not _BaseUrlNoSSL.EndsWith("/") Then
                    _BaseUrlNoSSL &= "/"
                End If
            End If
            Return _BaseUrlNoSSL
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToRepository", "Modules", "Repository")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"

#Region "Initializers"
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Function InitializeRemoteControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Function InitializeRemoteControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        ForUserId = idUser
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
#End Region

    Public Sub DisplayRemovedObject() Implements IGenericModuleDisplayAction.DisplayRemovedObject
        Me.LBempty.Text = Resource.getValue("action.RemovedObject")
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Public Sub DisplayEmptyAction() Implements IViewModuleRepositoryAction.DisplayEmptyAction
        Me.LBempty.Text = "&nbsp;"
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub

#Region "Actions url"
    Public Function GetUrlForPlayScorm(idLink As Long, idItem As Long, uniqueId As System.Guid, idCommunity As Integer, idModule As Integer, notSaveStat As Boolean) As String Implements IViewModuleRepositoryAction.GetUrlForPlayScorm
        Dim url As String = ""

        Return url
    End Function

    Public Function GetUrlForPlayScorm(idLink As Long, idItem As Long, uniqueId As System.Guid, idCommunity As Integer, idModule As Integer, idAction As Integer, notSaveStat As Boolean) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.GetUrlForPlayScorm
        Dim url As String = ""
        Dim BasePlayUrl As String = ""

        BasePlayUrl = Me.BaseUrl

        url = ""
        Return url

    End Function
#End Region

#End Region

    Private Function GetFileSize(ByVal size As Long) As String
        Dim sizeTranslated As String = ""
        If size = 0 Then
            sizeTranslated = "&nbsp;"
        Else
            If size < 1024 Then
                sizeTranslated = "1 kb"
            Else
                size = size / 1024
                If size < 1024 Then
                    sizeTranslated = Math.Round(size) & " kb"
                ElseIf size >= 1024 Then
                    sizeTranslated = Math.Round(size / 1024, 2) & " mb"
                End If
            End If
        End If
        Return sizeTranslated
    End Function

#Region "Display Actions"
    Private Sub DisplayActions(actions As List(Of dtoModuleActionControl)) Implements IViewModuleRepositoryAction.DisplayActions
        If actions.Count = 0 Then
            Me.RPTactions.Visible = False
        End If
        Me.RPTactions.DataSource = actions
        Me.RPTactions.DataBind()
        Me.MLVcontrol.SetActiveView(VIWdata)
    End Sub
    Private Sub DisplayEmptyActions() Implements IViewModuleRepositoryAction.DisplayEmptyActions
        Me.RPTactions.Visible = False
    End Sub

    Private Function GetUrlForMultimediaFile() As String Implements IViewModuleRepositoryAction.GetUrlForMultimediaFile
        Return PageUtility.BaseUrl(False)
    End Function
    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim action As dtoModuleActionControl = DirectCast(e.Item.DataItem, dtoModuleActionControl)
            Dim link As HyperLink = e.Item.FindControl("HYPaction")
            link.Target = IIf(action.isPopupUrl, "_blank", "")
            link.CssClass = "action "
            Select Case action.ControlType
                Case StandardActionType.Play
                    If action.LinkUrl.ToLower.Contains(".repository") Then
                        link.CssClass &= "download" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, " fileRepositoryCookie", "")

                    ElseIf action.LinkUrl.ToLower.Contains("scormplayerloader.aspx") Then
                        link.CssClass &= "scorm" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, " fileRepositoryCookieNoBlockUI", " ")
                    ElseIf action.LinkUrl.ToLower.Contains("multimediafileloader.aspx") Then
                        link.CssClass &= "multimedia" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, " fileRepositoryCookieNoBlockUI", " ")
                    End If
                    link.ToolTip = Me.Resource.getValue("action.RepositoryItemType." & ItemType.ToString)
                Case StandardActionType.ViewAdvancedStatistics
                    link.CssClass &= "stats" & IconSizeToString
                    link.ToolTip = Me.Resource.getValue("statistic.RepositoryItemType." & ItemType.ToString)
                Case StandardActionType.ViewPersonalStatistics
                    link.CssClass &= "stats" & IconSizeToString
                    link.ToolTip = Me.Resource.getValue("statistic.RepositoryItemType." & ItemType.ToString)
                Case StandardActionType.ViewUserStatistics
                    link.CssClass &= "stats" & IconSizeToString
                    link.ToolTip = Me.Resource.getValue("statistic.RepositoryItemType." & ItemType.ToString)
                Case StandardActionType.EditMetadata
                    link.CssClass &= "config" & IconSizeToString
                    link.ToolTip = Me.Resource.getValue("settings.RepositoryItemType." & ItemType.ToString)
                Case StandardActionType.EditPermission
                    link.CssClass &= "permissions" & IconSizeToString
                Case StandardActionType.DownloadItem
                    link.CssClass &= "download" & IconSizeToString & IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, IIf(ItemType = Repository.RepositoryItemType.FileStandard, " fileRepositoryCookie", " fileRepositoryCookieNoBlockUI"), "")
                    link.ToolTip = Me.Resource.getValue("action.RepositoryItemType." & ItemType.ToString)
            End Select
            link.Text = " " 'action.ControlType.ToString().Chars(0)
            If Not (ItemType = Repository.RepositoryItemType.ScormPackage AndAlso action.ControlType = StandardActionType.Play) Then
                link.NavigateUrl = IIf(action.LinkUrl.StartsWith(PageUtility.BaseUrl(False)), action.LinkUrl, PageUtility.BaseUrl(False) & action.LinkUrl)
            Else
                link.NavigateUrl = action.LinkUrl
            End If
        End If
    End Sub
#End Region

#Region "Display Folder"
    Public Sub DisplayCreateFolder(folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayCreateFolder

    End Sub
    Public Sub DisplayFolder(folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayFolder

    End Sub

    Public Sub DisplayFolder(name As String, url As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayFolder

    End Sub

    Public Sub DisplayFolderAction(idFolder As Long, folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayFolderAction

    End Sub
#End Region

#Region "Display Upload"

    Public Sub DisplayUploadAction(idFolder As Long, folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayUploadAction

    End Sub

    Public Sub DisplayUploadFile(folderName As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.DisplayUploadFile

    End Sub



#End Region
#Region "Display item"
    Public Sub DisplayItem(name As String, extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements IViewModuleRepositoryAction.DisplayItem
        'Me.LBfileName.Visible = True
        Me.LBfileName.Text = name
        BaseDisplayItem(extension, size, type)
    End Sub
    Public Sub DisplayItem(name As String, url As String, extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements IViewModuleRepositoryAction.DisplayItem
        Me.HYPfileName.NavigateUrl = PageUtility.BaseUrl & url
        Me.HYPfileName.Target = "_blank"
        Me.HYPfileName.Text = name
        Me.HYPfileName.ToolTip = Resource.getValue("fileTitle") & " " & name & " (" & GetFileSize(size) & ")."
        Me.HYPfileName.CssClass = IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, "fileRepositoryCookie", "")
        'Me.HYPfileName.Visible = True
        BaseDisplayItem(extension, size, type)
    End Sub
    Public Sub DisplayPlayItem(name As String, url As String, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements IViewModuleRepositoryAction.DisplayPlayItem
        Me.HYPfileName.NavigateUrl = url
        Me.HYPfileName.Target = "_blank"
        Me.HYPfileName.Text = name
        Me.HYPfileName.ToolTip = Me.Resource.getValue("action.RepositoryItemType." & type.ToString) & " " & name
        Me.HYPfileName.CssClass = IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, "fileRepositoryCookieNoBlockUI", "")
        'Me.HYPfileName.Visible = True
        BaseDisplayItem("", 0, type)
    End Sub
    Private Sub BaseDisplayItem(extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType)
        Me.MLVcontrol.SetActiveView(VIWdata)
        Me.LBsize.Text = GetFileSize(size)
        Me.LBsize.Visible = (type = Repository.RepositoryItemType.FileStandard)

        Dim ico As String = "<span class=""fileIco {0}"">&nbsp;</span>"

        Me.LBfileName.Text = String.Format(ico, GetIconClass(extension, type)) & Me.LBfileName.Text
        Me.HYPfileName.Text = String.Format(ico, GetIconClass(extension, type)) & Me.HYPfileName.Text
        'Me.LBfileName.Visible = False
        'Me.HYPfileName.Visible = False
    End Sub

    Private Function GetIconClass(extension As String, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) As String
        Select Case type
            Case Repository.RepositoryItemType.Folder
                Return "folder"
            Case Repository.RepositoryItemType.Multimedia
                Return "multimedia"
            Case Repository.RepositoryItemType.ScormPackage
                Return "scorm"
            Case Repository.RepositoryItemType.VideoStreaming
                Return "streaming"
            Case Else
                Return "ext" + extension.Replace(".", "")
        End Select
    End Function
#End Region



    Public Sub DisplayPlaceHolders(items As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder)) Implements IViewModuleRepositoryAction.DisplayPlaceHolders
        Dim places As New Dictionary(Of PlaceHolderType, Integer)
        places.Add(PlaceHolderType.zero, 0)
        places.Add(PlaceHolderType.one, 1)
        places.Add(PlaceHolderType.two, 2)
        places.Add(PlaceHolderType.three, 3)
        places.Add(PlaceHolderType.four, 4)

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

    Public Function getDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements IGenericModuleDisplayAction.getDescriptionByLink
        Return Me.CurrentPresenter.GetDescriptionByLink(link)
    End Function

    Public Function GetInLineDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements IGenericModuleDisplayAction.GetInLineDescriptionByLink
        Return Me.CurrentPresenter.GetDescriptionByLink(link)
    End Function
    Public Function GetDisplayItemDescription(name As String, extension As String, size As Long, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.GetDisplayItemDescription
        Dim result As String = name
        If type = Repository.RepositoryItemType.FileStandard Then
            result &= " (" & GetFileSize(size) & ")"
        End If
        Return result
    End Function

#Region "to Do"
    Public Function CreateFolderDescription(name As String) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.CreateFolderDescription
        Return name
    End Function
    Public Function FolderDescription(name As String) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.FolderDescription
        Return name
    End Function
    Public Function UploadFileDescription(name As String) As String Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleRepositoryAction.UploadFileDescription
        Return name
    End Function
#End Region



End Class