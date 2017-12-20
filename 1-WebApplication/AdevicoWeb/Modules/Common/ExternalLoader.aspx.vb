Imports lm.Comol.Core.BaseModules.ModulesLoader.Presentation
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.Authentication

Public Class ExternalLoader
    Inherits PageBase
    Implements IViewExternalModuleLoader


#Region "MVP"
    Private _Presenter As ExternalModuleLoaderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentPresenter() As ExternalModuleLoaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ExternalModuleLoaderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region
#Region "Implements"
    Private ReadOnly Property PreLoadedExternalID As String Implements IViewExternalModuleLoader.PreLoadedExternalID
        Get
            Return Request.QueryString("ExternalID")
        End Get
    End Property

    Private ReadOnly Property PreLoadedExternalSource As String Implements IViewExternalModuleLoader.PreLoadedExternalSource
        Get
            Return Request.QueryString("Source")
        End Get
    End Property

    Private ReadOnly Property PreLoadedServiceCode As String Implements IViewExternalModuleLoader.PreLoadedModuleCode
        Get
            Return Request.QueryString("Service")
        End Get
    End Property

    Private ReadOnly Property PreloadedServicePage As String Implements IViewExternalModuleLoader.PreloadedModulePage
        Get
            Return Request.QueryString("Page")
        End Get
    End Property
    Public ReadOnly Property PortalHome As String Implements IViewExternalModuleLoader.PortalHome
        Get
            Return Resource.getValue("PortalHome")
        End Get
    End Property

   

    Public ReadOnly Property PreloadedPreviousUrl As String Implements IViewExternalModuleLoader.PreloadedPreviousUrl
        Get
            If IsNothing(Request.UrlReferrer) Then
                Return ""
            Else
                Return Request.UrlReferrer.AbsoluteUri
            End If
        End Get
    End Property

    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

#Region "Inherits"

    Public Overrides Sub BindDati()
        Me.Master.ShowHelp = False
        Me.Master.ShowLanguage = False
        Me.LBmessage.Text = Resource.getValue("Loading")
        Me.IMGloading.Visible = True
        ' Me.UDPinfoLogon.Update()
        Me.CurrentPresenter.InitView(Now)
    End Sub
    Public Overrides Sub BindNoPermessi()
        'DirectCast(Me.Master, ExternalService).ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ExternalLoader", "Modules")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            '   DirectCast(Me.Master, Authentication). = .getValue("serviceTitle")
            'DirectCast(Me.Master, ExternalService).ServiceNopermission = .getValue("nopermission")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Function GetEncodedIdUser(urlIdentifier As String) As String Implements IViewExternalModuleLoader.GetEncodedIdUser
        Return Me.Request.QueryString(urlIdentifier)
    End Function
