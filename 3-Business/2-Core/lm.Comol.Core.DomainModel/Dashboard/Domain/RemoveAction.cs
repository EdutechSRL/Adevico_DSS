﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public enum RemoveAction
    {
        None = 0,
        FromAllSubCommunities = 1,
        FromCommunity = 2,
    }
}