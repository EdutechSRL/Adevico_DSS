using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Editor
{
   [Serializable]
    public class EditorRepositoryItem : DomainBaseObjectMetaInfo<long>
    {
       public virtual EditorRepositoryItem Folder { get; set; }
       public virtual Int32 IdCommunity {get;set;}
       public virtual Int32 IdOwner { get; set; }
       //public virtual Community Community {get;set;}
       //public virtual EditorRepositoryItem Folder {get;set;}
       public virtual String Name {get;set;}
       public virtual String DisplayName { get { return Name + Extension; } }
       public virtual String Description {get;set;}
       public virtual String Extension {get;set;}
       public virtual String MimeType {get;set;}
       public virtual long Size { get; set; }
       public virtual Boolean IsDirectory {get;set;}
       public virtual Guid Identifyer {get;set;}
  
    }
}
