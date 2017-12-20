Public Class UC_AuthenticationStepUnknownProfile
    Inherits BaseControl
    Public Event UpdateUserLanguage(ByVal oLingua As Lingua)

#Region "Property"
    Public ReadOnly Property SelectedLanguage() As Comol.Entity.Lingua
        Get
            Dim oLingua As New Lingua

            oLingua = ManagerLingua.GetByID(Me.DDLlanguages.SelectedValue)
            If Not IsNothing(oLingua) Then
                Return oLingua
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Property SelectedLanguageId() As Integer
        Get
            If Me.DDLlanguages.SelectedIndex > -1 Then
                Return CInt(Me.DDLlanguages.SelectedValue)
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            Try
                Me.DDLlanguages.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property

    Public Property SelectedProvider As lm.Comol.Core.Authentication.AuthenticationProviderType
        Get
            Return IIf(RBinternalProfile.Checked, lm.Comol.Core.Authentication.AuthenticationProviderType.Internal, lm.Comol.Core.Authentication.AuthenticationProviderType.Shibboleth)
        End Get
        Set(value As lm.Comol.Core.Authentication.AuthenticationProviderType)
            If lm.Comol.Core.Authentication.AuthenticationProviderType.Internal Then
                RBnewProfile.Checked = False
                RBinternalProfile.Checked = True
            Else
                RBinternalProfile.Checked = False
                RBnewProfile.Checked = True
            End If
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

    Public Sub InitializeControl(ByVal dProvider as lm.Comol.Core.Authentication.AuthenticationProviderType,ByVal providers As List(Of lm.Comol.Core.Authentication.AuthenticationProviderType))
        Dim LanguageID As Integer = Session("LinguaID")
        Try
            If Me.DDLlanguages.SelectedIndex > -1 Then
                LanguageID = Me.DDLlanguages.SelectedValue
            End If
            Me.DDLlanguages.DataSource = ManagerLingua.List
            Me.DDLlanguages.DataTextField = "Nome"
            Me.DDLlanguages.DataValueField = "ID"
            Me.DDLlanguages.DataBind()
            Me.DDLlanguages.SelectedValue = LanguageID
            Me.RBnewProfile.Checked = True
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
            .setLabel(Me.LBlanguage_t)

            .setLabel(LBinternalProfileRadio)
            .setLabel(LBinternalProfileRadioDescription)
            .setLabel(LBnewProfileRadio)
            .setLabel(LBnewProfileRadioDescription)
        End With
    End Sub
#End Region

    Private Sub DDLlanguages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLlanguages.SelectedIndexChanged
        Dim oLingua As New Lingua

        oLingua = ManagerLingua.GetByID(Me.DDLlanguages.SelectedValue)
        If Not IsNothing(oLingua) Then
            RaiseEvent UpdateUserLanguage(oLingua)
        End If
    End Sub

    Public Sub ReloadLanguageSettings(language As Lingua)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub

End Class