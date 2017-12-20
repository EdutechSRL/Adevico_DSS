Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Forum
Imports lm.ActionDataContract


Public Class ForumThreads
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

    Private Enum iconaForum
        Censurato = 1
        NoNewPost = 2
        NewPost = 3
        Waiting = 4
    End Enum

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

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

	Protected WithEvents DGthread As System.Web.UI.WebControls.DataGrid
	Protected WithEvents newthread As System.Web.UI.WebControls.Label
	Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
	Protected WithEvents PNLthread As System.Web.UI.WebControls.Panel
	Protected WithEvents IMBcancellaThread As System.Web.UI.WebControls.ImageButton
	'Protected WithEvents CHBnotificaMail As System.Web.UI.WebControls.CheckBox

    Protected WithEvents TBRmovimento1 As HtmlControl
    Protected WithEvents TBRmovimento0 As HtmlControl
	Protected WithEvents TBRlegenda As System.Web.UI.WebControls.TableRow
	Protected WithEvents IMGvota As System.Web.UI.WebControls.ImageButton

#Region "Pannelli"
	Protected WithEvents IMBgoToPost As System.Web.UI.WebControls.ImageButton
	Protected WithEvents LBselezioneForum_t As System.Web.UI.WebControls.Label
	Protected WithEvents RBLselezioneTopic As System.Web.UI.WebControls.RadioButtonList
	Protected WithEvents PNLlegenda As System.Web.UI.WebControls.Panel
	Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel

	Protected WithEvents LBavviso As System.Web.UI.WebControls.Label
#End Region

#Region "Permessi"
	Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
	Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "link"
	Protected WithEvents LBintestazione As System.Web.UI.WebControls.Label
	Protected WithEvents LNBnewThread As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBforum As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBjumpToforum As System.Web.UI.WebControls.Label
	Protected WithEvents DDLforum As System.Web.UI.WebControls.DropDownList


	Protected WithEvents LNBeliminaThread As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBArchivia As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBcensura As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBnotifica As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBsegnala As System.Web.UI.WebControls.LinkButton

	Protected WithEvents LNBforum_bottom As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBjumpToforum_bottom As System.Web.UI.WebControls.Label
	Protected WithEvents DDLforum_bottom As System.Web.UI.WebControls.DropDownList
#End Region

#Region "Labels"
	Protected WithEvents LBthreads_t As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBauthor_t As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBreplies_t As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBviews_t As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBlastPost_t As System.Web.UI.WebControls.LinkButton

	Protected WithEvents LBlogin As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBnReply As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBnView As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBlastPostGiorno As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LBlastPostOra As System.Web.UI.WebControls.LinkButton

#End Region

	Protected WithEvents IMbusta As System.Web.UI.WebControls.Image
	Protected WithEvents LBicona1 As System.Web.UI.WebControls.Label
	Protected WithEvents LBicona2 As System.Web.UI.WebControls.Label
	Protected WithEvents LBicona3 As System.Web.UI.WebControls.Label

	Protected WithEvents PNLstatistiche As System.Web.UI.WebControls.Panel
	Protected WithEvents RPTAccessiForum As System.Web.UI.WebControls.Repeater
	Protected WithEvents LBLStatistiche As System.Web.UI.WebControls.Label

#Region "Pannello Inserisci Thread"
	Protected WithEvents PNLnewThread As System.Web.UI.WebControls.Panel
	Protected WithEvents CTRLaggiungiThread As Comunita_OnLine.UC_AggiungiThread

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
		If IsNothing(oResource) Then
			Me.SetCulture(Session("LinguaCode"))
		End If


		If Me.SessioneScaduta() Then
			Exit Sub
		End If

		If Not Page.IsPostBack Then
			Session("IdThread") = ""
			Session("NomeThread") = ""
			Session("ThreadIsArchiviato") = False
			Me.SetupInternazionalizzazione()
			If Session("RuoloForum") Is "" Or Session("RuoloForum") = "0" Then
				Me.ViewState("RuoloForum") = "0"
			Else
				Me.ViewState("RuoloForum") = Session("RuoloForum")
			End If
			Me.ViewState("ForumIsArchiviato") = Session("ForumIsArchiviato")
			Me.ViewState("IdForum") = Session("IdForum")
		End If

		If Me.ViewState("RuoloForum") Is "" Or Me.ViewState("RuoloForum") = "0" Then
			PageUtility.RedirectToUrl("Forum/forums.aspx")
		Else
			Dim oServizio As New UCServices.Services_Forum
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try

			If oServizio.AccessoForum Or oServizio.GestioneForum Then
				Me.PNLpermessi.Visible = False
				Me.PNLcontenuto.Visible = True
				Try
					If Me.Page.IsPostBack = False Then
						Session("Azione") = "load"
						Me.Bind_Dati()
						Me.PageUtility.AddAction(ActionType.TopicList, Me.PageUtility.CreateObjectsList(ObjectType.Forum, Session("IdForum")), InteractionType.UserWithUser)
					End If
				Catch ex As Exception
					PageUtility.RedirectToUrl("Forum/forums.aspx")
				End Try
			Else
				Me.PNLcontenuto.Visible = False
				Me.PNLpermessi.Visible = True
			End If
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
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & Me.PageUtility.GetDefaultLogoutPage & "');</script>")
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
		PageUtility.RedirectToUrl("Comunita/EntrataComunita.aspx")
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
		oResource.ResourcesName = "pg_ForumThreads"
		oResource.Folder_Level1 = "Forum"
		oResource.setCulture()

	End Sub

	Private Sub SetupInternazionalizzazione()
		With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")

			.setLabel(Me.LBNopermessi)

			.setLabel(Me.LBavviso)

			Dim i As Integer
			For i = 1 To 4
				Dim oLabel As Label
                'oLabel = Me.Page.FindControl("LBicona" & i)
                oLabel = FindControlRecursive(Me.Master, "LBicona" & i)

				If IsNothing(oLabel) = False Then
					Me.oResource.setLabel_To_Value(oLabel, "iconaForum." & i)
				End If
			Next
			.setRadioButtonList(RBLselezioneTopic, True)
			.setRadioButtonList(Me.RBLselezioneTopic, False)
			.setLabel(LBselezioneForum_t)
			.setLabel(LBjumpToforum)
			.setLinkButton(Me.LNBforum, True, True)
			.setLinkButton(Me.LNBforum_bottom, True, True)
			Me.LBjumpToforum_bottom.Text = Me.LBjumpToforum.Text

			.setLinkButton(Me.LNBnewThread, True, True)
			.setLinkButton(Me.LNBannullaInserimento, True, True)
			.setLinkButton(Me.LNBrispondi, True, True)

			.setLabel(LBLStatistiche)
		End With
	End Sub
