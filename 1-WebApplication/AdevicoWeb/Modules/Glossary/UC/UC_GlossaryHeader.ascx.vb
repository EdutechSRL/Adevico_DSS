Public Class UC_GlossaryHeader
    Inherits GLbaseControl

#Region "Inherits Property"

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Inherits Methods"

    Protected Overrides Sub SetInternazionalizzazione()
    End Sub

#End Region
End Class