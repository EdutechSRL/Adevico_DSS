using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Dss.Domain.Templates
{
     [Serializable]
    public class dtoItemWeightSettings : ItemWeightSettings
    {
        public virtual Boolean UseFatherManualWeights { get; set; }
        public virtual DssError Error { get; set; }
        public dtoItemWeightSettings()
        {
            Error = DssError.None;
        }
        public static dtoItemWeightSettings Create(ItemWeightSettings item, Boolean useFatherManualWeights)
        {
            DssError error =  DssError.None;
            if (item.ManualWeights)
            {
                if (String.IsNullOrWhiteSpace(item.FuzzyMeWeights))
                    error = DssError.MissingManualWeight;
                else if (!item.IsValidFuzzyMeWeights)
                    error = DssError.InvalidManualWeight;
            }
            else if (!useFatherManualWeights)
            {
                if (item.IdRatingValue < 1)
                    error = DssError.MissingWeight;
                else if (item.IdRatingValueEnd < 1 && item.RatingType != RatingType.simple)
                    error = DssError.MissingWeight;
            }
            return Create(item, useFatherManualWeights,error);
        }
        public static dtoItemWeightSettings Create(ItemWeightSettings item, Boolean useFatherManualWeights, DssError error )
        {
            dtoItemWeightSettings dto = new dtoItemWeightSettings();
            dto.IdRatingValue = item.IdRatingValue;
            dto.IdRatingValueEnd = item.IdRatingValueEnd;
            dto.Weight = item.Weight;
            dto.WeightFuzzy = item.WeightFuzzy;
            dto.IsFuzzyWeight = item.IsFuzzyWeight;
            dto.RatingType = item.RatingType;
            dto.FuzzyMeWeights = item.FuzzyMeWeights;
            dto.ManualWeights = item.ManualWeights;
            dto.IsValidFuzzyMeWeights = item.IsValidFuzzyMeWeights;
            dto.UseFatherManualWeights = useFatherManualWeights;
            dto.Error = error;

            return dto;
        }
        public dtoItemWeightSettings Copy()
        {
            dtoItemWeightSettings dto = new dtoItemWeightSettings();
            dto.IdRatingValue = IdRatingValue;
            dto.IdRatingValueEnd = IdRatingValueEnd;
            dto.Weight = Weight;
            dto.WeightFuzzy = WeightFuzzy;
            dto.IsFuzzyWeight = IsFuzzyWeight;
            dto.RatingType = RatingType;
            dto.FuzzyMeWeights = FuzzyMeWeights;
            dto.ManualWeights = ManualWeights;
            dto.IsValidFuzzyMeWeights = IsValidFuzzyMeWeights;
            dto.UseFatherManualWeights = UseFatherManualWeights;
            dto.Error = Error;
            return dto;
        }
    }
}