#End Region

    Public Sub LoadUserLanguage(IdLanguage As Integer, code As String) Implements IViewExternalModuleLoader.LoadUserLanguage
        PageUtility.LinguaCode = code
        PageUtility.LinguaID = IdLanguage
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()

    End Sub

    Public Sub LoadModule(IdCommunity As Integer, destinationUrl As String) Implements IViewExternalModuleLoader.LoadModule
        LoadModule(Me.UtenteCorrente, IdCommunity, destinationUrl)
    End Sub
    Public Sub LoadModuleWithLogon(person As lm.Comol.Core.DomainModel.Person, IdCommunity As Integer, destinationUrl As String) Implements IViewExternalModuleLoader.LoadModuleWithLogon
        Dim persona As COL_Persona = LogonUser(person)
        LoadModule(persona, IdCommunity, destinationUrl)
    End Sub

    Private Sub LoadModule(persona As COL_Persona, IdCommunity As Integer, destinationUrl As String)
        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(Me.LinguaCode)
        If IdCommunity = 0 Then
            Me.LogonIntoPortalCommunity(persona)
        ElseIf String.IsNullOrEmpty(destinationUrl) Then
            Me.PageUtility.AccessToCommunity(persona.ID, IdCommunity, oResourceConfig, True)
        Else
            Dim cv As String = "CV=" & (lm.Comol.Core.DomainModel.ContentView.hideHeader Or lm.Comol.Core.DomainModel.ContentView.hideFooter)
            Me.PageUtility.AccessToCommunity(persona.ID, IdCommunity, oResourceConfig, IIf(destinationUrl.Contains("?"), destinationUrl & "&" & cv, destinationUrl & "?" & cv), True)
        End If
        'Me.IMGloading.Visible = False
        'Me.UDPinfoLogon.Update()
    End Sub
    Private Function LogonUser(person As lm.Comol.Core.DomainModel.Person) As COL_Persona
        Dim persona As COL_Persona = COL_Persona.GetPersona(person.Id, person.LanguageID)
        Session("IdRuolo") = ""
        Session("objPersona") = persona
        Session("ORGN_id") = persona.GetOrganizzazioneDefault
        Session("Istituzione") = persona.GetIstituzione
        Session("IdComunita") = 0
        Session("ArrPermessi") = ""
        Session("ArrComunita") = ""
        Session("RLPC_ID") = ""
        Session("CMNT_path_forAdmin") = ""

        Me.PageUtility.isPortalCommunity = True
        Me.PageUtility.isModalitaAmministrazione = False
        Me.PageUtility.AmministrazioneComunitaID = 0

        Try
            Dim oLingua As Lingua = ManagerLingua.GetByID(persona.Lingua.ID)

            Me.NewLinguaID = 0
            Session("LinguaID") = oLingua.ID
            Session("LinguaCode") = oLingua.Codice
            Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = oLingua.ID, .Icon = oLingua.Icona, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}
            Me.SetCookies(Session("LinguaID"), Session("LinguaCode"))
        Catch ex As Exception
            Session("LinguaID") = 1
            Session("LinguaCode") = "it-IT"
            Me.SetCookies(Session("LinguaID"), Session("LinguaCode"))
        End Try
        Return persona
    End Function


    Public Sub LogonIntoPortalCommunity(oPersona As COL_Persona)


        'Aggiornamento file XML

        Dim oTreeComunita As New COL_BusinessLogic_v2.Comunita.COL_TreeComunita
        Dim PRSN_ID As Integer
        Try
            PRSN_ID = oPersona.ID
            oTreeComunita.Directory = PageUtility.ProfilePath & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"
            oTreeComunita.AggiornaInfo(PRSN_ID, Session("LinguaID"), Me.PageUtility.SystemSettings.CodiceDB, True)

            'oTreeComunita.FindCommunityPath(PRSN_ID)
        Catch ex As Exception

        End Try

        'Memorizzo impostazioni utente
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Try
            oImpostazioni.Directory = Server.MapPath(Me.PageUtility.BaseUrl & "profili/") & PRSN_ID & "\"
            oImpostazioni.Nome = "settings_" & PRSN_ID & ".xml"
            If oImpostazioni.Exist Then
                oImpostazioni.RecuperaImpostazioni()
                Session("oImpostazioni") = oImpostazioni
            Else
                Session("oImpostazioni") = Nothing
            End If
        Catch ex As Exception
            Session("oImpostazioni") = Nothing
        End Try

        Try
            oPersona.RegistraAccesso(Me.SystemSettings.CodiceDB)
        Catch ex As Exception

        End Try

        PageUtility.CurrentModule = Nothing
        Me.PageUtility.AddLoginAction()

        'Dim LinkElenco As String = ""
        'Try
        '    If oImpostazioni.Visualizza_Iscritto = Main.ElencoRecord.Normale Then
        '        LinkElenco = "Comunita/EntrataComunita.aspx"
        '    Else
        '        LinkElenco = "Comunita/NavigazioneTreeView.aspx"
        '    End If
        'Catch ex As Exception
        '    LinkElenco = "Comunita/EntrataComunita.aspx"
        'End Try
        Dim LinkElenco As String = Me.SystemSettings.Presenter.DefaultLogonPage
        If LinkElenco = "" Then
            Try
                If oImpostazioni.Visualizza_Iscritto = Main.ElencoRecord.Normale Then
                    LinkElenco = "Comunita/EntrataComunita.aspx"
                Else
                    LinkElenco = "Comunita/NavigazioneTreeView.aspx"
                End If
            Catch ex As Exception
                LinkElenco = "Comunita/EntrataComunita.aspx"
            End Try
        End If
        WriteCookie(oPersona)

        Dim oResult As dtoLogoutAccess = Me.ReadLogoutAccessCookie(oPersona.ID, oPersona.Login)
        If Not IsNothing(oResult) AndAlso oResult.PageUrl <> "" Then
            If oResult.isDownloading Then
                Me.RedirectToUrl(LinkElenco)
            Else
                If oResult.CommunityID > 0 Then
                    '    Me.re()
                End If
                Me.ClearLogoutAccessCookie()
                Me.RedirectToUrl(oResult.PageUrl)
            End If
        Else
            Me.RedirectToUrl(LinkElenco)
        End If
    End Sub
    Public Sub WriteCookie(ByVal oPersona As COL_Persona)
        'Cookie Login Blog
        Dim secured As HttpCookie

        Dim userid = oPersona.ID
        Dim username = oPersona.Login

        Dim domain As String = "comol.local"
        Dim minutes As Long = 5
        Dim hash As New Hashtable()
        hash.Add("userId", userid)
        hash.Add("userName", username)
        hash.Add("expire", Now.AddMinutes(minutes))
        hash.Add("domain", domain)

        secured = SecuredCookie.encode_cookie("login", domain, minutes, hash)

        Response.Cookies.Add(secured)
    End Sub

