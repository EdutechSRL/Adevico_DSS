Imports lm.Comol.Modules.TaskList.Domain


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True), Serializable()> Public Class TaskListCommunityPermission
        Private _ModuleTaskList As ModuleTaskList
        Private _ID As Integer

        Public Property Permissions() As ModuleTaskList
            Get
                Return _ModuleTaskList
            End Get
            Set(ByVal value As ModuleTaskList)
                _ModuleTaskList = value
            End Set
        End Property
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property
    End Class
End Namespace


