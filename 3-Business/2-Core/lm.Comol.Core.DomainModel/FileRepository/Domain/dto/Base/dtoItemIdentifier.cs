using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoItemIdentifier : ICloneable 
    {
        public virtual long IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual long IdObject { get; set; }
        public virtual Int32 IdObjectType { get; set; }
        public virtual long IdTag { get; set; }
        public virtual String Tag { get; set; }
        public virtual Boolean IsSpecialFolder { get { return FolderType != Domain.FolderType.none && FolderType != Domain.FolderType.standard; } }
        public virtual ItemIdentifierType Type { get; set; }
        public virtual FolderType FolderType { get; set; }
        public dtoItemIdentifier()
        {
            Type = ItemIdentifierType.standard;
            FolderType = Domain.FolderType.standard;
        }

        public object Clone()
        {
            dtoItemIdentifier clone = new dtoItemIdentifier();
            clone.IdModule = IdModule;
            clone.ModuleCode = ModuleCode;
            clone.IdObject = IdObjectType;
            clone.IdTag = IdTag;
            clone.Tag = Tag;
            clone.Type = Type;
            clone.FolderType = FolderType;
            return clone;
        }
    }
  }