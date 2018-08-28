Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints


<Serializable(), CLSCompliant(True)> Public Class PrivateData
	Inherits DomainObject

#Region "Private Fields"
	Private _ID As Integer
	Private _PostalAddress As PostalAddress
	Private _Contact As Contact
	Private _BirthDate As Nullable(Of DateTime)
	Private _BirthPlace As String
	Private _Info As String
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
	Public Property Address() As PostalAddress
		Get
			Return _PostalAddress
		End Get
		Set(ByVal value As PostalAddress)
			_PostalAddress = value
		End Set
	End Property
	Public Property UserContact() As Contact
		Get
			Return _Contact
		End Get
		Set(ByVal value As Contact)
			_Contact = value
		End Set
	End Property
	<Nullable(True)> Public Property Note() As String
		Get
			Return _Info
		End Get
		Set(ByVal value As String)
			_Info = value
		End Set
	End Property
	<Nullable(True)> Public Property BirthDate() As Nullable(Of DateTime)
		Get
			Return _BirthDate
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_BirthDate = value
		End Set
	End Property
	<Nullable(True)> Public Property BirthPlace() As String
		Get
			Return _BirthPlace
		End Get
		Set(ByVal value As String)
			_BirthPlace = value
		End Set
	End Property
#End Region

	Sub New()
		MyBase.New()
	End Sub
End Class