Namespace Comol.Materiale.Scorm
	Public Class SCORM_Organization

#Region "Private Property"
		Private _ID As String
		Private _Title As String
		Private _Items As List(Of SCORM_Item)
		Private _StructureType As String
		Private _isDefault As String
		Private _isVisible As Boolean
		Private _isValid As Boolean
#End Region

#Region "Public Property"
		Public Property ID() As String
			Get
				Return _ID
			End Get
			Set(ByVal value As String)
				_ID = value
			End Set
		End Property
		Public Property Title() As String
			Get
				Return _Title
			End Get
			Set(ByVal value As String)
				_Title = value
			End Set
		End Property
		Public Property Items() As List(Of SCORM_Item)
			Get
				Items = _Items
			End Get
			Set(ByVal value As List(Of SCORM_Item))
				_Items = value
			End Set
		End Property
		Public Property StructureType() As String
			Get
				Return _StructureType
			End Get
			Set(ByVal value As String)
				_StructureType = value
			End Set
		End Property
		Public Property isVisible() As Boolean
			Get
				isVisible = _isVisible
			End Get
			Set(ByVal value As Boolean)
				_isVisible = value
			End Set
		End Property
		Public Property isDefault() As Boolean
			Get
				Return _isDefault
			End Get
			Set(ByVal value As Boolean)
				_isDefault = value
			End Set
		End Property
		Public Property IsValid() As Boolean
			Get
				Return _isValid
			End Get
			Set(ByVal value As Boolean)
				_isValid = value
			End Set
		End Property
#End Region

		Sub New()
			Me._isVisible = True
			Me._StructureType = ""
			Me._isDefault = True
			Me._Items = New List(Of SCORM_Item)
			Me._Title = ""
			Me._isValid = True
		End Sub
		Sub New(ByVal Identifier As String, ByVal Title As String, ByVal isVisible As Boolean, Optional ByVal StructureType As String = "", Optional ByVal isDefault As Boolean = False)
			Me._ID = Identifier
			Me._isVisible = isVisible
			Me._StructureType = StructureType
			Me._Items = New List(Of SCORM_Item)
			Me._Title = Title
			Me._isValid = True
			Me._isDefault = isDefault
		End Sub

		Public Function Validate() As Boolean
			If Me._isValid = False Then
				Return False
			Else
				Return IsNothing(Me._Items.Find(New GenericPredicate(Of SCORM_Item, Boolean)(False, AddressOf SCORM_Item.FindByValidita)))
			End If
		End Function
		Public Shared Function FindByValidita(ByVal item As SCORM_Organization, ByVal argument As Boolean) As Boolean
			Return item.Validate = argument
		End Function
	End Class
End Namespace