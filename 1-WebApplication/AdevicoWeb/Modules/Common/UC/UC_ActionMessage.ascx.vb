Imports lm.Comol.Core.DomainModel.Helpers

Public Class UC_ActionMessage
    Inherits BaseControl

    Public Property CssClass As String
        Get
            Return ViewStateOrDefault("CssClass", "")
        End Get
        Set(value As String)
            ViewState("CssClass") = value
        End Set
    End Property
    Public ReadOnly Property CssClassMessageType As String
        Get
            Return MessageType.GetStringValue()
        End Get
    End Property
    Public Property RaiseEvents As Boolean
        Get
            Return ViewStateOrDefault("RaiseEvents", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseEvents") = value
        End Set
    End Property
    Private Property MessageType As MessageType
        Get
            Return ViewStateOrDefault("MessageType", MessageType.none)
        End Get
        Set(value As MessageType)
            ViewState("MessageType") = value
        End Set
    End Property

    Public Event MessageItemCommand(ByVal cancel As Boolean, ByVal value As String)

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeControl(message As dtoMessage)
        Me.LTmessage.Text = message.Text
        Me.MessageType = message.Type

        If Not String.IsNullOrEmpty(message.ConfirmButton) Then
            LNBconfirm.Visible = True
            LNBconfirm.Text = message.ConfirmButton
            LNBconfirm.CommandArgument = message.ConfirmButtonValue
        End If
        If Not String.IsNullOrEmpty(message.CancelButton) Then
            LNBcancel.Visible = True
            LNBcancel.Text = message.CancelButton
            LNBcancel.CommandArgument = message.CancelButtonValue
        End If
    End Sub
    Public Sub InitializeControl(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
        Me.LTmessage.Text = message
        Me.MessageType = type
    End Sub
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Private Sub LNBconfirm_Click(sender As Object, e As System.EventArgs) Handles LNBconfirm.Click
        If RaiseEvents Then
            RaiseEvent MessageItemCommand(False, LNBconfirm.CommandArgument)
        End If
    End Sub
    Private Sub LNBcancel_Click(sender As Object, e As System.EventArgs) Handles LNBcancel.Click
        If RaiseEvents Then
            RaiseEvent MessageItemCommand(True, LNBcancel.CommandArgument)
        End If
    End Sub
End Class