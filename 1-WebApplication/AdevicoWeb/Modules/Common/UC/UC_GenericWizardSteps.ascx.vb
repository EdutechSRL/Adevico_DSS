Public Class UC_GenericWizardSteps
    Inherits CMbaseControl

#Region "Internal"
    Public Event ItemCommand(ByVal idStep As Integer)
    Private Property DisableAll As Boolean
        Get
            Return ViewStateOrDefault("DisableAll", False)
        End Get
        Set(value As Boolean)
            ViewState("DisableAll") = value
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
    Public Sub InitializeControl(items As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of Integer)))
        DisableAll = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext.isAnonymous
        If items.Any() Then
            Select Case items.Count
                Case 1
                    items(0).DisplayOrderDetail = lm.Comol.Core.Wizard.DisplayOrderEnum.first Or lm.Comol.Core.Wizard.DisplayOrderEnum.none
                Case Else
                    items.FirstOrDefault().DisplayOrderDetail = lm.Comol.Core.Wizard.DisplayOrderEnum.first
                    items.LastOrDefault.DisplayOrderDetail = lm.Comol.Core.Wizard.DisplayOrderEnum.last
            End Select
        End If
        Me.RPTsteps.DataSource = items
        Me.RPTsteps.DataBind()
    End Sub

    Private Sub RPTsteps_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsteps.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim baseItem As lm.Comol.Core.Wizard.NavigableWizardItemBase(Of Integer) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItemBase(Of Integer))

            Dim oDiv As HtmlControl = e.Item.FindControl("DVnavigationStep")
            If TypeOf baseItem Is lm.Comol.Core.Wizard.NavigableWizardItemSeparator(Of Integer) Then
                Dim oLiteral As Literal = e.Item.FindControl("LTseparator")
                oDiv.Visible = False
                oLiteral.Visible = True
            Else
                Dim item As lm.Comol.Core.Wizard.NavigableWizardItem(Of Integer) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItem(Of Integer))

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
                        oLinkbutton.CommandName = item.Id
                        oLinkbutton.CommandArgument = item.Url
                    Else
                        oHyperlink.Text = item.Name
                        oHyperlink.ToolTip = item.Tooltip
                        oHyperlink.NavigateUrl = BaseUrl & item.Url
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
            PageUtility.RedirectToUrl(e.CommandArgument)
        End If
    End Sub
#End Region


End Class


'Public Class ProjectWizardSteps
'    Inherits BaseControl





'End Class