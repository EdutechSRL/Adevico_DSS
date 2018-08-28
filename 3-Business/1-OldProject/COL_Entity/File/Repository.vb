Namespace File
	<Serializable(), CLSCompliant(True)> Public Class Repository
		Inherits DomainObject

		Private _ID As Guid
		Private _File As LearningObjectFile
		Private _Person As Person
		Private _Label As List(Of LabelTag)

		Public Property ID() As Guid
			Get
				Return _ID
			End Get
			Set(ByVal value As Guid)
				_ID = value
			End Set
		End Property
		Public Property UserFile() As LearningObjectFile
			Get
				Return _File
			End Get
			Set(ByVal value As LearningObjectFile)
				_File = value
			End Set
		End Property
		Public Property Labels() As List(Of LabelTag)
			Get
				Return _Label
			End Get
			Set(ByVal value As List(Of LabelTag))
				_Label = value
			End Set
		End Property

		Sub New()
			Me._Label = New List(Of LabelTag)
		End Sub
		Public Shared Function FindByUserID(ByVal item As UserRepository, ByVal argument As Person) As Boolean
			Return item.CreatedBy.ID = argument.ID
		End Function
		Public Shared Function FindByLabel(ByVal item As UserRepository, ByVal argument As LabelTag) As Boolean
			If argument.ID < 0 And item.Labels.Count = 0 Then
				Return True
			Else
				Return Not IsNothing(item.Labels.Find(New GenericPredicate(Of LabelTag, Int64)(argument.ID, AddressOf LabelTag.FindByID)))
			End If
		End Function
		'Public Shared Function FindByLabels(ByVal item As UserRepository, ByVal argument As List(Of LabelTag), ByVal AndClause As Boolean) As Boolean
		'	Dim numLabelFound As Integer = 0

		'	For Each oLabel As LabelTag In argument
		'		If Not AndClause Then
		'			If Not IsNothing(item.Labels.Find(New GenericPredicate(Of LabelTag, Int64)(oLabel.ID, AddressOf LabelTag.FindByID))) Then
		'				Return True
		'			End If
		'		ElseIf Not IsNothing(item.Labels.Find(New GenericPredicate(Of LabelTag, Int64)(oLabel.ID, AddressOf LabelTag.FindByID))) Then
		'			numLabelFound += 1
		'		End If
		'	Next
		'	If AndClause And numLabelFound = argument.Count Then
		'		Return True
		'	End If
		'	Return False
		'End Function
		Public Shared Function FindByFileName(ByVal item As UserRepository, ByVal argument As String) As Boolean
			Return item.UserFile.CompleteName = argument
		End Function

		Public Shared Function FindByVisibility(ByVal item As UserRepository, ByVal argument As VisibilityStatus) As Boolean
			Select Case argument
				Case VisibilityStatus.All
					Return True
				Case VisibilityStatus.Visible
					Return item.Isvisible And Not item.IsDeleted
				Case VisibilityStatus.Deleted
					Return item.IsDeleted
				Case VisibilityStatus.NotDeleted
					Return Not item.IsDeleted
			End Select
		End Function
	End Class
End Namespace