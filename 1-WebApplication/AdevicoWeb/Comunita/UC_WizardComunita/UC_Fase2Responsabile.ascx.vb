
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_Fase2Responsabile
    Inherits System.Web.UI.UserControl
    Protected oResourceResponsabile As ResourceManager
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
    Public Enum TipoRegistrazione
        AltroUtente = 0
        Amministratore = 1
        AmministratoreResponsabile = 2
    End Enum
   

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

    Public Event AggiornamentoVisualizzazione(ByVal Selezionato As Boolean)


    Public ReadOnly Property ResponsabileID() As String
        Get
            Try
                ResponsabileID = Me.HDNprsn_id.Value
            Catch ex As Exception
                ResponsabileID = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property GetNomeResponsabile() As String
        Get
            GetNomeResponsabile = Me.LBpersona.Text
        End Get
    End Property
    Public Property ComunitaID() As Integer
        Get
            If HDNcmnt_ID.Value = "" Then
                ComunitaID = 0
            Else
                ComunitaID = HDNcmnt_ID.Value
            End If
        End Get
        Set(ByVal Value As Integer)
            HDNcmnt_ID.Value = Value
        End Set
    End Property
    Public ReadOnly Property ComunitaPadreID() As Integer
        Get
            Try
                ComunitaPadreID = HDNidPadre.Value
            Catch ex As Exception

            End Try
        End Get
    End Property
    Public ReadOnly Property PersonaID() As Integer
        Get
            Try
                PersonaID = HDNprsn_id.Value
            Catch ex As Exception
                PersonaID = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property OrganizzazioneID() As Integer
        Get
            Try
                OrganizzazioneID = HDN_ORGN_ID.Value
            Catch ex As Exception

            End Try
        End Get
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
    Public ReadOnly Property isForInsert() As Boolean
        Get
            Try
                isForInsert = HDN_ForInsert.Value
            Catch ex As Exception
                isForInsert = True
            End Try
        End Get
    End Property


    Protected WithEvents HDN_TipoComunita As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNidPadre As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ORGN_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ForInsert As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBRsceltaUtenti As System.Web.UI.WebControls.TableRow
    Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents BNTannullaModifica As System.Web.UI.WebControls.Button

#Region "Filtro"
    Protected WithEvents TBRfiltroFacoltà As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBfacolta_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLfacolta As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBtipoRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoPersona_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox

#Region "Filtro Lettere"
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

    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoPersona As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
#End Region

#Region "Dati"
    Protected WithEvents TBRutenteSelezionato As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBpersona_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBpersona As System.Web.UI.WebControls.Label

    Protected WithEvents TBLresponsabili As System.Web.UI.WebControls.Table
    Protected WithEvents LBruoloResponsabile_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLruoloResponsabile As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBscelta As System.Web.UI.WebControls.Label
    Protected WithEvents RBLscelta As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TBRelencoUtenti As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents DGPersone As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnouser As System.Web.UI.WebControls.Label

    Protected WithEvents TBLutenti As System.Web.UI.WebControls.Table
