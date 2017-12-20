using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Domain.Templates;
using LM.MathLibrary.Extensions;

namespace lm.Comol.Core.Dss.Presentation.Evaluation
{
    public interface IViewAggregationSelector : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long IdObjectItem { get; set; }
        long IdFatherMethod { get; set; }
        Boolean IsDefaultForChildren { get; set; }
        Boolean AllowInheritsFromFather { get; set; }
        Boolean RaiseEventForMethodSelect { get; set; }
        Boolean RaiseEventForRatingSetSelect { get; set; }
        
        DssError CurrentError { get; set; }
        Boolean UseManualWeights { get; set; }
        Boolean UseOrderedWeights { get; set; }
        Boolean UseFuzzyWeights { get; set; }
        Boolean Disabled { get; set; }
        Boolean isValid { get; }
        String TranslationSelectMethod { get; set; }
        String TranslationSelectRating { get; set; }
        String TranslationInherits { get; set; }
        String TranslationMethodTitle { set; }
        String TranslationRatingSetTitle { set; }
        String TranslationWeightsSetTitle { set; }
        String GetMaxWeightName { get; }
        String GetMinWeightName { get; }
        String GetIntermediateWeightName { get; }
        List<long> IdFuzzyMethods { get;set; }
        List<long> IdManualMethods { get; set; }
        List<long> IdOrderedMethods { get; set; }
        List<dtoItemWeightBase> AvailableWeightItems { get; set; }
        void InitializeControl(long idFatherMethod,List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> methods, long idMethod, long idRatingSet, Boolean inheritsFromFather, Boolean isDefaultForChildren, Boolean allowInheritsFromFather, Boolean disabled = false, DssError err = DssError.None);
        void InitializeControl(long idFatherMethod, List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> methods, dtoItemMethodSettings settings, Boolean allowInheritsFromFather, Boolean disabled = false, DssError err = DssError.None);

        void InitializeControl(long idFatherMethod, List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> methods, long idMethod, long idRatingSet, List<dtoItemWeightBase> weightItems, Boolean inheritsFromFather, Boolean isDefaultForChildren, Boolean allowInheritsFromFather, Boolean disabled = false, DssError err = DssError.None);
        void InitializeControl(long idFatherMethod, List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> methods, dtoItemMethodSettings settings, List<dtoItemWeightBase> weightItems, Boolean allowInheritsFromFather, Boolean disabled = false, DssError err = DssError.None);


        dtoItemMethodSettings GetSettings();
        void LoadMethods(List<dtoSelectMethod> methods, long idSelected, Boolean inheritsFromCall);
        void LoadNoMethods();
        void LoadRatingSets(List<dtoSelectRatingSet> items, long idSelected);

        void HideRatingSetSelectionForInheritance();
        void DisplayEmptyRatingSet();
        void DisplaySessionTimeout();
        void UpdateCurrentMethod();
        void LoadManualWeights(List<dtoItemWeight> weights,Boolean areOrdered, Boolean areFuzzy);
        void HideManualWeights();

        NormalizationStatus CurrentNormalization { get; set; }
        List<dtoItemWeight> GetManualWeights();
        void HideNormalizationMessage();
        void DisplayNormalizationMessage(NormalizationStatus normalization);
        void UpdateWeights(List<dtoItemWeight> weights);
        List<dtoItemWeightBase> RequireNewWeights(Boolean newAreFuzzyWeights, Boolean newOrderedWeights);
    }
}