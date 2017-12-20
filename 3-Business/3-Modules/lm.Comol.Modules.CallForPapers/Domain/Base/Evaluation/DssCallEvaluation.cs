using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class DssCallEvaluation : DomainBaseObject<long>
    {
        public virtual long IdCall { get; set; }

        public virtual long IdSubmission { get; set; }
        public virtual long IdCommittee { get; set; }
        public virtual long IdEvaluation { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual DssEvaluationType Type { get; set; }
        public virtual double Ranking { get; set; }
        public virtual double Value { get; set; }
        public virtual String ValueFuzzy { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.RatingType RatingType { get; set; }
        
        public virtual DateTime LastUpdateOn { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual Boolean IsValid { get; set; }
        public DssCallEvaluation()
        {
            Deleted = BaseStatusDeleted.None;
        }

        public static DssCallEvaluation Create(long idSubmission, long idEvaluation, Boolean isCompleted, Boolean isValid, double ranking, double value, string fuzzy = "", lm.Comol.Core.Dss.Domain.RatingType rating = Core.Dss.Domain.RatingType.simple)
        {
            return new DssCallEvaluation() { IdSubmission = idSubmission, IdEvaluation = idEvaluation, IsCompleted = isCompleted, IsValid = isValid, Ranking = ranking, Value = value, ValueFuzzy = fuzzy, RatingType = rating };
        }
        public static DssCallEvaluation CreateForCall(long idCall, long idSubmission, Boolean isCompleted, Boolean isValid, double ranking, double value, string fuzzy = "", lm.Comol.Core.Dss.Domain.RatingType rating = Core.Dss.Domain.RatingType.simple)
        {
            DssCallEvaluation dto = Create(idSubmission, 0, isCompleted, isValid, ranking,value, fuzzy, rating);
            dto.Type = DssEvaluationType.Call;
            dto.IdCall = idCall;
            return dto;
        }
        public static DssCallEvaluation CreateForCommittee(long idCommittee, long idCall, long idSubmission, Boolean isCompleted, Boolean isValid, double ranking, double value, string fuzzy = "", lm.Comol.Core.Dss.Domain.RatingType rating = Core.Dss.Domain.RatingType.simple)
        {
            DssCallEvaluation dto = CreateForCall(idCall, idSubmission, isCompleted, isValid, ranking, value, fuzzy, rating);
            dto.Type = DssEvaluationType.Committee;
            dto.IdCommittee = idCommittee;
            return dto;
        }
        public static DssCallEvaluation CreateForEvaluator(long idEvaluator, long idCommittee, long idCall, long idSubmission, long idEvaluation, Boolean isCompleted, Boolean isValid, double ranking, double value, string fuzzy = "", lm.Comol.Core.Dss.Domain.RatingType rating = Core.Dss.Domain.RatingType.simple)
        {
            DssCallEvaluation dto = CreateForCommittee(idCommittee, idCall, idSubmission, isCompleted, isValid, ranking, value, fuzzy, rating);
            dto.IdEvaluation = idEvaluation;
            dto.Type = DssEvaluationType.Evaluator;
            dto.IdEvaluator = idEvaluator;
            return dto;
        }
    }

    [Serializable]
    public enum DssEvaluationType
    {
        None = 0,
        Evaluator = 1,
        Committee = 2,
        Call = 3,
    }
}