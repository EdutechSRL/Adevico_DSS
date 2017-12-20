Imports lm.Comol.Core.DomainModel.Helpers

Public Class UC_AutoCompleteField
    Inherits BaseControl

    Public Property SelectedLongItem As KeyValuePair(Of Long, String)
        Get
            Dim idItem As Long = 0
            If Not String.IsNullOrEmpty(HDNtxtID.Value) AndAlso IsNumeric(HDNtxtID.Value) Then
                idItem = CLng(HDNtxtID.Value)
            End If
            Return New KeyValuePair(Of Long, String)(idItem, TXTname.Text)
        End Get
        Set(value As KeyValuePair(Of Long, String))
            If Not IsNothing(value) Then
                HDNtxtID.Value = value.Key.ToString()
                TXTname.Text = value.Value
            End If
        End Set
    End Property
    Public Property SelectedItem As KeyValuePair(Of String, String)
        Get
            Return New KeyValuePair(Of String, String)(HDNtxtID.Value, TXTname.Text)
        End Get
        Set(value As KeyValuePair(Of String, String))
            If Not IsNothing(value) Then
                HDNtxtID.Value = value.Key
                TXTname.Text = value.Value
            End If
        End Set
    End Property
    Public Property TextClass As String
        Get
            Return ViewStateOrDefault("TextClass", "CTRL_AutoName" & Me.ClientID)
        End Get
        Set(value As String)
            ViewState("TextClass") = value
        End Set
    End Property
    Public Property ValueClass As String
        Get
            Return ViewStateOrDefault("ValueClass", "CTRL_AutoValue" & Me.ClientID)
        End Get
        Set(value As String)
            ViewState("ValueClass") = value
        End Set
    End Property
    Public Property CssClass As String
        Get
            Return ViewStateOrDefault("CssClass", "Testo_Campo")
        End Get
        Set(value As String)
            ViewState("CssClass") = value
        End Set
    End Property
    Public Property PermitClass As String
        Get
            Return ViewStateOrDefault("PermitClass", "")
        End Get
        Set(value As String)
            ViewState("PermitClass") = value
        End Set
    End Property
    Public Property [ReadOnly] As Boolean
        Get
            Return Me.TXTname.ReadOnly
        End Get
        Set(value As Boolean)
            ViewState("[ReadOnly]") = value
            Me.TXTname.ReadOnly = value
        End Set
    End Property

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitalizeControl(parameters As List(Of StringItem(Of String)))
        ControlRender(TextClass, ValueClass, PermitClass, parameters)
    End Sub
    Public Sub InitalizeControl(parameter As StringItem(Of String))
        Dim parameters As New List(Of StringItem(Of String))
        parameters.Add(parameter)
        ControlRender(TextClass, ValueClass, PermitClass, parameters)
    End Sub
    Public Sub InitalizeControl()
        ControlRender(TextClass, ValueClass, PermitClass, New List(Of StringItem(Of String)))
    End Sub

    Private Sub ControlRender(ByVal textClass As String, valueClass As String, permitClass As String, parameters As List(Of StringItem(Of String)))
        Me.HDNtxtID.Attributes.Add("class", valueClass)
        Me.TXTname.CssClass = CssClass & " " & textClass & IIf(String.IsNullOrEmpty(permitClass), "", " " & permitClass)
        Dim dataParameters As String = DataParameterstRender(parameters)
        '   If Not String.IsNullOrEmpty(dataParameters) Then
        Me.HDNtxtID.Attributes.Add("data-parameters", dataParameters)
    End Sub

    Private Function DataParameterstRender(parameters As List(Of StringItem(Of String))) As String
        Dim dataParameters As String = ""
        Dim strParameter As String = "'{0}':'{1}'"
        For Each parameter As StringItem(Of String) In parameters
            dataParameters &= "," & String.Format(strParameter, parameter.Name, parameter.Id)
        Next
        If Not String.IsNullOrEmpty(dataParameters) Then
            dataParameters = dataParameters.Remove(0, 1)
        End If
        Return dataParameters
    End Function
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Sub HideContent()
        Me.TXTname.Text = ""
    End Sub

End Class