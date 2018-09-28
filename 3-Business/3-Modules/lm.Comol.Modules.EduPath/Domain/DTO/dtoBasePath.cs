using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class dtoBasePath
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? EndDateOverflow { get; set; }
        public virtual Boolean FloatingDeadlines { get; set; }
        public virtual Boolean SingleAction { get; set; } 
        public virtual TimeSpan? StartSpan { get; set; }
        public virtual TimeSpan? EndSpan { get; set; }
        public virtual Boolean IsMooc { get; set; }
        public dtoBasePath()
        {
            Id = 0;
            Name = "";
            SingleAction = true;
            StartSpan = new TimeSpan(0);
            EndSpan = new TimeSpan(0);
        }
    }
}