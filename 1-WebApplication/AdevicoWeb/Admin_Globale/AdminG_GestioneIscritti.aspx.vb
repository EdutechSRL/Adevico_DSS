Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class AdminG_GestioneIscritti
    Inherits System.Web.UI.Page

#Region "Tab Strip"
    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
#End Region

    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button

#Region "Pannello Modifica"
    Protected WithEvents LBNomeCognome As System.Web.UI.WebControls.Label

    Protected WithEvents DDLruolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents BTNannulla As System.Web.UI.WebControls.Button
    Protected WithEvents PNLmodifica As System.Web.UI.WebControls.Panel
    Protected WithEvents HDrlpc As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNprsnID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNrlpc_Attivato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNrlpc_Abilitato As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents HDNrlpc_Responsabile As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents CHBresponsabile As System.Web.UI.WebControls.CheckBox
#End Region

#Region "Filtro"
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
    Protected WithEvents PNLfiltri As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox

#End Region

#Region "noquerystring"
    Protected WithEvents BTNlistacmnt As System.Web.UI.WebControls.Button
    Protected WithEvents BTNricercacmnt As System.Web.UI.WebControls.Button
    Protected WithEvents LBnoquery As System.Web.UI.WebControls.Label
    Protected WithEvents PNLnoquery As System.Web.UI.WebControls.Panel
#End Region

#Region "nopermessi"
    Protected WithEvents LBnopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
#End Region

#Region "Pannello DeIscrivi"
    Protected WithEvents PNLdeiscrivi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBinfoDeIscrivi As System.Web.UI.WebControls.Label
    Protected WithEvents BTNannullaDeiscrizione As System.Web.UI.WebControls.Button
    Protected WithEvents BTNdeIscriviCorrente As System.Web.UI.WebControls.Button
    Protected WithEvents BTNdeIscriviTutte As System.Web.UI.WebControls.Button
    Protected WithEvents TBLcomunita As System.Web.UI.WebControls.Table

    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_Path As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNprsn_Id As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Pannello Deiscrivi multiplo"
    Protected WithEvents PNLdeiscriviMultiplo As System.Web.UI.WebControls.Panel
    Protected WithEvents HDNelencoID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LBinfoDeIscrivi_multiplo As System.Web.UI.WebControls.Label
    Protected WithEvents BTNannullaDeiscrizione_multi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNdeIscriviCorrente_multi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNdeIscriviTutte_multi As System.Web.UI.WebControls.Button
