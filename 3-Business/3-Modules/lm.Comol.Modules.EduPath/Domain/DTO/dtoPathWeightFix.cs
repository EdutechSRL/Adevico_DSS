using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoPathWeightFix
    {
        public Int64 Id { get; set; }
        //public Path Path { get; set; }
        public String Name { get; set; }

        //public Community Community { get; set; }
        public Int32 CommunityId { get; set; }
        public String CommunityName { get; set; }

        public Int64 SavedWeightAuto { get; set; }
        public Int64 CalculatedWeightAuto { get; set; }
        public Int64 SavedWeight { get; set; }

        public Boolean Fix { get; set; }
        public Boolean NeedFix
        {
            get
            {
                return SavedWeightAuto != CalculatedWeightAuto || CalculatedWeightAuto > SavedWeightAuto;
            }
        }
    }
}
