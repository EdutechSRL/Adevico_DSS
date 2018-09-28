﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class PathCommunityRole: PRoleCRole
    {
        public virtual Role RoleCommunity { get; set; }
        public virtual liteCommunity Community { get; set; }    

        public PathCommunityRole() { }
    }
}
