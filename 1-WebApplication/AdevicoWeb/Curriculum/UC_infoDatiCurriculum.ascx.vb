Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo


Public Class UC_infoDatiCurriculum
    Inherits System.Web.UI.UserControl

    Protected oResourceDati As ResourceManager

#Region "dati Label"
    Protected WithEvents LBnome_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBcognome_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBdatanascita_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBsesso_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBindirizzo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcap_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcitta_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtelefono_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcellulare_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBfax_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmail_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBnazionalita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmadrelingua_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBpatente_t As System.Web.UI.WebControls.Label

    'Protected WithEvents LBrendiPubblico_t As System.Web.UI.WebControls.Label
    'Protected WithEvents LBmostraDatiSensibili_t As System.Web.UI.WebControls.Label
    'Protected WithEvents LBmostraRecapiti_t As System.Web.UI.WebControls.Label
    'Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
    'Protected WithEvents LBnote As System.Web.UI.WebControls.Label
#End Region
#Region "accessori"
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents TBRmessaggio As System.Web.UI.WebControls.TableRow
    'Protected WithEvents BTNaggiungi As System.Web.UI.WebControls.Button
    ' Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcrev_id As System.Web.UI.HtmlControls.HtmlInputHidden

#End Region

#Region "dati LABEL e table"
    Protected WithEvents LBnome As System.Web.UI.WebControls.Label
    Protected WithEvents LBcognome As System.Web.UI.WebControls.Label
    Protected WithEvents LBdataNascita As System.Web.UI.WebControls.Label
    Protected WithEvents LBSesso As System.Web.UI.WebControls.Label
    Protected WithEvents LBindirizzo As System.Web.UI.WebControls.Label
    Protected WithEvents LBcap As System.Web.UI.WebControls.Label
    Protected WithEvents LBcitta As System.Web.UI.WebControls.Label
    Protected WithEvents LBtelefono As System.Web.UI.WebControls.Label
    Protected WithEvents LBcellulare As System.Web.UI.WebControls.Label
    Protected WithEvents LBfax As System.Web.UI.WebControls.Label
    Protected WithEvents LBmail As System.Web.UI.WebControls.Label
    Protected WithEvents LBnazionalita As System.Web.UI.WebControls.Label
    Protected WithEvents LBmadrelingua As System.Web.UI.WebControls.Label
    Protected WithEvents LBpatente As System.Web.UI.WebControls.Label
    Protected WithEvents LBrendiPubblico As System.Web.UI.WebControls.Label
    Protected WithEvents LBmostraDatiSensibili As System.Web.UI.WebControls.Label
    Protected WithEvents LBmostraRecapiti As System.Web.UI.WebControls.Label

    Protected WithEvents TBRindirizzo As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcapCitta As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRtelFax As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcell As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRnazionalitaMadrelingua As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRpatente As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRdataNascitaSesso As System.Web.UI.WebControls.TableRow
    'Protected WithEvents TBRsesso As System.Web.UI.WebControls.TableRow
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

    'Public Event AggiornaDati(ByVal sender As Object, ByVal e As EventArgs)
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
                    '  Bind_precompilato()
                    ' RaiseEvent NascondiTab(Me, EventArgs.Empty)
                Else
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
            Me.SetupInternazionalizzazione()
        End If
        Dim PRSN_id As Integer
        PRSN_id = Me.HDNprsn_id.Value
        Dim oCurriculum As New COL_CurriculumEuropeo
        oCurriculum.EstraiByPersona(PRSN_id)
        If oCurriculum.ID = -1 Then
            ' Me.BTNaggiungi.Visible = True
            ' Me.BTNmodifica.Visible = False
            'Bind_precompilato()
            ' RaiseEvent NascondiTab(Me, EventArgs.Empty)
        Else
            ' Me.BTNaggiungi.Visible = False
            ' Me.BTNmodifica.Visible = True
            Bind_Dati(oCurriculum)
        End If

    End Sub
