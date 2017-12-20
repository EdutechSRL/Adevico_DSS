Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.Comol.Core.File

Public Class ADM_Istituzione
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Private Enum Inserimento
        InserimentoOK = 1
        ErroreGenerico = 0
        ModificaOK = 2
        CancellazioneOK = 3
        AltroAssociato = 4
        ErroreModifica = -2
        ErroreInserimento = -1
        ErroreCancellazione = -3
        ErroreCaricamentoDati = -5
    End Enum

#Region "Menu Gestione"
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBnuovo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenuAzione As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannulla As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodifica As System.Web.UI.WebControls.LinkButton
#End Region


    Protected WithEvents TBRinserisciLogo As System.Web.UI.WebControls.TableRow
    Protected WithEvents INlogo As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents TBRmodificaLogo As System.Web.UI.WebControls.TableRow
    Protected WithEvents IMFoto As System.Web.UI.WebControls.Image
    Protected WithEvents FILElogoModifica As System.Web.UI.HtmlControls.HtmlInputFile

    Protected WithEvents BTNUploadFoto As System.Web.UI.WebControls.Button
    Protected WithEvents BTNCancella As System.Web.UI.WebControls.Button

    Protected WithEvents LBragionesociale_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBindirizzo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcap_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcitta_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBstato_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBprovincia_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtelefono1_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtelefono2_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBfax_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBhomepage_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBuniversita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlogo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlogoModifica_t As System.Web.UI.WebControls.Label

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DGIstituzione As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLlista As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnorecord As System.Web.UI.WebControls.Label
    Protected WithEvents PNLnorecord As System.Web.UI.WebControls.Panel
    Protected WithEvents TBragioneSociale As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TBindirizzo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TBcap As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator7 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Cap As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TBcitta As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents DDLprovincia As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLstato As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBtelefono1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents phoneReqVal As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Regularexpressionvalidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TBtelefono2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TBfax As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator4 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TBhomePage As System.Web.UI.WebControls.TextBox
    Protected WithEvents CHBisUniversità As System.Web.UI.WebControls.CheckBox
    Protected WithEvents PNLinsMod As System.Web.UI.WebControls.Panel

    Protected WithEvents VLDSum As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents HDistt_id As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents LBtitoloServizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

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
        If Not Page.IsPostBack Then
            Session("azione") = "load"
            Me.SetupInternazionalizzazione()
            Me.DDLstato.Attributes.Add("onchange", "return AggiornaForm();")
            Me.Bind_Dati()
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
        oResource.ResourcesName = "pg_ADMistituzione"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitoloServizio)
            Me.Master.ServiceTitle = .getValue("LBtitoloServizio.text")
            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LNBnuovo, True, True)
            .setLinkButton(Me.LNBmodifica, True, True)
            .setLinkButton(Me.LNBinserisci, True, True)
            .setLinkButton(Me.LNBannulla, True, True)
            .setHeaderDatagrid(Me.DGIstituzione, 1, "ISTT_ragioneSociale", True)
            .setHeaderDatagrid(Me.DGIstituzione, 2, "ISTT_indirizzo", True)
            .setHeaderDatagrid(Me.DGIstituzione, 3, "ISTT_citta", True)
            .setHeaderDatagrid(Me.DGIstituzione, 4, "ISTT_telefono1", True)
            .setHeaderDatagrid(Me.DGIstituzione, 5, "ISTT_homePage", True)
            .setButton(Me.BTNCancella, True, , , True)
            .setButton(Me.BTNUploadFoto, True, , , True)
            .setLabel(Me.LBcap_t)
            .setLabel(Me.LBcitta_t)
            .setLabel(Me.LBfax_t)
            .setLabel(Me.LBhomepage_t)
            .setLabel(Me.LBindirizzo_t)
            .setLabel(Me.LBlogo_t)
            .setLabel(Me.LBlogoModifica_t)
            .setLabel(Me.LBprovincia_t)
            .setLabel(Me.LBragionesociale_t)
            .setLabel(Me.LBstato_t)
            .setLabel(Me.LBtelefono1_t)
            .setLabel(Me.LBtelefono2_t)
            .setLabel(Me.LBuniversita_t)
            .setCheckBox(Me.CHBisUniversità)
        End With
    End Sub
