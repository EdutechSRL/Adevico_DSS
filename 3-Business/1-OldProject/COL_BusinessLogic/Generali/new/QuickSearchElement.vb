Imports Comol.Entity

Public Class QuickSearchElement
	Inherits GenericElement
	Private _isDefault As Boolean
	Private _TextValue As String

	Public Property isDefault() As Boolean
		Get
			isDefault = _isDefault
		End Get
		Set(ByVal value As Boolean)
			_isDefault = value
		End Set
	End Property
	Public Property TextValue() As String
		Get
			TextValue = _TextValue
		End Get
		Set(ByVal value As String)
			_TextValue = value
		End Set
	End Property

	Public Sub New()
		Me.isDefault = False
	End Sub

	Public Sub New(ByVal Name As String, ByVal UniqueId As Integer, Optional ByVal TextValue As String = "", Optional ByVal isDefault As Boolean = False)
		MyBase.Text = Name
		MyBase.Value = UniqueId
		_TextValue = TextValue
		_isDefault = isDefault
	End Sub

End Class
