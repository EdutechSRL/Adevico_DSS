using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Repository
{
    public class MultimediaFileIndex : DomainObject<long>
    {
        public virtual MultimediaFileTransfer MultimediaFile { get; set; }
        public virtual String Fullname { get; set; }
        public virtual Boolean IsDefaultDocument { get; set; }
        public virtual Single Probability { get; set; }
    }
}