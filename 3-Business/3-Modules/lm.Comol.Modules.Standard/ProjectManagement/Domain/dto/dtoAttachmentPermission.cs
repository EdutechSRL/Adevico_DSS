using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoAttachmentPermission
    {
        public virtual Boolean Delete { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean UnDelete { get; set; }
        public virtual Boolean Download { get; set; }
        public virtual Boolean Unlink { get; set; }
        public virtual Boolean Play { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean ViewMyStatistics { get; set; }
        public virtual Boolean ViewOtherStatistics { get; set; }
        public virtual Boolean SetMetadata { get; set; }
      



        public dtoAttachmentPermission()
        { 
        }

    }
}