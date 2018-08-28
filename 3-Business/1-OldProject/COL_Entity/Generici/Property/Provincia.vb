<Serializable(), CLSCompliant(True)> Public Class Provincia
	Inherits DomainObject

#Region "Private Property"
	Private _ID As Integer
	Private _Nome As String
	Private _Sigla As String
#End Region

#Region "Public Property"
	Public Property ID() As Integer
		Get
			ID = _ID
		End Get
		Set(ByVal Value As Integer)
			_ID = Value
		End Set
	End Property
	Public Property Nome() As String
		Get
			Nome = _Nome
		End Get
		Set(ByVal Value As String)
			_Nome = Value
		End Set
	End Property
	Public Property Sigla() As String
		Get
			Sigla = _Sigla
		End Get
		Set(ByVal Value As String)
			_Sigla = Value
		End Set
	End Property
#End Region

#Region "Metodi New"
	Sub New()
		MyBase.New()
	End Sub
	Sub New(ByVal iID As Integer, ByVal iNome As String, ByVal iSigla As String)
		MyBase.New()
		Me._ID = iID
		Me._Nome = iNome
		Me._Sigla = iSigla
	End Sub
#End Region

	Public Shared Function FindByID(ByVal item As Provincia, ByVal ProvinciaID As Integer) As Boolean
		Return item.ID = ProvinciaID
	End Function

End Class