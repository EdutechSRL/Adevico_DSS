Public Interface IviewHeaderCommunity
    Inherits IviewBase

    Property ShowLanguage() As Boolean
    Property ShowNews() As Boolean
    ReadOnly Property isAutenticato() As Boolean
    ReadOnly Property PopUpRiepilogo() As PopUpWinComol
    ReadOnly Property PopUpAvvisoGenerale() As PopUpWinComol

    Sub LoadMenu(ByVal items As GenericCollection(Of MenuElement))
    Sub LoadPostIt(ByVal items As GenericCollection(Of PopUpWinComol))
    Sub SetCommunityName(ByVal CommunityName As String)
End Interface