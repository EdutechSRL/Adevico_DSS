Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class Inserimento_Evento
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

    Private _PageUtility As OLDpageUtility
    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Private Enum Inserimento
        DataMancante = -5
        ErroreProgramma = -4
        ErroreCreazioneOrari = -3
        ErroreCreazionePersonale = -2
        ErroreCreazione = -1
        ErroreGenerico = 0
        EventoInserito = 1
        EventoModificato = 2
    End Enum

    Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden

	Protected WithEvents LBdataI As System.Web.UI.WebControls.Label
	Protected WithEvents LBdataF As System.Web.UI.WebControls.Label
	Protected WithEvents HDNdataF As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNdataI As System.Web.UI.HtmlControls.HtmlInputHidden

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents LNBcalendario As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcrea As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcreaNuovo As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBReditorNote As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBReditorProgramma As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRaula As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRluogo As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRripetizioni As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRavvisoPersonale As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRannoAccademico As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRripetizione As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRprogramma As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmacro As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBprogrammaNormale_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBprogramma As System.Web.UI.WebControls.TextBox
    Protected WithEvents TBRnote As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBnoteNormale_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBnote As System.Web.UI.WebControls.TextBox


    Protected WithEvents PNLMain As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents DDLCategoria As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLAnnoAccademico As System.Web.UI.WebControls.DropDownList

    Protected WithEvents LBAvviso As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipoAvviso As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBAnnoAccademico As System.Web.UI.WebControls.Label
    Protected WithEvents LBAula As System.Web.UI.WebControls.Label
    Protected WithEvents TBAula As System.Web.UI.WebControls.TextBox


    Protected WithEvents LBnote As System.Web.UI.WebControls.Label
    Protected WithEvents TBLnote As System.Web.UI.WebControls.Table
    Protected WithEvents LNBmodificaNote As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaNote As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LBProgramma_t As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLeditorProgramma As UC_Editor
    Protected WithEvents CTRLeditorNote As UC_Editor
    Protected WithEvents TBLprogramma As System.Web.UI.WebControls.Table
    Protected WithEvents LBProgramma As System.Web.UI.WebControls.Label
    Protected WithEvents LNBmodificaProgramma As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaProgramma As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBvisualizzazione As System.Web.UI.WebControls.Label

    Protected WithEvents RBLRipetizione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBDataFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBFineRpetizione As System.Web.UI.WebControls.Label
    Protected WithEvents LBRipetizione As System.Web.UI.WebControls.Label

    Protected WithEvents DDLOra1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLOra2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLMinuti1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLMinuti2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBLOraInizio As System.Web.UI.WebControls.Label

    Protected WithEvents LBLOraFine As System.Web.UI.WebControls.Label

	'Protected WithEvents LBdataI As System.Web.UI.WebControls.TextBox
	'Protected WithEvents LBdataF As System.Web.UI.WebControls.TextBox
	Protected WithEvents TBLink As System.Web.UI.WebControls.TextBox
	Protected WithEvents TBLuogo As System.Web.UI.WebControls.TextBox
	Protected WithEvents TXBNomeEvento As System.Web.UI.WebControls.TextBox
	Protected WithEvents CBVisibile As System.Web.UI.WebControls.CheckBox
	Protected WithEvents TBAnni1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents TBAnni2 As System.Web.UI.WebControls.TextBox
	Protected WithEvents RBLCriterioAnnuale2 As System.Web.UI.WebControls.RadioButton
	Protected WithEvents RFVNomeEvento As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
	Protected WithEvents LBNomeEvento As System.Web.UI.WebControls.Label
	Protected WithEvents REVNomeEvento As System.Web.UI.WebControls.RegularExpressionValidator
	Protected WithEvents LBCategoria As System.Web.UI.WebControls.Label
	Protected WithEvents LBdataInizio As System.Web.UI.WebControls.Label
	Protected WithEvents LBCGgiorno As System.Web.UI.WebControls.Label
	Protected WithEvents RFVnumeroGiorni As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RVnumeroGiorni As System.Web.UI.WebControls.RangeValidator
	Protected WithEvents LBCSOgni As System.Web.UI.WebControls.Label
	Protected WithEvents LBCSSettimane As System.Web.UI.WebControls.Label
	Protected WithEvents RFVnumeroSettimane As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RVnumeroSettimane As System.Web.UI.WebControls.RangeValidator
	Protected WithEvents LBdal As System.Web.UI.WebControls.Label
	Protected WithEvents LBal As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCSLunedi As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCSMartedi As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCSMercoledi As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCSGiovedi As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCSVenerdi As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCSSabato As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCSDomenica As System.Web.UI.WebControls.Label
	Protected WithEvents RfvCMnumeroGiorni As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RVCMNumeroGiorni As System.Web.UI.WebControls.RangeValidator
	Protected WithEvents LBLCMOgni As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCMMesi As System.Web.UI.WebControls.Label
	Protected WithEvents RFVCMNumeroMesi As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RVCMNumeroMesi As System.Web.UI.WebControls.RangeValidator
	Protected WithEvents LBLCMOgni2 As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCMMesi2 As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCAOgni As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCAAnni As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCAdi As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCAdiOgni As System.Web.UI.WebControls.Label
	Protected WithEvents LBLCAanno2 As System.Web.UI.WebControls.Label
	Protected WithEvents LBLuogo As System.Web.UI.WebControls.Label
	Protected WithEvents REVLuogo As System.Web.UI.WebControls.RegularExpressionValidator
	Protected WithEvents REVAula As System.Web.UI.WebControls.RegularExpressionValidator
	Protected WithEvents LBnote_t As System.Web.UI.WebControls.Label
	Protected WithEvents LBLink As System.Web.UI.WebControls.Label
	Protected WithEvents LBMacro As System.Web.UI.WebControls.Label
	Protected WithEvents CBMacro As System.Web.UI.WebControls.CheckBox


#Region "Pannello Info"
	Protected WithEvents PNLinfo As System.Web.UI.WebControls.Panel
	Protected WithEvents LBinfo As System.Web.UI.WebControls.Label
#End Region

#Region "Ripetizione Giornaliera"
	Protected WithEvents PNLCriteriGiornalieri As System.Web.UI.WebControls.Panel

	Protected WithEvents RBCriteriGiornalieri1 As System.Web.UI.WebControls.RadioButton
	Protected WithEvents RDBferiali As System.Web.UI.WebControls.RadioButton
	Protected WithEvents RDBfestivi As System.Web.UI.WebControls.RadioButton
	Protected WithEvents TXBnumeroGiorni As System.Web.UI.WebControls.TextBox
#End Region

#Region "Ripetizione Settimanale"
	Protected WithEvents PNLCriteriSettimanali As System.Web.UI.WebControls.Panel
	Protected WithEvents TXBnumeroSettimane As System.Web.UI.WebControls.TextBox


	Protected WithEvents CBGiorno1 As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CBGiorno2 As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CBGiorno3 As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CBGiorno4 As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CBGiorno5 As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CBGiorno6 As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CBGiorno0 As System.Web.UI.WebControls.CheckBox
#End Region

#Region "Ripetizione Mensile"
	Protected WithEvents PNLCriteriMensili As System.Web.UI.WebControls.Panel
	Protected WithEvents RBCriterioMensile1 As System.Web.UI.WebControls.RadioButton
	Protected WithEvents TBGiorno1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents TBMese1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents RBCriterioMensile2 As System.Web.UI.WebControls.RadioButton
	Protected WithEvents DDLRicorrenzaMensile1 As System.Web.UI.WebControls.DropDownList
	Protected WithEvents DDLRicorrenzaMensile2 As System.Web.UI.WebControls.DropDownList
	Protected WithEvents TBRicorrenzaMensile As System.Web.UI.WebControls.TextBox
#End Region

#Region "Ripetizione Annuale"
	Protected WithEvents PNLCriteriAnnuali As System.Web.UI.WebControls.Panel

	Protected WithEvents DDLRicorrenzaAnnuale1 As System.Web.UI.WebControls.DropDownList
	Protected WithEvents DDLRicorrenzaAnnuale3 As System.Web.UI.WebControls.DropDownList
	Protected WithEvents DDLRicorrenzaAnnuale2 As System.Web.UI.WebControls.DropDownList
	Protected WithEvents RBLCriterioAnnuale1 As System.Web.UI.WebControls.RadioButton
	Protected WithEvents TBGiornoAnno As System.Web.UI.WebControls.TextBox
	Protected WithEvents RBCriterioAnnuale2 As System.Web.UI.WebControls.RadioButton
	Protected WithEvents DDLMeseAnno As System.Web.UI.WebControls.DropDownList
	'TBAnni1
