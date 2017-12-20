Public Class UC_EditCriterion
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Public Event ChangeToIntegerType(idCriterion As Long, minValue As Integer, maxValue As Integer)
    Public Event ChangeToDecimalType(idCriterion As Long, minValue As Decimal, maxValue As Decimal)
    Public Event AddOption(idCriterion As Long, name As String, value As Double)
    Public Event RemoveOption(idOption As Long)

    Public ReadOnly Property GetCriterion As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion
        Get
            Dim dto As New lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion
            dto.Description = Me.TXBdescription.Text
            ' dto.ToolTip = Me.TXBhelp.Text
            dto.Type = CriterionType
            Select Case dto.Type
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Textual
                    If IsNumeric(Me.TXBmaxChar.Text) Then
                        dto.MaxLength = CInt(Me.TXBmaxChar.Text)
                    End If
                    Return dto
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange
                    Dim minV, maxV As Decimal
                    Decimal.TryParse(Me.TXBminValue.Text, minV)
                    Decimal.TryParse(Me.TXBmaxValue.Text, maxV)
                    dto.DecimalMinValue = minV
                    dto.DecimalMaxValue = maxV
                    If minV > maxV Then
                        dto.DecimalMinValue = IIf(Me.CBXintegerRange.Checked, Math.Round(maxV, 0), Math.Round(maxV, 2))
                        dto.DecimalMaxValue = IIf(Me.CBXintegerRange.Checked, Math.Round(minV, 0), Math.Round(minV, 2))
                    ElseIf maxV = 0 Then
                        dto.DecimalMaxValue = 1
                    End If

                    CriterionType = IIf(Me.CBXintegerRange.Checked, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange)
                    dto.Type = CriterionType
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.StringRange
                    If Me.MLVadvanced.GetActiveView Is VIWdropdownlist Then
                        dto.Options = GetOptions(RPTcomboOptions)
                        dto.Type = lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.StringRange
                        dto.MinOption = 1
                        dto.MaxOption = 1
                    End If

            End Select
            If Me.RBLcommentType.SelectedIndex = -1 Then
                dto.CommentType = lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommentType.None
            Else
                dto.CommentType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommentType).GetByString(Me.RBLcommentType.SelectedValue, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommentType.None)
            End If

            Return dto
        End Get
    End Property


    Public Property IdCriterion As Long
        Get
            Return ViewStateOrDefault("IdCriterion", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdCriterion") = value
        End Set
    End Property
    Private Property CriterionType As lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType
        Get
            Return ViewStateOrDefault("CriterionType", lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.None)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType)
            Me.ViewState("CriterionType") = value
        End Set
    End Property
    Private Property AllowSave As Boolean
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property AllowPartialSave As Boolean
        Get
            Return ViewStateOrDefault("AllowPartialSave", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowPartialSave") = value
        End Set
    End Property
    Private Property HasEvaluations As Boolean
        Get
            Return ViewStateOrDefault("HasEvaluations", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("HasEvaluations") = value
        End Set
    End Property
    Private Property HasInEvaluations As Boolean
        Get
            Return ViewStateOrDefault("HasInEvaluations", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("HasInEvaluations") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBcriterionDescription_t)
            .setLabel(LBcriterionMaxChar_t)
            .setLabel(LBcriterionHelp_t)
            .setLabel(LBcommentType_t)
            .setLabel(LBrangeType_t)
            .setLabel(LBrangeMinValue_t)
            .setLabel(LBrangeMaxValue_t)
            .setRadioButtonList(RBLcommentType, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommentType.None.ToString)
            .setRadioButtonList(RBLcommentType, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommentType.Allowed.ToString)
            .setRadioButtonList(RBLcommentType, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommentType.Mandatory.ToString)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(item As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion, containerAllowSave As Boolean, containerAllowPartialSave As Boolean)
        Me.SetInternazionalizzazione()
        AllowSave = containerAllowSave
        AllowPartialSave = containerAllowPartialSave
        HasEvaluations = item.HasEvaluations
        HasInEvaluations = item.HasInEvaluations
        IdCriterion = item.Id

        Me.TXBdescription.Enabled = containerAllowSave OrElse containerAllowPartialSave
        Me.TXBhelp.Enabled = containerAllowSave OrElse containerAllowPartialSave
        Me.TXBmaxChar.Enabled = containerAllowSave OrElse (containerAllowPartialSave AndAlso Not item.HasEvaluations AndAlso item.HasInEvaluations)
        Me.CBXintegerRange.Enabled = containerAllowSave
        Me.TXBminValue.Enabled = containerAllowSave
        Me.TXBmaxValue.Enabled = containerAllowSave
        RBLcommentType.Enabled = containerAllowSave OrElse (containerAllowPartialSave AndAlso Not item.HasEvaluations)
        If IsNothing(item) Then
            Me.MLVcriterion.SetActiveView(VIWempty)
            CriterionType = lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.None
        ElseIf item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.None Then
            Me.MLVcriterion.SetActiveView(VIWempty)
            CriterionType = item.Type
        Else
            Me.MLVcriterion.SetActiveView(VIWcriterion)

            '   Me.DVhelp.Visible = Not (item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Note OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList)
            Me.DVmaxChar.Visible = (item.Type = lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Textual)
            Select Case item.Type
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Textual
                    Me.TXBmaxChar.Text = item.MaxLength
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange
                    Me.CBXintegerRange.Checked = True
                    Me.TXBminValue.Text = Math.Truncate(item.DecimalMinValue)
                    Me.TXBmaxValue.Text = Math.Truncate(item.DecimalMaxValue)


                    Me.DVrangeMaxValue.Visible = True
                    Me.DVrangeMinValue.Visible = True
                    Me.DVrangeType.Visible = True

                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange
                    Me.CBXintegerRange.Checked = False
                    Me.TXBminValue.Text = Math.Round(item.DecimalMinValue, 2)
                    Me.TXBmaxValue.Text = Math.Round(item.DecimalMaxValue, 2)
                    Me.DVrangeMaxValue.Visible = True
                    Me.DVrangeMinValue.Visible = True
                    Me.DVrangeType.Visible = True
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.StringRange
                    Me.MLVadvanced.SetActiveView(VIWdropdownlist)
                    Me.RPTcomboOptions.DataSource = item.Options
                    Me.RPTcomboOptions.DataBind()
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScale, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScaleFuzzy
                    MLVadvanced.SetActiveView(VIWdssValues)
                    RPTcomboDssOptions.DataSource = item.Options
                    RPTcomboDssOptions.DataBind()
                Case Else
                    Me.MLVadvanced.SetActiveView(VIWnone)
            End Select
            Me.TXBdescription.Text = item.Description
            'Me.TXBhelp.Text = item.ToolTip
            RBLcommentType.SelectedValue = item.CommentType.ToString
            CriterionType = item.Type
        End If
    End Sub

    Private Function GetOptions(ByVal rp As Repeater) As List(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption)
        Dim options As New List(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption)
        Dim displayOrder As Integer = 1

        For Each row As RepeaterItem In (From r As RepeaterItem In rp.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim dtoOption As New lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption
            Dim oLiteral As Literal = row.FindControl("LTidOption")
            Dim oTextBox As TextBox = row.FindControl("TXBoptionName")

            Dim hidden As HtmlInputHidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                dtoOption.DisplayOrder = CInt(hidden.Value)
            Else
                dtoOption.DisplayOrder = displayOrder
            End If

            dtoOption.Id = CLng(oLiteral.Text)
            dtoOption.Name = oTextBox.Text
            oTextBox = row.FindControl("TXBoptionValue")
            If Not String.IsNullOrEmpty(oTextBox.Text) AndAlso IsNumeric(oTextBox.Text) Then
                Decimal.TryParse(oTextBox.Text, dtoOption.Value)
            Else
                oTextBox.Text = "0"
                dtoOption.Value = 0
            End If

            displayOrder += 1
            options.Add(dtoOption)
        Next
        displayOrder = 1
        For Each opt As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption In options.OrderBy(Function(o) o.DisplayOrder).ToList
            opt.DisplayOrder = displayOrder
            displayOrder += 1
        Next
        Return options.OrderBy(Function(o) o.DisplayOrder).ToList
    End Function

    Private Sub RPToptions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcomboOptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim fOption As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption = DirectCast(e.Item.DataItem, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption)

            'Dim oLiteral As Literal = e.Item.FindControl("LTidOption")
            'oLiteral.Text = fOption.Id
            Dim oTextBox As TextBox = e.Item.FindControl("TXBoptionName")
            oTextBox.Text = fOption.Name
            oTextBox.Enabled = AllowSave OrElse (AllowPartialSave)

            oTextBox = e.Item.FindControl("TXBoptionValue")
            Dim v As Double = fOption.Value
            If Math.Floor(v) = Math.Round(v, 2) Then
                oTextBox.Text = Math.Floor(v)
            Else
                oTextBox.Text = Math.Round(fOption.Value, 2)
            End If
            oTextBox.Enabled = AllowSave OrElse (AllowPartialSave AndAlso Not HasEvaluations AndAlso Not HasInEvaluations)

            Dim oButton As Button = e.Item.FindControl("BTNdeleteOption")
            oButton.CommandArgument = fOption.Id
            oButton.Visible = AllowSave OrElse (AllowPartialSave AndAlso Not HasEvaluations)
            Me.Resource.setButton(oButton, True, False, False, True)

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = fOption.DisplayOrder

            Dim oLabel As Label = e.Item.FindControl("LBmoveOption")
            oLabel.ToolTip = Resource.getValue("LBmoveOption.Text")
            oLabel.Visible = AllowSave OrElse AllowPartialSave
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LToptionsTitle")
            Me.Resource.setLiteral(oLiteral)

            Dim oLabel As Label = e.Item.FindControl("LBevaluationName_t")
            Me.Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBevaluationValue_t")
            Me.Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oButton As Button = e.Item.FindControl("BTNaddOption")
            oButton.CommandName = "addoption"
            oButton.Visible = AllowSave OrElse AllowPartialSave
            Me.Resource.setButton(oButton, True, False, False, True)

            Dim oGeneric As HtmlControl = e.Item.FindControl("DVfooter")
            oGeneric.Visible = AllowSave OrElse AllowPartialSave
        End If
    End Sub

    Private Sub RPToptions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcomboOptions.ItemCommand
        Dim idOption As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idOption = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            RaiseEvent RemoveOption(idOption)
        ElseIf e.CommandName = "addoption" Then
            Dim name As String = "", value As Decimal = 0
            Dim oTextBox As TextBox = e.Item.FindControl("TXBoptionAddName")
            name = oTextBox.Text
            oTextBox = e.Item.FindControl("TXBoptionAddValue")
            Decimal.TryParse(oTextBox.Text, value)
            RaiseEvent AddOption(IdCriterion, name, Math.Round(value, 2))
        End If
    End Sub

    Private Sub CBXintegerRange_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXintegerRange.CheckedChanged
        If CBXintegerRange.Checked AndAlso CriterionType = lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange Then
            Dim minV, maxV As Integer
            Integer.TryParse(TXBminValue.Text, minV)
            Integer.TryParse(TXBmaxValue.Text, maxV)
            If minV > maxV Then
                Dim t As Double = minV
                minV = maxV
                maxV = minV
            ElseIf maxV = 0 Then
                maxV = 1
            End If
            RaiseEvent ChangeToIntegerType(IdCriterion, minV, maxV)
        ElseIf Not CBXintegerRange.Checked AndAlso CriterionType = lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange Then
            Dim minV, maxV As Decimal
            Decimal.TryParse(TXBminValue.Text, minV)
            Decimal.TryParse(TXBmaxValue.Text, maxV)
            If minV > maxV Then
                Dim t As Decimal = minV
                minV = maxV
                maxV = minV
            ElseIf maxV = 0 Then
                maxV = 1
            End If
            RaiseEvent ChangeToDecimalType(IdCriterion, Math.Round(minV, 2), Math.Round(maxV, 2))
        End If
    End Sub


    Private Sub RPTcomboDssOptions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcomboDssOptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim fOption As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption = DirectCast(e.Item.DataItem, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption)
            Dim oLiteral As Literal = e.Item.FindControl("LTnumber")
            Dim oControl As UC_FuzzyNumber = e.Item.FindControl("CTRLfuzzyNumber")
            oLiteral.Visible = Not fOption.IsFuzzy
            oControl.Visible = fOption.IsFuzzy
            If fOption.IsFuzzy Then
                oControl.InitializeControl(fOption.IsFuzzy, fOption.DoubleValue, fOption.FuzzyValue, fOption.DoubleValue.ToString, fOption.ShortName)
            Else
                Dim v As Double = fOption.DoubleValue
                If Math.Floor(v) = Math.Round(v, 3) Then
                    oLiteral.Text = Math.Floor(v).ToString()
                Else
                    oLiteral.Text = Math.Round(fOption.DoubleValue, 3).ToString()
                End If
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LToptionsTitle")
            Me.Resource.setLiteral(oLiteral)

            Dim oLabel As Label = e.Item.FindControl("LBevaluationName_t")
            Me.Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBevaluationValue_t")
            Me.Resource.setLabel(oLabel)

        End If
    End Sub
End Class