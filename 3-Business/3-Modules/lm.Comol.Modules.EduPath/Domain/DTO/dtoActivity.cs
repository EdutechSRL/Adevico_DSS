using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{


    [Serializable]
    public class dtoActivity:dtoGenericItem
    {
        public bool isRuleChecked { get; set; }
        public StatusStatistic statusStat { get; set; }
        public IList<dtoSubActivity> SubActivities { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
      
        public virtual Boolean isQuiz { get; set; }

        public dtoActivity()
            :base()
        {
            
            SubActivities = new List<dtoSubActivity>();
            isRuleChecked = false;
            EndDate = null;
            StartDate = null;
       
        }
        public dtoActivity(Activity oActivity, Status PersonalStatus, RoleEP RoleEP, bool isRuleChecked)
       :base(oActivity,PersonalStatus,RoleEP)
        {                    
            SubActivities = new List<dtoSubActivity>();
            this.isRuleChecked = isRuleChecked;
            EndDate = oActivity.EndDate;
            StartDate = oActivity.StartDate;         
            isQuiz = oActivity.isQuiz;
        }

        public dtoActivity(Activity oActivity, Status PersonalStatus, RoleEP RoleEP, bool isRuleChecked, IList<dtoSubActivity> SubActivities)
            : base(oActivity,PersonalStatus, RoleEP)
        {                      
            this.SubActivities = SubActivities;
            this.isRuleChecked = isRuleChecked;
            EndDate = oActivity.EndDate;
            StartDate = oActivity.StartDate;          
            isQuiz = oActivity.isQuiz;
        }
   

    }
}
