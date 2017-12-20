Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.BaseModules.Skins

Partial Public Class AjaxPopup
    Inherits System.Web.UI.MasterPage
    Private _Resource As ResourceManager

    Private _PageUtility As OLDpageUtility

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property IsoCode As String
        Get
            Return New System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID, False).TwoLetterISOLanguageName
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

    Protected ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public WriteOnly Property ServiceTitle() As String
        Set(ByVal value As String)
            Me.LBtitolo.Text = value
        End Set
    End Property
    Public WriteOnly Property ServiceTitleToolTip() As String
        Set(ByVal value As String)
            Me.LBtitolo.ToolTip = value
        End Set
    End Property
    Public WriteOnly Property ServiceNopermission() As String
        Set(ByVal value As String)
            Me.LBNopermessi.Text = value
        End Set
    End Property
    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property
    Public Property ShowNoPermission() As Boolean
        Get
            Return (Me.MLVservice.GetActiveView Is Me.VIWnoPermission)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.MLVservice.SetActiveView(Me.VIWnoPermission)
            Else
                Me.MLVservice.SetActiveView(Me.VIWservice)
            End If
        End Set
    End Property
    Public Property ShowHeaderLanguageChanger() As Boolean
        Get
            Return False ' Return Me.Intestazione.ShowLanguage
        End Get
        Set(ByVal value As Boolean)
            ' Me.Intestazione.ShowLanguage = value
        End Set
    End Property
    Public Property BRheaderActive() As Boolean
        Get
            If Not TypeOf Me.ViewState("BRheaderActive") Is Boolean Then
                Me.ViewState("BRheaderActive") = True
            End If
            Return CBool(Me.ViewState("BRheaderActive"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("BRheaderActive") = value
        End Set
    End Property
    Protected ReadOnly Property BRheaderCSS() As String
        Get
            If BRheaderActive Then
                Return ""
            Else
                Return "display=none;"
            End If
        End Get
    End Property
    Public Property EnabledFullWidth() As Boolean
        Get
            If Not TypeOf Me.ViewState("EnabledFullWidth") Is Boolean Then
                Me.ViewState("EnabledFullWidth") = False
            End If
            Return CBool(Me.ViewState("EnabledFullWidth"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("EnabledFullWidth") = value
        End Set
    End Property
    Public ReadOnly Property FullWidthClass As String
        Get
            If EnabledFullWidth Then
                Return "fullwidth"
            Else
                Return ""
            End If
        End Get

    End Property
    Public Property HideCloseButton() As Boolean
        Get
            Return BTNcloseWindow.Visible
        End Get
        Set(ByVal value As Boolean)
            BTNcloseWindow.Visible = Not value
        End Set
    End Property
    'Protected Property BaseUrl() As String
    '    Get
    '        Try
    '            Return String.Format("http://{0}{1}", HttpHost, VirtualFolderNormalized)
    '        Catch ex As Exception
    '            Return ""
    '        End Try
    '    End Get
    '    Set(ByVal value As String)

    '    End Set
    'End Property

    'Protected ReadOnly Property VirtualFolder() As String
    '    Get
    '        '(VirtualFolder.Equals("/")) ? string.Empty : VirtualFolder)
    '        Return HttpContext.Current.Request.ApplicationPath
    '    End Get
    'End Property

    'Protected ReadOnly Property VirtualFolderNormalized() As String
    '    Get
    '        If VirtualFolder.Equals("/") Then
    '            Return String.Empty
    '        Else
    '            Return VirtualFolder
    '        End If
    '    End Get
    'End Property

    'Protected ReadOnly Property HttpHost() As String
    '    Get
    '        Return HttpContext.Current.Request.ServerVariables("HTTP_HOST")
    '    End Get
    'End Property

    Public Sub ResolveCssJsLinks()

        'Me.Page.Header.InnerText = Me.Page.Header.InnerHtml.Replace("~", util.BaseUrl)

        'For Each cn As Control In Me.Page.Header.Controls
        '    If TypeOf (cn) Is HtmlLink Then
        '        Dim hl As HtmlLink = CType(cn, HtmlLink)
        '        Dim url As String = hl.Attributes("href").Replace("~", util.BaseUrl)
        '    Else
        '        'If TypeOf (cn) Is HtmlGenericControl Then
        '        Dim hgc As HtmlGenericControl = TryCast(cn, HtmlGenericControl)
        '        If hgc IsNot Nothing Then
        '            Dim url As String = hgc.Attributes("src").Replace("~", util.BaseUrl)
        '        End If

        '        'End If
        '    End If
        'Next

        'Dim cn As Control = Me.Page.Header.FindControl("jquery")
        'Dim hl As HtmlLink = CType(cn, HtmlLink)
        'Dim url As String = hl.Attributes("src").Replace("~", util.BaseUrl)
        'hl.Attributes.Item("src") = url
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Page.Header.DataBind()
        Me.BindLogo()
        If IsNothing(Me._Resource) AndAlso Not Page.IsPostBack Then
            SetCulture("pg_ISAuthenticationPage", "Authentication")
            _Resource.setButton(BTNcloseWindow, True)
        End If
    End Sub

    Public Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String)
        Me._Resource = New ResourceManager

        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.setCulture()
    End Sub
#Region "DocType"
    Public Property ShowDocType As Boolean
        Get
            Return Me.Lit_DocType.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.Lit_DocType.Visible = value
        End Set
    End Property

    Public ReadOnly Property DocTypeClass As String
        Get
            If ShowDocType Then
                Return "ok-doctype"
            Else
                Return "no-doctype"
            End If
        End Get

    End Property

    Public Property ReloadOpener As Boolean
        Get
            If (ViewState("ReloadOpener") Is Nothing) Then
                ViewState("ReloadOpener") = False
                Return False
            Else
                Return ViewState("ReloadOpener")
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("ReloadOpener") = value
        End Set
    End Property
#End Region

#Region "Skin"
    Private CurrentTemplateHeader As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
    Private CurrentTemplateFooter As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
    Private CurrentTemplateTitle As String = ""
    Public ReadOnly Property SkinFullBaseUrl As String
        Get
            Return Me.PageUtility.ApplicationUrlBase
        End Get
    End Property
    Public Function getConfigTemplate() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template

        Dim ConfTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        ConfTemplate.Header = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        ConfTemplate.Footer = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter

        'Header
        Dim ImgElement As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage()
        Dim TxtElementH As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText()

        ImgElement.Path = Me.SkinFullBaseUrl & SystemSettings.SkinSettings.HeadLogo.Url
        ImgElement.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter

        TxtElementH.IsHTML = True


        TxtElementH.Text = SystemSettings.SkinSettings.HeadLogo.Alt

        If Not String.IsNullOrEmpty(TxtElementH.Text) Then
            TxtElementH.Text = "<h1>" & TxtElementH.Text & "</h1>"
        End If

        ConfTemplate.Header.Left = ImgElement
        ConfTemplate.Header.Right = TxtElementH
        ConfTemplate.Header.Center = Nothing

        'Footer


        Dim ImgElements As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti() ' lm.Comol.Core.DomainModel.DocTemplate.MultiImageElement()
        ImgElements.Alignment = lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter
        ImgElements.ImgElements = New List(Of lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage)()

        Dim TxtElementF As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText()

        For Each logo As Comol.Entity.Configuration.SkinSettings.Logo In SystemSettings.SkinSettings.FootLogos
            'Dim TxtElementF As New lm.Comol.Core.DomainModel.DocTemplate.ImageElement()
            Dim ImgelementF As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage()

            ImgelementF.Path = SkinFullBaseUrl & logo.Url
            'Me.BaseUrl & logo.Url
            ImgelementF.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter

            ImgElements.ImgElements.Add(ImgelementF)

        Next

        TxtElementF.IsHTML = True
        TxtElementF.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter
        TxtElementF.Text = SystemSettings.SkinSettings.FootText

        ConfTemplate.Footer.Left = ImgElements
        ConfTemplate.Footer.Center = Nothing
        ConfTemplate.Footer.Right = TxtElementF

        Return ConfTemplate

    End Function
    Public Function getTemplateHeader(Optional ByVal Title As String = "") As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        If CurrentTemplateTitle <> Title Then
            CurrentTemplateTitle = Title
            CurrentTemplateHeader = Nothing
        End If
        If IsNothing(CurrentTemplateHeader) Then
            BindTemplate()
        End If

        Return CurrentTemplateHeader
    End Function

    Public Function getTemplateFooter() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        If IsNothing(CurrentTemplateFooter) Then
            BindTemplate()
        End If

        Return CurrentTemplateFooter
    End Function
    Private Sub BindTemplate()

        Dim Template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = Me.ServiceSkinNew.GetTemplateCommunitySkin(Me.SkinFullBaseUrl, SystemSettings.SkinSettings.SkinVirtualPath, Me.SystemSettings.DefaultLanguage.Codice, Me.PageUtility.CurrentUser.Lingua.Codice, CurrentTemplateTitle, Me.CurrentContext.UserContext.CurrentCommunityID, PageUtility.GetSkinIdOrganization, 0, getConfigTemplate(), SystemSettings.DocTemplateSettings.FooterFontSize)

        Me.CurrentTemplateHeader = Template.Header
        Me.CurrentTemplateFooter = Template.Footer
    End Sub
    'Private Function GetSkinOrganizationId() As Integer
    '    Dim Organization_Id As Integer = 0
    '    If Me.CurrentContext.UserContext.CurrentCommunityID > 0 Then
    '        Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
    '        'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
    '    Else
    '        'Non funziona nessuno dei due...
    '        'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
    '        Organization_Id = PageUtility.UserDefaultIdOrganization
    '    End If
    '    Return Organization_Id
    'End Function

    Public Sub BindLogo()

        'If Not MyBase.SystemSettings.Style.UseNewSkin Then
        '    'OLD SKIN
        '    Me.logo.Visible = True
        '    Me.Lit_Logo.Text = ""
        '    Me.Lit_Logo.Visible = False

        '    Dim SkinId As String = ""
        '    Try
        '        SkinId = Session("Current_SkinId")
        '    Catch ex As Exception
        '        'SkinId = ""
        '    End Try

        '    Dim SkinLogoPath As String = ""

        '    If Not SkinId = "" Then
        '        Dim SkinsHtmlDict As Dictionary(Of String, String) = Me.ServiceSkin.GetHTMLSkins(SkinId, CurrentContext.UserContext.CurrentUser.LanguageID)

        '        If Not IsNothing(SkinsHtmlDict) Then
        '            SkinLogoPath = SkinsHtmlDict(lm.Comol.Core.BaseModules.Skins.WebFormBuilder.SkinHeaderLogo)
        '        End If
        '    End If

        '    If SkinLogoPath = "" Then

        '        Me.logo.ImageUrl = If(Request.IsSecureConnection, Me.PageUtility.SecureApplicationUrlBase, Me.PageUtility.ApplicationUrlBase)

        '        Me.logo.ImageUrl &= MyBase.SystemSettings.Presenter.LogoIstituzione '"Style/NewMenuDemo/css/img/logo.png"
        '    Else
        '        Me.logo.ImageUrl = SkinLogoPath
        '    End If
        'Else
        Me.logo.Visible = False
        Me.Lit_Logo.Visible = True

        'NEW SKIN <- spostare in PageUtility?!
        Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

        Dim Organization_Id = PageUtility.GetSkinIdOrganization
        Dim Community_Id = Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

        'If Community_Id > 0 Then
        '    Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
        'Else
        '    Organization_Id = PageUtility.UserDefaultIdOrganization
        'End If

        ' Render Logo
        Dim html As lm.Comol.Modules.Standard.Skin.Domain.HTML.HTMLSkin = ServiceSkinNew.GetSkinHTML( _
            Community_Id, _
            Organization_Id, _
            VirPath, _
            Me.CurrentContext.UserContext.Language.Code, _
            Me.SystemSettings.DefaultLanguage.Codice,
            Me.SystemSettings.SkinSettings, Me.BaseUrl
            )


        Me.Lit_Logo.Text = html.HtmlHeadLogo

        'End If



    End Sub

    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
    Private ReadOnly Property ServiceSkinNew As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _SkinService
        End Get
    End Property

    Public ReadOnly Property SkinStyle As String
        Get

            Dim HTMLStyleSkin As String

            'NEW SKIN
            Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

            Dim Organization_Id As Integer = PageUtility.GetSkinIdOrganization
            Dim Community_Id As Integer = Me.CurrentContext.UserContext.CurrentCommunityID


            'Main CSS
            HTMLStyleSkin = ServiceSkinNew.GetCSSHtml( _
                Community_Id, _
                Organization_Id, _
                VirPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main, _
                Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf

            'IE CSS
            If (Request.Browser.Browser.Equals("IE")) Then
                HTMLStyleSkin &= ServiceSkinNew.GetCSSHtml( _
                    Community_Id, _
                    Organization_Id, _
                    VirPath, _
                    Me.SystemSettings.DefaultLanguage.Codice, _
                    lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.IE, _
                    Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf
            End If

            Return HTMLStyleSkin
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


#End Region

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Lit_Skin.Text = Me.SkinStyle()
    End Sub
End Class