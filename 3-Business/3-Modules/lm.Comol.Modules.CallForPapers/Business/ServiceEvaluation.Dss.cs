using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Domain.Templates;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LM.MathLibrary.Extensions;
using System.Linq.Expressions;
using lm.Comol.Modules.CallForPapers.Domain;
namespace lm.Comol.Modules.CallForPapers.Business
{
    public partial class ServiceEvaluation
    {
        private lm.Comol.Core.Dss.Business.ServiceDss _ServiceDss;
        private lm.Comol.Core.Dss.Business.ServiceDss ServiceDss
        {
            get
            {
                return _ServiceDss;
            }
        }

        #region Get Committee Dss Methods
            public Dictionary<long, Boolean > GetCommitteeDssMethodIsFuzzy(long idCall)
            {
                try
                {
                    return (from c in Manager.GetIQ<EvaluationCommittee>() 
                            where c.Deleted== Core.DomainModel.BaseStatusDeleted.None && c.Call !=null && c.Call.Id ==idCall && c.UseDss
                                select c).ToList().ToDictionary(c=> c.Id, c=> c.MethodSettings.IsFuzzyMethod);
                }
                catch{
                    return new Dictionary<long, bool>();
                }
            }
        #endregion

        #region DssRating get methods
            /// <summary>
            /// Get DSS evaluation for single Evaluator-submission
            /// </summary>
            /// <param name="idEvaluation"></param>
            /// <returns></returns>
            public DssCallEvaluation DssRatingGetByEvaluation(long idEvaluation)
            {
                return GetQueryDssCallEvaluation(e => e.IdEvaluation == idEvaluation && e.Type== DssEvaluationType.Evaluator && e.Deleted == Core.DomainModel.BaseStatusDeleted.None).Skip(0).Take(1).ToList().FirstOrDefault();
            }


            public List<DssCallEvaluation> DssRatingGetValues(long idCall, long idCommittee, long idEvaluator)
            {
                IQueryable<DssCallEvaluation> query = GetQueryDssCallEvaluation(e => e.IdCall == idCall
                    && e.IdCommittee == idCommittee && e.IdEvaluator == idEvaluator && e.Type == DssEvaluationType.Evaluator
                    && e.Deleted == Core.DomainModel.BaseStatusDeleted.None);

                return query.ToList();
            }

            /// <summary>
            /// Get dss evaluation at submission level - for each evaluator
            /// </summary>
            /// <param name="idCall"></param>
            /// <param name="idCommittees"></param>
            /// <returns></returns>
            public List<DssCallEvaluation> DssRatingGetEvaluationValues(long idCall, List<long> idCommittees)
            {
                IQueryable<DssCallEvaluation> query = GetQueryDssCallEvaluation(e => e.IdCall == idCall
                    && idCommittees.Contains(e.IdCommittee) && e.Type == DssEvaluationType.Evaluator
                    && e.Deleted == Core.DomainModel.BaseStatusDeleted.None);

                return query.ToList();
            }    

