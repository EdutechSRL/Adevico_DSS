Namespace UCServices


    Public Class Services_SocialNoticeboard
        Inherits Abstract.MyServices
        
        'Pari-pari a gestione servizio da piattaforma
        'Private _Send As Boolean
        'Private _Read As Boolean
        'Private _Delete As Boolean
        'Private _Add As Boolean
        'Private _Admin As Boolean
        Private Const Codice As String = "SRVBKSOCIAL"
        
        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"

        Public Property Send() As Boolean
            Get
                Return MyBase.GetPermissionValue(PermissionType.Send)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Send, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Send, 0)
                End If
            End Set
        End Property

        Public Property Read() As Boolean
            Get
                Return MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property

        Public Property Delete() As Boolean
            Get
                Return MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Delete, 0)
                End If
            End Set
        End Property

        Public Property Add() As Boolean
            Get
                Return MyBase.GetPermissionValue(PermissionType.Add)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Add, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Add, 0)
                End If
            End Set
        End Property

        Public Property Admin() As Boolean
            Get
                Return MyBase.GetPermissionValue(PermissionType.Admin)
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
            MyBase.PermessiAssociati = "00000000000000000000000000000000"
        End Sub

        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub

        Public Shared Function Create() As Services_SocialNoticeboard
            Return New Services_SocialNoticeboard("00000000000000000000000000000000")
        End Function

        Public Shared Function CreatePortal(ByVal UserTypeID As Integer) As Services_SocialNoticeboard

            Dim service As New Services_SocialNoticeboard
            service.Admin = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario _
                      OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo _
                      OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)

            service.Delete = service.Admin
            service.Add = service.Admin
            service.Send = (UserTypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest _
                      AndAlso UserTypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Esterno _
                      AndAlso UserTypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.PublicUser)

            service.Read = service.Send

            Return service

        End Function

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            Send = 100010
            Show = 100003
            Delete = 100009
            Create = 100004
        End Enum

        Public Enum ObjectType
            None = 0
            Post = 1
            Response = 2
        End Enum


        <Flags()> Public Enum Base2Permission
            AdminService = 64
            AddPost = 8192
            DeleteItem = 8
            ReadNoticeBoard = 1
            Send = 128
        End Enum

    End Class

End Namespace