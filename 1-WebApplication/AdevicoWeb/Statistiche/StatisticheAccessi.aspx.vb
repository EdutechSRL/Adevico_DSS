Imports Telerik
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Statistiche
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Eventi

Public Class StatisticheAccessi
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager
    Private _IdModule As Integer
    Private _PageUtility As OLDpageUtility

    Private Function GetIdModule() As Integer
        If _IdModule = 0 Then
            Dim manager As New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext)
            _IdModule = manager.GetModuleID(lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.UniqueId)
        End If
    End Function
    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            PageUtility = _PageUtility
        End Get
    End Property
    Private Enum Statistiche
        giornaliera = 0
        settimanale = 1
        mensile = 2
        annuale = 3
        iscritti = 4
        confronto = 5
    End Enum

    Protected WithEvents RBLvisualizzaAltro As RadioButtonList
    'Protected WithEvents RBLvisualizzaAltroIscritti As RadioButtonList

#Region "Pannello Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
#End Region
#Region "Statistiche"
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table

    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip

    Protected WithEvents LBL_TableData As System.Web.UI.WebControls.Label
    Protected WithEvents LNK_DownloadData As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRData As System.Web.UI.WebControls.TableRow

#Region "Filtro"
    Protected WithEvents TBRFiltri As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBnoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotale As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLchart As Telerik.WebControls.RadChart
    Protected WithEvents DDLgiorno As System.Web.UI.WebControls.DropDownList
    Protected WithEvents IMBapriInizio As System.Web.UI.WebControls.ImageButton

    Protected WithEvents HDNDataMin As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNDataMax As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNdataI As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents LBL_AnnoConf As System.Web.UI.WebControls.Label

    'Protected WithEvents RBLvisualizzaAltro As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBLOpzioni As System.Web.UI.WebControls.Label
    Protected WithEvents RBLtipoChart As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents CBmostraLabel As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBautopostback As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNesegui As System.Web.UI.WebControls.Button

    Protected WithEvents LBorganizzazione_t As System.Web.UI.WebControls.Label

#End Region

#Region "Date"
    Protected WithEvents DDLVaiA_anni As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNVaiA_oggi As System.Web.UI.WebControls.Button
    Protected WithEvents LBdataI As System.Web.UI.WebControls.Label

    Protected WithEvents LBL_Data_t As System.Web.UI.WebControls.Label

    Protected WithEvents DDLmese As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLanno As System.Web.UI.WebControls.DropDownList
#End Region

#Region "Pannelli"
    Protected WithEvents PNLStat_Giorno As System.Web.UI.WebControls.Panel
    Protected WithEvents LBtotaleG As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_giorno_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_giorno As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_settimana_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_settimana As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_mese_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_mese As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_anno_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtotaleG_anno As System.Web.UI.WebControls.Label

    'Protected WithEvents LBtotaleSett As System.Web.UI.WebControls.Label
    'Protected WithEvents PNLStat_Settimana As System.Web.UI.WebControls.Panel

    'Protected WithEvents PNLStat_Mese As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtotaleMens As System.Web.UI.WebControls.Label

    'Protected WithEvents PNLStat_Anno As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtotaleAnn As System.Web.UI.WebControls.Label

    Protected WithEvents PNLRuoli As System.Web.UI.WebControls.Panel
    Protected WithEvents LBruoli_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBtutti As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBLtipoPersona As System.Web.UI.WebControls.CheckBoxList

    Protected WithEvents PNLAnniConf As System.Web.UI.WebControls.Panel
    Protected WithEvents CBL_AnnoConf As System.Web.UI.WebControls.CheckBoxList

    Protected WithEvents PNLIscrittiIscrizioni As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLorgnIscritti As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBtuttiIscritti As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBLtipoPersonaIscritti As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents RBLvisualizzaIscrizioni As System.Web.UI.WebControls.RadioButtonList
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
        If Page.IsPostBack = False Then
            Me.SetupLingua()
        End If
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
            Me.TBSmenu.SelectedIndex = 0

            Session("tab") = "giornaliera"

            ShowContent()
            Me.Bind_Filtri()
            Bind_Organizzazione()
            Me.BindGrafici()

        End If
        If Me.HDNdataI.Value <> "" Then
            If LBdataI.Text <> HDNdataI.Value AndAlso Page.IsPostBack AndAlso TBSmenu.SelectedIndex = 0 AndAlso CBautopostback.Checked Then
                Me.SetDateDayWeek()
                SetUpDati(Statistiche.giornaliera)
                BindGrafici()
            Else
                LBdataI.Text = Me.HDNdataI.Value
            End If
        End If

            Me.Page.Form.DefaultButton = Me.BTNesegui.UniqueID
            Me.Page.Form.DefaultFocus = Me.BTNesegui.UniqueID
            Me.Master.Page.Form.DefaultButton = Me.BTNesegui.UniqueID
            Me.Master.Page.Form.DefaultFocus = Me.BTNesegui.UniqueID
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
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Return True
        End If
        Return False
    End Function

