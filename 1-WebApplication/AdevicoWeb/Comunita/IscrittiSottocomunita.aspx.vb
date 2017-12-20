Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices


Public Class IscrittiSottocomunita
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum
    Private Enum StringaAbilitato
        abilitato = 1
        bloccato = 0
        inAttesa = -1
    End Enum

#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLfiltroComunita As Comunita_OnLine.UC_FiltroComunitaGenerale
    Protected WithEvents CTRLfiltroUtenti As Comunita_OnLine.UC_FiltriUtente

    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents DGiscritti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DGiscrittiBis As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoIscritti As System.Web.UI.WebControls.Label
    Protected WithEvents HDazione As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBLexcel As System.Web.UI.WebControls.Table

#Region "Filtro"
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
    Protected WithEvents LNBfind As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBstampa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBexcel As System.Web.UI.WebControls.LinkButton

    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox

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
        Dim oServizioIscritti As New Services_ManagementSottoIscritti

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Try
            Try
                If Not Page.IsPostBack Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioIscritti.Codex)
                    oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
                Else
                    If Me.ViewState("PermessiAssociati") = "" Then
                        Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioIscritti.Codex)
                    End If
                    oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
                End If
            Catch ex As Exception
                oServizioIscritti.PermessiAssociati = "00000000000000000000000000000000"
            End Try

            If Page.IsPostBack = False Then
                Me.SetupInternazionalizzazione()
                If oServizioIscritti.Management Or oServizioIscritti.Change Or oServizioIscritti.Admin Or oServizioIscritti.Delete Then
                    Me.PNLcontenuto.Visible = True
                    Me.PNLpermessi.Visible = False
                    Me.PNLmenu.Visible = True

                    Dim totaleHistory As Integer
                    Dim ComunitaPath, ArrComunita(,) As String

                    If Session("limbo") = False Then
                        ComunitaPath = "."
                        'Recupero il percorso della comunità attuale, per trovarmi i suoi figli !
                        totaleHistory = -1
                        If IsArray(Session("ArrComunita")) Then
                            Try
                                ArrComunita = Session("ArrComunita")
                                totaleHistory = UBound(ArrComunita, 2)

                                ComunitaPath = ArrComunita(2, totaleHistory)
                            Catch ex As Exception

                            End Try
                        End If
                    End If

                    Me.CTRLfiltroComunita.SetupControl(Session("IdComunita"), ComunitaPath)
                    Me.CTRLfiltroUtenti.Bind_Dati(Me.CTRLfiltroComunita.ComunitaID, Me.CTRLfiltroComunita.ComunitaPath)
                    Me.Bind_Griglia()
                    Session("Azione") = "load"

                    Me.SetupFiltri()
                    Dim i_link As String = "./StampaIscrittiAvanzata.aspx"
                    Me.LNBstampa.Attributes.Add("onClick", "OpenWin('" & i_link & "','700','600','yes','yes');return false;")
                Else
                    Me.PNLmenu.Visible = False
                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                End If

            End If
        Catch ex As Exception

        End Try
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
            Try
                Dim CMNT_ID As Integer = 0
                Try
                    CMNT_ID = Session("idComunita")
                Catch ex As Exception
                    CMNT_ID = 0
                End Try
                If CMNT_ID <= 0 Then
                    Me.ExitToLimbo()
                    Return True
                End If
            Catch ex As Exception
                Me.ExitToLimbo()
                Return True
            End Try
        End If
    End Function

    Private Sub ExitToLimbo()
        Session("Limbo") = True
        Session("ORGN_id") = 0
        Session("IdRuolo") = ""
        Session("ArrPermessi") = ""
        Session("RLPC_ID") = ""

        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("TPCM_ID") = ""
        Me.Response.Expires = 0
        Me.Response.Redirect("./EntrataComunita.aspx", True)
    End Sub


