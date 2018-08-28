<Serializable(), CLSCompliant(True)> Public Class GenericClause(Of T)
	Private _OperatorClause As OperatorType
	Private _Clause As T
	Private _NextClause As GenericClause(Of T)

	Public Property OperatorForNextClause() As OperatorType
		Get
			If _NextClause Is Nothing Then
				Return OperatorType.None
			Else
				OperatorForNextClause = _OperatorClause
			End If
		End Get
		Set(ByVal value As OperatorType)
			_OperatorClause = value
		End Set
	End Property
	Public Property Clause() As T
		Get
			Clause = _Clause
		End Get
		Set(ByVal value As T)
			_Clause = value
		End Set
	End Property
	Public Property NextClause() As GenericClause(Of T)
		Get
			NextClause = _NextClause
		End Get
		Set(ByVal value As GenericClause(Of T))
			_NextClause = value
		End Set
	End Property
	Sub New()

	End Sub
	Sub New(ByVal pClause As T, Optional ByVal pOperator As OperatorType = OperatorType.AndCondition)
		_OperatorClause = pOperator
		_Clause = pClause
	End Sub

End Class
