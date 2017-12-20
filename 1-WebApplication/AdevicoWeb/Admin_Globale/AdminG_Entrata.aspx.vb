Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class AdminG_Entrata
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager

#Region "Gestione Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBcontenuto As System.Web.UI.WebControls.Label

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
        Dim oPersona As COL_Persona
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Try
            oPersona = Session("objPersona")
            If oPersona.Id <> 0 Then

            End If
        Catch ex As Exception
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            oPersona = Nothing
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Response.End()
            Exit Sub
        End Try
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ElencoMateriale"
        oResource.Folder_Level1 = "Generici"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel_To_Value(LBtitolo, "LBtitolo.normale")
            .setLabel(LBNopermessi)
         
            .setLabel(Me.LBcontenuto)
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.normale")
        End With
    End Sub
#End Region

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal 'Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal) 'Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class