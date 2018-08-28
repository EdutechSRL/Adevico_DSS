Namespace File
	<Serializable(), CLSCompliant(True)> Public Class LearningObjectFile
		Inherits DomainObject


#Region "Private Property"
		Private _ID As Guid
		Private _Name As String
		Private _Description As String
		Private _FileSystemName As String
		Private _FileSystemPath As String
		Private _Size As Int64
		Private _ContentType As MimeType
		Private _HardLinks As Int64
		Private _Extension As String
#End Region

#Region "Public Property"
		Public Property ID() As Guid
			Get
				Return _ID
			End Get
			Set(ByVal value As Guid)
				_ID = value
			End Set
		End Property
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				_Name = value
			End Set
		End Property
		Public ReadOnly Property CompleteName() As String
			Get
				Return _Name & Me._Extension
			End Get

		End Property
		Public Property Description() As String
			Get
				Return _Description
			End Get
			Set(ByVal value As String)
				_Description = value
			End Set
		End Property
		Public Property FileSystemName() As String
			Get
				Return _FileSystemName
			End Get
			Set(ByVal value As String)
				_FileSystemName = value
			End Set
		End Property
		Public Property FileSystemPath() As String
			Get
				Return _FileSystemPath
			End Get
			Set(ByVal value As String)
				_FileSystemPath = value
			End Set
		End Property
		Public Property Size() As Int64
			Get
				Return _Size
			End Get
			Set(ByVal value As Int64)
				_Size = value
			End Set
		End Property
		Public Property ContentType() As MimeType
			Get
				Return _ContentType
			End Get
			Set(ByVal value As MimeType)
				_ContentType = value
			End Set
		End Property
		Public Property HardLinks() As Int64
			Get
				Return _HardLinks
			End Get
			Set(ByVal value As Int64)
				_HardLinks = value
			End Set
		End Property
		Public Property Extension() As String
			Get
				Return _Extension
			End Get
			Set(ByVal value As String)
				_Extension = value
			End Set
		End Property
#End Region

		Sub New()
			MyBase.IsDeleted = False
		End Sub

		Public Shared Function FindByID(ByVal item As LearningObjectFile, ByVal argument As Guid) As Boolean
			Return item.ID = argument
		End Function
		Public Shared Function FindByCreator(ByVal item As LearningObjectFile, ByVal argument As Person) As Boolean
			Return item.CreatedBy.ID = argument.ID
		End Function
		Public Shared Function FindByCreator(ByVal item As LearningObjectFile, ByVal argument As Integer) As Boolean
			Return item.CreatedBy.ID = argument
		End Function
		Public Shared Function FindByName(ByVal item As LearningObjectFile, ByVal argument As String) As Boolean
			Return item.Name = argument
		End Function
		Public Shared Function FindByVisibility(ByVal item As LearningObjectFile, ByVal argument As VisibilityStatus) As Boolean
			Select Case argument
				Case VisibilityStatus.All
					Return True
					'Case VisibilityStatus.NotDeleted Or VisibilityStatus.Visible
					'	case 
					'Case VisibilityStatus.NotDeleted
					'	argument = VisibilityStatus.OnlyDeleted Or VisibilityStatus.OnlyVisible
					'Case VisibilityStatus.OnlyDeleted
					'Case VisibilityStatus.OnlyVisible
			End Select
		End Function
	End Class
End Namespace