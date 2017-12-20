Public Interface IviewPopUp
	Inherits IViewCommon

	Sub BindNoPermessi()
	Function HasPermessi() As Boolean
	ReadOnly Property AutoCloseWindow() As Boolean
	ReadOnly Property VerifyAuthentication() As Boolean
End Interface
