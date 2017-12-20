Imports lm.Comol.Core.BaseModules.ModulesLoader.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Partial Public Class LoadForum
    Inherits PageBase
    Implements IViewGenericUserSessionExpired


#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Private _baseManager As lm.Comol.Core.Business.BaseModuleManager
    Private ReadOnly Property CurrentManager() As lm.Comol.Core.Business.BaseModuleManager
        Get
            If IsNothing(_baseManager) Then
                _baseManager = New lm.Comol.Core.Business.BaseModuleManager(CurrentContext)
            End If
            Return _baseManager
        End Get
    End Property
    Private _Presenter As GenericUserSessionExpiredPresenter
    Public ReadOnly Property CurrentPresenter() As GenericUserSessionExpiredPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GenericUserSessionExpiredPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Public Overrides Sub BindDati()
        Dim idUser As Integer = PreLoadedIdUser

        If CurrentContext.UserContext.isAnonymous Then
            Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
            dto.IdCommunity = PreLoadedCommunityID
            dto.IdPerson = PreLoadedIdUser
            dto.DestinationUrl = "Forum/LoadForum.aspx" & Request.Url.Query

            Me.CurrentPresenter.InitView(idUser, dto)
            'Dim defaultUrl As String = PageUtility.BaseUrl & Me.SystemSettings.Presenter.DefaultStartPage
            'Try
            '    If idUser > 0 Then
            '        Dim person As lm.Comol.Core.DomainModel.Person = CurrentManager.GetPerson(idUser)
            '        If Not IsNothing(person) Then
            '            If person.IdDefaultProvider > 0 Then

            '            End If
            '        End If
            '    End If
            'Catch ex As Exception

            'End Try
            'Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(defaultUrl)

            'webPost.Redirect(dto)
        Else
            Dim access As lm.Comol.Core.DomainModel.SubscriptionStatus = lm.Comol.Core.DomainModel.SubscriptionStatus.none
            Dim idCommunity As Integer = Me.PreLoadedCommunityID
            idUser = PageUtility.CurrentUser.ID

            Dim url As String = GetForumActiveUrl(idUser, idCommunity)
            If Me.ComunitaCorrenteID <> idCommunity Then
                Dim oResourceConfig As New ResourceManager
                oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                access = PageUtility.AccessToCommunity(idUser, idCommunity, oResourceConfig, url, True)
            ElseIf Not String.IsNullOrEmpty(url) Then
                PageUtility.RedirectToUrl(url)
            End If
        End If

    End Sub

    Private Enum PageToLoad
        ForumList = 1
        TopicsList = 2
        PostList = 3
        SelectedPost = 4
    End Enum
    Private Function GetForumActiveUrl(ByVal idUser As Integer, ByVal idCommunity As Integer)
        Dim url As String = ""
        Dim oStringaSelezione As PageToLoad
        If Me.PreLoadedForumID > 0 AndAlso Me.PreLoadedTopicID > 0 AndAlso Me.PreLoadedPostID > 0 Then
            oStringaSelezione = PageToLoad.SelectedPost
        ElseIf Me.PreLoadedForumID > 0 AndAlso Me.PreLoadedTopicID > 0 Then
            oStringaSelezione = PageToLoad.PostList
        ElseIf Me.PreLoadedForumID > 0 Then
            oStringaSelezione = PageToLoad.TopicsList
        Else
            oStringaSelezione = PageToLoad.ForumList
        End If

        Dim oForum As New COL_Forums

        oForum.Id = Me.PreLoadedForumID
        oForum.Estrai()

        Dim community As lm.Comol.Core.DomainModel.Community
        Try
            Dim forumModule As UCServices.Services_Forum = UCServices.Services_Forum.Create
            forumModule.PermessiAssociati = lm.Comol.Core.DomainModel.PermissionHelper.LongToBinStr(CurrentManager.GetModulePermission(idUser, idCommunity, CurrentManager.GetModuleID(UCServices.Services_Forum.Codex)))

            community = CurrentManager.GetCommunity(idCommunity)

            If oForum.Hide = 1 Or (forumModule.GestioneForum = False And forumModule.AccessoForum = False) Then
                url = "Forum/forums.aspx"
            ElseIf oStringaSelezione = PageToLoad.ForumList Then
                If oForum.isArchiviato Then
                    url = "Forum/forums.aspx?sel=" & Main.FiltroArchiviazione.Archiviato & "#forum_" & Me.PreLoadedForumID
                Else
                    url = "Forum/forums.aspx?sel=" & Main.FiltroArchiviazione.NonArchiviato & "#forum_" & Me.PreLoadedForumID
                End If
            Else
                Dim oThread As New COL_Forum_threads
                oThread.Id = Me.PreLoadedTopicID

                Dim isVisibileTopic As Boolean
                Dim isAdmin As Boolean = False
                Dim RuoloForum As Main.RuoloForumStandard

                RuoloForum = oForum.getRuoloForIscritto(Session("RLPC_ID"), False)

                isAdmin = (forumModule.GestioneForum Or RuoloForum = Main.RuoloForumStandard.Amministratore Or RuoloForum = Main.RuoloForumStandard.Moderatore)

                If Not isAdmin Then
                    isVisibileTopic = oThread.isVisibile(Me.PreLoadedTopicID)
                Else
                    isVisibileTopic = True
                End If

                If oStringaSelezione = PageToLoad.TopicsList Or (forumModule.GestioneForum = False AndAlso Not isVisibileTopic) Then
                    Session("IdForum") = oForum.Id
                    Session("ForumIsArchiviato") = oForum.isArchiviato
                    Session("NomeForum") = oForum.Name
                    Session("RuoloForum") = oForum.getRuoloForIscritto(Session("RLPC_ID"), False)

                    If oThread.isArchiviato() Then
                        url = "Forum/ForumThreads.aspx?sel=" & Main.FiltroArchiviazione.Archiviato & "#topic_" & Me.PreLoadedTopicID
                    Else
                        url = "Forum/ForumThreads.aspx?sel=" & Main.FiltroArchiviazione.NonArchiviato & "#topic_" & Me.PreLoadedTopicID
                    End If
                ElseIf (oStringaSelezione = PageToLoad.PostList OrElse oStringaSelezione = PageToLoad.SelectedPost) AndAlso isVisibileTopic Then
                    Session("IdForum") = oForum.Id
                    Session("ForumIsArchiviato") = oForum.isArchiviato
                    Session("NomeForum") = oForum.Name

                    Session("RuoloForum") = oForum.getRuoloForIscritto(Session("RLPC_ID"), True)

                    Session("IdThread") = Me.PreLoadedTopicID
                    Session("NomeThread") = oThread.EstraiSubject(Me.PreLoadedTopicID)
                    Session("ThreadArchiviato") = oThread.isArchiviato()

                    If oStringaSelezione = PageToLoad.SelectedPost Then
                        url = "Forum/ElencoPost.aspx?#post_" & Me.PreLoadedPostID
                    Else
                        url = "Forum/ElencoPost.aspx"
                    End If
                Else
                    If oForum.isArchiviato Then
                        url = "Forum/forums.aspx?sel=" & Main.FiltroArchiviazione.Archiviato & "#forum_" & Me.PreLoadedForumID
                    Else
                        url = "Forum/forums.aspx?sel=" & Main.FiltroArchiviazione.NonArchiviato & "#forum_" & Me.PreLoadedForumID
                    End If
                End If
            End If
        Catch ex As Exception

        End Try

        '' DEVO RECUPERARE I PERMESSI !!!
        'Dim oComunita As New COL_Comunita
        'Dim oServizio As UCServices.Services_Forum = UCServices.Services_Forum.Create
        'Try
        '    oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByCode(Session("idRuolo"), Session("idComunita"), oServizio.Codex)
        '    If oServizio.PermessiAssociati = "" Then
        '        oServizio.PermessiAssociati = "00000000000000000000000000000000"
        '    End If
        'Catch ex As Exception
        '    oServizio.PermessiAssociati = "00000000000000000000000000000000"
        'End Try

        Return url
    End Function


