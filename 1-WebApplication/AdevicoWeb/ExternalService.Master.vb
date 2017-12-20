Imports lm.Comol.UI.Presentation

'Per Skin
'Imports lm.Comol.Core.BaseModules.Skins


Partial Public Class ExternalService
    Inherits System.Web.UI.MasterPage

#Region "Page property"
    Private _PageUtility As OLDpageUtility
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ' -----------------------            D A          F A R E ! ! !            ----------------------------
    Public ReadOnly Property BodyIdCode() As String
        Get
            Return "SrvExternal"
        End Get
    End Property

#Region "Public MasterPage Property"
    ''' <summary>
    ''' Imposta o verifica il titolo della pagina, quello che internamente viene utilizzato per il nome comunità.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Page_Title() As String
        Get
            Return Me.LBpageTitle.Text
        End Get
        Set(value As String)
            Me.LBpageTitle.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Imposta eventuali elementi del menu.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    '''  Mettere un UL id="top" intorno alla lista dei link e dei LI per ogni elemento
    ''' </remarks>
    Public Property ExternalMenu() As String
        Get
            Return Me.LTmenuExternal.Text
        End Get
        Set(value As String)
            Me.LTmenuExternal.Text = value
        End Set
    End Property

    Public Property ShowDocType As Boolean
        Get
            Return Me.LTdocType.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.LTdocType.Visible = value
        End Set
    End Property

    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property

    Private _BodyCssClass As String = "internal_body"

    Public Property BodyCssClass As String
        Get
            Return _BodyCssClass
        End Get
        Set(value As String)
            _BodyCssClass = value
        End Set
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
#End Region


