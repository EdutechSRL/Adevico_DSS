using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_TKattachment : DomainBaseObjectMetaInfo<long>
    {
        public virtual BaseCommunityFile File { get; set; }
        public virtual ModuleLink Link { get; set; }
        public virtual String Description { get; set; }
        public bool IsVisible { get; set; }
        public DTO_TKattachment()
        {
            Deleted = BaseStatusDeleted.None;
            IsVisible = true;
        }
    }
}


//public virtual AttachmentType Type { get; set; }
//public virtual String Url { get; set; }
//public virtual String UrlName { get; set; }

//SharedItems = new List<ProjectAttachmentLink>();