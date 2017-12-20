using System;
using System.Linq;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable()]
    public class VersionRolePermission : VersionPermission
    {
        public virtual liteCommunity Community { get; set; }
        public virtual Role AssignedTo { get; set; }

        public virtual VersionRolePermission Copy(TemplateDefinition template, TemplateDefinitionVersion version, litePerson person, String ipAddrees, String ipProxyAddress)
        {
            VersionRolePermission n = new VersionRolePermission();
            n.CreateMetaInfo(person, ipAddrees, ipProxyAddress);
            n.Version = version;
            n.Clone = Clone;
            n.Edit = Edit;
            n.ChangePermission = ChangePermission;
            n.Type = Type;
            n.Template = template;
            n.ToApply = ToApply;
            n.AssignedTo = AssignedTo;
            n.Community = Community;
            return n;
        }
    }
}
