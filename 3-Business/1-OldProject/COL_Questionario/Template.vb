Imports Microsoft.VisualBasic

<Serializable()> Public Class Template
    Private _id As Integer
    Private _tipo As String
    Private _testo As String
    Private _titolo As String
    Private _nome As String
    Private _idPersona As Integer
    Private _idLingua As Integer

    Public Enum tipoTempate
        Questionario = 5
    End Enum

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property tipo() As Integer
        Get
            Return _tipo
        End Get
        Set(ByVal value As Integer)
            _tipo = value
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

    Public Property titolo() As String
        Get
            Return _titolo
        End Get
        Set(ByVal value As String)
            _titolo = value
        End Set
    End Property

    Public Property nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Public Property idPersona() As Integer
        Get
            Return _idPersona
        End Get
        Set(ByVal value As Integer)
            _idPersona = value
        End Set
    End Property

    Public Property idLingua() As Integer
        Get
            Return _idLingua
        End Get
        Set(ByVal value As Integer)
            _idLingua = value
        End Set
    End Property


End Class
