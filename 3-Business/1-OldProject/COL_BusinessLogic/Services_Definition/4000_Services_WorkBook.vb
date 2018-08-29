Namespace UCServices
    Public Class Services_WorkBook
        Inherits UCServices.Abstract.MyServices
        Public Const Codex As String = "SRVLBEL"


#Region "Public Property"
        
        Public Property CreateWorkBook() As Boolean
            Get
                CreateWorkBook = MyBase.GetPermissionValue(PermissionType.Add)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Add, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property AddItemsToOther() As Boolean
            Get
                AddItemsToOther = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property ChangeOtherWorkbook() As Boolean
            Get
                ChangeOtherWorkbook = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property CreateGroupWorkbook() As Boolean
            Get
                CreateGroupWorkbook = MyBase.GetPermissionValue(PermissionType.Moderate)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Moderate, IIf(Value, 1, 0))
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
        Public Property GrantPermission() As Boolean
            Get
                GrantPermission = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Grant, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property PrintOtherWorkBook() As Boolean
            Get
                PrintOtherWorkBook = MyBase.GetPermissionValue(PermissionType.Print)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Print, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property DeleteItemsFromOther() As Boolean
            Get
                DeleteItemsFromOther = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Delete, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property ChangeApprovationStatus() As Boolean
            Get
                ChangeApprovationStatus = MyBase.GetPermissionValue(PermissionType.ChangeStatus)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.ChangeStatus, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property ListOtherWorkBook() As Boolean
            Get
                ListOtherWorkBook = MyBase.GetPermissionValue(PermissionType.Browse)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Browse, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property ReadOtherWorkBook() As Boolean
            Get
                ReadOtherWorkBook = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property DownLoadItemFiles() As Boolean
            Get
                DownLoadItemFiles = MyBase.GetPermissionValue(PermissionType.DownLoad)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.DownLoad, IIf(Value, 1, 0))
            End Set
        End Property
        '     Send = 7
        '     Receive = 8
        '     Synchronize = 9
        '	ChangeOwner = 12

#End Region

        Sub New()
            MyBase.New()
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Services_WorkBook
            Return New Services_WorkBook("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            List = 40003
            CreateWorkBook = 40004
            EditWorkBook = 40005
            VirtualDeleteWorkBook = 40006
            UndeleteWorkBook = 40007
            DeleteWorkBook = 40008
            CreateItem = 40009
            EditItem = 40010
            VirtualDeleteItem = 40011
            UndeleteItem = 40012
            DeleteItem = 40013
            UploadFile = 40014
            DownloadFile = 40015
            VirtualDeleteFile = 40016
            UndeleteFile = 40017
            DeleteFile = 40018
            ListWorkBookItems = 40019

            UploadInternalFile = 40020
            UploadCommunityFile = 40021
            DeleteInternalFile = 40022
            UnlinkCommunityFile = 40023

        End Enum
        Public Enum ObjectType
            None = 0
            File = 1
            FileScorm = 2
            WorkBook = 3
            WorkBookItem = 4
            Author = 5
        End Enum

        <Flags()> Public Enum Base2Permission
            CreateWorkBook = 8192 ' 13
            AddItemsToOther = 2 '1
            ChangeOtherWorkbook = 4 '2
            CreateGroupWorkbook = 16 '4
            GrantPermission = 32 '5
            AdminService = 64 '6
            PrintOtherWorkBook = 2048 '11
            DeleteItemsFromOther = 8 '3
            ChangeApprovationStatus = 16384
            ListOtherWorkBook = 1024 '10
            ReadOtherWorkBook = 1 '0
            DownLoadItemFiles = 32768 '15
        End Enum
    End Class
End Namespace