Public Class UC_UnsubscriptionDialogHeader
    Inherits DBbaseControl

#Region "Internal"
    Protected ReadOnly Property UnsubscriptionDialogTitleTranslation() As String
        Get
            Return Resource.getValue("UnsubscriptionDialogTitleTranslation")
        End Get
    End Property
#End Region
  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Internal"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
    Protected Friend Function GetOnUpdatingUnsubscriptionAction() As String
        Return Resource.getValue("GetOnUpdatingUnsubscriptionAction")
    End Function
#End Region
   
End Class