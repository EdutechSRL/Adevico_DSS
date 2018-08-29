
Namespace UCServices
    Public Class Services_Eventi
        Inherits Abstract.MyServices
        Const Codice = "SRVEVENTI"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property ReadEvents() As Boolean
            Get
                ReadEvents = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property
        Public Property AddEvents() As Boolean
            Get
                AddEvents = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
                End If
            End Set
        End Property
        Public Property ChangeEvents() As Boolean
            Get
                ChangeEvents = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Change, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Change, 0)
                End If
            End Set
        End Property
        Public Property DelEvents() As Boolean
            Get
                DelEvents = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 0)
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
        Public Property AdminService() As Boolean
            Get
                AdminService = MyBase.GetPermissionValue(PermissionType.Admin)
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

#Region "Posizione Permessi"
        Public Function GetPermission_ReadEvents() As PermissionType
            Return PermissionType.Read
        End Function
        Public Function GetPermission_AddEvents() As PermissionType
            Return PermissionType.Write
        End Function
        Public Function GetPermission_ChangeEvents() As PermissionType
            Return PermissionType.Change
        End Function
        Public Function GetPermission_DelEvents() As PermissionType
            Return PermissionType.Delete
        End Function
        Public Function GetPermission_Admin() As PermissionType
            Return PermissionType.Admin
        End Function

#End Region

		Public Shared Function Create() As Services_Eventi
			Return New Services_Eventi("00000000000000000000000000000000")
		End Function



        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            AddEvent = 77003
            EditEvent = 77004
            DeleteEvent = 77005
            SearchEvent = 77006
            ViewDayEvents = 77007
            ViewWeekEvents = 77008
            ViewMonthEvents = 77009
            ViewYearEvents = 77010
            MoveItem = 77011
        End Enum
        Public Enum ObjectType
            None = 0
            CommunityEvent = 1
            ReminderEvent = 2
        End Enum

        <Flags()> Public Enum Base2Permission
            ViewEvents = 1 '0
            AddEvents = 2 '1
            EditEvent = 4 '2
            DeleteEvent = 8 '3
            GrantPermission = 32 '5
            AdminService = 64 '6
        End Enum

    End Class
End Namespace