#End Region

#Region "Bind_Dati"
	Private Sub Bind_Dati()
		Dim totaleArchiviati, totaleAttivi As Integer
		Dim oForumThread As New COL_Forum_threads
		Dim oRuoloForum As New Main.RuoloForumStandard
		Dim ForumID, RuoloForum As Integer

		'oForumThread.Id = Session("IdThread")
		Try
			RuoloForum = Me.ViewState("RuoloForum")
		Catch ex As Exception

		End Try
		Try
			ForumID = Me.ViewState("IdForum")
		Catch ex As Exception
			Response.Redirect("./forums.aspx", True)
			Exit Sub
		End Try

		If Me.ViewState("ForumIsArchiviato") = True Then
			Me.PNLmenu.Visible = False
			Me.Bind_Topics(Main.FiltroArchiviazione.Tutti)
			Me.Bind_ForumList(Main.FiltroArchiviazione.Tutti)
			Me.LNBnewThread.Enabled = False
		Else
			Dim oServizio As New UCServices.Services_Forum
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try

			If oServizio.GestioneForum Or RuoloForum <> Main.RuoloForumStandard.Ospite Then
                Me.LNBnewThread.Visible = True
			Else
                Me.LNBnewThread.Visible = False
			End If

			If oServizio.GestioneForum Then
				oForumThread.HasThreadAssociatiForUtente(ForumID, totaleArchiviati, totaleAttivi, Main.FiltroVisibilità.Tutti)
			Else
				oForumThread.HasThreadAssociatiForUtente(ForumID, totaleArchiviati, totaleAttivi, Main.FiltroVisibilità.Tutti)
			End If

			If totaleArchiviati > 0 And totaleAttivi > 0 Then
				Me.RBLselezioneTopic.Enabled = True
				If Request.QueryString("sel") = "1" Then
					Me.RBLselezioneTopic.SelectedIndex = 1
				Else
					Me.RBLselezioneTopic.SelectedIndex = 0
				End If
				Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
				Me.Bind_ForumList(Main.FiltroArchiviazione.NonArchiviato)
			ElseIf totaleArchiviati > 0 Then
				Me.RBLselezioneTopic.Enabled = False
				Me.RBLselezioneTopic.SelectedIndex = 1
				Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
				Me.Bind_ForumList(Main.FiltroArchiviazione.NonArchiviato)
			ElseIf totaleAttivi > 0 Then
				Me.RBLselezioneTopic.Enabled = False
				Me.RBLselezioneTopic.SelectedIndex = 0
				Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
				Me.Bind_ForumList(Main.FiltroArchiviazione.NonArchiviato)
			Else
				Me.RBLselezioneTopic.Enabled = False
				Me.RBLselezioneTopic.SelectedIndex = 0
				Me.Bind_ForumList(Main.FiltroArchiviazione.NonArchiviato)
				Me.TBRlegenda.Visible = False
				Me.PNLthread.Visible = False
				Me.LBavviso.Visible = True
				Me.TBRmovimento1.Visible = False
				Me.TBRlegenda.Visible = False
			End If
			Me.PNLmenu.Visible = True
		End If

		Me.BindStatistiche(ForumID)

	End Sub

	Private Sub Bind_ForumList(ByVal oArchiviazione As Main.FiltroArchiviazione)
		Dim oDataset As DataSet
		Dim oForum As New COL_Forums


		Me.DDLforum.Items.Clear()
		Me.DDLforum_bottom.Items.Clear()
		Try
			Dim CMNT_ID, RLPC_id As Integer
			oForum.Id = Me.ViewState("IdThread")

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
	Private Sub Bind_Topics(ByVal oArchiviazione As Main.FiltroArchiviazione)
		Dim oForumThread As New COL_Forum_threads
		Dim oRuoloForum As New Main.RuoloForumStandard

		Dim oServizio As New UCServices.Services_Forum
		Try
			Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
			oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
		Catch ex As Exception
			oServizio.PermessiAssociati = "00000000000000000000000000000000"
		End Try

		oRuoloForum = CType(Me.ViewState("RuoloForum"), Main.RuoloForumStandard)


		Dim oDataset As New DataSet
		Try
			Dim i, totale As Integer

			If oServizio.GestioneForum Or oRuoloForum = Main.RuoloForumStandard.Amministratore Or oRuoloForum = Main.RuoloForumStandard.Moderatore Then
				oDataset = oForumThread.ElencaThreadByForum(Session("objPersona").id, Me.ViewState("IdForum"), Main.FiltroVisibilità.Tutti, oArchiviazione, True)
			Else
				oDataset = oForumThread.ElencaThreadByForum(Session("objPersona").id, Me.ViewState("IdForum"), Main.FiltroVisibilità.Tutti, oArchiviazione, False)
			End If

			totale = oDataset.Tables(0).Rows.Count
			If totale > 0 Then
				Me.PNLthread.Visible = True

				oDataset.Tables(0).Columns.Add(New DataColumn("proprieta"))
				oDataset.Tables(0).Columns.Add(New DataColumn("alternative"))
				oDataset.Tables(0).Columns.Add(New DataColumn("DataUltimoPost"))
				oDataset.Tables(0).Columns.Add(New DataColumn("oArchiviazione"))
				oDataset.Tables(0).Columns.Add(New DataColumn("posizione"))
				oDataset.Tables(0).Columns.Add(New DataColumn("isWaiting"))

				Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

				'oRuoloPersonaComunita.Estrai(Session("IdComunita"), Session("RLPC_ID"))
				oRuoloPersonaComunita.Estrai(Session("IdComunita"), Session("objPersona").id)
				For i = 0 To totale - 1
					Dim oRow As DataRow
					Dim showNewPost As Boolean = False
					Dim THRD_LastPost_ID, Real_LastPost_ID As Integer
					oRow = oDataset.Tables(0).Rows(i)
					oRow.Item("posizione") = i
					oRow.Item("isWaiting") = False

					If Not IsDBNull(oRow.Item("THRD_LastPost_ID")) Then
						THRD_LastPost_ID = oRow.Item("THRD_LastPost_ID")
						If Not IsDBNull(oRow.Item("Real_LastPost_ID")) Then
							Real_LastPost_ID = oRow.Item("Real_LastPost_ID")

							If Real_LastPost_ID > 0 Then
								If Real_LastPost_ID <> THRD_LastPost_ID Then
									' e diverso.....
									Dim oPost As New Forum.COL_Forum_posts
									With oPost
										.Id = Real_LastPost_ID
										.Estrai()
										If .Errore = Errori_Db.None Then
											Dim oPersona As New COL_Persona
											oRow.Item("THRD_LastPost") = .PostDate

											oRow.Item("THRD_LastPost_ID") = Real_LastPost_ID
											oPersona.ID = .PRSN_Id
											oPersona.Estrai(1)
											oRow.Item("AnagraficaLastPost") = oPersona.Cognome & " " & oPersona.Nome
										End If
									End With
								End If
							Else
								oRow.Item("THRD_LastPost_ID") = System.DBNull.Value
								oRow.Item("THRD_LastPost") = System.DBNull.Value
								oRow.Item("AnagraficaLastPost") = ""
							End If
						End If
					End If

					If IsDBNull(oRow.Item("THRD_LastPost")) Then
						showNewPost = False
					Else
						If Not Equals(oRuoloPersonaComunita.PenultimoCollegamento, New Date) Then
							If oRuoloPersonaComunita.PenultimoCollegamento < oRow.Item("THRD_LastPost") Then
								showNewPost = True
							End If
						End If
					End If

					If oRow.Item("THRD_Hide") Then
						oRow.Item("proprieta") = "./../images/forum/priority_post_locked_icon.gif"
						oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & iconaForum.Censurato)
						Try
							Dim oPost As New COL_Forum_posts
							With oPost
								.Id = THRD_LastPost_ID
								.Estrai()
								If .Errore = Errori_Db.None Then
									If .ParentID = 0 And .Approved = Main.PostApprovazione.InAttesa Then
										oRow.Item("isWaiting") = True
										oRow.Item("proprieta") = "./../images/forum/Waiting.gif"
										oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & iconaForum.Waiting)
									End If
								End If
							End With
						Catch ex As Exception

						End Try

					ElseIf showNewPost Then
						oRow.Item("proprieta") = "./../images/forum/new_posts_icon.gif"
						oRow.Item("alternative") = Me.oResource.getValue("forum." & iconaForum.NewPost)
					Else
						oRow.Item("proprieta") = "./../images/forum/no_new_posts_icon.gif"
						oRow.Item("alternative") = Me.oResource.getValue("forum." & iconaForum.NoNewPost)
					End If

					If IsDBNull(oRow.Item("THRD_LastPost")) Then
						oRow.Item("DataUltimoPost") = ""
					Else
                        oRow.Item("DataUltimoPost") = CDate(oRow.Item("THRD_LastPost")).ToString("D", oResource.CultureInfo) & " " & FormatDateTime(oRow.Item("THRD_LastPost"), DateFormat.ShortTime)
					End If
					oRow.Item("oArchiviazione") = CInt(CType(oArchiviazione, Main.FiltroArchiviazione))
				Next
				Me.DGthread.DataSource = oDataset
				Me.DGthread.DataBind()
				If totale > 8 Then
					Me.TBRmovimento1.Visible = True
				Else
					Me.TBRmovimento1.Visible = False
				End If
				Me.LBavviso.Visible = False
				Me.TBRlegenda.Visible = True
			Else
				Me.TBRmovimento1.Visible = False
				Me.TBRlegenda.Visible = False
				Me.PNLthread.Visible = False
				Me.LBavviso.Visible = True
			End If
		Catch ex As Exception
			Me.TBRlegenda.Visible = False
			Me.PNLthread.Visible = False
			Me.LBavviso.Visible = True
			Me.TBRmovimento1.Visible = False
		End Try
	End Sub

	Private Sub Bind_SelezioneOrigine()
		Dim totaleArchiviati, totaleAttivi As Integer
		Dim oForumThread As New COL_Forum_threads

		If Me.ViewState("ForumIsArchiviato") = True Then
			Me.PNLmenu.Visible = False
		Else
			Dim oServizio As New UCServices.Services_Forum
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try

			If oServizio.GestioneForum Then
				oForumThread.HasThreadAssociatiForUtente(Me.ViewState("IdForum"), totaleArchiviati, totaleAttivi, Main.FiltroVisibilità.Tutti)
			Else
				oForumThread.HasThreadAssociatiForUtente(Me.ViewState("IdForum"), totaleArchiviati, totaleAttivi, Main.FiltroVisibilità.Tutti)
			End If

			If totaleArchiviati > 0 And totaleAttivi > 0 Then
				Me.RBLselezioneTopic.Enabled = True
			ElseIf totaleArchiviati > 0 Then
				Me.RBLselezioneTopic.Enabled = True
				Me.RBLselezioneTopic.SelectedIndex = 1
			ElseIf totaleAttivi > 0 Then
				Me.RBLselezioneTopic.Enabled = False
				Me.RBLselezioneTopic.SelectedIndex = 0
			End If
			Me.PNLmenu.Visible = True
		End If

	End Sub
