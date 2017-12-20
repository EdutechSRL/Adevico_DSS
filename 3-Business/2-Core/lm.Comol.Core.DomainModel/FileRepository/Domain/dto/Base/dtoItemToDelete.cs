using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoItemToDelete : dtoBaseItem
    {
        public virtual Boolean IsDeleted {get;set;}
        public virtual Boolean IsDeleteAllowed { get { return (LinkedModules==null || !LinkedModules.Any()) && IsDeleteAllowedFromCascade; } }
        public virtual Boolean IsDeleteAllowedFromCascade { get; set; }
        public virtual Boolean IsAddedForCascade { get; set; }
        public virtual long IdCascadeFirstFather { get; set; }
        public virtual Boolean HasCascadeItems { get; set; }

        public virtual List<String> LinkedModules { get; set; }

        public dtoItemToDelete()
        {
            LinkedModules = new List<string>();
            IsDeleteAllowedFromCascade = true;
        }
        public dtoItemToDelete(liteRepositoryItem item) : base(item)
        {
            LinkedModules = new List<string>();
            IsDeleteAllowedFromCascade = true;
        }
    }
}