<Serializable()> Public Class UtenteInvitato

#Region "Private Property"
	Private _ID As Integer
    Private _QuestionarioID As Integer
    Private _IdRandomQuestionnaire As Integer
	Private _Nome As String
	Private _Cognome As String
	Private _Mail As String
	Private _PersonaID As Integer
    Private _Descrizione As String
    Private _Password As String
    Private _Attributo3 As String
    Private _statistica As Statistica
    Private _isSelezionato As Boolean
    Private _RispostaID As Integer
    Private _Url As String
    Private _AttemptNumber As Integer
    Private _ModifyedOn As DateTime
#End Region

#Region "Public Property"
    Public Property ID() As Integer
        Get
            ID = _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
    Public Property PersonaID() As Integer
        Get
            PersonaID = _PersonaID
        End Get
        Set(ByVal value As Integer)
            _PersonaID = value
        End Set
    End Property
    Public Property QuestionarioID() As Integer
        Get
            QuestionarioID = _QuestionarioID
        End Get
        Set(ByVal value As Integer)
            _QuestionarioID = value
        End Set
    End Property
    Public Property IdRandomQuestionnaire() As Integer
        Get
            Return _IdRandomQuestionnaire
        End Get
        Set(ByVal value As Integer)
            _IdRandomQuestionnaire = value
        End Set
    End Property
    Public Property RispostaID() As Integer
        Get
            RispostaID = _RispostaID
        End Get
        Set(ByVal value As Integer)
            _RispostaID = value
        End Set
    End Property

    Public Property Nome() As String
        Get
            Nome = _Nome
        End Get
        Set(ByVal Value As String)
            _Nome = Value
        End Set
    End Property

    Public Property Url() As String
        Get
            Url = _Url
        End Get
        Set(ByVal Value As String)
            _Url = Value
        End Set
    End Property

    Public Property Cognome() As String
        Get
            Cognome = _Cognome
        End Get
        Set(ByVal Value As String)
            _Cognome = Value
        End Set
    End Property
    Public ReadOnly Property Anagrafica() As String
        Get
            Anagrafica = _Nome & " " & _Cognome
        End Get
    End Property
    Public Property Mail() As String
        Get
            Mail = _Mail
        End Get
        Set(ByVal Value As String)
            _Mail = Value
        End Set
    End Property

    Public Property isSelezionato() As Boolean
        Get
            isSelezionato = _isSelezionato
        End Get
        Set(ByVal value As Boolean)
            _isSelezionato = value
        End Set
    End Property

    Public Property Descrizione() As String
        Get
            Descrizione = _Descrizione
        End Get
        Set(ByVal Value As String)
            _Descrizione = Value
        End Set
    End Property
    Public Property Password() As String
        Get
            Password = _Password
        End Get
        Set(ByVal Value As String)
            _Password = Value
        End Set
    End Property
    Public Property Attributo3() As String
        Get
            Attributo3 = _Attributo3
        End Get
        Set(ByVal Value As String)
            _Attributo3 = Value
        End Set
    End Property

    Public Property statistica() As Statistica
        Get
            Return _statistica
        End Get
        Set(ByVal value As Statistica)
            _statistica = value
        End Set
    End Property
    Public Property AttemptNumber() As Integer
        Get
            Return _AttemptNumber
        End Get
        Set(ByVal Value As Integer)
            _AttemptNumber = Value
        End Set
    End Property
    Public Property ModifyedOn() As DateTime
        Get
            Return _ModifyedOn
        End Get
        Set(ByVal Value As DateTime)
            _ModifyedOn = Value
        End Set
    End Property

#End Region

    Public Sub New()
        _ID = 0
        _PersonaID = 0
    End Sub
    Public Sub New(ByVal ID As Integer)
        _ID = ID
        _PersonaID = 0
    End Sub
    Public Sub New(ByVal ID As Integer, ByVal QuestionarioID As Integer, ByVal Nome As String, ByVal Cognome As String, ByVal Mail As String, _
      ByVal Attributo1 As String, ByVal Attributo2 As String, ByVal Attributo3 As String)
        Me._ID = ID
        Me._Nome = Nome
        Me._Cognome = Cognome
        Me._Mail = Mail
        Me._Descrizione = Descrizione
        Me._Password = Password
        Me._Attributo3 = Attributo3
        Me._PersonaID = 0
        Me._QuestionarioID = QuestionarioID
    End Sub
    Public Sub New(ByVal ID As Integer, ByVal QuestionarioID As Integer, ByVal Nome As String, ByVal Cognome As String, ByVal Mail As String, _
      ByVal Attributo1 As String, ByVal Attributo2 As String, ByVal Attributo3 As String, ByVal PersonaID As Integer)
        Me._ID = ID
        Me._Nome = Nome
        Me._Cognome = Cognome
        Me._Mail = Mail
        Me._Descrizione = Descrizione
        Me._Password = Password
        Me._Attributo3 = Attributo3
        Me._PersonaID = PersonaID
        Me._QuestionarioID = QuestionarioID
    End Sub
    Public Sub New(ByVal ID As Integer, ByVal QuestionarioID As Integer, ByVal Nome As String, ByVal Cognome As String, ByVal Mail As String, _
      ByVal PersonaID As Integer)
        Me._ID = ID
        Me._Nome = Nome
        Me._Cognome = Cognome
        Me._Mail = Mail
        Me._Descrizione = ""
        Me._Password = ""
        Me._Attributo3 = ""
        Me._PersonaID = PersonaID
        Me._QuestionarioID = QuestionarioID
    End Sub

    '' recupero l'invito dal DB e restituisco le info relative
    'Public Shared Function GetInvitoByID(ByVal InvitoID) As UtenteInvitato
    '    Dim oInvito As New UtenteInvitato

    '    Return oInvito
    'End Function
    Public Shared Function removeUtentiNonSelezionati(ByVal listaDom As List(Of UtenteInvitato)) As Integer

        Dim i As Integer
        i = listaDom.RemoveAll(New PredicateWrapper(Of UtenteInvitato, Boolean)(False, AddressOf trovaSelezionati))

        Return i

    End Function
    Public Shared Function trovaSelezionati(ByVal item As UtenteInvitato, ByVal argument As Boolean) As Boolean

        Return item.isSelezionato = argument

    End Function
    Public Shared Function findUtenteBYID(ByVal listaRisp As List(Of UtenteInvitato), ByVal idUtente As String) As UtenteInvitato

        Dim oRis As New UtenteInvitato
        oRis = listaRisp.Find(New PredicateWrapper(Of UtenteInvitato, String)(idUtente, AddressOf trovaIDUtente))

        Return oRis
    End Function
    Public Shared Function trovaIDUtente(ByVal item As UtenteInvitato, ByVal argument As String) As Boolean

        Return item.ID = argument

    End Function
    'Public Shared Function findUtenteBYIDPersona(ByVal listaRisp As List(Of UtenteInvitato), ByVal idPersona As String) As UtenteInvitato

    '    Dim oRis As New UtenteInvitato
    '    oRis = listaRisp.Find(New PredicateWrapper(Of UtenteInvitato, String)(idPersona, AddressOf trovaIDPersona))

    '    Return oRis
    'End Function
    'Public Shared Function trovaIDPersona(ByVal item As UtenteInvitato, ByVal argument As String) As Boolean

    '    Return item.PersonaID = argument

    'End Function

End Class