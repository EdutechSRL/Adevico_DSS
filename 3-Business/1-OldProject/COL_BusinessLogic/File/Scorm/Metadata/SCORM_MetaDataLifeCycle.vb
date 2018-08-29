Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_MetaDataLifeCycle

#Region "Private Property"
		Private _Version As List(Of SCORM_LangString)
		Private _Status As SCORM_SourceValue
		Private _Contribute As List(Of SCORM_contribute)
#End Region

#Region "Public Property"
		Public Property Version() As List(Of SCORM_LangString)
			Get
				Return _Version
			End Get
			Set(ByVal value As List(Of SCORM_LangString))
				_Version = value
			End Set
		End Property
		Public Property Status() As SCORM_SourceValue
			Get
				Return _Status
			End Get
			Set(ByVal value As SCORM_SourceValue)
				_Status = value
			End Set
		End Property
		Public Property Contributes() As List(Of SCORM_contribute)
			Get
				Return _Contribute
			End Get
			Set(ByVal value As List(Of SCORM_contribute))
				_Contribute = value
			End Set
		End Property
#End Region

		Sub New()
			_Version = New List(Of SCORM_LangString)
			_Contribute = New List(Of SCORM_contribute)
		End Sub
		Sub New(ByVal oVersion As List(Of SCORM_LangString))
			_Version = oVersion
			_Contribute = New List(Of SCORM_contribute)
		End Sub
		Sub New(ByVal oVersion As List(Of SCORM_LangString), ByVal oStatus As SCORM_SourceValue)
			_Version = oVersion
			_Status = oStatus
			_Contribute = New List(Of SCORM_contribute)
		End Sub
		Sub New(ByVal oVersion As List(Of SCORM_LangString), ByVal oStatus As SCORM_SourceValue, ByVal oContributes As List(Of SCORM_contribute))
			_Version = oVersion
			_Status = oStatus
			_Contribute = oContributes
		End Sub

	End Class
End Namespace