using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dss.Business;
using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Domain.Templates;
using LM.MathLibrary.Algorithms;
using LM.MathLibrary.Extensions;
namespace lm.Comol.Core.Dss.Presentation.Evaluation
{
    public class AggregationSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewAggregationSelector View
            {
                get { return (IViewAggregationSelector)base.View; }
            }
            private ServiceDss _Service;
            private ServiceDss Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceDss(AppContext);
                    return _Service;
                }
            }
            public AggregationSelectorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AggregationSelectorPresenter(iApplicationContext oContext, IViewAggregationSelector view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idFatherMethod,List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> methods, dtoItemMethodSettings settings,Boolean allowInheritsFromFather, Boolean disabled,List<dtoItemWeightBase> itemsWeight = null)
        {
            View.IdFatherMethod = idFatherMethod;
            View.AllowInheritsFromFather = allowInheritsFromFather;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.HideNormalizationMessage();
                View.Disabled = disabled;
                if (methods.Any())
                {
                    long idMethod = settings.IdMethod;
                    if (settings.IsDefaultForChildren && idMethod == 0)
                        idMethod = methods.FirstOrDefault().Id;
                    else if (settings.InheritsFromFather)
                        idMethod = -1;
                    List<long> idManualMethods = methods.Where(m => m.UseManualWeights).Select(m => m.Id).ToList();
                    List<long> idOrderedMethods = methods.Where(m => m.UseManualWeights && m.Type== AlgorithmType.owa).Select(m => m.Id).ToList();
                    List<long> idFuzzyMethods = methods.Where(m => m.IsFuzzy).Select(m => m.Id).ToList();
                    View.IdManualMethods = idManualMethods;
                    View.IdOrderedMethods = idOrderedMethods;
                    View.IdFuzzyMethods = idFuzzyMethods;
                    View.LoadMethods(methods, idMethod, settings.InheritsFromFather);
                    if (methods.Any(m => m.Id == idMethod) && !settings.InheritsFromFather)
                        UpdateView(methods.Where(m => m.Id == idMethod).SelectMany(m => m.RatingSets).ToList(), settings.IdRatingSet, idFuzzyMethods.Contains(idMethod), idManualMethods.Contains(idMethod), idOrderedMethods.Contains(settings.IdMethod), itemsWeight);
                    else if (settings.IsDefaultForChildren)
                        UpdateView(null, settings.IdRatingSet, idFuzzyMethods.Contains(settings.IdMethod), idManualMethods.Contains(settings.IdMethod), idOrderedMethods.Contains(settings.IdMethod), itemsWeight);
                    else if (settings.InheritsFromFather)
                        UpdateView(idFuzzyMethods.Contains(settings.IdMethod), idManualMethods.Contains(settings.IdMethod), idOrderedMethods.Contains(settings.IdMethod), itemsWeight);
                }
                else
                    View.LoadNoMethods();
            }
        }

        private void UpdateView(List<dtoSelectRatingSet> items, long idRatingSet, Boolean isFuzzy, Boolean useManualWeights, Boolean useOrderedWeights, List<dtoItemWeightBase> weights)
        {
            UpdateView(isFuzzy, useManualWeights, useOrderedWeights, weights);
            if (!useManualWeights)
                View.LoadRatingSets(items, idRatingSet);
        }
        private void UpdateView(Boolean isFuzzy, Boolean useManualWeights, Boolean useOrderedWeights, List<dtoItemWeightBase> weights)
        {
            View.UseOrderedWeights = useOrderedWeights;
            View.UseManualWeights = useManualWeights;
            View.UseFuzzyWeights = isFuzzy;
            View.AvailableWeightItems = weights;
            if (useManualWeights)
                UpdateView(isFuzzy, useOrderedWeights, weights);
            else
                View.HideManualWeights();
        }
        private void UpdateView(Boolean isFuzzy,Boolean useOrderedWeights, List<dtoItemWeightBase> weights)
        {
            List<dtoItemWeight> wItems = null;
            if (weights != null)
            {
                wItems = weights.Select(w => new dtoItemWeight(w)).ToList();
                switch (weights.Count)
                {
                    case 0:
                        break;
                    case 1:
                        wItems[0].IsFirst = true;
                        wItems[0].IsLast = true;
                        break;
                    default:
                        wItems[0].IsFirst = true;
                        wItems.LastOrDefault().IsLast = true;
                        break;
                }
                if (useOrderedWeights && wItems.Count>0)
                {
                    switch (wItems.Count)
                    {
                        case 1:
                            wItems[0].Name = View.GetMaxWeightName;
                            break;
                        default:
                            String template = View.GetIntermediateWeightName;
                            wItems[0].Name = View.GetMaxWeightName;
                            wItems.LastOrDefault().Name = View.GetMinWeightName;
                            for (Int32 index = 1; index < wItems.Count() - 1; index++)
                            {
                                wItems[index].Name = String.Format(template, (index + 1));
                            }
                            break;
                    }
                }
            }
            if (wItems!= null && wItems.Any())
                CheckWeightsNormalization(wItems, isFuzzy, false);
            View.LoadManualWeights(wItems, useOrderedWeights, isFuzzy);
        }

        public Boolean CheckWeightsNormalization(List<dtoItemWeight> weights, Boolean isFuzzy, Boolean autoUpdateWeights)
        {
            NormalizationStatus nStatus = CheckWeights(weights, isFuzzy, autoUpdateWeights);
            switch (nStatus)
            {
                case NormalizationStatus.normalized:
                    View.HideNormalizationMessage();
                    return true;
                case NormalizationStatus.normalizable:
                    View.DisplayNormalizationMessage(nStatus);
                    return true;
                default:
                    View.DisplayNormalizationMessage(nStatus);
                    return false;
            }
        }
        private NormalizationStatus CheckWeights(List<dtoItemWeight> weights, Boolean isFuzzy, Boolean autoUpdateWeights)
        {
            NormalizationStatus nStatus = NormalizationStatus.none;
            if (weights.Any(i => !String.IsNullOrWhiteSpace(i.Value)))
            {
                LM.MathLibrary.Extensions.FuzzyExtension.Normalization status;
                TriangularFuzzyNumber[] w = null;
                try
                {
                    if (isFuzzy)
                        w = weights.Select(i => TriangularFuzzyNumber.Parse(i.Value)).ToArray();
                    else
                        w = weights.Select(i => new TriangularFuzzyNumber(long.Parse(i.Value))).ToArray();
                    w = w.NormalizeWeights(out status).ToArray();
                    if (status == FuzzyExtension.Normalization.normalizable)
                    {
                        Int32 index = 0;
                        foreach (TriangularFuzzyNumber i in w)
                        {
                            if (w[index] != null)
                                weights[index].Value = (isFuzzy ? w[index].ToString() : w[index].ToCrispy().ToString());
                            else
                                weights[index].Value = "";
                            index++;
                        }
                        if (autoUpdateWeights)
                            View.UpdateWeights(weights);
                    }
                }
                catch (Exception ex)
                {
                    status = FuzzyExtension.Normalization.none;
                }
                nStatus = TranslateStatus(status);
            }
           
            View.CurrentNormalization = nStatus;
            return nStatus;
        }
        private NormalizationStatus TranslateStatus(FuzzyExtension.Normalization status)
        {
            switch (status)
            {
                case FuzzyExtension.Normalization.impossible:
                    return NormalizationStatus.impossible;
                case FuzzyExtension.Normalization.none:
                    return NormalizationStatus.fatalerror;
                case FuzzyExtension.Normalization.normalizable:
                    return NormalizationStatus.normalizable;
                case FuzzyExtension.Normalization.normalized:
                    return NormalizationStatus.normalized;
                default:
                    return NormalizationStatus.none;
            }
        }

        public void SelectMethod(long idFatherMethod, long idMethod, long idRatingSet, Boolean currentIsFuzzy, Boolean currentManualWeights, Boolean currentOrderedWeights)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean useOrderedWeights = ((idMethod < 1) ? View.IdOrderedMethods.Contains(idFatherMethod) : View.IdOrderedMethods.Contains(idMethod));
                Boolean useManualWeights = ((idMethod < 1) ? View.IdManualMethods.Contains(idFatherMethod) : View.IdManualMethods.Contains(idMethod));
                Boolean useFuzzyWeights = ((idMethod < 1) ? View.IdFuzzyMethods.Contains(idFatherMethod) : View.IdFuzzyMethods.Contains(idMethod));
                View.UseManualWeights = useManualWeights;
                View.UseOrderedWeights = useOrderedWeights;
                View.UseFuzzyWeights = useFuzzyWeights;
                if (idMethod < 1){
                    View.HideRatingSetSelectionForInheritance();
                    if (useManualWeights){
                        if (!(useFuzzyWeights == currentIsFuzzy && currentManualWeights && currentOrderedWeights == useOrderedWeights))
                            UpdateView(useFuzzyWeights, useOrderedWeights, View.RequireNewWeights(useFuzzyWeights,useOrderedWeights));
                    }
                    else
                        View.HideManualWeights();
                }
                else
                {
                    if (useManualWeights)
                        UpdateView(useFuzzyWeights, useOrderedWeights, View.RequireNewWeights(useFuzzyWeights, useOrderedWeights));
                    else
                    {
                        View.HideManualWeights();
                        View.LoadRatingSets(Service.RatingSetGetAvailable(idMethod, UserContext.Language.Id, CurrentManager.GetDefaultIdLanguage()), idRatingSet);
                    }
                }
                View.UpdateCurrentMethod();
            }
        }
    }
}