Namespace UCServices

    Public Class Services_DiarioLezioni
        Inherits Abstract.MyServices
        'controllare se i permessi hanno un senso...
        Private Const Codice As String = "SRVLEZ"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property Read() As Boolean
            Get
                Read = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Change() As Boolean
            Get
                Change = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Upload() As Boolean
            Get
                Upload = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Delete() As Boolean
            Get
                Upload = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Delete, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Print() As Boolean
            Get
                Print = MyBase.GetPermissionValue(PermissionType.Print)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Print, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property GrantPermission() As Boolean
            Get
                GrantPermission = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Grant, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Admin() As Boolean
            Get
                Admin = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Admin, IIf(Value, 1, 0))
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

        Public Shared Function Create() As Services_DiarioLezioni
            Return New Services_DiarioLezioni("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            ShowDiary = 10003
            CreateItem = 10004
            ChangeItem = 10005
            VirtualDeleteItem = 10006
            UndeleteItem = 10007
            DeleteItem = 10008
            DeleteDiary = 10009
            ImportItem = 10010
            AddFiles = 10011
            RemoveFiles = 10012
            MoveItem = 10013
            AddFile = 10014
            RemoveFile = 10015
            AddMultipleFilesNoDate = 10016
            AddFileToItemNoDate = 10017
            AddFileScormToItemNoDate = 10018
            AddFileScorm = 10019
            InitPublishFileIntoCommunity = 10020
            InitAddCommunityFiles = 10021
            InitUploadMultipleFiles = 10022
            InitAddItem = 10023
            InitEditItem = 10024
            CreateItemNoDate = 10025
            EditItemNoDate = 10026
            DownloadFileItem = 10027
        End Enum
        Public Enum ObjectType
            None = 0
            Diary = 1
            DiaryItem = 2
            File = 3
            FileScorm = 4
            DiaryItemFile = 5
            DiaryItemLinkedFile = 6
        End Enum


        <Flags()> Public Enum Base2Permission
            ViewLessons = 1 '0
            EditLesson = 4 '2
            UploadFile = 2 '1
            DeleteLesson = 8 '3
            PrintLessons = 2048 ' 11
            GrantPermission = 32 '5
            AdminService = 64 '6
        End Enum

    End Class
End Namespace