Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita


Public Class WizardModificaComunita
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager
	Private _OldPageUtility As OLDpageUtility

	Private ReadOnly Property Utility() As OLDpageUtility
		Get
			If IsNothing(_OldPageUtility) Then
				_OldPageUtility = New OLDpageUtility(Me.Context)
			End If
			Return _OldPageUtility
		End Get
	End Property

	Public Enum IndiceFasi
		Fase1_Dati = 0
		Fase2_SceltaResponsabile = 1
		Fase3_ModificaPadri = 5
		Fase3_AggiungiPadri = 6
		Fase4_SceltaServizi = 7
		Fase5_ServizioDefault = 8
	End Enum

#Region "Definizioni Generali"
	Protected WithEvents HDN_ComunitaAttualeID As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
	Protected WithEvents TBRmenu As System.Web.UI.WebControls.TableRow
	Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton

	Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
	Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
	Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
#End Region

#Region "Gestione Navigazione"
	Protected WithEvents PNLnavigazione As System.Web.UI.WebControls.Panel
	Protected WithEvents BTNelenco As System.Web.UI.WebControls.Button
	Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
	Protected WithEvents BTNavanti As System.Web.UI.WebControls.Button
	Protected WithEvents BTNsalva As System.Web.UI.WebControls.Button
	Protected WithEvents PNLnavigazione2 As System.Web.UI.WebControls.Panel
	Protected WithEvents BTNelenco2 As System.Web.UI.WebControls.Button
	Protected WithEvents BTNindietro2 As System.Web.UI.WebControls.Button
	Protected WithEvents BTNavanti2 As System.Web.UI.WebControls.Button
	Protected WithEvents BTNsalva2 As System.Web.UI.WebControls.Button
	Protected WithEvents BTNaggiungiPadre As System.Web.UI.WebControls.Button
	Protected WithEvents BTNaggiungiPadre2 As System.Web.UI.WebControls.Button
#End Region

