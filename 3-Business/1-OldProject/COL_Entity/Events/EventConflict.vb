Namespace Events
	<Serializable(), CLSCompliant(True)> Public Class EventConflict
		Private _InternalEvent As CommunityEvent
		Private _ExternalEvent As Object
		Private _Action As EventConflictAction

		Public Property InternalEvent() As CommunityEvent
			Get
				Return _InternalEvent
			End Get
			Set(ByVal value As CommunityEvent)
				_InternalEvent = value
			End Set
		End Property
		Public Property ExternalEvent() As Object
			Get
				Return _ExternalEvent
			End Get
			Set(ByVal value As Object)
				_ExternalEvent = value
			End Set
		End Property
		Public Property Action() As EventConflictAction
			Get
				Return _Action
			End Get
			Set(ByVal value As EventConflictAction)
				_Action = value
			End Set
		End Property
		Sub New()

		End Sub
		Sub New(ByVal iEvent As CommunityEvent)
			_InternalEvent = iEvent
		End Sub
		Sub New(ByVal iEvent As CommunityEvent, ByVal iExternalEvent As Object)
			_InternalEvent = iEvent
			_ExternalEvent = iExternalEvent
		End Sub
		Sub New(ByVal iEvent As CommunityEvent, ByVal iExternalEvent As Object, ByVal iAction As EventConflictAction)
			_InternalEvent = iEvent
			_ExternalEvent = iExternalEvent
			_Action = iAction
		End Sub

		Public Shared Function FindByCommunityEvent(ByVal item As EventConflict, ByVal argument As Integer) As Boolean
			Return item.InternalEvent.ID = argument
		End Function
	End Class

	<Serializable(), CLSCompliant(True)> Public Enum EventConflictAction
		None = 0
		OverWrite = 1
	End Enum
End Namespace