using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoPathManageStatistics
    {
        public virtual long IdPath { get; set; }
        public virtual List<long> IdAvailableUnits { get; set; }
        public virtual List<long> IdAvailableActivities { get; set; }
        public virtual long Units { get { return IdAvailableUnits.Count; } }
        public virtual long Activities { get { return IdAvailableActivities.Count; } }
        public virtual long UnitsToManage { get; set; }
        public virtual long UnitsToEvaluate { get; set; }
        public virtual long ActivitiesToManage { get; set; }
        public virtual long ActivitiesToEvaluate { get; set; }
        public virtual Boolean HasItemsToDo { get { return UnitsToManage > 0 || ActivitiesToManage > 0 || ActivitiesToEvaluate > 0 || UnitsToEvaluate>0; } }
        public dtoPathManageStatistics()
        {
            IdAvailableUnits = new List<long>();
            IdAvailableActivities = new List<long>();
        }

        public dtoPathManageStatistics(long idPath, List<dtoPathItems> items)
        {
            IdPath = idPath;
            IdAvailableUnits = items.Where(i => i.IdPath == idPath && i.Type == ItemType.Unit).Select(i => i.IdItems).DefaultIfEmpty(new List<long>()).FirstOrDefault();
            IdAvailableActivities = items.Where(i => i.IdPath == idPath && i.Type == ItemType.Activity).Select(i => i.IdItems).DefaultIfEmpty(new List<long>()).FirstOrDefault();
        }

        
    }
    [Serializable]
    public class dtoPathItems
    {
        public virtual long IdPath { get; set; }
        public virtual List<long> IdItems { get; set; }
        public virtual ItemType Type { get; set; }
    }
}