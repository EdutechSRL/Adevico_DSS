using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class ItemRating
    {
        public virtual long IdRatingValue { get; set; }
        public virtual long IdRatingValueEnd { get; set; }
        public virtual Double Value { get; set; }
        public virtual String ValueFuzzy { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual RatingType RatingType { get; set; }

        public ItemRating Copy()
        {
            ItemRating dto = new ItemRating();
            dto.IdRatingValue = IdRatingValue;
            dto.IdRatingValueEnd = IdRatingValueEnd;
            dto.Value = Value;
            dto.ValueFuzzy = ValueFuzzy;
            dto.IsFuzzy = IsFuzzy;
            dto.RatingType = RatingType;

            return dto;
        }

        public Boolean IsValid()
        {
            switch (RatingType)
            {
                case Domain.RatingType.simple:
                    return IdRatingValue > 0;
                case Domain.RatingType.extended:
                case Domain.RatingType.intermediateValues:
                    return IdRatingValue > 0 && IdRatingValueEnd > 0;
                default:
                    return false;
            }
        }
    }
}