#End Region

#Region "Bind _Dati"
    Private Sub Bind_Dati()
        Me.Bind_Stati()
        Me.Bind_Provincie(Me.DDLstato.SelectedValue)
        Bind_Griglia()
    End Sub
    Private Sub Bind_Stati()
		DDLstato.DataSource = COL_Stato.List
		DDLstato.DataTextField() = "Descrizione"
		DDLstato.DataValueField = "ID"
        DDLstato.DataBind()
		Try
			Dim oStatoItaliano As COL_Stato = COL_Stato.GetByName("Italia")
			If Not IsNothing(oStatoItaliano) Then
				DDLstato.SelectedValue = oStatoItaliano.ID
			End If
		Catch ex As Exception

		End Try

    End Sub
    Private Sub Bind_Provincie(ByVal StatoID As Integer)
        If StatoID = 193 Then
            DDLprovincia.Enabled = True
			DDLprovincia.DataSource = ManagerProvincia.List
			DDLprovincia.DataTextField() = "Nome"
			DDLprovincia.DataValueField = "ID"
            DDLprovincia.DataBind()

            Try
                Me.DDLprovincia.SelectedValue = 92
            Catch ex As Exception

            End Try
        Else
            DDLprovincia.Enabled = False
        End If
    End Sub
    Private Sub Bind_Istituzione(ByVal IstituzioneID As Integer)
        Me.HDistt_id.Value = IstituzioneID
        If IstituzioneID = 0 Then
            Me.TBragioneSociale.Text = ""
            Me.TBcap.Text = ""
            Me.TBcitta.Text = ""
            Me.TBfax.Text = ""
            Me.TBhomePage.Text = ""
            Me.TBindirizzo.Text = ""
            Me.TBtelefono1.Text = ""
            Me.TBtelefono2.Text = ""
            Me.CHBisUniversità.Checked = False
            Me.DDLstato.SelectedValue = 193
            Me.DDLprovincia.SelectedValue = 92
            Me.DDLprovincia.Enabled = True
            'Me.LBtitoloServizio.Text = Me.oResource.getValue("Titolo.Inserimento")
            Me.Master.ServiceTitle = Me.oResource.getValue("Titolo.Inserimento")

            Me.PNLlista.Visible = False
            Me.PNLinsMod.Visible = True
            Me.PNLmenu.Visible = False
            Me.PNLmenuAzione.Visible = True
            Me.LNBinserisci.Visible = True
            Me.LNBmodifica.Visible = False
            Me.TBRinserisciLogo.Visible = True
            Me.TBRmodificaLogo.Visible = False
        Else
            Dim oIstituzione As New COL_Istituzione

            With oIstituzione
                .Id = IstituzioneID
                .Estrai()
                If .Errore = Errori_Db.None Then
                    Me.TBragioneSociale.Text = .RagioneSociale
                    Me.TBcap.Text = .Cap
                    Me.TBcitta.Text = .Citta
                    Me.TBfax.Text = .Fax
                    Me.TBhomePage.Text = .HomePage
                    Me.TBindirizzo.Text = .Indirizzo
                    If .IsUniversita = True Then
                        Me.CHBisUniversità.Checked = True
                    Else
                        Me.CHBisUniversità.Checked = False
                    End If

                    DDLprovincia.SelectedValue = .Provincia.Id
                    DDLstato.SelectedValue = .Stato.Id
                    If DDLstato.SelectedValue <> "193" Then
                        Me.DDLprovincia.Enabled = False
                    End If
                    Me.TBtelefono1.Text = .Telefono1
                    Me.TBtelefono2.Text = .Telefono2

                    If .Logo = Nothing Then
                        IMFoto.Visible = True
                        IMFoto.ImageUrl = "./../images/noLogo.gif"
                    Else
                        Dim url As String = "./../images/logo/" & .Logo

                        If Exists.File(Server.MapPath(url)) Then
                            IMFoto.ImageUrl = url
                            IMFoto.Visible = True
                        Else
                            IMFoto.Visible = True
                            IMFoto.ImageUrl = "./../images/noLogo.gif"
                        End If
                    End If
                    Session("azione") = "modifica"
                    Me.TBRmodificaLogo.Visible = True
                    Me.TBRinserisciLogo.Visible = False

                    'Me.LBtitoloServizio.Text = Me.oResource.getValue("Titolo.Modifica")
                    Me.Master.ServiceTitle = Me.oResource.getValue("Titolo.Modifica")

                    Me.PNLlista.Visible = False
                    Me.PNLinsMod.Visible = True
                    Me.PNLmenu.Visible = False
                    Me.PNLmenuAzione.Visible = True
                    Me.LNBinserisci.Visible = False
                    Me.LNBmodifica.Visible = True
                Else
                    Me.Bind_Griglia()
                End If
            End With
        End If
    End Sub

    Private Sub DDLstato_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLstato.SelectedIndexChanged
        If Me.DDLstato.SelectedItem.Value <> "193" Then 'italia
            Bind_Provincie(DDLstato.SelectedItem.Text)
            DDLprovincia.SelectedValue = "0" 'estera provincia

            DDLprovincia.Enabled = False
        Else
            Bind_Provincie(DDLstato.SelectedItem.Text)
            DDLprovincia.Enabled = True
        End If
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub Bind_Griglia()
        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try
            Dim oIstituzione As New COL_Istituzione
            oDataset = oIstituzione.Elenca()

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                Me.Bind_Istituzione(0)
            Else
                Me.DGIstituzione.Visible = True

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "ISTT_ragioneSociale"
                    viewstate("SortDirection") = "desc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                DGIstituzione.DataSource = oDataview
                DGIstituzione.DataBind()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DGIstituzione_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGIstituzione.ItemCreated
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
                    If Me.DGIstituzione.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGIstituzione, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGIstituzione, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGIstituzione.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGIstituzione, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGIstituzione.HeaderStyle.CssClass
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
                    oResource.setPageDatagrid(Me.DGIstituzione, oLinkbutton)
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
                    Me.oResource.setImageButton(oImageButton, True, True, True, True)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

            Dim oImageButton2 As ImageButton
            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                Try
                    oImageButton2 = oCell.FindControl("IMBModifica")
                    Me.oResource.setImageButton(oImageButton2, True, True, True)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

        End If
    End Sub

    Sub DGIstituzione_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGIstituzione.PageIndexChanged
        DGIstituzione.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
        DGIstituzione.DataBind()
    End Sub

    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGIstituzione.SortCommand
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

    Private Sub DGIstituzione_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGIstituzione.ItemCommand
        Select Case e.CommandName
            Case "modifica"
                Try
                    Me.Bind_Istituzione(CInt(DGIstituzione.DataKeys.Item(e.Item.ItemIndex)))
                Catch ex As Exception
                    Dim alertMSG As String = ""

                    alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreCaricamentoDati, Inserimento))
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    End If

                    Me.PNLmenu.Visible = True
                    Me.PNLmenuAzione.Visible = False
                    Me.PNLinsMod.Visible = False
                    Me.PNLlista.Visible = True
                    Me.Bind_Griglia()
                End Try
            Case "elimina"
                Dim oIstituzione As New COL_Istituzione
                Dim totalePRSN, totaleORGN As Integer
                Dim alertMSG As String = ""

                oIstituzione.Id = CInt(DGIstituzione.DataKeys.Item(e.Item.ItemIndex))
                totalePRSN = oIstituzione.GetPersoneOrganizzazioniAssociate(totaleORGN)
                If totalePRSN = 0 And totaleORGN = 0 Then
                    Try
                        oIstituzione.Elimina()

                        If oIstituzione.Errore = Errori_Db.None Then
                            alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.CancellazioneOK, Inserimento))
                        Else
                            alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreCancellazione, Inserimento))
                        End If
                    Catch ex As Exception

                    End Try
                Else
                    alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.AltroAssociato, Inserimento))
                End If
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                End If
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                Me.Bind_Griglia()
        End Select
    End Sub

