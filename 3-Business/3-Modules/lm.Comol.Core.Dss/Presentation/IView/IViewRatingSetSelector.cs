using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Domain.Templates;

namespace lm.Comol.Core.Dss.Presentation.Evaluation
{
    public interface IViewRatingSetSelector : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean DisplayNumberField { get; set; }
        Int32 NumberFieldDefaultValue { get; set; }
        Boolean Disabled { get; set; }
        Boolean IsFuzzy { get; set; }

        Dictionary<long, long> GetRatingSetSelected();
        Dictionary<long, List<dtoGenericRatingValue>> GetRatingValues();

        void InitializeControl(dtoSelectMethod method, Boolean disabled = false);
        void InitializeControl(AlgorithmType type,Boolean isFuzzy, Boolean disabled = false);
        void LoadRatingSet(List<dtoSelectRatingSet> items);
        void DisplaySessionTimeout();
    }
}