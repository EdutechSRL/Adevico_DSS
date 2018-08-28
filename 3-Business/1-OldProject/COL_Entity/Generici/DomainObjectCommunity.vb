<Serializable(), CLSCompliant(True)> Public Class DomainObjectCommunity
	Inherits DomainObject

	Private _Community As Community
	Public Property CommunityOwner() As Community
		Get
			Return _Community
		End Get
		Set(ByVal value As Community)
			_Community = value
		End Set
	End Property

	Sub New()
		MyBase.New()
		MyBase.IsDeleted = False
	End Sub
End Class