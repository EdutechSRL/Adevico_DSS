using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iModuleCommunity : lm.Comol.Core.DomainModel.iDomainObject<long>
	{

		Community CommunityOwner { get; set; }
		ModuleDefinition ModuleDefinition { get; set; }
		bool Enabled { get; set; }
		bool isNotificable { get; set; }
		DateTime CreatedOn { get; set; }
		DateTime ModifiedOn { get; set; }
	}
}