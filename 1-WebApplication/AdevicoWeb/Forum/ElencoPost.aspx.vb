Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Forum
Imports lm.ActionDataContract
Imports lm.Comol.Core.File

Public Class ElencoPost
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

#Region "Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Principale"
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents CHBnotificaMail As System.Web.UI.WebControls.CheckBox

    'Protected WithEvents TBLprincipale As System.Web.UI.WebControls.Table
    Protected WithEvents TBRmovimento0 As HtmlControl

    Protected WithEvents LNBforum As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBtopics As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBjumpToforum As System.Web.UI.WebControls.Label
    Protected WithEvents DDLforum As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RBLordinamentoPost As System.Web.UI.WebControls.RadioButtonList

    Protected WithEvents TBRmovimento1 As HtmlControl
    Protected WithEvents LNBforum_bottom As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBtopics_bottom As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBjumpToforum_bottom As System.Web.UI.WebControls.Label
    Protected WithEvents DDLforum_bottom As System.Web.UI.WebControls.DropDownList
#End Region
    Protected WithEvents DGpost As System.Web.UI.WebControls.DataGrid

    Protected WithEvents IMGvota As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IMBcancellaPost As System.Web.UI.WebControls.ImageButton

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

#Region "Link"
    Protected WithEvents LKBlistaThread As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBreply As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcensura As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodifica As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBreplyQuote As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBaccetta As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBlistaAlbero As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBlistaThread_2 As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Label"
    Protected WithEvents LBautore_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmessaggio_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBtitoloMessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label

#End Region

#Region "Pannello Inserisci Post"
    Protected WithEvents PNLnewMessage As System.Web.UI.WebControls.Panel
    Protected WithEvents CTRLaggiungiPost As Comunita_OnLine.UC_AggiungiPost

#Region "Menu Inserimento"
    Protected WithEvents PNLmenuInserimento As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaInserimento As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBrispondi As System.Web.UI.WebControls.LinkButton
#End Region

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim RuoloForumID As Integer
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Try
            RuoloForumID = Session("RuoloForum")
        Catch ex As Exception
            RuoloForumID = 0
        End Try
        If RuoloForumID <= 0 Then
			Me.PageUtility.RedirectToUrl("Forum/forums.aspx")
		Else
			Dim oServizio As New UCServices.Services_Forum
			Dim PermessiAssociati As String
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizio.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try

			If oServizio.AccessoForum Or oServizio.GestioneForum Then
				Me.PNLpermessi.Visible = False
				Me.PNLmenu.Visible = True
				Try
					If Me.Page.IsPostBack = False Then
						Me.Bind_Dati()
					End If
                    Me.PageUtility.AddAction(ActionType.PostList, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Session("IdThread")), InteractionType.UserWithLearningObject)
                Catch ex As Exception
                    Me.PNLmenu.Visible = False
                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                End Try
            Else
                Me.PageUtility.AddAction(ActionType.NoPermission, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Session("IdThread")), InteractionType.UserWithLearningObject)
                Me.PNLmenu.Visible = False
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
			End If
		End If
		If Me.Page.IsPostBack = False Then
			Session("Azione") = "load"
			Me.SetupInternazionalizzazione()
		End If
	End Sub

	Private Function SessioneScaduta() As Boolean
		Dim oPersona As COL_Persona
		Dim isScaduta As Boolean = True
		Try
			oPersona = Session("objPersona")
			If oPersona.ID > 0 Then
				isScaduta = False
				Return False
			End If
		Catch ex As Exception

		End Try
		If isScaduta Then
			Dim alertMSG As String
			alertMSG = oResource.getValue("LogoutMessage")
			If alertMSG <> "" Then
				alertMSG = alertMSG.Replace("'", "\'")
			Else
				alertMSG = "Session timeout"
            End If
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
			Response.End()

			Return True
		Else
			Try
				If Session("idComunita") <= 0 Then
					Me.ExitToLimbo()
					Return True
				End If
			Catch ex As Exception
				Me.ExitToLimbo()
				Return True
			End Try
		End If
	End Function

	Private Sub ExitToLimbo()
		Session("Limbo") = True
		Session("ORGN_id") = 0
		Session("IdRuolo") = ""
		Session("ArrPermessi") = ""
		Session("RLPC_ID") = ""

		Session("AdminForChange") = False
		Session("CMNT_path_forAdmin") = ""
		Session("idComunita_forAdmin") = ""
		Session("TPCM_ID") = ""
		Me.Response.Expires = 0
		Me.PageUtility.RedirectToUrl("Comunita/EntrataComunita.aspx")

	End Sub

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
				oRuoloComunita.EstraiByLinguaDefault(CMNT_id, oPersona.ID)
				Me.ViewState("PRSN_TPRL_Gerarchia") = oRuoloComunita.TipoRuolo.Gerarchia

			Catch ex As Exception
				Me.ViewState("PRSN_TPRL_Gerarchia") = "99999"
			End Try
		Else
			Dim oComunita As New COL_Comunita
			oComunita.Id = Session("idComunita_forAdmin")

			'Vengo dalla pagina di amministrazione generale
			Try
				PermessiAssociati = oComunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Session("idComunita_forAdmin"), Codex)
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
	Private Sub SetupInternazionalizzazione()
		With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")

			.setLabel(Me.LBNopermessi)
			.setLabel(LBjumpToforum)
			.setCheckBox(CHBnotificaMail)
			.setLinkButton(Me.LNBforum, True, True)
			.setLinkButton(Me.LNBforum_bottom, True, True)
			Me.LBjumpToforum_bottom.Text = Me.LBjumpToforum.Text
			Dim NomeForum As String = Session("NomeForum")
			If NomeForum <> "" Then
				Me.LNBtopics.Text = Server.HtmlEncode(Session("NomeForum"))
			Else
				Me.LNBtopics.Text = "Topic List"
			End If


			Me.LNBtopics.Attributes.Add("onclick", "status='';return true;")
			Me.LNBtopics.Attributes.Add("onmouseover", "status='';return true;")
			Me.LNBtopics.Attributes.Add("onmouseout", "status='';return true;")

			Me.LNBtopics_bottom.Text = Me.LNBtopics.Text
			Me.LNBtopics_bottom.Attributes.Add("onclick", "status='';return true;")
			Me.LNBtopics_bottom.Attributes.Add("onmouseover", "status='';return true;")
			Me.LNBtopics_bottom.Attributes.Add("onmouseout", "status='';return true;")

			.setRadioButtonList(Me.RBLordinamentoPost, 0)
			.setRadioButtonList(Me.RBLordinamentoPost, 1)

			.setLinkButton(Me.LNBannullaInserimento, True, True)
		End With
	End Sub
