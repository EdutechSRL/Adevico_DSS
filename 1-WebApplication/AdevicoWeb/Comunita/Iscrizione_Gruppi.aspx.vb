Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_IscrizioneComunita

Public Class Iscrizione_Gruppi
	Inherits System.Web.UI.Page
	Private oResource As ResourceManager

    Private _PageUtility As PresentationLayer.OLDpageUtility
    Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property

	Private Enum Iscrizioni_code
		IscrizioniAperteIl = 0
		IscrizioniChiuse = 1
		IscrizioniComplete = 2
		IscrizioniEntro = 3
	End Enum
	Private Enum stringaRegistrazione
		errore = 0
		inAttesa = 1
		limiteIscrizione = 2
		iscritto = 3
	End Enum
	Private Enum StringaOrdinamento
		Crescente = 0
		Decrescente = 1
		Corrente = 2
	End Enum
	Private Enum stringaTitolo
		forSubscribe = 0
		Subscribed = 1
		standard = 2
	End Enum
	Private Enum StringaElenco
		noCommunityForFilter = 0
		noCommunity = 1
	End Enum
	Private Enum stringaMessaggio
		seleziona = 0
		indietro = 1
	End Enum
	Private Enum StringaAbilitato
		abilitato = 1
		bloccato = 0
		inAttesa = -1
		comunitaArchiviata = 2
		comunitaBloccata = 3
	End Enum


	Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
	Protected WithEvents LNBalbero As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBalberoGerarchico As System.Web.UI.WebControls.LinkButton

	Protected WithEvents PNLmenuIscritto As System.Web.UI.WebControls.Panel
	Protected WithEvents LNBelencoIscritte As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBiscriviAltre As System.Web.UI.WebControls.LinkButton

	Protected WithEvents PNLmenuDettagli As System.Web.UI.WebControls.Panel
	Protected WithEvents LNBannullaDettagli As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBiscriviDettagli As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBentraDettagli As System.Web.UI.WebControls.LinkButton

	Protected WithEvents PNLmenuConferma As System.Web.UI.WebControls.Panel
	Protected WithEvents LNBannullaConferma As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LNBiscriviConferma As System.Web.UI.WebControls.LinkButton

	Protected WithEvents LNBiscriviMultipli As System.Web.UI.WebControls.LinkButton
	Protected WithEvents PNLmenuAccesso As System.Web.UI.WebControls.Panel
	Protected WithEvents LNBannulla As System.Web.UI.WebControls.LinkButton


#Region "Filtri"
	Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox
	Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
	Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
	Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
	Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
	Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton
	Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
	Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table


	Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList

	Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
	Protected WithEvents LBnumeroRecord_c As System.Web.UI.WebControls.Label
	Protected WithEvents TBCtipoRicerca_c As System.Web.UI.WebControls.TableCell
	Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label

	Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
	Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
	Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList

	Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
	Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

	Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table

	Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
	Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label



	Protected WithEvents CBXmostraPadre As System.Web.UI.WebControls.CheckBox


	Protected WithEvents DDLresponsabile As System.Web.UI.WebControls.DropDownList

#Region "Lettere"
	Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBa As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBb As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBc As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBd As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBe As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBf As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBg As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBh As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBi As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBj As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBk As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBl As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBm As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBn As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBo As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBp As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBq As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBr As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBs As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBt As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBu As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBv As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBw As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBx As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBy As System.Web.UI.WebControls.LinkButton
	Protected WithEvents LKBz As System.Web.UI.WebControls.LinkButton
#End Region

	Protected WithEvents LBricercaByIscrizione_c As System.Web.UI.WebControls.Label
	Protected WithEvents RBLricercaByIscrizione As System.Web.UI.WebControls.RadioButtonList

#Region "Filtri automatici"
	Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNcomunitaSelezionate As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDN_filtroFacolta As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDN_filtroResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDN_filtroStatus As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

	Protected WithEvents TBRfiltriGenerici As System.Web.UI.WebControls.TableRow
#End Region

#Region "FORM PERMESSI"
	Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
	Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Pannello Contenuto"
	Protected WithEvents PNLContenuto As System.Web.UI.WebControls.Panel
	Protected WithEvents DGcomunita As System.Web.UI.WebControls.DataGrid
	Protected WithEvents LBmsgDG As System.Web.UI.WebControls.Label
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
#End Region

#Region "Pannello Messaggio"
	Protected WithEvents LBMessaggi As System.Web.UI.WebControls.Label
	Protected WithEvents PNLmessaggi As System.Web.UI.WebControls.Panel
#End Region

#Region "Form Dettagli"
	Protected WithEvents LBlegenda As System.Web.UI.WebControls.Label
	Protected WithEvents HDNisChiusaForPadre As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNtprl_id As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNcmnt_Path As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
	Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita
#End Region

	Protected WithEvents PNLiscrizioneAvvenuta As System.Web.UI.WebControls.Panel
	Protected WithEvents LBiscrizione As System.Web.UI.WebControls.Label

#Region "Conferma Iscrizione"
	Protected WithEvents PNLconferma As System.Web.UI.WebControls.Panel
	Protected WithEvents LBconferma As System.Web.UI.WebControls.Label
	Protected WithEvents LBconfermaMultipla As System.Web.UI.WebControls.Label
	Protected WithEvents HDisChiusa As System.Web.UI.HtmlControls.HtmlInputHidden
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

		If Not IsPostBack Then
			Me.SetupInternazionalizzazione()

			Try
				Dim HasPermessi As Boolean = True
				Dim ShowMultiplo As Boolean = False
				Dim oServizioIscrizione As New Services_IscrizioneComunita


				Session("CMNT_path_forNews") = ""
				Session("CMNT_ID_forNews") = ""
				Session("azione") = "load"
				Session("AdminForChange") = False
				Session("idComunita_forAdmin") = ""
				Session("CMNT_path_forAdmin") = ""


                'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & Iscrizione_Gruppi.stringaTitolo.forSubscribe)
                Me.Master.ServiceTitle = ("LBtitolo." & Iscrizione_Gruppi.stringaTitolo.forSubscribe)
                ShowMultiplo = True

				oServizioIscrizione = Me.ImpostaPermessiIscrizione


				HasPermessi = (oServizioIscrizione.Admin Or oServizioIscrizione.List)
				If HasPermessi Then
					Me.Reset_Contenuto(False, ShowMultiplo)
					If Me.Request.QueryString("re_set") <> "true" Then
						Me.ViewState("intCurPage") = 0
						Me.ViewState("intAnagrafica") = "-1"
						Me.ViewState("SortExspression") = "CMNT_Nome"
						Me.ViewState("SortDirection") = "asc"
						Me.LKBtutti.CssClass = "lettera_Selezionata"
						Me.TBRapriFiltro.Visible = False
						Me.TBRchiudiFiltro.Visible = True
						Me.TBRfiltri.Visible = True
					End If

					Me.SetupFiltri()
				Else
					Reset_NoPermessi()
				End If

				Me.PageUtility.AddAction(IIf(Me.PNLpermessi.Visible, ActionType.NoPermission, ActionType.CommunityList))
			Catch ex As Exception

			End Try
        End If

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID
	End Sub


	Private Function ImpostaPermessiIscrizione() As Services_IscrizioneComunita
		Dim ComunitaID As Integer = 0
		Dim iResponse As String = "00000000000000000000000000000000"
		Dim oServizioIscrizione As New Services_IscrizioneComunita
		Dim oPersona As COL_Persona

		Try
			oPersona = Session("objPersona")
			ComunitaID = Session("idComunita")
		Catch ex As Exception
			ComunitaID = 0
		End Try
		Try
			If ComunitaID = 0 Then
				Session("Limbo") = True
				If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Guest Then
					iResponse = "00000000000000000000000000000000"
				Else
					oServizioIscrizione.Admin = False
					oServizioIscrizione.List = True
					Return oServizioIscrizione
				End If
			Else
				iResponse = Permessi(oServizioIscrizione.Codex, Me.Page)
			End If
			If (iResponse = "") Then
				iResponse = "00000000000000000000000000000000"
			End If
		Catch ex As Exception
			iResponse = "00000000000000000000000000000000"
		End Try
		oServizioIscrizione.PermessiAssociati = iResponse
		Return oServizioIscrizione
	End Function

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
			Return True
			'Else
			'    Try
			'        Dim CMNT_ID As Integer = 0
			'        Try
			'            If Session("AdminForChange") = True Then
			'                CMNT_ID = Session("idComunita_forAdmin")
			'            Else
			'                CMNT_ID = Session("idComunita")
			'            End If
			'        Catch ex As Exception
			'            Try
			'                CMNT_ID = Session("idComunita")
			'            Catch ex2 As Exception
			'                CMNT_ID = 0
			'            End Try

			'        End Try

			'        If CMNT_ID <= 0 Then
			'            Me.ExitToLimbo()
			'            Return True
			'        End If
			'    Catch ex As Exception
			'        Me.ExitToLimbo()
			'        Return True
			'    End Try
		End If
	End Function

#Region "Reset Pannelli"
	Private Sub ResetFormAll()
		Me.PNLContenuto.Visible = False
		Me.PNLconferma.Visible = False
		Me.PNLdettagli.Visible = False
		Me.PNLmessaggi.Visible = False
		Me.PNLiscrizioneAvvenuta.Visible = False
		Me.PNLmenu.Visible = False
		Me.PNLmenuAccesso.Visible = False
		Me.PNLmenuConferma.Visible = False
		Me.PNLmenuIscritto.Visible = False
		Me.PNLmenuDettagli.Visible = False
		Me.PNLpermessi.Visible = False
	End Sub
	Private Sub Reset_NoPermessi()
		Me.ResetFormAll()
		'If Not NessunPermesso Then
		Me.PNLContenuto.Visible = True
		Me.DGcomunita.Visible = False
		Me.LBmsgDG.Visible = False
		Me.BTNCerca.Enabled = False
		Me.CBXautoUpdate.Checked = True
		Me.CBXautoUpdate.Enabled = False
		Me.DDLorganizzazione.Enabled = False
		Me.DDLresponsabile.Enabled = False
		Me.DDLTipoRicerca.Enabled = False
		Me.TBRfiltriGenerici.Visible = False
		'	End If
		Me.PNLpermessi.Visible = True
	End Sub
	Private Sub Reset_Contenuto(Optional ByVal update As Boolean = False, Optional ByVal ShowMultipli As Boolean = False)
		Session("Azione") = "loaded"
		Me.ResetFormAll()
		Me.PNLContenuto.Visible = True
		Me.PNLmenu.Visible = True
		Me.LNBiscriviMultipli.Visible = ShowMultipli
		Me.PNLpermessi.Visible = False
		If update Then
			Me.Bind_Griglia()
		End If
		Me.HDNcmnt_ID.Value = ""
		Me.HDNcmnt_Path.Value = ""
		Me.HDNisChiusaForPadre.Value = ""
		Me.HDisChiusa.Value = ""
	End Sub
	Private Sub Reset_ToDettagli()
		Me.ResetFormAll()
		Me.PNLdettagli.Visible = True
		Me.PNLmenuDettagli.Visible = True
	End Sub
	Private Sub Reset_ToMessaggi()
		Me.ResetFormAll()
		Me.PNLmessaggi.Visible = True
		Me.PNLmenuAccesso.Visible = True
	End Sub
	Private Sub Reset_ToConferma()
		Me.ResetFormAll()
		Me.PNLconferma.Visible = True
		Me.PNLmenuConferma.Visible = True
	End Sub
	Private Sub Reset_ToIscrizioneAvvenuta()
		Me.ResetFormAll()
		Me.PNLiscrizioneAvvenuta.Visible = True
		Me.PNLmenuAccesso.Visible = True
	End Sub
	Private Sub ResetFormToConferma(ByVal Multiplo As Boolean, ByVal Comunita As String, ByVal Responsabile As String)
		Me.ResetFormAll()
		Me.PNLconferma.Visible = True
		Me.PNLmenuConferma.Visible = True
		If Multiplo Then
			Me.LBconfermaMultipla.Visible = True
			Me.LBconferma.Visible = False
			Me.oResource.setLabel(Me.LBconfermaMultipla)
			Me.LBconfermaMultipla.Text = Me.LBconfermaMultipla.Text & "<br>" & Comunita
		Else
			Me.LBconfermaMultipla.Visible = False
			Me.LBconferma.Visible = True
			Me.oResource.setLabel(Me.LBconferma)
			Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeComunita#", Comunita)
			Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeResponsabile#", Responsabile)
		End If
	End Sub
