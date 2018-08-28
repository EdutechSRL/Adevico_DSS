Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints

<Serializable(), CLSCompliant(True)> Public Class PostalAddress
	Inherits DomainObject

#Region "Private Fields"
	Private _ID As Integer
	Private _Address As String
	Private _PostalCode As String
	Private _City As String
	Private _State As State
	Private _Province As Provincia
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
	<Nullable(True)> Public Property Address() As String
		Get
			Return _Address
		End Get
		Set(ByVal value As String)
			_Address = value
		End Set
	End Property
	<Nullable(True)> Public Property PostalCode() As String
		Get
			Return _PostalCode
		End Get
		Set(ByVal value As String)
			_PostalCode = value
		End Set
	End Property
	<Nullable(True)> Public Property City() As String
		Get
			Return _City
		End Get
		Set(ByVal value As String)
			_City = value
		End Set
	End Property

	Public Property AddressProvince() As Provincia
		Get
			Return _Province
		End Get
		Set(ByVal value As Provincia)
			_Province = value
		End Set
	End Property

	Public Property AddressState() As State
		Get
			Return _State
		End Get
		Set(ByVal value As State)
			_State = value
		End Set
	End Property
#End Region

	Sub New()
		MyBase.New()
	End Sub
End Class