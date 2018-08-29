Public Class ServicePage

#Region "Private Properties"
	Private _ID As Integer
	Private _Nome As String
	Private _Url As String
	Private _isDefault As Boolean
	Private _Servizio As PlainServiceComunita
	Private _LinguaID As Integer
	Private _ComunitaID As Integer
#End Region

#Region "Public Properties"
	Public Property ID() As Integer
		Get
			ID = _ID
		End Get
		Set(ByVal value As Integer)
			_ID = value
		End Set
	End Property
	Public Property Nome() As String
		Get
			Nome = _Nome
		End Get
		Set(ByVal value As String)
			_Nome = value
		End Set
	End Property
	Public Property Url() As String
		Get
			Url = _Url
		End Get
		Set(ByVal value As String)
			_Url = value
		End Set
	End Property
	Public Property isDefault() As Boolean
		Get
			isDefault = _isDefault
		End Get
		Set(ByVal value As Boolean)
			_isDefault = value
		End Set
	End Property
	Public Property Servizio() As PlainServiceComunita
		Get
			Servizio = _Servizio
		End Get
		Set(ByVal value As PlainServiceComunita)
			_Servizio = value
		End Set
	End Property
	Public Property LinguaID() As Integer
		Get
			LinguaID = _LinguaID
		End Get
		Set(ByVal value As Integer)
			_LinguaID = value
		End Set
	End Property
	Public Property ComunitaID() As Integer
		Get
			ComunitaID = _ComunitaID
		End Get
		Set(ByVal value As Integer)
			_ComunitaID = value
		End Set
	End Property
#End Region

	Public Sub New()

	End Sub
	Public Sub New(ByVal LinguaID As Integer, ByVal ID As Integer)
		With Me
			._ID = ID
			._isDefault = False
			_LinguaID = LinguaID
		End With
	End Sub
	Public Sub New(ByVal LinguaID As Integer, ByVal ID As Integer, ByVal NomePagina As String, ByVal ComunitaID As Integer)
		With Me
			._ID = ID
			._isDefault = False
			._Nome = NomePagina
			._Url = ""
			._Servizio = New PlainServiceComunita(0)
			._ComunitaID = ComunitaID
			_LinguaID = LinguaID
		End With
	End Sub
	Public Sub New(ByVal LinguaID As Integer, ByVal ID As Integer, ByVal NomePagina As String, ByVal Url As String, ByVal isDefault As Boolean, ByVal ComunitaID As Integer, ByVal ServizioID As Integer)
		With Me
			._ID = ID
			._isDefault = isDefault
			._Nome = NomePagina
			._Url = Url
			._Servizio = New PlainServiceComunita(ServizioID)
			._ComunitaID = ComunitaID
			_LinguaID = LinguaID
		End With
	End Sub
	Public Sub New(ByVal LinguaID As Integer, ByVal ID As Integer, ByVal NomePagina As String, ByVal Url As String, ByVal isDefault As Boolean, ByVal ComunitaID As Integer, ByVal oServizio As PlainServiceComunita)
		With Me
			._ID = ID
			._isDefault = isDefault
			._Nome = NomePagina
			._Url = Url
			._Servizio = oServizio
			._ComunitaID = ComunitaID
			_LinguaID = LinguaID
		End With
	End Sub

	Public Shared Function FindByLinguaID(ByVal item As ServicePage, ByVal argument As Integer) As Boolean
		Return item.LinguaID = argument
	End Function
	Public Shared Function FindByDefault(ByVal item As ServicePage, ByVal argument As Boolean) As Boolean
		Return item.isDefault = argument
	End Function
End Class
