using System;
namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class DescriptionEventItem : lm.Comol.Core.DomainModel.DomainObject<long>
    {
        public virtual String Description { get; set; }
        public virtual String DescriptionPlain { get; set; }
        public DescriptionEventItem()
        {
        }
    }
}