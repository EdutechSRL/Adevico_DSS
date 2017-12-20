Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Public Class UC_WizardEvaluationCommitteesSteps
    Inherits BaseControl

    Private Property DisableAll As Boolean
        Get
            Return ViewStateOrDefault("DisableAll", False)
        End Get
        Set(value As Boolean)
            ViewState("DisableAll") = value
        End Set
    End Property

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Private Property IdCall As Long
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCallCommunity") = value
        End Set
    End Property
    'Private Property CallType As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType
    '    Get
    '        Return ViewStateOrDefault("CallType", lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids)
    '    End Get
    '    Set(value As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType)
    '        ViewState("CallType") = value
    '    End Set
    'End Property
    Private Property CurrentView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters
        Get
            Return ViewStateOrDefault("CurrentView", lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters)
            ViewState("CurrentView") = value
        End Set
    End Property

#End Region

    Public Event ItemCommand(ByVal cStep As WizardEvaluationStep)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region
    Public Sub InitalizeControl(idItem As Long, idCommunity As Integer, cView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters, items As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep)))
        InitalizeControl(idItem, idCommunity, cView, items, EvaluationEditorErrors.None)
    End Sub

    Public Sub InitalizeControl(idItem As Long, idCommunity As Integer, cView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters, items As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep)), err As EvaluationEditorErrors)
        DisableAll = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext.isAnonymous
        IdCall = idItem
        IdCallCommunity = idCommunity
        CurrentView = cView
        For Each item As lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep) In items
            item.Name = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString)
            item.Tooltip = Resource.getValue("WizardEvaluationStep.Tooltip." & item.Id.Type.ToString)

            Dim m As String = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message")
            Select Case item.Id.Type
                Case WizardEvaluationStep.GeneralSettings
                    Dim oItem As dtoEvaluationCommitteeStep = DirectCast(item.Id, dtoEvaluationCommitteeStep)
                    If oItem.Errors.Count = 0 OrElse oItem.Errors.Contains(EditingErrors.None) Then
                        item.Message = String.Format(m, oItem.ItemsCount)
                    ElseIf oItem.Errors.Contains(EditingErrors.CommitteeDssSettings) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.CommitteeDssSettings.ToString)
                    End If

                Case WizardEvaluationStep.FullManageEvaluators
                    Dim oItem As dtoEvaluationEvaluatorsStep = DirectCast(item.Id, dtoEvaluationEvaluatorsStep)
                    If oItem.Errors.Count = 0 OrElse oItem.Errors.Contains(EditingErrors.None) Then
                        item.Message = String.Format(m, oItem.ItemsCount)
                    ElseIf oItem.Errors.Contains(EditingErrors.NoEvaluators) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.NoEvaluators.ToString)
                    ElseIf oItem.Errors.Contains(EditingErrors.UnassingedEvaluators) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.UnassingedEvaluators.ToString)
                    ElseIf oItem.Errors.Contains(EditingErrors.MoreEvaluatorAssignment) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.MoreEvaluatorAssignment.ToString)
                    ElseIf oItem.Errors.Contains(EditingErrors.CommitteeWithNoEvaluators) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.CommitteeWithNoEvaluators.ToString)
                    End If
                Case WizardEvaluationStep.ManageEvaluators
                    Dim oItem As dtoEvaluationViewEvaluatorsStep = DirectCast(item.Id, dtoEvaluationViewEvaluatorsStep)
                    If oItem.Errors.Count = 0 OrElse oItem.Errors.Contains(EditingErrors.None) Then
                        item.Message = String.Format(m, oItem.EvaluatorsCount, oItem.CommitteesCount)
                        If oItem.GetMembershipsCount(MembershipStatus.Replaced) > 0 Then
                            item.Message &= "<br/>" & String.Format(Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.MembershipStatus." & MembershipStatus.Replaced.ToString), oItem.GetMembershipsCount(MembershipStatus.Replaced))
                        End If
                        If oItem.GetMembershipsCount(MembershipStatus.Replacing) > 0 Then
                            item.Message &= "<br/>" & String.Format(Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.MembershipStatus." & MembershipStatus.Replacing.ToString), oItem.GetMembershipsCount(MembershipStatus.Replacing))
                        End If
                        If oItem.GetMembershipsCount(MembershipStatus.Removed) > 0 Then
                            item.Message &= "<br/>" & String.Format(Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.MembershipStatus." & MembershipStatus.Removed.ToString), oItem.GetMembershipsCount(MembershipStatus.Removed))
                        End If
                    ElseIf oItem.Errors.Contains(EditingErrors.NoEvaluators) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.NoEvaluators.ToString)
                    ElseIf oItem.Errors.Contains(EditingErrors.UnassingedEvaluators) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.UnassingedEvaluators.ToString)
                    ElseIf oItem.Errors.Contains(EditingErrors.MoreEvaluatorAssignment) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.MoreEvaluatorAssignment.ToString)
                    ElseIf oItem.Errors.Contains(EditingErrors.CommitteeWithNoEvaluators) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.CommitteeWithNoEvaluators.ToString)
                    End If
                Case WizardEvaluationStep.AssignSubmissionWithNoEvaluation
                    Dim errors As New List(Of EditingErrors)
                    Dim sItem As dtoAssignSubmissionWithNoEvaluationtStep = DirectCast(item.Id, dtoAssignSubmissionWithNoEvaluationtStep)

                    If errors.Count = 0 OrElse errors.Contains(EditingErrors.None) Then
                        item.Message = String.Format(m, sItem.ItemsCount, sItem.SubmittedCount, sItem.AcceptedCount, sItem.RejectedCount)
                    End If
                Case WizardEvaluationStep.AssignSubmission, WizardEvaluationStep.MultipleAssignSubmission
                    Dim errors As New List(Of EditingErrors)
                    Dim submissionCount As Long = 0
                    If item.Id.Type = WizardEvaluationStep.AssignSubmission Then
                        errors = DirectCast(item.Id, dtoEvaluationSingleAssignmentStep).Errors
                        submissionCount = DirectCast(item.Id, dtoEvaluationSingleAssignmentStep).ItemsCount
                    Else
                        errors = DirectCast(item.Id, dtoEvaluationMultipleAssignmentStep).Errors
                        submissionCount = DirectCast(item.Id, dtoEvaluationMultipleAssignmentStep).ItemsCount
                    End If

                    If errors.Count = 0 OrElse errors.Contains(EditingErrors.None) Then
                        item.Message = String.Format(m, submissionCount)
                    ElseIf errors.Contains(EditingErrors.NoEvaluatorsForAssignments) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.NoEvaluatorsForAssignments.ToString)
                    ElseIf errors.Contains(EditingErrors.NoSubmissionToEvaluate) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.NoSubmissionToEvaluate.ToString)
                    ElseIf errors.Contains(EditingErrors.SubmissionToAssign) Then
                        item.Message = Resource.getValue("WizardEvaluationStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.SubmissionToAssign.ToString)

                    End If
                Case WizardEvaluationStep.ManageEvaluations
                    Dim oItem As dtoEvaluationsManageStep = DirectCast(item.Id, dtoEvaluationsManageStep)
                    item.Message = String.Format(m, oItem.EvaluatingCount + oItem.NotStartedCount, oItem.EvaluatedCount)
            End Select
        Next
        Me.RPTsteps.DataSource = items
        Me.RPTsteps.DataBind()
    End Sub


  
    Private Sub RPTsteps_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsteps.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim baseItem As lm.Comol.Core.Wizard.NavigableWizardItemBase(Of dtoEvaluationStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItemBase(Of dtoEvaluationStep))

            Dim oDiv As HtmlControl = e.Item.FindControl("DVnavigationStep")
            If TypeOf baseItem Is lm.Comol.Core.Wizard.NavigableWizardItemSeparator(Of dtoEvaluationStep) Then
                Dim oLiteral As Literal = e.Item.FindControl("LTseparator")
                oDiv.Visible = False
                oLiteral.Visible = True
            Else
                Dim item As lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep))

                oDiv.Visible = True
                Dim oLabel As Label = e.Item.FindControl("LBitem")
                Dim oHyperlink As HyperLink = e.Item.FindControl("HTPlink")
                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBlink")

                If item.Status = lm.Comol.Core.Wizard.WizardItemStatus.disabled OrElse DisableAll OrElse item.Active Then
                    oLabel.Visible = True
                    oLabel.Text = item.Name
                Else
                    oHyperlink.Visible = Not item.AutoPostBack

                    oLinkbutton.Visible = item.AutoPostBack
                    If item.AutoPostBack Then
                        oLinkbutton.Text = item.Name
                        oLinkbutton.ToolTip = item.Tooltip
                        oLinkbutton.CommandName = item.Id.Type.ToString
                    Else
                        oHyperlink.Text = item.Name
                        oHyperlink.ToolTip = item.Tooltip
                        oHyperlink.NavigateUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(IdCall, IdCallCommunity, item.Id.Type, CurrentView)
                    End If
                End If
                oLabel = e.Item.FindControl("LBmessage")
                oLabel.Visible = Not String.IsNullOrEmpty(item.Message)
                If Not String.IsNullOrEmpty(item.Message) Then
                    oLabel.Text = item.Message
                End If


            End If
        End If
    End Sub

    Private Sub RPTsteps_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsteps.ItemCommand
        If Not String.IsNullOrEmpty(e.CommandName) Then
            Dim selStep As WizardEvaluationStep
            selStep = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WizardEvaluationStep).GetByString(e.CommandName, WizardEvaluationStep.none)
            Dim url As String = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(IdCall, IdCallCommunity, selStep, CurrentView)
            PageUtility.RedirectToUrl(url)
            ' RaiseEvent ItemCommand(lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep).GetByString(e.CommandName, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.none))
        End If
    End Sub

End Class