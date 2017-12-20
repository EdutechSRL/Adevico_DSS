Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class UC_AddCriterion
    Inherits BaseControl
    Implements IViewAddCriterion


#Region "Context"
    Private _Presenter As AddCriterionPresenter
    Private ReadOnly Property CurrentPresenter() As AddCriterionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddCriterionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CurrentType As CriterionType Implements IViewAddCriterion.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", CriterionType.Textual)
        End Get
        Set(value As CriterionType)
            ViewState("CurrentType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewAddCriterion.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
    Private Property UseDss As Boolean Implements IViewAddCriterion.UseDss
        Get
            Return ViewStateOrDefault("UseDss", False)
        End Get
        Set(value As Boolean)
            ViewState("UseDss") = value
        End Set
    End Property
#End Region

#Region "Control property/events"
    Public Event RefreshContainer()
    Public Property AjaxEnabled As Boolean
        Get
            Return ViewStateOrDefault("AjaxEnabled", False)
        End Get
        Set(value As Boolean)
            ViewState("AjaxEnabled") = value
        End Set
    End Property
    Private Property AvailableCriteria As List(Of dtoCriterionEvaluated)
        Get
            Return ViewStateOrDefault("AvailableCriteria", New List(Of dtoCriterionEvaluated))
        End Get
        Set(value As List(Of dtoCriterionEvaluated))
            ViewState("AvailableCriteria") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBgenericCriteria)
            .setLabel(LBcriterionOptions)
            .setLabel(LBstandardOptionsDescription)
            .setLabel(LBstandardCriteriaNumber_t)
            .setLabel(LBstandardCriteriaNumberHelp)

            .setLabel(LBmultipleCriterionOptions)
            .setLabel(LBadvancedCriterionOptionsDescription)
            .setLabel(LBadvancedCriterionOptionsList)
            .setLabel(LBadvancedCriterionOptionsListHelp)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idCall As Long, useDss As Boolean) Implements IViewAddCriterion.InitializeControl
        CurrentPresenter.InitView(idCall, useDss)
    End Sub
    Public Sub InitializeControl(idCall As Long, settings As lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings, method As lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod) Implements IViewAddCriterion.InitializeControl
        CurrentPresenter.InitView(idCall, True, settings, method)
    End Sub
    Private Sub LoadAvailableTypes(items As List(Of DisplayCriterionType)) Implements IViewAddCriterion.LoadAvailableTypes
        Me.MLVaddCriterion.SetActiveView(VIWfield)
        For Each item As DisplayCriterionType In items
            item.Name = Resource.getValue("CriterionSelctor.CriterionType." & item.Type.ToString)
            Dim oLabel As Label = Me.FindControl("LBtypes" & item.Type.ToString)
            oLabel.Text = item.Name
        Next

        LBtypesBoolean.Text = Resource.getValue("CriterionSelctor.CriterionType.Boolean") ' & item.Type.ToString)


        LIcriterionTypeStringRange.Visible = items.Any(Function(i) i.Type = CriterionType.StringRange)
        LIcriterionTypeRatingScaleFuzzy.Visible = items.Any(Function(i) i.Type = CriterionType.RatingScaleFuzzy)
        LIcriterionTypeRatingScale.Visible = items.Any(Function(i) i.Type = CriterionType.RatingScale)

        RBtypesBoolean.Checked = False
        RBtypesTextual.Checked = False
        RBtypesDecimalRange.Checked = False
        RBtypesStringRange.Checked = False
        RBtypesRatingScale.Checked = False
        RBtypesRatingScaleFuzzy.Checked = False
        RBtypesIntegerRange.Checked = True
        UpdatePreview(CriterionType.IntegerRange)
    End Sub
    Private Sub LoadCriteria(criteria As List(Of dtoCriterionEvaluated)) Implements IViewAddCriterion.LoadCriteria
        For Each item As dtoCriterionEvaluated In criteria
            item.Criterion = GenerateCommiteeCriterion(item.Criterion)


        Next
        AvailableCriteria = criteria
    End Sub

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewAddCriterion.DisplayNoPermission
        Me.MLVaddCriterion.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewAddCriterion.DisplaySessionTimeout
        Me.MLVaddCriterion.SetActiveView(VIWempty)
    End Sub

    Public Function CreateCriteria(committees As List(Of dtoCommittee), settings As lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings, idCommittee As Long) As List(Of BaseCriterion) Implements IViewAddCriterion.CreateCriteria
        Return CurrentPresenter.AddCriteria(committees, settings, idCommittee, GetCriteriaToCreate())
    End Function

    Public Function CreateCriteriaAdv(CommId As Int64) As List(Of BaseCriterion) '?? Implements IViewAddCriterion.CreateCriteria
        Return CurrentPresenter.AddCriteriaAdv(CommId, GetCriteriaToCreate())
    End Function


    Public Function GetCriteriaToCreate() As List(Of dtoCriterion) Implements IViewAddCriterion.GetCriteriaToCreate
        Dim items As New List(Of dtoCriterion)

        Dim crType As CriterionType = Me.CurrentType
        Select Case crType
            Case CriterionType.RatingScale, CriterionType.RatingScaleFuzzy
                Dim isFuzzy As Boolean = (crType = CriterionType.RatingScaleFuzzy)
                Dim scales As Dictionary(Of Long, Long) = Nothing
                Dim values As Dictionary(Of Long, List(Of lm.Comol.Core.Dss.Domain.dtoGenericRatingValue)) = Nothing
                If crType = CriterionType.RatingScale Then
                    scales = CTRLscales.GetRatingSetSelected()
                    values = CTRLscales.GetRatingValues()
                Else
                    scales = CTRLfuzzyScales.GetRatingSetSelected()
                    values = CTRLfuzzyScales.GetRatingValues()
                End If

                For Each item As KeyValuePair(Of Long, Long) In scales.Where(Function(s) s.Value >= 1)
                    For i As Integer = 1 To item.Value
                        Dim dto As New dtoCriterion()
                        Dim dCriterion As dtoCriterion = AvailableCriteria.Where(Function(c) c.Criterion.Type = crType).Select(Function(c) c.Criterion).FirstOrDefault()
                        With dto
                            .CommentType = CommentType.None
                            .Type = crType
                            .UseDss = UseDss
                            Select Case crType
                                Case CriterionType.RatingScale
                                    .IsFuzzy = False
                                Case CriterionType.RatingScaleFuzzy
                                    .IsFuzzy = True
                            End Select
                            .IdRatingSet = item.Key
                            .Options = New List(Of dtoCriterionOption)
                            If values.ContainsKey(item.Key) Then
                                Dim optIndex As Long = 1
                                For Each x As lm.Comol.Core.Dss.Domain.dtoGenericRatingValue In values(item.Key)
                                    .Options.Add(New dtoCriterionOption() With {.DisplayOrder = optIndex, .DoubleValue = x.Value,
                                                                                .FuzzyValue = x.FuzzyValue, .IdRatingSet = item.Key,
                                                                                 .IdRatingValue = x.Id, .IsFuzzy = x.IsFuzzy, .Name = x.Name,
                                                                                 .UseDss = True})
                                    optIndex += 1
                                Next
                            End If
                            .Name = Resource.getValue("CriterionCreator.CriterionType." & crType.ToString & ".Text")
                            .Description = Resource.getValue("CriterionCreator.CriterionType." & crType.ToString & ".Description")
                        End With
                        items.Add(dto)
                    Next
                Next
            Case Else
                Dim criterionCount As Integer = 1
                If Not String.IsNullOrEmpty(TXBstandardCriteriaNumber.Text) AndAlso IsNumeric(TXBstandardCriteriaNumber.Text) Then
                    criterionCount = CInt(TXBstandardCriteriaNumber.Text)
                    If criterionCount < 1 Then
                        criterionCount = 1
                    End If
                End If

                For i As Integer = 1 To criterionCount
                    Dim dto As New dtoCriterion()
                    Dim dCriterion As dtoCriterion = AvailableCriteria.Where(Function(c) c.Criterion.Type = crType).Select(Function(c) c.Criterion).FirstOrDefault()
                    With dto
                        .CommentType = CommentType.None
                        .Type = crType
                        .UseDss = UseDss
                        Select Case crType
                            Case CriterionType.StringRange

                                .Options = New List(Of dtoCriterionOption)
                                .MinOption = 1 'IIf(fType = CriterionType.CheckboxList, 0, 1)
                                Dim index As Integer = 1
                                If Not String.IsNullOrEmpty(Me.TXBcriterionOptions.Text) Then
                                    Dim fOptions As List(Of String) = Me.TXBcriterionOptions.Text.Split(CChar(vbCrLf)).ToList()
                                    For Each opt As String In fOptions.Where(Function(f) Not String.IsNullOrEmpty(f)).ToList
                                        .Options.Add(New dtoCriterionOption() With {.Name = opt, .Value = index})
                                        index += 1
                                    Next
                                Else
                                    For index = 1 To 3
                                        .Options.Add(New dtoCriterionOption() With {.Name = String.Format(Resource.getValue("OptionDefault"), index), .Value = index})
                                    Next
                                End If
                                .MaxOption = 1 ' IIf(fType = CriterionType.CheckboxList, .Options.Count, 1)
                            Case CriterionType.Textual
                                .MaxLength = dCriterion.MaxLength
                            Case CriterionType.DecimalRange, CriterionType.IntegerRange
                                .DecimalMinValue = dCriterion.DecimalMinValue
                                .DecimalMaxValue = dCriterion.DecimalMaxValue
                        End Select

                        .Name = Resource.getValue("CriterionCreator.CriterionType." & crType.ToString & ".Text")
                        .Description = Resource.getValue("CriterionCreator.CriterionType." & crType.ToString & ".Description")
                        Select Case crType
                            Case CriterionType.DecimalRange
                                If Not String.IsNullOrEmpty(.Description) Then
                                    .Description = String.Format(.Description, dto.DecimalMinValue, dto.DecimalMaxValue)
                                End If
                            Case CriterionType.StringRange
                                If dto.MinOption < 0 OrElse dto.MaxOption > 1 Then
                                    If dto.MinOption > 0 AndAlso dto.MaxOption Then
                                        dto.Description = String.Format(Resource.getValue("CriterionCreator.CriterionType." & dto.Type.ToString & ".Description.Min.Max"), dto.MinOption, dto.MaxOption)
                                    ElseIf dto.MaxOption > 0 Then
                                        dto.Description = String.Format(Resource.getValue("CriterionCreator.CriterionType." & dto.Type.ToString & ".Description.Max"), dto.MaxOption)
                                    ElseIf dto.Description > 0 Then
                                        dto.Description = String.Format(Resource.getValue("CriterionCreator.CriterionType." & dto.Type.ToString & ".Description.Min"), dto.MinOption)
                                    End If
                                End If
                        End Select
                        'If crType = CriterionType.StringRange Then
                        '    If dto.MinOption > 0 AndAlso dto.MaxOption Then
                        '        dto.ToolTip = String.Format(Resource.getValue("CriterionCreator.CriterionType." & dto.Type.ToString & ".Description.Min.Max"), dto.MinOption, dto.MaxOption)
                        '    ElseIf dto.MaxOption > 0 Then
                        '        dto.ToolTip = String.Format(Resource.getValue("CriterionCreator.CriterionType." & dto.Type.ToString & ".Description.Max"), dto.MaxOption)
                        '    ElseIf dto.MinOption > 0 Then
                        '        dto.ToolTip = String.Format(Resource.getValue("CriterionCreator.CriterionType." & dto.Type.ToString & ".Description.Min"), dto.MinOption)
                        '    End If
                        'Else
                        '    .ToolTip = Resource.getValue("CriterionCreator.FieldCreator." & fType.ToString & ".Help")
                        'End If
                    End With
                    items.Add(dto)
                Next
        End Select

        Return items
    End Function
    Private Sub InitializeRatingScale(method As lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod) Implements IViewAddCriterion.InitializeRatingScale
        If Not IsNothing(method) Then
            If method.IsFuzzy Then
                CTRLfuzzyScales.InitializeControl(method)
            Else
                CTRLscales.InitializeControl(method)
            End If
        End If
    End Sub
    Private Sub InitializeRatingScale(algorithmType As lm.Comol.Core.Dss.Domain.AlgorithmType, isFuzzy As Boolean) Implements IViewAddCriterion.InitializeRatingScale
        If isFuzzy Then
            CTRLfuzzyScales.InitializeControl(algorithmType, isFuzzy)
        Else
            CTRLscales.InitializeControl(algorithmType, isFuzzy)
        End If
    End Sub
