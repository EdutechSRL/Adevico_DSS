Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo


Imports iTextSharp

Public Class UC_DatiCurriculum
    Inherits System.Web.UI.UserControl
    Protected oResourceDati As ResourceManager

#Region "dati Label"
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcognome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdatanascita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBsesso_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBindirizzo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcap_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcitta_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtelefono_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcellulare_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBfax_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmail_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnazionalita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmadrelingua_t As System.Web.UI.WebControls.Label

    Protected WithEvents LBpatente_t As System.Web.UI.WebControls.Label

    Protected WithEvents HDNPatente As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNMadrelingua As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents LBrendiPubblico_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmostraDatiSensibili_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmostraRecapiti_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents LBnote As System.Web.UI.WebControls.Label
#End Region
#Region "accessori"
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents TBRmessaggio As System.Web.UI.WebControls.TableRow
    'Protected WithEvents BTNaggiungi As System.Web.UI.WebControls.Button
    ' Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcrev_id As System.Web.UI.HtmlControls.HtmlInputHidden

#End Region

#Region "dati textbox"
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcognome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBdataNascita As System.Web.UI.WebControls.TextBox
    Protected WithEvents RBLSesso As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TXBindirizzo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcap As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcitta As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBtelefono As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcellulare As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBfax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBnazionalita As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBmadrelingua As System.Web.UI.WebControls.TextBox

    Protected WithEvents TXBpatente As System.Web.UI.WebControls.TextBox

    Protected WithEvents CBXrendiPubblico As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBXmostraDatiSensibili As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBXmostraRecapiti As System.Web.UI.WebControls.CheckBox

#End Region

    Public Event AggiornaDati(ByVal sender As Object, ByVal e As EventArgs)
    Public Event NascondiTab(ByVal sender As Object, ByVal e As EventArgs)

	Public Property CurriculumID() As Integer
		Get
			If Me.HDNcrev_id.Value = "" Then
				Me.HDNcrev_id.Value = 0
			End If
			CurriculumID = Me.HDNprsn_id.Value
		End Get
		Set(ByVal Value As Integer)
			Me.HDNcrev_id.Value = Value
		End Set
	End Property



    Public Property PRSN_ID() As Integer
        Get
            If Me.HDNprsn_id.Value = "" Then
                Me.HDNprsn_id.Value = 0
            End If
            PRSN_ID = Me.HDNprsn_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNprsn_id.Value = Value
        End Set
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
        If IsNothing(oResourceDati) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            If Page.IsPostBack = False Then
                Me.TBRmessaggio.Visible = False
                Me.SetupInternazionalizzazione()
                Dim PRSN_id As Integer
                PRSN_id = Me.HDNprsn_id.Value
                Dim oCurriculum As New COL_CurriculumEuropeo
                oCurriculum.EstraiByPersona(PRSN_id)
                If oCurriculum.ID = -1 Then
                    '   Me.BTNaggiungi.Visible = True
                    '   Me.BTNmodifica.Visible = False
                    Bind_precompilato()
                    RaiseEvent NascondiTab(Me, EventArgs.Empty)
                Else
                    '  Me.BTNaggiungi.Visible = False
                    '  Me.BTNmodifica.Visible = True
                    Bind_Dati(oCurriculum)
                End If

            End If

        Catch ex As Exception

        End Try



    End Sub
    Public Sub start()
        Me.TBRmessaggio.Visible = False
        If IsNothing(oResourceDati) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            Dim PRSN_id As Integer
            PRSN_id = Me.HDNprsn_id.Value
            Dim oCurriculum As New COL_CurriculumEuropeo
            oCurriculum.EstraiByPersona(PRSN_id)
            If oCurriculum.ID = -1 Then
                Bind_precompilato()
                RaiseEvent NascondiTab(Me, EventArgs.Empty)
            Else
                Bind_Dati(oCurriculum)
            End If
        Catch ex As Exception

        End Try
    End Sub

