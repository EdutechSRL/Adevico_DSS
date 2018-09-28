using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class litePmActivityLink
    {
        public virtual long Id { get; set; }
        public virtual long IdProject { get; set; }
        public virtual litePmActivity Source { get; set; }
        public virtual litePmActivity Target { get; set; }
        public virtual PmActivityLinkType Type { get; set; }
        public virtual Double LeadLag { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual Boolean isVirtual { get; set; }

        public litePmActivityLink()
        {
        }
        public litePmActivityLink(litePmActivity source, ParsedActivityLink pal, litePmActivity target)
        {
            try
            {
                Source = source;
                Target = target;
                Type = pal.LinkType;
                LeadLag = pal.LeadLag;

                Source.Predecessors.Add(this);
                Target.Successors.Add(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public litePmActivityLink(litePmActivity source, ParsedActivityLink pal, Dictionary<Int64, litePmActivity> dict)
        {
            try
            {
                Source = source;
                Target = dict[pal.Id];
                Type = pal.LinkType;
                LeadLag = pal.LeadLag;

                Source.Predecessors.Add(this);
                Target.Successors.Add(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public litePmActivityLink(Int64 Id, ParsedActivityLink pal, Dictionary<Int64, litePmActivity> dict)
        {
            try
            {
                Source = dict[Id];
                Target = dict[pal.Id];
                Type = pal.LinkType;
                LeadLag = pal.LeadLag;

                Source.Predecessors.Add(this);
                Target.Successors.Add(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override string ToString()
        {
            return String.Format("{0}->{1} [{2}] {3}",
                this.Source.Id,
                this.Target.Id,
                this.LeadLag > 0 ? (this.LeadLag != 0 ? "+" + this.LeadLag.ToString() : "") : this.LeadLag.ToString(),
                this.Type
                );
        }
    }
}
