Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class ListaIscritti
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

#Region "TEMP"
    Private _ProfileService As lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService
    Private ReadOnly Property ProfileService() As lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService
        Get
            If IsNothing(_ProfileService) Then
                _ProfileService = New lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(PageUtility.CurrentContext)
            End If
            Return _ProfileService
        End Get
    End Property
    Private _PageUtility As OLDpageUtility

    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
#End Region

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBstampa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DGiscritti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LKBgestioneIscritti As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label

#Region "Pannello Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents LBnoIscritti As System.Web.UI.WebControls.Label
#Region "Filtro"
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox

#Region "Lettere"
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
#End Region

    Protected WithEvents LBtipoRuolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore As System.Web.UI.WebControls.Label
    Protected WithEvents LBiscrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLiscrizione As System.Web.UI.WebControls.DropDownList

    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox

    Protected WithEvents HDN_filtroRuolo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroIscrizione As System.Web.UI.HtmlControls.HtmlInputHidden
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

        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oServizioListaIscritti As New Services_Listaiscritti

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Try
            oPersona = Session("objPersona")
            If Not Page.IsPostBack Then
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(Services_Listaiscritti.Codex)
                oServizioListaIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Else
                If Me.ViewState("PermessiAssociati") = "" Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(Services_Listaiscritti.Codex)
                End If
                oServizioListaIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            End If
        Catch ex As Exception
            oServizioListaIscritti.PermessiAssociati = "00000000000000000000000000000000"
        End Try

        If Not Page.IsPostBack Then
            Try
                Session("azione") = "load"

                Me.SetupInternazionalizzazione()
                If oServizioListaIscritti.List And Not (oServizioListaIscritti.Admin Or oServizioListaIscritti.Management) Then
                    Me.LBiscrizione_t.Visible = False
                    Me.DDLiscrizione.Visible = False
                Else
                    Me.LBiscrizione_t.Visible = True
                    Me.DDLiscrizione.Visible = True
                End If
                Me.Bind_dati()
                Me.LKBgestioneIscritti.Visible = (oServizioListaIscritti.Admin Or oServizioListaIscritti.Management Or oServizioListaIscritti.InfoEstese)
                Me.LNBstampa.Visible = (oServizioListaIscritti.Admin Or oServizioListaIscritti.Management) '(oServizioListaIscritti.Print Or oServizioListaIscritti.Management Or oServizioListaIscritti.Admin Or oServizioListaIscritti.InfoEstese)
            Catch ex As Exception
                Me.PNLpermessi.Visible = True
                Me.oResource.setLabel(LBNopermessi)
                Me.PNLcontenuto.Visible = False
                Me.LNBstampa.Visible = False
                Me.LKBgestioneIscritti.Visible = False
            End Try
        End If

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID

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
                    Try
                        CMNT_ID = Session("idComunita")
                    Catch ex2 As Exception
                        CMNT_ID = 0
                    End Try
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

    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim oPersona As New COL_Persona
        Dim oRuoloComunita As New COL_RuoloPersonaComunita
        Dim CMNT_id As Integer

        Dim PermessiAssociati As String

        Try
            oPersona = Session("objPersona")
            PermessiAssociati = Permessi(Codex, Me.Page)

            If (PermessiAssociati = "") Then
                PermessiAssociati = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            PermessiAssociati = "00000000000000000000000000000000"
        End Try

        If Request.QueryString("CMNT_id") Is Nothing Then
            Try
                CMNT_id = Session("IdComunita")
                PermessiAssociati = Permessi(Codex, Me.Page)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try
        Else
            Dim oComunita As New COL_Comunita
            CMNT_id = Request.QueryString("CMNT_id")
            oComunita.Id = CMNT_id
            Try
                PermessiAssociati = oComunita.GetPermessiForServizioByPersona(oPersona.Id, Request.QueryString("CMNT_id"), Codex)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try
        End If

        Try
            oRuoloComunita.EstraiByLinguaDefault(CMNT_id, oPersona.Id)
            Me.ViewState("PRSN_TPRL_Gerarchia") = oRuoloComunita.TipoRuolo.Gerarchia

        Catch ex As Exception
            Me.ViewState("PRSN_TPRL_Gerarchia") = "99999"
        End Try
        Return PermessiAssociati
    End Function

