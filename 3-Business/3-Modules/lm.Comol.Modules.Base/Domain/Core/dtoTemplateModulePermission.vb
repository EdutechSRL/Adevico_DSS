Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoTemplateModulePermission

#Region "Private Property"
        Private _RoleId As Integer
        Private _ModuleID As Integer
        Private _Permission As Long
        Private _RoleName As String
        Private _Dto As List(Of dtoPermission)
#End Region

#Region "Public Property"
        Public Property RoleId() As Integer
            Get
                Return _RoleId
            End Get
            Set(ByVal value As Integer)
                _RoleId = value
            End Set
        End Property
        'Public Property ModuleID() As Integer
        '    Get
        '        Return _ModuleID
        '    End Get
        '    Set(ByVal value As Integer)
        '        _ModuleID = value
        '    End Set
        'End Property
        Public Property Permission() As Long
            Get
                Return _Permission
            End Get
            Set(ByVal value As Long)
                _Permission = value
            End Set
        End Property
        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal value As String)
                _RoleName = value
            End Set
        End Property
        Public Property Positions() As List(Of dtoPermission)
            Get
                Return _Dto
            End Get
            Set(ByVal value As List(Of dtoPermission))
                _Dto = value
            End Set
        End Property
       
#End Region

        Sub New()
            _Dto = New List(Of dtoPermission)
        End Sub

        Public Overridable ReadOnly Property PermissionToString() As String
            Get
                If _Permission = 0 Then
                    Return "00000000000000000000000000000000"
                Else
                    Dim ToConvert As String = Convert.ToString(_Permission, 2)

                    ToConvert = New String(ToConvert.Reverse.ToArray)
                    While ToConvert.Length < 32
                        ToConvert &= "0"
                    End While
                    Return ToConvert
                End If
            End Get
        End Property
        Public Sub Update(ByVal name As String, permissions As List(Of Integer))
            Me.RoleName = name
            For Each position In permissions
                Dim dto As New dtoPermission() With {.IdPosition = position, .IdOwner = RoleId}
                If (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(CLng(2 ^ position), Permission)) Then
                    dto.Selected = True
                Else
                    dto.Selected = False
                End If
                Me.Positions.Add(dto)
            Next
        End Sub
    End Class
End Namespace