Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2ServiziBase.ContattiMail

Public Class UC_RubricaMailGenerica
    Inherits System.Web.UI.UserControl
    Private oResource As ResourceManager
    Private n_BackColor As String


    'Private Enum TabSingoli
    '    Singoli = 1
    '    singoliSelezionato = 2
    '    singoliDisattivato = 3
    'End Enum
    'Private Enum TabGruppi
    '    gruppi = 1
    '    gruppiSelezionato = 2
    '    gruppiDisattivato = 3
    'End Enum

#Region "Private Property"

    Protected WithEvents HDazione As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDazione_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDazione_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDazione_CCN As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents HDN_Path As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_CMNT_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_isLimbo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_setA As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_setCCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_setCC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppi_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiNome_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiTutti_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLcomunita_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLabilitazioneA As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppi_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiNome_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiTutti_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLcomunita_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLabilitazioneCC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppi_CCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiNome_CCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiTutti_CCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLcomunita_CCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLabilitazioneCCN As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Public Property"
    Public Property BackColor() As String
        Get
            BackColor = Me.TBLprincipale.BackColor.ToString
        End Get
        Set(ByVal Value As String)
            Try
                Me.TBLprincipale.BackColor = System.Drawing.Color.FromName(Value)
            Catch ex As Exception
                Me.TBLprincipale.BackColor = System.Drawing.Color.White
            End Try
        End Set
    End Property
    Public Property ComunitaID() As Integer
        Get
            Try
                ComunitaID = Me.HDN_CMNT_ID.Value
            Catch ex As Exception
                ComunitaID = -1
            End Try
        End Get
        Set(ByVal Value As Integer)
            Me.HDN_CMNT_ID.Value = Value
        End Set
    End Property
    Public Property ComunitaPercorso() As String
        Get
            Try
                ComunitaPercorso = Me.HDN_Path.Value
            Catch ex As Exception
                ComunitaPercorso = ""
            End Try
        End Get
        Set(ByVal Value As String)
            Me.HDN_Path.Value = Value
        End Set
    End Property
    Public Property ShowSelezioneComunita() As Boolean
        Get
            ShowSelezioneComunita = Me.TBRselezioneComunita.Visible
        End Get
        Set(ByVal Value As Boolean)
            Me.TBRselezioneComunita.Visible = Value
        End Set
    End Property
    Public Property ShowSelezioneDestinatari() As Boolean
        Get
            ShowSelezioneDestinatari = Me.TBRselezioneDestinatari.Visible
        End Get
        Set(ByVal Value As Boolean)
            Me.TBRselezioneDestinatari.Visible = Value
        End Set
    End Property
    Public Property isForLimbo() As Boolean
        Get
            isForLimbo = (Me.HDN_isLimbo.Value = "0")
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                Me.HDN_isLimbo.Value = 1
            Else
                Me.HDN_isLimbo.Value = 0
            End If
        End Set
    End Property
    Public Property setA_Address() As Boolean
        Get
            setA_Address = (Me.HDN_setA.Value = "1")
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                Me.HDN_setA.Value = 1
                Me.HDN_setCC.Value = 0
                Me.HDN_setCCN.Value = 0
                Me.HDazione.Value = Me.HDazione_A.Value
            Else
                Me.HDN_setA.Value = 0
            End If
        End Set
    End Property
    Public Property setCCN_Address() As Boolean
        Get

            setCCN_Address = (Me.HDN_setCCN.Value = "1")
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                Me.HDN_setCC.Value = 0
                Me.HDN_setA.Value = 0
                Me.HDN_setCCN.Value = 1
                Me.HDazione.Value = Me.HDazione_CCN.Value
            Else
                Me.HDN_setCCN.Value = 0
            End If
        End Set
    End Property
    Public Property setCC_Address() As Boolean
        Get
            setCC_Address = (Me.HDN_setCC.Value = "1")
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                Me.HDN_setCC.Value = 1
                Me.HDN_setA.Value = 0
                Me.HDN_setCCN.Value = 0
                Me.HDazione.Value = Me.HDazione_CC.Value
            Else
                Me.HDN_setCC.Value = 0
            End If
        End Set
    End Property

    Public ReadOnly Property GetDestinatariMail_A() As String
        Get
            GetDestinatariMail_A = Me.getDestinatariMail(TipoDestinatarioMail.A)
        End Get
    End Property
    Public ReadOnly Property GetDestinatariMail_CC() As String
        Get
            GetDestinatariMail_CC = Me.getDestinatariMail(TipoDestinatarioMail.CC)
        End Get
    End Property
    Public ReadOnly Property GetDestinatariMail_CCN() As String
        Get
            GetDestinatariMail_CCN = Me.getDestinatariMail(TipoDestinatarioMail.CCN)
        End Get
    End Property

    Public ReadOnly Property Contatti_TO() As COL_CollectionContatti
        Get
            Contatti_TO = Me.getIndirizziMail(TipoDestinatarioMail.A)
        End Get
    End Property
    Public ReadOnly Property Contatti_CC() As COL_CollectionContatti
        Get
            Contatti_CC = Me.getIndirizziMail(TipoDestinatarioMail.CC)
        End Get
    End Property
    Public ReadOnly Property Contatti_CCN() As COL_CollectionContatti
        Get
            Contatti_CCN = Me.getIndirizziMail(TipoDestinatarioMail.CCN)
        End Get
    End Property

    Public ReadOnly Property HasSelezionati() As Boolean
        Get
            If setA_Address Then
                If Me.HDazione_A.Value = "" And Me.HDNgruppi_A.Value = "" Then
                    HasSelezionati = False
                Else
                    HasSelezionati = True
                End If
            ElseIf setCC_Address Then
                If Me.HDazione_CC.Value = "" And Me.HDNgruppi_CC.Value = "" Then
                    HasSelezionati = False
                Else
                    HasSelezionati = True
                End If
            ElseIf setCCN_Address Then
                If Me.HDazione_CCN.Value = "" And Me.HDNgruppi_CCN.Value = "" Then
                    HasSelezionati = False
                Else
                    HasSelezionati = True
                End If
			End If

			If HasSelezionati = False Then
				If Me.setA_Address Then
					HasSelezionati = GenericValidator.ValBool(Me.HDNgruppiTutti_A.Value, False)
				ElseIf Me.setCC_Address Then
					HasSelezionati = GenericValidator.ValBool(Me.HDNgruppiTutti_CC.Value, False)
				ElseIf Me.setCCN_Address Then
					HasSelezionati = GenericValidator.ValBool(Me.HDNgruppiTutti_CCN.Value, False)
				End If
			End If
        End Get
    End Property


#End Region


    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip


#Region "Gestione Filtro"
    Protected WithEvents PNLfiltri As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBvalore As System.Web.UI.WebControls.TextBox
    Protected WithEvents DGiscritti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DTLiscritti As System.Web.UI.WebControls.DataList
    Protected WithEvents LBnoIscritti As System.Web.UI.WebControls.Label

    Protected WithEvents BTNcerca As System.Web.UI.WebControls.Button
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
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

#Region "Pannelli"
    Protected WithEvents TBRselezioneComunita As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRselezioneDestinatari As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLprincipale As System.Web.UI.WebControls.Table
    Protected WithEvents LBselezionaComunita As System.Web.UI.WebControls.Label
    Protected WithEvents RBLcomunita As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBselezionaDest As System.Web.UI.WebControls.Label
    Protected WithEvents RBLabilitazione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBseleziona As System.Web.UI.WebControls.Label
    Protected WithEvents CBXtutti As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBLgruppi As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents PNLgruppi As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLsingoli As System.Web.UI.WebControls.Panel

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
        If Page.IsPostBack = False Then
            Try
                SetupInternazionalizzazione()
            Catch exUserLanguages As Exception
            End Try
        End If
    End Sub

    'Private Sub StartupData()
    '    Try
    '        Me.Bind_Dati()
    '    Catch ex As Exception

    '    End Try

    'End Sub