#End Region



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
			Me.DDLCategoria.Attributes.Add("onchange", "return AggiornaForm();")
			Session("Azione") = "insert"
			SetupInternazionalizzazione()
		End If

		Try
			Dim oServizio As New UCServices.Services_Eventi
			Try
				oServizio.PermessiAssociati = Permessi(UCServices.Services_Eventi.Codex, Me.Page)
				If (oServizio.PermessiAssociati = "") Then
					oServizio.PermessiAssociati = "00000000000000000000000000000000"
				End If
			Catch ex As Exception
				oServizio.PermessiAssociati = "00000000000000000000000000000000"
			End Try

			Me.LNBcrea.Visible = True
			Me.LNBcreaNuovo.Visible = False
			If oServizio.AddEvents Or oServizio.AdminService Then
				If Not Page.IsPostBack Then
					PNLcontenuto.Visible = True
					PNLpermessi.Visible = False
					Me.Setup_EventiGenerici()
				End If
			Else
				If Not Page.IsPostBack Then
					PNLcontenuto.Visible = True
					PNLpermessi.Visible = False
					Me.Setup_EventiPersonali()
					Me.LBtitolo.Text = Me.oResource.getValue("LBtitoloPersonale.text")
				End If
			End If
			Me.LBdataI.Text = Me.HDNdataI.Value
			Me.LBdataF.Text = Me.HDNdataF.Value
		Catch ex As Exception
			Me.LNBcrea.Visible = False
			Me.LNBcreaNuovo.Visible = False
			PNLcontenuto.Visible = False
			PNLpermessi.Visible = True
			LBNopermessi.Visible = True
		End Try

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
			Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Me.Response.Redirect(PageUtility.GetDefaultLogoutPage, True)
            Return True
        Else
            Return False
		End If
	End Function
#Region "Bind"
	Private Sub Setup_EventiGenerici()
		Dim oDataI, oDataF As DateTime

		Me.TXBnote.Attributes.Add("onkeypress", "return(LimitText(this,4000));")
		Me.TXBprogramma.Attributes.Add("onkeypress", "return(LimitText(this,8000));")

		PNLMain.Visible = True
		If Request.QueryString("giorno") = "" Then
			oDataI = Date.Today
			oDataF = Date.Today
		Else
			Try
				oDataI = CStr(Request.QueryString("giorno"))
				oDataF = CStr(Request.QueryString("giorno"))
			Catch ex As Exception
				oDataI = Date.Today
				oDataF = Date.Today
			End Try
		End If
		Me.LBdataI.Text = oDataI.ToString("dd/MM/yyyy")
		Me.LBdataF.Text = oDataF.ToString("dd/MM/yyyy")
		Me.HDNdataF.Value = oDataI.ToString("dd/MM/yyyy")
		Me.HDNdataI.Value = oDataF.ToString("dd/MM/yyyy")

		BindDDLOre()

		If Request.QueryString("ora") <> "" Then
			Try
				DDLOra1.SelectedValue = CInt(Request.QueryString("ora").Substring(0, 2))
				DDLMinuti1.SelectedValue = CInt(Request.QueryString("ora").Substring(3, 2))
				DDLOra2.SelectedValue = CInt(Request.QueryString("ora").Substring(0, 2)) + 1
				DDLMinuti2.SelectedValue = CInt(Request.QueryString("ora").Substring(3, 2))
			Catch ex As Exception
				DDLOra1.SelectedValue = 8
				DDLMinuti1.SelectedValue = 0
				DDLOra2.SelectedValue = 9
				DDLMinuti2.SelectedValue = 0
			End Try
		Else
			DDLOra1.SelectedValue = 8
			DDLMinuti1.SelectedValue = 0
			DDLOra2.SelectedValue = 9
			DDLMinuti2.SelectedValue = 0
		End If

		Dim giorno As Date
		giorno = Date.Parse("01/01/" & Now.Year)

		If giorno.DayOfWeek = 0 Then
			giorno = giorno.AddDays(-6)
		Else
			giorno = giorno.AddDays(1 - giorno.DayOfWeek)
		End If
		Dim j As Integer
		DDLRicorrenzaMensile2.Items.Clear()
		DDLRicorrenzaAnnuale2.Items.Clear()
		For j = 0 To 6
			If j = 6 Then
				DDLRicorrenzaMensile2.Items.Add(New ListItem(giorno.AddDays(j).ToString("dddd", oResource.CultureInfo.DateTimeFormat), "7"))
				DDLRicorrenzaAnnuale2.Items.Add(New ListItem(giorno.AddDays(j).ToString("dddd", oResource.CultureInfo.DateTimeFormat), "7"))
			Else
				DDLRicorrenzaMensile2.Items.Add(New ListItem(giorno.AddDays(j).ToString("dddd", oResource.CultureInfo.DateTimeFormat), CInt(giorno.AddDays(j).DayOfWeek)))
				DDLRicorrenzaAnnuale2.Items.Add(New ListItem(giorno.AddDays(j).ToString("dddd", oResource.CultureInfo.DateTimeFormat), CInt(giorno.AddDays(j).DayOfWeek)))
			End If
		Next

		Dim i As Integer
		Dim giorno1 As Date

		giorno1 = Date.Parse("01/01/" & Now.Year)
		DDLMeseAnno.Items.Clear()
		DDLRicorrenzaAnnuale3.Items.Clear()

		For i = 0 To 11
			DDLMeseAnno.Items.Add(New ListItem(giorno1.ToString("MMMM", oResource.CultureInfo.DateTimeFormat), CStr(i + 1)))
			DDLRicorrenzaAnnuale3.Items.Add(New ListItem(giorno1.ToString("MMMM", oResource.CultureInfo.DateTimeFormat), CStr(i + 1)))
			giorno1 = giorno1.AddMonths(1)
		Next

		Me.Bind_CategorieEventi()
		Me.Bind_AnniAccademici()
		Me.Bind_Ripetizioni()
		If Me.RBLRipetizione.SelectedIndex = 0 Then
			Me.TBRripetizioni.Visible = False
		End If

        CTRLeditorProgramma.HTML = ""
        CTRLeditorNote.HTML = ""
        LBProgramma.Text = ""
        LBnote.Text = ""
		Try
			Me.Setup_Editor()
			If Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione Then
				Me.TBRnote.Visible = False
				Me.TBRprogramma.Visible = False
				Me.TBReditorProgramma.Visible = True
				Me.TBReditorNote.Visible = True
			Else
				Me.TBReditorProgramma.Visible = False
				Me.TBReditorNote.Visible = False
				Me.TBRnote.Visible = True
				Me.TBRprogramma.Visible = True
			End If
		Catch ex As Exception
			Me.TBReditorProgramma.Visible = False
			Me.TBReditorNote.Visible = False
			Me.TBRnote.Visible = True
			Me.TBRprogramma.Visible = True
		End Try
		Me.TBRannoAccademico.Visible = True
		Me.TBRmacro.Visible = True
		Me.TBRaula.Visible = True
	End Sub
	Private Sub Setup_EventiPersonali()
		Me.TXBnote.Attributes.Add("onkeypress", "return(LimitText(this,4000));")
		Me.TXBprogramma.Attributes.Add("onkeypress", "return(LimitText(this,8000));")

		PNLMain.Visible = True
		If Request.QueryString("giorno") = "" Then
			LBdataI.Text = Date.Today
			LBdataF.Text = Date.Today
		Else
			LBdataI.Text = CStr(Request.QueryString("giorno"))
			LBdataF.Text = CStr(Request.QueryString("giorno"))
		End If
		BindDDLOre()

		If Request.QueryString("ora") <> "" Then
			Try
				DDLOra1.SelectedValue = CInt(Request.QueryString("ora").Substring(0, 2))
				DDLMinuti1.SelectedValue = CInt(Request.QueryString("ora").Substring(3, 2))
				DDLOra2.SelectedValue = CInt(Request.QueryString("ora").Substring(0, 2)) + 1
				DDLMinuti2.SelectedValue = CInt(Request.QueryString("ora").Substring(3, 2))
			Catch ex As Exception
				DDLOra1.SelectedValue = 8
				DDLMinuti1.SelectedValue = 0
				DDLOra2.SelectedValue = 9
				DDLMinuti2.SelectedValue = 0
			End Try
		Else
			DDLOra1.SelectedValue = 8
			DDLMinuti1.SelectedValue = 0
			DDLOra2.SelectedValue = 9
			DDLMinuti2.SelectedValue = 0
		End If


		DDLCategoria.Items.Clear()
		DDLCategoria.Items.Add(New ListItem(oResource.getValue("Inserimento.EventoPersonale"), 0))
		DDLCategoria.Enabled = True
		Me.HDNselezionato.Value = 0

		Me.Bind_Ripetizioni()
		Me.Bind_TipoAvviso()

		Me.TBRannoAccademico.Visible = False
		Me.TBRmacro.Visible = False
		Me.TBRaula.Visible = False
		LBvisualizzazione.Visible = False
		CBVisibile.Visible = False

		Me.TBRripetizione.Visible = False
		RBLRipetizione.SelectedValue = 0

		If Me.RBLRipetizione.SelectedIndex = 0 Then
			Me.TBRripetizioni.Visible = False
		End If

		Me.TBReditorProgramma.Visible = False
		Me.TBReditorNote.Visible = False
		Me.TBRnote.Visible = True
		Me.TBRprogramma.Visible = False
		Me.TBRavvisoPersonale.Visible = True
	End Sub
	Private Sub Bind_Ripetizioni()
		RBLRipetizione.SelectedValue = 0
		RBCriteriGiornalieri1.Checked = True
		RBCriterioMensile1.Checked = True
		RBLCriterioAnnuale1.Checked = True
	End Sub
	Private Sub Bind_TipoAvviso()
		Dim oReminder As New COL_Reminder
		Try
			Dim oDataset As DataSet
			oDataset = oReminder.Get_Tipo_Avviso(Session("LinguaID"))
			DDLTipoAvviso.DataTextField = "TPAV_descrizione"
			DDLTipoAvviso.DataValueField = "TPAV_id"
			DDLTipoAvviso.DataSource = oDataset
			DDLTipoAvviso.DataBind()
			If oDataset.Tables(0).Rows.Count > 0 Then
				DDLTipoAvviso.Visible = True
				LBAvviso.Visible = True
				DDLTipoAvviso.Enabled = True
				DDLTipoAvviso.SelectedValue = "2"
			Else
				DDLTipoAvviso.Visible = False
				LBAvviso.Visible = False
			End If
		Catch ex As Exception
			DDLTipoAvviso.Items.Clear()
			DDLTipoAvviso.Items.Add(New ListItem(oResource.getValue("inserimento.nessuno"), 4))
			DDLTipoAvviso.Enabled = False
			DDLTipoAvviso.SelectedValue = "4"
		End Try
		DDLTipoAvviso.Visible = True
		LBAvviso.Visible = True
	End Sub
	Private Sub BindDDLOre()
		Dim i As Integer
		For i = 0 To 23
			DDLOra1.Items.Add(New ListItem(i.ToString("00"), i))
			DDLOra2.Items.Add(New ListItem(i.ToString("00"), i))
		Next
		i = 0
        For i = 0 To 55 Step 5
            DDLMinuti1.Items.Add(New ListItem(i.ToString("00"), i))
            DDLMinuti2.Items.Add(New ListItem(i.ToString("00"), i))
        Next
	End Sub

	Private Sub Bind_Orari()
		Dim i, j, t, l, z As Integer
        Dim arrMinuti() As Integer = {0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55}

		For z = 1 To 7
			Dim oDDLOraInizio As DropDownList
			Dim oDDLOraFine As DropDownList
			Dim oDDLMinutiInizio As DropDownList
            Dim oDDLMinutiFine As DropDownList
            'oLink = FindControlRecursive(Master, "LKB" & Lettera)
            oDDLOraInizio = Me.FindControlRecursive(Master, "DDLOraInizio" & z)
            oDDLOraFine = Me.FindControlRecursive(Master, "DDLOraFine" & z)
            oDDLMinutiInizio = Me.FindControlRecursive(Master, "DDLMinutiInizio" & z)
            oDDLMinutiFine = Me.FindControlRecursive(Master, "DDLMinutiFine" & z)
            'oDDLOraInizio = Me.FindControl("DDLOraInizio" & z)
            'oDDLOraFine = Me.FindControl("DDLOraFine" & z)
            'oDDLMinutiInizio = Me.FindControl("DDLMinutiInizio" & z)
            'oDDLMinutiFine = Me.FindControl("DDLMinutiFine" & z)
			oDDLOraInizio.Items.Clear()
			oDDLOraFine.Items.Clear()
			oDDLMinutiInizio.Items.Clear()
			oDDLMinutiFine.Items.Clear()
		Next

		Try
			For i = 0 To 23	'carico le ddl delle ore
				For j = 1 To 7
					Dim oDDLOraInizio As DropDownList
                    Dim oDDLOraFine As DropDownList

                    'oDDLOraInizio = Me.FindControl("DDLOraInizio" & j)
                    'oDDLOraFine = Me.FindControl("DDLOraFine" & j)
                    oDDLOraInizio = Me.FindControlRecursive(Master, "DDLOraInizio" & j)
                    oDDLOraFine = Me.FindControlRecursive(Master, "DDLOraFine" & j)
					If i < 10 Then
						oDDLOraInizio.Items.Add(New ListItem("0" & i, i))
						oDDLOraFine.Items.Add(New ListItem("0" & i, i))
					Else
						oDDLOraInizio.Items.Add(New ListItem(i, i))
						oDDLOraFine.Items.Add(New ListItem(i, i))
					End If
					If i = 23 Then
						oDDLOraInizio.SelectedValue = DDLOra1.SelectedValue
						oDDLOraFine.SelectedValue = DDLOra2.SelectedValue
					End If

				Next
			Next

			For t = 0 To 3 'carico le dll dei minuti
				For l = 1 To 7
					Dim oDDLMinutiInizio As DropDownList
					Dim oDDLMinutiFine As DropDownList
                    oDDLMinutiInizio = Me.FindControlRecursive(Master, "DDLMinutiInizio" & l)
                    oDDLMinutiFine = Me.FindControlRecursive(Master, "DDLMinutiFine" & l)
					oDDLMinutiInizio.Items.Add(New ListItem(arrMinuti(t).ToString("00"), arrMinuti(t)))
					oDDLMinutiFine.Items.Add(New ListItem(arrMinuti(t).ToString("00"), arrMinuti(t)))
					If t = 3 Then
						oDDLMinutiInizio.SelectedValue = DDLMinuti1.SelectedValue
						oDDLMinutiFine.SelectedValue = DDLMinuti2.SelectedValue
					End If

				Next
			Next
		Catch ex As Exception

		End Try

	End Sub
	Private Sub Bind_CategorieEventi()
		Dim oDataset As New DataSet
		Dim oTipoEvento As New COL_Tipo_Evento
		Try
			DDLCategoria.Items.Clear()
			oDataset = oTipoEvento.Elenca(CInt(Session("LinguaID")))
			If oDataset.Tables(0).Rows.Count > 0 Then
				Dim oListItem As New ListItem
				DDLCategoria.DataTextField = "TPEV_nome"
				DDLCategoria.DataValueField = "TPEV_id"
				DDLCategoria.DataSource = oDataset
				DDLCategoria.DataBind()
				DDLCategoria.Items.Add(New ListItem(oResource.getValue("Inserimento.EventoPersonale"), 0))
				DDLCategoria.Enabled = True
			Else
				DDLCategoria.Items.Add(New ListItem(oResource.getValue("Inserimento.EventoPersonale"), 0))
				DDLCategoria.Enabled = True
			End If
		Catch ex As Exception
			DDLCategoria.Items.Clear()
			DDLCategoria.Items.Add(New ListItem(oResource.getValue("Inserimento.EventoPersonale"), 0))
			DDLCategoria.Enabled = True
		End Try
		Try
			Me.HDNselezionato.Value = Me.DDLCategoria.SelectedValue
		Catch ex As Exception

		End Try
	End Sub
	Private Sub Bind_AnniAccademici(Optional ByVal AnnoAttuale As Integer = -1)
		Dim minAnno, maxAnno, i As Integer
		Dim DataAttuale As Date
		Try
			DDLAnnoAccademico.Items.Clear()

			Try
				DataAttuale = CDate(LBdataI.Text)
			Catch ex As Exception
				DataAttuale = Now
			End Try
			If Not (DataAttuale > Now) Then
				DataAttuale = Now
			End If
			If DataAttuale.Month >= 9 Then
				maxAnno = Year(DataAttuale) + 1
			Else
				maxAnno = Year(DataAttuale)
			End If
			minAnno = maxAnno - 10
			For i = minAnno To maxAnno
				DDLAnnoAccademico.Items.Insert(0, New ListItem(i - 1 & "-" & i, i - 1))
			Next
			If AnnoAttuale > 0 Then
				Try
					Me.DDLAnnoAccademico.SelectedValue = AnnoAttuale
				Catch ex As Exception

				End Try
			End If
		Catch ex As Exception

		End Try
	End Sub
