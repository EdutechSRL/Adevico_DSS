Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class ActionsSettings

#Region "Private properties"
		Private _Enabled As Boolean
		Private _EnableWebPresence As Boolean
		Private _EnableBrowser As Boolean
		Private _EnableAction As Boolean
#End Region

#Region "Public properties"
		Public Property Enabled() As Boolean
			Get
				Enabled = _Enabled
			End Get
			Set(ByVal value As Boolean)
				_Enabled = value
			End Set
		End Property
		Public Property EnableWebPresence() As Boolean
			Get
				EnableWebPresence = _EnableWebPresence
			End Get
			Set(ByVal value As Boolean)
				_EnableWebPresence = value
			End Set
		End Property
		Public Property EnableBrowser() As Boolean
			Get
				EnableBrowser = _EnableBrowser
			End Get
			Set(ByVal value As Boolean)
				_EnableBrowser = value
			End Set
		End Property
		Public Property EnableAction() As Boolean
			Get
				EnableAction = _EnableAction
			End Get
			Set(ByVal value As Boolean)
				_EnableAction = value
			End Set
		End Property
#End Region

		Public Sub New()
			_Enabled = False
			_EnableWebPresence = False
			_EnableBrowser = False
			_EnableAction = False
		End Sub

	End Class
End Namespace