#Region "Bind_Dati"
    Private Sub SetupFiltri()
        Dim i As Integer

        Me.ViewState("intAnagrafica") = -1
        Me.LKBtutti.CssClass = "lettera_Selezionata"
        Me.DDLTipoRicerca.SelectedIndex = -1
        Me.TXBValore.Text = ""

        Try
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControl("LKB" & Chr(i))
                Dim Carattere As String = Chr(i)
                If IsNothing(oLinkButton) = False Then
                    oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim oPersona As New COL_Persona
        Dim oRuoloComunita As New COL_RuoloPersonaComunita
        Dim CMNT_id As Integer

        Dim PermessiAssociati As String


        If Page.IsPostBack = False Then
            Try
                oPersona = Session("objPersona")
                PermessiAssociati = Permessi(Codex, Me.Page)

                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try
        Else
            CMNT_id = Me.CTRLfiltroComunita.ComunitaID

            If CMNT_id = Session("IdComunita") Then
                ' Sono nella stessa comunità !
                Try
                    PermessiAssociati = Permessi(Codex, Me.Page)
                    If (PermessiAssociati = "") Then
                        PermessiAssociati = "00000000000000000000000000000000"
                    End If
                Catch ex As Exception
                    PermessiAssociati = "00000000000000000000000000000000"
                End Try

                Try
                    oRuoloComunita.EstraiByLinguaDefault(CMNT_id, oPersona.Id)
                    Me.ViewState("PRSN_TPRL_Gerarchia") = oRuoloComunita.TipoRuolo.Gerarchia

                Catch ex As Exception
                    Me.ViewState("PRSN_TPRL_Gerarchia") = "99999"
                End Try
            Else
                'sono in una comunità sottostante, che faccio con i permessi ?
                Dim oComunita As New COL_Comunita
                oComunita.Id = CMNT_id
                Try
                    PermessiAssociati = oComunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, CMNT_id, Codex)
                    If (PermessiAssociati = "") Then
                        PermessiAssociati = "00000000000000000000000000000000"
                    End If
                Catch ex As Exception
                    PermessiAssociati = "00000000000000000000000000000000"
                End Try
            End If
        End If
        Return PermessiAssociati
    End Function

    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim oPersona As New COL_Persona
            Dim Valore As String
            oPersona = Session("objPersona")

            Dim oComunita As New COL_Comunita

            oComunita.Id = Me.CTRLfiltroComunita.ComunitaID

            Dim ElencoRuoli_ID, ElencoTipiAttivazione_ID As String

            If Me.CTRLfiltroUtenti.AllRoles = False Then
                ElencoRuoli_ID = Me.CTRLfiltroUtenti.RoleSelected
            End If
            If Me.CTRLfiltroUtenti.AllActivation = False Then
                ElencoTipiAttivazione_ID = Me.CTRLfiltroUtenti.ActivationSelected
            End If

            Dim oFiltroAnagrafica As Main.FiltroAnagrafica
            Dim oFiltroCampoOrdine As Main.FiltroCampoOrdineComunita
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento

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

            If Me.TXBValore.Text <> "" Then
                Me.TXBValore.Text = Trim(Me.TXBValore.Text)
            End If

            Select Case Me.DDLTipoRicerca.SelectedValue
                Case Main.FiltroAnagrafica.tutti
                    Valore = ""
                Case Main.FiltroAnagrafica.nome
                    If Me.TXBValore.Text <> "" Then
                        Valore = Me.TXBValore.Text
                        oFiltroAnagrafica = Main.FiltroPersona.nome
                    End If
                Case Main.FiltroAnagrafica.cognome
                    If Me.TXBValore.Text <> "" Then
                        Valore = Me.TXBValore.Text
                        oFiltroAnagrafica = Main.FiltroPersona.cognome

                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                    End If
                Case Main.FiltroAnagrafica.matricola
                    If Me.TXBValore.Text <> "" Then
                        Valore = Me.TXBValore.Text
                        oFiltroAnagrafica = Main.FiltroPersona.matricola

                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                    End If
                Case Main.FiltroAnagrafica.dataNascita
                    If Me.TXBValore.Text <> "" Then
                        Try
                            If IsDate(Me.TXBValore.Text) Then
                                Valore = Me.TXBValore.Text
                                Valore = Main.DateToString(Valore, False)
                                oFiltroAnagrafica = Main.FiltroAnagrafica.dataNascita
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                Case Main.FiltroAnagrafica.login
                    If Me.TXBValore.Text <> "" Then
                        Valore = Me.TXBValore.Text
                        oFiltroAnagrafica = Main.FiltroAnagrafica.login
                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
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
            ElseIf LCase(viewstate("SortExspression")) = "prsn_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.nome
            ElseIf LCase(viewstate("SortExspression")) = "prsn_cognome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            Else
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            End If

            Dim ordinamento As Integer
            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            'Try
            '    If Valore = "" Then
            '        If Me.ViewState("intAnagrafica") = "" Then
            '            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
            '        Else
            '            oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
            '        End If
            '    End If
            'Catch ex As Exception
            '    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
            'End Try

            Dim totale As Decimal
            If ricalcola Then
                Me.ViewState("intCurPage") = 0
                Me.DGiscritti.CurrentPageIndex = 0
            End If

            Dim pageSize, pageIndex As Integer
            If Me.CTRLfiltroUtenti.isPaginated Then
                If Me.CTRLfiltroUtenti.PageSize > 0 Then
                    pageSize = Me.CTRLfiltroUtenti.PageSize

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
            Return oComunita.ElencaIscrittiSottoComunita(Me.CTRLfiltroComunita.ComunitaID, oPersona.Id, Session("LinguaID"), ElencoRuoli_ID, ElencoTipiAttivazione_ID, pageSize, pageIndex, Valore, oFiltroAnagrafica, oFiltroOrdinamento, oFiltroCampoOrdine, False, Main.FiltroUtenti.NoPassantiNoCreatori)
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

            dsTable.Tables(0).Columns.Add(New DataColumn("oCheck"))
            Dim i, totale As Integer

            totale = dsTable.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGiscritti.Visible = False
                LBnoIscritti.Visible = True
                Me.DGiscritti.VirtualItemCount = 0
            Else
                Me.DGiscritti.VirtualItemCount = dsTable.Tables(0).Rows(0).Item("Totale")
                dsTable.Tables(0).Columns.Add(New DataColumn("oIscrittoIl"))
                dsTable.Tables(0).Columns.Add("oCheckDisabled")
                dsTable.Tables(0).Columns.Add("Matricola")

                Dim PRSN_TPRL_Gerarchia, TPRL_Gerarchia As Integer
                Try
                    PRSN_TPRL_Gerarchia = Me.ViewState("PRSN_TPRL_Gerarchia")
                Catch ex As Exception
                    PRSN_TPRL_Gerarchia = "9999999"
                End Try

                If totale = 1 Then
                    Me.DGiscritti.Columns(0).Visible = False
                Else
                    Me.DGiscritti.Columns(0).Visible = True
                End If
                Dim HasMatricola As Boolean = False
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = dsTable.Tables(0).Rows(i)

                    If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                        If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                            oRow.Item("oIscrittoIl") = "&nbsp;--"
                        Else
                            oRow.Item("oIscrittoIl") = "&nbsp;" & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                        End If
                    Else
                        oRow.Item("oIscrittoIl") = "&nbsp;--"
                    End If
                    If viewstate("Paginazione") <> "si" Then

                    Else

                        Dim t As Integer
                        Dim selezionato() As String
                        selezionato = Me.HDazione.Value.Split(",")
                        For t = 1 To selezionato.Length - 2
                            If oRow.Item("RLPC_id") = selezionato(t) Then
                                oRow.Item("oCheck") = "checked"
                                Exit For
                            End If
                        Next
                    End If
                    If IsDBNull(oRow.Item("STDN_matricola")) Then
                        oRow.Item("Matricola") = "&nbsp;"
                    Else
                        HasMatricola = True
                        If oRow.Item("STDN_matricola") = "-1" Then
                            oRow.Item("Matricola") = Me.oResource.getValue("Excel.noMatricola")
                        Else
                            oRow.Item("Matricola") = oRow.Item("STDN_matricola")
                        End If
                    End If

                    If TPRL_Gerarchia < PRSN_TPRL_Gerarchia Then
                        oRow.Item("oCheckDisabled") = "disabled"
                    Else
                        oRow.Item("oCheckDisabled") = ""
                    End If
                    If oPersona.Id = oRow.Item("PRSN_ID") Then
                        oRow.Item("oCheckDisabled") = "disabled"
                    End If
                Next

                Me.DGiscritti.Columns(3).Visible = HasMatricola
                If Me.CTRLfiltroUtenti.isPaginated Then
                    Me.DGiscritti.AllowPaging = True
                    Me.DGiscritti.AllowCustomPaging = True
                    Me.DGiscritti.PageSize = Me.CTRLfiltroUtenti.PageSize
                Else
                    Me.DGiscritti.AllowPaging = False
                    Me.DGiscritti.AllowCustomPaging = False
                End If
                If totale > 0 Then
                    Me.DGiscritti.Visible = True

                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If ViewState("SortExspression") = "" Then
                        ViewState("SortExspression") = "PRSN_cognome"
                        ViewState("SortDirection") = "asc"
                    End If
                    oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")

                    Me.DGiscritti.Columns(2).Visible = False
                    Me.DGiscritti.DataSource = oDataview
                    Me.DGiscritti.DataBind()

                    LBnoIscritti.Visible = False
                    Me.DGiscritti.Visible = True
                Else
                    Me.DGiscritti.Visible = False
                    LBnoIscritti.Visible = True
                End If
            End If
        Catch ex As Exception

        End Try
        Me.SetSessioneStampa()
    End Sub
