<Serializable()> Public Class LibreriaQuestionario

    Private _id As Integer
    Private _idQuestionario As Integer
    Private _idLibreria As Integer
    Private _nDomandeDiffBassa As Integer
    Private _nDomandeDiffMedia As Integer
    Private _nDomandeDiffAlta As Integer
    Private _nDomandeDiffBassaDisponibili As Integer
    Private _nDomandeDiffMediaDisponibili As Integer
    Private _nDomandeDiffAltaDisponibili As Integer
    Private _nomeLibreria As String
    Private _nDomandeTotali As Integer
    Private _idLingua As Integer


    Public Property idQuestionario() As Integer
        Get
            Return _idQuestionario
        End Get
        Set(ByVal value As Integer)
            _idQuestionario = value
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

    Public Property idLibreria() As Integer
        Get
            Return _idLibreria
        End Get
        Set(ByVal value As Integer)
            _idLibreria = value
        End Set
    End Property

    Public Property nomeLibreria() As String
        Get
            Return _nomeLibreria
        End Get
        Set(ByVal value As String)
            _nomeLibreria = value
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

    Public Property nDomandeDiffBassaDisponibili() As Integer
        Get
            Return _nDomandeDiffBassaDisponibili
        End Get
        Set(ByVal value As Integer)
            _nDomandeDiffBassaDisponibili = value
        End Set
    End Property

    Public Property nDomandeDiffMediaDisponibili() As Integer
        Get
            Return _nDomandeDiffMediaDisponibili
        End Get
        Set(ByVal value As Integer)
            _nDomandeDiffMediaDisponibili = value
        End Set
    End Property

    Public Property nDomandeDiffAltaDisponibili() As Integer
        Get
            Return _nDomandeDiffAltaDisponibili
        End Get
        Set(ByVal value As Integer)
            _nDomandeDiffAltaDisponibili = value
        End Set
    End Property

    Public ReadOnly Property nDomandeTotali() As Integer
        Get
            Return _nDomandeDiffBassa + _nDomandeDiffMedia + _nDomandeDiffAlta
        End Get
    End Property

    Public Property idLingua() As Integer
        Get
            Return _idLingua
        End Get
        Set(ByVal value As Integer)
            _idLingua = value
        End Set
    End Property

    Public Shared Function removeLibreriaQuestionariBYIDLibreria(ByVal listaDom As List(Of LibreriaQuestionario), ByVal idLibreria As String) As Integer
        Dim i As Integer
        i = listaDom.RemoveAll(New PredicateWrapper(Of LibreriaQuestionario, String)(idLibreria, AddressOf trovaIDLibreria))

        Return i

    End Function

    Public Shared Function trovaIDLibreria(ByVal item As LibreriaQuestionario, ByVal argument As String) As Boolean

        Return item.idLibreria = argument

    End Function
End Class