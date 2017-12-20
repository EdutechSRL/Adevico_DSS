Imports System.Linq
Imports System.Web

Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports Telerik.Web.UI
Imports Telerik.Web.UI.Widgets
Imports lm.Comol.Core.BaseModules.Editor.Business

Public Class TelerikDBContentProvider
    Inherits FileBrowserContentProvider
    Private _Service As ServiceRepositoryContent
    Private _ServiceEditor As ServiceEditor
    Private _Person As Person
    Private _Community As Community
    Private _Utility As OLDpageUtility
    Private _StorePath As String

    Private ReadOnly Property Utility As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
    End Property

    Public Shared Function GetUserFolder(utility As OLDpageUtility) As String
        My.Resources.Culture = New System.Globalization.CultureInfo(utility.LinguaCode)
        Return My.Resources.Resource.UserFolder
    End Function


    Private ReadOnly Property CurrentService() As ServiceRepositoryContent
        Get
            If _Service Is Nothing Then
                My.Resources.Culture = New System.Globalization.CultureInfo(Utility.LinguaCode)
                _Service = New ServiceRepositoryContent(Utility.CurrentContext, Utility.CurrentContext.UserContext.CurrentUserID, Utility.CurrentContext.UserContext.CurrentCommunityID, My.Resources.Resource.UserFolder, My.Resources.Resource.CommunityFolder)
            End If
            Return _Service
        End Get
    End Property
    Private ReadOnly Property Service() As ServiceEditor
        Get
            If _ServiceEditor Is Nothing Then
                _ServiceEditor = New ServiceEditor(Utility.CurrentContext)
            End If
            Return _ServiceEditor
        End Get
    End Property

    Private ReadOnly itemHandlerPath As String

    Public Sub New(ByVal context As HttpContext, ByVal searchPatterns As String(), ByVal viewPaths As String(), ByVal uploadPaths As String(), ByVal deletePaths As String(), ByVal selectedUrl As String, _
     ByVal selectedItemTag As String)
        MyBase.New(context, searchPatterns, viewPaths, uploadPaths, deletePaths, selectedUrl, _
         selectedItemTag)



        Me.itemHandlerPath = Service.ImageHandlerPath(Utility.BaseUrlDrivePath & lm.Comol.Core.BaseModules.Editor.RootObject.ConfigurationFile(Utility.SystemSettings.EditorConfigurationPath)) 'ManagerConfiguration.GetInstance.Presenter.EditorItemHandlerPath
        If itemHandlerPath.StartsWith("~/") Then
            itemHandlerPath = HttpContext.Current.Request.ApplicationPath.TrimEnd("/"c) & itemHandlerPath.Substring(1)
        End If


        'If (_itemHandlerPath.StartsWith("~/")) Then
        '    If HttpContext.Current.Request.ApplicationPath = "/" Then
        '        Dim oUtility As New OLDpageUtility(HttpContext.Current)
        '        _itemHandlerPath = _itemHandlerPath.Substring(1) ' oUtility.ApplicationUrlBase + _itemHandlerPath.Substring(1)
        '    Else
        '        _itemHandlerPath = HttpContext.Current.Request.ApplicationPath + _itemHandlerPath.Substring(1)
        '    End If
        'End If
        'If System.String.IsNullOrEmpty(_itemHandlerPath) = False Then
        '    _itemHandlerPath = _itemHandlerPath.Replace("//", "/")
        'End If
        'If (Not (selectedItemTag Is Nothing) And selectedItemTag <> String.Empty) Then
        '    selectedItemTag = ExtractPath(RemoveProtocolNameAndServerName(selectedItemTag))
        'End If
        'If (Not (selectedItemTag Is Nothing) And selectedItemTag <> String.Empty) Then
        '    selectedItemTag = ExtractPathFromUrl(RemoveProtocolNameAndServerName(selectedItemTag))
        'End If

        Dim oCurrent As New OLDpageUtility(Me.Context)
        _Person = New Person(oCurrent.CurrentUser.ID)
        If oCurrent.WorkingCommunityID = 0 Then
            _Community = Nothing
        Else
            _Community = New Community(oCurrent.WorkingCommunityID)
        End If

    End Sub

