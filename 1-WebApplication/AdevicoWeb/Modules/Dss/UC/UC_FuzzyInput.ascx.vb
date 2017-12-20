Imports lm.Comol.Core.Dss.Domain.Templates
Imports lm.Comol.Core.Dss.Presentation.Evaluation
Imports lm.Comol.Core.Dss.Domain
Public Class UC_FuzzyInput
    Inherits DSSbaseControl
    Implements IViewFuzzyInput

#Region "Context"
    Private _Presenter As FuzzyInputPresenter
    Private ReadOnly Property CurrentPresenter() As FuzzyInputPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New FuzzyInputPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IsFuzzy As Boolean Implements IViewFuzzyInput.IsFuzzy
        Get
            Return ViewStateOrDefault("IsFuzzy", False)
        End Get
        Set(value As Boolean)
            ViewState("IsFuzzy") = value
        End Set
    End Property
    Public Property RaiseEvents As Boolean Implements IViewFuzzyInput.RaiseEvents
        Get
            Return ViewStateOrDefault("RaiseEvents", True)
        End Get
        Set(value As Boolean)
            ViewState("RaiseEvents") = value
        End Set
    End Property
    Public Property IdObjectItem As Long Implements IViewFuzzyInput.IdObjectItem
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
    Private Property IdMethod As Long Implements IViewFuzzyInput.IdMethod
        Get
            Return ViewStateOrDefault("IdMethod", 0)
        End Get
        Set(value As Long)
            ViewState("IdMethod") = value
        End Set
    End Property
    Public Property IdRatingSet As Long Implements IViewFuzzyInput.IdRatingSet
        Get
            Return ViewStateOrDefault("IdRatingSet", 0)
        End Get
        Set(value As Long)
            ViewState("IdRatingSet") = value
        End Set
    End Property
    Private Property Disabled As Boolean Implements IViewFuzzyInput.Disabled
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
    Public ReadOnly Property isValid As Boolean Implements IViewFuzzyInput.isValid
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
    Private Property CurrentError As DssError Implements IViewFuzzyInput.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", DssError.None)
        End Get
        Set(value As DssError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Private Property CurrentRatingType As RatingType Implements IViewFuzzyInput.CurrentRatingType
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
    Public Sub InitializeControl(method As dtoSelectMethod, ratingSet As dtoSelectRatingSet, objectSettings As dtoItemWeightSettings, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInput.InitializeControl
        BaseInitializer(err)
        CurrentPresenter.InitView(method, ratingSet, objectSettings, disabled)
    End Sub
    Public Sub InitializeControl(method As dtoSelectMethod, objectSettings As dtoItemWeightSettings, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInput.InitializeControl
        BaseInitializer(err)
        CurrentPresenter.InitView(method, objectSettings, disabled)
    End Sub
    Public Sub InitializeControl(method As dtoSelectMethod, idRatingSet As Long, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInput.InitializeControl
        BaseInitializer(err)
        CurrentPresenter.InitView(method, idRatingSet, disabled)
    End Sub
    Public Sub InitializeControl(idMethod As Long, ratingSet As dtoSelectRatingSet, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInput.InitializeControl
        BaseInitializer(err)
        CurrentPresenter.InitView(idMethod, ratingSet, disabled)
    End Sub
    Public Sub InitializeControl(idMethod As Long, idRatingSet As Long, Optional disabled As Boolean = False, Optional err As DssError = lm.Comol.Core.Dss.Domain.DssError.None) Implements IViewFuzzyInput.InitializeControl
        BaseInitializer(err)
        CurrentPresenter.InitView(idMethod, idRatingSet, disabled)
    End Sub

    Private Sub InitializeRating(isCurrent As Boolean, type As RatingType, ratingSet As dtoSelectRatingSet, idRatingValue As Long, Optional idRatingValueEnd As Long = -1) Implements IViewFuzzyInput.InitializeRating
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
    Public Sub InitializeDisabledControl() Implements IViewFuzzyInput.InitializeDisabledControl
        BaseInitializer(CurrentError)
        Disabled = True
        IdRatingSet = 0
    End Sub
    Private Sub LoadAvailableRatings(availableTypes As List(Of dtoRatingType), currentType As RatingType) Implements IViewFuzzyInput.LoadAvailableRatings
        CurrentRatingType = currentType
        RPTratingTabs.DataSource = availableTypes
        RPTratingTabs.DataBind()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewFuzzyInput.DisplaySessionTimeout
        SPNedit.Attributes("Class") = SPNedit.Attributes("Class").Replace("editable ", "")
        SPNfuzzyEdit.Visible = False
    End Sub
    Private Function GetTranslations() As Dictionary(Of RatingType, String) Implements IViewFuzzyInput.GetTranslations
        Return (From e As RatingType In [Enum].GetValues(GetType(RatingType)) Where e <> RatingType.none Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("Tab.RatingType." & e.ToString))
    End Function
    Public Function GetSettings() As dtoItemWeightSettings Implements IViewFuzzyInput.GetSettings
        Dim item As New dtoItemWeightSettings()
        item.IsFuzzyWeight = IsFuzzy
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
#End Region

#Region "Internal"
    Private Sub BaseInitializer(err As DssError)
        HDNfuzzyValue.Value = ""
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