#Region "messages"
    Public Sub LoadUnknowCommunity() Implements IViewExternalModuleLoader.LoadUnknowCommunity
        Me.LBmessage.Text = Resource.getValue("LoadUnknowCommunity")
        Me.IMGloading.Visible = False
        '  Me.UDPinfoLogon.Update()
    End Sub

    Public Sub LoadUnsubscribedCommunity(communityName As String) Implements IViewExternalModuleLoader.LoadUnsubscribedCommunity
        Me.LBmessage.Text = String.Format(Resource.getValue("LoadUnsubscribedCommunity"), communityName)
        Me.IMGloading.Visible = False
        '   Me.UDPinfoLogon.Update()
    End Sub

    Public Sub LoadWaitingMessage(person As lm.Comol.Core.DomainModel.Person, communityName As String) Implements IViewExternalModuleLoader.LoadWaitingMessage
        Me.LBmessage.Text = String.Format(Resource.getValue("LoadWaitingMessage"), person.Name, communityName)
        Me.IMGloading.Visible = True
        '  Me.UDPinfoLogon.Update()
    End Sub
    Public Sub ShowAuthenticationResult(result As AuthenticationResult) Implements IViewExternalModuleLoader.ShowAuthenticationResult
        Me.LBmessage.Text = Resource.getValue("AuthenticationResult." & result.ToString)
        Me.IMGloading.Visible = False
        '  Me.UDPinfoLogon.Update()
    End Sub

    Public Sub showLogonInfo(result As UrlProviderResult, loginUrl As String) Implements IViewExternalModuleLoader.showLogonInfo
        Me.LBmessage.Text = String.Format(Resource.getValue("UrlProviderResult." & result.ToString), loginUrl)
        Me.IMGloading.Visible = False
        '  Me.UDPinfoLogon.Update()
    End Sub

    Public Sub UnknowAuthenticationProvider() Implements IViewExternalModuleLoader.UnknowAuthenticationProvider
        Me.LBmessage.Text = Resource.getValue("UnknowAuthenticationProvider")
        Me.IMGloading.Visible = False
        '  Me.UDPinfoLogon.Update()
    End Sub

#End Region

   
End Class