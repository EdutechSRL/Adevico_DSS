using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoGenericAssignment
    {
        public int ItemID { get; set; }
        public String ItemName { get; set; }
        public RoleEP RoleEP { get; set; }
        public StatusAssignment Status { get; set; }
        public long DB_ID { get; set; }

        public dtoGenericAssignment()
        {
            ItemID = -1;
            ItemName = "";
            RoleEP = RoleEP.None;
            Status = StatusAssignment.None;
            DB_ID = 0;
        }
        public dtoGenericAssignment(int ItemID, String ItemName)
        {
            this.ItemID = ItemID;
            this.ItemName = ItemName;
            this.RoleEP = RoleEP.None;
            Status = StatusAssignment.None;
            this.DB_ID = 0;
        }
        public dtoGenericAssignment(int ItemID, String ItemName, RoleEP RoleEP)
        {
            this.ItemID = ItemID;
            this.ItemName = ItemName;
            this.RoleEP = RoleEP;
            Status = StatusAssignment.None;
            this.DB_ID = 0;
        }
        public dtoGenericAssignment(long DB_ID,int ItemID, String ItemName, RoleEP RoleEP)
        {
            this.DB_ID = DB_ID;
            this.ItemID = ItemID;
            this.ItemName = ItemName;
            this.RoleEP = RoleEP;
            Status = StatusAssignment.None;
        } 
    }


    public class dtoGenericAssignmentItemIDcomparer : IEqualityComparer<dtoGenericAssignment>
    {

        public bool Equals(dtoGenericAssignment x, dtoGenericAssignment y)
        {
            return x.ItemID == y.ItemID;
        }

        public int GetHashCode(dtoGenericAssignment obj)
        {
            return obj.ItemID.GetHashCode();
        }
    }
}
