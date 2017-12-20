Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita



Public Class StampaIscrittiAvanzata
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Protected WithEvents DGiscritti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents BTNOk As System.Web.UI.WebControls.Button
    Protected WithEvents BTNstampa As System.Web.UI.WebControls.Button
    Protected WithEvents CHLcolonne As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBcolonne As System.Web.UI.WebControls.Label

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

#Region "Public Property"
    Public ReadOnly Property ComunitaID() As Integer
        Get
            Try
                ComunitaID = CInt(Session("Stampa")(0))
            Catch ex As Exception
                ComunitaID = -1
            End Try
        End Get
    End Property
    Public ReadOnly Property ComunitaPath() As String
        Get
            Try
                ComunitaPath = Session("Stampa")(1)
            Catch ex As Exception
                ComunitaPath = ""
            End Try
        End Get
    End Property
    Public ReadOnly Property ActivationSelected() As String
        Get
            Try
                ActivationSelected = Session("Stampa")(2)
            Catch ex As Exception
                ActivationSelected = ""
            End Try
        End Get
    End Property
    Public ReadOnly Property AllActivation() As Boolean
        Get
            Try
                AllActivation = CBool(Session("Stampa")(3))
            Catch ex As Exception
                AllActivation = True
            End Try
        End Get
    End Property
    Public ReadOnly Property AllRoles() As Boolean
        Get
            Try
                AllRoles = CBool(Session("Stampa")(4))
            Catch ex As Exception
                AllRoles = True
            End Try
        End Get
    End Property
    Public ReadOnly Property RoleSelected() As String
        Get
            Try
                RoleSelected = Session("Stampa")(5)
            Catch ex As Exception
                RoleSelected = ""
            End Try
        End Get
    End Property
    Public ReadOnly Property isPaginated() As Boolean
        Get
            Try
                isPaginated = CBool(Session("Stampa")(6))
            Catch ex As Exception
                isPaginated = True
            End Try
        End Get
    End Property
    Public ReadOnly Property PageSize() As Integer
        Get
            Try
                If Me.isPaginated = False Then
                    PageSize = -1
                Else
                    PageSize = CInt(Session("Stampa")(7))
                End If
            Catch ex As Exception
                PageSize = -1
            End Try
        End Get
    End Property
    Public ReadOnly Property SortExspression() As String
        Get
            Try
                SortExspression = Session("Stampa")(8)
            Catch ex As Exception
                SortExspression = ""
            End Try
        End Get
    End Property
    Public ReadOnly Property SortDirection() As String
        Get
            Try
                SortDirection = Session("Stampa")(9)
            Catch ex As Exception
                SortDirection = ""
            End Try
        End Get
    End Property
    Public ReadOnly Property intAnagrafica() As Integer
        Get
            Try
                intAnagrafica = CInt(Session("intAnagrafica")(10))
            Catch ex As Exception
                intAnagrafica = Main.FiltroAnagrafica.tutti
            End Try
        End Get
    End Property
    Public ReadOnly Property intCurPage() As Integer
        Get
            Try
                intCurPage = CInt(Session("Stampa")(11))
            Catch ex As Exception
                intCurPage = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property TipoRicerca() As Integer
        Get
            Try
                TipoRicerca = CInt(Session("Stampa")(12))
            Catch ex As Exception
                TipoRicerca = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property Valore() As String
        Get
            Try
                Valore = CInt(Session("Stampa")(13))
            Catch ex As Exception
                Valore = ""
            End Try
        End Get
    End Property
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
            SetCulture(Session("LinguaCode"))
        End If

        If Not Page.IsPostBack Then
            SetupInternazionalizzazione()
            Bind_Griglia()
            Me.BTNOk.Attributes.Add("onclick", "ChiudiMi();return false;")
            Me.BTNstampa.Attributes.Add("onclick", "stampa();return false;")
        End If
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = code
        oResource.ResourcesName = "pg_StampaIscritti"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            .setLabel(Me.LBcolonne)
            '.setLabel(Me.LBNopermessi)
            .setCheckBoxList(Me.CHLcolonne, 1)
            .setCheckBoxList(Me.CHLcolonne, 2)
            .setCheckBoxList(Me.CHLcolonne, 3)
            .setCheckBoxList(Me.CHLcolonne, 4)
            .setCheckBoxList(Me.CHLcolonne, 5)
            .setCheckBoxList(Me.CHLcolonne, 6)
            .setCheckBoxList(Me.CHLcolonne, 7)
            .setCheckBoxList(Me.CHLcolonne, 8)

            .setButton(Me.BTNOk)
            .setButton(Me.BTNstampa)

            .setHeaderDatagrid(Me.DGiscritti, 1, "matricola", True)
            .setHeaderDatagrid(Me.DGiscritti, 2, "PRSN_Anagrafica", True)
            .setHeaderDatagrid(Me.DGiscritti, 3, "PRSN_login", True)
            .setHeaderDatagrid(Me.DGiscritti, 4, "Mail", True)
            .setHeaderDatagrid(Me.DGiscritti, 5, "TPRL_nome", True)
            .setHeaderDatagrid(Me.DGiscritti, 6, "TPPR_descrizione", True)
            .setHeaderDatagrid(Me.DGiscritti, 7, "RLPC_ultimoCollegamento", True)
            .setHeaderDatagrid(Me.DGiscritti, 8, "RLPC_IscrittoIl", True)

        End With
    End Sub
