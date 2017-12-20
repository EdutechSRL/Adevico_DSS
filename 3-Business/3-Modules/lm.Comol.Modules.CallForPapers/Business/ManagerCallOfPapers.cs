using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using System.Diagnostics;
using System.Linq.Expressions;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public class ManagerCallOfPapers : lm.Comol.Core.DomainModel.Common.iDomainManager
    {
        private iDataContext DC { set; get; }

        #region initClass
        public ManagerCallOfPapers() { }

        public ManagerCallOfPapers(iDataContext oDC)
        {
            this.DC = oDC;
        }

        public ManagerCallOfPapers(iApplicationContext oContext)
        {
            this.DC = oContext.DataContext;
        }
        #endregion

        public void RollBack()
        {
            if (DC.isInTransaction)
                 DC.Rollback();
        }

        public void BeginTransaction()
        {
            DC.BeginTransaction();
        }
        public Boolean IsInTransaction()
        {
            return DC.isInTransaction;
        }
        public void Commit()
        {
            DC.Commit();
        }

        public Community GetCommunity(int CommunityID)
        {
            Community oComm = null;
            oComm = DC.GetById<Community>(CommunityID);
            return oComm;
        }
        public Person GetPerson(int PersonID)
        {
            Person o = null;
            o = DC.GetById<Person>(PersonID);
            return o;
        }
        public Subscription GetSubscription(int PersonID, int CommunityID)
        {
            Subscription sub = null;
            sub = GetAll<Subscription>(x => x.Community.Id == CommunityID && x.Person.Id == PersonID).Skip(0).Take(1).ToList().FirstOrDefault();
            return sub;
        }
        public Role GetRole(int RoleID)
        {
            Role o = null;
            o = DC.GetById<Role>(RoleID);
            return o;
        }
        public int GetModuleID(string moduleCode)
        {
            int moduleID = -1;
            try{
                moduleID = (from m in Linq<ModuleDefinition>() where m.Code.Equals(moduleCode) select m.Id).FirstOrDefault();
               }
            catch (Exception ex){
                moduleID = -1;
            }
            return moduleID;
        }
        public Subscription GetActiveSubscription(int PersonID, int CommunityID)
        {
            Subscription sub = null;
            sub = GetAll<Subscription>(x => x.Community.Id == CommunityID && x.Person.Id == PersonID && x.Accepted && x.Enabled).Skip(0).Take(1).ToList().FirstOrDefault();
            return sub;
        }
        public int GetActiveSubscriptionRoleId(int PersonID, int CommunityID)
        {
            int RoleId = 0;
            RoleId = (from s in Linq<Subscription>()
                      where s.Community.Id == CommunityID && s.Person.Id == PersonID && s.Accepted && s.Enabled
                        select s.Role.Id).FirstOrDefault();
            return RoleId;
        }

        public Boolean HasModulePermission(int userID, int communityId, int moduleId, long RequiredPermission)
        {
            Boolean iResponse = false;
            int roleId = GetActiveSubscriptionRoleId(userID, communityId);
            if (roleId != 0)
            {
                ModuleDefinition module = (from cModule in Linq<CommunityModuleAssociation>()
                                           where cModule.Enabled && cModule.Service.Available && cModule.Community.Id == communityId && cModule.Service.Id == moduleId
                                           select cModule.Service).FirstOrDefault<ModuleDefinition>();

                if (module != null)
                {
                    //Find all Role perission for the input community and the input service
                    CommunityRoleModulePermission RolePermission = (from crmp in Linq<CommunityRoleModulePermission>()
                                                                    where crmp.Community.Id == communityId && crmp.Service == module && crmp.Role.Id == roleId
                                                                    select crmp).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (RolePermission != null)
                        iResponse = PermissionHelper.CheckPermissionSoft(RequiredPermission, RolePermission.PermissionInt);
                }
            }
            return iResponse;
        }
        public Boolean HasModulePermission(int userID, int roleId, int communityId, int moduleId, long RequiredPermission)
        {
            Boolean iResponse = false;
            Subscription subscription = GetActiveSubscription(userID, communityId);
            if (subscription != null)
            {
                ModuleDefinition module = (from cModule in Linq <CommunityModuleAssociation>()
                                           where cModule.Enabled && cModule.Service.Available && cModule.Community.Id == communityId && cModule.Service.Id == moduleId
                                           select cModule.Service).FirstOrDefault<ModuleDefinition>();

                if (module != null)
                {
                    //Find all Role perission for the input community and the input service
                    CommunityRoleModulePermission RolePermission = (from crmp in Linq<CommunityRoleModulePermission>()
                                                                    where crmp.Community.Id == communityId && crmp.Service == module && crmp.Role.Id == roleId
                                                                    select crmp).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (RolePermission != null)
                        iResponse = PermissionHelper.CheckPermissionSoft(RequiredPermission, RolePermission.PermissionInt);
                }
            }
            return iResponse;
        }
        public long GetModulePermission(int userID,  int communityId, int moduleId)
        {
            long permission = 0;
            int roleId = GetActiveSubscriptionRoleId(userID, communityId);
            if (roleId != 0)
            {
                ModuleDefinition module = (from cModule in Linq<CommunityModuleAssociation>()
                                           where cModule.Enabled && cModule.Service.Available && cModule.Community.Id == communityId && cModule.Service.Id == moduleId
                                           select cModule.Service).FirstOrDefault<ModuleDefinition>();

                if (module != null)
                {
                    String permissionValue = (from crmp in Linq<CommunityRoleModulePermission>()
                                                                    where crmp.Community.Id == communityId && crmp.Service == module && crmp.Role.Id == roleId
                                                                    select crmp.PermissionString).FirstOrDefault();
                    if (String.IsNullOrEmpty(permissionValue))
                        permission = 0;
                    else
                        permission = Convert.ToInt64(new String(permissionValue.Reverse().ToArray()), 2);
                   }
            }
            return permission;
        }


        #region "Common manager"
        public void SaveOrUpdate<T>(T item)
        {
            DC.SaveOrUpdate(item);
        }

        public void SaveOrUpdateList<T>(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                SaveOrUpdate(item);
            }
        }

        public void DeletePhysical<T>(T item)
        {
            DC.Delete(item);
        }

        public void DeletePhysicalList<T>(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                DeletePhysical(item);
            }
        }

        public void DeletePhysicalList<T>(IEnumerable<long> listId)
        {
            foreach (var id in listId)
            {
                T item = Get<T>(id);
                DeletePhysical(item);
            }
        }

        public T Get<T>(object id)
        {            
            return (T)DC.GetById<T>(id);
        }

        public IList<T> GetAll<T>(Expression<Func<T,bool>> condition = null)
        {
            var query=(from item in DC.GetCurrentSession().Linq<T>() select item);
            
            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query.ToList<T>();
        }

        public INHibernateQueryable<T> Linq<T>()
        {
            return DC.GetCurrentSession().Linq<T>();
        }

        public IQueryable<T> GetIQ<T>()
        {            
            return (from item in Linq<T>() select item);
        }
        
        #region Delete Permanently
        //public void DeletePathPersonAssignments(List<long> ListOfInterestedId)
        //{
        //    PathPersonAssignment AssignmentToDelete;
        //    foreach (long item in ListOfInterestedId)
        //    {
        //        AssignmentToDelete = Get<PathPersonAssignment>(item);
        //        DC.Delete(AssignmentToDelete);
        //    }
        //}
        //public void DeletePathCRoleAssignments(List<long> ListOfInterestedId)
        //{
        //    PathCRoleAssignment AssignmentToDelete;
        //    foreach (long item in ListOfInterestedId)
        //    {
        //        AssignmentToDelete = Get<PathCRoleAssignment>(item);
        //        DC.Delete(AssignmentToDelete);
        //    }
        //}
        //public void DeleteUnitAssignments(List<long> PersonAssignmentsId, List<long> CRoleAssignmentId)
        //{
        //    UnitPersonAssignment PersonAssignmentToDelete;
        //    foreach (long item in PersonAssignmentsId)
        //    {
        //        PersonAssignmentToDelete = Get<UnitPersonAssignment>(item);
        //        DC.Delete(PersonAssignmentToDelete);
        //    }
        //    UnitCRoleAssignment CRoleAssignmentToDelete;
        //    foreach (long item in CRoleAssignmentId)
        //    {
        //        CRoleAssignmentToDelete = Get<UnitCRoleAssignment>(item); 
        //        DC.Delete(CRoleAssignmentToDelete);
        //    }
        //}
        //public void DeleteActivityAssignments(List<long> PersonAssignmentsId, List<long> CRoleAssignmentId)
        //{
        //    ActivityPersonAssignment PersonAssignmentToDelete;
        //    foreach (long item in PersonAssignmentsId)
        //    {
        //        PersonAssignmentToDelete = Get<ActivityPersonAssignment>(item);
        //        DC.Delete(PersonAssignmentToDelete);
        //    }
        //    ActivityCRoleAssignment CRoleAssignmentToDelete;
        //    foreach (long item in CRoleAssignmentId)
        //    {
        //        CRoleAssignmentToDelete = Get<ActivityCRoleAssignment>(item);
        //        DC.Delete(CRoleAssignmentToDelete);
        //    }
        //}
        //public void DeleteOnlyEP(long PathID)
        //{
        //    Path oPath = Get<Path>(PathID);
        //    DC.Delete(oPath);
        //}
        //public void DeleteOnlyUnit(long UnitId)
        //{
        //    Unit oUnit = Get<Unit>(UnitId);
        //    DC.Delete(oUnit);
        //}
        //public void DeleteOnlyActivity(long ActivityId)
        //{
        //    Activity oActivity = Get<Activity>(ActivityId);
        //    DC.Delete(oActivity);
        //}
        public void DeleteGeneric(Object o)
        {
            DC.Delete(o);
        }
        #endregion

        public void Refresh<T>(T item) {
            DC.Refresh<T>(item);
        }
        #endregion
    }
}