Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2


Public Class CalendarioSettimanale
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

	Private _Utility As OLDpageUtility

	Private ReadOnly Property Utility() As OLDpageUtility
		Get
			If IsNothing(_Utility) Then
				_Utility = New OLDpageUtility(Me.Context)
			End If
			Return _Utility
		End Get
	End Property

#Region "calendario"
    Protected WithEvents RPTore As System.Web.UI.WebControls.Repeater
    Protected WithEvents RPTgiorni As System.Web.UI.WebControls.Repeater
    Protected WithEvents RPTpulsanti As System.Web.UI.WebControls.Repeater
    Protected WithEvents PNLLeft As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLsettimana As System.Web.UI.WebControls.Table
    Protected WithEvents LKBexport As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBLExport As System.Web.UI.WebControls.Table
#End Region

#Region "Permessi"
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
#End Region

#Region "pannello filtri"
    Protected WithEvents LBFiltroComunita As System.Web.UI.WebControls.Label
    Protected WithEvents RBLFiltroComunita As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBsettAttuale As System.Web.UI.WebControls.Label
    Protected WithEvents DDLfiltraOre As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBXLfiltroGiorni As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents BTNfiltraGiorni As System.Web.UI.WebControls.Button
    Protected WithEvents PNLfiltroGiorni As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNApplicaFiltroG As System.Web.UI.WebControls.Button
    Protected WithEvents PNLfiltriSettimana As System.Web.UI.WebControls.Panel
    Protected WithEvents LKBbackTOall As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLcalendarioGiornaliero As System.Web.UI.WebControls.Panel
    Protected WithEvents CLNmini As System.Web.UI.WebControls.Calendar
    Protected WithEvents PNLRight As System.Web.UI.WebControls.Panel
    Protected WithEvents LKBvediCALmini As System.Web.UI.WebControls.LinkButton
    Protected WithEvents CBXLFiltroEventi As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents DDLVaiA_mesi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLVaiA_anni As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBCalMini As System.Web.UI.WebControls.Table
    Protected WithEvents BTNVaiA_oggi As System.Web.UI.WebControls.Button
    Protected WithEvents CBEventiTutti As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNApplicaFiltroEventi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNfiltroEventi As System.Web.UI.WebControls.Button
    Protected WithEvents LKBtuttiIgiorni As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBsettSucc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBsettPrec As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBgoTOmensile As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBgoTOannuale As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBGestEVNT As System.Web.UI.WebControls.Label
    Protected WithEvents LBLFiltri As System.Web.UI.WebControls.Label
    Protected WithEvents LBLfiltroORE As System.Web.UI.WebControls.Label
    Protected WithEvents LBLfiltroEventi As System.Web.UI.WebControls.Label
    Protected WithEvents LBLCLNminiMESE As System.Web.UI.WebControls.Label
    Protected WithEvents LBLCLNminiANNO As System.Web.UI.WebControls.Label
    Protected WithEvents LBLvaiA As System.Web.UI.WebControls.Label
#End Region

#Region "dettaglio evento"

    Protected WithEvents PNLdettaglioEvento As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNomeEvento As System.Web.UI.WebControls.Label
    Protected WithEvents LBComunita As System.Web.UI.WebControls.Label
    Protected WithEvents LBTipo As System.Web.UI.WebControls.Label
    Protected WithEvents LBInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBfine As System.Web.UI.WebControls.Label
    Protected WithEvents HLLink As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LBLuogo As System.Web.UI.WebControls.Label
    Protected WithEvents LBaula As System.Web.UI.WebControls.Label
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LKBzoom As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBInserisciEvento As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBmodifica As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBelimina As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBtrova As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBchiudiDettaglio As System.Web.UI.WebControls.LinkButton
    Protected WithEvents BTNElimina As System.Web.UI.WebControls.Button
    Protected WithEvents BTNModifica As System.Web.UI.WebControls.Button
    Protected WithEvents BTNAnnulla As System.Web.UI.WebControls.Button
    Protected WithEvents RBLEliminaEventi As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TBLElimina As System.Web.UI.WebControls.Table
    Protected WithEvents LBLMessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoNome As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoCNMT As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoTipo As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoLink As System.Web.UI.WebControls.Label
    Protected WithEvents LBLdettEventoLuogo As System.Web.UI.WebControls.Label
    Protected WithEvents LBLVisualizzazione As System.Web.UI.WebControls.Label
    Protected WithEvents LKBIcalendar As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBcreaReminder As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBAvviso As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipoAvviso As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNCreaReminder As System.Web.UI.WebControls.Button
    Protected WithEvents BTNAnnullaCreazRem As System.Web.UI.WebControls.Button
#End Region

    Protected oDataset As DataSet

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


        Dim oPersona As New COL_Persona

        Try
            If Page.IsPostBack = False Then
                Me.Setup_Internazionalizzazione()
            End If
            Try
                If Session("idComunita") > 0 Then
                    Dim oServizio As New Services_Eventi
                    Try
                        oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                        If oServizio.PermessiAssociati = "" Then
                            oServizio.PermessiAssociati = "00000000000000000000000000000000"
                        End If
                    Catch ex As Exception
                        oServizio.PermessiAssociati = "00000000000000000000000000000000"
                    End Try
                    Try
                        If oServizio.ChangeEvents Or oServizio.DelEvents Or oServizio.AdminService Or oServizio.AddEvents Or oServizio.ReadEvents Then
                            PNLcontenuto.Visible = True
                            PNLpermessi.Visible = False
                            If Not Page.IsPostBack Then

                                If oServizio.AdminService Then
                                    oResource.setLinkButton(LKBInserisciEvento, True, True)
                                    oResource.setLinkButton(LKBmodifica, True, True)
                                    oResource.setLinkButton(LKBelimina, True, True)
                                End If

                                If oServizio.ChangeEvents Then
                                    oResource.setLinkButton(LKBelimina, True, True, True)
                                    oResource.setLinkButton(LKBmodifica, True, True)
                                End If

                                If oServizio.DelEvents Then
                                    oResource.setLinkButton(LKBmodifica, True, True, True)
                                    oResource.setLinkButton(LKBelimina, True, True)
                                End If

                                If oServizio.AddEvents Then
                                    oResource.setLinkButton(LKBmodifica, True, True, True)
                                    oResource.setLinkButton(LKBelimina, True, True, True)
                                    oResource.setLinkButton(LKBInserisciEvento, True, True)
                                End If
                            End If
                        Else
                            PNLcontenuto.Visible = False
                            oResource.setLabel(LBnopermessi)
                            If Not Page.IsPostBack Then
                                PNLpermessi.Visible = True
                                LBnopermessi.Visible = True
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                Else
                    PNLcontenuto.Visible = True
                    PNLpermessi.Visible = False
                End If
            Catch ex As Exception
                PNLcontenuto.Visible = True
                PNLpermessi.Visible = False
            End Try

            If Not Page.IsPostBack Then
                Setup_Script()
                setupFiltri()
            End If
            setupSettimana()
        Catch ex As Exception
            PNLcontenuto.Visible = False
            PNLpermessi.Visible = True
            LBnopermessi.Visible = True
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

#Region "Bind_Dati"

    Private Sub setupFiltri()
        'setup DDL ore da visualizzare sul calendario
        DDLfiltraOre.ToolTip() = "ore da visualizzare sul calendario"
        DDLfiltraOre.Items.Clear()
        DDLfiltraOre.Items.Add(New ListItem("08:00 - 19.00", "8_19"))
        DDLfiltraOre.Items.Add(New ListItem("09:00 - 18.00", "9_18"))
        DDLfiltraOre.Items.Add(New ListItem("00:00 - 24.00", "0_24"))
        If Session("oraFine") Is Nothing Or Session("oraInizio") Is Nothing Then
            DDLfiltraOre.Items(0).Selected = True
        Else
            DDLfiltraOre.SelectedValue = Session("oraInizio") & "_" & Session("oraFine")
        End If
        'setup Check Box List giorni da visualizzare sul calendario
        CBXLfiltroGiorni.Items.Clear()

        Dim giorno As Date
        giorno = Date.Parse("01/01/" & Now.Year)

        If giorno.DayOfWeek = 0 Then
            giorno = giorno.AddDays(-6)
        Else
            giorno = giorno.AddDays(1 - giorno.DayOfWeek)
        End If
        Dim j As Integer

        For j = 0 To 6
            CBXLfiltroGiorni.Items.Add(New ListItem(giorno.AddDays(j).ToString("dddd", oResource.CultureInfo.DateTimeFormat), CInt(giorno.AddDays(j).DayOfWeek)))
        Next

        For Each LIST As ListItem In CBXLfiltroGiorni.Items
            LIST.Selected = True
            Dim i As Integer
            i = LIST.Attributes.Count()
            'LIST.Attributes.AddAttribute()
        Next
        oResource.setRadioButtonList(RBLEliminaEventi, "0")
        oResource.setRadioButtonList(RBLEliminaEventi, "1")
        oResource.setRadioButtonList(RBLFiltroComunita, "-1")
        oResource.setRadioButtonList(RBLFiltroComunita, "0")
        Bind_FiltroEventi()
        Bind_CambioAnno()
    End Sub

    Private Sub Bind_FiltroEventi()
        Dim oDataset_TPEV As New DataSet
        Dim oTipoEvento As New COL_Tipo_Evento
        Dim i As Integer
        CBXLFiltroEventi.Items.Clear()
        'bind DDL per filtro tipo evento
        Try
            oDataset_TPEV = oTipoEvento.Elenca(CInt(Session("LinguaID")))
            If oDataset_TPEV.Tables(0).Rows.Count > 0 Then
                CBXLFiltroEventi.DataSource = oDataset_TPEV.Tables(0).DefaultView
                CBXLFiltroEventi.DataTextField = "TPEV_nome"
                CBXLFiltroEventi.DataValueField = "TPEV_id"
                CBXLFiltroEventi.DataBind()
            End If

            CBXLFiltroEventi.Items.Insert(CBXLFiltroEventi.Items.Count, New ListItem(oResource.getValue("CBXLFiltroEventi.-1"), -1))

            For Each LIST As ListItem In CBXLFiltroEventi.Items
                LIST.Selected = True
            Next

        Catch ex As Exception
            CBXLFiltroEventi.Items.Clear()
            CBXLFiltroEventi.Items.Insert(0, New ListItem(oResource.getValue("CBXLFiltroEventi.-2"), -2))
        End Try

    End Sub

    Private Sub Bind_CambioAnno()
        Dim i As Integer
        Dim giorno As Date
        giorno = Date.Parse("01/01/" & Now.Year)
        'bind DDL per cambiare mese sul calendario di dx
        DDLVaiA_mesi.Items.Clear()
        For i = 0 To 11
            DDLVaiA_mesi.Items.Add(New ListItem(giorno.ToString("MMMM", oResource.CultureInfo.DateTimeFormat), CStr(i + 1)))
            giorno = giorno.AddMonths(1)
        Next
        Try
            DDLVaiA_anni.Items.Clear()
            'bind DDL cambio anno sul calendario di dx
            Dim annoMinMax(1) As Integer
            Dim OOrario As New COL_Orario
            Dim AnnoMin, AnnoMax As Int16
            annoMinMax = OOrario.getAnni_minMAX   'caricamento anni min-max da eventi presenti nel DB
            AnnoMin = annoMinMax(0) - 2
            AnnoMax = annoMinMax(1) + 2

            If AnnoMin < 0 Then
                AnnoMin = Year(Now) - 2
                AnnoMax = AnnoMin + 2
            End If

            For i = 0 To (AnnoMax - AnnoMin)
                DDLVaiA_anni.Items.Add(New ListItem(CStr(AnnoMin + i), CInt(AnnoMin + i)))
            Next
            DDLVaiA_mesi.SelectedValue = Now.Month
            DDLVaiA_anni.SelectedValue = Now.Year
        Catch ex As Exception
            DDLVaiA_anni.Items.Add(New ListItem(CStr(Now.Year.ToString("YYYY")), CInt(Now.Year)))
        End Try
    End Sub
#End Region