#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents DGiscritti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoIscritti As System.Web.UI.WebControls.Label
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents BTNok As System.Web.UI.WebControls.Button
    Protected WithEvents PNLmessaggio As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLiscritti As System.Web.UI.WebControls.Panel
    Protected WithEvents HDazione As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LKBiscritti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBcancellaInAttesa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBabilita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBdisabilita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBelimina As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBseleziona As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents IMBstampa As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label

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
        If Session("objPersona") Is Nothing Then 'se la sessione è scaduta redirecto alla home
            Response.Redirect("./../index.aspx")
        End If
        Dim oPersona As New COL_Persona
        oPersona = Session("objPersona")
		If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
			If Not Page.IsPostBack Then
				If Session("idComunita_forAdmin") Is Nothing Then
					Me.PNLcontenuto.Visible = False
					Me.PNLnoquery.Visible = True
					Me.LBnoquery.Text = "Spiacenti, Errore nell'accesso alla pagina."
				Else
					Dim nomeCMNT As String
					nomeCMNT = COL_Comunita.EstraiNomeBylingua(Session("idComunita_forAdmin"), Session("LinguaID"))
                    'Me.LBtitolo.Text = "- Gestione Iscritti di '" & nomeCMNT & "' -"
                    Me.Master.ServiceTitle = "Gestione Iscritti di '" & nomeCMNT

					Me.ViewState("intCurPage") = 0
					Me.ViewState("intAnagrafica") = -1
					Me.LKBtutti.CssClass = "lettera_Selezionata"

					Me.Bind_TipoRuolo()
					Me.Bind_TipoRuoloFiltro()
					If Session("azione") = "tutti" Then
						Me.Bind_Tutti()
						Me.TBSmenu.SelectedIndex = 2
					ElseIf Session("azione") = "ultimiIscritti" Then
						Me.Bind_UltimiIscritti()
						Me.TBSmenu.SelectedIndex = 1
					ElseIf Session("azione") = "abilitati" Then
						Me.Bind_Abilitati()
						Me.TBSmenu.SelectedIndex = 3
					ElseIf Session("azione") = "bloccati" Then
						Me.Bind_Bloccati()
						Me.TBSmenu.SelectedIndex = 4
					ElseIf Session("azione") = "inattesa" Then
						Me.Bind_inAttesa()
						Me.TBSmenu.SelectedIndex = 5
					Else 'in ogni altro caso
						Me.Bind_Abilitati()
						Me.TBSmenu.SelectedIndex = 3
					End If
					'   Me.MostraLinkbutton(True)
					Session("azione") = "load"

				End If
			End If
			Me.SetupJavascript()
		Else
			Me.PNLcontenuto.Visible = False
			Me.PNLpermessi.Visible = True
			Me.LBnopermessi.Text = "Spiacente, non dispone dei permessi necessari per accedere a tale sezione."
		End If
    End Sub
    Private Sub SetupJavascript()
        'aggiunge ai link button le proprietà da visualizzare nella barra di stato
        Dim i As Integer
        For i = Asc("a") To Asc("z") 'status dei link button delle lettere
            Dim oLinkButton As New LinkButton
            oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
            Dim Carattere As String = Chr(i)

            If IsNothing(oLinkButton) = False Then

                oLinkButton.Attributes.Add("onclick", "window.status='Lettera " & Carattere.ToUpper & "' ;return true;")
                oLinkButton.Attributes.Add("onmouseover", "window.status='Lettera " & Carattere.ToUpper & "' ;return true;")
                oLinkButton.Attributes.Add("onfocus", "window.status='Lettera " & Carattere.ToUpper & "' ;return true;")
                oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                oLinkButton.ToolTip = "Lettera " & Carattere.ToUpper

            End If
        Next
        '-- SETTAGGIO PROPRIETA' LINKBUTTON

        LKBaltro.Attributes.Add("onclick", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onmouseover", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onfocus", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onmouseout", "window.status='';return true;")

        LKBtutti.Attributes.Add("onclick", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onmouseover", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onfocus", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.LKBcancellaInAttesa.Attributes.Add("onclick", "window.status='Elimina gli utenti in attesa di conferma.';return UserForCancella();")
        Me.LKBcancellaInAttesa.Attributes.Add("onmouseover", "window.status='Elimina gli utenti in attesa di conferma.';return true;")
        Me.LKBcancellaInAttesa.Attributes.Add("onfocus", "window.status='Elimina gli utenti in attesa di conferma.';return true;")
        Me.LKBcancellaInAttesa.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.LKBabilita.Attributes.Add("onclick", "window.status='Abilita selezionati';return UserSelezionati();")
        Me.LKBabilita.Attributes.Add("onmouseover", "window.status='Abilita selezionati';return true;")
        Me.LKBabilita.Attributes.Add("onfocus", "window.status='Abilita selezionati';return true;")
        Me.LKBabilita.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.LKBdisabilita.Attributes.Add("onclick", "window.status='Disabilita selezionati';return UserSelezionati();")
        Me.LKBdisabilita.Attributes.Add("onmouseover", "window.status='Disabilita selezionati';return true;")
        Me.LKBdisabilita.Attributes.Add("onfocus", "window.status='Disabilita selezionati';return true;")
        Me.LKBdisabilita.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.LKBelimina.Attributes.Add("onclick", "window.status='Elimina selezionati';return UserForCancella();")
        Me.LKBelimina.Attributes.Add("onmouseover", "window.status='Elimina selezionati';return true;")
        Me.LKBelimina.Attributes.Add("onfocus", "window.status='Elimina selezionati';return true;")
        Me.LKBelimina.Attributes.Add("onmouseout", "window.status='';return true;")

        Dim TPRL_id, CMNT_id As Integer
        TPRL_id = Me.DDLTipoRuolo.SelectedValue
        CMNT_id = Session("idComunita_forAdmin")
        Dim iLink As String = "./../comunita/stampaiscritti.aspx?TPRL_id=" & TPRL_id & "&CMNT_id=" & CMNT_id
        Me.IMBstampa.Attributes.Add("onClick", "OpenWin('" & iLink & "','700','600','yes','yes');window.status='Stampa le lezioni visualizzate.';return false;")
        Me.IMBstampa.Attributes.Add("onmouseout", "window.status='';return true;")
        Me.IMBstampa.Attributes.Add("onfocus", "window.status='Stampa le lezioni visualizzate.';return true;")
        Me.IMBstampa.Attributes.Add("onmouseover", "window.status='Stampa le lezioni visualizzate.';return true;")
    End Sub

#Region "Bind_Dati"
    Public Sub Pre_Bind(Optional ByVal ricalcola As Boolean = False, Optional ByVal index As Integer = 0)
        Me.DGiscritti.CurrentPageIndex = index

        If Me.TBSmenu.SelectedTab Is Nothing Then
            Bind_Griglia(Main.FiltroAbilitazione.Tutti, ricalcola)
        ElseIf Me.TBSmenu.SelectedTab.Value = "TABtutti" Then
            Bind_Griglia(Main.FiltroAbilitazione.Tutti, ricalcola)
        ElseIf Me.TBSmenu.SelectedTab.Value = "TABlast" Then
            Bind_Griglia(Main.FiltroAbilitazione.TuttiUltimiIscritti, ricalcola)
        ElseIf Me.TBSmenu.SelectedTab.Value = "TABbloccati" Then
            Bind_Griglia(Main.FiltroAbilitazione.NonAbilitatoAttivato, ricalcola)
        ElseIf Me.TBSmenu.SelectedTab.Value = "TABinAttesa" Then
            Bind_Griglia(Main.FiltroAbilitazione.NonAttivato, ricalcola)
        ElseIf Me.TBSmenu.SelectedTab.Value = "TABabilitati" Then
            Bind_Griglia(Main.FiltroAbilitazione.AttivatoAbilitato, ricalcola)
        End If

    End Sub
    Public Sub Bind_Griglia(ByVal oFiltro As Main.FiltroAbilitazione, Optional ByVal ricalcola As Boolean = False)
        'allora:normalmente carica le persone con l'abilitazione o attivazione, e li mette nei campi nascosti
        'se c'è stata la paginazione ceccha l'abilitazione in base ai campi nascosti
        'Dim oComunita As New COL_Comunita
        'If Request.QueryString("CMNT_id") Is Nothing Then
        '    Dim IdComunita As Integer = Session("IdComunita")
        '    oComunita.Id = IdComunita
        'Else
        '    oComunita.Id = Request.QueryString("CMNT_id")

        'End If
        'Dim IdComunita As Integer = Session("IdComunita")
        'oComunita.Id = IdComunita
        Dim dsTable As New DataSet
        Try

            dsTable = FiltraggioDati(oFiltro, ricalcola)
            dsTable.Tables(0).Columns.Add(New DataColumn("oCheck"))
            Dim i, totale As Integer

            totale = dsTable.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGiscritti.Visible = False
                LBnoIscritti.Visible = True
                Me.MostraLinkbutton(False)
                LBnoIscritti.Text = "Nessun utente in questa categoria"
            Else
                Me.MostraLinkbutton()
                dsTable.Tables(0).Columns.Add(New DataColumn("oIscrittoIl"))
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = dsTable.Tables(0).Rows(i)

                    If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                        If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                            oRow.Item("oIscrittoIl") = "--"
                        Else
                            oRow.Item("oIscrittoIl") = FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                        End If
                    Else
                        oRow.Item("oIscrittoIl") = "--"
                    End If
                    If viewstate("Paginazione") <> "si" Then

                    Else

                        Dim t As Integer
                        Dim selezionato() As String
                        selezionato = Me.HDazione.Value.Split(",")
                        For t = 1 To selezionato.Length - 2
                            If oRow.Item("RLPC_id") = selezionato(t) Then
                                oRow.Item("oCheck") = "checked"
                                Exit For
                            End If
                        Next

                    End If
                Next
                Me.DGiscritti.VirtualItemCount = dsTable.Tables(0).Rows(0).Item("Totale")
                If totale > 0 Then
                    Mod_Visualizzazione(totale - 1)

                    Me.DGiscritti.Visible = True

                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_Anagrafica"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    Me.DGiscritti.DataSource = oDataview
                    Me.DGiscritti.DataBind()
                    LBnoIscritti.Visible = False
                Else
                    Me.DGiscritti.Visible = False
                    LBnoIscritti.Visible = True
                    Me.MostraLinkbutton(False)
                    LBnoIscritti.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."

                End If
            End If
        Catch ex As Exception
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
            Me.MostraLinkbutton(False)
            LBnoIscritti.Text = "Nessun utente in questa categoria"
        End Try
    End Sub
    Public Sub Bind_TipoRuolo()
        Me.DDLruolo.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita
            If Session("idComunita_forAdmin") Is Nothing Then
                Me.PNLcontenuto.Visible = False
                Me.PNLnoquery.Visible = True
                Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
            Else
                oComunita.Id = Session("idComunita_forAdmin")

            End If

            '  oComunita.Id = Session("IdComunita")
            oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForTipoComunita_NoGuest)

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
					Me.DDLruolo.Items.Add(oListItem)
				Next
				Me.BTNmodifica.Enabled = True
			Else
				Me.DDLruolo.Items.Add(New ListItem("< nessun ruolo >", -1))
				Me.BTNmodifica.Enabled = False
			End If
		Catch ex As Exception
			Me.DDLruolo.Items.Add(New ListItem("< nessun ruolo >", -1))
			Me.BTNmodifica.Enabled = False
		End Try
	End Sub
	Public Sub Bind_TipoRuoloFiltro()

		Me.DDLTipoRuolo.Items.Clear()
		Try
			Dim oDataset As DataSet
			Dim i, Totale As Integer
			Dim oComunita As New COL_Comunita
			If Session("idComunita_forAdmin") Is Nothing Then
				Me.PNLcontenuto.Visible = False
				Me.PNLnoquery.Visible = True
				Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
			Else
				oComunita.Id = Session("idComunita_forAdmin")
			End If

			'  oComunita.Id = Session("IdComunita")
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
				Me.BTNmodifica.Enabled = True
			Else
				Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
				Me.BTNmodifica.Enabled = False
			End If
		Catch ex As Exception
			Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
			Me.BTNmodifica.Enabled = False
		End Try
	End Sub
#End Region

#Region "Gestione Griglia"

    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGiscritti.SortCommand

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
        'bind datagrid
        Pre_Bind()
    End Sub

    Sub DGiscritti_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs)
        'paginazione della datagrid iscritti
        Me.ViewState("intCurPage") = e.NewPageIndex
        viewstate("Paginazione") = "si"
        'bind datagrid
        Pre_Bind(False, e.NewPageIndex)
    End Sub

    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGiscritti.ItemCreated
        Dim i As Integer

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")
            If oSortDirection = "asc" Then
                oText = " 5"
            Else
                oText = " 6"
            End If
            For i = 0 To sender.columns.count - 1
                If sender.columns(i).visible = True And sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabel = New System.Web.UI.WebControls.Label

                    oCell = e.Item.Cells(i)
                    If oSortExspression = sender.columns(i).SortExpression Then
                        oLabel.text = oText
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                            If oSortDirection = "asc" Then
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento decrescentep per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                            Else
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                            End If

                            oLinkbutton.Font.Name = "webdings"
                            oLinkbutton.Font.Size = FontUnit.XSmall
                            oLabel.text = oLinkbutton.Text & " "
                            oLinkbutton.Text = oText
                            oCell.Controls.AddAt(0, oLabel)
                        Catch ex As Exception
                            oLabel.font.name = "webdings"
                            oLabel.font.size = FontUnit.XSmall
                            oLabel.text = "&nbsp;&nbsp;"
                            oCell.Controls.Add(oLabel)
                        End Try
                    Else
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl


                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oLinkbutton.Attributes.Add("onfocus", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onclick", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                        Catch ex As Exception

                        End Try
                        oLabel.font.name = "webdings"
                        oLabel.font.size = FontUnit.XSmall
                        oLabel.text = "&nbsp;&nbsp;"
                        oCell.Controls.Add(oLabel)
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
                    oWebControl.CssClass = "PagerLink"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "PagerSpan"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "PagerLink"
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Try
                If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                    e.Item.CssClass = "Righe_Disattivate"
                ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
                    e.Item.CssClass = "Righe_Disabilitate"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "Righe_Alternate"
                Else
                    e.Item.CssClass = "Righe_Normali"
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "Righe_Alternate"
                Else
                End If
            End Try

            'responsabile [R]
            Try
                If CBool(e.Item.DataItem("RLPC_Responsabile")) = True Then
                    e.Item.DataItem("PRSN_Anagrafica") = e.Item.DataItem("PRSN_Anagrafica") & "[R]"

                End If
            Catch ex As Exception

            End Try

            ' Cancellazione iscrizione utente.........
            Dim oImagebutton As ImageButton
            Try
                oImagebutton = e.Item.Cells(0).FindControl("IMBCancella")
                If Not IsNothing(oImagebutton) Then
                    oImagebutton.Attributes.Add("onclick", "window.status='De-Iscrivi.';return confirm('Sicuro/a di voler cancellare l\'iscrizione della persona selezionata ?');")
                    oImagebutton.Attributes.Add("onfocus", "window.status='De-Iscrivi.';return true;")
                    oImagebutton.Attributes.Add("onmouseover", "window.status='De-Iscrivi.';return true;")
                    oImagebutton.Attributes.Add("onmouseout", "window.status='';return true;")

                    oImagebutton.ToolTip = "Cancella iscrizione"
                End If
            Catch ex As Exception

            End Try

            'bottone informazioni
            oImagebutton = Nothing

            Dim Cell As New TableCell
            Dim TPPR_id As Integer
            Dim PRSN_ID As Integer

            Try
                PRSN_ID = e.Item.DataItem("PRSN_id")
                TPPR_id = e.Item.DataItem("PRSN_TPPR_id")
                Dim i_link2 As String
                i_link2 = "./../Admin/ADM_InfoPersona.aspx?TPPR_ID=" & TPPR_id & "&PRSN_ID=" & PRSN_ID
                Cell = CType(e.Item.Cells(0), TableCell)

                oImagebutton = Cell.FindControl("IMBinfo")
                'in base al tipo di utente decido la dimensione della finestra di popup
                Select Case TPPR_id
                    Case Main.TipoPersonaStandard.Studente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','540','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Docente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','590','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Tutor
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','600','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Esterno
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','520','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Amministrativo
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.SysAdmin
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Copisteria
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.DocenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','620','no','yes');return false;")
                    Case Main.TipoPersonaStandard.StudenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','610','no','yes');return false;")
                    Case Else
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','590','no','yes');return false;")
                End Select
                oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub DGiscritti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGiscritti.ItemCommand

        If e.CommandName = "modifica" Then
            Session("azione") = "modifica"
            Me.PNLmodifica.Visible = True
            Me.HDrlpc.Value = source.Items(e.Item.ItemIndex).Cells(1).Text()
            Me.HDNprsnID.Value = source.Items(e.Item.ItemIndex).Cells(12).Text()
            Me.HDNrlpc_Attivato.Value = source.Items(e.Item.ItemIndex).Cells(13).Text()
            Me.HDNrlpc_Abilitato.Value = source.Items(e.Item.ItemIndex).Cells(14).Text()
            Me.CHBresponsabile.Checked = CBool(source.Items(e.Item.ItemIndex).Cells(15).Text())
            If Me.CHBresponsabile.Checked = True Then
                Me.CHBresponsabile.Enabled = False
            Else
                Me.CHBresponsabile.Enabled = True
            End If

            Me.LBNomeCognome.Text = source.Items(e.Item.ItemIndex).Cells(5).Text()
            Me.DDLruolo.SelectedValue = source.Items(e.Item.ItemIndex).Cells(7).text
            'bind datagrid
            Pre_Bind()

            Me.MostraLinkbutton(False)

            'PROBLEMA DELLE SOTTO COMUNITA' --> devo disiscrivere a cascata l'utente dalle sotto-comunità

        ElseIf e.CommandName = "deiscrivi" Then
            Session("azione") = "deiscrivi"
            Try
                Me.HDNprsn_Id.Value = source.Items(e.Item.ItemIndex).Cells(12).text
                Me.HDNcmnt_ID.Value = Session("idComunita_forAdmin")
                Me.HDNcmnt_Path.Value = Session("CMNT_path_forAdmin")
                If Me.HDNcmnt_Path.Value = "" Then
                    Me.HDNcmnt_Path.Value = "."
                End If
                Me.DeIscrivi(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, Me.HDNprsn_Id.Value)

                If Me.LKBcancellaInAttesa.Visible = False Then
                    Me.DeIscrivi(Me.HDNcmnt_ID.Value, ".", Me.HDNprsn_Id.Value)
                Else
                    Dim oComunita As New COL_Comunita
                    oComunita.Id = Session("idComunita_forAdmin")
                    oComunita.EliminaUtentiInAttesa("," & source.Items(e.Item.ItemIndex).Cells(12).text & ",")
                    If oComunita.Errore = Errori_Db.None Then
                        Try
                            Me.AggiornaProfiloXML(Session("idComunita_forAdmin"), source.Items(e.Item.ItemIndex).Cells(12).text, Me.HDNcmnt_Path.Value)
                        Catch ex As Exception

                        End Try
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.DGiscritti.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        viewstate("Paginazione") = "si"
        'bind della datagrid
        Pre_Bind()
    End Sub

    Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
        If oRecord > Me.DGiscritti.PageSize Or oRecord > Me.DDLNumeroRecord.Items(0).Value Or Me.DGiscritti.VirtualItemCount > Me.DGiscritti.PageSize Then
            Me.DGiscritti.AllowPaging = True
            Me.DGiscritti.PageSize = Me.DDLNumeroRecord.SelectedItem.Value
        Else
            Me.DGiscritti.AllowPaging = False

        End If
        If oRecord < 0 Then
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
            MostraLinkbutton(False)
            LBnoIscritti.Text = "Nessun utente in questa categoria"
        End If
    End Sub
#End Region

#Region "Menu_Orizzontali"
    Public Sub MostraLinkbutton(Optional ByVal record As Boolean = True)
        If record = True AndAlso Not IsNothing(TBSmenu.SelectedTab) Then
            If TBSmenu.SelectedTab.Value = "TABtutti" OrElse TBSmenu.SelectedTab.Value = "TABlast" Then
                Me.LKBabilita.Visible = True
                Me.LKBdisabilita.Visible = True
                Me.LKBelimina.Visible = False 'per adesso
                Me.LKBcancellaInAttesa.Visible = False
                Me.LBseleziona.Visible = True
            ElseIf TBSmenu.SelectedTab.Value = "TABbloccati" Then
                Me.LKBabilita.Visible = True
                Me.LKBdisabilita.Visible = False
                Me.LKBelimina.Visible = False 'per adesso
                Me.LKBcancellaInAttesa.Visible = False
                Me.LBseleziona.Visible = True
            ElseIf TBSmenu.SelectedTab.Value = "TABinAttesa" Then
                Me.LKBabilita.Visible = True
                Me.LKBdisabilita.Visible = False
                Me.LKBelimina.Visible = False 'per adesso
                Me.LKBcancellaInAttesa.Visible = True
                Me.LBseleziona.Visible = True
            ElseIf TBSmenu.SelectedTab.Value = "TABabilitati" Then
                Me.LKBabilita.Visible = False
                Me.LKBdisabilita.Visible = True
                Me.LKBelimina.Visible = False 'per adesso
                Me.LKBcancellaInAttesa.Visible = False
                Me.LBseleziona.Visible = True
            End If
        Else
            Me.LKBabilita.Visible = False
            Me.LKBdisabilita.Visible = False
            Me.LKBelimina.Visible = False 'per adesso
            Me.LKBcancellaInAttesa.Visible = False
            Me.LBseleziona.Visible = False
        End If


    End Sub

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Dim toListaComunita, toRicercaByPersona As Boolean

        toListaComunita = False
        toRicercaByPersona = False
        If Request.QueryString("FROM") = "RicercaComunita" Then
            toListaComunita = True
        ElseIf Request.QueryString("FROM") = "ricercabypersona" Then
            toRicercaByPersona = True
        End If

        Select Case Me.TBSmenu.SelectedIndex
            Case 0
                'sono da mettere gli altri else che si faranno
                If toListaComunita Then
                    Response.Redirect("AdminG_ListaComunita.aspx?from=RicercaComunita", True)
                ElseIf toRicercaByPersona Then
                    Response.Redirect("AdminG_RicercaComunita.aspx?from=ricercabypersona", True)
                Else
                    Response.Redirect("AdminG_ListaComunita.aspx", True)
                End If
                Exit Sub
            Case 1
                Me.Bind_UltimiIscritti()
            Case 2
                Me.Bind_Tutti()
            Case 3
                Me.Bind_Abilitati()
            Case 4
                Me.Bind_Bloccati()
            Case 5
                Me.Bind_inAttesa()
            Case 6
                Me.Response.Redirect("./AdminG_AggiungiUtente.aspx?from=" & Request.QueryString("FROM"), True)
        End Select
    End Sub

    Private Sub Bind_UltimiIscritti()
        Session("azione") = "loaded"
        Me.DGiscritti.CurrentPageIndex = 0
        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""

        viewstate("SortExspression") = "rlpc_iscrittoil"
        viewstate("SortDirection") = "desc"

        Bind_Griglia(Main.FiltroAbilitazione.TuttiUltimiIscritti, True)
        Me.DGiscritti.Columns(16).Visible = True
        Me.DGiscritti.Columns(10).Visible = True
        Me.PNLmessaggio.Visible = False
        Me.PNLmodifica.Visible = False
    End Sub

    Public Sub Bind_Tutti()
        Session("azione") = "loaded"
        Me.DGiscritti.CurrentPageIndex = 0
        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        viewstate("SortExspression") = "PRSN_Anagrafica"
        viewstate("SortDirection") = ""

        Bind_Griglia(Main.FiltroAbilitazione.Tutti, True)

        Me.DGiscritti.Columns(16).Visible = False
        Me.DGiscritti.Columns(10).Visible = True
        Me.DGiscritti.Columns(3).Visible = True  'modifica
        Me.DGiscritti.Columns(2).Visible = True  'cancella
        Me.PNLmessaggio.Visible = False
        Me.PNLmodifica.Visible = False
    End Sub
    Public Sub Bind_Abilitati()
        Session("azione") = "loaded"
        Me.DGiscritti.CurrentPageIndex = 0
        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        viewstate("SortExspression") = "PRSN_Anagrafica"
        viewstate("SortDirection") = ""

        Bind_Griglia(Main.FiltroAbilitazione.AttivatoAbilitato, True)



        Me.DGiscritti.Columns(16).Visible = False
        Me.DGiscritti.Columns(10).Visible = True
        Me.DGiscritti.Columns(3).Visible = True  'modifica
        Me.DGiscritti.Columns(2).Visible = True  'cancella
        Me.PNLmessaggio.Visible = False
        Me.PNLmodifica.Visible = False
    End Sub
    Public Sub Bind_Bloccati()
        Session("azione") = "loaded"
        Me.DGiscritti.CurrentPageIndex = 0
        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        viewstate("SortExspression") = "PRSN_Anagrafica"
        viewstate("SortDirection") = ""


        Bind_Griglia(Main.FiltroAbilitazione.NonAbilitatoAttivato, True)
        Me.DGiscritti.Columns(16).Visible = False
        Me.DGiscritti.Columns(10).Visible = True
        Me.DGiscritti.Columns(3).Visible = True  'modifica
        Me.DGiscritti.Columns(2).Visible = True  'cancella
        Me.PNLmessaggio.Visible = False
        Me.PNLmodifica.Visible = False
    End Sub
    Public Sub Bind_inAttesa()
        Session("azione") = "loaded"
        Me.DGiscritti.CurrentPageIndex = 0
        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        viewstate("SortExspression") = "PRSN_Anagrafica"
        viewstate("SortDirection") = ""
        Me.LKBcancellaInAttesa.Visible = True

        Bind_Griglia(Main.FiltroAbilitazione.NonAttivato, True)
        Me.DGiscritti.Columns(16).Visible = True
        Me.DGiscritti.Columns(10).Visible = False
        Me.DGiscritti.Columns(3).Visible = False  'modifica
        Me.DGiscritti.Columns(2).Visible = True  'cancella
        Me.PNLmessaggio.Visible = False
        Me.PNLmodifica.Visible = False
    End Sub

#End Region

    Private Sub LKBcancellaInAttesa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBcancellaInAttesa.Click
        Try
            Dim CMNT_ID, i As Integer
            Dim ElencoID() As String
            If Session("idComunita_forAdmin") Is Nothing Then
                Me.PNLcontenuto.Visible = False
                Me.PNLnoquery.Visible = True
                Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
            Else
                Dim oComunita As New COL_Comunita
                CMNT_ID = Session("idComunita_forAdmin")

                oComunita.Id = CMNT_ID
                oComunita.EliminaUtentiInAttesa(Me.HDazione.Value)
                If oComunita.Errore = Errori_Db.None Then
                    ElencoID = Me.HDazione.Value.Split(",")
                    For i = 0 To UBound(ElencoID)
                        If IsNumeric(ElencoID(i)) Then
                            Try
                                Me.AggiornaProfiloXML(CMNT_ID, ElencoID(i), "." & CMNT_ID & ".")
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                End If
                Me.HDazione.Value = ","
            End If
        Catch ex As Exception

        End Try
        'bind della datagrid
        Pre_Bind()
    End Sub

#Region "Gestione iscritti"

    Private Sub DisabilitaIscritti(ByVal ElencoIscritti As String, ByVal CMNT_ID As Integer)
        Dim oTreeComunita As New COL_TreeComunita
        Dim oComunita As New COL_Comunita
        Dim PRSN_ID, TPRL_ID, i, totale As Integer
        Dim ElencoID() As String

        ElencoID = ElencoIscritti.Split(",")

        'salvo le modifiche su file xml
        For i = 0 To UBound(ElencoID)
            Try
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)

                    oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
                    oTreeComunita.Nome = PRSN_ID & ".xml"
                    If Session("idComunita_forAdmin") Is Nothing Then
                        Me.PNLcontenuto.Visible = False
                        Me.PNLnoquery.Visible = True
                        Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
                    Else
                        oTreeComunita.CambiaAbilitazione(Session("idComunita_forAdmin"), False)
                    End If
                    'oTreeComunita.CambiaAbilitazione(Session("idComunita"), False)
                End If
            Catch ex As Exception

            End Try
        Next
        'salvo le modifiche su db
        ' ElencoIscritti = ElencoIscritti.Replace(",", " ")
        oComunita.Id = CMNT_ID
        oComunita.DisabilitaIscritti(ElencoIscritti)
    End Sub

    Private Sub AbilitaIscritti(ByVal ElencoIscritti As String, ByVal CMNT_ID As Integer)
        Dim oTreeComunita As New COL_TreeComunita
        Dim oComunita As New COL_Comunita
        Dim PRSN_ID, TPRL_ID, i, totale As Integer
        Dim ElencoID() As String

        ElencoID = ElencoIscritti.Split(",")

        'salvo le modifiche su file xml
        For i = 0 To UBound(ElencoID)
            Try
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)

                    oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
                    oTreeComunita.Nome = PRSN_ID & ".xml"
                    If Session("idComunita_forAdmin") Is Nothing Then
                        Me.PNLcontenuto.Visible = False
                        Me.PNLnoquery.Visible = True
                        Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
                    Else
                        oTreeComunita.CambiaAbilitazione(Session("idComunita_forAdmin"), True)
                    End If
                    '  oTreeComunita.CambiaAbilitazione(Session("idComunita"), True)
                End If
            Catch ex As Exception

            End Try
        Next
        'salvo le modifiche su db
        '  ElencoIscritti = ElencoIscritti.Replace(",", " ")
        oComunita.Id = CMNT_ID
        oComunita.AbilitaIscritti(ElencoIscritti)
    End Sub

    Private Sub AttivaIscritti(ByVal ElencoIscritti As String, ByVal CMNT_ID As Integer)
        Dim oTreeComunita As New COL_TreeComunita
        Dim oComunita As New COL_Comunita
        Dim PRSN_ID, TPRL_ID, i, totale As Integer
        Dim ElencoID() As String

        ElencoID = ElencoIscritti.Split(",")

        'salvo le modifiche su file xml
        For i = 0 To UBound(ElencoID)
            Try
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)

                    oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
                    oTreeComunita.Nome = PRSN_ID & ".xml"
                    If Session("idComunita_forAdmin") Is Nothing Then
                        Me.PNLcontenuto.Visible = False
                        Me.PNLnoquery.Visible = True
                        Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
                    Else
                        Dim oPersona As New COL_Persona
						Dim oUtility As New OLDpageUtility(Me.Context)
                        oComunita.IdPadre = Session("idComunita_forAdmin")
                        oPersona.Id = PRSN_ID
                        oPersona.Estrai(Session("LinguaID"))
                        '###################################################################
                        '###################################################################
                        '###################################################################
                        '###################################################################

                        '                   MANCA INTERNAZIONALIZZAZIONE
                        If oPersona.Errore = Errori_Db.None Then

                        Else

                        End If
                        oTreeComunita.CambiaAttivazione(Session("idComunita_forAdmin"), True, Nothing)


                        '###################################################################
                        '###################################################################
                        '###################################################################
                        '###################################################################

						oComunita.MailAccettazione(oPersona, oUtility.LocalizedMail)
                    End If
                    ' oTreeComunita.CambiaAttivazione(Session("idComunita"), True)
                End If
            Catch ex As Exception

            End Try
        Next

        'salvo le modifiche su db
        ' ElencoIscritti = ElencoIscritti.Replace(",", " ")
        oComunita.Id = CMNT_ID
        oComunita.AttivaIscritti(ElencoIscritti)
    End Sub

