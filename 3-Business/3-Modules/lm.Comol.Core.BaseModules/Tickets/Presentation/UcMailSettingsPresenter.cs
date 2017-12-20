using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using TK = lm.Comol.Core.BaseModules.Tickets;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class UcMailSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
                
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewUcMailSets View
        {
            get { return (View.iViewUcMailSets)base.View; }
        }

        public UcMailSettingsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public UcMailSettingsPresenter(iApplicationContext oContext, View.iViewUcMailSets view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        public void InitializeView(Int64 UserId, Int32 CommunityId, Int64 TicketId)
        {
            if (UserId <= 0)
                return;

            //if (CommunityId <= 0)
            //    this.View.MailSettings = service.MailSettingsGet(UserId);
            //else
            //if (TicketId <= 0)
            //    this.View.MailSettings = service.MailSettingsGet(UserId); //, CommunityId
            //else
            //    this.View.MailSettings = service.MailSettingsGet(UserId, TicketId); //, CommunityId,

        }

        public bool UpdateSettings(Int64 UserId, Int32 CommunityId, Int64 TicketId)
        {
            if (UserId <= 0)
                return false;

            if (CommunityId <= 0)
                return service.MailSettingsSetPortal(View.MailSettings, UserId);
            else if (TicketId <= 0)
                return service.MailSettingsSetCommunity(View.MailSettings, CommunityId, UserId);

            return true;
            //return service.MailSettingsSetTicket(View.MailSettings, TicketId, UserId);
        }

        public int UpdateSettingsALL(Int64 UserId)
        {
            if (UserId <= 0)
                return -1;

            return service.MailSettingsSetALL(View.MailSettings);
        }

        public Int64 GetUserIdFromPerson(int PersonId)
        {
            Int64 UserId = service.UserGetIdfromPerson(PersonId);

            return UserId;
        }

    }
}
