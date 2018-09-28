using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
[Serializable]
    public class dtoValidateEp : IdtoValidate
    {
        public long id { get; set; }
        public Int16 Mark { get; set; }
        public Int64 Completion { get; set; }
        public IList<dtoValidateUnit> Units { get; set; }
        public StatusStatistic Status { get; set; }

        public dtoValidateEp() 
        {
            Units = new List<dtoValidateUnit>();
        }
    }
    [Serializable]
    public class dtoValidateUnit :IdtoValidate
    {
        public long id { get; set; }
        public Int16 Mark { get; set; }
        public Int64 Completion { get; set; }
        public IList<dtoValidateActivity> Activities { get; set; }
        public StatusStatistic Status { get; set; }              

        public dtoValidateUnit() 
        {
            Activities = new List<dtoValidateActivity>();
        }
    }
    [Serializable]
    public class dtoValidateActivity : IdtoValidate
    {
        public long id { get; set; }
        public Int16 Mark { get; set; }
        public Int64 Completion { get; set; }
        public StatusStatistic Status { get; set; }             

        public dtoValidateActivity() { }

    }

    public interface IdtoValidate
    {
         long id { get; set; }
         Int16 Mark { get; set; }
         Int64 Completion { get; set; }
         StatusStatistic Status { get; set; }              
    }

}