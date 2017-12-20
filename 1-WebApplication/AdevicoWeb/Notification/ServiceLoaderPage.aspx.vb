Imports lm.Modules.NotificationSystem.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.ModulesLoader.Presentation

Partial Public Class ServiceLoaderPage
    Inherits PageBase
    Implements IViewInternalModuleLoader


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
            Me.Master.ServiceTitle = .getValue("titolo.Loader")
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
    Private _PreLoadedPlainPageUrl As String
    Private _Presenter As InternalModuleLoaderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String

    Private _PageUtility As OLDpageUtility
    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

#Region "Base Context"
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As InternalModuleLoaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InternalModuleLoaderPresenter(Me.CurrentContext, Me)
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


    Public ReadOnly Property PreLoadedIdCommunity() As Integer Implements IViewInternalModuleLoader.PreLoadedIdCommunity
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
    Public ReadOnly Property PreLoadedPlainPageUrl As String Implements IViewInternalModuleLoader.PreLoadedPlainPageUrl
        Get
            If _PreLoadedPlainPageUrl = "" Then
                _PreLoadedPreviousUrl = PageUtility.DecryptQueryString("FromUrl", UtilityLibrary.SecretKeyUtil.EncType.Altro)
            End If
            Return _PreLoadedPreviousUrl
        End Get
    End Property
    Public ReadOnly Property PreLoadedPageUrl() As String Implements IViewInternalModuleLoader.PreLoadedPageUrl
        Get
            If _PreLoadedPageUrl = "" Then
                _PreLoadedPageUrl = PageUtility.DecryptQueryString("DestinationUrl", UtilityLibrary.SecretKeyUtil.EncType.Altro)
            End If
            Return _PreLoadedPageUrl
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousUrl() As String Implements IViewInternalModuleLoader.PreLoadedPreviousUrl
        Get
            If _PreLoadedPreviousUrl = "" Then
                _PreLoadedPreviousUrl = PageUtility.DecryptQueryString("FromPlainUrl", UtilityLibrary.SecretKeyUtil.EncType.Altro)
            End If
            Return _PreLoadedPreviousUrl
        End Get
    End Property

    Public Sub NavigateToCommunityUrl(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal url As String, ByVal Decode As Boolean) Implements IViewInternalModuleLoader.NavigateToCommunityUrl
        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(Me.LinguaCode)
        If Decode Then
            url = Me.PageUtility.GetUrlDecoded(url)
        End If
        If url.StartsWith(Me.BaseUrl) AndAlso Me.BaseUrl <> "/" Then
            url = Replace(url, Me.BaseUrl, "")
        End If
        Me.PageUtility.AccessToCommunity(PersonID, CommunityID, oResourceConfig, url, False)
    End Sub

    Public WriteOnly Property PreviousUrl() As String Implements IViewInternalModuleLoader.PreviousUrl
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Me.HYPbackHistory.Visible = False
            Else
                Me.HYPbackHistory.Visible = True
                Me.HYPbackHistory.NavigateUrl = Me.BaseUrl & value
            End If
        End Set
    End Property

    Public Sub ShowNoCommunityAccess(ByVal CommunityName As String) Implements IViewInternalModuleLoader.ShowNoCommunityAccess
        Me.LTnoCommunityAccess.Visible = True
        Me.Resource.setLiteral(Me.LTnoCommunityAccess)
        Me.LTnoCommunityAccess.Text = String.Format(Me.LTnoCommunityAccess.Text, CommunityName)
    End Sub

    Public Sub NoPermissionToAccess() Implements IViewInternalModuleLoader.NoPermissionToAccess
        Master.ShowNoPermission = True
    End Sub

    Public Sub NavigateToUrl(ByVal url As String, ByVal Decode As Boolean) Implements IViewInternalModuleLoader.NavigateToUrl
        Dim UrlDestination As String = IIf(Decode, Me.PageUtility.GetUrlDecoded(url), url)
        If UrlDestination.StartsWith(Me.BaseUrl) AndAlso Me.BaseUrl <> "/" Then
            UrlDestination = Replace(UrlDestination, Me.BaseUrl, "")
        End If
        Me.RedirectToUrl(UrlDestination)
    End Sub


    Public Function DecodeUrl(ByVal Url As String) As String Implements IViewInternalModuleLoader.DecodeUrl
        If String.IsNullOrEmpty(Url) Then
            Return Me.PageUtility.GetUrlDecoded(Url)
        Else
            Return ""
        End If
    End Function


    Public ReadOnly Property PortalName As String Implements IViewInternalModuleLoader.PortalName
        Get
            Return Resource.getValue("PortalName")
        End Get
    End Property
End Class