#Region "Bind_Dati"
    Private Sub Bind_dati()
        Me.SetupInternazionalizzazione()
        Me.ViewState("intCurPage") = 0
        Me.ViewState("intAnagrafica") = CType(Main.FiltroComunita.tutti, Main.FiltroComunita)
        Me.LKBtutti.CssClass = "lettera_Selezionata"

        If Me.Request.QueryString("lastSubscribed") <> "" Then
            viewstate("SortExspression") = "RLPC_IscrittoIl"
            viewstate("SortDirection") = "desc"
        End If
        Me.Bind_TipoRuoloFiltro()

        If Me.Request.QueryString("lastSubscribed") <> "" Then
            Me.Bind_Visualizzazione(Main.TipoAttivazione.NuoviIscritti)
        Else
            Me.Bind_Visualizzazione()
        End If


        Me.DDLiscrizione.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLTipoRuolo.AutoPostBack = Me.CBXautoUpdate.Checked

        Try
            Me.HDN_filtroRuolo.Value = Me.DDLTipoRuolo.SelectedValue
            Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
            Me.HDN_filtroValore.Value = Me.TXBValore.Text
            Me.HDN_filtroIscrizione.Value = Me.DDLiscrizione.SelectedValue
        Catch ex As Exception

        End Try

        Me.Bind_Griglia(True, True)
    End Sub
    Private Sub Bind_TipoRuoloFiltro()

        Me.DDLTipoRuolo.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita

            If Session("AdminForChange") = False Then
                oComunita.Id = Session("IdComunita")
            Else
                oComunita.Id = Session("idComunita_forAdmin")
            End If


            '  oComunita.Id = Session("IdComunita")
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
                    Me.DDLTipoRuolo.Items.Add(oListItem)
                Next
                DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
            Else
                Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
            End If
        Catch ex As Exception
            Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
        End Try
        Me.oResource.setDropDownList(Me.DDLTipoRuolo, -1)
    End Sub
    Private Sub Bind_Visualizzazione(Optional ByVal NuovoIndice As Integer = -15)
        Dim oServizioListaIscritti As New Services_Listaiscritti

        Try

            If Not Page.IsPostBack Then
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(Services_Listaiscritti.Codex)
                oServizioListaIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Else
                If Me.ViewState("PermessiAssociati") = "" Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(Services_Listaiscritti.Codex)
                End If
                oServizioListaIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            End If
        Catch ex As Exception
            oServizioListaIscritti.PermessiAssociati = "00000000000000000000000000000000"
        End Try
        Me.Bind_Visualizzazione(oServizioListaIscritti, NuovoIndice)
    End Sub
    Private Sub Bind_Visualizzazione(ByVal oServizioListaIscritti As UCServices.Services_Listaiscritti, Optional ByVal NuovoIndice As Integer = -15)
        Dim HasNewUser, HasWaitingUsers, HasBlockedUsers, TabTutti As Boolean

        Try
            Dim IndiceCorrente As Integer = Main.TipoAttivazione.Attivati
            Dim oComunita As New COL_Comunita
            Dim ComunitaID As Integer

            Try
                If Session("AdminForChange") = False Then
                    ComunitaID = Session("IdComunita")
                Else
                    ComunitaID = Session("idComunita_forAdmin")
                End If
                oComunita.Id = ComunitaID
                oComunita.Estrai()
            Catch ex As Exception

            End Try


            HasNewUser = oComunita.HasNewUsers(Session("objPersona").id, True)
            HasWaitingUsers = oComunita.HasWaitingUsers()
            HasBlockedUsers = oComunita.HasBlockedUsers()
            TabTutti = HasNewUser Or HasWaitingUsers Or HasBlockedUsers

            Try
                IndiceCorrente = Me.DDLiscrizione.SelectedIndex
            Catch ex As Exception

            End Try
            Me.DDLiscrizione.Items.Clear()

            Me.LBiscrizione_t.Visible = True
            Me.DDLiscrizione.Visible = True
            If oServizioListaIscritti.List And Not (oServizioListaIscritti.Management Or oServizioListaIscritti.Admin) Then
                If Not HasNewUser Then
                    Me.LBiscrizione_t.Visible = False
                    Me.DDLiscrizione.Visible = False
                    TabTutti = False
                End If
                HasWaitingUsers = False
                HasBlockedUsers = False
            End If

            If (TabTutti) Then
                Me.DDLiscrizione.Items.Add(New ListItem("Tutti", CType(Main.TipoAttivazione.Tutti, Main.TipoAttivazione)))
            End If
            If (HasNewUser) Then
                Me.DDLiscrizione.Items.Add(New ListItem("Nuovi iscritti", CType(Main.TipoAttivazione.NuoviIscritti, Main.TipoAttivazione)))
            End If
            Me.DDLiscrizione.Items.Add(New ListItem("Abilitati", CType(Main.TipoAttivazione.Attivati, Main.TipoAttivazione)))
            If (HasWaitingUsers) Then
                Me.DDLiscrizione.Items.Add(New ListItem("In attesa di conferma", CType(Main.TipoAttivazione.InAttesa, Main.TipoAttivazione)))
            End If
            If (HasBlockedUsers) Then
                Me.DDLiscrizione.Items.Add(New ListItem("Bloccati", CType(Main.TipoAttivazione.Bloccati, Main.TipoAttivazione)))
            End If
            Try
                If NuovoIndice <> -15 Then
                    Me.DDLiscrizione.SelectedValue = NuovoIndice
                Else
                    Me.DDLiscrizione.SelectedValue = IndiceCorrente
                End If
            Catch ex As Exception
                Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Attivati
            End Try

            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.Bloccati))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.Attivati))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.InAttesa))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.NuoviIscritti))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.Tutti))
        Catch ex As Exception

        End Try
    End Sub

    Private Function FiltraggioDati(ByVal Applicafiltri As Boolean, Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim Valore As String
            Dim oComunita As New COL_Comunita
            Dim TipoRuoloID, IscrizioneID, TipoRicercaID As Integer
            If Session("AdminForChange") = False Then
                oComunita.Id = Session("IdComunita")
            Else
                oComunita.Id = Session("idComunita_forAdmin")
            End If

            If Applicafiltri Then
                Try
                    Me.HDN_filtroIscrizione.Value = Me.DDLiscrizione.SelectedValue
                Catch ex As Exception

                End Try
                Try
                    Me.HDN_filtroRuolo.Value = Me.DDLTipoRuolo.SelectedValue
                Catch ex As Exception

                End Try
                Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
            End If

            Try
                IscrizioneID = Me.HDN_filtroIscrizione.Value
            Catch ex As Exception
                IscrizioneID = -1
            End Try
            Try
                TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
            Catch ex As Exception
                TipoRicercaID = -1
            End Try
            Try
                TipoRuoloID = Me.DDLTipoRuolo.SelectedValue
            Catch ex As Exception
                TipoRuoloID = -1
            End Try
            Try
                Valore = Me.HDN_filtroValore.Value
                If Valore <> "" Then
                    Valore = Trim(Valore)
                End If
            Catch ex As Exception
                Valore = ""
            End Try

            Dim oFiltroCampoOrdine As COL_Comunita.FiltroCampoOrdine
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento
            Dim oFiltroLettera As Main.FiltroAnagrafica
            Dim oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti

            Try
                If IsNothing(Me.ViewState("intAnagrafica")) Then
                    oFiltroLettera = Main.FiltroAnagrafica.tutti
                Else
                    oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroLettera = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                Me.ViewState("intAnagrafica") = -1
            End Try


            Try
                If Valore <> "" Then
                    Select Case TipoRicercaID
                        Case Main.FiltroRicercaAnagrafica.nome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nome
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                        Case Main.FiltroRicercaAnagrafica.cognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.cognome

                            oFiltroLettera = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.nomeCognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nomeCognome
                        Case Main.FiltroRicercaAnagrafica.matricola
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.matricola

                            oFiltroLettera = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            Try
                                If IsDate(Valore) Then
                                    Valore = Main.DateToString(Valore, False)
                                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                                End If
                            Catch ex As Exception

                            End Try
                        Case Main.FiltroRicercaAnagrafica.login
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.login

                            oFiltroLettera = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Else
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                    End Select
                Else
                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroLettera = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try
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
            ElseIf LCase(viewstate("SortExspression")) = "prsn_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.nome
            ElseIf LCase(viewstate("SortExspression")) = "prsn_cognome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            Else
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            End If

            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            If ricalcola Then
                Me.ViewState("intCurPage") = 0
                Me.DGiscritti.CurrentPageIndex = 0
            End If

            Dim oFiltro As Main.FiltroAbilitazione

            Me.DGiscritti.Columns(16).Visible = False
            Me.DGiscritti.Columns(10).Visible = True
            Select Case IscrizioneID
                Case Main.TipoAttivazione.Tutti
                    oFiltro = Main.FiltroAbilitazione.Tutti

                Case Main.TipoAttivazione.NuoviIscritti
                    oFiltro = Main.FiltroAbilitazione.TuttiUltimiIscritti
                    Me.DGiscritti.Columns(16).Visible = True
                    Me.DGiscritti.Columns(10).Visible = False
                Case Main.TipoAttivazione.InAttesa
                    oFiltro = Main.FiltroAbilitazione.NonAttivatoNonAbilitato
                    Me.DGiscritti.Columns(16).Visible = True
                    Me.DGiscritti.Columns(10).Visible = False
                Case Main.TipoAttivazione.Bloccati
                    oFiltro = Main.FiltroAbilitazione.NonAbilitatoAttivato
                Case Else
                    oFiltro = Main.FiltroAbilitazione.AttivatoAbilitato
            End Select
            oDataset = oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), 0, oFiltro, Main.FiltroUtenti.NoPassantiNoCreatori, TipoRuoloID, Me.DGiscritti.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroLettera, oFiltroOrdinamento, oFiltroCampoOrdine, , oFiltroRicerca)

            Dim i, totale As Integer
            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheck"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oIscrittoIl"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oUltimoCollegamento"))
                oDataset.Tables(0).Columns.Add("oCheckDisabled")
                Me.DGiscritti.VirtualItemCount = oDataset.Tables(0).Rows(0).Item("Totale")

                Dim PRSN_TPRL_Gerarchia, TPRL_Gerarchia As Integer
                Try
                    PRSN_TPRL_Gerarchia = Me.ViewState("PRSN_TPRL_Gerarchia")
                Catch ex As Exception
                    PRSN_TPRL_Gerarchia = "9999999"
                End Try

                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                        If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                            oRow.Item("oIscrittoIl") = "&nbsp;--"
                        Else
                            oRow.Item("oIscrittoIl") = "&nbsp;" & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                        End If
                    Else
                        oRow.Item("oIscrittoIl") = "&nbsp;--"
                    End If

                    If IsDBNull(oRow.Item("RLPC_ultimoCollegamento")) = False Then
                        If Equals(oRow.Item("RLPC_ultimoCollegamento"), New Date) Then
                            oRow.Item("oUltimoCollegamento") = "&nbsp;--"
                        Else
                            oRow.Item("oUltimoCollegamento") = "&nbsp;" & FormatDateTime(oRow.Item("RLPC_ultimoCollegamento"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_ultimoCollegamento"), DateFormat.ShortTime)
                        End If
                    Else
                        oRow.Item("oUltimoCollegamento") = "&nbsp;--"
                    End If

                    TPRL_Gerarchia = oRow.Item("TPRL_Gerarchia")
                    If TPRL_Gerarchia < PRSN_TPRL_Gerarchia Then
                        oRow.Item("oCheckDisabled") = "disabled"
                    Else
                        oRow.Item("oCheckDisabled") = ""
                    End If
                Next
            End If
         
            Return oDataset
        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub Bind_Griglia(ByVal Applicafiltri As Boolean, Optional ByVal ricalcola As Boolean = False)
        Dim oDataset As New DataSet
        Try
            Dim totale As Integer

            oDataset = FiltraggioDati(Applicafiltri, ricalcola)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGiscritti.Visible = False
                LBnoIscritti.Visible = True
                Me.DGiscritti.VirtualItemCount = 0
                Me.oResource.setLabel_To_Value(LBnoIscritti, "LBnoIscritti1")
            Else
                If totale > 0 Then
                    Mod_Visualizzazione(totale - 1)
                    Me.DGiscritti.Visible = True

                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_Cognome"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    Me.DGiscritti.Columns(4).Visible = False
                    Me.DGiscritti.DataSource = oDataview
                    Me.DGiscritti.DataBind()
                    LBnoIscritti.Visible = False

                    'Ora nascondo le colonne in base ai permessi.......
                    Dim oServizioIscritti As New UCServices.Services_GestioneIscritti
                    Try
                        oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
                    Catch ex As Exception
                        oServizioIscritti.PermessiAssociati = Me.GetPermessiForPage(Services_GestioneIscritti.Codex)
                    End Try
                    Me.DGiscritti.Columns(1).Visible = (oServizioIscritti.Admin Or oServizioIscritti.Change Or oServizioIscritti.Management Or oServizioIscritti.InfoEstese)
                Else
                    Me.DGiscritti.Visible = False
                    LBnoIscritti.Visible = True
                    Me.oResource.setLabel_To_Value(LBnoIscritti, "LBnoIscritti2")
                End If
            End If
        Catch ex As Exception
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
            Me.oResource.setLabel_To_Value(LBnoIscritti, "LBnoIscritti1")
        End Try
    End Sub


#End Region

#Region "Gestione Filtri"
    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.Bind_Griglia(True, True)
    End Sub
    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        Me.Bind_Griglia(Me.CBXautoUpdate.Checked, True)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
    Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
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
        Me.Bind_Griglia(Me.CBXautoUpdate.Checked, True)
    End Sub

    Private Sub DDLiscrizione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLiscrizione.SelectedIndexChanged
        Me.Bind_Griglia(Me.CBXautoUpdate.Checked, True)
    End Sub
    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        Me.Bind_Griglia(Me.CBXautoUpdate.Checked, True)
    End Sub
    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.DDLTipoRuolo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLiscrizione.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLTipoRuolo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.Bind_Griglia(Me.CBXautoUpdate.Checked, True)
    End Sub
#End Region

#Region "Gestione Griglia"

    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGiscritti.SortCommand
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
        Me.Bind_Griglia(Main.FiltroAbilitazione.AttivatoAbilitato, True)
    End Sub
    Sub DGiscritti_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGiscritti.PageIndexChanged
        'paginazione della datagrid iscritti
        Me.ViewState("intCurPage") = e.NewPageIndex
        viewstate("Paginazione") = "si"
        Me.DGiscritti.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia(Main.FiltroAbilitazione.AttivatoAbilitato, False)
    End Sub
    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGiscritti.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
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
                    If Me.DGiscritti.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    Me.oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    Me.oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)
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
                                Me.oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)
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

            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "ROW_PagerLink_Small"
                End If
                Try
                    If TypeOf (oWebControl) Is Label Then
                        Dim oLabel As Label = oWebControl
                        oLabel.Text = oLabel.Text
                        oLabel.CssClass = "ROW_PagerSpan_Small"
                    ElseIf TypeOf (oWebControl) Is LinkButton Then
                        Dim oLinkbutton As LinkButton= oWebControl
                        oLinkbutton.CssClass = "ROW_PagerLink_Small"
                        Me.oResource.setPageDatagrid(Me.DGiscritti, oLinkbutton)
                    End If
                
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "ROW_PagerLink_Small"
                    Me.oResource.setPageDatagrid(Me.DGiscritti, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
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
                Else
                End If
            End Try

            'responsabile [R]
            Try
                If CBool(e.Item.DataItem("RLPC_Responsabile")) = True Then
                    e.Item.DataItem("PRSN_Anagrafica") = e.Item.DataItem("PRSN_Anagrafica") & "[R]"

                End If
            Catch ex As Exception

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
                Me.oResource.setImageButton_Datagrid(Me.DGiscritti, oImagebutton, "IMBinfo", True, True)
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
                'Select Case TPPR_id
                '    Case Main.TipoPersonaStandard.Studente
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                '    Case Main.TipoPersonaStandard.Docente
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','480','no','yes');return false;")
                '    Case Main.TipoPersonaStandard.Tutor
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','480','no','yes');return false;")
                '    Case Main.TipoPersonaStandard.Esterno
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                '    Case Main.TipoPersonaStandard.Amministrativo
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                '    Case Main.TipoPersonaStandard.SysAdmin
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                '    Case Main.TipoPersonaStandard.Copisteria
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480',450','no','yes');return false;")
                '    Case Main.TipoPersonaStandard.Dottorando
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480',450','no','yes');return false;")
                '    Case Else
                '        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480',450','no','yes');return false;")
                'End Select
                ' oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try

            Try
                Dim mostraMail As Boolean = False
                Try
                    If e.Item.DataItem("PRSN_mostraMail") = True Then
                        mostraMail = True
                    End If
                Catch ex As Exception

                End Try

                Try
                    If Not IsDBNull(e.Item.DataItem("RLPC_PRSN_mostraMail")) Then
                        mostraMail = CBool(e.Item.DataItem("RLPC_PRSN_mostraMail"))
                    End If
                Catch ex As Exception

                End Try
                Dim oHyperLink As HyperLink
                Dim oLabel As Label

                oHyperLink = e.Item.Cells(6).FindControl("HYPMail")
                oLabel = e.Item.Cells(6).FindControl("LBnoMail")

                If IsNothing(oHyperLink) = False And IsNothing(oLabel) = False Then
                    oHyperLink.Visible = mostraMail
                    oLabel.Visible = Not mostraMail
                    If mostraMail And Me.DGiscritti.Columns(6).Visible = False Then
                        Me.DGiscritti.Columns(6).Visible = True
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.DGiscritti.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        viewstate("Paginazione") = "si"
        Me.Bind_Griglia(Main.FiltroAbilitazione.AttivatoAbilitato)
    End Sub
    Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
        If oRecord > Me.DGiscritti.PageSize Or oRecord > Me.DDLNumeroRecord.Items(0).Value Or Me.DGiscritti.VirtualItemCount > Me.DGiscritti.PageSize Then
            Me.DGiscritti.AllowPaging = True
            Me.DGiscritti.PageSize = Me.DDLNumeroRecord.SelectedItem.Value
        Else
            Me.DGiscritti.AllowPaging = False
        End If
        If oRecord < 0 Then
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
            'LBnoIscritti.Text = "Nessun utente in questa categoria"
            Me.oResource.setLabel_To_Value(LBnoIscritti, "LBnoIscritti1")
        End If
    End Sub
