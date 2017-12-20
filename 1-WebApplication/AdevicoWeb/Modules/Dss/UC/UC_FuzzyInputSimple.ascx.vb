Imports lm.Comol.Core.Dss.Domain
Imports lm.Comol.Core.Dss.Domain.Templates
Public Class UC_FuzzyInputSimple
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
            Return SLratingValues.SelectedIndex > -1
        End Get
    End Property
    Public Property Disabled As Boolean
        Get
            Return SLratingValues.Disabled
        End Get
        Set(value As Boolean)
            SLratingValues.Disabled = value
        End Set
    End Property
    Public ReadOnly Property IdSelected As Long
        Get
            If SLratingValues.SelectedIndex > -1 Then
                Return Long.Parse(SLratingValues.Value)
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
            LTfuzzyInputDescription.Text = .getValue("Description.RatingType." & RatingType.Simple.ToString)
            DVdescription.Visible = Not String.IsNullOrWhiteSpace(LTfuzzyInputDescription.Text)
            .setLabel(LBfuzzyInputTitle)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As dtoSelectRatingSet, idSelected As Long)
        SetInternazionalizzazione()
        If Not IsNothing(item) AndAlso item.Values.Any Then
            SLratingValues.DataSource = item.Values
            SLratingValues.DataTextField = "Name"
            SLratingValues.DataValueField = "Id"
            SLratingValues.DataBind()
            If (item.Values.Any(Function(v) v.Id = idSelected)) Then
                SLratingValues.Value = idSelected.ToString
            End If
        Else
            SLratingValues.Items.Clear()
        End If
    End Sub
    Public Sub InitializeControl(item As dtoGenericRatingSet, idSelected As Long)
        SetInternazionalizzazione()
        If Not IsNothing(item) AndAlso item.Values.Any Then
            SLratingValues.DataSource = item.Values
            SLratingValues.DataTextField = "Name"
            SLratingValues.DataValueField = "Id"
            SLratingValues.DataBind()
            If (item.Values.Any(Function(v) v.Id = idSelected)) Then
                SLratingValues.Value = idSelected.ToString
            End If
        Else
            SLratingValues.Items.Clear()
        End If

    End Sub
    Public Function GetValueToString() As String
        If IdSelected > 0 Then
            Return SLratingValues.Items(SLratingValues.SelectedIndex).Text
        Else
            Return ""
        End If
    End Function
#End Region

End Class