#Region "Localizzazione"
    Public Function SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_RubricaMailGenerica"
        oResource.Folder_Level1 = "Generici"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Function

    Public Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(LBselezionaComunita)
            .setLabel(LBselezionaDest)
            .setLabel(LBseleziona)
            .setRadioButtonList(Me.RBLcomunita, "0")
            .setRadioButtonList(Me.RBLcomunita, "-1")

            .setRadioButtonList(Me.RBLabilitazione, "1")
            .setRadioButtonList(Me.RBLabilitazione, "5")
            .setRadioButtonList(Me.RBLabilitazione, "4")
            .setRadioButtonList(Me.RBLabilitazione, "0")
            .setCheckBox(CBXtutti)

            TBSmenu.Tabs(0).Text = .getValue("RTABGruppi.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("RTABGruppi.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("RTABSingoli.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("RTABSingoli.ToolTip")

            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControl("LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next

            .setLabel(LBtipoRuolo_t)
            .setLabel(LBnumeroRecord_t)
            .setLabel(LBtipoRicerca_t)
            .setLabel(LBvalore_t)
            .setDropDownList(DDLTipoRicerca, -2)
            .setDropDownList(DDLTipoRicerca, -3)
            .setDropDownList(DDLTipoRicerca, -4)
            .setDropDownList(DDLTipoRicerca, -5)
            .setButton(Me.BTNcerca)

            .setLinkButton(LKBaltro, True, False)
            .setLinkButton(LKBtutti, True, False)
            .setLabel(LBnoIscritti)

        End With
    End Sub
#End Region


#Region "Gestione Selezione"
    Private Sub RBLcomunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLcomunita.SelectedIndexChanged
        If Me.setA_Address Then
            Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
        ElseIf Me.setCC_Address Then
            Me.HDN_RBLcomunita_CC.Value = Me.RBLcomunita.SelectedValue
        ElseIf Me.setCCN_Address Then
            Me.HDN_RBLcomunita_CCN.Value = Me.RBLcomunita.SelectedValue
        Else
            Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
        End If

        Dim selezionati As String
        Dim oComunita As New COL_Comunita
        selezionati = Me.GetGruppiSelezionati()

        Dim totale As Integer
        oComunita.Id = Me.ComunitaID
        If Me.RBLcomunita.SelectedValue = 0 Then
            totale = oComunita.GetTotaleIscritti(Me.RBLabilitazione.SelectedValue, Main.FiltroUtenti.NoPassantiNoCreatori, Session("objPersona").Id, False, "")
        Else
            totale = oComunita.GetTotaleIscritti(Me.RBLabilitazione.SelectedValue, Main.FiltroUtenti.NoPassantiNoCreatori, Session("objPersona").Id, True, Me.ComunitaPercorso)
        End If
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        Me.CBXtutti.Text = oResource.getValue("tutti") & " (" & totale & ")"
        Me.BindGruppi()

        If Me.CBXtutti.Checked = False Then
            Me.SetSelezionati(selezionati)
        End If
    End Sub
    Private Sub RBLabilitazioneSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLabilitazione.SelectedIndexChanged
        Dim oComunita As New COL_Comunita

        If Me.setA_Address Then
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
        ElseIf Me.setCC_Address Then
            Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
        ElseIf Me.setCCN_Address Then
            Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
        Else
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
        End If

        Dim selezionati As String
        selezionati = Me.GetGruppiSelezionati()

        Dim totale As Integer
        oComunita.Id = Me.ComunitaID
        If Me.RBLcomunita.SelectedValue = 0 Then
            totale = oComunita.GetTotaleIscritti(Me.RBLabilitazione.SelectedValue, Main.FiltroUtenti.NoPassantiNoCreatori, Session("objPersona").Id, False, "")
        Else
            totale = oComunita.GetTotaleIscritti(Me.RBLabilitazione.SelectedValue, Main.FiltroUtenti.NoPassantiNoCreatori, Session("objPersona").Id, True, Me.ComunitaPercorso)
        End If
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        Me.CBXtutti.Text = oResource.getValue("tutti") & " (" & totale & ")"
        Me.BindGruppi()

        If Me.CBXtutti.Checked = False Then
            Me.SetSelezionati(selezionati)
        End If
    End Sub

    Private Function GetGruppiSelezionati() As String
        Dim i, totale As Integer
        Dim iResponse As String = ""

        ' Recupero i gruppi selezionati
        If Me.setA_Address Then
            iResponse = Me.HDNgruppi_A.Value
        ElseIf Me.setCC_Address Then
            iResponse = Me.HDNgruppi_CC.Value
        ElseIf Me.setCCN_Address Then
            iResponse = Me.HDNgruppi_CCN.Value
        End If

        If iResponse <> "" Then
            iResponse = Right(iResponse, iResponse.Length - 1)
        End If
        Return iResponse
    End Function
    Private Sub SetSelezionati(ByVal selezionati As String)
        Dim i, totale, TPRL_ID As Integer
        Dim elencoID() As String

        If Me.setA_Address Then
            Me.HDNgruppi_A.Value = ""
        ElseIf Me.setCC_Address Then
            Me.HDNgruppi_CC.Value = ""
        ElseIf Me.setCCN_Address Then
            Me.HDNgruppi_CCN.Value = ""
        End If

        If selezionati <> "" Then
            elencoID = selezionati.Split(",")
            totale = elencoID.Length - 1
            For i = 0 To totale
                Dim oListItem As ListItem
                If IsNumeric(elencoID(i)) Then
                    TPRL_ID = elencoID(i)
                    oListItem = Me.CBLgruppi.Items.FindByValue(TPRL_ID)
                    If Not (IsNothing(oListItem)) Then
                        oListItem.Selected = True

                        If Me.setA_Address Then
                            If Me.HDNgruppi_A.Value = "" Then
                                Me.HDNgruppi_A.Value = "," & oListItem.Value & ","
                                Me.HDNgruppiNome_A.Value = "§" & oListItem.Text & "§"
                            Else
                                Me.HDNgruppi_A.Value = Me.HDNgruppi_A.Value & oListItem.Value & ","
                                Me.HDNgruppiNome_A.Value = Me.HDNgruppiNome_A.Value & oListItem.Text & "§"
                            End If
                        ElseIf Me.setCC_Address Then
                            If Me.HDNgruppi_CC.Value = "" Then
                                Me.HDNgruppi_CC.Value = "," & oListItem.Value & ","
                                Me.HDNgruppiNome_CC.Value = "§" & oListItem.Text & "§"
                            Else
                                Me.HDNgruppi_CC.Value = Me.HDNgruppi_CC.Value & oListItem.Value & ","
                                Me.HDNgruppiNome_CC.Value = Me.HDNgruppiNome_CC.Value & Me.CBLgruppi.Items.Item(i).Text & "§"
                            End If
                        ElseIf Me.setCCN_Address Then
                            If Me.HDNgruppi_CCN.Value = "" Then
                                Me.HDNgruppi_CCN.Value = "," & oListItem.Value & ","
                                Me.HDNgruppiNome_CCN.Value = "§" & oListItem.Text & "§"
                            Else
                                Me.HDNgruppi_CCN.Value = Me.HDNgruppi_CCN.Value & oListItem.Value & ","
                                Me.HDNgruppiNome_CCN.Value = Me.HDNgruppiNome_CCN.Value & oListItem.Text & "§"
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

#End Region


#Region "Bind_Dati"
    Public Sub Bind_Dati()
        Dim Totale As Integer
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita

        Try
            oPersona = Session("objPersona")
            oComunita.Id = Me.ComunitaID

            If IsNothing(oResource) Then
                SetCulture(Session("LinguaCode"))
            End If
            If Page.IsPostBack = False Then
                Try
                    SetupInternazionalizzazione()
                Catch exUserLanguages As Exception
                End Try
            End If

            If Me.ShowSelezioneComunita Then
                If Me.RBLcomunita.SelectedIndex = -1 Then
                    Me.RBLcomunita.SelectedIndex = 0
                End If
            Else
                Me.RBLcomunita.SelectedIndex = 0
            End If

            If Me.setA_Address Then
                Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
            ElseIf Me.setCC_Address Then
                Me.HDN_RBLcomunita_CC.Value = Me.RBLcomunita.SelectedValue
            ElseIf Me.setCCN_Address Then
                Me.HDN_RBLcomunita_CCN.Value = Me.RBLcomunita.SelectedValue
            Else
                Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
            End If

            Me.HDazione.Value = ""
            If Me.setA_Address Then
                Me.HDazione_A.Value = Me.HDazione.Value
            ElseIf Me.setCC_Address Then
                Me.HDazione_CC.Value = Me.HDazione.Value
            ElseIf Me.setCCN_Address Then
                Me.HDazione_CCN.Value = Me.HDazione.Value
            Else
                Me.HDazione_A.Value = Me.HDazione.Value
            End If

            Dim oAbilitazione As New Main.FiltroAbilitazione
            If Me.ShowSelezioneDestinatari Then
                Try
                    oAbilitazione = CType(Me.RBLabilitazione.SelectedValue, Main.FiltroAbilitazione)
                Catch ex As Exception
                    oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                    Me.RBLabilitazione.SelectedIndex = 0
                    If Me.setA_Address Then
                        Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                    ElseIf Me.setCC_Address Then
                        Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
                    ElseIf Me.setCCN_Address Then
                        Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
                    Else
                        Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                    End If
                End Try
            Else
                oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                Me.RBLabilitazione.SelectedIndex = 0
                If Me.setA_Address Then
                    Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                ElseIf Me.setCC_Address Then
                    Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
                ElseIf Me.setCCN_Address Then
                    Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
                Else
                    Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                End If
            End If

            If Me.RBLcomunita.SelectedValue = 0 Then
                Totale = oComunita.GetTotaleIscritti(oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, oPersona.Id, False, "")
            Else
                Totale = oComunita.GetTotaleIscritti(oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, oPersona.Id, True, Me.ComunitaPercorso)
            End If
            If IsNothing(oResource) Then
                SetCulture(Session("LinguaCode"))
            End If
            Me.CBXtutti.Text = oResource.getValue("tutti") & " (" & Totale & ")"
            Me.BindGruppi()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindGruppi()
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oDataset As DataSet
        Dim Totale As Integer

        Try
            oComunita.Id = Me.ComunitaID
            oPersona = Session("objPersona")

            Dim oAbilitazione As New Main.FiltroAbilitazione
            Try
                oAbilitazione = CType(Me.RBLabilitazione.SelectedValue, Main.FiltroAbilitazione)
            Catch ex As Exception
                oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                Me.RBLabilitazione.SelectedIndex = 0
                If Me.setA_Address Then
                    Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                ElseIf Me.setCC_Address Then
                    Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
                ElseIf Me.setCCN_Address Then
                    Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
                Else
                    Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                End If
            End Try


            If Me.RBLcomunita.SelectedValue = 0 Then
                oDataset = oComunita.RuoliAssociatiByIscrizione(Session("LinguaID"), Main.FiltroRuoli.ForAll_NoGuest, oPersona.Id, oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, False)
            Else
                oDataset = oComunita.RuoliAssociatiByIscrizione(Session("LinguaID"), Main.FiltroRuoli.ForAll_NoGuest, oPersona.Id, oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, True)
            End If
            If oDataset.HasErrors = False Then
                If oDataset.Tables.Count > 0 Then
                    If oDataset.Tables(0).Rows.Count > 1 Then
                        Me.CBLgruppi.DataSource = oDataset
                        If Me.RBLcomunita.SelectedValue = 0 Then
                            Me.CBLgruppi.DataTextField = "TPRL_DataTextField"
                        Else
                            Me.CBLgruppi.DataTextField = "TPRL_Nome"
                        End If
                        Me.CBLgruppi.DataValueField = "TPRL_ID"
                        Me.CBLgruppi.DataBind()
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Bind_Filtri"
    Private Sub Bind_TipoRuolo()
        Dim dstable As New DataSet
        Dim i, Totale, TPRL_ID As Integer
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona


        Try
            oComunita.Id = Session("IdComunita")
            oPersona = Session("objPersona")

            Dim oAbilitazione As New Main.FiltroAbilitazione
            Try
                oAbilitazione = CType(Me.RBLabilitazione.SelectedValue, Main.FiltroAbilitazione)
            Catch ex As Exception
                oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                Me.RBLabilitazione.SelectedIndex = 0
                If Me.setA_Address Then
                    Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                ElseIf Me.setCC_Address Then
                    Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
                ElseIf Me.setCCN_Address Then
                    Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
                Else
                    Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
                End If
            End Try

            If Me.RBLcomunita.SelectedValue = 0 Then
                dstable = oComunita.RuoliAssociatiByIscrizione(Session("LinguaID"), Main.FiltroRuoli.ForAll_NoGuest, oPersona.Id, oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, False)
            Else
                dstable = oComunita.RuoliAssociatiByIscrizione(Session("LinguaID"), Main.FiltroRuoli.ForAll_NoGuest, oPersona.Id, oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, True)
            End If
        Catch ex As Exception

        End Try

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Not (dstable.HasErrors) Then
            DDLTipoRuolo.Items.Clear()
            DDLTipoRuolo.Items.Add(New ListItem("Tutti", -1))
            Me.oResource.setDropDownList(Me.DDLTipoRuolo, -1)
            If dstable.Tables.Count > 0 Then
                Totale = dstable.Tables(0).Rows.Count() - 1
                Dim stringaGruppi As String


                For i = 0 To Totale
                    Dim oRow As DataRow
                    oRow = dstable.Tables(0).Rows(i)
                    TPRL_ID = oRow.Item("TPRL_ID")

                    If Me.setA_Address Then
                        stringaGruppi = Me.HDNgruppi_A.Value
                    ElseIf Me.setCC_Address Then
                        stringaGruppi = Me.HDNgruppi_CC.Value
                    ElseIf Me.setCCN_Address Then
                        stringaGruppi = Me.HDNgruppi_CCN.Value
                    End If
                    If InStr(stringaGruppi, "," & TPRL_ID & ",") = 0 Then
						Dim oListItem As New ListItem
                        If IsDBNull(oRow.Item("TPRL_Nome")) Then
                            oListItem.Text = "--"
                        Else
                            oListItem.Text = oRow.Item("TPRL_Nome")
                        End If
                        oListItem.Value = TPRL_ID
                        DDLTipoRuolo.Items.Add(oListItem)
                        oListItem = Nothing
                    End If
                Next
            End If
        End If
    End Sub
#End Region

#Region "Gestione Filtri"
    Private Sub Bind_Griglia(ByVal oFiltro As Main.FiltroAbilitazione, Optional ByVal ricalcola As Boolean = False)
        Dim oPersona As New COL_Persona
        Dim dsTable As New DataSet
        Try
            oPersona = Session("objPersona")
            dsTable = FiltraggioDati(oFiltro, ricalcola)

            dsTable.Tables(0).Columns.Add(New DataColumn("oCheck"))
            Dim i, totale As Integer

            totale = dsTable.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGiscritti.Visible = False
                LBnoIscritti.Visible = True
                Me.DGiscritti.VirtualItemCount = 0

                LBnoIscritti.Text = oResource.getValue("LBnoIscritti.1.text") '"Nessun utente in questa categoria"
            Else
                Me.DGiscritti.VirtualItemCount = dsTable.Tables(0).Rows(0).Item("Totale")

                dsTable.Tables(0).Columns.Add(New DataColumn("oIscrittoIl"))
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = dsTable.Tables(0).Rows(i)

                    If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                        If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                            oRow.Item("oIscrittoIl") = "--"
                        Else
                            oRow.Item("oIscrittoIl") = FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                        End If
                    Else
                        oRow.Item("oIscrittoIl") = "--"
                    End If


                    'Dim t As Integer
                    Dim idUtentiSelezionati() As String
                    Dim j As Integer
                    idUtentiSelezionati = Me.HDazione.Value.Split(",")
                    For j = 1 To idUtentiSelezionati.Length - 2
                        If oRow.Item("PRSN_ID") = idUtentiSelezionati(j) Then
                            oRow.Item("oCheck") = "checked"
                        End If
                    Next

                Next
                If totale > 0 Then
                    Mod_Visualizzazione(totale - 1)

                    dsTable.Tables(0).Columns.Add("oCheckDisabled")
                    For i = 0 To totale - 1
                        Dim oRow As DataRow
                        oRow = dsTable.Tables(0).Rows(i)

                        oRow.Item("oCheckDisabled") = ""

                        If oPersona.Id = oRow.Item("PRSN_ID") Then
                            oRow.Item("oCheckDisabled") = "disabled"
                        End If
                    Next

                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If ViewState("SortExspression") = "" Then
                        ViewState("SortExspression") = "PRSN_Cognome"
                        ViewState("SortDirection") = "asc"
                    End If
                    oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")

                    Me.DGiscritti.DataSource = oDataview
                    Me.DGiscritti.DataBind()

                    Me.DTLiscritti.DataSource = oDataview
                    DTLiscritti.DataBind()
                    LBnoIscritti.Visible = False
                Else
                    Me.DGiscritti.Visible = False
                    LBnoIscritti.Visible = True
                    LBnoIscritti.Text = oResource.getValue("LBnoIscritti.0.text") '"Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
                End If
            End If
        Catch ex As Exception
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
            LBnoIscritti.Text = oResource.getValue("LBnoIscritti.1.text") ' "Nessun utente in questa categoria"
        End Try
    End Sub

    Private Function FiltraggioDati(ByVal oFiltro As Main.FiltroAbilitazione, Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim oPersona As New COL_Persona
            Dim Valore As String
            oPersona = Session("objPersona")

            Dim oComunita As New COL_Comunita
            oComunita.Id = Me.ComunitaID

            Dim TPRL_id As Integer
            TPRL_id = Me.DDLTipoRuolo.SelectedValue

            Dim oFiltroCampoOrdine As COL_Comunita.FiltroCampoOrdine
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento

            Dim oFiltroAnagrafica As Main.FiltroAnagrafica
            Dim oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti

            If Me.TXBvalore.Text <> "" Then
                Valore = Trim(Me.TXBvalore.Text)
            End If
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
                Me.ViewState("intAnagrafica") = -1
            End Try


            Try
                If Valore <> "" Then
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
                                If IsDate(Valore) Then
                                    Valore = Main.DateToString(Valore, False)
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
                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try

            If ViewState("SortExspression") = "" Or LCase(ViewState("SortExspression")) = "prsn_anagrafica" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
            ElseIf LCase(ViewState("SortExspression")) = "prsn_datanascita" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataNascita
            ElseIf LCase(ViewState("SortExspression")) = "tprl_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoRuolo
            ElseIf LCase(ViewState("SortExspression")) = "tppr_descrizione" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoPersona
            ElseIf LCase(ViewState("SortExspression")) = "prsn_login" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.login
            ElseIf LCase(ViewState("SortExspression")) = "rlpc_iscrittoil" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataIscrizione
            ElseIf LCase(ViewState("SortExspression")) = "prsn_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.nome
            ElseIf LCase(ViewState("SortExspression")) = "prsn_cognome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            Else
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            End If

            Dim ordinamento As Integer
            If ViewState("SortDirection") = "" Or ViewState("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If


            Dim totale As Decimal
            If ricalcola Then
                Me.ViewState("intCurPage") = 0
                Me.DGiscritti.CurrentPageIndex = 0
            End If

            If Me.DDLTipoRuolo.SelectedValue < 1 Then
                Dim gruppiEsclusi As String
                If Me.setA_Address Then
                    gruppiEsclusi = Me.HDNgruppi_A.Value
                ElseIf Me.setCC_Address Then
                    gruppiEsclusi = Me.HDNgruppi_CC.Value
                ElseIf Me.setCCN_Address Then
                    gruppiEsclusi = Me.HDNgruppi_CCN.Value
                End If
                Return oComunita.ElencaIscrittiNoMittenteEsclusiGruppi(Session("LinguaID"), Session("objPersona").id, oFiltro, Main.FiltroUtenti.NoPassantiNoCreatori, gruppiEsclusi, Me.DGiscritti.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroAnagrafica, oFiltroOrdinamento, oFiltroCampoOrdine, , oFiltroCampoOrdine)
            Else
                Return oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), Session("objPersona").id, oFiltro, Main.FiltroUtenti.NoPassantiNoCreatori, TPRL_id, Me.DGiscritti.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroAnagrafica, oFiltroOrdinamento, oFiltroCampoOrdine, , oFiltroRicerca)
            End If
        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, True)
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
        Me.ViewState("intCurPage") = 0
        Me.DGiscritti.CurrentPageIndex = 0
        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, True)
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
    'Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click

    'End Sub

    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        Me.DGiscritti.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, True)
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, True)
    End Sub
