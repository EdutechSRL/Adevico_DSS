Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
	Public Class BaseFilePresenter
		Inherits DomainPresenter

		Public Overloads Property CurrentManager() As ManagerFiles
			Get
				Return _CurrentManager
			End Get
			Set(ByVal value As ManagerFiles)
				_CurrentManager = value
			End Set
		End Property
		Public Overloads ReadOnly Property View() As IviewBaseFileDownload
			Get
				Return MyBase.View
			End Get
		End Property

		Public Sub New(ByVal oContext As iApplicationContext)
			MyBase.New(oContext)
			'	MyBase.CurrentManager = New ManagerMyPersonalDiary(MyBase.AppContext)
		End Sub
		Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewBaseFileDownload)
			MyBase.New(oContext, view)
			MyBase.CurrentManager = New ManagerFiles(MyBase.AppContext)
		End Sub

		Public Sub ModuleDownloadFile(ByVal FileID As System.Guid, ByVal Path As String)
			Dim oFile As BaseFile = GetFile(FileID)
			If Not IsNothing(oFile) Then
				Me.View.DownloadFile(Path & oFile.FileSystemName, oFile)
			End If
		End Sub
		'Public Sub ModuleDownloadFile(ByVal FileID As System.Guid, ByVal Path As String)
		'	Dim oFile As BaseFile = GetFile(FileID)
		'	If Not IsNothing(oFile) Then
		'		Me.View.WriteFile(GetContent(Path & oFile.DisplayName), oFile.DisplayName, oFile.ContentType)
		'	End If
		'End Sub

		Public Function GetFile(ByVal FileID As System.Guid) As BaseFile
			Return Me.CurrentManager.GetBaseFile(FileID)
		End Function
		Public Function GetContent(ByVal FileNameAndPath As String) As Byte()
            Return lm.Comol.Core.File.ContentOf.File_ToByteArray(FileNameAndPath)
		End Function


        'Private Function LoadFileToByteArray(ByVal FileName As String) As Byte()
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


		'	Public Sub InitViewDiary(ByVal DiaryID As System.Guid)
		'		Dim oDiary As PersonalDiary = Nothing

		'		If DiaryID = System.Guid.Empty Then
		'			oDiary = Me.CurrentManager.GetCommunityDiary(Me.AppContext.UserContext.CurrentUser.Id, Me.AppContext.UserContext.CommunityID)
		'		Else
		'			oDiary = Me.CurrentManager.GetDiary(DiaryID)
		'		End If

		'		If oDiary Is Nothing Then
		'			Me.View.ChangeDiary(oDiary)
		'		Else
		'			Me.View.CurrentDiaryID = oDiary.Id
		'			Me.View.LoadItems(Me.GetDiaryItems(oDiary.Id, Me.View.OrderAscending))
		'		End If
		'	End Sub
		'	Public Sub LoadDiarys()

		'	End Sub
		'	Public Function SaveMyDiary(ByVal oDiary As PersonalDiary) As PersonalDiary
		'		Return Me.CurrentManager.SaveUserDiary(Me.UserContext.CurrentUser.Id, Me.UserContext.CurrentUser.Id, Me.UserContext.CommunityID, oDiary)
		'	End Function

		'	Public Sub LoadDiaryItems(ByVal DiaryID As System.Guid)
		'		Me.View.LoadItems(Me.GetDiaryItems(DiaryID, Me.View.OrderAscending))
		'	End Sub
		'	Public Function GetDiaryItems(ByVal DiaryID As System.Guid, ByVal OrderAscending As Boolean) As IList(Of iDiaryItem)
		'		If DiaryID = System.Guid.Empty Then
		'			Dim oDiary As PersonalDiary = Nothing
		'			oDiary = Me.CurrentManager.GetCommunityDiary(Me.AppContext.UserContext.CurrentUser.Id, Me.AppContext.UserContext.CommunityID)
		'			If Not IsNothing(oDiary) Then
		'				DiaryID = oDiary.Id
		'			End If
		'		End If
		'		Return Me.CurrentManager.GetDiaryItems(DiaryID, OrderAscending)
		'	End Function
		'	Public Function GetDiaryItem(ByVal ItemID As System.Guid) As iDiaryItem
		'		Return Me.CurrentManager.GetDiaryItem(ItemID)
		'	End Function

		'	Public Sub SaveMyItem(ByVal DiaryID As System.Guid, ByVal oItem As DiaryItem)
		'		Dim oSavedItem As DiaryItem = Me.CurrentManager.SaveMyItem(Me.UserContext.CurrentUser.Id, Me.UserContext.CurrentUser.Id, Me.UserContext.CommunityID, DiaryID, oItem)
		'		If IsNothing(oSavedItem) Then
		'			Me.View.ShowError(My.Resources.ModuleBaseResource.ItemGenericPersistError)
		'		Else
		'			Me.LoadDiaryItems(DiaryID)
		'		End If
		'	End Sub
		'	Public Sub EditItem(ByVal ItemID As System.Guid)
		'		Dim oItem As DiaryItem = Me.CurrentManager.GetDiaryItem(ItemID)
		'		If IsNothing(oItem) Then
		'			Me.View.ShowError(My.Resources.ModuleBaseResource.ItemNotFound)
		'		Else
		'			Me.View.LoadItem(oItem)
		'		End If
		'	End Sub
		'	Public Sub VirtualDeleteItem(ByVal ItemID As System.Guid)
		'		Dim oItem As DiaryItem = Me.CurrentManager.DeleteVirtualDiaryItem(ItemID)
		'		If IsNothing(oItem) Then
		'			Me.View.ShowError(My.Resources.ModuleBaseResource.ItemNotFound)
		'		Else
        '			Me.LoadDiaryItems(oItem.WorkBookOwner.Id)
        '		End If
        '	End Sub
        '	Public Sub VirtualUnDeleteItem(ByVal ItemID As System.Guid)
        '		Dim oItem As DiaryItem = Me.CurrentManager.UnDeleteVirtualDiaryItem(ItemID)
        '		If IsNothing(oItem) Then
        '			Me.View.ShowError(My.Resources.ModuleBaseResource.ItemNotFound)
        '		Else
        '			Me.LoadDiaryItems(oItem.WorkBookOwner.Id)
		'		End If
		'	End Sub
	End Class
End Namespace