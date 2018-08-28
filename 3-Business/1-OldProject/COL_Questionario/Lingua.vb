'Imports Microsoft.VisualBasic

'<Serializable()> Public Class Lingua

'    Private _id As Integer
'    Private _nome As String
'    Private _nomeQuestionario As String
'    Private _codice As String

'    Public Property id() As Integer
'        Get
'            Return _id
'        End Get
'        Set(ByVal value As Integer)
'            _id = value
'        End Set
'    End Property

'    Public Property nome() As String
'        Get
'            Return _nome
'        End Get
'        Set(ByVal value As String)
'            _nome = value
'        End Set
'    End Property

'    Public Property codice() As String
'        Get
'            Return _codice
'        End Get
'        Set(ByVal value As String)
'            _codice = value
'        End Set
'    End Property

'    ReadOnly Property sigla() As String
'        Get
'            Return _codice.Substring(0, 2).ToUpperInvariant
'        End Get

'    End Property

'    Public Shared Function findLinguaBYID(ByVal listaDom As List(Of Lingua), ByVal idLingua As String) As Lingua

'        Dim oLingua As New Lingua
'        oLingua = listaDom.Find(New PredicateWrapper(Of Lingua, String)(idLingua, AddressOf trovaID))

'        Return oLingua

'    End Function

'    Public Shared Function trovaID(ByVal item As Lingua, ByVal argument As String) As Boolean

'        Return item.id = argument

'    End Function
'End Class
