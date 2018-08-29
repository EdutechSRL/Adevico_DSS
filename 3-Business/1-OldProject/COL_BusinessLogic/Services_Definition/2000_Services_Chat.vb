Namespace UCServices
    Public Class Services_CHAT
        Inherits Abstract.MyServices

        Private Const Codice As String = "SRVCHAT"

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
        Public Property Write() As Boolean
            Get
                Write = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property GestionePermessi() As Boolean
            Get
                GestionePermessi = MyBase.GetPermissionValue(PermissionType.Grant)
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
#Region "Posizione Permessi"
        'Public Function GetPosition_Read() As PermissionType
        '    Return PermissionType.Read
        'End Function
        'Public Function GetPosition_Write() As PermissionType
        '    Return PermissionType.Write
        'End Function
        'Public Function GetPosition_Grant() As PermissionType
        '    Return PermissionType.Grant
        'End Function
        'Public Function GetPosition_Admin() As PermissionType
        '    Return PermissionType.Admin
        'End Function
#End Region

        Public Shared Function Create() As Services_CHAT
            Return New Services_CHAT("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            EnterIntoChat = 20003
            ExitFromChat = 20004
            CallUser = 20005
            BlockUser = 20006
            ChangeStatus = 20007
            SendFile = 20008
            GetFile = 20009
        End Enum
        Public Enum ObjectType
            None = 0
            Message = 1
            File = 2
            FileScorm = 3
        End Enum
    End Class
End Namespace
