Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_MetaDataClassification

#Region "Private Property"
		Private _Purpose As SCORM_SourceValue
		Private _TaxonPath As List(Of SCORM_TaxonPath)
		Private _Description As List(Of SCORM_LangString)
		Private _Keywords As List(Of SCORM_Keyword)
		Private _isValid As Boolean
#End Region

#Region "Public Property"
		Public Property Purpose() As SCORM_SourceValue
			Get
				Return _Purpose
			End Get
			Set(ByVal value As SCORM_SourceValue)
				_Purpose = value
			End Set
		End Property
		Public Property Description() As List(Of SCORM_LangString)
			Get
				Return _Description
			End Get
			Set(ByVal value As List(Of SCORM_LangString))
				_Description = value
			End Set
		End Property
		Public Property TaxonPath() As List(Of SCORM_TaxonPath)
			Get
				Return _TaxonPath
			End Get
			Set(ByVal value As List(Of SCORM_TaxonPath))
				_TaxonPath = value
			End Set
		End Property
		Public Property Keywords() As List(Of SCORM_Keyword)
			Get
				Return _Keywords
			End Get
			Set(ByVal value As List(Of SCORM_Keyword))
				_Keywords = value
			End Set
		End Property
		Public ReadOnly Property isValid() As Boolean
			Get
				Return _isValid
			End Get
		End Property
#End Region

		Sub New()
			Me._isValid = True
			_TaxonPath = New List(Of SCORM_TaxonPath)
			_Description = New List(Of SCORM_LangString)
			_Keywords = New List(Of SCORM_Keyword)
		End Sub
	End Class
End Namespace