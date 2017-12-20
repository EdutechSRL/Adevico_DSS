Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.Comol.Entities
Imports Comol.Entity

Public Interface IViewCommon
	ReadOnly Property Resource() As ResourceManager

	Property PageIstance() As Boolean


	ReadOnly Property IstituzioneID() As Integer
	ReadOnly Property OrganizzazioneID() As Integer


	' gestione Url
	ReadOnly Property BaseUrl() As String
	ReadOnly Property FullBaseUrl() As String
	ReadOnly Property CurrentPage() As String
	ReadOnly Property BaseUrlDrivePath() As String
	ReadOnly Property RequireSSL() As Boolean
    Sub GoToPortale()
    Sub GoToPortale(ByVal url As String)
    Sub RedirectToDefault(Optional ByVal QueryParameters As String = "")
	Sub RedirectToUrl(ByVal Url As String)
	Sub RedirectToLoginUrl(ByVal Url As String)
	Sub RedirectToEncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType)
	Function EncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
	Function EncryptedQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
	Function DecryptQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String

	'Gestione Impostazioni utente
    'Property Impostazioni() As COL_ImpostazioniUtente
	ReadOnly Property UtenteCorrente() As COL_Persona
	ReadOnly Property TipoPersonaID() As Integer

	ReadOnly Property UserSessionLanguage() As Lingua
	ReadOnly Property LinguaCode() As String
	ReadOnly Property LinguaID() As Integer
	Property NewLinguaID() As Integer

	'Accesso Comunita
	ReadOnly Property isPortalCommunity() As Boolean
	ReadOnly Property isNonIscrittoComunita() As Boolean
	ReadOnly Property isUtenteAnonimo() As Boolean
	Property isModalitaAmministrazione() As Boolean
	Property AmministrazioneComunitaID() As Integer
	ReadOnly Property ComunitaCorrenteID() As Integer
	ReadOnly Property ComunitaLavoroID() As Integer
	ReadOnly Property ComunitaCorrente() As COL_Comunita
	ReadOnly Property TipoComunitaCorrenteID() As Integer
	ReadOnly Property RuoloCorrenteID() As Integer
	ReadOnly Property ElencoServizi() As ServiziCorrenti
	Sub SetElencoServizi(ByVal oLista As ServiziCorrenti)

	'Internazionalizzazione
	Sub SetCultureSettings()
	Sub SetInternazionalizzazione()
	Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String)
	Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String)
	Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String, ByVal ResourceFolderLevel3 As String)
	Sub SetCookies(ByVal LinguaID As Integer, ByVal LinguaCode As String)

	''Registro Eventi
	'Sub RegistraEvento(ByVal statoAttività As String, Optional ByVal idAttività As Integer = 0, Optional ByVal classeAttività As String = "", Optional ByVal attributo1 As String = "", Optional ByVal attributo2 As String = "", Optional ByVal attributo3 As String = "")

	'Sub AddAction(ByVal oService As lm.ActionDataContract.iRemoteService, ByVal oType As Integer, ByVal oObjectActions As System.Collections.Generic.List(Of lm.ActionDataContract.ObjectAction), Optional ByVal TypeIteration As lm.ActionDataContract.InteractionType = lm.ActionDataContract.InteractionType.Generic)
	'Sub AddActionToModule(ByVal oService As lm.ActionDataContract.iRemoteService, ByVal ModuleID As Integer, ByVal oType As Integer, ByVal oObjectActions As System.Collections.Generic.List(Of lm.ActionDataContract.ObjectAction), Optional ByVal TypeIteration As lm.ActionDataContract.InteractionType = lm.ActionDataContract.InteractionType.Generic)
ReadOnly Property CurrentModuleID() As Integer



	'Metodi Base
	Sub BindDati()
ReadOnly Property SystemSettings() As ComolSettings

	'Optional ByVal LanguageCode As String = ""
	ReadOnly Property LocalizedMail() As MailLocalized
    ReadOnly Property AccessoSistema() As Boolean
	Sub CambiaImpostazioniLingua(ByVal LinguaID As Integer, Optional ByVal LingaCode As String = "", Optional ByVal ReloadUtenteCorrente As Boolean = False)
    ReadOnly Property ObjectPath(ByVal oPath As ConfigurationPath) As ObjectFilePath

	ReadOnly Property ApplicationUrlBase(Optional ByVal WithoutSSLfromConfig As Boolean = False) As String
	ReadOnly Property DefaultUrl() As String

	'Property Tutor() As COL_BusinessLogic_v2.Comol.Entities.KnowledgeTutor

End Interface
