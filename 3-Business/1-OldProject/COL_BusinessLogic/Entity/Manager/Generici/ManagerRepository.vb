Imports Comol.Entity
Imports Comol.Entity.File

Namespace Comol.Manager
	Public Class ManagerRepository
		Inherits ObjectBase
		Implements iManagerAdvanced

#Region "Private property"
		Private _UseCache As Boolean
		Private _CurrentUser As Person
		Private _CurrentCommunity As Community
		Private _CurrentDB As ConnectionDB
		Private _LabelList As List(Of LabelTag)
		Private _UserFiles As List(Of UserRepository)
		Private _Language As Lingua
		Private _UserFolderName As String
		Private _CommunityFolderName As String
		Private _StorePath As String
#End Region

#Region "Public property"
		Public ReadOnly Property CurrentCommunity() As Community Implements iManagerAdvanced.CurrentCommunity
			Get
				Return _CurrentCommunity
			End Get
		End Property
		Public ReadOnly Property CurrentUser() As Person Implements iManagerAdvanced.CurrentUser
			Get
				Return _CurrentUser
			End Get
		End Property
		Private ReadOnly Property Language() As Lingua Implements iManagerAdvanced.Language
			Get
				Return _Language
			End Get
		End Property
		Private ReadOnly Property UseCache() As Boolean Implements iManagerAdvanced.UseCache
			Get
				Return _UseCache
			End Get
		End Property
		Private ReadOnly Property CurrentDB() As ConnectionDB Implements iManagerAdvanced.CurrentDB
			Get
				If IsNothing(_CurrentDB) Then
					_CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
				End If
				Return _CurrentDB
			End Get
		End Property
		Public ReadOnly Property StorePath() As String
			Get
				If String.IsNullOrEmpty(_StorePath) Then
					_StorePath = ObjectFilePath.CreateByConfigPath(ManagerConfiguration.GetInstance.File.Repository, "", "").Drive
				End If
				Return _StorePath
			End Get
		End Property


		Public Property Files(Optional ByVal oVisibility As VisibilityStatus = VisibilityStatus.All) As List(Of UserRepository)
			Get
				If IsNothing(_UserFiles) Then
					_UserFiles = New List(Of UserRepository)
					_UserFiles = Me.ListFilesAvailableForUser(Me.CurrentUser, Me.CurrentCommunity)
				End If

				If oVisibility = VisibilityStatus.All Then
					Return _UserFiles
				Else
					Return _UserFiles.FindAll(New GenericPredicate(Of UserRepository, VisibilityStatus)(oVisibility, AddressOf UserRepository.FindByVisibility))
				End If
			End Get
			Set(ByVal value As List(Of UserRepository))
				_UserFiles = value
			End Set
		End Property
#End Region

		Public Sub New(ByVal oPersona As Person, ByVal oCommunity As Community, ByVal oLanguage As Lingua, Optional ByVal UseCache As Boolean = True, Optional ByVal UserFolderName As String = "", Optional ByVal CommunityFolderName As String = "")
			Me._UseCache = UseCache
			Me._CurrentUser = oPersona
			Me._CurrentCommunity = oCommunity
			Me._Language = oLanguage
			Me._UserFolderName = UserFolderName
			Me._CommunityFolderName = CommunityFolderName
		End Sub

