Imports Comunita_OnLine.ModuloGenerale
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.BaseModules.Skins
Imports lm.Comol.UI.Presentation


Public Class NotificaMessaggio
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager


#Region "private"
    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
#End Region

    Private Enum StringaSelezione
        Forum = 1
        ForumTopic = 2
        ForumTopicPost = 3
    End Enum
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


    Protected WithEvents Lit_Skin As System.Web.UI.WebControls.Literal

    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    Protected WithEvents TBSselezione As Global.Telerik.Web.UI.RadTabStrip


    'Protected WithEvents TBLselezione As System.Web.UI.WebControls.Table
    Protected WithEvents TBRconferma As HtmlControl

    Protected WithEvents LBconferma_t As System.Web.UI.WebControls.Label

    Protected WithEvents BTNconfermaSelezionati As System.Web.UI.WebControls.Button
    Protected WithEvents PNLselezione As System.Web.UI.WebControls.Panel

#Region "Pannello InvioMail"
    Protected WithEvents PNLconferma As System.Web.UI.WebControls.Panel
    Protected WithEvents LBtitoletto_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdestinatari_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBoggetto_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBoggetto As System.Web.UI.WebControls.Label
    Protected WithEvents LBtesto_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdestinatari As System.Web.UI.WebControls.Label
    Protected WithEvents TXBtesto As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNannulla As System.Web.UI.WebControls.Button
    Protected WithEvents BTNinvia As System.Web.UI.WebControls.Button
    Protected WithEvents RBLsceltaTesto As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBsceltaTesto_t As System.Web.UI.WebControls.Label
#End Region

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

    Protected WithEvents CTRLrubrica As Comunita_OnLine.UC_RubricaMailGenerica

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Dim ForumId, TopicID, PostID As String

            Me.SetupInternazionalizzazione()

            ForumId = Me.Request.QueryString("ForumId")
            PostID = Me.Request.QueryString("PostID")
            TopicID = Me.Request.QueryString("TopicID")

            Dim NomeForum, NomeThread As String
            If ForumId = "" And PostID = "" And TopicID = "" Then
                Response.Write("<script language=Javascript>" & "alert('" & Replace(Me.oResource.getValue("noselezione"), "'", "\'") & "');" & "</script>")
                Response.Write("<script language='javascript'>{ this.window.close();}</script>")
            Else
                Me.TBSselezione.SelectedIndex = 0
                Me.TBSselezione.Tabs(1).Enabled = False
                Me.RBLsceltaTesto.SelectedIndex = 0
                Me.Bind_Dati()
            End If


            Me.BTNannulla.Attributes.Add("onclick", "ChiudiMi();return false;")
        End If
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_NotificaMessaggio"
        oResource.Folder_Level1 = "Forum"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(LBNopermessi)

            TBSselezione.Tabs(0).Text = .getValue("TABselezione.Text")
            TBSselezione.Tabs(0).ToolTip = .getValue("TABselezione.ToolTip")
            TBSselezione.Tabs(1).Text = .getValue("TABmessaggio.Text")
            TBSselezione.Tabs(1).ToolTip = .getValue("TABmessaggio.ToolTip")
            .setLabel(LBconferma_t)
            .setButton(BTNconfermaSelezionati, True)


            .setLabel(LBtitoletto_t)
            .setLabel(LBdestinatari_t)
            .setLabel(LBoggetto_t)
            Me.LBoggetto.Text = .getValue("subject")
            .setLabel(LBtesto_t)
            .setButton(BTNannulla, True)
            .setButton(BTNinvia, True)
            .setRadioButtonList(RBLsceltaTesto, 1)
            .setRadioButtonList(RBLsceltaTesto, 2)
            .setRadioButtonList(RBLsceltaTesto, 3)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub Bind_Dati()
        'Definisco i dati relativi alla Rubrica Generica
        Me.CTRLrubrica.setCCN_Address = True
        If Session("AdminForChange") = True Then
            Try
                Me.CTRLrubrica.ComunitaID = Session("idComunita_forAdmin")
                Me.CTRLrubrica.ComunitaPercorso = Session("CMNT_path_forAdmin")
            Catch ex As Exception
            End Try
        Else
            Dim ArrComunita(,) As String
            Try
                Me.CTRLrubrica.ComunitaPercorso = ArrComunita(2, UBound(ArrComunita, 2))
            Catch ex As Exception
                Me.CTRLrubrica.ComunitaPercorso = "." & PageUtility.WorkingCommunityID & "."
            End Try
            Me.CTRLrubrica.ComunitaID = Session("IdComunita")
            Me.CTRLrubrica.Bind_Dati()
        End If
    End Sub

