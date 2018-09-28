using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoBaseAdvancedPathStat
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public Status Status { get; set; }
        public ItemType Type  { get; set; }
        public virtual Int64 Weight { get; set; }
        public long Number { get; set; }
        public long GlobalNumber { get; set; }
        public dtoBaseAdvancedPathStat()
        { }
    }
    [Serializable]
    public class dtoAdvancedPathStat : dtoBaseAdvancedPathStat
    {
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? EndDateOverflow { get; set; }
        public virtual Int64 MinCompletion { get; set; }
        public virtual Int16 MinMark { get; set; }
        public virtual Int64 Duration { get; set; }
        public virtual EPType EPType { get; set; }
       
        public virtual Int64 WeightAuto { get; set; }

        public virtual List<dtoAdvancedUnit> Structure { get; set; }
        public virtual List<dtoAdvancedUserStat> Users { get; set; }
        public dtoAdvancedPathStat()
        {
            Structure = new List<dtoAdvancedUnit>();
            Users = new List<dtoAdvancedUserStat>();
        }

    }
    [Serializable]
    public class dtoAdvancedUnit : dtoBaseAdvancedPathStat
    {
        public dtoAdvancedPathStat Father { get; set; }
        public List<dtoAdvancedActivity> Children { get; set; }
        public dtoAdvancedUnit()
        {
            Children = new List<dtoAdvancedActivity>();
        }
        public String GetGlobalDisplayNumber()
        {
            return GlobalNumber.ToString();
        }
        public String GetFullDisplayNumber()
        {
            return Number.ToString();
        }
        public String GetDisplayNumber()
        {
            return Number.ToString();
        }
        public String ToString()
        {
            return Id + "." + Name + "-" + Type.ToString() + " GL:" + GetGlobalDisplayNumber() + " FULL:" + GetFullDisplayNumber() + " Number:" + GetDisplayNumber();
        }
    }
    [Serializable]
    public class dtoAdvancedActivity : dtoBaseAdvancedPathStat
    {
        public dtoAdvancedPathStat Path { get; set; }
        public dtoAdvancedUnit Father { get; set; }
        public List<dtoAdvancedSubActivity> Children { get; set; }
        public dtoAdvancedActivity()
        {
            Children = new List<dtoAdvancedSubActivity>();
        }

        public String GetGlobalDisplayNumber()
        {
            return GlobalNumber.ToString();
        }
        public String GetFullDisplayNumber()
        {
            return Father.GetFullDisplayNumber() + "." + GetDisplayNumber();
        }
        public String GetDisplayNumber()
        {
            return Number.ToString();
        }
        public String ToString()
        {
            return Id + "." + Name + "-" + Type.ToString() + " GL:" + GetGlobalDisplayNumber() + " FULL:" + GetFullDisplayNumber() + " Number:" + GetDisplayNumber();
        }
    }
    [Serializable]
    public class dtoAdvancedSubActivity : dtoBaseAdvancedPathStat
    {
        public dtoAdvancedPathStat Path { get; set; }
        public dtoAdvancedUnit Unit { get; set; }
        public dtoAdvancedActivity Father { get; set; }
        public SubActivityType ContentType { get; set; }
        public dtoSubActivity SubActivity { get; set; }
        public Boolean isSingle { get; set; }

        public dtoAdvancedSubActivity() { }

        public String GetGlobalDisplayNumber()
        {
            return  GlobalNumber.ToString();
        }
        public String GetFullDisplayNumber()
        {
            return Father.GetFullDisplayNumber() + "." + GetDisplayNumber();
        }
        public String GetDisplayNumber()
        {
            return Number.ToString();
        }
        public String ToString()
        {
            return Id + "." + Name + "-" + Type.ToString() + " GL:" + GetGlobalDisplayNumber() + " FULL:" + GetFullDisplayNumber() + " Number:" + GetDisplayNumber();
        }
    }

    [Serializable]
    public class dtoAdvancedUserStat
    {
        public Int32 Id { get; set; } 
        public String TaxCode { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Mail { get; set; }
        public Int32 IdRole { get; set; }
        public String RoleName { get; set; } 
        public long IdCurrentAgency { get; set; }
        public String CurrentAgency { get; set; }
        public Boolean IsDeleted { get; set; }
        //public long IdStartAgency { get; set; }
        //public long IdEndAgency { get; set; }
        public dtoAdvancedUserObjectStat PathStatistics { get; set; }
        public List<dtoAdvancedUserObjectStat> Statistics { get; set; }

        public dtoAdvancedUserStat()
        {
            Statistics = new List<dtoAdvancedUserObjectStat>();
            PathStatistics = new dtoAdvancedUserObjectStat() { Type = ItemType.Path };
        }

    }
    [Serializable]
    public class dtoAdvancedUserObjectStat
    {
        public long IdStat { get; set; }
        public long IdObject { get; set; }
        public ItemType Type { get; set; }
        public StatusStatistic StatusStat { get; set; }
        public Int64 Completion { get; set; }
        public Int16 Mark { get; set; }

    }
}