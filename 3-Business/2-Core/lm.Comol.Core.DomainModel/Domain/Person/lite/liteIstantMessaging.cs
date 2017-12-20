
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class liteIstantMessaging
	{
        public virtual long Id { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual String Address { get; set; }
        public virtual String Note { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual IstantMessagingType Type { get; set; }
        public virtual IstantMessagingVisibility Visibility { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public liteIstantMessaging()
		{

		}
	}
}