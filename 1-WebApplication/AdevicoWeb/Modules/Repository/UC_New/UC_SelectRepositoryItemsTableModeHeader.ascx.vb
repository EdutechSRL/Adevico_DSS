Public Class UC_SelectRepositoryItemsTableModeHeader
    Inherits FRbaseControl
    Public Property SelectionMode As ListSelectionMode
        Get
            Return ViewStateOrDefault("SelectionMode", ListSelectionMode.Multiple)
        End Get
        Set(value As ListSelectionMode)
            ViewState("SelectionMode") = value
            Select Case value
                Case ListSelectionMode.Multiple
                    LTscriptSingle.Visible = False
                Case ListSelectionMode.Single
                    LTscriptSingle.Visible = True
            End Select
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
End Class