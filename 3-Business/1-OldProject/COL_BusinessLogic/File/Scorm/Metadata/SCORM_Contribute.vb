Namespace Comol.Materiale.Scorm.Metadata
	Public Class SCORM_Contribute

#Region "Private Property"
		Private _Role As SCORM_SourceValue
		Private _Entity As List(Of String)
		Private _DateContribution As Date
#End Region

#Region "Public Property"
		Public ReadOnly Property Role() As SCORM_SourceValue
			Get
				Return _Role
			End Get
		End Property
		Public ReadOnly Property Entity() As List(Of String)
			Get
				Return _Entity
			End Get
		End Property
		Public ReadOnly Property DateContribution() As Date
			Get
				Return _DateContribution
			End Get
		End Property
#End Region

		Sub New(ByVal oRole As SCORM_SourceValue, ByVal oEntity As List(Of String), ByVal oDate As Date)
			Me._DateContribution = oDate
			Me._Entity = oEntity
			Me._Role = oRole
		End Sub

	End Class
End Namespace