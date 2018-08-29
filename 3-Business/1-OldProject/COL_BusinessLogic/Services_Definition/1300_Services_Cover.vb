

Namespace UCServices
    Public Class Services_Cover
        Inherits Abstract.MyServices
        Private Const Codice As String = "SRVCOVER"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
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
#End Region

        Sub New()
            MyBase.New()
        End Sub
		Sub New(ByVal Permessi As String)
			MyBase.New()
			MyBase.PermessiAssociati = Permessi
		End Sub
		Public Shared Function Create() As Services_Cover
			Return New Services_Cover("00000000000000000000000000000000")
		End Function

		Public Enum ActionType
			None = 0
			NoPermission = 1
			GenericError = 2
			Show = 13003
			Create = 13004
			Edit = 13005
			Clean = 13006
			VirtualDelete = 13007
			Undelete = 13008
			Delete = 13009
		End Enum
		Public Enum ObjectType
			None = 0
			Cover = 1
        End Enum

        <Flags()> Public Enum Base2Permission
            ViewCover = 1 '0
            Management = 16 '4
            GrantPermission = 32 '5
            AdminService = 64 '6
        End Enum
    End Class
End Namespace