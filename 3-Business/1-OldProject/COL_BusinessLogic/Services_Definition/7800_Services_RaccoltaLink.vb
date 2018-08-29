

Namespace UCServices
    Public Class Services_RaccoltaLink
        Inherits Abstract.MyServices

        Private Const Codice As String = "SRVLINK"
        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property List() As Boolean
            Get
                List = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property
        Public Property AddLink() As Boolean
            Get
                AddLink = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
                End If
            End Set
        End Property
        Public Property ChangeLink() As Boolean
            Get
                ChangeLink = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Change, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Change, 0)
                End If
            End Set
        End Property
        Public Property RemoveLink() As Boolean
            Get
                RemoveLink = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 0)
                End If
            End Set
        End Property
        Public Property Moderate() As Boolean
            Get
                Moderate = MyBase.GetPermissionValue(PermissionType.Moderate)
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
        Public Property ImportLink() As Boolean
            Get
                ImportLink = MyBase.GetPermissionValue(PermissionType.Send)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Send, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Send, 0)
                End If
            End Set
        End Property
        Public Property ExportLink() As Boolean
            Get
                ExportLink = MyBase.GetPermissionValue(PermissionType.Receive)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Receive, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Receive, 0)
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

        Public Shared Function Create() As Services_RaccoltaLink
            Return New Services_RaccoltaLink("00000000000000000000000000000000")
        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            AddLink = 78003
            EditLinkTitle = 78004
            DeleteLink = 78005
            ViewLinks = 78006
            ImportLink = 78007
            ExportLink = 78008
            ManageLink = 78009
            EditLinkUrl = 78010
            EditLinkTitleAndUrl = 78011
            ViewLink = 78012
        End Enum
        Public Enum ObjectType
            None = 0
            Link = 1
        End Enum

        <Flags()> Public Enum Base2Permission
            ViewLinks = 1 '0
            AddLink = 2 '1
            EditLink = 4 '2
            DeleteLink = 8 '3
            GrantPermission = 32 '5
            AdminService = 64 '6
            Moderate = 16 '4
            ImportLink = 128 '7
            ExportLink = 256 '8
        End Enum
    End Class
End Namespace