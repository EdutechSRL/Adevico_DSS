Imports lm.Comol.UI.Presentation

'Per Skin
Imports lm.Comol.Core.BaseModules.Skins

'Imports System.IO

'Imports NHibernate
'Imports NHibernate.Linq


'End skin



Partial Public Class AjaxPortal
    Inherits System.Web.UI.MasterPage
    Implements IviewMaster

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
    Public WriteOnly Property ServiceTitle() As String Implements IviewMaster.ServiceTitle
        Set(ByVal value As String)
            Me.LBtitolo.Text = value
        End Set
    End Property
    Public WriteOnly Property ServiceTitleToolTip() As String Implements IviewMaster.ServiceTitleToolTip
        Set(ByVal value As String)
            Me.LBtitolo.ToolTip = value
        End Set
    End Property
    Public WriteOnly Property ServiceNopermission() As String Implements IviewMaster.ServiceNopermission
        Set(ByVal value As String)
            Me.LBNopermessi.Text = value
        End Set
    End Property
    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property
    Public Property ShowNoPermission() As Boolean Implements IviewMaster.ShowNoPermission
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

    '### NEW HEADER
    Public Property ShowHeaderLanguageChanger() As Boolean
        Get
            Return True 'Me.Intestazione.ShowLanguage
        End Get
        Set(ByVal value As Boolean)
            'Me.Intestazione.ShowLanguage = value
        End Set
    End Property
    '### NEW HEADER

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

    Protected ReadOnly Property JQueryNoticeCSSPath() As String
        Get
            If String.IsNullOrEmpty(Me.SystemSettings.Style.Header) Then
                Return "Scripts/"
            Else
                Return Me.SystemSettings.Style.Header
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
    Public Property DisplayTitleRow() As Boolean
        Get
            If Not TypeOf Me.ViewState("DisplayTitleRow") Is Boolean Then
                Me.ViewState("DisplayTitleRow") = True
            End If
            Return CBool(Me.ViewState("DisplayTitleRow"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("DisplayTitleRow") = value
        End Set
    End Property
    Public ReadOnly Property TitleRowClass As String
        Get
            If DisplayTitleRow Then
                Return ""
            Else
                Return "hide"
            End If
        End Get

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

    'Public Sub ResolveCssJsLinks()

    '    'Me.Page.Header.InnerText = Me.Page.Header.InnerHtml.Replace("~", util.BaseUrl)

    '    'For Each cn As Control In Me.Page.Header.Controls
    '    '    If TypeOf (cn) Is HtmlLink Then
    '    '        Dim hl As HtmlLink = CType(cn, HtmlLink)
    '    '        Dim url As String = hl.Attributes("href").Replace("~", util.BaseUrl)
    '    '    Else
    '    '        'If TypeOf (cn) Is HtmlGenericControl Then
    '    '        Dim hgc As HtmlGenericControl = TryCast(cn, HtmlGenericControl)
    '    '        If hgc IsNot Nothing Then
    '    '            Dim url As String = hgc.Attributes("src").Replace("~", util.BaseUrl)
    '    '        End If

    '    '        'End If
    '    '    End If
    '    'Next

    '    'Dim cn As Control = Me.Page.Header.FindControl("jquery")
    '    'Dim hl As HtmlLink = CType(cn, HtmlLink)
    '    'Dim url As String = hl.Attributes("src").Replace("~", util.BaseUrl)
    '    'hl.Attributes.Item("src") = url
    'End Sub

#Region "New Skin"

    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property ServiceSkinNew As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _SkinService
        End Get
    End Property

#End Region

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Vediamo se sto blocco puo' andare altrove...
        'If Not Page.IsPostBack Then
        'If Not Me.SystemSettings.Style.UseNewSkin Then
        '    'OLD SKIN
        '    Me.CheckSkin()
        'End If
        If Not Page.IsPostBack AndAlso PageUtility.ReadAutoOpenWindowCookie Then
            LoadNewWindowPage()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Page.Header.DataBind()
    End Sub

    Public ReadOnly Property BodyIdCode() As String
        Get
            Dim BodyId As String = ""
            If Not IsNothing(Me.PageUtility.CurrentModule) Then
                BodyId = Me.PageUtility.CurrentModule.Code
                If BodyId = "" Then
                    BodyId = "SrvGeneric"
                End If
            Else
                BodyId = "SrvHome"
            End If
            If ((ContentViewDisplay() And lm.Comol.Core.DomainModel.ContentView.hideHeader) > 0) Then
                BodyId = "popup"
            End If

            Return BodyId
        End Get
    End Property

    'Public ReadOnly Property Body() As HtmlGenericControl
    '    Get
    '        Return DirectCast(Page.Master.FindControl(BodyIdCode), HtmlGenericControl)
    '    End Get
    'End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Dim display As Integer = ContentViewDisplay()

        If ((display And lm.Comol.Core.DomainModel.ContentView.hideHeader) = 0) Then
            Me.UC_Header.Visibility = UC_PortalHeader.VisibilityState.Full
        Else
            Me.UC_Header.Visibility = UC_PortalHeader.VisibilityState.none
        End If

        Me.CtrlFooter.Visible = ((display And lm.Comol.Core.DomainModel.ContentView.hideFooter) = 0)

        Me.Lit_Skin.Text = Me.SkinStyle()

        'Me.Lit_DocType.Visible = Me.ShowDocType

    End Sub

    Private Function ContentViewDisplay() As Integer
        If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Request) AndAlso Not IsNothing(HttpContext.Current.Request.QueryString) AndAlso Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("CV")) AndAlso IsNumeric(HttpContext.Current.Request.QueryString("CV")) Then
            Return CInt(HttpContext.Current.Request.QueryString("CV"))
        Else
            Return 0
        End If
    End Function

    Private Sub LoadNewWindowPage()
        Dim cookie As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl = PageUtility.ReadLogoutAccessCookie()
        If (Not IsNothing(cookie) AndAlso cookie.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow) Then
            If Not String.IsNullOrEmpty(cookie.DestinationUrl) Then
                Dim Script As String = "window.open('" & cookie.DestinationUrl & "');"
                ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, Script, True)
            End If
          
        End If
        PageUtility.ClearAutoOpenWindowCookie()
        PageUtility.ClearLogoutAccessCookie()

      
    End Sub


