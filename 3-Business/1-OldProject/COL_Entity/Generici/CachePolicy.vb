Public Class CachePolicy

#Region "Const Wiki"
	Private Const _Wiki As String = "Wiki{0}"
	Private Const _WikiSezioni As String = "WikiSezioni{0}"
	Private Const _WikiTopics As String = "WikiTopics{0}"
	Private Const _WikiTopicHistory As String = "WikiTopicHistory{0}"
	Private Const _WikiAllTopics As String = "WikiAllTopics{0}"
#End Region
#Region "Const Settings"
	Private Const _LocalizedMailSettings As String = "Settings{0}"
	Private Const _GlobalSettings As String = "_GlobalSettings"
	Private Const _SettingsIcon As String = "SettingsIcon"
	Private Const _SettingsMimeType As String = "SettingsMimeType"
#End Region
#Region "Const Materiale"
	Private Const _ScormManifest As String = "ScormManifest{0}{1}"
	Private Const _MaterialeComunita As String = "MaterialeComunita{0}"
	Private Const _CategoriaMateriale As String = "CategoriaMateriale{0}"
	Private Const _HashCategoriaMateriale As String = "HashCategoriaMateriale{0}"
	Private Const _TipoMateriale As String = "TipoMateriale{0}"
	Private Const _HashTipoMateriale As String = "HashTipoMateriale{0}"
	Private Const _CategoriaMaterialeComunita As String = "CategoriaMaterialeComunita{0}{1}"
	Private Const _SpazioFileComunita As String = "SpazioFileComunita{0}"
#End Region
#Region "Const Menu"
	Private Const _MenuComunita As String = "MenuComunita{0}{1}{2}"
	Private Const _MenuPortale As String = "MenuPortale{0}{1}{2}"
	Private Const _NewsScorrevoli As String = "NewsScorrevoli{0}"
	Private Const _Postit As String = "Postit{0}{1}{2}"
#End Region
#Region "Const User Action - Presence"
    Private Const _CachedKey As String = "UserActionHashKey"

    Private Const _UserAction As String = "UserAction{0}{1}"
    Private Const _UserActionKeys As String = "UserActionKeys"
    Private Const _UserActionList As String = "UserActionList"
#End Region

    Private Const _DiarioLezione As String = "Diario{0}{1}"
    Private Const _PlainIscrizioni As String = "PlainIscrizioni{0}{1}"
    Private Const _ServiziComunita As String = "ServiziComunita{0}{1}{2}"
    Private Const _ListaIscritti As String = "ListaIscritti{0}{1}"
    Private Const _TipiComunita As String = "TipiComunita{0}"
    Private Const _TipiCorso As String = "TipiCorso{0}"
    Private Const _TipiCorsoDiLaurea As String = "TipiCorsoDiLaurea{0}{1}"
    Private Const _Periodo As String = "Periodo{0}{1}"
    Private Const _Organizzazione As String = "Organizzazione"
    Private Const _OrganizzazioneList As String = "_OrganizzazioneList"
    Private Const _OrganizzazioniAssociate As String = "OrganizzazioniAssociate(0)"
    Private Const _PagineDefault As String = "PagineDefault{0}{1}"
    Private Const _Bacheca As String = "Bacheca{0}"
    Private Const _Autenticazione As String = "Autenticazione{0}"
    Private Const _TipoAutenticazione As String = "TipoAutenticazione{0}"
    Private Const _AuthenticationServerConnections As String = "AuthenticationServerConnections{0}"
    Private Const _Registro As String = "Registro{0}"
    Private Const _Provincia As String = "Provincia{0}"
    Private Const _Stato As String = "Stato{0}"
    Private Const _Qualifica As String = "Qualifica{0}"
    Private Const _Regime As String = "Regime{0}"
    Private Const _Fascia As String = "Fascia{0}"
    Private Const _TipoDocente As String = "TipoDocente{0}"
    Private Const _Lingua As String = "Lingua{0}"
    Private Const _TipoIstituto As String = "TipoIstituto{0}"
    Private Const _Permessi As String = "Permessi{0}"
    Private Const _PermessiServizio As String = "PermessiServizio{0}{1}"
    Private Const _RuoliServizio As String = "RuoliServizio{0}{1}"
    Private Const _RuoliServizioComunita As String = "RuoliServizio{0}{1}{2}{3}"
    Private Const _DegreeType As String = "DegreeType{0}"
	Private Const _PermessiServizioUtente As String = "PermessiServizioUtente{0}{1}"
    Private Const _SmartTags As String = "SmartTags"
    Private Const _NotificationErrorSettings As String = "NotificationErrorSettings"

