
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class liteModuleDefinition : DomainObject<int>
	{
        public virtual String Name { get; set; }
        public virtual Boolean IsAvailable { get; set; }
        public virtual String Code { get; set; }
        public virtual Boolean IsNotificable { get; set; }
	}
}