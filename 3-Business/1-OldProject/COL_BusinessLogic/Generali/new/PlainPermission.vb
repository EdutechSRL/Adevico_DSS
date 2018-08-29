Public Class PlainPermission
    Inherits ObjectBase

#Region "Public Property"
    Private _ID As Integer
    Private _Descrizione As String
    Private _Nome As String
    Private _Posizione As Integer
#End Region

#Region "Public Property"
    Public Property Id() As Integer
        Get
            Id = _ID
        End Get
        Set(ByVal Value As Integer)
            _ID = Value
        End Set
    End Property
    Public Property Description() As String
        Get
            Description = _Descrizione
        End Get
        Set(ByVal Value As String)
            _Descrizione = Value
        End Set
    End Property
    Public Property Name() As String
        Get
            Name = _Nome
        End Get
        Set(ByVal Value As String)
            _Nome = Value
        End Set
    End Property
    Public Property Value() As Integer
        Get
            Value = _Posizione
        End Get
        Set(ByVal Value As Integer)
            _Posizione = Value
        End Set
    End Property
#End Region

#Region "Metodi New"
    Sub New()
    End Sub

#End Region


End Class