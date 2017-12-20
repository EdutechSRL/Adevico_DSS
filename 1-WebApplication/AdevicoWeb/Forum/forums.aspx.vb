Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Forum
Imports lm.ActionDataContract


Public Class forums
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

    Private Enum iconaForum
        nonAbilitato = 1
        moderatoNoNewPost = 2
        moderatoNewPost = 3
        liberoNoNewPost = 4
        liberoNewPost = 5
    End Enum


#Region "Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

#Region "Contenuto"
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents DGforum As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBavviso As System.Web.UI.WebControls.Label
    Protected WithEvents LNBnotifica As System.Web.UI.WebControls.LinkButton
    Protected WithEvents IMBgoToPost As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LBselezioneForum_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLselezioneForum As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents PNLlegenda As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBArchivia As System.Web.UI.WebControls.LinkButton
    Protected WithEvents IMGvota As System.Web.UI.WebControls.ImageButton
#End Region

    Protected WithEvents LBicona1 As System.Web.UI.WebControls.Label
    Protected WithEvents LBicona2 As System.Web.UI.WebControls.Label
    Protected WithEvents LBicona3 As System.Web.UI.WebControls.Label
    Protected WithEvents LBicona4 As System.Web.UI.WebControls.Label
    Protected WithEvents LBicona5 As System.Web.UI.WebControls.Label

    Protected WithEvents PNLstatistiche As System.Web.UI.WebControls.Panel
    Protected WithEvents RPTAccessiForum As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBLStatistiche As System.Web.UI.WebControls.Label

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    '  Protected WithEvents LKBcrea As System.Web.UI.WebControls.LinkButton
    ' Protected WithEvents LBtitoletto As System.Web.UI.WebControls.Label
    ' Protected WithEvents TXBnomeForum As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    ' Protected WithEvents TXBDescrizione As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents Requiredfieldvalidator As System.Web.UI.WebControls.RequiredFieldValidator
    ' Protected WithEvents PNLcrea As System.Web.UI.WebControls.Panel
    ' Protected WithEvents BTNcreaforum As System.Web.UI.WebControls.Button
    ' Protected WithEvents BTNannulla As System.Web.UI.WebControls.Button

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
        Dim oPersona As COL_Persona
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
		End If

		If Me.Page.IsPostBack = False Then

			Dim oServizio As New UCServices.Services_Forum
			Try
				Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizio.Codex)
				oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try

			Me.SetupInternazionalizzazione()

			If oServizio.AccessoForum Or oServizio.GestioneForum Then
				Me.PNLpermessi.Visible = False
				Try
					If Me.Page.IsPostBack = False Then
						Me.Bind_Dati()

					End If
				Catch ex As Exception
					Me.PNLcontenuto.Visible = False
					Me.PNLpermessi.Visible = True
					Exit Sub
				End Try
				Me.PNLmenu.Visible = True
				Me.PageUtility.AddAction(ActionType.ForumList, , InteractionType.UserWithLearningObject)
			Else
				Me.PNLmenu.Visible = False
				Me.PNLcontenuto.Visible = False
				Me.PNLpermessi.Visible = True
				Me.PageUtility.AddAction(ActionType.ForumList, , InteractionType.UserWithLearningObject)
				Exit Sub
			End If
		End If
		BindStatistiche()
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
		oResource.ResourcesName = "pg_Forum"
		oResource.Folder_Level1 = "Forum"
		oResource.setCulture()
	End Sub

	Private Sub SetupInternazionalizzazione()
		With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
			.setLabel(Me.LBNopermessi)

			'LBavviso.Text = "Nessun forum presente in questa comunità"
			.setLabel(Me.LBavviso)

			Dim i As Integer
			For i = 1 To 5
				Dim oLabel As Label
                'oLabel = Me.Page.FindControl("LBicona" & i)
                oLabel = FindControlRecursive(Me.Master, "LBicona" & i)

				Me.oResource.setLabel_To_Value(oLabel, "iconaForum." & i)
			Next
			.setRadioButtonList(Me.RBLselezioneForum, True)
			.setRadioButtonList(Me.RBLselezioneForum, False)
			.setLabel(LBselezioneForum_t)

			.setLabel(LBLStatistiche)
		End With
	End Sub
