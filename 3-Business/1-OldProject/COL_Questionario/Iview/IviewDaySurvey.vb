Public Interface IviewDaySurvey
    ReadOnly Property CurrentCommunity() As Community
    ReadOnly Property CurrentUser() As Person
    Property CurrentSurvey() As Questionario
    Property ShowPreview() As Boolean
    Property SurveyType() As Integer
    Property Width() As Integer
    Property Height() As Integer
    Property ShowResults() As Boolean
    ReadOnly Property SurveyLanguageId() As Integer
    Property IsActive() As Boolean
    ReadOnly Property LinkText() As String
    ReadOnly Property CurrentPresenter() As PresenterDaySurvey
    Property NumberOfDaySurveys() As Integer
    Sub Init(ByVal ShowPreview As Boolean, ByVal SurveyType As COL_Questionario.Questionario.TipoQuestionario, ByVal Width As Integer, ByVal Height As Integer, ByVal SurveyLanguageId As Integer)
    Sub BindDataList(ByVal oQuest As Questionario)
    Sub BindLinks(ByVal oQuest As List(Of Questionario))
    Sub BindPollResults(ByVal oQuest As Questionario)
    Sub RedirectToPath(ByVal nQuest As Integer)
End Interface
