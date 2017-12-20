using System;
using System.Linq;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable]
    public class LanguageItem: BaseLanguageItem 
    {
        public virtual Boolean IsSelected { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public virtual ItemStatus Status { get; set; }

        public LanguageItem()
        {
            DisplayAs = ItemDisplayAs.item;
            Status = ItemStatus.none;
            IsEnabled = true;
        }

        public LanguageItem(BaseLanguageItem item)
        {
            Id = item.Id;
            Code = item.Code;
            IsDefault = item.IsDefault;
            IsUserDefined = item.IsUserDefined;
            IsMultiLanguage = item.IsMultiLanguage;
            IsEnabled = true;
            Name = item.Name;
            ToolTip = item.ToolTip;
            DisplayAs = item.DisplayAs;
            Status = ItemStatus.none;
        }
    }
}