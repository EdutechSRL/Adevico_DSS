<Serializable(), CLSCompliant(True)> Public Class Role
	Inherits DomainObject

#Region "Private Property"
	Private _ID As Integer
	Private _Name As String
	Private _Description As String
	Private _PriorityOrder As Integer
	Private _CanBeDeleted As Boolean
	Private _CanBeModify As Boolean
#End Region

#Region "Public Property"
	Public Property ID() As Integer
		Get
			Return _ID
		End Get
		Set(ByVal value As Integer)
			_ID = value
		End Set
	End Property
	Public Property Name() As String
		Get
			Return _Name
		End Get
		Set(ByVal value As String)
			_Name = value
		End Set
	End Property
	Public Property Description() As String
		Get
			Return _Description
		End Get
		Set(ByVal value As String)
			_Description = value
		End Set
	End Property
	Public Property PriorityOrder() As Integer
		Get
			Return _PriorityOrder
		End Get
		Set(ByVal value As Integer)
			_PriorityOrder = value
		End Set
	End Property
	Public Property CanBeDeleted() As Boolean
		Get
			Return _CanBeDeleted
		End Get
		Set(ByVal value As Boolean)
			_CanBeDeleted = value
		End Set
	End Property
	Public Property CanBeModify() As Boolean
		Get
			Return _CanBeModify
		End Get
		Set(ByVal value As Boolean)
			_CanBeModify = value
		End Set
	End Property
#End Region

#Region "Metodi New"
	Sub New()
		Me._CanBeDeleted = True
		Me._CanBeModify = True
	End Sub
	Sub New(ByVal ID As Integer)
		Me._ID = ID
		Me._CanBeDeleted = True
		Me._CanBeModify = True
	End Sub
	Sub New( _
	ByVal ID As Integer, _
	ByVal iDescription As String, _
	ByVal Gerarchia As Integer, _
	Optional ByVal iCanBeDeleted As Boolean = True, _
	Optional ByVal iCanBeModify As Boolean = True)
		Me._ID = ID
		Me._Description = iDescription
		Me._PriorityOrder = Gerarchia
		Me._CanBeDeleted = iCanBeDeleted
		Me._CanBeModify = iCanBeModify
	End Sub
#End Region

End Class