Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IViewNoticeBoardContainer
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Function NoticeboardPermission(ByVal CommunityID As Integer) As ModuleNoticeBoard

        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property PreLoadedMessageID() As Long
        ReadOnly Property PreLoadedFromPage() As NoticeBoardContext.ViewModeType
        ReadOnly Property PreLoadedMessagesToShow() As MessagesToShow
        ReadOnly Property PortalName() As String

        Sub setHeaderTitle(ByVal CommunityName As String, ByVal Forportal As Boolean)
        Property Pager() As PagerBase
        Property CurrentMessagesToShow() As MessagesToShow
        Property CurrentMessageID() As Long
        Property NoticeboardCommunityID() As Integer
        Sub LoadMessage(ByVal oNoticeboard As NoticeBoard, ByVal Permission As ModuleNoticeBoard, ByVal isCurrent As Boolean, ByVal CommunityID As Integer, ByVal CurrentPage As NoticeBoardContext.ViewModeType, ByVal PreviousPage As NoticeBoardContext.ViewModeType)
        Sub LoadPreviousMessages(ByVal Messages As List(Of dtoSmallMessage), ByVal Permission As ModuleNoticeBoard, ByVal Mode As NoticeBoardContext.SmallViewType)
        Sub NoPermissionToAccess()

        WriteOnly Property SetPreviousURL() As String
        Sub SetNewMessageUrl(ByVal HTMLurl As String, ByVal AdvancedUrl As String)

        ''Action
        'Sub AddAction(ByVal ActionTypeID As Integer, ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddCleanAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddCreateAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddDeleteAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddEditAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddGenericErrorAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        'Sub AddNoPermissionAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        'Sub AddSetDefaultAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddShowMessageAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        'Sub AddShowHistoryAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        'Sub AddShowRecycleAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        'Sub AddUndeleteAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddUndeleteActionAndActivate(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddVirtualDeleteAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As List(Of String))
        'Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)
    End Interface
End Namespace