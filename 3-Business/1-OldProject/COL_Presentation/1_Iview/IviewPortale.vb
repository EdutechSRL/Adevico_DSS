Imports COL_BusinessLogic_v2

Public Interface IviewPortale
	ReadOnly Property CurrentPresenter() As PresenterPortale
	Property CurrentLanguageID() As Integer
	ReadOnly Property MailConfig() As MailLocalized
	ReadOnly Property DefaultSetting() As PresenterSettings

    ReadOnly Property IstituzioneID() As Integer

	Sub ChangeLanguageSettings()
End Interface