#End Region

#Region "Bind_Dati"
	Private Sub Bind_Dati()
		Dim RLPC_id, CMNT_ID, totaleArchiviati, totaleAttivi As Integer
		Dim oForum As New COL_Forums

		RLPC_id = Session("RLPC_ID")
		CMNT_ID = Session("IdComunita")

		oForum.HasForumAssociatiForUtente(RLPC_id, CMNT_ID, totaleArchiviati, totaleAttivi)

		If totaleArchiviati > 0 And totaleAttivi > 0 Then
			Me.RBLselezioneForum.Enabled = True
			If Request.QueryString("sel") = "1" Then
				Me.RBLselezioneForum.SelectedIndex = 1
			Else
				Me.RBLselezioneForum.SelectedIndex = 0
			End If
		ElseIf totaleArchiviati > 0 Then
			Me.RBLselezioneForum.Enabled = False
			Me.RBLselezioneForum.SelectedIndex = 1
		ElseIf totaleAttivi > 0 Then
			Me.RBLselezioneForum.Enabled = False
			Me.RBLselezioneForum.SelectedIndex = 0
		Else
			Me.RBLselezioneForum.Enabled = False
			Me.RBLselezioneForum.SelectedIndex = 0
		End If
		Me.Bind_Forum()
	End Sub
	Private Sub Bind_Forum()
		Dim oForum As New COL_Forums
		Dim RLPC_id, CMNT_ID, totaleArchiviati, totaleAttivi As Integer
		Dim oDataset As New DataSet

		Try
			Dim i, totale As Integer
			Dim identificativo As String

			RLPC_id = Session("RLPC_ID")
			CMNT_ID = Session("IdComunita")

			Dim oServizio As New UCServices.Services_Forum
			Dim forAdmin As Boolean = False

			Try
				Try
					Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizio.Codex)
					oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
				Catch ex As Exception
					oServizio.PermessiAssociati = "00000000000000000000000000000000"
				End Try
				forAdmin = oServizio.GestioneForum
			Catch ex As Exception

			End Try


			oDataset = oForum.ElencaByComunitaIscritto(Session("objPersona").id, CMNT_ID, RLPC_id, Me.RBLselezioneForum.SelectedValue, forAdmin)
			oDataset.Tables(0).Columns.Add(New DataColumn("IdName"))
			oDataset.Tables(0).Columns.Add(New DataColumn("proprieta"))
			oDataset.Tables(0).Columns.Add(New DataColumn("alternative"))
			oDataset.Tables(0).Columns.Add(New DataColumn("DataUltimoPost"))
			oDataset.Tables(0).Columns.Add(New DataColumn("posizione"))

			totale = oDataset.Tables(0).Rows.Count
			If totale > 0 Then
				Me.PNLlegenda.Visible = True
				Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

				oRuoloPersonaComunita.Estrai(CMNT_ID, Session("objPersona").id)	' RLPC_id)


				For i = 0 To totale - 1
					Dim oRow As DataRow
					Dim showNewPost As Boolean = False

					oRow = oDataset.Tables(0).Rows(i)

                    'ADD on 14/07/2015. SE un forum è in HIDE, manda in eccezione le ultime due righe,
                    'di conseguenza NON MOSTRA alcun forum!!!
                    'Per ottimizzare è stato spostato QUI!
                    If CBool(oRow.Item("FRUM_Hide")) = True Then
                        oDataset.Tables(0).Rows(i).Delete()
                    Else

                        oRow.Item("posizione") = i
                        identificativo = oRow.Item("FRUM_Id") & "§" & oRow.Item("FRUM_Name")
                        oRow.Item("IdName") = identificativo

                        Dim FRUM_LastPost_ID, RealLastPost As Integer
                        If Not IsDBNull(oRow.Item("FRUM_LastPost_ID")) Then
                            FRUM_LastPost_ID = oRow.Item("FRUM_LastPost_ID")
                            If Not IsDBNull(oRow.Item("RealLastPost")) Then
                                RealLastPost = oRow.Item("RealLastPost")

                                If RealLastPost > 0 Then
                                    If RealLastPost <> FRUM_LastPost_ID Then
                                        ' e diverso.....
                                        Dim oPost As New Forum.COL_Forum_posts
                                        With oPost
                                            .Id = RealLastPost
                                            .Estrai()
                                            If .Errore = Errori_Db.None Then
                                                Dim oPersona As New COL_Persona
                                                oRow.Item("FRUM_LastPost") = .PostDate

                                                oRow.Item("FRUM_LastPost_ID") = RealLastPost
                                                oPersona.ID = .PRSN_Id
                                                oPersona.Estrai(1)
                                                oRow.Item("AnagraficaLastPost") = oPersona.Cognome & " " & oPersona.Nome
                                            End If
                                        End With
                                    End If
                                Else
                                    oRow.Item("FRUM_LastPost_ID") = System.DBNull.Value
                                    oRow.Item("FRUM_LastPost") = System.DBNull.Value
                                    oRow.Item("AnagraficaLastPost") = ""
                                End If
                            End If
                        End If




                        If IsDBNull(oRow.Item("FRUM_LastPost")) Then
                            showNewPost = False
                        Else
                            If Not Equals(oRuoloPersonaComunita.PenultimoCollegamento, New Date) Then
                                If oRuoloPersonaComunita.PenultimoCollegamento < oRow.Item("FRUM_LastPost") Then
                                    showNewPost = True
                                End If
                            End If
                        End If


                        If oRow.Item("Iscritto") Then
                            If oRow.Item("Abilitato") Then
                                If CBool(oRow.Item("FRUM_Moderated")) = True And showNewPost Then
                                    oRow.Item("proprieta") = "./../images/forum/locked_new_posts_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & Me.iconaForum.moderatoNewPost)
                                ElseIf CBool(oRow.Item("FRUM_Moderated")) = True Then
                                    oRow.Item("proprieta") = "./../images/forum/closed_topic_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & Me.iconaForum.moderatoNoNewPost)
                                ElseIf showNewPost Then
                                    oRow.Item("proprieta") = "./../images/forum/new_posts_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("forum." & Me.iconaForum.liberoNewPost)
                                Else
                                    oRow.Item("proprieta") = "./../images/forum/no_new_posts_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("forum." & Me.iconaForum.liberoNoNewPost)
                                End If

                            Else
                                oRow.Item("proprieta") = "./../images/forum/forum_no_entry_icon.gif"
                                oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & Me.iconaForum.nonAbilitato)
                            End If
                        Else
                            If CBool(oRow.Item("FRUM_Moderated")) = True Then
                                If oRow.Item("FRUM_NPost") > 0 Then
                                    oRow.Item("proprieta") = "./../images/forum/locked_new_posts_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & Me.iconaForum.moderatoNewPost)
                                Else
                                    oRow.Item("proprieta") = "./../images/forum/closed_topic_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & Me.iconaForum.moderatoNoNewPost)
                                End If

                            Else
                                If oRow.Item("FRUM_NPost") > 0 Then
                                    oRow.Item("proprieta") = "./../images/forum/new_posts_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("iconaForum." & Me.iconaForum.liberoNewPost)
                                Else
                                    oRow.Item("proprieta") = "./../images/forum/no_new_posts_icon.gif"
                                    oRow.Item("alternative") = Me.oResource.getValue("forum." & Me.iconaForum.liberoNoNewPost)
                                End If
                            End If
                        End If




                        If IsDBNull(oRow.Item("FRUM_LastPost")) Then
                            oRow.Item("DataUltimoPost") = ""
                        Else
                            oRow.Item("DataUltimoPost") = CDate(oRow.Item("FRUM_LastPost")).ToString("D", oResource.CultureInfo) & " " & FormatDateTime(oRow.Item("FRUM_LastPost"), DateFormat.ShortTime)
                        End If
                    End If
                Next

				Me.DGforum.DataSource = oDataset
				DGforum.DataBind()
				If DGforum.Items.Count = "0" Then
					Me.LBavviso.Visible = True
					Me.DGforum.Visible = False
					Me.PNLlegenda.Visible = False
				Else
					Me.LBavviso.Visible = False
					Me.DGforum.Visible = True
				End If

			Else
				Me.PNLlegenda.Visible = False
				Me.LBavviso.Visible = True
				Me.DGforum.Visible = False
			End If
		Catch ex As Exception
			Me.PNLlegenda.Visible = False
			Me.LBavviso.Visible = True
			Me.DGforum.Visible = False
		End Try
	End Sub

	Private Sub Bind_Selezione()
		Dim RLPC_id, CMNT_ID, totaleArchiviati, totaleAttivi As Integer
		Dim oForum As New COL_Forums

		RLPC_id = Session("RLPC_ID")
		CMNT_ID = Session("IdComunita")

		oForum.HasForumAssociatiForUtente(RLPC_id, CMNT_ID, totaleArchiviati, totaleAttivi)

		If totaleArchiviati > 0 And totaleAttivi > 0 Then
			Me.RBLselezioneForum.Enabled = True
		ElseIf totaleArchiviati > 0 Then
			Me.RBLselezioneForum.Enabled = True
			Me.RBLselezioneForum.SelectedIndex = 1
		ElseIf totaleAttivi > 0 Then
			Me.RBLselezioneForum.Enabled = False
			Me.RBLselezioneForum.SelectedIndex = 0
		End If
	End Sub