#Region "Localizzazione"
    Private Sub SetupLingua()
        Try
            If IsNumeric(Session("LinguaID")) And Session("LinguaCode") <> "" Then

            Else
                Dim LinguaCode As String
                LinguaCode = "it-IT"
                Try
                    LinguaCode = Request.UserLanguages(0)
                Catch ex As Exception
                    LinguaCode = "it-IT"
                End Try
                If Request.Browser.Cookies = True Then
                    Try
                        LinguaCode = Request.Cookies("LinguaCode").Value
                    Catch ex As Exception

                    End Try
                End If
				Dim oLingua As New Lingua
				oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
				If Not IsNothing(oLingua) Then
					Session("LinguaID") = oLingua.Id
					Session("LinguaCode") = oLingua.Codice
				Else
					Session("LinguaID") = 1
					Session("LinguaCode") = "it-IT"
				End If
			End If
			SetCulture(Session("LinguaCode"))
			SetupInternazionalizzazione()
		Catch exUserLanguages As Exception
		End Try
    End Sub

	Private Sub SetCulture(ByVal Code As String)
		oResource = New ResourceManager
		oResource.UserLanguages = Code
		oResource.ResourcesName = "pg_statistiche_new"
		oResource.Folder_Level1 = "Statistiche"
		oResource.setCulture()
    End Sub

	Private Sub SetupInternazionalizzazione()
		With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")

            TBSmenu.Tabs(0).Text = .getValue("TABgiorno.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("TABgiorno.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("TABsettimanale.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("TABsettimanale.ToolTip")
            TBSmenu.Tabs(2).Text = .getValue("TABmensile.Text")
            TBSmenu.Tabs(2).ToolTip = .getValue("TABmensile.ToolTip")
            TBSmenu.Tabs(3).Text = .getValue("TABannuale.Text")
            TBSmenu.Tabs(3).ToolTip = .getValue("TABannuale.ToolTip")
            TBSmenu.Tabs(4).Text = .getValue("TABiscritti.Text")
            TBSmenu.Tabs(4).ToolTip = .getValue("TABiscritti.ToolTip")

			.setLabel(Me.LBtotaleG_anno_t)
			.setLabel(Me.LBtotaleG_mese_t)
			.setLabel(Me.LBtotaleG_settimana_t)
			.setLabel(Me.LBtotaleG_giorno_t)
			.setLabel(Me.LBtotaleG)

			.setLabel(LBNopermessi)
			.setLabel(LBruoli_t)
			.setCheckBox(CBtutti)

			.setLabel(Me.LBLOpzioni)

			.setCheckBox(CBmostraLabel)
			.setCheckBox(CBautopostback)
			.setButton(BTNesegui)

			.setLabel(LBorganizzazione_t)

			.setLabel(LBL_AnnoConf)

			.setCheckBox(CBtuttiIscritti)
			.setCheckBox(Me.CBtutti)

			.setRadioButtonList(RBLvisualizzaIscrizioni, 0)
			.setRadioButtonList(RBLvisualizzaIscrizioni, 1)

			.setRadioButtonList(Me.RBLtipoChart, 0)
			.setRadioButtonList(Me.RBLtipoChart, 1)

			.setRadioButtonList(Me.RBLvisualizzaAltro, 0)
			.setRadioButtonList(Me.RBLvisualizzaAltro, 1)

			'.setRadioButtonList(Me.RBLvisualizzaAltroIscritti, 0)
			'.setRadioButtonList(Me.RBLvisualizzaAltroIscritti, 1)

			.setRadioButtonList(RBLvisualizzaIscrizioni, 0)
			.setRadioButtonList(RBLvisualizzaIscrizioni, 1)

			.setLinkButton(LNK_DownloadData, True, True, False, False)

		End With
	End Sub
#End Region

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        ShowContent()
        Me.SetDateDayWeek()
        Me.BindGrafici()
    End Sub

#Region "Filtri"
#Region "Setup"
	Public Sub SetUpDati(ByVal posizione As Integer)
		'las sessione indica dov'ero
		'posizione indica dove sono
		If IsNothing(oResource) Then
			SetCulture(Session("LinguaCode"))
		End If
		Dim giorno, mese, anno As Integer
		If Session("tab") = "giornaliera" Then
			Try
				anno = Year(CDate(Me.HDNdataI.Value))
				mese = Month(CDate(Me.HDNdataI.Value))
				giorno = Day(CDate(Me.HDNdataI.Value))
			Catch ex As Exception
				Me.HDNdataI.Value = Now.Day & "/" & Now.Month & "/" & Now.Year
				anno = Now.Year
				mese = Now.Month
				giorno = Now.Day
			End Try
			Select Case posizione
				Case Statistiche.settimanale
					Me.Setup_FiltriSettimanali(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.mensile
					Me.Setup_FiltriMensili(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)
				Case Statistiche.annuale
					Me.Setup_FiltriAnnuali(giorno, mese, Year(CDate(Me.HDNdataI.Value)), Me.CBmostraLabel.Checked, CBtutti.Checked, Me.CBLtipoPersona)
				Case Statistiche.iscritti
					Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
					Me.LBdataI.Text = giorno & "/" & mese & "/" & anno
					Me.DDLanno.SelectedValue = anno
					If Not Me.RBLvisualizzaIscrizioni.SelectedValue = "1" Then
						Me.Bind_TipoIscritti()
					End If
			End Select

		ElseIf Session("tab") = "settimanale" Then
			anno = Year(CDate(Me.HDNdataI.Value))
			mese = Month(CDate(Me.HDNdataI.Value))
			giorno = Day(CDate(Me.HDNdataI.Value))

			Select Case posizione
				Case Statistiche.giornaliera
					Me.Setup_FiltriGiornalieri(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, False, Me.CBLtipoPersona)

				Case Statistiche.mensile
					Me.Setup_FiltriMensili(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.annuale
					Me.Setup_FiltriAnnuali(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.iscritti
					Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
					Me.LBdataI.Text = giorno & "/" & mese & "/" & anno
					Me.DDLanno.SelectedValue = anno

					If Not Me.RBLvisualizzaIscrizioni.SelectedValue = "1" Then
						Me.Bind_TipoIscritti()
					End If
			End Select

		ElseIf Session("tab") = "mensile" Then
			anno = Me.DDLanno.SelectedValue
			mese = Me.DDLmese.SelectedValue
			giorno = Day(CDate(Me.HDNdataI.Value))

			Select Case posizione
				Case Statistiche.giornaliera

					Me.Setup_FiltriGiornalieri(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, False, Me.CBLtipoPersona)

				Case Statistiche.settimanale
					Me.Setup_FiltriSettimanali(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.annuale
					Me.Setup_FiltriAnnuali(giorno, mese, Me.DDLanno.SelectedValue, Me.CBmostraLabel.Checked, CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.iscritti
					Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
					Me.LBdataI.Text = giorno & "/" & mese & "/" & anno
					Me.DDLanno.SelectedValue = anno

					If Not Me.RBLvisualizzaIscrizioni.SelectedValue = "1" Then
						Me.Bind_TipoIscritti()
					End If

			End Select

		ElseIf Session("tab") = "annuale" Then
			anno = Me.DDLanno.SelectedValue
			mese = Month(CDate(Me.HDNdataI.Value))
			giorno = Day(CDate(Me.HDNdataI.Value))

			Select Case posizione
				Case Statistiche.giornaliera
					Me.Setup_FiltriGiornalieri(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, False, Me.CBLtipoPersona)

				Case Statistiche.settimanale
					Me.Setup_FiltriSettimanali(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.mensile
					Me.Setup_FiltriMensili(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.iscritti
					Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
					Me.LBdataI.Text = giorno & "/" & mese & "/" & anno
					Me.DDLanno.SelectedValue = anno

					If Not Me.RBLvisualizzaIscrizioni.SelectedValue = "1" Then
						Me.Bind_TipoIscritti()
					End If

			End Select

		ElseIf Session("tab") = "iscritti" Then
			anno = Me.DDLanno.SelectedValue
			mese = Month(CDate(Me.HDNdataI.Value))
			giorno = Day(CDate(Me.HDNdataI.Value))

			Select Case posizione
				Case Statistiche.giornaliera
					Me.Setup_FiltriGiornalieri(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, False, Me.CBLtipoPersona)

				Case Statistiche.settimanale
					Me.Setup_FiltriSettimanali(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.mensile
					Me.Setup_FiltriMensili(giorno, mese, anno, Me.CBmostraLabel.Checked, Me.CBtutti.Checked, Me.CBLtipoPersona)

				Case Statistiche.annuale
					Me.Setup_FiltriAnnuali(giorno, mese, anno, Me.CBmostraLabel.Checked, CBtutti.Checked, Me.CBLtipoPersona)
			End Select
		End If
	End Sub

	Private Sub Setup_FiltriGiornalieri(ByVal giorno As Integer, ByVal mese As Integer, ByVal anno As Integer, ByVal ShowLabels As Boolean, ByVal ShowAll As Boolean, ByVal AutoPost As Boolean, ByVal oCBLtipoPersona As CheckBoxList)
		Dim oData As New DateTime

		Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
		Me.LBdataI.Text = giorno & "/" & mese & "/" & anno

		Me.CBmostraLabel.Checked = ShowLabels
		Me.CBtutti.Checked = ShowAll

		If ShowAll Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
		ElseIf oCBLtipoPersona.SelectedIndex = -1 Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
			Me.CBtutti.Checked = True
		Else
			Dim i, totale As Integer
			totale = oCBLtipoPersona.Items.Count - 1
			For i = 0 To totale
				Me.CBLtipoPersona.Items(i).Selected = oCBLtipoPersona.Items(i).Selected
			Next
			Me.CBLtipoPersona.Enabled = True
		End If
	End Sub

	Private Sub Setup_FiltriSettimanali(ByVal giorno As Integer, ByVal mese As Integer, ByVal anno As Integer, ByVal ShowLabels As Boolean, ByVal ShowAll As Boolean, ByVal oCBLtipoPersona As CheckBoxList)

		Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
		Me.LBdataI.Text = giorno & "/" & mese & "/" & anno

		Me.CBmostraLabel.Checked = ShowLabels
		Me.CBtutti.Checked = ShowAll

		If ShowAll Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
		ElseIf oCBLtipoPersona.SelectedIndex = -1 Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
			Me.CBtutti.Checked = True
		Else
			Dim i, totale As Integer
			totale = oCBLtipoPersona.Items.Count - 1
			For i = 0 To totale
                Me.CBLtipoPersona.Items(i).Selected = oCBLtipoPersona.Items(i).Selected
                Me.SetDateDayWeek()
                SetUpDati(Statistiche.giornaliera)
			Next
			Me.CBLtipoPersona.Enabled = True
		End If
	End Sub

	Private Sub Setup_FiltriMensili(ByVal giorno As Integer, ByVal mese As Integer, ByVal anno As Integer, ByVal ShowLabels As Boolean, ByVal ShowAll As Boolean, ByVal oCBLtipoPersona As CheckBoxList)

		Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
		Me.LBdataI.Text = giorno & "/" & mese & "/" & anno

		Me.DDLanno.SelectedValue = anno
		Me.DDLmese.SelectedValue = mese

		Me.CBmostraLabel.Checked = ShowLabels
		Me.CBtutti.Checked = ShowAll

		If ShowAll Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
		ElseIf oCBLtipoPersona.SelectedIndex = -1 Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
			Me.CBtutti.Checked = True
		Else
			Dim i, totale As Integer
			totale = oCBLtipoPersona.Items.Count - 1
			For i = 0 To totale
				Me.CBLtipoPersona.Items(i).Selected = oCBLtipoPersona.Items(i).Selected
			Next
			Me.CBLtipoPersona.Enabled = True
		End If
	End Sub

	Private Sub Setup_FiltriAnnuali(ByVal giorno As Integer, ByVal mese As Integer, ByVal anno As Integer, ByVal ShowLabels As Boolean, ByVal ShowAll As Boolean, ByVal oCBLtipoPersona As CheckBoxList)

		Me.HDNdataI.Value = giorno & "/" & mese & "/" & anno 'DateSerial(anno, mese, giorno).ToString("
		Me.LBdataI.Text = giorno & "/" & mese & "/" & anno

		Me.DDLanno.SelectedValue = anno
		Me.CBmostraLabel.Checked = ShowLabels
		Me.CBtutti.Checked = ShowAll

		If ShowAll Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
		ElseIf oCBLtipoPersona.SelectedIndex = -1 Then
			Me.CBLtipoPersona.Enabled = False
			Me.CBLtipoPersona.SelectedIndex = -1
			Me.CBtutti.Checked = True
		Else
			Dim i, totale As Integer
			totale = oCBLtipoPersona.Items.Count - 1
			For i = 0 To totale
				Me.CBLtipoPersona.Items(i).Selected = oCBLtipoPersona.Items(i).Selected
			Next
			Me.CBLtipoPersona.Enabled = True
		End If
	End Sub

	Private Sub Setup_FiltriIscritti(ByVal ShowAll As Boolean, ByVal oCBLtipoPersona As CheckBoxList)
		Me.CBtuttiIscritti.Checked = ShowAll
		If ShowAll Then
			Me.CBLtipoPersonaIscritti.Enabled = False
			Me.CBLtipoPersonaIscritti.SelectedIndex = -1
		ElseIf oCBLtipoPersona.SelectedIndex = -1 Then
			Me.CBLtipoPersonaIscritti.Enabled = False
			Me.CBLtipoPersonaIscritti.SelectedIndex = -1
			Me.CBtuttiIscritti.Checked = True
		Else
			Dim i, totale As Integer
			totale = oCBLtipoPersona.Items.Count - 1
			For i = 0 To totale
				Me.CBLtipoPersonaIscritti.Items(i).Selected = oCBLtipoPersona.Items(i).Selected
			Next
			Me.CBLtipoPersonaIscritti.Enabled = True
		End If
	End Sub

	Private Sub Bind_Organizzazione()

		Me.Bind_TipoIscritti()
		Dim oDataSet As New DataSet
		Dim oPersona As New COL_Persona
		Dim ISTT_ID, i, Totale As Integer

		oPersona = Session("objPersona")
		ISTT_ID = oPersona.Istituzione.Id

		Try
            oDataSet = oPersona.GetOrganizzazioniAssociate(True)
            '   oPersona.GetOrganizzazioniByIstituzione(ISTT_ID)
			Me.DDLorgnIscritti.Items.Clear()

			Totale = oDataSet.Tables(0).Rows.Count() - 1
			For i = 0 To Totale
				Dim oListItem As New ListItem
				If IsDBNull(oDataSet.Tables(0).Rows(i).Item("CMNT_nome")) Then
					oListItem.Text = "--"
				Else
					oListItem.Text = oDataSet.Tables(0).Rows(i).Item("CMNT_nome")
				End If
				oListItem.Value = oDataSet.Tables(0).Rows(i).Item("ORGN_id")
				Me.DDLorgnIscritti.Items.Add(oListItem)
				'metto nel value della ddl l'id della comunita e l'id dell'organizzazione facente parte
				'nella ddlorgn invece metto solo gli id delle organizzazioni come values
			Next

			DDLorgnIscritti.Items.Insert(0, New ListItem("-- Tutte --", -1))
			oResource.setDropDownList(Me.DDLorgnIscritti, -1)

			'If Totale = 1 Then
			'    'Me.DDLorgnIscritti.Enabled = False
			'Else
			'    'Me.DDLorgnIscritti.Enabled = True

			'    'DDLorgnIscritti.Items.Insert(0, New ListItem("-- Tutte --", -1))
			'    'oResource.setDropDownList(Me.DDLorgnIscritti, -1)
			'End If

			If DDLorgnIscritti.Items.Count > 1 Then
				Me.DDLorgnIscritti.SelectedIndex = -1
			End If

		Catch ex As Exception
			oResource.setDropDownList(Me.DDLorgnIscritti, -1)
		End Try
	End Sub
	Private Sub SetDateDayWeek()
		Dim IndiceSelezionato As String = ""

		Try
			IndiceSelezionato = Session("tab")
		Catch ex As Exception

		End Try
		Select Case IndiceSelezionato
			Case "giornaliera"
				Me.LBdataI.Text = Me.HDNdataI.Value
			Case "settimanale"
				Me.LBdataI.Text = Me.HDNdataI.Value
				Dim DtInit As DateTime
				Try
					DtInit = Date.Parse(Me.HDNdataI.Value)
				Catch ex As Exception
					DtInit = Now()
					Me.HDNdataI.Value = Now.Day & "/" & Now.Month & "/" & Now.Year
				End Try

				If DtInit.DayOfWeek = 0 Then
					Session("dtInizioSett") = DtInit.AddDays(-6)
				Else
					Session("dtInizioSett") = DtInit.AddDays(1 - DtInit.DayOfWeek)
				End If

				Dim strTest As String = Session("dtInizioSett")

		End Select
	End Sub
	Private Sub Setup_settimana()
		If Session("dtInizioSett") = Nothing Then
			If Date.Today.DayOfWeek = 0 Then
				Session("dtInizioSett") = Date.Today.AddDays(-6)
			Else
				Session("dtInizioSett") = Date.Today.AddDays(1 - Date.Today.DayOfWeek)
			End If
		End If
	End Sub
#End Region

#Region "Bind"
	Private Sub Bind_TipoPersona()
		Dim oDataset As DataSet
		Dim oTipoPersona As New COL_TipoPersona
		Dim oListItem As New ListItem


		Try
			oDataset = oTipoPersona.Elenca(Session("LinguaID"), Main.FiltroElencoTipiPersona.All)
			Me.CBLtipoPersona.Items.Clear()
			'Me.CBLtipoPersona.Items.Clear()
			If oDataset.Tables(0).Rows.Count > 0 Then
				Me.CBLtipoPersona.DataSource = oDataset
				Me.CBLtipoPersona.DataTextField() = "TPPR_descrizione"
				Me.CBLtipoPersona.DataValueField() = "TPPR_id"
				Me.CBLtipoPersona.DataBind()
				Me.SetColor_ForCBL(Me.CBLtipoPersona)
				Me.CBLtipoPersona.CellPadding = 0
				Me.CBLtipoPersona.CellSpacing = 0

				Me.Bind_TipoIscritti()
			Else
				oResource.setCheckBoxList(Me.CBLtipoPersona, -1)
				oResource.setCheckBoxList(Me.CBLtipoPersonaIscritti, -1)
			End If
		Catch ex As Exception
			oResource.setCheckBoxList(Me.CBLtipoPersona, -1)
			oResource.setCheckBoxList(Me.CBLtipoPersonaIscritti, -1)
		End Try
	End Sub
	Private Sub Bind_Filtri()
		Dim i As Integer
		Dim giorno As Date
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Try
			oResource.setDropDownList(Me.DDLmese, -1)
			For i = 0 To 11
				Me.DDLmese.Items.Add(New ListItem(giorno.ToString("MMMM"), CStr(i + 1)))
				giorno = giorno.AddMonths(1)
			Next
			oResource.setDropDownList(Me.DDLanno, -1)
			Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
			Dim DataMax, DataMin As DateTime
			oStatistiche.getRangeDateGenerali(DataMin, DataMax)

			Dim Inizio, Fine As Integer
			Inizio = DataMin.Year
			If Inizio <= 0 Then
				Inizio = Now.Year
			End If
			Fine = Now.Year

			For i = Fine To Inizio Step -1
				DDLanno.Items.Add(New ListItem(CStr(i), CInt(i)))
			Next

			Me.HDNDataMin.Value = DataMin.Day & "/" & DataMin.Month & "/" & DataMin.Year
			Me.HDNDataMax.Value = Now.Day & "/" & Now.Month & "/" & Now.Year

			Me.HDNdataI.Value = Now.Day & "/" & Now.Month & "/" & Now.Year
			Me.LBdataI.Text = Now.Day & "/" & Now.Month & "/" & Now.Year

			Me.CBL_AnnoConf.Items.Clear()
			For Year As Integer = DataMin.Year To Now.Year
                Me.CBL_AnnoConf.Items.Insert(0, New ListItem(Year, Year))
			Next
			Me.SetColor_ForCBL(Me.CBL_AnnoConf, DataMin.Year)
            For Each Item As ListItem In (From j As ListItem In CBL_AnnoConf.Items Select j).Skip(0).Take(5)
                Item.Selected = True
            Next

			DDLmese.SelectedValue = Now.Month
			Me.DDLanno.SelectedValue = Now.Year

		Catch ex As Exception

		End Try
		Me.Bind_TipoPersona()
	End Sub
#End Region

#Region "Postback"
	Private Sub CBmostraLabel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBmostraLabel.CheckedChanged
		Me.BindGrafici()
	End Sub
	Private Sub CBLtipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBLtipoPersona.SelectedIndexChanged
		Me.BindGrafici()
	End Sub
	Private Sub CBtutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBtutti.CheckedChanged
		If Me.CBtutti.Checked = True Then
			For Each LIST As ListItem In CBLtipoPersona.Items
				LIST.Selected = False
			Next
			Me.CBLtipoPersona.Enabled = False
		Else
			Me.CBLtipoPersona.Enabled = True
			For Each LIST As ListItem In CBLtipoPersona.Items
				LIST.Selected = True
			Next
		End If
		Me.BindGrafici()
	End Sub
	Private Sub IMBapriInizio_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBapriInizio.Click
		Me.SetDateDayWeek()
		SetUpDati(Statistiche.giornaliera)
		Me.BindGrafici()
	End Sub
	Private Sub DDLmese_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLmese.SelectedIndexChanged
		SetUpDati(Statistiche.giornaliera)
		Me.BindGrafici()
	End Sub
	Private Sub CBL_AnnoConf_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBL_AnnoConf.SelectedIndexChanged
		Me.BindGrafici()
	End Sub
	'Private Sub RBLvisualizzaAltroIscritti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLvisualizzaAltroIscritti.SelectedIndexChanged
	'    Me.ShowContent()
	'    Me.BindGrafici()
	'End Sub
	Private Sub RBLvisualizzaAltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLvisualizzaAltro.SelectedIndexChanged
		Me.ShowContent()
		Me.BindGrafici()
	End Sub
	Private Sub RBLtipoChart_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLtipoChart.SelectedIndexChanged
		Me.BindGrafici()
	End Sub
	Private Sub DDLanno_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLanno.SelectedIndexChanged
		'If Me.PNLIscrittiIscrizioni.Visible = True Then
		'    Me.Bind_TipoIscritti()
		'End If
		SetUpDati(Statistiche.giornaliera)
		Me.LabelData_CloseRow()
		Me.BindGrafici()
	End Sub
#Region "Iscritti"
	Private Sub RBLvisualizzaIscrizioni_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLvisualizzaIscrizioni.SelectedIndexChanged
		Me.ShowContent()
		Me.BindGrafici()
	End Sub

	Private Sub CBLtipoPersonaIscritti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBLtipoPersonaIscritti.SelectedIndexChanged
		Me.BindGrafici()
	End Sub
	Private Sub CBtuttiIscritti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBtuttiIscritti.CheckedChanged
		If Me.CBtuttiIscritti.Checked = True Then
			For Each LIST As ListItem In Me.CBLtipoPersonaIscritti.Items
				LIST.Selected = False
			Next
			Me.CBLtipoPersonaIscritti.Enabled = False
		Else
			If Me.CBLtipoPersonaIscritti.Items.Count > 0 Then
				Me.CBLtipoPersonaIscritti.Enabled = True
				For Each LIST As ListItem In Me.CBLtipoPersonaIscritti.Items
					LIST.Selected = True
				Next
			Else
				Me.CBLtipoPersonaIscritti.Enabled = False
				Me.CBtuttiIscritti.Checked = True
			End If

		End If
		If Me.RBLvisualizzaIscrizioni.SelectedIndex = 1 Then
			Me.Bind_graficoIscritti()
		Else
			Me.Bind_graficoIscrittiAndamento()
		End If
	End Sub
	Private Sub DDLorgnIscritti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorgnIscritti.SelectedIndexChanged
		If Session("tab") = "iscritti" Then
			Me.CBtuttiIscritti.Checked = True
			Me.Bind_TipoPersona()
		End If
		Me.BindGrafici()
	End Sub
#End Region
#End Region

#End Region

#Region "Bind Grafici"
	Private Sub BindGrafici()
		Me.TBRData.Visible = False
		Me.LBL_TableData.Text = ""

		Select Case Me.TBSmenu.SelectedIndex 'Session("tab")
			Case Statistiche.giornaliera '"giornaliera"
				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.TBRData.Visible = True
					Me.Bind_graficoGiornalieroConf()
				Else
					Me.Bind_graficoGiornaliero()
				End If
			Case Statistiche.settimanale '"settimanale"
				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.TBRData.Visible = True
					Me.Bind_graficoSettimanaConf()
				Else
					Me.Bind_graficoSettimana()
				End If

			Case Statistiche.mensile '"mensile"
				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.TBRData.Visible = True
					Me.Bind_graficoMensileConf()
				Else
					Me.Bind_graficoMensile()
				End If

			Case Statistiche.annuale '"annuale"
				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.TBRData.Visible = True
					Me.Bind_graficoAnnualeConf()
				Else
					Me.Bind_graficoAnnuale()
				End If

			Case Statistiche.iscritti '"iscritti"
				If Me.RBLvisualizzaIscrizioni.SelectedValue = "1" Then
					Me.TBRData.Visible = True
					Me.Bind_graficoIscrittiAndamento()
				Else
					Me.Bind_graficoIscritti()
				End If
        End Select


        Me.PageUtility.AddActionToModule(0, GetIdModule, lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.ActionType.LoadPortalAccessInfo, , lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
#Region "Giornaliero"
	Public Sub Bind_graficoGiornaliero()
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim oDataSet As New DataSet
		Dim totale, i, j, y, idTPPR, TotaleAccessi, IdCom As Integer  'tipoColore,
		Dim max As Integer = 0
		IdCom = CInt(Me.DDLorgnIscritti.SelectedValue)

		Try
			Dim giorno, mese, anno As Integer
			CTRLchart.Clear()
			giorno = Day(CDate(Me.HDNdataI.Value))
			mese = Month(CDate(Me.HDNdataI.Value))
			anno = Year(CDate(Me.HDNdataI.Value))

			For y = 0 To Me.CBLtipoPersona.Items.Count - 1
				'setto il tppr a -1 e y alla fine del ciclo
				If Me.CBtutti.Checked = True Then
					idTPPR = -1
					y = Me.CBLtipoPersona.Items.Count - 1
					'tipoColore = 13
				Else
					idTPPR = Me.CBLtipoPersona.Items.Item(y).Value
					'tipoColore = idTPPR 'y
				End If
				'colore della riga
				Dim Color As Drawing.Color
				Color = Me.GetColor(idTPPR)	'tipoColore)

				If Me.CBLtipoPersona.Items.Item(y).Selected = True Or Me.CBtutti.Checked = True Then
					oDataSet = oStatistiche.ElencaAccessiGiornalieri(giorno, mese, anno, idTPPR, IdCom, 0)

					totale = oDataSet.Tables(0).Rows.Count
					If totale > 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)
						If IsNothing(oChartSeries) Then
							oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, WebControls.ChartSeriesType.Line)
						End If

						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Red
							oChartSeries.LabelAppearance.RotationAngle = 45
						Else
							oChartSeries.ShowLabels = False
						End If

						LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True
						For j = 0 To 23
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem

							ViewState("trovato") = "no"
							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataSet.Tables(0).Rows(i)
								If oRow.Item("STAC_ora") = j Then
									'oChartSeriesItem.YValue
									oChartSeriesItem.YValue = oRow.Item("Totale")
									'oChartseriesItem.XValue = j
									'oChartSeriesItem.Label = oRow.Item("Totale")
									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato") = "si"
									If oRow.Item("Totale") > max Then
										max = oRow.Item("Totale")
									End If
									TotaleAccessi += oRow.Item("Totale")
									Exit For
								End If
							Next
							If ViewState("trovato") = "no" Then
								oChartSeriesItem.YValue = 0
								'oChartseriesItem.XValue = j
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					Else
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, WebControls.ChartSeriesType.Line)
						oChartSeries.Name = Me.CBLtipoPersona.Items.Item(y).Text
						If Me.CBtutti.Checked = True Then
							oChartSeries.Name = oResource.getValue("tutti")	'"Tutti"
						End If
						For j = 0 To 23
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							'oChartSeriesItem.XValue = j
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)
						Next
						If CTRLchart.Series.Count = 0 Then
							max = 10
						End If
						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					End If
				End If
			Next
			Try
				Dim totaleG, totaleM, totaleS, totaleA As Integer
				oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA, CInt(Me.DDLorgnIscritti.SelectedValue), 0)
				Me.LBtotaleG_anno.Text = totaleA
				Me.LBtotaleG_giorno.Text = totaleG
				Me.LBtotaleG_mese.Text = totaleM
				Me.LBtotaleG_settimana.Text = totaleS
			Catch ex As Exception
			End Try

			Dim stepScala As Integer = Me.GetScala(max)

			CTRLchart.YAxis.AutoScale = False
			CTRLchart.YAxis.AddRange(0, max + stepScala, stepScala)
			CTRLchart.XAxis.AutoScale = False
			CTRLchart.XAxis.AddRange(1, 24, 1)
			For j = 0 To 23
				CTRLchart.XAxis.SetItemLabel(j, (j).ToString)
			Next

			'stile delle scrittine dei valori degli assi
			CTRLchart.XAxis.LayoutStyle = WebControls.ChartAxisLayoutStyle.Inside
			CTRLchart.YAxis.AxisStyle = WebControls.ChartAxisLayoutStyle.Inside
			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(40)
			Me.CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("Ore") '"Ore"
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black

			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			CTRLchart.XAxis.AxisColor = System.Drawing.Color.Black
			CTRLchart.YAxis.AxisColor = System.Drawing.Color.Black

			Me.CTRLchart.Legend.Visible = False	'True
			Me.CTRLchart.Title.Visible = False

			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid


			Me.CTRLchart.ChartImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg

		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
		End Try
	End Sub

	Public Sub Bind_graficoGiornalieroConf()

		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim oDataSet As New DataSet
		Dim totale, i, j, y, TotaleAccessi As Integer
		Dim max As Integer = 0

		Me.LabelData_Init_Day()

		Try
			Dim giorno, mese, anno, AnnoMin As Integer
			CTRLchart.Clear()
			giorno = Day(CDate(Me.HDNdataI.Value))
			mese = Month(CDate(Me.HDNdataI.Value))
			AnnoMin = Year(CDate(Me.HDNDataMin.Value))

			For y = 0 To Me.CBL_AnnoConf.Items.Count - 1
				anno = Me.CBL_AnnoConf.Items(y).Value


				If Me.CBL_AnnoConf.Items(y).Selected Then
					Me.LabelData_OpenRow(anno) '------------------
					'colore della riga
					Dim Color As Drawing.Color
					Color = Me.GetColor(Me.CBL_AnnoConf.Items(y).Value - AnnoMin)

					oDataSet = oStatistiche.ElencaAccessiGiornalieri(giorno, mese, anno, -1, CInt(Me.DDLorgnIscritti.SelectedValue), 0)
					TotaleAccessi = 0
					totale = oDataSet.Tables(0).Rows.Count

					If totale > 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)
						If IsNothing(oChartSeries) Then
							oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, WebControls.ChartSeriesType.Line)
						End If

						'label coi valori
						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Red
							'oChartSeries.LabelAppearance.RotationAngle = 45
						Else
							oChartSeries.ShowLabels = False
						End If

						LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True
						For j = 0 To 23
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							ViewState("trovato") = "no"

							Dim ItemForTable As String = "0"

							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataSet.Tables(0).Rows(i)
								If oRow.Item("STAC_ora") = j Then
									oChartSeriesItem.YValue = oRow.Item("Totale")
									'oChartSeriesItem.XValue = j
									'oChartSeriesItem.Label = oRow.Item("Totale")

									'If oChartSeriesItem.YValue <= 0 Then
									'    oChartSeriesItem.Label = vbCrLf
									'    'oChartSeriesItem.Appearance.MainColor = System.Drawing.Color.Transparent
									'End If

									ItemForTable = oRow.Item("Totale").ToString	'--------

									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato") = "si"
									If oRow.Item("Totale") > max Then
										max = oRow.Item("Totale")
									End If
									TotaleAccessi += oRow.Item("Totale")
									Exit For

								End If
							Next

							Me.LabelData_AddItem(ItemForTable) '----------------

							If ViewState("trovato") = "no" Then
								oChartSeriesItem.YValue = 0
								'oChartSeriesItem.XValue = j
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
					Else
						For k As Integer = 0 To 23
							Me.LabelData_AddItem("0")
						Next
					End If
					Me.LabelData_AddItem(TotaleAccessi)
					Me.LabelData_CloseRow()	'---------------
				End If
			Next

			'Try
			'    Dim totaleG, totaleM, totaleS, totaleA As Integer
			'    oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA)
			'    Me.LBtotaleG_anno.Text = totaleA
			'    Me.LBtotaleG_giorno.Text = totaleG
			'    Me.LBtotaleG_mese.Text = totaleM
			'    Me.LBtotaleG_settimana.Text = totaleS

			'Catch ex As Exception

			'End Try

			Dim stepScala As Integer = Me.GetScala(max)

			CTRLchart.YAxis.AutoScale = False
			CTRLchart.XAxis.AutoScale = False
			'CTRLchart.XAxis.IsZeroBased = True

			CTRLchart.XAxis.AddRange(1, 24, 1)
			For j = 0 To 23
				CTRLchart.XAxis.SetItemLabel(j, (j).ToString)
			Next



			CTRLchart.YAxis.AddRange(0, max + stepScala, stepScala)

			'stile delle scrittine dei valori degli assi
			CTRLchart.XAxis.LayoutStyle = WebControls.ChartAxisLayoutStyle.Inside
			CTRLchart.YAxis.AxisStyle = WebControls.ChartAxisLayoutStyle.Inside
			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(40)
			Me.CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("Ore") '"Ore"
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black

			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			CTRLchart.XAxis.AxisColor = System.Drawing.Color.Black
			CTRLchart.YAxis.AxisColor = System.Drawing.Color.Black

			Me.CTRLchart.Legend.Visible = False	'True
			Me.CTRLchart.Title.Visible = False

			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid


			Me.LabelData_Close() 'Tabella

		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
			Me.LBL_TableData.Text = ""
		End Try
	End Sub
#End Region
#Region "Settimanale"
	Private Sub Bind_graficoSettimana()
		ViewState("zero") = ""
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim dataInizio As Date
		Dim i, totale, j, y, idTPPR, tipoColore As Integer	'max, maxTmp, TotaleAccessi,
		Dim MaxAbs, MaxTmp(7), MaxSum, TotaleAccessi As Integer
		Dim oDataset As New DataSet
		Try
			CTRLchart.Clear()
			MaxAbs = 0 'Il valore più alto tra tutti i singoli dati
			'MaxTmp = 0 'Valore max per ogni serie di dati
			MaxSum = 0 'Somma dei valori massimi di ogni serie
			TotaleAccessi = 0 'Totale degli accessi
			'TotaleAccessi = 0
			For y = 0 To Me.CBLtipoPersona.Items.Count - 1
				'maxTmp = 0
				'setto il tppr a -1 e y alla fine del ciclo
				If Me.CBtutti.Checked = True Then
					idTPPR = -1
					tipoColore = 13
				Else
					idTPPR = Me.CBLtipoPersona.Items.Item(y).Value
					tipoColore = y
				End If
				'colore della riga
				Dim Color As Drawing.Color
				Color = Me.GetColor(idTPPR)

				Dim descrizione As String
				If Me.CBtutti.Checked = True Then
					'Color = Color.IndianRed
					descrizione = oResource.getValue("tutti") '"Tutti"
				Else
					descrizione = Me.CBLtipoPersona.Items.Item(y).Text
					descrizione = UCase(descrizione.Chars(0)) & Right(descrizione, Len(descrizione) - 1)
				End If

				If Me.CBLtipoPersona.Items.Item(y).Selected = True Or Me.CBtutti.Checked = True Then
					dataInizio = CType(Session("dtInizioSett"), Date)
					LBdataI.Text = dataInizio.ToString("d MMM yy") & " - " & dataInizio.AddDays(6).ToString("d MMM yy")
					Dim tipoChart As WebControls.ChartSeriesType
					If Me.RBLtipoChart.SelectedValue = 0 Then
						tipoChart = WebControls.ChartSeriesType.Bar
					Else
						tipoChart = WebControls.ChartSeriesType.StackedBar
					End If
					oDataset = oStatistiche.ElencaAccessiSettimanali(dataInizio, dataInizio.AddDays(6), idTPPR, CInt(Me.DDLorgnIscritti.SelectedValue), 0)

					totale = oDataset.Tables(0).Rows.Count
					If totale <> 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						If IsNothing(oChartSeries) Then
							oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						End If

						'labels
						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Blue
						Else
							oChartSeries.ShowLabels = False
						End If

						LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For j = 0 To 6
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.Appearance.MainColor = Color
							ViewState("trovato2") = "no"

							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataset.Tables(0).Rows(i)
								If oRow.Item("STAC_Giorno") = CDate(Session("dtInizioSett")).AddDays(j).Day Then
									oChartSeriesItem.YValue = oRow.Item("Totale")
									oChartSeriesItem.Label = oRow.Item("Totale")
									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato2") = "si"
									'If Me.CBtutti.Checked = True Then
									'If oRow.Item("Totale") > totaleaccessi Then
									'TotaleAccessi += oRow.Item("Totale")
									'End If
									'End If
									'Se tutti prendo il valore massimo...
									If oRow.Item("Totale") > MaxAbs Then
										MaxAbs = oRow.Item("Totale")
									End If
									'End If
									MaxTmp(j) += oRow.Item("Totale")
									'If oRow.Item("Totale") > MaxTmp Then
									'    MaxTmp = oRow.Item("Totale")
									'End If
									TotaleAccessi += oRow.Item("Totale")
									Exit For
								End If
							Next
							If ViewState("trovato2") = "no" Then
								oChartSeriesItem.YValue = 0
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						ViewState("zero") = "no"
						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					Else

						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						If Me.CBtutti.Checked = True Then
							oChartSeries.Name = oResource.getValue("tutti")	'"Tutti"
						Else
							oChartSeries.Name = descrizione
						End If
						For j = 0 To 6
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)
						Next
						If ViewState("zero") <> "no" Then
							ViewState("zero") = "si"
						End If

						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					End If
				End If
				'MaxSum = MaxSum + MaxTmp '+= max
			Next
			For i = 0 To 6
				If MaxTmp(i) > MaxSum Then
					MaxSum = MaxTmp(i)
				End If
			Next

			'Me.LBtotaleSett.Text = oResource.getValue("LBtotale.text")
			'Me.LBtotaleSett.Text = Me.LBtotaleSett.Text.Replace("#t#", "<br>" & TotaleAccessi)
			Try
				Dim totaleG, totaleM, totaleS, totaleA As Integer
				oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA, CInt(Me.DDLorgnIscritti.SelectedValue), 0)
				Me.LBtotaleG_anno.Text = totaleA
				Me.LBtotaleG_giorno.Text = totaleG
				Me.LBtotaleG_mese.Text = totaleM
				Me.LBtotaleG_settimana.Text = totaleS
			Catch ex As Exception
			End Try


			If Me.RBLtipoChart.SelectedValue = "1" Then
				If Me.CBtutti.Checked = False Then
					MaxAbs = MaxSum
				End If
			End If

			Dim stepScala As Integer = Me.GetScala(MaxAbs)

			CTRLchart.YAxis.AutoScale = False
			CTRLchart.YAxis.AddRange(0, MaxAbs + stepScala, stepScala)

			CTRLchart.XAxis.AutoScale = False
			CTRLchart.XAxis.AddRange(0, 6, 1)
			For j = 0 To 6
				'CTRLchart.XAxis.SetItemLabel(j, dataInizio.AddDays(j).ToString("ddd"))
				CTRLchart.XAxis.SetItemLabel(j, Me.oResource.getValue("TBData.DT.Giorno." & (j + 1).ToString))
			Next

			CTRLchart.XAxis.LayoutStyle = WebControls.ChartAxisLayoutStyle.Inside
			CTRLchart.YAxis.AxisStyle = WebControls.ChartAxisLayoutStyle.Inside

			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(50)
			CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("GiorniSett") '"Giorni della settimana"
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black
			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			CTRLchart.XAxis.AxisColor = System.Drawing.Color.Black
			CTRLchart.YAxis.AxisColor = System.Drawing.Color.Black

			CTRLchart.Legend.Visible = False 'True
			Me.CTRLchart.Title.Visible = False

			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid



		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
		End Try
	End Sub

	Private Sub Bind_graficoSettimanaConf()
		ViewState("zero") = ""
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim dataInizio As Date
		Dim i, totale, j, y, max, maxStac, maxTmp, TotaleAccessi, AnnoMin As Integer
		maxStac = 0
		Dim oDataset As New DataSet

		AnnoMin = Year(CDate(Me.HDNDataMin.Value))

		Me.LabelData_Init_Week()

		Try
			CTRLchart.Clear()
			For y = 0 To Me.CBL_AnnoConf.Items.Count - 1 '0 To Me.CBLtipoPersona.Items.Count - 1
				maxTmp = 0
				'colore della riga
				Dim Color As Drawing.Color
				Color = Me.GetColor(Me.CBL_AnnoConf.Items(y).Value - AnnoMin)

				Dim descrizione As String
				descrizione = Me.CBL_AnnoConf.Items.Item(y).Text
				If Me.CBL_AnnoConf.Items(y).Selected Then 'Me.CBLtipoPersona.Items.Item(y).Selected = True Or Me.CBtutti.Checked = True Then
					Me.LabelData_OpenRow(CBL_AnnoConf.Items(y).Value) '------------------

					dataInizio = CType(Session("dtInizioSett"), Date)
					LBdataI.Text = dataInizio.ToString("d MMM yy") & " - " & dataInizio.AddDays(6).ToString("d MMM yy")

					dataInizio = Date.Parse(dataInizio.Day & "/" & dataInizio.Month & "/" & CBL_AnnoConf.Items(y).Value)

					Dim tipoChart As WebControls.ChartSeriesType
					If Me.RBLtipoChart.SelectedValue = 0 Then
						tipoChart = WebControls.ChartSeriesType.Bar
					Else
						tipoChart = WebControls.ChartSeriesType.StackedBar
					End If

					oDataset = oStatistiche.ElencaAccessiSettimanali(dataInizio, dataInizio.AddDays(6), -1, CInt(Me.DDLorgnIscritti.SelectedValue), 0)
					TotaleAccessi = 0
					totale = oDataset.Tables(0).Rows.Count
					If totale <> 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						If IsNothing(oChartSeries) Then
							oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						End If

						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Blue
						Else
							oChartSeries.ShowLabels = False
						End If

						LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For j = 0 To 6
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.Appearance.MainColor = Color
							ViewState("trovato2") = "no"
							Dim ItemForTable As String = "0"

							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataset.Tables(0).Rows(i)
								If oRow.Item("STAC_Giorno") = CDate(Session("dtInizioSett")).AddDays(j).Day Then
									oChartSeriesItem.YValue = oRow.Item("Totale")
									oChartSeriesItem.Label = oRow.Item("Totale")
									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato2") = "si"

									ItemForTable = oRow.Item("Totale").ToString	'--------

									'If Me.CBtutti.Checked = True Then
									If oRow.Item("Totale") > max Then
										max = oRow.Item("Totale")
									End If
									'End If

									If oRow.Item("Totale") > maxTmp Then
										maxTmp = oRow.Item("Totale")
									End If

									TotaleAccessi += oRow.Item("Totale")
									Exit For
								End If
							Next

							Me.LabelData_AddItem(ItemForTable) '----------------

							If ViewState("trovato2") = "no" Then
								oChartSeriesItem.YValue = 0
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						ViewState("zero") = "no"
					Else

						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						oChartSeries.Name = descrizione

						For j = 0 To 6
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)
							Me.LabelData_AddItem("0")
						Next
						If ViewState("zero") <> "no" Then
							ViewState("zero") = "si"
						End If
					End If
					Me.LabelData_AddItem(TotaleAccessi)
					Me.LabelData_CloseRow()	'---------------
				End If
				maxStac += maxTmp
			Next

			If Me.RBLtipoChart.SelectedValue = "1" Then
				max = maxStac
			End If

			'Me.LBtotaleSett.Text = oResource.getValue("LBtotale.text")
			'Me.LBtotaleSett.Text = Me.LBtotaleSett.Text.Replace("#t#", TotaleAccessi)
			'Try
			'    Dim totaleG, totaleM, totaleS, totaleA As Integer
			'    oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA, CInt(Me.DDLorgnIscritti.SelectedValue))
			'    Me.LBtotaleG_anno.Text = totaleA
			'    Me.LBtotaleG_giorno.Text = totaleG
			'    Me.LBtotaleG_mese.Text = totaleM
			'    Me.LBtotaleG_settimana.Text = totaleS
			'Catch ex As Exception
			'End Try

			Dim stepScala As Integer = Me.GetScala(max)

			CTRLchart.YAxis.AutoScale = False
			CTRLchart.YAxis.AddRange(0, max + stepScala, stepScala)

			CTRLchart.XAxis.AutoScale = False
			CTRLchart.XAxis.AddRange(0, 6, 1)
			For j = 0 To 6
				'CTRLchart.XAxis.SetItemLabel(j, dataInizio.AddDays(j).ToString("ddd"))
				CTRLchart.XAxis.SetItemLabel(j, Me.oResource.getValue("TBData.DT.Giorno." & (j + 1).ToString))
			Next

			CTRLchart.XAxis.LayoutStyle = WebControls.ChartAxisLayoutStyle.Inside
			CTRLchart.YAxis.AxisStyle = WebControls.ChartAxisLayoutStyle.Inside
			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(50)
			CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("GiorniSett") '"Giorni della settimana"
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black
			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			CTRLchart.XAxis.AxisColor = System.Drawing.Color.Black
			CTRLchart.YAxis.AxisColor = System.Drawing.Color.Black

			CTRLchart.Legend.Visible = False 'True
			Me.CTRLchart.Title.Visible = False

			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid



			Me.LabelData_Close() '---------------

		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
			Me.LBL_TableData.Text = "" '---------------
		End Try
	End Sub
#End Region
#Region "Mensile"
	Private Sub Bind_graficoMensile()
		ViewState("zero") = ""
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim y, i, idTPPR, totale, j, mese, anno As Integer ', tipoPersona
		Dim MaxAbs, MaxTmp(31), MaxSum, TotaleAccessi As Integer
		MaxAbs = 0
		'MaxTmp = 0
		MaxSum = 0
		TotaleAccessi = 0

		Dim oDataset As New DataSet
		Try
			Me.CTRLchart.Clear()
			mese = Me.DDLmese.SelectedValue
			anno = CInt(Me.DDLanno.SelectedItem.Value)

			Dim DayInThisMonth As Integer = 0

			'For k As Integer = 0 To Me.DDLanno.Items.Count - 1
			'    If DayInThisMonth < DateTime.DaysInMonth(CInt(Me.DDLanno.Items(k).Value), mese) Then
			DayInThisMonth = DateTime.DaysInMonth(anno, mese)
			'    End If
			'Next
			DayInThisMonth -= 1

			Me.LabelData_Init_Month(DayInThisMonth)

			For y = 0 To Me.CBLtipoPersona.Items.Count - 1
				'MaxTmp = 0
				mese = Me.DDLmese.SelectedValue
				anno = Me.DDLanno.SelectedValue

				If Me.CBtutti.Checked = True Then
					idTPPR = -1
				Else
					idTPPR = Me.CBLtipoPersona.Items.Item(y).Value
				End If

				'colore della riga
				Dim Color As Drawing.Color
				Color = Me.GetColor(idTPPR)

				Dim descrizione As String

				If Me.CBtutti.Checked = True Then
					descrizione = oResource.getValue("tutti") '"Tutti"
					'Color = Color.IndianRed
				Else
					descrizione = Me.CBLtipoPersona.Items.Item(y).Text
					descrizione = UCase(descrizione.Chars(0)) & Right(descrizione, Len(descrizione) - 1)
				End If

				If Me.CBLtipoPersona.Items.Item(y).Selected = True Or Me.CBtutti.Checked = True Then
					Dim tipoChart As WebControls.ChartSeriesType
					If Me.RBLtipoChart.SelectedValue = 0 Then
						tipoChart = WebControls.ChartSeriesType.Bar
					Else
						tipoChart = WebControls.ChartSeriesType.StackedBar
					End If

					oDataset = oStatistiche.ElencaAccessiMensili(mese, anno, idTPPR, CInt(Me.DDLorgnIscritti.SelectedValue), 0)
					totale = oDataset.Tables(0).Rows.Count
					If totale <> 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)


						If IsNothing(oChartSeries) Then
							oChartSeries = Me.CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						End If

						'labels
						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Blue
						Else
							oChartSeries.ShowLabels = False
						End If

						Me.LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For j = 0 To DayInThisMonth
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.Appearance.MainColor = Color
							ViewState("trovato3") = "no"

							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataset.Tables(0).Rows(i)
								If oRow.Item("STAC_Giorno") = j + 1 Then
									oChartSeriesItem.YValue = oRow.Item("Totale")
									oChartSeriesItem.Label = oRow.Item("Totale")
									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato3") = "si"

									TotaleAccessi += oRow.Item("Totale")
									If oRow.Item("Totale") > MaxAbs Then
										MaxAbs = oRow.Item("Totale")
									End If
									MaxTmp(j) += oRow.Item("Totale")

									Exit For
								End If
							Next
							If ViewState("trovato3") = "no" Then
								oChartSeriesItem.YValue = 0
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						ViewState("zero") = "no"
						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					Else

						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						oChartSeries.Name = descrizione
						For j = 0 To DayInThisMonth
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)
						Next
						If ViewState("zero") <> "no" Then
							ViewState("zero") = "si"
						End If

						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					End If
				End If
				'MaxSum = MaxSum + MaxTmp
			Next
			For i = 0 To DayInThisMonth
				If MaxTmp(i) > MaxSum Then
					MaxSum = MaxTmp(i)
				End If
			Next

			If Me.RBLtipoChart.SelectedValue = "1" Then
				If Me.CBtutti.Checked = False Then
					MaxAbs = MaxSum
				End If
			End If

			'Me.LBtotaleMens.Text = oResource.getValue("LBtotale.text")
			'Me.LBtotaleMens.Text = Me.LBtotaleMens.Text.Replace("#t#", "<br>" & TotaleAccessi)
			Try
				Dim totaleG, totaleM, totaleS, totaleA As Integer
				oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA, CInt(Me.DDLorgnIscritti.SelectedValue), 0)
				Me.LBtotaleG_anno.Text = totaleA
				Me.LBtotaleG_giorno.Text = totaleG
				Me.LBtotaleG_mese.Text = totaleM
				Me.LBtotaleG_settimana.Text = totaleS
			Catch ex As Exception
			End Try

			Dim stepScala As Integer = Me.GetScala(MaxAbs)

			CTRLchart.YAxis.AutoScale = False
			CTRLchart.YAxis.AddRange(0, MaxAbs + stepScala, stepScala)

			CTRLchart.XAxis.AutoScale = False
			CTRLchart.XAxis.AddRange(0, DayInThisMonth, 1)
			'CTRLchart.XAxis.AddRange(0, 30, 1)
			For j = 0 To DayInThisMonth
				CTRLchart.XAxis.SetItemLabel(j, j + 1)
			Next

			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)

			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(45)
			CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("GiorniMese") '"Giorni del mese"
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black
			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.Legend.Visible = False	'True
			Me.CTRLchart.Title.Visible = False


		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
		End Try
	End Sub

	Private Sub Bind_graficoMensileConf()
		ViewState("zero") = ""

		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim y, i, idTPPR, totale, j, max, maxStac, maxTmp, TotaleAccessi, mese, anno, AnnoMin As Integer
		Dim oDataset As New DataSet
		maxStac = 0
		Try
			AnnoMin = Year(CDate(Me.HDNDataMin.Value))
			mese = Me.DDLmese.SelectedValue

			Me.CTRLchart.Clear()

			Dim DayInThisMonth As Integer = 0
			For k As Integer = 0 To Me.DDLanno.Items.Count - 1
				If DayInThisMonth < DateTime.DaysInMonth(CInt(Me.DDLanno.Items(k).Value), mese) Then
					DayInThisMonth = DateTime.DaysInMonth(CInt(Me.DDLanno.Items(k).Value), mese)
				End If
			Next
			DayInThisMonth -= 1

			Me.LabelData_Init_Month(DayInThisMonth)

			For y = 0 To Me.CBL_AnnoConf.Items.Count - 1
				maxTmp = 0
				'setto il tppr a -1 e y alla fine del ciclo
				anno = Me.CBL_AnnoConf.Items(y).Value

				idTPPR = -1

				'colore della riga
				Dim Color As Drawing.Color
				Color = Me.GetColor(anno - AnnoMin)
				Dim descrizione As String
				descrizione = Me.CBL_AnnoConf.SelectedItem.Text
				'Dim ItemForTable As String = "0"

				If Me.CBL_AnnoConf.Items(y).Selected Then 'Me.CBLtipoPersona.Items.Item(y).Selected = True Or Me.CBtutti.Checked = True Then

					Me.LabelData_OpenRow(anno) '------------------

					Dim tipoChart As WebControls.ChartSeriesType
					If Me.RBLtipoChart.SelectedValue = 0 Then
						tipoChart = WebControls.ChartSeriesType.Bar
					Else
						tipoChart = WebControls.ChartSeriesType.StackedBar
					End If

					oDataset = oStatistiche.ElencaAccessiMensili(mese, anno, idTPPR, CInt(Me.DDLorgnIscritti.SelectedValue), 0)
					totale = oDataset.Tables(0).Rows.Count
					TotaleAccessi = 0
					If totale <> 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						If IsNothing(oChartSeries) Then
							oChartSeries = Me.CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						End If

						'labels
						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Blue
						Else
							oChartSeries.ShowLabels = False
						End If

						Me.LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For j = 0 To DayInThisMonth
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							Dim ItemForTable As String = "0"

							oChartSeriesItem.Appearance.MainColor = Color
							ViewState("trovato3") = "no"

							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataset.Tables(0).Rows(i)

								If oRow.Item("STAC_Giorno") = j + 1 Then
									oChartSeriesItem.YValue = oRow.Item("Totale")
									oChartSeriesItem.Label = oRow.Item("Totale")
									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato3") = "si"

									ItemForTable = oRow.Item("Totale").ToString	'--------

									'If Me.CBtutti.Checked = True Then
									If oRow.Item("Totale") > max Then
										max = oRow.Item("Totale")
									End If
									'End If
									If oRow.Item("Totale") > maxTmp Then
										maxTmp = oRow.Item("Totale")
									End If
									TotaleAccessi += oRow.Item("Totale")
									Exit For
								End If
							Next

							Me.LabelData_AddItem(ItemForTable) '----------------

							If ViewState("trovato3") = "no" Then
								oChartSeriesItem.YValue = 0
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						ViewState("zero") = "no"

					Else
						For k As Integer = 0 To DayInThisMonth
							Me.LabelData_AddItem("0")
						Next
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						oChartSeries.Name = descrizione
						For j = 0 To 30
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)
						Next
						If ViewState("zero") <> "no" Then
							ViewState("zero") = "si"
						End If
					End If
					Me.LabelData_AddItem(TotaleAccessi)
					Me.LabelData_CloseRow()	'---------------
				End If
				maxStac += maxTmp
			Next

			'Me.LBtotaleMens.Text = oResource.getValue("LBtotale.text")
			'Me.LBtotaleMens.Text = Me.LBtotaleMens.Text.Replace("#t#", TotaleAccessi)

			'Try
			'    Dim totaleG, totaleM, totaleS, totaleA As Integer
			'    oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA, CInt(Me.DDLorgnIscritti.SelectedValue))
			'    Me.LBtotaleG_anno.Text = totaleA
			'    Me.LBtotaleG_giorno.Text = totaleG
			'    Me.LBtotaleG_mese.Text = totaleM
			'    Me.LBtotaleG_settimana.Text = totaleS
			'Catch ex As Exception
			'End Try

			If Me.RBLtipoChart.SelectedValue = "1" Then
				max = maxStac
			End If

			Dim stepScala As Integer = Me.GetScala(max)

			CTRLchart.YAxis.AutoScale = False
			CTRLchart.YAxis.AddRange(0, max + stepScala, stepScala)

			CTRLchart.XAxis.AutoScale = False
			CTRLchart.XAxis.AddRange(0, DayInThisMonth, 1)

			For j = 0 To DayInThisMonth
				CTRLchart.XAxis.SetItemLabel(j, j + 1)
			Next

			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(45)
			CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("GiorniMese") '"Giorni del mese"
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black
			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.Legend.Visible = False	'True
			Me.CTRLchart.Title.Visible = False


			Me.LabelData_Close() '---------------

		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
			Me.LBL_TableData.Text = "" '---------------
		End Try
	End Sub
