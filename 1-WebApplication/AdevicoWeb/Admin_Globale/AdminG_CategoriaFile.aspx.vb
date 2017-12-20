Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona


Public Class AdminG_CategoriaFile
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

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
        NessunTipoComunita = -4
        ErroreAssociazioneLingue = -5
        CancellazioneCorretta = 5
    End Enum

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

#Region "Pannelli Menu"
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenuAzione As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaDati As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Pannello Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Pannello Lista"
    Protected WithEvents PNLlista As System.Web.UI.WebControls.Panel
    Protected WithEvents DGcategoria As System.Web.UI.WebControls.DataGrid
#End Region

#Region "Pannello NoRecord"
    Protected WithEvents PNLnoRecord As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnorecord As System.Web.UI.WebControls.Label
#End Region


#Region "Pannello Inserimento/Modifica"
    Protected WithEvents PNLdati As System.Web.UI.WebControls.Panel
    Protected WithEvents HDctgr_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBelencoTipiComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLtipoComunita As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents RPTnome As System.Web.UI.WebControls.Repeater
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
        Dim oPersona As New COL_Persona
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If
        If Not Page.IsPostBack Then
            Session("azione") = "load"

            oPersona = Session("objPersona")
            Me.PNLpermessi.Visible = False
            Me.PNLcontenuto.Visible = False
			If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
				Me.PNLcontenuto.Visible = True
				Me.Reset_ToInit()
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
        oResource.ResourcesName = "pg_AdminG_CategoriaFile"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBnorecord)
            .setLabel(Me.LBNopermessi)
            .setHeaderDatagrid(Me.DGcategoria, 1, "nome", True)
            .setHeaderDatagrid(Me.DGcategoria, 2, "FileAssociati", True)
            .setLinkButton(Me.LNBinserisci, True, True)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLinkButton(Me.LNBsalvaDati, True, True)
            .setLabel(Me.LBnome_t)
            .setLabel(Me.LBelencoTipiComunita_t)
        End With
    End Sub
#End Region


