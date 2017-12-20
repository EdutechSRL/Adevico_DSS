Imports lm.Comol.UI.Presentation
'Per Skin
Imports lm.Comol.Core.BaseModules.Skins


Partial Public Class Authentication
    Inherits System.Web.UI.MasterPage
    Private _Resource As ResourceManager

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "Property"
    Protected Friend ReadOnly Property PreloadedIdLogonSkin As Long
        Get
            If IsNumeric(Me.Request.QueryString("idLogonSkin")) Then
                Return CLng(Me.Request.QueryString("idLogonSkin"))
            Else
                Return -1
            End If
        End Get
    End Property
    Public ReadOnly Property IsoCode As String
        Get
            Return PageUtility.IsoLanguageCode
        End Get
    End Property
    Private _PageUtility As OLDpageUtility
    Protected ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property
    Public ReadOnly Property BaseUrl() As String
        Get
            Dim url As String = Me.Request.ApplicationPath
            If url.EndsWith("/") Then
                Return url
            Else
                Return url + "/"
            End If
        End Get
    End Property

    Public Property ShowLanguage As Boolean
        Get
            Return ViewStateOrDefault("ShowLang", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowLang") = value
        End Set
    End Property
    Public Property ShowHelp As Boolean
        Get

            Return ViewStateOrDefault("ShowHelp", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowHelp") = value
        End Set
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


#Region "New Skin"

    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _SkinService
        End Get
    End Property

#End Region

    'Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    Me.CheckSkin()
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindTopBar()

        Me.BindSkin()
    End Sub

    Private Sub BindTopBar()

        Dim TBS As TopBarSetting = Me.SystemSettings.TopBar

        If Me.ShowLanguage Then
            Me.LTlanguage.Text = TBS.RenderLoginLanguages(PageUtility.BaseUrl, Me.PageUtility.SecureApplicationUrlBase, PageUtility.LinguaID, PageUtility.LinguaCode)
            Me.LTlanguage.Visible = True
        Else
            Me.LTlanguage.Text = ""
            Me.LTlanguage.Visible = False
        End If
        Dim helpurl As String = ""
        Try

            helpurl = If(TBS.HelpUrl.IsHttps, Me.PageUtility.SecureApplicationUrlBase, PageUtility.BaseUrl)
            helpurl &= TBS.HelpUrl.Url

            Me.HYPhelp.NavigateUrl = helpurl

            If IsNothing(Me._Resource) Then
                SetCulture("pg_ISAuthenticationPage", "Authentication")
            End If

            Me._Resource.setHyperLink(Me.HYPhelp, True, True)
        Catch ex As Exception

        End Try
        If String.IsNullOrEmpty(helpurl) Then
            HYPhelp.Visible = False
        End If
    End Sub


    Public Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String)
        Me._Resource = New ResourceManager

        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.setCulture()
    End Sub

#Region "Skin - Loghi"
    'Private Sub CheckSkin()
    '    Dim skin As CustomSkin

    '    Dim Organization_Id = 0
    '    Dim UserOrganization_Id = PageUtility.CurrentUser.ORGNDefault_id
    '    Dim Community_Id = PageUtility.ComunitaCorrente.Id


    '    If Community_Id > 0 Then
    '        Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID()
    '    Else
    '        Organization_Id = PageUtility.CurrentUser.ORGNDefault_id
    '    End If

    '    skin = Me.ServiceSkin.GetSkinByCommunity(Community_Id, Organization_Id)

    '    Dim SkinId As String = ""

    '    If Not IsNothing(skin) Then
    '        SkinId = skin.Id.ToString
    '    End If

    '    Session("Current_SkinId") = SkinId

    'End Sub

    'Dim _serviceSkin As Business.ServiceSkin
    'Public ReadOnly Property ServiceSkin As Business.ServiceSkin
    '    Get
    '        If IsNothing(_serviceSkin) Then
    '            _serviceSkin = New Business.ServiceSkin(Me.CurrentContext, Server.MapPath(Me.BaseUrl))
    '        End If
    '        Return _serviceSkin
    '    End Get
    'End Property

    ''' <summary>
    ''' Recupera il logo per la login, attraverso SKIN...
    ''' </summary>
    ''' <remarks>
    ''' Verrà semplificato MOSTRUOSAMENTE con la nuova versione delle skin.
    ''' In alternativa caricarlo "staticamente" nel config...
    ''' </remarks>
    Private Sub BindSkin()
        Dim idSkin As Long = GetIdLogonSkin()


        'If Not Me.SystemSettings.Style.UseNewSkin Then

        '    'OLD VERSION
        '    Me.logo.ImageUrl = If(Request.IsSecureConnection, Me.PageUtility.SecureApplicationUrlBase, Me.PageUtility.ApplicationUrlBase)
        '    Me.logo.ImageUrl &= Me.SystemSettings.Presenter.LogoIstituzione '"Style/NewMenuDemo/css/img/logo.png"
        '    Me.logo.Visible = True

        '    Me.LTskin.Text = "<link media=""all"" type=""text/css"" href="""
        '    Me.LTskin.Text &= PageUtility.BaseUrl
        '    Me.LTskin.Text &= Me.SystemSettings.SkinSettings.LoginCss + """ rel=""stylesheet"">" + vbCrLf

        '    Me.Lit_Logo.Text = ""
        '    Me.Lit_Logo.Visible = False
        'Else

        'NEW VERSION
        Dim HTMLStyleSkin As String

        'CSS
        Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

        'Me.LTskin.Text = "<link media=""all"" type=""text/css"" href="""
        'Me.LTskin.Text &= PageUtility.BaseUrl
        'Me.LTskin.Text &= Me.SystemSettings.TopBar.LoginCss + """ rel=""stylesheet"">" + vbCrLf

        Me.LTskin.Text = SkinService.GetCSSHtml( _
             0, _
             0, _
             VirPath, _
             Me.SystemSettings.DefaultLanguage.Codice, _
             lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Login, _
             Me.BaseUrl, SystemSettings.SkinSettings,
             idSkin) & vbCrLf

        'Logo
        Dim html As lm.Comol.Modules.Standard.Skin.Domain.HTML.HTMLSkin = SkinService.GetSkinHTML( _
                0, _
                0, _
                VirPath, _
                Me.CurrentContext.UserContext.Language.Code,
                Me.SystemSettings.DefaultLanguage.Codice, _
                Me.SystemSettings.SkinSettings, Me.BaseUrl,
                idSkin)

        Me.CtrlFooter.LoadSkin(html)
        
        Me.Lit_Logo.Text = html.HtmlHeadLogo
        Me.Lit_Logo.Visible = True
        Me.logo.Visible = False

        'SE non trova html/skin carica valori da file configurazione
        'NOTA: questo puo' ESSERE BELLAMENTE ARATO, COMUNQUE se non trova skin, lo crea lui da config.
        'If Me.Lit_Logo.Text = "" Then
        '    Me.logo.ImageUrl = If(Request.IsSecureConnection, Me.PageUtility.SecureApplicationUrlBase, Me.PageUtility.ApplicationUrlBase)
        '    Me.logo.ImageUrl &= Me.SystemSettings.Presenter.LogoIstituzione '"Style/NewMenuDemo/css/img/logo.png"
        '    Me.logo.Visible = True
        '    Me.Lit_Logo.Visible = False
        'End If

        'If Me.LTskin.Text = "" Then
        '    Me.LTskin.Text = "<link media=""all"" type=""text/css"" href="""
        '    Me.LTskin.Text &= PageUtility.BaseUrl
        '    Me.LTskin.Text &= Me.SystemSettings.TopBar.LoginCss + """ rel=""stylesheet"">"
        'End If

        'End If
    End Sub
    'Dim ServiceSkin As Business.ServiceSkin = New Business.ServiceSkin(Me.CurrentContext, Server.MapPath(Me.BaseUrl))

    'Dim skin As CustomSkin = ServiceSkin.GetSkinByCommunity(0, 0)

    'Dim SkinLogoPath As String = ""

    ''If Not SkinID = "" Then
    'If Not IsNothing(skin) Then
    '    Session("Current_SkinId") = skin.Id
    '    Dim SkinsHtmlDict As Dictionary(Of String, String) = ServiceSkin.GetHTMLSkins(skin.Id, CurrentContext.UserContext.CurrentUser.LanguageID)

    '    If Not IsNothing(SkinsHtmlDict) Then
    '        SkinLogoPath = SkinsHtmlDict(lm.Comol.Core.BaseModules.Skins.WebFormBuilder.SkinHeaderLogo)
    '    End If
    'End If

    'If SkinLogoPath = "" Then

    'Else
    'Me.logo.ImageUrl = SkinLogoPath
    'End If

    Public Function GetIdLogonSkin() As Long
        Dim idSkin As Long = PreloadedIdLogonSkin
        If idSkin < 1 Then
            idSkin = ReadLogonSkinCookie()
        Else
            WriteLogonSkinCookie(idSkin)
        End If
        Return idSkin
    End Function
    Public Sub WriteLogonSkinCookie(ByVal idLogonSkin As Long)
        Dim oHttpCookie As New HttpCookie("LogonSkin")
        oHttpCookie.Expires = Now.AddDays(1)
        oHttpCookie.Values.Add("idLogonSkin", idLogonSkin)
        If (From k In Response.Cookies.AllKeys Where k = "LogonSkin").Any Then
            Response.Cookies.Set(oHttpCookie)
        Else
            Response.Cookies.Add(oHttpCookie)
        End If
    End Sub
    Public Function ReadLogonSkinCookie() As Long
        Dim idLogonSkin As Long = 0
        Dim oValues As New Hashtable
        Try
            For Each key As String In Request.Cookies("LogonSkin").Values.Keys
                oValues(key) = Request.Cookies("LogonSkin").Values(key)
            Next
        Catch ex As Exception

        End Try
        If Not IsNothing(oValues) Then
            If IsNumeric(oValues.Item("idLogonSkin")) Then
                idLogonSkin = oValues.Item("idLogonSkin")
            End If
        End If
        Return idLogonSkin
    End Function
    Public Sub ClearCookie(name As String)
        Response.Cookies("name").Expires = Now.AddDays(-1)
    End Sub

#End Region

#Region "DialogOpenOnPostback"
    Public Property OpenDialogOnPostback As Boolean
        Get
            Return Not HDNwindowopened.Attributes("class").Contains("disabled")
        End Get
        Set(ByVal value As Boolean)
            Dim cssClass As String = HDNwindowopened.Attributes("class")
            If value Then
                cssClass = Replace(cssClass, "disabled", "")
            ElseIf Not cssClass.Contains("disabled") Then
                cssClass &= " disabled"
            End If
            HDNwindowopened.Attributes("class") = cssClass
        End Set
    End Property
    Public Sub ClearOpenedDialogOnPostback()
        HDNwindowopened.Value = ""
    End Sub
#End Region
End Class