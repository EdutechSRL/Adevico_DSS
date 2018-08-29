Imports COL_BusinessLogic_v2.Comol.Materiale.Scorm.Metadata
Namespace Comol.Materiale.Scorm
	Public Class SCORM_MetaData

#Region "Private Property"
		Private _Schema As String
		Private _SchemaVersion As String
		Private _General As SCORM_MetaDataGeneral
		Private _Lifecycle As SCORM_MetaDataLifeCycle
		Private _Technical As SCORM_MetaDataTechnical
		Private _Rights As SCORM_MetaDataRights
		Private _Classification As SCORM_MetaDataClassification
#End Region

#Region "Public Property"
		Public Property Schema() As String
			Get
				Return _Schema
			End Get
			Set(ByVal value As String)
				_Schema = value
			End Set
		End Property
		Public Property SchemaVersion() As String
			Get
				Return _SchemaVersion
			End Get
			Set(ByVal value As String)
				_SchemaVersion = value
			End Set
		End Property
		Public Property General() As SCORM_MetaDataGeneral
			Get
				Return _General
			End Get
			Set(ByVal value As SCORM_MetaDataGeneral)
				_General = value
			End Set
		End Property
		Public Property Lifecycle() As SCORM_MetaDataLifeCycle
			Get
				Return _Lifecycle
			End Get
			Set(ByVal value As SCORM_MetaDataLifeCycle)
				_Lifecycle = value
			End Set
		End Property
		Public Property Technical() As SCORM_MetaDataTechnical
			Get
				Return _Technical
			End Get
			Set(ByVal value As SCORM_MetaDataTechnical)
				_Technical = value
			End Set
		End Property
		Public Property Rights() As SCORM_MetaDataRights
			Get
				Return _Rights
			End Get
			Set(ByVal value As SCORM_MetaDataRights)
				_Rights = value
			End Set
		End Property
		Public Property Classification() As SCORM_MetaDataClassification
			Get
				Return _Classification
			End Get
			Set(ByVal value As SCORM_MetaDataClassification)
				_Classification = value
			End Set
		End Property
#End Region

		Public Sub New()
			_Schema = ""
			_SchemaVersion = ""
			_General = New SCORM_MetaDataGeneral
			_Lifecycle = New SCORM_MetaDataLifeCycle
			_Technical = New SCORM_MetaDataTechnical
			_Rights = New SCORM_MetaDataRights
			_Classification = New SCORM_MetaDataClassification
		End Sub

	End Class
End Namespace