#Region "Gestione Griglia"
    Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
        If oRecord > Me.DGiscritti.PageSize Or oRecord > Me.DDLNumeroRecord.SelectedValue Or Me.DGiscritti.VirtualItemCount > Me.DGiscritti.PageSize Then
            Me.DGiscritti.AllowPaging = True
            Me.DGiscritti.PageSize = Me.DDLNumeroRecord.SelectedItem.Value
            Me.DGiscritti.Visible = True
        Else
            Me.DGiscritti.Visible = False
        End If
        If oRecord < 0 Then
            Me.DGiscritti.Visible = False
        End If
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGiscritti.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = ViewState("SortExspression")
        oSortDirection = ViewState("SortDirection")
        ViewState("SortExspression") = e.SortExpression

        If LCase(e.SortExpression) = LCase(oSortExpression) Then
            If ViewState("SortDirection") = "asc" Then
                ViewState("SortDirection") = "desc"
            Else
                ViewState("SortDirection") = "asc"
            End If
        End If
        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, False)
    End Sub
    Private Sub CambioPagina(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGiscritti.PageIndexChanged
        Dim oSortExpression, oSortDirection As String

        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, False)
    End Sub
    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGiscritti.ItemCreated
        Dim i As Integer
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
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
                    oWebControl.CssClass = "PagerLink"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "PagerSpan"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "PagerLink"
                    oResource.setPageDatagrid(Me.DGiscritti, oLinkbutton)
                End Try
            Next
        End If
    End Sub

