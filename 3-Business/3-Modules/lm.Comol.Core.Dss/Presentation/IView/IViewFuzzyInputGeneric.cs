using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Domain.Templates;

namespace lm.Comol.Core.Dss.Presentation.Evaluation
{
    public interface IViewFuzzyInputGeneric : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long IdObjectItem { get; set; }
        long IdMethod { get; set; }
        long IdRatingSet { get; set; }
        Boolean RaiseEvents { get; set; }
        DssError CurrentError { get; set; }
        Boolean Disabled { get; set; }
        Boolean isValid { get; }
        RatingType CurrentRatingType { get; set; }
        Boolean IsFuzzy { get; set; }

        Dictionary<RatingType, String> GetTranslations();
        dtoItemWeightSettings GetSettings();
        dtoItemRating GetItemRating();
        void InitializeDisabledControl();

        void InitializeControl(long idMethod,dtoGenericRatingSet ratingSet, dtoItemWeightSettings objectSettings,  Boolean disabled = false, DssError err = DssError.None);
        void InitializeControl(long idMethod, dtoGenericRatingSet ratingSet, dtoItemRating rating, Boolean disabled = false, DssError err = DssError.None);
        void InitializeControl(long idMethod, dtoGenericRatingSet ratingSet, Boolean disabled = false, DssError err = DssError.None);

        void LoadAvailableRatings(List<dtoRatingType> availableTypes, RatingType currentType);
        void InitializeRating(Boolean isCurrent, RatingType type, dtoGenericRatingSet ratingSet, long idRatingValue, long idRatingValueEnd = -1);
        void DisplaySessionTimeout();
    }
}