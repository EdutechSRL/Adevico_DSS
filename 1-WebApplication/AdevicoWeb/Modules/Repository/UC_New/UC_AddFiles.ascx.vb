Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_AddFiles
    Inherits FRbaseControl

#Region "Internal"
    Private _itemsCount As Integer
    Public Property MaxItems As Integer
        Get
            Return ViewStateOrDefault("MaxItems", 1)
        End Get
        Set(value As Integer)
            ViewState("MaxItems") = value
            RAUfiles.MultipleFileSelection = (value > 1)
        End Set
    End Property

    Private Property RepositoryIdCommunity As Integer
        Get
            Return ViewStateOrDefault("IdRepositoryCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdRepositoryCommunity") = value
        End Set
    End Property
    Private Property RepositoryType As RepositoryType
        Get
            Return ViewStateOrDefault("RepositoryType", RepositoryType.Community)
        End Get
        Set(value As RepositoryType)
            ViewState("RepositoryType") = value
        End Set
    End Property
    Private Property IdFolderFather As Long
        Get
            Return ViewStateOrDefault("IdFolderFather", -1)
        End Get
        Set(value As Long)
            ViewState("IdFolderFather") = value
        End Set
    End Property
    Public ReadOnly Property AddFilesDialogTitle As String
        Get
            Return Resource.getValue("AddFilesDialogTitle")
        End Get
    End Property
    Public Event AddFiles(idFather As Long, ByVal items As List(Of dtoUploadedItem))
    Private _HasFoldersToSelect As Boolean
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPcloseAddFilesDialog, False, True)
            .setButton(BTNaddFiles, True)
            .setLabel(LBaddFilesToCommunity_t)
            .setLabel(LBcurrentPath_t)
            .setLabel(LBselectDestinationFolderPath)
            .setLabel(LBfilesItemType_t)
            .setLabel(LBhideFiles_t)
            .setLabel(LBhideFiles)
            .setLabel(LBselectAsyncFiles_t)
            .setLabel(LBitemTypeDescription)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(type As RepositoryType, idRepCommunity As Integer, idFolder As Long, folderName As String, folderPath As String, quota As dtoContainerQuota, folders As List(Of dtoNodeFolderItem), types As List(Of ItemType), Optional maxSize As Long = -1)
        InitializeControl(type, idRepCommunity, MaxItems, idFolder, folderName, folderPath, quota, folders, types, maxSize)
    End Sub
    Public Sub InitializeControl(type As RepositoryType, idRepCommunity As Integer, numberOfFolders As Integer, idFolder As Long, folderName As String, folderPath As String, quota As dtoContainerQuota, folders As List(Of dtoNodeFolderItem), types As List(Of ItemType), Optional maxSize As Long = -1)
        InitializeRepositoryPath(type, idRepCommunity)
        RepositoryIdCommunity = idRepCommunity
        RepositoryType = type
        IdFolderFather = idFolder
        If MaxItems < 1 Then
            MaxItems = 1
        End If
        _itemsCount = MaxItems
        RAUfiles.MultipleFileSelection = (_itemsCount > 1)
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

        DVitemsType.Visible = (types.Count > 1)
        Select Case types.Count
            Case 1
                LTaddFilesDescription.Text = Resource.getValue("LTaddFilesDescription.1.text")
            Case Else
                Resource.setLiteral(LTaddFilesDescription)
        End Select
        InternalInitializeControl(type, idRepCommunity, idFolder, folderName, folderPath, quota, folders)
    End Sub

    Private Sub InternalInitializeControl(type As RepositoryType, idRepCommunity As Integer, idFolder As Long, folderName As String, folderPath As String, quota As dtoContainerQuota, folders As List(Of dtoNodeFolderItem))
        If String.IsNullOrWhiteSpace(folderPath) Then
            DVcurrentPath.Visible = False
        Else
            DVcurrentPath.Visible = True
            LBcurrentPath.Text = folderPath
            LBcurrentPath.ToolTip = folderName
        End If
        CTRLfolderSelector.InitializeControl(type, idRepCommunity, idFolder, folderName, folders)
        _HasFoldersToSelect = Not IsNothing(folders) AndAlso (folders.Where(Function(f) f.Type = NodeType.Item).Count > 1)
        Dim tempPath As String = GetUploadDiskPath()
        If Not String.IsNullOrWhiteSpace(tempPath) Then
            RAUfiles.TemporaryFolder = tempPath
            RAUfiles.TemporaryFileExpiration = New TimeSpan(2, 0, 0)
        End If
        InitializeQuotaInfo(idFolder, quota)
    End Sub

    Private Sub InitializeQuotaInfo(idFolder As Long, quota As dtoContainerQuota)
        If Not IsNothing(quota) Then
            Dim allowUpload As Boolean = (quota.AllowOverrideQuota AndAlso quota.MaxAvailableSize > 0) OrElse quota.FreeSpace > 0 OrElse ((quota.Overflow = OverflowAction.Allow OrElse quota.Overflow = OverflowAction.AllowWithWarning) AndAlso quota.MaxAvailableSize > 0)

            DisplayUploadQuotaInfo(idFolder, quota)
            If allowUpload Then
                RAUfiles.MaxFileSize = quota.MaxUploadFileSize
            End If
            DVcurrentPath.Visible = allowUpload
            DVfolderSelector.Visible = allowUpload AndAlso _HasFoldersToSelect
            DVhidden.Visible = allowUpload
            DVitemsType.Visible = allowUpload
            DVuploader.Visible = allowUpload
            BTNaddFiles.Visible = allowUpload
        Else
            CTRLmessages.Visible = False
        End If
    End Sub
    Private Sub DisplayUploadQuotaInfo(idFolder As Long, quota As dtoContainerQuota)
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

    Public Function GetFiles() As List(Of dtoUploadedItem)
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
            item.IsVisible = Not CBXhideItems.Checked
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
        CBXhideItems.Checked = False
        If RBLitemType.Items.Count > 0 Then
            RBLitemType.SelectedValue = CInt(ItemType.File)
        End If
    End Sub
    Protected Function GetCssClass(index As Integer)
        Dim cssClass As String = ""
        If index = 0 Then
            cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString
        End If
        If index - 1 = _itemsCount Then
            cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
        End If
        Return cssClass
    End Function

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
#End Region

    Private Sub BTNaddFiles_Click(sender As Object, e As EventArgs) Handles BTNaddFiles.Click
        Dim files As List(Of dtoUploadedItem) = GetFiles()
        If files.Any() Then
            RaiseEvent AddFiles(CTRLfolderSelector.IdSelectedFolder, files)
            ResetInputItems()
        End If
    End Sub

    Private Sub CTRLfolderSelector_SelectFolderWithQuota(idFolder As Long, quota As dtoContainerQuota) Handles CTRLfolderSelector.SelectFolderWithQuota
        InitializeQuotaInfo(idFolder, quota)
    End Sub
End Class