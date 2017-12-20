Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comol.Entities

Public Class AdminG_ManagementRuoli
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Private _oldPageutility As OLDpageUtility
    Private ReadOnly Property PageUtility
        Get
            If IsNothing(_oldPageutility) Then
                _oldPageutility = New OLDpageUtility(Me.Context)
            End If
            Return _oldPageutility
        End Get
    End Property
    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum
    Public Enum Inserimento
        ErroreGenerico = 0
        Creato = 1
        Modificato = 2
        PermessiAssociati = 3
        OperazioneConclusa = 4
        DescrizioneMancante = -1
        NONModificato = -2
        NONinserito = -3
        SelezionaLivello = -4
        NessunServizioPresente = -5
        NessunTipoComunita = -6
        PermessiAssociatiParziali = -7
        ErroreAssociazioneLingue = -8
        CancellazioneCorretta = 5
    End Enum
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenuAzione As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents HYPtoSettings As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LNBsalvaDati As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLlista As System.Web.UI.WebControls.Panel
    Protected WithEvents DGtipoRuolo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLnorecord As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnorecord As System.Web.UI.WebControls.Label

#Region "Gestione Dati"
    Protected WithEvents PNLdati As System.Web.UI.WebControls.Panel
    Protected WithEvents HDNtprl_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents RPTnome As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdescrizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents RPTdescrizione As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBlivelloRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBlivello As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBlivello_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoli_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBLlivelli As System.Web.UI.WebControls.Table

    Protected WithEvents TBRtipocomunita As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBtipoComunitaIns_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLtipoComunita As System.Web.UI.WebControls.CheckBoxList
#End Region

#Region "Permessi"
    Protected WithEvents HDNsetup As System.Web.UI.HtmlControls.HtmlInputHidden
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

        If Me.Page.IsPostBack = False Then
            Dim oPersona As New COL_Persona
            Session("azione") = "load"

            oPersona = Session("objPersona")
            Me.SetupInternazionalizzazione()

            Me.PNLcontenuto.Visible = False
            Me.PNLpermessi.Visible = False
            Me.PNLmenu.Visible = False
            Me.PNLmenuAzione.Visible = False
            Session("Azione") = "loaded"
            Me.HDNtprl_ID.Value = -1
			If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
				Me.PNLcontenuto.Visible = True
				Me.PNLmenu.Visible = True
				Me.Bind_Griglia()
			Else
				Me.PNLpermessi.Visible = True
			End If
        End If
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


#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_AdminG_ManagementRuoli"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(Me.LBnorecord)
            .setLabel(Me.LBNopermessi)
            .setLabel(Me.LBnome_t)
            .setLabel(Me.LBlivello_t)
            .setLabel(Me.LBdescrizione_t)
            .setHeaderDatagrid(Me.DGtipoRuolo, 1, "nome", True)
            .setHeaderDatagrid(Me.DGtipoRuolo, 2, "hasComunita", True)
            .setLabel(Me.LBlivelloRuolo_t)
            .setLabel(Me.LBruoli_t)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLinkButton(Me.LNBinserisci, True, True)
            .setLinkButton(Me.LNBsalvaDati, True, True)
            .setLabel(Me.LBtipoComunitaIns_t)
        End With

    End Sub
#End Region

