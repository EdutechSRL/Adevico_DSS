Imports COL_BusinessLogic_v2.CL_permessi

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoTemplateRolePermission

#Region "Private Property"
        Private _Permission As Long
        Private _IdCommunityType As Integer
        Private _CommunityTypeName As String
        Private _Dto As List(Of dtoPermission)
#End Region

#Region "Public Property"
        Public Property Permission() As Long
            Get
                Return _Permission
            End Get
            Set(ByVal value As Long)
                _Permission = value
            End Set
        End Property
        Public Property IdCommunityType() As Integer
            Get
                Return _IdCommunityType
            End Get
            Set(ByVal value As Integer)
                _IdCommunityType = value
            End Set
        End Property
        Public Property CommunityTypeName() As String
            Get
                Return _CommunityTypeName
            End Get
            Set(ByVal value As String)
                _CommunityTypeName = value
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
            Me.CommunityTypeName = name
            For Each position In permissions
                Dim dto As New dtoPermission() With {.IdPosition = position, .IdOwner = IdCommunityType}
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