
Public Class FileSystemManager
	Inherits ObjectBase

	Private Shared _rootFolder As String

	Shared Sub New()
		_rootFolder = HttpContext.Current.Request.PhysicalApplicationPath
		_rootFolder = _rootFolder.Substring(0, _rootFolder.LastIndexOf("\"))
	End Sub

	Public Shared Function GetRootPath() As String
		Return _rootFolder

	End Function

	Public Shared Sub SetRootPath(ByVal path As String)
		_rootFolder = path
	End Sub

	'Public Shared Function GetItems() As List(Of FileSystemItem)
	'	Return GetItems(_rootFolder)
	'End Function

	'Public Shared Function GetItems(ByVal path As String) As List(Of FileSystemItem)
	'	Dim folders As String() = Directory.GetDirectories(path)
	'	Dim files As String() = Directory.GetFiles(path)

	'	Dim list As New List(Of FileSystemItem)()
	'	For Each folder As String In folders
	'		Dim item As New FileSystemItem()
	'		Dim di As New DirectoryInfo(folder)
	'		item.Name = di.Name
	'		item.FullName = di.FullName
	'		item.CreatedDate = di.CreationTime
	'		item.IsFolder = True
	'		item.Extension = "folder"
	'		list.Add(item)
	'	Next

	'	For Each file As String In files
	'		Dim item As New FileSystemItem()
	'		Dim fi As New FileInfo(file)
	'		item.Name = fi.Name
	'		item.FullName = fi.FullName
	'		item.CreatedDate = fi.CreationTime
	'		item.IsFolder = True
	'		item.Size = fi.Length
	'		item.Extension = fi.Extension
	'		list.Add(item)
	'	Next
	'	If path.ToLower() <> _rootFolder.ToLower() Then
	'		Dim topItem As New FileSystemItem()
	'		Dim topDi As DirectoryInfo = New DirectoryInfo(path).Parent
	'		topItem.Name = "[Parent]"
	'		topItem.FullName = topDi.FullName
	'		list.Insert(0, topItem)

	'		Dim rootItem As New FileSystemItem()
	'		Dim rootDi As New DirectoryInfo(_rootFolder)
	'		rootItem.Name = "[Root]"
	'		rootItem.FullName = rootDi.FullName
	'		list.Insert(0, rootItem)
	'	End If
	'	Return list
	'End Function

	'Public Shared Sub CreateFolder(ByVal name As String, ByVal parentName As String)
	'	Dim di As New DirectoryInfo(parentName)
	'	di.CreateSubdirectory(name)
	'End Sub

	'Public Shared Sub DeleteFolder(ByVal path As String)
	'	Directory.Delete(path)
	'End Sub

	'Public Shared Sub MoveFolder(ByVal oldPath As String, ByVal newPath As String)
	'	Directory.Move(oldPath, newPath)
	'End Sub

	'Public Shared Sub CreateFile(ByVal fileName As String, ByVal path As String)
	'	Dim fs As FileStream = File.Create(path + "\" + fileName)
	'	fs.Close()
	'End Sub

	'Public Shared Sub CreateFile(ByVal fileName As String, ByVal path As String, ByVal contents As Byte())
	'	Dim fs As FileStream = File.Create(path + "\" + fileName)
	'	fs.Write(contents, 0, contents.Length)
	'	fs.Close()
	'End Sub

	'Public Shared Sub DeleteFile(ByVal path As String)
	'	File.Delete(path)
	'End Sub

	'Public Shared Sub MoveFile(ByVal oldPath As String, ByVal newPath As String)
	'	File.Move(oldPath, newPath)
	'End Sub

	'Public Shared Function GetItemInfo(ByVal path As String) As FileSystemItem
	'	Dim item As New FileSystemItem()
	'	If Directory.Exists(path) Then
	'		Dim di As New DirectoryInfo(path)
	'		item.Name = di.Name
	'		item.FullName = di.FullName
	'		item.CreatedDate = di.CreationTime
	'		item.IsFolder = True
	'		item.LastAccessDate = di.LastAccessTime
	'		item.FileCount = di.GetFiles().Length
	'		item.SubFolderCount = di.GetDirectories().Length
	'		item.Extension = "folder"
	'	Else
	'		Dim fi As New FileInfo(path)
	'		item.Name = fi.Name
	'		item.FullName = fi.FullName
	'		item.CreatedDate = fi.CreationTime
	'		item.LastAccessDate = fi.LastAccessTime
	'		item.LastWriteDate = fi.LastWriteTime
	'		item.IsFolder = False
	'		item.Size = fi.Length
	'		item.Extension = fi.Extension
	'	End If
	'	Return item
	'End Function

	'Public Shared Sub CopyFolder(ByVal source As String, ByVal destination As String)
	'	Dim files As String()
	'	If destination(destination.Length - 1) <> Path.DirectorySeparatorChar Then
	'		destination += Path.DirectorySeparatorChar
	'	End If
	'	If Not Directory.Exists(destination) Then
	'		Directory.CreateDirectory(destination)
	'	End If
	'	files = Directory.GetFileSystemEntries(source)
	'	For Each file As String In files
	'		If Directory.Exists(file) Then
	'			CopyFolder(file, destination + Path.GetFileName(file))
	'		Else
	'			file.Copy(file, destination + Path.GetFileName(file), True)
	'		End If
	'	Next
	'End Sub
End Class