#End Region

#Region "Modifica Ruolo"

    Private Sub BTNmodifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNmodifica.Click

        'cambia il ruolo alla persona
        If Session("azione") = "modifica" Then
            Try
                Dim PRSN_ID, TPRL_ID As Integer
                Dim isAbilitato, isAttivato, isResponsabile As Boolean
                Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

                TPRL_ID = Me.DDLruolo.SelectedItem.Value
                oRuoloPersonaComunita.Id = Me.HDrlpc.Value
                oRuoloPersonaComunita.TipoRuolo.Id = TPRL_ID
                oRuoloPersonaComunita.isResponsabile = CHBresponsabile.Checked
                oRuoloPersonaComunita.CambiaRuolo(Me.HDNprsnID.Value)

                If CBool(Me.HDNrlpc_Attivato.Value) Then
                    isAttivato = CBool(Me.HDNrlpc_Attivato.Value)
                Else
                    isAttivato = False
                End If
                If CBool(Me.HDNrlpc_Abilitato.Value) Then
                    isAbilitato = CBool(Me.HDNrlpc_Abilitato.Value)
                Else
                    isAbilitato = False
                End If
                If Me.CHBresponsabile.Checked = True Then
                    isResponsabile = True
                Else
                    isResponsabile = False
                End If
                PRSN_ID = Me.HDNprsnID.Value

                If Session("idComunita_forAdmin") Is Nothing Then
                    Me.PNLcontenuto.Visible = False
                    Me.PNLnoquery.Visible = True
                    Me.LBnoquery.Text = "Errore non grave di posizionamento riprova"
                End If
            Catch ex As Exception

            End Try
            Session("azione") = "loaded"
        End If
        Me.PNLmodifica.Visible = False

        Pre_Bind()
    End Sub

    Private Sub BTNannulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNannulla.Click
        Me.PNLiscritti.Visible = True
        Me.PNLmodifica.Visible = False
        Me.MostraLinkbutton(True)

        Pre_Bind()
        Session("azione") = "loaded"
    End Sub

