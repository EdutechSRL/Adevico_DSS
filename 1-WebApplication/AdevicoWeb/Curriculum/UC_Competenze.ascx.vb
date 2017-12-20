Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo


Imports iTextSharp

Public Class UC_Competenze
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
    Protected WithEvents TBLcompetenze As System.Web.UI.WebControls.Table
    ' Protected WithEvents BTNaggiungi As System.Web.UI.WebControls.Button
    'Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcrev_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBRmessaggio As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
#End Region
#Region "dati textbox"
    Protected WithEvents TXBcompetenzeRelazionali As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcompetenzeOrganizzative As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcompetenzeTecniche As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBcompetenzeArtistiche As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBaltreCompetenze As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBulterioriInfo As System.Web.UI.WebControls.TextBox
#End Region

    Public Event MostraModifica(ByVal sender As Object, ByVal e As EventArgs)
    Public Event MostraAggiungi(ByVal sender As Object, ByVal e As EventArgs)

    Public Property CurriculumID() As Integer
        Get
            If Me.HDNcrev_id.Value = "" Then
                Me.HDNcrev_id.Value = 0
            End If
            CurriculumID = Me.HDNcrev_id.Value
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
        Try
            If Page.IsPostBack = False Then
                Me.TBRmessaggio.Visible = False
                Me.SetupInternazionalizzazione()
            End If
            Me.Setup_starupScript()
            If IsNothing(oResourceCompetenze) Then
                SetCulture(Session("LinguaCode"))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub start()
        Me.TBRmessaggio.Visible = False
        Me.SetupInternazionalizzazione()
        Try
            Dim PRSN_id As Integer
            Dim oCurriculum As New COL_CurriculumEuropeo

            PRSN_id = Me.HDNprsn_id.Value
            oCurriculum.EstraiByPersona(PRSN_id)
            HDNcrev_id.Value = oCurriculum.ID
            If oCurriculum.CompetenzeArtistiche = "" And oCurriculum.CompetenzeOrganizzative = "" And oCurriculum.CompetenzeRelazionali = "" And oCurriculum.CompetenzeTecniche = "" And oCurriculum.AltreCompetenze = "" And oCurriculum.UlterioriInfo = "" Then
                RaiseEvent MostraAggiungi(Me, EventArgs.Empty)
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
            Me.TXBcompetenzeRelazionali.Text = oCurriculum.CompetenzeRelazionali
            Me.TXBcompetenzeOrganizzative.Text = oCurriculum.CompetenzeOrganizzative
            Me.TXBcompetenzeTecniche.Text = oCurriculum.CompetenzeTecniche
            Me.TXBcompetenzeArtistiche.Text = oCurriculum.CompetenzeArtistiche
            Me.TXBaltreCompetenze.Text = oCurriculum.AltreCompetenze
            Me.TXBulterioriInfo.Text = oCurriculum.UlterioriInfo
        Catch ex As Exception

        End Try
    End Function

    Private Sub Setup_starupScript()
        Me.TXBcompetenzeRelazionali.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
        Me.TXBcompetenzeOrganizzative.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
        Me.TXBcompetenzeTecniche.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
        Me.TXBcompetenzeArtistiche.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
        Me.TXBaltreCompetenze.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
        Me.TXBulterioriInfo.Attributes.Add("onkeypress", "return(LimitText(this,5000));")
    End Sub
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

    Public Sub ModificaCompetenza()
        Dim oCurriculum As New COL_CurriculumEuropeo
        Try
            With oCurriculum
                .ID = HDNcrev_id.Value

                .PRSN_ID = Me.HDNprsn_id.Value
                .Estrai()

                .CompetenzeRelazionali = Me.TXBcompetenzeRelazionali.Text
                .CompetenzeOrganizzative = Me.TXBcompetenzeOrganizzative.Text
                .CompetenzeTecniche = Me.TXBcompetenzeTecniche.Text
                .CompetenzeArtistiche = Me.TXBcompetenzeArtistiche.Text
                .AltreCompetenze = Me.TXBaltreCompetenze.Text
                .UlterioriInfo = Me.TXBulterioriInfo.Text

            End With
            oCurriculum.Modifica()
            RaiseEvent MostraModifica(Me, EventArgs.Empty)
        Catch ex As Exception
            Me.TBRmessaggio.Visible = True
            oResourceCompetenze.setLabel_To_Value(LBmessaggio, "errore2")
        End Try
        Me.TBRmessaggio.Visible = True
        oResourceCompetenze.setLabel_To_Value(LBmessaggio, "errore3")
    End Sub

End Class
