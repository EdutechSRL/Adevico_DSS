Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoCommunityItemRepository

        Private _Permission As RepositoryItemPermission
        Private _CommunityFile As CommunityFile
        Private _AvailableForAll As Boolean
        Private _Virtual As Boolean
        Private _DisplayName As String
#Region "Public Property"
        Public Property Permission() As RepositoryItemPermission
            Get
                Return _Permission
            End Get
            Set(ByVal value As RepositoryItemPermission)
                _Permission = value
            End Set
        End Property

        Public Property File() As CommunityFile
            Get
                Return _CommunityFile
            End Get
            Set(ByVal value As CommunityFile)
                _CommunityFile = value
            End Set
        End Property
        Public Property AvailableForAll() As Boolean
            Get
                Return _AvailableForAll
            End Get
            Set(ByVal value As Boolean)
                _AvailableForAll = value
            End Set
        End Property
        Public Property Virtual() As Boolean
            Get
                Return _Virtual
            End Get
            Set(ByVal value As Boolean)
                _Virtual = value
            End Set
        End Property
        Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal value As String)
                _DisplayName = value
            End Set
        End Property
#End Region

        Public Sub New()
            _Permission = New RepositoryItemPermission
        End Sub

        Public Sub New(ByVal oFile As CommunityFile, ByVal oPermission As ModuleCommunityRepository, ByVal CurrentUserID As Integer, ByVal isAvailableForAll As Boolean)
            Me._AvailableForAll = isAvailableForAll
            Me._CommunityFile = oFile
            _Permission = New RepositoryItemPermission

            Dim ItemOwner As Boolean = False
            If Not IsNothing(oFile.Owner) Then
                ItemOwner = oFile.Owner.Id = CurrentUserID
            End If
            Dim isMultimedia As Boolean = (File.isFile AndAlso (oFile.RepositoryItemType <> Repository.RepositoryItemType.FileStandard AndAlso oFile.RepositoryItemType <> Repository.RepositoryItemType.None))
            If oFile.isFile Then
                _Permission.Play = isMultimedia AndAlso (oPermission.Administration OrElse oPermission.DownLoad)
                _Permission.Download = oPermission.Administration OrElse (oFile.IsDownloadable AndAlso oPermission.DownLoad)
                _Permission.ViewAdvancedStatistics = isMultimedia AndAlso (oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner))
                _Permission.ViewBaseStatistics = isMultimedia AndAlso Not (oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner))
                _Permission.EditSettings = isMultimedia AndAlso (oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner))
            Else
                _Permission.Download = False
                _Permission.Link = False
                _Permission.Play = False
                _Permission.ViewAdvancedStatistics = False
                _Permission.ViewBaseStatistics = False
            End If
            If Not oFile Is Nothing Then
                If oFile.RepositoryItemType = Repository.RepositoryItemType.FileStandard Then
                    _DisplayName = oFile.DisplayName
                Else
                    _DisplayName = oFile.Name
                End If
            End If
            _Permission.ViewPermission = oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner)
            _Permission.Delete = oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.DeleteMyFile AndAlso ItemOwner)
            _Permission.UnDelete = False
            _Permission.VirtualDelete = False
            _Permission.Edit = oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner)
            _Permission.EditPermission = oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner)
        End Sub
    End Class
End Namespace