using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class ItemMethodSettings
    {
        public virtual long IdMethod { get; set; }
        public virtual long IdRatingSet { get; set; }
        public virtual Boolean IsFuzzyMethod { get; set; }
        public virtual Boolean UseManualWeights { get; set; }
        public virtual Boolean UseOrderedWeights { get; set; }
        public virtual Boolean InheritsFromFather { get; set; }

        public ItemMethodSettings Copy()
        {
            ItemMethodSettings dto = new ItemMethodSettings();
            dto.IdMethod = IdMethod;
            dto.IdRatingSet = IdRatingSet;
            dto.InheritsFromFather = InheritsFromFather;
            dto.IsFuzzyMethod = IsFuzzyMethod;
            dto.UseManualWeights = UseManualWeights;
            return dto;
        }
    }
}