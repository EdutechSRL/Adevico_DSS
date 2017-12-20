using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class TicketListExtPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
          
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketListExt View
        {
            get { return (View.iViewTicketListExt)base.View; }
        }

        public TicketListExtPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public TicketListExtPresenter(iApplicationContext oContext, View.iViewTicketListExt view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        /// <summary>
        /// Inizializzazione View.
        /// NOTA: i valori dei filtri vengono inizializzati sui parametri impostati in:
        /// new Domain.DTO.DTO_ListFilter();
        /// sia nella view che nel bind, senza che ci sia correlazione tra i due... :P
        /// </summary>
        public void InitView(Domain.Enums.TicketDraftDeleteError DeleteError = Domain.Enums.TicketDraftDeleteError.hide)
        {
            if (!CheckUserAccess())
                return;

            View.ShowDeletMessage(DeleteError);

            Domain.DTO.DTO_ListInit FiltersInit = new Domain.DTO.DTO_ListInit();

            ////DDL Lingue
            //Filters.r_availableLanguages = service.LanguagesGetAvailableSys();

            //Categorie disponibili - TO DO -
            FiltersInit.Categories = service.CategoriesGetTreeDLL(-1, CategoryTREEgetType.System);

            View.InitFilters(FiltersInit);

            //UpdateInfo();
            

            //BindList(new Domain.DTO.DTO_ListFilterUser());
        }

        public void BindList()//Domain.DTO.DTO_ListFilterUser Filters) //, Boolean CheckSession = false)
        {
            if (!CheckUserAccess())
                return;

            Domain.DTO.DTO_ListFilterUser Filters = View.GetFilters();

            List<Domain.DTO.DTO_TicketListItemUser> items = service.TicketGetListUsrExt(ref Filters, View.CurrentUser);

            bool HasCurrentDraft = (from Domain.DTO.DTO_TicketListItemUser itm in items
                                    where itm.IsDraft == true
                                    select itm).Any();

            //Domain.DTO.DTO_ListFilter Filters = View.Filters;
            UpdateInfo();
            View.SetTickets(items, Filters.PageIndex, Filters.RecordTotal, service.UserIsManagerOrResolver(), HasCurrentDraft);
        }

        private void UpdateInfo()
        {
            View.SetInfo(service.TicketListInfoGetUser(View.CurrentUser));
        }

        public bool CheckUserAccess()
        {
            if (View.CurrentUser == null || View.CurrentUser.UserId <= 0)
            {
                View.DisplaySessionTimeout(0);
                return false;
            }

            return true;

            Domain.DTO.DTO_Access Access = service.SettingsAccessGet(false);
            if (!(Access.IsActive && Access.CanShowTicket))
            {
                View.ShowNoAccess();
                return false;
            }

            return true;
        }

        public void DeleteDraft(Int64 idDraft, String baseFilePath, String baseThumbnailPath)
        {
            if(!CheckUserAccess())
                return;

            Domain.Enums.TicketDraftDeleteError error = service.TicketDeleteDraft(idDraft, View.CurrentUser.UserId, baseFilePath, baseThumbnailPath);


            if (error == Domain.Enums.TicketDraftDeleteError.none)
            {
                InitView(error); //Reinizializzazione filtri
                BindList();      //Aggiornamento dati
            }
            View.ShowDeletMessage(error);   //Visualizzazione messaggi
        }

    }
}
