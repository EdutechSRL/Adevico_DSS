Public Interface IPagingPresenter
	Sub ShowCurrentPage()
	Sub ShowLastPage()
	Sub ShowFirstPage()
	Sub ShowPreviousPage()
	Sub ShowNextPage()
	Sub ChangeRecordnumber(ByVal Recordnumber As Integer)
	Sub GoToPage(ByVal IndexPage As Integer)


	' VERIFICARE SE CE VANNO !!!
	Sub QuickSearch(ByVal elemento As String, ByVal Indice As Integer)
	Sub ShowByLetter(ByVal Lettera As Main.FiltroComunita)
	Sub SetQuickSearchElement()
	Sub Applyfilters()
	Sub Applyfilter(ByVal FilterIdentifier As Integer, Optional ByVal filter As FilterElement = Nothing)
	Sub RemoveFilter(ByVal FilterIdentifier As Integer)
End Interface