Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints

<Serializable(), CLSCompliant(True)> Public Class Contact
	Inherits DomainObject

#Region "Private Fields"
	Private _PersonalPhone As String
	Private _WorkPhone As String
	Private _MobilePhone As String
	Private _Fax As String
	Private _HomePage As String
#End Region

#Region "Public Property"
	<Nullable(True)> Public Property PersonalPhone() As String
		Get
			Return _PersonalPhone
		End Get
		Set(ByVal value As String)
			_PersonalPhone = value
		End Set
	End Property
	<Nullable(True)> Public Property WorkPhone() As String
		Get
			Return _WorkPhone
		End Get
		Set(ByVal value As String)
			_WorkPhone = value
		End Set
	End Property
	<Nullable(True)> Public Property MobilePhone() As String
		Get
			Return _MobilePhone
		End Get
		Set(ByVal value As String)
			_MobilePhone = value
		End Set
	End Property
	<Nullable(True)> Public Property Fax() As String
		Get
			Return _Fax
		End Get
		Set(ByVal value As String)
			_Fax = value
		End Set
	End Property
	<Nullable(True)> Public Property HomePage() As String
		Get
			Return _HomePage
		End Get
		Set(ByVal value As String)
			_HomePage = value
		End Set
	End Property
#End Region

	Sub New()
		MyBase.New()
	End Sub
End Class