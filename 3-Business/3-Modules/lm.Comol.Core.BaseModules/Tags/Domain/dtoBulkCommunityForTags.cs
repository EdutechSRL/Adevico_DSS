using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    [Serializable]
    public class dtoBulkCommunityForTags
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual List<long> IdSelectedTags { get; set; }
       
        public virtual Boolean Selected { get; set; }
        public dtoBulkCommunityForTags()
        {
            IdSelectedTags = new List<long>();
            Selected = false;
        }
    }
}