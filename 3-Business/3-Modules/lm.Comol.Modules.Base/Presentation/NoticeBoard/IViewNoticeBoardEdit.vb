Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IViewNoticeBoardEdit
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        Function NoticeboardPermission(ByVal CommunityID As Integer) As ModuleNoticeBoard

        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property PreLoadedMessageID() As Long
        ReadOnly Property PreLoadedPage() As Integer
        ReadOnly Property PreLoadedContainer() As NoticeBoardContext.ViewModeType
        ReadOnly Property PreLoadedFromPage() As NoticeBoardContext.ViewModeType
        ReadOnly Property PreLoadedMessagesToShow() As MessagesToShow


        ReadOnly Property PortalName() As String
        Property CurrentMessageID() As Long
        Property MessageText() As String
        Property MessageStyle() As TextStyle
        Property NoticeboardCommunityID() As Integer
        'WriteOnly Property EditorChanged() As Boolean
        Sub setHeaderTitle(ByVal CommunityName As String, ByVal ForEdit As Boolean, ByVal Forportal As Boolean)


        'Action
        Sub SendNoPermissionAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal MessageID As Long)
        Sub NoMessageWithThisID(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal MessageID As Long)
        Sub GotoPage(ByVal Container As NoticeBoardContext.ViewModeType, ByVal ToPage As NoticeBoardContext.ViewModeType, ByVal View As MessagesToShow, ByVal MessageID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer)
        Sub SetPreviousUrl(ByVal Container As NoticeBoardContext.ViewModeType, ByVal ToPage As NoticeBoardContext.ViewModeType, ByVal View As MessagesToShow, ByVal MessageID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer)


        Sub AddCreateAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        Sub AddEditAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddGenericErrorAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        'Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal PersonID As Integer)
    End Interface
End Namespace