#Region "INUTILI"
    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region


#Region "From Url"
    Private _CommunityID As Integer
    Private _ForumID As Integer
    Private _TopicID As Integer
    Private _PostID As Integer
    Public ReadOnly Property PreLoadedCommunityID() As Integer
        Get
            Try
                If _CommunityID = 0 Then
                    _CommunityID = CInt(Request.QueryString("CommunityID"))
                End If
            Catch ex As Exception

            End Try
            Return _CommunityID
        End Get
    End Property
    Public ReadOnly Property PreLoadedForumID() As Integer
        Get
            Try
                If _ForumID = 0 Then
                    _ForumID = CInt(Request.QueryString("ForumID"))
                End If
            Catch ex As Exception

            End Try
            Return _ForumID
        End Get
    End Property
    Public ReadOnly Property PreLoadedTopicID() As Integer
        Get
            Try
                If _TopicID = 0 Then
                    _TopicID = CInt(Request.QueryString("TopicID"))
                End If
            Catch ex As Exception

            End Try
            Return _TopicID
        End Get
    End Property
    Public ReadOnly Property PreLoadedPostID() As Integer
        Get
            Try
                If _PostID = 0 Then
                    _PostID = CInt(Request.QueryString("PostID"))
                End If
            Catch ex As Exception

            End Try
            Return _PostID
        End Get
    End Property

    Private ReadOnly Property LoadForumList() As Boolean
        Get
            Return Me.PreLoadedForumID = -1
        End Get
    End Property
    Private ReadOnly Property LoadTopicList() As Boolean
        Get
            Return Me.PreLoadedForumID > 0
        End Get
    End Property
    Private ReadOnly Property LoadPosts() As Boolean
        Get
            Return Me.PreLoadedForumID > 0 AndAlso Me.PreLoadedTopicID > 0
        End Get
    End Property

    Private ReadOnly Property PreLoadedIdUser() As Integer
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return Request.QueryString("IdUser")
            Else
                Return 0
            End If

        End Get
    End Property
