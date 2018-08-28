Imports Microsoft.VisualBasic
Imports System.Collections.Generic

<Serializable()> Public Class RispostaDomanda
    Private _id As Integer
    Private _idDomanda As Integer
    Private _idDomandaOpzione As Integer
    Private _tipo As Integer
    Private _idRispostaQuestionario As Integer
    Private _numeroOpzione As Integer
    Private _testoOpzione As String
    Private _valore As Double
    Private _valutazione As String

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property valore() As String
        Get
            Return _valore
        End Get
        Set(ByVal value As String)
            _valore = value
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

    Public Property idDomandaOpzione() As Integer
        Get
            Return _idDomandaOpzione
        End Get
        Set(ByVal value As Integer)
            _idDomandaOpzione = value
        End Set
    End Property

    Public Property idRispostaQuestionario() As Integer
        Get
            Return _idRispostaQuestionario
        End Get
        Set(ByVal value As Integer)
            _idRispostaQuestionario = value
        End Set
    End Property

    Public Property numeroOpzione() As Integer
        Get
            Return _numeroOpzione
        End Get
        Set(ByVal value As Integer)
            _numeroOpzione = value
        End Set
    End Property

    Public Property testoOpzione() As String
        Get
            Return _testoOpzione
        End Get
        Set(ByVal value As String)
            _testoOpzione = value
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

    Public Property valutazione() As String
        Get
            Return _valutazione
        End Get
        Set(ByVal value As String)
            _valutazione = value
        End Set
    End Property
End Class
