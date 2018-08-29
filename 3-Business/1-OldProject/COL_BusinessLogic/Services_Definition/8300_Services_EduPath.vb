Namespace UCServices
    Public Class Services_EduPath
        Inherits Abstract.MyServices
        Public Const Codex = "SRVEDUP"

        'Public Overloads Shared ReadOnly Property Codex() As String
        '    Get
        '        Codex = Code
        '    End Get
        'End Property

#Region "Public Property"

        Public Property Admin() As Boolean
            Get
                Admin = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Admin, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Browse() As Boolean
            Get
                Browse = MyBase.GetPermissionValue(PermissionType.Browse)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Browse, IIf(Value, 1, 0))
            End Set
        End Property
#End Region

        Sub New()
            MyBase.New()
            Me.Admin = False
            Me.Browse = False
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Services_EduPath
            Return New Services_EduPath("00000000000000000000000000000000")
        End Function
        Public Enum ActionType
            None = 83000
            NoPermission = 83001
            GenericError = 83002
            Create = 83003
            List = 83004
            Delete = 83005
            Edit = 83006
            Access = 83007
            Review = 83008
            DoSubActivity = 83009
            ViewOwnStat = 83010
            ViewUserStat = 83011
            Evaluate = 83012
            ModuleDisabled = 83013
            ModuleDisabledForCommunity = 83014
        End Enum
        Public Enum ObjectType
            None = 0
            EduPath = 1
            Unit = 2
            Activity = 3
            SubActivity = 4
            Assignment = 5
        End Enum


        <Flags()> Public Enum Base2Permission
            GrantPermission = 32 '5
            AdminService = 64 '6
            Add = 13 '8192
        End Enum

    End Class
End Namespace

