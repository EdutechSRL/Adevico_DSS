
Namespace UCServices
    Public Class Services_Bacheca
        Inherits Abstract.MyServices

		Private _Read As Boolean
		Private _Write As Boolean
		Private _GrantPermission As Boolean
		Private _Admin As Boolean
		Private Const Codice As String = "SRVBACH"

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
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property
        Public Property Write() As Boolean
            Get
                Write = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
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
#End Region

		'      Sub New()
		'          MyBase.New()
		'End Sub
		Sub New(ByVal Permessi As String)
			MyBase.New()
			MyBase.PermessiAssociati = Permessi
		End Sub

		Public Shared Function Create() As Services_Bacheca
			Return New Services_Bacheca("00000000000000000000000000000000")
		End Function

		Public Enum ActionType
			None = 0
			NoPermission = 1
			GenericError = 2
			Show = 12003
			Create = 12004
			Edit = 12005
			Clean = 12006
			VirtualDelete = 12007
			Undelete = 12008
			Delete = 12009
			ShowHistory = 12010
			SetDefault = 12011
            ShowRecycle = 12012
            UndeleteAndActivate = 12014
		End Enum
        Public Enum ObjectType
            None = 0
            WhiteBoard = 1
        End Enum

        <Flags()> Public Enum Base2Permission
            ReadNoticeBoard = 1
            WriteNoticeBoard = 2
            GrantPermission = 32
            AdminService = 64
        End Enum
    End Class
End Namespace