Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.ActionDataContract

Public Class UC_ModuleRepositoryUploader
    Inherits FRbaseControl
    Implements IViewModuleCommunityUploader

#Region "Context"
    Private _Presenter As ModuleCommunityUploaderPresenter
    Private ReadOnly Property CurrentPresenter() As ModuleCommunityUploaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleCommunityUploaderPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property DisplayErrorInline As Boolean Implements IViewModuleCommunityUploader.DisplayErrorInline
        Get
            Return ViewStateOrDefault("DisplayErrorInline", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayErrorInline") = value
        End Set
    End Property
    Public Property AllowUpload As Boolean Implements IViewModuleCommunityUploader.AllowUpload
        Get
            Return ViewStateOrDefault("AllowUpload", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowUpload") = value
        End Set
    End Property
    Public Property AllowUploadToFolder As Boolean Implements IViewModuleCommunityUploader.AllowUploadToFolder
        Get
            Return ViewStateOrDefault("AllowUploadToFolder", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowUploadToFolder") = value
        End Set
    End Property
    Public Property UsePublicUser As Boolean Implements IViewModuleCommunityUploader.UsePublicUser
        Get
            Return ViewStateOrDefault("UsePublicUser", False)
        End Get
        Set(value As Boolean)
            ViewState("UsePublicUser") = value
        End Set
    End Property
    Public Property UseAnonymousUser As Boolean Implements IViewModuleCommunityUploader.UseAnonymousUser
        Get
            Return ViewStateOrDefault("UseAnonymousUser", False)
        End Get
        Set(value As Boolean)
            ViewState("UseAnonymousUser") = value
        End Set
    End Property
    Public Property AllowAnonymousUpload As Boolean Implements IViewModuleUploader.AllowAnonymousUpload
        Get
            Return ViewStateOrDefault("AllowAnonymousUpload", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAnonymousUpload") = value
        End Set
    End Property
    Public Property MaxItems As Integer Implements IViewModuleUploader.MaxItems
        Get
            Return ViewStateOrDefault("MaxItems", 1)
        End Get
        Set(value As Integer)
            ViewState("MaxItems") = value
            RAUfiles.MultipleFileSelection = (value > 1)
        End Set
    End Property
    Public Property MaxFileInput As Integer Implements IViewModuleUploader.MaxFileInput
        Get
            Return RAUfiles.MaxFileInputsCount
        End Get
        Set(value As Integer)
            If value > 0 Then
                RAUfiles.MaxFileInputsCount = value
            Else
                RAUfiles.MaxFileInputsCount = 1
            End If
        End Set
    End Property
    Private Property RepositoryIdentifier As RepositoryIdentifier Implements IViewModuleUploader.RepositoryIdentifier
        Get
            Dim result As RepositoryIdentifier = Nothing
            If Not IsNothing(ViewState("RepositoryIdentifier")) Then
                Try
                    result = DirectCast(ViewState("RepositoryIdentifier"), RepositoryIdentifier)
                Catch ex As Exception

                End Try
            End If
            Return result
        End Get
        Set(value As RepositoryIdentifier)
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
    Private Property IdUploaderUser As Int32 Implements IViewModuleUploader.IdUploaderUser
        Get
            Return ViewStateOrDefault("IdUploaderUser", 0)
        End Get
        Set(value As Int32)
            ViewState("IdUploaderUser") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Property AjaxEnabled() As Boolean
        Get
            If TypeOf Me.ViewState("AjaxEnabled") Is Boolean Then
                Return Me.ViewState("AjaxEnabled")
            Else
                Me.ViewState("AjaxEnabled") = False
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AjaxEnabled") = value
        End Set
    End Property
    Public Event AllowUploadUpdate(ByVal allowUpload As Boolean)
    Public Event UploadingError(message As String, mType As lm.Comol.Core.DomainModel.Helpers.MessageType)
    Public Event IsValidOperation(ByRef isvalid As Boolean)
    Public WriteOnly Property PostbackTriggers As String
        Set(value As String)
            If Not String.IsNullOrWhiteSpace(value) Then
                If value.Contains(",") Then
                    RAUfiles.PostbackTriggers = value.Split(",").ToArray
                Else
                    RAUfiles.PostbackTriggers = New String() {value}
                End If
            End If
        End Set
    End Property
    Private _HasFoldersToSelect As Boolean
    Private _DisplayHideCommands As Boolean
    Public Property DisplayHideCommands() As Boolean
        Get
           Return _DisplayHideCommands
        End Get
        Set(ByVal value As Boolean)
            _DisplayHideCommands = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBaddFilesToCommunity_t)
            .setLabel(LBselectDestinationFolderPath)
            .setLabel(LBfilesItemType_t)
            .setLabel(LBhideFiles_t)
            .setLabel(LBhideFiles)
            .setLabel(LBselectAsyncFiles_t)
            .setLabel(LBitemTypeDescription)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idFolder As Long, repository As RepositoryIdentifier) Implements IViewModuleCommunityUploader.InitializeControl
        InitializeRepositoryPath(repository.Type, repository.IdCommunity)
        CurrentPresenter.InitView(idFolder, repository, AllowAnonymousUpload, UseAnonymousUser, UsePublicUser)
    End Sub
    Public Sub InitializeControlForCommunity(idFolder As Long, idCommunity As Integer) Implements IViewModuleCommunityUploader.InitializeControlForCommunity
        InitializeControl(idFolder, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(RepositoryType.Community, idCommunity))
    End Sub
    Public Sub InitializeControlForPortal(idFolder As Long) Implements IViewModuleCommunityUploader.InitializeControlForPortal
        InitializeControl(idFolder, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(RepositoryType.Portal, 0))
    End Sub

#Region "Display"
    Private Sub DisplayUploadQuotaInfo(idFolder As Long, quota As dtoContainerQuota) Implements IViewModuleCommunityUploader.DisplayUploadQuotaInfo
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Dim key As String = "DisplayUploadQuotaInfo"
        If quota.FreeSpace > 0 Then
            key &= ".FreeSpace"
        Else
            key &= ".MainFolder"
        End If

        If (quota.FreeSpace = 0 AndAlso quota.AllowOverrideQuota) OrElse (quota.MaxUploadFileSize > quota.FreeSpace AndAlso (quota.Overflow = OverflowAction.Allow OrElse quota.Overflow = OverflowAction.AllowWithWarning)) Then
            key &= ".AllowOverrideQuota"
        End If
        key &= ".OverflowAction." & quota.Overflow.ToString()
        Dim message As String = Resource.getValue(key)

        If Not String.IsNullOrWhiteSpace(message) Then
            If quota.FreeSpace > 0 AndAlso quota.FreeSpace > quota.MaxUploadFileSize Then
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.info
            End If

            message = Replace(message, "#freespace#", quota.GetFreeSpace)
            message = Replace(message, "#availablesize#", quota.GetAvailableSize)
            message = Replace(message, "#maxsize#", quota.GetMaxAvailableSize)
            message = Replace(message, "#uploadsize#", quota.GetFolderMaxUploadFileSize)
            message = Replace(message, "#oversize#", quota.GetOverSize)
        End If
        CTRLmessages.Visible = Not String.IsNullOrWhiteSpace(message)
        CTRLmessages.InitializeControl(message, mType)
    End Sub

    Private Sub DisplayUploadQuotaUnavailable() Implements IViewModuleCommunityUploader.DisplayUploadQuotaUnavailable
        InternalDisplayMessage(Resource.getValue("IViewModuleCommunityUploader.DisplayUploadQuotaUnavailable"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUploadUnavailable() Implements IViewModuleCommunityUploader.DisplayUploadUnavailable
        InternalDisplayMessage(Resource.getValue("IViewModuleCommunityUploader.DisplayUploadUnavailable"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayError(err As ItemUploadError, folderName As String, items As List(Of dtoUploadedItem)) Implements IViewModuleCommunityUploader.DisplayError
        Dim message As String = Resource.getValue("IViewModuleCommunityUploader.ItemUploadError." & err.ToString)
        If Not String.IsNullOrWhiteSpace(message) Then
            If message.Contains("{1}") Then
                message = String.Format(message, items.Count, GetFilenameRender(folderName, "", ItemType.Folder))
            ElseIf message.Contains("{0}") Then
                message = String.Format(message, GetFilenameRender(folderName, "", ItemType.Folder))
            End If
        End If
        InternalDisplayMessage(message, lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayError(err As ItemUploadError, Optional name As String = "", Optional extension As String = "", Optional type As ItemType = lm.Comol.Core.FileRepository.Domain.ItemType.None) Implements IViewModuleCommunityUploader.DisplayError
        Dim message As String = Resource.getValue("IViewModuleCommunityUploader.ItemUploadError." & err.ToString)
        If message.Contains("{0}") AndAlso Not String.IsNullOrWhiteSpace(message) Then
            message = String.Format(message, GetFilenameRender(name, extension, type))
        End If
        InternalDisplayMessage(message, lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType, idItem As Long, objType As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType) Implements IViewModuleCommunityUploader.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objType, idItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

#Region "Translations"
    Private Function GetRootFolderFullname() As String Implements IViewModuleCommunityUploader.GetRootFolderFullname
        Return Resource.getValue("RootFolder")
    End Function
    Private Function GetRootFolderName() As String Implements IViewModuleCommunityUploader.GetRootFolderName
        Return Resource.getValue("RootFolderName")
    End Function
    Private Function GetUnknownUserName() As String Implements IViewModuleCommunityUploader.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
#End Region


    Private Sub LoadFolderSelector(repository As RepositoryIdentifier, idFolder As Long, folderName As String, quota As dtoContainerQuota, folders As List(Of dtoNodeFolderItem)) Implements IViewModuleCommunityUploader.LoadFolderSelector

        CTRLfolderSelector.InitializeControl(repository.Type, repository.IdCommunity, idFolder, folderName, folders)
        _HasFoldersToSelect = (folders.Where(Function(f) f.Type = NodeType.Item).Count > 1)
        Dim tempPath As String = GetUploadDiskPath()
        If Not String.IsNullOrWhiteSpace(tempPath) Then
            RAUfiles.TemporaryFolder = tempPath
            RAUfiles.TemporaryFileExpiration = New TimeSpan(2, 0, 0)
        End If
        InitializeQuotaInfo(idFolder, quota)
    End Sub
    Private Sub InitializeQuotaInfo(idFolder As Long, quota As dtoContainerQuota) Implements IViewModuleCommunityUploader.InitializeQuotaInfo
        If Not IsNothing(quota) Then
            Dim allowUpload As Boolean = (quota.AllowOverrideQuota AndAlso quota.MaxAvailableSize > 0) OrElse quota.FreeSpace > 0 OrElse ((quota.Overflow = OverflowAction.Allow OrElse quota.Overflow = OverflowAction.AllowWithWarning) AndAlso quota.MaxAvailableSize > 0)

            DisplayUploadQuotaInfo(idFolder, quota)
            If allowUpload Then
                RAUfiles.MaxFileSize = quota.MaxUploadFileSize
            End If
            DVfolderSelector.Visible = allowUpload AndAlso _HasFoldersToSelect
            DVhidden.Visible = allowUpload AndAlso DisplayHideCommands
            DVitemsType.Visible = allowUpload
            DVuploader.Visible = allowUpload
            'BTNaddFiles.Visible = allowUpload
        Else
            CTRLmessages.Visible = False
        End If
    End Sub

    Private Sub LoadItemTypes(types As List(Of ItemType)) Implements IViewModuleUploader.LoadItemTypes
        If IsNothing(types) Then
            types = New List(Of ItemType)
        End If
        If Not types.Any() OrElse Not types.Any(Function(t) t = ItemType.File) Then
            types.Add(ItemType.File)
        End If
        RBLitemType.Items.Clear()
        Dim translations As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) = types.Select(Function(t) New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(t), .Translation = Resource.getValue("RBLitemType.ItemType." & t.ToString)}).ToList()
        For Each tItem As lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) In translations.OrderBy(Function(t) t.Translation).ToList()
            Dim oListItem As New ListItem() With {.Text = tItem.Translation, .Value = tItem.Id}
            oListItem.Attributes.Add("class", "item")

            RBLitemType.Items.Add(oListItem)
        Next
        RBLitemType.SelectedValue = CInt(ItemType.File)
    End Sub
    Private Function GetRepositoryDiskPath() As String Implements IViewModuleCommunityUploader.GetRepositoryDiskPath
        Return MyBase.GetRepositoryDiskPath()
    End Function

    Public Sub DisableControl() Implements IViewModuleUploader.DisableControl
        RAUfiles.Enabled = False
        RBLitemType.Enabled = False
        CBXhideItems.Enabled = False
    End Sub
    Public Function AddFilesToRepository(obj As Object, idObject As Long, idObjectType As Integer, moduleCode As String, idModuleAjaxAction As Integer, Optional idModuleAction As Integer = 0) As List(Of dtoModuleUploadedItem) Implements IViewModuleCommunityUploader.AddFilesToRepository
        CTRLerrorMessage.Visible = False
        Dim files As List(Of dtoUploadedItem) = GetFiles()
        ResetInputItems()
        Return CurrentPresenter.AddFiles(SystemSettings.NotificationErrorService.ComolUniqueID, IdUploaderUser, AllowAnonymousUpload, True, RepositoryIdentifier, CTRLfolderSelector.IdSelectedFolder, files, obj, idObject, idObjectType, moduleCode, idModuleAjaxAction, idModuleAction)
    End Function
    Private Sub NotifyAddedItems(idModule As Integer, idFolder As Long, folderName As String, folderUrl As String, items As List(Of liteRepositoryItem)) Implements IViewModuleCommunityUploader.NotifyAddedItems
        Dim oSender As New RepositoryCommunityNewsUtility(idModule, PageUtility)
        Dim backUrl As String = CleanBackUrl(folderUrl)
        folderUrl = SanitizeFolderUrl(folderUrl)

        For Each item As liteRepositoryItem In items
            Dim permissions As Integer = IIf(item.IsVisible, oSender.PermissionToSee, oSender.PermissionToAdmin)
            oSender.ItemAdded(item.Repository.IdCommunity, item.Id, item.IdVersion, item.Type, item.DisplayName, folderName, GetItemDownloadOrPlayUrl(item, True, Server.HtmlEncode(backUrl)), folderUrl, permissions)
        Next
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
#End Region

#Region "Internal"

    Private Sub InternalDisplayMessage(message As String, mType As lm.Comol.Core.DomainModel.Helpers.MessageType)
        If DisplayErrorInline Then
            CTRLerrorMessage.Visible = True
            CTRLerrorMessage.InitializeControl(message, mType)
        Else
            RaiseEvent UploadingError(message, mType)
        End If
    End Sub
    Private Function GetFiles() As List(Of dtoUploadedItem)
        Dim items As New List(Of dtoUploadedItem)
        If RAUfiles.UploadedFiles.Count > 0 Then
            items.AddRange(GetFiles(RAUfiles))
        End If
        Return items
    End Function
    Private Function GetFiles(uploader As Telerik.Web.UI.RadAsyncUpload) As List(Of dtoUploadedItem)
        Dim items As New List(Of dtoUploadedItem)
        For Each f As Telerik.Web.UI.UploadedFile In uploader.UploadedFiles
            Dim item As New dtoUploadedItem
            item.UniqueId = Guid.NewGuid
            item.IsVisible = True
            item.Type = CInt(RBLitemType.SelectedValue)
            item.Extension = f.GetExtension
            item.Name = EscapeInvalidCharacter(f.GetNameWithoutExtension)
            item.OriginalName = item.Name
            item.ContentType = f.ContentType
            item.Size = f.ContentLength
            item.SavedFileName = item.UniqueId.ToString & item.Extension
            item.ThumbnailPath = GetFinalThumbnailPath()

            item.SavedPath = GetFinalPath()
            Dim folderExist As Boolean = lm.Comol.Core.File.Exists.Directory(item.SavedPath)
            If Not folderExist Then
                folderExist = lm.Comol.Core.File.Create.Directory(item.SavedPath)
            End If
            If Not lm.Comol.Core.File.Exists.Directory(item.ThumbnailPath) Then
                lm.Comol.Core.File.Create.Directory(item.ThumbnailPath)
            End If
            If folderExist Then
                If lm.Comol.Core.File.Create.UploadFile(f, item.SavedPath & item.SavedFileName) Then
                    item.SavedFullPath = item.SavedPath & item.SavedFileName
                End If
            End If
            items.Add(item)
        Next
        Return items
    End Function
    Private Sub ResetInputItems()
        CTRLerrorMessage.Visible = False
        If RBLitemType.Items.Count > 0 Then
            RBLitemType.SelectedValue = CInt(ItemType.File)
        End If
        CBXhideItems.Checked = False
    End Sub
    Private Function EscapeInvalidCharacter(name As String) As String
        If name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) = -1 Then
            Return name
        Else
            'Escape invalid characters
            name = System.Text.RegularExpressions.Regex.Replace(name, "[^\w\.@-]", "", System.Text.RegularExpressions.RegexOptions.None)
            'If all characters are invalid return underscore as a file name
            If (System.IO.Path.GetFileNameWithoutExtension(name).Length = 0) Then
                Return name.Insert(0, "_")
            End If
            'Else return the escaped name
            Return name
        End If
    End Function
    Protected Friend Function GetFilenameRender(fullname As String, fileExtension As String, type As ItemType) As String
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
#End Region


End Class