Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Partial Public Class UC_WorkBookType
	Inherits BaseControlWithLoad

	Public Event TypeSelected(ByVal oType As IviewWorkBookAdd.viewWorkBookType)

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Me.LNBpersonalShared.CommandArgument = IviewWorkBookAdd.viewWorkBookType.PersonalShared
		Me.LNBcommunity.CommandArgument = IviewWorkBookAdd.viewWorkBookType.Community
		Me.LNBcommunityShared.CommandArgument = IviewWorkBookAdd.viewWorkBookType.CommunityShared
		Me.LNBpersonal.CommandArgument = IviewWorkBookAdd.viewWorkBookType.Personal
		Me.LNBpersonalCommunity.CommandArgument = IviewWorkBookAdd.viewWorkBookType.PersonalCommunity
		Me.LNBcommunityOther.CommandArgument = IviewWorkBookAdd.viewWorkBookType.OtherUser
	End Sub

	Public Sub DefineSelectableTypes(ByVal oList As List(Of IviewWorkBookAdd.viewWorkBookType))
		Me.DIVcommunity.Visible = oList.Contains(IviewWorkBookAdd.viewWorkBookType.Community)
		Me.DIVcommunityShared.Visible = oList.Contains(IviewWorkBookAdd.viewWorkBookType.CommunityShared)
		Me.DIVpersonal.Visible = oList.Contains(IviewWorkBookAdd.viewWorkBookType.Personal)
		Me.DIVpersonalCommunity.Visible = oList.Contains(IviewWorkBookAdd.viewWorkBookType.PersonalCommunity)
		Me.DIVpersonalShared.Visible = oList.Contains(IviewWorkBookAdd.viewWorkBookType.PersonalShared)
		Me.DIVcommunityOther.Visible = oList.Contains(IviewWorkBookAdd.viewWorkBookType.OtherUser)
		Me.SetInternazionalizzazione()
	End Sub

	Private Sub LNBcommunity_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles LNBcommunity.Command, LNBcommunityShared.Command, LNBpersonal.Command, LNBpersonalCommunity.Command, LNBpersonalShared.Command, LNBcommunityOther.Command
		RaiseEvent TypeSelected(e.CommandArgument)
	End Sub

	Public Overrides Sub BindDati()

	End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_WorkBookAdd", "Generici")
	End Sub
	Public Overrides Sub SetInternazionalizzazione()
		With Me.Resource
			.setLabel(Me.LBcommunity)
			.setLabel(Me.LBcommunityDescription)
			.setLabel(Me.LBcommunityShared)
			.setLabel(Me.LBcommunitySharedDescription)
			.setLabel(Me.LBpersonal)
			.setLabel(Me.LBpersonalCommunity)
			.setLabel(Me.LBpersonalCommunityDescription)
			.setLabel(Me.LBpersonalDescription)
			.setLabel(Me.LBpersonalShared)
			.setLabel(Me.LBpersonalSharedDescription)
			.setLabel(Me.LBcommunityOther)
			.setLabel(Me.LBcommunityOtherDescription)
			.setLinkButton(Me.LNBcommunity, True, True)
			.setLinkButton(Me.LNBcommunityShared, True, True)
			.setLinkButton(Me.LNBpersonal, True, True)
			.setLinkButton(Me.LNBpersonalCommunity, True, True)
			.setLinkButton(Me.LNBpersonalShared, True, True)
			.setLinkButton(Me.LNBcommunityOther, True, True)
		End With
	End Sub

	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
End Class