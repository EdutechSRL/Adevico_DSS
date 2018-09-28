using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoEpStructureValidate:IdtoItemValidate
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Boolean IsMooc { get; set; }
        
        public Int64 MinCompletion { get; set; }
        public Int16 MinMark { get; set; }
        public Int64 Completion { get; set; }
        public Int16 Mark { get; set; }
        public Status Status { get; set; }
        public StatusStatistic StatusStat { get; set; }
        public Int64 Weight { get; set; }
        public IList<dtoUnitStructureValidate> Units { get; set; }

        public dtoEpStructureValidate() 
        {
            Units = new List<dtoUnitStructureValidate>();
        }
    }
    [Serializable]
    public class dtoUnitStructureValidate : IdtoItemValidate
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int64 MinCompletion { get; set; }
        public Int16 MinMark { get; set; }
        public Int64 Completion { get; set; }
        public Int16 Mark { get; set; }
        public Status Status { get; set; }
        public StatusStatistic StatusStat { get; set; }
        public Int64 Weight { get; set; }
        public IList<dtoActStructureValidate> Activities { get; set; }

        public dtoUnitStructureValidate()
        {
            Activities = new List<dtoActStructureValidate>();
        }
    }
    [Serializable]
    public class dtoActStructureValidate : IdtoItemValidate
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public Int64 MinCompletion { get; set; }
        public Int16 MinMark { get; set; }
        public Int64 Completion { get; set; }
        public Int16 Mark { get; set; }
        public Status Status { get; set; }
        public StatusStatistic StatusStat { get; set; }
        public Int64 Weight { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public dtoActStructureValidate() { }
    }

    public interface IdtoItemValidate
    {
        long Id { get; set; }
        string Name { get; set; }
        Int64 MinCompletion { get; set; }
        Int16 MinMark { get; set; }
        Int64 Completion { get; set; }
        Int16 Mark { get; set; }
        Status Status { get; set; }
        StatusStatistic StatusStat { get; set; }
        Int64 Weight { get; set; }
       // bool isMandatory { get; set; }

    }
}








