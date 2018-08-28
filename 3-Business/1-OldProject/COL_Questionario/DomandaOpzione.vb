Imports Microsoft.VisualBasic

<Serializable()> Public Class DomandaOpzione

    Private _id As String
    Private _numero As String
    Private _testo As String
    Private _testoDopo As String
    Private _peso As Decimal
    Private _isCorretta As Boolean
    Private _isAltro As Boolean 'abilita la textbox per l'inserimento di testo libero
    Private _isValida As Boolean 'abilita il controllo di errore sulla lunghezza delle opzioni
    Private _numeroRisposte As String
    Private _arrayCBisVisible As String
    Private _suggestion As String

    Public Property suggestion() As String
        Get
            Return _suggestion
        End Get
        Set(ByVal value As String)
            _suggestion = value
        End Set
    End Property

    Public Property id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
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

    Public Property testo() As String
        Get
            Return _testo
        End Get
        Set(ByVal value As String)
            _testo = value
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

    Public Property peso() As Decimal
        Get
            Return _peso
        End Get
        Set(ByVal value As Decimal)
            _peso = value
        End Set
    End Property

    Public Property isCorretta() As Boolean
        Get
            Return _isCorretta
        End Get
        Set(ByVal value As Boolean)
            _isCorretta = value
        End Set
    End Property

    Public Property isAltro() As Boolean
        Get
            Return _isAltro
        End Get
        Set(ByVal value As Boolean)
            _isAltro = value
        End Set
    End Property

    Public Property isValida() As Boolean
        Get
            Return _isValida
        End Get
        Set(ByVal value As Boolean)
            _isValida = value
        End Set
    End Property

    Public Property numeroRisposte() As String
        Get
            Return _numeroRisposte
        End Get
        Set(ByVal value As String)
            _numeroRisposte = value
        End Set
    End Property

    Public Property arrayCBisVisible() As String
        Get
            Return _arrayCBisVisible
        End Get
        Set(ByVal value As String)
            _arrayCBisVisible = value
        End Set
    End Property

End Class
