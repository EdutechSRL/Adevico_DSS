Public Class ContextModule
	Private _Lingua As Lingua
	Private _ComunitaID As Integer
	Private _PersonaID As Integer

	Public Property Language() As Lingua
		Get
			Return _Lingua
		End Get
		Set(ByVal value As Lingua)
			_Lingua = value
		End Set
	End Property
	Public Property CommunityID() As Integer
		Get
			Return _ComunitaID
		End Get
		Set(ByVal value As Integer)
			_ComunitaID = value
		End Set
	End Property
	Public Property UserID() As Integer
		Get
			Return _PersonaID
		End Get
		Set(ByVal value As Integer)
			_PersonaID = value
		End Set
	End Property
	
	Public Sub New()
		_PersonaID = 0
		_ComunitaID = -1
		_Lingua = Nothing
	End Sub

End Class