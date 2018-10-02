Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <CLSCompliant(True), Serializable()> Public Class NoticeBoardContext
        Implements ICloneable

        Public UserID As Integer
        Public CommunityID As Integer
        Public isPortalCommunity As Boolean
        Public MessageID As Long
        Public SmallView As SmallViewType
        Public ViewMode As ViewModeType
        Public PageIndex As Integer
        Public PreviousView As ViewModeType
        Public Enum SmallViewType
            None = 0
            LastFourMessage = 1
            AlsoPreviousMessages = 2
            AllMessage = 3
        End Enum
        Public Enum ViewModeType
            None = 0
            CurrentMessage = 1
            Message = 2
            EditMessageHTML = 3
            DeleteMessage = 4
            ManageMessages = 5
            EditMessageADV = 6
            NewMessageHTML = 7
            NewMessageADV = 8
            DashBoard = 9
            PortalDashBoard = 10
            CommunityDashBoard = 11
            CommunityNoticeboard = 12
            PortalNoticeboard = 13
        End Enum

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New NoticeBoardContext
            o.CommunityID = CommunityID
            o.isPortalCommunity = isPortalCommunity
            o.MessageID = MessageID
            o.PageIndex = PageIndex
            o.SmallView = SmallView
            o.UserID = UserID
            o.ViewMode = ViewMode
            Return o
        End Function
    End Class
End Namespace