#End Region

#Region "Gestione DataList"

	Private Sub DGforum_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGforum.ItemDataBound
		If IsNothing(oResource) Then
			Me.SetCulture(Session("LinguaCode"))
		End If

		If e.Item.ItemType = ListItemType.Header Then
			Dim LBforum_t, LBthreads_t, LBposts_t, LBlastPost_t As Label
			LBforum_t = e.Item.FindControl("LBforum_t")
			LBthreads_t = e.Item.FindControl("LBthreads_t")
			LBposts_t = e.Item.FindControl("LBposts_t")
			LBlastPost_t = e.Item.FindControl("LBlastPost_t")

			With oResource
				.setLabel(LBforum_t)
				.setLabel(LBthreads_t)
				.setLabel(LBposts_t)
				.setLabel(LBlastPost_t)
			End With

		ElseIf e.Item.ItemType = ListItemType.Footer Then

		ElseIf (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
			' Entrata o iscrizione ?
			Dim isIscritto As Boolean = False
			Dim isAbilitato As Boolean = True

			Try
				isIscritto = e.Item.DataItem("Iscritto")
			Catch ex As Exception

			End Try
			Try
				If isIscritto Then
					isAbilitato = e.Item.DataItem("Abilitato")
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
						oLBvoto_t.Visible = True
					Else
						oLBvoto_t.Visible = False
					End If

				End If
			Catch ex As Exception

            End Try

            Dim oServizio As New UCServices.Services_Forum
            Try
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizio.Codex)
                oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Catch ex As Exception
                oServizio.PermessiAssociati = "00000000000000000000000000000000"
            End Try
            Dim oRuoloForum As Main.RuoloForumStandard
            Try
                oRuoloForum = CType(e.Item.DataItem("RuoloForum"), Main.RuoloForumStandard)
            Catch ex As Exception
                oRuoloForum = Main.RuoloForumStandard.Ospite

            End Try
			Try
				Dim oLBtitolo, oLBdescrizione As Label
				oLBtitolo = e.Item.FindControl("LBtitoloForum")
				If Not IsNothing(oLBtitolo) Then
					Dim NomeForum As String = e.Item.DataItem("FRUM_Name")
					oLBtitolo.Text = NomeForum
				End If

				Try
					oLBdescrizione = e.Item.FindControl("LBdescrizione")
					If Not IsNothing(oLBdescrizione) Then
						Dim descrizione As String = e.Item.DataItem("FRUM_Description")
						oLBdescrizione.Text = descrizione
					End If
				Catch ex As Exception

				End Try


				Dim oLNBtitolo As LinkButton
				oLNBtitolo = e.Item.FindControl("LNBtitolo")

				oLNBtitolo.Text = e.Item.DataItem("FRUM_Name")

				If isIscritto And isAbilitato Then
					If Not IsNothing(oLNBtitolo) Then
						oLBtitolo.Visible = False
						oLNBtitolo.Visible = True

						oLNBtitolo.Attributes.Add("onmouseover", "status='" & Replace(Me.oResource.getValue("status_Forum"), "'", "\'") & "';return true;")
						oLNBtitolo.Attributes.Add("onmouseout", "status='';return true;")
						oLNBtitolo.Attributes.Add("onclick", "status='" & Replace(Me.oResource.getValue("status_Forum"), "'", "\'") & "';return true;")
						oLNBtitolo.ToolTip = Me.oResource.getValue("status_Forum")
					End If
				ElseIf isAbilitato Then
					oLNBtitolo.Attributes.Add("onmouseover", "status='" & Replace(Me.oResource.getValue("status_ForumIscrivi"), "'", "\'") & "';return true;")
					oLNBtitolo.Attributes.Add("onmouseout", "status='';return true;")
					oLNBtitolo.Attributes.Add("onclick", "status='" & Replace(Me.oResource.getValue("status_ForumIscrivi"), "'", "\'") & "';return true;")
					oLNBtitolo.ToolTip = Me.oResource.getValue("status_ForumIscrivi")
					oLBtitolo.Visible = False
					oLNBtitolo.Visible = True
				Else
					If Not IsNothing(oLNBtitolo) Then
						oLBtitolo.Visible = True
						oLNBtitolo.Visible = False
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

                If Not isIscritto OrElse oRuoloForum = RuoloForumStandard.Ospite Then
                    oLBvotoTesto.Visible = False
                    oIMGvota.Visible = False
                    oSelect.Visible = False
                ElseIf isIscritto And isAbilitato Then
                    If e.Item.DataItem("FRUM_Archiviato") = 0 Then
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

                Else
                    oLBvotoTesto.Visible = False
                    oIMGvota.Visible = False
                    oSelect.Visible = False
                End If

			Catch ex As Exception


			End Try
			Try
				Dim oPersona As New COL_Persona
				Dim oLKBnotifica As LinkButton

				oLKBnotifica = e.Item.FindControl("LNBnotifica")
                If Not IsNothing(oLKBnotifica) AndAlso isIscritto AndAlso oRuoloForum <> RuoloForumStandard.Ospite Then
                    If isAbilitato And Me.RBLselezioneForum.SelectedValue = False Then
                        Dim oForum As New COL_Forums
                        oForum.Id = e.Item.DataItem("FRUM_id")
                        oPersona = Session("objPersona")
                        If oForum.VerificaNotificaByPersona(oPersona.ID) = False Then
                            Me.oResource.setLinkButtonToValue(oLKBnotifica, "notificaAbilita", True, True)
                        Else
                            Me.oResource.setLinkButtonToValue(oLKBnotifica, "notificaDisabilita", True, True)
                        End If
                        oLKBnotifica.Visible = True
                    Else
                        oLKBnotifica.Visible = False
                    End If
                Else
                    oLKBnotifica.Visible = False
                End If
			Catch ex As Exception

			End Try

			Try
				Dim LNBmoderatori, LNBnotifica, oLNBArchivia, oLNBsegnala As LinkButton
				Dim LBhasNotifica, LBend, LBinit, oLBhasModeratori, oLBhasSegnala As Label


				LNBmoderatori = e.Item.FindControl("LNBmoderatori")
				LNBnotifica = e.Item.FindControl("LNBnotifica")
				LBhasNotifica = e.Item.FindControl("LBhasNotifica")
				oLBhasModeratori = e.Item.FindControl("LBhasModeratori")
				LBend = e.Item.FindControl("LBend")
				LBinit = e.Item.FindControl("LBinit")

				oLBhasSegnala = e.Item.FindControl("LBhasSegnala")
				oLNBsegnala = e.Item.FindControl("LNBsegnala")

				LNBmoderatori.Visible = False
				LBhasNotifica.Visible = False


				oLNBArchivia = e.Item.FindControl("LNBArchivia")

				Try
					
					If oServizio.GestioneForum Or ((oRuoloForum = Main.RuoloForumStandard.Amministratore Or oRuoloForum = Main.RuoloForumStandard.Moderatore) And isAbilitato) Then
						oLNBArchivia.Visible = True
					Else
						oLNBArchivia.Visible = False
					End If
					If Me.RBLselezioneForum.SelectedIndex = 0 Then
						Me.oResource.setLinkButtonToValue(oLNBArchivia, "NonArchiviato", True, True)
					Else
						Me.oResource.setLinkButtonToValue(oLNBArchivia, "Archiviato", True, True)
					End If

				Catch ex As Exception

				End Try

				If CBool(e.Item.DataItem("FRUM_Moderated")) = True Then
					LNBmoderatori.Visible = True

					Dim i_link As String
					Dim FRUM_ID As Integer
					FRUM_ID = e.Item.DataItem("FRUM_ID")
					i_link = "./InfoModeratori.aspx?FRUM_ID=" & FRUM_ID

					Me.oResource.setLinkButton(LNBmoderatori, True, True)
					oLBhasModeratori.Visible = oLNBArchivia.Visible
					LNBmoderatori.Attributes.Add("onClick", "OpenWin('" & i_link & "','800','400','no','yes');return false;")
					If LNBnotifica.Visible = True Then
						LBhasNotifica.Visible = True
					End If
				Else
					oLBhasModeratori.Visible = (oLNBArchivia.Visible And LNBnotifica.Visible)
				End If

                If Not isIscritto OrElse Not isAbilitato OrElse oRuoloForum = RuoloForumStandard.Ospite Then
                    oLBhasSegnala.Visible = False
                    oLNBsegnala.Visible = False
                Else
                    oLBhasSegnala.Visible = (oLNBArchivia.Visible Or LNBnotifica.Visible Or LNBmoderatori.Visible)
                    oLNBsegnala.Visible = True

                    Dim i_link As String
                    i_link = "./NotificaMessaggio.aspx?ForumId=" & e.Item.DataItem("FRUM_ID")


                    Me.oResource.setLinkButton(oLNBsegnala, True, True)
                    oLNBsegnala.Attributes.Add("onClick", "OpenWin('" & i_link & "','800','550','no','yes');return false;")
                End If


				LBinit.Visible = (oLNBsegnala.Visible Or oLNBArchivia.Visible Or LNBmoderatori.Visible Or LNBnotifica.Visible)
				LBend.Visible = LBinit.Visible
			Catch ex As Exception

			End Try

			'Vai direttamente al post !
			Try
				Dim oIMBgoToPost As ImageButton
				oIMBgoToPost = e.Item.FindControl("IMBgoToPost")
				Me.oResource.setImageButton(oIMBgoToPost, False, True, True)

				If IsDBNull(e.Item.DataItem("FRUM_LastPost_ID")) Or Not isAbilitato Then
					oIMBgoToPost.Visible = False
				Else
					oIMBgoToPost.Visible = True
				End If
			Catch ex As Exception

			End Try
		End If
	End Sub
	Public Sub DGforum_itemcommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGforum.ItemCommand
		Dim Forum_Name, Forum_id, RuoloForum, FRUM_LastPost_ID As String
		Dim isIscritto, isArchiviato As Boolean

		Forum_id = Me.DGforum.Items(e.Item.ItemIndex).Cells(1).Text()
		Forum_Name = Me.DGforum.Items(e.Item.ItemIndex).Cells(2).Text()
		RuoloForum = Me.DGforum.Items(e.Item.ItemIndex).Cells(4).Text()
		FRUM_LastPost_ID = Me.DGforum.Items(e.Item.ItemIndex).Cells(7).Text()
		isIscritto = CBool(Me.DGforum.Items(e.Item.ItemIndex).Cells(3).Text())
		isArchiviato = Me.DGforum.Items(e.Item.ItemIndex).Cells(9).Text()
		Select Case e.CommandName
			Case "normale"
				If isIscritto Then
					Session("IdForum") = Forum_id
					Session("ForumIsArchiviato") = isArchiviato
					Session("NomeForum") = Forum_Name
					Session("RuoloForum") = RuoloForum

					'Me.PageUtility.AddAction(ActionType.TopicList yuh, CreateObjectsList(ObjectType.Forum, Forum_id), InteractionType.UserWithUser)
					Me.PageUtility.RedirectToUrl("Forum/ForumThreads.aspx")
				Else
					Dim oForum As New COL_Forums
					Dim RLPC_id As Integer
					RLPC_id = Session("RLPC_ID")

					Try
						oForum.Id = Forum_id
                        oForum.Estrai()
                        Dim CurrentTypeId As Integer = PageUtility.CurrentUser.TipoPersona.ID
                        Dim DefaultRole As Integer = IIf(CurrentTypeId = TipoPersonaStandard.Guest Or CurrentTypeId = TipoPersonaStandard.PublicUser, CInt(Main.RuoloForumStandard.Ospite), -1)
                        RuoloForum = oForum.IscriviUtente(RLPC_id, DefaultRole)

                        Session("IdForum") = Forum_id
						Session("ForumIsArchiviato") = oForum.isArchiviato
						Session("NomeForum") = Forum_Name
						Session("RuoloForum") = RuoloForum

						'Me.PageUtility.AddAction(ActionType.Show, CreateObjectsList(ObjectType.Forum, Forum_id), InteractionType.UserWithUser)
						Me.PageUtility.RedirectToUrl("Forum/ForumThreads.aspx")
					Catch ex As Exception

					End Try
				End If
			Case "vaiApost"
				Dim oPost As New COL_Forum_posts
				Dim oThread As New COL_Forum_threads

				Try
					Session("IdForum") = Forum_id
					Session("NomeForum") = Forum_Name
					Session("ForumIsArchiviato") = isArchiviato
					If isIscritto Then
						Session("RuoloForum") = RuoloForum
					Else
						Dim oForum As New COL_Forums
						With oForum
							.Id = Forum_id
							RuoloForum = .IscriviUtente(Session("RLPC_ID"))
							Session("RuoloForum") = RuoloForum
						End With
					End If

					oThread = oPost.GetThread(FRUM_LastPost_ID)

					Session("IdThread") = oThread.Id
					Session("NomeThread") = oThread.Subject

					'Me.PageUtility.AddAction(ActionType.Show, CreateObjectsList(ObjectType.Thread, oThread.Id), InteractionType.UserWithUser)
					Me.PageUtility.RedirectToUrl("Forum/ElencoPost.aspx?#post_" & FRUM_LastPost_ID)
				Catch ex As Exception

				End Try
			Case "Vota"
				Dim ForumID As Integer
				Dim oForum As COL_Forums
				Dim oDropDownlist As HtmlSelect

				ForumID = CInt(Me.DGforum.DataKeys.Item(e.Item.ItemIndex)) 'sender.CommandArgument
				'posizione = sender.CommandName

				oDropDownlist = e.Item.FindControl("SL_voto_") 'Me.DGforum.Items(posizione).FindControl("SL_voto_") ' & ForumID)
				If IsNothing(oDropDownlist) = False Then
					Try
						oForum.Vota(ForumID, Session("objPersona").id, oDropDownlist.Items(oDropDownlist.SelectedIndex).Value)
					Catch ex As Exception

					End Try
					Me.Bind_Forum()
				End If
		End Select
	End Sub
