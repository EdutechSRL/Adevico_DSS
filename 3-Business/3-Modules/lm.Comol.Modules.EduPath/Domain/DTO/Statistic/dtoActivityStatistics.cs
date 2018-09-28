using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// X ora non usata....
namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoActivityStatistic
    {
        public long Id { get; set; }
        public long parentId { get; set; }
        public String Name { get; set; }
        public IList<dtoSubActivityStatistic> SubActivities { get; set; }

        public dtoActivityStatistic()
        { }
    }

    [Serializable]
    public class dtoSubActivityStatistic
    {
        // public long Id { get; set; }
        public long parentId { get; set; }
        // public SubActivityType ContentType { get; set; }
        public Int64 Completion { get; set; }
        public Int16 Mark { get; set; }
        //   public Int64 Weight { get; set; }
        public StatusStatistic StatusStat { get; set; }
        //  public Status Status { get; set; }
        //  public lm.Comol.Core.DomainModel.ModuleLink ModuleLink { get; set; }
        public bool isMandatory { get; set; }
        public bool isSingle { get; set; }
        // public string Name { get; set; }
        public string Other { get; set; }
        public Boolean ActivityCompleted { get; set; }
        public dtoSubActivity SubActivity { get; set; }

        public dtoSubActivityStatistic() { }
    }

}