#End Region

#Region "Gestione Griglia Dati"
	Private Sub DGthread_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGthread.ItemDataBound
		If IsNothing(oResource) Then
			Me.SetCulture(Session("LinguaCode"))
		End If
		If e.Item.ItemType = ListItemType.Header Then
			Dim LBauthor_t, LBthreads_t, LBreplies_t, LBviews_t, LBlastPost_t As Label
			LBauthor_t = e.Item.FindControl("LBauthor_t")
			LBthreads_t = e.Item.FindControl("LBthreads_t")
			LBreplies_t = e.Item.FindControl("LBreplies_t")
			LBviews_t = e.Item.FindControl("LBviews_t")
			LBlastPost_t = e.Item.FindControl("LBlastPost_t")

			With oResource
				.setLabel(LBauthor_t)
				.setLabel(LBthreads_t)
				.setLabel(LBreplies_t)
				.setLabel(LBviews_t)
				.setLabel(LBlastPost_t)
			End With
		ElseIf (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
			Dim isForumArchiviato As Boolean = False
			Dim selezione_Archiviato As Main.FiltroArchiviazione
			Dim isCensurato As Boolean = False
			Dim isWaiting As Boolean = False

			isForumArchiviato = Me.ViewState("ForumIsArchiviato")
			Try
				selezione_Archiviato = e.Item.DataItem("oArchiviazione")
			Catch ex As Exception

			End Try
			Try
				isCensurato = (e.Item.DataItem("THRD_Hide") = 1)
			Catch ex As Exception

			End Try
			Try
				isWaiting = e.Item.DataItem("isWaiting")
			Catch ex As Exception

			End Try

			Dim oServizio As New UCServices.Services_Forum
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try


			Dim oPNLmenuTopic As Panel
			oPNLmenuTopic = e.Item.FindControl("PNLmenuTopic")


			Try
				Dim oLBtitoloTopic As Label
				Dim oLNBtitolo As LinkButton
				oLNBtitolo = e.Item.FindControl("LNBtitolo")
				oLBtitoloTopic = e.Item.FindControl("LBtitoloTopic")

				If IsDBNull(e.Item.DataItem("THRD_Subject")) = False Then
					If Not IsNothing(oLBtitoloTopic) Then
						oLBtitoloTopic.Text = e.Item.DataItem("THRD_Subject")
					End If
					If Not IsNothing(oLNBtitolo) Then
						oLNBtitolo.Text = e.Item.DataItem("THRD_Subject")
						oLNBtitolo.Attributes.Add("onmouseover", "status='" & Replace(Me.oResource.getValue("status_Topic"), "'", "\'") & "';return true;")
						oLNBtitolo.Attributes.Add("onmouseout", "status='';return true;")
						oLNBtitolo.Attributes.Add("onclick", "status='" & Replace(Me.oResource.getValue("status_Topic"), "'", "\'") & "';return true;")
						oLNBtitolo.ToolTip = Me.oResource.getValue("status_Topic")
					End If

					If isCensurato And Not isWaiting Then
						If oServizio.GestioneForum Or Me.ViewState("RuoloForum") = Main.RuoloForumStandard.Amministratore Or Session("RuoloForum") = Main.RuoloForumStandard.Moderatore Then
							oLNBtitolo.Visible = True
							oLBtitoloTopic.Visible = False
						Else
							oLNBtitolo.Visible = False
							oLBtitoloTopic.Visible = True
						End If
					Else
						oLNBtitolo.Visible = True
						oLBtitoloTopic.Visible = False
					End If
				End If
			Catch ex As Exception

			End Try

			Dim oRuoloForum As New Main.RuoloForumStandard
			oRuoloForum = CType(Me.ViewState("RuoloForum"), Main.RuoloForumStandard)

			If isForumArchiviato Then
				' se il forum è archiviato non posso fare un tubo !!!
				oPNLmenuTopic.Visible = False
			Else
				oPNLmenuTopic.Visible = True
				' se il forum NON è archiviato possiamo fare solo determinate cose

				Dim oLBinit, oLBhasCensura, oLBhasArchivia, oLBhasNotifica, oLBhasSegnala, oLBend, oLBinit_1, oLBend_1 As Label
				Dim oLNBArchivia, oLNBcensura, oLNBnotifica, oLNBsegnala, oLNBeliminaThread As LinkButton
                Dim oTBRcomandi0, oTBRcomandi1 As HtmlControl

				oTBRcomandi0 = e.Item.FindControl("TBRcomandi0")
				oTBRcomandi1 = e.Item.FindControl("TBRcomandi1")

				oLBinit = e.Item.FindControl("LBinit")
				oLBhasArchivia = e.Item.FindControl("LBhasArchivia")
				oLNBArchivia = e.Item.FindControl("LNBArchivia")
				oLNBeliminaThread = e.Item.FindControl("LNBeliminaThread")
				oLBhasCensura = e.Item.FindControl("LBhasCensura")
				oLNBcensura = e.Item.FindControl("LNBcensura")
				oLBhasNotifica = e.Item.FindControl("LBhasNotifica")
				oLNBnotifica = e.Item.FindControl("LNBnotifica")
				oLBhasSegnala = e.Item.FindControl("LBhasSegnala")
				oLNBsegnala = e.Item.FindControl("LNBsegnala")
				oLBend = e.Item.FindControl("LBend")
				oLBinit_1 = e.Item.FindControl("LBinit_1")
				oLBend_1 = e.Item.FindControl("LBend_1")


				Dim HasRiga1 As Boolean = False
				Dim HasRiga2 As Boolean = False

				If selezione_Archiviato = Main.FiltroArchiviazione.Archiviato Then
					'Topics Archiviati !
					Me.oResource.setLinkButtonToValue(oLNBArchivia, "Archiviato", True, True)
					oLBhasCensura.Visible = False
					oLNBcensura.Visible = False
					oLBhasArchivia.Visible = False

					If oServizio.GestioneForum Or oRuoloForum = Main.RuoloForumStandard.Amministratore Or oRuoloForum = Main.RuoloForumStandard.Moderatore Then
						Me.oResource.setLinkButton(oLNBeliminaThread, True, True, , True)
						oLNBeliminaThread.Visible = True
						oLBhasArchivia.Visible = True
						HasRiga1 = True
					Else
						oLNBeliminaThread.Visible = False
						oLBhasArchivia.Visible = False
					End If

                    If isCensurato Or isWaiting OrElse oRuoloForum = RuoloForumStandard.Ospite Then
                        oLBhasNotifica.Visible = False
                        oLNBnotifica.Visible = False
                    Else
                        oLBhasNotifica.Visible = True
                        oLNBnotifica.Visible = True
                    End If
				Else

					If oServizio.GestioneForum Or oRuoloForum = Main.RuoloForumStandard.Amministratore Or oRuoloForum = Main.RuoloForumStandard.Moderatore Then
						Me.oResource.setLinkButton(oLNBeliminaThread, True, True, , True)
						Me.oResource.setLinkButtonToValue(oLNBArchivia, "NonArchiviato", True, True)
						oLNBArchivia.Visible = True
						oLBhasArchivia.Visible = True
						oLNBeliminaThread.Visible = True
						HasRiga1 = True
					Else
						oLNBeliminaThread.Visible = False
						oLBhasArchivia.Visible = False
						oLNBArchivia.Visible = False
					End If


					' Gestione Censura
					If oServizio.GestioneForum Or oRuoloForum = Main.RuoloForumStandard.Amministratore Or oRuoloForum = Main.RuoloForumStandard.Moderatore Then
						If e.Item.DataItem("THRD_Hide") = 1 Then
							If isWaiting Then
								Me.oResource.setLinkButtonToValue(oLNBcensura, "isWaiting", True, True)
							Else
								Me.oResource.setLinkButtonToValue(oLNBcensura, "Censurato", True, True)
							End If

						Else
							Me.oResource.setLinkButtonToValue(oLNBcensura, "NonCensurato", True, True)
						End If
						oLBhasCensura.Visible = True
						oLNBcensura.Visible = True
						HasRiga1 = True
					Else
						oLBhasCensura.Visible = False
						oLNBcensura.Visible = False
					End If

					'Gestione Notifica
					Dim oThread As New COL_Forum_threads
					oThread.Id = e.Item.DataItem("THRD_ID")

                    If isCensurato OrElse isWaiting OrElse oRuoloForum = RuoloForumStandard.Ospite Then
                        oLBhasNotifica.Visible = False
                        oLNBnotifica.Visible = False
                    Else
                        If oThread.VerificaNotificaByPersona(Session("objPersona").Id) = False Then
                            Me.oResource.setLinkButtonToValue(oLNBnotifica, "notificaAbilita", True, True)
                        Else
                            Me.oResource.setLinkButtonToValue(oLNBnotifica, "notificaDisabilita", True, True)
                        End If
                        oLBhasNotifica.Visible = True
                        oLNBnotifica.Visible = True
                        HasRiga2 = True
                    End If


					'segnala ad altri
                    If isCensurato OrElse oRuoloForum = RuoloForumStandard.Ospite Then
                        oLBhasSegnala.Visible = False
                        oLNBsegnala.Visible = False
                    Else
                        oLBhasSegnala.Visible = True
                        oLNBsegnala.Visible = True


                        HasRiga2 = True
                    End If
					If oLNBsegnala.Visible = True Then
						Dim i_link As String
						i_link = "./NotificaMessaggio.aspx?ForumId=" & Me.ViewState("IdForum") & "&TopicID=" & e.Item.DataItem("THRD_ID")


						Me.oResource.setLinkButton(oLNBsegnala, True, True)
                        oLNBsegnala.Attributes.Add("onClick", "OpenWin('" & i_link & "','800','550','no','yes');return false;")

					End If
				End If
				oTBRcomandi0.Visible = HasRiga1
				oTBRcomandi1.Visible = HasRiga2

				If HasRiga1 And HasRiga2 Then
					oLBinit_1.Visible = False
					oLBend.Visible = False
				ElseIf HasRiga1 Then
					oLBend.Visible = True
				ElseIf HasRiga2 Then
					oLBinit_1.Visible = True
					oLBend_1.Visible = True
				End If
			End If

			Try
				Dim oIMBgoToPost As ImageButton
				oIMBgoToPost = e.Item.FindControl("IMBgoToPost")
				Me.oResource.setImageButton(oIMBgoToPost, False, True, True)

				If IsDBNull(e.Item.DataItem("THRD_LastPost_ID")) Then
					oIMBgoToPost.Visible = False
				Else
					If oServizio.GestioneForum = False Then
						Try
							If Me.ViewState("RuoloForum") = Main.RuoloForumStandard.Amministratore Or Me.ViewState("RuoloForum") = Main.RuoloForumStandard.Moderatore Then
								oIMBgoToPost.Visible = True
							Else
								oIMBgoToPost.Visible = Not (isCensurato And isWaiting = False)
							End If
						Catch ex As Exception

						End Try
					Else

						oIMBgoToPost.Visible = True
					End If

				End If
			Catch ex As Exception

			End Try


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
			Try
				Dim HasVotato As Boolean = False
				Dim oLBvotoTesto As Label
				Dim oSelect As HtmlControls.HtmlSelect
				Dim oIMGvota As ImageButton

				Try
					HasVotato = e.Item.DataItem("HasVotato")
				Catch ex As Exception

				End Try
				oIMGvota = e.Item.FindControl("IMGvota")
				oLBvotoTesto = e.Item.FindControl("LBvotoTesto")
				oSelect = e.Item.FindControl("SL_voto_")

                If isForumArchiviato OrElse selezione_Archiviato = Main.FiltroArchiviazione.Archiviato OrElse oRuoloForum = RuoloForumStandard.Ospite Then
                    'nessun voto possibile
                    oLBvotoTesto.Visible = False
                    oIMGvota.Visible = False
                    oSelect.Visible = False
                ElseIf isCensurato Then
                    oLBvotoTesto.Visible = False
                    oIMGvota.Visible = False
                    oSelect.Visible = False
                Else
                    'If oServizio.GestioneForum Or oRuoloForum = Main.RuoloForumStandard.Amministratore Or oRuoloForum = Main.RuoloForumStandard.Moderatore Then
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
                End If

			Catch ex As Exception


			End Try
		End If
	End Sub

	Private Sub DGthread_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGthread.ItemCommand
		If e.CommandName = "normale" Or e.CommandName = "entra" Then
			Dim ThreadID, ThreadNome As String
			Dim isArchiviato As Boolean

			ThreadID = Me.DGthread.Items(e.Item.ItemIndex).Cells(6).Text()
			ThreadNome = Me.DGthread.Items(e.Item.ItemIndex).Cells(5).Text()
			isArchiviato = Me.DGthread.Items(e.Item.ItemIndex).Cells(2).Text()
			Session("IdThread") = ThreadID
			Session("ThreadIsArchiviato") = isArchiviato
			Session("NomeThread") = ThreadNome

			PageUtility.RedirectToUrl("Forum/ElencoPost.aspx")
		ElseIf e.CommandName = "Vota" Then
			Dim TopicID As Integer
			'   Dim oForumTopic As COL_Forum_threads
			Dim oDropDownlist As HtmlSelect

			TopicID = CInt(Me.DGthread.DataKeys.Item(e.Item.ItemIndex))

			oDropDownlist = e.Item.FindControl("SL_voto_") 'Me.DGthread.Items(posizione).FindControl("SL_voto_") ' & ForumID)
			If IsNothing(oDropDownlist) = False Then
				Try
					COL_Forum_threads.Vota(TopicID, Session("objPersona").id, oDropDownlist.Items(oDropDownlist.SelectedIndex).Value)
				Catch ex As Exception

				End Try
				If Me.RBLselezioneTopic.SelectedIndex = 0 Then
					Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
				Else
					Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
				End If
			End If
		End If
	End Sub
