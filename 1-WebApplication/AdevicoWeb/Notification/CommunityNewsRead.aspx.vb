Imports lm.Modules.NotificationSystem.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract


Partial Public Class CommunityNewsRead
    Inherits PageBase
    Implements IViewServiceCommunityNewsReadLoader
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Base"
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
    Public Overrides Sub BindDati()
        If Not Me.Page.IsPostBack Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityLastUpdates", "Notification")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("titolo.NewsReader")
            .setLiteral(Me.LTnoCommunityAccess)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Private _CommunityID As Integer
    Private _PreLoadedPageUrl As String
    Private _PreLoadedPreviousPageUrl As String
    Private _PreLoadedPreviousUrl As String
    Private _PreLoadedNewsID As System.Guid
    Private _Presenter As ServiceCommunityNewsReadLoaderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String


#Region "Base Context"
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As ServiceCommunityNewsReadLoaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ServiceCommunityNewsReadLoaderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region


    Public ReadOnly Property PreLoadedCommunityID() As Integer Implements IViewServiceCommunityNewsReadLoader.PreLoadedCommunityID
        Get
            Try
                If _CommunityID = 0 Then
                    _CommunityID = CInt(PageUtility.DecryptQueryString("CommunityID", UtilityLibrary.SecretKeyUtil.EncType.Altro))
                End If
            Catch ex As Exception

            End Try
            Return _CommunityID
        End Get
    End Property
    Public ReadOnly Property PreLoadedPageUrl() As String Implements IViewServiceCommunityNewsReadLoader.PreLoadedPageUrl
        Get
            If _PreLoadedPageUrl = "" Then
                _PreLoadedPageUrl = PageUtility.DecryptQueryString("DestinationUrl", UtilityLibrary.SecretKeyUtil.EncType.Altro)
            End If
            Return _PreLoadedPageUrl
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousUrl() As String Implements IViewServiceCommunityNewsReadLoader.PreLoadedPreviousUrl
        Get
            If _PreLoadedPreviousUrl = "" Then
                _PreLoadedPreviousUrl = PageUtility.DecryptQueryString("FromUrl", UtilityLibrary.SecretKeyUtil.EncType.Altro)
            End If
            Return _PreLoadedPreviousUrl
        End Get
    End Property
    Public ReadOnly Property PreLoadedNewsID() As System.Guid Implements IViewServiceCommunityNewsReadLoader.PreLoadedNewsID
        Get
            If Request.QueryString.HasKeys Then
                Dim NewsID As String = Me.DecryptQueryString("NewsID", UtilityLibrary.SecretKeyUtil.EncType.Altro)
                If Not String.IsNullOrEmpty(NewsID) Then
                    Try
                        _PreLoadedNewsID = New System.Guid(NewsID)
                    Catch ex As Exception

                    End Try
                End If
            End If
            Return _PreLoadedNewsID
        End Get
    End Property
    'Public Sub NavigateToCommunityUrl(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal url As String) Implements IViewServiceCommunityNewsReadLoader.NavigateToCommunityUrl
    '    Dim oResourceConfig As New ResourceManager
    '    oResourceConfig = GetResourceConfig(Me.LinguaCode)

    '    Me.PageUtility.AccessToCommunity(PersonID, CommunityID, oResourceConfig, Me.PageUtility.GetUrlDecoded(url), False)
    'End Sub

    Public WriteOnly Property PreviousUrl() As String Implements IViewServiceCommunityNewsReadLoader.PreviousUrl
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Me.HYPbackHistory.Visible = False
            Else
                Me.HYPbackHistory.Visible = True
                Me.HYPbackHistory.NavigateUrl = Me.BaseUrl & value
            End If
        End Set
    End Property

    Public Sub ShowNoCommunityAccess(ByVal CommunityName As String) Implements IViewServiceCommunityNewsReadLoader.ShowNoCommunityAccess
        Me.LTnoCommunityAccess.Visible = True
        Me.Resource.setLiteral(Me.LTnoCommunityAccess)
        Me.LTnoCommunityAccess.Text = String.Format(Me.LTnoCommunityAccess.Text, CommunityName)
    End Sub

    Public Sub NoPermissionToAccess() Implements IViewServiceCommunityNewsReadLoader.NoPermissionToAccess
        Master.ShowNoPermission = True
    End Sub

    Public Sub NavigateToUrl(ByVal url As String) Implements IViewServiceCommunityNewsReadLoader.NavigateToUrl
        Me.RedirectToUrl(Me.PageUtility.GetUrlDecoded(url))
    End Sub



    Public Sub SetAllNewsRead(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal url As String) Implements lm.Modules.NotificationSystem.Presentation.IViewServiceCommunityNewsReadLoader.SetAllNewsRead
        Me.ReadAllNews(PersonID, CommunityID, url)
    End Sub

    Public Sub SetNewsRead(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal NewsID As System.Guid, ByVal url As String) Implements lm.Modules.NotificationSystem.Presentation.IViewServiceCommunityNewsReadLoader.SetNewsRead
        Me.RedirectToUrl(Me.PageUtility.GetUrlDecoded(url))
    End Sub


    Private Sub ReadAllNews(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal LoadUrl As String)
        Dim iResponse As lm.Comol.Core.DomainModel.SubscriptionStatus = lm.Comol.Core.DomainModel.SubscriptionStatus.none
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim RoleID As Integer = Main.TipoRuoloStandard.AccessoNonAutenticato

        Try
            oTreeComunita.Directory = PageUtility.ProfilePath & PersonID & "\"
            oTreeComunita.Nome = PersonID & ".xml"
        Catch ex As Exception

        End Try

        Try
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Me.LinguaCode)
            Dim oIscrizione As New COL_RuoloPersonaComunita
            Dim UpdateDate As DateTime = Now
            oIscrizione.Estrai(CommunityID, PersonID)
            If oIscrizione.Abilitato AndAlso oIscrizione.Attivato And (oIscrizione.TipoRuolo.Id = Main.TipoRuoloStandard.AccessoNonAutenticato Or oIscrizione.TipoRuolo.Id > 0) Then
                If Me.Session("LogonAs") = False Then
                    oIscrizione.UpdateUltimocollegamento()
                End If
                Dim oComunita As New COL_Comunita With {.Id = CommunityID}
                oComunita.RegistraAccesso(CommunityID, PersonID, oResourceConfig.getValue("systemDBcodice"))
                UpdateDate = oIscrizione.UltimoCollegamento
            End If
            Me.PageUtility.SendNotificationUpdateCommunityAccess(PersonID, CommunityID, UpdateDate)
        Catch ex As Exception

        End Try
        Me.RedirectToUrl(Me.PageUtility.GetUrlDecoded(LoadUrl))
    End Sub

End Class