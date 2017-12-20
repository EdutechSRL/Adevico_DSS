using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class CategoryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

        private TicketService service;

        protected virtual new View.iViewCategory View
        {
            get { return (View.iViewCategory)base.View; }
        }

        public CategoryPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public CategoryPresenter(iApplicationContext oContext, View.iViewCategory view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

        private lm.Comol.Core.BaseModules.Tickets.ModuleTicket _module;
        private lm.Comol.Core.BaseModules.Tickets.ModuleTicket Module
        {
            get
            {
                if ((_module == null))
                {
                    Int32 idUser = UserContext.CurrentUserID;
                    _module = service.PermissionGetService(idUser, CurrentCommunityId);
                }
                return _module;
            }
        }
        #endregion

        /// <summary>
        /// Al primo caricamento della pagina!
        /// </summary>
        public void InitView()
        {
            if (!CheckSessionAccess())
                return;

            if (!(Module.ManageCategory || Module.Administration))
            {
                View.SendUserActions(service.ModuleID,
                ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                View.ShowNoPermission();
                return;
            }

            Domain.Category Category = new Domain.Category();

            //View.CommunityCategories = service.CategoriesGetCommunity(true);

            if (View.CurrentCategoryId > 0)
            {
                //Categoria
                Domain.Category Cat = service.CategoryGetDetached(View.CurrentCategoryId, Domain.Enums.RolesLoad.all, false);
                if (Cat == null || Cat.Id <= 0 || Cat.IdCommunity != CurrentCommunityId)
                {
                    View.ShowWrongCategory();
                    return;
                }

                Domain.DTO.DTO_CategoryTypeComPermission Permission = service.CategoryTypeGetPermission(this.CurrentCommunityId);


                View.InitCategories(Permission, Cat.Type, Cat.IsDefault);
                View.Category = Cat;

                //Traduzioni
                List<lm.Comol.Core.DomainModel.Languages.LanguageItem> langs = new List<lm.Comol.Core.DomainModel.Languages.LanguageItem>();
                //List<Domain.DTO.DTO_CategoryTranslation> translations = new List<Domain.DTO.DTO_CategoryTranslation>();
                langs = (from t in Cat.Translations
                         where t.Deleted == BaseStatusDeleted.None
                         select new lm.Comol.Core.DomainModel.Languages.LanguageItem
                         {
                             IsMultiLanguage = (t.LanguageId == TicketService.LangMultiID) ? true : false,
                             Id = t.LanguageId,
                             Code = (t.LanguageId == TicketService.LangMultiID) ? t.LanguageCode.ToUpper() : t.LanguageCode,
                             Name = (t.LanguageId == TicketService.LangMultiID) ? t.LanguageName.ToUpper() : t.LanguageName,
                             Status = DomainModel.Languages.ItemStatus.valid
                         }).Distinct().ToList();
                //.OrderBy(li => li.IsMultiLanguage).ThenBy(li => li.Code).ToList();

                //.OrderBy(li => li.Code).ToList();
                //langs = langs.OrderBy(li => li.IsMultiLanguage).ToList();
                //.OrderBy(li => li.IsMultiLanguage).ThenBy(li => li.Code).Distinct().ToList();


                ////TEST!!!
                //String TEST = "";
                //foreach (lm.Comol.Core.DomainModel.Languages.LanguageItem _lng in langs)
                //{
                //    TEST += _lng.Code + " - " + _lng.Name + " - " + _lng.IsMultiLanguage + "/r/n";
                //}

                //langs = langs.OrderBy(li => li.IsMultiLanguage).ThenBy(li => li.Code).ToList();

                //TEST += "/r/n -------  /r/n";

                //foreach(lm.Comol.Core.DomainModel.Languages.LanguageItem _lng in langs)
                //{
                //    TEST += _lng.Code + " - " + _lng.Name + " - " + _lng.IsMultiLanguage + "/r/n";
                //}


                ////END TEST!!!



                lm.Comol.Core.DomainModel.Languages.LanguageItem current =
                    (from lm.Comol.Core.DomainModel.Languages.LanguageItem lng in langs
                     where lng.Id == TicketService.LangMultiID
                     select lng).FirstOrDefault();

                View.BindLanguages(service.LanguagesGetAvailableSys().Distinct().ToList(), langs, current);
                View.PreviousLanguage = current;


                //UTENTI
                IList<Domain.DTO.DTO_UserRole> roles = (from Domain.LK_UserCategory lkcat in Cat.UserRoles select new Domain.DTO.DTO_UserRole() { User = lkcat.User, IsManager = lkcat.IsManager }).ToList();

                int Managercount = (from Domain.DTO.DTO_UserRole rl in roles where rl.IsManager == true select rl.IsManager).Count();

                if (Managercount <= 1)
                    View.ShowDeleteManagers = false;
                else
                    View.ShowDeleteManagers = true;

                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetCategory(Cat.Id));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.CategoryLoadManage, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action


                View.AssociatedUsers = roles;

                View.ShowForcedReassigned(null);
            }
            else
            {
                View.RedirectToList();
                //Vai alla creazione!
                //View.Category = new Domain.Category();
            }


            //Category;            
        }

        /// <summary>
        /// Aggiornamento view per i vari postback...
        /// </summary>
        private void UpdateView(lm.Comol.Core.DomainModel.Languages.BaseLanguageItem NewLanguage)
        {
            if (!CheckSessionAccess())
                return;



            Domain.Category Category = new Domain.Category();

            //View.CommunityCategories = service.CategoriesGetCommunity(true);

            if (View.CurrentCategoryId > 0)
            {
                //Categoria
                Domain.Category Cat = service.CategoryGetDetached(View.CurrentCategoryId, Domain.Enums.RolesLoad.all, false);

                //Traduzioni
                List<lm.Comol.Core.DomainModel.Languages.LanguageItem> langs = new List<lm.Comol.Core.DomainModel.Languages.LanguageItem>();

                langs = (from t in Cat.Translations
                         where t.Deleted == BaseStatusDeleted.None
                         orderby t.LanguageCode
                         select new lm.Comol.Core.DomainModel.Languages.LanguageItem
                         {
                             IsMultiLanguage = (t.LanguageId == TicketService.LangMultiID) ? true : false,
                             Id = t.LanguageId,
                             Code = (t.LanguageId == TicketService.LangMultiID) ? t.LanguageCode.ToUpper() : t.LanguageCode,
                             Name = (t.LanguageId == TicketService.LangMultiID) ? t.LanguageName.ToUpper() : t.LanguageName,
                             Status = DomainModel.Languages.ItemStatus.valid
                         }).Distinct().ToList();

                //langs.OrderBy(l => l.IsMultiLanguage).ThenBy(l => l.Name).ToList();

                lm.Comol.Core.DomainModel.Languages.LanguageItem current = View.CurrentLanguage;


                if (NewLanguage != null)
                {
                    current = new DomainModel.Languages.LanguageItem(NewLanguage);
                    langs.Add(current);
                }
                else if (current.Id != TicketService.LangMultiID)
                {
                    Domain.CategoryTranslation trn = (from Domain.CategoryTranslation ct in Cat.Translations where ct.LanguageId == current.Id select ct).FirstOrDefault();
                    Cat.Name = trn.Name;
                    Cat.Description = trn.Description;
                }

                View.Category = Cat;

                View.BindLanguages(service.LanguagesGetAvailableSys(), langs, current);
                View.PreviousLanguage = current;

                //UTENTI
                IList<Domain.DTO.DTO_UserRole> roles = (from Domain.LK_UserCategory lkcat in Cat.UserRoles select new Domain.DTO.DTO_UserRole() { User = lkcat.User, IsManager = lkcat.IsManager }).ToList();

                int Managercount = (from Domain.DTO.DTO_UserRole rl in roles where rl.IsManager == true select rl.IsManager).Count();

                if (Managercount <= 1)
                    View.ShowDeleteManagers = false;
                else
                    View.ShowDeleteManagers = true;

                View.AssociatedUsers = roles;

                View.ShowForcedReassigned(null);
            }
            else
            {
                //Error!
            }

        }

        public void Save(Boolean UpdateView, Boolean SaveUsers = true)
        {
            
            if (!CheckSessionAccess())
                return;

            if (!(Module.ManageCategory || Module.Administration))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                View.ShowNoPermission();
                return;
            }

            //Se la view non è valida, non faccio nulla (la view si arrangia a mostrare gli errori (Nome vuoto)
            if (!View.Validate())
                return;

            lm.Comol.Core.DomainModel.Languages.LanguageItem Lang = View.PreviousLanguage;

            Domain.Category Cat = View.Category;

            Domain.Enums.CategoryAddModifyError Error = service.CategoryModify(Cat.Id, Cat.Name, Cat.Description, Cat.Type, Lang);



            Domain.Enums.CategoryAssignersError ResError = Domain.Enums.CategoryAssignersError.none;

            if(SaveUsers)
                ResError = service.CategoryUserUpdate(Cat.Id, View.UsersSettings);


            if (Error == Domain.Enums.CategoryAddModifyError.none)
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetCategory(Cat.Id));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.CategoryModify, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action
            }

            
            if (ResError != Domain.Enums.CategoryAssignersError.none)
            {
                Domain.DTO.DTO_CategoryCheckResponse resp = service.CategoryUsersCheck(Cat.Id);
                
                if(!resp.PreviousAssigned)
                {
                    if (UpdateView)
                    {
                        this.InitView();
                        //this.UpdateView(null);
                    }

                    View.ShowForcedReassigned(resp);
                    return;
                }   
            }
            
            if (Error == Domain.Enums.CategoryAddModifyError.none)
            {
                if (UpdateView)
                    this.UpdateView(null);

                View.ShowReassignError(ResError);
                return;
            }
            else
            {
                //Other error
            }


            
        }

        public void LanguageAdd(lm.Comol.Core.DomainModel.Languages.BaseLanguageItem NewLanguage)
        {

            if (!CheckSessionAccess())
                return;

            Save(false);
            this.UpdateView(NewLanguage);
        }

        public void UserRemove(Int64 UserId)
        {

            if (!CheckSessionAccess())
                return;

            Save(false, false);

            Boolean deleted = service.CategoryUserRemove(View.CurrentCategoryId, UserId);

            this.UpdateView(null);

            if (!deleted)
            {
                Domain.DTO.DTO_CategoryCheckResponse resp = service.CategoryUsersCheck(View.CurrentCategoryId);

                if (!resp.PreviousAssigned)
                {
                    this.InitView();
                    View.ShowForcedReassigned(resp);
                    return;
                }
                //this.View.ShowReassignError(Domain.Enums.CategoryAssignersError.deleteError);
            }
        }

        public void UserAdd(int PersonId, Boolean IsManager)
        {

            if (!CheckSessionAccess())
                return;

            Save(false);
            service.CategoryPersonAdd(View.CurrentCategoryId, IsManager, PersonId);
            this.InitView();
        }

        public void UsersAdd(IList<int> PersonsId, Boolean IsManager)
        {

            if (!CheckSessionAccess())
                return;

            Save(false);
            IList<int> unAddedIds = service.CategoryPersonsAdd(View.CurrentCategoryId, IsManager, PersonsId);
            this.InitView();
        }

        public void LanguageDelete()
        {

            if (!CheckSessionAccess())
                return;

            Save(false);
            service.CategoryLanguageDelete(View.CurrentCategoryId, View.CurrentLanguage.Id);

            this.InitView();
        }

        /// <summary>
        /// Comunità corrente. Da view (URL) se presente, altrimenti dalla sessione utente
        /// </summary>
        public Int32 CurrentCommunityId
        {
            get
            {
                Int32 VComId = View.ViewCommunityId;
                if (VComId > 0)
                {
                    return VComId;
                }
                else
                {
                    Int32 SysComId = UserContext.CurrentCommunityID;
                    View.ViewCommunityId = SysComId;
                    return SysComId;
                }

                //return UserContext.CurrentCommunityID;
            }
        }

        public bool CheckSessionAccess()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(this.CurrentCommunityId);
                return false;
            }

            Domain.DTO.DTO_Access Access = service.SettingsAccessGet(true);
            if (!(Access.IsActive && Access.CanManageCategory))
            {
                View.ShowNoAccess();
                return false;
            }

            return true;
        }


        #region Notification

        public void SendNotificationALL()
        {
            SendNotification((long)ModuleTicket.NotificationActionCategoryUserReceiver.All);
        }

        public void SendNotificationManagers()
        {
            SendNotification((long)ModuleTicket.NotificationActionCategoryUserReceiver.Managers);
        }

        public void SendNotificationResolvers()
        {
            SendNotification((long)ModuleTicket.NotificationActionCategoryUserReceiver.Resolvers);
        }

        public void SendNotificationUser(Int64 userID)
        {
            SendNotification(userID);
        }

        private void SendNotification(long userId)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(this.CurrentCommunityId);
                return;
            }

            Domain.SettingsPortal settingsPortal = service.PortalSettingsGet();
            if (!(settingsPortal.IsNotificationUserActive && settingsPortal.IsNotificationManActive))
            {
                View.ShowSendResponse(false);
                return;
            }

            NotificationAction action = new NotificationAction();
            action.IdModuleUsers = new List<long>();
            action.IdModuleUsers.Add(userId);

            action.ModuleCode = ModuleTicket.UniqueCode;
            action.IdCommunity = CurrentCommunityId;
            action.IdObjectType = (long)ModuleTicket.ObjectType.Category;
            action.IdObject = View.CurrentCategoryId;

            action.IdModuleAction = (long)ModuleTicket.NotificationActionType.CategoriesNotification;

            View.SendNotification(action, UserContext.CurrentUserID);

            //this.InitView();

            View.ShowSendResponse(true);

        }

        #endregion
    }
}