using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    [Serializable()]
    public class DtoSkinsID
    {
        public Int64 PortalId { get; set; }
        public Int64 OrganizationId { get; set; }
        public Int64 CommunityId { get; set; }
    }
}
