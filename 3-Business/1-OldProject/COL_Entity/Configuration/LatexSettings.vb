Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class LatexSettings

#Region "Private properties"
		Private _CssStylePath As String
		Private _JavascriptPath As String
		Private _Servers As List(Of ForeignRenderServer)
#End Region

#Region "Public properties"
		Public Property CssStylePath() As String
			Get
				CssStylePath = _CssStylePath
			End Get
			Set(ByVal value As String)
				_CssStylePath = value
			End Set
		End Property
		Public Property JavascriptPath() As String
			Get
				JavascriptPath = _JavascriptPath
			End Get
			Set(ByVal value As String)
				_JavascriptPath = value
			End Set
		End Property
		Public Property Servers() As List(Of ForeignRenderServer)
			Get
				Servers = _Servers
			End Get
			Set(ByVal value As List(Of ForeignRenderServer))
				_Servers = value
			End Set
		End Property
		Public ReadOnly Property IsEnabled() As Boolean
			Get
				If _Servers.Count = 0 Then
					Return False
				Else
					Return ((From o In _Servers Select o Where o.isEnabled).Count > 0)
				End If
			End Get
		End Property

#End Region

		Public Sub New()
			Me._Servers = New List(Of ForeignRenderServer)
		End Sub

		Public Function FindAvailableServers() As List(Of ForeignRenderServer)
			Return (From o In _Servers Select o Where o.isEnabled).ToList
		End Function
	End Class
End Namespace