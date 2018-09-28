using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoCommunityProjectTasksGroup
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String IdRow { get; set; }
        public virtual String IdRowFirstItem { get; set; }
        public virtual String IdRowLastItem { get; set; }
        public virtual Int32 CellsCount { get; set; }
        public virtual List<dtoTasksGroup> Projects { get; set; }
        public virtual Int32 PreviousPageIndex { get; set; }
        public virtual Int32 NextPageIndex { get; set; }

        public dtoCommunityProjectTasksGroup() 
        {
            Projects = new List<dtoTasksGroup>();
            PreviousPageIndex = -1;
            NextPageIndex = -1;
        }       
    }
}