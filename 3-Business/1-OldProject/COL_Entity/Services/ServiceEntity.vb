<Serializable(), CLSCompliant(True)> Public Class ServiceEntity(Of T)
	Private _Service As ServiceBase
	Private _Entity As T

	Public Property Service() As ServiceBase
		Get
			Service = _service
		End Get
		Set(ByVal value As ServiceBase)
			_service = value
		End Set
	End Property
	Public Property Entity() As T
		Get
			Entity = _Entity
		End Get
		Set(ByVal value As T)
			_Entity = value
		End Set
	End Property

	Sub New()

	End Sub
	Sub New(ByVal oService As ServiceBase, ByVal oEntity As T)
		_Service = oService
		_Entity = oEntity
	End Sub
End Class