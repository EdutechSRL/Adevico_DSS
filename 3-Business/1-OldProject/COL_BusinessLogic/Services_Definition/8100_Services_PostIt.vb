

Namespace UCServices
    Public Class Services_PostIt
        Inherits Abstract.MyServices
        Private Const Codice As String = "SRVPOSTIT"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property CreatePostIt() As Boolean
            Get
                CreatePostIt = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
                End If
            End Set
        End Property
        Public Property ViewPostIt() As Boolean
            Get
                ViewPostIt = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property
        Public Property Edit() As Boolean
            Get
                Edit = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Change, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Change, 0)
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
        Public Property GestionePermessi() As Boolean
            Get
                GestionePermessi = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Grant, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Grant, 0)
                End If
            End Set
        End Property
        Public Property GestioneServizio() As Boolean
            Get
                GestioneServizio = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 0)
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

        Public Shared Function Create() As Services_PostIt
            Return New Services_PostIt("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            CreatePostIt = 81003
            EditPostIt = 810004
            DeletePostIt = 81005
            ViewService = 81008
            HidePostIt = 81009
            ShowPostIt = 81010
            CreatePersonalPostIt = 81011
            CreateCommunityPostit = 81012
            CreateSystemPostIt = 81013
        End Enum

        Public Enum ObjectType
            None = 0
            Person = 1
            Community = 2
            PostIt = 3
        End Enum

        <Flags()> Public Enum Base2Permission
            ViewList = 1 '0
            Create = 2 '1
            Edit = 4 '2
            Delete = 8 '3
            GrantPermission = 32 '5
            AdminService = 64 '6
        End Enum
    End Class
End Namespace
