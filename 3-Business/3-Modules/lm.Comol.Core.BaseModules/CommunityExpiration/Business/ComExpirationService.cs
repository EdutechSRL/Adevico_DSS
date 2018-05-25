using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityExpiration;
using Entity = Comol.Entity;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Business
{
    public class ComExpirationService : CoreServices
    {
        /// <summary>
        /// Definizzione service
        /// </summary>
        /// <param name="oContext">Contesto (solo Data Context)</param>
        public ComExpirationService(iDataContext oContext, Entity.DelaySubscriptionConfig SysSettings)
               : base(oContext)    //, string xmlPath
        {
            settings = SysSettings;
        }


        private Entity.DelaySubscriptionConfig _settings { get; set; }
        private Entity.DelaySubscriptionConfig settings
        {
            get
            {
                if (_settings == null)
                    _settings = new Entity.DelaySubscriptionConfig();

                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        /// <summary>
        /// Definizione service
        /// </summary>
        /// <param name="oAppContext">Contesto (data + User)</param>
        public ComExpirationService(iApplicationContext oAppContext, Entity.DelaySubscriptionConfig SysSettings)
            : base(oAppContext)    //, string xmlPath
        {
            settings = SysSettings;
        }



        public IList<Domain.dtoExpirationConfig> GetPortalExpirationConfig(
            int CommunityTypeId,
            int LangId,
            string LangCode)
        {
            List<Domain.ExpirationConfig> oldconfiguration = 
                (from Domain.ExpirationConfig conf in Manager.GetIQ<Domain.ExpirationConfig>()
                where conf.CommunityId == 0 && conf.CommunityTypeId == CommunityTypeId
                select conf
                 ).ToList();

            List<Domain.dtoExpirationConfig> config =
                oldconfiguration.Select(c => new Domain.dtoExpirationConfig(c))
                .ToList();
            
            IList<int> idRoles = Manager.GetAvailableRolesByType(CommunityTypeId);
            IList<int> oldRoles = (from Domain.dtoExpirationConfig conf in config select conf.RoleId).ToList();
            IList<int> newRoles = idRoles.Except(oldRoles).ToList();

            if(newRoles != null && newRoles.Any())
            {
                foreach(int newroleId in newRoles)
                {
                    Domain.dtoExpirationConfig newcfg = new Domain.dtoExpirationConfig();
                    newcfg.Duration = -1;
                    newcfg.RoleId = newroleId;

                    config.Add(newcfg);
                }
            }


            if (config != null && config.Any())
            {
                config.ForEach(c =>
                {
                    c.RoleName = Manager.GetTranslatedRole(c.RoleId, LangId);
                    c.DurationName = settings.GetValidityName(LangCode, c.Duration);
                }
                );
            }

            return config;
        }

        public IList<Domain.dtoExpirationConfig> GetCommunityExpirationConfig(
            int CommunityId,  
            int LangId, 
            string LangCode)
        {

            Language defLang = Manager.GetDefaultLanguage();

            if(defLang == null)
            {
                defLang = new Language();
                defLang.Code = "us-EN";
                defLang.Id = 1;
            }

            if (LangId <= 0)
            {
                LangId = defLang.Id;
            }

            if(string.IsNullOrWhiteSpace(LangCode))
            {
                LangCode = defLang.Code;
            }

            List<Domain.ExpirationConfig> oldcfg = (from Domain.ExpirationConfig conf in Manager.GetIQ<Domain.ExpirationConfig>()
                 where conf.CommunityId == CommunityId
                 select conf
                 ).ToList();

            List<Domain.dtoExpirationConfig> config = oldcfg.Select(c => new Domain.dtoExpirationConfig(c)).ToList();

            IList<int> idCommunityRoles = Manager.GetAvailableRoles(CommunityId);
            IList<int> oldRoles = (from Domain.dtoExpirationConfig conf in config select conf.RoleId).ToList();
            IList<int> newRoles = idCommunityRoles.Except(oldRoles).ToList();
            
            if (newRoles != null && newRoles.Any())
            {
                int ComTypeId = Manager.GetIdCommunityType(CommunityId);

                List<Domain.ExpirationConfig> oldsysConf =
                    (from Domain.ExpirationConfig conf in Manager.GetIQ<Domain.ExpirationConfig>()
                    where conf.CommunityId == 0 && conf.CommunityTypeId == ComTypeId
                     select conf
                    ).ToList();

                List<Domain.dtoExpirationConfig> SysConfig =
                    oldsysConf.Select(c => new Domain.dtoExpirationConfig(c))
                    .ToList();
                
                foreach (int newroleId in newRoles)
                {
                    Domain.dtoExpirationConfig newcfg = new Domain.dtoExpirationConfig();
                    newcfg.RoleId = newroleId;

                    Domain.dtoExpirationConfig sysConf = SysConfig.FirstOrDefault(c => c.RoleId == newroleId);

                    if(sysConf != null && sysConf.RoleId > 0)
                    {
                        newcfg.Duration = sysConf.Duration;
                    } else
                    {
                        newcfg.Duration = -1;
                    }  
                    
                    config.Add(newcfg);
                }
            }     

            if (config != null && config.Any())
            {
                config.ForEach(c =>
                {
                    c.RoleName = Manager.GetTranslatedRole(c.RoleId, LangId);
                    c.DurationName = settings.GetValidityName(LangCode, c.Duration);
                }
                );
            }

            return config;
        }

        public int SetExpirationsPortal(
            IList<Domain.dtoExpirationConfig> config, 
            int CommunityTypeId)
        {

            
            if (!Manager.IsInTransaction())
            Manager.BeginTransaction();

            try
            {

                IList<Domain.ExpirationConfig> actualConfig = 
                    Manager.GetAll<Domain.ExpirationConfig>(c => c.CommunityId == 0 && c.CommunityTypeId == CommunityTypeId);

                foreach(Domain.ExpirationConfig conf in actualConfig)
                {
                    Domain.dtoExpirationConfig newCfg = config.FirstOrDefault(c => c.RoleId == conf.RoleId);

                    if(newCfg != null && newCfg.RoleId == conf.RoleId && newCfg.Duration != conf.Duration)
                    {
                        conf.UpdateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                        conf.Duration = newCfg.Duration;

                        Manager.SaveOrUpdate<Domain.ExpirationConfig>(conf);
                    }

                }

                IList<int> ActualConfId = actualConfig.Select(c => c.RoleId).ToList();

                IList<Domain.dtoExpirationConfig> NewCfgs = config.Where(c => !ActualConfId.Contains(c.RoleId)).ToList();

                foreach (Domain.dtoExpirationConfig newCfg in NewCfgs)
                {
                    Domain.ExpirationConfig conf = new Domain.ExpirationConfig();
                    conf.CreateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                    conf.CommunityId = 0;
                    conf.CommunityTypeId = CommunityTypeId;
                    conf.Duration = newCfg.Duration;
                    conf.RoleId = newCfg.RoleId;
                    
                    Manager.SaveOrUpdate<Domain.ExpirationConfig>(conf);

                    actualConfig.Add(conf);
                }


                //Manager.SaveOrUpdateList<Domain.ExpirationConfig>(actualConfig);

                Manager.Commit();

                //CacheHelper.SystemRoleReset(CommunityTypeId);
               

            } catch
            {
                Manager.RollBack();
                return -1;
            }                             

            return 1;
        }

        public int SetExpirationsCommunity(
            IList<Domain.dtoExpirationConfig> config,
            int CommunityId)
        {


            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {

                IList<Domain.ExpirationConfig> actualConfig =
                    Manager.GetAll<Domain.ExpirationConfig>(c => c.CommunityId == CommunityId);

                foreach (Domain.ExpirationConfig conf in actualConfig)
                {
                    Domain.dtoExpirationConfig newCfg = config.FirstOrDefault(c => c.RoleId == conf.RoleId);

                    if (newCfg != null && newCfg.Duration != conf.Duration)
                    {
                        conf.UpdateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                        conf.Duration = newCfg.Duration;

                        Manager.SaveOrUpdate<Domain.ExpirationConfig>(conf);
                    }

                }

                IList<int> OldRoleId = actualConfig.Select(c => c.RoleId).ToList();


                foreach (Domain.dtoExpirationConfig newCfg in config.Where(c => c.Id == 0 || !OldRoleId.Contains(c.RoleId)))
                {
                    Domain.ExpirationConfig conf = new Domain.ExpirationConfig();
                    conf.CreateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                    conf.CommunityId = CommunityId;
                    conf.CommunityTypeId = 0;
                    conf.Duration = newCfg.Duration;
                    conf.RoleId = newCfg.RoleId;

                    Manager.SaveOrUpdate<Domain.ExpirationConfig>(conf);

                    actualConfig.Add(conf);
                }


                Manager.SaveOrUpdateList<Domain.ExpirationConfig>(actualConfig);

                Manager.Commit();

                CacheHelper.CommunityRoleReset(CommunityId);
            }
            catch
            {
                Manager.RollBack();
                return -1;
            }

            return 0;
        }
        

        public bool HasCommunityPermission(int CommunityId, int PersonId)
        {
            bool haspermission = false;
            
            try
            {
                string permissions = Manager.GetModulePermissionString(
                    PersonId,
                    CommunityId,
                    ModuleId);


                COL_BusinessLogic_v2.UCServices.Services_GestioneIscritti SubService = 
                    new COL_BusinessLogic_v2.UCServices.Services_GestioneIscritti(permissions);

                haspermission = SubService.Admin || SubService.Management;

            } catch { }      

            return haspermission;
        }

        public int ModuleId
        {
            get
            {
                return Manager.GetModuleID(ModuleCode);
            }
        }

        public string ModuleCode
        {         
            get
            {
                return COL_BusinessLogic_v2.UCServices.Services_GestioneIscritti.Codex;
            }
        }
      
        private IDictionary<int, int> ComRoleDuration(int CommunityId)
        {
            IDictionary<int, int> CRD = CacheHelper.CommunityRoleDurationGet(CommunityId);

            if(CRD == null || !CRD.Keys.Any())
            {
                CRD = new Dictionary<int, int>();

                IList<Domain.dtoExpirationConfig> dtoExpCom = GetCommunityExpirationConfig(CommunityId, -1, "");

                foreach(Domain.dtoExpirationConfig dtoexp in dtoExpCom)
                {
                    if(!CRD.ContainsKey(dtoexp.RoleId))
                    {
                        CRD.Add(dtoexp.RoleId, dtoexp.Duration);
                    }
                }

                CacheHelper.CommunityRoleDurationSet(CommunityId, CRD);
            }

            return CRD;
        }

        public Domain.dtoUserExpiration ComUserDurationGet(int CommunityId, int UserId, int RoleId = 0)
        {
            IDictionary<int, Domain.dtoUserExpiration> UserDuration = CacheHelper.UserDurationGet(UserId);


            //Se non c'è in cache
            if(UserDuration == null || !UserDuration.Keys.Any())
            {
                UpdateUserCommunityCache(UserId);
                UserDuration = CacheHelper.UserDurationGet(UserId);
            }

            //Se c'è in cache
            if(UserDuration != null && UserDuration.ContainsKey(CommunityId))
            {
                return UserDuration[CommunityId];
            }

            //Se non c'è in cache non è stato salvato (creato) = Controllo il ruolo.
          
            if(RoleId == 0)
            {
                RoleId =
                    (from Subscription sub
                    in Manager.GetIQ<Subscription>()
                     where sub.Community != null
                         && sub.Community.Id == CommunityId
                         && sub.Person != null
                         && sub.Person.Id == UserId
                         && sub.Role != null
                     select sub.Role.Id).Skip(0).Take(1).FirstOrDefault();

                //RoleId = Manager.GetActiveSubscriptionIdRole(CommunityId, UserId);
            }
                


            int duration = -1;


            IDictionary<int, int> CRD = ComRoleDuration(CommunityId);

            if (CRD == null || !CRD.Keys.Any())
            {
                CRD = new Dictionary<int, int>();

                IList<Domain.dtoExpirationConfig> dtoExpCom = GetCommunityExpirationConfig(CommunityId, -1, "");

                foreach (Domain.dtoExpirationConfig dtoexp in dtoExpCom)
                {
                    if (!CRD.ContainsKey(dtoexp.RoleId))
                    {
                        CRD.Add(dtoexp.RoleId, dtoexp.Duration);
                    }
                }

                CacheHelper.CommunityRoleDurationSet(CommunityId, CRD);
            }

            if (CRD.ContainsKey(RoleId))
            {
                duration = CRD[RoleId];
            }

            return new Domain.dtoUserExpiration(null, duration);
        }

        /// <summary>
        /// Recupera da DB un dcitionary con tutte le impostazioni degli utenti in una data comunità.
        /// </summary>
        /// <param name="CommunityId">Id comunità</param>
        /// <returns></returns>
        /// <remarks>
        /// Gli utenti NON impostati, NON sono presenti in lista: usare il ruolo/comunità!
        /// </remarks>
        public IDictionary<int, Domain.dtoUserExpiration> ComDurationGet(int CommunityId)
        {
            //IDictionary<int, Domain.dtoUserExpiration> dictionary = new Dictionary<int, Domain.dtoUserExpiration>;

            //IList<Domain.SubscriptionExpiration> CUDs = 
            return  Manager.GetAll<Domain.SubscriptionExpiration>(
                c => c.CommunityId == CommunityId
                && c.Deleted == BaseStatusDeleted.None)
                .Distinct()
                .ToDictionary(
                    se => se.PersonId,
                    se => new Domain.dtoUserExpiration
                    {
                        Duration = se.Duration,
                        StartDateTime = se.StartDate
                    });


        }

        public void ComUserDurationSet(int CommunityId, int UserId, int Duration, bool AddToPrevious)
        {

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            Domain.SubscriptionExpiration expiration = Manager.GetAll<Domain.SubscriptionExpiration>(exp =>
                           exp.Deleted == BaseStatusDeleted.None
                           && exp.CommunityId == CommunityId
                           && exp.PersonId == UserId)
                            .OrderByDescending(exp => exp.CreatedOn)
                            .FirstOrDefault();

            if(expiration == null || expiration.Id <= 0)
            {
                expiration = new Domain.SubscriptionExpiration();
                expiration.CreateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                expiration.PersonId = UserId;
                expiration.CommunityId = UC.CurrentCommunityID;
            } else
            {
                expiration.UpdateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
            }

            if(AddToPrevious && Duration >= 0)
            {
                expiration.Duration += Duration;
            }
            else
            {
                expiration.Duration = Duration;
            }

            //if (expiration.StartDate != null)
            //{
            //    expiration.EndDate = expiration.StartDate + new TimeSpan(Duration, 0, 0, 0);
            //}

            bool saved = false;

            try
            {
                Manager.SaveOrUpdate<Domain.SubscriptionExpiration>(expiration);
                Manager.Commit();
                saved = true;
            }  catch(Exception ex)
            {
                Manager.RollBack();
            }

            if(saved)
            {
                CacheHelper.UserReset(UserId);
            }

        }

        public void ComUserStartUpdate(int CommunityId, IList<int> UsersId, bool Start)
        {
            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            IList<Domain.SubscriptionExpiration> expirations = new List<Domain.SubscriptionExpiration>();

            UsersId = UsersId.Distinct().ToList();

            int counter = UsersId.Count;

            if (counter > 500)
            {
                int block = (counter / 500) + 1;

                for (int curblock = 0; curblock < block; curblock++)
                {
                    IList<int> currentId = UsersId.Skip(500 * curblock).Take(500).ToList();
                    IList<Domain.SubscriptionExpiration> curExps = Manager.GetAll<Domain.SubscriptionExpiration>(se => currentId.Contains(se.PersonId));
                    expirations = expirations.Union(curExps).ToList();
                }

                expirations = expirations.Distinct().ToList();

            } else
            {
                expirations = Manager.GetAll<Domain.SubscriptionExpiration>(exp =>
                   exp.Deleted == BaseStatusDeleted.None
                   && exp.CommunityId == CommunityId
                   && UsersId.Contains(exp.PersonId));
            }

            IList<int> settedUsersId = expirations.Select(exp => exp.PersonId).Distinct().ToList();
            IList<int> unSettedUsersId = UsersId.Except(settedUsersId).ToList();

            if(unSettedUsersId != null && unSettedUsersId.Any())
            {
                foreach (int UserIdNotSet in unSettedUsersId)
                {
                    Domain.SubscriptionExpiration expiration = new Domain.SubscriptionExpiration();
                    expiration.CreateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                    expiration.CommunityId = CommunityId;
                    expiration.PersonId = UserIdNotSet;

                    Domain.dtoUserExpiration exp = ComUserDurationGet(CommunityId, UserIdNotSet);
                    expiration.Duration = exp.Duration;

                    expirations.Add(expiration);
                }
            }

            DateTime StartDt = DateTime.Now;

            foreach(Domain.SubscriptionExpiration exp in expirations)
            {
                if(Start && exp.Duration >= 0)
                {
                    exp.StartDate = StartDt;
                    //exp.EndDate = StartDt.AddDays(exp.Duration);
                } else
                {
                    exp.StartDate = null;
                    //exp.EndDate = null;
                }

            }

            bool saved = false;

            try
            {
                Manager.SaveOrUpdateList<Domain.SubscriptionExpiration>(expirations);
                Manager.Commit();
                saved = true;
            }
            catch
            {
                Manager.RollBack();
            }

            if (saved)
            {
                CacheHelper.UserReset();
            }
        }

        private void UpdateUserCommunityCache(int UserId)
        {
            //Recupero da cache l'elenco per l'utente.
            //In sua assenza lo ricreo ex-novo, con chiave ID utente che contiene un dictionary di chiave (Id Comunità) e valore (durata).

            IList<Domain.SubscriptionExpiration> CUDs = Manager.GetAll<Domain.SubscriptionExpiration>(
                c => c.PersonId == UserId
                && c.Deleted == BaseStatusDeleted.None);
            
            Dictionary<int, Domain.dtoUserExpiration> value = new Dictionary<int, Domain.dtoUserExpiration>();

            foreach(Domain.SubscriptionExpiration se in CUDs)
            {
                if(!value.ContainsKey(se.CommunityId))
                {
                    value.Add(se.CommunityId, new Domain.dtoUserExpiration(se.StartDate, se.Duration));
                }
            }

            CacheHelper.UserDurationSet(UserId, value);
        }

        public void ClearCache()
        {
            CacheHelper.CacheReset();
        }
    }  
}
