Public Class UC_EnrollDialogHeader
    Inherits DBbaseControl

    'Protected ReadOnly Property UnsubscriptionDialogTitleTranslation() As String
    '    Get
    '        Return Resource.getValue("UnsubscriptionDialogTitleTranslation")
    '    End Get
    'End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
    Protected Function GetOnUpdatingEnrollAction() As String
        Return Resource.getValue("GetOnUpdatingEnrollAction")
    End Function
End Class