#End Region

#Region "Bind_Dati"
	Private Sub ChangeNumeroRecord(ByVal num As Integer)
		Try
			Me.DDLNumeroRecord.SelectedValue = num

		Catch ex As Exception
			Dim i, totale As Integer
			totale = Me.DDLNumeroRecord.Items.Count - 1
			For i = 0 To totale
				If Me.DDLNumeroRecord.Items(0).Value <= num Then
					Me.DDLNumeroRecord.SelectedIndex = -1
					Me.DDLNumeroRecord.Items(0).Selected = True
				Else
					Exit For
				End If
			Next
		End Try
	End Sub

	Private Sub SetupFiltri()
		Dim oImpostazioni As New COL_ImpostazioniUtente
		Me.Bind_Organizzazioni()
		If Me.Request.QueryString("re_set") = "true" Then

			Try
				Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")
			Catch ex As Exception
				Me.Response.Cookies("RicercaComunitaUtente")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
			End Try

			Me.SetupSearchParameters()
			Try
				Me.Bind_Responsabili(Me.Request.Cookies("DDLresponsabile")("DDLorganizzazione"))
			Catch ex As Exception
				Me.Bind_Responsabili()
			End Try

		ElseIf Not IsNothing(Session("oImpostazioni")) Then
			Try
				oImpostazioni = Session("oImpostazioni")
				Me.Bind_Organizzazioni()
				With oImpostazioni
					Try
						Me.DDLorganizzazione.SelectedValue = .Organizzazione_Ricerca
					Catch ex As Exception

					End Try

					Try
						Me.ChangeNumeroRecord(oImpostazioni.Nrecord_Ricerca)
					Catch ex As Exception

					End Try
				End With
			Catch ex As Exception

			End Try

			Me.Bind_Responsabili()
			Me.CBXautoUpdate.Checked = True
			Me.SaveSearchParameters()
		Else
			Me.CBXautoUpdate.Checked = True
			Me.Bind_Responsabili()
		End If

		Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
		Me.DDLTipoRicerca.Attributes.Add("onchange", "return AggiornaForm();")

		Try
			Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue
			Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue

			Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
			Me.HDN_filtroValore.Value = Me.TXBValore.Text
			Me.HDNselezionato.Value = Me.HDN_filtroTipoRicerca.Value
		Catch ex As Exception

		End Try

		Me.Bind_Griglia(True)
	End Sub

	'Bind dati relativi ai filtri.

	Private Sub Bind_Organizzazioni()
		Dim oDataset As New DataSet
		Dim oPersona As New COL_Persona

		Me.DDLorganizzazione.Items.Clear()
		Try
			oPersona = Session("objPersona")
			oDataset = oPersona.GetOrganizzazioniAssociate(True)

			If oDataset.Tables(0).Rows.Count > 0 Then
				Dim oComunita As New COL_Comunita

				Dim ArrComunita(,) As String
				Dim FacoltaID As Integer
				Dim show As Boolean = False
				Try
					If IsArray(Session("ArrComunita")) And Session("limbo") = False Then
						ArrComunita = Session("ArrComunita")
						oComunita.Id = ArrComunita(0, 0)
						oComunita.Estrai()
						FacoltaID = oComunita.Organizzazione.Id
						show = False
					ElseIf Session("limbo") = True Then
						show = True
						FacoltaID = -1
					End If
				Catch ex As Exception
					Try
						FacoltaID = Session("ORGN_id")
						show = False
					Catch exc As Exception
						FacoltaID = -1
					End Try

				End Try

				Me.DDLorganizzazione.DataValueField = "ORGN_id"
				Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
				Me.DDLorganizzazione.DataSource = oDataset
				Me.DDLorganizzazione.DataBind()
				'If oDataset.Tables(0).Rows.Count > 1 Then
				'    Me.DDLorganizzazione.Items.Insert(0, New ListItem("<< tutte >>", -1))
				'End If

				If Me.DDLorganizzazione.Items.Count > 1 Then
					Me.DDLorganizzazione.Enabled = True

					If FacoltaID >= 0 Then
						Try
							Me.DDLorganizzazione.SelectedValue = FacoltaID
						Catch ex As Exception
							Me.DDLorganizzazione.Items.Clear()
							Me.DDLorganizzazione.Items.Add(New ListItem(oComunita.Nome, FacoltaID))
							Me.DDLorganizzazione.SelectedIndex = 0
						End Try
					Else
						Me.DDLorganizzazione.SelectedIndex = 0
					End If
				Else
					Me.DDLorganizzazione.Enabled = False
				End If
			Else
				Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
				Me.DDLorganizzazione.Enabled = False
			End If
		Catch ex As Exception
			Me.DDLorganizzazione.Items.Clear()
			Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
			Me.DDLorganizzazione.Enabled = False
		End Try
		oResource.setDropDownList(Me.DDLorganizzazione, -1)
		oResource.setDropDownList(Me.DDLorganizzazione, 0)
	End Sub

	Private Sub Bind_Responsabili(Optional ByVal DocenteID As Integer = -1)
		Dim oDataSet As New DataSet
		Dim oComunita As COL_Comunita
		Dim FacoltaID As Integer = -1
		Dim ComunitaID As Integer = -1
		Try
			Dim TipoComuniaID As Integer = Main.TipoComunitaStandard.GruppoDiLavoro

			Me.DDLresponsabile.Items.Clear()
			Try
				FacoltaID = Me.DDLorganizzazione.SelectedValue
			Catch ex As Exception
				FacoltaID = -1
			End Try
			Try
				If Session("IdComunita") > 0 Then
					ComunitaID = Session("IdComunita")
				End If
			Catch ex As Exception

			End Try


			oDataSet = oComunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , FiltroStatoComunita.Attiva, FiltroRicercaComunitaByIscrizione.nonIscritto)
			If oDataSet.Tables(0).Rows.Count > 0 Then
				DDLresponsabile.DataSource = oDataSet
				DDLresponsabile.DataTextField() = "Anagrafica"
				DDLresponsabile.DataValueField() = "PRSN_ID"
				DDLresponsabile.DataBind()

				'aggiungo manualmente elemento che indica tutti i tipi di comunità
				DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
			Else
				DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
			End If
		Catch ex As Exception
			Me.DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
		End Try
		oResource.setDropDownList(Me.DDLresponsabile, -1)
		If DocenteID > 0 Then
			Try
				Me.DDLresponsabile.SelectedValue = DocenteID
			Catch ex As Exception

			End Try
		End If

		Try
			If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
				Me.DDLresponsabile.Visible = True
				Me.TXBValore.Visible = False
				Me.TXBValore.Text = ""
			Else
				Me.DDLresponsabile.Visible = False
				Me.TXBValore.Visible = True
			End If
		Catch ex As Exception

		End Try
	End Sub
	Private Function FiltraggioDati(Optional ByVal ApplicaFiltri As Boolean = False) As DataSet
		Dim oPersona As New COL_Persona

		Dim i, totale, totaleHistory As Integer
		Dim oDataset As New DataSet
		Try
			Dim valore As String = ""
			Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
			Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti

			oPersona = Session("objPersona")
			Try
				If IsNumeric(Me.ViewState("intAnagrafica")) Then
					oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroComunita)
				Else
					oFiltroLettera = Main.FiltroComunita.tutti
					Me.SelezionaLink_All()
				End If
			Catch ex As Exception
				oFiltroLettera = Main.FiltroComunita.tutti
				Me.SelezionaLink_All()
			End Try

			If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
				If Me.TXBValore.Text <> "" Then
					Me.TXBValore.Text = Trim(Me.TXBValore.Text)
				End If
				valore = Me.TXBValore.Text
			Else
				Try
					valore = Trim(Me.HDN_filtroValore.Value)
				Catch ex As Exception

				End Try
			End If

			Dim TipoRicercaID As Integer
			If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
				TipoRicercaID = Me.DDLTipoRicerca.SelectedValue
			Else
				Try
					TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
				Catch ex As Exception
					TipoRicercaID = -1
				End Try
			End If
			If valore <> "" Or (Me.CBXautoUpdate.Checked And Me.DDLresponsabile.Visible) Or (Not Me.CBXautoUpdate.Checked And TipoRicercaID = Main.FiltroComunita.IDresponsabile) Then
				Select Case TipoRicercaID
					Case Main.FiltroComunita.nome
						oFiltroTipoRicerca = Main.FiltroComunita.nome
					Case Main.FiltroComunita.creataDopo
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.creataDopo
						End If
					Case Main.FiltroComunita.creataPrima
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.creataPrima
						End If
					Case Main.FiltroComunita.dataIscrizioneDopo
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.dataIscrizioneDopo
						End If
					Case Main.FiltroComunita.dataFineIscrizionePrima
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.dataFineIscrizionePrima
						End If
					Case Main.FiltroComunita.contiene
						oFiltroTipoRicerca = Main.FiltroComunita.contiene
					Case Main.FiltroComunita.cognomeDocente
						oFiltroTipoRicerca = Main.FiltroComunita.cognomeDocente
					Case Main.FiltroComunita.IDresponsabile
						Try
							If Me.CBXautoUpdate.Checked Then
								valore = Me.DDLresponsabile.SelectedValue
							Else
								valore = Me.HDN_filtroResponsabileID.Value
							End If

							oFiltroTipoRicerca = Main.FiltroComunita.IDresponsabile
						Catch ex As Exception
							valore = -1
						End Try
					Case Else
						valore = ""
				End Select
			End If
			If (Me.CBXautoUpdate.Checked Or ApplicaFiltri = True) And valore = "" Then
				Me.TXBValore.Text = valore
				Me.HDN_filtroValore.Value = ""
			End If

			Dim ComunitaPadreID As Integer
			Try
				ComunitaPadreID = Session("idComunita")
				If ComunitaPadreID < 1 Then
					ComunitaPadreID = -1
				End If
			Catch ex As Exception
				ComunitaPadreID = -1
			End Try

			Dim FacoltaID, StatusID As Integer
			Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto
			If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
				Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
				Me.HDN_filtroValore.Value = Me.TXBValore.Text
				Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue

				Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue
			End If
			Try
				FacoltaID = Me.HDN_filtroFacolta.Value
			Catch ex As Exception
				FacoltaID = -1
			End Try

			Try
				StatusID = Me.HDN_filtroStatus.Value
			Catch ex As Exception

			End Try
			Dim oComunita As New COL_Comunita

			If valore <> "" Then
				valore = Replace(valore, "'", "''")
			End If
			oDataset = oComunita.RicercaComunita(oFiltroIscrizione, Session("LinguaID"), FacoltaID, ComunitaPadreID, oPersona.ID, oFiltroTipoRicerca, oFiltroLettera, valore, Main.TipoComunitaStandard.GruppoDiLavoro, , , , , StatusID)

			oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
			oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
			oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
			oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
			oDataset.Tables(0).Columns.Add(New DataColumn("Iscritti"))

			If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("CMNT_Totale") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Totale"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("HasNews") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("HasNews"))
			End If


			Dim oDataview As New DataView
			Dim ElencoComunitaID As String = ","

			oDataview = oDataset.Tables(0).DefaultView
			oDataview.AllowDelete = True
			totale = oDataset.Tables(0).Rows.Count


			If IsArray(Session("ArrComunita")) Then
				Try
					Dim ArrComunita(,) As String
					ArrComunita = Session("ArrComunita")
					totaleHistory = UBound(ArrComunita, 2)
					oDataview.RowFilter = "ALCM_PATH not like '" & ArrComunita(2, totaleHistory) & "%"
					While oDataview.Count > 0
						oDataview.Delete(0)
					End While
					oDataview.RowFilter = ""
					oDataset.AcceptChanges()
				Catch ex As Exception
					oDataview.RowFilter = ""
				End Try
			End If



			While oDataview.Count > 0
				Dim ComunitaID As Integer


				If ElencoComunitaID = "," Then
					ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
					oDataview.RowFilter = "CMNT_ID=" & ComunitaID
				Else
					oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

					If oDataview.Count > 0 Then
						ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
					End If
				End If

				If oDataview.Count = 1 Then
					oDataview.RowFilter = ""
					ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
				ElseIf oDataview.Count > 1 Then
					oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
					If oDataview.Count = 1 Then
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"	'' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
						While oDataview.Count > 0
							oDataview.Delete(0)
						End While
					ElseIf oDataview.Count = 0 Then
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"	'''%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
						While oDataview.Count > 1
							oDataview.Delete(1)
						End While
					ElseIf oDataview.Count > 1 Then
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
						While oDataview.Count > 1
							oDataview.Delete(1)
						End While
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
						While oDataview.Count > 0
							oDataview.Delete(0)
						End While
					End If

					oDataview.RowFilter = ""
					ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
				End If

			End While
			oDataview.RowFilter = ""
			oDataset.AcceptChanges()


			Dim ImageBaseDir, img As String
			ImageBaseDir = GetPercorsoApplicazione(Me.Request)
			ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
			ImageBaseDir = Replace(ImageBaseDir, "//", "/")

			totale = oDataset.Tables(0).Rows.Count
			Me.DGcomunita.Columns(6).Visible = False

            For i = 0 To totale - 1
                Dim oRow As DataRow

                oRow = oDataset.Tables(0).Rows(i)


                If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                    oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                    oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                Else
                    If oRow.Item("CMNT_Responsabile") = "" Then
                        oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                        oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                    Else
                        oRow.Item("AnagraficaResponsabile") = oRow.Item("CMNT_Responsabile")
                    End If
                End If
                If oRow.Item("CMNT_IsChiusa") = True Then
                    oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                    oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                    oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                    oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                Else
                    oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                    oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                End If
                If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                    img = oRow.Item("TPCM_icona")
                    img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                    oRow.Item("TPCM_icona") = img
                End If
                If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                    If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                        oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                    End If
                End If
                If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                    If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                        oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                    End If
                End If
                If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                    oRow.Item("AnnoAccademico") = "&nbsp;"
                Else
                    oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                End If
                If IsDBNull(oRow.Item("PRDO_descrizione")) Then
                    oRow.Item("Periodo") = "&nbsp;"
                Else
                    oRow.Item("Periodo") = oRow.Item("PRDO_descrizione")
                End If
                If Me.CBXmostraPadre.Checked Then
                    If IsDBNull(oRow.Item("CMNT_NomePadre")) Then
                        oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                        oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                    Else
                        If oRow.Item("CMNT_NomePadre") = "" Then
                            oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                            oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                        Else
                            oRow.Item("CMNT_Esteso") = "<span class=small_Padre>" & oRow.Item("CMNT_nomePadre") & "</span>&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                            oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_NomePadre") & "&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                        End If
                    End If
                Else
                    oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                    oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                End If

                Try
                    Dim numIscritti, maxIscritti As Integer
                    maxIscritti = oRow.Item("CMNT_MaxIscritti")
                    numIscritti = oRow.Item("CMNT_Iscritti")
                    Try
                        oRow.Item("Iscritti") = numIscritti
                    Catch ex As Exception
                        oRow.Item("Iscritti") = 0
                        numIscritti = 0
                    End Try

                    If (maxIscritti <= 0) Then
                        oRow.Item("CMNT_Iscritti") = 0
                    Else
                        If numIscritti > maxIscritti Then
                            oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            oRow.Item("Iscritti") = oResource.getValue("limiti.superato")
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#num1#", "<b>" & numIscritti & "</b>")
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#limite#", maxIscritti)
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#numOver#", numIscritti - maxIscritti)
                        ElseIf numIscritti = maxIscritti Then
                            oRow.Item("CMNT_Iscritti") = -1
                            oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                        Else
                            oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                        End If

                        Me.DGcomunita.Columns(6).Visible = True
                    End If
                Catch ex As Exception

                End Try
                Try
                    If Me.PageUtility.CommunityNewsCount(Me.PageUtility.CurrentUser.ID, CInt(oRow.Item("CMNT_ID"))).Count <= 0 Then
                        oRow.Item("HasNews") = False
                    Else
                        oRow.Item("HasNews") = True
                    End If
                Catch ex As Exception
                    oRow.Item("HasNews") = False
                End Try
            Next
		Catch ex As Exception

		End Try
		Return oDataset
	End Function
	Private Function FiltraggioDatiRistretto() As DataSet
		Dim oPersona As New COL_Persona

		Dim i, totale, totaleHistory, PRSN_ID As Integer
		Dim oDataset As New DataSet

		Try

			Dim valore As String = ""
			Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
			Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti

			oPersona = Session("objPersona")
			Try
				If IsNumeric(Me.ViewState("intAnagrafica")) Then
					oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroComunita)
				Else
					oFiltroLettera = Main.FiltroComunita.tutti
					Me.SelezionaLink_All()
				End If
			Catch ex As Exception
				oFiltroLettera = Main.FiltroComunita.tutti
				Me.SelezionaLink_All()
			End Try

			If Me.CBXautoUpdate.Checked Then
				If Me.TXBValore.Text <> "" Then
					Me.TXBValore.Text = Trim(Me.TXBValore.Text)
				End If
				valore = Me.TXBValore.Text
			Else
				Try
					valore = Trim(Me.HDN_filtroValore.Value)
				Catch ex As Exception

				End Try
			End If

			Dim TipoRicercaID As Integer
			If Me.CBXautoUpdate.Checked Then
				Try
					TipoRicercaID = Me.DDLTipoRicerca.SelectedValue
				Catch ex As Exception
					TipoRicercaID = -1
				End Try
			Else
				Try
					TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
				Catch ex As Exception
					TipoRicercaID = -1
				End Try
			End If

			If valore <> "" Or (Me.CBXautoUpdate.Checked And Me.DDLresponsabile.Visible) Or (Not Me.CBXautoUpdate.Checked And TipoRicercaID = Main.FiltroComunita.IDresponsabile) Then
				Select Case TipoRicercaID
					Case Main.FiltroComunita.nome
						oFiltroTipoRicerca = Main.FiltroComunita.nome
					Case Main.FiltroComunita.creataDopo
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.creataDopo
						End If
					Case Main.FiltroComunita.creataPrima
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.creataPrima
						End If
					Case Main.FiltroComunita.dataIscrizioneDopo
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.dataIscrizioneDopo
						End If
					Case Main.FiltroComunita.dataFineIscrizionePrima
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.dataFineIscrizionePrima
						End If
					Case Main.FiltroComunita.contiene
						oFiltroTipoRicerca = Main.FiltroComunita.contiene
					Case Main.FiltroComunita.cognomeDocente
						oFiltroTipoRicerca = Main.FiltroComunita.cognomeDocente
					Case Main.FiltroComunita.IDresponsabile
						Try
							If Me.CBXautoUpdate.Checked Then
								valore = Me.DDLresponsabile.SelectedValue
							Else
								valore = Me.HDN_filtroResponsabileID.Value
							End If

							oFiltroTipoRicerca = Main.FiltroComunita.IDresponsabile
						Catch ex As Exception
							valore = -1
						End Try
					Case Else
						valore = ""
				End Select
			End If

			Dim ComunitaPadreID As Integer
			Try
				ComunitaPadreID = Session("idComunita")
				If ComunitaPadreID < 1 Then
					ComunitaPadreID = -1
				End If
			Catch ex As Exception
				ComunitaPadreID = -1
			End Try

			Dim FacoltaID, StatusID As Integer
			Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto
			If Me.CBXautoUpdate.Checked Then
				Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
				Me.HDN_filtroValore.Value = Me.TXBValore.Text
				Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
				Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue
			End If
			Try
				FacoltaID = Me.HDN_filtroFacolta.Value
			Catch ex As Exception
				FacoltaID = -1
			End Try

			Try
				StatusID = Me.HDN_filtroStatus.Value
			Catch ex As Exception
				StatusID = 0
			End Try

			Dim oComunita As New COL_Comunita
			If valore <> "" Then
				valore = Replace(valore, "'", "''")
			End If
			oDataset = oComunita.RicercaComunita(oFiltroIscrizione, Session("LinguaID"), FacoltaID, ComunitaPadreID, oPersona.ID, oFiltroTipoRicerca, oFiltroLettera, valore, Main.TipoComunitaStandard.GruppoDiLavoro, , , StatusID)

			Dim oDataview As New DataView
			Dim ElencoComunitaID As String = ","

			oDataview = oDataset.Tables(0).DefaultView
			oDataview.AllowDelete = True
			totale = oDataset.Tables(0).Rows.Count


			If IsArray(Session("ArrComunita")) Then
				Try
					Dim ArrComunita(,) As String
					ArrComunita = Session("ArrComunita")
					totaleHistory = UBound(ArrComunita, 2)
					oDataview.RowFilter = "ALCM_PATH not like '" & ArrComunita(2, totaleHistory) & "%"
					While oDataview.Count > 0
						oDataview.Delete(0)
					End While
					oDataview.RowFilter = ""
					oDataset.AcceptChanges()
				Catch ex As Exception
					oDataview.RowFilter = ""
				End Try
			End If



			While oDataview.Count > 0
				Dim ComunitaID As Integer


				If ElencoComunitaID = "," Then
					ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
					oDataview.RowFilter = "CMNT_ID=" & ComunitaID
				Else
					oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

					If oDataview.Count > 0 Then
						ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
					End If
				End If

				If oDataview.Count = 1 Then
					oDataview.RowFilter = ""
					ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
				ElseIf oDataview.Count > 1 Then
					oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
					If oDataview.Count = 1 Then
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"	'' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
						While oDataview.Count > 0
							oDataview.Delete(0)
						End While
					ElseIf oDataview.Count = 0 Then
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"	'''%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
						While oDataview.Count > 1
							oDataview.Delete(1)
						End While
					ElseIf oDataview.Count > 1 Then
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
						While oDataview.Count > 1
							oDataview.Delete(1)
						End While
						oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
						While oDataview.Count > 0
							oDataview.Delete(0)
						End While
					End If

					oDataview.RowFilter = ""
					ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
				End If

			End While
			oDataview.RowFilter = ""
			oDataset.AcceptChanges()
		Catch ex As Exception

		End Try
		Return oDataset
	End Function
	Private Sub Bind_Griglia(Optional ByVal ApplicaFiltri As Boolean = False)
		'carica le comunità nella datagrid DGComunita
		'se gli passo 0 seleziona tutte
		Me.LBmsgDG.Visible = False
		Me.DGcomunita.Visible = True 'se la datagrid era vuota allora era stata nascosta


		Dim oPersona As New COL_Persona
		Dim oDataset As DataSet
		Dim i, totale, totaleHistory As Integer
		Dim Path, img As String
		oPersona = Session("objPersona")


		Try
			Dim oTreeComunita As New COL_TreeComunita

			oDataset = Me.FiltraggioDati(ApplicaFiltri)
			Me.CBXmostraPadre.Enabled = False
			totale = oDataset.Tables(0).Rows.Count
			If totale = 0 Then 'se datagrid vuota
				' al posto della datagrid mostro un messaggio!
				Me.DGcomunita.Visible = False
				Me.LBmsgDG.Visible = True
				oResource.setLabel_To_Value(Me.LBmsgDG, "elenco." & StringaElenco.noCommunityForFilter)

				Me.LBnumeroRecord_c.Visible = False
				Me.DDLNumeroRecord.Visible = False
				Me.DGcomunita.PagerStyle.Position = PagerPosition.Top
				Me.PNLmenu.Visible = False
				Me.LNBiscriviMultipli.Enabled = False
			Else
				Me.PNLmenu.Visible = True
				totale = oDataset.Tables(0).Rows.Count

				If totale <= Me.DDLNumeroRecord.Items(0).Value Then
					Me.LBnumeroRecord_c.Visible = False
					Me.DDLNumeroRecord.Visible = False
					Me.DGcomunita.PagerStyle.Position = PagerPosition.Top
				Else
					Me.LBnumeroRecord_c.Visible = True
					Me.DDLNumeroRecord.Visible = True
					Me.DGcomunita.PagerStyle.Position = PagerPosition.TopAndBottom
				End If
				If totale > 0 Then
					Me.CBXmostraPadre.Enabled = True
					Me.DGcomunita.Columns(6).Visible = False
					Me.DGcomunita.Columns(2).Visible = False
					Me.DGcomunita.Columns(3).Visible = False
					Me.DGcomunita.Columns(28).Visible = True

					Dim oDataview As DataView
					oDataview = oDataset.Tables(0).DefaultView
					If ViewState("SortExspression") = "" Then
						ViewState("SortExspression") = "ALCM_Livello,CMNT_Nome"
						ViewState("SortDirection") = "asc"
					End If
					Me.LNBiscriviMultipli.Enabled = True
					oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")
					DGcomunita.DataSource = oDataview
					DGcomunita.DataBind()
				Else
					Me.LNBiscriviMultipli.Enabled = False
					Me.DGcomunita.Visible = False
					Me.LBmsgDG.Visible = True
					oResource.setLabel_To_Value(Me.LBmsgDG, "elenco." & StringaElenco.noCommunityForFilter)
				End If
			End If
		Catch ex As Exception
			Me.DGcomunita.Visible = False
			Me.LBmsgDG.Visible = True
			Me.LBnumeroRecord_c.Visible = False
			Me.DDLNumeroRecord.Visible = False
			Me.LNBiscriviMultipli.Enabled = False
			oResource.setLabel_To_Value(Me.LBmsgDG, "elenco." & StringaElenco.noCommunityForFilter)
		End Try
	End Sub

