using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Dss.Domain.Templates
{
     [Serializable]
    public class dtoItemRating : ItemRating
    {
        public virtual DssError Error { get; set; }
        public dtoItemRating()
        {
            Error = DssError.None;
        }
        public static dtoItemRating Create(ItemRating item)
        {
            DssError error =  DssError.None;
            if (item != null)
            {
                if (item.IdRatingValue < 1)
                    error = DssError.MissingRating;
                else if (item.IdRatingValueEnd < 1 && item.RatingType != RatingType.simple)
                    error = DssError.MissingRating;
                return Create(item, error);
            }
            else
            {
                return new dtoItemRating() { Error = DssError.MissingRating};
            }
            
        }
        public static dtoItemRating Create(ItemRating item, DssError error)
        {
            dtoItemRating dto = new dtoItemRating();
            dto.IdRatingValue = item.IdRatingValue;
            dto.IdRatingValueEnd = item.IdRatingValueEnd;
            dto.Value = item.Value;
            dto.ValueFuzzy = item.ValueFuzzy;
            dto.IsFuzzy = item.IsFuzzy;
            dto.RatingType = item.RatingType;
            dto.Error = error;

            return dto;
        }
        public dtoItemRating Copy()
        {
            dtoItemRating dto = new dtoItemRating();
            dto.IdRatingValue = IdRatingValue;
            dto.IdRatingValueEnd = IdRatingValueEnd;
            dto.Value = Value;
            dto.ValueFuzzy = ValueFuzzy;
            dto.IsFuzzy = IsFuzzy;
            dto.RatingType = RatingType;
            dto.Error = Error;
            return dto;
        }
    }
}