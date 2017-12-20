using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Domain
{
    [Serializable]
    public class dtoEditTileItem : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual String ToolTip { get; set; }
        public virtual List<dtoToolTipLanguageItem> Translations { get; set; }
        public virtual TileItemType Type { get; set; }
        public virtual String NavigateUrl { get; set; }
        public virtual String CssClass { get; set; }
                  
        public virtual long IdModulePage { get; set; }
        public virtual Boolean HasModulePage { get { return String.IsNullOrEmpty(NavigateUrl) && (IdModulePage > 0); } }

        public dtoEditTileItem()
        {
            Translations = new List<dtoToolTipLanguageItem>();
        }

        public dtoEditTileItem(liteTileItem item, List<Language> languages)
        {
            Id = item.Id;
            Deleted = item.Deleted;
            CssClass = item.CssClass;
            ToolTip = item.ToolTip;
            NavigateUrl = item.NavigateUrl;
            //IdModulePage = item.IdModulePage;

            Translations = languages.Select(l=> new dtoToolTipLanguageItem() { 
                            IdLanguage= l.Id,
                            IsMultiLanguage= false,
                            LanguageCode= l.Code,
                            ToolTip= item.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage== l.Id).Select(t=> t.ToolTip).FirstOrDefault()
                        }
                        ).ToList();
            Type = item.Type;
        }
    }

}