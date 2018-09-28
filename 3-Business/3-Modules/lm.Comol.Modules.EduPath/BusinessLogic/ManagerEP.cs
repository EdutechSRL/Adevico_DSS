using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using lm.Comol.Modules.EduPath.Domain;
using System.Diagnostics;
using System.Linq.Expressions;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public class ManagerEP : lm.Comol.Core.Business.BaseModuleManager
    {
        //private iDataContext DC { set; get; }

        #region initClass
        public ManagerEP() { }

        public ManagerEP(iDataContext oDC) : base(oDC)
        {
        }

        public ManagerEP(iApplicationContext oContext)
            : base(oContext)
        {
        }
        #endregion

     
        //public int GetActiveSubscriptionRoleId(int PersonID, int CommunityID)
        //{
        //    int RoleId = 0;
        //    RoleId = (from s in Linq<Subscription>()
        //              where s.Community.Id == CommunityID && s.Person.Id == PersonID && s.Accepted && s.Enabled
        //              select s.Role.Id).FirstOrDefault();
        //    return RoleId;
        //}

        public List<litePerson> GetPersons_ByIdRole(int idRole, int idCommunity)
        {
            return GetLitePersons(GetIdPerson_ByIdRole(idRole, idCommunity));
        }

        public int GetIdPerson_ByIdRoleCount(int idRole, int idCommunity)
        {
            return (from s in Linq<LazySubscription>()
                    where s.IdCommunity == idCommunity && s.IdRole == idRole && s.Accepted && s.Enabled
                    select s.IdPerson).Count();
        }

        public List<int> GetIdPerson_ByIdRole(int idRole, int idCommunity)
        {
            return (from s in Linq<LazySubscription>()
                    where s.IdCommunity == idCommunity && s.IdRole == idRole && s.Accepted && s.Enabled
                    select s.IdPerson).ToList();
        }

        public IList<Role> GetAvailableFullRoles(Int32 idcommunity)
        {
            return GetAvailableFullRoles(Get<Community>(idcommunity));
        }

        public IList<Role> GetAvailableFullRoles(Community community)
        {
            List<Role> roles = new List<Role>();

            try
            {
                roles = (from r in Linq<RoleCommunityTypeTemplate>() where r.Type == community.TypeOfCommunity select r.Role).ToList();
            }
            catch (Exception ex)
            {

            }
            return roles;
        }

        public List<Int32> GetActiveIdRoles(Int32 idCommunity)
        {
            List<Int32> roles = new List<Int32>();
            try
            {
                roles = (from r in Linq<LazySubscription>() where r.IdCommunity == idCommunity select r.IdRole).ToList();
            }
            catch (Exception ex)
            {
            }
            return roles;
        }
        public List<dtoUserRole> GetUsersRoles(List<Int32> idUsers,Int32 idCommunity)
        {
            List<dtoUserRole> roles = new List<dtoUserRole>();
            try
            {
                var query = (from r in Linq<LazySubscription>() where r.IdCommunity == idCommunity select r);
                if (idUsers.Count < maxItemsForQuery)
                    roles = query.Where(s=> idUsers.Contains(s.IdPerson)).Select(r => new dtoUserRole() { IdUser = r.IdPerson, IdRole = r.IdRole }).ToList();
                else
                    roles = query.Select(r => new dtoUserRole() { IdUser = r.IdPerson, IdRole = r.IdRole }).ToList().Where(s=> idUsers.Contains(s.IdUser)).ToList();
            }
            catch (Exception ex)
            {
            }
            return roles;
        }
        public IList<Role> GetActiveRoles(Int32 idcommunity)
        {
            IList<Role> roles = new List<Role>();
            try
            {
                var q=(from r in Linq<LazySubscription>() where r.IdCommunity == idcommunity && r.Enabled==true && r.Accepted==true && r.IdRole>0 select r.IdRole).ToList().Distinct().ToList();

                roles = (from item in q select Get<Role>(item)).ToList();
            }
            catch (Exception ex)
            {
            }


            return roles;
        }
        public Int32 GetIdCommunityRole(Int32 idPerson, Int32 idCommunity)
        {
            Int32 idRole = (Int32)RoleTypeStandard.Guest;
            try
            {
                idRole = (from s in Linq<LazySubscription>() where s.IdCommunity== idCommunity && s.IdPerson==idPerson && s.Accepted && s.Enabled select s.IdRole).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch(Exception)
            {}
            return idRole;
        }
        //public void DeletePhysicalList<T>(IEnumerable<T> list)
        //{
        //    foreach (var item in list)
        //    {
        //        DeletePhysical(item);
        //    }
        //}

        //public void DeletePhysicalListIds<T>(IEnumerable<long> listId)
        //{
        //    foreach (var id in listId)
        //    {
        //        T item = Get<T>(id);
        //        DeletePhysical(item);
        //    }
        //}

        #region Delete Permanently
      
        public void DeleteOnlyEP(long PathID)
        {
            Path oPath = Get<Path>(PathID);
            if (oPath != null)
                DeletePhysical(oPath);
        }
        public void DeleteOnlyUnit(long UnitId)
        {
            Unit oUnit = Get<Unit>(UnitId);
            if (oUnit != null)
                DeletePhysical(oUnit);
        }
        public void DeleteOnlyActivity(long ActivityId)
        {
            Activity oActivity = Get<Activity>(ActivityId);
            if (oActivity != null)
                DeletePhysical(oActivity);
        }
        #endregion    
    }
}