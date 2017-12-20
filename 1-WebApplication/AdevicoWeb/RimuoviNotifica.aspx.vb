Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona


Public Class RimuoviNotifica
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Protected WithEvents LBinfoRimuoviNotifica As System.Web.UI.WebControls.Label
    Protected WithEvents BTNhome As System.Web.UI.WebControls.Button
    Protected WithEvents BTNchiudi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma As System.Web.UI.WebControls.Button

    Protected WithEvents HDN_PersonaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ForumID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_TopicID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_PostID As System.Web.UI.HtmlControls.HtmlInputHidden

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
        If Page.IsPostBack = False Then

            SetupDatiForum()

        End If
    End Sub

    Private Sub SetupDatiForum()
        Dim ForumId, TopicID, PostID, PersonaID, ComunitaID As Integer
        Dim AddCode, ExpUrl, ExpUrl2, Expfor, action, DefaultUrl As String

        Dim pageUtility As New OLDpageUtility(HttpContext.Current)
        Dim RichiediSSL As Boolean = pageUtility.SystemSettings.Login.isSSLrequired
        Dim RichiediLoginSSL As Boolean = pageUtility.SystemSettings.Login.isSSLloginRequired
      

        Try
            AddCode = Me.Request.QueryString("AddCode")
            If AddCode = "" Then
                PersonaID = 0
            Else
                Dim oPersona As New COL_Persona
                PersonaID = AddCode.Remove(AddCode.Length - 5, 5)
                oPersona.Id = PersonaID
                oPersona.Estrai(Session("LinguaID"))
                If oPersona.Errore = Errori_Db.None Then
                    Session("LinguaID") = oPersona.Lingua.Id
                    Session("LinguaCode") = oPersona.Lingua.Codice
                Else
                    PersonaID = 0
                End If
            End If
        Catch ex As Exception
            PersonaID = 0
        End Try


        Try
            ExpUrl = Me.Request.QueryString("ExpUrl")
            If ExpUrl = "" Then
                ForumId = 0
            Else
                If ExpUrl.Chars(0) = "k" Then
                    ForumId = ExpUrl.Remove(0, 5)
                End If

            End If
        Catch ex As Exception
            ForumId = 0
        End Try


        ExpUrl2 = Me.Request.QueryString("ExpUrl2")
        TopicID = 0
        Try
            If ExpUrl2 <> "" Then
                If ExpUrl2.Chars(0) = "j" Then
                    TopicID = ExpUrl2.Remove(0, 6)
                End If
            End If
        Catch ex As Exception
            TopicID = 0
        End Try

        Expfor = Me.Request.QueryString("for")
        PostID = 0
        If Expfor <> "" Then
            Dim stringaInit As String
            Try
                stringaInit = Left(Expfor, 5)
                If stringaInit = "x25jt" Then
                    PostID = Expfor.Remove(0, 13)
                End If
            Catch ex As Exception
                PostID = 0
            End Try
        End If

        Me.SetupLingua()

        Dim oForum As New COL_Forums

        oForum.RecuperaDettagli(ForumId, TopicID, PostID, ComunitaID)
        oForum.Id = ForumId
        oForum.Estrai()

        If ForumId > 0 And TopicID > 0 And PostID > 0 Then
            Me.LBinfoRimuoviNotifica.Text = Me.oResource.getValue("LBinfoRimuoviNotifica.Topic")
        ElseIf ForumId > 0 And TopicID > 0 Then
            Me.LBinfoRimuoviNotifica.Text = Me.oResource.getValue("LBinfoRimuoviNotifica.Forum")
        End If
        If oForum.Name <> "" Then
            Me.LBinfoRimuoviNotifica.Text = Replace(Me.LBinfoRimuoviNotifica.Text, "#nomeforum#", oForum.Name)
        Else
            Me.LBinfoRimuoviNotifica.Text = Replace(Me.LBinfoRimuoviNotifica.Text, "#nomeforum#", "")
        End If
        Dim oComunita As New COL_Comunita
        oComunita.Id = ComunitaID
        oComunita.Estrai()
        If oComunita.Nome <> "" Then
            Me.LBinfoRimuoviNotifica.Text = Replace(Me.LBinfoRimuoviNotifica.Text, "#nomeComunita#", oComunita.Nome)
        Else
            Me.LBinfoRimuoviNotifica.Text = Replace(Me.LBinfoRimuoviNotifica.Text, "#nomeComunita#", "")
        End If

        Dim oForumTopic As New COL_Forum_threads
        Me.LBinfoRimuoviNotifica.Text = Replace(Me.LBinfoRimuoviNotifica.Text, "#NomeTopic#", oForumTopic.EstraiSubject(TopicID))
        Me.LBinfoRimuoviNotifica.Text = Replace(Me.LBinfoRimuoviNotifica.Text, "#NomeTopic#", "")

        Me.HDN_PersonaID.Value = PersonaID
        Me.HDN_ForumID.Value = ForumId
        Me.HDN_TopicID.Value = TopicID
        Me.HDN_ComunitaID.Value = ComunitaID
        Me.HDN_PostID.Value = PostID
    End Sub

