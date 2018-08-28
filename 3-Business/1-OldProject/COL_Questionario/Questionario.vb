Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

<Serializable()> Public Class Questionario
#Region "private properties"

    Private _id As Integer
    Private _nome As String
    Private _descrizione As String
    Private _dataCreazione As String
    Private _idPersonaCreator As Integer
    Private _idGruppo As Integer
    Private _idQuestionarioMultilingua As Integer
    Private _idLingua As Integer
    Private _idLinguaDefault As Integer
    Private _isCancellato As Boolean
    Private _url As String
    Private _tipo As Integer
    Private _dataInizio As String
    Private _dataFine As String
    Private _isBloccato As Boolean
    Private _isReadOnly As Boolean 'le domande associate al questionario sono in sola lettura
    Private _isDefault As Boolean
    Private _tipoGrafico As Integer
    Private _pagine As New List(Of QuestionarioPagina)
    Private _domande As New List(Of Domanda)
    Private _rispostaQuest As New RispostaQuestionario
    Private _risposteQuestionario As New List(Of RispostaQuestionario)
    Private _pesoTotale As Integer
    Private _scalaValutazione As Integer
    Private _linguePresenti As New List(Of Lingua)
    Private _durata As New Integer
    Private _creator As String
    Private _editor As String
    Private _stato As String
    Private _forUtentiComunita As Boolean
    Private _forUtentiPortale As Boolean
    Private _forUtentiEsterni As Boolean
    Private _forUtentiInvitati As Boolean
    Private _risultatiAnonimi As Boolean
    Private _visualizzaRisposta As Boolean
    Private _visualizzaCorrezione As Boolean
    Private _visualizzaSuggerimenti As Boolean
    Private _editaRisposta As Boolean
    Private _idPersonaEditor As Integer
    Private _dataModifica As String
    Private _isRandomOrder As Boolean
    Private _isPassword As Boolean
    Private _librerie As New List(Of LibreriaQuestionario)
    Private _nDomandeDiffBassa As Integer
    Private _nDomandeDiffMedia As Integer
    Private _nDomandeDiffAlta As Integer
    Private _nDomandeTotali As Integer
    Private _isRandomOrder_Options As Boolean
    Private _nQuestionsPerPage As Integer = RootObject.nQuestionsPerPage_Default
    Private _idFiglio As Integer
    Private _idDestinatario_Persona As Integer
    Private _idDestinatario_UtenteInvitato As Integer
    Private _password As String
    Private _quanteRisposte As Integer
    Private _ownerType As COL_BusinessLogic_v2.OwnerType_enum
    Private _ownerId As Int64 'attenzione al rischio di conversioni implicite...
    Private _ownerGUID As Guid
#End Region

