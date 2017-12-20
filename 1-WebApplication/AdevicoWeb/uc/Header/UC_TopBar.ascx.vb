Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.Header
Imports lm.Comol.Modules.Standard.Header.Presentation

Public Class UC_TopBar
    Inherits BaseControl
    Implements Presentation.IViewTopBar


#Region "Context"
    Private _presenter As Presentation.TopBarPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As Presentation.TopBarPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New Presentation.TopBarPresenter(Me.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CurrentPresenter.InitView(Domain.DisplayNameMode.namesurname) 'PageUtility.SystemSettings.Presenter.DefaultDisplayNameMode)
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_TopBar", "UC")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"
    ''' <summary>
    ''' Effetual il render della topbar, richiamando la funzione in TopBarSetting
    ''' </summary>
    ''' <remarks>
    ''' Active Service:
    ''' 7   SRVTESI      Tesi
    ''' 36  SRVCRCLA     Curriculum
    ''' 28  SRVELNC_CMNT Elenca sotto comunità (eventualmente modificare o lasciare vuoto, se indipendente da servizio
    ''' 19  SRVLINK      Servizio link
    ''' </remarks>
    Public Sub LoadTopBar(ByVal UserName As String, ByVal providerTypes As List(Of Integer), ByVal IdProfileType As Integer, ByVal IdLanguage As Integer, ByVal LanguageName As String, ByVal availableModules As List(Of String)) Implements IViewTopBar.LoadTopBar

        Dim TopBarSettings As TopBarSetting = MyBase.SystemSettings.TopBar

        Dim RenderSettings As New RenderParameter_DTO() With { _
            .ActiveServices = availableModules.ToArray, _
            .AutenticationProviderTypes = providerTypes, _
            .Welcome_t = Resource.getValue("Welcome"), _
            .Help_t = Resource.getValue("Help"), _
            .Home_t = Resource.getValue("Home"), _
            .Profilo_t = Resource.getValue("Profile"), _
            .Tool_t = Resource.getValue("Tools"), _
            .Logout_t = Resource.getValue("LogOut"), _
            .LanguageDef_t = LanguageName, _
            .LanguageId = IdLanguage, _
            .PersonTypeId = IdProfileType, _
            .UserName = UserName, _
            .BaseUrl = Me.PageUtility.ApplicationUrlBase, _
            .BaseUrlHttps = Me.PageUtility.SecureApplicationUrlBase, _
            .CurrentLanguageId = IdLanguage
            }
        '.Language_t = Resource.getValue("Languages"), _
        'Intanto SOLO per MVC, poi verrà sistemato...
        If Not IsNothing(TopBarSettings) Then
            Me.LTtopBar.Text = TopBarSettings.Render(RenderSettings)
        End If


    End Sub
    Private Function GetRenderTopBar(userName As String, providerTypes As System.Collections.Generic.List(Of Integer), idProfileType As Integer, idLanguage As Integer, languageName As String, availableModules As System.Collections.Generic.List(Of String)) As String Implements lm.Comol.Modules.Standard.Header.Presentation.IViewTopBar.GetRenderTopBar
        Dim TopBarSettings As TopBarSetting = MyBase.SystemSettings.TopBar

        Dim RenderSettings As New RenderParameter_DTO() With { _
            .ActiveServices = availableModules.ToArray, _
            .AutenticationProviderTypes = providerTypes, _
            .Welcome_t = Resource.getValue("Welcome"), _
            .Help_t = Resource.getValue("Help"), _
            .Home_t = Resource.getValue("Home"), _
            .Profilo_t = Resource.getValue("Profile"), _
            .Tool_t = Resource.getValue("Tools"), _
            .Logout_t = Resource.getValue("LogOut"), _
            .LanguageDef_t = languageName, _
            .LanguageId = idLanguage, _
            .PersonTypeId = idProfileType, _
            .UserName = userName, _
            .BaseUrl = Me.PageUtility.ApplicationUrlBase, _
            .BaseUrlHttps = Me.PageUtility.SecureApplicationUrlBase, _
            .CurrentLanguageId = idLanguage
            }
        '.Language_t = Resource.getValue("Languages"), _
        'Intanto SOLO per MVC, poi verrà sistemato...
        If Not IsNothing(TopBarSettings) Then
            Return TopBarSettings.Render(RenderSettings)
        Else
            Return ""
        End If
    End Function

    Private Sub RenderTopBar(render As String) Implements lm.Comol.Modules.Standard.Header.Presentation.IViewTopBar.RenderTopBar
        Me.LTtopBar.Text = render
    End Sub
    Public Sub LoadUnregisteredTopBar() Implements IViewTopBar.LoadUnregisteredTopBar
        Me.LTtopBar.Text = ""
    End Sub
#End Region

   
End Class