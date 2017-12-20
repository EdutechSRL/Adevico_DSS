Public Class UC_InLineInput
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Property ReadOnlyInput As Boolean
        Get
            Return (INtextboxInput.Attributes("readonly") = "readonly")
        End Get
        Set(value As Boolean)
            If DataType <> ValidationDataType.Date Then
                If value Then
                    INtextboxInput.Attributes("readonly") = "readonly"
                Else
                    INtextboxInput.Attributes.Remove("readonly")
                End If
            End If
            If value Then
                LBinput.ToolTip = ""
            Else
                LBinput.ToolTip = Resource.getValue("LBinput.ToolTip")
            End If
        End Set
    End Property
    Public ReadOnly Property OldValue As String
        Get
            If LBinput.Text = "&nbsp;" Then
                Return ""
            Else
                Return LBinput.Text
            End If
        End Get
    End Property
    Public ReadOnly Property NewValue As String
        Get
            Return INtextboxInput.Value
        End Get
    End Property
    Public ReadOnly Property IsUpdated As Boolean
        Get
            Return OldValue <> NewValue
        End Get
    End Property
    Public ReadOnly Property IsInEditMode As Boolean
        Get
            Return HDNinputInEditMode.Value = "edit"
        End Get
    End Property


    Public Property InputMaxChar As Integer
        Get
            Return INtextboxInput.MaxLength
        End Get
        Set(value As Integer)
            INtextboxInput.MaxLength = value
        End Set
    End Property

    Public Property ContainerCssClass As String
        Get
            Return ViewStateOrDefault("ContainerCssClass", "")
        End Get
        Set(value As String)
            ViewState("ContainerCssClass") = value
        End Set
    End Property
    Public Property EditCssClass As String
        Get
            Return ViewStateOrDefault("EditCssClass", "")
        End Get
        Set(value As String)
            ViewState("EditCssClass") = value
        End Set
    End Property
    Public Property EditToolTip As String
        Get
            Return ViewStateOrDefault("EditToolTip", "")
        End Get
        Set(value As String)
            ViewState("EditToolTip") = value
        End Set
    End Property
    Public Property ValidationExpression As String
        Get
            Return RXVnput.ValidationExpression
        End Get
        Set(value As String)
            RXVnput.ValidationExpression = value
        End Set
    End Property
    Public Property DataType As ValidationDataType
        Get
            Return RNVinput.Type
        End Get
        Set(value As ValidationDataType)
            Select Case value
                Case ValidationDataType.Date
                    If String.IsNullOrEmpty(RNVinput.MinimumValue) Then
                        RNVinput.MinimumValue = New DateTime(1980, 1, 1).Date
                    End If
                    If String.IsNullOrEmpty(RNVinput.MaximumValue) Then
                        RNVinput.MaximumValue = DateTime.MaxValue.AddDays(-1).Date
                    End If
                    INtextboxInput.Attributes("readonly") = "readonly"
                Case ValidationDataType.Integer
                    If String.IsNullOrEmpty(RNVinput.MinimumValue) Then
                        RNVinput.MinimumValue = 0
                    End If
                    If String.IsNullOrEmpty(RNVinput.MaximumValue) Then
                        RNVinput.MaximumValue = Integer.MaxValue - 1
                    End If
                Case ValidationDataType.Double
                    If String.IsNullOrEmpty(RNVinput.MinimumValue) Then
                        RNVinput.MinimumValue = 0
                    End If
                    If String.IsNullOrEmpty(RNVinput.MaximumValue) Then
                        RNVinput.MaximumValue = Double.MaxValue - 1
                    End If
            End Select
            RNVinput.Type = value
        End Set
    End Property
    Public Property MaximumValue As String
        Get
            Return RNVinput.MaximumValue
        End Get
        Set(value As String)
            RNVinput.MaximumValue = value
        End Set
    End Property
    Public Property MinimumValue As String
        Get
            Return RNVinput.MinimumValue
        End Get
        Set(value As String)
            RNVinput.MinimumValue = value
        End Set
    End Property
    Public Property ValidationEnabled As Boolean
        Get
            Return ViewStateOrDefault("ValidationEnabled", True)
        End Get
        Set(value As Boolean)
            ViewState("ValidationEnabled") = value
        End Set
    End Property
    Public Property IsEnabled As Boolean
        Get
            Return ViewStateOrDefault("IsEnabled", True)
        End Get
        Set(value As Boolean)
            ViewState("IsEnabled") = value
            If value Then
                INtextboxInput.Attributes.Remove("readonly")
            Else
                INtextboxInput.Attributes("readonly") = "readonly"
            End If
        End Set
    End Property
    Public ReadOnly Property IsValid As Boolean
        Get
            Return Not ValidationEnabled OrElse (RNVinput.Enabled AndAlso RNVinput.IsValid) OrElse (RXVnput.Enabled AndAlso RXVnput.IsValid)
        End Get
    End Property

    'Public Property EditMode As Boolean
    '    Get
    '        Return ViewStateOrDefault("EditMode", False)
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("EditMode") = value
    '    End Set
    'End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UCInLineInput", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            LBactionSave.ToolTip = Resource.getValue("LBactionSave.ToolTip")
            LBactionCancel.ToolTip = Resource.getValue("LBactionCancel.ToolTip")

        End With
    End Sub
