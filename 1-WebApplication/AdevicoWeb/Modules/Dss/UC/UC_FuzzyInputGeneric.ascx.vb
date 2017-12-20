
Imports lm.Comol.Core.Dss.Domain.Templates
Imports lm.Comol.Core.Dss.Presentation.Evaluation
Imports lm.Comol.Core.Dss.Domain
Public Class UC_FuzzyInputGeneric
    Inherits DSSbaseControl
    Implements IViewFuzzyInputGeneric

#Region "Context"
    Private _Presenter As FuzzyInputGenericPresenter
    Private ReadOnly Property CurrentPresenter() As FuzzyInputGenericPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New FuzzyInputGenericPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IsFuzzy As Boolean Implements IViewFuzzyInputGeneric.IsFuzzy
        Get
            Return ViewStateOrDefault("IsFuzzy", False)
        End Get
        Set(value As Boolean)
            ViewState("IsFuzzy") = value
        End Set
    End Property
    Public Property RaiseEvents As Boolean Implements IViewFuzzyInputGeneric.RaiseEvents
        Get
            Return ViewStateOrDefault("RaiseEvents", True)
        End Get
        Set(value As Boolean)
            ViewState("RaiseEvents") = value
        End Set
    End Property
    Public Property IdObjectItem As Long Implements IViewFuzzyInputGeneric.IdObjectItem
        Get
            Return ViewStateOrDefault("IdObjectItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdObjectItem") = value
            CTRLextended.IdObjectItem = value
            CTRLintermediate.IdObjectItem = value
            CTRLsimple.IdObjectItem = value
        End Set
    End Property
    Private Property IdMethod As Long Implements IViewFuzzyInputGeneric.IdMethod
        Get
            Return ViewStateOrDefault("IdMethod", 0)
        End Get
        Set(value As Long)
            ViewState("IdMethod") = value
        End Set
    End Property
    Public Property IdRatingSet As Long Implements IViewFuzzyInputGeneric.IdRatingSet
        Get
            Return ViewStateOrDefault("IdRatingSet", 0)
        End Get
        Set(value As Long)
            ViewState("IdRatingSet") = value
        End Set
    End Property
    Private Property Disabled As Boolean Implements IViewFuzzyInputGeneric.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
            If value Then
                SPNedit.Attributes("Class") = SPNedit.Attributes("Class").Replace("editable ", "")
            Else
                If Not SPNedit.Attributes("Class").Contains("editable ") Then
                    SPNedit.Attributes("Class") = "editable " & SPNedit.Attributes("Class")
                End If
            End If
            SPNfuzzyEdit.Visible = Not value
            CTRLextended.Disabled = value
            CTRLintermediate.Disabled = value
            CTRLsimple.Disabled = value
        End Set
    End Property
    Public ReadOnly Property isValid As Boolean Implements IViewFuzzyInputGeneric.isValid
        Get
            Select Case CurrentRatingType
                Case RatingType.extended
                    Return CTRLextended.IsValid
                Case RatingType.intermediateValues
                    Return CTRLintermediate.IsValid
                Case RatingType.simple
                    Return CTRLsimple.IsValid
                Case Else
                    Return False
            End Select
        End Get
    End Property
    Private Property CurrentError As DssError Implements IViewFuzzyInputGeneric.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", DssError.None)
        End Get
        Set(value As DssError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Private Property CurrentRatingType As RatingType Implements IViewFuzzyInputGeneric.CurrentRatingType
        Get
            If String.IsNullOrWhiteSpace(HDNfuzzyType.Value) Then
                Return RatingType.none
            Else
                Dim result As RatingType = RatingType.none
                Try
                    result = CInt(HDNfuzzyType.Value.Replace("#tab-", ""))
                Catch ex As Exception

                End Try
                Return result
            End If
        End Get
        Set(value As RatingType)
            HDNfuzzyType.Value = "#tab-" & CInt(value)
        End Set
    End Property
#End Region

#Region "Internal"
    Public Property TranslationWeightTitle As String
        Get
            Return ViewStateOrDefault("TranslationWeightTitle", Resource.getValue("LTweightTitle.text"))
        End Get
        Set(value As String)
            ViewState("WeightTitleTranslation") = value
            If Not String.IsNullOrWhiteSpace(value) Then
                LTweightTitle.Text = value
            End If
        End Set
    End Property
    Public Property TranslationWeightChoose As String
        Get
            Return ViewStateOrDefault("TranslationWeightChoose", Resource.getValue("LBweightChoose.text"))
        End Get
        Set(value As String)
            ViewState("TranslationWeightChoose") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            If String.IsNullOrWhiteSpace(LTweightTitle.Text) Then
                .setLiteral(LTweightTitle)
            End If
            If String.IsNullOrWhiteSpace(LBweightChoose.Text) Then
                .setLabel(LBweightChoose)
            End If
            SPNfuzzyEdit.Attributes("title") = .getValue("SPNfuzzyEdit.title")
            .setLabel(LBsaveRating)
            .setLabel(LBcancelRating)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idMethod As Long, ratingSet As dtoGenericRatingSet, objectSettings As dtoItemWeightSettings, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInputGeneric.InitializeControl
        HDNfuzzyValue.Value = ""
        BaseInitializer(err)
        CurrentPresenter.InitView(idMethod, ratingSet, objectSettings, disabled)
    End Sub
    Public Sub InitializeControl(idMethod As Long, ratingSet As dtoGenericRatingSet, rating As dtoItemRating, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInputGeneric.InitializeControl
        HDNfuzzyValue.Value = ""
        BaseInitializer(err)
        CurrentPresenter.InitView(idMethod, ratingSet, rating, disabled)
    End Sub
    Public Sub InitializeControl(idMethod As Long, ratingSet As dtoGenericRatingSet, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInputGeneric.InitializeControl
        HDNfuzzyValue.Value = ""
        BaseInitializer(err)
        CurrentPresenter.InitView(idMethod, ratingSet, disabled)
    End Sub


    Private Sub InitializeRating(isCurrent As Boolean, type As RatingType, ratingSet As dtoGenericRatingSet, idRatingValue As Long, Optional idRatingValueEnd As Long = -1) Implements IViewFuzzyInputGeneric.InitializeRating

        Select Case type
            Case RatingType.simple
                CTRLsimple.Visible = True
                CTRLsimple.InitializeControl(ratingSet, idRatingValue)
                If isCurrent Then
                    HDNfuzzyValue.Value = CInt(type).ToString & ":" & CTRLsimple.IdSelected.ToString() & ";"
                    If CTRLsimple.IsValid Then
                        LBweightChoose.Text = CTRLsimple.GetValueToString()
                    End If
                End If
            Case RatingType.extended
                CTRLextended.Visible = True
                CTRLextended.InitializeControl(ratingSet, idRatingValue, idRatingValueEnd)
                If isCurrent Then
                    HDNfuzzyValue.Value = CInt(type).ToString & ":" & CTRLextended.IdSelectedFrom.ToString() & ";" & CTRLextended.IdSelectedTo.ToString() & ";"
                    If CTRLextended.IsValid Then
                        LBweightChoose.Text = CTRLextended.GetValueToString()
                    End If
                End If
            Case RatingType.intermediateValues
                CTRLintermediate.Visible = True
                CTRLintermediate.InitializeControl(ratingSet, idRatingValue, idRatingValueEnd)
                If isCurrent Then
                    HDNfuzzyValue.Value = CInt(type).ToString & ":" & CTRLintermediate.IdSelectedFrom.ToString() & ";" & CTRLintermediate.IdSelectedTo.ToString() & ";"
                    If CTRLintermediate.IsValid Then
                        LBweightChoose.Text = CTRLintermediate.GetValueToString()
                    End If
                End If
        End Select
    End Sub
    Public Sub InitializeDisabledControl() Implements IViewFuzzyInputGeneric.InitializeDisabledControl
        BaseInitializer(CurrentError)
        Disabled = True
        IdRatingSet = 0
    End Sub
    Private Sub LoadAvailableRatings(availableTypes As List(Of dtoRatingType), currentType As RatingType) Implements IViewFuzzyInputGeneric.LoadAvailableRatings
        CurrentRatingType = currentType
        RPTratingTabs.DataSource = availableTypes
        RPTratingTabs.DataBind()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewFuzzyInputGeneric.DisplaySessionTimeout
        SPNedit.Attributes("Class") = SPNedit.Attributes("Class").Replace("editable ", "")
        SPNfuzzyEdit.Visible = False
    End Sub
    Private Function GetTranslations() As Dictionary(Of RatingType, String) Implements IViewFuzzyInputGeneric.GetTranslations
        Return (From e As RatingType In [Enum].GetValues(GetType(RatingType)) Where e <> RatingType.none Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("Tab.RatingType." & e.ToString))
    End Function
    Public Function GetSettings() As dtoItemWeightSettings Implements IViewFuzzyInputGeneric.GetSettings
        Dim item As New dtoItemWeightSettings()
        item.IsFuzzyWeight = IsFuzzy
        item.RatingType = CurrentRatingType
        If IdRatingSet = 0 Then
            item.Error = DssError.MissingRatingSet
        Else
            Select Case item.RatingType
                Case RatingType.extended
                    item.IdRatingValue = CTRLextended.IdSelectedFrom
                    item.IdRatingValueEnd = CTRLextended.IdSelectedTo
                    If CTRLextended.IsValid Then
                        item.Error = DssError.None
                    Else
                        item.Error = DssError.MissingWeight
                    End If
                Case RatingType.intermediateValues
                    item.IdRatingValue = CTRLintermediate.IdSelectedFrom
                    item.IdRatingValueEnd = CTRLintermediate.IdSelectedTo
                    If CTRLintermediate.IsValid Then
                        item.Error = DssError.None
                    Else
                        item.Error = DssError.MissingWeight
                    End If
                Case RatingType.simple
                    item.IdRatingValue = CTRLsimple.IdSelected
                    If CTRLsimple.IsValid Then
                        item.Error = DssError.None
                    Else
                        item.Error = DssError.MissingWeight
                    End If
                    'If IsFuzzy Then
                    'Else
                    '    item.Weight = 
                    'End If
                Case Else
                    item.Error = DssError.MissingWeight
            End Select
        End If
        Return CurrentPresenter.VerifySettings(item)
    End Function
    Public Function GetItemRating() As dtoItemRating Implements IViewFuzzyInputGeneric.GetItemRating
        Dim item As New dtoItemRating()
        item.IsFuzzy = IsFuzzy
        item.RatingType = CurrentRatingType
        If IdRatingSet = 0 Then
            item.Error = DssError.MissingRatingSet
        ElseIf String.IsNullOrWhiteSpace(HDNfuzzyValue.Value) Then
            item.Error = DssError.MissingWeight
        Else
            Select Case item.RatingType
                Case RatingType.extended
                    item.IdRatingValue = CTRLextended.IdSelectedFrom
                    item.IdRatingValueEnd = CTRLextended.IdSelectedTo
                    If CTRLextended.IsValid Then
                        item.Error = DssError.None
                    Else
                        item.Error = DssError.MissingWeight
                    End If
                Case RatingType.intermediateValues
                    item.IdRatingValue = CTRLintermediate.IdSelectedFrom
                    item.IdRatingValueEnd = CTRLintermediate.IdSelectedTo
                    If CTRLintermediate.IsValid Then
                        item.Error = DssError.None
                    Else
                        item.Error = DssError.MissingWeight
                    End If
                Case RatingType.simple
                    item.IdRatingValue = CTRLsimple.IdSelected
                    If CTRLsimple.IsValid Then
                        item.Error = DssError.None
                    Else
                        item.Error = DssError.MissingWeight
                    End If
                Case Else
                    item.Error = DssError.MissingWeight
            End Select
        End If
        Return CurrentPresenter.VerifySettings(item)
    End Function
#End Region

#Region "Internal"
    Private Sub BaseInitializer(err As DssError)
        LBweightChoose.Text = TranslationWeightChoose
        LBweightChoose.ToolTip = Resource.getValue("LBweightChoose.ToolTip")
        SetInternazionalizzazione()
        CurrentError = err
        CTRLsimple.Visible = False
        CTRLextended.Visible = False
        CTRLintermediate.Visible = False
    End Sub
#End Region


End Class