#End Region

	Private Sub Setup_Editor()
		Dim oImpostazioni As New COL_ImpostazioniUtente
		Dim showEditor As Boolean = True 'False

		If Request.Browser.Browser = "IE" And Request.Browser.MajorVersion >= 5 Then
			showEditor = True
		End If
		Try
			If Not IsNothing(Session("oImpostazioni")) Then
				oImpostazioni = Session("oImpostazioni")
				showEditor = oImpostazioni.ApriEditorDiario
			End If
		Catch ex As Exception

		End Try
		If showEditor Then
			Me.LNBmodificaNote.Visible = False
			Me.LNBmodificaProgramma.Visible = False
			Me.LNBsalvaNote.Visible = False
			Me.LNBsalvaProgramma.Visible = False
            CTRLeditorProgramma.Visible = True
            CTRLeditorNote.Visible = True
			Me.TBLnote.Visible = False
			Me.TBLprogramma.Visible = False
		Else
			Me.LNBmodificaNote.Visible = True
			Me.LNBmodificaProgramma.Visible = True
			Me.LNBsalvaNote.Visible = False
			Me.LNBsalvaProgramma.Visible = False
            CTRLeditorProgramma.Visible = False
            CTRLeditorNote.Visible = False
			Me.TBLnote.Visible = True
			Me.TBLprogramma.Visible = True
		End If
	End Sub
