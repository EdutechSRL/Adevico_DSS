Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IViewDashBoard
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property NoticeBoardPermission() As ModuleNoticeBoard
        ReadOnly Property CommunitiesNoticeBoardPermission() As IList(Of ModuleCommunityPermission(Of ModuleNoticeBoard))

        ReadOnly Property PreloadedCommunityID() As Integer
        Property CurrentMessageID() As Long
        Property MessageCommunityID() As Integer
        Sub ViewMessage()
        Sub NoPermissionToAccess()

        Function GetMessageNavigationUrl(ByVal MessageID As Long, ByVal oContext As NoticeBoardContext, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType) As String
        Sub NavigationUrl(ByVal oContext As NoticeBoardContext, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType)
        Sub setHeaderTitle(ByVal CommunityName As String)


        'Action
        Sub AddShowMessageAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)


        'Community
        Sub LoadLastSubscription(ByVal oList As List(Of dtoSubscription))
        '  Sub setNewsUrl(ByVal CommunityName As String)
    End Interface
End Namespace