Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class Chat_FileHelp
    Inherits System.Web.UI.Page

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

    Protected oResource As ResourceManager
    Protected WithEvents LblFunzione As System.Web.UI.WebControls.Label
    Protected WithEvents LblTasto As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls2 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls3 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls4 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls5 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls6 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls7 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls8 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls9 As System.Web.UI.WebControls.Label
    Protected WithEvents LblPuls10 As System.Web.UI.WebControls.Label
    Protected WithEvents LblNote As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If

    End Sub

#Region "Localizzazione"

    Private Sub setupLingua()
        Try
            If IsNumeric(Session("LinguaID")) And Session("LinguaCode") <> "" Then

            Else
                Dim LinguaCode As String

                LinguaCode = "en-US"
                Try
                    LinguaCode = Request.UserLanguages(0)
                Catch ex As Exception
                    LinguaCode = "en-US"
                End Try
                If Request.Browser.Cookies = True Then
                    Try
                        LinguaCode = Request.Cookies("LinguaCode").Value
                    Catch ex As Exception

                    End Try
                End If
                'Setto ora il valore nelle variabili di sessione.....
				Dim oLingua As New Lingua
				oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
				If Not IsNothing(oLingua) Then
					Session("LinguaID") = oLingua.Id
					Session("LinguaCode") = oLingua.Codice
				Else
					Session("LinguaID") = 2
					Session("LinguaCode") = "en-US"
				End If
            End If

        Catch exUserLanguages As Exception
        End Try
    End Sub

    Public Function SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Chat_FileHelp"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()
    End Function

    Public Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LblFunzione)
            .setLabel(Me.LblNote)
            .setLabel(Me.LblTasto)
            .setLabel(Me.LblPuls1)
            .setLabel(Me.LblPuls2)
            .setLabel(Me.LblPuls3)
            .setLabel(Me.LblPuls4)
            .setLabel(Me.LblPuls5)
            .setLabel(Me.LblPuls6)
            .setLabel(Me.LblPuls7)
            .setLabel(Me.LblPuls8)
            .setLabel(Me.LblPuls9)
            .setLabel(Me.LblPuls10)

        End With
    End Sub

#End Region
End Class