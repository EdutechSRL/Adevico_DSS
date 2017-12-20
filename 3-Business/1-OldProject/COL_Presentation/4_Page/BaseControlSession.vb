Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comol.Manager
Imports COL_BusinessLogic_v2.Comol.Entities


Public MustInherit Class BaseControlSession
    Inherits System.Web.UI.UserControl
    Implements IViewCommon

    Private _Resource As ResourceManager
    Private _ServiziCorrenti As ServiziCorrenti
    Private _Instance As Boolean
    Protected _Language As Comol.Entity.Lingua
    Private _PageUtility As PresentationLayer.OLDpageUtility

    Protected ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property

    Public Sub New()
        Me._Instance = False
    End Sub

    Public Property PageIstance() As Boolean Implements IViewCommon.PageIstance
        Get
            PageIstance = _Instance
        End Get
        Set(ByVal value As Boolean)
            _Instance = value
        End Set
    End Property

    Protected ReadOnly Property Resource() As ResourceManager Implements IViewCommon.Resource
        Get
            Try
                If IsNothing(_Resource) Then
                    Me.SetCultureSettings()
                End If
                Resource = _Resource
            Catch ex As Exception
                Resource = New ResourceManager
            End Try
        End Get
    End Property

    Protected ReadOnly Property TipoComunitaCorrenteID() As Integer Implements IViewCommon.TipoComunitaCorrenteID
        Get
            Try
                TipoComunitaCorrenteID = Session("TPCM_ID")
            Catch ex As Exception
                TipoComunitaCorrenteID = Main.TipoComunitaStandard.Portale
            End Try
        End Get
    End Property

    Protected ReadOnly Property ComunitaCorrenteID() As Integer Implements IViewCommon.ComunitaCorrenteID
        Get
            Try
                ComunitaCorrenteID = Session("idComunita")
            Catch ex As Exception
                ComunitaCorrenteID = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property ComunitaCorrente() As COL_BusinessLogic_v2.Comunita.COL_Comunita Implements IViewCommon.ComunitaCorrente
        Get
            If ComunitaLavoroID > 0 Then
                Dim oComunita As New COL_Comunita
                oComunita.Id = Me.ComunitaLavoroID

                If TypeOf (Session("ComunitaCorrente")) Is COL_Comunita Then
                    If Session("ComunitaCorrente").id <> Me.ComunitaLavoroID Then
                        oComunita.EstraiByLingua(Me.LinguaID)
                        Session("ComunitaCorrente") = oComunita
                    End If
                Else
                    oComunita.EstraiByLingua(Me.LinguaID)
                    Session("ComunitaCorrente") = oComunita
                End If
                Return Session("ComunitaCorrente")
            Else
                Return New COL_Comunita(0)
            End If
            'If ComunitaCorrenteID > 0 Then
            '	Dim oComunita As New COL_Comunita
            '	oComunita.Id = Me.ComunitaCorrenteID

            '	If TypeOf (Session("ComunitaCorrente")) Is COL_Comunita Then
            '		If Session("ComunitaCorrente").id <> Me.ComunitaCorrenteID Then
            '			oComunita.EstraiByLingua(Me.LinguaID)
            '			Session("ComunitaCorrente") = oComunita
            '		End If
            '	Else
            '		oComunita.EstraiByLingua(Me.LinguaID)
            '		Session("ComunitaCorrente") = oComunita
            '	End If
            '	Return Session("ComunitaCorrente")
            'Else
            '	Return New COL_Comunita(0)
            'End If
        End Get
    End Property
    Public ReadOnly Property ComunitaLavoroID() As Integer Implements IViewCommon.ComunitaLavoroID
        Get
            If Me.isModalitaAmministrazione Then
                Return Me.AmministrazioneComunitaID
            Else
                Return Me.ComunitaCorrenteID
            End If
        End Get
    End Property
    Protected ReadOnly Property RuoloCorrenteID() As Integer Implements IViewCommon.RuoloCorrenteID
        Get
            Try
                RuoloCorrenteID = Session("IdRuolo")
            Catch ex As Exception
                RuoloCorrenteID = Main.TipoRuoloStandard.AccessoNonAutenticato
            End Try
        End Get
    End Property
    Public ReadOnly Property TipoPersonaID() As Integer Implements IViewCommon.TipoPersonaID
        Get
            Dim TipoID As Integer = Main.TipoPersonaStandard.Guest
            Try
                TipoID = Me.UtenteCorrente.TipoPersona.ID
            Catch ex As Exception
                TipoID = Main.TipoPersonaStandard.Guest
            End Try
            TipoPersonaID = TipoID
        End Get
    End Property
    Protected ReadOnly Property UtenteCorrente() As COL_Persona Implements IViewCommon.UtenteCorrente
        Get
            Try
                UtenteCorrente = Session("objPersona")
            Catch ex As Exception
                UtenteCorrente = Nothing
            End Try
        End Get
    End Property
    Protected ReadOnly Property IstituzioneID() As Integer Implements IViewCommon.IstituzioneID
        Get
            Try
                IstituzioneID = Session("ISTT_ID")
            Catch ex As Exception
                IstituzioneID = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property OrganizzazioneID() As Integer Implements IViewCommon.OrganizzazioneID
        Get
            Try
                OrganizzazioneID = Session("ORGN_id")
            Catch ex As Exception
                OrganizzazioneID = 0
            End Try
        End Get
    End Property
    Protected ReadOnly Property isPortalCommunity() As Boolean Implements IViewCommon.isPortalCommunity
        Get
            Dim isPortale As Boolean = True
            Try
                isPortale = Session("Limbo")
            Catch ex As Exception

            End Try
            If Not isPortale And ComunitaCorrenteID = 0 Then
                isPortalCommunity = False
            Else
                isPortalCommunity = isPortale
            End If

        End Get
    End Property
    Protected ReadOnly Property ElencoServizi() As ServiziCorrenti Implements IViewCommon.ElencoServizi
        Get
            If _Instance = False Then
                Me._ServiziCorrenti = New ServiziCorrenti
                Try
                    For i As Integer = 0 To UBound(Session("ArrPermessi"), 1)
                        _ServiziCorrenti.Add(Session("ArrPermessi")(i, 1), Session("ArrPermessi")(i, 0), Session("ArrPermessi")(i, 2))
                    Next
                Catch ex As Exception

                End Try
            Else
                _Instance = True
            End If
            ElencoServizi = _ServiziCorrenti
        End Get
    End Property
    Protected ReadOnly Property LinguaCode() As String Implements IViewCommon.LinguaCode
        Get
            If Me._Language Is Nothing Then
                Me.setLanguage()
            End If
            Return Me._Language.Codice
            ''Ricontrollare, va'...
            'Dim CodeLingua As String = "en-US"
            'Try
            '	If Session("LinguaCode") = "" Then
            '		Try
            '			LinguaCode = Request.UserLanguages(0)
            '		Catch ex As Exception
            '			LinguaCode = "en-US"
            '		End Try
            '		If Request.Browser.Cookies = True Then
            '			Try
            '				LinguaCode = Request.Cookies("LinguaCode").Value
            '			Catch ex As Exception
            '			End Try
            '		End If
            '	Else
            '		Try
            '			CodeLingua = Session("LinguaCode")
            '		Catch ex As Exception
            '		End Try
            '	End If
            'Catch ex As Exception
            '	LinguaCode = "en-US"
            'End Try
            'Return CodeLingua
        End Get
    End Property
    Protected ReadOnly Property LinguaID() As Integer Implements IViewCommon.LinguaID
        Get
            If Me._Language Is Nothing Then
                Me.setLanguage()
            End If
            Return Me._Language.ID
            'Try
            '	If IsNumeric(Session("LinguaID")) Then
            '		LinguaID = CInt(Session("LinguaID"))
            '	Else
            '		LinguaID = 1
            '	End If
            'Catch ex As Exception
            '	LinguaID = 1
            'End Try
            'Return LinguaID
        End Get
    End Property
    Public ReadOnly Property UserSessionLanguage() As Lingua Implements IViewCommon.UserSessionLanguage
        Get
            Return _Language ' New Lingua(Me.LinguaID, Me.LinguaCode)
        End Get
    End Property

    Private Sub setLanguage()
        Dim oLingua As New Lingua
        If Not String.IsNullOrEmpty(Session("LinguaCode")) Then
            Try
                oLingua = New Lingua(CInt(Session("LinguaID")), Session("LinguaCode"))
            Catch ex As Exception

            End Try
        Else
            oLingua = ManagerLingua.GetByCodeOrDefault(Me.LinguaCode)
            If IsNothing(oLingua) Then
                oLingua = SystemSettings.DefaultLanguage
            End If
            Session("LinguaID") = oLingua.ID
            Session("LinguaCode") = oLingua.Codice
        End If
        _Language = oLingua
    End Sub
    Public Sub OverloadLanguage(ByVal oLingua As Lingua)
        Session("LinguaID") = oLingua.ID
        Session("LinguaCode") = oLingua.Codice
        Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = LinguaID, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}
        _Language = oLingua
    End Sub
    Protected Property NewLinguaID() As Integer Implements IViewCommon.NewLinguaID
        Get
            Try
                If IsNumeric(Session("NewLinguaID")) Then
                    NewLinguaID = CInt(Session("NewLinguaID"))
                Else
                    NewLinguaID = 0
                End If
            Catch ex As Exception
                NewLinguaID = 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Session("NewLinguaID") = value
        End Set
    End Property

    Public ReadOnly Property RequireSSL() As Boolean Implements IViewCommon.RequireSSL
        Get
            Dim RichiediSSL As Boolean = False
            Try
                RequireSSL = SystemSettings.Login.isSSLrequired
            Catch ex As Exception
                RequireSSL = False
            End Try
        End Get
    End Property
    Public ReadOnly Property BaseUrl() As String Implements IViewCommon.BaseUrl
        Get
            Dim url As String = Me.Request.ApplicationPath
            If url.EndsWith("/") Then
                Return url
            Else
                Return url + "/"
            End If
        End Get
    End Property
    Public ReadOnly Property FullBaseUrl() As String Implements IViewCommon.FullBaseUrl
        Get
            Return Me.Request.Url.AbsoluteUri.Replace( _
             Me.Request.Url.PathAndQuery, "") + Me.BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPage() As String Implements IViewCommon.CurrentPage
        Get
            Try
                Return Me.Request.Url.ToString()
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Public ReadOnly Property BaseUrlDrivePath() As String Implements IViewCommon.BaseUrlDrivePath
        Get
            Return Server.MapPath(Me.BaseUrl)
        End Get
    End Property
    Public Sub RedirectToUrl(ByVal Url As String) Implements IViewCommon.RedirectToUrl
        Dim Redirect As String = "http"

        If RequireSSL Then
            Redirect &= "s://" & Me.Request.Url.Host & Me.BaseUrl & Url
        Else
            Redirect &= "://" & Me.Request.Url.Host & Me.BaseUrl & Url
        End If
        Response.Redirect(Redirect, True)
    End Sub
    Public Sub RedirectToLoginUrl(ByVal Url As String) Implements IViewCommon.RedirectToLoginUrl
        Dim Redirect As String = "http"

        If RequireSSL Or Me.SystemSettings.Login.isSSLloginRequired Then
            Redirect &= "s://" & Me.Request.Url.Host & Me.BaseUrl & Url
        Else
            Redirect &= "://" & Me.Request.Url.Host & Me.BaseUrl & Url
        End If
        Response.Redirect(Redirect, True)
    End Sub
    Public Sub RedirectToEncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType) Implements IViewCommon.RedirectToEncryptedUrl
        Response.Redirect(Me.EncryptedUrl(UrlPage, UrlQuerystring, oTypeEnc), True)
    End Sub
    Public Function EncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String Implements IViewCommon.EncryptedUrl
        Dim Redirect As String = "http"

        If RequireSSL Then
            Redirect &= "s://"
        Else
            Redirect &= "://"
        End If
        Redirect &= Me.Request.Url.Host & Me.BaseUrl & UrlPage & "?" & Me.CryptQuerystring(UrlQuerystring, oTypeEnc)

        Return Redirect
    End Function
    Public Function EncryptedQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String Implements IViewCommon.EncryptedQueryString
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        If Enc.DecryptVerifyURL(Request.QueryString) Then
            Return Enc.Querystring(Value)
        Else
            Return ""
        End If
    End Function
    Public Function DecryptQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String Implements IViewCommon.DecryptQueryString
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        If Enc.DecryptVerifyURL(Request.QueryString) Then
            Return Enc.Querystring(Value)
        Else
            Return ""
        End If
    End Function

