using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class liteBaseStatistic : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual Int32 IdPerson { get; protected set; }
        public virtual long IdPath { get; protected set; }
        public virtual long? IdUnit { get; protected set; }
        public virtual long? IdActivity { get; protected set; }
        public virtual long? IdSubActivity { get; protected set; }
        public virtual StatusStatistic Status { get; protected set; }
        public virtual DateTime CreatedOn { get;  protected set; }
        public virtual DateTime? StartDate { get;  protected set; }
        public virtual Int16 Mark { get;  protected set; }
        public virtual Int64 Completion { get;  protected set; }
        public virtual StatisticDiscriminator Discriminator { get; protected set; }

        public virtual Int16 MandatoryPassedItemCount { get;  protected set; }
        public virtual Int16 MandatoryCompletedItemCount { get;  protected set; }
        public virtual Int16 MandatoryPassedCompletedItemCount { get;  protected set; }
    }
}