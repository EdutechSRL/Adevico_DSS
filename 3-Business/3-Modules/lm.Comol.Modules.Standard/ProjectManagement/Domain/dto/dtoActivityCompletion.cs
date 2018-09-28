using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
     [Serializable]
    public class dtoActivityCompletion
    {
        public virtual long Id { get; set; }
        public virtual long IdResource { get; set; }
        public virtual long IdPerson { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual int Completeness { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public virtual Boolean Deleted { get; set; }

        public virtual String UniqueId { get; set; }
        public dtoActivityCompletion()
        {
            UniqueId = Guid.NewGuid().ToString();
        }
    }
}