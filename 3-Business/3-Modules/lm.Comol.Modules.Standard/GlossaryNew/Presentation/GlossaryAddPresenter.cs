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
    public class GlossaryAddPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
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
                var module = Service.GetPermissions(View.IdCommunity, litePerson);

                if (module.AddGlossary || module.Administration || module.ManageGlossary)
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.StartAddGlossary);
                    View.ItemData = new DTO_Glossary {IdLanguage = UserContext.Language.Id};
                    var steps = Service.GetAvailableSteps(GlossaryStep.Settings, true);
                    View.LoadViewData(View.IdCommunity, Service.GetAvailableLanguages(), steps, true);
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public void SaveOrUpdate(DTO_Glossary glossary)
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
            if (module.AddGlossary || module.Administration || module.ManageGlossary)
            {
                var result = Service.SaveOrUpdateGlossary(glossary);
                if (result.SaveState == SaveStateEnum.Saved)
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), result.Id, ModuleGlossaryNew.ObjectType.Glossary, ModuleGlossaryNew.ActionType.SaveGlossary);
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
            List<String> resourceErrorList;
            var isValid = GlossaryHelper.ValidateFields(obj, out resourceErrorList);
            if (!isValid)
                View.ShowErrors(resourceErrorList);
            return isValid;
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewGlossaryAdd View
        {
            get { return (IViewGlossaryAdd) base.View; }
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

        public GlossaryAddPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossaryAddPresenter(iApplicationContext oContext, IViewGlossaryAdd view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}