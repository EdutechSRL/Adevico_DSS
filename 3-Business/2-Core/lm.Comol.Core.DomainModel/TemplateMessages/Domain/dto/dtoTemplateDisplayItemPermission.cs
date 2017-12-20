using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable()]
    public class dtoTemplateDisplayItemPermission
    {
        public virtual long Id { get {return Template.Id;} }
        public virtual BaseStatusDeleted Deleted {  get {return Template.Deleted;} }
        public virtual dtoTemplatePermission Permission { get; set; }
        public virtual dtoDisplayTemplateDefinition Template { get; set; }

        public dtoTemplateDisplayItemPermission()
        {
            Permission = new dtoTemplatePermission();
        }

        public dtoTemplateDisplayItemPermission(dtoDisplayTemplateDefinition t)
        {
            Template = t;
        }
    }
}