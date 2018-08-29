Public Class ServiceElement
	Private _ServizioID As Integer
	Private _ServizioCode As String
	Private _Services As MyServices

	Public Property ServizioID() As Integer
		Get
			ServizioID = _ServizioID
		End Get
		Set(ByVal value As Integer)
			_ServizioID = value
		End Set
	End Property
	Public Property ServizioCode() As String
		Get
			ServizioCode = _ServizioCode
		End Get
		Set(ByVal value As String)
			_ServizioCode = value
		End Set
	End Property
	Public Property ServizioConcreto() As MyServices
		Get
			ServizioConcreto = _Services
		End Get
		Set(ByVal value As MyServices)
			_Services = value
		End Set
	End Property

	Public Sub New()

	End Sub
	Public Sub New(ByVal ID As Integer, ByVal Code As String)
		_ServizioID = ID
		_ServizioCode = Code
	End Sub
	Public Sub New(ByVal ID As Integer, ByVal Code As String, ByVal Servizio As MyServices)
		_ServizioID = ID
		_ServizioCode = Code
		_Services = Servizio
	End Sub
End Class