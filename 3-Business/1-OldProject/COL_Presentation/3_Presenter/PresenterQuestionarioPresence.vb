Public Class PresenterQuestionarioPresence
    Inherits GenericPresenter

    Public Sub New(ByVal view As IViewCommon)
        MyBase.view = view
    End Sub
    Private Shadows ReadOnly Property View() As IviewQuestionarioPresence
        Get
            View = MyBase.view
        End Get
    End Property

    'Public Enum OrderValue
    '    Nome = 1
    '    Cognome = 2
    '    Matricola = 3
    '    IP = 4
    '    TempoRimanente = 5
    '    UltimoSalvataggio = 6
    'End Enum
    'Public Enum OrderDir As Byte
    '    Asc = 0
    '    Desc = 1
    'End Enum

    Public Function Invalida(ByVal QuestionarioID As Guid)
        Return True
    End Function

    Public Function Ricomincia(ByVal QuestionarioID As Guid)
        Return True
    End Function

    Public Function StampaLista()
        Return True
    End Function

	''' <summary>
	'''  MODIFICATO !!!!!!!!!!!!!!!!!!!!
	''' </summary>
	''' <param name="OrderValue"></param>
	''' <param name="orderDir"></param>
	''' <returns></returns>
	''' <remarks></remarks>
    Public Function AggiornaLista(ByVal OrderValue As String, ByVal orderDir As String)
        Dim IscrittiList As New List(Of IscrittoQuestionario)

        Dim Iscritti As List(Of String)
        Iscritti = getIscritti()

        Dim oIscrittoElement As IscrittoQuestionario

        For Each Iscritto As String In Iscritti
            Dim UserActionList As IList
			'UserActionList = ManagerUserAction.GetUserActionFiltUser(CInt(Iscritto))
			'oIscrittoElement = New IscrittoQuestionario

			'With oIscrittoElement
			'    .ID = CInt(Iscritto)
			'    .Nome = Iscritto 'M
			'    .Cognome = Iscritto 'M
			'    .Matricola = Iscritto 'M
			'    .IP = ""
			'    Dim IsFirstAction As Boolean = True
			'    For Each UserAction As UserAction In UserActionList
			'        .IP &= "Client IP: " & UserAction.UserClientIP & " Proxy IP:" & UserAction.UserProxyIP & "<br />"
			'        If IsFirstAction Then
			'            .HasMultiIP = False
			'            IsFirstAction = False
			'        Else
			'            .HasMultiIP = True
			'        End If
			'    Next

			'    .TempoRimanente = Now() - Now() 'M
			'End With

            IscrittiList.Add(oIscrittoElement)
        Next

        If (Not Me.View.OrderValue = "") Then
            IscrittiList.Sort(New GenericComparer(Of MailingAddress)(Me.View.OrderValue))
        End If

        If Me.View.OrderDir Then
            IscrittiList.Reverse()
        End If

        Me.View.ListaIscritti = IscrittiList

        Return True
    End Function


    ''' <summary>
    ''' SOLO TEMPORANEA!!! VERRA' ELIMINATA PER UTILIZZARE METODI DEI QUESTIONARI PER IL RECUPERO DI UNA LISTA APPROPRIATA,
    ''' INTANTO VA BENE COSì!
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getIscritti() As IList
        Dim Iscritti As New List(Of String)
        For i As Integer = 1 To 20
            Iscritti.Add(i.ToString)
        Next

        Return Iscritti
    End Function
End Class

Public Class IscrittoQuestionario

    Private _ID As Integer
    Private _Nome As String
    Private _Cognome As String
    Private _Matricola As String
    Private _IP As String
    Private _TempoRimanente As TimeSpan
    Private _Salvataggio As String
    Private _HasMultiIP As Boolean

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
    Public Property Nome() As String
        Get
            Return _Nome
        End Get
        Set(ByVal value As String)
            _Nome = value
        End Set
    End Property
    Public Property Cognome() As String
        Get
            Return _Cognome
        End Get
        Set(ByVal value As String)
            _Cognome = value
        End Set
    End Property
    Public Property Matricola() As String
        Get
            Return _Matricola
        End Get
        Set(ByVal value As String)
            _Matricola = value
        End Set
    End Property
    Public Property IP() As String
        Get
            Return _IP
        End Get
        Set(ByVal value As String)
            _IP = value
        End Set
    End Property
    Public Property TempoRimanente() As TimeSpan
        Get
            Return _TempoRimanente
        End Get
        Set(ByVal value As TimeSpan)
            _TempoRimanente = value
        End Set
    End Property
    Public Property Salvataggio() As String
        Get
            Return _Salvataggio
        End Get
        Set(ByVal value As String)
            _Salvataggio = value
        End Set
    End Property
    Public Property HasMultiIP() As Boolean
        Get
            Return _HasMultiIP
        End Get
        Set(ByVal value As Boolean)
            _HasMultiIP = value
        End Set
    End Property

End Class