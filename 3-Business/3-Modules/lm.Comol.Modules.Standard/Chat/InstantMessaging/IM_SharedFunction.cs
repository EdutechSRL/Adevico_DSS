using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Chat
{
    public static class IM_SharedFunction
    {
        /// <summary>
        /// Crea la chat tra due utenti, a meno che questa non ci sia già...
        /// </summary>
        /// <param name="StartUser"></param>
        /// <param name="TargetUser"></param>
        public static Guid CreateChat(Int32 StartUserId, String StartUserDisplayName, Int32 TargetUserId, String TargetUserDisplayName)
        {
            InstantMessangerService.Ct1o1_User_DTO StartUser = new InstantMessangerService.Ct1o1_User_DTO();
            StartUser.Id = StartUserId;
            StartUser.DisplayName = StartUserDisplayName;

            InstantMessangerService.Ct1o1_User_DTO TargetUser = new InstantMessangerService.Ct1o1_User_DTO();
            TargetUser.Id = TargetUserId;
            TargetUser.DisplayName = TargetUserDisplayName;

            return CreateChat(StartUser, TargetUser);
        }

        /// <summary>
        /// Crea la chat tra due utenti, a meno che questa non ci sia già...
        /// </summary>
        /// <param name="StartUser"></param>
        /// <param name="TargetUser"></param>
        public static Guid CreateChat(InstantMessangerService.Ct1o1_User_DTO StartUser, InstantMessangerService.Ct1o1_User_DTO TargetUser)
        {

            Guid ChatKey;

            InstantMessangerService.IInstantMessengerService service = null;
            try
            {
                service = new InstantMessangerService.InstantMessengerServiceClient();

                ChatKey = service.CreateChat(StartUser, TargetUser).Id;

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

                ChatKey = Guid.Empty;
            }

            return ChatKey;
        }

        /// <summary>
        /// Recupera il numero di chat correnti
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Int32 GetCurrentChatCount(Int32 UserId)
        {
            Int32 ChatCount;

            InstantMessangerService.IInstantMessengerService service = null;
            try
            {
                service = new InstantMessangerService.InstantMessengerServiceClient();

                try
                {
                    ChatCount = service.GetChats(UserId).Count();
                }
                catch
                {
                    ChatCount = 0;
                }
                

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

                ChatCount = 0;
            }

            return ChatCount;
        }

        public static void DiscardChat(Int32 UserId, Guid ChatId)
        {
            InstantMessangerService.IInstantMessengerService service = null;
            try
            {
                service = new InstantMessangerService.InstantMessengerServiceClient();

                service.DiscardChat(UserId, ChatId);
                
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

        public static void DiscardAllChats(Int32 UserId)
        {
            InstantMessangerService.IInstantMessengerService service = null;
            try
            {
                service = new InstantMessangerService.InstantMessengerServiceClient();

                service.DiscardAllChats(UserId);

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
