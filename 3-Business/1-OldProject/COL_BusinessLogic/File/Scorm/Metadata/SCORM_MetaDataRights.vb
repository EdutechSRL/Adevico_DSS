Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_MetaDataRights

#Region "Private Property"
		Private _CopyrightAndOtherRestrictions As SCORM_sourceValue
		Private _Cost As SCORM_SourceValue
		Private _Description As List(Of SCORM_LangString)
		Private _isValid As Boolean
#End Region

#Region "Public Property"
		Public Property Cost() As SCORM_SourceValue
			Get
				Return _Cost
			End Get
			Set(ByVal value As SCORM_SourceValue)
				_Cost = value
			End Set
		End Property
		Public Property CopyrightAndOtherRestrictions() As SCORM_SourceValue
			Get
				Return _CopyrightAndOtherRestrictions
			End Get
			Set(ByVal value As SCORM_SourceValue)
				_CopyrightAndOtherRestrictions = value
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
		Public ReadOnly Property isValid() As Boolean
			Get
				Return _isValid
			End Get
		End Property
#End Region

		Sub New()
			Me._isValid = True
		End Sub
	End Class
End Namespace