#Region "Skin"
    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property SkinServiceNew As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _SkinService
        End Get
    End Property

    Public Sub InitializeMaster(ByVal initializer As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext)
        Me.ShowDocType = initializer.ShowDocType
        Me.Page_Title = initializer.Title
        Me.BodyCssClass = initializer.CssClass
        If Not (IsNothing(initializer.Source) OrElse initializer.Source.ObjectLongID = 0) Then
            initializer.Skin = SkinServiceNew.GetModuleSkin(initializer.Source, initializer.Skin)
        End If
        BindSkin(initializer)
    End Sub
    Public Sub InitializeMasterForPreview(ByVal initializer As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext)
        Me.ShowDocType = initializer.ShowDocType
        Me.Page_Title = initializer.Title
        Me.BodyCssClass = initializer.CssClass
        BindSkin(initializer)
    End Sub
    Public Sub BindSkin(Optional ByVal idModuleSkin As Integer = 0, Optional ByVal idCommunity As Integer = 0, Optional ByVal idOrganization As Integer = 0)
        Dim dtoSKin As New lm.Comol.Core.DomainModel.Helpers.dtoItemSkin
        dtoSKin.IdSkin = idModuleSkin
        dtoSKin.IdCommunity = idCommunity
        dtoSKin.IdOrganization = idOrganization
        dtoSKin.IsForPortal = (idCommunity = 0) AndAlso idOrganization = 0
        BindSkin(dtoSKin, False)
    End Sub


    Public Sub BindSkin(ByVal initializer As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext)
        BindSkin(initializer.Skin, True)
    End Sub
    ''' <summary>
    ''' Carica tutti gli elementi legate alle skin,
    ''' sia di portale che di Modulo.
    ''' </summary>
    ''' <param name="dtoSKin">
    ''' Parametri per l'identificazione corretta delle skin.
    ''' </param>
    ''' <remarks>
    ''' C'è da rivedere la nomenclatura delle funzioni (al momento è un casino)
    ''' </remarks>
    Public Sub BindSkin(ByVal dtoSKin As lm.Comol.Core.DomainModel.Helpers.dtoItemSkin, ByVal forObject As Boolean)

        If IsNothing(dtoSKin) Then
            dtoSKin = New lm.Comol.Core.DomainModel.Helpers.dtoItemSkin
            dtoSKin.IdSkin = 0
            dtoSKin.IdCommunity = 0
            dtoSKin.IdOrganization = 0
            dtoSKin.IsForPortal = True
            dtoSKin.ForObject = forObject
        End If

        _dtoSKin = dtoSKin

        Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

        '--->   CSS   <---      SOLO SKIN SISTEMA!
        Dim HTMLStyleSkin As String = ""


        'Main CSS
        HTMLStyleSkin = SkinServiceNew.GetCSSHtml( _
            dtoSKin.IdCommunity, _
            dtoSKin.IdOrganization, _
            VirPath, _
            Me.SystemSettings.DefaultLanguage.Codice, _
            lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main, _
            Me.BaseUrl, SystemSettings.SkinSettings, 0, forObject) & vbCrLf


        'Admin CSS
        'HTMLStyleSkin &= SkinService.GetCSSHtml( _
        '    Me.CurrentContext.UserContext.CurrentCommunityID, _
        '    Me.CurrentContext.UserContext.CurrentCommunityOrganizationID, _
        '    VirPath, _
        '    Me.SystemSettings.DefaultLanguage.Codice, _
        '    lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Admin) & vbCrLf

        'IE CSS
        If (Request.Browser.Browser.Equals("IE")) Then
            HTMLStyleSkin &= SkinServiceNew.GetCSSHtml( _
                dtoSKin.IdCommunity, _
                dtoSKin.IdOrganization, _
                VirPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.IE, _
                Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf
        End If

        Me.LTskin.Text = HTMLStyleSkin


        '---> Logo Header <---

        ' Render Logo  !!!! MODIFICARE ED AGGIUNGERE LA FUNZIONE PER LE SKIN DI MODULO !!!!
        Dim html As lm.Comol.Modules.Standard.Skin.Domain.HTML.HTMLSkin = SkinServiceNew.GetSkinHTML( _
            dtoSKin.IdCommunity, _
            dtoSKin.IdOrganization, _
            VirPath, _
            Me.CurrentContext.UserContext.Language.Code, _
            Me.SystemSettings.DefaultLanguage.Codice,
            Me.SystemSettings.SkinSettings, _
            Me.BaseUrl, _
            dtoSKin.IdSkin, forObject)

        Me.LTheaderLogo.Text = html.HtmlHeadLogo

        ' Footer  !!!! MODIFICARE ED AGGIUNGERE LA FUNZIONE PER LE SKIN DI MODULO !!!!
        LTbottomText.Text = html.FooterText

        Me.LTbottomLogos.Text = ""

        Dim logocount As Integer = 0
        For Each Ftlogo As String In html.HtmlFooterLogos
            Me.LTbottomLogos.Text += "<li class=""logo l" + logocount.ToString() + """>" + Ftlogo + "</li>"
            logocount += 1
        Next
        'Template  !!!! MODIFICARE ED AGGIUNGERE LA FUNZIONE PER LE SKIN DI MODULO !!!!
        Me._HeaderTemplate = html.HeaderTemplate
        Me._FooterTemplate = html.FooterTemplate

        'SOLO per test:     !!!! ELIMINARE QUANDO OK I PUNTI PRECEDENTI !!!!
        'Me._HeaderTemplate = "head_right"
        'Me._FooterTemplate = "right"

    End Sub

    Private _dtoSKin As lm.Comol.Core.DomainModel.Helpers.dtoItemSkin

    Private WriteOnly Property _HeaderTemplate As String
        Set(value As String)
            Me.ViewState("HeaderTemplate") = value
        End Set
    End Property
    'Dim _HeaderTemplate As String = ""
    Public ReadOnly Property HeaderTemplate As String
        Get
            Return Me.ViewState("HeaderTemplate") '_HeaderTemplate
        End Get
    End Property
    'Dim _FooterTemplate As String = ""
    Private WriteOnly Property _FooterTemplate As String
        Set(value As String)
            Me.ViewState("FooterTemplate") = value
        End Set
    End Property
    Public ReadOnly Property FooterTemplate As String
        Get
            Return Me.ViewState("FooterTemplate") '_FooterTemplate
        End Get
    End Property

    Private _CurrentTemplateHeader As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
    ' lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter
    Private _CurrentTemplateFooter As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
    'lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter

    Private CurrentTemplateTitle As String = ""

    Public Function getTemplateHeader(Optional ByVal Title As String = "") As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        'lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter
        If CurrentTemplateTitle <> Title Then
            CurrentTemplateTitle = Title
            _CurrentTemplateHeader = Nothing
        End If

        If IsNothing(_CurrentTemplateHeader) Then
            BindTemplate()
        End If

        Return _CurrentTemplateHeader
    End Function

    Public Function getTemplateFooter() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        If IsNothing(_CurrentTemplateHeader) Then
            BindTemplate()
        End If

        Return _CurrentTemplateFooter
    End Function

    Private Sub BindTemplate()

        'lm.Comol.Core.DomainModel.DocTemplate.Template
        Dim Template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = Me.SkinServiceNew.GetTemplateCommunitySkin(Me.SkinFullBaseUrl, SystemSettings.SkinSettings.SkinVirtualPath, Me.SystemSettings.DefaultLanguage.Codice, Me.PageUtility.CurrentUser.Lingua.Codice, CurrentTemplateTitle, _dtoSKin.IdCommunity, _dtoSKin.IdOrganization, _dtoSKin.IdSkin, getConfigTemplate(), SystemSettings.DocTemplateSettings.FooterFontSize)

        If Not IsNothing(Template) Then
            Me._CurrentTemplateHeader = Template.Header
            Me._CurrentTemplateFooter = Template.Footer
        End If
        'If Not Me.SystemSettings.Style.UseNewSkin Then
        '    'OLD SKIN : non restituisco template... (poi setto quelli di SETTINGS...)
        '    _CurrentTemplateHeader = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        '    _CurrentTemplateFooter = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        'Else


        '    Dim Organization_Id As Integer = _dtoSKin.IdOrganization    'GetSkinOrganizationId()
        '    Dim Community_Id As Integer = _dtoSKin.IdCommunity          'Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

        '    Dim SkinSrvTemplate As lm.Comol.Core.DomainModel.DocTemplate.Template = _
        '        Me.SkinServiceNew.GetSkinTemplate( _
        '            _dtoSKin.IdSkin, _
        '            Me.SystemSettings.DefaultLanguage.Codice, _
        '            Me.PageUtility.CurrentUser.Lingua.Codice, _
        '            CurrentTemplateTitle, _
        '            Me.BaseUrl, _
        '            SystemSettings.SkinSettings.SkinVirtualPath)

        '    If IsNothing(SkinSrvTemplate) Then
        '        SkinSrvTemplate = New lm.Comol.Core.DomainModel.DocTemplate.Template
        '    End If

        '    Dim SkinSysTemplate As lm.Comol.Core.DomainModel.DocTemplate.Template = _
        '        Me.SkinServiceNew.GetCommunityTemplate( _
        '            Community_Id, Organization_Id, _
        '            Me.SystemSettings.DefaultLanguage.Codice, _
        '            Me.PageUtility.CurrentUser.Lingua.Codice, _
        '            CurrentTemplateTitle, _
        '            Me.BaseUrl, _
        '            SystemSettings.SkinSettings.SkinVirtualPath)

        '    If IsNothing(SkinSysTemplate) Then
        '        SkinSysTemplate = New lm.Comol.Core.DomainModel.DocTemplate.Template
        '    End If


        '    'Set Header
        '    If IsNothing(Me._CurrentTemplateHeader) Then
        '        Me._CurrentTemplateHeader = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()

        '        If IsNothing(SkinSysTemplate.Header) Then
        '            SkinSysTemplate.Header = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
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

        '        Me._CurrentTemplateHeader.LeftElement = ImgElement
        '        If Not IsNothing(SkinSysTemplate.Header.LeftElement) Then
        '            Me._CurrentTemplateHeader.LeftElement = SkinSysTemplate.Header.LeftElement
        '        End If

        '        Me._CurrentTemplateHeader.RightElement = TxtElement
        '        If Not IsNothing(SkinSysTemplate.Header.RightElement) Then
        '            Me._CurrentTemplateHeader.RightElement = SkinSysTemplate.Header.RightElement
        '        End If

        '        Me._CurrentTemplateHeader.CenterElement = Nothing

        '    End If

        '    'Set footer
        '    If IsNothing(Me._CurrentTemplateFooter) Then
        '        Me._CurrentTemplateFooter = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
        '        Dim DefaultTemplateFooter As New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()

        '        If IsNothing(SkinSysTemplate.Footer) Then
        '            SkinSysTemplate.Footer = New lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter()
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

        '        Me._CurrentTemplateFooter.LeftElement = ImgElements
        '        If Not IsNothing(SkinSysTemplate.Footer.LeftElement) Then
        '            Me._CurrentTemplateFooter.LeftElement = SkinSysTemplate.Footer.LeftElement
        '        End If

        '        Me._CurrentTemplateFooter.RightElement = TxtElement
        '        If Not IsNothing(SkinSysTemplate.Footer.RightElement) Then
        '            Me._CurrentTemplateFooter.RightElement = SkinSysTemplate.Footer.RightElement
        '        End If
        '    End If

        'End If
    End Sub

    Public Function getConfigTemplate() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        'lm.Comol.Core.DomainModel.DocTemplate.Template()

        Dim ConfTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template() ' lm.Comol.Core.DomainModel.DocTemplate.Template
        ConfTemplate.Header = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter()
        ConfTemplate.Footer = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter()

        'Header
        Dim ImgElement As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage()
        Dim TxtElementH As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText

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
        Dim ImgElements As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti()
        ImgElements.Alignment = lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter
        ImgElements.ImgElements = New List(Of lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage)()

        Dim TxtElementF As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText()

        For Each logo As Comol.Entity.Configuration.SkinSettings.Logo In SystemSettings.SkinSettings.FootLogos
            'Dim TxtElementF As New lm.Comol.Core.DomainModel.DocTemplate.ImageElement()
            Dim ImgelementF As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage()

            ImgelementF.Path = Me.SkinFullBaseUrl & logo.Url
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

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Head1.DataBind()
    End Sub
End Class