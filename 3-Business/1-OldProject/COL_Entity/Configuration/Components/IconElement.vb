Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class IconElement

#Region "Private Property"
		Private _Icon As String
		Private _Extension As String
#End Region

#Region "Public Property"
		Public ReadOnly Property Icon() As String
			Get
				Return _Icon
			End Get
		End Property
		Public ReadOnly Property Extension() As String
			Get
				Return _Extension
			End Get
		End Property
#End Region

		Public Sub New(ByVal iExtension As String, ByVal iIcon As String)
			Me._Extension = iExtension
			Me._Icon = iIcon
		End Sub
	End Class
End Namespace