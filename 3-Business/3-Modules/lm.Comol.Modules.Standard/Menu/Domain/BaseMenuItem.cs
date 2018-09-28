using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [CLSCompliant(true),Serializable]
    public class BaseMenuItem : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual long DisplayOrder { get; set; }         //*
        public virtual string CssClass { get; set; }    //255  //* 
        public virtual Boolean IsEnabled { get; set; }      //*
        public virtual Boolean ShowDisabledItems { get; set; }     //*
        public virtual String Link { get; set; }    //255   //*
        public virtual String Name { get; set; }    //150       //*
        public virtual IList<MenuItemTranslation> Translations { get; set; }    //*
        public virtual TextPosition TextPosition { get; set; }             //*
        public virtual int IdModule { get; set; }          //*
        public virtual String ModuleCode { get; set; } 
        public virtual long Permission { get; set; }       //*
        public virtual IList<ProfileAssignment> ProfileAvailability { get; set; }
        public virtual Menubar Menubar { get; set; }

    }
}