#End Region


#Region "Gestione Griglia"
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
    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGiscritti.ItemCreated
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
                    If Me.DGiscritti.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGiscritti.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGiscritti.HeaderStyle.CssClass
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
            If Not Me.DGiscritti.Columns(0).Visible Then
                n -= 1
            End If
            If Not Me.DGiscritti.Columns(3).Visible Then
                n -= 1
            End If
            e.Item.Cells(0).Attributes.Item("colspan") = n.ToString

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
            Dim PermessiAssociati As String
            Dim oServizioIscritti As New UCServices.Services_ManagementSottoIscritti
            Try
                oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Catch ex As Exception
                oServizioIscritti.PermessiAssociati = Me.GetPermessiForPage(oServizioIscritti.Codex)
            End Try


            Try
                If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                End If
            End Try

            'bottone informazioni
            Dim oImagebutton As ImageButton
            Dim Cell As New TableCell
            Dim TPPR_id As Integer
            Dim PRSN_ID As Integer

            Try
                PRSN_ID = e.Item.DataItem("PRSN_id")
                TPPR_id = e.Item.DataItem("PRSN_TPPR_id")
                Dim i_link2 As String
                i_link2 = "./InfoIscritto.aspx?TPPR_ID=" & TPPR_id & "&PRSN_ID=" & PRSN_ID
                Cell = CType(e.Item.Cells(0), TableCell)

                oImagebutton = Cell.FindControl("IMBinfo")
                'in base al tipo di utente decido la dimensione della finestra di popup
                Select Case TPPR_id
                    Case Main.TipoPersonaStandard.Studente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Docente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','520','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Tutor
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Esterno
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Amministrativo
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.SysAdmin
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Copisteria
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Dottorando
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.DocenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.StudenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Else
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                End Select
                oResource.setImageButton_Datagrid(Me.DGiscritti, oImagebutton, "IMBinfo", True, True, False, False)
                If oServizioIscritti.InfoEstese Or oServizioIscritti.Admin Or oServizioIscritti.Management Then
                    oImagebutton.Visible = True
                Else
                    oImagebutton.Visible = False
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oImagebuttonModifica As ImageButton
                PRSN_ID = e.Item.DataItem("PRSN_id")
                Dim i_link2 As String
                i_link2 = "./ModificaRuoloIscritto.aspx?CMNT_ID=" & Me.CTRLfiltroComunita.ComunitaID & "&PRSN_ID=" & PRSN_ID
                Cell = CType(e.Item.Cells(0), TableCell)

                oImagebuttonModifica = Cell.FindControl("IMBmodifica")
                If Not IsNothing(oImagebuttonModifica) Then
                    If oServizioIscritti.Change Or oServizioIscritti.Admin Or oServizioIscritti.Management Then
                        oResource.setImageButton_Datagrid(Me.DGiscritti, oImagebuttonModifica, "IMBmodifica", True, True, True)
                        oImagebuttonModifica.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','450','250','no','yes');return false;")
                        oImagebuttonModifica.Visible = True
                    Else
                        oImagebuttonModifica.Visible = False
                    End If
                End If

            Catch ex As Exception

            End Try
            Try
                Dim oImagebuttonDeiscrivi As ImageButton
                PRSN_ID = e.Item.DataItem("PRSN_id")
                Dim i_link2 As String
                i_link2 = "./DeiscriviIscritto.aspx?CMNT_ID=" & Me.CTRLfiltroComunita.ComunitaID & "&PRSN_ID=" & PRSN_ID & "&CMNT_PATH=" & Me.CTRLfiltroComunita.ComunitaPath
                Cell = CType(e.Item.Cells(0), TableCell)

                oImagebuttonDeiscrivi = Cell.FindControl("IMBCancella")
                If Not IsNothing(oImagebuttonDeiscrivi) Then
                    If oServizioIscritti.Delete Or oServizioIscritti.Admin Or oServizioIscritti.Management Then
                        oResource.setImageButton_Datagrid(Me.DGiscritti, oImagebuttonDeiscrivi, "IMBCancella", True, True, True, True)
                        oImagebuttonDeiscrivi.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','650','450','no','yes');return false;")
                        oImagebuttonDeiscrivi.Visible = True
                    Else
                        oImagebuttonDeiscrivi.Visible = False
                    End If
                End If
            Catch ex As Exception

            End Try

            Try
               
            Catch ex As Exception

            End Try
            ''colonna Cancella
            'Me.DGiscritti.Columns(2).Visible = (oServizioIscritti.Admin Or oServizioIscritti.Delete Or oServizioIscritti.Management Or oServizioIscritti.AddUser)
            ''colonna(Modifica)
            'Me.DGiscritti.Columns(3).Visible = (oServizioIscritti.Admin Or oServizioIscritti.Change Or oServizioIscritti.Management Or oServizioIscritti.AddUser)
            ''colonna(Informazioni)
            'Me.DGiscritti.Columns(4).Visible = (oServizioIscritti.Admin Or oServizioIscritti.Change Or oServizioIscritti.Management Or oServizioIscritti.InfoEstese)
            ''Colonna Checkbox
            'Me.DGiscritti.Columns(0).Visible = (Me.DGiscritti.Columns(2).Visible Or Me.DGiscritti.Columns(3).Visible)
        End If
    End Sub
    Private Sub DGiscritti_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGiscritti.ItemCommand
        Dim PRSN_ID As Integer

        Try
            PRSN_ID = DGiscritti.DataKeys.Item(e.Item.ItemIndex)
            If e.CommandName = "login" Then

            ElseIf e.CommandName = "blocca" Then

            ElseIf e.CommandName = "attiva" Then

            ElseIf e.CommandName = "cambioTipologia" Then

            ElseIf e.CommandName = "elimina" Then

            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region


