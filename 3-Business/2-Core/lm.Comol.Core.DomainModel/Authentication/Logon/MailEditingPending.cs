using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class MailEditingPending :  DomainBaseObject<long>
    {
        public virtual Person Person { get; set; }
        public virtual String ActivationCode { get; set; }
        public virtual String Mail { get; set; }
        public virtual System.Guid UrlIdentifier { get; set; }
        public virtual DateTime CreatedOn { get; set; }

        public MailEditingPending()
		{
            CreatedOn = DateTime.Now;
		}
    }
}