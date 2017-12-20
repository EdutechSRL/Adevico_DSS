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

namespace lm.Comol.Core.Business
{
    public class BaseModuleManager : lm.Comol.Core.DomainModel.Common.iDomainManager
    {
        protected const int maxItemsForQuery = 500;
        private iDataContext DC { set; get; }

        #region initClass
        public BaseModuleManager() { }

        public BaseModuleManager(iDataContext oDC)
        {
            this.DC = oDC;
        }

        public BaseModuleManager(iApplicationContext oContext)
        {
            this.DC = oContext.DataContext;
        }
        #endregion

        #region "DM Methods"
        #region "Transactions"
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
        #endregion

        #region "Query"
        public T Get<T>(object id)
        {
            return (T)DC.GetById<T>(id);
        }

        /// <summary>
        /// Recupera TUTTI gli oggetti completi.
        /// </summary>
        /// <typeparam name="T">Tipo oggetto</typeparam>
        /// <param name="condition">Lambda expression per filtro dati</param>
        /// <returns>Lista di oggetti del tipo indicato</returns>
        /// <remarks>SE non si usa "condition", recupera TUTTI gli oggetti nel dB prima di filtrarli</remarks>
        /// <example>
        /// .GetAll(Of Person)()   => TUTTI gli oggetti person completi, presenti nel dB
        /// .GetAll(Of Person)(p => p.LanguageID == value) => Oggetti person completi in cui .LanguageID == value
        /// </example>
        /// <remarks>
        /// .GetAll(Of Person)(p => p.LanguageID == value)     è equivalente a
        /// from Person p in GetIQ(Of Person)() where p.LanguageID == value select p   
        /// </remarks>
        public IList<T> GetAll<T>(Expression<Func<T, bool>> condition = null)
        {
            var query = (from item in DC.GetCurrentSession().Linq<T>() select item);

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

        /// <summary>
        /// Da usare con Query Linq. Recupera solo i PARAMETRI INDICATI degli oggetti filtrati nell'espressione LINQ.
        /// </summary>
        /// <typeparam name="T">Tipo oggetto su cui lavorare</typeparam>
        /// <returns>A seconda della query Linq.</returns>
        /// <example>
        /// from Person p in GetIQ(Of Person)() where p.LanguageID == value select p.SurnameAndName    = elenco di soli nome e cognome delle persone con LanguageID == value
        /// </example>
        /// <remarks>
        /// from Person p in GetIQ(Of Person)() where p.LanguageID == value select p   è equivalente a
        /// .GetAll(Of Person)(p => p.LanguageID == value)
        /// </remarks>
        public IQueryable<T> GetIQ<T>()
        {
            return (from item in Linq<T>() select item);
        }
        #endregion

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

        #region Delete Permanently
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
        public void DeleteGeneric(Object o)
        {
            DC.Delete(o);
        }
        #endregion

        private IQuery GetNamedQuery(String namedQuery)
        {
            return DC.GetCurrentSession().GetNamedQuery(namedQuery);
        }

        public void Refresh<T>(T obj)
        {
            DC.GetCurrentSession().Refresh(obj);
        }
        public void Flush()
        {
            DC.GetCurrentSession().Flush();
        }
        public void Detach<T>(T obj)
        {
            DC.GetCurrentSession().Evict(obj);
        }

        public void DetachList<T>(IList<T> objs)
        {
            foreach (T obj in objs)
            {
                DC.GetCurrentSession().Evict(obj);
            }
        }

        //public T Merge<T>(T obj)
        //{
        //    return (T)DC.GetCurrentSession().Merge(obj);
        //}
        #endregion

        #region "Common Methods"
        #region "Transaction"
        public void Tx(Action method)
        {
            ////usage:
            //Tx(() =>
            //{
            //    // my code comes here
            //});

            try
            {
                this.BeginTransaction();

                method();

                this.Commit();
            }
            catch (Exception ex)
            {
                this.RollBack();
                throw ex;
            }

        }
        #endregion
        #region "Community"
        public Int32 GetIdCommunityType(Int32 idCommunity)
        {
            return (from c in DC.GetCurrentSession().Linq<liteCommunity>() where c.Id == idCommunity select c.IdType).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public Community GetCommunity(Int32 idCommunity)
        {
            Community community = DC.GetById<Community>(idCommunity);
            return (community == null || community.Id == 0) ? null : community;
        }
        public liteCommunityInfo GetLiteCommunityInfo(Int32 idCommunity)
        {
            liteCommunityInfo community = DC.GetById<liteCommunityInfo>(idCommunity);
            return (community == null || community.Id == 0) ? null : community;
        }
        public liteCommunity GetLiteCommunity(Int32 idCommunity)
        {
            liteCommunity community = DC.GetById<liteCommunity>(idCommunity);
            return (community == null || community.Id == 0) ? null : community;
        }
        public String GetCommunityName(int idCommunity)
        {
            return (from c in DC.GetCurrentSession().Linq<liteCommunity>() where c.Id == idCommunity select c.Name).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public List<liteCommunity> GetLiteCommunities(List<Int32> idCommunities)
        {
            List<liteCommunity> results = new List<liteCommunity>();
            if (idCommunities.Count <= maxItemsForQuery)
                results = (from s in Linq<liteCommunity>() where idCommunities.Contains(s.Id) select s).ToList();
            else
            {
                Int32 pageIndex = 0;
                List<Int32> idPagedCommunities = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                while (idPagedCommunities.Any())
                {
                    results.AddRange((from s in Linq<liteCommunity>() where idPagedCommunities.Contains(s.Id) select s).ToList());
                    pageIndex++;
                    idPagedCommunities = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                }
            }
            return results;
        }
        #endregion
        #region "Organization"
        public Int32 GetIdCommunityFromOrganization(int idOrganization)
        {
            Int32 idCommunity = 0;
            try
            {
                idCommunity = (from c in DC.GetCurrentSession().Linq<liteCommunityInfo>() where c.IdOrganization == idOrganization && c.IdFather == 0 select c.Id).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex) { }
            return idCommunity;
        }
        public Int32 GetIdOrganizationFromCommunity(Int32 idCommunity)
        {
            Int32 idOrganization = 0;
            try
            {
                idOrganization = (from c in DC.GetCurrentSession().Linq<liteCommunity>()
                                  where c.Id == idCommunity
                                  select c.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex) { }
            return idOrganization;
        }
        public Organization GetOrganization(int idOrganization)
        {
            Organization organization = DC.GetById<Organization>(idOrganization);
            return (organization == null || organization.Id == 0) ? null : organization;
        }
        public String GetOrganizationName(int idOrganization)
        {
            String name = "";
            try
            {
                name = (from o in DC.GetCurrentSession().Linq<Organization>() where o.Id == idOrganization select o.Name).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return name;
        }
        public IList<Organization> GetAllOrganizations()
        {
            IList<Organization> oOrg = null;
            oOrg = (from item in DC.GetCurrentSession().Linq<Organization>() select item).ToList();
            return oOrg;
        }
        #endregion
        #region "Person"
        public Person GetUnknownUser()
        {
            Person person = null;
            try
            {
                person = (from p in Linq<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return person;
        }
        public Person GetUserByMail(String mail)
        {
            Person person = null;
            try
            {
                person = (from p in Linq<Person>() where p.Mail == mail select p).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return person;
        }
        public Int32 GetUserDefaultIdOrganization(Int32 idPerson)
        {
            Int32 idOrganization = 0;
            try
            {
                liteOrganizationProfile dOrganization = (from o in DC.GetCurrentSession().Linq<liteOrganizationProfile>()
                                                         where o.IdPerson == idPerson && o.isDefault
                                                         select o).Skip(0).Take(1).ToList().FirstOrDefault();
                idOrganization = (dOrganization == null) ? 0 : dOrganization.IdOrganization;
            }
            catch (Exception ex) { }
            return idOrganization;
        }
        public String GetUserDefaultOrganizationName(Int32 idPerson)
        {
            String name = "";
            try
            {
                liteOrganizationProfile dOrganization = (from o in DC.GetCurrentSession().Linq<liteOrganizationProfile>()
                                                         where o.IdPerson == idPerson && o.isDefault
                                                         select o).Skip(0).Take(1).ToList().FirstOrDefault();
                Int32 idOrganization = (dOrganization == null) ? 0 : dOrganization.IdOrganization;
                name = GetOrganizationName(idOrganization);
            }
            catch (Exception ex) { }
            return name;
        }
        public List<Int32> GetUserOrganizations(Int32 idPerson)
        {
            return (from o in DC.GetCurrentSession().Linq<liteOrganizationProfile>()
                    where o.IdPerson == idPerson
                    select o.Id).ToList();
        }
        public Person GetPerson(Int32 idPerson)
        {
            Person o = null;
            o = DC.GetById<Person>(idPerson);
            return o;
        }
        public litePerson GetLitePerson(Int32 idPerson)
        {
            return DC.GetById<litePerson>(idPerson);
        }
        public litePerson GetLiteUnknownUser()
        {
            litePerson person = null;
            try
            {
                person = (from p in Linq<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return person;
        }
        public litePerson GetLitePublicUser()
        {
            litePerson person = null;
            try
            {
                person = (from p in Linq<litePerson>() where p.TypeID == (int)UserTypeStandard.PublicUser select p).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return person;
        }
        public litePerson GetLitePublicUser(Int32 idCommunity)
        {
            if (idCommunity <= 0)
                return GetLitePublicUser();
            else
            {
                litePerson person = null;
                try
                {
                    List<litePerson> persons = (from p in Linq<litePerson>() where p.TypeID == (int)UserTypeStandard.PublicUser select p).ToList();
                    person = persons.Where(p => (from s in Linq<liteSubscription>() where s.Accepted && s.Enabled && s.IdCommunity == idCommunity && s.Person.Id == p.Id select s.Id).Any()).FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                return person;
            }

        }
        public String GetUsername(Int32 idPerson)
        {
            return (from p in Linq<litePerson>() where p.Id == idPerson select new { Name = p.Name, Surname = p.Surname }).Skip(0).Take(1).ToList().Select(i => i.Surname + ' ' + i.Name).FirstOrDefault();
        }
        public List<litePerson> GetLitePersons(List<Int32> idUsers)
        {
            List<litePerson> persons = new List<litePerson>();
            if (idUsers.Count <= maxItemsForQuery)
                persons = (from s in Linq<litePerson>() where idUsers.Contains(s.Id) select s).ToList();
            else
            {
                Int32 pageIndex = 0;
                List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                while (idPagedUsers.Any())
                {
                    persons.AddRange((from s in Linq<litePerson>() where idPagedUsers.Contains(s.Id) select s).ToList());
                    pageIndex++;
                    idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                }
            }
            return persons;
        }
        public Int32 GetIdProfileType(Int32 idPerson)
        {
            Int32 idType = (from p in Linq<litePerson>() where p.Id == idPerson select p.TypeID).Skip(0).Take(1).ToList().FirstOrDefault();
            if (idType == 0)
                idType = (Int32)UserTypeStandard.Guest;
            return idType;
        }

        public Int32 GetIdUnknownUser()
        {
            return (from p in Linq<litePerson>() where p.TypeID == (Int32)UserTypeStandard.Guest select p.Id).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        #endregion
        #region "Language"
        public Language GetLanguage(Int32 idLanguage)
        {
            Language language = DC.GetById<Language>(idLanguage);
            return (language == null || language.Id == 0) ? null : language;
        }
        public Language GetLanguage(String code)
        {
            Language language = null;
            try
            {
                language = (from l in DC.GetCurrentSession().Linq<Language>() where l.Code == code select l).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex) { }
            return language;
        }
        public Language GetLanguageByCodeOrDefault(String lCode, Boolean getDefault = true)
        {
            Language result = null;
            if (!String.IsNullOrEmpty(lCode))
            {
                List<Language> languages = (from l in DC.GetCurrentSession().Linq<Language>() where l.Code == lCode || l.Code.StartsWith(lCode + "-") select l).ToList();
                if (languages.Count == 1 || languages.Count > 1)
                    result = languages.FirstOrDefault();
            }
            return (result == null) ? ((getDefault) ? GetDefaultLanguage() : null) : result;
        }
        public Language GetLanguageByIdOrDefault(Int32 idLanguage, Boolean getDefault = true)
        {
            Language language = Get<Language>(idLanguage);
            return (language == null) ? ((getDefault) ? GetDefaultLanguage() : null) : language;
        }
        public Language GetDefaultLanguage()
        {
            return (from l in DC.GetCurrentSession().Linq<Language>() where l.isDefault select l).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public Int32 GetDefaultIdLanguage()
        {
            return (from l in DC.GetCurrentSession().Linq<Language>() where l.isDefault select l.Id).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public Language GetUserLanguage(Int32 idPerson, Boolean orDefault = false)
        {
            return GetUserLanguage(GetLitePerson(idPerson), orDefault);
        }
        public Language GetUserLanguage(litePerson p, Boolean orDefault = false)
        {
            return (p != null) ? GetLanguageByIdOrDefault(p.LanguageID, orDefault) : ((orDefault) ? GetDefaultLanguage() : null);
        }
        public IList<Language> GetAllLanguages()
        {
            IList<Language> oLangs = null;
            oLangs = (from item in DC.GetCurrentSession().Linq<Language>() select item).ToList();
            return oLangs;
        }
        #endregion



        #endregion

        #region "Enrollments"
        #region "OLD"
        public Subscription GetSubscription(int PersonID, int CommunityID)
        {
            Subscription sub = null;
            sub = GetAll<Subscription>(x => x.Community.Id == CommunityID && x.Person.Id == PersonID).Skip(0).Take(1).ToList().FirstOrDefault();
            return sub;
        }
        public Subscription GetSubscription(Person person, Community community)
        {
            Subscription sub = null;
            sub = GetAll<Subscription>(x => x.Community == community && x.Person == person).Skip(0).Take(1).ToList().FirstOrDefault();
            return sub;
        }
        public Role GetSubscriptionRole(int PersonID, int CommunityID)
        {
            return (from s in GetIQ<Subscription>() where s.Community.Id == CommunityID && s.Person.Id == PersonID select (Role)s.Role).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public Role GetRole(int RoleID)
        {
            Role o = null;
            o = DC.GetById<Role>(RoleID);
            return o;
        }
        public Subscription GetActiveSubscription(int PersonID, int CommunityID)
        {
            Subscription sub = null;
            sub = GetAll<Subscription>(x => x.Community.Id == CommunityID && x.Person.Id == PersonID && x.Accepted && x.Enabled).Skip(0).Take(1).ToList().FirstOrDefault();
            return sub;
        }
        public Boolean HasActiveSubscription(Person person, Community community)
        {
            return (from s in GetIQ<Subscription>()
                    where s.Community == community && s.Person == person && s.Accepted && s.Enabled && s.Role.Id != -3 && s.Role.Id != -2
                    select s.Id).Any();
        }
        #endregion


        public Boolean HasActiveSubscription(int IdPerson, int IdCommunity)
        {
            return (from s in GetIQ<liteSubscription>()
                    where s.IdCommunity == IdCommunity && s.Person.Id == IdPerson && s.Accepted && s.Enabled
                    select s.Id).Any();
        }

        public int GetActiveSubscriptionIdRole(Int32 idPerson, Int32 idCommunity)
        {
            return GetSubscriptionIdRole(idPerson, idCommunity, true);
        }
        public Int32 GetSubscriptionIdRole(Int32 idPerson, Int32 idCommunity, Boolean onlyActive = true)
        {
            string key = idPerson.ToString() + "_" + idCommunity.ToString() + "_" + onlyActive.ToString();
            //if (subscriptionsRole == null)
            //    subscriptionsRole = 
            if (subscriptionsRole.ContainsKey(key) && subscriptionsRole[key] > 0)
                return subscriptionsRole[key];
            else
            {
                var query = (from s in Linq<liteSubscriptionInfo>()
                             where s.IdCommunity == idCommunity && s.IdPerson == idPerson
                             select s);
                if (onlyActive)
                    query = query.Where(s => s.Accepted && s.Enabled);
                List<Int32> idRoles = query.Select(s => s.IdRole).ToList();
                subscriptionsRole[key] = (idRoles == null || !idRoles.Any()) ? 0 : idRoles.FirstOrDefault();
                return subscriptionsRole[key];
            }
        }
        public liteSubscriptionInfo GetLiteSubscriptionInfo(Int32 idPerson, Int32 idCommunity)
        {
            return (from s in Linq<liteSubscriptionInfo>()
                    where s.IdCommunity == idCommunity && s.IdPerson == idPerson
                    select s).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        private Dictionary<String, Int32> subscriptionsRole = new Dictionary<string, int>();
        //public liteSubscription GetLiteSubscription(int idPerson, int idCommunity)
        //{
        //    liteSubscription sub = (from s in Linq<liteSubscription>()
        //                                where s.IdCommunity== idPerson && s.).Skip(0).Take(1).ToList().FirstOrDefault();
        //    return sub;
        //}
        #endregion


        public lm.Comol.Core.Authentication.liteGenericEncryption GetUrlMacEncryptor()
        {
            lm.Comol.Core.Authentication.liteGenericEncryption result = null;
            try
            {
                result = (from e in Linq<lm.Comol.Core.Authentication.liteGenericEncryption>()
                          where e.Deleted == BaseStatusDeleted.None && e.Type == Authentication.MacType.Url
                          select e).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public int GetModuleID(string moduleCode)
        {
            int moduleID = -1;
            try
            {
                moduleID = (from m in Linq<ModuleDefinition>() where m.Code.Equals(moduleCode) select m.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                moduleID = -1;
            }
            return moduleID;
        }
        public Dictionary<String, Int32> GetIdModules(List<String> codes)
        {
            Dictionary<String, Int32> result = new Dictionary<String, Int32>();
            try
            {
                result = (from m in Linq<ModuleDefinition>() where codes.Contains(m.Code) select m).ToDictionary(m => m.Code, m => m.Id);
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public String GetModuleCode(int IdModule)
        {
            String moduleCode = "";
            try
            {
                moduleCode = (from m in Linq<ModuleDefinition>() where m.Id == IdModule select m.Code).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                moduleCode = "";
            }
            return moduleCode;
        }


        //public int GetActiveSubscriptionRoleId(Person person, Community community)
        //{
        //    int RoleId = 0;
        //    RoleId = (from s in Linq<Subscription>()
        //              where s.Community == community && s.Person == person && s.Accepted && s.Enabled
        //              select s.Role.Id).FirstOrDefault();
        //    return RoleId;
        //}
        public List<LazySubscription> GetBaseActiveSubscriptions(Int32 idPerson, List<Int32> idCommunities)
        {
            List<LazySubscription> subscriptions = new List<LazySubscription>();
            Int32 pageIndex = 0;

            List<Int32> idItems = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
            while (idItems.Any())
            {
                subscriptions.AddRange(
                    (from s in GetIQ<LazySubscription>()
                     where s.IdPerson == idPerson && idItems.Contains(s.IdCommunity) && s.Accepted && s.Enabled
                     select s).ToList());
                pageIndex++;
                idItems = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
            }


            return subscriptions;
        }


        public int GetSubscriptionRoleId(Person person, Community community, Boolean onlyActive = true)
        {
            return GetSubscriptionIdRole(person.Id, (community == null ? 0 : community.Id), onlyActive);
        }
        public Role GetDefaultRole(Community community)
        {
            return (from ct in Linq<RoleCommunityTypeTemplate>()
                    where ct.Type == community.TypeOfCommunity && ct.isDefault == true
                    select ct).Skip(0).Take(1).ToList().Select(c => c.Role).FirstOrDefault();
        }
        public Int32 GetDefaultIdRole(Int32 idCommunity)
        {
            Int32 idRole = 0;
            try
            {
                BeginTransaction();
                Community c = Get<Community>(idCommunity);
                if (c == null)
                    idRole = 0;
                else
                {
                    Role r = GetDefaultRole(c);
                    idRole = (r == null) ? 0 : r.Id;
                }
                Commit();
            }
            catch (Exception ex)
            {
                RollBack();
            }
            return idRole;
        }


        public Boolean HasModulePermission(int userID, int communityId, int moduleId, long RequiredPermission)
        {
            Boolean iResponse = false;
            int roleId = GetActiveSubscriptionIdRole(userID, communityId);
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
        public long GetModulePermission(Int32 idUser, Int32 idCommunity, String moduleCode)
        {
            return GetModulePermission(idUser, idCommunity, GetModuleID(moduleCode));
        }
        public long GetModulePermission(Int32 idUser, Int32 idCommunity, Int32 idModule)
        {
            long permission = 0;
            int idRole = GetActiveSubscriptionIdRole(idUser, idCommunity);
            if (idRole != 0)
            {
                ModuleDefinition module = (from cModule in Linq<CommunityModuleAssociation>()
                                           where cModule.Enabled && cModule.Service.Available && cModule.Community.Id == idCommunity && cModule.Service.Id == idModule
                                           select cModule.Service).FirstOrDefault<ModuleDefinition>();

                if (module != null)
                {
                    String permissionValue = (from crmp in Linq<CommunityRoleModulePermission>()
                                              where crmp.Community.Id == idCommunity && crmp.Service == module && crmp.Role.Id == idRole
                                              select crmp.PermissionString).FirstOrDefault();
                    if (String.IsNullOrEmpty(permissionValue))
                        permission = 0;
                    else
                        permission = Convert.ToInt64(new String(permissionValue.Reverse().ToArray()), 2);
                }
            }
            return permission;
        }
        public long GetModulePermissionByRole(int idRole, int idCommunity, int idModule)
        {
            long permission = 0;
            if (idRole != 0)
            {
                ModuleDefinition module = (from cModule in Linq<CommunityModuleAssociation>()
                                           where cModule.Enabled && cModule.Service.Available && cModule.Community.Id == idCommunity && cModule.Service.Id == idModule
                                           select cModule.Service).FirstOrDefault<ModuleDefinition>();

                if (module != null)
                {
                    String permissionValue = (from crmp in Linq<CommunityRoleModulePermission>()
                                              where crmp.Community.Id == idCommunity && crmp.Service == module && crmp.Role.Id == idRole
                                              select crmp.PermissionString).FirstOrDefault();
                    if (String.IsNullOrEmpty(permissionValue))
                        permission = 0;
                    else
                        permission = Convert.ToInt64(new String(permissionValue.Reverse().ToArray()), 2);
                }
            }
            return permission;
        }
        public String GetCommunityTypeName(Int32 idType, Int32 idLanguage)
        {
            String result = "";
            result = GetTranslatedItem<dtoTranslatedCommunityType>(idLanguage).Where(t => t.Id == idType).Select(t => t.Name).FirstOrDefault();
            return result;
        }
        /// <summary>
        /// Give module availability on system or on community
        /// </summary>
        /// <param name="moduleCode">module code</param>
        /// <param name="idCommunity">community identifier</param>
        /// <returns></returns>
        /// 
        private Dictionary<String, Dictionary<Int32, Boolean>> _ActiveModules;
        public Boolean IsModuleActive(String moduleCode, Int32 idCommunity = 0)
        {
            if (_ActiveModules == null)
                _ActiveModules = new Dictionary<string, Dictionary<int, bool>>();
            if (_ActiveModules.ContainsKey(moduleCode) && _ActiveModules[moduleCode].ContainsKey(idCommunity))
                return _ActiveModules[moduleCode][idCommunity];
            else
            {
                ModuleDefinition module = (from m in Linq<ModuleDefinition>()
                                           where m.Code == moduleCode && m.Available
                                           select m).Skip(0).Take(1).ToList().FirstOrDefault();
                Boolean result = (module != null);
                if (idCommunity > 0 && module != null)
                    result = (from m in Linq<liteCommunityModuleAssociation>()
                              where m.IsEnabled && m.IdCommunity == idCommunity && m.IdModule == module.Id
                              select m.Id).Any();
                if (_ActiveModules.ContainsKey(moduleCode))
                    _ActiveModules[moduleCode].Add(idCommunity, result);
                else
                    _ActiveModules.Add(moduleCode, new Dictionary<int, bool>() { { idCommunity, result } });
                return result;
            }
        }


        private Dictionary<String, Dictionary<Int32, ModuleStatus>> _ModulesStatus;

        public ModuleStatus GetModuleStatus(String moduleCode, Int32 idCommunity = 0)
        {
            if (_ModulesStatus == null)
                _ModulesStatus = new Dictionary<string, Dictionary<int, ModuleStatus>>();
            if (_ModulesStatus.ContainsKey(moduleCode) && _ModulesStatus[moduleCode].ContainsKey(idCommunity))
                return _ModulesStatus[moduleCode][idCommunity];
            else
            {
                ModuleDefinition module = (from m in Linq<ModuleDefinition>()
                                           where m.Code == moduleCode && m.Available
                                           select m).Skip(0).Take(1).ToList().FirstOrDefault();
                ModuleStatus status = (module == null) ? ModuleStatus.DisableForSystem : ModuleStatus.ActiveForSystem;

                if (idCommunity > 0 && module != null)
                    status = (from m in Linq<liteCommunityModuleAssociation>()
                              where m.IsEnabled && m.IdCommunity == idCommunity && m.IdModule == module.Id
                              select m.Id).Any() ? ModuleStatus.ActiveForCommunity : ModuleStatus.DisableForCommunity;
                if (_ModulesStatus.ContainsKey(moduleCode))
                    _ModulesStatus[moduleCode].Add(idCommunity, status);
                else
                    _ModulesStatus.Add(moduleCode, new Dictionary<int, ModuleStatus>() { { idCommunity, status } });
                return status;
            }
        }


        //public long GetModulePermission(Person person, Community community, int moduleId)
        //{
        //    long permission = 0;
        //    int roleId = GetActiveSubscriptionIdRole(person, community);
        //    if (roleId != 0)
        //    {
        //        ModuleDefinition module = (from cModule in Linq<CommunityModuleAssociation>()
        //                                   where cModule.Enabled && cModule.Service.Available && cModule.Community == community && cModule.Service.Id == moduleId
        //                                   select cModule.Service).FirstOrDefault<ModuleDefinition>();

        //        if (module != null)
        //        {
        //            String permissionValue = (from crmp in Linq<CommunityRoleModulePermission>()
        //                                      where crmp.Community == community && crmp.Service == module && crmp.Role.Id == roleId
        //                                      select crmp.PermissionString).FirstOrDefault();
        //            if (String.IsNullOrEmpty(permissionValue))
        //                permission = 0;
        //            else
        //                permission = Convert.ToInt64(new String(permissionValue.Reverse().ToArray()), 2);
        //        }
        //    }
        //    return permission;
        //}
        public List<Int32> GetAvailableRoles(Community community)
        {
            List<Int32> roles = new List<Int32>();
            try
            {
                roles = (from r in Linq<RoleCommunityTypeTemplate>() where r.Type == community.TypeOfCommunity select r.Role.Id).ToList();
            }
            catch (Exception ex)
            {

            }
            return roles;
        }
        public List<Int32> GetAvailableRoles(Int32 idCommunity)
        {
            List<Int32> roles = new List<Int32>();
            try
            {
                liteCommunity c = Get<liteCommunity>(idCommunity);
                if (c != null)
                    roles = (from r in Linq<lm.Comol.Core.Communities._RoleCommunityTypeTemplate>() where r.IdCommunityType == c.IdType select r.IdRole).ToList();
            }
            catch (Exception ex)
            {

            }
            return roles;
        }
        #region "Agency"

        public LazyAffiliation GetUserAffiliation(Int32 idUser, DateTime date)
        {
            LazyAffiliation result = null;
            List<LazyAffiliation> affiliations = UserAffiliations(idUser);
            if (affiliations.Any())
            {
                result = affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                  (
                      (!a.ToDate.HasValue && a.FromDate <= date))
                      || (a.FromDate <= date && a.ToDate.HasValue && a.ToDate.Value >= date)
                  ).OrderByDescending(a => a.FromDate).FirstOrDefault();

            }

            return result;
        }
        public Boolean UserHasAgencyAffiliations(List<LazyAffiliation> affiliations, DateTime date)
        {
            Boolean result = false;
            result = (affiliations.Any() && affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                (
                    (!a.ToDate.HasValue && a.FromDate <= date))
                    || (a.FromDate <= date && a.ToDate.HasValue && a.ToDate.Value >= date)
                ).Any());

            return result;
        }
        public Boolean UserHasAgencyAffiliations(Int32 idUser, DateTime date)
        {
            Boolean result = false;
            List<LazyAffiliation> affiliations = UserAffiliations(idUser);
            result = (affiliations.Any() && affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                (
                    (!a.ToDate.HasValue && a.FromDate <= date))
                    || (a.FromDate <= date && a.ToDate.HasValue && a.ToDate.Value >= date)
                ).Any());

            return result;
        }
        public Boolean UsersHasAgencyAffiliations(List<Int32> idUsers, DateTime date)
        {
            Boolean result = false;
            List<LazyAffiliation> affiliations = UsersAffiliations(idUsers);
            result = (affiliations.Any() && affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                (
                    (!a.ToDate.HasValue && a.FromDate <= date))
                    || (a.FromDate <= date && a.ToDate.HasValue && a.ToDate.Value >= date)
                ).Any());

            return result;
        }
        public Boolean UserHasAgencyAffiliations(Int32 idUser, DateTime? fromDate, DateTime? toDate)
        {
            Boolean result = false;
            List<LazyAffiliation> affiliations = UserAffiliations(idUser);
            result = (affiliations.Any() && affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                    (
                        (!a.ToDate.HasValue && toDate.HasValue && UserHasAgencyAffiliations(affiliations, toDate.Value))
                        ||
                        (fromDate.HasValue && UserHasAgencyAffiliations(affiliations, fromDate.Value))
                    )

                ).Any());

            return result;
        }
        public Boolean UsersHasAgencyAffiliations(List<Int32> idUsers, DateTime? fromDate, DateTime? toDate)
        {
            Boolean result = false;
            List<LazyAffiliation> affiliations = UsersAffiliations(idUsers);
            result = (affiliations.Any() && affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                    (
                        (!a.ToDate.HasValue && toDate.HasValue && UserHasAgencyAffiliations(affiliations, toDate.Value))
                        ||
                        (fromDate.HasValue && UserHasAgencyAffiliations(affiliations, fromDate.Value))
                    )

                ).Any());

            return result;
        }
        public List<LazyAffiliation> UserAffiliations(Int32 idUser)
        {
            List<LazyAffiliation> result = null;
            Boolean isInTransaction = IsInTransaction();
            try
            {
                if (!isInTransaction)
                    BeginTransaction();
                result = (from a in GetIQ<LazyAffiliation>() where a.IdPerson == idUser select a).ToList();
                if (!isInTransaction)
                    Commit();
            }
            catch (Exception ex)
            {
                if (!isInTransaction)
                    RollBack();
                result = new List<LazyAffiliation>();
            }
            return result;
        }
        public List<LazyAffiliation> UsersAffiliations(List<Int32> idUsers)
        {
            List<LazyAffiliation> result = new List<LazyAffiliation>();
            Boolean isInTransaction = IsInTransaction();
            try
            {
                Int32 pageSize = 100;
                Int32 pageIndex = 0;

                if (!isInTransaction)
                    BeginTransaction();

                var affiliations = (from u in GetIQ<LazyAffiliation>() select u);
                var usersQuery = idUsers.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                while (usersQuery.Any())
                {
                    result.AddRange(affiliations.Where(a => usersQuery.Contains(a.IdPerson)).ToList());

                    pageIndex++;
                    usersQuery = usersQuery.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                if (!isInTransaction)
                    Commit();
            }
            catch (Exception ex)
            {
                if (!isInTransaction)
                    RollBack();
                result = new List<LazyAffiliation>();
            }
            return result;
        }
        #endregion

        #region "Translated Items"
        public List<T> GetTranslatedItem<T>(Int32 idLanguage)
        {
            return GetNamedQuery("GetTranslated" + typeof(T).Name).SetInt32("IdLanguage", idLanguage).List<T>().ToList();
        }
        public List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> GetTranslatedRoles(Int32 idLanguage)
        {
            return GetTranslatedItem<dtoTranslatedRoleType>(idLanguage);
        }
        public List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> GetTranslatedRoles(Int32 idCommunity, Int32 idLanguage, Boolean skipGuest = true)
        {
            List<Int32> idRoles = GetAvailableRoles(idCommunity);
            if (skipGuest)
                idRoles.Remove((int)RoleTypeStandard.Guest);
            return GetTranslatedItem<dtoTranslatedRoleType>(idLanguage).Where(r => idRoles.Contains(r.Id)).OrderBy(r => r.Name).ToList();
        }
        public List<lm.Comol.Core.DomainModel.dtoTranslatedCommunityType> GetTranslatedCommunityTypes(Int32 idLanguage)
        {
            return GetTranslatedItem<dtoTranslatedCommunityType>(idLanguage);
        }
        public List<lm.Comol.Core.DomainModel.dtoTranslatedProfileType> GetTranslatedProfileTypes(Int32 idLanguage)
        {
            return GetTranslatedItem<dtoTranslatedProfileType>(idLanguage);
        }
        public String GetTranslatedRole(Int32 idRole, Int32 idLanguage)
        {
            return GetTranslatedRoles(idLanguage).Where(r => r.Id == idRole).Select(r => r.Name).DefaultIfEmpty("").FirstOrDefault();
        }
        public String GetTranslatedProfileType(Int32 idProfileType, Int32 idLanguage)
        {
            return GetTranslatedProfileTypes(idLanguage).Where(r => r.Id == idProfileType).Select(r => r.Name).DefaultIfEmpty("").FirstOrDefault();
        }
        #endregion



    }

    public static class LinqExtension
    {

        public static IList<T> ToProxySafeIList<T>(this IList<T> list)
        where T : class
        {
            if (list.Count == 0) return list;

            var proxy = list[0] as NHibernate.Proxy.INHibernateProxy;
            if (proxy != null)
            {
                list[0] = proxy.HibernateLazyInitializer.GetImplementation() as T;
            }
            return list;
        }

        public static List<T> ToProxySafeList<T>(this List<T> list)
       where T : class
        {
            if (list.Count == 0) return list;
            for (Int32 i = 0; i < list.Count; i++)
            {
                var proxy = list[i] as NHibernate.Proxy.INHibernateProxy;
                if (proxy != null)
                {
                    list[i] = proxy.HibernateLazyInitializer.GetImplementation() as T;
                }
            }

            return list;
        }
    }
}