#Region "Eventi cambio data e ora"
	Private Sub DDLCategoria_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLCategoria.SelectedIndexChanged
		If IsNothing(oResource) Then
			Me.SetCulture(Session("LinguaCode"))
		End If
		If DDLCategoria.SelectedValue = Main.TipiEvento.Reminder Then
			'reminder
			Me.TBRavvisoPersonale.Visible = True
			Me.TBRannoAccademico.Visible = False
			Me.TBRaula.Visible = False
			Me.TBRmacro.Visible = False

			Me.Bind_TipoAvviso()
			LBProgramma.Visible = False
			Me.TBRripetizione.Visible = False
			RBLRipetizione.SelectedValue = 0
			CBVisibile.Visible = False
			LBvisualizzazione.Visible = False
			Me.TBRnote.Visible = False
			Me.TBRprogramma.Visible = False
			Me.TBReditorProgramma.Visible = False
			Me.TBReditorNote.Visible = False
			Me.TBRripetizione.Visible = False
			Me.LBtitolo.Text = Me.oResource.getValue("LBtitoloPersonale.text")
		Else
			'evento
			Me.oResource.setLabel(Me.LBtitolo)
			Try
				If Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione And Me.TBReditorProgramma.Visible = False Then
					Me.LBProgramma.Text = Me.TXBprogramma.Text
					Me.LBnote.Text = Me.TXBnote.Text
                    CTRLeditorNote.HTML = Me.LBnote.Text
                    CTRLeditorProgramma.HTML = Me.LBProgramma.Text
					Me.Setup_Editor()
				ElseIf Me.TBReditorProgramma.Visible And Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione Then
					Me.TXBprogramma.Text = Me.LBProgramma.Text
					Me.TXBnote.Text = Me.LBnote.Text
				End If
				If Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione Then
					Me.TBRnote.Visible = False
					Me.TBRprogramma.Visible = False
					Me.TBReditorProgramma.Visible = True
					Me.TBReditorNote.Visible = True
				Else
					Me.TBReditorProgramma.Visible = False
					Me.TBReditorNote.Visible = False
					Me.TBRnote.Visible = True
					Me.TBRprogramma.Visible = True
				End If
			Catch ex As Exception
				Me.TBReditorProgramma.Visible = False
				Me.TBReditorNote.Visible = False
				Me.TBRnote.Visible = True
				Me.TBRprogramma.Visible = True
			End Try

			Me.TBRannoAccademico.Visible = True
			Me.TBRavvisoPersonale.Visible = False
			Me.TBRaula.Visible = True
			Me.TBRripetizione.Visible = True
			Me.TBRmacro.Visible = True

			LBProgramma.Visible = True
            CTRLeditorProgramma.Visible = True
			CBVisibile.Visible = True
			LBvisualizzazione.Visible = True

		End If
	End Sub

	Private Sub RBLRipetizione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLRipetizione.SelectedIndexChanged
		Me.TBRripetizioni.Visible = False
		Me.TBRore.Visible = False
		PNLCriteriGiornalieri.Visible = False
		PNLCriteriSettimanali.Visible = False
		PNLCriteriMensili.Visible = False
		PNLCriteriAnnuali.Visible = False
		Me.LBFineRpetizione.Visible = False
		Me.LBDataFine.Visible = False
		Select Case RBLRipetizione.SelectedIndex
			Case 0
				'Nessuna
				LBDataFine.Visible = True
				Me.TBRore.Visible = True
			Case 1
				'Giornaliera
				LBFineRpetizione.Visible = True
				PNLCriteriGiornalieri.Visible = True
				Me.TBRripetizioni.Visible = True
				Me.TBRore.Visible = True
			Case 2
				'Settimanale
				LBFineRpetizione.Visible = True
				PNLCriteriSettimanali.Visible = True
				Dim oCheckbox As New CheckBox
                oCheckbox = Me.FindControlRecursive(Master, "CBGiorno" & CInt(CDate(LBdataI.Text).DayOfWeek).ToString)
				oCheckbox.Checked = True
				Me.TBRripetizioni.Visible = True
				Bind_Orari()
			Case 3
				'Mensile
				LBFineRpetizione.Visible = True
				Me.TBRripetizioni.Visible = True
				Me.TBRore.Visible = True
				PNLCriteriMensili.Visible = True
				DDLRicorrenzaMensile2.SelectedValue = CInt(CDate(LBdataI.Text).DayOfWeek)
			Case 4
				'Annuale
				LBFineRpetizione.Visible = True
				Me.TBRripetizioni.Visible = True
				Me.TBRore.Visible = True
				PNLCriteriAnnuali.Visible = True
				TBGiornoAnno.Text = CDate(LBdataI.Text).Day
				DDLMeseAnno.SelectedValue = CDate(LBdataI.Text).Month
				DDLRicorrenzaAnnuale2.SelectedValue = CInt(CDate(LBdataI.Text).DayOfWeek)

			Case 5
				'Perpetua
				LBDataFine.Visible = True
				LBFineRpetizione.Visible = False
				Me.TBRripetizioni.Visible = False
				Me.TBRore.Visible = True
		End Select
	End Sub
#End Region

#Region "Pulsanti Menu"
	Private Sub LNBcalendario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcalendario.Click
		If Me.LNBcrea.Visible Then
			Response.Redirect("CalendarioSettimanale.aspx")
		End If
	End Sub
	Private Sub LNBcreaNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreaNuovo.Click
		Response.Redirect("InserimentoEvento.aspx")
	End Sub

	Private Sub LNBcrea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcrea.Click
		Try
			If Session("Azione") = "insert" Or CStr(Session("Azione")).Substring(0, 5) = "error" Then
				If CDate(LBdataF.Text) = CDate(LBdataI.Text) Then
					If (DDLOra2.SelectedIndex < DDLOra1.SelectedIndex) Or (DDLOra2.SelectedIndex = DDLOra1.SelectedIndex And DDLMinuti2.SelectedIndex < DDLMinuti1.SelectedIndex) Then
						Response.Write("<script language=Javascript>alert('" & Replace(Me.oResource.getValue("erroreData"), "'", "\'") & "');</script>")
					Else
						Me.InserisciEventi()
					End If
				Else
					Me.InserisciEventi()
				End If
			End If
		Catch ex As Exception

		End Try
	End Sub
#End Region

