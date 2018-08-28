Imports lm.Comol.Core.DomainModel

Public Interface IViewModuleQuizAction
    Inherits lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction

    ReadOnly Property PreLoadedContentView As lm.Comol.Core.DomainModel.ContentView
    ReadOnly Property DestinationUrl As String
    ReadOnly Property GetBaseUrl(ByVal useSSL As Boolean) As String
    ReadOnly Property GetBaseUrl() As String
    Property CurrentInfo() As dtoUserQuest
    Property InsideOtherModule As Boolean
    Property ItemIdentifier As String
    Property ForUserId As Integer
    Property ItemStatus As QuizStatus
    Property Score As Integer
    Property MaxScore As Integer
    Property MinScore As Integer

    Property QuizName As String

    Sub DisplayUnknownAction()
    Sub DisplayEmptyAction()
    Sub DisplayEmptyActions()

    Sub DisplayActions(actions As List(Of dtoModuleActionControl))
    Sub DisplayPlaceHolders(items As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder))
    Sub DisplayItemCompiled(quest As dtoUserQuest)
    Sub DisplayTextInfo(quest As dtoUserQuest)
    Sub DisplayItemToCompile(quest As dtoUserQuest, url As String)
    Sub DisplayItemViewCompiled(quest As dtoUserQuest, url As String)

    Sub DisplayAttemptItem(result As dtoUserQuest)
    Sub DisplayAttemptItem(result As dtoUserQuest, url As String)
    Function GetQuestionnaireDescription(result As dtoUserQuest) As String
End Interface

'    void DisplayFolder(String folderName);
'    void DisplayCreateFolder(String folderName);
'    void DisplayUploadFile(String folderName);
'    void DisplayFolderAction(long idFolder, String folderName);
'    void DisplayUploadAction(long idFolder, String folderName);

'    void DisplayItem(String name, String extension,long size, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
'    void DisplayFolder(String name, String url);
'    void DisplayItem(String name, String url, String extension, long size, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
'    void DisplayPlayItem(String name, String url, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);



'}