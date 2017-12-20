using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class dtoScormSettingsBase
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual ScormSettingsType Type { get; set; }
        public virtual String DataId { get { return Type.ToString() + Id.ToString(); } }
        public virtual String DataChildren { get; set; }
    }
}
