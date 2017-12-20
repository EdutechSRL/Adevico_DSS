using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.toRemove
{
    [Serializable]
    public class OldMultimediaObject
    {
        public virtual long Id { get; set; }
        public virtual long IdMultimediaFileTransfer { get; set; }
        public virtual String Fullname { get; set; }
        public virtual Boolean IsDefaultDocument { get; set; }
        public virtual Single Probability {get; set;}
        public virtual Boolean Transferred { get; set; }

    }
    [Serializable]
    public class NewMultimediaObject
    {
        public virtual long Id { get; set; }
        public virtual long IdItem { get; set; }
        public virtual System.Guid UniqueIdItem { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual long IdItemTransfer { get; set; }
        public virtual String Fullname { get; set; }
        public virtual Boolean IsDefaultDocument { get; set; }
        public virtual Single Probability { get; set; }
    }
}