using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Chat
{
    public class IM_UserPresenter
    {
        /// <summary>
        /// La View che visualizza gli utenti/chat correnti
        /// </summary>
        private IView_IM_Users _view;

        public IM_UserPresenter(IView_IM_Users View)
        {
            this._view = View;
        }


        /// <summary>
        /// Recupera l'elenco delle chat correnti
        /// </summary>
        public void SetUsers()
        {
            InstantMessangerService.IInstantMessengerService service = null;
            try
            {
                service = new InstantMessangerService.InstantMessengerServiceClient();

                this._view.SetCurrentUsers(service.GetChats(_view.CurrentUserId));

                if (service != null)
                {
                    System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService> remoteService =
                     (System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService>)service;
                    remoteService.Close();
                    remoteService = null;
                }

            }
            catch (Exception ex)
            {
                if (service != null)
                {
                    System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService> remoteService =
                     (System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService>)service;
                    remoteService.Abort();
                    remoteService = null;
                }
            }
        }
    }
}