#Region "Settaggio dati iniziali"

    Private Sub Setup_Script()
        If Session("dtInizioSett") = Nothing Then
            If Date.Today.DayOfWeek = 0 Then
                Session("dtInizioSett") = Date.Today.AddDays(-6)
            Else
                Session("dtInizioSett") = Date.Today.AddDays(1 - Date.Today.DayOfWeek)
            End If
        End If

        CLNmini.VisibleDate = CDate(Session("dtInizioSett"))

        If Session("oraInizio") = Nothing Then
            Session("oraInizio") = 8   'ora inizio visualizzata sul calendario
        End If

        If Session("oraFine") = Nothing Then
            Session("oraFine") = 19   'ora fine visualizzata sul calendario
        End If

        Session("Evento_CMNT_id") = Nothing


        Try
            If Request.Cookies("CalendarioSett")("RBLComunita") = "-1" Or Request.Cookies("CalendarioSett")("RBLComunita") = "0" Then
                RBLFiltroComunita.SelectedValue = Request.Cookies("CalendarioSett")("RBLComunita")
            Else
                RBLFiltroComunita.SelectedValue = 0
            End If
        Catch ex As Exception
            RBLFiltroComunita.SelectedValue = 0
        End Try

        Dim RuoloID As Integer
        Dim ComunitaID As Integer = -1

        Try
            ComunitaID = Session("idComunita")
        Catch ex As Exception

        End Try
        Try
            RuoloID = Session("IdRuolo")
        Catch ex As Exception

        End Try
        If ComunitaID > 0 Then
            LBFiltroComunita.Visible = True
            RBLFiltroComunita.Visible = True
            If RuoloID = Main.TipoRuoloStandard.AccessoNonAutenticato Then
                Me.RBLFiltroComunita.Enabled = False
                Me.RBLFiltroComunita.SelectedValue = 0
            Else
                RBLFiltroComunita.Enabled = True
            End If
        Else
            LBFiltroComunita.Visible = False
            RBLFiltroComunita.Visible = False
        End If
        Session("giorniVisual") = "1234560"   'stringa giorni visualizzati sul calendario (per filtro CBL)

    End Sub

#End Region

#Region "Caricamento eventi da DB"

    Private Sub setupSettimana()
        Dim dataInizio As Date
        dataInizio = CType(Session("dtInizioSett"), Date)
        LBsettAttuale.Text = dataInizio.ToString("d MMM yy", oResource.CultureInfo.DateTimeFormat) & " - " & dataInizio.AddDays(6).ToString("d MMM yy", oResource.CultureInfo.DateTimeFormat)
        Dim oEvento As New COL_Evento
        Dim oDatasetReminder As DataSet
        Dim oReminder As New COL_Reminder
        Dim oPersona As New COL_Persona
        Dim i As Integer



        Try
            Dim oServizio As New Services_Eventi

            Dim tmp As String

            Dim IDcmt As Integer

            Try
                If Not Session("idComunita") Is Nothing And RBLFiltroComunita.SelectedValue = 0 Then
                    IDcmt = CInt(Session("idComunita"))
                Else
                    IDcmt = -1
                End If
            Catch ex As Exception
                IDcmt = -1
            End Try

            tmp = dataInizio.ToString("dd/MM/yy")
            oPersona = Session("objPersona")
            oEvento.TipoEvento.Id = -1

            Dim tipoEvento As String
            tipoEvento = ","
            For Each LIST As ListItem In CBXLFiltroEventi.Items
                If LIST.Selected Then
                    tipoEvento = tipoEvento & LIST.Value & ","
                End If
            Next

            oReminder.idPersona = oPersona.Id

            If CBXLFiltroEventi.Items.FindByValue("-1").Selected Or CBEventiTutti.Checked Then
				oDatasetReminder = oReminder.Estrai(dataInizio, dataInizio.AddDays(6), 0)
            End If

            'caricamento eventi della settimana selezionata, associati alla persona o della comunità corrente o di tutte le comunita (IDcmt=-1)
			oDataset = oEvento.TrovaEventiSettimanaliPersona(oPersona.Id, DateToString(dataInizio, False), DateToString(dataInizio.AddDays(6), False), oServizio.Codex, IDcmt, tipoEvento, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)


            Try
                If oDatasetReminder.Tables(0).Rows.Count > 0 Then
                    Dim oRow As DataRow
                    For i = 0 To oDatasetReminder.Tables(0).Rows.Count - 1
                        Try
                            oRow = oDataset.Tables(0).NewRow
                        Catch ex As Exception
                            oDataset.Tables.Add()
                            oDataset.Tables(0).Columns.Add(New DataColumn("TPEV_id"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_id"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_nome"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_inizio"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_fine"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("TPEV_icon"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_id"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_nome"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("TPEV_nome"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_link"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_luogo"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_aula"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_visibile"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_note"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("annoAccademico"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_macro"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("PREV_ProgrammaSvolto"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_ripeti"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_perpetuo"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_id"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_annoAccademico"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("dataRender"))
                            oDataset.Tables(0).Columns.Add(New DataColumn("Creatore"))
                            oRow = oDataset.Tables(0).NewRow
                        End Try
                        oRow.Item("TPEV_id") = -1
                        oRow.Item("ORRI_id") = CInt(oDatasetReminder.Tables(0).Rows(i).Item("RMND_id")) * -1
                        oRow.Item("EVNT_nome") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_oggetto")
                        oRow.Item("ORRI_inizio") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")
                        oRow.Item("ORRI_fine") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_fine")
                        oRow.Item("TPEV_icon") = "#fffee7"
                        oRow.Item("CMNT_id") = "0"
                        oRow.Item("CMNT_nome") = "--"
                        oRow.Item("TPEV_nome") = oResource.getValue("CBXLFiltroEventi.-1")
                        oRow.Item("ORRI_link") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_link")
                        oRow.Item("EVNT_luogo") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_luogo")
                        oRow.Item("ORRI_aula") = ""
                        oRow.Item("ORRI_visibile") = Not oDatasetReminder.Tables(0).Rows(i).Item("RMND_Sospeso")
                        oRow.Item("EVNT_macro") = 0
                        oRow.Item("Creatore") = "--"
                        oRow.Item("dataRender") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")
                        oDataset.Tables(0).Rows.Add(oRow)
                    Next
                End If
            Catch ex As Exception

            End Try

            i = 0

            Try
                If oDataset.Tables(0).Rows.Count > 0 Then
                    Dim giorni As String
                    Dim j As Integer
                    giorni = Session("giorniVisual")
                    For j = 0 To 6
                        If giorni.IndexOf(dataInizio.DayOfWeek) > -1 Then
                            For i = 0 To oDataset.Tables(0).Rows.Count - 1
                                If CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")).Date = dataInizio.Date And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")) = CDate(oDataset.Tables(0).Rows(i).Item("dataRender")) Then
                                    If (CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")).Hour < Session("oraInizio") And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_fine")).Hour = Session("oraInizio") And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_fine")).Minute > 0) Or (CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")).Hour < Session("oraInizio") And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_fine")).Hour > Session("oraInizio")) Or (CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")).Hour < Session("oraInizio") And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_fine")).Date > dataInizio.Date) Then
                                        oDataset.Tables(0).Rows(i).Item("dataRender") = dataInizio.ToShortDateString & " " & Session("oraInizio") & ":00:00.000"
                                    End If
                                Else
                                    If CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")) = CDate(oDataset.Tables(0).Rows(i).Item("dataRender")) And (giorni.IndexOf(CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")).DayOfWeek) < 0 Or CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")).Hour > CInt(Session("orafine")) - 1) Then
                                        If CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")) <= CDate(dataInizio & " " & Session("oraInizio") & ":00:00.000") And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_fine")) > CDate(dataInizio & " " & Session("oraInizio") & ":00:00.000") And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_fine")) <= CDate(dataInizio & " " & CStr(CInt(Session("orafine")) - 1) & ":59:59.000") Then
                                            'evento che inizia prima dell'orario selezionato e finisce dopo l'orario selezionato
                                            oDataset.Tables(0).Rows(i).Item("dataRender") = dataInizio & " " & Session("oraInizio") & ":00:00.000"
                                        End If
                                        If CDate(oDataset.Tables(0).Rows(i).Item("ORRI_inizio")) <= CDate(dataInizio & " " & Session("oraInizio") & ":00:00.000") And CDate(oDataset.Tables(0).Rows(i).Item("ORRI_fine")) > CDate(dataInizio & " " & Session("oraInizio") & ":00:00.000") Then
                                            'evento che inizia prima della data selzionata
                                            oDataset.Tables(0).Rows(i).Item("dataRender") = dataInizio & " " & Session("oraInizio") & ":00:00.000"
                                        End If
                                    End If
                                End If
                            Next
                        End If
                        dataInizio = dataInizio.AddDays(1)
                    Next


                End If

            Catch ex As Exception

            End Try

        Catch ex As Exception
        End Try

        Try
            setupPulsanti()
            creaRigaOre()

            PNLdettaglioEvento.Visible = False
            oResource.setLinkButton(LKBmodifica, True, True, True)
            oResource.setLinkButton(LKBelimina, True, True, True)
            TBCalMini.Visible = True
            LKBvediCALmini.Visible = False

            If Not Session("ORRI_id") = Nothing Then
                setupDettaglioEvento()

            End If
            oDataset.Dispose()
            oDatasetReminder.Dispose()
        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Bind repeater generazione calendario"

    Private Sub creaRigaOre()
        'creazione tabella ore per repeater creazione calendario (verticale)
        Dim i As Integer
        Dim dsOre As DataSet = New DataSet
        Dim tabOre As DataTable = New System.Data.DataTable
        Dim colOre As DataColumn = New System.Data.DataColumn
        Dim colID As DataColumn = New System.Data.DataColumn
        Dim rowOre As DataRow
        tabOre.TableName = "tabellaOre"
        colOre.DataType = System.Type.GetType("System.String")
        colOre.ColumnName = "rigaOre"
        tabOre.Columns.Add(colOre)
        colID.DataType = System.Type.GetType("System.String")
        colID.ColumnName = "rigaID"
        tabOre.Columns.Add(colID)
        For i = CInt(Session("oraInizio")) To CInt(Session("oraFine")) - 1
            rowOre = tabOre.NewRow()
            rowOre.Item("rigaOre") = i.ToString("00") & ":00"
            rowOre.Item("rigaID") = i.ToString("00") & ":00"
            tabOre.Rows.Add(rowOre)
            rowOre = tabOre.NewRow()
            rowOre.Item("rigaOre") = i.ToString("00") & ":30"
            rowOre.Item("rigaID") = i.ToString("00") & ":30"
            tabOre.Rows.Add(rowOre)
        Next
        dsOre.Tables.Add(tabOre)
        RPTore.DataSource = dsOre.Tables("tabellaOre")
        RPTore.DataBind()
    End Sub

    Private Sub RPTore_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTore.ItemCreated
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                setupGiorni(CType(e.Item.FindControl("RPTgiorni"), Repeater), e.Item.ItemIndex, RPTore.DataSource.Rows(e.Item.ItemIndex).Item("rigaID"))
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub setupGiorni(ByVal RPTgiorni As Repeater, ByVal riga As Integer, ByVal ora As String)
        'creazione celle giorno calendario
        Dim i As Integer
        Dim dsGiorni As DataSet = New DataSet
        Dim tabGiorni As DataTable = New System.Data.DataTable
        Dim colGiorniData As DataColumn = New System.Data.DataColumn
        Dim colGiorniOra As DataColumn = New System.Data.DataColumn
        Dim rowGiorni As DataRow
        Dim giorni As String

        giorni = Session("giorniVisual")
        Try
            tabGiorni.TableName = "tabellaGiorni"
            colGiorniData.DataType = System.Type.GetType("System.String")
            colGiorniData.ColumnName = "giorno"
            tabGiorni.Columns.Add(colGiorniData)
            colGiorniOra.DataType = System.Type.GetType("System.String")
            colGiorniOra.ColumnName = "ora"
            tabGiorni.Columns.Add(colGiorniOra)
            For i = 1 To 7
                If giorni.IndexOf(CDate(Session("dtInizioSett")).AddDays(i - 1).DayOfWeek) > -1 Then
                    rowGiorni = tabGiorni.NewRow()
                    rowGiorni.Item("giorno") = CDate(Session("dtInizioSett")).AddDays(i - 1).ToString("dd/MM/yyyy")
                    rowGiorni.Item("ora") = ora
                    tabGiorni.Rows.Add(rowGiorni)
                End If
            Next
            dsGiorni.Tables.Add(tabGiorni)
            RPTgiorni.DataSource = dsGiorni.Tables("tabellaGiorni")
            RPTgiorni.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Public Sub RPTgiorni_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim cellaGiorno, cellaColore As System.Web.UI.HtmlControls.HtmlTableCell
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try
                cellaGiorno = CType(e.Item.FindControl("CellaG"), System.Web.UI.HtmlControls.HtmlTableCell)
                cellaColore = CType(e.Item.FindControl("CellaColore"), System.Web.UI.HtmlControls.HtmlTableCell)
                cellaGiorno.ID = CStr(e.Item.DataItem("giorno")) & "_" & CStr(e.Item.DataItem("ora"))
                Dim link As String
                link = "inserimentoevento.aspx?giorno=" & CStr(e.Item.DataItem("giorno")) & "&ora=" & CStr(e.Item.DataItem("ora"))
                cellaGiorno.Attributes.Add("ondblclick", "window.open('" & link & "',target='_top');")
                If CDate(e.Item.DataItem("giorno")).Date = Now.Date Then
                    cellaGiorno.BgColor = "#fff4c0"
                End If

                cellaColore.Width = "4"
                Dim numGiorni As Integer
                For Each LIST As ListItem In CBXLfiltroGiorni.Items
                    If LIST.Selected = True Then
                        numGiorni += 1
                    End If
                Next
                cellaGiorno.Width = CInt(630 / numGiorni)

                'richiamo funzione per scrittura eventi nel singolo giorno
                scriviEventi(cellaGiorno, cellaColore)
            Catch ex As Exception

            End Try
        End If
    End Sub

