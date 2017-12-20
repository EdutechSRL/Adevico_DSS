using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class Template : lm.Comol.Core.DomainModel.DomainBaseObjectMetaInfo<Int64>
    {
        public virtual String Name { get; set; }
        public virtual TemplateType Type { get; set; }

        public virtual Boolean IsActive { get; set; }
        public virtual Boolean IsSystem { get; set; }
        public virtual Boolean HasDraft { get; set; }
        public virtual Boolean HasActive { get; set; }
        public virtual Boolean HasDefinitive { get; set; }

        public virtual IList<TemplateVersion> Versions { get; set; }

        //public virtual TemplateVersion WorkingRevision { get; set; }
        public virtual void UpdateInfo()
        {
            HasDraft = false;
            HasActive = false;
            HasDefinitive = false;

            if (Versions != null && Versions.Count() > 0)
            {
                foreach (TemplateVersion vers in Versions)
                {
                    if (vers.IsDraft)
                        HasDraft = true;
                    else
                    {
                        HasDefinitive = true;
                        if(vers.IsActive)
                            HasActive = true;
                    }
                }
            }
        }

        public virtual IList<ServiceContent> Services { get; set; }
    }
}