#End Region

#Region "Setup Parametri Ricerca"
	Private Sub SaveSearchParameters() 'ByVal visualizzazione As Integer)
		Try
			Me.Response.Cookies("RicercaComunitaUtente")("DDLNumeroRecord") = Me.DDLNumeroRecord.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("DDLTipoRicerca") = Me.DDLTipoRicerca.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("TXBValore") = Me.TXBValore.Text
			Me.Response.Cookies("RicercaComunitaUtente")("intCurPage") = Me.ViewState("intCurPage")
			Me.Response.Cookies("RicercaComunitaUtente")("intAnagrafica") = Me.ViewState("intAnagrafica")
			Me.Response.Cookies("RicercaComunitaUtente")("SortDirection") = Me.ViewState("SortDirection")
			Me.Response.Cookies("RicercaComunitaUtente")("SortExspression") = Me.ViewState("SortExspression")
			Me.Response.Cookies("RicercaComunitaUtente")("CBXmostraPadre") = Me.CBXmostraPadre.Checked
			Me.Response.Cookies("RicercaComunitaUtente")("TBRapriFiltro") = Me.TBRapriFiltro.Visible
			Me.Response.Cookies("RicercaComunitaUtente")("CBXautoUpdate") = Me.CBXautoUpdate.Checked
			Me.Response.Cookies("RicercaComunitaUtente")("DDLresponsabile") = Me.DDLresponsabile.SelectedValue
		Catch ex As Exception

		End Try
	End Sub
	Private Sub SetupSearchParameters()
		Try
			'Recupero fattori di ricerca relativi all'ordinamento
			Try
				Me.ViewState("SortDirection") = Me.Request.Cookies("RicercaComunitaUtente")("SortDirection")
				Me.ViewState("SortExspression") = Me.Request.Cookies("RicercaComunitaUtente")("SortExspression")
			Catch ex As Exception

			End Try

			Try
				Me.TBRapriFiltro.Visible = Me.Request.Cookies("RicercaComunitaUtente")("TBRapriFiltro")
				Me.TBRfiltri.Visible = Not Me.TBRapriFiltro.Visible
				Me.TBRchiudiFiltro.Visible = Not Me.TBRapriFiltro.Visible
			Catch ex As Exception
				Me.TBRapriFiltro.Visible = False
				Me.TBRchiudiFiltro.Visible = True
				Me.TBRfiltri.Visible = True
			End Try

			Try
				Me.CBXautoUpdate.Checked = Me.Response.Cookies("RicercaComunitaUtente")("CBXautoUpdate")
			Catch ex As Exception
				Me.CBXautoUpdate.Checked = True
			End Try

			Try
				'Recupero dati relativi alla paginazione corrente
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("intCurPage")) Then
					Me.ViewState("intCurPage") = CInt(Me.Request.Cookies("RicercaComunitaUtente")("intCurPage"))
					Me.DGcomunita.CurrentPageIndex = CInt(Me.ViewState("intCurPage"))
				Else
					Me.ViewState("intCurPage") = 0
					Me.DGcomunita.CurrentPageIndex = 0
				End If
			Catch ex As Exception
				Me.ViewState("intCurPage") = 0
				Me.DGcomunita.CurrentPageIndex = 0
			End Try
			Try
				Me.TXBValore.Text = Me.Request.Cookies("RicercaComunitaUtente")("TXBValore")
			Catch ex As Exception
				Me.TXBValore.Text = ""
			End Try

			'Vedo se selezionare qualche linkbutton !!
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("intAnagrafica")) Then
					Dim oFiltro As Main.FiltroComunita
					Dim Lettera As String
					Lettera = CType(CInt(Me.Request.Cookies("RicercaComunitaUtente")("intAnagrafica")), Main.FiltroComunita).ToString

					Dim oLink As System.Web.UI.WebControls.LinkButton
                    oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
					If IsNothing(oLink) = False Then
						oLink.CssClass = "lettera_Selezionata"
						Me.ViewState("intAnagrafica") = CInt(Me.Request.Cookies("RicercaComunitaUtente")("intAnagrafica"))
					Else
						Me.ViewState("intAnagrafica") = -1
						Me.LKBtutti.CssClass = "lettera_Selezionata"
					End If
				Else
					Me.ViewState("intAnagrafica") = -1
					Me.LKBtutti.CssClass = "lettera_Selezionata"
				End If

			Catch ex As Exception
				Me.ViewState("intAnagrafica") = -1
				Me.LKBtutti.CssClass = "lettera_Selezionata"
			End Try

			' Setto l'organizzazione
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")) Then
					Try
						Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")
					Catch ex As Exception

					End Try
				End If
			Catch ex As Exception

			End Try

			' Setto il numero di record
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLNumeroRecord")) Then
					Me.DDLNumeroRecord.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLNumeroRecord")
				End If
			Catch ex As Exception

			End Try

			' Setto il tipo di ricerca
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLTipoRicerca")) Then
					Me.DDLTipoRicerca.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLTipoRicerca")
				End If
			Catch ex As Exception
			End Try

			Try
				Me.CBXmostraPadre.Checked = CBool(Me.Request.Cookies("RicercaComunitaUtente")("CBXmostraPadre"))
			Catch ex As Exception
				Me.CBXmostraPadre.Checked = False
			End Try
		Catch ex As Exception

		End Try
	End Sub
