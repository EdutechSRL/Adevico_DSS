Imports System.Collections
Imports System.Collections.Generic
Imports System.Reflection

Public Class GenericComparer(Of T)
	Implements IComparer(Of T)

	Private _propertyName As String

	Public Property PropertyName() As String
		Get
			Return _propertyName
		End Get
		Set(ByVal value As String)
			_propertyName = value
		End Set
	End Property
	Public Sub New(ByVal p_propertyName As String)
		Me.PropertyName = p_propertyName
	End Sub

	Public Function Compare(ByVal x As T, ByVal y As T) As Integer Implements System.Collections.Generic.IComparer(Of T).Compare
		Dim DotIndex As Integer = Me.PropertyName.IndexOf(".")
		Dim t As Type = x.GetType()
		If DotIndex <> -1 Then
			Dim ThisProperty As String = Me.PropertyName.Substring(0, DotIndex)
			Dim Remainder As String = Me.PropertyName.Substring(DotIndex + 1)
			Dim val As PropertyInfo = t.GetProperty(ThisProperty)

			Return Compare(x.GetType().GetProperty(ThisProperty).GetValue(x, Nothing), y.GetType().GetProperty(ThisProperty).GetValue(y, Nothing), Remainder)
		Else

			Dim val As PropertyInfo = t.GetProperty(Me.PropertyName)
			If Not (val Is Nothing) Then
				Return Comparer.DefaultInvariant.Compare(val.GetValue(x, Nothing), val.GetValue(y, Nothing))
			Else
				Throw New Exception(Me.PropertyName + " is not a valid property to sort on.  It doesn't exist in the Class.")
			End If
		End If

	End Function
	Private Function Compare(ByVal x As Object, ByVal y As Object, ByVal PropertyName As String)
		Dim DotIndex As Integer = PropertyName.IndexOf(".")
		If DotIndex <> -1 Then
			Dim ThisProperty As String = PropertyName.Substring(0, DotIndex)
			Dim Remainder As String = PropertyName.Substring(DotIndex + 1)
			Return Compare(x.GetType().GetProperty(ThisProperty).GetValue(x, Nothing), y.GetType().GetProperty(ThisProperty).GetValue(y, Nothing), Remainder)
		Else
			Dim val As PropertyInfo = x.GetType().GetProperty(PropertyName)
			If Not (val Is Nothing) Then
				Return Comparer.DefaultInvariant.Compare(val.GetValue(x, Nothing), val.GetValue(y, Nothing))
			Else
				Throw New Exception(Me.PropertyName + " is not a valid property to sort on.  It doesn't exist in the Class.")
			End If
		End If
	End Function
End Class