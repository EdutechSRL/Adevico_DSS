using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class ItemsSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
        private ProfileManagementService _Service;
        //private int ModuleID
        //{
        //    get
        //    {
        //        if (_ModuleID <= 0)
        //        {
        //            _ModuleID = this.Service.ServiceModuleID();
        //        }
        //        return _ModuleID;
        //    }
        //}
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewItemsSelector View
        {
            get { return (IViewItemsSelector)base.View; }
        }
        private ProfileManagementService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ProfileManagementService(AppContext);
                return _Service;
            }
        }
        public ItemsSelectorPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public ItemsSelectorPresenter(iApplicationContext oContext, IViewItemsSelector view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(ProfileExternalResource source, dtoImportSettings settings)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.AutoGenerateLogin = settings.AutoGenerateLogin;
                lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider provider = Service.GetAuthenticationProvider(UserContext.Language.Id, settings.IdProvider);
                Service.AnalyzeItems(source,settings);
                List<ProfileExternalResource> items = new List<ProfileExternalResource>();
                items.Add(source);
                View.LoadItems(items, GetInvalidItems(source), settings.IdProfileType, (provider == null) ? "" : provider.Translation.Name);
            }
        }

        private List<InvalidImport> GetInvalidItems(ProfileExternalResource source)
        {
            List<InvalidImport> invalidItems = new List<InvalidImport>();
            if (source.Rows.Where(r => r.isValid() == false).Any())
                invalidItems.Add(InvalidImport.InvalidData);
            if (source.Rows.Where(r => r.HasDuplicatedValues).Any())
                invalidItems.Add(InvalidImport.SourceDuplicatedData);
            if (source.Rows.Where(r => r.HasDBDuplicatedValues).Any())
                invalidItems.Add(InvalidImport.AlreadyExist);
            return invalidItems;
        }
    }
}
