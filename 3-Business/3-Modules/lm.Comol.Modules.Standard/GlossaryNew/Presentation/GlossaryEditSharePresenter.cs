using System;
using System.Collections.Generic;
using lm.Comol.Core.BaseModules.CommunityManagement;
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
    public class GlossaryEditSharePresenter : DomainPresenter
    {
        public void InitView(Boolean fromListGlossary)
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            View.FromViewGlossary = fromListGlossary;
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
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditGlossary || module.Administration || module.ManageGlossary)
                {
                    var glossary = Service.GetDTO_Glossary(idGlossary, View.IdCommunity);
                    Service.UpdateGlossaryPermission(module, glossary, litePerson, View.IdCommunity);
                    // Verifico permessi specifici del glossario
                    if (glossary.Permission.EditGlossary)
                    {
                        View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.StartEditGlossary);
                        var steps = Service.GetAvailableSteps(GlossaryStep.Settings, true);
                        View.LoadViewData(View.IdCommunity, glossary, Service.GetAvailableLanguages(), steps, fromListGlossary);
                        return;
                    }
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
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
            if (module.EditGlossary || module.Administration || module.ManageGlossary)
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
            var result = GlossaryHelper.ValidateFields(obj, out resourceErrorList);
            if (!result)
                View.ShowErrors(resourceErrorList);
            return result;
        }

        public void AddCommunityClick(Int32 userId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            /*
             * Esempio da EditCallAvailabilityPresenter.cs
             * 
            Dictionary<Int32, long> rPermissions = new Dictionary<Int32, long>();
            Boolean forAdmin = (UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative);
            Core.BaseModules.CommunityManagement.CommunityAvailability availability = (forAdmin) ? Core.BaseModules.CommunityManagement.CommunityAvailability.All : Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed;
            long permissions = -1;
            switch (View.CallType)
            {
                case CallForPaperType.CallForBids:
                    permissions = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.AddCall;
                    break;
                case CallForPaperType.RequestForMembership:
                    permissions = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests | (long)ModuleRequestForMembership.Base2Permission.AddRequest;
                    break;
            }
            rPermissions.Add(View.IdCallModule, permissions);
            rPermissions.Add(CommunityService.ServiceModuleID(), (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.Manage | (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.AdminService);
            List<Int32> idCommunities = CallService.GetIdCommunityAssignments(idCall);
            */


            var forAdmin = UserContext.UserTypeID == (Int32) UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32) UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32) UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long) (ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            /*
                switch (View.CallType)
                {
                    case CallForPaperType.CallForBids:
                        permissions = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.AddCall;
                        break;
                    case CallForPaperType.RequestForMembership:
                        permissions = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests | (long)ModuleRequestForMembership.Base2Permission.AddRequest;
                        break;
                }
        

                rPermissions.Add(View.IdCallModule, permissions);
                rPermissions.Add(CommunityService.ServiceModuleID(), (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.Manage | (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.AdminService);
            */

            // Rimuovo la comunità corrente e le comunità su cui è già condiviso
            var unloadIdCommunities = GlossaryDisplayOrders(View.IdGlossary);
            if (!unloadIdCommunities.Contains(View.IdCommunity))
                unloadIdCommunities.Add(View.IdCommunity);

            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public List<Int32> GlossaryDisplayOrders(Int64 idGlossary)
        {
            var result = Service.GetSharedIdList(idGlossary);
            return result;
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.AddGlossaryShareList(idCommunities, View.IdGlossary);
            InitView(View.FromViewGlossary);
        }

        public Int32 HasPendingShare(long idGlossary, int idCommunity)
        {
            return Service.HasPendingShare(idGlossary, idCommunity);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewGlossaryEditShare View
        {
            get { return (IViewGlossaryEditShare) base.View; }
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

        public GlossaryEditSharePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossaryEditSharePresenter(iApplicationContext oContext, IViewGlossaryEditShare view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}