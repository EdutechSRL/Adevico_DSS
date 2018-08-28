<Serializable(), CLSCompliant(True)> Public Class ServiceClause
	Private _PermissionOperator As OperatorType
	Private _Service As ServiceBase

	Public Property PermissionOperator() As OperatorType
		Get
			PermissionOperator = _PermissionOperator
		End Get
		Set(ByVal value As OperatorType)
			_PermissionOperator = value
		End Set
	End Property
	Public Property Service() As ServiceBase
		Get
			Service = _Service
		End Get
		Set(ByVal value As ServiceBase)
			_Service = value
		End Set
	End Property

	Sub New()

	End Sub
	Sub New(ByVal pService As ServiceBase, Optional ByVal pClause As OperatorType = OperatorType.OrCondition)
		_PermissionOperator = pClause
		_Service = pService
	End Sub
End Class