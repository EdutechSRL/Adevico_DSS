using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;

using lm.Comol.Core.Notification.Domain;


namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class CategoriesListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewCategoriesList View
        {
            get { return (View.iViewCategoriesList)base.View; }
        }

        public CategoriesListPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public CategoriesListPresenter(iApplicationContext oContext, View.iViewCategoriesList view)
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

        public void InitView()
        {
            if (!CheckSessionAccess())
                return;
            
            if (!(Module.EditCategory || Module.ManageCategory || Module.Administration))
            {
                View.SendUserActions(service.ModuleID,
                ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                View.ShowNoPermission();
                return;
            }

            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));

            View.SendUserActions(service.ModuleID,
                ModuleTicket.ActionType.CategoryList, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            SetCommunityName();
            View.BindList(service.CategoriesGetCommunity(true, true, CurrentCommunityId), Module.ManageCategory, service.CategoryDefaultGetID());
        }

        //public void DeleteCategory(Int64 CategoryID)
        //{
        //    service.CategoryDelete(CategoryID);
        //    View.BindList(service.CategoriesGetCommunity(true, true, CurrentCommunityId), Module.ManageCategory);
        //}

        public void RecoverCategory(Int64 CategoryID)
        {
            if (!CheckSessionAccess())
                return;

            if (!(Module.EditCategory || Module.ManageCategory || Module.Administration))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                View.ShowNoPermission();
                return;
            }

            service.CategoryRecover(CategoryID);
            
            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));
            Objects.Add(ModuleTicket.KVPgetCategory(CategoryID));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.CategoryUndelete, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            View.BindList(service.CategoriesGetCommunity(true, true, CurrentCommunityId), Module.ManageCategory, service.CategoryDefaultGetID());
        }

        private void SetCommunityName()
        {
            String ComName = service.CommunityNameGet(CurrentCommunityId);
            if (ComName != TicketService.ComErrName)
                View.CommunityName = ComName;
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
                View.ShowSendInfo(false);
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

            View.ShowSendInfo(true);
            
        }

#endregion

    }
}
