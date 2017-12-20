Public Class PresenterEdtorSmartTag
	Inherits GenericPresenter

	Public Sub New(ByVal view As IviewEditorSmartTag)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewEditorSmartTag
		Get
			View = MyBase.view
		End Get
	End Property

	Sub Init(ByVal BaseUrl As String)
		Me.View.LoadSmartTag(ManagerConfiguration.GetSmartTags(BaseUrl).TagList)
	End Sub
	Sub TranslateText(ByVal iTag As String, ByVal iTagText As String, ByVal BaseUrl As String)
		Dim StringToTranslate As String = String.Format(Comol.Entity.SmartTag.OpenTag, iTag) & iTagText & String.Format(Comol.Entity.SmartTag.CloseTag, iTag)

		Me.View.PreviewText = ManagerConfiguration.GetSmartTags(BaseUrl).TagAll(StringToTranslate)
	End Sub

End Class