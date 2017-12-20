using System;
using System.Linq;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable]
    public class BaseLanguageItem
    {
        public virtual Int32 Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Code { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsUserDefined { get; set; }
        public virtual Boolean IsMultiLanguage { get; set; }
        public virtual Boolean IsSelected { get; set; }
        public virtual String ShortCode { get { return (IsMultiLanguage) ? Code : (String.IsNullOrEmpty(Code) ? "" : (Code.Contains("-") ? Code.Split('-').FirstOrDefault().ToUpper() : Code.ToUpper())); } }
        public virtual ItemDisplayAs DisplayAs { get; set; }
        public BaseLanguageItem()
        {
            DisplayAs = ItemDisplayAs.item;
        }
        public BaseLanguageItem(Language language)
        {
            Id = language.Id;
            Name = language.Name;
            Code = language.Code;
            ToolTip = language.Name;
            IsDefault = language.isDefault;
            DisplayAs = ItemDisplayAs.item;
        }
    }
}