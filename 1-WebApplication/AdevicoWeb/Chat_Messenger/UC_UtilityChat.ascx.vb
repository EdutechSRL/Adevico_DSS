'Pannello utility Chat
'Inserire qui eventuali altri strumenti di getione chat (Stampa, cancella ed altri bottoni)...
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Chat

Public Class UC_UtilityChat
    Inherits System.Web.UI.UserControl
    Protected oResource As ResourceManager

    Dim MinRef As Integer = 10 '10000 'Tempo di refresh minimo: 10"
    Dim Messaggi As New WS_Chat.WS_ChatSoapClient

    Public Property IDCom() As Integer
        Get
            Return CInt(ViewState("IDCom"))
            'Return IdComunita
        End Get
        Set(ByVal Value As Integer)
            'IdComunita = Value
            ViewState("IDCom") = Value
        End Set
    End Property

#Region " Codice generato da Progettazione Web Form "
    Protected WithEvents DDLTime As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Pan_UtiAdm As System.Web.UI.WebControls.Panel
    Protected WithEvents IBAClear As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LblRef As System.Web.UI.WebControls.Label
    Protected WithEvents BtSmall As System.Web.UI.WebControls.Button
    Protected WithEvents BtNormal As System.Web.UI.WebControls.Button
    Protected WithEvents BtBig As System.Web.UI.WebControls.Button


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
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
            Me.changeRef(MinRef)
        End If

        'Intanto... Poi si vedra'...
        Me.Pan_UtiAdm.Visible = False
    End Sub

#Region "Refresh"

    Private Sub changeRef(ByVal time As Integer)
        'If Not time = 0 Then
        Session("RefTime") = time
        'Session("RefTime") = "<script type=" & Chr(34) & "text/javascript" & Chr(34) & ">"
        'Session("RefTime") &= "setTimeout('location.href=" & Chr(34) & "ChatOutPut_Ext.aspx" & Chr(34) & "'," & time.ToString & ");"
        'Session("RefTime") &= "</script>"
        'Else
        '    Session("RefTime") = ""
        'End If
    End Sub

    Private Sub DDLTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTime.SelectedIndexChanged
        MinRef = 1000 * CInt(DDLTime.SelectedValue)  'Serve solo per mantenere il valore al refresh della pagina
        Me.changeRef(CInt(DDLTime.SelectedValue)) ' * 1000)
    End Sub

#End Region

#Region "Pulsanti"


    Public WriteOnly Property IsAdmin() As Boolean
        Set(ByVal Value As Boolean)
            Me.Pan_UtiAdm.Visible = Value
        End Set
    End Property

    Private Sub IBAClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If Not IDCom = 0 Then
            Messaggi.ResetMsg(Session("objPersona").Id, Me.IDCom) '
        End If
    End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_UtilityChat"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LblRef)

            .setImageButton(Me.IBAClear, True, True, False, False)

        End With
    End Sub
#End Region

End Class
