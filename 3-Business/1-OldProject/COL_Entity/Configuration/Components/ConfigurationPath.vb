Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class ConfigurationPath

#Region "Private properties"
		Private _VirtualPath As String
		Private _DrivePath As String
		Private _SameServer As Boolean
		Private _ServerPath As String
		Private _ServerVirtualPath As String
		Private _RewritePath As String
		Private _MaxSize As Integer
		Private _MaxUploadSize As Integer
		Private _isLazy As Boolean
        Private _isInvalid As Boolean
#End Region

#Region "Public properties"
		Public Property VirtualPath() As String
			Get
				VirtualPath = _VirtualPath
			End Get
			Set(ByVal value As String)
				_VirtualPath = value
			End Set
		End Property
		Public Property DrivePath() As String
			Get
				DrivePath = _DrivePath
			End Get
			Set(ByVal value As String)
				_DrivePath = value
			End Set
        End Property
        Public Property isOnThisServer() As Boolean
            Get
                isOnThisServer = _SameServer
            End Get
            Set(ByVal value As Boolean)
                _SameServer = value
            End Set
        End Property
        Public Property ServerPath() As String
            Get
                ServerPath = _ServerPath
            End Get
            Set(ByVal value As String)
                _ServerPath = value
            End Set
        End Property
		Public Property ServerVirtualPath() As String
			Get
				ServerVirtualPath = _ServerVirtualPath
			End Get
			Set(ByVal value As String)
				_ServerVirtualPath = value
			End Set
		End Property
		Public Property RewritePath() As String
			Get
				RewritePath = _RewritePath
			End Get
			Set(ByVal value As String)
				_RewritePath = value
			End Set
		End Property
		Public Property isInvalid() As Boolean
			Get
				isInvalid = _isInvalid
			End Get
			Set(ByVal value As Boolean)
				_isInvalid = value
			End Set
		End Property
		Public Property MaxSize() As Integer
			Get
				MaxSize = _MaxSize
			End Get
			Set(ByVal value As Integer)
				_MaxSize = value
			End Set
		End Property
		Public Property MaxUploadSize() As Integer
			Get
				MaxUploadSize = _MaxUploadSize
			End Get
			Set(ByVal value As Integer)
				_MaxUploadSize = value
			End Set
		End Property
#End Region

		Sub New()
			_isInvalid = False
		End Sub
		Public Sub New(ByVal VirtualPath As String, ByVal DrivePath As String, ByVal isSameServer As Boolean, Optional ByVal ServerPath As String = "", Optional ByVal ServerVirtualPath As String = "", Optional ByVal iRevritePath As String = "")
			_VirtualPath = VirtualPath
			_DrivePath = DrivePath
			_SameServer = isSameServer
			_ServerPath = ServerPath
			_ServerVirtualPath = ServerVirtualPath
			_RewritePath = iRevritePath
			_isInvalid = False
			_MaxSize = -1
			_MaxUploadSize = -1
		End Sub
	End Class
End Namespace