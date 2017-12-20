Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class UC_InputCriterion
    Inherits BaseControl
    Implements IViewInputCriterion

#Region "Context"
    Private _Presenter As InputCriterionPresenter
    Private ReadOnly Property CurrentPresenter() As InputCriterionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InputCriterionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property Disabled As Boolean Implements IViewInputCriterion.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
            Me.TXBcomment.Enabled = Not value
            Me.TXBdecimalrange.Enabled = Not value
            Me.DDLintegerRange.Enabled = Not value
            Me.DDLstringRange.Enabled = Not value
            Me.TXBtextual.Enabled = Not value
        End Set
    End Property
    Public Property CriterionType As CriterionType Implements IViewInputCriterion.CriterionType
        Get
            Return ViewStateOrDefault("CriterionType", CriterionType.Textual)
        End Get
        Set(value As CriterionType)
            ViewState("CriterionType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewInputCriterion.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
    Public Property IdCriterion As Long Implements IViewInputCriterion.IdCriterion
        Get
            Return ViewStateOrDefault("IdCriterion", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCriterion") = value
        End Set
    End Property
    Private Property IdCriterionEvaluated As Long Implements IViewInputCriterion.IdCriterionEvaluated
        Get
            Return ViewStateOrDefault("IdCriterionEvaluated", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCriterionEvaluated") = value
        End Set
    End Property
    Private Property IdSubmission As Long Implements IViewInputCriterion.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdEvaluation As Long Implements IViewInputCriterion.IdEvaluation
        Get
            Return ViewStateOrDefault("IdEvaluation", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdEvaluation") = value
        End Set
    End Property
    Private Property Mandatory As Boolean Implements IViewInputCriterion.Mandatory
        Get
            Return ViewStateOrDefault("Mandatory", True)
        End Get
        Set(value As Boolean)
            ViewState("Mandatory") = value
        End Set
    End Property

    Public ReadOnly Property isValid As Boolean Implements IViewInputCriterion.isValid
        Get
            Dim isMandatory As Boolean = Me.Mandatory
            Select Case CriterionType
                'Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                '    Dim selected As Integer = (From i As ListItem In Me.CBLitems.Items Where i.Selected).Count
                '    Return (selected >= MinOptions AndAlso selected <= MaxOptions)
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange
                    Dim v As Decimal
                    Decimal.TryParse(Me.TXBdecimalrange.Text, v)
                    Return (Me.REVdecimal.MinimumValue >= v AndAlso Me.REVdecimal.MaximumValue <= v) AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.StringRange
                    Return Me.DDLstringRange.SelectedIndex <> -1 AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))
                    'Dim selected As Integer = (From i As ListItem In Me.RBLitems.Items Where i.Selected).Count
                    'Return (selected >= MinOptions AndAlso selected <= MaxOptions) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Me.RBLitems.SelectedIndex <> -1))

                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Textual
                    Return Not String.IsNullOrEmpty(Me.TXBtextual.Text) AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))
                Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange
                    Return Me.DDLintegerRange.SelectedIndex <> -1 AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))

            End Select
        End Get
    End Property

    'Private Property MaxChars As Integer Implements IViewInputCriterion.MaxChars
    '    Get
    '        Return ViewStateOrDefault("MaxChars", CInt(0))
    '    End Get
    '    Set(value As Integer)
    '        ViewState("MaxChars") = value
    '    End Set
    'End Property

    Private Property MaxOptions As Integer Implements IViewInputCriterion.MaxOptions
        Get
            Return ViewStateOrDefault("MaxOptions", CInt(1))
        End Get
        Set(value As Integer)
            ViewState("MaxOptions") = value
        End Set
    End Property
    Private Property MinOptions As Integer Implements IViewInputCriterion.MinOptions
        Get
            Return ViewStateOrDefault("MinOptions", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MinOptions") = value
        End Set
    End Property
    Private Property MinValue As Decimal Implements IViewInputCriterion.MinValue
        Get
            Return ViewStateOrDefault("MinValue", CDec(0))
        End Get
        Set(value As Decimal)
            ViewState("MinValue") = value
        End Set
    End Property
    Private Property MaxValue As Decimal Implements IViewInputCriterion.MaxValue
        Get
            Return ViewStateOrDefault("MaxValue", MinValue + 1)
        End Get
        Set(value As Decimal)
            ViewState("MaxValue") = value
        End Set
    End Property
    Public Property CurrentError As lm.Comol.Modules.CallForPapers.Domain.FieldError Implements IViewInputCriterion.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", lm.Comol.Modules.CallForPapers.Domain.FieldError.None)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.FieldError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewInputCriterion.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property AvailableOptions As List(Of dtoCriterionOption) Implements IViewInputCriterion.AvailableOptions
        Get
            Return ViewStateOrDefault("AvailableOptions", New List(Of dtoCriterionOption))
        End Get
        Set(value As List(Of dtoCriterionOption))
            ViewState("AvailableOptions") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected ReadOnly Property CssError As String
        Get
            Return IIf(CurrentError = lm.Comol.Modules.CallForPapers.Domain.FieldError.None, "", "error")
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LTmaxCharsComment)
            '.setLabel(LBcommentText)
            .setLabel(LBerrorMessageComment)
        End With
    End Sub
