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

namespace lm.Comol.Core.Dss.Presentation.Evaluation
{
    public class FuzzyInputPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewFuzzyInput View
            {
                get { return (IViewFuzzyInput)base.View; }
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
            public FuzzyInputPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public FuzzyInputPresenter(iApplicationContext oContext, IViewFuzzyInput view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            //public void InitView(dtoSelectMethod method, Boolean disabled)
            //{
            //    BaseInitialize(method, disabled, (method., RatingType.simple);
            //}
        
        public void InitView(dtoSelectMethod method,dtoSelectRatingSet ratingSet, dtoItemWeightSettings objectSettings, Boolean disabled)
        {
            RatingType type = objectSettings.RatingType;
            if (type == RatingType.none)
                type = RatingType.simple;
            BaseInitialize(method, disabled, ratingSet, type, objectSettings.IdRatingValue, objectSettings.IdRatingValueEnd);
        }
        public void InitView(dtoSelectMethod method, dtoItemWeightSettings objectSettings, Boolean disabled)
        {
            long idRatingSet =Service.RatingSetGetIdByValue(objectSettings.IdRatingValue);
            RatingType type = objectSettings.RatingType;
            if (type == RatingType.none)
                type = RatingType.simple;
            BaseInitialize(method, disabled, idRatingSet, type, objectSettings.IdRatingValue, objectSettings.IdRatingValueEnd);
        }
        public void InitView(dtoSelectMethod method, long idRatingSet, Boolean disabled)
        {
            BaseInitialize(method, disabled, idRatingSet, RatingType.simple);
        }

        public void InitView(long idMethod, long idRatingSet, Boolean disabled)
        {
            dtoSelectMethod method = Service.MethodGetAvailable(idMethod,UserContext.Language.Id);
            BaseInitialize(method, disabled, idRatingSet, RatingType.simple);
        }
        public void InitView(long idMethod, dtoSelectRatingSet ratingSet, Boolean disabled)
        {
            dtoSelectMethod method = Service.MethodGetAvailable(idMethod, UserContext.Language.Id);
            BaseInitialize(method, disabled, ratingSet, RatingType.simple);
        }
        private void BaseInitialize(dtoSelectMethod method, Boolean disabled,long idRatingSet, RatingType type,long idRatingValue = -1, long idRatingValueEnd = -1)
        {
            BaseInitialize(method,disabled,  method.RatingSets.Where(s=> s.Id== idRatingSet).FirstOrDefault(),type,idRatingValue,idRatingValueEnd);
        }
        private void BaseInitialize(dtoSelectMethod method, Boolean disabled, dtoSelectRatingSet ratingSet, RatingType type, long idRatingValue = -1, long idRatingValueEnd = -1)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Disabled = disabled;
                View.IsFuzzy = method.IsFuzzy;
                Dictionary<RatingType, String> translations = View.GetTranslations();
                View.IdMethod = method.Id;
                if (ratingSet == null)
                    View.IdRatingSet = 0;
                else
                    View.IdRatingSet = ratingSet.Id;
                List<dtoRatingType> types = new List<dtoRatingType>();
                if ((method.Rating & RatingType.simple) > 0)
                {
                    types.Add(new dtoRatingType() { Id = (int)RatingType.simple, Name = translations[RatingType.simple], Type = RatingType.simple });
                    if (type == RatingType.simple)
                        View.InitializeRating((idRatingValue>0),RatingType.simple, ratingSet, idRatingValue);
                    else
                        View.InitializeRating(false,RatingType.simple, ratingSet, -1);
                }
                if ((method.Rating & RatingType.extended) > 0)
                {
                    types.Add(new dtoRatingType() { Id = (int)RatingType.extended, Name = translations[RatingType.extended], Type = RatingType.extended });
                    if (type == RatingType.extended)
                        View.InitializeRating((idRatingValue > 0), RatingType.extended, ratingSet, idRatingValue, idRatingValueEnd);
                    else
                        View.InitializeRating(false, RatingType.extended, ratingSet, -1, -1);
                }
                if ((method.Rating & RatingType.intermediateValues) > 0)
                {
                    types.Add(new dtoRatingType() { Id = (int)RatingType.intermediateValues, Name = translations[RatingType.intermediateValues], Type = RatingType.intermediateValues });
                    if (type == RatingType.intermediateValues)
                        View.InitializeRating((idRatingValue > 0), RatingType.intermediateValues, ratingSet, idRatingValue, idRatingValueEnd);
                    else
                        View.InitializeRating(false, RatingType.intermediateValues, ratingSet, -1, -1);
                }
                View.LoadAvailableRatings(types, type);
                if (ratingSet == null)
                    View.Disabled = true;
            }
        }

        public dtoItemWeightSettings VerifySettings(dtoItemWeightSettings settings)
        {
            if (settings.Error == DssError.None)
            {
                switch (settings.RatingType)
                {
                    case RatingType.simple:
                        TemplateRatingValue item = Service.RatingValueGet(settings.IdRatingValue);
                        if (item !=null){
                            if (item.IsFuzzy)
                            {
                                TriangularFuzzyNumber fValue = null;
                                if (TriangularFuzzyNumber.TryParse(item.FuzzyValue, out fValue))
                                {
                                    settings.Weight = fValue.CenterOfGravity;
                                    settings.WeightFuzzy = item.FuzzyValue;
                                }
                                else
                                    settings.Error = DssError.InvalidWeight;
                            }
                            else
                                settings.Weight = item.Value;
                        }
                        else
                            settings.Error = DssError.InvalidWeight;
                        break;
                    case RatingType.intermediateValues:
                        TemplateRatingValue fromValue = Service.RatingValueGet(settings.IdRatingValue);
                        TemplateRatingValue toValue = Service.RatingValueGet(settings.IdRatingValueEnd);
                        if (fromValue != null && toValue != null)
                        {
                            if (fromValue.IsFuzzy)
                            {
                                try{
                                    TriangularFuzzyNumber fValue = TriangularFuzzyNumber.Intermediate(TriangularFuzzyNumber.Parse(fromValue.FuzzyValue), TriangularFuzzyNumber.Parse(toValue.FuzzyValue));
                                    settings.Weight = fValue.CenterOfGravity;
                                    settings.WeightFuzzy = fValue.ToString();
                                }
                                catch (Exception ex)
                                {
                                    settings.Error = DssError.InvalidWeight;
                                }
                            }
                            else
                                 settings.Weight = ((fromValue.Value + toValue.Value) /2);
                        }
                        else
                            settings.Error = DssError.InvalidWeight;
                        break;
                }
            }
            return settings;
        }
    }
}