<CLSCompliant(True)> Public Class PopUpWinComol
	Private _PopUp As EeekSoft.Web.PopupWin


	Public Property ID() As String
		Get
			ID = _PopUp.ID
		End Get
		Set(ByVal value As String)
			_PopUp.ID = value
		End Set
	End Property
	Public Property Title() As String
		Get
			Title = _PopUp.Title
		End Get
		Set(ByVal value As String)
			_PopUp.Title = value
		End Set
	End Property
	Public Property Text() As String
		Get
			Text = _PopUp.Text
		End Get
		Set(ByVal value As String)
			_PopUp.Text = value
		End Set
	End Property
	Public Property AutoShow() As Boolean
		Get
			AutoShow = _PopUp.AutoShow
		End Get
		Set(ByVal value As Boolean)
			_PopUp.AutoShow = value
		End Set
	End Property
	Public Property DragDrop() As Boolean
		Get
			DragDrop = _PopUp.DragDrop
		End Get
		Set(ByVal value As Boolean)
			_PopUp.DragDrop = value
		End Set
	End Property
	Public Property HideAfter() As Integer
		Get
			HideAfter = _PopUp.HideAfter
		End Get
		Set(ByVal value As Integer)
			_PopUp.HideAfter = value
		End Set
	End Property
	Public Property Visible() As Boolean
		Get
			Visible = _PopUp.Visible
		End Get
		Set(ByVal value As Boolean)
			_PopUp.Visible = value
		End Set
	End Property
	Public Property Message() As String
		Get
			Message = _PopUp.Message
		End Get
		Set(ByVal value As String)
			_PopUp.Message = value
		End Set
	End Property
	Public Property OffsetX() As Integer
		Get
			OffsetX = _PopUp.OffsetX
		End Get
		Set(ByVal value As Integer)
			_PopUp.OffsetX = value
		End Set
	End Property
	Public Property Width() As System.Web.UI.WebControls.Unit
		Get
			Width = _PopUp.Width
		End Get
		Set(ByVal value As System.Web.UI.WebControls.Unit)
			_PopUp.Width = value
		End Set
	End Property
	Public Property Height() As System.Web.UI.WebControls.Unit
		Get
			Height = _PopUp.Height
		End Get
		Set(ByVal value As System.Web.UI.WebControls.Unit)
			_PopUp.Height = value
		End Set
	End Property
	Public Property ColorStyle() As PopupColorStyle
		Get
			ColorStyle = _PopUp.ColorStyle
		End Get
		Set(ByVal value As PopupColorStyle)
			_PopUp.ColorStyle = value
		End Set
	End Property
	Public Property ActionType() As PopupAction
		Get
			ActionType = _PopUp.ActionType
		End Get
		Set(ByVal value As PopupAction)
			_PopUp.ActionType = value
		End Set
	End Property
	Public Property Link() As String
		Get
			Link = _PopUp.Link
		End Get
		Set(ByVal value As String)
			_PopUp.Link = value
		End Set
	End Property
	Public Property WebPopUp() As EeekSoft.Web.PopupWin
		Get
			Return _PopUp
		End Get
		Set(ByVal value As EeekSoft.Web.PopupWin)
			_PopUp = value
		End Set
	End Property




	Sub New()
		_PopUp = New PopupWin
	End Sub
	Sub New(ByVal oPopup As PopupWin)
		_PopUp = oPopup
	End Sub

End Class