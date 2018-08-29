Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_MetaDataGeneral

#Region "Private Property"
		Private _Identifier As SCORM_Identifier
		Private _Title As List(Of SCORM_LangString)
		Private _Description As List(Of SCORM_LangString)
		Private _Language As List(Of String)
		Private _Keywords As List(Of SCORM_Keyword)
		Private _isValid As Boolean
#End Region

#Region "Public Property"
		Public Property Identifier() As SCORM_Identifier
			Get
				Return _Identifier
			End Get
			Set(ByVal value As SCORM_Identifier)
				_Identifier = value
			End Set
		End Property
		Public Property Title() As List(Of SCORM_LangString)
			Get
				Return _Title
			End Get
			Set(ByVal value As List(Of SCORM_LangString))
				_Title = value
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
		Public Property Language() As List(Of String)
			Get
				Return _Language
			End Get
			Set(ByVal value As List(Of String))
				_Language = value
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
			_Identifier = New SCORM_Identifier
			_Title = New List(Of SCORM_LangString)
			_Description = New List(Of SCORM_LangString)
			_Language = New List(Of String)
			_Keywords = New List(Of SCORM_Keyword)
			_isValid = True
		End Sub
	
	End Class
End Namespace