#End Region

    Private Sub BTNconfermaSelezionati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconfermaSelezionati.Click
        If Me.CTRLrubrica.setA_Address Then
            Me.CTRLrubrica.SalvaGruppiSelezionati()
        ElseIf Me.CTRLrubrica.setCC_Address Then
            Me.CTRLrubrica.SalvaGruppiSelezionati()
        ElseIf Me.CTRLrubrica.setCCN_Address Then
            Me.CTRLrubrica.SalvaGruppiSelezionati()
        End If


        If Me.CTRLrubrica.HasSelezionati Then
            Me.TBSselezione.SelectedIndex = 1
            Me.TBSselezione.Tabs(1).Enabled = True
            Me.PNLselezione.Visible = False
            Me.PNLconferma.Visible = True
            Me.TBRconferma.Visible = False

            Me.LBdestinatari.Text = ""
            If Me.CTRLrubrica.setA_Address Then
                Try
                    Me.LBdestinatari.Text = Me.CTRLrubrica.GetDestinatariMail_A
                Catch ex As Exception

                End Try
            ElseIf Me.CTRLrubrica.setCC_Address Then
                Me.CTRLrubrica.SalvaGruppiSelezionati()
                Try
                    Me.LBdestinatari.Text = Me.CTRLrubrica.GetDestinatariMail_CC
                Catch ex As Exception

                End Try
            ElseIf Me.CTRLrubrica.setCCN_Address Then
                Try
                    Me.LBdestinatari.Text = Me.CTRLrubrica.GetDestinatariMail_CCN
                Catch ex As Exception

                End Try
            End If
            Me.Bind_DatiMessaggio()
        Else
            Me.TBRconferma.Visible = True
            Me.TBSselezione.Tabs(1).Enabled = False
            Me.PNLselezione.Visible = True
            Me.PNLconferma.Visible = False
        End If
    End Sub


    Private Sub Bind_DatiMessaggio()
        Dim ForumId, TopicID, PostID As String
        Dim oStringaSelezione As StringaSelezione
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        ForumId = Me.Request.QueryString("ForumId")
        PostID = Me.Request.QueryString("PostID")
        TopicID = Me.Request.QueryString("TopicID")

        Dim NomeForum, NomeThread As String
        If ForumId = "" And PostID = "" And TopicID = "" Then
            Me.BTNinvia.Enabled = False
            Me.TXBtesto.Text = ""
        ElseIf IsNumeric(ForumId) And IsNumeric(TopicID) And IsNumeric(PostID) Then
            Me.BTNinvia.Enabled = True
            oStringaSelezione = StringaSelezione.ForumTopicPost
            Me.TXBtesto.Text = oResource.getValue("messaggio." & Me.RBLsceltaTesto.SelectedValue & "." & Me.StringaSelezione.ForumTopicPost)

            NomeForum = Session("NomeForum")
            If NomeForum = "" Then
                NomeForum = "--"
            End If

            NomeThread = Session("NomeThread")
            If NomeThread = "" Then
                NomeThread = "--"
            End If
        ElseIf IsNumeric(ForumId) And IsNumeric(TopicID) Then
            Dim oForumTopic As COL_Forum_threads

            Me.BTNinvia.Enabled = True
            oStringaSelezione = StringaSelezione.ForumTopic
            Me.TXBtesto.Text = oResource.getValue("messaggio." & Me.RBLsceltaTesto.SelectedValue & "." & Me.StringaSelezione.ForumTopic)

            NomeThread = oForumTopic.EstraiSubject(TopicID)
            If NomeThread = "" Then
                NomeThread = "--"
            End If

            NomeForum = Session("NomeForum")
            If NomeForum = "" Then
                NomeForum = "--"
            End If

        ElseIf IsNumeric(ForumId) Then
            Dim oForum As New COL_Forums
            oForum.Id = ForumId
            oForum.Estrai()
            If oForum.Errore = Errori_Db.None Then
                NomeForum = oForum.Name
            Else
                NomeForum = "--"
            End If

            oStringaSelezione = StringaSelezione.Forum
            Me.TXBtesto.Text = oResource.getValue("messaggio." & Me.RBLsceltaTesto.SelectedValue & "." & Me.StringaSelezione.Forum)
            Me.BTNinvia.Enabled = True
        End If
        If Me.BTNinvia.Enabled = True Then
            Me.TXBtesto.Text = Replace(Me.TXBtesto.Text, "#anagrafica#", Session("objPersona").Cognome & " " & Session("objPersona").Nome)
            Me.TXBtesto.Text = Replace(Me.TXBtesto.Text, "#nomeComunita#", COL_Comunita.EstraiNomeBylingua(Session("IdComunita"), Session("LinguaID")))
            Me.TXBtesto.Text = Replace(Me.TXBtesto.Text, "#nomeForum#", NomeForum)
            Me.TXBtesto.Text = Replace(Me.TXBtesto.Text, "#nomeTopic#", NomeThread)
            Me.TXBtesto.Text = Replace(Me.TXBtesto.Text, "<br>", vbCrLf)
            If oStringaSelezione = StringaSelezione.ForumTopicPost Then
                Dim oForumPost As New COL_Forum_posts
                Me.TXBtesto.Text = Replace(Me.TXBtesto.Text, "#nomePost#", oForumPost.EstraiSubject(PostID))
            End If
        End If
    End Sub