#End Region

#Region "Bind_Dati"
	Private Sub Bind_Dati()
		Me.Bind_Checkbox()
		If Session("ForumIsArchiviato") = True Then
			Me.Bind_ForumList(Main.FiltroArchiviazione.Archiviato)
		Else
			Me.Bind_ForumList(Main.FiltroArchiviazione.NonArchiviato)
		End If
		If Me.Page.IsPostBack = False Then
			Dim oImpostazioni As New COL_ImpostazioniUtente
			Try
				If IsNothing(Session("oImpostazioni")) Then
					Me.RBLordinamentoPost.SelectedIndex = 1
				Else
					Try
						oImpostazioni = Session("oImpostazioni")
						Me.RBLordinamentoPost.SelectedValue = oImpostazioni.OrdinamentoPost
					Catch ex As Exception
						Me.RBLordinamentoPost.SelectedIndex = 1
					End Try

				End If
			Catch ex As Exception
				Me.RBLordinamentoPost.SelectedIndex = 1
			End Try
		End If
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

	Private Sub Bind_ForumList(ByVal oArchiviazione As Main.FiltroArchiviazione)
		Dim oDataset As DataSet
		Dim oForum As New COL_Forums


		Me.DDLforum.Items.Clear()
		Me.DDLforum_bottom.Items.Clear()
		Try
			Dim CMNT_ID, RLPC_id As Integer
			oForum.Id = Session("IdForum")

			If Session("AdminForChange") = False Then
				CMNT_ID = Session("IdComunita")
				RLPC_id = Session("RLPC_ID")
			Else
				Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

				CMNT_ID = Session("idComunita_forAdmin")
				oRuoloPersonaComunita.Estrai(CMNT_ID, Session("objPersona"))
				RLPC_id = oRuoloPersonaComunita.Id
			End If
			oDataset = oForum.ElencaByComunitaIscritto(Session("objPersona").id, CMNT_ID, RLPC_id, oArchiviazione)
			Me.DDLforum.DataSource = oDataset
			Me.DDLforum.DataValueField = "FRUM_ID"
			Me.DDLforum.DataTextField = "FRUM_Name"
			Me.DDLforum.DataBind()

			Me.DDLforum_bottom.DataSource = oDataset
			Me.DDLforum_bottom.DataValueField = "FRUM_ID"
			Me.DDLforum_bottom.DataTextField = "FRUM_Name"
			Me.DDLforum_bottom.DataBind()
		Catch ex As Exception

		End Try
		Me.DDLforum.Items.Insert(0, New ListItem("-- Seleziona il forum --", 0))
		Me.oResource.setDropDownList(Me.DDLforum, 0)
		Me.DDLforum_bottom.Items.Insert(0, Me.DDLforum.Items(0))
	End Sub
	Private Sub Bind_Checkbox()
		Try
			Dim oPersona As New COL_Persona
			Dim oThread As New COL_Forum_threads

			oPersona = Session("objPersona")
			oThread.Id = Session("IdThread")
			Me.CHBnotificaMail.Checked = oThread.VerificaNotificaByPersona(oPersona.ID)
		Catch ex As Exception

		End Try
	End Sub
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
				'nascondo la DL e visualizzo messaggio
				Me.PageUtility.RedirectToUrl("Forum/ForumThreads.aspx")
				Me.TBRmovimento1.Visible = False
			Else
				oDataset.Tables(0).Columns.Add(New DataColumn("oPRSN_fotoPath"))
				oDataset.Tables(0).Columns.Add(New DataColumn("stile"))
				oDataset.Tables(0).Columns.Add(New DataColumn("stile2"))
				oDataset.Tables(0).Columns.Add(New DataColumn("posizione"))


				If totale < 8 Then
					Me.TBRmovimento1.Visible = False
				Else
					Me.TBRmovimento1.Visible = Me.TBRmovimento0.Visible
				End If
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
			If Me.RBLordinamentoPost.SelectedIndex = 0 Then
				oDataView.Sort = "POST_PostDate ASC"
			Else
				oDataView.Sort = "POST_PostDate DESC"
			End If

			DGpost.DataSource = oDataView
			DGpost.DataBind()

			Me.DGpost.Visible = True
		Catch ex As Exception
			Me.TBRmovimento1.Visible = False
			Me.DGpost.Visible = False
		End Try
	End Sub

	Private Sub Bind_Menu(ByVal HasReplies As Integer, ByVal oRuoloForum As Main.RuoloForumStandard, ByVal oCell As DataGridItem, ByVal POST_Approved As Integer, ByVal Parent_ID As Integer, ByVal oRuoloUser As Main.RuoloForumStandard, ByVal POST_PRSN_ID As Integer, ByVal POST_Body As String, ByVal POST_ID As Integer, ByVal HasVotato As Boolean)
		Dim LNBreply, LNBreplyQuote, LNBaccetta, LNBcensura, LNBmodifica, LNBsegnala As LinkButton
		Dim LBaccetta, LBreplyQuote, LBcensura, LBmodifica, LBmessaggio, LBapprovato, LBcancellaPost, LBhasSegnala As Label

		'TIPO POST
		Dim oTipoPost As New Main.PostApprovazione

		' Recupero i permessi globali
		Dim oServizio As New UCServices.Services_Forum
		Dim PermessiAssociati As String
		Try
			Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizio.Codex)
			oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
		Catch ex As Exception
			oServizio.PermessiAssociati = "00000000000000000000000000000000"
		End Try


		oTipoPost = CType(POST_Approved, Main.PostApprovazione)
		LBapprovato = oCell.FindControl("LBapprovato")
		LBmessaggio = oCell.FindControl("LBMessaggio")


		LBreplyQuote = oCell.FindControl("LBreplyQuote")
		LBmodifica = oCell.FindControl("LBmodifica")
		LBcensura = oCell.FindControl("LBcensura")
		LBcancellaPost = oCell.FindControl("LBcancellaPost")
		LBhasSegnala = oCell.FindControl("LBhasSegnala")
		LBaccetta = oCell.FindControl("LBaccetta")

		LNBreply = oCell.FindControl("LNBreply")
		If Not IsNothing(LNBreply) Then
			Me.oResource.setLinkButton(LNBreply, True, True)
			LNBreply.CommandArgument = Parent_ID
		End If

		LNBmodifica = oCell.FindControl("LNBmodifica")
		If Not IsNothing(LNBmodifica) Then
			Me.oResource.setLinkButton(LNBmodifica, True, True)
			LNBmodifica.CommandArgument = Parent_ID
		End If

		LNBaccetta = oCell.FindControl("LNBaccetta")
		If Not IsNothing(LNBaccetta) Then
			Me.oResource.setLinkButton(LNBaccetta, True, True)
			LNBaccetta.CommandArgument = Parent_ID
		End If

		LNBreplyQuote = oCell.FindControl("LNBreplyQuote")
		If Not IsNothing(LNBreplyQuote) Then
			Me.oResource.setLinkButton(LNBreplyQuote, True, True)
			LNBreplyQuote.CommandArgument = Parent_ID
		End If


		' Setting dei dati
        Dim IdCurrentUserRole As New Main.RuoloForumStandard

        Try
            IdCurrentUserRole = CType(Session("RuoloForum"), Main.RuoloForumStandard)
        Catch ex As Exception
            IdCurrentUserRole = Main.RuoloForumStandard.Ospite
        End Try

		If Not IsNothing(LBapprovato) Then
			If oTipoPost = Main.PostApprovazione.Approvato Then
				LBapprovato.Visible = False
				LBaccetta.Visible = False
				LNBaccetta.Visible = False
			ElseIf oTipoPost = Main.PostApprovazione.Censurato Then
				Me.oResource.setLabel_To_Value(LBapprovato, "LBapprovato.censurato")
				LBapprovato.Visible = True
				LBaccetta.Visible = False
				LNBaccetta.Visible = False
                If oServizio.GestioneForum = False And (IdCurrentUserRole = Main.RuoloForumStandard.Ospite Or IdCurrentUserRole = Main.RuoloForumStandard.Partecipante) Then
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
                If oServizio.GestioneForum = False And (IdCurrentUserRole = Main.RuoloForumStandard.Ospite Or IdCurrentUserRole = Main.RuoloForumStandard.Partecipante) Then
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
					LNBaccetta.Visible = True
                ElseIf IdCurrentUserRole = Main.RuoloForumStandard.Amministratore Or IdCurrentUserRole = Main.RuoloForumStandard.Moderatore Then
                    '  amministratore del forum o moderatore
                    LBaccetta.Visible = True
                    LNBaccetta.Visible = True
				Else
					LBaccetta.Visible = False
					LNBaccetta.Visible = False
				End If
			End If
			If Session("ThreadArchiviato") Or Session("ForumIsArchiviato") Then
				LNBaccetta.Enabled = False
			Else
				LNBaccetta.Enabled = True
			End If
		End If


		LNBcensura = oCell.FindControl("LNBcensura")
		If Not IsNothing(LNBcensura) Then
			If POST_Approved = Main.PostApprovazione.Censurato Then
				Me.oResource.setLinkButtonToValue(LNBcensura, "Censurato", True, True)
			Else
				Me.oResource.setLinkButtonToValue(LNBcensura, "NonCensurato", True, True)
			End If
			LNBcensura.CommandArgument = Parent_ID

			If oServizio.GestioneForum Then
				LBcensura.Visible = True
				LNBcensura.Visible = True
            ElseIf IdCurrentUserRole = Main.RuoloForumStandard.Amministratore And oRuoloForum <> Main.RuoloForumStandard.Amministratore Then
                LBcensura.Visible = True
                LNBcensura.Visible = True
            ElseIf IdCurrentUserRole = Main.RuoloForumStandard.Moderatore And (oRuoloForum <> Main.RuoloForumStandard.Amministratore And oRuoloForum <> Main.RuoloForumStandard.Moderatore) Then
                LBcensura.Visible = True
                LNBcensura.Visible = True
			Else
				LBcensura.Visible = False
				LNBcensura.Visible = False
			End If
			If Session("ThreadArchiviato") Or Session("ForumIsArchiviato") Then
				LNBcensura.Enabled = False
			Else
				LNBcensura.Enabled = True
			End If
		End If


		'cancella
		Dim oIMBcancellaPost As ImageButton
		Try
			oIMBcancellaPost = oCell.FindControl("IMBcancellaPost")
			If Not IsNothing(oIMBcancellaPost) Then
				If Parent_ID = 0 Then
					Me.oResource.setImageButton_To_Value(oIMBcancellaPost, False, "EliminaTopic", True, True, True)
				Else
					Me.oResource.setImageButton_To_Value(oIMBcancellaPost, False, "EliminaPost", True, True, True)
				End If
			End If

			If oServizio.GestioneForum Then
				LBcancellaPost.Visible = True
				oIMBcancellaPost.Visible = True
            ElseIf IdCurrentUserRole = Main.RuoloForumStandard.Amministratore And oRuoloForum <> Main.RuoloForumStandard.Amministratore Then
                LBcancellaPost.Visible = True
                oIMBcancellaPost.Visible = True
            ElseIf IdCurrentUserRole = Main.RuoloForumStandard.Moderatore And (oRuoloForum <> Main.RuoloForumStandard.Amministratore And oRuoloForum <> Main.RuoloForumStandard.Moderatore) Then
                LBcancellaPost.Visible = True
                oIMBcancellaPost.Visible = True
			Else
				LBcancellaPost.Visible = False
				oIMBcancellaPost.Visible = False
			End If

			If Session("ThreadArchiviato") Or Session("ForumIsArchiviato") Then
				oIMBcancellaPost.Enabled = False
			Else
				oIMBcancellaPost.Enabled = True
			End If
		Catch ex As Exception

		End Try

        'Dim IdForumRole As Integer = Main.RuoloForumStandard.Ospite
        'Try
        '    IdForumRole = Session("RuoloForum")
        'Catch ex As Exception

        'End Try

        Try

            LNBsegnala = oCell.FindControl("LNBsegnala")

            If Session("ThreadArchiviato") Or Session("ForumIsArchiviato") Then
                If oServizio.GestioneForum Or IdCurrentUserRole = Main.RuoloForumStandard.Amministratore Or IdCurrentUserRole = Main.RuoloForumStandard.Moderatore Then
                    LBhasSegnala.Visible = True
                    LNBsegnala.Visible = True
                Else
                    LBhasSegnala.Visible = False
                    LNBsegnala.Visible = False
                End If
            ElseIf IdCurrentUserRole <> Main.RuoloForumStandard.Ospite Then
                If POST_Approved = Main.PostApprovazione.Censurato Or POST_Approved = Main.PostApprovazione.InAttesa Then
                    If oServizio.GestioneForum Or IdCurrentUserRole = Main.RuoloForumStandard.Amministratore Or IdCurrentUserRole = Main.RuoloForumStandard.Moderatore Then
                        LBhasSegnala.Visible = True
                        LNBsegnala.Visible = True
                    Else
                        LBhasSegnala.Visible = False
                        LNBsegnala.Visible = False
                    End If
                Else
                    LBhasSegnala.Visible = True
                    LNBsegnala.Visible = True
                End If
            Else
                LBhasSegnala.Visible = True
                LNBsegnala.Visible = True
            End If
            If LNBsegnala.Visible Then
                Dim i_link As String
                i_link = "./NotificaMessaggio.aspx?ForumId=" & Session("IdForum") & "&TopicID=" & Session("IdThread") & "&PostID= " & POST_ID

                Me.oResource.setLinkButton(LNBsegnala, True, True)
                LNBsegnala.Attributes.Add("onClick", "OpenWin('" & i_link & "','800','550','no','yes');return false;")
            End If
        Catch ex As Exception

        End Try

		Try
            If IdCurrentUserRole <> Main.RuoloForumStandard.Ospite Then
                If Session("ThreadArchiviato") Or Session("ForumIsArchiviato") Then
                    LNBreply.Visible = False
                    LNBreplyQuote.Visible = False
                    LNBmodifica.Visible = False
                Else
                    If POST_Approved = Main.PostApprovazione.Approvato Then
                        LNBreply.Visible = True
                        LNBreplyQuote.Visible = True

                        If POST_PRSN_ID = Session("objPersona").id And HasReplies = 0 Then
                            LNBmodifica.Visible = True
                        Else
                            LNBmodifica.Visible = False
                        End If
                    Else
                        LNBreply.Visible = False
                        LNBreplyQuote.Visible = False
                        LNBmodifica.Visible = False
                    End If
                End If
            Else
                LNBreply.Visible = False
                LNBreplyQuote.Visible = False
                LNBmodifica.Visible = False
            End If
		Catch ex As Exception

		End Try

		Try
			LBreplyQuote.Visible = LNBreply.Visible And LNBreplyQuote.Visible
		Catch ex As Exception

		End Try
		Try
			LBmodifica.Visible = LNBmodifica.Visible And (LNBreply.Visible Or LNBreplyQuote.Visible)
		Catch ex As Exception

		End Try

		Try
			LBhasSegnala.Visible = LNBsegnala.Visible And (LNBmodifica.Visible Or LNBreply.Visible Or LNBreplyQuote.Visible)
		Catch ex As Exception

		End Try

		Try
			LBcensura.Visible = LNBcensura.Visible And (LNBsegnala.Visible Or LNBmodifica.Visible Or LNBreply.Visible Or LNBreplyQuote.Visible)
		Catch ex As Exception

		End Try
		Try
			LBaccetta.Visible = LNBaccetta.Visible And (LNBcensura.Visible Or LNBsegnala.Visible Or LNBmodifica.Visible Or LNBreply.Visible Or LNBreplyQuote.Visible)
		Catch ex As Exception

		End Try
		Try
			LBcancellaPost.Visible = IMBcancellaPost.Visible And (LNBaccetta.Visible Or LNBcensura.Visible Or LNBsegnala.Visible Or LNBmodifica.Visible Or LNBreply.Visible Or LNBreplyQuote.Visible)
		Catch ex As Exception

		End Try


		Try

			Dim oLBvotoTesto As Label
			Dim oSelect As HtmlControls.HtmlSelect
			Dim oIMGvota As ImageButton


			oIMGvota = oCell.FindControl("IMGvota")
			oLBvotoTesto = oCell.FindControl("LBvotoTesto")
			oSelect = oCell.FindControl("SL_voto_")

            If oTipoPost = Main.PostApprovazione.Approvato AndAlso IdCurrentUserRole <> Main.RuoloForumStandard.Ospite Then
                If HasVotato Then
                    oLBvotoTesto.Visible = False
                    oIMGvota.Visible = False
                    oSelect.Visible = False
                Else
                    Me.oResource.setLabel(oLBvotoTesto)
                    oLBvotoTesto.Visible = True
                    oIMGvota.Visible = True
                    oSelect.Visible = True
                End If
            Else
                oLBvotoTesto.Visible = False
                oIMGvota.Visible = False
                oSelect.Visible = False
            End If

		Catch ex As Exception


		End Try
	End Sub