#End Region

#Region "filtri vari"
    Private Function FiltraggioDati(ByVal oFiltro As Main.FiltroAbilitazione, Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim oPersona As New COL_Persona
            Dim Valore As String
            oPersona = Session("objPersona")

            Dim oComunita As New COL_Comunita

            oComunita.Id = Session("idComunita_forAdmin")

            'Dim CMNT_ID As Integer = Session("IdComunita")
            'oComunita.Id = CMNT_ID

            Dim TPRL_id As Integer
            TPRL_id = Me.DDLTipoRuolo.SelectedValue

            Dim oFiltroCampoOrdine As COL_Comunita.FiltroCampoOrdine
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento

            Dim oFiltroAnagrafica As Main.FiltroAnagrafica
            Dim oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti

            If Me.TXBValore.Text <> "" Then
                Valore = Trim(Me.TXBValore.Text)
            End If
            Try
                If IsNothing(Me.ViewState("intAnagrafica")) Then
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Else
                    oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                Me.ViewState("intAnagrafica") = -1
            End Try


            Try
                If Valore <> "" Then
                    Select Case Me.DDLTipoRicerca.SelectedItem.Value
                        Case Main.FiltroRicercaAnagrafica.nome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nome
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                        Case Main.FiltroRicercaAnagrafica.cognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.cognome

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.nomeCognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nomeCognome
                        Case Main.FiltroRicercaAnagrafica.matricola
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.matricola

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            Try
                                If IsDate(Valore) Then
                                    Valore = Main.DateToString(Valore, False)
                                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                                End If
                            Catch ex As Exception

                            End Try
                        Case Main.FiltroRicercaAnagrafica.login
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.login

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Else
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                    End Select
                Else
                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try

            If viewstate("SortExspression") = "" Or LCase(viewstate("SortExspression")) = "prsn_anagrafica" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
            ElseIf LCase(viewstate("SortExspression")) = "prsn_datanascita" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataNascita
            ElseIf LCase(viewstate("SortExspression")) = "tprl_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoRuolo
            ElseIf LCase(viewstate("SortExspression")) = "tppr_descrizione" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoPersona
            ElseIf LCase(viewstate("SortExspression")) = "prsn_login" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.login
            Else
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
            End If

            Dim ordinamento As Integer
            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            Dim totale As Decimal
            If ricalcola Then
                Me.ViewState("intCurPage") = 0
                Me.DGiscritti.CurrentPageIndex = 0
            End If
            Return oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), oPersona.Id, oFiltro, Main.FiltroUtenti.NoPassantiNoCreatori, TPRL_id, Me.DGiscritti.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroAnagrafica, oFiltroOrdinamento, oFiltroCampoOrdine, , oFiltroRicerca)

        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Pre_Bind(True)
    End Sub

    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        Pre_Bind(True)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
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
        Me.DGiscritti.CurrentPageIndex = 0
        Pre_Bind(True)
    End Sub

    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        Pre_Bind(True)
    End Sub
