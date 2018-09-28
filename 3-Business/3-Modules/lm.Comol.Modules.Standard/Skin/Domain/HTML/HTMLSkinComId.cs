using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.HTML
{
    [Serializable()]

    public class HTMLSkinComId
    {
        public Int64 CommunitySkinID { get; set; }
        public Int64 OrganizationSkinID { get; set; }

        public HTMLSkinComId()
        {
            CommunitySkinID = -1;
            OrganizationSkinID = -1;
        }
    }
}
