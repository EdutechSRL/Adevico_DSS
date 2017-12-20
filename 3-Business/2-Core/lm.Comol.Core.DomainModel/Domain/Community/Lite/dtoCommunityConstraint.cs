using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class dtoCommunityConstraint
    {
        public virtual long Id { get; set; }
        public String Name { get; set; }
        public virtual Int32 IdSource { get; set; }
        public virtual Int32 IdDestinationCommunity { get; set; }
        public virtual ModuleObject Object { get; set; }
        public virtual ConstraintType Type { get; set; }
        public virtual Boolean IsRespected { get; set; }
        public virtual Boolean WillBeRespected { get; set; }
        public virtual Boolean IsReverse { get; set; }

        public dtoCommunityConstraint()
        {

        }

        public dtoCommunityConstraint(liteCommunityConstraint c,List<liteSubscriptionInfo> subscriptions, Boolean isReverse, List<liteCommunityInfo> communities= null)
        {
            Id = c.Id;
            IdSource = c.IdSource;
            IdDestinationCommunity = c.IdDestinationCommunity;
            Type= c.Type;
            IsReverse = isReverse;
            WillBeRespected = false;
            if (communities != null)
            {
                Name = (isReverse) ? communities.Where(cm => cm.Id == c.IdSource).Select(cm => cm.Name).Skip(0).Take(1).ToList().FirstOrDefault() : communities.Where(cm => cm.Id == c.IdDestinationCommunity).Select(cm => cm.Name).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            if (isReverse)
            {
                switch (c.Type)
                {
                    case ConstraintType.NotEnrolledTo:
                        WillBeRespected = false;
                        IsRespected = true;
                        break;
                    case ConstraintType.EnrolledTo:
                        WillBeRespected = true;
                        IsRespected = true;
                        break;
                }
            }
            else
            {
                switch (c.Type)
                {
                    case ConstraintType.NotEnrolledTo:
                        IsRespected = !(from s in subscriptions where s.IdCommunity == c.IdDestinationCommunity select s).Any();
                        break;
                    case ConstraintType.EnrolledTo:
                        IsRespected = (from s in subscriptions where s.IdCommunity == c.IdDestinationCommunity select s).Any();
                        break;
                }
            }
           
        }
    }
}