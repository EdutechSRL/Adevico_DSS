Namespace Comol.Entities
	Public Class PlainRole
		Private _RoleType As New Role
		Private _Language As New Lingua

		Public Property Language() As Lingua
			Get
				Return _Language
			End Get
			Set(ByVal value As Lingua)
				_Language = value
			End Set
		End Property

		Public Property RoleType() As Role
			Get
				Return _RoleType
			End Get
			Set(ByVal value As Role)
				_RoleType = value
			End Set
		End Property

		Sub New()

		End Sub

		Sub New(ByVal iLanguage As Lingua, ByVal iRole As Role)
			Me._Language = iLanguage
			Me._RoleType = iRole
		End Sub

	End Class
End Namespace