#Region "Inserimento eventi"

	Private Sub InserisciEventi()
		Dim isInseriti As Boolean = False
		Dim Perpetuo As Boolean = False
		Dim i, totale As Integer
		Dim StringaErrore As String
		Dim iResponse As Inserimento

		If TXBNomeEvento.Text.Trim.Length > 0 Then
			Try
				Dim oDataset As New DataSet
				Dim oEvento As New COL_Evento
				Dim oOrario As New COL_Orario
				Dim oProgrammaEvento As New COL_Programma_Evento
				Dim oComunita As New COL_Comunita
				Dim oTipoEvento As New COL_Tipo_Evento
				Dim oPersona As New COL_Persona
				oPersona = Session("objPersona")

				oDataset = Me.FillEventsData(Perpetuo)


				If oDataset.Tables(0).Rows.Count > 0 And Session("Azione") = "insert" Then

					oTipoEvento.Id = DDLCategoria.SelectedValue

					totale = oDataset.Tables(0).Rows.Count

					If totale > 0 Then
                        totale = totale - 1

                        Dim oServiceUtility As New EventNotificationUtility(Me.PageUtility)

						If DDLCategoria.SelectedValue <> 0 Then
							'sto inserendo un evento
							oComunita.Id = Session("idComunita")
							With oEvento
								.Comunita = oComunita
								.idPadre = 0
								.AnnoAccademico = Me.DDLAnnoAccademico.SelectedValue
								.Link = ""
								.Luogo = TBLuogo.Text
								.Macro = CBMacro.Checked
								.Nome = TXBNomeEvento.Text
								.Note = ""
								.TipoEvento = oTipoEvento
								.Visualizza = CBVisibile.Checked
								.Perpetuo = Perpetuo
								.idPersona = oPersona.Id
								Select Case RBLRipetizione.SelectedValue
									Case "0"
										.Ripeti = False
									Case "1" Or "2" Or "3" Or "4" Or "5"
										.Ripeti = True
								End Select
							End With
							oEvento.Aggiungi() 'aggiungo l'evento
							If oEvento.Errore = Errori_Db.None Then
								Dim NonInserite As Integer = 0

								isInseriti = True
								iResponse = Inserimento.EventoInserito
								For i = 0 To totale
									Dim oRow As DataRow

									oRow = oDataset.Tables(0).Rows(i)
									With oOrario
										.Evento.Id = oEvento.Id
										.Inizio = CDate(oRow.Item("Inizio"))
										.Fine = CDate(oRow.Item("Fine"))
										.Luogo = TBAula.Text
										.Link = TBLink.Text
										.Visibile = CBVisibile.Checked
										.idPersona = oPersona.Id

										If Me.TBReditorNote.Visible And Me.TBReditorProgramma.Visible Then
                                            If CTRLeditorProgramma.HTML = "" Then
                                                .Note = ""
                                            Else
                                                .Note = CTRLeditorProgramma.HTML
                                                .Note = Replace(.Note, "<script>", "&lt;script&gt;")
                                                .Note = Replace(.Note, "</script>", "&lt;/script&gt;")
                                                .Note = Replace(.Note, "<script", "&lt; script")
                                                .Note = Replace(.Note, "</script", "&lt;script")
                                            End If
										Else
											If Me.TXBnote.Text = "" Then
												.Note = ""
											Else
												.Note = Me.TXBnote.Text
												.Note = Replace(.Note, "<script>", "&lt;script&gt;")
												.Note = Replace(.Note, "</script>", "&lt;/script&gt;")
												.Note = Replace(.Note, "<script", "&lt; script")
												.Note = Replace(.Note, "</script", "&lt;script")
											End If
										End If
										If .Note <> "" Then
											.Note = Replace(.Note, "</pre>", "")

											If InStr(.Note, "<pre") > 0 Then
												Dim startPos, endPos As Integer

												While InStr(.Note, "<pre") > 0
													startPos = InStr(.Note, "<pre")
													endPos = InStr(startPos, .Note, ">")
													If startPos > 0 And endPos > 0 Then
														.Note = .Note.Remove(startPos - 1, endPos - startPos + 1)
													End If
												End While
											End If
										End If

									End With
									oOrario.Aggiungi()
                                    If oOrario.Errore = Errori_Db.None Then
                                        oServiceUtility.NotifyAddToCommunity(oComunita.Id, oOrario.Id, oOrario.Inizio, oOrario.Fine)
                                        With oProgrammaEvento
                                            If Me.TBReditorNote.Visible And Me.TBReditorProgramma.Visible Then
                                                If CTRLeditorProgramma.HTML = "" Then
                                                    .ProgrammaSvolto = ""
                                                Else
                                                    .ProgrammaSvolto = CTRLeditorProgramma.HTML
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script>", "&lt;script&gt;")
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script>", "&lt;/script&gt;")
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script", "&lt; script")
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script", "&lt;script")
                                                End If
                                            Else
                                                If Me.TXBprogramma.Text = "" Then
                                                    .ProgrammaSvolto = ""
                                                Else
                                                    .ProgrammaSvolto = Me.TXBprogramma.Text
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script>", "&lt;script&gt;")
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script>", "&lt;/script&gt;")
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script", "&lt; script")
                                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script", "&lt;script")
                                                End If
                                            End If
                                            If .ProgrammaSvolto <> "" Then
                                                .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</pre>", "")

                                                If InStr(.ProgrammaSvolto, "<pre") > 0 Then
                                                    Dim startPos, endPos As Integer

                                                    While InStr(.ProgrammaSvolto, "<pre") > 0
                                                        startPos = InStr(.ProgrammaSvolto, "<pre")
                                                        endPos = InStr(startPos, .ProgrammaSvolto, ">")
                                                        If startPos > 0 And endPos > 0 Then
                                                            .ProgrammaSvolto = .ProgrammaSvolto.Remove(startPos - 1, endPos - startPos + 1)
                                                        End If
                                                    End While
                                                End If
                                            End If
                                            .Id = oOrario.Id
                                        End With
                                        oProgrammaEvento.Aggiungi()
                                        If oProgrammaEvento.Errore <> Errori_Db.None Then
                                            iResponse = Inserimento.ErroreProgramma
                                        End If
                                    Else
                                        NonInserite += 1
                                    End If
								Next
								If NonInserite > 0 Then
									If totale = NonInserite Then
										oEvento.Cancella()
										iResponse = Inserimento.ErroreCreazione
									Else
										iResponse = Inserimento.ErroreCreazioneOrari
									End If
								End If
							Else
								isInseriti = False
								iResponse = Inserimento.ErroreCreazione
							End If
						Else
							'sto inserendo un evento personale
							Dim oReminder As New COL_Reminder
							For i = 0 To totale
								Dim oRow As DataRow

								oRow = oDataset.Tables(0).Rows(i)
								With oReminder
									.Inizio = CDate(oRow.Item("Inizio"))
									.Fine = CDate(oRow.Item("Fine"))
									.idPersona = oPersona.Id
									.idTipoAvviso = Me.DDLTipoAvviso.SelectedValue
									.Link = TBLink.Text
									.Luogo = TBLuogo.Text
									.Oggetto = TXBNomeEvento.Text
									.Sospeso = False
									If Me.TBReditorNote.Visible And Me.TBReditorProgramma.Visible Then
                                        If CTRLeditorNote.HTML = "" Then
                                            .Testo = ""
                                        Else
                                            .Testo = CTRLeditorNote.HTML
                                            .Testo = Replace(.Testo, "<script>", "&lt;script&gt;")
                                            .Testo = Replace(.Testo, "</script>", "&lt;/script&gt;")
                                            .Testo = Replace(.Testo, "<script", "&lt; script")
                                            .Testo = Replace(.Testo, "</script", "&lt;script")
                                        End If
									Else
										If Me.TXBnote.Text = "" Then
											.Testo = ""
										Else
											.Testo = Me.TXBnote.Text
											.Testo = Replace(.Testo, "<script>", "&lt;script&gt;")
											.Testo = Replace(.Testo, "</script>", "&lt;/script&gt;")
											.Testo = Replace(.Testo, "<script", "&lt; script")
											.Testo = Replace(.Testo, "</script", "&lt;script")
										End If
									End If
									.Creazione = Date.Today
								End With
							Next
							oReminder.Aggiungi(0, 0, 0)
                            If oReminder.Errore = Errori_Db.None Then
                                isInseriti = True
                                iResponse = Inserimento.EventoInserito
                            Else
                                isInseriti = False
                                iResponse = Inserimento.ErroreCreazionePersonale
                            End If
						End If
					Else
						isInseriti = False
						iResponse = Inserimento.ErroreCreazione
					End If
				Else
					isInseriti = False
					Try
						If Not CStr(Session("Azione")).Substring(0, 5) = "error" Then
							iResponse = Inserimento.DataMancante
						End If
					Catch ex As Exception
						iResponse = Inserimento.ErroreGenerico
					End Try
				End If

			Catch ex As Exception
				iResponse = Inserimento.ErroreGenerico
			End Try

			If isInseriti Then
				Me.LBinfo.CssClass = "avviso11_black"
				Me.PNLinfo.Visible = True
				Me.PNLcontenuto.Visible = False
				Me.LNBcrea.Visible = False
				Me.LNBcreaNuovo.Visible = True

				If CDate(LBdataI.Text).DayOfWeek = 0 Then
					Session("dtInizioSett") = CDate(LBdataI.Text).AddDays(-6)
				Else
					Session("dtInizioSett") = CDate(LBdataI.Text).AddDays(1 - CDate(LBdataI.Text).DayOfWeek)
				End If
				LBinfo.Text = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
				Session("Azione") = "loaded"
			Else
				Me.LBinfo.CssClass = "avviso11"
				If CStr(Session("Azione")).Substring(0, 5) = "error" Then
				Else
					StringaErrore = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
					If StringaErrore <> "" Then
						StringaErrore = Replace(StringaErrore, "'", "\'")
					End If
					Response.Write("<script language=Javascript>alert('" & StringaErrore & "');</script>")
					PNLcontenuto.Visible = True
					Me.LNBcrea.Visible = True
					Me.LNBcreaNuovo.Visible = False
				End If
			End If
		Else
			isInseriti = False
			RFVNomeEvento.IsValid = False
		End If
	End Sub
	Private Function FillEventsData(ByRef Perpetuo As Boolean) As DataSet
		Dim oDataset As DataSet
		Try

			Dim oTabellaOrari As New DataTable
			oDataset = CreaDatasetEventi()
			oTabellaOrari = oDataset.Tables(0)

			Dim oRiga As DataRow = oTabellaOrari.NewRow()

			Dim i, j As Integer


			Dim oArray() As DateTime
			Dim inizio, fine As DateTime
			inizio = Me.LBdataI.Text
			fine = Me.LBdataF.Text

			If (CDate(Me.LBdataI.Text & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue)) <= (CDate(Me.LBdataF.Text & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue)) Then
				oArray = GeneraStartArray()
				If IsArray(oArray) Then
					' While inizio <= fine
					For j = 0 To 6
						Dim oCheckbox As New CheckBox
						Dim oLBLerr As Label
                        oCheckbox = Me.FindControlRecursive(Master, "CBGiorno" & j)
						If oCheckbox.Checked = False And (Session("Azione") = "error" & j) Then
							Session("Azione") = "insert"
							If j = 0 Then
                                oLBLerr = Me.FindControlRecursive(Master, "LBLerrDLL" & 7)
							Else
                                oLBLerr = Me.FindControlRecursive(Master, "LBLerrDLL" & j)
							End If
							oLBLerr.Visible = False
						End If
					Next
					For i = 0 To UBound(oArray)
						Dim temp As DateTime
						temp = oArray(i)
						If temp <= fine Then
							oRiga = oTabellaOrari.NewRow()
							oRiga("Visibile") = True
							Select Case RBLRipetizione.SelectedValue
								Case "0", "5"

									Perpetuo = (RBLRipetizione.SelectedValue = "5")

									oRiga("Inizio") = oArray(i) & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue
									oRiga("Fine") = Me.LBdataF.Text & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue
									oTabellaOrari.Rows.Add(oRiga)
									Exit For
								Case "1" 'ripetizione giornaliera
									Perpetuo = False
									'If RBCriteriGiornalieri2.Checked Then
									'    TXBumeroGiorni.Text = 7
									'End If
									oRiga("Inizio") = oArray(i) & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue
									oRiga("Fine") = oArray(i) & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue
									oTabellaOrari.Rows.Add(oRiga)

								Case "2" 'settimanale
									Perpetuo = False
									Dim dataTemp As Date
									dataTemp = CDate(oArray(i))
									Dim oDDLoreInizio, oDDLMinutiInizio, oDDLoreFine, oDDLMinutiFine As DropDownList
									Dim oLBLerr As Label

									Try
										If dataTemp.DayOfWeek = 0 Then
                                            oDDLoreInizio = Me.FindControlRecursive(Master, "DDLOraInizio" & 7)
                                            oDDLMinutiInizio = Me.FindControlRecursive(Master, "DDLMinutiInizio" & 7)
                                            oDDLoreFine = Me.FindControlRecursive(Master, "DDLOraFine" & 7)
                                            oDDLMinutiFine = Me.FindControlRecursive(Master, "DDLMinutiFine" & 7)
                                            oLBLerr = Me.FindControlRecursive(Master, "LBLerrDLL" & 7)
										Else
                                            oDDLoreInizio = Me.FindControlRecursive(Master, "DDLOraInizio" & CInt(dataTemp.DayOfWeek))
                                            oDDLMinutiInizio = Me.FindControlRecursive(Master, "DDLMinutiInizio" & CInt(dataTemp.DayOfWeek))
                                            oDDLoreFine = Me.FindControlRecursive(Master, "DDLOraFine" & CInt(dataTemp.DayOfWeek))
                                            oDDLMinutiFine = Me.FindControlRecursive(Master, "DDLMinutiFine" & CInt(dataTemp.DayOfWeek))
                                            oLBLerr = Me.FindControlRecursive(Master, "LBLerrDLL" & CInt(dataTemp.DayOfWeek))
										End If

									Catch ex As Exception

									End Try
									oLBLerr.Visible = False

									If (oDDLoreFine.SelectedIndex < oDDLoreInizio.SelectedIndex) Or (oDDLoreFine.SelectedIndex = oDDLoreInizio.SelectedIndex And oDDLMinutiFine.SelectedIndex <= oDDLMinutiInizio.SelectedIndex) Then
										Try
											oLBLerr.Visible = True
											Session("Azione") = "error" & dataTemp.DayOfWeek
										Catch ex As Exception

										End Try
									Else
										If Session("Azione") = "error" & dataTemp.DayOfWeek Then
											Session("Azione") = "insert"
											oLBLerr.Visible = False
										End If
									End If

									oRiga("Inizio") = oArray(i) & " " & oDDLoreInizio.SelectedValue & ":" & oDDLMinutiInizio.SelectedValue
									oRiga("Fine") = oArray(i) & " " & oDDLoreFine.SelectedValue & ":" & oDDLMinutiFine.SelectedValue
									oTabellaOrari.Rows.Add(oRiga)
								Case "3" 'mensile
									Perpetuo = False
									oRiga("Inizio") = oArray(i) & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue
									oRiga("Fine") = oArray(i) & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue
									oTabellaOrari.Rows.Add(oRiga)
								Case "4" 'annuale
									Perpetuo = False
									oRiga("Inizio") = oArray(i) & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue
									oRiga("Fine") = oArray(i) & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue
									oTabellaOrari.Rows.Add(oRiga)

							End Select
							inizio = temp
						Else
							Exit For
							' Exit While
						End If
					Next
					' End While
				End If
			End If
		Catch ex As Exception

		End Try
		Return oDataset
	End Function

