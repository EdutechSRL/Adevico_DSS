Imports lm.Comol.Core.File
Public Class DebugAccess
    Inherits System.Web.UI.Page
    Private _PageUtility As PresentationLayer.OLDpageUtility

    Private ReadOnly Property SendToUrl As String
        Get
            If Not String.IsNullOrEmpty(Me.Request.QueryString("Url") & "<br>") Then
                Return Server.HtmlDecode(Me.Request.QueryString("Url") & "<br>")
            End If
            Return ""
        End Get
    End Property
    Private ReadOnly Property CommunityID As Integer
        Get
            If IsNumeric(Me.Request.QueryString("CommunityID") & "<br>") Then
                Return CInt(Me.Request.QueryString("CommunityID") & "<br>")
            End If
            Return 0
        End Get
    End Property
    Private ReadOnly Property LogonUserID As Integer
        Get
            If IsNumeric(Me.Request.QueryString("UserID") & "<br>") Then
                Return CInt(Me.Request.QueryString("UserID") & "<br>")
            End If
            Return 0
        End Get
    End Property
    Protected Friend ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub BTNlogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlogin.Click
        Dim idCommunity = 0
        If Not String.IsNullOrEmpty(Me.comunitaID.Text) AndAlso IsNumeric(Me.comunitaID.Text) Then
            idCommunity = CInt(Me.comunitaID.Text)
        End If
        AutoLogon(Me.userID.Text, idCommunity, Me.TXBdestinazione.Text)
    End Sub

    Private Sub UrlAutoLogon()
        Dim UrlUserID As Integer = Me.LogonUserID
        Dim UrlCommunityID As Integer = Me.CommunityID
        Dim Url As String = Me.SendToUrl

        If UrlUserID > 0 Then
            Me.AutoLogon(UrlUserID, UrlCommunityID, Url)
        End If
    End Sub

    Private Sub AutoLogon(ByVal pUserID As Integer, ByVal pCommunityID As Integer, ByVal pUrl As String)
        Dim oPersona As COL_Persona = COL_Persona.GetPersona(pUserID, 1)
        oPersona.Istituzione.Id = 1
        Session("LinguaID") = oPersona.Lingua.ID
        Session("LinguaCode") = oPersona.Lingua.Codice
        Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = Session("LinguaID"), .Icon = oPersona.Lingua.Icona, .Code = oPersona.Lingua.Codice, .isDefault = oPersona.Lingua.isDefault, .Name = oPersona.Lingua.Nome}
        Session("objPersona") = oPersona
        Session("ORGN_id") = oPersona.GetOrganizzazioneDefault
        Session("Istituzione") = oPersona.GetIstituzione

        If pCommunityID = 0 Then
            Me.PageUtility.isPortalCommunity = True
            Me.PageUtility.isModalitaAmministrazione = False
            Me.PageUtility.AmministrazioneComunitaID = 0
            Session("IdRuolo") = ""

            Session("IdComunita") = 0
            Session("ArrPermessi") = ""
            Session("ArrComunita") = ""
            Session("RLPC_ID") = ""
            Session("CMNT_path_forAdmin") = ""
            If pUrl = "" Then
                Me.PageUtility.RedirectToUrl("Modules/Noticeboard/NoticeboardDashboard.aspx?lfp=false")
            Else
                Me.PageUtility.RedirectToUrl(pUrl)
            End If
        Else
            Me.PageUtility.isPortalCommunity = False
            Me.PageUtility.isModalitaAmministrazione = False
            Me.PageUtility.AmministrazioneComunitaID = 0
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
            If pUrl = "" Then
                Me.PageUtility.AccessToCommunity(pUserID, pCommunityID, oResourceConfig, True)

            Else
                Me.PageUtility.AccessToCommunity(pUserID, pCommunityID, oResourceConfig, pUrl, True)
            End If
        End If

    End Sub

    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        PageUtility.IsInDebugMode = True
    End Sub
End Class