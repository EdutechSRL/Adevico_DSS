Public Interface IViewAdvancedStatistics
    Inherits lm.Comol.Core.DomainModel.Common.iDomainView

    ReadOnly Property PreloadedBackUrl As String
    ReadOnly Property PreloadedIdCommunity As Integer
    ReadOnly Property PreloadedIdQuestionnaire As Integer
    ReadOnly Property PreloadedIdOwner As Long
    ReadOnly Property PreloadedIdLink As Long
    ReadOnly Property PreloadedIdOwnerGuid As Guid
    ReadOnly Property PreloadedIdOwnerType As Integer

    ReadOnly Property GetBaseUrl As String
    Property QuestContext As dtoContext
    Property BackUrl As String
    Property SelectedParticipantTypes As List(Of participantType)


    Function GetModuleByLink(idCommunity As Integer, idUser As Integer, idLink As lm.Comol.Core.DomainModel.ModuleLink) As ModuleQuestionnaire

    Sub DisplayNoPermission()
    Sub DisplayUnknownItem()
    Sub DisplaySessionTimeout()
    Sub LoadUserStatisticsPage(context As dtoContext, ByVal idUser As Integer)
    Sub LoadStatisticsPage(idCommunity As Integer, ByVal idQuestionnaireType As Integer)
    Sub LoadStatisticsPage(idCommunity As Integer, ByVal idQuestionnaireType As Integer, idOwner As Long, idOwnerType As Integer)

    Sub LoadAvailableParticipant(items As List(Of participantType))

End Interface