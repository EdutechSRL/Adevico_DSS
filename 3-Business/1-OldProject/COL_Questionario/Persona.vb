Imports Microsoft.VisualBasic

Public Class Persona
    Private _id As Integer
    Private _nome As String
    Private _statistica As Statistica

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

    Public Property statistica() As Statistica
        Get
            Return _statistica
        End Get
        Set(ByVal value As Statistica)
            _statistica = value
        End Set
    End Property
End Class
