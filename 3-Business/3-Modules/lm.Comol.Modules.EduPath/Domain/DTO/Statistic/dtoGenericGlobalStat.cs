using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoGenericGlobalStat
    {
        public long parentId { get; set; }
        public long itemId { get; set; }
        public long startedCount { get; set; }
        public long completedCount { get; set; }
        public long passedCount { get; set; }
        public long compPassedCount { get; set; }
        public long notStartedCount { get { return (userCount - (startedCount + compPassedCount + completedCount + passedCount)); } }
        public long userCount { get; set; }
       // public bool isMandatory { get; set; }
        public Status status { get; set; }
        public Int64 Weight { get; set; }
        public string itemName { get; set; }

        public dtoGenericGlobalStat()
        { }
    }

    [Serializable]
    public class dtoEpGlobalStat : dtoGenericGlobalStat
    {
        public IList<dtoUnitGlobalStat> childrenStat { get; set; }
        public Boolean HasSubActivityType(SubActivityType type)
        {
            return (childrenStat != null && childrenStat.Any() && childrenStat.Where(c => c.HasSubActivityType(type)).Any());
        }
        public dtoEpGlobalStat()
        { }

    }
    [Serializable]
    public class dtoUnitGlobalStat : dtoGenericGlobalStat
    {        
        public IList<dtoActivityGlobalStat> childrenStat { get; set; }
        public Boolean HasSubActivityType(SubActivityType type) { 
            return (childrenStat != null && childrenStat.Any() && childrenStat.Where(c=>c.HasSubActivityType(type)).Any());
        }
        public dtoUnitGlobalStat() { }
    }

    [Serializable]
    public class dtoActivityGlobalStat : dtoGenericGlobalStat
    {
        public IList<dtoSubActGlobalStat> childrenStat { get; set; }
        public Boolean HasSubActivityType(SubActivityType type) { 
            return (childrenStat != null && childrenStat.Any() && childrenStat.Where(c=>c.ContentType==type).Any());
        }
        public dtoActivityGlobalStat() { }
    }

    [Serializable]
    public class dtoSubActGlobalStat : dtoGenericGlobalStat
    {
        public lm.Comol.Core.DomainModel.ModuleLink ModuleLink { get; set; }
        public SubActivityType ContentType { get; set; }
        public bool canEvaluate { get; set; }
        public dtoSubActGlobalStat() { }
        public dtoSubActivity SubActivity { get; set; }
        public Boolean isSingle { get; set; }
    }

}
