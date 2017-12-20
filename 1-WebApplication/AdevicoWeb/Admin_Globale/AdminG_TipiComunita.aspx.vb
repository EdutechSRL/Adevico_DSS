Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona


Public Class AdminG_TipiComunita
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager
    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

#Region "Menu"
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenuAzione As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBsalvaDati As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBaddRuolo As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LNBdefault As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBtipocomunitaForAll As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsetToAllCommunity As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Modifica"
    Protected WithEvents PNLmodifica As System.Web.UI.WebControls.Panel
    Protected WithEvents CTRLmodifica As UC_modificaTipoComunita

    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
#End Region

#Region "Pannello Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Pannello Lista"
    Protected WithEvents PNLlista As System.Web.UI.WebControls.Panel
    Protected WithEvents DGtipoComunita As System.Web.UI.WebControls.DataGrid

#End Region

#Region "Pannello NoRecord"
    Protected WithEvents PNLnoRecord As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnorecord As System.Web.UI.WebControls.Label
#End Region
    Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden

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
            SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If


        If Not Page.IsPostBack Then
            Session("azione") = "load"
            oPersona = Session("objPersona")

            Me.SetupInternazionalizzazione()
			If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
				Me.PNLcontenuto.Visible = True
				Me.PNLmenu.Visible = True
				Me.PNLpermessi.Visible = False
				Bind_Griglia()
			Else
				Me.PNLcontenuto.Visible = False
				Me.PNLpermessi.Visible = True
			End If
        Else
            If Me.HDNazione.Value = "reload" Then
                Me.CTRLmodifica.AggiornaRuoli()
                Me.HDNazione.Value = "gestioneTipo"
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

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_AdminG_TipiComunita"
        oResource.Folder_Level1 = "Admin_globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(LBNopermessi)
            .setLabel(Me.LBnorecord)
            .setLinkButton(Me.LNBinserisci, True, True)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLinkButton(Me.LNBsalvaDati, True, True)
            .setLinkButton(Me.LNBaddRuolo, True, True)
            .setLinkButton(Me.LNBdefault, True, True)
            .setLinkButton(Me.LNBtipocomunitaForAll, True, True)
            .setHeaderDatagrid(Me.DGtipoComunita, 1, "TPCM_descrizione", True)
            .setHeaderDatagrid(Me.DGtipoComunita, 2, "Comunita", True)

            Dim i_link As String
            i_link = "./AdminG_AggiungiTipoRuolo.aspx"
            'If Request.Browser.Browser = "IE" And Request.Browser.MajorVersion >= 5 Then
            '    Dim quote As String = """"
            '    Me.LNBaddRuolo.Attributes.Add("onclick", "window.showModalDialog(" & quote & i_link & quote & ",self," & quote & "dialogWidth:600px;dialogHeight:700px;center:1;scroll:1;help:0;status:0" & quote & ");return false;")
            '    'Me.LNBaddRuolo.Attributes.Add("onclick", "OpenWin('" & i_link & "','600','500','no','no');return false;")
            'Else
            Me.LNBaddRuolo.Attributes.Add("onclick", "OpenWin('" & i_link & "','600','700','no','no');return false;")
            ' End If
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.CategoriaFile).Text = .getValue("TABcategoriaFile.Text")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.CategoriaFile).ToolTip = .getValue("TABcategoriaFile.ToolTip")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.Modello).Text = .getValue("TABmodelli.Text")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.Modello).ToolTip = .getValue("TABmodelli.ToolTip")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.Permessi).Text = .getValue("TABpermessi.Text")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.Permessi).ToolTip = .getValue("TABpermessi.ToolTip")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.Servizi).Text = .getValue("TABservizio.Text")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.Servizi).ToolTip = .getValue("TABservizio.ToolTip")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.DatiPrincipali).Text = .getValue("TABtipologia.Text")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.DatiPrincipali).ToolTip = .getValue("TABtipologia.ToolTip")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.TipoRuolo).Text = .getValue("TABtipoRuolo.Text")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.TipoRuolo).ToolTip = .getValue("TABtipoRuolo.ToolTip")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.RuoliProfili).Text = .getValue("TABruoliProfilo.Text")
            TBSmenu.Tabs(Me.CTRLmodifica.IndiceSelezionato.RuoliProfili).ToolTip = .getValue("TABruoliProfilo.ToolTip")

        End With
    End Sub
#End Region