#Region "Gestione Navigazione"
	Protected WithEvents TBLinserimento As System.Web.UI.WebControls.Table
	Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
	Protected WithEvents CTRLdati As Comunita_OnLine.UC_Fase1DatiComunita
	Protected WithEvents TBLresponsabile As System.Web.UI.WebControls.Table
	Protected WithEvents CTRLresponsabile As Comunita_OnLine.UC_Fase2Responsabile
	Protected WithEvents TBLpadri As System.Web.UI.WebControls.Table
	Protected WithEvents CTRLvisualizzaPadri As Comunita_OnLine.UC_Fase3VisualizzaComunitaPadri
	Protected WithEvents TBLservizi As System.Web.UI.WebControls.Table
	Protected WithEvents CTRLservizi As Comunita_OnLine.UC_Fase4ModificaServizi
	Protected WithEvents TBLfinale As System.Web.UI.WebControls.Table
	Protected WithEvents CTRLfinale As Comunita_OnLine.UC_Fase5sceltaDefault

	Protected WithEvents TBLaggiungiPadri As System.Web.UI.WebControls.Table
	Protected WithEvents CTRLaggiungiPadri As Comunita_OnLine.UC_Fase3AggiungiComunitaPadri
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
			SetCulture(Session("LinguaCode"))
		End If

		If Me.SessioneScaduta() Then
			Exit Sub
		End If

		If Not Page.IsPostBack Then	'solo al primo giro
			Dim oPersona As COL_Persona
			Dim oServizio As New UCServices.Services_AmministraComunita
			Dim ForAdmin As Boolean = False
			Dim ComunitaID As Integer = 0

			oPersona = Session("objPersona")
			Try
				If Utility.isModalitaAmministrazione = True Then
					ComunitaID = Session("idComunita_forAdmin")
				Else
					ComunitaID = Session("idComunita")
				End If
			Catch ex As Exception
				Try
					ComunitaID = Session("idComunita")
				Catch ex2 As Exception
					ComunitaID = 0
				End Try
			End Try

			Try
				If ForAdmin = False Then
					oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
				Else
					If ComunitaID = Session("idComunita") Then
						oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
					Else
						oServizio.PermessiAssociati = oPersona.GetPermessiForServizioForAdmin(ComunitaID, oServizio.Codex, False, oServizio.GetPermission_Change, oServizio.GetPermission_Moderate, oServizio.GetPermission_Moderate)
					End If
				End If

				If (oServizio.PermessiAssociati = "") Then
					oServizio.PermessiAssociati = "00000000000000000000000000000000"
				End If
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try
			Me.SetupInternazionalizzazione()


			Dim oFaseAttiva As IndiceFasi = IndiceFasi.Fase1_Dati
			If Session("azione") = "associa" Then
				oFaseAttiva = IndiceFasi.Fase3_ModificaPadri
			End If
			Session("Azione") = "loaded"

			Response.CacheControl = "no-cache"
			Response.AddHeader("Pragma", "no-cache")
			Response.Expires = -1

			If oServizio.Admin Or oServizio.Moderate Or oServizio.CreateComunity Then
				Me.PNLcontenuto.Visible = True
				Me.TBRmenu.Visible = False
				Me.PNLpermessi.Visible = False
				Me.PNLnavigazione.Visible = True
				Me.PNLnavigazione2.Visible = True
				Me.Bind_Dati(ComunitaID, oFaseAttiva)
			Else
				Me.TBRmenu.Visible = True
				Me.PNLcontenuto.Visible = False
				Me.PNLnavigazione.Visible = False
				Me.PNLnavigazione2.Visible = False
				Me.PNLpermessi.Visible = True
			End If
        End If

        Me.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID

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
		Else
			Try
				Dim CMNT_ID As Integer = 0
				Try
					If Utility.isModalitaAmministrazione = True Then
						CMNT_ID = Session("idComunita_forAdmin")
					Else
						CMNT_ID = Session("idComunita")
					End If
				Catch ex As Exception
					Try
						CMNT_ID = Session("idComunita")
					Catch ex2 As Exception
						CMNT_ID = 0
					End Try
				End Try
				If CMNT_ID <= 0 Then
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

		Utility.isModalitaAmministrazione = False
		Session("CMNT_path_forAdmin") = ""
		Session("idComunita_forAdmin") = ""
		Session("TPCM_ID") = ""
		Me.Response.Expires = 0
		Me.Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/EntrataComunita.aspx", True)
	End Sub

#Region "Localizzazione"
	Private Sub SetCulture(ByVal Code As String)
		oResource = New ResourceManager
		oResource.UserLanguages = Code
		oResource.ResourcesName = "pg_WizardCreaComunita"
		oResource.Folder_Level1 = "Comunita"
		oResource.setCulture()
	End Sub

	Private Sub SetupInternazionalizzazione()
		With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
			.setLabel(LBNopermessi)
			.setButton(Me.BTNavanti, True, , , True)
			.setButton(Me.BTNsalva, True, , , True)
			.setButton(Me.BTNavanti2, True, , , True)
			.setButton(Me.BTNsalva2, True, , , True)
			.setLinkButton(Me.LNBindietro, True, True)

			.setButton(Me.BTNelenco, True, , , True)
			.setButton(Me.BTNindietro, True, , , True)
			.setButton(Me.BTNelenco2, True, , , True)
			.setButton(Me.BTNindietro2, True, , , True)
			.setButton(Me.BTNaggiungiPadre2, True, , , True)
			.setButton(Me.BTNaggiungiPadre, True, , , True)
		End With
	End Sub
