Imports COL_DataLayer

Namespace CL_permessi
    Public Class PermissionAssociation
        Public Property Id() As Integer
        Public Property Description() As String
        Public Property Name() As String
        Public Property Position() As Integer
        Public Property IsAssociated() As Boolean

        Public Function ToBinary() As Long
            Return 2 ^ Position
        End Function

    End Class

End Namespace