using System;
using System.Collections.Generic;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class ImportPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            //View.SetTitle((idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "");
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.IdCommunity = idCommunity;
                View.IdGlossary = idGlossary;
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                View.LoadViewData(View.IdCommunity, Service.GetCommunityName(View.IdCommunity));
            }
        }

        public void AddCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32) UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32) UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32) UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long) (ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = Service.GetIdCommunityWithoutGlossaries(View.IdCommunity);

            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.ShowCommunity(idCommunities);
        }

        public Boolean ImportGlossaries(List<long> listId, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            return Service.ImportGlossaries(View.IdCommunity, listId, selectedIndex);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewImport View
        {
            get { return (IViewImport) base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public ImportPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public ImportPresenter(iApplicationContext oContext, IViewImport view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}