#End Region

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceResponsabile) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If
    End Sub

    Public Sub SetupControllo(ByVal ComunitaID As Integer, ByVal ComunitaPadreID As Integer, ByVal OrganizzazioneID As Integer, ByVal TipoComunitaID As Integer, ByVal ForInsert As Boolean)
        Try

            Me.HDN_ORGN_ID.Value = OrganizzazioneID
            Me.HDNidPadre.Value = ComunitaPadreID
            Me.HDNprsn_id.Value = 0
            Me.HDN_TipoComunita.Value = TipoComunitaID
            Me.HDN_ForInsert.Value = ForInsert
            If Not ForInsert Then
                Me.HDNcmnt_ID.Value = ComunitaID
            Else
                Me.HDNcmnt_ID.Value = 0
            End If

            If IsNothing(oResourceResponsabile) Then
                Me.SetCulture(Session("LinguaCode"))
            End If
            SetupScript()
            Me.Bind_RuoloResponsabile()

            If Not ForInsert Then
                Me.Bind_SceltaResponsabile(ComunitaID)
            Else
                Me.Bind_SceltaResponsabile(ComunitaPadreID)
            End If

            Me.Bind_TipoPersona()
            Me.Bind_Organizzazioni()
            Me.SetupInternazionalizzazione()

            HDNhasSetup.Value = True
            Dim oPersona As New COL_Persona

            If ForInsert Then
                If Me.RBLscelta.SelectedValue = 0 Then
                    oPersona = Session("objPersona")
                    Me.LBpersona.Text = "<b>" & oPersona.Cognome & "</b> " & oPersona.Nome & "&nbsp;<a href='mailto:" & oPersona.Mail & "'>" & oPersona.Mail & "</a>"
                    Me.TBRsceltaUtenti.Visible = False
                    Me.TBRelencoUtenti.Visible = False
                    Me.TBRutenteSelezionato.Visible = True
                    Me.HDNprsn_id.Value = oPersona.Id
                    Try
                        Me.DDLruoloResponsabile.SelectedValue = Main.TipoRuoloStandard.AdminComunità
                        Me.DDLruoloResponsabile.Enabled = False
                    Catch ex As Exception

                    End Try
                Else
                    Me.DDLruoloResponsabile.Enabled = True
                    Me.TBRutenteSelezionato.Visible = False
                    Me.TBRsceltaUtenti.Visible = True
                    Me.TBRelencoUtenti.Visible = True
                    Me.TBRchiudiFiltro.Visible = True
                    Me.TBRapriFiltro.Visible = False
                    If Me.RBLscelta.SelectedValue = 1 Then
                        Me.DDLTipoPersona.Visible = True
                        Me.DDLTipoRuolo.Visible = False
                        Me.LBtipoRuolo_t.Visible = False
                        Me.LBtipoPersona_t.Visible = True
                        Me.TBRfiltroFacoltà.Visible = False
                    ElseIf Me.RBLscelta.SelectedValue = 2 Then
                        Me.DDLTipoPersona.Visible = True
                        Me.DDLTipoRuolo.Visible = False
                        Me.LBtipoRuolo_t.Visible = False
                        Me.LBtipoPersona_t.Visible = True
                        Me.TBRfiltroFacoltà.Visible = True
                    Else
                        Me.DDLTipoPersona.Visible = False
                        Me.DDLTipoRuolo.Visible = True
                        Me.LBtipoRuolo_t.Visible = True
                        Me.LBtipoPersona_t.Visible = False
                        Me.TBRfiltroFacoltà.Visible = False
                        Me.Bind_TipoRuolo()
                    End If
                End If
                If Me.RBLscelta.SelectedValue <> 0 Then
                    Me.Bind_Griglia(True)
                End If
            Else
                Dim oComunita As New COL_Comunita
                Dim RuoloID As Integer = Main.TipoRuoloStandard.AdminComunità
                oComunita.Id = ComunitaID
                Try
                    oPersona = oComunita.GetResponsabile()

                    Dim oRuoloComunita As New COL_RuoloPersonaComunita
                    oRuoloComunita.EstraiByLinguaDefault(ComunitaID, oPersona.Id)

                    If oRuoloComunita.Persona.Id = 0 Then
                        Me.LBpersona.Text = Me.oResourceResponsabile.getValue("nessunResponsabile")
                    Else
                        Me.LBpersona.Text = "<b>" & oPersona.Cognome & "</b> " & oPersona.Nome & "&nbsp;<a href='mailto:" & oPersona.Mail & "'>" & oPersona.Mail & "</a>"
                    End If
                    Me.TBRsceltaUtenti.Visible = False
                    Me.TBRelencoUtenti.Visible = False
                    Me.TBRutenteSelezionato.Visible = True
                    Me.HDNprsn_id.Value = oPersona.Id
                    RuoloID = oRuoloComunita.TipoRuolo.Id
                Catch ex As Exception
                    Me.LBpersona.Text = "//"
                    Me.TBRsceltaUtenti.Visible = False
                    Me.TBRelencoUtenti.Visible = False
                    Me.TBRutenteSelezionato.Visible = True
                    Me.HDNprsn_id.Value = 0
                End Try
                Try
                    Me.DDLruoloResponsabile.SelectedValue = RuoloID
                Catch ex As Exception

                End Try
                Me.DDLruoloResponsabile.Enabled = False

                Me.DDLTipoPersona.Visible = False
                Me.DDLTipoRuolo.Visible = True
                Me.LBtipoRuolo_t.Visible = True
                Me.LBtipoPersona_t.Visible = False
                Me.TBRfiltroFacoltà.Visible = False
                Me.Bind_TipoRuolo()
            End If

            Try
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Catch ex As Exception

            End Try
            Me.ViewState("intCurPage") = 0
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"

            If Me.isForInsert = False And Me.HDNprsn_id.Value = 0 Then
                RaiseEvent AggiornamentoVisualizzazione(False)
            Else
                RaiseEvent AggiornamentoVisualizzazione(True)
            End If
        Catch ex As Exception
            RaiseEvent AggiornamentoVisualizzazione(False)
        End Try
    End Sub
    Public Sub ResetControllo()
        Me.HDN_ORGN_ID.Value = 0
        Me.HDNidPadre.Value = 0
        Me.HDNprsn_id.Value = 0
        Me.HDN_TipoComunita.Value = -1
        Me.HDNcmnt_ID.Value = 0
        HDNhasSetup.Value = False
    End Sub
    Public Sub AggiornaDati()
        Try
            Dim oPersona As New COL_Persona
            If IsNothing(oResourceResponsabile) Then
                Me.SetCulture(Session("LinguaCode"))
            End If

            If Me.PersonaID > 0 Then
                Me.TBRelencoUtenti.Visible = False
                Me.TBRutenteSelezionato.Visible = True
                Me.TBRsceltaUtenti.Visible = False

                oPersona.Id = Me.PersonaID
                oPersona.Estrai(Session("LinguaID"))
                Me.LBpersona.Text = "<b>" & oPersona.Cognome & "</b> " & oPersona.Nome & "&nbsp;<a href='mailto:" & oPersona.Mail & "'>" & oPersona.Mail & "</a>"
            ElseIf Me.isForInsert = True Then
                Me.TBRelencoUtenti.Visible = True
                Me.TBRutenteSelezionato.Visible = False
                Me.TBRsceltaUtenti.Visible = True
                If Me.RBLscelta.SelectedValue <> 0 And Me.TBRsceltaUtenti.Visible = False Then
                    Me.Bind_Griglia(True)
                End If
            End If

            RaiseEvent AggiornamentoVisualizzazione(Me.PersonaID > 0)
        Catch ex As Exception
            RaiseEvent AggiornamentoVisualizzazione(False)
        End Try
    End Sub

