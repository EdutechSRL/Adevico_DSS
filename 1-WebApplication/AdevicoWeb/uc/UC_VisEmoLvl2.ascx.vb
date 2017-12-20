
Public Class UC_VisEmoLvl2
    Inherits System.Web.UI.UserControl
    Protected oResource As ResourceManager


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DLEmo As System.Web.UI.WebControls.DataList
    Protected WithEvents PAdm As System.Web.UI.WebControls.Panel
    Protected WithEvents PanEmoXML As System.Web.UI.WebControls.Panel
    Protected WithEvents PanEmo As System.Web.UI.WebControls.Panel

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

        Me.SetFaccine()
    End Sub

    Private Sub SetFaccine()
        If False Then
            'Vecchio codice che utilizza l'xml
            Dim DsEmoOut As New DataSet
            Dim StrXMLDataPath As String = Request.MapPath(GetPercorsoApplicazione(Me.Request) & "/uc/Emoticon.xml") '"Emoticon.xml" 'GetPercorsoApplicazione(me.Request) & "/Emoticon.xml"
            Try
                DsEmoOut.ReadXml(StrXMLDataPath)
                Me.DLEmo.DataSource = DsEmoOut
                Me.DLEmo.DataBind()

                Me.PanEmoXML.Visible = True
                Me.PanEmo.Visible = False
            Catch ex As Exception
                Me.PanEmoXML.Visible = False
                Me.PanEmo.Visible = True
            End Try

        Else
            Me.PanEmoXML.Visible = False
            Me.PanEmo.Visible = True
        End If
    End Sub

    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Chat_Emo"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource

        End With
    End Sub

End Class