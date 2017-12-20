Imports lm.Comol.UI.Presentation
'Per Skin
Imports lm.Comol.Core.BaseModules.Skins

Partial Public Class AdminPortal
    Inherits System.Web.UI.MasterPage
    Implements IviewMaster

    Private util As New OLDpageUtility(HttpContext.Current)

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

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
    Protected ReadOnly Property JQueryNoticeCSSPath() As String
        Get
            If String.IsNullOrEmpty(Me.SystemSettings.Style.Header) Then
                Return "Scripts/"
            Else
                Return Me.SystemSettings.Style.Header
            End If
        End Get
    End Property
    Public WriteOnly Property ServiceTitle() As String Implements PresentationLayer.IviewMaster.ServiceTitle
        Set(ByVal value As String)
            Me.LBtitolo.Text = value
        End Set
    End Property
    Public WriteOnly Property ServiceTitleToolTip() As String Implements IviewMaster.ServiceTitleToolTip
        Set(ByVal value As String)
            Me.LBtitolo.ToolTip = value
        End Set
    End Property
    Public Property ShowNoPermission() As Boolean Implements PresentationLayer.IviewMaster.ShowNoPermission
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
    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
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


    Public Sub ResolveCssJsLinks()

    End Sub

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        'If Not Me.SystemSettings.Style.UseNewSkin Then
        '    Me.CheckSkin()
        'End If
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
            Return BodyId
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim styleSkin As String = Me.SkinStyle()

        If Not String.IsNullOrEmpty(styleSkin) Then
            Me.Lit_Skin.Text = styleSkin
        End If

    End Sub

    Public ReadOnly Property SkinStyle As String
        Get
            Dim HTMLStyleSkin As String

            'If Not Me.SystemSettings.Style.UseNewSkin Then
            '    'OLD SKIN
            '    Dim SkinId As String = ""
            '    Try
            '        SkinId = Session("Current_SkinId").ToString()
            '    Catch ex As Exception

            '    End Try


            '    If SkinId = "" Then
            '        HTMLStyleSkin = ""
            '    Else
            '        Dim SkinsHtmlDict As Dictionary(Of String, String) = Me.ServiceSkin.GetHTMLSkins(SkinId, PageUtility.CurrentUser.Lingua.ID)
            '        HTMLStyleSkin = SkinsHtmlDict(WebFormBuilder.SkinHeaderCssLink)
            '    End If
            'Else

            'NEW SKIN
            Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

            Dim Organization_Id = PageUtility.GetSkinIdOrganization
            Dim Community_Id = Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

            'If Community_Id > 0 Then
            '    Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
            '    'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
            'Else
            '    'Non funziona nessuno dei due...
            '    'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
            '    Organization_Id = PageUtility.UserDefaultOrganizationId
            'End If


            'Main CSS
            HTMLStyleSkin = SkinService.GetCSSHtml( _
                Community_Id, _
                Organization_Id, _
                VirPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main, _
                Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf

            'Admin CSS
            HTMLStyleSkin &= SkinService.GetCSSHtml( _
                Community_Id, _
                Organization_Id, _
                VirPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Admin, _
                Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf

            'IE CSS
            If (Request.Browser.Browser.Equals("IE")) Then
                HTMLStyleSkin &= SkinService.GetCSSHtml( _
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

    'Dim _serviceSkin As Business.ServiceSkin
    'Public ReadOnly Property ServiceSkin As Business.ServiceSkin
    '    Get
    '        If IsNothing(_serviceSkin) Then
    '            _serviceSkin = New Business.ServiceSkin(Me.CurrentContext, Server.MapPath(Me.BaseUrl))
    '        End If
    '        Return _serviceSkin
    '    End Get
    'End Property

    Public WriteOnly Property ServiceNopermission() As String Implements IviewMaster.ServiceNopermission
        Set(ByVal value As String)
            Me.LBNopermessi.Text = value
        End Set
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
            Return Me.LTdocType.Visible
        End Get
        Set(ByVal value As Boolean)
            'Me.ViewState("IsDocTypeActive") = value
            Me.LTdocType.Visible = value
        End Set
    End Property
#End Region

    ' OLD SKIN
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
End Class