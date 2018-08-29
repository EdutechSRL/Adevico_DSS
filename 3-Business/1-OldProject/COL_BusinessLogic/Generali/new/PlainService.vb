<Serializable()> Public  Class PlainService
    Private _ServizioID As Integer
    Private _ServizioCode As String
    Private _ServizioPermessi As String
    Private _ServizioName As String

    Public Property ID() As Integer
        Get
            ID = _ServizioID
        End Get
        Set(ByVal value As Integer)
            _ServizioID = value
        End Set
    End Property
    Public Property Code() As String
        Get
            Code = _ServizioCode
        End Get
        Set(ByVal value As String)
            _ServizioCode = value
        End Set
    End Property
    Public Property Permessi() As String
        Get
            Permessi = _ServizioPermessi
        End Get
        Set(ByVal value As String)
            _ServizioPermessi = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Name = _ServizioName
        End Get
        Set(ByVal value As String)
            _ServizioName = value
        End Set
    End Property
    Public Sub New()

    End Sub
    Public Sub New(ByVal ID As Integer)
        _ServizioID = ID
    End Sub
    Public Sub New(ByVal ID As Integer, ByVal Code As String)
        _ServizioID = ID
        _ServizioCode = Code
    End Sub
    Public Sub New(ByVal ID As Integer, ByVal Code As String, ByVal Permessi As String)
        _ServizioID = ID
        _ServizioCode = Code
        _ServizioPermessi = Permessi
    End Sub
    Public Sub New(ByVal ID As Integer, ByVal iName As String, ByVal Code As String, ByVal Permessi As String)
        _ServizioID = ID
        _ServizioCode = Code
        _ServizioPermessi = Permessi
        _ServizioName = iName
    End Sub

    Public Shared Function Create(ByVal ID As Integer, ByVal Code As String) As PlainService
        Dim o As New PlainService
        o.ID = ID
        o.Code = Code
        o.Permessi = "00000000000000000000000000000000"
        Return o
    End Function
    Public Shared Function Create(ByVal ID As Integer, ByVal Code As String, ByVal Name As String) As PlainService
        Dim o As New PlainService
        o.ID = ID
        o.Code = Code
        o.Permessi = "00000000000000000000000000000000"
        o.Name = Name
        Return o
    End Function
End Class