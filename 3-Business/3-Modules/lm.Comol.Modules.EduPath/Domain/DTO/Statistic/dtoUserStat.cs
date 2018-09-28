using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
   [Serializable]
    public class dtoUserStat
    {
        public int UserId { get; set; }
        public String TaxCode { get; set; }
        public String SurnameAndName { get; set; }
        public StatusStatistic StatusStat { get; set; }
        public Int64 Completion { get; set; }
        public Int16 Mark { get; set; }   

        public dtoUserStat()
        { }
    }

    [Serializable]
    public class dtoListUserStat
    {
        public string ItemName { get; set; }
        public Status Status { get; set; }
        public IList<dtoUserStatExtended> usersStat { get; set; }
       

        public dtoListUserStat() { }
    }
    [Serializable]
    public class dtoUserStatExtended:dtoUserStat
    {
        public Int16 MinMark { get; set; }
        public Int64 MinCompletion { get; set; }
        public Int64 Weight { get; set; }

        public dtoUserStatExtended() { }
    }


    [Serializable]
    public class dtoSubActListUserStat : IdtoSubActUserList
    {
        public long IdSubActivity { get; set; }
        public lm.Comol.Core.DomainModel.ModuleLink ModuleLink { get; set; }
        public String Name { get; set; }
        public Status Status { get; set; }
        public SubActivityType ContentType { get; set; }
        public IList<dtoUserStat> usersStat { get; set; }
       

        public dtoSubActListUserStat() { }
    }
    [Serializable]
    public class dtoUserStatToEvaluate : dtoUserStat
    {
        public Int64 StatId { get; set; }

    }

    [Serializable]
    public class dtoSubActListUserToEval:IdtoSubActUserList
    {
       public long IdSubActivity { get; set; }
       public lm.Comol.Core.DomainModel.ModuleLink ModuleLink { get; set; }
       public String Name { get; set; }
       public Status Status { get; set; }
       public SubActivityType ContentType { get; set; }
       public IList<dtoUserStatToEvaluate> userStat { get; set; }
       

        public dtoSubActListUserToEval() { }
    }


    public interface IdtoSubActUserList
    {
        long IdSubActivity { get; set; }
        lm.Comol.Core.DomainModel.ModuleLink ModuleLink { get; set; }
        String Name { get; set; }
        Status Status { get; set; }
        SubActivityType ContentType { get; set; }
    }
}