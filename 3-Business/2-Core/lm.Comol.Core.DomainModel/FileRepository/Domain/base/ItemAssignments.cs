using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class ItemAssignments : BaseAssignments 
    {
        public virtual long Permissions { get; set; }
        public virtual Boolean Denyed { get; set; }
        public virtual Boolean Inherited { get; set; }


        public ItemAssignments(){

        }
        public ItemAssignments(lm.Comol.Core.DomainModel.litePerson person, String ipAddress, String proxyIpAddress, DateTime? date = null)
        {
            CreateMetaInfo(person, ipAddress, proxyIpAddress, date);
        }


        public virtual Boolean isSettingsEqual(ItemAssignments other, Boolean ignoreDeny = false)
        {
            return IdCommunity == other.IdCommunity && other.IdPerson == IdPerson && other.IdRole == IdRole && other.Type == Type && (ignoreDeny || other.Denyed == Denyed);
        }
    }
}