#End Region


    Private Sub LKBgestioneIscritti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBgestioneIscritti.Click
        Response.Redirect("./gestioneIscritti.aspx")
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ListaIscritti"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBNopermessi)
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBtipoRuolo)
            .setLabel(Me.LBnumeroRecord)
            .setLabel(Me.LBtipoRicerca)
            .setLabel(Me.LBvalore)
            .setDropDownList(DDLTipoRicerca, -2)
            .setDropDownList(DDLTipoRicerca, -3)
            .setDropDownList(DDLTipoRicerca, -4)

       
            .setDropDownList(DDLTipoRicerca, -7)
            .setButton(BTNCerca)
            .setHeaderDatagrid(Me.DGiscritti, 2, "cognome", True)
            .setHeaderDatagrid(Me.DGiscritti, 3, "nome", True)
            .setHeaderDatagrid(Me.DGiscritti, 4, "anagrafica", True)
            .setHeaderDatagrid(Me.DGiscritti, 5, "login", True)
            .setHeaderDatagrid(Me.DGiscritti, 6, "mail", True)
            .setHeaderDatagrid(Me.DGiscritti, 8, "ruolo", True)
            .setHeaderDatagrid(Me.DGiscritti, 10, "ultimoCollegamento", True)
            .setHeaderDatagrid(Me.DGiscritti, 16, "iscrittoIl", True)
            .setLinkButton(Me.LKBgestioneIscritti, True, True)
            .setLinkButton(Me.LNBstampa, True, True)

            .setLinkButton(Me.LKBtutti, True, True)
            .setLinkButton(LKBaltro, True, True)
            .setLinkButton(LNBapriFiltro, True, True)
            .setLinkButton(LNBchiudiFiltro, True, True)
            .setCheckBox(Me.CBXautoUpdate)
            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    Me.oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next
            .setLabel(Me.LBiscrizione_t)
            Me.LNBstampa.Attributes.Add("onClick", "Stampa();window.status='';return false;")
        End With
    End Sub
