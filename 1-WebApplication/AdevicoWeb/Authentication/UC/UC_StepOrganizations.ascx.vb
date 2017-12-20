Public Class UC_AuthenticationStepOrganizations
    Inherits BaseControl


#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property AvailableOrganizationsId As List(Of Integer)
        Get
            If Me.MLVorganizations.GetActiveView Is VIWuser Then
                Return (From l As ListItem In Me.RBLorganizations.Items Select CInt(l.Value)).ToList
            Else
                Return (From l As ListItem In Me.DDLorganizations.Items Select CInt(l.Value)).ToList
            End If
        End Get
    End Property

    Public Property SelectedOrganizationId() As Integer
        Get
            If Me.MLVorganizations.ActiveViewIndex = 0 Then
                If Me.RBLorganizations.SelectedIndex = -1 Then : Return 0
                Else : Return Me.RBLorganizations.SelectedValue
                End If
            Else
                Return Me.DDLorganizations.SelectedValue
            End If
        End Get
        Set(ByVal value As Integer)
            If Me.MLVorganizations.ActiveViewIndex = 0 Then
                Me.RBLorganizations.SelectedValue = value
            Else
                Me.DDLorganizations.SelectedValue = value
            End If
        End Set
    End Property
    Public ReadOnly Property AllSelectedOrganizationId() As List(Of Integer)
        Get
            Dim items As New List(Of Integer)
            If Me.MLVorganizations.ActiveViewIndex = 0 Then
                If Me.RBLorganizations.SelectedIndex > 0 Then
                    items.Add(Me.RBLorganizations.SelectedValue)
                End If
            Else
                items.Add(Me.DDLorganizations.SelectedValue)
                items.AddRange((From l As ListItem In Me.CBLorganzations.Items Where l.Selected Select CInt(l.Value)).ToList)
            End If
            Return items
        End Get
    End Property
    Public ReadOnly Property SelectedOrganizationName() As String
        Get
            If Me.MLVorganizations.ActiveViewIndex = 0 Then
                If Me.RBLorganizations.SelectedIndex = -1 Then : Return ""
                Else : Return Me.RBLorganizations.SelectedItem.Text
                End If
            ElseIf Me.DDLorganizations.SelectedIndex >= 0 Then
                Return Me.DDLorganizations.SelectedItem.Text
            Else : Return ""

            End If
        End Get
    End Property

    Public ReadOnly Property OtherOrganizationsToSubscribe() As List(Of String)
        Get
            Return (From l As ListItem In Me.CBLorganzations.Items Where l.Selected Select l.Text).ToList
        End Get
    End Property
    Public Property OtherOrganizationsToSubscribeID() As List(Of Integer)
        Get
            If Me.CBLorganzations.Items.Count > 0 Then
                Return (From l As ListItem In Me.CBLorganzations.Items Where l.Selected Select CInt(l.Value)).ToList
            Else
                Return New List(Of Integer)
            End If
        End Get
        Set(ByVal value As List(Of Integer))
            Me.CBLorganzations.SelectedIndex = -1
            For Each oItemID As Integer In value
                Dim oListItem As ListItem = Me.CBLorganzations.Items.FindByValue(oItemID)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
        End Set
    End Property
    Public ReadOnly Property OtherOrganizations() As Dictionary(Of Integer, String)
        Get
            Return (From l As ListItem In Me.CBLorganzations.Items Where l.Selected Select New With {.Key = CInt(l.Value), .Value = l.Text}).ToDictionary(Function(c) c.Key, Function(c) c.Value)
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

    'Public ReadOnly Property ItemsCount As Integer
    '    Get
    '        If Me.MLVorganizations.ActiveViewIndex = 0 Then
    '            If Me.RBLorganizations.SelectedIndex = -1 Then : Return 0
    '            Else : Return Me.RBLorganizations.Items.Count
    '            End If
    '        Else
    '            Return Me.DDLorganizations.Items.Count
    '        End If
    '    End Get
    'End Property


    'Public ReadOnly Property DefaultOrganization() As dtoBaseItem
    '    Get
    '        Dim oDto As New dtoBaseItem

    '        If Me.MLVorganizations.ActiveViewIndex = 0 Then
    '            oDto.Id = Me.RBLorganizations.SelectedValue
    '            oDto.Name = Me.RBLorganizations.SelectedItem.Text
    '        Else
    '            oDto.Id = Me.DDLorganizations.SelectedValue
    '            oDto.Name = Me.DDLorganizations.SelectedItem.Text
    '        End If

    '        Return oDto
    '    End Get
    'End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Page.IsPostBack = False Then
        '    Me.SetInternazionalizzazione()
        'End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBsubscribeToAlso)
            .setLabel(Me.LBsubscribeToDefault)
            .setLabel(Me.LBsubscribeTo)
        End With
    End Sub
