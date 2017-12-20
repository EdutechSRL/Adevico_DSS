Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.Forum


Public Class UC_AggiungiUtenteForum
    Inherits System.Web.UI.UserControl
    Private oResourceAggiungi As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Public Enum Inserimento
        NessunaSelezione = -3
        TipoRuoloMancante = -2
        ErroreModifica = -1
        ErroreInserimento = 0
        InserimentoOk = 1
        ModificaAvvenuta = 2
        SetupOk = 3
    End Enum
    Public Event AggiornaMenu(ByVal showRole As Boolean, ByVal abilitato As Boolean)

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
#Region "datagrid"
    Protected WithEvents DGPersone As System.Web.UI.WebControls.DataGrid
    Protected WithEvents HDabilitato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLpersone As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLpaginazione As System.Web.UI.WebControls.Panel
#End Region
#Region "filtri"
    Protected WithEvents PNLfiltro As System.Web.UI.WebControls.Panel
    Protected WithEvents LBtipoRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton

#End Region
#Region "no users"
    Protected WithEvents PNLNoUsers As System.Web.UI.WebControls.Panel
    Protected WithEvents LBMessaggio As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
#Region "aggiungi con ruolo"
    Protected WithEvents PNLruoloUtente As System.Web.UI.WebControls.Panel
    Protected WithEvents LBdescrizione As System.Web.UI.WebControls.Label
    Protected WithEvents LBtiporuoloAggiungi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoRuoloAggiungi As System.Web.UI.WebControls.DropDownList
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceAggiungi) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
            Bind_TipoRuoloFiltro()
            Me.PNLpersone.Visible = True
            Me.PNLruoloUtente.Visible = False
            Me.PNLNoUsers.Visible = False
            Me.ViewState("intCurPage") = 0
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceAggiungi = New ResourceManager

        oResourceAggiungi.UserLanguages = Code
        oResourceAggiungi.ResourcesName = "pg_UC_GestioneiscrittiForum"
        oResourceAggiungi.Folder_Level1 = "Forum"
        oResourceAggiungi.Folder_Level2 = "UC"
        oResourceAggiungi.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        Dim i As Integer

        With oResourceAggiungi
            .setLabel(LBtipoRuolo_t)
            .setLabel(LBnumeroRecord_t)
            .setLabel(LBtipoRicerca_t)
            .setLabel(LBvalore_t)
            .setLabel(LBdescrizione_t)
            .setDropDownList(DDLTipoRicerca, -2)
            .setDropDownList(DDLTipoRicerca, -3)
            .setDropDownList(DDLTipoRicerca, -4)
            .setButton(BTNCerca)
            .setHeaderDatagrid(Me.DGPersone, 3, "PRSN_Anagrafica", True)
            .setHeaderDatagrid(Me.DGPersone, 4, "PRSN_login", True)
            .setHeaderDatagrid(Me.DGPersone, 5, "TPRL_nome", True)
            .setHeaderDatagrid(Me.DGPersone, 7, "TPPR_descrizione", True)

            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControl("LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    .setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next
        End With

    End Sub
#End Region

    Public Sub SetupControllo(ByVal ReloadFiltri As Boolean, Optional ByVal ricalcola As Boolean = False)
        Me.PNLruoloUtente.Visible = False
        Me.PNLpersone.Visible = True
        Me.PNLfiltro.Visible = True

        If ricalcola Then
            Me.HDabilitato.Value = ""
        End If
        If ReloadFiltri Then
            Me.Bind_Ruoli_Forum()
            Me.Bind_TipoRuoloFiltro()
        End If
        Me.Bind_Griglia(ricalcola)
    End Sub

#Region "bind"
    Private Sub Bind_Griglia(Optional ByVal ricalcola As Boolean = False)
        Dim oPersona As New COL_Persona
        Dim dsTable As New DataSet
        Try
            oPersona = Session("objPersona")
            dsTable = FiltraggioDati(ricalcola)

            dsTable.Tables(0).Columns.Add(New DataColumn("oCheck"))
            Dim i, totale As Integer
            totale = dsTable.Tables(0).Rows.Count


            If totale = 0 Then
                Me.DGPersone.Visible = False
                ''''''  Me.MostraLinkbutton(False)
                Me.DGPersone.VirtualItemCount = 0
                Me.PNLNoUsers.Visible = True
                LBMessaggio.Text = oResourceAggiungi.getValue("LBnorecord.text") '"Nessun utente in questa categoria"
            Else
                Me.DGPersone.VirtualItemCount = dsTable.Tables(0).Rows(0).Item("Totale")
                '''''''''   Me.MostraLinkbutton()

                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = dsTable.Tables(0).Rows(i)

                    If viewstate("Paginazione") <> "si" Then

                    Else

                        Dim t As Integer
                        Dim selezionato() As String
                        selezionato = Me.HDabilitato.Value.Split(",")
                        For t = 1 To selezionato.Length - 2
                            If oRow.Item("RLPC_id") = selezionato(t) Then
                                oRow.Item("oCheck") = "checked"
                                Exit For
                            End If
                        Next

                    End If
                Next
                If totale > 0 Then
                    ''''''''''''Mod_Visualizzazione(totale - 1)
                    Me.DGPersone.Visible = True


                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_Cognome"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    Me.DGPersone.DataSource = oDataview
                    Me.DGPersone.DataBind()
                    Me.PNLNoUsers.Visible = False

                    If Me.DGPersone.PageCount <= 1 Then
                        Me.DGPersone.PagerStyle.Position = PagerPosition.Bottom
                        Me.LBnumeroRecord_t.Visible = False
                        Me.DDLNumeroRecord.Visible = False
                    Else
                        Me.DGPersone.PagerStyle.Position = PagerPosition.TopAndBottom
                        Me.LBnumeroRecord_t.Visible = True
                        Me.DDLNumeroRecord.Visible = True
                    End If
                Else
                    Me.DGPersone.Visible = False
                    Me.PNLNoUsers.Visible = True
                    '''''    Me.MostraLinkbutton(False)
                    LBMessaggio.Text = oResourceAggiungi.getValue("LBnorecord.text") '"Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
                End If
            End If
        Catch ex As Exception
            Me.DGPersone.Visible = False
            PNLNoUsers.Visible = True

            LBMessaggio.Text = oResourceAggiungi.getValue("LBnorecord.text") ' "Nessun utente in questa categoria"
        End Try
    End Sub
    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim oPersona As New COL_Persona
            Dim Valore As String
            oPersona = Session("objPersona")

            Dim TPRL_id, CMNT_id As Integer
            CMNT_id = Session("IdComunita")
            Dim oForum As New COL_Forums
            oForum.Id = Session("IdForum")

            TPRL_id = Me.DDLTipoRuolo.SelectedValue

            Dim oFiltroAnagrafica As Main.FiltroAnagrafica
            Dim oFiltroCampoOrdine As COL_Comunita.FiltroCampoOrdine
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento

            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
            If Me.TXBValore.Text <> "" Then
                Valore = Me.TXBValore.Text.Trim
                If Valore <> "" Then
                    oFiltroAnagrafica = CType(Me.DDLTipoRicerca.SelectedValue, Main.FiltroAnagrafica)

                    If oFiltroAnagrafica = Main.FiltroAnagrafica.dataNascita Then
                        If IsDate(Valore) = False Then
                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Valore = ""
                        End If
                    End If
                End If
            End If

            If viewstate("SortExspression") = "" Or LCase(viewstate("SortExspression")) = "prsn_anagrafica" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
                ' ElseIf LCase(viewstate("SortExspression")) = "prsn_datanascita" Then
                '    oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataNascita
            ElseIf LCase(viewstate("SortExspression")) = "tprl_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoRuolo
            ElseIf LCase(viewstate("SortExspression")) = "tppr_descrizione" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoPersona
            ElseIf LCase(viewstate("SortExspression")) = "prsn_login" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.login
                'ElseIf LCase(viewstate("SortExspression")) = "rlpc_iscrittoil" Then
                '    oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataIscrizione
                'ElseIf LCase(viewstate("SortExspression")) = "prsn_nome" Then
                '    oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.nome
                'ElseIf LCase(viewstate("SortExspression")) = "prsn_cognome" Then
                '    oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            Else
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            End If

            Dim ordinamento As Integer
            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            Try
                If Valore = "" Then
                    If Me.ViewState("intAnagrafica") = "" Then
                        oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                    Else
                        oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                    End If
                End If
            Catch ex As Exception
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
            End Try

            Dim totale As Decimal
            If ricalcola Then
                Me.ViewState("intCurPage") = 0
                Me.DGPersone.CurrentPageIndex = 0
            End If


            Return oForum.ElencaNoIscritti(CMNT_id, oForum.Id, Session("LinguaID"), TPRL_id, Me.DGPersone.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroAnagrafica, oFiltroOrdinamento, oFiltroCampoOrdine)


        Catch ex As Exception
            Return oDataset
        End Try
    End Function
    Private Sub Bind_TipoRuoloFiltro()

        Me.DDLTipoRuolo.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita

            Dim IdComunita As Integer = Session("IdComunita")
            oComunita.Id = IdComunita

            oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)

            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
					Dim oListItem As New ListItem
					If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRL_nome")) Then
						oListItem.Text = "--"
					Else
						oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRL_nome")
					End If
					oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRL_ID")
					Me.DDLTipoRuolo.Items.Add(oListItem)
				Next
				DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
				'Me.BTNmodifica.Enabled = True
			Else
				Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
				' Me.BTNmodifica.Enabled = False
			End If
		Catch ex As Exception
			Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
			' Me.BTNmodifica.Enabled = False
		End Try
		oResourceAggiungi.setDropDownList(Me.DDLTipoRuolo, -1)
	End Sub
	Private Sub Bind_Ruoli_Forum()
		'carica le ddl per la creazione del forum e per la ricerca in gestione iscritti 

		Me.DDLtipoRuoloAggiungi.Items.Clear() 'ddl per la ricerca
		Try
			Dim oDataset As DataSet
			Dim i, Totale As Integer
			Dim oTipoRuoloForum As New COL_TipoRuoloForum
			oDataset = oTipoRuoloForum.Elenca(Session("LinguaID"))

			Totale = oDataset.Tables(0).Rows.Count()

			If Totale > 0 Then
				Totale = Totale - 1
				For i = 0 To Totale
					Dim oListItem As New ListItem
					If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRF_nome")) Then
						oListItem.Text = "--"
					Else
						oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")
					End If
					oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
					Me.DDLtipoRuoloAggiungi.Items.Add(oListItem)

				Next
				Me.DDLtipoRuoloAggiungi.Items.Insert(0, New ListItem("< Ruolo Default >", -1))
				Me.DDLtipoRuoloAggiungi.SelectedValue = -1

			Else
				Me.DDLtipoRuoloAggiungi.Items.Insert(0, New ListItem("< Ruolo Default >", -1))
			End If
		Catch ex As Exception
			Me.DDLtipoRuoloAggiungi.Items.Insert(0, New ListItem("< Ruolo Default >", -1))
		End Try
		oResourceAggiungi.setDropDownList(Me.DDLtipoRuoloAggiungi, -1)
	End Sub
