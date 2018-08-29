Namespace UCServices
    Public Class Services_OnLineUsers
        Inherits UCServices.Abstract.MyServices
        Const Codice = "SRVOLUSR"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property ViewUsersOnLineLowDetails() As Boolean
            Get
                ViewUsersOnLineLowDetails = MyBase.GetPermissionValue(PermissionType.Receive)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Receive, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Receive, 0)
                End If
            End Set
        End Property
        Public Property ViewUsersOnLine() As Boolean
            Get
                ViewUsersOnLine = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property
        Public Property ViewUsersAndModuleOnLine() As Boolean
            Get
                ViewUsersAndModuleOnLine = MyBase.GetPermissionValue(PermissionType.Browse)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Browse, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Browse, 0)
                End If
            End Set
        End Property
        Public Property ViewUsersWithAction() As Boolean
            Get
                ViewUsersWithAction = MyBase.GetPermissionValue(PermissionType.Moderate)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Moderate, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Moderate, 0)
                End If
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                ManagementPermission = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Grant, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Grant, 0)
                End If
            End Set
        End Property
        Public Property Administration() As Boolean
            Get
                Administration = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 0)
                End If
            End Set
        End Property
        Public Property Export() As Boolean
            Get
                Export = MyBase.GetPermissionValue(PermissionType.Send)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Send, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Send, 0)
                End If
            End Set
        End Property
        Public Property Print() As Boolean
            Get
                Print = MyBase.GetPermissionValue(PermissionType.Print)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Print, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Print, 0)
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
        Public Shared Function Create() As Services_OnLineUsers
            Return New Services_OnLineUsers("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            ViewUsersOnLine = 75003
            ViewUsersOnLineModule = 75004
            ViewUsersOnLineAction = 75005
        End Enum
        Public Enum ObjectType
            None = 0
            Community = 1
        End Enum
    End Class
End Namespace