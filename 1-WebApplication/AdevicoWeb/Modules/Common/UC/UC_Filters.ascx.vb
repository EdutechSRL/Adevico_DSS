Public Class UC_Filters
    Inherits BaseControl

#Region "inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "internal"
    Public Function GetBaseUrl() As String
        Return PageUtility.ApplicationUrlBase
    End Function

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region


#Region "internal"

#End Region

End Class