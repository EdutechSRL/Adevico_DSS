Imports System.Collections
Imports System.Collections.Generic


Public Class GenericCollection(Of T)
	Inherits List(Of T)
	Implements IEquatable(Of GenericCollection(Of T))


	Public Function isEqual(ByVal other As GenericCollection(Of T)) As Boolean Implements System.IEquatable(Of GenericCollection(Of T)).Equals
		Dim isUguale As Boolean = False

		For Each obj As T In Me
			'	isUguale = obj.isequal(other)
		Next

	End Function
End Class
