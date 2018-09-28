using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum GanttCssClass
    {
        None = 0,
        Summary = 1,
        Critical = 2,
        Milestone = 3
    }

//    gtaskred
//Per i task “critici”

//gtaskblue
//per i task “normali”

//ggroupblack
//per i task summary

//gmilestone
//per le milestone

}