#End Region

#Region "scrittura eventi sul calendario"

    Private Sub scriviEventi(ByVal cellaG As System.Web.UI.HtmlControls.HtmlTableCell, ByVal cellaC As System.Web.UI.HtmlControls.HtmlTableCell)
        Dim butt As Button
        Dim giorno, ora As String
        Dim RowEventiGiorno() As DataRow
        Dim ColoreEvento, ColTemp As String
        Dim i, numGiorni As Integer
        ora = cellaG.ID.Substring(cellaG.ID.IndexOf("_") + 1)
        giorno = cellaG.ID.Substring(0, cellaG.ID.Length - ora.Length - 1)
        Dim StringaFiltro As String
        Dim inizio, fine As Date

        ' controllo quanti giorni sono selezionati
        For Each LIST As ListItem In CBXLfiltroGiorni.Items
            If LIST.Selected = True Then
                numGiorni += 1
            End If
        Next

        'settaggio stringa per select su dataset (solo eventi del giorno)

        If ora.Substring(3, 2).CompareTo("00") = 0 Then
            'es: se sono in una cella dalle 8.00 alle 8.29
            inizio = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "00:00")
            fine = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "29:59")
            StringaFiltro = "(dataRender >= '" & inizio.ToString & "') AND (dataRender < '" & fine.ToString & "')"
        Else
            'es: se sono in una cella dalle 8.30 alle 8.59
            inizio = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "30:00")
            fine = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "59:59")
            StringaFiltro = "(dataRender >= '" & inizio.ToString & "') AND (dataRender < '" & fine.ToString & "')"
        End If

        Try
            RowEventiGiorno = oDataset.Tables(0).Select(StringaFiltro)

            Dim LKB_temp As LinkButton
            LKB_temp = CType(cellaG.FindControl("LKB_temp"), LinkButton)

            If PNLcalendarioGiornaliero.Visible = True Then
                'controllo se è stato selezionato un giorno solo per adattare la colonna di quel giorno
                cellaG.Width = "600"
                cellaG.Height = "30"
            End If
            Dim toolTipPulsante As String
            For i = 0 To RowEventiGiorno.Length - 1

                If i = 0 Then
                    'settaggio colore cella come colore associato all'evento
                    For Each LIST As ListItem In CBXLFiltroEventi.Items
                        If RowEventiGiorno(i).Item("TPEV_id") = LIST.Value And list.Selected Then
                            ColoreEvento = RowEventiGiorno(0).Item("TPEV_icon").ToString
                            Exit For
                        End If
                    Next
                Else
                    'settaggio colore cella con controllo del tipo evento
                    For Each LIST As ListItem In CBXLFiltroEventi.Items
                        If RowEventiGiorno(i).Item("TPEV_id") = LIST.Value And list.Selected Then
                            ColTemp = RowEventiGiorno(i).Item("TPEV_icon").ToString
                            Exit For
                        End If
                    Next
                    If ColoreEvento.CompareTo(ColTemp) = 0 Then
                    Else
                        ColoreEvento = "#dcdcdc"  'colore per cella con più tipi di evento diversi fra loro
                    End If
                End If
                LKB_temp = New LinkButton


                AddHandler LKB_temp.Click, AddressOf DettaglioEvento

                LKB_temp.Visible = True
                Dim nomeEvento As String
                nomeEvento = RowEventiGiorno(i).Item("EVNT_nome").ToString

                If RowEventiGiorno(i).Item("ORRI_inizio") <> RowEventiGiorno(i).Item("dataRender") Then
                    nomeEvento = "^ " & nomeEvento
                End If


                If PNLcalendarioGiornaliero.Visible = True Then
                    'troncamento nome evento a 130 chars se è visualizzato solo un giorno
                    If nomeEvento.Length > 130 Then
                        nomeEvento = nomeEvento.Substring(0, 130) & "..."
                    End If
                    'nome evento con ora e if sulla data inizio e fine

                    LKB_temp.Text = "&nbsp;&nbsp;" & CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm", oResource.CultureInfo.DateTimeFormat) & " - " & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm", oResource.CultureInfo.DateTimeFormat) & "  " & nomeEvento

                    If CDate(RowEventiGiorno(i).Item("ORRI_inizio")).Date < inizio.Date Then
                        LKB_temp.Text = "^ " & CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm '-' d/MM/yy ", oResource.CultureInfo.DateTimeFormat) & " - " & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm", oResource.CultureInfo.DateTimeFormat) & "  " & nomeEvento.Substring(2)
                    End If

                    If CDate(RowEventiGiorno(i).Item("ORRI_fine")).Date > fine.Date Then
                        LKB_temp.Text = CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm") & " - " & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm '-' d/MM/yy") & "  " & nomeEvento
                    End If

                    If CDate(RowEventiGiorno(i).Item("ORRI_inizio")).Date < inizio.Date And CDate(RowEventiGiorno(i).Item("ORRI_fine")).Date > fine.Date Then
                        LKB_temp.Text = "^ " & CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm '-' d/MM/yy") & " - " & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm '-' d/MM/yy") & "  " & nomeEvento.Substring(2)
                    End If


                    LKB_temp.Font.Name = "tahoma"
                    LKB_temp.Font.Size = System.Web.UI.WebControls.FontUnit.Parse("13px")
                Else

                    'troncamento nome evento 

                    If nomeEvento.Length > CInt(84 / numGiorni) Then
                        nomeEvento = nomeEvento.Substring(0, CInt(84 / numGiorni)) & "..."
                    End If

                    LKB_temp.Text = "&nbsp;" & nomeEvento
                    LKB_temp.Font.Name = "tahoma"
                    LKB_temp.Font.Size = System.Web.UI.WebControls.FontUnit.Parse("10px")
                End If

                'tooltip con ora e if sulla data inizio e fine evento

                toolTipPulsante = "<font style=" & ControlChars.Quote & "FONT-SIZE: 11px; COLOR: white; FONT-FAMILY:  Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & "><b>" & nomeEvento & "</b></font></font></td></tr><tr><td align=left><font style=" & ControlChars.Quote & "FONT-SIZE: 10px; COLOR: black; FONT-FAMILY:  Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & ">" & oResource.getValue("LBLdettEventoInizio.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm") & "</b><br>" & oResource.getValue("LBLdettEventoFine.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm") & "</b>"


                If CDate(RowEventiGiorno(i).Item("ORRI_fine")).Date > fine.Date Then
                    toolTipPulsante = "<font style=" & ControlChars.Quote & "FONT-SIZE: 11px; COLOR: white; TEXT-ALIGN: center; FONT-FAMILY: Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & "><b>" & nomeEvento & "</b></font></font></td></tr><tr><td align=left><font style=" & ControlChars.Quote & "FONT-SIZE: 10px; COLOR: black; TEXT-ALIGN: left; FONT-FAMILY:  Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & ">" & oResource.getValue("LBLdettEventoInizio.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm") & "</b><br>" & oResource.getValue("LBLdettEventoFine.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm d/MM/yy") & "</b>"
                End If

                If CDate(RowEventiGiorno(i).Item("ORRI_inizio")).Date < inizio.Date Then
                    toolTipPulsante = "<font style=" & ControlChars.Quote & "FONT-SIZE: 11px; COLOR: white; TEXT-ALIGN: center; FONT-FAMILY: Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & "><b>" & nomeEvento.Substring(2) & "</b></font></font></td></tr><tr><td align=left><font style=" & ControlChars.Quote & "FONT-SIZE: 10px; COLOR: black; TEXT-ALIGN: left; FONT-FAMILY:  Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & ">" & oResource.getValue("LBLdettEventoInizio.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm d/MM/yy") & "</b><br>" & oResource.getValue("LBLdettEventoFine.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm") & "</b>"
                End If

                If CDate(RowEventiGiorno(i).Item("ORRI_inizio")).Date < inizio.Date And CDate(RowEventiGiorno(i).Item("ORRI_fine")).Date > fine.Date Then
                    toolTipPulsante = "<font style=" & ControlChars.Quote & "FONT-SIZE: 11px; COLOR: white; TEXT-ALIGN: center; FONT-FAMILY: Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & "><b>" & nomeEvento.Substring(2) & "</b></font></font></td></tr><tr><td align=left><font style=" & ControlChars.Quote & "FONT-SIZE: 10px; COLOR: black; TEXT-ALIGN: left; FONT-FAMILY:  Tahoma, 'Times New Roman', Arial, 'Lucida Sans' " & ControlChars.Quote & ">" & oResource.getValue("LBLdettEventoInizio.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString("HH:mm d/MM/yy") & "</b><br>" & oResource.getValue("LBLdettEventoFine.text") & ":  <b>" & CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString("HH:mm d/MM/yy") & "</b>"
                End If

                Dim Luogo, Aula As String
                If Not IsDBNull(RowEventiGiorno(0).Item("ORRI_aula")) Then
                    Aula = RowEventiGiorno(0).Item("ORRI_aula")
                End If
                If Not IsDBNull(RowEventiGiorno(0).Item("EVNT_luogo")) Then
                    Luogo = RowEventiGiorno(0).Item("EVNT_luogo")
                End If
                If Luogo = "" And Aula = "" Then

                ElseIf Luogo = "" Then
                    Luogo = Aula
                ElseIf Aula = "" Then

                ElseIf Aula <> Luogo Then
                    Luogo = Aula & " - " & Luogo
                End If
                toolTipPulsante = toolTipPulsante & "<br><b>" & Luogo & "</b>"

                Try


                    If RowEventiGiorno(i).Item("CMNT_id").ToString.CompareTo(CStr(Session("idComunita"))) = 0 Then
                        LKB_temp.Font.Bold = True   'nome evento in grasseto se evento della comunità corrente
                    End If



                    If RowEventiGiorno(i).Item("TPEV_id") = -1 Then
                        'colore lkb evento personale
                        LKB_temp.ForeColor = System.Drawing.Color.Black 'settaggio colore eventi personali
                        LKB_temp.Font.Bold = False

                        If RowEventiGiorno(i).Item("ORRI_visibile") = False Then   ' reminder sospeso....
                            LKB_temp.ForeColor = System.Drawing.Color.Red   'settaggio colore reminder sospesi
                            LKB_temp.Text = LKB_temp.Text & " (!)"
                            toolTipPulsante = toolTipPulsante & "<br><b>-- " & oResource.getValue("evento.sospeso").ToUpper & "--</b>"
                        End If

                        LKB_temp.Attributes.Add("onclick", "window.status='" & RowEventiGiorno(i).Item("TPEV_nome").ToString.Replace("'", "\'") & " ' ;return true;")
                        LKB_temp.Attributes.Add("onfocus", "window.status='" & RowEventiGiorno(i).Item("TPEV_nome").ToString.Replace("'", "\'") & " ';return true;")
                    Else
                        LKB_temp.Attributes.Add("onclick", "window.status='" & RowEventiGiorno(i).Item("TPEV_nome").ToString.Replace("'", "\'") & " --> " & oResource.getValue("calendarioSettimanale.comunita").Replace("'", "\'") & ": " & RowEventiGiorno(i).Item("CMNT_nome").ToString.Replace("'", "\'") & " <--' ;return true;")
                        toolTipPulsante = toolTipPulsante & "<br>" & oResource.getValue("calendarioSettimanale.comunita") & ": <br><b>" & RowEventiGiorno(i).Item("CMNT_nome").ToString & "</b>"
                        'LKB_temp.Attributes.Add("onmouseover", "window.status='" & RowEventiGiorno(i).Item("TPEV_nome").ToString & " --> " & oResource.getValue("calendarioSettimanale.comunita") & ": " & RowEventiGiorno(i).Item("CMNT_nome").ToString & " <--' ;return true;")
                        LKB_temp.Attributes.Add("onfocus", "window.status='" & RowEventiGiorno(i).Item("TPEV_nome").ToString.Replace("'", "\'") & " --> " & oResource.getValue("calendarioSettimanale.comunita").Replace("'", "\'") & ": " & RowEventiGiorno(i).Item("CMNT_nome").ToString.Replace("'", "\'") & " <--' ;return true;")
                        'LKB_temp.Attributes.Add("onmouseout", "window.status='';return true;")

                        If RowEventiGiorno(i).Item("ORRI_visibile") = False Then
                            LKB_temp.ForeColor = System.Drawing.Color.LightGray   'settaggio colore eventi nascosti
                            toolTipPulsante = toolTipPulsante & "<br>" & oResource.getValue("evento.nascosto").Replace("'", "\'")
                        Else
                            LKB_temp.ForeColor = System.Drawing.Color.DarkCyan    'settaggio colore standard
                        End If
                    End If

                    LKB_temp.Attributes.Add("onMouseOver", "toolTip('" & toolTipPulsante.Replace("'", "\'") & "'); window.status='" & RowEventiGiorno(i).Item("TPEV_nome").ToString & " '; return true;")
                    LKB_temp.Attributes.Add("onMouseOut", "toolTip(); window.status=''; return true;")


                Catch ex As Exception

                End Try

                LKB_temp.CommandArgument = CDate(RowEventiGiorno(i).Item("ORRI_fine")).ToString
                LKB_temp.CommandName = CDate(RowEventiGiorno(i).Item("ORRI_inizio")).ToString

				'Dim oLinkEvento As LinkButton
				'oLinkEvento = Me.FindControl("LNBevent_" & RowEventiGiorno(i).Item("ORRI_id").ToString)
				'If isnothing( Then

				'End If
				LKB_temp.ID = "LNBevent_" & RowEventiGiorno(i).Item("ORRI_id").ToString


				If i <= (RowEventiGiorno.Length - 1) Then
					LKB_temp.Text = LKB_temp.Text & "<BR>"
				End If


				Try
					If CInt(Session("ORRI_id")) = RowEventiGiorno(i).Item("ORRI_id").ToString Then
						'controllo se è stato selezionato questo evento e setto il colore del nome e della cella colore
						LKB_temp.ForeColor = System.Drawing.Color.Navy
						LKB_temp.Font.Bold = True
						cellaC.BgColor = "#000099"
					End If
				Catch ex As Exception

				End Try



				If CBEventiTutti.Checked Then
					cellaG.Controls.Add(LKB_temp)  'aggiunta del LKB dell'evento se l'evento è da visualizzare (filtro per tipo)
					cellaC.BgColor = ColoreEvento	'assegnazione colore alla cella
				Else
					For Each LIST As ListItem In CBXLFiltroEventi.Items
						If RowEventiGiorno(i).Item("TPEV_id") = LIST.Value And LIST.Selected Then
							cellaG.Controls.Add(LKB_temp)  'aggiunta del LKB dell'evento se l'evento è da visualizzare (filtro per tipo)
							cellaC.BgColor = ColoreEvento	'assegnazione colore alla cella
							Exit For
						End If
					Next
				End If

			Next

            Try

                If ora.Substring(3, 2).CompareTo("00") = 0 Then
                    inizio = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "00:00")
                    fine = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "29:59")
                    StringaFiltro = "(EVNT_macro = 0 ) AND ((ORRI_inizio >= '" & inizio.ToString & "') AND (ORRI_inizio <= '" & fine.ToString & "')"
                    StringaFiltro = StringaFiltro & " or  (ORRI_fine < '" & fine.ToString & "' and ORRI_fine > '" & inizio.ToString & "')"
                    StringaFiltro = StringaFiltro & " or  (ORRI_fine > '" & fine.ToString & "' and ORRI_inizio < '" & inizio.ToString & "'))"

                Else
                    inizio = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "30:00")
                    fine = CDate(Trim(giorno) & " " & ora.Substring(0, 3).ToString & "59:59")
                    StringaFiltro = "(EVNT_macro = 0 ) AND ((ORRI_inizio >= '" & inizio.ToString & "') AND (ORRI_inizio <= '" & fine.ToString & "')"
                    StringaFiltro = StringaFiltro & " or  (ORRI_fine < '" & fine.ToString & "' and ORRI_fine > '" & inizio.ToString & "')"
                    StringaFiltro = StringaFiltro & " or  (ORRI_fine > '" & fine.ToString & "' and ORRI_inizio < '" & inizio.ToString & "'))"
                End If

                RowEventiGiorno = oDataset.Tables(0).Select(StringaFiltro)

                If RowEventiGiorno.Length > 0 Then
                    cellaG.BgColor = "#eaffdc"
                End If
            Catch ex As Exception

            End Try
            If (CDate(Session("LBVisibileInizio")) >= inizio And CDate(Session("LBVisibileInizio")) <= fine) Or (CDate(Session("LBVisibileFine")) > inizio And CDate(Session("LBVisibileInizio")) <= inizio) Or (CDate(Session("LBVisibileFine")) > inizio And CDate(Session("LBVisibileFine")) < fine) Then
                cellaG.BgColor = "#ffffff"
                cellaC.BgColor = "#000099"
            End If

        Catch ex As Exception
			'Dim oErrore As New COL_DataLayer.MailDBerrori
			'oErrore.Oggetto = "Eventi"
			'oErrore.Body = ex.Message
			'oErrore.InviaMail()
        End Try

    End Sub