#Region "Internazionalizzazione"

    Private Sub SetupLingua()
        Try
            Dim LinguaCode As String

            ' Di default metteremo l'inglese !
            LinguaCode = "it-IT"
            Try
                LinguaCode = Request.UserLanguages(0)
            Catch ex As Exception
                LinguaCode = "it-IT"
            End Try

            ' Verifico se l'utente ha qualche cookie !
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
				Session("LinguaID") = 1
				Session("LinguaCode") = "it-IT"
			End If

            'setto il codice della cultura individuata, e poi faccio il bind dei dati !!!
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        Catch exUserLanguages As Exception
        End Try
    End Sub
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_AccessoForum"
        oResource.Folder_Level1 = "Root"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        oResource.setButton(Me.BTNhome, True, , , True)
        oResource.setButton(Me.BTNchiudi, True, , , True)
        oResource.setButton(Me.BTNconferma, True, , , True)
        Me.BTNchiudi.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

    Private Sub BTNhome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNhome.Click
        Dim pageUtility As New OLDpageUtility(HttpContext.Current)
        Dim url As String = pageUtility.GetDefaultLogoutPage()
        If String.IsNullOrEmpty(url) Then
            pageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False), pageUtility.SystemSettings.Login.isSSLloginRequired)
        ElseIf url.StartsWith(pageUtility.BaseUrl) Then
            pageUtility.RedirectToUrl(url, pageUtility.SystemSettings.Login.isSSLloginRequired)
        ElseIf url.StartsWith("http") Then
            Response.Redirect(url)
        End If
    End Sub

    Private Sub BTNconferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconferma.Click
        Dim oForumTopic As New COL_Forum_threads
        Dim oForum As New COL_Forums
        Dim PersonaID, ForumId, TopicID, ComunitaID, PostID As Integer
        Dim alertMSG As String = ""

        PersonaID = Me.HDN_PersonaID.Value
        ForumId = Me.HDN_ForumID.Value
        TopicID = Me.HDN_TopicID.Value
        ComunitaID = Me.HDN_ComunitaID.Value
		PostID = Me.HDN_PostID.Value

		If Me.oResource Is Nothing Then
			Me.SetCulture(Session("LinguaCode"))
		End If
        If ForumId > 0 And TopicID > 0 And PostID > 0 Then
            oForumTopic.Id = TopicID
            oForumTopic.RimuoviNotifica(PersonaID)


            If oForumTopic.Errore = Errori_Db.None Then
                alertMSG = Me.oResource.getValue("RimuoviTopic.true")
                If alertMSG <> "" Then
                    alertMSG = Replace(alertMSG, "'", "\'")
                End If
                Dim PageUtility As New OLDpageUtility(Me.Context)
                Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
                Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
                Response.End()
            Else
                alertMSG = Me.oResource.getValue("RimuoviTopic.false")
                If alertMSG <> "" Then
                    alertMSG = Replace(alertMSG, "'", "\'")
                End If
                Dim PageUtility As New OLDpageUtility(Me.Context)
                Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
                Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            End If
        ElseIf ForumId > 0 And TopicID > 0 Then
            oForum.Id = ForumId
            oForum.RimuoviNotifica(PersonaID)
            If oForum.Errore = Errori_Db.None Then
                alertMSG = Me.oResource.getValue("RimuoviForum.true")
                If alertMSG <> "" Then
                    alertMSG = Replace(alertMSG, "'", "\'")
                End If
                Dim PageUtility As New OLDpageUtility(Me.Context)
                Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
                Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
                Response.End()
            Else
                alertMSG = Me.oResource.getValue("RimuoviForum.false")
                If alertMSG <> "" Then
                    alertMSG = Replace(alertMSG, "'", "\'")
                End If
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
        End If



    End Sub
End Class
