Namespace Comol.Materiale.Scorm
	Public Class SCORM_Item

#Region "Private Property"
		Private _ID As String		'Identificatore univoco
		Private _IDresource As String		'Riferimento alla risorsa di riferimento
		Private _IndexResourceURL As String	'Riferimento all'URL del file d'inizio
		Private _Title As String
		Private _Items As List(Of SCORM_Item)
		Private _Resource As SCORM_Resource
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
		Public Property IDresource() As String
			Get
				Return _IDresource
			End Get
			Set(ByVal value As String)
				_IDresource = value
			End Set
		End Property
		Public Property IndexResourceURL() As String
			Get
				Return _IndexResourceURL
			End Get
			Set(ByVal value As String)
				_IndexResourceURL = value
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
		Public Property isVisible() As Boolean
			Get
				isVisible = _isVisible
			End Get
			Set(ByVal value As Boolean)
				_isVisible = value
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

		Public Property Resource() As SCORM_Resource
			Get
				Return _Resource
			End Get
			Set(ByVal value As SCORM_Resource)
				_Resource = value
			End Set
		End Property
#End Region

		Public Sub New()
			_Items = New List(Of SCORM_Item)
			_isVisible = isVisible
			_isValid = True
		End Sub
		Public Sub New(ByVal Title As String, ByVal Index As String, _
		 ByVal IDitem As String, ByVal idResource As String, ByVal isVisible As Boolean)
			_ID = IDitem
			_IDresource = idResource
			_IndexResourceURL = Index
			_Title = Title
			_Items = New List(Of SCORM_Item)
			_isVisible = isVisible
			_isValid = True
		End Sub
		Public Function Validate() As Boolean
			If Me._isValid = False Then
				Return False
			Else
				Return IsNothing(Me._Items.Find(New GenericPredicate(Of SCORM_Item, Boolean)(False, AddressOf SCORM_Item.FindByValidita)))
			End If
		End Function
		Public Shared Function FindByValidita(ByVal item As SCORM_Item, ByVal argument As Boolean) As Boolean
			Return item.Validate = argument
		End Function

	End Class
End Namespace