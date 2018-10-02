Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IViewNoticeBoard
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property PreLoadedMessageID() As Long
        ReadOnly Property PreLoadedView() As NoticeBoardContext.ViewModeType
        ReadOnly Property PreLoadedSmallView() As NoticeBoardContext.SmallViewType
        ReadOnly Property PreLoadedPreviousView() As NoticeBoardContext.ViewModeType
    End Interface
End Namespace