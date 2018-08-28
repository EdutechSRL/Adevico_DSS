Imports Microsoft.VisualBasic

<Serializable()> Public Class TipoGrafico

    Private _id As Integer
    Private _nome As String
    Private _descrizione As String
    Private _icona As String


    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
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

    Public Property icona() As String
        Get
            Return _icona
        End Get
        Set(ByVal value As String)
            _icona = value
        End Set
    End Property

End Class
