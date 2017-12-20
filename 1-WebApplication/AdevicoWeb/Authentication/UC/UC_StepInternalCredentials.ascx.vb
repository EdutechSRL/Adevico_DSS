Public Class UC_AuthenticationStepInternalCredentials
    Inherits BaseControl

#Region "Property"
    Public ReadOnly Property Login As String
        Get
            Return Me.TXBlogin.Text
        End Get
    End Property
    Public ReadOnly Property Password As String
        Get
            Return Me.TXBpassword.Text
        End Get
    End Property
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
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
        'If Page.IsPostBack = False Then
        '    Me.SetInternazionalizzazione()
        'End If
    End Sub

    Public Sub InitializeControl()
        Dim LanguageID As Integer = Session("LinguaID")
        Try
            Me.TXBlogin.Text = ""
            Me.TXBpassword.Text = ""
            Me.SPNmessages.Attributes.Add("class", "invisible")
            isInitialized = True
        Catch ex As Exception

        End Try
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBlogin_t)
            .setLabel(Me.LBpwd)
            .setLiteral(Me.LTloginError)
        End With
    End Sub
#End Region

    Public Sub DisplayInvalidCredentials()
        Me.SPNmessages.Attributes.Add("class", "")
        Resource.setLiteral(Me.LTloginError)
    End Sub
    Public Sub DisplayInternalCredentialsMessage(message As lm.Comol.Core.BaseModules.ProfileManagement.ProfileSubscriptionMessage)
        Me.SPNmessages.Attributes.Add("class", "")
        Me.LTloginError.Text = Resource.getValue("ProfileSubscriptionMessage." & message.tostring)
    End Sub
    Public Sub ReloadLanguageSettings(language As Lingua)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub


End Class