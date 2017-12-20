Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_StiliChat 'MustInherit
    Inherits System.Web.UI.UserControl
    Protected oResource As ResourceManager

#Region "Pannello Stili"
    Protected WithEvents SelColor As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents BgColor As System.Web.UI.HtmlControls.HtmlSelect

    Protected WithEvents CBItalic As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBUnder As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBBold As System.Web.UI.WebControls.CheckBox
#End Region

#Region "Proprieta"

    Public ReadOnly Property IsItalic() As Boolean
        Get
            If Me.CBItalic.Checked Then
                IsItalic = True
            Else
                IsItalic = False
            End If
        End Get
    End Property
    Public ReadOnly Property IsBold() As Boolean
        Get
            If Me.CBBold.Checked Then
                IsBold = True
            Else
                IsBold = False
            End If
        End Get
    End Property
    Public ReadOnly Property IsUnder() As Boolean
        Get
            If Me.CBUnder.Checked Then
                IsUnder = True
            Else
                IsUnder = False
            End If
        End Get
    End Property

    Public ReadOnly Property GetColor(ByVal Tipo As String) As String
        Get
            If Tipo = "Bg" Then
                GetColor &= "ffffff"
                'Select Case BgColor.SelectedIndex()
                '    Case 1  'Red
                '        GetColor &= "ff0000"
                '    Case 2  'Green
                '        GetColor &= "00ff00"
                '    Case 3  'Blue
                '        GetColor &= "0000ff"
                '    Case 4  'Zurin
                '        GetColor &= "00ffff"
                '    Case 5  'Yellow
                '        GetColor &= "ffff00"
                '    Case 6  'Violet
                '        GetColor &= "ff00ff"
                '    Case 7  'Dark Gray
                '        GetColor &= "afafaf"
                '    Case 8  'Gray
                '        GetColor &= "afafaf"
                '    Case 9  'Brown
                '        GetColor &= "af5050"
                '    Case 10 'Dark green
                '        GetColor &= "50af50"
                '    Case 11 'Dark violet
                '        GetColor &= "5050af"
                '    Case 12 'Dark blue
                '        GetColor &= "50afaf"
                '    Case 13 'Dark yellow
                '        GetColor &= "afaf50"
                '    Case 14 'Dark Violet
                '        GetColor &= "af50af"
                '    Case 15 'Black
                '        GetColor &= "000000"
                '    Case Else 'White
                '        GetColor &= "ffffff"
                'End Select
            Else    '"Fore"
                Select Case SelColor.SelectedIndex()
                    Case 1  'Red
                        GetColor &= "ff0000"
                    Case 2  'Green
                        GetColor &= "00ff00"
                    Case 3  'Blue
                        GetColor &= "0000ff"
                    Case 4  'Zurin
                        GetColor &= "00ffff"
                    Case 5  'Yellow
                        GetColor &= "ffff00"
                    Case 6  'Violet
                        GetColor &= "ff00ff"
                    Case 7  'Dark Gray
                        GetColor &= "afafaf"
                    Case 8  'Gray
                        GetColor &= "afafaf"
                    Case 9  'Brown
                        GetColor &= "af5050"
                    Case 10 'Dark green
                        GetColor &= "50af50"
                    Case 11 'Dark violet
                        GetColor &= "5050af"
                    Case 12 'Dark blue
                        GetColor &= "50afaf"
                    Case 13 'Dark yellow
                        GetColor &= "afaf50"
                    Case 14 'Dark Violet
                        GetColor &= "af50af"
                    Case 15 'white
                        GetColor &= "ffffff"
                    Case Else
                        GetColor &= "000000"
                End Select
            End If
        End Get
    End Property

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
            SetCulture(Session("LinguaCode"), "page_UC_StiliChat")
        End If
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
        End If
        
    End Sub


#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String, ByVal ResourcesName As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_StiliChat"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setCheckBox(Me.CBBold)
            .setCheckBox(Me.CBItalic)
            .setCheckBox(Me.CBUnder)
        End With
    End Sub
#End Region


End Class