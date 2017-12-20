Public Interface IviewListGeneric
	Inherits IviewGeneric

	Property PageIndex() As Integer
	Property PageCount() As Integer
	Property ItemsForPage() As Integer
	Property ItemsCount() As Integer

	Sub SetLista(ByVal items As IList)
	'Sub UpdatedFilter(ByVal Filter As Integer)
	'Sub RefreshPageCounter(ByVal pageIndex As Integer, ByVal pageCount As Integer, ByVal RecordForPage As Integer, ByVal RecordCount As Integer)
End Interface