Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.File
Imports lm.Comol.Core.BaseModules.Skins
Imports lm.Comol.UI.Presentation

Public Class RevisioneArgomento
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

#Region "Principale"
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

#End Region
    Protected WithEvents DGpost As System.Web.UI.WebControls.DataGrid

    Protected WithEvents Lit_Skin As System.Web.UI.WebControls.Literal

#Region "Label"
    Protected WithEvents LBautore_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmessaggio_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBtitoloMessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
#End Region

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
        Dim RuoloForumID As Integer
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Me.PNLcontenuto.Visible = False
        Else
            Try
                RuoloForumID = Session("RuoloForum")
            Catch ex As Exception
                RuoloForumID = 0
            End Try

            If RuoloForumID <= 0 Then
                Me.PNLcontenuto.Visible = False
            Else
                Dim oServizio As New UCServices.Services_Forum
                Try
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
                    oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
                Catch ex As Exception
                    oServizio.PermessiAssociati = "00000000000000000000000000000000"
                End Try

                If oServizio.AccessoForum Or oServizio.GestioneForum Then
                    Try
                        If Me.Page.IsPostBack = False Then
                            Me.Bind_Dati()
                        End If
                    Catch ex As Exception
                        Me.PNLcontenuto.Visible = False
                    End Try
                Else
                    Me.PNLcontenuto.Visible = False
                End If
            End If
        End If
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                isScaduta = False
                Return False
            End If
        Catch ex As Exception

        End Try
        If isScaduta Then
            Return True
        Else
            Try
                If Session("idComunita") <= 0 Then
                    Return True
                End If
            Catch ex As Exception
                Return True
            End Try
        End If
    End Function

    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim oPersona As New COL_Persona
        Dim oRuoloComunita As New COL_RuoloPersonaComunita
        Dim CMNT_id As Integer

        Dim PermessiAssociati As String

        Try
            oPersona = Session("objPersona")
            PermessiAssociati = Permessi(Codex, Me.Page)

            If (PermessiAssociati = "") Then
                PermessiAssociati = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            PermessiAssociati = "00000000000000000000000000000000"
        End Try

        If Session("AdminForChange") = False Then
            Try
                CMNT_id = Session("IdComunita")
                PermessiAssociati = Permessi(Codex, Me.Page)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try

            Try
                oRuoloComunita.EstraiByLinguaDefault(CMNT_id, oPersona.Id)
                Me.ViewState("PRSN_TPRL_Gerarchia") = oRuoloComunita.TipoRuolo.Gerarchia

            Catch ex As Exception
                Me.ViewState("PRSN_TPRL_Gerarchia") = "99999"
            End Try
        Else
            Dim oComunita As New COL_Comunita
            oComunita.Id = Session("idComunita_forAdmin")

            'Vengo dalla pagina di amministrazione generale
            Try
                PermessiAssociati = oComunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Codex)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"

            End Try
        End If

        Return PermessiAssociati
    End Function

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ElencoForumPost"
        oResource.Folder_Level1 = "Forum"
        oResource.setCulture()
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub Bind_Dati()
        Me.Bind_Post()
    End Sub

    Private Function ReplaceEmoticon(ByVal testo As String) As String
        Dim smile(22) As String
        Dim smileimages(22) As String
        smile(1) = ":-)"
        smile(2) = ":D"
        smile(3) = ":-O"
        smile(4) = ":-p"
        smile(5) = ";-)"
        smile(6) = "(H)"
        smile(7) = ":$"
        smile(8) = "|-)"
        smile(9) = ":("
        smile(10) = ";-("
        smile(11) = ":S"
        smile(12) = ":@"
        smile(13) = "(*)"
        smile(14) = "(L)"
        smile(15) = "(U)"
        smile(16) = "(Y)"
        smile(17) = "(N)"
        smile(18) = "(pp)"
        smile(19) = "8-|"
        smile(20) = "(6)"
        smile(21) = "(?)"

        smileimages(1) = "smiley1.gif"
        smileimages(2) = "smiley4.gif"
        smileimages(3) = "smiley3.gif"
        smileimages(4) = "smiley17.gif"
        smileimages(5) = "smiley2.gif"
        smileimages(6) = "smiley16.gif"
        smileimages(7) = "smiley9.gif"
        smileimages(8) = "smiley12.gif"
        smileimages(9) = "smiley6.gif"
        smileimages(10) = "smiley19.gif"
        smileimages(11) = "smiley5.gif"
        smileimages(12) = "smiley7.gif"
        smileimages(13) = "smiley10.gif"
        smileimages(14) = "smiley27.gif"
        smileimages(15) = "smiley28.gif"
        smileimages(16) = "smiley20.gif"
        smileimages(17) = "smiley21.gif"
        smileimages(18) = "smiley32.gif"
        smileimages(19) = "smiley23.gif"
        smileimages(20) = "smiley15.gif"
        smileimages(21) = "smiley25.gif"

        Dim i As Integer
        For i = 1 To 21
            testo = testo.Replace(smile(i), "<img src=""./../images/forum/smile/" & smileimages(i) & """>")
        Next

        Return testo
    End Function
    Private Sub Bind_Post()
        Dim i, totale As Integer
        Dim oDataset As New DataSet
        Try
            Dim IdThread As Integer = Session("IdThread")
            Dim IdForum As Integer = Session("IdForum")
            Dim oForum_Posts As New COL_Forum_posts
            Dim oRuoloForum As New Main.RuoloForumStandard

            Try
                oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)
            Catch ex As Exception
                oRuoloForum = Main.RuoloForumStandard.Ospite
            End Try

            If oRuoloForum = Main.RuoloForumStandard.Amministratore Or oRuoloForum = Main.RuoloForumStandard.Moderatore Then
                oDataset = oForum_Posts.ElencaByThread(Session("LinguaID"), IdForum, IdThread, Main.FiltroPostApprovazione.Tutti, False, Session("objPersona").id)
            Else
                oDataset = oForum_Posts.ElencaByThread(Session("LinguaID"), IdForum, IdThread, Main.FiltroPostApprovazione.Approvati_PropriCens_Attesa, False, Session("objPersona").id)
            End If

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then

            Else
                oDataset.Tables(0).Columns.Add(New DataColumn("oPRSN_fotoPath"))
                oDataset.Tables(0).Columns.Add(New DataColumn("stile"))
                oDataset.Tables(0).Columns.Add(New DataColumn("stile2"))
                oDataset.Tables(0).Columns.Add(New DataColumn("posizione"))

                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    Dim path As String

                    oRow = oDataset.Tables(0).Rows(i)
                    oRow.Item("posizione") = i
                    If IsDBNull(oRow.Item("PRSN_fotoPath")) Then
                        path = "./../images/noImage.jpg"
                    Else
                        path = "./../Profili/" & oRow.Item("PRSN_id") & "/" & oRow.Item("PRSN_fotoPath")
                        If Not Exists.File(Server.MapPath(path)) Then
                            path = "./../images/noImage.jpg"
                        End If
                    End If

                    oRow.Item("PRSN_fotoPath") = path
                    If Not IsDBNull(oRow.Item("Post_Subject")) Then
                        oRow.Item("Post_Subject") = Server.HtmlEncode(oRow.Item("Post_Subject"))
                    End If

                    If Not IsDBNull(oRow.Item("PRSN_Anagrafica")) Then
                        oRow.Item("PRSN_Anagrafica") = Server.HtmlEncode(oRow.Item("PRSN_Anagrafica"))
                    End If

                    If Not IsDBNull(oRow.Item("Post_body")) Then
                        'oRow.Item("POST_Body") = Server.HtmlEncode(oRow.Item("POST_Body"))
                        oRow.Item("POST_Body") = SmartTagsAvailable.TagAll(oRow.Item("POST_Body"))
                        oRow.Item("POST_Body") = oRow.Item("POST_Body").Replace("[quote]", "<blockquote><font size=1>QUOTE:</font><hr><i>")
                        oRow.Item("POST_Body") = oRow.Item("POST_Body").Replace("[/quote]", "</i><hr></blockquote>")
                        oRow.Item("POST_Body") = oRow.Item("POST_Body").Replace(vbCrLf, "<br>")
                        oRow.Item("POST_Body") = ReplaceEmoticon(oRow.Item("POST_Body"))
                        oRow.Item("POST_Body") = oRow.Item("POST_Body").Replace("#%_234_%#", " ")
                    End If

                    If i = 0 Then
                        oRow.Item("stile") = "ForumNW_RowNormal"
                        oRow.Item("stile2") = "ForumNW_RowNormal_top"
                    ElseIf i Mod 2 = 0 Then
                        oRow.Item("stile") = "ForumNW_RowNormal"
                        oRow.Item("stile2") = "ForumNW_RowNormal_top"
                    Else
                        oRow.Item("stile") = "ForumNW_RowAlternato"
                        oRow.Item("stile2") = "ForumNW_RowAlternato_top"
                    End If
                Next
            End If
            Dim oDataView As DataView
            oDataView = oDataset.Tables(0).DefaultView
            oDataView.Sort = "POST_PostDate DESC"

            DGpost.DataSource = oDataView
            DGpost.DataBind()

            Me.DGpost.Visible = True
        Catch ex As Exception
            Me.DGpost.Visible = False
        End Try
    End Sub

    Private Sub Bind_Menu(ByVal HasReplies As Integer, ByVal oRuoloForum As Main.RuoloForumStandard, ByVal oCell As DataGridItem, ByVal POST_Approved As Integer, ByVal Parent_ID As Integer, ByVal oRuoloUser As Main.RuoloForumStandard, ByVal POST_PRSN_ID As Integer, ByVal POST_Body As String, ByVal POST_ID As Integer)
        Dim LBaccetta, LBmessaggio, LBapprovato As Label

        'TIPO POST
        Dim oTipoPost As New Main.PostApprovazione

        ' Recupero i permessi globali
        Dim oServizio As New UCServices.Services_Forum
        Try
            Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
            oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
        Catch ex As Exception
            oServizio.PermessiAssociati = "00000000000000000000000000000000"
        End Try


        oTipoPost = CType(POST_Approved, Main.PostApprovazione)
        LBapprovato = oCell.FindControl("LBapprovato")
        LBmessaggio = oCell.FindControl("LBMessaggio")


        LBaccetta = oCell.FindControl("LBaccetta")

        ' Setting dei dati


        If Not IsNothing(LBapprovato) Then
            If oTipoPost = Main.PostApprovazione.Approvato Then
                LBapprovato.Visible = False
                LBaccetta.Visible = False
            ElseIf oTipoPost = Main.PostApprovazione.Censurato Then
                Me.oResource.setLabel_To_Value(LBapprovato, "LBapprovato.censurato")
                LBapprovato.Visible = True
                LBaccetta.Visible = False
                If oServizio.GestioneForum = False And (Session("RuoloForum") = Main.RuoloForumStandard.Ospite Or Session("RuoloForum") = Main.RuoloForumStandard.Partecipante) Then
                    Dim oTesto As String
                    If POST_PRSN_ID = Session("objPersona").id Then
                        oTesto = "<br>"
                        oTesto = oTesto & "***********               " & LBapprovato.Text & "         ***********<br>"
                        oTesto = oTesto & POST_Body
                        oTesto = oTesto & "<br><br>"
                        oTesto = oTesto & "***********                                           ***********<br><br>"
                        LBmessaggio.Text = oTesto
                    Else
                        LBmessaggio.Text = "<br><br>" & LBapprovato.Text & "<br><br>"
                    End If
                Else
                    Dim oTesto As String
                    oTesto = "<br>"
                    oTesto = oTesto & "***********               " & LBapprovato.Text & "         ***********<br>"
                    oTesto = oTesto & POST_Body
                    oTesto = oTesto & "<br><br>"
                    oTesto = oTesto & "***********                                           ***********<br><br>"
                    LBmessaggio.Text = oTesto
                End If

            ElseIf oTipoPost = Main.PostApprovazione.InAttesa Then
                Me.oResource.setLabel_To_Value(LBapprovato, "LBapprovato.inattesa")
                LBapprovato.Visible = True
                If oServizio.GestioneForum = False And (Session("RuoloForum") = Main.RuoloForumStandard.Ospite Or Session("RuoloForum") = Main.RuoloForumStandard.Partecipante) Then
                    Dim oTesto As String
                    If POST_PRSN_ID = Session("objPersona").id Then
                        oTesto = "<br>"
                        oTesto = oTesto & "***********       " & LBapprovato.Text & "     ***********<br>"
                        oTesto = oTesto & POST_Body
                        oTesto = oTesto & "<br><br>"
                        oTesto = oTesto & "***********                                           ***********<br><br>"
                        LBmessaggio.Text = oTesto
                    Else
                        LBmessaggio.Text = "<br><br>" & LBapprovato.Text & "<br><br>"
                    End If
                Else
                    Dim oTesto As String
                    oTesto = "<br>"
                    oTesto = oTesto & "***********       " & LBapprovato.Text & "     ***********<br>"
                    oTesto = oTesto & POST_Body
                    oTesto = oTesto & "<br><br>"
                    oTesto = oTesto & "***********                                           ***********<br><br>"
                    LBmessaggio.Text = oTesto
                End If


                If oServizio.GestioneForum Then
                    LBaccetta.Visible = True
                ElseIf Session("RuoloForum") = Main.RuoloForumStandard.Amministratore Or Session("RuoloForum") = Main.RuoloForumStandard.Moderatore Then
                    '  amministratore del forum o moderatore
                    LBaccetta.Visible = True
                Else
                    LBaccetta.Visible = False
                End If
            End If
        End If
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGpost_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGpost.ItemDataBound
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                Dim oLabel As Label
                oLabel = e.Item.FindControl("LBautore_t")
                If Not IsNothing(oLabel) Then
                    oResource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBmessaggio_t")
                If Not IsNothing(oLabel) Then
                    oResource.setLabel(oLabel)
                End If
            Catch ex As Exception

            End Try
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Try
                'DATA
                Dim oLBdataPost As Label
                Dim oDataInserimento, oDataModifica As DateTime
                oLBdataPost = e.Item.FindControl("LBdataPost")

                If Not IsNothing(oLBdataPost) Then

                    Me.oResource.setLabel(oLBdataPost)
                    If IsDBNull(e.Item.DataItem("POST_PostDate")) Then
                        oLBdataPost.Text = ""
                    Else
                        oDataInserimento = e.Item.DataItem("POST_PostDate")

                        oLBdataPost.Text = Replace(oLBdataPost.Text, "#datapost#", oDataInserimento.ToString("D", oResource.CultureInfo))
                        oLBdataPost.Text = Replace(oLBdataPost.Text, "#orapost#", FormatDateTime(oDataInserimento, DateFormat.ShortTime))
                    End If
                End If

                Dim oLBdataPostModificato As Label
                oLBdataPostModificato = e.Item.FindControl("LBdataPostModificato")
                If Not IsNothing(oLBdataPostModificato) Then

                    Me.oResource.setLabel(oLBdataPostModificato)
                    If IsDBNull(e.Item.DataItem("POST_ModificatoIl")) Then
                        oLBdataPostModificato.Text = ""
                        oLBdataPostModificato.Visible = False
                    Else
                        oDataModifica = e.Item.DataItem("POST_ModificatoIl")
                        If IsNothing(oLBdataPost) = False Then
                            If Not Equals(oDataInserimento, New Date) Then
                                Me.oResource.setLabel(oLBdataPost)
                                oLBdataPost.Text = Replace(oLBdataPost.Text, "#datapost#", FormatDateTime(oDataInserimento, DateFormat.ShortDate))
                                oLBdataPost.Text = Replace(oLBdataPost.Text, "#orapost#", FormatDateTime(oDataInserimento, DateFormat.ShortTime))
                            End If
                        End If
                        oLBdataPostModificato.Text = Replace(oLBdataPostModificato.Text, "#datapost#", oDataModifica.ToString("D", oResource.CultureInfo))
                        oLBdataPostModificato.Text = Replace(oLBdataPostModificato.Text, "#orapost#", FormatDateTime(oDataModifica, DateFormat.ShortTime))
                        oLBdataPostModificato.Visible = True
                    End If
                End If

                Try
                    Dim oLBvoto_t As Label
                    oLBvoto_t = e.Item.FindControl("LBvoto_t")

                    If IsNothing(oLBvoto_t) = False Then
                        Me.oResource.setLabel(oLBvoto_t)

                        If IsDBNull(e.Item.DataItem("VotoMedio")) = False Then
                            oLBvoto_t.Text = Replace(oLBvoto_t.Text, "#voto#", e.Item.DataItem("VotoMedio"))
                            oLBvoto_t.Visible = True
                        Else
                            oLBvoto_t.Visible = False
                        End If

                    End If
                Catch ex As Exception

                End Try


                Try
                    Dim oLBtotalePost_t As Label
                    oLBtotalePost_t = e.Item.FindControl("LBtotalePost_t")
                    Me.oResource.setLabel(oLBtotalePost_t)
                Catch ex As Exception

                End Try


                'PHOTOPATH
                Dim oImage As New System.web.ui.webcontrols.Image
                oImage = e.Item.FindControl("IMGmessaggio")
                If Not IsNothing(oImage) Then
                    If IsDBNull(e.Item.DataItem("FRIM_Image")) Then
                        oImage.Visible = False
                    Else
                        If e.Item.DataItem("FRIM_id") = 1 Then
                            oImage.Visible = False
                        Else
                            oImage.ImageUrl = "./../" & e.Item.DataItem("FRIM_Image")
                            oImage.ToolTip = e.Item.DataItem("FRIM_Nome")
                            oImage.Visible = True
                        End If

                    End If
                End If

                Me.Bind_Menu(e.Item.DataItem("HasReplies"), e.Item.DataItem("POST_IdRuolo"), e.Item, e.Item.DataItem("POST_Approved"), e.Item.DataItem("Post_parentId"), e.Item.DataItem("POST_IdRuolo"), e.Item.DataItem("POST_PRSN_Id"), e.Item.DataItem("POST_Body"), e.Item.DataItem("POST_ID"))

            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region

    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            Return ManagerConfiguration.GetSmartTags(Me.PageUtility.ApplicationUrlBase(True))
        End Get
    End Property

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.Lit_Skin.Text = Me.SkinStyle()
    End Sub
End Class