Namespace UCServices
    Public Class Services_Mail
        Inherits Abstract.MyServices

        Private Const Codice As String = "SRVMAIL"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property SendMail() As Boolean
            Get
                SendMail = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
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

        Sub New()
            MyBase.New()
        End Sub
		Sub New(ByVal Permessi As String)
			MyBase.New()
			MyBase.PermessiAssociati = Permessi
		End Sub
		Public Shared Function Create() As Services_Mail
			Return New Services_Mail("00000000000000000000000000000000")
		End Function

		Public Enum ActionType
			None = 0
			NoPermission = 1
			GenericError = 2
			EnterService = 11003
			SendMail = 11004
		End Enum
		Public Enum ObjectType
			None = 0
			Message = 1
			Contact = 2
        End Enum



        <Flags()> Public Enum Base2Permission
            GrantPermission = 32 '5
            AdminService = 64 '6
            SendMail = 2 '1
        End Enum

    End Class
End Namespace