#End Region

#Region "pulsanti selezione giorno"

    Private Sub setupPulsanti()
        Dim i As Integer
        Dim dsPuls As DataSet = New DataSet
        Dim tabPuls As DataTable = New System.Data.DataTable
        Dim colPuls As DataColumn = New System.Data.DataColumn
        Dim colDataPuls As DataColumn = New System.Data.DataColumn
        Dim rowPuls As DataRow
        Dim giorni As String
        giorni = Session("giorniVisual")
        Try
            tabPuls.TableName = "tabellaPulsanti"
            colPuls.DataType = System.Type.GetType("System.String")
            colPuls.ColumnName = "IDPulsanti"
            tabPuls.Columns.Add(colPuls)
            colDataPuls.DataType = System.Type.GetType("System.String")
            colDataPuls.ColumnName = "TextPulsanti"
            tabPuls.Columns.Add(colDataPuls)
            Dim dataInizio As Date
            dataInizio = CType(Session("dtInizioSett"), Date)
            rowPuls = tabPuls.NewRow()
            rowPuls.Item("IDPulsanti") = "BTN_0"
            rowPuls.Item("TextPulsanti") = ""
            tabPuls.Rows.Add(rowPuls)
            For i = 1 To 7
                If giorni.IndexOf(CDate(Session("dtInizioSett")).AddDays(i - 1).DayOfWeek) > -1 Then
                    rowPuls = tabPuls.NewRow()
                    rowPuls.Item("IDPulsanti") = dataInizio.AddDays(i - 1).ToString
                    rowPuls.Item("TextPulsanti") = dataInizio.AddDays(i - 1).ToString("ddd d MMM yy", oResource.CultureInfo.DateTimeFormat)
                    tabPuls.Rows.Add(rowPuls)
                End If
            Next
            dsPuls.Tables.Add(tabPuls)
            RPTpulsanti.DataSource = dsPuls.Tables("tabellaPulsanti")
            RPTpulsanti.DataBind()
        Catch ex As Exception

        End Try
    End Sub



    Private Sub RPTpulsanti_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpulsanti.ItemCreated
        Dim BTNtp As System.Web.UI.WebControls.Button
        Try
            BTNtp = CType(e.Item.FindControl("BTNtemp"), System.Web.UI.WebControls.Button)
            BTNtp.ID = e.Item.DataItem("IDPulsanti")
            BTNtp.Text = e.Item.DataItem("TextPulsanti")

            If e.Item.ItemIndex = 0 Then
                BTNtp.Visible = False
                BTNtp.Width = Unit.Pixel(30)
            Else
                If PNLcalendarioGiornaliero.Visible = True Then
                    BTNtp.Width = Unit.Pixel(600)
                Else
                    Dim numGiorni As Integer
                    For Each LIST As ListItem In CBXLfiltroGiorni.Items
                        If LIST.Selected = True Then
                            numGiorni += 1
                        End If
                    Next
                    BTNtp.Width = Unit.Pixel(CInt(630 / numGiorni))
                End If
            End If

            If CType(BTNtp.Text, Date).Date = Now().Date Then
                BTNtp.BorderColor = System.Drawing.Color.Red
                BTNtp.BorderStyle = BorderStyle.Ridge
            ElseIf CType(BTNtp.Text, Date).DayOfWeek = DayOfWeek.Sunday Then
                BTNtp.BackColor = System.Drawing.Color.LightSalmon
                BTNtp.ForeColor = System.Drawing.Color.Red
            End If


        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Eventi filtri"

    Private Sub BTNfiltraGiorni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNfiltraGiorni.Click
        If PNLfiltroGiorni.Visible = True Then
            PNLfiltroGiorni.Visible = False
        Else
            PNLfiltroGiorni.Visible = True
        End If
    End Sub

    Private Sub DDLfiltraOre_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltraOre.SelectedIndexChanged
        Session("oraFine") = DDLfiltraOre.SelectedValue.ToString.Substring(DDLfiltraOre.SelectedValue.ToString.IndexOf("_") + 1)
        Session("oraInizio") = DDLfiltraOre.SelectedValue.ToString.Substring(0, DDLfiltraOre.SelectedValue.ToString.Length - CType(Session("oraFine"), String).Length - 1)
        setupSettimana()
    End Sub


    Private Sub BTNApplicaFiltroG_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNApplicaFiltroG.Click
        Session("giorniVisual") = ""
        Dim numGiorni As Integer
        For Each LIST As ListItem In CBXLfiltroGiorni.Items
            If LIST.Selected = True Then
                Session("giorniVisual") = CStr(Session("giorniVisual")) & LIST.Value
                numGiorni += 1
            Else
                Session("giorniVisual") = CStr(Session("giorniVisual")) & "8"
            End If
        Next
        If numGiorni = 0 Then
            Session("giorniVisual") = "1888888"
            CBXLfiltroGiorni.Items(0).Selected = True
        End If
        PNLfiltroGiorni.Visible = False
        setupSettimana()
    End Sub

    Private Sub LKBbackTOall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBbackTOall.Click
        PNLcalendarioGiornaliero.Visible = False
        BTNfiltraGiorni.Visible = True
        Session("giorniVisual") = "1234560"
        For Each LIST As ListItem In CBXLfiltroGiorni.Items
            LIST.Selected = True
        Next
        If CDate(Session("dtInizioSett")).DayOfWeek = 0 Then
            Session("dtInizioSett") = CDate(Session("dtInizioSett")).AddDays(-6)
        Else
            Session("dtInizioSett") = CDate(Session("dtInizioSett")).AddDays(1 - CDate(Session("dtInizioSett")).DayOfWeek)
        End If
        CLNmini.SelectedDate = Nothing
        setupSettimana()
    End Sub
    Private Sub BTNApplicaFiltroEventi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNApplicaFiltroEventi.Click
        Dim i As Integer
        For Each LIST As ListItem In CBXLFiltroEventi.Items
            If LIST.Selected Then
                i += 1
            End If
        Next

        If i = 0 Then
            CBEventiTutti.Checked = True
            CBXLFiltroEventi.Visible = False
            BTNApplicaFiltroEventi.Visible = False
            For Each LIST As ListItem In CBXLFiltroEventi.Items
                LIST.Selected = True
            Next
        End If
        Session("ORRI_id") = Nothing
        Session("LBVisibileInizio") = Nothing
        Session("LBVisibileFine") = Nothing
        PNLdettaglioEvento.Visible = False
        TBCalMini.Visible = True
        CBXLFiltroEventi.Visible = False
        BTNApplicaFiltroEventi.Visible = False
        setupSettimana()
    End Sub

    Private Sub CBEventiTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBEventiTutti.CheckedChanged
        If CBEventiTutti.Checked Then
            CBXLFiltroEventi.Visible = False
            BTNApplicaFiltroEventi.Visible = False

        Else
            CBXLFiltroEventi.Visible = True
            BTNApplicaFiltroEventi.Visible = True
        End If

        For Each LIST As ListItem In CBXLFiltroEventi.Items
            LIST.Selected = True
        Next

        Session("ORRI_id") = Nothing
        Session("LBVisibileInizio") = Nothing
        Session("LBVisibileFine") = Nothing
        PNLdettaglioEvento.Visible = False
        TBCalMini.Visible = True
        setupSettimana()

    End Sub

    Private Sub BTNfiltroEventi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNfiltroEventi.Click
        If CBXLFiltroEventi.Visible = False Then
            CBXLFiltroEventi.Visible = True
            BTNApplicaFiltroEventi.Visible = True
            CBEventiTutti.Checked = False
        Else
            CBXLFiltroEventi.Visible = False
            BTNApplicaFiltroEventi.Visible = False
        End If
    End Sub

    Private Sub RBLFiltroComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLFiltroComunita.SelectedIndexChanged
        Try
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
        Catch ex As Exception

        End Try

        Session("ORRI_id") = Nothing
        Session("LBVisibileInizio") = Nothing
        Session("LBVisibileFine") = Nothing
        PNLdettaglioEvento.Visible = False
        TBCalMini.Visible = True
        setupSettimana()
    End Sub

    Public Sub ZoomGiorno(ByVal sender As Object, ByVal e As EventArgs)
        Dim BTNGiorno, BTNback As Button
        BTNGiorno = CType(sender, Button)
        Session("giorniVisual") = ""
        Dim i As Integer
        For i = 0 To 6
            If CType(BTNGiorno.ID, Date).DayOfWeek = i Then
                Session("giorniVisual") = Session("giorniVisual") & i
            Else
                Session("giorniVisual") = Session("giorniVisual") & "8"
            End If
        Next
        If CDate(BTNGiorno.ID).DayOfWeek = 0 Then
            Session("dtInizioSett") = CDate(BTNGiorno.ID).AddDays(-6)
        Else
            Session("dtInizioSett") = CDate(BTNGiorno.ID).AddDays(1 - CDate(BTNGiorno.ID).DayOfWeek)
        End If
        BTNfiltraGiorni.Visible = False
        PNLcalendarioGiornaliero.Visible = True
        CLNmini.SelectedDate = CType(BTNGiorno.ID, Date)
        setupSettimana()
        BTNGiorno.Width = Unit.Percentage(500)
        CLNmini.SelectedDate = CType(BTNGiorno.ID, Date)
        CLNmini.VisibleDate = CType(BTNGiorno.ID, Date)
    End Sub

    Private Sub LKBtuttiIgiorni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBtuttiIgiorni.Click
        For Each LIST As ListItem In CBXLfiltroGiorni.Items
            LIST.Selected = True
        Next
        Session("giorniVisual") = "1234560"
        PNLfiltroGiorni.Visible = False
        setupSettimana()
    End Sub

    Private Sub LKBgoTOannuale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgoTOannuale.Click
        Session("AnnoAttuale") = CDate(Session("dtInizioSett")).Year
        Try
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
        Catch ex As Exception

        End Try
        Response.Redirect(".\CalendarioAnnuale.aspx")
    End Sub

    Private Sub LKBgoTOmensile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgoTOmensile.Click
        Session("meseAttuale") = CDate("01/" & CDate(Session("dtInizioSett")).Month & "/" & CDate(Session("dtInizioSett")).Year)
        Try
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
        Catch ex As Exception

		End Try
		Response.Redirect(".\CalendarioMensile.aspx")
    End Sub

    Private Sub LKBsettPrec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBsettPrec.Click
        Session("dtInizioSett") = CDate(Session("dtInizioSett")).AddDays(-7)
        CLNmini.VisibleDate = CDate(Session("dtInizioSett"))
        CLNmini.SelectedDate = Nothing
        setupSettimana()
    End Sub

    Private Sub LKBsettSucc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBsettSucc.Click
        Session("dtInizioSett") = CDate(Session("dtInizioSett")).AddDays(7)
        CLNmini.VisibleDate = CDate(Session("dtInizioSett"))
        CLNmini.SelectedDate = Nothing
        setupSettimana()
    End Sub

