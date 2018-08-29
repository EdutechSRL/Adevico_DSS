

Namespace UCServices
    Public Class Services_ManagementSottoIscritti
        Inherits Abstract.MyServices

        Const Codice = "SRVmngmISCRITTI"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"
        'Public Property AddUser() As Boolean
        '    Get
        '        AddUser = MyBase.GetPermissionValue(PermissionType.Write)
        '    End Get
        '    Set(ByVal Value As Boolean)
        '        If Value Then
        '            MyBase.SetPermissionByPosition(PermissionType.Write, 1)
        '        Else
        '            MyBase.SetPermissionByPosition(PermissionType.Write, 0)
        '        End If
        '    End Set
        'End Property
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

        Public Property Management() As Boolean
            Get
                Management = MyBase.GetPermissionValue(PermissionType.Moderate)
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
        Public Property List() As Boolean
            Get
                List = MyBase.GetPermissionValue(PermissionType.Browse)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Browse, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Browse, 0)
                End If
            End Set
        End Property

        Public Property InfoEstese() As Boolean
            Get
                InfoEstese = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
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
    End Class
End Namespace