using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class ItemWeightSettings
    {
        public virtual long IdRatingValue { get; set; }
        public virtual long IdRatingValueEnd { get; set; }
        public virtual Double Weight { get; set; }
        public virtual String WeightFuzzy { get; set; }
        public virtual Boolean IsFuzzyWeight { get; set; }
        public virtual RatingType RatingType { get; set; }
        public virtual String FuzzyMeWeights { get; set; }
        public virtual Boolean ManualWeights { get; set; }
        public virtual Boolean IsValidFuzzyMeWeights { get; set; }

        public ItemWeightSettings Copy()
        {
            ItemWeightSettings dto = new ItemWeightSettings();
            dto.IdRatingValue = IdRatingValue;
            dto.IdRatingValueEnd = IdRatingValueEnd;
            dto.Weight = Weight;
            dto.WeightFuzzy = WeightFuzzy;
            dto.IsFuzzyWeight = IsFuzzyWeight;
            dto.RatingType = RatingType;
            dto.FuzzyMeWeights = FuzzyMeWeights;
            dto.ManualWeights = ManualWeights;
            dto.IsValidFuzzyMeWeights = IsValidFuzzyMeWeights;
            return dto;
        }
    }
}
