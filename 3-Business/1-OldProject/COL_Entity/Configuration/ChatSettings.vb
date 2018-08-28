Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class ChatSettings

#Region "Private properties"
		Private _Enabled As Boolean
		Private _DefaultUrl As String
		Private _DefaultFileUrl As String
#End Region

#Region "Public properties"
		Public Property Enabled() As Boolean
			Get
				Enabled = _Enabled
			End Get
			Set(ByVal value As Boolean)
				_Enabled = value
			End Set
		End Property
		Public Property DefaultUrl() As String
			Get
				DefaultUrl = _DefaultUrl
			End Get
			Set(ByVal value As String)
				_DefaultUrl = value
			End Set
		End Property
		Public Property DefaultFileUrl() As String
			Get
				DefaultFileUrl = _DefaultFileUrl
			End Get
			Set(ByVal value As String)
				_DefaultFileUrl = value
			End Set
		End Property
#End Region

		Public Sub New()
			_Enabled = False
		End Sub

	End Class
End Namespace