using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class WaitingActivationProfile :  DomainBaseObject<long>
    {
        public virtual Person Person { get; set; }
        public virtual System.Guid UrlIdentifier { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public WaitingActivationProfile()
		{
            CreatedOn = DateTime.Now;
		}
    }
}