Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2ServiziBase.ContattiMail


Public Class UC_RubricaMailGlobale
    Inherits System.Web.UI.UserControl
    'Public oLocate As COL_Localizzazione
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
    Protected WithEvents HDN_setA As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_setCCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_setCC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppi_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiNome_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiTutti_A As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLabilitazioneA As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppi_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiNome_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiTutti_CC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLabilitazioneCC As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppi_CCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiNome_CCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNgruppiTutti_CCN As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RBLabilitazioneCCN As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Public Property"
    Public Property setA_Address() As Boolean
        Get
            setA_Address = (Me.HDN_setA.Value = "1")
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

    Protected WithEvents LBselezionaFacolta As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
#Region "Pannelli"
    Protected WithEvents LBselezionaDest As System.Web.UI.WebControls.Label
    Protected WithEvents RBLabilitazione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBseleziona As System.Web.UI.WebControls.Label
    'Protected WithEvents IMBgruppi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents IMBsingoli As System.Web.UI.WebControls.ImageButton
    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
    Protected WithEvents HDN_PanView As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents CBXtutti As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBLgruppi As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents PNLgruppi As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLfiltroTipoPersona As System.Web.UI.WebControls.DropDownList
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
        If IsNothing(Me.oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Page.IsPostBack = False Then
            Try
                SetupInternazionalizzazione()
            Catch exUserLanguages As Exception
            End Try
        End If

        If Session("objPersona") Is Nothing Then 'se la sessione è scaduta redirecto alla home
            Response.Redirect("./../../index.aspx")
        End If
    End Sub

    Private Sub StartupData()
        Try

            'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, TabSingoli.Singoli)
            'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, TabGruppi.gruppiSelezionato)
            Me.Bind_Dati()
        Catch ex As Exception

        End Try
        Me.SetStartupScript()
    End Sub
#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        ' localizzazione utente
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_RubricaMailGlobale"
        oResource.Folder_Level1 = "Admin_globale"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub

    Public Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(LBselezionaDest)
            .setLabel(LBfiltraTipo)
            .setLabel(LBseleziona)

            .setRadioButtonList(Me.RBLabilitazione, "1")
            .setRadioButtonList(Me.RBLabilitazione, "5")
            .setRadioButtonList(Me.RBLabilitazione, "4")
            .setRadioButtonList(Me.RBLabilitazione, "0")
            .setDropDownList(DDLfiltro, -1)
            .setDropDownList(DDLfiltro, 0)
            .setCheckBox(CBXtutti)
            .setButton(BTNfind)

            TBSmenu.Tabs(0).Text = .getValue("RTABGruppi.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("RTABGruppi.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("RTABSingoli.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("RTABSingoli.ToolTip")
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
        '   BTNscelta.Attributes.Add("onclick", "destinatariToSMS();return false;")
        BTNfind.Attributes.Add("onclick", "findDestinatari();return false;")
        Me.BTNscelta.Attributes.Add("onclick", "return hasSelezionati();")
        '    BTNrimuovi.Attributes.Add("onclick", "destinatariToLista();return false;")
    End Sub

#Region "Gestione Selezione"
    Private Sub RBLabilitazioneSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLabilitazione.SelectedIndexChanged
        Dim selezionati As String
        selezionati = Me.GetGruppiSelezionati()
        Me.BindGruppi()

        Dim totale As Integer
        Dim oOrganizzazione As New COL_Organizzazione
        oOrganizzazione.Id = Me.DDLorganizzazione.SelectedValue
        totale = oOrganizzazione.GetTotaleIscritti(Me.RBLabilitazione.SelectedValue, Session("objPersona").Id)

        Me.CBXtutti.Text = oResource.getValue("tutti") & " (" & totale & ")"

        If Me.CBXtutti.Checked = False Then
            Me.SetSelezionati(selezionati)
        End If
        If Me.setA_Address Then
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
        ElseIf Me.setCC_Address Then
            Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
        ElseIf Me.setCCN_Address Then
            Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
        Else
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
        End If

        Me.Bind_IscrittiOrganizzazione()

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
        Dim i, totale, TPPR_ID As Integer
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
                    TPPR_ID = elencoID(i)
                    oListItem = Me.CBLgruppi.Items.FindByValue(TPPR_ID)
                    If Not (IsNothing(oListItem)) Then
                        oListItem.Selected = True

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
                End If
            Next
        End If
    End Sub

#End Region


#Region "Metodi Standard"
    Public Sub Bind_Dati()
        Dim Totale As Integer
        Dim oPersona As New COL_Persona
        Dim oOrganizzazione As New COL_Organizzazione

        Try
            oPersona = Session("objPersona")

            Me.Bind_Organizzazione()

            oOrganizzazione.Id = Me.DDLorganizzazione.SelectedValue
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

            Totale = oOrganizzazione.GetTotaleIscritti(oAbilitazione, oPersona.Id)

            If IsNothing(oResource) Then
                SetCulture(Session("LinguaCode"))
            End If
            Me.CBXtutti.Text = oResource.getValue("tutti") & " (" & Totale & ")"
            Me.BindGruppi()
        Catch ex As Exception

        End Try
        Me.SetStartupScript()
    End Sub

    Private Sub Bind_Organizzazione()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLorganizzazione.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLorganizzazione.DataValueField = "ORGN_id"
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()

                If Me.DDLorganizzazione.Items.Count > 1 Then
                    Me.DDLorganizzazione.Enabled = True
                    Me.DDLorganizzazione.Items.Insert(0, New ListItem("< tutte >", -1))
                    Me.DDLorganizzazione.SelectedIndex = 0
                Else
                    Me.DDLorganizzazione.Enabled = False
                End If
            Else
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazione.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Clear()
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazione.Enabled = False
        End Try
        oResource.setDropDownList(Me.DDLorganizzazione, 0)
        oResource.setDropDownList(Me.DDLorganizzazione, -1)
    End Sub

    Private Sub BindGruppi()
        Dim oOrganizzazione As New COL_Organizzazione
        Dim oPersona As New COL_Persona
        Dim oDataset As DataSet
        Dim Totale As Integer

        Try
            oOrganizzazione.Id = Me.DDLorganizzazione.SelectedValue
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
            oDataset = oOrganizzazione.GetTipiPersonaAssociati(Session("LinguaID"), oPersona.Id, oAbilitazione)

            If oDataset.HasErrors = False Then
                If oDataset.Tables.Count > 0 Then
                    If oDataset.Tables(0).Rows.Count > 1 Then
                        Me.CBLgruppi.DataSource = oDataset
                        Me.CBLgruppi.DataTextField = "TPPR_DataTextField"
                        Me.CBLgruppi.DataValueField = "TPPR_ID"
                        Me.CBLgruppi.DataBind()
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoPersona()
        Dim dstable As New DataSet
        Dim i, Totale, TPPR_ID As Integer
        Dim oOrganizzazione As New COL_Organizzazione
        Dim oPersona As New COL_Persona


        Try
            oPersona = Session("objPersona")
            oOrganizzazione.Id = Me.DDLorganizzazione.SelectedValue

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


            dstable = oOrganizzazione.GetTipiPersonaAssociati(Session("LinguaID"), oPersona.Id, oAbilitazione)

        Catch ex As Exception

        End Try

        If Not (dstable.HasErrors) Then
            DDLfiltro.Items.Clear()
            DDLfiltro.Items.Add(New ListItem("Nessuno", -1))
            DDLfiltroTipoPersona.Items.Clear()
            DDLfiltroTipoPersona.Items.Add(New ListItem("Tutti", 0))

            If dstable.Tables.Count > 0 Then
                Totale = dstable.Tables(0).Rows.Count() - 1
                Dim stringaGruppi As String


                For i = 0 To Totale
                    Dim oRow As DataRow
                    oRow = dstable.Tables(0).Rows(i)
                    TPPR_ID = oRow.Item("TPPR_ID")

                    If Me.setA_Address Then
                        stringaGruppi = Me.HDNgruppi_A.Value
                    ElseIf Me.setCC_Address Then
                        stringaGruppi = Me.HDNgruppi_CC.Value
                    ElseIf Me.setCCN_Address Then
                        stringaGruppi = Me.HDNgruppi_CCN.Value
                    End If
                    If InStr(stringaGruppi, "," & TPPR_ID & ",") = 0 Then
						Dim oListItem As New ListItem
						If IsDBNull(oRow.Item("TPPR_descrizione")) Then
							oListItem.Text = "--"
						Else
							oListItem.Text = oRow.Item("TPPR_descrizione")
						End If
						oListItem.Value = TPPR_ID
						DDLfiltro.Items.Add(oListItem)
						DDLfiltroTipoPersona.Items.Add(oListItem)
						oListItem = Nothing
					End If
				Next
				If DDLfiltroTipoPersona.SelectedIndex = -1 Or DDLfiltroTipoPersona.SelectedValue = 0 Then
					Me.Bind_IscrittiOrganizzazione()
				Else
					Me.Bind_IscrittiOrganizzazione(DDLfiltroTipoPersona.SelectedValue)
				End If
			End If
		End If
	End Sub

	Private Sub Bind_IscrittiOrganizzazione(Optional ByVal TPPR_ID As Integer = -1)
		Dim oDataSet As DataSet
		Dim i, Totale As Integer
		Dim oPersona As New COL_Persona
		Dim oOrganizzazione As New COL_Organizzazione

		If IsNothing(Session("objPersona")) Then
			oPersona.ID = 0
		Else
			oPersona = Session("objPersona")
		End If

		Try
			oOrganizzazione.Id = Me.DDLorganizzazione.SelectedValue

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

			If TPPR_ID = -1 Then
				Dim gruppiEsclusi As String
				If Me.setA_Address Then
					gruppiEsclusi = Me.HDNgruppi_A.Value
				ElseIf Me.setCC_Address Then
					gruppiEsclusi = Me.HDNgruppi_CC.Value = ""
				ElseIf Me.setCCN_Address Then
					gruppiEsclusi = Me.HDNgruppi_CCN.Value = ""
				End If
				oDataSet = oOrganizzazione.GetIscrittiEsclusiTipoPersona(gruppiEsclusi.Trim, Me.DDLorganizzazione.SelectedValue, oAbilitazione, oPersona.ID)
			Else
				oDataSet = oOrganizzazione.GetIscrittiByTipoPersona(Me.DDLorganizzazione.SelectedValue, TPPR_ID, oAbilitazione, oPersona.ID)
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


						oListItem.Value = PRSN_ID & "," & TPPR_ID & "," & PRSN_Mail


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
            'Me.IMBsingoli.Enabled = False
            'Dim tabGruppi As TabGruppi
            'Dim tabSingoli As TabSingoli
            Me.TBSmenu.SelectedIndex = 0
            Me.TBSmenu.Tabs(1).Enabled = False
            'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.singoliDisattivato)
            'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiDisattivato)
        Else
            Me.CBLgruppi.Enabled = True
            'Me.IMBsingoli.Enabled = True
            'Dim tabGruppi As TabGruppi
            'Dim tabSingoli As TabSingoli
            Me.TBSmenu.SelectedIndex = 0
            Me.TBSmenu.Tabs(1).Enabled = True

            'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.Singoli)
            'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
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
        Me.HDN_PanView.Value = Me.TBSmenu.SelectedIndex
        Select Case Me.TBSmenu.SelectedIndex
            Case 0 'Gruppi
                Me.ShowGruppi()
            Case 1 'Singoli
                Me.ShowSingoli()
        End Select
    End Sub

    Private Sub ShowGruppi()
        Me.PNLgruppi.Visible = True
        Me.PNLsingoli.Visible = False
        'Dim tabGruppi As TabGruppi
        'Dim tabSingoli As TabSingoli
        'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.Singoli)
        'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
    End Sub
    'Private Sub IMBgruppi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBgruppi.Click
    '    If Me.PNLgruppi.Visible = False Then

    '    End If
    'End Sub
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
                Me.Bind_TipoPersona()

            Catch ex As Exception

            End Try

            'Dim tabGruppi As TabGruppi
            'Dim tabSingoli As TabSingoli
            'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.singoliSelezionato)
            'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppi)
        End If
    End Sub
    'Private Sub IMBsingoli_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBsingoli.Click

    'End Sub

    Private Sub DDLfiltroTipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltroTipoPersona.SelectedIndexChanged
        If DDLfiltroTipoPersona.SelectedIndex = -1 Or DDLfiltroTipoPersona.SelectedValue = 0 Then
            Me.Bind_IscrittiOrganizzazione()
        Else
            Me.Bind_IscrittiOrganizzazione(DDLfiltroTipoPersona.SelectedValue)
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

        If Me.DDLfiltroTipoPersona.SelectedValue > 0 Then
            totale = oListBox.Items.Count - 1
            pos = LBXelenco.Items.Count
            For i = totale To oListBox.SelectedIndex Step -1
                Dim oListItem As New ListItem
                oListItem = oListBox.Items(i)

                If oListItem.Selected = True Then
                    oListItem.Selected = False
                    If CInt(Me.DDLfiltroTipoPersona.SelectedValue) = Me.getTipo_Ruolo(oListItem.Value) Then
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
    'riga = PRSN_ID,TPPR_ID,Mail
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
        Dim iResponseFinale(4) As String

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
            Dim nomeDestinatario, id_Destinatario, mail_Destinatario, quote As String
            quote = """"
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

        Dim oAbilitazione As Main.FiltroAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
        Dim elencoGruppi() As String

        Select Case oTipo
            Case TipoDestinatarioMail.A
                selectAll = (Me.HDNgruppiTutti_A.Value = "True")
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
        Dim oOrganizzazione As New COL_Organizzazione
        Try
            oOrganizzazione.Id = CInt(Me.DDLorganizzazione.SelectedValue)
        Catch ex As Exception
            oOrganizzazione.Id = 0
        End Try

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

                        oDataset = oOrganizzazione.GetIscrittiByTipoPersona(Me.DDLorganizzazione.SelectedValue, RuoloID, oAbilitazione, Session("objPersona").id)
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
        Me.TBSmenu.SelectedIndex = 0
        'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.Singoli)
        'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)

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


        'Dim tabGruppi As TabGruppi
        'Dim tabSingoli As TabSingoli

        If Me.setA_Address Then
            If Me.HDNgruppiTutti_A.Value = "True" Then
                Me.CBXtutti.Checked = True
                Me.CBLgruppi.Enabled = False
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.singoliDisattivato)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
                Me.TBSmenu.SelectedIndex = 0
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True

                Me.TBSmenu.SelectedIndex = 0
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.Singoli)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneA.Value
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
                Me.TBSmenu.Tabs(1).Enabled = False
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.singoliDisattivato)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True
                Me.TBSmenu.SelectedIndex = 0
                Me.TBSmenu.Tabs(1).Enabled = True
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.Singoli)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneCC.Value
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
                Me.TBSmenu.Tabs(1).Enabled = False
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.singoliDisattivato)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True
                Me.TBSmenu.SelectedIndex = 0
                Me.TBSmenu.Tabs(1).Enabled = True
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.Singoli)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneCCN.Value
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

                Me.TBSmenu.Tabs(1).Enabled = False
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.singoliDisattivato)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
            Else
                Me.CBXtutti.Checked = False
                Me.CBLgruppi.Enabled = True
                Me.TBSmenu.SelectedIndex = 0

                Me.TBSmenu.Tabs(1).Enabled = True
                'Me.IMBsingoli.ImageUrl = oLocate.setStringaTab(IMBsingoli, tabSingoli.Singoli)
                'Me.IMBgruppi.ImageUrl = oLocate.setStringaTab(IMBgruppi, tabGruppi.gruppiSelezionato)
            End If

            Try
                Me.RBLabilitazione.SelectedValue = Me.HDN_RBLabilitazioneA.Value
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


    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Dim selezionati As String
        selezionati = Me.GetGruppiSelezionati()
        Me.BindGruppi()

        Dim totale As Integer
        Dim oOrganizzazione As New COL_Organizzazione
        oOrganizzazione.Id = Me.DDLorganizzazione.SelectedValue
        totale = oOrganizzazione.GetTotaleIscritti(Me.RBLabilitazione.SelectedValue, Session("objPersona").Id)

        Me.CBXtutti.Text = oResource.getValue("tutti") & " (" & totale & ")"

        If Me.CBXtutti.Checked = False Then
            Me.SetSelezionati(selezionati)
        End If
        If Me.setA_Address Then
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
        ElseIf Me.setCC_Address Then
            Me.HDN_RBLabilitazioneCC.Value = Me.RBLabilitazione.SelectedValue
        ElseIf Me.setCCN_Address Then
            Me.HDN_RBLabilitazioneCCN.Value = Me.RBLabilitazione.SelectedValue
        Else
            Me.HDN_RBLabilitazioneA.Value = Me.RBLabilitazione.SelectedValue
        End If

        Me.Bind_IscrittiOrganizzazione()
    End Sub

    Private Sub DDLfiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltro.SelectedIndexChanged

    End Sub

End Class