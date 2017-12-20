Public Class UC_ActionMessages
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Public Event ItemCommand(ByVal cancel As Boolean, ByVal value As String)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeControl(items As List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage))
        Me.RPTmessages.DataSource = items
        Me.RPTmessages.DataBind()
    End Sub
    Public Sub InitializeControl(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType, Optional confirmButtonText As String = "", Optional confirmButtonValue As String = "", Optional cancelButtonText As String = "", Optional cancelButtonValue As String = "")
        Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = message, .Type = type, .ConfirmButton = confirmButtonText, .ConfirmButtonValue = confirmButtonValue, .CancelButton = cancelButtonText, .CancelButtonValue = cancelButtonValue})
        InitializeControl(items)
    End Sub

    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Private Sub RPTmessages_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTmessages.ItemCommand
        RaiseEvent ItemCommand(e.CommandName = "cancel", e.CommandArgument)
    End Sub
    Private Sub RPTmessages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmessages.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As lm.Comol.Core.DomainModel.Helpers.dtoMessage = DirectCast(e.Item.DataItem, lm.Comol.Core.DomainModel.Helpers.dtoMessage)
            Dim oControl As UC_ActionMessage = e.Item.FindControl("CTRLmessage")
            oControl.InitializeControl(item)
        End If
    End Sub

End Class