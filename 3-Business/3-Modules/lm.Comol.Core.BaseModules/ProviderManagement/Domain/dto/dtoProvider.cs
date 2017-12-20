using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoProvider : dtoBaseProvider
    {
        public virtual String Name { get; set; }
        public virtual String UniqueCode { get; set; }
        public virtual int IdOldAuthentication { get; set; }
        public virtual Boolean AllowMultipleInsert { get; set; }
        public virtual Boolean MultipleItemsForRecord { get; set; }
        public virtual String MultipleItemsSeparator { get; set; }
        public virtual long UsedBy { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public dtoProvider() {
            MultipleItemsForRecord = false;
            MultipleItemsSeparator = "";
        }

        public virtual List<LogoutMode> GetAvailableLogoutMode(){
            List<LogoutMode> items = new List<LogoutMode>();
            switch (this.Type)
            {
                case AuthenticationProviderType.Internal:
                    items.Add(Authentication.LogoutMode.internalLogonPage);
                    items.Add(Authentication.LogoutMode.portalPage);
                    break;
               case AuthenticationProviderType.Url:
                case AuthenticationProviderType.UrlMacProvider:
                    items.Add(Authentication.LogoutMode.externalPage);
                    items.Add(Authentication.LogoutMode.logoutMessage);
                    items.Add(Authentication.LogoutMode.logoutMessageAndClose);
                    items.Add(Authentication.LogoutMode.logoutMessageAndUrl);
                    items.Add(Authentication.LogoutMode.portalPage);
                    break;
            }
            return items;
        }
    }
}