#End Region

#Region "bindDati"
    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim oPersona As New COL_Persona
            Dim Valore As String
            oPersona = Session("objPersona")

            Dim oComunita As New COL_Comunita

            oComunita.Id = Me.ComunitaID 'Me.CTRLfiltroComunita.ComunitaID
            oComunita.Estrai()
            ' Me.LBtitolo.Text = "Iscritti a " & oComunita.Organizzazione.RagioneSociale & " nella comunità di " & oComunita.Nome
            Me.LBtitolo.Text = oResource.getValue("LBtitolo.text")
            Me.LBtitolo.Text = Me.LBtitolo.Text.Replace("#o#", oComunita.Organizzazione.RagioneSociale)
            Me.LBtitolo.Text = Me.LBtitolo.Text.Replace("#c#", oComunita.Nome)
            Dim ElencoRuoli_ID, ElencoTipiAttivazione_ID As String

            If Me.AllRoles = False Then 'Me.CTRLfiltroUtenti.AllRoles = False Then
                ElencoRuoli_ID = Me.RoleSelected 'Me.CTRLfiltroUtenti.RoleSelected
            End If
            If Me.AllActivation = False Then 'Me.CTRLfiltroUtenti.AllActivation = False Then
                ElencoTipiAttivazione_ID = Me.ActivationSelected  'Me.CTRLfiltroUtenti.RoleSelected
            End If

            Dim oFiltroAnagrafica As Main.FiltroAnagrafica
            Dim oFiltroCampoOrdine As Main.FiltroCampoOrdineComunita
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento

            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
            If Me.Valore <> "" Then 'Me.TXBValore.Text
                Valore = Me.Valore 'Me.TXBValore.Text.Trim
                If Valore <> "" Then
                    oFiltroAnagrafica = CType(Me.TipoRicerca, Main.FiltroAnagrafica)

                    If oFiltroAnagrafica = Main.FiltroAnagrafica.dataNascita Then
                        If IsDate(Valore) = False Then
                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Valore = ""
                        End If
                    End If
                End If
            End If

            Try
                If IsNothing(Me.ViewState("intAnagrafica")) Then
                    Try
                        oFiltroAnagrafica = CType(Me.intAnagrafica, Main.FiltroAnagrafica)
                    Catch ex As Exception
                        oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                    End Try
                    Me.ViewState("intAnagrafica") = CType(oFiltroAnagrafica, Main.FiltroAnagrafica)
                Else
                    Try
                        If Me.ViewState("intAnagrafica") = "" Then
                            oFiltroAnagrafica = CType(Me.intAnagrafica, Main.FiltroAnagrafica)
                            Me.ViewState("intAnagrafica") = CType(oFiltroAnagrafica, Main.FiltroAnagrafica)
                        Else
                            oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                        End If
                    Catch ex As Exception

                    End Try

                End If
            Catch ex As Exception
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
            End Try


            Select Case Me.TipoRicerca
                Case Main.FiltroAnagrafica.tutti
                    Valore = ""
                Case Main.FiltroAnagrafica.nome
                    If Me.Valore <> "" Then
                        Valore = Me.Valore
                        oFiltroAnagrafica = Main.FiltroPersona.nome
                    End If
                Case Main.FiltroAnagrafica.cognome
                    If Me.Valore <> "" Then
                        Valore = Me.Valore
                        oFiltroAnagrafica = Main.FiltroPersona.cognome
                    End If
                Case Main.FiltroAnagrafica.matricola
                    If Me.Valore <> "" Then
                        Valore = Me.Valore
                        oFiltroAnagrafica = Main.FiltroPersona.matricola
                    End If
                Case Main.FiltroAnagrafica.dataNascita
                    If Me.Valore <> "" Then
                        Try
                            If IsDate(Me.Valore) Then
                                Valore = Me.Valore
                                Valore = Main.DateToString(Valore, False)
                                oFiltroAnagrafica = Main.FiltroAnagrafica.dataNascita
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                Case Main.FiltroAnagrafica.login
                    If Me.Valore <> "" Then
                        Valore = Me.Valore
                        oFiltroAnagrafica = Main.FiltroAnagrafica.login
                    End If
                Case Else
            End Select

            If viewstate("SortExspression") = "" Or LCase(viewstate("SortExspression")) = "prsn_anagrafica" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
            ElseIf LCase(viewstate("SortExspression")) = "prsn_datanascita" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataNascita
            ElseIf LCase(viewstate("SortExspression")) = "tprl_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoRuolo
            ElseIf LCase(viewstate("SortExspression")) = "tppr_descrizione" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoPersona
            ElseIf LCase(viewstate("SortExspression")) = "prsn_login" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.login
            ElseIf LCase(viewstate("SortExspression")) = "rlpc_iscrittoil" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataIscrizione
            ElseIf LCase(viewstate("SortExspression")) = "tppr_descrizione" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoPersona
            Else
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
            End If

            Dim ordinamento As Integer
            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If


            Dim totale As Decimal
            If ricalcola Then
                Me.ViewState("intCurPage") = 0
                Me.DGiscritti.CurrentPageIndex = 0
            End If

            Dim pageSize, pageIndex As Integer
            If Me.isPaginated Then 'Me.CTRLfiltroUtenti.isPaginated
                If Me.PageSize > 0 Then 'Me.CTRLfiltroUtenti.PageSize
                    pageSize = Me.PageSize 'Me.CTRLfiltroUtenti.PageSize

                    If ricalcola Then
                        pageIndex = 0
                        Me.ViewState("intCurPage") = 0
                    Else
                        pageIndex = Me.ViewState("intCurPage")
                    End If
                Else
                    pageSize = -1
                    pageIndex = 0
                End If
            Else
                pageSize = -1
                pageIndex = 0
            End If
            Return oComunita.ElencaIscrittiSottoComunita(Me.ComunitaID, oPersona.Id, Session("LinguaID"), ElencoRuoli_ID, ElencoTipiAttivazione_ID, pageSize, pageIndex, Valore, oFiltroAnagrafica, oFiltroOrdinamento, oFiltroCampoOrdine, False, Main.FiltroUtenti.NoPassantiNoCreatori)
        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub Bind_Griglia(Optional ByVal ricalcola As Boolean = False)
        Dim oPersona As New COL_Persona
        Dim dsTable As New DataSet

        Try
            oPersona = Session("objPersona")
            dsTable = FiltraggioDati(ricalcola)

            ' dsTable.Tables(0).Columns.Add(New DataColumn("oCheck"))
            Dim i, totale As Integer

            totale = dsTable.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGiscritti.Visible = False
                ' LBnoIscritti.Visible = True
                Me.DGiscritti.VirtualItemCount = 0
            Else
                Me.DGiscritti.VirtualItemCount = dsTable.Tables(0).Rows(0).Item("Totale")
                dsTable.Tables(0).Columns.Add(New DataColumn("oIscrittoIl"))
                dsTable.Tables(0).Columns.Add(New DataColumn("oRLPC_ultimoCollegamento"))
                dsTable.Tables(0).Columns.Add("Matricola")

                Dim PRSN_TPRL_Gerarchia, TPRL_Gerarchia As Integer
                Try
                    PRSN_TPRL_Gerarchia = Me.ViewState("PRSN_TPRL_Gerarchia")
                Catch ex As Exception
                    PRSN_TPRL_Gerarchia = "9999999"
                End Try


                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = dsTable.Tables(0).Rows(i)

                    If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                        If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                            oRow.Item("oIscrittoIl") = "&nbsp;--"
                        Else
                            oRow.Item("oIscrittoIl") = "&nbsp;" & CDate(oRow.Item("RLPC_IscrittoIl")).ToString("dd/MM/yy hh.mm")
                        End If
                    Else
                        oRow.Item("oIscrittoIl") = "&nbsp;--"
                    End If

                    If IsDBNull(oRow.Item("RLPC_ultimoCollegamento")) = False Then
                        If Equals(oRow.Item("RLPC_ultimoCollegamento"), New Date) Then
                            oRow.Item("oRLPC_ultimoCollegamento") = "&nbsp;--"
                        Else
                            oRow.Item("oRLPC_ultimoCollegamento") = "&nbsp;" & CDate(oRow.Item("RLPC_ultimoCollegamento")).ToString("dd/MM/yy hh.mm")
                        End If
                    Else
                        oRow.Item("oRLPC_ultimoCollegamento") = "&nbsp;--"
                    End If

                    If IsDBNull(oRow.Item("STDN_matricola")) Then
                        oRow.Item("Matricola") = "&nbsp;"
                    Else
                        If oRow.Item("STDN_matricola") <> "-1" Then
                            oRow.Item("Matricola") = oRow.Item("STDN_matricola")
                        Else
                            oRow.Item("Matricola") = "<b>" & oResource.getValue("noMatricola") & "</b>"
                        End If
                    End If
                    oRow.Item("PRSN_Anagrafica") = "<b>" & oRow.Item("PRSN_Cognome") & "</b> " & oRow.Item("PRSN_Nome")

                Next
                If Me.isPaginated Then
                    Me.DGiscritti.AllowPaging = True
                    Me.DGiscritti.AllowCustomPaging = True
                    Me.DGiscritti.PageSize = Me.PageSize 'Me.CTRLfiltroUtenti.PageSize
                Else
                    Me.DGiscritti.AllowPaging = False
                    Me.DGiscritti.AllowCustomPaging = False
                End If
                If totale > 0 Then
                    Me.DGiscritti.Visible = True

                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_Anagrafica"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")


                    Me.DGiscritti.Columns(1).Visible = Me.CHLcolonne.Items.Item(0).Selected 'matricola
                    Me.DGiscritti.Columns(2).Visible = Me.CHLcolonne.Items.Item(1).Selected 'anagrafica
                    Me.DGiscritti.Columns(3).Visible = Me.CHLcolonne.Items.Item(2).Selected 'login
                    Me.DGiscritti.Columns(4).Visible = Me.CHLcolonne.Items.Item(3).Selected 'mail
                    Me.DGiscritti.Columns(5).Visible = Me.CHLcolonne.Items.Item(4).Selected 'ruolo
                    Me.DGiscritti.Columns(6).Visible = Me.CHLcolonne.Items.Item(5).Selected 'tipopersona
                    Me.DGiscritti.Columns(7).Visible = Me.CHLcolonne.Items.Item(6).Selected 'ultimocollegamewn
                    Me.DGiscritti.Columns(8).Visible = Me.CHLcolonne.Items.Item(7).Selected 'iscritto il

                    '  Me.DGiscritti.Columns(2).Visible = False
                    Me.DGiscritti.DataSource = oDataview
                    Me.DGiscritti.DataBind()
                    '   LBnoIscritti.Visible = False
                    Me.DGiscritti.Visible = True
                Else
                    Me.DGiscritti.Visible = False
                    '  LBnoIscritti.Visible = True
                End If
            End If
        Catch ex As Exception

        End Try
        '  Me.SetSessioneStampa()
    End Sub
