using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoCommittee : dtoBaseCommittee 
    {
        public virtual long IdCall { get; set; }
        public virtual IList<dtoCriterion> Criteria { get; set; }
        public virtual List<long> Submitters { get; set; }
        public virtual List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> BaseWeights { get; set; }
        public virtual Boolean HasDssErrors(Int32 committeesCount)
        {
            return UseDss && (MethodSettings.Error != Core.Dss.Domain.DssError.None || (committeesCount>1 && WeightSettings.Error != Core.Dss.Domain.DssError.None)
                                || (!MethodSettings.UseManualWeights && Criteria != null && Criteria.Any() && Criteria.Any(c => c.HasDssErrors())));
        }
        public virtual List<lm.Comol.Core.Dss.Domain.DssError> GetDssErrors()
        {
            List<lm.Comol.Core.Dss.Domain.DssError> errors = new List<Core.Dss.Domain.DssError>();
            if (UseDss){
                if ((MethodSettings.Error & Core.Dss.Domain.DssError.MissingMethod)>0)
                    errors.Add(lm.Comol.Core.Dss.Domain.DssError.MissingMethod);
                if ((MethodSettings.Error & Core.Dss.Domain.DssError.MissingRatingSet)>0)
                    errors.Add(lm.Comol.Core.Dss.Domain.DssError.MissingRatingSet);
                if ((WeightSettings.Error & Core.Dss.Domain.DssError.MissingWeight)>0)
                    errors.Add(lm.Comol.Core.Dss.Domain.DssError.MissingWeight);
                if ((WeightSettings.Error & Core.Dss.Domain.DssError.InvalidWeight)>0)
                    errors.Add(lm.Comol.Core.Dss.Domain.DssError.InvalidWeight);
                if ((WeightSettings.Error & Core.Dss.Domain.DssError.InvalidManualWeight) > 0)
                    errors.Add(lm.Comol.Core.Dss.Domain.DssError.InvalidManualWeight);
                if ((WeightSettings.Error & Core.Dss.Domain.DssError.MissingManualWeight) > 0)
                    errors.Add(lm.Comol.Core.Dss.Domain.DssError.MissingManualWeight);
            }
            return errors;
        }
        public virtual List<lm.Comol.Core.Dss.Domain.DssError> GetCriteriaDssErrors()
        {
            List<lm.Comol.Core.Dss.Domain.DssError> errors = new List<Core.Dss.Domain.DssError>();
            if (UseDss && !MethodSettings.UseManualWeights )
            {
               errors.AddRange(Criteria.SelectMany(c=> c.GetDssErrors()).Distinct().ToList());
            }
            return errors;
        }
        public dtoCommittee() {
            BaseWeights = new List<Core.Dss.Domain.Templates.dtoItemWeightBase>();
            Criteria = new List<dtoCriterion>();
            Submitters = new List<long>();
        }
        public dtoCommittee(long id, long idCall, int displayOrder, String name, String description, BaseStatusDeleted deleted)
            : base()
        {
            Id = id;
            Deleted = deleted;
            IdCall = idCall;
            DisplayOrder = displayOrder;
            Name = name;
            Description = description;
            Criteria = new List<dtoCriterion>();
            Submitters = new List<long>();
            BaseWeights = new List<Core.Dss.Domain.Templates.dtoItemWeightBase>();
        }
        public dtoCommittee(long id, long idCall, int displayOrder, String name, String description, BaseStatusDeleted deleted, List<dtoCriterion> criteria)
            : this(id,idCall, displayOrder, name, description, deleted)
        {
            Criteria = criteria;
        }
        public dtoCommittee(EvaluationCommittee committee, List<long> idSubmitters)
            : this(committee.Id, (committee.Call != null) ? committee.Call.Id : 0, committee.DisplayOrder, committee.Name, committee.Description, committee.Deleted)
        {
            this.ForAllSubmittersType = committee.ForAllSubmittersType;
            Submitters = idSubmitters;
            //Criteria = new List<dtoCriterion>();
        }

        public virtual Dictionary<long, String> GetFuzzyMeItems()
        {
            Dictionary<long, String> results = new Dictionary<long, String>();
            if (WeightSettings != null && !String.IsNullOrWhiteSpace(WeightSettings.FuzzyMeWeights))
            {
                List<String> gItems = WeightSettings.FuzzyMeWeights.Split('#').ToList();
                foreach (String g in gItems)
                {
                    List<String> values = g.Split(':').ToList();
                    if (values.Any())
                    {
                        long id = 0;
                        if (long.TryParse(values[0], out id))
                        {
                            if (results.ContainsKey(id))
                                results[id] = (values.Count() == 2 ? values[1] : results[id]);
                            else
                                results.Add(id, (values.Count() == 2 ? values[1] : ""));
                        }
                    }
                }
            }
            return results;
        }
    }
}