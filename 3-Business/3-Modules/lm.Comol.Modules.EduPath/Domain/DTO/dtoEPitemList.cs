using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoEPitemList : dtoGenericItem, IComparable, IEquatable<dtoEPitemList>   
   {
        public Boolean isDefault { get; set; }
        public long ActivityToManage { get; set; }
        public long UnitToManage { get; set; }
        public long ActivityToEvaluate { get; set; }
        public long UnitToEvaluate { get; set; }
        public bool isModificable { get; set; }

        public Boolean isMooc { get; set; }
        public MoocType moocType { get; set; }
        public EPType type { get; set; } 

        public dtoEPitemList()
            :base()
        {            
           
        }
        //public dtoEPitemList(Path oPath, Status PersonalStatus, RoleEP AssRoleEP, Int16 UnitToManage,Int16 UnitToEvaluate, Int16 ActivityToManage, Int16 ActivityToEvaluate )
        //:base   (oPath,PersonalStatus,AssRoleEP)
        //{
        //    isDefault = oPath.isDefault;
        //    this.ActivityToManage = ActivityToManage;
        //    this.UnitToManage = UnitToManage;
        //    this.ActivityToEvaluate = ActivityToEvaluate;
        //    this.UnitToEvaluate = UnitToEvaluate;
        //}
        public dtoEPitemList(Path oPath, Status PersonalStatus, RoleEP AssRoleEP)
            : base(oPath, PersonalStatus,AssRoleEP)
        {
            isDefault = oPath.isDefault;
            ActivityToManage = 0;
            UnitToManage = 0;
            ActivityToEvaluate = 0;
            UnitToEvaluate = 0;
            isMooc = oPath.IsMooc;
            moocType = oPath.MoocType;
        }
        public dtoEPitemList(Path oPath, Status PersonalStatus, RoleEP AssRoleEP, dtoPathManageStatistics statistics)
        :base   (oPath,PersonalStatus,AssRoleEP)
        {
            isDefault = oPath.isDefault;
            ActivityToManage = statistics.ActivitiesToManage;
            ActivityToEvaluate = statistics.ActivitiesToEvaluate;
            UnitToManage = statistics.UnitsToManage;
            UnitToEvaluate = statistics.UnitsToEvaluate;
            isMooc = oPath.IsMooc;
            moocType = oPath.MoocType;
        }

        
        public int CompareTo(object obj)
        {
            dtoEPitemList Obj = (dtoEPitemList)obj;

            return this.Id.CompareTo(Obj.Id);
        }

        public bool Equals(dtoEPitemList other)
        {
            return this.Id.Equals(other.Id);
        }
   }
}
