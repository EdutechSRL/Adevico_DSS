Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_Identifier

#Region "Private Property"
		Private _Entry As SCORM_LangString
		Private _Catalog As String
#End Region

#Region "Public Property"
		Public ReadOnly Property Entry() As SCORM_LangString
			Get
				Return _Entry
			End Get
		End Property
		Public ReadOnly Property Catalog() As String
			Get
				Return _Catalog
			End Get
		End Property
#End Region

		Sub New()

		End Sub
		Sub New(ByVal iCatalog As String, ByVal oEntry As SCORM_LangString)
			Me._Catalog = iCatalog
			Me._Entry = oEntry
		End Sub

	End Class
End Namespace