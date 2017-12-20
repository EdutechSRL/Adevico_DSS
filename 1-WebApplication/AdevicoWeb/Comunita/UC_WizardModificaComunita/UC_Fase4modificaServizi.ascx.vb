Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_Fase4ModificaServizi
    Inherits System.Web.UI.UserControl

    Private oResourceServizi As ResourceManager

    Public Event AggiornamentoVisualizzazione(ByVal Selezionato As Boolean)
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ORGN_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_TipoComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_PersonaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasServizi As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNserviziSelezionati As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
#Region "Sezione Profili"
    Protected WithEvents TBRprofilo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBsceltaServizio As System.Web.UI.WebControls.Label
    Protected WithEvents RBLsceltaServizio As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents DDLprofilo As System.Web.UI.WebControls.DropDownList

    Protected WithEvents BTNcambiaProfilo As System.Web.UI.WebControls.Button
    Protected WithEvents BTNannullaModificheProfilo As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalvaModificheProfilo As System.Web.UI.WebControls.Button
#End Region

#Region "FORM Servizi"
    Protected WithEvents PNLservizi As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LBserviceName_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdescription_t As System.Web.UI.WebControls.Label
    Protected WithEvents DGServizi As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents HDNserviziSelezionati As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region


#Region "FORM Impostazioni"
    'Protected WithEvents PNLmenuSecondario As System.Web.UI.WebControls.Panel
    'Protected WithEvents PNLimpostazioni As System.Web.UI.WebControls.Panel
    ''  Protected WithEvents TBLpermessiRuoli As System.Web.UI.WebControls.Table
    'Protected WithEvents LBdefinizioneServizio As System.Web.UI.WebControls.Label
    'Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents LNBsalvaImpostazioniIndietro As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents TBLpermessiRuoli As System.Web.UI.WebControls.Table
    'Protected WithEvents HDNpermessi As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents LNBsalvaImpostazioni As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents HDNsrvz_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents RBLruoli As System.Web.UI.WebControls.RadioButtonList
    'Protected WithEvents LBlegendaRuoli As System.Web.UI.WebControls.Label
    'Protected WithEvents LBlegendaPermessi As System.Web.UI.WebControls.Label
#End Region


    Private Enum StatusPermessi
        Definiti = 0
        ListaCompleta = 1
        ListaDefault = 2
    End Enum

    Public ReadOnly Property isInizializzato() As Boolean
        Get
            Try
                isInizializzato = HDNhasSetup.Value
            Catch ex As Exception
                isInizializzato = False
            End Try
        End Get
    End Property

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
        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If
    End Sub


    Public Sub SetupControl(ByVal TipoComunitaID As Integer, ByVal OrganizzazioneID As Integer, ByVal PersonaID As Integer, ByVal ResponsabileID As Integer, Optional ByVal ComunitaID As Integer = 0)
        Dim SceltaDefault As Integer = 0
        Dim ProfiloID As Integer = -1

        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.HDN_ResponsabileID.Value = ResponsabileID
        Me.HDN_TipoComunitaID.Value = TipoComunitaID
        Me.HDN_ORGN_ID.Value = OrganizzazioneID
        Me.HDNhasSetup.Value = True
        Me.HDN_PersonaID.Value = PersonaID
        Me.HDNcmnt_ID.Value = ComunitaID
        Me.HDNhasServizi.Value = False
        Me.Bind_SezioneProfilo(ComunitaID, TipoComunitaID)
        Me.Bind_Servizi(True)
        Me.SetupInternazionalizzazione()

        RaiseEvent AggiornamentoVisualizzazione(AbilitaPulsanti)
    End Sub
    Public Sub AggiornaDati(ByVal TipoComunitaID As Integer, ByVal ComunitaID As Integer)
        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Try
            Dim ricalcola As Boolean = False
            If TipoComunitaID <> Me.HDN_TipoComunitaID.Value Then
                Dim ProfiloID As Integer = -1

                If ComunitaID > 0 Then
                    Dim oComunita As COL_Comunita
                    oComunita.Id = ComunitaID
                    ProfiloID = oComunita.GetProfiloServizioID()
                End If
                Me.Bind_DatiProfili(TipoComunitaID, ProfiloID)
                ricalcola = True
            End If
            Me.Bind_Servizi(False)
        Catch ex As Exception
            Me.HDNhasServizi.Value = False
        End Try
        RaiseEvent AggiornamentoVisualizzazione(AbilitaPulsanti)
    End Sub


    Private Function AbilitaPulsanti() As Boolean
        Dim HasSelezionati As Boolean = False
        Try
            If Me.HDNhasServizi.Value = True Then
                If Not (Me.HDNserviziSelezionati.Value = "" Or Me.HDNserviziSelezionati.Value = "," Or Me.HDNserviziSelezionati.Value = ",,") Then
                    HasSelezionati = True
                End If
            End If

        Catch ex As Exception
            HasSelezionati = False
        End Try
        Return HasSelezionati
    End Function

