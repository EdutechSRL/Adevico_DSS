Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleCommunityRepository
        Implements IEquatable(Of ModuleCommunityRepository)

#Region "Private Property"
        Private _UploadFile As Boolean
        Private _DeleteMyFile As Boolean
        Private _ManagementPermission As Boolean
        Private _Administration As Boolean
        Private _Edit As Boolean
        Private _DownLoad As Boolean
        Private _ListFiles As Boolean
#End Region

#Region "Public Property"
        Public Property UploadFile() As Boolean
            Get
                UploadFile = _UploadFile
            End Get
            Set(ByVal Value As Boolean)
                _UploadFile = Value
            End Set
        End Property
        Public Property DeleteMyFile() As Boolean
            Get
                DeleteMyFile = _DeleteMyFile
            End Get
            Set(ByVal Value As Boolean)
                _DeleteMyFile = Value
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                ManagementPermission = _ManagementPermission
            End Get
            Set(ByVal Value As Boolean)
                _ManagementPermission = Value
            End Set
        End Property
        Public Property Administration() As Boolean
            Get
                Administration = _Administration
            End Get
            Set(ByVal Value As Boolean)
                _Administration = Value
            End Set
        End Property
        Public Property ListFiles() As Boolean
            Get
                ListFiles = _ListFiles
            End Get
            Set(ByVal Value As Boolean)
                _ListFiles = Value
            End Set
        End Property
        Public Property Edit() As Boolean
            Get
                Edit = _Edit
            End Get
            Set(ByVal Value As Boolean)
                _Edit = Value
            End Set
        End Property
        Public Property DownLoad() As Boolean
            Get
                DownLoad = _DownLoad
            End Get
            Set(ByVal Value As Boolean)
                _DownLoad = Value
            End Set
        End Property
#End Region

        Sub New()

        End Sub

        Sub New(ByVal oService As Services_File)
            With oService
                Me.UploadFile = .Admin OrElse .Moderate OrElse .Upload
                Me.Administration = .Admin
                Me.DeleteMyFile = .Delete
                Me.DownLoad = .Admin OrElse .Moderate OrElse .Read
                Me.Edit = .Change
                Me.ManagementPermission = .GrantPermission
                Me.ListFiles = .Read
            End With
        End Sub

        Sub New(ByVal permissionValue As String)
            If (String.IsNullOrEmpty(permissionValue)) Then
                CreateFromModuleLongValue(CLng(0))
            Else
                CreateFromModuleLongValue(Convert.ToInt64(New String(permissionValue.Reverse().ToArray()), 2))
            End If
        End Sub
        Sub New(ByVal permission As Long)
            CreateFromModuleLongValue(permission)
        End Sub
        Private Sub CreateFromModuleLongValue(ByVal permission As Long)
            Me.UploadFile = PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService, permission) Or PermissionHelper.CheckPermissionSoft(Base2Permission.Moderate, permission) Or PermissionHelper.CheckPermissionSoft(Base2Permission.UploadFile, permission)
            Me.UploadFile = PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Moderate Or Base2Permission.UploadFile, permission)
            Me.Administration = PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService, permission)
            Me.DeleteMyFile = PermissionHelper.CheckPermissionSoft(Base2Permission.DeleteFile, permission)
            Me.DownLoad = PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Moderate Or Base2Permission.DownloadFile, permission)
            Me.Edit = PermissionHelper.CheckPermissionSoft(Base2Permission.EditFile, permission)
            Me.ManagementPermission = PermissionHelper.CheckPermissionSoft(Base2Permission.GrantPermission, permission)
            Me.ListFiles = PermissionHelper.CheckPermissionSoft(Base2Permission.DownloadFile, permission)

        End Sub
        Shared Function CreatePortalmodule(ByVal UserTypeID As Integer) As ModuleCommunityRepository
            Dim oService As New ModuleCommunityRepository
            With oService
                .Administration = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .DeleteMyFile = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .DownLoad = (UserTypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)
                .Edit = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .ListFiles = (UserTypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)
                .ManagementPermission = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .UploadFile = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            End With
            Return oService
        End Function

        Shared Function CreateFromCore(ByVal oModule As CoreModuleRepository) As ModuleCommunityRepository
            Dim oService As New ModuleCommunityRepository
            With oService
                .Administration = oModule.Administration
                .DeleteMyFile = oModule.DeleteMyFile
                .DownLoad = oModule.DownLoad
                .Edit = oModule.Edit
                .ListFiles = oModule.ListFiles
                .ManagementPermission = oModule.ManagementPermission
                .UploadFile = oModule.UploadFile
            End With
            Return oService
        End Function

        <Flags()> Public Enum Base2Permission As Long
            DownloadFile = 1 '0
            UploadFile = 2 '1
            EditFile = 4 '2
            DeleteFile = 8 '3
            Moderate = 16 '4
            GrantPermission = 32 '5
            AdminService = 64 '6
            PrintList = 2048 '11
            ChangeFileOwner = 4096 '11
        End Enum

        Public Function Equals1(ByVal other As ModuleCommunityRepository) As Boolean Implements System.IEquatable(Of ModuleCommunityRepository).Equals
            Return Me.Administration = other.Administration AndAlso Me.DeleteMyFile = other.DeleteMyFile AndAlso Me.DownLoad = other.DownLoad AndAlso Me.Edit = other.Edit AndAlso Me.ListFiles = other.ListFiles AndAlso Me.ManagementPermission = other.ManagementPermission AndAlso Me.UploadFile = other.UploadFile
        End Function
    End Class
End Namespace