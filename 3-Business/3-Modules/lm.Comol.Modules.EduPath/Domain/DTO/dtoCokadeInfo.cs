using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain.DTO
{
    public class dtoCokadeInfo
    {
        //Info per link al path
        public long PathId { get; set; }
        public int CommunityId { get; set; }

        //Alt su Stellina
        public string PathName { get; set; }

        //Completamento e tipo
        public dtoCokadeMoocInfo Info { get; set; }
    }
}
