using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iCommunityModuleAssociation : lm.Comol.Core.DomainModel.iDomainObject<int>
	{

		ModuleDefinition Service { get; set; }
		Community Community { get; set; }
		Boolean Enabled { get; set; }
	}
}