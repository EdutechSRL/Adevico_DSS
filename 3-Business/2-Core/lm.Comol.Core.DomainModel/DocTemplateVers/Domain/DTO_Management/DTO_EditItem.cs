using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    [Serializable]
    public class DTO_EditItem<T>
    {
        public DTO_EditItem() {}
        public DTO_EditItem(T NewData) 
        {
            Data = NewData;
        }

        public virtual DTO_EditElementPermission Permissions { get; set; }
        public virtual T Data { get; set; }
        public virtual IList<DTO_EditPreviousVersion> PreviousVersion { get; set; }
    }
}
