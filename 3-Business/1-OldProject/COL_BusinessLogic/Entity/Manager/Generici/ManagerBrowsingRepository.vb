Imports Comol.Entity
Imports Comol.Entity.File

Namespace Comol.Manager
	Public Class ManagerBrowsingRepository
		Inherits ManagerRepository



        '#Region "NUOVE FUNZIONI TELERIK"
        '        Public Function GetDirectoryItem(ByVal path As String, ByVal includeSubfolders As Boolean) As Telerik.Web.UI.DirectoryItem
        '            Dim item As DataRow = Me.GetItemRowFromPath(path)

        '            Return If((item IsNot Nothing AndAlso Convert.ToInt32(item("IsDirectory")) = 1), Me.CreateDirectoryItem(item, includeSubfolders), Nothing)
        '        End Function

        '        Private Function CreateDirectoryItem(ByVal item As DataRow, ByVal includeSubfolders As Boolean) As DirectoryItem
        '            'correct permissions should be applied from the content provider
        '            Dim directory As New DirectoryItem(item("Name").ToString(), Me.GetLoaction(item), Me.GetFullPath(item), [String].Empty, PathPermissions.Read, Nothing, _
        '             Nothing)

        '            If includeSubfolders Then
        '                Dim subDirItems As DataRow() = GetChildDirectories(item)
        '                Dim subDirs As New List(Of DirectoryItem)()

        '                For Each subDir As DataRow In subDirItems
        '                    subDirs.Add(CreateDirectoryItem(subDir, False))
        '                Next

        '                directory.Directories = subDirs.ToArray()
        '            End If

        '            Return directory
        '        End Function

        '#End Region
		Private _Folders = New List(Of LabelTag)
		Private _FoldersList = New List(Of LabelTag)

		Public Property Folders(Optional ByVal oVisibility As VisibilityStatus = VisibilityStatus.All) As List(Of LabelTag)
			Get
				If IsNothing(_Folders) Then
					_Folders = New List(Of LabelTag)
				End If
				Return _Folders.FindAll(New GenericPredicate(Of LabelTag, VisibilityStatus)(oVisibility, AddressOf LabelTag.FindByVisibility))
			End Get
			Set(ByVal value As List(Of LabelTag))
				_Folders = value
				_FoldersList = FolderToList(value)
			End Set
		End Property
		Public ReadOnly Property FolderList(Optional ByVal oVisibility As VisibilityStatus = VisibilityStatus.All) As List(Of LabelTag)
			Get
				Return _FoldersList.FindAll(New GenericPredicate(Of LabelTag, VisibilityStatus)(oVisibility, AddressOf LabelTag.FindByVisibility))
			End Get
		End Property

		Public ReadOnly Property Fathers() As List(Of LabelTag)
			Get
				Return Folders(VisibilityStatus.Visible).FindAll(New GenericPredicate(Of LabelTag, Boolean)(True, AddressOf LabelTag.FindMainFather))
			End Get
		End Property
		Public Sub New(ByVal oPerson As Person, ByVal oCommunity As Community, ByVal oLanguage As Lingua, Optional ByVal UseCache As Boolean = True, Optional ByVal UserFolderName As String = "", Optional ByVal CommunityFolderName As String = "")
			MyBase.new(oPerson, oCommunity, oLanguage, UseCache, UserFolderName, CommunityFolderName)

			'Me.Files = Me.ListFilesAvailableForUser(oPerson, oCommunity)
			Me.Folders = ListLabelAvailableForUser(oPerson, oCommunity, VisibilityStatus.All, True)
		End Sub

		Private Function GetLabelByPath(ByVal path As String) As LabelTag
			Dim oLabelTag As LabelTag = GetItemFolder(path)
			If oLabelTag Is Nothing Then
				Return New LabelTag()
			End If
			Return oLabelTag
		End Function
		Public Function GetChildDirectoryRows(ByVal path As String, ByVal oVisibility As VisibilityStatus) As List(Of LabelTag)
			Dim oFather As LabelTag = GetLabelByPath(path)
			If oFather Is Nothing Then
				Return New List(Of LabelTag)
			Else
				Return oFather.Labels.FindAll(New GenericPredicate(Of LabelTag, VisibilityStatus)(oVisibility, AddressOf LabelTag.FindByVisibility))
			End If
		End Function
		Public Function GetChildFileRows(ByVal path As String) As List(Of UserRepository)
			Dim oFolder As LabelTag = GetLabelByPath(path)

			Return Me.Files(VisibilityStatus.Visible).FindAll(New GenericPredicate(Of UserRepository, LabelTag)(oFolder, AddressOf UserRepository.FindByLabel))
		End Function

		Private Function FolderToList(ByVal Folders As List(Of LabelTag)) As List(Of LabelTag)
			Dim oList As List(Of LabelTag) = New List(Of LabelTag)

			For Each oFolder As LabelTag In Folders
				oList.Add(oFolder)
				If oFolder.Labels.Count > 0 Then
					oList.AddRange(FolderToList(oFolder.Labels))
				End If
			Next
			Return oList
		End Function
		Private Function SearchFolder(ByVal LabelID As Int64, ByVal Name As String) As LabelTag
			Dim oList As List(Of LabelTag) = Me._FoldersList.FindAll(New GenericPredicate(Of LabelTag, Int64)(LabelID, AddressOf LabelTag.FindByFather))
			If oList.Count = 0 Then
				Return Nothing
			Else
				Return oList.Find(New GenericPredicate(Of LabelTag, String)(Name, AddressOf LabelTag.FindByName))
			End If
		End Function
		Public Function GetItemFolder(ByVal path As String) As LabelTag
			If path.EndsWith("/") Then
				path = path.Substring(0, path.Length - 1)
			End If
			Dim names() As String = path.Split("/")
			'Start search in root;
			Dim searchedLabel As LabelTag = Nothing
			Dim itemId As Int64 = 0
			Dim i As Integer
			For i = 1 To names.Length - 1
				Dim name As String = names(i)
				searchedLabel = Me.SearchFolder(itemId, name)

				If searchedLabel Is Nothing Then
					Return Nothing
				End If
				itemId = searchedLabel.ID
			Next i
			Return searchedLabel
		End Function
		Public Function GetContent(ByVal path As String, ByVal oVisibility As VisibilityStatus) As Byte()
			Dim FolderName As String = Left(path, path.LastIndexOf("/"))
			Dim FileName As String = Replace(path, FolderName & "/", "")

			Dim oLabelFolder As LabelTag = Me.GetLabelByPath(FolderName)
			If oLabelFolder Is Nothing Then
				Return Nothing
			End If
			Dim oRepository As UserRepository = GetFileByName(FileName, oLabelFolder, oVisibility)
			If oRepository Is Nothing Then
				Return Nothing
            Else
                Return lm.Comol.Core.File.ContentOf.File_ToByteArray(oRepository.UserFile.FileSystemPath & oRepository.UserFile.FileSystemName)
			End If
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

		Public Sub CreateLabelByPath(ByVal name As String, ByVal path As String)
			Dim oLabelFather As LabelTag = Me.GetLabelByPath(path)
			If Me.GetLabelByPath(CombinePath(path, name)).ID = 0 Then
				Dim oLabel As New LabelTag
				oLabel.DisplayName = name
				oLabel.CreatedAt = Now
				oLabel.CreatedBy = Me.CurrentUser
				oLabel.CommunityOwner = Me.CurrentCommunity
				If oLabelFather Is Nothing Then
					oLabel.FatherLabel = Nothing
				ElseIf oLabelFather.ID < 0 Then
					oLabel.FatherLabel = Nothing
				Else
					oLabel.FatherLabel = oLabelFather
				End If
				If Not IsNothing(MyBase.AddLabel(oLabel)) Then
					Me.Folders = ListLabelAvailableForUser(Me.CurrentUser, Me.CurrentCommunity, VisibilityStatus.All, True)
					'Me.Files = Me.ListFilesAvailableForUser(Me.CurrentUser, Me.CurrentCommunity)
				End If
			End If
		End Sub
		Public Sub DeleteLabelByPath(ByVal path As String)
			Dim oLabelTag As LabelTag = Me.GetLabelByPath(path)

			If oLabelTag.ID > 0 Then
				Me.DeleteLabel(oLabelTag)
			End If
		End Sub

		Private Sub DeleteLabel(ByVal oLabel As LabelTag)
			If oLabel.ID > 0 Then
				For Each oChildLabel As LabelTag In oLabel.Labels
					Me.DeleteLabel(oChildLabel)
				Next
				MyBase.RemoveLabel(oLabel)
			End If
		End Sub

		Public Sub CreateFile(ByVal oFile As LearningObjectFile, ByVal Path As String)
			Dim oFolderLabel As LabelTag = Me.GetLabelByPath(Path)
			If oFolderLabel Is Nothing Then
				FileHelpers.DeleteFile(oFile.FileSystemPath & oFile.FileSystemName)
			Else
				If oFolderLabel.isUserDefined Then
					Dim oExistingRepositoryItem As UserRepository = GetFileByName(oFile.CompleteName, oFolderLabel, VisibilityStatus.Visible)

					If MyBase.AddFileToUserRepository(oFile, oFolderLabel) Is Nothing Then
						FileHelpers.DeleteFile(oFile.FileSystemPath & oFile.FileSystemName)
					ElseIf Not oExistingRepositoryItem Is Nothing Then
						MyBase.RemoveFromRepository(oExistingRepositoryItem)
					End If
				End If
			End If
		End Sub
		Public Sub DeleteFileItem(ByVal FileName As String, ByVal path As String)
			Dim oFolderLabel As LabelTag = Me.GetLabelByPath(path)
			Dim oRepositoryItem As UserRepository = GetFileByName(FileName, oFolderLabel, VisibilityStatus.Visible)
			If oRepositoryItem Is Nothing Then
				Exit Sub
			Else
				MyBase.RemoveFromRepository(oRepositoryItem)
			End If
		End Sub
		Public Function GetAllDirectoryRows(ByVal path As String) As List(Of LabelTag)
			Dim oList As New List(Of LabelTag)
			Dim oRootLabel As LabelTag = GetItemFolder(path)
			If oRootLabel Is Nothing Then
				Return New List(Of LabelTag)
			End If
			oList.Add(oRootLabel)
			FillChildDirectoryRows(oRootLabel, oList)
			Return oList
		End Function
		Private Sub FillChildDirectoryRows(ByVal oLabel As LabelTag, ByVal toFill As List(Of LabelTag))
			Dim oChildList As List(Of LabelTag) = oLabel.Labels	' Me.Labels.FindAll(New GenericPredicate(Of LabelTag, Int64)(oLabel.ID, AddressOf LabelTag.FindByFather))

			For Each oChildLabel As LabelTag In oChildList
				toFill.Add(oChildLabel)
				FillChildDirectoryRows(oChildLabel, toFill)
			Next
		End Sub
		Public Function GetItemPath(ByVal oLabel As LabelTag) As String
			If oLabel.FatherLabel Is Nothing Then
				Return String.Format("{0}/", oLabel.DisplayName)
			Else
				Return GetItemPath(oLabel.FatherLabel) + String.Format("{0}/", oLabel.DisplayName)
			End If

		End Function
		Public Sub ReplaceItemContent(ByVal path As String, ByVal content As Byte())
			'Dim itemId As Integer = GetItemId(path)
			'If itemId < 0 Then
			'	Return
			'End If
			'Dim command As OleDbCommand = New OleDbCommand("UPDATE Items SET Content=@Content WHERE ItemID=@ItemID", Connection)
			'command.Parameters.Add(New OleDbParameter("@Content", content))
			'command.Parameters.Add(New OleDbParameter("@ItemID", itemId))
			'Connection.Open()
			'command.ExecuteNonQuery()
			'CloseConnection()
		End Sub


	
		
	End Class
End Namespace