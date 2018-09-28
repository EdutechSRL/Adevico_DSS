using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class _BaseItem : DomainBaseObject<long>{
        public virtual long DisplayOrder { get; set; }         //*
        public virtual string CssClass { get; set; }    //255  //* 
        public virtual Boolean IsEnabled { get; set; }      //*
        public virtual Boolean ShowDisabledItems { get; set; }     //*
        public virtual String Link { get; set; }    //255   //*
        public virtual String Name { get; set; }    //150       //*
        public virtual IList<_ItemTranslation> Translations { get; set; }    //*
        public virtual TextPosition TextPosition { get; set; }             //*
        public virtual int IdModule { get; set; }          //*
        public virtual String ModuleCode { get; set; } 
        public virtual long Permission { get; set; }       //*
        public virtual IList<_ProfileAssignment> ProfileAvailability { get; set; }
        public virtual long IdMenubar { get; set; }


        protected virtual _ItemTranslation Translate(int IdLanguage)
        {
            if (Translations.Count == 0 || !Translations.Where(t => t.Language.Id == IdLanguage).Any())
                return new _ItemTranslation() { Name = Name, ToolTip = Name };
            else
                return Translations.Where(t => t.Language.Id == IdLanguage).FirstOrDefault();
        }

        protected virtual Boolean IsValid(int IdProfileType)
        {
            if (ProfileAvailability == null && ProfileAvailability.Count == 0)
                return false;
            else
                return (from p in ProfileAvailability where p.IdProfileType == IdProfileType && p.Deleted == BaseStatusDeleted.None select p.Id).Any();
        }
        protected virtual Boolean IsValid(List<CommunityRoleModulePermission> permissions)
        {
            if (IdModule==0)
                return true;

            Boolean result = (from p in permissions where p.Service.Available && p.Service.Id == IdModule && PermissionHelper.CheckPermissionSoft(Permission, p.PermissionInt) select p).Any();


            return  result;

        }
        protected virtual String ItemToString(int IdLanguage, String Baseurl, Boolean available)
        {
            String itemText = "<a href=\"#\" title=\"{0}\">{1}</a>\n\r";
            String itemUrl = "<a href=\"{0}\" title=\"{1}\" >{2}</a>\n\r";

            _ItemTranslation translation = Translate(IdLanguage);
            if (string.IsNullOrEmpty(Link) || (!available && ShowDisabledItems))
                return String.Format(itemText, translation.ToolTip, translation.Name);
            else
                return String.Format(itemUrl, Baseurl + Link, translation.ToolTip, translation.Name);
        }
    }
}