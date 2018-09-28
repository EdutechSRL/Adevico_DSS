using System;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC
{
    public class UC_GlossaryShareStatePresenter : DomainPresenter
    {
        public void InitView(Boolean isManage)
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;

            View.SetTitle((idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "");
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                var module = Service.GetPermissions(View.IdCommunity, litePerson);
                View.IdCommunity = idCommunity;
                View.IdGlossary = idGlossary;
                View.ForManageEnabled = module.Administration || module.ManageGlossary;
                View.ForManage = isManage;


                if (module.AddGlossary || module.Administration || module.ManageGlossary)
                {
                    var dto_glossary = Service.GetDTO_GlossaryShare(View.IdGlossary, View.IdCommunity);
                    View.LoadViewData(View.IdCommunity, dto_glossary, View.ForManageEnabled, View.ForManage);
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public void ChangeShareStatus(ShareStatusEnum state)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.ChangeShareStatus(View.IdGlossary, View.IdCommunity, state);
            var dto_glossary = Service.GetDTO_GlossaryShare(View.IdGlossary, View.IdCommunity);
            View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), View.IdGlossary, ModuleGlossaryNew.ActionType.ChangeGlossaryState);
            View.LoadViewData(View.IdCommunity, dto_glossary, View.ForManageEnabled, View.ForManage);
        }

        public void ChangeShareVisibility(Boolean state)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.ChangeShareVisibility(View.IdGlossary, View.IdCommunity, state);
            var dto_glossary = Service.GetDTO_GlossaryShare(View.IdGlossary, View.IdCommunity);
            View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), View.IdGlossary, ModuleGlossaryNew.ActionType.ChangeGlossaryVisibility);
            View.LoadViewData(View.IdCommunity, dto_glossary, View.ForManageEnabled, View.ForManage);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewUC_GlosssaryShareState View
        {
            get { return (IViewUC_GlosssaryShareState) base.View; }
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

        public UC_GlossaryShareStatePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public UC_GlossaryShareStatePresenter(iApplicationContext oContext, IViewUC_GlosssaryShareState view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}