#End Region

#Region "Inserimento nuovo Thread"
	Private Sub LNBrispondi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBrispondi.Click
		Try
			If Session("Azione") = "insert" Then
				If Me.CTRLaggiungiThread.Inserisci() = False Then
					Response.Write("<script language=Javascript>" & "alert('" & Replace(Me.oResource.getValue("inserimento.False"), "'", "\'") & "');" & "</script>")
				Else
					' REGISTRAZIONE EVENTO
                    Me.PageUtility.AddAction(ActionType.CreateTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, 0), InteractionType.UserWithUser)

				End If
				Session("Azione") = "loaded"
			End If

		Catch ex As Exception

		End Try
		Me.PNLnewThread.Visible = False
		Me.PNLthread.Visible = True
		Me.TBRmovimento0.Visible = True
		Me.Bind_Dati()
		Me.PNLmenu.Visible = True
		Me.PNLmenuInserimento.Visible = False
	End Sub

	Private Sub LNBannullaInserimento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaInserimento.Click
		Me.PNLnewThread.Visible = False
		Me.PNLthread.Visible = True
		Session("Azione") = "loaded"
		Me.CTRLaggiungiThread.Cancellaform()
		Me.TBRmovimento0.Visible = True
		Me.Bind_Dati()
		Try
			If Me.DGthread.VirtualItemCount > 8 Then
				Me.TBRmovimento1.Visible = True
			Else
				Me.TBRmovimento1.Visible = False
			End If
		Catch ex As Exception

		End Try
		Try
			If Me.DGthread.Items.Count = 0 Then
				Me.TBRmovimento1.Visible = False
				Me.TBRlegenda.Visible = False
				Me.PNLthread.Visible = False
				Me.LBavviso.Visible = True
			End If
		Catch ex As Exception

		End Try
		Me.PNLmenu.Visible = True
		Me.PNLmenuInserimento.Visible = False
	End Sub

