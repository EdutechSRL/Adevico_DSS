Public Class PopupError
    Inherits ERpageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("StandardError.Title")
        End With
    End Sub
    Protected Friend Overrides Function GetMessageContainer() As UC_ActionMessages
        Return CTRLmessage
    End Function
#End Region
#Region "Internal"
    Private Sub InternalError_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
#End Region
End Class