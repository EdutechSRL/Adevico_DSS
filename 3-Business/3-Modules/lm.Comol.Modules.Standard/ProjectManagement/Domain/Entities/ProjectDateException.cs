using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class ProjectDateException : DomainBaseObjectLiteMetaInfo<long> 
    {

        public virtual ProjectCalendar Calendar { get; set; }
        public virtual Project Project { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32 Day { get; set; }
        public virtual Int32 Month { get; set; }
        public virtual Int32 Year { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual Boolean EveryYear { get; set; }
        public virtual Boolean EveryMonth { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual DateType Type { get; set; }
        public virtual Boolean Include { get; set; }
        public ProjectDateException()
        {

        }
    }
}