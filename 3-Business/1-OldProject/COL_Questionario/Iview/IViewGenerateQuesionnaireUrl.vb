Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.Mail

Public Interface IViewGenerateQuesionnaireUrl
    Inherits lm.Comol.Core.DomainModel.Common.iDomainView

    ReadOnly Property PreloadedIdQuestionnnaire As Integer

    Sub GotoQuestionnaire(idQuestionnaire As Integer, idUser As Integer)
    Sub DisplayAccessError(ByVal qName As String)
    Sub DisplayAccessError(ByVal qName As String, ByVal uName As String, ByVal surname As String)
    Sub DisplayUnknownQuesionnaire()
    Sub DisplaySessionTimeout(ByVal idCommunity As Integer)
    Sub DisplayNoPermission()
End Interface