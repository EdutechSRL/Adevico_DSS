Public Class UC_ClientSideSwitch
    Inherits CMbaseControl


#Region "Internal"
    Public Property IsOn As Boolean
        Get
            If RBLswitch.SelectedIndex = -1 Then
                RBLswitch.SelectedIndex = 0
            End If
            Return (RBLswitch.SelectedIndex = 0)
        End Get
        Set(value As Boolean)
            If value Then
                RBLswitch.SelectedIndex = 0
            Else
                RBLswitch.SelectedIndex = 1
            End If
        End Set
    End Property
    Public Property DataDisable As String
        Get
            Return ViewStateOrDefault("DataDisable", "")
        End Get
        Set(value As String)
            ViewState("DataDisable") = value
        End Set
    End Property
    Public WriteOnly Property TextOn As String
        Set(value As String)
            HYPswitchOn.Text = value
            RBLswitch.Items(0).Text = value
        End Set
    End Property
    Public WriteOnly Property TextOff As String
        Set(value As String)
            HYPswitchOff.Text = value
            RBLswitch.Items(1).Text = value
        End Set
    End Property
    Public WriteOnly Property ToolTipOn As String
        Set(value As String)
            HYPswitchOn.ToolTip = value
        End Set
    End Property
    Public WriteOnly Property ToolTipOff As String
        Set(value As String)
            HYPswitchOff.ToolTip = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            If String.IsNullOrEmpty(HYPswitchOn.Text) Then
                .setHyperLink(HYPswitchOn, False, True)
            End If
            If String.IsNullOrEmpty(HYPswitchOff.Text) Then
                .setHyperLink(HYPswitchOff, False, True)
            End If
        End With
        RBLswitch.Items(0).Attributes("class") = "checkwrapper"
        RBLswitch.Items(1).Attributes("class") = "checkwrapper"
    End Sub
#End Region
#Region "Internal"

#End Region
End Class