Namespace File
	<Serializable(), CLSCompliant(True)> Public Class MimeType

#Region "Private Property"
		Private _Type As String
		Private _Extension As String
#End Region

#Region "Public Property"
		Public Property Type() As String
			Get
				Return _Type
			End Get
			Set(ByVal value As String)
				_Type = value
			End Set
		End Property
		Public Property Extension() As String
			Get
				Return _Extension
			End Get
			Set(ByVal value As String)
				_Extension = value
			End Set
		End Property
#End Region

		Public Sub New(ByVal iExtension As String, ByVal iMimeType As String)
			Me._Extension = iExtension
			Me._Type = iMimeType
		End Sub
	End Class
End Namespace