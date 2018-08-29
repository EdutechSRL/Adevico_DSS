Namespace UCServices
    Public Class Services_TaskList
        Inherits UCServices.Abstract.MyServices
        Public Const Codex As String = "SRVTASK"


#Region "Public Property"

        'Aggiunta Tia x prova

        Public Property AddPersonalProject() As Boolean
            Get
                AddPersonalProject = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, 1)
            End Set
        End Property

        Public Property AddCommunityProject() As Boolean
            Get
                AddCommunityProject = MyBase.GetPermissionValue(PermissionType.Add)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Add, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property ViewCommunityProjects() As Boolean
            Get
                ViewCommunityProjects = MyBase.GetPermissionValue(PermissionType.Browse)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Browse, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property Administration() As Boolean
            Get
                Administration = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Admin, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property ManagementPermission() As Boolean
            Get
                ManagementPermission = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Grant, IIf(Value, 1, 0))
            End Set
        End Property

#End Region

        Sub New()
            MyBase.New()
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Services_TaskList
            Return New Services_TaskList("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            StartAddProject = 74000
            ProjectAdded = 74001
            AnnulAddProject = 74002
            StartAddTasks = 74003
            TaskAdded = 74004
            AnnulAddTasks = 74005
            StartVirtualDeleteWithReallocateResource = 74006
            StartUnDeleteWithReallocateResource = 74007
            AnnulVirtualDeleteWithReallocateResource = 74008
            AnnulUnDeleteWithReallocateResource = 74009
            FinishVirtualDeleteWithReallocateResource = 74010
            FinishUnDeleteWithReallocateResource = 74011
            StartUndeleteTask = 74012
            FinishUndeleteTask = 74013
            StartVirtualDeleteTask = 74014
            FinishVirtualDeleteTask = 74015
            StartDeleteTask = 74016
            FinishDeleteTask = 74017
            'AddedTaskAssignment = 74018
            StartManageTaskAssignment = 74019
            FinishManageTaskAssignment = 74020
            StartUpdateTaskDetail = 74021
            StartViewTaskDetail = 74022
            FinishUpdateTaskDetail = 74023
            FinishViewTaskDetail = 74024
            StartViewProjectMap = 74025
            FinishViewProjectMap = 74026
            ViewAssignedTask = 74027
            ViewTaskManagement = 74028
            ViewInvolvingProject = 74029
            ViewGantt = 74030
            'ViewTaskAdministration = 74031


        End Enum

        Public Enum ObjectType
            None = 0
            Project = 1
            Task = 2
            TaskAssignment = 3
            TaskFile = 4
            TaskLinkedFile = 5

            'DiaryItemFile = 5,
            'DiaryItemLinkedFile = 6
        End Enum

        <Flags()> Public Enum Base2Permission
            AddCommunityProject = 8192 '13 Add
            Administration = 64 '6 Admin
            ManagementPermission = 32 '5 Grant
            ViewCommunityProjects = 1024 '10 Browse
            AddPersonalProject = 2 '1 Write
        End Enum
    End Class
End Namespace

'    PermissionType
'    None = -1                                           '    Admin = 6 '64
'    Read = 0 '1                                         '    Send = 7 ' 128
'    Write = 1 '2                                        '    Receive = 8 '256
'    Change = 2 '4                                       '    Synchronize = 9 '512
'    Delete = 3 '8                                       '    Browse = 10 '1024      ViewCommunityProjects
'    Moderate = 4 '16                                    '    Print = 11 '2048
'    Grant = 5 '32                                       '    ChangeOwner = 12 '4096
'    Add = 13 '8192        AddCommunityProject                                        '    ChangeStatus = 14 '16384
'    DownLoad = 15 '32768
