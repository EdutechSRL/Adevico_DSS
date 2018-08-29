
Namespace UCServices
    Public Class Services_IscrizioneComunita
        Inherits Abstract.MyServices

        Private Const Codice As String = "SRVISCR_CMNT"

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

        Public Shared Function Create() As Services_IscrizioneComunita
            Return New Services_IscrizioneComunita("00000000000000000000000000000000")
        End Function
		Public Enum ActionType
			None = 0
			NoPermission = 1
			GenericError = 2
			CommunityList = 17003
			SubscribeCommunity = 17004
			Details = 17005
			EnterCommunity = 17006
			'Show = 3
			'Subscribe = 20
			'         List = 13
			'         Access = 21
			'         ListToSubscribe = 22
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