#End Region

#Region "Generazione Eventi"
	Private Function CreaDatasetEventi() As DataSet
		Try
			Dim oDataset As New DataSet("Evento")
			'creo nuova tabella orario
			Dim TableOrario As New DataTable("Orario")
			'creo e aggingo le colonne
			TableOrario.Columns.Add("Id", System.Type.GetType("System.Int32")) 'colonna che identifica l'elemento...
			TableOrario.Columns("Id").AutoIncrement = True '...come id univoco
			TableOrario.Columns.Add("Inizio", System.Type.GetType("System.String"))	'inizio
			TableOrario.Columns.Add("Fine", System.Type.GetType("System.String")) 'fine
			TableOrario.Columns.Add("Visibile", System.Type.GetType("System.Boolean"))
			oDataset.Tables.Add(TableOrario)
			Return oDataset	'ritorna il dataset vuoto appena creato
		Catch ex As Exception

		End Try

	End Function
	Private Function GeneraStartArray() As DateTime()
		Dim oArray() As DateTime = Nothing
		Dim GiornoSettimana, i, j, a, b, n As Integer
		GiornoSettimana = CDate(Me.LBdataI.Text).DayOfWeek
		Dim Inizio, Fine As DateTime
		Inizio = Me.LBdataI.Text
		Fine = Me.LBdataF.Text
		j = 0
		Select Case RBLRipetizione.SelectedValue
			Case "0", "5" 'NESSUNA o PERPETUA
				ReDim Preserve oArray(j)
				oArray(j) = Inizio
			Case "1" 'GIORNALIERA
				If Me.RBCriteriGiornalieri1.Checked Then
					'si tratta di tutti i giorni
					Dim numGiorni As Integer

					If Me.TXBnumeroGiorni.Text = "" Then
						numGiorni = 1
					Else
						If Me.TXBnumeroGiorni.Text.Trim = "" Then
							numGiorni = 1
						ElseIf IsNumeric(Me.TXBnumeroGiorni.Text) Then
							numGiorni = CInt(Me.TXBnumeroGiorni.Text)
						Else
							numGiorni = 1
						End If
					End If

					While Inizio <= Me.LBdataF.Text
						ReDim Preserve oArray(j)
						oArray(j) = Inizio
						j += 1
						Inizio = Inizio.AddDays(numGiorni)
						If Inizio > Me.LBdataF.Text Then
							Exit While
						End If
					End While

				ElseIf Me.RDBferiali.Checked Then ' RIPETIZIONE X GIORNI FERIALI
					While Inizio <= Me.LBdataF.Text
						For i = GiornoSettimana To (GiornoSettimana + 6)
							If (Weekday(Inizio, Microsoft.VisualBasic.FirstDayOfWeek.Monday) <> 7) Then
								ReDim Preserve oArray(j)
								oArray(j) = Inizio
								j += 1
								Inizio = Inizio.AddDays(1)
							Else
								Inizio = Inizio.AddDays(1)
							End If
							If Inizio > Me.LBdataF.Text Then
								Exit For
							End If
						Next
						If Inizio > Me.LBdataF.Text Then
							Exit While
						End If
					End While
				ElseIf Me.RDBfestivi.Checked Then ' RIPETIZIONE X GIORNI FESTIVI
					While Inizio <= Me.LBdataF.Text
						If (Weekday(Inizio, Microsoft.VisualBasic.FirstDayOfWeek.Monday) = 7) Then
							ReDim Preserve oArray(j)
							oArray(j) = Inizio
							j += 1
							Inizio = Inizio.AddDays(7)
						Else
							Inizio = Inizio.AddDays(1)
						End If

						If Inizio > Me.LBdataF.Text Then
							Exit While
						End If
					End While
				Else 'ripetizione ogni TOT giorni
					Dim numGiorni As Integer

					If Me.TXBnumeroGiorni.Text = "" Then
						numGiorni = 1
					Else
						If Me.TXBnumeroGiorni.Text.Trim = "" Then
							numGiorni = 1
						ElseIf IsNumeric(Me.TXBnumeroGiorni.Text) Then
							numGiorni = CInt(Me.TXBnumeroGiorni.Text)
						Else
							numGiorni = 1
						End If
					End If

					While Inizio <= Me.LBdataF.Text
						ReDim Preserve oArray(j)
						oArray(j) = Inizio
						j += 1
						Inizio = Inizio.AddDays(numGiorni)
						If Inizio > Me.LBdataF.Text Then
							Exit While
						End If
					End While
				End If
			Case "2" 'settimanle
				For i = GiornoSettimana To (GiornoSettimana + 6)
					Dim oCheckbox As New CheckBox
					'se la chechbox corrispondente al giorno della settimana è cekkata allora faccio l'inserimento
					If i > 6 Then
                        oCheckbox = Me.FindControlRecursive(Master, "CBGiorno" & (i - 7))
						If oCheckbox.Checked = True Then
							ReDim Preserve oArray(j)
							oArray(j) = Inizio
							j = j + 1
						End If
					Else
                        oCheckbox = Me.FindControlRecursive(Master, "CBGiorno" & i)
						If oCheckbox.Checked = True Then
							ReDim Preserve oArray(j)
							oArray(j) = Inizio
							j = j + 1
						End If
					End If
					Inizio = Inizio.AddDays(1) 'incremento di un giorno la data
					If Inizio > Me.LBdataF.Text Then
						Exit For
					End If
				Next

				Try
					Dim oArraySettimana() As DateTime = Nothing
					Dim TotaleGiorni, numSett, SettCorrente As Integer

					oArraySettimana = oArray
					TotaleGiorni = UBound(oArraySettimana)
					Inizio = oArray(0)
					numSett = Me.TXBnumeroSettimane.Text
					j = TotaleGiorni + 1
					SettCorrente = 1
					While Inizio <= Me.LBdataF.Text
						For i = 0 To TotaleGiorni
							Inizio = oArraySettimana(i).AddDays(7 * numSett * SettCorrente)
							If Inizio > Fine Then
								Exit While
							Else
								ReDim Preserve oArray(j)
								oArray(j) = Inizio
								j = j + 1
							End If
						Next
						SettCorrente += 1
						' numSett = numSett * SettCorrente

					End While
				Catch ex As Exception

				End Try


			Case "3" ' mensile
				If RBCriterioMensile2.Checked Then
					Dim anno, mese, giorno As Integer
					Dim primodelmesesuccessivo, ultimodelmese As String
					n = Me.DDLRicorrenzaMensile1.SelectedValue 'primo, secondo, terzo etc
					a = Me.DDLRicorrenzaMensile2.SelectedValue ' lun, mar, mer, gio, ven  
					While Inizio <= Fine
						b = 0
						ReDim Preserve oArray(j)
						If (Inizio.Month + 1) > 12 Then
							anno = (Inizio.Year + 1)
							mese = 1
						Else
							anno = Inizio.Year
							mese = (Inizio.Month + 1)
						End If
						primodelmesesuccessivo = "01" & "/" & mese & "/" & anno
						ultimodelmese = DateAdd("d", -1, primodelmesesuccessivo)
						giorno = 1
						For i = 1 To (7 * n) ' n è primo, secondo, terzo etc etc
							giorno = i
							If (giorno <= Day(ultimodelmese)) Then
								If Weekday(i & "/" & Inizio.Month & "/" & Inizio.Year, Microsoft.VisualBasic.FirstDayOfWeek.Monday) = a Then
									b = b + 1 ' contatore che dovrà essere = n
									oArray(j) = i & "/" & Inizio.Month & "/" & Inizio.Year
									If (b = n) Then
										Exit For
									End If
								End If
							Else : Exit For
							End If
						Next
						j = j + 1
						Inizio = Inizio.AddMonths(TBRicorrenzaMensile.Text)
					End While
				ElseIf Me.RBCriterioMensile1.Checked Then
					Dim numGiorni, addMesi As Integer
					Dim isFebbraio As Boolean = False
					Dim normalAdd As Boolean = True
					ReDim Preserve oArray(j)
					Dim temp As DateTime
					numGiorni = TBGiorno1.Text
					addMesi = Me.TBMese1.Text

					If numGiorni > 28 Then
						If Month(Inizio) = 2 Then
							If DateTime.IsLeapYear(Inizio.Year) And numGiorni = 29 Then
								oArray(0) = TBGiorno1.Text & "/" & Month(Me.LBdataI.Text) & "/" & Year(Me.LBdataI.Text)
							ElseIf DateTime.IsLeapYear(Inizio.Year) And numGiorni > 29 Then
								oArray(0) = "29/" & Month(Me.LBdataI.Text) & "/" & Year(Me.LBdataI.Text)
								normalAdd = False
							Else
								oArray(0) = "28/" & Month(Me.LBdataI.Text) & "/" & Year(Me.LBdataI.Text)
								normalAdd = False
							End If
						Else
							Try
								oArray(0) = TBGiorno1.Text & "/" & Month(Me.LBdataI.Text) & "/" & Year(Me.LBdataI.Text)
							Catch ex As Exception
								oArray(0) = TBGiorno1.Text - 1 & "/" & Month(Me.LBdataI.Text) & "/" & Year(Me.LBdataI.Text)
								normalAdd = False
							End Try
						End If
					Else
						oArray(0) = TBGiorno1.Text & "/" & Month(Me.LBdataI.Text) & "/" & Year(Me.LBdataI.Text)
					End If
					Inizio = oArray(0)


					j = j + 1
					i = 1
					If normalAdd Then
						While Inizio <= Fine
							temp = Inizio.AddMonths(addMesi * i)
							If temp <= Fine Then
								ReDim Preserve oArray(j)
								oArray(j) = temp
								j += 1
							Else

								Exit While
							End If
							i += 1
						End While
					Else
						Dim UltimoDelMese As DateTime
						While Inizio <= Fine
							temp = Inizio.AddMonths(addMesi * i)
							If temp.Day < TBGiorno1.Text Then
								UltimoDelMese = DateAdd("d", -1, "01" & "/" & temp.Month + 1 & "/" & temp.Year)
								If UltimoDelMese.Day < TBGiorno1.Text Then
									temp = UltimoDelMese
								ElseIf UltimoDelMese.Day = TBGiorno1.Text Then
									temp = UltimoDelMese
								ElseIf UltimoDelMese.Day > TBGiorno1.Text Then
									temp = UltimoDelMese.AddDays(-1)
								End If
							End If
							If temp <= Fine Then
								ReDim Preserve oArray(j)
								oArray(j) = temp
								j += 1
							Else
								Exit While
							End If
							i += 1
						End While
					End If
				End If
			Case "4" ' annuale
				If RBLCriterioAnnuale2.Checked Then
					Dim anno, mese, giorno As Integer
					Dim primodelmesesuccessivo, ultimodelmese As String
					n = Me.DDLRicorrenzaAnnuale1.SelectedValue 'primo, secondo, terzo etc
					a = Me.DDLRicorrenzaAnnuale2.SelectedValue ' lun, mar, mer, gio, ven  
					'  Inizio = TBGiornoAnno.Text & "/" & Me.DDLMeseAnno.SelectedValue & "/" & Year(Me.LBdataI.Text)

					While (Inizio) <= Me.LBdataF.Text
						b = 0
						ReDim Preserve oArray(j)
						If (DDLRicorrenzaAnnuale3.SelectedValue + 1) > 12 Then
							anno = (Inizio.Year + 1)
							mese = 1
						Else
							anno = Inizio.Year
							mese = (DDLRicorrenzaAnnuale3.SelectedValue + 1)
						End If
						primodelmesesuccessivo = "01" & "/" & mese & "/" & anno
						ultimodelmese = DateAdd("d", -1, primodelmesesuccessivo)
						giorno = 1
						For i = 1 To (7 * n) ' n è primo, secondo, terzo etc etc
							giorno = i
							If (giorno <= Day(ultimodelmese)) Then
								If Weekday(i & "/" & DDLRicorrenzaAnnuale3.SelectedValue & "/" & Inizio.Year, Microsoft.VisualBasic.FirstDayOfWeek.Monday) = a Then
									b = b + 1 ' contatore che dovrà essere = n
									oArray(j) = i & "/" & DDLRicorrenzaAnnuale3.SelectedValue & "/" & Inizio.Year
									If (b = n) Then
										Exit For
									End If
								End If
							Else : Exit For
							End If
						Next
						j = j + 1
						Inizio = Inizio.AddYears(TBAnni2.Text)
					End While
				Else
					ReDim Preserve oArray(j)
					'  oArray(j) = TBGiornoAnno.Text & "/" & Me.DDLMeseAnno.SelectedValue & "/" & Year(Me.LBdataI.Text)

					Dim numGiorni, addAnni, numMese As Integer
					ReDim Preserve oArray(j)
					Dim temp As DateTime
					numGiorni = TBGiornoAnno.Text
					numMese = Me.DDLMeseAnno.SelectedValue
					addAnni = Me.TBAnni1.Text

					If numGiorni > 28 Then
						If Month(Inizio) = 2 Then
							temp = "1/2/" & Inizio.Year
							If DateTime.IsLeapYear(temp.Year) And numGiorni = 29 Then
								oArray(0) = TBGiorno1.Text & "/" & numMese & "/" & Year(Inizio)
							ElseIf DateTime.IsLeapYear(temp.Year) And numGiorni > 29 Then
								oArray(0) = "29/" & numMese & "/" & Year(Me.LBdataI.Text)
							Else
								oArray(0) = "28/" & numMese & "/" & Year(Me.LBdataI.Text)
							End If
						Else
							Try
								oArray(0) = TBGiornoAnno.Text & "/" & numMese & "/" & Year(Inizio)
							Catch ex As Exception
								oArray(0) = TBGiornoAnno.Text - 1 & "/" & numMese & "/" & Year(Inizio)

							End Try
						End If
					Else
						oArray(0) = TBGiornoAnno.Text & "/" & numMese & "/" & Year(Inizio)
					End If
					Inizio = oArray(0)

					j = j + 1
					i = 1
					While Inizio <= Fine
						temp = Inizio.AddYears(addAnni * i)
						If temp <= Fine Then
							ReDim Preserve oArray(j)
							oArray(j) = temp
							j += 1
						Else

							Exit While
						End If
						i += 1
					End While
				End If
		End Select
		Return oArray
	End Function
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_InserimentoEvento"
        oResource.Folder_Level1 = "Eventi"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(LBtitolo)
            .setLabel(LBNomeEvento)
            .setRequiredFieldValidator(RFVNomeEvento, True, False)
            .setRegularExpressionValidator(REVNomeEvento)
            .setLabel(LBCategoria)
            .setLabel(LBdataInizio)
            .setLabel(LBDataFine)
            .setLabel(LBFineRpetizione)
            .setLabel(LBprogrammaNormale_t)
            .setLabel(LBnoteNormale_t)
            .setLabel(LBLOraInizio)
            .setLabel(LBLOraFine)
            .setLabel(LBRipetizione)
            .setRadioButtonList(RBLRipetizione, "0")
            .setRadioButtonList(RBLRipetizione, "1")
            .setRadioButtonList(RBLRipetizione, "2")
            .setRadioButtonList(RBLRipetizione, "3")
            .setRadioButtonList(RBLRipetizione, "4")
            .setRadioButtonList(RBLRipetizione, "5")
            RBCriteriGiornalieri1.Text = .getValue("RBCriteriGiornalieri1.text")
            .setLabel(LBCGgiorno)
            .setRequiredFieldValidator(RFVnumeroGiorni, False, True)
            .setRangeValidator(RVnumeroGiorni)
            RDBferiali.Text = .getValue("RDBferiali.text")
            RDBfestivi.Text = .getValue("RDBfestivi.text")
            .setLabel(LBCSOgni)
            .setLabel(LBCSSettimane)
            .setRequiredFieldValidator(RFVnumeroSettimane, False, True)
            .setRangeValidator(RVnumeroSettimane)
            .setLabel(LBdal)
            .setLabel(LBal)
            .setLabel_To_Value(LBLCSLunedi, "weekday.1")
            .setLabel_To_Value(LBLCSMartedi, "weekday.2")
            .setLabel_To_Value(LBLCSMercoledi, "weekday.3")
            .setLabel_To_Value(LBLCSGiovedi, "weekday.4")
            .setLabel_To_Value(LBLCSVenerdi, "weekday.5")
            .setLabel_To_Value(LBLCSSabato, "weekday.6")
            .setLabel_To_Value(LBLCSDomenica, "weekday.0")
            RBCriterioMensile1.Text = .getValue("RBCriterioMensile1.text")
            .setRequiredFieldValidator(RfvCMnumeroGiorni, False, True)
            .setRangeValidator(RVCMNumeroGiorni)
            .setLabel(LBLCMOgni)
            .setLabel(LBLCMMesi)
            .setRequiredFieldValidator(RFVCMNumeroMesi, False, True)
            .setRangeValidator(RVCMNumeroMesi)
            .setDropDownList(DDLRicorrenzaMensile1, "1")
            .setDropDownList(DDLRicorrenzaMensile1, "2")
            .setDropDownList(DDLRicorrenzaMensile1, "3")
            .setDropDownList(DDLRicorrenzaMensile1, "4")
            .setDropDownList(DDLRicorrenzaMensile1, "5")
            .setLabel(LBLCMOgni2)
            .setLabel(LBLCMMesi2)
            RBLCriterioAnnuale1.Text = .getValue("RBLCriterioAnnuale1.text")
            .setLabel(LBLCAOgni)
            .setLabel(LBLCAAnni)
            RBLCriterioAnnuale2.Text = .getValue("RBLCriterioAnnuale2.text")
            RBCriterioMensile2.Text = .getValue("RBLCriterioAnnuale2.text")
            .setDropDownList(DDLRicorrenzaAnnuale1, "1")
            .setDropDownList(DDLRicorrenzaAnnuale1, "2")
            .setDropDownList(DDLRicorrenzaAnnuale1, "3")
            .setDropDownList(DDLRicorrenzaAnnuale1, "4")
            .setDropDownList(DDLRicorrenzaAnnuale1, "5")
            .setLabel(LBLCAdi)
            .setLabel(LBLCAdiOgni)
            .setLabel(LBLCAanno2)
            .setLabel(LBLuogo)
            .setRegularExpressionValidator(REVLuogo)
            .setLabel(LBAula)
            .setRegularExpressionValidator(REVAula)
            .setLabel(LBnote_t)
            .setLabel(LBProgramma_t)
            .setLabel(LBLink)
            .setLabel(LBAnnoAccademico)
            .setLabel(LBAvviso)
            .setLabel(LBvisualizzazione)
            .setLinkButton(Me.LNBcalendario, True, True)
            .setLinkButton(Me.LNBcrea, True, True)
            .setLinkButton(Me.LNBcreaNuovo, True, True)
            .setLabel(LBMacro)
            .setCheckBox(CBVisibile)
            .setCheckBox(CBMacro)
            .setLinkButton(Me.LNBmodificaNote, True, True)
            .setLinkButton(Me.LNBmodificaProgramma, True, True)
            .setLinkButton(Me.LNBsalvaNote, True, True)
            .setLinkButton(Me.LNBsalvaProgramma, True, True)
        End With
    End Sub
