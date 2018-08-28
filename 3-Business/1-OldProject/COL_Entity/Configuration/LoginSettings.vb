Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class LoginSettings

#Region "Private properties"
		Private _isSSLrequired As Boolean
		Private _isSSLloginRequired As Boolean
        Private _UpdateXMPprofile As Integer
		Private _LoginControl As String
		Private _DefaultLoginControl As String
		Private _showHelpToSubscription As Boolean
        Private _SubscriptionActive As Boolean
        Private _ProfileAutoActivation As Boolean
        Private _AlwaysDefaultPageForInternal As Boolean
        Private _SendRegistrationMail As Boolean
#End Region

#Region "Public properties"
        Public Property isSSLrequired() As Boolean
            Get
                isSSLrequired = _isSSLrequired
            End Get
            Set(ByVal value As Boolean)
                _isSSLrequired = value
            End Set
        End Property
		Public Property isSSLloginRequired() As Boolean
			Get
				isSSLloginRequired = _isSSLloginRequired
			End Get
			Set(ByVal value As Boolean)
				_isSSLloginRequired = value
			End Set
		End Property
		Public Property DaysToUpdateProfile() As Integer
			Get
				DaysToUpdateProfile = _UpdateXMPprofile
			End Get
			Set(ByVal value As Integer)
				_UpdateXMPprofile = value
			End Set
		End Property
        Public Property showHelpToSubscription() As Boolean
            Get
                showHelpToSubscription = _showHelpToSubscription
            End Get
            Set(ByVal value As Boolean)
                _showHelpToSubscription = value
            End Set
        End Property
		Public Property SubscriptionActive() As Boolean
			Get
				SubscriptionActive = _SubscriptionActive
			End Get
			Set(ByVal value As Boolean)
				_SubscriptionActive = value
			End Set
		End Property
        Public Property ProfileAutoActivation() As Boolean
            Get
                ProfileAutoActivation = _ProfileAutoActivation
            End Get
            Set(ByVal value As Boolean)
                _ProfileAutoActivation = value
            End Set
        End Property
        Public Property AlwaysDefaultPageForInternal() As Boolean
            Get
                AlwaysDefaultPageForInternal = _AlwaysDefaultPageForInternal
            End Get
            Set(ByVal value As Boolean)
                _AlwaysDefaultPageForInternal = value
            End Set
        End Property

        Public Property SendRegistrationMail() As Boolean
            Get
                SendRegistrationMail = _SendRegistrationMail
            End Get
            Set(ByVal value As Boolean)
                _SendRegistrationMail = value
            End Set
        End Property
#End Region

		Public Sub New()
			_UpdateXMPprofile = 4
			_isSSLrequired = False
			_isSSLloginRequired = False
            _LoginControl = ""
			_DefaultLoginControl = ""
			_SubscriptionActive = True
            _showHelpToSubscription = False
            _ProfileAutoActivation = False
            _SendRegistrationMail = True
		End Sub
    End Class


End Namespace