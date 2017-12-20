using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using TK = lm.Comol.Core.BaseModules.Tickets;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class UcPasswordPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewUcPassword View
        {
            get { return (View.iViewUcPassword)base.View; }
        }

        public UcPasswordPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public UcPasswordPresenter(iApplicationContext oContext, View.iViewUcPassword view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        public void ChangePassword()
        {
            if (!CheckUser())
            {
                View.ShowResponse(Domain.Enums.ExternalUserPasswordErrors.SessionTimeout);
                return;
            }


            Domain.Enums.ExternalUserPasswordErrors error = service.UserExtChangePassword(View.Password, View.NewPassword, View.CurrentUser.UserId);

            if (error != Domain.Enums.ExternalUserPasswordErrors.none)
            {
                View.ShowResponse(error);
                return;
            }

                

            bool sended = service.NotificationSendPasswordChanged(View.CurrentUser.UserId, View.NotificationSettings);

            if (!sended)
                error = Domain.Enums.ExternalUserPasswordErrors.MailSendError;


            View.ShowResponse(error);

        }

        public bool CheckUser()
        {
            if (View.CurrentUser == null || View.CurrentUser.UserId <= 0)
            {
                //View.DisplaySessionTimeout(0);
                return false;
            }
            return true;
        }

    }
}
