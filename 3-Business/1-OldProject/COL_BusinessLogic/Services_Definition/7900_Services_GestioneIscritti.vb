Namespace UCServices
    Public Class Services_GestioneIscritti
        Inherits Abstract.MyServices

        Private Const Codice As String = "SRVISCRITTI"
        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property AddUser() As Boolean
            Get
                AddUser = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
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

        Public Property Management() As Boolean
            Get
                Management = MyBase.GetPermissionValue(PermissionType.Moderate)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Moderate, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Moderate, 0)
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
        Public Property List() As Boolean
            Get
                List = MyBase.GetPermissionValue(PermissionType.Browse)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Browse, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Browse, 0)
                End If
            End Set
        End Property

        Public Property InfoEstese() As Boolean
            Get
                InfoEstese = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
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

        Public Shared Function Create() As Services_GestioneIscritti
            Return New Services_GestioneIscritti("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            AddPerson = 79003
            EditSubscription = 790004
            DeleteSubscription = 79005
            AcceptUser = 79006
            BlockUser = 79007
            ViewList = 79008
            SearchUser = 79009
            SelfSubscription = 79010
            SelfWaitingSubscription = 79011
            SelfUnSubscription = 79012
            ExpirationManage = 79013
            ExpirationUpdate = 79013

        End Enum

        Public Enum ObjectType
            None = 0
            Person = 1
            Role = 2
            Community = 3
            CommunityType = 4
        End Enum

        <Flags()> Public Enum Base2Permission
            ViewInfoEstese = 1 '0
            AddUser = 2 '1
            EditSubscription = 4 '2
            DeleteSubscription = 8 '3
            Management = 16 '4
            GrantPermission = 32 '5
            AdminService = 64 '6
            PrintList = 2048 '11
            ListSubscriptions = 1024 '10
        End Enum
    End Class
End Namespace
