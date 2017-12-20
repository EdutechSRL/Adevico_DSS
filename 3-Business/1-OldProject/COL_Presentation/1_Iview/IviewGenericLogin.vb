Imports System.Net
Imports COL_BusinessLogic_v2.Comol.Authentication

Public Interface IviewGenericLogin

	ReadOnly Property Presenter() As PresenterGenericLogin
	Property Login() As String
	Property Password() As String
	Property MailToRetrieve() As Mail.MailAddress
	ReadOnly Property SystemActive() As Boolean
	Property EnableLink(ByVal oType As LinkType) As Boolean
	Property ShowLink(ByVal oType As LinkType) As Boolean
	WriteOnly Property EnableHelpToSubscription() As Boolean
	ReadOnly Property Config() As LoginSettings
	Property NewLinguaID() As Integer
	ReadOnly Property LinguaID() As Integer
	Property Recapiti() As String
	Property ShowRecapiti() As Boolean
	ReadOnly Property MailConfig() As MailLocalized
	ReadOnly Property MailTitle(ByVal isComol As Boolean) As String
	ReadOnly Property MailBody(ByVal isComol As Boolean) As String


	Sub ShowForm(ByVal oForm As Form)
	Sub ShowMessageToUser(ByVal Message As MessageType)

	Enum MessageType
		MailInviata = 0
		MailNonInviata = 1
		PasswordErrata = 2
		AccountDisabilitato = 3
		LogonBloccato = 4
		AccountNonAttivato = 5
		AccountNonRegistrato = 6
		NoConnessioneDB = -1
		NoConnessioneLDAP = -2
		NoConnessioneAlcuniLDAP = -3
		ConnessioneDBdisponibile = 7
		ConnessioneLDAPdisponibile = 8
		AccountNonTrovato = 9
		NoMessage = 10
	End Enum

	Enum Form
		Login
		RetrivePassword
	End Enum
	Enum LinkType
		Subscription
		RetrivePassword
        Logon
	End Enum

End Interface
