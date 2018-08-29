

Namespace UCServices

    <Serializable(), CLSCompliant(True)> Public Class Services_Faq
        Inherits Abstract.MyServices

        Const Code = "SRVFAQ"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Code
            End Get
        End Property

#Region "Public Property"
        '    None = -1               '<- NO PERMISSIONE
        '    Read = 0 '1             '<- Visualizzazione FAQ
        '    Write = 1 '2            '<- Scrive/Crea FAQ
        '    Change = 2 '4           '<- Modifica FAQ
        '    Delete = 3 '8           '<- Cancella FAQ
        '    Moderate = 4 '16        '<- Manage delle CATEGORY
        '    Admin = 6 '64           '<- ALL


        Public Property Read() As Boolean
            Get
                Read = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Write() As Boolean
            Get
                Write = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property Change() As Boolean
            Get
                Change = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property Delete() As Boolean
            Get
                Change = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Delete, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property Moderate() As Boolean
            Get
                Moderate = MyBase.GetPermissionValue(PermissionType.Moderate)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Moderate, IIf(Value, 1, 0))
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
#End Region

        Sub New()
            MyBase.New()

            Me.Read = False
            Me.Write = False
            Me.Change = False
            Me.Delete = False
            Me.Moderate = False
            Me.Admin = False
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Services_Faq
            Return New Services_Faq("00000000000000000000000000000000")
        End Function
        Public Enum ActionType
            None = 84000
            NoPermission = 84001
            GenericError = 84002

            List = 84003    'User see Community Faq

            Manage = 84004  'Admin manage Faq or Category
            Create = 84005  '...
            Modify = 84006
            Delete = 84007
            'CreateFaq = 83011
            'ModifyFaq = 83012
            'DeleteFaq = 83013

            'CreateCategory = 83021
            'ModifyCategory = 83022
            'DeleteCategory = 83023

        End Enum
        Public Enum ObjectType
            None = 0
            Faq = 1
            Category = 2
        End Enum


        <Flags()> Public Enum Base2Permission
            ViewFaq = 1         '0      '<- Visualizzazione FAQ
            CreateFaq = 2       '1      '<- Scrive/Crea FAQ
            ModifyFaq = 4       '2      '<- Modifica FAQ
            DeleteFaq = 8       '3      '<- Cancella FAQ
            ManageCategory = 16 '4      '<- Manage delle CATEGORY
            Admin = 64          '6      '<- ALL
        End Enum

    End Class




    'Public Enum PermissionType
    '    None = -1               '<- NO PERMISSIONE
    '    Read = 0 '1             '<- Visualizzazione FAQ
    '    Write = 1 '2            '<- Scrive/Crea FAQ
    '    Change = 2 '4           '<- Modifica FAQ
    '    Delete = 3 '8           '<- Cancella FAQ
    '    Moderate = 4 '16        '<- Manage delle CATEGORY
    '    Admin = 6 '64           '<- ALL
    'End Enum
End Namespace