#Region "Public Access to private Properties"
    Public Shared ReadOnly Property IconPath As String
        Get
            Return RootObject.quizIconPath
        End Get
    End Property
    Public ReadOnly Property isVisibleDurata() As Boolean
        Get
            Select Case _tipo
                Case TipoQuestionario.Autovalutazione
                    Return False
                Case TipoQuestionario.Meeting
                    Return False
                Case Else
                    Return True
            End Select
        End Get
    End Property
    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property
    Public Property idFiglio() As Integer
        Get
            Return _idFiglio
        End Get
        Set(ByVal value As Integer)
            _idFiglio = value
        End Set
    End Property
    Public Property nRisposte() As Integer
        Get
            Return _quanteRisposte
        End Get
        Set(ByVal value As Integer)
            _quanteRisposte = value
        End Set
    End Property
    Public Property password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property
    Public Property idDestinatario_Persona() As Integer
        Get
            Return _idDestinatario_Persona
        End Get
        Set(ByVal value As Integer)
            _idDestinatario_Persona = value
        End Set
    End Property
    Public Property idDestinatario_UtenteInvitato() As Integer
        Get
            Return _idDestinatario_UtenteInvitato
        End Get
        Set(ByVal value As Integer)
            _idDestinatario_UtenteInvitato = value
        End Set
    End Property
    Public Property nQuestionsPerPage() As Integer
        Get
            If _nQuestionsPerPage = 0 Then
                Return RootObject.nQuestionsPerPage_Default
            Else
                Return _nQuestionsPerPage
            End If
        End Get
        Set(ByVal value As Integer)
            _nQuestionsPerPage = value
        End Set
    End Property
    Public Property url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property
    Public Property creator() As String
        Get
            Return _creator
        End Get
        Set(ByVal value As String)
            _creator = value
        End Set
    End Property
    Public Property editor() As String
        Get
            Return _editor
        End Get
        Set(ByVal value As String)
            _editor = value
        End Set
    End Property
    Public Property stato() As String
        Get
            Return _stato
        End Get
        Set(ByVal value As String)
            _stato = value
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
    Public Property dataFine() As String
        Get
            Return _dataFine
        End Get
        Set(ByVal value As String)
            _dataFine = value
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
    Public Property idPersonaCreator() As Integer
        Get
            Return _idPersonaCreator
        End Get
        Set(ByVal value As Integer)
            _idPersonaCreator = value
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
    Public Property idGruppo() As Integer
        Get
            Return _idGruppo
        End Get
        Set(ByVal value As Integer)
            _idGruppo = value
        End Set
    End Property
    Public Property isBloccato() As Boolean
        Get
            Return _isBloccato
        End Get
        Set(ByVal value As Boolean)
            _isBloccato = value
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
    Public Property isDefault() As Boolean
        Get
            Return _isDefault
        End Get
        Set(ByVal value As Boolean)
            _isDefault = value
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
    Public Property nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
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
    Public Property tipo() As Integer
        Get
            Return _tipo
        End Get
        Set(ByVal value As Integer)
            _tipo = value
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
    Public Property idLinguaDefault() As Integer
        Get
            Return _idLinguaDefault
        End Get
        Set(ByVal value As Integer)
            _idLinguaDefault = value
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
    Public Property pagine() As List(Of QuestionarioPagina)
        Get
            Return _pagine
        End Get
        Set(ByVal value As List(Of QuestionarioPagina))
            _pagine = value
        End Set
    End Property
    Public Property domande() As List(Of Domanda)
        Get
            Return _domande
        End Get
        Set(ByVal value As List(Of Domanda))
            _domande = value
        End Set
    End Property
    Public Property rispostaQuest() As RispostaQuestionario
        Get
            Return _rispostaQuest
        End Get
        Set(ByVal value As RispostaQuestionario)
            _rispostaQuest = value
        End Set
    End Property
    Public Property risposteQuestionario() As List(Of RispostaQuestionario)
        Get
            Return _risposteQuestionario
        End Get
        Set(ByVal value As List(Of RispostaQuestionario))
            _risposteQuestionario = value
        End Set
    End Property
    Public Property pesoTotale() As Integer
        Get
            Return _pesoTotale
        End Get
        Set(ByVal value As Integer)
            _pesoTotale = value
        End Set
    End Property
    Public Property scalaValutazione() As Integer
        Get
            Return _scalaValutazione
        End Get
        Set(ByVal value As Integer)
            _scalaValutazione = value
        End Set
    End Property
    Private _MinScore As Integer
    Private _MaxAttempts As Integer
    Private _DisplayScoreToUser As Boolean
    Private _DisplayAttemptScoreToUser As Boolean
    Private _DisplayResultsStatus As Boolean
    Private _DisplayAvailableAttempts As Boolean
    Private _DisplayCurrentAttempts As Boolean
    Private _DisplayNotPassedScoreToUser As Boolean
    Public Overridable Property MinScore As Integer
        Get
            Return _MinScore
        End Get
        Set(value As Integer)
            _MinScore = value
        End Set
    End Property
    Public Overridable Property MaxAttempts As Integer
        Get
            Return _MaxAttempts
        End Get
        Set(value As Integer)
            _MaxAttempts = value
        End Set
    End Property
    Public Overridable Property DisplayScoreToUser As Boolean
        Get
            Return _DisplayScoreToUser
        End Get
        Set(value As Boolean)
            _DisplayScoreToUser = value
        End Set
    End Property
    Public Overridable Property DisplayAttemptScoreToUser As Boolean
        Get
            Return _DisplayAttemptScoreToUser
        End Get
        Set(value As Boolean)
            _DisplayAttemptScoreToUser = value
        End Set
    End Property
    Public Overridable Property DisplayAvailableAttempts As Boolean
        Get
            Return _DisplayAvailableAttempts
        End Get
        Set(value As Boolean)
            _DisplayAvailableAttempts = value
        End Set
    End Property
    Public Overridable Property DisplayResultsStatus As Boolean
        Get
            Return _DisplayResultsStatus
        End Get
        Set(value As Boolean)
            _DisplayResultsStatus = value
        End Set
    End Property
    Public Overridable Property DisplayCurrentAttempts As Boolean
        Get
            Return _DisplayCurrentAttempts
        End Get
        Set(value As Boolean)
            _DisplayCurrentAttempts = value
        End Set
    End Property

    Public Property linguePresenti() As List(Of Lingua)
        Get
            Return _linguePresenti
        End Get
        Set(ByVal value As List(Of Lingua))
            _linguePresenti = value
        End Set
    End Property
    Public Property durata() As Integer
        Get
            Return _durata
        End Get
        Set(ByVal value As Integer)
            _durata = value
        End Set
    End Property
    Public Property forUtentiComunita() As Boolean
        Get
            Return _forUtentiComunita
        End Get
        Set(ByVal value As Boolean)
            _forUtentiComunita = value
        End Set
    End Property
    Public Property forUtentiPortale() As Boolean
        Get
            Return _forUtentiPortale
        End Get
        Set(ByVal value As Boolean)
            _forUtentiPortale = value
        End Set
    End Property
    Public Property forUtentiEsterni() As Boolean
        Get
            Return _forUtentiEsterni
        End Get
        Set(ByVal value As Boolean)
            _forUtentiEsterni = value
        End Set
    End Property
    Public Property forUtentiInvitati() As Boolean
        Get
            Return _forUtentiInvitati
        End Get
        Set(ByVal value As Boolean)
            _forUtentiInvitati = value
        End Set
    End Property
    Public Property risultatiAnonimi() As Boolean
        Get
            Return _risultatiAnonimi
        End Get
        Set(ByVal value As Boolean)
            _risultatiAnonimi = value
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
    Public Property dataModifica() As String
        Get
            Return _dataModifica
        End Get
        Set(ByVal value As String)
            _dataModifica = value
        End Set
    End Property
    Public Property isRandomOrder() As Boolean
        Get
            Return _isRandomOrder
        End Get
        Set(ByVal value As Boolean)
            _isRandomOrder = value
        End Set
    End Property
    Public Property isPassword() As Boolean
        Get
            Return _isPassword
        End Get
        Set(ByVal value As Boolean)
            _isPassword = value
        End Set
    End Property
    Public Property isRandomOrder_Options() As Boolean
        Get
            Return _isRandomOrder_Options
        End Get
        Set(ByVal value As Boolean)
            _isRandomOrder_Options = value
        End Set
    End Property
    Public Property ownerType() As COL_BusinessLogic_v2.OwnerType_enum
        Get
            Return _ownerType
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.OwnerType_enum)
            _ownerType = value
        End Set
    End Property
    Public Property ownerId() As Int64
        Get
            Return _ownerId
        End Get
        Set(ByVal value As Int64)
            _ownerId = value
        End Set
    End Property
    Public Property ownerGUID() As Guid
        Get
            Return _ownerGUID
        End Get
        Set(ByVal value As Guid)
            _ownerGUID = value
        End Set
    End Property
    Public Property visualizzaCorrezione() As Boolean
        Get
            Return _visualizzaCorrezione
        End Get
        Set(ByVal value As Boolean)
            _visualizzaCorrezione = value
        End Set
    End Property
    Public Property visualizzaSuggerimenti() As Boolean
        Get
            Return _visualizzaSuggerimenti
        End Get
        Set(ByVal value As Boolean)
            _visualizzaSuggerimenti = value
        End Set
    End Property
    Public Property editaRisposta() As Boolean
        Get
            Return _editaRisposta
        End Get
        Set(ByVal value As Boolean)
            _editaRisposta = value
        End Set
    End Property
    Public Property librerieQuestionario() As List(Of LibreriaQuestionario)
        Get
            Return _librerie
        End Get
        Set(ByVal value As List(Of LibreriaQuestionario))
            _librerie = value
        End Set
    End Property
    Public Property nDomandeDiffBassa() As Integer
        Get
            Return _nDomandeDiffBassa
        End Get
        Set(ByVal value As Integer)
            _nDomandeDiffBassa = value
        End Set
    End Property
    Public Property nDomandeDiffMedia() As Integer
        Get
            Return _nDomandeDiffMedia
        End Get
        Set(ByVal value As Integer)
            _nDomandeDiffMedia = value
        End Set
    End Property
    Public Property nDomandeDiffAlta() As Integer
        Get
            Return _nDomandeDiffAlta
        End Get
        Set(ByVal value As Integer)
            _nDomandeDiffAlta = value
        End Set
    End Property
