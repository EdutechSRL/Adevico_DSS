using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class dtoSelectMethod : DomainBaseObject<long>
    {
        public virtual ItemObjectTranslation Translation { get; set; }
        public virtual AlgorithmType Type { get; set; }
        public virtual Boolean BuiltIn { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean UseManualWeights { get; set; }
        public virtual List<dtoSelectRatingSet> RatingSets { get; set; }
        public virtual RatingType Rating { get; set; }
        public virtual String Name { get { return (Translation == null ? "" : Translation.Name); } }
        /// <summary>
        /// display rating also if evalutations not ended
        /// </summary>
        public virtual Boolean DisplayPartialRating { get; set; }
        /// <summary>
        /// display rating for single completed evaluation
        /// </summary>
        public virtual Boolean DisplaySingleRating { get; set; }
        public dtoSelectMethod()
        {
            Translation = new ItemObjectTranslation();
            RatingSets = new List<dtoSelectRatingSet>();
        }

        public static dtoSelectMethod Create(TemplateMethod method,List<dtoSelectRatingSet> sets, Int32 idLanguage, Int32 idDefaultLanguage)
        {
            dtoSelectMethod dto = new dtoSelectMethod();
            dto.Translation = method.GetTranslation(idLanguage,idDefaultLanguage);
            dto.Type = method.Type;
            dto.Id = method.Id;
            dto.Deleted = method.Deleted;
            dto.BuiltIn = method.BuiltIn;
            dto.IsFuzzy = method.IsFuzzy;
            dto.Rating = method.Rating;
            dto.DisplayPartialRating = method.DisplayPartialRating;
            dto.DisplaySingleRating = method.DisplaySingleRating;
            dto.UseManualWeights = method.UseManualWeights;
            if (sets!=null && sets.Any()){
                if (sets.Any(s => s.OnlyForAlgorithm == method.Type && s.IsFuzzy == method.IsFuzzy))
                    dto.RatingSets = sets.Where(s => s.OnlyForAlgorithm == method.Type && s.IsFuzzy == method.IsFuzzy).Select(s => s.Copy(method.Id)).ToList();
                else
                    dto.RatingSets = sets.Where(s => (s.OnlyForAlgorithm == AlgorithmType.none && s.IsFuzzy == method.IsFuzzy)).Select(s => s.Copy(method.Id)).ToList();
            }
            return dto;
        }
        public Boolean IsValid()
        {
            return RatingSets != null && RatingSets.Any(r=> r.IsValid());
        }
        public String ToString()
        {
            return String.Format("Id: {0} Name: {1}  Name: {2} Type: {3} IsFuzzy:{4} Rating:{5}",
                Id, Translation.Name, Type.ToString(), IsFuzzy, Rating.ToString());
        }
    }
}