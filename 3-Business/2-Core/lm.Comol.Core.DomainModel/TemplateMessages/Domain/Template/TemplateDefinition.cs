using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class TemplateDefinition : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual TemplateOwner OwnerInfo { get; set; }
        public virtual TemplateType Type { get; set; }
        public virtual Boolean IsEnabled {get;set;}
        public virtual String CurrentModulesContent { get; set; }
        public virtual TemplateDefinitionVersion LastVersion { get; set; }
        public virtual IList<TemplateDefinitionVersion> Versions { get; set; }
        //public virtual IList<VersionPermission> ActivePermissions { get; set; }

        public TemplateDefinition()
        {
            Type = TemplateType.None;
            IsEnabled = true;
            OwnerInfo = new TemplateOwner();
            Versions = new List<TemplateDefinitionVersion>();
            //ActivePermissions = new List<VersionPermission>();
        }

        public TemplateDefinitionVersion GetActiveVersion()
        {
            return (Versions == null) ? null : Versions.Where(v => v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Active).OrderByDescending(v=>v.ModifiedBy).FirstOrDefault();
        }

        public virtual TemplateDefinition Copy(String namePrefix, litePerson person, String ipAddrees, String ipProxyAddress)
        {
            TemplateDefinition tv = new TemplateDefinition();
            tv.CreateMetaInfo(person, ipAddrees, ipProxyAddress);

            tv.Name = namePrefix + Name;
            tv.Type = Type;
            tv.IsEnabled = IsEnabled;
            tv.CurrentModulesContent = CurrentModulesContent;
            if (Versions != null)
            {
                Int32 index = 1;
                foreach (TemplateDefinitionVersion v in Versions.Where(t => t.Deleted == BaseStatusDeleted.None).ToList())
                {
                    tv.Versions.Add(v.Copy(tv, person, ipAddrees, ipProxyAddress, namePrefix));
                    tv.Versions.Last().Number = index++;
                }
                tv.LastVersion = tv.Versions.Where(v => v.Status == TemplateStatus.Active).OrderByDescending(v => v.Id).FirstOrDefault();
                if (tv.LastVersion==null)
                    tv.LastVersion = tv.Versions.OrderByDescending(v => v.Id).FirstOrDefault();
            }
            return tv;
        }
    }
}