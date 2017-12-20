Imports lm.Comol.Core.Dss.Domain.Templates
Imports lm.Comol.Core.Dss.Presentation.Evaluation
Imports lm.Comol.Core.Dss.Domain

Public Class UC_DssAggregationSelector
    Inherits DSSbaseControl
    Implements IViewAggregationSelector

#Region "Context"
    Private _Presenter As AggregationSelectorPresenter
    Private ReadOnly Property CurrentPresenter() As AggregationSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AggregationSelectorPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdFatherMethod As Long Implements IViewAggregationSelector.IdFatherMethod
        Get
            Return ViewStateOrDefault("IdFatherMethod", 0)
        End Get
        Set(value As Long)
            ViewState("IdFatherMethod") = value
        End Set
    End Property
    Public Property IsDefaultForChildren As Boolean Implements IViewAggregationSelector.IsDefaultForChildren
        Get
            Return ViewStateOrDefault("IsDefaultForChildren", False)
        End Get
        Set(value As Boolean)
            ViewState("IsDefaultForChildren") = value
        End Set
    End Property
    Public Property AllowInheritsFromFather As Boolean Implements IViewAggregationSelector.AllowInheritsFromFather
        Get
            Return ViewStateOrDefault("AllowInheritsFromFather", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowInheritsFromFather") = value
        End Set
    End Property
    Public Property RaiseEventForMethodSelect As Boolean Implements IViewAggregationSelector.RaiseEventForMethodSelect
        Get
            Return ViewStateOrDefault("RaiseEventForMethodSelect", True)
        End Get
        Set(value As Boolean)
            ViewState("RaiseEventForMethodSelect") = value
        End Set
    End Property
    Public Property RaiseEventForRatingSetSelect As Boolean Implements IViewAggregationSelector.RaiseEventForRatingSetSelect
        Get
            Return ViewStateOrDefault("RaiseEventForRatingSetSelect", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseEventForRatingSetSelect") = value
        End Set
    End Property
    Public Property IdObjectItem As Long Implements IViewAggregationSelector.IdObjectItem
        Get
            Return ViewStateOrDefault("IdObjectItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdObjectItem") = value
        End Set
    End Property


    Private Property Disabled As Boolean Implements IViewAggregationSelector.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
            DDLmethods.Enabled = Not value
            DDLratingSet.Enabled = Not value
            For Each row As RepeaterItem In RPTweights.Items
                DirectCast(row.FindControl("TXBweight"), TextBox).Enabled = Not value
            Next
        End Set
    End Property
    Private Property UseManualWeights As Boolean Implements IViewAggregationSelector.UseManualWeights
        Get
            Return ViewStateOrDefault("UseManualWeights", False)
        End Get
        Set(value As Boolean)
            ViewState("UseManualWeights") = value
        End Set
    End Property
    Private Property UseOrderedWeights As Boolean Implements IViewAggregationSelector.UseOrderedWeights
        Get
            Return ViewStateOrDefault("UseOrderedWeights", False)
        End Get
        Set(value As Boolean)
            ViewState("UseOrderedWeights") = value
        End Set
    End Property
    Private Property UseFuzzyWeights As Boolean Implements IViewAggregationSelector.UseFuzzyWeights
        Get
            Return ViewStateOrDefault("UseFuzzyWeights", False)
        End Get
        Set(value As Boolean)
            ViewState("UseFuzzyWeights") = value
        End Set
    End Property
    Public ReadOnly Property isValid As Boolean Implements IViewAggregationSelector.isValid
        Get
            'Dim isMandatory As Boolean = Me.Mandatory
            'Select Case CriterionType
            '    'Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
            '    '    Dim selected As Integer = (From i As ListItem In Me.CBLitems.Items Where i.Selected).Count
            '    '    Return (selected >= MinOptions AndAlso selected <= MaxOptions)
            '    Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.DecimalRange
            '        Dim v As Decimal
            '        Decimal.TryParse(Me.TXBdecimalrange.Text, v)
            '        Return (Me.REVdecimal.MinimumValue >= v AndAlso Me.REVdecimal.MaximumValue <= v) AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))
            '    Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.StringRange
            '        Return Me.DDLstringRange.SelectedIndex <> -1 AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))
            '        'Dim selected As Integer = (From i As ListItem In Me.RBLitems.Items Where i.Selected).Count
            '        'Return (selected >= MinOptions AndAlso selected <= MaxOptions) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Me.RBLitems.SelectedIndex <> -1))

            '    Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.Textual
            '        Return Not String.IsNullOrEmpty(Me.TXBtextual.Text) AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))
            '    Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType.IntegerRange
            '        Return Me.DDLintegerRange.SelectedIndex <> -1 AndAlso (Not isMandatory OrElse Not String.IsNullOrEmpty(Me.TXBcomment.Text))

            'End Select
            Return True
        End Get
    End Property


    Private Property IdFuzzyMethods As List(Of Long) Implements IViewAggregationSelector.IdFuzzyMethods
        Get
            Return ViewStateOrDefault("IdFuzzyMethods", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("IdFuzzyMethods") = value
        End Set
    End Property
    Private Property IdManualMethods As List(Of Long) Implements IViewAggregationSelector.IdManualMethods
        Get
            Return ViewStateOrDefault("IdManualMethods", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("IdManualMethods") = value
        End Set
    End Property
    Private Property IdOrderedMethods As List(Of Long) Implements IViewAggregationSelector.IdOrderedMethods
        Get
            Return ViewStateOrDefault("IdOrderedMethods", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("IdOrderedMethods") = value
        End Set
    End Property
    Public Property CurrentError As DssError Implements IViewAggregationSelector.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", DssError.None)
        End Get
        Set(value As DssError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Private Property CurrentNormalization As NormalizationStatus Implements IViewAggregationSelector.CurrentNormalization
        Get
            Return ViewStateOrDefault("CurrentNormalization", NormalizationStatus.none)
        End Get
        Set(value As NormalizationStatus)
            ViewState("CurrentNormalization") = value
        End Set
    End Property

    Public WriteOnly Property TranslationMethodTitle As String Implements IViewAggregationSelector.TranslationMethodTitle
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then
                Resource.setLabel(LBdssMethods_t)
            Else
                LBdssMethods_t.Text = value
            End If
        End Set
    End Property

    Public WriteOnly Property TranslationRatingSetTitle As String Implements IViewAggregationSelector.TranslationRatingSetTitle
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then
                Resource.setLabel(LBdssRatingSets_t)
            Else
                LBdssRatingSets_t.Text = value
            End If
        End Set
    End Property
    Public WriteOnly Property TranslationWeightsSetTitle As String Implements IViewAggregationSelector.TranslationWeightsSetTitle
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then
                Resource.setLabel(LBmanualWeightsTitle)
            Else
                LBmanualWeightsTitle.Text = value
            End If
        End Set
    End Property
    Public Property TranslationSelectMethod As String Implements IViewAggregationSelector.TranslationSelectMethod
        Get
            Dim translation As String = ViewState("SelectMethodTranslation")
            If String.IsNullOrWhiteSpace(translation) Then
                translation = Resource.getValue("SelectMethodTranslation")
                ViewState("SelectMethodTranslation") = translation
            End If
            Return translation
        End Get
        Set(value As String)
            ViewState("TranslationSelectMethod") = value
        End Set
    End Property
    Public Property TranslationSelectRating As String Implements IViewAggregationSelector.TranslationSelectRating
        Get
            Dim translation As String = ViewState("SelectRatingTranslation")
            If String.IsNullOrWhiteSpace(translation) Then
                translation = Resource.getValue("SelectRatingTranslation")
                ViewState("SelectRatingTranslation") = translation
            End If
            Return translation
        End Get
        Set(value As String)
            ViewState("TranslationSelectRating") = value
        End Set
    End Property
    Public Property TranslationInherits As String Implements IViewAggregationSelector.TranslationInherits
        Get
            Dim translation As String = ViewState("TranslationInherits")
            If String.IsNullOrWhiteSpace(translation) Then
                translation = Resource.getValue("TranslationInherits")
                ViewState("TranslationInherits") = translation
            End If
            Return translation
        End Get
        Set(value As String)
            ViewState("TranslationInherits") = value
        End Set
    End Property
    Private Property AvailableWeightItems As List(Of dtoItemWeightBase) Implements IViewAggregationSelector.AvailableWeightItems
        Get
            Return ViewStateOrDefault("AvailableWeightItems", New List(Of dtoItemWeightBase))
        End Get
        Set(value As List(Of dtoItemWeightBase))
            ViewState("AvailableWeightItems") = value
        End Set
    End Property
    Private ReadOnly Property GetMaxWeightName() As String Implements IViewAggregationSelector.GetMaxWeightName
        Get
            Return Resource.getValue("GetMaxWeightName")
        End Get
    End Property
    Private ReadOnly Property GetMinWeightName() As String Implements IViewAggregationSelector.GetMinWeightName
        Get
            Return Resource.getValue("GetMinWeightName")
        End Get
    End Property
    Private ReadOnly Property GetIntermediateWeightName() As String Implements IViewAggregationSelector.GetIntermediateWeightName
        Get
            Return Resource.getValue("GetIntermediateWeightName")
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event SelectedMethod(ByVal value As dtoItemMethodSettings, obj As UC_DssAggregationSelector)
    Public Event RequireWeights(idObject As Long, isFuzzyWeights As Boolean, orderedWeights As Boolean, ByRef items As List(Of dtoItemWeightBase), obj As UC_DssAggregationSelector)
    Private _CssClass
    Public Property CssClass As String
        Get
            If Not String.IsNullOrWhiteSpace(_CssClass) Then
                Return " " & _CssClass
            Else
                Return _CssClass
            End If
        End Get
        Set(value As String)
            _CssClass = value
        End Set
    End Property
    Public WriteOnly Property Description As String
        Set(value As String)
            DVdescription.Visible = Not String.IsNullOrWhiteSpace(value)
            LTdescription.Text = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            If String.IsNullOrWhiteSpace(HYPhelp.NavigateUrl) Then
                HYPhelp.NavigateUrl = PageUtility.ApplicationUrlBase() & lm.Comol.Core.Dss.Domain.RootObject.HelpMethods
            End If
            If String.IsNullOrWhiteSpace(LBdssMethods_t.Text) Then
                .setLabel(LBdssMethods_t)
            End If
            If String.IsNullOrWhiteSpace(LBdssRatingSets_t.Text) Then
                .setLabel(LBdssRatingSets_t)
            End If
            If String.IsNullOrWhiteSpace(LBmanualWeightsTitle.Text) Then
                .setLabel(LBmanualWeightsTitle)
            End If
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idFatherMethod As Long, methods As List(Of dtoSelectMethod), settings As dtoItemMethodSettings, allowInherits As Boolean, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewAggregationSelector.InitializeControl
        DDLratingSet.AutoPostBack = RaiseEventForRatingSetSelect
        IsDefaultForChildren = settings.IsDefaultForChildren
        CurrentError = err
        SetInternazionalizzazione()
        CurrentPresenter.InitView(idFatherMethod, methods, settings, allowInherits, disabled)
    End Sub
    Public Sub InitializeControl(idFatherMethod As Long, methods As List(Of dtoSelectMethod), settings As dtoItemMethodSettings, weightItems As List(Of dtoItemWeightBase), allowInheritsFromFather As Boolean, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewAggregationSelector.InitializeControl
        DDLratingSet.AutoPostBack = RaiseEventForRatingSetSelect
        IsDefaultForChildren = settings.IsDefaultForChildren
        CurrentError = err
        SetInternazionalizzazione()
        CurrentPresenter.InitView(idFatherMethod, methods, settings, allowInheritsFromFather, disabled, weightItems)
    End Sub
    Public Sub InitializeControl(idFatherMethod As Long, methods As List(Of dtoSelectMethod), idMethod As Long, idRatingSet As Long, inheritsFromFather As Boolean, isDefaultForChildren As Boolean, allowInherits As Boolean, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewAggregationSelector.InitializeControl
        InitializeControl(idFatherMethod, methods, New dtoItemMethodSettings() With {.IdMethod = idMethod, .IdRatingSet = idRatingSet, .InheritsFromFather = inheritsFromFather, .IsDefaultForChildren = isDefaultForChildren}, allowInherits, disabled, err)
    End Sub
    Public Sub InitializeControl(idFatherMethod As Long, methods As List(Of dtoSelectMethod), idMethod As Long, idRatingSet As Long, weightItems As List(Of dtoItemWeightBase), inheritsFromFather As Boolean, isDefaultForChildren As Boolean, allowInheritsFromFather As Boolean, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewAggregationSelector.InitializeControl
        InitializeControl(idFatherMethod, methods, New dtoItemMethodSettings() With {.IdMethod = idMethod, .IdRatingSet = idRatingSet, .InheritsFromFather = inheritsFromFather, .IsDefaultForChildren = isDefaultForChildren}, weightItems, allowInheritsFromFather, disabled, err)
    End Sub
    Private Sub LoadMethods(methods As List(Of lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod), idSelected As Long, inheritsFromCall As Boolean) Implements IViewAggregationSelector.LoadMethods
        IdFuzzyMethods = methods.Where(Function(m) m.IsFuzzy).Select(Function(m) m.Id).ToList()
        DDLmethods.DataSource = methods
        DDLmethods.DataTextField = "Name"
        DDLmethods.DataValueField = "Id"
        DDLmethods.DataBind()
        If Not IsDefaultForChildren Then
            DVratingSet.Visible = False
            If AllowInheritsFromFather Then
                DDLmethods.Items.Insert(0, New ListItem() With {.Value = "-1", .Text = TranslationInherits})
            Else
                DDLmethods.Items.Insert(0, New ListItem() With {.Value = "-1", .Text = TranslationSelectMethod})
            End If
        End If
        If methods.Any(Function(m) m.Id = idSelected) Then
            DDLmethods.SelectedValue = idSelected.ToString
        Else
            DDLmethods.SelectedIndex = 0
        End If
    End Sub
    Private Sub LoadNoMethods() Implements IViewAggregationSelector.LoadNoMethods
        SPNhelp.Visible = False
        If IsDefaultForChildren Then
            DDLmethods.Items.Insert(0, New ListItem() With {.Value = "0", .Text = Resource.getValue("NoMethodsFound")})
        ElseIf AllowInheritsFromFather Then
            DDLmethods.Items.Insert(0, New ListItem() With {.Value = "-1", .Text = TranslationInherits})
        Else
            DDLmethods.Items.Insert(0, New ListItem() With {.Value = "0", .Text = Resource.getValue("NoMethodsFound")})
        End If
        DDLmethods.Enabled = False
        DDLratingSet.Enabled = False
    End Sub

    Private Sub LoadRatingSets(items As List(Of dtoSelectRatingSet), idSelected As Long) Implements IViewAggregationSelector.LoadRatingSets
        DVmanualRating.Visible = False
        DVratingSet.Visible = True
        DDLratingSet.Items.Clear()
        DDLratingSet.Enabled = Not IsNothing(items) AndAlso items.Any() AndAlso Not Disabled
        If Not IsNothing(items) Then
            If items.Any Then
                DDLratingSet.DataSource = items
                DDLratingSet.DataTextField = "Name"
                DDLratingSet.DataValueField = "Id"
                DDLratingSet.DataBind()
                If items.Any(Function(m) m.Id = idSelected) Then
                    DDLratingSet.SelectedValue = idSelected.ToString
                Else
                    DDLratingSet.SelectedIndex = 0
                End If
            Else
                DDLratingSet.Items.Insert(0, New ListItem() With {.Value = "0", .Text = Resource.getValue("NoRatingSetsFound")})
            End If
        End If
    End Sub
    Private Sub DisplayEmptyRatingSet() Implements IViewAggregationSelector.DisplayEmptyRatingSet
        DVratingSet.Visible = True
        DDLratingSet.Enabled = False
        DDLratingSet.Items.Clear()
        DDLratingSet.Items.Insert(0, New ListItem() With {.Value = "0", .Text = Resource.getValue("NoRatingSetsFound")})
    End Sub
    Private Sub HideRatingSetSelectionForInheritance() Implements IViewAggregationSelector.HideRatingSetSelectionForInheritance
        DVratingSet.Visible = False
    End Sub

    Private Function GetManualWeights() As List(Of dtoItemWeight) Implements IViewAggregationSelector.GetManualWeights
        Dim results As New List(Of dtoItemWeight)
        Dim bItems As List(Of dtoItemWeightBase) = AvailableWeightItems
        For Each row As RepeaterItem In RPTweights.Items
            Dim item As dtoItemWeight
            Dim oLiteral As Literal = row.FindControl("LTidItem")
            Dim idObject As Long = 0
            Long.TryParse(oLiteral.Text, idObject)
            If bItems.Any(Function(i) i.IdObject = idObject) Then
                item = New dtoItemWeight(bItems.Where(Function(i) i.IdObject = idObject).FirstOrDefault())
            Else
                item = New dtoItemWeight()
                item.IdObject = idObject
                item.Name = DirectCast(row.FindControl("LBweight"), Label).Text
            End If
            item.Value = DirectCast(row.FindControl("TXBweight"), TextBox).Text
            results.Add(item)
        Next
        Return results
    End Function

    Private Sub HideManualWeights() Implements IViewAggregationSelector.HideManualWeights
        DVmanualRating.Visible = False
    End Sub

    Private Sub LoadManualWeights(weights As List(Of dtoItemWeight), areOrdered As Boolean, areFuzzy As Boolean) Implements IViewAggregationSelector.LoadManualWeights
        DVmanualRating.Visible = (Not IsNothing(weights) AndAlso weights.Count > 0)
        DVratingSet.Visible = False
        LBmanualWeightsInfo.Text = Resource.getValue("LBmanualWeightsInfo.areFuzzy." & areFuzzy.ToString)
        If Not IsNothing(weights) Then
            RPTweights.DataSource = weights
            RPTweights.DataBind()
        Else
            RPTweights.Visible = False
            RPTweights.DataSource = New List(Of dtoItemWeight)
            RPTweights.DataBind()
        End If
    End Sub
    Private Sub UpdateWeights(weights As List(Of dtoItemWeight)) Implements IViewAggregationSelector.UpdateWeights
        Dim index As Integer = 0
        For Each row As RepeaterItem In RPTweights.Items
            DirectCast(row.FindControl("TXBweight"), TextBox).Text = weights(index).Value
            index += 1
        Next
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewAggregationSelector.DisplaySessionTimeout
        Disabled = True
    End Sub

    Public Function GetSettings() As dtoItemMethodSettings Implements IViewAggregationSelector.GetSettings
        Dim dto As New dtoItemMethodSettings()
        dto.IsFuzzyMethod = UseFuzzyWeights
        If DDLmethods.SelectedIndex > -1 Then
            Dim idMethod As Long = 0
            Long.TryParse(DDLmethods.SelectedValue, idMethod)
            dto.IdMethod = idMethod
            dto.IsDefaultForChildren = IsDefaultForChildren
            dto.InheritsFromFather = Not IsDefaultForChildren AndAlso idMethod = -1
            If IsDefaultForChildren AndAlso idMethod < 1 Then
                dto.Error = DssError.MissingMethod
            ElseIf Not IsDefaultForChildren AndAlso idMethod = -1 Then
                dto.InheritsFromFather = True
            ElseIf idMethod < 1 Then
                dto.Error = DssError.MissingMethod
            End If
        Else
            dto.Error = DssError.MissingMethod
        End If
        dto.UseManualWeights = UseManualWeights
        dto.UseOrderedWeights = UseOrderedWeights
        dto.FuzzyMeWeights = ""
        If (dto.UseManualWeights) Then
            Dim items As List(Of dtoItemWeight) = GetManualWeights()
            If IsNothing(items) OrElse Not items.Any() OrElse items.Any(Function(i) String.IsNullOrWhiteSpace(i.Value)) Then
                dto.Error = dto.Error Or DssError.MissingManualWeight
                If items.Any() Then
                    dto.FuzzyMeWeights = String.Join("#", items.Select(Function(i) i.IdObject.ToString() & ":" & i.Value & If(String.IsNullOrWhiteSpace(i.Value) OrElse i.Value.EndsWith(";"), "", ";")).ToList())
                End If
            ElseIf Not IsNothing(items) AndAlso (items.Any()) Then
                If Not CurrentPresenter.CheckWeightsNormalization(items, dto.IsFuzzyMethod, True) Then
                    Select Case CurrentNormalization
                        Case NormalizationStatus.normalizable
                            items = GetManualWeights()
                        Case Else
                            dto.Error = dto.Error Or DssError.InvalidManualWeight
                    End Select
                End If
                If dto.IsFuzzyMethod Then
                    dto.FuzzyMeWeights = String.Join("#", items.Select(Function(i) i.IdObject.ToString() & ":" & i.Value & If(i.Value.EndsWith(";"), "", ";")).ToList())
                Else
                    dto.FuzzyMeWeights = String.Join("#", items.Select(Function(i) i.IdObject.ToString() & ":" & i.Value).ToList())
                End If
            Else
                dto.Error = dto.Error Or DssError.InvalidManualWeight
            End If
        ElseIf DDLratingSet.SelectedIndex > -1 Then
            Dim idRatingSet As Long = 0
            If Not dto.InheritsFromFather Then
                Long.TryParse(DDLratingSet.SelectedValue, idRatingSet)
            End If
            dto.IdRatingSet = idRatingSet
            If idRatingSet < 1 AndAlso Not dto.InheritsFromFather Then
                dto.Error = dto.Error Or DssError.MissingRatingSet
            End If
        Else
            dto.Error = dto.Error Or DssError.MissingRatingSet
        End If
        Return dto
    End Function

    Private Sub UpdateCurrentMethod() Implements IViewAggregationSelector.UpdateCurrentMethod
        If RaiseEventForMethodSelect Then
            RaiseEvent SelectedMethod(GetSettings, Me)
        End If
    End Sub
    Private Sub DisplayNormalizationMessage(normalization As NormalizationStatus) Implements IViewAggregationSelector.DisplayNormalizationMessage
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Select Case normalization
            Case NormalizationStatus.impossible, NormalizationStatus.none
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case NormalizationStatus.normalized
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End Select
        CTRLweightsMessage.Visible = True
        CTRLweightsMessage.InitializeControl(Resource.getValue("DisplayNormalizationMessage.NormalizationStatus." & normalization.ToString), mType)
    End Sub
    Private Sub HideNormalizationMessage() Implements IViewAggregationSelector.HideNormalizationMessage
        CTRLweightsMessage.Visible = False
    End Sub
    Private Function RequireNewWeights(newAreFuzzyWeights As Boolean, orderedWeights As Boolean) As List(Of dtoItemWeightBase) Implements IViewAggregationSelector.RequireNewWeights
        Dim results As New List(Of dtoItemWeightBase)
        RaiseEvent RequireWeights(IdObjectItem, newAreFuzzyWeights, orderedWeights, results, Me)
        Return results
    End Function
#End Region

    Private Sub DDLmethods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLmethods.SelectedIndexChanged
        Dim idMethod As Long = 0
        If DDLmethods.SelectedIndex <> -1 Then
            Long.TryParse(DDLmethods.SelectedValue, idMethod)
        End If

        Dim idRatingSet As Long = 0
        If DDLratingSet.SelectedIndex <> -1 Then
            Long.TryParse(DDLratingSet.SelectedValue, idRatingSet)
        End If
        CurrentPresenter.SelectMethod(IdFatherMethod, idMethod, idRatingSet, UseFuzzyWeights, UseManualWeights, UseOrderedWeights)
    End Sub
    Private Sub DDLratingSet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLratingSet.SelectedIndexChanged
        RaiseEvent SelectedMethod(GetSettings, Me)
    End Sub
    Public Function CssItemClass(item As dtoItemWeight) As String
        Dim cssClass As String = ""
        If item.IsFirst Then
            cssClass &= " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString()
        End If
        If item.IsLast Then
            cssClass &= " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End If
        Return cssClass
    End Function

    Private Sub RPTweights_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTweights.ItemDataBound
        DirectCast(e.Item.FindControl("TXBweight"), TextBox).Enabled = Not Disabled
    End Sub
End Class