#End Region

    Private Sub LKBabilita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBabilita.Click
        'aggiorna la ditagrid dopo avere fatto le modifiche con le checkbox
        ' Dim oComunita As New COL_Comunita
        Dim CMNT_ID As Integer
        If Session("idComunita_forAdmin") Is Nothing Then
            Me.PNLcontenuto.Visible = False
            Me.PNLnoquery.Visible = True
            Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
        Else
            CMNT_ID = Session("idComunita_forAdmin")
        End If
        'Dim CMNT_ID As Integer = Session("IdComunita")
        Dim i, totale As Integer
        Dim ElencoID() As String

        Dim Selezionato As String
        '        If Equals(Me.LKBtuttiIscritti.BackColor, Me.LKBtuttiIscritti.BackColor.Yellow) Or Equals(Me.LKBbloccati.BackColor, Me.LKBbloccati.BackColor.Yellow) Then
        If Me.TBSmenu.SelectedIndex = 1 Or Me.TBSmenu.SelectedIndex = 2 Or Me.TBSmenu.SelectedIndex = 4 Then
            If Me.HDazione.Value <> "," Then
                Me.AbilitaIscritti(Me.HDazione.Value, CMNT_ID)
            End If

        ElseIf Me.TBSmenu.SelectedIndex = 5 Then

            If Me.HDazione.Value <> "," Then
                Me.AttivaIscritti(Me.HDazione.Value, CMNT_ID)
            End If
        End If

        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        Pre_Bind()
    End Sub
    Private Sub LKBelimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBelimina.Click
        Try
            Me.HDNelencoID.Value = Me.HDazione.Value
            Me.LBinfoDeIscrivi_multiplo.Text = "Sicuro di voler cancellare l'iscrizione degli utenti selezionati ? <br>" _
                & " utilizzare i pulsanti sottostanti per annullare l'operazione, per cancellare l'iscrizione alla sola comunità corrente o " _
                & " per cancellare l'iscrizione anche alle relative sottocomunità."

            Me.PNLdeiscriviMultiplo.Visible = True
            Me.PNLcontenuto.Visible = False
            Pre_Bind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LKBdisabilita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBdisabilita.Click
        Dim CMNT_ID As Integer
        If Session("idComunita_forAdmin") Is Nothing Then
            Me.PNLcontenuto.Visible = False
            Me.PNLnoquery.Visible = True
            Me.LBnoquery.Text = "Spiacenti, Si è verificato un errore, riprova"
        Else
            CMNT_ID = Session("idComunita_forAdmin")
        End If
        Dim i, totale As Integer
        Dim ElencoID() As String

        Dim Selezionato As String
        If Me.TBSmenu.SelectedIndex = 1 Or Me.TBSmenu.SelectedIndex = 2 Or Me.TBSmenu.SelectedIndex = 3 Then
            If Me.HDazione.Value <> "," Then
                Me.DisabilitaIscritti(Me.HDazione.Value, CMNT_ID)
            End If
        End If

        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        Pre_Bind()
    End Sub

    Private Sub BTNlistacmnt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlistacmnt.Click
        Response.Redirect("./adming_listacomunita.aspx")
    End Sub
    Private Sub BTNricercacmnt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNricercacmnt.Click
        Response.Redirect("./adming_ricercacomunita.aspx")
    End Sub

