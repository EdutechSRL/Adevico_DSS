Imports lm.Comol.Core.Dss.Domain.Templates
Imports lm.Comol.Core.Dss.Presentation.Evaluation
Imports lm.Comol.Core.Dss.Domain
Public Class UC_FuzzyNumber
    Inherits DSSbaseControl
    Implements IViewFuzzyNumber

#Region "Context"
    Private _Presenter As FuzzyNumberPresenter
    Private ReadOnly Property CurrentPresenter() As FuzzyNumberPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New FuzzyNumberPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Internal"
    Private _ShowFazzyValueWithCog As Boolean
    Public Property ShowFazzyValueWithCog As Boolean
        Get
            Return _ShowFazzyValueWithCog
        End Get
        Set(value As Boolean)
            _ShowFazzyValueWithCog = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(isFuzzy As Boolean, value As Double) Implements IViewFuzzyNumber.InitializeControl
        CurrentPresenter.InitView(isFuzzy, value)
    End Sub
    Public Sub InitializeControl(isFuzzy As Boolean, ranking As Double, value As Double) Implements IViewFuzzyNumber.InitializeControl
        CurrentPresenter.InitView(isFuzzy, ranking, value)
    End Sub
    Public Sub InitializeControl(isFuzzy As Boolean, value As Double, fuzzyValue As String, name As String, shortName As String) Implements IViewFuzzyNumber.InitializeControl
        CurrentPresenter.InitView(isFuzzy, value, fuzzyValue, name, shortName)
    End Sub
    Public Sub InitializeControl(isFuzzy As Boolean, ranking As Double, value As Double, fuzzyString As String, name As String, shortName As String) Implements IViewFuzzyNumber.InitializeControl
        CurrentPresenter.InitView(isFuzzy, ranking, value, fuzzyString, name, shortName)
    End Sub

    Public Sub InitializeControl(isFuzzy As Boolean, ranking As Double, value As Double, fuzzyString As String, type As RatingType, name As String, shortName As String, Optional endName As String = "", Optional endShortName As String = "") Implements IViewFuzzyNumber.InitializeControl
        Select Case Type
            Case RatingType.extended
                name = name & " &rarr; " & endName
                shortName = shortName & " &rarr; " & endShortName
            Case RatingType.intermediateValues
                name = name & " / " & endName
                shortName = shortName & " / " & endShortName
        End Select
        CurrentPresenter.InitView(isFuzzy, ranking, value, fuzzyString, name, shortName)
    End Sub
    Public Sub InitializeControl(isFuzzy As Boolean, value As Double, fuzzyString As String, type As RatingType, name As String, shortName As String, Optional endName As String = "", Optional endShortName As String = "") Implements IViewFuzzyNumber.InitializeControl
        Select Case type
            Case RatingType.extended
                name = name & " &rarr; " & endName
                shortName = shortName & " &rarr; " & endShortName
            Case RatingType.intermediateValues
                name = name & " / " & endName
                shortName = shortName & " / " & endShortName
        End Select
        CurrentPresenter.InitView(isFuzzy, value, fuzzyString, name, shortName)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewFuzzyNumber.DisplaySessionTimeout

    End Sub
    Private Sub RenderFuzzyNumber(name As String, shortName As String, fuzzyString As String, centerOfGravity As String) Implements IViewFuzzyNumber.RenderFuzzyNumber
        LTfuzzyNumber.Visible = True
        LTnumber.Visible = False
        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#number#", fuzzyString)
        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#cog#", centerOfGravity)
        If ShowFazzyValueWithCog Then
            LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#name#", fuzzyString & " (" & centerOfGravity & ")")
        Else
            LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#name#", name)
        End If

        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#shortName#", shortName)
    End Sub
    Private Sub RenderRankingFuzzyNumber(name As String, shortName As String, ranking As String, fuzzyString As String, centerOfGravity As String) Implements IViewFuzzyNumber.RenderRankingFuzzyNumber
        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#ranking#", ranking)
        RenderRankingFuzzyNumber(name, shortName, fuzzyString, centerOfGravity)
    End Sub
    Private Sub RenderRankingFuzzyNumber(name As String, shortName As String, fuzzyString As String, centerOfGravity As String) Implements IViewFuzzyNumber.RenderRankingFuzzyNumber
        LTfuzzyNumber.Visible = True
        LTnumber.Visible = False
        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#number#", fuzzyString)
        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#cog#", centerOfGravity)

        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#name#", name)

        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#shortName#", shortName)
        LTfuzzyNumber.Text = Replace(LTfuzzyNumber.Text, "#ranking#", "")
    End Sub

    Private Sub RenderNumber(name As String, number As String) Implements IViewFuzzyNumber.RenderNumber
        LTfuzzyNumber.Visible = False
        LTnumber.Visible = True
        LTnumber.Text = number
    End Sub
#End Region




End Class