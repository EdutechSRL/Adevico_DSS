Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
	<CLSCompliant(True), Serializable()> Public Class WorkBookCommunityPermission
		Private _ModulePersonalDiary As ModuleWorkBook
		Private _ID As Integer

		Public Property Permissions() As ModuleWorkBook
			Get
				Return _ModulePersonalDiary
			End Get
			Set(ByVal value As ModuleWorkBook)
				_ModulePersonalDiary = value
			End Set
		End Property
		Public Property ID() As Integer
			Get
				Return _ID
			End Get
			Set(ByVal value As Integer)
				_ID = value
			End Set
		End Property
	End Class
End Namespace