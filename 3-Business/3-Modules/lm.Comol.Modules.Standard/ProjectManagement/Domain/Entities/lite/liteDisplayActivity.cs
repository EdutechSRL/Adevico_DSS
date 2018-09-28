using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class liteDisplayActivity
    {
        public virtual long Id { get; set; }
        public virtual long IdProject { get; set; }
        public virtual long IdParent { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual long Depth { get; set; }
        public virtual long WBSindex { get; set; }
        public virtual String WBSstring { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public virtual Boolean IsSummary { get; set; }
        public virtual Double Duration { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public liteDisplayActivity()
        {
        }
    }
}