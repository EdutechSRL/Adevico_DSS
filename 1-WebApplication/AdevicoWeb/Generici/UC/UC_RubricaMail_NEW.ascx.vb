Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2ServiziBase.ContattiMail


Public Class UC_RubricaMail_NEW
    Inherits System.Web.UI.UserControl
    Private oResource As ResourceManager

    Private n_BackColor As String

    Public Property BackColor() As String
        Get
            BackColor = n_BackColor
        End Get
        Set(ByVal Value As String)
            Try
                n_BackColor = Value
            Catch ex As Exception
                n_BackColor = "White"
            End Try
        End Set
    End Property
    Private Enum TabSingoli
        Singoli = 1
        singoliSelezionato = 2
        singoliDisattivato = 3
    End Enum
    Private Enum TabGruppi
        gruppi = 1
        gruppiSelezionato = 2
        gruppiDisattivato = 3
    End Enum

#Region "Private Property"
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
            Try
                setA_Address = (Me.HDN_setA.Value = "1")
            Catch ex As Exception
                setA_Address = False
            End Try

        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                Me.HDN_setA.Value = 1
                Me.HDN_setCC.Value = 0
                Me.HDN_setCCN.Value = 0
                Me.LSB_A.Visible = True
                Me.LSB_CC.Visible = False
                Me.LSB_CCN.Visible = False
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
                Me.LSB_A.Visible = False
                Me.LSB_CC.Visible = False
                Me.LSB_CCN.Visible = True
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
                Me.LSB_A.Visible = False
                Me.LSB_CC.Visible = True
                Me.LSB_CCN.Visible = False
            Else
                Me.HDN_setCC.Value = 0
            End If
        End Set
    End Property
    'Public ReadOnly Property GetGruppi() As String(,)
    '    Get
    '        If Me.HDN_setA.Value = 1 Then
    '            GetGruppi = GetGruppiA
    '        ElseIf Me.HDN_setCC.Value = 1 Then
    '            GetGruppi = GetGruppiCC
    '        ElseIf Me.HDN_setCCN.Value = 1 Then
    '            GetGruppi = GetGruppiCCN
    '        End If
    '    End Get
    'End Property
    'Public ReadOnly Property GetGruppiA() As String(,)
    '    Get
    '        GetGruppiA = Me.getElencoGruppi(TipoDestinatarioMail.A)
    '    End Get
    'End Property
    'Public ReadOnly Property GetGruppiCC() As String(,)
    '    Get
    '        GetGruppiCC = Me.getElencoGruppi(TipoDestinatarioMail.CC)
    '    End Get
    'End Property
    'Public ReadOnly Property GetGruppiCCN() As String(,)
    '    Get
    '        GetGruppiCCN = Me.getElencoGruppi(TipoDestinatarioMail.CCN)
    '    End Get
    'End Property

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

#End Region

#Region "Pannelli"
    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
    Protected WithEvents LBselezionaComunita As System.Web.UI.WebControls.Label
    Protected WithEvents RBLcomunita As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBselezionaDest As System.Web.UI.WebControls.Label
    Protected WithEvents RBLabilitazione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBseleziona As System.Web.UI.WebControls.Label
    'Protected WithEvents IMBgruppi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents IMBsingoli As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CBXtutti As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBLgruppi As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents PNLgruppi As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLfiltroTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNfind As System.Web.UI.WebControls.Button
    Protected WithEvents DDLfiltro As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBXelenco As System.Web.UI.WebControls.ListBox
    Protected WithEvents BTNscelta As System.Web.UI.WebControls.Button
    Protected WithEvents LSB_A As System.Web.UI.WebControls.ListBox
    Protected WithEvents LSB_CC As System.Web.UI.WebControls.ListBox
    Protected WithEvents LSB_CCN As System.Web.UI.WebControls.ListBox
    Protected WithEvents BTNrimuovi As System.Web.UI.WebControls.Button
    Protected WithEvents PNLsingoli As System.Web.UI.WebControls.Panel

    Protected WithEvents LBfiltraTipo As System.Web.UI.WebControls.Label

    Protected WithEvents HDN_PanView As System.Web.UI.HtmlControls.HtmlInputHidden
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

        'If Session("objPersona") Is Nothing Then 'se la sessione è scaduta redirecto alla home
        '    Response.Redirect("./../../index.aspx")
        'End If
    End Sub

    Private Sub StartupData()
        Try
            Me.Bind_Dati()
        Catch ex As Exception

        End Try

    End Sub