#Region "Label Manager"
		Public Function ListLabel(ByVal oPerson As Person, Optional ByVal asTree As Boolean = False, Optional ByVal sortExpression As String = "", Optional ByVal oSortDirection As sortDirection = sortDirection.None, Optional ByVal ForceRetrieve As Boolean = False) As System.Collections.IList
			Dim oLabelList As New List(Of LabelTag)
			If Me._UseCache Then
				Dim cacheKey As String = CachePolicyRepository.LabelUser(oPerson.ID)
				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
					oLabelList = GetUserLabels(oPerson, asTree)
					ObjectBase.Cache.Insert(cacheKey, oLabelList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
				Else
					oLabelList = CType(ObjectBase.Cache(cacheKey), List(Of LabelTag))
				End If
			Else
				oLabelList = GetUserLabels(oPerson, asTree)
			End If

			If (Not sortExpression Is Nothing AndAlso oSortDirection <> sortDirection.None) Then
				oLabelList.Sort(New GenericComparer(Of LabelTag)(sortExpression))
			End If

			If (oSortDirection = sortDirection.Descending) Then
				oLabelList.Reverse()
			End If
			Return oLabelList
		End Function
		'Public Function ListLabel(ByVal oPerson As Person, ByVal oCommunity As Community, Optional ByVal asTree As Boolean = False, Optional ByVal sortExpression As String = "", Optional ByVal oSortDirection As sortDirection = sortDirection.None, Optional ByVal ForceRetrieve As Boolean = False) As System.Collections.IList
		'	Dim oLabelList As New List(Of LabelTag)
		'	If Me._UseCache Then
		'		Dim cacheKey As String = CachePolicyRepository.LabelCommunity(oCommunity.ID)
		'		If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'			oLabelList = GetCommunityLabels(oPerson, oCommunity)
		'			ObjectBase.Cache.Insert(cacheKey, oLabelList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
		'		Else
		'			oLabelList = CType(ObjectBase.Cache(cacheKey), List(Of LabelTag))
		'		End If
		'	Else
		'		oLabelList = GetCommunityLabels(oPerson, oCommunity)
		'	End If

		'	If (Not sortExpression Is Nothing AndAlso oSortDirection <> sortDirection.None) Then
		'		oLabelList.Sort(New GenericComparer(Of LabelTag)(sortExpression))
		'	End If

		'	If (oSortDirection = sortDirection.Descending) Then
		'		oLabelList.Reverse()
		'	End If
		'	Return oLabelList
		'End Function

		Public Function ListLabelAvailableForUser(ByVal oPerson As Person, ByVal oCommunity As Community, Optional ByVal oVisibility As VisibilityStatus = VisibilityStatus.All, Optional ByVal asTree As Boolean = False, Optional ByVal sortExpression As String = "", Optional ByVal oSortDirection As sortDirection = sortDirection.None, Optional ByVal ForceRetrieve As Boolean = False) As List(Of LabelTag)
			Dim oPersonalList As New List(Of LabelTag)
			Dim oList As New List(Of LabelTag)
			'Dim oCommunityList As New List(Of LabelTag)


			'Dim oCommunityLabel As New LabelTag(-2, "Communita", Nothing, oCommunity)

			oPersonalList = Me.ListLabel(oPerson, asTree, sortExpression, oSortDirection, ForceRetrieve)
			If oPersonalList.Count > 0 Then
				oList.AddRange(oPersonalList)
			End If

			' DA AGGIUNGERE SE VOGLIO I FOLDER DI COMUNITA --> L3 !!
			'If Not oCommunity Is Nothing Then
			'	oCommunityList = Me.ListLabel(oPerson, oCommunity, asTree, sortExpression, oSortDirection, ForceRetrieve)
			'End If
			

			Dim AddLabel As Boolean = False
			''If oPersonalList.Count > 0 Then
			''	Dim iSearchList As List(Of LabelTag)
			''	iSearchList = oPersonalList.FindAll(New GenericPredicate(Of LabelTag, Boolean)(True, AddressOf LabelTag.FindByNofather))
			''	For Each oSearchedLabel As LabelTag In iSearchList
			''		If oSearchedLabel.ID <> oPersonal.ID Then
			''			oSearchedLabel.FatherLabel = oPersonal
			''			AddLabel = True
			''		End If
			''	Next
			''	oPersonal.Labels = iSearchList
			''	oList.Insert(0, oPersonal)
			''Else
			''	oList.Insert(0, oPersonal)
			''End If

			'If oCommunityList.Count > 0 Then
			'	If Not asTree Then : oList.AddRange(oCommunityList)

			'	End If
			'	AddLabel = False
			'	Dim iSearchList As List(Of LabelTag)
			'	iSearchList = oCommunityList.FindAll(New GenericPredicate(Of LabelTag, Boolean)(True, AddressOf LabelTag.FindByNofather))
			'	For Each oSearchedLabel As LabelTag In iSearchList
			'		If oSearchedLabel.ID <> oCommunityLabel.ID And oSearchedLabel.isUserDefined = False Then
			'			oSearchedLabel.FatherLabel = oCommunityLabel
			'			AddLabel = True
			'		End If
			'	Next
			'	oCommunityLabel.Labels = iSearchList
			'End If

			If oVisibility <> VisibilityStatus.All Then
				oList = oList.FindAll(New GenericPredicate(Of LabelTag, VisibilityStatus)(oVisibility, AddressOf LabelTag.FindByVisibility))
			End If

			'If Not oCommunity Is Nothing Then
			'	oList.Insert(0, oCommunityLabel)
			'End If
			If (Not sortExpression Is Nothing AndAlso oSortDirection <> sortDirection.None) Then
				oList.Sort(New GenericComparer(Of LabelTag)(sortExpression))
			End If

			If (oSortDirection = sortDirection.Descending) Then
				oList.Reverse()
			End If
			Return oList
		End Function

		Private Function GetUserLabels(ByVal oPerson As Person, ByVal asFolders As Boolean) As List(Of LabelTag)
			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)
			Dim oLabelList As List(Of LabelTag)

			If asFolders Then
				oLabelList = oDal.GetUserLabelsAsFolder(oPerson, Nothing)
			Else
				oLabelList = oDal.GetUserLabels(oPerson)
				'	Return LabelTreeConfiguration(oLabelList)
			End If

			Dim AddLabel As Boolean = False
			Dim oPersonal As New LabelTag(-1, Me._UserFolderName, Nothing)
			Dim oReturnList As New List(Of LabelTag)
			If Not asFolders Then : oReturnList.AddRange(oLabelList)

			End If
			If oLabelList.Count > 0 Then
				Dim iSearchList As List(Of LabelTag)
				iSearchList = oLabelList.FindAll(New GenericPredicate(Of LabelTag, Boolean)(True, AddressOf LabelTag.FindByNofather))
				For Each oSearchedLabel As LabelTag In iSearchList
					If oSearchedLabel.ID <> oPersonal.ID Then
						oSearchedLabel.FatherLabel = oPersonal
						AddLabel = True
					End If
				Next
				oPersonal.Labels = iSearchList
				oReturnList.Insert(0, oPersonal)
			Else
				oReturnList.Insert(0, oPersonal)
			End If
			Return oReturnList
		End Function
		Private Function GetCommunityLabels(ByVal oPerson As Person, ByVal oCommunity As Community) As List(Of LabelTag)
			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)
			Dim oLabelList As List(Of LabelTag) = oDal.GetCommunityLabels(oPerson, oCommunity)

			Return oLabelList 'LabelTreeConfiguration(oLabelList)
		End Function
		'Private Function LabelTreeConfiguration(ByVal oList As List(Of LabelTag)) As List(Of LabelTag)
		'	For Each oLabel As LabelTag In oList
		'		If Not (oLabel.FatherLabel Is Nothing) AndAlso oLabel.FatherLabel.DisplayName = "" Then
		'			Dim oFatherLabel As LabelTag = oList.Find(New GenericPredicate(Of LabelTag, Int64)(oLabel.FatherLabel.ID, AddressOf LabelTag.FindByID))
		'			If Not oFatherLabel Is Nothing Then
		'				oLabel.FatherLabel = oFatherLabel
		'			End If
		'		End If
		'	Next
		'	Return oList
		'End Function

		Public Function AddLabel(ByVal oLabel As LabelTag) As LabelTag
			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)
			Dim oLabelNew As LabelTag = oDal.AddLabel(oLabel)
			If Not oLabelNew Is Nothing Then
				If oLabelNew.isUserDefined Then
					ObjectBase.PurgeCacheItems(CachePolicyRepository.LabelUser(oLabelNew.CreatedBy.ID))
				Else
					ObjectBase.PurgeCacheItems(CachePolicyRepository.LabelCommunity(oLabelNew.CommunityOwner.ID))
				End If
			End If
			Return oLabelNew
		End Function

		Public Function RemoveLabel(ByVal oLabel As LabelTag, Optional ByVal ClearCache As Boolean = False) As Boolean
			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)

			If oDal.RemoveLabel(oLabel, Me.CurrentUser) Then
				Me.Files = Nothing
				If ClearCache Then
					Cache.Remove(IIf(oLabel.isUserDefined, CachePolicyRepository.LabelUser(oLabel.CreatedBy.ID), CachePolicyRepository.LabelCommunity(oLabel.CommunityOwner.ID)))
					Cache.Remove(IIf(oLabel.isUserDefined, CachePolicyRepository.FileUser(oLabel.CreatedBy.ID), CachePolicyRepository.FileCommunity(oLabel.CommunityOwner.ID)))
				End If
				Return True
			Else
				Return False
			End If
		End Function
