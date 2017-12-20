using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class ModuleProviderManagement
    {
        public const string UniqueID = "CSV";
        public virtual Boolean ViewProviders {get;set;}
        public virtual Boolean AddProvider { get; set; }
        public virtual Boolean EditProvider {get;set;}
        public virtual Boolean DeleteProvider { get; set; }
        public virtual Boolean VirtualDeleteProvider { get; set; }
        public virtual Boolean VirtualUndeleteProvider { get; set; }
        public virtual Boolean Administration {get;set;}


        public ModuleProviderManagement()
        {
         
        }

        public static ModuleProviderManagement CreatePortalmodule(int UserTypeID)
        {
            ModuleProviderManagement module = new ModuleProviderManagement();
            module.ViewProviders = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.AddProvider = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.EditProvider = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator );
            module.DeleteProvider = (UserTypeID == (int)UserTypeStandard.SysAdmin);
            module.VirtualDeleteProvider = (UserTypeID == (int)UserTypeStandard.SysAdmin);
            module.VirtualUndeleteProvider = (UserTypeID == (int)UserTypeStandard.SysAdmin);
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin);
            return module;
        }


        [Flags]
        public enum Base2Permission
        {
            ViewProfile = 1,
            EditProfile = 2,
            RenewPassword = 4,
            DeleteProfile = 8,
            GrantPermission = 32,
            AdminService = 64,
            AddProfile = 2048,
            AddProfileAuthenticationProvider = 8192
        }

        public enum ActionType{
            None = 0,
            LoadProviders = 1,
            ViewProviderInfo=2,
            EditProvider =3,
            DeleteProvider = 4,
            VirtualDeleteProvider = 5,
            VirtualUndeleteProvider = 6,
            AddProvoder= 7
         }

        public enum ObjectType{
            None = 0,
            AuthenticationProvider = 1,
            ProviderAttribute = 2,
            ProviderRole = 3,
            ProviderTranslation = 4,
            ProviderLoginFormat = 5
        }
    }
}