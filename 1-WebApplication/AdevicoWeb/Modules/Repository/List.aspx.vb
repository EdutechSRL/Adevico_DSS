Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public Class RepositoryList
    Inherits FRrepositoryPageBase
    Implements IViewRepository


#Region "Context"
    Private _Presenter As RepositoryPresenter
    Public ReadOnly Property CurrentPresenter() As RepositoryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RepositoryPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdFolder As Long Implements IViewRepository.PreloadIdFolder
        Get
            If IsNumeric(Me.Request.QueryString("idFolder")) Then
                Return CLng(Me.Request.QueryString("idFolder"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadFolderType As lm.Comol.Core.FileRepository.Domain.FolderType Implements IViewRepository.PreloadFolderType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.FileRepository.Domain.FolderType).GetByString(Request.QueryString("ftype"), lm.Comol.Core.FileRepository.Domain.FolderType.standard)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadOrderBy As lm.Comol.Core.FileRepository.Domain.OrderBy Implements IViewRepository.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.FileRepository.Domain.OrderBy).GetByString(Request.QueryString("o"), lm.Comol.Core.FileRepository.Domain.OrderBy.name)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadAscending As Boolean Implements IViewRepository.PreloadAscending
        Get
            Dim ascending As Boolean = True
            Dim order As OrderBy = PreloadOrderBy
            If String.IsNullOrWhiteSpace(Request.QueryString("asc")) Then
                Select Case order
                    Case OrderBy.date
                        ascending = False
                End Select
            Else
                ascending = Request.QueryString("asc").ToLower() = "true"
            End If

            Return ascending
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadLoadFromCookies As Boolean Implements IViewRepository.PreloadLoadFromCookies
        Get
            Dim fromCookies As Boolean = False
            If Not String.IsNullOrWhiteSpace(Request.QueryString("fromCookies")) Then
                Boolean.TryParse(Request.QueryString("fromCookies"), fromCookies)
            End If

            Return fromCookies
        End Get
    End Property
#End Region
   
    Private Property CurrentOrderBy As OrderBy Implements IViewRepository.CurrentOrderBy
        Get
            Return CTRLtree.CurrentOrderBy
        End Get
        Set(value As OrderBy)
            CTRLtree.CurrentOrderBy = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewRepository.CurrentAscending
        Get
            Return CTRLtree.CurrentAscending ' ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(value As Boolean)
            CTRLtree.CurrentAscending = value
            'ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property Columns As List(Of lm.Comol.Core.BaseModules.FileRepository.Domain.Column) Implements IViewRepository.Columns
        Get
            Return ViewStateOrDefault("Columns", New List(Of lm.Comol.Core.BaseModules.FileRepository.Domain.Column))
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.FileRepository.Domain.Column))
            ViewState("Columns") = value
        End Set
    End Property
    Private Property IdCurrentFolder As Long Implements IViewRepository.IdCurrentFolder
        Get
            Return ViewStateOrDefault("IdCurrentFolder", 0)
        End Get
        Set(value As Long)
            ViewState("IdCurrentFolder") = value
        End Set
    End Property
    Private Property CurrentSelectedItems As List(Of Long) Implements IViewRepository.CurrentSelectedItems
        Get
            Return ViewStateOrDefault("CurrentSelectedItems", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("CurrentSelectedItems") = value
        End Set
    End Property
    Private Property CurrentPath As String Implements IViewRepository.CurrentPath
        Get
            Return ViewStateOrDefault("CurrentPath", "/")
        End Get
        Set(value As String)
            ViewState("CurrentPath") = value
        End Set
    End Property
    Private Property CurrentFolderIdentifierPath As String Implements IViewRepository.CurrentFolderIdentifierPath
        Get
            Return ViewStateOrDefault("CurrentFolderIdentifierPath", "")
        End Get
        Set(value As String)
            ViewState("CurrentFolderIdentifierPath") = value
        End Set
    End Property
    Private Property CurrentFolderType As FolderType Implements IViewRepository.CurrentFolderType
        Get
            Return ViewStateOrDefault("CurrentFolderType", FolderType.standard)
        End Get
        Set(value As FolderType)
            ViewState("CurrentFolderType") = value
        End Set
    End Property

    Private Property IsInitialized As Boolean Implements IViewRepository.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Private _RepositoryIdentifier As String
    Public Property RepositoryIdentifier As String Implements IViewRepository.RepositoryIdentifier
        Get
            If Not String.IsNullOrWhiteSpace(_RepositoryIdentifier) Then
                Return _RepositoryIdentifier
            Else
                Return ViewStateOrDefault("RepositoryIdentifier", "")
            End If
        End Get
        Set(value As String)
            _RepositoryIdentifier = value
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Private Property ReorderColumnHidden As Boolean
        Get
            Return ViewStateOrDefault("ReorderColumnHidden", False)
        End Get
        Set(value As Boolean)
            ViewState("ReorderColumnHidden") = value
        End Set
    End Property
    Private _noItemsToDisplay As Boolean
    Private _displayPath As Boolean
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        _noItemsToDisplay = True
        If PreloadLoadFromCookies Then
            CurrentPresenter.InitViewFromCookies(PreloadType, PreloadIdFolder, PreloadIdentifierPath, PreloadFolderType, PreloadIdCommunity)
        Else
            CurrentPresenter.InitView(PreloadType, PreloadIdFolder, PreloadIdentifierPath, PreloadFolderType, PreloadIdCommunity)
        End If

    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim url As String = ""
        If IsInitialized Then
            url = RootObject.RepositoryItems(RepositoryType, RepositoryIdCommunity, 0, IdCurrentFolder, CurrentFolderType, CurrentOrderBy, CurrentAscending, CurrentFolderIdentifierPath)
        ElseIf String.IsNullOrWhiteSpace(PreloadIdentifierPath) Then
            If Not IsInitialized Then
                url = RootObject.RepositoryItems(RepositoryType, RepositoryIdCommunity, PreloadIdItem, PreloadIdFolder, PreloadFolderType, PreloadOrderBy, PreloadAscending)
            End If
        End If

        If String.IsNullOrWhiteSpace(url) Then
            url = Replace(Request.Url.AbsolutePath, PageUtility.ApplicationUrlBase(), "")
            url = Replace(url, PageUtility.ApplicationUrlBase(True), "")
        End If
        RedirectOnSessionTimeOut(url, RepositoryIdCommunity)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTtitleName)
            .setLiteral(LTtitleDate)
            .setLiteral(LTtitleStatistics)
            .setLabel(LBtitleActionInfo)
            LBtitleActionInfo.ToolTip = .getValue("LBtitleActionInfo.ToolTip")
        End With
    End Sub
