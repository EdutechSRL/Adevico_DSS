Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_Keyword

		Private _Value As List(Of SCORM_LangString)


		Public Property Value() As List(Of SCORM_LangString)
			Get
				Return _Value
			End Get
			Set(ByVal value As List(Of SCORM_LangString))
				_Value = value
			End Set
		End Property

		Sub New()
			Me._Value = New List(Of SCORM_LangString)()
		End Sub
		Sub New(ByVal oLista As List(Of SCORM_LangString))
			Me._Value = oLista
		End Sub
	End Class
End Namespace