#Region "conferma Mail"

    Private Sub BTNinvia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNinvia.Click
        If Me.CTRLrubrica.HasSelezionati Then
            Dim oPersona As New COL_Persona
            Dim oUtility As New OLDpageUtility(Me.Context)
            Dim oMail As New COL_E_Mail(oUtility.LocalizedMail)

            Dim CMNT_ID, numFile As Integer
            Dim arrListaFiles As New ArrayList
            oPersona = Session("objPersona")

            oMail.Mittente = New MailAddress(oPersona.Mail, oPersona.Anagrafica)
            oMail.IndirizziCCN = Me.CTRLrubrica.Contatti_CCN.GetEmailAddresses
            oMail.Oggetto = Me.LBoggetto.Text
            oMail.Body = Me.TXBtesto.Text
            Dim Link As String = Me.LinkAccessoForum

            oMail.Body = oMail.Body & vbCrLf & vbCrLf & Me.oResource.getValue("messaggio.link")

            oMail.Body = Replace(oMail.Body, "#link#", Link)
            oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
            oMail.Attachment = arrListaFiles

            oMail.SendMailWithRecipientsLimit(Me.PageUtility.SystemSettings.Presenter.DefaultSplitMailRecipients)

            If oMail.Errore = Errori_Db.None Then
                Response.Write("<script language=Javascript>" & "alert('" & Replace(Me.oResource.getValue("mail.inviata"), "'", "\'") & "');" & "</script>")
                Response.Write("<script language='javascript'>{ this.window.close();}</script>")
            Else
                Response.Write("<script language=Javascript>" & "alert('" & Replace(Me.oResource.getValue("mail.NonInviata"), "'", "\'") & "');" & "</script>")
            End If
        End If
    End Sub
