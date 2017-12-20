Public MustInherit Class PagerBaseControl
	Inherits System.Web.UI.UserControl
	Private _PageIndexStart As Integer
	Private _PageIndexEnd As Integer

	Public Enum Pager
		GoToFirst
		GoToPreviuous
		GoToNext
		GoToLast
		GoToPage
	End Enum

	Public Property PageIndexStart() As Integer
		Get
			PageIndexStart = _PageIndexStart
		End Get
		Set(ByVal value As Integer)
			_PageIndexStart = value
		End Set
	End Property
	Public Property PageIndexEnd() As Integer
		Get
			PageIndexEnd = _PageIndexEnd
		End Get
		Set(ByVal value As Integer)
			_PageIndexEnd = value
		End Set
	End Property

	Public Overridable Sub UpdatePageCounter(ByVal pageIndex As Integer, ByVal pageCount As Integer)
		If pageCount <= 9 Then
			Me._PageIndexStart = 1
			Me._PageIndexEnd = pageCount
		Else
			If pageIndex <= 5 Then
				Me._PageIndexStart = 1
			Else
				Me._PageIndexStart = pageIndex - 4
			End If

			If pageIndex = pageCount Then
				Me._PageIndexEnd = pageIndex
			ElseIf pageIndex + 4 < pageCount Then
				Me._PageIndexEnd = pageIndex + 4
			Else
				Me._PageIndexEnd = pageCount
			End If
		End If
		Me.ShowNavigationButton(pageIndex, pageCount)
		Me.ShowPageIndex(pageIndex, pageCount)
	End Sub

	Public Overridable Sub UpdatePageCounter(ByVal pageIndex As Integer, ByVal pageCount As Integer, ByVal RecordForPage As Integer, ByVal RecordCount As Integer)
		Me.UpdatePageCounter(pageIndex, pageCount)
	End Sub
	Protected MustOverride Sub ShowNavigationButton(ByVal pageIndex As Integer, ByVal pageCount As Integer)
	Protected MustOverride Sub ShowPageIndex(ByVal pageIndex As Integer, ByVal pageCount As Integer)
End Class