#End Region

	Private Sub RBLselezioneTopic_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLselezioneTopic.SelectedIndexChanged
		If Me.ViewState("ForumIsArchiviato") = True Then
			Me.Bind_ForumList(Main.FiltroArchiviazione.Archiviato)
		Else
			Me.Bind_ForumList(Main.FiltroArchiviazione.NonArchiviato)
		End If

		If Me.RBLselezioneTopic.SelectedIndex = 0 Then
			Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
		Else
			Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
		End If

		If Me.ViewState("ForumIsArchiviato") = True Or Me.RBLselezioneTopic.SelectedIndex = 1 Then
			Me.LNBnewThread.Enabled = False
		Else
			Dim oServizio As New UCServices.Services_Forum
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try
			If oServizio.GestioneForum Or Me.ViewState("RuoloForum") <> Main.RuoloForumStandard.Ospite Then
				Me.LNBnewThread.Enabled = True
			Else
				Me.LNBnewThread.Enabled = False
			End If
		End If
	End Sub

	Protected Sub IMBgoToPost_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBgoToPost.Click
		Try
			Dim oThread As New COL_Forum_threads
			oThread.Id = sender.CommandName
			Session("IdThread") = sender.CommandName
			Session("NomeThread") = oThread.EstraiSubject()
			Session("ThreadIsArchiviato") = oThread.isArchiviato()

			PageUtility.RedirectToUrl("Forum/ElencoPost.aspx?#post_" & sender.CommandArgument)
		Catch ex As Exception

		End Try
	End Sub

	Protected Sub LNBArchivia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBArchivia.Click
		Try
			Dim oThread As New COL_Forum_threads
			oThread.Id = sender.CommandArgument
			oThread.ModificaArchiviazione()

			Me.Bind_SelezioneOrigine()
			If Me.RBLselezioneTopic.SelectedIndex = 0 Then
				Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
			Else
				Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
			End If
		Catch ex As Exception

		End Try
	End Sub

	Protected Sub LNBcensura_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcensura.Click
		Try
			Dim THRD_Id As Integer = sender.CommandArgument
			Dim isVisibile As Boolean = (sender.CommandName = 0)
			If THRD_Id > 0 Then
				Dim oThread As New COL_Forum_threads

				oThread.Id = THRD_Id
				If isVisibile Then
					oThread.Nascondi()
				Else
					oThread.Mostra()
				End If
				If Me.RBLselezioneTopic.SelectedIndex = 0 Then
					Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
				Else
					Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
				End If
			End If
		Catch ex As Exception

		End Try
	End Sub
	Protected Sub LNBnotifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnotifica.Click
		Try
			Dim oThread As New COL_Forum_threads
			Dim oPersona As New COL_Persona
			oThread.Id = sender.CommandArgument
			oPersona = Session("objPersona")

			oThread.ModificaNotifica(oPersona.ID)
			If Me.RBLselezioneTopic.SelectedIndex = 0 Then
				Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
			Else
				Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
			End If
		Catch ex As Exception

		End Try
	End Sub

	Protected Sub LNBeliminaThread_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBeliminaThread.Click
		Try
			Dim oRuoloForum As New Main.RuoloForumStandard
			Dim oServizio As New UCServices.Services_Forum
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try


			oRuoloForum = CType(Me.ViewState("RuoloForum"), Main.RuoloForumStandard)

			If oServizio.GestioneForum And Not (oRuoloForum = Main.RuoloForumStandard.Ospite Or oRuoloForum = Main.RuoloForumStandard.Partecipante) Then
				Dim oThread As New COL_Forum_threads
				oThread.Id = sender.CommandArgument
				oThread.Elimina()
			End If
			If Me.RBLselezioneTopic.SelectedIndex = 0 Then
				Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
			Else
				Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
			End If
		Catch ex As Exception

		End Try
	End Sub

	Private Sub LNBnewThread_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnewThread.Click
		Dim oRuoloForum As Main.RuoloForumStandard
		Try
			oRuoloForum = CType(Me.ViewState("RuoloForum"), Main.RuoloForumStandard)
		Catch ex As Exception
			oRuoloForum = Main.RuoloForumStandard.Ospite
		End Try
		If oRuoloForum <> Main.RuoloForumStandard.Ospite Then
			Session("Azione") = "insert"
			Me.PNLnewThread.Visible = True
			Me.PNLthread.Visible = False
			Me.TBRlegenda.Visible = False
			Me.TBRmovimento0.Visible = False
			Me.TBRmovimento1.Visible = False
			Me.LBavviso.Visible = False
			Me.PNLmenu.Visible = False
			Me.PNLmenuInserimento.Visible = True
		End If
	End Sub

	Protected Sub LNBforum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBforum.Click
		Dim oSelezionato As Main.FiltroArchiviazione

		If Me.ViewState("ForumIsArchiviato") = True Then
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


		PageUtility.RedirectToUrl("Forum/forums.aspx?sel=" & CInt(oSelezionato))
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
					PageUtility.RedirectToUrl("Forum/ForumThreads.aspx")
				End If
			End If
		Catch ex As Exception

		End Try

	End Sub


	'Protected Sub IMGvota_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMGvota.Click
	'    If sender.CommandArgument <> "" Then
	'        Dim TopicID, posizione As Integer
	'        '   Dim oForumTopic As COL_Forum_threads
	'        Dim oDropDownlist As HtmlSelect

	'        TopicID = sender.CommandArgument
	'        posizione = sender.CommandName


	'        oDropDownlist = Me.DGthread.Items(posizione).FindControl("SL_voto_") ' & ForumID)
	'        If IsNothing(oDropDownlist) = False Then
	'            Try
	'                COL_Forum_threads.Vota(TopicID, Session("objPersona").id, oDropDownlist.Items(oDropDownlist.SelectedIndex).Value)
	'            Catch ex As Exception

	'            End Try
	'            If Me.RBLselezioneTopic.SelectedIndex = 0 Then
	'                Me.Bind_Topics(Main.FiltroArchiviazione.NonArchiviato)
	'            Else
	'                Me.Bind_Topics(Main.FiltroArchiviazione.Archiviato)
	'            End If
	'        End If
	'    End If
	'End Sub
	''<%# DataBinder.Eval(Container.DataItem, "posizione") %>  
