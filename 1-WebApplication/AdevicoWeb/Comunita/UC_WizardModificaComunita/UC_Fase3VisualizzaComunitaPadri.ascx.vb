Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports Telerik
Imports Telerik.WebControls


Public Class UC_Fase3VisualizzaComunitaPadri
    Inherits System.Web.UI.UserControl
    Private oResourceUCcomunita As ResourceManager


    Protected WithEvents HDN_ComunitaPadriElenco As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ComunitaAttualePadreID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ComunitaAttualeID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ComunitaAttualePercorso As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_OrganizzazioneID As System.Web.UI.HtmlControls.HtmlInputHidden

    Public ReadOnly Property ComunitaPadreID() As Integer
        Get
            Try
                ComunitaPadreID = HDN_ComunitaAttualePadreID.Value
            Catch ex As Exception
                ComunitaPadreID = 0
            End Try
        End Get
    End Property
    Public Property Width() As String
        Get
            Width = Me.TBLelencoComunita.Width.ToString
        End Get
        Set(ByVal Value As String)
            Dim isPercent As Boolean = False
            Dim isPixel As Boolean = False
            Value = LCase(Value)
            If InStr(Value, "%") > 0 Then
                isPercent = True
                Value = Left(Value, InStr(Value, "%") - 1)
            ElseIf InStr(Value, "px") > 0 Then
                isPixel = True
                Value = Left(Value, InStr(Value, "px") - 1)
            End If

            Try
                If isPercent And CInt(Value) > 0 Then
                    Me.TBLelencoComunita.Width = System.Web.UI.WebControls.Unit.Percentage(Value)
                ElseIf isPixel And CInt(Value) > 0 Then
                    Me.TBLelencoComunita.Width = System.Web.UI.WebControls.Unit.Pixel(Value)
                End If
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public ReadOnly Property isInizializzato() As Boolean
        Get
            Try
                isInizializzato = HDNhasSetup.Value
            Catch ex As Exception
                isInizializzato = False
            End Try
        End Get
    End Property

    Protected WithEvents TBLelencoComunita As System.Web.UI.WebControls.Table
    Protected WithEvents LBinfoComunitaPadri As System.Web.UI.WebControls.Label
    Protected WithEvents LBnoRecordAssociate As System.Web.UI.WebControls.Label
    Protected WithEvents DGComunitaAssociate As System.Web.UI.WebControls.DataGrid

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
        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
    End Sub

    Public Sub SetupControl(ByVal ComunitaAttualeID As Integer, ByVal ComunitaAttualePercorso As String)
        Dim oComunita As New COL_Comunita

        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        oComunita.Id = ComunitaAttualeID
        oComunita.Estrai()
        If oComunita.Errore = Errori_Db.None Then
            Me.HDN_ComunitaAttualePercorso.Value = ComunitaAttualePercorso
            Me.HDN_ComunitaAttualeID.Value = ComunitaAttualeID
            Me.HDN_ComunitaAttualePadreID.Value = oComunita.IdPadre
            Me.HDN_OrganizzazioneID.Value = oComunita.Organizzazione.Id
            Me.TBLelencoComunita.Enabled = True
            Me.Bind_Griglia()
        Else
            Me.TBLelencoComunita.Enabled = False
        End If
        Me.HDNhasSetup.Value = True

        Me.SetupInternazionalizzazione()
    End Sub
    Public Sub ResetControllo()
        Me.HDN_ComunitaAttualePercorso.Value = ""
        Me.HDN_ComunitaAttualeID.Value = ""
        Me.HDN_ComunitaPadriElenco.Value = ""
        Me.HDN_ComunitaAttualePadreID.Value = ""
        Me.HDNhasSetup.Value = False
    End Sub

