using System;
using System.Collections.Generic;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC
{
    public class UC_GlossarySharePresenter : DomainPresenter
    {
        public void InitView()
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
                View.IdCommunity = idCommunity;
                View.IdGlossary = idGlossary;
                var module = Service.GetPermissions(View.IdCommunity, litePerson);

                if (module.AddGlossary || module.Administration || module.ManageGlossary)
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.StartEditGlossaryShare);
                    var glossary = View.IdGlossary > 0 ? Service.GetDTO_Glossary(View.IdGlossary, View.IdCommunity) : new DTO_Glossary {IdLanguage = -1, IdCommunity = View.IdCommunity, TermsArePaged = false};
                    var communityShareList = Service.GetDTO_GlossaryShareList(View.IdGlossary);


                    View.LoadViewData(View.IdCommunity, glossary, communityShareList);
                }
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public void SaveOrUpdate(DTO_Glossary glossary, List<DTO_Share> dtoShares)
        {
            View.ShowErrors(SaveStateEnum.None, MessageType.none);

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var result = Service.SaveOrUpdateGlossaryShare(glossary, dtoShares);
            if (result.SaveState == SaveStateEnum.Saved)
            {
                var glossary2 = View.IdGlossary > 0 ? Service.GetDTO_Glossary(View.IdGlossary, View.IdCommunity) : new DTO_Glossary {IdLanguage = -1, IdCommunity = View.IdCommunity, TermsArePaged = false};
                var communityShareList = Service.GetDTO_GlossaryShareList(View.IdGlossary);

                View.LoadViewData(View.IdCommunity, glossary2, communityShareList);

                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), result.Id, ModuleGlossaryNew.ActionType.SaveGlossaryShare);
                View.ShowErrors(result.SaveState, MessageType.success);
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.GenericError);
                View.ShowErrors(result.SaveState);
            }
        }

        public Boolean ValidateFields(Object obj)
        {
            List<String> resourceErrorList;
            var result = GlossaryHelper.ValidateFields(obj, out resourceErrorList);
            if (!result)
                View.ShowErrors(resourceErrorList);
            return result;
        }

        public void VirtualDeleteShare(long idShare)
        {
            String errors;
            Service.DeleteVirtualShare(idShare, out errors);
            if (String.IsNullOrWhiteSpace(errors))
                InitView();
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewUC_GlossaryShare View
        {
            get { return (IViewUC_GlossaryShare) base.View; }
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

        public UC_GlossarySharePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public UC_GlossarySharePresenter(iApplicationContext oContext, IViewUC_GlossaryShare view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}