#Region "Bind_Dati"
    Function Bind_Griglia()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try

            'carico la lista dei tipicomunità
            oDataset = oTipoComunita.Elenca(Session("LinguaID"))

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                ' al posto della datagrid mostro un messaggio!
                Me.PNLlista.Visible = False
                Me.PNLnoRecord.Visible = True
            Else
                oDataset.Tables(0).Columns.Add("ComunitaAssociate")
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If oRow.Item("associata") = 0 Then
                        oRow.Item("ComunitaAssociate") = Me.oResource.getValue("No")
                    Else
                        oRow.Item("ComunitaAssociate") = Me.oResource.getValue("Si")
                    End If
                    If totale = 1 Then
                        oRow.Item("totale") = -1
                    End If
                Next
                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "TPCM_descrizione"
                    viewstate("SortDirection") = "asc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                Me.DGtipoComunita.DataSource = oDataview
                Me.DGtipoComunita.DataBind()
            End If
        Catch ex As Exception 'se c'è qualche errore nascondo la DG e mostro messaggio di errore
            Me.PNLlista.Visible = False
            Me.PNLnoRecord.Visible = True
            Me.LBnorecord.Text = "Nessun elemento"
        End Try
    End Function

    'Public Sub Bind_TipoComunita(Optional ByVal CTGR_ID As Integer = -1)
    '    Dim oCategoria As COL_CategoriaFile
    '    Dim dataset As New DataSet
    '    Dim Totale, i As Integer

    '    Try
    '        dataset = oCategoria.GetTipoComunitaAssociateNonAssociate(CTGR_ID)
    '        Totale = dataset.Tables(0).Rows.Count
    '        If Totale > 0 Then
    '            Me.CHLtipoComunita.DataSource = dataset
    '            Me.CHLtipoComunita.DataTextField() = "TPCM_descrizione"
    '            Me.CHLtipoComunita.DataValueField = "TPCM_Id"
    '            Me.CHLtipoComunita.DataBind()

    '            For i = 0 To Totale - 1
    '                Dim oRow As DataRow
    '                oRow = dataset.Tables(0).Rows(i)

    '                If oRow.Item("Associato") = 1 Then
    '                    If Me.HDassociato.Value = "" Then
    '                        Me.HDassociato.Value = "," & oRow.Item("TPCM_id") & ","
    '                    Else
    '                        Me.HDassociato.Value += oRow.Item("TPCM_id") & ","
    '                    End If
    '                    Me.CHLtipoComunita.Items.Item(i).Selected = True
    '                End If
    '            Next

    '            Me.BTNinserisci.Enabled = True
    '            Me.BTNmodifica.Enabled = True
    '        Else
    '            Me.BTNinserisci.Enabled = False
    '            Me.BTNmodifica.Enabled = False
    '            Me.HDassociato.Value = ""
    '        End If
    '    Catch ex As Exception
    '        Me.HDassociato.Value = ""
    '        Me.BTNinserisci.Enabled = False
    '        Me.BTNmodifica.Enabled = False
    '    End Try
    'End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGtipoComunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGtipoComunita.ItemCreated
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
                    If Me.DGtipoComunita.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGtipoComunita, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGtipoComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGtipoComunita.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGtipoComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGtipoComunita.HeaderStyle.CssClass
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
                    oResource.setPageDatagrid(Me.DGtipoComunita, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim oImageButton As ImageButton
            Dim oCell As New TableCell
            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                Try
                    oImageButton = oCell.FindControl("IMBCancella")
                    oImageButton.Attributes.Add("onclick", "window.status='Elimina';return confirm('Sei sicuro di cancellare questo Tipo Comunità?');")
                    Dim NoDelete As Boolean

                    NoDelete = e.Item.DataItem("TPCM_noDelete")


                    If NoDelete Or (NoDelete = False And e.Item.DataItem("ComunitaAssociate").CompareTo("No")) Then
                        oImageButton.ToolTip = "Non è possibile eliminare il Tipo Comunità."
                        oImageButton.ImageUrl = "./../images/x_d.gif"
                        oImageButton.Enabled = False
                    Else
                        oImageButton.ToolTip = "Elimina il Tipo Comunità."
                        oImageButton.ImageUrl = "./../images/x.gif"
                        oImageButton.Enabled = True
                    End If

                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

        End If
    End Sub
    Private Sub DGtipoComunita_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGtipoComunita.PageIndexChanged
        Me.DGtipoComunita.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGtipoComunita.SortCommand
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

    Private Sub DGtipoComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGtipoComunita.ItemCommand
        Dim TPCM_ID As Integer


        Try
            TPCM_ID = CInt(Me.DGtipoComunita.DataKeys.Item(e.Item.ItemIndex))

            Select Case e.CommandName
                Case "modifica"
                    Session("azione") = "modifica"
                    Me.PNLmenu.Visible = False
                    Me.PNLmenuAzione.Visible = True
                    Me.PNLmodifica.Visible = True
                    Me.PNLnoRecord.Visible = False
                    Me.PNLlista.Visible = False
                    Me.TBSmenu.SelectedIndex = 0
                    Me.CTRLmodifica.Setup_Controllo(TPCM_ID, UC_modificaTipoComunita.IndiceSelezionato.DatiPrincipali)
                Case "elimina"
                    Try

                        Dim oTipoComunita As New COL_Tipo_Comunita
                        oTipoComunita.ID = TPCM_ID
                        oTipoComunita.Elimina()
                        Bind_Griglia()
                    Catch ex As Exception

                    End Try
            End Select
        Catch ex As Exception

        End Try
    End Sub

#End Region


    Private Sub LNBinserisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        Me.Response.Redirect("./AdminG_WizardTipoComunita.aspx")
    End Sub

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Me.LNBsetToAllCommunity.Visible = False
        Me.LNBaddRuolo.Visible = False
        Me.LNBdefault.Visible = False
        Me.LNBtipocomunitaForAll.Visible = False
        Me.LNBsalvaDati.Enabled = True
        Me.CTRLmodifica.CambioElemento(Me.TBSmenu.SelectedIndex)
        If Me.TBSmenu.SelectedIndex = Me.CTRLmodifica.IndiceSelezionato.TipoRuolo Then
            Me.LNBaddRuolo.Visible = True
        ElseIf Me.TBSmenu.SelectedIndex = Me.CTRLmodifica.IndiceSelezionato.Servizi Then
            Me.LNBdefault.Visible = False
            If Me.CTRLmodifica.TipoComunitaID <> -2 Then
                Me.LNBtipocomunitaForAll.Visible = True
            Else
                Me.LNBtipocomunitaForAll.Visible = False
            End If

        ElseIf Me.TBSmenu.SelectedIndex = Me.CTRLmodifica.IndiceSelezionato.Permessi Then
            '         Me.LNBdefault.Visible = False
            'if me.CTRLmodifica.TipoComunitaID <> -2 then
            '	Me.LNBtipocomunitaForAll.Visible = True
            'else
            '	Me.LNBtipocomunitaForAll.Visible = False 
            'End If
        End If
    End Sub

    Private Sub LNBsalvaDati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaDati.Click
        Dim iResponse As UC_modificaTipoComunita.Inserimento
        Dim alertMSG As String = ""
		iResponse = Me.CTRLmodifica.SalvaDati(Me.TBSmenu.SelectedIndex)

        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, UC_modificaTipoComunita.Inserimento))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If

        'If iResponse = UC_modificaTipoComunita.Inserimento.Creato Or iResponse = UC_modificaTipoComunita.Inserimento.Modificato Then
        '    Session("Azione") = "loaded"
        '    Me.PNLmenu.Visible = True
        '    Me.PNLmenuAzione.Visible = False
        '    Me.PNLlista.Visible = True
        '    Me.PNLmodifica.Visible = False
        '    Me.Bind_Griglia()
        'End If
    End Sub

    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.LNBtipocomunitaForAll.Visible = False
        Me.LNBaddRuolo.Visible = False
        Me.LNBdefault.Visible = False
        Me.PNLmodifica.Visible = False
        Me.PNLmenuAzione.Visible = False
        Me.PNLmenu.Visible = True
        Me.PNLlista.Visible = True
        Me.Bind_Griglia()
    End Sub

    Private Sub CTRLmodifica_AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean, ByVal displaySetToAllCommunities As Boolean) Handles CTRLmodifica.AggiornaMenuServizi
        Me.LNBtipocomunitaForAll.Visible = AttivaDefault
        Me.LNBdefault.Visible = Not AttivaDefault
        Me.LNBsetToAllCommunity.Visible = displaySetToAllCommunities
        Me.LNBsetToAllCommunity.Enabled = Abilitato
        Me.LNBtipocomunitaForAll.Enabled = Abilitato
        Me.LNBdefault.Enabled = Abilitato
        Me.LNBsalvaDati.Enabled = Abilitato
    End Sub

    Private Sub LNBtipocomunitaForAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBtipocomunitaForAll.Click
        Dim iResponse As UC_modificaTipoComunita.Inserimento
        Dim alertMSG As String = ""
        iResponse = Me.CTRLmodifica.SalvaDati(Me.TBSmenu.SelectedIndex, True, False)

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, UC_modificaTipoComunita.Inserimento))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If
    End Sub

    Private Sub LNBdefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdefault.Click
        Dim iResponse As UC_modificaTipoComunita.Inserimento
        Dim alertMSG As String = ""
        iResponse = Me.CTRLmodifica.SalvaDati(Me.TBSmenu.SelectedIndex, False, True)
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, UC_modificaTipoComunita.Inserimento))
        If alertMSG <> "" AndAlso HasErrors(iResponse) Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If
    End Sub
    Private Sub LNBsetToAllCommunity_Click(sender As Object, e As System.EventArgs) Handles LNBsetToAllCommunity.Click
        Me.CTRLmodifica.SalvaDati(Me.TBSmenu.SelectedIndex, False, False, True)
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property

    Private Function HasErrors(message As UC_modificaTipoComunita.Inserimento) As Boolean
        Select Case message
            Case UC_modificaTipoComunita.Inserimento.Creato, UC_modificaTipoComunita.Inserimento.Modificato, UC_modificaTipoComunita.Inserimento.PermessiAssociati, UC_modificaTipoComunita.Inserimento.ServiziAssociati, UC_modificaTipoComunita.Inserimento.ServiziDefault
                Return False
            Case Else
                Return True
        End Select
    End Function
   
End Class