#End Region
#Region "Calculated public properties"
    Public Property visualizzaRisposta() As Boolean
        Get
            Return (_visualizzaRisposta Or _editaRisposta Or _visualizzaCorrezione)
        End Get
        Set(ByVal value As Boolean)
            _visualizzaRisposta = value
        End Set
    End Property
    Public ReadOnly Property isPrimaRisposta() As Boolean
        Get
            If Not _rispostaQuest Is Nothing Then
                If RootObject.setNullDate(_rispostaQuest.dataInizio) Is System.DBNull.Value Then
                    'If CDate(_dataFine) < Now Or CDate(_dataInizio) > Now Then 'se il questionario e' scaduto non si possono aggiungere risposte
                    '    Return False
                    'Else

                    Return True
                    'End If
                Else
                    If Not RootObject.setNullDate(_rispostaQuest.dataFine) Is System.DBNull.Value Then
                        Return False
                    Else
                        If durata > 0 Then
                            'se e' a tempo devo controllare che il tempo non sia scaduto
                            If Now > CDate(RootObject.setNullDate(_rispostaQuest.dataInizio)).AddMinutes(durata).AddSeconds(RootObject.maxOvertimeSalvataggio) Then
                                Return False
                            Else
                                If CDate(_dataFine) < CDate(_rispostaQuest.dataInizio) Or CDate(_dataInizio) > CDate(_rispostaQuest.dataInizio) Then
                                    'se il questionario e' scaduto non si possono aggiungere risposte, ma se l'ho cominciato lo posso finire
                                    Return False
                                Else
                                    Return True
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Return True
        End Get
    End Property

    Public ReadOnly Property nDomandeTotali() As Integer
        Get
            Return nDomandeDiffBassa + nDomandeDiffMedia + nDomandeDiffAlta
        End Get
    End Property
