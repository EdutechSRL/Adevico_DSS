Public Class UC_AuthenticationStepPrivacy
    Inherits BaseControl


#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property AcceptedMandatoryPolicy As Boolean
        Get
            Return CTRLpolicy.AcceptedMandatoryPolicy
        End Get
    End Property

    Public Property GetPolicyInfo As List(Of lm.Comol.Core.BaseModules.PolicyManagement.dtoUserPolicyInfo)
        Get
            Return Me.CTRLpolicy.GetItemsValue
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.PolicyManagement.dtoUserPolicyInfo))
            '
        End Set
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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
          
        End With
    End Sub
#End Region


#Region "Initialize Control"
    Public Sub InitializeControl()
        Me.CTRLpolicy.InitializeControl()
        Me.isInitialized = True
    End Sub
    Public Sub ReloadLanguageSettings(language As Lingua)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
        Me.CTRLpolicy.ReloadLanguageSettings(language)
    End Sub
#End Region

End Class