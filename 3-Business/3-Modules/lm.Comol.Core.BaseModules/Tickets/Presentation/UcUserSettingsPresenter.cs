using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain.DTO;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.DomainModel;
using Telerik.Web.UI;
using TK = lm.Comol.Core.BaseModules.Tickets;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class UcUserSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewUcUserSettings View
        {
            get { return (View.iViewUcUserSettings)base.View; }
        }

        public UcUserSettingsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public UcUserSettingsPresenter(iApplicationContext oContext, View.iViewUcUserSettings view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        public void Initilize(bool hideError = true)
        {
            InitializePerson(UserContext.CurrentUserID);
        }


        public void InitializePerson(int PersonId, bool hideError = true)
        {
            InitializeUser(service.UserGetIdfromPerson(PersonId), hideError);
        }

        public void InitializeUser(Int64 UserId, bool hideError = true)
        {
            if (!CheckComolSession())
            {
                View.DisplaySessionTimeout(0);
                return;
            }



            bool Found = true;

            if (UserId < 0)
                Found = false;

            Domain.TicketUser user = service.UserGet(UserId);

            if (user == null)
                Found = false;

            if(Found)
            {
                if(hideError)
                    View.ShowError(ViewSettingsUserError.none);

                Domain.DTO.DTO_UserSettings settings = new DTO_UserSettings();
                settings.isManager = service.UserIsManagerOrResolver();
                settings.isBehalfer = service.SettingsPermissionIsBehalfer(UserId);

                
                //settings.isNotificationOn = true;
                settings.isUserNotificationOn = user.IsNotificationActiveUser;
                settings.isManagerNotificationOn = user.IsNotificationActiveManager;

                settings.Settings = service.NotificationGetUser(UserId, 0).Settings;

                Tickets.Domain.SettingsPortal settPortal = service.SettingsGlobalGet(false, 0);

                settings.isManagerSysNotificationOn = settPortal.IsNotificationUserActive;
                settings.isUserSysNotificationOn = settPortal.IsNotificationManActive;
                //settings.Settings.

                //, true


                View.Settings = settings;

            }
            else
            {
                View.ShowError(Domain.Enums.ViewSettingsUserError.usernotfound);
            }
        }


        public void Save()
        {
            SavePerson(UserContext.CurrentUserID);
        }

        public void SavePerson(int PersonId)
        {
            SaveUser(service.UserGetIdfromPerson(PersonId));
        }

        public void SaveUser(Int64 UserId)
        {
            if (!CheckComolSession())
            {
                View.DisplaySessionTimeout(0);
                return;
            }

            Domain.DTO.DTO_UserSettings settings = View.Settings;

            if (
                service.SettingsSetGlobalUser(UserId, settings.Settings, settings.isUserNotificationOn,
                    settings.isManagerNotificationOn)
                )
            {
                InitializeUser(UserId, false);
                View.ShowError(ViewSettingsUserError.success);
            }
            else
            {
                View.ShowError(ViewSettingsUserError.internalError);
            }

        }

        public bool CheckComolSession()
        {
            return !(UserContext.isAnonymous);
        }
    }
}
