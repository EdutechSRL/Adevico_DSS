using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Domain.Templates;

namespace lm.Comol.Core.Dss.Presentation.Evaluation
{
    public interface IViewFuzzyNumber : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        void InitializeControl(Boolean isFuzzy, Double value);
        void InitializeControl(Boolean isFuzzy, Double ranking, Double value);
        void InitializeControl(Boolean isFuzzy, Double value, String fuzzyString, String name, String shortName);
        void InitializeControl(Boolean isFuzzy, Double ranking, Double value, String fuzzyString, String name, String shortName);
        void InitializeControl(Boolean isFuzzy, Double value, String fuzzyString, RatingType type, String name, String shortName, String endName = "", String endShortName="");
        void InitializeControl(Boolean isFuzzy, Double ranking, Double value, String fuzzyString, RatingType type, String name, String shortName, String endName = "", String endShortName = "");

        void RenderFuzzyNumber(String name, String shortName, String fuzzyString, String centerOfGravity);
        void RenderRankingFuzzyNumber(String name, String shortName, String ranking, String fuzzyString, String centerOfGravity);
        void RenderRankingFuzzyNumber(String name, String shortName, String fuzzyString, String centerOfGravity);
        void RenderNumber(String name, String number);
        void DisplaySessionTimeout();
    }
}