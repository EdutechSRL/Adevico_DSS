Imports System.Net.Mail

Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class MailLocalized

#Region "Private properties"
		Private _SendMailErrorTo As MailAddress
		Private _SystemSender As MailAddress
		Private _ErrorSystemSender As MailAddress
		Private _SubjectPrefix As String
		Private _CopySubject As String
		Private _SystemFirma As String
		Private _SystemFirmaNotifica As String
		Private _ErrorSubject As String
		Private _SMTPServer As String
		Private _RealMailSenderAccount As MailAddress
		Private _SendMailByReply As String
		Private _NotificationMessages As Hashtable
        Private _Lingua As Lingua
        Private _AuthenticationEnabled As Boolean
        Private _UseSsl As Boolean
        Private _HostPort As Integer
        Private _CredentialsUsername As String
        Private _CredentialsPassword As String
#End Region

#Region "Public properties"
		Public Property SubjectPrefix() As String
			Get
				SubjectPrefix = _SubjectPrefix
			End Get
			Set(ByVal value As String)
				_SubjectPrefix = value
			End Set
		End Property
		Public Property SystemSender() As MailAddress
			Get
				SystemSender = _SystemSender
			End Get
			Set(ByVal value As MailAddress)
				_SystemSender = value
			End Set
		End Property
		Public Property ErrorSender() As MailAddress
			Get
				ErrorSender = _ErrorSystemSender
			End Get
			Set(ByVal value As MailAddress)
				_ErrorSystemSender = value
			End Set
		End Property
		Public Property SendErrorTo() As MailAddress
			Get
				SendErrorTo = _SendMailErrorTo
			End Get
			Set(ByVal value As MailAddress)
				_SendMailErrorTo = value
			End Set
		End Property
		Public Property SubjectForSenderCopy() As String
			Get
				SubjectForSenderCopy = _CopySubject
			End Get
			Set(ByVal value As String)
				_CopySubject = value
			End Set
		End Property
		Public Property SystemFirma() As String
			Get
				SystemFirma = _SystemFirma
			End Get
			Set(ByVal value As String)
				_SystemFirma = value
			End Set
		End Property
		Public Property SystemFirmaNotifica() As String
			Get
				SystemFirmaNotifica = _SystemFirmaNotifica
			End Get
			Set(ByVal value As String)
				_SystemFirmaNotifica = value
			End Set
		End Property
		Public Property ErrorSubject() As String
			Get
				ErrorSubject = _ErrorSubject
			End Get
			Set(ByVal value As String)
				_ErrorSubject = value
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
		Public Property RealMailSenderAccount() As MailAddress
			Get
				RealMailSenderAccount = _RealMailSenderAccount
			End Get
			Set(ByVal value As MailAddress)
				_RealMailSenderAccount = value
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

		Public ReadOnly Property NotificationMessages(ByVal oType As NotificationMessage.NotificationType) As NotificationMessage
			Get
				Return DirectCast(_NotificationMessages.Item(oType), NotificationMessage)
			End Get
		End Property
		Public Property Language() As Lingua
			Get
				Language = _Lingua
			End Get
			Set(ByVal value As Lingua)
				_Lingua = value
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
			_NotificationMessages = New Hashtable
		End Sub
		Public Sub New(ByVal SenderMail As String, ByVal ErrorSender As String, ByVal ErrorReciver As String, ByVal ServerSMTP As String, ByVal RealSenderAccount As String, ByVal oSendMailByReply As Boolean)
			_NotificationMessages = New Hashtable
			Try
				_SendMailErrorTo = New MailAddress(ErrorReciver, ErrorReciver)
			Catch ex As Exception
				_SendMailErrorTo = Nothing
			End Try
			_SMTPServer = ServerSMTP
			If RealSenderAccount <> "" Then
				_RealMailSenderAccount = New MailAddress(RealSenderAccount)
			Else
				_RealMailSenderAccount = Nothing
			End If
			_SendMailByReply = oSendMailByReply
		End Sub
		Public Sub AddNotification(ByVal oNotificationMessage As NotificationMessage, ByVal oType As NotificationMessage.NotificationType)
			_NotificationMessages.Add(oType, oNotificationMessage)
		End Sub
		Public Function GetNotification(ByVal oType As NotificationMessage.NotificationType) As NotificationMessage
			Return DirectCast(_NotificationMessages.Item(oType), NotificationMessage)
		End Function

		Public Shared Function FindByLanguage(ByVal item As MailLocalized, ByVal argument As Lingua) As Boolean
			Return item.Language.Codice = argument.Codice
		End Function
	End Class
End Namespace