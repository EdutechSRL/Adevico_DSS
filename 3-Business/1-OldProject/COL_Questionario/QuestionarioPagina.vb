Imports Microsoft.VisualBasic
Imports System.Collections.Generic

<Serializable()> Public Class QuestionarioPagina
    Private _id As Integer
    Private _dallaDomanda As Integer
    Private _allaDomanda As Integer
    Private _numeroPagina As Integer
    Private _numeroDomande As Integer
    Private _nomePagina As String
    Private _idQuestionarioMultilingua As Integer
    Private _randomOrdineDomande As Boolean
    Private _descrizione As String
    Private _domande As New list(Of Domanda)

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property dallaDomanda() As Integer
        Get
            Return _dallaDomanda
        End Get
        Set(ByVal value As Integer)
            _dallaDomanda = value
        End Set
    End Property

    Public Property allaDomanda() As Integer
        Get
            Return _allaDomanda
        End Get
        Set(ByVal value As Integer)
            _allaDomanda = value
        End Set
    End Property

    Public Property numeroDomande() As Integer
        Get
            Return _numeroDomande
        End Get
        Set(ByVal value As Integer)
            _numeroDomande = value
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

    Public Property numeroPagina() As Integer
        Get
            Return _numeroPagina
        End Get
        Set(ByVal value As Integer)
            _numeroPagina = value
        End Set
    End Property

    Public Property nomePagina() As String
        Get
            Return _nomePagina
        End Get
        Set(ByVal value As String)
            _nomePagina = value
        End Set
    End Property

    Public Property idQuestionarioMultilingua() As Integer
        Get
            Return _idQuestionarioMultilingua
        End Get
        Set(ByVal value As Integer)
            _idQuestionarioMultilingua = value
        End Set
    End Property

    Public Property randomOrdineDomande() As Boolean
        Get
            Return _randomOrdineDomande
        End Get
        Set(ByVal value As Boolean)
            _randomOrdineDomande = value
        End Set
    End Property


    Public Property domande() As list(Of Domanda)
        Get
            Return _domande
        End Get
        Set(ByVal value As list(Of Domanda))
            _domande = value
        End Set
    End Property

    Public Shared Function findPaginaBYID(ByVal listaDom As List(Of QuestionarioPagina), ByVal idPagina As String) As QuestionarioPagina

        Dim oPagina As New QuestionarioPagina
        oPagina = listaDom.Find(New PredicateWrapper(Of QuestionarioPagina, String)(idPagina, AddressOf trovaID))

        Return oPagina

    End Function

    Public Shared Function trovaID(ByVal item As QuestionarioPagina, ByVal argument As String) As Boolean

        Return item.id = argument

    End Function

End Class
