Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True), Flags()> Public Enum WorkBookCommunityFilter As Integer
        None = 0
        AllCommunities = 1
        CurrentCommunity = 2
        Portal = 4
    End Enum
End Namespace