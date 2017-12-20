Public Class ComuniaLimbo
    Inherits System.Web.UI.Page

    Private _Utility As OLDpageUtility
    Private ReadOnly Property Utility() As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
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
        Session("CMNT_path_forAdmin") = ""
        Session("CMNT_path_forNews") = ""
        Session("CMNT_ID_forNews") = ""
        Session("AdminForChange") = False
        Session("idComunita_forAdmin") = ""
        Session("RLPC_ID") = 0
        Session("ArrComunita") = Nothing
        Session("limbo") = True
        Session("IdRuolo") = 0
        Session("IdComunita") = 0

        Dim LinkElenco As String = Me.Utility.SystemSettings.Presenter.DefaultLogonPage
        If LinkElenco = "" Then
            LinkElenco = "Comunita/EntrataComunita.aspx"
        End If
        Me.Utility.RedirectToUrl(LinkElenco)
    End Sub

End Class
