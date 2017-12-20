Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_ChatHelp
    Inherits System.Web.UI.UserControl
    Protected oResource As ResourceManager

#Region "Oggetti form"
    Protected WithEvents IBPan1_Chat As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Lbl_IBChat As System.Web.UI.WebControls.Label
    Protected WithEvents IBPan2_Stili As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBPan3_Smile As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBPan4_Tools As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBPan5_File As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBPan6_Utenti As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Lbl_IBUtenti As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents Lbl_ElencoUtenti As System.Web.UI.WebControls.Label
    Protected WithEvents IBPan7_Comunita As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Lbl_IBCom As System.Web.UI.WebControls.Label
    Protected WithEvents ImgLinkHelp As System.Web.UI.WebControls.Image
    Protected WithEvents BtSendAll As System.Web.UI.WebControls.Button
    Protected WithEvents IMBaggiorna As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Lbl_Help As System.Web.UI.WebControls.Label
    Protected WithEvents Lbl_IBStrum As System.Web.UI.WebControls.Label
    Protected WithEvents BtSendTo As System.Web.UI.WebControls.Button
    Protected WithEvents BtPrivate As System.Web.UI.WebControls.Button

    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    Protected WithEvents Lbl_Refresh As System.Web.UI.WebControls.Label
    Protected WithEvents Label35 As System.Web.UI.WebControls.Label
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
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Not Page.IsPostBack Then ' = False
            Me.SetupInternazionalizzazione()
        End If
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_ChatHelp"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.Lbl_IBChat)
            .setLabel(Me.Label16)
            .setLabel(Me.Label15)
            .setLabel(Me.Lbl_IBStrum)
            .setLabel(Me.Label20)
            .setLabel(Me.Label10)
            .setLabel(Me.Label25)
            .setLabel(Me.Lbl_IBUtenti)
            .setLabel(Me.Lbl_ElencoUtenti)
            .setLabel(Me.Label26)
            .setLabel(Me.Label27)
            .setLabel(Me.Label28)
            .setLabel(Me.Label29)
            .setLabel(Me.Label30)
            .setLabel(Me.Label31)
            .setLabel(Me.Label32)
            .setLabel(Me.Label33)
            .setLabel(Me.Label34)
            .setLabel(Me.Label35)

            .setLabel(Me.Label21)
            .setLabel(Me.Label23)

            .setLabel(Me.Lbl_Refresh)

            .setLabel(Me.Lbl_IBCom)
            .setLabel(Me.Lbl_Help)
            .setLabel(Me.Label4)
            .setLabel(Me.Label6)
            .setLabel(Me.Label8)

            .setButton(Me.BtSendAll, False, False, False, False)
            .setButton(Me.BtSendTo, False, False, False, False)
            .setButton(Me.BtPrivate, False, False, False, False)

        End With
    End Sub
#End Region
End Class
