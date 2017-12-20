Imports lm.Comol.Core.BaseModules.OrganizationStyle
Imports lm.Comol.Core.BaseModules.Skins
Imports lm.Comol.UI.Presentation

Public Class UC_PortalFooter
    Inherits BaseControl

#Region "Context"
    'Dim _serviceSkin As Business.ServiceSkin
    'Private ReadOnly Property ServiceSkin As Business.ServiceSkin
    '    Get
    '        If IsNothing(_serviceSkin) Then
    '            _serviceSkin = New Business.ServiceSkin(Me.CurrentContext, Server.MapPath(Me.BaseUrl))
    '        End If
    '        Return _serviceSkin
    '    End Get
    'End Property
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
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

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'If Not Me.SystemSettings.Style.UseNewSkin Then
        '    'Old Skin
        '    Me.LoadSkin()
        'Else
        'NEW SKIN


        If AutoUpdate Then
            Me.AutoLoadNewSkin()
        End If

        'End If


        'Dim langId = PageUtility.UserSessionLanguage.ID
        'Dim langCode = PageUtility.UserSessionLanguage.Codice

        'Dim OrgId = 0

        'Dim UserOrgId = PageUtility.CurrentUser.ORGNDefault_id
        'Dim CommunityId = PageUtility.ComunitaCorrente.Id
        'If (CommunityId > 0) Then
        '    'i'm into a community
        '    OrgId = PageUtility.ComunitaCorrente.GetOrganizzazioneID()
        '    OrgId = IIf(OrgId > 0, OrgId, 0)
        'Else
        '    'i'm into the portal
        '    OrgId = IIf(UserOrgId > 0, UserOrgId, 0)
        'End If

        'Try
        '    Dim org As Organization = Application(String.Format("Organization_{0}", OrgId))
        '    If (org Is Nothing) Then
        '        org = Application(String.Format("Organization_{0}", 0))
        '    End If

        '    Dim htmlcode As HtmlCode = (From item As HtmlCode In org.Footer.HtmlCodes Where item.Language = langCode Or item.LanguageID = langId).FirstOrDefault()
        '    If (htmlcode Is Nothing) Then
        '        htmlcode = org.Footer.HtmlCodes(0)
        '    End If

        '    LtrFooter.Text = htmlcode.Code

        'Catch ex As Exception

        'End Try

    End Sub

    Private Sub AutoLoadNewSkin()
        'NEW SKIN
        Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

        Dim Organization_Id = PageUtility.GetSkinIdOrganization()
        Dim Community_Id = Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

        'If Community_Id > 0 Then
        '    Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
        '    'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
        'Else
        '    'Non funziona nessuno dei due...
        '    'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
        '    Organization_Id = PageUtility.UserDefaultIdOrganization
        'End If

        Dim html As lm.Comol.Modules.Standard.Skin.Domain.HTML.HTMLSkin = SkinService.GetSkinHTML( _
            Community_Id, _
            Organization_Id, _
            VirPath, _
            Me.CurrentContext.UserContext.Language.Code, _
            Me.SystemSettings.DefaultLanguage.Codice, _
            Me.SystemSettings.SkinSettings, Me.BaseUrl)

        LTRtext.Text = html.FooterText

        Me.LTRlogos.Text = ""
        Dim logocount As Integer = 0
        For Each Ftlogo As String In html.HtmlFooterLogos
            Me.LTRlogos.Text += "<li class=""logo l" + logocount.ToString() + """>" + Ftlogo + "</li>"
            logocount += 1
        Next

    End Sub

    Public Sub LoadSkin(ByVal html As lm.Comol.Modules.Standard.Skin.Domain.HTML.HTMLSkin)

        LTRtext.Text = html.FooterText

        Me.LTRlogos.Text = ""
        Dim logocount As Integer = 0
        For Each Ftlogo As String In html.HtmlFooterLogos
            Me.LTRlogos.Text += "<li class=""logo l" + logocount.ToString() + """>" + Ftlogo + "</li>"
            logocount += 1
        Next

    End Sub

    'Private Sub LoadSkin()
    '    Dim SkinId As String = Session("Current_SkinId")

    '    If SkinId = "" Then
    '        Exit Sub
    '    End If

    '    'Dim skin As CustomSkin = Session("Current_Skin")
    '    'Dim dict As Dictionary(Of String, String) = Session("Current_SkinWeb")
    '    Dim SkinHtmlDict As Dictionary(Of String, String)
    '    Try
    '        SkinHtmlDict = Me.ServiceSkin.GetHTMLSkins(SkinId, PageUtility.CurrentUser.Lingua.ID)
    '    Catch ex As Exception
    '        SkinHtmlDict = Nothing
    '    End Try


    '    Dim langId = PageUtility.LinguaID
    '    Dim langCode = PageUtility.LinguaCode


    '    If Not IsNothing(SkinHtmlDict) Then
    '        LTRlogos.Text = SkinHtmlDict(WebFormBuilder.SkinFooterLogos)

    '        Dim KeyLId = String.Format(WebFormBuilder.SkinFooterTextLocalized, langId)
    '        Dim KeyLCode = String.Format(WebFormBuilder.SkinFooterTextLocalized, langCode)

    '        Dim HtmlLId = ""
    '        Dim HtmlLCode = ""
    '        If (SkinHtmlDict.Keys.Contains(KeyLId)) Then
    '            'Try
    '            HtmlLId = SkinHtmlDict(KeyLId)

    '            'Catch ex As Exception

    '            'End Try
    '        End If


    '        If (SkinHtmlDict.Keys.Contains(KeyLCode)) Then
    '            'Try
    '            HtmlLCode = SkinHtmlDict(KeyLCode)
    '            'Catch ex As Exception

    '            'End Try
    '        End If

    '        If (HtmlLId IsNot Nothing AndAlso HtmlLId <> "") Then
    '            LTRtext.Text = HtmlLId
    '        Else
    '            LTRtext.Text = HtmlLCode
    '        End If

    '        'LtrFooter.Text = ???
    '    End If
    'End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region
   
    'Public ReadOnly Property ApplicationUrlBase(Optional ByVal WithoutSSLfromConfig As Boolean = False) As String
    '    Get
    '        Dim Redirect As String = "http"

    '        If PageUtility.RequireSSL And Not WithoutSSLfromConfig Then
    '            Redirect &= "s://" & Me.Request.Url.Host & Me.PageUtility.BaseUrl
    '        Else
    '            Redirect &= "://" & Me.Request.Url.Host & Me.PageUtility.BaseUrl
    '        End If
    '        ApplicationUrlBase = Redirect
    '    End Get
    'End Property



    Public Property AutoUpdate As Boolean
        Get


            If String.IsNullOrEmpty(Me.ViewState("NoAutoUpdate")) Then
                Return True
            End If

            Return False
        End Get
        Set(value As Boolean)
            
            If Not value Then
                Me.ViewState("NoAutoUpdate") = "1"
            Else
                Me.ViewState("NoAutoUpdate") = ""
            End If

        End Set
    End Property

End Class