#End Region

#Region "File Manager"
		Public Function ListFile(ByVal oPerson As Person, Optional ByVal sortExpression As String = "", Optional ByVal oSortDirection As sortDirection = sortDirection.None, Optional ByVal ForceRetrieve As Boolean = False) As System.Collections.IList
			Dim oList As New List(Of UserRepository)
			If Me._UseCache Then
				Dim cacheKey As String = CachePolicyRepository.FileUser(oPerson.ID)
				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
					oList = GetUserFiles(oPerson)
					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
				Else
					oList = CType(ObjectBase.Cache(cacheKey), List(Of UserRepository))
				End If
			Else
				oList = GetUserFiles(oPerson)
			End If

			If (Not sortExpression Is Nothing AndAlso oSortDirection <> sortDirection.None) Then
				oList.Sort(New GenericComparer(Of UserRepository)(sortExpression))
			End If

			If (oSortDirection = sortDirection.Descending) Then
				oList.Reverse()
			End If
			Return oList
		End Function

		Public Function ListFilesAvailableForUser(ByVal oPerson As Person, ByVal oCommunity As Community, Optional ByVal sortExpression As String = "", Optional ByVal oSortDirection As sortDirection = sortDirection.None, Optional ByVal ForceRetrieve As Boolean = False) As List(Of UserRepository)
			Dim oList As New List(Of UserRepository)
			oList = Me.ListFile(oPerson, sortExpression, oSortDirection, ForceRetrieve)

			If (Not sortExpression Is Nothing AndAlso oSortDirection <> sortDirection.None) Then
				oList.Sort(New GenericComparer(Of UserRepository)(sortExpression))
			End If

			If (oSortDirection = sortDirection.Descending) Then
				oList.Reverse()
			End If
			Return oList
		End Function
		Private Function GetUserFiles(ByVal oPerson As Person) As List(Of UserRepository)
			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)
			Return oDal.GetUserFiles(oPerson)
		End Function
		Public Function GetFileByName(ByVal iName As String, ByVal oLabel As LabelTag, ByVal oVisibility As VisibilityStatus) As UserRepository
			Dim oFileList As List(Of UserRepository) = Me.Files(VisibilityStatus.Visible).FindAll(New GenericPredicate(Of UserRepository, LabelTag)(oLabel, AddressOf UserRepository.FindByLabel))
			If oFileList.Count > 0 Then
				oFileList = oFileList.FindAll(New GenericPredicate(Of UserRepository, VisibilityStatus)(oVisibility, AddressOf UserRepository.FindByVisibility))
				Return oFileList.Find(New GenericPredicate(Of UserRepository, String)(iName, AddressOf UserRepository.FindByFileName))
			Else
				Return Nothing
			End If
		End Function
		Public Function GetFileFromRepository(ByVal iId As Guid) As LearningObjectFile
			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)

			If iId = Guid.Empty Then
				Return Nothing
			Else
				Return oDal.GetFileData(iId)
			End If
		End Function
		Public Function AddFileToUserRepository(ByVal oFile As LearningObjectFile, ByVal oLabel As LabelTag) As UserRepository
			Dim oUserRepository As New UserRepository

			oUserRepository.UserFile = oFile
			oUserRepository.CreatedBy = Me.CurrentUser
			oUserRepository.CreatedAt = oFile.CreatedAt
			If oLabel.ID > 0 Then
				oUserRepository.Labels.Add(oLabel)
			End If

			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)
			oUserRepository = oDal.AddFileToUserRepository(oUserRepository)
			If oUserRepository Is Nothing Then
				FileHelpers.DeleteFile(oFile.FileSystemPath & oFile.FileSystemName)
				Return Nothing
			Else
				Me.Files = Nothing
				Cache.Remove(CachePolicyRepository.FileUser(Me.CurrentUser.ID))
				Return oUserRepository
			End If
		End Function

		Public Sub RemoveFromRepository(ByVal oRepository As UserRepository)
			Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)

			If TypeOf oRepository Is UserRepository Then
				oDal.RemoveFromUserRepository(oRepository, Me._CurrentUser)
				Me.Files = Nothing
				Cache.Remove(CachePolicyRepository.FileUser(Me.CurrentUser.ID))
			End If

		End Sub
		'Public Function AddFileToUserRepository(ByVal oFile As LearningObjectFile, ByVal oLabels As List(Of LabelTag)) As UserRepository
		'	Dim oUserRepository As New UserRepository

		'	oUserRepository.UserFile = oFile
		'	oUserRepository.CreatedBy = Me.CurrentUser
		'	oUserRepository.CreatedAt = oFile.CreatedAt
		'	If oLabels.Count > 0 Then
		'		oUserRepository.Labels.AddRange(oLabels)
		'	End If

		'	Dim oDal As New DAL.StandardDB.DALrepository(Me.CurrentDB)
		'	Return oDal.AddFileToUserRepository(oUserRepository)
		'End Function


        Public Function LoadFileToByteArray(ByVal oFile As LearningObjectFile) As Byte()
            Return lm.Comol.Core.File.ContentOf.File_ToByteArray(Me.StorePath & oFile.ID.ToString & ".stored")
        End Function

        'Public Function LoadFileToByteArray(ByVal FileName As String) As Byte()
        '	Dim oFile As System.IO.FileInfo
        '	oFile = New System.IO.FileInfo(FileName)

        '	Dim oFileStream As System.IO.FileStream = oFile.OpenRead()
        '	Dim lBytes As Long = oFileStream.Length

        '	Dim fileData(lBytes) As Byte

        '	' Read the file into a byte array
        '	oFileStream.Read(fileData, 0, lBytes)
        '	oFileStream.Close()
        '	Return fileData
        'End Function

		'Public Function GetFileByName(ByVal iName As String, ByVal oList As List(Of LabelTag)) As UserRepository
		'	Dim oFileList As List(Of UserRepository) = Me.Files.FindAll(New GenericPredicate(Of UserRepository, List(Of LabelTag), Boolean)(oList, AddressOf UserRepository.FindByLabels))
		'	If oFileList.Count > 0 Then
		'		Return oFileList.Find(New GenericPredicate(Of UserRepository, String)(iName, AddressOf UserRepository.FindByFileName))

		'	Else
		'		Return Nothing
		'	End If
		'End Function

		'Public Function GetFileContent(ByVal iID As Guid) As LearningObjectFile
		'	Dim oFileRepository As LearningObjectFile

		'	Return oFileRepository
		'End Function
#End Region

	End Class
End Namespace