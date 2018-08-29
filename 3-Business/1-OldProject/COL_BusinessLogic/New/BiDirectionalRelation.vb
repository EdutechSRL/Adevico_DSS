Public Class BiDirectionalRelation(Of Oggetto1 As Class, Oggetto2 As Class)
    Public ReadOnly Elemento1 As Oggetto1
    Public ReadOnly Elemento2 As Oggetto2

    Public Sub New(ByVal obj1 As Oggetto1, ByVal obj2 As Oggetto2)
        Me.Elemento1 = obj1
        Me.Elemento2 = obj2
    End Sub
    Public Function Contains(ByVal obj As Oggetto1) As Boolean
        Return Me.Elemento1 Is obj
    End Function
    Public Function Contains(ByVal obj As Oggetto2) As Boolean
        Return Me.Elemento2 Is obj
    End Function
End Class