Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices

Public Class UC_GestioneiscrittiForum
    Inherits System.Web.UI.UserControl
    Private oResourceIscritti As ResourceManager

    Private _PageUtility As OLDpageUtility
    Private _BaseUrl As String
    Private _Servizio As Services_Forum
    Private _CurrentForumRoleID As Main.RuoloForumStandard

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public ReadOnly Property CurrentForumRoleID() As Main.RuoloForumStandard
        Get
            If _CurrentForumRoleID = RuoloForumStandard.NotSet Then
                Try
                    Dim oForum As New Forum.COL_Forums With {.Id = CurrentForumID}
                    _CurrentForumRoleID = oForum.getRuoloForIscritto(Session("RLPC_ID"), False)
                Catch ex As Exception

                End Try
            End If
            Return _CurrentForumRoleID
        End Get
    End Property

    Private ReadOnly Property CurrentService() As COL_BusinessLogic_v2.UCServices.Services_Forum
        Get
            If IsNothing(_Servizio) Then
                If Me.PageUtility.isPortalCommunity Then
                    Dim oTipoPersonaID As Integer = Main.TipoPersonaStandard.Guest
                    Try
                        oTipoPersonaID = Me.PageUtility.CurrentUser.TipoPersona.ID
                    Catch ex As Exception

                    End Try
                    _Servizio = Services_Forum.Create
                    _Servizio.AccessoForum = True
                    _Servizio.GestioneForum = (oTipoPersonaID = Main.TipoPersonaStandard.AdminSecondario OrElse oTipoPersonaID = Main.TipoPersonaStandard.SysAdmin)
                ElseIf Me.PageUtility.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_Forum(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.PageUtility.AmministrazioneComunitaID, Services_Forum.Codex))
                Else
					_Servizio = Me.PageUtility.GetCurrentServices.Find(Services_Forum.Codex)
					If IsNothing(_Servizio) Then
						_Servizio = Services_Forum.Create
					End If
                End If
            End If
            Return _Servizio
        End Get
    End Property
    Private ReadOnly Property CurrentForumID() As Integer
        Get
            Try
                Return Session("IdForum")
            Catch ex As Exception

            End Try
            Return 0
        End Get
    End Property



    Public Enum Inserimento
        NessunaSelezione = -3
        TipoRuoloMancante = -2
        ErroreModifica = -1
        ErroreInserimento = 0
        InserimentoOk = 1
        ModificaAvvenuta = 2
        SetupOk = 3
    End Enum

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Private Enum StringaAbilita
        Abilita = 1
        Disabilita = 0
    End Enum

    Public Event AggiornaMenu(ByVal showModifica As Boolean, ByVal abilitato As Boolean)

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

#Region "griglia"
    Protected WithEvents PNLiscritti As System.Web.UI.WebControls.Panel
    Protected WithEvents LBpageRecord As System.Web.UI.WebControls.Label
    Protected WithEvents DGiscrittiForum As System.Web.UI.WebControls.DataGrid
    Protected WithEvents IMBmodificaIscritto As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IMBcancellaIscritto As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LKBdisAbilita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DDLpaginazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PNLpaginazione As System.Web.UI.WebControls.Panel
#End Region
    Protected WithEvents PNLfiltri As System.Web.UI.WebControls.Panel
#Region "search"
    Protected WithEvents LBnome As System.Web.UI.WebControls.Label
    Protected WithEvents LBcognome As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoloForum As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoloComunita As System.Web.UI.WebControls.Label
    Protected WithEvents DDLruoloForumRic As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLruoloComunitaRic As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcognome As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNcerca As System.Web.UI.WebControls.Button
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
#End Region
#Region "cambio ruolo"
    Protected WithEvents LBcognomeNome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoloForum_t As System.Web.UI.WebControls.Label
    Protected WithEvents PNLmodificaRuolo As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLruoloForum As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBCognomeNome As System.Web.UI.WebControls.Label
    Protected WithEvents HDrlpc As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region
