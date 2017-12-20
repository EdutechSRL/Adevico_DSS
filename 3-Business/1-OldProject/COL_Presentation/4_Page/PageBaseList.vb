Public MustInherit Class PageBaseList
	Inherits PageBase
	Implements IviewListGeneric



	Public Property ItemsCount() As Integer Implements IviewListGeneric.ItemsCount
		Get
			Try
				Return DirectCast(Me.ViewState("ItemsCount"), Integer)
			Catch ex As Exception
				Return 0
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("ItemsCount") = value
		End Set
	End Property

	Public Property ItemsForPage() As Integer Implements IviewListGeneric.ItemsForPage
		Get
			Try
				Return DirectCast(Me.ViewState("ItemsForPage"), Integer)
			Catch ex As Exception
				Return 10
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("ItemsForPage") = value
		End Set
	End Property

	Public Property PageCount() As Integer Implements IviewListGeneric.PageCount
		Get
			Try
				Return DirectCast(Me.ViewState("PageCount"), Integer)
			Catch ex As Exception
				Return 1
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("PageCount") = value
		End Set
	End Property

	Public Property PageIndex() As Integer Implements IviewListGeneric.PageIndex
		Get
			Try
				Return DirectCast(Me.ViewState("PageIndex"), Integer)
			Catch ex As Exception
				Return 1
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("PageIndex") = value
		End Set
	End Property

	'Public Sub RefreshPageCounter(ByVal pageIndex As Integer, ByVal pageCount As Integer, ByVal RecordForPage As Integer, ByVal RecordCount As Integer) Implements IviewListGeneric.RefreshPageCounter

	'End Sub

	Public Sub SetLista(ByVal items As System.Collections.IList) Implements IviewListGeneric.SetLista

	End Sub


	'Public MustOverride Sub UpdatedFilter(ByVal Filter As Integer) Implements IviewListGeneric.UpdatedFilter

End Class