#End Region
    Public Sub InitializeControl(idCall As Long, idSubmission As Long, idEvaluation As Long, idCommunity As Integer, item As dtoCriterionEvaluated, disabled As Boolean) Implements IViewInputCriterion.InitializeControl
        CurrentError = lm.Comol.Modules.CallForPapers.Domain.FieldError.None
        Me.CurrentPresenter.InitView(idCall, idSubmission, idEvaluation, idCommunity, item, disabled)
        Me.SetInternazionalizzazione()
    End Sub
    Public Sub InitializeControl(idCall As Long, idSubmission As Long, idEvaluation As Long, idCommunity As Integer, item As dtoCriterionEvaluated, disabled As Boolean, err As lm.Comol.Modules.CallForPapers.Domain.FieldError) Implements IViewInputCriterion.InitializeControl
        Me.SetInternazionalizzazione()
        CurrentError = err
        Me.CurrentPresenter.InitView(idCall, idSubmission, idEvaluation, idCommunity, item, disabled)

        If Not IsNothing(item) AndAlso Not IsNothing(item.Criterion) Then
            Dim oLabel As Label = Me.FindControl("LBerrorMessage" & item.Criterion.Type.ToString.ToLower)

            If Not IsNothing(oLabel) Then
                oLabel.Visible = Not (err = lm.Comol.Modules.CallForPapers.Domain.FieldError.None)

                If Not (err = lm.Comol.Modules.CallForPapers.Domain.FieldError.None) Then
                    oLabel.Text = Resource.getValue("FieldError." & err.ToString())
                End If
            End If
        End If
    End Sub

    Private Sub SetupView(item As dtoCriterionEvaluated, idCommunity As Integer) Implements IViewInputCriterion.SetupView
        IdCriterionEvaluated = item.IdValueCriterion
        IdCriterion = item.IdCriterion
        MinValue = item.Criterion.DecimalMinValue
        MaxValue = item.Criterion.DecimalMaxValue
        CriterionType = item.Criterion.Type
        Mandatory = item.CommentMandatory
        LBcriterionDescription_t.Text = item.Criterion.Description

        'If item.Criterion.Then Then
        '    oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Help")
        '    oLabel.Visible = Not String.IsNullOrEmpty(item.Field.ToolTip)
        '    oLabel.Text = item.Field.ToolTip
        'End If

        Dim oLabel As Label = Me.FindControl("LB" & item.Criterion.Type.ToString.ToLower & "Text")
        Select Case item.Criterion.Type
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScale, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScaleFuzzy
                CTRLfuzzyInput.TranslationWeightTitle = item.Criterion.Name
            Case Else
                oLabel.Text = item.Criterion.Name
        End Select

        Me.CommentType = item.Criterion.CommentType

        Me.SPNmaxChartextual.Visible = False


        Resource.setLabel(LBcommentText)


        'Select Case item.Criterion.CommentType
        '    Case CommentType.Allowed

        '    Case CommentType.Mandatory
        '        Me.DVcomment.Visible = True
        '        LBcommentText.Text = String.Format("<span class=""mandatory comment"">*</span>{0}:", LBcommentText.Text)
        'End Select

        If item.Criterion.CommentType <> CommentType.None Then
            Me.DVcomment.Visible = True

            If item.Criterion.CommentType = CommentType.Mandatory Then
                'LBcommentText.Text = Replace(LBcommentText.Text, ":", "<span class=""mandatory"">*</span>:")
                LBcommentText.Text = String.Format("<span class=""mandatory comment"">*</span>{0}", LBcommentText.Text)
            End If
            If item.Criterion.CommentMaxLength > 0 AndAlso Not Disabled Then
                TXBcomment.Attributes.Add("maxlength", item.Criterion.CommentMaxLength)
                Me.SPNmaxCharsComment.Visible = True
            End If
            Me.TXBcomment.Text = item.Comment
            Me.LBcommentHelp.Text = Resource.getValue("Help.CommentType." & item.Criterion.CommentType.ToString())
        End If

        'If Not IsNothing(oTextBox) Then
        Dim cssClass = Me.DVcriterion.Attributes("class")
        If (item.CriterionError = lm.Comol.Modules.CallForPapers.Domain.FieldError.None) Then
            cssClass = Replace(cssClass, " error", "")
        Else
            If Not cssClass.Contains(" error") Then
                cssClass &= " error"
            End If

        End If
        DVcriterion.Attributes("class") = cssClass


        Dim oValidator As RequiredFieldValidator
        Select Case item.Criterion.Type
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange
                Me.DDLintegerRange.DataSource = (From i As Integer In Enumerable.Range(item.Criterion.DecimalMinValue, (item.Criterion.DecimalMaxValue - item.Criterion.DecimalMinValue) + 1) Select i).ToList()
                Me.DDLintegerRange.DataBind()

                Me.DDLintegerRange.Items.Insert(0, New ListItem(Resource.getValue("Select.vote"), item.Criterion.DecimalMinValue - 1))
                Try
                    If item.IsValueEmpty Then
                        Me.DDLintegerRange.SelectedIndex = 0
                    Else
                        Me.DDLintegerRange.SelectedValue = Decimal.ToInt32(item.DecimalValue)
                    End If
                Catch ex As Exception
                    Me.DDLintegerRange.SelectedIndex = 0
                End Try

                Me.MinOptions = item.Criterion.MinOption
                Me.MaxOptions = item.Criterion.MaxOption
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange
                Me.REVdecimal.MinimumValue = item.Criterion.DecimalMinValue
                Me.REVdecimal.MaximumValue = item.Criterion.DecimalMinValue
                Me.REVdecimal.Type = ValidationDataType.Double
                If item.IsValueEmpty Then
                    Me.TXBdecimalrange.Text = ""
                Else
                    Me.TXBdecimalrange.Text = Math.Round(item.DecimalValue, 2).ToString
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Textual
                Me.TXBtextual.Text = item.StringValue

                If item.Criterion.MaxLength > 0 Then
                    TXBtextual.Attributes.Add("maxlength", item.Criterion.MaxLength)
                End If
                Me.SPNmaxChartextual.Visible = (item.Criterion.MaxLength > 0)
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.StringRange
                AvailableOptions = item.Criterion.Options
                Me.DDLstringRange.DataSource = item.Criterion.Options
                Me.DDLstringRange.DataTextField = "Name"
                Me.DDLstringRange.DataValueField = "Id"
                Me.DDLstringRange.DataBind()

                Me.DDLstringRange.Items.Insert(0, New ListItem(Resource.getValue("Select.vote"), item.Criterion.Options.Select(Function(o) o.Value).Min() - 1))
                If item.IsValueEmpty Then
                    Me.DDLstringRange.SelectedIndex = 0
                Else
                    Dim oItem As ListItem = Me.DDLstringRange.Items.FindByValue(item.IdOption)
                    If Not IsNothing(oItem) Then
                        oItem.Selected = True
                    Else
                        Me.DDLstringRange.SelectedIndex = 0
                    End If
                End If

                Me.MinOptions = item.Criterion.MinOption
                Me.MaxOptions = item.Criterion.MaxOption
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScale
                AvailableOptions = item.Criterion.Options
                MinOptions = 1
                MaxOptions = 1
                CTRLfuzzyInput.InitializeControl(item.Criterion.MethodSettings.IdMethod, item.Criterion.GetDssRatingSet(), item.DssValue, Disabled)
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScaleFuzzy
                AvailableOptions = item.Criterion.Options
                MinOptions = 1
                MaxOptions = 1
                CTRLfuzzyInput.InitializeControl(item.Criterion.MethodSettings.IdMethod, item.Criterion.GetDssRatingSet(), item.DssValue, Disabled)
            Case CriterionType.Boolean

                If Not item.IsValueEmpty Then
                    If (item.DecimalValue > 0) Then
                        Me.RBLboolean.SelectedValue = "1"
                    Else
                        Me.RBLboolean.SelectedValue = "0"
                    End If
                End If

                'If (item.DecimalValue > 0) Then
                '    Me.CBXboolean.Checked = True
                'Else
                '    Me.CBXboolean.Checked = False
                'End If
        End Select
        Me.MLVcriterion.SetActiveView(VIWcriterion)
        Select Case CriterionType
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScale, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScaleFuzzy
                MLVcriterionType.SetActiveView(VIWdss)
            Case Else
                Dim view As System.Web.UI.WebControls.View = Me.FindControl("VIW" & item.Criterion.Type.ToString.ToLower)
                If Not IsNothing(view) Then
                    Me.MLVcriterionType.SetActiveView(view)
                End If
        End Select
    End Sub


    Private Property CommentType As CommentType
        Get
            Return ViewStateOrDefault("CurrentCommentType", CommentType.None)
        End Get
        Set(value As CommentType)
            ViewState("CurrentCommentType") = value
        End Set
    End Property

    Public Function GetCriterion() As dtoCriterionEvaluated Implements IViewInputCriterion.GetCriterion
        Dim dto As New dtoCriterionEvaluated
        dto.Criterion = New dtoCriterion
        dto.IdValueCriterion = IdCriterionEvaluated
        dto.Criterion.Id = IdCriterion
        dto.Criterion.Type = Me.CriterionType
        dto.Criterion.DecimalMaxValue = MaxValue
        dto.Criterion.DecimalMinValue = MinValue
        dto.Criterion.MaxOption = MaxOptions
        dto.Criterion.MinOption = MinOptions

        dto.Criterion.CommentType = CommentType

        dto.Comment = Me.TXBcomment.Text
        dto.IsValueEmpty = False
        Dim oLabel As Label = Me.FindControl("LB" & dto.Criterion.Type.ToString.ToLower & "Text")
        If Not IsNothing(oLabel) Then
            dto.Criterion.Name = oLabel.Text
        End If
        Select Case CriterionType
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Textual
                dto.StringValue = Me.TXBtextual.Text
                dto.IsValueEmpty = String.IsNullOrEmpty(Me.TXBtextual.Text)
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange
                If String.IsNullOrEmpty(Me.TXBdecimalrange.Text) Then
                    dto.IsValueEmpty = True
                Else
                    Decimal.TryParse(Me.TXBdecimalrange.Text, dto.DecimalValue)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange
                If Me.DDLintegerRange.SelectedIndex <= 0 Then
                    dto.IsValueEmpty = True
                    dto.DecimalValue = 0
                Else
                    dto.DecimalValue = CDec(Me.DDLintegerRange.SelectedValue)
                End If

            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.StringRange
                If DDLstringRange.SelectedIndex <= 0 Then
                    dto.IdOption = 0
                    dto.IsValueEmpty = True
                Else
                    dto.IdOption = CInt(DDLstringRange.SelectedValue)
                    dto.DecimalValue = AvailableOptions.Where(Function(o) o.Id = dto.IdOption).Select(Function(o) o.Value).FirstOrDefault()
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScale, lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.RatingScaleFuzzy
                Dim settings As lm.Comol.Core.Dss.Domain.Templates.dtoItemRating = CTRLfuzzyInput.GetItemRating()
                If settings.Error <> lm.Comol.Core.Dss.Domain.DssError.None Then
                    dto.IsValueEmpty = True
                End If
                dto.DssValue = settings
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Boolean
                dto.IsValueEmpty = True

                If RBLboolean.SelectedIndex > -1 Then
                    If RBLboolean.SelectedValue = "1" Then
                        dto.DecimalValue = 1
                        dto.IsValueEmpty = False
                    ElseIf RBLboolean.SelectedValue = "0" Then
                        dto.DecimalValue = 0
                        dto.IsValueEmpty = False
                    End If
                End If





                'If (Me.CBXboolean.Checked) Then
                '    dto.DecimalValue = 1
                'Else
                '    dto.DecimalValue = 0
                'End If
        End Select
        Return (dto)
    End Function

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewInputCriterion.DisplayNoPermission
        Me.MLVcriterion.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewInputCriterion.DisplaySessionTimeout
        Me.MLVcriterion.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyCriterion() Implements IViewInputCriterion.DisplayEmptyCriterion
        Me.MLVcriterion.SetActiveView(VIWempty)
    End Sub

    Public Sub DisplayInputError() Implements IViewInputCriterion.DisplayInputError

    End Sub
    Private Sub HideInputError() Implements IViewInputCriterion.HideInputError

    End Sub

End Class