#End Region

#Region "Gestione Griglia"
	Private Sub DGpost_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGpost.ItemDataBound
		'Dim oRuoloForum As New Main.RuoloForumStandard
		'oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)

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
				Dim oLinkButton As LinkButton

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
                                oLBdataPost.Text = Replace(oLBdataPost.Text, "#datapost#", oDataInserimento.ToString("D", oResource.CultureInfo))
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
							oLBvoto_t.Text = Replace(oLBvoto_t.Text, "#numvoti#", e.Item.DataItem("TotaleVoti"))
							oLBvoto_t.Visible = True
						Else
							oLBvoto_t.Visible = False
						End If

					End If
				Catch ex As Exception

				End Try


				Dim HasVotato As Boolean = False
				Try
					HasVotato = e.Item.DataItem("HasVotato")
				Catch ex As Exception

				End Try


				Try
					Dim oLBtotalePost_t As Label
					oLBtotalePost_t = e.Item.FindControl("LBtotalePost_t")
					Me.oResource.setLabel(oLBtotalePost_t)
				Catch ex As Exception

				End Try


				'PHOTOPATH
				Dim oImage As New System.Web.UI.WebControls.Image
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

				Dim oHyperLink As HyperLink
				oHyperLink = e.Item.FindControl("HYLparent")
				If Not IsNothing(oHyperLink) Then
					If e.Item.DataItem("POST_ParentID") = 0 Then
						oHyperLink.Visible = False
					Else
						oHyperLink.Visible = True
						Me.oResource.setHyperLink(oHyperLink, True, True)
						oHyperLink.NavigateUrl = "#post_" & e.Item.DataItem("POST_ParentID")
					End If
				End If

				Me.Bind_Menu(e.Item.DataItem("HasReplies"), e.Item.DataItem("POST_IdRuolo"), e.Item, e.Item.DataItem("POST_Approved"), e.Item.DataItem("Post_parentId"), e.Item.DataItem("POST_IdRuolo"), e.Item.DataItem("POST_PRSN_Id"), e.Item.DataItem("POST_Body"), e.Item.DataItem("POST_ID"), HasVotato)

				Dim test As String = e.Item.DataItem("POST_Body")
				test &= ""

			Catch ex As Exception

			End Try
		End If
	End Sub

	Private Sub DGpost_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGpost.ItemCommand
		Dim POST_Id, FRIM_ID, POST_Approved, POST_FRUM_Id, POST_THRD_ID, POST_PostLevel As Integer
		'  Dim POST_Body, POST_Subject As String
		Try
			Dim oForumPost As New COL_Forum_posts

			POST_Id = CInt(Me.DGpost.DataKeys.Item(e.Item.ItemIndex))

			'POST_Body = Me.DGpost.Items(e.Item.ItemIndex).Cells(7).Text()
			'POST_Subject = Me.DGpost.Items(e.Item.ItemIndex).Cells(8).Text()
			'FRIM_ID = Me.DGpost.Items(e.Item.ItemIndex).Cells(2).Text()
			POST_Approved = Me.DGpost.Items(e.Item.ItemIndex).Cells(1).Text()

			'     POST_FRUM_Id = Me.DGpost.Items(e.Item.ItemIndex).Cells(11).Text()
			'   POST_THRD_ID = Me.DGpost.Items(e.Item.ItemIndex).Cells(12).Text()
			'    POST_PostLevel = Me.DGpost.Items(e.Item.ItemIndex).Cells(13).Text()

			If IsNothing(oResource) Then
				Me.SetCulture(Session("LinguaCode"))
			End If

            Dim oServiceNotification As New ForumNotificationUtility(Me.PageUtility)
            oForumPost.Id = POST_Id
            oForumPost.Estrai()

            Dim oForum As New COL_Forums
            oForum.Id = oForumPost.FRUM_id
            oForum.Estrai()

            Dim oTopicName As String = ""

			If e.CommandName = "censura" Then
				If POST_Approved = Main.PostApprovazione.Approvato Then
                    oForumPost.Censura(POST_Id)

                    oTopicName = COL_Forum_threads.EstraiSubject(oForumPost.ThreadID)

                    oServiceNotification.NotifyPostCensured(Me.PageUtility.WorkingCommunityID, oForum.Id, oForum.Name, oForumPost.ThreadID, oTopicName, POST_Id, oForumPost.Subject, oForumPost.PRSN_Id, COL_Persona.GetPersona(oForumPost.PRSN_Id, 1).Anagrafica)

					Me.Bind_Post()
				ElseIf POST_Approved = Main.PostApprovazione.Censurato Then
					oForumPost.Riabilita(POST_Id)
					Me.Bind_Post()
				End If
			ElseIf e.CommandName = "modifica" Then
				Session("Azione") = "modifica"
				CTRLaggiungiPost.Azione = CTRLaggiungiPost.Action.Modifica
				CTRLaggiungiPost.Bind_MessageForModifica(POST_Id)
				Me.PNLcontenuto.Visible = False
				Me.PNLnewMessage.Visible = True
				Me.PNLmenu.Visible = False
				Me.PNLmenuInserimento.Visible = True
				oResource.setLinkButtonToValue(Me.LNBrispondi, "modifica", True, True)

			ElseIf e.CommandName = "reply" Then
				Session("Azione") = "inserisci"
				Me.PNLcontenuto.Visible = False
				Me.PNLnewMessage.Visible = True
				Me.PNLmenu.Visible = False
				Me.PNLmenuInserimento.Visible = True
				CTRLaggiungiPost.Azione = CTRLaggiungiPost.Action.Inserisci
				oResource.setLinkButtonToValue(Me.LNBrispondi, "inserisci", True, True)
				CTRLaggiungiPost.Bind_MessageForReply(False, False, POST_Id)

			ElseIf e.CommandName = "replyQuota" Then
				Me.PNLcontenuto.Visible = False
				Me.PNLnewMessage.Visible = True
				Me.PNLmenu.Visible = False
				Me.PNLmenuInserimento.Visible = True
				Session("Azione") = "inserisci"
				CTRLaggiungiPost.Azione = CTRLaggiungiPost.Action.Inserisci
				CTRLaggiungiPost.Bind_MessageForReply(False, True, POST_Id)
				oResource.setLinkButtonToValue(Me.LNBrispondi, "inserisci", True, True)

			ElseIf e.CommandName = "accetta" Then
				Dim oPersona As New COL_Persona
				Dim oResourceConfig As New ResourceManager
				oResourceConfig = GetResourceConfig(Session("LinguaCode"))
				oPersona = Session("objPersona")
                oForumPost.Riabilita(POST_Id)

                oTopicName = COL_Forum_threads.EstraiSubject(oForumPost.ThreadID)

                oServiceNotification.NotifyPostModeratedAccepted(Me.PageUtility.WorkingCommunityID, oForum.Id, oForum.Name, oForumPost.ThreadID, oTopicName, POST_Id, oForumPost.Subject, oForumPost.PRSN_Id)



				oForumPost.ThreadID = Session("IdThread")

				Dim oUtility As New OLDpageUtility(Me.Context)
                Dim UrlBase As String = oUtility.ApplicationUrlBase & "Forum/LoadForum.aspx?CommunityID=" & PageUtility.WorkingCommunityID.ToString  ' & oUtility.SystemSettings.Presenter.DefaultForumLogin
				Dim UrlBaseNotifica As String = oUtility.ApplicationUrlBase & oUtility.SystemSettings.Presenter.DefaultRemoveServiceNotification
				oForumPost.NotificaViaMail(UrlBaseNotifica, UrlBase, oPersona, oUtility.LocalizedMail, Main.RuoloForumStandard.Ospite)
				oForumPost.NotificaViaMail(UrlBaseNotifica, UrlBase, oPersona, oUtility.LocalizedMail, Main.RuoloForumStandard.Partecipante)
				Me.Bind_Post()
			ElseIf e.CommandName = "Vota" Then
				'Dim PostID, posizione As Integer
				'Dim oForumPost As COL_Forum_posts
				Dim oDropDownlist As HtmlSelect

				'PostID = sender.CommandArgument
				'posizione = sender.CommandName

				oDropDownlist = e.Item.FindControl("SL_voto_") 'Me.DGpost.Items(posizione).FindControl("SL_voto_") ' & ForumID)
				If IsNothing(oDropDownlist) = False Then
					Try
						Dim voto As Integer = oDropDownlist.Items(oDropDownlist.SelectedIndex).Value
						oForumPost.Vota(POST_Id, Session("objPersona").id, voto)
					Catch ex As Exception

					End Try
					Me.Bind_Post()
				End If
			End If
		Catch ex As Exception
			Me.Bind_Post()
		End Try
	End Sub

