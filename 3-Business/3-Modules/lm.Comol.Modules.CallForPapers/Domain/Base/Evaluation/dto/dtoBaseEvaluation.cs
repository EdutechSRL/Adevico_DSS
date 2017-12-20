using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoBaseEvaluation : dtoBase 
    {
        public virtual long IdCommittee { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual long IdSubmission { get; set; }
        public virtual long IdSubmitterType { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual String EvaluatorName { get; set; }
        public virtual Boolean Evaluated { get; set; }
        public virtual Boolean Anonymous { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual String SubmittedBy { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public dtoBaseEvaluation()
        {
            Deleted = BaseStatusDeleted.None;
        }

        //public dtoBaseEvaluation(Evaluation evaluation)
        //{
        //    Deleted = evaluation.Deleted;
        //    Id = evaluation.Id;
        //    Evaluated= evaluation.Evaluated;
        //    IdCommittee= evaluation.Committee.Id;
        //    IdEvaluator= evaluation.Evaluator.Id;
        //    IdSubmission= evaluation.Submission.Id;
        //    Status= evaluation.Status;
        //    DisplayName = (evaluation.Submission!= null && evaluation.Submission.Owner !=null) ? evaluation.Submission.Owner.SurnameAndName : " -- ";
        //    Anonymous = (evaluation.Submission==null || evaluation.Submission.isAnonymous || evaluation.Submission.Owner == null);
        //    SubmittedOn = (evaluation.Submission!= null) ? evaluation.Submission.SubmittedOn : null;
        //    EvaluatorName = (evaluation.Evaluator != null && evaluation.Evaluator.Person != null) ? evaluation.Evaluator.Person.SurnameAndName : "";
        //    //SubmittedBy = (evaluation.Submission != null && evaluation.Submission.SubmittedBy!=null ) ? evaluation.Submission.SubmittedOn : null;   
        //}

        public dtoBaseEvaluation(lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expEvaluation evaluation, String anonymousDisplayName, String unknownUserName)
        {
            Deleted = evaluation.Deleted;
            Id = evaluation.Id;
            Evaluated= evaluation.Evaluated;
            //IdCommittee = evaluation.IdCommittee;
            IdCommittee = (evaluation.AdvCommission == null) ? evaluation.IdCommittee : evaluation.AdvCommission.Id;
            //IdEvaluator = evaluation.IdEvaluator;
            IdEvaluator = (evaluation.AdvEvaluator == null) ? evaluation.IdEvaluator : evaluation.AdvEvaluator.Id;

            IdSubmission = evaluation.IdSubmission;
            Status= evaluation.Status;
            Anonymous = (evaluation.Submission == null || evaluation.Submission.isAnonymous);
            DisplayName = (Anonymous) ? anonymousDisplayName : ((evaluation.Submission.Owner==null) ? unknownUserName : evaluation.Submission.Owner.SurnameAndName);
            
            SubmittedOn = (evaluation.Submission!= null) ? evaluation.Submission.SubmittedOn : null;
            EvaluatorName = (evaluation.Evaluator != null && evaluation.Evaluator.Person != null) ? evaluation.Evaluator.Person.SurnameAndName : unknownUserName;
            IdSubmitterType = (evaluation.Submission != null && evaluation.Submission.Type != null) ? evaluation.Submission.Type.Id : 0;
            //SubmittedBy = (evaluation.Submission != null && evaluation.Submission.SubmittedBy!=null ) ? evaluation.Submission.SubmittedOn : null;   
        }
    }
    
    [Serializable]
    public class dtoEvaluation : dtoBaseEvaluation
    {
        public virtual long Position  { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual DateTime? EvaluationStartedOn { get; set; }
        public virtual DateTime? EvaluatedOn { get; set; }
        public virtual String SubmitterType { get; set; }
        public virtual double AverageRating { get; set; }
        public virtual double SumRating { get; set; }

        public virtual bool BoolRating { get; set; }
        public virtual bool IsPassed { get; set; }

        public virtual dtoDssRating DssRating { get; set; }

        public virtual String Comment { get; set; }
        public virtual List<dtoCriterionEvaluated> Criteria { get; set; }
        
        public dtoEvaluation()
        {
            Deleted = BaseStatusDeleted.None;
            Criteria = new List<dtoCriterionEvaluated>();
            DssRating = new dtoDssRating() { IsValid = false  };
        }

        //public dtoEvaluation(Evaluation evaluation) :base(evaluation)
        //{
        //    ModifiedOn = evaluation.ModifiedOn;
        //    EvaluationStartedOn = evaluation.EvaluationStartedOn;
        //    EvaluatedOn = evaluation.EvaluatedOn;
        //    AverageRating= evaluation.AverageRating;
        //    SumRating= evaluation.SumRating;
        //    Comment = evaluation.Comment;
        //    Criteria = new List<dtoCriterionEvaluated>();
        //    SubmitterType = (evaluation.Submission != null) ? evaluation.Submission.Type.Name : "";
        //}
        public dtoEvaluation(
            lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expEvaluation evaluation, 
            DssCallEvaluation dssEvaluation,
            String anonymousDisplayName, 
            String unknownUserName)
            : base(evaluation, anonymousDisplayName, unknownUserName)
        {
            ModifiedOn = evaluation.LastUpdateOn;
            EvaluationStartedOn = evaluation.EvaluationStartedOn;
            EvaluatedOn = evaluation.EvaluatedOn;
            AverageRating = evaluation.AverageRating;
            SumRating = evaluation.SumRating;
            DssRating = new dtoDssRating() { IsValid = false };
            DssRating.IsValid = evaluation.UseDss && (dssEvaluation!=null && dssEvaluation.IsValid);
            DssRating.IsCompleted = evaluation.UseDss && (dssEvaluation != null && dssEvaluation.IsCompleted);
            DssRating.Ranking = evaluation.DssRanking;
            DssRating.Value = evaluation.DssValue;
            DssRating.ValueFuzzy = evaluation.DssValueFuzzy;
            DssRating.IsFuzzy = evaluation.DssIsFuzzy;
            Comment = evaluation.Comment;
            Criteria = new List<dtoCriterionEvaluated>();
            SubmitterType = (evaluation.Submission != null) ? evaluation.Submission.Type.Name : "";
        }

        public dtoEvaluation(
            lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expEvaluation evaluation, 
            DssCallEvaluation dssEvaluation, 
            String anonymousDisplayName, 
            String unknownUserName, 
            List<dtoCriterion> criteria)
            : this(evaluation,dssEvaluation, anonymousDisplayName, unknownUserName)
        {
            if (criteria != null)
                Criteria = criteria.Select(c => new dtoCriterionEvaluated(c)).ToList();
        }

        //public dtoEvaluation(Evaluation evaluation, List<dtoCriterionEvaluated> criteria)
        //    : this(evaluation)
        //{
        //    Criteria = criteria;
        //}
        public String AverageRatingToString()
        {
            return DoubleToString(AverageRating);
        }
        public String SumRatingToString(){
            return DoubleToString(SumRating);
        }
        //public String DssRatingValueToString()
        //{
        //    return DoubleToString((DssRating!= null ? DssRating.Value : 0));
        //}
        private String DoubleToString(double value)
        {
            Double fractional = value - Math.Floor(value);
            return (fractional == 0) ? String.Format("{0:N0}", value) : String.Format("{0:N2}", value);
        }
        public void UpdateValues(lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expEvaluation evaluation) {
            if (evaluation != null && evaluation.EvaluatedCriteria != null && Criteria != null)
            {
                foreach (dtoCriterionEvaluated cValue in Criteria) {
                    cValue.UpdateValue(evaluation.EvaluatedCriteria.Where(v => v.Criterion != null && v.Criterion.Id == cValue.Criterion.Id).FirstOrDefault());
                }
            }
        }
    }
}