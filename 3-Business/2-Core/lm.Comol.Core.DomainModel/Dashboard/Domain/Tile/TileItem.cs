using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class TileItem : lm.Comol.Core.DomainModel.DomainBaseObjectLiteMetaInfo<long>, ITileBaseItem
    {
        public virtual Tile Tile {get;set;}
        public virtual IList<TileItemTranslation> Translations { get; set; }
        public virtual String NavigateUrl { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual String CssClass { get; set; }
        public virtual TileItemType Type { get; set; }
        // public virtual long IdModulePage { get; set; }
        public TileItem()
        {
            Translations = new List<TileItemTranslation>();
            Type = TileItemType.UserDefinedUrl;
        }

        public virtual String GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? ToolTip : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.ToolTip).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.ToolTip).FirstOrDefault();
            if (String.IsNullOrEmpty(translation))
                translation = ToolTip;
            return translation;
        }
        public virtual String GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            String translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? ToolTip : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.ToolTip).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.ToolTip).FirstOrDefault();
            if (String.IsNullOrEmpty(translation))
                translation = ToolTip;
            return translation;
        }
        public virtual String GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti, Boolean useFirstOccurence)
        {
            String translation = "";
            if (Translations == null || Translations.Any())
                translation = Translations.Where(t => t.IdLanguage == idUserLanguage).FirstOrDefault().ToolTip;
            if (String.IsNullOrEmpty(translation) && !String.IsNullOrEmpty(ToolTip) && firstIsMulti)
                translation = ToolTip;
            if (String.IsNullOrEmpty(translation) && Translations.Any())
                translation = Translations.Where(t => t.IdLanguage == idDefaultLanguage).FirstOrDefault().ToolTip;
            if (String.IsNullOrEmpty(translation) && Translations.Any() && useFirstOccurence)
                translation = Translations.FirstOrDefault().ToolTip;
            return (String.IsNullOrEmpty(translation)) ? Id.ToString() : translation;
        }

        public virtual TileItem Copy(Tile tile, litePerson person, String ipAddress, String proxyIpAddress, DateTime? createdOn = null)
        {
            TileItem clone = new TileItem() { CssClass = this.CssClass, ToolTip = this.ToolTip, Type = this.Type, NavigateUrl = this.NavigateUrl };
            clone.CreateMetaInfo(person, ipAddress, proxyIpAddress, createdOn);
            clone.Tile = tile;
            if (Translations != null)
                clone.Translations = Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => t.Copy(tile,clone, person, ipAddress, proxyIpAddress, clone.CreatedOn)).ToList();
            return clone;
        }
    }
}