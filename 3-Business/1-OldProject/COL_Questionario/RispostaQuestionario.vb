Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

<Serializable()> Public Class RispostaQuestionario
    <Serializable()> Public Class PosizioneDomanda
        Private _numeroPagina As Integer
        Private _numero As Integer
        Public Property numero() As Integer
            Get
                Return _numero
            End Get
            Set(ByVal value As Integer)
                _numero = value
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
    End Class
    Private _id As Integer
    Private _idPersona As Integer
    Private _idQuestionario As Integer
    Private _idUtenteInvitato As Integer
    Private _indirizzoIPStart As String
    Private _indirizzoIPEdit As String
    Private _indirizzoIPEnd As String
    Private _dataInizio As String
    Private _dataModifica As String
    Private _ultimaRisposta As Integer
    Private _dataFine As String
    Private _risposteDomande As New List(Of RispostaDomanda)
    Private _idQuestionarioRandom As Integer
    Private _oStatistica As New Statistica
    Private _domandeNoRisp As New List(Of PosizioneDomanda)

    Public Property domandeNoRisp() As List(Of PosizioneDomanda)
        Get
            Return _domandeNoRisp
        End Get
        Set(ByVal value As List(Of PosizioneDomanda))
            _domandeNoRisp = value
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
    Public Property idQuestionario() As Integer
        Get
            Return _idQuestionario
        End Get
        Set(ByVal value As Integer)
            _idQuestionario = value
        End Set
    End Property
    Public Property idQuestionarioRandom() As Integer
        Get
            Return _idQuestionarioRandom
        End Get
        Set(ByVal value As Integer)
            _idQuestionarioRandom = value
        End Set
    End Property
    Public Property idUtenteInvitato() As Integer
        Get
            Return _idUtenteInvitato
        End Get
        Set(ByVal value As Integer)
            _idUtenteInvitato = value
        End Set
    End Property
    Public Property indirizzoIPStart() As String
        Get
            Return _indirizzoIPStart
        End Get
        Set(ByVal value As String)
            _indirizzoIPStart = value
        End Set
    End Property
    Public Property indirizzoIPEdit() As String
        Get
            Return _indirizzoIPEdit
        End Get
        Set(ByVal value As String)
            _indirizzoIPEdit = value
        End Set
    End Property
    Public Property indirizzoIPEnd() As String
        Get
            Return _indirizzoIPEnd
        End Get
        Set(ByVal value As String)
            _indirizzoIPEnd = value
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
    Public Property dataInizio() As String
        Get
            Return _dataInizio
        End Get
        Set(ByVal value As String)
            _dataInizio = value
        End Set
    End Property
    Public Property ultimaRisposta() As Integer
        Get
            Return _ultimaRisposta
        End Get
        Set(ByVal value As Integer)
            _ultimaRisposta = value
        End Set
    End Property
    Public Property dataFine() As String
        Get
            Return _dataFine
        End Get
        Set(ByVal value As String)
            _dataFine = value
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
    Public Property risposteDomande() As List(Of RispostaDomanda)
        Get
            Return _risposteDomande
        End Get
        Set(ByVal value As List(Of RispostaDomanda))
            _risposteDomande = value
        End Set
    End Property
    Public Property oStatistica() As Statistica
        Get
            Return _oStatistica
        End Get
        Set(ByVal value As Statistica)
            _oStatistica = value
        End Set
    End Property

    Public Function trovaID(ByVal item As RispostaDomanda, ByVal argument As String) As Boolean

        Return item.id = argument

    End Function
    Public Function trovaIDDomanda(ByVal item As RispostaDomanda, ByVal argument As String) As Boolean

        Return item.idDomanda = argument

    End Function
    Public Function findRispostaByIDPersona(ByVal listaRisp As List(Of RispostaQuestionario), ByVal idPersona As String) As RispostaQuestionario

        Dim oRis As New RispostaQuestionario
        oRis = listaRisp.Find(New PredicateWrapper(Of RispostaQuestionario, String)(idPersona, AddressOf trovaIDPersona))

        Return oRis
    End Function
    Public Function trovaIDPersona(ByVal item As RispostaQuestionario, ByVal argument As String) As Boolean

        Return item.idPersona = argument

    End Function
    Public Function trovaIDPersona(ByVal item As Persona, ByVal argument As String) As Boolean

        Return item.id = argument

    End Function
    Public Function trovaIDUtenteInvitato(ByVal item As UtenteInvitato, ByVal argument As String) As Boolean

        Return item.ID = argument

    End Function

    'Public Function findRisposteByIDDomandaOpzione(ByVal listaRisp As List(Of RispostaDomanda), ByVal idDomandaOpzione As String) As List(Of RispostaDomanda)

    '    listaRisp = listaRisp.FindAll(New PredicateWrapper(Of RispostaDomanda, String)(idDomandaOpzione, AddressOf trovaIDDomandaOpzione))

    '    Return listaRisp
    'End Function

    Public Function findRispostaByIDDomandaOpzione(ByVal listaRisp As List(Of RispostaDomanda), ByVal idDomandaOpzione As String) As RispostaDomanda

        Dim oRis As New RispostaDomanda
        oRis = listaRisp.Find(New PredicateWrapper(Of RispostaDomanda, String)(idDomandaOpzione, AddressOf trovaidDomandaOpzione))

        Return oRis
    End Function
    Public Function findRispostaByNumeroOpzione(ByVal listaRisp As List(Of RispostaDomanda), ByVal nDomandaOpzione As String) As RispostaDomanda

        Dim oRis As New RispostaDomanda
        oRis = listaRisp.Find(New PredicateWrapper(Of RispostaDomanda, String)(nDomandaOpzione, AddressOf trovanDomandaOpzione))

        Return oRis
    End Function
    Public Function trovanDomandaOpzione(ByVal item As RispostaDomanda, ByVal argument As String) As Boolean

        Return item.numeroOpzione = argument

    End Function
    Public Function trovanDomandaOpzioneEVal(ByVal item As RispostaDomanda, ByVal argument As String, ByVal argument2 As String) As Boolean

        Return item.numeroOpzione = argument And item.valore = argument2

    End Function
    Public Function trovaidDomandaOpzione(ByVal item As RispostaDomanda, ByVal argument As String) As Boolean

        Return item.idDomandaOpzione = argument

    End Function
    Public Function findRisposteByIDDomanda(ByVal listaRisp As List(Of RispostaDomanda), ByVal idDomanda As String) As List(Of RispostaDomanda)

        Dim oRisposta As New RispostaDomanda
        Dim oRisposte As New List(Of RispostaDomanda)

        For Each oRisposta In listaRisp
            If oRisposta.idDomanda = idDomanda Then
                oRisposte.Add(oRisposta)
            End If
        Next
        Return oRisposte
    End Function
    Public Function findRisposteByNumeroDomanda(ByVal listaRisp As List(Of RispostaDomanda), ByVal nDomanda As String) As List(Of RispostaDomanda)

        Dim oRisposta As New RispostaDomanda
        Dim oRisposte As New List(Of RispostaDomanda)

        For Each oRisposta In listaRisp
            If oRisposta.numeroOpzione = nDomanda Then
                oRisposte.Add(oRisposta)
            End If
        Next
        Return oRisposte
    End Function
    Public Function findRisposteDomanda(ByVal listaRisp As List(Of RispostaQuestionario), ByVal odomanda As Domanda) As List(Of RispostaDomanda)

        Dim listaRispDomanda As New List(Of RispostaDomanda)
        For Each rispQ As RispostaQuestionario In listaRisp
            For Each risp As RispostaDomanda In rispQ.risposteDomande
                Select Case odomanda.tipo
                    Case Domanda.TipoDomanda.Multipla
                        For Each odomOpzione As DomandaOpzione In odomanda.domandaMultiplaOpzioni
                            If risp.idDomandaOpzione = odomOpzione.id Then
                                listaRispDomanda.Add(risp)
                            End If
                        Next

                    Case Domanda.TipoDomanda.Rating
                        For Each odomOpzione As DomandaOpzione In odomanda.domandaRating.opzioniRating
                            If risp.idDomandaOpzione = odomOpzione.id Then
                                listaRispDomanda.Add(risp)
                            End If
                        Next

                    Case Domanda.TipoDomanda.TestoLibero
                        For Each odomOpzione As DomandaTestoLibero In odomanda.opzioniTestoLibero
                            If risp.idDomandaOpzione = odomOpzione.id Then
                                listaRispDomanda.Add(risp)
                            End If
                        Next

                    Case Domanda.TipoDomanda.Numerica
                        For Each odomOpzione As DomandaNumerica In odomanda.opzioniNumerica
                            If risp.idDomandaOpzione = odomOpzione.id Then
                                listaRispDomanda.Add(risp)
                            End If
                        Next
                End Select

            Next
        Next

        Return listaRispDomanda
    End Function
    Public Function findRisposteLibere(ByVal oquest As Questionario, ByVal oDomanda As Domanda, ByRef idDomandaOpzione As Integer) As List(Of RispostaDomanda)

        Dim listaRispDomanda As New List(Of RispostaDomanda)

        For Each risp As RispostaDomanda In oDomanda.risposteDomanda
            Select Case oDomanda.tipo
                Case Domanda.TipoDomanda.Multipla
                    For Each odomOpzione As DomandaOpzione In oDomanda.domandaMultiplaOpzioni
                        If odomOpzione.id = idDomandaOpzione And odomOpzione.numero = risp.numeroOpzione And odomOpzione.isAltro And risp.testoOpzione <> String.Empty Then
                            listaRispDomanda.Add(risp)
                        End If
                    Next

                Case Domanda.TipoDomanda.Rating
                    For Each odomOpzione As DomandaOpzione In oDomanda.domandaRating.opzioniRating
                        If odomOpzione.id = idDomandaOpzione And odomOpzione.numero = risp.numeroOpzione And odomOpzione.isAltro And risp.testoOpzione <> String.Empty Then
                            listaRispDomanda.Add(risp)
                        End If
                    Next

                Case Domanda.TipoDomanda.TestoLibero
                    For Each odomOpzione As DomandaTestoLibero In oDomanda.opzioniTestoLibero
                        If odomOpzione.id = idDomandaOpzione And risp.idDomandaOpzione = odomOpzione.id Then
                            listaRispDomanda.Add(risp)
                        End If
                    Next

                Case Domanda.TipoDomanda.Numerica
                    For Each odomOpzione As DomandaNumerica In oDomanda.opzioniNumerica
                        If odomOpzione.id = idDomandaOpzione And risp.numeroOpzione = odomOpzione.numero Then
                            listaRispDomanda.Add(risp)
                        End If
                    Next
            End Select

        Next

        Return listaRispDomanda
    End Function
    Public Function findPersonaBYID(ByVal listaRisp As List(Of Persona), ByVal idPersona As String) As Persona

        Dim oRis As New Persona
        oRis = listaRisp.Find(New PredicateWrapper(Of Persona, String)(idPersona, AddressOf trovaIDPersona))

        Return oRis
    End Function
    Public Function findUtenteInvitatoBYID(ByVal listaRisp As List(Of UtenteInvitato), ByVal idPersona As String) As UtenteInvitato

        Dim oRis As New UtenteInvitato
        oRis = listaRisp.Find(New PredicateWrapper(Of UtenteInvitato, String)(idPersona, AddressOf trovaIDUtenteInvitato))

        Return oRis
    End Function
    Public Function findUtenteInvitatoBYIDPersona(ByVal listaRisp As List(Of UtenteInvitato), ByVal idPersona As String) As UtenteInvitato

        Dim oRis As New UtenteInvitato
        oRis = listaRisp.Find(New PredicateWrapper(Of UtenteInvitato, String)(idPersona, AddressOf trovaUtenteInvitatoIDPersona))

        Return oRis
    End Function
    Public Function findUtenteInvitatoBYIDRisposta(ByVal listaRisp As List(Of UtenteInvitato), ByVal idRisposta As String) As UtenteInvitato

        Dim oRis As New UtenteInvitato
        oRis = listaRisp.Find(New PredicateWrapper(Of UtenteInvitato, String)(idRisposta, AddressOf trovaUtenteInvitatoIDRisposta))

        Return oRis
    End Function
    Public Function trovaUtenteInvitatoIDPersona(ByVal item As UtenteInvitato, ByVal argument As String) As Boolean
        Return item.PersonaID = argument
    End Function
    Public Function trovaUtenteInvitatoIDRisposta(ByVal item As UtenteInvitato, ByVal argument As String) As Boolean
        Return item.RispostaID = argument
    End Function

End Class
