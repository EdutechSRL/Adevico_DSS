using lm.Comol.Core.DomainModel;
using System;

namespace lm.Comol.Core.DomainModel.Repository
{
    [Serializable(), CLSCompliant(true)]
    public class dtoBaseGenericFile
    {
        public virtual String Name { get; set; }
        public virtual String Extension { get; set; }
        public virtual String Fullname { get { return Name + Extension; } }
        public virtual long Size { get; set; }
        public virtual RepositoryItemType Type { get; set; }

        public dtoBaseGenericFile()
        {
            Type = RepositoryItemType.FileStandard;
        }
    }
}