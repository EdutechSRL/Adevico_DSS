Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Public MustInherit Class OpenPageBase
    Inherits System.Web.UI.Page
    Private _Resource As ResourceManager
    Private _ServiziCorrenti As ServiziCorrenti
    Private _Instance As Boolean
    Protected _Language As Comol.Entity.Lingua
    Private _PageUtility As PresentationLayer.OLDpageUtility

    Protected ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property

    Public Sub New()
    End Sub
    Protected ReadOnly Property Resource() As ResourceManager
        Get
            Try
                If IsNothing(_Resource) Then
                    Me.SetCultureSettings()
                End If
                Resource = _Resource
            Catch ex As Exception
                Resource = New ResourceManager
            End Try
        End Get
    End Property
    Protected Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String)
        Me._Resource = New ResourceManager

        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.setCulture()
    End Sub
    Protected Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String)
        Me._Resource = New ResourceManager
        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.Folder_Level2 = ResourceFolderLevel2
        Me._Resource.setCulture()
    End Sub
    Protected Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String, ByVal ResourceFolderLevel3 As String)
        Me._Resource = New ResourceManager
        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.Folder_Level2 = ResourceFolderLevel2
        Me._Resource.Folder_Level3 = ResourceFolderLevel3
        Me._Resource.setCulture()
    End Sub


    Protected MustOverride Sub SetCultureSettings()
    Protected MustOverride Sub SetInternazionalizzazione()
    Public MustOverride Sub BindDati()

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.SetCultureSettings()
    End Sub

    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance '(Me.LinguaCode, Me.BaseUrlDrivePath, BaseUrl)
        End Get
    End Property

    Protected Property PostItSistema() As COL_BusinessLogic_v2.COL_PostIt
        Get
            Try
                PostItSistema = DirectCast(Me.Application.Item("oSystemPostIt"), COL_PostIt)
            Catch ex As Exception
                PostItSistema = Nothing
                Me.Application.Item("ShowSystemPostIt") = False
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.COL_PostIt)
            Me.Application.Item("oSystemPostIt") = value
        End Set
    End Property
    Protected Property ShowPostItSistema() As Boolean
        Get
            Try
                ShowPostItSistema = DirectCast(Me.Application.Item("ShowSystemPostIt"), Boolean)
            Catch ex As Exception
                Me.Application.Item("ShowSystemPostIt") = False
                ShowPostItSistema = False
            End Try
        End Get
        Set(ByVal value As Boolean)
            Me.Application.Item("ShowSystemPostIt") = value
        End Set
    End Property
    Protected Property RiepilogoPostIt() As Integer
        Get
            Try
                RiepilogoPostIt = DirectCast(Session("Popupwin"), Integer)
            Catch ex As Exception
                RiepilogoPostIt = 0
            End Try

        End Get
        Set(ByVal value As Integer)
            Session("Popupwin") = value
        End Set
    End Property


    Public MustOverride ReadOnly Property AlwaysBind() As Boolean
    Public MustOverride ReadOnly Property VerifyAuthentication() As Boolean


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If AllowAccess() Then
            If Page.IsPostBack = False Then
                Me.SetInternazionalizzazione()
            End If
            If HasPermessi() Then
                If AlwaysBind Then
                    Me.BindDati()
                ElseIf Page.IsPostBack = False Then
                    Me.RegistraAccessoPagina()
                    Me.BindDati()
                End If
            End If
        Else
            Me.BindNoPermessi()
        End If
    End Sub
    Public MustOverride Function HasPermessi() As Boolean
    Public MustOverride Sub RegistraAccessoPagina()
    Public MustOverride Sub BindNoPermessi()

    Private Function AllowAccess() As Boolean
        Dim iResponse As Boolean = False

        If Me.VerifyAuthentication Then
            iResponse = Not IsSessioneScaduta()
        Else
            iResponse = True
        End If
        Return iResponse
    End Function

    Protected Function IsSessioneScaduta() As Boolean
        Dim o As Object = Me.PageUtility.CurrentContext.UserContext
        Dim isScaduta As Boolean = Me.PageUtility.CurrentContext.UserContext.isAnonymous

        If Not isScaduta And Me.PageUtility.CurrentContext.UserContext.CurrentUserID = Me.PageUtility.AnonymousPerson.ID Then
            isScaduta = True
        End If
        'If isScaduta Then
        '    isScaduta = True
        'ElseIf Me.PageUtility.isPortalCommunity And Me.PageUtility.WorkingCommunityID > 0 Then
        '    isScaduta = True
        'End If
        Return isScaduta
    End Function
    Protected Function ViewGetPostBackEventReference(ByVal argomenti As String) As String
        Return Page.ClientScript.GetPostBackEventReference(Me, argomenti)
    End Function


    Protected Sub WriteLogoutAccessCookie(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostPage As String, ByVal ForDownload As Boolean)
        Dim oHttpCookie As New HttpCookie("LogoutAccess")
        Dim minutes As Long = Me.SystemSettings.BlogSettings.ValidationTime
        oHttpCookie.Expires = Now.AddMinutes(minutes)
        oHttpCookie.Values.Add("PersonID", PersonID)
        oHttpCookie.Values.Add("PersonLogin", PersonLogin)
        oHttpCookie.Values.Add("PostPage", Replace(PostPage, "&", "#_#"))
        oHttpCookie.Values.Add("Download", ForDownload)
        oHttpCookie.Values.Add("CommunityID", CommunityID)
        If (From k In Response.Cookies.AllKeys Where k = "LogoutAccess").Any Then
            Response.Cookies.Set(oHttpCookie)
        Else
            Response.Cookies.Add(oHttpCookie)
        End If
        'Dim secured As HttpCookie
        'Dim domain As String = Me.SystemSettings.BlogSettings.DomainCookie
        'Dim minutes As Long = Me.SystemSettings.BlogSettings.ValidationTime
        'Dim hash As New Hashtable()
        'hash.Add("PersonID", PersonID)
        'hash.Add("PersonLogin", PersonLogin)
        'hash.Add("PostPage", PostPage)
        'hash.Add("Download", ForDownload)
        'hash.Add("CommunityID", CommunityID)
        'hash.Add("expire", Now.AddMinutes(minutes))
        'hash.Add("domain", domain)
        'secured = SecuredCookie.encode_cookie("LogoutAccess", domain, minutes, hash)

        'If (From k In Response.Cookies.AllKeys Where k = "LogoutAccess").Any Then
        '    Response.Cookies.Set(secured)
        'Else
        '    Response.Cookies.Add(secured)
        'End If
    End Sub
    Protected Function ReadLogoutAccessCookie(ByVal PersonID As Integer, ByVal PersonLogin As String) As dtoLogoutAccess
        Dim iResponse As New dtoLogoutAccess
        Dim oValues As New Hashtable
        Try
            For Each key As String In Request.Cookies("LogoutAccess").Values.Keys
                oValues(key) = Request.Cookies("LogoutAccess").Values(key)
            Next
            '    SecuredCookie.decode_cookie(Request.Cookies("LogoutAccess"))
        Catch ex As Exception

        End Try
        If Not IsNothing(oValues) Then
            iResponse.CommunityID = oValues.Item("CommunityID")
            iResponse.PersonID = oValues.Item("PersonID")
            iResponse.PersonLogin = oValues.Item("PersonLogin")
            iResponse.isDownloading = oValues.Item("Download")
            If (iResponse.PersonID > 0 AndAlso (iResponse.PersonID = PersonID AndAlso iResponse.PersonLogin = PersonLogin)) OrElse iResponse.PersonID <= 0 Then
                If Not String.IsNullOrEmpty(oValues.Item("PostPage")) Then
                    iResponse.PageUrl = Replace(oValues.Item("PostPage"), "#_#", "&")
                End If
            Else
                iResponse.PageUrl = ""
            End If
        End If
        Return iResponse
        'Dim iResponse As New dtoLogoutAccess
        'Dim oValues As New Hashtable
        'Try
        '    oValues = SecuredCookie.decode_cookie(Request.Cookies("LogoutAccess"))
        'Catch ex As Exception

        'End Try
        'If Not IsNothing(oValues) Then
        '    iResponse.CommunityID = oValues.Item("CommunityID")
        '    iResponse.PersonID = oValues.Item("PersonID")
        '    iResponse.PersonLogin = oValues.Item("PersonLogin")
        '    iResponse.PageUrl = oValues.Item("PostPage")
        '    iResponse.isDownloading = oValues.Item("Download")
        '    If (iResponse.PersonID > 0 AndAlso (iResponse.PersonID = PersonID AndAlso iResponse.PersonLogin = PersonLogin)) OrElse iResponse.PersonID <= 0 Then
        '        iResponse.PageUrl = oValues.Item("Download")
        '    Else
        '        iResponse.PageUrl = ""
        '    End If
        'End If
        'Return iResponse
    End Function
    Protected Sub ClearLogoutAccessCookie()
        Response.Cookies("LogoutAccess").Expires = Now.AddDays(-1)
    End Sub
    Protected Shared Function removeBRfromStringEnd(ByRef value As String) As String
        value = value.Trim
        If value.EndsWith("<br>") Then
            Return removeBRfromStringEnd(value.Remove(value.Length - 4)) 'vengono rimossi anche piu' br consecutivi
        Else
            Return value
        End If
    End Function


End Class