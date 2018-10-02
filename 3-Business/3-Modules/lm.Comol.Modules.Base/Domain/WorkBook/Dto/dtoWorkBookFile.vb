Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoWorkBookFile

        Private _ID As System.Guid
        Private _Permission As WorkBooFilePermission
#Region "Public Property"
        Public Property ID() As System.Guid
            Get
                Return _ID
            End Get
            Set(ByVal value As System.Guid)
                _ID = value
            End Set
        End Property
        Public Property Permission() As WorkBooFilePermission
            Get
                Return _Permission
            End Get
            Set(ByVal value As WorkBooFilePermission)
                _Permission = value
            End Set
        End Property
        Public Name As String
        Public Extension As String
        Public Size As Long
        Public isVisibile As Boolean
        Public isSCORM As Boolean
        Public isVideocast As Boolean
        Public isCommunityFile As Boolean

        Public Approvation As ApprovationStatus
        Public ApprovedBy As Person
        Public ApprovedOn As DateTime?
        Public Owner As Person
        Public CreatedBy As Person
        Public CreatedOn As DateTime?
        Public Overridable ReadOnly Property isApproved() As Boolean
            Get
                Return (Approvation = MetaApprovationStatus.Approved Or Approvation = MetaApprovationStatus.Ignore)
            End Get
        End Property
        Public isDeleted As Boolean
        Public ModifiedBy As Person
        Public ModifiedOn As DateTime
        Public ItemOwner As System.Guid
        Public CommunityFileID As Long
        Public CommunityFileGuidID As System.Guid
        Public InternalFileID As System.Guid
        Public FromItemDeleted As Boolean

        Public AllowPublish As Boolean
#End Region


        Public Sub New()
            Me.Permission = New WorkBooFilePermission
            Me.FromItemDeleted = False
            Me.AllowPublish = False
        End Sub
        Public Sub New(ByVal oWorkbookFile As WorkBookFile)
            Me.BaseInfo(oWorkbookFile)
            Me.Permission = New WorkBooFilePermission
            Me.FromItemDeleted = False
            Me.AllowPublish = False
        End Sub
        Public Sub New(ByVal oCommunityFile As WorkBookCommunityFile, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository, ByVal isItemDeleted As Boolean)
            Me.BaseInfo(oCommunityFile)

            Me.isCommunityFile = True
            Me.ID = oCommunityFile.Id
            Me.Name = oCommunityFile.FileCommunity.Name
            Me.Size = oCommunityFile.FileCommunity.Size
            Me.isVisibile = oCommunityFile.FileCommunity.isVisible
            Me.isSCORM = oCommunityFile.FileCommunity.isSCORM
            Me.isVideocast = oCommunityFile.FileCommunity.isVideocast
            CommunityFileID = oCommunityFile.FileCommunity.Id
            CommunityFileGuidID = oCommunityFile.FileCommunity.UniqueID
            Me.Extension = oCommunityFile.FileCommunity.Extension
            Me.Permission = New WorkBooFilePermission
            With Me._Permission
                .ChangeApprovation = oPermission.ChangeApprovation
                .Delete = False
                .Play = oCommunityFile.FileCommunity.isSCORM OrElse oCommunityFile.FileCommunity.isVideocast
                .Download = ((Not .Play Or oCommunityFile.FileCommunity.IsDownloadable) AndAlso (Not oCommunityFile.isDeleted OrElse oPermission.DownLoadFile)) OrElse oModule.Administration  'orelse oCommunityFile.FileCommunity.IsDownloadable
                .UnDelete = False
                .Unlink = oPermission.Delete
                .VirtualDelete = False
            End With

            Me.FromItemDeleted = isItemDeleted
            Me.AllowPublish = False
        End Sub

        Public Sub New(ByVal oInternalFile As WorkBookInternalFile, ByVal oPermission As WorkBookItemPermission, ByVal isItemDeleted As Boolean, ByVal pAllowPublish As Boolean)
            Me.BaseInfo(oInternalFile)
            Me.ID = oInternalFile.Id
            Me.InternalFileID = oInternalFile.File.Id
            Me.Name = oInternalFile.File.Name & oInternalFile.File.Extension
            Me.Size = oInternalFile.File.Size
            Me.isVisibile = True
            Me.isSCORM = False
            Me.isVideocast = False
            Me.Extension = oInternalFile.File.Extension
            Me._Permission = New WorkBooFilePermission
            With Me._Permission
                .ChangeApprovation = oPermission.ChangeApprovation
                .Delete = oInternalFile.isDeleted AndAlso oPermission.Delete
                .Download = Not oInternalFile.isDeleted OrElse oPermission.DownLoadFile
                .Play = False
                .UnDelete = oInternalFile.isDeleted AndAlso oPermission.Delete
                .Unlink = False
                .VirtualDelete = Not oInternalFile.isDeleted AndAlso oPermission.Delete
            End With
            Me.FromItemDeleted = isItemDeleted
            Me.AllowPublish = pAllowPublish
        End Sub

        Private Sub BaseInfo(ByVal oWorkbookFile As WorkBookFile)
            Me.Approvation = oWorkbookFile.Approvation
            Me.ApprovedBy = oWorkbookFile.ApprovedBy
            Me.ApprovedOn = oWorkbookFile.ApprovedOn
            Me.Owner = oWorkbookFile.Owner
            Me.CreatedBy = oWorkbookFile.CreatedBy
            Me.CreatedOn = oWorkbookFile.CreatedOn
            If oWorkbookFile.ModifiedOn.HasValue Then
                Me.ModifiedOn = oWorkbookFile.ModifiedOn.Value
            Else
                Me.ModifiedOn = Me.CreatedOn
            End If
            Me.isDeleted = oWorkbookFile.isDeleted
            Me.ModifiedBy = oWorkbookFile.ModifiedBy
            Me.ItemOwner = oWorkbookFile.ItemOwner.Id
            Me.isCommunityFile = False
        End Sub


    End Class
End Namespace