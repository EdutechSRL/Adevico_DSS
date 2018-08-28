Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

<Serializable()> Public Class Domanda

    Private _id As String
    Private _numero As Integer
    Private _testo As String
    Private _tipo As Integer
    Private _idPagina As Integer
    Private _numeroPagina As Integer
    Private _testoPrima As String
    Private _testoDopo As String
    Private _idQuestionarioMultilingua As Integer
    Private _isMultipla As Boolean
    Private _idDomandaMultipla As Integer
    Private _idLingua As Integer
    Private _idDomandaMultilingua As Integer
    Private _peso As Integer = 1
    Private _difficolta As Integer = 1
    Private _tipoGrafico As Integer
    Private _numeroRisposteDomanda As Integer
    Private _numeroMaxRisposte As Integer
    Private _domandaDropDown As New DropDown
    Private _domandaRating As New DomandaRating
    Private _opzioniTestoLibero As New list(Of DomandaTestoLibero)
    Private _opzioniNumerica As New list(Of DomandaNumerica)
    Private _domandaMultiplaOpzioni As New list(Of DomandaOpzione)
    Private _risposteDomanda As New list(Of RispostaDomanda)
    Private _isValida As Boolean = True 'true se sono state selezionate piu' risposte di numeroMaxRisposte o se il testo delle opzioni e' troppo lungo
    Private _isObbligatoria As Boolean
    Private _isValutabile As Boolean = True
    Private _idPersonaEditor As Integer
    Private _dataModifica As Date
    Private _dataCreazione As Date
    Private _idPersonaCreator As Integer
    Private _idQuestionario As Integer
    Private _domandaCount As Integer
    Private _suggerimento As String
    Private _difficoltaTesto As String
    Private _isReadOnly As Boolean
    Private _isNew As Boolean
    Private _isSelected As Boolean
    Private _virtualNumber As Long


    Public Enum TipoDomanda
        Nessuno = 0
        Rating = 1
        Numerica = 2
        Multipla = 3
        DropDown = 4
        TestoLibero = 5
        Meeting = 6

        RatingStars = 10
    End Enum

    Public Enum DifficoltaDomanda
        Tutte = -1
        Bassa = 0
        Media = 1
        Alta = 2
    End Enum

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property isSelected() As Boolean
        Get
            Return _isSelected
        End Get
        Set(ByVal value As Boolean)
            _isSelected = value
        End Set
    End Property

    Public Property isNew() As Boolean
        Get
            Return _isNew
        End Get
        Set(ByVal value As Boolean)
            _isNew = value
        End Set
    End Property

    Public Property idDomandaMultipla() As Integer
        Get
            Return _idDomandaMultipla
        End Get
        Set(ByVal value As Integer)
            _idDomandaMultipla = value
        End Set
    End Property

    Public Property idDomandaMultilingua() As Integer
        Get
            Return _idDomandaMultilingua
        End Get
        Set(ByVal value As Integer)
            _idDomandaMultilingua = value
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

    Public Property idPagina() As Integer
        Get
            Return _idPagina
        End Get
        Set(ByVal value As Integer)
            _idPagina = value
        End Set
    End Property

    Public Property numeroPagina() As Integer
        Get
            Return _numeroPagina
        End Get
        Set(ByVal value As Integer)
            _numeroPagina = value
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

    Public Property testo() As String
        Get
            Return _testo
        End Get
        Set(ByVal value As String)
            _testo = value
        End Set
    End Property

    Public Property suggerimento() As String
        Get
            Return _suggerimento
        End Get
        Set(ByVal value As String)
            _suggerimento = value
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

    Public Property peso() As Integer
        Get
            Return _peso
        End Get
        Set(ByVal value As Integer)
            _peso = value
        End Set
    End Property

    Public Property difficolta() As Integer
        Get
            Return _difficolta
        End Get
        Set(ByVal value As Integer)
            _difficolta = value
        End Set
    End Property

    Public ReadOnly Property difficoltaTesto() As String
        Get
            Select Case _difficolta
                Case 0
                    _difficoltaTesto = "- Diff. Min"
                Case 1
                    _difficoltaTesto = "- Diff. Med"
                Case 2
                    _difficoltaTesto = "- Diff. Max"
            End Select
            Return _difficoltaTesto
        End Get
    End Property

    Public Property tipo() As Integer
        Get
            Return _tipo
        End Get
        Set(ByVal value As Integer)
            _tipo = value
        End Set
    End Property

    Public Property domandaCount() As Integer
        Get
            Return _domandaCount
        End Get
        Set(ByVal value As Integer)
            _domandaCount = value
        End Set
    End Property

    Public Property idQuestionario() As Integer
        Get
            Return _idQuestionario
        End Get
        Set(ByVal value As Integer)
            _idQuestionario = value
        End Set
    End Property

    Public Property tipoGrafico() As Integer
        Get
            Return _tipoGrafico
        End Get
        Set(ByVal value As Integer)
            _tipoGrafico = value
        End Set
    End Property

    Public Property domandaDropDown() As DropDown
        Get
            Return _domandaDropDown
        End Get
        Set(ByVal value As DropDown)
            _domandaDropDown = value
        End Set
    End Property

    Public Property domandaRating() As DomandaRating
        Get
            Return _domandaRating
        End Get
        Set(ByVal value As DomandaRating)
            _domandaRating = value
        End Set
    End Property

    Public Property opzioniTestoLibero() As list(Of DomandaTestoLibero)
        Get
            Return _opzioniTestoLibero
        End Get
        Set(ByVal value As list(Of DomandaTestoLibero))
            _opzioniTestoLibero = value
        End Set
    End Property

    Public Property opzioniNumerica() As list(Of DomandaNumerica)
        Get
            Return _opzioniNumerica
        End Get
        Set(ByVal value As list(Of DomandaNumerica))
            _opzioniNumerica = value
        End Set
    End Property

    Public Property isMultipla() As Boolean
        Get
            Return _isMultipla
        End Get
        Set(ByVal value As Boolean)
            _isMultipla = value
        End Set
    End Property


    Public Property numeroRisposteDomanda() As String
        Get
            Return _numeroRisposteDomanda
        End Get
        Set(ByVal value As String)
            _numeroRisposteDomanda = value
        End Set
    End Property

    Public Property numeroMaxRisposte() As String
        Get
            Return _numeroMaxRisposte
        End Get
        Set(ByVal value As String)
            _numeroMaxRisposte = value
        End Set
    End Property

    Public Property domandaMultiplaOpzioni() As list(Of DomandaOpzione)
        Get
            Return _domandaMultiplaOpzioni
        End Get
        Set(ByVal value As list(Of DomandaOpzione))
            _domandaMultiplaOpzioni = value
        End Set
    End Property

    Public Property risposteDomanda() As list(Of RispostaDomanda)
        Get
            Return _risposteDomanda
        End Get
        Set(ByVal value As list(Of RispostaDomanda))
            _risposteDomanda = value
        End Set
    End Property
    Public Property VirtualNumber() As Long
        Get
            Return _virtualNumber
        End Get
        Set(ByVal value As Long)
            _virtualNumber = value
        End Set
    End Property
    Public Shared Function findDomandaBYID(ByVal listaDom As List(Of Domanda), ByVal idDomanda As String) As Domanda

        Dim odomanda As New Domanda
        'odomanda = listaDom.Find(New PredicateWrapper(Of Domanda, String)(idDomanda, AddressOf trovaID))

        odomanda = listaDom.FirstOrDefault(Function(dm) dm.id = idDomanda)

        Return odomanda

    End Function

    Public Shared Function removeDomandaBYID(ByVal listaDom As List(Of Domanda), ByVal idDom As String) As Integer

        Dim i As Integer
        i = listaDom.RemoveAll(New PredicateWrapper(Of Domanda, String)(idDom, AddressOf trovaID))

        Return i

    End Function

    Public Shared Function trovaID(ByVal item As Domanda, ByVal argument As String) As Boolean

        Return item.id = argument

    End Function
    'Public Shared Function trovaIdOpzione(ByVal item As DomandaOpzione, ByVal argument As String) As Boolean

    '    Return item.id = argument

    'End Function
    Public Shared Function findDomandaBYNumero(ByVal listaDom As List(Of Domanda), ByVal numeroDomanda As String) As Domanda

        Dim odomanda As New Domanda
        odomanda = listaDom.Find(New PredicateWrapper(Of Domanda, String)(numeroDomanda, AddressOf trovaNumero))

        Return odomanda

    End Function

    Public Shared Function trovaNumero(ByVal item As Domanda, ByVal argument As String) As Boolean

        Return item.numero = argument

    End Function


    Public Property isValida() As Boolean
        Get
            Return _isValida
        End Get
        Set(ByVal value As Boolean)
            _isValida = value
        End Set
    End Property

    Public Property isValutabile() As Boolean
        Get
            Return _isValutabile
        End Get
        Set(ByVal value As Boolean)
            _isValutabile = value
        End Set
    End Property

    Public Property isObbligatoria() As Boolean
        Get
            Return _isObbligatoria
        End Get
        Set(ByVal value As Boolean)
            _isObbligatoria = value
        End Set
    End Property

    Public Property idPersonaEditor() As Integer
        Get
            Return _idPersonaEditor
        End Get
        Set(ByVal value As Integer)
            _idPersonaEditor = value
        End Set
    End Property

    Public Property dataModifica() As Date
        Get
            Return _dataModifica
        End Get
        Set(ByVal value As Date)
            _dataModifica = value
        End Set
    End Property

    Public Property dataCreazione() As Date
        Get

            Return _dataCreazione
        End Get
        Set(ByVal value As Date)
            _dataCreazione = value
        End Set
    End Property

    Public Property idPersonaCreator() As Integer
        Get
            Return _idPersonaCreator
        End Get
        Set(ByVal value As Integer)
            _idPersonaCreator = value
        End Set
    End Property

    Public Property isReadOnly() As Boolean
        Get
            Return _isReadOnly
        End Get
        Set(ByVal value As Boolean)
            _isReadOnly = value
        End Set
    End Property

End Class