#Region "ArmoredQueryString"
    Private Function CryptQuerystring(ByVal data As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        Return Enc.ArmorURL(data)
    End Function

    Private Function DecryptVerifyQuerystring(ByVal oTypeEnc As SecretKeyUtil.EncType) As Boolean
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        Return Enc.DecryptVerifyURL(Request.QueryString)
    End Function

#End Region
    'Public Property Impostazioni() As COL_ImpostazioniUtente Implements IViewCommon.Impostazioni
    '    Get
    '        Try
    '            If IsNothing(Session("oImpostazioni")) Then
    '                Impostazioni = Nothing
    '            Else
    '                Impostazioni = Session("oImpostazioni")
    '            End If
    '        Catch ex As Exception
    '            Impostazioni = Nothing
    '        End Try
    '    End Get
    '    Set(ByVal value As COL_ImpostazioniUtente)
    '        Session("oImpostazioni") = value
    '    End Set
    'End Property


    Public Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String) Implements IViewCommon.SetCulture
        Me._Resource = New ResourceManager

        Me._Resource.UserLanguages = Me.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.setCulture()
    End Sub
    Public Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String) Implements IViewCommon.SetCulture
        Me._Resource = New ResourceManager
        Me._Resource.UserLanguages = Me.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.Folder_Level2 = ResourceFolderLevel2
        Me._Resource.setCulture()
    End Sub
    Public Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String, ByVal ResourceFolderLevel3 As String) Implements IViewCommon.SetCulture
        Me._Resource = New ResourceManager
        Me._Resource.UserLanguages = Me.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.Folder_Level2 = ResourceFolderLevel2
        Me._Resource.Folder_Level3 = ResourceFolderLevel3
        Me._Resource.setCulture()
    End Sub

    Public ReadOnly Property isNonIscrittoComunita() As Boolean Implements IViewCommon.isNonIscrittoComunita
        Get
            Try
                If IsNothing(Me.UtenteCorrente) Then
                    isNonIscrittoComunita = True
                ElseIf Me.TipoPersonaID = Main.TipoPersonaStandard.Guest Then
                    isNonIscrittoComunita = True
                ElseIf Me.RuoloCorrenteID = Main.TipoRuoloStandard.AccessoNonAutenticato Then
                    isNonIscrittoComunita = True
                Else
                    isNonIscrittoComunita = False
                End If
            Catch ex As Exception
                isNonIscrittoComunita = True
            End Try
        End Get

    End Property

    Public ReadOnly Property isUtenteAnonimo() As Boolean Implements IViewCommon.isUtenteAnonimo
        Get
            Try
                If IsNothing(Me.UtenteCorrente) Then
                    isUtenteAnonimo = True
                ElseIf Me.TipoPersonaID = Main.TipoPersonaStandard.Guest Then
                    isUtenteAnonimo = True
                Else
                    isUtenteAnonimo = False
                End If
            Catch ex As Exception
                isUtenteAnonimo = True
            End Try
        End Get
    End Property

    Public Property AmministrazioneComunitaID() As Integer Implements IViewCommon.AmministrazioneComunitaID
        Get
            Try
                If Not IsNumeric(Session("idComunita_forAdmin")) Then
                    Session("idComunita_forAdmin") = -1
                End If
            Catch ex As Exception
                Session("idComunita_forAdmin") = -1
            End Try
            AmministrazioneComunitaID = DirectCast(Session("idComunita_forAdmin"), Integer)
        End Get
        Set(ByVal value As Integer)
            Session("idComunita_forAdmin") = value
        End Set
    End Property

    Public Property isModalitaAmministrazione() As Boolean Implements IViewCommon.isModalitaAmministrazione
        Get
            Try
                If Not CBool(Session("AdminForChange")) Then
                    Session("AdminForChange") = False
                End If
            Catch ex As Exception
                Session("AdminForChange") = False
            End Try
            isModalitaAmministrazione = Session("AdminForChange")
        End Get
        Set(ByVal value As Boolean)
            Session("AdminForChange") = value
        End Set
    End Property

    Public Sub GoToPortale() Implements IViewCommon.GoToPortale
        Dim LinkElenco As String = Me.SystemSettings.Presenter.DefaultHomeHeaderLink

        If LinkElenco = "" Then
            LinkElenco = "Comunita/EntrataComunita.aspx"
        End If
        GoToPortale(LinkElenco)
    End Sub
    Public Sub GoToPortale(ByVal url As String) Implements IViewCommon.GoToPortale
        Session("IdCmntPadre") = 0
        Session("limbo") = True
        Session("ArrComunita") = Nothing
        Session("IdComunita") = 0
        Session("IdRuolo") = Nothing
        Session("ORGN_id") = Nothing
        Session("RLPC_id") = Nothing
        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("CMNT_path_forNews") = ""
        Session("CMNT_ID_forNews") = ""
        Session("TPCM_ID") = -1

        Me.RedirectToUrl(url)
    End Sub

    Public MustOverride Sub SetCultureSettings() Implements IViewCommon.SetCultureSettings
    Public MustOverride Sub SetInternazionalizzazione() Implements IViewCommon.SetInternazionalizzazione
    Public MustOverride Sub BindDati() Implements IViewCommon.BindDati

    Public Sub SetCookies(ByVal LinguaID As Integer, ByVal LinguaCode As String) Implements IViewCommon.SetCookies
        Dim oBrowser As System.Web.HttpBrowserCapabilities
        oBrowser = Request.Browser

        If oBrowser.Cookies Then
            Dim oCookie_ID As New System.Web.HttpCookie("LinguaID", LinguaID.ToString)
            Dim oCookie_Code As New System.Web.HttpCookie("LinguaCode", LinguaCode)

            oCookie_ID.Expires = Now.AddYears(1)
            oCookie_Code.Expires = Now.AddYears(1)

            Me.Response.Cookies.Add(oCookie_ID)
            Me.Response.Cookies.Add(oCookie_Code)
        End If
    End Sub


    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.SetCultureSettings()
    End Sub


    Protected Sub ExitToLimbo()
        Session("ORGN_id") = 0
        Session("IdRuolo") = ""
        Session("ArrPermessi") = ""
        Session("RLPC_ID") = ""

        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("TPCM_ID") = ""
        Me.Response.Expires = 0
        Me.Response.Redirect(BaseUrl & "EntrataComunita.aspx", True)
    End Sub



    Public Overridable Function IsSessioneScaduta() As Boolean
        Dim isScaduta As Boolean = True

        If Not IsNothing(Me.UtenteCorrente) Then
            If Me.UtenteCorrente.ID > 0 Then
                isScaduta = False
            End If
        End If
        If isScaduta Then
            Dim alertMSG As String
            alertMSG = Me.Resource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            isScaduta = True
        ElseIf Me.isPortalCommunity And Me.ComunitaCorrenteID > 0 Then
            Me.ExitToLimbo()
            isScaduta = True
        End If
        Return isScaduta
    End Function

    Public Sub SetElencoServizi(ByVal oLista As COL_BusinessLogic_v2.ServiziCorrenti) Implements IViewCommon.SetElencoServizi
        Me._ServiziCorrenti = oLista
    End Sub

    Public ReadOnly Property AccessoSistema() As Boolean Implements IViewCommon.AccessoSistema
        Get
            Try
                AccessoSistema = Me.Application("SystemAcess")
            Catch ex As Exception
                AccessoSistema = True
            End Try
        End Get
        'Set(ByVal value As Boolean)
        '    Application.Lock()
        '    Me.Application.Add("SystemAcess", value)
        '    Application.UnLock()
        'End Set
    End Property

    Public ReadOnly Property SystemSettings() As ComolSettings Implements IViewCommon.SystemSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance '(Me.LinguaCode, Me.BaseUrlDrivePath, BaseUrl)
        End Get
    End Property

    Public ReadOnly Property LocalizedMail() As MailLocalized Implements IViewCommon.LocalizedMail
        Get
            Try
                'If LanguageCode = "" Then
                Return ManagerConfiguration.GetMailLocalized(Me.UserSessionLanguage)
                'Else
                'Return SystemSettings.Mail.Localized(SystemConfiguration.GetLocalizedConfigurations(LanguageCode))
                'End If

            Catch ex As Exception

            End Try
            Return Nothing
        End Get
    End Property

    Public Sub CambiaImpostazioniLingua(ByVal LinguaID As Integer, Optional ByVal LingaCode As String = "", Optional ByVal ReloadCurrentUser As Boolean = False) Implements IViewCommon.CambiaImpostazioniLingua
        Session("LinguaID") = LinguaID
        If LingaCode = "" Then
            Dim oLingua As New Lingua
            oLingua = ManagerLingua.GetByID(LinguaID)
            Session("LinguaCode") = oLingua.Codice
            Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = LinguaID, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}
        Else
            Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = LinguaID, .Code = LingaCode}
            Session("LinguaCode") = LingaCode
        End If

        Me.SetCookies(LinguaID, LingaCode)
        If ReloadCurrentUser Then
            Dim oPersona As New COL_Persona
            oPersona.ID = Me.UtenteCorrente.ID
            oPersona.Estrai(LinguaID)
            If oPersona.Errore = Errori_Db.None Then
                Session("objPersona") = oPersona
            End If
        End If
        Session("NewLinguaID") = 0
    End Sub

    Public ReadOnly Property ObjectPath(ByVal oPath As ConfigurationPath) As ObjectFilePath Implements IViewCommon.ObjectPath
        Get
            Dim oObjectPath As New ObjectFilePath(oPath.isOnThisServer)

            Try
                If oPath.isOnThisServer Then
                    oObjectPath.Virtual = Me.BaseUrl & oPath.VirtualPath ' Me.BaseUrl & "/" & oPath.VirtualPath
                    oObjectPath.Virtual = Replace(oObjectPath.Virtual, "//", "/")
                    If oPath.DrivePath <> "" Then
                        oObjectPath.Drive = oPath.DrivePath 'Me.BaseUrlDrivePath & oPath.DrivePath
                    Else
                        oObjectPath.Drive = Me.BaseUrlDrivePath & oPath.VirtualPath
                    End If
                    oObjectPath.Drive = Replace(oObjectPath.Drive, "/", "\")
                    oObjectPath.Drive = Replace(oObjectPath.Drive, "\\", "\")
                Else
                    If oPath.ServerVirtualPath <> "" Then
                        oObjectPath.Virtual = oPath.ServerVirtualPath & oPath.VirtualPath
                    End If
                    'oObjectPath.Virtual = oPath.ServerVirtualPath & oPath.VirtualPath 'oPath.ServerVirtualPath & "/" & oPath.VirtualPath
                    If oPath.DrivePath <> "" Then
                        oObjectPath.Drive = oPath.ServerPath & oPath.DrivePath
                    Else
                        oObjectPath.Drive = oPath.ServerPath & oPath.VirtualPath
                    End If
                    oObjectPath.SharePath = oPath.ServerPath
                End If
                oObjectPath.isOnShare = Not oPath.isOnThisServer
                oObjectPath.RewritePath = oPath.RewritePath
            Catch ex As Exception

            End Try
            Return oObjectPath
        End Get
    End Property

    Public ReadOnly Property ApplicationUrlBase(Optional ByVal WithoutSSLfromConfig As Boolean = False) As String Implements IViewCommon.ApplicationUrlBase
        Get
            Dim Redirect As String = "http"

            If RequireSSL And Not WithoutSSLfromConfig Then
                Redirect &= "s://" & Me.Request.Url.Host & Me.BaseUrl
            Else
                Redirect &= "://" & Me.Request.Url.Host & Me.BaseUrl
            End If
            ApplicationUrlBase = Redirect
        End Get
    End Property
    Public ReadOnly Property DefaultUrl() As String Implements IViewCommon.DefaultUrl
        Get
            If (String.IsNullOrEmpty(SystemSettings.Presenter.FullDefaultStartPage)) Then
                Return (Me.ApplicationUrlBase & Me.SystemSettings.Presenter.DefaultStartPage)
            Else
                Return SystemSettings.Presenter.FullDefaultStartPage
            End If
        End Get
    End Property

    Public Sub RedirectToDefault(Optional ByVal QueryParameters As String = "") Implements IViewCommon.RedirectToDefault
        If QueryParameters = "" Then
            Response.Redirect(Me.DefaultUrl, True)
        Else
            Response.Redirect(Me.DefaultUrl & QueryParameters, True)
        End If
    End Sub

    Public Function CanRedirectToDefaultPage(ByVal Codice As String, ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer) As Boolean
        Dim Redirigi As Boolean = False
        If Codice = Services_Bacheca.Codex Then

            Try
                Dim oServizio As New Services_Bacheca(COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice))
                If oServizio.Admin Or oServizio.Read Or oServizio.Write Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_CHAT.Codex Then
            Dim oServizio As New Services_CHAT
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.Admin Or oServizio.Read Or oServizio.Write Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_DiarioLezioni.Codex Then
            Dim oServizio As New Services_DiarioLezioni
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.Admin Or oServizio.Read Or oServizio.Change Or oServizio.Upload Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_Eventi.Codex Then
            Dim oServizio As New Services_Eventi
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.AdminService Or oServizio.AddEvents Or oServizio.ChangeEvents Or oServizio.ReadEvents Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_File.Codex Then
            Dim oServizio As New Services_File
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.Admin Or oServizio.Moderate Or oServizio.Read Or oServizio.Upload Or oServizio.Change Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_Forum.Codex Then
            Dim oServizio As New Services_Forum
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.AccessoForum Or oServizio.GestioneForum Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_Gallery.Codex Then
            Dim oServizio As New Services_Gallery
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.Admin Or oServizio.Management Or oServizio.List Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_RaccoltaLink.Codex Then
            Dim oServizio As New Services_RaccoltaLink
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.Admin Or oServizio.Moderate Or oServizio.List Or oServizio.AddLink Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_Statistiche.Codex Then
            Dim oServizio As New Services_Statistiche
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.Admin Or oServizio.List Or oServizio.Management Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_Listaiscritti.Codex Then
            Dim oServizio As New Services_Listaiscritti
            Try
                oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                If oServizio.Management Or oServizio.List Or oServizio.Admin Then
                    Redirigi = True
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_Cover.Codex Then
            Dim oServizio As New Services_Cover
            Try
                Dim oRuoloComunita As New COL_BusinessLogic_v2.Comunita.COL_RuoloPersonaComunita
                If Not COL_RuoloPersonaComunita.isSkipCover(CMNT_ID, PRSN_ID) Then
                    oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                    If oServizio.Management Or oServizio.Read Or oServizio.Admin Then
                        Redirigi = True
                    End If
                End If
            Catch ex As Exception

            End Try
        ElseIf Codice = Services_Wiki.Codex Then
            Dim oServizio As New Services_Wiki
            Try
                Dim oRuoloComunita As New COL_BusinessLogic_v2.Comunita.COL_RuoloPersonaComunita
                If Not COL_RuoloPersonaComunita.isSkipCover(CMNT_ID, PRSN_ID) Then
                    oServizio.PermessiAssociati = COL_Comunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
                    If oServizio.Admin Or oServizio.Lettura Or oServizio.GestioneWiki Then
                        Redirigi = True
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
        Return Redirigi
    End Function

    Public Function PlainRedirectToDefaultPage(ByVal ComunitaID As Integer, ByVal PersonaID As Integer) As String
        Dim urlDefault As String = "", Codice As String = ""
        Dim DefaultPageID As Integer
        Dim hasDefaultPage As Boolean = False
        Dim urlRedirect As String = "Comunita/comunita.aspx"

        Try
            hasDefaultPage = COL_Comunita.GetDefaultPage(PersonaID, urlDefault, Codice, DefaultPageID)
            If hasDefaultPage AndAlso urlDefault <> "" Then
                Dim Redirigi As Boolean = False
                Redirigi = CanRedirectToDefaultPage(Codice, PersonaID, PersonaID)
                If Redirigi Then
                    urlDefault = Replace(urlDefault, "./", "")
                    urlRedirect = urlDefault
                End If
            End If
        Catch ex As Exception

        End Try

        Return urlRedirect
    End Function

    Protected Property UniqueGuidSession() As Guid
        Get
            Return Me.PageUtility.UniqueGuidSession
        End Get
        Set(ByVal value As Guid)
            Me.PageUtility.UniqueGuidSession = value
        End Set
    End Property

    Public ReadOnly Property CurrentModuleID() As Integer Implements IViewCommon.CurrentModuleID
        Get
            Return PageUtility.CurrentModule.ID
        End Get
    End Property

    'Public Sub AddAction###(ByVal oService As lm.ActionDataContract.iRemoteService, ByVal oType As Integer, ByVal oObjectActions As System.Collections.Generic.List(Of lm.ActionDataContract.ObjectAction), Optional ByVal TypeIteration As lm.ActionDataContract.InteractionType = lm.ActionDataContract.InteractionType.Generic) Implements IViewCommon.AddAction
    '    Me.PageUtility.AddAction###(oService, oType, oObjectActions, TypeIteration)
    'End Sub

    'Public Sub AddActionToModule(ByVal oService As lm.ActionDataContract.iRemoteService, ByVal ModuleID As Integer, ByVal oType As Integer, ByVal oObjectActions As System.Collections.Generic.List(Of lm.ActionDataContract.ObjectAction), Optional ByVal TypeIteration As lm.ActionDataContract.InteractionType = lm.ActionDataContract.InteractionType.Generic) Implements IViewCommon.AddActionToModule
    '    Me.PageUtility.AddActionToModule(oService,ModuleID, oType, oObjectActions, TypeIteration)
    'End Sub

    Public Sub WriteLogoutAccessCookie(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostPage As String, ByVal ForDownload As Boolean)
        Dim oHttpCookie As New HttpCookie("LogoutAccess")
        Dim minutes As Long = Me.SystemSettings.BlogSettings.ValidationTime
        oHttpCookie.Expires = Now.AddMinutes(minutes)
        oHttpCookie.Values.Add("PersonID", PersonID)
        oHttpCookie.Values.Add("PersonLogin", PersonLogin)
        oHttpCookie.Values.Add("PostPage", Replace(PostPage, "&", "#_#"))
        oHttpCookie.Values.Add("Download", ForDownload)
        oHttpCookie.Values.Add("CommunityID", CommunityID)
        If (From k In Response.Cookies.AllKeys Where k = "LogoutAccess").Any Then
            Response.Cookies.Set(oHttpCookie)
        Else
            Response.Cookies.Add(oHttpCookie)
        End If

        'Dim secured As HttpCookie
        'Dim domain As String = Me.SystemSettings.BlogSettings.DomainCookie
        'Dim minutes As Long = Me.SystemSettings.BlogSettings.ValidationTime
        'Dim hash As New Hashtable()
        'hash.Add("PersonID", PersonID)
        'hash.Add("PersonLogin", PersonLogin)
        'hash.Add("PostPage", PostPage)
        'hash.Add("Download", ForDownload)
        'hash.Add("CommunityID", CommunityID)
        'hash.Add("expire", Now.AddMinutes(minutes))
        'hash.Add("domain", domain)
        'secured = SecuredCookie.encode_cookie("LogoutAccess", domain, minutes, hash)

        'If (From k In Response.Cookies.AllKeys Where k = "LogoutAccess").Any Then
        '    Response.Cookies.Set(secured)
        'Else
        '    Response.Cookies.Add(secured)
        'End If
    End Sub
    Public Function ReadLogoutAccessCookie(ByVal PersonID As Integer, ByVal PersonLogin As String) As dtoLogoutAccess
        Dim iResponse As New dtoLogoutAccess
        Dim oValues As New Hashtable
        Try
            For Each key As String In Request.Cookies("LogoutAccess").Values.Keys
                oValues(key) = Request.Cookies("LogoutAccess").Values(key)
            Next
            '    SecuredCookie.decode_cookie(Request.Cookies("LogoutAccess"))
        Catch ex As Exception

        End Try
        If Not IsNothing(oValues) Then
            iResponse.CommunityID = oValues.Item("CommunityID")
            iResponse.PersonID = oValues.Item("PersonID")
            iResponse.PersonLogin = oValues.Item("PersonLogin")
            iResponse.isDownloading = oValues.Item("Download")
            If (iResponse.PersonID > 0 AndAlso (iResponse.PersonID = PersonID AndAlso iResponse.PersonLogin = PersonLogin)) OrElse iResponse.PersonID <= 0 Then
                If Not String.IsNullOrEmpty(oValues.Item("PostPage")) Then
                    iResponse.PageUrl = Replace(oValues.Item("PostPage"), "#_#", "&")
                End If
            Else
                iResponse.PageUrl = ""
            End If
        End If
        Return iResponse
    End Function
    Public Sub ClearLogoutAccessCookie()
        Response.Cookies("LogoutAccess").Expires = Now.AddDays(-1)
    End Sub
    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
End Class