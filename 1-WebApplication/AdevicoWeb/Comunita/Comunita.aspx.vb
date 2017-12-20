Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices

Public Class Comunita
    Inherits PageBase


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
        If Not Page.IsPostBack Then
            GoBacheca()
        End If

        If Session("objPersona") Is Nothing Then 'se la sessione è scaduta redirecto alla home
            Response.Redirect("./../index.aspx")
        End If
    End Sub

    Private Sub GoBacheca()
        'Dim url As String = SystemSettings.Presenter.DefaultCommunityModule()
        'If Not String.IsNullOrEmpty(url) AndAlso url.Contains("{0}") Then
        '    url = String.Format(url, PageUtility.CurrentContext.UserContext.CurrentCommunityID)
        '    PageUtility.RedirectToUrl(url)
        'ElseIf Not String.IsNullOrEmpty(url) Then
        '    PageUtility.RedirectToUrl(url)
        'End If

        Dim oUtility As New OLDpageUtility(Me.Context)
        Try
            Dim oServizio As New UCServices.Services_Bacheca(GetPermessiForService(oServizio.Codex, Me.Page))
            If oServizio.Read Then 'se ho i permessi per la bacheca...

                oUtility.RedirectToUrl("Modules/Noticeboard/NoticeboardDashboard.aspx?lfp=false")
                '                oUtility.RedirectToUrl("Generici/CommunityNoticeBoard.aspx?View=CurrentMessage&SmallView=LastFourMessage")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
End Class