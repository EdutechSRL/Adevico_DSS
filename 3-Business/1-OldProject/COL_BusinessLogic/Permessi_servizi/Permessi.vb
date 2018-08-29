Imports COL_DataLayer
Imports System
Imports System.Runtime.Serialization
Namespace CL_permessi
    <Serializable()> Public Class Permessi
        Inherits ObjectBase

#Region "Public Property"
        Private _ID As Integer
        Private _Descrizione As String
        Private _Nome As String
        Private _Posizione As Integer
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = _ID
            End Get
            Set(ByVal Value As Integer)
                _ID = Value
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
        Public Property Nome() As String
            Get
                Nome = _Nome
            End Get
            Set(ByVal Value As String)
                _Nome = Value
            End Set
        End Property
        Public Property Posizione() As Integer
            Get
                Posizione = _Posizione
            End Get
            Set(ByVal Value As Integer)
                _Posizione = Value
            End Set
        End Property
#End Region

#Region "Metodi New"
        Sub New()
        End Sub
        Sub New(ByVal PermId As Integer, ByVal Name As String, ByVal Desc As String, ByVal Position As Integer)
            Me._Descrizione = Desc
            Me._ID = PermId
            Me._Nome = Name
            Me._Posizione = Position
        End Sub
#End Region

        Public Shared Function FindByPosizione(ByVal item As Permessi, ByVal argument As Integer) As Boolean
            Return item.Posizione = argument
        End Function
    End Class

End Namespace