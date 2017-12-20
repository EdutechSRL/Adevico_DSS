using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Notification.Domain;


namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class CategoryTreePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewCategoryTree View
        {
            get { return (View.iViewCategoryTree)base.View; }
        }

        public CategoryTreePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public CategoryTreePresenter(iApplicationContext oContext, View.iViewCategoryTree view)
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

        public void InitView(bool MessageNoReorder = false)
        {
            View.ShowMessage(Domain.Enums.CategoryTreeMessageType.none);

            if (!CheckSessionAccess())
                return;

            if (!(Module.ManageCategory || Module.Administration))
            {
                View.SendUserActions(service.ModuleID,
                ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                View.ShowMessage(Domain.Enums.CategoryTreeMessageType.NoPermission);
                //View.ShowNoPermission();
                return;
            }

            View.BindTree(service.CategoriesGetCommunityTree(true, CurrentCommunityId));

            if(MessageNoReorder)
            {
                View.ShowMessage(Domain.Enums.CategoryTreeMessageType.NoReorder);
            }

        }

        public void Save(IList<Domain.liteCategoryReorderItem> Items, Boolean Refresh)
        {
            if (!CheckSessionAccess())
                return;

            if (!(Module.ManageCategory || Module.Administration))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                //View.ShowNoPermission();
                View.ShowMessage(Domain.Enums.CategoryTreeMessageType.NoPermission);
                return;
            }

            Domain.Enums.CategoryReorderResponse Saved = service.CategoryReorder(Items);

            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.CategoryReorder, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            if(Refresh)
                this.InitView();

            switch(Saved)
            {
                case Domain.Enums.CategoryReorderResponse.Error:
                     View.ShowMessage(Domain.Enums.CategoryTreeMessageType.UnSaved);
                     break;

                case Domain.Enums.CategoryReorderResponse.NoDefaultReorder:
                     View.ShowMessage(Domain.Enums.CategoryTreeMessageType.DefaultReorderWarning);
                     break;

                case Domain.Enums.CategoryReorderResponse.Success:
                     View.ShowMessage(Domain.Enums.CategoryTreeMessageType.Saved);
                     break;
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

        /// <summary>
        /// ID comunità corrente (tiene conto dell'URL o della sessione utente)
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
            }
        }


        #region Notification

        public void SendNotificationALL()
        {
            SendNotification(ModuleTicket.NotificationActionCategoryUserReceiver.All);
        }

        public void SendNotificationManagers()
        {
            SendNotification(ModuleTicket.NotificationActionCategoryUserReceiver.Managers);
        }

        public void SendNotificationResolvers()
        {
            SendNotification(ModuleTicket.NotificationActionCategoryUserReceiver.Resolvers);
        }

        private void SendNotification(ModuleTicket.NotificationActionCategoryUserReceiver receiver)
        {
            Domain.SettingsPortal settingsPortal = service.PortalSettingsGet();
            if (!(settingsPortal.IsNotificationUserActive && settingsPortal.IsNotificationManActive))
            {
                View.ShowMessage(Domain.Enums.CategoryTreeMessageType.MessageUnsend);
                return;
            }

            NotificationAction action = new NotificationAction();
            action.IdModuleUsers = new List<long>();
            action.IdModuleUsers.Add((long)receiver);

            action.ModuleCode = ModuleTicket.UniqueCode;
            action.IdCommunity = CurrentCommunityId;
            action.IdObjectType = (long)ModuleTicket.ObjectType.Category;
            action.IdObject = -1;

            action.IdModuleAction = (long)ModuleTicket.NotificationActionType.CategoriesNotification;

            View.SendNotification(action, UserContext.CurrentUserID);

            this.InitView();

            View.ShowMessage(Domain.Enums.CategoryTreeMessageType.MessageSend);

        }

        #endregion
    }
}
