Imports System
Imports System.Collections.Generic
Imports System.Text
Imports ManagedDesigns.ApplicationBlocks.Validation
Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints
Imports System.Reflection

<Serializable(), CLSCompliant(True)> Public Class DomainObject
	Implements IValidableObject, ICloneable

	Private _IsArchive As Boolean
	Private _Isvisible As Boolean
	Private _isDeleted As Boolean
	Private _CreatedBy As Person
	Private _CreatedAt As DateTime
	Private _ModifiedAt As Nullable(Of DateTime)
	Private _ModifiedBy As Person
	Private _Deletedby As Person
	Private _DeletedAt As Nullable(Of DateTime)


	Public Property Isvisible() As Boolean
		Get
			Isvisible = _Isvisible
		End Get
		Set(ByVal value As Boolean)
			_Isvisible = value
		End Set
	End Property
	Public Property IsArchive() As Boolean
		Get
			IsArchive = _IsArchive
		End Get
		Set(ByVal Value As Boolean)
			_IsArchive = Value
		End Set
	End Property
	Public Property CreatedBy() As Person
		Get
			Return _CreatedBy
		End Get
		Set(ByVal value As Person)
			_CreatedBy = value
		End Set
	End Property
	Public Property CreatedAt() As DateTime
		Get
			Return _CreatedAt
		End Get
		Set(ByVal value As DateTime)
			_CreatedAt = value
		End Set
	End Property
	Public Property ModifiedAt() As Nullable(Of DateTime)
		Get
			Return _ModifiedAt
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_ModifiedAt = value
		End Set
	End Property
	Public Property ModifiedBy() As Person
		Get
			Return _ModifiedBy
		End Get
		Set(ByVal value As Person)
			_ModifiedBy = value
		End Set
	End Property
	Public Property DeletedAt() As Nullable(Of DateTime)
		Get
			Return _DeletedAt
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_DeletedAt = value
		End Set
	End Property
	Public Property Deletedby() As Person
		Get
			Return _Deletedby
		End Get
		Set(ByVal value As Person)
			_Deletedby = value
		End Set
	End Property
	Public Property IsDeleted() As Boolean
		Get
			IsDeleted = _isDeleted
		End Get
		Set(ByVal value As Boolean)
			_isDeleted = value
		End Set
	End Property

	Public Sub New()
		_isDeleted = False
		_IsArchive = False
		_Isvisible = True
		'SharedLogger.LoggerContext(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType())
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