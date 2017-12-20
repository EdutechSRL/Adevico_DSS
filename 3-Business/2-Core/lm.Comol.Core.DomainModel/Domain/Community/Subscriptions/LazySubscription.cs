using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class LazySubscription : lm.Comol.Core.DomainModel.DomainObject<int>
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Boolean Accepted { get; set; }
        public virtual Boolean Enabled { get; set; }
        public virtual DateTime? SubscriptedOn { get; set; }
        public virtual DateTime? LastAccessOn { get; set; }
        public virtual Boolean isResponsabile { get; set; }
        public Boolean DisplayMail { get; set; }
    }
}