#End Region

	Public Sub LNBnotifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnotifica.Click
		Try
			Dim oForum As New COL_Forums
			Dim oPersona As New COL_Persona
			oForum.Id = sender.CommandArgument
			oPersona = Session("objPersona")
			oForum.ModificaNotifica(oPersona.ID)
		Catch ex As Exception

		End Try
		Me.Bind_Forum()
	End Sub


	Protected Sub IMBgoToPost_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBgoToPost.Click
		Dim oPost As New COL_Forum_posts
		Dim oThread As New COL_Forum_threads
		Dim oForum As New COL_Forums
		Try
			With oPost
				.Id = sender.CommandArgument
				oThread = .GetThread(sender.CommandArgument)

				oForum.Id = oThread.ThreadForum
				oForum.Estrai()
				Session("IdForum") = oForum.Id
				Session("ForumIsArchiviato") = oForum.isArchiviato
				Session("NomeForum") = oForum.Name
				Session("RuoloForum") = oForum.getRuoloForIscritto(Session("RLPC_ID"), True)

				Session("IdThread") = oThread.Id
				Session("NomeThread") = oThread.Subject
				Session("ThreadArchiviato") = oThread.isArchiviato()

				'Me.PageUtility.AddAction(ActionType.Show, CreateObjectsList(ObjectType.Thread, oThread.Id), InteractionType.UserWithUser)
				Me.PageUtility.RedirectToUrl("Forum/ElencoPost.aspx?#post_" & .Id)
			End With
		Catch ex As Exception

		End Try
	End Sub

    Private Sub RBLselezioneForum_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLselezioneForum.SelectedIndexChanged
        Me.Bind_Forum()
    End Sub

    Protected Sub LNBArchivia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBArchivia.Click
        Try
            Dim oForum As COL_Forums
            oForum.ModificaArchiviazione(sender.CommandArgument)

            Me.Bind_Selezione()
            Me.Bind_Forum()
        Catch ex As Exception

        End Try
    End Sub

    'Protected Sub IMGvota_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMGvota.Click
    '    If sender.CommandArgument <> "" Then
    '        Try
    '            Dim ForumID, posizione As Integer
    '            Dim oForum As COL_Forums
    '            Dim oDropDownlist As HtmlSelect

    '            ForumID = sender.CommandArgument
    '            posizione = sender.CommandName


    '            oDropDownlist = Me.DGforum.Items(posizione).FindControl("SL_voto_") ' & ForumID)
    '            If IsNothing(oDropDownlist) = False Then
    '                Try
    '                    oForum.Vota(ForumID, Session("objPersona").id, oDropDownlist.Items(oDropDownlist.SelectedIndex).Value)
    '                Catch ex As Exception

    '                End Try
    '                Me.Bind_Forum()
    '            End If
    '        Catch ex As Exception

    '        End Try
    '    End If
    'End Sub

