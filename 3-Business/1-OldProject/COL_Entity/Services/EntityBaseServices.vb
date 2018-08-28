<Serializable(), CLSCompliant(True)> Public Class EntityBaseServices(Of T)
	Private _Service As New List(Of ServiceBase)
	Private _Entity As T

	Public Property Services() As List(Of ServiceBase)
		Get
			Services = _Service
		End Get
		Set(ByVal value As List(Of ServiceBase))
			_Service = value
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
	Sub New(ByVal oEntity As T, ByVal oService As ServiceBase)
		_Service.Add(oService)
		_Entity = oEntity
	End Sub
	Sub New(ByVal oEntity As T, ByVal oServices As List(Of ServiceBase))
		_Service = oServices
		_Entity = oEntity
	End Sub
End Class