using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class dtoSelectRatingSet : DomainBaseObject<long>
    {
        public virtual long IdMethod { get; set; }
        public virtual ItemObjectTranslation Translation { get; set; }
        public virtual String Name { get { return (Translation == null ? "" : Translation.Name); } }
        public virtual List<dtoSelectRatingValue> Values { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean IsForWeights { get; set; }
        
        public virtual AlgorithmType OnlyForAlgorithm { get; set; }
        public virtual Boolean IsDefault { get; set; }

        public dtoSelectRatingSet()
        {
            Translation = new ItemObjectTranslation();
            Values = new List<dtoSelectRatingValue>();
        }

        public static dtoSelectRatingSet Create(TemplateRatingSet ratingSet, Int32 idLanguage, Int32 idDefaultLanguage)
        {
            dtoSelectRatingSet dto = new dtoSelectRatingSet();
            dto.Id = ratingSet.Id;
            dto.Deleted = ratingSet.Deleted;
            dto.Translation = ratingSet.GetTranslation(idLanguage, idDefaultLanguage);
            dto.IsFuzzy = ratingSet.IsFuzzy;
            dto.IsForWeights = ratingSet.IsForWeights;
            dto.OnlyForAlgorithm = ratingSet.OnlyForAlgorithm;
            dto.IsDefault = ratingSet.IsDefault;
            if (ratingSet.Values !=null)
                dto.Values = ratingSet.Values.Where(v => v.Deleted == BaseStatusDeleted.None).Select(s => dtoSelectRatingValue.Create(ratingSet.Id,s, idLanguage, idDefaultLanguage)).ToList();
            return dto;
        }
        public dtoSelectRatingSet Copy(long idMethod)
        {
            dtoSelectRatingSet dto = new dtoSelectRatingSet();
            dto.Id = Id;
            dto.Deleted = Deleted;
            dto.Translation = Translation;
            dto.IsFuzzy = IsFuzzy;
            dto.IsForWeights = IsForWeights;
            dto.OnlyForAlgorithm = OnlyForAlgorithm;
            dto.IdMethod = idMethod;
            if (Values != null)
                dto.Values = Values.Select(v => v.Copy(idMethod, Id)).ToList();
            return dto;
        }
        public Boolean IsValid()
        {
            return Values != null && Values.Any();
        }
        public String ToString()
        {
            return String.Format("Id: {0} IdMethod: {1}  Name: {2} IsFuzzy: {3} OnlyForAlgorithm:{4}",
                Id, IdMethod, Translation.Name, IsFuzzy, OnlyForAlgorithm.ToString());
        }
    }
}