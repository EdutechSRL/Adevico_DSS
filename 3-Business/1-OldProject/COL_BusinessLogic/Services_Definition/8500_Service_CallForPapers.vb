Namespace UCServices

    <Serializable(), CLSCompliant(True)> Public Class Service_CallForPapers
        Inherits Abstract.MyServices
        Public Const Code = "SRVCFP"

#Region "Public Property"
        Public Property ListCalls() As Boolean
            Get
                ListCalls = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property AddSubmission() As Boolean
            Get
                AddSubmission = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property CreateCall() As Boolean
            Get
                CreateCall = MyBase.GetPermissionValue(PermissionType.Add)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Add, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property EditCall() As Boolean
            Get
                EditCall = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
            End Set
        End Property
        Public Property DeleteCall() As Boolean
            Get
                DeleteCall = MyBase.GetPermissionValue(PermissionType.Delete)
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
            Me.ListCalls = False
            Me.AddSubmission = False
            Me.CreateCall = False
            Me.EditCall = False
            Me.DeleteCall = False
            Me.Moderate = False
            Me.Admin = False
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Service_CallForPapers
            Return New Service_CallForPapers("00000000000000000000000000000000")
        End Function
       

        'Public Enum ActionType
        '    None = 85000
        '    NoPermission = 85001
        '    GenericError = 85002
        '    List = 85003
        '    Manage = 85004
        '    EditCall = 85008
        '    DeleteCall = 85009
        '    AddSubmission = 83010
        '    EditSubmission = 83011
        '    ConfirmSubmission = 83012
        '    DeleteSubmission = 83013
        '    EvaluateSubmission = 83014
        '    ViewSubmission = 83015
        '    ViewUnknowSubmission = 83016
        '    ViewCallSubmission = 83017
        '    ViewUnknowCallForPaper = 83018
        '    ViewPreviewCallForPaper = 83019
        'End Enum
        Public Enum ObjectType
            None = 0
            CallForPaper = 1
            FieldsSection = 2
            FieldDefinition = 3
            FieldValue = 4
            RequestedFile = 5
            SubmittedFile = 6
            SubmitterType = 7
            UserSubmission = 8
            AttachmentFile = 9
            Criterion = 10
            Evaluator = 11
            Evaluation = 12
        End Enum

        <Flags()> Public Enum Base2Permission
            ListCalls = 1         '0      '<-
            AddSubmission = 2       '1      '<- S
            EditCall = 4       '2      '<- 
            DeleteCall = 8       '3      '<- 
            Moderate = 16 '4      '<- 
            Admin = 64          '6      '<- ALL
            AddCall = 8192
        End Enum
    End Class
End Namespace