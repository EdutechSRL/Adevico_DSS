
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Communities;
namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable(), CLSCompliant(true)]
    public class dtoNewProfileSubscription
    {
        public Int32 Id { get { return (Node == null) ? 0 : Node.Community.Id; } }
        public String Name { get { return (Node == null) ? "" : Node.Community.Name; } }
        public Int32 IdCommunityType { get { return (Node == null) ? -4 : Node.Community.IdType; } }
        public Int32 IdRole { get; set; }
        public String RoleName { get; set; }
        public Int32 IdPreviousRole { get; set; }
        public lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem Node { get; set; }
        public dtoNewProfileSubscription()
        {

        }
    }
}