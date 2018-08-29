Public Class GenericTreeNode

#Region "Private Property"
	Private _Text As String
	Private _Value As String
	Private _isSystemNode As Boolean
	Private _ChildNodes As List(Of GenericTreeNode)
	Private _Expanded As Boolean
#End Region

#Region "Public Property"
	Public Property Text() As String
		Get
			Return _Text
		End Get
		Set(ByVal value As String)
			_Text = value
		End Set
	End Property
	Public Property Value() As String
		Get
			Return _Value
		End Get
		Set(ByVal value As String)
			_Value = value
		End Set
	End Property
	Public Property isSystemNode() As Boolean
		Get
			Return _isSystemNode
		End Get
		Set(ByVal value As Boolean)
			_isSystemNode = value
		End Set
	End Property
	Public Property ChildNodes() As List(Of GenericTreeNode)
		Get
			Return _ChildNodes
		End Get
		Set(ByVal value As List(Of GenericTreeNode))
			_ChildNodes = value
		End Set
	End Property
	Public Property Expanded() As Boolean
		Get
			Return _Expanded
		End Get
		Set(ByVal value As Boolean)
			_Expanded = value
		End Set
	End Property
#End Region

	Sub New()
		_ChildNodes = New List(Of GenericTreeNode)
		_isSystemNode = False
		_Expanded = False
	End Sub

	Sub New(ByVal iText As String, ByVal iValue As String, Optional ByVal isSistemNode As Boolean = False)
		_Text = iText
		_Value = iValue
		_ChildNodes = New List(Of GenericTreeNode)
		_isSystemNode = isSistemNode
		_Expanded = False
	End Sub

End Class