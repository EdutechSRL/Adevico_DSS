Public Interface IviewEditor
	Property EditorEnabled() As Boolean
	Property EditorMaxChar() As Int64

	Property ImagesPaths() As String()
	ReadOnly Property Text() As String
	Property HTML() As String
	Property ShowScrollingSpeed() As Boolean
	Property ShowAddSmartTag() As Boolean
	WriteOnly Property ShowAddImage() As Boolean
	WriteOnly Property ShowAddDocument() As Boolean

	Property AutoScrollingSpeed() As ScrollingSpeed
	ReadOnly Property CustomDialogScript() As String
	ReadOnly Property UserLanguage() As Lingua
	ReadOnly Property CurrentCommunity() As Community
	ReadOnly Property CurrentUser() As Person
	WriteOnly Property AllowPreview() As Boolean
	Property FontSizes() As String
	Property FontNames() As String
	Property DisabledTags() As String
	Sub SetAdvancedTools(ByVal oList As List(Of SmartTag))
	'Property DefaultFontSize() As String
	'Property DefaultFontName() As String
	'Property DefaultFontColor() As String
	'Property DefaultBackGroundColor() As String


End Interface