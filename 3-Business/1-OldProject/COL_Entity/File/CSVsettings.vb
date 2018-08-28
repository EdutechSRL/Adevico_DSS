Namespace File
	<Serializable(), CLSCompliant(True)> Public Class FileCSV

#Region "Private Property"
		Private _ColumnDelimeter As String
		Private _TextDelimeter As String
		Private _Fields As New List(Of FileCsvField)
#End Region
#Region "Public Property"
		Public Property ColumnDelimeter() As String
			Get
				ColumnDelimeter = _ColumnDelimeter
			End Get
			Set(ByVal value As String)
				_ColumnDelimeter = value
			End Set
		End Property
		Public Property TextDelimeter() As String
			Get
				TextDelimeter = _TextDelimeter
			End Get
			Set(ByVal value As String)
				_TextDelimeter = value
			End Set
		End Property
		Public Property Fields() As List(Of FileCsvField)
			Get
				Fields = _Fields
			End Get
			Set(ByVal value As List(Of FileCsvField))
				_Fields = value
			End Set
		End Property
#End Region

		Sub New()
			_Fields = New List(Of FileCsvField)
		End Sub
		Sub New(ByVal oColumnDelimeter As String, ByVal oTextDelimeter As String)
			_ColumnDelimeter = oColumnDelimeter
			_TextDelimeter = oTextDelimeter
			_Fields = New List(Of FileCsvField)
		End Sub
		Sub New(ByVal oColumnDelimeter As String, ByVal oTextDelimeter As String, ByVal oFields As List(Of FileCsvField))
			_ColumnDelimeter = oColumnDelimeter
			_TextDelimeter = oTextDelimeter
			_Fields = oFields
		End Sub

	End Class
End Namespace
