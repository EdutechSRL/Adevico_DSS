Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC

Public Class UC_GlossaryShareState
    Inherits GLbaseControl
    Implements IViewUC_GlosssaryShareState

#Region "Context"

    Private _Presenter As UC_GlossaryShareStatePresenter

    Private ReadOnly Property CurrentPresenter() As UC_GlossaryShareStatePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_GlossaryShareStatePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Private Property FromIdCommunity As Int32
        Get
            Return ViewStateOrDefault("FromIdCommunity", 0)
        End Get
        Set(value As Int32)
            ViewState("FromIdCommunity") = value
        End Set
    End Property

    Private Property DtoShare As DTO_Share
        Get
            Return ViewStateOrDefault("DtoShare", New DTO_Share)
        End Get
        Set(value As DTO_Share)
            ViewState("DtoShare") = value
        End Set
    End Property

#End Region

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBaddTerm_t)
            .setLabel(LBeditTerm_t)
            .setLabel(LBdeleteTerm_t)
            .setLiteral(LTpermission_t)
            .setLabel(LBorRefuseIt_t)
            .setLabel(LBallowGlossarySharing_t)
            .setLabel(LBsharedStatus_t)
            .setLiteral(LTsharedFrom_t)

            .setLabel(LBaddTerm2_t)
            .setLabel(LBeditTerm2_t)
            .setLabel(LBdeleteTerm2_t)
            .setLiteral(LTpermission2_t)
            .setLabel(LBsharedStatus2_t)
            .setLiteral(LTsharedFrom2_t)
            .setLiteral(LTpermanentDiscard2_t)
            .setLabel(LBvisibiltyStatus2_t)

            SWHshareEnabled.SetText(Resource, False, False)

            .setLinkButton(LNBshareAccept, True, True, False, True)
            .setLinkButton(LNBshareRefuse, True, True, False, True)

            .setLinkButton(LNBrefuse2, True, True, False, True)
        End With
    End Sub

    Public Property ItemData() As DTO_Glossary Implements IViewUC_GlosssaryShareState.ItemData

    Public Sub SetTitle(ByVal name As String) Implements IViewUC_GlosssaryShareState.SetTitle
    End Sub

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal dto_share As DTO_Share, ByVal manageEnabled As Boolean, ByVal manage As Boolean) Implements IViewUC_GlosssaryShareState.LoadViewData

        If manageEnabled AndAlso dto_share IsNot Nothing Then
            'If manage Then
            FromIdCommunity = idCommunity
            DtoShare = dto_share
            Dim key As String = "ShareStatusEnum." + dto_share.Status.ToString()
            Dim shareState As String = Resource.getValue(key)

            LTshareState.Text = shareState
            LTshareState2.Text = shareState
            LTshareState3.Text = shareState
            LTshareState4.Text = shareState

            LTcommunityFrom.Text = dto_share.FromCommunityName

            LTglossaryDescription.Text = String.Empty
            LTglossaryDescription2.Text = String.Empty

            'LTglossaryDescription.Text = dto_share.GlossaryDescription
            CBXaddTerm.Checked = dto_share.HasPermission(SharePermissionEnum.AddTerm)
            CBXeditTerm.Checked = dto_share.HasPermission(SharePermissionEnum.EditTerm)
            CBXdeleteTerm.Checked = dto_share.HasPermission(SharePermissionEnum.DeleteTerm)

            LTcommunityFrom2.Text = dto_share.FromCommunityName
            'LTglossaryDescription2.Text = dto_share.GlossaryDescription

            CBXaddTerm2.Checked = dto_share.HasPermission(SharePermissionEnum.AddTerm)
            CBXeditTerm2.Checked = dto_share.HasPermission(SharePermissionEnum.EditTerm)
            CBXdeleteTerm2.Checked = dto_share.HasPermission(SharePermissionEnum.DeleteTerm)

            SWHshareEnabled.Status = dto_share.Visible

            If dto_share.Status = ShareStatusEnum.Pending Then
                MLVglossaryShare.SetActiveView(VIWglossarySharePending)
                PNLRefuse.Visible = True
                PNLRefuse1.Visible = True
            ElseIf dto_share.Status = ShareStatusEnum.Active Then
                MLVglossaryShare.SetActiveView(VIWglossaryShareEnabled)
                PNLRefuse.Visible = True
                PNLRefuse1.Visible = True
            ElseIf dto_share.Status = ShareStatusEnum.ForceActive Then
                MLVglossaryShare.SetActiveView(VIWglossaryShareEnabled)
            ElseIf dto_share.Status = ShareStatusEnum.Refused Then
                MLVglossaryShare.SetActiveView(VIWglossaryShareRefused)
            Else
                MLVglossaryShare.SetActiveView(VIWglossaryShareEmpty)
            End If

        Else
            MLVglossaryShare.SetActiveView(VIWglossaryShareEmpty)
        End If
    End Sub

    Public Sub InitializeControl(ByVal idCommunity As Integer, ByVal idGlossary As Long, ByVal isManage As Boolean)
        CurrentPresenter.InitView(isManage)
    End Sub

    Public Function GetShareState() As String
        Dim result As String = DtoShare.Status.ToString().ToLower()
        Return result
    End Function

    Private Sub LNBshareAccept_Click(sender As Object, e As EventArgs) Handles LNBshareAccept.Click
        CurrentPresenter.ChangeShareStatus(ShareStatusEnum.Active)
    End Sub

    Private Sub LNBshareRefuse_Click(sender As Object, e As EventArgs) Handles LNBshareRefuse.Click
        CurrentPresenter.ChangeShareStatus(ShareStatusEnum.Refused)
    End Sub

    Private Sub SWHshareEnabled_StatusChange(Status As Boolean) Handles SWHshareEnabled.StatusChange
        CurrentPresenter.ChangeShareVisibility(Status)
    End Sub

    Private Sub LNBrefuse2_Click(sender As Object, e As EventArgs) Handles LNBrefuse2.Click
        CurrentPresenter.ChangeShareStatus(ShareStatusEnum.Refused)
    End Sub
End Class