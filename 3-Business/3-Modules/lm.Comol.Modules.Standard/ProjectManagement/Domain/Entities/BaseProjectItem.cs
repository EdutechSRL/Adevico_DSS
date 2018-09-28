using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class BaseProjectItem : DomainBaseObjectLiteMetaInfo<long> 
    {
        public virtual liteCommunity Community { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }

        public virtual int Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual ProjectItemStatus Status { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public BaseProjectItem()
        {

        }
    }
}