#End Region
#Region "Annuale"
	Private Sub Bind_graficoAnnuale()
		ViewState("zero") = ""
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim y, i, idTPPR, totale, j, anno, tipoPersona As Integer ', valoreMinimo
		Dim MaxAbs, MaxTmp(12), MaxSum, TotaleAccessi As Integer
		MaxAbs = 0
		'MaxTmp = 0
		MaxSum = 0
		TotaleAccessi = 0

		Dim oDataset As New DataSet
		Try
			'valoreMinimo = -1
			Me.CTRLchart.Clear()

			Dim tipoChart As WebControls.ChartSeriesType
			If Me.RBLtipoChart.SelectedValue = 0 Then
				tipoChart = WebControls.ChartSeriesType.Bar
			Else
				tipoChart = WebControls.ChartSeriesType.StackedBar
			End If
			For y = 0 To Me.CBLtipoPersona.Items.Count - 1
				anno = Me.DDLanno.SelectedValue
				If Me.CBtutti.Checked = True Then
					idTPPR = -1
					tipoPersona = 13
				Else
					idTPPR = Me.CBLtipoPersona.Items.Item(y).Value
					tipoPersona = y
				End If
				Dim Color As Drawing.Color
				Color = Me.GetColor(idTPPR)

				If Me.CBLtipoPersona.Items.Item(y).Selected = True Or Me.CBtutti.Checked = True Then

					oDataset = oStatistiche.ElencaAccessiAnnuali(anno, idTPPR, CInt(Me.DDLorgnIscritti.SelectedValue))
					totale = oDataset.Tables(0).Rows.Count
					If totale <> 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)


						If IsNothing(oChartSeries) Then
							oChartSeries = Me.CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						End If

						'labels
						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Blue
						Else
							oChartSeries.ShowLabels = False
						End If

						Me.LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For j = 0 To 11
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.Appearance.MainColor = Color
							ViewState("trovato4") = "no"

							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataset.Tables(0).Rows(i)
								If oRow.Item("STAC_Mese") = j + 1 Then
									oChartSeriesItem.YValue = oRow.Item("Totale")
									oChartSeriesItem.Label = oRow.Item("Totale")
									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato4") = "si"

									If oRow.Item("Totale") > MaxAbs Then
										MaxAbs = oRow.Item("Totale")
									End If
									'End If
									MaxTmp(j) += oRow.Item("Totale")
									TotaleAccessi += oRow.Item("Totale")

									Exit For
								End If
							Next
							If ViewState("trovato4") = "no" Then
								oChartSeriesItem.YValue = 0
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						ViewState("zero") = "no"
						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					Else

						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						For j = 0 To 11
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)
						Next
						If ViewState("zero") <> "no" Then
							ViewState("zero") = "si"
						End If
						If Me.CBtutti.Checked = True Then
							Exit For
						End If
					End If
				End If
				'MaxSum = MaxSum + MaxTmp '+= max
			Next

			For i = 0 To 11
				If MaxTmp(i) > MaxSum Then
					MaxSum = MaxTmp(i)
				End If
			Next

			If Me.RBLtipoChart.SelectedValue = "1" Then
				If Me.CBtutti.Checked = False Then
					MaxAbs = MaxSum
				End If
			End If

			'Me.LBtotaleAnn.Text = oResource.getValue("LBtotale.text")
			'Me.LBtotaleAnn.Text = Me.LBtotaleAnn.Text.Replace("#t#", "<br>" & TotaleAccessi)
			Try
				Dim totaleG, totaleM, totaleS, totaleA As Integer
				oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA, CInt(Me.DDLorgnIscritti.SelectedValue), 0)

				Me.LBtotaleG_anno.Text = totaleA
				Me.LBtotaleG_giorno.Text = totaleG
				Me.LBtotaleG_mese.Text = totaleM
				Me.LBtotaleG_settimana.Text = totaleS
			Catch ex As Exception
			End Try

			Dim stepScala As Integer = Me.GetScala(MaxAbs)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("MesiAnno")
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black
			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			CTRLchart.YAxis.AutoScale = False
			CTRLchart.YAxis.AddRange(0, MaxAbs + stepScala, stepScala)

			CTRLchart.XAxis.AutoScale = False
			CTRLchart.XAxis.AddRange(0, 11, 1)
			For j = 0 To 11
				CTRLchart.XAxis.SetItemLabel(j, Me.oResource.getValue("TBData.DT.Mese." & (j + 1).ToString))
			Next

			'CTRLchart.XAxis.SetItemLabel(1, "Pippo")

			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(50)
			CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)


			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.Legend.Visible = False	'True
			Me.CTRLchart.Title.Visible = False

		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
		End Try
	End Sub

	Private Sub Bind_graficoAnnualeConf()
		ViewState("zero") = ""

		Me.LabelData_Init_Year()

		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim y, i, idTPPR, totale, j, max, MaxStac, Maxtmp, TotaleAccessi, anno, annoMin, valoreMinimo As Integer
		Dim oDataset As New DataSet

		MaxStac = 0
		annoMin = Year(CDate(Me.HDNDataMin.Value))

		Try
			valoreMinimo = -1
			Me.CTRLchart.Clear()

			Dim tipoChart As WebControls.ChartSeriesType
			If Me.RBLtipoChart.SelectedValue = 0 Then
				tipoChart = WebControls.ChartSeriesType.Bar
			Else
				tipoChart = WebControls.ChartSeriesType.StackedBar
			End If
			For y = 0 To Me.CBL_AnnoConf.Items.Count - 1


				Maxtmp = 0
				anno = Me.CBL_AnnoConf.Items(y).Value

				idTPPR = -1
				Dim Color As Drawing.Color
				Color = Me.GetColor(anno - annoMin)

				If Me.CBL_AnnoConf.Items(y).Selected Then
					oDataset = oStatistiche.ElencaAccessiAnnuali(anno, idTPPR, CInt(Me.DDLorgnIscritti.SelectedValue))
					Try
						totale = oDataset.Tables(0).Rows.Count 'numero elementi
					Catch ex As Exception
						totale = 0
					End Try

					Me.LabelData_OpenRow(anno) '------------------
					TotaleAccessi = 0
					If totale > 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						If IsNothing(oChartSeries) Then
							oChartSeries = Me.CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						End If

						'labels
						If Me.CBmostraLabel.Checked = True Then
							oChartSeries.ShowLabels = True
							oChartSeries.LabelAppearance.TextColor = Color.Blue
						Else
							oChartSeries.ShowLabels = False
						End If


						Me.LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For j = 0 To 11	'mesi
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.Appearance.MainColor = Color

							'Me.CTRLchart.XAxis.Item(j).Label = Me.oResource.getValue("TBData.DT.Mese." & (j + 1).ToString)
							ViewState("trovato4") = "no"

							Dim ItemForTable As String = "0"
							Dim HasData As Boolean = True

							For i = 0 To totale - 1
								Dim oRow As DataRow
								Try
									oRow = oDataset.Tables(0).Rows(i)
								Catch ex As Exception
									HasData = False
								End Try

								If HasData Then
									If oRow.Item("STAC_Mese") = j + 1 Then
										oChartSeriesItem.YValue = oRow.Item("Totale")
										oChartSeriesItem.Label = oRow.Item("Totale")

										ItemForTable = oRow.Item("Totale").ToString	'--------

										'
										oChartSeries.Items.Add(oChartSeriesItem)

										ViewState("trovato4") = "si"

										'If Me.CBtutti.Checked = True Then
										If oRow.Item("Totale") > max Then
											max = oRow.Item("Totale")
										End If
										'End If
										If oRow.Item("Totale") > Maxtmp Then
											Maxtmp = oRow.Item("Totale")
										End If

										Try
											If valoreMinimo = -1 Then
												valoreMinimo = oRow.Item("Totale")
											Else
												If oRow.Item("Totale") < valoreMinimo Then
													valoreMinimo = oRow.Item("Totale")
												End If
											End If
										Catch ex As Exception

										End Try
										TotaleAccessi += oRow.Item("Totale")
										Exit For
									End If
								Else
									ItemForTable = "0"
								End If
							Next

							Me.LabelData_AddItem(ItemForTable) '----------------

							If ViewState("trovato4") = "no" Then
								oChartSeriesItem.YValue = 0
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						ViewState("zero") = "no"
					Else
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						For j = 0 To 11
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)
							Me.LabelData_AddItem(TotaleAccessi.ToString)
						Next
						If ViewState("zero") <> "no" Then
							ViewState("zero") = "si"
						End If

						Me.LabelData_CloseRow()	'---------------
					End If
					Me.LabelData_AddItem(TotaleAccessi)

				End If
				MaxStac += Maxtmp
				Me.LabelData_CloseRow()	'---------------
			Next
			'Me.LBtotaleAnn.Text = oResource.getValue("LBtotale.text")
			'Me.LBtotaleAnn.Text = Me.LBtotaleAnn.Text.Replace("#t#", TotaleAccessi)
			'Try
			'    Dim totaleG, totaleM, totaleS, totaleA As Integer
			'    oStatistiche.getRiepilogoAccessi(New Date(Year(CDate(Me.HDNdataI.Value)), Month(CDate(Me.HDNdataI.Value)), Day(CDate(Me.HDNdataI.Value))), totaleG, totaleS, totaleM, totaleA, CInt(Me.DDLorgnIscritti.SelectedValue))
			'    Me.LBtotaleG_anno.Text = totaleA
			'    Me.LBtotaleG_giorno.Text = totaleG
			'    Me.LBtotaleG_mese.Text = totaleM
			'    Me.LBtotaleG_settimana.Text = totaleS
			'Catch ex As Exception
			'End Try

			Dim stepUnita As Integer '= 500

			CTRLchart.YAxis.AutoScale = False
			stepUnita = Me.GetScala(max)
			If Me.RBLtipoChart.SelectedValue = "1" Then
				max = MaxStac
			End If

			stepUnita = Me.GetScala(max)
			CTRLchart.YAxis.AddRange(0, max + stepUnita, stepUnita)	'RIVEDERE... In teoria ho il valore di fondo scala... :p

			CTRLchart.XAxis.AutoScale = False
			CTRLchart.XAxis.AddRange(0, 11, 1)
			For j = 0 To 11
				CTRLchart.XAxis.SetItemLabel(j, Me.oResource.getValue("TBData.DT.Mese." & (j + 1).ToString))
			Next

			CTRLchart.Gridlines.HorizontalGridlines.Visible = True

			CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
			CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(50)
			CTRLchart.Width = System.Web.UI.WebControls.Unit.Pixel(740)

			'label degli assi cartesiani
			CTRLchart.XAxis.Label.Text = oResource.getValue("MesiAnno")
			CTRLchart.XAxis.Label.TextColor = System.Drawing.Color.Black

			Dim data As Date
			data = Date.Parse("01/01/2005")
			For j = 0 To 11
				CTRLchart.XAxis.SetItemLabel(j, data.AddMonths(j).ToString("MMM"))
			Next

			CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"
			CTRLchart.YAxis.Label.TextColor = System.Drawing.Color.Black

			Me.CTRLchart.Background.MainColor = System.Drawing.Color.OldLace
			Me.CTRLchart.Background.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.PlotArea.MainColor = System.Drawing.Color.AliceBlue
			Me.CTRLchart.PlotArea.FillStyle = WebControls.FillStyle.Solid

			Me.CTRLchart.Legend.Visible = False	'True
			Me.CTRLchart.Title.Visible = False

			Me.LabelData_Close() '---------------

		Catch ex As Exception
			CTRLchart.Clear()
			LBnoRecord.Visible = True
			oResource.setLabel(LBnoRecord)
			Me.CTRLchart.Visible = False
			Me.LBL_TableData.Text = "" '---------------
		End Try
	End Sub
