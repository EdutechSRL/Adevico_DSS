
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class IstantMessaging : DomainBaseObjectMetaInfo<long>
	{
        public virtual Person Person { get; set; }
        public virtual String Address { get; set; }
        public virtual String Note { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual IstantMessagingType Type { get; set; }
        public virtual IstantMessagingVisibility Visibility { get; set; }
        public virtual Int32  DisplayOrder { get; set; }
        
        public IstantMessaging()
		{

		}
	}
    [Serializable]
    public enum IstantMessagingType{
        none = 0,
        skype = 1,
        icq = 2,
        yahoomessenger = 3,
        hangout = 4
    }
    [Serializable,Flags]
    public enum IstantMessagingVisibility
    {
        ignore = -1,
        none = 0,
        portalmanagers = 1,
        communityadministrators = 2,
        communitymanagers = 4,
        communitymembers = 8,
        all = 16,
    }
}