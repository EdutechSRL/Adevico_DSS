<Serializable(), CLSCompliant(True)> Public Class Lingua


#Region "Private Property"
	Private _id As Integer
	Private _Nome As String
	Private _Codice As String
	Private _default As Boolean
	Private _Icona As String
#End Region

#Region "Public Property"
	Public Property ID() As Integer
		Get
			ID = _id
		End Get
		Set(ByVal Value As Integer)
			_id = Value
		End Set
	End Property
	'Public Property Nome() As String
	'	Get
	'		Nome = _Nome
	'	End Get
	'	Set(ByVal Value As String)
	'		_Nome = Value
	'	End Set
	'End Property
	'Public Property Codice() As String
	'	Get
	'		Codice = _Codice
	'	End Get
	'	Set(ByVal Value As String)
	'		_Codice = Value
	'	End Set
	'End Property
	Public ReadOnly Property Nome() As String
		Get
			Nome = _Nome
		End Get
	End Property
	Public ReadOnly Property Codice() As String
		Get
			Codice = _Codice
		End Get
	End Property
	Public Property isDefault() As Boolean
		Get
			isDefault = _default
		End Get
		Set(ByVal Value As Boolean)
			_default = Value
		End Set
	End Property
	Public Property Icona() As String
		Get
			Icona = _Icona
		End Get
		Set(ByVal Value As String)
			_Icona = Value
		End Set
	End Property
	Public ReadOnly Property Sigla() As String
		Get
			Return _Codice.Substring(0, 2).ToUpperInvariant
		End Get

	End Property
#End Region

#Region "Metodi New"
	Sub New()

	End Sub
	Sub New(ByVal iID As Integer, ByVal iNome As String, ByVal iCodice As String)
		Me._id = iID
		Me._Codice = iCodice
		Me._Nome = iNome
	End Sub
	Sub New(ByVal iID As Integer, ByVal iCodice As String)
		Me._id = iID
		Me._Codice = iCodice
	End Sub
	Sub New(ByVal iID As Integer, ByVal iNome As String, ByVal iCodice As String, ByVal isDefault As Boolean, ByVal iIcona As String)
		Me._id = iID
		Me._Nome = iNome
		Me._Codice = iCodice
		Me._default = isDefault
		Me._Icona = iIcona
	End Sub

	Public Shared Function CreateByName(ByVal iID As Integer, ByVal iNome As String) As Lingua
		Return New Lingua(iID, iNome, "")
	End Function
	Public Shared Function CreateByCode(ByVal iID As Integer, ByVal iCode As String) As Lingua
		Return New Lingua(iID, "", iCode)
	End Function
	Public Shared Function CreateByNameAndCode(ByVal iID As Integer, ByVal iNome As String, ByVal iCode As String) As Lingua
		Return New Lingua(iID, iNome, iCode)
	End Function
#End Region
End Class