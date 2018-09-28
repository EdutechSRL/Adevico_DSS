using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
 [Serializable]
    public class dtoUnit:dtoGenericItem
    {
        public bool isRuleChecked { get; set; }
        public StatusStatistic statusStat { get; set; }
        public IList<dtoActivity> Activities { get; set; }

        public dtoUnit()
            :base()
        {
           
            Activities = new List<dtoActivity>();
            isRuleChecked = false;
        }
        public dtoUnit(Unit oUnit, Status PersonalStatus, RoleEP RoleEP, bool isRuleChecked, IList<dtoActivity> Activities)
        :base(oUnit,PersonalStatus,RoleEP)
        {
                    
            this.Activities = Activities;
            this.isRuleChecked = isRuleChecked;
        }

    }
}
