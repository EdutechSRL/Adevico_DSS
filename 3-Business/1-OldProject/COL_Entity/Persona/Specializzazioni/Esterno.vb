<Serializable(), CLSCompliant(True)> Public Class Esterno
	Inherits Person

	Private _AccessReason As String

	Public Overrides Property OtherInfo() As String
		Get
			Return _AccessReason
		End Get
		Set(ByVal value As String)
			MyBase.OtherInfo = value
		End Set
	End Property
	Public Property AccessReason() As String
		Get
			Return _AccessReason
		End Get
		Set(ByVal value As String)
			_AccessReason = value
		End Set
	End Property

	
	Sub New()
		MyBase.New()
	End Sub

End Class