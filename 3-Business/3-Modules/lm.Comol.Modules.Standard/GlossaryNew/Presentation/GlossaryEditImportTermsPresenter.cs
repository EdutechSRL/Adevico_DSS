using System;
using System.Collections.Generic;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class GlossaryEditImportTermsPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            //View.SetTitle((idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "");
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
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditGlossary || module.Administration || module.ManageGlossary)
                {
                    var glossary = Service.GetDTO_Glossary(idGlossary, View.IdCommunity);
                    Service.UpdateGlossaryPermission(module, glossary, litePerson, View.IdCommunity);
                    // Verifico permessi specifici del glossario
                    if (glossary.Permission.EditGlossary)
                    {
                        var steps = Service.GetAvailableSteps(GlossaryStep.Settings, true);
                        View.LoadViewData(View.IdCommunity, glossary, steps);
                        return;
                    }
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void AddCommunityClick(int integer)
        {
            View.ShowInfo(SaveStateEnum.None, MessageType.none);

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32) UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32) UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32) UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long) (ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = Service.GetIdCommunityWithoutGlossaries();

            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            View.ShowInfo(SaveStateEnum.None, MessageType.none);

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.ShowCommunity(idCommunities);
        }

        public void ImportTerms(List<long> listId, int selectedIndex)
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
            if (module.EditGlossary || module.Administration || module.ManageGlossary)
            {
                if (Service.ImportTerms(View.IdCommunity, View.IdGlossary, listId, selectedIndex))
                    View.ShowInfo(SaveStateEnum.Saved, MessageType.success);
                else
                    View.ShowInfo(SaveStateEnum.NotSaved, MessageType.error);
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public Int32 HasPendingShare(long idGlossary, int idCommunity)
        {
            return Service.HasPendingShare(idGlossary, idCommunity);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewGlossaryImportTerms View
        {
            get { return (IViewGlossaryImportTerms) base.View; }
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

        public GlossaryEditImportTermsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossaryEditImportTermsPresenter(iApplicationContext oContext, IViewGlossaryImportTerms view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}