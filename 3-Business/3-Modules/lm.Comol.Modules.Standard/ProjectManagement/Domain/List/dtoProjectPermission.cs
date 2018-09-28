using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectUrls
    {
        public virtual String Edit { get; set; }
        public virtual String PhisicalDelete { get; set; }
        public virtual String EditResources { get; set; }
        public virtual String ProjectMap{ get; set; }
        public virtual String ProjectUrl { get; set; }

        public dtoProjectUrls() { }

    }
}