#End Region

#End Region

    Private Sub CBXtutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXtutti.CheckedChanged
        If Me.CBXtutti.Checked Then
            Me.CBLgruppi.SelectedIndex = -1
            Me.CBLgruppi.Enabled = False
        Else
            Me.CBLgruppi.Enabled = True
        End If

        If Me.setA_Address Then
            Me.HDNgruppiTutti_A.Value = Me.CBXtutti.Checked
            If Me.CBXtutti.Checked Then
                Me.HDNgruppiNome_A.Value = ""
                Me.HDNgruppi_A.Value = ""
            End If
        ElseIf Me.setCC_Address Then
            Me.HDNgruppiTutti_CC.Value = Me.CBXtutti.Checked
            If Me.CBXtutti.Checked Then
                Me.HDNgruppiNome_CC.Value = ""
                Me.HDNgruppi_CC.Value = ""
            End If
        ElseIf Me.setCCN_Address Then
            Me.HDNgruppiTutti_CCN.Value = Me.CBXtutti.Checked
            If Me.CBXtutti.Checked Then
                Me.HDNgruppiNome_CCN.Value = ""
                Me.HDNgruppi_CCN.Value = ""
            End If
        Else
            Me.HDNgruppiTutti_A.Value = Me.CBXtutti.Checked
            If Me.CBXtutti.Checked Then
                Me.HDNgruppiNome_A.Value = ""
                Me.HDNgruppi_A.Value = ""
            End If
        End If
    End Sub

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        If Me.TBSmenu.SelectedIndex = 0 Then
            Me.PNLgruppi.Visible = True
            Me.PNLsingoli.Visible = False
        Else
            If Me.PNLsingoli.Visible = False Then
                Me.PNLsingoli.Visible = True
                Me.PNLgruppi.Visible = False

                Dim i As Integer
                Try

                    Dim isCambioRuolo As Boolean
                    Dim oldGruppo As String

                    If Me.setA_Address Then
                        oldGruppo = Me.HDNgruppi_A.Value
                        Me.HDNgruppi_A.Value = ""
                        Me.HDNgruppiNome_A.Value = ""
                    ElseIf Me.setCC_Address Then
                        oldGruppo = Me.HDNgruppi_CC.Value
                        Me.HDNgruppi_CC.Value = ""
                        Me.HDNgruppiNome_CC.Value = ""
                    ElseIf Me.setCCN_Address Then
                        oldGruppo = Me.HDNgruppi_CCN.Value
                        Me.HDNgruppi_CCN.Value = ""
                        Me.HDNgruppiNome_CCN.Value = ""
                    End If

                    If Me.CBLgruppi.SelectedIndex > -1 Then
                        For i = Me.CBLgruppi.SelectedIndex To Me.CBLgruppi.Items.Count - 1
                            If Me.CBLgruppi.Items.Item(i).Selected Then
                                Dim stringa, testo As String
                                testo = Me.CBLgruppi.Items.Item(i).Text
                                stringa = testo.Substring(testo.LastIndexOf("(") + 1, testo.LastIndexOf(")") - testo.LastIndexOf("(") - 1)

                                If Me.setA_Address Then
                                    If Me.HDNgruppi_A.Value = "" Then
                                        Me.HDNgruppi_A.Value = "," & Me.CBLgruppi.Items.Item(i).Value & ","
                                        Me.HDNgruppiNome_A.Value = "§" & Me.CBLgruppi.Items.Item(i).Text & "§"
                                    Else
                                        Me.HDNgruppi_A.Value = Me.HDNgruppi_A.Value & Me.CBLgruppi.Items.Item(i).Value & ","
                                        Me.HDNgruppiNome_A.Value = Me.HDNgruppiNome_A.Value & Me.CBLgruppi.Items.Item(i).Text & "§"
                                    End If
                                ElseIf Me.setCC_Address Then
                                    If Me.HDNgruppi_CC.Value = "" Then
                                        Me.HDNgruppi_CC.Value = "," & Me.CBLgruppi.Items.Item(i).Value & ","
                                        Me.HDNgruppiNome_CC.Value = "§" & Me.CBLgruppi.Items.Item(i).Text & "§"
                                    Else
                                        Me.HDNgruppi_CC.Value = Me.HDNgruppi_CC.Value & Me.CBLgruppi.Items.Item(i).Value & ","
                                        Me.HDNgruppiNome_CC.Value = Me.HDNgruppiNome_CC.Value & Me.CBLgruppi.Items.Item(i).Text & "§"
                                    End If
                                ElseIf Me.setCCN_Address Then
                                    If Me.HDNgruppi_CCN.Value = "" Then
                                        Me.HDNgruppi_CCN.Value = "," & Me.CBLgruppi.Items.Item(i).Value & ","
                                        Me.HDNgruppiNome_CCN.Value = "§" & Me.CBLgruppi.Items.Item(i).Text & "§"
                                    Else
                                        Me.HDNgruppi_CCN.Value = Me.HDNgruppi_CCN.Value & Me.CBLgruppi.Items.Item(i).Value & ","
                                        Me.HDNgruppiNome_CCN.Value = Me.HDNgruppiNome_CCN.Value & Me.CBLgruppi.Items.Item(i).Text & "§"
                                    End If
                                End If
                            End If
                        Next
                    Else
                        If Me.setA_Address Then
                            Me.HDNgruppi_A.Value = ""
                            Me.HDNgruppiNome_A.Value = ""
                        ElseIf Me.setCC_Address Then
                            Me.HDNgruppi_CC.Value = ""
                            Me.HDNgruppiNome_CC.Value = ""
                        ElseIf Me.setCCN_Address Then
                            Me.HDNgruppi_CCN.Value = ""
                            Me.HDNgruppiNome_CCN.Value = ""
                        End If
                    End If

                    If Me.setA_Address Then
                        isCambioRuolo = (oldGruppo <> Me.HDNgruppi_A.Value)
                    ElseIf Me.setCC_Address Then
                        isCambioRuolo = (oldGruppo <> Me.HDNgruppi_CC.Value)
                    ElseIf Me.setCCN_Address Then
                        isCambioRuolo = (oldGruppo <> Me.HDNgruppi_CCN.Value)
                    End If
                    If isCambioRuolo Then
                        Me.Bind_TipoRuolo()
                        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, True)
                    Else
                        If Me.setA_Address And Me.HDNgruppi_A.Value = "" Then
                            Me.Bind_TipoRuolo()
                        ElseIf Me.setCC_Address And Me.HDNgruppi_CC.Value = "" Then
                            Me.Bind_TipoRuolo()
                        ElseIf Me.setCCN_Address And Me.HDNgruppi_CCN.Value = "" Then
                            Me.Bind_TipoRuolo()
                        Else
                            Me.Bind_TipoRuolo()
                        End If
                        Me.Bind_Griglia(Me.RBLabilitazione.SelectedValue, False)
                    End If


                Catch ex As Exception

                End Try
            End If
        End If
    End Sub

