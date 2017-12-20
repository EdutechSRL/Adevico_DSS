Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices



Public Class Management_ProfiliServizi
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents DGprofilo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoRecord As System.Web.UI.WebControls.Label

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


        If Page.IsPostBack = False Then
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")
            Me.SetupInternazionalizzazione()

            Me.PNLpermessi.Visible = False
            Me.PNLcontenuto.Visible = True
            Me.Bind_Griglia()
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

#Region "Bind_Dati"

    Private Sub Bind_Griglia()
        Dim oDataset As New DataSet
        Dim i, totale As Integer
        Dim oProfilo As New COL_ProfiloServizio

        Try
            oDataset = oProfilo.Elenca(Session("objPersona").id, Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count

            If totale > 0 Then
                Me.DGprofilo.Visible = True
                Me.LBnoRecord.Visible = False
                oDataset.Tables(0).Columns.Add(New DataColumn("ProfiloNome"))
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    Dim isModificato As Boolean = False

                    oRow = oDataset.Tables(0).Rows(i)
                    oRow.Item("ProfiloNome") = "<b>" & oRow.Item("PRFS_Nome") & "</b>"

                    Try
                        If oRow.Item("PRFS_CreatoIl") <> oRow.Item("PRFS_modificatoIl") Then
                            isModificato = True
                        End If
                    Catch ex As Exception

                    End Try
                  
                    If oRow.Item("PRFS_CreatoreID") <> oRow.Item("PRFS_PRSN_ID") Then
                        If isModificato Then
                            oRow.Item("ProfiloNome") &= Me.oResource.getValue("profiloModificatoDa")
                            Try
                                oRow.Item("ProfiloNome") = Replace(oRow.Item("ProfiloNome"), "#data#", CDate(oRow.Item("PRFS_modificatoIl")).ToString("dd/MM/yy HH:mm"))
                            Catch ex As Exception

                            End Try
                            oRow.Item("ProfiloNome") = Replace(oRow.Item("ProfiloNome"), "#da#", oRow.Item("Modifica"))
                        End If
                    Else
                        If isModificato Then
                            oRow.Item("ProfiloNome") &= Me.oResource.getValue("profiloModificato")
                            Try
                                oRow.Item("ProfiloNome") = Replace(oRow.Item("ProfiloNome"), "#data#", CDate(oRow.Item("PRFS_modificatoIl")).ToString("dd/MM/yy HH:mm"))
                            Catch ex As Exception

                            End Try

                        End If
                    End If
                Next
                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "TPCM_Descrizione"
                    viewstate("SortDirection") = ""
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                If totale <= Me.DGprofilo.PageSize Then
                    Me.DGprofilo.PagerStyle.Position = PagerPosition.Top
                Else
                    Me.DGprofilo.PagerStyle.Position = PagerPosition.TopAndBottom
                End If
                Me.DGprofilo.DataSource = oDataview
                Me.DGprofilo.DataBind()

            Else
                Me.DGprofilo.Visible = False
                Me.LBnoRecord.Visible = True
            End If
        Catch ex As Exception
            Me.DGprofilo.Visible = False
            Me.LBnoRecord.Visible = True
        End Try
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGprofilo.SortCommand
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
    Protected Sub DGprofilo_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGprofilo.PageIndexChanged
        Me.DGprofilo.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub
    Private Sub DGprofilo_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGprofilo.ItemCreated
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
                    If Me.DGprofilo.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGprofilo, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGprofilo, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGprofilo.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGprofilo, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGprofilo.HeaderStyle.CssClass
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
                    oResource.setPageDatagrid(Me.DGprofilo, oLinkbutton)
                End Try
            Next
            e.Item.Cells(0).Attributes.Item("colspan") = "3"
        End If
    End Sub
    Private Sub DGprofilo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGprofilo.ItemDataBound
        Dim i As Integer
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")

            ' Cancellazione iscrizione utente.........
            Dim oImagebutton As ImageButton
            Try
                oImagebutton = e.Item.Cells(0).FindControl("IMBelimina")
                If Not IsNothing(oImagebutton) Then
                    oResource.setImageButton_Datagrid(Me.DGprofilo, oImagebutton, "IMBelimina", oImagebutton.Enabled, True, True, True)
                End If
            Catch ex As Exception

            End Try

            'Modifica ruolo
            Try
                oImagebutton = e.Item.Cells(0).FindControl("IMBmodifica")
                If Not IsNothing(oImagebutton) Then
                    oResource.setImageButton_Datagrid(Me.DGprofilo, oImagebutton, "IMBmodifica", True, True, True)
                End If
            Catch ex As Exception

            End Try


            Try
                Dim oLBdescrizione As Label
                Dim oTBRdescrizione As TableRow

                oLBdescrizione = e.Item.Cells(0).FindControl("LBdescrizione")
                oTBRdescrizione = e.Item.Cells(0).FindControl("TBRdescrizione")
                oTBRdescrizione.Visible = False

                Try
                    If e.Item.DataItem("PRFS_Descrizione") <> "" Then
                        oTBRdescrizione.Visible = True
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try


            Try
                Dim oLNBdefinisciRuoli, oLNDdefinisciServizi, oLNBdefinisciPermessi As LinkButton
                oLNBdefinisciRuoli = e.Item.Cells(0).FindControl("LNBdefinisciRuoli")
                oLNDdefinisciServizi = e.Item.Cells(0).FindControl("LNDdefinisciServizi")
                oLNBdefinisciPermessi = e.Item.Cells(0).FindControl("LNBdefinisciPermessi")
                oResource.setLinkButton(oLNBdefinisciRuoli, True, True)
                oResource.setLinkButton(oLNDdefinisciServizi, True, True)
                oResource.setLinkButton(oLNBdefinisciPermessi, True, True)
                oLNBdefinisciRuoli.CssClass = cssLink
                oLNDdefinisciServizi.CssClass = cssLink
                oLNBdefinisciPermessi.CssClass = cssLink

            Catch ex As Exception

            End Try


        End If
    End Sub
    Private Sub DGprofilo_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGprofilo.ItemCommand
        Dim ProfiloID As Integer
        Try
            ProfiloID = source.Items(e.Item.ItemIndex).Cells(0).Text()
        Catch ex As Exception
            ProfiloID = -1
        End Try

        Dim oPageUtility As New OLDpageUtility(HttpContext.Current)
        Dim LinkElenco As String = oPageUtility.ApplicationUrlBase() & "Comunita/Wizard_ProfiliServizi.aspx"

        Select Case e.CommandName
            Case "modifica"
                Session("Azione") = "modifica_" & ProfiloID
                Session("Azione_selezione") = ""
                Response.Redirect(LinkElenco, True)
            Case "elimina"
                Dim oProfilo As New COL_ProfiloServizio
                With oProfilo
                    .Id = ProfiloID
                    .Estrai()
                    If .Errore = Errori_Db.None Then
                        .Cancella()
                    End If
                End With
                Me.Bind_Griglia()
            Case "ruoli"
                Session("Azione") = "modifica_" & ProfiloID
                Session("Azione_selezione") = "AssociaRuoli"
                Response.Redirect(LinkElenco, True)
            Case "servizi"
                Session("Azione") = "modifica_" & ProfiloID
                Session("Azione_selezione") = "AssociaServizi"
                Response.Redirect(LinkElenco, True)
            Case "permessi"
                Session("Azione") = "modifica_" & ProfiloID
                Session("Azione_selezione") = "DefinisciPermessi"
                Response.Redirect(LinkElenco, True)
            Case Else
                Me.Bind_Griglia()
        End Select
    End Sub
#End Region

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = code
        oResource.ResourcesName = "pg_Management_ProfiliServizi"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LNBinserisci, True, True)
            .setLabel(Me.LBnoRecord)
            .setHeaderDatagrid(Me.DGprofilo, 2, "tipoComunita", True)
            .setHeaderDatagrid(Me.DGprofilo, 3, "profilo", True)
        End With
    End Sub
#End Region

    Private Sub LNBinserisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        Session("Azione") = "inserisci"
        Session("Azione_selezione") = ""
        Response.Redirect("./Wizard_ProfiliServizi.aspx")
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class