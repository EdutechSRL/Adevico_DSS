Namespace File
	<Serializable(), CLSCompliant(True)> Public Class FileCsvField

#Region "Private properties"
		Private _HeaderName As String
		Private _DBField As String
		Private _PropertyName As String

#End Region

#Region "Public properties"
		Public ReadOnly Property Header() As String
			Get
				Header = _HeaderName
			End Get
		End Property
		Public ReadOnly Property DBField() As String
			Get
				DBField = _DBField
			End Get
		End Property
		Public ReadOnly Property PropertyName() As String
			Get
				PropertyName = _PropertyName
			End Get
		End Property

#End Region

		Public Sub New(ByVal oHeaderName As String, ByVal oDBField As String, ByVal oPropertyName As String)
			_HeaderName = oHeaderName
			_DBField = oDBField
			_PropertyName = oPropertyName
		End Sub
	End Class
End Namespace