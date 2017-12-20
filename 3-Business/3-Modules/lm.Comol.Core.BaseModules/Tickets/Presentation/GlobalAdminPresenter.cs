using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;
using lm.Comol.Core.BaseModules.Tickets.Domain.DTO;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class GlobalAdminPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;
        /// <summary>
        /// TEMPORANEO
        /// </summary>
        private lm.Comol.Core.Business.BaseModuleManager Manager { get; set; }
        protected virtual new View.iViewGlobalAdmin View
        {
            get { return (View.iViewGlobalAdmin)base.View; }
        }

        public GlobalAdminPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
            //TEMPORANEO
            Manager = new lm.Comol.Core.Business.BaseModuleManager(oContext);
        }
        public GlobalAdminPresenter(iApplicationContext oContext, View.iViewGlobalAdmin view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
            //TEMPORANEO
            Manager = new lm.Comol.Core.Business.BaseModuleManager(oContext);
        }
    #endregion

        public void InitView(Domain.Enums.GlobalAdminStatus Status = GlobalAdminStatus.none, bool draftLimitErr = false, bool intLimitErr = false, bool extLimitErr = false)
        {
            if (!CheckSession())
                return;

            //if (!service.PersonCurrentIsSysAdmin())
            //{
            //    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, 0, ModuleTicket.InteractionType.None);

            //    View.ShowNoPermission();
            //    return;
            //}
            //Solo portale.
            View.ViewCommunityId = 0;

            //Begin Action
            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.SettingsGlobalLoad, 0, ModuleTicket.InteractionType.UserWithLearningObject);
            //End Action
            
            View.Settings = service.SettingsGlobalGet(true, View.CurrentCategorySelection);
            //View.SetCategories(settings);
            View.CategoryInfo = service.CategorySysInfoGet();
            View.Permissions = service.SettingsPermissionGet(UserContext.Language.Id);

            View.ShowMessage(Status, draftLimitErr, intLimitErr, extLimitErr);
        }

        public void SaveSettings(
            bool enableUser,
            bool enableManager,
            Domain.Enums.MailSettings userSettings,
            Domain.Enums.MailSettings managerSettings)
        {
            if (!CheckSession())
                return;

          


            GlobalAdminStatus SaveStatus = GlobalAdminStatus.SaveOK;

            //if (!service.PersonCurrentIsSysAdmin())
            //{
            //    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, 0, ModuleTicket.InteractionType.None);

            //    View.ShowNoPermission();
            //    return;
            //}

            //Begin Action
            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.SettingsGlobalModify, 0, ModuleTicket.InteractionType.UserWithLearningObject);
            //End Action

            Domain.SettingsPortal settings = View.Settings;


            bool successMailSet = service.MailSendSetConfig(enableUser, enableManager, userSettings, managerSettings);
            
            Domain.DTO.DTO_Access Access = new Domain.DTO.DTO_Access();
            Access.CanManageCategory = settings.CanCreateCategory;
            Access.CanShowTicket = settings.CanShowTicket;
            Access.CanEditTicket = settings.CanEditTicket;

            //TO DO: Rinominare View.Permission in View.BehalfPermission?
            // O comunque rivedere quando sarà ora...

            bool successSet = service.SettingsGlobalSet(
                settings.HasExternalLimitation,
                settings.ExternalLimitation,
                settings.HasInternalLimitation,
                settings.InternalLimitation,
                settings.HasDraftLimitation,
                settings.DraftLimitation,
                settings.CommunityTypeSettings,
                0, 
                Access,
                View.Permissions);//0 ==> View.Settings.MailSettings,

            bool hasDraftLimitationError = (settings.DraftLimitation <= 0);
            bool hasInternalLimitationError = (settings.InternalLimitation < 0);
            bool hasExternalLimitationError = (settings.ExternalLimitation < 0);

            if(!(successSet && successMailSet))
            {
                SaveStatus = GlobalAdminStatus.InternalError;
            }

            InitView(SaveStatus, hasDraftLimitationError, hasInternalLimitationError, hasExternalLimitationError);
        }


        //private void UpdateInfo()
        //{
        //    int CategoryPublic = 0;
        //    int CategoryCommunity = 0;
        //    int CategoryTicket = 0;

        //    service.SettingsGetInfo(ref CategoryPublic,ref CategoryCommunity,ref CategoryTicket);
        //    View.SetInfo(CategoryPublic, CategoryCommunity, CategoryTicket);
        //}

        public void UpdateCommunityTypes()
        {
            if (!CheckSession())
                return;

            service.SettingsSystemRefresh();

            InitView(GlobalAdminStatus.SaveOK);
        }

        public bool CheckSession()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(0);
                return false;
            } else if (!service.PersonCurrentIsSysAdmin())
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, 0, ModuleTicket.InteractionType.None);

                View.ShowNoPermission();
                return false;

                //View.ShowNoPermission();
                //return false;
            }

            return true;
        }

        public void CategorySetDefault(Int64 CategoryId)
        {
            if (!CheckSession())
                return;

            View.ShowMessage(
                service.CategoryDefaultSet(CategoryId)
                    ? GlobalAdminStatus.CategoryDefSetted
                    : GlobalAdminStatus.InternalError, false, false, false);

            View.SetCategories(service.SettingsGlobalGet(true, 0));

        }

        public void CategoryRemoveDefault()
        {
            if (!CheckSession())
                return;

            if (!service.PersonCurrentIsSysAdmin())
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, 0, ModuleTicket.InteractionType.None);

                View.ShowNoPermission();
                return;
            }

            View.ShowMessage(
                service.CategoryDefaultRemove() 
                ? GlobalAdminStatus.CategoryDefRemoved 
                : GlobalAdminStatus.InternalError,
                false, false, false);

            View.SetCategories(service.SettingsGlobalGet(true, 0));
            
        }

        public void SetSwitch(Domain.Enums.GlobalAdminSwitch Switch, bool status)
        {
            if (!CheckSession())
                return;

            bool success = service.SettingsSwitchSet(Switch, status);
            //this.InitView(GlobalAdminStatus.none);

            Domain.SettingsPortal settings = service.SettingsGlobalGet(false, View.CurrentCategorySelection);

            View.SetCategories(settings);

            View.ShowSwitchChanged(new DTO_PortalSettingsSwitch(settings), Switch, status, success);
            
        }


        public void PermissionUserDelete(Int64 permissionId)
        {
            if (!CheckSession())
                return;

            Int64 responsePermissionId = service.SettingPermissionUserDelete(permissionId);
            if (responsePermissionId > 0)
            {
                //Begin Action
                    List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                    //Objects.Add(ModuleTicket.KVPgetPerson(UserContext.CurrentUserID));
                    Objects.Add(ModuleTicket.KVPgetPermission(responsePermissionId));

                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.PermissionRemove, View.ViewCommunityId, ModuleTicket.InteractionType.UserWithUser, Objects);
                
                //End Action

                this.InitView(GlobalAdminStatus.none);
                //Show OK
            }
            else
            {
                //show error
                this.InitView(GlobalAdminStatus.InternalError);
            }
        }

        public void PermissionUserAdd(IList<Int32> usersId, Domain.Enums.PermissionType permissionType)
        {
            if (!CheckSession())
                return;

            if (service.SettingPermissionUserAdd(usersId, permissionType))
            {
                //Begin Action
                if (permissionType == PermissionType.Behalf)
                {   
                    List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                    //Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                    //Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.PermissionBehalfSet, View.ViewCommunityId, ModuleTicket.InteractionType.UserWithUser, Objects);
                }
                //End Action

                this.InitView(GlobalAdminStatus.none);
                //Messaggio OK
            }
            else
            {
                this.InitView(GlobalAdminStatus.InternalError);
                //Messaggio Errore!;
            }
        }

        ///// <summary>
        ///// Carico il controllo utente per la selezione degli utenti, recuperando quando attualmente registrato su DB
        ///// evitando di lasciare dentro utenti che già ci sono o mettendo utenti che sono stati rimossi
        ///// </summary>
        ///// <remarks>
        ///// E' stato tolto in favore dell'INIT,
        ///// dove recupero tutti i permission in una botta unica e li suddivido DOPO averli caricati.
        ///// </remarks>
        //public void SelectUsersToAdd()
        //{
        //    if (!CheckSession())
        //        return;

        //    List<Int32> idUsers = (from p in Manager.Linq<Domain.liteSettingsPermission>()
        //                            where p.Deleted == BaseStatusDeleted.None && p.PermissionType == Domain.Enums.PermissionType.Behalf
        //                            && p.User != null && p.User.Person != null && p.User.Person.Id > 0
        //                            select p.User.Person.Id).ToList().Distinct().ToList();
        //    idUsers.AddRange((from p in Manager.Linq<litePerson>()
        //                        where p.TypeID == (int)UserTypeStandard.TypingOffice || p.TypeID == (int)UserTypeStandard.Guest
        //                        || p.TypeID == (int)UserTypeStandard.PublicUser
        //                        select p.Id).ToList());
        //    View.InitializeUsersSelector(idUsers);
            
        //}

        public void MailSetStatus(bool enabled, bool forManager)
        {
            bool success = service.MailSetStatus(enabled, forManager);

            if (forManager)
                View.ShowSwitchChanged(new DTO_PortalSettingsSwitch(service.SettingsGlobalGet(false, View.CurrentCategorySelection)), GlobalAdminSwitch.NotificationManager, enabled, success);
            else
                View.ShowSwitchChanged(new DTO_PortalSettingsSwitch(service.SettingsGlobalGet(false, View.CurrentCategorySelection)), GlobalAdminSwitch.NotificationUser, enabled, success);
            

        }

        //public void MailSendSettingsSet()
        //{
           

        //    //return true;
        //}
     }
}