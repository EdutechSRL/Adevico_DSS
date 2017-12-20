Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_Fase5Finale
    Inherits System.Web.UI.UserControl

    Private oResourceFinale As ResourceManager

    Protected WithEvents LBfinale As System.Web.UI.WebControls.Label
    Protected WithEvents LBcomunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcomunita As System.Web.UI.WebControls.Label
    Protected WithEvents LBresponsabile_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBresponsabile As System.Web.UI.WebControls.Label
    Protected WithEvents TBRpadri As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBpadri_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBpadri As System.Web.UI.WebControls.Label
    Protected WithEvents TBRservizi As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBservizi_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBservizi As System.Web.UI.WebControls.Label



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
        If IsNothing(oResourceFinale) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
    End Sub

    Public Sub SetupControl(ByVal ComunitaPadreID As Integer, ByVal NomeComunita As String, ByVal NomeResponsabile As String, ByVal Padri As String, ByVal Servizi As String)
        If IsNothing(oResourceFinale) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()

        Me.LBcomunita.Text = NomeComunita
        Me.LBresponsabile.Text = NomeResponsabile

        Try
            Me.LBpadri.Text = "<b>" & COL_Comunita.EstraiNomeBylingua(ComunitaPadreID, Session("LinguaID")) & "</b><br>"
        Catch ex As Exception

        End Try
        Me.LBpadri.Text &= Padri
        Me.LBservizi.Text = Servizi
    End Sub



#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        oResourceFinale = New ResourceManager

        oResourceFinale.UserLanguages = code
        oResourceFinale.ResourcesName = "pg_UC_Fase5finale"
        oResourceFinale.Folder_Level1 = "Comunita"
        oResourceFinale.Folder_Level2 = "UC_WizardComunita"
        oResourceFinale.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        If IsNothing(oResourceFinale) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        With oResourceFinale
            .setLabel(Me.LBfinale)
            .setLabel(Me.LBcomunita_t)
            .setLabel(Me.LBresponsabile_t)
            .setLabel(Me.LBservizi_t)
            .setLabel(Me.LBpadri_t)
        End With
    End Sub
#End Region

End Class