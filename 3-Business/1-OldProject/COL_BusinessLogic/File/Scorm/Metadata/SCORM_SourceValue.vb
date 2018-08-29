Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_SourceValue

#Region "Private Property"
		Private _Value As SCORM_LangString
		Private _Source As SCORM_LangString
#End Region

#Region "Public Property"
		Public ReadOnly Property Source() As SCORM_LangString
			Get
				Return _Source
			End Get
		End Property
		Public ReadOnly Property Value() As SCORM_LangString
			Get
				Return _Value
			End Get
		End Property
#End Region

		Sub New(ByVal oSource As SCORM_LangString, ByVal oValue As SCORM_LangString)
			Me._Value = oValue
			Me._Source = oSource
		End Sub

	End Class
End Namespace