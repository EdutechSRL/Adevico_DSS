Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewTodayActivities
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property ModulePermission() As ModuleNoticeBoard
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleNoticeBoard))
        Property CurrentNoticeBoardContext() As NoticeBoardContext

        Property Pager() As PagerBase
        ReadOnly Property PreLoadedMessageID() As Long
        ReadOnly Property PreLoadedView() As NoticeBoardContext.ViewModeType
        ReadOnly Property PreLoadedSmallView() As NoticeBoardContext.SmallViewType
        ReadOnly Property PreLoadedPreviousView() As NoticeBoardContext.ViewModeType

        Property CurrentMessageID() As Long
        Property CurrentView() As NoticeBoardContext.ViewModeType
        Property CurrentSmallView() As NoticeBoardContext.SmallViewType
        WriteOnly Property ShowAdvancedEditor() As Boolean


        Sub ViewMessage(ByVal Message As NoticeBoard, ByVal Permission As ModuleNoticeBoard, ByVal isCurrent As Boolean, ByVal PrintUrl As String)
        Sub ViewPreviousMessages(ByVal Messages As List(Of dtoSmallMessage), ByVal Permission As ModuleNoticeBoard, ByVal Mode As NoticeBoardContext.SmallViewType)
        Sub EditMessage(ByVal Message As NoticeBoard, ByVal Htmleditor As Boolean)
        Sub NoPermissionToAccess()

        Function GetMessageNavigationUrl(ByVal MessageID As Long, ByVal oContext As NoticeBoardContext, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType) As String
        Function GetNavigationUrl(ByVal oContext As NoticeBoardContext, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType) As String
        Sub NavigationUrl(ByVal oContext As NoticeBoardContext, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType)
        ReadOnly Property PreLoadedPage() As Integer

        WriteOnly Property SetBackUrlFromEditor() As String
        WriteOnly Property SetPreviousURL() As String
        Sub SetNewMessageUrl(ByVal HTMLurl As String, ByVal AdvancedUrl As String)
        Sub setHeaderTitle(ByVal CommunityName As String)


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