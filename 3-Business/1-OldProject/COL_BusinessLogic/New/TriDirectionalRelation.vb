Public Class TriDirectionalRelation(Of Oggetto1 As Class, Oggetto2 As Class, Oggetto3 As Class)
    Public ReadOnly Elemento1 As Oggetto1
    Public ReadOnly Elemento2 As Oggetto2
    Public ReadOnly Elemento3 As Oggetto3

    Public Sub New(ByVal obj1 As Oggetto1, ByVal obj2 As Oggetto2, ByVal obj3 As Oggetto3)
        Me.Elemento1 = obj1
        Me.Elemento2 = obj2
        Me.Elemento3 = obj3
    End Sub

    Public Function Contains(ByVal obj As Oggetto1) As Boolean
        Return Me.Elemento1 Is obj
    End Function
    Public Function Contains(ByVal obj As Oggetto2) As Boolean
        Return Me.Elemento2 Is obj
    End Function
    Public Function Contains(ByVal obj As Oggetto3) As Boolean
        Return Me.Elemento3 Is obj
    End Function
End Class