#Region "Bind_Dati"

    Private Sub Bind_Griglia()
        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try
            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")

            oComunita.Id = Me.HDN_ComunitaAttualeID.Value
            oDataset = oComunita.GetPadri(Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count

            If totale = 0 Then 'se datagrid vuota
                DGComunitaAssociate.Visible = False
                Me.LBnoRecordAssociate.Visible = True
            Else
                Me.DGComunitaAssociate.Visible = True
                Me.LBnoRecordAssociate.Visible = False

                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_status"))

                If totale = 1 Then
                    Me.DGComunitaAssociate.PagerStyle.Position = PagerPosition.Top
                    Me.DGComunitaAssociate.Columns(3).Visible = False
                Else
                    Me.DGComunitaAssociate.Columns(3).Visible = True

                    If totale > 15 Then
                        Me.DGComunitaAssociate.PagerStyle.Position = PagerPosition.TopAndBottom
                    Else
                        Me.DGComunitaAssociate.PagerStyle.Position = PagerPosition.Top
                    End If
                End If

                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    'icona relativa al tipo comunità
                    oRow.Item("TPCM_Icona") = "./../../" & oRow.Item("TPCM_Icona")
                    If oRow.Item("CMNT_ID") = oComunita.Id Then
                        oRow.Item("CMNT_Nome") = oRow.Item("CMNT_Nome") & " " & Me.oResourceUCcomunita.getValue("principale")
                    End If
                    If IsDBNull(oRow.Item("Responsabile")) Then
                        oRow.Item("Responsabile") = oRow.Item("AnagraficaCreatore")
                    End If
                    If oRow.Item("CMNT_Bloccata") Then
                        oRow.Item("CMNT_status") = Me.oResourceUCcomunita.getValue("Status.Bloccata")
                    ElseIf oRow.Item("CMNT_Archiviata") Then
                        oRow.Item("CMNT_status") = Me.oResourceUCcomunita.getValue("Status.Archiviata")
                    Else
                        oRow.Item("CMNT_status") = Me.oResourceUCcomunita.getValue("Status.Attiva")
                    End If
                Next

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "CMNT_nome"
                    viewstate("SortDirection") = "asc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                DGComunitaAssociate.DataSource = oDataview
                DGComunitaAssociate.DataBind()
            End If
        Catch ex As Exception 'se c'è qualche errore nascondo la DG e mostro messaggio di errore

            DGComunitaAssociate.Visible = False
            Me.LBnoRecordAssociate.Visible = True
        End Try
    End Sub
#End Region

#Region "Griglia Comunità associate"
    Private Sub DGComunitaAssociate_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunitaAssociate.ItemCreated
        Dim i As Integer

        If IsNothing(oResourceUCcomunita) Then
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
                    If Me.DGComunitaAssociate.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResourceUCcomunita.setHeaderOrderbyLink_Datagrid(Me.DGComunitaAssociate, oLinkbutton, Main.FiltroOrdinamento.Decrescente)
                                Else
                                    oResourceUCcomunita.setHeaderOrderbyLink_Datagrid(Me.DGComunitaAssociate, oLinkbutton, Main.FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGComunitaAssociate.HeaderStyle.CssClass
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
                                oResourceUCcomunita.setHeaderOrderbyLink_Datagrid(Me.DGComunitaAssociate, oLinkbutton, Main.FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGComunitaAssociate.HeaderStyle.CssClass
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
                Dim num As Integer = 0
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                If Me.DGComunitaAssociate.Columns(3).Visible Then
                    num += 1
                End If
                num += 2
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
                    oResourceUCcomunita.setPageDatagrid(Me.DGComunitaAssociate, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"
            Try
                If e.Item.DataItem("CMNT_ID") = Me.ComunitaPadreID Then
                    e.Item.CssClass = "ROW_Selezionate_Small"
                ElseIf CBool(e.Item.DataItem("CMNT_Bloccata")) = True Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("CMNT_Archiviata")) = True Then
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
                Dim oLBcomunita, oLBresponsabile As Label

                oLBcomunita = e.Item.Cells(1).FindControl("LBcomunita")
                If IsNothing(oLBcomunita) = False Then
                    oLBcomunita.CssClass = cssRiga
                End If
                oLBresponsabile = e.Item.Cells(1).FindControl("LBresponsabile")
                If IsNothing(oLBresponsabile) = False Then
                    oLBresponsabile.CssClass = cssRiga
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oBTNremove As Button

                oBTNremove = e.Item.Cells(1).FindControl("BTNremove")
                If IsNothing(oBTNremove) = False Then
                    If e.Item.DataItem("CMNT_ID") = Me.ComunitaPadreID Then
                        oBTNremove.Visible = False
                    Else
                        oBTNremove.Visible = True
                    End If
                    Me.oResourceUCcomunita.setButton(oBTNremove, True, , True, True)
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub SortElencoAssociate(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGComunitaAssociate.SortCommand
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

    Private Sub DGComunitaAssociate_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGComunitaAssociate.ItemCommand
        If e.CommandName = "remove" Then
            Dim oComunita As New COL_Comunita
            Dim Percorso As String = ""
            Dim pageUtility As New OLDpageUtility(Context)
            Percorso = Me.HDN_ComunitaAttualePercorso.Value
            'If Percorso <> "" Then
            '    Percorso = Replace(Me.HDN_ComunitaAttualePadreID.Value, "." & Me.HDN_ComunitaAttualeID.Value & ".", ".")
            'End If
            oComunita.Id = Me.HDN_ComunitaAttualeID.Value
            oComunita.RimuoviCollegamentoPadre(e.CommandArgument, False, Percorso, PageUtility.ProfilePath)
        End If
        Me.Bind_Griglia()
    End Sub
#End Region

    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Selezionate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceUCcomunita.getValue("principale")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceUCcomunita.getValue("bloccate")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceUCcomunita.getValue("archiviate")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        oResourceUCcomunita = New ResourceManager

        oResourceUCcomunita.UserLanguages = code
        oResourceUCcomunita.ResourcesName = "pg_UC_Fase3VisualizzaComunitaPadri"
        oResourceUCcomunita.Folder_Level1 = "Comunita"
        oResourceUCcomunita.Folder_Level2 = "UC_WizardComunita"
        oResourceUCcomunita.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        With oResourceUCcomunita
            .setLabel(LBinfoComunitaPadri)
            .setLabel(Me.LBnoRecordAssociate)
        End With
    End Sub
#End Region

End Class