#End Region
#Region "Iscritti"
	Private Sub Bind_graficoIscritti()
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim y, idTPPR, totale, i As Integer
		Dim oDataset As New DataSet
		Try
			Me.CTRLchart.Clear()
			For y = 0 To Me.CBLtipoPersonaIscritti.Items.Count - 1
				If Me.CBtuttiIscritti.Checked = True Then
					idTPPR = -1
					y = Me.CBLtipoPersonaIscritti.Items.Count - 1
				Else
					idTPPR = Me.CBLtipoPersonaIscritti.Items.Item(y).Value
				End If
				Dim Color As Drawing.Color
				If Me.CBLtipoPersonaIscritti.Items.Item(y).Selected = True Or Me.CBtuttiIscritti.Checked = True Then
					oDataset = oStatistiche.ElencaIscritti(Session("LinguaID"), Me.DDLorgnIscritti.SelectedValue, idTPPR, 0)
					totale = oDataset.Tables(0).Rows.Count
					If totale > 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()

						oChartSeries = Me.CTRLchart.GetChartSeries(0)

						If IsNothing(oChartSeries) Then
							oChartSeries = Me.CTRLchart.CreateSeries(String.Empty, Color.Blue, WebControls.ChartSeriesType.Pie)
						End If

						'oChartSeries.Type = WebControls.ChartSeriesType.Pie
						oChartSeries.Appearance.FillStyle = WebControls.FillStyle.Solid
						'oChartSeries.LabelAppearance.Distance = 50

						oChartSeries.NotOverlapLabels = True
						'oChartSeries.ShowLabelConnectors = True

						'oChartSeries.DefaultLabel = "#%" '
						oChartSeries.LabelAppearance.Distance = 25

						oChartSeries.ShowLabels = Me.CBmostraLabel.Checked

						'oChartSeries.Appearance.GradientFocus

						'oChartSeries.ShowLabels = True
						Me.LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For i = 0 To totale - 1

							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							Dim oRow As DataRow
							oRow = oDataset.Tables(0).Rows(i)

							Color = Me.GetColor(oRow.Item("TPPR_id"))
							oChartSeriesItem.Appearance.MainColor = Color
							oChartSeriesItem.Appearance.FillStyle = WebControls.FillStyle.Solid

							oChartSeriesItem.YValue = CInt(oRow.Item("Totale"))
							oChartSeriesItem.Label = "#Y (#%{P2})"
							'Dim descrizione As String
							'descrizione = oRow.Item("TPPR_descrizione")
							'descrizione = UCase(descrizione.Chars(0)) & Right(descrizione, Len(descrizione) - 1)
							'Try
							'    oChartSeriesItem.Label = CInt(oRow.Item("Totale")) 'descrizione & " [" & oRow.Item("Totale") & "]"
							'Catch ex As Exception
							'    oChartSeriesItem.Label = 0
							'End Try

							oChartSeries.Items.Add(oChartSeriesItem)

							'oChartSeries.ShowPercen = Me.CBmostraLabel.Checked 'False
							'oChartSeries.ShowLabelConnectors = True

							'oChartSeries.PercentFormat = "####"
						Next
						'oChartSeries.DefaultLabel = "#%"
					End If
				End If
			Next

			If CTRLchart.Series.Count = 0 Then
				CTRLchart.Clear()
				Me.CTRLchart.Visible = False
				Me.LBnoRecord.Visible = True
				Me.LBnoRecord.Text = oResource.getValue("LBnoRecord.text")
			Else
				Me.CTRLchart.Visible = True
				Me.LBnoRecord.Visible = False
				Me.CTRLchart.Legend.Visible = False	'True

				'Me.CTRLchart.TemplateSourceDirectory = "D:\Inetpub\wwwroot\Comunita_OnLine\Template\Telerick_Pie_web20"

				CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(20)
				CTRLchart.Margins.Bottom = System.Web.UI.WebControls.Unit.Pixel(20)
				CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(20)
				CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(20)

				Me.CTRLchart.Legend.HAlignment = WebControls.ChartPosition.Right
				Me.CTRLchart.Legend.Location = WebControls.ChartLocation.OutsidePlotArea

			End If

		Catch ex As Exception
			CTRLchart.Clear()
		End Try
	End Sub
	Private Sub Bind_graficoIscrittiAndamento()
		ViewState("zero") = ""
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim y, i, idTPPR, totale, j, max, anno, tipoPersona, valoreMinimo, TotaleIscritti As Integer
		Dim oDataset As New DataSet
		Dim totaleShow As Integer = 0
		Try
			valoreMinimo = -1
			Me.CTRLchart.Clear()

			Dim tipoChart As WebControls.ChartSeriesType

			tipoChart = WebControls.ChartSeriesType.Bar

			Me.LabelData_Init_Iscritti() '--------------------

			For y = 0 To Me.CBLtipoPersona.Items.Count - 1
				TotaleIscritti = 0
				anno = Me.DDLanno.SelectedValue

				If Me.CBtutti.Checked = True Then
					idTPPR = -1
					y = Me.CBLtipoPersona.Items.Count - 1

				Else
					idTPPR = Me.CBLtipoPersona.Items.Item(y).Value

				End If
				tipoPersona = y

				Dim Color As Drawing.Color
				Color = Me.GetColor(idTPPR)

				Dim descrizione As String = ""


				'If Me.CBtutti.Checked = True Then
				'    descrizione = oResource.getValue("tutti")
				'    'Color = Color.IndianRed
				'Else
				'    descrizione = Me.CBLtipoPersona.Items.Item(y).Text
				'    descrizione = UCase(descrizione.Chars(0)) & Right(descrizione, Len(descrizione) - 1)
				'End If
				If Me.CBLtipoPersona.Items.Item(y).Selected = True Or Me.CBtutti.Checked = True Then

					oDataset = oStatistiche.AndamentoIscrizioni(Session("LinguaID"), Me.DDLorgnIscritti.SelectedValue, idTPPR, Me.DDLanno.SelectedValue, 0)

					totale = oDataset.Tables(0).Rows.Count
					totaleShow += totale
					If totale <> 0 Then
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)


						If IsNothing(oChartSeries) Then
							oChartSeries = Me.CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						End If

						'legenda
						If Me.CBtutti.Checked = True Then
							oChartSeries.Name = oResource.getValue("tutti")	'"Tutti"
							Me.LabelData_OpenRow(Me.oResource.getValue("tutti"), 2)	'-------------
						Else
							oChartSeries.Name = descrizione
							Me.LabelData_OpenRow(Me.CBLtipoPersona.Items.Item(y).Text, 2) '------------
						End If

						oChartSeries.ShowLabels = Me.CBmostraLabel.Checked

						Me.LBnoRecord.Visible = False
						Me.CTRLchart.Visible = True

						For j = 0 To 11
							Dim DataToAdd As String = "0"
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.Appearance.MainColor = Color
							ViewState("trovato4") = "no"

							For i = 0 To totale - 1
								Dim oRow As DataRow
								oRow = oDataset.Tables(0).Rows(i)
								If oRow.Item("Mese") = j + 1 Then
									oChartSeriesItem.YValue = oRow.Item("Totale")
									oChartSeriesItem.Label = oRow.Item("Totale")
									TotaleIscritti += oRow.Item("Totale")
									oChartSeries.Items.Add(oChartSeriesItem)
									ViewState("trovato4") = "si"

									DataToAdd = oRow.Item("Totale")

									If oRow.Item("Totale") > max Then
										max = oRow.Item("Totale")
									End If

									Try
										If valoreMinimo = -1 Then
											valoreMinimo = oRow.Item("Totale")
										Else
											If oRow.Item("Totale") < valoreMinimo Then
												valoreMinimo = oRow.Item("Totale")
											End If
										End If
									Catch ex As Exception
									End Try
									Exit For
								End If
							Next
							Me.LabelData_AddItem(DataToAdd)

							If ViewState("trovato4") = "no" Then
								oChartSeriesItem.YValue = 0
								oChartSeriesItem.Label = " "
								oChartSeries.Items.Add(oChartSeriesItem)
							End If
						Next
						ViewState("zero") = "no"
						If Me.CBtutti.Checked = True Then
							Me.LabelData_AddItem(TotaleIscritti)
							Me.LabelData_CloseRow()
							Exit For
						End If
					Else
						Dim oChartSeries As New Telerik.WebControls.ChartSeries
						oChartSeries.Clear()
						oChartSeries = Me.CTRLchart.GetChartSeries(y)

						oChartSeries = CTRLchart.CreateSeries(String.Empty, Color, tipoChart)
						oChartSeries.ShowLabels = Me.CBmostraLabel.Checked
						oChartSeries.Name = descrizione

						Me.LabelData_AddItem(Me.CBLtipoPersona.Items(y).Text, 2)

						For j = 0 To 11
							Dim oChartSeriesItem As New Telerik.WebControls.ChartSeriesItem
							oChartSeriesItem.YValue = 0
							oChartSeriesItem.Label = " "
							oChartSeries.Items.Add(oChartSeriesItem)

							Me.LabelData_AddItem("0")
						Next
						If ViewState("zero") <> "no" Then
							ViewState("zero") = "si"
						End If
						If Me.CBtutti.Checked = True Then
							Me.LabelData_AddItem(TotaleIscritti)
							Me.LabelData_CloseRow()
							Exit For
						End If
					End If
					Me.LabelData_AddItem(TotaleIscritti)
					Me.LabelData_CloseRow()
				End If

			Next
			If totaleShow = 0 Then
				Me.LBL_TableData.Text = ""
				Me.CTRLchart.Clear()
				Me.CTRLchart.Visible = False
				Me.LBnoRecord.Visible = True
				Me.LBnoRecord.Text = oResource.getValue("LBnoRecord.text")
			Else
				Me.LabelData_Close()

				Dim stepUnita As Integer = Me.GetScala(max)	'500

				If Me.CBtutti.Checked = True Then
					CTRLchart.YAxis.AutoScale = False
					CTRLchart.YAxis.AddRange(0, max + stepUnita * 2, stepUnita)
				Else
					If ViewState("zero") = "si" Then
						CTRLchart.YAxis.AutoScale = False
						CTRLchart.YAxis.AddRange(0, 50, 20)
					Else
						If valoreMinimo > 0 Then
							CTRLchart.YAxis.AutoScale = False
							CTRLchart.YAxis.AddRange(0, max + stepUnita * 2, stepUnita)
						Else
							CTRLchart.YAxis.AutoScale = True
						End If

					End If
				End If

				CTRLchart.XAxis.AutoScale = False
				CTRLchart.XAxis.AddRange(0, 11, 1)
				For j = 0 To 11
					CTRLchart.XAxis.SetItemLabel(j, Me.oResource.getValue("TBData.DT.Mese." & (j + 1).ToString))
				Next

				'stile delle scrittine dei valori degli assi
				CTRLchart.XAxis.LayoutStyle = WebControls.ChartAxisLayoutStyle.Inside
				'CTRLchart.YAxis.LayoutStyle = WebControls.ChartAxisLayoutStyle.Inside
				CTRLchart.Gridlines.HorizontalGridlines.Visible = True

				'label degli assi cartesiani
				CTRLchart.XAxis.Label.Text = oResource.getValue("MesiAnno")	'"Mesi dell'anno"
				CTRLchart.YAxis.Label.Text = oResource.getValue("Accessi") '"N. Accessi"

				Me.CTRLchart.Legend.Visible = False	'True

				CTRLchart.Margins.Top = System.Web.UI.WebControls.Unit.Pixel(10)
				CTRLchart.Margins.Bottom = System.Web.UI.WebControls.Unit.Pixel(20)
				CTRLchart.Margins.Left = System.Web.UI.WebControls.Unit.Pixel(45)
				CTRLchart.Margins.Right = System.Web.UI.WebControls.Unit.Pixel(5)
			End If
		Catch ex As Exception
			Me.LBL_TableData.Text = ""
			CTRLchart.Clear()
			Me.CTRLchart.Visible = False
			Me.LBnoRecord.Visible = True
			Me.LBnoRecord.Text = oResource.getValue("LBnoRecord.text")
		End Try
	End Sub
