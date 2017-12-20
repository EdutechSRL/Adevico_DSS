using System;
using System.Linq;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable()]
    public class VersionPersonPermission : VersionPermission
    {
        public virtual litePerson AssignedTo { get; set; }

        public virtual VersionPersonPermission Copy(TemplateDefinition template, TemplateDefinitionVersion version, litePerson person, String ipAddrees, String ipProxyAddress)
        {
            VersionPersonPermission n = new VersionPersonPermission();
            n.CreateMetaInfo(person, ipAddrees, ipProxyAddress);
            n.Version = version;
            n.Clone = Clone;
            n.Edit = Edit;
            n.ChangePermission = ChangePermission;
            n.Type = Type;
            n.Template = template;
            n.ToApply = ToApply;
            n.AssignedTo = AssignedTo;
            return n;
        }
    }
}