#Region "norecord"
    Protected WithEvents LBnorecord As System.Web.UI.WebControls.Label
    Protected WithEvents PNLnorecord As System.Web.UI.WebControls.Panel
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If IsNothing(oResourceIscritti) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
           
            Me.ViewState("intCurPage") = 0
            Me.ViewState("intAnagrafica") = CType(Main.FiltroComunita.tutti, Main.FiltroComunita)
            Me.LKBtutti.CssClass = "lettera_Selezionata"
            Me.SetStartupScripts()
        End If

    End Sub
    Private Function SetStartupScripts()
        'aggiunge ai link button le proprietà da visualizzare nella barra di stato
        Try
            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControl("LKB" & Chr(i))
                Dim Carattere As String = Chr(i)
                If IsNothing(oLinkButton) = False Then
                    oResourceIscritti.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next
        Catch ex As Exception

        End Try

    End Function

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceIscritti = New ResourceManager

        oResourceIscritti.UserLanguages = Code
        oResourceIscritti.ResourcesName = "pg_UC_GestioneiscrittiForum"
        oResourceIscritti.Folder_Level1 = "Forum"
        oResourceIscritti.Folder_Level2 = "UC"
        oResourceIscritti.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceIscritti
            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)
            .setLabel(LBnome)
            .setLabel(LBcognome)
            .setLabel(LBruoloForum)
            .setLabel(LBruoloComunita)
            .setLabel(LBpageRecord)
            .setLabel(LBcognomeNome_t)
            .setLabel(LBruoloForum_t)
            .setLabel(LBnorecord)
            .setButton(BTNcerca)

            .setHeaderDatagrid(Me.DGiscrittiForum, 2, "PRSN_Anagrafica", True)
            .setHeaderDatagrid(Me.DGiscrittiForum, 4, "TPRF_nome", True)
            .setHeaderDatagrid(Me.DGiscrittiForum, 5, "TPRL_nome", True)
            .setHeaderDatagrid(Me.DGiscrittiForum, 6, "RLPF_Abilitato", True)
        End With

    End Sub
#End Region

    Public Sub SetupControllo(ByVal ReloadFiltri As Boolean, Optional ByVal ricalcola As Boolean = False)
        Me.PNLmodificaRuolo.Visible = False
        Me.PNLfiltri.Visible = True
        If ReloadFiltri Then
            Me.Bind_Ruoli_Forum()
            Me.Bind_TipoRuoloComunita()
        End If
        Me.Bind_Griglia_Iscritti(ricalcola)
    End Sub

