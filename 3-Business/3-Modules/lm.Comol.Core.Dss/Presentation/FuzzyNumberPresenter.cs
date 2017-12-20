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
    public class FuzzyNumberPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewFuzzyNumber View
        {
            get { return (IViewFuzzyNumber)base.View; }
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
        public FuzzyNumberPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public FuzzyNumberPresenter(iApplicationContext oContext, IViewFuzzyNumber view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(Boolean isFuzzy, Double value)
        {
            if (isFuzzy)
            {
                TriangularFuzzyNumber fValue = value.ToFuzzy();
                View.RenderFuzzyNumber(value.ToString(), value.ToString(), fValue.ToString(), fValue.CenterOfGravity.Round());
            }
            else
            {
                if (Math.Floor(value) == Math.Round(value, 3))
                    View.RenderNumber(Math.Floor(value).ToString(), Math.Floor(value).ToString());
                else
                    View.RenderNumber(value.Round().ToString(), value.Round());
            }
        }
        public void InitView(Boolean isFuzzy, Double ranking, Double value)
        {
            if (isFuzzy)
            {
                TriangularFuzzyNumber fValue = value.ToFuzzy();
                View.RenderRankingFuzzyNumber(value.ToString(), ranking.ToString(), ranking.ToString(), fValue.ToString(), fValue.CenterOfGravity.Round());
            }
            else
                InitView(false, value);
        }
        public void InitView(Boolean isFuzzy, Double value, String fuzzyString, String text, String shortText)
        {
            if (isFuzzy)
            {
                TriangularFuzzyNumber fValue = null;
                if (String.IsNullOrWhiteSpace(fuzzyString))
                {
                    fValue = value.ToFuzzy();
                    fuzzyString = fValue.ToString();
                }
                else
                {
                    try
                    {
                        TriangularFuzzyNumber.TryParse(fuzzyString, out fValue);
                    }
                    catch (Exception ex)
                    {
                        fValue = value.ToFuzzy();
                    }
                }

                View.RenderFuzzyNumber(text, shortText, fuzzyString, (fValue == null ? "0" : fValue.CenterOfGravity.Round()));
            }
            else
            {
                if (Math.Floor(value) == Math.Round(value, 3))
                    View.RenderNumber(text, Math.Floor(value).ToString());
                else
                    View.RenderNumber(text, value.Round());
            }
        }
        public void InitView(Boolean isFuzzy, Double ranking, Double value, String fuzzyString, String text, String shortText)
        {
            if (isFuzzy)
            {
                TriangularFuzzyNumber fValue = null;
                if (String.IsNullOrWhiteSpace(fuzzyString))
                {
                    fValue = value.ToFuzzy();
                    fuzzyString = fValue.ToString();
                }
                else
                {
                    try
                    {
                        TriangularFuzzyNumber.TryParse(fuzzyString, out fValue);
                    }
                    catch (Exception ex)
                    {
                        fValue = value.ToFuzzy();
                    }
                }
                View.RenderRankingFuzzyNumber(text, shortText, ranking.ToString(), fuzzyString, (fValue == null ? "0" : fValue.CenterOfGravity.Round()));
            }
            else
            {
                if (Math.Floor(value) == Math.Round(value, 3))
                    View.RenderNumber(text, Math.Floor(value).ToString());
                else
                    View.RenderNumber(text, value.Round());
            }
        }
    }
}