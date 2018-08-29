Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_TaxonPath

#Region "Private Property"
		Private _Source As SCORM_LangString
		Private _Taxon As List(Of SCORM_Taxon)
		Private _isValid As Boolean
#End Region

#Region "Public Property"
		Public Property Source() As SCORM_LangString
			Get
				Source = _Source
			End Get
			Set(ByVal value As SCORM_LangString)
				_Source = value
			End Set
		End Property
		Public Property Taxon() As List(Of SCORM_Taxon)
			Get
				Taxon = _Taxon
			End Get
			Set(ByVal value As List(Of SCORM_Taxon))
				_Taxon = value
			End Set
		End Property
		Public ReadOnly Property isValid() As Boolean
			Get
				Return _isValid
			End Get
		End Property
#End Region

		Sub New()
			Me._Taxon = New List(Of SCORM_Taxon)
			Me._isValid = True
		End Sub
		Sub New(ByVal oSource As SCORM_LangString)
			Me._Source = oSource
			Me._Taxon = New List(Of SCORM_Taxon)
			Me._isValid = True
		End Sub
		Sub New(ByVal oSource As SCORM_LangString, ByVal oLista As List(Of SCORM_Taxon))
			Me._Source = oSource
			Me._Taxon = oLista
			Me._isValid = True
		End Sub
	End Class
End Namespace