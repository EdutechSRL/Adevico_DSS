Namespace File
	<Serializable(), CLSCompliant(True)> Public Class LabelTag
		Inherits DomainObject

#Region "Private Property"
		Private _ID As Int64
		Private _Community As Community
		Private _DisplayName As String
		Private _Description As String
		Private _Father As LabelTag
		Private _ChildLabel As List(Of LabelTag)
		Private _HardLinks As Int64
		Private _isSystem As Boolean
#End Region

#Region "Public Property"
		Public Property ID() As Int64
			Get
				Return _ID
			End Get
			Set(ByVal value As Int64)
				_ID = value
			End Set
		End Property
		Public Property CommunityOwner() As Community
			Get
				Return _Community
			End Get
			Set(ByVal value As Community)
				_Community = value
			End Set
		End Property
		Public Property DisplayName() As String
			Get
				Return _DisplayName
			End Get
			Set(ByVal value As String)
				_DisplayName = value
			End Set
		End Property
		Public Property Description() As String
			Get
				Return _Description
			End Get
			Set(ByVal value As String)
				_Description = value
			End Set
		End Property
		Public Property FatherLabel() As LabelTag
			Get
				Return _Father
			End Get
			Set(ByVal value As LabelTag)
				_Father = value
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
		Public Property Labels() As List(Of LabelTag)
			Get
				Return _ChildLabel
			End Get
			Set(ByVal value As List(Of LabelTag))
				_ChildLabel = value
			End Set
		End Property
		Public Property isSystemDefined() As Boolean
			Get
				Return _isSystem
			End Get
			Set(ByVal value As Boolean)
				_isSystem = value
			End Set
		End Property
		Public ReadOnly Property isUserDefined() As Boolean
			Get
				If _Community Is Nothing Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property
		Public ReadOnly Property Path() As String
			Get
				If _Father Is Nothing Then
					Return _DisplayName & "/"
				Else
					Return _Father.Path & _DisplayName & "/"
				End If
			End Get
		End Property
#End Region

		Sub New()
			MyBase.IsDeleted = False
			Me._isSystem = False
			Me._ChildLabel = New List(Of LabelTag)
		End Sub
		Sub New(ByVal iID As Int64)
			MyBase.IsDeleted = False
			Me._isSystem = False
			Me._ChildLabel = New List(Of LabelTag)
			Me._ID = iID
		End Sub
		Sub New(ByVal iID As Int64, ByVal iDisplayName As String, ByVal iFather As LabelTag)
			MyBase.IsDeleted = False
			Me._isSystem = False
			Me._ChildLabel = New List(Of LabelTag)
			Me._ID = iID
			Me._DisplayName = iDisplayName
			Me._Father = iFather
		End Sub
		Sub New(ByVal iID As Int64, ByVal iDisplayName As String, ByVal iFather As LabelTag, ByVal iCommunity As Community)
			MyBase.IsDeleted = False
			Me._isSystem = False
			Me._ChildLabel = New List(Of LabelTag)
			Me._ID = iID
			Me._DisplayName = iDisplayName
			Me._Father = iFather
			Me._Community = iCommunity
		End Sub
		Public Shared Function FindByID(ByVal item As LabelTag, ByVal argument As Int64) As Boolean
			Return item.ID = argument
		End Function
		Public Shared Function FindByCreator(ByVal item As LabelTag, ByVal argument As Person) As Boolean
			Return item.CreatedBy.ID = argument.ID
		End Function
		Public Shared Function FindByCreator(ByVal item As LabelTag, ByVal argument As Integer) As Boolean
			Return item.CreatedBy.ID = argument
		End Function
		Public Shared Function FindByCommunity(ByVal item As LabelTag, ByVal argument As Community) As Boolean
			If IsNothing(item.CommunityOwner) Then
				Return False
			Else
				Return item.CommunityOwner.ID = argument.ID
			End If
		End Function
		Public Shared Function FindBySystem(ByVal item As LabelTag, ByVal argument As Boolean) As Boolean
			Return item.isSystemDefined
		End Function
		Public Shared Function FindByNofather(ByVal item As LabelTag, ByVal argument As Boolean) As Boolean
			Return (item.FatherLabel Is Nothing) = argument
		End Function
		Public Shared Function FindByFather(ByVal item As LabelTag, ByVal argument As Int64) As Boolean
			If argument = 0 And item.FatherLabel Is Nothing Then
				Return True
			ElseIf item.FatherLabel Is Nothing Then
				Return False
			ElseIf argument = 0 And item.FatherLabel.ID < 0 Then
				Return True
			Else
				Return item.FatherLabel.ID = argument
			End If
		End Function
		Public Shared Function FindMainFather(ByVal item As LabelTag, ByVal argument As Boolean) As Boolean
			Return item.FatherLabel Is Nothing
		End Function
		Public Shared Function FindByName(ByVal item As LabelTag, ByVal argument As String) As Boolean
			Return item.DisplayName = argument
		End Function
		Public Shared Function FindPersonal(ByVal item As LabelTag, ByVal argument As Boolean) As Boolean
			Return item.isUserDefined
		End Function

		Public Shared Function FindByVisibility(ByVal item As LabelTag, ByVal argument As VisibilityStatus) As Boolean
			Select Case argument
				Case VisibilityStatus.All
					Return True
				Case VisibilityStatus.Deleted
					Return item.IsDeleted
				Case VisibilityStatus.NotDeleted
					Return Not item.IsDeleted
				Case VisibilityStatus.Visible
					Return item.Isvisible And item.IsDeleted = False
			End Select
		End Function
		Public Shared Function FindByPathStart(ByVal item As LabelTag, ByVal argument As String) As Boolean
			Return item.Path.StartsWith(argument)
		End Function
	End Class
End Namespace