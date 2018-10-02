Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewReallocateUsers
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentCommunityID() As Integer
        ReadOnly Property CurrentTaskID() As Long
        ReadOnly Property CurrentModeType() As ModeType
        ReadOnly Property PreviusPage() As PreviusPageName
        Property ParentID() As Long
        Property TaskToAssignName() As String
        Property SessionUniqueKey() As System.Guid
        Sub ClearUniqueKey()
        Sub GoBackPage(ByVal ListOfReallocateTA As List(Of Long))
        Property ListOfUsers() As List(Of dtoReallocateTAWithHeader)
        Property CurrentStep() As StepType

        Sub InitSelectUsers(ByVal ListOfdtoOfTaskWithResource As List(Of dtoReallocateTAWithHeader))
        Sub InitFinalUserResume(ByVal ListOfdtoOfTaskWithResource As List(Of dtoReallocateTAWithHeader))
        Function GetUserFromSelectUsers() As List(Of dtoReallocateTA)
        Function GetUserFromSelectUsersWithHeader() As List(Of dtoReallocateTAWithHeader)
        Function GetUserFromResumeUsers() As List(Of dtoReallocateTA)
        Sub ShowError(ByVal ErrorString As String)

        Enum ModeType
            None = 0
            VirtualDelete = 1
            Undelete = 2
        End Enum

        Enum StepType
            SelectUsers = 0
            ResumeUsers = 1
            AddUsers = 2
        End Enum

        Enum PreviusPageName
            TaskMap = 0
            AssignedTasks = 1
            InvolvingProject = 2
            ManageTaskAssignment = 3
        End Enum


    End Interface
End Namespace