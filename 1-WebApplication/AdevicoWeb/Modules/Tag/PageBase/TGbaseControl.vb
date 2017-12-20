Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Public MustInherit Class TGbaseControl
    Inherits BaseControl

#Region "Implements"

#End Region


#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
     Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
    Protected Friend Function GetDateTimeString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Return GetDateToString(datetime, defaultString) & " " & GetTimeToString(datetime, defaultString)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetDateToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Dim pattern As String = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
            If (pattern.Contains("yyyy")) Then
                pattern = pattern.Replace("yyyy", "yy")
            End If
            Return datetime.Value.ToString(pattern)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetTimeToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Return datetime.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
        Else
            Return defaultString
        End If
    End Function

#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Dashboard", "Modules", "Dashboard")
    End Sub
#End Region

End Class