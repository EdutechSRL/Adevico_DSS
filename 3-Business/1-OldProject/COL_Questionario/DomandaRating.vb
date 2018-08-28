Imports Microsoft.VisualBasic

<Serializable()> Public Class DomandaRating
    Private _id As String
    Private _idDomanda As String
    Private _numeroRating As String
    Private _mostraND As Boolean
    Private _testoND As String
    Private _numeroRisposte As String
    Private _tipoIntestazione As Integer
    Private _intestazioniRating As New list(Of DomandaOpzione)
    Private _opzioniRating As New list(Of DomandaOpzione)
    Private _intestazioniMeeting As New List(Of Date)

    Public Enum TipoIntestazioneRating
        Numerazione = 0
        Testi = 1
    End Enum

    Public Property id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Public Property tipoIntestazione() As Integer
        Get
            Return _tipoIntestazione
        End Get
        Set(ByVal value As Integer)
            _tipoIntestazione = value
        End Set
    End Property


    Public Property idDomanda() As String
        Get
            Return _idDomanda
        End Get
        Set(ByVal value As String)
            _idDomanda = value
        End Set
    End Property

    Public Property numeroRating() As String
        Get
            If _numeroRating < 3 Then
                Return 2
            Else
                Return _numeroRating
            End If
        End Get
        Set(ByVal value As String)
            _numeroRating = value
        End Set
    End Property
    Public Property numeroMeeting() As String
        Get
            If _numeroRating < 2 Then
                Return 1
            Else
                Return _numeroRating
            End If
        End Get
        Set(ByVal value As String)
            _numeroRating = value
        End Set
    End Property
    Public Property mostraND() As Boolean
        Get
            Return _mostraND
        End Get
        Set(ByVal value As Boolean)
            _mostraND = value
        End Set
    End Property

    Public Property testoND() As String
        Get
            Return _testoND
        End Get
        Set(ByVal value As String)
            _testoND = value
        End Set
    End Property

    Public Property numeroRisposte() As String
        Get
            If _numeroRisposte < 1 Then
                Return 1
            End If
            Return _numeroRisposte
        End Get
        Set(ByVal value As String)
            _numeroRisposte = value
        End Set
    End Property

    Public Property opzioniRating() As List(Of DomandaOpzione)
        Get
            Return _opzioniRating
        End Get
        Set(ByVal value As List(Of DomandaOpzione))
            _opzioniRating = value
        End Set
    End Property
    Public Property intestazioniMeeting() As List(Of Date)
        Get
            If _intestazioniMeeting.Count = 0 Then
                For Each oDomOpz As DomandaOpzione In _intestazioniRating
                    _intestazioniMeeting.Add(CDate(oDomOpz.testo))
                Next
            End If
            Return _intestazioniMeeting
        End Get
        Set(ByVal value As List(Of Date))
            Dim n As Integer = 1
            _intestazioniMeeting.Clear()
            _intestazioniRating.Clear()
            For Each item As Date In value
                Dim oDomandaOpzione As New DomandaOpzione
                oDomandaOpzione.numero = n
                oDomandaOpzione.testo = item.Date.ToString("dd/MM/yyyy")
                _intestazioniRating.Add(oDomandaOpzione)
                n = n + 1
            Next
        End Set
    End Property
    Public Property intestazioniRating() As list(Of DomandaOpzione)
        Get
            Return _intestazioniRating
        End Get
        Set(ByVal value As list(Of DomandaOpzione))
            _intestazioniRating = value
        End Set
    End Property



End Class
