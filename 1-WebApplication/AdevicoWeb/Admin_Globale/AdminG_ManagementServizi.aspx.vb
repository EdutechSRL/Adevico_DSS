Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi


Public Class AdminG_ManagementServizi
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

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

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLinserisci As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBmenu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents DGServizi As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoRecord As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            Dim oPersona As New COL_Persona
            Session("azione") = "load"

            Me.SetupInternazionalizzazione()

            Try
                oPersona = Session("objPersona")
				If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
					Me.PNLcontenuto.Visible = True
					Me.PNLpermessi.Visible = False
					Me.PNLinserisci.Visible = True
					Me.BindGriglia(True)
				Else
					Me.PNLinserisci.Visible = False
					Me.PNLcontenuto.Visible = False
					Me.PNLpermessi.Visible = True
				End If
            Catch ex As Exception
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
            End Try
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
        oResource.ResourcesName = "pg_AdminG_ManagementServizi"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LNBinserisci, True, True)
            .setLinkButton(Me.LNBmenu, True, True)
            .setLabel(Me.LBnoRecord)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub BindGriglia(Optional ByVal Setup As Boolean = False)
        Dim oDataset As New DataSet
        Dim oServizio As New COL_Servizio

        Me.LNBmenu.Enabled = False

        Try
            oDataset = oServizio.ElencaForAdmin(Session("LinguaID"))
            If oDataset.Tables.Count > 0 Then
                Dim i, pos, TotaleRecord As Integer
                TotaleRecord = oDataset.Tables(0).Rows.Count

                If TotaleRecord > 0 Then
                    For i = 0 To TotaleRecord - 1
                        Dim oRow As DataRow
                        oRow = oDataset.Tables(0).Rows(i)

                        If Not IsDBNull(oRow.Item("Nome")) Then
                            oRow.Item("SRVZ_nome") = oRow.Item("Nome")
                        End If
                        If Not IsDBNull(oRow.Item("Descrizione")) Then
                            oRow.Item("SRVZ_Descrizione") = oRow.Item("Descrizione")
                        End If
                    Next
                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        ViewState("SortExspression") = "SRVZ_Nome" 'SRVZ_Associato DESC,
                        viewstate("SortDirection") = ""
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    Me.LBnoRecord.Visible = False
                    Me.LNBmenu.Enabled = True
                    Me.DGServizi.Visible = True
                    DGServizi.DataSource = oDataview


                    If Setup Then
                        Dim pageIndex As Integer = 0
                        Dim numPagine As Integer = 0
                        Try
                            pageIndex = Me.Request.QueryString("pageIndex")
                            numPagine = CInt(TotaleRecord / Me.DGServizi.PageSize)
                            If numPagine < pageIndex Then
                                pageIndex = numPagine
                            End If
                        Catch ex As Exception
                            pageIndex = 0
                        End Try
                       
                        Me.DGServizi.CurrentPageIndex = pageIndex
                    End If
                    DGServizi.DataBind()
                Else
                    Me.DGServizi.Visible = False
                    Me.LBnoRecord.Visible = True
                End If

            Else
                Me.DGServizi.Visible = False
                Me.LBnoRecord.Visible = True
            End If
        Catch ex As Exception

            Me.DGServizi.Visible = False
            Me.LBnoRecord.Visible = True
        End Try
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGServizi_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGServizi.ItemCreated
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
                    If Me.DGServizi.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGServizi.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGServizi.HeaderStyle.CssClass
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
            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                Dim num As Integer = 1
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
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
                    oResource.setPageDatagrid(Me.DGServizi, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"

            Try
                If CBool(e.Item.DataItem("SRVZ_nonDisattivabile")) = True Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("SRVZ_Associato")) = False Then
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
                Dim oTBRnome As TableRow
                oTBRnome = e.Item.Cells(1).FindControl("TBRnome")

                If IsNothing(oTBRnome) = False Then
                    oTBRnome.CssClass = cssRiga
                End If
            Catch ex As Exception

            End Try

            Try
                Dim div As HtmlGenericControl = e.Item.Cells(1).FindControl("DVdescription")

                If IsNothing(div) = False Then
                    If IsDBNull(e.Item.DataItem("SRVZ_Descrizione")) Then
                        div.Visible = False
                    Else
                        Try
                            If Trim(e.Item.DataItem("SRVZ_Descrizione")) = "" Or e.Item.DataItem("SRVZ_Descrizione") = "&nbsp;" Then
                                div.Visible = False
                            End If
                        Catch ex As Exception
                            div.Visible = False
                        End Try
                    End If
                End If
            Catch ex As Exception

            End Try

            'Link Button per nascondere/ativare servizio
            Dim oLNBattiva As LinkButton
            Dim oLNBdisattiva As LinkButton
            Try
                oLNBattiva = e.Item.Cells(1).FindControl("LNBattiva")
                oLNBdisattiva = e.Item.Cells(1).FindControl("LNBdisattiva")

                oLNBdisattiva.CssClass = cssLink
                oLNBattiva.CssClass = cssLink
                Me.oResource.setLinkButton(oLNBdisattiva, True, True)
                Me.oResource.setLinkButton(oLNBattiva, True, True)
                If CBool(e.Item.DataItem("SRVZ_nonDisattivabile")) = True Then
                    oLNBattiva.Visible = False
                    oLNBdisattiva.Visible = False
                Else
					' If e.Item.DataItem("SRVZ_Associato") = 1 Then
					If CBool(e.Item.DataItem("SRVZ_Attivato")) Then
						oLNBdisattiva.Visible = True
						oLNBattiva.Visible = False
					Else
						oLNBdisattiva.Visible = False
						oLNBattiva.Visible = True
					End If
					'oLNBattiva.CommandArgument = e.Item.DataItem("SRVZ_ID")
					'oLNBdisattiva.CommandArgument = e.Item.DataItem("SRVZ_ID")
					'  Else
					'oLNBattiva.Visible = False
					'oLNBdisattiva.Visible = False
					'End If
				End If

            Catch ex As Exception

		End Try

 
            Try
                Dim oLNBassociaPermessi, oLNBassociaTipiComunita, oLNBdefinisciRuoli As LinkButton
                oLNBassociaPermessi = e.Item.Cells(1).FindControl("LNBassociaPermessi")
                oLNBassociaTipiComunita = e.Item.Cells(1).FindControl("LNBassociaTipiComunita")
                oLNBdefinisciRuoli = e.Item.Cells(1).FindControl("LNBdefinisciRuoli")

                oLNBdefinisciRuoli.CssClass = cssLink
                oLNBassociaPermessi.CssClass = cssLink
                oLNBassociaTipiComunita.CssClass = cssLink
                Me.oResource.setLinkButton(oLNBdefinisciRuoli, True, True)
                Me.oResource.setLinkButton(oLNBassociaPermessi, True, True)
                Me.oResource.setLinkButton(oLNBdefinisciRuoli, True, True)
            Catch ex As Exception

            End Try

            Try
                Dim oIMBinfo As ImageButton
                Dim oIMBelimina As ImageButton

                oIMBinfo = e.Item.Cells(0).FindControl("IMBinfo")
                oIMBinfo.Attributes.Add("onClick", "window.status='Visualizza informazioni dettagliate.';OpenWin('" & "./DettagliServizio.aspx?SRVZ_ID=" & e.Item.DataItem("SRVZ_ID") & "','550','500','yes','no');return false;")

                oIMBelimina = e.Item.Cells(0).FindControl("IMBelimina")
                If CBool(e.Item.DataItem("SRVZ_nonDisattivabile")) = True Then
                    oIMBelimina.Visible = False
                Else
                    oIMBelimina.Visible = True
                    Me.oResource.setImageButton(oIMBelimina, True, True, True, True)
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGServizi_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGServizi.PageIndexChanged
        Me.DGServizi.CurrentPageIndex = e.NewPageIndex
        Me.BindGriglia()
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGServizi.SortCommand
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
        Me.BindGriglia()
    End Sub

    Private Sub DGtipoComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGServizi.ItemCommand
        Dim ServizioID As Integer
        Dim oServizio As New COL_Servizio

        Try
            ServizioID = CInt(Me.DGServizi.DataKeys.Item(e.Item.ItemIndex))
            Select Case e.CommandName
                Case "modifica"
                    Session("Azione") = "modifica_" & ServizioID
                    Session("Azione_selezione") = ""
                    Response.Redirect("./AdminG_WizardModificaServizio.aspx?pageIndex=" & Me.DGServizi.CurrentPageIndex)
				Case "elimina"
					oServizio.ID = ServizioID
					oServizio.Elimina()
					Me.BindGriglia()
                Case "attiva"
                    oServizio.AttivaServizio(ServizioID, True)
                    Me.BindGriglia()
                Case "disattiva"
                    oServizio.AttivaServizio(ServizioID, False)
                    Me.BindGriglia()
                Case "ruoli"
                    Session("Azione") = "modifica_" & ServizioID
                    Session("Azione_selezione") = "DefinisciPermessi"
                    Response.Redirect("./AdminG_WizardModificaServizio.aspx?pageIndex=" & Me.DGServizi.CurrentPageIndex)

                Case "permessi"
                    Session("Azione") = "modifica_" & ServizioID
                    Session("Azione_selezione") = "AssociaPermessi"
                    Response.Redirect("./AdminG_WizardModificaServizio.aspx?pageIndex=" & Me.DGServizi.CurrentPageIndex)
                Case "tipocomunita"
                    Session("Azione") = "modifica_" & ServizioID
                    Session("Azione_selezione") = "tipocomunita"
                    Response.Redirect("./AdminG_WizardModificaServizio.aspx?pageIndex=" & Me.DGServizi.CurrentPageIndex)
                Case Else
                    Me.BindGriglia()
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
        oTableCell.Text = oResource.getValue("NONdisattivabile")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("NONassociato")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function
#End Region

    Private Sub LNBinserisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        Session("Azione") = "inserisci"
        Session("Azione_selezione") = ""
        Response.Redirect("./WizardCreaServizio.aspx")
    End Sub

    Private Sub LNBmenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmenu.Click
        Session("Azione") = "inserisci"
        Session("Azione_selezione") = ""
        Response.Redirect("./AdminG_MenuComunita.aspx")
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class