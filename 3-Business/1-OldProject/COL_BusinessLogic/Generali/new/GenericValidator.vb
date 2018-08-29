Public Class GenericValidator

	Public Shared Function ValString(ByVal oBj As Object, ByVal Defaultvalue As String) As String
        Try
            If IsDBNull(oBj) Then
                ValString = Defaultvalue
            ElseIf oBj = "" Then
                ValString = Defaultvalue
            Else
                ValString = oBj.ToString
            End If
        Catch ex As Exception
            ValString = Defaultvalue
        End Try
	End Function

	Public Shared Function ValInteger(ByVal oBj As Object, ByVal Defaultvalue As Integer) As Integer
        Try
            If IsDBNull(oBj) Then
                ValInteger = Defaultvalue
            Else
                ValInteger = oBj
            End If
        Catch ex As Exception
            ValInteger = Defaultvalue
        End Try
	End Function
	Public Shared Function ValData(ByVal oBj As Object, ByVal Defaultvalue As DateTime) As DateTime
        Try
            If IsDBNull(oBj) Then
                ValData = Defaultvalue
            Else
                ValData = oBj
            End If
        Catch ex As Exception
            ValData = Defaultvalue
        End Try
	End Function
	Public Shared Function ValBool(ByVal oBj As Object, ByVal Defaultvalue As Boolean) As Boolean
        Try
            If IsDBNull(oBj) Then
                ValBool = Defaultvalue
            Else
                ValBool = oBj
            End If
        Catch ex As Exception
            ValBool = Defaultvalue
        End Try
	End Function
End Class
