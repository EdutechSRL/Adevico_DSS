using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoCriterionForEvaluation :dtoBase 
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual Boolean IsInvalid { get; set; }
        public virtual int  DisplayOrder { get; set; }
        public virtual String DisplayId
        {
            get
            {
                return (Id > 0) ? "C" + Id.ToString() : "G" + Id.ToString();
            }
        }
        public dtoCriterionForEvaluation()
            : base()
        {
        }

        public dtoCriterionForEvaluation(long id, String name, int display, EvaluationStatus status, Boolean invalid)
            : base()
        {
            Id = id;
            DisplayOrder = display;
            Name = name;
            Status = status;
            IsInvalid = invalid;
        }
    }
}