#End Region

#Region "Filtro"
	Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
		Me.TBRfiltri.Visible = True
		Me.TBRchiudiFiltro.Visible = True
		Me.TBRapriFiltro.Visible = False
		Me.Bind_Griglia()
	End Sub
	Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
		Me.TBRfiltri.Visible = False
		Me.TBRchiudiFiltro.Visible = False
		Me.TBRapriFiltro.Visible = True
		Me.Bind_Griglia()
	End Sub
	Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
		Me.Bind_Responsabili()
		Me.Bind_Griglia()
	End Sub

	Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
		Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked

		Me.HDNcomunitaSelezionate.Value = ""
		Me.Bind_Griglia(True)
	End Sub
	Private Sub DDLresponsabile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLresponsabile.SelectedIndexChanged
		If Me.CBXautoUpdate.Checked Then
			DGcomunita.PageSize = DDLNumeroRecord.SelectedItem.Value
			DGcomunita.CurrentPageIndex = 0
			Me.ViewState("intCurPage") = 0
			Me.HDNcomunitaSelezionate.Value = ""
			Me.Bind_Griglia(True)
		End If
	End Sub
	Private Sub DDLTipoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRicerca.SelectedIndexChanged
		If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
			Me.DDLresponsabile.Visible = True
			Me.TXBValore.Text = ""
			Me.TXBValore.Visible = False
		Else
			Me.DDLresponsabile.Visible = False
			Me.TXBValore.Visible = True
		End If
		If Me.CBXautoUpdate.Checked Then
			Me.HDNcomunitaSelezionate.Value = ""
		End If
		Me.Bind_Griglia()
	End Sub

	Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
		Me.ViewState("intCurPage") = 0
		Me.DDLNumeroRecord.SelectedIndex = Me.DDLNumeroRecord.SelectedIndex
		DGcomunita.PageSize = DDLNumeroRecord.SelectedItem.Value
		DGcomunita.CurrentPageIndex = 0
		Bind_Griglia(True)
	End Sub

	Private Sub CBXmostraPadre_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXmostraPadre.CheckedChanged
		If Me.DDLNumeroRecord.SelectedValue <> Me.DGcomunita.PageSize Then
			DGcomunita.PageSize = DDLNumeroRecord.SelectedItem.Value
			DGcomunita.CurrentPageIndex = 0
			Me.ViewState("intCurPage") = 0
		End If
		Me.Bind_Griglia()
	End Sub

	Private Sub BTNCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
		DGcomunita.PageSize = DDLNumeroRecord.SelectedItem.Value
		DGcomunita.CurrentPageIndex = 0
		Me.ViewState("intCurPage") = 0
		Me.HDNcomunitaSelezionate.Value = ""
		Me.Bind_Griglia(True)
	End Sub

	Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
		If sender.commandArgument <> "" Then
			Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
			Me.ViewState("intAnagrafica") = sender.commandArgument
			sender.CssClass = "lettera_Selezionata"
		Else
			Me.ViewState("intAnagrafica") = -1
			Me.LKBtutti.CssClass = "lettera_Selezionata"
		End If
		Me.ViewState("intCurPage") = 0
		Me.DGcomunita.CurrentPageIndex = 0
		Me.Bind_Griglia()
	End Sub
	Private Sub DeselezionaLink(ByVal Lettera As String)
		Dim oFiltro As Main.FiltroComunita
		Lettera = CType(CInt(Lettera), Main.FiltroComunita).ToString

		Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
		If IsNothing(oLink) = False Then
			oLink.CssClass = "lettera"
		End If
	End Sub
	Private Sub SelezionaLink_All()
		Dim i As Integer
		Try
			For i = Asc("a") To Asc("z") 'status dei link button delle lettere
				Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
				Dim Carattere As String = Chr(i)
				If IsNothing(oLinkButton) = False Then
					oLinkButton.CssClass = "lettera"
				End If
			Next
			Me.LKBaltro.CssClass = "lettera"
			Me.ViewState("intAnagrafica") = -1
			Me.LKBtutti.CssClass = "lettera_Selezionata"
		Catch ex As Exception

		End Try
	End Sub
