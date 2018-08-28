Public Class KeyElment(Of T)
	Inherits DomainObject

	Private _Key As T
	Private _Translation As Translation(Of T)

	Public Property Element() As Translation(Of T)
		Get
			Element = _Translation
		End Get
		Set(ByVal value As Translation(Of T))
			_Translation = value
		End Set
	End Property
	Public Property Key() As T
		Get
			Key = _Key
		End Get
		Set(ByVal value As T)
			_Key = value
		End Set
	End Property
	Sub New(ByVal iElement As Translation(Of T), ByVal iKey As T)
		Me._Translation = iElement
		Me._Key = iKey
	End Sub

End Class