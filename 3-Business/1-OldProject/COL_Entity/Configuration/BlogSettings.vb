Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class BlogSettings

#Region "Private properties"
		Private _BlogHome As String
		Private _DomainCookie As String
		Private _ValidationTime As Integer
#End Region

#Region "Public properties"
		Public Property BlogHome() As String
			Get
				BlogHome = _BlogHome
			End Get
			Set(ByVal value As String)
				_BlogHome = value
			End Set
		End Property
		Public Property DomainCookie() As String
			Get
				DomainCookie = _DomainCookie
			End Get
			Set(ByVal value As String)
				_DomainCookie = value
			End Set
		End Property
		Public Property ValidationTime() As String
			Get
				ValidationTime = _ValidationTime
			End Get
			Set(ByVal value As String)
				_ValidationTime = value
			End Set
		End Property
#End Region

		Public Sub New()

		End Sub
	End Class
End Namespace