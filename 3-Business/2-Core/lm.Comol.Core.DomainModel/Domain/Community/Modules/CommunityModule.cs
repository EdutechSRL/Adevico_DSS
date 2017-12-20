using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityModule : DomainObject<int>, iCommunityModuleAssociation
	{
        public virtual Community Community { get; set; }
        public virtual ModuleDefinition Service { get; set; }
        public virtual Boolean isNotificabile { get; set; }
        public virtual Boolean Enabled { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public CommunityModule() { }
	}
}