#End Region

    Private Sub LNBsalvaNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaNote.Click
        Dim note As String = ""
        note = CTRLeditorNote.HTML

        note = Replace(note, "<script>", "&lt;script&gt;")
        note = Replace(note, "</script>", "&lt;/script&gt;")
        note = Replace(note, "<script", "&lt; script")
        note = Replace(note, "</script", "&lt;script")
        note = Replace(note, "</pre>", "")

        If InStr(note, "<pre") > 0 Then
            Dim startPos, endPos As Integer

            While InStr(note, "<pre") > 0
                startPos = InStr(note, "<pre")
                endPos = InStr(startPos, note, ">")
                If startPos > 0 And endPos > 0 Then
                    note = note.Remove(startPos - 1, endPos - startPos + 1)
                End If
            End While
        End If
        If IsNothing(note) Then
            note = ""
        Else
            If note.Length > 4000 Then
                note = Left(note, 4000)
            End If
        End If
        Me.LBnote.Text = note
        Me.LNBsalvaNote.Visible = False
        Me.LNBmodificaNote.Visible = True
        CTRLeditorNote.Visible = False
        CTRLeditorNote.IsEnabled = False
        Me.TBLnote.Visible = True
    End Sub
    Private Sub LNBmodificaNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodificaNote.Click
        CTRLeditorNote.IsEnabled = True
        CTRLeditorNote.HTML = Me.LBnote.Text
        Me.LNBsalvaNote.Visible = True
        Me.LNBmodificaNote.Visible = False
        Me.TBLnote.Visible = False
        CTRLeditorNote.Visible = True
    End Sub
    Private Sub LNBsalvaProgramma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaProgramma.Click
        Dim programmaSvolto As String = ""
        programmaSvolto = CTRLeditorProgramma.HTML

        If programmaSvolto <> "" Then
            programmaSvolto = Replace(programmaSvolto, "<script>", "&lt;script&gt;")
            programmaSvolto = Replace(programmaSvolto, "</script>", "&lt;/script&gt;")
            programmaSvolto = Replace(programmaSvolto, "<script", "&lt; script")
            programmaSvolto = Replace(programmaSvolto, "</script", "&lt;script")
        End If


        programmaSvolto = Replace(programmaSvolto, "</pre>", "")
        If InStr(programmaSvolto, "<pre") > 0 Then
            Dim startPos, endPos As Integer

            While InStr(programmaSvolto, "<pre") > 0
                startPos = InStr(programmaSvolto, "<pre")
                endPos = InStr(startPos, programmaSvolto, ">")
                If startPos > 0 And endPos > 0 Then
                    programmaSvolto = programmaSvolto.Remove(startPos - 1, endPos - startPos + 1)
                End If
            End While
        End If
        If IsNothing(programmaSvolto) Then
            programmaSvolto = ""
        End If
        Me.LBProgramma.Text = programmaSvolto
        Me.LNBsalvaProgramma.Visible = False
        Me.LNBmodificaProgramma.Visible = True
        CTRLeditorProgramma.Visible = False
        CTRLeditorProgramma.IsEnabled = False
        Me.TBLprogramma.Visible = True
    End Sub
    Private Sub LNBmodificaProgramma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodificaProgramma.Click
        CTRLeditorProgramma.IsEnabled = True
        CTRLeditorProgramma.HTML = Me.LBProgramma.Text
        Me.LNBsalvaProgramma.Visible = True
        Me.LNBmodificaProgramma.Visible = False
        Me.TBLprogramma.Visible = False
        CTRLeditorProgramma.Visible = True
    End Sub


    Public ReadOnly Property CalendarScript As String
        Get
            Dim Var As String

            Try
                Select Case Session("LinguaCode")
                    Case "it-IT"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-it.js" & """" & "></script>"
                    Case "en-US"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
                    Case "de-DE"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-de.js" & """" & "></script>"
                    Case "fr-FR"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-fr.js" & """" & "></script>"
                    Case "es-ES"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-es.js" & """" & "></script>"
                    Case Else
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
                End Select
            Catch ex As Exception
                Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
            End Try
            Return Var
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
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

End Class


'<%
'		try
'			Select Case Session("LinguaCode")
'                Case "it-IT"
'                    response.write("<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-it.js" & """" &"></script>")
'                Case "en-US"
'                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
'                Case "de-DE"
'                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-de.js" & """" &"></script>")
'                Case "fr-FR"
'                   response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-fr.js" & """" &"></script>")
'                Case "es-ES"
'                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-es.js" & """" &"></script>")
'                Case Else
'                  response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
'            End Select
'		catch ex as exception
'			response.write("<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
'		end try%>