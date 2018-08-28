''' <summary>
''' Periodi di validità
''' </summary>
Public Class dsValidity

    ''' <summary>
    ''' Dizionari nomi internazionalizzati
    ''' </summary>
    ''' <returns></returns>
    Public Property Names As IDictionary(Of String, String)

    ''' <summary>
    ''' Nome default (il primo elemento caricato)
    ''' </summary>
    ''' <returns></returns>
    Public Property DefaultName As String

    ''' <summary>
    ''' Valore in giorni
    ''' </summary>
    ''' <returns></returns>
    Public Property Value As Integer

    Public Sub New()
        Names = New Dictionary(Of String, String)
    End Sub

End Class
