Public Interface IviewPaging__
	Inherits IviewGeneric

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
End Interface