#Region "Gestione DeIscrizione"
    Private Sub ResetData____()
        'If Equals(LKBtuttiIscritti.BackColor, System.Drawing.Color.Yellow) Then

        'ElseIf Equals(LKBtuttiIscritti.BackColor, System.Drawing.Color.Yellow) Then

        'ElseIf Equals(LKBabilitati.BackColor, System.Drawing.Color.Yellow) Then

        'ElseIf Equals(LKBbloccati.BackColor, System.Drawing.Color.Yellow) Then
        '    Me.Bloccati()
        'ElseIf Equals(LKBinattesa.BackColor, System.Drawing.Color.Yellow) Then
        '    Me.inAttesa()
        'Else
        '    Me.MostraAbilitati()
        'End If
    End Sub

    Private Sub DeIscrivi(ByVal CMNT_ID As Integer, ByVal CMNT_Path As String, ByVal PRSN_ID As Integer)
        Dim oComunita As New COL_Comunita
        Dim i, totale As Integer
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Try
            oPersona.Id = PRSN_ID
            oPersona.Estrai(Session("LinguaID"))

            If Session("azione") = "deiscrivi" Then
                Try
                    Dim multipli As Boolean = False


                    oComunita.Id = CMNT_ID
                    Try
                        Dim oDataview As DataView
                        Dim hasCreatori, HasPassanti, HasOther As Boolean

                        oDataset = oPersona.ElencaComunitaDaDeiscrivere(CMNT_ID, CMNT_Path)
                        totale = oDataset.Tables(0).Rows.Count
                        If totale > 1 Then
                            oDataview = oDataset.Tables(0).DefaultView
                            oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID

                            ' Esistono dei creatori
                            oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID & " and RLPC_TPRL_ID = -2"
                            hasCreatori = (oDataview.Count > 0)

                            'esistono dei passanti.....
                            oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID & " and RLPC_TPRL_ID = -3"
                            HasPassanti = (oDataview.Count > 0)

                            'altri....
                            oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID & " and RLPC_TPRL_ID <> -3 and RLPC_TPRL_ID <> -2"
                            HasOther = (oDataview.Count > 0)

                            If Not HasOther And Not hasCreatori And HasPassanti Then
                                'Ho sotto comunità solo come passante......
                                multipli = False
                            ElseIf Not HasOther And hasCreatori And Not HasPassanti Then
                                'Ho sotto comunità solo come creatore......
                                multipli = False
                            Else
                                multipli = True
                            End If
                        Else
                            multipli = False
                        End If

                        If multipli Then
                            Dim j As Integer
                            Dim ordinamento, nomeComunita As String


                            Me.PNLdeiscrivi.Visible = True
                            Me.PNLcontenuto.Visible = False

                            Me.HDNcmnt_ID.Value = CMNT_ID
                            Me.HDNcmnt_Path.Value = CMNT_Path


                            oDataview.Sort = "CMNT_PATH,CMNT_Nome"

                            totale = oDataview.Count - 1
                            For i = 0 To totale
                                Dim oRow As DataRow
                                Dim oTableRow As New TableRow
                                Dim oCell As New TableCell
                                Dim oImage As New System.Web.UI.WebControls.Image

                                oRow = oDataview.Item(i).Row

                                oImage.AlternateText = ""
                                oImage.ToolTip = oRow.Item("TPCM_Descrizione")
                                oImage.ImageUrl = "./../" & oRow.Item("TPCM_Icona")
                                oCell.Controls.Add(oImage)
                                oTableRow.Cells.Add(oCell)


                                oCell = New TableCell
                                nomeComunita = oRow.Item("CMNT_nome")
                                If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                                    nomeComunita = nomeComunita & " (Creata da: " & oRow.Item("AnagraficaCreatore") & ") "
                                ElseIf oRow.Item("CMNT_Responsabile") = "" Then
                                    nomeComunita = nomeComunita & " (Creata da: " & oRow.Item("AnagraficaCreatore") & ") "
                                Else
                                    nomeComunita = nomeComunita & " (" & oRow.Item("CMNT_Responsabile") & ") "
                                End If
                                oCell.Text = nomeComunita
                                oTableRow.Cells.Add(oCell)

                                Me.TBLcomunita.Rows.Add(oTableRow)
                            Next

                            oDataview.RowFilter = "CMNT_ID = " & CMNT_ID

                            Me.LBinfoDeIscrivi.Text = "La persona selezionata (<b>" & oPersona.Cognome & " " & oPersona.Nome & "</b>) "
                            Me.LBinfoDeIscrivi.Text = Me.LBinfoDeIscrivi.Text & " di cui ui vuole annullare l'iscrizione, è iscritta ad altre sottocomunità, desidera cancellare l'iscrizione anche alle comunità elencate qui sotto ?"
                        Else
                            oPersona.DeIscriviFromComunita(CMNT_ID, CMNT_Path, False)
                            If oPersona.Errore = Errori_Db.None Then
                                Me.AggiornaProfiloXML(CMNT_ID, oPersona.Id, CMNT_Path)
                            End If
                            Session("azione") = "loaded"
                            Me.MostraLinkbutton(True)

                            Pre_Bind()
                        End If
                    Catch ex As Exception

                    End Try

                Catch ex As Exception

                End Try
            Else
                Me.PNLdeiscrivi.Visible = False
                Me.PNLcontenuto.Visible = True
                Me.PNLiscritti.Visible = True
                Me.MostraLinkbutton(True)

                Pre_Bind()
                Session("azione") = "loaded"
            End If
        Catch ex As Exception
            Me.PNLdeiscrivi.Visible = False
            Me.PNLcontenuto.Visible = True
            Me.PNLiscritti.Visible = True
            Me.MostraLinkbutton(True)

            Pre_Bind()
            Session("azione") = "loaded"
        End Try

    End Sub

    Private Sub AggiornaProfiloXML(ByVal CMNT_ID As Integer, ByVal PRSN_Id As Integer, ByVal CMNT_Path As String)
        Dim oRuolo As New COL_RuoloPersonaComunita
        Dim oTreeComunita As New COL_TreeComunita

        Try
            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_Id)

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_Id & "\"
            oTreeComunita.Nome = PRSN_Id & ".xml"

            If oRuolo.Errore = Errori_Db.None Then

            Else
                oTreeComunita.Delete(CMNT_ID, CMNT_Path)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNdeIscriviCorrente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdeIscriviCorrente.Click
        Dim oPersona As New COL_Persona
        Dim i, totale As Integer
        Dim oDataset As New DataSet

        If Session("azione") = "deiscrivi" Then
            Try
                oPersona.Id = Me.HDNprsn_Id.Value
                oPersona.DeIscriviFromComunita(Me.HDNcmnt_ID.Value, ".", False)

                If oPersona.Errore = Errori_Db.None Then
                    Me.AggiornaProfiloXML(Me.HDNcmnt_ID.Value, oPersona.Id, Me.HDNcmnt_Path.Value)
                End If
            Catch ex As Exception

            End Try
        End If
        Session("azione") = "loaded"
        Me.HDNprsn_Id.Value = 0
        Me.HDNcmnt_ID.Value = 0
        Me.HDNcmnt_Path.Value = "."
        Me.PNLdeiscrivi.Visible = False
        Me.PNLcontenuto.Visible = True
        Me.Pre_Bind()
    End Sub
    Private Sub BTNdeIscriviTutte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdeIscriviTutte.Click
        Dim oPersona As New COL_Persona
        Dim i, totale As Integer
        Dim oDataset As New DataSet

        If Session("azione") = "deiscrivi" Then
            Try
                oPersona.Id = Me.HDNprsn_Id.Value
                oPersona.DeIscriviFromComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, True)
                If oPersona.Errore = Errori_Db.None Then
                    Me.AggiornaProfiloXML(Me.HDNcmnt_ID.Value, oPersona.Id, Me.HDNcmnt_Path.Value)
                End If
            Catch ex As Exception

            End Try
        End If
        Me.HDNprsn_Id.Value = 0
        Me.HDNcmnt_ID.Value = 0
        Me.HDNcmnt_Path.Value = "."
        Session("azione") = "loaded"
        Me.PNLdeiscrivi.Visible = False
        Me.PNLcontenuto.Visible = True
        Pre_Bind()
    End Sub
    Private Sub BTNannullaDeiscrizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNannullaDeiscrizione.Click
        Me.HDNprsn_Id.Value = 0
        Me.HDNcmnt_ID.Value = 0
        Me.HDNcmnt_Path.Value = "."
        Me.PNLdeiscrivi.Visible = False
        Me.PNLcontenuto.Visible = True
        Session("azione") = "loaded"
        Me.Pre_Bind()
    End Sub
