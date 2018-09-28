using System;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class GlossaryListPresenter : DomainPresenter
    {
        public void InitView(Boolean isFirstOpen)
        {
            var idCommunity = View.PreloadIdCommunity;
            var forManagement = View.PreloadForManagement;
            if (idCommunity <= 0)
                idCommunity = UserContext.CurrentCommunityID;

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
                View.SetTitle((idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "");
                var module = Service.GetPermissions(View.IdCommunity, litePerson);

                // Test
                //Service.ExportAllCommunityGlossary();
                forManagement = forManagement && (module.Administration || module.ManageGlossary);

                if (module.ViewTerm || module.Administration || module.ManageGlossary)
                {
                    if (forManagement)
                        View.LoadViewData(View.IdCommunity, module.Administration || module.ManageGlossary, forManagement);
                    else
                    {
                        long idGlossary = -1;
                        // controllo se aprire la lista di tutti i glossari o se solo direttamente il glossario di default o l'unico glossario
                        if (isFirstOpen)
                            idGlossary = Service.GetGlossaryDefaultId(View.IdCommunity);

                        if (idGlossary > 0)
                            View.GoToGlossaryView(idGlossary);
                        else
                            View.LoadViewData(View.IdCommunity, module.Administration || module.ManageGlossary, forManagement);
                    }
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(idCommunity, Service.GetServiceIdModule());
                }
            }
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewGlossaryList View
        {
            get { return (IViewGlossaryList) base.View; }
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

        public GlossaryListPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossaryListPresenter(iApplicationContext oContext, IViewGlossaryList view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}