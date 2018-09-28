using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoGenericAssignmentWithOldRoleEP:dtoGenericAssignment
    {
        public RoleEP OldRoleEP;

        public dtoGenericAssignmentWithOldRoleEP()
            : base()
        {
            OldRoleEP = RoleEP.None;
        }
        public dtoGenericAssignmentWithOldRoleEP(int ItemID, String ItemName)
            : base(ItemID, ItemName)
        {
            OldRoleEP = RoleEP.None;
        }
        public dtoGenericAssignmentWithOldRoleEP(int ItemID, String ItemName, RoleEP NewRoleEP, RoleEP OldRoleEP)
       :base(ItemID,ItemName,NewRoleEP)
        {
            this.OldRoleEP = OldRoleEP;
        }
        public dtoGenericAssignmentWithOldRoleEP(long DB_ID,int ItemID, String ItemName, RoleEP NewRoleEP, RoleEP OldRoleEP)
            : base(DB_ID,ItemID, ItemName, NewRoleEP)
        {
            this.OldRoleEP = OldRoleEP;
        }
        public dtoGenericAssignmentWithOldRoleEP(int ItemID, String ItemName, RoleEP RoleEP)
            : base(ItemID, ItemName, RoleEP)
        {
            this.OldRoleEP = RoleEP;
        }
        public dtoGenericAssignmentWithOldRoleEP(long DB_ID, int ItemID, String ItemName, RoleEP RoleEP)
            : base(DB_ID,ItemID, ItemName, RoleEP)
        {
            this.OldRoleEP = RoleEP;
        }
    }

    public class dtoGenericAssignmentWithOldRoleEPItemIDcomparer : IEqualityComparer<dtoGenericAssignmentWithOldRoleEP>
    {

        public bool Equals(dtoGenericAssignmentWithOldRoleEP x, dtoGenericAssignmentWithOldRoleEP y)
        {
            return x.ItemID == y.ItemID;
        }

        public int GetHashCode(dtoGenericAssignmentWithOldRoleEP obj)
        {
            return obj.ItemID.GetHashCode();
        }
    }

    //public class dtoGenericAssignmentWithOldRoleEPIDcomparer : IEqualityComparer<dtoGenericAssignmentWithOldRoleEP>
    //{

    //    public bool Equals(dtoGenericAssignmentWithOldRoleEP x, dtoGenericAssignmentWithOldRoleEP y)
    //    {
    //        if x
    //        return x.ItemID == y.ItemID;
    //    }

    //    public int GetHashCode(dtoGenericAssignmentWithOldRoleEP obj)
    //    {
    //        return obj.ItemID.GetHashCode();
    //    }
    //}



    public class dtoGenericAssWithOldRoleEpAndDeleteItemIDcomparer : IEqualityComparer<dtoGenericAssWithOldRoleEpAndDelete>
    {

        public bool Equals(dtoGenericAssWithOldRoleEpAndDelete x, dtoGenericAssWithOldRoleEpAndDelete y)
        {
            return x.ItemID == y.ItemID;
        }

        public int GetHashCode(dtoGenericAssWithOldRoleEpAndDelete obj)
        {
            return obj.ItemID.GetHashCode();
        }
    }


    public class dtoGenericAssWithOldRoleEpAndDelete:dtoGenericAssignmentWithOldRoleEP
    {
        public bool isDeleted { get; set; }

           public dtoGenericAssWithOldRoleEpAndDelete()
            : base()
        {
            OldRoleEP = RoleEP.None;
        }
        public dtoGenericAssWithOldRoleEpAndDelete(int ItemID, String ItemName)
            : base(ItemID, ItemName)
        {
            OldRoleEP = RoleEP.None;
        }
        public dtoGenericAssWithOldRoleEpAndDelete(int ItemID, String ItemName, RoleEP NewRoleEP, RoleEP OldRoleEP)
       :base(ItemID,ItemName,NewRoleEP)
        {
            this.OldRoleEP = OldRoleEP;
        }
        public dtoGenericAssWithOldRoleEpAndDelete(long DB_ID,int ItemID, String ItemName, RoleEP NewRoleEP, RoleEP OldRoleEP)
            : base(DB_ID,ItemID, ItemName, NewRoleEP)
        {
            this.OldRoleEP = OldRoleEP;
        }
        public dtoGenericAssWithOldRoleEpAndDelete(int ItemID, String ItemName, RoleEP RoleEP)
            : base(ItemID, ItemName, RoleEP)
        {
            this.OldRoleEP = RoleEP;
        }
        public dtoGenericAssWithOldRoleEpAndDelete(long DB_ID, int ItemID, String ItemName, RoleEP RoleEP)
            : base(DB_ID,ItemID, ItemName, RoleEP)
        {
            this.OldRoleEP = RoleEP;
        }

    }



}
