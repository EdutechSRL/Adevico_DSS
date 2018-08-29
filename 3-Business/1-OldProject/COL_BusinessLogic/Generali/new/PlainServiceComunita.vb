Public Class PlainServiceComunita
	Inherits PlainService

#Region "Private Properties"
	Private _ComunitaID As Integer
	Private _isNonDisattivabile As Boolean
	Private _isAttivato As Boolean
	Private _isAbilitato As Boolean
	Private _isNotificabile As Boolean
#End Region

#Region "Public Properties"
	Public Property ComunitaID() As Integer
		Get
			ComunitaID = _ComunitaID
		End Get
		Set(ByVal Value As Integer)
			_ComunitaID = Value
		End Set
	End Property
	Public Property isAttivato() As Boolean
		Get
			isAttivato = _isAttivato
		End Get
		Set(ByVal Value As Boolean)
			_isAttivato = Value
		End Set
	End Property
	Public Property isAbilitato() As Boolean
		Get
			isAbilitato = _isAbilitato
		End Get
		Set(ByVal Value As Boolean)
			_isAbilitato = Value
		End Set
	End Property
	Public Property isNonDisattivabile() As Boolean
		Get
			isNonDisattivabile = _isNonDisattivabile
		End Get
		Set(ByVal Value As Boolean)
			_isNonDisattivabile = Value
		End Set
	End Property
	Public Property isNotificabile() As Boolean
		Get
			isNotificabile = _isNotificabile
		End Get
		Set(ByVal Value As Boolean)
			_isNotificabile = Value
		End Set
	End Property
#End Region

	Public Sub New()
		MyBase.New()
	End Sub

	Public Sub New(ByVal ID As Integer)
		MyBase.New(ID)
	End Sub
	Public Sub New(ByVal ID As Integer, ByVal Code As String, ByVal isAttivato As Boolean, ByVal isAbilitato As Boolean, ByVal ComunitaID As Integer)
		MyBase.New(ID, Code)
		_isAttivato = isAttivato
		_isAbilitato = isAbilitato
		_ComunitaID = ComunitaID
	End Sub
	Public Sub New(ByVal ID As Integer, ByVal Permessi As String, ByVal Code As String, ByVal isAttivato As Boolean, ByVal isAbilitato As Boolean, ByVal ComunitaID As Integer)
		MyBase.New(ID, Code, Permessi)
		_isAttivato = isAttivato
		_isAbilitato = isAbilitato
		_ComunitaID = ComunitaID
	End Sub
End Class