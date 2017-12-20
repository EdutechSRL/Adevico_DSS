using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityModuleAssociation : DomainObject<int>, iCommunityModuleAssociation
	{
        public virtual Community Community { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual ModuleDefinition Service { get; set; }
	}
}