#Region "Bind_Dati"
    Public Function Bind_Dati(ByVal oCurriculum As COL_CurriculumEuropeo)
        Try
            HDNcrev_id.Value = oCurriculum.ID
            Me.TXBnome.Text = oCurriculum.Nome
            Me.TXBcognome.Text = oCurriculum.Cognome
            Me.TXBdataNascita.Text = oCurriculum.DataNascita
            If oCurriculum.Sesso = 1 Then
                Me.RBLSesso.SelectedValue = 1
            ElseIf oCurriculum.Sesso = 0 Then
                Me.RBLSesso.SelectedValue = 0
            End If
            Me.TXBindirizzo.Text = oCurriculum.Indirizzo
            Me.TXBcap.Text = oCurriculum.Cap
            Me.TXBcitta.Text = oCurriculum.Citta
            Me.TXBtelefono.Text = oCurriculum.Telefono
            Me.TXBcellulare.Text = oCurriculum.Cellulare
            Me.TXBfax.Text = oCurriculum.Fax
            Me.TXBmail.Text = oCurriculum.Mail
            Me.TXBnazionalita.Text = oCurriculum.Nazionalita
            Me.TXBmadrelingua.Text = oCurriculum.Madrelingua
            'Me.TXBcompetenzeRelazionali.Text = oCurriculum.CompetenzeRelazionali
            'Me.TXBcompetenzeOrganizzative.Text = oCurriculum.CompetenzeOrganizzative
            'Me.TXBcompetenzeTecniche.Text = oCurriculum.CompetenzeTecniche
            'Me.TXBcompetenzeArtistiche.Text = oCurriculum.CompetenzeArtistiche
            'Me.TXBaltreCompetenze.Text = oCurriculum.AltreCompetenze
            Me.TXBpatente.Text = oCurriculum.Patente
            ' Me.TXBulterioriInfo.Text = oCurriculum.UlterioriInfo
            If oCurriculum.RendiPubblico = False Then
                Me.CBXrendiPubblico.Checked = False
            Else
                Me.CBXrendiPubblico.Checked = True
            End If
            If oCurriculum.MostraDatiSensibili = False Then
                Me.CBXmostraDatiSensibili.Checked = False
            Else
                Me.CBXmostraDatiSensibili.Checked = True
            End If
            If oCurriculum.MostraRecapiti = False Then
                Me.CBXmostraRecapiti.Checked = False
            Else
                Me.CBXmostraRecapiti.Checked = True
            End If


        Catch ex As Exception

        End Try
    End Function
    Public Function Bind_precompilato()
        Dim oPersona As New COL_Persona
        oPersona.Id = Me.HDNprsn_id.Value
        oPersona.EstraiTutto(Session("LinguaID"))
        Try

            Me.TXBnome.Text = oPersona.Nome
            Me.TXBcognome.Text = oPersona.Cognome
            Me.TXBdataNascita.Text = Format(CDate(oPersona.DataNascita), "dd/MM/yyyy")
            If oPersona.Sesso = 1 Then
                Me.RBLSesso.SelectedValue = 1
            ElseIf oPersona.Sesso = 0 Then
                Me.RBLSesso.SelectedValue = 0
            End If
            Me.TXBindirizzo.Text = oPersona.Indirizzo
            Me.TXBcap.Text = oPersona.Cap
            Me.TXBcitta.Text = oPersona.Citta
            Me.TXBtelefono.Text = oPersona.Telefono1
            Me.TXBcellulare.Text = oPersona.Cellulare
            Me.TXBfax.Text = oPersona.Fax
            Me.TXBmail.Text = oPersona.Mail
        Catch ex As Exception

        End Try

    End Function

    'Private Sub Setup_starupScript()
    '    Me.TXBcompetenzeRelazionali.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
    '    Me.TXBcompetenzeOrganizzative.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
    '    Me.TXBcompetenzeTecniche.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
    '    Me.TXBcompetenzeArtistiche.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
    '    Me.TXBaltreCompetenze.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
    '    Me.TXBulterioriInfo.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
    'End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceDati = New ResourceManager

        oResourceDati.UserLanguages = Code
        oResourceDati.ResourcesName = "pg_UC_DatiCurriculum"
        oResourceDati.Folder_Level1 = "Curriculum"
        oResourceDati.Folder_Level2 = "UC"
        oResourceDati.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceDati
            .setLabel(LBnome_t)
            .setLabel(LBrendiPubblico_t)
            .setLabel(LBnote)
            .setLabel(LBmostraDatiSensibili_t)
            .setLabel(LBmostraRecapiti_t)
            .setLabel(LBcognome_t)
            .setLabel(LBdatanascita_t)
            .setLabel(LBsesso_t)
            .setRadioButtonList(Me.RBLSesso, "0")
            .setRadioButtonList(Me.RBLSesso, "1")
            .setLabel(LBindirizzo_t)
            .setLabel(LBcap_t)
            .setLabel(LBcitta_t)
            .setLabel(LBtelefono_t)
            .setLabel(LBcellulare_t)
            .setLabel(LBfax_t)
            .setLabel(LBmail_t)
            .setLabel(LBnazionalita_t)
            .setLabel(LBmadrelingua_t)
            .setLabel(LBpatente_t)
        End With
    End Sub
