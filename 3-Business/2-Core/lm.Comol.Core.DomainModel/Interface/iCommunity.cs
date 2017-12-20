using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iCommunity : lm.Comol.Core.DomainModel.iDomainObject<int>
	{

		string Name { get; set; }
        iPerson Creator { get; set; }
		iCommunity Father { get; set; }
		CommunityType TypeOfCommunity { get; set; }
        Boolean isArchived { get; set; }
		int IdOrganization { get; set; }
        int Level { get; set; }
        Boolean isClosedByAdministrator { get; set; }
	}
}