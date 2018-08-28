
<Serializable(), CLSCompliant(True)> Public Class SummaryElement

	Private _Total As Int64
	Private _NumberOfIstance As Int64
	Private _Element As Object

	Public Property NumberOfIstance() As Int64
		Get
			Return _NumberOfIstance
		End Get
		Set(ByVal value As Int64)
			_NumberOfIstance = value
		End Set
	End Property
	Public Property Total() As Int64
		Get
			Return _Total
		End Get
		Set(ByVal value As Int64)
			_Total = value
		End Set
	End Property
	Public Property Element() As Object
		Get
			Return _Element
		End Get
		Set(ByVal value As Object)
			_Element = value
		End Set
	End Property

	Sub New()

	End Sub
	Sub New(ByVal iObject As Object, ByVal iNumberOfIstance As Int64, ByVal iTotal As Int64)
		Me._Element = iObject
		Me._NumberOfIstance = iNumberOfIstance
		Me._Total = iTotal
	End Sub

End Class