#End Region

    Private Sub LNBnuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBnuovo.Click
        Me.Bind_Istituzione(0)
        Session("azione") = "aggiungi"
    End Sub
    Private Sub LNBannulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Me.PNLlista.Visible = True
        Me.PNLinsMod.Visible = False
        Me.PNLmenuAzione.Visible = False
        Me.PNLmenu.Visible = True
        Me.Bind_Griglia()
        Session("azione") = "loaded"
    End Sub

    Private Sub LNBinserisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        Dim alertMSG As String = ""
        If Session("azione") = "aggiungi" Then
            Dim oIstituzione As New COL_Istituzione
            Dim isInserito As Boolean = False

            Session("azione") = "loaded"
            With oIstituzione
                .RagioneSociale = Me.TBragioneSociale.Text
                .Cap = Me.TBcap.Text
                .Citta = Me.TBcitta.Text
                .Fax = Me.TBfax.Text
                .HomePage = Me.TBhomePage.Text
                .Indirizzo = Me.TBindirizzo.Text
                .IsUniversita = Me.CHBisUniversità.Checked

                .Provincia.Id = Me.DDLprovincia.SelectedItem.Value
                .Stato.Id = Me.DDLstato.SelectedItem.Value
                .Telefono1 = Me.TBtelefono1.Text
                .Telefono2 = Me.TBtelefono2.Text
            End With
            oIstituzione.Aggiungi()

            If oIstituzione.Errore = Errori_Db.None Then
                Dim OLDpath, TempPath, NewPath, NomeFile, Estensione As String
                Dim ofile As New COL_File
                isInserito = True
                If Not Me.INlogo.Value Is "" Then 'controllo che la texbox non sia vuota
                    'cancello l'immagine vecchia se presente
                    OLDpath = Server.MapPath("./../images/Logo/temp/" & ofile.FileNameOnly(Me.INlogo.PostedFile.FileName))

                    Try
                        Delete.File_FM(OLDpath)

                        Try 'mando il file al server in una cartella temporanea

                            TempPath = Server.MapPath("./../images/logo/temp/")
                            ofile.Upload(INlogo, TempPath)

                            Try 'ridimensiono l'immagine

                                Dim strFileNameOnly As String = ofile.FileNameOnly(INlogo.PostedFile.FileName)
                                Dim data As String = Now.ToString
                                NomeFile = Left(strFileNameOnly, InStrRev(strFileNameOnly, ".") - 1)
                                Estensione = Right(strFileNameOnly, Len(strFileNameOnly) - InStrRev(strFileNameOnly, ".") + 1)
                                data = data.Replace(" ", "-")
                                data = data.Replace("/", "-")
                                data = data.Replace(".", "_")
                                data = data.Replace(":", "_")

                                NewPath = Server.MapPath("./../images/logo/" & NomeFile & "_" & data & Estensione)

                                If ofile.ResizeLogo(TempPath & strFileNameOnly, NewPath, 43, 356) < 1 Then
                                    Delete.File_FM(TempPath & strFileNameOnly)
                                    Exit Try
                                Else
                                    Delete.File_FM(TempPath & strFileNameOnly)
                                End If
                                Try
                                    'aggiorno il nome del file sul db
                                    oIstituzione.AssociaLogo(NomeFile & "_" & data & Estensione)
                                    '    Me.AggiornaApplication()
                                    'ricarico i dati della pagina
                                    Me.Bind_Istituzione(oIstituzione.Id)
                                Catch ex As Exception
                                    'errore nell'update del nome dell'immagine
                                End Try
                            Catch ex As Exception
                                'errore nel ridimensionamento 
                            End Try
                        Catch ex As Exception
                            'errore nell'upload
                        End Try
                    Catch ex As Exception
                        'errore nella cancellazione
                    End Try
                Else
                    IMFoto.Visible = False 'file non trovato su disco
                End If
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.InserimentoOK, Inserimento))
            Else
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreInserimento, Inserimento))
            End If

            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If isInserito Then
                Me.PNLmenu.Visible = True
                Me.PNLmenuAzione.Visible = False
                Me.PNLlista.Visible = True
                Me.PNLinsMod.Visible = False
                Me.Bind_Griglia()
            End If
        Else
            Me.PNLmenu.Visible = True
            Me.PNLmenuAzione.Visible = False
            Me.PNLlista.Visible = True
            Me.PNLinsMod.Visible = False
            Me.Bind_Griglia()
        End If
    End Sub

    Private Sub LNBmodifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBmodifica.Click
        Dim alertMSG As String = ""
        If Session("azione") = "modifica" Then
            Dim oIstituzione As New COL_Istituzione

            Session("azione") = "loaded"
            With oIstituzione
                .Id = Me.HDistt_id.Value
                .Estrai()

                .RagioneSociale = Me.TBragioneSociale.Text
                .Cap = Me.TBcap.Text
                .Citta = Me.TBcitta.Text
                .Fax = Me.TBfax.Text
                .HomePage = Me.TBhomePage.Text
                .Indirizzo = Me.TBindirizzo.Text
                .IsUniversita = Me.CHBisUniversità.Checked

                .Provincia.Id = Me.DDLprovincia.SelectedItem.Value
                .Stato.Id = Me.DDLstato.SelectedItem.Value
                .Telefono1 = Me.TBtelefono1.Text
                .Telefono2 = Me.TBtelefono2.Text

            End With
            oIstituzione.Modifica()
            If oIstituzione.Errore = Errori_Db.None Then
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ModificaOK, Inserimento))
            Else
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreModifica, Inserimento))
            End If

            Dim OLDpath, TempPath, NewPath, NomeFile, Estensione As String
            Dim oFile As COL_File
            If Not Me.INlogo.Value Is "" Then 'controllo che la texbox non sia vuota
                'cancello l'immagine vecchia se presente
                OLDpath = Server.MapPath("./../images/logo/" & oIstituzione.Logo)

                Try
                    Delete.File_FM(OLDpath)

                    Try 'mando il file al server in una cartella temporanea

                        TempPath = Server.MapPath("./../images/logo/temp/")
                        oFile.Upload(INlogo, TempPath)

                        Try 'ridimensiono l'immagine

                            Dim strFileNameOnly As String = oFile.FileNameOnly(INlogo.PostedFile.FileName)
                            Dim data As String = Now.ToString

                            NomeFile = Left(strFileNameOnly, InStrRev(strFileNameOnly, ".") - 1)
                            Estensione = Right(strFileNameOnly, Len(strFileNameOnly) - InStrRev(strFileNameOnly, ".") + 1)
                            data = data.Replace(" ", "-")
                            data = data.Replace("/", "-")
                            data = data.Replace(".", "_")
                            data = data.Replace(":", "_")
                            NewPath = Server.MapPath("./../images/logo/" & NomeFile & "_" & data & Estensione)

                            If oFile.ResizeLogo(TempPath & strFileNameOnly, NewPath, 43, 356) < 1 Then

                                IMFoto.Visible = False 'file non trovato su disco

                                Delete.File_FM(TempPath & strFileNameOnly)

                                Exit Try
                            Else
                                Delete.File_FM(TempPath & strFileNameOnly)
                            End If

                            Try
                                'aggiorno il nome del file sul db
                                oIstituzione.AssociaLogo(NomeFile & "_" & data & Estensione)
                                'ricarico i dati della pagina
                                ' Me.bind_datiIstituz()
                                Me.IMFoto.ImageUrl = "./../images/logo/" & NomeFile & "_" & data & Estensione
                            Catch ex As Exception
                                'errore nell'update del nome dell'immagine
                            End Try
                        Catch ex As Exception
                            'errore nel ridimensionamento 
                        End Try
                    Catch ex As Exception
                        'errore nell'upload
                    End Try
                Catch ex As Exception
                    'errore nella cancellazione
                End Try
            End If

            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If

            If oIstituzione.Errore = Errori_Db.None Then
                Me.PNLmenu.Visible = True
                Me.PNLmenuAzione.Visible = False
                Me.PNLlista.Visible = True
                Me.PNLinsMod.Visible = False
                Me.Bind_Griglia()
            End If
        Else
            Me.PNLmenu.Visible = True
            Me.PNLmenuAzione.Visible = False
            Me.PNLlista.Visible = True
            Me.PNLinsMod.Visible = False
            Me.Bind_Griglia()
        End If
    End Sub


    Private Sub BTNUploadFoto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNUploadFoto.Click
        If Me.FILElogoModifica.Value = "" Then
            Exit Sub
        End If
        If (Me.FILElogoModifica.PostedFile.FileName.ToString <> "") Then
            Dim ofile As New COL_File
            Dim oIstituzione As New COL_Istituzione
            oIstituzione.Id = HDistt_id.Value

            Dim OLDpath, TempPath, NewPath, NomeFile, Estensione As String

            If Not Me.FILElogoModifica.Value Is "" Then 'controllo che la texbox non sia vuota
                'cancello l'immagine vecchia se presente
                OLDpath = Server.MapPath("./../images/logo/" & oIstituzione.Logo)
                Try
                    Delete.File_FM(OLDpath)

                    Try 'mando il file al server in una cartella temporanea

                        TempPath = Server.MapPath("./../images/logo/temp/")
                        ofile.Upload(FILElogoModifica, TempPath)

                        Try 'ridimensiono l'immagine

                            Dim strFileNameOnly As String = ofile.FileNameOnly(FILElogoModifica.PostedFile.FileName)
                            Dim data As String = Now.ToString

                            NomeFile = Left(strFileNameOnly, InStrRev(strFileNameOnly, ".") - 1)
                            Estensione = Right(strFileNameOnly, Len(strFileNameOnly) - InStrRev(strFileNameOnly, ".") + 1)
                            data = data.Replace(" ", "-")
                            data = data.Replace("/", "-")
                            data = data.Replace(".", "_")
                            data = data.Replace(":", "_")
                            'NewPath = Server.MapPath("./../images/Organizzazione/" & NomeFile & data & Estensione)

                            NewPath = Server.MapPath("./../images/logo/" & NomeFile & data & Estensione)

                            If ofile.ResizeLogo(TempPath & strFileNameOnly, NewPath, 43, 356) < 1 Then

                                IMFoto.Visible = False 'file non trovato su disco

                                Delete.File_FM(TempPath & strFileNameOnly)

                                Exit Try
                            Else
                                Delete.File_FM(TempPath & strFileNameOnly)
                            End If

                            Try
                                'aggiorno il nome del file sul db
                                oIstituzione.AssociaLogo(NomeFile & data & Estensione)
                                '  Me.AggiornaApplication()
                                'ricarico i dati della pagina
                                Me.IMFoto.ImageUrl = "./../images/logo/" & NomeFile & data & Estensione
                                '                                Me.bind_datiIstituz()
                            Catch ex As Exception
                                'errore nell'update del nome dell'immagine
                            End Try
                        Catch ex As Exception
                            'errore nel ridimensionamento 
                        End Try
                    Catch ex As Exception
                        'errore nell'upload
                    End Try
                Catch ex As Exception
                    'errore nella cancellazione
                End Try
            End If
        End If
    End Sub

    Private Sub BTNCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCancella.Click
        Dim oIstituzione As New COL_Istituzione
        oIstituzione.Id = HDistt_id.Value
        Delete.File_FM(Server.MapPath("./../images/logo/ & oIstituzione.logo"))
        oIstituzione.AssociaLogo()
        IMFoto.ImageUrl = "./../images/noLogo.gif"
        Me.IMFoto.Visible = True
        ' Me.AggiornaApplication()
        'Me.Bind_Istituzione()
    End Sub

    'Private Sub AggiornaApplication()
    '    Dim totale, i As Integer
    '    Dim oIstituzione As New COL_Istituzione

    '    Me.Application.Lock()
    '    Try
    '        Dim oArray As String(,)
    '        Dim oDataset As New DataSet

    '        oDataset = oIstituzione.Elenca
    '        totale = oDataset.Tables(0).Rows.Count - 1
    '        For i = 0 To totale
    '            Dim oRow As DataRow

    '            oRow = oDataset.Tables(0).Rows(i)
    '            ReDim Preserve oArray(2, i)
    '            oArray(0, i) = oRow.Item("ISTT_id")
    '            If IsDBNull(oRow.Item("ISTT_Logo")) Then
    '                oArray(1, i) = "./images/noLogo.gif"
    '            Else
    '                If Exists.File(Server.MapPath("./../images/Logo/" & oRow.Item("ISTT_Logo"))) Then
    '                    oArray(1, i) = "./images/Logo/" & oRow.Item("ISTT_Logo")
    '                Else
    '                    oArray(1, i) = "./images/noLogo.gif"
    '                End If
    '            End If
    '            oArray(2, i) = oRow.Item("ISTT_homePage")
    '        Next
    '        Me.Application.Add("ArrayLogoIstituzione", oArray)
    '    Catch ex As Exception
    '        Me.Application.Add("ArrayLogoIstituzione", Nothing)
    '    End Try
    '    Me.Application.UnLock()
    'End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class