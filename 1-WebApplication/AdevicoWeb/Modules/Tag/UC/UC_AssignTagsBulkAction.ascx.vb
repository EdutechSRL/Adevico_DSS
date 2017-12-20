Public Class UC_AssignTagsBulkAction
    Inherits TGbaseControl

    Public Event ApplyTags(ByVal tags As List(Of Long), applyToAll As Boolean)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBassignTagsBulkTitle)
            .setLiteral(LTtagSelector_t)
            .setLabel(LBselectOnAllPages)
            .setButton(BTNapplyTagsOnSelectedCommunities, True)
        End With
    End Sub
#End Region
#Region "Internal"
    Public Sub InitializeControl(tags As List(Of lm.Comol.Core.Tag.Domain.dtoTagSelectItem), ByVal hasMultiPage As Boolean)
        CTRLtagsSelector.InitializeControl(tags)
        CBXselectAll.Visible = hasMultiPage
        LBselectOnAllPages.Visible = hasMultiPage
    End Sub
    Private Sub BTNapplyTagsOnSelectedCommunities_Click(sender As Object, e As EventArgs) Handles BTNapplyTagsOnSelectedCommunities.Click
        RaiseEvent ApplyTags(CTRLtagsSelector.GetSelectedTags, CBXselectAll.Checked)
    End Sub
#End Region

  
End Class