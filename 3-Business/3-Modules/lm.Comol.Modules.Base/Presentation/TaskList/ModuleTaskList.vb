Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <Serializable(), CLSCompliant(True)> Public Class ModuleTaskList

        Private _CreateCommunityProject As Boolean
        Private _CreatePersonalProject As Boolean
        Private _CreatePersonalCommunityProject As Boolean
        Private _ManagementPermission As Boolean 'Noi abbiam ruoli con permessi predefiniti..che fare?Dove metterli?
        Private _Administration As Boolean '? controllare cosa si intende
        Private _DownloadAllowed As Boolean '?
        Private _PrintTaskList As Boolean '?
        Private _ViewTaskList As Boolean
        'aDD BY TIA 
        Private _UploadFile As Boolean
        Private _DeleteTask As Boolean
        Private _AddTask As Boolean
        Private _Edit As Boolean




#Region "Public Property Module Tasklist"

        Public Property CreateCommunityProject() As Boolean
            Get
                CreateCommunityProject = _CreateCommunityProject
            End Get
            Set(ByVal Value As Boolean)
                _CreateCommunityProject = Value
            End Set
        End Property
        Public Property CreatePersonalProject() As Boolean
            Get
                CreatePersonalProject = _CreatePersonalProject
            End Get
            Set(ByVal Value As Boolean)
                _CreatePersonalProject = Value
            End Set
        End Property
        Public Property CreatePersonalCommunityProject() As Boolean
            Get
                CreatePersonalCommunityProject = _CreatePersonalCommunityProject
            End Get
            Set(ByVal Value As Boolean)
                _CreatePersonalCommunityProject = Value
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                ManagementPermission = _ManagementPermission
            End Get
            Set(ByVal Value As Boolean)
                _ManagementPermission = Value
            End Set
        End Property
        Public Property Administration() As Boolean
            Get
                Administration = _Administration
            End Get
            Set(ByVal Value As Boolean)
                _Administration = Value
            End Set
        End Property
        Public Property DownloadAllowed() As Boolean
            Get
                DownloadAllowed = _DownloadAllowed
            End Get
            Set(ByVal Value As Boolean)
                _DownloadAllowed = Value
            End Set
        End Property
        Public Property PrintTaskList() As Boolean
            Get
                PrintTaskList = _PrintTaskList
            End Get
            Set(ByVal Value As Boolean)
                _PrintTaskList = Value
            End Set
        End Property
        Public Property ViewTaskList() As Boolean
            Get
                ViewTaskList = _ViewTaskList
            End Get
            Set(ByVal Value As Boolean)
                _ViewTaskList = Value
            End Set
        End Property

        '---------------------------------------------------------------------------------------------------------------------------------------


        Public Property UploadFile() As Boolean
            Get
                UploadFile = _UploadFile
            End Get
            Set(ByVal value As Boolean)
                _UploadFile = value
            End Set
        End Property

        Public Property DeleteTask() As Boolean
            Get
                DeleteTask = _DeleteTask
            End Get
            Set(ByVal value As Boolean)
                _DeleteTask = value
            End Set
        End Property

        Public Property AddTask() As Boolean
            Get
                AddTask = _AddTask
            End Get
            Set(ByVal value As Boolean)
                _AddTask = value
            End Set
        End Property

        Public Property Edit() As Boolean
            Get
                Edit = _Edit
            End Get
            Set(ByVal value As Boolean)
                _Edit = value
            End Set
        End Property



#End Region

        Sub New()
            '_CreateCommunityProject = False
            '_CreatePersonalProject = False
            '_CreatePersonalCommunityProject = False
            '_ManagementPermission = False
            '_Administration = False
            '_DownloadAllowed = False
            '_PrintTaskList = False
            '-------------------------
            '_UploadFile = False
            '_DeleteTask = False
            '_AddTask = False
            '_Edit = False


        End Sub

        Sub New(ByVal oService As COL_BusinessLogic_v2.UCServices.Services_TaskList)
            Me.Administration = oService.Administration
            Me.CreateCommunityProject = oService.Administration OrElse oService.AddCommunityProject
            Me.CreatePersonalCommunityProject = True
            Me.CreatePersonalProject = True
            Me.DownloadAllowed = oService.Administration 'secondo me va rivisto, solo gli admin possono scaricare files?!?!?! Mettere .ViewCommunityTask
            Me.ManagementPermission = oService.ManagementPermission
            Me.PrintTaskList = oService.Administration OrElse oService.ViewCommunityProjects
            Me.ViewTaskList = oService.Administration OrElse oService.ViewCommunityProjects

            'Me.UploadFile = False
            'Me.DeleteTask = False
            'Me.AddTask = False
            'Me.Edit = False

        End Sub

        Sub New(ByVal permission As Long)
            '[Me].ViewDiaryItems = PermissionHelper.CheckPermissionSoft(CLng(Base2Permission.ViewLessons) Or CLng(Base2Permission.EditLesson) Or CLng(Base2Permission.AdminService), permission)

            Me.Administration = PermissionHelper.CheckPermissionSoft(CLng(Base2Permission.Administration), permission)
            Me.CreateCommunityProject = PermissionHelper.CheckPermissionSoft(CLng(Base2Permission.Administration) Or CLng(Base2Permission.AddCommunityProject), permission)
            Me.CreatePersonalCommunityProject = True
            Me.CreatePersonalProject = True
            Me.DownloadAllowed = PermissionHelper.CheckPermissionSoft(CLng(Base2Permission.Administration), permission) 'secondo me va rivisto, solo gli admin possono scaricare files?!?!?! Mettere .ViewCommunityTask
            Me.ManagementPermission = PermissionHelper.CheckPermissionSoft(CLng(Base2Permission.ManagementPermission), permission)
            Me.PrintTaskList = PermissionHelper.CheckPermissionSoft(CLng(Base2Permission.Administration) Or CLng(Base2Permission.ViewCommunityProjects), permission)
            Me.ViewTaskList = PermissionHelper.CheckPermissionSoft(CLng(Base2Permission.Administration) Or CLng(Base2Permission.ViewCommunityProjects), permission)

            'Me.UploadFile = False
            'Me.DeleteTask = False
            ' Me.AddTask = False
            ' Me.Edit = False
        End Sub

        Shared Function CreatePortalmodule(ByVal TypeID As Integer) As ModuleTaskList
            Dim oService As New ModuleTaskList
            With oService
                .Administration = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo)
                .CreateCommunityProject = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo)
                .CreatePersonalCommunityProject = (TypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)
                .CreatePersonalProject = (TypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)
                .DownloadAllowed = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo)
                .ManagementPermission = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .PrintTaskList = (TypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)
                .ViewTaskList = (TypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)

                .UploadFile = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo)
                '.DeleteTask = False
                '.AddTask = False
                '.Edit = False
            End With

            Return oService
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

            DownloadFileItem = 74032
            ShowFileItem = 74033
            HideFileItem = 74034
            AddFile = 74035
            RemoveFile = 74036
            AddFiles = 74037
            RemoveFiles = 74038

        End Enum

        Public Enum ObjectType
            None = 0
            Project = 1
            Task = 2
            TaskAssignment = 3
            FileScorm = 4
            TaskFile = 5
            TaskLinkedFile = 5
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