#End Region

#Region "Pannello Inserimento Messaggio"
	Private Sub LNBannullaInserimento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaInserimento.Click
		Session("Azione") = "loaded"
		Me.PNLcontenuto.Visible = True
		Me.PNLnewMessage.Visible = False
		Me.TBRmovimento0.Visible = True
		Me.Bind_Post()
		Me.PNLmenu.Visible = True
		Me.PNLmenuInserimento.Visible = False
	End Sub

	Private Sub LNBrispondi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBrispondi.Click
		Try

			If Session("Azione") = "inserisci" Or Session("Azione") = "modifica" Then
				Dim risultato As Boolean
				risultato = Me.CTRLaggiungiPost.Rispondi()
				If risultato = False Then
					If Me.CTRLaggiungiPost.Azione = UC_AggiungiPost.Action.Modifica Then
						Response.Write("<script language=Javascript>" & "alert('" & Replace(Me.oResource.getValue("modifica.False"), "'", "\'") & "');" & "</script>")
					Else
						Response.Write("<script language=Javascript>" & "alert('" & Replace(Me.oResource.getValue("inserimento.False"), "'", "\'") & "');" & "</script>")
					End If
                Else
                    If Me.CTRLaggiungiPost.Azione = UC_AggiungiPost.Action.Modifica Then
                        Me.PageUtility.AddAction(ActionType.EditPost, Me.PageUtility.CreateObjectsList(ObjectType.Post, 0), InteractionType.UserWithUser)
                    Else
                        Me.PageUtility.AddAction(ActionType.CreatePost, Me.PageUtility.CreateObjectsList(ObjectType.Post, 0), InteractionType.UserWithUser)
                    End If


				End If
			End If
		Catch ex As Exception

		End Try
		Session("Azione") = "loaded"
		Me.PNLcontenuto.Visible = True
		Me.PNLnewMessage.Visible = False
		Me.TBRmovimento0.Visible = True
		Me.Bind_Post()
		Me.PNLmenu.Visible = True
		Me.PNLmenuInserimento.Visible = False
	End Sub

