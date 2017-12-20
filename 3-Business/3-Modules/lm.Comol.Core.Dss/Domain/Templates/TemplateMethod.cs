using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class TemplateMethod : DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual ItemObjectTranslation DefaultTranslation { get; set; }
        public virtual AlgorithmType Type { get; set; }
        public virtual Boolean BuiltIn { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public virtual Boolean UseManualWeights { get; set; }
        
        public virtual IList<DssTemplateTranslation> Translations { get; set; }
        public virtual RatingType Rating { get; set; }

        /// <summary>
        /// display rating also if evalutations not ended
        /// </summary>
        public virtual Boolean DisplayPartialRating { get; set; }
        /// <summary>
        /// display rating for single completed evaluation
        /// </summary>
        public virtual Boolean DisplaySingleRating { get; set; }
        /// <summary>
        /// display rating for single group completed evaluations
        /// </summary>
        public virtual Boolean DisplaySingleGroupRating { get; set; }
        
        public TemplateMethod()
        {
            DefaultTranslation = new ItemObjectTranslation();
            Translations = new List<DssTemplateTranslation>();
            Rating = RatingType.simple | RatingType.intermediateValues;
        }
        public ItemObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            ItemObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null)
                translation = DefaultTranslation;
            return translation;
        }
        public ItemObjectTranslation GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            ItemObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
            return translation;
        }
        //public virtual TemplateMethod Copy(TemplateMethod method, Int32 idPerson, String ipAddress, String ipProxyAddress)
        //{
        //    TemplateMethod t = new TemplateMethod();
        //    t.CreateMetaInfo(idPerson, ipAddress, ipProxyAddress);

        //    t.Type = Type;
        //    t.BuiltIn = BuiltIn;
        //    t.IsFuzzy = IsFuzzy;
        //    t.DefaultTranslation = DefaultTranslation.Copy();

        //    return t;
        //}
    }
}