#End Region

    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
        Me.Bind_Griglia(Me.CBXautoUpdate.Checked)
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
        Me.Bind_Griglia(Me.CBXautoUpdate.Checked)
    End Sub



    Public ReadOnly Property BodyId As String
        Get
            Return Me.Master.BodyIdCode
        End Get
    End Property
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

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



'<%-- 
'       < %= Me.BodyId() % >.onkeydown = SubmitRicerca(event);

'   	function SubmitRicerca(event) {
'   	    if (document.all) {
'   	        if (event.keyCode == 13) {
'   	            event.returnValue = false;
'   	            event.cancel = true;
'   	            try {
'   	                document.forms[0].BTNCerca.click();
'   	            }
'   	            catch (ex) {
'   	                return false;
'   	            }
'   	        }
'   	    }
'   	    else if (document.getElementById) {
'   	        if (event.which == 13) {
'   	            event.returnValue = false;
'   	            event.cancel = true;
'   	            try {
'   	                document.forms[0].BTNCerca.click();
'   	            }
'   	            catch (ex) {
'   	                return false;
'   	            }
'   	        }
'   	    }
'   	    else if (document.layers) {
'   	        if (event.which == 13) {
'   	            event.returnValue = false;
'   	            event.cancel = true;
'   	            try {
'   	                document.forms[0].BTNCerca.click();
'   	            }
'   	            catch (ex) {
'   	                return false;
'   	            }
'   	        }
'   	    }
'   	    else
'   	        return true;
'   	}		
'       --%>