Imports EeekSoft

Public Interface IviewHeaderAdmin
	Inherits IviewBase

	Property ShowNews() As Boolean
	'ReadOnly Property PopUpAvvisoGenerale() As EeekSoft.Web.PopupWin
	ReadOnly Property PopUpAvvisoGenerale() As PopUpWinComol
	ReadOnly Property ShowLogo() As Boolean

	Sub CaricaLogo(ByVal items As LinkElement)
	Sub GeneraMenu(ByVal items As GenericCollection(Of MenuElement))
	Sub CaricaHistory(ByVal items As HistoryElement)
End Interface