#Region "Gestione Filtri"
    Private Sub LNBfind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBfind.Click
        Me.DGiscritti.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub

    Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.ViewState("intCurPage") = 0
        Me.DGiscritti.CurrentPageIndex = 0
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroComunita
        Lettera = CType(CInt(Lettera), Main.FiltroComunita).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControl("LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub

#End Region

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = code
        oResource.ResourcesName = "pg_IscrittiSottocomunita"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)

            .setDropDownList(Me.DDLTipoRicerca, -1)
            .setDropDownList(Me.DDLTipoRicerca, 1)
            .setDropDownList(Me.DDLTipoRicerca, 2)
            .setDropDownList(Me.DDLTipoRicerca, 3)
            .setDropDownList(Me.DDLTipoRicerca, 4)
            .setDropDownList(Me.DDLTipoRicerca, 5)
            .setDropDownList(Me.DDLTipoRicerca, 6)
            .setDropDownList(Me.DDLTipoRicerca, 7)

            .setLabel(Me.LBnoIscritti)
            .setLabel(Me.LBvalore_t)
            .setLabel(Me.LBtipoRicerca_t)

            .setHeaderDatagrid(Me.DGiscritti, 3, "matricola", True)
            .setHeaderDatagrid(Me.DGiscritti, 4, "cognome", True)
            .setHeaderDatagrid(Me.DGiscritti, 5, "nome", True)


            .setHeaderDatagrid(Me.DGiscritti, 6, "PRSN_Anagrafica", True)
            .setHeaderDatagrid(Me.DGiscritti, 7, "PRSN_login", True)
            .setHeaderDatagrid(Me.DGiscritti, 8, "Mail", True)
            .setHeaderDatagrid(Me.DGiscritti, 9, "TPRL_nome", True)
            .setHeaderDatagrid(Me.DGiscritti, 10, "TPPR_descrizione", True)
            .setHeaderDatagrid(Me.DGiscritti, 11, "RLPC_ultimoCollegamento", True)
            .setHeaderDatagrid(Me.DGiscritti, 12, "RLPC_IscrittoIl", True)

            .setLinkButton(Me.LNBfind, True, True)
            .setLinkButton(Me.LNBexcel, True, True)
            .setLinkButton(Me.LNBstampa, True, True)
            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)

            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControl("LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next
        End With
    End Sub
#End Region

    Private Sub SetSessioneStampa()
        Dim oArray(14) As String

        Session("Stampa") = ""
        Try
            oArray(0) = Me.CTRLfiltroComunita.ComunitaID
            oArray(1) = Me.CTRLfiltroComunita.ComunitaPath
            oArray(2) = Me.CTRLfiltroUtenti.ActivationSelected
            oArray(3) = Me.CTRLfiltroUtenti.AllActivation
            oArray(4) = Me.CTRLfiltroUtenti.AllRoles
            oArray(5) = Me.CTRLfiltroUtenti.RoleSelected
            oArray(6) = Me.CTRLfiltroUtenti.isPaginated
            oArray(7) = Me.CTRLfiltroUtenti.PageSize

            oArray(8) = Me.ViewState("SortExspression")
            oArray(9) = Me.ViewState("SortDirection")
            oArray(10) = Me.ViewState("intAnagrafica")
            oArray(11) = Me.ViewState("intCurPage")
            oArray(12) = Me.DDLTipoRicerca.SelectedValue
            oArray(13) = Me.TXBValore.Text
            Session("Stampa") = oArray
        Catch ex As Exception

        End Try

    End Sub
    Private Sub CTRLfiltroUtenti_AggiornaDati(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLfiltroUtenti.AggiornaDati
        Me.Bind_Griglia(True)
    End Sub

    Private Sub CTRLfiltroComunita_AggiornaDati(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLfiltroComunita.AggiornaDati
        Dim oServizioIscritti As New Services_ManagementSottoIscritti

        Me.GetPermessiForPage(oServizioIscritti.Codex)
        Me.CTRLfiltroUtenti.Bind_Dati(Me.CTRLfiltroComunita.ComunitaID, Me.CTRLfiltroComunita.ComunitaPath)
        Me.Bind_Griglia(True)
    End Sub

    Private Sub LNBexcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexcel.Click
        Try
            Dim FileName As String = "Iscritti.xls"
            Dim oStringWriter As System.IO.StringWriter = New System.IO.StringWriter
            Dim oHTMLWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)


            Response.Buffer = True
            Page.Response.Clear()

            Me.Bind_GrigliaExcel()


            Me.TBLexcel.RenderControl(oHTMLWriter)

            Dim OpenType As String = "attachment"
            Page.Response.ContentType = "application/ms-excel"
            Page.Response.AddHeader("Content-Disposition", OpenType + ";filename=" + FileName)

            Page.Response.Write("<html>" & vbCrLf)
			Page.Response.Write("<head>" & vbCrLf)
            Page.Response.Write("<META charset=utf-8>" & vbCrLf)

            context.Response.Output.Write(ConvertEncoding(oStringWriter) & vbCrLf)

            Page.Response.Write("</head>" & vbCrLf)
            Page.Response.Write("</body>")

            Response.Flush()
            context.Response.End()
        Catch ex As Exception

        End Try
    End Sub

#Region "Funzioni Excel"
    Private Function ConvertEncoding(ByVal oStringWriter As System.IO.StringWriter) As String
        Dim unicodeString As String = oStringWriter.ToString

        ' Create two different encodings.
        Dim ascii As System.Text.Encoding = System.Text.Encoding.UTF8
        Dim [unicode] As System.Text.Encoding = System.Text.Encoding.Unicode

        ' Convert the string into a byte[].
        Dim unicodeBytes As Byte() = [unicode].GetBytes(unicodeString)

        ' Perform the conversion from one encoding to the other.
        Dim asciiBytes As Byte() = System.Text.Encoding.Convert([unicode], ascii, unicodeBytes)

        ' Convert the new byte[] into a char[] and then into a string.
        ' This is a slightly different approach to converting to illustrate
        ' the use of GetCharCount/GetChars.
        Dim asciiChars(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)) As Char
        ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)
        Dim asciiString As New String(asciiChars)

        Return asciiString
    End Function

    Private Sub ExcelHeader()
        Dim oTableRow As New TableRow
        Dim oTableCell As New TableCell
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona

        Try
            oPersona = Session("objPersona")
            oComunita.Id = Me.CTRLfiltroComunita.ComunitaID
            oComunita.Estrai()

            oTableCell.ColumnSpan = 8
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Font.Bold = True
            oTableCell.Font.Size = FontUnit.Point(20)
            oTableCell.Font.Name = "Verdana"
            oTableCell.ForeColor = System.Drawing.Color.Black
            oTableCell.VerticalAlign = VerticalAlign.Middle
            oTableCell.Text = oResource.getValue("Excel.organizzazione")
            oTableCell.Text = oTableCell.Text.Replace("#c#", oComunita.Organizzazione.RagioneSociale)

            oTableRow.Cells.Add(oTableCell)
            Me.TBLexcel.Rows.Add(oTableRow)

            oTableRow = New TableRow
            oTableCell = New TableCell

            oTableCell.Text = oResource.getValue("Excel.comunita")
            oTableCell.Text = oTableCell.Text.Replace("#c#", oComunita.Nome)
            oTableCell.ColumnSpan = 8
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Font.Bold = True
            oTableCell.Font.Size = FontUnit.Point(16)
            oTableCell.Font.Name = "Verdana"
            oTableCell.ForeColor = System.Drawing.Color.DarkBlue
            oTableCell.VerticalAlign = VerticalAlign.Middle
            oTableRow.Cells.Add(oTableCell)
            Me.TBLexcel.Rows.Add(oTableRow)

            oTableRow = New TableRow
            oTableCell = New TableCell
            oTableCell.ColumnSpan = 8
            oTableCell.Text = oResource.getValue("Excel.stampato")
            oTableCell.Font.Italic = True
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Left
            oTableCell.Text = oTableCell.Text.Replace("#users#", oPersona.Cognome & " " & oPersona.Nome)
            oTableCell.Text = oTableCell.Text.Replace("#data#", FormatDateTime(Now, DateFormat.LongDate))
            oTableRow.Cells.Add(oTableCell)
            Me.TBLexcel.Rows.Add(oTableRow)

            oTableRow = New TableRow
            oTableCell = New TableCell
            oTableCell.ColumnSpan = 8
            oTableCell.Text = "&nbsp"
            oTableCell.Height = Unit.Pixel(20)
            Me.TBLexcel.Rows.Add(oTableRow)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExcelHeader_UserList()
        Dim oTableRow As New TableRow
        Dim oTableCell As New TableCell


        Try


            oTableRow = New TableRow

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = "&nbsp;"
            oTableCell.Width = Unit.Pixel(70)
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("DGiscritti.matricola.HeaderText")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue

            oTableCell.Text = oResource.getValue("DGiscritti.PRSN_Anagrafica.HeaderText")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("DGiscritti.PRSN_login.HeaderText")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("DGiscritti.Mail.HeaderText")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("DGiscritti.TPRL_nome.HeaderText")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("DGiscritti.RLPC_IscrittoIl.HeaderText")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("DGiscritti.RLPC_ultimoCollegamento.HeaderText")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = "&nbsp;"
            oTableRow.Cells.Add(oTableCell)

            Me.TBLexcel.Rows.Add(oTableRow)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_GrigliaExcel(Optional ByVal ricalcola As Boolean = False)
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita
        Dim oDataSet As New DataSet

        Try
            oPersona = Session("objPersona")
            oDataSet = FiltraggioDati(ricalcola)

            oDataSet.Tables(0).Columns.Add(New DataColumn("oCheck"))
            Dim i, totale As Integer



            Me.ExcelHeader()

            totale = oDataSet.Tables(0).Rows.Count - 1

            Dim oTableRow As New TableRow
            Dim oTableCell As New TableCell


            Me.ExcelHeader_UserList()

            For i = 0 To totale
                Dim oRow As DataRow
                oRow = oDataSet.Tables(0).Rows(i)

                oTableRow = New TableRow

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.Black
                oTableCell.HorizontalAlign = HorizontalAlign.Right
                oTableCell.Text = i + 1
                oTableCell.Width = Unit.Pixel(70)
                oTableRow.Cells.Add(oTableCell)


                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                If IsDBNull(oRow.Item("STDN_matricola")) Then
                    oTableCell.Text = "&nbsp;"
                Else
                    oTableCell.Text = oRow.Item("STDN_matricola")
                End If
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("PRSN_Anagrafica") & " (" & oRow.Item("TPPR_descrizione") & ")"
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("PRSN_login")
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("PRSN_mail")
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("TPRL_nome")
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                    If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                        oTableCell.Text = "--"
                    Else
                        oTableCell.Text = FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                    End If
                Else
                    oTableCell.Text = "--"
                End If
                oTableRow.Cells.Add(oTableCell)


                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                If IsDBNull(oRow.Item("RLPC_ultimoCollegamento")) = False Then
                    oTableCell.Text = CDate(oRow.Item("RLPC_ultimoCollegamento")).ToString("dd/MM/yy HH:mm:ss")
                Else
                    oTableCell.Text = "--"
                End If
                oTableRow.Cells.Add(oTableCell)


                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left

                If oRow.Item("RLPC_Attivato") = True And oRow.Item("RLPC_Abilitato") = True Then
                    oTableCell.Text = oResource.getValue("abilitato." & StringaAbilitato.abilitato)
                ElseIf oRow.Item("RLPC_Attivato") = True And oRow.Item("RLPC_Abilitato") = False Then
                    oTableCell.Text = oResource.getValue("abilitato." & StringaAbilitato.bloccato)
                ElseIf oRow.Item("RLPC_Attivato") = False Then
                    oTableCell.Text = oResource.getValue("abilitato." & StringaAbilitato.inAttesa)
                Else
                    oTableCell.Text = "&nbsp;"
                End If
                oTableRow.Cells.Add(oTableCell)
                Me.TBLexcel.Rows.Add(oTableRow)
            Next
        Catch ex As Exception
            Dim d As Integer
            d = 5
        End Try
    End Sub
#End Region

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class


'    	    function Stampa() {
'//    	        OpenWin('./stampaiscritti.aspx?TPRL_id=' + document.forms[0].DDLTipoRuolo.value, '700', '600', 'yes', 'yes')
'    	        OpenWin('./stampaiscritti.aspx?TPRL_id=' + <%= Me.DDLTipoRuolo.ClientId %>.value, '700', '600', 'yes', 'yes')
'    	        return false;
'    	    }	