Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comol.Manager
Imports COL_BusinessLogic_v2.Comol.Entities


Public MustInherit Class BaseControl
    Inherits System.Web.UI.UserControl


    Private _Resource As ResourceManager
    Private _ServiziCorrenti As ServiziCorrenti
    Private _Instance As Boolean
    Protected _Language As Comol.Entity.Lingua
    Private _PageUtility As PresentationLayer.OLDpageUtility
    Private _BaseUrl As String
    Protected ReadOnly Property BaseUrl() As String
        Get
            If String.IsNullOrEmpty(_BaseUrl) Then
                _BaseUrl = PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
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
    Public ReadOnly Property PreLoadedContentView As lm.Comol.Core.DomainModel.ContentView
        Get
            If IsNumeric(Request.QueryString("CV")) Then
                Try
                    Return Request.QueryString("CV")
                Catch ex As Exception
                    Return lm.Comol.Core.DomainModel.ContentView.viewAll
                End Try
            Else
                Return lm.Comol.Core.DomainModel.ContentView.viewAll
            End If
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
    'Public MustOverride Sub BindDati()

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.SetCultureSettings()
    End Sub

    Private _SystemSettings As ComolSettings
    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            If IsNothing(_SystemSettings) Then
                _SystemSettings = ManagerConfiguration.GetInstance
            End If
            Return _SystemSettings
        End Get
    End Property

    Public ReadOnly Property ObjectPath(ByVal oPath As ConfigurationPath) As ObjectFilePath
        Get
            Dim oObjectPath As New ObjectFilePath(oPath.isOnThisServer)

            Try
                If oPath.isOnThisServer Then
                    oObjectPath.Virtual = Me.PageUtility.BaseUrl & oPath.VirtualPath ' Me.BaseUrl & "/" & oPath.VirtualPath
                    oObjectPath.Virtual = Replace(oObjectPath.Virtual, "//", "/")
                    If oPath.DrivePath <> "" Then
                        oObjectPath.Drive = oPath.DrivePath 'Me.BaseUrlDrivePath & oPath.DrivePath
                    Else
                        oObjectPath.Drive = Me.PageUtility.BaseUrlDrivePath & oPath.VirtualPath
                    End If
                    oObjectPath.Drive = Replace(oObjectPath.Drive, "/", "\")
                    oObjectPath.Drive = Replace(oObjectPath.Drive, "\\", "\")
                Else
                    If oPath.ServerVirtualPath <> "" Then
                        oObjectPath.Virtual = oPath.ServerVirtualPath & oPath.VirtualPath
                    End If
                    'oObjectPath.Virtual = oPath.ServerVirtualPath & oPath.VirtualPath 'oPath.ServerVirtualPath & "/" & oPath.VirtualPath
                    If oPath.DrivePath <> "" Then
                        oObjectPath.Drive = oPath.ServerPath & oPath.DrivePath
                    Else
                        oObjectPath.Drive = oPath.ServerPath & oPath.VirtualPath
                    End If
                    oObjectPath.SharePath = oPath.ServerPath
                End If
                oObjectPath.isOnShare = Not oPath.isOnThisServer
                oObjectPath.RewritePath = oPath.RewritePath
            Catch ex As Exception

            End Try
            Return oObjectPath
        End Get
    End Property
    Protected Sub WriteLogoutAccessCookie(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostPage As String, ByVal ForDownload As Boolean)
        Dim oHttpCookie As New HttpCookie("LogoutAccess")
        Dim minutes As Long = 5
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
    End Function
    Protected Sub ClearLogoutAccessCookie()
        Response.Cookies("LogoutAccess").Expires = Now.AddDays(-1)
    End Sub


    'Public MustOverride ReadOnly Property AlwaysBind() As Boolean
    Public MustOverride ReadOnly Property VerifyAuthentication() As Boolean
    Protected Overridable Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not VerifyAuthentication OrElse Not IsSessioneScaduta() Then
            If Page.IsPostBack = False Then
                Me.SetInternazionalizzazione()
            End If
            'If AlwaysBind Then
            '    Me.BindDati()
            'ElseIf Page.IsPostBack = False Then
            '    Me.BindDati()
            'End If
        ElseIf Not VerifyAuthentication AndAlso IsSessioneScaduta() Then

        End If
    End Sub

    'Private Sub CommunityRepositoryItemError_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    '   If Me.CurrentContext.UserContext.isAnonymous Then

    '    End If
    Protected Function IsSessioneScaduta() As Boolean
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

    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function

    Protected Sub OverloadLanguage(ByVal oLingua As Lingua)
        Session("LinguaID") = oLingua.ID
        Session("LinguaCode") = oLingua.Codice
        Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = oLingua.ID, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}
        _Language = oLingua
    End Sub


    ''' <summary>
    ''' Data un oggetto lingua, lo aggiorna all'interno del base
    ''' e riessegue le funzioni per l'internazionalizzazione
    ''' </summary>
    ''' <param name="oLingua"></param>
    ''' <remarks>
    ''' NOTA PER AGGIORNAMENTI FUTURI:
    ''' E' PROTECTED perchè se la mettessi PUBLIC, non dovrei implementare nulla 
    ''' e la pagina potrebbe richiamare DIRETTAMENTE tale funzione per tutti i suoi controlli, MA
    ''' non verrebbe fatta alcuna inizializzazione sullo UserControl,
    ''' che rischierebbe di trovarsi con tutti gli elementi internazionalizzati da Bind di vario genere (es: liste)
    ''' in una lingua diversa dagli elementi "statici", che vengono cioè impostati in SetInternazionalizzazione.
    ''' Per QUESTO motivo, l'UC dovrà esporre una propria funzione PUBBLICA alla pagina che nel migliore dei casi,
    ''' richiama solamente il presente metodo, altrimenti conterrà anche tutti i suoi Bind. (DOPO aver lanciato QUESTO metodo).
    ''' L'alternativa è mettelo PUBBLICO ed AGGIUNGERE una funzione che sarà implementata dall'UC per l'aggiornamento dei contenuti,
    ''' MA avrebbe un impatto su TUTTO COMOL con risultati imprevedibili.
    ''' </remarks>
    Protected Sub UpdateLanguage(ByVal oLingua As Lingua)
        _Language = oLingua
        SetCultureSettings()
        SetInternazionalizzazione()
    End Sub

End Class