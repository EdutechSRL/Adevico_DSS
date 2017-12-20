Imports lm.Comol.Core.Dss.Domain
Imports lm.Comol.Core.Dss.Domain.Templates
Public Class UC_FuzzyInputExtended
    Inherits DSSbaseControl

#Region "Internal"
    Public Property IdObjectItem As Long
        Get
            Return ViewStateOrDefault("IdObjectItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdObjectItem") = value
        End Set
    End Property
    Public ReadOnly Property IsValid As Boolean
        Get
            Return SLratingValuesFrom.SelectedIndex > -1 AndAlso SLratingValuesTo.SelectedIndex > -1
        End Get
    End Property
    Public Property Disabled As Boolean
        Get
            Return SLratingValuesFrom.Disabled
        End Get
        Set(value As Boolean)
            SLratingValuesFrom.Disabled = value
            SLratingValuesTo.Disabled = value
        End Set
    End Property
    Public ReadOnly Property IdSelectedFrom As Long
        Get
            If SLratingValuesFrom.SelectedIndex > -1 Then
                Return Long.Parse(SLratingValuesFrom.Value)
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property IdSelectedTo As Long
        Get
            If SLratingValuesTo.SelectedIndex > -1 Then
                Return Long.Parse(SLratingValuesTo.Value)
            Else
                Return 0
            End If
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            LTfuzzyInputDescription.Text = .getValue("Description.RatingType." & RatingType.extended.ToString)
            DVdescription.Visible = Not String.IsNullOrWhiteSpace(LTfuzzyInputDescription.Text)
            .setLabel(LBfuzzyInputTitleTo)
            LBfuzzyInputTitle.Text = .getValue("LBfuzzyInputTitle.RatingType." & RatingType.extended.ToString)

        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As dtoSelectRatingSet, idFrom As Long, idTo As Long)
        SetInternazionalizzazione()
        If Not IsNothing(item) AndAlso item.Values.Any Then
            SLratingValuesFrom.DataSource = item.Values
            SLratingValuesFrom.DataTextField = "Name"
            SLratingValuesFrom.DataValueField = "Id"
            SLratingValuesFrom.DataBind()
            If (item.Values.Any(Function(v) v.Id = idFrom)) Then
                SLratingValuesFrom.Value = idFrom.ToString
            End If

            SLratingValuesTo.DataSource = item.Values
            SLratingValuesTo.DataTextField = "Name"
            SLratingValuesTo.DataValueField = "Id"
            SLratingValuesTo.DataBind()
            If (item.Values.Any(Function(v) v.Id = idTo)) Then
                SLratingValuesTo.Value = idTo.ToString
            End If
        Else
            SLratingValuesTo.Items.Clear()
            SLratingValuesFrom.Items.Clear()
        End If
    End Sub
    Public Sub InitializeControl(item As dtoGenericRatingSet, idFrom As Long, idTo As Long)
        SetInternazionalizzazione()
        If Not IsNothing(item) AndAlso item.Values.Any Then
            SLratingValuesFrom.DataSource = item.Values
            SLratingValuesFrom.DataTextField = "Name"
            SLratingValuesFrom.DataValueField = "Id"
            SLratingValuesFrom.DataBind()
            If (item.Values.Any(Function(v) v.Id = idFrom)) Then
                SLratingValuesFrom.Value = idFrom.ToString
            End If

            SLratingValuesTo.DataSource = item.Values
            SLratingValuesTo.DataTextField = "Name"
            SLratingValuesTo.DataValueField = "Id"
            SLratingValuesTo.DataBind()
            If (item.Values.Any(Function(v) v.Id = idTo)) Then
                SLratingValuesTo.Value = idTo.ToString
            End If
        Else
            SLratingValuesTo.Items.Clear()
            SLratingValuesFrom.Items.Clear()
        End If
    End Sub
    Public Function GetValueToString() As String
        Dim result As String = ""
        If IdSelectedFrom > 0 Then
            result = SLratingValuesFrom.Items(SLratingValuesFrom.SelectedIndex).Text
        Else
            result = ""
        End If
        If IdSelectedTo > 0 Then
            result &= " &rarr; " & SLratingValuesTo.Items(SLratingValuesTo.SelectedIndex).Text
        ElseIf Not String.IsNullOrWhiteSpace(result) Then
            result &= " &rarr; "
        End If
        Return result
    End Function
#End Region

End Class