using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ExternalUser
	{
        public virtual string ExternalUserInfo { get; set; }
        public virtual int PersonOwnerID { get; set; }

        public ExternalUser()
		{
		}
	}
}