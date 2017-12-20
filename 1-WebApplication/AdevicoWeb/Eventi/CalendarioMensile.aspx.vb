Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.UCServices

Imports COL_BusinessLogic_v2

Public Class CalendarioMensile
    Inherits System.Web.UI.Page
 Protected oResource As ResourceManager

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

#Region " calendario "

    Protected WithEvents RPTgiorniSettimana As System.Web.UI.WebControls.Repeater
    Protected WithEvents RPTgiorni As System.Web.UI.WebControls.Repeater
    Protected WithEvents RPTsettimane As System.Web.UI.WebControls.Repeater
    Protected WithEvents LKBmesePrec As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBmeseSuc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBLMeseCorrente As System.Web.UI.WebControls.Label
    Protected WithEvents LKBSettimana As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DDLVaiA_mesi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLVaiA_anni As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNVai As System.Web.UI.WebControls.Button
    Protected WithEvents LKBgoTOsettimanale As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBgoTOannuale As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBFiltroComunita As System.Web.UI.WebControls.Label
    Protected WithEvents RBLFiltroComunita As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents CBEventiTutti As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNApplicaFiltroEventi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNfiltroEventi As System.Web.UI.WebControls.Button
    Protected WithEvents CBXLFiltroEventi As System.Web.UI.WebControls.CheckBoxList
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBLVisualizzazione As System.Web.UI.WebControls.Label
    Protected WithEvents LBLcambiaMese As System.Web.UI.WebControls.Label
    Protected WithEvents LBLEventiVisual As System.Web.UI.WebControls.Label

#End Region