#Region "Generici"
    Public Shared Function RuoliServizioComunita() As String
        Return String.Format(_RuoliServizioComunita, "", "", "", "")
    End Function
    Public Shared Function RuoliServizioComunita(ByVal ComunitaID As Integer) As String
        Return String.Format(_RuoliServizioComunita, "_" & ComunitaID.ToString, "", "", "")
    End Function
    Public Shared Function RuoliServizioComunita(ByVal ComunitaID As Integer, ByVal ServizioID As Integer) As String
        Return String.Format(_RuoliServizioComunita, "_" & ComunitaID.ToString, "_" & ServizioID.ToString, "", "")
    End Function
    Public Shared Function RuoliServizioComunita(ByVal ComunitaID As Integer, ByVal ServizioID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_RuoliServizioComunita, "_" & ComunitaID.ToString, "_" & ServizioID.ToString, "_" & LinguaID.ToString, "")
    End Function
    Public Shared Function RuoliServizioComunita(ByVal ComunitaID As Integer, ByVal ServizioID As Integer, ByVal LinguaID As Integer, ByVal DefaultValue As Boolean) As String
        Return String.Format(_RuoliServizioComunita, "_" & ComunitaID.ToString, "_" & ServizioID.ToString, "_" & LinguaID.ToString, "_" & DefaultValue.ToString)
    End Function
    Public Shared Function RuoliServizio() As String
        Return String.Format(_RuoliServizio, "", "")
    End Function
    Public Shared Function RuoliServizio(ByVal ServizioID As Integer) As String
        Return String.Format(_RuoliServizio, "_" & ServizioID.ToString, "")
    End Function
    Public Shared Function RuoliServizio(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_RuoliServizio, "_" & ServizioID.ToString, "_" & LinguaID.ToString)
    End Function
    Public Shared Function Permessi() As String
        Return String.Format(_Permessi, "")
    End Function
    Public Shared Function Permessi(ByVal LinguaID As Integer) As String
        Return String.Format(_Permessi, "_" & LinguaID.ToString)
    End Function
    Public Shared Function PermessiServizio() As String
        Return String.Format(_PermessiServizio, "")
    End Function
    Public Shared Function PermessiServizio(ByVal ServizioID As Integer) As String
        Return String.Format(_PermessiServizio, "_" & ServizioID.ToString, "")
    End Function
    Public Shared Function PermessiServizio(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_PermessiServizio, "_" & ServizioID.ToString, "_" & LinguaID.ToString)
    End Function
    Public Shared Function Provincia() As String
        Return String.Format(_Provincia, "")
    End Function
    Public Shared Function Stato() As String
        Return String.Format(_Stato, "")
    End Function
    Public Shared Function TipoIstituto() As String
        Return String.Format(_TipoIstituto, "")
    End Function
    Public Shared Function Qualifica() As String
        Return String.Format(_Qualifica, "")
    End Function
    Public Shared Function Qualifica(ByVal LinguaID As Integer) As String
        Return String.Format(_Qualifica, "_" & LinguaID.ToString)
    End Function
    Public Shared Function Regime() As String
        Return String.Format(_Regime, "")
    End Function
    Public Shared Function Regime(ByVal LinguaID As Integer) As String
        Return String.Format(_Regime, "_" & LinguaID.ToString)
    End Function
    Public Shared Function Fascia() As String
        Return String.Format(_Fascia, "")
    End Function
    Public Shared Function Fascia(ByVal LinguaID As Integer) As String
        Return String.Format(_Fascia, "_" & LinguaID.ToString)
    End Function
    Public Shared Function TipoDocente() As String
        Return String.Format(_TipoDocente, "")
    End Function
    Public Shared Function TipoDocente(ByVal LinguaID As Integer) As String
        Return String.Format(_TipoDocente, "_" & LinguaID.ToString)
	End Function

	Public Shared Function PermessiServizioUtente() As String
		Return String.Format(_PermessiServizioUtente, "", "")
	End Function
	Public Shared Function PermessiServizioUtente(ByVal ServiceCode As String) As String
		Return String.Format(_PermessiServizioUtente, "_" & ServiceCode.ToString, "")
	End Function
	Public Shared Function PermessiServizioUtente(ByVal ServiceCode As String, ByVal PersonId As Integer) As String
		Return String.Format(_PermessiServizioUtente, "_" & ServiceCode, "_" & PersonId.ToString)
	End Function





    Public Shared Function Lingua() As String
        Return String.Format(_Lingua, "")
    End Function
    Public Shared Function Lingua(ByVal LinguaID As Integer) As String
        Return String.Format(_Lingua, "_" & LinguaID.ToString)
    End Function