#End Region

    Public Sub AutoInitialize(value As String)
        InitializeInternalControl(value, ContainerCssClass, InputMaxChar, EditCssClass, EditToolTip)
        HDNinputInEditMode.Value = "init"
        RXVnput.Enabled = Not String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RXVnput.Visible = Not String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RNVinput.Enabled = String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RNVinput.Visible = String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
    End Sub
    Public Sub AutoInitialize(oldValue As String, newValue As String)
        InitializeInternalControl(oldValue, ContainerCssClass, InputMaxChar, EditCssClass, EditToolTip)
        HDNinputInEditMode.Value = "edit"
        INtextboxInput.Value = newValue

        RXVnput.Enabled = Not String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RXVnput.Visible = Not String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RNVinput.Enabled = String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RNVinput.Visible = String.IsNullOrEmpty(ValidationExpression) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
    End Sub
    Public Sub InitializeControl(value As String, containerCssClass As String, ByVal inputMaxChar As Integer, Optional editCssClass As String = "", Optional editToolTip As String = "", Optional ByVal regex As String = "")
        InitializeInternalControl(value, containerCssClass, inputMaxChar, editCssClass, editToolTip)
        HDNinputInEditMode.Value = "init"
        RXVnput.ValidationExpression = regex
        RXVnput.Enabled = Not String.IsNullOrEmpty(regex) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RXVnput.Visible = Not String.IsNullOrEmpty(regex) AndAlso ValidationEnabled AndAlso Not ReadOnlyInput
        RNVinput.Enabled = False
        RNVinput.Visible = False
    End Sub
    Public Sub InitializeControl(value As String, containerCssClass As String, ByVal inputMaxChar As Integer, ByVal validation As ValidationDataType, ByVal minimumvalue As String, ByVal maximumvalue As String, Optional editCssClass As String = "", Optional editToolTip As String = "")
        InitializeInternalControl(value, containerCssClass, inputMaxChar, editCssClass, editToolTip)
        HDNinputInEditMode.Value = "init"
        RNVinput.Type = validation
        RNVinput.Enabled = ValidationEnabled AndAlso Not ReadOnlyInput
        RNVinput.Visible = ValidationEnabled AndAlso Not ReadOnlyInput
        RNVinput.MaximumValue = maximumvalue
        RNVinput.MinimumValue = minimumvalue
        RXVnput.Enabled = False
        RXVnput.Visible = False
    End Sub
    Private Sub InitializeInternalControl(value As String, containerCssClass As String, ByVal inputMaxChar As Integer, editCssClass As String, editToolTip As String)
        HDNinputInEditMode.Value = ""
        If String.IsNullOrEmpty(value) AndAlso Not containerCssClass.Contains(" empty") Then
            containerCssClass &= " empty"
        ElseIf Not String.IsNullOrEmpty(value) AndAlso containerCssClass.Contains("empty") Then
            containerCssClass = Replace(containerCssClass, " empty", "")
        End If

        '   SPNcontainer.Attributes("class") = IIf(IsInEditMode, LTeditModeCssClass.Text, LTviewModeCssClass.Text & " " & LTdisabledModeCssClass.Text) & " " & containerCssClass


        SPNcontainer.Attributes("class") = IIf(IsInEditMode, LTeditModeCssClass.Text, LTviewModeCssClass.Text) & " " & containerCssClass


        INtextboxInput.MaxLength = inputMaxChar
        If ReadOnlyInput Then
            editToolTip = ""
            LBinput.ToolTip = ""
        ElseIf String.IsNullOrEmpty(editToolTip) Then
            editToolTip = Resource.getValue("DefaultAction")
            LBinput.ToolTip = Resource.getValue("LBinput.ToolTip")
        End If

        LTspanEditOpen.Text = String.Format(LTspanEdit.Text, editCssClass, editToolTip)

        If String.IsNullOrEmpty(value) Then
            LBinput.Text = "&nbsp;"
        Else
            LBinput.Text = value
        End If
 
        INtextboxInput.Value = value
    End Sub

    'Public Sub UpdateEditMode()
    '    UpdateToEditMode(EditMode)
    'End Sub
    'Public Sub UpdateToEditMode(inEditMode As Boolean)
    '    SPNcontainer.Attributes("class") = IIf(inEditMode, LTeditModeCssClass.Text, LTviewModeCssClass.Text) & " " & ContainerCssClass
    'End Sub
End Class