
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ModuleDefinition : DomainObject<int>, iModuleDefinition
	{
        public virtual bool Available { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual bool isNotificable { get; set; }

	}
}