#End Region

	Private Sub Bind_Dati(ByVal ComunitaID As Integer, ByVal FaseAttiva As IndiceFasi)
		Dim oComunita As New COL_Comunita

		Me.resetForm_ToFase1()

		Try
			oComunita.Id = ComunitaID
			oComunita.Estrai()
			Me.HDN_ComunitaAttualeID.Value = ComunitaID
			If Me.CTRLdati.SetupIniziale(ComunitaID, oComunita.IdPadre, False) Then
				Me.BTNavanti.Enabled = True

				If Not (oComunita.IdPadre = 0 And FaseAttiva = IndiceFasi.Fase3_AggiungiPadri) Then
					Select Case FaseAttiva
						Case IndiceFasi.Fase2_SceltaResponsabile
							Me.resetForm_ToFase2()
							If Me.CTRLresponsabile.isInizializzato = False Then
								Me.CTRLresponsabile.SetupControllo(ComunitaID, oComunita.IdPadre, oComunita.Organizzazione.Id, Me.CTRLdati.TipoComunita_ID, False)
							End If
						Case IndiceFasi.Fase3_ModificaPadri
							Me.resetForm_ToFase3()

							Dim ComunitaPath As String
							Try
								If Utility.isModalitaAmministrazione = True Then
									ComunitaPath = Session("CMNT_path_forAdmin")
								Else
									Dim ArrComunita(,) As String = Session("ArrComunita")
									Try
										ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
									Catch ex As Exception
										ComunitaPath = ""
									End Try
								End If
							Catch ex As Exception
								Try
									ComunitaPath = Session("CMNT_path_forAdmin")
								Catch ex2 As Exception
									ComunitaPath = ""
								End Try
							End Try
							Me.CTRLvisualizzaPadri.SetupControl(oComunita.Id, ComunitaPath)
						Case IndiceFasi.Fase4_SceltaServizi
							Me.resetForm_ToFase4()

							If Me.CTRLresponsabile.isInizializzato = False Then
								Me.CTRLresponsabile.SetupControllo(ComunitaID, oComunita.IdPadre, oComunita.Organizzazione.Id, Me.CTRLdati.TipoComunita_ID, False)
							End If
							Me.CTRLservizi.SetupControl(Me.CTRLdati.TipoComunita_ID, oComunita.Organizzazione.Id, Session("objPersona").id, Me.CTRLresponsabile.ResponsabileID, ComunitaID)

						Case IndiceFasi.Fase5_ServizioDefault
							Me.resetForm_ToFase5()
							Me.CTRLfinale.SetupControl(ComunitaID)
					End Select
				End If
			Else
				Me.BTNavanti.Enabled = False
			End If
		Catch ex As Exception
			Me.BTNavanti.Enabled = False
		End Try
	End Sub

	Private Sub resetForm_HideAll()
		Me.TBLdati.Visible = False
		Me.TBLfinale.Visible = False
		Me.TBLpadri.Visible = False
		Me.TBLresponsabile.Visible = False
		Me.TBLservizi.Visible = False
		Me.TBLaggiungiPadri.Visible = False
		Me.BTNaggiungiPadre2.Visible = False
		Me.BTNaggiungiPadre.Visible = False
	End Sub
	Private Sub resetForm_ToFase1()
		Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
		Me.BTNindietro.Visible = False
		Me.BTNavanti.Visible = True
		Me.BTNsalva.Visible = True
		Me.BTNavanti.Enabled = True
		Me.BTNavanti2.Enabled = True
		Me.BTNsalva.Enabled = True

		Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
		Me.BTNelenco2.Visible = Me.BTNelenco.Visible
		Me.BTNindietro2.Visible = Me.BTNindietro.Visible
		Me.BTNavanti2.Visible = Me.BTNavanti.Visible
		Me.BTNsalva2.Visible = Me.BTNsalva.Visible
		Me.TBLdati.Visible = True
		Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase1_Dati)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase1_Dati)
	End Sub
	Private Sub resetForm_ToFase2()
		Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
		Me.BTNindietro.Visible = True
		Me.BTNavanti.Visible = True
		Me.BTNsalva.Visible = True
		Me.BTNsalva.Enabled = True

		Me.BTNelenco2.Visible = Me.BTNelenco.Visible
		Me.BTNindietro2.Visible = Me.BTNindietro.Visible
		Me.BTNavanti2.Visible = Me.BTNavanti.Visible
		Me.BTNsalva2.Visible = Me.BTNsalva.Visible
		Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
		Me.TBLresponsabile.Visible = True
		Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase2_SceltaResponsabile)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase2_SceltaResponsabile)
	End Sub
	Private Sub resetForm_ToFase3()
		Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
		Me.BTNindietro.Visible = True
		Me.BTNavanti.Visible = True
		Me.BTNsalva.Visible = False
		Me.BTNavanti.Enabled = True
		If Me.CTRLdati.TipoComunita_ID = Main.TipoComunitaStandard.Organizzazione Then
			Me.BTNaggiungiPadre.Visible = False
		Else
			Me.BTNaggiungiPadre.Visible = True
		End If
		Me.BTNaggiungiPadre2.Visible = Me.BTNaggiungiPadre.Visible

		Me.BTNelenco2.Visible = Me.BTNelenco.Visible
		Me.BTNindietro2.Visible = Me.BTNindietro.Visible
		Me.BTNavanti2.Visible = Me.BTNavanti.Visible
		Me.BTNsalva2.Visible = Me.BTNsalva.Visible
		Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
		Me.TBLpadri.Visible = True
		Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase3_ModificaPadri)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase3_ModificaPadri)
	End Sub
	Private Sub resetForm_ToFase3Aggiungi()
		Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
		Me.BTNindietro.Visible = True
		Me.BTNavanti.Visible = False
		Me.BTNsalva.Visible = True
		Me.BTNsalva.Enabled = True

		Me.BTNelenco2.Visible = Me.BTNelenco.Visible
		Me.BTNindietro2.Visible = Me.BTNindietro.Visible
		Me.BTNavanti2.Visible = Me.BTNavanti.Visible
		Me.BTNsalva2.Visible = Me.BTNsalva.Visible
		Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
		Me.TBLaggiungiPadri.Visible = True
		Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase3_AggiungiPadri)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase3_AggiungiPadri)
	End Sub
	Private Sub resetForm_ToFase4()
		Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
		Me.BTNindietro.Visible = True
		Me.BTNavanti.Visible = True
		Me.BTNsalva.Visible = True
		Me.BTNsalva.Enabled = True

		Me.BTNelenco2.Visible = Me.BTNelenco.Visible
		Me.BTNindietro2.Visible = Me.BTNindietro.Visible
		Me.BTNavanti2.Visible = Me.BTNavanti.Visible
		Me.BTNsalva2.Visible = Me.BTNsalva.Visible
		Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
		Me.TBLservizi.Visible = True
		Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase4_SceltaServizi)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase4_SceltaServizi)
	End Sub
	Private Sub resetForm_ToFase5()
		Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
		Me.BTNindietro.Visible = True
		Me.BTNavanti.Visible = False
		Me.BTNsalva.Visible = True
		Me.BTNsalva.Enabled = True

		Me.BTNelenco2.Visible = Me.BTNelenco.Visible
		Me.BTNindietro2.Visible = Me.BTNindietro.Visible
		Me.BTNavanti2.Visible = Me.BTNavanti.Visible
		Me.BTNsalva2.Visible = Me.BTNsalva.Visible
		Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
		Me.TBLfinale.Visible = True
		Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase5_ServizioDefault)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.modifica." & Me.IndiceFasi.Fase5_ServizioDefault)
	End Sub
	Private Sub ReturnToPreviousPage()
        If Request.QueryString("fromGestione") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
        Else
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement)
        End If
    End Sub

	Private Sub BTNavanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNavanti.Click, BTNavanti2.Click
		Dim ComunitaID As Integer
		Dim ComunitaPath As String
		Try
			If Utility.isModalitaAmministrazione Then
				ComunitaID = Session("idComunita_forAdmin")
				ComunitaPath = Session("CMNT_path_forAdmin")
			Else
				Dim ArrComunita(,) As String = Session("ArrComunita")
				ComunitaID = Session("idComunita")

				Try
					ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
				Catch ex As Exception
					ComunitaPath = ""
				End Try
			End If
		Catch ex As Exception
			Try
				Dim ArrComunita(,) As String = Session("ArrComunita")
				ComunitaID = Session("idComunita")
				ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
			Catch ex2 As Exception
				ComunitaID = 0
				ComunitaPath = ""
			End Try
		End Try

		If Me.TBLdati.Visible Then
			Me.resetForm_ToFase2()
			If Me.CTRLresponsabile.isInizializzato = False Then
				Dim oComunita As New COL_Comunita

				oComunita.Id = ComunitaID
				oComunita.Estrai()
				Me.CTRLresponsabile.SetupControllo(ComunitaID, oComunita.IdPadre, oComunita.Organizzazione.Id, Me.CTRLdati.TipoComunita_ID, False)
			Else
				Me.CTRLresponsabile.AggiornaDati()
			End If
		ElseIf Me.TBLresponsabile.Visible Then
			Dim oComunita As New COL_Comunita

			oComunita.Id = ComunitaID
			oComunita.Estrai()

          
            If Me.CTRLdati.TipoComunita_ID = Main.TipoComunitaStandard.Organizzazione Then
                Me.resetForm_ToFase4()
                If Me.CTRLservizi.isInizializzato = False Then
                    Me.CTRLservizi.SetupControl(Me.CTRLdati.TipoComunita_ID, oComunita.Organizzazione.Id, Session("objPersona").id, Me.CTRLresponsabile.ResponsabileID, ComunitaID)
                Else
                    Me.CTRLservizi.AggiornaDati(Me.CTRLdati.TipoComunita_ID, ComunitaID)
                End If
            Else
                Me.resetForm_ToFase3()
                Me.CTRLvisualizzaPadri.SetupControl(oComunita.Id, ComunitaPath)
            End If
        ElseIf Me.TBLpadri.Visible Then
            Me.resetForm_ToFase4()
            If Me.CTRLservizi.isInizializzato = False Then
                Dim oComunita As New COL_Comunita

                oComunita.Id = ComunitaID
                oComunita.Estrai()
                If Me.CTRLresponsabile.isInizializzato = False Then
                    Me.CTRLresponsabile.SetupControllo(ComunitaID, oComunita.IdPadre, oComunita.Organizzazione.Id, Me.CTRLdati.TipoComunita_ID, False)
                End If
                Me.CTRLservizi.SetupControl(Me.CTRLdati.TipoComunita_ID, oComunita.Organizzazione.Id, Session("objPersona").id, Me.CTRLresponsabile.ResponsabileID, ComunitaID)
            Else
                Me.CTRLservizi.AggiornaDati(Me.CTRLdati.TipoComunita_ID, ComunitaID)
            End If
        ElseIf Me.TBLservizi.Visible Then
            Me.resetForm_ToFase5()
            Me.CTRLfinale.SetupControl(ComunitaID)
        End If
	End Sub
	Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click, BTNindietro2.Click
		Dim ComunitaID As Integer
		Dim ComunitaPath As String
		Try
			If Utility.isModalitaAmministrazione = True Then
				ComunitaID = Session("idComunita_forAdmin")
				ComunitaPath = Session("CMNT_path_forAdmin")
			Else
				Dim ArrComunita(,) As String = Session("ArrComunita")
				ComunitaID = Session("idComunita")

				Try
					ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
				Catch ex As Exception
					ComunitaPath = ""
				End Try
			End If
		Catch ex As Exception
			Try
				Dim ArrComunita(,) As String = Session("ArrComunita")
				ComunitaID = Session("idComunita")
				ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
			Catch ex2 As Exception
				ComunitaID = 0
				ComunitaPath = ""
			End Try
		End Try

		If Me.TBLresponsabile.Visible Then
			Me.resetForm_ToFase1()
		ElseIf Me.TBLpadri.Visible Then
			Me.resetForm_ToFase2()
			If Me.CTRLresponsabile.isInizializzato = False Then
				Dim oComunita As New COL_Comunita

				oComunita.Id = ComunitaID
				oComunita.Estrai()
				Me.CTRLresponsabile.SetupControllo(ComunitaID, oComunita.IdPadre, oComunita.Organizzazione.Id, Me.CTRLdati.TipoComunita_ID, False)
			Else
				Me.CTRLresponsabile.AggiornaDati()
			End If

		ElseIf Me.TBLaggiungiPadri.Visible Then
			Dim oComunita As New COL_Comunita

			oComunita.Id = ComunitaID
			oComunita.Estrai()
			Me.resetForm_ToFase3()
			Me.CTRLvisualizzaPadri.SetupControl(oComunita.Id, ComunitaPath)
		ElseIf Me.TBLservizi.Visible = True Then
			Dim oComunita As New COL_Comunita

			oComunita.Id = ComunitaID
			oComunita.Estrai()
			If Me.CTRLdati.TipoComunita_ID = Main.TipoComunitaStandard.Organizzazione Then
				Me.resetForm_ToFase2()

				If Me.CTRLresponsabile.isInizializzato = False Then
					Me.CTRLresponsabile.SetupControllo(ComunitaID, oComunita.IdPadre, oComunita.Organizzazione.Id, Me.CTRLdati.TipoComunita_ID, False)
				Else
					Me.CTRLresponsabile.AggiornaDati()
				End If
			Else
				Me.resetForm_ToFase3()
				Me.CTRLvisualizzaPadri.SetupControl(oComunita.Id, ComunitaPath)
			End If
		ElseIf Me.TBLfinale.Visible Then
			Me.resetForm_ToFase4()
			If Me.CTRLservizi.isInizializzato = False Then
				Dim oComunita As New COL_Comunita

				oComunita.Id = ComunitaID
				oComunita.Estrai()
				Me.CTRLservizi.SetupControl(Me.CTRLdati.TipoComunita_ID, oComunita.Organizzazione.Id, Session("objPersona").id, Me.CTRLresponsabile.ResponsabileID)
			Else
				Me.CTRLservizi.AggiornaDati(Me.CTRLdati.TipoComunita_ID, 0)
			End If

		End If
	End Sub

	Private Sub BTNaggiungiPadre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaggiungiPadre.Click, BTNaggiungiPadre2.Click
		Dim ComunitaID As Integer
		Dim ComunitaPath As String
		Try
			If Utility.isModalitaAmministrazione = True Then
				ComunitaID = Session("idComunita_forAdmin")
				ComunitaPath = Session("CMNT_path_forAdmin")
			Else
				Dim ArrComunita(,) As String = Session("ArrComunita")
				ComunitaID = Session("idComunita")

				Try
					ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
				Catch ex As Exception
					ComunitaPath = ""
				End Try
			End If
		Catch ex As Exception
			Try
				ComunitaID = Session("idComunita")
				ComunitaPath = Session("CMNT_path_forAdmin")
			Catch ex2 As Exception
				ComunitaID = 0
				ComunitaPath = ""
			End Try
		End Try

		Dim oComunita As New COL_Comunita
		Me.resetForm_ToFase3Aggiungi()
		oComunita.Id = ComunitaID
		oComunita.Estrai()
		Me.CTRLaggiungiPadri.SetupControl(oComunita.Livello - 1, oComunita.Id, ComunitaPath)

	End Sub

	Private Sub CTRLresponsabile_AggiornamentoVisualizzazione(ByVal Selezionato As Boolean) Handles CTRLresponsabile.AggiornamentoVisualizzazione
		Me.BTNsalva.Enabled = Selezionato
		Me.BTNsalva2.Enabled = Selezionato
	End Sub

	Private Sub CTRLservizi_AggiornamentoVisualizzazione(ByVal Selezionato As Boolean) Handles CTRLservizi.AggiornamentoVisualizzazione
		Me.BTNavanti.Enabled = Selezionato
		Me.BTNavanti2.Enabled = Selezionato
		Me.BTNsalva2.Enabled = Selezionato
		Me.BTNsalva.Enabled = Selezionato
	End Sub

	Private Sub CTRLfinale_AggiornamentoVisualizzazione(ByVal Selezionato As Boolean) Handles CTRLfinale.AggiornamentoVisualizzazione
		Me.BTNsalva2.Enabled = Selezionato
		Me.BTNsalva.Enabled = Selezionato
	End Sub
	Private Sub BTNsalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsalva.Click, BTNsalva2.Click
		If Session("Azione") = "loaded" And Me.HDN_ComunitaAttualeID.Value <> 0 Then
			Dim iResponse As WizardComunita_Message
			Dim Messaggio As String = ""
			Dim ComunitaPath As String = ""
			Dim ComunitaID As Integer

			If Utility.isModalitaAmministrazione = True Then
				ComunitaID = Session("idComunita_forAdmin")
				ComunitaPath = Session("CMNT_path_forAdmin")
			Else

				ComunitaID = Session("idComunita")

                Try
                    Dim ArrComunita(,) As String = Session("ArrComunita")
                    ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                Catch ex As Exception
                    'Dim oComunita As New COL_Comunita
                    'oComunita.Id = ComunitaID
                    'oComunita.Estrai()
                    'If oComunita.TipoComunita.ID = 0 Then
                    '    ComunitaPath = "." &
                    'Else
                    ComunitaPath = ""
                    ' End If

                End Try
            End If

			If Me.TBLdati.Visible Then
				iResponse = Me.CTRLdati.SalvaModifiche

				If iResponse <> WizardComunita_Message.ComunitaModificata Then
					Messaggio = Me.oResource.getValue("WizardComunita_Message." & CType(iResponse, WizardComunita_Message))
					If Messaggio <> "" Then
						Messaggio = Messaggio.Replace("'", "\'")
						Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
					End If
				End If
			ElseIf Me.TBLresponsabile.Visible Then
				' VERIFICARE
				iResponse = Me.CTRLresponsabile.RegistraResponsabile(ComunitaID, ComunitaPath, True)
				If iResponse <> WizardComunita_Message.ResponsabileAssociato Then
					Messaggio = Me.oResource.getValue("WizardComunita_Message." & CType(iResponse, WizardComunita_Message))
					If Messaggio <> "" Then
						Messaggio = Messaggio.Replace("'", "\'")
						Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
					End If
				End If
			ElseIf Me.TBLaggiungiPadri.Visible Then
				Dim oComunita As New COL_Comunita
				Dim ResponsabileID As Integer
				oComunita.Id = ComunitaID
				oComunita.Estrai()

				If Me.CTRLresponsabile.isInizializzato Then
					ResponsabileID = Me.CTRLresponsabile.ResponsabileID
				Else
					Try
						ResponsabileID = oComunita.GetResponsabile.Id
					Catch ex As Exception

					End Try

				End If
				iResponse = Me.CTRLaggiungiPadri.RegistraPadri(ComunitaID, oComunita.CreatoreID, ResponsabileID)
				If iResponse <> ModuloEnum.WizardComunita_Message.PadriAssociati Then
					Messaggio = Me.oResource.getValue("WizardComunita_Message." & CType(iResponse, WizardComunita_Message))
					If Messaggio <> "" Then
						Messaggio = Messaggio.Replace("'", "\'")
						Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
					End If
				Else
					Me.resetForm_ToFase3()
					Me.CTRLvisualizzaPadri.SetupControl(oComunita.Id, ComunitaPath)
				End If
			ElseIf Me.TBLservizi.Visible Then
				iResponse = Me.CTRLservizi.SalvaImpostazioni()
				If iResponse <> ModuloEnum.WizardComunita_Message.ServiziAttivati Then
					Messaggio = Me.oResource.getValue("WizardComunita_Message." & CType(iResponse, WizardComunita_Message))
					If Messaggio <> "" Then
						Messaggio = Messaggio.Replace("'", "\'")
						Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
					End If
				End If
			ElseIf Me.TBLfinale.Visible Then
				iResponse = Me.CTRLfinale.RegistraServizioDefault()
				If iResponse <> ModuloEnum.WizardComunita_Message.ServiziDefault Then
					Messaggio = Me.oResource.getValue("WizardComunita_Message." & CType(iResponse, WizardComunita_Message))
					If Messaggio <> "" Then
						Messaggio = Messaggio.Replace("'", "\'")
						Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
					End If
				End If
			End If
		End If
	End Sub


	Private Sub BTNelenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNelenco.Click, BTNelenco2.Click
		If Request.QueryString("toTree") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true&toTree=true")
		Else
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
		End If
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
End Class