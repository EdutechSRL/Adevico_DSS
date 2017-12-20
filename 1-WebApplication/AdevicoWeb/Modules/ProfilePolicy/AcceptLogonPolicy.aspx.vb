Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.PolicyManagement
Imports lm.Comol.Core.BaseModules.PolicyManagement.Presentation

Public Class AcceptLogonPolicy
    Inherits PageBase
    Implements IViewAcceptLogonPolicy


#Region "Context"
    Private _Presenter As AcceptLogonPolicyPresenter
    Public ReadOnly Property CurrentPresenter() As AcceptLogonPolicyPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AcceptLogonPolicyPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property PreloggedUserId As Integer Implements IViewAcceptLogonPolicy.PreloggedUserId
        Get
            Return Me.PageUtility.PreloggedUserId
        End Get
        Set(value As Integer)
            Me.PageUtility.PreloggedUserId = value
        End Set
    End Property
    Public Property PreloggedProviderId As Long Implements IViewAcceptLogonPolicy.PreloggedProviderId
        Get
            Return Me.PageUtility.PreloggedProviderId
        End Get
        Set(value As Long)
            Me.PageUtility.PreloggedProviderId = value
        End Set
    End Property
    Public Property PreloggedProviderUrl As String Implements IViewAcceptLogonPolicy.PreloggedProviderUrl
        Get
            Return Me.PageUtility.PreloggedProviderUrl
        End Get
        Set(value As String)
            Me.PageUtility.PreloggedProviderUrl = value
        End Set
    End Property
    Public Property LoggedUserId As Integer Implements IViewAcceptLogonPolicy.LoggedUserId
        Get
            Return ViewStateOrDefault("LoggedUserId", CInt(0))
        End Get
        Set(value As Integer)
            Me.ViewState("LoggedUserId") = value
        End Set
    End Property
    Public Property LoggedProviderId As Long Implements IViewAcceptLogonPolicy.LoggedProviderId
        Get
            Return ViewStateOrDefault("LoggedProviderId", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("LoggedProviderId") = value
        End Set
    End Property
    Public Property LoggedProviderUrl As String Implements IViewAcceptLogonPolicy.LoggedProviderUrl
        Get
            Return ViewStateOrDefault("LoggedProviderUrl", "")
        End Get
        Set(value As String)
            Me.ViewState("LoggedProviderUrl") = value
        End Set
    End Property
#End Region

    'Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
    '    If (ViewState(Key) Is Nothing) Then
    '        ViewState(Key) = DefaultValue
    '        Return DefaultValue
    '    Else
    '        Return ViewState(Key)
    '    End If
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AcceptLogonPolicy", "Modules", "ProfilePolicy")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTtitlePolicy)
            .setButton(BTNsavePolicy, True, , , True)
        End With
    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub GotoInternalAuthenticationPage() Implements IViewAcceptLogonPolicy.GotoInternalAuthenticationPage
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False))
    End Sub
    Public Sub GotoInternalShibbolethAuthenticationPage() Implements IViewAcceptLogonPolicy.GotoInternalShibbolethAuthenticationPage
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalShibbolethAuthenticationPage(False))
    End Sub
    Public Sub GotoShibbolethAuthenticationPage() Implements IViewAcceptLogonPolicy.GotoShibbolethAuthenticationPage
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.ShibbolethLogin(False))
    End Sub
    Public Sub LoadPolicySubmission(idUser As Integer) Implements IViewAcceptLogonPolicy.LoadPolicySubmission
        Me.CTRLpolicy.Visible = True
        Me.CTRLpolicy.InitializeControl(idUser)
    End Sub
    Public Sub LogonUser(user As Person, idDefaultCommunity As Integer, idProvider As Long, ByVal url As String) Implements IViewAcceptLogonPolicy.LogonUser
        Me.PageUtility.LogonUser(user, idDefaultCommunity, idProvider, url)
    End Sub
    Public Sub GotoExternalUrl(url As String) Implements IViewAcceptLogonPolicy.GotoExternalUrl
        If Not String.IsNullOrEmpty(url) Then
            Response.Redirect(url)
        End If
    End Sub
#End Region


    Private Sub BTNsavePolicy_Click(sender As Object, e As System.EventArgs) Handles BTNsavePolicy.Click
        Me.CTRLpolicy.SaveItems()
    End Sub

    Private Sub CTRLpolicy_AllMandatoryAccepted() Handles CTRLpolicy.AllMandatoryAccepted
        Me.CurrentPresenter.AcceptPolicy()
    End Sub

  
End Class