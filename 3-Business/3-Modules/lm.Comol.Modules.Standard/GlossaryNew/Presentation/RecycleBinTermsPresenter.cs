using System;
using System.Linq;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class RecycleBinTermsPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            var fromViewMap = View.PreloadFromViewMap;

            if (idCommunity <= 0)
                idCommunity = UserContext.CurrentCommunityID;

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
                var module = Service.GetPermissions(idCommunity, litePerson);

                if (module.DeleteTerm || module.Administration || module.ManageGlossary)
                    LoadTerms();
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public void RecoverTerm(long idTerm)
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
                var errors = String.Empty;
                if (Service.RestoreVirtualTerm(idTerm, out errors))
                {
                    LoadTerms();
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), idTerm, ModuleGlossaryNew.ObjectType.Term, ModuleGlossaryNew.ActionType.RecoverTerm);
                }
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void DeleteTerm(long idTerm)
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
                var errors = String.Empty;
                if (Service.DeleteTerm(idTerm, out errors))
                {
                    LoadTerms();
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), idTerm, ModuleGlossaryNew.ObjectType.Term, ModuleGlossaryNew.ActionType.DeleteTerm);
                }
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        private void LoadTerms()
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var termList = Service.GetDTO_TermDelete(f => f.IdGlossary == View.IdGlossary && f.Deleted != BaseStatusDeleted.None);
            View.LoadViewData(termList.ToList());
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewRecycleBinTerms View
        {
            get { return (IViewRecycleBinTerms) base.View; }
        }

        public ServiceGlossary Service
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

        public RecycleBinTermsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public RecycleBinTermsPresenter(iApplicationContext oContext, IViewRecycleBinTerms view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}