
Public Class PresenterEditor
	Private _Manager As ManagerBrowsingRepository
	Private _View As IviewEditor

	Public Sub New(ByVal view As IviewEditor)
		_View = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewEditor
		Get
			View = _View
		End Get
	End Property

	Private ReadOnly Property CurrentManager() As ManagerBrowsingRepository
		Get
			If _Manager Is Nothing Then
				My.Resources.Culture = New System.Globalization.CultureInfo(Me.View.UserLanguage.Codice)
				_Manager = New ManagerBrowsingRepository(Me.View.CurrentUser, Me.View.CurrentCommunity, Me.View.UserLanguage, True, My.Resources.Resource.UserFolder, My.Resources.Resource.CommunityFolder)

			End If
			Return _Manager
		End Get
	End Property

	Public Sub Init(ByVal BaseUrl As String)
		Dim oFathers As List(Of LabelTag) = CurrentManager.Fathers
		Dim iResultStringPath(oFathers.Count - 1) As String
		Dim i As Integer

		For Each oFather As LabelTag In oFathers
			iResultStringPath(i) = "Repository/" & oFather.DisplayName
			i += 1
		Next
		Me.View.ImagesPaths = iResultStringPath
		Me.UpdateAdvancedTags(BaseUrl)
	End Sub

	Public Sub UpdateAdvancedTags(ByVal BaseUrl As String)
		If Me.View.ShowAddSmartTag Then
			Me.View.SetAdvancedTools(GetAdvancedTags(BaseUrl))
		Else
			Me.View.SetAdvancedTools(New List(Of SmartTag))
		End If

	End Sub
	Public Function GetFile(ByVal iId As String) As LearningObjectFile
		If iId = "" Then
			Return Nothing
		Else
			Return Me.CurrentManager.GetFileFromRepository(New Guid(iId))
		End If
	End Function
	Public Function GetContent(ByVal oFile As LearningObjectFile) As Byte()
		Try
			Return Me.CurrentManager.LoadFileToByteArray(oFile)
		Catch ex As Exception
			Return Nothing
		End Try
	End Function
	Private Function GetAdvancedTags(ByVal BaseUrl As String) As List(Of SmartTag)
		Dim oList As List(Of SmartTag) = ManagerConfiguration.GetSmartTags(BaseUrl).TagList

		If oList.Count = 0 Or Me.View.DisabledTags = "" Then
			Return oList
		Else
			Dim oReturnList As New List(Of SmartTag)
			Dim iDisabledTags() As String = Me.View.DisabledTags.Split(",")

			For Each oSmarTag As SmartTag In oList
				If Array.IndexOf(iDisabledTags, oSmarTag.Tag) = -1 Then
					oReturnList.Add(oSmarTag)
				End If
			Next
			Return oReturnList
		End If
	End Function

End Class