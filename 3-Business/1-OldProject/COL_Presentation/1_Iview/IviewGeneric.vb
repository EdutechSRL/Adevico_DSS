Public Interface IviewGeneric
	Inherits IviewBase

	Function HasPermessi() As Boolean
	Sub ShowAlertMessage(ByVal message As String)
	Sub ShowAlertError(ByVal errorMessage As String)
	Sub ShowMessageToPage(ByVal errorMessage As String)
	Sub RegistraAccessoPagina()
	Sub BindNoPermessi()
End Interface