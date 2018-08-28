Namespace Configuration.Components
    <Serializable(), CLSCompliant(True)> Public Class dtoErrorType
        Public Enabled As Boolean
        Public Type As ErrorsNotificationService.ErrorType
        Public SendTo As ErrorsNotificationService.PersistTo
        Public Sub New()

        End Sub
        Public Sub New(ByVal pEnabled As String, ByVal pType As String, ByVal pPersistTo As String)
            Try
                Me.Enabled = CBool(pEnabled)
            Catch ex As Exception
                Me.Enabled = False
            End Try
          
            Me.Type = EnumParser(Of ErrorsNotificationService.ErrorType).GetByString(pType, ErrorsNotificationService.ErrorType.GenericError)
            Me.SendTo = EnumParser(Of ErrorsNotificationService.PersistTo).GetByString(pPersistTo, ErrorsNotificationService.PersistTo.Mail)
        End Sub


        Private Class EnumParser(Of T)
            Public Shared Function GetByString(ByVal Expression As String, ByVal DefaultValue As T) As T
                Dim iResponse As T
                If String.IsNullOrEmpty(Expression) Then
                    iResponse = DefaultValue
                Else
                    If [Enum].IsDefined(GetType(T), Expression) Then
                        iResponse = [Enum].Parse(GetType(T), Expression)
                    Else
                        iResponse = DefaultValue
                    End If
                End If

                Return iResponse
            End Function

        End Class
    End Class
End Namespace