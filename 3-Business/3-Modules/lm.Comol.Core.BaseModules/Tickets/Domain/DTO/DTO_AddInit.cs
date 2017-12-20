using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_AddInit
    {
        public DTO_AddInit()
        {
            IsNew = false;
            CurrentUser = new DTO_User();
            CanModifyUser = false;
            availableLanguages = new List<DomainModel.Languages.BaseLanguageItem>();
            Categories = new List<DTO_CategoryTree>();
            //SelectedCategoryId = 0;

            TicketData = new DTO_Ticket();
            CanListUsers = false;
        }

        public DTO_User CurrentUser { get; set; }
        public Boolean CanModifyUser { get; set; }

        public List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> availableLanguages { get; set; }

        public IList<DTO_CategoryTree> Categories { get; set; }

        //public Int64 SelectedCategoryId { get; set; }

        public DTO_Ticket TicketData { get; set; }

        //public String CurrentCommunityName { get; set; }
        public int CurrentCommunityId { get; set; }

        public int FileCommunityId { get; set; }
        //public IList<int> UserOrganizations { get; set; }

        public bool CanDraft { get; set; }
        public bool CanAdd { get; set; }

        public bool CanBehalf { get; set; }

        /// <summary>
        /// Indica se è il primo accesso dell'utente: Ticket creato in automatico in bozza.
        /// </summary>
        public bool IsNew { get; set; }

        public bool HasOtherDraft { get; set; }


        //NOTIFICATION
        public Domain.Enums.MailSettings CreatorMailSettings { get; set; }
        public bool IsDefaultNotCreator { get; set; }
        public Domain.Enums.MailSettings OwnerMailSettings { get; set; }
        public bool IsDefaultNotOwner { get; set; }

        public bool IsOwnerNotificationEnable { get; set; }
        public bool IsCreatorNotificationEnable { get; set; }

        public bool CanListUsers { get; set; }
    }
}
