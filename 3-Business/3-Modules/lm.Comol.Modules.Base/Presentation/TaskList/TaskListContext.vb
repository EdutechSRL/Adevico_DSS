Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True), Serializable()> Public Class TaskListContext
        Implements ICloneable

        Public UserID As Integer
        Public CommunityID As Integer
        Public isPortal As Boolean
        Public isPersonal As Boolean
        Public PageIndex As Integer
        Public PageSize As Integer
        ' Public TaskID As Long
        'Public ProjectID As Long
        'Public TaskAssignmentID As Long 'Serve??????


        'NoticeBoardContext.vb

        'Public UserID As Integer
        'Public MessageID As Long
        'Public SmallView As SmallViewType
        'Public ViewMode As ViewModeType

        'Public PreviousView As ViewModeType
        'Public Enum SmallViewType
        '    None = 0
        '    LastFourMessage = 1
        '    AlsoPreviousMessages = 2
        '    AllMessage = 3
        'End Enum

        'Public Enum ViewModeType
        '    None = 0
        '    CurrentMessage = 1
        '    Message = 2
        '    EditMessageHTML = 3
        '    DeleteMessage = 4
        '    ManageMessages = 5
        '    EditMessageADV = 6
        '    NewMessageHTML = 7
        '    NewMessageADV = 8
        '    DashBoard = 9
        'End Enum

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New TaskListContext
            o.UserID = UserID
            o.CommunityID = CommunityID
            'o.ProjectID = ProjectID
            o.isPortal = isPortal
            o.isPersonal = isPersonal
            o.PageIndex = PageIndex
            o.PageSize = PageSize
            'o.TaskID = TaskID
            'o.TaskAssignmentID = TaskAssignmentID

            Return o
        End Function
    End Class
End Namespace