using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.EduPath.Domain
{
    public enum StatisticDiscriminator
    { 
        BaseClass = 0,
        Path = 1,
        Unit = 2,
        Activity =3,
        SubActivity =4
    }

    public abstract class BaseStatistic : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        //public virtual Int32 IdPerson { get; protected set; }
        //public virtual long IdPath { get; protected set; }
        //public virtual long? IdUnit { get; protected set; }
        //public virtual long? IdActivity { get; protected set; }
        //public virtual long? IdSubActivity { get; protected set; }

        
        public virtual litePerson Person { get; set; }
        public virtual StatusStatistic Status { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual Int16 Mark { get; set; }
        public virtual Int64 Completion { get; set; }

        public virtual StatisticDiscriminator Discriminator { get; protected set; }
        
        public BaseStatistic()
        {
            Person = null;
            Status = StatusStatistic.None;
            StartDate = null;
            EndDate = null;          
        
        }

        public BaseStatistic(litePerson person, StatusStatistic status, DateTime? startDate, DateTime? endDate,Int32 idCreatedBy,
             DateTime? createdOn, String creatorIpAddress, String creatorProxyIpAddress)
        {
            base.UpdateMetaInfo(idCreatedBy, creatorIpAddress, creatorProxyIpAddress, (createdOn.HasValue? createdOn.Value : DateTime.Now));
            Person = person;
            Status = Status;
            StartDate = StartDate;
            EndDate = EndDate;
        }

    }

}