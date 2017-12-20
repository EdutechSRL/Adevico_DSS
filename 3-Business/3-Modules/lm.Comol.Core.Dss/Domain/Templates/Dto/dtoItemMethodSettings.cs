using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class dtoItemMethodSettings : ItemMethodSettings
    {
        public virtual Boolean UseFatherManualWeights { get; set; }
        public virtual String FuzzyMeWeights { get; set; }
        public virtual Boolean IsDefaultForChildren { get; set; }
        public virtual DssError Error { get; set; }
        
        public dtoItemMethodSettings()
        {
            Error = DssError.None;
            InheritsFromFather = true;
            IsDefaultForChildren = false;
        }
        public static dtoItemMethodSettings Create(ItemMethodSettings item, Boolean useFatherManualWeights)
        {
            DssError error = DssError.None;
            if (item.IdMethod < 1 && !item.InheritsFromFather)
                error = DssError.MissingMethod;
            if (item.IdRatingSet < 1 && !item.UseManualWeights)
                error = error | DssError.MissingRatingSet;
            return Create(item, useFatherManualWeights, false, error);
        }
        public static dtoItemMethodSettings Create(ItemMethodSettings item,Boolean useFatherManualWeights, Boolean isDefaultForChildren, DssError error = DssError.None )
        {
            dtoItemMethodSettings dto = new dtoItemMethodSettings();
            dto.IdMethod = item.IdMethod;
            dto.IdRatingSet = item.IdRatingSet;
            dto.InheritsFromFather = item.InheritsFromFather;
            dto.IsFuzzyMethod = item.IsFuzzyMethod;
            dto.UseManualWeights = item.UseManualWeights;
            dto.UseOrderedWeights = item.UseOrderedWeights;
            dto.UseFatherManualWeights = useFatherManualWeights;
            dto.IsDefaultForChildren = isDefaultForChildren;
            dto.Error = error;
            return dto;
        }
        public dtoItemMethodSettings Copy()
        {
            dtoItemMethodSettings dto = new dtoItemMethodSettings();
            dto.IdMethod = IdMethod;
            dto.IdRatingSet = IdRatingSet;
            dto.InheritsFromFather = InheritsFromFather;
            dto.IsFuzzyMethod = IsFuzzyMethod;
            dto.UseManualWeights = UseManualWeights;
            dto.IsDefaultForChildren = IsDefaultForChildren;
            dto.UseOrderedWeights = UseOrderedWeights;
            dto.FuzzyMeWeights = FuzzyMeWeights;
            dto.UseFatherManualWeights = UseFatherManualWeights;
            dto.Error = Error;
            return dto;
        }
    }
}