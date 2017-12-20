Public Class AuthenticationError
    Inherits ERpageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub SetInternazionalizzazione()
 
    End Sub
    Protected Friend Overrides Function GetMessageContainer() As UC_ActionMessages
        Return CTRLmessage
    End Function
#End Region
#Region "Internal"

#End Region
   
End Class