#End Region

#Region "Deiscrizione multipla"
    Private Sub BTNdeIscriviTutte_multi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdeIscriviTutte_multi.Click
        Dim CMNT_ID, i, totale, PRSN_ID As Integer
        Dim ElencoID(), CMNT_Path As String


        Try
            CMNT_ID = Session("idComunita_forAdmin")
            CMNT_Path = Session("CMNT_path_forAdmin")
            ElencoID = Me.HDazione.Value.Split(",")
            Try
                Dim ArrComunita(,) As String = Session("ArrComunita")
                CMNT_Path = ArrComunita(2, UBound(ArrComunita, 2))
            Catch ex As Exception
                CMNT_Path = "."
            End Try

            Dim oPersona As New COL_Persona
            For i = 0 To UBound(ElencoID)
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)
                    Try
                        oPersona.Id = PRSN_ID
                        oPersona.DeIscriviFromComunita(CMNT_ID, CMNT_Path, True)
                        If oPersona.Errore = Errori_Db.None Then
                            Me.AggiornaProfiloXML(CMNT_ID, oPersona.Id, CMNT_Path)
                        End If
                    Catch ex As Exception

                    End Try
                End If
            Next

        Catch ex As Exception

        End Try

        Session("azione") = "loaded"
        Me.MostraLinkbutton(True)
        Me.PNLcontenuto.Visible = True
        Me.PNLdeiscriviMultiplo.Visible = False
        Me.Pre_Bind()
    End Sub
    Private Sub BTNdeIscriviCorrente_multi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdeIscriviCorrente_multi.Click
        Dim CMNT_ID, i, totale, PRSN_ID As Integer
        Dim ElencoID(), CMNT_Path As String

        If Request.QueryString("CMNT_id") Is Nothing Then
            CMNT_ID = Session("IdComunita")
        Else
            CMNT_ID = Request.QueryString("CMNT_id")
        End If

        Try
            ElencoID = Me.HDazione.Value.Split(",")
            Try
                Dim ArrComunita(,) As String = Session("ArrComunita")
                CMNT_Path = ArrComunita(2, UBound(ArrComunita, 2))
            Catch ex As Exception
                CMNT_Path = "."
            End Try

            Dim oPersona As New COL_Persona
            For i = 0 To UBound(ElencoID)
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)
                    Try
                        oPersona.Id = PRSN_ID
                        oPersona.DeIscriviFromComunita(CMNT_ID, CMNT_Path, False)
                        If oPersona.Errore = Errori_Db.None Then
                            Me.AggiornaProfiloXML(CMNT_ID, oPersona.Id, CMNT_Path)
                        End If
                    Catch ex As Exception

                    End Try
                End If
            Next

        Catch ex As Exception

        End Try

        Session("azione") = "loaded"
        Me.MostraLinkbutton(True)
        Me.PNLdeiscriviMultiplo.Visible = False
        Me.PNLcontenuto.Visible = True
        Me.Pre_Bind()
    End Sub

    Private Sub BTNannullaDeiscrizione_multi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNannullaDeiscrizione_multi.Click
        Me.PNLdeiscriviMultiplo.Visible = False
        Me.PNLcontenuto.Visible = True

        Session("azione") = "loaded"
        Me.Pre_Bind()
    End Sub
#End Region


    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click
        Try
            If Me.Request.QueryString("from") = "" Then
                Me.Response.Redirect("./AdminG_ListaComunita.aspx")
            Else
                Select Case LCase(Me.Request.QueryString("from"))
                    Case "ricercacomunita"
                        Me.Response.Redirect("./AdminG_ListaComunita.aspx?re_set=true")
                    Case "ricercabypersona"
                        Me.Response.Redirect("./AdminG_RicercaComunita.aspx?re_set=true")
                    Case Else
                        Me.Response.Redirect("./AdminG_ListaComunita.aspx")
                End Select
            End If
        Catch ex As Exception

        End Try
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
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal 'Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal) 'Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class