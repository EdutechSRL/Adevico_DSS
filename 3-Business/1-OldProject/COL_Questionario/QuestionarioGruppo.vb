Imports Microsoft.VisualBasic

<Serializable()> Public Class QuestionarioGruppo
#Region "private"
    Private _id As Integer
    Private _nome As String
    Private _idGruppoPadre As Integer
    Private _descrizione As String
    Private _dataCreazione As String
    Private _dataModifica As String
    Private _idComunita As Integer
    Private _tipo As Integer
    Private _idPersona As Integer
    Private _isPubblico As Boolean
    Private _isCondiviso As Boolean
    Private _isPersonale As Boolean
    Private _isComunita As Boolean
    Private _isCancellato As Boolean
    Private _questionari As New list(Of Questionario)
#End Region

    Public Sub New()
    End Sub
    Public Sub New(ByVal CommunityId As Integer)
        _dataCreazione = Now.ToString
        _dataModifica = Now.ToString
        _idComunita = CommunityId
        _nome = COL_Questionario.RootObject.nomeGruppoDefault
        _idGruppoPadre = 0
    End Sub
    Public Enum TipoGruppo
        CartellaQuestionari = 0
        CartellaLibrerie = 1
    End Enum
    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
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
    Public Property idGruppoPadre() As Integer
        Get
            Return _idGruppoPadre
        End Get
        Set(ByVal value As Integer)
            _idGruppoPadre = value
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
    Public Property idComunita() As Integer
        Get
            Return _idComunita
        End Get
        Set(ByVal value As Integer)
            _idComunita = value
        End Set
    End Property
    Public Property descrizione() As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property
    Public Property dataCreazione() As String
        Get
            Return _dataCreazione
        End Get
        Set(ByVal value As String)
            _dataCreazione = value
        End Set
    End Property
    Public Property dataModifica() As String
        Get
            Return _dataModifica
        End Get
        Set(ByVal value As String)
            _dataModifica = value
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
    Public Property isPubblico() As Boolean
        Get
            Return _isPubblico
        End Get
        Set(ByVal value As Boolean)
            _isPubblico = value
        End Set
    End Property
    Public Property isCondiviso() As Boolean
        Get
            Return _isCondiviso
        End Get
        Set(ByVal value As Boolean)
            _isCondiviso = value
        End Set
    End Property
    Public Property isPersonale() As Boolean
        Get
            Return _isPersonale
        End Get
        Set(ByVal value As Boolean)
            _isPersonale = value
        End Set
    End Property
    Public Property isCancellato() As Boolean
        Get
            Return _isCancellato
        End Get
        Set(ByVal value As Boolean)
            _isCancellato = value
        End Set
    End Property
    Public Property isComunita() As Boolean
        Get
            Return _isComunita
        End Get
        Set(ByVal value As Boolean)
            _isComunita = value
        End Set
    End Property
    Public Property questionari() As List(Of Questionario)
        Get
            Return _questionari
        End Get
        Set(ByVal value As List(Of Questionario))
            _questionari = value
        End Set
    End Property

End Class