#End Region

    Private Sub TBSselezione_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSselezione.TabClick
        If Me.TBSselezione.SelectedIndex = 0 Then
            Me.CTRLrubrica.UpdateForm()
            Me.PNLselezione.Visible = True
            Me.PNLconferma.Visible = False
        Else

            If Me.CTRLrubrica.setA_Address Then
                Me.CTRLrubrica.SalvaGruppiSelezionati()
            ElseIf Me.CTRLrubrica.setCC_Address Then
                Me.CTRLrubrica.SalvaGruppiSelezionati()
            ElseIf Me.CTRLrubrica.setCCN_Address Then
                Me.CTRLrubrica.SalvaGruppiSelezionati()
            End If
            If Me.CTRLrubrica.HasSelezionati Then
                Me.PNLselezione.Visible = False
                Me.PNLconferma.Visible = True
                Me.TBRconferma.Visible = False
                Me.LBdestinatari.Text = ""
                If Me.CTRLrubrica.setA_Address Then
                    Try
                        Me.LBdestinatari.Text = Me.CTRLrubrica.GetDestinatariMail_A
                    Catch ex As Exception

                    End Try
                ElseIf Me.CTRLrubrica.setCC_Address Then
                    Me.CTRLrubrica.SalvaGruppiSelezionati()
                    Try
                        Me.LBdestinatari.Text = Me.CTRLrubrica.GetDestinatariMail_CC
                    Catch ex As Exception

                    End Try
                ElseIf Me.CTRLrubrica.setCCN_Address Then
                    Try
                        Me.LBdestinatari.Text = Me.CTRLrubrica.GetDestinatariMail_CCN
                    Catch ex As Exception

                    End Try

                End If
                Me.Bind_DatiMessaggio()
            Else
                Me.TBRconferma.Visible = True
                Me.TBSselezione.Tabs(1).Enabled = False
                Me.PNLselezione.Visible = True
                Me.PNLconferma.Visible = False
                Me.TBSselezione.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub RBLsceltaTesto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLsceltaTesto.SelectedIndexChanged
        Me.Bind_DatiMessaggio()
    End Sub


    Private Function LinkAccessoForum() As String
        Dim ForumId, TopicID, PostID As String

        '

        ForumId = Me.Request.QueryString("ForumId")
        PostID = Me.Request.QueryString("PostID")
        TopicID = Me.Request.QueryString("TopicID")

        Dim idCommunity As Integer = PageUtility.WorkingCommunityID
        Dim Link As String = ""
        'If PageUtility.SystemSettings.Presenter.isNew Then
        Link = " " & PageUtility.ApplicationUrlBase() & "Forum/LoadForum.aspx?CommunityID=" & idCommunity.ToString
        If Not String.IsNullOrEmpty(ForumId) Then
            Link &= "&ForumID=" & ForumId.Trim
        End If
        If Not String.IsNullOrEmpty(TopicID) Then
            Link &= "&TopicID=" & TopicID.Trim
        End If
        If Not String.IsNullOrEmpty(PostID) Then
            Link &= "&PostID=" & PostID.Trim
        End If
        'Else
        '    Dim oStringaSelezione As StringaSelezione
        '    Dim NomeForum, NomeThread As String
        '    If ForumId = "" And PostID = "" And TopicID = "" Then
        '        Return ""
        '    ElseIf IsNumeric(ForumId) And IsNumeric(TopicID) And IsNumeric(PostID) Then
        '        oStringaSelezione = StringaSelezione.ForumTopicPost
        '    ElseIf IsNumeric(ForumId) And IsNumeric(TopicID) Then
        '        oStringaSelezione = StringaSelezione.ForumTopic
        '    ElseIf IsNumeric(ForumId) Then
        '        oStringaSelezione = StringaSelezione.Forum
        '    End If

        '    Dim oPersona As COL_Persona
        '    Dim RandomCode As String



        '    RandomCode = oPersona.generaPasswordNumerica(8)

        '    Link = " " & Me.PageUtility.ApplicationUrlBase & "AccessoForum.aspx?AddCode=" & RandomCode & idCommunity

        '    RandomCode = oPersona.generaPasswordNumerica(4)
        '    Link = Link & "&ExpUrl=k" & RandomCode & ForumId

        '    If oStringaSelezione = StringaSelezione.ForumTopic Or oStringaSelezione = StringaSelezione.ForumTopicPost Then
        '        RandomCode = oPersona.generaPasswordNumerica(5)
        '        Link = Link & "&ExpUrl2=j" & RandomCode & TopicID
        '    End If
        '    If oStringaSelezione = StringaSelezione.ForumTopicPost Then
        '        RandomCode = oPersona.generaPasswordNumerica(8)
        '        Link = Link & "&for=x25jt" & RandomCode & TopicID
        '    End If
        '    Link = Link & "&action=logon"
        'End If

        Return Link

    End Function

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.Lit_Skin.Text = Me.SkinStyle()
    End Sub
End Class