#End Region

#Region "Implements"
    Public Sub GoToDefaultPage() Implements IViewGenericUserSessionExpired.GoToDefaultPage
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage, SystemSettings.Login.isSSLloginRequired)
    End Sub
    Public Sub LoadExternalProviderPage(url As String) Implements IViewGenericUserSessionExpired.LoadExternalProviderPage
        Response.Redirect(url)
    End Sub
    Public Sub LoadInternalLoginPage() Implements IViewGenericUserSessionExpired.LoadInternalLoginPage
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False), SystemSettings.Login.isSSLloginRequired)
    End Sub
    'Public Sub LoadOldAuthenticationPage(idAuthenticationType As Integer) Implements IViewGenericUserSessionExpired.LoadOldAuthenticationPage
    '    Dim RemoteUrl As String = ""
    '    Select Case IdAuthenticationType
    '        Case Main.TipoAutenticazione.IOP
    '            RemoteUrl = (From o In Me.SystemSettings.UrlProviders Where o.ComolID = IdAuthenticationType Select o.RemoteLogin).FirstOrDefault

    '    End Select
    '    If RemoteUrl = "" Then
    '        RemoteUrl = Me.PageUtility.DefaultUrl
    '    End If
    '    Me.Response.Redirect(RemoteUrl, True)
    'End Sub

    Public Sub WriteLogonCookies(item As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl) Implements IViewGenericUserSessionExpired.WriteLogonCookies
        WriteLogoutAccessCookie(item)
    End Sub
#End Region
End Class