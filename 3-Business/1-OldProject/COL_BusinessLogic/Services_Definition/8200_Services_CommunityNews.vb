Namespace UCServices
    <CLSCompliant(True)> Public Class Services_CommunityNews
        Inherits Abstract.MyServices
        Private Const Codice As String = "SRVCMNTNEWS"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property ViewMyNews() As Boolean
            Get
                ViewMyNews = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property DeleteMyNews() As Boolean
            Get
                DeleteMyNews = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Delete, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                ManagementPermission = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Grant, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property ViewOtherNews() As Boolean
            Get
                ViewOtherNews = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Admin, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property DeleteOtherNews() As Boolean
            Get
                ViewOtherNews = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
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

        Public Shared Function Create() As Services_CommunityNews
            Return New Services_CommunityNews("00000000000000000000000000000000")
        End Function

        <CLSCompliant(True)> Public Enum ActionType
            None = 0
            NoPermission = 82001
            GenericError = 82002
            ViewMyNews = 82003
            DeleteMyNews = 820004
            ViewOtherNews = 82005
            DeleteOtherMyNews = 82006
            AddNews = 82007
            LoadNews = 82008
            ManageTemplates = 82009
            ManagePersonalSettings = 82010
            ManageUserSettings = 82011
            ViewWeekNews = 82012
            ViewMonthNews = 82013
            ViewLastNews = 82014
            ViewTodayNews = 82015
            ViewYesterdayNews = 82016
            ViewDayNews = 82017
            ViewAllNews = 82018
        End Enum

        Public Enum ObjectType
            None = 0
            News = 1
            Community = 2
            Person = 3
            Day = 4
        End Enum

        <Flags()> Public Enum Base2Permission
            ViewMyNews = 1 '0
            DeleteOtherNews = 4 '2
            DeleteMyNews = 8 '3
            GrantPermission = 32 '5
            ViewOtherNews = 64 '6
        End Enum
    End Class
End Namespace
