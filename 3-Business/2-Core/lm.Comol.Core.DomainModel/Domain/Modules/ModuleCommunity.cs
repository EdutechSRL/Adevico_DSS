using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ModuleCommunity : DomainObject<long>, iModuleCommunity
	{
		public virtual Community Community { get; set; }
		Community iModuleCommunity.CommunityOwner{ get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual bool isNotificable { get; set; }
        public virtual ModuleDefinition ModuleDefinition { get; set; }
	}
}