#End Region


    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGiscritti.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")
            If oSortDirection = "asc" Then
                oText = "5"
            Else
                oText = "6"
            End If

            For i = 0 To sender.columns.count - 1
                If sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    'oLabelAfter.font.name = "webdings"
                    'oLabelAfter.font.size = FontUnit.XSmall
                    'oLabelAfter.text = "&nbsp;"

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If Me.DGiscritti.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Decrescente)
                                End If


                                'oLabelAfter.font.name = oLinkbutton.Font.Name
                                'oLabelAfter.font.size = oLinkbutton.Font.Size
                                oLabelAfter.CssClass = Me.DGiscritti.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                oLinkbutton.Font.Name = "webdings"
                                oLinkbutton.Font.Size = FontUnit.XSmall
                                oLinkbutton.Text = oText
                                ' oCell.Controls.AddAt(0, oLabelBefore)
                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                '  oCell.Controls.AddAt(0, oLabelBefore)
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        Else
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)

                                'oLabelAfter.font.name = oLinkbutton.Font.Name
                                'oLabelAfter.font.size = oLinkbutton.Font.Size
                                oLabelAfter.CssClass = Me.DGiscritti.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                oLinkbutton.Font.Name = "webdings"
                                oLinkbutton.Font.Size = FontUnit.XSmall
                                oLinkbutton.Text = "5"
                                ' oCell.Controls.AddAt(0, oLabelBefore)
                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                ' oCell.Controls.AddAt(0, oLabelBefore)
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
                    oResource.setPageDatagrid(Me.DGiscritti, oLinkbutton)
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
            'Dim PermessiAssociati As String
            'Dim oServizioIscritti As New UCServices.Services_GestioneIscritti
            'Try
            '    oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            'Catch ex As Exception
            '    oServizioIscritti.PermessiAssociati = Me.GetPermessiForPage(oServizioIscritti.Codex)
            'End Try



        End If
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGiscritti.SortCommand
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
        Me.Bind_Griglia()
    End Sub
    Private Sub DGiscritti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGiscritti.PageIndexChanged
        Dim oSortExpression, oSortDirection As String

        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub

    Private Sub CHLcolonne_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CHLcolonne.SelectedIndexChanged
        Me.Bind_Griglia()
    End Sub
End Class