#End Region
#Region "enum"
    Public Enum TipoQuestionario
        Compilabili = -1
        Questionario = 0
        LibreriaDiDomande = 1
        Sondaggio = 2
        Modello = 3
        Random = 4
        Autovalutazione = 5
        Meeting = 6
        RandomRepeat = 7
        'EPQuestionario = 8
        'EPRandom = 16
        'EPAutovalutazione = 32
    End Enum
    Public Enum StatoQuestionario
        NonCompilato = 0
        Incompleto = 1
        Compilato = 2
    End Enum
    Public Enum Destinatari
        UtentiComunita = 0
        UtentiPortale = 1
        UtentiEsterni = 2
        UtentiInvitati = 3
    End Enum

#End Region

#Region "find & remove"
    Public Shared Function findQuestionarioBYIDMultilingua(ByVal listaDom As List(Of Questionario), ByVal idML As String) As Boolean

        Dim oLib As Questionario
        oLib = listaDom.Find(New PredicateWrapper(Of Questionario, String)(idML, AddressOf trovaID))

        If Not oLib Is Nothing Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Shared Function findQuestionarioBYID(ByVal listaDom As List(Of Questionario), ByVal id As String) As Questionario

        Dim oLib As Questionario
        oLib = listaDom.Find(New PredicateWrapper(Of Questionario, String)(id, AddressOf trovaID))

        Return oLib
        'If Not oLib Is Nothing Then
        '    Return True
        'Else
        '    Return False
        'End If

    End Function
    Public Shared Function removeQuestionariBYIDMultilingua(ByVal listaDom As List(Of Questionario), ByVal idML As String) As Integer

        Dim i As Integer
        i = listaDom.RemoveAll(New PredicateWrapper(Of Questionario, String)(idML, AddressOf trovaIDMultilingua))

        Return i

    End Function
    Public Shared Function removeQuestionariBYID(ByVal listaDom As List(Of Questionario), ByVal idQuest As String) As Integer

        Dim i As Integer
        i = listaDom.RemoveAll(New PredicateWrapper(Of Questionario, String)(idQuest, AddressOf trovaID))

        Return i

    End Function
    Public Shared Function trovaID(ByVal item As Questionario, ByVal argument As String) As Boolean

        Return item.id = argument

    End Function
    Public Shared Function trovaIDMultilingua(ByVal item As Questionario, ByVal argument As String) As Boolean

        Return item.idQuestionarioMultilingua = argument

    End Function
#End Region

#Region "new"


    Public Sub New()
        ownerType = COL_BusinessLogic_v2.OwnerType_enum.None
        ownerGUID = Nothing
        ownerId = 0
    End Sub
    'Public Sub New(ByVal communityId As Integer)
    '    ownerType = COL_BusinessLogic_v2.OwnerType_enum.None
    '    ownerGUID = Nothing
    '    ownerId = 0
    '    idGruppo = 
    'End Sub
    Public Sub New(ByVal questId As Integer)
        id = questId
    End Sub
#End Region

End Class
