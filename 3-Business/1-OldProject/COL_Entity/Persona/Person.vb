<Serializable(), CLSCompliant(True)> Public Class Person
	Inherits DomainObject
	Implements IComparable

	'Private _SubscriptedCommunities As IList(Of Iscrizione)

	Private _ID As Integer
	Private _Name As String
    Private _Surname As String
    Private _Mail As String
    Private _TaxCode As String
    Private _Login As String
    Private _Password As String
    Private _Sex As Integer
    Private _PersonalInfo As PrivateData
    Private _Type As PersonType
    Private _Language As Lingua

    Private _OtherInfo As String


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
	Public Property Surname() As String
		Get
			Return _Surname
		End Get
		Set(ByVal value As String)
			_Surname = value
		End Set
	End Property
    Public Property Mail() As String
        Get
            Return _Mail
        End Get
        Set(ByVal value As String)
            _Mail = value
        End Set
    End Property
    Public Property TaxCode() As String
        Get
            Return _TaxCode
        End Get
        Set(ByVal value As String)
            _TaxCode = value
        End Set
    End Property
    Public Property Login() As String
        Get
            Return _Login
        End Get
        Set(ByVal value As String)
            _Login = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property
    Public Property Sex() As Integer
        Get
            Return Me._Sex
        End Get
        Set(ByVal value As Integer)
            Me._Sex = value
        End Set
    End Property
    Public Property PersonalInfo() As PrivateData
        Get
            Return Me._PersonalInfo
        End Get
        Set(ByVal value As PrivateData)
            Me._PersonalInfo = value
        End Set
    End Property
    Public Property Type() As PersonType
        Get
            Return Me._Type
        End Get
        Set(ByVal value As PersonType)
            Me._Type = value
        End Set
    End Property
    Public Property UIlanguage() As Lingua
        Get
            Return _Language
        End Get
        Set(ByVal value As Lingua)
            _Language = value
        End Set
    End Property
    Public Overridable Property OtherInfo() As String
        Get
            Return _OtherInfo
        End Get
        Set(ByVal value As String)
            _OtherInfo = value
        End Set
    End Property

    'Public Property ComunitaIscritte() As IList(Of Iscrizione)
    '	Get
    '		Return _listaComunitaIscritte
    '	End Get
    '	Set(ByVal value As IList(Of Iscrizione))
    '		_listaComunitaIscritte = value
    '	End Set
    'End Property

    'Public Property ComunitaCorrente() As Comunita
    '	Get
    '		Return _comunitacorrente
    '	End Get
    '	Set(ByVal value As Comunita)
    '		_comunitacorrente = value
    '	End Set
    'End Property

    Public Sub New()
        MyBase.New()
        Me._PersonalInfo = New PrivateData
        Me._Type = New PersonType
        Me._Language = New Lingua
    End Sub
    Public Sub New(ByVal iID As Integer)
        MyBase.New()
        Me._PersonalInfo = New PrivateData
        Me._Type = New PersonType
        Me._Language = New Lingua
        Me._ID = iID
    End Sub
	Public Sub New(ByVal iID As Integer, ByVal iName As String, ByVal iSurname As String)
		MyBase.New()
		Me._PersonalInfo = New PrivateData
		Me._Type = New PersonType
		Me._Language = New Lingua
		Me._ID = iID
		Me._Name = iName
		Me._Surname = iSurname
	End Sub

	Public Shared Function FindByID(ByVal item As Person, ByVal argument As Integer) As Boolean
		Return item.ID = argument
	End Function

	Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
		Dim objPerson As Person
		objPerson = CType(obj, Person)
		Me.ID.CompareTo(objPerson.ID)
	End Function
End Class