#Region "OVERRIDES"
    Public Overrides Function ResolveRootDirectoryAsTree(ByVal path As String) As DirectoryItem
        Dim directory As DirectoryItem = CurrentService.GetDirectoryItem(path, True)

        If directory Is Nothing Then
            Return Nothing
        End If

        'directory.Files = CurrentService.GetChildFiles(path, Me.SearchPatterns, Me.itemHandlerPath)
        'directory.Permissions = directory GetPermissions(path)
        'For Each dir As DirectoryItem In directory.Directories
        '    dir.Permissions = GetPermissions(path)
        'Next

        Return directory
    End Function

    Public Overrides Function ResolveDirectory(ByVal path As String) As DirectoryItem
        Dim directory As DirectoryItem = CurrentService.GetDirectoryItem(path, True)

        If directory Is Nothing Then
            Return Nothing
        End If

        'directory.Permissions = GetPermissions(directory.Path)
        directory.Files = CurrentService.GetChildFiles(path, Me.SearchPatterns, Me.itemHandlerPath)
        'For Each file As FileItem In directory.Files
        '    file.Permissions = GetPermissions(file.Location)
        'Next

        Return directory
    End Function

    Public Overrides Function GetFileName(ByVal url As String) As String
        Return Path.GetFileName(ExtractPathFromUrl(url))
    End Function
    Public Overrides Function GetPath(ByVal url As String) As String
        Return CurrentService.GetPath(ExtractPathFromUrl(url))
    End Function


    Private ReadOnly Property StorePath() As String
        Get
            If String.IsNullOrEmpty(_StorePath) Then
                _StorePath = ObjectFilePath.CreateByConfigPath(ManagerConfiguration.GetInstance.File.Repository, "", "").Drive
            End If
            Return _StorePath
        End Get
    End Property

    Public Overrides Function GetFile(ByVal url As String) As Stream
        Dim buffer As Byte() = CurrentService.GetItemContent(ExtractPathFromUrl(url), Utility.SystemSettings.BaseFileRepositoryPath.DrivePath)
        If Not [Object].Equals(buffer, Nothing) Then
            Return New MemoryStream(buffer)
        End If
        Return Nothing
    End Function
    Public Overrides Function StoreBitmap(ByVal bitmap As Bitmap, ByVal url As String, ByVal format As ImageFormat) As String
        Dim newItemPath As String = ExtractPathFromUrl(url)
        Dim name As String = GetFileName(newItemPath)
        Dim itemPath As String = GetPath(newItemPath)

        Dim tempFilePath As String = Path.GetTempFileName()
        bitmap.Save(tempFilePath)
        Dim content As Byte()
        Using inputStream As FileStream = File.OpenRead(tempFilePath)
            Dim size As Long = inputStream.Length
            Content = New Byte(size - 1) {}
            inputStream.Read(Content, 0, Convert.ToInt32(size))
        End Using

        If lm.Comol.Core.File.Exists.File_FM(tempFilePath) Then
            lm.Comol.Core.File.Delete.File_FM(tempFilePath)
        End If
        Return String.Empty

        'Dim newItemPath As String = ExtractPathFromUrl(url)
        'Dim name As String = GetFileName(newItemPath)
        'Dim path As String = GetPath(newItemPath)
        'Dim tempFilePath As String = System.IO.Path.GetTempFileName()
        'bitmap.Save(tempFilePath)
        'Dim content As Byte()
        'Using inputStream As FileStream = File.OpenRead(tempFilePath)
        '    Dim size As Long = inputStream.Length
        '    content = New Byte(size - 1) {}
        '    inputStream.Read(content, 0, Convert.ToInt32(size))
        'End Using

        'If File.Exists(tempFilePath) Then
        '    File.Delete(tempFilePath)
        'End If

        'Dim [error] As String = CurrentService.StoreFile(name, path, GetImageMimeType(bitmap), content)

        'Return If([String].IsNullOrEmpty([error]), [String].Format("{0}{1}{2}", path, PathSeparator, name), [String].Empty)
        Return ""
    End Function
    Public Overrides Function StoreFile(ByVal file As UploadedFile, ByVal path As String, ByVal name As String, ByVal ParamArray arguments As String()) As String
        Dim item As New lm.Comol.Core.BaseModules.Editor.EditorRepositoryItem

        With item
            .Identifyer = Guid.NewGuid
            '.FileSystemPath = Me.CurrentManager.StorePath
            '.FileSystemName = .Id.ToString
            .IsDirectory = False
            lm.Comol.Core.File.Create.FromStream(file.InputStream, Utility.BaseUserRepositoryPath & .Identifyer.ToString & ".stored")
            .Size = file.InputStream.Length
            .Name = file.GetNameWithoutExtension
            .Extension = file.GetExtension
            .Description = ""
            .MimeType = file.ContentType
        End With

        Dim fItem As lm.Comol.Core.BaseModules.Editor.EditorRepositoryItem = Me.CurrentService.StoreFile(item, path)
        If IsNothing(fItem) Then
            lm.Comol.Core.File.Delete.File_FM(Utility.BaseUserRepositoryPath & item.Identifyer.ToString & ".stored")
        End If
        'Dim fileLength As Long = file.InputStream.Length
        'Dim content As Byte() = New Byte(fileLength - 1) {}
        'file.InputStream.Read(content, 0, CInt(fileLength))

        'Dim [error] As String = dataServer.StoreFile(name, path, file.ContentType, content)

        'Return If([String].IsNullOrEmpty([error]), [String].Format("{0}{1}{2}", path, PathSeparator, name), [String].Empty)
        Return ""
    End Function
    Public Overrides Function DeleteFile(ByVal path As String) As String
        CurrentService.DeleteItem(path)
        Return String.Empty
    End Function
    Public Overrides Function DeleteDirectory(ByVal path As String) As String
        CurrentService.DeleteItem(path)
        Return String.Empty
    End Function
    Public Overrides Function CreateDirectory(ByVal location As String, ByVal name As String) As String
        If CurrentService.ItemExists([String].Format("{0}{1}", location, name)) Then
            Return "Directory with the same name already exists!"
        End If

        CurrentService.CreateDirectory(name, location)
        Return String.Empty
        'Dim [error] As String = CurrentService.CreateDirectory(name, location)
        'Return If(Not [String].IsNullOrEmpty([error]), [String].Format("{0}{1}", location, name), [String].Empty)
    End Function
    Public Overrides Function MoveFile(ByVal path As String, ByVal newPath As String) As String
        If Not CurrentService.ItemExists(newPath) Then
            CurrentService.UpdateItem(path, newPath)
            Return String.Empty
        Else
            Return "File or folder with the same name already exists!"
        End If
    End Function
    Public Overrides Function MoveDirectory(ByVal path As String, ByVal newPath As String) As String
        Return MoveFile(path, newPath)
    End Function
    Public Overrides Function CopyFile(ByVal path As String, ByVal newPath As String) As String
        If Not CurrentService.ItemExists(newPath) Then
            CurrentService.CopyItem(path, newPath, Utility.BaseUserRepositoryPath)
            Return String.Empty
        Else
            Return "File or folder with the same name already exists!"
        End If
    End Function
    Public Overrides Function CopyDirectory(ByVal path As String, ByVal destinationPath As String) As String
        Dim destFullName As String = destinationPath & path.Trim(PathSeparator).Substring(path.Trim(PathSeparator).LastIndexOf(PathSeparator) + 1)

        Return CopyFile(path, destFullName)
    End Function

    Public Overrides Function CheckDeletePermissions(ByVal folderPath As String) As Boolean
        If String.IsNullOrEmpty(folderPath) Then
            folderPath = "/"
        End If
        For Each path As String In Me.DeletePaths
            If folderPath.StartsWith(path, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Overrides Function CheckWritePermissions(ByVal folderPath As String) As Boolean
        If String.IsNullOrEmpty(folderPath) Then
            folderPath = "/"
        End If
        For Each path As String In Me.UploadPaths
            If folderPath.StartsWith(path, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Overrides Function CheckReadPermissions(ByVal folderPath As String) As Boolean
        If String.IsNullOrEmpty(folderPath) Then
            folderPath = "/"
        End If
        For Each viewPath As String In Me.ViewPaths
            If folderPath.StartsWith(viewPath, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Overrides ReadOnly Property CanCreateDirectory() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

#Region "PRIVATE METHODS"

    Private Function GetPermissions(ByVal folderPath As String) As PathPermissions
        Dim permissions As PathPermissions = PathPermissions.Read
        If CheckDeletePermissions(folderPath) Then
            permissions = PathPermissions.Delete Or permissions
        End If
        If CheckWritePermissions(folderPath) Then
            permissions = PathPermissions.Upload Or permissions
        End If

        Return permissions
    End Function
    Private Function ExtractPathFromUrl(ByVal url As String) As String
        Dim itemUrl As String = RemoveProtocolNameAndServerName(url)
        If itemUrl Is Nothing Then
            Return String.Empty
        End If
        If itemUrl.StartsWith(Me.itemHandlerPath) Then
            Return itemUrl.Substring(GetItemUrl(String.Empty).Length)
        End If
        Return itemUrl
    End Function
    Private Function GetItemUrl(ByVal virtualItemPath As String) As String
        Dim escapedPath As String = Context.Server.UrlEncode(virtualItemPath)
        Return String.Format("{0}?path={1}", Me.itemHandlerPath, escapedPath)
    End Function
    Private Function GetImageMimeType(ByVal bitmap As Bitmap) As String
        For Each codec As ImageCodecInfo In ImageCodecInfo.GetImageDecoders()
            If codec.FormatID = bitmap.RawFormat.Guid Then
                Return codec.MimeType
            End If
        Next

        Return "image/unknown"
    End Function

#End Region
End Class
