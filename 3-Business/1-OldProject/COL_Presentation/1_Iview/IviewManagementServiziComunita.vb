Public Interface IviewManagementServiziComunita
	ReadOnly Property ComunitaID() As Integer
	ReadOnly Property ServizioID() As Integer
	ReadOnly Property RuoloID() As Integer
	ReadOnly Property LinguaCorrenteID() As Integer

	Sub PopulateDetails(ByVal Lista As List(Of RuoloServizio))
End Interface