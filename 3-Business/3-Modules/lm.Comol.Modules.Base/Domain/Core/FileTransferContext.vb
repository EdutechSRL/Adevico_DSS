Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class FileTransferContext
        Public SourceRepositoryFolder As String
        Public DestinationRepositoryFolder As String
        Public CommunityID As Integer
        Public DestinationFolderID As Long
        Public Visible As Boolean
        Public DownlodableByCommunity As Boolean
        Public OwnerID As Integer
        Public isPersonal As Boolean
    End Class
End Namespace