#Region "Gestione e Bind Griglia Iscritti"

    Private Sub Bind_Griglia_Iscritti(Optional ByVal ricalcola As Boolean = False)
        Dim oDataset As DataSet
        Dim totale As Integer

        Try
            oDataset = FiltraggioDati(ricalcola)
            totale = oDataset.Tables(0).Rows.Count
            If ricalcola Then
                Me.PNLmodificaRuolo.Visible = False
            End If
            If totale = 0 Then 'se datagrid vuota
                Me.PNLiscritti.Visible = False
                PNLpaginazione.Visible = False
                PNLnorecord.Visible = True
                'LBnorecord.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
                oResourceIscritti.setLabel_To_Value(LBnorecord, "LBnorecord.text")
            Else
                Try
                    Dim oRLPF_Abilitato As New DataColumn
                    oRLPF_Abilitato.ColumnName = "oRLPF_Abilitato"
                    oDataset.Tables(0).Columns.Add(oRLPF_Abilitato)

                    Dim i, pos, TotaleRecord As Integer
                    TotaleRecord = oDataset.Tables(0).Rows.Count

                    For i = 0 To TotaleRecord - 1
                        Dim oRow As DataRow
                        oRow = oDataset.Tables(0).Rows(i)
                        Try
                            If IsDBNull(oRow.Item("RLPF_Abilitato")) = False Then
                                If CBool(oRow.Item("RLPF_Abilitato")) Then
                                    oRow.Item("oRLPF_Abilitato") = oResourceIscritti.getValue("DGiscrittiForum.blocca.0.text") '"Blocca"
                                Else
                                    oRow.Item("oRLPF_Abilitato") = oResourceIscritti.getValue("DGiscrittiForum.blocca.1.text") '"Sblocca"
                                End If
                            End If

                        Catch ex As Exception

                        End Try
                    Next
                    If TotaleRecord < Me.DDLpaginazione.Items(0).Value Then
                        Me.DGiscrittiForum.PagerStyle.Position = PagerPosition.Bottom
                    Else
                        Me.DGiscrittiForum.PagerStyle.Position = PagerPosition.TopAndBottom
                    End If
                Catch ex As Exception

                End Try

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression2") = "" Then
                    viewstate("SortExspression2") = "PRSN_Anagrafica"
                    viewstate("SortDirection2") = "desc"
                End If
                oDataview.Sort = viewstate("SortExspression2") & " " & viewstate("SortDirection2")
                Me.DGiscrittiForum.DataSource = oDataview
                Me.PNLiscritti.Visible = True
                PNLnorecord.Visible = False
                DGiscrittiForum.DataBind()
            End If
        Catch ex As Exception 'se c'è qualche errore nascondo la DG e mostro messaggio di errore
            Me.PNLiscritti.Visible = False
            PNLpaginazione.Visible = False
            PNLnorecord.Visible = True
            oResourceIscritti.setLabel_To_Value(LBnorecord, "LBnorecord.text")
            'LBnorecord.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
        End Try
    End Sub

    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Dim oForum As New COL_Forums
        oForum.Id = Session("IdForum")
        Dim TPRF_id, TPRL_id As Integer

        TPRF_id = Me.DDLruoloForumRic.SelectedValue
        TPRL_id = Me.DDLruoloComunitaRic.SelectedValue

        If ViewState("SortExspression") = "" Or LCase(ViewState("SortExspression")) = "PRSN_Anagrafica" Then
            oForum.FiltroCampoOrdine = oForum.FltrOrdine.anagrafica

        ElseIf LCase(ViewState("SortExspression")) = "TPRF_nome" Then
            oForum.FiltroCampoOrdine = oForum.FltrOrdine.RuoloForum

        ElseIf LCase(ViewState("SortExspression")) = "TPRL_nome" Then
            oForum.FiltroCampoOrdine = oForum.FltrOrdine.RuoloComunita
        Else
            oForum.FiltroCampoOrdine = oForum.FltrOrdine.anagrafica
        End If


        If ViewState("SortDirection") = "" Or ViewState("SortDirection") = "asc" Then
            oForum.FiltroOrdinamento = oForum.FltrOrdinamento.Crescente
        Else
            oForum.FiltroOrdinamento = oForum.FltrOrdinamento.Decrescente
        End If

        'definisco il filtraggio per lettera !
        Try
            If Me.ViewState("intAnagrafica") = "" Then
                oForum.FiltroAnagrafica = COL_Forums.FltrAnagrafica.tutti
            Else
                oForum.FiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), COL_Forums.FltrAnagrafica)
            End If
        Catch ex As Exception
            oForum.FiltroAnagrafica = COL_Forums.FltrAnagrafica.tutti
        End Try
        Dim nome, cognome As String
        nome = Me.TXBnome.Text
        cognome = Me.TXBcognome.Text

        Try

            If nome <> "" Or cognome <> "" Then
                oForum.FiltroAnagrafica = COL_Forums.FltrAnagrafica.tutti
            End If
            Dim totale As Decimal
            'If ISTT_ID > 0 Then
            If ricalcola Then
                Me.DGiscrittiForum.VirtualItemCount = oForum.GetPersoneIscrittePaginateTotale(TPRF_id, TPRL_id, nome, cognome)

                totale = Decimal.Parse(Me.DGiscrittiForum.VirtualItemCount / Me.DGiscrittiForum.PageSize)
                Me.ViewState("intCurPage") = 0
                Me.DGiscrittiForum.CurrentPageIndex = 0
            End If
            Return oForum.GetPersoneIscrittePaginate(Session("LinguaID"), TPRF_id, Me.DGiscrittiForum.PageSize, Me.ViewState("intCurPage"), TPRL_id, nome, cognome)
            'Else
            '    Me.ViewState("intCurPage") = 0
            '    Me.DGiscrittiForum.VirtualItemCount = 0
            '    Me.DGiscrittiForum.CurrentPageIndex = 0
            '    Return oDataset
            'End If
        Catch ex As Exception
            Return oDataset
        End Try
    End Function
    Private Sub SetupLingua()
        Try
            If IsNumeric(Session("LinguaID")) And Session("LinguaCode") <> "" Then

            Else
                Dim LinguaCode As String

                LinguaCode = "en-US"
                Try
                    LinguaCode = Request.UserLanguages(0)
                Catch ex As Exception
                    LinguaCode = "en-US"
                End Try
                If Request.Browser.Cookies = True Then
                    Try
                        LinguaCode = Request.Cookies("LinguaCode").Value
                    Catch ex As Exception

                    End Try
                End If
                'Setto ora il valore nelle variabili di sessione.....
				Dim oLingua As New Lingua
				oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
				If Not IsNothing(oLingua) Then
					Session("LinguaID") = oLingua.Id
					Session("LinguaCode") = oLingua.Codice
				Else
					Session("LinguaID") = 1
					Session("LinguaCode") = "it-IT"
				End If
            End If
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        Catch exUserLanguages As Exception
        End Try
    End Sub

    Private Sub DGiscrittiForum_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGiscrittiForum.ItemCommand
        Select Case e.CommandName
            Case "modifica"
                Try
                    Me.HDrlpc.Value = CInt(Me.DGiscrittiForum.DataKeys.Item(e.Item.ItemIndex))

                    Bind_RuoliForumLimitato()
                    Me.PNLiscritti.Visible = False
                    Me.PNLnorecord.Visible = False
                    Me.PNLfiltri.Visible = False
                    Me.PNLmodificaRuolo.Visible = True
                    Me.LBCognomeNome.Text = DGiscrittiForum.Items(e.Item.ItemIndex).Cells(2).Text
                    Me.DDLruoloForum.SelectedValue = DGiscrittiForum.Items(e.Item.ItemIndex).Cells(3).Text
                    If Me.DDLruoloForum.SelectedValue = -1 Then
                        RaiseEvent AggiornaMenu(True, False)
                    Else
                        RaiseEvent AggiornaMenu(True, True)
                    End If
                Catch ex As Exception

                End Try
            Case "remove"
                Dim oForum As New COL_Forums With {.Id = Me.CurrentForumID}
                Try
                    oForum.DisiscriviUtente(CInt(Me.DGiscrittiForum.DataKeys.Item(e.Item.ItemIndex)))
                Catch ex As Exception

                End Try
                Me.Bind_Griglia_Iscritti()
            Case "abilitazione"
                Try
                    Dim oForum As New COL_Forums With {.Id = Me.CurrentForumID}
                    oForum.bloccaIscritto(CInt(Me.DGiscrittiForum.DataKeys.Item(e.Item.ItemIndex)), CBool(e.CommandArgument))

                Catch ex As Exception

                End Try
                Me.Bind_Griglia_Iscritti()
        End Select

    End Sub
    Private Sub DGiscrittiForum_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGiscrittiForum.ItemCreated
        Dim i As Integer
        If IsNothing(oResourceIscritti) Then
            SetCulture(Session("LinguaCode"))
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
                    If Me.DGiscrittiForum.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResourceIscritti.setHeaderOrderbyLink_Datagrid(Me.DGiscrittiForum, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResourceIscritti.setHeaderOrderbyLink_Datagrid(Me.DGiscrittiForum, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGiscrittiForum.HeaderStyle.CssClass
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
                                oResourceIscritti.setHeaderOrderbyLink_Datagrid(Me.DGiscrittiForum, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGiscrittiForum.HeaderStyle.CssClass
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
                    oResourceIscritti.setPageDatagrid(Me.designerPlaceholderDeclaration, oLinkbutton)
                End Try
            Next
        End If
    End Sub
    Private Sub DGiscrittiForum_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGiscrittiForum.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLNBremove As LinkButton = Nothing, oLNBmodifica As LinkButton = Nothing
            oLNBremove = e.Item.FindControl("LNBremove")
            oLNBmodifica = e.Item.FindControl("LNBmodifica")

            Dim oIscrizioneID As Integer = CInt(Me.DGiscrittiForum.DataKeys.Item(e.Item.ItemIndex))
            Dim RuoloCorrenteID As Integer = e.Item.DataItem("RLPF_TPRF_id")

            If Not IsNothing(oLNBmodifica) Then
                Try
                    If CurrentService.GestioneForum Then
                        oLNBmodifica.Visible = True
                    ElseIf oIscrizioneID = Session("RLPC_id") Then
                        oLNBmodifica.Visible = False
                    ElseIf _CurrentForumRoleID <= RuoloCorrenteID Then
                        oLNBmodifica.Visible = True
                    Else
                        oLNBmodifica.Visible = False
                    End If
                Catch ex As Exception

                End Try
                Me.oResourceIscritti.setLinkButton(oLNBremove, True, True, , True)
                Me.oResourceIscritti.setLinkButton(oLNBmodifica, True, True)
                oLNBremove.Text = String.Format(oLNBremove.Text, Me.BaseUrl & "images/x.gif", oLNBremove.ToolTip)
                oLNBmodifica.Text = String.Format(oLNBmodifica.Text, Me.BaseUrl & "images/m.gif", oLNBmodifica.ToolTip)
                oLNBremove.Visible = oLNBmodifica.Visible
            End If


            Dim oLNBabilitazione As LinkButton = Nothing
            oLNBabilitazione = e.Item.FindControl("LKBdisAbilita")
            Dim cssLink As String = "ROW_ItemLink_Small"

            Try
                If IsNothing(oLNBabilitazione) = False Then
                    Dim isAbilitato As Boolean = CBool(e.Item.DataItem("RLPF_Abilitato"))
                    oLNBabilitazione.CommandArgument = Not isAbilitato

                    oResourceIscritti.setLinkButton_Datagrid(Me.DGiscrittiForum, oLNBabilitazione, IIf(isAbilitato, "blocca." & Me.StringaAbilita.Disabilita, "blocca." & Me.StringaAbilita.Abilita), True, True, , True)
                    oLNBabilitazione.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLNBabilitazione.CssClass = cssLink

                    oLNBabilitazione.Visible = (CurrentService.GestioneForum) OrElse Me.CurrentForumRoleID = RuoloForumStandard.Amministratore OrElse (Me.CurrentForumRoleID = RuoloForumStandard.Moderatore And RuoloCorrenteID <> Main.RuoloForumStandard.Amministratore)

                    If Not (CurrentService.GestioneForum AndAlso Not isAbilitato) AndAlso oIscrizioneID = Session("RLPC_id") Then
                        oLNBabilitazione.Visible = False
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub


    Public Sub LKBdisAbilita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBdisAbilita.Click
        Dim oForum As New COL_Forums
        Dim RLPC_ID As Integer
        Dim otableCell As New System.Web.UI.WebControls.TableCell
        Dim oDatagridItem As System.Web.UI.WebControls.DataGridItem

        Try
            otableCell = sender.parent
            oDatagridItem = otableCell.Parent
            If CInt(DGiscrittiForum.Items(oDatagridItem.ItemIndex).Cells(3).Text) > Session("IdRuolo") Then
                '    olinkbutton.Enabled = False

                RLPC_ID = CInt(Me.DGiscrittiForum.DataKeys.Item(oDatagridItem.ItemIndex))
                oForum.Id = Session("IdForum")
                If sender.CommandName = oResourceIscritti.getValue("DGiscrittiForum.blocca.0.text") Then '"Blocca"
                    oForum.bloccaIscritto(RLPC_ID, False)
                ElseIf sender.CommandName = oResourceIscritti.getValue("DGiscrittiForum.blocca.1.text") Then '"Sblocca"
                    oForum.bloccaIscritto(RLPC_ID, True)
                End If
            Else

            End If
        Catch ex As Exception

        End Try
        Me.Bind_Griglia_Iscritti()
    End Sub

    Private Sub SortElenco2(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGiscrittiForum.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression2")
        oSortDirection = viewstate("SortDirection2")
        viewstate("SortExspression2") = e.SortExpression

        If LCase(e.SortExpression) = LCase(oSortExpression) Then
            If viewstate("SortDirection2") = "asc" Then
                viewstate("SortDirection2") = "desc"
            Else
                viewstate("SortDirection2") = "asc"
            End If
        End If
        Bind_Griglia_Iscritti()
    End Sub

    Private Sub CambioPagina2(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGiscrittiForum.PageIndexChanged
        Dim oSortExpression, oSortDirection As String
        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        Bind_Griglia_Iscritti()
    End Sub
    Private Sub Cambio_NumPagine(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpaginazione.SelectedIndexChanged
        DGiscrittiForum.PageSize = DDLpaginazione.Items(DDLpaginazione.SelectedIndex).Value
        DGiscrittiForum.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Bind_Griglia_Iscritti()
    End Sub

    Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
        Me.PNLpaginazione.Visible = False
        If oRecord > Me.DGiscrittiForum.PageSize Or oRecord > 10 Or Me.DGiscrittiForum.VirtualItemCount > Me.DGiscrittiForum.PageSize Then
            Me.DGiscrittiForum.AllowPaging = True
            Me.DGiscrittiForum.PageSize = Me.DDLpaginazione.SelectedItem.Value
            PNLpaginazione.Visible = True
        Else
            Me.DGiscrittiForum.AllowPaging = False
            PNLpaginazione.Visible = False
        End If
        If oRecord < 0 Then
            Me.PNLiscritti.Visible = False
            PNLpaginazione.Visible = False
            PNLnorecord.Visible = True
            'LBnorecord.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
            oResourceIscritti.setLabel_To_Value(LBnorecord, "LBnorecord.text")
        End If
    End Sub

#End Region

#Region "bind"
    Private Sub Bind_TipoRuoloComunita()
        'questa carica i ruoli della comunità in cui si trova il forum
        Me.DDLruoloComunitaRic.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita
            oComunita.Id = Session("IdComunita")
            oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)

            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
					Dim oListItem As New ListItem
					If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRL_nome")) Then
						oListItem.Text = "--"
					Else
						oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRL_nome")
					End If
					oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRL_ID")
					Me.DDLruoloComunitaRic.Items.Add(oListItem)
				Next
				Me.DDLruoloComunitaRic.Items.Insert(0, New ListItem("< Tutti >", -1))
				Me.DDLruoloComunitaRic.SelectedValue = "-1"
			Else
				Me.DDLruoloComunitaRic.Items.Add(New ListItem("< nessun ruolo >", -1))
			End If
		Catch ex As Exception
			Me.DDLruoloComunitaRic.Items.Add(New ListItem("< nessun ruolo >", -1))
		End Try
		Me.oResourceIscritti.setDropDownList(Me.DDLruoloComunitaRic, -1)
	End Sub
	Private Sub Bind_Ruoli_Forum()
		Me.DDLruoloForumRic.Items.Clear()
		Try
			Dim oDataset As DataSet
			Dim i, Totale As Integer
			Dim oTipoRuoloForum As New COL_TipoRuoloForum
			oDataset = oTipoRuoloForum.Elenca(Session("LinguaID"))

			Totale = oDataset.Tables(0).Rows.Count()
			If Totale > 0 Then
				Totale = Totale - 1
				For i = 0 To Totale
					Dim oListItem As New ListItem
					If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRF_nome")) Then
						oListItem.Text = "--"
					Else
						oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")
					End If
					oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
					Me.DDLruoloForumRic.Items.Add(oListItem)
				Next
				Me.DDLruoloForumRic.Items.Insert(0, New ListItem("< Tutti >", -1))
				Me.DDLruoloForumRic.SelectedValue = -1
			Else
				Me.DDLruoloForumRic.Items.Add(New ListItem("< nessun ruolo >", -1))
			End If
		Catch ex As Exception
			Me.DDLruoloForumRic.Items.Add(New ListItem("< nessun ruolo >", -1))
		End Try
		Me.oResourceIscritti.setDropDownList(Me.DDLruoloForumRic, -1)
	End Sub
    Private Sub Bind_RuoliForumLimitato()
        Me.DDLruoloForum.Items.Clear() 'ddlper la modifica ruolo
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oTipoRuoloForum As New COL_TipoRuoloForum
            oDataset = oTipoRuoloForum.Elenca(Session("LinguaID"))

            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 AndAlso Not (Me.CurrentService.GestioneForum = False AndAlso Me.CurrentForumRoleID > RuoloForumStandard.Moderatore) Then
                Totale = Totale - 1
                For i = 0 To Totale
                    Dim oListItem As New ListItem

                    If Not IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRF_nome")) Then
                        oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
                        oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")
                        If Me.CurrentService.GestioneForum Then
                            Me.DDLruoloForum.Items.Add(oListItem)
                        ElseIf Me.CurrentForumRoleID >= Me.CurrentForumRoleID Then
                            Me.DDLruoloForum.Items.Add(oListItem)
                        End If
                    End If
                    'If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRF_nome")) Then
                    '    oListItem.Text = "--"
                    'ElseIf RuoloForum > -1 Then
                    '    If oDataset.Tables(0).Rows(i).Item("TPRF_ID") > RuoloForum Then
                    '        oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
                    '        oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")

                    '    End If
                    'ElseIf oDataset.Tables(0).Rows(i).Item("TPRF_ID") > Session("RuoloForum") Then
                    '    oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
                    '    oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")
                    '    Me.DDLruoloForum.Items.Add(oListItem)
                    'End If
                Next
            Else
                Me.DDLruoloForum.Items.Add(New ListItem("< nessun ruolo >", -1))
            End If
        Catch ex As Exception
            Me.DDLruoloForum.Items.Add(New ListItem("< nessun ruolo >", -1))
        End Try
        Me.oResourceIscritti.setDropDownList(Me.DDLruoloForum, -1)
    End Sub
#End Region
#Region "Filtri"

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        Bind_Griglia_Iscritti(True)
    End Sub

    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.Bind_Griglia_Iscritti(True)
    End Sub

    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As COL_Forums.FltrAnagrafica
        Lettera = CType(CInt(Lettera), COL_Forums.FltrAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControl("LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub

#End Region


#Region "Cambia ruolo e cancella iscritto"
    Public Sub AnnullaModificaRuolo()
        Me.PNLmodificaRuolo.Visible = False
        Me.PNLfiltri.Visible = True
        Me.PNLiscritti.Visible = True
        Me.Bind_Griglia_Iscritti()
    End Sub
    Public Function ModificaRuolo() As Inserimento
        Dim iResponse As Inserimento = Inserimento.ErroreModifica
        If Me.DDLruoloForum.SelectedItem.Value <> "0" Then

            Dim oForum As New COL_Forums
            Dim TPRF_id, RLPC_id As Integer
            oForum.Id = Session("IdForum")
            RLPC_id = Me.HDrlpc.Value
            TPRF_id = Me.DDLruoloForum.SelectedItem.Value
            Try
                oForum.CambiaRuoloIscritto(TPRF_id, RLPC_id)
                If oForum.Errore = Errori_Db.None Then
                    iResponse = Inserimento.ModificaAvvenuta
                Else
                    iResponse = Inserimento.ErroreModifica
                End If
            Catch ex As Exception

            End Try
            Me.PNLmodificaRuolo.Visible = False
            Me.PNLfiltri.Visible = True
            Me.PNLiscritti.Visible = True
            Me.Bind_Griglia_Iscritti()
            Return iResponse
        Else
            iResponse = Inserimento.TipoRuoloMancante
        End If
        Return iResponse
    End Function

    'Public Sub IMBmodificaIscritto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBmodificaIscritto.Click
    '    'Dim pippo, pluto As Integer
    '    Dim otableCell As New System.Web.UI.WebControls.TableCell
    '    Dim oDatagridItem As System.Web.UI.WebControls.DataGridItem
    '    otableCell = sender.parent
    '    oDatagridItem = otableCell.Parent
    '    Try
    '        ' Bind_Ruoli_Forum()
    '        Me.HDrlpc.Value = CInt(Me.DGiscrittiForum.DataKeys.Item(oDatagridItem.ItemIndex))
    '        If Me.HDrlpc.Value = Session("RLPC_id") Then
    '            'Me.IMBmodificaIscritto.ImageUrl = "../images/m_d.gif"
    '        Else
    '            Dim str_test As String = DGiscrittiForum.Items(oDatagridItem.ItemIndex).Cells(3).Text
    '            str_test = Session("RuoloForum")
    '            If CInt(DGiscrittiForum.Items(oDatagridItem.ItemIndex).Cells(3).Text) > Session("RuoloForum") Then
    '                Bind_RuoliForumLimitato()
    '                Me.PNLiscritti.Visible = False
    '                Me.PNLnorecord.Visible = False
    '                Me.PNLfiltri.Visible = False
    '                Me.PNLmodificaRuolo.Visible = True
    '                Me.LBCognomeNome.Text = DGiscrittiForum.Items(oDatagridItem.ItemIndex).Cells(2).Text
    '                Me.DDLruoloForum.SelectedValue = DGiscrittiForum.Items(oDatagridItem.ItemIndex).Cells(3).Text
    '                If Me.DDLruoloForum.SelectedValue = -1 Then
    '                    RaiseEvent AggiornaMenu(True, False)
    '                Else
    '                    RaiseEvent AggiornaMenu(True, True)
    '                End If
    '            Else
    '                'Me.IMBmodificaIscritto.ImageUrl = "../images/x_d.gif"
    '            End If
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Public Sub IMBcancellaIscritto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBcancellaIscritto.Click
    '    Dim otableCell As New System.Web.UI.WebControls.TableCell
    '    Dim oDatagridItem As System.Web.UI.WebControls.DataGridItem
    '    otableCell = sender.parent
    '    oDatagridItem = otableCell.Parent
    '    If DGiscrittiForum.Items(oDatagridItem.ItemIndex).Cells(3).Text > Session("IdRuolo") Then

    '        Dim oForum As New COL_Forums
    '        Dim RLPC_id As New Integer
    '        RLPC_id = CInt(Me.DGiscrittiForum.DataKeys.Item(oDatagridItem.ItemIndex))
    '        oForum.Id = Session("IdForum")
    '        Try
    '            oForum.DisiscriviUtente(RLPC_id)
    '        Catch ex As Exception

    '        End Try
    '        Me.Bind_Griglia_Iscritti()
    '    End If
    'End Sub
#End Region

    
End Class