#End Region

#Region "dettaglio evento"

    Public Sub DettaglioEvento(ByVal sender As Object, ByVal e As EventArgs)
		Dim oLinkButton As LinkButton
		Dim OrarioID As Long
		Try
			Dim Name As String = ""
			oLinkButton = CType(sender, LinkButton)
			Name = Replace(oLinkButton.ID, "LNBevent_", "")
			OrarioID = CLng(Name)
			If Session("ORRI_id") = OrarioID Then
				Session("ORRI_id") = Nothing
				Session("LBVisibileInizio") = Nothing
				Session("LBVisibileFine") = Nothing
				PNLdettaglioEvento.Visible = False
				oResource.setLinkButton(LKBmodifica, True, True, True)
				oResource.setLinkButton(LKBelimina, True, True, True)
				LKBmodifica.Enabled = True
				LKBelimina.Enabled = True
				TBCalMini.Visible = True
				LKBvediCALmini.Visible = False
			Else
				Session("ORRI_id") = OrarioID
				Session("LBVisibileInizio") = CDate(oLinkButton.CommandName())
				Session("LBVisibileFine") = CDate(oLinkButton.CommandArgument())
				setupDettaglioEvento()
				oResource.setLinkButton(LKBmodifica, True, True)
				oResource.setLinkButton(LKBelimina, True, True)
			End If
			TBLElimina.Visible = False
			LBLMessaggio.Visible = False
			setupSettimana()
		Catch ex As Exception

		End Try
    End Sub

    Private Sub setupDettaglioEvento()
        Dim RowDettEvento() As DataRow
        Try

            RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
            If RowDettEvento.Length = 1 Then

                If RowDettEvento(0).Item("EVNT_nome").ToString.Length > 24 Then
                    LBNomeEvento.Text = RowDettEvento(0).Item("EVNT_nome").ToString.Substring(0, 24) & "..."
                Else
                    LBNomeEvento.Text = RowDettEvento(0).Item("EVNT_nome").ToString
                End If

                If RowDettEvento(0).Item("CMNT_nome").ToString.Length > 24 Then
                    LBComunita.Text = RowDettEvento(0).Item("CMNT_nome").ToString.Substring(0, 24) & "..."
                Else
                    LBComunita.Text = RowDettEvento(0).Item("CMNT_nome").ToString
                End If

                If RowDettEvento(0).Item("TPEV_nome").ToString.Length > 24 Then
                    LBTipo.Text = RowDettEvento(0).Item("TPEV_nome").ToString.Substring(0, 24) & "..."
                Else
                    LBTipo.Text = RowDettEvento(0).Item("TPEV_nome").ToString
                End If

                If RowDettEvento(0).Item("EVNT_nome").ToString.Length > 24 Then
                    LBNomeEvento.Text = RowDettEvento(0).Item("EVNT_nome").ToString.Substring(0, 24) & "..."
                Else
                    LBNomeEvento.Text = RowDettEvento(0).Item("EVNT_nome").ToString
                End If

                If CDate(RowDettEvento(0).Item("ORRI_inizio")).Date < CDate(RowDettEvento(0).Item("dataRender")).Date Then
                    LBInizio.Text = CDate(RowDettEvento(0).Item("ORRI_inizio")).ToString("HH:mm") & " " & oResource.getValue("calendarioSettimanale.del") & " " & CDate(RowDettEvento(0).Item("ORRI_inizio")).ToString("dd-MM-yy")
                Else
                    LBInizio.Text = CDate(RowDettEvento(0).Item("ORRI_inizio")).ToString("HH:mm")
                End If

                If CDate(RowDettEvento(0).Item("ORRI_inizio")).Date < CDate(RowDettEvento(0).Item("ORRI_fine")).Date Then
                    LBfine.Text = CDate(RowDettEvento(0).Item("ORRI_fine")).ToString("HH:mm") & " " & oResource.getValue("calendarioSettimanale.del") & " " & CDate(RowDettEvento(0).Item("ORRI_fine")).ToString("dd-MM-yy")
                Else
                    LBfine.Text = CDate(RowDettEvento(0).Item("ORRI_fine")).ToString("HH:mm")
                End If


                Try
                    If RowDettEvento(0).Item("ORRI_link").ToString.Substring(0, 7).CompareTo("http://") <> 0 Then
                        HLLink.NavigateUrl = "http://" & RowDettEvento(0).Item("ORRI_link").ToString
                    Else
                        HLLink.NavigateUrl = RowDettEvento(0).Item("ORRI_link").ToString
                    End If
                Catch ex As Exception

                End Try


                If RowDettEvento(0).Item("ORRI_link").ToString.Length > 24 Then
                    HLLink.Text = RowDettEvento(0).Item("ORRI_link").ToString.Substring(0, 24) & "..."
                Else
                    HLLink.Text = RowDettEvento(0).Item("ORRI_link").ToString
                End If


                If RowDettEvento(0).Item("EVNT_luogo").ToString.Length > 11 Then
                    LBLuogo.Text = RowDettEvento(0).Item("EVNT_luogo").ToString.Substring(0, 11) & "..."
                Else
                    LBLuogo.Text = RowDettEvento(0).Item("EVNT_luogo").ToString
                End If

                If RowDettEvento(0).Item("ORRI_aula").ToString.Length > 11 Then
                    LBaula.Text = RowDettEvento(0).Item("ORRI_aula").ToString.Substring(0, 11) & "..."
                Else
                    LBaula.Text = RowDettEvento(0).Item("ORRI_aula").ToString
                End If
                If LBaula.Text = LBLuogo.Text Then
                    LBaula.Text = ""
                ElseIf LBaula.Text <> "" And LBLuogo.Text <> "" Then
                    LBaula.Text = " - " & LBaula.Text
                End If
                PNLdettaglioEvento.Visible = True
                Dim quote As String
                quote = """"
                If CInt(RowDettEvento(0).Item("ORRI_id")) > 0 Then
                    ' evento
                    LKBcreaReminder.Visible = True
                    Dim i_link As String

                    i_link = "./DettaglioEvento.aspx?ORRI_id=" & RowDettEvento(0).Item("ORRI_id").ToString & "&tipo=" & RowDettEvento(0).Item("TPEV_id").ToString

                    If Request.Browser.Browser = "IE" And Request.Browser.MajorVersion >= 5 Then
                        LKBzoom.Attributes.Add("onclick", "window.showModalDialog(" & quote & i_link & quote & ",null," & quote & "dialogWidth:530px;dialogHeight:600px;center:1;scroll:1;help:0;status:0" & quote & ");return false;")
                    Else
                        LKBzoom.Attributes.Add("onclick", "OpenWin('" & i_link & "','530','600','yes','yes');return false;")
                    End If
                Else
                    ' evento personale
                    Dim i_link As String
                    LKBcreaReminder.Visible = False
                    i_link = "./DettaglioEvento.aspx?ORRI_id=" & CStr(CInt(RowDettEvento(0).Item("ORRI_id")) * -1) & "&tipo=" & RowDettEvento(0).Item("TPEV_id").ToString
                    If Request.Browser.Browser = "IE" And Request.Browser.MajorVersion >= 5 Then
                        LKBzoom.Attributes.Add("onclick", "window.showModalDialog(" & quote & i_link & quote & ",null," & quote & "dialogWidth:530px;dialogHeight:450px;center:1;scroll:1;help:0;status:0" & quote & ");return false;")
                    Else
                        LKBzoom.Attributes.Add("onclick", "OpenWin('" & i_link & "','530','450','yes','yes');return false;")
                    End If

                End If

                Dim PermessiAssociati As String
                Dim oServizio As New Services_Eventi

                Try
                    PermessiAssociati = RowDettEvento(0).Item("LKSC_Permessi").ToString
                    If Not (PermessiAssociati = "") Then
                        oServizio.PermessiAssociati = PermessiAssociati
                    Else
                        oServizio.PermessiAssociati = "00000000000000000000000000000000"
                    End If

                    oResource.setLinkButton(LKBmodifica, True, True)
                    oResource.setLinkButton(LKBelimina, True, True)
                    LKBmodifica.Enabled = False
                    LKBelimina.Enabled = False
                    LKBmodifica.ToolTip = oResource.getValue("info.3")
                    LKBelimina.ToolTip = oResource.getValue("info.4")
                    Dim ricorrenze As Integer
                    If CInt(Session("ORRI_id")) > 0 Then
                        Dim oEvento As New COL_Evento
                        ricorrenze = oEvento.getNUMricorrenze(CInt(Session("ORRI_id")))
                    End If


                    If CInt(Session("ORRI_id")) < 0 Or ricorrenze = 1 Then
                        If oServizio.AdminService Then
                            LKBmodifica.Enabled = True
                            LKBelimina.Enabled = True
                            oResource.setLinkButton(LKBmodifica, True, True)
                            oResource.setLinkButton(LKBelimina, True, True, False, True)
                        End If

                        If oServizio.DelEvents Then
                            LKBelimina.Enabled = True
                            oResource.setLinkButton(LKBelimina, True, True, False, True)
                        End If

                        If oServizio.ChangeEvents Then
                            LKBmodifica.Enabled = True
                            oResource.setLinkButton(LKBmodifica, True, True)
                        End If

                        If CInt(RowDettEvento(0).Item("ORRI_id")) < 0 Then
                            LKBmodifica.Enabled = True
                            LKBelimina.Enabled = True
                            oResource.setLinkButton(LKBmodifica, True, True)
                            oResource.setLinkButton(LKBelimina, True, True, False, True)
                        End If

                    Else

                        If oServizio.AdminService Then
                            LKBmodifica.Enabled = True
                            LKBelimina.Enabled = True
                            oResource.setLinkButton(LKBmodifica, True, True)
                            oResource.setLinkButton(LKBelimina, True, True)
                        End If

                        If oServizio.DelEvents Then
                            LKBelimina.Enabled = True
                            oResource.setLinkButton(LKBelimina, True, True)
                        End If

                        If oServizio.ChangeEvents Then
                            LKBmodifica.Enabled = True
                            oResource.setLinkButton(LKBmodifica, True, True)
                        End If

                    End If


                Catch ex As Exception

                End Try

                LKBmodifica.Visible = True
                LKBelimina.Visible = True
                TBLElimina.Visible = False
                LBLMessaggio.Visible = False
                TBCalMini.Visible = False
                LKBvediCALmini.Visible = True
            Else
                    'Session("ORRI_id") = Nothing
                    'Session("LBVisibileInizio") = Nothing
                    'Session("LBVisibileFine") = Nothing
                    PNLdettaglioEvento.Visible = False
                    oResource.setLinkButton(LKBmodifica, True, True, True)
                    oResource.setLinkButton(LKBelimina, True, True, True)
                    TBCalMini.Visible = True
                    LKBvediCALmini.Visible = False
                End If
        Catch ex As Exception
            Session("ORRI_id") = Nothing
            Session("LBVisibileInizio") = Nothing
            Session("LBVisibileFine") = Nothing
            PNLdettaglioEvento.Visible = False
            oResource.setLinkButton(LKBmodifica, True, True, True)
            oResource.setLinkButton(LKBelimina, True, True, True)
            TBCalMini.Visible = True
            LKBvediCALmini.Visible = False
        End Try

    End Sub

    Private Sub LKBchiudiDettaglio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBchiudiDettaglio.Click

        Session("ORRI_id") = Nothing
        Session("LBVisibileInizio") = Nothing
        Session("LBVisibileFine") = Nothing

        TBCalMini.Visible = True
        PNLdettaglioEvento.Visible = False
        LKBmodifica.Enabled = True
        LKBelimina.Enabled = True
        LKBvediCALmini.Visible = False
        LBLMessaggio.Visible = False
        oResource.setLinkButton(LKBmodifica, True, True, True)
        oResource.setLinkButton(LKBelimina, True, True, True)
        TBLElimina.Visible = False
        setupSettimana()

    End Sub

    Private Sub LKBtrova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBtrova.Click
        Try
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
        Catch ex As Exception

        End Try
        Response.Redirect("RicercaEvento.aspx?reset=true")
    End Sub

    Private Sub LKBmodifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmodifica.Click
        BTNModifica.Visible = True
        BTNElimina.Visible = False
        TBLElimina.Visible = True
        Try
            Dim RowDettEvento() As DataRow
            oResource.setRadioButtonList(RBLEliminaEventi, "0")
            oResource.setRadioButtonList(RBLEliminaEventi, "1")
            If CInt(Session("ORRI_id")) < 0 Then
                '  REMINDER
                RBLEliminaEventi.Visible = False
                RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
                Session("Comando") = "modifica"
                Session("ReminderID") = CInt(RowDettEvento(0).Item("ORRI_id").ToString) * -1
                Response.Redirect("modificaevento.aspx")
            Else
                '  EVENTO/I
                Dim oEvento As New COL_Evento
                Dim ricorrenze As Integer
                ricorrenze = oEvento.getNUMricorrenze(CInt(Session("ORRI_id")))
                If ricorrenze = 1 Then
                    RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
                    Session("Comando") = "modifica"
                    Session("OrarioID") = RowDettEvento(0).Item("ORRI_id").ToString
                    Session("EventoID") = RowDettEvento(0).Item("EVNT_id").ToString
                    Session("Evento_CMNT_id") = RowDettEvento(0).Item("CMNT_id").ToString
                    Response.Redirect("modificaevento.aspx")
                Else
                    RBLEliminaEventi.Items(1).Text = RBLEliminaEventi.Items(1).Text & " (" & ricorrenze.ToString & ")"
                    RBLEliminaEventi.Visible = True
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNModifica.Click
        Dim RowDettEvento() As DataRow
        Try
            '  EVENTO
            If RBLEliminaEventi.SelectedValue = 0 Then
                'modifica solo evento selezionato
                RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
                Session("Comando") = "modifica"
                Session("OrarioID") = RowDettEvento(0).Item("ORRI_id").ToString
                Session("EventoID") = RowDettEvento(0).Item("EVNT_id").ToString
                Session("Evento_CMNT_id") = RowDettEvento(0).Item("CMNT_id").ToString
            ElseIf RBLEliminaEventi.SelectedValue = 1 Then
                'modifica di tutte le ricorrenze dell'evento selezionato   
                RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
                Session("Comando") = "modificaTUTTE"
                Session("OrarioID") = RowDettEvento(0).Item("ORRI_id").ToString
                Session("EventoID") = RowDettEvento(0).Item("EVNT_id").ToString
                Session("Evento_CMNT_id") = RowDettEvento(0).Item("CMNT_id").ToString
            End If
            RBLEliminaEventi.SelectedValue = 0
            Response.Redirect("modificaevento.aspx")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub LKBelimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBelimina.Click
        BTNModifica.Visible = False
        BTNElimina.Visible = True
        TBLElimina.Visible = True
        Dim RowDettEvento() As DataRow
        Try
            oResource.setRadioButtonList(RBLEliminaEventi, "0")
            oResource.setRadioButtonList(RBLEliminaEventi, "1")

            Dim ricorrenze As Integer
            If CInt(Session("ORRI_id")) > 0 Then
                Dim oEvento As New COL_Evento
                ricorrenze = oEvento.getNUMricorrenze(CInt(Session("ORRI_id")))
            End If

            If CInt(Session("ORRI_id")) < 0 Then
                RBLEliminaEventi.Visible = False
                RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
                '  sto eliminando un REMINDER
                Dim oReminder As New COL_Reminder
                oReminder.Id = CInt(RowDettEvento(0).Item("ORRI_id").ToString) * -1
                oReminder.Cancella()
                oResource.setLabel_To_Value(LBLMessaggio, "info.1")
                LBLMessaggio.Visible = True
                Session("ORRI_id") = Nothing
                Session("LBVisibileInizio") = Nothing
                Session("LBVisibileFine") = Nothing
                TBCalMini.Visible = True
                PNLdettaglioEvento.Visible = False
                LKBvediCALmini.Visible = False
                oResource.setLinkButton(LKBmodifica, True, True, True)
                oResource.setLinkButton(LKBelimina, True, True, True)
                TBLElimina.Visible = False
                setupSettimana()
            Else
                If ricorrenze = 1 Then
                    RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
                    Dim oEvento As New COL_Evento
                    Dim oProgramma As New COL_Programma_Evento
                    oEvento.Id = RowDettEvento(0).Item("EVNT_id").ToString
                    oEvento.Cancella()
                    oResource.setLabel_To_Value(LBLMessaggio, "info.1")
                    LBLMessaggio.Visible = True
                    Session("ORRI_id") = Nothing
                    Session("LBVisibileInizio") = Nothing
                    Session("LBVisibileFine") = Nothing
                    TBCalMini.Visible = True
                    PNLdettaglioEvento.Visible = False
                    LKBvediCALmini.Visible = False
                    oResource.setLinkButton(LKBmodifica, True, True, True)
                    oResource.setLinkButton(LKBelimina, True, True, True)
                    TBLElimina.Visible = False
                    setupSettimana()
                Else
                    RBLEliminaEventi.Items(1).Text = RBLEliminaEventi.Items(1).Text & " (" & ricorrenze.ToString & ")"
                    RBLEliminaEventi.Visible = True
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub BTNElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNElimina.Click
        Dim RowDettEvento() As DataRow
        Try
            RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
            'elimino uno o piu eventi
            If RBLEliminaEventi.SelectedValue = 0 Then
                'eliminazione solo evento selezionato
                Dim oOrario As New COL_Orario
                Dim oProgramma As New COL_Programma_Evento
                oOrario.Id = RowDettEvento(0).Item("ORRI_id").ToString
                oOrario.Cancella()
                oProgramma.Id = RowDettEvento(0).Item("ORRI_id").ToString
                oProgramma.Cancella()
                oResource.setLabel_To_Value(LBLMessaggio, "info.1")
                LBLMessaggio.Visible = True
            ElseIf RBLEliminaEventi.SelectedValue = 1 Then
                'eliminazione di tutte le ricorrenze dell'evento selezionato
                Dim oEvento As New COL_Evento
                Dim oProgramma As New COL_Programma_Evento
                oEvento.Id = RowDettEvento(0).Item("EVNT_id").ToString
                oEvento.Cancella()
                oResource.setLabel_To_Value(LBLMessaggio, "info.2")
                LBLMessaggio.Visible = True
            End If
            RBLEliminaEventi.SelectedValue = 0

            Session("ORRI_id") = Nothing
            Session("LBVisibileInizio") = Nothing
            Session("LBVisibileFine") = Nothing

            TBCalMini.Visible = True
            PNLdettaglioEvento.Visible = False

            LKBvediCALmini.Visible = False
            oResource.setLinkButton(LKBmodifica, True, True, True)
            oResource.setLinkButton(LKBelimina, True, True, True)
            TBLElimina.Visible = False

        Catch ex As Exception

        End Try
        setupSettimana()
    End Sub

    Private Sub BTNAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNAnnulla.Click
        TBLElimina.Visible = False
    End Sub

    Private Sub LKBcreaReminder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBcreaReminder.Click
        Dim oReminder As New COL_Reminder
        DDLTipoAvviso.Items.Clear()
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
            DDLTipoAvviso.Items.Add(New ListItem(oResource.getValue("CBXLFiltroEventi.-2"), 4))
            DDLTipoAvviso.Enabled = False
            DDLTipoAvviso.SelectedValue = "4"
        End Try
        BTNCreaReminder.Visible = True
        BTNAnnullaCreazRem.Visible = True
        DDLTipoAvviso.Visible = True
        LBAvviso.Visible = True
    End Sub

    Private Sub BTNCreaReminder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCreaReminder.Click
        Try
            Dim RowDettEvento() As DataRow
            RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")

            Try
                Dim oReminder As New COL_Reminder
                With oReminder
                    .Inizio = RowDettEvento(0).Item("ORRI_inizio")
                    .Fine = RowDettEvento(0).Item("ORRI_fine")
                    .idPersona = Session("objPersona").id
                    .idTipoAvviso = DDLTipoAvviso.SelectedValue
                    .Link = RowDettEvento(0).Item("ORRI_link")

                    Dim Luogo, Aula As String
                    If Not IsDBNull(RowDettEvento(0).Item("ORRI_aula")) Then
                        Aula = RowDettEvento(0).Item("ORRI_aula")
                    End If
                    If Not IsDBNull(RowDettEvento(0).Item("EVNT_luogo")) Then
                        Luogo = RowDettEvento(0).Item("EVNT_luogo")
                    End If
                    If Luogo = "" And Aula = "" Then

                    ElseIf Luogo = "" Then
                        Luogo = Aula
                    ElseIf Aula = "" Then

                    ElseIf Aula <> Luogo Then
                        Luogo = Aula & " - " & Luogo
                    End If
                    .Luogo = Luogo
                    .Oggetto = RowDettEvento(0).Item("EVNT_nome")
                    .Sospeso = False
                    .Testo = "- " & RowDettEvento(0).Item("CMNT_nome") & " - " & RowDettEvento(0).Item("TPEV_nome") & " - " & RowDettEvento(0).Item("EVNT_note")
                    .Creazione = Date.Today
                End With
                oReminder.Aggiungi(CInt(RowDettEvento(0).Item("ORRI_id")), CInt(RowDettEvento(0).Item("EVNT_id")), 1)
                oResource.setLabel_To_Value(LBLMessaggio, "info.5")
                LBLMessaggio.Visible = True
            Catch ex As Exception
                oResource.setLabel_To_Value(LBLMessaggio, "info.6")
                LBLMessaggio.Visible = True
            End Try

            BTNCreaReminder.Visible = False
            BTNAnnullaCreazRem.Visible = False
            DDLTipoAvviso.Visible = False
            LBAvviso.Visible = False

            Session("ORRI_id") = Nothing
            Session("LBVisibileInizio") = Nothing
            Session("LBVisibileFine") = Nothing
            TBCalMini.Visible = True
            PNLdettaglioEvento.Visible = False
            LKBvediCALmini.Visible = False
            oResource.setLinkButton(LKBmodifica, True, True, True)
            oResource.setLinkButton(LKBelimina, True, True, True)
            LKBmodifica.Enabled = True
            LKBelimina.Enabled = True
            setupSettimana()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub BTNAnnullaCreazRem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNAnnullaCreazRem.Click
        BTNCreaReminder.Visible = False
        BTNAnnullaCreazRem.Visible = False
        DDLTipoAvviso.Visible = False
        LBAvviso.Visible = False
    End Sub

#End Region

#Region "Eventi calendario mini dx"

    Private Sub LKBvediCALmini_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBvediCALmini.Click
        setupSettimana()
        TBCalMini.Visible = True
        LKBvediCALmini.Visible = False
    End Sub

    Private Sub CLNmini_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CLNmini.SelectionChanged
        If CLNmini.SelectedDates.Count > 1 Then
            Session("dtInizioSett") = CLNmini.SelectedDates.Item(0)
            If PNLcalendarioGiornaliero.Visible = True Then
                Session("giorniVisual") = "1234560"
            End If
            PNLcalendarioGiornaliero.Visible = False
            BTNfiltraGiorni.Visible = True
        Else
            Dim i As Integer
            Session("giorniVisual") = ""
            For i = 0 To 6
                If CLNmini.SelectedDate.DayOfWeek = i Then
                    Session("giorniVisual") = Session("giorniVisual") & i
                Else
                    Session("giorniVisual") = Session("giorniVisual") & "8"
                End If
            Next
            If CLNmini.SelectedDate.DayOfWeek = 0 Then
                Session("dtInizioSett") = CLNmini.SelectedDate.AddDays(-6)
            Else
                Session("dtInizioSett") = CLNmini.SelectedDate.AddDays(1 - CLNmini.SelectedDate.DayOfWeek)
            End If
            BTNfiltraGiorni.Visible = False
            PNLcalendarioGiornaliero.Visible = True
        End If
        setupSettimana()
    End Sub

    Private Sub BTNVaiA_oggi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNVaiA_oggi.Click
        CLNmini.VisibleDate = Now
        DDLVaiA_mesi.SelectedValue = Now.Month
        DDLVaiA_anni.SelectedValue = Now.Year
    End Sub

    Private Sub DDLVaiA_anni_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLVaiA_anni.SelectedIndexChanged
        CLNmini.VisibleDate = CDate("01/" & DDLVaiA_mesi.SelectedValue & "/" & DDLVaiA_anni.SelectedValue)
    End Sub

    Private Sub DDLVaiA_mesi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLVaiA_mesi.SelectedIndexChanged
        CLNmini.VisibleDate = CDate("01/" & DDLVaiA_mesi.SelectedValue & "/" & DDLVaiA_anni.SelectedValue)
    End Sub

    Private Sub CLNmini_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles CLNmini.DayRender
        If e.Day.Date >= CDate(Session("dtInizioSett")).Date And e.Day.Date <= CDate(Session("dtInizioSett")).Date.AddDays(6) And Not e.Day.IsToday And e.Day.IsSelected = False And PNLcalendarioGiornaliero.Visible = False Then
            e.Cell.BackColor = System.Drawing.Color.White
            e.Cell.BorderColor = System.Drawing.Color.CornflowerBlue
            e.Cell.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
        End If

    End Sub
#End Region

#Region "inserisci evento"

    Private Sub LKBInserisciEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBInserisciEvento.Click
        PNLcalendarioGiornaliero.Visible = False
        BTNfiltraGiorni.Visible = True
        Session("giorniVisual") = "1234560"
        Session("Azione") = "Insert"
        Response.Redirect("inserimentoevento.aspx")
    End Sub

    Private Sub DBClickCellaGiorno(ByVal sender As Object, ByVal e As EventArgs)
        Dim cellaG As System.Web.UI.HtmlControls.HtmlTableCell
        cellaG = CType(sender, System.Web.UI.HtmlControls.HtmlTableCell)
        Dim ora, giorno As String
        ora = cellaG.ID.Substring(cellaG.ID.IndexOf("_") + 1)
        giorno = cellaG.ID.Substring(0, cellaG.ID.Length - ora.Length - 1)
        Session("Azione") = "Insert"
        Response.Redirect("inserimentoevento.aspx?giorno=" & giorno & ";ora=" & ora)
    End Sub

#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_CalendarioSettimanale"
        oResource.Folder_Level1 = "Eventi"
        oResource.setCulture()
    End Sub

    Private Sub Setup_Internazionalizzazione()
        With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(LBGestEVNT)
            .setLinkButton(LKBInserisciEvento, True, True)
            .setLinkButton(LKBelimina, True, True)
            .setLinkButton(LKBmodifica, True, True)
            .setLinkButton(LKBtrova, True, True)
            .setButton(BTNElimina, True, , True, True)
            .setButton(BTNAnnulla, True)
            .setButton(BTNModifica, True, , , True)
            .setLinkButton(LKBsettPrec, True, True)
            .setLinkButton(LKBsettSucc, True, True)
            .setLabel(LBLFiltri)
            .setLabel(LBLfiltroORE)
            .setButton(BTNfiltraGiorni, True, , , True)
            .setLinkButton(LKBtuttiIgiorni, True, True)
            .setButton(BTNApplicaFiltroG, True, , , True)
            .setLabel(LBLfiltroEventi)
            .setCheckBox(CBEventiTutti)
            .setButton(BTNfiltroEventi, True, , , True)
            .setButton(BTNApplicaFiltroEventi, True, , , True)
            .setLabel(LBFiltroComunita)
            .setLabel(LBLCLNminiMESE)
            .setLabel(LBLCLNminiANNO)
            .setLabel(LBLvaiA)
            .setButton(BTNVaiA_oggi, True, , , True)
            .setLinkButton(LKBvediCALmini, True, True)
            .setLabel(LBLdettEventoTitolo)
            .setLabel(LBLdettEventoNome)
            .setLabel(LBLdettEventoCNMT)
            .setLabel(LBLdettEventoTipo)
            .setLabel(LBLdettEventoInizio)
            .setLabel(LBLdettEventoFine)
            .setLabel(LBLdettEventoLink)
            .setLabel(LBLdettEventoLuogo)
            .setLinkButton(LKBchiudiDettaglio, True, True)
            .setLinkButton(LKBzoom, True, True)
            .setLabel(LBLVisualizzazione)
            .setLinkButton(LKBgoTOmensile, True, True)
            .setLinkButton(LKBgoTOannuale, True, True)
            .setLinkButton(LKBbackTOall, True, True)
            .setLinkButton(LKBexport, True, True)
            .setLinkButton(LKBIcalendar, True, True)
            .setLinkButton(LKBcreaReminder, True, True)
            .setLabel(LBAvviso)
            .setButton(BTNCreaReminder, True, False, True, True)
            .setButton(BTNAnnullaCreazRem, True, , , True)
            CLNmini.PrevMonthText = .getValue("CLNmini.mesePrecedente")
            CLNmini.NextMonthText = .getValue("CLNmini.meseSuccessivo")
        End With
        setupFiltri()
    End Sub

    Private Sub SetCookies(ByVal LinguaID As Integer, ByVal LinguaCode As String)
        Dim oBrowser As System.Web.HttpBrowserCapabilities
        oBrowser = Request.Browser
        Try
            If oBrowser.Cookies Then
                Dim oCookie_ID As New System.Web.HttpCookie("LinguaID", LinguaID.ToString)
                Dim oCookie_Code As New System.Web.HttpCookie("LinguaCode", LinguaCode)
                oCookie_ID.Expires = Now.AddYears(1)
                oCookie_Code.Expires = Now.AddYears(1)
                Response.Cookies.Add(oCookie_ID)
                Response.Cookies.Add(oCookie_Code)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "EXPORT DATA"

    Private Sub LKBIcalendar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBIcalendar.Click
        CreaIcalendar()
    End Sub

    Public Sub CreaIcalendar()

        Dim RowDettEvento() As DataRow
        Try

            RowDettEvento = oDataset.Tables(0).Select("(ORRI_id = " & Session("ORRI_id") & ")")
            If RowDettEvento.Length = 1 Then
                Dim result As New System.Text.StringBuilder
                result.Remove(0, result.Length)
                result.AppendFormat("BEGIN:VCALENDAR{0}", System.Environment.NewLine)
                result.AppendFormat("VERSION:2.0{0}", System.Environment.NewLine)
                result.AppendFormat("METHOD:PUBLISH{0}", System.Environment.NewLine)
                result.AppendFormat("BEGIN:VEVENT{0}", System.Environment.NewLine)
                result.AppendFormat("UID:{0}{1}", "105487345" & RowDettEvento(0).Item("ORRI_id").ToString & "9877" & RowDettEvento(0).Item("EVNT_id").ToString & "745ComOL", System.Environment.NewLine)
                result.AppendFormat("SUMMARY:{0}{1}", RowDettEvento(0).Item("EVNT_nome").ToString, System.Environment.NewLine)
                result.AppendFormat("ORGANIZER:{0}{1}", RowDettEvento(0).Item("CMNT_nome").ToString & " -- " & RowDettEvento(0).Item("Creatore").ToString, System.Environment.NewLine)

                Dim Luogo, Aula As String
                If Not IsDBNull(RowDettEvento(0).Item("ORRI_aula")) Then
                    Aula = RowDettEvento(0).Item("ORRI_aula")
                End If
                If Not IsDBNull(RowDettEvento(0).Item("EVNT_luogo")) Then
                    Luogo = RowDettEvento(0).Item("EVNT_luogo")
                End If
                If Luogo = "" And Aula = "" Then

                ElseIf Luogo = "" Then
                    Luogo = Aula
                ElseIf Aula = "" Then

                ElseIf Aula <> Luogo Then
                    Luogo = Aula & " - " & Luogo
                End If
                result.AppendFormat("LOCATION:{0}{1}", Luogo, System.Environment.NewLine)
                result.AppendFormat("DTSTART:{0}{1}", CDate(RowDettEvento(0).Item("ORRI_inizio").ToString).ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z"), System.Environment.NewLine)
                result.AppendFormat("DTEND:{0}{1}", CDate(RowDettEvento(0).Item("ORRI_fine").ToString).ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z"), System.Environment.NewLine)
                result.AppendFormat("DTSTAMP:{0}{1}", Now.ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z"), System.Environment.NewLine)
                result.AppendFormat("DESCRIPTION:{0}{1}", RowDettEvento(0).Item("EVNT_note").ToString.Replace(ControlChars.CrLf, "\n"), System.Environment.NewLine)

                If RowDettEvento(0).Item("ORRI_link").ToString.Length > 0 Then
                    result.AppendFormat("URL:{0}{1}", RowDettEvento(0).Item("ORRI_link").ToString, System.Environment.NewLine)
                End If
                Dim Trigger As TimeSpan

                result.AppendFormat("BEGIN:VALARM{0}", System.Environment.NewLine)
                result.AppendFormat("TRIGGER:{0}{1}", "-PT15M", System.Environment.NewLine)
                result.AppendFormat("ACTION:{0}{1}", "DISPLAY", System.Environment.NewLine)
                result.AppendFormat("DESCRIPTION:{0}{1}", "Reminder", System.Environment.NewLine)
                result.AppendFormat("END:VALARM{0}", System.Environment.NewLine)
                result.AppendFormat("END:VEVENT{0}", System.Environment.NewLine)
                result.AppendFormat("END:VCALENDAR{0}", System.Environment.NewLine)
                Dim FileName As String = "iCalendar.ics"
                Page.Response.Clear()
                Page.Response.ClearContent()
                Page.Response.ClearHeaders()
                Page.Response.Write(result)
                Dim OpenType As String = "attachment"
                Page.Response.ContentType = "text/plain"
                Page.Response.AddHeader("Content-Disposition", OpenType + ";filename=" + FileName)
                Page.Response.End()
            End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub LKBexport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBexport.Click
        Try
            Dim oRow_1 As New TableRow
            Dim oRow_2 As New TableRow
            Dim oCell_1 As New TableCell
            Dim oCell_2 As New TableCell
            Dim i, j As Integer
            Dim giorni As String
            giorni = Session("giorniVisual")

            oCell_1.BackColor = System.Drawing.Color.White
            oCell_1.Width = Unit.Pixel(60)
            oRow_1.Cells.Add(oCell_1)


            For j = 1 To 7
                If giorni.IndexOf(CDate(Session("dtInizioSett")).AddDays(j - 1).DayOfWeek) > -1 Then
                    oCell_1 = New TableCell
                    oCell_1.BackColor = System.Drawing.Color.White
                    oCell_1.Width = Unit.Pixel(5)
                    oRow_1.Cells.Add(oCell_1)

                    oCell_1 = New TableCell
                    oCell_1.BackColor = System.Drawing.Color.LightYellow
                    oCell_1.Width = Unit.Pixel(120)
                    oCell_1.Height = Unit.Pixel(25)
                    oCell_1.HorizontalAlign = HorizontalAlign.Center
                    oCell_1.VerticalAlign = VerticalAlign.Middle
                    oCell_1.ForeColor = System.Drawing.Color.DarkBlue
                    oCell_1.BorderColor = System.Drawing.Color.White

                    oCell_1.Text = CDate(Session("dtInizioSett")).AddDays(j - 1).ToString("ddd d MMM yy", oResource.CultureInfo.DateTimeFormat)


                    If CDate(Session("dtInizioSett")).AddDays(j - 1).DayOfWeek = DayOfWeek.Sunday Then
                        oCell_1.ForeColor = System.Drawing.Color.Red
                        oCell_1.BorderColor = System.Drawing.Color.Red
                    End If
                    oCell_1.Font.Bold = True
                    oCell_1.Font.Name = "Tahoma"
                    oCell_1.Font.Size = FontUnit.Point(8)
                    oRow_1.Cells.Add(oCell_1)
                End If
            Next

            TBLExport.Rows.Add(oRow_1)
            oRow_2.Height = Unit.Pixel(5)
            TBLExport.Rows.Add(oRow_2)

            For i = CInt(Session("oraInizio")) To CInt(Session("oraFine")) - 1

                oRow_1 = New TableRow
                oRow_2 = New TableRow

                oCell_1 = New TableCell
                oCell_1.BackColor = System.Drawing.Color.LightYellow
                oCell_1.Text = i.ToString("00") & " : 00"
                oCell_1.Width = Unit.Pixel(60)

                oCell_1.Font.Name = "Tahoma"
                oCell_1.Font.Size = FontUnit.Point(10)
                oCell_1.Font.Bold = True
                oCell_1.BorderColor = System.Drawing.Color.White
                oCell_1.HorizontalAlign = HorizontalAlign.Center
                oCell_1.VerticalAlign = VerticalAlign.Top
                oRow_1.Cells.Add(oCell_1)

                oCell_2 = New TableCell
                oCell_2.BackColor = System.Drawing.Color.LightYellow
                oCell_2.Width = Unit.Pixel(60)

                oCell_2.Text = i.ToString("00") & " : 30"
                oCell_2.Font.Name = "Tahoma"
                oCell_2.Font.Size = FontUnit.Point(10)
                oCell_2.HorizontalAlign = HorizontalAlign.Center
                oCell_2.VerticalAlign = VerticalAlign.Top
                oCell_2.BorderColor = System.Drawing.Color.White

                oCell_2.Font.Bold = True
                oRow_2.Cells.Add(oCell_2)

                For j = 1 To 7
                    If giorni.IndexOf(CDate(Session("dtInizioSett")).AddDays(j - 1).DayOfWeek) > -1 Then
                        oCell_1 = New TableCell
                        oCell_1.BackColor = System.Drawing.Color.White
                        oCell_1.Width = Unit.Pixel(5)

                        oRow_1.Cells.Add(oCell_1)

                        oCell_1 = New TableCell
                        oCell_1.BackColor = System.Drawing.Color.LightYellow

                        Dim inizio, fine As Date
                        Dim stringafiltro As String
                        Dim RowEventiGiorno() As DataRow
                        Dim k As Integer
                        inizio = CDate(CDate(Session("dtInizioSett")).AddDays(j - 1).ToString("dd/MM/yyyy") & " " & i & ":00:00")
                        fine = CDate(CDate(Session("dtInizioSett")).AddDays(j - 1).ToString("dd/MM/yyyy") & " " & i & ":29:59")
                        stringafiltro = "(dataRender >= '" & inizio.ToString & "') AND (dataRender < '" & fine.ToString & "')"
                        RowEventiGiorno = oDataset.Tables(0).Select(stringafiltro)
                        For k = 0 To RowEventiGiorno.Length - 1
                            Dim nomeEvento As String
                            If RowEventiGiorno(k).Item("ORRI_visibile") = False Then
                                'reminder sospeso
                                nomeEvento = "<font style=" & ControlChars.Quote & " FONT-SIZE: 7pt;" & ControlChars.Quote & "> " & CDate(RowEventiGiorno(k).Item("ORRI_inizio")).ToString(" dd-MM  HH:mm    ") & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & CDate(RowEventiGiorno(k).Item("ORRI_fine")).ToString(" dd-MM  HH:mm    ") & " &nbsp;&nbsp;</font><font style=" & ControlChars.Quote & "FONT-SIZE: 9pt; FONT-WEIGHT: bold; COLOR: red;  " & ControlChars.Quote & "> " & RowEventiGiorno(k).Item("EVNT_nome").ToString & " (!) " & oResource.getValue("evento.sospeso") & " </font>"
                            Else
                                nomeEvento = "<font style=" & ControlChars.Quote & " FONT-SIZE: 7pt;" & ControlChars.Quote & "> " & CDate(RowEventiGiorno(k).Item("ORRI_inizio")).ToString(" dd-MM  HH:mm    ") & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & CDate(RowEventiGiorno(k).Item("ORRI_fine")).ToString(" dd-MM  HH:mm    ") & " &nbsp;&nbsp;</font><font style=" & ControlChars.Quote & "FONT-SIZE: 9pt; FONT-WEIGHT: bold;" & ControlChars.Quote & "> " & RowEventiGiorno(k).Item("EVNT_nome").ToString & " </font>"
                            End If
                            If RowEventiGiorno(k).Item("ORRI_inizio") <> RowEventiGiorno(k).Item("dataRender") Then
                                nomeEvento = "^" & nomeEvento
                            End If

                            oCell_1.Text = oCell_1.Text & nomeEvento & "<BR>"
                        Next
                        If RowEventiGiorno.Length > 0 Then
                            oCell_1.Height = Unit.Pixel(34 * RowEventiGiorno.Length)
                        Else
                            oCell_1.Height = Unit.Pixel(34)
                        End If

                        stringafiltro = "(EVNT_macro = 0 ) AND ( (ORRI_inizio >= '" & inizio.ToString & "') AND (ORRI_inizio <= '" & fine.ToString & "')"
                        stringafiltro = stringafiltro & " or  (ORRI_fine < '" & fine.ToString & "' and ORRI_fine > '" & inizio.ToString & "')"
                        stringafiltro = stringafiltro & " or  (ORRI_fine > '" & fine.ToString & "' and ORRI_inizio < '" & inizio.ToString & "'))"
                        RowEventiGiorno = oDataset.Tables(0).Select(stringafiltro)

                        If RowEventiGiorno.Length > 0 Then
                            oCell_1.BackColor = Color.PaleGoldenrod
                        End If
                        oCell_1.Width = Unit.Pixel(120)

                        oCell_1.BorderColor = System.Drawing.Color.LightYellow
                        oCell_1.BorderWidth = Unit.Point(1)
                        oCell_1.HorizontalAlign = HorizontalAlign.Center
                        oCell_1.VerticalAlign = VerticalAlign.Top
                        oRow_1.Cells.Add(oCell_1)

                        oCell_2 = New TableCell
                        oCell_2.BackColor = System.Drawing.Color.White
                        oCell_2.Width = Unit.Pixel(5)

                        oRow_2.Cells.Add(oCell_2)

                        oCell_2 = New TableCell
                        oCell_2.BackColor = System.Drawing.Color.LightYellow
                        oCell_2.Width = Unit.Pixel(120)
                        oCell_2.Height = Unit.Pixel(34)
                        oCell_2.BorderColor = System.Drawing.Color.LightYellow
                        oCell_2.BorderWidth = Unit.Point(1)
                        oCell_2.HorizontalAlign = HorizontalAlign.Center
                        oCell_2.VerticalAlign = VerticalAlign.Top
                        inizio = CDate(CDate(Session("dtInizioSett")).AddDays(j - 1).ToString("dd/MM/yyyy") & " " & i & ":30:00")
                        fine = CDate(CDate(Session("dtInizioSett")).AddDays(j - 1).ToString("dd/MM/yyyy") & " " & i & ":59:59")
                        stringafiltro = "(dataRender >= '" & inizio.ToString & "') AND (dataRender < '" & fine.ToString & "')"
                        RowEventiGiorno = oDataset.Tables(0).Select(stringafiltro)
                        For k = 0 To RowEventiGiorno.Length - 1
                            Dim nomeEvento As String
                            If RowEventiGiorno(k).Item("ORRI_visibile") = False Then
                                'reminder sospeso
                                nomeEvento = "<font style=" & ControlChars.Quote & " FONT-SIZE: 7pt;" & ControlChars.Quote & "> " & CDate(RowEventiGiorno(k).Item("ORRI_inizio")).ToString(" dd-MM  HH:mm    ") & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & CDate(RowEventiGiorno(k).Item("ORRI_fine")).ToString(" dd-MM  HH:mm    ") & " &nbsp;&nbsp;</font><font style=" & ControlChars.Quote & "FONT-SIZE: 9pt; FONT-WEIGHT: bold; COLOR: red;  " & ControlChars.Quote & "> " & RowEventiGiorno(k).Item("EVNT_nome").ToString & " (!) " & oResource.getValue("evento.sospeso") & " </font>"
                            Else
                                nomeEvento = "<font style=" & ControlChars.Quote & " FONT-SIZE: 7pt;" & ControlChars.Quote & "> " & CDate(RowEventiGiorno(k).Item("ORRI_inizio")).ToString(" dd-MM  HH:mm    ") & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & CDate(RowEventiGiorno(k).Item("ORRI_fine")).ToString(" dd-MM  HH:mm    ") & " &nbsp;&nbsp;</font><font style=" & ControlChars.Quote & "FONT-SIZE: 9pt; FONT-WEIGHT: bold;" & ControlChars.Quote & "> " & RowEventiGiorno(k).Item("EVNT_nome").ToString & " </font>"
                            End If
                            If RowEventiGiorno(k).Item("ORRI_inizio") <> RowEventiGiorno(k).Item("dataRender") Then
                                nomeEvento = "^" & nomeEvento
                            End If

                            oCell_2.Text = oCell_2.Text & nomeEvento & "<BR>"
                        Next
                        If RowEventiGiorno.Length > 0 Then
                            oCell_2.Height = Unit.Pixel(34 * RowEventiGiorno.Length)
                        Else
                            oCell_2.Height = Unit.Pixel(34)
                        End If
                        stringafiltro = "(EVNT_macro = 0 ) AND ((ORRI_inizio >= '" & inizio.ToString & "') AND (ORRI_inizio <= '" & fine.ToString & "')"
                        stringafiltro = stringafiltro & " or  (ORRI_fine < '" & fine.ToString & "' and ORRI_fine > '" & inizio.ToString & "')"
                        stringafiltro = stringafiltro & " or  (ORRI_fine > '" & fine.ToString & "' and ORRI_inizio < '" & inizio.ToString & "'))"
                        RowEventiGiorno = oDataset.Tables(0).Select(stringafiltro)
                        If RowEventiGiorno.Length > 0 Then
                            oCell_2.BackColor = Color.PaleGoldenrod
                        End If
                        oRow_2.Cells.Add(oCell_2)
                    End If
                Next
                TBLExport.Rows.Add(oRow_1)
                TBLExport.Rows.Add(oRow_2)
            Next

            Dim FileName As String = "agenda_" & CDate(Session("dtInizioSett")).ToString("dd_MM_yy") & "__" & CDate(Session("dtInizioSett")).AddDays(6).ToString("dd_MM_yy") & ".xls"
            Dim oStringWriter As System.IO.StringWriter = New System.IO.StringWriter
            Dim oHTMLWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
            Response.Buffer = True
            TBLExport.Visible = True
            Page.Response.Clear()
            Page.Response.ClearContent()
            Page.Response.ClearHeaders()
            TBLExport.RenderControl(oHTMLWriter)
			Page.Response.Write("<html><head></head><body>")
            Page.Response.Write(oStringWriter)
            Page.Response.Write("</body></html>")
            Dim OpenType As String = "attachment"
            Page.Response.ContentType = "application/ms-excel"
            Page.Response.AddHeader("Content-Disposition", OpenType + ";filename=" + FileName)
            Page.Response.End()
        Catch ex As Exception
            TBLExport.Visible = False
        End Try

    End Sub
#End Region

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

End Class