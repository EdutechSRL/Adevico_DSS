using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class RepositoryIdentifier
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual RepositoryType Type { get; set; }
        public virtual Int32 IdPerson { get; set; }

        public String ToString()
        {
            return ToString("-");
        }

        public String ToString(String separator)
        {
            return IdCommunity.ToString() + separator + Type.ToString() + separator + IdPerson.ToString();
        }
        public static RepositoryIdentifier Create(RepositoryType type, Int32 idCommunity)
        {
            return new RepositoryIdentifier(){ Type= type,  IdCommunity=(type == RepositoryType.Community ? idCommunity : 0)};
        }
        public Boolean IsValid()
        {
            return (Type == RepositoryType.Portal || (Type == RepositoryType.Community && IdCommunity > 0) );
        }
    }
}