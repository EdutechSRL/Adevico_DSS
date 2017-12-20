Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo



Public Class UC_infoCompetenze
    Inherits System.Web.UI.UserControl
    Protected oResourceCompetenze As ResourceManager


#Region "dati Label"
    Protected WithEvents LBcompetenzeRelazionali_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcompetenzeOrganizzative_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcompetenzeTecniche_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcompetenzeArtistiche_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBaltreCompetenze_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBulterioriInfo_t As System.Web.UI.WebControls.Label
#End Region
#Region "accessori"
    Protected WithEvents TBRrelazionali As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRorganizzative As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRtecniche As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRartistiche As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRaltre As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRulterioriInfo As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBLcompetenze As System.Web.UI.WebControls.Table
    ' Protected WithEvents BTNaggiungi As System.Web.UI.WebControls.Button
    'Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    ' Protected WithEvents HDNcrev_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBRmessaggio As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
#End Region
#Region "dati LAbel"
    Protected WithEvents LBcompetenzeRelazionali As System.Web.UI.WebControls.Label
    Protected WithEvents LBcompetenzeOrganizzative As System.Web.UI.WebControls.Label
    Protected WithEvents LBcompetenzeTecniche As System.Web.UI.WebControls.Label
    Protected WithEvents LBcompetenzeArtistiche As System.Web.UI.WebControls.Label
    Protected WithEvents LBaltreCompetenze As System.Web.UI.WebControls.Label
    Protected WithEvents LBulterioriInfo As System.Web.UI.WebControls.Label
#End Region


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
        If IsNothing(oResourceCompetenze) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            If Page.IsPostBack = False Then
                Me.TBRmessaggio.Visible = False
                Me.SetupInternazionalizzazione()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function start() As Boolean
        If IsNothing(oResourceCompetenze) Then
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If
        Me.TBRmessaggio.Visible = False
        Dim PRSN_id As Integer
        PRSN_id = Me.HDNprsn_id.Value
        Dim oCurriculum As New COL_CurriculumEuropeo
        oCurriculum.EstraiByPersona(PRSN_id)

        If oCurriculum.CompetenzeArtistiche = "" And oCurriculum.CompetenzeOrganizzative = "" And oCurriculum.CompetenzeRelazionali = "" And oCurriculum.CompetenzeTecniche = "" And oCurriculum.AltreCompetenze = "" And oCurriculum.UlterioriInfo = "" Then
            Return False
        Else
            Bind_Dati(oCurriculum)
            Return True
        End If
    End Function

#Region "Bind_Dati"
    Public Function Bind_Dati(ByVal oCurriculum As COL_CurriculumEuropeo)
        Try

            If oCurriculum.CompetenzeRelazionali <> "" Then
                Me.LBcompetenzeRelazionali.Text = Replace(oCurriculum.CompetenzeRelazionali, vbCrLf, "<br>")
            End If
            If oCurriculum.CompetenzeOrganizzative <> "" Then
                Me.LBcompetenzeOrganizzative.Text = Replace(oCurriculum.CompetenzeOrganizzative, vbCrLf, "<br>")
            End If
            If oCurriculum.CompetenzeTecniche <> "" Then
                Me.LBcompetenzeTecniche.Text = Replace(oCurriculum.CompetenzeTecniche, vbCrLf, "<br>")
            End If
            If oCurriculum.CompetenzeArtistiche <> "" Then
                Me.LBcompetenzeArtistiche.Text = Replace(oCurriculum.CompetenzeArtistiche, vbCrLf, "<br>")
            End If
            If oCurriculum.AltreCompetenze <> "" Then
                Me.LBaltreCompetenze.Text = Replace(oCurriculum.AltreCompetenze, vbCrLf, "<br>")
            End If
            If oCurriculum.UlterioriInfo <> "" Then
                Me.LBulterioriInfo.Text = Replace(oCurriculum.UlterioriInfo, vbCrLf, "<br>")
            End If


            TBRrelazionali.Visible = Not (Me.LBcompetenzeRelazionali.Text = "")
            TBRorganizzative.Visible = Not (Me.LBcompetenzeOrganizzative.Text = "")
            TBRtecniche.Visible = Not (Me.LBcompetenzeTecniche.Text = "")
            TBRartistiche.Visible = Not (Me.LBcompetenzeArtistiche.Text = "")
            TBRaltre.Visible = Not (Me.LBaltreCompetenze.Text = "")
            TBRulterioriInfo.Visible = Not (Me.LBulterioriInfo.Text = "")
        Catch ex As Exception

        End Try
    End Function
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceCompetenze = New ResourceManager

        oResourceCompetenze.UserLanguages = Code
        oResourceCompetenze.ResourcesName = "pg_UC_Competenze"
        oResourceCompetenze.Folder_Level1 = "Curriculum"
        oResourceCompetenze.Folder_Level2 = "UC"
        oResourceCompetenze.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceCompetenze

            .setLabel(LBcompetenzeRelazionali_t)
            .setLabel(LBcompetenzeOrganizzative_t)
            .setLabel(LBcompetenzeTecniche_t)
            .setLabel(LBaltreCompetenze_t)

            .setLabel(LBulterioriInfo_t)
        End With
    End Sub
#End Region

End Class
