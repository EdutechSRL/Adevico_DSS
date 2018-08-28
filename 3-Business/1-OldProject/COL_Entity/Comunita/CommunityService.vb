Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints


<Serializable(), CLSCompliant(True)> Public Class CommunityService
	Inherits DomainObject

	Private _CommunityID As Integer
	Private _Service As ServiceBase

	Public Property CommunityID() As Integer
		Get
			Return _CommunityID
		End Get
		Set(ByVal value As Integer)
			_CommunityID = value
		End Set
	End Property
	Public Property Service() As ServiceBase
		Get
			Return _Service
		End Get
		Set(ByVal value As ServiceBase)
			_Service = value
		End Set
	End Property

	Sub New()

	End Sub

	Public Shared Function FindByCommunity(ByVal item As CommunityService, ByVal argument As Integer) As Boolean
		Return item.CommunityID = argument
	End Function
	Public Shared Function FindByServiceCode(ByVal item As CommunityService, ByVal argument As String) As Boolean
		Return item.Service.Code = argument
	End Function

End Class