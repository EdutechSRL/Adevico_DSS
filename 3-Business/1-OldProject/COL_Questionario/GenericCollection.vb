Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Reflection

<Serializable()> Public Class GenericCollection(Of T)
    Inherits List(Of T)

    Public Sub New()
        MyBase.New()
    End Sub


    Public Sub New(ByVal list As IList(Of T))
        MyBase.New(list)
    End Sub

    Public Overloads Sub Add(ByVal item As T)
        MyBase.Add(item)
    End Sub


End Class

