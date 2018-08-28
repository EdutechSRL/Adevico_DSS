using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.DTO;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
//using lm.Comol.Modules.ScormStat;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using NHibernate.Transform;

namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices
    {

        #region Global Settigns

        /// <summary>
        /// Imposta parametri globali per l'utente
        /// </summary>
        /// <param name="userId">User ID: l'utente DEVE essere già presente come USER, altrimenti usare SettingsSetGlobalPerson</param>
        /// <param name="settings">Impostazioni Notifiche</param>
        /// <param name="notificationUserEnabled"></param>
        /// <param name="notificationManagerEnabled"></param>
        /// <returns></returns>
        public bool SettingsSetGlobalUser(
            Int64 userId,
            Domain.Enums.MailSettings settings,
            bool notificationUserEnabled,
            bool notificationManagerEnabled
            )
        {

            TicketUser user = this.UserGet(userId); // che fa questo: Manager.Get<TicketUser>(UserId);
            
            if (user == null && user.Id <= 0)
                return false;

            MailNotification mailNotification = Manager.GetAll<MailNotification>(mn => mn.User.Id == userId && mn.IsPortal == true).Skip(0).Take(1).ToList().FirstOrDefault();

            if (mailNotification == null)
            {
                mailNotification = new MailNotification();
                mailNotification.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                mailNotification.User = user;
            }
            else
                mailNotification.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if(settings == MailSettings.Default)
                settings = MailSettings.UserDefault;

            mailNotification.IsPortal = true;
            mailNotification.Ticket = null;
            mailNotification.Settings = settings;

            user.IsNotificationActiveUser = notificationUserEnabled;
            user.IsNotificationActiveManager = notificationManagerEnabled;
            user.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            //SEMPRE, perchè da portale SE IMPOSTATE prendo quelle utente!
            mailNotification.IsDefaultUser = false;

            try
            {
                Manager.SaveOrUpdate<MailNotification>(mailNotification);
                Manager.SaveOrUpdate<TicketUser>(user);
            }
            catch
            {
                return false;
            }

            return true;
        }



        ///// <summary>
        ///// Impostazioni globali per una Person
        ///// </summary>
        ///// <param name="PersonId">Id Persona</param>
        ///// <param name="Settings">Impostazioni Notifiche</param>
        ///// <returns></returns>
        //public bool SettingsSetGlobalPerson(
        //    Int32 PersonId,
        //    Domain.Enums.MailSettings Settings
        //    )
        //{
        //    TicketUser User = Manager.GetAll<TicketUser>(u => u.Person != null && u.Person.Id == PersonId).Skip(0).Take(1).ToList().FirstOrDefault();

        //    Int64 UserId = 0;

        //    if (User == null && User.Id <= 0)
        //    {
        //        UserId = UserCreateFromPerson(PersonId, Settings).Id;
        //    }
        //    else
        //        UserId = User.Id;

        //    return SettingsSetGlobalUser(UserId, Settings);
        //}


        

        ///// <summary>
        ///// Aggiorna le impostazioni di notifica per un utente.
        ///// </summary>
        ///// <param name="TicketId">Id Ticket</param>
        ///// <param name="UserId">Id Utente</param>
        ///// <param name="Notifications">Impostazioni notifiche</param>
        ///// <returns></returns>
        //public bool SettingsNotificationUpdateUser(
        //        Int64 TicketId,
        //        Int64 UserId,
        //        Domain.Enums.MailSettings Notifications
        //        )
        //{
        //    return true;
        //}

        /// <summary>
        /// Imposta parametri globali
        /// </summary>
        /// <param name="HasExternalLimitation">Se ci sono limitazioni per gli esterni</param>
        /// <param name="MaxTicketExternal">Limite di ticket aperti da utenti esterni</param>
        /// <param name="HasInternalLimitation">Se ci sono limitazioni per gli interni</param>
        /// <param name="MaxTicketInternal">Limite di ticket aperti da utente interni</param>
        /// <param name="IsServiceEnable">Se il servizio è abilitato (forse inutile)</param>
        /// <returns></returns>
        public bool SettingsGlobalSet(
            bool HasExternalLimitation,
            int MaxTicketExternal,
            bool HasInternalLimitation,
            int MaxTicketInternal,
            bool HasDraftLimitation,
            int MaxDraft,
            IList<Domain.SettingsComType> ComTypeSettings,
            Domain.Enums.MailSettings MailSets,
            Domain.DTO.DTO_Access Access,
            Domain.DTO.DTO_SettingsPermissionList BehalfPermission)
        {

            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();

            bool success = true;

            SettingsPortal settings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();

            if (settings == null)
            {
                settings = new SettingsPortal();
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                settings.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            if (MaxTicketExternal >= 0)
            {
                settings.ExternalLimitation = MaxTicketExternal;
                settings.HasExternalLimitation = HasExternalLimitation;    
            }
            if (MaxTicketInternal >= 0)
            {
                settings.HasInternalLimitation = HasInternalLimitation;
                settings.InternalLimitation = MaxTicketInternal;    
            }
            if (MaxDraft >= 0)
            {
                settings.HasDraftLimitation = HasDraftLimitation;
                settings.DraftLimitation = (MaxDraft); // <= 1) ? 1 : MaxDraft;     GEstito fuori!
            }
            

            settings.IsActive = ModuleIsActive();

            if (!settings.IsActive)
            {
                settings.CanCreateCategory = false;
                settings.CanShowTicket = false;
                settings.CanEditTicket = false;
            }
            else
            {
                settings.CanCreateCategory = Access.CanManageCategory;
                settings.CanShowTicket = Access.CanShowTicket;
                settings.CanEditTicket = Access.CanEditTicket;
            }

            if (settings.CanShowTicket == false)
                settings.CanEditTicket = false;

            //settings.MailSettings = MailSets;

            Manager.SaveOrUpdate<SettingsPortal>(settings);

            foreach (SettingsComType SCT in ComTypeSettings)
            {
                SettingsComType OldSCT = Manager.Get<SettingsComType>(SCT.Id);
                OldSCT.CreatePrivate = SCT.CreatePrivate;
                OldSCT.CreatePublic = SCT.CreatePublic;
                OldSCT.CreateTicket = SCT.CreateTicket;
                OldSCT.ViewTicket = SCT.ViewTicket;
                OldSCT.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                Manager.SaveOrUpdate<SettingsComType>(OldSCT);
            }


            //= PermissionType.Behalf;

            //GET da dB => prendo TUTTO quello che mi serve (Behalf, not deleted)
            IList<Domain.SettingsPermission> sysPerm =
                Manager.GetAll<Domain.SettingsPermission>(p =>
                    p.Deleted == BaseStatusDeleted.None);

            IList<Domain.SettingsPermission> PermissionToUpadate =
                PermissionSetForSaveOrUpdate(ref sysPerm, PermissionType.Behalf, BehalfPermission.PersonTypePermission, BehalfPermission.UserPermission);

            Manager.SaveOrUpdateList<Domain.SettingsPermission>(PermissionToUpadate);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                if(Manager.IsInTransaction())
                    Manager.RollBack();
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Crea una lista di oggetti SettingsPermission da passare a Manager.SaveOrUpdate
        /// con gli oggetti da aggiungere e quelli da modificare con il DELETED e tutte le relative metainfo.
        /// </summary>
        /// <param name="sysPerm"></param>
        /// <param name="PermType">Tipo permission</param>
        /// <returns></returns>
        private IList<Domain.SettingsPermission> PermissionSetForSaveOrUpdate(
            ref IList<Domain.SettingsPermission> systemPermission,
            PermissionType PermType,
            IList<Domain.DTO.DTO_SettingsPermissionPersonType> DTOptPermissions,
            IList<Domain.DTO.DTO_SettingsPermissionUsers> DTOusrPermissions)
        {

            IList<Domain.SettingsPermission> sysPerm = systemPermission.Where(sp => sp.PermissionType == PermType).ToList();

            //Divido tra PersonType e Users
            IList<Domain.SettingsPermission> sysPermPT =
                sysPerm.Where(spt => spt.PersonTypeId > 0).ToList();

            IList<Domain.SettingsPermission> sysPermUsr =
                sysPerm.Except(sysPermPT).ToList();

            DTOptPermissions = DTOptPermissions.Where(ptp =>
                   (int)ptp.PersonTypeId != PersonExternal.TypeID
                    && (int)ptp.PersonTypeId != (int)UserTypeStandard.TypingOffice
                    && (int)ptp.PersonTypeId != (int)UserTypeStandard.Guest
                    && (int)ptp.PersonTypeId != (int)UserTypeStandard.PublicUser
                ).ToList();


            // PERSON TYPE
            IList<Domain.SettingsPermission> DELETEpermList =
                sysPermPT.Where(dsp => dsp.PersonTypeId != null 
                    && (int)dsp.PersonTypeId > 0
                    && DTOptPermissions.All(ptp => ptp.PersonTypeId != (int)dsp.PersonTypeId))
                .ToList();



            foreach (Domain.SettingsPermission dsp in DELETEpermList)
            {
                dsp.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            //.ForEach(sp => sp.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress))


            // Quelli da aggiungere sono TUTTI quelli che mi sono stati dati ESCLUSI quelli che ho già su dB

            IList<Domain.SettingsPermission> ADDpermList =
                (from Domain.DTO.DTO_SettingsPermissionPersonType Dspt in DTOptPermissions
                 where sysPermPT.All(sPT => (Int32)sPT.PersonTypeId != Dspt.PersonTypeId)
                 select new Domain.SettingsPermission
                 {
                     PersonTypeId = Dspt.PersonTypeId,
                     PermissionType = PermType,
                 }
                    )
                    .ToList();

            foreach (Domain.SettingsPermission dsp in ADDpermList)
            {
                dsp.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            }

            IList<Domain.SettingsPermission> ReturnPermission = DELETEpermList.Union(ADDpermList).ToList();

            systemPermission = systemPermission.Except(ReturnPermission).ToList();

            return ReturnPermission;
        }



        /// <summary>
        /// Recupera le impostazioni globali
        /// </summary>
        /// <param name="LoadCategories">Indica se caricare o meno l'elenco categorie</param>
        /// <param name="PreselectCategoryID">SE = 0, preseleziona la categoria di default, altrimenti quella indicata.</param>
        /// <returns></returns>
        /// <remarks>PreselectCategoryID viene impostata sulla selezione dell'utente nella pagina GlobalAdmin, per aggiornare il controllo, mantenendo la selezione. Altrimenti usare semplicemente 0!</remarks>
        public SettingsPortal SettingsGlobalGet(bool LoadCategories, Int64 PreselectCategoryID)
        {
            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();


            SettingsPortal settings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();

            if (settings == null)
            {
                settings = new SettingsPortal();
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                settings.ExternalLimitation = 0;
                settings.HasExternalLimitation = false;
                settings.HasInternalLimitation = false;
                settings.InternalLimitation = 0;
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                settings.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }


            settings.IsActive = ModuleIsActive();
            if (!settings.IsActive)
            {
                settings.CanCreateCategory = false;
                settings.CanShowTicket = false;
                settings.CanEditTicket = false;
            }

            if (settings.CanShowTicket == false)
            {
                settings.CanEditTicket = false;
            }

            if (settings.CanEditTicket == false)
            {
                settings.CanBehalf = false;
            }

            settings.CommunityTypeSettings = Manager.GetAll<SettingsComType>().ToList();

            settings.PublicCategories = CategoriesGetTreeDLL(0, CategoryTREEgetType.System);

            if (PreselectCategoryID > 0)
                settings.CategoryDefault = CategoryGetDTOCatTree(PreselectCategoryID);
            else
                settings.CategoryDefault = CategoryGetDTOCatTree(this.CategoryDefaultGetID());

            try
            {
                Manager.SaveOrUpdate<SettingsPortal>(settings);
                Manager.Commit();
            }
            catch (Exception)
            {
                if(Manager.IsInTransaction())
                    Manager.RollBack();

            }
            

            //if (LoadCategories)
            //{
            //    settings.PublicCategories = this.CategoriesGetSystem();
            //}

            //if (settings.PublicCategories == null || settings.PublicCategories.Count <= 0)
            //{
            //    settings.PublicCategories = new List<Category>();
            //}
            Manager.Detach(settings);
            return settings;
        }

        public Domain.DTO.DTO_Access SettingsAccessGet(bool IsInternal)
        {
            bool IsActive = ModuleIsActive();

            SettingsPortal sp = Manager.GetAll<SettingsPortal>().FirstOrDefault();
            if (sp == null)
                sp = new SettingsPortal();

            return new Domain.DTO.DTO_Access
            {
                CanManageCategory = sp.CanCreateCategory & IsActive,
                CanShowTicket = sp.CanShowTicket & IsActive,
                CanEditTicket = sp.CanEditTicket & IsActive & sp.CanShowTicket,
                IsActive = IsActive,
                MaxDraft = (sp.HasDraftLimitation)? sp.DraftLimitation : -1,
                MaxSended = (IsInternal)?
                    ((sp.HasInternalLimitation)? sp.InternalLimitation : -1) :
                    ((sp.HasExternalLimitation)? sp.ExternalLimitation : -1)
            };


            //return (
            //        select new Domain.DTO.DTO_Access {
            //             CanManageCategory = sp.CanCreateCategory & IsActive,
            //             CanShowTicket = sp.CanShowTicket & IsActive,
            //             CanEditTicket = sp.CanEditTicket & IsActive & sp.CanShowTicket,
            //             IsActive = IsActive
            //        }).Skip(0).Take(1).FirstOrDefault();

        }

        /// <summary>
        /// Controlla a livello di sistema se il servizio è attivo
        /// </summary>
        /// <returns>True se è attivo</returns>
        /// <remarks>To Do!!!!</remarks>
        private bool ModuleIsActive()
        {
            return Manager.IsModuleActive(ModuleTicket.UniqueCode);
        }

        /// <summary>
        /// Inizializza le impostazioni iniziali o le aggiorna importando i nuovi Tipi Comunità.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///  - Recupera tutti i tipi comunità che non sono presenti nel "TicketSystem" e li aggiunge con i relativi Settings (tutto a FALSE).
        /// </remarks>
        public bool SettingsSystemRefresh()
        {
            IList<int> SetComTypesIds = (from SettingsComType sct in Manager.GetIQ<SettingsComType>()
                                         select sct.CommunityType.Id).ToList();

            IList<CommunityType> ComTypes = Manager.GetAll<CommunityType>(ct => !(SetComTypesIds.Contains(ct.Id))).ToList();

            if (ComTypes.Any())
            {

                IList<SettingsComType> Scts = new List<SettingsComType>();

                foreach (CommunityType ct in ComTypes)
                {
                    SettingsComType sct = new SettingsComType();
                    sct.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                    sct.CommunityType = ct;
                    sct.CreatePrivate = false;
                    sct.CreatePublic = false;
                    sct.CreateTicket = false;
                    sct.ViewTicket = false;

                    Scts.Add(sct);
                }

                if (Scts.Any())
                {
                    Manager.SaveOrUpdateList<SettingsComType>(Scts);
                }
            }

            return true;
        }

        #endregion


        #region Mail settings

        /// <summary>
        /// Mail settings per uno specifico ticket
        /// </summary>
        /// <param name="UserId">Id USER (Ticket)</param>
        /// <param name="CommunityId">Id Comunità</param>
        /// <param name="TicketId">Id Ticket</param>
        /// <returns>
        /// Mail settings del ticket specifico,
        /// altrimenti della comunità,
        /// altrimenti del portale.
        /// </returns>
        public Domain.Enums.MailSettings MailSettingsGet(Int64 UserId, Int64 TicketId, bool IsManager, ref bool isDefault) //int CommunityId, 
        {
            Domain.Enums.MailSettings mailSettings = MailSettings.none;

            //Notifiche utente/Ticket
            Domain.MailNotification mailNot =
                (from Domain.MailNotification mn in Manager.GetIQ<Domain.MailNotification>()
                    where mn.User.Id == UserId &&
                          mn.Ticket != null &&
                          mn.Ticket.Id == TicketId &&
                          mn.IsPortal == false
                    select mn).FirstOrDefault();

            if (mailNot != null)
            {
                if (IsManager)
                {
                    isDefault = mailNot.IsDefaultManager;
                }
                else
                {
                    isDefault = mailNot.IsDefaultUser;
                }
            }
            else
            {
                isDefault = true;
            }


            //Non ho trovato quelle per utente+Ticket : Recupero valori di default!
            if(isDefault || mailNot == null)
            {
                isDefault = true;

                //Impostazioni dell'utente
                mailNot =
                    (from Domain.MailNotification mn in Manager.GetIQ<Domain.MailNotification>()
                     where mn.User.Id == UserId &&
                     mn.Ticket == null &&
                     mn.IsPortal
                     select mn).FirstOrDefault();

                if (mailNot == null)
                {
                    mailNot = new Domain.MailNotification();
                }

                //Recupero le impostazioni di portale.
                if ((mailNot.IsDefaultUser && !IsManager)
                    || (mailNot.IsDefaultManager && IsManager))
                {
                    MailSettingsGetPortal(IsManager);
                }
            }

            mailSettings = mailNot.Settings;
            //if (isDefault)
            //{
            //    mailSettings = MailSettingsGet(UserId);
            //}
            //else
            //{
            //    mailSettings = mailNot.Settings;
            //}

            return mailSettings;
        }


        public MailSettings MailSettingsGetPortal(bool isForManager)
        {
            SettingsPortal setPortal = this.PortalSettingsGet();
            return (isForManager) ? setPortal.MailSettingsManager : setPortal.MailSettingsUser;
            
        }
        //public Domain.Enums.MailSettings MailSettingsGet(Int64 UserId, Int64 TicketId) //int CommunityId, 
        //{
        //    Domain.Enums.MailSettings sets =
        //        (from Domain.MailNotification mn in Manager.GetIQ<Domain.MailNotification>()
        //         where mn.User.Id == UserId &&
        //         mn.Ticket != null &&
        //         mn.Ticket.Id == TicketId &&
        //         mn.IsPortal == false
        //         select mn.Settings).FirstOrDefault();

        //    if (sets == Domain.Enums.MailSettings.Default)
        //    {
        //        sets = MailSettingsGet(UserId); //, CommunityId);
        //    }

        //    return sets;
        //}

        ///// <summary>
        ///// Mail settings per uno specifico ticket
        ///// </summary>
        ///// <param name="UserId">Id USER (Ticket)</param>
        ///// <param name="CommunityId">Id Comunità</param>
        ///// <returns>
        ///// Mail settings del portale.
        ///// </returns>
        //public Domain.Enums.MailSettings MailSettingsGet(Int64 UserId)
        //{
        //    Domain.Enums.MailSettings sets = Domain.Enums.MailSettings.Default;

        //    Domain.MailNotification notif =
        //        (from Domain.MailNotification mn in Manager.GetIQ<Domain.MailNotification>()
        //         where mn.User.Id == UserId &&
        //         mn.IdCommunity <= 0 &&
        //         mn.Ticket == null &&
        //         mn.IsPortal == true
        //         select mn).FirstOrDefault();

        //    if (notif == null)
        //    {
        //      
        //    }
        //    else
        //    {
        //        sets = notif.Settings;
        //    }


        //    return sets;
        //}

        /// <summary>
        /// Imposta i mail settings di un utente a livello di portale
        /// </summary>
        /// <param name="MailSettings"></param>
        public bool MailSettingsSetPortal(Domain.Enums.MailSettings MailSettings, Int64 UserId = -1)
        {
            Int64 _usrId = UserId;
            if (_usrId <= 0)
                _usrId = this.CurrentUser.Id;

            Domain.MailNotification MailNot = Manager.GetAll<Domain.MailNotification>(mn => mn.IsPortal == true && mn.User != null && mn.User.Id == _usrId).FirstOrDefault();

            if (MailNot == null)
            {
                MailNot = new MailNotification();
                MailNot.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                MailNot.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            MailNot.IdCommunity = 0;
            MailNot.IsPortal = true;
            MailNot.Settings = MailSettings;
            MailNot.Ticket = null;

            if (UserId <= 0)
                MailNot.User = CurrentUser;
            else
            {
                Domain.TicketUser User = Manager.Get<Domain.TicketUser>(UserId);
                if (User != null)
                    MailNot.User = User;
                else
                    return false;
            }

            try
            {
                Manager.SaveOrUpdate<Domain.MailNotification>(MailNot);
            }
            catch
            {
                return false;
            }

            return true;
        }





        /// <summary>
        /// Imposta i mail settings di comunità per un utente
        /// </summary>
        /// <param name="MailSettings"></param>
        /// <param name="CommunityId"></param>
        public bool MailSettingsSetCommunity(Domain.Enums.MailSettings MailSettings, int CommunityId, Int64 UserId = -1)
        {
            Int64 _usrId = UserId;
            if (_usrId <= 0)
                _usrId = this.CurrentUser.Id;

            Domain.MailNotification MailNot = Manager.GetAll<Domain.MailNotification>(mn => mn.IsPortal == false && mn.User != null && mn.User.Id == _usrId && mn.IdCommunity == CommunityId).FirstOrDefault();

            if (MailNot == null)
            {
                MailNot = new MailNotification();
                MailNot.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                MailNot.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            MailNot.IdCommunity = CommunityId;
            MailNot.IsPortal = false;
            MailNot.Settings = MailSettings;
            MailNot.Ticket = null;

            if (UserId <= 0)
                MailNot.User = CurrentUser;
            else
            {
                Domain.TicketUser User = Manager.Get<Domain.TicketUser>(UserId);
                if (User != null)
                    MailNot.User = User;
                else
                    return false;
            }

            try
            {
                Manager.SaveOrUpdate<Domain.MailNotification>(MailNot);
            }
            catch
            {
                return false;
            }

            return true;
        }

        ///// <summary>
        ///// Imposta i parametri mail di un utente a livello di Ticket
        ///// </summary>
        ///// <param name="MailSettings"></param>
        ///// <param name="Ticket"></param>
        //private bool MailSettingsSetTicket(Domain.Enums.MailSettings MailSettings, Ticket Ticket, Int64 UserId = -1)
        //{
        //    if (Ticket == null)
        //        return false;

        //    Int64 _usrId = UserId;
        //    if (_usrId <= 0)
        //        _usrId = this.CurrentUser.Id;

        //    Domain.MailNotification MailNot = Manager.GetAll<Domain.MailNotification>(mn => mn.IsPortal == false && mn.User != null && mn.User.Id == _usrId && mn.Ticket.Id == Ticket.Id).FirstOrDefault();

        //    if (MailNot == null)
        //    {
        //        MailNot = new MailNotification();
        //        MailNot.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //    }
        //    else
        //    {
        //        MailNot.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //    }

        //    MailNot.IdCommunity = 0;
        //    MailNot.IsPortal = false;
        //    MailNot.Settings = MailSettings;
        //    MailNot.Ticket = Ticket;

        //    if (UserId <= 0)
        //        MailNot.User = CurrentUser;
        //    else
        //    {
        //        Domain.TicketUser User = Manager.Get<Domain.TicketUser>(UserId);
        //        if (User != null)
        //            MailNot.User = User;
        //        else
        //            return false;
        //    }

        //    try
        //    {
        //        Manager.SaveOrUpdate<Domain.MailNotification>(MailNot);
        //    }
        //    catch { return false; }

        //    return true;

        //}
        
        ///// <summary>
        ///// Imposta i parametri mail di un utente a livello di Ticket
        ///// </summary>
        ///// <param name="MailSettings"></param>
        ///// <param name="TicketID"></param>
        //public bool MailSettingsSetTicket(Domain.Enums.MailSettings MailSettings, Int64 TicketID, Int64 UserId = -1)
        //{
        //    Domain.Ticket Ticket = Manager.Get<Ticket>(TicketID);
        //    if (Ticket == null)
        //        return false;

        //    Domain.MailNotification MailNot = Manager.GetAll<Domain.MailNotification>(mn => mn.IsPortal == true && mn.User != null && mn.User.Id == this.CurrentUser.Id && mn.Ticket.Id == TicketID).FirstOrDefault();

        //    if (MailNot == null)
        //    {
        //        MailNot = new MailNotification();
        //        MailNot.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //    }
        //    else
        //    {
        //        MailNot.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //    }

        //    MailNot.IdCommunity = 0;
        //    MailNot.IsPortal = false;
        //    MailNot.Settings = MailSettings;
        //    MailNot.Ticket = Ticket;

        //    if (UserId <= 0)
        //        MailNot.User = CurrentUser;
        //    else
        //    {
        //        Domain.TicketUser User = Manager.Get<Domain.TicketUser>(UserId);
        //        if (User != null)
        //            MailNot.User = User;
        //        else
        //            return false;
        //    }

        //    try
        //    {
        //        Manager.SaveOrUpdate<Domain.MailNotification>(MailNot);
        //    }
        //    catch { return false; }

        //    return true;
        //}

        /// <summary>
        /// Imposta i parametri mail di un utente a livello di Ticket
        /// </summary>
        /// <param name="MailSettings"></param>
        /// <param name="TicketID"></param>
        public int MailSettingsSetALL(Domain.Enums.MailSettings MailSettings)
        {
            //To Do

            return -1;
        }
        #endregion

        /// <summary>
        /// DTO con la lista Utenti e Typi persona già internazionalizzati
        /// </summary>
        /// <param name="idLanguage">ID lingua per l'internazionalizzazione</param>
        /// <returns></returns>
        public Domain.DTO.DTO_SettingsPermissionList SettingsPermissionGet(int idLanguage)
        {
            
            Domain.DTO.DTO_SettingsPermissionList prm = new DTO_SettingsPermissionList();

            IList<Domain.liteSettingsPermission> sysPerm = Manager.GetAll<Domain.liteSettingsPermission>(p =>
                    p.Deleted == BaseStatusDeleted.None &&
                    p.PermissionType == PermissionType.Behalf);

            //if (!sysPerm.Any())
            //    return prm;

            IDictionary<Int32, Boolean> selectedPersonType =
                sysPerm.Where(p => p.PersonTypeId != null && p.PersonTypeId > 0)
                    .Distinct()
                    .ToDictionary(p => (Int32)p.PersonTypeId, p => true);

            List<dtoTranslatedProfileType> PersonTypes = Manager.GetTranslatedProfileTypes(idLanguage);

            PersonTypes = PersonTypes.Where(pt => pt.Id != PersonExternal.TypeID
                && pt.Id != (int)UserTypeStandard.TypingOffice
                && pt.Id != (int)UserTypeStandard.Guest
                && pt.Id != (int)UserTypeStandard.PublicUser
                )
                .ToList();
            
            //&& pt.Id != (int)UserTypeStandard.ExternalUser
            
            prm.PersonTypePermission = (from dtoTranslatedProfileType dTP in PersonTypes
                                        select new DTO_SettingsPermissionPersonType
                                        {
                                            PersonTypeId = dTP.Id,
                                            DisplayName = dTP.Name,
                                            IsSelected = selectedPersonType.ContainsKey(dTP.Id)
                                        }).ToList();

            IDictionary<Int32, dtoTranslatedProfileType> personTypeDictionary =
                PersonTypes.Distinct().ToDictionary(pt => pt.Id, pt => pt);


            prm.UserPermission = (from Domain.liteSettingsPermission lp in sysPerm
                where lp.User != null && lp.User.Id > 0 && lp.User.Person != null && lp.User.Person.Id > 0
                select new DTO_SettingsPermissionUsers
                {
                    Id = lp.Id,
                    UserId = lp.User.Id,
                    PersonId = lp.User.Person.Id,
                    DisplayName = lp.User.Person.SurnameAndName,
                    PersonType = (lp.User.Person.TypeID > 0)
                        ? personTypeDictionary[lp.User.Person.TypeID].Name
                        : "--"
                }).OrderBy(p => p.DisplayName).ToList();


            //IDictionary<int, string> personType = Manager.GetTranslatedProfileTypes(idLanguage).Distinct().ToDictionary(t => t.Id, t => t.Name);

            
                
                
                
                //(from Domain.liteSettingsPermission lp in sysPerm
                //          where lp.PersonTypeId != null && lp.PermissionType > 0
                //                        select new DTO_SettingsPermission
                //                        {
                //                            Id = lp.Id,
                //                            UserId = -1,
                //                            PersonTypeId = (int)lp.PersonTypeId,
                //                            ObjectDisplayName = personType[(int)lp.PersonTypeId]
                //                        }).ToList();

            return prm;
        }

        public bool SettingPermissionUserAdd(IList<Int32> personIds, PermissionType permission)
        {

            IList<Int32> enabledUserId = (from liteSettingsPermission per in Manager.GetIQ<liteSettingsPermission>()
                where per.User != null 
                    && per.User.Person != null 
                    && per.User.Person.Id > 0 
                    && per.PermissionType == permission
                    && per.Deleted == BaseStatusDeleted.None
                select per.User.Person.Id).ToList();

            personIds = personIds.Except(enabledUserId).ToList();

            if (!personIds.Any())
                return true;

            //Utenti già iscritti
            IList<liteUser> users = Manager.GetAll<liteUser>(tu =>
                tu.Person != null && tu.Person.Id > 0
                && personIds.Contains(tu.Person.Id));

            //Lista ID utenti non iscritti
            IList<Int32> noTicktPersonIds = personIds.Except(from liteUser usr in users select usr.Person.Id).ToList();
    
            
                //Where(pid =>
                //!users.Contains(u => u.)
                //(ou => ou.Person.Id == pid)).ToList();

            //IList<liteUser> newUsers = new List<liteUser>();

            foreach (Int32 pID in noTicktPersonIds)
            {
                TicketUser usr = this.UserCreateFromPerson(pID, 0);
                if (usr != null)
                {
                    users.Add(UserConvetToLiteUser(usr));    
                }
            }

            IList<SettingsPermission> newPermission = new List<SettingsPermission>();

            foreach (liteUser ltUser in users)
            {
                SettingsPermission perm = new SettingsPermission();
                perm.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                perm.PermissionType = permission;
                perm.PersonId = ltUser.Person.Id;
                perm.User = ltUser;

                newPermission.Add(perm);
            }

            if (!newPermission.Any())
                return true;

            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();

            Manager.SaveOrUpdateList<SettingsPermission>(newPermission);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Rimuove un permesso ad un utente
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public Int64 SettingPermissionUserDelete(Int64 permissionId)
        {
            Int64 PermissionId = -1;

            Domain.SettingsPermission perm = Manager.Get<Domain.SettingsPermission>(permissionId);

            if (perm == null)
                return -1;

            PermissionId = perm.Id;

            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();


            perm.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            
            Manager.SaveOrUpdate<SettingsPermission>(perm);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                PermissionId = -1;
                Manager.RollBack();
                //return false;
            }

            return PermissionId;
        }

        public bool SettingPermissionGet(Domain.Enums.PermissionType permissionType)
        {
            return SettingPermissionGet(this.CurrentUser.Id, this.CurrentPerson.TypeID, permissionType);
        }

        public bool SettingPermissionGet(Int64 userId, Int32 userTypeId, Domain.Enums.PermissionType permissionType)
        {
            if (userId <= 0 || userTypeId <= 0)
                return false;

            bool hasPermission =
                (from liteSettingsPermission perm
                    in Manager.GetIQ<liteSettingsPermission>()
                    where (perm.Deleted == BaseStatusDeleted.None
                        && perm.PermissionType == permissionType
                        && (
                            (perm.PersonTypeId == userTypeId) ||
                            (perm.User != null && perm.User.Id == userId)
                        )
                    )
                    select perm.Id
                ).Any();


            SettingsPortal portalSetting = SettingsGlobalGet(false, 0);
            
            return hasPermission && portalSetting.CanBehalf;
        }

        public bool SettingsPermissionIsBehalfer(Int64 userId)
        {
            if (userId <= 0)
                return false;

            return 
                (from liteSettingsPermission perm
                    in Manager.GetIQ<liteSettingsPermission>()
                 where (perm.Deleted == BaseStatusDeleted.None
                     && perm.PermissionType == PermissionType.Behalf
                     && perm.User != null && perm.User.Id == userId
                 )
                 select perm.Id
                ).Any();

        }

        public bool SettingPermissionGet(Int64 userId, Int32 userTypeId, Domain.Enums.PermissionType permissionType, Int64 permissionValue)
        {
            bool hasPermission =
                (from liteSettingsPermission perm
                    in Manager.GetIQ<liteSettingsPermission>()
                    where (perm.Deleted == BaseStatusDeleted.None
                        && perm.PermissionType == permissionType
                        && perm.PermissionValue == permissionValue
                        && (
                            (perm.PersonTypeId == userTypeId) ||
                            (perm.User != null && perm.User.Id == userId)
                        )
                    )
                    select perm.Id
                ).Any();

            return hasPermission;
        }

        /// <summary>
        /// Imposta i valori sui vari "switch" di amministrazione
        /// </summary>
        /// <param name="CurrentSwitch">Indica lo switch da modificare</param>
        /// <param name="newstatus">Indica lo stato da impostare</param>
        /// <returns></returns>
        public bool SettingsSwitchSet(Domain.Enums.GlobalAdminSwitch CurrentSwitch, bool newstatus)
        {
            bool done = true;
            //Check permessi

            if (! this.Manager.IsInTransaction())
                Manager.BeginTransaction();

   
            SettingsPortal settings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();

            if (settings == null)
            {
                settings = new SettingsPortal();
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                settings.ExternalLimitation = 0;
                settings.HasExternalLimitation = false;
                settings.HasInternalLimitation = false;
                settings.InternalLimitation = 0;
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                settings.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            switch (CurrentSwitch)
            {
                case Domain.Enums.GlobalAdminSwitch.TicketBehalf:
                    settings.CanBehalf = newstatus;
                    break;
                case Domain.Enums.GlobalAdminSwitch.TicketWrite:
                    settings.CanEditTicket = newstatus;
                    settings.CanBehalf &= newstatus;
                    break;
                case Domain.Enums.GlobalAdminSwitch.TicketRead:
                    settings.CanBehalf &= newstatus;
                    settings.CanEditTicket &= newstatus;
                    settings.CanShowTicket = newstatus;
                    break;
                case Domain.Enums.GlobalAdminSwitch.CategoryManagement:
                    settings.CanBehalf &= newstatus;
                    settings.CanEditTicket &= newstatus;
                    settings.CanShowTicket &= newstatus;
                    settings.CanCreateCategory = newstatus;
                    break;
            }

            Manager.SaveOrUpdate<SettingsPortal>(settings);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                
                if(Manager.IsInTransaction())
                    Manager.RollBack();
                done = false;
            }

            return done;
        }

        /// <summary>
        /// Come BEHALFER, imposto le notifiche per il proprietario del Ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public bool NotificationSetTicketOwner(Int64 ticketId, Domain.Enums.MailSettings settings, bool IsCurrent)
        {
            Ticket ticket = Manager.Get<Ticket>(ticketId);

            //Controllo che gli oggetti ci siano ed il Ticket sia in behalf
            if (ticket == null 
                || ticket.Owner == null
                || !ticket.IsBehalf
                )
                return false;

            if (IsCurrent)
            {
                //Verifico se l'utente corrente è davvero l'OWNER!
                if (ticket.Owner.Id != this.UserGetIdfromPerson(UC.CurrentUserID))
                return false;
            }
            else
            {
                //Verifico che l'utente corrente ABBIA i permessi di BEHALF
                if (!this.SettingsPermissionIsBehalfer(this.UserGetIdfromPerson(UC.CurrentUserID)))
                    return false;
            }

            //se il ticket è NASCOSTO all'utente, gli DISABILITO eventuali notifiche!
            if (ticket.IsHideToOwner)
                settings = MailSettings.none;

            return NotificationSetTicket(ticket, settings, ticket.Owner, true);
        }

        /// <summary>
        /// Imposto i MIEI parametri Ticket come CREATORE del Ticket (behalfer o no)
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public bool NotificationSetTicketCreatorCurrent(Int64 ticketId, Domain.Enums.MailSettings settings)
        {
            Ticket ticket = Manager.Get<Ticket>(ticketId);

            if (ticket == null || ticket.Owner == null)
                return false;

            TicketUser currentUser = this.UserGetfromPerson(UC.CurrentUserID);

            if (ticket.IsBehalf)
            {
                if (ticket.CreatedBy.Id != UC.CurrentUserID)
                     return false;
                //if (!this.SettingsPermissionIsBehalfer(this.UserGetIdfromPerson(UC.CurrentUserID)))
                    
            }
            else
            {
                if (ticket.CreatedBy.Id != UC.CurrentUserID && ticket.Owner.Id != currentUser.Id)
                    return false;    
            }
            
            return NotificationSetTicket(ticket, settings, currentUser, true);
        }

        public bool NotificationSetTicketCreatorExternal(Int64 ticketId, Domain.Enums.MailSettings settings, Int64 currentUserId)
        {
            Ticket ticket = Manager.Get<Ticket>(ticketId);

            if (ticket == null || ticket.Owner == null)
                return false;

            if (ticket.Owner.Id != currentUserId)
                return false;

            return NotificationSetTicket(ticket, settings, ticket.Owner, true);
        }


        public bool NotificationSetTicket(Int64 ticketId, Domain.Enums.MailSettings settings, Int64 userId,
            bool isForUser)
        {
            Ticket ticket = Manager.Get<Ticket>(ticketId);
            TicketUser user = Manager.Get<TicketUser>(userId);

            if (ticket == null || user == null)
                return false;

            return NotificationSetTicket(ticket, settings, user, isForUser);
        }

        private bool NotificationSetTicket(Ticket ticket, Domain.Enums.MailSettings settings, TicketUser user, bool isForUser)
        {
            if (settings == MailSettings.DISABLED)
                return true;
            else if (settings == MailSettings.Default)
            {
                settings = isForUser ? MailSettings.UserDefault : MailSettings.ManResDefault;
            }

            if (user == null)
                return false;

            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();

            Domain.MailNotification notSetting =
                Manager.GetAll<Domain.MailNotification>(
                    mn => mn.IsPortal == false
                          && mn.Ticket != null && mn.Ticket.Id == ticket.Id
                          && mn.User != null && mn.User.Id == user.Id
                    ).OrderByDescending(mn => mn.CreatedOn)
                    .FirstOrDefault();

            if (notSetting == null)
            {
                notSetting = new MailNotification();
                notSetting.User = user;
                notSetting.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                notSetting.Ticket = ticket;
                notSetting.Settings = MailSettings.Default;
            }
            else
            {
                notSetting.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            if (notSetting.User == null || notSetting.Ticket == null)
                return false;

            notSetting.Settings = MailSettingsMaskTicket(settings, notSetting.Settings, isForUser);

            if (isForUser)
            {
                notSetting.IsDefaultUser = (settings == MailSettings.UserDefault);
            }
            else
            {
                notSetting.IsDefaultManager = (settings == MailSettings.ManResDefault);
            }
            
            try
            {
                Manager.SaveOrUpdate(notSetting);
                Manager.Commit();
            }
            catch (Exception)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                return false;
            }

            return true;
        }

        private Domain.Enums.MailSettings MailSettingsMaskTicket(Domain.Enums.MailSettings newSettings,
            Domain.Enums.MailSettings oldSettings, bool maskUser)
        {
            if ((Int64)newSettings < 0)
                return oldSettings;

            //if (oldSettings < 0)
            //    oldSettings = 0;


            //MailSettings maskMan = MailSettings.ModerationChangedMan |
            //           MailSettings.NewMessageManager |
            //           MailSettings.NewTicketManager |
            //           MailSettings.StatusChangedManager |
            //           MailSettings.TicketAssCategoryMan |
            //           MailSettings.TicketNewAssignmentMan |
            //           MailSettings.TicketResetAssMan;

            //MailSettings maskUsr = MailSettings.ModerationChangedUsr |
            //           MailSettings.NewMessageUsr |
            //           MailSettings.NewTicketUsr |
            //           MailSettings.OwnerChanged |
            //           MailSettings.StatusChangedUsr |
            //           MailSettings.TicketResetAssUsr;

            MailSettings outSettings = MailSettings.none;

            if (maskUser)
            {
                //Impostazioni utente
                if (newSettings == MailSettings.Default)
                    newSettings = MailSettings.ManResDefault;

                if (oldSettings == MailSettings.Default)
                    oldSettings = MailSettings.UserDefault;

                outSettings = (newSettings | ManagerMask) &
                              (oldSettings | UserMask);

            }
            else
            {
                //Impostazioni utente
                if (newSettings == MailSettings.Default)
                    newSettings = MailSettings.UserDefault;

                if (oldSettings == MailSettings.Default)
                    oldSettings = MailSettings.ManResDefault;

                outSettings = (newSettings | UserMask) &
                              (oldSettings | ManagerMask);
            }

            return outSettings;
        }

        /// <summary>
        /// Toglie i valori inopportuni x set su impostaizoni globali sistema o impostazioni globali utente.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="settingType"></param>
        /// <returns></returns>
        public static MailSettings MailSettingsMaskPortal(MailSettings settings, MailSettingsMaskType settingType)
        {

            MailSettings outSet = MailSettings.none;

            switch (settingType)
            {
                case MailSettingsMaskType.globalUser:
                    outSet = settings & UserMask;
                    break;
                case MailSettingsMaskType.globalManager:
                    outSet = settings & ManagerMask;
                    break;
                case MailSettingsMaskType.globalBoth:
                    outSet = settings & (ManagerMask | UserMask);
                    break;
            }
            
            return outSet;
        }

        public const MailSettings ManagerMask = MailSettings.NewTicketManager | MailSettings.NewMessageManager |
                       MailSettings.TicketNewAssignmentMan | MailSettings.StatusChangedManager |
                       MailSettings.ModerationChangedMan;

        public const MailSettings UserMask = MailSettings.NewTicketUsr | MailSettings.NewMessageUsr |
                       MailSettings.StatusChangedUsr | MailSettings.ModerationChangedUsr |
                       MailSettings.OwnerChanged;

        //Config sistema
        public bool MailSetStatus(bool enabled, bool forManager)
        {
            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();


            SettingsPortal settings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();


            if (settings == null)
            {
                settings = new SettingsPortal();
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                settings.ExternalLimitation = 0;
                settings.HasExternalLimitation = false;
                settings.HasInternalLimitation = false;
                settings.InternalLimitation = 0;
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                settings.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            if (forManager)
            {
                settings.IsNotificationManActive = enabled;
            }
            else
            {
                settings.IsNotificationUserActive = enabled;
            }

            Manager.SaveOrUpdate<SettingsPortal>(settings);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                return false;
            }

            return true;
        }


        public bool MailSendSetConfig(
            bool userEnabled, 
            bool managerEnabled, 
            Domain.Enums.MailSettings userSettings,
            Domain.Enums.MailSettings managerSettings)
        {

            userSettings = MailSettingsMaskPortal(userSettings, MailSettingsMaskType.globalUser);
            managerSettings = MailSettingsMaskPortal(managerSettings, MailSettingsMaskType.globalManager);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            SettingsPortal settings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();


            if (settings == null)
            {
                settings = new SettingsPortal();
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                settings.ExternalLimitation = 0;
                settings.HasExternalLimitation = false;
                settings.HasInternalLimitation = false;
                settings.InternalLimitation = 0;
                settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                settings.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            settings.IsNotificationUserActive = userEnabled;
            settings.IsNotificationManActive = managerEnabled;
            
            //Domain.MailNotification notSetting =
            //   Manager.GetAll<Domain.MailNotification>(
            //       mn => mn.IsPortal == true
            //             && mn.Ticket == null
            //             && mn.User == null
            //       ).OrderByDescending(mn => mn.CreatedOn)
            //       .FirstOrDefault();

            //if (notSetting == null)
            //{
            //    notSetting = new MailNotification();
            //    notSetting.User = null;
            //    notSetting.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            //    notSetting.Ticket = null;
            //    notSetting.IsPortal = true;
            //    notSetting.Settings = MailSettings.none;
            //    notSetting.IsDefaultManager = false;
            //    notSetting.IsDefaultUser = false;
            //}
            //else
            //{
            //    notSetting.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            //}

            //notSetting.Settings = SettingsMask(userSettings, notSetting.Settings, true);
            //notSetting.Settings = SettingsMask(managerSettings, notSetting.Settings, false);

            settings.MailSettingsUser = userSettings; //SettingsMask(userSettings, settings.MailSettingsUser, true);
            settings.MailSettingsManager = managerSettings; //SettingsMask(managerSettings, settings.MailSettingsManager, false);


            Manager.SaveOrUpdate<SettingsPortal>(settings);
            //Manager.SaveOrUpdate<Domain.MailNotification>(notSetting);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                return false;
            }

            return true;
        }




        public Domain.SettingsPortal PortalSettingsGet()
        {
         
            if (_portalSettings == null)
            {
                _portalSettings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();
                Manager.Detach(_portalSettings);
                
            }

            if (_portalSettings == null)
            {
                //settings = new SettingsPortal();
                //settings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                _portalSettings = new SettingsPortal();
                _portalSettings.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                _portalSettings.ExternalLimitation = 0;
                _portalSettings.HasExternalLimitation = false;
                _portalSettings.HasInternalLimitation = false;
                _portalSettings.InternalLimitation = 0;

                Manager.SaveOrUpdate(_portalSettings);
            }
                

            return _portalSettings;
        }
    }
}
