

Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewMultipleFileUploader
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        Property MaxFileInputsCount() As Integer
        Property FolderID() As Long
        Property RepositoryCommunityID() As Integer
        WriteOnly Property CommunityName() As String
        WriteOnly Property FilePath() As String
        WriteOnly Property AllowFolderChange() As Boolean
        Property VisibleToDonwloaders() As Boolean
        Property DownlodableByCommunity() As Boolean
        Property AllowPersonalPermission() As Boolean
        ReadOnly Property Portalname() As String
        ReadOnly Property BaseFolder() As String

        Sub InitializeControl(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository)
        Sub LoadFolderSelector(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)

    End Interface
End Namespace