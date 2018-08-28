<Serializable(), CLSCompliant(True)> Public Class dtoAuthenticationType
    Public Id As Integer
    Public Name As String
End Class

<Serializable(), CLSCompliant(True)> Public Enum EditingUI
    ByUser = 1
    ByAdministrator = 2
End Enum

<Serializable(), CLSCompliant(True)> Public Class dtoBaseItem
    Public Id As Integer
    Public Name As String
End Class

<Serializable(), CLSCompliant(True)> Public Class dtoBaseSpecializedDetails

End Class

<Serializable(), CLSCompliant(True)> Public Class dtoSpecializedExternal
    Inherits dtoBaseSpecializedDetails
    Public ExternalUserInfo As String
End Class