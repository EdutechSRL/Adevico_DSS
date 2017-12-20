Imports System.Collections.Generic

Public Interface IviewPermissionDetails
	ReadOnly Property ComunitaID() As Integer
	ReadOnly Property ServizioID() As Integer
	ReadOnly Property RuoloID() As Integer
	ReadOnly Property LinguaCorrenteID() As Integer

	Sub ChangeTitoloDettaglio(ByVal Testo As String)
	Sub ShowNoServizio()
	Sub PopulateDetails(ByVal Lista As List(Of RuoloServizio))
End Interface