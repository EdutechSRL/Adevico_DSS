Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
	<Serializable(), CLSCompliant(True)> Public Enum WorkBookType
		None = -999
		Personal = 0
		PersonalShared = 1
		PersonalCommunity = 2
		Community = 3
		CommunityShared = 4
        PersonalSharedCommunity = 5
        Assignment = 6
	End Enum
End Namespace
