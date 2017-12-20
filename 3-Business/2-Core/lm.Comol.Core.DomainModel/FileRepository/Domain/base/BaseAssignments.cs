using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class BaseAssignments : lm.Comol.Core.DomainModel.DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual long IdItem { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual AssignmentType Type { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }

        public BaseAssignments()
        {
            Repository = new RepositoryIdentifier();
        }
    }
}