#End Region

#Region "Gestione Griglia"
	Private Sub DGcomunita_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DGcomunita.SortCommand
		Dim oSortExpression, oSortDirection As String
		oSortExpression = ViewState("SortExspression")
		oSortDirection = ViewState("SortDirection")
		ViewState("SortExspression") = e.SortExpression

		If e.SortExpression = oSortExpression Then

			If ViewState("SortDirection") = "asc" Then
				ViewState("SortDirection") = "desc"
			Else
				ViewState("SortDirection") = "asc"
			End If
		Else
			ViewState("SortDirection") = "asc"
		End If
		Me.Bind_Griglia()
	End Sub
	Private Sub DGComunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGcomunita.ItemCreated
		Dim i As Integer

		If IsNothing(oResource) Then
			Me.SetCulture(Session("LinguaCode"))
		End If
		If e.Item.ItemType = ListItemType.Header Then
			Dim oSortExspression, oSortDirection, oText, StringaMouse As String
			oSortExspression = ViewState("SortExspression")
			oSortDirection = ViewState("SortDirection")


			For i = 0 To sender.columns.count - 1
				If sender.columns(i).SortExpression <> "" Then
					Dim oWebControl As WebControl
					Dim oCell As New TableCell
					Dim oLabelAfter As New System.Web.UI.WebControls.Label
					Dim oLabelBefore As New System.Web.UI.WebControls.Label

					oLabelBefore.Font.Name = "webdings"
					oLabelBefore.Font.Size = FontUnit.XSmall
					oLabelBefore.Text = "&nbsp;"

					oCell = e.Item.Cells(i)
					If Me.DGcomunita.Columns(i).SortExpression <> "" Then
						If oSortExspression = sender.columns(i).SortExpression Then
							Try
								oWebControl = oCell.Controls(0)
								Dim oLinkbutton As LinkButton
								oLinkbutton = oWebControl
								oLinkbutton.CssClass = "ROW_HeaderLink_Small"

								oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
								If oSortDirection = "asc" Then
									oResource.setHeaderOrderbyLink_Datagrid(Me.DGcomunita, oLinkbutton, FiltroOrdinamento.Decrescente)
								Else
									oResource.setHeaderOrderbyLink_Datagrid(Me.DGcomunita, oLinkbutton, FiltroOrdinamento.Crescente)
								End If
								oLabelAfter.CssClass = Me.DGcomunita.HeaderStyle.CssClass
								oLabelAfter.Text = oLinkbutton.Text & " "
								'oLinkbutton.Font.Name = "webdings"
								'oLinkbutton.Font.Size = FontUnit.XSmall

								If oSortDirection = "asc" Then
									'  oText = "5"
									oText = "<img src='./../images/dg/down.gif' id='Image_" & i & "' >"
									If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
									Else
										StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
										oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
									End If
									If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
									Else
										StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
										oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
									End If
								Else
									'  oText = "6"
									oText = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
									If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
									Else
										StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
										oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
									End If
									If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
									Else
										StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
										oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
									End If
								End If
								oLinkbutton.Text = oText


								oCell.Controls.AddAt(0, oLabelAfter)
							Catch ex As Exception
								oCell.Controls.AddAt(0, oLabelAfter)
							End Try
						Else
							Try
								oWebControl = oCell.Controls(0)
								Dim oLinkbutton As LinkButton
								oLinkbutton = oWebControl
								oLinkbutton.CssClass = "ROW_HeaderLink_Small"

								oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
								oResource.setHeaderOrderbyLink_Datagrid(Me.DGcomunita, oLinkbutton, FiltroOrdinamento.Crescente)
								oLabelAfter.CssClass = Me.DGcomunita.HeaderStyle.CssClass
								oLabelAfter.Text = oLinkbutton.Text & " "
								'oLinkbutton.Font.Name = "webdings"
								'oLinkbutton.Font.Size = FontUnit.XSmall
								oLinkbutton.Text = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
								If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
								Else
									StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
									oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
								End If
								If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
								Else
									StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
									oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
								End If

								oCell.Controls.AddAt(0, oLabelAfter)
							Catch ex As Exception
								oCell.Controls.AddAt(0, oLabelAfter)
							End Try
						End If
					End If
				End If
			Next
		End If
		If e.Item.ItemType = ListItemType.Pager Then
			Dim oCell As TableCell
			Dim n As Integer
			oCell = CType(e.Item.Controls(0), TableCell)

			n = oCell.ColumnSpan
			' Aggiungo riga con descrizione:

			Try
				Dim oRow As TableRow
				Dim oTableCell As New TableCell
				Dim num As Integer = 0
				oRow = oCell.Parent()

				oTableCell.Controls.Add(Me.CreaLegenda)
				If Me.DGcomunita.Columns(2).Visible Then
					num += 1
				End If
				If Me.DGcomunita.Columns(3).Visible Then
					num += 1
				End If
				If Me.DGcomunita.Columns(6).Visible Then
					num += 1
				End If
				num += 2
				oTableCell.ColumnSpan = num
				oTableCell.HorizontalAlign = HorizontalAlign.Left
				oCell.ColumnSpan = 2
				oRow.Cells.AddAt(0, oTableCell)
				e.Item.Cells(0).Attributes.Item("colspan") = num.ToString
			Catch ex As Exception

			End Try


			For n = 0 To oCell.Controls.Count - 1 Step 2
				Dim szLnk As String
				szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
				Dim oWebControl As WebControl

				oWebControl = oCell.Controls(n)

				If (oWebControl.GetType().ToString() = szLnk) Then
					oWebControl.CssClass = "ROW_PagerLink_Small"
				End If
				Try
					Dim oLabel As Label
					oLabel = oWebControl
					oLabel.Text = oLabel.Text
					oLabel.CssClass = "ROW_PagerSpan_Small"
				Catch ex As Exception
					Dim oLinkbutton As LinkButton
					oLinkbutton = oWebControl
					oLinkbutton.CssClass = "ROW_PagerLink_Small"
					oResource.setPageDatagrid(Me.DGcomunita, oLinkbutton)
				End Try
			Next
		End If
		If (e.Item.ItemType = ListItemType.Footer) Then
			e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count
			For i = 1 To e.Item.Cells.Count - 1
				e.Item.Cells.RemoveAt(1)
			Next
		End If

		If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
			Try
				Dim cssLink As String = "ROW_ItemLink_Small"
				Dim cssRiga As String = "ROW_TD_Small"
				Dim hasIscrizione As Boolean = False


				Dim oLNBiscrivi, oLNBdettagli As LinkButton


				oLNBdettagli = e.Item.Cells(1).FindControl("LNBdettagli")
				oLNBiscrivi = e.Item.Cells(1).FindControl("LNBiscrivi")
				oLNBiscrivi.CssClass = cssLink
				oLNBdettagli.CssClass = cssLink

				Dim oCell As New TableCell
				Dim CMNT_Nome As String

				Try
					CMNT_Nome = e.Item.DataItem("CMNT_Nome")
					If CMNT_Nome <> "" Then
						CMNT_Nome = ": " & Replace(CMNT_Nome, "'", "\'")
					End If
				Catch ex As Exception
					CMNT_Nome = ""
				End Try


				Try
					Dim oTBRnome As TableRow
					Dim oTBCchiusa, oTBCnome As TableCell

					oTBRnome = e.Item.Cells(1).FindControl("TBRnome")
					oTBCchiusa = e.Item.Cells(1).FindControl("TBCchiusa")
					oTBCnome = e.Item.Cells(1).FindControl("TBCnome")

					If IsNothing(oTBRnome) = False Then
						oTBRnome.CssClass = cssRiga
					End If
					If IsNothing(oTBCchiusa) = False Then
						oTBCchiusa.CssClass = cssRiga
					End If
					If IsNothing(oTBCnome) = False Then
						oTBCnome.CssClass = cssRiga
					End If
				Catch ex As Exception

				End Try

				Try
					Dim oIMGisChiusa As System.Web.UI.WebControls.Image
					oIMGisChiusa = e.Item.Cells(1).FindControl("IMGisChiusa")

					If IsNothing(oIMGisChiusa) = False Then
						Dim ImageBaseDir As String
						ImageBaseDir = GetPercorsoApplicazione(Me.Request)
						ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/"

						oIMGisChiusa.Visible = True
						oIMGisChiusa.ImageUrl = ImageBaseDir & oResource.getValue("stato.image." & e.Item.DataItem("CMNT_isChiusa"))
						oIMGisChiusa.AlternateText = oResource.getValue("stato." & e.Item.DataItem("CMNT_isChiusa"))
					End If
				Catch ex As Exception

				End Try

				Try
					Dim oLBcomunitaNome As Label
					oLBcomunitaNome = e.Item.Cells(1).FindControl("LBcomunitaNome")
					If IsNothing(oLBcomunitaNome) = False Then
						oLBcomunitaNome.CssClass = cssRiga
					End If
				Catch ex As Exception

				End Try
				Try
					If IsNothing(oLNBdettagli) = False Then
						oResource.setLinkButton(oLNBdettagli, True, True)
						oLNBdettagli.CssClass = cssLink
					End If
				Catch ex As Exception

				End Try

				'' SISTEMARE
				Dim CanSubscribe As Boolean = True
				Try
					Dim oPersona As New COL_Persona
					' Link iscrizione comunità
					If IsDBNull(e.Item.DataItem("RLPC_TPRL_ID")) Then
						oLNBiscrivi.Visible = True
					Else
						If e.Item.DataItem("RLPC_TPRL_ID") > 0 Then
							oLNBiscrivi.Visible = False
							CanSubscribe = False
						Else
							oLNBiscrivi.Visible = True
						End If
					End If

				
					oPersona = Session("objPersona")

					If e.Item.DataItem("CMNT_CanSubscribe") Then
						If oLNBiscrivi.Visible = True Then
							If IsNothing(oLNBiscrivi) = False Then
								oResource.setLinkButton(oLNBiscrivi, True, False)
							End If


							' iscrizione alla comunità
							Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime
							If IsDate(e.Item.DataItem("CMNT_dataInizioIscrizione")) Then
								CMNT_dataInizioIscrizione = e.Item.DataItem("CMNT_dataInizioIscrizione")
							End If
							If IsDate(e.Item.DataItem("CMNT_dataFineIscrizione")) Then
								Dim DataTemp As DateTime
								CMNT_dataFineIscrizione = e.Item.DataItem("CMNT_dataFineIscrizione")
								DataTemp = CMNT_dataFineIscrizione.Date()
								DataTemp = DataTemp.AddHours(23)
								DataTemp = DataTemp.AddMinutes(59)
								CMNT_dataFineIscrizione = DataTemp
							End If
							If CMNT_dataInizioIscrizione > Now Then
								' devo iscrivermi, ma iscrizioni non aperte !
								CMNT_Nome = CMNT_Nome = oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniAperteIl)
								CMNT_Nome = CMNT_Nome.Replace("#%%#", CMNT_dataInizioIscrizione)
								oLNBiscrivi.Enabled = False
								CanSubscribe = False
							Else
								If IsDate(CMNT_dataFineIscrizione) Then
									If CMNT_dataFineIscrizione < Now And Not Equals(New Date, CMNT_dataFineIscrizione) Then
										oLNBiscrivi.Text = oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniChiuse)
										If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Copisteria Then
											oLNBiscrivi.Enabled = True
										Else
											oLNBiscrivi.Enabled = False
											CanSubscribe = False
										End If
									Else
										oLNBiscrivi.Enabled = True
									End If
								Else
									oLNBiscrivi.Enabled = True
								End If
							End If


							' se il numero iscritti è stato superato chiudo la possibilità di iscrivere !!!

							If e.Item.DataItem("CMNT_Iscritti") >= 0 And oLNBiscrivi.Enabled Then
								oLNBiscrivi.Enabled = True
							Else
								If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Copisteria And oLNBiscrivi.Enabled = True Then
									oLNBiscrivi.Enabled = True
								Else
									oLNBiscrivi.Enabled = False
									CanSubscribe = False
								End If
							End If
						End If
					Else
						If IsNothing(oLNBiscrivi) = False Then
							oResource.setLinkButton(oLNBiscrivi, True, False)
							oLNBiscrivi.Enabled = False
							CanSubscribe = False
						End If
					End If
					If oLNBiscrivi.Enabled Then
						If e.Item.DataItem("CMNT_Bloccata") Or e.Item.DataItem("CMNT_Archiviata") Then
							oLNBiscrivi.Enabled = False
							CanSubscribe = False
						End If
					End If


				Catch ex As Exception

				End Try

				If CanSubscribe Then
					Me.LNBiscriviMultipli.Enabled = True
				End If
				Try
					Dim oCheckbox As System.Web.UI.HtmlControls.HtmlInputCheckBox
					oCheckbox = e.Item.Cells(25).FindControl("CBcorso")
					If Not IsNothing(oCheckbox) Then
						Try
							oCheckbox.Visible = CanSubscribe
							If InStr(Me.HDNcomunitaSelezionate.Value, "," & e.Item.DataItem("CMNT_ID") & ",") > 0 Then
								If oCheckbox.Visible Then
									oCheckbox.Checked = True
								Else
									oCheckbox.Checked = False
									Me.HDNcomunitaSelezionate.Value = Replace(Me.HDNcomunitaSelezionate.Value, "," & e.Item.DataItem("CMNT_ID") & ",", ",")
								End If
							End If
							oCheckbox.Value = e.Item.DataItem("CMNT_ID")
						Catch ex As Exception

						End Try
					End If
				Catch ex As Exception

				End Try
			Catch ex As Exception

			End Try
		End If
	End Sub
	Sub DGComunita_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGcomunita.PageIndexChanged
		Me.ViewState("intCurPage") = e.NewPageIndex
		DGcomunita.CurrentPageIndex = e.NewPageIndex
		Me.Bind_Griglia()
	End Sub
	Private Sub DGComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGcomunita.ItemCommand
		If e.CommandName = "Iscrivi" Or e.CommandName = "dettagli" Or e.CommandName = "Login" Or e.CommandName = "legginews" Then

			Dim oComunita As New COL_Comunita
			Dim oPersona As New COL_Persona
			Dim isAttivoForIscrizione As Boolean = True
			Dim ComunitaID, PersonaID As Integer
			Dim ComunitaPath As String
			Dim isChiusaForPadre As Boolean

			ComunitaID = CInt(DGcomunita.DataKeys.Item(e.Item.ItemIndex))
			ComunitaPath = DGcomunita.Items(e.Item.ItemIndex).Cells(11).Text()
			isChiusaForPadre = CBool(e.Item.Cells(20).Text)
			oComunita.Id = ComunitaID
			oComunita.Estrai()
			oPersona = Session("objPersona")
			PersonaID = oPersona.ID

			If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Copisteria Then
				isAttivoForIscrizione = oComunita.HasAccessoCopisteria
			End If
			Select Case e.CommandName
				Case "Login"
					Me.EntraComunita(ComunitaID, ComunitaPath)
				Case "Iscrivi"
					'richiamo la sub che effettua l'iscrizione della persona

					Dim oImpostazioni As New COL_ImpostazioniUtente
					Dim exitSub As Boolean = False
					Try
						oImpostazioni = Session("oImpostazioni")
						exitSub = Not oImpostazioni.ShowConferma
					Catch ex As Exception
						exitSub = False
					End Try

					Me.ResetFormAll()
					If isAttivoForIscrizione And (oComunita.CanSubscribe And oComunita.Archiviata = False And oComunita.Bloccata = False) Then
						Me.PageUtility.AddAction(ActionType.SubscribeCommunity, Me.PageUtility.CreateObjectsList(ObjectType.Community, ComunitaID))

						If Not exitSub Then
							Session("azione") = "iscrivi"
							Me.HDNcmnt_ID.Value = ComunitaID
							Me.HDisChiusa.Value = isChiusaForPadre
							Me.HDNcmnt_Path.Value = ComunitaPath
							Me.ResetFormToConferma(False, e.Item.Cells(25).Text, e.Item.Cells(5).Text)
						Else
							If Session("azione") <> "iscrivi" Then
								Dim iResponse As Main.ErroriIscrizioneComunita
								Dim oUtility As New OLDpageUtility(Me.Context)

								Session("azione") = "iscrivi"
                                iResponse = oPersona.IscrizioneComunitaNew(ComunitaID, ComunitaPath, isChiusaForPadre, oUtility.BaseUrlDrivePath & "profili\" & PersonaID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                                If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                                    Me.Reset_ToMessaggi()
                                    Me.LBMessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                                Else
                                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                                        oServiceUtility.NotifyAddSelfSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Nome)
                                    Else
                                        oServiceUtility.NotifyAddWaitingSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Nome)
                                    End If
                                    Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                                    Me.Reset_ToIscrizioneAvvenuta()
                                End If
                            Else
                                Session("azione") = "loaded"
                                Me.Reset_Contenuto(True, True)
                            End If
                        End If
                    Else
                        Dim alertMSG As String = ""
                        If Not isAttivoForIscrizione Then
                            alertMSG = oResource.getValue("messaggio.BloccataForCopisteria")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità bloccata !"
                            End If
                        ElseIf Not oComunita.Bloccata Then
                            alertMSG = oResource.getValue("messaggio.Bloccata")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità bloccata !"
                            End If
                        ElseIf Not oComunita.Archiviata Then
                            alertMSG = oResource.getValue("messaggio.Archiviata")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità archiviata !"
                            End If
                        Else
                            alertMSG = oResource.getValue("messaggio.NoIscrizione")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi alla comunità selezionata !"
                            End If
                        End If

                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")

                        Session("azione") = "loaded"
                        Me.Reset_Contenuto(True, True)
                    End If

                Case "dettagli"
                    Me.PageUtility.AddAction(ActionType.Details, Me.PageUtility.CreateObjectsList(ObjectType.Community, ComunitaID))

                    Dim oRuoloComunita As New COL_RuoloPersonaComunita
                    Me.LNBentraDettagli.Visible = False
                    Me.LNBiscriviDettagli.Visible = False
                    Try
                        oRuoloComunita.Estrai(ComunitaID, PersonaID)
                        If oRuoloComunita.Errore = Errori_Db.None Then
                            If oRuoloComunita.TipoRuolo.Id > -1 Then
                                Me.LNBiscriviDettagli.Visible = False
                            Else
                                Me.LNBiscriviDettagli.Visible = True
                                Me.LNBiscriviDettagli.Enabled = True
                            End If
                        Else
                            Me.LNBiscriviDettagli.Visible = True
                            Me.LNBiscriviDettagli.Enabled = True
                        End If
                        If Me.LNBiscriviDettagli.Enabled = True Then
                            If oComunita.DataInizioIscrizione > Now Then
                                Me.LNBiscriviDettagli.Visible = False
                            Else
                                If Not Equals(New Date, oComunita.DataFineIscrizione) Then
                                    Dim DataTemp As DateTime
                                    DataTemp = oComunita.DataFineIscrizione.Date
                                    DataTemp = DataTemp.AddHours(23)
                                    DataTemp = DataTemp.AddMinutes(59)
                                    If DataTemp < Now Then
                                        Me.LNBiscriviDettagli.Visible = False
                                    End If
                                End If
                            End If
                        End If

                        If oComunita.Bloccata Or oComunita.Archiviata Or Not oComunita.CanSubscribe Or Not isAttivoForIscrizione Then
                            Me.LNBiscriviDettagli.Enabled = False
                        End If
                    Catch ex As Exception
                        Me.LNBiscriviDettagli.Visible = False
                    End Try

                    Me.HDNisChiusaForPadre.Value = isChiusaForPadre
                    Me.HDNcmnt_Path.Value = ComunitaPath
                    Me.HDNcmnt_ID.Value = ComunitaID
                    Me.VisualizzaDettagli(ComunitaID)
                Case "legginews"
                    Me.SaveSearchParameters()
                    Session("CMNT_path_forNews") = ComunitaPath
                    Session("CMNT_ID_forNews") = ComunitaID
                    Me.Response.Redirect("./../generici/News_Comunita.aspx?from=RicercaComunita", True)
            End Select
        End If
    End Sub

    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If


        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("ISCRIZIONIdisattivate")


        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("ISCRIZIONInonPossibili")

        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function
