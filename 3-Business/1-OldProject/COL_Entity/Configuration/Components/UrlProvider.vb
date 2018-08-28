Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class UrlProvider

#Region "Private Property"
		Private _ComolID As Integer
		Private _Name As String
		Private _QueryID As String
		Private _DeltaTime As TimeSpan
		Private _EncryptionAlgorithm As Integer
		Private _Key As String
		Private _Vect As String
		Private _RemoteLogin As String
        Private _LoginPrefixs As List(Of AutoLoginPrefix)
        Private _UserDataFormat As Integer
#End Region
#Region "Public Property"
		Public Property ComolID() As Integer
			Get
				Return _ComolID
			End Get
			Set(ByVal value As Integer)
				_ComolID = value
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
		Public Property QueryID() As String
			Get
				Return _QueryID
			End Get
			Set(ByVal value As String)
				_QueryID = value
			End Set
		End Property
		Public Property DeltaTime() As TimeSpan
			Get
				Return _DeltaTime
			End Get
			Set(ByVal value As TimeSpan)
				_DeltaTime = value
			End Set
		End Property
		Public Property EncryptionAlgorithm() As Integer
			Get
				Return _EncryptionAlgorithm
			End Get
			Set(ByVal value As Integer)
				_EncryptionAlgorithm = value
			End Set
		End Property
		Public Property Key() As String
			Get
				Return _Key
			End Get
			Set(ByVal value As String)
				_Key = value
			End Set
		End Property
		Public Property Vect() As String
			Get
				Return _Vect
			End Get
			Set(ByVal value As String)
				_Vect = value
			End Set
		End Property
		Public Property RemoteLogin() As String
			Get
				Return _RemoteLogin
			End Get
			Set(ByVal value As String)
				_RemoteLogin = value
			End Set
		End Property
		Public Property LoginPrefixs() As List(Of AutoLoginPrefix)
			Get
				Return _LoginPrefixs
			End Get
			Set(ByVal value As List(Of AutoLoginPrefix))
				_LoginPrefixs = value
			End Set
        End Property
        Public Property UserDataFormat() As Integer
            Get
                Return _UserDataFormat
            End Get
            Set(ByVal value As Integer)
                _UserDataFormat = value
            End Set
        End Property
#End Region
		Sub New()
			_LoginPrefixs = New List(Of AutoLoginPrefix)
		End Sub
	End Class
End Namespace