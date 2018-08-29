
Namespace UCServices
    Public Class Services_NotificationManagement
        Inherits UCServices.Abstract.MyServices
        Const Codice = "NOTFmanage"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property EditTemplate() As Boolean
            Get
                EditTemplate = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Change, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Change, 0)
                End If
            End Set
        End Property
        Public Property AddTemplate() As Boolean
            Get
                AddTemplate = MyBase.GetPermissionValue(PermissionType.Add)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Add, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Add, 0)
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
#End Region

        Sub New()
            MyBase.New()
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Services_NotificationManagement
            Return New Services_NotificationManagement("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            EditTemplate = 76003
            AddTemplate = 76004
        End Enum
        Public Enum ObjectType
            None = 0
            Template = 1
        End Enum
    End Class
End Namespace