#End Region
#End Region
#Region "Filtri iscritti"

	Private Sub Bind_TipoIscritti()
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))

		Dim oDsIscritti As New DataSet
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim ORGN_id As Integer
		Try
			ORGN_id = Me.DDLorgnIscritti.SelectedValue
		Catch ex As Exception
			ORGN_id = -1
		End Try

		oDsIscritti = oStatistiche.ElencaIscritti(Session("LinguaID"), ORGN_id, -1, 0) 'idTPPR
		Me.CBLtipoPersonaIscritti.Items.Clear()
		Dim oItemPersonaIscritti As ListItem


		Dim StrCol0, StrCol1 As String
		StrCol0 = "<font style='FONT-SIZE: 7pt; BACKGROUND-COLOR: "
		StrCol1 = "'>&nbsp;&nbsp;&nbsp;</font>&nbsp;"
		If oDsIscritti.Tables(0).Rows.Count > 0 Then

			For Each oRow As DataRow In oDsIscritti.Tables(0).Rows
				oItemPersonaIscritti = New ListItem(StrCol0 & Me.GetColorStr(oRow.Item("TPPR_id")) & StrCol1 & oRow.Item("TPPR_Descrizione") & " [" & oRow.Item("Totale") & "]", oRow.Item("TPPR_id"))
				Me.CBLtipoPersonaIscritti.Items.Add(oItemPersonaIscritti)
			Next
		End If

	End Sub

