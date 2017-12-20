Public Class UC_CommunityTreeMenu
    Inherits DBbaseControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        If Not Page.IsPostBack OrElse HYPnodeDetails.Text.Contains("{0}") Then
            With Resource
                HYPnodeDetails.Text = String.Format(HYPnodeDetails.Text, .getValue("HYPnodeDetails.text"))
                HYPnodeDetails.ToolTip = .getValue("HYPnodeDetails.ToolTip")
                HYPnodeViewNews.Text = String.Format(HYPnodeViewNews.Text, .getValue("HYPnodeViewNews.text"))
                HYPnodeViewNews.ToolTip = .getValue("HYPnodeViewNews.ToolTip")

                LNBnodeEnrollTo.Text = String.Format(LNBnodeEnrollTo.Text, .getValue("LNBnodeEnrollTo.text"))
                LNBnodeEnrollTo.ToolTip = .getValue("LNBnodeEnrollTo.ToolTip")
                LNBnodeUnsubscribeFrom.Text = String.Format(LNBnodeUnsubscribeFrom.Text, .getValue("LNBnodeUnsubscribeFrom.text"))
                LNBnodeUnsubscribeFrom.ToolTip = .getValue("LNBnodeUnsubscribeFrom.ToolTip")
                LNBnodeAccessTo.Text = String.Format(LNBnodeAccessTo.Text, .getValue("LNBnodeAccessTo.text"))
                LNBnodeAccessTo.ToolTip = .getValue("LNBnodeAccessTo.ToolTip")
            End With
        End If
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityNodeItem)
        If IsNothing(item.Details.Permissions) Then
            DBmenu.Visible = False
        Else
            SetInternazionalizzazione()
            DBmenu.Visible = item.Details.Permissions.ViewDetails OrElse item.Details.Permissions.AccessTo OrElse item.Details.Permissions.EnrollTo OrElse item.Details.Permissions.UnsubscribeFrom OrElse item.Details.Permissions.ViewNews

            HYPnodeDetails.Visible = item.Details.Permissions.ViewDetails
            HYPnodeDetails.NavigateUrl = PageUtility.ApplicationUrlBase() & lm.Comol.Core.Dashboard.Domain.RootObject.CommunityDetails(item.IdCommunity, True)

            DVnews.Visible = item.Details.Permissions.ViewNews
            If item.Details.Permissions.ViewNews Then
                Dim FromDay As DateTime = DateTime.MinValue
                If item.Details.LastAccessOn.HasValue Then
                    FromDay = item.Details.LastAccessOn.Value
                ElseIf item.Details.EnrolledOn.HasValue Then
                    FromDay = item.Details.EnrolledOn.Value
                Else
                    FromDay = Now.Date.AddDays(-30)
                End If

                HYPnodeViewNews.NavigateUrl = PageUtility.ApplicationUrlBase() & "Notification/CommunityNews.aspx?FromDay=" & Me.PageUtility.GetUrlEncoded(FromDay.ToString) & "&PageSize=25&Page=0&CommunityID=" & item.IdCommunity & "&PR_View=" & IIf(item.ForAdvanced, lm.Modules.NotificationSystem.Domain.ViewModeType.FromDashboardTreeAdvanced.ToString, lm.Modules.NotificationSystem.Domain.ViewModeType.FromDashboardTree.ToString)
            End If
            DVnews.Visible = item.Details.Permissions.ViewNews
            DVaccessTo.Visible = item.Details.Permissions.AccessTo
            DVenrollTo.Visible = item.Details.Permissions.EnrollTo
            DVunsubscribeFrom.Visible = item.Details.Permissions.UnsubscribeFrom
            If item.Details.Permissions.AccessTo AndAlso item.Details.Permissions.UnsubscribeFrom Then
                DVaccessTo.Attributes("class") = Replace(DVunsubscribeFrom.Attributes("class"), lm.Comol.Core.Wizard.DisplayOrderEnum.last.ToString(), "")
            ElseIf item.Details.Permissions.AccessTo AndAlso Not item.Details.Permissions.UnsubscribeFrom Then
                DVaccessTo.Attributes("class") = DVunsubscribeFrom.Attributes("class")
            End If

            LNBnodeEnrollTo.CommandArgument = item.IdCommunity & "," & item.CurrentPath
            LNBnodeUnsubscribeFrom.CommandArgument = LNBnodeEnrollTo.CommandArgument
            LNBnodeAccessTo.CommandArgument = LNBnodeEnrollTo.CommandArgument
        End If
    End Sub
#End Region
   
End Class