#Region "Bind_Dati"
    Private Sub Reset_ToInit()
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.PNLmenu.Visible = True
        Me.LNBinserisci.Visible = True
        Me.PNLmenuAzione.Visible = False
        Me.PNLnoRecord.Visible = False
        Me.PNLdati.Visible = False
        Me.PNLlista.Visible = True
        Session("Azione") = "loaded"
        Me.Bind_Griglia()
    End Sub
    Private Sub Bind_Categoria(Optional ByVal CategoriaID As Integer = -5)

        Me.PNLmenu.Visible = False
        Me.PNLmenuAzione.Visible = True
        Me.PNLnoRecord.Visible = False
        Me.PNLlista.Visible = False
        Me.PNLdati.Visible = True

        If CategoriaID = -5 Then
            Session("azione") = "aggiungi"
            Me.TXBnome.Text = ""
        Else
            Dim oCategoria As New COL_CategoriaFile
            oCategoria.Id = CategoriaID
            oCategoria.Estrai()
            If oCategoria.Errore = Errori_Db.None Then
                Me.HDctgr_id.Value = CategoriaID
                Me.TXBnome.Text = oCategoria.nome
                Session("azione") = "modifica"
            Else
                CategoriaID = -5
                Session("azione") = "aggiungi"
                Me.TXBnome.Text = ""
            End If
        End If
        Me.HDctgr_id.Value = CategoriaID
        Me.Bind_TipoComunita(CategoriaID)
        Me.Bind_Lingue()
    End Sub
    Private Sub Bind_Griglia()
        Dim oCategoria As New COL_CategoriaFile
		Dim oDataset As DataSet
        Dim i, totale As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Try
            oDataset = oCategoria.Elenca(Session("LinguaID"), True)

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                Me.PNLlista.Visible = False
                Me.PNLnoRecord.Visible = True
            Else
                oDataset.Tables(0).Columns.Add("FileAssociati")
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If oRow.Item("totale") = 0 Then
                        oRow.Item("FileAssociati") = oResource.getValue("no")
                    Else
                        oRow.Item("FileAssociati") = oResource.getValue("si")
                    End If
                    If totale = 1 Then
                        oRow.Item("totale") = -1
                    End If
                Next
                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "CTGR_nome"
                    viewstate("SortDirection") = "asc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                Me.DGcategoria.DataSource = oDataview
                Me.DGcategoria.DataBind()
            End If
        Catch ex As Exception
            Me.PNLlista.Visible = False
            Me.PNLnoRecord.Visible = True
        End Try
    End Sub
    Private Sub Bind_Lingue()
        Dim oCategoria As New COL_CategoriaFile
        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try
            oCategoria.Id = Me.HDctgr_id.Value
            oDataset = oCategoria.ElencaDefinizioniLingue()
            For i = 0 To oDataset.Tables(0).Rows.Count - 1
                If IsDBNull(oDataset.Tables(0).Rows(i).Item("Nome")) Then
                    oDataset.Tables(0).Rows(i).Item("Nome") = ""
                End If
            Next
            Me.RPTnome.DataSource = oDataset
            Me.RPTnome.DataBind()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoComunita(Optional ByVal CategoriaID As Integer = -5)
        Dim oCategoria As COL_CategoriaFile
        Dim dataset As New DataSet
        Dim Totale, i As Integer

        Try
            dataset = oCategoria.ElencaTipiComunita(CategoriaID, Session("LinguaID"))
            Totale = dataset.Tables(0).Rows.Count
            If Totale > 0 Then
                Me.CBLtipoComunita.DataSource = dataset
                Me.CBLtipoComunita.DataTextField() = "TPCM_descrizione"
                Me.CBLtipoComunita.DataValueField = "TPCM_Id"
                Me.CBLtipoComunita.DataBind()

                For i = 0 To Totale - 1
                    Dim oRow As DataRow
                    oRow = dataset.Tables(0).Rows(i)

                    If oRow.Item("Associato") = 1 Then
                        Me.CBLtipoComunita.Items.Item(i).Selected = True
                    End If
                Next
                Me.LNBsalvaDati.Enabled = True
            Else
                Me.LNBsalvaDati.Enabled = False
            End If
        Catch ex As Exception
            Me.LNBsalvaDati.Enabled = False
        End Try
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGcategoria_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGcategoria.ItemCreated
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
                    If Me.DGcategoria.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGcategoria, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGcategoria, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGcategoria.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGcategoria, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGcategoria.HeaderStyle.CssClass
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
                    oResource.setPageDatagrid(Me.DGcategoria, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"

            Try
                If CBool(e.Item.DataItem("CTGR_noDelete")) = True Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf e.Item.DataItem("Totale") > 0 Then
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

            Dim oImageButton As ImageButton
            Dim NoDelete As Boolean = False
            Try
                NoDelete = e.Item.DataItem("CTGR_noDelete")
            Catch ex As Exception

            End Try

            Try

                oImageButton = e.Item.Cells(0).FindControl("IMBCancella")
                If NoDelete Then 'Or (NoDelete = False And e.Item.DataItem("Totale")>0) Then
                    oImageButton.Enabled = False
                    Me.oResource.setImageButton_Datagrid(Me.DGcategoria, oImageButton, "IMBCancella", False, True, True)
                Else
                    oImageButton.Enabled = True
                    Me.oResource.setImageButton_Datagrid(Me.DGcategoria, oImageButton, "IMBCancella", True, True, True, True)
                End If
            Catch ex As Exception

            End Try

            Try

                oImageButton = e.Item.Cells(0).FindControl("IMBmodifica")
                oImageButton.Enabled = True
                Me.oResource.setImageButton_Datagrid(Me.DGcategoria, oImageButton, "IMBmodifica", True, True, True)
                'End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGcategoria_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGcategoria.PageIndexChanged
        DGcategoria.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGcategoria.SortCommand
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

    Private Sub DGcategoria_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGcategoria.ItemCommand
        Dim CTGR_ID As Integer


        Try
            CTGR_ID = CInt(Me.DGcategoria.DataKeys.Item(e.Item.ItemIndex))

            Select Case e.CommandName
                Case "modifica"
                    Me.Bind_Categoria(CTGR_ID)
                Case "elimina"
                    Dim oCategoria As New COL_CategoriaFile
                    Dim alertMSG As String = ""
                    Dim iResponse As Inserimento
                    oCategoria.Id = CTGR_ID
                    oCategoria.Estrai()
                    If oCategoria.Errore = Errori_Db.None Then
                        If oCategoria.noDelete = False Then
                            oCategoria.Elimina()
                            If oCategoria.Errore = Errori_Db.None Then
                                iResponse = Inserimento.CancellazioneCorretta
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
                    Me.Bind_Griglia()
            End Select
        Catch ex As Exception

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
        oTableCell.Text = oResource.getValue("FileAssociati")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function

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
#End Region

#Region "Gestione Inserimento/Modifica"
    Private Function Salva_DefinizioniLingue() As Inserimento
        Dim LinguaID, i, totale As Integer
        Dim Termine As String = ""
        Dim oCategoria As New COL_CategoriaFile

        oCategoria.Id = Me.HDctgr_id.Value
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
                            Termine = oText.Text
                        Catch ex As Exception
                            Termine = ""
                        End Try

                        If Termine = "" Then
                            Termine = Me.TXBnome.Text
                        End If

                        oCategoria.Translate(Termine, LinguaID)
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
    Private Function Salva_Inserimento() As Inserimento
        Dim oCategoria As New COL_CategoriaFile
        Dim iResponse As Inserimento = Inserimento.ErroreGenerico
        Try
            With oCategoria
                .nome = Me.TXBnome.Text
                .Aggiungi()
                If .Errore = Errori_Db.None Then
                    Dim i, totale As Integer
                    Me.HDctgr_id.Value = .Id
                    If Me.CBLtipoComunita.SelectedIndex > -1 Then
                        totale = Me.CBLtipoComunita.Items.Count - 1
                        Dim Lista As String = ","
                        For i = Me.CBLtipoComunita.SelectedIndex To totale
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
            End With
        Catch ex As Exception

        End Try
        Return iResponse
    End Function
    Private Function Salva_Modifiche() As Inserimento
        Dim oCategoria As New COL_CategoriaFile
        Dim iResponse As Inserimento = Inserimento.ErroreGenerico
        Try
            Dim i, totale As Integer
            With oCategoria
                .Id = Me.HDctgr_id.Value
                .Estrai()
                If .Errore = Errori_Db.None Then
                    .nome = Me.TXBnome.Text
                    If Me.CBLtipoComunita.SelectedIndex > -1 Then
                        totale = Me.CBLtipoComunita.Items.Count - 1
                        Dim Lista As String = ","
                        For i = Me.CBLtipoComunita.SelectedIndex To totale
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
                        iResponse = Inserimento.Modificato
                    End If
                Else
                    iResponse = Inserimento.NONModificato
                End If
            End With
        Catch ex As Exception
            iResponse = Inserimento.NONModificato
        End Try
        Return iResponse
    End Function

    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.Reset_ToInit()
    End Sub
    Private Sub LNBinserisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        Me.Bind_Categoria()
    End Sub

    Private Sub LNBsalvaDati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBsalvaDati.Click
        Dim iResponse As Inserimento
        Dim alertMSG As String = ""

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Session("Azione") = "aggiungi" And Me.HDctgr_id.Value = -5 Then
            iResponse = Me.Salva_Inserimento()
            alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If iResponse = Inserimento.Creato Or iResponse = Inserimento.ErroreAssociazioneLingue Then
                Me.Reset_ToInit()
                Session("Azione") = "loaded"
            End If
        ElseIf Session("Azione") = "modifica" And Me.HDctgr_id.Value > 0 Then
            iResponse = Me.Salva_Modifiche()
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

#End Region

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class