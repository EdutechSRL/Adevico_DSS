Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_AddVersion
    Inherits FRbaseControl

#Region "Internal"
    Private Property IdItem As Long
        Get
            Return ViewStateOrDefault("IdItem", -1)
        End Get
        Set(value As Long)
            ViewState("IdItem") = value
        End Set
    End Property
    Public ReadOnly Property AddVersionDialogTitle As String
        Get
            Return Resource.getValue("AddVersionDialogTitle")
        End Get
    End Property
    Public Event AddVersion(idItem As Long, file As dtoUploadedItem)

    'Private Property RepositoryIdPerson As Integer
    '    Get
    '        Return ViewStateOrDefault("IdRepositoryPerson", 0)
    '    End Get
    '    Set(value As Integer)
    '        ViewState("IdRepositoryPerson") = value
    '    End Set
    'End Property
    'Private Property RepositoryIdCommunity As Integer
    '    Get
    '        Return ViewStateOrDefault("IdRepositoryCommunity", 0)
    '    End Get
    '    Set(value As Integer)
    '        ViewState("IdRepositoryCommunity") = value
    '    End Set
    'End Property
    Private Property FileType As ItemType
        Get
            Return ViewStateOrDefault("FileType", ItemType.File)
        End Get
        Set(value As ItemType)
            ViewState("FileType") = value
        End Set
    End Property
    Private Property FileExtension As String
        Get
            Return ViewStateOrDefault("FileExtension", "")
        End Get
        Set(value As String)
            ViewState("FileExtension") = value
        End Set
    End Property
    Public ReadOnly Property DialogIdentifier As String
        Get
            Return LTcssClassDialog.Text
        End Get
    End Property
    Public Property UpdateLinks As Boolean
        Get
            Return ViewStateOrDefault("UpdateLinks", False)
        End Get
        Set(value As Boolean)
            ViewState("UpdateLinks") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPcloseAddVersionDialog, False, True)
            .setButton(BTNaddVersion, True)
            '.setLiteral(LTaddVersionDescription)
            .setLabel(LBaddVersion_t)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As dtoDisplayRepositoryItem, quota As dtoContainerQuota)
        IdItem = item.Id
        Dim tempPath As String = GetUploadDiskPath()
        If Not String.IsNullOrWhiteSpace(tempPath) Then
            RAUfiles.TemporaryFolder = tempPath
            RAUfiles.TemporaryFileExpiration = New TimeSpan(2, 0, 0)
        End If
        RAUfiles.AllowedFileExtensions = New String() {item.Extension}
        Resource.setLiteral(LTaddVersionDescription)
        FileType = item.Type
        FileExtension = item.Extension
        LTaddVersionDescription.Text = Replace(LTaddVersionDescription.Text, "#filerender#", GetFilenameRender(item.DisplayName, item.Extension, item.Type))
        InitializeRepositoryPath(item.Repository)
        InitializeQuotaInfo(item.IdFolder, quota)
    End Sub
    Public Sub InitializeControlForInternalItem(item As dtoDisplayRepositoryItem)
        IdItem = item.Id
        Dim tempPath As String = GetUploadDiskPath()
        If Not String.IsNullOrWhiteSpace(tempPath) Then
            RAUfiles.TemporaryFolder = tempPath
            RAUfiles.TemporaryFileExpiration = New TimeSpan(2, 0, 0)
        End If
        RAUfiles.AllowedFileExtensions = New String() {item.Extension}
        Resource.setLiteral(LTaddVersionDescription)
        FileType = item.Type
        FileExtension = item.Extension
        LTaddVersionDescription.Text = Replace(LTaddVersionDescription.Text, "#filerender#", GetFilenameRender(item.DisplayName, item.Extension, item.Type))
        InitializeRepositoryPath(item.Repository)
        InitializeQuotaInfo(item.IdFolder, Nothing)
    End Sub

    Private Sub InitializeQuotaInfo(idFolder As Long, quota As dtoContainerQuota)
        If Not IsNothing(quota) Then
            Dim allowUpload As Boolean = (quota.AllowOverrideQuota AndAlso quota.MaxAvailableSize > 0) OrElse quota.FreeSpace > 0 OrElse ((quota.Overflow = OverflowAction.Allow OrElse quota.Overflow = OverflowAction.AllowWithWarning) AndAlso quota.MaxAvailableSize > 0)

            DisplayUploadQuotaInfo(idFolder, quota)
            If allowUpload Then
                RAUfiles.MaxFileSize = quota.MaxUploadFileSize
            End If
            DVuploader.Visible = allowUpload
            BTNaddVersion.Visible = allowUpload
        Else
            CTRLmessages.Visible = False
        End If
    End Sub
    Private Sub DisplayUploadQuotaInfo(idFolder As Long, quota As dtoContainerQuota)
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Dim key As String = "DisplayUploadQuotaInfo"
        If quota.FreeSpace > 0 Then
            key &= ".FreeSpace"
        End If


        If (quota.FreeSpace = 0 AndAlso quota.AllowOverrideQuota) OrElse (quota.MaxUploadFileSize > quota.FreeSpace AndAlso (quota.Overflow = OverflowAction.Allow OrElse quota.Overflow = OverflowAction.AllowWithWarning)) Then
            key &= ".AllowOverrideQuota"
        End If
        key &= ".OverflowAction." & quota.Overflow.ToString()
        Dim message As String = Resource.getValue(key)

        If Not String.IsNullOrWhiteSpace(message) Then
            Select Case quota.Overflow
                Case OverflowAction.Allow, OverflowAction.AllowWithWarning
                    If quota.FreeSpace > 0 AndAlso quota.FreeSpace > quota.MaxUploadFileSize Then
                        mType = lm.Comol.Core.DomainModel.Helpers.MessageType.info
                    End If
            End Select
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
        'If RAUdropFiles.UploadedFiles.Count > 0 Then
        '    items.AddRange(GetFiles(RAUdropFiles))
        'End If
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
            item.Extension = f.GetExtension
            item.Name = EscapeInvalidCharacter(f.GetNameWithoutExtension)
            item.OriginalName = item.Name
            item.ContentType = f.ContentType
            item.Size = f.ContentLength
            item.SavedFileName = item.UniqueId.ToString & item.Extension
            item.ThumbnailPath = GetFinalThumbnailPath()
            item.SavedPath = GetFinalPath()
            item.Type = FileType
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
#End Region

    Private Sub BTNaddVersion_Click(sender As Object, e As EventArgs) Handles BTNaddVersion.Click
        Dim files As List(Of dtoUploadedItem) = GetFiles()
        If files.Any() Then
            RaiseEvent AddVersion(IdItem, files.FirstOrDefault())
        End If
    End Sub

End Class