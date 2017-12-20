Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class RicercaEvento
    Inherits System.Web.UI.Page
   Protected oResource As ResourceManager

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

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum


    Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LBvisualizzazione As System.Web.UI.WebControls.Label
    Protected WithEvents LKBgoTOsettimanale As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBgoTOmensile As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBgoTOannuale As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBelimina As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnopermessi As System.Web.UI.WebControls.Label

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

#Region "Filtri"
    Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBAzione_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLricerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBcategoria As System.Web.UI.WebControls.Label
    Protected WithEvents DDLCategoria As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBFiltroComunita As System.Web.UI.WebControls.Label
    Protected WithEvents RBLFiltroComunita As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TBLcalendari As System.Web.UI.WebControls.Table
    Protected WithEvents TBCinizio As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBdataInizio As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdataI As System.Web.UI.WebControls.TextBox
    Protected WithEvents TBCfine As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBDataFine As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdataF As System.Web.UI.WebControls.TextBox
    Protected WithEvents TBCscriptSingolo As System.Web.UI.WebControls.TableCell
    Protected WithEvents TBCscript As System.Web.UI.WebControls.TableCell
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
#End Region

    Protected WithEvents TBRdati As System.Web.UI.WebControls.TableRow
    Protected WithEvents DGEventi As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents HDabilitato As System.Web.UI.HtmlControls.HtmlInputHidden

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Try
            Dim isForComunita As Boolean = False
            Try
                If Session("idComunita") > 0 Then
                    isForComunita = True
                End If
            Catch ex As Exception

            End Try

            Me.PNLcontenuto.Visible = True
            Me.PNLpermessi.Visible = False

            If isForComunita Then
                Dim oServizio As New UCServices.Services_Eventi
                Try
                    With oServizio
                        .PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                        If .PermessiAssociati = "" Then
                            .PermessiAssociati = "00000000000000000000000000000000"
                        End If
                    End With

                Catch ex As Exception
                    oServizio.PermessiAssociati = "00000000000000000000000000000000"
                End Try
                If Not (oServizio.ReadEvents Or oServizio.DelEvents Or oServizio.AdminService Or oServizio.ChangeEvents Or oServizio.AddEvents) Then
                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                    Exit Sub
                End If
            End If
            If Not Page.IsPostBack Then
                Me.Setup_Internazionalizzazione()
                Me.Bind_Dati(isForComunita)
            End If
        Catch ex As Exception

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
    Private Sub Bind_Dati(ByVal isForComunita As Boolean)
        Dim RuoloID As Integer

        If isForComunita Then
            RBLFiltroComunita.Enabled = True
            Try
                RuoloID = Session("IdRuolo")
            Catch ex As Exception

            End Try
            If RuoloID = Main.TipoRuoloStandard.AccessoNonAutenticato Then
                Me.RBLFiltroComunita.Enabled = False
                Me.RBLFiltroComunita.SelectedValue = 0
            Else
                RBLFiltroComunita.Enabled = True
            End If
        Else
            RBLFiltroComunita.Enabled = False
            RBLFiltroComunita.SelectedIndex = -1
        End If

        Try
            If Request.Cookies("CalendarioSett")("RBLComunita") = "-1" Or Request.Cookies("CalendarioSett")("RBLComunita") = "0" Then
                RBLFiltroComunita.SelectedValue = Request.Cookies("CalendarioSett")("RBLComunita")
            Else
                RBLFiltroComunita.SelectedValue = "0"
            End If
        Catch ex As Exception
            RBLFiltroComunita.SelectedValue = "0"
        End Try

        Me.Bind_TipologiaEvento()
        Session("Evento_CMNT_id") = Nothing

        If Me.Request.QueryString("reset") = "true" Then
            Try
                If IsNumeric(Me.Request.Cookies("RicercaEvento")("DDLCategoria")) Then
                    Me.SetupSearchParameters()
                    Me.Bind_GrigliaEventi()
                End If
            Catch ex As Exception
                Me.DDLricerca.SelectedValue = 4
                Me.TXBdataI.Text = Format(CDate(Date.Now), "d/M/yyyy")
                Me.TXBdataF.Text = Format(CDate(Date.Now), "d/M/yyyy")
                Me.TBLcalendari.Visible = False
                Me.DGEventi.Visible = False
            End Try
        Else
            Me.DDLricerca.SelectedValue = 4
            Me.TXBdataI.Text = Format(CDate(Date.Now), "d/M/yyyy")
            Me.TXBdataF.Text = Format(CDate(Date.Now), "d/M/yyyy")
            Me.TBLcalendari.Visible = False
            Me.DGEventi.Visible = False
        End If
    End Sub
    Private Sub Bind_TipologiaEvento()
        Dim oDataset As New DataSet
        Dim oTipoEvento As New COL_Tipo_Evento
        Try
            DDLCategoria.Items.Clear()
            oDataset = oTipoEvento.Elenca(CInt(Session("LinguaID")))
            If oDataset.Tables(0).Rows.Count > 0 Then
                DDLCategoria.Items.Clear()
                Dim oListItem As New ListItem
                DDLCategoria.DataTextField = "TPEV_nome"
                DDLCategoria.DataValueField = "TPEV_id"
                DDLCategoria.DataSource = oDataset
                DDLCategoria.DataBind()
                DDLCategoria.Enabled = True
                DDLCategoria.Items.Add(New ListItem("Evento Personale", 0))
                oResource.setDropDownList(DDLCategoria, 0)
                DDLCategoria.Items.Insert(0, New ListItem("-- Tutti --", -1))
                oResource.setDropDownList(DDLCategoria, -1)
                DDLCategoria.SelectedValue = -1
            Else
                DDLCategoria.Items.Clear()
                DDLCategoria.Items.Insert(0, New ListItem("-- Tutti --", -1))
                oResource.setDropDownList(DDLCategoria, -1)
                DDLCategoria.SelectedValue = -1
            End If
        Catch ex As Exception
            DDLCategoria.Items.Clear()
            DDLCategoria.Items.Insert(0, New ListItem("-- Tutti --", -1))
            oResource.setDropDownList(DDLCategoria, -1)
            DDLCategoria.SelectedValue = -1
        End Try

    End Sub

    Private Sub Bind_GrigliaEventi()
        Try
            ' Tipo Evento - Categoria

            If Me.DDLCategoria.SelectedValue <> 0 Then
                Try
                    Me.Bind_ElencoEventi()
                Catch ex As Exception

                End Try
            Else
                Try
                    Me.Bind_ElencoReminder()
                Catch ex As Exception
                End Try
            End If
        Catch ex As Exception
            DGEventi.Visible = False
            Me.LBnoRecord.Visible = True
            Me.LBnoRecord.Text = oResource.getValue("LBnoRecord.2")
        End Try
    End Sub

    Private Sub Bind_ElencoEventi()
        Dim totale, i, ComunitaID As Integer
        Dim oDataset As New DataSet
        Dim oDatasetReminder As New DataSet
        Dim oFiltroRicercaEventi As New Main.FiltroRicercaEventi
        Dim oEvento As New COL_Evento
        Dim oReminder As New COL_Reminder
        Dim oListItem As New ListItem
        Dim oPersona As New COL_Persona
        Dim oServizio As New Services_Eventi
        oPersona = Session("objPersona")

        Try
            If Not Session("idComunita") Is Nothing And RBLFiltroComunita.SelectedValue = 0 Then
                ComunitaID = CInt(Session("idComunita"))
            Else
                ComunitaID = -1
            End If
        Catch ex As Exception
            ComunitaID = -1
        End Try
        oEvento.Comunita.Id = ComunitaID
        oEvento.TipoEvento.Id = Me.DDLCategoria.SelectedValue
        Select Case Me.DDLricerca.SelectedValue
            Case "0"
                oDataset = oEvento.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataF.Text), oFiltroRicercaEventi.CompresiTraDueDate, oPersona.Id, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
            Case "1"
                oDataset = oEvento.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.PrimaDiUnaData, oPersona.Id, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
            Case "2"
                oDataset = oEvento.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.DopoUnaData, oPersona.Id, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
            Case "3"
                oDataset = oEvento.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.InUnaDataSpecifica, oPersona.Id, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
            Case "4"
                oDataset = oEvento.TrovaEventi(Date.Today, Date.Today, oFiltroRicercaEventi.TutteLeDate, oPersona.Id, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
            Case "5"
                oDataset = oEvento.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataF.Text), oFiltroRicercaEventi.EsclusiCompresiTraDueDate, oPersona.Id, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
            Case "6"
                oDataset = oEvento.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.ModificatiDopoIl, oPersona.Id, oServizio.GetPermission_Admin, Session("LinguaID"), False, oServizio.GetPermission_Admin, oServizio.GetPermission_AddEvents, oServizio.GetPermission_ChangeEvents, oServizio.GetPermission_DelEvents, oServizio.GetPermission_ReadEvents)
        End Select
        'se seleziono TUTTI eventi devo estrarre anche i "REMINDER"
        If Me.DDLCategoria.SelectedValue = -1 Then
            oReminder.idPersona = oPersona.Id
            Select Case Me.DDLricerca.SelectedValue
                Case "0"
                    oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataF.Text), oFiltroRicercaEventi.CompresiTraDueDate)
                Case "1"
                    oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.PrimaDiUnaData)
                Case "2"
                    oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.DopoUnaData)
                Case "3"
                    oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.InUnaDataSpecifica)
                Case "4"
                    oDatasetReminder = oReminder.TrovaEventi(Date.Today, Date.Today, oFiltroRicercaEventi.TutteLeDate)
                Case "5"
                    oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataF.Text), oFiltroRicercaEventi.EsclusiCompresiTraDueDate)
                Case "6"
                    oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.ModificatiDopoIl)
            End Select
            i = 0
            If oDatasetReminder.Tables(0).Rows.Count > 0 Then
                Dim oRow As DataRow
                For i = 0 To oDatasetReminder.Tables(0).Rows.Count - 1
                    oRow = oDataset.Tables(0).NewRow
                    oRow.Item("TPEV_id") = -1
                    oRow.Item("ORRI_id") = CInt(oDatasetReminder.Tables(0).Rows(i).Item("RMND_id")) * -1
                    oRow.Item("EVNT_nome") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_oggetto")
                    oRow.Item("ORRI_inizio") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")
                    oRow.Item("ORRI_fine") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_fine")
                    oRow.Item("ora_inizio") = CDate(oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")).ToString("dd/MM/yy '-' HH:mm", oResource.CultureInfo.DateTimeFormat)
                    oRow.Item("ora_fine") = CDate(oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_fine")).ToString("dd/MM/yy '-' HH:mm", oResource.CultureInfo.DateTimeFormat)
                    oRow.Item("CMNT_nome") = ""
                    oRow.Item("TPEV_nome") = oResource.getValue("DDLCategoria.0")
                    'oRow.Item("ORRI_dataModifica") = CDate(oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_creazione")).ToString("dd/MM/yy  HH:mm ", oResource.CultureInfo.DateTimeFormat)
                    oDataset.Tables(0).Rows.Add(oRow)
                Next
            End If
        End If
        totale = oDataset.Tables(0).Rows.Count
        If totale <> 0 Then

            totale = oDataset.Tables(0).Rows.Count
            If totale <> 0 Then
                Dim j As Integer
                Dim datatemp As String
                oDataset.AcceptChanges()
                For j = 0 To totale - 1
                    oDataset.Tables(0).Rows(j).Item("ora_inizio") = CDate(oDataset.Tables(0).Rows(j).Item("ORRI_inizio")).ToString("dd/MM/yy '-' HH:mm", oResource.CultureInfo.DateTimeFormat)
                    oDataset.Tables(0).Rows(j).Item("ora_fine") = CDate(oDataset.Tables(0).Rows(j).Item("ORRI_fine")).ToString("dd/MM/yy '-' HH:mm", oResource.CultureInfo.DateTimeFormat)
                Next
            End If

            Me.LBnoRecord.Visible = False
            Dim oDataview As DataView
            oDataview = oDataset.Tables(0).DefaultView
            If viewstate("SortExspression") = "" Then
                viewstate("SortExspression") = "ORRI_inizio"
                viewstate("SortDirection") = "asc"
            End If
            oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")
            '  DGEventi.CurrentPageIndex = 0

            DGEventi.DataSource = oDataview
            DGEventi.DataBind()
            DGEventi.Columns(4).Visible = True
            DGEventi.Columns(8).Visible = True
            DGEventi.Columns(9).Visible = True
            DGEventi.Columns(10).Visible = False
            DGEventi.Visible = True
        Else
            DGEventi.Visible = False
            Me.LBnoRecord.Visible = True
            Me.LBnoRecord.Text = oResource.getValue("LBnoRecord.1")
        End If
    End Sub
    Private Sub Bind_ElencoReminder()
        Dim oReminder As New COL_Reminder
        Dim oDataset As New DataSet
        Dim oDatasetReminder As New DataSet
        Dim PRSN_ID, i, totale As Integer
        Dim oFiltroRicercaEventi As New Main.FiltroRicercaEventi

        oDataset.Tables.Add()
        oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_id"))
        oDataset.Tables(0).Columns.Add(New DataColumn("TPEV_id"))
        oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_id"))
        oDataset.Tables(0).Columns.Add(New DataColumn("EVNT_nome"))
        oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_inizio"))
        oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_fine"))
        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_nome"))
        oDataset.Tables(0).Columns.Add(New DataColumn("TPEV_nome"))
        oDataset.Tables(0).Columns.Add(New DataColumn("annoAccademico"))
        oDataset.Tables(0).Columns.Add(New DataColumn("ORRI_dataModifica"))
        oDataset.Tables(0).Columns.Add(New DataColumn("ora_inizio"))
        oDataset.Tables(0).Columns.Add(New DataColumn("ora_fine"))
        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_id"))

        Try
            PRSN_ID = Session("objPersona").id
        Catch ex As Exception
            PRSN_ID = 0
        End Try

        oReminder.idPersona = PRSN_ID
        Select Case Me.DDLricerca.SelectedValue
            Case "0"
                oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataF.Text), oFiltroRicercaEventi.CompresiTraDueDate)
            Case "1"
                oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.PrimaDiUnaData)
            Case "2"
                oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.DopoUnaData)
            Case "3"
                oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.InUnaDataSpecifica)
            Case "4"
                oDatasetReminder = oReminder.TrovaEventi(Date.Today, Date.Today, oFiltroRicercaEventi.TutteLeDate)
            Case "5"
                oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataF.Text), oFiltroRicercaEventi.EsclusiCompresiTraDueDate)
            Case "6"
                oDatasetReminder = oReminder.TrovaEventi(CDate(Me.TXBdataI.Text), CDate(Me.TXBdataI.Text), oFiltroRicercaEventi.ModificatiDopoIl)
        End Select
        totale = oDatasetReminder.Tables(0).Rows.Count
        If totale > 0 Then
            Try

                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).NewRow
                    oRow.Item("EVNT_id") = 0
                    oRow.Item("TPEV_id") = -1
                    oRow.Item("ORRI_id") = CInt(oDatasetReminder.Tables(0).Rows(i).Item("RMND_id")) * -1
                    oRow.Item("EVNT_nome") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_oggetto")
                    oRow.Item("ORRI_inizio") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")
                    oRow.Item("ORRI_fine") = oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_fine")
                    oRow.Item("ora_inizio") = CDate(oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_inizio")).ToString("dd/MM/yy '-' HH:mm", oResource.CultureInfo.DateTimeFormat)
                    oRow.Item("ora_fine") = CDate(oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_fine")).ToString("dd/MM/yy '-'  HH:mm", oResource.CultureInfo.DateTimeFormat)
                    oRow.Item("CMNT_nome") = ""
                    oRow.Item("TPEV_nome") = oResource.getValue("DDLCategoria.0")
                    oRow.Item("annoAccademico") = " "
                    oRow.Item("ORRI_dataModifica") = CDate(oDatasetReminder.Tables(0).Rows(i).Item("RMND_data_creazione")).ToString("dd/MM/yy '-'  HH:mm", oResource.CultureInfo.DateTimeFormat)
                    oRow.Item("CMNT_id") = 0
                    oDataset.Tables(0).Rows.Add(oRow)
                Next
            Catch ex As Exception

            End Try

            Me.LBnoRecord.Visible = False
            Dim oDataview As DataView
            oDataview = oDataset.Tables(0).DefaultView
            If viewstate("SortExspression") = "" Then
                viewstate("SortExspression") = "ORRI_inizio"
                viewstate("SortDirection") = "asc"
            End If
            oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")
            DGEventi.DataSource = oDataview
            DGEventi.DataBind()
            DGEventi.Columns(4).Visible = False
            DGEventi.Columns(8).Visible = False
            DGEventi.Columns(9).Visible = False
            DGEventi.Columns(10).Visible = False
            DGEventi.Visible = True
        Else
            DGEventi.Visible = False
            Me.LBnoRecord.Visible = True
            Me.LBnoRecord.Text = oResource.getValue("LBnoRecord.1")
        End If
    End Sub
#End Region


#Region "Gestione Griglia Eventi"
    Private Sub DGEventi_PageIndexChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGEventi.PageIndexChanged
        DGEventi.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        Me.Bind_GrigliaEventi()
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGEventi.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If e.SortExpression = oSortExpression Then
            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        End If
        Me.Bind_GrigliaEventi()
    End Sub

    Private Sub DGEventi_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGEventi.ItemCommand
        Select Case e.CommandName
            Case "elimina"
                Dim oOrario As New COL_Orario
                Dim oEvento As New COL_Evento
                Dim oReminder As New COL_Reminder
                'se l'id è minore di zero è un reminder
                If DGEventi.Items(e.Item.ItemIndex).Cells(3).Text() < 0 Then
                    oReminder.Id = DGEventi.Items(e.Item.ItemIndex).Cells(3).Text() * -1
                    oReminder.Cancella()
                Else
                    oOrario.Id = DGEventi.Items(e.Item.ItemIndex).Cells(3).Text()
                    oOrario.Cancella()
                End If
                Me.Bind_GrigliaEventi()
            Case "modifica"
                SaveSearchParameters()
                'se orario è minore di 0 è un reminder
                If DGEventi.Items(e.Item.ItemIndex).Cells(3).Text() < 0 Then
                    Session("OrarioID") = Nothing
                    Session("EventoID") = Nothing
                    Session("ReminderID") = DGEventi.Items(e.Item.ItemIndex).Cells(3).Text() * -1
                Else
                    Session("ReminderID") = Nothing
                    Session("OrarioID") = DGEventi.Items(e.Item.ItemIndex).Cells(3).Text()
                    Session("EventoID") = DGEventi.DataKeys.Item(e.Item.ItemIndex)
                    Session("Evento_CMNT_id") = DGEventi.Items(e.Item.ItemIndex).Cells(11).Text()
                End If
                Session("Comando") = "modifica"

                Response.Redirect("./modificaEvento.aspx?from=trova")
            Case "vaiCalendario"
                Dim forExit As Boolean = False
                Try
                    Dim oOrario As New COL_Orario
                    SaveSearchParameters()
                    'variabili che servono per centrare l'evento nel calendario, aprirlo e visuallizare i dettagli
                    Session("ORRI_id") = DGEventi.Items(e.Item.ItemIndex).Cells(3).Text()
                    oOrario.Id = Session("ORRI_id")
                    With oOrario
                        .Estrai()
                        If .Errore = Errori_Db.None Then
                            Session("LBVisibileFine") = .Fine
                            Session("LBVisibileInizio") = .Inizio
                            If .Inizio.DayOfWeek = 0 Then
                                Session("dtInizioSett") = .Inizio.AddDays(-6)
                            Else
                                Session("dtInizioSett") = .Inizio.AddDays(1 - .Inizio.DayOfWeek)
                            End If
                            forExit = True
                            Response.Redirect("./calendarioSettimanale.aspx")
                        Else
                            Me.Bind_GrigliaEventi()
                        End If
                    End With

                Catch ex As Exception
                    If Not forExit Then
                        Me.Bind_GrigliaEventi()
                    End If
                End Try
        End Select
    End Sub

    Private Sub DGEventi_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGEventi.ItemCreated
        Dim i As Integer
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText, StringaMouse As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")


            For i = 0 To sender.columns.count - 1
                If sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If Me.DGEventi.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_SmallOther9"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGEventi, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGEventi, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGEventi.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall

                                If oSortDirection = "asc" Then
                                    '  oText = "5"
                                    oText = "<img src='./../images/dg/downForum.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                Else
                                    '  oText = "6"
                                    oText = "<img src='./../images/dg/upForum.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
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
                                oLinkbutton.CssClass = "ROW_HeaderLink_SmallOther9"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGEventi, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGEventi.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall
                                oLinkbutton.Text = "<img src='./../images/dg/upForum.gif' id='Image_" & i & "' >"
                                If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                End If
                                If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
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
        If (e.Item.ItemType = ListItemType.Footer) Then
            'rimuovo le altre colonne 
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)

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
                    oResource.setPageDatagrid(Me.DGEventi, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oIMBcancella As ImageButton
            Dim oCheckbox As System.Web.UI.HtmlControls.HtmlInputCheckBox
            Dim oCell As New TableCell
            Try

                oCell = CType(e.Item.Cells(1), TableCell)
                oCheckbox = e.Item.Cells(0).FindControl("CBabilitato")
                Try
                    oIMBcancella = oCell.FindControl("IMBCancella")
                    Dim oServizio As New UCServices.Services_Eventi
                    oServizio.PermessiAssociati = e.Item.DataItem("LKSC_Permessi")
                    If oServizio.AdminService Or oServizio.DelEvents Then
                        oIMBcancella.Visible = True
                        Me.LNBelimina.Enabled = True
                        oResource.setImageButton_Datagrid(Me.DGEventi, oIMBcancella, "IMBcancella", True, True, True, True)
                    Else
                        oIMBcancella.Visible = False
                    End If
                Catch ex As Exception

                End Try
                Try
                    If Not IsNothing(oCheckbox) Then
                        oCheckbox.Visible = oIMBcancella.Visible
                        If oCheckbox.Visible Then
                            If InStr(Me.HDabilitato.Value, "," & e.Item.DataItem("ORRI_id") & ",") > 0 Then
                                oCheckbox.Checked = True
                            End If
                            oCheckbox.Value = e.Item.DataItem("ORRI_id")
                        End If
                    End If
                Catch ex As Exception
                    If Not IsNothing(oCheckbox) Then
                        oCheckbox.Visible = False
                    End If
                End Try
            Catch ex As Exception

            End Try

            Try
                Dim oIMBModifica As ImageButton
                oCell = CType(e.Item.Cells(0), TableCell)
                Try
                    oIMBModifica = oCell.FindControl("IMBModifica")

                    Dim oServizio As New UCServices.Services_Eventi
                    oServizio.PermessiAssociati = e.Item.DataItem("LKSC_Permessi")
                    If oServizio.AdminService Or oServizio.ChangeEvents Then
                        oIMBModifica.Visible = True
                        oResource.setImageButton_Datagrid(Me.DGEventi, oIMBModifica, "IMBModifica", True, True, True, False)
                    Else
                        oIMBModifica.Visible = False
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

            Dim oLinkButton As ImageButton
            Try
                oCell = CType(e.Item.Cells(3), TableCell)
                Try
                    oLinkButton = oCell.Controls(0)
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='';return true;")
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End If

    End Sub
#End Region


#Region "eventi webcontrols"
    Private Sub DDLricerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLricerca.SelectedIndexChanged
        Me.TBLcalendari.Visible = False
        Me.TBCinizio.Visible = False
        Me.TBCfine.Visible = False
        Me.TBCscriptSingolo.Visible = False
        Me.TBCscript.Visible = False
        Me.DGEventi.Visible = False
        Select Case Me.DDLricerca.SelectedValue
            Case 0
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCfine.Visible = True
                Me.TBCscript.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.0")
                Me.LBDataFine.Text = oResource.getValue("LBDataFine.0")
            Case 1
                Me.TBLcalendari.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.1")
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
            Case 2
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.2")
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
            Case 3
                Me.TBLcalendari.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.3")
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
            Case 4
                Exit Select
            Case 5
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCfine.Visible = True
                Me.TBCscript.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.5")
                Me.LBDataFine.Text = oResource.getValue("LBDataFine.5")
            Case 6
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.4")
        End Select
        Session("EventoID") = Nothing
        Session("OrarioID") = Nothing
    End Sub

    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.ViewState("intCurPage") = 0
        Me.DGEventi.CurrentPageIndex = 0
        Me.LNBelimina.enabled = False
        Me.Bind_GrigliaEventi()
    End Sub

    Private Sub LKBgoTOannuale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgoTOannuale.Click
        SaveSearchParameters()
        Session("AnnoAttuale") = Now.Year
        Response.Redirect(".\CalendarioAnnuale.aspx")
    End Sub

    Private Sub LKBgoTOsettimanale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgoTOsettimanale.Click
        Dim DataTemp As Date
        DataTemp = Now
        SaveSearchParameters()

        If DataTemp.DayOfWeek = 0 Then
            DataTemp = DataTemp.AddDays(-6)
        Else
            DataTemp = DataTemp.AddDays(1 - DataTemp.DayOfWeek)
        End If

        Session("dtInizioSett") = DataTemp
        Response.Redirect(".\CalendarioSettimanale.aspx")
    End Sub

    Private Sub LKBgoTOmensile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgoTOmensile.Click
        SaveSearchParameters()
        Session("meseAttuale") = CDate("01/" & Now.Month & "/" & Now.Year)
        Response.Redirect(".\CalendarioMensile.aspx")
    End Sub

#End Region

#Region "Gestione Cookies"
    Private Sub SaveSearchParameters()
        Try
            Response.Cookies("RicercaEvento")("DDLricerca") = Me.DDLricerca.SelectedValue
            Response.Cookies("CalendarioSett")("RBLComunita") = RBLFiltroComunita.SelectedValue
            Response.Cookies("RicercaEvento")("TXBdataI") = Me.TXBdataI.Text
            Response.Cookies("RicercaEvento")("TXBdataF") = Me.TXBdataF.Text
            Response.Cookies("RicercaEvento")("DDLCategoria") = Me.DDLCategoria.SelectedValue
            Response.Cookies("RicercaEvento")("intCurPage") = Me.ViewState("intCurPage")
            Response.Cookies("RicercaEvento")("SortDirection") = Me.ViewState("SortDirection")
            Response.Cookies("RicercaEvento")("SortExspression") = Me.ViewState("SortExspression")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetupSearchParameters()
        Me.DGEventi.Visible = True
        'Recupero fattori di ricerca relativi all'ordinamento
        Try
            Me.ViewState("SortDirection") = Me.Request.Cookies("RicercaEvento")("SortDirection")
            Me.ViewState("SortExspression") = Me.Request.Cookies("RicercaEvento")("SortExspression")
        Catch ex As Exception

        End Try

        Try
            'Recupero dati relativi alla paginazione corrente
            If IsNumeric(Me.Request.Cookies("RicercaEvento")("intCurPage")) Then
                Me.ViewState("intCurPage") = CInt(Me.Request.Cookies("RicercaEvento")("intCurPage"))
                Me.DGEventi.CurrentPageIndex = CInt(Me.ViewState("intCurPage"))
            Else
                Me.ViewState("intCurPage") = 0
                Me.DGEventi.CurrentPageIndex = 0
            End If
        Catch ex As Exception
            Me.ViewState("intCurPage") = 0
            Me.DGEventi.CurrentPageIndex = 0
        End Try
        Try
            Me.TXBdataI.Text = Request.Cookies("RicercaEvento")("TXBdataI")
        Catch ex As Exception
            Me.TXBdataI.Text = Date.Now.ToString("dd/MM/yyyy")
        End Try
        Try
            Me.TXBdataF.Text = Me.Request.Cookies("RicercaEvento")("TXBdataF")
        Catch ex As Exception
            Me.TXBdataF.Text = Date.Now.ToString("dd/MM/yyyy")
        End Try

        Try
            Dim RicercaID As Integer
            RicercaID = Me.Request.Cookies("RicercaEvento")("DDLricerca")
            Me.DDLricerca.SelectedValue = RicercaID
        Catch ex As Exception
            Me.DDLricerca.SelectedIndex = 0
        End Try

        Me.TBLcalendari.Visible = False
        Me.TBCinizio.Visible = False
        Me.TBCfine.Visible = False
        Me.TBCscriptSingolo.Visible = False
        Me.TBCscript.Visible = False

        Select Case Me.DDLricerca.SelectedValue
            Case 0
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCfine.Visible = True
                Me.TBCscript.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.0")
                Me.LBDataFine.Text = oResource.getValue("LBDataFine.0")
            Case 1
                Me.TBLcalendari.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.1")
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
            Case 2
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.2")
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
            Case 3
                Me.TBLcalendari.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.3")
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
            Case 4
                Exit Select
            Case 5
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCfine.Visible = True
                Me.TBCscript.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.5")
                Me.LBDataFine.Text = oResource.getValue("LBDataFine.5")
            Case 6
                Me.TBLcalendari.Visible = True
                Me.TBCinizio.Visible = True
                Me.TBCscriptSingolo.Visible = True
                Me.LBdataInizio.Text = oResource.getValue("LBdataInizio.4")
        End Select

        Try
            If IsNumeric(Me.Request.Cookies("RicercaEvento")("DDLCategoria")) Then
                Try
                    Me.DDLCategoria.SelectedValue = Me.Request.Cookies("RicercaEvento")("DDLCategoria")
                Catch ex As Exception
                    DDLCategoria.SelectedValue = "-1"
                End Try
            End If
        Catch ex As Exception
            DDLCategoria.SelectedValue = "-1"
        End Try

    End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_RicercaEvento"
        oResource.Folder_Level1 = "Eventi"
        oResource.setCulture()
    End Sub
    Private Sub Setup_Internazionalizzazione()
        With oResource
            .setLabel(Me.LBtitolo)
            .setLabel(Me.LBAzione_t)
            .setDropDownList(Me.DDLricerca, 4)
            .setDropDownList(Me.DDLricerca, 3)
            .setDropDownList(Me.DDLricerca, 0)
            .setDropDownList(Me.DDLricerca, 5)
            .setDropDownList(Me.DDLricerca, 1)
            .setDropDownList(Me.DDLricerca, 2)
            .setDropDownList(Me.DDLricerca, 6)
            .setLabel(Me.LBvisualizzazione)
            .setLinkButton(Me.LKBgoTOsettimanale, True, True)
            .setLinkButton(Me.LKBgoTOmensile, True, True)
            .setLinkButton(Me.LKBgoTOannuale, True, True)
            .setLinkButton(Me.LNBelimina, True, True)

            Me.LNBelimina.Attributes.Add("onclick", "window.status='';return Conferma('" & Replace(.getValue("Conferma"), "'", "\'") & "','" & Replace(.getValue("ConfermaSelezione"), "'", "\'") & "');")

            .setLabel(Me.LBdataInizio)
            .setLabel(Me.LBDataFine)

            .setLabel(Me.LBtipoRicerca_t)
            .setLabel(Me.LBFiltroComunita)
            .setLabel(LBcategoria)

            .setRadioButtonList(Me.RBLFiltroComunita, -1)
            .setRadioButtonList(Me.RBLFiltroComunita, 0)
            .setButton(BTNCerca, True, , , True)

            .setHeaderDatagrid(Me.DGEventi, 4, "TPEV_nome", True)
            .setHeaderDatagrid(Me.DGEventi, 5, "EVNT_nome", True)
            .setHeaderDatagrid(Me.DGEventi, 6, "ORRI_inizio", True)
            .setHeaderDatagrid(Me.DGEventi, 7, "ORRI_fine", True)
            .setHeaderDatagrid(Me.DGEventi, 8, "annoAccademico", True)
            .setHeaderDatagrid(Me.DGEventi, 9, "CMNT_nome", True)

        End With
    End Sub
#End Region

    Private Sub LNBelimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelimina.Click
        Dim ListaEventi As String

        ListaEventi = Me.HDabilitato.Value
        If ListaEventi = "" Or ListaEventi = "," Or ListaEventi = ",," Then
            Me.HDabilitato.Value = ""
            Me.Bind_GrigliaEventi()
        Else
            Dim ElencoID As String()
            Try
                Dim i, totale, OrarioID As Integer
                Dim oOrario As New COL_Orario
                Dim oReminder As New COL_Reminder

                ElencoID = Me.HDabilitato.Value.Split(",")
                totale = ElencoID.Length - 2
                For i = 1 To totale
                    If IsNumeric(ElencoID(i)) Then
                        OrarioID = ElencoID(i)
                        If OrarioID > 0 Then
                            oOrario.Id = OrarioID
                            oOrario.Cancella()
                        Else
                            oReminder.Id = OrarioID * -1
                            oReminder.Cancella()
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try
            Me.HDabilitato.Value = ""
            Me.Bind_GrigliaEventi()
        End If
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
End Class