#End Region

#Region "Implements"
#Region "Messages"
    Private Sub DisplayUnknownCommunity() Implements IViewRepository.DisplayUnknownCommunity
        MLVcontent.SetActiveView(VIWempty)
        CTRLmessages.InitializeControl(Resource.getValue("IViewRepository.DisplayUnknownCommunity"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnknownUser() Implements IViewRepository.DisplayUnknownUser
        MLVcontent.SetActiveView(VIWempty)
        CTRLmessages.InitializeControl(Resource.getValue("IViewRepository.DisplayUnknownUser"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayMessage(action As ItemAction, executed As Boolean, name As String, extension As String, type As ItemType, Optional ByVal startFolder As String = "", Optional ByVal endFolder As String = "") Implements IViewRepository.DisplayMessage
        Dim key As String = "IViewRepository.DisplayMessage.Executed." & executed.ToString() & ".ItemAction." & action.ToString
        Dim message As String = ""
        Select Case action
            Case ItemAction.move
                Select Case executed
                    Case True
                        message = String.Format(Resource.getValue(key), GetFilenameRender(name, extension, type), GetFilenameRender(startFolder, "", ItemType.Folder), GetFilenameRender(endFolder, "", ItemType.Folder))
                    Case Else
                        message = String.Format(Resource.getValue(key), GetFilenameRender(name, extension, type), GetFilenameRender(endFolder, "", ItemType.Folder))
                End Select

            Case Else
                message = String.Format(Resource.getValue(key), GetFilenameRender(name, extension, type))
        End Select
        DisplayMessage(message, IIf(executed, lm.Comol.Core.DomainModel.Helpers.MessageType.success, lm.Comol.Core.DomainModel.Helpers.MessageType.error))
    End Sub
    Private Sub DisplayUnavailableItem(action As ItemAction) Implements IViewRepository.DisplayUnavailableItem
        DisplayMessage(Resource.getValue("IViewRepository.DisplayUnavailableItem.ItemAction." & action.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnavailableItem(action As ItemAction, folderName As String) Implements IViewRepository.DisplayUnavailableItem
        DisplayMessage(String.Format(Resource.getValue("IViewRepository.DisplayUnavailableItem.ItemAction." & action.ToString), GetFilenameRender(folderName, "", ItemType.Folder)), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnavailableItems(action As ItemAction) Implements IViewRepository.DisplayUnavailableItems
        DisplayMessage(Resource.getValue("IViewRepository.DisplayUnavailableItems.ItemAction." & action.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayMessage(action As ItemAction, executed As Boolean, executedItems As Dictionary(Of ItemType, Long), startItems As Dictionary(Of ItemType, Long), Optional startFolder As String = "", Optional endFolder As String = "") Implements IViewRepository.DisplayMessage
        Dim key As String = "IViewRepository.DisplayMessage.MultipleActions.Executed." & executed.ToString() & ".ItemAction." & action.ToString
        Dim message As String = ""
        Select Case action
            Case ItemAction.move
                Select Case executed
                    Case True
                        message = String.Format(Resource.getValue(key), GetTranslatedItems(executed, executedItems, startItems), GetFilenameRender(startFolder, "", ItemType.Folder), GetFilenameRender(endFolder, "", ItemType.Folder))
                    Case Else
                        message = String.Format(Resource.getValue(key), GetTranslatedItems(executed, executedItems, startItems), GetFilenameRender(endFolder, "", ItemType.Folder))
                End Select

            Case Else
                message = String.Format(Resource.getValue(key), GetTranslatedItems(executed, executedItems, startItems))
        End Select
        DisplayMessage(message, IIf(executed, lm.Comol.Core.DomainModel.Helpers.MessageType.success, lm.Comol.Core.DomainModel.Helpers.MessageType.error))
    End Sub

    Private Sub DisplayUnknownItem(action As ItemAction, Optional multiple As Boolean = False) Implements IViewRepository.DisplayUnknownItem
        DisplayMessage(Resource.getValue("IViewRepository.DisplayUnknownItem.ItemAction." & action.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnableToInitialize(action As ItemAction, name As String, extension As String, type As ItemType) Implements IViewRepository.DisplayUnableToInitialize
        DisplayMessage(String.Format(Resource.getValue("IViewRepository.DisplayUnableToInitialize.ItemAction." & action.ToString), GetFilenameRender(name, extension, type)), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnableToExecute(action As ItemAction) Implements IViewRepository.DisplayUnableToExecute
        DisplayMessage(Resource.getValue("IViewRepository.DisplayUnableToExecute.action." & action.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplaySameDirectory() Implements IViewRepository.DisplaySameDirectory
        DisplayMessage(Resource.getValue("IViewRepository.DisplaySameDirectory"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
   
    Private Sub DisplayUnavailableSpace(name As String, extension As String, type As ItemType, size As Long, folder As dtoFolderTreeItem) Implements IViewRepository.DisplayUnavailableSpace
        Dim key As String = "IViewRepository.DisplayUnavailableSpace"
        Dim message As String = ""
        If Not IsNothing(folder.Quota) Then
            key &= ".OverflowAction." & folder.Quota.Overflow.ToString
            message = String.Format(Resource.getValue(key), "{0}", folder.GetPossibleOverSize(size), folder.GetMaxSize())
        Else
            message = Resource.getValue(key)
        End If
        DisplayMessage(String.Format(message, GetFilenameRender(name, extension, type)), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnableToAddFolders(folderName As String, cFolders As Long) Implements IViewRepository.DisplayUnableToAddFolders
        DisplayMessage(String.Format(Resource.getValue("IViewRepository.DisplayUnableToAddFolders"), cfolders), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayAddedFolders(folderName As String, added As Long, notAdded As Long) Implements IViewRepository.DisplayAddedFolders
        Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        If added > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue("IViewRepository.DisplayAddedFolders"), added), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If notAdded > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue("IViewRepository.DisplayUnableToAddFolders"), notAdded), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If items.Any() Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(items)
        End If
    End Sub
    Private Sub DisplayUnableToAddLinks(folderName As String, cLinks As Long) Implements IViewRepository.DisplayUnableToAddLinks
        DisplayMessage(String.Format(Resource.getValue("IViewRepository.DisplayUnableToAddLinks"), cLinks), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayAddedLinks(folderName As String, added As Long, notAdded As Long) Implements IViewRepository.DisplayAddedLinks
        Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        If added > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue("IViewRepository.DisplayAddedLinks"), added), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If notAdded > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue("IViewRepository.DisplayUnableToAddLinks"), notAdded), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If items.Any() Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(items)
        End If
    End Sub
    Private Sub DisplayAddedFiles(folderName As String, added As Long, notAdded As Long) Implements IViewRepository.DisplayAddedFiles
        Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        If added > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue("IViewRepository.DisplayAddedFiles"), added), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If notAdded > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue("IViewRepository.DisplayUnableToAddFiles"), notAdded), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If items.Any() Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(items)
        End If
        If items.Any() Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(items)
        End If
    End Sub
    Private Sub DisplayUnableToAddFiles(folderName As String, cFiles As Long) Implements IViewRepository.DisplayUnableToAddFiles
        DisplayMessage(String.Format(Resource.getValue("IViewRepository.DisplayUnableToAddFiles"), cFiles), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayDeletedItems(items As List(Of dtoItemToDelete)) Implements IViewRepository.DisplayDeletedItems
        Dim item As dtoItemToDelete = Nothing
        Dim message As String = ""
        Dim key As String = "IViewRepository.DisplayDeletedItems"
        Dim messages As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        If items.Any(Function(i) i.IsDeleted) Then
            messages.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = Resource.getValue(key & ".Deleted"), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If items.Any(Function(i) Not i.IsDeleted) Then
            messages.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue(key & ".NotDeleted"), items.Where(Function(i) Not i.IsDeleted AndAlso Not Not i.IsAddedForCascade).Count), .Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success})
        End If
        If messages.Any() Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(messages)
        End If
    End Sub
    Private Sub DisplayDeletedItem(item As dtoItemToDelete) Implements IViewRepository.DisplayDeletedItem
        DisplayDeletedItems(New List(Of dtoItemToDelete) From {item})
    End Sub
    Private Sub DisplayAddedVersion(folderName As String, addedVersion As dtoCreatedItem) Implements IViewRepository.DisplayAddedVersion
        Dim key As String = "IViewRepository.DisplayAddedVersion." & addedVersion.IsAdded
        If Not addedVersion.IsAdded Then
            key &= ".UnableToAddFile.ItemUploadError." & addedVersion.Error.ToString
        End If
        Dim message As String = Resource.getValue(key)
        If Not String.IsNullOrWhiteSpace(message) Then
            message = Replace(message, "#renderFile#", GetFilenameRender(addedVersion.ToAdd.DisplayName, addedVersion.ToAdd.Extension, addedVersion.ToAdd.Type))
            CTRLmessages.InitializeControl(message, IIf(addedVersion.IsAdded, lm.Comol.Core.DomainModel.Helpers.MessageType.success, lm.Comol.Core.DomainModel.Helpers.MessageType.error))
        End If
    End Sub
#End Region

#Region "Initializer"
    Private Sub InitializeHeaderSettings(isGeneric As Boolean, currentSet As PresetType, availableOptions As Dictionary(Of PresetType, List(Of ViewOption)), activeOptions As Dictionary(Of PresetType, List(Of ViewOption)), Optional idCommunity As Integer = -1, Optional idPerson As Integer = -1) Implements IViewRepository.InitializeHeaderSettings
        CTRLheader.InitializeHeader(isGeneric, currentSet, availableOptions, activeOptions, idCommunity, idPerson, RepositoryType.ToString)
    End Sub
    Private Sub InitializePresets(availableOptions As List(Of ViewOption), currentSet As PresetType, Optional presets As List(Of PresetType) = Nothing) Implements IViewRepository.InitializePresets
        CTRLviewOptions.InitializeControl(availableOptions, currentSet, presets)
    End Sub
    Private Function GetFolderTypeTranslation() As Dictionary(Of FolderType, String) Implements IViewRepository.GetFolderTypeTranslation
        Return (From e As FolderType In [Enum].GetValues(GetType(FolderType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.FolderType." & e.ToString))
    End Function
    Private Function GetTypesTranslations() As Dictionary(Of ItemType, String) Implements IViewRepository.GetTypesTranslations
        Return (From e As ItemType In [Enum].GetValues(GetType(ItemType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.ItemType." & e.ToString))
    End Function
    Private Function GetUnknownUserName() As String Implements IViewRepository.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Sub InitializeTree(curentFolder As dtoDisplayRepositoryItem, items As List(Of dtoDisplayRepositoryItem), identifier As RepositoryIdentifier, repositoryIdentifier As String) Implements IViewRepository.InitializeTree
        CTRLtree.InitializeControl(curentFolder, items, CurrentOrderBy, CurrentAscending, identifier, repositoryIdentifier)
    End Sub
    Private Sub InitializeBreadCrumb(folders As List(Of dtoFolderItem), rootUrl As String, order As OrderBy, ascending As Boolean) Implements IViewRepository.InitializeBreadCrumb
        DVbreadcrumb.Visible = True
        CTRLbreadcrumb.InitializeControl(folders, rootUrl, order, ascending)
    End Sub
    Private Sub HideBreadCrumb() Implements IViewRepository.HideBreadCrumb
        DVbreadcrumb.Visible = False
    End Sub
    Private Sub LoadDiskStatistics(items As List(Of dtoFolderSize)) Implements IViewRepository.LoadDiskStatistics
        DVrepositorySize.Visible = Not IsNothing(items) AndAlso items.Any()
        CTRLrepositorySize.InitializeControl(items)
    End Sub
    Private Sub DisplayFolderInfo(folderInfo As dtoFolderSize) Implements IViewRepository.DisplayFolderInfo
        If Not IsNothing(folderInfo) Then
            Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Dim isMainFolder As Boolean = (folderInfo.IdFolder = 0 AndAlso folderInfo.FolderType = FolderType.none)
            Dim key As String = "DisplayFolderInfo." & IIf(isMainFolder, "MainFolder", "Folder") & ".OverflowAction." & folderInfo.Quota.Overflow.ToString()
            If folderInfo.Quota.AllowOverrideQuota Then
                key &= ".AllowOverrideQuota"
            End If
            Dim message As String = Resource.getValue(key)

            If Not String.IsNullOrWhiteSpace(message) Then
                Select Case folderInfo.Quota.Overflow
                    Case OverflowAction.Allow, OverflowAction.AllowWithWarning
                        If folderInfo.Quota.Overflow = OverflowAction.Allow Then
                            mType = lm.Comol.Core.DomainModel.Helpers.MessageType.info
                        End If
                        If isMainFolder Then
                            message = String.Format(message, folderInfo.Quota.GetAvailableSize, folderInfo.GetOverSize, folderInfo.Quota.GetMaxAvailableSize)
                        Else
                            message = String.Format(message, folderInfo.Name, folderInfo.Quota.GetAvailableSize, folderInfo.GetOverSize, folderInfo.Quota.GetMaxAvailableSize)
                        End If
                    Case Else
                        mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
                            If isMainFolder Then
                                message = String.Format(message, folderInfo.GetOverSize, folderInfo.Quota.GetAvailableSize)
                            Else
                                message = String.Format(message, folderInfo.Name, folderInfo.GetOverSize, folderInfo.Quota.GetAvailableSize)
                            End If
                End Select
            End If
            CTRLfolderInfo.Visible = Not String.IsNullOrWhiteSpace(message)
            CTRLfolderInfo.InitializeControl(message, mType)
        Else
            HideFolderInfo()
        End If
    End Sub
    Private Sub HideFolderInfo() Implements IViewRepository.HideFolderInfo
        CTRLfolderInfo.Visible = False
    End Sub
    Private Sub InitializeAvailableOrderBy(items As List(Of OrderBy), selected As OrderBy) Implements IViewRepository.InitializeAvailableOrderBy
        CTRLorderBy.Visible = True
        CTRLorderBy.InitializeControl(items, selected)
    End Sub
    Private Sub HideOrderBySelector() Implements IViewRepository.HideOrderBySelector
        CTRLorderBy.Visible = False
    End Sub
    Private Sub UpdateBreadCrumbUrls(orderBy As OrderBy, ascending As Boolean) Implements IViewRepository.UpdateBreadCrumbUrls
        CTRLbreadcrumb.UpdateItemsUrl(orderBy, ascending)
    End Sub
    Private Sub UpdateTreeUrls(orderBy As OrderBy, ascending As Boolean) Implements IViewRepository.UpdateTreeUrls
        CTRLtree.CurrentOrderBy = orderBy
        CTRLtree.CurrentAscending = ascending
        'CTRLfolders.UpdateItemsUrl(orderBy, ascending)
    End Sub
    Private Sub DisplayMoveItemSelector(idItem As Long, idFolder As Long, folderName As String, folders As List(Of dtoNodeFolderItem), name As String, extension As String, type As ItemType) Implements IViewRepository.DisplayMoveItemSelector
        If (String.IsNullOrWhiteSpace(folderName) AndAlso idFolder = 0) Then
            folderName = Resource.getValue("RootFolder")
        End If
        Master.SetOpenDialogOnPostbackByCssClass(CTRLmoveDialog.DialogIdentifier)
        If folders.Any(Function(f) f.Id = 0) Then
            folders.Where(Function(f) f.Id = 0).FirstOrDefault().Name = Resource.getValue("RootFolder")
        End If
        CTRLmoveDialog.Visible = True
        CTRLmoveDialog.InitializeControl(idItem, idFolder, folderName, CurrentPath, folders, GetFilenameRender(name, extension, type))
    End Sub
    Private Sub DisplayAddVersion(item As dtoDisplayRepositoryItem, quota As dtoContainerQuota) Implements IViewRepository.DisplayAddVersion
        Master.SetOpenDialogOnPostbackByCssClass(CTRLaddVersion.DialogIdentifier)
        CTRLaddVersion.Visible = True
        CTRLaddVersion.InitializeControl(item, quota)
    End Sub
    Private Sub SetFoldersCookies(folders As List(Of Long)) Implements IViewRepository.SetFoldersCookies
        For Each idFolder As Long In folders.Where(Function(f) f <> 0)
            Dim oCookie As HttpCookie = Request.Cookies(String.Format(LTcookieTemplate.Text, idFolder.ToString & "-" & RepositoryIdentifier))
            If IsNothing(oCookie) Then
                oCookie = New HttpCookie(String.Format(LTcookieTemplate.Text, idFolder.ToString & "-" & RepositoryIdentifier))
                oCookie.Value = Boolean.FalseString.ToLower()
                Response.Cookies.Add(oCookie)
            Else
                oCookie.Value = Boolean.FalseString.ToLower()
            End If
        Next
    End Sub
#End Region

#Region "Community News"
    Private Function CleanBackUrl(backUrl As String) As String
        backUrl = Replace(backUrl, ApplicationUrlBase(True), "")
        backUrl = Replace(backUrl, ApplicationUrlBase(False), "")
        Return backUrl
    End Function
    Private Function SanitizeFolderUrl(folderUrl As String) As String
        If Not folderUrl.StartsWith(ApplicationUrlBase(True)) AndAlso Not folderUrl.StartsWith(ApplicationUrlBase(False)) Then
            folderUrl = ApplicationUrlBase & folderUrl
        End If
        Return folderUrl
    End Function
    Private Sub NotifyAddedItems(idFolder As Long, folderName As String, folderUrl As String, items As List(Of RepositoryItem)) Implements IViewRepository.NotifyAddedItems
        Dim oSender As New RepositoryCommunityNewsUtility(CurrentPresenter.CurrentIdModule, PageUtility)
        Dim backUrl As String = CleanBackUrl(folderUrl)
        folderUrl = SanitizeFolderUrl(folderUrl)
        
        For Each item As RepositoryItem In items
            Dim permissions As Integer = IIf(item.IsVisible, oSender.PermissionToSee, oSender.PermissionToAdmin)
            oSender.ItemAdded(item.Repository.IdCommunity, item.Id, item.IdVersion, item.Type, item.DisplayName, folderName, GetItemDownloadOrPlayUrl(item, True, Server.HtmlEncode(backUrl)), folderUrl, permissions)
        Next
    End Sub
    Private Sub NotifyAddedVersion(idFolder As Long, folderName As String, folderUrl As String, addedVersion As dtoCreatedItem) Implements IViewRepository.NotifyAddedVersion
        Dim oSender As New RepositoryCommunityNewsUtility(CurrentPresenter.CurrentIdModule, PageUtility)
        Dim backUrl As String = CleanBackUrl(folderUrl)
        folderUrl = SanitizeFolderUrl(folderUrl)
        Dim permissions As Integer = IIf(addedVersion.Added.IsVisible, oSender.PermissionToSee, oSender.PermissionToAdmin)
        oSender.ItemAdded(addedVersion.Added.Repository.IdCommunity, addedVersion.Added.Id, addedVersion.Added.IdVersion, addedVersion.Added.Type, addedVersion.Added.DisplayName, folderName, GetItemDownloadOrPlayUrl(addedVersion.Added, True, Server.HtmlEncode(backUrl)), folderUrl, permissions)
    End Sub

    Private Sub NotifyVisibilityChanged(idFolder As Long, folderName As String, folderUrl As String, item As liteRepositoryItem) Implements IViewRepository.NotifyVisibilityChanged
        NotifyVisibilityChanged(idFolder, folderName, folderUrl, New List(Of liteRepositoryItem) From {item})
    End Sub
    Private Sub NotifyVisibilityChanged(idFolder As Long, folderName As String, folderUrl As String, items As List(Of liteRepositoryItem)) Implements IViewRepository.NotifyVisibilityChanged
        Dim oSender As New RepositoryCommunityNewsUtility(CurrentPresenter.CurrentIdModule, PageUtility)
        Dim backUrl As String = CleanBackUrl(folderUrl)
        folderUrl = SanitizeFolderUrl(folderUrl)

        For Each item As liteRepositoryItem In items
            oSender.ItemEditVisibility(item.IsVisible, item.Repository.IdCommunity, item.Id, item.IdVersion, item.Type, item.DisplayName, folderName, GetItemDownloadOrPlayUrl(item, False), folderUrl)
        Next
    End Sub
    Private Sub NotifyDelete(folderUrl As String, item As dtoItemToDelete) Implements IViewRepository.NotifyDelete
        NotifyDelete(folderUrl, New List(Of dtoItemToDelete) From {item})
    End Sub

    Private Sub NotifyDelete(folderUrl As String, items As List(Of dtoItemToDelete)) Implements IViewRepository.NotifyDelete
        Dim oSender As New RepositoryCommunityNewsUtility(CurrentPresenter.CurrentIdModule, PageUtility)
        Dim backUrl As String = CleanBackUrl(folderUrl)
        folderUrl = SanitizeFolderUrl(folderUrl)

        For Each item As dtoItemToDelete In items
            oSender.ItemDeleted(item.Repository.IdCommunity, item.Id, item.Type, item.DisplayName, GetFolderTypeTranslation(FolderType.recycleBin), folderUrl)
        Next
    End Sub

    Private Sub NotifyVirtualDelete(idFolder As Long, folderName As String, folderUrl As String, item As liteRepositoryItem, isDeleted As Boolean) Implements IViewRepository.NotifyVirtualDelete
        NotifyVirtualDelete(idFolder, folderName, folderUrl, New List(Of liteRepositoryItem) From {item}, isDeleted)
    End Sub

    Private Sub NotifyVirtualDelete(idFolder As Long, folderName As String, folderUrl As String, items As List(Of liteRepositoryItem), isDeleted As Boolean) Implements IViewRepository.NotifyVirtualDelete
        Dim oSender As New RepositoryCommunityNewsUtility(CurrentPresenter.CurrentIdModule, PageUtility)
        Dim backUrl As String = CleanBackUrl(folderUrl)
        folderUrl = SanitizeFolderUrl(folderUrl)

        For Each item As liteRepositoryItem In items
            oSender.ItemVirtualDeleted(item.IsVisible, item.Repository.IdCommunity, item.Id, item.Type, item.DisplayName, folderName, GetItemDownloadOrPlayUrl(item, False), folderUrl, isDeleted)
        Next
    End Sub

#End Region

    Private Sub SetTitle(type As lm.Comol.Core.FileRepository.Domain.RepositoryType, Optional name As String = "", Optional isCurrent As Boolean = False, Optional settingsFound As Boolean = False) Implements IViewRepository.SetTitle
        Dim key As String = "Repository.ServiceTitle.RepositoryType." & type.ToString
        Dim title As String = ""
        Dim tooltip As String = ""
        Dim nopermissions As String = ""
        If isCurrent Then
            title = Resource.getValue(key)
        End If

        If Not String.IsNullOrWhiteSpace(name) Then
            key &= ".Name"
            tooltip = String.Format(Resource.getValue(key), name)
            If Not isCurrent Then
                title = tooltip
            End If
            nopermissions = String.Format(Resource.getValue("Repository.ServiceTitle.NoPermission.RepositoryType." & type.ToString & ".Name"), name)
        Else
            tooltip = title
            nopermissions = Resource.getValue("Repository.ServiceTitle.NoPermission.RepositoryType." & type.ToString)
        End If
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = tooltip
        Master.ServiceNopermission = nopermissions
    End Sub

    Private Sub LoadItems(items As List(Of dtoDisplayRepositoryItem), hideReorderColumn As Boolean, displayPath As Boolean, availableColumns As List(Of lm.Comol.Core.BaseModules.FileRepository.Domain.Column)) Implements IViewRepository.LoadItems
        ReorderColumnHidden = hideReorderColumn
        Columns = availableColumns
        _displayPath = displayPath
        THselect.Visible = availableColumns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.selectitem)
        THactions.Visible = availableColumns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.actions)
        THdisplayOrder.Visible = availableColumns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.displayorder)
        THstatistics.Visible = availableColumns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.stats)
        THdate.Visible = availableColumns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.date)
        THindicators.Visible = availableColumns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.indicators)
        RPTitems.DataSource = items
        RPTitems.DataBind()
    End Sub
    Private Sub InitializeFolderCommands(actions As List(Of ItemAction), quota As dtoContainerQuota, idFather As Long, fatherPath As String, fatherType As FolderType, folderName As String, Optional fatherUrl As String = "", Optional fatherName As String = "", Optional folders As List(Of dtoNodeFolderItem) = Nothing, Optional types As List(Of ItemType) = Nothing) Implements IViewRepository.InitializeFolderCommands
        CTRLcommands.Visible = actions.Any
        If actions.Any Then
            CTRLcommands.InitializeControl(actions, quota, idFather, fatherPath, fatherType, folderName, fatherUrl, fatherName, CurrentOrderBy, CurrentAscending)
            If actions.Contains(ItemAction.addfolder) Then
                CTRLaddFolder.Visible = True
                CTRLaddFolder.InitializeControl(IdCurrentFolder, folderName, CurrentPath, folders)
            Else
                CTRLaddFolder.Visible = False
            End If
            If actions.Contains(ItemAction.addlink) Then
                CTRLaddLinks.Visible = True
                CTRLaddLinks.InitializeControl(IdCurrentFolder, folderName, CurrentPath, folders)
            Else
                CTRLaddLinks.Visible = False
            End If
            If actions.Contains(ItemAction.upload) Then
                CTRLaddFiles.Visible = True
                CTRLaddFiles.InitializeControl(RepositoryType, RepositoryIdCommunity, IdCurrentFolder, folderName, CurrentPath, quota, folders, types)
            Else
                CTRLaddFiles.Visible = False
            End If
        End If
    End Sub
    Private Function GetRootFolderFullname() As String Implements IViewRepository.GetRootFolderFullname
        Return Resource.getValue("RootFolder")
    End Function
    Private Function GetRootFolderName() As String Implements IViewRepository.GetRootFolderName
        Return Resource.getValue("RootFolderName")
    End Function
    Private Function GetCurrentUrl() As String Implements IViewRepository.GetCurrentUrl
        Dim url As String = Replace(Request.Url.AbsolutePath, PageUtility.ApplicationUrlBase(), "")
        url = Replace(url, PageUtility.ApplicationUrlBase(True), "")
        Return url
    End Function
    Private Sub UpdateFolderCommandsUrls(orderBy As OrderBy, ascending As Boolean) Implements IViewRepository.UpdateFolderCommandsUrls
        CTRLcommands.UpdateItemsUrl(orderBy, ascending)
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewRepository.GoToUrl
        If String.IsNullOrEmpty(url) Then
            PageUtility.RedirectToUrl(GetCurrentUrl)
        Else
            PageUtility.RedirectToUrl(url)
        End If
    End Sub
    Private Function GetSelectedItems() As List(Of Long) Implements IViewRepository.GetSelectedItems
        Dim items As New List(Of Long)
        For Each row As RepeaterItem In RPTitems.Items
            Dim idItem As Long = 0
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBselect")
            Long.TryParse(oCheck.Value, idItem)
            If idItem > 0 AndAlso oCheck.Checked Then
                items.Add(idItem)
            End If
        Next
        Return items
    End Function
#End Region

#Region "Internal"
    Private Sub RepositoryList_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub

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
    Private Sub DisplayMessage(ByVal message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(message, type)
    End Sub
    Private Function GetTranslatedItems(executed As Boolean, executedItems As Dictionary(Of ItemType, Long), startItems As Dictionary(Of ItemType, Long)) As String
        Dim translations As New List(Of String)
        Dim key As String = ""
        Dim translation As String = ""
        For Each item As KeyValuePair(Of ItemType, Long) In startItems
            key = "MultipleActions.ItemType." & item.Key.ToString
            Select Case item.Value
                Case 1, 0
                    key &= ".1"
                Case Else
                    key &= ".n"
            End Select
            Dim items As Long = 0
            If executedItems.ContainsKey(item.Key) Then
                Select Case executedItems(item.Key)
                    Case 1
                        key &= ".1"
                    Case Else
                        key &= ".n"
                End Select
                items = executedItems(item.Key)
            Else
                key &= ".n"
            End If
            If Not executed Then
                items = (item.Value - items)
            End If
            If items < 0 Then
                items = 0
            End If
            translation = Replace(Resource.getValue(key), "#selecteditems#", item.Value)
            translation = Replace(translation, "#items#", items)
            translations.Add(translation)
        Next
        If translations.Any() Then
            Return String.Join(", ", translations)
        Else
            Return ""
        End If
    End Function
#Region "Controls"
    Private Sub CTRLorderBy_ChangeOrderBy(selected As OrderBy, ascending As Boolean) Handles CTRLorderBy.ChangeOrderBy
        CTRLmessages.Visible = False
        UpdateBreadCrumbUrls(selected, ascending)
        UpdateTreeUrls(selected, ascending)
        UpdateFolderCommandsUrls(selected, ascending)
        CurrentPresenter.ReorderItems(IdCurrentFolder, CurrentFolderType, CurrentFolderIdentifierPath, selected, ascending, RepositoryType, RepositoryIdCommunity)
    End Sub
    Protected Sub CTRLmenu_MenuItemCommand(ByVal idItem As Long, ByVal action As lm.Comol.Core.FileRepository.Domain.ItemAction)
        If isValidOperation() Then
            CurrentPresenter.ExecuteAction(idItem, action, RepositoryType, RepositoryIdCommunity)
        End If
    End Sub
    Protected Sub CTRL_ItemSelectedFolder(idFolder As Long, path As String, type As FolderType)
        If isValidOperation() Then
            CurrentSelectedItems = New List(Of Long)
            CTRLtree.UpdateSelectedFolder(idFolder, path, type)
            CurrentPresenter.SelectFolder(idFolder, path, type, CurrentOrderBy, CurrentAscending, RepositoryType, RepositoryIdCommunity)
        End If
    End Sub
#Region "Move items"
    Private Sub CTRLmoveDialog_MoveToFolder(idItem As Long, idFolder As Long) Handles CTRLmoveDialog.MoveToFolder
        Master.ClearOpenedDialogOnPostback()
        If isValidOperation() Then
            CurrentPresenter.MoveToFolder(idItem, idFolder, RepositoryType, RepositoryIdCommunity)
        End If

    End Sub
    Private Sub CTRLmoveDialog_MoveToFolder(idItems As List(Of Long), idFolder As Long) Handles CTRLmoveDialog.MoveToFolders
        Master.SetOpenDialogOnPostbackByCssClass("")
    End Sub
#End Region

#Region "BreadCrumb actions"
    Private Sub CTRLbreadcrumb_SelectedFolder(idFolder As Long, path As String, folderType As FolderType) Handles CTRLbreadcrumb.SelectedFolder
        CTRLmessages.Visible = False
        CurrentSelectedItems = New List(Of Long)
        CTRLtree.UpdateSelectedFolder(idFolder, path, folderType)
        CurrentPresenter.SelectFolder(idFolder, path, folderType, CurrentOrderBy, CurrentAscending, RepositoryType, RepositoryIdCommunity)
    End Sub
#End Region

#Region "Command actions"
    Private Sub CTRLcommands_ItemCommand(action As ItemAction) Handles CTRLcommands.ItemCommand
        CTRLmessages.Visible = False
        If isValidOperation() Then
            Dim idItems As List(Of Long) = GetSelectedItems()
            CurrentSelectedItems = idItems
            If idItems.Any() Then
                CurrentPresenter.ExecuteAction(idItems, action, RepositoryType, RepositoryIdCommunity)
            End If
        End If
    End Sub
    Private Sub CTRLcommands_ItemLoadFolder(idFolder As Long, path As String, folderType As FolderType) Handles CTRLcommands.ItemLoadFolder
        CurrentSelectedItems = New List(Of Long)
        CTRLtree.UpdateSelectedFolder(idFolder, path, folderType)
        CurrentPresenter.SelectFolder(idFolder, path, folderType, CurrentOrderBy, CurrentAscending, RepositoryType, RepositoryIdCommunity)
    End Sub
#End Region

#Region "Tree actions"
    Private Sub CTRLtree_SelectedFolder(idFolder As Long, path As String, folderType As FolderType) Handles CTRLtree.SelectedFolder
        CTRLmessages.Visible = False
        CurrentSelectedItems = New List(Of Long)
        CurrentPresenter.SelectFolder(idFolder, path, folderType, CurrentOrderBy, CurrentAscending, RepositoryType, RepositoryIdCommunity)
    End Sub
#End Region

#Region "Add item"
    Private Sub CTRLaddFolder_AddFolders(idFather As Long, items As List(Of dtoFolderName)) Handles CTRLaddFolder.AddFolders
        If isValidOperation() Then
            CurrentPresenter.AddFolders(idFather, items, RepositoryType, RepositoryIdCommunity)
        End If
    End Sub
    Private Sub CTRLaddLinks_AddLinks(idFather As Long, items As List(Of dtoUrlItem)) Handles CTRLaddLinks.AddLinks
        If isValidOperation() Then
            CurrentPresenter.AddLinks(idFather, items, RepositoryType, RepositoryIdCommunity)
        End If
    End Sub
    Private Sub CTRLaddFiles_AddFiles(idFather As Long, items As List(Of dtoUploadedItem)) Handles CTRLaddFiles.AddFiles
        If isValidOperation() Then
            CurrentPresenter.AddFiles(SystemSettings.NotificationErrorService.ComolUniqueID, idFather, items, RepositoryType, RepositoryIdCommunity)
        End If
        Master.ClearOpenedDialogOnPostback()
    End Sub
#End Region


    Private Sub CTRLaddVersion_AddVersion(idItem As Long, file As dtoUploadedItem) Handles CTRLaddVersion.AddVersion
        CTRLmessages.Visible = False
        If isValidOperation() Then
            CurrentPresenter.AddVersionToFile(SystemSettings.NotificationErrorService.ComolUniqueID, idItem, file, RepositoryType, RepositoryIdCommunity)
        End If
        Master.ClearOpenedDialogOnPostback()
    End Sub

    Private Sub DeleteTemporaryItems(items As List(Of dtoUploadedItem))
        For Each item As dtoUploadedItem In items
            lm.Comol.Core.File.Delete.File(item.SavedFullPath)
        Next
    End Sub
#End Region
    Private Sub RPTitems_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTitems.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoDisplayRepositoryItem = e.Item.DataItem
                Dim oRow As HtmlTableRow = e.Item.FindControl("TRrowItem")
                oRow.Attributes("class") = oRow.Attributes("class") & ItemCssClass(dto)
                If dto.Availability <> ItemAvailability.available Then
                    oRow.Attributes("title") = ItemAvailabilityTooltip(dto)
                End If
                Dim oCell As HtmlTableCell = e.Item.FindControl("TDdisplayOrder")
                oCell.Visible = Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.displayorder)

                oCell = e.Item.FindControl("TDselect")
                oCell.Visible = Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.selectitem)

                Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBselect")
                If Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.selectitem) AndAlso dto.Permissions.AllowSelection Then
                    oCheck.Value = dto.Id
                    oCheck.Checked = CurrentSelectedItems.Contains(dto.Id)
                    oCheck.Disabled = False
                Else
                    oCheck.Value = 0
                    oCheck.Disabled = True
                End If

                Dim oLabel As Label = Nothing
                Dim oControl As UC_RenderItem = e.Item.FindControl("CTRLitem")
                oControl.InitializeControl(dto, _displayPath, RepositoryType, RepositoryIdCommunity, CurrentOrderBy, CurrentAscending)
                oCell = e.Item.FindControl("TDindicators")
                If Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.indicators) Then
                    oCell.Visible = True
                    oLabel = e.Item.FindControl("LBitemVersions")
                    If dto.HasVersions Then
                        oLabel.Visible = True
                        oLabel.ToolTip = Resource.getValue("LBitemVersions.ToolTip")
                    Else
                        oLabel.Visible = False
                    End If
                    oLabel = e.Item.FindControl("LBitemOtherPermissions")
                    If Not dto.HasDefaultPermissions Then
                        oLabel.Visible = True
                        oLabel.ToolTip = Resource.getValue("LBitemOtherPermissions.ToolTip")
                    Else
                        oLabel.Visible = False
                    End If
                Else
                    oCell.Visible = False
                End If

                Dim oLiteral As Literal = Nothing
                oCell = e.Item.FindControl("TDdate")
                If Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.date) Then
                    oLiteral = e.Item.FindControl("LTdate")
                    If (dto.Type = ItemType.SharedDocument OrElse (dto.HasVersions AndAlso dto.ModifiedOn.HasValue)) Then
                        oLiteral.Text = GetDateTimeString(dto.ModifiedOn, "")
                    Else
                        oLiteral.Text = GetDateTimeString(dto.CreatedOn, "")
                    End If
                End If
                oCell.Visible = Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.date)

                oCell = e.Item.FindControl("TDstatistics")
                If Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.stats) Then
                    oCell.Visible = True
                    oLiteral = e.Item.FindControl("LTstatistics")
                    If dto.Permissions.ViewOtherStatistics Then
                        oLiteral.Text = String.Format(LTtemplateStats.Text, LTstatsCssClass.Text, dto.Downloaded, Resource.getValue("OtherDownloadStatistics"))
                    End If
                    If dto.Permissions.ViewMyStatistics Then
                        oLiteral.Text &= String.Format(LTtemplateStats.Text, LTmyStatsCssClass.Text, dto.MyDownloads, Resource.getValue("MyDownloadStatistics"))
                    End If
                Else
                    oCell.Visible = False
                End If
                oCell = e.Item.FindControl("TDactions")
                If Columns.Contains(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.actions) AndAlso (dto.FolderType = FolderType.standard OrElse dto.IsFile) Then
                    oCell.Visible = True
                    Dim oHyperlink As HyperLink = e.Item.FindControl("HYPdetails")
                    oHyperlink.ToolTip = Resource.getValue("HYPdetails.ToolTip")
                    oHyperlink.NavigateUrl = ApplicationUrlBase & RootObject.Details(dto.Id, dto.IdFolder, dto.IdentifierPath, ItemAction.details, True)
                Else
                    oCell.Visible = False
                End If
            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTitems.Items.Count = 0)
                If (RPTitems.Items.Count = 0) Then
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                    oLabel.Text = Resource.getValue("NoFileFolders")

                End If
        End Select
    End Sub
    Public Function ItemCssClass(item As dtoDisplayRepositoryItem) As String
        Dim cssClass As String = ""
        Select Case item.Type
            Case ItemType.File
                cssClass = cssClass
            Case Else
                cssClass &= " " & item.Type.ToString().ToLower
        End Select
        Select Case item.Status
            Case ItemStatus.Active
                cssClass = cssClass
            Case Else
                cssClass &= " " & item.Status.ToString().ToLower
        End Select
        Select Case item.Availability
            Case ItemAvailability.available
                cssClass = cssClass
            Case Else
                cssClass &= " " & item.Availability.ToString().ToLower
        End Select
        If Not item.IsVisible Then
            cssClass &= " " & LThiddenItemCssClass.Text
        End If
        Return cssClass
    End Function
    Public Function ItemAvailabilityTooltip(item As dtoDisplayRepositoryItem) As String
        Select Case item.Availability
            Case ItemAvailability.available
                Return ""
            Case Else
                Dim message As String = Resource.getValue("ItemAvailabilityTooltip.ItemAvailability." & item.Availability.ToString)
                If String.IsNullOrWhiteSpace(message) Then
                    Return ""
                ElseIf message.Contains("{0}") Then
                    Return String.Format(message, item.DisplayName)
                Else
                    Return message
                End If
        End Select
    End Function



    Public Function ItemDataCommands(item As dtoDisplayRepositoryItem) As String
        Dim commands As String = ""
        If item.Permissions.GetMultipleActions().Any Then
            commands = String.Join(",", item.Permissions.GetMultipleActions().Select(Function(i) CInt(i)).ToList())
        End If
        Return commands
    End Function

    Public Function CssClassNoReorder() As String
        If ReorderColumnHidden Then
            Return "noorder"
        End If
        Return ""
    End Function

#End Region

End Class