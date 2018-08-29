Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_Taxon

#Region "Private Property"
		Private _ID As String
		Private _Entry As List(Of SCORM_LangString)
#End Region

#Region "Public Property"
		Public Property ID() As String
			Get
				ID = _ID
			End Get
			Set(ByVal value As String)
				_ID = value
			End Set
		End Property
		Public Property Entry() As List(Of SCORM_LangString)
			Get
				Entry = _Entry
			End Get
			Set(ByVal value As List(Of SCORM_LangString))
				_Entry = value
			End Set
		End Property
#End Region

		Sub New(ByVal Identifier As String)
			Me._ID = Identifier
			Me._Entry = New List(Of SCORM_LangString)
		End Sub
		Sub New(ByVal Identifier As String, ByVal oEntry As List(Of SCORM_LangString))
			Me._ID = Identifier
			Me._Entry = oEntry
		End Sub
	End Class
End Namespace