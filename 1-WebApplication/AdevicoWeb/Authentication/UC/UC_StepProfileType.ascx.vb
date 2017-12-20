Public Class UC_AuthenticationStepProfileType
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property SelectedProfileName() As String
        Get
            If Me.RBLuserTypes.SelectedIndex < 0 Then
                Return "--"
            Else
                Return Me.RBLuserTypes.SelectedItem.Text
            End If
        End Get
    End Property
    Public ReadOnly Property AvailableProfileTypes As List(Of Integer)
        Get
            Return (From l As ListItem In Me.RBLuserTypes.Items Select CInt(l.Value)).ToList
        End Get
    End Property
    Public Property SelectedProfileTypeId() As Integer
        Get
            If Me.RBLuserTypes.SelectedIndex < 0 Then
                Return 0
            Else
                Return Me.RBLuserTypes.SelectedValue
            End If
        End Get
        Set(ByVal value As Integer)
            Try
                Me.RBLuserTypes.SelectedValue = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBprofileToSelect)
        End With
    End Sub
#End Region


#Region "Initialize Control"

    Public Sub InitializeControlForManagement(idProfile As Integer)
        Dim oUserTypes As List(Of COL_TipoPersona)  = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList

            Me.RBLuserTypes.DataSource = oUserTypes
            Me.RBLuserTypes.DataValueField = "ID"
            Me.RBLuserTypes.DataTextField = "Descrizione"
            Me.RBLuserTypes.DataBind()

            If idProfile = 0 Then
            Me.RBLuserTypes.SelectedValue = Main.TipoPersonaStandard.StudenteStandard
        ElseIf (From t As COL_TipoPersona In oUserTypes Where t.ID = idProfile Select t.ID).Any Then
            Me.RBLuserTypes.SelectedValue = idProfile
        ElseIf oUserTypes.Count > 0 Then
            Me.RBLuserTypes.SelectedIndex = 0
        End If

        SetInternazionalizzazione()
        Me.isInitialized = True
    End Sub
    Public Sub InitializeControl(forManagement As Boolean, idOrganization As Integer, idProfile As Integer)
        Dim oUserTypes As List(Of COL_TipoPersona) = Nothing

        If forManagement Then
            oUserTypes = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList
        Else
            oUserTypes = COL_TipoPersona.ListForSubscription(Me.PageUtility.LinguaID, idOrganization)
        End If
        Me.RBLuserTypes.DataSource = oUserTypes
        Me.RBLuserTypes.DataValueField = "ID"
        Me.RBLuserTypes.DataTextField = "Descrizione"
        Me.RBLuserTypes.DataBind()

        If idProfile = 0 Then
            If forManagement Then
                Me.RBLuserTypes.SelectedValue = Main.TipoPersonaStandard.StudenteStandard
            Else
                Me.RBLuserTypes.SelectedIndex = 0
            End If
        ElseIf (From t As COL_TipoPersona In oUserTypes Where t.ID = idProfile Select t.ID).Any Then
            Me.RBLuserTypes.SelectedValue = idProfile
        ElseIf oUserTypes.Count > 0 Then
            Me.RBLuserTypes.SelectedIndex = 0
        End If

        SetInternazionalizzazione()
        Me.isInitialized = True
    End Sub
    Public Sub InitializeControl(idOrganization As Integer, idProfile As Integer, OtherRolesId As List(Of Integer), IdDefaultRole As Integer)
        Dim profileTypes As List(Of COL_TipoPersona) = COL_TipoPersona.ListForSubscription(Me.PageUtility.LinguaID, idOrganization)
        Dim allProfiles As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList

        If OtherRolesId.Count > 0 Then
            Dim available As List(Of Integer) = profileTypes.Select(Function(r) r.ID).ToList
            profileTypes.AddRange((From a As COL_TipoPersona In allProfiles Where OtherRolesId.Contains(a.ID) AndAlso Not available.Contains(a.ID) Select a).ToList)
        End If


        Me.RBLuserTypes.DataSource = profileTypes.OrderBy(Function(r) r.Descrizione).ToList
        Me.RBLuserTypes.DataValueField = "ID"
        Me.RBLuserTypes.DataTextField = "Descrizione"
        Me.RBLuserTypes.DataBind()

        If IdDefaultRole > 0 AndAlso (From t As COL_TipoPersona In profileTypes Where t.ID = IdDefaultRole Select t.ID).Any Then
            Me.RBLuserTypes.SelectedValue = IdDefaultRole
        ElseIf idProfile = 0 Then
            Me.RBLuserTypes.SelectedIndex = 0
        ElseIf (From t As COL_TipoPersona In profileTypes Where t.ID = idProfile Select t.ID).Any Then
            Me.RBLuserTypes.SelectedValue = idProfile
        ElseIf profileTypes.Count > 0 Then
            Me.RBLuserTypes.SelectedIndex = 0
        End If

        SetInternazionalizzazione()
        Me.isInitialized = True
    End Sub
    Public Sub ReloadLanguageSettings(language As Lingua)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
#End Region

End Class