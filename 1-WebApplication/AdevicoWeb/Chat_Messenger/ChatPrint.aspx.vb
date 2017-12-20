Imports COL_BusinessLogic_v2


Public Class ChatPrint
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager
#Region "Oggetti web"
    Protected WithEvents LblComunita As System.Web.UI.WebControls.Label
    Protected WithEvents LblData_t As System.Web.UI.WebControls.Label
    Protected WithEvents LblTime As System.Web.UI.WebControls.Label
    Protected WithEvents LblMessaggi As System.Web.UI.WebControls.Label
    Protected WithEvents Messaggi As System.Web.UI.WebControls.Label
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
        'Localizzazione
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If

        Dim NmCom As String
        Try
            NmCom = Request.QueryString.Item("NmCom")
        Catch ex As Exception
            NmCom = ""
        End Try
        Me.LblComunita.Text = NmCom

        Dim IdCom As Integer
        Try
            IdCom = CInt(Request.QueryString.Item("IDCom"))
        Catch ex As Exception
            IdCom = 0
        End Try

        Try
            Me.LblMessaggi.Text = Session("ChatMsg" & IdCom)
        Catch ex As Exception
            Me.LblMessaggi.Text = ""
        End Try

        Me.LblTime.Text = Now.ToString

    End Sub

#Region "Localizzazioni"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ChatPrint"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LblData_t)
        End With
    End Sub

#End Region

End Class
