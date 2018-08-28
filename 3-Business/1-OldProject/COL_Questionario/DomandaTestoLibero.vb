Imports Microsoft.VisualBasic

<Serializable()> Public Class DomandaTestoLibero

    Private _id As Integer
    Private _idDomanda As Integer
    Private _numeroRighe As Integer
    Private _numeroColonne As Integer
    Private _etichetta As String
    Private _numeroRisposte As Integer
    Private _numero As String
    Private _peso As Integer = 100
    Private _isSingleLine As Boolean = True

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
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
    Public Property idDomanda() As Integer
        Get
            Return _idDomanda
        End Get
        Set(ByVal value As Integer)
            _idDomanda = value
        End Set
    End Property
    Public Property numeroRighe() As Integer
        Get
            Return _numeroRighe
        End Get
        Set(ByVal value As Integer)
            _numeroRighe = value
        End Set
    End Property
    Public Property numeroColonne() As Integer
        Get
            Return _numeroColonne
        End Get
        Set(ByVal value As Integer)
            _numeroColonne = value
        End Set
    End Property
    Public Property etichetta() As String
        Get
            Return _etichetta
        End Get
        Set(ByVal value As String)
            _etichetta = value
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
    Public Property peso() As Integer
        Get
            Return _peso
        End Get
        Set(ByVal value As Integer)
            _peso = value
        End Set
    End Property

    Public Property isSingleLine As Boolean
        Get
            Return _isSingleLine
        End Get
        Set(ByVal value As Boolean)
            _isSingleLine = value
        End Set
    End Property
End Class
