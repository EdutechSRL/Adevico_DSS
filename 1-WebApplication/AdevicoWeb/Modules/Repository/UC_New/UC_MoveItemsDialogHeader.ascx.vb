Public Class UC_MoveItemsDialogHeader
    Inherits FRbaseControl

    'Protected ReadOnly Property UnsubscriptionDialogTitleTranslation() As String
    '    Get
    '        Return Resource.getValue("UnsubscriptionDialogTitleTranslation")
    '    End Get
    'End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
    Protected Function GetOnUpdatingMoveAction() As String
        Return Resource.getValue("GetOnUpdatingMoveAction")
    End Function
End Class