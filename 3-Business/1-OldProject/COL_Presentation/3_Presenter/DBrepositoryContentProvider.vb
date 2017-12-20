Imports Telerik.WebControls.RadEditorUtils
Imports System.Drawing
Imports System
Imports System.Collections
Imports lm.Comol.Core.File

Public Class DBrepositoryContentProvider
	Inherits FileBrowserContentProvider
	Private _Manager As ManagerBrowsingRepository
	Private _itemHandlerPath As String
	Private _Person As Person
	Private _Community As Community


	Private ReadOnly Property CurrentManager() As ManagerBrowsingRepository
		Get
			If _Manager Is Nothing Then
				Dim oCurrent As New OLDpageUtility(Me.Context)
				My.Resources.Culture = New System.Globalization.CultureInfo(oCurrent.LinguaCode)
				_Manager = New ManagerBrowsingRepository(_Person, _Community, oCurrent.UserSessionLanguage, True, My.Resources.Resource.UserFolder, My.Resources.Resource.CommunityFolder)
			End If
			Return _Manager
		End Get
	End Property

	Private ReadOnly Property ItemHandlerPath() As String
		Get
			Return _itemHandlerPath
		End Get
	End Property

	Private Function GetPermission(ByVal oLabel As LabelTag) As PathPermissions
		If oLabel Is Nothing Then
			Return PathPermissions.Read
		ElseIf oLabel.ID < 0 And oLabel.isUserDefined Then
			Return PathPermissions.Read Or PathPermissions.Upload
		ElseIf oLabel.ID < 0 Then
			Return PathPermissions.Read
		ElseIf oLabel.isUserDefined And oLabel.CreatedBy.ID = Me._Person.ID Then
			Return PathPermissions.Read Or PathPermissions.Delete Or PathPermissions.Upload
		Else
			Return PathPermissions.Read Or PathPermissions.Delete Or PathPermissions.Upload
		End If
	End Function

	Private Function GetFilePermission(ByVal oRepository As UserRepository) As PathPermissions
		If oRepository.UserFile Is Nothing Then
			Return PathPermissions.Read
		ElseIf oRepository.CreatedBy.ID = Me._Person.ID And TypeOf oRepository Is UserRepository Then
			Return PathPermissions.Read Or PathPermissions.Delete Or PathPermissions.Upload
		ElseIf Not TypeOf oRepository Is UserRepository Then
			' Verificare permessi comunità :)
			Return PathPermissions.Read	 ' Or PathPermissions.Upload
		Else
			Return PathPermissions.Read
		End If
	End Function

	Public Sub New(ByVal context As HttpContext, ByVal searchPatterns As String(), ByVal viewPaths As String(), ByVal uploadPaths As String(), ByVal deletePaths As String(), ByVal selectedUrl As String, ByVal selectedItemTag As String)
		MyBase.New(context, searchPatterns, viewPaths, uploadPaths, deletePaths, selectedUrl, selectedItemTag)
		_itemHandlerPath = ManagerConfiguration.GetInstance.Presenter.EditorItemHandlerPath
		If (_itemHandlerPath.StartsWith("~/")) Then
			If HttpContext.Current.Request.ApplicationPath = "/" Then
				Dim oUtility As New OLDpageUtility(HttpContext.Current)
				_itemHandlerPath = _itemHandlerPath.Substring(1) ' oUtility.ApplicationUrlBase + _itemHandlerPath.Substring(1)
			Else
				_itemHandlerPath = HttpContext.Current.Request.ApplicationPath + _itemHandlerPath.Substring(1)
			End If
		End If
		If System.String.IsNullOrEmpty(_itemHandlerPath) = False Then
			_itemHandlerPath = _itemHandlerPath.Replace("//", "/")
		End If
		If (Not (selectedItemTag Is Nothing) And selectedItemTag <> String.Empty) Then
			selectedItemTag = ExtractPath(RemoveProtocolNameAndServerName(selectedItemTag))
		End If
		Dim oCurrent As New OLDpageUtility(Me.Context)
		_Person = New Person(oCurrent.CurrentUser.ID)
		If oCurrent.WorkingCommunityID = 0 Then
			_Community = Nothing
		Else
			_Community = New Community(oCurrent.WorkingCommunityID)
		End If

	End Sub

	Private Function GetItemUrl(ByVal virtualItemPath As String) As String
		Return String.Format("{0}?path={1}", ItemHandlerPath, virtualItemPath)
	End Function

	Private Function GetFileUrl(ByVal oFile As LearningObjectFile) As String
		Return String.Format("{0}?path={1}", ItemHandlerPath, oFile.ID)
	End Function

	Private Function ExtractPath(ByVal itemUrl As String) As String
		If itemUrl Is Nothing Then
			Return String.Empty
		End If
		If itemUrl.StartsWith(_itemHandlerPath) Then
			Return itemUrl.Substring(GetItemUrl(String.Empty).Length)
		End If
		Return itemUrl
	End Function

	Private Function GetName(ByVal path As String) As String
		If path Is Nothing Then
			Return String.Empty
		End If
		Return path.Substring(path.LastIndexOf("/") + 1)
	End Function

	Private Function GetDirectoryPath(ByVal path As String) As String
		Return path.Substring(0, path.LastIndexOf("/") + 1)
	End Function

	Private Function IsChildOf(ByVal parentPath As String, ByVal childPath As String) As Boolean
		Return childPath.StartsWith(parentPath)
	End Function

	Private Function EndWithSlash(ByVal path As String) As String
		If Not path.EndsWith("/") Then
			Return path + "/"
		End If
		Return path
	End Function

	Private Function CombinePath(ByVal path1 As String, ByVal path2 As String) As String
		If path1.EndsWith("/") Then
			Return String.Format("{0}{1}", path1, path2)
		End If
		If path1.EndsWith("\\") Then
			path1 = path1.Substring(0, path1.Length - 1)
		End If
		Return String.Format("{0}/{1}", path1, path2)
	End Function

	Private Function GetChildDirectories(ByVal path As String, ByVal oVisibility As VisibilityStatus) As DirectoryItem()
		Dim directories() As DirectoryItem = New DirectoryItem() {}
		If IsChildOf(path, ExtractPath(SelectedUrl)) Then
			Dim oList As List(Of LabelTag) = Me.CurrentManager.GetChildDirectoryRows(path, oVisibility)
			ReDim directories(oList.Count - 1)
			Dim i As Integer
			For i = 0 To oList.Count - 1
				Dim oLabelTag As LabelTag = oList(i)
				Dim name As String = oLabelTag.DisplayName
				Dim itemFullPath As String = EndWithSlash(CombinePath(path, oLabelTag.DisplayName))
				directories(i) = New DirectoryItem(name, String.Empty, itemFullPath, _
				 itemFullPath, Me.GetPermission(oLabelTag), GetChildFiles(itemFullPath), _
				 GetChildDirectories(itemFullPath, oVisibility))
			Next i
			Return directories
		End If
		Return New DirectoryItem() {}
	End Function

	Private Function GetChildFiles(ByVal itemPath As String) As FileItem()
		If IsChildOf(itemPath, ExtractPath(SelectedUrl)) Then
			Dim oList As List(Of UserRepository) = Me.CurrentManager.GetChildFileRows(itemPath)
			Dim i As Integer

			Dim files As ArrayList = New ArrayList
			For i = 0 To oList.Count - 1
				Dim oRepository As UserRepository = oList(i)
				Dim name As String = oRepository.UserFile.CompleteName
				If (IsExtensionAllowed(oRepository.UserFile.Extension)) Then
					Dim itemFullPath As String = CombinePath(itemPath, oRepository.UserFile.CompleteName)

					Dim oFileItem As New FileItem(oRepository.UserFile.CompleteName, oRepository.UserFile.Extension, oRepository.UserFile.Size, _
					String.Empty, GetFileUrl(oRepository.UserFile), itemFullPath, GetFilePermission(oRepository))
					Dim oAttributes(2) As String
					oAttributes(0) = oRepository.UserFile.CompleteName
					oAttributes(1) = itemFullPath
					oAttributes(2) = oFileItem.Url
					oFileItem.Attributes = oAttributes

					files.Add(oFileItem)	 '/ SizeHelpers.FS(SizeHelpers.FileSizeCostants.)


					'Dim itemFullPath As String = CombinePath(itemPath, name)
					'files.Add(New FileItem(oRepository.UserFile.CompleteDisplayName, Path.GetExtension(oRepository.UserFile.CompleteDisplayName), oRepository.UserFile.Size, _
					'String.Empty, GetItemUrl(itemFullPath), itemFullPath, PathPermissions.Read))
				End If
			Next i
			Return CType(files.ToArray(GetType(FileItem)), FileItem())
		End If
		Return New FileItem() {}
	End Function

	Private Function IsExtensionAllowed(ByVal extension As String) As Boolean
		Return Array.IndexOf(SearchPatterns, "*.*") >= 0 Or Array.IndexOf(SearchPatterns, "*" + extension.ToLower()) >= 0
	End Function

	Private Function RemoveLastSlash(ByVal path As String) As String
		If path.EndsWith("/") Then
			Return path.Substring(0, path.Length - 1)
		End If
		Return path
	End Function

	Public Overrides Function ResolveRootDirectoryAsTree(ByVal path As String) As DirectoryItem
		Dim returnValue As DirectoryItem = New DirectoryItem(GetName(path), GetDirectoryPath(path), _
		 String.Empty, String.Empty, Me.GetPermission(Me.CurrentManager.GetItemFolder(path)), GetChildFiles(path), GetChildDirectories(path, VisibilityStatus.Visible))
		Return returnValue
	End Function

	Public Overrides Function ResolveRootDirectoryAsList(ByVal path As String) As DirectoryItem()
		Dim oFather As LabelTag = Me.CurrentManager.GetItemFolder(path)
		Dim oDirectories() As DirectoryItem
		If oFather Is Nothing Then
			Return oDirectories
		Else
			Dim oLabelList As List(Of LabelTag) = Me.CurrentManager.FolderList(VisibilityStatus.Visible).FindAll(New GenericPredicate(Of LabelTag, String)(oFather.Path, AddressOf LabelTag.FindByPathStart))
			If oLabelList.Count > 0 Then
				ReDim oDirectories(oLabelList.Count - 1)
				Dim i As Integer
				For Each oLabel As LabelTag In oLabelList
					Dim fullPath As String = RemoveLastSlash(Me.CurrentManager.GetItemPath(oLabel))

					oDirectories(i) = New DirectoryItem(oLabel.DisplayName, GetDirectoryPath(fullPath), _
					 String.Empty, String.Empty, GetPermission(oLabel), GetChildFiles(fullPath), New DirectoryItem() {})
					i += 1
				Next
				Return oDirectories
			Else
				Return oDirectories
			End If
		End If
		Return Nothing
	End Function

	Public Overrides Function ResolveDirectory(ByVal path As String) As DirectoryItem
		Dim directories() As DirectoryItem = New DirectoryItem() {}
		If DisplayMode <> FileBrowserDisplayMode.List Then
			directories = GetChildDirectories(path, VisibilityStatus.Visible)
		End If
		Return New DirectoryItem(GetName(path), EndWithSlash(GetDirectoryPath(path)), _
		 String.Empty, String.Empty, Me.GetPermission(Me.CurrentManager.GetItemFolder(path)), GetChildFiles(path), directories)
	End Function

	Public Overrides Function GetFileName(ByVal url As String) As String
		Return GetName(url)
	End Function

	Public Overrides Function GetPath(ByVal url As String) As String
		Return GetDirectoryPath(ExtractPath(RemoveProtocolNameAndServerName(url)))
	End Function

	Public Overrides Function GetFile(ByVal url As String) As Stream
		Dim content As Byte() = Me.CurrentManager.GetContent(ExtractPath(RemoveProtocolNameAndServerName(url)), VisibilityStatus.Visible)
		If Not Content Is Nothing Then
			Return New MemoryStream(Content)
		End If
		Return Nothing
	End Function

	Public Overrides Function StoreBitmap(ByVal bitmap As Bitmap, ByVal url As String, ByVal format As ImageFormat) As String
		Dim newItemPath As String = ExtractPath(RemoveProtocolNameAndServerName(url))
		Dim name As String = GetName(newItemPath)
		Dim itemPath As String = GetPath(newItemPath)
		Dim tempFilePath As String = Path.GetTempFileName()
		bitmap.Save(tempFilePath)
		Dim inputStream As FileStream = File.OpenRead(tempFilePath)
		Dim size As Integer = CType(inputStream.Length, Integer)
		Dim content(size - 1) As Byte
		inputStream.Read(content, 0, size)
		inputStream.Close()

        Delete.File_FM(tempFilePath)

        'DataServer.CreateItem(name, itemPath, "image/gif", False, content.Length, content)
        Return String.Empty
    End Function

	Public Overrides Function StoreFile(ByVal oFileUploaded As HttpPostedFile, ByVal path As String, ByVal name As String, ByVal ParamArray arguments As String()) As String
		Dim oFile As New LearningObjectFile

		With oFile
			.ID = Guid.NewGuid
			.FileSystemPath = Me.CurrentManager.StorePath
			.FileSystemName = .ID.ToString & ".stored"

			oFileUploaded.SaveAs(.FileSystemPath & .FileSystemName)
			.Size = oFileUploaded.ContentLength
			.Name = Left(name, name.IndexOf("."))
			.Extension = Replace(name, .Name, "")
			.Description = ""
			.CreatedBy = Me._Person
			.CreatedAt = Now
			.ContentType = New MimeType(.Extension, oFileUploaded.ContentType)
		End With

		Me.CurrentManager.CreateFile(oFile, path)
		''oFileUploaded.ContentType


		''Dim fileLength As Integer = CType(File.InputStream.Length, Integer)
		''Dim content(fileLength - 1) As Byte
		''file.InputStream.Read(content, 0, fileLength)
		'If Not DataServer.GetItemRow(CombinePath(path, name)) Is Nothing Then
		'	DataServer.ReplaceItemContent(path, content)
		'Else
		'	DataServer.CreateItem(name, path, file.ContentType, False, fileLength, content)
		'End If
		Return String.Empty
	End Function

	Public Overrides Function DeleteFile(ByVal path As String) As String
		Me.CurrentManager.DeleteFileItem(GetFileName(path), Me.GetPath(path))
		Return String.Empty
	End Function


	Public Overrides Function DeleteDirectory(ByVal path As String) As String
		Me.CurrentManager.DeleteLabelByPath(path)
		Return String.Empty
	End Function
	Public Overrides Function CreateDirectory(ByVal path As String, ByVal name As String) As String
		Me.CurrentManager.CreateLabelByPath(name, path)
		Return String.Empty
	End Function
	Public Overrides ReadOnly Property CanCreateDirectory() As Boolean
		Get
			Return True
		End Get
	End Property
End Class