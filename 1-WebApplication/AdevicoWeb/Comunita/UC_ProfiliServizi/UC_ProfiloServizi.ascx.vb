Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi


Public Class UC_ProfiloServizi
    Inherits System.Web.UI.UserControl

    Private oResource As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Public ReadOnly Property ProfiloID() As Integer
        Get
            Try
                ProfiloID = Me.HDN_profiloID.Value
            Catch ex As Exception
                Me.HDN_profiloID.Value = 0
                ProfiloID = 0
            End Try
        End Get
    End Property

    Public ReadOnly Property hasAssociati() As Integer
        Get
            Try
                hasAssociati = (Me.HDN_checkbox.Value <> "" And Me.HDN_checkbox.Value <> ",")
            Catch ex As Exception
                hasAssociati = False
            End Try
        End Get
    End Property
    Public ReadOnly Property isDefinito() As Boolean
        Get
            Try
                isDefinito = (HDN_isDefinito.Value = True)
            Catch ex As Exception
                isDefinito = False
            End Try
        End Get
    End Property

    Protected WithEvents HDN_isDefinito As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents HDN_checkbox As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_profiloID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_TPCM_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBLservizio As System.Web.UI.WebControls.Table
    Protected WithEvents LBinfoServizi As System.Web.UI.WebControls.Label
    Protected WithEvents LBserviziAttivi_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents DGServizi As System.Web.UI.WebControls.DataGrid

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
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_DatiProfilo"
        oResource.Folder_Level1 = "Comunita"
        oResource.Folder_Level2 = "UC_ProfiliServizi"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBinfoServizi)
            .setLabel(Me.LBserviziAttivi_t)
            .setLabel(Me.LBnoRecord)
            .setHeaderDatagrid(Me.DGServizi, 0, "nome", True)
            .setHeaderDatagrid(Me.DGServizi, 1, "attivatoDefault", True)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Public Function Setup_Controllo(ByVal ProfiloID As Integer) As WizardProfilo_Message
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ProfiloNonTrovato

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()
        Try
            Dim oProfilo As New COL_ProfiloServizio


            Me.HDN_profiloID.Value = ProfiloID
            Me.HDN_checkbox.Value = ""
            Me.HDN_isDefinito.Value = False
            oProfilo.Id = ProfiloID
            If ProfiloID > 0 Then
                oProfilo.Estrai()
                If oProfilo.Errore <> Errori_Db.None Then
                    iResponse = WizardProfilo_Message.ProfiloNonTrovato
                Else
                    Me.HDN_TPCM_ID.Value = oProfilo.TipoComunitaID
                End If
            Else
                iResponse = WizardProfilo_Message.ProfiloNonTrovato
            End If

            Return Bind_Griglia(True)

        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Private Function Bind_Griglia(Optional ByVal Ricalcola As Boolean = False) As WizardProfilo_Message
        Dim oProfilo As New COL_ProfiloServizio
        Dim i, totale As Integer
        Dim oDataset As New DataSet


        Try
            oProfilo.Id = Me.HDN_profiloID.Value
            oProfilo.TipoComunitaID = Me.HDN_TPCM_ID.Value
            oDataset = oProfilo.ElencaServiziDefiniti(Session("LinguaID"))

            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.LBnoRecord.Visible = True
                Me.DGServizi.Visible = False
            Else
                Me.LBnoRecord.Visible = False
                Me.DGServizi.Visible = True

                If Ricalcola = True Then
                    Me.HDN_checkbox.Value = ","

                    For i = 0 To oDataset.Tables(0).Rows.Count - 1
                        Dim oRow As DataRow
                        oRow = oDataset.Tables(0).Rows(i)
                        If oRow.Item("isDefault") Then
                            Me.HDN_checkbox.Value &= oRow.Item("SRVZ_ID") & ","
                        ElseIf oRow.Item("isNonDisattivabile") Then
                            Me.HDN_checkbox.Value &= oRow.Item("SRVZ_ID") & ","
                        End If
                    Next
                End If
                Try
                    Me.HDN_isDefinito.Value = (oDataset.Tables(0).Rows(0).Item("isDefinito") = 1)
                Catch ex As Exception
                    Me.HDN_isDefinito.Value = False
                End Try
                If Me.HDN_checkbox.Value = "," Then
                    Me.HDN_checkbox.Value = ""
                End If
                Me.DGServizi.DataSource = oDataset
                Me.DGServizi.DataBind()
                Return WizardProfilo_Message.OperazioneConclusa
            End If

        Catch ex As Exception
            Me.LBnoRecord.Visible = True
            Me.DGServizi.Visible = False
        End Try
        Return WizardProfilo_Message.TipoRuoloNonTrovato
    End Function
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
                                If oSortDirection = "asc" Then
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
                oCell.ColumnSpan = num
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
                If CBool(e.Item.DataItem("SRVZ_Attivato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("SRVZ_nonDisattivabile")) = True Then
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
                Dim oTBRdescrizione As TableRow
                oTBRdescrizione = e.Item.Cells(1).FindControl("TBRdescrizione")

                If IsNothing(oTBRdescrizione) = False Then
                    oTBRdescrizione.CssClass = cssRiga

                    If IsDBNull(e.Item.DataItem("SRVZ_Descrizione")) Then
                        oTBRdescrizione.Visible = False
                    Else
                        Try
                            If Trim(e.Item.DataItem("SRVZ_Descrizione")) = "" Or e.Item.DataItem("SRVZ_Descrizione") = "&nbsp;" Then
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
                Dim oCheck As HtmlControls.HtmlInputCheckBox
                oCheck = e.Item.Cells(1).FindControl("CBXservizioAttivato")
                If Not IsNothing(oCheck) Then
                    Try
                        If InStr(Me.HDN_checkbox.Value, "," & e.Item.DataItem("SRVZ_ID") & ",") > 0 Then
                            oCheck.Checked = True
                        Else
                            oCheck.Checked = False
                        End If
                    Catch ex As Exception
                        oCheck.Checked = False
                    End Try
                    oCheck.Value = e.Item.DataItem("SRVZ_ID")
                    Try
                        If e.Item.DataItem("SRVZ_nonDisattivabile") = True Or e.Item.DataItem("SRVZ_Attivato") = False Then
                            oCheck.Disabled = True
                        Else
                            oCheck.Disabled = False
                        End If
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGServizi_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGServizi.PageIndexChanged
        Me.DGServizi.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
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
        Me.Bind_Griglia()
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
        oTableCell.Text = oResource.getValue("NONattivato")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("NONdisattivabile")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function
#End Region

    Public Function Salva_Dati() As WizardProfilo_Message
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ErroreGenerico
        Dim oProfilo As New COL_ProfiloServizio

        Try
            With oProfilo
                If Me.HDN_profiloID.Value = "" Or Me.HDN_profiloID.Value = "0" Then
                    Me.Bind_Griglia()
                    Return ModuloEnum.WizardProfilo_Message.ProfiloNonTrovato

                ElseIf Me.HDN_checkbox.Value = "" Or Me.HDN_checkbox.Value = "," Then
                    Me.Bind_Griglia()
                    Return ModuloEnum.WizardProfilo_Message.SelezionaServizio

                ElseIf Me.HDN_profiloID.Value > 0 Then
                    .Id = Me.HDN_profiloID.Value
                    .Estrai()
                    If .Errore <> Errori_Db.None Then
                        Me.Bind_Griglia()
                        Return WizardProfilo_Message.ProfiloNonTrovato
                    Else
                        .DefinisciServiziAttivati(Me.HDN_checkbox.Value)
                        If .Errore = Errori_Db.None Then
                            Me.HDN_checkbox.Value = ""
                            Me.HDN_isDefinito.Value = True
                            Me.Bind_Griglia(True)
                            Return ModuloEnum.WizardProfilo_Message.ServiziAssociati
                        Else
                            Me.Bind_Griglia()
                            Return ModuloEnum.WizardProfilo_Message.ErroreGenerico
                        End If
                    End If
                Else
                    Me.Bind_Griglia()
                    Return WizardProfilo_Message.ProfiloNonTrovato
                End If
            End With
        Catch ex As Exception
            Me.Bind_Griglia()
        End Try
        Return iResponse
    End Function

End Class