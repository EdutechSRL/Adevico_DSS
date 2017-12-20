Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement

Public Class UC_AuthenticationStepAuthenticationTypes
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property AvailableProvidersId As List(Of Long)
        Get
            Return (From l As ListItem In Me.RBLauthenticationProviders.Items Select CLng(l.Value)).ToList
        End Get
    End Property
    Public Property SelectedProviderId As Long
        Get
            If Me.RBLauthenticationProviders.SelectedIndex < 0 Then
                Return 0
            Else
                Return Me.RBLauthenticationProviders.SelectedValue
            End If
        End Get
        Set(value As Long)
            Try
                Me.RBLauthenticationProviders.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public ReadOnly Property SelectedProviderType As lm.Comol.Core.Authentication.AuthenticationProviderType
        Get
            If AvailableProviders.Count = 0 Then
                Return lm.Comol.Core.Authentication.AuthenticationProviderType.Internal
            Else
                Return (From p As dtoBaseProvider In AvailableProviders Where p.IdProvider = SelectedProviderId Select p.Type).FirstOrDefault
            End If
        End Get
    End Property
    Public ReadOnly Property SelectedProviderAllowAdminProfileInsert As Boolean
        Get
            If AvailableProviders.Count = 0 Then
                Return False
            Else
                Return (From p As dtoBaseProvider In AvailableProviders Where p.IdProvider = SelectedProviderId Select p.AllowAdminProfileInsert).FirstOrDefault
            End If
        End Get
    End Property
    Private Property AvailableProviders As List(Of dtoBaseProvider)
        Get
            Return ViewStateOrDefault("AvailableProviders", New List(Of dtoBaseProvider))
        End Get
        Set(value As List(Of dtoBaseProvider))
            ViewState("AvailableProviders") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBauthenticationProviders_t)
        End With
    End Sub
#End Region


#Region "Initialize Control"
    Public Sub InitializeControl(providers As List(Of dtoBaseProvider), idProvider As Long)
        AvailableProviders = providers
        Me.RBLauthenticationProviders.Items.Clear()
        For Each provider As dtoBaseProvider In providers
            Me.RBLauthenticationProviders.Items.Add(New ListItem(provider.Translation.Name, provider.IdProvider))
        Next

        If idProvider = 0 AndAlso providers.Count > 0 Then
            Me.RBLauthenticationProviders.SelectedIndex = 0
        ElseIf (From p As dtoBaseProvider In providers Where p.IdProvider = idProvider Select p).Any Then
            Me.RBLauthenticationProviders.SelectedValue = idProvider
        ElseIf providers.Count > 0 Then
            Me.RBLauthenticationProviders.SelectedIndex = 0
        End If
        Me.isInitialized = True
    End Sub
#End Region


End Class