#Region "Bind_Dati"
    Public Sub Bind_Dati(ByVal oCurriculum As COL_CurriculumEuropeo)
        Try
            HDNcrev_id.Value = oCurriculum.ID
            Me.LBnome.Text = oCurriculum.Nome
            Me.LBcognome.Text = oCurriculum.Cognome
            Me.LBdataNascita.Text = oCurriculum.DataNascita
            If oCurriculum.Sesso = 1 Then
                Me.LBSesso.Text = oResourceDati.getValue("Uomo")
            ElseIf oCurriculum.Sesso = 0 Then
                Me.LBSesso.Text = oResourceDati.getValue("Donna")
            End If
            Me.LBindirizzo.Text = oCurriculum.Indirizzo
            Me.LBcap.Text = oCurriculum.Cap
            Me.LBcitta.Text = oCurriculum.Citta
            Me.LBtelefono.Text = oCurriculum.Telefono
            Me.LBcellulare.Text = oCurriculum.Cellulare
            Me.LBfax.Text = oCurriculum.Fax
            Me.LBmail.Text = oCurriculum.Mail
            Me.LBnazionalita.Text = oCurriculum.Nazionalita
            Me.LBmadrelingua.Text = oCurriculum.Madrelingua
            Me.LBpatente.Text = oCurriculum.Patente
            'recapiti telefonici
            If oCurriculum.MostraRecapiti = True Then
                TBRtelFax.Visible = Not ((Me.LBtelefono.Text = "") And (Me.LBfax.Text = ""))
                If Me.LBtelefono.Text = "" Then
                    Me.LBtelefono.Text = oResourceDati.getValue("nd")
                End If
                If Me.LBfax.Text = "" Then
                    Me.LBfax.Text = oResourceDati.getValue("nd")
                End If

                TBRcell.Visible = Not (Me.LBcellulare.Text = "")
            Else
                Me.TBRtelFax.Visible = False
                Me.TBRcell.Visible = False
            End If
            'datisensibili
            If oCurriculum.MostraDatiSensibili = True Then
                Me.TBRdataNascitaSesso.Visible = Not ((Me.LBdataNascita.Text = "") And (Me.LBSesso.Text = ""))
                If Me.LBdataNascita.Text = "" Then
                    Me.LBdataNascita.Text = oResourceDati.getValue("nd")
                End If
                If Me.LBSesso.Text = "" Then
                    Me.LBSesso.Text = oResourceDati.getValue("nd")
                End If

                Me.TBRindirizzo.Visible = Not (Me.LBindirizzo.Text = "")

                Me.TBRcapCitta.Visible = Not ((Me.LBcap.Text = "") And (Me.LBcitta.Text = ""))
                If Me.LBcap.Text = "" Then
                    Me.LBcap.Text = oResourceDati.getValue("nd")
                End If
                If Me.LBcitta.Text = "" Then
                    Me.LBcitta.Text = oResourceDati.getValue("nd")
                End If

                TBRnazionalitaMadrelingua.Visible = Not ((Me.LBnazionalita.Text = "") And (Me.LBmadrelingua.Text = ""))
                If Me.LBnazionalita.Text = "" Then
                    Me.LBnazionalita.Text = oResourceDati.getValue("nd")
                End If
                If Me.LBmadrelingua.Text = "" Then
                    Me.LBmadrelingua.Text = oResourceDati.getValue("nd")
                End If
            Else
                Me.TBRdataNascitaSesso.Visible = False
                Me.TBRindirizzo.Visible = False
                Me.TBRcapCitta.Visible = False
                Me.TBRnazionalitaMadrelingua.Visible = False
            End If


            TBRpatente.Visible = Not (Me.LBpatente.Text = "")
            If Me.LBpatente.Text = "" Then
                Me.LBpatente.Text = oResourceDati.getValue("nd")
            End If
        Catch ex As Exception

        End Try
    End Sub
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
            .setLabel(LBnome_s)

            .setLabel(LBcognome_s)
            .setLabel(LBdatanascita_s)
            .setLabel(LBsesso_t)

            .setLabel(LBindirizzo_t)
            .setLabel(LBcap_t)
            .setLabel(LBcitta_t)
            .setLabel(LBtelefono_t)
            .setLabel(LBcellulare_t)
            .setLabel(LBfax_t)
            .setLabel(LBmail_s)
            .setLabel(LBnazionalita_t)
            .setLabel(LBmadrelingua_t)
            .setLabel(LBpatente_t)
        End With
    End Sub
#End Region

End Class
