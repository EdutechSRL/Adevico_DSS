using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoBasicCommitteeItem
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual List<dtoBasicComitteeEvaluatorItem> Evaluators { get; set; }
        public virtual long DisplayOrder { get; set; }

        public dtoBasicCommitteeItem()
        {
            Evaluators = new List<dtoBasicComitteeEvaluatorItem>();
        }

        public virtual long ItemsCount(ManageEvaluationsAction action) {
            return (Evaluators == null || (Evaluators.Any() && Evaluators.Count == 0)) ? 0 : Evaluators.Select(e => e.ItemsCount(action)).Sum();
        }
    }

    [Serializable]
    public class dtoBasicComitteeEvaluatorItem
    {
        public virtual long IdEvaluator { get; set; }
        public virtual long IdCommittee { get; set; }
        public virtual long Completed { get; set; }
        public virtual long Started { get; set; }
        public virtual String Name { get; set; }
        public dtoBasicComitteeEvaluatorItem()
        {

        }
        public virtual long ItemsCount(ManageEvaluationsAction action) { 
            switch (action){
                case ManageEvaluationsAction.OpenAll:
                    return Completed;
                case ManageEvaluationsAction.CloseAll:
                    return Started;
                default:
                    return 0;
            }
        }
    }

    [Serializable]
    public class dtoComitteeEvaluatorItem :dtoBasicComitteeEvaluatorItem
    {
        public virtual long IdSubmission { get; set; }
        public virtual long IdEvaluation { get; set; }
        public dtoComitteeEvaluatorItem()
        {

        }
    }
    [Serializable]
    public class dtoBasicSubmissionItem
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdOwner { get; set; }
        public virtual Boolean isAnonymous{ get; set; }
        public virtual String DisplayName { get; set; }
        public virtual List<dtoBasicSubmissionCommitteeItem> Committees { get; set; }
        public virtual long ItemsCount(ManageEvaluationsAction action)
        {
            return (Committees == null || (Committees.Any() && Committees.Count == 0)) ? 0 : Committees.Select(e => e.ItemsCount(action)).Sum();
        }

        public virtual List<long> GetIdEvaluations()
        {
            return Committees.SelectMany(c => c.GetIdEvaluations()).ToList();
        }
}

    [Serializable]
    public class dtoBasicSubmissionCommitteeItem 
    {
        public virtual long IdSubmission { get; set; }
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual List<dtoComitteeEvaluatorItem> Evaluators { get; set; }
        public virtual long DisplayOrder { get; set; }

        public dtoBasicSubmissionCommitteeItem()
        {
            Evaluators = new List<dtoComitteeEvaluatorItem>();
        }

        public virtual long ItemsCount(ManageEvaluationsAction action) {
            return (Evaluators == null || (Evaluators.Any() && Evaluators.Count == 0)) ? 0 : Evaluators.Select(e => e.ItemsCount(action)).Sum();
        }
        public virtual List<long> GetIdEvaluations()
        {
            return (Evaluators == null || (Evaluators.Any() && Evaluators.Count == 0)) ? new List<long>() : Evaluators.Select(e => e.IdEvaluation).ToList();
        }
    }


    //[Serializable]
    //public class dtoSelectSubmissionItem
    //{
    //    public virtual long IdSubmission { get; set; }
    //    public virtual List<dtoSelectSubmissionCommitteeItem> Committees { get; set; }

    //    public virtual dtoSelectSubmissionItem()
    //    {
    //        Committees = new List<dtoSelectSubmissionCommitteeItem>();
    //    }

    //    public virtual List<dtoSelectSubmissionCommitteeEvaluatorItem> GetItems(Boolean selected)
    //    {
    //        return Committees.SelectMany(c => c.GetItems(selected)).ToList();
    //    }
    //}

    //[Serializable]
    //public class dtoSelectSubmissionCommitteeItem
    //{
    //    public virtual long IdSubmission { get; set; }
    //    public virtual long IdCommittee { get; set; }
    //    public virtual List<dtoSelectSubmissionCommitteeEvaluatorItem> Evaluators { get; set; }
    //    public virtual Boolean Selected { get; set; }
    //    public virtual dtoSelectSubmissionCommitteeItem()
    //    {
    //        Evaluators = new List<dtoSelectSubmissionCommitteeEvaluatorItem>();
    //    }

    //    public virtual List<dtoSelectSubmissionCommitteeEvaluatorItem> GetItems(Boolean selected) {
    //        return Evaluators.Where(e => e.Selected == selected).ToList();
    //    }
    //}
    [Serializable]
    public class dtoSelectEvaluationItem
    {
        public virtual long IdEvaluation { get; set; }
        //public virtual long IdEvaluator { get; set; }
        //public virtual long IdSubmission { get; set; }
        //public virtual long IdCommittee { get; set; }
        public virtual Boolean Selected { get; set; }
    }
}