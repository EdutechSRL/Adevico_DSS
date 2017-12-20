using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoBaseSummaryItem {
        public virtual long Position { get; set; }
        public virtual long IdSubmission { get; set; }
        public virtual long IdRevision { get; set; }
        public virtual Int32 IdSubmissionOwner { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual Boolean Anonymous { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual String SubmitterType { get; set; }
        public virtual long IdSubmitterType { get; set; }

        public virtual bool IsAdvance { get; set; }
        public dtoBaseSummaryItem(){}
        //public dtoBaseSummaryItem(UserSubmission s, String anonymousUser)
        //{
        //    Anonymous= s.isAnonymous || (s.Owner==null);
        //    DisplayName= (s.isAnonymous || (s.Owner==null)) ? anonymousUser : s.Owner.SurnameAndName;
        //    SubmittedOn= s.SubmittedOn;
        //    SubmitterType= s.Type.Name;
        //    IdSubmitterType = s.Type.Id;
        //    IdSubmission = s.Id;
        //    IdSubmissionOwner = (s.isAnonymous || (s.Owner == null)) ? 0 : s.Owner.Id;
        //    //IdRevision = s.Revisions.Where(r => r.Deleted == BaseStatusDeleted.None && r.IsActive).Select(r => r.Id).OrderByDescending(r => r).Skip(0).Take(1).ToList().FirstOrDefault();
        //}
        public dtoBaseSummaryItem(expSubmission s, String anonymousUser, String unknownUser, List<expRevision> revisions = null )
        {
            Anonymous = s.isAnonymous || (s.Owner == null);
            DisplayName = (s.isAnonymous) ? anonymousUser : ((s.Owner == null) ? unknownUser : s.Owner.SurnameAndName);
            SubmittedOn = s.SubmittedOn;
            SubmitterType = s.Type.Name;
            IdSubmitterType = s.Type.Id;
            IdSubmission = s.Id;
            IdSubmissionOwner = (s.isAnonymous || (s.Owner == null)) ? 0 : s.Owner.Id;
            if (revisions!=null && revisions.Any())
                IdRevision = revisions.Where(r => r.IdSubmission == s.Id).Select(r => r.Id).OrderByDescending(r => r).Skip(0).Take(1).ToList().FirstOrDefault();
        }
    }

    [Serializable]
    public class dtoEvaluationSummaryItem : dtoBaseSummaryItem
    {
        public virtual Boolean Evaluated { get; set; }
        public virtual Boolean IsAdvance { get; set; }
        public virtual Boolean ShowScore { get; set; }
        public virtual List<dtoEvaluationDisplayItem> Evaluations { get; set; }
        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get { return (DssEvaluation != null ? DssEvaluation.Ranking : 0); } }
        public virtual double AverageRating {
            get
            {
                if(IsAdvance)
                {
                    return Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement
                        && e.Status != EvaluationStatus.Invalidated).Select(e => e.SumRating).Average();
                }
                return Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement 
                && e.Status != EvaluationStatus.Invalidated).Select(e => e.AverageRating).Average();
            }
        }

        public virtual double SumRating { get { return Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Select(e => e.SumRating).Sum(); } }
        public virtual Dictionary<EvaluationStatus, long> Counters { get; set; }
        
        public virtual int BoolPassedCount
        {
            get
            {
                return Evaluations.Count(ev =>
                    ev.Status != EvaluationStatus.EvaluatorReplacement
                    && ev.Status != EvaluationStatus.Invalidated
                    && ev.BoolRating
                );
            }
        }
        public virtual bool AllPassed {
            get
            {
                return (Evaluations.Count() <= Evaluations.Count(ev => ev.Passed));
            }
        }




        public virtual EvaluationStatus Status { get { 
            if (!Evaluations.Where(e => e.Status!= EvaluationStatus.None && e.Status!= EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                return EvaluationStatus.None;
            else if (Evaluations.Where(e=> !e.Evaluated && e.Status != EvaluationStatus.None).Any())
                return EvaluationStatus.Evaluating;
            else if (!Evaluations.Where(e => !e.Evaluated && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                return EvaluationStatus.Evaluated;
            else
                return EvaluationStatus.None;
        } }

        public dtoEvaluationSummaryItem()
        {
            Evaluations= new List<dtoEvaluationDisplayItem> ();
            Counters = new Dictionary<EvaluationStatus, long>() {
                { EvaluationStatus.Evaluated, 0 },
                { EvaluationStatus.Evaluating, 0 },
                { EvaluationStatus.EvaluatorReplacement, 0 },
                { EvaluationStatus.Invalidated, 0 },
                { EvaluationStatus.None, 0 },
                { EvaluationStatus.Confirmed, 0 },
            };
        }
        //public dtoEvaluationSummaryItem(UserSubmission s, String anonymousUser)
        //    : base(s,anonymousUser )
        //{
        //     Evaluations= new List<dtoEvaluationDisplayItem> ();
        //    Counters = new Dictionary<EvaluationStatus, long>() { { EvaluationStatus.Evaluated, 0 }, { EvaluationStatus.Evaluating, 0 }, { EvaluationStatus.EvaluatorReplacement, 0 }, { EvaluationStatus.Invalidated, 0 }, { EvaluationStatus.None, 0 } };
        //}

        public dtoEvaluationSummaryItem(
            dtoBaseSummaryItem item, 
            List<expEvaluation> evaluations,
            DssCallEvaluation dssEvaluation, 
            bool IsAdvanceEvaluation = false,
            bool HasScore = false)
            : this()
        {
            Anonymous = item.Anonymous;
            DisplayName = item.DisplayName;
            SubmittedOn = item.SubmittedOn;
            SubmitterType = item.SubmitterType;
            IdSubmission = item.IdSubmission;
            IdSubmissionOwner = item.IdSubmissionOwner;
            IdRevision = item.IdRevision;
            IdSubmitterType = item.IdSubmitterType;
            if (evaluations!=null && evaluations.Any())
                Evaluations = evaluations.Select(e=> dtoEvaluationDisplayItem.GetAsEvaluationsSummaryItem(e)).ToList();
            DssEvaluation = dssEvaluation;
            IsAdvance = IsAdvanceEvaluation;
            ShowScore = HasScore;
        }
        
        public void UpdateCounters() {
            Counters[EvaluationStatus.None] = Evaluations.Where(e => e.Status == EvaluationStatus.None).Count();
            Counters[EvaluationStatus.Evaluated] = Evaluations.Where(e => e.Status == EvaluationStatus.Evaluated).Count();
            Counters[EvaluationStatus.Evaluating] = Evaluations.Where(e => e.Status == EvaluationStatus.Evaluating).Count();
            Counters[EvaluationStatus.EvaluatorReplacement] = Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Count();
            Counters[EvaluationStatus.Invalidated] = Evaluations.Where(e => e.Status == EvaluationStatus.Invalidated).Count();
            Counters[EvaluationStatus.Confirmed] = Evaluations.Count(e => e.Status == EvaluationStatus.Confirmed);
        }

        public long GetEvaluationsCount(EvaluationStatus status)
        {
            return (Counters == null || !Counters.ContainsKey(status)) ? 0 : Counters[status];
        }
        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2){
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }

    }

    [Serializable]
    public class dtoCommitteesSummaryItem : dtoBaseSummaryItem
    {
        public virtual List<dtoCommitteeDisplayItem> Committees { get; set; }
        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get { return (DssEvaluation != null ? DssEvaluation.Ranking : 0); } }
        public virtual double AverageRating {
            get
            {
                return 
                    (IsAdvance)?
                        (Committees == null || Committees.Count == 0) ? 0 :
                            Committees.Select(e => e.SumRating).Average()
                    :
                        (Committees == null || Committees.Count == 0) ? 0 : 
                            Committees.Select(e => e.AverageRating).Average(); }
        }
        public virtual double SumRating { get { return (Committees==null || Committees.Count==0) ? 0 : Committees.Select(e => e.SumRating).Sum(); } }
        public virtual Dictionary<EvaluationStatus, long> Counters { get; set; }
        public virtual EvaluationStatus Status
        {
            get
            {
                if (!Committees.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.None;
                else if (Committees.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any())
                    return EvaluationStatus.Evaluating;
                else if (!Committees.Where(e => !e.Evaluated && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.Evaluated;
                else
                    return EvaluationStatus.None;
            }
        }
        public dtoCommitteesSummaryItem()
        {
            Committees = new List<dtoCommitteeDisplayItem>();
            Counters = new Dictionary<EvaluationStatus, long>() { { EvaluationStatus.Evaluated, 0 }, { EvaluationStatus.Evaluating, 0 }, { EvaluationStatus.EvaluatorReplacement, 0 }, { EvaluationStatus.Invalidated, 0 }, { EvaluationStatus.None, 0 } };
        }
        public dtoCommitteesSummaryItem(dtoBaseSummaryItem item,DssCallEvaluation dssEvaluation) :this()
        {
            Anonymous = item.Anonymous;
            DisplayName = item.DisplayName;
            SubmittedOn = item.SubmittedOn;
            SubmitterType = item.SubmitterType;
            IdSubmitterType = item.IdSubmitterType;
            IdSubmission = item.IdSubmission;
            IdSubmissionOwner = item.IdSubmissionOwner;
            IdRevision = item.IdRevision;
            DssEvaluation = dssEvaluation;
        }

    //public dtoCommitteesSummaryItem(UserSubmission s, String anonymousUser)
    //    : base(s, anonymousUser)
    //{
    //    Committees = new List<dtoCommitteeDisplayItem>();
    //    Counters = new Dictionary<EvaluationStatus, long>() { { EvaluationStatus.Evaluated, 0 }, { EvaluationStatus.Evaluating, 0 }, { EvaluationStatus.EvaluatorReplacement, 0 }, { EvaluationStatus.Invalidated, 0 }, { EvaluationStatus.None, 0 } };
    //}

        public void UpdateCounters()
        {
            var query = Committees.Where(c => c.Evaluations != null).SelectMany(c => c.Evaluations);
            Counters[EvaluationStatus.None] = query.Where(s => s.Status == EvaluationStatus.None).Count();
            Counters[EvaluationStatus.Evaluated] = query.Where(s => s.Status == EvaluationStatus.Evaluated).Count();
            Counters[EvaluationStatus.Evaluating] = query.Where(s => s.Status == EvaluationStatus.Evaluating).Count();
            Counters[EvaluationStatus.EvaluatorReplacement] = query.Where(s => s.Status == EvaluationStatus.EvaluatorReplacement).Count();
            Counters[EvaluationStatus.Invalidated] = query.Where(s => s.Status == EvaluationStatus.Invalidated).Count();
        }

        public long GetEvaluationsCount(EvaluationStatus status)
        {
            return (Counters == null || !Counters.ContainsKey(status)) ? 0 : Counters[status];
        }

        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }
    }

    [Serializable]
    public class dtoCommitteeDisplayItem : dtoBase
    {
        public virtual long  IdCommittee { get; set; }
        public virtual String CommitteeName { get; set; }
        public virtual List<dtoEvaluationDisplayItem> Evaluations { get; set; }
        public virtual displayAs Display { get; set; }
        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get { return (DssEvaluation != null ? DssEvaluation.Ranking : 0); } }
        public virtual double AverageRating { get { return (Evaluations == null || !Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()) ? 0 : Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Select(e => e.AverageRating).Sum(); } }
        public virtual double SumRating { get { return (Evaluations == null || !Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()) ? 0 : Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Select(e => e.SumRating).Sum(); } }
        public virtual Boolean Evaluated { get {return (Evaluations != null && !Evaluations.Where(e=> !e.Evaluated && e.Status!= EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement ).Any());}  }
        public virtual EvaluationStatus Status
        {
            get
            {
                if (!Evaluations.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.None;
                else if (Evaluations.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any())
                    return EvaluationStatus.Evaluating;
                else if (!Evaluations.Where(e => !e.Evaluated).Any())
                    return EvaluationStatus.Evaluated;
                else
                    return EvaluationStatus.None;
            }
        }
        public virtual Boolean isEmpty
        {
            get
            {
                return Evaluations == null || !Evaluations.Any();
            }
        }
        public virtual Boolean HasComments
        {
            get
            {
                return (Evaluations.Any() && Evaluations.Where(e=> !String.IsNullOrEmpty(e.Comment) || (e.Criteria !=null && e.Criteria.Any() && e.Criteria.Where(c=> !String.IsNullOrEmpty(c.Comment)).Any())).Any());
            }
        }
        public dtoCommitteeDisplayItem()
        {
            Evaluations = new List<dtoEvaluationDisplayItem>();
        }

        public long GetEvaluationsCount(EvaluationStatus status)
        {
            return Evaluations.Where(e => e.Status == status).Count();
        }
        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }
    }

    [Serializable]
    public class dtoCommitteeSummaryItem : dtoBaseSummaryItem
    {
        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get { return (DssEvaluation != null ? DssEvaluation.Ranking : 0); } }
        public virtual List<dtoCriterionSummaryItem> Criteria { get; set; }
        public virtual List<dtoEvaluatorDisplayItem> Evaluators { get; set; }
        public virtual double SumRating { 
            get {
                if (Evaluators == null || Evaluators.Count == 0 || !Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return 0;
                else
                    return Evaluators.Where(e=> e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Select(e => e.SumRating).Sum(); 
            } 
        }
        public virtual double AverageRating
        {
            get
            {
                if (Evaluators == null || Evaluators.Count == 0 || !Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return 0;
                else
                    return Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Select(e => e.AverageRating).Average();
            }
        }

        public virtual EvaluationStatus Status
        {
            get
            {
                if (!Evaluators.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.None;
                else if (Evaluators.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any())
                    return EvaluationStatus.Evaluating;
                else if (!Evaluators.Where(e => !e.Evaluated && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.Evaluated;
                else
                    return EvaluationStatus.None;
            }
        }
        public dtoCommitteeSummaryItem()
        {
            Evaluators = new List<dtoEvaluatorDisplayItem>();
            Criteria = new List<dtoCriterionSummaryItem>();
        }
        public dtoCommitteeSummaryItem(dtoBaseSummaryItem item)
            : this()
        {
            Anonymous = item.Anonymous;
            DisplayName = item.DisplayName;
            SubmittedOn = item.SubmittedOn;
            SubmitterType = item.SubmitterType;
            IdSubmitterType = item.IdSubmitterType;
            IdSubmission = item.IdSubmission;
            IdSubmissionOwner = item.IdSubmissionOwner;
            IdRevision = item.IdRevision;
        }

        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }

        public Boolean HasComments()
        {
            return  Evaluators.Where(e=> !String.IsNullOrEmpty(e.Comment) || e.Values.Where(v=> !String.IsNullOrEmpty(v.Comment)).Any()).Any()  || Criteria.Where(c => c.Evaluations.Where(e => !String.IsNullOrEmpty(e.Criterion.Comment)).Any()).Any();
        }
    }

    [Serializable]
    public class dtoCriterionSummaryItem : dtoCriterion 
    {
        public virtual long EvaluatorsCount { get; set; }
        public virtual long IdSubmissionReferee { get; set; }
        public virtual displayAs DisplayAs { get; set; }
        public virtual List<dtoCriterionValueSummaryItem> Evaluations { get; set; }
        public virtual double SumRating { 
            get { 
                if (Evaluations == null || Evaluations.Count==0 || !Evaluations.Where(e => e.Evaluator.MembershipStatus != MembershipStatus.Removed && e.Evaluator.MembershipStatus != MembershipStatus.Replaced && e.Criterion!=null ).Any())
                    return 0;
                else
                    return Evaluations.Where(e => e.Evaluator.MembershipStatus != MembershipStatus.Removed && e.Evaluator.MembershipStatus != MembershipStatus.Replaced && e.Criterion != null).Select(e => (double)e.Criterion.DecimalValue).Sum();
            }
        }
        public virtual double AverageRating
        {
            get
            {
                if (Evaluations == null || Evaluations.Count == 0 || !Evaluations.Where(e => e.Evaluator.MembershipStatus != MembershipStatus.Removed && e.Evaluator.MembershipStatus != MembershipStatus.Replaced && e.Criterion != null).Any())
                    return 0;
                else
                    return Evaluations.Where(e => e.Evaluator.MembershipStatus != MembershipStatus.Removed && e.Evaluator.MembershipStatus != MembershipStatus.Replaced && e.Criterion != null).Select(e => (double)e.Criterion.DecimalValue).Average();
            }
        }
        public virtual double DssRating
        {
            get
            {
                if (Evaluations == null || Evaluations.Count == 0 || !Evaluations.Where(e => e.Evaluator.MembershipStatus != MembershipStatus.Removed && e.Evaluator.MembershipStatus != MembershipStatus.Replaced && e.Criterion != null).Any())
                    return 0;
                else
                {
                    var query = Evaluations.Where(e => e.Evaluator.MembershipStatus != MembershipStatus.Removed && e.Evaluator.MembershipStatus != MembershipStatus.Replaced && e.Criterion != null).Select(e => new { Criterion = e.Criterion.Criterion, DecimalValue = e.Criterion.DecimalValue, DssValue = e.Criterion.DssValue }).ToList();
                    List<double> values =  query.Where(e=>e.Criterion.Type != CriterionType.RatingScale && e.Criterion.Type != CriterionType.RatingScaleFuzzy).Select(e => (double)e.DecimalValue).ToList();
                    values.AddRange(query.Where(e => (e.Criterion.Type == CriterionType.RatingScale || e.Criterion.Type == CriterionType.RatingScaleFuzzy) && e.DssValue != null).Select(e => e.DssValue.Value).ToList());
                    while (values.Count < query.Count())
                    {
                        values.Add(0);
                    }
                    return values.Average();
                }
                    
            }
        }
        public dtoCriterionSummaryItem()
        {
            DisplayAs = displayAs.item;
            Evaluations = new List<dtoCriterionValueSummaryItem>();
        }
        public dtoCriterionSummaryItem(BaseCriterion criterion)
            : base(criterion)
        {
          DisplayAs = displayAs.item;
          Evaluations = new List<dtoCriterionValueSummaryItem>();
        }
        public dtoCriterionSummaryItem(expCriterion criterion, long count=0)
            : base(criterion)
        {
            DisplayAs = displayAs.item;
            Evaluations = new List<dtoCriterionValueSummaryItem>();
            EvaluatorsCount = count;
        }

        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }

        public void LoadEvaluations(List<dtoEvaluatorDisplayItem> evaluators)
        {
            Evaluations = (from e in evaluators.OrderBy(e => e.EvaluatorName)
                           .ThenBy(e => e.IdMembership)
                           .ToList()
                           select new dtoCriterionValueSummaryItem()
                            {
                                Evaluator= e,
                                Criterion = e.Values.Where(v=> v.IdCriterion==  this.Id).FirstOrDefault()
                            })
                            .OrderBy(ev => ev.Evaluator.Name)
                            .ToList();
        }
    }

    [Serializable]
    public class dtoCriterionValueSummaryItem 
    {
        //public virtual long IdEvaluation { get; set; }

        public virtual dtoEvaluatorDisplayItem Evaluator { get; set; }
        public virtual dtoCriterionEvaluated Criterion { get; set; }
        public dtoCriterionValueSummaryItem()
        {

        }
    }
        
    [Serializable]
    public class dtoEvaluatorDisplayItem 
    {
        public virtual long IdSubmission { get; set; }
        public virtual long IdEvaluation { get; set; }
        public virtual long IdMembership { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual long IdCommittee { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }
        public virtual string EvaluatorName
        {
            get { return Surname + " " + Name; }
            set { }
        }
        public virtual string SubmitterName { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual DateTime? EvaluationStartedOn { get; set; }
        public virtual DateTime? EvaluatedOn { get; set; }
        public virtual DssCallEvaluation DssEvaluation { get; set; }

        public virtual double AverageRating { get; set; }
        public virtual double SumRating { get; set; }

        public virtual bool BoolRating { get; set; }
        public virtual bool IsPassed { get; set; }

        public virtual String Comment { get; set; }
        public virtual Boolean Evaluated { get; set; }
        public virtual Boolean IgnoreEvaluation { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual MembershipStatus MembershipStatus { get; set; }
        public virtual List<dtoCriterionEvaluated> Values { get; set; }

        public dtoEvaluatorDisplayItem()
        {
            Values = new List<dtoCriterionEvaluated>();
        }

        public dtoEvaluatorDisplayItem (Evaluation evaluation,DssCallEvaluation dssEvaluation) :this()
        {
            ModifiedOn = evaluation.ModifiedOn;
            EvaluationStartedOn = evaluation.EvaluationStartedOn;
            EvaluatedOn = evaluation.EvaluatedOn;
            AverageRating = evaluation.AverageRating;

            BoolRating = evaluation.BoolRating;
            IsPassed = evaluation.IsPassed;

            DssEvaluation = dssEvaluation;
            SumRating = evaluation.SumRating;
            Status = evaluation.Status;
            Evaluated = evaluation.Evaluated;
            IdCommittee = (evaluation.Committee !=null) ? evaluation.Committee.Id :0; 
        }

        public String DssRankingToString(int decimals = 2)
        {
            return GetDoubleToString((DssEvaluation!= null ? DssEvaluation.Ranking : 0), decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }
    }

    [Serializable]
    public class dtoEvaluationDisplayItem : dtoBase {
        ////public virtual String DisplayNumber { get; set; }
        ////public virtual dtoSubmissionDisplay Submission { get; set; }
        public virtual long  IdCommittee { get; set; }
        //public virtual String CommitteeName { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual String EvaluatorName { get; set; }
        public virtual displayAs Display { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual DateTime? EvaluationStartedOn { get; set; }
        public virtual DateTime? EvaluatedOn { get; set; }
        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get { return (DssEvaluation != null ? DssEvaluation.Ranking : 0); } }
        public virtual double AverageRating { get; set; }
        public virtual double SumRating { get; set; }
        public virtual bool BoolRating { get; set; }
        public virtual String Comment { get; set; }
        public virtual Boolean Evaluated { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual List<dtoCriterionEvaluated> Criteria { get; set; }

        public virtual bool Passed { get; set; }

        public dtoEvaluationDisplayItem() {
            Criteria = new List<dtoCriterionEvaluated>();
        }
        
        //public static dtoEvaluationDisplayItem GetAsEvaluationsSummaryItem(Evaluation evaluation)
        //{
        //    dtoEvaluationDisplayItem item = new dtoEvaluationDisplayItem();
        //    item.ModifiedOn = evaluation.ModifiedOn;
        //    item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
        //    item.EvaluatedOn = evaluation.EvaluatedOn;
        //    item.AverageRating = evaluation.AverageRating;
        //    item.SumRating = evaluation.SumRating;
        //    item.Status = evaluation.Status;
        //    item.Evaluated = evaluation.Evaluated;
        //    item.Id = evaluation.Id;
        //    item.IdCommittee = (evaluation.Committee != null) ? evaluation.Committee.Id : 0;
        //    item.IdEvaluator = (evaluation.Evaluator != null) ? evaluation.Evaluator.Id : 0;
        //    return item;
        //}
        public static dtoEvaluationDisplayItem GetAsEvaluationsSummaryItem(expEvaluation evaluation)
        {
            dtoEvaluationDisplayItem item = new dtoEvaluationDisplayItem();
            item.ModifiedOn = evaluation.LastUpdateOn;
            item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
            item.EvaluatedOn = evaluation.EvaluatedOn;
            item.AverageRating = evaluation.AverageRating;
            item.SumRating = evaluation.SumRating;
            item.BoolRating = evaluation.BoolRating;
            item.Passed = evaluation.IsPassed;

            item.Status = evaluation.Status;
            item.Evaluated = evaluation.Evaluated;
            item.Id = evaluation.Id;
            item.IdCommittee = (evaluation.Committee != null) ? evaluation.Committee.Id : 0;
            item.IdEvaluator = (evaluation.Evaluator != null) ? evaluation.Evaluator.Id : 0;
            return item;
        }
        public static dtoEvaluationDisplayItem GetForCommitteesSummaryItem(expEvaluation evaluation)
        {
            dtoEvaluationDisplayItem item = new dtoEvaluationDisplayItem();
            item.ModifiedOn = evaluation.LastUpdateOn;
            item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
            item.EvaluatedOn = evaluation.EvaluatedOn;
            item.AverageRating = evaluation.AverageRating;
            item.SumRating = evaluation.SumRating;
            item.BoolRating = evaluation.BoolRating;
            item.Passed = evaluation.IsPassed;

            item.Status = evaluation.Status;
            item.Evaluated = evaluation.Evaluated;
            item.Id = evaluation.Id;
            item.IdCommittee = (evaluation.Committee != null) ? evaluation.Committee.Id : 0;
            item.IdEvaluator = (evaluation.Evaluator != null) ? evaluation.Evaluator.Id : 0;
            return item;
        }
        //public static dtoEvaluationDisplayItem GetForCommitteesSummaryItem(Evaluation evaluation)
        //{
        //    dtoEvaluationDisplayItem item = new dtoEvaluationDisplayItem();
        //    item.ModifiedOn = evaluation.ModifiedOn;
        //    item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
        //    item.EvaluatedOn = evaluation.EvaluatedOn;
        //    item.AverageRating = evaluation.AverageRating;
        //    item.SumRating = evaluation.SumRating;
        //    item.Status = evaluation.Status;
        //    item.Evaluated = evaluation.Evaluated;
        //    item.Id = evaluation.Id;
        //    item.IdCommittee = (evaluation.Committee != null) ? evaluation.Committee.Id : 0;
        //    item.IdEvaluator = (evaluation.Evaluator != null) ? evaluation.Evaluator.Id : 0;
        //    return item;
        //}
        //public static dtoEvaluationDisplayItem GetForEvaluationsDisplay(Evaluation evaluation, String anonymous)
        //{
        //    dtoEvaluationDisplayItem item = new dtoEvaluationDisplayItem();
        //    item.ModifiedOn = evaluation.ModifiedOn;
        //    item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
        //    item.EvaluatedOn = evaluation.EvaluatedOn;
        //    item.AverageRating = evaluation.AverageRating;
        //    item.SumRating = evaluation.SumRating;
        //    item.Status = evaluation.Status;
        //    item.Evaluated = evaluation.Evaluated;
        //    item.Id = evaluation.Id;
        //    item.IdCommittee = (evaluation.Committee != null) ? evaluation.Committee.Id : 0;
        //    item.IdEvaluator = (evaluation.Evaluator != null) ? evaluation.Evaluator.Id : 0;
        //    item.EvaluatorName = (evaluation.Evaluator != null && evaluation.Evaluator.Person != null && evaluation.Evaluator.Person.TypeID != (int)UserTypeStandard.Guest) ? evaluation.Evaluator.Person.SurnameAndName : anonymous;
        //    return item;
        //}
        //public dtoEvaluationDisplayItem(Evaluation evaluation, Boolean full)
        //{
        //    ModifiedOn = evaluation.ModifiedOn;
        //    EvaluationStartedOn = evaluation.EvaluationStartedOn;
        //    EvaluatedOn = evaluation.EvaluatedOn;
        //    AverageRating= evaluation.AverageRating;
        //    SumRating= evaluation.SumRating;
        //    Status = evaluation.Status;
        //    Evaluated = evaluation.Evaluated;
        //    if (full) {
        //        if (evaluation.Committee != null) {
        //            IdCommittee = evaluation.Committee.Id;
        //            CommitteeName = evaluation.Committee.Name;
        //        }
        //        if (evaluation.Evaluator != null)
        //        {
        //            IdEvaluator = evaluation.Evaluator.Id;
        //            EvaluatorName = (evaluation.Evaluator!= null && evaluation.Evaluator.Person !=null)? evaluation.Evaluator.Person.SurnameAndName :"";
        //        }
        //        Comment = evaluation.Comment;
        //        Criteria = new List<dtoCriterionEvaluated>();
        //    }
        //}

        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }
    }


    [Serializable]
    public class dtoCommitteeEvaluationsDisplayItem : dtoBase
    {
        public virtual long IdCommittee { get; set; }
        public virtual String CommitteeName { get; set; }
        public virtual List<dtoCommitteeEvaluatorEvaluationDisplayItem> Evaluations { get; set; }
        public virtual displayAs Display { get; set; }

        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get { return (DssEvaluation != null ? DssEvaluation.Ranking : 0); } }
        public virtual double SumRating { get { return (Evaluations == null || !Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()) ? 0 : Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Select(e => e.SumRating).Sum(); } }
        public virtual double AverageRating { get { return (Evaluations == null || !Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()) ? 0 : Evaluations.Where(e => e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Select(e => e.AverageRating).Average(); } }
        public virtual Boolean Evaluated { get { return (Evaluations != null && !Evaluations.Where(e => !e.Evaluated && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any()); } }
        public virtual EvaluationStatus Status
        {
            get
            {
                if (!Evaluations.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.None;
                else if (Evaluations.Where(e => e.Status != EvaluationStatus.None).Any())
                    return EvaluationStatus.Evaluating;
                else if (!Evaluations.Where(e => !e.Evaluated).Any())
                    return EvaluationStatus.Evaluated;
                else
                    return EvaluationStatus.None;
            }
        }
        public virtual Boolean isEmpty
        {
            get
            {
                return Evaluations == null || !Evaluations.Any();
            }
        }
        public virtual Boolean HasComments
        {
            get
            {
                return (Evaluations.Any() && Evaluations.Where(e => !String.IsNullOrEmpty(e.Comment) || (e.Criteria != null && e.Criteria.Any() && e.Criteria.Where(c => !String.IsNullOrEmpty(c.Comment)).Any())).Any());
            }
        }
        public dtoCommitteeEvaluationsDisplayItem()
        {
            Evaluations = new List<dtoCommitteeEvaluatorEvaluationDisplayItem>();
        }

        public long GetEvaluationsCount(EvaluationStatus status)
        {
            return Evaluations.Where(e => e.Status == status).Count();
        }
        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }
    }

      [Serializable]
    public class dtoCommitteeEvaluatorEvaluationDisplayItem : dtoBase {
        public virtual long  IdCommittee { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual String EvaluatorName { get; set; }
        public virtual displayAs Display { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual DateTime? EvaluationStartedOn { get; set; }
        public virtual DateTime? EvaluatedOn { get; set; }
        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get; set; }
        public virtual double AverageRating { get; set; }
        public virtual double SumRating { get; set; }
        public virtual String Comment { get; set; }
        public virtual Boolean Evaluated { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual List<dtoCriterionEvaluatedDisplayItem> Criteria { get; set; }

        public dtoCommitteeEvaluatorEvaluationDisplayItem()
        {
            Criteria = new List<dtoCriterionEvaluatedDisplayItem>();
        }

        public static dtoCommitteeEvaluatorEvaluationDisplayItem GetForEvaluationsDisplay(expEvaluation evaluation, DssCallEvaluation dssEvaluation,String unknownUser)
        {
            dtoCommitteeEvaluatorEvaluationDisplayItem item = new dtoCommitteeEvaluatorEvaluationDisplayItem();
            item.Id = evaluation.Id;
            item.ModifiedOn = evaluation.LastUpdateOn;
            item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
            item.EvaluatedOn = evaluation.EvaluatedOn;
            item.AverageRating = evaluation.AverageRating;
            item.SumRating = evaluation.SumRating;
            item.Status = evaluation.Status;
            item.Evaluated = evaluation.Evaluated;
            item.IdCommittee = (evaluation.Committee != null) ? evaluation.Committee.Id : 
                (evaluation.AdvCommission != null) ? evaluation.AdvCommission.Id : 0;


            if(evaluation.Evaluator != null)
            {
                item.IdEvaluator = evaluation.Evaluator.Id;
                item.EvaluatorName = (evaluation.Evaluator.Person != null && evaluation.Evaluator.Person.TypeID != (int)UserTypeStandard.Guest) ? evaluation.Evaluator.Person.SurnameAndName : unknownUser;
            } else if (evaluation.AdvEvaluator != null)
            {
                item.IdEvaluator = evaluation.AdvEvaluator.Id;
                item.EvaluatorName = (evaluation.AdvEvaluator.Member != null && evaluation.AdvEvaluator.Member.TypeID != (int)UserTypeStandard.Guest) ? evaluation.AdvEvaluator.Member.SurnameAndName : unknownUser;
            } else
            {
                item.IdEvaluator = 0;
                item.EvaluatorName = unknownUser;
            }
            
            item.Comment = evaluation.Comment;
            item.DssRanking = evaluation.DssRanking;
            item.DssEvaluation = dssEvaluation;
            return item;
        }


        public String DssRankingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }
    }


    [Serializable]
    public class dtoCriterionEvaluatedDisplayItem : dtoCriterionEvaluated
    {
        public virtual long IdCommittee { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual displayAs Display { get; set; }

        public dtoCriterionEvaluatedDisplayItem()
            : base()
        {
            Display =  displayAs.item;
        }
        public dtoCriterionEvaluatedDisplayItem(dtoCriterion criterion, CriterionEvaluated valueItem)
            : base(criterion, valueItem)
        {
        }
    }


    [Serializable]
    public class dtoSubmissionCommitteeItem
    {
        public virtual List<dtoCriterionSummaryItem> Criteria { get; set; }
        public virtual List<dtoEvaluatorDisplayItem> Evaluators { get; set; }
        public virtual long IdCommittee { get; set; }
        public virtual long IdSubmission { get; set; }
        public virtual String Name { get; set; }
        public virtual DssCallEvaluation DssEvaluation { get; set; }
        public virtual double DssRanking { get { return (DssEvaluation != null ? DssEvaluation.Ranking : 0); } }

        public virtual bool IsAdvance { get; set; }
        public virtual double SumRating
        {
            get
            {
                if (Evaluators == null || Evaluators.Count == 0 || !Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return 0;
                else
                    return Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Select(e => e.SumRating).Sum();
            }
        }
        public virtual double AverageRating
        {
            get
            {
                if(IsAdvance)
                {
                    if (Evaluators == null || Evaluators.Count == 0 || !Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                        return 0;
                    else
                        return Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Select(e => e.SumRating).Average();
                } else
                {
                    if (Evaluators == null || Evaluators.Count == 0 || !Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                        return 0;
                    else
                        return Evaluators.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Select(e => e.AverageRating).Average();
                }
                
            }
        }
        
        public virtual EvaluationStatus Status
        {
            get
            {
                if (!Evaluators.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.None;
                else if (Evaluators.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any())
                    return EvaluationStatus.Evaluating;
                else if (!Evaluators.Where(e => !e.Evaluated && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).Any())
                    return EvaluationStatus.Evaluated;
                else
                    return EvaluationStatus.None;
            }
        }


        public dtoSubmissionCommitteeItem()
        {
            Evaluators = new List<dtoEvaluatorDisplayItem>();
            Criteria = new List<dtoCriterionSummaryItem>();
        }
        public dtoSubmissionCommitteeItem(long idCommittee,String name, long idSubmission, DssCallEvaluation dssEvaluation)
        {
            Evaluators = new List<dtoEvaluatorDisplayItem>();
            Criteria = new List<dtoCriterionSummaryItem>();
            IdCommittee = idCommittee;
            Name = name;
            IdSubmission = idSubmission;
            DssEvaluation = dssEvaluation;
        }
        public String DssRatingToString(int decimals = 2)
        {
            return GetDoubleToString(DssRanking, decimals);
        }
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }

        public Boolean HasComments()
        {
            return Evaluators.Where(e => !String.IsNullOrEmpty(e.Comment) || e.Values.Where(v => !String.IsNullOrEmpty(v.Comment)).Any()).Any() || Criteria.Where(c => c.Evaluations.Where(e => !String.IsNullOrEmpty(e.Criterion.Comment)).Any()).Any();
        }
       
    }
}