#Region "Skin/Logo Management"

    'Dim _serviceSkin As Business.ServiceSkin
    'Public ReadOnly Property ServiceSkin As Business.ServiceSkin
    '    Get
    '        If IsNothing(_serviceSkin) Then
    '            _serviceSkin = New Business.ServiceSkin(Me.CurrentContext, Server.MapPath(Me.BaseUrl))
    '        End If
    '        Return _serviceSkin
    '    End Get
    'End Property

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

    '' OLD SKIN
    'Private Sub CheckSkin()
    '    Dim skin As CustomSkin

    '    Dim Organization_Id = 0
    '    'Dim UserOrganization_Id = PageUtility.CurrentUser.ORGNDefault_id
    '    Dim Community_Id = PageUtility.ComunitaCorrente.Id


    '    If Community_Id > 0 Then
    '        Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID()
    '    Else
    '        Organization_Id = PageUtility.CurrentUser.ORGNDefault_id
    '    End If
    '    'If (Community_Id > 0) Then
    '    '    'I'm into a community

    '    '    Organization_Id = IIf(Organization_Id > 0, Organization_Id, 0)
    '    'Else
    '    '    'I'm into the portal
    '    '    Organization_Id = IIf(UserOrganization_Id > 0, UserOrganization_Id, 0)
    '    'End If



    '    'Caricamento Skin -> check update and load in application...
    '    'Me.ServiceSkin.CheckUpdate()
    '    skin = Me.ServiceSkin.GetSkinByCommunity(Community_Id, Organization_Id)

    '    'Dim langCode As String = PageUtility.LinguaCode
    '    Dim SkinId As String = ""

    '    If Not IsNothing(skin) Then
    '        SkinId = skin.Id.ToString
    '    End If

    '    Session("Current_SkinId") = SkinId

    '    'If skin IsNot Nothing Then
    '    '    'In sessione per "passarla" al footer -> Usare l'application e salvare in sessione solo la key...
    '    '    Session("Current_SkinId") = skin.Id 'Usare questa invece della successiva...
    '    'End If

    '    'Return SkinId.ToString()
    'End Sub

    Public ReadOnly Property SkinStyle As String
        Get

            Dim HTMLStyleSkin As String

            'NEW SKIN
            Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

            Dim Organization_Id As Integer = GetSkinOrganizationId()
            Dim Community_Id As Integer = Me.CurrentContext.UserContext.CurrentCommunityID


            'Main CSS
            HTMLStyleSkin = ServiceSkinNew.GetCSSHtml( _
                Community_Id, _
                Organization_Id, _
                VirPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main, _
                Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf

            'Admin CSS
            'HTMLStyleSkin &= SkinService.GetCSSHtml( _
            '    Me.CurrentContext.UserContext.CurrentCommunityID, _
            '    Me.CurrentContext.UserContext.CurrentCommunityOrganizationID, _
            '    VirPath, _
            '    Me.SystemSettings.DefaultLanguage.Codice, _
            '    lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Admin) & vbCrLf

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
            'End If

            Return HTMLStyleSkin
        End Get
    End Property

    Private CurrentTemplateHeader As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
    Private CurrentTemplateFooter As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter


    Private CurrentTemplateTitle As String = ""

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

        Dim Template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = _
            Me.ServiceSkinNew.GetTemplateCommunitySkin( _
                Me.SkinFullBaseUrl, _
                SystemSettings.SkinSettings.SkinVirtualPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                Me.PageUtility.CurrentUser.Lingua.Codice, _
                CurrentTemplateTitle, _
                Me.CurrentContext.UserContext.CurrentCommunityID, _
                GetSkinOrganizationId(), _
                Me.SystemSettings.DocTemplateSettings.FooterFontSize, _
                getConfigTemplate(), _
                SystemSettings.DocTemplateSettings.FooterFontSize)

        Me.CurrentTemplateHeader = Template.Header
        Me.CurrentTemplateFooter = Template.Footer

        'If Not Me.SystemSettings.Style.UseNewSkin Then
        '    'OLD SKIN : non restituisco template... (poi setto quelli di SETTINGS...)
        '    CurrentTemplateHeader = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        '    CurrentTemplateFooter = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        'Else

        '    'Get Skin
        '    Dim Organization_Id As Integer = GetSkinOrganizationId()
        '    Dim Community_Id As Integer = Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

        '    Dim SkinTemplate As lm.Comol.Core.DomainModel.DocTemplate.Template = _
        '        Me.ServiceSkinNew.GetCommunityTemplate( _
        '            Community_Id, Organization_Id, _
        '            Me.SystemSettings.DefaultLanguage.Codice, _
        '            Me.PageUtility.CurrentUser.Lingua.Codice, _
        '            CurrentTemplateTitle, _
        '            Me.BaseUrl, _
        '            SystemSettings.SkinSettings.SkinVirtualPath)

        '    If IsNothing(SkinTemplate) Then
        '        SkinTemplate = New lm.Comol.Core.DomainModel.DocTemplate.Template
        '    End If

        '    'Set Header
        '    If IsNothing(Me.CurrentTemplateHeader) Then
        '        Me.CurrentTemplateHeader = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()

        '        If IsNothing(SkinTemplate.Header) Then
        '            SkinTemplate.Header = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        '        End If


        '        Dim ImgElement As New lm.Comol.Core.DomainModel.DocTemplate.ImageElement()
        '        Dim TxtElement As New lm.Comol.Core.DomainModel.DocTemplate.TextElement()

        '        ImgElement.Path = Me.BaseUrl & SystemSettings.SkinSettings.HeadLogo.Url
        '        ImgElement.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter

        '        TxtElement.IsHTML = True

        '        If Not String.IsNullOrEmpty(CurrentTemplateTitle) Then
        '            TxtElement.Text = CurrentTemplateTitle
        '        Else
        '            TxtElement.Text = SystemSettings.SkinSettings.HeadLogo.Alt
        '        End If

        '        If Not String.IsNullOrEmpty(TxtElement.Text) Then
        '            TxtElement.Text = "<h1>" & TxtElement.Text & "</h1>"
        '        End If

        '        Me.CurrentTemplateHeader.LeftElement = ImgElement
        '        If Not IsNothing(SkinTemplate.Header.LeftElement) Then
        '            Me.CurrentTemplateHeader.LeftElement = SkinTemplate.Header.LeftElement
        '        End If

        '        Me.CurrentTemplateHeader.RightElement = TxtElement
        '        If Not IsNothing(SkinTemplate.Header.RightElement) Then
        '            Me.CurrentTemplateHeader.RightElement = SkinTemplate.Header.RightElement
        '        End If

        '        Me.CurrentTemplateHeader.CenterElement = Nothing

        '    End If

        '    'Set footer
        '    If IsNothing(Me.CurrentTemplateFooter) Then
        '        Me.CurrentTemplateFooter = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        '        Dim DefaultTemplateFooter As New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()

        '        If IsNothing(SkinTemplate.Footer) Then
        '            SkinTemplate.Footer = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        '        End If

        '        Dim ImgElements As New lm.Comol.Core.DomainModel.DocTemplate.MultiImageElement()
        '        ImgElements.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter
        '        ImgElements.ImgElements = New List(Of lm.Comol.Core.DomainModel.DocTemplate.ImageElement)()

        '        Dim TxtElement As New lm.Comol.Core.DomainModel.DocTemplate.TextElement()

        '        For Each logo As Comol.Entity.Configuration.SkinSettings.Logo In SystemSettings.SkinSettings.FootLogos
        '            Dim ImgElement As New lm.Comol.Core.DomainModel.DocTemplate.ImageElement()

        '            ImgElement.Path = Me.BaseUrl & logo.Url
        '            ImgElement.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter

        '            ImgElements.ImgElements.Add(ImgElement)

        '        Next

        '        TxtElement.IsHTML = True
        '        TxtElement.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter
        '        TxtElement.Text = SystemSettings.SkinSettings.FootText

        '        DefaultTemplateFooter.CenterElement = Nothing

        '        Me.CurrentTemplateFooter.LeftElement = ImgElements
        '        If Not IsNothing(SkinTemplate.Footer.LeftElement) Then
        '            Me.CurrentTemplateFooter.LeftElement = SkinTemplate.Footer.LeftElement
        '        End If

        '        Me.CurrentTemplateFooter.RightElement = TxtElement
        '        If Not IsNothing(SkinTemplate.Footer.RightElement) Then
        '            Me.CurrentTemplateFooter.RightElement = SkinTemplate.Footer.RightElement
        '        End If
        '    End If
        'End If
    End Sub

    Private Function GetSkinOrganizationId() As Integer
        'Dim Organization_Id As Integer = 0
        'If Me.CurrentContext.UserContext.CurrentCommunityID > 0 Then
        '    Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
        '    'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
        'Else
        '    'Non funziona nessuno dei due...
        '    'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
        '    Organization_Id = PageUtility.UserDefaultIdOrganization
        'End If
        'Return Organization_Id
        Return PageUtility.GetSkinIdOrganization()
    End Function

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


    Public ReadOnly Property SkinFullBaseUrl As String
        Get
            Return Me.PageUtility.ApplicationUrlBase
        End Get
    End Property
    
#End Region


#Region "DocType"
    Public Property ShowDocType As Boolean
        Get
            'Dim value As Boolean = False
            'Try
            '    value = System.Convert.ToBoolean(Me.ViewState("IsDocTypeActive"))
            'Catch ex As Exception
            '    value = False
            'End Try
            'Return value
            Return Me.Lit_DocType.Visible
        End Get
        Set(ByVal value As Boolean)
            'Me.ViewState("IsDocTypeActive") = value
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
    Public Sub SetOpenDialogOnPostbackByCssClass(value As String)
        If value.StartsWith(".") Then
            HDNwindowopened.Value = value
        Else
            HDNwindowopened.Value = "." & value
        End If

    End Sub
#End Region
#Region "Advance ToolTip"
    ''' <summary>
    ''' Crea una stringa html che si inserirà nell'header per l'utilizzo di ToolTip avanzati.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnableToolTip()

        If Not _IsAdvancedToolTipEnabled Then

            Dim BaseUrl As String = Me.BaseUrl + "Scripts/"
            Dim HTMLString As String = ""

            HTMLString += "<!-- Script tooltip -->" + vbCrLf
            HTMLString += "<script type=""text/javascript"" src=""" + BaseUrl + "jquery.tooltip.js""></script>" + vbCrLf
            HTMLString += "<script type=""text/javascript"" src=""" + BaseUrl + "jquery.tooltip.min.js""></script>" + vbCrLf
            HTMLString += "<script type=""text/javascript"" src=""" + BaseUrl + "jquery.tooltip.pack.js""></script>" + vbCrLf

            HTMLString += "<link rel=""Stylesheet"" href=""" + BaseUrl + "jquery.tooltip.css"" />" + vbCrLf

            Me.Lit_ToolTip.Text = HTMLString + vbCrLf

            _IsAdvancedToolTipEnabled = True
        End If
    End Sub

    Private _IsAdvancedToolTipEnabled As Boolean = False


    ''' <summary>Abilita l'utilizzo di tooltip</summary>
    ''' <param name="ActivateCssClass">La classe dell'elemento su cui utilizzare il tooltip avanzato</param>
    ''' <param name="Separator">Un separatore per le righe</param>
    ''' <param name="AddCssClass">Una classe opzionale che viene aggiunta</param>
    ''' <param name="ShowUrl">Mostra o meno l'url nel caso di anchor</param>
    ''' <remarks></remarks>
    Public Sub AddToolTip(Optional ByVal ActivateCssClass As String = ".tooltip", _
                                      Optional ByVal Separator As String = " / ", _
                                      Optional ByVal AddCssClass As String = "", _
                                      Optional ByVal ShowUrl As Boolean = False)

        EnableToolTip()

        Dim HTMLString As String = ""

        HTMLString += "<script type=""text/javascript"">" + vbCrLf
        HTMLString += "    $(function () {" + vbCrLf
        HTMLString += "        $('" + ActivateCssClass + "').tooltip({" + vbCrLf
        HTMLString += "            showBody: """ + Separator + """," + vbCrLf

        HTMLString += "            showURL: """
        If ShowUrl Then
            HTMLString += "true """ + vbCrLf
        Else
            HTMLString += "false """ + vbCrLf
        End If


        If Not AddCssClass = "" Then
            HTMLString += "," + vbCrLf + "            extraClass: """ + AddCssClass + """" + vbCrLf
        End If

        HTMLString += vbCrLf + "        });" + vbCrLf
        HTMLString += "    });" + vbCrLf
        HTMLString += "</script>" + vbCrLf

        Lit_ToolTip.Text &= vbCrLf + HTMLString

    End Sub
#End Region


End Class