Public MustInherit Class BaseControlAvanzato
    Inherits BaseControlSession
	Implements IviewAdvancedUserControl

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsSessioneScaduta() Then
			Me.SetInternazionalizzazione()

			If HasPermessi() Then
				Me.BindDati()
			Else
				Me.BindNoPermessi()
			End If
		End If
	End Sub

	Public MustOverride Function HasPermessi() As Boolean Implements IviewAdvancedUserControl.HasPermessi
	Public MustOverride Sub BindNoPermessi() Implements IviewAdvancedUserControl.BindNoPermessi

End Class