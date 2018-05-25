using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Domain
{
    public class SubscriptionExpiration : DomainBaseObjectLiteMetaInfo<long>
    {

        /// <summary>
        /// Id Subscription
        /// </summary>
        public virtual int SubscriptionId { get; set; }
        /// <summary>
        /// Id Comunità (portale = 0)
        /// </summary>
        public virtual int CommunityId { get; set; }
        /// <summary>
        /// Id Persona (portale = 0)
        /// </summary>
        public virtual int PersonId { get; set; }
        /// <summary>
        /// Durata periodo
        /// </summary>
        public virtual int Duration { get; set; }
        /// <summary>
        /// Inizio periodo
        /// </summary>
        public virtual DateTime? StartDate { get; set; }
        ///// <summary>
        ///// Fine periodo
        ///// </summary>
        //public virtual DateTime? EndDate { get; set; }
    }
}
