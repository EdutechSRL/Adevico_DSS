Partial Public Class CTRLgridUnder
	Inherits PagerBaseControl
	
	Public Event OnSetPageSize(ByVal size As Integer)
	Public Event OnGotoPage(ByVal oPage As Pager, ByVal Index As Integer)

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Protected Overrides Sub ShowNavigationButton(ByVal pageIndex As Integer, ByVal pageCount As Integer)
		Me.IMBfirst.Visible = (Me.PageIndexStart > 1 And pageCount > 9)
		Me.IMBprev.Visible = (Me.PageIndexStart > 1 And pageCount > 9)
		Me.IMBnext.Visible = (Me.PageIndexEnd < pageCount And pageCount > 9)
		Me.IMBlast.Visible = (Me.PageIndexEnd < pageCount And pageCount > 9)
	End Sub

	Protected Overrides Sub ShowPageIndex(ByVal pageIndex As Integer, ByVal pageCount As Integer)
		For Index As Integer = 0 To 8
			Dim oLinkButton As New LinkButton
			oLinkButton = FindControl("LNB" & Chr(Index + 1))

			If IsNothing(oLinkButton) = False Then
				If pageCount = 0 Then
					oLinkButton.Visible = False
				Else
					oLinkButton.Text = (Index + Me.PageIndexStart).ToString
					If Index + Me.PageIndexStart = pageIndex Then
						oLinkButton.CssClass = "selected"
					Else
						oLinkButton.CssClass = ""
					End If
					If Index + Me.PageIndexStart <= Me.PageIndexEnd Then
						oLinkButton.Visible = True
					Else
						oLinkButton.Visible = False
					End If
				End If
			
			End If
		Next
	End Sub

	Private Sub SetPageIndex(ByVal pageIndex As Integer)
		Dim oLinkButton As New LinkButton
		oLinkButton = FindControl("LNB" & Chr((pageIndex - Me.PageIndexStart + 1)))
		If IsNothing(oLinkButton) = False Then
			oLinkButton.CssClass = "selected"
		End If
	End Sub

	Private Sub IMBfirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBfirst.Click
		RaiseEvent OnGotoPage(Pager.GoToFirst, 1)
	End Sub
	Private Sub IMBprev_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBprev.Click
		RaiseEvent OnGotoPage(Pager.GoToPreviuous, Me.PageIndexStart - 1)
	End Sub
	Private Sub IMBnext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBnext.Click
		RaiseEvent OnGotoPage(Pager.GoToNext, Me.PageIndexStart + 1)
	End Sub
	Private Sub IMBlast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBlast.Click
		RaiseEvent OnGotoPage(Pager.GoToPage, Me.PageIndexStart + CInt(sender.commandArgument) - 1)
	End Sub

	Private Sub DDLNumPagine_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumPagine.SelectedIndexChanged
		RaiseEvent OnSetPageSize(Me.DDLNumPagine.SelectedValue)
	End Sub

	Private Sub LNB1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNB1.Click, LNB2.Click, LNB3.Click, LNB4.Click, LNB5.Click, LNB6.Click, LNB7.Click, LNB8.Click, LNB9.Click
		RaiseEvent OnGotoPage(Pager.GoToPage, Me.PageIndexStart + CInt(sender.commandArgument) - 1)
	End Sub
End Class