#Region "Bind Statistiche"
	Private Sub BindStatistiche(ByVal IdForum As Integer)
		Try
			Dim oTable As New DataSet
			Dim oForum As New Forum.COL_Forums

			oTable = oForum.StatistichePostForum(IdForum)
			If oTable.Tables(0).Rows.Count > 0 Then
				Me.RPTAccessiForum.DataSource = oTable.Tables(0).DefaultView
				Me.RPTAccessiForum.DataBind()
				Me.PNLstatistiche.Visible = True
			Else
				Me.PNLstatistiche.Visible = False
			End If
		Catch ex As Exception
			Me.PNLstatistiche.Visible = False
		End Try

	End Sub
#End Region

	Private Function CreateObjectsList(ByVal oType As ObjectType, ByVal oValueID As String) As List(Of lm.ActionDataContract.ObjectAction)
		Dim oList As New List(Of lm.ActionDataContract.ObjectAction)
		oList.Add(New lm.ActionDataContract.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = PageUtility.CurrentModule.ID})
		Return oList
	End Function

	Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
		PageUtility.CurrentModule = PageUtility.GetModule(Services_Forum.Codex)
    End Sub


    Private Function FindControlRecursive(ByVal Root As Control, ByVal Id As String) As Control
        If Root.ID = Id Then
            Return Root
        End If

        For Each Ctl As Control In Root.Controls
            Dim FoundCtl As Control = FindControlRecursive(Ctl, Id)
            If FoundCtl IsNot Nothing Then
                Return FoundCtl
            End If
        Next
        Return Nothing
    End Function

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

'		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
'	</head>
'	<body>
'		 <form id="aspnetForm" runat="server">
'<%--		 <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>--%>
'			<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
'			    <tr>
'				    <td colspan="3" >
'				    <div>
'				        <%--<HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>	--%>
'				    </div>
'				    <br style="clear:both;" />
'				    </td>
'			    </tr>
'				<tr class="contenitore">
'					<td colspan="3">
'						<table cellspacing="0" cellpadding="0" width="900px" border="0">
'							<tr>
'								<td class="RigaTitolo" align="left">
'									<asp:Label ID="LBTitolo" Runat="server">Forum Topics</asp:Label>
'								</td>
'							</tr>
'							<tr>
'								<td align="right">

'								</td>
'							</tr>
'							<tr>
'								<td align="center">

'								</td>
'							</tr>
'						</table>
'					</td>
'				</tr>
'			</table>
'			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
'		</form>
'	</body>
'</HTML>
