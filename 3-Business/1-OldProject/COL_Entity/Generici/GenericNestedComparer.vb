Imports System.Reflection
Public Class GenericNestedComparer
	Implements IComparer

	Public Enum OrderEnum
		Ascending = 1
		Descending = -1
		none = 0
	End Enum
	Private _propertieslist As String
	Private _order As OrderEnum
	Private _orders As String = ""

	Public Property Orders() As String
		Get
			Return _orders
		End Get
		Set(ByVal value As String)
			_orders = value
		End Set
	End Property

	Public Property PropertiesList() As String
		Get
			Return _propertieslist
		End Get
		Set(ByVal value As String)
			If value.Contains(";") Then
				_orders = value
			End If
			_propertieslist = value
		End Set
	End Property

	Public Property Order() As OrderEnum
		Get
			Return _order
		End Get
		Set(ByVal value As OrderEnum)
			_order = value
		End Set
	End Property

	Public Function GetNested(ByVal WorkObj As Object, ByVal PropertiesList As String) As Object
		If WorkObj IsNot Nothing Then
			Dim WorkingType As Type = WorkObj.GetType
			Dim Properties() As String
			Dim WorkingObject As Object
			Properties = PropertiesList.Split(".")
			Try
				WorkingObject = WorkObj

				For i As Integer = 0 To Properties.Length - 1
					Dim y As PropertyInfo = WorkingType.GetProperty(Properties(i))
					WorkingObject = y.GetValue(WorkingObject, Nothing)
					WorkingType = y.PropertyType
				Next

				Return WorkingObject
			Catch ex As Exception
				Return Nothing
			End Try
		Else
			Return Nothing
		End If

	End Function

	Public Sub New(Optional ByVal Properties As String = "", Optional ByVal Order As OrderEnum = OrderEnum.Ascending)
		MyBase.New()
		PropertiesList = Properties
		Me.Order = Order
	End Sub

	Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

		If Orders <> "" Then
			Dim orderslist() As String = Orders.Split(";")
			Dim c As Integer = 0
			Dim idx As Integer = 0


			While (c = 0) And (idx < orderslist.Length)
				If orderslist(idx).Contains(" ASC") Then
					Me.Order = OrderEnum.Ascending
				Else
					If orderslist(idx).Contains(" DESC") Then
						Me.Order = OrderEnum.Descending
					End If
				End If

				Me.PropertiesList = orderslist(idx).Replace(" ASC", "").Replace(" DESC", "")

				c = Me.SingleCompare(x, y)
				idx += 1
			End While
			Return c
		Else
			Return SingleCompare(x, y)
		End If

	End Function

	Public Function SingleCompare(ByVal x As Object, ByVal y As Object) As Integer
		Dim x_obj As IComparable
		Dim y_obj As IComparable
		x_obj = GetNested(x, PropertiesList)
		y_obj = GetNested(y, PropertiesList)

		Return x_obj.CompareTo(y_obj) * Order
	End Function

End Class