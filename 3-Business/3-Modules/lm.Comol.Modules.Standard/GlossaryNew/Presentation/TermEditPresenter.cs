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
    public class TermEditPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            var idTerm = View.PreloadIdTerm;
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
                View.IdTerm = idTerm;
                View.FromViewMap = fromViewMap;

                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditTerm || module.Administration || module.ManageGlossary)
                {
                    var glossary = Service.GetDTO_Glossary(View.IdGlossary, View.IdCommunity);
                    Service.UpdateGlossaryPermission(module, glossary, litePerson, idCommunity);
                    // Verifico permessi specifici del termine
                    if (glossary.Permission.EditTerm)
                    {
                        View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.StartEditTerm);
                        View.LoadViewData(View.IdCommunity, View.IdGlossary, Service.GetTermDTO(idTerm), View.PreloadFromType, View.PreloadPageIndex, View.LoadfromCookies, View.IdCookies);
                        //View.InitializeUploaderControl(RepositoryAttachmentUploadActions.uploadtomoduleitem, idCommunity);
                        return;
                    }
                }

                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void SaveOrUpdate(DTO_Term term)
        {
            View.ShowInfo(SaveStateEnum.None, MessageType.none);

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
                term.IdCommunity = View.IdCommunity;
                var result = Service.SaveOrUpdateTerm(term);
                if (result.SaveState == SaveStateEnum.Saved)
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), result.Id, ModuleGlossaryNew.ObjectType.Term, ModuleGlossaryNew.ActionType.SaveTerm);
                    //View.GoToGlossaryView();
                    View.ShowInfo(SaveStateEnum.Saved, MessageType.success);
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.GenericError);
                    View.ShowInfo(SaveStateEnum.NotSaved, MessageType.error);
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

        public TermEditPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public TermEditPresenter(iApplicationContext oContext, IViewTermEdit view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public virtual BaseModuleManager CurrentManager { get; set; }

        protected virtual IViewTermEdit View
        {
            get { return (IViewTermEdit) base.View; }
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