#Region "Metodi Standard"
    'riga = PRSN_ID,TPRL_ID,Mail
    'data una riga estrae l'id della persona !
    Private Function getPRSN_ID(ByVal valore) As Integer
        Dim oArray() As String

        Try
            oArray = Split(valore, ",")
            Return CInt(oArray(0))
        Catch ex As Exception
            Return 0
        End Try
        Return 0
    End Function
    ' Data una riga estrae il Ruolo svolto
    Private Function getTipo_Ruolo(ByVal valore) As Integer
        Dim oArray() As String

        Try
            oArray = Split(valore, ",")
            Return CInt(oArray(1))
        Catch ex As Exception
            Return 0
        End Try
        Return 0
    End Function
    ' Data una riga estrae l'indirizzo mail svolto
    Private Function getMail(ByVal valore) As String
        Dim oArray() As String

        Try
            oArray = Split(valore, ",")
            Return oArray(2)
        Catch ex As Exception
            Return 0
        End Try
        Return 0
    End Function
#End Region

    Private Function getIndirizziMail(ByVal oTipo As TipoDestinatarioMail) As COL_CollectionContatti
        Dim ElencoContatti As New COL_CollectionContatti
        Dim selectAll As Boolean = False
        Dim i, totale As Integer


        ' Recupero se ci sono gruppi !!
        Select Case oTipo
            Case TipoDestinatarioMail.A
                selectAll = (Me.HDNgruppiTutti_A.Value = "True")
            Case TipoDestinatarioMail.CC
                selectAll = (Me.HDNgruppiTutti_CC.Value = "True")
            Case TipoDestinatarioMail.CCN
                selectAll = (Me.HDNgruppiTutti_CCN.Value = "True")
            Case Else
                selectAll = (Me.HDNgruppiTutti_A.Value = "True")
        End Select


        'Recupero gli Indirizzi Fissi !!!
        If selectAll = False Then
            Dim ElencoId As String
            'Dim oListBox As ListBox
            Select Case oTipo
                Case TipoDestinatarioMail.A
                    ElencoId = Me.HDazione_A.Value
                Case TipoDestinatarioMail.CC
                    ElencoId = Me.HDazione_CC.Value
                Case TipoDestinatarioMail.CCN
                    ElencoId = Me.HDazione_CCN.Value
                Case Else
                    ElencoId = Me.HDazione_A.Value
            End Select

            If ElencoId <> "" Then
                Dim oComunita As COL_Comunita
                Dim oDataset As DataSet
                Try
                    oDataset = oComunita.getMailAnagraficaIscritti(Me.ComunitaID, ElencoId)

                    totale = oDataset.Tables(0).Rows.Count - 1
                    Dim nomeDestinatario, id_Destinatario, mail_Destinatario, quote As String
                    quote = """"
                    For i = 0 To totale
                        Dim oRow As DataRow
                        oRow = oDataset.Tables(0).Rows(i)

                        nomeDestinatario = oRow.Item("PRSN_Cognome") & oRow.Item("PRSN_Nome")
                        id_Destinatario = oRow.Item("PRSN_ID")
                        mail_Destinatario = oRow.Item("PRSN_Mail")

                        ElencoContatti.Add(New COL_Contatto(id_Destinatario, nomeDestinatario, mail_Destinatario, -1000, oTipo))
                    Next
                Catch ex As Exception

                End Try
            End If

        End If
        Dim oContatto As COL_Contatto
        For Each oContatto In Me.getIndirizziMailGruppi(oTipo)
            ElencoContatti.Add(oContatto)
        Next

        Return ElencoContatti
    End Function

    Private Function getIndirizziMailGruppi(ByVal oTipo As TipoDestinatarioMail) As COL_CollectionContatti
        Dim ElencoContatti As New COL_CollectionContatti
        Dim selectAll As Boolean = False
        Dim i, j, totale As Integer
        Dim InSottoComunita As Boolean = False

        Dim oAbilitazione As Main.FiltroAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
        Dim elencoGruppi() As String

        Select Case oTipo
            Case TipoDestinatarioMail.A
                selectAll = (Me.HDNgruppiTutti_A.Value = "True")
                InSottoComunita = (Me.HDN_RBLcomunita_A.Value <> "0")
                Try
                    oAbilitazione = CType(Me.HDN_RBLabilitazioneA.Value, Main.FiltroAbilitazione)
                Catch ex As Exception
                    oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                End Try
                If selectAll = False Then
                    elencoGruppi = Me.HDNgruppi_A.Value.Split(",")
                End If
            Case TipoDestinatarioMail.CC
                selectAll = (Me.HDNgruppiTutti_CC.Value = "True")
                InSottoComunita = (Me.HDN_RBLcomunita_CC.Value <> "0")
                Try
                    oAbilitazione = CType(Me.HDN_RBLabilitazioneCC.Value, Main.FiltroAbilitazione)
                Catch ex As Exception
                    oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                End Try
                If selectAll = False Then
                    elencoGruppi = Me.HDNgruppi_CC.Value.Split(",")
                End If
            Case TipoDestinatarioMail.CCN
                selectAll = (Me.HDNgruppiTutti_CCN.Value = "True")
                InSottoComunita = (Me.HDN_RBLcomunita_CCN.Value <> "0")
                Try
                    oAbilitazione = CType(Me.HDN_RBLabilitazioneCCN.Value, Main.FiltroAbilitazione)
                Catch ex As Exception
                    oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                End Try
                If selectAll = False Then
                    elencoGruppi = Me.HDNgruppi_CCN.Value.Split(",")
                End If
            Case Else
                selectAll = (Me.HDNgruppiTutti_A.Value = "True")
                InSottoComunita = (Me.HDN_RBLcomunita_A.Value <> "0")
                Try
                    oAbilitazione = CType(Me.HDN_RBLabilitazioneA.Value, Main.FiltroAbilitazione)
                Catch ex As Exception
                    oAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                End Try
                If selectAll = False Then
                    elencoGruppi = Me.HDNgruppi_A.Value.Split(",")
                End If
        End Select

        Dim TotaleUser, RuoloID As Integer
        Dim oComunita As New COL_Comunita
        oComunita.Id = Me.ComunitaID

        If selectAll Then
            ReDim elencoGruppi(1)
            elencoGruppi(0) = -1
        End If
        If IsArray(elencoGruppi) Then
            Try
                totale = elencoGruppi.Length - 1
                For i = 0 To totale
                    If IsNumeric(elencoGruppi(i)) Then
                        Dim oDataset As New DataSet
                        RuoloID = elencoGruppi(i)

                        oDataset = oComunita.GetIscrittiByRuolo(RuoloID, oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, InSottoComunita, False, Session("objPersona").id)
                        Try
                            TotaleUser = oDataset.Tables(0).Rows.Count - 1
                            If TotaleUser >= 0 Then
                                For j = 0 To TotaleUser
                                    Dim oRow As DataRow
                                    oRow = oDataset.Tables(0).Rows(j)
                                    If RuoloID = -1 Then
                                        ElencoContatti.Add(New COL_Contatto(oRow.Item("PRSN_ID"), oRow.Item("PRSN_Anagrafica"), oRow.Item("PRSN_Mail"), -1000, oTipo))
                                    Else
                                        ElencoContatti.Add(New COL_Contatto(oRow.Item("PRSN_ID"), oRow.Item("PRSN_Anagrafica"), oRow.Item("PRSN_Mail"), RuoloID, oTipo, True))
                                    End If
                                Next
                            End If
                        Catch exNoUserForRuolo As Exception

                        End Try
                        If RuoloID <= 0 Then
                            Exit For
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try
        End If
        Return ElencoContatti
    End Function
    Private Function getDestinatariMail(ByVal filtra As TipoDestinatarioMail) As String
        Dim i, totale, pos As Integer
        Dim response As String
        Dim selectAll As Boolean = False
        response = ""

        Select Case filtra
            Case TipoDestinatarioMail.A
                If Me.HDNgruppiTutti_A.Value = "" Or Me.HDNgruppiTutti_A.Value = "False" Then
                    selectAll = False
                Else
                    selectAll = True
                End If
            Case TipoDestinatarioMail.CC
                If Me.HDNgruppiTutti_CC.Value = "" Or Me.HDNgruppiTutti_CC.Value = "False" Then
                    selectAll = False
                Else
                    selectAll = True
                End If
            Case TipoDestinatarioMail.CCN
                If Me.HDNgruppiTutti_CCN.Value = "" Or Me.HDNgruppiTutti_CCN.Value = "False" Then
                    selectAll = False
                Else
                    selectAll = True
                End If
            Case Else
                If Me.HDNgruppiTutti_A.Value = "" Or Me.HDNgruppiTutti_A.Value = "False" Then
                    selectAll = False
                Else
                    selectAll = True
                End If
        End Select
        If selectAll = True Then
            ' usare la localizzazione !!!!
            Return "[" & "Tutti" & "];"
        Else
            ' Recupero Elenco gruppi selezionati
            Dim elencoGruppiNome() As String

            Select Case filtra
                Case TipoDestinatarioMail.A
                    elencoGruppiNome = Me.HDNgruppiNome_A.Value.Split("§")
                Case TipoDestinatarioMail.CC
                    elencoGruppiNome = Me.HDNgruppiNome_CC.Value.Split("§")
                Case TipoDestinatarioMail.CCN
                    elencoGruppiNome = Me.HDNgruppiNome_CCN.Value.Split("§")
                Case Else
                    elencoGruppiNome = Me.HDNgruppiNome_A.Value.Split("§")
            End Select

            totale = elencoGruppiNome.Length - 1
            Try
                For i = 0 To totale
                    If elencoGruppiNome(i) <> "" Then
                        response = response & "[" & elencoGruppiNome(i) & "];"
                    End If
                Next
            Catch ex As Exception

            End Try

            'Recupero singoli
            Dim ElencoId As String
            Select Case filtra
                Case TipoDestinatarioMail.A
                    ElencoId = Me.HDazione_A.Value
                Case TipoDestinatarioMail.CC
                    ElencoId = Me.HDazione_CC.Value
                Case TipoDestinatarioMail.CCN
                    ElencoId = Me.HDazione_CCN.Value
                Case Else
                    ElencoId = Me.HDazione_A.Value
            End Select

            If ElencoId <> "" Then
                Dim oComunita As COL_Comunita
                Dim oDataset As DataSet
                Try
                    oDataset = oComunita.getMailAnagraficaIscritti(Me.ComunitaID, ElencoId)

                    totale = oDataset.Tables(0).Rows.Count - 1
                    Dim nomeDestinatario, id_Destinatario, mail_Destinatario, quote As String
                    quote = """"
                    For i = 0 To totale
                        Dim oRow As DataRow
                        oRow = oDataset.Tables(0).Rows(i)

                        nomeDestinatario = "<b>" & oRow.Item("PRSN_Cognome") & "</b> " & oRow.Item("PRSN_Nome")
                        id_Destinatario = oRow.Item("PRSN_ID")
                        mail_Destinatario = oRow.Item("PRSN_Mail")

                        response = response & nomeDestinatario & ";"
                        If response.Length > 400 Then
                            response = response & " .... "
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
            End If
        End If

        Return response
    End Function

    Public Function ResetForm()
        Dim oListBox As ListBox

        'If Me.setA_Address Then
        '    oListBox = Me.LSB_A
        'ElseIf Me.setCC_Address Then
        '    oListBox = Me.LSB_CC
        'ElseIf Me.setCCN_Address Then
        '    oListBox = Me.LSB_CCN
        'Else
        '    oListBox = Me.LSB_A
        'End If
        'oListBox.Items.Clear()

        Me.PNLgruppi.Visible = True
        Me.PNLsingoli.Visible = False

        Me.CBLgruppi.SelectedIndex = -1

        If Me.setA_Address Then
            Me.HDNgruppiNome_A.Value = ""
            Me.HDNgruppi_A.Value = ""
        ElseIf Me.setCC_Address Then
            Me.HDNgruppiNome_CC.Value = ""
            Me.HDNgruppi_CC.Value = ""
        ElseIf Me.setCCN_Address Then
            Me.HDNgruppiNome_CCN.Value = ""
            Me.HDNgruppi_CCN.Value = ""
        Else
            Me.HDNgruppiNome_A.Value = ""
            Me.HDNgruppi_A.Value = ""
        End If
        Me.CBXtutti.Checked = False
        Me.CBLgruppi.Enabled = True
        Me.CBLgruppi.SelectedIndex = -1
        If Me.setA_Address Then
            Me.HDNgruppiTutti_A.Value = "False"
            If Me.HDNgruppiTutti_CC.Value = "" Then
                Me.HDNgruppiTutti_CC.Value = "False"
            End If
            If Me.HDNgruppiTutti_CCN.Value = "" Then
                Me.HDNgruppiTutti_CCN.Value = "False"
            End If
        ElseIf Me.setCC_Address Then
            If Me.HDNgruppiTutti_A.Value = "" Then
                Me.HDNgruppiTutti_A.Value = "False"
            End If
            Me.HDNgruppiTutti_CC.Value = "False"
            If Me.HDNgruppiTutti_CCN.Value = "" Then
                Me.HDNgruppiTutti_CCN.Value = "False"
            End If
        ElseIf Me.setCCN_Address Then
            If Me.HDNgruppiTutti_A.Value = "" Then
                Me.HDNgruppiTutti_A.Value = "False"
            End If
            If Me.HDNgruppiTutti_CC.Value = "" Then
                Me.HDNgruppiTutti_CC.Value = "False"
            End If
            Me.HDNgruppiTutti_CCN.Value = "False"
        Else
            Me.HDNgruppiTutti_A.Value = "False"
            If Me.HDNgruppiTutti_CC.Value = "" Then
                Me.HDNgruppiTutti_CC.Value = "False"
            End If
            If Me.HDNgruppiTutti_CCN.Value = "" Then
                Me.HDNgruppiTutti_CCN.Value = "False"
            End If
        End If

        Me.TBSmenu.SelectedIndex = 0
        Me.PNLgruppi.Visible = True
        Me.PNLsingoli.Visible = False
        Me.RBLcomunita.SelectedIndex = 0

        If Me.setA_Address Then
            Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
            If Me.HDN_RBLcomunita_CC.Value = "" Then
                Me.HDN_RBLcomunita_CC.Value = Me.RBLcomunita.SelectedValue
            End If
            If Me.HDN_RBLcomunita_CCN.Value = "" Then
                Me.HDN_RBLcomunita_CCN.Value = Me.RBLcomunita.SelectedValue
            End If
        ElseIf Me.setCC_Address Then
            If Me.HDN_RBLcomunita_A.Value = "" Then
                Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
            End If
            Me.HDN_RBLcomunita_CC.Value = Me.RBLcomunita.SelectedValue
            If Me.HDN_RBLcomunita_CCN.Value = "" Then
                Me.HDN_RBLcomunita_CCN.Value = Me.RBLcomunita.SelectedValue
            End If
        ElseIf Me.setCCN_Address Then
            If Me.HDN_RBLcomunita_A.Value = "" Then
                Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
            End If
            If Me.HDN_RBLcomunita_CC.Value = "" Then
                Me.HDN_RBLcomunita_CC.Value = Me.RBLcomunita.SelectedValue
            End If
            Me.HDN_RBLcomunita_CCN.Value = Me.RBLcomunita.SelectedValue
        Else
            Me.HDN_RBLcomunita_A.Value = Me.RBLcomunita.SelectedValue
            If Me.HDN_RBLcomunita_CC.Value = "" Then
                Me.HDN_RBLcomunita_CC.Value = Me.RBLcomunita.SelectedValue
            End If
            If Me.HDN_RBLcomunita_CCN.Value = "" Then
                Me.HDN_RBLcomunita_CCN.Value = Me.RBLcomunita.SelectedValue
            End If
        End If



        Me.RBLabilitazione.SelectedIndex = 0
        If Me.setA_Address Then
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
            If Me.HDN_RBLabilitazioneCC.Value = "" Then
                Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
            End If
            If Me.HDN_RBLabilitazioneCCN.Value = "" Then
                Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
            End If
        ElseIf Me.setCC_Address Then
            If Me.HDN_RBLabilitazioneA.Value = "" Then
                Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
            End If
            Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
            If Me.HDN_RBLabilitazioneCCN.Value = "" Then
                Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
            End If
        ElseIf Me.setCCN_Address Then
            If Me.HDN_RBLabilitazioneA.Value = "" Then
                Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
            End If
            If Me.HDN_RBLabilitazioneCC.Value = "" Then
                Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
            End If
            Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
        Else
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
            If Me.HDN_RBLabilitazioneCC.Value = "" Then
                Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
            End If
            If Me.HDN_RBLabilitazioneCCN.Value = "" Then
                Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
            End If
        End If
    End Function

    Public Function UpdateForm()
        Me.PNLgruppi.Visible = True
        Me.PNLsingoli.Visible = False


        If Me.setA_Address Then
            If Me.HDNgruppiTutti_A.Value = "True" Then
                Me.CBXtutti.Checked = True
                Me.CBLgruppi.Enabled = False
                Me.TBSmenu.SelectedIndex = 0
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True

                Me.TBSmenu.SelectedIndex = 0
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneA.Value
            Catch ex As Exception

            End Try
            Try
                Me.RBLcomunita.SelectedValue = Me.HDN_RBLcomunita_A.Value
            Catch ex As Exception

            End Try
            Dim selezionati As String
            selezionati = Me.GetGruppiSelezionati()

            Me.BindGruppi()
            If Me.CBXtutti.Checked = False Then
                Me.SetSelezionati(selezionati)
            End If
        ElseIf Me.setCC_Address Then
            If Me.HDNgruppiTutti_CC.Value = "True" Then
                Me.CBXtutti.Checked = True
                Me.CBLgruppi.Enabled = False
                Me.TBSmenu.SelectedIndex = 0
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True

                Me.TBSmenu.SelectedIndex = 0
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneCC.Value
            Catch ex As Exception

            End Try
            Try
                Me.RBLcomunita.SelectedValue = Me.HDN_RBLcomunita_CC.Value
            Catch ex As Exception

            End Try
            Dim selezionati As String
            selezionati = Me.GetGruppiSelezionati()

            Me.BindGruppi()
            If Me.CBXtutti.Checked = False Then
                Me.SetSelezionati(selezionati)
            End If
        ElseIf Me.setCCN_Address Then
            If Me.HDNgruppiTutti_CCN.Value = "True" Then
                Me.CBXtutti.Checked = True
                Me.CBLgruppi.Enabled = False
                Me.TBSmenu.SelectedIndex = 0
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True

                Me.TBSmenu.SelectedIndex = 0
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneCCN.Value
            Catch ex As Exception

            End Try
            Try
                Me.RBLcomunita.SelectedValue = Me.HDN_RBLcomunita_CCN.Value
            Catch ex As Exception

            End Try
            Dim selezionati As String
            selezionati = Me.GetGruppiSelezionati()

            Me.BindGruppi()
            If Me.CBXtutti.Checked = False Then
                Me.SetSelezionati(selezionati)
            End If
        Else
            If Me.HDNgruppiTutti_A.Value = "True" Then
                Me.CBXtutti.Checked = True
                Me.CBLgruppi.Enabled = False
                Me.TBSmenu.SelectedIndex = 0
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True

                Me.TBSmenu.SelectedIndex = 0
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneA.Value
            Catch ex As Exception

            End Try
            Try
                Me.RBLcomunita.SelectedValue = Me.HDN_RBLcomunita_A.Value
            Catch ex As Exception

            End Try
            Dim selezionati As String
            selezionati = Me.GetGruppiSelezionati()

            Me.BindGruppi()
            If Me.CBXtutti.Checked = False Then
                Me.SetSelezionati(selezionati)
            End If
        End If

        If Me.setA_Address Then
            Me.HDazione.Value = Me.HDazione_A.Value
        ElseIf Me.setCC_Address Then
            Me.HDazione.Value = Me.HDazione_CC.Value
        ElseIf Me.setCCN_Address Then
            Me.HDazione.Value = Me.HDazione_CCN.Value
        Else
            Me.HDazione.Value = Me.HDazione_A.Value
        End If
    End Function

    Public Function SalvaGruppiSelezionati()
        Dim i As Integer
        If Me.CBLgruppi.SelectedIndex > -1 Then
            If Me.setA_Address Then
                Me.HDNgruppi_A.Value = ""
                Me.HDNgruppiNome_A.Value = ""
            ElseIf Me.setCC_Address Then
                Me.HDNgruppi_CC.Value = ""
                Me.HDNgruppiNome_CC.Value = ""
            ElseIf Me.setCCN_Address Then
                Me.HDNgruppi_CCN.Value = ""
                Me.HDNgruppiNome_CCN.Value = ""
            End If
            For i = Me.CBLgruppi.SelectedIndex To Me.CBLgruppi.Items.Count - 1
                If Me.CBLgruppi.Items.Item(i).Selected Then
                    Dim stringa, testo As String
                    testo = Me.CBLgruppi.Items.Item(i).Text
                    ' stringa = testo.Substring(testo.LastIndexOf("(") + 1, testo.LastIndexOf(")") - testo.LastIndexOf("(") - 1)

                    If Me.setA_Address Then
                        If Me.HDNgruppi_A.Value = "" Then
                            Me.HDNgruppi_A.Value = "," & Me.CBLgruppi.Items.Item(i).Value & ","
                            Me.HDNgruppiNome_A.Value = "§" & Me.CBLgruppi.Items.Item(i).Text & "§"
                        Else
                            Me.HDNgruppi_A.Value = Me.HDNgruppi_A.Value & Me.CBLgruppi.Items.Item(i).Value & ","
                            Me.HDNgruppiNome_A.Value = Me.HDNgruppiNome_A.Value & Me.CBLgruppi.Items.Item(i).Text & "§"
                        End If
                    ElseIf Me.setCC_Address Then
                        If Me.HDNgruppi_CC.Value = "" Then
                            Me.HDNgruppi_CC.Value = "," & Me.CBLgruppi.Items.Item(i).Value & ","
                            Me.HDNgruppiNome_CC.Value = "§" & Me.CBLgruppi.Items.Item(i).Text & "§"
                        Else
                            Me.HDNgruppi_CC.Value = Me.HDNgruppi_CC.Value & Me.CBLgruppi.Items.Item(i).Value & ","
                            Me.HDNgruppiNome_CC.Value = Me.HDNgruppiNome_CC.Value & Me.CBLgruppi.Items.Item(i).Text & "§"
                        End If
                    ElseIf Me.setCCN_Address Then
                        If Me.HDNgruppi_CCN.Value = "" Then
                            Me.HDNgruppi_CCN.Value = "," & Me.CBLgruppi.Items.Item(i).Value & ","
                            Me.HDNgruppiNome_CCN.Value = "§" & Me.CBLgruppi.Items.Item(i).Text & "§"
                        Else
                            Me.HDNgruppi_CCN.Value = Me.HDNgruppi_CCN.Value & Me.CBLgruppi.Items.Item(i).Value & ","
                            Me.HDNgruppiNome_CCN.Value = Me.HDNgruppiNome_CCN.Value & Me.CBLgruppi.Items.Item(i).Text & "§"
                        End If
                    End If
                End If
            Next
        Else
            If Me.setA_Address Then
                Me.HDNgruppi_A.Value = ""
                Me.HDNgruppiNome_A.Value = ""
            ElseIf Me.setCC_Address Then
                Me.HDNgruppi_CC.Value = ""
                Me.HDNgruppiNome_CC.Value = ""
            ElseIf Me.setCCN_Address Then
                Me.HDNgruppi_CCN.Value = ""
                Me.HDNgruppiNome_CCN.Value = ""
            End If
        End If
    End Function


End Class
