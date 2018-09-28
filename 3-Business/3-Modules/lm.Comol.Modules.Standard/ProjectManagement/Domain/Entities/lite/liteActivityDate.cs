using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class liteActivityDate : DomainBaseObject<long>
    {
        public virtual long IdProject { get; set; }
        public virtual long DisplayOrder { get; set; }
        #region "For CPM"
            public virtual Double EarlyStart { get; set; }
            public virtual Double EarlyFinish { get; set; }
            public virtual Double LatestStart { get; set; }
            public virtual Double LatestFinish { get; set; }
            public virtual DateTime? EarlyStartDate { get; set; }
            public virtual DateTime? EarlyFinishDate { get; set; }
            public virtual DateTime? LatestStartDate { get; set; }
            public virtual DateTime? LatestFinishDate { get; set; }
            public virtual DateTime? Deadline { get; set; }
            public virtual Boolean IsCompleted { get; set; }
            public virtual Boolean IsSummary { get; set; }
            public virtual long WBSindex { get; set; }
            public virtual String WBSstring { get; set; }
            public virtual Double Duration { get; set; }
        #endregion

        public liteActivityDate()
        {

        }
    }
}