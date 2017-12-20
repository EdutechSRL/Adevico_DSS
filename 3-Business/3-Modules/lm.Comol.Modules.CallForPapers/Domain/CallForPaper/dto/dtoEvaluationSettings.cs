using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoEvaluationSettings
    {
        public virtual String AwardDate { get; set; }
        public virtual bool DisplayWinner { get; set; }

        public virtual System.DateTime? EndEvaluationOn { get; set; }
        public virtual EvaluationType EvaluationType { get; set; }
        public virtual List<EvaluationType> AllowedTypes { get; set; }
        public virtual List<EvaluationType> AllowedChangeTo { get; set; }

        public dtoEvaluationSettings()
        {
            AllowedChangeTo = new List<EvaluationType>() { EvaluationType.Sum, EvaluationType.Average, EvaluationType.Dss };
            AllowedTypes = new List<EvaluationType>() { EvaluationType.Sum, EvaluationType.Average, EvaluationType.Dss };
        }
    }
}