Public Class GenericPresenter
	Protected view As IViewCommon

	Public Sub New()

	End Sub
	Public Sub New(ByVal view As IviewBase)
		Me.view = view
	End Sub


End Class