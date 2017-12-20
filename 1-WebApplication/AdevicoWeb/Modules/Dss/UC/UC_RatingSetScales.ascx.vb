
Imports lm.Comol.Core.Dss.Domain.Templates
Imports lm.Comol.Core.Dss.Presentation.Evaluation
Imports lm.Comol.Core.Dss.Domain
Public Class UC_RatingSetScales
    Inherits DSSbaseControl
    Implements IViewRatingSetSelector

#Region "Context"
    Private _Presenter As RatingScalesPresenter
    Private ReadOnly Property CurrentPresenter() As RatingScalesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RatingScalesPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IsFuzzy As Boolean Implements IViewRatingSetSelector.IsFuzzy
        Get
            Return ViewStateOrDefault("IsFuzzy", False)
        End Get
        Set(value As Boolean)
            ViewState("IsFuzzy") = value
        End Set
    End Property
    Public Property Disabled As Boolean Implements IViewRatingSetSelector.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
            For Each row As RepeaterItem In RPTratingSets.Items
                Dim oSelect As HtmlSelect = row.FindControl("SLratingValues")
                oSelect.Disabled = value
            Next
        End Set
    End Property
    Public Property DisplayNumberField As Boolean Implements IViewRatingSetSelector.DisplayNumberField
        Get
            Return ViewStateOrDefault("DisplayNumberField", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayNumberField") = value
            THnumber.Visible = value
            For Each row As RepeaterItem In RPTratingSets.Items
                Dim cell As HtmlTableCell = row.FindControl("TDnumber")
                cell.Visible = value
            Next
        End Set
    End Property
    Public Property NumberFieldDefaultValue As Integer Implements IViewRatingSetSelector.NumberFieldDefaultValue
        Get
            Return ViewStateOrDefault("NumberFieldDefaultValue", 1)
        End Get
        Set(value As Integer)
            ViewState("NumberFieldDefaultValue") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTratingSetHeader)
            .setLiteral(LTratingSetNumber)
            .setLiteral(LTratingSetValues)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(method As dtoSelectMethod, Optional disabled As Boolean = False) Implements IViewRatingSetSelector.InitializeControl
        SetInternazionalizzazione()
        CurrentPresenter.InitView(method, disabled)
    End Sub
    Public Sub InitializeControl(type As AlgorithmType, isFuzzy As Boolean, Optional disabled As Boolean = False) Implements IViewRatingSetSelector.InitializeControl
        SetInternazionalizzazione()
        CurrentPresenter.InitView(type, isFuzzy, disabled)
    End Sub
    Private Sub LoadRatingSet(items As List(Of dtoSelectRatingSet)) Implements IViewRatingSetSelector.LoadRatingSet
        RPTratingSets.DataSource = items
        RPTratingSets.DataBind()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewRatingSetSelector.DisplaySessionTimeout
        Disabled = True
    End Sub
    Public Function GetRatingSetSelected() As Dictionary(Of Long, Long) Implements IViewRatingSetSelector.GetRatingSetSelected
        Dim result As New Dictionary(Of Long, Long)
        For Each row As RepeaterItem In RPTratingSets.Items
            Dim oLiteral As Literal = row.FindControl("LTidRatingSet")
            Dim idRatingSet As Long = 0
            Long.TryParse(oLiteral.Text, idRatingSet)
            If idRatingSet > 0 AndAlso Not result.ContainsKey(idRatingSet) Then
                Dim oTextBox As TextBox = row.FindControl("TBXratingSet")
                Dim count As Long = 0
                Long.TryParse(oTextBox.Text, count)
                If count > 0 Then
                    result.Add(idRatingSet, count)
                Else
                    oTextBox.Text = "0"
                End If
            End If
        Next
        Return result
    End Function
    Public Function GetRatingValues() As Dictionary(Of Long, List(Of dtoGenericRatingValue)) Implements IViewRatingSetSelector.GetRatingValues
        Return CurrentPresenter.GetRatingSetValues(GetRatingSetSelected().Keys.ToList())
    End Function
#End Region

#Region "Internal"

#End Region

    Private Sub RPTratingSets_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTratingSets.ItemDataBound
        Dim dto As dtoSelectRatingSet = DirectCast(e.Item.DataItem, dtoSelectRatingSet)
        Dim oSelect As HtmlSelect = e.Item.FindControl("SLratingValues")
        oSelect.Disabled = Disabled
        If Not IsNothing(dto.Values) Then
            oSelect.DataSource = dto.Values
            oSelect.DataTextField = "Name"
            oSelect.DataValueField = "Id"
            oSelect.DataBind()
        End If
        Dim cell As HtmlTableCell = e.Item.FindControl("TDnumber")
        cell.Visible = Not Disabled
    End Sub

   
End Class