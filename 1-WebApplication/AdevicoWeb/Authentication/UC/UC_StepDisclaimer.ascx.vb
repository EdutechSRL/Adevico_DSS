Partial Public Class UC_AuthenticationStepDisclaimer
    Inherits BaseControl
    Public Event UpdateUserLanguage(ByVal oLingua As Lingua)

#Region "Property"
    Public ReadOnly Property SelectedLanguage() As Comol.Entity.Lingua
        Get
            Dim oLingua As New Lingua

            oLingua = ManagerLingua.GetByID(Me.RBLlanguages.SelectedValue)
            If Not IsNothing(oLingua) Then
                Return oLingua
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Property SelectedLanguageId() As Integer
        Get
            If Me.RBLlanguages.SelectedIndex > -1 Then
                Return CInt(Me.RBLlanguages.SelectedValue)
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            Try
                Me.RBLlanguages.SelectedValue = value
            Catch ex As Exception

            End Try
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

    Public Sub InitializeControl()
        Dim LanguageID As Integer = Session("LinguaID")
        Try
            If Me.RBLlanguages.SelectedIndex > -1 Then
                LanguageID = Me.RBLlanguages.SelectedValue
            End If
            Me.RBLlanguages.DataSource = ManagerLingua.List
            Me.RBLlanguages.DataTextField = "Nome"
            Me.RBLlanguages.DataValueField = "ID"
            Me.RBLlanguages.DataBind()
            Me.RBLlanguages.SelectedValue = LanguageID

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
        End With
    End Sub
#End Region

    Private Sub RBLlanguages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLlanguages.SelectedIndexChanged
        Dim oLingua As New Lingua

        oLingua = ManagerLingua.GetByID(Me.RBLlanguages.SelectedValue)
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