using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable]
    public class dtoUserEnrollments
    {
        public List<Int32> IdCommunities { get; set; }
        public Int32 IdPerson { get; set; }
        public litePerson Person { get; set; }
        public dtoUserEnrollments()
        {

        }
        public Boolean IsValid()
        {
            return (Person != null && Person.Id > 0);
        }
        public Boolean IsEnrolled(Int32 idCommunity)
        {
            return (IdCommunities!=null && IdCommunities.Contains(idCommunity));
        }
    }
}