#Region "Bind Dati"
    Private Sub Bind_Griglia()
        Dim oTipoRuolo As New COL_TipoRuolo
        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try
            oDataset = oTipoRuolo.Elenca(Session("LinguaID"), True)

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then
                Me.PNLlista.Visible = False
                Me.PNLnorecord.Visible = True
            Else
                oDataset.Tables(0).Columns.Add("ComunitaAssociate")
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If oRow.Item("associato") = 0 Then
                        oRow.Item("ComunitaAssociate") = Me.oResource.getValue("no")
                    Else
                        oRow.Item("ComunitaAssociate") = Me.oResource.getValue("si")
                    End If
                    If totale = 1 Then
                        oRow.Item("totale") = -1
                    End If
                Next
                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "TPRL_nome"
                    viewstate("SortDirection") = "asc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                Me.DGtipoRuolo.DataSource = oDataview
                Me.DGtipoRuolo.DataBind()
            End If
        Catch ex As Exception
            Me.PNLlista.Visible = False
            Me.PNLnorecord.Visible = True
        End Try
    End Sub
    Private Sub Bind_DatiRuolo(ByVal RuoloID As Integer)
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.HYPtoSettings.Visible = False

        If RuoloID = 0 Then
            Session("Azione") = "insert"
            Me.TXBdescrizione.Text = ""
            Me.TXBnome.Text = ""
            Me.Bind_Livelli(RuoloID)
            'Me.LBTitolo.Text = Me.oResource.getValue("titolo.inserimento")
            Me.Master.ServiceTitle = Me.oResource.getValue("titolo.inserimento")
            Me.TBRtipocomunita.Visible = True
            Me.Bind_TipiComunità()
            Me.HDNsetup.Value = False
        Else
            Dim oRuolo As New COL_TipoRuolo
            oRuolo.Id = RuoloID
            oRuolo.Estrai()
            If oRuolo.Errore = Errori_Db.None Then
                Me.HDNtprl_ID.Value = RuoloID
                Me.TXBdescrizione.Text = oRuolo.Descrizione
                Me.TXBnome.Text = oRuolo.Nome
                Try
                    Me.Bind_Livelli(RuoloID, oRuolo.Gerarchia)
                Catch ex As Exception

                End Try
                'Me.LBTitolo.Text = Me.oResource.getValue("titolo.modifica")
                Me.Master.ServiceTitle = Me.oResource.getValue("titolo.modifica")
                Me.TBRtipocomunita.Visible = False
                Me.HYPtoSettings.Visible = True
                HYPtoSettings.NavigateUrl = PageUtility.BaseUrl & "Modules/RoleManagement/RoleSettings.aspx?idRole=" & RuoloID
                Me.HDNsetup.Value = False
            Else
                Session("Azione") = "insert"
                'Me.LBTitolo.Text = Me.oResource.getValue("titolo.inserimento")
                Me.Master.ServiceTitle = Me.oResource.getValue("titolo.inserimento")

                Me.TBRtipocomunita.Visible = True
                Me.TXBdescrizione.Text = ""
                Me.TXBnome.Text = ""
                Me.HDNtprl_ID.Value = 0
                Me.Bind_Livelli(0)
                Me.Bind_TipiComunità()
                Me.HDNsetup.Value = False
            End If
        End If
        Me.Bind_Lingue()
    End Sub

    Private Sub Bind_Lingue()
        Dim oTipoRuolo As New COL_TipoRuolo
		Dim i, totale As Integer
		Dim oLista As New List(Of PlainRole)
        Try
            oTipoRuolo.Id = Me.HDNtprl_ID.Value
			oLista = oTipoRuolo.ElencaDefinizioniLingue()
			Me.RPTnome.DataSource = oLista
            Me.RPTnome.DataBind()
			Me.RPTdescrizione.DataSource = oLista
			Me.RPTdescrizione.DataBind()
		Catch ex As Exception

		End Try
    End Sub
    Private Sub Bind_Livelli(ByVal RuoloID As Integer, Optional ByVal GerarchiaID As Integer = -1)
        Dim oTipoRuolo As New COL_TipoRuolo
        Dim nomeRuoli As String
        Dim oDataset As DataSet
        Dim TPRL_gerarchiaMAX, i, j, totale As Integer
        Try
            Dim oDataview As New DataView
            TPRL_gerarchiaMAX = oTipoRuolo.EstraiGerarchiaMassima
            oDataset = oTipoRuolo.Elenca(Session("LinguaID"), True)
            totale = oDataset.Tables(0).Rows.Count
            Me.RBlivello.Items.Clear()
            If totale = 0 Then

            Else
                oDataview = oDataset.Tables(0).DefaultView
                For i = 0 To TPRL_gerarchiaMAX + 1
                    Dim oTbrow As New TableRow
                    Dim oCell As New TableCell
                    Dim oCell2 As New TableCell
                    RBlivello.Items.Insert(i, i)

                    oDataview.RowFilter = "TPRL_Gerarchia=" & i
                    If oDataview.Count = 0 Then
                        oCell.Text = "&nbsp;"
                    Else
                        nomeRuoli = ""
                        For j = 0 To oDataview.Count - 1
                            Dim oRow As DataRow
                            oRow = oDataview.Item(j).Row
                            If nomeRuoli = "" Then
                                nomeRuoli = oRow.Item("TPRL_Nome")
                            Else
                                nomeRuoli = nomeRuoli & ", " & oRow.Item("TPRL_Nome")
                            End If
                            If oRow.Item("TPRL_id") = RuoloID Then
                                oCell.BackColor = System.Drawing.Color.LightBlue
                                oCell2.BackColor = System.Drawing.Color.LightBlue
                            End If
                            oCell.CssClass = "ROW_TD_Small9"
                            oCell2.CssClass = "ROW_TD_Small9"
                            oCell.BorderColor = System.Drawing.Color.LightGray
                            oCell.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
                            oCell2.BorderColor = System.Drawing.Color.LightGray
                            oCell2.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)

                            oCell2.HorizontalAlign = HorizontalAlign.Center

                            oCell2.Text = i
                            oCell.Text = nomeRuoli
                            oTbrow.Cells.Add(oCell2)
                            oTbrow.Cells.Add(oCell)
                        Next
                    End If

                    Me.TBLlivelli.Rows.Add(oTbrow)
                Next
            End If

        Catch ex As Exception

        End Try
        Try
            Me.RBlivello.SelectedValue = GerarchiaID
        Catch ex As Exception
            If RuoloID = 0 Then
                Me.RBlivello.Items(Me.RBlivello.Items.Count - 1).Selected = True
            End If
        End Try
    End Sub
    Private Sub Bind_TipiComunità()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As New DataSet
        Dim i, totale As Integer
        Try
            oDataset = oTipoComunita.Elenca(Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.CBLtipoComunita.DataValueField = "TPCM_ID"
                Me.CBLtipoComunita.DataTextField = "TPCM_Descrizione"
                Me.CBLtipoComunita.DataSource = oDataset
                Me.CBLtipoComunita.DataBind()
            Else
                Me.TBRtipocomunita.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGtipoRuolo_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGtipoRuolo.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
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
                    If Me.DGtipoRuolo.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGtipoRuolo, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGtipoRuolo, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGtipoRuolo.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "


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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGtipoRuolo, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGtipoRuolo.HeaderStyle.CssClass
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

            n = oCell.ColumnSpan
            ' Aggiungo riga con descrizione:

            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                Dim num As Integer = 2
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                oTableCell.ColumnSpan = num
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oCell.ColumnSpan = 1
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
                    oResource.setPageDatagrid(Me.DGtipoRuolo, oLinkbutton)
                End Try
            Next
        End If

   
	End Sub

	Private Sub DGtipoRuolo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGtipoRuolo.ItemDataBound
		If IsNothing(oResource) Then
			Me.SetCulture(Session("LinguaCode"))
		End If
		If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
			Dim cssLink As String = "ROW_ItemLink_Small"
			Dim cssRiga As String = "ROW_TD_Small"

			Try
				If CBool(e.Item.DataItem("TPRL_noDelete")) = True Then
					e.Item.CssClass = "ROW_Disattivate_Small"
					cssLink = "ROW_ItemLinkDisattivate_Small"
					cssRiga = "ROW_ItemDisattivate_Small"
				ElseIf CBool(e.Item.DataItem("associato")) = True Then
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
					e.Item.CssClass = "ROW_Normal_Small"
				End If
			End Try

			Try
				Dim oTBRdescrizione As TableRow
				oTBRdescrizione = e.Item.Cells(1).FindControl("TBRdescrizione")

				If IsNothing(oTBRdescrizione) = False Then
					oTBRdescrizione.CssClass = cssRiga

					If IsDBNull(e.Item.DataItem("TPRL_Descrizione")) Then
						oTBRdescrizione.Visible = False
					Else
						Try
							If Trim(e.Item.DataItem("TPRL_Descrizione")) = "" Then
								oTBRdescrizione.Visible = False
							End If
						Catch ex As Exception
							oTBRdescrizione.Visible = False
						End Try
					End If
				End If
			Catch ex As Exception

			End Try
			Try
				Dim oLBruolo As Label

				oLBruolo = e.Item.Cells(1).FindControl("LBruolo")
				If IsNothing(oLBruolo) = False Then
					oLBruolo.CssClass = cssRiga
				End If
			Catch ex As Exception

			End Try

			Try
				Dim oLBdescrz_t As Label

				oLBdescrz_t = e.Item.Cells(1).FindControl("LBdescrz_t")
				If IsNothing(oLBdescrz_t) = False Then
					oLBdescrz_t.CssClass = cssRiga
					Me.oResource.setLabel(oLBdescrz_t)
				End If
			Catch ex As Exception

			End Try
			Try
				Dim oLBdescrizione As Label

				oLBdescrizione = e.Item.Cells(1).FindControl("LBdescrizione")
				If IsNothing(oLBdescrizione) = False Then
					oLBdescrizione.CssClass = cssRiga

				End If
			Catch ex As Exception

			End Try


            'Try
            '	Dim oLNBvisualizzaPermessi As LinkButton

            '	oLNBvisualizzaPermessi = e.Item.Cells(1).FindControl("LNBvisualizzaPermessi")
            '	If IsNothing(oLNBvisualizzaPermessi) = False Then
            '		oLNBvisualizzaPermessi.CssClass = cssLink
            '		Me.oResource.setLinkButton(oLNBvisualizzaPermessi, True, True)
            '	End If
            'Catch ex As Exception

            'End Try

			Dim oImageButton As ImageButton

			Dim NoDelete As Boolean = False
			Dim NoModify As Boolean = False

			Try
				NoDelete = e.Item.DataItem("TPRL_noDelete")
			Catch ex As Exception

			End Try
			Try
				NoModify = e.Item.DataItem("TPRL_noModify")
			Catch ex As Exception

			End Try


			Try

				oImageButton = e.Item.Cells(0).FindControl("IMBCancella")
				If NoDelete Or (NoDelete = False And e.Item.DataItem("associato") = True) Then
					oImageButton.Enabled = False
					Me.oResource.setImageButton_Datagrid(Me.DGtipoRuolo, oImageButton, "IMBCancella", False, True, True)
				Else
					oImageButton.Enabled = True
					Me.oResource.setImageButton_Datagrid(Me.DGtipoRuolo, oImageButton, "IMBCancella", True, True, True, True)
				End If
			Catch ex As Exception

			End Try

			Try

				oImageButton = e.Item.Cells(0).FindControl("IMBmodifica")
				'If NoModify Then
				'    oImageButton.Enabled = False
				'    Me.oResource.setImageButton_Datagrid(Me.DGtipoRuolo, oImageButton, "IMBmodifica", False, True, True)
				'Else
				oImageButton.Enabled = True
				Me.oResource.setImageButton_Datagrid(Me.DGtipoRuolo, oImageButton, "IMBmodifica", True, True, True)
				'End If
			Catch ex As Exception

            End Try

            Dim oHyperLink As HyperLink = e.Item.FindControl("HYPsettings")
            oHyperLink.NavigateUrl = PageUtility.BaseUrl & "Modules/RoleManagement/RoleSettings.aspx?idRole=" & e.Item.DataItem("TPRL_ID")
		End If
	End Sub
    Private Sub DGtipoRuolo_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGtipoRuolo.PageIndexChanged
        Me.DGtipoRuolo.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGtipoRuolo.SortCommand
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
        Bind_Griglia()
    End Sub

    Private Sub DGtipoRuolo_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGtipoRuolo.ItemCommand
        Dim TPRL_ID As Integer
        Dim alertMSG As String = ""

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Try
            TPRL_ID = CInt(Me.DGtipoRuolo.DataKeys.Item(e.Item.ItemIndex))
            Me.HYPtoSettings.Visible = False
            Select Case e.CommandName
                Case "modifica"
                    Me.PNLmenuAzione.Visible = True
                    Me.PNLnorecord.Visible = False
                    Me.PNLdati.Visible = True
                    Session("Azione") = "modifica"
                    Me.PNLlista.Visible = False
                    Me.PNLmenu.Visible = False
                    Me.Bind_DatiRuolo(TPRL_ID)

                Case "elimina"
                    Try
                        Dim oTipoRuolo As New COL_TipoRuolo
                        Dim iResponse As Inserimento
                        oTipoRuolo.Id = TPRL_ID
                        oTipoRuolo.Estrai()
                        If oTipoRuolo.Errore = Errori_Db.None Then
                            If oTipoRuolo.noDelete = False Then
                                oTipoRuolo.Elimina()
                                If oTipoRuolo.Errore = Errori_Db.None Then
									iResponse = Inserimento.CancellazioneCorretta
									Me.Bind_Griglia()
									Exit Sub
                                Else
                                    iResponse = Inserimento.ErroreGenerico
                                End If
                            End If
                        Else
                            iResponse = Inserimento.ErroreGenerico
                        End If

                        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
                        If alertMSG <> "" Then
                            alertMSG = alertMSG.Replace("'", "\'")
                            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                        End If


                    Catch ex As Exception

					End Try
					Me.Bind_Griglia()
                Case Else
                    'Bind_Griglia()

            End Select
        Catch ex As Exception
            Bind_Griglia()
        End Try
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
        oTableCell.Text = oResource.getValue("NONcancellabili")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("ComunitaAssociate")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function

#End Region

    Private Sub RPTnome_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnome.ItemCreated
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaNome_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTdescrizione_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTdescrizione.ItemCreated
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaDescrizione_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub LNBinserisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        Me.PNLmenuAzione.Visible = False
        Me.PNLmenu.Visible = False
        Me.PNLnorecord.Visible = False
        Me.PNLlista.Visible = False
        Me.PNLdati.Visible = False

        Session("Azione") = "insert"
        Me.HDNtprl_ID.Value = 0
        Me.PNLdati.Visible = True
        Me.PNLmenuAzione.Visible = True
        Me.Bind_DatiRuolo(0)
    End Sub

    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.Reset_ToInit()
    End Sub

    Private Sub Reset_ToInit()
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Me.PNLmenuAzione.Visible = False
        Me.PNLnorecord.Visible = False
        Me.PNLdati.Visible = False
        Me.PNLlista.Visible = True
        Me.Bind_Griglia()
        Me.PNLmenu.Visible = True
        Session("Azione") = "loaded"
        'Me.oResource.setLabel(Me.LBTitolo)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
    End Sub

    Private Sub LNBsalvaDati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaDati.Click
        Dim iResponse As Inserimento
        Dim alertMSG As String = ""

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Session("Azione") = "insert" And Me.HDNtprl_ID.Value = 0 Then
            iResponse = Me.Salva_InserimenoRuolo()
            alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If iResponse = Inserimento.Creato Or iResponse = Inserimento.ErroreAssociazioneLingue Then
                Dim utility As New OLDpageUtility(Me.Context)
                utility.RedirectToUrl("Modules/RoleManagement/RoleSettings.aspx?idRole=" & Me.HDNtprl_ID.Value)
            End If
        ElseIf Session("Azione") = "modifica" And (Me.HDNtprl_ID.Value > 0 Or Me.HDNtprl_ID.Value = -4) Then
            iResponse = Me.Salva_ModificheRuolo()
            alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If iResponse = Inserimento.Modificato Or iResponse = Inserimento.ErroreAssociazioneLingue Then
                Me.Reset_ToInit()
                Session("Azione") = "loaded"
            End If
        Else
            Me.Reset_ToInit()
        End If
    End Sub
    Private Function Salva_InserimenoRuolo() As Inserimento
        Dim oRuolo As New COL_TipoRuolo
        Dim i, totale As Integer
        Dim iResponse As Inserimento = Inserimento.ErroreGenerico

        With oRuolo
            If Me.RBlivello.Items.Count = 0 Then
                iResponse = Inserimento.SelezionaLivello
            Else
                If Me.RBlivello.SelectedIndex < 0 Then
                    iResponse = Inserimento.SelezionaLivello
                Else
                    .Gerarchia = Me.RBlivello.SelectedValue
                    .Nome = Me.TXBnome.Text
                    .Descrizione = Me.TXBdescrizione.Text
                    .noDelete = False
                    .noModify = False
                    .Aggiungi()
                    If .Errore = Errori_Db.None Then
                        Me.HDNtprl_ID.Value = .Id
                        If Me.CBLtipoComunita.SelectedIndex > -1 Then
                            Dim Lista As String = ","
                            totale = Me.CBLtipoComunita.Items.Count - 1
                            For i = 0 To totale
                                If Me.CBLtipoComunita.Items(i).Selected Then
                                    Lista &= Me.CBLtipoComunita.Items(i).Value & ","
                                End If
                            Next

                            If Lista <> "," Then
                                .AssociaTipiComunita(Lista)
                            End If
                        End If

                        iResponse = Me.Salva_DefinizioniLingue
                        If iResponse = Inserimento.OperazioneConclusa Then
                            iResponse = Inserimento.Creato
                        End If
                    Else
                        iResponse = Inserimento.NONinserito
                    End If
                End If
            End If

        End With
        Return iResponse
    End Function
    Private Function Salva_ModificheRuolo() As Inserimento
        Dim oRuolo As New COL_TipoRuolo
        Dim iResponse As Inserimento = Inserimento.ErroreGenerico

        With oRuolo
            .Id = Me.HDNtprl_ID.Value
            .Estrai()
            If .Errore = Errori_Db.None Then
                If Me.RBlivello.Items.Count = 0 Then
                    iResponse = Inserimento.SelezionaLivello
                Else
                    If Me.RBlivello.SelectedIndex < 0 Then
                        iResponse = Inserimento.SelezionaLivello
                    Else
                        .Gerarchia = Me.RBlivello.SelectedValue
                        .Nome = Me.TXBnome.Text
                        .Descrizione = Me.TXBdescrizione.Text
                        .Modifica()
                        If .Errore = Errori_Db.None Then
                            iResponse = Me.Salva_DefinizioniLingue
                            If iResponse = Inserimento.OperazioneConclusa Then
                                iResponse = Inserimento.Modificato
                            End If
                        Else
                            iResponse = Inserimento.NONModificato
                        End If
                    End If
                End If
            Else
                iResponse = Inserimento.ErroreGenerico
            End If
        End With
        Return iResponse
    End Function
    Private Function Salva_DefinizioniLingue() As Inserimento
        Dim LinguaID, i, totale As Integer
        Dim Nome, Descrizione As String
        Dim oRuolo As New COL_TipoRuolo

        oRuolo.Id = Me.HDNtprl_ID.Value
        totale = Me.RPTnome.Items.Count

        Try
            If totale > 0 Then
                For i = 0 To totale - 1
                    Dim oLabel As Label
                    Dim oText As TextBox

                    Try
                        oLabel = Me.RPTnome.Items(i).FindControl("LBlinguaID")
                        LinguaID = oLabel.Text
                    Catch ex As Exception
                        LinguaID = 0
                    End Try
                    If LinguaID > 0 Then
                        Try
                            oText = Me.RPTnome.Items(i).FindControl("TXBtermine")
                            Nome = oText.Text
                        Catch ex As Exception
                            Nome = ""
                        End Try

                        If Nome = "" Then
                            Nome = Me.TXBnome.Text
                        End If

                        Try
                            oText = Me.RPTdescrizione.Items(i).FindControl("TXBtermine2")
                            Descrizione = oText.Text
                        Catch ex As Exception
                            Descrizione = ""
                        End Try

                        If Descrizione = "" Then
                            Descrizione = Me.TXBdescrizione.Text
                        End If

                        oRuolo.Translate(Nome, Descrizione, LinguaID)
                    End If
                Next
                Return Inserimento.OperazioneConclusa
            Else
                Return Inserimento.ErroreAssociazioneLingue
            End If
        Catch ex As Exception
            Return Inserimento.ErroreAssociazioneLingue
        End Try
    End Function


    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class