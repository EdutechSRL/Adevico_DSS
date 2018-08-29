

Namespace UCServices

    <Serializable(), CLSCompliant(True)> Public Class Services_File
        Inherits Abstract.MyServices
        'controllare se i permessi hanno un senso...
        Public Const Codex As String = "SRVMATER"

#Region "Public Property"
        Public Property Read() As Boolean
            Get
                Read = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property
        Public Property Change() As Boolean
            Get
                Change = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Change, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Change, 0)
                End If
            End Set
        End Property
        Public Property Upload() As Boolean
            Get
                Upload = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
                End If
            End Set
        End Property
        Public Property Delete() As Boolean
            Get
                Delete = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 0)
                End If
            End Set
        End Property
        Public Property Moderate() As Boolean
            Get
                Moderate = MyBase.GetPermissionValue(PermissionType.Moderate)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Moderate, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Moderate, 0)
                End If
            End Set
        End Property
        Public Property Print() As Boolean
            Get
                Print = MyBase.GetPermissionValue(PermissionType.Print)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Print, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Print, 0)
                End If
            End Set
        End Property
        Public Property GrantPermission() As Boolean
            Get
                GrantPermission = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Grant, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Grant, 0)
                End If
            End Set
        End Property
        Public Property Admin() As Boolean
            Get
                Admin = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 0)
                End If
            End Set
        End Property
        Public Property ChangeOwner() As Boolean
            Get
                ChangeOwner = MyBase.GetPermissionValue(PermissionType.ChangeOwner)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.ChangeOwner, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.ChangeOwner, 0)
                End If
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

        Public Shared Function Create() As Services_File
            Return New Services_File("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            ListForDownload = 70003
            ListForAdmin = 70004
            Statistics = 70005
            CreateFolder = 70006
            UploadFile = 70007
            DownloadFile = 70008
            PlayFile = 70009
            Details = 70010
            MoveFile = 70011
            MoveFolder = 70012
            ChangeInfo = 70013
            StartOtherUpload = 70014
            EndOtherUpload = 70015
            StartUpload = 70016
            EndUpload = 70017
            DeleteFolder = 70018
            AjaxUpdate = 70019
            DeleteFile = 70020
            ShowFile = 70021
            HideFile = 70022
            ShowFolder = 70023
            HideFolder = 70024
            ImportFiles = 70025
            DeleteMultiple = 70026
            ImportFolders = 70027
            ViewDeleteMultiplePage = 70028
            ViewImportFoldersPage = 70029
            ImportItemsCompleted = 70030
            FolderPermissionToSome = 70031
            FolderPermissionToCommunity = 70032
            FolderPermissionModifyed = 70033
            FilePermissionToCommunity = 70034
            FilePermissionToSome = 70035
            FilePermissionModifyed = 70036
            FolderEditing = 70037
            FolderEdited = 70038
            FileEditing = 70039
            FileEdited = 70040
            FolderEditingPermission = 70041
            FileEditingPermission = 70042
        End Enum
        Public Enum ObjectType
            None = 0
            File = 1
            FileScorm = 2
            Folder = 3
        End Enum

        <Flags()> Public Enum Base2Permission
            DownloadFile = 1 '0
            UploadFile = 2 '1
            EditFile = 4 '2
            DeleteFile = 8 '3
            Moderate = 16 '4
            GrantPermission = 32 '5
            AdminService = 64 '6
            PrintList = 2048 '11
            ChangeFileOwner = 4096 '11
        End Enum

    End Class
End Namespace