#End Region
#Region "Autenticazione"
    Public Shared Function AuthenticationServerConnections() As String
        Return String.Format(_AuthenticationServerConnections, "")
    End Function
    Public Shared Function AuthenticationServerConnections(ByVal TypeID As Integer) As String
        Return String.Format(_AuthenticationServerConnections, "_" & TypeID.ToString)
    End Function
    Public Shared Function Autenticazione() As String
        Return String.Format(_Autenticazione, "")
    End Function
    Public Shared Function Autenticazione(ByVal LinguaID As Integer) As String
        Return String.Format(_Autenticazione, "_" & LinguaID.ToString)
    End Function
    Public Shared Function TipoAutenticazione() As String
        Return String.Format(_TipoAutenticazione, "")
    End Function
    Public Shared Function TipoAutenticazione(ByVal LinguaID As Integer) As String
        Return String.Format(_TipoAutenticazione, "_" & LinguaID.ToString)
    End Function
#End Region

#Region "ModuloEsse3"
    Public Shared Function Registro() As String
        Return String.Format(_Registro, "")
    End Function
    Public Shared Function Registro(ByVal PersonaID As Integer) As String
        Return String.Format(_Registro, "_" & PersonaID.ToString)
    End Function
#End Region

#Region "Materiale"
#Region "Materiale Comunita"

    Public Shared Function SpazioFileComunita() As String
        Return String.Format(_SpazioFileComunita, "", "")
    End Function
    Public Shared Function SpazioFileComunita(ByVal ComunitaID As Integer) As String
        Return String.Format(_SpazioFileComunita, "_" & ComunitaID.ToString)
    End Function
    Public Shared Function MaterialeComunita() As String
        Return String.Format(_MaterialeComunita, "")
    End Function
    Public Shared Function MaterialeComunita(ByVal ComunitaID As Integer) As String
        Return String.Format(_MaterialeComunita, "_" & ComunitaID.ToString)
    End Function
    Public Shared Function TipoMateriale() As String
        Return String.Format(_TipoMateriale, "", "")
    End Function
    Public Shared Function TipoMateriale(ByVal LinguaID As Integer) As String
        Return String.Format(_TipoMateriale, "_" & LinguaID.ToString, "")
    End Function
    Public Shared Function HashTipoMateriale() As String
        Return String.Format(_HashTipoMateriale, "", "")
    End Function
    Public Shared Function HashTipoMateriale(ByVal LinguaID As Integer) As String
        Return String.Format(_HashTipoMateriale, "_" & LinguaID.ToString, "")
    End Function
    Public Shared Function CategoriaMateriale() As String
        Return String.Format(_CategoriaMateriale, "", "")
    End Function
    Public Shared Function CategoriaMateriale(ByVal LinguaID As Integer) As String
        Return String.Format(_CategoriaMateriale, "_" & LinguaID.ToString, "")
    End Function
    Public Shared Function HashCategoriaMateriale() As String
        Return String.Format(_HashCategoriaMateriale, "", "")
    End Function
    Public Shared Function HashCategoriaMateriale(ByVal LinguaID As Integer) As String
        Return String.Format(_HashCategoriaMateriale, "_" & LinguaID.ToString, "")
    End Function
    Public Shared Function CategoriaMaterialeComunita() As String
        Return String.Format(_CategoriaMaterialeComunita, "", "")
    End Function
    Public Shared Function CategoriaMaterialeComunita(ByVal TipoComunitaID As Integer) As String
        Return String.Format(_CategoriaMaterialeComunita, "_" & TipoComunitaID.ToString, "")
    End Function
    Public Shared Function CategoriaMaterialeComunita(ByVal TipoComunitaID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_CategoriaMaterialeComunita, "_" & TipoComunitaID.ToString, "_" & LinguaID.ToString)
    End Function
