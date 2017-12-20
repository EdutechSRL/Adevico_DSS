using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Notification.Domain;
using log4net.Config;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class TicketListManPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketListMan View
        {
            get { return (View.iViewTicketListMan)base.View; }
        }

        public TicketListManPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }

        public TicketListManPresenter(iApplicationContext oContext, View.iViewTicketListMan view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        /// <summary>
        /// Inizializzazione View:
        /// Bind filtri, bind tabelle, etc...
        /// </summary>
        public void InitView()
        {
            if (!CheckSessionAccess())
                return;

            //DateTime DT1 = DateTime.Now;
            //Controllo dei permessi viene fatto QUI DENTRO:
            InitViewFilters();      //Altrimenti devo recuperare 2 volte le categorie...
            //Se non ho categorie o se non ho ticket associati,
            //richiamo il "ShowNoPermission" della View, che fa il REDIRECT sulla lista personale.
            //DateTime DT2 = DateTime.Now;
            BindInfos();
            //DateTime DT3 = DateTime.Now;
            BindTable(null, true);
            //DateTime DT_END = DateTime.Now;

            //TimeSpan TS_FILTERS = DT2 - DT1;
            //TimeSpan TS_Infos = DT3 - DT2;
            //TimeSpan TS_TABLE = DT_END - DT3;

            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
            
            View.SendUserActions(
                service.ModuleID, 
                ModuleTicket.ActionType.TicketListManRes, 
                View.ViewCommunityId, 
                ModuleTicket.InteractionType.UserWithLearningObject, 
                Objects);
            //End Action
        }

        /// <summary>
        /// Interna: inizializza i filtri
        /// </summary>
        private void InitViewFilters()
        {
            //IList<Domain.DTO.DTO_CategoryTree> Categories = service.CategoriesGetTreeDLL(CurrentCommunityId, Domain.Enums.CategoryTREEgetType.FilterManager);

           
            
            //if (Categories == null || !Categories.Any())
            //{
            //    if(!service.UserHasTicketAssociationCom(this.CurrentCommunityId))
            //    {
            //        View.ShowNoPermission();
            //        return;
            //    }   
            //}

            View.BindFilters(service.LanguagesGetCurrent());

            BindDLLCategories(null);

            //View.BindFilters(
            //    Categories,
            //    service.LanguagesGetCurrent(),
            //    null);
        }

        public void ReBindDDLCategory(Int64 SelectedCategoryId)
        {
            if (!CheckSessionAccess())
                return;

            Domain.DTO.DTO_CategoryTree SelectedCate = new Domain.DTO.DTO_CategoryTree();
            SelectedCate.Id = -1;

            SelectedCate = service.CategoryGetDTOCatTree(SelectedCategoryId);

            BindDLLCategories(SelectedCate);
            
        }

        /// <summary>
        /// Carica le categorie e controlla che l'utente sia manager o resolver.
        /// </summary>
        /// <param name="SelectedCate"></param>
        private void BindDLLCategories(Domain.DTO.DTO_CategoryTree SelectedCate)
        {
            //IList<Domain.DTO.DTO_CategoryTree> Categories = service.CategoryGetDDLFilter_ManResCurrent(CurrentCommunityId);
            Int32 ComID = CurrentCommunityId;
            IList<Domain.DTO.DTO_CategoryTree> Categories;

            //if (ComID > 0)
            //{
            //    Categories = service.CategoryGetDDLFilter_ManResCurrent(ComID);
            //}
            //else
            //{
            Categories = service.CategoryGetDDLManRes_Filters();    
            //}
            
            if (Categories == null || !Categories.Any())
            {
                View.ShowNoPermission();
            }
            
            View.BindCategoriesFilter(
                Categories,
                SelectedCate);

        }

        public void BindInfos()
        {
            View.SetInfo(service.TicketListInfoGetManRes());
        }
        /// <summary>
        /// Aggiorna la tabella in base ai filtri che trova.
        /// </summary>
        public void BindTable(Domain.DTO.DTO_ListFilterManager Filters, bool forInit = false)
        {
            if (!CheckSessionAccess())
                return;

            //Domain.DTO.DTO_ListFilterManager Filters = View.Filters;
            if (Filters == null)
                Filters = new Domain.DTO.DTO_ListFilterManager();

            IList<Domain.DTO.DTO_TicketListItemManager> items = service.TicketsGetManRes(ref Filters);

            bool HasDraft = service.TicketUserHasDraft();

            int ItemCount = Filters.RecordTotal;
            int CurrentPage = Filters.PageIndex;

            View.BindTable(items, ItemCount, CurrentPage, HasDraft);

            //if (Filters.CategoryId > 0)
            if(!forInit)
                ReBindDDLCategory(Filters.CategoryId);
        }

        //TODO: notification - V - test
        public void ChangeStatus(Int64 TicketId, Domain.Enums.TicketStatus Status, Domain.Enums.MessageUserType UserType)
        {
            if (!CheckSessionAccess())
                return;

            Int64 messageId = 0;
            Boolean Changed = service.TicketStatusModify(TicketId, Status, View.GetChangeStatusMessage(Status), true, UserType, ref messageId);

            //Begin Action
            if (Changed)
            {
                Int64 userId = service.UserGetIdfromPerson(UserContext.CurrentUserID);

                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(userId));
                Objects.Add(ModuleTicket.KVPgetTicket(TicketId));

                View.SendUserActions(
                service.ModuleID, 
                ModuleTicket.ActionType.TicketStatusChanged, 
                View.ViewCommunityId, 
                ModuleTicket.InteractionType.UserWithLearningObject, 
                Objects);
                //End Action

                //TODO: Notification - TEST
                if (messageId > 0)
                    SendNotification(messageId, userId, ModuleTicket.NotificationActionType.StatusChanged);
            }
            
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
                View.DisplaySessionTimeout(View.ViewCommunityId);
                return false;
            }

            Domain.DTO.DTO_Access Access = service.SettingsAccessGet(true);
            if (!(Access.IsActive && Access.CanShowTicket))
            {
                View.ShowNoAccess();
                return false;
            }

            if (!Access.CanEditTicket)
            {
                View.DisableAddNew();
            }

            return true;
        }

        private void SendNotification(Int64 messageId, Int64 tkUserId, ModuleTicket.NotificationActionType actionType)
        {
            SettingsPortal settingsPortal = service.PortalSettingsGet();
            if (!(settingsPortal.IsNotificationUserActive && settingsPortal.IsNotificationManActive))
                return;

            //test ID community
            int currentCommunityId = UserContext.CurrentCommunityID;

            NotificationAction action = new NotificationAction();
            action.IdCommunity = CurrentCommunityId;
            action.IdObject = messageId;
            action.IdObjectType = (long)ModuleTicket.ObjectType.Message;
            action.ModuleCode = ModuleTicket.UniqueCode;

            action.IdModuleUsers = new List<long>();
            action.IdModuleUsers.Add(tkUserId);

            action.IdModuleAction = (long)actionType;

            //IList<NotificationAction> actions = new List<NotificationAction>();
            //actions.Add(action);

            //action.IdModuleAction = (long) ModuleTicket.MailSenderActionType.TicketSendMessageMan;

            View.SendNotification(action, UserContext.CurrentUserID);

        }


    }
}
