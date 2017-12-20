Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation

Public Class UC_ItemModuleAssignment
    Inherits BaseControl
    Implements IViewItemModuleAssignments

#Region "Implements"
    Public ReadOnly Property GetModuleId As Integer Implements IViewItemModuleAssignments.GetModuleId
        Get
            If Me.DDLmodules.Enabled Then
                Return Me.DDLmodules.SelectedValue
            Else : Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property GetPermission As Long Implements IViewItemModuleAssignments.GetPermission
        Get
            Dim values As List(Of Long) = (From i As ListItem In CBLpermissions.Items Where i.Selected = True Select CLng(i.Value)).ToList
            Dim Permissions As Long = 0

            If values.Count > 0 Then
                For Each value As Long In values
                    Permissions = Permissions Or (2 ^ value)
                Next
            End If
            Return Permissions
        End Get
    End Property

    Private Property StartIdModule As Long
        Get
            Return ViewStateOrDefault("StartIdModule", 0)
        End Get
        Set(value As Long)
            ViewState("StartIdModule") = value
        End Set
    End Property
    Private Property StartPermission As Long
        Get
            Return ViewStateOrDefault("StartPermission", 0)
        End Get
        Set(value As Long)
            ViewState("StartPermission") = value
        End Set
    End Property


    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region
  


#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MenubarEdit", "Modules", "Menu")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBmodule_t)
        End With
    End Sub
#End Region

    Public Sub InitalizeControl(idModule As Integer, permissions As Long, allowEdit As Boolean) Implements IViewItemModuleAssignments.InitalizeControl
        Me.StartIdModule = idModule
        Me.StartPermission = permissions
        LoadModules(idModule)

        Dim isEnabled As Boolean = (Me.DDLmodules.Enabled AndAlso idModule = Me.DDLmodules.SelectedValue AndAlso idModule > 0)
        Me.CBLpermissions.Visible = Me.DDLmodules.Enabled
        Me.CBLpermissions.Enabled = allowEdit
        If Me.DDLmodules.Enabled AndAlso idModule = Me.DDLmodules.SelectedValue AndAlso idModule > 0 Then
            LoadPermission(idModule, permissions)
        Else
            Me.CBLpermissions.Items.Clear()
        End If

    End Sub

    Private Sub LoadModules(idModule As Integer)
        Try
            Me.DDLmodules.DataSource = ManagerService.ListSystemTranslated(PageUtility.LinguaID, True)
            Me.DDLmodules.DataTextField = "Name"
            Me.DDLmodules.DataValueField = "Id"
            Me.DDLmodules.DataBind()
            Me.DDLmodules.Items.Insert(0, New ListItem("--", 0))
            Me.DDLmodules.SelectedValue = idModule
        Catch ex As Exception

        End Try
        Me.DDLmodules.Enabled = (Me.DDLmodules.Items.Count > 1)
    End Sub
    Private Sub LoadPermission(ByVal idModule As Integer, permissions As Long)
        Me.CBLpermissions.Items.Clear()

        Dim list As List(Of PlainPermission) = ManagerService.LoadModulePermissions(PageUtility.LinguaID, idModule)
        Me.CBLpermissions.Items.AddRange((From p As PlainPermission In list Select New ListItem(p.Name, p.Value) With {.Selected = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(permissions, 2 ^ p.Value)}).ToArray)

        'Me.CBLpermissions.DataSource = list
        'Me.CBLpermissions.DataTextField = "Name"
        'Me.CBLpermissions.DataValueField = "Value"
        'Me.CBLpermissions.DataBind()


    End Sub
    Private Sub DDLmodules_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLmodules.SelectedIndexChanged
        If Me.DDLmodules.SelectedValue = 0 Then
            Me.CBLpermissions.Items.Clear()
            Me.CBLpermissions.Visible = False
        Else
            If Me.DDLmodules.SelectedValue = StartIdModule Then
                LoadPermission(Me.DDLmodules.SelectedValue, StartPermission)
            Else
                LoadPermission(Me.DDLmodules.SelectedValue, 0)
            End If
        End If
    End Sub

End Class