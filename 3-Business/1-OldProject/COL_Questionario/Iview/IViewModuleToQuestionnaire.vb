Imports lm.Comol.Core.DomainModel

Public Interface IViewModuleToQuestionnaire
    Inherits lm.Comol.Core.DomainModel.Common.iDomainView

    Property SourceModuleCode As String
    Property SourceModuleIdAction As Integer
    Property SourceIdOwnerType As Long
    Property SourceIdOwner As Long
    Property IdCommunityQuestionnaire As Integer
    Property CurrentAction As QuestionnaireAction
    Property AjaxViewUpdate As Boolean

    Sub InitializeControl(idCommunity As Integer, sourceModuleCode As String, sourceModuleIdAction As Integer, sourceIdOwner As Long, sourceOwnerIdType As Long)
    Sub InitializeQuestionnaireSelector(idCommunity As Integer, sourceModuleCode As String, sourceModuleIdAction As Integer)
    Sub LoadAvailableActions(actions As List(Of QuestionnaireAction), dAction As QuestionnaireAction)
    Sub DisplayAction(action As QuestionnaireAction)
    Sub DisplaySessionTimeout(idCommunity As Integer, idModule As Integer)
    Sub ChangeDisplayAction(action As QuestionnaireAction)

    Function GetActionTitle(action As QuestionnaireAction) As String
End Interface