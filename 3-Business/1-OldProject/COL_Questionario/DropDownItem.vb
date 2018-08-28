Imports Microsoft.VisualBasic

<Serializable()> Public Class DropDownItem

    Private _id As Integer
    Private _idDropDown As Integer
    Private _testo As String
    Private _numero As String
    Private _indice As Integer
    Private _numeroRisposte As Integer
    Private _peso As Decimal
    Private _isCorretta As Boolean
    Private _isValida As Boolean = True
    Private _suggestion As String

    Public Property suggestion() As String
        Get
            Return _suggestion
        End Get
        Set(ByVal value As String)
            _suggestion = value
        End Set
    End Property

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property idDropDown() As Integer
        Get
            Return _idDropDown
        End Get
        Set(ByVal value As Integer)
            _idDropDown = value
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

    Public Property numero() As Integer
        Get
            Return _numero
        End Get
        Set(ByVal value As Integer)
            _numero = value
        End Set
    End Property

    Public Property indice() As Integer
        Get
            Return _indice
        End Get
        Set(ByVal value As Integer)
            _indice = value
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
    Public Property isValida() As Boolean
        Get
            Return _isValida
        End Get
        Set(ByVal value As Boolean)
            _isValida = value
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

    Public Property numeroRisposte() As Integer
        Get
            Return _numeroRisposte
        End Get
        Set(ByVal value As Integer)
            _numeroRisposte = value
        End Set
    End Property

End Class
