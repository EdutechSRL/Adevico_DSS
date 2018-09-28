using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
   [Serializable]
    public class dtoGenericAssignmentWithDelete:dtoGenericAssignment
    {
        public bool isDeleted { get; set; }

           
        public dtoGenericAssignmentWithDelete()
            :base()
        {
            isDeleted = false;
        }
        public dtoGenericAssignmentWithDelete(int ItemID, String ItemName)
        :base(ItemID,ItemName)
        {
            isDeleted = false;
        }
        public dtoGenericAssignmentWithDelete(int ItemID, String ItemName, RoleEP RoleEP, bool isDeleted)
       :base(ItemID,ItemName,RoleEP)
        {
            this.isDeleted = isDeleted;
        }
        public dtoGenericAssignmentWithDelete(long DB_ID,int ItemID, String ItemName, RoleEP RoleEP, bool isDeleted)
            : base(DB_ID,ItemID, ItemName, RoleEP)
        {
            this.isDeleted = isDeleted;
        }
    }
}
