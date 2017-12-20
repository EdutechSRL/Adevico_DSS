using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewAddCriterion : IViewBase
    {
        long IdCall { get; set; }
        Boolean UseDss { get; set; }
        CriterionType CurrentType { get; set; }
        List<dtoCriterion> GetCriteriaToCreate();
        void InitializeControl(long idCall, Boolean useDss);
        void InitializeControl(long idCall, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings, lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod method);

        void InitializeRatingScale(lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod method);
        void InitializeRatingScale(lm.Comol.Core.Dss.Domain.AlgorithmType algorithmType,Boolean isFuzzy);
        void LoadAvailableTypes(List<DisplayCriterionType> items);
        void LoadCriteria(List<dtoCriterionEvaluated> criteria);
        List<BaseCriterion> CreateCriteria(List<dtoCommittee> committees,lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings, long idCommittee);
    }
}