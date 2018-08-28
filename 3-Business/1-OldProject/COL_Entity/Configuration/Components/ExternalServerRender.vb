Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class ForeignRenderServer

#Region "Private Property"
		Private _Name As String
		Private _RemoteUrl As String
		Private _isDefault As Boolean
		Private _isEnabled As Boolean
#End Region

#Region "Public Property"
		Public ReadOnly Property Name() As String
			Get
				Return _Name
			End Get
		End Property
		Public ReadOnly Property RemoteUrl() As String
			Get
				Return _RemoteUrl
			End Get
		End Property
		Public ReadOnly Property isDefault() As Boolean
			Get
				Return _isDefault
			End Get
		End Property
		Public ReadOnly Property isEnabled() As Boolean
			Get
				Return _isEnabled
			End Get
		End Property
#End Region

		Public Sub New(ByVal iName As String, ByVal iRemoteUrl As String, ByVal iIsDefault As Boolean, ByVal iIsEnabled As Boolean)
			Me._Name = iName
			Me._RemoteUrl = iRemoteUrl
			Me._isDefault = iIsDefault
			Me._isEnabled = iIsEnabled
		End Sub
	End Class
End Namespace