#Region "Bind_Dati"
    Private Sub Bind_Servizi(ByVal Ricalcola As Boolean) '
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet


        Try
            Dim i, totale
            oPersona = Session("objPersona")
            oComunita.Id = Me.HDNcmnt_ID.Value
            oDataset = oComunita.ElencaServizi(oPersona.Lingua.Id)

            totale = oDataset.Tables(0).Rows.Count
            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.HDNhasServizi.Value = True
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDisabled"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))
                If Ricalcola Then
                    Me.HDNserviziSelezionati.Value = ""
                End If
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    oRow.Item("oCheckDisabled") = ""
                    If CBool(oRow.Item("isNonDisattivabile")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = "checked"

                        If Not (InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0) Then
                            If Me.HDNserviziSelezionati.Value = "" Then
                                Me.HDNserviziSelezionati.Value = "," & oRow.Item("SRVZ_id") & ","
                            Else
                                Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value & oRow.Item("SRVZ_id") & ","
                            End If
                        End If
                    ElseIf Not CBool(oRow.Item("isAbilitato")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = ""

                        If InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                            Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value.Replace("," & oRow.Item("SRVZ_ID") & ",", ",")
                        End If
                    Else
                        If Ricalcola Then
                            If CBool(oRow.Item("isDefault")) Then
                                oRow.Item("oCheckDefault") = "checked"

                                If Me.HDNserviziSelezionati.Value = "" Then
                                    Me.HDNserviziSelezionati.Value = "," & oRow.Item("SRVZ_id") & ","
                                Else
                                    Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value & oRow.Item("SRVZ_id") & ","
                                End If
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        Else
                            If InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                                oRow.Item("oCheckDefault") = "checked"
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        End If
                    End If
                Next
                Dim oDataView As New DataView
                oDataView = oDataset.Tables(0).DefaultView
                oDataView.Sort = "isAbilitato DESC,isNonDisattivabile ASC, SRVZ_Nome "
                Me.DGServizi.DataSource = oDataView
                Me.DGServizi.DataBind()
            Else
                Me.HDNhasServizi.Value = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_SezioneProfilo(ByVal ComunitaID As Integer, ByVal TipoComunitaID As Integer)
        Dim SceltaDefault As Integer = 0
        Dim ProfiloID As Integer = -1
        Dim oComunita As New COL_Comunita

        oComunita.Id = ComunitaID
        ProfiloID = oComunita.GetProfiloServizioID()
        If ProfiloID > 0 Then
            SceltaDefault = 1
        End If
        Me.RBLsceltaServizio.SelectedValue = SceltaDefault
        Me.Bind_DatiProfili(TipoComunitaID, ProfiloID)
        If SceltaDefault <> 0 Then
            Me.DDLprofilo.Visible = True
        Else
            Me.DDLprofilo.Visible = False
        End If

        Me.RBLsceltaServizio.Enabled = False
        Me.DDLprofilo.Enabled = False
        If Me.DDLprofilo.Items.Count = 0 Then
            Me.BTNcambiaProfilo.Enabled = False
        Else
            Me.BTNcambiaProfilo.Enabled = True
        End If
    End Sub
    Private Sub Bind_DatiProfili(ByVal TipoComunitaID As Integer, Optional ByVal SelezionatoID As Integer = -1)
        Dim oProfilo As New COL_ProfiloServizio
        Dim oDataset As New DataSet
        Dim i, totale As Integer

        Try
            Dim IsResponsabile, isAttuale As Boolean

            oDataset = oProfilo.ElencaByComunita(Me.HDNcmnt_ID.Value, Me.HDN_PersonaID.Value, Me.HDN_ResponsabileID.Value, Session("LinguaID"), TipoComunitaID)
            totale = oDataset.Tables(0).Rows.Count - 1
            For i = 0 To totale
                Dim oRow As DataRow
                Dim oListItem As New ListItem

                oRow = oDataset.Tables(0).Rows(i)
                IsResponsabile = CBool(oRow.Item("IsResponsabile"))
                isAttuale = CBool(oRow.Item("isAttuale"))

                oListItem.Value = oRow.Item("PRFS_ID")
                If Me.HDN_ResponsabileID.Value = Me.HDN_PersonaID.Value Then
                    If Me.HDNcmnt_ID.Value = 0 Then
                        oListItem.Text = oRow.Item("PRFS_Nome")
                    ElseIf isAttuale Then
                        oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsAttuale") & oRow.Item("PRFS_Nome")
                    Else
                        oListItem.Text = oRow.Item("PRFS_Nome")
                    End If
                Else
                    If Me.HDNcmnt_ID.Value = 0 Then
                        If IsResponsabile Then
                            oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsResponsabile") & oRow.Item("PRFS_Nome")
                        Else
                            oListItem.Text = oRow.Item("PRFS_Nome")
                        End If
                    Else
                        If IsResponsabile Then
                            oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsResponsabile") & oRow.Item("PRFS_Nome")
                        ElseIf isAttuale Then
                            oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsAttuale") & oRow.Item("PRFS_Nome")
                        Else
                            oListItem.Text = oRow.Item("PRFS_Nome")
                        End If
                    End If
                End If
                Me.DDLprofilo.Items.Add(oListItem)
            Next
        Catch ex As Exception

        End Try

        Try
            Me.DDLprofilo.SelectedValue = SelezionatoID
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Sezione Profilo Servizio"
    Private Sub BTNcambiaProfilo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcambiaProfilo.Click
        If Me.DDLprofilo.Items.Count = 0 Then
            Me.RBLsceltaServizio.Enabled = False
        Else
            Me.RBLsceltaServizio.Enabled = True
        End If
        Me.DDLprofilo.Enabled = True
        Me.BTNcambiaProfilo.Visible = False
        Me.BTNannullaModificheProfilo.Visible = True
        Me.BTNsalvaModificheProfilo.Visible = True
        Me.Bind_Servizi(False)
    End Sub
    Private Sub BTNsalvaModificheProfilo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsalvaModificheProfilo.Click
        Dim ProfiloID As Integer = -1
        Dim ComunitaID As Integer
        Dim oComunita As New COL_Comunita
        Dim Aggiorna As Boolean = False


        ComunitaID = Me.HDNcmnt_ID.Value
        oComunita.Id = ComunitaID
        ProfiloID = oComunita.GetProfiloServizioID()
        If ProfiloID > 0 And Me.RBLsceltaServizio.SelectedValue = 0 Then
            Aggiorna = True
            oComunita.DefinisciServiziPermessiDiSistema()
        Else
            Try
                If ProfiloID <> Me.DDLprofilo.SelectedValue And Me.RBLsceltaServizio.SelectedValue = 1 Then
                    Aggiorna = True
                    oComunita.DefinisciServiziPermessiProfiloPersonale(Session("objPersona").id, Me.DDLprofilo.SelectedValue)
                End If
            Catch ex As Exception

            End Try
        End If

        Me.DDLprofilo.Enabled = False
        Me.RBLsceltaServizio.Enabled = False
        Me.BTNcambiaProfilo.Visible = True
        Me.BTNannullaModificheProfilo.Visible = False
        Me.BTNsalvaModificheProfilo.Visible = False
        Me.Bind_Servizi(Aggiorna)


    End Sub
    Private Sub BTNannullaModificheProfilo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNannullaModificheProfilo.Click
        Me.DDLprofilo.Enabled = False
        Me.RBLsceltaServizio.Enabled = False
        Me.BTNcambiaProfilo.Visible = True
        Me.BTNannullaModificheProfilo.Visible = False
        Me.BTNsalvaModificheProfilo.Visible = False
        Me.Bind_Servizi(False)
    End Sub
    Private Sub RBLsceltaServizio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLsceltaServizio.SelectedIndexChanged
        If Me.RBLsceltaServizio.SelectedValue = 0 Then
            Me.DDLprofilo.Visible = False
        Else
            Me.DDLprofilo.Visible = True
        End If
        Me.Bind_Servizi(True)
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGServizi_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGServizi.ItemCreated
        Dim i As Integer

        If IsNothing(oResourceServizi) Then
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
                                    oResourceServizi.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, Main.FiltroOrdinamento.Decrescente)
                                Else
                                    oResourceServizi.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, Main.FiltroOrdinamento.Crescente)
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
                                oResourceServizi.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, Main.FiltroOrdinamento.Crescente)
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
                    Me.oResourceServizi.setPageDatagrid(Me.DGServizi, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"

            Try
                If CBool(e.Item.DataItem("isAbilitato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("isNonDisattivabile")) = True Then
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

            Dim oLNBimpostazioni As LinkButton
            Dim oLBseparatore As Label
            Try
                oLBseparatore = e.Item.Cells(0).FindControl("LBseparatore")
                oLNBimpostazioni = e.Item.Cells(0).FindControl("LNBimpostazioni")
                oLBseparatore.CssClass = cssRiga
                oLNBimpostazioni.CssClass = cssLink

                oLBseparatore.Visible = False
                oLNBimpostazioni.Visible = False
                oLNBimpostazioni.CommandArgument = e.Item.DataItem("SRVZ_Nome")
            Catch ex As Exception

            End Try
            Try
                Dim oCheck As HtmlControls.HtmlInputCheckBox
                oCheck = e.Item.Cells(1).FindControl("CBXservizioAttivato")
                If Not IsNothing(oCheck) Then
                    Try
                        If InStr(Me.HDNserviziSelezionati.Value, "," & e.Item.DataItem("SRVZ_ID") & ",") > 0 Then
                            oCheck.Checked = True
                        Else
                            oCheck.Checked = False
                        End If
                    Catch ex As Exception
                        oCheck.Checked = False
                    End Try
                    oCheck.Value = e.Item.DataItem("SRVZ_ID")
                    Try
                        If e.Item.DataItem("isNonDisattivabile") = True Or e.Item.DataItem("isAbilitato") = False Then
                            oCheck.Disabled = True
                        Else
                            oCheck.Disabled = False
                        End If
                    Catch ex As Exception

                    End Try

                    If Not (oCheck.Checked = False And oCheck.Disabled = True) Then
                        oLBseparatore.Visible = True
                        oLNBimpostazioni.Visible = True
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGServizi_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGServizi.PageIndexChanged
        Me.DGServizi.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Servizi(False)
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
        Me.Bind_Servizi(False)
    End Sub
    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If


        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceServizi.getValue("NONattivato")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceServizi.getValue("NONdisattivabile")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function
#End Region


#Region "Bind_Dati"
    Private Function AggiornaPermessi()
        Dim oServizio As New COL_Servizio
        Dim oDataSet As New DataSet
        Dim i, totale, ComunitaID, TPRL_ID As Integer
        Dim isAdmin As Boolean = False

        Try
            If Session("AdminForChange") = True Then
                isAdmin = True
            Else
                TPRL_ID = Session("IdRuolo")
            End If
        Catch ex As Exception
            TPRL_ID = Session("IdRuolo")
        End Try

        ComunitaID = Me.HDNcmnt_ID.Value

        If Not isAdmin Then
            oDataSet = oServizio.ElencaByTipoRuoloByComunita(TPRL_ID, ComunitaID)
            totale = oDataSet.Tables(0).Rows.Count - 1

            Dim ArrPermessi(totale, 2) As String
            For i = 0 To totale
                Dim oRow As DataRow
                oRow = oDataSet.Tables(0).Rows(i)
                ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
                ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
                ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
            Next
            Session("ArrPermessi") = ArrPermessi
        End If
    End Function
#End Region

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResourceServizi = New ResourceManager

        oResourceServizi.UserLanguages = code
        oResourceServizi.ResourcesName = "pg_ManagementServizi"
        oResourceServizi.Folder_Level1 = "Comunita"
        oResourceServizi.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceServizi
            .setLabel(Me.LBserviceName_t)
            .setLabel(Me.LBdescription_t)

            .setLabel(Me.LBsceltaServizio)
            .setRadioButtonList(Me.RBLsceltaServizio, 0)
            .setRadioButtonList(Me.RBLsceltaServizio, 1)
            .setButton(Me.BTNcambiaProfilo, True)
            .setButton(Me.BTNannullaModificheProfilo, True)
            .setButton(Me.BTNsalvaModificheProfilo, True, , True, True)

        End With
    End Sub
#End Region

    Public Function SalvaImpostazioni() As WizardComunita_Message
        Dim oComunita As New COL_Comunita
        Dim iResponse As WizardComunita_Message = WizardComunita_Message.ErroreServizi
        Dim Ricalcola As Boolean = False
        Try

            oComunita.Id = Me.HDNcmnt_ID.Value
            oComunita.Estrai()
            If oComunita.Errore = Errori_Db.None Then
                Dim ListaServizi As String
                Dim ElencoServiziID As String()
                ListaServizi = Me.HDNserviziSelezionati.Value

                If ListaServizi = "" Or ListaServizi = "," Or ListaServizi = ",," Then
                    iResponse = WizardComunita_Message.ServiziDefault
                Else
                    If Me.RBLsceltaServizio.SelectedValue = 0 Then
                        oComunita.DefinisciServiziDefault(ListaServizi)
                    Else
                        oComunita.AssociaProfiloServizi(Session("objPersona").id, Me.DDLprofilo.SelectedValue, ListaServizi)
                    End If
                    If oComunita.Errore = Errori_Db.None Then
                        Ricalcola = True
                        iResponse = WizardComunita_Message.ServiziAttivati
                    Else
                        iResponse = WizardComunita_Message.ServiziDefault
                    End If
                End If
            Else
                iResponse = WizardComunita_Message.NessunaComunita
            End If
        Catch ex As Exception
            iResponse = WizardComunita_Message.ErroreServizi
        End Try
        Me.Bind_Servizi(Ricalcola)
        Return iResponse
    End Function
End Class
