

Public Class UC_ChatFoot
    Inherits System.Web.UI.UserControl

    Protected WithEvents LBfooterTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBfooterIndirizzo As System.Web.UI.WebControls.Label

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
        If Page.IsPostBack = False Then
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
            Try
                Me.LBfooterTitolo.Text = oResourceConfig.getValue("footerTitolo")
            Catch ex As Exception

            End Try

            Try
                Me.LBfooterIndirizzo.Text = oResourceConfig.getValue("footerIndirizzo")
            Catch ex As Exception

            End Try
        End If
    End Sub

End Class
