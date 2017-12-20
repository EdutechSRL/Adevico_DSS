Public Interface IviewGestioneUtente
	Property UniqueID() As Integer
	Property Fase() As Status
	Property Persona() As COL_Persona


	Enum Status
		Show
		Insert
		Change
		Delete
	End Enum
End Interface
