Imports Microsoft.VisualBasic

<Serializable()> Public Class Statistica
    Private _idPersona As Integer
    Private _nomeUtente As String
    Private _punteggio As Decimal
    Private _nRisposteTotali As Integer
    Private _nRisposteCorrette As Integer
    Private _nRisposteErrate As Integer
    Private _nRisposteNonValutate As Integer
    Private _nRisposteParzialmenteCorrette As Integer
    Private _nRisposteSaltate As Integer
    Private _coeffDifficolta As Decimal
    ' Private _nOpzioniTotali As Integer
    'Private _nOpzioniCorrette As Integer
    'Private _nOpzioniErrate As Integer
    'Private _nOpzioniNonValutate As Integer
    Private _punteggioRelativo As Decimal
    Private _isFinito As Boolean
    Private _pesoTotaleDomande As Integer

    Public Property coeffDifficolta() As Decimal
        Get
            Return _coeffDifficolta
        End Get
        Set(ByVal value As Decimal)
            _coeffDifficolta = value
        End Set
    End Property
    Public Property punteggioRelativo() As Decimal
        Get
            Return _punteggioRelativo
        End Get
        Set(ByVal value As Decimal)
            _punteggioRelativo = value
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
    Public Property punteggio() As Decimal
        Get
            Return _punteggio
        End Get
        Set(ByVal value As Decimal)
            _punteggio = value
        End Set
    End Property
    Public Property nRisposteTotali() As Integer
        Get
            Return _nRisposteTotali
        End Get
        Set(ByVal value As Integer)
            _nRisposteTotali = value
        End Set
    End Property
    Public Property nRisposteCorrette() As Integer
        Get
            Return _nRisposteCorrette
        End Get
        Set(ByVal value As Integer)
            _nRisposteCorrette = value
        End Set
    End Property
    Public Property nRisposteErrate() As Integer
        Get
            Return _nRisposteErrate
        End Get
        Set(ByVal value As Integer)
            _nRisposteErrate = value
        End Set
    End Property
    Public Property nRisposteNonValutate() As Integer
        Get
            Return _nRisposteNonValutate
        End Get
        Set(ByVal value As Integer)
            _nRisposteNonValutate = value
        End Set
    End Property
    Public Property nRisposteParzialmenteCorrette() As Integer
        Get
            Return _nRisposteParzialmenteCorrette
        End Get
        Set(ByVal value As Integer)
            _nRisposteParzialmenteCorrette = value
        End Set
    End Property
    Public Property nRisposteSaltate() As Integer
        Get
            Return _nRisposteSaltate
        End Get
        Set(ByVal value As Integer)
            _nRisposteSaltate = value
        End Set
    End Property
    'Public Property nOpzioniTotali() As Integer
    '    Get
    '        Return _nOpzioniTotali
    '    End Get
    '    Set(ByVal value As Integer)
    '        _nOpzioniTotali = value
    '    End Set
    'End Property
    'Public Property nOpzioniCorrette() As Integer
    '    Get
    '        Return _nOpzioniCorrette
    '    End Get
    '    Set(ByVal value As Integer)
    '        _nOpzioniCorrette = value
    '    End Set
    'End Property
    'Public Property nOpzioniErrate() As Integer
    '    Get
    '        Return _nOpzioniErrate
    '    End Get
    '    Set(ByVal value As Integer)
    '        _nOpzioniErrate = value
    '    End Set
    'End Property
    'Public Property nOpzioniNonValutate() As Integer
    '    Get
    '        Return _nOpzioniNonValutate
    '    End Get
    '    Set(ByVal value As Integer)
    '        _nOpzioniNonValutate = value
    '    End Set
    'End Property
    Public Property nomeUtente() As String
        Get
            Return _nomeUtente
        End Get
        Set(ByVal value As String)
            _nomeUtente = value
        End Set
    End Property
    Public Property isFinito() As Boolean
        Get
            Return _isFinito
        End Get
        Set(ByVal value As Boolean)
            _isFinito = value
        End Set
    End Property
    Public Property pesoTotaleDomande() As Integer
        Get
            Return _pesoTotaleDomande
        End Get
        Set(ByVal value As Integer)
            _pesoTotaleDomande = value
        End Set
    End Property

    Public Sub reset()
        _nRisposteTotali = 0
        _nRisposteCorrette = 0
        _nRisposteErrate = 0
        _nRisposteNonValutate = 0
        _nRisposteParzialmenteCorrette = 0
        _nRisposteSaltate = 0
        _pesoTotaleDomande = 0
        _punteggio = 0
        _punteggioRelativo = 0
    End Sub
End Class