#End Region
#Region "filtri"

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.Bind_Griglia(True)
    End Sub

    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControl("LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
    Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.ViewState("intCurPage") = 0
        Me.DGPersone.CurrentPageIndex = 0
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.DGPersone.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        viewstate("Paginazione") = "si"
        Me.Bind_Griglia(True)
    End Sub
#End Region

#Region "gestione Griglia"
    Sub DGPersone_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGPersone.PageIndexChanged
        'paginazione della datagrid iscritti
        Me.ViewState("intCurPage") = e.NewPageIndex
        viewstate("Paginazione") = "si"
        Me.DGPersone.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia(False)
    End Sub
    Private Sub DGPersone_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGPersone.ItemCreated
        Dim i As Integer
        If IsNothing(oResourceAggiungi) Then
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
                    If Me.DGPersone.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResourceAggiungi.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResourceAggiungi.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGPersone.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
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
                                oResourceAggiungi.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGPersone.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
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
                    oResourceAggiungi.setPageDatagrid(Me.DGPersone, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"

            Try
                If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                End If
            End Try

            'bottone informazioni
            Dim oImagebutton As ImageButton
            Dim Cell As New TableCell
            Dim TPPR_id As Integer
            Dim PRSN_ID As Integer

            Try
                PRSN_ID = e.Item.DataItem("PRSN_id")
                TPPR_id = e.Item.DataItem("PRSN_TPPR_id")
                Dim i_link2 As String
                i_link2 = "./../Comunita/InfoIscritto.aspx?TPPR_ID=" & TPPR_id & "&PRSN_ID=" & PRSN_ID
                Cell = CType(e.Item.Cells(0), TableCell)

                oImagebutton = Cell.FindControl("IMBinfo")
                oResourceAggiungi.setImageButton_Datagrid(Me.DGPersone, oImagebutton, "IMBinfo", True, True)
                'in base al tipo di utente decido la dimensione della finestra di popup
                Select Case TPPR_id
                    Case Main.TipoPersonaStandard.Studente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Docente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','520','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Tutor
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Esterno
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Amministrativo
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.SysAdmin
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Copisteria
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Dottorando
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.DocenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.StudenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Else
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                End Select

                'oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try

            'Aggiungi
            Try
                oImagebutton = e.Item.Cells(0).FindControl("IMBaggiungi")
                If Not IsNothing(oImagebutton) Then
                    oResourceAggiungi.setImageButton_Datagrid(Me.DGPersone, oImagebutton, "IMBaggiungi", True, True, True)

                End If
            Catch ex As Exception

            End Try

        End If
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGPersone.SortCommand
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
        Else
            viewstate("SortDirection") = "asc"
        End If
        Me.Bind_Griglia()
    End Sub
    Private Sub DGPersone_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGPersone.ItemCommand
        If e.CommandName = "Aggiungi" Then
            'inserisco solo l'utente selezionato

            Session("Azione") = "insert"
            Me.PNLruoloUtente.Visible = True
            Me.PNLpersone.Visible = False
            Me.PNLfiltro.Visible = False
            Me.HDabilitato.Value = "," & CInt(DGPersone.Items(e.Item.ItemIndex).Cells(8).Text()) & ","
            Bind_Ruoli_Forum()
            Me.LBdescrizione.Text = DGPersone.Items(e.Item.ItemIndex).Cells(3).Text()
            RaiseEvent AggiornaMenu(True, True)
        End If
    End Sub
#End Region

    Public Function AssociaSelezionati() As Inserimento
        If Me.HDabilitato.Value = "" Or Me.HDabilitato.Value = "," Or Me.HDabilitato.Value = ",," Then
            Return Inserimento.NessunaSelezione
        ElseIf Me.DDLtipoRuoloAggiungi.SelectedValue <= 0 Then
            Return Inserimento.TipoRuoloMancante
        Else
            AggiungiUtenti(Me.HDabilitato.Value)
            Return Inserimento.InserimentoOk
        End If
    End Function

    Public Function ShowIscrizione() As Inserimento
        If IsNothing(oResourceAggiungi) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Me.HDabilitato.Value <> "" Then
            Me.PNLruoloUtente.Visible = True
            Me.PNLpersone.Visible = False
            Me.PNLfiltro.Visible = False
            Bind_Ruoli_Forum()

            oResourceAggiungi.setLabel(Me.LBdescrizione)


            Session("Azione") = "insert"
            Return Inserimento.SetupOk
        Else
            Return Inserimento.NessunaSelezione
        End If
    End Function
    Private Function AggiungiUtenti(ByVal ListaIscritti As String)
        Try

            Dim ElencoIDutenti() As String
            Dim RLPC_id As Integer
            Dim totale, i As Integer

            ElencoIDutenti = ListaIscritti.Split(",")
            totale = ElencoIDutenti.Length - 2

            For i = 1 To totale
                ' isIscritto = False
                RLPC_id = ElencoIDutenti(i)
                Dim oForum As New COL_Forums
                oForum.Id = Session("IdForum")
                oForum.IscriviUtente(RLPC_id, Me.DDLtipoRuoloAggiungi.SelectedValue)
            Next

        Catch ex As Exception

        End Try
        Me.HDabilitato.Value = ""
        Me.Bind_Griglia(True)
        Me.PNLruoloUtente.Visible = False
        Me.PNLpersone.Visible = True
        Me.PNLfiltro.Visible = True
    End Function
End Class