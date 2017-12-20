

Public Class chatUtenteOut_Ext
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager
    Protected WithEvents Lbl_Messaggio As System.Web.UI.WebControls.Label

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

    End Sub

#Region "Localizzazioni"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ChatUtenteOut_Ext"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.Lbl_Messaggio)
        End With
    End Sub

#End Region
End Class
