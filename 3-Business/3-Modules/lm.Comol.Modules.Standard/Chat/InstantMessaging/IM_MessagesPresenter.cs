using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Chat
{
    /// <summary>
    /// Prenter per la parte di instant messaging.
    /// </summary>
    public class IM_MessagesPresenter
    {


        /// <summary>
        /// La View che visualizza i messaggi
        /// </summary>
        private IView_IM_Messages _view;

        public IM_MessagesPresenter(IView_IM_Messages View)
        {
            this._view = View;
        }

        /// <summary>
        /// Invia un messaggio recuperando i dati dalla view
        /// </summary>
        public void SendMessage()//Guid ChatId, Int32 SenderId, String Message)
        {
            InstantMessangerService.IInstantMessengerService service = null;
            try
            {
                service = new InstantMessangerService.InstantMessengerServiceClient();

                service.SendMessage(_view.CurrentChatId, _view.CurrentUserId, _view.CurrentMessage);
                this._view.SetCurrentChat(service.GetChat(this._view.CurrentChatId, this._view.CurrentUserId));

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

        /// <summary>
        /// Recupera i messaggi per la chat corrente e li invia alla view
        /// </summary>
        public void SetMessages()
        {
            if (this._view.CurrentChatId != null && this._view.CurrentChatId != Guid.Empty)
            {
                InstantMessangerService.IInstantMessengerService service = null;
                try
                {
                    service = new InstantMessangerService.InstantMessengerServiceClient();


                    this._view.SetCurrentChat(service.GetChat(this._view.CurrentChatId, this._view.CurrentUserId));

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
            else {
                this._view.SetCurrentChat(null);
            }
        }

        //public void Test(){

        //    InstantMessangerService.IInstantMessengerService service = null;
        //    try{
        //        service = new InstantMessangerService.InstantMessengerServiceClient();

        //        service.CreateChat(null,null);

        //        if (service!=null){
        //            System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService> remoteService =
        //             (System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService>) service;
        //            remoteService.Close();
        //            remoteService = null;
        //        }

        //    }
        //    catch (Exception ex){
        //        if (service!=null){
        //            System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService> remoteService =
        //             (System.ServiceModel.ClientBase<InstantMessangerService.IInstantMessengerService>) service;
        //            remoteService.Abort();
        //            remoteService = null;
        //        }
        //    }
        //}
    }
}
