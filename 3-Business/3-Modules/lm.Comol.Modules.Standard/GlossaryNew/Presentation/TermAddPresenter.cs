using System;
using System.Collections.Generic;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class TermAddPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            var fromViewMap = View.PreloadFromViewMap;

            View.SetTitle((idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "");
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
                View.IdGlossary = idGlossary;
                View.FromViewMap = fromViewMap;
                var module = Service.GetPermissions(View.IdCommunity, litePerson);
                if (module.AddTerm || module.Administration || module.ManageGlossary)
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.StartAddTerm);
                    View.LoadViewData(View.IdCommunity, View.IdGlossary);
                    View.ItemData = new DTO_Term();
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public void SaveOrUpdate(DTO_Term term)
        {
            View.ShowErrors(SaveStateEnum.None, MessageType.none);

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }

            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.AddTerm || module.Administration || module.ManageGlossary)
            {
                term.IdCommunity = View.IdCommunity;
                var result = Service.SaveOrUpdateTerm(term);
                if (result.SaveState == SaveStateEnum.Saved)
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), result.Id, ModuleGlossaryNew.ObjectType.Term, ModuleGlossaryNew.ActionType.SaveTerm);
                    View.GoToGlossaryView();
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.GenericError);
                    View.ShowErrors(result.SaveState);
                }
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public Boolean ValidateFields(Object obj)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            List<String> resourceErrorList;
            var result = GlossaryHelper.ValidateFields(obj, out resourceErrorList);
            if (!result)
                View.ShowErrors(resourceErrorList);
            return result;
        }

        #region Initialize

        private Int32 currentIdModule;
        private ServiceGlossary service;

        public TermAddPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public TermAddPresenter(iApplicationContext oContext, IViewTermAdd view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public virtual BaseModuleManager CurrentManager { get; set; }

        protected virtual IViewTermAdd View
        {
            get { return (IViewTermAdd) base.View; }
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

        #endregion
    }
}