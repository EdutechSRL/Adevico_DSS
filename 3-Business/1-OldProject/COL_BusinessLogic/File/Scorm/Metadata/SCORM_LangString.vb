Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_LangString

#Region "Private Property"
		Private _Value As String
		Private _Language As String
#End Region

#Region "Public Property"
		Public Property Value() As String
			Get
				Return _Value
			End Get
			Set(ByVal value As String)
				_Value = value
			End Set
		End Property
		Public Property Language() As String
			Get
				Return _Language
			End Get
			Set(ByVal value As String)
				_Language = value
			End Set
		End Property
#End Region

		Public Sub New()

		End Sub
		Public Sub New(ByVal value As String, ByVal Language As String)
			Me._Language = Language
			Me._Value = value
		End Sub

	End Class
End Namespace