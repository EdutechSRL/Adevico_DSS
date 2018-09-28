using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class GlossaryListOrderPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.IdCommunity = idCommunity;
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                var module = Service.GetPermissions(View.IdCommunity, litePerson);
                if (module.Administration || module.ManageGlossary)
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.StartManageSort);
                    LoadGlossary();
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(idCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public void ReorderGlossary(List<long> idGlossaryList, long defaultId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.Administration || module.ManageGlossary)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.SaveManageSort);
                var result = Service.ReorderGlossary(View.IdCommunity, idGlossaryList, defaultId);
                if (result)
                {
                    LoadGlossary();
                    View.ShowMessage(SaveStateEnum.Saved, MessageType.success);
                }
                else
                {
                    View.ShowMessage(SaveStateEnum.NotSaved, MessageType.error);
                }
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        private void LoadGlossary()
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var glossaryList = Service.GetDTO_GlossaryListOrdered(View.IdCommunity, false).OrderBy(f => f.DisplayOrder).ToList();
            View.LoadViewData(glossaryList);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewGlossaryListOrder View
        {
            get { return (IViewGlossaryListOrder) base.View; }
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

        public GlossaryListOrderPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossaryListOrderPresenter(iApplicationContext oContext, IViewGlossaryListOrder view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}