#End Region
#Region "Scorm Manifest"
    Public Shared Function ScormManifest() As String
        Return String.Format(_ScormManifest, "", "")
    End Function
    Public Shared Function ScormManifest(ByVal ScormID As Integer) As String
        Return String.Format(_ScormManifest, "_" & ScormID.ToString, "")
    End Function
    Public Shared Function ScormManifest(ByVal ScormID As Integer, ByVal LinguaCode As String) As String
        Return String.Format(_ScormManifest, "_" & ScormID.ToString, "_" & LinguaCode)
    End Function
#End Region

#End Region

#Region "Bacheca"
    Public Shared Function Bacheca() As String
        Return String.Format(_Bacheca, "")
    End Function
    Public Shared Function Bacheca(ByVal ComunitaID As Integer) As String
        Return String.Format(_Bacheca, "_" & ComunitaID.ToString)
    End Function
#End Region

#Region "Settings"
    Public Shared Function NotificationErrorSettings() As String
        Return _NotificationErrorSettings
    End Function
    Public Shared Function GlobalSettings() As String
        Return _GlobalSettings
    End Function
    Public Shared Function Settings() As String
        Return String.Format(_LocalizedMailSettings, "")
    End Function
    Public Shared Function LocalizedMailSettings(ByVal LanguageCode As String) As String
        Return String.Format(_LocalizedMailSettings, "_" & LanguageCode)
    End Function
    Public Shared Function SettingsIcon() As String
        Return _SettingsIcon
    End Function
    Public Shared Function SettingsMimeType() As String
        Return _SettingsMimeType
    End Function
    Public Shared Function SmartTags() As String
        Return _SmartTags
    End Function
#End Region

#Region "Diario Lezioni"
    Public Shared Function DiarioLezione() As String
        Return String.Format(_DiarioLezione, "", "")
    End Function
    Public Shared Function DiarioLezione(ByVal ComunitaID As Integer) As String
        Return String.Format(_DiarioLezione, "_" & ComunitaID.ToString, "_")
    End Function
    Public Shared Function DiarioLezione(ByVal ComunitaID As Integer, ByVal RoleID As Integer) As String
        Return String.Format(_DiarioLezione, "_" & ComunitaID.ToString, "_" & RoleID.ToString)
    End Function
#End Region

#Region "UserAction - Presence"
    Public Shared Function CachedKey() As String
        Return _CachedKey
    End Function

    Public Shared Function UserAction(ByVal UserID As String, ByVal SessionID As String) As String
        Return String.Format(_UserAction, "_" & UserID, "_" & SessionID)
    End Function
    Public Shared Function UseActionKeys() As String
        Return _UserActionKeys
    End Function
    Public Shared Function UserActionList() As String
        Return _UserActionList
    End Function
#End Region
    Public Shared Function PlainIscrizioni() As String
        Return String.Format(_PlainIscrizioni, "", "")
    End Function
    Public Shared Function PlainIscrizioni(ByVal UtenteID As Integer) As String
        Return String.Format(_PlainIscrizioni, "_" & UtenteID.ToString, "_")
    End Function
    Public Shared Function PlainIscrizioni(ByVal UtenteID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_PlainIscrizioni, "_" & UtenteID.ToString, "_" & LinguaID.ToString)
    End Function

