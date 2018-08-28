Imports System
Imports System.Collections.Generic
Imports System.Text
Imports ManagedDesigns.ApplicationBlocks.Validation
Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints
Imports System.Reflection

<Serializable(), CLSCompliant(True)> Public Class BaseDomainObject
	Implements IValidableObject, ICloneable

	Public Sub New()
	End Sub

	Public ReadOnly Property isValid() As Boolean _
	 Implements IValidableObject.IsValid
		Get
			Try
				Return ConstraintValidator.IsValid(Me)
			Catch ex As Exception
				Return False
			End Try
		End Get
	End Property

	Public Function Validate() As IList(Of ValidationError) _
	 Implements IValidableObject.Validate
		Dim errors As IList(Of ValidationError) = DirectCast(ConstraintValidator.Validate(Me), IList(Of ValidationError))
		Return errors
	End Function

	Public Function Clone() As Object Implements System.ICloneable.Clone
		Dim bf As BindingFlags = BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public
		'First we create an instance of this specific type.
		Dim newObject As Object = Activator.CreateInstance(Me.GetType())

		'We get the array of fields for the new type instance.
		Dim fields As FieldInfo() = newObject.GetType().GetFields(bf)


		Dim i As Integer = 0

		Dim l() As FieldInfo = Me.GetType().GetFields(bf)

		For Each fi As FieldInfo In l

			'For Each fi As FieldInfo In Me.GetType().GetFields(bf)
			'We query if the fiels support the ICloneable interface.
			Dim ICloneType As Type = fi.FieldType.GetInterface("ICloneable", True)

			If ICloneType IsNot Nothing Then
				'Getting the ICloneable interface from the object.
				Dim IClone As ICloneable = DirectCast(fi.GetValue(Me), ICloneable)

				'We use the clone method to set the new value to the field.
				If IClone IsNot Nothing Then
					fields(i).SetValue(newObject, IClone.Clone())
				End If

			Else
				' If the field doesn't support the ICloneable
				' interface then just set it.
				fields(i).SetValue(newObject, fi.GetValue(Me))
			End If

			'Now we check if the object support the
			'IEnumerable interface, so if it does
			'we need to enumerate all its items and check if
			'they support the ICloneable interface.
			Dim IEnumerableType As Type = fi.FieldType.GetInterface("IEnumerable", True)
			If IEnumerableType IsNot Nothing Then
				'Get the IEnumerable interface from the field.
				Dim IEnum As IEnumerable = DirectCast(fi.GetValue(Me), IEnumerable)

				'This version support the IList and the
				'IDictionary interfaces to iterate on collections.
				Dim IListType As Type = fields(i).FieldType.GetInterface("IList", True)
				Dim IDicType As Type = fields(i).FieldType.GetInterface("IDictionary", True)

				Dim j As Integer = 0
				If IListType IsNot Nothing Then
					'Getting the IList interface.
					Dim list As IList = DirectCast(fields(i).GetValue(newObject), IList)

					For Each obj As Object In IEnum
						'Checking to see if the current item
						'support the ICloneable interface.
						ICloneType = obj.GetType().GetInterface("ICloneable", True)

						If ICloneType IsNot Nothing Then
							'If it does support the ICloneable interface,
							'we use it to set the clone of
							'the object in the list.
							Dim cloned As ICloneable = DirectCast(obj, ICloneable)

							list(j) = cloned.Clone()
						End If

						'NOTE: If the item in the list is not
						'support the ICloneable interface then in the
						'cloned list this item will be the same
						'item as in the original list
						'(as long as this type is a reference type).

						j += 1
					Next
				ElseIf IDicType IsNot Nothing Then
					'Getting the dictionary interface.
					Dim dic As IDictionary = DirectCast(fields(i).GetValue(newObject), IDictionary)
					j = 0

					For Each de As DictionaryEntry In IEnum
						'Checking to see if the item
						'support the ICloneable interface.
						ICloneType = de.Value.GetType().GetInterface("ICloneable", True)

						If ICloneType IsNot Nothing Then
							Dim cloned As ICloneable = DirectCast(de.Value, ICloneable)

							dic(de.Key) = cloned.Clone()
						End If
						j += 1
					Next
				End If
			End If
			i += 1
		Next
		Return newObject
	End Function
End Class