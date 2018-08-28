Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class URLelement

#Region "Private properties"
		Private _Name As String
		Private _Path As String
#End Region

#Region "Public properties"
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				_Name = value
			End Set
		End Property
		Public Property Path() As String
			Get
				Return _Path
			End Get
			Set(ByVal value As String)
				_Path = value
			End Set
		End Property
#End Region

		Sub New()

		End Sub
		Sub New(ByVal iName As String, ByVal iPath As String)
			Me._Name = iName
			Me._Path = iPath
		End Sub

		Public Shared Function FindByName(ByVal item As URLelement, ByVal argument As String) As Boolean
			Return item.Name = argument
		End Function
	End Class
End Namespace