#End Region

#Region "Dettagli"
    Private Sub LNBiscriviDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviDettagli.Click
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Dim exitSub As Boolean = False
        Try
            oImpostazioni = Session("oImpostazioni")
            exitSub = Not oImpostazioni.ShowConferma
        Catch ex As Exception
            exitSub = False
        End Try
        Me.PNLdettagli.Visible = False

        If Not exitSub Then
            Session("azione") = "iscrivi"
            Dim oComunita As New COL_Comunita
            oComunita.Id = Me.HDNcmnt_ID.Value

            Me.HDisChiusa.Value = CBool(Me.HDNisChiusaForPadre.Value)
            Me.ResetFormToConferma(False, oComunita.EstraiNomeBylingua(Session("linguaID")), oComunita.GetNomeResponsabile_NomeCreatore())
        Else
            If Session("azione") <> "iscrivi" Then
                Session("azione") = "iscrivi"
                Try
                    Dim iResponse As Main.ErroriIscrizioneComunita
                    Dim oUtility As New OLDpageUtility(Me.Context)
                    Dim oPersona As New COL_Persona

                    oPersona = Session("objPersona")

                    iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, Me.HDisChiusa.Value, oUtility.BaseUrlDrivePath & "profili/" & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                    If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                        Me.Reset_ToMessaggi()
                        Me.LBMessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Else
                        Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                        If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                            oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        Else
                            oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        End If
                        Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                        Me.Reset_ToIscrizioneAvvenuta()
                    End If
                    Me.HDNcmnt_ID.Value = ""
                    Me.HDisChiusa.Value = ""
                    Me.HDNcmnt_Path.Value = ""
                Catch ex As Exception
                    Session("azione") = "loaded"
                    Me.Reset_Contenuto(True, True)
                End Try
            Else
                Session("azione") = "loaded"
                Me.Reset_Contenuto(True, True)
            End If
        End If
    End Sub
    Private Sub LNBannullaDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDettagli.Click
        Session("azione") = "loaded"
        Me.Reset_Contenuto(True, True)
    End Sub
    Private Sub LNBentraDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBentraDettagli.Click
        Me.EntraComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value)
    End Sub
    Private Sub VisualizzaDettagli(ByVal CMNT_Id As Integer)
        Try
            Reset_ToDettagli()
            Me.CTRLDettagli.SetupDettagliComunita(CMNT_Id)
        Catch ex As Exception
            Me.Reset_Contenuto(True, True)
        End Try
    End Sub