#Region "Localizzazione"
    Public Function SetCulture(ByVal Code As String)
        ' localizzazione utente
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Rubrica_New"
        oResource.Folder_Level1 = "Generici"
        oResource.setCulture()
    End Function

    Public Sub SetupInternazionalizzazione()
        With oResource

            TBSmenu.Tabs(0).Text = .getValue("RTABGruppi.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("RTABGruppi.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("RTABSingoli.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("RTABSingoli.ToolTip")

            .setLabel(LBselezionaComunita)
            .setLabel(LBselezionaDest)
            .setLabel(LBfiltraTipo)
            .setLabel(LBseleziona)
            .setRadioButtonList(Me.RBLcomunita, "0")
            .setRadioButtonList(Me.RBLcomunita, "-1")

            .setRadioButtonList(Me.RBLabilitazione, "1")
            .setRadioButtonList(Me.RBLabilitazione, "5")
            .setRadioButtonList(Me.RBLabilitazione, "4")
            .setRadioButtonList(Me.RBLabilitazione, "0")
            .setDropDownList(DDLfiltro, -1)
            .setDropDownList(DDLfiltro, 0)
            .setCheckBox(CBXtutti)
            .setButton(BTNfind)
        End With

    End Sub
#End Region

    Private Sub SetStartupScript()
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim oScript As String

        oScript = "<script language=Javascript>" & vbCrLf
        oScript = oScript & "CampiRubrica[0]='" & Replace(Me.DDLfiltro.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "CampiRubrica[1]='" & Replace(Me.BTNfind.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "CampiRubrica[2]='" & Replace(Me.BTNrimuovi.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "CampiRubrica[3]='" & Replace(Me.BTNscelta.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "CampiRubrica[4]='" & Replace(Me.LBXelenco.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "CampiRubrica[5]='" & Replace(Me.LSB_A.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "CampiRubrica[6]='" & Replace(Me.LSB_CC.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "CampiRubrica[7]='" & Replace(Me.LSB_CCN.UniqueID, ":", "_") & "';" & vbCrLf

        oScript = oScript & "</script>" & vbCrLf
        '  oPage = Me.Parent.Page
        If (Not Me.Page.ClientScript.IsClientScriptBlockRegistered("clientScriptRubrica")) Then
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScriptRubrica", oScript)
        End If

        DDLfiltro.Attributes.Add("onchange", "selezionaFiltrato(this);return false;")
        BTNfind.Attributes.Add("onclick", "findDestinatari();return false;")
        Me.BTNscelta.Attributes.Add("onclick", "return hasSelezionati();")

    End Sub

#Region "Gestione Selezione"
    Private Sub RBLcomunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLcomunita.SelectedIndexChanged
        If Me.RBLcomunita.SelectedValue = 0 Then
            Me.DDLfiltro.Enabled = True
        Else
            Me.DDLfiltro.Enabled = False
            Me.DDLfiltro.SelectedIndex = 0
        End If
        'selezionati = Me.GetGruppiSelezionati()
        'Me.BindGruppi()

        'If Me.CBXtutti.Checked = False Then
        '    Me.SetSelezionati(selezionati)
        'End If
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
        If totale > 0 Then
            Me.CBXtutti.Enabled = True
        Else
            Me.CBXtutti.Checked = False
            Me.CBXtutti.Enabled = False
        End If
        Me.BindGruppi()

        If Me.CBXtutti.Checked = False Then
            Me.SetSelezionati(selezionati)
        End If

        Me.Bind_IscrittiComunita()

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
        If totale > 0 Then
            Me.CBXtutti.Enabled = True
        Else
            Me.CBXtutti.Checked = False
            Me.CBXtutti.Enabled = False
        End If
        Me.BindGruppi()

        If Me.CBXtutti.Checked = False Then
            Me.SetSelezionati(selezionati)
        End If

        Me.Bind_IscrittiComunita()

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
                                Me.HDNgruppiNome_CC.Value = Me.HDNgruppiNome_CC.Value & oListItem.Text & "§"
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


#Region "Metodi Standard"
    Public Sub Bind_Dati()
        Dim Totale As Integer
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita

        Try
            oPersona = Session("objPersona")
            oComunita.Id = Me.ComunitaID

            If Me.RBLcomunita.SelectedIndex = -1 Then
                Me.RBLcomunita.SelectedIndex = 0
            End If

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
                Totale = oComunita.GetTotaleIscritti(oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, oPersona.Id, False, "")
            Else
                Totale = oComunita.GetTotaleIscritti(oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, oPersona.Id, True, Me.ComunitaPercorso)
            End If
            If IsNothing(oResource) Then
                SetCulture(Session("LinguaCode"))
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
            If Totale > 0 Then
                Me.CBXtutti.Enabled = True
            Else
                Me.CBXtutti.Checked = False
                Me.CBXtutti.Enabled = False
            End If
            Me.CBXtutti.Text = oResource.getValue("tutti") & " (" & Totale & ")"
            Me.BindGruppi()
        Catch ex As Exception
            Me.HDN_RBLcomunita_A.Value = 0
            Me.HDN_RBLcomunita_CCN.Value = 0
            Me.HDN_RBLcomunita_CC.Value = 0
        End Try
        Me.SetStartupScript()
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
                    If oDataset.Tables(0).Rows.Count > 0 Then
                        Me.CBLgruppi.DataSource = oDataset
                        If Me.RBLcomunita.SelectedValue = 0 Then
                            Me.CBLgruppi.DataTextField = "TPRL_DataTextField"
                        Else
                            Me.CBLgruppi.DataTextField = "TPRL_Nome"
                        End If
                        Me.CBLgruppi.DataValueField = "TPRL_ID"
                        Me.CBLgruppi.DataBind()
                    Else
                        Me.CBLgruppi.Items.Clear()
                    End If
                Else
                    Me.CBLgruppi.Items.Clear()
                End If
            Else
                Me.CBLgruppi.Items.Clear()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoRuolo()
        Dim dstable As New DataSet
        Dim i, Totale, TPRL_ID As Integer
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona


        Try
            oComunita.Id = Me.ComunitaID  'Session("IdComunita")
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

        If Not (dstable.HasErrors) Then
            DDLfiltro.Items.Clear()
            DDLfiltro.Items.Add(New ListItem("Nessuno", -1))
            DDLfiltroTipoRuolo.Items.Clear()
            DDLfiltroTipoRuolo.Items.Add(New ListItem("Tutti", 0))

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
						DDLfiltro.Items.Add(oListItem)
						DDLfiltroTipoRuolo.Items.Add(oListItem)
						oListItem = Nothing
					End If
				Next
				If DDLfiltroTipoRuolo.SelectedIndex = -1 Or DDLfiltroTipoRuolo.SelectedValue = 0 Then
					Me.Bind_IscrittiComunita()
				Else
					Me.Bind_IscrittiComunita(DDLfiltroTipoRuolo.SelectedValue)
				End If
			End If
		End If
	End Sub

	Private Sub Bind_IscrittiComunita(Optional ByVal TPRL_ID As Integer = -1)
		Dim oDataSet As DataSet
		Dim i, Totale As Integer
		Dim oPersona As New COL_Persona
		Dim oComunita As New COL_Comunita

		If IsNothing(Session("objPersona")) Then
			oPersona.ID = 0
		Else
			oPersona = Session("objPersona")
		End If

		Try
			oComunita.Id = Me.ComunitaID

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

			If TPRL_ID = -1 Then
				Dim gruppiEsclusi As String
				If Me.setA_Address Then
					gruppiEsclusi = Me.HDNgruppi_A.Value
				ElseIf Me.setCC_Address Then
					gruppiEsclusi = Me.HDNgruppi_CC.Value
				ElseIf Me.setCCN_Address Then
					gruppiEsclusi = Me.HDNgruppi_CCN.Value
				End If
				oDataSet = oComunita.GetIscrittiEsclusiRuoli(gruppiEsclusi.Trim, Me.ComunitaID, oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, Not (Me.RBLcomunita.SelectedValue = 0), False, oPersona.ID)
			Else
				oDataSet = oComunita.GetIscrittiByRuolo(TPRL_ID, oAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, Not (Me.RBLcomunita.SelectedValue = 0), False, oPersona.ID)
			End If

			Totale = oDataSet.Tables(0).Rows.Count() - 1
			If Totale >= 0 Then
				Dim PRSN_Mail As String
				Dim PRSN_ID As Integer
				LBXelenco.Items.Clear()

				For i = 0 To Totale
					Dim oRow As DataRow
					oRow = oDataSet.Tables(0).Rows(i)
					PRSN_ID = oRow.Item("PRSN_ID")
					If CLng(PRSN_ID) <> oPersona.ID Then
						Dim oListItem As New ListItem

						If IsDBNull(oRow.Item("PRSN_Anagrafica")) Then
							oListItem.Text = "--"
						Else
							oListItem.Text = oRow.Item("PRSN_Anagrafica")
						End If
						PRSN_Mail = oRow.Item("PRSN_Mail")

						If Me.RBLcomunita.SelectedValue = 0 And TPRL_ID = -1 Then
							oListItem.Value = PRSN_ID & "," & oRow.Item("RLPC_TPRL_ID") & "," & PRSN_Mail
						Else
							oListItem.Value = PRSN_ID & "," & TPRL_ID & "," & PRSN_Mail
						End If

						Dim olist As New ListItem

						If Me.setA_Address Then
							olist = Me.LSB_A.Items.FindByValue(oListItem.Value)
							If IsNothing(Me.LSB_A.Items.FindByValue(oListItem.Value)) Then
								LBXelenco.Items.Add(oListItem)
							End If
						ElseIf Me.setCC_Address Then
							olist = Me.LSB_CC.Items.FindByValue(oListItem.Value)
							If IsNothing(Me.LSB_CC.Items.FindByValue(oListItem.Value)) Then
								LBXelenco.Items.Add(oListItem)
							End If
						ElseIf Me.setCCN_Address Then
							olist = Me.LSB_CCN.Items.FindByValue(oListItem.Value)
							If IsNothing(Me.LSB_CCN.Items.FindByValue(oListItem.Value)) Then
								LBXelenco.Items.Add(oListItem)
							End If
						End If

					End If
				Next
			End If
		Catch ex As Exception
			LBXelenco.Items.Clear()
		End Try



	End Sub
#End Region

    Private Sub CBXtutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXtutti.CheckedChanged
        If Me.CBXtutti.Checked Then
            Me.CBLgruppi.SelectedIndex = -1
            Me.CBLgruppi.Enabled = False

            Dim tabGruppi As TabGruppi
            Dim tabSingoli As TabSingoli

        Else
            Me.CBLgruppi.Enabled = True

            Dim tabGruppi As TabGruppi
            Dim tabSingoli As TabSingoli

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

    Private Sub DDLfiltroTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltroTipoRuolo.SelectedIndexChanged
        If DDLfiltroTipoRuolo.SelectedIndex = -1 Or DDLfiltroTipoRuolo.SelectedValue = 0 Then
            Me.Bind_IscrittiComunita()
        Else
            Me.Bind_IscrittiComunita(DDLfiltroTipoRuolo.SelectedValue)
        End If
    End Sub

    Private Sub BTNscelta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNscelta.Click
        Dim i, totale, pos As Integer

        Dim oListBox As ListBox
        If Me.setA_Address Then
            oListBox = Me.LSB_A
        ElseIf Me.setCC_Address Then
            oListBox = Me.LSB_CC
        ElseIf Me.setCCN_Address Then
            oListBox = Me.LSB_CCN
        Else
            oListBox = Me.LSB_A
        End If

        If LBXelenco.SelectedIndex > -1 Then
            totale = Me.LBXelenco.Items.Count - 1
            pos = oListBox.Items.Count
            For i = totale To LBXelenco.SelectedIndex Step -1
                Dim oListItem As New ListItem
                oListItem = Me.LBXelenco.Items(i)
                If oListItem.Selected = True Then
                    oListBox.Items.Insert(pos, oListItem)
                    Me.LBXelenco.Items.RemoveAt(i)
                Else
                    oListItem.Selected = False
                End If
            Next
        End If

    End Sub
    Private Sub BTNrimuovi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNrimuovi.Click
        Dim i, totale, pos As Integer

        Dim oListBox As ListBox
        If Me.setA_Address Then
            oListBox = Me.LSB_A
        ElseIf Me.setCC_Address Then
            oListBox = Me.LSB_CC
        ElseIf Me.setCCN_Address Then
            oListBox = Me.LSB_CCN
        Else
            oListBox = Me.LSB_A
        End If

        If Me.DDLfiltroTipoRuolo.SelectedValue > 0 Then
            totale = oListBox.Items.Count - 1
            pos = LBXelenco.Items.Count
            For i = totale To oListBox.SelectedIndex Step -1
                Dim oListItem As New ListItem
                oListItem = oListBox.Items(i)

                If oListItem.Selected = True Then
                    oListItem.Selected = False
                    If CInt(Me.DDLfiltroTipoRuolo.SelectedValue) = Me.getTipo_Ruolo(oListItem.Value) Then
                        LBXelenco.Items.Insert(pos, oListItem)
                    End If

                    oListBox.Items.RemoveAt(i)
                End If
            Next
        Else
            totale = oListBox.Items.Count - 1
            pos = LBXelenco.Items.Count
            For i = totale To 0 Step -1
                Dim oListItem As New ListItem
                oListItem = oListBox.Items(i)
                If oListItem.Selected = True Then
                    oListItem.Selected = False
                    If IsNothing(Me.LBXelenco.Items.FindByValue(oListItem.Value)) Then
                        LBXelenco.Items.Insert(pos, oListItem)
                    End If
                    oListBox.Items.RemoveAt(i)
                End If
            Next
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
        Dim ElencoSingoli As String = ""

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

            Dim oListBox As ListBox
            Select Case oTipo
                Case TipoDestinatarioMail.A
                    oListBox = Me.LSB_A
                Case TipoDestinatarioMail.CC
                    oListBox = Me.LSB_CC
                Case TipoDestinatarioMail.CCN
                    oListBox = Me.LSB_CCN
                Case Else
                    oListBox = Me.LSB_A
            End Select

            totale = oListBox.Items.Count - 1
            Dim nomeDestinatario, id_Destinatario, mail_Destinatario As String
            For i = 0 To totale
                Dim oListItem As New ListItem
                oListItem = oListBox.Items(i)

                nomeDestinatario = oListItem.Text
                id_Destinatario = Me.getPRSN_ID(oListItem.Value)
                mail_Destinatario = Me.getMail(oListItem.Value)

                ElencoContatti.Add(New COL_Contatto(id_Destinatario, nomeDestinatario, mail_Destinatario, -1000, oTipo))
            Next
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
            Dim oListBox As ListBox
            Select Case filtra
                Case TipoDestinatarioMail.A
                    oListBox = Me.LSB_A
                Case TipoDestinatarioMail.CC
                    oListBox = Me.LSB_CC
                Case TipoDestinatarioMail.CCN
                    oListBox = Me.LSB_CCN
                Case Else
                    oListBox = Me.LSB_A
            End Select

            totale = oListBox.Items.Count - 1
            For i = 0 To totale
                Dim oListItem As New ListItem
                oListItem = oListBox.Items(i)

                response = response & oListItem.Text & ";"

                If response.Length > 400 Then
                    response = response & " .... "
                    Exit For
                End If
            Next
        End If

        Return response
    End Function

    Public Function ResetForm()
        Dim oListBox As ListBox

        If Me.setA_Address Then
            oListBox = Me.LSB_A
        ElseIf Me.setCC_Address Then
            oListBox = Me.LSB_CC
        ElseIf Me.setCCN_Address Then
            oListBox = Me.LSB_CCN
        Else
            oListBox = Me.LSB_A
        End If
        oListBox.Items.Clear()

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

        'Dim tabGruppi As TabGruppi
        'Dim tabSingoli As TabSingoli
        'Me.IMBsingoli.ImageUrl = oResource.setStringaTab(IMBsingoli, tabSingoli.Singoli)
        'Me.IMBgruppi.ImageUrl = oResource.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
        Me.TBSmenu.SelectedIndex = 0

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


        Me.DDLfiltro.Enabled = True

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
        Me.TBSmenu.SelectedIndex = 0

        If Me.setA_Address Then
            If Me.HDNgruppiTutti_A.Value = "True" Then
                Me.CBXtutti.Checked = True
                Me.CBLgruppi.Enabled = False
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True
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
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True
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
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True
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
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True
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

    Private Function SetupJavascript()
        Try
            'aggiunge ai link button le proprietà da visualizzare nella barra di stato
            Dim i As Integer
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
    End Function

#Region "Gestione Pannelli"

    'Private Sub IMBgruppi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBgruppi.Click
    '    Me.ShowGruppi()
    'End Sub
    'Private Sub IMBsingoli_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBsingoli.Click
    '    Me.ShowSingoli()
    'End Sub
    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Me.HDN_PanView.Value = Me.TBSmenu.SelectedIndex
        Select Case Me.TBSmenu.SelectedIndex
            Case 0 'Gruppi
                Me.ShowGruppi()
            Case 1 'Singoli
                Me.ShowSingoli()
        End Select
    End Sub
    Private Sub ShowGruppi()
        If Me.PNLgruppi.Visible = False Then
            Me.PNLgruppi.Visible = True
            Me.PNLsingoli.Visible = False
            Dim tabGruppi As TabGruppi
            Dim tabSingoli As TabSingoli
            'Me.IMBsingoli.ImageUrl = oResource.setStringaTab(IMBsingoli, tabSingoli.Singoli)
            'Me.IMBgruppi.ImageUrl = oResource.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
        End If
    End Sub
    Private Sub ShowSingoli()
        If Me.PNLsingoli.Visible = False Then
            Me.PNLsingoli.Visible = True
            Me.PNLgruppi.Visible = False

            Dim i As Integer
            Try
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
                Me.Bind_TipoRuolo()

            Catch ex As Exception

            End Try

            Dim tabGruppi As TabGruppi
            Dim tabSingoli As TabSingoli
            'Me.IMBsingoli.ImageUrl = oResource.setStringaTab(IMBsingoli, tabSingoli.singoliSelezionato)
            'Me.IMBgruppi.ImageUrl = oResource.setStringaTab(IMBgruppi, tabGruppi.gruppi)
        End If
    End Sub

#End Region

    
End Class

'Vecchi colori
'00ffcc - Gruppi
'ffffc0 - Singoli
'ffffe0 - Singoli2