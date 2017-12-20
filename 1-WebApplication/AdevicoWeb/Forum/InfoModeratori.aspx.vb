Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2

Imports lm.Comol.Core.BaseModules.Skins
Imports lm.Comol.UI.Presentation

Public Class InfoModeratori
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

    Protected WithEvents BTNOk As System.Web.UI.WebControls.Button
    Protected WithEvents DTLmoderatori As System.Web.UI.WebControls.DataList
    Protected WithEvents LBlegend As System.Web.UI.WebControls.Label

    Protected WithEvents Lit_Skin As System.Web.UI.WebControls.Literal

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub



    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property PageUtility() As OLDpageUtility
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

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

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

            'If Not Me.SystemSettings.Style.UseNewSkin Then
            '    'OLD SKIN
            '    'Me.CheckSkin()

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

            'Dim Organization_Id As Integer = 0
            'Dim Community_Id As Integer = Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

            'If Community_Id > 0 Then
            '    Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
            '    'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
            'Else
            '    'Non funziona nessuno dei due...
            '    'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
            '    Organization_Id = PageUtility.UserDefaultOrganizationId
            'End If
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        BindModeratori()
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
            BTNOk.Attributes.Add("onclick", "ChiudiMi();return false;")
        End If
    End Sub

    Public Sub BindModeratori()
        Dim oForum As New COL_Forums

        oForum.Id = Request.QueryString("FRUM_ID")
        Dim oDataset As New DataSet
        Try

            oDataset = oForum.ElencaModeratori

            Me.DTLmoderatori.DataSource = oDataset
            DTLmoderatori.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub DTLmoderatori_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DTLmoderatori.ItemCreated
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLBlogin As Label
            Try
                oLBlogin = e.Item.FindControl("LBlogin_t")
                If Not (IsNothing(oLBlogin)) Then
                    oResource.setLabel(oLBlogin)
                End If
            Catch ex As Exception

            End Try

            Dim oLBanagrafica As Label
            Try
                oLBanagrafica = e.Item.FindControl("LBanagrafica_t")
                If Not (IsNothing(oLBanagrafica)) Then
                    oResource.setLabel(oLBanagrafica)
                End If
            Catch ex As Exception

            End Try

            Dim oLBruolo As Label
            Try
                oLBruolo = e.Item.FindControl("LBruolo_t")
                If Not (IsNothing(oLBruolo)) Then
                    oResource.setLabel(oLBruolo)
                End If
            Catch ex As Exception

            End Try

            Dim oLBemail As Label
            Try
                oLBemail = e.Item.FindControl("LBemail_t")
                If Not (IsNothing(oLBemail)) Then
                    oResource.setLabel(oLBemail)
                End If
            Catch ex As Exception

            End Try
        End If

    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_InfoModeratori"
        oResource.Folder_Level1 = "Forum"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBlegend)
            .setButton(Me.BTNOk)


        End With
    End Sub
#End Region

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.Lit_Skin.Text = Me.SkinStyle()
    End Sub
End Class