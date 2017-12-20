Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation
Imports lm.Comol.Core.Authentication
Public Class UC_InfoProvider
    Inherits BaseControl
    Implements IViewProviderInfo


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
    Private _Presenter As ProviderInfoPresenter
    Private ReadOnly Property CurrentPresenter() As ProviderInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProviderInfoPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean
        Get
            Return Me.ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("isInitialized") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AuthenticationProvider", "Modules", "ProviderManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBproviderName_t)
            .setLabel(LBuniqueCode_t)
            .setLabel(LBproviderType_t)
            .setLabel(LBdisplayToUser_t)

            .setLabel(LBallowAdminProfileInsert_t)

            .setLabel(LBallowMultipleInsert_t)

            .setLabel(LBidentifierFields_t)


            .setLabel(LBdateToChangePassword_t)

            .setLabel(LBsenderUrl_t)
            .setLabel(LBverifyRemoteUrl_t)

            .setLabel(LBremoteLoginUrl_t)
            .setLabel(LBurlIdentifier_t)
            .setLabel(LBdeltaTime_t)
            .setLabel(LBdeltaTime)
            .setLabel(LBtokenFormat_t)


            .setLabel(LBkey_t)
            .setLabel(LBinitializationVector_t)
            .setLabel(LBencryptionAlgorithm_t)

        End With
    End Sub

#End Region

    Public Sub InitializeControl(idProvider As Long) Implements IViewProviderInfo.InitializeControl
        Me.isInitialized = True
        Me.CurrentPresenter.InitView(idProvider)
    End Sub

    Public Sub LoadProvider(provider As dtoProvider) Implements IViewProviderInfo.LoadProvider
        Me.MLVcontrolData.SetActiveView(VIWdata)

        Me.LBproviderType.Text = Resource.getValue("AuthenticationProviderType." & provider.Type.ToString)
        Select Case provider.Type
            Case AuthenticationProviderType.Internal
                Dim days = DirectCast(provider, dtoInternalProvider).ChangePasswordAfterDays

                Me.LBdateToChangePassword.Text = Me.Resource.getValue("DDLpasswordDays." & (days / 30).ToString)
                Me.MLVtypes.SetActiveView(VIWinternalProvider)
            Case AuthenticationProviderType.Url
                Dim urlProvider As dtoUrlProvider = DirectCast(provider, dtoUrlProvider)
                With urlProvider
                    Me.LBsenderUrl.Text = .SenderUrl
                    Me.LBverifyRemoteUrl.Text = Me.Resource.getValue("Option." & urlProvider.VerifyRemoteUrl.ToString)
                    Me.LBremoteLoginUrl.Text = .RemoteLoginUrl
                    Me.LBurlIdentifier.Text = .UrlIdentifier
                    Me.LBdeltaTimeInfo.Text = .DeltaTime.TotalSeconds
                    Me.LBtokenFormat.Text = Resource.getValue("UrlUserTokenFormat." & urlProvider.TokenFormat.ToString)
                  
                    If IsNothing(.EncryptionInfo) Then
                        Me.LBkey.Text = ""
                        Me.LBinitializationVector.Text = ""
                        Me.LBencryptionAlgorithm.Text = Resource.getValue("EncryptionAlgorithm." & lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael.ToString)
                    Else
                        Me.LBkey.Text = .EncryptionInfo.Key
                        Me.LBinitializationVector.Text = .EncryptionInfo.InitializationVector
                        Me.LBencryptionAlgorithm.Text = Resource.getValue("EncryptionAlgorithm." & .EncryptionInfo.EncryptionAlgorithm.ToString)
                    End If
                End With
                Me.MLVtypes.SetActiveView(VIWurlProvider)
            Case Else
                Me.MLVtypes.SetActiveView(VIWbase)
        End Select
        With provider
            Me.LBproviderName.Text = .Name
            Me.LBuniqueCode.Text = .UniqueCode
            Me.LBdisplayToUser.Text = Me.Resource.getValue("Option." & .DisplayToUser.ToString)
            Me.LBallowAdminProfileInsert.Text = Me.Resource.getValue("Option." & .AllowAdminProfileInsert.ToString)
            Me.LBallowMultipleInsert.Text = Me.Resource.getValue("Option." & .AllowMultipleInsert.ToString)
            LBidentifierFields.Text = ""

            If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.longField, .IdentifierFields) Then
                LBidentifierFields.Text = Resource.getValue("IdentifierField." & IdentifierField.longField.ToString())
            End If
            If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.stringField, .IdentifierFields) Then
                LBidentifierFields.Text = IIf(String.IsNullOrEmpty(LBidentifierFields.Text), "", ", ") & Resource.getValue("IdentifierField." & IdentifierField.stringField.ToString())
            End If
        End With
    End Sub

    Public Sub DisplayProviderUnknown() Implements IViewProviderInfo.DisplayProviderUnknown
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub
End Class