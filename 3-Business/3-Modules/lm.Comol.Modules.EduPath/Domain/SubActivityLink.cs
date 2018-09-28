using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class SubActivityLink : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual Int64 IdObject { get; set; }
        public virtual Int64 IdModule { get; set; }
        public virtual SubActivity SubActivity { get; set; }
        public virtual SubActivityLinkType ContentType { get; set; }

        public virtual Boolean Mandatory { get; set; }
        public virtual Boolean Visible { get; set; }

        public SubActivityLink()
        {
            Mandatory = false;
            Visible = false;
        }
    }

    public enum SubActivityLinkType
    {
        quiz = 1
    }
}