#End Region

    Public Sub AggiungiCurriculum()
        Dim oCurriculum As New COL_CurriculumEuropeo

        Try
            With oCurriculum
                .PRSN_ID = Me.HDNprsn_id.Value
                .Nome = Me.TXBnome.Text
                .Cognome = Me.TXBcognome.Text
                .DataNascita = Me.TXBdataNascita.Text
                .Sesso = Me.RBLSesso.SelectedValue
                .Indirizzo = Me.TXBindirizzo.Text
                .Cap = Me.TXBcap.Text
                .Citta = Me.TXBcitta.Text
                .Telefono = Me.TXBtelefono.Text
                .Cellulare = Me.TXBcellulare.Text
                .Fax = Me.TXBfax.Text
                .Mail = Me.TXBmail.Text
                .Nazionalita = Me.TXBnazionalita.Text
                .Madrelingua = Me.TXBmadrelingua.Text
                '.CompetenzeRelazionali = Me.TXBcompetenzeRelazionali.Text
                '.CompetenzeOrganizzative = Me.TXBcompetenzeOrganizzative.Text
                '.CompetenzeTecniche = Me.TXBcompetenzeTecniche.Text
                '.CompetenzeArtistiche = Me.TXBcompetenzeArtistiche.Text
                '.AltreCompetenze = Me.TXBaltreCompetenze.Text
                .Patente = Me.TXBpatente.Text
                ' .UlterioriInfo = Me.TXBulterioriInfo.Text
                .RendiPubblico = Me.CBXrendiPubblico.Checked
                .MostraDatiSensibili = Me.CBXmostraDatiSensibili.Checked
                .MostraRecapiti = Me.CBXmostraRecapiti.Checked
            End With
            oCurriculum.Aggiungi()

            If oCurriculum.Errore = Errori_Db.None Then
                Me.HDNcrev_id.Value = oCurriculum.ID
            End If
        Catch ex As Exception
            Me.TBRmessaggio.Visible = True
            '  Me.LBmessaggio.Text = "Spiacenti, è avvenuto un errore"
            oResourceDati.setLabel_To_Value(LBmessaggio, "errore2")
        End Try
        Me.TBRmessaggio.Visible = True
        ' Me.LBmessaggio.Text = "Inserimento avvenuto correttamente"
        oResourceDati.setLabel_To_Value(LBmessaggio, "errore1")
        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
    End Sub


    Public Sub ModificaCurriculum()
        Dim oCurriculum As New COL_CurriculumEuropeo
        Try
            With oCurriculum
                .ID = HDNcrev_id.Value
                .PRSN_ID = Me.HDNprsn_id.Value
                .Nome = Me.TXBnome.Text
                .Cognome = Me.TXBcognome.Text
                .DataNascita = Me.TXBdataNascita.Text
                .Sesso = Me.RBLSesso.SelectedValue
                .Indirizzo = Me.TXBindirizzo.Text
                .Cap = Me.TXBcap.Text
                .Citta = Me.TXBcitta.Text
                .Telefono = Me.TXBtelefono.Text
                .Cellulare = Me.TXBcellulare.Text
                .Fax = Me.TXBfax.Text
                .Mail = Me.TXBmail.Text
                .Nazionalita = Me.TXBnazionalita.Text
                .Madrelingua = Me.TXBmadrelingua.Text
                '.CompetenzeRelazionali = Me.TXBcompetenzeRelazionali.Text
                '.CompetenzeOrganizzative = Me.TXBcompetenzeOrganizzative.Text
                '.CompetenzeTecniche = Me.TXBcompetenzeTecniche.Text
                '.CompetenzeArtistiche = Me.TXBcompetenzeArtistiche.Text
                '.AltreCompetenze = Me.TXBaltreCompetenze.Text
                .Patente = Me.TXBpatente.Text
                '.UlterioriInfo = Me.TXBulterioriInfo.Text
                .RendiPubblico = Me.CBXrendiPubblico.Checked
                .MostraDatiSensibili = Me.CBXmostraDatiSensibili.Checked
                .MostraRecapiti = Me.CBXmostraRecapiti.Checked
            End With
            oCurriculum.Modifica()

        Catch ex As Exception
            Me.TBRmessaggio.Visible = True
            'Me.LBmessaggio.Text = "è avvenuto un errore"
            oResourceDati.setLabel_To_Value(LBmessaggio, "errore2")
        End Try
        Me.TBRmessaggio.Visible = True
        ' Me.LBmessaggio.Text = "Modifica avvenuta correttamente"
        oResourceDati.setLabel_To_Value(LBmessaggio, "errore3")
    End Sub
    
    Public Function GetPatente() As String
        Return Me.HDNPatente.Value
    End Function

    Public Function Getmadrelingua() As String
        Return Me.HDNMadrelingua.Value
    End Function

End Class