#End Region



	Public Sub IMBcancellaPost_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBcancellaPost.Click
		Dim oRuoloForum As New Main.RuoloForumStandard
		oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)
		Try


			If oRuoloForum = Main.RuoloForumStandard.Ospite Or oRuoloForum = Main.RuoloForumStandard.Partecipante Then

			Else
				Dim oPost As New COL_Forum_posts
				oPost.Id = sender.CommandArgument
				oPost.Elimina()
			End If
		Catch ex As Exception

		End Try
		Me.Bind_Post()
	End Sub
	Private Sub CHBnotificaMail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CHBnotificaMail.CheckedChanged
		Try
			Dim oThread As New COL_Forum_threads
			Dim oPersona As New COL_Persona

			oThread.Id = Session("IdThread")
			oPersona = Session("objPersona")

			If Me.CHBnotificaMail.Checked = True Then
				oThread.AggiungiNotifica(oPersona.ID)
			Else
				oThread.RimuoviNotifica(oPersona.ID)
			End If

		Catch ex As Exception

		End Try
	End Sub


	Protected Sub LNBforum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBforum.Click
		Dim oSelezionato As Main.FiltroArchiviazione

		If Session("ForumIsArchiviato") = True Then
			oSelezionato = Main.FiltroArchiviazione.Archiviato
		Else
			oSelezionato = Main.FiltroArchiviazione.NonArchiviato
		End If
		Session("IdThread") = ""
		Session("NomeThread") = ""
		Session("ThreadArchiviato") = False

		Session("IdForum") = ""
		Session("ForumIsArchiviato") = False
		Session("NomeForum") = ""
		Session("RuoloForum") = ""

		Me.PageUtility.RedirectToUrl("Forum/forums.aspx?sel=" & CInt(oSelezionato))
	End Sub
	Protected Sub LNBtopics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBtopics.Click
		Dim oSelezionato As Main.FiltroArchiviazione

		If Session("ThreadArchiviato") = True And Session("ForumIsArchiviato") = False Then
			oSelezionato = Main.FiltroArchiviazione.Archiviato
		Else
			oSelezionato = Main.FiltroArchiviazione.NonArchiviato
		End If
		Session("IdThread") = ""
		Session("NomeThread") = ""
		Session("ThreadArchiviato") = False


		'Me.PageUtility.AddAction(ActionType.Show, CreateObjectsList(ObjectType.Forum, Session("IdForum")), InteractionType.UserWithUser)
		Me.PageUtility.RedirectToUrl("Forum/ForumThreads.aspx?sel=" & CInt(oSelezionato))
	End Sub
	Protected Sub DDLforum_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLforum.SelectedIndexChanged
		Dim oForum As New COL_Forums

		Try
			Session("IdThread") = ""
			Session("NomeThread") = ""
			Session("ThreadArchiviato") = False

			If Me.DDLforum.SelectedValue > 0 Then
				oForum.Id = Me.DDLforum.SelectedValue
				oForum.Estrai()

				If oForum.Errore = Errori_Db.None Then
					Session("IdForum") = oForum.Id
					Session("ForumIsArchiviato") = oForum.isArchiviato
					Session("NomeForum") = oForum.Name


					Dim CMNT_ID, RLPC_id As Integer
					If Session("AdminForChange") = False Then
						CMNT_ID = Session("IdComunita")
						RLPC_id = Session("RLPC_ID")
					Else
						Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

						CMNT_ID = Session("idComunita_forAdmin")
						oRuoloPersonaComunita.Estrai(CMNT_ID, Session("objPersona"))
						RLPC_id = oRuoloPersonaComunita.Id
					End If
					Session("RuoloForum") = oForum.getRuoloForIscritto(RLPC_id, True)
					Me.PageUtility.RedirectToUrl("Forum/ForumThreads.aspx")
				End If
			End If
		Catch ex As Exception

		End Try

	End Sub

    Private Sub RBLordinamentoPost_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLordinamentoPost.SelectedIndexChanged
        Me.Bind_Post()
    End Sub

	Private Function CreateObjectsList(ByVal oType As ObjectType, ByVal oValueID As String) As List(Of lm.ActionDataContract.ObjectAction)
		Dim oList As New List(Of lm.ActionDataContract.ObjectAction)
		oList.Add(New lm.ActionDataContract.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = PageUtility.CurrentModule.ID})
		Return oList
	End Function

	Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
		PageUtility.CurrentModule = PageUtility.GetModule(Services_Forum.Codex)
    End Sub


    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            Return ManagerConfiguration.GetSmartTags(Me.PageUtility.ApplicationUrlBase(True))
        End Get
    End Property

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub
End Class

'<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
'<HTML>
'	<head runat="server">
'		<title>Comunità On Line - Forum</title>

'		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
'		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
'		<meta content="JavaScript" name="vs_defaultClientScript"/>
'		 <LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
'		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>



'	</HEAD>
'	<body>
'		 <form id="aspnetForm" runat="server">
'		 <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
'			<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
'			    <tr>
'				    <td colspan="3" >
'				    <div>
'				        <%--<HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>--%>	
'				    </div>
'				    <br style="clear:both;" />
'				    </td>
'			    </tr>
'				<tr class="contenitore">
'					<td colSpan="3">
'						<table cellSpacing="0" cellPadding="0" width="900px" border="0">
'							<tr>
'								<td class="RigaTitolo" align="left">
'									<%--<asp:Label ID="LBTitolo" Runat="server">Forum Topics: elenco post</asp:Label>--%>
'								</td>
'							</tr>
'							<tr>
'								<td >

'								</td>
'							</tr>
'							<tr>
'								<td align="center" colspan=2>

'								</td>
'							</tr>
'						</table>
'					</td>
'				</tr>
'				<tr class="contenitore">
'					<td colSpan="3"></td>
'				</tr>
'			</table>
'			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
'		</form>
'	</body>
'</HTML>