            /// <summary>
            /// Get dss evaluation at Call level
            /// </summary>
            /// <param name="idCall"></param>
            /// <returns></returns>
            public List<DssCallEvaluation> DssRatingGetValues(long idCall)
            {
                return GetQueryDssCallEvaluation(e => e.IdCall == idCall && e.Type == DssEvaluationType.Call && e.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
            }

            /// <summary>
            /// Get all DSS evaluations of a call from call level to specified level !
            /// </summary>
            /// <param name="idCall"></param>
            /// <param name="toLevel"></param>
            /// <returns></returns>
            public List<DssCallEvaluation> DssRatingGetValues(long idCall, DssEvaluationType toLevel)
            {
                IQueryable<DssCallEvaluation> query = GetQueryDssCallEvaluation(e => e.IdCall == idCall && e.Deleted == Core.DomainModel.BaseStatusDeleted.None);
                switch (toLevel)
                {
                    case DssEvaluationType.Call:
                        query = query.Where(e => e.Type == DssEvaluationType.Call);
                        break;
                    case DssEvaluationType.Committee:
                        query = query.Where(e => e.Type == DssEvaluationType.Call || e.Type == DssEvaluationType.Committee );
                        break;
                }
                return query.ToList();
            }

            /// <summary>
            /// Get dss evaluation at Committee level
            /// </summary>
            /// <param name="idCall"></param>
            /// <param name="idCommittee"></param>
            /// <returns></returns>
            public List<DssCallEvaluation> DssRatingGetValues(long idCall, long idCommittee)
            {
                IQueryable<DssCallEvaluation> query = GetQueryDssCallEvaluation(e => e.IdCall == idCall
                    && e.IdCommittee == idCommittee && e.Type== DssEvaluationType.Committee 
                    && e.Deleted == Core.DomainModel.BaseStatusDeleted.None);

                return query.ToList();
            }

            /// <summary>
            /// Get dss evaluation at Committee level
            /// </summary>
            /// <param name="idCall"></param>
            /// <param name="idCommittees"></param>
            /// <returns></returns>
            public List<DssCallEvaluation> DssRatingGetValues(long idCall, List<long> idCommittees)
            {
                IQueryable<DssCallEvaluation> query = GetQueryDssCallEvaluation(e => e.IdCall == idCall
                    && idCommittees.Contains(e.IdCommittee) && e.Type == DssEvaluationType.Committee 
                    && e.Deleted == Core.DomainModel.BaseStatusDeleted.None);

                return query.ToList();
            }    

            /// <summary>
            /// Tell if Dss evaluation of a call MUST be updated or no
            /// </summary>
            /// <param name="idCall"></param>
            /// <param name="referenceTime"></param>
            /// <returns></returns>
            public Boolean DssRatingMustUpdate(long idCall, DateTime referenceTime)
            {
                List<DateTime?> dates = (from v in Manager.GetIQ<CriterionEvaluated>()
                                        where v.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                            && v.Call != null && v.Call.Id == idCall
                                        select v.ModifiedOn).ToList();
                return dates.Any(d => d != null && d.Value > referenceTime);
            }

            /// <summary>
            /// Generic DSS evaluation query
            /// </summary>
            /// <param name="filters"></param>
            /// <returns></returns>
            private IQueryable<DssCallEvaluation> GetQueryDssCallEvaluation(Expression<Func<DssCallEvaluation, bool>> filters)
            {
                return (from q in Manager.GetIQ<DssCallEvaluation>() select q).Where(filters);
            }
        #endregion
        
        #region "Evaluator rating"
            /// <summary>
            /// Calculate rating for all submissions of a single evaluator of a single committee
            /// </summary>
            /// <param name="idEvaluator"></param>
            /// <param name="committee"></param>
            /// <param name="idEvaluation"></param>
            /// <param name="updatedOn">update time to save</param>
            private void EvaluatorSetDssRating(long idEvaluator,  EvaluationCommittee committee, long idEvaluation,DateTime updatedOn)
            {
                try
                {
                    List<Evaluation> evaluations = (from e in Manager.GetIQ<Evaluation>()
                                                    where e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                    && e.Committee != null && e.Committee.Id == committee.Id
                                                    && e.Evaluator != null && e.Evaluator.Id == idEvaluator && e.Status != EvaluationStatus.Invalidated 
                                                    select e).ToList();

                    List<DssCallEvaluation> dssEvaluations = (from e in Manager.GetIQ<DssCallEvaluation>()
                                                              where e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                              && e.Type== DssEvaluationType.Evaluator 
                                                              && e.IdEvaluator==idEvaluator && e.IdCommittee== committee.Id 
                                                              select e).ToList();
                    long idMethod = committee.MethodSettings.IdMethod;
                    long idCall = (committee.Call != null ? committee.Call.Id : 0);
                    TemplateMethod method = ServiceDss.MethodGet(idMethod);
                    if (method != null)
                    {
                        List<BaseCriterion> criteria = committee.Criteria.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                        Boolean isValid = false;
                        Boolean isGroupCompleted = method.DisplaySingleGroupRating && !evaluations.Any(e=> e.Status== EvaluationStatus.Evaluating || e.Status== EvaluationStatus.None);
                        if (method.IsFuzzy)
                        {
                            dtoAlgorithmInputFuzzy aInput = CommitteeGetAlgorithmInputFuzzy(method,committee.WeightSettings.FuzzyMeWeights, criteria, evaluations);
                            List<dtoAlgorithmAlternativeFuzzy> results = ServiceDss.Calculate(aInput);
                            isValid = method.DisplayPartialRating || (!aInput.HasEmptyValues() && !evaluations.Any(e => e.Status != EvaluationStatus.Evaluated));
                            foreach (Evaluation e in evaluations)
                            {
                                dtoAlgorithmAlternativeFuzzy value = results.Where(r => r.Id == e.Id).FirstOrDefault();
                                if (value!=null){
                                    e.DssRanking = GetValidDouble(value.FinalValue);
                                    e.DssIsFuzzy = value.IsFuzzyValue;
                                    if (IsValidDouble(value.FinalValue))
                                    {
                                        e.DssValue = value.FinalValue;
                                        e.DssValueFuzzy = value.FinalValueFuzzy;
                                    }
                                    else
                                    {
                                        e.DssValue = 0;
                                        e.DssValueFuzzy = ((double)0).ToFuzzy().ToString();
                                    }
                                }
                                else
                                {
                                    e.DssRanking = 0;
                                    e.DssIsFuzzy = true;
                                    e.DssValue = 0;
                                    e.DssValueFuzzy = ((double)0).ToFuzzy().ToString();
                                }
                                DssEvaluationSet(idEvaluator, committee.Id, idCall, (e.Submission == null ? 0 : e.Submission.Id), e.Id, dssEvaluations, updatedOn, isGroupCompleted || (method.DisplaySingleRating && (e.Status == EvaluationStatus.Evaluated)), isValid, true,e.DssRanking, e.DssValue, e.DssValueFuzzy);
                            }
                        }
                        else
                        {
                            dtoAlgorithmInput aInput = CommitteeGetAlgorithmInput(method, committee.WeightSettings.FuzzyMeWeights,criteria, evaluations);
                            isValid = method.DisplayPartialRating || (!aInput.HasEmptyValues() && !evaluations.Any(e => e.Status != EvaluationStatus.Evaluated));
                            List<dtoAlgorithmAlternative> results = ServiceDss.Calculate(aInput);
                            foreach (Evaluation e in evaluations)
                            {
                                dtoAlgorithmAlternative value = results.Where(r => r.Id == e.Id).FirstOrDefault();
                                if (value != null)
                                {
                                    e.DssRanking = GetValidDouble(value.Ranking);
                                    e.DssIsFuzzy = value.IsFuzzyValue;
                                    if (IsValidDouble(value.FinalValue))
                                    {
                                        e.DssValue = value.FinalValue;
                                        e.DssValueFuzzy = value.FinalValueFuzzy;
                                    }
                                    else
                                    {
                                        e.DssValue = 0;
                                        e.DssValueFuzzy = ((double)0).ToFuzzy().ToString();
                                    }
                                }
                                else
                                {
                                    e.DssRanking = 0;
                                    e.DssIsFuzzy = true;
                                    e.DssValue = 0;
                                    e.DssValueFuzzy = ((double)0).ToFuzzy().ToString();
                                }
                                DssEvaluationSet(idEvaluator, committee.Id, idCall, (e.Submission == null ? 0 : e.Submission.Id), e.Id, dssEvaluations, updatedOn, isGroupCompleted || (method.DisplaySingleRating && (e.Status == EvaluationStatus.Evaluated)), isValid, false, e.DssRanking, e.DssValue, e.DssValueFuzzy);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            private double GetValidDouble(double value, double defaultValue = 0)
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                    return defaultValue;
                return value;
            }
            private Boolean IsValidDouble(double value)
            {
                return !(double.IsNaN(value) || double.IsInfinity(value));
            }
            private dtoAlgorithmInputFuzzy CommitteeGetAlgorithmInputFuzzy(TemplateMethod method,String fuzzyMeWeights, List<BaseCriterion> criteria, List<Evaluation> evaluations)
            {
                dtoAlgorithmInputFuzzy aInput = new dtoAlgorithmInputFuzzy(method.Type);
                if (method.UseManualWeights)
                {
                    List<String> values = (String.IsNullOrWhiteSpace(fuzzyMeWeights) ? new List<String>() : fuzzyMeWeights.Split('#').ToList().Select(v=> v.Split(':')[1]).ToList());
                    aInput.Weights = values.Select(v => LM.MathLibrary.Algorithms.TriangularFuzzyNumber.Parse(v)).ToList();
                }
                else
                    aInput.Weights = criteria.Select(v => LM.MathLibrary.Algorithms.TriangularFuzzyNumber.Parse(v.WeightSettings.WeightFuzzy)).ToList();

                foreach (Evaluation e in evaluations)
                {
                    dtoAlgorithmAlternativeFuzzy alternative = new dtoAlgorithmAlternativeFuzzy() { Id = e.Id, IsFuzzyValue=true };
                    List<CriterionEvaluated> values = (from v in Manager.GetIQ<CriterionEvaluated>()
                                                       where v.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                       && v.Evaluation != null && v.Evaluation.Id == e.Id
                                                       select v).ToList();
                    if (values.Any())
                    {
                        foreach (BaseCriterion c in criteria)
                        {
                            CriterionEvaluated evaluated = values.Where(v => v.Criterion == c).FirstOrDefault();
                            if (evaluated != null && evaluated.Deleted == Core.DomainModel.BaseStatusDeleted.None)
                            {
                                if (evaluated.IsValueEmpty)
                                    alternative.HasEmptyValues = true;
                                switch (c.Type)
                                {
                                    case CriterionType.RatingScaleFuzzy:
                                    case CriterionType.RatingScale:
                                        if (evaluated.DssValue != null){
                                            if (!String.IsNullOrWhiteSpace(evaluated.DssValue.ValueFuzzy))
                                                alternative.Values.Add(evaluated.DssValue.ValueFuzzy.ToFuzzy());
                                            else
                                                alternative.Values.Add(evaluated.DssValue.Value.ToFuzzy());
                                        }
                                        else
                                        {
                                            alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0));
                                            alternative.HasEmptyValues = true;
                                        }
                                        break;
                                    case CriterionType.DecimalRange:
                                    case CriterionType.IntegerRange:
                                    case CriterionType.StringRange:
                                        alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(((double)evaluated.DecimalValue)));
                                        break;
                                }
                            }
                            else
                            {
                                alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0));
                                alternative.HasEmptyValues = true;
                            }
                        }
                    }
                    else { 
                        criteria.ForEach(c => alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0)));
                        alternative.HasEmptyValues = true;
                    }
                    aInput.Alternatives.Add(alternative);
                }
                return aInput;
            }
            private dtoAlgorithmInput CommitteeGetAlgorithmInput(TemplateMethod method, String fuzzyMeWeights, List<BaseCriterion> criteria, List<Evaluation> evaluations)
            {
                dtoAlgorithmInput aInput = new dtoAlgorithmInput(method.Type);
                if (method.UseManualWeights)
                {
                    List<String> values = (String.IsNullOrWhiteSpace(fuzzyMeWeights) ? new List<String>() : fuzzyMeWeights.Split('#').ToList().Select(v => v.Split(':')[1]).ToList());
                    aInput.Weights = values.Select(v => Double.Parse(v)).ToList();
                }
                else
                    aInput.Weights = criteria.Select(v => v.WeightSettings.Weight).ToList();

                foreach (Evaluation e in evaluations)
                {
                    dtoAlgorithmAlternative alternative = new dtoAlgorithmAlternative() { Id = e.Id };
                    List<CriterionEvaluated> values = (from v in Manager.GetIQ<CriterionEvaluated>()
                                                       where v.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                       && v.Evaluation != null && v.Evaluation.Id == e.Id
                                                       select v).ToList();

                    if (values.Any())
                    {
                        foreach (BaseCriterion c in criteria)
                        {
                            CriterionEvaluated evaluated = values.Where(v => v.Criterion == c).FirstOrDefault();
                            if (evaluated != null && evaluated.Deleted == Core.DomainModel.BaseStatusDeleted.None)
                            {
                                if (evaluated.IsValueEmpty)
                                    alternative.HasEmptyValues = true;
                                switch (c.Type)
                                {
                                    case CriterionType.RatingScaleFuzzy:
                                    case CriterionType.RatingScale:
                                        if (evaluated.DssValue != null && evaluated.DssValue.IsValid())
                                            alternative.Values.Add(evaluated.DssValue.Value);
                                        else
                                        {
                                            alternative.Values.Add(0);
                                            alternative.HasEmptyValues = true;
                                        }
                                        break;
                                    case CriterionType.DecimalRange:
                                    case CriterionType.IntegerRange:
                                    case CriterionType.StringRange:
                                        alternative.Values.Add(((double)evaluated.DecimalValue));
                                        break;
                                }
                            }
                            else
                            {
                                alternative.Values.Add(0);
                                alternative.HasEmptyValues = true;
                            }
                        }
                    }
                    else { 
                        criteria.ForEach(c => alternative.Values.Add(0));
                        alternative.HasEmptyValues = true;
                    }
                    aInput.Alternatives.Add(alternative);
                }
                return aInput;
            }
            private void DssEvaluationSet(long idEvaluator, long idCommittee, long idCall, long idSubmission, long idEvaluation, List<DssCallEvaluation> dssEvaluations, DateTime updatedOn, Boolean isCompleted, Boolean isValid, Boolean isFuzzy, Double ranking, Double value, String valueFuzzy)
            {
                DssCallEvaluation dEvaluation = dssEvaluations.Where(d => d.IdEvaluation == idEvaluation && d.IdSubmission == idSubmission && d.IdEvaluator == idEvaluator && d.IdCommittee == idCommittee).FirstOrDefault();
                 if (dEvaluation == null)
                 {
                     dEvaluation = DssCallEvaluation.CreateForEvaluator(idEvaluator, idCommittee, idCall, idSubmission,idEvaluation, isCompleted, isValid,ranking, value, valueFuzzy);
                     dEvaluation.LastUpdateOn = updatedOn;
                     dEvaluation.IsFuzzy = isFuzzy;
                     Manager.SaveOrUpdate(dEvaluation);
                     dssEvaluations.Add(dEvaluation);
                 }
                 else
                 {
                     dEvaluation.IsCompleted = isCompleted;
                     dEvaluation.IsValid = isValid;
                     dEvaluation.IsFuzzy = isFuzzy;
                     dEvaluation.Ranking = ranking;
                     dEvaluation.Value = value;
                     dEvaluation.ValueFuzzy = valueFuzzy;
                     dEvaluation.LastUpdateOn = updatedOn;
                     Manager.SaveOrUpdate(dEvaluation);
                 }
            }
        #endregion
        public Boolean  DssRatingSetForCall(long idCall,out DateTime updatedOn)
        {
            Boolean result = false;
            updatedOn = DateTime.MinValue;
            try
            {
                CallForPapers.Domain.CallForPaper call = Manager.Get<CallForPapers.Domain.CallForPaper>(idCall);
                if (call != null)
                {

                    // recupero le attuali votazioni
                    List<DssCallEvaluation> dssEvaluations = (from e in Manager.GetIQ<DssCallEvaluation>()
                                                              where e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                              && (e.Type == DssEvaluationType.Committee || e.Type == DssEvaluationType.Call)
                                                              && e.IdCall==idCall 
                                                              select e).ToList();
                    // recupero le commissioni
                    List<EvaluationCommittee> committees = (from c in Manager.GetIQ<EvaluationCommittee>()
                                                           where c.Deleted == Core.DomainModel.BaseStatusDeleted.None && c.Call != null && c.Call.Id == idCall
                                                           select c).ToList();
                    // recupero le valutazioni
                    List<Evaluation> evaluations = (from e in Manager.GetIQ<Evaluation>()
                                                    where e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                    && e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement 
                                                    && e.Call != null && e.Call.Id == idCall && e.Submission !=null 
                                                    select e).ToList();
                    List<long> idSubmissions = evaluations.Select(e => e.Submission.Id).Distinct().ToList();
                    // recupero gli eventuali voti dati ai criteri
                    List<CriterionEvaluated> values = (from v in Manager.GetIQ<CriterionEvaluated>()
                                                       where v.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                       && v.Evaluation != null && v.Call != null && v.Call.Id == idCall
                                                       select v).ToList();
                    /// Aggregazione a livello di bando
                    TemplateMethod method = ServiceDss.MethodGet(call.IdDssMethod);
                    if (method != null)
                    {
                        updatedOn = DateTime.Now;
                        List<DssRankingGroup> groups = (from g in Manager.GetIQ<DssRankingGroup>() where g.Deleted == Core.DomainModel.BaseStatusDeleted.None && g.IdCall== idCall select g).ToList();
                        if (groups.Any())
                        {
                            // per eventuali sviluppi futuri 

                        }
                        else
                        {
                            Dictionary<long, List<long>> submissions = null;
                            if (committees.Any(c => !c.ForAllSubmittersType))
                            {
                                submissions = (from s in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.UserSubmission>()
                                               where s.Deleted == Core.DomainModel.BaseStatusDeleted.None && s.Call != null && s.Call.Id == idCall
                                               select s).ToList().Where(s => idSubmissions.Contains(s.Id)).GroupBy(s => s.Type.Id).ToDictionary(s => s.Key, s => s.Select(x => x.Id).ToList());
                            }
                            else
                                submissions = new Dictionary<long, List<long>>() { {(long)-1, idSubmissions} };

                            List<TemplateMethod> methods = ServiceDss.MethodsGetAll();
                            foreach (var item in submissions)
                            {
                                long kk = item.Key;
                                if (kk == 258)
                                    kk = 258;
                                IEnumerable<EvaluationCommittee> commiteesForType = committees.Where(c=> c.ForAllSubmittersType || c.AssignedTypes.Any(t=> t.SubmitterType != null && t.SubmitterType.Id == item.Key));
                                Boolean allowPartialRanking = method.DisplayPartialRating && !methods.Any(m => !m.DisplayPartialRating && commiteesForType.Any(s => s.MethodSettings.IdMethod == m.Id));
                                if (commiteesForType.Count() == 1)
                                    allowPartialRanking = methods.Any(m => !m.DisplayPartialRating && m.Id == commiteesForType.FirstOrDefault().MethodSettings.IdMethod);
                                
                                Dictionary<long, List<dtoDssEvaluationResultItem>> cResults = new Dictionary<long, List<dtoDssEvaluationResultItem>>();
                                /// aggrego i dati di ciascuna commissione
                                foreach (EvaluationCommittee committee in commiteesForType)
                                {
                                    /// prendo tutte le valuazioni ed i valutarori
                                    /// data una valutazione per ciascun criterio faccio la media dei voti dei valutatori 
                                    /// committees.Average()
                                    /// 
                                    //List<long> idCommitteeSubmissions = evaluations.Where(e => e.Committee != null && e.Committee.Id == committee.Id).Select(e => e.Submission.Id).Distinct().ToList();
                                    int evaluatorsCount = evaluations.Where(e => e.Committee != null && e.Committee.Id == committee.Id && e.Evaluator != null).Select(e => e.Evaluator.Id).Distinct().Count();
                                    //cResults.Add(committee.Id, CommitteeSetDssRating(idCall, committee, evaluatorsCount, (sameNumberForAllCommissions ? idSubmissions : idCommitteeSubmissions), dssEvaluations, evaluations.Where(e => e.Committee != null && e.Committee.Id == committee.Id), values, updatedOn));
                                    cResults.Add(committee.Id, CommitteeSetDssRating(idCall, allowPartialRanking, committee, evaluatorsCount, item.Value, dssEvaluations, evaluations.Where(e => e.Committee != null && e.Committee.Id == committee.Id && item.Value.Contains(e.Submission.Id)), values, updatedOn));
                                }
                                if (commiteesForType.Count() == 1)
                                    DssRatingSetForCall(idCall, commiteesForType.FirstOrDefault().MethodSettings.IsFuzzyMethod, item.Value, dssEvaluations, updatedOn);
                                else
                                    DssRatingSetForCall(call, method, commiteesForType.ToList(), item.Value, dssEvaluations, cResults, updatedOn);
                                if (!dssEvaluations.Any(e => item.Value.Contains(e.IdSubmission) && !(e.IsCompleted && e.IsValid)))
                                {
                                    foreach (DssCallEvaluation e in GetQueryDssCallEvaluation(d => d.Type== DssEvaluationType.Evaluator && item.Value.Contains(d.IdSubmission) && (!d.IsValid || !d.IsCompleted)))
                                    {
                                        e.IsValid = true;
                                        e.IsCompleted = true;
                                        Manager.SaveOrUpdate(e);
                                    }
                                }
                            }
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        #region "Committee level"

            /// <summary>
            /// Get committee evaluations 
            /// </summary>
            /// <param name="idCall"></param>
            /// <param name="committee"></param>
            /// <param name="evaluatorsCount">number of evaluators for avarege</param>
            /// <param name="idSubmissions">list of submissions</param>
            /// <param name="dssEvaluations">list of dss evaluation records for this committee</param>
            /// <param name="evaluations">list of evaluations of this committee</param>
            /// <param name="values">list of evaluated values</param>
            /// <param name="updatedOn">update date for recalc usage</param>
            /// <returns></returns>
            private List<dtoDssEvaluationResultItem> CommitteeSetDssRating(long idCall,  Boolean allowPartialRanking,EvaluationCommittee committee, int evaluatorsCount, List<long> idSubmissions, List<DssCallEvaluation> dssEvaluations, IEnumerable<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values, DateTime updatedOn)
            {
                List<dtoDssEvaluationResultItem> result = new List<dtoDssEvaluationResultItem>();
                try
                {
                    // trovo come aggregare i voti sui criteri
                    long idMethod = committee.MethodSettings.IdMethod;
                    TemplateMethod method = ServiceDss.MethodGet(idMethod);
                    if (method != null)
                    {
                        List<BaseCriterion> criteria = committee.Criteria.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                        Boolean isValid = false;
                        Boolean isCompleted =  !evaluations.Any(e => e.Status != EvaluationStatus.Evaluated);
                        Boolean isFuzzy = false;
                        double value = 0;
                        String valueFuzzy = "";
                        double ranking = 0;
                        if (method.IsFuzzy){
                            dtoAlgorithmInputFuzzy aInput = CommitteeGetAlgorithmInputFuzzy(method,committee.WeightSettings.FuzzyMeWeights, criteria, evaluatorsCount, idSubmissions, evaluations, values);
                            List<dtoAlgorithmAlternativeFuzzy> rValues = ServiceDss.Calculate(aInput);

                            isValid = (allowPartialRanking && method.DisplayPartialRating) || (!aInput.HasEmptyValues() && !evaluations.Any(e => e.Status != EvaluationStatus.Evaluated));
                            foreach (long idSubmission in idSubmissions)
                            {
                                dtoAlgorithmAlternativeFuzzy fValue = rValues.Where(r => r.Id == idSubmission).FirstOrDefault();
                                if (fValue != null)
                                {
                                    ranking = fValue.Ranking;
                                    isFuzzy = fValue.IsFuzzyValue;
                                    value = fValue.FinalValue;
                                    valueFuzzy = fValue.FinalValueFuzzy;
                                }
                                else
                                {
                                    isFuzzy = true;
                                    ranking = 0;
                                    value = 0;
                                    valueFuzzy = ((double)0).ToFuzzy().ToString();
                                }
                                DssRatingAddEvaluation(result, idCall, committee.Id, idSubmission, dssEvaluations, updatedOn, isCompleted, isValid, rValues.Any(r => r.Id == idSubmission && r.HasEmptyValues), ranking,value, valueFuzzy, isFuzzy);
                            }
                        }
                        else{
                            dtoAlgorithmInput aInput = CommitteeGetAlgorithmInput(method, committee.WeightSettings.FuzzyMeWeights, criteria, evaluatorsCount, idSubmissions, evaluations, values);
                            List<dtoAlgorithmAlternative> rValues = ServiceDss.Calculate(aInput);
                            isValid = (allowPartialRanking && method.DisplayPartialRating) || (!aInput.HasEmptyValues() && !evaluations.Any(e => e.Status != EvaluationStatus.Evaluated));
                            foreach (long idSubmission in idSubmissions)
                            {
                                dtoAlgorithmAlternative fValue = rValues.Where(r => r.Id == idSubmission).FirstOrDefault();
                                if (fValue != null)
                                {
                                    ranking = fValue.Ranking;
                                    isFuzzy = fValue.IsFuzzyValue;
                                    value = fValue.FinalValue;
                                    valueFuzzy = fValue.FinalValueFuzzy;
                                }
                                else
                                {
                                    isFuzzy = false;
                                    ranking = 0;
                                    value = 0;
                                    valueFuzzy = ((double)0).ToFuzzy().ToString();
                                }
                                DssRatingAddEvaluation(result, idCall, committee.Id, idSubmission, dssEvaluations, updatedOn, isCompleted, isValid, rValues.Any(r => r.Id == idSubmission && r.HasEmptyValues), ranking, value, valueFuzzy, isFuzzy);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            private void DssRatingAddEvaluation(List<dtoDssEvaluationResultItem> itemsToAdd, long idCall, long idCommittee, long idSubmission, List<DssCallEvaluation> dssEvaluations, DateTime updatedOn, Boolean isCompleted, Boolean isValid, Boolean isEmpty,Double ranking, Double value, String valueFuzzy, Boolean isFuzzy)
            {
                DssCommitteeEvaluationSet(idCommittee, idCall, idSubmission, dssEvaluations, updatedOn, isCompleted, isValid, isFuzzy, ranking, value, valueFuzzy);
                itemsToAdd.Add(new dtoDssEvaluationResultItem()
                {
                    IdObject = idCommittee,
                    IdSubmission = idSubmission,
                    IsCompleted = isCompleted,
                    IsValid = isValid,
                    Type = DssEvaluationType.Committee,
                    Ranking = ranking,
                    Value = value,
                    ValueFuzzy = valueFuzzy,
                    IsFuzzyValue= isFuzzy,
                    HasEmptyValues = isEmpty
                });
            }
            /// <summary>
            /// Get algorithm input for committee - fuzzy values
            /// </summary>
            /// <param name="method"></param>
            /// <param name="criteria"></param>
            /// <param name="evaluatorsCount"></param>
            /// <param name="idSubmissions"></param>
            /// <param name="evaluations"></param>
            /// <param name="values"></param>
            /// <returns></returns>
            private dtoAlgorithmInputFuzzy CommitteeGetAlgorithmInputFuzzy(TemplateMethod method, String fuzzyMeWeights, List<BaseCriterion> criteria, int evaluatorsCount, List<long> idSubmissions, IEnumerable<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values)
            {
                dtoAlgorithmInputFuzzy aInput = new dtoAlgorithmInputFuzzy(method.Type);
                if (method.UseManualWeights)
                {
                    List<String> wValues = (String.IsNullOrWhiteSpace(fuzzyMeWeights) ? new List<String>() : fuzzyMeWeights.Split('#').ToList().Select(v => v.Split(':')[1]).ToList());
                    aInput.Weights = wValues.Select(v => LM.MathLibrary.Algorithms.TriangularFuzzyNumber.Parse(v)).ToList();
                }
                else
                    aInput.Weights = criteria.Select(v => LM.MathLibrary.Algorithms.TriangularFuzzyNumber.Parse(v.WeightSettings.WeightFuzzy)).ToList();

                foreach (long idSubmission in idSubmissions)
                {
                    dtoAlgorithmAlternativeFuzzy alternative = new dtoAlgorithmAlternativeFuzzy() { Id = idSubmission, IsFuzzyValue = true };
                    if (values.Any())
                    {
                        foreach (BaseCriterion c in criteria)
                        {
                            List<CriterionEvaluated> vItems = values.Where(v => v.Submission != null && v.Submission.Id == idSubmission && v.Criterion == c && v.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                            if (vItems != null && vItems.Any())
                            {
                                List<LM.MathLibrary.Algorithms.TriangularFuzzyNumber> averageValues = new List<LM.MathLibrary.Algorithms.TriangularFuzzyNumber>();
                                alternative.HasEmptyValues = vItems.Any(v => v.IsValueEmpty);
                                switch (c.Type)
                                {
                                    case CriterionType.RatingScaleFuzzy:
                                    case CriterionType.RatingScale:
                                        averageValues.AddRange(vItems.Where(v=>v.DssValue!=null && v.DssValue.IsValid() && !String.IsNullOrWhiteSpace(v.DssValue.ValueFuzzy)).Select(v => LM.MathLibrary.Algorithms.TriangularFuzzyNumber.Parse(v.DssValue.ValueFuzzy)).ToList());
                                        averageValues.AddRange(vItems.Where(v => v.DssValue == null || !v.DssValue.IsValid() || String.IsNullOrWhiteSpace(v.DssValue.ValueFuzzy)).Select(v => new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0)).ToList());
                                        if (vItems.Any(v=>v.DssValue==null || !v.DssValue.IsValid()))
                                           alternative.HasEmptyValues = true;
                                        break;
                                    case CriterionType.DecimalRange:
                                    case CriterionType.IntegerRange:
                                    case CriterionType.StringRange:
                                        averageValues.AddRange(vItems.Select(v =>new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(((double)v.DecimalValue))).ToList());
                                        break;
                                }
                                if (!alternative.HasEmptyValues)
                                    alternative.HasEmptyValues = (averageValues.Count < evaluatorsCount);
                                while (averageValues.Count < evaluatorsCount)
                                {
                                    averageValues.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0));
                                }
                                alternative.Values.Add(averageValues.Average());
                            }
                            else
                            {
                                alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0));
                                alternative.HasEmptyValues = true;
                            }
                        }
                    }
                    else
                        criteria.ForEach(c => alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0)));
                    aInput.Alternatives.Add(alternative);
                }
                return aInput;
            }
            /// <summary>
            /// Get algorithm input for committee - NO fuzzy values
            /// </summary>
            /// <param name="method"></param>
            /// <param name="criteria"></param>
            /// <param name="evaluatorsCount"></param>
            /// <param name="idSubmissions"></param>
            /// <param name="evaluations"></param>
            /// <param name="values"></param>
            /// <returns></returns>
            private dtoAlgorithmInput CommitteeGetAlgorithmInput(TemplateMethod method, String fuzzyMeWeights, List<BaseCriterion> criteria, int evaluatorsCount, List<long> idSubmissions, IEnumerable<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values)
            {
                dtoAlgorithmInput aInput = new dtoAlgorithmInput(method.Type);
                if (method.UseManualWeights)
                {
                    List<String> wValues = (String.IsNullOrWhiteSpace(fuzzyMeWeights) ? new List<String>() : fuzzyMeWeights.Split('#').ToList().Select(v => v.Split(':')[1]).ToList());
                    aInput.Weights = wValues.Select(v => Double.Parse(v)).ToList();
                }
                else
                    aInput.Weights = criteria.Select(v => v.WeightSettings.Weight).ToList();

                foreach (long idSubmission in idSubmissions)
                {
                    dtoAlgorithmAlternative alternative = new dtoAlgorithmAlternative() { Id = idSubmission };
                    //int count = evaluations.Where(e => e.Submission != null && e.Submission.Id == idSubmission).Count();
                    if (values.Any())
                    {
                        foreach (BaseCriterion c in criteria)
                        {
                            List<CriterionEvaluated> vItems = values.Where(v => v.Submission!=null && v.Submission.Id==idSubmission && v.Criterion == c && v.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                            if (vItems != null && vItems.Any())
                            {
                                List<double> averageValues = new List<double>();
                                alternative.HasEmptyValues = vItems.Any(v=> v.IsValueEmpty);
                                switch (c.Type)
                                {
                                    case CriterionType.RatingScaleFuzzy:
                                    case CriterionType.RatingScale:
                                        averageValues.AddRange(vItems.Where(v=>v.DssValue!=null && v.DssValue.IsValid()).Select(v => ((double)v.DssValue.Value)).ToList());
                                        averageValues.AddRange(vItems.Where(v=>v.DssValue==null || !v.DssValue.IsValid()).Select(v => ((double)0)).ToList());
                                        if (vItems.Any(v=>v.DssValue==null || !v.DssValue.IsValid()))
                                           alternative.HasEmptyValues = true;
                                        break;
                                    case CriterionType.DecimalRange:
                                    case CriterionType.IntegerRange:
                                    case CriterionType.StringRange:
                                        averageValues.AddRange(vItems.Select(v=> ((double)v.DecimalValue)).ToList());
                                        break;
                                }
                                if (!alternative.HasEmptyValues)
                                    alternative.HasEmptyValues = (averageValues.Count < evaluatorsCount);
                                while (averageValues.Count < evaluatorsCount)
                                {
                                    averageValues.Add(0);
                                }
                                alternative.Values.Add(averageValues.Average());
                            }
                            else
                            {
                                alternative.Values.Add(0);
                                alternative.HasEmptyValues = true;
                            }
                        }
                    }
                    else
                    {
                        criteria.ForEach(c => alternative.Values.Add(0));
                        alternative.HasEmptyValues = true;
                    }
                    aInput.Alternatives.Add(alternative);
                }
                return aInput;
            }
            /// <summary>
            /// Create/update DSS evaluation records for committee level
            /// </summary>
            /// <param name="idCommittee"></param>
            /// <param name="idCall"></param>
            /// <param name="idSubmission"></param>
            /// <param name="dssEvaluations"></param>
            /// <param name="updatedOn"></param>
            /// <param name="isCompleted"></param>
            /// <param name="isValid"></param>
            /// <param name="isFuzzy"></param>
            /// <param name="ranking"></param>
            /// <param name="value"></param>
            /// <param name="valueFuzzy"></param>
            private void DssCommitteeEvaluationSet(long idCommittee, long idCall, long idSubmission, List<DssCallEvaluation> dssEvaluations, DateTime updatedOn, Boolean isCompleted, Boolean isValid, Boolean isFuzzy,Double ranking, Double value, String valueFuzzy)
            {
                DssCallEvaluation dEvaluation = dssEvaluations.Where(d => d.Type== DssEvaluationType.Committee && d.IdSubmission == idSubmission  && d.IdCommittee == idCommittee).FirstOrDefault();
                if (dEvaluation == null)
                {
                    dEvaluation = DssCallEvaluation.CreateForCommittee(idCommittee, idCall, idSubmission, isCompleted, isValid,ranking, value, valueFuzzy);
                    dEvaluation.IsFuzzy = isFuzzy;
                    dEvaluation.LastUpdateOn = updatedOn;
                    Manager.SaveOrUpdate(dEvaluation);
                    dssEvaluations.Add(dEvaluation);
                }
                else
                {
                    dEvaluation.IsCompleted = isCompleted;
                    dEvaluation.IsValid = isValid;
                    dEvaluation.IsFuzzy = isFuzzy;
                    dEvaluation.Value = value;
                    dEvaluation.ValueFuzzy = valueFuzzy;
                    dEvaluation.LastUpdateOn = updatedOn;
                    Manager.SaveOrUpdate(dEvaluation);
                }
            }
        #endregion

        #region Call Level

            private void DssRatingSetForCall(long idCall,Boolean isFuzzy,List<long> idSubmissions, List<DssCallEvaluation> dssEvaluations, DateTime updatedOn)
            {
                try
                {
                    foreach (long idSubmission in idSubmissions)
                    {
                        DssCallEvaluation cEvaluation = dssEvaluations.Where(d => d.IdSubmission == idSubmission && d.Type == DssEvaluationType.Committee).FirstOrDefault();
                        if (cEvaluation != null)
                            DssCallEvaluationSet(idCall, idSubmission, dssEvaluations, updatedOn, cEvaluation.IsCompleted, cEvaluation.IsValid, cEvaluation.IsFuzzy, cEvaluation.Ranking, cEvaluation.Value, cEvaluation.ValueFuzzy);
                        else
                            DssCallEvaluationSet(idCall, idSubmission, dssEvaluations, updatedOn, false, false, isFuzzy, 0, 0, "");
                    }
                }
                catch (Exception ex)
                {

                }
            }

            /// <summary>
            /// Get committee evaluations 
            /// </summary>
            /// <param name="idCall"> idCall </param>
            /// <param name="method">DSS TemplateMethod to apply</param>
            /// <param name="committees">list of evaluable committees</param>
            /// <param name="dssEvaluations">list of dss evaluation records for this call</param>
            /// <param name="values">list of committees evaluated submissions</param>
            /// <param name="updatedOn">update date for recalc usage</param>
            /// <returns></returns>
            private void DssRatingSetForCall(BaseForPaper call, TemplateMethod method, List<EvaluationCommittee> committees, List<long> idSubmissions, List<DssCallEvaluation> dssEvaluations, Dictionary<long, List<dtoDssEvaluationResultItem>> values, DateTime updatedOn)
            {
                try
                {
                    if (method != null && call!=null)
                    {
                        Boolean isValid = false;
                        Boolean isCompleted = !dssEvaluations.Any(e => e.Type == DssEvaluationType.Committee && !e.IsCompleted && committees.Any(c => c.Id == e.IdCommittee));
                        if (method.IsFuzzy)
                        {
                            dtoAlgorithmInputFuzzy aInput = CallGetAlgorithmInputFuzzy(call, method, committees, idSubmissions, values);
                            List<dtoAlgorithmAlternativeFuzzy> rValues = ServiceDss.Calculate(aInput);

                            isValid = method.DisplayPartialRating || (!aInput.HasEmptyValues() && !dssEvaluations.Any(e => e.Type == DssEvaluationType.Committee && committees.Any(c => c.Id == e.IdCommittee) && !e.IsValid));
                            foreach (long idSubmission in idSubmissions)
                            {
                                dtoAlgorithmAlternativeFuzzy fValue = rValues.Where(r => r.Id == idSubmission).FirstOrDefault();
                                if (fValue != null)
                                    DssCallEvaluationSet(call.Id, idSubmission, dssEvaluations, updatedOn, isCompleted, isValid, true, fValue.Ranking, fValue.FinalValue, fValue.FinalValueFuzzy);
                                else
                                    DssCallEvaluationSet(call.Id, idSubmission, dssEvaluations, updatedOn, isCompleted, isValid, true, 0, 0, "");                               
                            }
                        }
                        else
                        {
                            dtoAlgorithmInput aInput = CallGetAlgorithmInput(call, method, committees, idSubmissions, values);
                            List<dtoAlgorithmAlternative> rValues = ServiceDss.Calculate(aInput);
                            isValid = !aInput.HasEmptyValues() && !dssEvaluations.Any(e => e.Type == DssEvaluationType.Committee && committees.Any(c => c.Id == e.IdCommittee) && !e.IsValid);
                            foreach (long idSubmission in idSubmissions)
                            {
                                dtoAlgorithmAlternative fValue = rValues.Where(r => r.Id == idSubmission).FirstOrDefault();
                                if (fValue != null)
                                    DssCallEvaluationSet(call.Id, idSubmission, dssEvaluations, updatedOn, isCompleted, isValid, false, fValue.Ranking, fValue.FinalValue, fValue.FinalValueFuzzy);
                                else
                                    DssCallEvaluationSet(call.Id, idSubmission, dssEvaluations, updatedOn, isCompleted, isValid, false, 0, 0, "");           
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            /// <summary>
            /// Get algorithm input for call - fuzzy values
            /// </summary>
            /// <param name="method"></param>
            /// <param name="criteria"></param>
            /// <param name="idSubmissions"></param>
            /// <param name="evaluations"></param>
            /// <param name="values"></param>
            /// <returns></returns>
            private dtoAlgorithmInputFuzzy CallGetAlgorithmInputFuzzy(BaseForPaper call, TemplateMethod method, List<EvaluationCommittee> committees, List<long> idSubmissions, Dictionary<long, List<dtoDssEvaluationResultItem>> values)
            {
                dtoAlgorithmInputFuzzy aInput = new dtoAlgorithmInputFuzzy(method.Type);

                if (method.UseManualWeights)
                {
                    List<String> wValues = (String.IsNullOrWhiteSpace(call.FuzzyMeWeights) ? new List<String>() : call.FuzzyMeWeights.Split('#').ToList().Select(v => v.Split(':')[1]).ToList());
                    aInput.Weights = wValues.Select(v => LM.MathLibrary.Algorithms.TriangularFuzzyNumber.Parse(v)).ToList();
                }
                else
                    aInput.Weights = committees.Select(v => LM.MathLibrary.Algorithms.TriangularFuzzyNumber.Parse(v.WeightSettings.WeightFuzzy)).ToList();

                foreach (long idSubmission in idSubmissions)
                {
                    dtoAlgorithmAlternativeFuzzy alternative = new dtoAlgorithmAlternativeFuzzy() { Id = idSubmission, IsFuzzyValue=true };
                    if (values.Any())
                    {
                        foreach (EvaluationCommittee c in committees)
                        {
                            if (values.ContainsKey(c.Id) && values[c.Id].Any(a => a.IdSubmission == idSubmission))
                            {
                                dtoDssEvaluationResultItem value = values[c.Id].Where(a => a.IdSubmission == idSubmission).FirstOrDefault();
                                alternative.HasEmptyValues = value.HasEmptyValues;
                                alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(value.Value));
                            }
                            else
                            {
                                alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0));
                                alternative.HasEmptyValues = true;
                            }
                        }
                    }
                    else
                        committees.ForEach(c => alternative.Values.Add(new LM.MathLibrary.Algorithms.TriangularFuzzyNumber(0)));
                    aInput.Alternatives.Add(alternative);
                }
                return aInput;
            }
            /// <summary>
            /// Get algorithm input for call - NO fuzzy values
            /// </summary>
            /// <param name="method"></param>
            /// <param name="criteria"></param>
            /// <param name="evaluatorsCount"></param>
            /// <param name="idSubmissions"></param>
            /// <param name="evaluations"></param>
            /// <param name="values"></param>
            /// <returns></returns>
            private dtoAlgorithmInput CallGetAlgorithmInput(BaseForPaper call, TemplateMethod method, List<EvaluationCommittee> committees, List<long> idSubmissions, Dictionary<long, List<dtoDssEvaluationResultItem>> values)
            {
                dtoAlgorithmInput aInput = new dtoAlgorithmInput(method.Type);

                if (method.UseManualWeights)
                {
                    List<String> wValues = (String.IsNullOrWhiteSpace(call.FuzzyMeWeights) ? new List<String>() : call.FuzzyMeWeights.Split('#').ToList().Select(v => v.Split(':')[1]).ToList());
                    aInput.Weights = wValues.Select(v => Double.Parse(v)).ToList();
                }
                else
                    aInput.Weights = committees.Select(v => v.WeightSettings.Weight).ToList();

                foreach (long idSubmission in idSubmissions)
                {
                    dtoAlgorithmAlternative alternative = new dtoAlgorithmAlternative() { Id = idSubmission };
                    //int count = evaluations.Where(e => e.Submission != null && e.Submission.Id == idSubmission).Count();
                    if (values.Any())
                    {
                        foreach (EvaluationCommittee c in committees)
                        {
                            if (values.ContainsKey(c.Id) && values[c.Id].Any(a=> a.IdSubmission==idSubmission))
                            {
                                dtoDssEvaluationResultItem value = values[c.Id].Where(a => a.IdSubmission == idSubmission).FirstOrDefault();
                                alternative.HasEmptyValues = value.HasEmptyValues;
                                alternative.Values.Add(value.Value);
                            }
                            else
                            {
                                alternative.Values.Add(0);
                                alternative.HasEmptyValues = true;
                            }
                        }
                    }
                    else
                    {
                        committees.ForEach(c => alternative.Values.Add(0));
                        alternative.HasEmptyValues = true;
                    }
                    aInput.Alternatives.Add(alternative);
                }
                return aInput;
            }
            /// <summary>
            /// Create/update DSS evaluation records for call level
            /// </summary>
            /// <param name="idCall"></param>
            /// <param name="idSubmission"></param>
            /// <param name="dssEvaluations"></param>
            /// <param name="updatedOn"></param>
            /// <param name="isCompleted"></param>
            /// <param name="isValid"></param>
            /// <param name="isFuzzy"></param>
            /// <param name="value"></param>
            /// <param name="valueFuzzy"></param>
            private void DssCallEvaluationSet(long idCall, long idSubmission, List<DssCallEvaluation> dssEvaluations, DateTime updatedOn, Boolean isCompleted, Boolean isValid, Boolean isFuzzy, Double ranking,  Double value, String valueFuzzy)
            {
                DssCallEvaluation dEvaluation = dssEvaluations.Where(d => d.Type == DssEvaluationType.Call && d.IdSubmission == idSubmission && d.IdCall == idCall).FirstOrDefault();
                if (dEvaluation == null)
                {
                    dEvaluation = DssCallEvaluation.CreateForCall(idCall, idSubmission, isCompleted, isValid, ranking,value, valueFuzzy);
                    dEvaluation.LastUpdateOn = updatedOn;
                    dEvaluation.IsFuzzy = isFuzzy;
                    Manager.SaveOrUpdate(dEvaluation);
                    dssEvaluations.Add(dEvaluation);
                }
                else
                {
                    dEvaluation.IsCompleted = isCompleted;
                    dEvaluation.IsValid = isValid;
                    dEvaluation.IsFuzzy = isFuzzy;
                    dEvaluation.Value = value;
                    dEvaluation.ValueFuzzy = valueFuzzy;
                    dEvaluation.Ranking = ranking;
                    dEvaluation.LastUpdateOn = updatedOn;
                    Manager.SaveOrUpdate(dEvaluation);
                }
            }
        #endregion

        #region "Update manual Settings to Committee/call"
            public void UpdateCommitteeManualSettings(List<dtoCommittee> committees,long idCriterion)
            {
                dtoCommittee committee = committees.Where(c => c.Criteria.Any(cc => cc.Id == idCriterion)).FirstOrDefault();
                if (committee != null && committee.MethodSettings.UseManualWeights)
                {
                    List<String> values = (String.IsNullOrWhiteSpace(committee.WeightSettings.FuzzyMeWeights) ? new List<String>() : committee.WeightSettings.FuzzyMeWeights.Split('#').ToList());
                    if (values.Any()){
                        if (committee.MethodSettings.UseOrderedWeights)
                        {
                            switch (values.Count())
                            {
                                case 0:
                                    break;
                                case 1:
                                case 2:
                                    values[values.Count - 1] = "";
                                    break;
                                default:
                                    values.RemoveAt(values.Count - 2);
                                    if (values[values.Count - 1].Contains(":"))
                                        values[values.Count - 1] = values.Count.ToString() + ":" + values[values.Count - 1].Split(':')[1];
                                    break;
                            }
                        }
                        else
                            values = values.Where(v => !v.StartsWith(idCriterion.ToString() + ":")).ToList();
                        committee.MethodSettings.FuzzyMeWeights = (values.Count==1 ? values.FirstOrDefault() : String.Join("#", values));
                        committee.WeightSettings.FuzzyMeWeights = committee.MethodSettings.FuzzyMeWeights;
                        EvaluationCommittee eCommittee = Manager.Get<EvaluationCommittee>(committee.Id);
                        if (eCommittee != null)
                        {
                            eCommittee.WeightSettings.FuzzyMeWeights = committee.WeightSettings.FuzzyMeWeights;
                            Manager.SaveOrUpdate(eCommittee);
                        }
                    }
                }
            }
            public void UpdateCallManualSettings(long idCall, List<dtoCommittee> availableCommittees, long idCommittee)
            {
                BaseForPaper bCall = Manager.Get<BaseForPaper>(idCall);
                CallForPaper call = Manager.Get<CallForPaper>(idCall);
                if (call != null && bCall != null && bCall.UseManualWeights)
                {
                    List<dtoCommittee> committees = availableCommittees.Where(c => c.Id != idCommittee).ToList();
                    if (committees.Any())
                    {
                        List<String> values = (String.IsNullOrWhiteSpace(bCall.FuzzyMeWeights) ? new List<String>() : bCall.FuzzyMeWeights.Split('#').ToList());
                        if (values.Any())
                        {
                            if (bCall.UseOrderedWeights)
                            {
                                switch (values.Count())
                                {
                                    case 0:
                                        break;
                                    case 1:
                                    case 2:
                                        values[values.Count - 1] = "";
                                        break;
                                    default:
                                        values.RemoveAt(values.Count - 2);
                                        if (!String.IsNullOrWhiteSpace(values[values.Count - 1]) && values[values.Count - 1].Contains(":"))
                                            values[values.Count - 1] = values.Count.ToString() + ":" + values[values.Count - 1].Split(':')[1];
                                        break;
                                }
                            }
                            else
                                values = values.Where(v => !v.StartsWith(idCommittee.ToString() + ":")).ToList();

                            call.IsValidFuzzyMeWeights = false;
                            call.FuzzyMeWeights = (values.Count == 1 ? values.FirstOrDefault() : String.Join("#", values));
                            bCall.IsValidFuzzyMeWeights = false;
                            bCall.FuzzyMeWeights = call.FuzzyMeWeights;
                        }
                    }
                    else
                    {
                        call.IsValidFuzzyMeWeights = false;
                        call.FuzzyMeWeights = "";
                        bCall.IsValidFuzzyMeWeights = false;
                        bCall.FuzzyMeWeights = "";
                    }
                    Manager.SaveOrUpdate(bCall);
                    Manager.SaveOrUpdate(call);
                }
            }
        #endregion
    }
}