#End Region

#Region "Initialize Control"
    Public Sub InitializeControl(forManagement As Boolean)
        If forManagement Then
            InitializeForManagement()
        Else
            Me.InitializeControlForSubscription()
        End If
        Me.isInitialized = True
    End Sub
    Public Sub InitializeForManagement()
        Dim Organizations As List(Of COL_BusinessLogic_v2.Comunita.COL_Organizzazione) = COL_BusinessLogic_v2.Comunita.COL_Organizzazione.List()
        Me.DDLorganizations.DataSource = Organizations
        Me.DDLorganizations.DataValueField = "Id"
        Me.DDLorganizations.DataTextField = "RagioneSociale"
        Me.DDLorganizations.DataBind()

        Dim others As List(Of COL_BusinessLogic_v2.Comunita.COL_Organizzazione) = (From o In Organizations Where o.Id <> Me.DDLorganizations.SelectedValue Select o).ToList
        Me.CBLorganzations.Visible = (others.Count > 0)
        Me.LBsubscribeToAlso.Visible = (others.Count > 0)
        If others.Count = 0 Then
            Me.CBLorganzations.Items.Clear()
        Else
            Me.CBLorganzations.DataSource = others
            Me.CBLorganzations.DataValueField = "Id"
            Me.CBLorganzations.DataTextField = "RagioneSociale"
            Me.CBLorganzations.DataBind()
        End If
     

        Me.MLVorganizations.SetActiveView(VIWadministrator)
    End Sub
    Public Sub InitializeControlForSubscription()
        Dim Organizations As List(Of COL_BusinessLogic_v2.Comunita.COL_Organizzazione) = COL_BusinessLogic_v2.Comunita.COL_Organizzazione.ListForSubscription()

        Me.RBLorganizations.DataSource = Organizations
        Me.RBLorganizations.DataValueField = "Id"
        Me.RBLorganizations.DataTextField = "RagioneSociale"
        Me.RBLorganizations.DataBind()
        Me.MLVorganizations.SetActiveView(VIWuser)

        'If Organizations.Count = 1 Then
        Me.RBLorganizations.SelectedIndex = 0
        ' End If
    End Sub
    Public Sub ReloadLanguageSettings(language As Lingua)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
#End Region

    Private Sub DDLorganizations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizations.SelectedIndexChanged
        Dim Organizations As List(Of COL_BusinessLogic_v2.Comunita.COL_Organizzazione) = COL_BusinessLogic_v2.Comunita.COL_Organizzazione.List()

        Dim oList As List(Of ListItem) = (From l As ListItem In Me.CBLorganzations.Items Where l.Selected).ToList

        Me.CBLorganzations.DataSource = (From o In Organizations Where o.Id <> Me.DDLorganizations.SelectedValue Select o).ToList
        Me.CBLorganzations.DataValueField = "Id"
        Me.CBLorganzations.DataTextField = "RagioneSociale"
        Me.CBLorganzations.DataBind()

        For Each oItem As ListItem In oList
            Dim oListItem As ListItem = Me.CBLorganzations.Items.FindByValue(oItem.Value)
            If Not IsNothing(oListItem) Then
                oListItem.Selected = True
            End If
        Next
    End Sub

End Class