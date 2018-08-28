Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class NotificationMessage

#Region "Private properties"
		Private _Message As String
		Private _Subject As String
		Private _MessageType As NotificationType
#End Region

#Region "Public properties"
		Public Property Message() As String
			Get
				Message = _Message
			End Get
			Set(ByVal value As String)
				_Message = value
			End Set
		End Property
		Public Property Subject() As String
			Get
				Subject = _Subject
			End Get
			Set(ByVal value As String)
				_Subject = value
			End Set
		End Property
		Public ReadOnly Property MessageType() As NotificationType
			Get
				MessageType = _MessageType
			End Get
		End Property

		Enum NotificationType
			Hour0
			Hour12
			Hour24
            'AreaSubscription
            'AreaAcceptSupscription
            'AreaDenySupscription
			ComunitySubscription
			CommunityAcceptSupscription
			CommunityDenySupscription
            'NewTopicArea
            'NewPostArea
			NewTopicComunita
			NewPostComunita
			ConfermaIscrizionePortale
            'Login
            'LoginLDAP
		End Enum

#End Region

		Sub New(ByVal oType As NotificationType)
			_MessageType = oType
		End Sub
	End Class
End Namespace