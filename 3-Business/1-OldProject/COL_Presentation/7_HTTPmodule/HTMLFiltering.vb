Imports System
Imports System.Text
'Imports System.IO
Imports System.Web
Imports System.Collections
Imports System.Diagnostics
Imports System.Text.RegularExpressions

Public Class HTMLFiltering
    Inherits Stream
    Private _sink As Stream
    Private _position As Long
    Private buf As StringBuilder

    Public Sub New(ByVal sink As Stream, Optional ByVal h As Hashtable = Nothing)
        _sink = sink
        buf = New StringBuilder()
        If h IsNot Nothing Then
            hash = h
        End If
    End Sub

    Public hash As New Hashtable

    ' The following members of Stream must be overriden.
    Public Overloads Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overloads Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overloads Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overloads Overrides ReadOnly Property Length() As Long
        Get
            Return 0
        End Get
    End Property
    Public Overloads Overrides Property Position() As Long
        Get
            Return _position
        End Get
        Set(ByVal value As Long)
            _position = value
        End Set
    End Property

    Public Overloads Overrides Function Seek(ByVal offset As Long, ByVal direction As System.IO.SeekOrigin) As Long
        Return _sink.Seek(offset, direction)
    End Function
    Public Overloads Overrides Sub SetLength(ByVal length As Long)
        _sink.SetLength(length)
    End Sub
    Public Overloads Overrides Sub Close()
        _sink.Close()
    End Sub
    Public Overloads Overrides Sub Flush()
        _sink.Flush()
    End Sub
    Public Overloads Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer
        Return _sink.Read(buffer, offset, count)
    End Function
    ' The Write method actually does the filtering.
    Public Overloads Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)
        Try
            Dim testo As String = System.Text.Encoding.[Default].GetString(buffer, offset, count)
            buf.Append(testo)
            If testo.ToLower().IndexOf("</html>") <> -1 Then
                Dim st As String = Modifica(buf.ToString())
                _sink.Write(System.Text.Encoding.[Default].GetBytes(st), 0, st.Length)
            End If
        Catch ex As Exception
            _sink.Write(System.Text.Encoding.UTF8.GetBytes(ex.Message), 0, ex.Message.Length)
        End Try
    End Sub

    Private Function Modifica(ByVal tutto As String) As String

        'tutto = tutto.Replace("{latex}", "<img class='lateximg' src='/Eq2Img/?$")
        'tutto = tutto.Replace("{/latex}", "$' alt='latex'/>")

        For Each k As String In hash.Keys
            tutto = tutto.Replace(k, hash(k))
        Next

        Return tutto
    End Function
End Class