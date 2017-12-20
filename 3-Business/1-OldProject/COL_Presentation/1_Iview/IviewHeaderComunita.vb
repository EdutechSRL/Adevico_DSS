Public Interface IviewHeaderComunita
	Inherits IviewBase

	Property ShowLanguage() As Boolean
    Property ShowNews() As Boolean
    'Property EnableAjaxManager() As Boolean
	ReadOnly Property isAutenticato() As Boolean
	ReadOnly Property PopUpRiepilogo() As PopUpWinComol
	ReadOnly Property PopUpAvvisoGenerale() As PopUpWinComol
    Property HeaderNewsMemoHeight() As System.Web.UI.WebControls.Unit
    Sub LoadMenu(ByVal items As GenericCollection(Of MenuElement))
    Sub LoadPostIt(ByVal items As GenericCollection(Of PopUpWinComol))
    Sub SetCommunityName(ByVal ShortName As String, ByVal LongName As String)
End Interface