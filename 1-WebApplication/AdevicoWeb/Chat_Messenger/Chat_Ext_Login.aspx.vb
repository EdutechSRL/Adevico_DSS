Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class Chat_Ext_Login
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

    Protected WithEvents LblGestioneWEB As System.Web.UI.WebControls.Label
    'Protected WithEvents UC_ChatHead As Comunita_OnLine.UC_ChatHead
    Protected WithEvents UC_ChatLogin As Comunita_OnLine.UC_LoginChat
    'Protected WithEvents CTRLFooter As Comunita_OnLine.UC_ChatFoot
    Protected WithEvents HYLchiudi As System.Web.UI.HtmlControls.HtmlAnchor

    'Public oLocate As COL_Localizzazione
    Private oResource As ResourceManager

    Dim oPersona As New COL_BusinessLogic_v2.CL_persona.COL_Persona

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            oPersona = Session("objPersona")
        Catch ex As Exception
            Me.LblGestioneWEB.Visible = False
            Me.UC_ChatLogin.Visible = True
            Exit Sub
        End Try

        If Me.Application.Item("SystemAcess") = False Then
            ' Accesso consentito solo agli admin
            If oPersona.TipoPersona.id <> Main.TipoPersonaStandard.SysAdmin Then
                Me.LblGestioneWEB.Visible = True
                ' Rivedere la logica...
                Me.UC_ChatLogin.Visible = False
            Else
                Me.LblGestioneWEB.Visible = False
                Me.UC_ChatLogin.Visible = True
            End If
        End If
        'Inserire qui il codice utente necessario per inizializzare la pagina
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

            SetCulture(Session("LinguaCode"))

            Me.SetupInternazionalizzazione()
        Catch exUserLanguages As Exception
        End Try
    End Sub

    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Chat_Ext"
        oResource.Folder_Level1 = "pg_ChatBuffer_Ext"
        oResource.setCulture()
    End Sub

    Public Sub SetupInternazionalizzazione()
        With oResource

            '.setLabel(Me.LblGestioneWEB)

            .setHtmlAnchor(Me.HYLchiudi, True, True, False, False)

        End With
    End Sub

#End Region

End Class
