Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class UC_RangeSettings
    Inherits DBbaseControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBrangeItemsFrom)
            .setLabel(LBrangeItemsTo)
            .setLabel(LBrangeItemsDisplay)
            .setLabel(LBrangeItems_t)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(range As RangeSettings, type As DashboardType)
        LBrangeItemsFrom.Text = Resource.getValue("LBrangeItemsFrom.DashboardType." & type.ToString)

        If Not IsNothing(range) AndAlso range.DisplayItems > 0 Then
            TXBrangeMin.Text = range.LowerLimit
            TXBrangeMax.Text = range.HigherLimit
            TXBrangeDisplayItems.Text = range.DisplayItems
        Else
            TXBrangeMin.Text = 0
            TXBrangeMax.Text = 0
            TXBrangeDisplayItems.Text = 0
        End If
    End Sub
    Public Function GetRange() As RangeSettings
        Dim dto As New RangeSettings
        If IsNumeric(TXBrangeMin.Text) AndAlso IsNumeric(TXBrangeMax.Text) AndAlso IsNumeric(TXBrangeDisplayItems.Text) Then
            dto.LowerLimit = CInt(TXBrangeMin.Text)
            dto.HigherLimit = CInt(TXBrangeMax.Text)
            dto.DisplayItems = CInt(TXBrangeDisplayItems.Text)
            'If dto.LowerLimit < dto.HigherLimit AndAlso dto.HigherLimit > 0 AndAlso dto.DisplayItems >= dto.LowerLimit AndAlso dto.DisplayItems > 0 Then
            '    Return dto
            'Else

            'End If
        End If
        Return dto
    End Function
#End Region
End Class