#End Region

#Region "Entrata Comunita"
    Private Sub EntraComunita(ByVal CMNT_ID As Integer, ByVal CMNT_path As String)
        'Dim oResourceConfig As New ResourceManager
        'oResourceConfig = GetResourceConfig(Session("LinguaCode"))
        'Dim status As lm.Comol.Core.DomainModel.SubscriptionStatus
        'Dim idPerson As Integer = PageUtility.CurrentUser.ID
        'status = PageUtility.AccessToCommunity(idPerson, idCommunity, path, oResourceConfig, True)

        'Dim oTreeComunita As New COL_TreeComunita
        'Try
        '    oTreeComunita.Directory = Server.MapPath(PageUtility.BaseUrl & "profili/") & idPerson & "\"
        '    oTreeComunita.Nome = idPerson & ".xml"
        'Catch ex As Exception

        'End Try
        'Select Case status
        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.activemember
        '        Exit Sub
        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked
        '        Me.Reset_ToMessaggi()
        '        oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & Me.StringaAbilitato.bloccato)
        '        oTreeComunita.CambiaAttivazione(idCommunity, False, oResource)
        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
        '        Me.Reset_ToMessaggi()
        '        oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & Me.StringaAbilitato.inAttesa)
        '        oTreeComunita.CambiaAbilitazione(idCommunity, False)

        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
        '        Me.Reset_ToMessaggi()
        '        oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & Me.StringaAbilitato.comunitaBloccata)
        '        oTreeComunita.CambiaAbilitazione(idCommunity, False)
        '        oTreeComunita.CambiaIsBloccata(idCommunity, True)
        '    Case Else
        '        oTreeComunita.Delete(idCommunity, path)
        'End Select
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim PRSN_ID, RuoloID As Integer

        Try
            oPersona = Session("objPersona")
            PRSN_ID = oPersona.ID

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"
        Catch ex As Exception

        End Try

        Try
            Dim oRuolo As New COL_RuoloPersonaComunita
            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
            If oRuolo.Errore = Errori_Db.None Then
                RuoloID = oRuolo.TipoRuolo.Id
            End If

            'verifico se l'utente ha l'abilitazione per fare l'accesso alla comunità

            Dim oComunita As New COL_Comunita

            oComunita.Id = CMNT_ID
            oComunita.Estrai()
            GenericCacheManager.PurgeCacheItems(CachePolicy.PermessiServizioUtente())
            If oComunita.Errore = Errori_Db.None Then
                oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona

                If oRuolo.Abilitato And oRuolo.Attivato Then 'se l'utente è attivato E abilitato allora
                    ' metto in sessione i permessi che l'utente ha per quella comunità
                    Me.PageUtility.AddAction(ActionType.EnterCommunity, Me.PageUtility.CreateObjectsList(ObjectType.Community, CMNT_ID))
                    Dim IdRuolo, i, j, dimensione, dimeArrCmnt, ORGN_ID As Integer
                    Session("IdRuolo") = RuoloID
                    Session("IdComunita") = CMNT_ID


                    Dim Elenco_CMNT_ID() As String
                    Elenco_CMNT_ID = CMNT_Path.Split(".")

                    Dim totale As Integer
                    Dim ArrComunita(,) As String

                    With oComunita
                        Session("ORGN_id") = .Organizzazione.Id
                        Try

                            Dim oServizio As New COL_Servizio
                            Dim oDataSet As New DataSet
                            oDataSet = oServizio.ElencaByTipoRuoloByComunita(Session("IdRuolo"), CMNT_ID)
                            totale = oDataSet.Tables(0).Rows.Count - 1

                            Dim ArrPermessi(totale, 2) As String
                            For i = 0 To totale
                                Dim oRow As DataRow
                                oRow = oDataSet.Tables(0).Rows(i)
                                ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
                                ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
                                ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
                            Next
                            Session("ArrPermessi") = ArrPermessi
                        Catch ex As Exception

                        End Try

                        Try
                            If Session("LogonAs") = False Then
                                oRuolo.UpdateUltimocollegamento()
                            End If
                        Catch ex As Exception

                        End Try

                        'Aggiorno gli array relativi al menu history !!!


                        Dim tempArray(,), Path As String
                        j = 0
                        For i = 0 To UBound(Elenco_CMNT_ID) - 1

                            If IsNumeric(Elenco_CMNT_ID(i)) Then
                                If Elenco_CMNT_ID(i) > 0 Then
                                    ReDim Preserve ArrComunita(3, j)
                                    ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                    ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                    If Path = "" Then
                                        Path = "." & Elenco_CMNT_ID(i) & "."
                                    Else
                                        Path = Path & Elenco_CMNT_ID(i) & "."
                                    End If
                                    ArrComunita(2, j) = Path
                                    ' Ruolo svolto..........
                                    ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                    j = j + 1
                                End If
                            End If
                        Next

                        Session("ArrComunita") = ArrComunita
                        Session("limbo") = False

                    End With

                    Session("RLPC_ID") = oRuolo.Id

                    oComunita.RegistraAccesso(CMNT_ID, PRSN_ID, Me.PageUtility.SystemSettings.CodiceDB)
                    Me.PageUtility.SendNotificationUpdateCommunityAccess(PRSN_ID, CMNT_ID, oRuolo.UltimoCollegamento)
                    oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)

                    Session("AdminForChange") = False
                    Session("CMNT_path_forAdmin") = ""
                    Session("idComunita_forAdmin") = ""
                    ' REGISTRAZIONE EVENTO
                    Session("TPCM_ID") = oComunita.TipoComunita.ID

                    Dim defaultUrl As String = PageUtility.GetCommunityDefaultPage(CMNT_ID, PRSN_ID)
                    If oComunita.ShowCover(CMNT_ID, PRSN_ID) Then
                        If oRuolo.SaltaCopertina Then
                            Me.PageUtility.RedirectToUrl(defaultUrl)
                        Else
                            Me.PageUtility.RedirectToUrl("Generici/Cover.aspx")
                        End If
                    Else
                        Me.PageUtility.RedirectToUrl(defaultUrl)
                    End If

                ElseIf oRuolo.Attivato = False Then
                    Me.Reset_ToMessaggi()
                    'Me.LBMessaggi.Text = "Non è possibile loggarsi! Non si è stati attivati"
                    oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.bloccato)
                    oTreeComunita.CambiaAttivazione(CMNT_ID, False, oResource)
                ElseIf oRuolo.Abilitato = False Then
                    Me.Reset_ToMessaggi()
                    oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.inAttesa)
                    oTreeComunita.CambiaAbilitazione(CMNT_ID, False)

                ElseIf oComunita.Bloccata = True Then
                    Me.Reset_ToMessaggi()
                    oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & Me.StringaAbilitato.comunitaBloccata)
                    oTreeComunita.CambiaAbilitazione(CMNT_ID, False)
                    oTreeComunita.CambiaIsBloccata(CMNT_ID, True)
                End If
            Else
                oTreeComunita.Delete(CMNT_ID, CMNT_Path)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_RicercaComunita"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBNopermessi)

            .setLabel(Me.LBtipoRicerca_c)

            .setDropDownList(Me.DDLTipoRicerca, -2)
            .setDropDownList(Me.DDLTipoRicerca, -7)
            .setDropDownList(Me.DDLTipoRicerca, -5)
            .setDropDownList(Me.DDLTipoRicerca, -6)
            .setDropDownList(Me.DDLTipoRicerca, -3)
            .setDropDownList(Me.DDLTipoRicerca, -4)

            .setCheckBox(Me.CBXmostraPadre)
            .setLabel(Me.LBnumeroRecord_c)


            .setButton(Me.BTNCerca)

            .setLabel(Me.LBlegenda)
            .setLabel(Me.LBorganizzazione_c)

            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)


            .setLinkButton(Me.LNBannullaConferma, True, True)
            .setLinkButton(Me.LNBannullaDettagli, True, True)
            .setLinkButton(Me.LNBelencoIscritte, True, True)
            .setLinkButton(Me.LNBentraDettagli, True, True)
            .setLinkButton(Me.LNBiscriviAltre, True, True)
            .setLinkButton(Me.LNBiscriviConferma, True, True)
            .setLinkButton(Me.LNBiscriviDettagli, True, True)
            .setLinkButton(Me.LNBannulla, True, True)
            .setLinkButton(Me.LNBiscriviMultipli, True, True)
            Me.LNBiscriviMultipli.Attributes.Add("onclick", "return HasCorsiSelezionati(false,'" & Replace(Me.oResource.getValue("MessaggioSelezione"), "'", "\'") & "','');")


            .setHeaderDatagrid(Me.DGcomunita, 0, "TPCM_Descrizione", True)
            .setHeaderDatagrid(Me.DGcomunita, 1, "CMNT_Nome", True)
            .setHeaderDatagrid(Me.DGcomunita, 2, "AnnoAccademico", True)
            .setHeaderDatagrid(Me.DGcomunita, 3, "Periodo", True)

            .setHeaderDatagrid(Me.DGcomunita, 5, "AnagraficaResponsabile", True)
            .setHeaderDatagrid(Me.DGcomunita, 6, "Iscritti", True)

            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)
            Dim i As Integer
            Try
                For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                    Dim oLinkButton As New LinkButton
                    oLinkButton = Me.FindControlRecursive(Me.Master, "LKB" & Chr(i))
                    Dim Carattere As String = Chr(i)
                    If IsNothing(oLinkButton) = False Then
                        .setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                    End If
                Next
            Catch ex As Exception

            End Try
        End With
    End Sub
