Namespace File
	<Serializable(), CLSCompliant(True)> Public Class ServiceRepository
		Inherits Repository

		Private _Community As Community
		Private _Service As Object

		Public Property CommunityOwner() As Community
			Get
				Return _Community
			End Get
			Set(ByVal value As Community)
				_Community = value
			End Set
		End Property

		Sub New()
			MyBase.New()
		End Sub
	End Class
End Namespace