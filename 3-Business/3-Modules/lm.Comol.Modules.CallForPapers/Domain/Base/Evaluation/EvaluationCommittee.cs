using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class EvaluationCommittee : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual BaseForPaper Call { get; set; }
        public virtual Boolean ForAllSubmittersType { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual IList<BaseCriterion> Criteria { get; set; }
        public virtual IList<CommitteeAssignedSubmitterType> AssignedTypes { get; set; }
        public virtual IList<CommitteeMember> Members { get; set; }
        public virtual Boolean UseDss  { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.Templates.ItemMethodSettings MethodSettings { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.Templates.ItemWeightSettings WeightSettings { get; set; }

        public virtual Boolean HasDssInvalidMethod()
        {
            return (!MethodSettings.InheritsFromFather && (MethodSettings.IdMethod<1 || (!MethodSettings.UseManualWeights && MethodSettings.IdRatingSet <1 )))
                    ||
                    (MethodSettings.InheritsFromFather && (Call.IdDssMethod<1 || (!MethodSettings.UseManualWeights && Call.IdDssRatingSet <1) ));
        }
        public virtual Boolean HasDssInvalidWeight(Boolean multipleCommittees)
        {
            return multipleCommittees && (!Call.UseManualWeights && ((!WeightSettings.IsValidFuzzyMeWeights && WeightSettings.ManualWeights) || (!WeightSettings.ManualWeights && (WeightSettings.IdRatingValue < 1)
                    || (((WeightSettings.RatingType & Core.Dss.Domain.RatingType.intermediateValues)>0) && WeightSettings.IdRatingValueEnd<1)
                    || ((WeightSettings.RatingType & Core.Dss.Domain.RatingType.extended)>0 && WeightSettings.IdRatingValueEnd<1))));
        }
        public virtual Boolean HasDssErrors(Boolean multipleCommittees)
        {
            Boolean hasErrors = HasDssInvalidMethod() || HasDssInvalidWeight(multipleCommittees);
            hasErrors = hasErrors || (!MethodSettings.UseManualWeights && (Criteria != null && Criteria.Any(c=> c.Deleted== BaseStatusDeleted.None && c.HasDssErrors())));
            hasErrors = hasErrors || (Criteria != null && (Criteria.Any(c=>c.Deleted== BaseStatusDeleted.None && (c.Type== CriterionType.RatingScale || c.Type== CriterionType.RatingScaleFuzzy))
                        && Criteria.Where(c => c.Deleted == BaseStatusDeleted.None && (c.Type == CriterionType.RatingScale || c.Type == CriterionType.RatingScaleFuzzy)).
                        Select(c => (DssCriterion)c).Any(c => c.HasDssFuzzyErrors(MethodSettings))));
            return hasErrors;
            //return UseDss && (
            //    (!MethodSettings.InheritsFromFather && (MethodSettings.IdMethod<1 || MethodSettings.IdRatingSet <1 ))
            //    ||
            //    (MethodSettings.InheritsFromFather && (Call.IdDssMethod<1 || Call.IdDssRatingSet <1 )))
            //    ||
            //    (   WeightSettings.IdRatingValue <1
            //        || ((WeightSettings.RatingType & Core.Dss.Domain.RatingType.intermediateValues)>0) && WeightSettings.IdRatingValueEnd<1 ) 
            //        || ((WeightSettings.RatingType & Core.Dss.Domain.RatingType.extended)>0 && WeightSettings.IdRatingValueEnd<1)
            //    || (Criteria != null &&
            //        (
            //        Criteria.Any(c=> c.Deleted== BaseStatusDeleted.None && c.HasDssErrors())
            //        ||
            //        (Criteria.Any(c=>c.Deleted== BaseStatusDeleted.None && c.Type== CriterionType.RatingScale || c.Type== CriterionType.RatingScaleFuzzy)
            //        && Criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == CriterionType.RatingScale || c.Type == CriterionType.RatingScaleFuzzy).
            //        Select(c => (DssCriterion)c).Any(c => c.HasDssFuzzyErrors(MethodSettings)))));
        }
        public EvaluationCommittee() {
            Criteria = new List<BaseCriterion>();
            AssignedTypes = new List<CommitteeAssignedSubmitterType>();
            Members = new List<CommitteeMember>();

        }
    }
}