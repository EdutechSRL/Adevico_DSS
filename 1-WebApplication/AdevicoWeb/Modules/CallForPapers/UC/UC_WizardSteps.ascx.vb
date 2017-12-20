Public Class WizardSteps
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
    Private Property CallType As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType
        Get
            Return ViewStateOrDefault("CallType", lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType)
            ViewState("CallType") = value
        End Set
    End Property
    Private Property CurrentView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters
        Get
            Return ViewStateOrDefault("CurrentView", lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionOpened)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters)
            ViewState("CurrentView") = value
        End Set
    End Property

#End Region

    Public Event ItemCommand(ByVal callStep As lm.Comol.Modules.CallForPapers.Domain.WizardCallStep)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region


    Public Sub InitalizeControl(idItem As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType, idCommunity As Integer, cView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters, items As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep)))
        DisableAll = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext.isAnonymous
        IdCall = idItem
        IdCallCommunity = idCommunity
        CallType = type
        CurrentView = cView
        For Each item As lm.Comol.Core.Wizard.NavigableWizardItem(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep) In items
            item.Name = Resource.getValue("WizardCallStep." & item.Id.ToString)
            item.Tooltip = Resource.getValue("WizardCallStep.Tooltip." & item.Id.ToString)
        Next
        Me.RPTsteps.DataSource = items
        Me.RPTsteps.DataBind()
    End Sub

    Private Sub RPTsteps_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsteps.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim baseItem As lm.Comol.Core.Wizard.NavigableWizardItemBase(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItemBase(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep))

            Dim oDiv As HtmlControl = e.Item.FindControl("DVnavigationStep")
            If TypeOf baseItem Is lm.Comol.Core.Wizard.NavigableWizardItemSeparator(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep) Then
                Dim oLiteral As Literal = e.Item.FindControl("LTseparator")
                oDiv.Visible = False
                oLiteral.Visible = True
            Else
                Dim item As lm.Comol.Core.Wizard.NavigableWizardItem(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItem(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep))

                oDiv.Visible = True
                Dim oLabel As Label = e.Item.FindControl("LBitem")
                Dim oHyperlink As HyperLink = e.Item.FindControl("HTPlink")
                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBlink")

                If (item.Status) = lm.Comol.Core.Wizard.WizardItemStatus.disabled OrElse DisableAll OrElse item.Active Then
                    oLabel.Visible = True
                    oLabel.Text = item.Name
                Else
                    oHyperlink.Visible = Not item.AutoPostBack

                    oLinkbutton.Visible = item.AutoPostBack
                    If item.AutoPostBack Then
                        oLinkbutton.Text = item.Name
                        oLinkbutton.ToolTip = item.Tooltip
                        oLinkbutton.CommandName = item.Id.ToString
                    Else
                        oHyperlink.Text = item.Name
                        oHyperlink.ToolTip = item.Tooltip
                        oHyperlink.NavigateUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditByStep(CallType, IdCall, IdCallCommunity, item.Id, CurrentView)
                    End If
                End If
                oLabel = e.Item.FindControl("LBmessage")
                If item.Status <> lm.Comol.Core.Wizard.WizardItemStatus.valid Then
                    Select Case item.Id
                        Case lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.NotificationTemplateMail, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.SubmittersType, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.SubmitterTemplateMail, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.SubmissionEditor, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.CallAvailability, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.Attachments
                            item.Message = Resource.getValue("WizardCallStep." & item.Id.ToString & ".Status." & item.Status.ToString)
                        Case Else

                    End Select
                End If
                oLabel.Visible = Not String.IsNullOrEmpty(item.Message)
                oLabel.Text = item.Message
            End If
        End If
    End Sub

    Private Sub RPTsteps_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsteps.ItemCommand
        If Not String.IsNullOrEmpty(e.CommandName) Then
            Dim selStep As lm.Comol.Modules.CallForPapers.Domain.WizardCallStep
            selStep = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep).GetByString(e.CommandName, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.none)
            Dim url As String = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditByStep(CallType, IdCall, IdCallCommunity, selStep, CurrentView)
            PageUtility.RedirectToUrl(url)
            ' RaiseEvent ItemCommand(lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.WizardCallStep).GetByString(e.CommandName, lm.Comol.Modules.CallForPapers.Domain.WizardCallStep.none))
        End If
    End Sub

   
End Class