#End Region
#Region "Colori"
	Private Sub SetColor_ForCBL(ByVal CBLTipi As System.Web.UI.WebControls.CheckBoxList, Optional ByVal YearMin As Integer = -1)
		Dim ItemCount As Integer
		Try
			ItemCount = CBLTipi.Items.Count
		Catch ex As Exception
			Exit Sub
		End Try
		If ItemCount <= 0 Then
			Exit Sub
		End If

		Dim StrCol0, StrCol1, StrText, Color As String
		StrCol0 = "<font style='FONT-SIZE: 7pt; BACKGROUND-COLOR: "
		StrCol1 = "'>&nbsp;&nbsp;&nbsp;</font>&nbsp;"

		For i As Integer = 0 To ItemCount - 1
			StrText = CBLTipi.Items(i).Text

			If YearMin > 0 Then
				Color = Me.GetColorStr(CBLTipi.Items(i).Value - YearMin)
			Else
				Color = Me.GetColorStr(CBLTipi.Items(i).Value)
			End If
			CBLTipi.Items(i).Text = StrCol0 & Color & StrCol1 & StrText	'& StrTable2
		Next
	End Sub
	Private Sub SetColor_ForRBL(ByVal RBLTipi As System.Web.UI.WebControls.RadioButtonList)
		Dim ItemCount As Integer
		Try
			ItemCount = RBLTipi.Items.Count
		Catch ex As Exception
			Exit Sub
		End Try
		If ItemCount <= 0 Then
			Exit Sub
		End If

		Dim StrCol0, StrCol1, StrText, Color As String
		StrCol0 = "<font style='FONT-SIZE: 7pt; BACKGROUND-COLOR: "
		StrCol1 = "'>&nbsp;&nbsp;&nbsp;</font>&nbsp;"

		For i As Integer = 0 To ItemCount - 1
			StrText = RBLTipi.Items(i).Text
			Color = Me.GetColorStr(RBLTipi.Items(i).Value)

			RBLTipi.Items(i).Text = StrCol0 & Color & StrCol1 & StrText	'& StrTable2
		Next
	End Sub
	Private Function GetColorStr(ByVal Index As Integer) As String
		Dim Color As String
		Select Case Index
			Case 0
				Color = "Gray" 'ok
			Case 1
				Color = "LightPink"	'ok
			Case 2
				Color = "Red" 'ok
			Case 3
				Color = "LightCoral" 'ok
			Case 4
				Color = "Yellow" 'ok
			Case 5
				Color = "Blue" 'ok
			Case 6
				Color = "Aqua" 'ok
			Case 7
				Color = "Violet" 'ok
			Case 8
				Color = "Green"	'ok
			Case 9
				Color = "Indigo" '
			Case 10
				Color = "DodgerBlue" '
			Case 11
				Color = "Brown"	'
			Case 12
				Color = "SkyBlue" '
			Case 13
				Color = "IndianRed"	'
			Case 14
				Color = "Gold" '
			Case 15
				Color = "PaleGoldenrod"	'
			Case 16
				Color = "LightGrey"	'<- !!! in html lightgray divernta lightgrEy!!! A differenza di tutto il resto del mondo... -ARGH!-
			Case 17
				Color = "Orange"
			Case 18
				Color = "BlueViolet"
			Case 19
				Color = "BurlyWood"

			Case Else
				Color = "IndianRed"
		End Select
		Return Color
	End Function
	Private Function GetColor(ByVal Index As Integer) As Drawing.Color
		Dim Color As Drawing.Color

		Select Case Index
			Case 0
				Color = Color.Gray '
			Case 1
				Color = Color.LightPink	'
			Case 2
				Color = Color.Red '
			Case 3
				Color = Color.LightCoral '
			Case 4
				Color = Color.Yellow '
			Case 5
				Color = Color.Blue '
			Case 6
				Color = Color.Aqua '
			Case 7
				Color = Color.Violet '
			Case 8
				Color = Color.Green	'
			Case 9
				Color = Color.Indigo '
			Case 10
				Color = Color.DodgerBlue '
			Case 11
				Color = Color.Brown	'
			Case 12
				Color = Color.SkyBlue '
			Case 13
				Color = Color.IndianRed	'
			Case 14
				Color = Color.Gold '
			Case 15
				Color = Color.PaleGoldenrod	'
			Case 16
				Color = Color.LightGray	 '<- !!! in html lightgray divernta lightgrEy!!! A differenza di tutto il resto del mondo... -ARGH!-
			Case 17
				Color = Color.Orange '
			Case 18
				Color = Color.BlueViolet
			Case 19
				Color = Color.BurlyWood
			Case Else
				Color = Color.IndianRed
		End Select
		Return Color
	End Function
