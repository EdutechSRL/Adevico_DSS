Imports Comunita_OnLine.ModuloGenerale
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class CurriculumIscrittiComunita
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum



#Region "Filtri"
    Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBvalore As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBtipoRuolo_t As System.Web.UI.WebControls.Label

    Protected WithEvents DDLtipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNcerca As System.Web.UI.WebControls.Button

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
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
#End Region
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents TBRpersone As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRnorecord As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBnoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents DGpersona As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLvetrina As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcurriculum As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LKBtornaLista As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBnumeroRecord As System.Web.UI.WebControls.Label
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LKBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBAvanti As System.Web.UI.WebControls.LinkButton

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

#Region "Struttura"
  Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
    Private Enum TabIndex
        Dati = 0
        Competenze = 1
        Formazione = 2
        Lingua = 3
        Esperienze = 4
    End Enum

    Protected WithEvents TBRdati As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRformazione As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRlingua As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBResperienze As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcompetenze As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLdati As Comunita_OnLine.UC_infoDatiCurriculum
    Protected WithEvents CTRLformazione As Comunita_OnLine.UC_infoFormazione
    Protected WithEvents CTRLlingua As Comunita_OnLine.UC_infoConoscenzaLingua
    Protected WithEvents CTRLlavoro As Comunita_OnLine.UC_infoEsperienzeLavorative
    Protected WithEvents CTRLcompetenze As Comunita_OnLine.UC_infoCompetenze
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
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
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
            Me.Bind_Filtri()
            Me.ViewState("intCurPage") = 0
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
            SetupJavascript()

            BindGriglia(True)
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
            oPersona = Nothing
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Response.End()
            Return True
        End If
    End Function

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_CurriculumIscrittiComunita"
        oResource.Folder_Level1 = "Curriculum"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(Me.LBtipoRuolo_t)
            .setLabel(LBtipoRicerca_t)
            .setDropDownList(DDLtipoRicerca, -1)
            .setDropDownList(DDLtipoRicerca, 1)
            .setDropDownList(DDLtipoRicerca, 2)
            .setLabel(LBvalore_t)
            .setButton(BTNcerca)
            .setLinkButton(LKBaltro, True, False)
            .setLinkButton(LKBtutti, True, False)

            oResource.setHeaderDatagrid(Me.DGpersona, 0, "PRSN_Cognome", True)
            oResource.setHeaderDatagrid(Me.DGpersona, 1, "PRSN_Nome", True)
            oResource.setHeaderDatagrid(Me.DGpersona, 2, "TipoRuolo", True)
            oResource.setHeaderDatagrid(Me.DGpersona, 3, "Facolta", True)
            'manca il linkbuttonn
            .setLabel(LBnoRecord)
            TBSmenu.Tabs(0).Text = .getValue("TABdati.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("TABdati.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("TABcompetenze.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("TABcompetenze.ToolTip")
            TBSmenu.Tabs(2).Text = .getValue("TABformazione.Text")
            TBSmenu.Tabs(2).ToolTip = .getValue("TABformazione.ToolTip")
            TBSmenu.Tabs(3).Text = .getValue("TABlingua.Text")
            TBSmenu.Tabs(3).ToolTip = .getValue("TABlingua.ToolTip")
            TBSmenu.Tabs(4).Text = .getValue("TABesperienze.Text")
            TBSmenu.Tabs(4).ToolTip = .getValue("TABesperienze.ToolTip")
            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)

            .setLinkButton(Me.LKBindietro, True, True, False, False)
            .setLinkButton(Me.LKBAvanti, True, True, False, False)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub Bind_Filtri()
        Me.Bind_TipoRuolo()
    End Sub
    Private Sub Bind_TipoRuolo()
        Dim oDataset As DataSet
        Dim oTipoRuolo As New COL_TipoRuolo
        Dim oListItem As New ListItem

        Try
            Dim CMNT_ID As Integer
            If Session("AdminForChange") = False Then
                CMNT_ID = Session("IdComunita")
            Else
                CMNT_ID = Session("idComunita_forAdmin")
            End If


            oDataset = oTipoRuolo.ElencaConCurriculum(CMNT_ID, Session("LinguaID"), Main.FiltroVisibilità.Pubblici)
            DDLtipoRuolo.Items.Clear()
            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                DDLtipoRuolo.DataSource = oDataview
                DDLtipoRuolo.DataTextField() = "TPRL_nome"
                DDLtipoRuolo.DataValueField() = "TPRL_id"
                DDLtipoRuolo.DataBind()

                If DDLtipoRuolo.Items.Count > 1 Then
                    DDLtipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
                End If
            Else
                DDLtipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLtipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        oResource.setDropDownList(Me.DDLtipoRuolo, -1)
    End Sub

    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet

        Try
            Dim oPersona As New COL_Persona
            Dim oOrganizzazione As New COL_Organizzazione
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento
            Dim oFiltroAnagrafica As Main.FiltroAnagrafica
            Dim oCampoOrdinePersona As Main.FiltroCampoOrdinePersona
            Dim CMNT_ID As Integer
            If Session("AdminForChange") = False Then
                CMNT_ID = Session("IdComunita")
            Else
                CMNT_ID = Session("idComunita_forAdmin")
            End If
            If viewstate("SortExspression") = "" Or LCase(viewstate("SortExspression")) = "PRSN_Cognome" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.anagrafica
            ElseIf LCase(viewstate("SortExspression")) = "prsn_login" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.login
            Else
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.anagrafica
            End If

            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            'definisco il filtraggio per lettera !
            Try
                If Me.ViewState("intAnagrafica") = "" Then
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Else
                    oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.ViewState("intAnagrafica") = CType(Main.FiltroAnagrafica.tutti, Main.FiltroAnagrafica)
            End Try

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
            End Try
            Dim valore As String
            Dim oFiltroPersona As Main.FiltroPersona

            oFiltroPersona = Main.FiltroPersona.tutte

            Select Case Me.DDLtipoRicerca.SelectedValue
                Case Main.FiltroPersona.tutte
                    valore = ""
                Case Main.FiltroPersona.nome
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        oFiltroPersona = Main.FiltroPersona.nome
                    End If
                Case Main.FiltroPersona.cognome
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        oFiltroPersona = Main.FiltroPersona.cognome
                    End If
                Case Main.FiltroPersona.login
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        oFiltroPersona = Main.FiltroPersona.login
                    End If
                Case Else
                    valore = ""
                    oFiltroPersona = Main.FiltroPersona.tutte
            End Select

            If oFiltroPersona <> Main.FiltroPersona.tutte Then
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.ViewState("intAnagrafica") = CType(Main.FiltroAnagrafica.tutti, Main.FiltroAnagrafica)
            End If

            Dim totale As Decimal
            If CMNT_ID > 0 Then
                If ricalcola Then
                    Me.ViewState("intCurPage") = 0
                    Me.DGpersona.CurrentPageIndex = 0
                End If
                Return oPersona.GetPersoneCurriculumByComunita(CMNT_ID, Me.DGpersona.PageSize, Me.ViewState("intCurPage"), Me.DDLtipoRuolo.SelectedValue, oFiltroAnagrafica, oFiltroOrdinamento, oCampoOrdinePersona, oFiltroPersona, valore, Main.FiltroVisibilità.Pubblici)
            Else
                Me.ViewState("intCurPage") = 0
                Me.DGpersona.VirtualItemCount = 0
                Me.DGpersona.CurrentPageIndex = 0
                Return oDataset
            End If
        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        'oLink = Me.FindControl("LKB" & Lettera)
        
        oLink = FindControlRecursive(Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub

    Public Sub BindGriglia(Optional ByVal ricalcola As Boolean = False)
        Dim dstable As New DataSet

        dstable = FiltraggioDati(ricalcola)

        Try
            Dim i, pos, TotaleRecord As Integer
            TotaleRecord = dstable.Tables(0).Rows.Count

            If TotaleRecord > 0 Then
                Me.DGpersona.VirtualItemCount = dstable.Tables(0).Rows(0).Item("Totale")
                If Me.DDLNumeroRecord.Items(0).Value < Me.DGpersona.VirtualItemCount Then
                    Me.LBnumeroRecord.Visible = True
                    Me.DDLNumeroRecord.Visible = True
                    Me.DGpersona.AllowPaging = True
                Else
                    Me.LBnumeroRecord.Visible = False
                    Me.DDLNumeroRecord.Visible = False
                    Me.DGpersona.AllowPaging = False
                End If
            Else
                Me.LBnumeroRecord.Visible = False
                Me.DDLNumeroRecord.Visible = False
            End If
            If TotaleRecord > 0 Then
                ' dstable.Tables(0).Columns.Add(New DataColumn("oHasCurriculum"))
                'For i = 0 To TotaleRecord - 1
                '    Dim oRow As DataRow
                '    oRow = dstable.Tables(0).Rows(i)
                '    If IsDBNull(oRow.Item("PRSN_Ricevimento")) = False Then
                '        oRow.Item("oRicevimento") = oRow.Item("PRSN_Ricevimento")
                '    Else
                '        oRow.Item("oRicevimento") = oResource.getValue("Nonspecificato") '"Non specificato"
                '    End If
                ' Next


                '        Me.PNLnorecord.Visible = False
                ' Mod_Visualizzazione(TotaleRecord - 1)

                Dim oDataview As DataView
                oDataview = dstable.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "PRSN_Cognome"
                    viewstate("SortDirection") = "asc"
                End If

                Me.TBRpersone.Visible = True
                TBRnorecord.Visible = False

                Me.DGpersona.DataSource = dstable
                Me.DGpersona.DataBind()
            Else
                Me.TBRpersone.Visible = False
                TBRnorecord.Visible = True
            End If
        Catch ex As Exception
            TBRpersone.Visible = False
            TBRnorecord.Visible = True
        End Try
    End Sub
#End Region

    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.BindGriglia(True)
    End Sub

#Region "Gestione Griglia"
    ' Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
    'Me.PNLpaginazione.Visible = False
    'If oRecord > Me.DGpersona.PageSize Or oRecord > 10 Or Me.DGpersona.VirtualItemCount > Me.DGpersona.PageSize Then
    '    Me.DGpersona.AllowPaging = True
    '    Me.DGpersona.PageSize = Me.DDLpaginazione.SelectedItem.Value
    '    PNLpaginazione.Visible = True
    'Else
    '    Me.DGpersona.AllowPaging = False
    '    PNLpaginazione.Visible = False
    'End If
    'If oRecord < 0 Then
    '    Me.TBRpersone.Visible = False
    '    Me.TBRnorecord.Visible = True
    'End If
    ' End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGpersona.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If LCase(e.SortExpression) = LCase(oSortExpression) Then
            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        End If
        BindGriglia()
    End Sub
    Private Sub CambioPagina(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGpersona.PageIndexChanged
        Dim oSortExpression, oSortDirection As String

        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        BindGriglia()
    End Sub
    'Private Sub Cambio_NumPagine(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpaginazione.SelectedIndexChanged
    '    DGpersona.PageSize = DDLpaginazione.Items(DDLpaginazione.SelectedIndex).Value
    '    DGpersona.CurrentPageIndex = 0
    '    Me.ViewState("intCurPage") = 0
    '    BindGriglia()
    'End Sub
    Private Sub DGpersona_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGpersona.ItemCreated
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
                    If Me.DGpersona.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGpersona, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGpersona, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGpersona.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGpersona, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGpersona.HeaderStyle.CssClass
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
                    oResource.setPageDatagrid(Me.DGpersona, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Footer) Then
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then

            'gestione iscritti
            Try
                Dim oLKBvisualizzaCurriculum As LinkButton
                oLKBvisualizzaCurriculum = e.Item.Cells(3).Controls(0)

                If IsNothing(oLKBvisualizzaCurriculum) = False Then
                    oLKBvisualizzaCurriculum.CssClass = "Linksmall_Under11"
                    oResource.setLinkButton_Datagrid(Me.DGpersona, oLKBvisualizzaCurriculum, "LKBvisualizzaCurriculum", True, True, False, True)
                End If
            Catch ex As Exception

            End Try



        End If

    End Sub

    Private Sub DGpersona_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGpersona.ItemCommand
        Select Case e.CommandName

            Case "curriculum"
                'Dim NextPersId, PrevPersId As Integer

                Dim PRSN_id As Integer
                PRSN_id = CInt(Me.DGpersona.DataKeys.Item(e.Item.ItemIndex))
                ViewState("TblIndex") = e.Item.ItemIndex

                'Try
                '    NextPersId = CInt(Me.DGpersona.DataKeys.Item(e.Item.ItemIndex + 1))
                'Catch ex As Exception
                '    NextPersId = 0
                'End Try

                'Try
                '    NextPersId = CInt(Me.DGpersona.DataKeys.Item(e.Item.ItemIndex - 1))
                'Catch ex As Exception
                '    PrevPersId = 0
                'End Try

                Dim nominativo As String
                nominativo = DGpersona.Items(e.Item.ItemIndex).Cells(6).Text & " " & DGpersona.Items(e.Item.ItemIndex).Cells(5).Text

                If IsNothing(oResource) Then
                    Me.SetCulture(Session("LinguaCode"))
                End If

                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.Show")
                'Me.LBTitolo.Text = Me.LBTitolo.Text & ":&nbsp;" & nominativo

                Me.Master.ServiceTitle = oResource.getValue("LBTitolo.Show") & ":&nbsp;" & nominativo
                

                Me.PNLvetrina.Visible = False
                Me.PNLcurriculum.Visible = True
                PNLmenu.Visible = True
                Me.TBRdati.Visible = True
                Me.TBRformazione.Visible = False
                Me.TBRlingua.Visible = False
                Me.TBResperienze.Visible = False
                Me.TBRcompetenze.Visible = False
                Me.TBSmenu.SelectedIndex = 0

                Me.CTRLdati.PRSN_ID = PRSN_id
                Me.CTRLformazione.PRSN_ID = PRSN_id
                Me.CTRLlingua.PRSN_ID = PRSN_id
                Me.CTRLlavoro.PRSN_ID = PRSN_id
                Me.CTRLcompetenze.PRSN_ID = PRSN_id

                Me.CTRLdati.start()
                Me.TBSmenu.Tabs(TabIndex.Competenze).Visible = Me.CTRLcompetenze.start()
                Me.TBSmenu.Tabs(TabIndex.Formazione).Visible = Me.CTRLformazione.start()
                Me.TBSmenu.Tabs(TabIndex.Lingua).Visible = Me.CTRLlingua.start()
                Me.TBSmenu.Tabs(TabIndex.Esperienze).Visible = Me.CTRLlavoro.start()
                If Not (TBSmenu.Tabs(TabIndex.Competenze).Visible = True And TBSmenu.Tabs(TabIndex.Formazione).Visible = True And TBSmenu.Tabs(TabIndex.Lingua).Visible = True And TBSmenu.Tabs(TabIndex.Esperienze).Visible = True) Then
                    Dim larghezza As Integer = 0
                    If TBSmenu.Tabs(TabIndex.Dati).Visible = True Then
                        larghezza = larghezza + 170
                    End If
                    If TBSmenu.Tabs(TabIndex.Competenze).Visible = True Then
                        larghezza = larghezza + 125
                    End If
                    If TBSmenu.Tabs(TabIndex.Formazione).Visible = True Then
                        larghezza = larghezza + 105
                    End If
                    If TBSmenu.Tabs(TabIndex.Lingua).Visible = True Then
                        larghezza = larghezza + 180
                    End If
                    If TBSmenu.Tabs(TabIndex.Esperienze).Visible = True Then
                        larghezza = larghezza + 180
                    End If
                    TBSmenu.Width = System.Web.UI.WebControls.Unit.Pixel(larghezza)
                End If
                Me.BTNcerca.Visible = False

                Me.ShowLink()
                'BindLinkPersone(e.Item.ItemIndex)
            Case Else

        End Select

    End Sub

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        Me.BindGriglia(True)
    End Sub

    Private Sub DDLtipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoRuolo.SelectedIndexChanged
        Me.BindGriglia(True)
    End Sub
#End Region

#Region "tab"

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Me.ViewState("TabSelezionato") = Me.TBSmenu.SelectedIndex
        Me.ShowTabPanel(Me.TBSmenu.SelectedIndex)
    End Sub
#End Region

    Private Sub LKBtornaLista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBtornaLista.Click
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        'Me.oResource.setLabel(Me.LBTitolo)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")

        Me.PNLvetrina.Visible = True
        Me.PNLcurriculum.Visible = False
        PNLmenu.Visible = False
        Me.BTNcerca.Visible = True
        Me.BindGriglia(True)
    End Sub
    Private Function SetupJavascript()
        'aggiunge ai link button le proprietà da visualizzare nella barra di stato
        Dim i As Integer
        For i = Asc("a") To Asc("z") 'status dei link button delle lettere
            Dim oLinkButton As New LinkButton
            oLinkButton = FindControlRecursive(Master, "LKB" & Chr(i)) 'FindLnkLettera("LKB" & Chr(i)) 'FindControl("LKB" & Chr(i))
            Dim Carattere As String = Chr(i)

            If IsNothing(oLinkButton) = False Then
                oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
            End If
        Next
    End Function
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.DGpersona.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        viewstate("Paginazione") = "si"
        Me.BindGriglia(True)
    End Sub

    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
        Me.BindGriglia()
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
        Me.BindGriglia()
    End Sub


    'Private Sub BindLinkPersone(ByVal TblIndex As Integer)
    '    Dim TblIndexNext As Integer

    '    Try
    '        'se va tutto ok
    '        Me.LKBindietro.Text = "< " & DGpersona.Items(TblIndex - 1).Cells(6).Text & " " & DGpersona.Items(TblIndex - 1).Cells(5).Text
    '        Me.LKBindietro.Enabled = True
    '        Me.ViewState("TblIndex") = TblIndex

    '    Catch ex As Exception
    '        'vedo se è la prima pagina
    '        If Me.DGpersona.CurrentPageIndex = 0 Then 'blocco la retrocessione
    '            Me.LKBindietro.Text = "<-"
    '            Me.LKBindietro.Enabled = False
    '        Else 'imposto la pagina precedente e faccio bind della tabella
    '            Me.DGpersona.CurrentPageIndex -= 1
    '            Me.ViewState("intCurPage") -= 1
    '            BindGriglia()
    '            TblIndexNext = CInt(Me.DDLNumeroRecord.SelectedValue)

    '            'Imposto il link al nuovo nome (Mantengo anche il -1, perchè inizia a 0! Cos' il codice è uguale a prima...
    '            Me.LKBindietro.Text = "< " & DGpersona.Items(TblIndexNext - 1).Cells(6).Text & " " & DGpersona.Items(TblIndexNext - 1).Cells(5).Text

    '            Me.DGpersona.CurrentPageIndex += 1
    '            Me.ViewState("intCurPage") += 1
    '            BindGriglia()

    '            Me.LKBindietro.Enabled = True
    '            Me.ViewState("TblIndex") = TblIndex

    '        End If

    '        'Me.ViewState("TblIndex") = 0
    '    End Try





    '    Try
    '        'Se è tutto ok, continuo a scorrere la lista
    '        Me.LKBAvanti.Text = DGpersona.Items(TblIndex + 1).Cells(6).Text & " " & DGpersona.Items(TblIndex + 1).Cells(5).Text & " >"
    '        Me.LKBAvanti.Enabled = True
    '        Me.ViewState("TblIndex") = TblIndex
    '    Catch ex As Exception
    '        'E' l'ultima riga della pagina corrente
    '        If Me.DGpersona.CurrentPageIndex >= Me.DGpersona.PageCount - 1 Then 'Se sono all'ultima pagina, disabilito l'avanzamento
    '            Me.LKBAvanti.Text = "->"
    '            Me.LKBAvanti.Enabled = False
    '        Else 'altrimenti imposto la pagina corrente alla successiva e faccio bind datatable
    '            Me.DGpersona.CurrentPageIndex += 1
    '            Me.ViewState("intCurPage") += 1
    '            BindGriglia()

    '            'Imposto il puntatore a zero
    '            TblIndexNext = 0
    '            'Aggiorno il link
    '            Me.LKBAvanti.Text = DGpersona.Items(TblIndexNext).Cells(6).Text & " " & DGpersona.Items(TblIndexNext).Cells(5).Text & " >"
    '            Me.LKBAvanti.Enabled = True

    '            Me.DGpersona.CurrentPageIndex -= 1
    '            Me.ViewState("intCurPage") -= 1
    '            BindGriglia()

    '            Me.ViewState("TblIndex") = TblIndex
    '        End If


    '        'Me.ViewState("TblIndex") = 0
    '    End Try

    '    'Me.ViewState("TabSelezionato") = Me.TBSmenu.SelectedIndex

    'End Sub


    Private Sub LKBAvanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBAvanti.Click

        BindDatiPersona(CInt(Me.ViewState("TblIndex")) + 1)
    End Sub

    Private Sub LKBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBindietro.Click
        BindDatiPersona(CInt(Me.ViewState("TblIndex")) - 1)
    End Sub

    Private Sub BindDatiPersona(ByVal TblIndex As Integer)

        If TblIndex < 0 Then
            'pagina prima
            Me.DGpersona.CurrentPageIndex -= 1
            Me.ViewState("intCurPage") -= 1
            BindGriglia()
            TblIndex = Me.DDLNumeroRecord.SelectedValue
        ElseIf TblIndex >= Me.DGpersona.Items.Count Then
            Me.DGpersona.CurrentPageIndex += 1
            Me.ViewState("intCurPage") += 1
            BindGriglia()
            TblIndex = 0
        End If

        ViewState("TblIndex") = TblIndex

        Dim PRSN_id As Integer
        PRSN_id = CInt(Me.DGpersona.DataKeys.Item(TblIndex))

        Dim nominativo As String
        nominativo = DGpersona.Items(TblIndex).Cells(6).Text & " " & DGpersona.Items(TblIndex).Cells(5).Text

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.Show")
        'Me.LBTitolo.Text = Me.LBTitolo.Text & "&nbsp;" & nominativo

        Me.Master.ServiceTitle = oResource.getValue("LBTitolo.Show") & ":&nbsp;" & nominativo

        Me.PNLvetrina.Visible = False
        Me.PNLcurriculum.Visible = True
        PNLmenu.Visible = True
        Me.TBRdati.Visible = True
        Me.TBRformazione.Visible = False
        Me.TBRlingua.Visible = False
        Me.TBResperienze.Visible = False
        Me.TBRcompetenze.Visible = False
        Me.TBSmenu.SelectedIndex = 0

        Me.CTRLdati.PRSN_ID = PRSN_id
        Me.CTRLformazione.PRSN_ID = PRSN_id
        Me.CTRLlingua.PRSN_ID = PRSN_id
        Me.CTRLlavoro.PRSN_ID = PRSN_id
        Me.CTRLcompetenze.PRSN_ID = PRSN_id

        Me.CTRLdati.start()
        Me.TBSmenu.Tabs(TabIndex.Competenze).Visible = Me.CTRLcompetenze.start()
        Me.TBSmenu.Tabs(TabIndex.Formazione).Visible = Me.CTRLformazione.start()
        Me.TBSmenu.Tabs(TabIndex.Lingua).Visible = Me.CTRLlingua.start()
        Me.TBSmenu.Tabs(TabIndex.Esperienze).Visible = Me.CTRLlavoro.start()
        If Not (TBSmenu.Tabs(TabIndex.Competenze).Visible = True And TBSmenu.Tabs(TabIndex.Formazione).Visible = True And TBSmenu.Tabs(TabIndex.Lingua).Visible = True And TBSmenu.Tabs(TabIndex.Esperienze).Visible = True) Then
            Dim larghezza As Integer = 0
            If TBSmenu.Tabs(TabIndex.Dati).Visible = True Then
                larghezza = larghezza + 170
            End If
            If TBSmenu.Tabs(TabIndex.Competenze).Visible = True Then
                larghezza = larghezza + 125
            End If
            If TBSmenu.Tabs(TabIndex.Formazione).Visible = True Then
                larghezza = larghezza + 105
            End If
            If TBSmenu.Tabs(TabIndex.Lingua).Visible = True Then
                larghezza = larghezza + 180
            End If
            If TBSmenu.Tabs(TabIndex.Esperienze).Visible = True Then
                larghezza = larghezza + 180
            End If
            TBSmenu.Width = System.Web.UI.WebControls.Unit.Pixel(larghezza)
        End If
        Me.BTNcerca.Visible = False

        If Me.TBSmenu.Tabs.Item(CInt(Me.ViewState("TabSelezionato"))).Visible Then
            Me.TBSmenu.SelectedIndex = CInt(Me.ViewState("TabSelezionato"))
            ShowTabPanel(CInt(Me.ViewState("TabSelezionato")))
        Else
            Me.TBSmenu.SelectedIndex = 0
            ShowTabPanel(0)
        End If

        Me.ShowLink()
        'BindLinkPersone(TblIndex)
    End Sub
    Private Sub ShowLink()
        Me.LKBAvanti.Enabled = True
        Me.LKBindietro.Enabled = True

        If Me.DGpersona.CurrentPageIndex >= Me.DGpersona.PageCount - 1 Then
            If Me.ViewState("TblIndex") >= Me.DGpersona.Items.Count - 1 Then
                Me.LKBAvanti.Enabled = False
            End If
        End If
        If Me.DGpersona.CurrentPageIndex = 0 Then
            If Me.ViewState("TblIndex") = 0 Then
                Me.LKBindietro.Enabled = False
            End If
        End If
    End Sub
    Private Sub ShowTabPanel(ByVal IdPanel As Integer)
        Me.TBRdati.Visible = False
        Me.TBRformazione.Visible = False
        Me.TBRlingua.Visible = False
        Me.TBResperienze.Visible = False
        Me.TBRcompetenze.Visible = False

        Select Case IdPanel 'Me.TBSmenu.SelectedIndex
            Case 0
                Me.TBRdati.Visible = True
                Me.CTRLdati.start()
            Case 1
                Me.TBRcompetenze.Visible = True
                Me.CTRLcompetenze.start()
            Case 2
                Me.TBRformazione.Visible = True
                Me.CTRLformazione.start()
            Case 3
                Me.TBRlingua.Visible = True
                Me.CTRLlingua.start()
            Case 4
                Me.TBResperienze.Visible = True
                Me.CTRLlavoro.start()
        End Select


    End Sub

    'Private Function FindLnkLettera(ByVal Id As String) As LinkButton
    '    Dim Ctrl As Control = Me.Master.FindControl("aspnetForm")

    '    'Dim TEST As String = ""
    '    'For Each Ctrl As Control In Me.Master.Controls
    '    '    TEST &= Ctrl.ID.ToString & vbCrLf
    '    'Next
    '    If IsNothing(Ctrl) Then
    '        Return Nothing
    '    Else
    '        Dim lnkLettera As LinkButton = Ctrl.FindControl(Id)
    '        Return lnkLettera
    '    End If
    'End Function

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

End Class