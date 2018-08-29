Public Class PlainOrganizzazione
	Inherits COL_Comunita
	Private _isFacolta As Boolean

	Public Property IsFacolta() As Boolean
		Get
			IsFacolta = _isFacolta
		End Get
		Set(ByVal Value As Boolean)
			_isFacolta = Value
		End Set
	End Property
	Public Sub New(ByVal ComunitaID As Integer, ByVal nome As String, ByVal isFacolta As Boolean, ByVal oOrganizzazione As COL_Organizzazione)
		n_CMNT_id = ComunitaID
		Me.n_CMNT_nome = nome
		_isFacolta = isFacolta
		Me.Organizzazione = oOrganizzazione
		Me.n_Errore = Errori_Db.None
	End Sub
End Class