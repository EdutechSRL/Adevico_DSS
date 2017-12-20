using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class TicketListUsrPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketListUsr View
        {
            get { return (View.iViewTicketListUsr)base.View; }
        }

        public TicketListUsrPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }

        public TicketListUsrPresenter(iApplicationContext oContext, View.iViewTicketListUsr view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        /// <summary>
        /// Inizializzazione View.
        /// NOTA: i valori dei filtri vanno allineati "a mano" tra view ed inizializzazione dell'oggetto "DTO_ListFilterUser": IN REVISIONE!
        /// </summary>
        public void InitView(Domain.Enums.TicketDraftDeleteError DeleteError = Domain.Enums.TicketDraftDeleteError.hide)
        {

            if (!CheckSessionAccess())
                return;

            if (View.ViewCommunityId != UserContext.CurrentCommunityID)
                View.ViewCommunityId = UserContext.CurrentCommunityID;


            View.ShowDeletMessage(DeleteError);


            Domain.DTO.DTO_ListInit FiltersInit = new Domain.DTO.DTO_ListInit();

            ////DDL Lingue
            //Filters.r_availableLanguages = service.LanguagesGetAvailableSys();

            //Categorie disponibili - TO DO -
            FiltersInit.Categories = service.CategoriesGetTreeDLL(-1, CategoryTREEgetType.FilterUser);

            View.InitFilters(FiltersInit);

            //UpdateInfo();


            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketListUser, View.ViewCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            //BindList(new Domain.DTO.DTO_ListFilterUser());
        }

        public void BindList()
        {
            if (!CheckSessionAccess())
                return;

            Domain.DTO.DTO_ListFilterUser Filters = View.GetFilters();
        
            List<Domain.DTO.DTO_TicketListItemUser> items = service.TicketGetListUsr(ref Filters);

            bool HasCurrentDraft = (from Domain.DTO.DTO_TicketListItemUser itm in items
                             where itm.IsDraft == true
                             select itm).Any();
                                 
                                 //service.TicketUserHasDraft();

            //(items == null) ? false : (from Domain.DTO.DTO_TicketListItemUser itm in items where itm.Status == Domain.Enums.TicketStatus.draft select itm.Code).Any();

            //Domain.DTO.DTO_ListFilter Filters = View.Filters;

            bool CanBehalf = service.SettingPermissionGet(Domain.Enums.PermissionType.Behalf);
            
            UpdateInfo(CanBehalf);



            View.SetTickets(
                items,
                Filters.PageIndex,
                Filters.RecordTotal,
                service.UserIsManagerOrResolver(),
                HasCurrentDraft,
                CanBehalf);
        }

        private void UpdateInfo(bool CanBehalf)
        {
            if(CanBehalf)
                View.SetInfo(service.TicketListInfoGetBehalfer(), CanBehalf);
            else
                View.SetInfo(service.TicketListInfoGetUser(), CanBehalf);
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

            return true;
        }

        public void DeleteDraft(Int64 idDraft, String baseFilePath, String baseThumbnailPath)
        {
            Domain.Enums.TicketDraftDeleteError error = service.TicketDeleteDraft(idDraft, baseFilePath, baseThumbnailPath);
            if(error == Domain.Enums.TicketDraftDeleteError.none)
            {
                InitView(error); //Reinizializzazione filtri
                BindList();      //Aggiornamento dati
            }
            View.ShowDeletMessage(error);   //Visualizzazione messaggi
        }
    }

}