#Region "Bind Dati"
    Private Sub SetupScript()
        'aggiunge ai link button le proprietà da visualizzare nella barra di stato
        Dim i As Integer

        If IsNothing(oResourceResponsabile) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        For i = Asc("a") To Asc("z") 'status dei link button delle lettere
            Dim oLinkButton As New LinkButton
            oLinkButton = FindControl("LKB" & Chr(i))
            Dim Carattere As String = Chr(i)

            If IsNothing(oLinkButton) = False Then
                Me.oResourceResponsabile.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
            End If
        Next
    End Sub
    Private Sub Bind_RuoloResponsabile()
        Me.DDLruoloResponsabile.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer

            If Me.isForInsert Then
                Dim oTipoComunita As New COL_Tipo_Comunita
                oTipoComunita.ID = Me.HDN_TipoComunita.Value
                oDataset = oTipoComunita.ElencaTipoRuoloAssociati(Session("LinguaID"), Main.FiltroRuoli.ForTipoComunita_NoGuest)
            Else
                Dim oComunita As New COL_Comunita
                oComunita.Id = Me.ComunitaID
                If oComunita.GetProfiloServizioID > 0 Then
                    oDataset = oComunita.RuoliAssociabiliByPersona(0, Main.FiltroRuoli.ForProfiloComunita_NoGuest)
                Else
                    oDataset = oComunita.RuoliAssociabiliByPersona(0, Main.FiltroRuoli.ForTipoComunita_NoGuest)
                End If
            End If

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
                    Me.DDLruoloResponsabile.Items.Add(oListItem)
                Next
                'Me.DDLtipoRuoloAggiungi.SelectedValue = oComunita.RuoloDefault
                Try
                    Me.DDLruoloResponsabile.SelectedValue = Main.TipoRuoloStandard.AdminComunità
                Catch ex As Exception

                End Try
            Else
                Me.DDLruoloResponsabile.Items.Add(New ListItem("< nessun ruolo >", -1))
            End If
        Catch ex As Exception
            Me.DDLruoloResponsabile.Items.Add(New ListItem("< nessun ruolo >", -1))
        End Try
        Me.oResourceResponsabile.setDropDownList(DDLruoloResponsabile, -1)
    End Sub
    Private Sub Bind_SceltaResponsabile(ByVal ComunitaID As Integer)
        Dim oComunita As New COL_Comunita

        Try
            oComunita.Id = ComunitaID
            oComunita.Estrai()

            Me.RBLscelta.Items.Clear()
            Me.RBLscelta.Items.Add(New ListItem(Session("objPersona").Nome & " " & Session("objPersona").Cognome, 0))

            'dall'intero sistema
            Me.RBLscelta.Items.Add(New ListItem(oResourceResponsabile.getValue("RBLscelta.1"), 1))
            'da una facoltà
            Me.RBLscelta.Items.Add(New ListItem(oResourceResponsabile.getValue("RBLscelta.2"), 2))

            If oComunita.TipoComunita.ID = Main.TipoComunitaStandard.Organizzazione And Me.isForInsert = False Then
                'dalla comunità corrente
                Me.RBLscelta.Items.Add(New ListItem(oResourceResponsabile.getValue("RBLscelta.3"), 3))
            ElseIf oComunita.TipoComunita.ID <> Main.TipoComunitaStandard.Organizzazione Then
                'Me.RBLscelta.Items.Add(New ListItem(oResourceResponsabile.getValue("RBLscelta.3"), 3))
                Me.RBLscelta.Items.Add(New ListItem(oComunita.Nome, 3))
            End If
        Catch ex As Exception

        End Try
        Try
            If Me.isForInsert Then
                Me.RBLscelta.SelectedIndex = 0
            Else
                Me.RBLscelta.SelectedValue = 3
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub Bind_TipoRuolo(Optional ByVal TipoRuoloId As Integer = -1)
        Dim oDataSet As New DataSet
        Dim TPCM_ID As Integer
        Try
            Dim oComunita As New COL_Comunita
            Try
                If Me.RBLscelta.SelectedValue = 3 Then
                    If Me.isForInsert Then
                        oComunita.Id = Me.HDNidPadre.Value
                    Else
                        oComunita.Id = Me.HDNcmnt_ID.Value
                    End If
                ElseIf Me.RBLscelta.SelectedValue = 2 Then
                    ' facoltà
                    oComunita.Id = Me.DDLfacolta.SelectedValue
                End If
            Catch ex As Exception

            End Try
            oDataSet = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)

            oComunita.Estrai()
            If oDataSet.Tables(0).Rows.Count > 0 Then
                TPCM_ID = oComunita.TipoComunita.ID
                DDLTipoRuolo.DataSource = oDataSet
                DDLTipoRuolo.DataTextField() = "TPRL_nome"
                DDLTipoRuolo.DataValueField() = "TPRL_id"
                DDLTipoRuolo.DataBind()

                If DDLTipoRuolo.Items.Count > 0 Then
                    DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", Main.TipoRuoloStandard.Tutti_NoGuest))
                End If

                If TPCM_ID > -1 And TipoRuoloId = -1 Then
                    Try
                        Select Case TPCM_ID
                          
                            Case Main.TipoComunitaStandard.GruppoDiLavoro
                                Try
                                    Me.DDLTipoRuolo.SelectedValue = 1
                                Catch ex As Exception
                                    Me.DDLTipoRuolo.SelectedIndex = 1
                                End Try

                           
                            Case Main.TipoComunitaStandard.Organizzazione
                                Try
                                    Me.DDLTipoRuolo.SelectedValue = Main.TipoRuoloStandard.AdminComunità
                                Catch ex As Exception
                                    Me.DDLTipoRuolo.SelectedIndex = 1
                                End Try
                           
                            Case Else
                                Me.DDLTipoRuolo.SelectedIndex = 1
                        End Select
                    Catch ex As Exception
                        Me.DDLTipoRuolo.SelectedIndex = 0
                    End Try
                Else
                    If TipoRuoloId > -1 Then
                        Try
                            Me.DDLTipoRuolo.SelectedValue = TipoRuoloId
                        Catch ex As Exception

                        End Try
                    End If
                End If

            End If
        Catch ex As Exception
            DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", Main.TipoRuoloStandard.Tutti_NoGuest))
        End Try
        Me.oResourceResponsabile.setDropDownList(DDLTipoRuolo, Main.TipoRuoloStandard.Tutti_NoGuest)
    End Sub
    Private Sub Bind_TipoPersona()
        Dim oDataset As DataSet
        Dim oTipoPersona As New COL_TipoPersona
        Dim oListItem As New ListItem

        Try
            oDataset = oTipoPersona.Elenca(Session("LinguaID"), Main.FiltroElencoTipiPersona.WithUserAssociated_NoGuest)
            DDLTipoPersona.Items.Clear()
            If oDataset.Tables(0).Rows.Count > 0 Then
                DDLTipoPersona.DataSource = oDataset
                DDLTipoPersona.DataTextField() = "TPPR_descrizione"
                DDLTipoPersona.DataValueField() = "TPPR_id"
                DDLTipoPersona.DataBind()
                DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
            Else
                DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        Me.oResourceResponsabile.setDropDownList(DDLTipoPersona, -1)
    End Sub
    Private Sub Bind_Organizzazioni()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLfacolta.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLfacolta.DataValueField = "CMNT_ID"
                Me.DDLfacolta.DataTextField = "ORGN_ragioneSociale"
                Me.DDLfacolta.DataSource = oDataset
                Me.DDLfacolta.DataBind()

                If Me.DDLfacolta.Items.Count > 1 Then
                    Me.DDLfacolta.Enabled = True
                    Me.DDLfacolta.SelectedIndex = 0
                Else
                    Me.DDLfacolta.Enabled = False
                End If
            Else
                Me.DDLfacolta.Items.Add(New ListItem("< nessuna >", -1))
                Me.DDLfacolta.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLfacolta.Items.Clear()
            Me.DDLfacolta.Items.Add(New ListItem("< nessuna >", -1))
            Me.DDLfacolta.Enabled = False
        End Try
        oResourceResponsabile.setDropDownList(Me.DDLfacolta, -1)
    End Sub