#End Region

	Private Sub ShowContent()

		Me.PNLStat_Giorno.Visible = False
		'Me.PNLStat_Settimana.Visible = False
		'Me.PNLStat_Mese.Visible = False
		'Me.PNLStat_Anno.Visible = False

		Me.PNLIscrittiIscrizioni.Visible = False
		Me.PNLAnniConf.Visible = False
		Me.PNLRuoli.Visible = False

		Me.LBL_Data_t.Visible = True

		Me.LBdataI.Visible = False
		Me.DDLmese.Visible = False
		Me.DDLanno.Visible = False
		Me.IMBapriInizio.Visible = False

		Me.RBLvisualizzaAltro.Visible = False
		Me.RBLvisualizzaIscrizioni.Visible = False

		Me.RBLtipoChart.Visible = False

		SetUpDati(Statistiche.giornaliera)

		Select Case Me.TBSmenu.SelectedIndex
			Case Statistiche.giornaliera

				'Me.LBorganizzazione_t.Visible = False
				'Me.DDLorgnIscritti.Visible = False

				Me.LBdataI.Visible = True

				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.PNLAnniConf.Visible = True
				Else
					Me.PNLStat_Giorno.Visible = True
					Me.PNLRuoli.Visible = True
				End If

				Me.IMBapriInizio.Visible = True
				Me.RBLvisualizzaAltro.Visible = True

				Session("tab") = "giornaliera"

			Case Statistiche.settimanale
				'Me.LBorganizzazione_t.Visible = False
				'Me.DDLorgnIscritti.Visible = False

				Setup_settimana()

				If Session("tab") <> "settimanale" Then
					SetUpDati(Statistiche.settimanale)
				End If

				'Me.PNLStat_Settimana.Visible = True


				Me.LBdataI.Visible = True

				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.PNLAnniConf.Visible = True
				Else
					Me.PNLStat_Giorno.Visible = True
					Me.PNLRuoli.Visible = True
				End If
				Me.IMBapriInizio.Visible = True
				Me.RBLvisualizzaAltro.Visible = True
				Me.RBLtipoChart.Visible = True
				Session("tab") = "settimanale"

			Case Statistiche.mensile
				'Me.LBorganizzazione_t.Visible = False
				'Me.DDLorgnIscritti.Visible = False

				If Session("tab") <> "mensile" Then
					SetUpDati(Statistiche.mensile)
				End If

				'Me.PNLStat_Mese.Visible = True

				Me.DDLmese.Visible = True
				Me.DDLanno.Visible = True

				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.PNLAnniConf.Visible = True
				Else
					Me.PNLStat_Giorno.Visible = True
					Me.PNLRuoli.Visible = True
				End If

				Me.RBLvisualizzaAltro.Visible = True
				Me.RBLtipoChart.Visible = True
				Session("tab") = "mensile"

			Case Statistiche.annuale
				'Me.LBorganizzazione_t.Visible = False
				'Me.DDLorgnIscritti.Visible = False

				If Session("tab") <> "annuale" Then
					SetUpDati(Statistiche.annuale)
				End If

				'Me.PNLStat_Anno.Visible = True
				Me.DDLanno.Visible = True

				If Me.RBLvisualizzaAltro.SelectedValue = "0" Then
					Me.PNLAnniConf.Visible = True
				Else
					Me.PNLStat_Giorno.Visible = True
					Me.PNLRuoli.Visible = True
				End If

				Me.RBLvisualizzaAltro.Visible = True
				Me.RBLtipoChart.Visible = True
				Session("tab") = "annuale"

			Case Statistiche.iscritti
				'Me.LBorganizzazione_t.Visible = True
				'Me.DDLorgnIscritti.Visible = True

				Me.CTRLchart.Visible = True
				Me.CTRLchart.Visible = False

				Me.RBLvisualizzaIscrizioni.Visible = True

				If Me.RBLvisualizzaIscrizioni.SelectedValue = "1" Then
					'Me.PNLStat_Giorno.Visible = True
					Me.DDLanno.Visible = True
					Me.PNLRuoli.Visible = True
				Else
					Me.LBL_Data_t.Visible = False
					PNLIscrittiIscrizioni.Visible = True
				End If

				If Session("tab") <> "iscritti" Then
					SetUpDati(Statistiche.iscritti)
				End If
				Session("tab") = "iscritti"

			Case Else
				Session("tab") = "giornaliera"
		End Select
	End Sub

	Private Function GetScala(ByVal ValMax As Integer) As Integer
		If ValMax <= 0 Then
			ValMax = 1
		End If
		'Se avessi valori in virgola mobile, la funzione diverrebbe un double...
		'ValMax -> Valore Massimo
		'Ordine -> Ordine di grandezza
		'Step -> Step finale
		'SteTmp -> Step da elevare
		'Comparatore -> Valore per la scelta della scala
		Dim Ordine As Integer
		Dim Comparatore, PassoTmp, Passo As Double

		'Calcolo l'ordine di grandezza
		'Ordine = arrotonda.difetto(log10(ValMax)) <- Importante arrotondare per difetto...
		'Ordine = Int(log(ValMax) / 2.30258509299405)
		Ordine = Int(Math.Log10(ValMax))

		'Calcolo il "comparatore"
		Comparatore = ValMax / 10 ^ Ordine

		'Scelgo l'intervallo di step
		If Comparatore > 5 Then
			PassoTmp = 1
		ElseIf Comparatore > 2 Then
			PassoTmp = 0.5
		Else
			PassoTmp = 0.2
		End If

		'Scalo lo Step in base all'ordine di grandezza
		Passo = PassoTmp * 10 ^ Ordine

		'Siccome ho solo interi, step inferiori a 1 sono inutili...
		If Passo < 1 Then
			Passo = 1
		End If
		Return Int(Passo) 'int per maggior sicurezza... In teoria inutile... :p
		'E' possibile calcolare anche il valore di fondo scala... 
		'Fondoscala = (Int(ValMax/Passo)+1)*Passo 'In questo modo il fondo-scala è il più piccolo multiplo di Passo, maggiore di ValMax

		'Vecchio metodo...
		'Non è universale e valido per ogni valore... (Quello attuale al di la' dei blocchi su step minimo = 1 funziona sicuramente con ogni tipo di numero positivo, anche in virgola mobile... Sarebbe da verificare sui negativi...)
		'If max > 6000 Then
		'    stepScala = 1000
		'ElseIf max > 4000 Then
		'    stepScala = 200
		'ElseIf max > 1000 Then
		'    stepScala = 100
		'ElseIf max > 300 Then
		'    stepScala = 50
		'ElseIf max > 100 Then
		'    stepScala = 20
		'Else
		'    stepScala = 5
		'End If
	End Function

	Private Sub BTNesegui_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNesegui.Click
		Me.ShowContent()
		Me.BindGrafici()
		'Me.Send_Data()
	End Sub

	Private Sub CBautopostback_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBautopostback.CheckedChanged
		Dim Enable As Boolean = Me.CBautopostback.Checked

		Me.BTNesegui.Visible = Not Enable
		Me.DDLorgnIscritti.AutoPostBack = Enable
		Me.RBLtipoChart.AutoPostBack = Enable
		Me.RBLvisualizzaAltro.AutoPostBack = Enable
		Me.RBLvisualizzaIscrizioni.AutoPostBack = Enable

		Me.DDLgiorno.AutoPostBack = Enable
		Me.DDLmese.AutoPostBack = Enable
		Me.DDLanno.AutoPostBack = Enable

		Me.CBL_AnnoConf.AutoPostBack = Enable

		Me.CBtutti.AutoPostBack = Enable
		Me.CBLtipoPersona.AutoPostBack = Enable

		Me.CBtuttiIscritti.AutoPostBack = Enable
		Me.CBLtipoPersonaIscritti.AutoPostBack = Enable
		Me.CBmostraLabel.AutoPostBack = Enable
	End Sub

	Private Sub Send_Data()
		Dim oResourceConfig As New ResourceManager
		oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		Dim oDsStatistiche As New DataSet
		Dim oDataset As New DataSet	 'oDsIscritti,

		Dim giorno, mese, anno, y As Integer
		Dim oStatistiche As New COL_Statistiche(oResourceConfig.getValue("systemDBcodice"))
		Dim oTipoPersona As New COL_TipoPersona

		Select Case Me.TBSmenu.SelectedIndex
			Case Statistiche.giornaliera
				'If Me.RBLvisualizzaAltro.SelectedIndex = "1" Then
				'Andamento
				giorno = Day(CDate(Me.HDNdataI.Value))
				mese = Month(CDate(Me.HDNdataI.Value))
				anno = Year(CDate(Me.HDNdataI.Value))
				'tutti
				oDataset = oStatistiche.ElencaAccessiGiornalieri(giorno, mese, anno, -1, CInt(Me.DDLorgnIscritti.SelectedValue))
				oDataset.Tables(0).TableName = "All"
				oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				'singoli
				For y = 0 To Me.CBLtipoPersona.Items.Count - 1
					oDataset = oStatistiche.ElencaAccessiGiornalieri(giorno, mese, anno, CInt(CBLtipoPersona.Items(y).Value), CInt(Me.DDLorgnIscritti.SelectedValue))
					oDataset.Tables(0).TableName = y.ToString
					oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				Next

			Case Statistiche.settimanale
				giorno = Day(CDate(Me.HDNdataI.Value))
				mese = Month(CDate(Me.HDNdataI.Value))
				anno = Year(CDate(Me.HDNdataI.Value))
				Dim dataInizio As DateTime
				dataInizio = CType(Me.HDNdataI.Value, Date)
				'tutti
				oDataset = oStatistiche.ElencaAccessiSettimanali(dataInizio, dataInizio.AddDays(6), -1, CInt(Me.DDLorgnIscritti.SelectedValue))
				oDataset.Tables(0).TableName = "All"
				oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				'singoli
				For y = 0 To Me.CBLtipoPersona.Items.Count - 1
					oDataset = oStatistiche.ElencaAccessiSettimanali(dataInizio, dataInizio.AddDays(6), CInt(CBLtipoPersona.Items(y).Value), CInt(Me.DDLorgnIscritti.SelectedValue))
					oDataset.Tables(0).TableName = y.ToString
					oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				Next

			Case Statistiche.mensile
				mese = CInt(Me.DDLmese.SelectedValue)
				anno = CInt(Me.DDLanno.SelectedValue)
				'tutti
				oDataset = oStatistiche.ElencaAccessiMensili(mese, anno, -1, CInt(Me.DDLorgnIscritti.SelectedValue))
				oDataset.Tables(0).TableName = "All"
				oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				'singoli
				For y = 0 To Me.CBLtipoPersona.Items.Count - 1
					oDataset = oStatistiche.ElencaAccessiMensili(mese, anno, CInt(CBLtipoPersona.Items(y).Value), CInt(Me.DDLorgnIscritti.SelectedValue))
					oDataset.Tables(0).TableName = y.ToString
					oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				Next
			Case Statistiche.annuale
				anno = CInt(Me.DDLanno.SelectedValue)

				oDataset = oStatistiche.ElencaAccessiAnnuali(anno, -1, CInt(Me.DDLorgnIscritti.SelectedValue))
				oDataset.Tables(0).TableName = "All"
				oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				For y = 0 To Me.CBLtipoPersona.Items.Count - 1
					oDataset = oStatistiche.ElencaAccessiAnnuali(anno, CInt(CBLtipoPersona.Items(y).Value), CInt(Me.DDLorgnIscritti.SelectedValue))
					oDataset.Tables(0).TableName = y.ToString
					oDsStatistiche.Tables.Add(oDataset.Tables(0).Copy)
				Next
			Case Statistiche.iscritti
		End Select
		'Try
		'    Response.Clear()
		'    Response.AddHeader("Content-Disposition", "attachment; filename=" & Me.TBSmenu.SelectedIndex.ToString & ".xml")
		'    Response.ContentType = "application/xml"

		'    'text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream())
		'    oDsStatistiche.WriteXml(Response.OutputStream())
		'Catch ex As Exception
		'Finally
		'    Response.End()
		'End Try
	End Sub