#Region "Menu sistema"
    Public Shared Function RenderAllCommunity() As String
        Return "MenuCommunity"
    End Function
    Public Shared Function RenderCommunity(idCommunity As Integer) As String
        Return "MenuCommunity_" + idCommunity.ToString()
    End Function
    Public Shared Function RenderCommunity(idCommunity As Integer, idRole As Integer) As String
        Return RenderCommunity(idCommunity) + "_" + idRole.ToString()
    End Function
    Public Shared Function RenderCommunity(idCommunity As Integer, idRole As Integer, idLanguage As Integer) As String
        Return RenderCommunity(idCommunity, idRole) + "_" + idLanguage.ToString()
    End Function



    Public Shared Function MenuComunita() As String
        Return String.Format(_MenuComunita, "", "", "")
    End Function
    Public Shared Function MenuComunita(ByVal ComunitaID As Integer) As String
        Return String.Format(_MenuComunita, "_" & ComunitaID.ToString, "_", "")
    End Function
    Public Shared Function MenuComunita(ByVal ComunitaID As Integer, ByVal RuoloID As Integer) As String
        Return String.Format(_MenuComunita, "_" & ComunitaID.ToString, "_" & RuoloID.ToString, "_")
    End Function
    Public Shared Function MenuComunita(ByVal ComunitaID As Integer, ByVal RuoloID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_MenuComunita, "_" & ComunitaID.ToString, "_" & RuoloID.ToString, "_" & LinguaID.ToString)
    End Function

    Public Shared Function MenuPortale() As String
        Return String.Format(_MenuPortale, "", "", "")
    End Function
    Public Shared Function MenuPortale(ByVal isForAdmin As Boolean) As String
        Return String.Format(_MenuPortale, "_" & isForAdmin.ToString, "", "")
    End Function
    Public Shared Function MenuPortale(ByVal isForAdmin As Boolean, ByVal TipoPersonaID As Integer) As String
        Return String.Format(_MenuPortale, "_" & isForAdmin.ToString, "_" & TipoPersonaID.ToString, "_")
    End Function
    Public Shared Function MenuPortale(ByVal isForAdmin As Boolean, ByVal TipoPersonaID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_MenuPortale, "_" & isForAdmin.ToString, "_" & TipoPersonaID.ToString, "_" & LinguaID.ToString)
    End Function
#End Region

#Region "Header sistema"
    Public Shared Function NewsScorrevoli() As String
        Return String.Format(_NewsScorrevoli, "")
    End Function
    Public Shared Function NewsScorrevoli(ByVal LinguaID As Integer) As String
        Return String.Format(_NewsScorrevoli, "_" & LinguaID.ToString)
    End Function

    Public Shared Function ServiziComunita() As String
        Return String.Format(_ServiziComunita, "_", "", "")
    End Function
    Public Shared Function ServiziComunita(ByVal ComunitaID As Integer) As String
        Return String.Format(_ServiziComunita, "_" & ComunitaID.ToString, "_", "")
    End Function
    Public Shared Function ServiziComunita(ByVal ComunitaID As Integer, ByVal RuoloID As Integer) As String
        Return String.Format(_ServiziComunita, "_" & ComunitaID.ToString, "_" & RuoloID.ToString, "_")
    End Function
    Public Shared Function ServiziComunita(ByVal ComunitaID As Integer, ByVal RuoloID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_ServiziComunita, "_" & ComunitaID.ToString, "_" & RuoloID.ToString, "_" & LinguaID.ToString)
    End Function

    Public Shared Function Postit() As String
        Return String.Format(_Postit, "_", "", "")
    End Function
    Public Shared Function Postit(ByVal PersonaID As Integer) As String
        Return String.Format(_Postit, "_" & PersonaID.ToString, "_", "")
    End Function
    Public Shared Function Postit(ByVal PersonaID As Integer, ByVal ComunitaID As Integer) As String
        Return String.Format(_Postit, "_" & PersonaID.ToString, "_" & ComunitaID.ToString, "_")
    End Function
    Public Shared Function Postit(ByVal PersonaID As Integer, ByVal ComunitaID As Integer, ByVal oFiltro As Integer) As String
        Return String.Format(_Postit, "_" & PersonaID.ToString, "_" & ComunitaID.ToString, "_" & oFiltro.ToString)
    End Function
