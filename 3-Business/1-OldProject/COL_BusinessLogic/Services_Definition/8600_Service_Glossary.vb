Namespace UCServices

    <Serializable(), CLSCompliant(True)> Public Class Services_Glossary
        Inherits Abstract.MyServices

        Const Code = "SRVGLS"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Code
            End Get
        End Property

#Region "Public Property"
        '    None = -1               '<- NO PERMISSIONE
        '    Read = 0 '1             '<- Visualizzazione Terms
        '    Write = 1 '2            '<- Crea Terms
        '    Change = 2 '4           '<- Modifica Terms
        '    Delete = 3 '8           '<- Cancella Terms
        '    Moderate = 4 '16        '<- Manage dei Glossary
        '    Admin = 6 '64           '<- ALL


        Public Property ViewTerms() As Boolean
            Get
                ViewTerms = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property CreateTerm() As Boolean
            Get
                CreateTerm = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property ModifyTerm() As Boolean
            Get
                ModifyTerm = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property DeleteTerm() As Boolean
            Get
                DeleteTerm = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Delete, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property ManageGlossary() As Boolean
            Get
                ManageGlossary = MyBase.GetPermissionValue(PermissionType.Moderate)
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

            Me.ViewTerms = False
            Me.CreateTerm = False
            Me.ModifyTerm = False
            Me.DeleteTerm = False
            Me.ManageGlossary = False
            Me.Admin = False
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Services_Glossary
            Return New Services_Glossary("00000000000000000000000000000000")
        End Function
        Public Enum ActionType
            None = 86000
            NoPermission = 86001
            GenericError = 86002

            List = 86002    'User see Community Items

            Manage = 86003  'Manage Items or Glossary
            Create = 86004  '...
            Modify = 86005
            Delete = 86006

        End Enum
        Public Enum ObjectType
            None = 0
            Item = 1
            Glossary = 2
        End Enum


        <Flags()> Public Enum Base2Permission
            ViewTerms = 1         '0      '<- Visualizzazione Terms
            CreateTerm = 2       '1      '<- Scrive/Crea Terms
            ModifyTerm = 4       '2      '<- Modifica Terms
            DeleteTerm = 8       '3      '<- Cancella Terms
            ManageGlossary = 16 '4      '<- Manage delle CATEGORY
            Admin = 64          '6      '<- ALL
        End Enum

    End Class

End Namespace