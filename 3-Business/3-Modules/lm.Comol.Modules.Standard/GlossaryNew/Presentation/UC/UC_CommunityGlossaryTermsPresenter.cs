using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC
{
    public class UC_CommunityGlossaryTermsPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            if (idCommunity <= 0)
                idCommunity = UserContext.CurrentCommunityID;

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                var module = Service.GetPermissions(idCommunity, litePerson);
                View.IdCommunity = idCommunity;
                View.IdGlossary = View.PreloadIdGlossary;
                if (module.ViewTerm || module.Administration || module.ManageGlossary)
                {
                    //UpdateView(litePerson, module);
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        private void UpdateView(litePerson person, ModuleGlossaryNew module)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }

            if (View.IdCommunity > 0)
            {
                List<DTO_Glossary> glossaryList = null;
                //if (View.ForManagement)
                //glossaryList = Service.GetActiveDTO_GlossaryListOrdered(f => f.IdCommunity == View.IdCommunity && f.Deleted == BaseStatusDeleted.None && f.Glossary.Deleted == BaseStatusDeleted.None);
                glossaryList = Service.GetDTO_GlossaryListOrdered(View.IdCommunity, false);

                // Calcolo i permessi di tutti i glossari
                foreach (var dtoGlossary in glossaryList)
                    Service.UpdateGlossaryPermission(module, dtoGlossary, person, View.IdCommunity);

                // Carico tutti i glossari pubblici, pubblicati e non della mia comunità
                var publicGlossaryList = Service.GetDTO_GlossaryListFromliteGlossary(f => f.IdCommunity != View.IdCommunity && f.Deleted == BaseStatusDeleted.None && f.IsPublic && f.IsPublished);

                // li metto in sola visualizzazione
                // non verifico al momento i permessi che ho nella comunità di origine del glossario
                foreach (var dtoGlossary in publicGlossaryList)
                    dtoGlossary.Permission.ViewTerm = module.ViewTerm;

                // Dovrei aver concatenato tutti i permessi e quindi
                // invio dati alla vista solo con i glossari visibili
                //View.LoadViewData(glossaryList.Where(f => f.Permission.ViewTerm).ToList(), publicGlossaryList);

                var list = Service.GetCommunityGlossaryTerms(View.IdGlossary, glossaryList.Where(f => f.Permission.ViewTerm).Select(f => f.Id).ToList(), publicGlossaryList.Select(f => f.Id).ToList());

                View.LoadViewData(list, false);
            }
        }

        public void UpdateView(List<Int32> listIds)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var list = Service.GetCommunityGlossaryTerms(View.IdGlossary, listIds);
            View.LoadViewData(list, false);
        }

        public void UpdateView(string fileName, Boolean showCommunityMapper)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var list = Service.GetCommunityGlossaryTerms(fileName);
            View.LoadViewData(list, showCommunityMapper);
        }

        public void ImportTerms(List<long> listId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.ImportTerms(View.IdCommunity, View.IdGlossary, listId, -1);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewUC_CommunityGlossaryTerms View
        {
            get { return (IViewUC_CommunityGlossaryTerms) base.View; }
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

        public UC_CommunityGlossaryTermsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public UC_CommunityGlossaryTermsPresenter(iApplicationContext oContext, IViewUC_CommunityGlossaryTerms view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}