#Region "Bind Statistiche"
    Private Sub BindStatistiche()
        Dim oTable As New DataSet
        Dim oForum As New Forum.COL_Forums

        Dim IDComunita As Integer
        Try
            IDComunita = CInt(Session("idComunita"))
            oTable = oForum.StatistichePostComunita(IDComunita)
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


	Private Function CreateObjectsList(ByVal oType As Services_Forum.ObjectType, ByVal oValueID As String) As List(Of lm.ActionDataContract.ObjectAction)
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
'		<META http-equiv="Content-Type" content="text/html; charset=windows-1252"/>
'		<script language="Javascript" src="./../jscript/generali.js"></script>
'		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
'		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
'		<meta content="JavaScript" name="vs_defaultClientScript"/>
'		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
'		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
'	</HEAD>
'	<body>
'		 <form id="aspnetForm" runat="server">
'		 <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
'			<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
'			    <tr>
'				    <td colspan="3" >
'				    <div>
'				        <HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>	
'				    </div>
'				    <br style="clear:both;" />
'				    </td>
'			    </tr>
'				<tr class="contenitore">
'					<td colSpan="3">
'						<table cellSpacing="0" cellPadding="0" width="900px" border="0">
'							<tr>
'								<td class="RigaTitolo" align="left">
'									<asp:Label ID="LBTitolo" Runat="server">Forum</asp:Label>
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
'			</table><FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
'		</form>
'	</body>
'</HTML>
