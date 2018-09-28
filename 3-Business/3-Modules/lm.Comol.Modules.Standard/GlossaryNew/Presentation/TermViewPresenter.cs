using System;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class TermViewPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            var idTerm = View.PreloadIdTerm;
            var fromViewMap = View.PreloadFromViewMap;

            View.IdCommunity = idCommunity;
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
                View.IdTerm = idTerm;
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.ViewTerm || module.Administration || module.ManageGlossary)
                {
                    var term = GetGlossaryItem(View.IdTerm);
                    var glossary = Service.GetDTO_Glossary(View.IdGlossary, View.IdCommunity);
                    Service.UpdateGlossaryPermission(module, glossary, litePerson, View.IdCommunity);
                    View.LoadViewData(View.IdCommunity, glossary, term, fromViewMap);
                    Service.StatAddTermView(View.IdCommunity, View.IdGlossary, View.IdTerm, UserContext.CurrentUserID);
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public Term GetGlossaryItem(Int64 idTerm)
        {
            return CurrentManager.Get<Term>(idTerm);
        }

        public void VirtualDelete(long idTerm)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }

            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.DeleteTerm || module.Administration || module.ManageGlossary)
            {
                String error;
                if (Service.DeleteVirtualTerm(idTerm, out error))
                {
                }
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void ChangePublishState(long idTerm, bool boolean)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.EditTerm || module.Administration || module.ManageGlossary)
            {
                Service.ChangePublishStateTerm(idTerm, boolean);
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        #region Initialize

        private Int32 currentIdModule;
        private ServiceGlossary service;

        public TermViewPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public TermViewPresenter(iApplicationContext oContext, IViewTermView view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public virtual BaseModuleManager CurrentManager { get; set; }

        protected virtual IViewTermView View
        {
            get { return (IViewTermView) base.View; }
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