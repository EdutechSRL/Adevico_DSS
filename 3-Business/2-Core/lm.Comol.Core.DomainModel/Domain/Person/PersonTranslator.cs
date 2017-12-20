
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ProfileTypeChanger : DomainObject<int>
	{
        public virtual int Discriminator { get; set; }
        public virtual int TypeID { get; set; }

        public ProfileTypeChanger()
		{
		}
	}
}