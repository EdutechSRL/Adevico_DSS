Public Partial Class CTRLgridTop
	Inherits PagerBaseControl

	Private _LetterSelected As Main.FiltroComunita

	Public Property QuickSearchElementSelected() As QuickSearchElement
		Get
			Dim oElement As QuickSearchElement

			Try
				oElement = New QuickSearchElement(Me.DDLsearch.SelectedItem.Text, Me.DDLsearch.SelectedItem.Value, Me.TXBcercaveloce.Text)
			Catch ex As Exception
				oElement = Nothing
			End Try
			QuickSearchElementSelected = oElement
		End Get
		Set(ByVal value As QuickSearchElement)
			Try
				Me.DDLsearch.SelectedValue = value.Value
			Catch ex As Exception

			End Try
			Me.TXBcercaveloce.Text = value.TextValue
		End Set
	End Property
	Public Property QuickSearchElements() As GenericCollection(Of QuickSearchElement)
		Get
			Dim oElements As New GenericCollection(Of QuickSearchElement)
			For Each oListitem As ListItem In Me.DDLsearch.Items
				oElements.Add(New QuickSearchElement(oListitem.Text, oListitem.Value, , oListitem.Selected))
			Next
			QuickSearchElements = oElements
		End Get
		Set(ByVal value As GenericCollection(Of QuickSearchElement))
			Me.DDLsearch.Items.Clear()
			For Each oElement As QuickSearchElement In value
				Dim oListitem As New ListItem(oElement.Text, oElement.Value)

				If oElement.isDefault Then
					If Me.DDLsearch.SelectedIndex <> -1 Then
						Me.DDLsearch.SelectedIndex = -1
					End If
					oListitem.Selected = True
				End If
				Me.DDLsearch.Items.Add(oListitem)
			Next
		End Set
	End Property

	Public ReadOnly Property LetterSelected() As Main.FiltroComunita
		Get
			LetterSelected = _LetterSelected
		End Get
	End Property



	Public Event OnSetPageSize(ByVal size As Integer)
	Public Event OnGotoPage(ByVal oPage As Pager, ByVal Index As Integer)
	Public Event ShowByLetter(ByVal searchBy As Main.FiltroComunita)
	Public Event FindByValue(ByVal Valure As String, ByVal FindBy As QuickSearchElement)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


	Public Overrides Sub UpdatePageCounter(ByVal pageIndex As Integer, ByVal pageCount As Integer, ByVal RecordForPage As Integer, ByVal RecordCount As Integer)
		MyBase.UpdatePageCounter(pageIndex, pageCount)
		Me.SetPageRecord(pageIndex, pageCount, RecordForPage, RecordCount)
	End Sub

	Private Sub SetPageRecord(ByVal pageIndex As Integer, ByVal pageCount As Integer, ByVal RecordForPage As Integer, ByVal RecordCount As Integer)
		Dim StartRecord As Integer = Me.PageIndexEnd - 1 + pageIndex
		Dim EndRecord As Integer

		If pageCount = 1 Then
			EndRecord = RecordCount
		ElseIf StartRecord + RecordForPage > RecordCount Then
			EndRecord = RecordCount
		Else
			EndRecord = StartRecord + RecordForPage - 1
		End If
		If pageCount = 0 Then
			Me.LBpageRecord.Text = "0"
		Else
			Me.LBpageRecord.Text = String.Format("{0} - {1} di {2}", StartRecord, EndRecord, RecordCount)
		End If

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
					If Index + Me.PageIndexStart = pageIndex Then : oLinkButton.CssClass = "selected"
					Else : oLinkButton.CssClass = ""
					End If

					If Index + Me.PageIndexStart <= Me.PageIndexStart Then : oLinkButton.Visible = True
					Else : oLinkButton.Visible = False
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

	Private Sub LNB1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNB1.Click, LNB2.Click, LNB3.Click, LNB4.Click, LNB5.Click, LNB6.Click, LNB7.Click, LNB8.Click, LNB9.Click
		RaiseEvent OnGotoPage(Pager.GoToPage, Me.PageIndexStart + CInt(sender.commandArgument) - 1)
	End Sub


	Private Sub FiltroLettera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBa.Click, LNBb.Click, LNBc.Click, LNBd.Click, LNBe.Click, LNBf.Click, LNBg.Click, LNBh.Click, LNBi.Click, LNBj.Click, LNBk.Click, LNBl.Click, LNBm.Click, LNBn.Click, LNBo.Click, LNBp.Click, LNBq.Click, LNBr.Click, LNBs.Click, LNBt.Click, LNBtutti.Click, LNBu.Click, LNBv.Click, LNBw.Click, LNBx.Click, LNBy.Click, LNBz.Click
		DeselezionaLettere()
		sender.CssClass = "selected"
		_LetterSelected = CType(sender.commandArgument, Main.FiltroComunita)
		RaiseEvent ShowByLetter(CType(sender.commandArgument, Main.FiltroComunita))
	End Sub

	Private Sub DeselezionaLettere()
		LNBa.CssClass = "lettera" : LNBb.CssClass = "lettera" : LNBc.CssClass = "lettera" : LNBd.CssClass = "lettera"
		LNBe.CssClass = "lettera" : LNBf.CssClass = "lettera" : LNBg.CssClass = "lettera" : LNBh.CssClass = "lettera"
		LNBi.CssClass = "lettera" : LNBj.CssClass = "lettera" : LNBk.CssClass = "lettera" : LNBl.CssClass = "lettera"
		LNBm.CssClass = "lettera" : LNBn.CssClass = "lettera" : LNBo.CssClass = "lettera" : LNBp.CssClass = "lettera"
		LNBq.CssClass = "lettera" : LNBr.CssClass = "lettera" : LNBs.CssClass = "lettera" : LNBt.CssClass = "lettera"
		LNBtutti.CssClass = "lettera" : LNBu.CssClass = "lettera" : LNBv.CssClass = "lettera" : LNBw.CssClass = "lettera"
		LNBx.CssClass = "lettera" : LNBy.CssClass = "lettera" : LNBz.CssClass = "lettera"
	End Sub

	Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
		RaiseEvent FindByValue(Me.TXBcercaveloce.Text, New QuickSearchElement(Me.DDLsearch.SelectedValue, Me.DDLsearch.SelectedItem.Text))
	End Sub
End Class