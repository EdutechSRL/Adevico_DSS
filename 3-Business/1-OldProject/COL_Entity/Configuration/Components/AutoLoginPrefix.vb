Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class AutoLoginPrefix
#Region "Private properties"
		Private _Name As String
		Private _Prefix As String
		Private _isDefault As Boolean
#End Region

#Region "Public properties"
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				_Name = value
			End Set
		End Property
		Public Property Prefix() As String
			Get
				Return _Prefix
			End Get
			Set(ByVal value As String)
				_Prefix = value
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
#End Region

		Sub New()
			_isDefault = False
		End Sub
		Sub New(ByVal iName As String, ByVal iPrefix As String)
			Me._Name = iName
			Me._Prefix = iPrefix
			_isDefault = False
		End Sub
		Sub New(ByVal iName As String, ByVal iPrefix As String, ByVal iIsDefault As Boolean)
			Me._Name = iName
			Me._Prefix = iPrefix
			_isDefault = iIsDefault
		End Sub
	End Class
End Namespace