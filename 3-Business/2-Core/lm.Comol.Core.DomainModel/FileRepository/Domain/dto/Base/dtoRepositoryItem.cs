using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoRepositoryItem : dtoBaseRepositoryItem
    {
        public virtual dtoRepositoryItem Father { get; set; }
        public virtual List<dtoRepositoryItem> Children { get; set; }

        #region "Folder"
            public virtual Boolean IsEmpty { get { return IsFile || Children == null || !Children.Any(); } }
        #endregion

        public dtoRepositoryItem()
        {
            Children = new List<dtoRepositoryItem>();
        }


        public dtoRepositoryItem(liteRepositoryItem item, dtoRepositoryItem father = null, lm.Comol.Core.DomainModel.litePerson owner = null, lm.Comol.Core.DomainModel.litePerson modifiedBy = null, String unknownUser = "")
            : base(item, owner,modifiedBy, unknownUser)
        {
            Children = new List<dtoRepositoryItem>();
            Father = father;
        }

        public List<long> GetAllId()
        {
            List<long> idItems = new List<long>() { Id};
            if (!IsFile)
                Children.ForEach(c => idItems.AddRange(c.GetAllId()));
            return idItems;
        }
    }
}