using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Repository
{
    [Serializable]
    public class dtoCSVfile
    {
        public virtual Guid Id { get; set; }
        public virtual String RealName { get; set; }
        public virtual String Name { get; set; }
        public virtual long Size { get; set; }
        public virtual DateTime UploadedOn { get; set; }
    }
}