#End Region

    Public Shared Function ListaIScritti(ByVal ComunitaID As Integer) As String
        Return String.Format(_ListaIscritti, "_" & ComunitaID.ToString)
    End Function
    Public Shared Function ListaIScritti(ByVal ComunitaID As Integer, ByVal LinguaId As Integer) As String
        Return String.Format(_ListaIscritti, "_" & ComunitaID.ToString, "_" & LinguaId.ToString)
    End Function


    Public Shared Function DegreeType() As String
        Return String.Format(_DegreeType, "")
    End Function
    Public Shared Function DegreeType(ByVal LinguaID As Integer) As String
        Return String.Format(_DegreeType, "_" & LinguaID.ToString)
    End Function

    Public Shared Function TipiComunita() As String
        Return String.Format(_TipiComunita, "")
    End Function
    Public Shared Function TipiComunita(ByVal LinguaID As Integer) As String
        Return String.Format(_TipiComunita, "_" & LinguaID.ToString)
    End Function
    Public Shared Function TipiCorsoDiLaurea() As String
        Return String.Format(_TipiCorsoDiLaurea, "", "")
    End Function
    Public Shared Function TipiCorsoDiLaurea(ByVal LinguaID As Integer) As String
        Return String.Format(_TipiCorsoDiLaurea, "_" & LinguaID.ToString, "")
    End Function
    Public Shared Function TipiCorsoDiLaurea(ByVal LinguaID As Integer, ByVal FacoltaID As Integer) As String
        Return String.Format(_TipiCorsoDiLaurea, "_" & LinguaID.ToString, "_" & FacoltaID.ToString)
    End Function
    Public Shared Function Periodo() As String
        Return String.Format(_Periodo, "", "")
    End Function
    Public Shared Function Periodo(ByVal LinguaID As Integer) As String
        Return String.Format(_Periodo, "_" & LinguaID.ToString, "")
    End Function
    Public Shared Function Periodo(ByVal LinguaID As Integer, ByVal FacoltaID As Integer) As String
        Return String.Format(_Periodo, "_" & LinguaID.ToString, "_" & FacoltaID.ToString)
    End Function
    Public Shared Function Organizzazione() As String
        Return _Organizzazione
    End Function
    Public Shared Function OrganizzazioneList() As String
        Return _OrganizzazioneList
    End Function
    Public Shared Function TipoCorso() As String
        Return String.Format(_TipiCorso, "", "")
    End Function
    Public Shared Function TipoCorso(ByVal LinguaID As Integer) As String
        Return String.Format(_TipiCorso, "_" & LinguaID.ToString, "")
    End Function

    Public Shared Function OrganizzazioniAssociate() As String
        Return String.Format(_OrganizzazioniAssociate, "", "")
    End Function
    Public Shared Function OrganizzazioniAssociate(ByVal PersonaID As Integer) As String
        Return String.Format(_OrganizzazioniAssociate, "_" & PersonaID.ToString, "")
    End Function

    Public Shared Function PagineDefault() As String
        Return String.Format(_PagineDefault, "", "")
    End Function
    Public Shared Function PagineDefault(ByVal ComunitaID As Integer) As String
        Return String.Format(_PagineDefault, "_" & ComunitaID.ToString, "")
    End Function
    Public Shared Function PagineDefault(ByVal ComunitaID As Integer, ByVal LinguaID As Integer) As String
        Return String.Format(_PagineDefault, "_" & ComunitaID.ToString, "_" & LinguaID.ToString)
    End Function

#Region "Wiki New"
    Public Shared Function Wiki(ByVal ComunitaID As Integer) As String
        Return String.Format(_Wiki, "_" & ComunitaID.ToString)
    End Function
    Public Shared Function WikiSezioni(ByVal WikiID As String) As String
        Return String.Format(_WikiSezioni, "_" & WikiID)
    End Function
    Public Shared Function WikiSezioniHome() As String
        Return String.Format(_WikiSezioni, "_Home")
    End Function
    Public Shared Function WikiTopics(ByVal SezioneID As String) As String
        Return String.Format(_WikiTopics, "_" & SezioneID)
    End Function
    Public Shared Function WikiTopicsHistory(ByVal TopicID As String) As String
        Return String.Format(_WikiTopicHistory, "_" & TopicID)
    End Function
#End Region


End Class