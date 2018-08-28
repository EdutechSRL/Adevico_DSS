Imports Comol.Entity.File
Imports Comol.Entity.Configuration.Components
Imports System.Net.Mail

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class MailSettings

#Region "Private properties"
		Private _SendMailError As Boolean
		Private _SendMailByReply As Boolean
		Private _SMTPServer As String
		Private _SaveMail As Boolean
		Private _SendMailErrorTo As MailAddress
		Private _SystemSender As MailAddress
		Private _ErrorSystemSender As MailAddress
        Private _RealMailSenderAccount As MailAddress
        Private _AuthenticationEnabled As Boolean
        Private _UseSsl As Boolean
        Private _HostPort As Integer
        Private _CredentialsUsername As String
        Private _CredentialsPassword As String
#End Region

#Region "Public properties"
		Public Property isErrorSendingActivated() As Boolean
			Get
				isErrorSendingActivated = _SendMailError
			End Get
			Set(ByVal value As Boolean)
				_SendMailError = value
			End Set
		End Property
		Public Property ServerSMTP() As String
			Get
				ServerSMTP = _SMTPServer
			End Get
			Set(ByVal value As String)
				_SMTPServer = value
			End Set
		End Property
		Public Property isSaveActivated() As Boolean
			Get
				Return _saveMail
			End Get
			Set(ByVal value As Boolean)
				_saveMail = value
			End Set
		End Property
		Public Property SendMailByReply() As Boolean
			Get
				SendMailByReply = _SendMailByReply
			End Get
			Set(ByVal value As Boolean)
				_SendMailByReply = value
			End Set
		End Property
		Public Property SendMailErrorTo() As MailAddress
			Get
				Return _SendMailErrorTo
			End Get
			Set(ByVal value As MailAddress)
				_SendMailErrorTo = value
			End Set
		End Property
		Public Property SystemSender() As MailAddress
			Get
				Return _SystemSender
			End Get
			Set(ByVal value As MailAddress)
				_SystemSender = value
			End Set
		End Property
		Public Property ErrorSystemSender() As MailAddress
			Get
				Return _ErrorSystemSender
			End Get
			Set(ByVal value As MailAddress)
				_ErrorSystemSender = value
			End Set
		End Property
		Public Property RealMailSenderAccount() As MailAddress
			Get
				Return _RealMailSenderAccount
			End Get
			Set(ByVal value As MailAddress)
				_RealMailSenderAccount = value
			End Set
        End Property
        Public Property AuthenticationEnabled() As Boolean
            Get
                Return _AuthenticationEnabled
            End Get
            Set(ByVal value As Boolean)
                _AuthenticationEnabled = value
            End Set
        End Property
        Public Property UseSsl() As Boolean
            Get
                Return _UseSsl
            End Get
            Set(ByVal value As Boolean)
                _UseSsl = value
            End Set
        End Property
        Public Property HostPort() As Integer
            Get
                Return _HostPort
            End Get
            Set(ByVal value As Integer)
                _HostPort = value
            End Set
        End Property
        Public Property CredentialsUsername() As String
            Get
                CredentialsUsername = _CredentialsUsername
            End Get
            Set(ByVal value As String)
                _CredentialsUsername = value
            End Set
        End Property
        Public Property CredentialsPassword() As String
            Get
                CredentialsPassword = _CredentialsPassword
            End Get
            Set(ByVal value As String)
                _CredentialsPassword = value
            End Set
        End Property
#End Region

		Sub New()
			Me._SendMailError = False
			Me._saveMail = False
            Me._SendMailByReply = False
            _AuthenticationEnabled = False
            _UseSsl = False
            _HostPort = 25
        End Sub
	End Class
End Namespace