#End Region

    Private Sub LNBiscriviConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviConferma.Click
        Dim iResponse As Main.ErroriIscrizioneComunita
        Dim oPersona As New COL_Persona

        If Session("azione") = "iscrivi" Then
            Me.PNLconferma.Visible = False
            Try
                Dim oUtility As New OLDpageUtility(Me.Context)

                oPersona = Session("objPersona")

                iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, Me.HDisChiusa.Value, oUtility.BaseUrlDrivePath & "profili/" & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                    Me.Reset_ToMessaggi()
                    Me.LBMessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                Else
                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                        oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    Else
                        oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    End If
                    Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Me.Reset_ToIscrizioneAvvenuta()
                End If
                Me.HDNcmnt_ID.Value = ""
                Me.HDisChiusa.Value = ""
                Me.HDNcmnt_Path.Value = ""
            Catch ex As Exception
                Me.Reset_Contenuto(True, True)
            End Try
        ElseIf Session("azione") = "iscriviMultipli" Then
            Try
                Me.IscrizioneMultipla(False)
            Catch ex As Exception
                Me.Reset_Contenuto(True, True)
            End Try
        Else
            Session("azione") = "loaded"
            Me.Reset_Contenuto(True, True)
        End If
    End Sub
    Private Sub LNBannullaConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaConferma.Click
        Me.Reset_Contenuto(True, True)
    End Sub

    Private Sub LNBelencoIscritte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelencoIscritte.Click
        Try
            Me.PageUtility.RedirectToUrl("Generici/EntrataComunita.aspx")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LNBiscriviAltre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviAltre.Click
        Me.Reset_Contenuto(True, True)
    End Sub

    Private Sub LNBannulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Me.Reset_Contenuto(True, True)
    End Sub


    Private Sub IscrizioneMultipla(ByVal isForconferma As Boolean)
        Dim i, totale, CMNT_ID As Integer
        Dim oDataset As DataSet
        Dim oDataview As DataView

        oDataset = Me.FiltraggioDatiRistretto
        oDataview = oDataset.Tables(0).DefaultView
        oDataview.RowFilter = "'" & Me.HDNcomunitaSelezionate.Value & "' like '%,' + CMNT_ID +',%'"
        totale = oDataview.Count

        If isForconferma Then
            Dim ListaComunita As String
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataview.Item(i).Row

                If ListaComunita = "" Then
                    ListaComunita = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome") & " - "
                Else
                    ListaComunita &= "<li>" & oRow.Item("CMNT_Nome") & " - "
                End If
                If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                    ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                Else
                    If oRow.Item("CMNT_Responsabile") = "" Then
                        ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                    Else
                        ListaComunita &= oRow.Item("CMNT_Responsabile") & vbCrLf
                    End If
                End If
            Next
            If ListaComunita <> "" Then
                ListaComunita &= "</ul>"
            End If
            Me.ResetFormToConferma(True, ListaComunita, "")
        Else
            Dim ListaLimiteSuperato, ListaIscrizioneInAttesa, ListaIscrizioneAvvenuta, ListaErroreGenerico As String
            Dim iResponse As Main.ErroriIscrizioneComunita
            Dim oUtility As New OLDpageUtility(Me.Context)

            Dim oPersona As New COL_Persona


            oPersona = Session("objPersona")

            Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataview.Item(i).Row

                iResponse = oPersona.IscrizioneComunitaNew(oRow.Item("CMNT_ID"), oRow.Item("ALCM_Path"), oRow.Item("ALCM_isChiusaForPadre"), oUtility.BaseUrlDrivePath & "./../profili/" & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                Select Case iResponse
                    Case Main.ErroriIscrizioneComunita.LimiteSuperato
                        If ListaLimiteSuperato = "" Then
                            ListaLimiteSuperato = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaLimiteSuperato &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.IscrizioneInAttesa
                        oServiceUtility.NotifyAddWaitingSubscription(oRow.Item("CMNT_ID"), Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        If ListaIscrizioneInAttesa = "" Then
                            ListaIscrizioneInAttesa = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaIscrizioneInAttesa &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
                        oServiceUtility.NotifyAddSelfSubscription(oRow.Item("CMNT_ID"), Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        If ListaIscrizioneAvvenuta = "" Then
                            ListaIscrizioneAvvenuta = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaIscrizioneAvvenuta &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.ErroreGenerico
                        If ListaErroreGenerico = "" Then
                            ListaErroreGenerico = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaErroreGenerico &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                End Select
            Next

			Me.HDNcomunitaSelezionate.Value = ""
			Me.Reset_ToIscrizioneAvvenuta()
			If ListaIscrizioneAvvenuta <> "" Then
				ListaIscrizioneAvvenuta &= "</ul>"
				LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.IscrizioneAvvenuta, Main.ErroriIscrizioneComunita))
				LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaIscrizioneAvvenuta) & "<br>"
			End If
			If ListaIscrizioneInAttesa <> "" Then
				ListaIscrizioneInAttesa &= "</ul>"
				LBiscrizione.Text &= Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.IscrizioneInAttesa, Main.ErroriIscrizioneComunita))
				LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaIscrizioneInAttesa) & "<br>"
			End If
			If ListaLimiteSuperato <> "" Then
				ListaLimiteSuperato &= "</ul>"
				LBiscrizione.Text &= Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.LimiteSuperato, Main.ErroriIscrizioneComunita))
				LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaLimiteSuperato) & "<br>"
			End If
			If ListaErroreGenerico <> "" Then
				ListaErroreGenerico &= "</ul>"
				LBiscrizione.Text &= Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.ErroreGenerico, Main.ErroriIscrizioneComunita))
				LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaErroreGenerico) & "<br>"
			End If
		End If
	End Sub
	Private Sub LNBiscriviMultipli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviMultipli.Click
		If Session("azione") <> "iscriviMultipli" Then
			Try
				Dim oImpostazioni As New COL_ImpostazioniUtente
				Dim isRequiredConfirm As Boolean = False
				Try
					oImpostazioni = Session("oImpostazioni")
					isRequiredConfirm = oImpostazioni.ShowConferma
				Catch ex As Exception

				End Try

				If isRequiredConfirm Then
					Me.IscrizioneMultipla(True)
					Session("azione") = "iscriviMultipli"
				Else
					Me.IscrizioneMultipla(False)
				End If
			Catch ex As Exception

			End Try
		Else
			Session("azione") = "loaded"
			Me.Reset_Contenuto(True, True)
		End If
	End Sub


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_IscrizioneComunita.Codex)
    End Sub

    Public ReadOnly Property BodyId As String
        Get
            Return Me.Master.BodyIdCode
        End Get
    End Property
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

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
End Class



'<%= Me.BodyId() %>.onkeydown = SubmitRicerca(event);
'	function SubmitRicerca(event){
'			if (document.all){
'			if (event.keyCode == 13){
'				event.returnValue=false;
'				event.cancel = true;
'				try{
'                    eval('BTNCerca=<%=Me.BTNCerca.ClientID%>');
'					//document.forms[0].BTNCerca.click();
'                    BTNCerca.click();
'                    }
'				catch (ex){
'					return false;}
'				}
'			}
'		else if (document.getElementById){
'			if (event.which == 13){
'				event.returnValue=false;
'				event.cancel = true;
'				try{
'					eval('BTNCerca=<%=Me.BTNCerca.ClientID%>');
'					//document.forms[0].BTNCerca.click();
'                    BTNCerca.click();
'                    }
'				catch(ex){
'					return false;}
'				}
'			}
'		else if(document.layers){
'			if(event.which == 13){
'				event.returnValue=false;
'				event.cancel = true;
'					try{
'					    //document.forms[0].BTNCerca.click();}
'                        eval('BTNCerca=<%=Me.BTNCerca.ClientID%>');
'					    BTNCerca.click();
'                    }
'				catch(ex){
'					return false;}
'				}
'			}
'		else
'			return true;
'	}