#Region "Tabella Dati"
	Private Sub LabelData_Init_Year()

		If IsNothing(oResource) Then
			SetCulture(Session("LinguaCode"))
		End If

		Me.LBL_TableData.Text = ""
		Me.LBL_TableData.Text &= "<table border=1 width=900 cellspacing=0 bordercolor=#8080FF style='FONT-SIZE: 8px; FONT-FAMILY: Verdana' bgcolor=#EEF7FF>" & vbCrLf
		Me.LBL_TableData.Text &= "<tr class=ROW_TD_Titolo>" & vbCrLf

		Me.LBL_TableData.Text &= "<td>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.HT.Mese")
		Me.LBL_TableData.Text &= "</td>"

		For i As Integer = 0 To 11
			Me.LBL_TableData.Text &= "<td align=right>"
			Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.DT.Mese." & (i + 1).ToString) & "&nbsp;"	'
			'Me.LBL_TableData.Text &= (i + 1).ToString
			Me.LBL_TableData.Text &= "</td>"
		Next
		Me.LBL_TableData.Text &= "<td align=right>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("Totali") & "&nbsp;"
		Me.LBL_TableData.Text &= "</td>"
		Me.LBL_TableData.Text &= "</tr>" & vbCrLf
	End Sub
	Private Sub LabelData_Init_Iscritti()

		If IsNothing(oResource) Then
			SetCulture(Session("LinguaCode"))
		End If

		Me.LBL_TableData.Text = ""
		Me.LBL_TableData.Text &= "<table border=1 width=900 cellspacing=0 bordercolor=#8080FF style='FONT-SIZE: 8px; FONT-FAMILY: Verdana' bgcolor=#EEF7FF>" & vbCrLf
		Me.LBL_TableData.Text &= "<tr class=ROW_TD_Titolo>" & vbCrLf

		Me.LBL_TableData.Text &= "<td>" & vbCrLf
		Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.HT.Mese")
		Me.LBL_TableData.Text &= "</td>" & vbCrLf

		For i As Integer = 0 To 11
			Me.LBL_TableData.Text &= "<td align=right>" & vbCrLf
			Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.DT.Mese." & (i + 1).ToString) & "&nbsp;"
			'Me.LBL_TableData.Text &= (i + 1).ToString
			Me.LBL_TableData.Text &= "</td>" & vbCrLf
		Next
		Me.LBL_TableData.Text &= "<td align=right>" & vbCrLf
		Me.LBL_TableData.Text &= Me.oResource.getValue("Totali") & "&nbsp;"
		Me.LBL_TableData.Text &= "</td>" & vbCrLf
		Me.LBL_TableData.Text &= "</tr>" & vbCrLf
	End Sub
	Private Sub LabelData_Init_Week()
		If IsNothing(oResource) Then
			SetCulture(Session("LinguaCode"))
		End If
		Me.LBL_TableData.Text = ""
		Me.LBL_TableData.Text &= "<table border=1 width=900 cellspacing=0 bordercolor=#8080FF style='FONT-SIZE: 8px; FONT-FAMILY: Verdana' bgcolor=#EEF7FF>" & vbCrLf
		Me.LBL_TableData.Text &= "<tr class=ROW_TD_Titolo>" & vbCrLf

		Me.LBL_TableData.Text &= "<td>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.HT.Giorno")
		Me.LBL_TableData.Text &= "</td>"

		For i As Integer = 0 To 6
			Me.LBL_TableData.Text &= "<td align=right>"
			Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.DT.Giorno." & (i + 1).ToString) '
			Me.LBL_TableData.Text &= "</td>"
		Next
		Me.LBL_TableData.Text &= "<td align=right>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("Totali") & "&nbsp;"
		Me.LBL_TableData.Text &= "</td>"
		Me.LBL_TableData.Text &= "</tr>" & vbCrLf
	End Sub
	Private Sub LabelData_Init_Day()
		If IsNothing(oResource) Then
			SetCulture(Session("LinguaCode"))
		End If
		Me.LBL_TableData.Text = ""
		Me.LBL_TableData.Text &= "<table border=1 width=900 cellspacing=0 bordercolor=#8080FF style='FONT-SIZE: 8px; FONT-FAMILY: Verdana' bgcolor=#EEF7FF>" & vbCrLf
		Me.LBL_TableData.Text &= "<tr class=ROW_TD_Titolo>" & vbCrLf

		Me.LBL_TableData.Text &= "<td>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.HT.Ora")	'Internazionalizzare!
		Me.LBL_TableData.Text &= "</td>"

		For i As Integer = 0 To 23
			Me.LBL_TableData.Text &= "<td align=right>"
			Me.LBL_TableData.Text &= i.ToString & ".00"
			Me.LBL_TableData.Text &= "</td>"
		Next

		Me.LBL_TableData.Text &= "<td align=right>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("Totali") & "&nbsp;"
		Me.LBL_TableData.Text &= "</td>"

		Me.LBL_TableData.Text &= "</tr>" & vbCrLf
	End Sub
	Private Sub LabelData_Init_Month(ByVal MonthDay As Integer)
		If IsNothing(oResource) Then
			SetCulture(Session("LinguaCode"))
		End If

		Me.LBL_TableData.Text = ""
		Me.LBL_TableData.Text &= "<table border=1 width=900 cellspacing=0 bordercolor=#8080FF style='FONT-SIZE: 8px; FONT-FAMILY: Verdana' bgcolor=#EEF7FF>" & vbCrLf
		Me.LBL_TableData.Text &= "<tr class=ROW_TD_Titolo>" & vbCrLf

		Me.LBL_TableData.Text &= "<td>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("TBData.HT.Giorno") 'Internazionalizzare!
		Me.LBL_TableData.Text &= "</td>"

		For i As Integer = 0 To MonthDay
			Me.LBL_TableData.Text &= "<td align=right>"
			Me.LBL_TableData.Text &= (i + 1).ToString & "&nbsp;"
			Me.LBL_TableData.Text &= "</td>"
		Next
		Me.LBL_TableData.Text &= "<td align=right>"
		Me.LBL_TableData.Text &= Me.oResource.getValue("Totali") & "&nbsp;"
		Me.LBL_TableData.Text &= "</td>"
		Me.LBL_TableData.Text &= "</tr>" & vbCrLf
	End Sub

	Private Sub LabelData_OpenRow(Optional ByVal RowLabel As String = "", Optional ByVal align As Integer = 0)
		Me.LBL_TableData.Text &= "<tr class=ROW_TD_Small>" & vbCrLf

		If align = 1 Then
			Me.LBL_TableData.Text &= "<td align=center>"
		ElseIf align = 2 Then
			Me.LBL_TableData.Text &= "<td align=left>"
		Else
			Me.LBL_TableData.Text &= "<td align=right>"
		End If


		'Me.LBL_TableData.Text &= "<td align=right>"
		Me.LBL_TableData.Text &= RowLabel & "&nbsp;"
		Me.LBL_TableData.Text &= "</td>"
	End Sub
	Private Sub LabelData_CloseRow()
		Me.LBL_TableData.Text &= "</tr>" & vbCrLf
	End Sub
	Private Sub LabelData_AddItem(ByVal Item As String, Optional ByVal align As Integer = 0)
		If Item = "" Then
			Item = "0"
		End If
		If align = 1 Then
			Me.LBL_TableData.Text &= "<td align=center>"
		ElseIf align = 2 Then
			Me.LBL_TableData.Text &= "<td align=left>"
		Else
			Me.LBL_TableData.Text &= "<td align=right>"
		End If

		Me.LBL_TableData.Text &= Item
		Me.LBL_TableData.Text &= "</td>"
	End Sub
	Private Sub LabelData_Close()

		Me.LBL_TableData.Text &= "</table>" & vbCrLf

	End Sub

	Private Sub LNK_DownloadData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNK_DownloadData.Click
		Dim FileName As String
		FileName = "COL_Statistiche"
		'select case Session("tab") = "iscritti"
		Dim IndiceSelezionato As String = ""

		Try
			IndiceSelezionato = Session("tab")
		Catch ex As Exception

		End Try
		Select Case IndiceSelezionato
			Case "annuale"
				FileName &= "_Annuali"
			Case "mensile"
				FileName &= "_" & Me.DDLmese.SelectedValue
			Case "settimanale"
				FileName &= "_" & Me.HDNdataI.Value
			Case "giornaliera"
				FileName &= "_" & Me.HDNdataI.Value
			Case "iscritti"
				FileName &= "_Iscritti" & Me.DDLanno.SelectedValue
		End Select

		FileName &= ".xls"

		Response.Clear()
		Response.AddHeader("Content-Disposition", "attachment; filename=" & FileName)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Html As String = String.Format("<html><head><META charset=utf-8/><meta name=ProgId content=Excel.Sheet></head><body> {0} </body>", Me.LBL_TableData.Text.Replace("FONT-SIZE: 8px;", "FONT-SIZE: 14px;"))

        Response.Write(Html)

        'Response.Write(Me.LBL_TableData.Text.Replace("FONT-SIZE: 8px;", "FONT-SIZE: 14px;"))
		Response.End()
	End Sub

#End Region

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
End Class