

Namespace UCServices
    Public Class Services_AmministraComunita
        Inherits Abstract.MyServices

        Private Const Codice As String = "SRVADMCMNT"
        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        Public Property CreateComunity() As Boolean
            Get
                CreateComunity = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Write, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Write, 0)
                End If
            End Set
        End Property
        Public Property Change() As Boolean
            Get
                Change = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Change, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Change, 0)
                End If
            End Set
        End Property
        Public Property Delete() As Boolean
            Get
                Delete = MyBase.GetPermissionValue(PermissionType.Delete)
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
#End Region

        Sub New()
            MyBase.New()
		End Sub
		Sub New(ByVal Permessi As String)
			MyBase.New()
			MyBase.PermessiAssociati = Permessi
		End Sub

#Region "Posizione Permessi"
        Public Function GetPermission_CreateComunity() As PermissionType
            Return PermissionType.Write
        End Function
        Public Function GetPermission_Change() As PermissionType
            Return PermissionType.Change
        End Function
        Public Function GetPermission_Delete() As PermissionType
            Return PermissionType.Delete
        End Function
        Public Function GetPermission_Moderate() As PermissionType
            Return PermissionType.Moderate
        End Function
        Public Function GetPermission_GrantPermission() As PermissionType
            Return PermissionType.Grant
        End Function
        Public Function GetPermission_Admin() As PermissionType
            Return PermissionType.Admin
        End Function

#End Region

        Public Shared Function Create() As Services_AmministraComunita
            Return New Services_AmministraComunita("00000000000000000000000000000000")
        End Function
        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
			List = 21003
			Admin = 21004
			Access = 21005
            ListToSubscribe = 21006
            CreateCommunity = 21007
            DeleteCommunity = 21008
            EditCommunity = 21009
            AdminSubCommunity = 21010
        End Enum
        Public Enum ObjectType
            None = 0
            Community = 1
        End Enum


        <Flags()> Public Enum Base2Permission
            CreateComunity = 2 '1
            EditComunity = 4 '2
            DeleteComunity = 8 '3
            Moderate = 16 '4
            GrantPermission = 32 '5
            AdminService = 64 '6
        End Enum
    End Class
End Namespace