#End Region

#Region "Internal"
    Private Function GenerateCommiteeCriterion(ByVal dto As dtoCriterion) As dtoCriterion
        Dim result As dtoCriterion = dto

        '  If result.Type <> CriterionType.Note Then
        result.Name = Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Text")
        '  End If
        result.Description = Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Description")

        'If result.Type <>   Then
        '    result.ToolTip = Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Help")
        'End If

        If result.Type = CriterionType.StringRange AndAlso dto.MinOption < 0 OrElse dto.MaxOption > 1 Then
            If dto.MinOption > 0 AndAlso dto.MaxOption Then
                dto.Description = String.Format(Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Description.Min.Max"), dto.MinOption, dto.MaxOption)
            ElseIf dto.MaxOption > 0 Then
                dto.Description = String.Format(Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Description.Max"), dto.MaxOption)
            ElseIf dto.MinOption > 0 Then
                dto.Description = String.Format(Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Description.Min"), dto.MinOption)
            End If
        End If
        ' Select Case dto.Type
        '    Case CriterionType.StringRange
        'If dto.MinOption > 0 AndAlso dto.MaxOption Then
        '    dto.ToolTip = String.Format(Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Description.Min.Max"), dto.MinOption, dto.MaxOption)
        'ElseIf dto.MaxOption > 0 Then
        '    dto.ToolTip = String.Format(Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Description.Max"), dto.MaxOption)
        'ElseIf dto.MinOption > 0 Then
        '    dto.ToolTip = String.Format(Resource.getValue("CriterionSelctor.CriterionType." & dto.Type.ToString & ".Description.Min"), dto.MinOption)
        'End If
        If dto.Type <> CriterionType.RatingScale AndAlso dto.Type <> CriterionType.RatingScaleFuzzy Then
            For Each opt As dtoCriterionOption In dto.Options
                opt.Name = String.Format(Resource.getValue("OptionDefault"), opt.Value)
            Next
        End If
        
        'Case CriterionType.DropDownList, CriterionType.RadioButtonList
        '    For Each opt As dtoFieldOption In dto.Options
        '        opt.Name = String.Format(Resource.getValue("OptionDefault"), opt.Value)
        '    Next
        '    End Select
        Return result
    End Function

    Protected Sub RBcriterionType_CheckedChanged(sender As Object, e As System.EventArgs)
        Dim itemType As CriterionType = sender.attributes("value")

        UpdatePreview(itemType)
    End Sub

    Private Sub UpdatePreview(itemType As CriterionType)

        Dim isMultiple As Boolean = itemType = CriterionType.StringRange

        CurrentType = itemType

        Dim criterionName As String = Resource.getValue("CriterionSelctor.CriterionType." & itemType.ToString)
        LBcriterionName.Text = criterionName
        CTRLinputCriterion.InitializeControl(0, 0, 0, 0, AvailableCriteria.Where(Function(f) f.Criterion.Type = itemType).FirstOrDefault(), True, False)

        TXBstandardCriteriaNumber.Text = 1
        If isMultiple Then
            Resource.setLabel(LBmultipleCriterionOptions)
            LBmultipleCriterionOptions.Text = String.Format(LBmultipleCriterionOptions.Text, criterionName)
        Else
            Resource.setLabel(LBcriterionOptions)
            LBcriterionOptions.Text = String.Format(LBcriterionOptions.Text, criterionName)
        End If
        LBcriterionOptions.Visible = Not isMultiple
        LBmultipleCriterionOptions.Visible = isMultiple
        DVmultipleChoiceCriteria.Visible = isMultiple
        DVmultipleChoiceCriteriaDescription.Visible = isMultiple

        DVstandardCriteriaDescription.Visible = (itemType <> CriterionType.RatingScale AndAlso itemType <> CriterionType.RatingScaleFuzzy)
        CTRLscales.Visible = (itemType = CriterionType.RatingScale)
        CTRLfuzzyScales.Visible = (itemType = CriterionType.RatingScaleFuzzy)
    End Sub
#End Region

    Public Property IsAdvance As Boolean
        Get
            Return ViewStateOrDefault("CriterionForAdvance", False)
        End Get
        Set(value As Boolean)
            ViewState("CriterionForAdvance") = value
        End Set
    End Property

End Class