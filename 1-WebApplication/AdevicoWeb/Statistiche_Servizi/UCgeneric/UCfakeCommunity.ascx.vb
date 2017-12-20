Imports lm.Comol.Core.Statistiche

Partial Public Class UCfakeCommunity
	Inherits System.Web.UI.UserControl
	Implements IcommunityContainer

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Public Sub Bind(ByVal Community_Id As Integer) Implements lm.Comol.Core.Statistiche.IcommunityContainer.Bind

	End Sub

	Public Property CommunityID() As Integer Implements lm.Comol.Core.Statistiche.IcommunityContainer.CommunityID
		Get

		End Get
		Set(ByVal value As Integer)

		End Set
	End Property
End Class