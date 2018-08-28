<Serializable(), CLSCompliant(True)> Public Class AcademicYear
	Inherits DomainObject

#Region "Private Property"
	Private _ID As Integer
	Private _Description As String
#End Region

#Region "Public Property"
	Public Property Year() As Integer
		Get
			Year = _ID
		End Get
		Set(ByVal value As Integer)
			_ID = value
		End Set
	End Property
	Public Property Description() As String
		Get
			If _Description = "" Then
				_Description = Right(_ID, 2) & "/" & Right(_ID + 1, 2)
			End If
			Description = _Description
		End Get
		Set(ByVal value As String)
			_Description = value
		End Set
	End Property
	Public ReadOnly Property LongDescription() As String
		Get
			LongDescription = _ID & "/" & _ID + 1
		End Get
	End Property
#End Region

	Public Sub New()

	End Sub
	Public Sub New(ByVal iYear As Integer)
		With Me
			_ID = iYear
			_Description = ""
		End With
	End Sub
	Public Sub New(ByVal iYear As Integer, ByVal iDescription As String)
		With Me
			_ID = iYear
			_Description = iDescription
		End With
	End Sub
	Public Overloads Function ToString(ByVal strict As Boolean) As String
		If strict Then
			Return Right(_ID, 2) & "/" & Right(_ID + 1, 2)
		Else
			Return Me._Description
		End If
	End Function
End Class