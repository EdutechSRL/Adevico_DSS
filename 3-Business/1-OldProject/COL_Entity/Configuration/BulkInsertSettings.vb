Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class BulkInsertSettings
		Public Enum ConfigType
			Questionari = 0
		End Enum

#Region "Private properties"
		Private _BulkInsertList As New Hashtable
#End Region

#Region "Public properties"
		Public ReadOnly Property Questionario() As ConfigurationPath
			Get
				Questionario = DirectCast(_BulkInsertList.Item(ConfigType.Questionari), ConfigurationPath)
			End Get
		End Property
#End Region

		Sub New()
			_BulkInsertList = New Hashtable
		End Sub
		Public Function GetByCode(ByVal oConfig As ConfigType) As ConfigurationPath
			Return DirectCast(_BulkInsertList.Item(oConfig), ConfigurationPath)
		End Function

		Public Sub AddSettings(ByVal oConfigurationPath As ConfigurationPath, ByVal oConfigType As ConfigType)
			_BulkInsertList.Add(oConfigType, oConfigurationPath)
		End Sub
	End Class
End Namespace