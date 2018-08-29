

Namespace UCServices
    Public Class Services_ElencaComunita
        Inherits Abstract.MyServices

        Const Codice = "SRVELNC_CMNT"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
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
        Public Property List() As Boolean
            Get
                List = MyBase.GetPermissionValue(PermissionType.Read)
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

        Public Shared Function Create() As Services_ElencaComunita
            Return New Services_ElencaComunita("00000000000000000000000000000000")
        End Function
        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
			Access = 18003
            List = 18004
            CommunityDetails = 18005
            RemoveSubscription = 18006
            ViewDashBoard = 18007
        End Enum
        Public Enum ObjectType
            None = 0
            Community = 1
        End Enum

        <Flags()> Public Enum Base2Permission
            ViewCommunities = 1 '0
            GrantPermission = 32 '5
            AdminService = 64 '6
        End Enum
    End Class
End Namespace