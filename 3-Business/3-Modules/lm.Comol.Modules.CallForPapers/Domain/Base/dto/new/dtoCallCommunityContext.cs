using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
	public class dtoCallCommunityContext
	{
        public virtual Int32 IdCommunity { get; set; }
        public virtual String CallName { get; set; }
        public virtual String CommunityName { get; set; }

	}
}
