Namespace UCServices

    Public Class Services_Wiki
        Inherits Abstract.MyServices
        Public Const Codex As String = "SRVWIKI"


        Public Property GestioneWiki() As Boolean
            Get
                GestioneWiki = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property GestioneSezioni() As Boolean
            Get
                GestioneSezioni = MyBase.GetPermissionValue(PermissionType.Moderate)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Moderate, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property GestioneTopics() As Boolean
            Get
                GestioneTopics = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property GestioneCronologia() As Boolean
            Get
                GestioneCronologia = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Delete, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property GrantPermission() As Boolean
            Get
                GrantPermission = MyBase.GetPermissionValue(PermissionType.Grant)
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
      
        Public Property Lettura() As Boolean
            Get
                Lettura = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property

        Sub New()
            MyBase.New()
            me.Admin = False
            Me.GestioneCronologia = False
            Me.GestioneSezioni = False
            Me.GestioneTopics = False
            Me.GestioneWiki = False
            Me.GrantPermission = False
            Me.Lettura = False
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub

        Public Shared Function Create() As Services_Wiki
            Return New Services_Wiki("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            List = 30003                'Lista topic in una sezione
            CreateTopic = 30004         'Nuovo topic
            EditTopic = 30005           'Modifica di un topic
            DeleteTopic = 30006         'Delete one specific topic
            CreateSection = 30007
            EditSection = 30008
            DeleteSection = 30009
            SearchTopic = 30010
            BackFromHistory = 30011
            ShowHistory = 30012
            ResumeTopic = 30013
            ResumeSection = 30014
            ViewCrossTopic = 30015      'Un link dal testo di un topic ad un'altro
            ViewTopic = 30016

        End Enum
        Public Enum ObjectType
            None = 0
            Wiki = 1
            Section = 2
            Topic = 3
            SearchString = 4

        End Enum

        <Flags()> Public Enum Base2Permission
            ViewTopic = 1 '0
            ManageWiki = 2 '1
            ManageSection = 16 '4
            ManageTopics = 4 '2
            ManageHistory = 8 '3
            GrantPermission = 32 '5
            AdminService = 64 '6
        End Enum
    End Class
End Namespace