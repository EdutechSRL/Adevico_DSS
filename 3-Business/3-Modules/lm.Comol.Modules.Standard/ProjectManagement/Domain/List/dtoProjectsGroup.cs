using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectsGroup
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String CssClass { get; set; }
        public virtual String IdRow { get; set; }
        public virtual String IdRowFirstItem { get; set; }
        public virtual String IdRowLastItem { get; set; }
        public virtual Int32 CellsCount { get; set; }
        public virtual List<dtoPlainProject> Projects { get; set; }
        public virtual Int32 PreviousPageIndex { get; set; }
        public virtual Int32 NextPageIndex { get; set; }
        public virtual dtoTimeGroup Time { get; set; }      

        public dtoProjectsGroup() 
        {
            Projects = new List<dtoPlainProject>();
            PreviousPageIndex = -1;
            NextPageIndex = -1;
        }
        public dtoProjectsGroup(TimeGroup timeline, long from, long to)
        {
            Projects = new List<dtoPlainProject>();
            PreviousPageIndex = -1;
            NextPageIndex = -1;
            Time = new dtoTimeGroup();
            Time.TimeLine = timeline;
            Time.FromTicks = from;
            Time.ToTicks = to;
        }

        
    }
}