#Region " Permessi "

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBnascondi As System.Web.UI.WebControls.LinkButton

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

    Protected oDataset As DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Try
            Dim oServizio As New Services_Eventi

            Try
                oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                If oServizio.PermessiAssociati = "" Then
                    oServizio.PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                oServizio.PermessiAssociati = "00000000000000000000000000000000"
            End Try
            If Not Page.IsPostBack Then
                Me.Setup_Internazionalizzazione()
                Me.Bind_Dati()
            End If

            If oServizio.ChangeEvents Or oServizio.DelEvents Or oServizio.ReadEvents Or oServizio.AdminService Or oServizio.AddEvents Then
                PNLcontenuto.Visible = True
                PNLpermessi.Visible = False
            Else
                'PNLpermessi.Visible = True
                'LBnopermessi.Visible = True
                'LBnopermessi.Text = "NON SEI ENTRATO IN NESSUNA COMUNITA' <br> PUOI SOLAMENTE GESTIRE I TUOI EVENTI PERSONALI"
            End If
            CreaCalendario()
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
    Private Sub Bind_Dati()
        If Session("meseAttuale") = Nothing Then
            Session("meseAttuale") = Now.AddDays(1 - Now.Day)
        End If

        Bind_CambioAnno()
        Bind_FiltroEventi()

        Try
            If Request.Cookies("CalendarioSett")("RBLComunita") = "-1" Or Request.Cookies("CalendarioSett")("RBLComunita") = "0" Then
                RBLFiltroComunita.SelectedValue = Request.Cookies("CalendarioSett")("RBLComunita")
            Else
                RBLFiltroComunita.SelectedValue = "0"
            End If
        Catch ex As Exception
            RBLFiltroComunita.SelectedValue = "0"
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
            'bind DDL cambio anno sul calendario di dx
            Dim annoMinMax(1) As Integer
            Dim OOrario As New COL_Orario
            Dim AnnoMin, AnnoMax As Int16
            annoMinMax = OOrario.getAnni_minMAX   'caricamento anni min-max da eventi presenti nel DB
            AnnoMin = annoMinMax(0) - 2
            AnnoMax = annoMinMax(1) + 2
            DDLVaiA_anni.Items.Clear()
            For i = 0 To (AnnoMax - AnnoMin)
                DDLVaiA_anni.Items.Add(New ListItem(CStr(AnnoMin + i), CInt(AnnoMin + i)))
            Next
            DDLVaiA_mesi.SelectedValue = Now.Month
            DDLVaiA_anni.SelectedValue = Now.Year
        Catch ex As Exception
            DDLVaiA_anni.Items.Add(New ListItem(CStr(Now.Year.ToString("YYYY")), CInt(Now.Year)))
        End Try
    End Sub
    Private Sub RetrieveEventsFromDB()

        Dim dataInizio As Date
        dataInizio = CDate(Session("meseAttuale"))
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

            oReminder.idPersona = oPersona.Id

            Dim tipoEvento As String
            tipoEvento = ","
            For Each LIST As ListItem In CBXLFiltroEventi.Items
                If LIST.Selected Then
                    tipoEvento = tipoEvento & LIST.Value & ","
                End If
            Next
            If CBXLFiltroEventi.Items.FindByValue("-1").Selected Or CBEventiTutti.Checked Then
				oDatasetReminder = oReminder.Estrai(dataInizio, dataInizio.AddMonths(1).AddDays(-1), 0)
            End If


            'caricamento eventi associati alla persona o della comunità corrente o di tutte le comunita (IDcmt=-1)
			oDataset = oEvento.TrovaEventiSettimanaliPersona(oPersona.Id, DateToString(dataInizio, False), DateToString(dataInizio.AddMonths(1).AddDays(-1), False), oServizio.Codex, IDcmt, tipoEvento, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
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
                        oRow = oDataset.Tables(0).NewRow
                    End Try

                    oRow.Item("TPEV_id") = -1
                    oRow.Item("ORRI_id") = CInt(oDatasetReminder.Tables(0).Rows(i).Item("RMND_id")) * -1
                    oRow.Item("EVNT_nome") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_oggetto")
                    oRow.Item("ORRI_inizio") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")
                    oRow.Item("ORRI_fine") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_fine")
                    oRow.Item("TPEV_icon") = "#fffee7"
                    oRow.Item("CMNT_id") = "0"
                    oRow.Item("CMNT_nome") = "n/a"
                    oRow.Item("TPEV_nome") = Me.oResource.getValue("TipoPersonale")
                    oRow.Item("ORRI_link") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_link")
                    oRow.Item("EVNT_luogo") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_luogo")
                    oRow.Item("ORRI_aula") = "n/a"
                    oRow.Item("ORRI_visibile") = 1
                    oRow.Item("dataRender") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")
                    oDataset.Tables(0).Rows.Add(oRow)
                Next
            End If
            oDataset.Dispose()
            oDatasetReminder.Dispose()
        Catch ex As Exception
        End Try

    End Sub

#End Region

#Region " bind repeater calendario"

    Private Sub CreaCalendario()
        Me.RetrieveEventsFromDB()
        SetupGiorniSettimana()
        Dim i As Integer
        Dim datainizio, datatemp As Date
        Dim dsSettimane As DataSet = New DataSet
        Dim tabSettimane As DataTable = New System.Data.DataTable
        Dim colSettimana As DataColumn = New System.Data.DataColumn
        Dim colPrimoGiorno As DataColumn = New System.Data.DataColumn
        Dim rowSettimana As DataRow
        Try
            datainizio = CDate(Session("meseAttuale"))
            LBLMeseCorrente.Text = datainizio.ToString("MMMM yyyy", oResource.CultureInfo.DateTimeFormat)
            LKBmesePrec.Text = datainizio.AddMonths(-1).ToString("MMMM yyyy", oResource.CultureInfo.DateTimeFormat)
            LKBmeseSuc.Text = datainizio.AddMonths(1).ToString("MMMM yyyy", oResource.CultureInfo.DateTimeFormat)
            If datainizio.DayOfWeek = 0 Then
                datatemp = datainizio.AddDays(-6)
            Else
                datatemp = datainizio.AddDays(1 - datainizio.DayOfWeek)
            End If
            tabSettimane.TableName = "tabellaSettimane"
            colSettimana.DataType = System.Type.GetType("System.Int32")
            colSettimana.ColumnName = "Settimana"
            tabSettimane.Columns.Add(colSettimana)
            colPrimoGiorno.DataType = System.Type.GetType("System.String")
            colPrimoGiorno.ColumnName = "PrimoGiorno"
            tabSettimane.Columns.Add(colPrimoGiorno)
            For i = 1 To 6
                rowSettimana = tabSettimane.NewRow()
                rowSettimana.Item("Settimana") = i
                rowSettimana.Item("PrimoGiorno") = datatemp
                tabSettimane.Rows.Add(rowSettimana)
                datatemp = datatemp.AddDays(7)
                If datatemp.Month = datainizio.AddMonths(1).Month Then
                    i = 7
                End If
            Next
            dsSettimane.Tables.Add(tabSettimane)
            RPTsettimane.DataSource = dsSettimane.Tables("tabellaSettimane")
            RPTsettimane.DataBind()
        Catch ex As Exception

        End Try

        Dim TestoLink As String = ""
        TestoLink = Me.oResource.getValue("LINKmese")
        If TestoLink <> "" Then
            Dim TestoLinkS, TestoLinkP As String
            TestoLinkS = TestoLink
            TestoLinkP = TestoLink
            TestoLinkP = Replace(TestoLinkP, "#mese#", Me.LKBmesePrec.Text)
            TestoLinkS = Replace(TestoLinkS, "#mese#", Me.LKBmeseSuc.Text)
            Me.LKBmeseSuc.Attributes.Add("onmouseover", "window.status='" & Replace(TestoLinkS, "'", "\'") & "';return true;")
            Me.LKBmeseSuc.Attributes.Add("onfocusr", "window.status='" & Replace(TestoLinkS, "'", "\'") & "';return true;")
            Me.LKBmeseSuc.Attributes.Add("onmouseout", "window.status='';return true;")
            Me.LKBmeseSuc.ToolTip = TestoLinkS

            Me.LKBmesePrec.Attributes.Add("onmouseover", "window.status='" & Replace(TestoLinkP, "'", "\'") & "';return true;")
            Me.LKBmesePrec.Attributes.Add("onfocusr", "window.status='" & Replace(TestoLinkP, "'", "\'") & "';return true;")
            Me.LKBmesePrec.Attributes.Add("onmouseout", "window.status='';return true;")
            Me.LKBmesePrec.ToolTip = TestoLinkP
        End If
    End Sub

    Private Sub RPTsettimane_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsettimane.ItemCreated
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                setupGiorni(CType(e.Item.FindControl("RPTgiorni"), Repeater), e.Item.ItemIndex + 1)
                Dim cellaSelect As System.Web.UI.HtmlControls.HtmlTableCell
                cellaSelect = CType(e.Item.FindControl("cellaSelectSett"), System.Web.UI.HtmlControls.HtmlTableCell)
                cellaSelect.ID = "SETT_" & e.Item.ItemIndex + 1
                Dim LKB_tempSett As LinkButton
                oResource.setLinkButton(CType(cellaSelect.FindControl("LKBSettimana"), LinkButton), True, True)
                LKB_tempSett = CType(cellaSelect.FindControl("LKBSettimana"), LinkButton)
                AddHandler LKB_tempSett.Click, AddressOf LKBSettimana_Click

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub setupGiorni(ByVal RPTgiorni As Repeater, ByVal settimana As Integer)
        Dim i As Integer
        Dim dsGiorni As DataSet = New DataSet
        Dim tabGiorni As DataTable = New System.Data.DataTable
        Dim colData As DataColumn = New System.Data.DataColumn
        Dim colID As DataColumn = New System.Data.DataColumn
        Dim rowGiorni As DataRow
        Dim datainizio As Date

        datainizio = CDate(Session("meseAttuale"))

        Try
            tabGiorni.TableName = "tabellaGiorni"
            colData.DataType = System.Type.GetType("System.String")
            colData.ColumnName = "giorno"
            tabGiorni.Columns.Add(colData)

            colID.DataType = System.Type.GetType("System.String")
            colID.ColumnName = "ID"
            tabGiorni.Columns.Add(colID)
            Dim dataTemp As DateTime
            If datainizio.DayOfWeek = 0 Then
                dataTemp = datainizio.AddDays(-6 + ((settimana - 1) * 7))
            Else
                dataTemp = datainizio.AddDays(1 - datainizio.DayOfWeek + ((settimana - 1) * 7))
            End If

            For i = 1 To 7
                If settimana = 1 Then
                    'controllo il giorno di inizio del mese
                    If datainizio.DayOfWeek = 0 Then
                        rowGiorni = tabGiorni.NewRow()
                        If datainizio.AddDays(i - 7).Month = datainizio.Month Then
                            rowGiorni.Item("giorno") = datainizio.AddDays(i - 7).ToString("dd")
                            rowGiorni.Item("id") = datainizio.AddDays(i - 7).ToString()
                        Else
                            rowGiorni.Item("giorno") = "&nbsp;"
                            rowGiorni.Item("id") = "-1"
                        End If
                        dataTemp = datainizio.AddDays(i - 7)
                    Else
                        rowGiorni = tabGiorni.NewRow()
                        If datainizio.AddDays(i - datainizio.DayOfWeek).Month = datainizio.Month Then
                            rowGiorni.Item("giorno") = datainizio.AddDays(i - datainizio.DayOfWeek).ToString("dd")
                            rowGiorni.Item("id") = datainizio.AddDays(i - datainizio.DayOfWeek).ToString()
                        Else
                            rowGiorni.Item("giorno") = "&nbsp;"
                            rowGiorni.Item("id") = "-1"
                        End If
                        dataTemp = datainizio.AddDays(i - datainizio.DayOfWeek)
                    End If
                Else
                    rowGiorni = tabGiorni.NewRow()
                    If dataTemp.Month = datainizio.Month Then
                        rowGiorni.Item("giorno") = dataTemp.ToString("dd")
                        rowGiorni.Item("id") = dataTemp.ToString()
                    Else
                        rowGiorni.Item("giorno") = "&nbsp;"
                        rowGiorni.Item("id") = "-1"
                    End If
                    dataTemp = dataTemp.AddDays(1)
                End If
                tabGiorni.Rows.Add(rowGiorni)
            Next
            dsGiorni.Tables.Add(tabGiorni)
            RPTgiorni.DataSource = dsGiorni.Tables("tabellaGiorni")
            RPTgiorni.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub RPTgiorni_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim cellaColore As System.Web.UI.HtmlControls.HtmlTableCell
        Dim RowEventiGiorno() As DataRow
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try


                cellaColore = CType(e.Item.FindControl("CellaColore"), System.Web.UI.HtmlControls.HtmlTableCell)
                If CStr(e.Item.DataItem("giorno")).CompareTo("&nbsp;") = 0 Then
                    'la cella è vuota
                    cellaColore.BorderColor = "#e5ede7"
                    cellaColore.BgColor = "#e5ede7"
                Else
                    Dim LKB_temp As LinkButton
                    LKB_temp = CType(cellaColore.FindControl("LKB_temp"), LinkButton)
                    AddHandler LKB_temp.Click, AddressOf ClickGiorno
                    LKB_temp.Visible = True
                    LKB_temp.ToolTip = CDate(e.Item.DataItem("ID")).ToString("dddd dd/MM/yy", oResource.CultureInfo.DateTimeFormat)
                    oResource.setLinkButton(LKB_temp, True, True)
                    Dim dataAttuale As Date
                    dataAttuale = CDate(e.Item.DataItem("ID"))
                    If dataAttuale.Date = Now.Date Then
                        cellaColore.BgColor = "#fff4c0"
                    ElseIf dataAttuale.DayOfWeek = DayOfWeek.Sunday Then
                        cellaColore.BgColor = "#e3b996"
                    End If

                    Dim StringaFiltro As String

                    StringaFiltro = "(ORRI_inizio >= '" & dataAttuale.ToString("dd/MM/yyyy") & " 00:00:00.000" & "') AND (ORRI_inizio <= '" & dataAttuale.ToString("dd/MM/yyyy") & " 23:59:59.000')"
                    StringaFiltro = StringaFiltro & " or  (ORRI_fine <= '" & dataAttuale.ToString("dd/MM/yyyy") & " 23:59:59.000' and ORRI_fine >= '" & dataAttuale.ToString("dd/MM/yyyy") & " 00:00:00.000')"
                    StringaFiltro = StringaFiltro & " or  (ORRI_fine > '" & dataAttuale.ToString("dd/MM/yyyy") & " 23:59:59.000' and ORRI_inizio < '" & dataAttuale.ToString("dd/MM/yyyy") & " 00:00:00.000')"

                    RowEventiGiorno = oDataset.Tables(0).Select(StringaFiltro)

                    If RowEventiGiorno.Length > 0 Then
                        cellaColore.BgColor = "#9cbfab"
                    End If

                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub SetupGiorniSettimana()
        Dim i As Integer
        Dim dataI As Date
        Dim dsGiorniS As DataSet = New DataSet
        Dim tabGiorniS As DataTable = New System.Data.DataTable
        Dim colNomeGiorni As DataColumn = New System.Data.DataColumn
        Dim rowGiorni As DataRow

        Try
            dataI = Now.AddDays(-Now.DayOfWeek)

            tabGiorniS.TableName = "NomiGiorni"
            colNomeGiorni.DataType = System.Type.GetType("System.String")
            colNomeGiorni.ColumnName = "giornoSettimana"
            tabGiorniS.Columns.Add(colNomeGiorni)

            rowGiorni = tabGiorniS.NewRow()
            rowGiorni.Item("giornoSettimana") = ""
            tabGiorniS.Rows.Add(rowGiorni)

            For i = 1 To 7
                rowGiorni = tabGiorniS.NewRow()
                rowGiorni.Item("giornoSettimana") = dataI.AddDays(i).ToString("ddd", oResource.CultureInfo.DateTimeFormat).ToUpper
                tabGiorniS.Rows.Add(rowGiorni)
            Next
            dsGiorniS.Tables.Add(tabGiorniS)
            RPTgiorniSettimana.DataSource = dsGiorniS.Tables("NomiGiorni")
            RPTgiorniSettimana.DataBind()
        Catch ex As Exception

        End Try
    End Sub

#End Region

    Public Sub LKBSettimana_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lkbTemp As LinkButton
        lkbTemp = CType(sender, System.Web.UI.WebControls.LinkButton)
        Session("dtInizioSett") = CDate(lkbTemp.CommandArgument)
        Try
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
        Catch ex As Exception

        End Try
        Response.Redirect(".\CalendarioSettimanale.aspx")
    End Sub

    Private Sub LKBmesePrec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmesePrec.Click
        Session("meseAttuale") = CDate(Session("meseAttuale")).AddMonths(-1)
        CreaCalendario()
    End Sub

    Private Sub LKBmeseSuc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmeseSuc.Click
        Session("meseAttuale") = CDate(Session("meseAttuale")).AddMonths(1)
        CreaCalendario()
    End Sub

    Private Sub BTNVai_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNVai.Click
        Session("meseAttuale") = CDate("01/" & DDLVaiA_mesi.SelectedValue & "/" & DDLVaiA_anni.SelectedValue)
        CreaCalendario()
    End Sub

    Private Sub LKBgoTOannuale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgoTOannuale.Click
        Session("AnnoAttuale") = CDate(Session("meseAttuale")).Year
        Try
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
        Catch ex As Exception

        End Try
        Response.Redirect(".\CalendarioAnnuale.aspx")
    End Sub

    Private Sub LKBgoTOsettimanale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgoTOsettimanale.Click
        Dim DataTemp As Date
        DataTemp = CDate("01/" & CDate(Session("meseAttuale")).Month & "/" & CDate(Session("meseAttuale")).Year)

        If DataTemp.DayOfWeek = 0 Then
            DataTemp = DataTemp.AddDays(-6)
        Else
            DataTemp = DataTemp.AddDays(1 - DataTemp.DayOfWeek)
        End If

        Try
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
        Catch ex As Exception

        End Try
        Session("dtInizioSett") = DataTemp
        Response.Redirect(".\CalendarioSettimanale.aspx")
    End Sub

    Private Sub RPTgiorniSettimana_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTgiorniSettimana.ItemCreated
        If e.Item.ItemIndex Mod 7 = 0 Then
            Dim cellaTemp As System.Web.UI.HtmlControls.HtmlTableCell
            cellaTemp = CType(e.Item.FindControl("CellaTemp"), System.Web.UI.HtmlControls.HtmlTableCell)
            If e.Item.ItemIndex = 0 Then
                cellaTemp.BgColor = "#e5ede7"
            Else
                cellaTemp.BgColor = "#e3b996"
            End If
        End If
    End Sub

    Private Sub RBLFiltroComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLFiltroComunita.SelectedIndexChanged
        CreaCalendario()
    End Sub

    Private Sub LNBnascondi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnascondi.Click
        PNLpermessi.Visible = False

    End Sub

    Public Sub ClickGiorno(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim LKB_temp As LinkButton
            LKB_temp = CType(sender, LinkButton)

            Dim DataTemp As Date
            DataTemp = CDate(LKB_temp.CommandArgument)

            If DataTemp.DayOfWeek = 0 Then
                DataTemp = DataTemp.AddDays(-6)
            Else
                DataTemp = DataTemp.AddDays(1 - DataTemp.DayOfWeek)
            End If

            Dim i As Integer
            For i = 0 To 6
                If DataTemp.DayOfWeek = i Then
                    Session("giorniVisual") = Session("giorniVisual") & i
                Else
                    Session("giorniVisual") = Session("giorniVisual") & "8"
                End If
            Next

            Try
                Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
            Catch ex As Exception

            End Try

            Session("dtInizioSett") = DataTemp
            Response.Redirect(".\CalendarioSettimanale.aspx")

        Catch ex As Exception

        End Try

    End Sub

#Region "CBXL filtro tipo eventi"

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
        CBXLFiltroEventi.Visible = False
        BTNApplicaFiltroEventi.Visible = False
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

    End Sub

    Private Sub Bind_FiltroEventi()
        Dim oDataset_TPEV As New DataSet
        Dim oTipoEvento As New COL_Tipo_Evento
        Dim i As Integer
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
            CBXLFiltroEventi.Items.Insert(0, New ListItem(oResource.getValue("CBXLFiltroEventi.-2"), -2))
        End Try

    End Sub


#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_CalendarioEventi"
        oResource.Folder_Level1 = "Eventi"
        oResource.setCulture()
    End Sub

    Private Sub Setup_Internazionalizzazione()
        With oResource
            .setCheckBox(CBEventiTutti)
            .setButton(BTNfiltroEventi, True, , , True)
            .setButton(BTNApplicaFiltroEventi, True, , , True)
            .setLabel(LBFiltroComunita)
            .setLinkButton(LKBgoTOsettimanale, True, True)
            .setLinkButton(LKBgoTOannuale, True, True)
            'Me.LBtitolo.Text = .getValue("LBtitoloMese")
            Me.Master.ServiceTitle = .getValue("LBtitoloMese")
            .setLabel(LBLVisualizzazione)
            .setLabel(LBLcambiaMese)
            .setLabel(LBLEventiVisual)
            .setCheckBox(CBEventiTutti)
            .setButton(BTNfiltroEventi, True, , , True)
            .setButton(BTNApplicaFiltroEventi, True, , , True)
            .setButton(BTNVai, True, , , True)
            .setRadioButtonList(RBLFiltroComunita, "-1")
            .setRadioButtonList(RBLFiltroComunita, "0")
        End With
    End Sub

#End Region

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

End Class

'<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
'<html>
'	<head runat="server">
'		<title>Comunità On Line - Calendario Mensile</title>
'		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
'		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
'		<meta name="vs_defaultClientScript" content="JavaScript"/>
'		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
'		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
'<%--		<script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>--%>
'	</head>
'	<body >
'		<form id="aspnetForm" method="post" runat="server">
'		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
'			<table id="table1" cellSpacing="1" align="center" cellPadding="1" width="780" border="0">
'				<tr>
'					<td><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
'				</tr>
'				<tr>
'					<td>

'					</td>
'				</tr>
'			</table>
'			<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
'		</form>
'	</body>
'</html>
