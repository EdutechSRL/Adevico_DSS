Imports Microsoft.VisualBasic

<Serializable()> Public Class DomandaNumerica

    Private _id As Integer
    Private _idDomanda As Integer
    Private _testoPrima As String
    Private _testoDopo As String
    Private _dimensione As Integer
    Private _rispostaCorretta As Double
    Private _numeroRisposte As Integer
    Private _numero As String
    Private _peso As Int16 = 100

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property
    Public Property peso() As Integer
        Get
            Return _peso
        End Get
        Set(ByVal value As Integer)
            _peso = value
        End Set
    End Property
    Public Property numero() As String
        Get
            Return _numero
        End Get
        Set(ByVal value As String)
            _numero = value
        End Set
    End Property
    Public Property rispostaCorretta() As Double
        Get
            Return _rispostaCorretta
        End Get
        Set(ByVal value As Double)
            _rispostaCorretta = value
        End Set
    End Property
    Public Property idDomanda() As Integer
        Get
            Return _idDomanda
        End Get
        Set(ByVal value As Integer)
            _idDomanda = value
        End Set
    End Property
    Public Property testoPrima() As String
        Get
            Return _testoPrima
        End Get
        Set(ByVal value As String)
            _testoPrima = value
        End Set
    End Property
    Public Property testoDopo() As String
        Get
            Return _testoDopo
        End Get
        Set(ByVal value As String)
            _testoDopo = value
        End Set
    End Property
    Public Property dimensione() As Integer
        Get
            Return _dimensione
        End Get
        Set(ByVal value As Integer)
            _dimensione = value
        End Set
    End Property
    Public Property numeroRisposte() As Integer
        Get
            Return _numeroRisposte
        End Get
        Set(ByVal value As Integer)
            _numeroRisposte = value
        End Set
    End Property
End Class