#End Region

#Region "Bind Filtri"
    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
        Me.Bind_Griglia()
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
        Me.Bind_Griglia()
    End Sub


    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0 'azzero indice paginazione
        viewstate("RipristinaCheck") = "si"
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0
        viewstate("RipristinaCheck") = "si"
        If Me.TXBValore.Text <> "" Then
            Bind_Griglia(True)
        Else
            Bind_Griglia()
        End If

    End Sub

    Private Sub DDLTipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoPersona.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0
        viewstate("RipristinaCheck") = "si"
        If Me.TXBValore.Text <> "" Then
            Bind_Griglia(True)
        Else
            Bind_Griglia()
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
        Me.DGPersone.CurrentPageIndex = 0
        Me.Bind_Griglia(True)
    End Sub

    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControl("LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub

    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        'eseguire il filtraggio!!!
        If Me.TXBValore.Text <> "" Then
            Bind_Griglia(True)
        End If

    End Sub

    Private Sub DDLfacolta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfacolta.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0 'azzero indice paginazione
        Try
            Me.Bind_TipoRuolo(Me.DDLTipoRuolo.SelectedValue)
        Catch ex As Exception
            Me.Bind_TipoRuolo()
        End Try
        Me.Bind_Griglia(True)
    End Sub
    Private Function Bind_Griglia(Optional ByVal Filtraggio As Boolean = False)
        Dim CMNT_id_passato As Integer
        Dim dsTable As New DataSet
        Dim oComunita As New COL_Comunita
        Dim valore As String
        valore = Me.TXBValore.Text

        'definisco il filtraggio per lettera !
        Dim oFiltroAnagrafica As Main.FiltroAnagrafica
        Dim oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti
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


        Try
            If valore <> "" Then
                If Filtraggio = True Then
                    Select Case Me.DDLTipoRicerca.SelectedItem.Value
                        Case Main.FiltroRicercaAnagrafica.nome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nome
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                        Case Main.FiltroRicercaAnagrafica.cognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.cognome

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.nomeCognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nomeCognome
                        Case Main.FiltroRicercaAnagrafica.matricola
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.matricola

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            Try
                                If IsDate(valore) Then
                                    valore = Main.DateToString(valore, False)
                                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                                End If
                            Catch ex As Exception

                            End Try
                        Case Main.FiltroRicercaAnagrafica.login
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.login

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Else
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                    End Select
                Else
                    Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                    Me.LKBtutti.CssClass = "lettera_Selezionata"
                End If
            End If
        Catch ex As Exception
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End Try


        Try
            'se è un'organizzazione va bene così..
            DGPersone.Columns(6).Visible = False
            DGPersone.Columns(8).Visible = False

            If Me.RBLscelta.SelectedValue = 1 Then
                Dim oOrganizzazione As New COL_Organizzazione
                dsTable = oOrganizzazione.ElencaNonIscrittiByIstituzione(Session("LinguaID"), Session("ISTT_ID"), Me.DDLTipoPersona.SelectedItem.Value, valore, oFiltroAnagrafica, oFiltroRicerca)
                DGPersone.Columns(8).Visible = True
            Else
                oComunita.Id = 0
                If Me.DDLfacolta.Visible = True Then
                    CMNT_id_passato = Me.DDLfacolta.SelectedValue
                Else
                    CMNT_id_passato = HDNidPadre.Value
                End If
                dsTable = oComunita.ElencaNonIscritti_MaIscrittiACmntPassata(Session("LinguaID"), Me.DDLTipoRuolo.SelectedItem.Value, CMNT_id_passato, valore, oFiltroAnagrafica, Main.FiltroUtenti.NoPassantiNoCreatori, oFiltroRicerca)
                If Me.DDLTipoRuolo.SelectedValue = -1 Then
                    DGPersone.Columns(6).Visible = True
                End If
            End If
            dsTable.Tables(0).Columns.Add(New DataColumn("oCheckAbilitato"))
            dsTable.Tables(0).Columns.Add(New DataColumn("oPRSN_datanascita"))
            Dim i, totale As Integer

            If dsTable.Tables(0).Rows.Count = 0 Then
                Me.DGPersone.Visible = False
                Me.LBnouser.Visible = True
            Else

                'ordinamento delle colonne e databind della griglia

                Dim oDataview As DataView
                oDataview = dsTable.Tables(0).DefaultView
                If viewstate("SortExspression2") = "" Then
                    viewstate("SortExspression2") = "PRSN_cognome"
                    viewstate("SortDirection") = "asc"
                End If
                oDataview.Sort = viewstate("SortExspression2") & " " & viewstate("SortDirection")
                If dsTable.Tables(0).Rows.Count > Me.DDLNumeroRecord.Items(0).Value Then
                    Me.LBnumeroRecord_t.Visible = True
                    Me.DDLNumeroRecord.Visible = True
                    Me.DGPersone.PagerStyle.Position = PagerPosition.TopAndBottom
                Else
                    Me.LBnumeroRecord_t.Visible = False
                    Me.DDLNumeroRecord.Visible = False
                    Me.DGPersone.PagerStyle.Position = PagerPosition.Top
                End If
                Me.DGPersone.DataSource = oDataview
                Me.DGPersone.PageSize = Me.DDLNumeroRecord.SelectedValue
                Me.DGPersone.DataBind()
                Me.DGPersone.Visible = True
                Me.LBnouser.Visible = False
            End If
        Catch ex As Exception
            Me.DGPersone.Visible = False
            Me.LBnouser.Visible = True
        End Try
    End Function
#End Region

#Region "Gestione Griglia"
    Sub DGPersone_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGPersone.PageIndexChanged
        DGPersone.CurrentPageIndex = e.NewPageIndex
        viewstate("RipristinaCheck") = "si"
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DGPersone_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGPersone.ItemCreated
        Dim i As Integer
        If IsNothing(oResourceResponsabile) Then
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
                    If Me.DGPersone.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResourceResponsabile.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResourceResponsabile.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGPersone.HeaderStyle.CssClass
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
                                oResourceResponsabile.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGPersone.HeaderStyle.CssClass
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
            ' Aggiungo riga con descrizione:

            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                Dim num As Integer = 4
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                If Me.DDLTipoRuolo.Visible = True And Me.DDLTipoRuolo.SelectedValue < 0 Then
                    num += 1
                ElseIf Me.DDLTipoPersona.Visible And Me.DDLTipoPersona.SelectedValue < 0 Then
                    num += 1
                End If


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
                    oResourceResponsabile.setPageDatagrid(Me.DGPersone, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim isSelezionabile As Boolean = False
            Try
                If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.DataItem("PRSN_ID") = Me.PersonaID Then
                    e.Item.CssClass = "ROW_Selezionate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                    isSelezionabile = True
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                    isSelezionabile = True
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                End If
            End Try



            Dim oImagebutton As ImageButton
            Dim Cell As New TableCell
            Dim TPPR_id As Integer
            Dim PRSN_ID As Integer

            Try
                PRSN_ID = e.Item.DataItem("PRSN_id")
                TPPR_id = e.Item.DataItem("PRSN_TPPR_id")
                Dim i_link2 As String
                i_link2 = "./InfoIscritto.aspx?TPPR_ID=" & TPPR_id & "&PRSN_ID=" & PRSN_ID
                Cell = CType(e.Item.Cells(1), TableCell)

                oImagebutton = Cell.FindControl("IMBinfo")
                Me.oResourceResponsabile.setImageButton_Datagrid(Me.DGPersone, oImagebutton, "IMBinfo", True, True)
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

                'oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try

            Try
                'Gestione link MAIL !!!
                Dim oHYPMail As HyperLink
                oHYPMail = e.Item.Cells(6).FindControl("HYPMail")
                If Not IsNothing(oHYPMail) Then
                    oHYPMail.CssClass = cssLink
                End If

            Catch ex As Exception

            End Try

            Try
                Dim oLinkButton As LinkButton
                oLinkButton = e.Item.Cells(11).FindControl("LNBseleziona")

                oLinkButton.Enabled = isSelezionabile
                If isSelezionabile Then
                    If e.Item.DataItem("PRSN_ID") = Me.PersonaID Then 'Me.HDNprsn_id.Value Then
                        oLinkButton.Enabled = False
                        Me.oResourceResponsabile.setLinkButtonToValue(oLinkButton, "Select", True, True)
                    Else
                        Me.oResourceResponsabile.setLinkButtonToValue(oLinkButton, "NotSelect", True, True)
                    End If
                Else
                    oLinkButton.Visible = False
                    Me.oResourceResponsabile.setLinkButtonToValue(oLinkButton, "NotSelect", True, True)
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub DGPersone_SortCommand(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGPersone.SortCommand
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
        Me.Bind_Griglia(True)
    End Sub

    Private Sub DGPersone_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGPersone.ItemCommand
        If e.CommandName = "seleziona" Then
            HDNprsn_id.Value = 0
            Try
                Dim oPersona As New COL_Persona
                HDNprsn_id.Value = CInt(DGPersone.DataKeys.Item(e.Item.ItemIndex))
                oPersona.Id = HDNprsn_id.Value
                oPersona.Estrai(Session("LinguaID"))
                If oPersona.Errore = Errori_Db.None Then
                    Me.LBpersona.Text = "<b>" & oPersona.Cognome & "</b> " & oPersona.Nome & "&nbsp;<a href='mailto:" & oPersona.Mail & "'>" & oPersona.Mail & "</a>"
                    Me.TBRsceltaUtenti.Visible = False
                    Me.TBRelencoUtenti.Visible = False
                    Me.TBRutenteSelezionato.Visible = True
                Else
                    Me.TBRutenteSelezionato.Visible = False
                    Me.TBRsceltaUtenti.Visible = True
                    Me.TBRelencoUtenti.Visible = True
                End If
            Catch ex As Exception

            End Try
            If HDNprsn_id.Value = 0 Then
                Me.Bind_Griglia(True)
                RaiseEvent AggiornamentoVisualizzazione(False)
            Else
                RaiseEvent AggiornamentoVisualizzazione(True)
            End If
        Else
            Me.Bind_Griglia(True)
        End If

    End Sub
    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(oResourceResponsabile) Then
            Me.SetCulture(Session("LinguaCode"))
        End If


        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Selezionate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceResponsabile.getValue("Selezionati")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceResponsabile.getValue("NONattivati")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResourceResponsabile.getValue("NONabilitati")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceResponsabile = New ResourceManager
        oResourceResponsabile.UserLanguages = Code
        oResourceResponsabile.ResourcesName = "pg_UC_Fase2Responsabile"
        oResourceResponsabile.Folder_Level1 = "Comunita"
        oResourceResponsabile.Folder_Level2 = "UC_WizardComunita"
        oResourceResponsabile.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceResponsabile
            .setLabel(Me.LBruoloResponsabile_t)
            .setButton(Me.BTNCerca, True)
            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)

            .setLabel(Me.LBfacolta_t)
            .setLabel(Me.LBnumeroRecord_t)
            .setLabel(Me.LBtipoPersona_t)
            .setLabel(Me.LBtipoRicerca_t)
            .setLabel(Me.LBtipoRuolo_t)
            .setLabel(Me.LBvalore_t)
            .setLabel(Me.LBpersona_t)
            .setDropDownList(DDLTipoRicerca, -2)
            .setDropDownList(DDLTipoRicerca, -3)
            .setDropDownList(DDLTipoRicerca, -4)
            .setDropDownList(DDLTipoRicerca, -7)

            .setHeaderDatagrid(Me.DGPersone, 2, "cognome", True)
            .setHeaderDatagrid(Me.DGPersone, 3, "nome", True)
            .setHeaderDatagrid(Me.DGPersone, 4, "PRSN_Anagrafica", True)
            .setHeaderDatagrid(Me.DGPersone, 5, "PRSN_mail", True)
            .setHeaderDatagrid(Me.DGPersone, 6, "TPRL_nome", True)
            .setHeaderDatagrid(Me.DGPersone, 8, "TPPR_descrizione", True)
            .setButton(BTNmodifica, True)
            .setButton(Me.BNTannullaModifica, True)

        End With
    End Sub
#End Region

    Public Function RegistraResponsabile(ByVal ComunitaID As Integer, ByVal Percorso As String, ByVal isIscritto As Boolean) As WizardComunita_Message
        Dim iResponse As WizardComunita_Message = ModuloEnum.WizardComunita_Message.NesunaOperazione

        Try
            If Me.isForInsert Then
                If Me.PersonaID = Session("objPersona").id Then
                    If ComunitaID = 0 Then
                        Return WizardComunita_Message.DatiMancanti
                    ElseIf Me.AggiornaIscrizione(ComunitaID, Me.PersonaID) Then
                        Return WizardComunita_Message.ResponsabileAssociato
                    Else
                        Return WizardComunita_Message.ResponsabileNonAssociato
                    End If
                Else
                    Return AssociaResponsabile(ComunitaID, Percorso, Me.PersonaID)
                End If
            Else
                Dim oRuoloComunita As New COL_RuoloPersonaComunita
                oRuoloComunita.EstraiByLinguaDefault(ComunitaID, Me.PersonaID)

                If oRuoloComunita.Errore = Errori_Db.None Then
                    If Me.AggiornaIscrizione(ComunitaID, Me.PersonaID) Then
                        Return WizardComunita_Message.ResponsabileAssociato
                    Else
                        Return WizardComunita_Message.ResponsabileNonAssociato
                    End If
                Else
                    Return AssociaResponsabile(ComunitaID, Percorso, Me.PersonaID)
                End If
            End If
        Catch ex As Exception
            Return WizardComunita_Message.ResponsabileNonAssociato
        End Try
        Return iResponse
    End Function

    Private Function AggiornaIscrizione(ByVal ComunitaID As Integer, ByVal PRSN_ID As Integer) As Boolean
        Dim oComunita As New COL_Comunita
        Try
            Dim oRuoloComunita As New COL_RuoloPersonaComunita
            oRuoloComunita.EstraiByLinguaDefault(ComunitaID, PRSN_ID)

            If oRuoloComunita.Errore = Errori_Db.None Then
                Dim isModificato As Boolean = False
                Dim RuoloResponsabileID As Integer = -1

                Try
                    RuoloResponsabileID = Me.DDLruoloResponsabile.SelectedValue
                Catch ex As Exception

                End Try
                oComunita.Id = ComunitaID
                oComunita.Estrai()

                If oRuoloComunita.TipoRuolo.Id = Main.TipoRuoloStandard.Creatore Then
                    Try
                        Me.DDLruoloResponsabile.SelectedValue = Main.TipoRuoloStandard.AdminComunità
                        oRuoloComunita.TipoRuolo.Descrizione = Me.DDLruoloResponsabile.SelectedItem.Text
                    Catch ex As Exception

                    End Try
                    oRuoloComunita.TipoRuolo.Id = Main.TipoRuoloStandard.AdminComunità
                    isModificato = True
                Else
                    If RuoloResponsabileID <> -1 Then
                        oRuoloComunita.TipoRuolo.Id = Me.DDLruoloResponsabile.SelectedValue
                        oRuoloComunita.TipoRuolo.Descrizione = Me.DDLruoloResponsabile.SelectedItem.Text
                        isModificato = True
                    End If
                End If
                oRuoloComunita.isResponsabile = True
                oRuoloComunita.Modifica()

                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function AggiornaXML(ByVal ComunitaID As Integer, ByVal PRSN_ID As Integer, ByVal isResponsabile As Boolean, ByVal CMNT_PathPassante As String)
        Try

            Dim oRuoloComunita As New COL_RuoloPersonaComunita
            oRuoloComunita.EstraiByLinguaDefault(ComunitaID, PRSN_ID)

            If oRuoloComunita.Errore = Errori_Db.None Then
                If (isResponsabile = False And oRuoloComunita.isResponsabile = True) Or (isResponsabile And oRuoloComunita.isResponsabile = False) Then
                    oRuoloComunita.isResponsabile = isResponsabile
                    oRuoloComunita.Modifica()
                End If
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Function AssociaResponsabile(ByVal ComunitaID As Integer, ByVal Percorso As String, ByVal IDresponsabile As Integer) As WizardComunita_Message
        Dim iResponse As WizardComunita_Message = WizardComunita_Message.ResponsabileNonAssociato
        Try
            If Me.PersonaID > 0 Then
                Dim oComunita As New COL_Comunita 'comunità corrente

                Dim oComunitaCreata As New COL_Comunita 'comunità creata
                oComunitaCreata.Id = ComunitaID
                oComunitaCreata.Estrai()

              
                iResponse = AggiungiUtenteResponsabile(ComunitaID, Percorso, IDresponsabile)

            Else
                iResponse = WizardComunita_Message.DatiMancanti
            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Private Function AggiungiUtenteResponsabile(ByVal ComunitaID As Integer, ByVal Percorso As String, ByVal IDresponsabile As Integer) As WizardComunita_Message
        Dim iResponse As WizardComunita_Message = WizardComunita_Message.ResponsabileNonAssociato
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim isIscritto As Boolean = False

        Try
            Dim RuoloID As Integer
            Dim oDataset As New DataSet
            Dim totaleC, totalePadri, j As Integer
            Dim ElencoIDcomunita() As String
            Dim PercorsoPadri As String

            PercorsoPadri = Percorso

            RuoloID = Me.DDLruoloResponsabile.SelectedValue
            oComunita.Id = ComunitaID

            ElencoIDcomunita = Percorso.Split(".")
            isIscritto = False
            totaleC = UBound(ElencoIDcomunita) - 1
            RuoloID = VerificaRuolo(ComunitaID, IDresponsabile, RuoloID)
            If totaleC = 1 Then
                oComunita.IscriviUtente(IDresponsabile, RuoloID, , , True)
                Me.AggiornaXML(ComunitaID, IDresponsabile, True, Percorso)
                isIscritto = True
            Else
                For j = totaleC To 0 Step -1
                    If IsNumeric(ElencoIDcomunita(j)) Then
                        oDataset = oPersona.VerificaIscrizioneAPadri(ElencoIDcomunita(j), IDresponsabile)
                        totalePadri = oDataset.Tables(0).Rows.Count
                        If totalePadri > 0 Then
                            isIscritto = True
                            oComunita.IscriviUtente(IDresponsabile, RuoloID, , , True)
                            Me.AggiornaXML(ComunitaID, IDresponsabile, True, Percorso)
                            Exit For
                        Else
                            If ElencoIDcomunita(j - 1) = "" Then
                                Exit For
                            Else
                                oComunita.IscriviPassanteAcomunita(IDresponsabile, ElencoIDcomunita(j - 1))
                                PercorsoPadri = Left(PercorsoPadri, InStr(PercorsoPadri, "." & ElencoIDcomunita(j - 1) & "."))
                                PercorsoPadri = PercorsoPadri & ElencoIDcomunita(j - 1) & "."
                                Me.AggiornaXML(ElencoIDcomunita(j - 1), IDresponsabile, False, PercorsoPadri)
                            End If
                        End If
                    End If
                Next
            End If
            If isIscritto = False Then
                oComunita.IscriviUtente(IDresponsabile, RuoloID, , , True)
                Me.AggiornaXML(ComunitaID, IDresponsabile, True, Percorso)
                isIscritto = True
            End If
        Catch ex As Exception

        End Try
        If isIscritto Then
            iResponse = WizardComunita_Message.ResponsabileAssociato
        End If
        Return iResponse
    End Function


    Private Function VerificaRuolo(ByVal ComunitaID As Integer, ByVal ResponsabileID As Integer, ByVal RuoloID As Integer) As Integer
        Dim oRuoloComunita As New COL_RuoloPersonaComunita

        Try
            oRuoloComunita.EstraiByLinguaDefault(ComunitaID, ResponsabileID)

            If oRuoloComunita.Errore = Errori_Db.None Then
                If oRuoloComunita.TipoRuolo.Id = -2 Then
                    RuoloID = CType(Main.TipoRuoloStandard.AdminComunità, Main.TipoRuoloStandard)
                End If
            End If
        Catch ex As Exception
            Return RuoloID
        End Try
        Return RuoloID
    End Function


    Private Sub RBLscelta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLscelta.SelectedIndexChanged
        If Me.RBLscelta.SelectedValue = 0 Then
            Dim oPersona As New COL_Persona
            Me.TBRelencoUtenti.Visible = False
            Me.TBRutenteSelezionato.Visible = True
            Me.TBRsceltaUtenti.Visible = False
            Try
                Me.DDLruoloResponsabile.SelectedValue = Main.TipoRuoloStandard.AdminComunità
                Me.DDLruoloResponsabile.Enabled = False
            Catch ex As Exception

            End Try
            oPersona = Session("objPersona")
            Me.LBpersona.Text = "<b>" & oPersona.Cognome & "</b> " & oPersona.Nome & "&nbsp;<a href='mailto:" & oPersona.Mail & "'>" & oPersona.Mail & "</a>"
            RaiseEvent AggiornamentoVisualizzazione(True)
        Else
            Me.DDLruoloResponsabile.Enabled = True
            Me.TBRutenteSelezionato.Visible = False
            Me.TBRelencoUtenti.Visible = True
            Me.TBRchiudiFiltro.Visible = True
            Me.TBRapriFiltro.Visible = False
            Try
                If Me.HDNprsn_id.Value > 0 Then
                    RaiseEvent AggiornamentoVisualizzazione(True)
                Else
                    RaiseEvent AggiornamentoVisualizzazione(False)
                End If
            Catch ex As Exception
                RaiseEvent AggiornamentoVisualizzazione(False)
            End Try

            If Me.RBLscelta.SelectedValue = 1 Then
                Me.DDLTipoPersona.Visible = True
                Me.DDLTipoRuolo.Visible = False
                Me.LBtipoRuolo_t.Visible = False
                Me.LBtipoPersona_t.Visible = True
                Me.TBRfiltroFacoltà.Visible = False
            ElseIf Me.RBLscelta.SelectedValue = 2 Then
                Me.DDLTipoPersona.Visible = False
                Me.DDLTipoRuolo.Visible = True
                Me.LBtipoRuolo_t.Visible = True
                Me.LBtipoPersona_t.Visible = False
                Me.TBRfiltroFacoltà.Visible = True
                Try
                    Me.Bind_TipoRuolo(Me.DDLTipoRuolo.SelectedValue)
                Catch ex As Exception
                    Me.Bind_TipoRuolo()
                End Try
            Else
                Me.DDLTipoPersona.Visible = False
                Me.DDLTipoRuolo.Visible = True
                Me.LBtipoRuolo_t.Visible = True
                Me.LBtipoPersona_t.Visible = False
                Me.TBRfiltroFacoltà.Visible = False

                Try
                    Me.Bind_TipoRuolo(Me.DDLTipoRuolo.SelectedValue)
                Catch ex As Exception
                    Me.Bind_TipoRuolo()
                End Try
            End If
            Try
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Catch ex As Exception

            End Try
            Me.ViewState("intCurPage") = 0
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
            Me.TXBValore.Text = ""
            Me.Bind_Griglia(True)
        End If
    End Sub

    Private Sub BTNmodifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNmodifica.Click
        Me.TBRutenteSelezionato.Visible = False
        Me.TBRsceltaUtenti.Visible = True

        If Me.HDNprsn_id.Value = 0 And Me.isForInsert = False Then
            Me.DDLTipoPersona.Visible = False
            Me.DDLTipoRuolo.Visible = True
            Me.LBtipoRuolo_t.Visible = True
            Me.LBtipoPersona_t.Visible = False
            Me.TBRfiltroFacoltà.Visible = False
            Me.Bind_TipoRuolo()
            Me.TBRelencoUtenti.Visible = True
            Me.Bind_Griglia(True)
        Else
            If Me.RBLscelta.SelectedValue = 0 Then
                Me.TBRelencoUtenti.Visible = False
            Else
                Me.TBRelencoUtenti.Visible = True
                Me.Bind_Griglia(True)
            End If
        End If
    End Sub

    Private Sub BNTannullaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BNTannullaModifica.Click
        Me.TBRutenteSelezionato.Visible = True
        Me.TBRsceltaUtenti.Visible = False
        Me.TBRelencoUtenti.Visible = False

        Try
            If Me.HDNprsn_id.Value = 0 Then
                Me.DDLruoloResponsabile.Enabled = False
            Else
                Me.DDLruoloResponsabile.Enabled = True
            End If
        Catch ex As Exception

        End Try

    End Sub
End Class