using System;
using System.Linq;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable()]
    public class VersionPermission : DomainBaseObjectLiteMetaInfo<long> 
    {
        public virtual Boolean Clone {get;set;}
        public virtual Boolean Edit { get; set; }
        public virtual Boolean See { get; set; }
        public virtual Boolean ChangePermission { get; set; }
        public virtual PermissionType Type { get; set; }
        public virtual TemplateDefinitionVersion Version { get; set; }
        public virtual TemplateDefinition Template { get; set; }
        public virtual Boolean ToApply { get; set; }

        public virtual VersionPermission Copy(TemplateDefinition template,TemplateDefinitionVersion version, litePerson person, String ipAddrees, String ipProxyAddress)
        {
            VersionPermission n = new VersionPermission();
            n.CreateMetaInfo(person, ipAddrees, ipProxyAddress);
            n.Version = version;
            n.Clone = Clone;
            n.Edit = Edit;
            n.ChangePermission = ChangePermission;
            n.Type = Type;
            n.Template = template;
            n.ToApply = ToApply;
            return n;
        }
    }
}