Public Class Structure_FilePahConfiguration
	Private _ConfigType As ConfigFileType
	Private _Path As String
	Private _Configuration As ConfigurationPath

	Public Property Configuration() As ConfigurationPath
		Get
			Return _Configuration
		End Get
		Set(ByVal value As ConfigurationPath)
			_Configuration = value
		End Set
	End Property
	Public Property Path() As String
		Get
			Return _Path
		End Get
		Set(ByVal value As String)
			_Path = value
		End Set
	End Property
	Public Property ConfigType() As ConfigFileType
		Get
			Return _ConfigType
		End Get
		Set(ByVal value As ConfigFileType)
			_ConfigType = value
		End Set
	End Property

	Sub New(ByVal oType As ConfigFileType, ByVal oPath As String, ByVal oConfiguration As ConfigurationPath)
		_ConfigType = oType
		_Path = oPath
		_Configuration = oConfiguration
	End Sub
End Class
