Imports Comol.Entity.File

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class CSVsettings

		Public Enum CSVtype
			KnowledgeTutor = 1
		End Enum

		Private _HashTableTag As Hashtable

#Region "Public properties"
		Public ReadOnly Property KnowledgeTutor() As FileCSV
			Get
				KnowledgeTutor = DirectCast(_HashTableTag.Item(CSVtype.KnowledgeTutor), FileCSV)
			End Get
		End Property
#End Region

		Sub New()
			_HashTableTag = New Hashtable
		End Sub

		Public Sub AddSettings(ByVal oFileCSVsettings As FileCSV, ByVal oCSVtype As CSVtype)
			_HashTableTag.Add(oCSVtype, oFileCSVsettings)
		End Sub
	End Class
End Namespace