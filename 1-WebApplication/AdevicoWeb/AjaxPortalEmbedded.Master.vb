Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.BaseModules.Skins

Partial Public Class AjaxPortalEmbedded
    Inherits System.Web.UI.MasterPage
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
    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim display As Integer = ContentViewDisplay()
        LTskin.Text = Me.SkinStyle()
    End Sub

    Private Function ContentViewDisplay() As Integer
        If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Request) AndAlso Not IsNothing(HttpContext.Current.Request.QueryString) AndAlso Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("CV")) AndAlso IsNumeric(HttpContext.Current.Request.QueryString("CV")) Then
            Return CInt(HttpContext.Current.Request.QueryString("CV"))
        Else
            Return 0
        End If
    End Function


#Region "Skin/Logo Management"

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
    End Sub

    Private Function GetSkinOrganizationId() As Integer
        Return PageUtility.GetSkinIdOrganization
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
End Class