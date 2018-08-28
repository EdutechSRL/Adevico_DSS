<Serializable(), CLSCompliant(True)> Public Class Translation(Of T)
	Inherits DomainObject

	Private _Object As T
	Private _Language As Lingua

	Public Property ObjectLanguage() As Lingua
		Get
			ObjectLanguage = _Language
		End Get
		Set(ByVal value As Lingua)
			_Language = value
		End Set
	End Property
	Public Property ObjectTranslated() As T
		Get
			ObjectTranslated = _Object
		End Get
		Set(ByVal value As T)
			_Object = value
		End Set
	End Property
	Sub New(ByVal iLanguage As Lingua, ByVal iObject As T)
		Me._Language = iLanguage
		Me._Object = iObject
	End Sub

End Class