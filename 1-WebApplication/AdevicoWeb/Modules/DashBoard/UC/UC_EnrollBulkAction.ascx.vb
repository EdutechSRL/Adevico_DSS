Public Class UC_EnrollBulkAction
    Inherits DBbaseControl

    Public Event EnrollToBulkActions(applyToAll As Boolean)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBenrollToCommunitiesBulkTitle)
            .setLabel(LBenrollToSelectOnAllPages)
            .setButton(BTNenrollToSelectedCommunities, True)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(ByVal hasMultiPage As Boolean, items As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityToEnroll))
        CBXselectAll.Visible = hasMultiPage
        LBenrollToSelectOnAllPages.Visible = hasMultiPage

        If IsNothing(items) OrElse Not items.Any() Then
            LBextraInfoForCommunitiesSelected.Visible = False
        Else
            LBextraInfoForCommunitiesSelected.Visible = True
            If items.Count = 1 Then
                LBextraInfoForCommunitiesSelected.Text = Resource.getValue("LBextraInfoForCommunitiesSelected.1")
                If LBextraInfoForCommunitiesSelected.Text.Contains("{0}") Then
                    LBextraInfoForCommunitiesSelected.Text = String.Format(LBextraInfoForCommunitiesSelected.Text, items.Select(Function(i) i.PageIndex).FirstOrDefault)
                End If
            Else
                Resource.setLabel(LBextraInfoForCommunitiesSelected)
                If LBextraInfoForCommunitiesSelected.Text.Contains("{0}") Then
                    LBextraInfoForCommunitiesSelected.Text = String.Format(LBextraInfoForCommunitiesSelected.Text, items.Count, String.Join(", ", items.Select(Function(i) i.PageIndex).Distinct().ToArray))
                End If
            End If
        End If
    End Sub
    Private Sub BTNenrollToSelectedCommunities_Click(sender As Object, e As EventArgs) Handles BTNenrollToSelectedCommunities.Click
        RaiseEvent EnrollToBulkActions(CBXselectAll.Checked)
    End Sub
#End Region

End Class